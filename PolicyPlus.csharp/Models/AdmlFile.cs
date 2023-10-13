using System;
using System.Collections.Generic;
using System.Xml;
using PolicyPlus.csharp.Helpers;
using PolicyPlus.csharp.UI.Elements;

namespace PolicyPlus.csharp.Models
{
    public sealed class AdmlFile
    {
        public string SourceFile { get; set; }
        public decimal Revision { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> StringTable { get; } = new();
        public Dictionary<string, Presentation> PresentationTable { get; } = new();

        private AdmlFile()
        {
        }

        public static AdmlFile Load(string file)
        {
            // ADML documentation: https://technet.microsoft.com/en-us/library/cc772050(v=ws.10).aspx
            var adml = new AdmlFile
            {
                SourceFile = file
            };
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            // Load ADML metadata
            var policyDefinitionResources = xmlDoc.GetElementsByTagName("policyDefinitionResources")[0];
            adml.Revision = decimal.Parse(policyDefinitionResources.Attributes["revision"].Value, System.Globalization.CultureInfo.InvariantCulture);
            foreach (XmlNode child in policyDefinitionResources.ChildNodes)
            {
                switch (child.LocalName ?? "")
                {
                    case "displayName":
                        {
                            adml.DisplayName = child.InnerText;
                            break;
                        }
                    case "description":
                        {
                            adml.Description = child.InnerText;
                            break;
                        }
                }
            }
            // Load localized strings
            var stringTableList = xmlDoc.GetElementsByTagName("stringTable");
            if (stringTableList.Count > 0)
            {
                var stringTable = stringTableList[0];
                foreach (XmlNode stringElement in stringTable.ChildNodes)
                {
                    if (stringElement.LocalName != "string")
                    {
                        continue;
                    }

                    var key = stringElement.Attributes["id"].Value;
                    var value = stringElement.InnerText;
                    adml.StringTable.Add(key, value);
                }
            }
            // Load presentations (UI arrangements)
            var presTableList = xmlDoc.GetElementsByTagName("presentationTable");
            if (presTableList.Count <= 0)
            {
                return adml;
            }

            var presTable = presTableList[0];
            foreach (XmlNode presElement in presTable.ChildNodes)
            {
                if (presElement.LocalName != "presentation")
                {
                    continue;
                }

                var presentation = new Presentation
                {
                    Name = presElement.Attributes["id"].Value
                };
                foreach (XmlNode uiElement in presElement.ChildNodes)
                {
                    PresentationElement? presPart = null;
                    switch (uiElement.LocalName ?? "")
                    {
                        case "text":
                            {
                                presPart = new LabelPresentationElement
                                {
                                    Text = uiElement.InnerText
                                };
                                break;
                            }
                        case "decimalTextBox":
                            {
                                presPart = new NumericBoxPresentationElement
                                {
                                    DefaultValue = Convert.ToUInt32(uiElement.AttributeOrDefault("defaultValue", 1)),
                                    HasSpinner = (bool)uiElement.AttributeOrDefault("spin", true),
                                    SpinnerIncrement = Convert.ToUInt32(uiElement.AttributeOrDefault("spinStep", 1)),
                                    Label = uiElement.InnerText
                                };
                                break;
                            }
                        case "textBox":
                            {
                                var textPart = new TextBoxPresentationElement();
                                foreach (XmlNode textboxInfo in uiElement.ChildNodes)
                                {
                                    switch (textboxInfo.LocalName ?? "")
                                    {
                                        case "label":
                                            {
                                                textPart.Label = textboxInfo.InnerText;
                                                break;
                                            }
                                        case "defaultValue":
                                            {
                                                textPart.DefaultValue = textboxInfo.InnerText;
                                                break;
                                            }
                                    }
                                }
                                presPart = textPart;
                                break;
                            }
                        case "checkBox":
                            {
                                presPart = new CheckBoxPresentationElement
                                {
                                    DefaultState = (bool)uiElement.AttributeOrDefault("defaultChecked", false),
                                    Text = uiElement.InnerText
                                };
                                break;
                            }
                        case "comboBox":
                            {
                                var comboPart = new ComboBoxPresentationElement
                                {
                                    NoSort = (bool)uiElement.AttributeOrDefault("noSort", false)
                                };
                                foreach (XmlNode comboInfo in uiElement.ChildNodes)
                                {
                                    switch (comboInfo.LocalName ?? "")
                                    {
                                        case "label":
                                            {
                                                comboPart.Label = comboInfo.InnerText;
                                                break;
                                            }
                                        case "default":
                                            {
                                                comboPart.DefaultText = comboInfo.InnerText;
                                                break;
                                            }
                                        case "suggestion":
                                            {
                                                comboPart.Suggestions.Add(comboInfo.InnerText);
                                                break;
                                            }
                                    }
                                }
                                presPart = comboPart;
                                break;
                            }
                        case "dropdownList":
                            {
                                presPart = new DropDownPresentationElement
                                {
                                    NoSort = (bool)uiElement.AttributeOrDefault("noSort", false),
                                    DefaultItemId = uiElement.AttributeOrNull("defaultItem") != null ? int.Parse(uiElement.AttributeOrNull("defaultItem")) : null,
                                    Label = uiElement.InnerText
                                };
                                break;
                            }
                        case "listBox":
                            {
                                presPart = new ListPresentationElement
                                {
                                    Label = uiElement.InnerText
                                };
                                break;
                            }
                        case "multiTextBox":
                            {
                                presPart = new MultiTextPresentationElement
                                {
                                    Label = uiElement.InnerText
                                };
                                break;
                            }
                    }

                    if (presPart is null)
                    {
                        continue;
                    }

                    if (uiElement.Attributes["refId"] is not null)
                    {
                        presPart.Id = uiElement.Attributes["refId"].Value;
                    }

                    presPart.ElementType = uiElement.LocalName;
                    presentation.Elements.Add(presPart);
                }
                adml.PresentationTable.Add(presentation.Name, presentation);
            }
            return adml;
        }
    }
}