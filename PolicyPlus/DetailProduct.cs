using System;

namespace PolicyPlus
{
    public partial class DetailProduct
    {
        private PolicyPlusProduct SelectedProduct;

        public DetailProduct()
        {
            InitializeComponent();
        }
        public void PresentDialog(PolicyPlusProduct Product)
        {
            PrepareDialog(Product);
            ShowDialog();
        }
        private void PrepareDialog(PolicyPlusProduct Product)
        {
            SelectedProduct = Product;
            NameTextbox.Text = Product.DisplayName;
            IdTextbox.Text = Product.UniqueID;
            DefinedTextbox.Text = Product.RawProduct.DefinedIn.SourceFile;
            DisplayCodeTextbox.Text = Product.RawProduct.DisplayCode;
            switch (Product.RawProduct.Type)
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
            VersionTextbox.Text = Product.RawProduct.Type == AdmxProductType.Product ? "" : Product.RawProduct.Version.ToString();
            if (Product.Parent is null)
            {
                ParentTextbox.Text = "";
                ParentButton.Enabled = false;
            }
            else
            {
                ParentTextbox.Text = Product.Parent.DisplayName;
                ParentButton.Enabled = true;
            }
            ChildrenListview.Items.Clear();
            if (Product.Children is not null)
            {
                foreach (var child in Product.Children)
                {
                    var lsvi = ChildrenListview.Items.Add(child.RawProduct.Version.ToString());
                    lsvi.SubItems.Add(child.DisplayName);
                    lsvi.Tag = child;
                }
            }
        }
        private void ChildrenListview_ClientSizeChanged(object sender, EventArgs e)
        {
            ChName.Width = ChildrenListview.ClientSize.Width - ChVersion.Width;
        }
        private void ChildrenListview_DoubleClick(object sender, EventArgs e)
        {
            if (ChildrenListview.SelectedItems.Count == 0)
                return;
            PrepareDialog((PolicyPlusProduct)ChildrenListview.SelectedItems[0].Tag);
        }
        private void ParentButton_Click(object sender, EventArgs e)
        {
            PrepareDialog(SelectedProduct.Parent);
        }
    }
}