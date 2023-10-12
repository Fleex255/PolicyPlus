using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace PolicyPlus
{
    public partial class EditSetting
    {
        private PolicyPlusPolicy CurrentSetting;
        private AdmxPolicySection CurrentSection;
        private AdmxBundle AdmxWorkspace;
        private IPolicySource CompPolSource, UserPolSource;
        private PolicyLoader CompPolLoader, UserPolLoader;
        private Dictionary<string, string> CompComments, UserComments;
        // Above: passed in; below: internal state
        private Dictionary<string, Control> ElementControls;
        private List<Control> ResizableControls;
        private IPolicySource CurrentSource;
        private PolicyLoader CurrentLoader;
        private Dictionary<string, string> CurrentComments;
        private bool ChangesMade; // To either side

        public EditSetting()
        {
            InitializeComponent();
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (ChangesMade)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
        }
        private void EditSetting_Shown(object sender, EventArgs e)
        {
            SettingNameLabel.Text = CurrentSetting.DisplayName;
            if (CurrentSetting.SupportedOn is null)
                SupportedTextbox.Text = "";
            else
                SupportedTextbox.Text = CurrentSetting.SupportedOn.DisplayName;
            HelpTextbox.Text = Main.PrettifyDescription(CurrentSetting.DisplayExplanation);
            if (CurrentSetting.RawPolicy.Section == AdmxPolicySection.Both)
            {
                SectionDropdown.Enabled = true;
                CurrentSection = CurrentSection == AdmxPolicySection.Both ? AdmxPolicySection.Machine : CurrentSection;
            }
            else
            {
                SectionDropdown.Enabled = false;
                CurrentSection = CurrentSetting.RawPolicy.Section;
            }
            ExtraOptionsPanel.HorizontalScroll.Maximum = 0;
            ExtraOptionsPanel.VerticalScroll.Visible = true;
            ExtraOptionsPanel.AutoScroll = true;
            PreparePolicyElements();
            SectionDropdown.Text = CurrentSection == AdmxPolicySection.Machine ? "Computer" : "User";
            SectionDropdown_SelectedIndexChanged(null, null); // Force an update of the current source
            PreparePolicyState();
            StateRadiosChanged(null, null);
        }
        public void PreparePolicyElements()
        {
            for (int n = ExtraOptionsTable.RowCount - 1; n >= 0; n -= 1) // Go backwards because Dispose changes the indexes
            {
                var ctl = ExtraOptionsTable.GetControlFromPosition(0, n);
                if (ctl is not null)
                    ctl.Dispose();
            }
            ExtraOptionsTable.Controls.Clear();
            ExtraOptionsTable.RowCount = 0;
            int curTabIndex = 10;
            void addControl(string ID, Control Control, string Label)
            {
                ExtraOptionsTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                if (string.IsNullOrEmpty(Label)) // Just a single control
                {
                    if (Control.AutoSize)
                        ResizableControls.Add(Control);
                    ExtraOptionsTable.Controls.Add(Control, 0, ExtraOptionsTable.RowStyles.Count - 1);
                }
                else // Has a label attached
                {
                    var flowPanel = new FlowLayoutPanel() { WrapContents = true, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
                    flowPanel.Margin = new Padding(0);
                    ExtraOptionsTable.Controls.Add(flowPanel, 0, ExtraOptionsTable.RowStyles.Count - 1);
                    var labelControl = new Label() { AutoSize = true, Text = Label };
                    labelControl.Anchor = AnchorStyles.Left;
                    Control.Anchor = AnchorStyles.Left;
                    flowPanel.Controls.Add(labelControl);
                    flowPanel.Controls.Add(Control);
                    ResizableControls.Add(flowPanel);
                }
                if (!string.IsNullOrEmpty(ID))
                {
                    ElementControls.Add(ID, Control);
                    Control.TabStop = true;
                    Control.TabIndex = curTabIndex;
                    curTabIndex += 1;
                }
                else
                {
                    Control.TabStop = false;
                }
            };
            ExtraOptionsTable.RowStyles.Clear();
            ElementControls = new Dictionary<string, Control>();
            ResizableControls = new List<Control>();
            // Create the Windows Forms elements
            if (CurrentSetting.RawPolicy.Elements is not null & CurrentSetting.Presentation is not null)
            {
                var elemDict = CurrentSetting.RawPolicy.Elements.ToDictionary(e => e.ID);
                foreach (var pres in CurrentSetting.Presentation.Elements)
                {
                    switch (pres.ElementType ?? "")
                    {
                        case "text": // A plain label
                            {
                                LabelPresentationElement textPres = (LabelPresentationElement)pres;
                                var label = new Label() { Text = textPres.Text, AutoSize = true };
                                label.Margin = new Padding(3, 6, 3, 6);
                                addControl(textPres.ID, label, "");
                                break;
                            }
                        case "decimalTextBox": // Numeric spin box or a plain text box restricted to numbers
                            {
                                NumericBoxPresentationElement decimalTextPres = (NumericBoxPresentationElement)pres;
                                DecimalPolicyElement numeric = (DecimalPolicyElement)elemDict[pres.ID];
                                Control newControl;
                                if (decimalTextPres.HasSpinner)
                                {
                                    newControl = new NumericUpDown()
                                    {
                                        Minimum = numeric.Minimum,
                                        Maximum = numeric.Maximum,
                                        Increment = decimalTextPres.SpinnerIncrement,
                                        Value = decimalTextPres.DefaultValue
                                    };
                                }
                                else
                                {
                                    var text = new TextBox();
                                    text.TextChanged += () =>
                                            {
                                                int argresult = 0;
                                                if (!int.TryParse(text.Text, out argresult))
                                                    text.Text = decimalTextPres.DefaultValue.ToString();
                                                int curNum = Conversions.ToInteger(text.Text);
                                                if (curNum > numeric.Maximum)
                                                    text.Text = numeric.Maximum.ToString();
                                                if (curNum < numeric.Minimum)
                                                    text.Text = numeric.Minimum.ToString();
                                            };
                                    text.KeyPress += (Sender, EventArgs) => { if (!(char.IsControl(EventArgs.KeyChar) | char.IsDigit(EventArgs.KeyChar))) EventArgs.Handled = true; };
                                    text.Text = decimalTextPres.DefaultValue.ToString();
                                    newControl = text;
                                }
                                addControl(pres.ID, newControl, decimalTextPres.Label);
                                break;
                            }
                        case "textBox": // Simple text box
                            {
                                TextBoxPresentationElement textboxPres = (TextBoxPresentationElement)pres;
                                TextPolicyElement text = (TextPolicyElement)elemDict[pres.ID];
                                var textbox = new TextBox()
                                {
                                    Width = (int)Math.Round(ExtraOptionsTable.Width * 0.75d),
                                    Text = textboxPres.DefaultValue,
                                    MaxLength = text.MaxLength
                                };
                                addControl(pres.ID, textbox, textboxPres.Label);
                                break;
                            }
                        case "checkBox": // Check box
                            {
                                CheckBoxPresentationElement checkPres = (CheckBoxPresentationElement)pres;
                                var checkbox = new CheckBox() { TextAlign = ContentAlignment.MiddleLeft };
                                checkbox.Text = checkPres.Text;
                                checkbox.Width = ExtraOptionsTable.ClientSize.Width;
                                using (var g = checkbox.CreateGraphics()) // Figure out how tall it should be
                                {
                                    var size = g.MeasureString(checkbox.Text, checkbox.Font, checkbox.Width);
                                    checkbox.Height = (int)Math.Round(size.Height + checkbox.Padding.Vertical + checkbox.Margin.Vertical);
                                }
                                checkbox.Checked = checkPres.DefaultState;
                                addControl(pres.ID, checkbox, "");
                                break;
                            }
                        case "comboBox": // Text box with suggestions, not tested because it's not used in any default ADML
                            {
                                ComboBoxPresentationElement comboPres = (ComboBoxPresentationElement)pres;
                                TextPolicyElement text = (TextPolicyElement)elemDict[pres.ID];
                                var combobox = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDown };
                                combobox.MaxLength = text.MaxLength;
                                combobox.Width = (int)Math.Round(ExtraOptionsTable.Width * 0.75d);
                                combobox.Text = comboPres.DefaultText;
                                combobox.Sorted = !comboPres.NoSort;
                                foreach (var suggestion in comboPres.Suggestions)
                                    combobox.Items.Add(suggestion);
                                addControl(pres.ID, combobox, comboPres.Label);
                                break;
                            }
                        case "dropdownList": // Dropdown list of options
                            {
                                DropDownPresentationElement dropdownPres = (DropDownPresentationElement)pres;
                                var combobox = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList };
                                combobox.Sorted = !dropdownPres.NoSort;
                                EnumPolicyElement enumElem = (EnumPolicyElement)elemDict[pres.ID];
                                int itemId = 0;
                                using (var g = combobox.CreateGraphics()) // Figure out how wide it should be, and add entries
                                {
                                    int maxWidth = combobox.Width;
                                    foreach (var entry in enumElem.Items)
                                    {
                                        var map = new DropdownPresentationMap() { ID = itemId, DisplayName = AdmxWorkspace.ResolveString(entry.DisplayCode, CurrentSetting.RawPolicy.DefinedIn) };
                                        float width = g.MeasureString(map.DisplayName, combobox.Font).Width + 25f; // A little extra margin
                                        if (width > maxWidth)
                                            maxWidth = (int)Math.Round(width);
                                        combobox.Items.Add(map);
                                        if (itemId == dropdownPres.DefaultItemID.GetValueOrDefault(-1))
                                            combobox.SelectedItem = map;
                                        itemId += 1;
                                    }
                                    combobox.Width = maxWidth;
                                }
                                addControl(pres.ID, combobox, dropdownPres.Label);
                                break;
                            }
                        case "listBox": // Button to launch a grid view editor
                            {
                                ListPresentationElement listPres = (ListPresentationElement)pres;
                                ListPolicyElement list = (ListPolicyElement)elemDict[pres.ID];
                                var button = new Button()
                                {
                                    UseVisualStyleBackColor = true,
                                    Text = "Edit..."
                                };
                                button.Click += () => { if (My.MyProject.Forms.ListEditor.PresentDialog(listPres.Label, button.Tag, list.UserProvidesNames) == DialogResult.OK) button.Tag = My.MyProject.Forms.ListEditor.FinalData; };
                                addControl(pres.ID, button, listPres.Label);
                                break;
                            }
                        case "multiTextBox": // Multiline text box
                            {
                                MultiTextPresentationElement multiTextPres = (MultiTextPresentationElement)pres;
                                var bigTextbox = new TextBox()
                                {
                                    AutoSize = false,
                                    Width = (int)Math.Round(ExtraOptionsPanel.Width * 0.8d),
                                    Multiline = true,
                                    ScrollBars = ScrollBars.Both,
                                    WordWrap = false,
                                    AcceptsReturn = true
                                };
                                bigTextbox.Height *= 4;
                                addControl(pres.ID, bigTextbox, multiTextPres.Label);
                                break;
                            }
                    }
                }
                OptionsTableResized();
            }
        }
        public void PreparePolicyState()
        {
            // Set the value of the UI elements depending on the current policy state
            switch (PolicyProcessing.GetPolicyState(CurrentSource, CurrentSetting))
            {
                case PolicyState.Disabled:
                    {
                        DisabledOption.Checked = true;
                        break;
                    }
                case PolicyState.Enabled:
                    {
                        EnabledOption.Checked = true;
                        var optionStates = PolicyProcessing.GetPolicyOptionStates(CurrentSource, CurrentSetting);
                        foreach (var kv in optionStates)
                        {
                            var uiControl = ElementControls[kv.Key];
                            if (kv.Value is uint) // Numeric box
                            {
                                if (uiControl is TextBox)
                                {
                                    ((TextBox)uiControl).Text = kv.Value.ToString();
                                }
                                else
                                {
                                    ((NumericUpDown)uiControl).Value = Conversions.ToDecimal(kv.Value);
                                }
                            }
                            else if (kv.Value is string) // Text box or combo box
                            {
                                if (uiControl is ComboBox)
                                {
                                    ((ComboBox)uiControl).Text = Conversions.ToString(kv.Value);
                                }
                                else
                                {
                                    ((TextBox)uiControl).Text = Conversions.ToString(kv.Value);
                                }
                            }
                            else if (kv.Value is int) // Dropdown list
                            {
                                ComboBox combobox = (ComboBox)uiControl;
                                var matchingItem = combobox.Items.OfType<DropdownPresentationMap>().FirstOrDefault(i => i.ID == Conversions.ToInteger(kv.Value));
                                if (matchingItem is not null)
                                    combobox.SelectedItem = matchingItem;
                            }
                            else if (kv.Value is bool) // Check box
                            {
                                ((CheckBox)uiControl).Checked = Conversions.ToBoolean(kv.Value);
                            }
                            else if (kv.Value is string[]) // Multiline text box
                            {
                                ((TextBox)uiControl).Lines = (string[])kv.Value;
                            }
                            else // List box (pop-out button)
                            {
                                uiControl.Tag = kv.Value;
                            }
                        }

                        break;
                    }

                default:
                    {
                        NotConfiguredOption.Checked = true;
                        break;
                    }
            }
            bool canWrite = CurrentLoader.GetWritability() != PolicySourceWritability.NoWriting;
            ApplyButton.Enabled = canWrite;
            OkButton.Enabled = canWrite;
            if (CurrentComments is null)
            {
                CommentTextbox.Enabled = false;
                CommentTextbox.Text = "Comments unavailable for this policy source";
            }
            else if (CurrentComments.ContainsKey(CurrentSetting.UniqueID))
            {
                CommentTextbox.Enabled = true;
                CommentTextbox.Text = CurrentComments[CurrentSetting.UniqueID];
            }
            else
            {
                CommentTextbox.Enabled = true;
                CommentTextbox.Text = "";
            }
        }
        public void ApplyToPolicySource()
        {
            // Write the new state to the policy source object
            PolicyProcessing.ForgetPolicy(CurrentSource, CurrentSetting);
            if (EnabledOption.Checked)
            {
                var options = new Dictionary<string, object>();
                if (CurrentSetting.RawPolicy.Elements is not null)
                {
                    foreach (var elem in CurrentSetting.RawPolicy.Elements)
                    {
                        var uiControl = ElementControls[elem.ID];
                        switch (elem.ElementType ?? "")
                        {
                            case "decimal":
                                {
                                    if (uiControl is TextBox)
                                    {
                                        options.Add(elem.ID, Conversions.ToUInteger(((TextBox)uiControl).Text));
                                    }
                                    else
                                    {
                                        options.Add(elem.ID, (uint)Math.Round(((NumericUpDown)uiControl).Value));
                                    }

                                    break;
                                }
                            case "text":
                                {
                                    if (uiControl is ComboBox)
                                    {
                                        options.Add(elem.ID, ((ComboBox)uiControl).Text);
                                    }
                                    else
                                    {
                                        options.Add(elem.ID, ((TextBox)uiControl).Text);
                                    }

                                    break;
                                }
                            case "boolean":
                                {
                                    options.Add(elem.ID, ((CheckBox)uiControl).Checked);
                                    break;
                                }
                            case "enum":
                                {
                                    options.Add(elem.ID, ((DropdownPresentationMap)((ComboBox)uiControl).SelectedItem).ID);
                                    break;
                                }
                            case "list":
                                {
                                    options.Add(elem.ID, uiControl.Tag);
                                    break;
                                }
                            case "multiText":
                                {
                                    options.Add(elem.ID, ((TextBox)uiControl).Lines);
                                    break;
                                }
                        }
                    }
                }
                PolicyProcessing.SetPolicyState(CurrentSource, CurrentSetting, PolicyState.Enabled, options);
            }
            else if (DisabledOption.Checked)
            {
                PolicyProcessing.SetPolicyState(CurrentSource, CurrentSetting, PolicyState.Disabled, null);
            }
            // Update the comment for this policy
            if (CurrentComments is not null)
            {
                if (string.IsNullOrEmpty(CommentTextbox.Text))
                {
                    if (CurrentComments.ContainsKey(CurrentSetting.UniqueID))
                        CurrentComments.Remove(CurrentSetting.UniqueID);
                }
                else if (CurrentComments.ContainsKey(CurrentSetting.UniqueID))
                    CurrentComments[CurrentSetting.UniqueID] = CommentTextbox.Text;
                else
                    CurrentComments.Add(CurrentSetting.UniqueID, CommentTextbox.Text);
            }
        }
        public DialogResult PresentDialog(PolicyPlusPolicy Policy, AdmxPolicySection Section, AdmxBundle Workspace, IPolicySource CompPolSource, IPolicySource UserPolSource, PolicyLoader CompPolLoader, PolicyLoader UserPolLoader, Dictionary<string, string> CompComments, Dictionary<string, string> UserComments)
        {
            CurrentSetting = Policy;
            CurrentSection = Section;
            AdmxWorkspace = Workspace;
            this.CompPolSource = CompPolSource;
            this.UserPolSource = UserPolSource;
            this.CompPolLoader = CompPolLoader;
            this.UserPolLoader = UserPolLoader;
            this.CompComments = CompComments;
            this.UserComments = UserComments;
            ChangesMade = false;
            return ShowDialog();
        }
        private void StateRadiosChanged(object sender, EventArgs e)
        {
            if (ElementControls is null)
                return; // A change to the tab order causes a spurious CheckedChanged
            bool allowOptions = EnabledOption.Checked;
            foreach (var kv in ElementControls)
                kv.Value.Enabled = allowOptions;
        }
        private void SectionDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isUser = SectionDropdown.Text == "User";
            CurrentSource = isUser ? UserPolSource : CompPolSource;
            CurrentLoader = isUser ? UserPolLoader : CompPolLoader;
            CurrentComments = isUser ? UserComments : CompComments;
            PreparePolicyState();
        }
        private void OkButton_Click(object sender, EventArgs e)
        {
            ApplyToPolicySource();
            DialogResult = DialogResult.OK;
        }
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ApplyToPolicySource();
            ChangesMade = true;
        }
        private void OptionsTableResized()
        {
            // Update the width limit on the extra options controls (in case the vertical scrollbar appeared or disappeared)
            if (ResizableControls is null)
                return;
            ExtraOptionsTable.MaximumSize = new Size(ExtraOptionsPanel.ClientSize.Width, 0);
            ExtraOptionsTable.MinimumSize = ExtraOptionsTable.MaximumSize;
            foreach (var ctl in ResizableControls)
                ctl.MaximumSize = new Size(ExtraOptionsPanel.ClientSize.Width, 0);
        }
        private void EditSetting_Resize(object sender, EventArgs e)
        {
            // Share the extra width between the two halves of the form
            int extraWidth = Width - 654;
            ExtraOptionsPanel.Width = (int)Math.Round(299d + extraWidth / 2d);
            HelpTextbox.Width = (int)Math.Round(309d + extraWidth / 2d);
            HelpTextbox.Left = ExtraOptionsPanel.Left + ExtraOptionsPanel.Width + 6;
            CommentTextbox.Left = HelpTextbox.Left;
            CommentTextbox.Width = HelpTextbox.Width;
            SupportedTextbox.Left = HelpTextbox.Left;
            SupportedTextbox.Width = HelpTextbox.Width;
            CommentLabel.Left = CommentTextbox.Left - 57;
            SupportedLabel.Left = SupportedTextbox.Left - 77;
            OptionsTableResized();
        }
        private void EditSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ChangesMade)
                DialogResult = DialogResult.OK;
        }
        private class DropdownPresentationMap // Used for keeping the ID with an option in dropdown boxes
        {
            public int ID;
            public string DisplayName;
            public override string ToString()
            {
                return DisplayName;
            }
        }
    }
}