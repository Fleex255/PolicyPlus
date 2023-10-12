using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PolicyPlus.Helpers;
using PolicyPlus.Models;

namespace PolicyPlus.UI
{
    public partial class FilterOptions
    {
        public FilterConfiguration CurrentFilter;
        private Dictionary<PolicyPlusProduct, TreeNode> _productNodes;

        public FilterOptions()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(FilterConfiguration configuration, AdmxBundle workspace)
        {
            CurrentFilter = null;
            _productNodes = new Dictionary<PolicyPlusProduct, TreeNode>();
            AllowedProductsTreeview.Nodes.Clear();
            AddProductsToNodeCollection(workspace.Products.Values, AllowedProductsTreeview.Nodes); // Recursively add products
            PrepareDialog(configuration);
            return ShowDialog();
        }

        private void AddProductsToNodeCollection(IEnumerable<PolicyPlusProduct> products, TreeNodeCollection nodes)
        {
            foreach (var product in products)
            {
                var node = nodes.Add(product.DisplayName);
                _productNodes.Add(product, node);
                node.Tag = product;
                if (product.Children is not null)
                {
                    AddProductsToNodeCollection(product.Children, node.Nodes);
                }
            }
        }

        public void PrepareDialog(FilterConfiguration configuration)
        {
            // Set the UI element state from the current filter
            if (configuration.ManagedPolicy.HasValue)
            {
                PolicyTypeCombobox.SelectedIndex = configuration.ManagedPolicy.Value ? 1 : 2;
            }
            else
            {
                PolicyTypeCombobox.SelectedIndex = 0;
            }
            if (configuration.PolicyState.HasValue)
            {
                switch (configuration.PolicyState.Value)
                {
                    case FilterPolicyState.NotConfigured:
                        {
                            PolicyStateCombobox.SelectedIndex = 1;
                            break;
                        }
                    case FilterPolicyState.Configured:
                        {
                            PolicyStateCombobox.SelectedIndex = 2;
                            break;
                        }
                    case FilterPolicyState.Enabled:
                        {
                            PolicyStateCombobox.SelectedIndex = 3;
                            break;
                        }
                    case FilterPolicyState.Disabled:
                        {
                            PolicyStateCombobox.SelectedIndex = 4;
                            break;
                        }
                }
            }
            else
            {
                PolicyStateCombobox.SelectedIndex = 0;
            }
            if (configuration.Commented.HasValue)
            {
                CommentedCombobox.SelectedIndex = configuration.Commented.Value ? 1 : 2;
            }
            else
            {
                CommentedCombobox.SelectedIndex = 0;
            }
            foreach (var node in _productNodes.Values)
            {
                node.Checked = false;
            }

            if (configuration.AllowedProducts is null)
            {
                SupportedCheckbox.Checked = false;
                AlwaysMatchAnyCheckbox.Checked = true;
                MatchBlankSupportCheckbox.Checked = true;
            }
            else
            {
                SupportedCheckbox.Checked = true;
                foreach (var product in configuration.AllowedProducts)
                {
                    _productNodes[product].Checked = true;
                }

                AlwaysMatchAnyCheckbox.Checked = configuration.AlwaysMatchAny;
                MatchBlankSupportCheckbox.Checked = configuration.MatchBlankSupport;
                // Expand to show all products with a different check state than their parent
                foreach (TreeNode treeNode in AllowedProductsTreeview.Nodes)
                {
                    ExpandIfNecessary(treeNode);
                }
            }
        }

        private static void ExpandIfNecessary(TreeNode treeNode)
        {
            foreach (TreeNode subnode in treeNode.Nodes)
            {
                ExpandIfNecessary(subnode);
                if (subnode.IsExpanded || subnode.Checked != treeNode.Checked)
                {
                    treeNode.Expand();
                }
            }
        }

        private void SupportedCheckbox_CheckedChanged(object sender, EventArgs e) => RequirementsBox.Enabled = SupportedCheckbox.Checked;

        private void FilterOptions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void ResetButton_Click(object sender, EventArgs e) => PrepareDialog(new FilterConfiguration());

        private void OkButton_Click(object sender, EventArgs e)
        {
            // Create a filter configuration object from the user's settings
            var newConf = new FilterConfiguration();
            switch (PolicyTypeCombobox.SelectedIndex)
            {
                case 1:
                    {
                        newConf.ManagedPolicy = true;
                        break;
                    }
                case 2:
                    {
                        newConf.ManagedPolicy = false;
                        break;
                    }
            }
            switch (PolicyStateCombobox.SelectedIndex)
            {
                case 1:
                    {
                        newConf.PolicyState = FilterPolicyState.NotConfigured;
                        break;
                    }
                case 2:
                    {
                        newConf.PolicyState = FilterPolicyState.Configured;
                        break;
                    }
                case 3:
                    {
                        newConf.PolicyState = FilterPolicyState.Enabled;
                        break;
                    }
                case 4:
                    {
                        newConf.PolicyState = FilterPolicyState.Disabled;
                        break;
                    }
            }
            switch (CommentedCombobox.SelectedIndex)
            {
                case 1:
                    {
                        newConf.Commented = true;
                        break;
                    }
                case 2:
                    {
                        newConf.Commented = false;
                        break;
                    }
            }
            if (SupportedCheckbox.Checked)
            {
                newConf.AlwaysMatchAny = AlwaysMatchAnyCheckbox.Checked;
                newConf.MatchBlankSupport = MatchBlankSupportCheckbox.Checked;
                newConf.AllowedProducts = new List<PolicyPlusProduct>();
                foreach (var kv in _productNodes)
                {
                    if (kv.Value.Checked)
                    {
                        newConf.AllowedProducts.Add(kv.Key);
                    }
                }
            }
            CurrentFilter = newConf;
            DialogResult = DialogResult.OK;
        }

        private void AllowedProductsTreeview_EnabledChanged(object sender, EventArgs e) =>
            // Without this, the tree view looks really bad when disabled
            AllowedProductsTreeview.BackColor = AllowedProductsTreeview.Enabled ? SystemColors.Window : SystemColors.Control;

        private void AllowedProductsTreeview_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // Recursively set the check state on child products only in response to the user
            if (e.Action == TreeViewAction.Unknown)
            {
                return;
            }

            PropogateCheckStateDown(e.Node);
        }

        public void PropogateCheckStateDown(TreeNode node)
        {
            foreach (TreeNode subnode in node.Nodes)
            {
                subnode.Checked = node.Checked;
                PropogateCheckStateDown(subnode);
            }
        }
    }

    // The TreeView control has a b*g: the displayed check state gets out of sync with the Checked
    // property when the checkbox is double-clicked Fix adapted from https://stackoverflow.com/a/3174824
}