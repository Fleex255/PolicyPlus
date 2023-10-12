using System;
using System.Linq;
using System.Windows.Forms;

namespace PolicyPlus
{
    public partial class LoadedProducts
    {
        public LoadedProducts()
        {
            InitializeComponent();
        }
        public void PresentDialog(AdmxBundle Workspace)
        {
            // Fill the top-level products list
            LsvTopLevelProducts.SelectedIndices.Clear();
            LsvTopLevelProducts.Items.Clear();
            foreach (var product in Workspace.Products.Values.OrderBy(p => p.DisplayName))
            {
                var lsvi = LsvTopLevelProducts.Items.Add(product.DisplayName);
                lsvi.SubItems.Add(product.Children.Count.ToString());
                lsvi.Tag = product;
            }
            // Clear the other lists
            UpdateMajorList();
            // Finagle the column widths
            foreach (var lsv in new[] { LsvTopLevelProducts, LsvMajorVersions, LsvMinorVersions })
            {
                int lastColWidths = 0;
                for (int n = 1, loopTo = lsv.Columns.Count - 1; n <= loopTo; n++)
                    lastColWidths += lsv.Columns[n].Width;
                lsv.Columns[0].Width = lsv.ClientRectangle.Width - lastColWidths - SystemInformation.VerticalScrollBarWidth;
            }
            ShowDialog();
        }
        public void UpdateMajorList()
        {
            // Show the major versions of the selected top-level product
            LsvMajorVersions.SelectedIndices.Clear();
            LsvMajorVersions.Items.Clear();
            if (LsvTopLevelProducts.SelectedItems.Count > 0)
            {
                PolicyPlusProduct selProduct = (PolicyPlusProduct)LsvTopLevelProducts.SelectedItems[0].Tag;
                foreach (var product in selProduct.Children.OrderBy(p => p.RawProduct.Version))
                {
                    var lsvi = LsvMajorVersions.Items.Add(product.DisplayName);
                    lsvi.SubItems.Add(product.RawProduct.Version.ToString());
                    lsvi.SubItems.Add(product.Children.Count.ToString());
                    lsvi.Tag = product;
                }
                LabelMajorVersion.Text = "Major versions of \"" + selProduct.DisplayName + "\"";
            }
            else
            {
                LabelMajorVersion.Text = "Select a product to show its major versions";
            }
            UpdateMinorList();
        }
        public void UpdateMinorList()
        {
            LsvMinorVersions.SelectedIndices.Clear();
            LsvMinorVersions.Items.Clear();
            // Show the minor versions of the selected major version
            if (LsvMajorVersions.SelectedItems.Count > 0)
            {
                PolicyPlusProduct selProduct = (PolicyPlusProduct)LsvMajorVersions.SelectedItems[0].Tag;
                foreach (var product in selProduct.Children.OrderBy(p => p.RawProduct.Version))
                {
                    var lsvi = LsvMinorVersions.Items.Add(product.DisplayName);
                    lsvi.SubItems.Add(product.RawProduct.Version.ToString());
                    lsvi.Tag = product;
                }
                LabelMinorVersion.Text = "Minor versions of \"" + selProduct.DisplayName + "\"";
            }
            else
            {
                LabelMinorVersion.Text = "Select a major version to show its minor versions";
            }
        }
        public void OpenProductDetails(object sender, EventArgs e)
        {
            ListView lsv = (ListView)sender;
            if (lsv.SelectedItems.Count == 0)
                return;
            PolicyPlusProduct product = (PolicyPlusProduct)lsv.SelectedItems[0].Tag;
            My.MyProject.Forms.DetailProduct.PresentDialog(product);
        }
        public void ListKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenProductDetails(sender, e);
                e.Handled = true;
            }
        }

        private void UpdateMajorList(object sender, EventArgs e) => UpdateMajorList();
        private void UpdateMinorList(object sender, EventArgs e) => UpdateMinorList();
    }
}