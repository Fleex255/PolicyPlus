using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PolicyPlus.csharp.Models;
using PolicyPlus.csharp.UI.Elements;

namespace PolicyPlus.csharp.UI
{
    public partial class EditSetting
    {
        private PolicyPlusPolicy _currentSetting;
        private AdmxPolicySection _currentSection;
        private AdmxBundle _admxWorkspace;
        private IPolicySource _compPolSource, _userPolSource;
        private PolicyLoader _compPolLoader, _userPolLoader;
        private Dictionary<string, string> _compComments, _userComments;

        // Above: passed in; below: internal state
        private Dictionary<string, Control> _elementControls;

        private List<Control> _resizableControls;
        private IPolicySource _currentSource;
        private PolicyLoader _currentLoader;
        private Dictionary<string, string> _currentComments;
        private bool _changesMade; // To either side

        public EditSetting()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e) => DialogResult = _changesMade ? DialogResult.OK : DialogResult.Cancel;

        private void EditSetting_Shown(object sender, EventArgs e)
        {
            SettingNameLabel.Text = _currentSetting.DisplayName;
            SupportedTextbox.Text = _currentSetting.SupportedOn is null ? "" : _currentSetting.SupportedOn.DisplayName;

            HelpTextbox.Text = Main.PrettifyDescription(_currentSetting.DisplayExplanation);
            if (_currentSetting.RawPolicy.Section == AdmxPolicySection.Both)
            {
                SectionDropdown.Enabled = true;
                _currentSection = _currentSection == AdmxPolicySection.Both ? AdmxPolicySection.Machine : _currentSection;
            }
            else
            {
                SectionDropdown.Enabled = false;
                _currentSection = _currentSetting.RawPolicy.Section;
            }
            ExtraOptionsPanel.HorizontalScroll.Maximum = 0;
            ExtraOptionsPanel.VerticalScroll.Visible = true;
            ExtraOptionsPanel.AutoScroll = true;
            PreparePolicyElements();
            SectionDropdown.Text = _currentSection == AdmxPolicySection.Machine ? "Computer" : "User";
            SectionDropdown_SelectedIndexChanged(null, null); // Force an update of the current source
            PreparePolicyState();
            StateRadiosChanged(null, null);
        }

