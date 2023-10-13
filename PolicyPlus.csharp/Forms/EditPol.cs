using System;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Win32;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class EditPol
    {
        public PolFile EditingPol { get; set; }
        private bool _editingUserSource;

        public EditPol()
        {
            InitializeComponent();
        }

        public void UpdateTree()
        {
            // Repopulate the main list view, keeping the scroll position in the same place
            var topItemIndex = default(int?);
            if (LsvPol.TopItem is not null)
            {
                topItemIndex = LsvPol.TopItem.Index;
            }

            LsvPol.BeginUpdate();
            LsvPol.Items.Clear();
            AddKey("", 0);
            LsvPol.EndUpdate();
            if (topItemIndex.HasValue && LsvPol.Items.Count > topItemIndex.Value)
            {
                LsvPol.TopItem = LsvPol.Items[topItemIndex.Value];
            }
        }

        private void AddKey(string prefix, int depth)
        {
            var subkeys = EditingPol.GetKeyNames(prefix);
            subkeys.Sort(StringComparer.InvariantCultureIgnoreCase);
            foreach (var subkey in subkeys)
            {
                var keypath = string.IsNullOrEmpty(prefix) ? subkey : prefix + @"\" + subkey;
                var lsvi = LsvPol.Items.Add(subkey);
                lsvi.IndentCount = depth;
                lsvi.ImageIndex = 0; // Folder
                lsvi.Tag = keypath;
                AddKey(keypath, depth + 1);
            }
            var values = EditingPol.GetValueNames(prefix, false);
            values.Sort(StringComparer.InvariantCultureIgnoreCase);
            var iconIndex = default(int);
            foreach (var value in values)
            {
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                var data = EditingPol.GetValue(prefix, value);
                var kind = EditingPol.GetValueKind(prefix, value);

                if (value.Equals("**deletevalues", StringComparison.InvariantCultureIgnoreCase))
                {
                    _ = AddToLsv("Delete values", 8, true, depth, value, prefix, kind, data!).SubItems.Add(data.ToString());
                }
                else if (value.StartsWith("**del.", StringComparison.InvariantCultureIgnoreCase))
                {
                    _ = AddToLsv("Delete value", 8, true, depth, value, prefix, kind, data!).SubItems.Add(value.Substring(6));
                }
                else if (value.StartsWith("**delvals", StringComparison.InvariantCultureIgnoreCase))
                {
                    _ = AddToLsv("Delete all values", 8, true, depth, value, prefix, kind, data!);
                }
                else
                {
                    var text = "";
                    switch (data)
                    {
                        case string[] strings:
                            text = string.Join(" ", strings);
                            iconIndex = 39; // Two pages
                            break;

                        case string:
                            text = data.ToString();
                            iconIndex = kind == RegistryValueKind.ExpandString ? 42 : 40; // One page with arrow, or without
                            break;

                        case uint:
                            text = data.ToString();
                            iconIndex = 15; // Calculator
                            break;

                        case ulong:
                            text = data.ToString();
                            iconIndex = 41; // Calculator+
                            break;

                        case byte[] bytes:
                            text = BitConverter.ToString(bytes).Replace("-", " ");
                            iconIndex = 13; // Gear
                            break;
                    }
                    _ = AddToLsv(value, iconIndex, false, depth, value, prefix, kind, data!).SubItems.Add(text);
                }
            }
        }

        private ListViewItem AddToLsv(string itemText, int icon, bool deletion, int depth, string value, string prefix, RegistryValueKind kind, object data)
        {
            var lsvItem = LsvPol.Items.Add(itemText, icon);
            lsvItem.IndentCount = depth;
            var tag = new PolValueInfo { Name = value, Key = prefix };
            if (deletion)
            {
                tag.IsDeleter = true;
            }
            else
            {
                tag.Kind = kind;
                tag.Data = data;
            }
            lsvItem.Tag = tag;
            return lsvItem;
        }

        public void PresentDialog(ImageList images, PolFile pol, bool isUser)
        {
            LsvPol.SmallImageList = images;
            EditingPol = pol;
            _editingUserSource = isUser;
            UpdateTree();
            ChItem.Width = LsvPol.ClientSize.Width - ChValue.Width - SystemInformation.VerticalScrollBarWidth;
            LsvPol_SelectedIndexChanged(null, null);
            _ = ShowDialog(this);
        }

        private void ButtonSave_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

        private void EditPol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        public void SelectKey(string keyPath)
        {
            var lsvi = LsvPol.Items.OfType<ListViewItem>().FirstOrDefault(i => i.Tag is string && keyPath.Equals(i.Tag.ToString(), StringComparison.InvariantCultureIgnoreCase));
            if (lsvi is null)
            {
                return;
            }

            lsvi.Selected = true;
            lsvi.EnsureVisible();
        }

        public void SelectValue(string keyPath, string valueName)
        {
            var lsvi = LsvPol.Items.OfType<ListViewItem>().FirstOrDefault((item) =>
    {
        if (item.Tag is not PolValueInfo)
        {
            return false;
        }

        var pvi = (PolValueInfo)item.Tag;
        return pvi.Key.Equals(keyPath, StringComparison.InvariantCultureIgnoreCase) && pvi.Name.Equals(valueName, StringComparison.InvariantCultureIgnoreCase);
    });
            if (lsvi is null)
            {
                return;
            }

            lsvi.Selected = true;
            lsvi.EnsureVisible();
        }

        public bool IsKeyNameValid(string name) => !name.Contains('\\');

        public bool IsKeyNameAvailable(string containerPath, string name) => !EditingPol.GetKeyNames(containerPath).Any(k => k.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        private void ButtonAddKey_Click(object sender, EventArgs e)
        {
            var dialog = new EditPolKey();
            var keyName = dialog.PresentDialog("");
            if (string.IsNullOrEmpty(keyName))
            {
                return;
            }

            if (!IsKeyNameValid(keyName))
            {
                _ = MessageBox.Show("The key name is not valid.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var containerKey = (LsvPol.SelectedItems.Count > 0 ? LsvPol.SelectedItems[0].Tag : "").ToString();
            if (!IsKeyNameAvailable(containerKey!, keyName))
            {
                _ = MessageBox.Show("The key name is already taken.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var newPath = string.IsNullOrEmpty(containerKey) ? keyName : containerKey + @"\" + keyName;
            EditingPol.SetValue(newPath, "", Array.CreateInstance(typeof(byte), 0), RegistryValueKind.None);
            UpdateTree();
            SelectKey(newPath);
        }

        public object? PromptForNewValueData(string valueName, object currentData, RegistryValueKind kind)
        {
            if (kind is RegistryValueKind.String or RegistryValueKind.ExpandString)
            {
                var dialog = new EditPolStringData();
                if (dialog.PresentDialog(valueName, currentData!.ToString()) == DialogResult.OK)
                {
                    return dialog.TextData.Text;
                }

                return null;
            }

            if (kind is RegistryValueKind.DWord or RegistryValueKind.QWord)
            {
                var dialog = new EditPolNumericData();

                if (dialog.PresentDialog(valueName, (ulong)currentData, kind == RegistryValueKind.QWord) == DialogResult.OK)
                {
                    return dialog.NumData.Value;
                }

                return null;
            }
            if (kind == RegistryValueKind.MultiString)
            {
                var dialog = new EditPolMultiStringData();

                if (dialog.PresentDialog(valueName, (string[])currentData) == DialogResult.OK)
                {
                    return dialog.TextData.Lines;
                }

                return null;
            }
            _ = MessageBox.Show("This value kind is not supported.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return null;
        }

        private void ButtonAddValue_Click(object sender, EventArgs e)
        {
            var dialog = new EditPolValue();
            var keyPath = LsvPol.SelectedItems[0].Tag.ToString();
            if (dialog.PresentDialog() != DialogResult.OK)
            {
                return;
            }

            var value = dialog.ChosenName;
            var kind = dialog.SelectedKind;
            object defaultData;
            if (kind is RegistryValueKind.String or RegistryValueKind.ExpandString)
            {
                defaultData = "";
            }
            else if (kind is RegistryValueKind.DWord or RegistryValueKind.QWord)
            {
                defaultData = 0;
            }
            else // Multi-string
            {
                defaultData = Array.CreateInstance(typeof(string), 0);
            }

            if (PromptForNewValueData(value, defaultData, kind) is { } newData)
            {
                EditingPol.SetValue(keyPath!, value, newData, kind);
                UpdateTree();
                SelectValue(keyPath, value);
            }
        }

        private void ButtonDeleteValue_Click(object sender, EventArgs e)
        {
            var dialog = new EditPolDelete();
            var tag = LsvPol.SelectedItems[0].Tag;
            if (tag is string tagString)
            {
                if (dialog.PresentDialog(tagString.Split(@"\").Last()) != DialogResult.OK)
                {
                    return;
                }

                if (dialog.OptPurge.Checked)
                {
                    EditingPol.ClearKey(tagString); // Delete everything
                }
                else if (dialog.OptClearFirst.Checked)
                {
                    EditingPol.ForgetKeyClearance(tagString); // So the clearance is before the values in the POL
                    EditingPol.ClearKey(tagString);
                    // Add the existing values back
                    for (var index = LsvPol.SelectedIndices[0] + 1; index < LsvPol.Items.Count; index++)
                    {
                        var subItem = LsvPol.Items[index];
                        if (subItem.IndentCount <= LsvPol.SelectedItems[0].IndentCount)
                        {
                            break;
                        }

                        if (subItem.IndentCount != LsvPol.SelectedItems[0].IndentCount + 1 ||
                            subItem.Tag is not PolValueInfo valueInfo)
                        {
                            continue;
                        }

                        if (!valueInfo.IsDeleter)
                        {
                            EditingPol.SetValue(valueInfo.Key, valueInfo.Name, valueInfo.Data, valueInfo.Kind);
                        }
                    }
                }
                else
                {
                    // Delete only the specified value
                    EditingPol.DeleteValue(tagString, dialog.TextValueName.Text);
                }
                UpdateTree();
                SelectKey(tagString);
            }
            else
            {
                var info = (PolValueInfo)tag;
                EditingPol.DeleteValue(info.Key, info.Name);
                UpdateTree();
                SelectValue(info.Key, "**del." + info.Name);
            }
        }

        private void ButtonForget_Click(object sender, EventArgs e)
        {
            var containerKey = "";
            var tag = LsvPol.SelectedItems[0].Tag;
            if (tag is string)
            {
                if (MessageBox.Show("Are you sure you want to remove this key and all its contents?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    return;
                }

                var keyPath = tag.ToString();
                if (keyPath.Contains('\\'))
                {
                    containerKey = keyPath.Remove(keyPath.LastIndexOf('\\'));
                }

                RemoveKey(keyPath);
            }
            else
            {
                var info = (PolValueInfo)tag;
                containerKey = info.Key;
                EditingPol.ForgetValue(info.Key, info.Name);
            }
            UpdateTree();
            if (!string.IsNullOrEmpty(containerKey))
            {
                var pathParts = containerKey.Split(@"\");
                for (int n = 1, loopTo = pathParts.Length; n <= loopTo; n++)
                {
                    SelectKey(string.Join(@"\", pathParts.Take(n)));
                }
            }
            else
            {
                LsvPol_SelectedIndexChanged(null, null);
            }
        }

        private void RemoveKey(string key)
        {
            foreach (var subkey in EditingPol.GetKeyNames(key))
            {
                RemoveKey(key + @"\" + subkey);
            }

            EditingPol.ClearKey(key);
            EditingPol.ForgetKeyClearance(key);
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            var info = (PolValueInfo)LsvPol.SelectedItems[0].Tag;
            if (PromptForNewValueData(info.Name, info.Data, info.Kind) is not { } newData)
            {
                return;
            }

            EditingPol.SetValue(info.Key, info.Name, newData, info.Kind);
            UpdateTree();
            SelectValue(info.Key, info.Name);
        }

        private void LsvPol_SelectedIndexChanged(object? sender, EventArgs? e)
        {
            // Update the enabled status of all the buttons
            if (LsvPol.SelectedItems.Count == 0)
            {
                ButtonAddKey.Enabled = true;
                ButtonAddValue.Enabled = false;
                ButtonDeleteValue.Enabled = false;
                ButtonEdit.Enabled = false;
                ButtonForget.Enabled = false;
                ButtonExport.Enabled = true;
            }
            else
            {
                var tag = LsvPol.SelectedItems[0].Tag;
                ButtonForget.Enabled = true;
                if (tag is string) // It's a key
                {
                    ButtonAddKey.Enabled = true;
                    ButtonAddValue.Enabled = true;
                    ButtonEdit.Enabled = false;
                    ButtonDeleteValue.Enabled = true;
                    ButtonExport.Enabled = true;
                }
                else // It's a value
                {
                    ButtonAddKey.Enabled = false;
                    ButtonAddValue.Enabled = false;
                    var delete = ((PolValueInfo)tag).IsDeleter;
                    ButtonEdit.Enabled = !delete;
                    ButtonDeleteValue.Enabled = !delete;
                    ButtonExport.Enabled = false;
                }
            }
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            var dialog = new ImportReg();
            if (dialog.PresentDialog(EditingPol) == DialogResult.OK)
            {
                UpdateTree();
            }
        }

        private void ButtonExport_Click(object sender, EventArgs e)
        {
            var dialog = new ExportReg();
            var branch = "";
            if (LsvPol.SelectedItems.Count > 0)
            {
                branch = LsvPol.SelectedItems[0].Tag.ToString();
            }

            dialog.PresentDialog(branch!, EditingPol, _editingUserSource);
        }

        private class PolValueInfo
        {
            public string Key;
            public string Name;
            public RegistryValueKind Kind;
            public object Data;
            public bool IsDeleter;
        }
    }
}