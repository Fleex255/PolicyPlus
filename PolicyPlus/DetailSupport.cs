using System;
using System.Windows.Forms;

namespace PolicyPlus
{
    public partial class DetailSupport
    {
        public DetailSupport()
        {
            InitializeComponent();
        }
        public void PresentDialog(PolicyPlusSupport Supported)
        {
            PrepareDialog(Supported);
            ShowDialog();
        }
        public void PrepareDialog(PolicyPlusSupport Supported)
        {
            NameTextbox.Text = Supported.DisplayName;
            IdTextbox.Text = Supported.UniqueID;
            DefinedTextbox.Text = Supported.RawSupport.DefinedIn.SourceFile;
            DisplayCodeTextbox.Text = Supported.RawSupport.DisplayCode;
            switch (Supported.RawSupport.Logic)
            {
                case AdmxSupportLogicType.AllOf:
                    {
                        LogicTextbox.Text = "Match all the referenced products";
                        break;
                    }
                case AdmxSupportLogicType.AnyOf:
                    {
                        LogicTextbox.Text = "Match any of the referenced products";
                        break;
                    }
                case AdmxSupportLogicType.Blank:
                    {
                        LogicTextbox.Text = "Do not match products";
                        break;
                    }
            }
            EntriesListview.Items.Clear();
            if (Supported.Elements is not null)
            {
                foreach (var element in Supported.Elements)
                {
                    ListViewItem lsvi;
                    if (element.SupportDefinition is not null)
                    {
                        lsvi = EntriesListview.Items.Add(element.SupportDefinition.DisplayName);
                    }
                    else if (element.Product is not null)
                    {
                        lsvi = EntriesListview.Items.Add(element.Product.DisplayName);
                        if (element.RawSupportEntry.IsRange)
                        {
                            if (element.RawSupportEntry.MinVersion.HasValue)
                                lsvi.SubItems.Add(element.RawSupportEntry.MinVersion.Value.ToString());
                            else
                                lsvi.SubItems.Add("");
                            if (element.RawSupportEntry.MaxVersion.HasValue)
                                lsvi.SubItems.Add(element.RawSupportEntry.MaxVersion.Value.ToString());
                            else
                                lsvi.SubItems.Add("");
                        }
                    }
                    else
                    {
                        lsvi = EntriesListview.Items.Add("<missing: " + element.RawSupportEntry.ProductID + ">");
                    }
                    lsvi.Tag = element;
                }
            }
        }
        private void EntriesListview_ClientSizeChanged(object sender, EventArgs e)
        {
            ChName.Width = EntriesListview.ClientSize.Width - ChMinVer.Width - ChMaxVer.Width;
        }
        private void EntriesListview_DoubleClick(object sender, EventArgs e)
        {
            if (EntriesListview.SelectedItems.Count == 0)
                return;
            PolicyPlusSupportEntry supEntry = (PolicyPlusSupportEntry)EntriesListview.SelectedItems[0].Tag;
            if (supEntry.Product is not null)
            {
                My.MyProject.Forms.DetailProduct.PresentDialog(supEntry.Product);
            }
            else if (supEntry.SupportDefinition is not null)
            {
                PrepareDialog(supEntry.SupportDefinition);
            }
        }
    }
}