        public void PreparePolicyElements()
        {
            for (var n = ExtraOptionsTable.RowCount - 1; n >= 0; --n) // Go backwards because Dispose changes the indexes
            {
                if (ExtraOptionsTable.GetControlFromPosition(0, n) is { } ctl)
                {
                    ctl.Dispose();
                }
            }
            ExtraOptionsTable.Controls.Clear();
            ExtraOptionsTable.RowCount = 0;
            var curTabIndex = 10;

            ExtraOptionsTable.RowStyles.Clear();
            _elementControls = new Dictionary<string, Control>();
            _resizableControls = new List<Control>();
            // Create the Windows Forms elements
            if (_currentSetting.RawPolicy.Elements is null || _currentSetting.Presentation is null)
            {
                return;
            }

            var elemDict = _currentSetting.RawPolicy.Elements.ToDictionary(e => e.Id);
            foreach (var pres in _currentSetting.Presentation.Elements)
            {
                switch (pres.ElementType ?? "")
                {
                    case "text": // A plain label
                        {
                            var textPres = (LabelPresentationElement)pres;
                            var label = new Label
                            {
                                Text = textPres.Text,
                                AutoSize = true,
                                Margin = new Padding(3, 6, 3, 6)
                            };
                            AddControl(textPres.Id, label, "", ref curTabIndex);
                            break;
                        }
                    case "decimalTextBox": // Numeric spin box or a plain text box restricted to numbers
                        {
                            var decimalTextPres = (NumericBoxPresentationElement)pres;
                            var numeric = (DecimalPolicyElement)elemDict[pres.Id];
                            Control newControl;
                            if (decimalTextPres.HasSpinner)
                            {
                                newControl = new NumericUpDown
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
                                text.TextChanged += (sender, e) =>
                                {
                                    if (!int.TryParse(text.Text, out var argresult))
                                    {
                                        text.Text = decimalTextPres.DefaultValue.ToString();
                                    }

                                    var curNum = int.Parse(text.Text);
                                    if (curNum > numeric.Maximum)
                                    {
                                        text.Text = numeric.Maximum.ToString();
                                    }

                                    if (curNum < numeric.Minimum)
                                    {
                                        text.Text = numeric.Minimum.ToString();
                                    }
                                };

                                text.KeyPress += (sender, eventArgs) =>
                                {
                                    if (!(char.IsControl(eventArgs.KeyChar) || char.IsDigit(eventArgs.KeyChar)))
                                    {
                                        eventArgs.Handled = true;
                                    }
                                };
                                text.Text = decimalTextPres.DefaultValue.ToString();
                                newControl = text;
                            }

                            AddControl(pres.Id, newControl, decimalTextPres.Label, ref curTabIndex);
                            break;
                        }
                    case "textBox": // Simple text box
                        {
                            var textboxPres = (TextBoxPresentationElement)pres;
                            var text = (TextPolicyElement)elemDict[pres.Id];
                            var textbox = new TextBox
                            {
                                Width = (int)Math.Round(ExtraOptionsTable.Width * 0.75d),
                                Text = textboxPres.DefaultValue,
                                MaxLength = text.MaxLength
                            };
                            AddControl(pres.Id, textbox, textboxPres.Label, ref curTabIndex);
                            break;
                        }
                    case "checkBox": // Check box
                        {
                            var checkPres = (CheckBoxPresentationElement)pres;
                            var checkbox = new CheckBox
                            {
                                TextAlign = ContentAlignment.MiddleLeft,
                                Text = checkPres.Text,
                                Width = ExtraOptionsTable.ClientSize.Width
                            };
                            using (var g = checkbox.CreateGraphics()) // Figure out how tall it should be
                            {
                                var size = g.MeasureString(checkbox.Text, checkbox.Font, checkbox.Width);
                                checkbox.Height =
                                    (int)Math.Round(size.Height + checkbox.Padding.Vertical + checkbox.Margin.Vertical);
                            }

                            checkbox.Checked = checkPres.DefaultState;
                            AddControl(pres.Id, checkbox, "", ref curTabIndex);
                            break;
                        }
                    case "comboBox": // Text box with suggestions, not tested because it's not used in any default ADML
                        {
                            var comboPres = (ComboBoxPresentationElement)pres;
                            var text = (TextPolicyElement)elemDict[pres.Id];
                            var combobox = new ComboBox
                            {
                                DropDownStyle = ComboBoxStyle.DropDown,
                                MaxLength = text.MaxLength,
                                Width = (int)Math.Round(ExtraOptionsTable.Width * 0.75d),
                                Text = comboPres.DefaultText,
                                Sorted = !comboPres.NoSort
                            };
                            foreach (var suggestion in comboPres.Suggestions)
                            {
                                _ = combobox.Items.Add(suggestion);
                            }

                            AddControl(pres.Id, combobox, comboPres.Label, ref curTabIndex);
                            break;
                        }
                    case "dropdownList": // Dropdown list of options
                        {
                            var dropdownPres = (DropDownPresentationElement)pres;
                            var combobox = new ComboBox
                            {
                                DropDownStyle = ComboBoxStyle.DropDownList,
                                Sorted = !dropdownPres.NoSort
                            };
                            var enumElem = (EnumPolicyElement)elemDict[pres.Id];
                            var itemId = 0;
                            using (var g = combobox.CreateGraphics()) // Figure out how wide it should be, and add entries
                            {
                                var maxWidth = combobox.Width;
                                foreach (var entry in enumElem.Items)
                                {
                                    var map = new DropdownPresentationMap
                                    {
                                        Id = itemId,
                                        DisplayName = _admxWorkspace.ResolveString(entry.DisplayCode,
                                            _currentSetting.RawPolicy.DefinedIn)
                                    };
                                    var width = g.MeasureString(map.DisplayName, combobox.Font).Width +
                                                25f; // A little extra margin
                                    if (width > maxWidth)
                                    {
                                        maxWidth = (int)Math.Round(width);
                                    }

                                    _ = combobox.Items.Add(map);
                                    if (itemId == (dropdownPres.DefaultItemId ?? -1))
                                    {
                                        combobox.SelectedItem = map;
                                    }

                                    itemId++;
                                }

                                combobox.Width = maxWidth;
                            }

                            AddControl(pres.Id, combobox, dropdownPres.Label, ref curTabIndex);
                            break;
                        }
                    case "listBox": // Button to launch a grid view editor
                        {
                            var listPres = (ListPresentationElement)pres;
                            var list = (ListPolicyElement)elemDict[pres.Id];
                            var button = new Button
                            {
                                UseVisualStyleBackColor = true,
                                Text = "Edit..."
                            };
                            button.Click += (sender, e) =>
                            {
                                var dialog = new ListEditor();
                                if (dialog.PresentDialog(listPres.Label, button.Tag,
                                        list.UserProvidesNames) == DialogResult.OK)
                                {
                                    button.Tag = dialog.FinalData;
                                }
                            };
                            AddControl(pres.Id, button, listPres.Label, ref curTabIndex);
                            break;
                        }
                    case "multiTextBox": // Multiline text box
                        {
                            var multiTextPres = (MultiTextPresentationElement)pres;
                            var bigTextbox = new TextBox
                            {
                                AutoSize = false,
                                Width = (int)Math.Round(ExtraOptionsPanel.Width * 0.8d),
                                Multiline = true,
                                ScrollBars = ScrollBars.Both,
                                WordWrap = false,
                                AcceptsReturn = true
                            };
                            bigTextbox.Height *= 4;
                            AddControl(pres.Id, bigTextbox, multiTextPres.Label, ref curTabIndex);
                            break;
                        }
                }
            }

            OptionsTableResized();
        }

