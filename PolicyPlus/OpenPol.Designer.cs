using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class OpenPol : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            ComputerGroup = new GroupBox();
            CompNullOption = new RadioButton();
            CompNullOption.CheckedChanged += new EventHandler(CompOptionsCheckedChanged);
            CompFileBrowseButton = new Button();
            CompFileBrowseButton.Click += new EventHandler(CompFileBrowseButton_Click);
            CompPolFilenameTextbox = new TextBox();
            CompFileOption = new RadioButton();
            CompFileOption.CheckedChanged += new EventHandler(CompOptionsCheckedChanged);
            CompRegistryOption = new RadioButton();
            CompRegistryOption.CheckedChanged += new EventHandler(CompOptionsCheckedChanged);
            CompLocalOption = new RadioButton();
            CompLocalOption.CheckedChanged += new EventHandler(CompOptionsCheckedChanged);
            UserGroup = new GroupBox();
            UserPerUserRegOption = new RadioButton();
            UserPerUserRegOption.CheckedChanged += new EventHandler(UserOptionsCheckedChanged);
            UserPerUserGpoOption = new RadioButton();
            UserPerUserGpoOption.CheckedChanged += new EventHandler(UserOptionsCheckedChanged);
            UserBrowseHiveButton = new Button();
            UserBrowseHiveButton.Click += new EventHandler(UserBrowseRegistryButton_Click);
            UserNullOption = new RadioButton();
            UserNullOption.CheckedChanged += new EventHandler(UserOptionsCheckedChanged);
            UserBrowseGpoButton = new Button();
            UserBrowseGpoButton.Click += new EventHandler(UserBrowseGpoButton_Click);
            UserHivePathTextbox = new TextBox();
            UserFileBrowseButton = new Button();
            UserFileBrowseButton.Click += new EventHandler(UserFileBrowseButton_Click);
            UserGpoSidTextbox = new TextBox();
            UserPolFilenameTextbox = new TextBox();
            UserFileOption = new RadioButton();
            UserFileOption.CheckedChanged += new EventHandler(UserOptionsCheckedChanged);
            UserRegistryOption = new RadioButton();
            UserRegistryOption.CheckedChanged += new EventHandler(UserOptionsCheckedChanged);
            UserLocalOption = new RadioButton();
            UserLocalOption.CheckedChanged += new EventHandler(UserOptionsCheckedChanged);
            OkButton = new Button();
            OkButton.Click += new EventHandler(OkButton_Click);
            CompRegTextbox = new TextBox();
            UserRegTextbox = new TextBox();
            ComputerGroup.SuspendLayout();
            UserGroup.SuspendLayout();
            SuspendLayout();
            // 
            // ComputerGroup
            // 
            ComputerGroup.Controls.Add(CompRegTextbox);
            ComputerGroup.Controls.Add(CompNullOption);
            ComputerGroup.Controls.Add(CompFileBrowseButton);
            ComputerGroup.Controls.Add(CompPolFilenameTextbox);
            ComputerGroup.Controls.Add(CompFileOption);
            ComputerGroup.Controls.Add(CompRegistryOption);
            ComputerGroup.Controls.Add(CompLocalOption);
            ComputerGroup.Location = new Point(12, 12);
            ComputerGroup.Name = "ComputerGroup";
            ComputerGroup.Size = new Size(224, 111);
            ComputerGroup.TabIndex = 0;
            ComputerGroup.TabStop = false;
            ComputerGroup.Text = "Computer";
            // 
            // CompNullOption
            // 
            CompNullOption.AutoSize = true;
            CompNullOption.Location = new Point(6, 88);
            CompNullOption.Name = "CompNullOption";
            CompNullOption.Size = new Size(119, 17);
            CompNullOption.TabIndex = 6;
            CompNullOption.TabStop = true;
            CompNullOption.Text = "Scratch space (null)";
            CompNullOption.UseVisualStyleBackColor = true;
            // 
            // CompFileBrowseButton
            // 
            CompFileBrowseButton.Location = new Point(180, 62);
            CompFileBrowseButton.Name = "CompFileBrowseButton";
            CompFileBrowseButton.Size = new Size(38, 23);
            CompFileBrowseButton.TabIndex = 5;
            CompFileBrowseButton.Text = "...";
            CompFileBrowseButton.UseVisualStyleBackColor = true;
            // 
            // CompPolFilenameTextbox
            // 
            CompPolFilenameTextbox.Location = new Point(74, 64);
            CompPolFilenameTextbox.Name = "CompPolFilenameTextbox";
            CompPolFilenameTextbox.Size = new Size(100, 20);
            CompPolFilenameTextbox.TabIndex = 4;
            // 
            // CompFileOption
            // 
            CompFileOption.AutoSize = true;
            CompFileOption.Location = new Point(6, 65);
            CompFileOption.Name = "CompFileOption";
            CompFileOption.Size = new Size(62, 17);
            CompFileOption.TabIndex = 3;
            CompFileOption.TabStop = true;
            CompFileOption.Text = "POL file";
            CompFileOption.UseVisualStyleBackColor = true;
            // 
            // CompRegistryOption
            // 
            CompRegistryOption.AutoSize = true;
            CompRegistryOption.Location = new Point(6, 42);
            CompRegistryOption.Name = "CompRegistryOption";
            CompRegistryOption.Size = new Size(92, 17);
            CompRegistryOption.TabIndex = 1;
            CompRegistryOption.TabStop = true;
            CompRegistryOption.Text = "Local Registry";
            CompRegistryOption.UseVisualStyleBackColor = true;
            // 
            // CompLocalOption
            // 
            CompLocalOption.AutoSize = true;
            CompLocalOption.Location = new Point(6, 19);
            CompLocalOption.Name = "CompLocalOption";
            CompLocalOption.Size = new Size(77, 17);
            CompLocalOption.TabIndex = 0;
            CompLocalOption.TabStop = true;
            CompLocalOption.Text = "Local GPO";
            CompLocalOption.UseVisualStyleBackColor = true;
            // 
            // UserGroup
            // 
            UserGroup.Controls.Add(UserRegTextbox);
            UserGroup.Controls.Add(UserPerUserRegOption);
            UserGroup.Controls.Add(UserPerUserGpoOption);
            UserGroup.Controls.Add(UserBrowseHiveButton);
            UserGroup.Controls.Add(UserNullOption);
            UserGroup.Controls.Add(UserBrowseGpoButton);
            UserGroup.Controls.Add(UserHivePathTextbox);
            UserGroup.Controls.Add(UserFileBrowseButton);
            UserGroup.Controls.Add(UserGpoSidTextbox);
            UserGroup.Controls.Add(UserPolFilenameTextbox);
            UserGroup.Controls.Add(UserFileOption);
            UserGroup.Controls.Add(UserRegistryOption);
            UserGroup.Controls.Add(UserLocalOption);
            UserGroup.Location = new Point(242, 12);
            UserGroup.Name = "UserGroup";
            UserGroup.Size = new Size(224, 157);
            UserGroup.TabIndex = 1;
            UserGroup.TabStop = false;
            UserGroup.Text = "User";
            // 
            // UserPerUserRegOption
            // 
            UserPerUserRegOption.AutoSize = true;
            UserPerUserRegOption.Location = new Point(6, 111);
            UserPerUserRegOption.Name = "UserPerUserRegOption";
            UserPerUserRegOption.Size = new Size(70, 17);
            UserPerUserRegOption.TabIndex = 9;
            UserPerUserRegOption.TabStop = true;
            UserPerUserRegOption.Text = "User hive";
            UserPerUserRegOption.UseVisualStyleBackColor = true;
            // 
            // UserPerUserGpoOption
            // 
            UserPerUserGpoOption.AutoSize = true;
            UserPerUserGpoOption.Location = new Point(6, 88);
            UserPerUserGpoOption.Name = "UserPerUserGpoOption";
            UserPerUserGpoOption.Size = new Size(73, 17);
            UserPerUserGpoOption.TabIndex = 6;
            UserPerUserGpoOption.TabStop = true;
            UserPerUserGpoOption.Text = "User GPO";
            UserPerUserGpoOption.UseVisualStyleBackColor = true;
            // 
            // UserBrowseHiveButton
            // 
            UserBrowseHiveButton.Location = new Point(180, 108);
            UserBrowseHiveButton.Name = "UserBrowseHiveButton";
            UserBrowseHiveButton.Size = new Size(38, 23);
            UserBrowseHiveButton.TabIndex = 11;
            UserBrowseHiveButton.Text = "...";
            UserBrowseHiveButton.UseVisualStyleBackColor = true;
            // 
            // UserNullOption
            // 
            UserNullOption.AutoSize = true;
            UserNullOption.Location = new Point(6, 134);
            UserNullOption.Name = "UserNullOption";
            UserNullOption.Size = new Size(119, 17);
            UserNullOption.TabIndex = 12;
            UserNullOption.TabStop = true;
            UserNullOption.Text = "Scratch space (null)";
            UserNullOption.UseVisualStyleBackColor = true;
            // 
            // UserBrowseGpoButton
            // 
            UserBrowseGpoButton.Location = new Point(180, 85);
            UserBrowseGpoButton.Name = "UserBrowseGpoButton";
            UserBrowseGpoButton.Size = new Size(38, 23);
            UserBrowseGpoButton.TabIndex = 8;
            UserBrowseGpoButton.Text = "...";
            UserBrowseGpoButton.UseVisualStyleBackColor = true;
            // 
            // UserHivePathTextbox
            // 
            UserHivePathTextbox.Location = new Point(85, 110);
            UserHivePathTextbox.Name = "UserHivePathTextbox";
            UserHivePathTextbox.Size = new Size(89, 20);
            UserHivePathTextbox.TabIndex = 10;
            // 
            // UserFileBrowseButton
            // 
            UserFileBrowseButton.Location = new Point(180, 62);
            UserFileBrowseButton.Name = "UserFileBrowseButton";
            UserFileBrowseButton.Size = new Size(38, 23);
            UserFileBrowseButton.TabIndex = 5;
            UserFileBrowseButton.Text = "...";
            UserFileBrowseButton.UseVisualStyleBackColor = true;
            // 
            // UserGpoSidTextbox
            // 
            UserGpoSidTextbox.Location = new Point(85, 87);
            UserGpoSidTextbox.Name = "UserGpoSidTextbox";
            UserGpoSidTextbox.Size = new Size(89, 20);
            UserGpoSidTextbox.TabIndex = 7;
            // 
            // UserPolFilenameTextbox
            // 
            UserPolFilenameTextbox.Location = new Point(74, 64);
            UserPolFilenameTextbox.Name = "UserPolFilenameTextbox";
            UserPolFilenameTextbox.Size = new Size(100, 20);
            UserPolFilenameTextbox.TabIndex = 4;
            // 
            // UserFileOption
            // 
            UserFileOption.AutoSize = true;
            UserFileOption.Location = new Point(6, 65);
            UserFileOption.Name = "UserFileOption";
            UserFileOption.Size = new Size(62, 17);
            UserFileOption.TabIndex = 3;
            UserFileOption.TabStop = true;
            UserFileOption.Text = "POL file";
            UserFileOption.UseVisualStyleBackColor = true;
            // 
            // UserRegistryOption
            // 
            UserRegistryOption.AutoSize = true;
            UserRegistryOption.Location = new Point(6, 42);
            UserRegistryOption.Name = "UserRegistryOption";
            UserRegistryOption.Size = new Size(92, 17);
            UserRegistryOption.TabIndex = 1;
            UserRegistryOption.TabStop = true;
            UserRegistryOption.Text = "Local Registry";
            UserRegistryOption.UseVisualStyleBackColor = true;
            // 
            // UserLocalOption
            // 
            UserLocalOption.AutoSize = true;
            UserLocalOption.Location = new Point(6, 19);
            UserLocalOption.Name = "UserLocalOption";
            UserLocalOption.Size = new Size(77, 17);
            UserLocalOption.TabIndex = 0;
            UserLocalOption.TabStop = true;
            UserLocalOption.Text = "Local GPO";
            UserLocalOption.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            OkButton.Location = new Point(391, 175);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 18;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            // 
            // CompRegTextbox
            // 
            CompRegTextbox.Location = new Point(104, 41);
            CompRegTextbox.Name = "CompRegTextbox";
            CompRegTextbox.Size = new Size(114, 20);
            CompRegTextbox.TabIndex = 2;
            CompRegTextbox.Text = "HKLM";
            // 
            // UserRegTextbox
            // 
            UserRegTextbox.Location = new Point(104, 41);
            UserRegTextbox.Name = "UserRegTextbox";
            UserRegTextbox.Size = new Size(114, 20);
            UserRegTextbox.TabIndex = 2;
            UserRegTextbox.Text = "HKCU";
            // 
            // OpenPol
            // 
            AcceptButton = OkButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 210);
            Controls.Add(OkButton);
            Controls.Add(UserGroup);
            Controls.Add(ComputerGroup);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OpenPol";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Open Policy Resources";
            ComputerGroup.ResumeLayout(false);
            ComputerGroup.PerformLayout();
            UserGroup.ResumeLayout(false);
            UserGroup.PerformLayout();
            KeyDown += new KeyEventHandler(OpenPol_KeyDown);
            ResumeLayout(false);

        }

        internal GroupBox ComputerGroup;
        internal RadioButton CompNullOption;
        internal Button CompFileBrowseButton;
        internal TextBox CompPolFilenameTextbox;
        internal RadioButton CompFileOption;
        internal RadioButton CompRegistryOption;
        internal RadioButton CompLocalOption;
        internal GroupBox UserGroup;
        internal RadioButton UserPerUserRegOption;
        internal RadioButton UserPerUserGpoOption;
        internal Button UserBrowseHiveButton;
        internal RadioButton UserNullOption;
        internal Button UserBrowseGpoButton;
        internal TextBox UserHivePathTextbox;
        internal Button UserFileBrowseButton;
        internal TextBox UserGpoSidTextbox;
        internal TextBox UserPolFilenameTextbox;
        internal RadioButton UserFileOption;
        internal RadioButton UserRegistryOption;
        internal RadioButton UserLocalOption;
        internal Button OkButton;
        internal TextBox CompRegTextbox;
        internal TextBox UserRegTextbox;
    }
}