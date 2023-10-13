using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace PolicyPlus.csharp.Models
{
    public partial class PolFile
    {
        private class PolEntryData // Represents one record in a POL file
        {
            public RegistryValueKind Kind;
            public byte[] Data;

            public string AsString() // Get a UTF-16LE string
            {
                var sb = new System.Text.StringBuilder(64);
                for (int x = 0, loopTo = (Data.Length / 2) - 1; x <= loopTo; x++)
                {
                    var charCode = Data[x * 2] + (Data[(x * 2) + 1] << 8);
                    if (charCode == 0)
                    {
                        break;
                    }

                    _ = sb.Append((char)charCode);
                }
                return sb!.ToString();
            }

            public static PolEntryData FromString(string text, bool expand = false) // Save a UTF-16LE string
            {
                var ped = new PolEntryData { Kind = RegistryValueKind.String };
                if (expand)
                {
                    ped.Kind = RegistryValueKind.ExpandString;
                }

                var data = new byte[(text.Length * 2) + 1 + 1];
                for (int x = 0, loopTo = text.Length - 1; x <= loopTo; x++)
                {
                    var charCode = (int)char.GetNumericValue(text[x]);
                    data[x * 2] = (byte)(charCode & 0xFF);
                    data[(x * 2) + 1] = (byte)(charCode >> 8);
                }
                ped.Data = data;
                return ped;
            }

            public uint AsDword() => Data[0] + ((uint)Data[1] << 8) + ((uint)Data[2] << 16) + ((uint)Data[3] << 24);

            public static PolEntryData FromDword(uint dword)
            {
                var ped = new PolEntryData { Kind = RegistryValueKind.DWord };
                var data = new byte[4];
                data[0] = (byte)(dword & 0xFFU);
                data[1] = (byte)(dword >> 8 & 0xFFU);
                data[2] = (byte)(dword >> 16 & 0xFFU);
                data[3] = (byte)(dword >> 24);
                ped.Data = data;
                return ped;
            }

            public ulong AsQword()
            {
                var value = 0UL;
                for (var n = 0; n <= 7; n++)
                {
                    value += (ulong)Data[n] << n * 8;
                }

                return value;
            }

            public static PolEntryData FromQword(ulong qword)
            {
                var ped = new PolEntryData { Kind = RegistryValueKind.QWord };
                var data = new byte[8];
                for (var n = 0; n <= 7; n++)
                {
                    data[n] = (byte)(qword >> n * 8 & 0xFFUL);
                }

                ped.Data = data;
                return ped;
            }

            public string[] AsMultiString()
            {
                var strings = new List<string>();
                var sb = new System.Text.StringBuilder();
                for (double n = 0d, loopTo = (Data.Length / 2d) - 1d; n <= loopTo; n++)
                {
                    var charCode = (byte)(Data[(int)Math.Round(n * 2d)] + (Data[(int)Math.Round((n * 2d) + 1d)] << 8));
                    if (charCode == 0)
                    {
                        if (sb.Length == 0)
                        {
                            break;
                        }

                        strings.Add(sb.ToString());
                        _ = sb.Clear();
                    }
                    else
                    {
                        _ = sb.Append((char)charCode);
                    }
                }
                return strings.ToArray();
            }

            public static PolEntryData FromMultiString(string[] strings)
            {
                var ped = new PolEntryData { Kind = RegistryValueKind.MultiString };
                var data = new byte[(strings.Sum(s => s.Length + 1) + 1) * 2];
                var n = 0;
                foreach (var s in strings)
                {
                    foreach (var charCode in s.Select(c => (int)char.GetNumericValue(c)))
                    {
                        data[n] = (byte)(charCode & 0xFF);
                        data[n + 1] = (byte)(charCode >> 8);
                        n += 2;
                    }

                    n += 2; // Leave two null bytes after each string
                }
                ped.Data = data;
                return ped;
            }

            public byte[] AsBinary() => (byte[])Data.Clone();

            public static PolEntryData FromBinary(byte[] binary, RegistryValueKind kind = RegistryValueKind.Binary) => new()
            {
                Kind = kind,
                Data = (byte[])binary.Clone()
            };

            public object AsArbitrary() =>
                // Get the data in the best .NET type for it
                Kind switch
                {
                    RegistryValueKind.String => AsString(),
                    RegistryValueKind.DWord => AsDword(),
                    RegistryValueKind.ExpandString => AsString(),
                    RegistryValueKind.QWord => AsQword(),
                    RegistryValueKind.MultiString => AsMultiString(),
                    _ => AsBinary()
                };

            public static PolEntryData FromArbitrary(object data, RegistryValueKind kind) =>
                // Take an arbitrary .NET object and turn it into bytes
                kind switch
                {
                    RegistryValueKind.String => FromString(data.ToString()!),
                    RegistryValueKind.DWord => FromDword(Convert.ToUInt32(data)),
                    RegistryValueKind.ExpandString => FromString(data.ToString()!, true),
                    RegistryValueKind.QWord => FromQword((ulong)data),
                    RegistryValueKind.MultiString => FromMultiString((string[])data),
                    _ => FromBinary((byte[])data, kind)
                };
        }
    }
}