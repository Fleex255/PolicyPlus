using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace PolicyPlus.Models
{
    public class SpolFile
    {
        public List<SpolPolicyState> Policies = new();
        private int _parserLine = 0;

        public static SpolFile FromText(string text)
        {
            var spol = new SpolFile();
            try
            {
                spol.LoadFromText(text);
                return spol;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " (Error found on line " + spol._parserLine + ".)");
            }
        }

        private void LoadFromText(string text)
        {
            // Load a SPOL script into policy states
            var allLines = Strings.Split(text, Constants.vbCrLf);
            if (NextLine(allLines) != "Policy Plus Semantic Policy")
            {
                throw new Exception("Incorrect signature.");
            }

            while (!AtEnd(allLines))
            {
                var line = NextLine(allLines);
                if (string.IsNullOrEmpty(Strings.Trim(line)))
                {
                    continue;
                }

                var policyHeaderParts = Strings.Split(line, " ", 2); // Section and policy ID
                var singlePolicy = new SpolPolicyState
                {
                    UniqueId = policyHeaderParts[1],
                    Section = policyHeaderParts[0] == "U" ? AdmxPolicySection.User : AdmxPolicySection.Machine
                };
                const string commentPrefix = "comment: ";
                if (Strings.LTrim(PeekLine(allLines)).ToLowerInvariant().StartsWith(commentPrefix))
                {
                    var escapedCommentText = Strings.LTrim(NextLine(allLines)).Substring(commentPrefix.Length);
                    var commentText = new System.Text.StringBuilder();
                    for (int n = 0, loopTo = escapedCommentText.Length - 1; n <= loopTo; n++)
                    {
                        if (escapedCommentText[n] == '\\')
                        {
                            if (n == escapedCommentText.Length - 1)
                            {
                                throw new Exception("Escape sequence started at end of comment.");
                            }

                            switch (escapedCommentText[n + 1])
                            {
                                case '\\':
                                    {
                                        commentText.Append('\\');
                                        break;
                                    }
                                case 'n':
                                    {
                                        commentText.Append(Constants.vbCrLf);
                                        break;
                                    }

                                default:
                                    {
                                        throw new Exception(@"Unknown comment escape sequence \" + escapedCommentText[n + 1] + ".");
                                    }
                            }
                            n++;
                        }
                        else
                        {
                            commentText.Append(escapedCommentText[n]);
                        }
                    }
                    singlePolicy.Comment = commentText.ToString();
                }

                singlePolicy.BasicState = (Strings.Trim(NextLine(allLines)).ToLowerInvariant() ?? "") switch
                {
                    "not configured" => PolicyState.NotConfigured,
                    "enabled" => PolicyState.Enabled,
                    "disabled" => PolicyState.Disabled,
                    _ => throw new Exception("Unknown policy state.")
                };
                if (singlePolicy.BasicState == PolicyState.Enabled)
                {
                    while (!AtEnd(allLines) && !string.IsNullOrEmpty(Strings.Trim(PeekLine(allLines))))
                    {
                        var optionParts = Strings.Split(Strings.Trim(NextLine(allLines)), ": ", 2); // Name and value
                        var valueText = optionParts[1];
                        object newObj;
                        var argresult = 0U;
                        var argresult1 = false;
                        if (valueText.StartsWith("#"))
                        {
                            newObj = Conversions.ToInteger(valueText.Substring(1));
                        }
                        else if (uint.TryParse(valueText, out argresult))
                        {
                            newObj = Conversions.ToUInteger(valueText);
                        }
                        else if (bool.TryParse(valueText, out argresult1))
                        {
                            newObj = Conversions.ToBoolean(valueText);
                        }
                        else if (valueText.StartsWith("'") && valueText.EndsWith("'"))
                        {
                            newObj = valueText.Substring(1, valueText.Length - 2);
                        }
                        else if (valueText.StartsWith("\"") && valueText.EndsWith("\""))
                        {
                            newObj = GetAllStrings(valueText, '"').ToArray();
                        }
                        else if (valueText == "None")
                        {
                            newObj = Array.Empty<string>();
                        }
                        else if (valueText == "[")
                        {
                            var entries = new List<List<string>>();
                            while (Strings.Trim(PeekLine(allLines)) != "]")
                            {
                                entries.Add(GetAllStrings(NextLine(allLines), '"'));
                            }

                            NextLine(allLines); // Skip the closing bracket
                            if (entries.Count == 0)
                            {
                                newObj = null; // PolicyProcessing will ignore an empty list element
                            }
                            else if (entries[0].Count == 1)
                            {
                                newObj = entries.ConvertAll(l => l[0]);
                            }
                            else
                            {
                                newObj = entries.ToDictionary(l => l[0], l => l[1]);
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown option data format.");
                        }
                        singlePolicy.ExtraOptions.Add(optionParts[0], newObj);
                    }
                }
                Policies.Add(singlePolicy);
            }
        }

        private static List<string> GetAllStrings(string splittable, char delimiter)
        {
            var list = new List<string>();
            System.Text.StringBuilder sb = null;
            for (int n = 0, loopTo = splittable.Length - 1; n <= loopTo; n++)
            {
                if (splittable[n] == delimiter)
                {
                    if (sb is null)
                    {
                        sb = new System.Text.StringBuilder();
                    }
                    else if (n + 1 < splittable.Length - 1 && splittable[n + 1] == delimiter)
                    {
                        sb.Append(delimiter);
                        n++;
                    }
                    else
                    {
                        list.Add(sb.ToString());
                        sb = null;
                    }
                }
                else
                {
                    sb?.Append(splittable[n]);
                }
            }
            return list;
        }

        private string PeekLine(IReadOnlyList<string> allLines) => allLines[_parserLine];

        private bool AtEnd(IReadOnlyCollection<string> allLines) => _parserLine >= allLines.Count;

        private string NextLine(IReadOnlyList<string> allLines)
        {
            _parserLine++;
            // For human-readability in errors
            return allLines[_parserLine - 1];
        }

        public static string GetFragment(SpolPolicyState state)
        {
            // Create a SPOL text fragment from the given policy state
            var sb = new System.Text.StringBuilder();
            sb.Append(state.Section == AdmxPolicySection.Machine ? "C " : "U ");
            sb.AppendLine(state.UniqueId);
            if (!string.IsNullOrEmpty(state.Comment))
            {
                // Escape newlines and backslashes in the comment so it can fit on one SPOL line
                _ = sb.Append(" Comment: ").AppendLine(state.Comment.Replace(@"\", @"\\").Replace(Constants.vbCrLf, @"\n"));
            }
            switch (state.BasicState)
            {
                case PolicyState.NotConfigured:
                    {
                        sb.AppendLine(" Not Configured");
                        break;
                    }
                case PolicyState.Enabled:
                    {
                        sb.AppendLine(" Enabled");
                        break;
                    }
                case PolicyState.Disabled:
                    {
                        sb.AppendLine(" Disabled");
                        break;
                    }
                case PolicyState.Unknown:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (state.BasicState != PolicyState.Enabled || state.ExtraOptions is null)
            {
                return sb.ToString();
            }

            foreach (var kv in state.ExtraOptions)
            {
                sb.Append("  ");
                sb.Append(kv.Key);
                sb.Append(": ");
                switch (kv.Value.GetType())
                {
                    case var @case when @case == typeof(int):
                        {
                            sb.Append("#");
                            sb.AppendLine(Conversions.ToInteger(kv.Value).ToString());
                            break;
                        }
                    case var case1 when case1 == typeof(uint):
                        {
                            sb.AppendLine(Conversions.ToUInteger(kv.Value).ToString());
                            break;
                        }
                    case var case2 when case2 == typeof(bool):
                        {
                            sb.AppendLine(Conversions.ToString(Conversions.ToBoolean(kv.Value)));
                            break;
                        }
                    case var case3 when case3 == typeof(string):
                        {
                            sb.Append("'");
                            sb.Append(Conversions.ToString(kv.Value));
                            sb.AppendLine("'");
                            break;
                        }
                    case var case4 when case4 == typeof(string[]):
                        {
                            var stringArray = (string[])kv.Value;
                            sb.AppendLine(stringArray.Length == 0
                                ? "None"
                                : string.Join(", ", stringArray.Select(DoubleQuoteString))); // List(Of String) or Dictionary(Of String, String)

                            break;
                        }

                    default:
                        {
                            sb.AppendLine("[");
                            if (kv.Value is List<string> list)
                            {
                                foreach (var listEntry in list)
                                {
                                    sb.Append("   ");
                                    sb.AppendLine(DoubleQuoteString(listEntry));
                                }
                            }
                            else
                            {
                                foreach (var listKv in (Dictionary<string, string>)kv.Value)
                                {
                                    sb.Append("   ");
                                    sb.Append(DoubleQuoteString(listKv.Key));
                                    sb.Append(": ");
                                    sb.AppendLine(DoubleQuoteString(listKv.Value));
                                }
                            }
                            sb.AppendLine("  ]");
                            break;
                        }
                }
            }
            return sb.ToString();
        }

        private static string DoubleQuoteString(string text) => "\"" + text.Replace("\"", "\"\"") + "\"";

        public int ApplyAll(AdmxBundle admxWorkspace, IPolicySource userSource, IPolicySource compSource, Dictionary<string, string> userComments, Dictionary<string, string> compComments)
        {
            // Write the policy states to the policy sources
            var failures = 0;
            foreach (var policy in Policies)
            {
                try
                {
                    if (policy.Section == AdmxPolicySection.Machine)
                    {
                        policy.Apply(compSource, admxWorkspace, compComments);
                    }
                    else
                    {
                        policy.Apply(userSource, admxWorkspace, userComments);
                    }
                }
                catch (Exception)
                {
                    failures++;
                }
            }
            return failures;
        }
    }
}