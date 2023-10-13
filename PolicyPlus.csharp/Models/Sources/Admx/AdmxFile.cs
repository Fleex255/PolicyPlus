using System;
using System.Collections.Generic;
using System.Xml;
using PolicyPlus.csharp.Elements;
using PolicyPlus.csharp.Helpers;

namespace PolicyPlus.csharp.Models.Sources.Admx
{
    public sealed class AdmxFile
    {
        public string SourceFile;
        public string AdmxNamespace;
        public string SupersededAdm;
        public decimal MinAdmlVersion;
        public Dictionary<string, string> Prefixes = new();
        public List<AdmxProduct> Products = new();
        public List<AdmxSupportDefinition> SupportedOnDefinitions = new();
        public List<AdmxCategory> Categories = new();
        public List<AdmxPolicy> Policies = new();

        private AdmxFile()
        {
        }

        public static AdmxFile Load(string file)
        {
            // ADMX documentation: https://technet.microsoft.com/en-us/library/cc772138(v=ws.10).aspx
            var admx = new AdmxFile
            {
                SourceFile = file
            };
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            var policyDefinitions = xmlDoc.GetElementsByTagName("policyDefinitions")[0];
            foreach (XmlNode child in policyDefinitions.ChildNodes)
            {
                switch (child.LocalName ?? string.Empty)
                {
                    case "policyNamespaces": // Referenced namespaces and current namespace
                        {
                            foreach (XmlNode policyNamespace in child.ChildNodes)
                            {
                                var prefix = policyNamespace.Attributes["prefix"].Value;
                                var fqNamespace = policyNamespace.Attributes["namespace"].Value;
                                if (policyNamespace.LocalName == "target")
                                {
                                    admx.AdmxNamespace = fqNamespace;
                                }

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
                                switch (supportInfo.LocalName)
                                {
                                    case "definitions":
                                        {
                                            foreach (XmlNode supportDef in supportInfo.ChildNodes)
                                            {
                                                if (supportDef.LocalName != "definition")
                                                {
                                                    continue;
                                                }

                                                var definition = new AdmxSupportDefinition
                                                {
                                                    Id = supportDef.Attributes["name"].Value,
                                                    DisplayCode = supportDef.Attributes["displayName"].Value,
                                                    Logic = AdmxSupportLogicType.Blank
                                                };
                                                foreach (XmlNode logicElement in supportDef.ChildNodes)
                                                {
                                                    var canLoad = true;
                                                    switch (logicElement.LocalName)
                                                    {
                                                        case "or":
                                                            definition.Logic = AdmxSupportLogicType.AnyOf;
                                                            break;

                                                        case "and":
                                                            definition.Logic = AdmxSupportLogicType.AllOf;
                                                            break;

                                                        default:
                                                            canLoad = false;
                                                            break;
                                                    }

                                                    if (!canLoad)
                                                    {
                                                        continue;
                                                    }

                                                    definition.Entries = new List<AdmxSupportEntry>();
                                                    foreach (XmlNode conditionElement in logicElement.ChildNodes)
                                                    {
                                                        switch (conditionElement.LocalName)
                                                        {
                                                            case "reference":
                                                                {
                                                                    var product = conditionElement.Attributes["ref"].Value;
                                                                    definition.Entries.Add(new AdmxSupportEntry { ProductId = product, IsRange = false });
                                                                    break;
                                                                }
                                                            case "range":
                                                                {
                                                                    var entry = new AdmxSupportEntry
                                                                    {
                                                                        IsRange = true,
                                                                        ProductId = conditionElement.Attributes["ref"].Value
                                                                    };
                                                                    if (conditionElement.Attributes["maxVersionIndex"] is { } maxVerAttr)
                                                                    {
                                                                        entry.MaxVersion = int.Parse(maxVerAttr.Value);
                                                                    }

                                                                    if (conditionElement.Attributes["minVersionIndex"] is { } minVerAttr)
                                                                    {
                                                                        entry.MinVersion = int.Parse(minVerAttr.Value);
                                                                    }

                                                                    definition.Entries.Add(entry);
                                                                    break;
                                                                }
                                                        }
                                                    }
                                                    break;
                                                }
                                                definition.DefinedIn = admx;
                                                admx.SupportedOnDefinitions.Add(definition);
                                            }

                                            break;
                                        }
                                    // Product definitions
                                    case "products":
                                        LoadProducts(supportInfo, "product", null, admx); // Start the recursive load
                                        break;
                                }
                            }

                            break;
                        }
                    case "categories": // Categories
                        {
                            foreach (XmlNode categoryElement in child.ChildNodes)
                            {
                                if (categoryElement.LocalName != "category")
                                {
                                    continue;
                                }

                                var category = new AdmxCategory
                                {
                                    Id = categoryElement.Attributes["name"].Value,
                                    DisplayCode = categoryElement.Attributes["displayName"].Value,
                                    ExplainCode = categoryElement.AttributeOrNull("explainText")
                                };
                                if (categoryElement.HasChildNodes)
                                {
                                    var parentCatElement = categoryElement["parentCategory"];
                                    category.ParentId = parentCatElement.Attributes["ref"].Value;
                                }
                                category.DefinedIn = admx;
                                admx.Categories.Add(category);
                            }

                            break;
                        }
                    case "policies": // Policy settings
                        {
                            foreach (XmlNode polElement in child.ChildNodes)
                            {
                                if (polElement.LocalName != "policy")
                                {
                                    continue;
                                }

                                var policy = new AdmxPolicy
                                {
                                    Id = polElement.Attributes["name"].Value,
                                    DefinedIn = admx,
                                    DisplayCode = polElement.Attributes["displayName"].Value,
                                    RegistryKey = polElement.Attributes["key"].Value
                                };
                                var polClass = polElement.Attributes["class"].Value;
                                policy.Section = (polClass ?? string.Empty) switch
                                {
                                    "Machine" => AdmxPolicySection.Machine,
                                    "User" => AdmxPolicySection.User,
                                    _ => AdmxPolicySection.Both
                                };
                                policy.ExplainCode = polElement.AttributeOrNull("explainText");
                                policy.PresentationId = polElement.AttributeOrNull("presentation");
                                policy.ClientExtension = polElement.AttributeOrNull("clientExtension");
                                policy.RegistryValue = polElement.AttributeOrNull("valueName");
                                policy.AffectedValues = LoadOnOffValList("enabledValue", "disabledValue", "enabledList", "disabledList", polElement);
                                foreach (XmlNode polInfo in polElement.ChildNodes)
                                {
                                    switch (polInfo.LocalName ?? string.Empty)
                                    {
                                        case "parentCategory":
                                            {
                                                policy.CategoryId = polInfo.Attributes["ref"].Value;
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
                                                    PolicyElement? entry = null;
                                                    switch (uiElement.LocalName ?? string.Empty)
                                                    {
                                                        case "decimal":
                                                            {
                                                                entry = new DecimalPolicyElement
                                                                {
                                                                    Minimum = Convert.ToUInt32(uiElement.AttributeOrDefault("minValue", 0)),
                                                                    Maximum = Convert.ToUInt32(uiElement.AttributeOrDefault("maxValue", uint.MaxValue)),
                                                                    NoOverwrite = (bool)uiElement.AttributeOrDefault("soft", false),
                                                                    StoreAsText = (bool)uiElement.AttributeOrDefault("storeAsText", false)
                                                                };
                                                                break;
                                                            }
                                                        case "boolean":
                                                            {
                                                                entry = new BooleanPolicyElement
                                                                {
                                                                    AffectedRegistry = LoadOnOffValList("trueValue", "falseValue", "trueList", "falseList", uiElement)
                                                                };
                                                                break;
                                                            }
                                                        case "text":
                                                            {
                                                                entry = new TextPolicyElement
                                                                {
                                                                    MaxLength = (int)uiElement.AttributeOrDefault("maxLength", 255),
                                                                    Required = (bool)uiElement.AttributeOrDefault("required", false),
                                                                    RegExpandSz = (bool)uiElement.AttributeOrDefault("expandable", false),
                                                                    NoOverwrite = (bool)uiElement.AttributeOrDefault("soft", false)
                                                                };
                                                                break;
                                                            }
                                                        case "list":
                                                            {
                                                                entry = new ListPolicyElement
                                                                {
                                                                    NoPurgeOthers = (bool)uiElement.AttributeOrDefault("additive", false),
                                                                    RegExpandSz = (bool)uiElement.AttributeOrDefault("expandable", false),
                                                                    UserProvidesNames = (bool)uiElement.AttributeOrDefault("explicitValue", false),
                                                                    HasPrefix = uiElement.Attributes["valuePrefix"] is not null,
                                                                    RegistryValue = uiElement.AttributeOrNull("valuePrefix")
                                                                };
                                                                break;
                                                            }
                                                        case "enum":
                                                            {
                                                                var enumEntry = new EnumPolicyElement
                                                                {
                                                                    Required = (bool)uiElement.AttributeOrDefault("required", false),
                                                                    Items = new List<EnumPolicyElementItem>()
                                                                };
                                                                foreach (XmlNode itemElement in uiElement.ChildNodes)
                                                                {
                                                                    if (itemElement.LocalName != "item")
                                                                    {
                                                                        continue;
                                                                    }

                                                                    var enumItem = new EnumPolicyElementItem
                                                                    {
                                                                        DisplayCode = itemElement.Attributes["displayName"].Value
                                                                    };
                                                                    foreach (XmlNode valElement in itemElement.ChildNodes)
                                                                    {
                                                                        switch (valElement.LocalName)
                                                                        {
                                                                            case "value":
                                                                                enumItem.Value = LoadRegItem(valElement);
                                                                                break;

                                                                            case "valueList":
                                                                                enumItem.ValueList = LoadOneRegList(valElement);
                                                                                break;
                                                                        }
                                                                    }
                                                                    enumEntry.Items.Add(enumItem);
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

                                                    if (entry is null)
                                                    {
                                                        continue;
                                                    }

                                                    entry.ClientExtension = uiElement.AttributeOrNull("clientExtension");
                                                    entry.RegistryKey = uiElement.AttributeOrNull("key");
                                                    if (string.IsNullOrEmpty(entry.RegistryValue))
                                                    {
                                                        entry.RegistryValue = uiElement.AttributeOrNull("valueName");
                                                    }

                                                    entry.Id = uiElement.Attributes["id"].Value;
                                                    entry.ElementType = uiElement.LocalName;
                                                    policy.Elements.Add(entry);
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

        private static PolicyRegistryList LoadOnOffValList(string onValueName, string offValueName, string onListName, string offListName, XmlNode node)
        {
            var regList = new PolicyRegistryList();
            foreach (XmlNode subElement in node.ChildNodes)
            {
                if ((subElement.Name ?? string.Empty) == (onValueName ?? string.Empty))
                {
                    regList.OnValue = LoadRegItem(subElement);
                }
                else if ((subElement.Name ?? string.Empty) == (offValueName ?? string.Empty))
                {
                    regList.OffValue = LoadRegItem(subElement);
                }
                else if ((subElement.Name ?? string.Empty) == (onListName ?? string.Empty))
                {
                    regList.OnValueList = LoadOneRegList(subElement);
                }
                else if ((subElement.Name ?? string.Empty) == (offListName ?? string.Empty))
                {
                    regList.OffValueList = LoadOneRegList(subElement);
                }
            }
            return regList;
        }

        private static PolicyRegistrySingleList LoadOneRegList(XmlNode node)
        {
            var singleList = new PolicyRegistrySingleList
            {
                DefaultRegistryKey = node.AttributeOrNull("defaultKey"),
                AffectedValues = new List<PolicyRegistryListEntry>()
            };
            foreach (XmlNode itemElement in node.ChildNodes)
            {
                if (itemElement.LocalName != "item")
                {
                    continue;
                }

                var listEntry = new PolicyRegistryListEntry
                {
                    RegistryValue = itemElement.Attributes["valueName"].Value,
                    RegistryKey = itemElement.AttributeOrNull("key")
                };
                foreach (XmlNode valElement in itemElement.ChildNodes)
                {
                    if (valElement.LocalName != "value")
                    {
                        continue;
                    }

                    listEntry.Value = LoadRegItem(valElement);
                    break;
                }
                singleList.AffectedValues.Add(listEntry);
            }
            return singleList;
        }

        private static PolicyRegistryValue LoadRegItem(XmlNode node)
        {
            var regItem = new PolicyRegistryValue();
            foreach (XmlNode subElement in node.ChildNodes)
            {
                if (subElement.LocalName == "delete")
                {
                    regItem.RegistryType = PolicyRegistryValueType.Delete;
                    break;
                }

                if (subElement.LocalName == "decimal")
                {
                    regItem.RegistryType = PolicyRegistryValueType.Numeric;
                    regItem.NumberValue = Convert.ToUInt32(subElement.Attributes?["value"].Value);
                    break;
                }

                if (subElement.LocalName != "string")
                {
                    continue;
                }

                regItem.RegistryType = PolicyRegistryValueType.Text;
                regItem.StringValue = subElement.InnerText;
                break;
            }
            return regItem;
        }

        private static void LoadProducts(XmlNode node, string childTagName, AdmxProduct? parent, AdmxFile admx)
        {
            foreach (XmlNode subproductElement in node.ChildNodes)
            {
                if ((subproductElement.LocalName ?? string.Empty) != (childTagName ?? string.Empty))
                {
                    continue;
                }
                var product = new AdmxProduct
                {
                    Id = subproductElement.Attributes["name"].Value,
                    DisplayCode = subproductElement.Attributes["displayName"].Value
                };

                if (parent is not null)
                {
                    product.Version = int.Parse(subproductElement.Attributes["versionIndex"].Value);
                }
                product.Parent = parent;
                product.DefinedIn = admx;
                admx.Products.Add(product);
                if (parent is null)
                {
                    product.Type = AdmxProductType.Product;
                    LoadProducts(subproductElement, "majorVersion", product, admx);
                }
                else if (parent.Parent is null)
                {
                    product.Type = AdmxProductType.MajorRevision;
                    LoadProducts(subproductElement, "minorVersion", product, admx);
                }
                else
                {
                    product.Type = AdmxProductType.MinorRevision;
                }
            }
        }
    }
}