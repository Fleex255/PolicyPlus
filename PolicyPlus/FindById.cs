using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PolicyPlus
{
    public partial class FindById
    {
        public AdmxBundle AdmxWorkspace;
        public PolicyPlusPolicy SelectedPolicy;
        public PolicyPlusCategory SelectedCategory;
        public PolicyPlusProduct SelectedProduct;
        public PolicyPlusSupport SelectedSupport;
        public AdmxPolicySection SelectedSection; // Specifies the section for policies
        private Image CategoryImage;
        private Image PolicyImage;
        private Image ProductImage;
        private Image SupportImage;
        private Image NotFoundImage;
        private Image BlankImage;

        public FindById()
        {
            InitializeComponent();
        }
        private void FindById_Load(object sender, EventArgs e)
        {
            CategoryImage = My.MyProject.Forms.Main.PolicyIcons.Images[0];
            PolicyImage = My.MyProject.Forms.Main.PolicyIcons.Images[4];
            ProductImage = My.MyProject.Forms.Main.PolicyIcons.Images[10];
            SupportImage = My.MyProject.Forms.Main.PolicyIcons.Images[11];
            NotFoundImage = My.MyProject.Forms.Main.PolicyIcons.Images[8];
            BlankImage = My.MyProject.Forms.Main.PolicyIcons.Images[9];
        }
        private void IdTextbox_TextChanged(object sender, EventArgs e)
        {
            if (AdmxWorkspace is null)
                return; // Wait until actually shown
            SelectedPolicy = null;
            SelectedCategory = null;
            SelectedProduct = null;
            SelectedSupport = null;
            string id = Strings.Trim(IdTextbox.Text);
            if (AdmxWorkspace.FlatCategories.ContainsKey(id))
            {
                StatusImage.Image = CategoryImage;
                SelectedCategory = AdmxWorkspace.FlatCategories[id];
            }
            else if (AdmxWorkspace.FlatProducts.ContainsKey(id))
            {
                StatusImage.Image = ProductImage;
                SelectedProduct = AdmxWorkspace.FlatProducts[id];
            }
            else if (AdmxWorkspace.SupportDefinitions.ContainsKey(id))
            {
                StatusImage.Image = SupportImage;
                SelectedSupport = AdmxWorkspace.SupportDefinitions[id];
            }
            else // Check for a policy
            {
                string[] policyAndSection = Strings.Split(id, "@", 2);
                string policyId = policyAndSection[0]; // Cut off the section override
                if (AdmxWorkspace.Policies.ContainsKey(policyId))
                {
                    StatusImage.Image = PolicyImage;
                    SelectedPolicy = AdmxWorkspace.Policies[policyId];
                    if (policyAndSection.Length == 2 && policyAndSection[1].Length == 1 && "UC".Contains(policyAndSection[1]))
                    {
                        SelectedSection = policyAndSection[1] == "U" ? AdmxPolicySection.User : AdmxPolicySection.Machine;
                    }
                    else
                    {
                        SelectedSection = AdmxPolicySection.Both;
                    }
                }
                else
                {
                    StatusImage.Image = string.IsNullOrEmpty(id) ? BlankImage : NotFoundImage;
                }
            }
        }
        private void FindById_Shown(object sender, EventArgs e)
        {
            if (IdTextbox.Text == " ")
                IdTextbox.Text = ""; // It's set to a single space in the designer
            IdTextbox.Focus();
            IdTextbox.SelectAll();
        }
        private void GoButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK; // Close
        }
        private void FindById_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
    }
}