using System;
using System.Linq;
using System.Windows.Forms;
using PolicyPlus.Models;
using PolicyPlus.UI.Elements;
using MyProject = PolicyPlus.My.MyProject;

namespace PolicyPlus.UI
{
    public partial class InspectPolicyElements
    {
        private PolicyPlusPolicy _selectedPolicy;

        public InspectPolicyElements()
        {
            InitializeComponent();
        }

        public void PresentDialog(PolicyPlusPolicy policy, ImageList images, AdmxBundle admxWorkspace)
        {
            _selectedPolicy = policy;
            PolicyNameTextbox.Text = policy.DisplayName;
            InfoTreeview.Nodes.Clear();
            InfoTreeview.ImageList = images;
            InfoTreeview.Nodes.Add("Registry key: " + policy.RawPolicy.RegistryKey).ImageIndex = 0; // Folder
            if (!string.IsNullOrEmpty(policy.RawPolicy.RegistryValue))
            {
                InfoTreeview.Nodes.Add("Registry value: " + policy.RawPolicy.RegistryValue).ImageIndex = 13; // Gear
            }

            if (!string.IsNullOrEmpty(policy.RawPolicy.ClientExtension))
            {
                InfoTreeview.Nodes.Add("Client extension: " + policy.RawPolicy.ClientExtension).ImageIndex = 19; // DOS window
            } // Delete

            // Add the policy's basic Registry info
            AddList(policy.RawPolicy.AffectedValues, InfoTreeview.Nodes, !string.IsNullOrEmpty(policy.RawPolicy.RegistryValue));
            // Add all the info on the policy's elements
            if (policy.Presentation is not null && policy.RawPolicy.Elements is not null)
            {
                var presNode = InfoTreeview.Nodes.Add("Presentation: " + policy.Presentation.Name);
                presNode.ImageIndex = 20; // Form
                foreach (var presElem in policy.Presentation.Elements)
                {
                    var presPartNode = presNode.Nodes.Add("Presentation element (type: " + presElem.ElementType + ")" + (!string.IsNullOrEmpty(presElem.Id) ? ", ID: " + presElem.Id + "" : ""));
                    switch (presElem.ElementType ?? "")
                    {
                        case "text":
                            {
                                var labelPres = (LabelPresentationElement)presElem;
                                presPartNode.ImageIndex = 21; // Text rows
                                presPartNode.Nodes.Add("Text: \"" + labelPres.Text + "\"").ImageIndex = 14;
                                break;
                            }
                        case "decimalTextBox":
                            {
                                var decTextPres = (NumericBoxPresentationElement)presElem;
                                presPartNode.ImageIndex = 22; // Calculator with pencil
                                if (!string.IsNullOrEmpty(decTextPres.Label))
                                {
                                    presPartNode.Nodes.Add("Label: \"" + decTextPres.Label + "\"").ImageIndex = 14;
                                }

                                presPartNode.Nodes.Add("Default: " + decTextPres.DefaultValue).ImageIndex = 23; // Wrench
                                presPartNode.Nodes.Add(decTextPres.HasSpinner ? "Spinner increment: " + decTextPres.SpinnerIncrement : "No spinner").ImageIndex = 6;
                                break;
                            }
                        case "textBox":
                            {
                                var textPres = (TextBoxPresentationElement)presElem;
                                presPartNode.ImageIndex = 24; // Text field
                                if (!string.IsNullOrEmpty(textPres.Label))
                                {
                                    presPartNode.Nodes.Add("Label: \"" + textPres.Label + "\"").ImageIndex = 14;
                                }

                                presPartNode.Nodes.Add("Default: \"" + textPres.DefaultValue + "\"").ImageIndex = 23;
                                break;
                            }
                        case "checkBox":
                            {
                                var checkPres = (CheckBoxPresentationElement)presElem;
                                presPartNode.ImageIndex = 25; // Tickmark
                                presPartNode.Nodes.Add("Text: \"" + checkPres.Text + "\"").ImageIndex = 14;
                                presPartNode.Nodes.Add("Default: " + (checkPres.DefaultState ? "checked" : "unchecked")).ImageIndex = 23;
                                break;
                            }
                        case "comboBox":
                            {
                                var comboPres = (ComboBoxPresentationElement)presElem;
                                presPartNode.ImageIndex = 26; // Bar with text
                                if (!string.IsNullOrEmpty(comboPres.Label))
                                {
                                    presPartNode.Nodes.Add("Label: \"" + comboPres.Label + "\"").ImageIndex = 14;
                                }

                                presPartNode.Nodes.Add("Default: \"" + comboPres.DefaultText + "\"").ImageIndex = 23;
                                presPartNode.Nodes.Add("Sorting: " + (comboPres.NoSort ? "from ADMX" : "alphabetical")).ImageIndex = 28; // Sorted table
                                if (comboPres.Suggestions is not null)
                                {
                                    var sugNode = presPartNode.Nodes.Add(comboPres.Suggestions.Count + " suggestions");
                                    sugNode.ImageIndex = 29; // Letter
                                    foreach (var sug in comboPres.Suggestions)
                                    {
                                        sugNode.Nodes.Add("\"" + sug + "\"").ImageIndex = 14;
                                    }
                                }

                                break;
                            }
                        case "dropdownList":
                            {
                                var dropdownPres = (DropDownPresentationElement)presElem;
                                presPartNode.ImageIndex = 30; // List view
                                if (!string.IsNullOrEmpty(dropdownPres.Label))
                                {
                                    presPartNode.Nodes.Add("Label: \"" + dropdownPres.Label + "\"").ImageIndex = 14;
                                }

                                if (dropdownPres.DefaultItemId.HasValue)
                                {
                                    presPartNode.Nodes.Add("Default: #" + dropdownPres.DefaultItemId.Value).ImageIndex = 23;
                                }

                                presPartNode.Nodes.Add("Sorting: " + (dropdownPres.NoSort ? "from ADMX" : "alphabetical")).ImageIndex = 28;
                                break;
                            }
                        case "listBox":
                            {
                                var listPres = (ListPresentationElement)presElem;
                                presPartNode.ImageIndex = 27; // Table window
                                presPartNode.Nodes.Add("Label: \"" + listPres.Label + "\"").ImageIndex = 14;
                                break;
                            }
                        case "multiTextBox":
                            {
                                var multiTextPres = (MultiTextPresentationElement)presElem;
                                presPartNode.ImageIndex = 38; // Cascading boxes
                                presPartNode.Nodes.Add("Label: \"" + multiTextPres.Label + "\"").ImageIndex = 14;
                                break;
                            }
                    }
                    if (string.IsNullOrEmpty(presElem.Id))
                    {
                        continue;
                    }

                    var elem = policy.RawPolicy.Elements.First(e => (e.Id ?? "") == (presElem.Id ?? ""));
                    var elemNode = presPartNode.Nodes.Add("Policy element (type: " + elem.ElementType + ")");
                    elemNode.ImageIndex = 31; // Brick
                    if (!string.IsNullOrEmpty(elem.ClientExtension))
                    {
                        elemNode.Nodes.Add("Client extension: " + elem.ClientExtension).ImageIndex = 19;
                    }

                    if (!string.IsNullOrEmpty(elem.RegistryKey))
                    {
                        elemNode.Nodes.Add("Registry key: " + elem.RegistryKey).ImageIndex = 0;
                    }

                    if (elem.ElementType != "list")
                    {
                        elemNode.Nodes.Add("Registry value: " + elem.RegistryValue).ImageIndex = 13;
                    }

                    switch (elem.ElementType ?? "")
                    {
                        case "decimal":
                            {
                                var decimalElem = (DecimalPolicyElement)elem;
                                elemNode.Nodes.Add("Minimum: " + decimalElem.Minimum).ImageIndex = 35; // Down arrow
                                elemNode.Nodes.Add("Maximum: " + decimalElem.Maximum).ImageIndex = 6;
                                if (decimalElem.StoreAsText)
                                {
                                    elemNode.Nodes.Add("Stored as text").ImageIndex = 33; // Letters
                                }

                                elemNode.Nodes.Add("Required: " + (decimalElem.Required ? "yes" : "no")).ImageIndex = 32; // Exclamation
                                if (decimalElem.NoOverwrite)
                                {
                                    elemNode.Nodes.Add("Soft").ImageIndex = 34; // Soft speaker
                                }

                                break;
                            }
                        case "boolean":
                            {
                                var booleanElem = (BooleanPolicyElement)elem;
                                AddList(booleanElem.AffectedRegistry, elemNode.Nodes, true);
                                break;
                            }
                        case "text":
                            {
                                var textElem = (TextPolicyElement)elem;
                                elemNode.Nodes.Add("Maximum length: " + textElem.MaxLength).ImageIndex = 6;
                                if (textElem.RegExpandSz)
                                {
                                    elemNode.Nodes.Add("Stored as expandable string").ImageIndex = 36; // Letters with arrow
                                }

                                elemNode.Nodes.Add("Required: " + (textElem.Required ? "yes" : "no")).ImageIndex = 32;
                                if (textElem.NoOverwrite)
                                {
                                    elemNode.Nodes.Add("Soft").ImageIndex = 34;
                                }

                                break;
                            }
                        case "list":
                            {
                                var listElem = (ListPolicyElement)elem;
                                if (listElem.UserProvidesNames)
                                {
                                    elemNode.Nodes.Add("User provides value names").ImageIndex = 13;
                                }
                                else if (listElem.HasPrefix)
                                {
                                    elemNode.Nodes.Add("Value prefix: \"" + listElem.RegistryValue + "\"").ImageIndex = 13;
                                }
                                else
                                {
                                    elemNode.Nodes.Add("No prefix (values named for their data)").ImageIndex = 13;
                                }
                                if (listElem.RegExpandSz)
                                {
                                    elemNode.Nodes.Add("Stored as expandable strings").ImageIndex = 36;
                                }

                                elemNode.Nodes.Add("Preserve existing values: " + (listElem.NoPurgeOthers ? "yes" : "no")).ImageIndex = 34;
                                break;
                            }
                        case "enum":
                            {
                                var enumElem = (EnumPolicyElement)elem;
                                elemNode.Nodes.Add("Required: " + (enumElem.Required ? "yes" : "no")).ImageIndex = 32;
                                var itemsNode = elemNode.Nodes.Add(enumElem.Items.Count + " choices");
                                itemsNode.ImageIndex = 26;
                                var id = 0;
                                foreach (var item in enumElem.Items)
                                {
                                    var itemNode = itemsNode.Nodes.Add("Choice #" + id);
                                    itemNode.ImageIndex = 29;
                                    itemNode.Nodes.Add("Display code: " + item.DisplayCode).ImageIndex = 14;
                                    itemNode.Nodes.Add("Display name: \"" + admxWorkspace.ResolveString(item.DisplayCode, policy.RawPolicy.DefinedIn) + "\"").ImageIndex = 21;
                                    AddValueData(item.Value, itemNode);
                                    if (item.ValueList is not null)
                                    {
                                        var regNode = itemNode.Nodes.Add("Additional Registry settings modified");
                                        regNode.ImageIndex = 12;
                                        AddSingleListContents(item.ValueList, regNode);
                                    }
                                    id++;
                                }

                                break;
                            }
                        case "multiText":
                            {
                                break;
                            }
                            // Has no special attributes
                    }
                }
            }
            // Make SelectedImageIndex always be the same as ImageIndex
            NormalizeSelIndex(InfoTreeview.Nodes);
            ShowDialog();
        }

