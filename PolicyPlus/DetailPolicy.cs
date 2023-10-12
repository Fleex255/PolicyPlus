using System;

namespace PolicyPlus
{
    public partial class DetailPolicy
    {
        private PolicyPlusPolicy SelectedPolicy;

        public DetailPolicy()
        {
            InitializeComponent();
        }
        public void PresentDialog(PolicyPlusPolicy Policy)
        {
            SelectedPolicy = Policy;
            NameTextbox.Text = Policy.DisplayName;
            IdTextbox.Text = Policy.UniqueID;
            DefinedTextbox.Text = Policy.RawPolicy.DefinedIn.SourceFile;
            DisplayCodeTextbox.Text = Policy.RawPolicy.DisplayCode;
            InfoCodeTextbox.Text = Policy.RawPolicy.ExplainCode;
            PresentCodeTextbox.Text = Policy.RawPolicy.PresentationID;
            switch (Policy.RawPolicy.Section)
            {
                case AdmxPolicySection.Both:
                    {
                        SectionTextbox.Text = "User or computer";
                        break;
                    }
                case AdmxPolicySection.Machine:
                    {
                        SectionTextbox.Text = "Computer";
                        break;
                    }
                case AdmxPolicySection.User:
                    {
                        SectionTextbox.Text = "User";
                        break;
                    }
            }
            SupportButton.Enabled = Policy.SupportedOn is not null;
            if (Policy.SupportedOn is not null)
            {
                SupportTextbox.Text = Policy.SupportedOn.DisplayName;
            }
            else if (!string.IsNullOrEmpty(Policy.RawPolicy.SupportedCode))
            {
                SupportTextbox.Text = "<missing: " + Policy.RawPolicy.SupportedCode + ">";
            }
            else
            {
                SupportTextbox.Text = "";
            }
            CategoryButton.Enabled = Policy.Category is not null;
            if (Policy.Category is not null)
            {
                CategoryTextbox.Text = Policy.Category.DisplayName;
            }
            else if (!string.IsNullOrEmpty(Policy.RawPolicy.CategoryID))
            {
                CategoryTextbox.Text = "<orphaned from " + Policy.RawPolicy.CategoryID + ">";
            }
            else
            {
                CategoryTextbox.Text = "<uncategorized>";
            }
            ShowDialog();
        }
        private void SupportButton_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.DetailSupport.PresentDialog(SelectedPolicy.SupportedOn);
        }
        private void CategoryButton_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.DetailCategory.PresentDialog(SelectedPolicy.Category);
        }
    }
}