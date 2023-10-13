using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace PolicyPlus.csharp.UI
{
    public partial class ListEditor
    {
        private bool _userProvidesNames;
        public object FinalData;

        public ListEditor()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(string title, object data, bool twoColumn)
        {
            _userProvidesNames = twoColumn;
            NameColumn.Visible = twoColumn;
            ElementNameLabel.Text = title;
            EntriesDatagrid.Rows.Clear();
            if (data is not null)
            {
                if (twoColumn)
                {
                    foreach (var kv in (Dictionary<string, string>)data)
                    {
                        _ = EntriesDatagrid.Rows.Add(kv.Key, kv.Value);
                    }
                }
                else
                {
                    foreach (var entry in (List<string>)data)
                    {
                        _ = EntriesDatagrid.Rows.Add("", entry);
                    }
                }
            }
            FinalData = null;
            return ShowDialog(this);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (_userProvidesNames)
            {
                // Check for duplicate keys
                var dict = new Dictionary<string, string>();
                foreach (DataGridViewRow row in EntriesDatagrid.Rows)
                {
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    var key = row.Cells[0].Value.ToString();
                    if (dict.ContainsKey(key))
                    {
                        _ = MessageBox.Show("Multiple entries are named \"" + key + "\"! Remove or rename all but one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    dict.Add(key, row.Cells[1].Value.ToString());
                }
                FinalData = dict;
            }
            else
            {
                var list = new List<string>();
                foreach (DataGridViewRow row in EntriesDatagrid.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        list.Add(row.Cells[1].Value.ToString());
                    }
                }
                FinalData = list;
            }
            DialogResult = DialogResult.OK;
        }
    }
}