        private static void AddList(PolicyRegistryList regList, TreeNodeCollection nodes, bool hasValue)
        {
            var listNode = nodes.Add("Affected Registry settings");
            listNode.ImageIndex = 12; // Database
            if (regList.OnValue is not null)
            {
                var onNode = listNode.Nodes.Add("Set when enabled");
                onNode.ImageIndex = 17; // Checkmark
                AddValueData(regList.OnValue, onNode);
            }
            if (regList.OnValueList is not null)
            {
                var onListNode = listNode.Nodes.Add("Set list when enabled");
                onListNode.ImageIndex = 17;
                AddSingleListContents(regList.OnValueList, onListNode);
            }
            if (regList.OffValue is not null)
            {
                var offNode = listNode.Nodes.Add("Set when disabled");
                offNode.ImageIndex = 8; // Minus
                AddValueData(regList.OffValue, offNode);
            }
            if (regList.OffValueList is not null)
            {
                var offListNode = listNode.Nodes.Add("Set list when disabled");
                offListNode.ImageIndex = 8;
                AddSingleListContents(regList.OffValueList, offListNode);
            }
            if (listNode.Nodes.Count == 0)
            {
                listNode.Nodes.Add(hasValue ? "Left implicit" : "Left to elements").ImageIndex = 37;
            }
        }

