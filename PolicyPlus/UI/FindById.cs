using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using PolicyPlus.Models;
using MyProject = PolicyPlus.My.MyProject;

namespace PolicyPlus.UI
{
    public partial class FindById
    {
        public AdmxBundle AdmxWorkspace;
        public PolicyPlusPolicy SelectedPolicy;
        public PolicyPlusCategory SelectedCategory;
        public PolicyPlusProduct SelectedProduct;
        public PolicyPlusSupport SelectedSupport;
        public AdmxPolicySection SelectedSection; // Specifies the section for policies
        private Image _categoryImage;
        private Image _policyImage;
        private Image _productImage;
        private Image _supportImage;
        private Image _notFoundImage;
        private Image _blankImage;

        public FindById()
        {
            InitializeComponent();
        }

        private void FindById_Load(object sender, EventArgs e)
        {
            _categoryImage = MyProject.Forms.Main.PolicyIcons.Images[0];
            _policyImage = MyProject.Forms.Main.PolicyIcons.Images[4];
            _productImage = MyProject.Forms.Main.PolicyIcons.Images[10];
            _supportImage = MyProject.Forms.Main.PolicyIcons.Images[11];
            _notFoundImage = MyProject.Forms.Main.PolicyIcons.Images[8];
            _blankImage = MyProject.Forms.Main.PolicyIcons.Images[9];
        }

        private void IdTextbox_TextChanged(object sender, EventArgs e)
        {
            if (AdmxWorkspace is null)
            {
                return; // Wait until actually shown
            }

            SelectedPolicy = null;
            SelectedCategory = null;
            SelectedProduct = null;
            SelectedSupport = null;
            var id = Strings.Trim(IdTextbox.Text);
            if (AdmxWorkspace.FlatCategories.TryGetValue(id, out var category))
            {
                StatusImage.Image = _categoryImage;
                SelectedCategory = category;
            }
            else if (AdmxWorkspace.FlatProducts.TryGetValue(id, out var product))
            {
                StatusImage.Image = _productImage;
                SelectedProduct = product;
            }
            else if (AdmxWorkspace.SupportDefinitions.TryGetValue(id, out var definition))
            {
                StatusImage.Image = _supportImage;
                SelectedSupport = definition;
            }
            else // Check for a policy
            {
                var policyAndSection = Strings.Split(id, "@", 2);
                var policyId = policyAndSection[0]; // Cut off the section override
                if (AdmxWorkspace.Policies.TryGetValue(policyId, out var policy))
                {
                    StatusImage.Image = _policyImage;
                    SelectedPolicy = policy;
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
                    StatusImage.Image = string.IsNullOrEmpty(id) ? _blankImage : _notFoundImage;
                }
            }
        }

        private void FindById_Shown(object sender, EventArgs e)
        {
            if (IdTextbox.Text == " ")
            {
                IdTextbox.Text = ""; // It's set to a single space in the designer
            }

            IdTextbox.Focus();
            IdTextbox.SelectAll();
        }

        private void GoButton_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK; // Close

        private void FindById_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}