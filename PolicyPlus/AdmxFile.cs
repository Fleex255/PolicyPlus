using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualBasic.CompilerServices;

namespace PolicyPlus
{
    public class AdmxFile
    {
        public string SourceFile;
        public string AdmxNamespace;
        public string SupersededAdm;
        public decimal MinAdmlVersion;
        public Dictionary<string, string> Prefixes = new Dictionary<string, string>();
        public List<AdmxProduct> Products = new List<AdmxProduct>();
        public List<AdmxSupportDefinition> SupportedOnDefinitions = new List<AdmxSupportDefinition>();
        public List<AdmxCategory> Categories = new List<AdmxCategory>();
        public List<AdmxPolicy> Policies = new List<AdmxPolicy>();
        private AdmxFile()
        {
        }
        public static AdmxFile Load(string File)
        {
            // ADMX documentation: https://technet.microsoft.com/en-us/library/cc772138(v=ws.10).aspx
            var admx = new AdmxFile();
            admx.SourceFile = File;
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(File);
            var policyDefinitions = xmlDoc.GetElementsByTagName("policyDefinitions")[0];
            foreach (XmlNode child in policyDefinitions.ChildNodes)
            {
                switch (child.LocalName ?? "")
                {
                    case "policyNamespaces": // Referenced namespaces and current namespace
                        {
                            foreach (XmlNode policyNamespace in child.ChildNodes)
                            {
                                string prefix = policyNamespace.Attributes["prefix"].Value;
                                string fqNamespace = policyNamespace.Attributes["namespace"].Value;
                                if (policyNamespace.LocalName == "target")
                                    admx.AdmxNamespace = fqNamespace;
                                admx.Prefixes.Add(prefix, fqNamespace);
                            }

                            break;
                        }
                    case "supersededAdm": // The ADM file that this ADMX supersedes
                        {
                            admx.SupersededAdm = child.Attributes["fileName"].Value;
                            break;
                        }
                    case "resources": // Minimum required version
                        {
                            admx.MinAdmlVersion = decimal.Parse(child.Attributes["minRequiredRevision"].Value, System.Globalization.CultureInfo.InvariantCulture);
                            break;
                        }
                    case "supportedOn": // Support definitions
                        {
                            foreach (XmlNode supportInfo in child.ChildNodes)
                            {
                                if (supportInfo.LocalName == "definitions")
                                {
                                    foreach (XmlNode supportDef in supportInfo.ChildNodes)
                                    {
                                        if (supportDef.LocalName != "definition")
                                            continue;
                                        var definition = new AdmxSupportDefinition();
                                        definition.ID = supportDef.Attributes["name"].Value;
                                        definition.DisplayCode = supportDef.Attributes["displayName"].Value;
                                        definition.Logic = AdmxSupportLogicType.Blank;
                                        foreach (XmlNode logicElement in supportDef.ChildNodes)
                                        {
                                            bool canLoad = true;
                                            if (logicElement.LocalName == "or")
                                            {
                                                definition.Logic = AdmxSupportLogicType.AnyOf;
                                            }
                                            else if (logicElement.LocalName == "and")
                                            {
                                                definition.Logic = AdmxSupportLogicType.AllOf;
                                            }
                                            else
                                            {
                                                canLoad = false;
                                            }
                                            if (canLoad)
                                            {
                                                definition.Entries = new List<AdmxSupportEntry>();
                                                foreach (XmlNode conditionElement in logicElement.ChildNodes)
                                                {
                                                    if (conditionElement.LocalName == "reference")
                                                    {
                                                        string product = conditionElement.Attributes["ref"].Value;
                                                        definition.Entries.Add(new AdmxSupportEntry() { ProductID = product, IsRange = false });
                                                    }
                                                    else if (conditionElement.LocalName == "range")
                                                    {
                                                        var entry = new AdmxSupportEntry() { IsRange = true };
                                                        entry.ProductID = conditionElement.Attributes["ref"].Value;
                                                        var maxVerAttr = conditionElement.Attributes["maxVersionIndex"];
                                                        if (maxVerAttr is not null)
                                                            entry.MaxVersion = int.Parse(maxVerAttr.Value);
                                                        var minVerAttr = conditionElement.Attributes["minVersionIndex"];
                                                        if (minVerAttr is not null)
                                                            entry.MinVersion = int.Parse(minVerAttr.Value);
                                                        definition.Entries.Add(entry);
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                        definition.DefinedIn = admx;
                                        admx.SupportedOnDefinitions.Add(definition);
                                    }
                                }
                                else if (supportInfo.LocalName == "products") // Product definitions
                                {
                                    Action<XmlNode, string, AdmxProduct> loadProducts;
                                    loadProducts = new Action<XmlNode, string, AdmxProduct>((Node, ChildTagName, Parent) => { foreach (XmlNode subproductElement in Node.ChildNodes) { if ((subproductElement.LocalName ?? "") != (ChildTagName ?? "")) continue; var product = new AdmxProduct(); product.ID = subproductElement.Attributes["name"].Value; product.DisplayCode = subproductElement.Attributes["displayName"].Value; if (Parent is not null) product.Version = Conversions.ToInteger(subproductElement.Attributes["versionIndex"].Value); product.Parent = Parent; product.DefinedIn = admx; admx.Products.Add(product); if (Parent is null) { product.Type = AdmxProductType.Product; loadProducts(subproductElement, "majorVersion", product); } else if (Parent.Parent is null) { product.Type = AdmxProductType.MajorRevision; loadProducts(subproductElement, "minorVersion", product); } else { product.Type = AdmxProductType.MinorRevision; } } });
                                    loadProducts(supportInfo, "product", null); // Start the recursive load
                                }
                            }

                            break;
                        }
                    case "categories": // Categories
                        {
                            foreach (XmlNode categoryElement in child.ChildNodes)
                            {
                                if (categoryElement.LocalName != "category")
                                    continue;
                                var category = new AdmxCategory();
                                category.ID = categoryElement.Attributes["name"].Value;
                                category.DisplayCode = categoryElement.Attributes["displayName"].Value;
                                category.ExplainCode = categoryElement.AttributeOrNull("explainText");
                                if (categoryElement.HasChildNodes)
                                {
                                    var parentCatElement = categoryElement["parentCategory"];
                                    category.ParentID = parentCatElement.Attributes["ref"].Value;
                                }
                                category.DefinedIn = admx;
                                admx.Categories.Add(category);
                            }

                            break;
                        }
                    case "policies": // Policy settings
                        {
                            PolicyRegistryValue loadRegItem(XmlNode Node)
                            {
                                var regItem = new PolicyRegistryValue();
                                foreach (XmlNode subElement in Node.ChildNodes)
                                {
                                    if (subElement.LocalName == "delete")
                                    {
                                        regItem.RegistryType = PolicyRegistryValueType.Delete;
                                        break;
                                    }
                                    else if (subElement.LocalName == "decimal")
                                    {
                                        regItem.RegistryType = PolicyRegistryValueType.Numeric;
                                        regItem.NumberValue = Conversions.ToUInteger(subElement.Attributes["value"].Value);
                                        break;
                                    }
                                    else if (subElement.LocalName == "string")
                                    {
                                        regItem.RegistryType = PolicyRegistryValueType.Text;
                                        regItem.StringValue = subElement.InnerText;
                                        break;
                                    }
                                }
                                return regItem;
                            };
                            PolicyRegistrySingleList loadOneRegList(XmlNode Node)
                            {
                                var singleList = new PolicyRegistrySingleList();
                                singleList.DefaultRegistryKey = Node.AttributeOrNull("defaultKey");
                                singleList.AffectedValues = new List<PolicyRegistryListEntry>();
                                foreach (XmlNode itemElement in Node.ChildNodes)
                                {
                                    if (itemElement.LocalName != "item")
                                        continue;
                                    var listEntry = new PolicyRegistryListEntry();
                                    listEntry.RegistryValue = itemElement.Attributes["valueName"].Value;
                                    listEntry.RegistryKey = itemElement.AttributeOrNull("key");
                                    foreach (XmlNode valElement in itemElement.ChildNodes)
                                    {
                                        if (valElement.LocalName == "value")
                                        {
                                            listEntry.Value = loadRegItem(valElement);
                                            break;
                                        }
                                    }
                                    singleList.AffectedValues.Add(listEntry);
                                }
                                return singleList;
                            };
                            PolicyRegistryList loadOnOffValList(string OnValueName, string OffValueName, string OnListName, string OffListName, XmlNode Node)
                            {
                                var regList = new PolicyRegistryList();
                                foreach (XmlNode subElement in Node.ChildNodes)
                                {
                                    if ((subElement.Name ?? "") == (OnValueName ?? ""))
                                    {
                                        regList.OnValue = loadRegItem(subElement);
                                    }
                                    else if ((subElement.Name ?? "") == (OffValueName ?? ""))
                                    {
                                        regList.OffValue = loadRegItem(subElement);
                                    }
                                    else if ((subElement.Name ?? "") == (OnListName ?? ""))
                                    {
                                        regList.OnValueList = loadOneRegList(subElement);
                                    }
                                    else if ((subElement.Name ?? "") == (OffListName ?? ""))
                                    {
                                        regList.OffValueList = loadOneRegList(subElement);
                                    }
                                }
                                return regList;
                            };
                            foreach (XmlNode polElement in child.ChildNodes)
                            {
                                if (polElement.LocalName != "policy")
                                    continue;
                                var policy = new AdmxPolicy();
                                policy.ID = polElement.Attributes["name"].Value;
                                policy.DefinedIn = admx;
                                policy.DisplayCode = polElement.Attributes["displayName"].Value;
                                policy.RegistryKey = polElement.Attributes["key"].Value;
                                string polClass = polElement.Attributes["class"].Value;
                                switch (polClass ?? "")
                                {
                                    case "Machine":
                                        {
                                            policy.Section = AdmxPolicySection.Machine;
                                            break;
                                        }
                                    case "User":
                                        {
                                            policy.Section = AdmxPolicySection.User;
                                            break;
                                        }

                                    default:
                                        {
                                            policy.Section = AdmxPolicySection.Both;
                                            break;
                                        }
                                }
                                policy.ExplainCode = polElement.AttributeOrNull("explainText");
                                policy.PresentationID = polElement.AttributeOrNull("presentation");
                                policy.ClientExtension = polElement.AttributeOrNull("clientExtension");
                                policy.RegistryValue = polElement.AttributeOrNull("valueName");
                                policy.AffectedValues = loadOnOffValList("enabledValue", "disabledValue", "enabledList", "disabledList", polElement);
                                foreach (XmlNode polInfo in polElement.ChildNodes)
                                {
                                    switch (polInfo.LocalName ?? "")
                                    {
                                        case "parentCategory":
                                            {
                                                policy.CategoryID = polInfo.Attributes["ref"].Value;
                                                break;
                                            }
                                        case "supportedOn":
                                            {
                                                policy.SupportedCode = polInfo.Attributes["ref"].Value;
                                                break;
                                            }
                                        case "elements":
                                            {
                                                policy.Elements = new List<PolicyElement>();
                                                foreach (XmlNode uiElement in polInfo.ChildNodes)
                                                {
                                                    PolicyElement entry = null;
                                                    switch (uiElement.LocalName ?? "")
                                                    {
                                                        case "decimal":
                                                            {
                                                                var decimalEntry = new DecimalPolicyElement();
                                                                decimalEntry.Minimum = Conversions.ToUInteger(uiElement.AttributeOrDefault("minValue", 0));
                                                                decimalEntry.Maximum = Conversions.ToUInteger(uiElement.AttributeOrDefault("maxValue", uint.MaxValue));
                                                                decimalEntry.NoOverwrite = Conversions.ToBoolean(uiElement.AttributeOrDefault("soft", false));
                                                                decimalEntry.StoreAsText = Conversions.ToBoolean(uiElement.AttributeOrDefault("storeAsText", false));
                                                                entry = decimalEntry;
                                                                break;
                                                            }
                                                        case "boolean":
                                                            {
                                                                var boolEntry = new BooleanPolicyElement();
                                                                boolEntry.AffectedRegistry = loadOnOffValList("trueValue", "falseValue", "trueList", "falseList", uiElement);
                                                                entry = boolEntry;
                                                                break;
                                                            }
                                                        case "text":
                                                            {
                                                                var textEntry = new TextPolicyElement();
                                                                textEntry.MaxLength = Conversions.ToInteger(uiElement.AttributeOrDefault("maxLength", 255));
                                                                textEntry.Required = Conversions.ToBoolean(uiElement.AttributeOrDefault("required", false));
                                                                textEntry.RegExpandSz = Conversions.ToBoolean(uiElement.AttributeOrDefault("expandable", false));
                                                                textEntry.NoOverwrite = Conversions.ToBoolean(uiElement.AttributeOrDefault("soft", false));
                                                                entry = textEntry;
                                                                break;
                                                            }
                                                        case "list":
                                                            {
                                                                var listEntry = new ListPolicyElement();
                                                                listEntry.NoPurgeOthers = Conversions.ToBoolean(uiElement.AttributeOrDefault("additive", false));
                                                                listEntry.RegExpandSz = Conversions.ToBoolean(uiElement.AttributeOrDefault("expandable", false));
                                                                listEntry.UserProvidesNames = Conversions.ToBoolean(uiElement.AttributeOrDefault("explicitValue", false));
                                                                listEntry.HasPrefix = uiElement.Attributes["valuePrefix"] is not null;
                                                                listEntry.RegistryValue = uiElement.AttributeOrNull("valuePrefix");
                                                                entry = listEntry;
                                                                break;
                                                            }
                                                        case "enum":
                                                            {
                                                                var enumEntry = new EnumPolicyElement();
                                                                enumEntry.Required = Conversions.ToBoolean(uiElement.AttributeOrDefault("required", false));
                                                                enumEntry.Items = new List<EnumPolicyElementItem>();
                                                                foreach (XmlNode itemElement in uiElement.ChildNodes)
                                                                {
                                                                    if (itemElement.LocalName == "item")
                                                                    {
                                                                        var enumItem = new EnumPolicyElementItem();
                                                                        enumItem.DisplayCode = itemElement.Attributes["displayName"].Value;
                                                                        foreach (XmlNode valElement in itemElement.ChildNodes)
                                                                        {
                                                                            if (valElement.LocalName == "value")
                                                                            {
                                                                                enumItem.Value = loadRegItem(valElement);
                                                                            }
                                                                            else if (valElement.LocalName == "valueList")
                                                                            {
                                                                                enumItem.ValueList = loadOneRegList(valElement);
                                                                            }
                                                                        }
                                                                        enumEntry.Items.Add(enumItem);
                                                                    }
                                                                }
                                                                entry = enumEntry;
                                                                break;
                                                            }
                                                        case "multiText":
                                                            {
                                                                entry = new MultiTextPolicyElement();
                                                                break;
                                                            }
                                                    }
                                                    if (entry is not null)
                                                    {
                                                        entry.ClientExtension = uiElement.AttributeOrNull("clientExtension");
                                                        entry.RegistryKey = uiElement.AttributeOrNull("key");
                                                        if (string.IsNullOrEmpty(entry.RegistryValue))
                                                            entry.RegistryValue = uiElement.AttributeOrNull("valueName");
                                                        entry.ID = uiElement.Attributes["id"].Value;
                                                        entry.ElementType = uiElement.LocalName;
                                                        policy.Elements.Add(entry);
                                                    }
                                                }

                                                break;
                                            }
                                    }
                                }
                                admx.Policies.Add(policy);
                            }

                            break;
                        }
                }
            }
            return admx;
        }
    }
}