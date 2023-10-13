﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace PolicyPlus.UI
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
                        EntriesDatagrid.Rows.Add(kv.Key, kv.Value);
                    }
                }
                else
                {
                    foreach (var entry in (List<string>)data)
                    {
                        EntriesDatagrid.Rows.Add("", entry);
                    }
                }
            }
            FinalData = null;
            return ShowDialog();
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

                    var key = Conversions.ToString(row.Cells[0].Value);
                    if (dict.ContainsKey(key))
                    {
                        Interaction.MsgBox("Multiple entries are named \"" + key + "\"! Remove or rename all but one.", MsgBoxStyle.Exclamation);
                        return;
                    }

                    dict.Add(key, Conversions.ToString(row.Cells[1].Value));
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
                        list.Add(Conversions.ToString(row.Cells[1].Value));
                    }
                }
                FinalData = list;
            }
            DialogResult = DialogResult.OK;
        }
    }
}