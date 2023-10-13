﻿using System;
using System.Windows.Forms;
using PolicyPlus.Models;
using MyProject = PolicyPlus.My.MyProject;

namespace PolicyPlus.UI
{
    public partial class DetailSupport
    {
        public DetailSupport()
        {
            InitializeComponent();
        }

        public void PresentDialog(PolicyPlusSupport supported)
        {
            PrepareDialog(supported);
            ShowDialog();
        }

        public void PrepareDialog(PolicyPlusSupport supported)
        {
            NameTextbox.Text = supported.DisplayName;
            IdTextbox.Text = supported.UniqueId;
            DefinedTextbox.Text = supported.RawSupport.DefinedIn.SourceFile;
            DisplayCodeTextbox.Text = supported.RawSupport.DisplayCode;
            switch (supported.RawSupport.Logic)
            {
                case AdmxSupportLogicType.AllOf:
                    {
                        LogicTextbox.Text = "Match all the referenced products";
                        break;
                    }
                case AdmxSupportLogicType.AnyOf:
                    {
                        LogicTextbox.Text = "Match any of the referenced products";
                        break;
                    }
                case AdmxSupportLogicType.Blank:
                    {
                        LogicTextbox.Text = "Do not match products";
                        break;
                    }
            }
            EntriesListview.Items.Clear();
            if (supported.Elements is not null)
            {
                foreach (var element in supported.Elements)
                {
                    ListViewItem lsvi;
                    if (element.SupportDefinition is not null)
                    {
                        lsvi = EntriesListview.Items.Add(element.SupportDefinition.DisplayName);
                    }
                    else if (element.Product is not null)
                    {
                        lsvi = EntriesListview.Items.Add(element.Product.DisplayName);
                        if (element.RawSupportEntry.IsRange)
                        {
                            if (element.RawSupportEntry.MinVersion.HasValue)
                            {
                                lsvi.SubItems.Add(element.RawSupportEntry.MinVersion.Value.ToString());
                            }
                            else
                            {
                                lsvi.SubItems.Add("");
                            }

                            if (element.RawSupportEntry.MaxVersion.HasValue)
                            {
                                lsvi.SubItems.Add(element.RawSupportEntry.MaxVersion.Value.ToString());
                            }
                            else
                            {
                                lsvi.SubItems.Add("");
                            }
                        }
                    }
                    else
                    {
                        lsvi = EntriesListview.Items.Add("<missing: " + element.RawSupportEntry.ProductId + ">");
                    }
                    lsvi.Tag = element;
                }
            }
        }

        private void EntriesListview_ClientSizeChanged(object sender, EventArgs e) => ChName.Width = EntriesListview.ClientSize.Width - ChMinVer.Width - ChMaxVer.Width;

        private void EntriesListview_DoubleClick(object sender, EventArgs e)
        {
            if (EntriesListview.SelectedItems.Count == 0)
            {
                return;
            }

            var supEntry = (PolicyPlusSupportEntry)EntriesListview.SelectedItems[0].Tag;
            if (supEntry.Product is not null)
            {
                MyProject.Forms.DetailProduct.PresentDialog(supEntry.Product);
            }
            else if (supEntry.SupportDefinition is not null)
            {
                PrepareDialog(supEntry.SupportDefinition);
            }
        }
    }
}