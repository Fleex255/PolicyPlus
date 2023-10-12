using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;

namespace PolicyPlus
{
    public partial class DetailAdmx
    {
        public DetailAdmx()
        {
            InitializeComponent();
        }
        public void PresentDialog(AdmxFile Admx, AdmxBundle Workspace)
        {
            TextPath.Text = Admx.SourceFile;
            TextNamespace.Text = Admx.AdmxNamespace;
            TextSupersededAdm.Text = Admx.SupersededAdm;
            void fillListview(ListView Control, IEnumerable Collection, Func<object, string> IdSelector, Func<object, string> NameSelector)
            {
                Control.Items.Clear();
                foreach (var item in Collection)
                {
                    var lsvi = Control.Items.Add(IdSelector(item));
                    lsvi.Tag = item;
                    lsvi.SubItems.Add(NameSelector(item));
                }
                Control.Columns[1].Width = Control.ClientRectangle.Width - Control.Columns[0].Width - SystemInformation.VerticalScrollBarWidth;
            };
            fillListview(LsvPolicies, Workspace.Policies.Values.Where(p => ReferenceEquals(p.RawPolicy.DefinedIn, Admx)), (PolicyPlusPolicy p) => p.RawPolicy.ID, (PolicyPlusPolicy p) => p.DisplayName);
            fillListview(LsvCategories, Workspace.FlatCategories.Values.Where(c => ReferenceEquals(c.RawCategory.DefinedIn, Admx)), (PolicyPlusCategory c) => c.RawCategory.ID, (PolicyPlusCategory c) => c.DisplayName);
            fillListview(LsvProducts, Workspace.FlatProducts.Values.Where(p => ReferenceEquals(p.RawProduct.DefinedIn, Admx)), (PolicyPlusProduct p) => p.RawProduct.ID, (PolicyPlusProduct p) => p.DisplayName);
            fillListview(LsvSupportDefinitions, Workspace.SupportDefinitions.Values.Where(s => ReferenceEquals(s.RawSupport.DefinedIn, Admx)), (PolicyPlusSupport s) => s.RawSupport.ID, (PolicyPlusSupport s) => s.DisplayName);
            ShowDialog();
        }
        private void LsvPolicies_DoubleClick(object sender, EventArgs e)
        {
            My.MyProject.Forms.DetailPolicy.PresentDialog((PolicyPlusPolicy)LsvPolicies.SelectedItems[0].Tag);
        }
        private void LsvCategories_DoubleClick(object sender, EventArgs e)
        {
            My.MyProject.Forms.DetailCategory.PresentDialog((PolicyPlusCategory)LsvCategories.SelectedItems[0].Tag);
        }
        private void LsvProducts_DoubleClick(object sender, EventArgs e)
        {
            My.MyProject.Forms.DetailProduct.PresentDialog((PolicyPlusProduct)LsvProducts.SelectedItems[0].Tag);
        }
        private void LsvSupportDefinitions_DoubleClick(object sender, EventArgs e)
        {
            My.MyProject.Forms.DetailSupport.PresentDialog((PolicyPlusSupport)LsvSupportDefinitions.SelectedItems[0].Tag);
        }
    }
}