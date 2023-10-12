using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace PolicyPlus
{
    public class SpolFile
    {
        public List<SpolPolicyState> Policies = new List<SpolPolicyState>();
        private int ParserLine = 0;
        public static SpolFile FromText(string Text)
        {
            var spol = new SpolFile();
            try
            {
                spol.LoadFromText(Text);
                return spol;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " (Error found on line " + spol.ParserLine + ".)");
            }
        }
        private void LoadFromText(string Text)
        {
            // Load a SPOL script into policy states
            string[] allLines = Strings.Split(Text, Constants.vbCrLf);
            string line = "";
            string nextLine()
            {
                ParserLine += 1;
                line = allLines[ParserLine - 1]; // For human-readability in errors
                return line;
            };
            bool atEnd() => ParserLine >= allLines.Length;
            string peekLine() => allLines[ParserLine]; // +1 for next, -1 for array
            List<string> getAllStrings(string Splittable, char Delimiter)
            {
                var list = new List<string>();
                System.Text.StringBuilder sb = null;
                for (int n = 0, loopTo = Splittable.Length - 1; n <= loopTo; n++)
                {
                    if (Splittable[n] == Delimiter)
                    {
                        if (sb is null)
                        {
                            sb = new System.Text.StringBuilder();
                        }
                        else if (n + 1 < Splittable.Length - 1 && Splittable[n + 1] == Delimiter)
                        {
                            sb.Append(Delimiter);
                            n += 1;
                        }
                        else
                        {
                            list.Add(sb.ToString());
                            sb = null;
                        }
                    }
                    else if (sb is not null)
                    {
                        sb.Append(Splittable[n]);
                    }
                }
                return list;
            };
            if (nextLine() != "Policy Plus Semantic Policy")
                throw new Exception("Incorrect signature.");
            while (!atEnd())
            {
                if (string.IsNullOrEmpty(Strings.Trim(nextLine())))
                    continue;
                string[] policyHeaderParts = Strings.Split(line, " ", 2); // Section and policy ID
                var singlePolicy = new SpolPolicyState() { UniqueID = policyHeaderParts[1] };
                singlePolicy.Section = policyHeaderParts[0] == "U" ? AdmxPolicySection.User : AdmxPolicySection.Machine;
                const string commentPrefix = "comment: ";
                if (Strings.LTrim(peekLine()).ToLowerInvariant().StartsWith(commentPrefix))
                {
                    string escapedCommentText = Strings.LTrim(nextLine()).Substring(commentPrefix.Length);
                    var commentText = new System.Text.StringBuilder();
                    for (int n = 0, loopTo = escapedCommentText.Length - 1; n <= loopTo; n++)
                    {
                        if (escapedCommentText[n] == '\\')
                        {
                            if (n == escapedCommentText.Length - 1)
                                throw new Exception("Escape sequence started at end of comment.");
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
                            n += 1;
                        }
                        else
                        {
                            commentText.Append(escapedCommentText[n]);
                        }
                    }
                    singlePolicy.Comment = commentText.ToString();
                }
                switch (Strings.Trim(nextLine()).ToLowerInvariant() ?? "")
                {
                    case "not configured":
                        {
                            singlePolicy.BasicState = PolicyState.NotConfigured;
                            break;
                        }
                    case "enabled":
                        {
                            singlePolicy.BasicState = PolicyState.Enabled;
                            break;
                        }
                    case "disabled":
                        {
                            singlePolicy.BasicState = PolicyState.Disabled;
                            break;
                        }

                    default:
                        {
                            throw new Exception("Unknown policy state.");
                        }
                }
                if (singlePolicy.BasicState == PolicyState.Enabled)
                {
                    while (!atEnd() && !string.IsNullOrEmpty(Strings.Trim(peekLine())))
                    {
                        string[] optionParts = Strings.Split(Strings.Trim(nextLine()), ": ", 2); // Name and value
                        string valueText = optionParts[1];
                        object newObj;
                        uint argresult = 0U;
                        bool argresult1 = false;
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
                        else if (valueText.StartsWith("'") & valueText.EndsWith("'"))
                        {
                            newObj = valueText.Substring(1, valueText.Length - 2);
                        }
                        else if (valueText.StartsWith("\"") & valueText.EndsWith("\""))
                        {
                            newObj = getAllStrings(valueText, '"').ToArray();
                        }
                        else if (valueText == "None")
                        {
                            newObj = Array.CreateInstance(typeof(string), 0);
                        }
                        else if (valueText == "[")
                        {
                            var entries = new List<List<string>>();
                            while (Strings.Trim(peekLine()) != "]")
                                entries.Add(getAllStrings(nextLine(), '"'));
                            nextLine(); // Skip the closing bracket
                            if (entries.Count == 0)
                            {
                                newObj = null; // PolicyProcessing will ignore an empty list element
                            }
                            else if (entries[0].Count == 1)
                            {
                                newObj = entries.Select(l => l[0]).ToList();
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
        public static string GetFragment(SpolPolicyState State)
        {
            // Create a SPOL text fragment from the given policy state
            var sb = new System.Text.StringBuilder();
            sb.Append(State.Section == AdmxPolicySection.Machine ? "C " : "U ");
            sb.AppendLine(State.UniqueID);
            if (!string.IsNullOrEmpty(State.Comment))
            {
                // Escape newlines and backslashes in the comment so it can fit on one SPOL line
                sb.AppendLine(" Comment: " + State.Comment.Replace(@"\", @"\\").Replace(Constants.vbCrLf, @"\n"));
            }
            switch (State.BasicState)
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
            }
            string doubleQuoteString(string Text) => "\"" + Text.Replace("\"", "\"\"") + "\"";
            if (State.BasicState == PolicyState.Enabled & State.ExtraOptions is not null)
            {
                foreach (var kv in State.ExtraOptions)
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
                                string[] stringArray = (string[])kv.Value;
                                if (stringArray.Length == 0)
                                    sb.AppendLine("None");
                                else
                                    sb.AppendLine(string.Join(", ", stringArray.Select(doubleQuoteString))); // List(Of String) or Dictionary(Of String, String)
                                break;
                            }

                        default:
                            {
                                sb.AppendLine("[");
                                if (kv.Value is List<string>)
                                {
                                    foreach (var listEntry in (List<string>)kv.Value)
                                    {
                                        sb.Append("   ");
                                        sb.AppendLine(doubleQuoteString(listEntry));
                                    }
                                }
                                else
                                {
                                    foreach (var listKv in (Dictionary<string, string>)kv.Value)
                                    {
                                        sb.Append("   ");
                                        sb.Append(doubleQuoteString(listKv.Key));
                                        sb.Append(": ");
                                        sb.AppendLine(doubleQuoteString(listKv.Value));
                                    }
                                }
                                sb.AppendLine("  ]");
                                break;
                            }
                    }
                }
            }
            return sb.ToString();
        }
        public int ApplyAll(AdmxBundle AdmxWorkspace, IPolicySource UserSource, IPolicySource CompSource, Dictionary<string, string> UserComments, Dictionary<string, string> CompComments)
        {
            // Write the policy states to the policy sources
            int failures = 0;
            foreach (var policy in Policies)
            {
                try
                {
                    if (policy.Section == AdmxPolicySection.Machine)
                    {
                        policy.Apply(CompSource, AdmxWorkspace, CompComments);
                    }
                    else
                    {
                        policy.Apply(UserSource, AdmxWorkspace, UserComments);
                    }
                }
                catch (Exception ex)
                {
                    failures += 1;
                }
            }
            return failures;
        }
    }

    public class SpolPolicyState
    {
        public string UniqueID;
        public AdmxPolicySection Section;
        public PolicyState BasicState;
        public string Comment;
        public Dictionary<string, object> ExtraOptions = new Dictionary<string, object>();
        public void Apply(IPolicySource PolicySource, AdmxBundle AdmxWorkspace, Dictionary<string, string> CommentsMap)
        {
            var pol = AdmxWorkspace.Policies[UniqueID];
            if (CommentsMap is not null & !string.IsNullOrEmpty(Comment))
                CommentsMap[UniqueID] = Comment;
            PolicyProcessing.ForgetPolicy(PolicySource, pol);
            PolicyProcessing.SetPolicyState(PolicySource, pol, BasicState, ExtraOptions);
        }
    }
}