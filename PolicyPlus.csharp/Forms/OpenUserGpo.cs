﻿using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace PolicyPlus.csharp.UI
{
    public partial class OpenUserGpo
    {
        public string SelectedSid;

        public OpenUserGpo()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object? sender, EventArgs? e)
        {
            // Resolve the username to a security identifier (SID)
            try
            {
                var userAccount = new NTAccount(UsernameTextbox.Text);
                var sid = (SecurityIdentifier)userAccount.Translate(typeof(SecurityIdentifier));
                SidTextbox.Text = sid.ToString();
            }
            catch (Exception)
            {
                _ = MessageBox.Show("The name could not be translated to a SID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SidTextbox.Text) && !string.IsNullOrEmpty(UsernameTextbox.Text))
            {
                // Automatically resolve if the user didn't click Search
                SearchButton_Click(null, null);
                if (string.IsNullOrEmpty(SidTextbox.Text))
                {
                    return;
                }
            }
            try
            {
                // Make sure the SID is valid
                var sid = new SecurityIdentifier(SidTextbox.Text);
            }
            catch (Exception)
            {
                _ = MessageBox.Show("The SID is not valid. Enter a SID in the lower box, or enter a username in the top box and press Search to translate.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }
            SelectedSid = SidTextbox.Text;
            DialogResult = DialogResult.OK;
        }

        private void OpenUserGpo_Shown(object sender, EventArgs e)
        {
            SidTextbox.Text = string.Empty;
            UsernameTextbox.Text = string.Empty;
        }

        private void OpenUserGpo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}