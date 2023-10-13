using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class DetailAdmx
    {
        public DetailAdmx()
        {
            InitializeComponent();
        }

        public void PresentDialog(AdmxFile admx, AdmxBundle workspace)
        {
            TextPath.Text = admx.SourceFile;
            TextNamespace.Text = admx.AdmxNamespace;
            TextSupersededAdm.Text = admx.SupersededAdm;
            FillListview(LsvPolicies,
                workspace.Policies.Values.Where(
                    p => ReferenceEquals(p.RawPolicy.DefinedIn, admx)),
                p => ((PolicyPlusPolicy)p).RawPolicy.Id,
                p => ((PolicyPlusPolicy)p).DisplayName);

            FillListview(LsvCategories,
                workspace.FlatCategories.Values.Where(c => ReferenceEquals(c.RawCategory.DefinedIn, admx)),
                c => ((PolicyPlusCategory)c).RawCategory.Id,
                c => ((PolicyPlusCategory)c).DisplayName);

            FillListview(LsvProducts,
                workspace.FlatProducts.Values.Where(p => ReferenceEquals(p.RawProduct.DefinedIn, admx)),
                p => ((PolicyPlusProduct)p).RawProduct.Id,
                p => ((PolicyPlusProduct)p).DisplayName);

            FillListview(LsvSupportDefinitions,
                workspace.SupportDefinitions.Values.Where(s => ReferenceEquals(s.RawSupport.DefinedIn, admx)),
                s => ((PolicyPlusSupport)s).RawSupport.Id,
                s => ((PolicyPlusSupport)s).DisplayName);

            _ = ShowDialog(this);
        }

        private static void FillListview(ListView control, IEnumerable collection, Func<object, string> idSelector, Func<object, string> nameSelector)
        {
            control.Items.Clear();
            foreach (var item in collection)
            {
                var lsvi = control.Items.Add(idSelector(item));
                lsvi.Tag = item;
                _ = lsvi.SubItems.Add(nameSelector(item));
            }
            control.Columns[1].Width = control.ClientRectangle.Width - control.Columns[0].Width - SystemInformation.VerticalScrollBarWidth;
        }

        private void LsvPolicies_DoubleClick(object sender, EventArgs e) => new DetailPolicy().PresentDialog((PolicyPlusPolicy)LsvPolicies.SelectedItems[0].Tag);

        private void LsvCategories_DoubleClick(object sender, EventArgs e) => new DetailCategory().PresentDialog((PolicyPlusCategory)LsvCategories.SelectedItems[0].Tag);

        private void LsvProducts_DoubleClick(object sender, EventArgs e) => new DetailProduct().PresentDialog((PolicyPlusProduct)LsvProducts.SelectedItems[0].Tag);

        private void LsvSupportDefinitions_DoubleClick(object sender, EventArgs e) => new DetailSupport().PresentDialog((PolicyPlusSupport)LsvSupportDefinitions.SelectedItems[0].Tag);
    }
}