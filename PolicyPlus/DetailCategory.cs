using System;

namespace PolicyPlus
{
    public partial class DetailCategory
    {
        private PolicyPlusCategory SelectedCategory;

        public DetailCategory()
        {
            InitializeComponent();
        }
        public void PresentDialog(PolicyPlusCategory Category)
        {
            PrepareDialog(Category);
            ShowDialog();
        }
        private void PrepareDialog(PolicyPlusCategory Category)
        {
            SelectedCategory = Category;
            NameTextbox.Text = Category.DisplayName;
            IdTextbox.Text = Category.UniqueID;
            DefinedTextbox.Text = Category.RawCategory.DefinedIn.SourceFile;
            DisplayCodeTextbox.Text = Category.RawCategory.DisplayCode;
            InfoCodeTextbox.Text = Category.RawCategory.ExplainCode;
            ParentButton.Enabled = Category.Parent is not null;
            if (Category.Parent is not null)
            {
                ParentTextbox.Text = Category.Parent.DisplayName;
            }
            else if (!string.IsNullOrEmpty(Category.RawCategory.ParentID))
            {
                ParentTextbox.Text = "<orphaned from " + Category.RawCategory.ParentID + ">";
            }
            else
            {
                ParentTextbox.Text = "";
            }
        }
        private void ParentButton_Click(object sender, EventArgs e)
        {
            PrepareDialog(SelectedCategory.Parent);
        }
    }
}