        private static void AddSingleListContents(PolicyRegistrySingleList singleList, TreeNode node)
        {
            if (!string.IsNullOrEmpty(singleList.DefaultRegistryKey))
            {
                node.Nodes.Add("Registry key: " + singleList.DefaultRegistryKey).ImageIndex = 0;
            }

            foreach (var entry in singleList.AffectedValues)
            {
                AddListEntry(entry, node);
            }
        }

        private static void AddListEntry(PolicyRegistryListEntry regVal, TreeNode node)
        {
            var entryNode = node.Nodes.Add("Set a value");
            entryNode.ImageIndex = 16; // Gear with pencil
            if (!string.IsNullOrEmpty(regVal.RegistryKey))
            {
                entryNode.Nodes.Add("Registry key: " + regVal.RegistryKey).ImageIndex = 0;
            }

            entryNode.Nodes.Add("Registry value: " + regVal.RegistryValue).ImageIndex = 13;
            AddValueData(regVal.Value, entryNode);
        }

        private static void AddValueData(PolicyRegistryValue regVal, TreeNode node)
        {
            switch (regVal.RegistryType)
            {
                case PolicyRegistryValueType.Delete: { node.Nodes.Add("Delete value").ImageIndex = 18; break; }
                case PolicyRegistryValueType.Numeric: { node.Nodes.Add("Numeric value: " + regVal.NumberValue).ImageIndex = 15; break; }
                case PolicyRegistryValueType.Text: { node.Nodes.Add("Text value: \"" + regVal.StringValue + "\"").ImageIndex = 14; break; }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void NormalizeSelIndex(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.SelectedImageIndex = node.ImageIndex;
                NormalizeSelIndex(node.Nodes);
            }
        }

        private void InfoTreeview_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control && InfoTreeview.SelectedNode is not null)
            {
                MyProject.Computer.Clipboard.SetText(InfoTreeview.SelectedNode.Text);
            }
        }

        private void PolicyDetailsButton_Click(object sender, EventArgs e) => MyProject.Forms.DetailPolicy.PresentDialog(_selectedPolicy);
    }
}