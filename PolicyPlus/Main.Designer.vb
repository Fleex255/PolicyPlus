<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ChSettingEnabled As System.Windows.Forms.ColumnHeader
        Dim ChSettingCommented As System.Windows.Forms.ColumnHeader
        Dim ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
        Dim ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
        Dim ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenADMXFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenADMXFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetADMLLanguageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseADMXWorkspaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenPolicyResourcesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SavePoliciesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditRawPOLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EmptyCategoriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OnlyFilteredObjectsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FilterOptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeduplicatePoliciesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadedADMXFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AllProductsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AllSupportDefinitionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FindToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ByIDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ByTextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ByRegistryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SearchResultsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FindNextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShareToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportSemanticPolicyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportPOLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportREGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportPOLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportREGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AcquireADMXFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.ComboAppliesTo = New System.Windows.Forms.ComboBox()
        Me.CategoriesTree = New System.Windows.Forms.TreeView()
        Me.PolicyObjectContext = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CmeCatOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.CmePolEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.CmeAllDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.CmePolInspectElements = New System.Windows.Forms.ToolStripMenuItem()
        Me.CmePolSpolFragment = New System.Windows.Forms.ToolStripMenuItem()
        Me.PolicyIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.PoliciesList = New System.Windows.Forms.ListView()
        Me.ChSettingName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SettingInfoPanel = New System.Windows.Forms.Panel()
        Me.PolicyInfoTable = New System.Windows.Forms.TableLayoutPanel()
        Me.PolicyTitleLabel = New System.Windows.Forms.Label()
        Me.PolicySupportedLabel = New System.Windows.Forms.Label()
        Me.PolicyDescLabel = New System.Windows.Forms.Label()
        Me.InfoStrip = New System.Windows.Forms.StatusStrip()
        Me.ComputerSourceLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.UserSourceLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PolicyIsPrefTable = New System.Windows.Forms.TableLayoutPanel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PolicyIsPrefLabel = New System.Windows.Forms.Label()
        ChSettingEnabled = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        ChSettingCommented = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.MainMenu.SuspendLayout()
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer.Panel1.SuspendLayout()
        Me.SplitContainer.Panel2.SuspendLayout()
        Me.SplitContainer.SuspendLayout()
        Me.PolicyObjectContext.SuspendLayout()
        Me.SettingInfoPanel.SuspendLayout()
        Me.PolicyInfoTable.SuspendLayout()
        Me.InfoStrip.SuspendLayout()
        Me.PolicyIsPrefTable.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ChSettingEnabled
        '
        ChSettingEnabled.Text = "State"
        ChSettingEnabled.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        ChSettingEnabled.Width = 107
        '
        'ChSettingCommented
        '
        ChSettingCommented.Text = "Comment"
        ChSettingCommented.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        ChSettingCommented.Width = 68
        '
        'ToolStripSeparator1
        '
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New System.Drawing.Size(190, 6)
        '
        'ToolStripSeparator2
        '
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New System.Drawing.Size(234, 6)
        '
        'ToolStripSeparator3
        '
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New System.Drawing.Size(234, 6)
        '
        'ToolStripSeparator4
        '
        ToolStripSeparator4.Name = "ToolStripSeparator4"
        ToolStripSeparator4.Size = New System.Drawing.Size(197, 6)
        '
        'ToolStripSeparator5
        '
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Size = New System.Drawing.Size(194, 6)
        '
        'ToolStripStatusLabel1
        '
        ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        ToolStripStatusLabel1.Size = New System.Drawing.Size(102, 17)
        ToolStripStatusLabel1.Text = "Computer source:"
        '
        'ToolStripStatusLabel2
        '
        ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        ToolStripStatusLabel2.Size = New System.Drawing.Size(71, 17)
        ToolStripStatusLabel2.Text = "User source:"
        '
        'ToolStripSeparator6
        '
        ToolStripSeparator6.Name = "ToolStripSeparator6"
        ToolStripSeparator6.Size = New System.Drawing.Size(190, 6)
        '
        'MainMenu
        '
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ViewToolStripMenuItem, Me.FindToolStripMenuItem, Me.ShareToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(706, 24)
        Me.MainMenu.TabIndex = 0
        Me.MainMenu.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenADMXFolderToolStripMenuItem, Me.OpenADMXFileToolStripMenuItem, Me.SetADMLLanguageToolStripMenuItem, Me.CloseADMXWorkspaceToolStripMenuItem, ToolStripSeparator2, Me.OpenPolicyResourcesToolStripMenuItem, Me.SavePoliciesToolStripMenuItem, Me.EditRawPOLToolStripMenuItem, ToolStripSeparator3, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenADMXFolderToolStripMenuItem
        '
        Me.OpenADMXFolderToolStripMenuItem.Name = "OpenADMXFolderToolStripMenuItem"
        Me.OpenADMXFolderToolStripMenuItem.Size = New System.Drawing.Size(237, 22)
        Me.OpenADMXFolderToolStripMenuItem.Text = "Open ADMX Folder"
        '
        'OpenADMXFileToolStripMenuItem
        '
        Me.OpenADMXFileToolStripMenuItem.Name = "OpenADMXFileToolStripMenuItem"
        Me.OpenADMXFileToolStripMenuItem.Size = New System.Drawing.Size(237, 22)
        Me.OpenADMXFileToolStripMenuItem.Text = "Open ADMX File"
        '
        'SetADMLLanguageToolStripMenuItem
        '
        Me.SetADMLLanguageToolStripMenuItem.Name = "SetADMLLanguageToolStripMenuItem"
        Me.SetADMLLanguageToolStripMenuItem.Size = New System.Drawing.Size(237, 22)
        Me.SetADMLLanguageToolStripMenuItem.Text = "Set ADML Language"
        '
        'CloseADMXWorkspaceToolStripMenuItem
        '
        Me.CloseADMXWorkspaceToolStripMenuItem.Name = "CloseADMXWorkspaceToolStripMenuItem"
        Me.CloseADMXWorkspaceToolStripMenuItem.Size = New System.Drawing.Size(237, 22)
        Me.CloseADMXWorkspaceToolStripMenuItem.Text = "Close ADMX Workspace"
        '
        'OpenPolicyResourcesToolStripMenuItem
        '
        Me.OpenPolicyResourcesToolStripMenuItem.Name = "OpenPolicyResourcesToolStripMenuItem"
        Me.OpenPolicyResourcesToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenPolicyResourcesToolStripMenuItem.Size = New System.Drawing.Size(237, 22)
        Me.OpenPolicyResourcesToolStripMenuItem.Text = "Open Policy Resources"
        '
        'SavePoliciesToolStripMenuItem
        '
        Me.SavePoliciesToolStripMenuItem.Name = "SavePoliciesToolStripMenuItem"
        Me.SavePoliciesToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SavePoliciesToolStripMenuItem.Size = New System.Drawing.Size(237, 22)
        Me.SavePoliciesToolStripMenuItem.Text = "Save Policies"
        '
        'EditRawPOLToolStripMenuItem
        '
        Me.EditRawPOLToolStripMenuItem.Name = "EditRawPOLToolStripMenuItem"
        Me.EditRawPOLToolStripMenuItem.Size = New System.Drawing.Size(237, 22)
        Me.EditRawPOLToolStripMenuItem.Text = "Edit Raw POL"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(237, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EmptyCategoriesToolStripMenuItem, Me.OnlyFilteredObjectsToolStripMenuItem, ToolStripSeparator1, Me.FilterOptionsToolStripMenuItem, Me.DeduplicatePoliciesToolStripMenuItem, ToolStripSeparator6, Me.LoadedADMXFilesToolStripMenuItem, Me.AllProductsToolStripMenuItem, Me.AllSupportDefinitionsToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'EmptyCategoriesToolStripMenuItem
        '
        Me.EmptyCategoriesToolStripMenuItem.Name = "EmptyCategoriesToolStripMenuItem"
        Me.EmptyCategoriesToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.EmptyCategoriesToolStripMenuItem.Text = "Empty Categories"
        '
        'OnlyFilteredObjectsToolStripMenuItem
        '
        Me.OnlyFilteredObjectsToolStripMenuItem.Name = "OnlyFilteredObjectsToolStripMenuItem"
        Me.OnlyFilteredObjectsToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.OnlyFilteredObjectsToolStripMenuItem.Text = "Only Filtered Policies"
        '
        'FilterOptionsToolStripMenuItem
        '
        Me.FilterOptionsToolStripMenuItem.Name = "FilterOptionsToolStripMenuItem"
        Me.FilterOptionsToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.FilterOptionsToolStripMenuItem.Text = "Filter Options"
        '
        'DeduplicatePoliciesToolStripMenuItem
        '
        Me.DeduplicatePoliciesToolStripMenuItem.Name = "DeduplicatePoliciesToolStripMenuItem"
        Me.DeduplicatePoliciesToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.DeduplicatePoliciesToolStripMenuItem.Text = "Deduplicate Policies"
        Me.DeduplicatePoliciesToolStripMenuItem.Visible = False
        '
        'LoadedADMXFilesToolStripMenuItem
        '
        Me.LoadedADMXFilesToolStripMenuItem.Name = "LoadedADMXFilesToolStripMenuItem"
        Me.LoadedADMXFilesToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.LoadedADMXFilesToolStripMenuItem.Text = "Loaded ADMX Files"
        '
        'AllProductsToolStripMenuItem
        '
        Me.AllProductsToolStripMenuItem.Name = "AllProductsToolStripMenuItem"
        Me.AllProductsToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.AllProductsToolStripMenuItem.Text = "All Products"
        '
        'AllSupportDefinitionsToolStripMenuItem
        '
        Me.AllSupportDefinitionsToolStripMenuItem.Name = "AllSupportDefinitionsToolStripMenuItem"
        Me.AllSupportDefinitionsToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.AllSupportDefinitionsToolStripMenuItem.Text = "All Support Definitions"
        '
        'FindToolStripMenuItem
        '
        Me.FindToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ByIDToolStripMenuItem, Me.ByTextToolStripMenuItem, Me.ByRegistryToolStripMenuItem, ToolStripSeparator4, Me.SearchResultsToolStripMenuItem, Me.FindNextToolStripMenuItem})
        Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
        Me.FindToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.FindToolStripMenuItem.Text = "Find"
        '
        'ByIDToolStripMenuItem
        '
        Me.ByIDToolStripMenuItem.Name = "ByIDToolStripMenuItem"
        Me.ByIDToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.G), System.Windows.Forms.Keys)
        Me.ByIDToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.ByIDToolStripMenuItem.Text = "By ID"
        '
        'ByTextToolStripMenuItem
        '
        Me.ByTextToolStripMenuItem.Name = "ByTextToolStripMenuItem"
        Me.ByTextToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.ByTextToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.ByTextToolStripMenuItem.Text = "By Text"
        '
        'ByRegistryToolStripMenuItem
        '
        Me.ByRegistryToolStripMenuItem.Name = "ByRegistryToolStripMenuItem"
        Me.ByRegistryToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.ByRegistryToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.ByRegistryToolStripMenuItem.Text = "By Registry"
        '
        'SearchResultsToolStripMenuItem
        '
        Me.SearchResultsToolStripMenuItem.Name = "SearchResultsToolStripMenuItem"
        Me.SearchResultsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F3), System.Windows.Forms.Keys)
        Me.SearchResultsToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.SearchResultsToolStripMenuItem.Text = "Search Results"
        '
        'FindNextToolStripMenuItem
        '
        Me.FindNextToolStripMenuItem.Name = "FindNextToolStripMenuItem"
        Me.FindNextToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.FindNextToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.FindNextToolStripMenuItem.Text = "Find Next"
        '
        'ShareToolStripMenuItem
        '
        Me.ShareToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImportSemanticPolicyToolStripMenuItem, Me.ImportPOLToolStripMenuItem, Me.ImportREGToolStripMenuItem, ToolStripSeparator5, Me.ExportPOLToolStripMenuItem, Me.ExportREGToolStripMenuItem})
        Me.ShareToolStripMenuItem.Name = "ShareToolStripMenuItem"
        Me.ShareToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.ShareToolStripMenuItem.Text = "Share"
        '
        'ImportSemanticPolicyToolStripMenuItem
        '
        Me.ImportSemanticPolicyToolStripMenuItem.Name = "ImportSemanticPolicyToolStripMenuItem"
        Me.ImportSemanticPolicyToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.ImportSemanticPolicyToolStripMenuItem.Text = "Import Semantic Policy"
        '
        'ImportPOLToolStripMenuItem
        '
        Me.ImportPOLToolStripMenuItem.Name = "ImportPOLToolStripMenuItem"
        Me.ImportPOLToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.ImportPOLToolStripMenuItem.Text = "Import POL"
        '
        'ImportREGToolStripMenuItem
        '
        Me.ImportREGToolStripMenuItem.Name = "ImportREGToolStripMenuItem"
        Me.ImportREGToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.ImportREGToolStripMenuItem.Text = "Import REG"
        '
        'ExportPOLToolStripMenuItem
        '
        Me.ExportPOLToolStripMenuItem.Name = "ExportPOLToolStripMenuItem"
        Me.ExportPOLToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.ExportPOLToolStripMenuItem.Text = "Export POL"
        '
        'ExportREGToolStripMenuItem
        '
        Me.ExportREGToolStripMenuItem.Name = "ExportREGToolStripMenuItem"
        Me.ExportREGToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.ExportREGToolStripMenuItem.Text = "Export REG"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.AcquireADMXFilesToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(178, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'AcquireADMXFilesToolStripMenuItem
        '
        Me.AcquireADMXFilesToolStripMenuItem.Name = "AcquireADMXFilesToolStripMenuItem"
        Me.AcquireADMXFilesToolStripMenuItem.Size = New System.Drawing.Size(178, 22)
        Me.AcquireADMXFilesToolStripMenuItem.Text = "Acquire ADMX Files"
        '
        'SplitContainer
        '
        Me.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer.Name = "SplitContainer"
        '
        'SplitContainer.Panel1
        '
        Me.SplitContainer.Panel1.Controls.Add(Me.ComboAppliesTo)
        Me.SplitContainer.Panel1.Controls.Add(Me.CategoriesTree)
        '
        'SplitContainer.Panel2
        '
        Me.SplitContainer.Panel2.BackColor = System.Drawing.Color.White
        Me.SplitContainer.Panel2.Controls.Add(Me.PoliciesList)
        Me.SplitContainer.Panel2.Controls.Add(Me.SettingInfoPanel)
        Me.SplitContainer.Size = New System.Drawing.Size(706, 350)
        Me.SplitContainer.SplitterDistance = 190
        Me.SplitContainer.TabIndex = 1
        Me.SplitContainer.TabStop = False
        '
        'ComboAppliesTo
        '
        Me.ComboAppliesTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboAppliesTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboAppliesTo.Items.AddRange(New Object() {"User or Computer", "User", "Computer"})
        Me.ComboAppliesTo.Location = New System.Drawing.Point(0, 0)
        Me.ComboAppliesTo.Name = "ComboAppliesTo"
        Me.ComboAppliesTo.Size = New System.Drawing.Size(190, 21)
        Me.ComboAppliesTo.TabIndex = 1
        '
        'CategoriesTree
        '
        Me.CategoriesTree.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CategoriesTree.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.CategoriesTree.ContextMenuStrip = Me.PolicyObjectContext
        Me.CategoriesTree.HideSelection = False
        Me.CategoriesTree.ImageIndex = 0
        Me.CategoriesTree.ImageList = Me.PolicyIcons
        Me.CategoriesTree.Location = New System.Drawing.Point(0, 19)
        Me.CategoriesTree.Name = "CategoriesTree"
        Me.CategoriesTree.SelectedImageIndex = 0
        Me.CategoriesTree.ShowNodeToolTips = True
        Me.CategoriesTree.Size = New System.Drawing.Size(190, 331)
        Me.CategoriesTree.TabIndex = 2
        '
        'PolicyObjectContext
        '
        Me.PolicyObjectContext.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CmeCatOpen, Me.CmePolEdit, Me.CmeAllDetails, Me.CmePolInspectElements, Me.CmePolSpolFragment})
        Me.PolicyObjectContext.Name = "PolicyObjectContext"
        Me.PolicyObjectContext.Size = New System.Drawing.Size(213, 114)
        '
        'CmeCatOpen
        '
        Me.CmeCatOpen.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmeCatOpen.Name = "CmeCatOpen"
        Me.CmeCatOpen.Size = New System.Drawing.Size(212, 22)
        Me.CmeCatOpen.Tag = "C"
        Me.CmeCatOpen.Text = "Open"
        '
        'CmePolEdit
        '
        Me.CmePolEdit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmePolEdit.Name = "CmePolEdit"
        Me.CmePolEdit.Size = New System.Drawing.Size(212, 22)
        Me.CmePolEdit.Tag = "P"
        Me.CmePolEdit.Text = "Edit"
        '
        'CmeAllDetails
        '
        Me.CmeAllDetails.Name = "CmeAllDetails"
        Me.CmeAllDetails.Size = New System.Drawing.Size(212, 22)
        Me.CmeAllDetails.Text = "Details"
        '
        'CmePolInspectElements
        '
        Me.CmePolInspectElements.Name = "CmePolInspectElements"
        Me.CmePolInspectElements.Size = New System.Drawing.Size(212, 22)
        Me.CmePolInspectElements.Tag = "P"
        Me.CmePolInspectElements.Text = "Element Inspector"
        '
        'CmePolSpolFragment
        '
        Me.CmePolSpolFragment.Name = "CmePolSpolFragment"
        Me.CmePolSpolFragment.Size = New System.Drawing.Size(212, 22)
        Me.CmePolSpolFragment.Tag = "P"
        Me.CmePolSpolFragment.Text = "Semantic Policy Fragment"
        '
        'PolicyIcons
        '
        Me.PolicyIcons.ImageStream = CType(resources.GetObject("PolicyIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.PolicyIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.PolicyIcons.Images.SetKeyName(0, "folder.png")
        Me.PolicyIcons.Images.SetKeyName(1, "folder_error.png")
        Me.PolicyIcons.Images.SetKeyName(2, "folder_delete.png")
        Me.PolicyIcons.Images.SetKeyName(3, "folder_go.png")
        Me.PolicyIcons.Images.SetKeyName(4, "page_white.png")
        Me.PolicyIcons.Images.SetKeyName(5, "page_white_gear.png")
        Me.PolicyIcons.Images.SetKeyName(6, "arrow_up.png")
        Me.PolicyIcons.Images.SetKeyName(7, "page_white_error.png")
        Me.PolicyIcons.Images.SetKeyName(8, "delete.png")
        Me.PolicyIcons.Images.SetKeyName(9, "arrow_right.png")
        Me.PolicyIcons.Images.SetKeyName(10, "package.png")
        Me.PolicyIcons.Images.SetKeyName(11, "computer.png")
        Me.PolicyIcons.Images.SetKeyName(12, "database.png")
        Me.PolicyIcons.Images.SetKeyName(13, "cog.png")
        Me.PolicyIcons.Images.SetKeyName(14, "text_allcaps.png")
        Me.PolicyIcons.Images.SetKeyName(15, "calculator.png")
        Me.PolicyIcons.Images.SetKeyName(16, "cog_edit.png")
        Me.PolicyIcons.Images.SetKeyName(17, "accept.png")
        Me.PolicyIcons.Images.SetKeyName(18, "cross.png")
        Me.PolicyIcons.Images.SetKeyName(19, "application_xp_terminal.png")
        Me.PolicyIcons.Images.SetKeyName(20, "application_form.png")
        Me.PolicyIcons.Images.SetKeyName(21, "text_align_left.png")
        Me.PolicyIcons.Images.SetKeyName(22, "calculator_edit.png")
        Me.PolicyIcons.Images.SetKeyName(23, "wrench.png")
        Me.PolicyIcons.Images.SetKeyName(24, "textfield.png")
        Me.PolicyIcons.Images.SetKeyName(25, "tick.png")
        Me.PolicyIcons.Images.SetKeyName(26, "text_horizontalrule.png")
        Me.PolicyIcons.Images.SetKeyName(27, "table.png")
        Me.PolicyIcons.Images.SetKeyName(28, "table_sort.png")
        Me.PolicyIcons.Images.SetKeyName(29, "font_go.png")
        Me.PolicyIcons.Images.SetKeyName(30, "application_view_list.png")
        Me.PolicyIcons.Images.SetKeyName(31, "brick.png")
        Me.PolicyIcons.Images.SetKeyName(32, "error.png")
        Me.PolicyIcons.Images.SetKeyName(33, "style.png")
        Me.PolicyIcons.Images.SetKeyName(34, "sound_low.png")
        Me.PolicyIcons.Images.SetKeyName(35, "arrow_down.png")
        Me.PolicyIcons.Images.SetKeyName(36, "style_go.png")
        Me.PolicyIcons.Images.SetKeyName(37, "exclamation.png")
        Me.PolicyIcons.Images.SetKeyName(38, "application_cascade.png")
        Me.PolicyIcons.Images.SetKeyName(39, "page_copy.png")
        Me.PolicyIcons.Images.SetKeyName(40, "page.png")
        Me.PolicyIcons.Images.SetKeyName(41, "calculator_add.png")
        Me.PolicyIcons.Images.SetKeyName(42, "page_go.png")
        '
        'PoliciesList
        '
        Me.PoliciesList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PoliciesList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.PoliciesList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChSettingName, ChSettingEnabled, ChSettingCommented})
        Me.PoliciesList.ContextMenuStrip = Me.PolicyObjectContext
        Me.PoliciesList.FullRowSelect = True
        Me.PoliciesList.HideSelection = False
        Me.PoliciesList.Location = New System.Drawing.Point(190, 0)
        Me.PoliciesList.MultiSelect = False
        Me.PoliciesList.Name = "PoliciesList"
        Me.PoliciesList.ShowItemToolTips = True
        Me.PoliciesList.Size = New System.Drawing.Size(322, 350)
        Me.PoliciesList.SmallImageList = Me.PolicyIcons
        Me.PoliciesList.TabIndex = 3
        Me.PoliciesList.UseCompatibleStateImageBehavior = False
        Me.PoliciesList.View = System.Windows.Forms.View.Details
        '
        'ChSettingName
        '
        Me.ChSettingName.Text = "Name"
        Me.ChSettingName.Width = 116
        '
        'SettingInfoPanel
        '
        Me.SettingInfoPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SettingInfoPanel.AutoScroll = True
        Me.SettingInfoPanel.Controls.Add(Me.PolicyInfoTable)
        Me.SettingInfoPanel.Location = New System.Drawing.Point(0, 0)
        Me.SettingInfoPanel.Name = "SettingInfoPanel"
        Me.SettingInfoPanel.Size = New System.Drawing.Size(184, 350)
        Me.SettingInfoPanel.TabIndex = 0
        '
        'PolicyInfoTable
        '
        Me.PolicyInfoTable.AutoSize = True
        Me.PolicyInfoTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PolicyInfoTable.ColumnCount = 1
        Me.PolicyInfoTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 706.0!))
        Me.PolicyInfoTable.Controls.Add(Me.PolicyTitleLabel, 0, 0)
        Me.PolicyInfoTable.Controls.Add(Me.PolicySupportedLabel, 0, 1)
        Me.PolicyInfoTable.Controls.Add(Me.PolicyDescLabel, 0, 3)
        Me.PolicyInfoTable.Controls.Add(Me.PolicyIsPrefTable, 0, 2)
        Me.PolicyInfoTable.Location = New System.Drawing.Point(0, 0)
        Me.PolicyInfoTable.Name = "PolicyInfoTable"
        Me.PolicyInfoTable.RowCount = 5
        Me.PolicyInfoTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PolicyInfoTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PolicyInfoTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PolicyInfoTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PolicyInfoTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.PolicyInfoTable.Size = New System.Drawing.Size(706, 156)
        Me.PolicyInfoTable.TabIndex = 0
        '
        'PolicyTitleLabel
        '
        Me.PolicyTitleLabel.AutoSize = True
        Me.PolicyTitleLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PolicyTitleLabel.Location = New System.Drawing.Point(3, 0)
        Me.PolicyTitleLabel.Margin = New System.Windows.Forms.Padding(3, 0, 3, 24)
        Me.PolicyTitleLabel.Name = "PolicyTitleLabel"
        Me.PolicyTitleLabel.Size = New System.Drawing.Size(66, 13)
        Me.PolicyTitleLabel.TabIndex = 0
        Me.PolicyTitleLabel.Text = "Policy title"
        Me.PolicyTitleLabel.UseMnemonic = False
        '
        'PolicySupportedLabel
        '
        Me.PolicySupportedLabel.AutoSize = True
        Me.PolicySupportedLabel.Location = New System.Drawing.Point(3, 37)
        Me.PolicySupportedLabel.Margin = New System.Windows.Forms.Padding(3, 0, 3, 24)
        Me.PolicySupportedLabel.Name = "PolicySupportedLabel"
        Me.PolicySupportedLabel.Size = New System.Drawing.Size(72, 13)
        Me.PolicySupportedLabel.TabIndex = 1
        Me.PolicySupportedLabel.Text = "Requirements"
        Me.PolicySupportedLabel.UseMnemonic = False
        '
        'PolicyDescLabel
        '
        Me.PolicyDescLabel.AutoSize = True
        Me.PolicyDescLabel.Location = New System.Drawing.Point(3, 123)
        Me.PolicyDescLabel.Name = "PolicyDescLabel"
        Me.PolicyDescLabel.Size = New System.Drawing.Size(89, 13)
        Me.PolicyDescLabel.TabIndex = 2
        Me.PolicyDescLabel.Text = "Policy description"
        Me.PolicyDescLabel.UseMnemonic = False
        '
        'InfoStrip
        '
        Me.InfoStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {ToolStripStatusLabel1, Me.ComputerSourceLabel, ToolStripStatusLabel2, Me.UserSourceLabel})
        Me.InfoStrip.Location = New System.Drawing.Point(0, 352)
        Me.InfoStrip.Name = "InfoStrip"
        Me.InfoStrip.Size = New System.Drawing.Size(706, 22)
        Me.InfoStrip.TabIndex = 2
        Me.InfoStrip.Text = "StatusStrip1"
        '
        'ComputerSourceLabel
        '
        Me.ComputerSourceLabel.Name = "ComputerSourceLabel"
        Me.ComputerSourceLabel.Size = New System.Drawing.Size(85, 17)
        Me.ComputerSourceLabel.Text = "Computer info"
        '
        'UserSourceLabel
        '
        Me.UserSourceLabel.Name = "UserSourceLabel"
        Me.UserSourceLabel.Size = New System.Drawing.Size(54, 17)
        Me.UserSourceLabel.Text = "User info"
        '
        'PolicyIsPrefTable
        '
        Me.PolicyIsPrefTable.AutoSize = True
        Me.PolicyIsPrefTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PolicyIsPrefTable.ColumnCount = 2
        Me.PolicyIsPrefTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.PolicyIsPrefTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.PolicyIsPrefTable.Controls.Add(Me.PictureBox1, 0, 0)
        Me.PolicyIsPrefTable.Controls.Add(Me.PolicyIsPrefLabel, 1, 0)
        Me.PolicyIsPrefTable.Location = New System.Drawing.Point(3, 77)
        Me.PolicyIsPrefTable.Margin = New System.Windows.Forms.Padding(3, 3, 0, 24)
        Me.PolicyIsPrefTable.Name = "PolicyIsPrefTable"
        Me.PolicyIsPrefTable.RowCount = 1
        Me.PolicyIsPrefTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PolicyIsPrefTable.Size = New System.Drawing.Size(703, 22)
        Me.PolicyIsPrefTable.TabIndex = 4
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(3, 3, 0, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'PolicyIsPrefLabel
        '
        Me.PolicyIsPrefLabel.AutoSize = True
        Me.PolicyIsPrefLabel.Location = New System.Drawing.Point(22, 0)
        Me.PolicyIsPrefLabel.Name = "PolicyIsPrefLabel"
        Me.PolicyIsPrefLabel.Size = New System.Drawing.Size(700, 13)
        Me.PolicyIsPrefLabel.TabIndex = 1
        Me.PolicyIsPrefLabel.Text = "Because it is not stored in a Policies section of the Registry, this policy is a " &
    "preference and will not be automatically undone if the setting is removed."
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(706, 374)
        Me.Controls.Add(Me.InfoStrip)
        Me.Controls.Add(Me.SplitContainer)
        Me.Controls.Add(Me.MainMenu)
        Me.MainMenuStrip = Me.MainMenu
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "Main"
        Me.ShowIcon = False
        Me.Text = "Policy Plus"
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.SplitContainer.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer.ResumeLayout(False)
        Me.PolicyObjectContext.ResumeLayout(False)
        Me.SettingInfoPanel.ResumeLayout(False)
        Me.SettingInfoPanel.PerformLayout()
        Me.PolicyInfoTable.ResumeLayout(False)
        Me.PolicyInfoTable.PerformLayout()
        Me.InfoStrip.ResumeLayout(False)
        Me.InfoStrip.PerformLayout()
        Me.PolicyIsPrefTable.ResumeLayout(False)
        Me.PolicyIsPrefTable.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MainMenu As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenADMXFolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenADMXFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseADMXWorkspaceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SplitContainer As SplitContainer
    Friend WithEvents CategoriesTree As TreeView
    Friend WithEvents PoliciesList As ListView
    Friend WithEvents SettingInfoPanel As Panel
    Friend WithEvents PolicyIcons As ImageList
    Friend WithEvents PolicyInfoTable As TableLayoutPanel
    Friend WithEvents PolicyTitleLabel As Label
    Friend WithEvents PolicySupportedLabel As Label
    Friend WithEvents PolicyDescLabel As Label
    Friend WithEvents ChSettingName As ColumnHeader
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EmptyCategoriesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ComboAppliesTo As ComboBox
    Friend WithEvents DeduplicatePoliciesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FindToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ByIDToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenPolicyResourcesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SavePoliciesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ByTextToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ByRegistryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchResultsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FindNextToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PolicyObjectContext As ContextMenuStrip
    Friend WithEvents CmeCatOpen As ToolStripMenuItem
    Friend WithEvents CmePolEdit As ToolStripMenuItem
    Friend WithEvents CmeAllDetails As ToolStripMenuItem
    Friend WithEvents CmePolInspectElements As ToolStripMenuItem
    Friend WithEvents OnlyFilteredObjectsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FilterOptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShareToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportSemanticPolicyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportPOLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportPOLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CmePolSpolFragment As ToolStripMenuItem
    Friend WithEvents AcquireADMXFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents InfoStrip As StatusStrip
    Friend WithEvents ComputerSourceLabel As ToolStripStatusLabel
    Friend WithEvents UserSourceLabel As ToolStripStatusLabel
    Friend WithEvents LoadedADMXFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AllSupportDefinitionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AllProductsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditRawPOLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportREGToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportREGToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SetADMLLanguageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PolicyIsPrefTable As TableLayoutPanel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PolicyIsPrefLabel As Label
End Class
