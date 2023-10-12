using System;
using PolicyPlus.Models;

namespace PolicyPlus.UI
{
    public partial class DetailProduct
    {
        private PolicyPlusProduct _selectedProduct;

        public DetailProduct()
        {
            InitializeComponent();
        }

        public void PresentDialog(PolicyPlusProduct product)
        {
            PrepareDialog(product);
            ShowDialog();
        }

        private void PrepareDialog(PolicyPlusProduct product)
        {
            _selectedProduct = product;
            NameTextbox.Text = product.DisplayName;
            IdTextbox.Text = product.UniqueId;
            DefinedTextbox.Text = product.RawProduct.DefinedIn.SourceFile;
            DisplayCodeTextbox.Text = product.RawProduct.DisplayCode;
            switch (product.RawProduct.Type)
            {
                case AdmxProductType.MajorRevision:
                    {
                        KindTextbox.Text = "Major revision";
                        break;
                    }
                case AdmxProductType.MinorRevision:
                    {
                        KindTextbox.Text = "Minor revision";
                        break;
                    }
                case AdmxProductType.Product:
                    {
                        KindTextbox.Text = "Top-level product";
                        break;
                    }
            }
            VersionTextbox.Text = product.RawProduct.Type == AdmxProductType.Product ? "" : product.RawProduct.Version.ToString();
            if (product.Parent is null)
            {
                ParentTextbox.Text = "";
                ParentButton.Enabled = false;
            }
            else
            {
                ParentTextbox.Text = product.Parent.DisplayName;
                ParentButton.Enabled = true;
            }
            ChildrenListview.Items.Clear();
            if (product.Children is not null)
            {
                foreach (var child in product.Children)
                {
                    var lsvi = ChildrenListview.Items.Add(child.RawProduct.Version.ToString());
                    lsvi.SubItems.Add(child.DisplayName);
                    lsvi.Tag = child;
                }
            }
        }

        private void ChildrenListview_ClientSizeChanged(object sender, EventArgs e) => ChName.Width = ChildrenListview.ClientSize.Width - ChVersion.Width;

        private void ChildrenListview_DoubleClick(object sender, EventArgs e)
        {
            if (ChildrenListview.SelectedItems.Count == 0)
            {
                return;
            }

            PrepareDialog((PolicyPlusProduct)ChildrenListview.SelectedItems[0].Tag);
        }

        private void ParentButton_Click(object sender, EventArgs e) => PrepareDialog(_selectedProduct.Parent);
    }
}