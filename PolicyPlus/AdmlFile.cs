using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualBasic.CompilerServices;

namespace PolicyPlus
{
    public class AdmlFile
    {
        public string SourceFile;
        public decimal Revision;
        public string DisplayName;
        public string Description;
        public Dictionary<string, string> StringTable = new Dictionary<string, string>();
        public Dictionary<string, Presentation> PresentationTable = new Dictionary<string, Presentation>();
        private AdmlFile()
        {
        }
        public static AdmlFile Load(string File)
        {
            // ADML documentation: https://technet.microsoft.com/en-us/library/cc772050(v=ws.10).aspx
            var adml = new AdmlFile();
            adml.SourceFile = File;
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(File);
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
                        continue;
                    string key = stringElement.Attributes["id"].Value;
                    string value = stringElement.InnerText;
                    adml.StringTable.Add(key, value);
                }
            }
            // Load presentations (UI arrangements)
            var presTableList = xmlDoc.GetElementsByTagName("presentationTable");
            if (presTableList.Count > 0)
            {
                var presTable = presTableList[0];
                foreach (XmlNode presElement in presTable.ChildNodes)
                {
                    if (presElement.LocalName != "presentation")
                        continue;
                    var presentation = new Presentation();
                    presentation.Name = presElement.Attributes["id"].Value;
                    foreach (XmlNode uiElement in presElement.ChildNodes)
                    {
                        PresentationElement presPart = null;
                        switch (uiElement.LocalName ?? "")
                        {
                            case "text":
                                {
                                    var textPart = new LabelPresentationElement();
                                    textPart.Text = uiElement.InnerText;
                                    presPart = textPart;
                                    break;
                                }
                            case "decimalTextBox":
                                {
                                    var decTextPart = new NumericBoxPresentationElement();
                                    decTextPart.DefaultValue = Conversions.ToUInteger(uiElement.AttributeOrDefault("defaultValue", 1));
                                    decTextPart.HasSpinner = Conversions.ToBoolean(uiElement.AttributeOrDefault("spin", true));
                                    decTextPart.SpinnerIncrement = Conversions.ToUInteger(uiElement.AttributeOrDefault("spinStep", 1));
                                    decTextPart.Label = uiElement.InnerText;
                                    presPart = decTextPart;
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
                                    var checkPart = new CheckBoxPresentationElement();
                                    checkPart.DefaultState = Conversions.ToBoolean(uiElement.AttributeOrDefault("defaultChecked", false));
                                    checkPart.Text = uiElement.InnerText;
                                    presPart = checkPart;
                                    break;
                                }
                            case "comboBox":
                                {
                                    var comboPart = new ComboBoxPresentationElement();
                                    comboPart.NoSort = Conversions.ToBoolean(uiElement.AttributeOrDefault("noSort", false));
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
                                    var dropPart = new DropDownPresentationElement();
                                    dropPart.NoSort = Conversions.ToBoolean(uiElement.AttributeOrDefault("noSort", false));
                                    dropPart.DefaultItemID = Conversions.ToInteger(uiElement.AttributeOrNull("defaultItem"));
                                    dropPart.Label = uiElement.InnerText;
                                    presPart = dropPart;
                                    break;
                                }
                            case "listBox":
                                {
                                    var listPart = new ListPresentationElement();
                                    listPart.Label = uiElement.InnerText;
                                    presPart = listPart;
                                    break;
                                }
                            case "multiTextBox":
                                {
                                    var multiTextPart = new MultiTextPresentationElement();
                                    multiTextPart.Label = uiElement.InnerText;
                                    presPart = multiTextPart;
                                    break;
                                }
                        }
                        if (presPart is not null)
                        {
                            if (uiElement.Attributes["refId"] is not null)
                                presPart.ID = uiElement.Attributes["refId"].Value;
                            presPart.ElementType = uiElement.LocalName;
                            presentation.Elements.Add(presPart);
                        }
                    }
                    adml.PresentationTable.Add(presentation.Name, presentation);
                }
            }
            return adml;
        }
    }
}