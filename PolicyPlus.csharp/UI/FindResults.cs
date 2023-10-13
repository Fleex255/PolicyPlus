using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class FindResults
    {
        private AdmxBundle _admxWorkspace;
        private Func<PolicyPlusPolicy, bool> _searchFunc;
        private bool _cancelingSearch;
        private bool _cancelDueToFormClose;
        private bool _searchPending;
        private bool _hasSearched;
        private int _lastSelectedIndex;
        public PolicyPlusPolicy SelectedPolicy;

        public FindResults()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialogStartSearch(AdmxBundle workspace, Func<PolicyPlusPolicy, bool> searcher)
        {
            // Start running a search defined by one of the Find By windows
            _admxWorkspace = workspace;
            _searchFunc = searcher;
            ResultsListview.Items.Clear();
            SearchProgress.Maximum = workspace.Policies.Count;
            SearchProgress.Value = 0;
            StopButton.Enabled = true;
            _cancelingSearch = false;
            _cancelDueToFormClose = false;
            ProgressLabel.Text = "Starting search";
            _searchPending = true;
            _hasSearched = true;
            _lastSelectedIndex = -1;
            return ShowDialog(this);
        }

        public DialogResult PresentDialog()
        {
            // Open the dialog normally, like from the main form
            if (!_hasSearched)
            {
                _ = MessageBox.Show("No search has been run yet, so there are no results to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return DialogResult.Cancel;
            }
            _cancelingSearch = false;
            _cancelDueToFormClose = false;
            _searchPending = false;
            return ShowDialog(this);
        }

        public void ClearSearch()
        {
            _hasSearched = false;
            ResultsListview.Items.Clear();
        }

        public PolicyPlusPolicy? NextPolicy()
        {
            if (_lastSelectedIndex >= ResultsListview.Items.Count - 1 || !_hasSearched)
            {
                return null;
            }

            _lastSelectedIndex++;
            return (PolicyPlusPolicy)ResultsListview.Items[_lastSelectedIndex].Tag;
        }

        public void SearchJob(AdmxBundle workspace, Func<PolicyPlusPolicy, bool> searcher)
        {
            // The long-running task that searches all the policies
            var searchedSoFar = 0;
            var results = 0;
            var stoppedByCancel = false;
            var pendingInsertions = new List<PolicyPlusPolicy>();

            foreach (var policy in workspace.Policies)
            {
                if ((bool)LocalVolatileRead())
                {
                    stoppedByCancel = true;
                    break;
                }
                searchedSoFar++;
                var isHit = searcher(policy.Value); // The potentially expensive check
                if (isHit)
                {
                    results++;
                    pendingInsertions.Add(policy.Value);
                }
                if (searchedSoFar % 20 == 0) // UI updating is costly
                {
                    Invoke(new Action(() =>
                        {
                            AddPendingInsertions(pendingInsertions);
                            SearchProgress.Value = searchedSoFar;
                            ProgressLabel.Text = "Searching: checked " + searchedSoFar + " policies so far, found " + results + " hits";
                        }));
                }
            }

            if (stoppedByCancel && (bool)LocalVolatileRead2())
            {
                return; // Avoid accessing a disposed object
            }

            Invoke(new Action(() =>
                {
                    AddPendingInsertions(pendingInsertions);
                    var status = stoppedByCancel ? "Search canceled" : "Finished searching";
                    ProgressLabel.Text = status + ": checked " + searchedSoFar + " policies, found " + results + " hits";
                    SearchProgress.Value = SearchProgress.Maximum;
                    StopButton.Enabled = false;
                }));
        }

        private object LocalVolatileRead2()
        {
            object argaddress2 = _cancelDueToFormClose;
            var ret = Thread.VolatileRead(ref argaddress2);
            _cancelDueToFormClose = (bool)argaddress2;
            return ret;
        }

        //private object LocalVolatileRead1()
        //{
        //    object argaddress1 = _cancelDueToFormClose;
        //    var ret = System.Threading.Thread.VolatileRead(ref argaddress1);
        //    _cancelDueToFormClose = (bool)(argaddress1);
        //    return ret;
        //}

        private void AddPendingInsertions(List<PolicyPlusPolicy> pendingInsertions)
        {
            ResultsListview.BeginUpdate();
            foreach (var insert in pendingInsertions)
            {
                var lsvi = ResultsListview.Items.Add(insert.DisplayName);
                lsvi.Tag = insert;
                _ = lsvi.SubItems.Add(insert.Category.DisplayName);
            }
            ResultsListview.EndUpdate();
            pendingInsertions.Clear();
        }

        private object LocalVolatileRead()
        {
            object argaddress = _cancelingSearch; var ret = Thread.VolatileRead(ref argaddress);
            _cancelingSearch = (bool)argaddress;
            return ret;
        }

        public void StopSearch(bool force)
        {
            object argaddress = _cancelingSearch;
            Thread.VolatileWrite(ref argaddress, true);
            _cancelingSearch = (bool)argaddress;
            object argaddress1 = _cancelDueToFormClose;
            Thread.VolatileWrite(ref argaddress1, force);
            _cancelDueToFormClose = (bool)argaddress1;
        }

        private void FindResults_Shown(object sender, EventArgs e)
        {
            if (_searchPending)
            {
                _ = Task.Factory.StartNew(() => SearchJob(_admxWorkspace, _searchFunc));
            }
            else if (_lastSelectedIndex >= 0 && _lastSelectedIndex < ResultsListview.Items.Count)
            {
                // Restore the last selection
                var lastSelected = ResultsListview.Items[_lastSelectedIndex];
                lastSelected.Selected = true;
                lastSelected.Focused = true;
                lastSelected.EnsureVisible();
            }
        }

        private void StopButton_Click(object sender, EventArgs e) => StopSearch(false);

        private void ResultsListview_SizeChanged(object sender, EventArgs e) => ChTitle.Width = ResultsListview.ClientSize.Width - ChCategory.Width;

        private void FindResults_Closing(object sender, CancelEventArgs e)
        {
            StopSearch(true);
            if (SearchProgress.Value == SearchProgress.Maximum)
            {
                return;
            }

            ProgressLabel.Text = "Search canceled";
            SearchProgress.Maximum = 100;
            SearchProgress.Value = SearchProgress.Maximum;
        }

        private void GoClicked(object sender, EventArgs e)
        {
            if (ResultsListview.SelectedItems.Count == 0)
            {
                return;
            }

            SelectedPolicy = (PolicyPlusPolicy)ResultsListview.SelectedItems[0].Tag;
            _lastSelectedIndex = ResultsListview.SelectedIndices[0]; // Remember which item is selected
            DialogResult = DialogResult.OK;
        }

        private void FindResults_Load(object sender, EventArgs e)
        {
            // Enable double-buffering for the results view
            var doubleBufferProp = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            doubleBufferProp?.SetValue(ResultsListview, true);
        }
    }
}