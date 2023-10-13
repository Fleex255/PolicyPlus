using System;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class DetailPolicy
    {
        private PolicyPlusPolicy _selectedPolicy;

        public DetailPolicy()
        {
            InitializeComponent();
        }

        public void PresentDialog(PolicyPlusPolicy policy)
        {
            _selectedPolicy = policy;
            NameTextbox.Text = policy.DisplayName;
            IdTextbox.Text = policy.UniqueId;
            DefinedTextbox.Text = policy.RawPolicy.DefinedIn.SourceFile;
            DisplayCodeTextbox.Text = policy.RawPolicy.DisplayCode;
            InfoCodeTextbox.Text = policy.RawPolicy.ExplainCode;
            PresentCodeTextbox.Text = policy.RawPolicy.PresentationId;
            switch (policy.RawPolicy.Section)
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
            SupportButton.Enabled = policy.SupportedOn is not null;
            if (policy.SupportedOn is not null)
            {
                SupportTextbox.Text = policy.SupportedOn.DisplayName;
            }
            else if (!string.IsNullOrEmpty(policy.RawPolicy.SupportedCode))
            {
                SupportTextbox.Text = "<missing: " + policy.RawPolicy.SupportedCode + ">";
            }
            else
            {
                SupportTextbox.Text = "";
            }
            CategoryButton.Enabled = policy.Category is not null;
            if (policy.Category is not null)
            {
                CategoryTextbox.Text = policy.Category.DisplayName;
            }
            else if (!string.IsNullOrEmpty(policy.RawPolicy.CategoryId))
            {
                CategoryTextbox.Text = "<orphaned from " + policy.RawPolicy.CategoryId + ">";
            }
            else
            {
                CategoryTextbox.Text = "<uncategorized>";
            }
            _ = ShowDialog(this);
        }

        private void SupportButton_Click(object sender, EventArgs e) => new DetailSupport().PresentDialog(_selectedPolicy.SupportedOn);

        private void CategoryButton_Click(object sender, EventArgs e) => new DetailCategory().PresentDialog(_selectedPolicy.Category);
    }
}