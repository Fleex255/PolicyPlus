using System;
using System.IO;
using System.Windows.Forms;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class LoadedAdmx
    {
        private AdmxBundle _bundle;

        public LoadedAdmx()
        {
            InitializeComponent();
        }

        public void PresentDialog(AdmxBundle workspace)
        {
            LsvAdmx.Items.Clear();
            foreach (var admx in workspace.Sources.Keys)
            {
                var lsvi = LsvAdmx.Items.Add(Path.GetFileName(admx.SourceFile));
                _ = lsvi.SubItems.Add(Path.GetDirectoryName(admx.SourceFile));
                _ = lsvi.SubItems.Add(admx.AdmxNamespace);
                lsvi.Tag = admx;
            }
            LoadedAdmx_SizeChanged(null, null);
            ChNamespace.Width -= SystemInformation.VerticalScrollBarWidth; // For some reason, this only needs to be taken into account on the first draw
            _bundle = workspace;
            _ = ShowDialog(this);
        }

        private void LsvAdmx_DoubleClick(object sender, EventArgs e) => new DetailAdmx().PresentDialog((AdmxFile)LsvAdmx.SelectedItems[0].Tag, _bundle);

        private void LoadedAdmx_SizeChanged(object sender, EventArgs e) => ChNamespace.Width = Math.Max(30, LsvAdmx.ClientRectangle.Width - ChFolder.Width - ChFileTitle.Width);

        private void LsvAdmx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && LsvAdmx.SelectedItems.Count > 0)
            {
                LsvAdmx_DoubleClick(sender, e);
            }
        }
    }
}