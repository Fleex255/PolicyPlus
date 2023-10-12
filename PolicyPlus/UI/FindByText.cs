using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using PolicyPlus.Models;

namespace PolicyPlus.UI
{
    public partial class FindByText
    {
        private Dictionary<string, string>[] _commentSources;
        public Func<PolicyPlusPolicy, bool> Searcher;

        public FindByText()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(params Dictionary<string, string>[] commentDicts)
        {
            _commentSources = commentDicts.Where(d => d is not null).ToArray();
            return ShowDialog();
        }

        private void FindByText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            var text = StringTextbox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Interaction.MsgBox("Please enter search terms.", MsgBoxStyle.Exclamation);
                return;
            }
            var checkTitle = TitleCheckbox.Checked;
            var checkDesc = DescriptionCheckbox.Checked;
            var checkComment = CommentCheckbox.Checked;
            if (!(checkTitle || checkDesc || checkComment))
            {
                Interaction.MsgBox("At least one attribute must be searched. Check one of the boxes and try again.", MsgBoxStyle.Exclamation);
                return;
            }
            Searcher = new Func<PolicyPlusPolicy, bool>((policy) =>
                {
                    // Parse the query string for wildcards or quoted strings
                    var rawSplitted = Strings.Split(text);
                    var simpleWords = new List<string>();
                    var wildcards = new List<string>();
                    var quotedStrings = new List<string>();
                    var partialQuotedString = "";
                    for (int n = 0, loopTo = rawSplitted.Length - 1; n <= loopTo; n++)
                    {
                        var curString = rawSplitted[n];
                        if (!string.IsNullOrEmpty(partialQuotedString))
                        {
                            partialQuotedString += curString + " ";
                            if (curString.EndsWith("\""))
                            {
                                quotedStrings.Add(CleanupStr(partialQuotedString));
                                partialQuotedString = "";
                            }
                        }
                        else if (curString.StartsWith("\""))
                        {
                            partialQuotedString = curString + " ";
                        }
                        else if (curString.Contains("*") || curString.Contains("?"))
                        {
                            wildcards.Add(CleanupStr(curString));
                        }
                        else
                        {
                            simpleWords.Add(CleanupStr(curString));
                        }
                    }

                    if (checkTitle)
                    {
                        if (IsStringAHit(policy.DisplayName, simpleWords, wildcards, quotedStrings))
                        {
                            return true;
                        }
                    }

                    if (!checkDesc)
                    {
                        return checkComment && _commentSources.Any((source) =>
                            source.ContainsKey(policy.UniqueId) && IsStringAHit(source[policy.UniqueId], simpleWords, wildcards, quotedStrings));
                    }

                    if (IsStringAHit(policy.DisplayExplanation, simpleWords, wildcards, quotedStrings))
                    {
                        return true;
                    }
                    return checkComment && _commentSources.Any((source) => source.ContainsKey(policy.UniqueId) && IsStringAHit(source[policy.UniqueId], simpleWords, wildcards, quotedStrings));
                });
            DialogResult = DialogResult.OK;
        }

        // Do the searching
        private static bool IsStringAHit(string searchedText, IEnumerable<string> simpleWords, IEnumerable<string> wildcards, IEnumerable<string> quotedStrings)
        {
            var cleanText = CleanupStr(searchedText);
            var wordsInText = Strings.Split(cleanText);
            return simpleWords.All(w => wordsInText.Contains(w)) && wildcards.All(w => wordsInText.Any(wit => LikeOperator.LikeString(wit, w, CompareMethod.Binary))) && quotedStrings.All(w => cleanText.Contains(" " + w + " ") || cleanText.StartsWith(w + " ") || cleanText.EndsWith(" " + w) || (cleanText ?? "") == (w ?? "")); // Plain search terms
        }

        // Wildcards Quoted strings
        private static string CleanupStr(string rawText) => new(Strings.Trim(rawText.ToLowerInvariant()).Where(c => !".,'\";/!(){}[]".Contains(Conversions.ToString(c))).ToArray());
    }
}