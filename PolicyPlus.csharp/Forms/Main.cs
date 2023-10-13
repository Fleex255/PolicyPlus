using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using PolicyPlus.csharp.Helpers;
using PolicyPlus.csharp.Models;
using PolicyPlus.csharp.Models.Sources;
using PolicyPlus.csharp.Models.Sources.Admx;

namespace PolicyPlus.csharp.UI
{
    public partial class Main
    {
        private readonly Dictionary<PolicyPlusCategory, TreeNode> _categoryNodes = new();
        private AdmxBundle _admxWorkspace = new();
        private ConfigurationStorage _configuration;
        private PolicyPlusCategory? _currentCategory;
        private FilterConfiguration? _currentFilter = new();
        private PolicyPlusPolicy? _currentSetting;
        private PolicyPlusCategory? _highlightCategory;
        private Dictionary<string, string>? _userComments;
        private Dictionary<string, string>? _compComments;
        private PolicyLoader _userPolicyLoader;
        private PolicyLoader _compPolicyLoader;
        private IPolicySource _userPolicySource;
        private IPolicySource _compPolicySource;
        private bool _viewEmptyCategories;
        private bool _viewFilteredOnly;
        private AdmxPolicySection _viewPolicyTypes = AdmxPolicySection.Both;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // Create the configuration manager (for the Registry)
            _configuration = new ConfigurationStorage(RegistryHive.CurrentUser, @"Software\Policy Plus");
            // Restore the last ADMX source and policy loaders
            OpenLastAdmxSource();
            var compLoaderType =
                (PolicyLoaderSource)(int)_configuration.GetValue("CompSourceType", 0)!;
            var compLoaderData = _configuration.GetValue("CompSourceData", "");
            var userLoaderType =
                (PolicyLoaderSource)(int)_configuration.GetValue("UserSourceType", 0)!;
            var userLoaderData = _configuration.GetValue("UserSourceData", "");
            try
            {
                OpenPolicyLoaders(new PolicyLoader(userLoaderType, userLoaderData?.ToString()!, true),
                    new PolicyLoader(compLoaderType, compLoaderData?.ToString()!, false), true);
            }
            catch (Exception)
            {
                _ = MessageBox.Show("The previous policy sources are not accessible. The defaults will be loaded.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _configuration.SetValue("CompSourceType", (int)PolicyLoaderSource.LocalGpo);
                _configuration.SetValue("UserSourceType", (int)PolicyLoaderSource.LocalGpo);
                OpenPolicyLoaders(new PolicyLoader(PolicyLoaderSource.LocalGpo, "", true),
                    new PolicyLoader(PolicyLoaderSource.LocalGpo, "", false), true);
            }

            var dialog = new OpenPol();
            dialog.SetLastSources(compLoaderType, compLoaderData!.ToString()!, userLoaderType,
                userLoaderData!.ToString()!);

            // Set up the UI
            ComboAppliesTo.Text = ComboAppliesTo.Items[0].ToString();
            CategoriesTree.Height -= InfoStrip.ClientSize.Height;
            SettingInfoPanel.Height -= InfoStrip.ClientSize.Height;
            PoliciesList.Height -= InfoStrip.ClientSize.Height;
            InfoStrip.Items.Insert(2, new ToolStripSeparator());
            PopulateAdmxUi();
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            // Check whether ADMX files probably need to be downloaded
            if ((int)_configuration.GetValue("CheckedPolicyDefinitions", 0)! != 0)
            {
                return;
            }

            _configuration.SetValue("CheckedPolicyDefinitions", 1);
            if (SystemInfo.HasGroupPolicyInfrastructure() ||
                _admxWorkspace.Categories.Values.Count(c => IsOrphanCategory(c) && !IsEmptyCategory(c)) <= 2)
            {
                return;
            }

            if (MessageBox.Show($"Welcome to Policy Plus!{Environment.NewLine}{Environment.NewLine}Home editions do not come with the full set of policy definitions. Would you like to download them now? This can also be done later with Help | Acquire ADMX Files.",
                    "Welcome", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                AcquireADMXFilesToolStripMenuItem_Click(null, null);
            }
        }

        public void OpenLastAdmxSource()
        {
            var defaultAdmxSource = Environment.ExpandEnvironmentVariables(@"%windir%\PolicyDefinitions");
            var admxSource = _configuration.GetValue("AdmxSource", defaultAdmxSource)!.ToString();
            try
            {
                var fails = _admxWorkspace.LoadFolder(admxSource!, GetPreferredLanguageCode());
                if (DisplayAdmxLoadErrorReport(fails, true) == DialogResult.No)
                {
                    throw new Exception("You decided to not use the problematic ADMX bundle.");
                }
            }
            catch (Exception ex)
            {
                _admxWorkspace = new AdmxBundle();
                var loadFailReason = "";
                if ((admxSource ?? "") != (defaultAdmxSource ?? ""))
                {
                    if (MessageBox.Show($"Policy definitions could not be loaded from \"{admxSource}\": {ex.Message}{Environment.NewLine}{Environment.NewLine}Load from the default location?",
                    "Welcome", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            _configuration.SetValue("AdmxSource", defaultAdmxSource!);
                            _admxWorkspace = new AdmxBundle();
                            _ = DisplayAdmxLoadErrorReport(_admxWorkspace.LoadFolder(defaultAdmxSource!,
                                GetPreferredLanguageCode()));
                        }
                        catch (Exception ex2)
                        {
                            loadFailReason = ex2.Message;
                        }
                    }
                }
                else
                {
                    loadFailReason = ex.Message;
                }

                if (!string.IsNullOrEmpty(loadFailReason))
                {
                    _ = MessageBox.Show($"Policy definitions could not be loaded: {loadFailReason}",
                    "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        public void PopulateAdmxUi()
        {
            // Populate the left categories tree
            CategoriesTree.Nodes.Clear();
            _categoryNodes.Clear();
            AddCategory(_admxWorkspace.Categories.Values, CategoriesTree.Nodes);
            CategoriesTree.Sort();
            _currentCategory = null;
            UpdateCategoryListing();
            ClearSelections();
            UpdatePolicyInfo();
        }

        private void AddCategory(IEnumerable<PolicyPlusCategory> categoryList, TreeNodeCollection parentNode)
        {
            foreach (var category in categoryList.Where(ShouldShowCategory))
            {
                var newNode = parentNode.Add(category.UniqueId, category.DisplayName,
                    GetImageIndexForCategory(category));
                newNode.SelectedImageIndex = 3;
                newNode.Tag = category;
                _categoryNodes.Add(category, newNode);
                AddCategory(category.Children, newNode.Nodes);
            }
        } // "Go" arrow

        public void UpdateCategoryListing()
        {
            // Update the right pane to include the current category's children
            var topItemIndex = default(int?);
            if (PoliciesList.TopItem is not null)
            {
                topItemIndex = PoliciesList.TopItem.Index;
            }

            var inSameCategory = false;
            PoliciesList.Items.Clear();
            if (_currentCategory is null)
            {
                return;
            }

            if (_currentSetting is not null && ReferenceEquals(_currentSetting.Category, _currentCategory))
            {
                inSameCategory = true;
            }

            if (_currentCategory.Parent is not null) // Add the parent
            {
                var listItem = PoliciesList.Items.Add("Up: " + _currentCategory.Parent.DisplayName);
                listItem.Tag = _currentCategory.Parent;
                listItem.ImageIndex = 6; // Up arrow
                _ = listItem.SubItems.Add("Parent");
            }

            foreach (var category in
                     _currentCategory.Children.Where(ShouldShowCategory)
                         .OrderBy(c => c.DisplayName)) // Add subcategories
            {
                var listItem = PoliciesList.Items.Add(category.DisplayName);
                listItem.Tag = category;
                listItem.ImageIndex = GetImageIndexForCategory(category);
            }

            foreach (var policy in
                     _currentCategory.Policies.Where(ShouldShowPolicy).OrderBy(p => p.DisplayName)) // Add policies
            {
                var listItem = PoliciesList.Items.Add(policy.DisplayName);
                listItem.Tag = policy;
                listItem.ImageIndex = GetImageIndexForSetting(policy);
                _ = listItem.SubItems.Add(GetPolicyState(policy));
                _ = listItem.SubItems.Add(GetPolicyCommentText(policy));
                if (!ReferenceEquals(policy, _currentSetting)) // Keep the current policy selected
                {
                    continue;
                }

                listItem.Selected = true;
                listItem.Focused = true;
                listItem.EnsureVisible();
            }

            if (topItemIndex.HasValue && inSameCategory &&
                PoliciesList.Items.Count >
                topItemIndex.Value) // Minimize the list view's jumping around when refreshing
            {
                PoliciesList.TopItem = PoliciesList.Items[topItemIndex.Value];
            }

            if (CategoriesTree.SelectedNode is null ||
                !ReferenceEquals(CategoriesTree.SelectedNode.Tag, _currentCategory)) // Update the tree view
            {
                CategoriesTree.SelectedNode = _categoryNodes[_currentCategory];
            }
        }

        public void UpdatePolicyInfo()
        {
            // Update the middle pane with the selected object's information
            var hasCurrentSetting = _currentSetting is not null || _highlightCategory is not null ||
                                    _currentCategory is not null;
            PolicyTitleLabel.Visible = hasCurrentSetting;
            PolicySupportedLabel.Visible = hasCurrentSetting;
            if (_currentSetting is not null)
            {
                PolicyTitleLabel.Text = _currentSetting.DisplayName;
                if (_currentSetting.SupportedOn is null)
                {
                    PolicySupportedLabel.Text = "(no requirements information)";
                }
                else
                {
                    PolicySupportedLabel.Text =
                        "Requirements:" + Environment.NewLine + _currentSetting.SupportedOn.DisplayName;
                }

                PolicyDescLabel.Text = PrettifyDescription(_currentSetting.DisplayExplanation);
                PolicyIsPrefTable.Visible = IsPreference(_currentSetting);
            }
            else if (_highlightCategory is not null || _currentCategory is not null)
            {
                var shownCategory = _highlightCategory ?? _currentCategory;
                PolicyTitleLabel.Text = shownCategory.DisplayName;
                PolicySupportedLabel.Text = (_highlightCategory is null ? "This" : "The selected") +
                                            " category contains " + shownCategory.Policies.Count + " policies and " +
                                            shownCategory.Children.Count + " subcategories.";
                PolicyDescLabel.Text = PrettifyDescription(shownCategory.DisplayExplanation);
                PolicyIsPrefTable.Visible = false;
            }
            else
            {
                PolicyDescLabel.Text = "Select an item to see its description.";
                PolicyIsPrefTable.Visible = false;
            }

            SettingInfoPanel_ClientSizeChanged(null, null);
        }

        public bool IsOrphanCategory(PolicyPlusCategory category) =>
            category.Parent is null && !string.IsNullOrEmpty(category.RawCategory.ParentId);

        public bool IsEmptyCategory(PolicyPlusCategory category) =>
            category.Children.Count == 0 && category.Policies.Count == 0;

        public int GetImageIndexForCategory(PolicyPlusCategory category)
        {
            if (IsOrphanCategory(category))
            {
                return 1; // Orphaned
            }

            return IsEmptyCategory(category)
                ? 2
                : // Empty
                0; // Normal
        }

        public int GetImageIndexForSetting(PolicyPlusPolicy setting)
        {
            if (IsPreference(setting))
            {
                return 7; // Preference, not policy (exclamation mark)
            }

            if (setting.RawPolicy.Elements is null || setting.RawPolicy.Elements.Count == 0)
            {
                return 4; // Normal
            }

            return 5; // Extra configuration
        }

        public bool ShouldShowCategory(PolicyPlusCategory category)
        {
            // Should this category be shown considering the current filter?
            if (_viewEmptyCategories)
            {
                return true;
            }

            // Only if it has visible children
            return category.Policies.Any(ShouldShowPolicy) || category.Children.Any(ShouldShowCategory);
        }

        public bool ShouldShowPolicy(PolicyPlusPolicy policy)
        {
            // Should this policy be shown considering the current filter and active sections?
            if (!PolicyVisibleInSection(policy, _viewPolicyTypes))
            {
                return false;
            }

            if (!_viewFilteredOnly)
            {
                return true;
            }

            var visibleAfterFilter = false;
            if ((int)(_viewPolicyTypes & AdmxPolicySection.Machine) > 0 &&
                PolicyVisibleInSection(policy, AdmxPolicySection.Machine))
            {
                if (IsPolicyVisibleAfterFilter(policy, false))
                {
                    visibleAfterFilter = true;
                }
            }
            else if ((int)(_viewPolicyTypes & AdmxPolicySection.User) > 0 &&
                     PolicyVisibleInSection(policy, AdmxPolicySection.User))
            {
                if (IsPolicyVisibleAfterFilter(policy, true))
                {
                    visibleAfterFilter = true;
                }
            }

            return visibleAfterFilter;
        }

        public void MoveToVisibleCategoryAndReload()
        {
            // Move up in the categories tree until a visible one is found
            var newFocusCategory = _currentCategory;
            var newFocusPolicy = _currentSetting;
            while (newFocusCategory is not null && !ShouldShowCategory(newFocusCategory))
            {
                newFocusCategory = newFocusCategory.Parent;
                newFocusPolicy = null;
            }

            if (newFocusPolicy is not null && !ShouldShowPolicy(newFocusPolicy))
            {
                newFocusPolicy = null;
            }

            PopulateAdmxUi();
            _currentCategory = newFocusCategory;
            UpdateCategoryListing();
            _currentSetting = newFocusPolicy;
            UpdatePolicyInfo();
        }

        public string GetPolicyState(PolicyPlusPolicy policy)
        {
            // Get a human-readable string describing the status of a policy, considering all active sections
            if (_viewPolicyTypes == AdmxPolicySection.Both)
            {
                var userState = GetPolicyState(policy, AdmxPolicySection.User);
                var machState = GetPolicyState(policy, AdmxPolicySection.Machine);
                var section = policy.RawPolicy.Section;
                return section switch
                {
                    AdmxPolicySection.Both when (userState ?? "") == (machState ?? "") => userState + " (2)",
                    AdmxPolicySection.Both when userState == "Not Configured" => machState + " (C)",
                    AdmxPolicySection.Both when machState == "Not Configured" => userState + " (U)",
                    AdmxPolicySection.Both => "Mixed",
                    AdmxPolicySection.Machine => machState + " (C)",
                    _ => userState + " (U)"
                };
            }

            return GetPolicyState(policy, _viewPolicyTypes);
        }

        public string GetPolicyState(PolicyPlusPolicy policy, AdmxPolicySection section) =>
            // Get the human-readable status of a policy considering only one section
            PolicyProcessing.GetPolicyState(
                    section == AdmxPolicySection.Machine ? _compPolicySource : _userPolicySource, policy) switch
            {
                PolicyState.Disabled => "Disabled",
                PolicyState.Enabled => "Enabled",
                PolicyState.NotConfigured => "Not Configured",
                _ => "Unknown"
            };

        public string GetPolicyCommentText(PolicyPlusPolicy policy)
        {
            // Get the comment text to show in the Comment column, considering all active sections
            if (_viewPolicyTypes != AdmxPolicySection.Both)
            {
                return GetPolicyComment(policy, _viewPolicyTypes);
            }

            var userComment = GetPolicyComment(policy, AdmxPolicySection.User);
            var compComment = GetPolicyComment(policy, AdmxPolicySection.Machine);
            if (string.IsNullOrEmpty(userComment) && string.IsNullOrEmpty(compComment))
            {
                return "";
            }

            if (!string.IsNullOrEmpty(userComment) && !string.IsNullOrEmpty(compComment))
            {
                return "(multiple)";
            }

            return !string.IsNullOrEmpty(userComment) ? userComment : compComment;
        }

        public string GetPolicyComment(PolicyPlusPolicy policy, AdmxPolicySection section)
        {
            // Get a policy's comment in one section
            var commentSource = section == AdmxPolicySection.Machine ? _compComments : _userComments;
            if (commentSource is null)
            {
                return string.Empty;
            }

            return commentSource.TryGetValue(policy.UniqueId, out var comment) ? comment : string.Empty;
        }

        public bool IsPreference(PolicyPlusPolicy policy) => !string.IsNullOrEmpty(policy.RawPolicy.RegistryKey) &&
                                                             !RegistryPolicyProxy.IsPolicyKey(policy.RawPolicy
                                                                 .RegistryKey);

        public void ShowSettingEditor(PolicyPlusPolicy policy, AdmxPolicySection section)
        {
            // Show the Edit Policy Setting dialog for a policy and reload if changes were made
            if (new EditSetting().PresentDialog(policy, section, _admxWorkspace, _compPolicySource,
                    _userPolicySource, _compPolicyLoader, _userPolicyLoader, _compComments!, _userComments!) !=
                DialogResult.OK)
            {
                return;
            }

            // Keep the selection where it is if possible
            if (_currentCategory is null || ShouldShowCategory(_currentCategory))
            {
                UpdateCategoryListing();
            }
            else
            {
                MoveToVisibleCategoryAndReload();
            }
        }

        public void ClearSelections()
        {
            _currentSetting = null;
            _highlightCategory = null;
        }

        public void OpenPolicyLoaders(PolicyLoader user, PolicyLoader computer, bool quiet)
        {
            // Create policy sources from the given loaders
            if (_compPolicyLoader is not null || _userPolicyLoader is not null)
            {
                ClosePolicySources();
            }

            _userPolicyLoader = user;
            _userPolicySource = user.OpenSource();
            _compPolicyLoader = computer;
            _compPolicySource = computer.OpenSource();
            var allOk = true;

            var userStatus = PolicyStatus(user, ref allOk);
            var compStatus = PolicyStatus(computer, ref allOk);
            _userComments = LoadComments(user);
            _compComments = LoadComments(computer);
            UserSourceLabel.Text = _userPolicyLoader.GetDisplayInfo();
            ComputerSourceLabel.Text = _compPolicyLoader.GetDisplayInfo();
            if (allOk)
            {
                if (!quiet)
                {
                    _ = MessageBox.Show("Both the user and computer policy sources are loaded and writable.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                var msgText = $"Not all policy sources are fully writable.{Environment.NewLine}{Environment.NewLine}The user source {userStatus}.{Environment.NewLine}{Environment.NewLine}The computer source {compStatus}.";

                _ = MessageBox.Show(msgText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static string PolicyStatus(PolicyLoader loader, ref bool allOk)
        {
            switch (loader.GetWritability())
            {
                case PolicySourceWritability.Writable:
                    {
                        return "is fully writable";
                    }
                case PolicySourceWritability.NoCommit:
                    {
                        allOk = false;
                        return "cannot be saved";
                    }
                case PolicySourceWritability.NoWriting:
                default:
                    allOk = false;
                    return "cannot be modified";
            }
        }

        private static Dictionary<string, string>? LoadComments(PolicyLoader loader)
        {
            var cmtxPath = loader.GetCmtxPath();
            if (string.IsNullOrEmpty(cmtxPath))
            {
                return null;
            }

            try
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(cmtxPath) ?? throw new InvalidOperationException());
                return File.Exists(cmtxPath)
                    ? CmtxFile.Load(cmtxPath).ToCommentTable()
                    : new Dictionary<string, string>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void ClosePolicySources()
        {
            // Clean up the policy sources
            var allOk = _userPolicyLoader?.Close() == true;
            if (_compPolicyLoader?.Close() == false)
            {
                allOk = false;
            }

            if (!allOk)
            {
                _ = MessageBox.Show("Cleanup did not complete fully because the loaded resources are open in other programs.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void ShowSearchDialog(Func<PolicyPlusPolicy, bool>? searcher)
        {
            // Show the search dialog and make it start a search if appropriate
            DialogResult result;
            var dialog = new FindResults();
            if (searcher is null)
            {
                result = dialog.PresentDialog();
            }
            else
            {
                result = dialog.PresentDialogStartSearch(_admxWorkspace, searcher);
            }

            if (result == DialogResult.OK)
            {
                var selPol = dialog.SelectedPolicy;
                ShowSettingEditor(selPol, _viewPolicyTypes);
                FocusPolicy(selPol);
            }
        }

        public void ClearAdmxWorkspace()
        {
            // Clear out all the per-workspace bookkeeping
            _admxWorkspace = new AdmxBundle();

            new FindResults().ClearSearch();
        }

        public void FocusPolicy(PolicyPlusPolicy policy)
        {
            // Try to automatically select a policy in the list view
            if (_categoryNodes.ContainsKey(policy.Category))
            {
                _currentCategory = policy.Category;
                UpdateCategoryListing();
                foreach (ListViewItem entry in PoliciesList.Items)
                {
                    if (ReferenceEquals(entry.Tag, policy))
                    {
                        entry.Selected = true;
                        entry.Focused = true;
                        entry.EnsureVisible();
                        break;
                    }
                }
            }
        }

        public bool IsPolicyVisibleAfterFilter(PolicyPlusPolicy policy, bool isUser)
        {
            // Calculate whether a policy is visible with the current filter
            if (_currentFilter.ManagedPolicy.HasValue && IsPreference(policy) == _currentFilter.ManagedPolicy.Value)
            {
                return false;
            }

            if (_currentFilter.PolicyState.HasValue)
            {
                var policyState =
                    PolicyProcessing.GetPolicyState(isUser ? _userPolicySource : _compPolicySource, policy);
                switch (_currentFilter.PolicyState.Value)
                {
                    case FilterPolicyState.Configured:
                        {
                            if (policyState == PolicyState.NotConfigured)
                            {
                                return false;
                            }

                            break;
                        }
                    case FilterPolicyState.NotConfigured:
                        {
                            if (policyState != PolicyState.NotConfigured)
                            {
                                return false;
                            }

                            break;
                        }
                    case FilterPolicyState.Disabled:
                        {
                            if (policyState != PolicyState.Disabled)
                            {
                                return false;
                            }

                            break;
                        }
                    case FilterPolicyState.Enabled:
                        {
                            if (policyState != PolicyState.Enabled)
                            {
                                return false;
                            }

                            break;
                        }
                }
            }

            if (!_currentFilter.Commented.HasValue)
            {
                return _currentFilter.AllowedProducts is null || PolicyProcessing.IsPolicySupported(policy,
                    _currentFilter.AllowedProducts, _currentFilter.AlwaysMatchAny, _currentFilter.MatchBlankSupport);
            }

            var commentDict = isUser ? _userComments : _compComments;
            if ((commentDict.ContainsKey(policy.UniqueId) && !string.IsNullOrEmpty(commentDict[policy.UniqueId])) !=
                _currentFilter.Commented.Value)
            {
                return false;
            }

            return _currentFilter.AllowedProducts is null || PolicyProcessing.IsPolicySupported(policy,
                _currentFilter.AllowedProducts, _currentFilter.AlwaysMatchAny, _currentFilter.MatchBlankSupport);
        }

        public bool PolicyVisibleInSection(PolicyPlusPolicy policy, AdmxPolicySection section) =>
            // Does this policy apply to the given section?
            (int)(policy.RawPolicy.Section & section) > 0;

        public PolFile GetOrCreatePolFromPolicySource(IPolicySource source)
        {
            switch (source)
            {
                case PolFile file:
                    // If it's already a POL, just save it
                    return file;

                case RegistryPolicyProxy proxy:
                    {
                        // Recurse through the Registry branch and create a POL
                        var regRoot = proxy.EncapsulatedRegistry;
                        var pol = new PolFile();

                        foreach (var policyPath in RegistryPolicyProxy.PolicyKeys)
                        {
                            using var policyKey = regRoot.OpenSubKey(policyPath, false);
                            AddSubtree(pol, policyPath, policyKey!);
                        }

                        return pol;
                    }
                default:
                    throw new InvalidOperationException("Policy source type not supported");
            }
        }

        private static void AddSubtree(IPolicySource pol, string pathRoot, RegistryKey key)
        {
            foreach (var valName in key.GetValueNames())
            {
                var valData = key.GetValue(valName, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                pol.SetValue(pathRoot, valName, valData!, key.GetValueKind(valName));
            }

            foreach (var subkeyName in key.GetSubKeyNames())
            {
                using var subkey = key.OpenSubKey(subkeyName, false);
                AddSubtree(pol, pathRoot + @"\" + subkeyName, subkey!);
            }
        }

        public DialogResult DisplayAdmxLoadErrorReport(IEnumerable<AdmxLoadFailure> failures, bool askContinue = false)
        {
            if (!failures.Any())
            {
                return DialogResult.OK;
            }

            const string header = "Errors were encountered while adding administrative templates to the workspace.";

            if (askContinue)
            {
                return MessageBox.Show($"{header}{Environment.NewLine}{Environment.NewLine}Continue trying to use this workspace?{string.Join(Environment.NewLine + Environment.NewLine, failures.Select(f => f.ToString()))}",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation);
            }

            return MessageBox.Show(header, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public string GetPreferredLanguageCode() =>
            _configuration.GetValue("LanguageCode", CultureInfo.CurrentCulture.Name).ToString() ?? throw new Exception("Incorrect LanguageCode");

        private void CategoriesTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // When the user selects a new category in the left pane
            _currentCategory = (PolicyPlusCategory)e.Node.Tag;
            UpdateCategoryListing();
            ClearSelections();
            UpdatePolicyInfo();
        }

        private void ResizePolicyNameColumn(object sender, EventArgs e)
        {
            // Fit the policy name column to the window size
            if (IsHandleCreated)
            {
                _ = BeginInvoke(() => PoliciesList.Columns[0].Width = PoliciesList.ClientSize.Width -
                                                                  (PoliciesList.Columns[1].Width +
                                                                   PoliciesList.Columns[2].Width));
            }
        }

        private void PoliciesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When the user highlights an item in the right pane
            if (PoliciesList.SelectedItems.Count > 0)
            {
                var selObject = PoliciesList.SelectedItems[0].Tag;
                if (selObject is PolicyPlusPolicy policy)
                {
                    _currentSetting = policy;
                    _highlightCategory = null;
                }
                else if (selObject is PolicyPlusCategory category)
                {
                    _highlightCategory = category;
                    _currentSetting = null;
                }
            }
            else
            {
                ClearSelections();
            }

            UpdatePolicyInfo();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void OpenADMXFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Open ADMX Folder dialog and load the policy definitions

            var dialog = new OpenAdmxFolder();
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                if (dialog.ClearWorkspace)
                {
                    ClearAdmxWorkspace();
                }

                _ = DisplayAdmxLoadErrorReport(_admxWorkspace.LoadFolder(dialog.SelectedFolder,
                    GetPreferredLanguageCode()));
                // Only update the last source when successfully opening a complete source
                if (dialog.ClearWorkspace)
                {
                    _configuration.SetValue("AdmxSource", dialog.SelectedFolder);
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"The folder could not be fully added to the workspace. {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            PopulateAdmxUi();
        }

        private void OpenADMXFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open a single ADMX file
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Policy definitions files|*.admx";
            ofd.Title = "Open ADMX file";
            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                _ = DisplayAdmxLoadErrorReport(_admxWorkspace.LoadFile(ofd.FileName, GetPreferredLanguageCode()));
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"The ADMX file could not be added to the workspace. {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            PopulateAdmxUi();
        }

        private void CloseADMXWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close all policy definitions and clear the workspace
            ClearAdmxWorkspace();
            PopulateAdmxUi();
        }

        private void EmptyCategoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle whether empty categories are visible
            _viewEmptyCategories = !_viewEmptyCategories;
            EmptyCategoriesToolStripMenuItem.Checked = _viewEmptyCategories;
            MoveToVisibleCategoryAndReload();
        }

        private void ComboAppliesTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When the user chooses a different section to work with
            _viewPolicyTypes = (ComboAppliesTo.Text ?? "") switch
            {
                "User" => AdmxPolicySection.User,
                "Computer" => AdmxPolicySection.Machine,
                _ => AdmxPolicySection.Both
            };

            MoveToVisibleCategoryAndReload();
        }

        private void PoliciesList_DoubleClick(object sender, EventArgs e)
        {
            // When the user opens a policy object in the right pane
            if (PoliciesList.SelectedItems.Count == 0)
            {
                return;
            }

            var policyItem = PoliciesList.SelectedItems[0].Tag;
            if (policyItem is PolicyPlusCategory item)
            {
                _currentCategory = item;
                UpdateCategoryListing();
            }
            else if (policyItem is PolicyPlusPolicy policy)
            {
                ShowSettingEditor(policy, _viewPolicyTypes);
            }
        }

        private void DeduplicatePoliciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make otherwise-identical pairs of user and computer policies into single dual-section policies
            ClearSelections();
            var deduped = PolicyProcessing.DeduplicatePolicies(_admxWorkspace);
            _ = MessageBox.Show("Deduplicated " + deduped + " policies.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateCategoryListing();
            UpdatePolicyInfo();
        }

        private void FindByIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Find By ID window and try to move to the selected object
            var findByIdWindow = new FindById
            {
                AdmxWorkspace = _admxWorkspace
            };
            if (findByIdWindow.ShowDialog(this) == DialogResult.OK)
            {
                var selCat = findByIdWindow.SelectedCategory;
                var selPol = findByIdWindow.SelectedPolicy;
                var selPro = findByIdWindow.SelectedProduct;
                var selSup = findByIdWindow.SelectedSupport;
                if (selCat is not null)
                {
                    if (_categoryNodes.ContainsKey(selCat))
                    {
                        _currentCategory = selCat;
                        UpdateCategoryListing();
                    }
                    else
                    {
                        _ = MessageBox.Show("The category is not currently visible. Change the view settings and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if (selPol is not null)
                {
                    ShowSettingEditor(selPol,
                        (AdmxPolicySection)Math.Min((int)_viewPolicyTypes,
                            (int)findByIdWindow.SelectedSection));
                    FocusPolicy(selPol);
                }
                else if (selPro is not null)
                {
                    var dialog = new DetailProduct();
                    dialog.PresentDialog(selPro);
                }
                else if (selSup is not null)
                {
                    var dialog = new DetailSupport();
                    dialog.PresentDialog(selSup);
                }
                else
                {
                    _ = MessageBox.Show("That object could not be found.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void OpenPolicyResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Open Policy Resources dialog and open its loaders
            var dialog = new OpenPol();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                OpenPolicyLoaders(dialog.SelectedUser, dialog.SelectedComputer,
                    false);
                MoveToVisibleCategoryAndReload();
            }
        }

        private void SavePoliciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save policy state and comments to disk Doesn't matter, it's just comments
            SaveComments(_userComments!, _userPolicyLoader);
            SaveComments(_compComments!, _compPolicyLoader);
            try
            {
                var compStatus = "not writable";
                var userStatus = "not writable";
                if (_compPolicyLoader.GetWritability() == PolicySourceWritability.Writable)
                {
                    compStatus = _compPolicyLoader.Save();
                }

                if (_userPolicyLoader.GetWritability() == PolicySourceWritability.Writable)
                {
                    userStatus = _userPolicyLoader.Save();
                }

                _configuration.SetValue("CompSourceType", (int)_compPolicyLoader.Source);
                _configuration.SetValue("UserSourceType", (int)_userPolicyLoader.Source);
                _configuration.SetValue("CompSourceData", _compPolicyLoader.LoaderData ?? "");
                _configuration.SetValue("UserSourceData", _userPolicyLoader.LoaderData ?? "");

                _ = MessageBox.Show($"Success.{Environment.NewLine}{Environment.NewLine}User policies: {userStatus}.{Environment.NewLine}{Environment.NewLine}Computer policies: {compStatus}.",
    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Saving failed!{Environment.NewLine}{Environment.NewLine}{ex.Message}",
    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static void SaveComments(Dictionary<string, string> comments, PolicyLoader loader)
        {
            try
            {
                if (comments is not null)
                {
                    CmtxFile.FromCommentTable(comments).Save(loader.GetCmtxPath());
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show author and version information if it was compiled into the program
            var about =
                $"Policy Plus by Ben Nordick.{Environment.NewLine}{Environment.NewLine}Available on GitHub: Fleex255/PolicyPlus." +
                Environment.NewLine +
                "C# version by Zafer Balkan. Available on Github: zbalkan/PolicyPlus" +
                Environment.NewLine;

            if (!string.IsNullOrEmpty(VersionHolder.Version.Trim()))
            {
                about += $" Version: {VersionHolder.Version.Trim()}.";
            }

            _ = MessageBox.Show(about, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ByTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Find By Text window and start the search
            var dialog = new FindByText();
            if (dialog.PresentDialog(_userComments!, _compComments!) == DialogResult.OK)
            {
                ShowSearchDialog(dialog.Searcher);
            }
        }

        private void SearchResultsToolStripMenuItem_Click(object sender, EventArgs e) =>
            // Show the search results window but don't start a search
            ShowSearchDialog(null);

        private void FindNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new FindResults();
            // Move to the next policy in the search results
            while (true)
            {
                var nextPol = dialog.NextPolicy();
                if (nextPol is null)
                {
                    _ = MessageBox.Show("There are no more results that match the filter.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }

                if (!ShouldShowPolicy(nextPol))
                {
                    continue;
                }

                FocusPolicy(nextPol);
                break;
            }
        }

        private void ByRegistryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Find By Registry window and start the search
            var dialog = new FindByRegistry();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                ShowSearchDialog(dialog.Searcher);
            }
        }

        private void SettingInfoPanel_ClientSizeChanged(object? sender, EventArgs? e)
        {
            // Finagle the middle pane's UI elements
            SettingInfoPanel.AutoScrollMinSize = SettingInfoPanel.Size;
            PolicyTitleLabel.MaximumSize = new Size(PolicyInfoTable.Width, 0);
            PolicySupportedLabel.MaximumSize = new Size(PolicyInfoTable.Width, 0);
            PolicyDescLabel.MaximumSize = new Size(PolicyInfoTable.Width, 0);
            PolicyIsPrefLabel.MaximumSize =
                new Size(PolicyInfoTable.Width - 22, 0); // Leave room for the exclamation icon
            PolicyInfoTable.MaximumSize =
                new Size(
                    SettingInfoPanel.Width - (SettingInfoPanel.VerticalScroll.Visible
                        ? SystemInformation.VerticalScrollBarWidth
                        : 0), 0);
            PolicyInfoTable.Width = PolicyInfoTable.MaximumSize.Width;
            if (PolicyInfoTable.ColumnCount > 0)
            {
                PolicyInfoTable.ColumnStyles[0].Width =
                    PolicyInfoTable.ClientSize.Width; // Only once everything is initialized
            }

            PolicyInfoTable.PerformLayout(); // Force the table to take up its full desired size
            _ = PInvoke.ShowScrollBar(SettingInfoPanel.Handle, 0, false); // 0 means horizontal
        }

        private void Main_Closed(object sender, EventArgs e) =>
            ClosePolicySources(); // Make sure everything is cleaned up before quitting

        private void PoliciesList_KeyDown(object sender, KeyEventArgs e)
        {
            // Activate a right pane item if the user presses Enter on it
            if (e.KeyCode == Keys.Enter && PoliciesList.SelectedItems.Count > 0)
            {
                PoliciesList_DoubleClick(sender, e);
            }
        }

        private void FilterOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new FilterOptions();
            // Show the Filter Options dialog and refresh if the filter changes
            if (dialog.PresentDialog(_currentFilter!, _admxWorkspace) != DialogResult.OK)
            {
                return;
            }

            _currentFilter = dialog.CurrentFilter;
            _viewFilteredOnly = true;
            OnlyFilteredObjectsToolStripMenuItem.Checked = true;
            MoveToVisibleCategoryAndReload();
        }

        private void OnlyFilteredObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle whether the filter is used
            _viewFilteredOnly = !_viewFilteredOnly;
            OnlyFilteredObjectsToolStripMenuItem.Checked = _viewFilteredOnly;
            MoveToVisibleCategoryAndReload();
        }

        private void ImportSemanticPolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new ImportSpol();
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            // Open the SPOL import dialog and apply the data
            var spol = dialog.Spol ?? throw new Exception("SPOL cannot be null.");
            var fails = spol.ApplyAll(_admxWorkspace, _userPolicySource, _compPolicySource, _userComments!,
                _compComments!);
            MoveToVisibleCategoryAndReload();
            if (fails == 0)
            {
                _ = MessageBox.Show("Semantic Policy successfully applied.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _ = MessageBox.Show($"{fails} out of {spol.Policies.Count} could not be applied.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ImportPOLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open a POL file and write it to a policy source
            using var ofd = new OpenFileDialog();
            ofd.Filter = "POL files|*.pol";
            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            PolFile pol;
            try
            {
                pol = PolFile.Load(ofd.FileName);
            }
            catch (Exception)
            {
                _ = MessageBox.Show("The POL file could not be loaded.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var dialog = new OpenSection();
            if (dialog.PresentDialog(true, true) != DialogResult.OK)
            {
                return;
            }

            var section = dialog.SelectedSection == AdmxPolicySection.User
                ? _userPolicySource
                : _compPolicySource;
            pol.Apply(section);
            MoveToVisibleCategoryAndReload();
            _ = MessageBox.Show("POL import successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportPOLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a POL file from a current policy source
            using var sfd = new SaveFileDialog();
            sfd.Filter = "POL files|*.pol";

            var dialog = new OpenSection();

            if (sfd.ShowDialog(this) != DialogResult.OK ||
                dialog.PresentDialog(true, true) != DialogResult.OK)
            {
                return;
            }

            var section = dialog.SelectedSection == AdmxPolicySection.Machine
                ? _compPolicySource
                : _userPolicySource;
            try
            {
                GetOrCreatePolFromPolicySource(section).Save(sfd.FileName);
                _ = MessageBox.Show("POL exported successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                _ = MessageBox.Show("The POL file could not be saved.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AcquireADMXFilesToolStripMenuItem_Click(object? sender, EventArgs? e)
        {
            // Show the Acquire ADMX Files dialog and load the new ADMX files
            var dialog = new DownloadAdmx();
            if (dialog.ShowDialog(this) != DialogResult.OK ||
                string.IsNullOrEmpty(dialog.NewPolicySourceFolder))
            {
                return;
            }

            ClearAdmxWorkspace();
            _ = DisplayAdmxLoadErrorReport(_admxWorkspace.LoadFolder(dialog.NewPolicySourceFolder,
                GetPreferredLanguageCode()));
            _configuration.SetValue("AdmxSource", dialog.NewPolicySourceFolder);
            PopulateAdmxUi();
        }

        private void LoadedADMXFilesToolStripMenuItem_Click(object sender, EventArgs e) =>
            new LoadedAdmx().PresentDialog(_admxWorkspace);

        private void AllSupportDefinitionsToolStripMenuItem_Click(object sender, EventArgs e) =>
            new LoadedSupportDefinitions().PresentDialog(_admxWorkspace);

        private void AllProductsToolStripMenuItem_Click(object sender, EventArgs e) =>
            new LoadedProducts().PresentDialog(_admxWorkspace);

        private void EditRawPOLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var userIsPol = _userPolicySource is PolFile;
            var compIsPol = _compPolicySource is PolFile;
            if (!(userIsPol || compIsPol))
            {
                _ = MessageBox.Show("Neither loaded source is backed by a POL file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if ((int)_configuration.GetValue("EditPolDangerAcknowledged", 0)! == 0)
            {
                if (MessageBox.Show($"Caution! This tool is for very advanced users. Improper modifications may result in inconsistencies in policies' states.{Environment.NewLine}{Environment.NewLine}Changes operate directly on the policy source, though they will not be committed to disk until you save. Are you sure you want to continue?",
    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    return;
                }

                _configuration.SetValue("EditPolDangerAcknowledged", 1);
            }
            var dialog = new OpenSection();

            if (dialog.PresentDialog(userIsPol, compIsPol) == DialogResult.OK)
            {
                new EditPol().PresentDialog(PolicyIcons,
                    (PolFile)(dialog.SelectedSection == AdmxPolicySection.Machine
                        ? _compPolicySource
                        : _userPolicySource), dialog.SelectedSection == AdmxPolicySection.User);
            }

            MoveToVisibleCategoryAndReload();
        }

        private void ExportREGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenSection();

            if (dialog.PresentDialog(true, true) != DialogResult.OK)
            {
                return;
            }

            var source = dialog.SelectedSection == AdmxPolicySection.Machine
                ? _compPolicySource
                : _userPolicySource;

            new ExportReg().PresentDialog("", GetOrCreatePolFromPolicySource(source),
                dialog.SelectedSection == AdmxPolicySection.User);
        }

        private void ImportREGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenSection();

            if (dialog.PresentDialog(true, true) != DialogResult.OK)
            {
                return;
            }

            var source = dialog.SelectedSection == AdmxPolicySection.Machine
                ? _compPolicySource
                : _userPolicySource;

            if (new ImportReg().PresentDialog(source) == DialogResult.OK)
            {
                MoveToVisibleCategoryAndReload();
            }
        }

        private void SetADMLLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new LanguageOptions();
            if (dialog.PresentDialog(GetPreferredLanguageCode()) != DialogResult.OK)
            {
                return;
            }

            _configuration.SetValue("LanguageCode", dialog.NewLanguage);

            if (MessageBox.Show("Language changes will take effect when ADML files are next loaded. Would you like to reload the workspace now?",
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ClearAdmxWorkspace();
            OpenLastAdmxSource();
            PopulateAdmxUi();
        }

        private void PolicyObjectContext_Opening(object sender, CancelEventArgs e)
        {
            // When the right-click menu is opened
            bool showingForCategory;
            if (ReferenceEquals(PolicyObjectContext.SourceControl, CategoriesTree))
            {
                showingForCategory = true;
                PolicyObjectContext.Tag = CategoriesTree.SelectedNode.Tag;
            }
            else if (PoliciesList.SelectedItems.Count > 0) // Shown from the main view
            {
                var selEntryTag = PoliciesList.SelectedItems[0].Tag;
                showingForCategory = selEntryTag is PolicyPlusCategory;
                PolicyObjectContext.Tag = selEntryTag;
            }
            else
            {
                e.Cancel = true;
                return;
            }

            // Items are tagged in the designer for the objects they apply to
            foreach (var item in PolicyObjectContext.Items.OfType<ToolStripMenuItem>())
            {
                var ok = !(item.Tag.ToString() == "P" && showingForCategory);

                if (item.Tag.ToString() == "C" && !showingForCategory)
                {
                    ok = false;
                }

                item.Visible = ok;
            }
        }

        private void PolicyObjectContext_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // When the user clicks an item in the right-click menu
            var polObject = PolicyObjectContext.Tag; // The current policy object is in the Tag field
            if (ReferenceEquals(e.ClickedItem, CmeCatOpen))
            {
                _currentCategory = (PolicyPlusCategory)polObject;
                UpdateCategoryListing();
            }
            else if (ReferenceEquals(e.ClickedItem, CmePolEdit))
            {
                ShowSettingEditor((PolicyPlusPolicy)polObject, _viewPolicyTypes);
            }
            else if (ReferenceEquals(e.ClickedItem, CmeAllDetails))
            {
                if (polObject is PolicyPlusCategory category)
                {
                    new DetailCategory().PresentDialog(category);
                }
                else
                {
                    new DetailPolicy().PresentDialog((PolicyPlusPolicy)polObject);
                }
            }
            else if (ReferenceEquals(e.ClickedItem, CmePolInspectElements))
            {
                new InspectPolicyElements().PresentDialog((PolicyPlusPolicy)polObject, PolicyIcons,
                    _admxWorkspace);
            }
            else if (ReferenceEquals(e.ClickedItem, CmePolSpolFragment))
            {
                new InspectSpolFragment().PresentDialog((PolicyPlusPolicy)polObject, _compPolicySource,
                    _userPolicySource, _compComments!, _userComments!);
            }
        }

        private void CategoriesTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Right-clicking doesn't actually select the node by default
            if (e.Button == MouseButtons.Right)
            {
                CategoriesTree.SelectedNode = e.Node;
            }
        }

        public static string PrettifyDescription(string description)
        {
            // Remove extra indentation from paragraphs
            var sb = new StringBuilder();
            foreach (var line in description.Split(Environment.NewLine))
            {
                _ = sb.AppendLine(line.Trim());
            }

            return sb.ToString().TrimEnd();
        }
    }
}