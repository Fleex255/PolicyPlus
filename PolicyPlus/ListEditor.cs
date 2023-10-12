using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace PolicyPlus
{
    public partial class ListEditor
    {
        private bool UserProvidesNames;
        public object FinalData;

        public ListEditor()
        {
            InitializeComponent();
        }
        public DialogResult PresentDialog(string Title, object Data, bool TwoColumn)
        {
            UserProvidesNames = TwoColumn;
            NameColumn.Visible = TwoColumn;
            ElementNameLabel.Text = Title;
            EntriesDatagrid.Rows.Clear();
            if (Data is not null)
            {
                if (TwoColumn)
                {
                    Dictionary<string, string> dict = (Dictionary<string, string>)Data;
                    foreach (var kv in dict)
                        EntriesDatagrid.Rows.Add(kv.Key, kv.Value);
                }
                else
                {
                    List<string> list = (List<string>)Data;
                    foreach (var entry in list)
                        EntriesDatagrid.Rows.Add("", entry);
                }
            }
            FinalData = null;
            return ShowDialog();
        }
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (UserProvidesNames)
            {
                // Check for duplicate keys
                var dict = new Dictionary<string, string>();
                foreach (DataGridViewRow row in EntriesDatagrid.Rows)
                {
                    if (row.IsNewRow)
                        continue;
                    string key = Conversions.ToString(row.Cells[0].Value);
                    if (dict.ContainsKey(key))
                    {
                        Interaction.MsgBox("Multiple entries are named \"" + key + "\"! Remove or rename all but one.", MsgBoxStyle.Exclamation);
                        return;
                    }
                    else
                    {
                        dict.Add(key, Conversions.ToString(row.Cells[1].Value));
                    }
                }
                FinalData = dict;
            }
            else
            {
                var list = new List<string>();
                foreach (DataGridViewRow row in EntriesDatagrid.Rows)
                {
                    if (!row.IsNewRow)
                        list.Add(Conversions.ToString(row.Cells[1].Value));
                }
                FinalData = list;
            }
            DialogResult = DialogResult.OK;
        }
    }
}