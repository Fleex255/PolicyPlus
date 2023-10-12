using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace PolicyPlus
{
    public partial class Main
    {
        private ConfigurationStorage Configuration;
        private AdmxBundle AdmxWorkspace = new AdmxBundle();
        private IPolicySource UserPolicySource, CompPolicySource;
        private PolicyLoader UserPolicyLoader, CompPolicyLoader;
        private Dictionary<string, string> UserComments, CompComments;
        private PolicyPlusCategory CurrentCategory;
        private PolicyPlusPolicy CurrentSetting;
        private FilterConfiguration CurrentFilter = new FilterConfiguration();
        private PolicyPlusCategory HighlightCategory;
        private Dictionary<PolicyPlusCategory, TreeNode> CategoryNodes = new Dictionary<PolicyPlusCategory, TreeNode>();
        private bool ViewEmptyCategories = false;
        private AdmxPolicySection ViewPolicyTypes = AdmxPolicySection.Both;
        private bool ViewFilteredOnly = false;

        public Main()
        {
            InitializeComponent();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            // Create the configuration manager (for the Registry)
            Configuration = new ConfigurationStorage(RegistryHive.CurrentUser, @"Software\Policy Plus");
            // Restore the last ADMX source and policy loaders
            OpenLastAdmxSource();
            PolicyLoaderSource compLoaderType = (PolicyLoaderSource)Conversions.ToInteger(Configuration.GetValue("CompSourceType", 0));
            var compLoaderData = Configuration.GetValue("CompSourceData", "");
            PolicyLoaderSource userLoaderType = (PolicyLoaderSource)Conversions.ToInteger(Configuration.GetValue("UserSourceType", 0));
            var userLoaderData = Configuration.GetValue("UserSourceData", "");
            try
            {
                OpenPolicyLoaders(new PolicyLoader(userLoaderType, Conversions.ToString(userLoaderData), true), new PolicyLoader(compLoaderType, Conversions.ToString(compLoaderData), false), true);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("The previous policy sources are not accessible. The defaults will be loaded.", MsgBoxStyle.Exclamation);
                Configuration.SetValue("CompSourceType", (int)PolicyLoaderSource.LocalGpo);
                Configuration.SetValue("UserSourceType", (int)PolicyLoaderSource.LocalGpo);
                OpenPolicyLoaders(new PolicyLoader(PolicyLoaderSource.LocalGpo, "", true), new PolicyLoader(PolicyLoaderSource.LocalGpo, "", false), true);
            }
            My.MyProject.Forms.OpenPol.SetLastSources(compLoaderType, Conversions.ToString(compLoaderData), userLoaderType, Conversions.ToString(userLoaderData));
            // Set up the UI
            ComboAppliesTo.Text = Conversions.ToString(ComboAppliesTo.Items[0]);
            CategoriesTree.Height -= InfoStrip.ClientSize.Height;
            SettingInfoPanel.Height -= InfoStrip.ClientSize.Height;
            PoliciesList.Height -= InfoStrip.ClientSize.Height;
            InfoStrip.Items.Insert(2, new ToolStripSeparator());
            PopulateAdmxUi();
        }
        private void Main_Shown(object sender, EventArgs e)
        {
            // Check whether ADMX files probably need to be downloaded
            if (Conversions.ToInteger(Configuration.GetValue("CheckedPolicyDefinitions", 0)) == 0)
            {
                Configuration.SetValue("CheckedPolicyDefinitions", 1);
                if (!SystemInfo.HasGroupPolicyInfrastructure() && AdmxWorkspace.Categories.Values.Where(c => IsOrphanCategory(c) & !IsEmptyCategory(c)).Count() > 2)
                {
                    if (Interaction.MsgBox($"Welcome to Policy Plus!{Constants.vbCrLf}{Constants.vbCrLf}Home editions do not come with the full set of policy definitions. Would you like to download them now? " + "This can also be done later with Help | Acquire ADMX Files.", MsgBoxStyle.Information | MsgBoxStyle.YesNo) == MsgBoxResult.Yes)
                    {
                        AcquireADMXFilesToolStripMenuItem_Click(null, null);
                    }
                }
            }
        }
        public void OpenLastAdmxSource()
        {
            string defaultAdmxSource = Environment.ExpandEnvironmentVariables(@"%windir%\PolicyDefinitions");
            string admxSource = Conversions.ToString(Configuration.GetValue("AdmxSource", defaultAdmxSource));
            try
            {
                var fails = AdmxWorkspace.LoadFolder(admxSource, GetPreferredLanguageCode());
                if (DisplayAdmxLoadErrorReport(fails, true) == MsgBoxResult.No)
                    throw new Exception("You decided to not use the problematic ADMX bundle.");
            }
            catch (Exception ex)
            {
                AdmxWorkspace = new AdmxBundle();
                string loadFailReason = "";
                if ((admxSource ?? "") != (defaultAdmxSource ?? ""))
                {
                    if (Interaction.MsgBox("Policy definitions could not be loaded from \"" + admxSource + "\": " + ex.Message + Constants.vbCrLf + Constants.vbCrLf + "Load from the default location?", MsgBoxStyle.YesNo | MsgBoxStyle.Question) == MsgBoxResult.Yes)
                    {
                        try
                        {
                            Configuration.SetValue("AdmxSource", defaultAdmxSource);
                            AdmxWorkspace = new AdmxBundle();
                            DisplayAdmxLoadErrorReport(AdmxWorkspace.LoadFolder(defaultAdmxSource, GetPreferredLanguageCode()));
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
                    Interaction.MsgBox("Policy definitions could not be loaded: " + loadFailReason, MsgBoxStyle.Exclamation);
            }
        }
        public void PopulateAdmxUi()
        {
            // Populate the left categories tree
            CategoriesTree.Nodes.Clear();
            CategoryNodes.Clear();
            Action<IEnumerable<PolicyPlusCategory>, TreeNodeCollection> addCategory;
            addCategory = new Action<IEnumerable<PolicyPlusCategory>, TreeNodeCollection>((CategoryList, ParentNode) => { foreach (var category in CategoryList.Where(ShouldShowCategory)) { var newNode = ParentNode.Add(category.UniqueID, category.DisplayName, GetImageIndexForCategory(category)); newNode.SelectedImageIndex = 3; newNode.Tag = category; CategoryNodes.Add(category, newNode); addCategory(category.Children, newNode.Nodes); } }); // "Go" arrow
            addCategory(AdmxWorkspace.Categories.Values, CategoriesTree.Nodes);
            CategoriesTree.Sort();
            CurrentCategory = null;
            UpdateCategoryListing();
            ClearSelections();
            UpdatePolicyInfo();
        }
        public void UpdateCategoryListing()
        {
            // Update the right pane to include the current category's children
            var topItemIndex = default(int?);
            if (PoliciesList.TopItem is not null)
                topItemIndex = PoliciesList.TopItem.Index;
            bool inSameCategory = false;
            PoliciesList.Items.Clear();
            if (CurrentCategory is not null)
            {
                if (CurrentSetting is not null && ReferenceEquals(CurrentSetting.Category, CurrentCategory))
                    inSameCategory = true;
                if (CurrentCategory.Parent is not null) // Add the parent
                {
                    var listItem = PoliciesList.Items.Add("Up: " + CurrentCategory.Parent.DisplayName);
                    listItem.Tag = CurrentCategory.Parent;
                    listItem.ImageIndex = 6; // Up arrow
                    listItem.SubItems.Add("Parent");
                }
                foreach (var category in CurrentCategory.Children.Where(ShouldShowCategory).OrderBy(c => c.DisplayName)) // Add subcategories
                {
                    var listItem = PoliciesList.Items.Add(category.DisplayName);
                    listItem.Tag = category;
                    listItem.ImageIndex = GetImageIndexForCategory(category);
                }
                foreach (var policy in CurrentCategory.Policies.Where(ShouldShowPolicy).OrderBy(p => p.DisplayName)) // Add policies
                {
                    var listItem = PoliciesList.Items.Add(policy.DisplayName);
                    listItem.Tag = policy;
                    listItem.ImageIndex = GetImageIndexForSetting(policy);
                    listItem.SubItems.Add(GetPolicyState(policy));
                    listItem.SubItems.Add(GetPolicyCommentText(policy));
                    if (ReferenceEquals(policy, CurrentSetting)) // Keep the current policy selected
                    {
                        listItem.Selected = true;
                        listItem.Focused = true;
                        listItem.EnsureVisible();
                    }
                }
                if (topItemIndex.HasValue & inSameCategory) // Minimize the list view's jumping around when refreshing
                {
                    if (PoliciesList.Items.Count > topItemIndex.Value)
                        PoliciesList.TopItem = PoliciesList.Items[topItemIndex.Value];
                }
                if (CategoriesTree.SelectedNode is null || !ReferenceEquals(CategoriesTree.SelectedNode.Tag, CurrentCategory)) // Update the tree view
                {
                    CategoriesTree.SelectedNode = CategoryNodes[CurrentCategory];
                }
            }
        }
        public void UpdatePolicyInfo()
        {
            // Update the middle pane with the selected object's information
            bool hasCurrentSetting = CurrentSetting is not null | HighlightCategory is not null | CurrentCategory is not null;
            PolicyTitleLabel.Visible = hasCurrentSetting;
            PolicySupportedLabel.Visible = hasCurrentSetting;
            if (CurrentSetting is not null)
            {
                PolicyTitleLabel.Text = CurrentSetting.DisplayName;
                if (CurrentSetting.SupportedOn is null)
                {
                    PolicySupportedLabel.Text = "(no requirements information)";
                }
                else
                {
                    PolicySupportedLabel.Text = "Requirements:" + Constants.vbCrLf + CurrentSetting.SupportedOn.DisplayName;
                }
                PolicyDescLabel.Text = PrettifyDescription(CurrentSetting.DisplayExplanation);
                PolicyIsPrefTable.Visible = IsPreference(CurrentSetting);
            }
            else if (HighlightCategory is not null | CurrentCategory is not null)
            {
                var shownCategory = HighlightCategory ?? CurrentCategory;
                PolicyTitleLabel.Text = shownCategory.DisplayName;
                PolicySupportedLabel.Text = (HighlightCategory is null ? "This" : "The selected") + " category contains " + shownCategory.Policies.Count + " policies and " + shownCategory.Children.Count + " subcategories.";
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
        public bool IsOrphanCategory(PolicyPlusCategory Category)
        {
            return Category.Parent is null & !string.IsNullOrEmpty(Category.RawCategory.ParentID);
        }
        public bool IsEmptyCategory(PolicyPlusCategory Category)
        {
            return Category.Children.Count == 0 & Category.Policies.Count == 0;
        }
        public int GetImageIndexForCategory(PolicyPlusCategory Category)
        {
            if (IsOrphanCategory(Category))
            {
                return 1; // Orphaned
            }
            else if (IsEmptyCategory(Category))
            {
                return 2; // Empty
            }
            else
            {
                return 0;
            } // Normal
        }
        public int GetImageIndexForSetting(PolicyPlusPolicy Setting)
        {
            if (IsPreference(Setting))
            {
                return 7; // Preference, not policy (exclamation mark)
            }
            else if (Setting.RawPolicy.Elements is null || Setting.RawPolicy.Elements.Count == 0)
            {
                return 4; // Normal
            }
            else
            {
                return 5;
            } // Extra configuration
        }
        public bool ShouldShowCategory(PolicyPlusCategory Category)
        {
            // Should this category be shown considering the current filter?
            if (ViewEmptyCategories)
            {
                return true;
            }
            else // Only if it has visible children
            {
                return Category.Policies.Any(ShouldShowPolicy) || Category.Children.Any(ShouldShowCategory);
            }
        }
        public bool ShouldShowPolicy(PolicyPlusPolicy Policy)
        {
            // Should this policy be shown considering the current filter and active sections?
            if (!PolicyVisibleInSection(Policy, ViewPolicyTypes))
                return false;
            if (ViewFilteredOnly)
            {
                bool visibleAfterFilter = false;
                if ((int)(ViewPolicyTypes & AdmxPolicySection.Machine) > 0 & PolicyVisibleInSection(Policy, AdmxPolicySection.Machine))
                {
                    if (IsPolicyVisibleAfterFilter(Policy, false))
                        visibleAfterFilter = true;
                }
                else if ((int)(ViewPolicyTypes & AdmxPolicySection.User) > 0 & PolicyVisibleInSection(Policy, AdmxPolicySection.User))
                {
                    if (IsPolicyVisibleAfterFilter(Policy, true))
                        visibleAfterFilter = true;
                }
                if (!visibleAfterFilter)
                    return false;
            }
            return true;
        }
        public void MoveToVisibleCategoryAndReload()
        {
            // Move up in the categories tree until a visible one is found
            var newFocusCategory = CurrentCategory;
            var newFocusPolicy = CurrentSetting;
            while (!(newFocusCategory is null) && !ShouldShowCategory(newFocusCategory))
            {
                newFocusCategory = newFocusCategory.Parent;
                newFocusPolicy = null;
            }
            if (newFocusPolicy is not null && !ShouldShowPolicy(newFocusPolicy))
                newFocusPolicy = null;
            PopulateAdmxUi();
            CurrentCategory = newFocusCategory;
            UpdateCategoryListing();
            CurrentSetting = newFocusPolicy;
            UpdatePolicyInfo();
        }
        public string GetPolicyState(PolicyPlusPolicy Policy)
        {
            // Get a human-readable string describing the status of a policy, considering all active sections
            if (ViewPolicyTypes == AdmxPolicySection.Both)
            {
                string userState = GetPolicyState(Policy, AdmxPolicySection.User);
                string machState = GetPolicyState(Policy, AdmxPolicySection.Machine);
                var section = Policy.RawPolicy.Section;
                if (section == AdmxPolicySection.Both)
                {
                    if ((userState ?? "") == (machState ?? ""))
                    {
                        return userState + " (2)";
                    }
                    else if (userState == "Not Configured")
                    {
                        return machState + " (C)";
                    }
                    else if (machState == "Not Configured")
                    {
                        return userState + " (U)";
                    }
                    else
                    {
                        return "Mixed";
                    }
                }
                else if (section == AdmxPolicySection.Machine)
                    return machState + " (C)";
                else
                    return userState + " (U)";
            }
            else
            {
                return GetPolicyState(Policy, ViewPolicyTypes);
            }
        }
        public string GetPolicyState(PolicyPlusPolicy Policy, AdmxPolicySection Section)
        {
            // Get the human-readable status of a policy considering only one section
            switch (PolicyProcessing.GetPolicyState(Section == AdmxPolicySection.Machine ? CompPolicySource : UserPolicySource, Policy))
            {
                case PolicyState.Disabled:
                    {
                        return "Disabled";
                    }
                case PolicyState.Enabled:
                    {
                        return "Enabled";
                    }
                case PolicyState.NotConfigured:
                    {
                        return "Not Configured";
                    }

                default:
                    {
                        return "Unknown";
                    }
            }
        }
        public string GetPolicyCommentText(PolicyPlusPolicy Policy)
        {
            // Get the comment text to show in the Comment column, considering all active sections
            if (ViewPolicyTypes == AdmxPolicySection.Both)
            {
                string userComment = GetPolicyComment(Policy, AdmxPolicySection.User);
                string compComment = GetPolicyComment(Policy, AdmxPolicySection.Machine);
                if (string.IsNullOrEmpty(userComment) & string.IsNullOrEmpty(compComment))
                {
                    return "";
                }
                else if (!string.IsNullOrEmpty(userComment) & !string.IsNullOrEmpty(compComment))
                {
                    return "(multiple)";
                }
                else if (!string.IsNullOrEmpty(userComment))
                {
                    return userComment;
                }
                else
                {
                    return compComment;
                }
            }
            else
            {
                return GetPolicyComment(Policy, ViewPolicyTypes);
            }
        }
        public string GetPolicyComment(PolicyPlusPolicy Policy, AdmxPolicySection Section)
        {
            // Get a policy's comment in one section
            var commentSource = Section == AdmxPolicySection.Machine ? CompComments : UserComments;
            if (commentSource is null)
            {
                return "";
            }
            else if (commentSource.ContainsKey(Policy.UniqueID))
                return commentSource[Policy.UniqueID];
            else
                return "";
        }
        public bool IsPreference(PolicyPlusPolicy Policy)
        {
            return !string.IsNullOrEmpty(Policy.RawPolicy.RegistryKey) & !RegistryPolicyProxy.IsPolicyKey(Policy.RawPolicy.RegistryKey);
        }
        public void ShowSettingEditor(PolicyPlusPolicy Policy, AdmxPolicySection Section)
        {
            // Show the Edit Policy Setting dialog for a policy and reload if changes were made
            if (My.MyProject.Forms.EditSetting.PresentDialog(Policy, Section, AdmxWorkspace, CompPolicySource, UserPolicySource, CompPolicyLoader, UserPolicyLoader, CompComments, UserComments) == DialogResult.OK)
            {
                // Keep the selection where it is if possible
                if (CurrentCategory is null || ShouldShowCategory(CurrentCategory))
                    UpdateCategoryListing();
                else
                    MoveToVisibleCategoryAndReload();
            }
        }
        public void ClearSelections()
        {
            CurrentSetting = null;
            HighlightCategory = null;
        }
        public void OpenPolicyLoaders(PolicyLoader User, PolicyLoader Computer, bool Quiet)
        {
            // Create policy sources from the given loaders
            if (CompPolicyLoader is not null | UserPolicyLoader is not null)
                ClosePolicySources();
            UserPolicyLoader = User;
            UserPolicySource = User.OpenSource();
            CompPolicyLoader = Computer;
            CompPolicySource = Computer.OpenSource();
            bool allOk = true;
            string policyStatus(PolicyLoader Loader) { switch (Loader.GetWritability()) { case PolicySourceWritability.Writable: { return "is fully writable"; } case PolicySourceWritability.NoCommit: { allOk = false; return "cannot be saved"; } default: { allOk = false; return "cannot be modified"; } } }; // No writing
            Dictionary<string, string> loadComments(PolicyLoader Loader)
            {
                string cmtxPath = Loader.GetCmtxPath();
                if (string.IsNullOrEmpty(cmtxPath))
                {
                    return null;
                }
                else
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(cmtxPath));
                        if (System.IO.File.Exists(cmtxPath))
                        {
                            return CmtxFile.Load(cmtxPath).ToCommentTable();
                        }
                        else
                        {
                            return new Dictionary<string, string>();
                        }
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            };
            string userStatus = policyStatus(User);
            string compStatus = policyStatus(Computer);
            UserComments = loadComments(User);
            CompComments = loadComments(Computer);
            UserSourceLabel.Text = UserPolicyLoader.GetDisplayInfo();
            ComputerSourceLabel.Text = CompPolicyLoader.GetDisplayInfo();
            if (allOk)
            {
                if (!Quiet)
                {
                    Interaction.MsgBox("Both the user and computer policy sources are loaded and writable.", MsgBoxStyle.Information);
                }
            }
            else
            {
                string msgText = "Not all policy sources are fully writable." + Constants.vbCrLf + Constants.vbCrLf + "The user source " + userStatus + "." + Constants.vbCrLf + Constants.vbCrLf + "The computer source " + compStatus + ".";
                Interaction.MsgBox(msgText, MsgBoxStyle.Exclamation);
            }
        }
        public void ClosePolicySources()
        {
            // Clean up the policy sources
            bool allOk = true;
            if (UserPolicyLoader is not null)
            {
                if (!UserPolicyLoader.Close())
                    allOk = false;
            }
            if (CompPolicyLoader is not null)
            {
                if (!CompPolicyLoader.Close())
                    allOk = false;
            }
            if (!allOk)
            {
                Interaction.MsgBox("Cleanup did not complete fully because the loaded resources are open in other programs.", MsgBoxStyle.Exclamation);
            }
        }
        public void ShowSearchDialog(Func<PolicyPlusPolicy, bool> Searcher)
        {
            // Show the search dialog and make it start a search if appropriate
            DialogResult result;
            if (Searcher is null)
            {
                result = My.MyProject.Forms.FindResults.PresentDialog();
            }
            else
            {
                result = My.MyProject.Forms.FindResults.PresentDialogStartSearch(AdmxWorkspace, Searcher);
            }
            if (result == DialogResult.OK)
            {
                var selPol = My.MyProject.Forms.FindResults.SelectedPolicy;
                ShowSettingEditor(selPol, ViewPolicyTypes);
                FocusPolicy(selPol);
            }
        }
        public void ClearAdmxWorkspace()
        {
            // Clear out all the per-workspace bookkeeping
            AdmxWorkspace = new AdmxBundle();
            My.MyProject.Forms.FindResults.ClearSearch();
        }
        public void FocusPolicy(PolicyPlusPolicy Policy)
        {
            // Try to automatically select a policy in the list view
            if (CategoryNodes.ContainsKey(Policy.Category))
            {
                CurrentCategory = Policy.Category;
                UpdateCategoryListing();
                foreach (ListViewItem entry in PoliciesList.Items)
                {
                    if (ReferenceEquals(entry.Tag, Policy))
                    {
                        entry.Selected = true;
                        entry.Focused = true;
                        entry.EnsureVisible();
                        break;
                    }
                }
            }
        }
        public bool IsPolicyVisibleAfterFilter(PolicyPlusPolicy Policy, bool IsUser)
        {
            // Calculate whether a policy is visible with the current filter
            if (CurrentFilter.ManagedPolicy.HasValue)
            {
                if (IsPreference(Policy) == CurrentFilter.ManagedPolicy.Value)
                    return false;
            }
            if (CurrentFilter.PolicyState.HasValue)
            {
                var policyState = PolicyProcessing.GetPolicyState(IsUser ? UserPolicySource : CompPolicySource, Policy);
                switch (CurrentFilter.PolicyState.Value)
                {
                    case FilterPolicyState.Configured:
                        {
                            if (policyState == PolicyState.NotConfigured)
                                return false;
                            break;
                        }
                    case FilterPolicyState.NotConfigured:
                        {
                            if (policyState != PolicyState.NotConfigured)
                                return false;
                            break;
                        }
                    case FilterPolicyState.Disabled:
                        {
                            if (policyState != PolicyState.Disabled)
                                return false;
                            break;
                        }
                    case FilterPolicyState.Enabled:
                        {
                            if (policyState != PolicyState.Enabled)
                                return false;
                            break;
                        }
                }
            }
            if (CurrentFilter.Commented.HasValue)
            {
                var commentDict = IsUser ? UserComments : CompComments;
                if ((commentDict.ContainsKey(Policy.UniqueID) && !string.IsNullOrEmpty(commentDict[Policy.UniqueID])) != CurrentFilter.Commented.Value)
                    return false;
            }
            if (CurrentFilter.AllowedProducts is not null)
            {
                if (!PolicyProcessing.IsPolicySupported(Policy, CurrentFilter.AllowedProducts, CurrentFilter.AlwaysMatchAny, CurrentFilter.MatchBlankSupport))
                    return false;
            }
            return true;
        }
        public bool PolicyVisibleInSection(PolicyPlusPolicy Policy, AdmxPolicySection Section)
        {
            // Does this policy apply to the given section?
            return (int)(Policy.RawPolicy.Section & Section) > 0;
        }
        public PolFile GetOrCreatePolFromPolicySource(IPolicySource Source)
        {
            if (Source is PolFile)
            {
                // If it's already a POL, just save it
                return (PolFile)Source;
            }
            else if (Source is RegistryPolicyProxy)
            {
                // Recurse through the Registry branch and create a POL
                var regRoot = ((RegistryPolicyProxy)Source).EncapsulatedRegistry;
                var pol = new PolFile();
                Action<string, RegistryKey> addSubtree;
                addSubtree = new Action<string, RegistryKey>((PathRoot, Key) =>
                    {
                        foreach (var valName in Key.GetValueNames())
                        {
                            var valData = Key.GetValue(valName, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                            pol.SetValue(PathRoot, valName, valData, Key.GetValueKind(valName));
                        }
                        foreach (var subkeyName in Key.GetSubKeyNames())
                        {
                            using (var subkey = Key.OpenSubKey(subkeyName, false))
                            {
                                addSubtree(PathRoot + @"\" + subkeyName, subkey);
                            }
                        }
                    });
                foreach (var policyPath in RegistryPolicyProxy.PolicyKeys)
                {
                    using (var policyKey = regRoot.OpenSubKey(policyPath, false))
                    {
                        addSubtree(policyPath, policyKey);
                    }
                }
                return pol;
            }
            else
            {
                throw new InvalidOperationException("Policy source type not supported");
            }
        }
        public MsgBoxResult DisplayAdmxLoadErrorReport(IEnumerable<AdmxLoadFailure> Failures, bool AskContinue = false)
        {
            if (Failures.Count() == 0)
                return MsgBoxResult.Ok;
            var boxStyle = AskContinue ? MsgBoxStyle.Exclamation | MsgBoxStyle.YesNo : MsgBoxStyle.Exclamation;
            string header = "Errors were encountered while adding administrative templates to the workspace.";
            return Interaction.MsgBox(header + (AskContinue ? " Continue trying to use this workspace?" : "") + Constants.vbCrLf + Constants.vbCrLf + string.Join(Constants.vbCrLf + Constants.vbCrLf, Failures.Select(f => f.ToString())), boxStyle);
        }
        public string GetPreferredLanguageCode()
        {
            return Conversions.ToString(Configuration.GetValue("LanguageCode", System.Globalization.CultureInfo.CurrentCulture.Name));
        }
        private void CategoriesTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // When the user selects a new category in the left pane
            CurrentCategory = (PolicyPlusCategory)e.Node.Tag;
            UpdateCategoryListing();
            ClearSelections();
            UpdatePolicyInfo();
        }
        private void ResizePolicyNameColumn(object sender, EventArgs e)
        {
            // Fit the policy name column to the window size
            if (IsHandleCreated)
                BeginInvoke(new Action(() => PoliciesList.Columns[0].Width = PoliciesList.ClientSize.Width - (PoliciesList.Columns[1].Width + PoliciesList.Columns[2].Width)));
        }
        private void PoliciesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When the user highlights an item in the right pane
            if (PoliciesList.SelectedItems.Count > 0)
            {
                var selObject = PoliciesList.SelectedItems[0].Tag;
                if (selObject is PolicyPlusPolicy)
                {
                    CurrentSetting = (PolicyPlusPolicy)selObject;
                    HighlightCategory = null;
                }
                else if (selObject is PolicyPlusCategory)
                {
                    HighlightCategory = (PolicyPlusCategory)selObject;
                    CurrentSetting = null;
                }
            }
            else
            {
                ClearSelections();
            }
            UpdatePolicyInfo();
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void OpenADMXFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Open ADMX Folder dialog and load the policy definitions
            if (My.MyProject.Forms.OpenAdmxFolder.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (My.MyProject.Forms.OpenAdmxFolder.ClearWorkspace)
                        ClearAdmxWorkspace();
                    DisplayAdmxLoadErrorReport(AdmxWorkspace.LoadFolder(My.MyProject.Forms.OpenAdmxFolder.SelectedFolder, GetPreferredLanguageCode()));
                    // Only update the last source when successfully opening a complete source
                    if (My.MyProject.Forms.OpenAdmxFolder.ClearWorkspace)
                        Configuration.SetValue("AdmxSource", My.MyProject.Forms.OpenAdmxFolder.SelectedFolder);
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox("The folder could not be fully added to the workspace. " + ex.Message, MsgBoxStyle.Exclamation);
                }
                PopulateAdmxUi();
            }
        }
        private void OpenADMXFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open a single ADMX file
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Policy definitions files|*.admx";
                ofd.Title = "Open ADMX file";
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;
                try
                {
                    DisplayAdmxLoadErrorReport(AdmxWorkspace.LoadFile(ofd.FileName, GetPreferredLanguageCode()));
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox("The ADMX file could not be added to the workspace. " + ex.Message, MsgBoxStyle.Exclamation);
                }
                PopulateAdmxUi();
            }
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
            ViewEmptyCategories = !ViewEmptyCategories;
            EmptyCategoriesToolStripMenuItem.Checked = ViewEmptyCategories;
            MoveToVisibleCategoryAndReload();
        }
        private void ComboAppliesTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When the user chooses a different section to work with
            switch (ComboAppliesTo.Text ?? "")
            {
                case "User":
                    {
                        ViewPolicyTypes = AdmxPolicySection.User;
                        break;
                    }
                case "Computer":
                    {
                        ViewPolicyTypes = AdmxPolicySection.Machine;
                        break;
                    }

                default:
                    {
                        ViewPolicyTypes = AdmxPolicySection.Both;
                        break;
                    }
            }
            MoveToVisibleCategoryAndReload();
        }
        private void PoliciesList_DoubleClick(object sender, EventArgs e)
        {
            // When the user opens a policy object in the right pane
            if (PoliciesList.SelectedItems.Count == 0)
                return;
            var policyItem = PoliciesList.SelectedItems[0].Tag;
            if (policyItem is PolicyPlusCategory)
            {
                CurrentCategory = (PolicyPlusCategory)policyItem;
                UpdateCategoryListing();
            }
            else if (policyItem is PolicyPlusPolicy)
            {
                ShowSettingEditor((PolicyPlusPolicy)policyItem, ViewPolicyTypes);
            }
        }
        private void DeduplicatePoliciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make otherwise-identical pairs of user and computer policies into single dual-section policies
            ClearSelections();
            int deduped = PolicyProcessing.DeduplicatePolicies(AdmxWorkspace);
            Interaction.MsgBox("Deduplicated " + deduped + " policies.", MsgBoxStyle.Information);
            UpdateCategoryListing();
            UpdatePolicyInfo();
        }
        private void FindByIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Find By ID window and try to move to the selected object
            My.MyProject.Forms.FindById.AdmxWorkspace = AdmxWorkspace;
            if (My.MyProject.Forms.FindById.ShowDialog() == DialogResult.OK)
            {
                var selCat = My.MyProject.Forms.FindById.SelectedCategory;
                var selPol = My.MyProject.Forms.FindById.SelectedPolicy;
                var selPro = My.MyProject.Forms.FindById.SelectedProduct;
                var selSup = My.MyProject.Forms.FindById.SelectedSupport;
                if (selCat is not null)
                {
                    if (CategoryNodes.ContainsKey(selCat))
                    {
                        CurrentCategory = selCat;
                        UpdateCategoryListing();
                    }
                    else
                    {
                        Interaction.MsgBox("The category is not currently visible. Change the view settings and try again.", MsgBoxStyle.Exclamation);
                    }
                }
                else if (selPol is not null)
                {
                    ShowSettingEditor(selPol, (AdmxPolicySection)Math.Min((int)ViewPolicyTypes, (int)My.MyProject.Forms.FindById.SelectedSection));
                    FocusPolicy(selPol);
                }
                else if (selPro is not null)
                {
                    My.MyProject.Forms.DetailProduct.PresentDialog(selPro);
                }
                else if (selSup is not null)
                {
                    My.MyProject.Forms.DetailSupport.PresentDialog(selSup);
                }
                else
                {
                    Interaction.MsgBox("That object could not be found.", MsgBoxStyle.Exclamation);
                }
            }
        }
        private void OpenPolicyResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Open Policy Resources dialog and open its loaders
            if (My.MyProject.Forms.OpenPol.ShowDialog() == DialogResult.OK)
            {
                OpenPolicyLoaders(My.MyProject.Forms.OpenPol.SelectedUser, My.MyProject.Forms.OpenPol.SelectedComputer, false);
                MoveToVisibleCategoryAndReload();
            }
        }
        private void SavePoliciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save policy state and comments to disk
            // Doesn't matter, it's just comments
            void saveComments(Dictionary<string, string> Comments, PolicyLoader Loader) { try { if (Comments is not null) CmtxFile.FromCommentTable(Comments).Save(Loader.GetCmtxPath()); } catch (Exception ex) { } };
            saveComments(UserComments, UserPolicyLoader);
            saveComments(CompComments, CompPolicyLoader);
            try
            {
                string compStatus = "not writable";
                string userStatus = "not writable";
                if (CompPolicyLoader.GetWritability() == PolicySourceWritability.Writable)
                    compStatus = CompPolicyLoader.Save();
                if (UserPolicyLoader.GetWritability() == PolicySourceWritability.Writable)
                    userStatus = UserPolicyLoader.Save();
                Configuration.SetValue("CompSourceType", (int)CompPolicyLoader.Source);
                Configuration.SetValue("UserSourceType", (int)UserPolicyLoader.Source);
                Configuration.SetValue("CompSourceData", CompPolicyLoader.LoaderData ?? "");
                Configuration.SetValue("UserSourceData", UserPolicyLoader.LoaderData ?? "");
                Interaction.MsgBox("Success." + Constants.vbCrLf + Constants.vbCrLf + "User policies: " + userStatus + "." + Constants.vbCrLf + Constants.vbCrLf + "Computer policies: " + compStatus + ".", MsgBoxStyle.Information);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Saving failed!" + Constants.vbCrLf + Constants.vbCrLf + ex.Message, MsgBoxStyle.Exclamation);
            }
        }
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show author and version information if it was compiled into the program
            string about = $"Policy Plus by Ben Nordick.{Constants.vbCrLf}{Constants.vbCrLf}Available on GitHub: Fleex255/PolicyPlus.";
            if (!string.IsNullOrEmpty(VersionHolder.Version.Trim()))
                about += $" Version: {VersionHolder.Version.Trim()}.";
            Interaction.MsgBox(about, MsgBoxStyle.Information);
        }
        private void ByTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Find By Text window and start the search
            if (My.MyProject.Forms.FindByText.PresentDialog(UserComments, CompComments) == DialogResult.OK)
            {
                ShowSearchDialog(My.MyProject.Forms.FindByText.Searcher);
            }
        }
        private void SearchResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the search results window but don't start a search
            ShowSearchDialog(null);
        }
        private void FindNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Move to the next policy in the search results
            do
            {
                var nextPol = My.MyProject.Forms.FindResults.NextPolicy();
                if (nextPol is null)
                {
                    Interaction.MsgBox("There are no more results that match the filter.", MsgBoxStyle.Information);
                    break;
                }
                else if (ShouldShowPolicy(nextPol))
                {
                    FocusPolicy(nextPol);
                    break;
                }
            }
            while (true);
        }
        private void ByRegistryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Find By Registry window and start the search
            if (My.MyProject.Forms.FindByRegistry.ShowDialog() == DialogResult.OK)
                ShowSearchDialog(My.MyProject.Forms.FindByRegistry.Searcher);
        }
        private void SettingInfoPanel_ClientSizeChanged(object sender, EventArgs e)
        {
            // Finagle the middle pane's UI elements
            SettingInfoPanel.AutoScrollMinSize = SettingInfoPanel.Size;
            PolicyTitleLabel.MaximumSize = new Size(PolicyInfoTable.Width, 0);
            PolicySupportedLabel.MaximumSize = new Size(PolicyInfoTable.Width, 0);
            PolicyDescLabel.MaximumSize = new Size(PolicyInfoTable.Width, 0);
            PolicyIsPrefLabel.MaximumSize = new Size(PolicyInfoTable.Width - 22, 0); // Leave room for the exclamation icon
            PolicyInfoTable.MaximumSize = new Size(SettingInfoPanel.Width - (SettingInfoPanel.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0), 0);
            PolicyInfoTable.Width = PolicyInfoTable.MaximumSize.Width;
            if (PolicyInfoTable.ColumnCount > 0)
                PolicyInfoTable.ColumnStyles[0].Width = PolicyInfoTable.ClientSize.Width; // Only once everything is initialized
            PolicyInfoTable.PerformLayout(); // Force the table to take up its full desired size
            PInvoke.ShowScrollBar(SettingInfoPanel.Handle, 0, false); // 0 means horizontal
        }
        private void Main_Closed(object sender, EventArgs e)
        {
            ClosePolicySources(); // Make sure everything is cleaned up before quitting
        }
        private void PoliciesList_KeyDown(object sender, KeyEventArgs e)
        {
            // Activate a right pane item if the user presses Enter on it
            if (e.KeyCode == Keys.Enter & PoliciesList.SelectedItems.Count > 0)
                PoliciesList_DoubleClick(sender, e);
        }
        private void FilterOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Filter Options dialog and refresh if the filter changes
            if (My.MyProject.Forms.FilterOptions.PresentDialog(CurrentFilter, AdmxWorkspace) == DialogResult.OK)
            {
                CurrentFilter = My.MyProject.Forms.FilterOptions.CurrentFilter;
                ViewFilteredOnly = true;
                OnlyFilteredObjectsToolStripMenuItem.Checked = true;
                MoveToVisibleCategoryAndReload();
            }
        }
        private void OnlyFilteredObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle whether the filter is used
            ViewFilteredOnly = !ViewFilteredOnly;
            OnlyFilteredObjectsToolStripMenuItem.Checked = ViewFilteredOnly;
            MoveToVisibleCategoryAndReload();
        }
        private void ImportSemanticPolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open the SPOL import dialog and apply the data
            if (My.MyProject.Forms.ImportSpol.ShowDialog() == DialogResult.OK)
            {
                var spol = My.MyProject.Forms.ImportSpol.Spol;
                int fails = spol.ApplyAll(AdmxWorkspace, UserPolicySource, CompPolicySource, UserComments, CompComments);
                MoveToVisibleCategoryAndReload();
                if (fails == 0)
                {
                    Interaction.MsgBox("Semantic Policy successfully applied.", MsgBoxStyle.Information);
                }
                else
                {
                    Interaction.MsgBox(fails + " out of " + spol.Policies.Count + " could not be applied.", MsgBoxStyle.Exclamation);
                }
            }
        }
        private void ImportPOLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open a POL file and write it to a policy source
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "POL files|*.pol";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    PolFile pol = null;
                    try
                    {
                        pol = PolFile.Load(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        Interaction.MsgBox("The POL file could not be loaded.", MsgBoxStyle.Exclamation);
                        return;
                    }
                    if (My.MyProject.Forms.OpenSection.PresentDialog(true, true) == DialogResult.OK)
                    {
                        var section = My.MyProject.Forms.OpenSection.SelectedSection == AdmxPolicySection.User ? UserPolicySource : CompPolicySource;
                        pol.Apply(section);
                        MoveToVisibleCategoryAndReload();
                        Interaction.MsgBox("POL import successful.", MsgBoxStyle.Information);
                    }
                }
            }
        }
        private void ExportPOLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a POL file from a current policy source
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "POL files|*.pol";
                if (sfd.ShowDialog() == DialogResult.OK && My.MyProject.Forms.OpenSection.PresentDialog(true, true) == DialogResult.OK)
                {
                    var section = My.MyProject.Forms.OpenSection.SelectedSection == AdmxPolicySection.Machine ? CompPolicySource : UserPolicySource;
                    try
                    {
                        GetOrCreatePolFromPolicySource(section).Save(sfd.FileName);
                        Interaction.MsgBox("POL exported successfully.", MsgBoxStyle.Information);
                    }
                    catch (Exception ex)
                    {
                        Interaction.MsgBox("The POL file could not be saved.", MsgBoxStyle.Exclamation);
                    }
                }
            }
        }
        private void AcquireADMXFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Acquire ADMX Files dialog and load the new ADMX files
            if (My.MyProject.Forms.DownloadAdmx.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(My.MyProject.Forms.DownloadAdmx.NewPolicySourceFolder))
                {
                    ClearAdmxWorkspace();
                    DisplayAdmxLoadErrorReport(AdmxWorkspace.LoadFolder(My.MyProject.Forms.DownloadAdmx.NewPolicySourceFolder, GetPreferredLanguageCode()));
                    Configuration.SetValue("AdmxSource", My.MyProject.Forms.DownloadAdmx.NewPolicySourceFolder);
                    PopulateAdmxUi();
                }
            }
        }
        private void LoadedADMXFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.LoadedAdmx.PresentDialog(AdmxWorkspace);
        }
        private void AllSupportDefinitionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.LoadedSupportDefinitions.PresentDialog(AdmxWorkspace);
        }
        private void AllProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.LoadedProducts.PresentDialog(AdmxWorkspace);
        }
        private void EditRawPOLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool userIsPol = UserPolicySource is PolFile;
            bool compIsPol = CompPolicySource is PolFile;
            if (!(userIsPol | compIsPol))
            {
                Interaction.MsgBox("Neither loaded source is backed by a POL file.", MsgBoxStyle.Exclamation);
                return;
            }
            if (Conversions.ToInteger(Configuration.GetValue("EditPolDangerAcknowledged", 0)) == 0)
            {
                if (Interaction.MsgBox("Caution! This tool is for very advanced users. Improper modifications may result in inconsistencies in policies' states." + Constants.vbCrLf + Constants.vbCrLf + "Changes operate directly on the policy source, though they will not be committed to disk until you save. Are you sure you want to continue?", MsgBoxStyle.Exclamation | MsgBoxStyle.YesNo) == MsgBoxResult.No)
                    return;
                Configuration.SetValue("EditPolDangerAcknowledged", 1);
            }
            if (My.MyProject.Forms.OpenSection.PresentDialog(userIsPol, compIsPol) == DialogResult.OK)
            {
                My.MyProject.Forms.EditPol.PresentDialog(PolicyIcons, (PolFile)(My.MyProject.Forms.OpenSection.SelectedSection == AdmxPolicySection.Machine ? CompPolicySource : UserPolicySource), My.MyProject.Forms.OpenSection.SelectedSection == AdmxPolicySection.User);
            }
            MoveToVisibleCategoryAndReload();
        }
        private void ExportREGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (My.MyProject.Forms.OpenSection.PresentDialog(true, true) == DialogResult.OK)
            {
                var source = My.MyProject.Forms.OpenSection.SelectedSection == AdmxPolicySection.Machine ? CompPolicySource : UserPolicySource;
                My.MyProject.Forms.ExportReg.PresentDialog("", GetOrCreatePolFromPolicySource(source), My.MyProject.Forms.OpenSection.SelectedSection == AdmxPolicySection.User);
            }
        }
        private void ImportREGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (My.MyProject.Forms.OpenSection.PresentDialog(true, true) == DialogResult.OK)
            {
                var source = My.MyProject.Forms.OpenSection.SelectedSection == AdmxPolicySection.Machine ? CompPolicySource : UserPolicySource;
                if (My.MyProject.Forms.ImportReg.PresentDialog(source) == DialogResult.OK)
                    MoveToVisibleCategoryAndReload();
            }
        }
        private void SetADMLLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (My.MyProject.Forms.LanguageOptions.PresentDialog(GetPreferredLanguageCode()) == DialogResult.OK)
            {
                Configuration.SetValue("LanguageCode", My.MyProject.Forms.LanguageOptions.NewLanguage);
                if (Interaction.MsgBox("Language changes will take effect when ADML files are next loaded. Would you like to reload the workspace now?", MsgBoxStyle.YesNo | MsgBoxStyle.Question) == MsgBoxResult.Yes)
                {
                    ClearAdmxWorkspace();
                    OpenLastAdmxSource();
                    PopulateAdmxUi();
                }
            }
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
                bool ok = true;
                if (Conversions.ToString(item.Tag) == "P" & showingForCategory)
                    ok = false;
                if (Conversions.ToString(item.Tag) == "C" & !showingForCategory)
                    ok = false;
                item.Visible = ok;
            }
        }
        private void PolicyObjectContext_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // When the user clicks an item in the right-click menu
            var polObject = PolicyObjectContext.Tag; // The current policy object is in the Tag field
            if (ReferenceEquals(e.ClickedItem, CmeCatOpen))
            {
                CurrentCategory = (PolicyPlusCategory)polObject;
                UpdateCategoryListing();
            }
            else if (ReferenceEquals(e.ClickedItem, CmePolEdit))
            {
                ShowSettingEditor((PolicyPlusPolicy)polObject, ViewPolicyTypes);
            }
            else if (ReferenceEquals(e.ClickedItem, CmeAllDetails))
            {
                if (polObject is PolicyPlusCategory)
                {
                    My.MyProject.Forms.DetailCategory.PresentDialog((PolicyPlusCategory)polObject);
                }
                else
                {
                    My.MyProject.Forms.DetailPolicy.PresentDialog((PolicyPlusPolicy)polObject);
                }
            }
            else if (ReferenceEquals(e.ClickedItem, CmePolInspectElements))
            {
                My.MyProject.Forms.InspectPolicyElements.PresentDialog((PolicyPlusPolicy)polObject, PolicyIcons, AdmxWorkspace);
            }
            else if (ReferenceEquals(e.ClickedItem, CmePolSpolFragment))
            {
                My.MyProject.Forms.InspectSpolFragment.PresentDialog((PolicyPlusPolicy)polObject, AdmxWorkspace, CompPolicySource, UserPolicySource, CompComments, UserComments);
            }
        }
        private void CategoriesTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Right-clicking doesn't actually select the node by default
            if (e.Button == MouseButtons.Right)
                CategoriesTree.SelectedNode = e.Node;
        }
        public static string PrettifyDescription(string Description)
        {
            // Remove extra indentation from paragraphs
            var sb = new StringBuilder();
            foreach (var line in Description.Split(Conversions.ToChar(Constants.vbCrLf)))
                sb.AppendLine(line.Trim());
            return sb.ToString().TrimEnd();
        }
    }
}