        private void AddControl(string id, Control control, string label, ref int curTabIndex)
        {
            _ = ExtraOptionsTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            if (string.IsNullOrEmpty(label)) // Just a single control
            {
                if (control.AutoSize)
                {
                    _resizableControls.Add(control);
                }

                ExtraOptionsTable.Controls.Add(control, 0, ExtraOptionsTable.RowStyles.Count - 1);
            }
            else // Has a label attached
            {
                var flowPanel = new FlowLayoutPanel
                {
                    WrapContents = true,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0)
                };
                ExtraOptionsTable.Controls.Add(flowPanel, 0, ExtraOptionsTable.RowStyles.Count - 1);
                var labelControl = new Label
                {
                    AutoSize = true,
                    Text = label,
                    Anchor = AnchorStyles.Left
                };
                control.Anchor = AnchorStyles.Left;
                flowPanel.Controls.Add(labelControl);
                flowPanel.Controls.Add(control);
                _resizableControls.Add(flowPanel);
            }
            if (!string.IsNullOrEmpty(id))
            {
                _elementControls.Add(id, control);
                control.TabStop = true;
                control.TabIndex = curTabIndex;
                curTabIndex++;
            }
            else
            {
                control.TabStop = false;
            }
        }

        public void PreparePolicyState()
        {
            // Set the value of the UI elements depending on the current policy state
            switch (PolicyProcessing.GetPolicyState(_currentSource, _currentSetting))
            {
                case PolicyState.Disabled:
                    {
                        DisabledOption.Checked = true;
                        break;
                    }
                case PolicyState.Enabled:
                    {
                        EnabledOption.Checked = true;
                        foreach (var kv in PolicyProcessing.GetPolicyOptionStates(_currentSource, _currentSetting))
                        {
                            var uiControl = _elementControls[kv.Key];
                            switch (kv.Value)
                            {
                                // Numeric box
                                case uint when uiControl is TextBox box:
                                    box.Text = kv.Value.ToString();
                                    break;

                                case uint:
                                    ((NumericUpDown)uiControl).Value = Convert.ToDecimal(kv.Value);
                                    break;
                                // Text box or combo box
                                case string when uiControl is ComboBox box:
                                    box.Text = kv.Value.ToString();
                                    break;

                                case string:
                                    ((TextBox)uiControl).Text = kv.Value.ToString();
                                    break;
                                // Dropdown list
                                case int:
                                    {
                                        var combobox = (ComboBox)uiControl;
                                        if (combobox.Items.OfType<DropdownPresentationMap>().FirstOrDefault(i => i.Id == (int)kv.Value) is { } matchingItem)
                                        {
                                            combobox.SelectedItem = matchingItem;
                                        }

                                        break;
                                    }
                                // Check box
                                case bool:
                                    ((CheckBox)uiControl).Checked = (bool)kv.Value;
                                    break;
                                // Multiline text box
                                case string[] value:
                                    ((TextBox)uiControl).Lines = value;
                                    break;
                                // List box (pop-out button)
                                default:
                                    uiControl.Tag = kv.Value;
                                    break;
                            }
                        }

                        break;
                    }

                case PolicyState.NotConfigured:
                case PolicyState.Unknown:
                default:
                    {
                        NotConfiguredOption.Checked = true;
                        break;
                    }
            }
            var canWrite = _currentLoader.GetWritability() != PolicySourceWritability.NoWriting;
            ApplyButton.Enabled = canWrite;
            OkButton.Enabled = canWrite;
            if (_currentComments is null)
            {
                CommentTextbox.Enabled = false;
                CommentTextbox.Text = "Comments unavailable for this policy source";
            }
            else if (_currentComments.TryGetValue(_currentSetting.UniqueId, out var comment))
            {
                CommentTextbox.Enabled = true;
                CommentTextbox.Text = comment;
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
            PolicyProcessing.ForgetPolicy(_currentSource, _currentSetting);
            if (EnabledOption.Checked)
            {
                var options = new Dictionary<string, object>();
                if (_currentSetting.RawPolicy.Elements is not null)
                {
                    foreach (var elem in _currentSetting.RawPolicy.Elements)
                    {
                        var uiControl = _elementControls[elem.Id];
                        switch (elem.ElementType ?? "")
                        {
                            case "decimal":
                                {
                                    if (uiControl is TextBox box)
                                    {
                                        options.Add(elem.Id, Convert.ToUInt32(box.Text));
                                    }
                                    else
                                    {
                                        options.Add(elem.Id, (uint)Math.Round(((NumericUpDown)uiControl).Value));
                                    }

                                    break;
                                }
                            case "text":
                                {
                                    if (uiControl is ComboBox box)
                                    {
                                        options.Add(elem.Id, box.Text);
                                    }
                                    else
                                    {
                                        options.Add(elem.Id, ((TextBox)uiControl).Text);
                                    }

                                    break;
                                }
                            case "boolean":
                                {
                                    options.Add(elem.Id, ((CheckBox)uiControl).Checked);
                                    break;
                                }
                            case "enum":
                                {
                                    options.Add(elem.Id, ((DropdownPresentationMap)((ComboBox)uiControl).SelectedItem).Id);
                                    break;
                                }
                            case "list":
                                {
                                    options.Add(elem.Id, uiControl.Tag);
                                    break;
                                }
                            case "multiText":
                                {
                                    options.Add(elem.Id, ((TextBox)uiControl).Lines);
                                    break;
                                }
                        }
                    }
                }
                PolicyProcessing.SetPolicyState(_currentSource, _currentSetting, PolicyState.Enabled, options);
            }
            else if (DisabledOption.Checked)
            {
                PolicyProcessing.SetPolicyState(_currentSource, _currentSetting, PolicyState.Disabled, null);
            }
            if (_currentComments is null)
            {
                return;
            }

            // Update the comment for this policy
            if (string.IsNullOrEmpty(CommentTextbox.Text))
            {
                if (_currentComments.ContainsKey(_currentSetting.UniqueId))
                {
                    _ = _currentComments.Remove(_currentSetting.UniqueId);
                }
            }
            else if (_currentComments.ContainsKey(_currentSetting.UniqueId))
            {
                _currentComments[_currentSetting.UniqueId] = CommentTextbox.Text;
            }
            else
            {
                _currentComments.Add(_currentSetting.UniqueId, CommentTextbox.Text);
            }
        }

        public DialogResult PresentDialog(PolicyPlusPolicy policy, AdmxPolicySection section, AdmxBundle workspace, IPolicySource compPolSource, IPolicySource userPolSource, PolicyLoader compPolLoader, PolicyLoader userPolLoader, Dictionary<string, string> compComments, Dictionary<string, string> userComments)
        {
            _currentSetting = policy;
            _currentSection = section;
            _admxWorkspace = workspace;
            _compPolSource = compPolSource;
            _userPolSource = userPolSource;
            _compPolLoader = compPolLoader;
            _userPolLoader = userPolLoader;
            _compComments = compComments;
            _userComments = userComments;
            _changesMade = false;
            return ShowDialog();
        }

        private void StateRadiosChanged(object? sender, EventArgs? e)
        {
            if (_elementControls is null)
            {
                return; // A change to the tab order causes a spurious CheckedChanged
            }

            var allowOptions = EnabledOption.Checked;
            foreach (var kv in _elementControls)
            {
                kv.Value.Enabled = allowOptions;
            }
        }

        private void SectionDropdown_SelectedIndexChanged(object? sender, EventArgs? e)
        {
            var isUser = SectionDropdown.Text == "User";
            _currentSource = isUser ? _userPolSource : _compPolSource;
            _currentLoader = isUser ? _userPolLoader : _compPolLoader;
            _currentComments = isUser ? _userComments : _compComments;
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
            _changesMade = true;
        }

        private void OptionsTableResized()
        {
            // Update the width limit on the extra options controls (in case the vertical scrollbar
            // appeared or disappeared)
            if (_resizableControls is null)
            {
                return;
            }

            ExtraOptionsTable.MaximumSize = new Size(ExtraOptionsPanel.ClientSize.Width, 0);
            ExtraOptionsTable.MinimumSize = ExtraOptionsTable.MaximumSize;
            foreach (var ctl in _resizableControls)
            {
                ctl.MaximumSize = new Size(ExtraOptionsPanel.ClientSize.Width, 0);
            }
        }

        private void EditSetting_Resize(object sender, EventArgs e)
        {
            // Share the extra width between the two halves of the form
            var extraWidth = Width - 654;
            ExtraOptionsPanel.Width = (int)Math.Round(299d + (extraWidth / 2d));
            HelpTextbox.Width = (int)Math.Round(309d + (extraWidth / 2d));
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
            if (_changesMade)
            {
                DialogResult = DialogResult.OK;
            }
        }

        private class DropdownPresentationMap // Used for keeping the ID with an option in dropdown boxes
        {
            public int Id;
            public string DisplayName;

            public override string ToString() => DisplayName;
        }
    }
}