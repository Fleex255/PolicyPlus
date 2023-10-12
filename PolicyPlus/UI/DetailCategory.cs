using System;
using PolicyPlus.Models;

namespace PolicyPlus.UI
{
    public partial class DetailCategory
    {
        private PolicyPlusCategory _selectedCategory;

        public DetailCategory()
        {
            InitializeComponent();
        }

        public void PresentDialog(PolicyPlusCategory category)
        {
            PrepareDialog(category);
            ShowDialog();
        }

        private void PrepareDialog(PolicyPlusCategory category)
        {
            _selectedCategory = category;
            NameTextbox.Text = category.DisplayName;
            IdTextbox.Text = category.UniqueId;
            DefinedTextbox.Text = category.RawCategory.DefinedIn.SourceFile;
            DisplayCodeTextbox.Text = category.RawCategory.DisplayCode;
            InfoCodeTextbox.Text = category.RawCategory.ExplainCode;
            ParentButton.Enabled = category.Parent is not null;
            if (category.Parent is not null)
            {
                ParentTextbox.Text = category.Parent.DisplayName;
            }
            else if (!string.IsNullOrEmpty(category.RawCategory.ParentId))
            {
                ParentTextbox.Text = "<orphaned from " + category.RawCategory.ParentId + ">";
            }
            else
            {
                ParentTextbox.Text = "";
            }
        }

        private void ParentButton_Click(object sender, EventArgs e) => PrepareDialog(_selectedCategory.Parent);
    }
}