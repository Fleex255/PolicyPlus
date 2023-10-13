using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using PolicyPlus.csharp.Helpers;
using PolicyPlus.csharp.Models.Sources;

namespace PolicyPlus.csharp.Models
{
    public class PolicyLoader
    {
        private readonly bool _user; // Whether this is for a user policy source
        private IPolicySource _sourceObject;
        private readonly string _mainSourcePath; // Path to the POL file or NTUSER.DAT
        private RegistryKey? _mainSourceRegKey; // The hive key, or the mounted hive file
        private readonly string _gptIniPath; // Path to the gpt.ini file, used to increment the version
        private bool _writable;

        public PolicyLoader(PolicyLoaderSource source, string argument, bool isUser)
        {
            Source = source;
            _user = isUser;
            LoaderData = argument;
            // Parse the argument and open the physical resource
            switch (source)
            {
                case PolicyLoaderSource.LocalGpo:
                    {
                        _mainSourcePath = Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\GroupPolicy\" + (isUser ? "User" : "Machine") + @"\Registry.pol");
                        _gptIniPath = Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\GroupPolicy\gpt.ini");
                        break;
                    }
                case PolicyLoaderSource.LocalRegistry:
                    {
                        var pathParts = argument.Split(@"\", 2);
                        var baseName = pathParts[0].ToLowerInvariant();
                        var baseKey = baseName switch
                        {
                            "hkcu" or "hkey_current_user" => RegistryKey.OpenBaseKey(RegistryHive.CurrentUser,
                                RegistryView.Default),
                            "hku" or "hkey_users" => RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Default),
                            "hklm" or "hkey_local_machine" => RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                RegistryView.Default),
                            _ => throw new Exception("The root key is not valid.")
                        };
                        _mainSourceRegKey = pathParts.Length == 2 ? baseKey.CreateSubKey(pathParts[1]) : baseKey;

                        break;
                    }
                case PolicyLoaderSource.PolFile:
                    {
                        _mainSourcePath = argument;
                        break;
                    }
                case PolicyLoaderSource.SidGpo:
                    {
                        _mainSourcePath = Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\GroupPolicyUsers\" + argument + @"\User\Registry.pol");
                        _gptIniPath = Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\GroupPolicyUsers\" + argument + @"\gpt.ini");
                        break;
                    }
                case PolicyLoaderSource.NtUserDat:
                    {
                        _mainSourcePath = argument;
                        break;
                    }
                case PolicyLoaderSource.Null:
                    {
                        _mainSourcePath = string.Empty;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }

        public IPolicySource OpenSource()
        {
            // Create an IPolicySource so PolicyProcessing can work
            switch (Source)
            {
                case PolicyLoaderSource.LocalRegistry:
                    {
                        var regPol = RegistryPolicyProxy.EncapsulateKey(_mainSourceRegKey!);
                        try
                        {
                            regPol.SetValue(@"Software\Policies", "_PolicyPlusSecCheck", "Testing to see whether Policy Plus can write to policy keys", RegistryValueKind.String);
                            regPol.DeleteValue(@"Software\Policies", "_PolicyPlusSecCheck");
                            _writable = true;
                        }
                        catch (Exception)
                        {
                            _writable = false;
                        }
                        _sourceObject = regPol;
                        break;
                    }
                case PolicyLoaderSource.NtUserDat:
                    {
                        // Turn on the backup and restore privileges to allow the use of RegLoadKey
                        Privilege.EnablePrivilege("SeBackupPrivilege");
                        Privilege.EnablePrivilege("SeRestorePrivilege");
                        // Load the hive
                        using (var machHive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
                        {
                            var subkeyName = "PolicyPlusMount:" + Guid.NewGuid();
                            _ = PInvoke.RegLoadKeyW(new IntPtr(int.MinValue + 0x00000002), subkeyName, _mainSourcePath); // HKEY_LOCAL_MACHINE
                            _mainSourceRegKey = machHive.OpenSubKey(subkeyName, true);
                            if (_mainSourceRegKey is null)
                            {
                                _writable = false;
                                _sourceObject = new PolFile();
                                return _sourceObject;
                            }

                            _writable = true;
                        }
                        _sourceObject = RegistryPolicyProxy.EncapsulateKey(_mainSourceRegKey);
                        break;
                    }
                case PolicyLoaderSource.Null:
                    {
                        _sourceObject = new PolFile();
                        break;
                    }

                default:
                    {
                        if (File.Exists(_mainSourcePath))
                        {
                            // Open a POL file
                            try
                            {
                                using var fPol = new FileStream(_mainSourcePath, FileMode.Open, FileAccess.ReadWrite);
                                _writable = true;
                            }
                            catch (Exception)
                            {
                                _writable = false;
                            }
                            _sourceObject = PolFile.Load(_mainSourcePath);
                        }
                        else
                        {
                            // Create a new POL file
                            try
                            {
                                _ = Directory.CreateDirectory(Path.GetDirectoryName(_mainSourcePath)!);
                                var pol = new PolFile();
                                pol.Save(_mainSourcePath);
                                _sourceObject = pol;
                                _writable = true;
                            }
                            catch (Exception)
                            {
                                _sourceObject = new PolFile();
                                _writable = false;
                            }
                        }

                        break;
                    }
            }
            return _sourceObject;
        }

        public bool Close() // Whether cleanup was successful
        {
            if (Source != PolicyLoaderSource.NtUserDat || _sourceObject is not RegistryPolicyProxy)
            {
                return true;
            }

            var subkeyName = _mainSourceRegKey.Name.Split(@"\", 2)[1]; // Remove the host hive name
            _mainSourceRegKey.Dispose();
            return PInvoke.RegUnLoadKeyW(new IntPtr(int.MinValue + 0x00000002), subkeyName) == 0;
        }

        public string Save() // Returns human-readable info on what happened
        {
            switch (Source)
            {
                case PolicyLoaderSource.LocalGpo:
                    {
                        var oldPol = File.Exists(_mainSourcePath) ? PolFile.Load(_mainSourcePath) : new PolFile();

                        var pol = (PolFile)_sourceObject;
                        pol.Save(_mainSourcePath);
                        UpdateGptIni();
                        // Figure out whether this edition can handle Group Policy application by itself
                        if (SystemInfo.HasGroupPolicyInfrastructure())
                        {
                            _ = PInvoke.RefreshPolicyEx(!_user, 0U);
                            return "saved to disk and invoked policy refresh";
                        }

                        pol.ApplyDifference(oldPol, RegistryPolicyProxy.EncapsulateKey(_user ? RegistryHive.CurrentUser : RegistryHive.LocalMachine));
                        _ = PInvoke.SendNotifyMessageW(new IntPtr(0xFFFF), 0x1A, UIntPtr.Zero, IntPtr.Zero); // Broadcast WM_SETTINGCHANGE
                        return "saved to disk and applied diff to Registry";
                    }
                case PolicyLoaderSource.LocalRegistry:
                    {
                        return "already applied";
                    }
                case PolicyLoaderSource.NtUserDat:
                    {
                        return "will apply when policy source is closed";
                    }
                case PolicyLoaderSource.Null:
                    {
                        return "discarded";
                    }
                case PolicyLoaderSource.PolFile:
                    {
                        ((PolFile)_sourceObject).Save(_mainSourcePath);
                        return "saved to disk";
                    }
                case PolicyLoaderSource.SidGpo:
                    {
                        ((PolFile)_sourceObject).Save(_mainSourcePath);
                        UpdateGptIni();
                        _ = PInvoke.RefreshPolicyEx(false, 0U);
                        return "saved to disk and invoked policy refresh";
                    }
                default:
                    return string.Empty;
            }
        }

        public string GetCmtxPath() =>
            Source switch
            {
                // Get the path to the comments file, or nothing if comments don't work
                PolicyLoaderSource.PolFile or PolicyLoaderSource.NtUserDat => Path.ChangeExtension(
                    _mainSourcePath, "cmtx"),
                PolicyLoaderSource.LocalRegistry => Environment.ExpandEnvironmentVariables(
                    @"%LOCALAPPDATA%\Policy Plus\Reg" + (_user ? "User" : "Machine") + ".cmtx"),
                _ => !string.IsNullOrEmpty(_mainSourcePath)
                    ? Path.Combine(Path.GetDirectoryName(_mainSourcePath) ?? throw new InvalidOperationException(), "comment.cmtx")
                    : string.Empty
            };

        public PolicySourceWritability GetWritability() =>
            Source switch
            {
                // Get whether the source can be updated
                PolicyLoaderSource.Null => PolicySourceWritability.Writable,
                PolicyLoaderSource.LocalRegistry => _writable
                    ? PolicySourceWritability.Writable
                    : PolicySourceWritability.NoWriting,
                _ => _writable ? PolicySourceWritability.Writable : PolicySourceWritability.NoCommit
            };

        private void UpdateGptIni()
        {
            // Increment the version number in gpt.ini
            const string machExtensionsLine = "gPCMachineExtensionNames=[{35378EAC-683F-11D2-A89A-00C04FBBCFA2}{D02B1F72-3407-48AE-BA88-E8213C6761F1}]";
            const string userExtensionsLine = "gPCUserExtensionNames=[{35378EAC-683F-11D2-A89A-00C04FBBCFA2}{D02B1F73-3407-48AE-BA88-E8213C6761F1}]";
            if (File.Exists(_gptIniPath))
            {
                // Alter the existing gpt.ini's Version line and add any necessary other lines
                var lines = File.ReadLines(_gptIniPath).ToList();
                using var fGpt = new StreamWriter(_gptIniPath, false);
                bool seenMachExts = default, seenUserExts = default, seenVersion = default;
                foreach (var line in lines)
                {
                    if (line.StartsWith("Version", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var curVersion = int.Parse(line.Split("=", 2)[1]);
                        curVersion += _user ? 0x10000 : 1;
                        fGpt.WriteLine("Version=" + curVersion);
                        seenVersion = true;
                    }
                    else
                    {
                        fGpt.WriteLine(line);
                        if (line.StartsWith("gPCMachineExtensionNames=", StringComparison.InvariantCultureIgnoreCase))
                        {
                            seenMachExts = true;
                        }

                        if (line.StartsWith("gPCUserExtensionNames=", StringComparison.InvariantCultureIgnoreCase))
                        {
                            seenUserExts = true;
                        }
                    }
                }
                if (!seenVersion)
                {
                    fGpt.WriteLine("Version=" + 0x10001);
                }

                if (!seenMachExts)
                {
                    fGpt.WriteLine(machExtensionsLine);
                }

                if (!seenUserExts)
                {
                    fGpt.WriteLine(userExtensionsLine);
                }
            }
            else
            {
                // Create a new gpt.ini
                using var fGpt = new StreamWriter(_gptIniPath);
                fGpt.WriteLine("[General]");
                fGpt.WriteLine(machExtensionsLine);
                fGpt.WriteLine(userExtensionsLine);
                fGpt.WriteLine("Version=" + 0x10001);
            }
        }

        public PolicyLoaderSource Source { get; }

        public string LoaderData { get; }

        public string GetDisplayInfo()
        {
            // Get the human-readable name of the loader for display in the status bar
            var name = Source switch
            {
                PolicyLoaderSource.LocalGpo => "Local GPO",
                PolicyLoaderSource.LocalRegistry => "Registry",
                PolicyLoaderSource.PolFile => "File",
                PolicyLoaderSource.SidGpo => "User GPO",
                PolicyLoaderSource.NtUserDat => "User hive",
                PolicyLoaderSource.Null => "Scratch space",
                _ => string.Empty
            };
            if (!string.IsNullOrEmpty(LoaderData))
            {
                return name + " (" + LoaderData + ")";
            }

            return name;
        }
    }
}