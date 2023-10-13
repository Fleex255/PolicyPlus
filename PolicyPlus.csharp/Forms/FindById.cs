using System;
using System.Drawing;
using System.Windows.Forms;

using PolicyPlus.csharp.Models;
using PolicyPlus.csharp.Models.Sources.Admx;

namespace PolicyPlus.csharp.UI
{
    public partial class FindById
    {
        public AdmxBundle AdmxWorkspace;
        public PolicyPlusPolicy? SelectedPolicy;
        public PolicyPlusCategory? SelectedCategory;
        public PolicyPlusProduct? SelectedProduct;
        public PolicyPlusSupport? SelectedSupport;
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
            var main = (Main)Parent;
            _categoryImage = main.PolicyIcons.Images[0];
            _policyImage = main.PolicyIcons.Images[4];
            _productImage = main.PolicyIcons.Images[10];
            _supportImage = main.PolicyIcons.Images[11];
            _notFoundImage = main.PolicyIcons.Images[8];
            _blankImage = main.PolicyIcons.Images[9];
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
            var id = IdTextbox.Text.Trim();
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
                var policyAndSection = id.Split("@", 2);
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
                IdTextbox.Text = string.Empty; // It's set to a single space in the designer
            }

            _ = IdTextbox.Focus();
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