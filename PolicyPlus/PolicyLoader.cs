using System;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace PolicyPlus
{
    public class PolicyLoader
    {
        private PolicyLoaderSource SourceType;
        private string OriginalArgument;
        private bool User; // Whether this is for a user policy source
        private IPolicySource SourceObject;
        private string MainSourcePath; // Path to the POL file or NTUSER.DAT
        private RegistryKey MainSourceRegKey; // The hive key, or the mounted hive file
        private string GptIniPath; // Path to the gpt.ini file, used to increment the version
        private bool Writable;
        public PolicyLoader(PolicyLoaderSource Source, string Argument, bool IsUser)
        {
            SourceType = Source;
            User = IsUser;
            OriginalArgument = Argument;
            // Parse the argument and open the physical resource
            switch (Source)
            {
                case PolicyLoaderSource.LocalGpo:
                    {
                        MainSourcePath = Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\GroupPolicy\" + (IsUser ? "User" : "Machine") + @"\Registry.pol");
                        GptIniPath = Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\GroupPolicy\gpt.ini");
                        break;
                    }
                case PolicyLoaderSource.LocalRegistry:
                    {
                        string[] pathParts = Strings.Split(Argument, @"\", 2);
                        string baseName = pathParts[0].ToLowerInvariant();
                        RegistryKey baseKey;
                        if (baseName == "hkcu" | baseName == "hkey_current_user")
                        {
                            baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
                        }
                        else if (baseName == "hku" | baseName == "hkey_users")
                        {
                            baseKey = RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Default);
                        }
                        else if (baseName == "hklm" | baseName == "hkey_local_machine")
                        {
                            baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);
                        }
                        else
                        {
                            throw new Exception("The root key is not valid.");
                        }
                        if (pathParts.Length == 2)
                        {
                            MainSourceRegKey = baseKey.CreateSubKey(pathParts[1]);
                        }
                        else
                        {
                            MainSourceRegKey = baseKey;
                        }

                        break;
                    }
                case PolicyLoaderSource.PolFile:
                    {
                        MainSourcePath = Argument;
                        break;
                    }
                case PolicyLoaderSource.SidGpo:
                    {
                        MainSourcePath = Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\GroupPolicyUsers\" + Argument + @"\User\Registry.pol");
                        GptIniPath = Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\GroupPolicyUsers\" + Argument + @"\gpt.ini");
                        break;
                    }
                case PolicyLoaderSource.NtUserDat:
                    {
                        MainSourcePath = Argument;
                        break;
                    }
                case PolicyLoaderSource.Null:
                    {
                        MainSourcePath = "";
                        break;
                    }
            }
        }
        public IPolicySource OpenSource()
        {
            // Create an IPolicySource so PolicyProcessing can work
            switch (SourceType)
            {
                case PolicyLoaderSource.LocalRegistry:
                    {
                        var regPol = RegistryPolicyProxy.EncapsulateKey(MainSourceRegKey);
                        try
                        {
                            regPol.SetValue(@"Software\Policies", "_PolicyPlusSecCheck", "Testing to see whether Policy Plus can write to policy keys", RegistryValueKind.String);
                            regPol.DeleteValue(@"Software\Policies", "_PolicyPlusSecCheck");
                            Writable = true;
                        }
                        catch (Exception ex)
                        {
                            Writable = false;
                        }
                        SourceObject = regPol;
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
                            string subkeyName = "PolicyPlusMount:" + Guid.NewGuid().ToString();
                            PInvoke.RegLoadKeyW(new IntPtr(int.MinValue + 0x00000002), ref subkeyName, ref MainSourcePath); // HKEY_LOCAL_MACHINE
                            MainSourceRegKey = machHive.OpenSubKey(subkeyName, true);
                            if (MainSourceRegKey is null)
                            {
                                Writable = false;
                                SourceObject = new PolFile();
                                return SourceObject;
                            }
                            else
                            {
                                Writable = true;
                            }
                        }
                        SourceObject = RegistryPolicyProxy.EncapsulateKey(MainSourceRegKey);
                        break;
                    }
                case PolicyLoaderSource.Null:
                    {
                        SourceObject = new PolFile();
                        break;
                    }

                default:
                    {
                        if (System.IO.File.Exists(MainSourcePath))
                        {
                            // Open a POL file
                            try
                            {
                                using (var fPol = new System.IO.FileStream(MainSourcePath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
                                {
                                    Writable = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                Writable = false;
                            }
                            SourceObject = PolFile.Load(MainSourcePath);
                        }
                        else
                        {
                            // Create a new POL file
                            try
                            {
                                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(MainSourcePath));
                                var pol = new PolFile();
                                pol.Save(MainSourcePath);
                                SourceObject = pol;
                                Writable = true;
                            }
                            catch (Exception ex)
                            {
                                SourceObject = new PolFile();
                                Writable = false;
                            }
                        }

                        break;
                    }
            }
            return SourceObject;
        }
        public bool Close() // Whether cleanup was successful
        {
            if (SourceType == PolicyLoaderSource.NtUserDat & SourceObject is RegistryPolicyProxy)
            {
                string subkeyName = Strings.Split(MainSourceRegKey.Name, @"\", 2)[1]; // Remove the host hive name
                MainSourceRegKey.Dispose();
                return PInvoke.RegUnLoadKeyW(new IntPtr(int.MinValue + 0x00000002), ref subkeyName) == 0;
            }
            return true;
        }
        public string Save() // Returns human-readable info on what happened
        {
            switch (SourceType)
            {
                case PolicyLoaderSource.LocalGpo:
                    {
                        PolFile oldPol;
                        if (System.IO.File.Exists(MainSourcePath))
                            oldPol = PolFile.Load(MainSourcePath);
                        else
                            oldPol = new PolFile();
                        PolFile pol = (PolFile)SourceObject;
                        pol.Save(MainSourcePath);
                        UpdateGptIni();
                        // Figure out whether this edition can handle Group Policy application by itself
                        if (SystemInfo.HasGroupPolicyInfrastructure())
                        {
                            PInvoke.RefreshPolicyEx(!User, 0U);
                            return "saved to disk and invoked policy refresh";
                        }
                        else
                        {
                            pol.ApplyDifference(oldPol, RegistryPolicyProxy.EncapsulateKey(User ? RegistryHive.CurrentUser : RegistryHive.LocalMachine));
                            PInvoke.SendNotifyMessageW(new IntPtr(0xFFFF), 0x1A, UIntPtr.Zero, IntPtr.Zero); // Broadcast WM_SETTINGCHANGE
                            return "saved to disk and applied diff to Registry";
                        }
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
                        ((PolFile)SourceObject).Save(MainSourcePath);
                        return "saved to disk";
                    }
                case PolicyLoaderSource.SidGpo:
                    {
                        ((PolFile)SourceObject).Save(MainSourcePath);
                        UpdateGptIni();
                        PInvoke.RefreshPolicyEx(false, 0U);
                        return "saved to disk and invoked policy refresh";
                    }
            }
            return "";
        }
        public string GetCmtxPath()
        {
            // Get the path to the comments file, or nothing if comments don't work
            if (SourceType == PolicyLoaderSource.PolFile | SourceType == PolicyLoaderSource.NtUserDat)
            {
                return System.IO.Path.ChangeExtension(MainSourcePath, "cmtx");
            }
            else if (SourceType == PolicyLoaderSource.LocalRegistry)
            {
                return Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%\Policy Plus\Reg" + (User ? "User" : "Machine") + ".cmtx");
            }
            else if (!string.IsNullOrEmpty(MainSourcePath))
            {
                return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(MainSourcePath), "comment.cmtx");
            }
            else
            {
                return "";
            }
        }
        public PolicySourceWritability GetWritability()
        {
            // Get whether the source can be updated
            if (SourceType == PolicyLoaderSource.Null)
            {
                return PolicySourceWritability.Writable;
            }
            else if (SourceType == PolicyLoaderSource.LocalRegistry)
            {
                return Writable ? PolicySourceWritability.Writable : PolicySourceWritability.NoWriting;
            }
            else
            {
                return Writable ? PolicySourceWritability.Writable : PolicySourceWritability.NoCommit;
            }
        }
        private void UpdateGptIni()
        {
            // Increment the version number in gpt.ini
            const string MachExtensionsLine = "gPCMachineExtensionNames=[{35378EAC-683F-11D2-A89A-00C04FBBCFA2}{D02B1F72-3407-48AE-BA88-E8213C6761F1}]";
            const string UserExtensionsLine = "gPCUserExtensionNames=[{35378EAC-683F-11D2-A89A-00C04FBBCFA2}{D02B1F73-3407-48AE-BA88-E8213C6761F1}]";
            if (System.IO.File.Exists(GptIniPath))
            {
                // Alter the existing gpt.ini's Version line and add any necessary other lines
                var lines = System.IO.File.ReadLines(GptIniPath).ToList();
                using (var fGpt = new System.IO.StreamWriter(GptIniPath, false))
                {
                    bool seenMachExts = default, seenUserExts = default, seenVersion = default;
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("Version", StringComparison.InvariantCultureIgnoreCase))
                        {
                            int curVersion = Conversions.ToInteger(Strings.Split(line, "=", 2)[1]);
                            curVersion += User ? 0x10000 : 1;
                            fGpt.WriteLine("Version=" + curVersion);
                            seenVersion = true;
                        }
                        else
                        {
                            fGpt.WriteLine(line);
                            if (line.StartsWith("gPCMachineExtensionNames=", StringComparison.InvariantCultureIgnoreCase))
                                seenMachExts = true;
                            if (line.StartsWith("gPCUserExtensionNames=", StringComparison.InvariantCultureIgnoreCase))
                                seenUserExts = true;
                        }
                    }
                    if (!seenVersion)
                        fGpt.WriteLine("Version=" + 0x10001);
                    if (!seenMachExts)
                        fGpt.WriteLine(MachExtensionsLine);
                    if (!seenUserExts)
                        fGpt.WriteLine(UserExtensionsLine);
                }
            }
            else
            {
                // Create a new gpt.ini
                using (var fGpt = new System.IO.StreamWriter(GptIniPath))
                {
                    fGpt.WriteLine("[General]");
                    fGpt.WriteLine(MachExtensionsLine);
                    fGpt.WriteLine(UserExtensionsLine);
                    fGpt.WriteLine("Version=" + 0x10001);
                }
            }
        }
        public PolicyLoaderSource Source
        {
            get
            {
                return SourceType;
            }
        }
        public string LoaderData
        {
            get
            {
                return OriginalArgument;
            }
        }
        public string GetDisplayInfo()
        {
            // Get the human-readable name of the loader for display in the status bar
            string name = "";
            switch (SourceType)
            {
                case PolicyLoaderSource.LocalGpo:
                    {
                        name = "Local GPO";
                        break;
                    }
                case PolicyLoaderSource.LocalRegistry:
                    {
                        name = "Registry";
                        break;
                    }
                case PolicyLoaderSource.PolFile:
                    {
                        name = "File";
                        break;
                    }
                case PolicyLoaderSource.SidGpo:
                    {
                        name = "User GPO";
                        break;
                    }
                case PolicyLoaderSource.NtUserDat:
                    {
                        name = "User hive";
                        break;
                    }
                case PolicyLoaderSource.Null:
                    {
                        name = "Scratch space";
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(OriginalArgument))
                return name + " (" + OriginalArgument + ")";
            else
                return name;
        }
    }

    public enum PolicyLoaderSource
    {
        LocalGpo,
        LocalRegistry,
        PolFile,
        SidGpo,
        NtUserDat,
        Null
    }

    public enum PolicySourceWritability
    {
        Writable, // Full writability
        NoCommit, // Enable the OK button, but don't try to save
        NoWriting // Disable the OK button (there's no buffer)
    }
}