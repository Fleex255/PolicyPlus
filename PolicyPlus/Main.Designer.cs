using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Main : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ColumnHeader ChSettingEnabled;
            ColumnHeader ChSettingCommented;
            ToolStripSeparator ToolStripSeparator1;
            ToolStripSeparator ToolStripSeparator2;
            ToolStripSeparator ToolStripSeparator3;
            ToolStripSeparator ToolStripSeparator4;
            ToolStripSeparator ToolStripSeparator5;
            ToolStripStatusLabel ToolStripStatusLabel1;
            ToolStripStatusLabel ToolStripStatusLabel2;
            ToolStripSeparator ToolStripSeparator6;
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            MainMenu = new MenuStrip();
            FileToolStripMenuItem = new ToolStripMenuItem();
            OpenADMXFolderToolStripMenuItem = new ToolStripMenuItem();
            OpenADMXFolderToolStripMenuItem.Click += new EventHandler(OpenADMXFolderToolStripMenuItem_Click);
            OpenADMXFileToolStripMenuItem = new ToolStripMenuItem();
            OpenADMXFileToolStripMenuItem.Click += new EventHandler(OpenADMXFileToolStripMenuItem_Click);
            SetADMLLanguageToolStripMenuItem = new ToolStripMenuItem();
            SetADMLLanguageToolStripMenuItem.Click += new EventHandler(SetADMLLanguageToolStripMenuItem_Click);
            CloseADMXWorkspaceToolStripMenuItem = new ToolStripMenuItem();
            CloseADMXWorkspaceToolStripMenuItem.Click += new EventHandler(CloseADMXWorkspaceToolStripMenuItem_Click);
            OpenPolicyResourcesToolStripMenuItem = new ToolStripMenuItem();
            OpenPolicyResourcesToolStripMenuItem.Click += new EventHandler(OpenPolicyResourcesToolStripMenuItem_Click);
            SavePoliciesToolStripMenuItem = new ToolStripMenuItem();
            SavePoliciesToolStripMenuItem.Click += new EventHandler(SavePoliciesToolStripMenuItem_Click);
            EditRawPOLToolStripMenuItem = new ToolStripMenuItem();
            EditRawPOLToolStripMenuItem.Click += new EventHandler(EditRawPOLToolStripMenuItem_Click);
            ExitToolStripMenuItem = new ToolStripMenuItem();
            ExitToolStripMenuItem.Click += new EventHandler(ExitToolStripMenuItem_Click);
            ViewToolStripMenuItem = new ToolStripMenuItem();
            EmptyCategoriesToolStripMenuItem = new ToolStripMenuItem();
            EmptyCategoriesToolStripMenuItem.Click += new EventHandler(EmptyCategoriesToolStripMenuItem_Click);
            OnlyFilteredObjectsToolStripMenuItem = new ToolStripMenuItem();
            OnlyFilteredObjectsToolStripMenuItem.Click += new EventHandler(OnlyFilteredObjectsToolStripMenuItem_Click);
            FilterOptionsToolStripMenuItem = new ToolStripMenuItem();
            FilterOptionsToolStripMenuItem.Click += new EventHandler(FilterOptionsToolStripMenuItem_Click);
            DeduplicatePoliciesToolStripMenuItem = new ToolStripMenuItem();
            DeduplicatePoliciesToolStripMenuItem.Click += new EventHandler(DeduplicatePoliciesToolStripMenuItem_Click);
            LoadedADMXFilesToolStripMenuItem = new ToolStripMenuItem();
            LoadedADMXFilesToolStripMenuItem.Click += new EventHandler(LoadedADMXFilesToolStripMenuItem_Click);
            AllProductsToolStripMenuItem = new ToolStripMenuItem();
            AllProductsToolStripMenuItem.Click += new EventHandler(AllProductsToolStripMenuItem_Click);
            AllSupportDefinitionsToolStripMenuItem = new ToolStripMenuItem();
            AllSupportDefinitionsToolStripMenuItem.Click += new EventHandler(AllSupportDefinitionsToolStripMenuItem_Click);
            FindToolStripMenuItem = new ToolStripMenuItem();
            ByIDToolStripMenuItem = new ToolStripMenuItem();
            ByIDToolStripMenuItem.Click += new EventHandler(FindByIDToolStripMenuItem_Click);
            ByTextToolStripMenuItem = new ToolStripMenuItem();
            ByTextToolStripMenuItem.Click += new EventHandler(ByTextToolStripMenuItem_Click);
            ByRegistryToolStripMenuItem = new ToolStripMenuItem();
            ByRegistryToolStripMenuItem.Click += new EventHandler(ByRegistryToolStripMenuItem_Click);
            SearchResultsToolStripMenuItem = new ToolStripMenuItem();
            SearchResultsToolStripMenuItem.Click += new EventHandler(SearchResultsToolStripMenuItem_Click);
            FindNextToolStripMenuItem = new ToolStripMenuItem();
            FindNextToolStripMenuItem.Click += new EventHandler(FindNextToolStripMenuItem_Click);
            ShareToolStripMenuItem = new ToolStripMenuItem();
            ImportSemanticPolicyToolStripMenuItem = new ToolStripMenuItem();
            ImportSemanticPolicyToolStripMenuItem.Click += new EventHandler(ImportSemanticPolicyToolStripMenuItem_Click);
            ImportPOLToolStripMenuItem = new ToolStripMenuItem();
            ImportPOLToolStripMenuItem.Click += new EventHandler(ImportPOLToolStripMenuItem_Click);
            ImportREGToolStripMenuItem = new ToolStripMenuItem();
            ImportREGToolStripMenuItem.Click += new EventHandler(ImportREGToolStripMenuItem_Click);
            ExportPOLToolStripMenuItem = new ToolStripMenuItem();
            ExportPOLToolStripMenuItem.Click += new EventHandler(ExportPOLToolStripMenuItem_Click);
            ExportREGToolStripMenuItem = new ToolStripMenuItem();
            ExportREGToolStripMenuItem.Click += new EventHandler(ExportREGToolStripMenuItem_Click);
            HelpToolStripMenuItem = new ToolStripMenuItem();
            AboutToolStripMenuItem = new ToolStripMenuItem();
            AboutToolStripMenuItem.Click += new EventHandler(AboutToolStripMenuItem_Click);
            AcquireADMXFilesToolStripMenuItem = new ToolStripMenuItem();
            AcquireADMXFilesToolStripMenuItem.Click += new EventHandler(AcquireADMXFilesToolStripMenuItem_Click);
            SplitContainer = new SplitContainer();
            ComboAppliesTo = new ComboBox();
            ComboAppliesTo.SelectedIndexChanged += new EventHandler(ComboAppliesTo_SelectedIndexChanged);
            CategoriesTree = new TreeView();
            CategoriesTree.AfterSelect += new TreeViewEventHandler(CategoriesTree_AfterSelect);
            CategoriesTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(CategoriesTree_NodeMouseClick);
            PolicyObjectContext = new ContextMenuStrip(components);
            PolicyObjectContext.Opening += new System.ComponentModel.CancelEventHandler(PolicyObjectContext_Opening);
            PolicyObjectContext.ItemClicked += new ToolStripItemClickedEventHandler(PolicyObjectContext_ItemClicked);
            CmeCatOpen = new ToolStripMenuItem();
            CmePolEdit = new ToolStripMenuItem();
            CmeAllDetails = new ToolStripMenuItem();
            CmePolInspectElements = new ToolStripMenuItem();
            CmePolSpolFragment = new ToolStripMenuItem();
            PolicyIcons = new ImageList(components);
            PoliciesList = new ListView();
            PoliciesList.SizeChanged += new EventHandler(ResizePolicyNameColumn);
            PoliciesList.SelectedIndexChanged += new EventHandler(PoliciesList_SelectedIndexChanged);
            PoliciesList.DoubleClick += new EventHandler(PoliciesList_DoubleClick);
            PoliciesList.KeyDown += new KeyEventHandler(PoliciesList_KeyDown);
            ChSettingName = new ColumnHeader();
            SettingInfoPanel = new Panel();
            SettingInfoPanel.ClientSizeChanged += new EventHandler(SettingInfoPanel_ClientSizeChanged);
            SettingInfoPanel.SizeChanged += new EventHandler(SettingInfoPanel_ClientSizeChanged);
            PolicyInfoTable = new TableLayoutPanel();
            PolicyTitleLabel = new Label();
            PolicySupportedLabel = new Label();
            PolicyDescLabel = new Label();
            InfoStrip = new StatusStrip();
            ComputerSourceLabel = new ToolStripStatusLabel();
            UserSourceLabel = new ToolStripStatusLabel();
            PolicyIsPrefTable = new TableLayoutPanel();
            PictureBox1 = new PictureBox();
            PolicyIsPrefLabel = new Label();
            ChSettingEnabled = new ColumnHeader();
            ChSettingCommented = new ColumnHeader();
            ToolStripSeparator1 = new ToolStripSeparator();
            ToolStripSeparator2 = new ToolStripSeparator();
            ToolStripSeparator3 = new ToolStripSeparator();
            ToolStripSeparator4 = new ToolStripSeparator();
            ToolStripSeparator5 = new ToolStripSeparator();
            ToolStripStatusLabel1 = new ToolStripStatusLabel();
            ToolStripStatusLabel2 = new ToolStripStatusLabel();
            ToolStripSeparator6 = new ToolStripSeparator();
            MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer).BeginInit();
            SplitContainer.Panel1.SuspendLayout();
            SplitContainer.Panel2.SuspendLayout();
            SplitContainer.SuspendLayout();
            PolicyObjectContext.SuspendLayout();
            SettingInfoPanel.SuspendLayout();
            PolicyInfoTable.SuspendLayout();
            InfoStrip.SuspendLayout();
            PolicyIsPrefTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // ChSettingEnabled
            // 
            ChSettingEnabled.Text = "State";
            ChSettingEnabled.TextAlign = HorizontalAlignment.Center;
            ChSettingEnabled.Width = 107;
            // 
            // ChSettingCommented
            // 
            ChSettingCommented.Text = "Comment";
            ChSettingCommented.TextAlign = HorizontalAlignment.Center;
            ChSettingCommented.Width = 68;
            // 
            // ToolStripSeparator1
            // 
            ToolStripSeparator1.Name = "ToolStripSeparator1";
            ToolStripSeparator1.Size = new Size(190, 6);
            // 
            // ToolStripSeparator2
            // 
            ToolStripSeparator2.Name = "ToolStripSeparator2";
            ToolStripSeparator2.Size = new Size(234, 6);
            // 
            // ToolStripSeparator3
            // 
            ToolStripSeparator3.Name = "ToolStripSeparator3";
            ToolStripSeparator3.Size = new Size(234, 6);
            // 
            // ToolStripSeparator4
            // 
            ToolStripSeparator4.Name = "ToolStripSeparator4";
            ToolStripSeparator4.Size = new Size(197, 6);
            // 
            // ToolStripSeparator5
            // 
            ToolStripSeparator5.Name = "ToolStripSeparator5";
            ToolStripSeparator5.Size = new Size(194, 6);
            // 
            // ToolStripStatusLabel1
            // 
            ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            ToolStripStatusLabel1.Size = new Size(102, 17);
            ToolStripStatusLabel1.Text = "Computer source:";
            // 
            // ToolStripStatusLabel2
            // 
            ToolStripStatusLabel2.Name = "ToolStripStatusLabel2";
            ToolStripStatusLabel2.Size = new Size(71, 17);
            ToolStripStatusLabel2.Text = "User source:";
            // 
            // ToolStripSeparator6
            // 
            ToolStripSeparator6.Name = "ToolStripSeparator6";
            ToolStripSeparator6.Size = new Size(190, 6);
            // 
            // MainMenu
            // 
            MainMenu.Items.AddRange(new ToolStripItem[] { FileToolStripMenuItem, ViewToolStripMenuItem, FindToolStripMenuItem, ShareToolStripMenuItem, HelpToolStripMenuItem });
            MainMenu.Location = new Point(0, 0);
            MainMenu.Name = "MainMenu";
            MainMenu.Size = new Size(706, 24);
            MainMenu.TabIndex = 0;
            MainMenu.Text = "MenuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            FileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenADMXFolderToolStripMenuItem, OpenADMXFileToolStripMenuItem, SetADMLLanguageToolStripMenuItem, CloseADMXWorkspaceToolStripMenuItem, ToolStripSeparator2, OpenPolicyResourcesToolStripMenuItem, SavePoliciesToolStripMenuItem, EditRawPOLToolStripMenuItem, ToolStripSeparator3, ExitToolStripMenuItem });
            FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            FileToolStripMenuItem.Size = new Size(37, 20);
            FileToolStripMenuItem.Text = "File";
            // 
            // OpenADMXFolderToolStripMenuItem
            // 
            OpenADMXFolderToolStripMenuItem.Name = "OpenADMXFolderToolStripMenuItem";
            OpenADMXFolderToolStripMenuItem.Size = new Size(237, 22);
            OpenADMXFolderToolStripMenuItem.Text = "Open ADMX Folder";
            // 
            // OpenADMXFileToolStripMenuItem
            // 
            OpenADMXFileToolStripMenuItem.Name = "OpenADMXFileToolStripMenuItem";
            OpenADMXFileToolStripMenuItem.Size = new Size(237, 22);
            OpenADMXFileToolStripMenuItem.Text = "Open ADMX File";
            // 
            // SetADMLLanguageToolStripMenuItem
            // 
            SetADMLLanguageToolStripMenuItem.Name = "SetADMLLanguageToolStripMenuItem";
            SetADMLLanguageToolStripMenuItem.Size = new Size(237, 22);
            SetADMLLanguageToolStripMenuItem.Text = "Set ADML Language";
            // 
            // CloseADMXWorkspaceToolStripMenuItem
            // 
            CloseADMXWorkspaceToolStripMenuItem.Name = "CloseADMXWorkspaceToolStripMenuItem";
            CloseADMXWorkspaceToolStripMenuItem.Size = new Size(237, 22);
            CloseADMXWorkspaceToolStripMenuItem.Text = "Close ADMX Workspace";
            // 
            // OpenPolicyResourcesToolStripMenuItem
            // 
            OpenPolicyResourcesToolStripMenuItem.Name = "OpenPolicyResourcesToolStripMenuItem";
            OpenPolicyResourcesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            OpenPolicyResourcesToolStripMenuItem.Size = new Size(237, 22);
            OpenPolicyResourcesToolStripMenuItem.Text = "Open Policy Resources";
            // 
            // SavePoliciesToolStripMenuItem
            // 
            SavePoliciesToolStripMenuItem.Name = "SavePoliciesToolStripMenuItem";
            SavePoliciesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            SavePoliciesToolStripMenuItem.Size = new Size(237, 22);
            SavePoliciesToolStripMenuItem.Text = "Save Policies";
            // 
            // EditRawPOLToolStripMenuItem
            // 
            EditRawPOLToolStripMenuItem.Name = "EditRawPOLToolStripMenuItem";
            EditRawPOLToolStripMenuItem.Size = new Size(237, 22);
            EditRawPOLToolStripMenuItem.Text = "Edit Raw POL";
            // 
            // ExitToolStripMenuItem
            // 
            ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            ExitToolStripMenuItem.Size = new Size(237, 22);
            ExitToolStripMenuItem.Text = "Exit";
            // 
            // ViewToolStripMenuItem
            // 
            ViewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { EmptyCategoriesToolStripMenuItem, OnlyFilteredObjectsToolStripMenuItem, ToolStripSeparator1, FilterOptionsToolStripMenuItem, DeduplicatePoliciesToolStripMenuItem, ToolStripSeparator6, LoadedADMXFilesToolStripMenuItem, AllProductsToolStripMenuItem, AllSupportDefinitionsToolStripMenuItem });
            ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            ViewToolStripMenuItem.Size = new Size(44, 20);
            ViewToolStripMenuItem.Text = "View";
            // 
            // EmptyCategoriesToolStripMenuItem
            // 
            EmptyCategoriesToolStripMenuItem.Name = "EmptyCategoriesToolStripMenuItem";
            EmptyCategoriesToolStripMenuItem.Size = new Size(193, 22);
            EmptyCategoriesToolStripMenuItem.Text = "Empty Categories";
            // 
            // OnlyFilteredObjectsToolStripMenuItem
            // 
            OnlyFilteredObjectsToolStripMenuItem.Name = "OnlyFilteredObjectsToolStripMenuItem";
            OnlyFilteredObjectsToolStripMenuItem.Size = new Size(193, 22);
            OnlyFilteredObjectsToolStripMenuItem.Text = "Only Filtered Policies";
            // 
            // FilterOptionsToolStripMenuItem
            // 
            FilterOptionsToolStripMenuItem.Name = "FilterOptionsToolStripMenuItem";
            FilterOptionsToolStripMenuItem.Size = new Size(193, 22);
            FilterOptionsToolStripMenuItem.Text = "Filter Options";
            // 
            // DeduplicatePoliciesToolStripMenuItem
            // 
            DeduplicatePoliciesToolStripMenuItem.Name = "DeduplicatePoliciesToolStripMenuItem";
            DeduplicatePoliciesToolStripMenuItem.Size = new Size(193, 22);
            DeduplicatePoliciesToolStripMenuItem.Text = "Deduplicate Policies";
            DeduplicatePoliciesToolStripMenuItem.Visible = false;
            // 
            // LoadedADMXFilesToolStripMenuItem
            // 
            LoadedADMXFilesToolStripMenuItem.Name = "LoadedADMXFilesToolStripMenuItem";
            LoadedADMXFilesToolStripMenuItem.Size = new Size(193, 22);
            LoadedADMXFilesToolStripMenuItem.Text = "Loaded ADMX Files";
            // 
            // AllProductsToolStripMenuItem
            // 
            AllProductsToolStripMenuItem.Name = "AllProductsToolStripMenuItem";
            AllProductsToolStripMenuItem.Size = new Size(193, 22);
            AllProductsToolStripMenuItem.Text = "All Products";
            // 
            // AllSupportDefinitionsToolStripMenuItem
            // 
            AllSupportDefinitionsToolStripMenuItem.Name = "AllSupportDefinitionsToolStripMenuItem";
            AllSupportDefinitionsToolStripMenuItem.Size = new Size(193, 22);
            AllSupportDefinitionsToolStripMenuItem.Text = "All Support Definitions";
            // 
            // FindToolStripMenuItem
            // 
            FindToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ByIDToolStripMenuItem, ByTextToolStripMenuItem, ByRegistryToolStripMenuItem, ToolStripSeparator4, SearchResultsToolStripMenuItem, FindNextToolStripMenuItem });
            FindToolStripMenuItem.Name = "FindToolStripMenuItem";
            FindToolStripMenuItem.Size = new Size(42, 20);
            FindToolStripMenuItem.Text = "Find";
            // 
            // ByIDToolStripMenuItem
            // 
            ByIDToolStripMenuItem.Name = "ByIDToolStripMenuItem";
            ByIDToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.G;
            ByIDToolStripMenuItem.Size = new Size(200, 22);
            ByIDToolStripMenuItem.Text = "By ID";
            // 
            // ByTextToolStripMenuItem
            // 
            ByTextToolStripMenuItem.Name = "ByTextToolStripMenuItem";
            ByTextToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            ByTextToolStripMenuItem.Size = new Size(200, 22);
            ByTextToolStripMenuItem.Text = "By Text";
            // 
            // ByRegistryToolStripMenuItem
            // 
            ByRegistryToolStripMenuItem.Name = "ByRegistryToolStripMenuItem";
            ByRegistryToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            ByRegistryToolStripMenuItem.Size = new Size(200, 22);
            ByRegistryToolStripMenuItem.Text = "By Registry";
            // 
            // SearchResultsToolStripMenuItem
            // 
            SearchResultsToolStripMenuItem.Name = "SearchResultsToolStripMenuItem";
            SearchResultsToolStripMenuItem.ShortcutKeys = Keys.Shift | Keys.F3;
            SearchResultsToolStripMenuItem.Size = new Size(200, 22);
            SearchResultsToolStripMenuItem.Text = "Search Results";
            // 
            // FindNextToolStripMenuItem
            // 
            FindNextToolStripMenuItem.Name = "FindNextToolStripMenuItem";
            FindNextToolStripMenuItem.ShortcutKeys = Keys.F3;
            FindNextToolStripMenuItem.Size = new Size(200, 22);
            FindNextToolStripMenuItem.Text = "Find Next";
            // 
            // ShareToolStripMenuItem
            // 
            ShareToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ImportSemanticPolicyToolStripMenuItem, ImportPOLToolStripMenuItem, ImportREGToolStripMenuItem, ToolStripSeparator5, ExportPOLToolStripMenuItem, ExportREGToolStripMenuItem });
            ShareToolStripMenuItem.Name = "ShareToolStripMenuItem";
            ShareToolStripMenuItem.Size = new Size(48, 20);
            ShareToolStripMenuItem.Text = "Share";
            // 
            // ImportSemanticPolicyToolStripMenuItem
            // 
            ImportSemanticPolicyToolStripMenuItem.Name = "ImportSemanticPolicyToolStripMenuItem";
            ImportSemanticPolicyToolStripMenuItem.Size = new Size(197, 22);
            ImportSemanticPolicyToolStripMenuItem.Text = "Import Semantic Policy";
            // 
            // ImportPOLToolStripMenuItem
            // 
            ImportPOLToolStripMenuItem.Name = "ImportPOLToolStripMenuItem";
            ImportPOLToolStripMenuItem.Size = new Size(197, 22);
            ImportPOLToolStripMenuItem.Text = "Import POL";
            // 
            // ImportREGToolStripMenuItem
            // 
            ImportREGToolStripMenuItem.Name = "ImportREGToolStripMenuItem";
            ImportREGToolStripMenuItem.Size = new Size(197, 22);
            ImportREGToolStripMenuItem.Text = "Import REG";
            // 
            // ExportPOLToolStripMenuItem
            // 
            ExportPOLToolStripMenuItem.Name = "ExportPOLToolStripMenuItem";
            ExportPOLToolStripMenuItem.Size = new Size(197, 22);
            ExportPOLToolStripMenuItem.Text = "Export POL";
            // 
            // ExportREGToolStripMenuItem
            // 
            ExportREGToolStripMenuItem.Name = "ExportREGToolStripMenuItem";
            ExportREGToolStripMenuItem.Size = new Size(197, 22);
            ExportREGToolStripMenuItem.Text = "Export REG";
            // 
            // HelpToolStripMenuItem
            // 
            HelpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { AboutToolStripMenuItem, AcquireADMXFilesToolStripMenuItem });
            HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            HelpToolStripMenuItem.Size = new Size(44, 20);
            HelpToolStripMenuItem.Text = "Help";
            // 
            // AboutToolStripMenuItem
            // 
            AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            AboutToolStripMenuItem.Size = new Size(178, 22);
            AboutToolStripMenuItem.Text = "About";
            // 
            // AcquireADMXFilesToolStripMenuItem
            // 
            AcquireADMXFilesToolStripMenuItem.Name = "AcquireADMXFilesToolStripMenuItem";
            AcquireADMXFilesToolStripMenuItem.Size = new Size(178, 22);
            AcquireADMXFilesToolStripMenuItem.Text = "Acquire ADMX Files";
            // 
            // SplitContainer
            // 
            SplitContainer.Dock = DockStyle.Fill;
            SplitContainer.Location = new Point(0, 24);
            SplitContainer.Name = "SplitContainer";
            // 
            // SplitContainer.Panel1
            // 
            SplitContainer.Panel1.Controls.Add(ComboAppliesTo);
            SplitContainer.Panel1.Controls.Add(CategoriesTree);
            // 
            // SplitContainer.Panel2
            // 
            SplitContainer.Panel2.BackColor = Color.White;
            SplitContainer.Panel2.Controls.Add(PoliciesList);
            SplitContainer.Panel2.Controls.Add(SettingInfoPanel);
            SplitContainer.Size = new Size(706, 350);
            SplitContainer.SplitterDistance = 190;
            SplitContainer.TabIndex = 1;
            SplitContainer.TabStop = false;
            // 
            // ComboAppliesTo
            // 
            ComboAppliesTo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ComboAppliesTo.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboAppliesTo.Items.AddRange(new object[] { "User or Computer", "User", "Computer" });
            ComboAppliesTo.Location = new Point(0, 0);
            ComboAppliesTo.Name = "ComboAppliesTo";
            ComboAppliesTo.Size = new Size(190, 21);
            ComboAppliesTo.TabIndex = 1;
            // 
            // CategoriesTree
            // 
            CategoriesTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            CategoriesTree.BorderStyle = BorderStyle.None;
            CategoriesTree.ContextMenuStrip = PolicyObjectContext;
            CategoriesTree.HideSelection = false;
            CategoriesTree.ImageIndex = 0;
            CategoriesTree.ImageList = PolicyIcons;
            CategoriesTree.Location = new Point(0, 19);
            CategoriesTree.Name = "CategoriesTree";
            CategoriesTree.SelectedImageIndex = 0;
            CategoriesTree.ShowNodeToolTips = true;
            CategoriesTree.Size = new Size(190, 331);
            CategoriesTree.TabIndex = 2;
            // 
            // PolicyObjectContext
            // 
            PolicyObjectContext.Items.AddRange(new ToolStripItem[] { CmeCatOpen, CmePolEdit, CmeAllDetails, CmePolInspectElements, CmePolSpolFragment });
            PolicyObjectContext.Name = "PolicyObjectContext";
            PolicyObjectContext.Size = new Size(213, 114);
            // 
            // CmeCatOpen
            // 
            CmeCatOpen.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            CmeCatOpen.Name = "CmeCatOpen";
            CmeCatOpen.Size = new Size(212, 22);
            CmeCatOpen.Tag = "C";
            CmeCatOpen.Text = "Open";
            // 
            // CmePolEdit
            // 
            CmePolEdit.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            CmePolEdit.Name = "CmePolEdit";
            CmePolEdit.Size = new Size(212, 22);
            CmePolEdit.Tag = "P";
            CmePolEdit.Text = "Edit";
            // 
            // CmeAllDetails
            // 
            CmeAllDetails.Name = "CmeAllDetails";
            CmeAllDetails.Size = new Size(212, 22);
            CmeAllDetails.Text = "Details";
            // 
            // CmePolInspectElements
            // 
            CmePolInspectElements.Name = "CmePolInspectElements";
            CmePolInspectElements.Size = new Size(212, 22);
            CmePolInspectElements.Tag = "P";
            CmePolInspectElements.Text = "Element Inspector";
            // 
            // CmePolSpolFragment
            // 
            CmePolSpolFragment.Name = "CmePolSpolFragment";
            CmePolSpolFragment.Size = new Size(212, 22);
            CmePolSpolFragment.Tag = "P";
            CmePolSpolFragment.Text = "Semantic Policy Fragment";
            // 
            // PolicyIcons
            // 
            PolicyIcons.ImageStream = (ImageListStreamer)resources.GetObject("PolicyIcons.ImageStream");
            PolicyIcons.TransparentColor = Color.Transparent;
            PolicyIcons.Images.SetKeyName(0, "folder.png");
            PolicyIcons.Images.SetKeyName(1, "folder_error.png");
            PolicyIcons.Images.SetKeyName(2, "folder_delete.png");
            PolicyIcons.Images.SetKeyName(3, "folder_go.png");
            PolicyIcons.Images.SetKeyName(4, "page_white.png");
            PolicyIcons.Images.SetKeyName(5, "page_white_gear.png");
            PolicyIcons.Images.SetKeyName(6, "arrow_up.png");
            PolicyIcons.Images.SetKeyName(7, "page_white_error.png");
            PolicyIcons.Images.SetKeyName(8, "delete.png");
            PolicyIcons.Images.SetKeyName(9, "arrow_right.png");
            PolicyIcons.Images.SetKeyName(10, "package.png");
            PolicyIcons.Images.SetKeyName(11, "computer.png");
            PolicyIcons.Images.SetKeyName(12, "database.png");
            PolicyIcons.Images.SetKeyName(13, "cog.png");
            PolicyIcons.Images.SetKeyName(14, "text_allcaps.png");
            PolicyIcons.Images.SetKeyName(15, "calculator.png");
            PolicyIcons.Images.SetKeyName(16, "cog_edit.png");
            PolicyIcons.Images.SetKeyName(17, "accept.png");
            PolicyIcons.Images.SetKeyName(18, "cross.png");
            PolicyIcons.Images.SetKeyName(19, "application_xp_terminal.png");
            PolicyIcons.Images.SetKeyName(20, "application_form.png");
            PolicyIcons.Images.SetKeyName(21, "text_align_left.png");
            PolicyIcons.Images.SetKeyName(22, "calculator_edit.png");
            PolicyIcons.Images.SetKeyName(23, "wrench.png");
            PolicyIcons.Images.SetKeyName(24, "textfield.png");
            PolicyIcons.Images.SetKeyName(25, "tick.png");
            PolicyIcons.Images.SetKeyName(26, "text_horizontalrule.png");
            PolicyIcons.Images.SetKeyName(27, "table.png");
            PolicyIcons.Images.SetKeyName(28, "table_sort.png");
            PolicyIcons.Images.SetKeyName(29, "font_go.png");
            PolicyIcons.Images.SetKeyName(30, "application_view_list.png");
            PolicyIcons.Images.SetKeyName(31, "brick.png");
            PolicyIcons.Images.SetKeyName(32, "error.png");
            PolicyIcons.Images.SetKeyName(33, "style.png");
            PolicyIcons.Images.SetKeyName(34, "sound_low.png");
            PolicyIcons.Images.SetKeyName(35, "arrow_down.png");
            PolicyIcons.Images.SetKeyName(36, "style_go.png");
            PolicyIcons.Images.SetKeyName(37, "exclamation.png");
            PolicyIcons.Images.SetKeyName(38, "application_cascade.png");
            PolicyIcons.Images.SetKeyName(39, "page_copy.png");
            PolicyIcons.Images.SetKeyName(40, "page.png");
            PolicyIcons.Images.SetKeyName(41, "calculator_add.png");
            PolicyIcons.Images.SetKeyName(42, "page_go.png");
            // 
            // PoliciesList
            // 
            PoliciesList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            PoliciesList.BorderStyle = BorderStyle.None;
            PoliciesList.Columns.AddRange(new ColumnHeader[] { ChSettingName, ChSettingEnabled, ChSettingCommented });
            PoliciesList.ContextMenuStrip = PolicyObjectContext;
            PoliciesList.FullRowSelect = true;
            PoliciesList.HideSelection = false;
            PoliciesList.Location = new Point(190, 0);
            PoliciesList.MultiSelect = false;
            PoliciesList.Name = "PoliciesList";
            PoliciesList.ShowItemToolTips = true;
            PoliciesList.Size = new Size(322, 350);
            PoliciesList.SmallImageList = PolicyIcons;
            PoliciesList.TabIndex = 3;
            PoliciesList.UseCompatibleStateImageBehavior = false;
            PoliciesList.View = View.Details;
            // 
            // ChSettingName
            // 
            ChSettingName.Text = "Name";
            ChSettingName.Width = 116;
            // 
            // SettingInfoPanel
            // 
            SettingInfoPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            SettingInfoPanel.AutoScroll = true;
            SettingInfoPanel.Controls.Add(PolicyInfoTable);
            SettingInfoPanel.Location = new Point(0, 0);
            SettingInfoPanel.Name = "SettingInfoPanel";
            SettingInfoPanel.Size = new Size(184, 350);
            SettingInfoPanel.TabIndex = 0;
            // 
            // PolicyInfoTable
            // 
            PolicyInfoTable.AutoSize = true;
            PolicyInfoTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PolicyInfoTable.ColumnCount = 1;
            PolicyInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 706.0f));
            PolicyInfoTable.Controls.Add(PolicyTitleLabel, 0, 0);
            PolicyInfoTable.Controls.Add(PolicySupportedLabel, 0, 1);
            PolicyInfoTable.Controls.Add(PolicyDescLabel, 0, 3);
            PolicyInfoTable.Controls.Add(PolicyIsPrefTable, 0, 2);
            PolicyInfoTable.Location = new Point(0, 0);
            PolicyInfoTable.Name = "PolicyInfoTable";
            PolicyInfoTable.RowCount = 5;
            PolicyInfoTable.RowStyles.Add(new RowStyle());
            PolicyInfoTable.RowStyles.Add(new RowStyle());
            PolicyInfoTable.RowStyles.Add(new RowStyle());
            PolicyInfoTable.RowStyles.Add(new RowStyle());
            PolicyInfoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20.0f));
            PolicyInfoTable.Size = new Size(706, 156);
            PolicyInfoTable.TabIndex = 0;
            // 
            // PolicyTitleLabel
            // 
            PolicyTitleLabel.AutoSize = true;
            PolicyTitleLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            PolicyTitleLabel.Location = new Point(3, 0);
            PolicyTitleLabel.Margin = new Padding(3, 0, 3, 24);
            PolicyTitleLabel.Name = "PolicyTitleLabel";
            PolicyTitleLabel.Size = new Size(66, 13);
            PolicyTitleLabel.TabIndex = 0;
            PolicyTitleLabel.Text = "Policy title";
            PolicyTitleLabel.UseMnemonic = false;
            // 
            // PolicySupportedLabel
            // 
            PolicySupportedLabel.AutoSize = true;
            PolicySupportedLabel.Location = new Point(3, 37);
            PolicySupportedLabel.Margin = new Padding(3, 0, 3, 24);
            PolicySupportedLabel.Name = "PolicySupportedLabel";
            PolicySupportedLabel.Size = new Size(72, 13);
            PolicySupportedLabel.TabIndex = 1;
            PolicySupportedLabel.Text = "Requirements";
            PolicySupportedLabel.UseMnemonic = false;
            // 
            // PolicyDescLabel
            // 
            PolicyDescLabel.AutoSize = true;
            PolicyDescLabel.Location = new Point(3, 123);
            PolicyDescLabel.Name = "PolicyDescLabel";
            PolicyDescLabel.Size = new Size(89, 13);
            PolicyDescLabel.TabIndex = 2;
            PolicyDescLabel.Text = "Policy description";
            PolicyDescLabel.UseMnemonic = false;
            // 
            // InfoStrip
            // 
            InfoStrip.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabel1, ComputerSourceLabel, ToolStripStatusLabel2, UserSourceLabel });
            InfoStrip.Location = new Point(0, 352);
            InfoStrip.Name = "InfoStrip";
            InfoStrip.Size = new Size(706, 22);
            InfoStrip.TabIndex = 2;
            InfoStrip.Text = "StatusStrip1";
            // 
            // ComputerSourceLabel
            // 
            ComputerSourceLabel.Name = "ComputerSourceLabel";
            ComputerSourceLabel.Size = new Size(85, 17);
            ComputerSourceLabel.Text = "Computer info";
            // 
            // UserSourceLabel
            // 
            UserSourceLabel.Name = "UserSourceLabel";
            UserSourceLabel.Size = new Size(54, 17);
            UserSourceLabel.Text = "User info";
            // 
            // PolicyIsPrefTable
            // 
            PolicyIsPrefTable.AutoSize = true;
            PolicyIsPrefTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PolicyIsPrefTable.ColumnCount = 2;
            PolicyIsPrefTable.ColumnStyles.Add(new ColumnStyle());
            PolicyIsPrefTable.ColumnStyles.Add(new ColumnStyle());
            PolicyIsPrefTable.Controls.Add(PictureBox1, 0, 0);
            PolicyIsPrefTable.Controls.Add(PolicyIsPrefLabel, 1, 0);
            PolicyIsPrefTable.Location = new Point(3, 77);
            PolicyIsPrefTable.Margin = new Padding(3, 3, 0, 24);
            PolicyIsPrefTable.Name = "PolicyIsPrefTable";
            PolicyIsPrefTable.RowCount = 1;
            PolicyIsPrefTable.RowStyles.Add(new RowStyle());
            PolicyIsPrefTable.Size = new Size(703, 22);
            PolicyIsPrefTable.TabIndex = 4;
            // 
            // PictureBox1
            // 
            PictureBox1.Image = (Image)resources.GetObject("PictureBox1.Image");
            PictureBox1.Location = new Point(3, 3);
            PictureBox1.Margin = new Padding(3, 3, 0, 3);
            PictureBox1.Name = "PictureBox1";
            PictureBox1.Size = new Size(16, 16);
            PictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            PictureBox1.TabIndex = 0;
            PictureBox1.TabStop = false;
            // 
            // PolicyIsPrefLabel
            // 
            PolicyIsPrefLabel.AutoSize = true;
            PolicyIsPrefLabel.Location = new Point(22, 0);
            PolicyIsPrefLabel.Name = "PolicyIsPrefLabel";
            PolicyIsPrefLabel.Size = new Size(700, 13);
            PolicyIsPrefLabel.TabIndex = 1;
            PolicyIsPrefLabel.Text = "Because it is not stored in a Policies section of the Registry, this policy is a " + "preference and will not be automatically undone if the setting is removed.";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(706, 374);
            Controls.Add(InfoStrip);
            Controls.Add(SplitContainer);
            Controls.Add(MainMenu);
            MainMenuStrip = MainMenu;
            MinimumSize = new Size(600, 400);
            Name = "Main";
            ShowIcon = false;
            Text = "Policy Plus";
            MainMenu.ResumeLayout(false);
            MainMenu.PerformLayout();
            SplitContainer.Panel1.ResumeLayout(false);
            SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainer).EndInit();
            SplitContainer.ResumeLayout(false);
            PolicyObjectContext.ResumeLayout(false);
            SettingInfoPanel.ResumeLayout(false);
            SettingInfoPanel.PerformLayout();
            PolicyInfoTable.ResumeLayout(false);
            PolicyInfoTable.PerformLayout();
            InfoStrip.ResumeLayout(false);
            InfoStrip.PerformLayout();
            PolicyIsPrefTable.ResumeLayout(false);
            PolicyIsPrefTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).EndInit();
            Load += new EventHandler(Main_Load);
            Shown += new EventHandler(Main_Shown);
            SizeChanged += new EventHandler(ResizePolicyNameColumn);
            Closed += new EventHandler(Main_Closed);
            ResumeLayout(false);
            PerformLayout();

        }

        internal MenuStrip MainMenu;
        internal ToolStripMenuItem FileToolStripMenuItem;
        internal ToolStripMenuItem OpenADMXFolderToolStripMenuItem;
        internal ToolStripMenuItem OpenADMXFileToolStripMenuItem;
        internal ToolStripMenuItem CloseADMXWorkspaceToolStripMenuItem;
        internal ToolStripMenuItem ExitToolStripMenuItem;
        internal SplitContainer SplitContainer;
        internal TreeView CategoriesTree;
        internal ListView PoliciesList;
        internal Panel SettingInfoPanel;
        internal ImageList PolicyIcons;
        internal TableLayoutPanel PolicyInfoTable;
        internal Label PolicyTitleLabel;
        internal Label PolicySupportedLabel;
        internal Label PolicyDescLabel;
        internal ColumnHeader ChSettingName;
        internal ToolStripMenuItem ViewToolStripMenuItem;
        internal ToolStripMenuItem EmptyCategoriesToolStripMenuItem;
        internal ComboBox ComboAppliesTo;
        internal ToolStripMenuItem DeduplicatePoliciesToolStripMenuItem;
        internal ToolStripMenuItem FindToolStripMenuItem;
        internal ToolStripMenuItem ByIDToolStripMenuItem;
        internal ToolStripMenuItem OpenPolicyResourcesToolStripMenuItem;
        internal ToolStripMenuItem SavePoliciesToolStripMenuItem;
        internal ToolStripMenuItem HelpToolStripMenuItem;
        internal ToolStripMenuItem AboutToolStripMenuItem;
        internal ToolStripMenuItem ByTextToolStripMenuItem;
        internal ToolStripMenuItem ByRegistryToolStripMenuItem;
        internal ToolStripMenuItem SearchResultsToolStripMenuItem;
        internal ToolStripMenuItem FindNextToolStripMenuItem;
        internal ContextMenuStrip PolicyObjectContext;
        internal ToolStripMenuItem CmeCatOpen;
        internal ToolStripMenuItem CmePolEdit;
        internal ToolStripMenuItem CmeAllDetails;
        internal ToolStripMenuItem CmePolInspectElements;
        internal ToolStripMenuItem OnlyFilteredObjectsToolStripMenuItem;
        internal ToolStripMenuItem FilterOptionsToolStripMenuItem;
        internal ToolStripMenuItem ShareToolStripMenuItem;
        internal ToolStripMenuItem ImportSemanticPolicyToolStripMenuItem;
        internal ToolStripMenuItem ImportPOLToolStripMenuItem;
        internal ToolStripMenuItem ExportPOLToolStripMenuItem;
        internal ToolStripMenuItem CmePolSpolFragment;
        internal ToolStripMenuItem AcquireADMXFilesToolStripMenuItem;
        internal StatusStrip InfoStrip;
        internal ToolStripStatusLabel ComputerSourceLabel;
        internal ToolStripStatusLabel UserSourceLabel;
        internal ToolStripMenuItem LoadedADMXFilesToolStripMenuItem;
        internal ToolStripMenuItem AllSupportDefinitionsToolStripMenuItem;
        internal ToolStripMenuItem AllProductsToolStripMenuItem;
        internal ToolStripMenuItem EditRawPOLToolStripMenuItem;
        internal ToolStripMenuItem ExportREGToolStripMenuItem;
        internal ToolStripMenuItem ImportREGToolStripMenuItem;
        internal ToolStripMenuItem SetADMLLanguageToolStripMenuItem;
        internal TableLayoutPanel PolicyIsPrefTable;
        internal PictureBox PictureBox1;
        internal Label PolicyIsPrefLabel;
    }
}