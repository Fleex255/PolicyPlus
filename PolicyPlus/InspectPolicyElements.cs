using System;
using System.Linq;
using System.Windows.Forms;

namespace PolicyPlus
{
    public partial class InspectPolicyElements
    {
        private PolicyPlusPolicy SelectedPolicy;

        public InspectPolicyElements()
        {
            InitializeComponent();
        }
        public void PresentDialog(PolicyPlusPolicy Policy, ImageList Images, AdmxBundle AdmxWorkspace)
        {
            SelectedPolicy = Policy;
            PolicyNameTextbox.Text = Policy.DisplayName;
            InfoTreeview.Nodes.Clear();
            InfoTreeview.ImageList = Images;
            InfoTreeview.Nodes.Add("Registry key: " + Policy.RawPolicy.RegistryKey).ImageIndex = 0; // Folder
            if (!string.IsNullOrEmpty(Policy.RawPolicy.RegistryValue))
                InfoTreeview.Nodes.Add("Registry value: " + Policy.RawPolicy.RegistryValue).ImageIndex = 13; // Gear
            if (!string.IsNullOrEmpty(Policy.RawPolicy.ClientExtension))
                InfoTreeview.Nodes.Add("Client extension: " + Policy.RawPolicy.ClientExtension).ImageIndex = 19; // DOS window
                                                                                                                 // Methods for adding info on a policy Registry information object
            void addValueData(PolicyRegistryValue RegVal, TreeNode Node) { switch (RegVal.RegistryType) { case PolicyRegistryValueType.Delete: { Node.Nodes.Add("Delete value").ImageIndex = 18; break; } case PolicyRegistryValueType.Numeric: { Node.Nodes.Add("Numeric value: " + RegVal.NumberValue).ImageIndex = 15; break; } case PolicyRegistryValueType.Text: { Node.Nodes.Add("Text value: \"" + RegVal.StringValue + "\"").ImageIndex = 14; break; } } }; // Delete
                                                                                                                                                                                                                                                                                                                                                                                                                                                                    // Calculator
                                                                                                                                                                                                                                                                                                                                                                                                                                                                    // Text
            void addListEntry(PolicyRegistryListEntry RegVal, TreeNode Node)
            {
                var entryNode = Node.Nodes.Add("Set a value");
                entryNode.ImageIndex = 16; // Gear with pencil
                if (!string.IsNullOrEmpty(RegVal.RegistryKey))
                    entryNode.Nodes.Add("Registry key: " + RegVal.RegistryKey).ImageIndex = 0;
                entryNode.Nodes.Add("Registry value: " + RegVal.RegistryValue).ImageIndex = 13;
                addValueData(RegVal.Value, entryNode);
            };
            void addSingleListContents(PolicyRegistrySingleList SingleList, TreeNode Node)
            {
                if (!string.IsNullOrEmpty(SingleList.DefaultRegistryKey))
                    Node.Nodes.Add("Registry key: " + SingleList.DefaultRegistryKey).ImageIndex = 0;
                foreach (var entry in SingleList.AffectedValues)
                    addListEntry(entry, Node);
            };
            void addList(PolicyRegistryList RegList, TreeNodeCollection Nodes, bool HasValue)
            {
                var listNode = Nodes.Add("Affected Registry settings");
                listNode.ImageIndex = 12; // Database
                if (RegList.OnValue is not null)
                {
                    var onNode = listNode.Nodes.Add("Set when enabled");
                    onNode.ImageIndex = 17; // Checkmark
                    addValueData(RegList.OnValue, onNode);
                }
                if (RegList.OnValueList is not null)
                {
                    var onListNode = listNode.Nodes.Add("Set list when enabled");
                    onListNode.ImageIndex = 17;
                    addSingleListContents(RegList.OnValueList, onListNode);
                }
                if (RegList.OffValue is not null)
                {
                    var offNode = listNode.Nodes.Add("Set when disabled");
                    offNode.ImageIndex = 8; // Minus
                    addValueData(RegList.OffValue, offNode);
                }
                if (RegList.OffValueList is not null)
                {
                    var offListNode = listNode.Nodes.Add("Set list when disabled");
                    offListNode.ImageIndex = 8;
                    addSingleListContents(RegList.OffValueList, offListNode);
                }
                if (listNode.Nodes.Count == 0)
                    listNode.Nodes.Add(HasValue ? "Left implicit" : "Left to elements").ImageIndex = 37;
            };
            // Add the policy's basic Registry info
            addList(Policy.RawPolicy.AffectedValues, InfoTreeview.Nodes, !string.IsNullOrEmpty(Policy.RawPolicy.RegistryValue));
            // Add all the info on the policy's elements
            if (Policy.Presentation is not null & Policy.RawPolicy.Elements is not null)
            {
                var presNode = InfoTreeview.Nodes.Add("Presentation: " + Policy.Presentation.Name);
                presNode.ImageIndex = 20; // Form
                foreach (var presElem in Policy.Presentation.Elements)
                {
                    var presPartNode = presNode.Nodes.Add("Presentation element (type: " + presElem.ElementType + ")" + (!string.IsNullOrEmpty(presElem.ID) ? ", ID: " + presElem.ID + "" : ""));
                    switch (presElem.ElementType ?? "")
                    {
                        case "text":
                            {
                                LabelPresentationElement labelPres = (LabelPresentationElement)presElem;
                                presPartNode.ImageIndex = 21; // Text rows
                                presPartNode.Nodes.Add("Text: \"" + labelPres.Text + "\"").ImageIndex = 14;
                                break;
                            }
                        case "decimalTextBox":
                            {
                                NumericBoxPresentationElement decTextPres = (NumericBoxPresentationElement)presElem;
                                presPartNode.ImageIndex = 22; // Calculator with pencil
                                if (!string.IsNullOrEmpty(decTextPres.Label))
                                    presPartNode.Nodes.Add("Label: \"" + decTextPres.Label + "\"").ImageIndex = 14;
                                presPartNode.Nodes.Add("Default: " + decTextPres.DefaultValue).ImageIndex = 23; // Wrench
                                presPartNode.Nodes.Add(decTextPres.HasSpinner ? "Spinner increment: " + decTextPres.SpinnerIncrement : "No spinner").ImageIndex = 6;
                                break;
                            }
                        case "textBox":
                            {
                                TextBoxPresentationElement textPres = (TextBoxPresentationElement)presElem;
                                presPartNode.ImageIndex = 24; // Text field
                                if (!string.IsNullOrEmpty(textPres.Label))
                                    presPartNode.Nodes.Add("Label: \"" + textPres.Label + "\"").ImageIndex = 14;
                                presPartNode.Nodes.Add("Default: \"" + textPres.DefaultValue + "\"").ImageIndex = 23;
                                break;
                            }
                        case "checkBox":
                            {
                                CheckBoxPresentationElement checkPres = (CheckBoxPresentationElement)presElem;
                                presPartNode.ImageIndex = 25; // Tickmark
                                presPartNode.Nodes.Add("Text: \"" + checkPres.Text + "\"").ImageIndex = 14;
                                presPartNode.Nodes.Add("Default: " + (checkPres.DefaultState ? "checked" : "unchecked")).ImageIndex = 23;
                                break;
                            }
                        case "comboBox":
                            {
                                ComboBoxPresentationElement comboPres = (ComboBoxPresentationElement)presElem;
                                presPartNode.ImageIndex = 26; // Bar with text
                                if (!string.IsNullOrEmpty(comboPres.Label))
                                    presPartNode.Nodes.Add("Label: \"" + comboPres.Label + "\"").ImageIndex = 14;
                                presPartNode.Nodes.Add("Default: \"" + comboPres.DefaultText + "\"").ImageIndex = 23;
                                presPartNode.Nodes.Add("Sorting: " + (comboPres.NoSort ? "from ADMX" : "alphabetical")).ImageIndex = 28; // Sorted table
                                if (comboPres.Suggestions is not null)
                                {
                                    var sugNode = presPartNode.Nodes.Add(comboPres.Suggestions.Count + " suggestions");
                                    sugNode.ImageIndex = 29; // Letter
                                    foreach (var sug in comboPres.Suggestions)
                                        sugNode.Nodes.Add("\"" + sug + "\"").ImageIndex = 14;
                                }

                                break;
                            }
                        case "dropdownList":
                            {
                                DropDownPresentationElement dropdownPres = (DropDownPresentationElement)presElem;
                                presPartNode.ImageIndex = 30; // List view
                                if (!string.IsNullOrEmpty(dropdownPres.Label))
                                    presPartNode.Nodes.Add("Label: \"" + dropdownPres.Label + "\"").ImageIndex = 14;
                                if (dropdownPres.DefaultItemID.HasValue)
                                    presPartNode.Nodes.Add("Default: #" + dropdownPres.DefaultItemID.Value).ImageIndex = 23;
                                presPartNode.Nodes.Add("Sorting: " + (dropdownPres.NoSort ? "from ADMX" : "alphabetical")).ImageIndex = 28;
                                break;
                            }
                        case "listBox":
                            {
                                ListPresentationElement listPres = (ListPresentationElement)presElem;
                                presPartNode.ImageIndex = 27; // Table window
                                presPartNode.Nodes.Add("Label: \"" + listPres.Label + "\"").ImageIndex = 14;
                                break;
                            }
                        case "multiTextBox":
                            {
                                MultiTextPresentationElement multiTextPres = (MultiTextPresentationElement)presElem;
                                presPartNode.ImageIndex = 38; // Cascading boxes
                                presPartNode.Nodes.Add("Label: \"" + multiTextPres.Label + "\"").ImageIndex = 14;
                                break;
                            }
                    }
                    if (string.IsNullOrEmpty(presElem.ID))
                        continue;
                    var elem = Policy.RawPolicy.Elements.First(e => (e.ID ?? "") == (presElem.ID ?? ""));
                    var elemNode = presPartNode.Nodes.Add("Policy element (type: " + elem.ElementType + ")");
                    elemNode.ImageIndex = 31; // Brick
                    if (!string.IsNullOrEmpty(elem.ClientExtension))
                        elemNode.Nodes.Add("Client extension: " + elem.ClientExtension).ImageIndex = 19;
                    if (!string.IsNullOrEmpty(elem.RegistryKey))
                        elemNode.Nodes.Add("Registry key: " + elem.RegistryKey).ImageIndex = 0;
                    if (elem.ElementType != "list")
                        elemNode.Nodes.Add("Registry value: " + elem.RegistryValue).ImageIndex = 13;
                    switch (elem.ElementType ?? "")
                    {
                        case "decimal":
                            {
                                DecimalPolicyElement decimalElem = (DecimalPolicyElement)elem;
                                elemNode.Nodes.Add("Minimum: " + decimalElem.Minimum).ImageIndex = 35; // Down arrow
                                elemNode.Nodes.Add("Maximum: " + decimalElem.Maximum).ImageIndex = 6;
                                if (decimalElem.StoreAsText)
                                    elemNode.Nodes.Add("Stored as text").ImageIndex = 33; // Letters
                                elemNode.Nodes.Add("Required: " + (decimalElem.Required ? "yes" : "no")).ImageIndex = 32; // Exclamation
                                if (decimalElem.NoOverwrite)
                                    elemNode.Nodes.Add("Soft").ImageIndex = 34; // Soft speaker
                                break;
                            }
                        case "boolean":
                            {
                                BooleanPolicyElement booleanElem = (BooleanPolicyElement)elem;
                                addList(booleanElem.AffectedRegistry, elemNode.Nodes, true);
                                break;
                            }
                        case "text":
                            {
                                TextPolicyElement textElem = (TextPolicyElement)elem;
                                elemNode.Nodes.Add("Maximum length: " + textElem.MaxLength).ImageIndex = 6;
                                if (textElem.RegExpandSz)
                                    elemNode.Nodes.Add("Stored as expandable string").ImageIndex = 36; // Letters with arrow
                                elemNode.Nodes.Add("Required: " + (textElem.Required ? "yes" : "no")).ImageIndex = 32;
                                if (textElem.NoOverwrite)
                                    elemNode.Nodes.Add("Soft").ImageIndex = 34;
                                break;
                            }
                        case "list":
                            {
                                ListPolicyElement listElem = (ListPolicyElement)elem;
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
                                    elemNode.Nodes.Add("Stored as expandable strings").ImageIndex = 36;
                                elemNode.Nodes.Add("Preserve existing values: " + (listElem.NoPurgeOthers ? "yes" : "no")).ImageIndex = 34;
                                break;
                            }
                        case "enum":
                            {
                                EnumPolicyElement enumElem = (EnumPolicyElement)elem;
                                elemNode.Nodes.Add("Required: " + (enumElem.Required ? "yes" : "no")).ImageIndex = 32;
                                var itemsNode = elemNode.Nodes.Add(enumElem.Items.Count + " choices");
                                itemsNode.ImageIndex = 26;
                                int id = 0;
                                foreach (var item in enumElem.Items)
                                {
                                    var itemNode = itemsNode.Nodes.Add("Choice #" + id);
                                    itemNode.ImageIndex = 29;
                                    itemNode.Nodes.Add("Display code: " + item.DisplayCode).ImageIndex = 14;
                                    itemNode.Nodes.Add("Display name: \"" + AdmxWorkspace.ResolveString(item.DisplayCode, Policy.RawPolicy.DefinedIn) + "\"").ImageIndex = 21;
                                    addValueData(item.Value, itemNode);
                                    if (item.ValueList is not null)
                                    {
                                        var regNode = itemNode.Nodes.Add("Additional Registry settings modified");
                                        regNode.ImageIndex = 12;
                                        addSingleListContents(item.ValueList, regNode);
                                    }
                                    id += 1;
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
            Action<TreeNodeCollection> normalizeSelIndex;
            normalizeSelIndex = new Action<TreeNodeCollection>((Nodes) => { foreach (TreeNode node in Nodes) { node.SelectedImageIndex = node.ImageIndex; normalizeSelIndex(node.Nodes); } });
            normalizeSelIndex(InfoTreeview.Nodes);
            ShowDialog();
        }
        private void InfoTreeview_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C & e.Modifiers == Keys.Control & InfoTreeview.SelectedNode is not null)
            {
                My.MyProject.Computer.Clipboard.SetText(InfoTreeview.SelectedNode.Text);
            }
        }
        private void PolicyDetailsButton_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.DetailPolicy.PresentDialog(SelectedPolicy);
        }
    }
}