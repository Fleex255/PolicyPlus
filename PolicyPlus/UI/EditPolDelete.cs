﻿using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PolicyPlus.UI
{
    public partial class EditPolDelete
    {
        public EditPolDelete()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(string containerKey)
        {
            OptClearFirst.Checked = false;
            OptDeleteOne.Checked = false;
            OptPurge.Checked = false;
            TextKey.Text = containerKey;
            TextValueName.Text = "";
            return ShowDialog();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (OptClearFirst.Checked || OptPurge.Checked)
            {
                DialogResult = DialogResult.OK;
            }

            if (!OptDeleteOne.Checked)
            {
                return;
            }

            if (string.IsNullOrEmpty(TextValueName.Text))
            {
                Interaction.MsgBox("You must enter a value name.", MsgBoxStyle.Exclamation);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void EditPolDelete_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        public void ChoiceChanged(object sender, EventArgs e) => TextValueName.Enabled = OptDeleteOne.Checked;
    }
}