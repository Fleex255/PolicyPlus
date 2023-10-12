using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace PolicyPlus
{
    public static class PolicyProcessing
    {
        public static PolicyState GetPolicyState(IPolicySource PolicySource, PolicyPlusPolicy Policy)
        {
            // Determine the basic state of a policy
            decimal enabledEvidence = 0m;
            decimal disabledEvidence = 0m;
            var rawpol = Policy.RawPolicy;
            void checkOneVal(PolicyRegistryValue Value, string Key, string ValueName, ref decimal EvidenceVar)
            {
                if (Value is null)
                    return;
                if (ValuePresent(Value, PolicySource, Key, ValueName))
                    EvidenceVar += 1m;
            };
            void checkValList(PolicyRegistrySingleList ValList, string DefaultKey, ref decimal EvidenceVar)
            {
                if (ValList is null)
                    return;
                string listKey = string.IsNullOrEmpty(ValList.DefaultRegistryKey) ? DefaultKey : ValList.DefaultRegistryKey;
                foreach (var regVal in ValList.AffectedValues)
                {
                    string entryKey = string.IsNullOrEmpty(regVal.RegistryKey) ? listKey : regVal.RegistryKey;
                    checkOneVal(regVal.Value, entryKey, regVal.RegistryValue, ref EvidenceVar);
                }
            };
            // Check the policy's standard Registry values
            if (!string.IsNullOrEmpty(rawpol.RegistryValue))
            {
                if (rawpol.AffectedValues.OnValue is null)
                {
                    checkOneVal(new PolicyRegistryValue() { NumberValue = 1U, RegistryType = PolicyRegistryValueType.Numeric }, rawpol.RegistryKey, rawpol.RegistryValue, ref enabledEvidence);
                }
                else
                {
                    checkOneVal(rawpol.AffectedValues.OnValue, rawpol.RegistryKey, rawpol.RegistryValue, ref enabledEvidence);
                }
                if (rawpol.AffectedValues.OffValue is null)
                {
                    checkOneVal(new PolicyRegistryValue() { RegistryType = PolicyRegistryValueType.Delete }, rawpol.RegistryKey, rawpol.RegistryValue, ref disabledEvidence);
                }
                else
                {
                    checkOneVal(rawpol.AffectedValues.OffValue, rawpol.RegistryKey, rawpol.RegistryValue, ref disabledEvidence);
                }
            }
            checkValList(rawpol.AffectedValues.OnValueList, rawpol.RegistryKey, ref enabledEvidence);
            checkValList(rawpol.AffectedValues.OffValueList, rawpol.RegistryKey, ref disabledEvidence);
            // Check the policy's elements
            if (rawpol.Elements is not null)
            {
                decimal deletedElements = 0m;
                decimal presentElements = 0m;
                foreach (var elem in rawpol.Elements)
                {
                    string elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? rawpol.RegistryKey : elem.RegistryKey;
                    if (elem.ElementType == "list")
                    {
                        int neededValues = 0;
                        if (PolicySource.WillDeleteValue(elemKey, ""))
                        {
                            deletedElements += 1m;
                            neededValues = 1;
                        }
                        if (PolicySource.GetValueNames(elemKey).Count > 0)
                        {
                            deletedElements -= neededValues;
                            presentElements += 1m;
                        }
                    }
                    else if (elem.ElementType == "boolean")
                    {
                        BooleanPolicyElement booleanElem = (BooleanPolicyElement)elem;
                        if (PolicySource.WillDeleteValue(elemKey, elem.RegistryValue))
                        {
                            deletedElements += 1m; // Implicit checkboxes are deleted when the policy is disabled
                        }
                        else
                        {
                            decimal checkboxDisabled = 0m;
                            checkOneVal(booleanElem.AffectedRegistry.OffValue, elemKey, elem.RegistryValue, ref checkboxDisabled);
                            checkValList(booleanElem.AffectedRegistry.OffValueList, elemKey, ref checkboxDisabled);
                            deletedElements += checkboxDisabled * 0.1m; // Checkboxes in the off state are weak evidence for the policy being disabled
                            checkOneVal(booleanElem.AffectedRegistry.OnValue, elemKey, elem.RegistryValue, ref presentElements);
                            checkValList(booleanElem.AffectedRegistry.OnValueList, elemKey, ref presentElements);
                        }
                    }
                    else if (PolicySource.WillDeleteValue(elemKey, elem.RegistryValue))
                    {
                        deletedElements += 1m;
                    }
                    else if (PolicySource.ContainsValue(elemKey, elem.RegistryValue))
                    {
                        presentElements += 1m;
                    }
                }
                if (presentElements > 0m)
                {
                    enabledEvidence += presentElements;
                }
                else if (deletedElements > 0m)
                {
                    disabledEvidence += deletedElements;
                }
            }
            // Judge the evidence collected
            if (enabledEvidence > disabledEvidence)
            {
                return PolicyState.Enabled;
            }
            else if (disabledEvidence > enabledEvidence)
            {
                return PolicyState.Disabled;
            }
            else if (enabledEvidence == 0m) // No evidence for either side
            {
                return PolicyState.NotConfigured;
            }
            else
            {
                return PolicyState.Unknown;
            }
        }
        private static bool ValuePresent(PolicyRegistryValue Value, IPolicySource Source, string Key, string ValueName)
        {
            // Determine whether the given value is found in the Registry
            switch (Value.RegistryType)
            {
                case PolicyRegistryValueType.Delete:
                    {
                        return Source.WillDeleteValue(Key, ValueName);
                    }
                case PolicyRegistryValueType.Numeric:
                    {
                        if (!Source.ContainsValue(Key, ValueName))
                            return false;
                        var sourceVal = Source.GetValue(Key, ValueName);
                        if (!(sourceVal is uint) & !(sourceVal is int))
                            return false;
                        return Conversions.ToLong(sourceVal) == Value.NumberValue;
                    }
                case PolicyRegistryValueType.Text:
                    {
                        if (!Source.ContainsValue(Key, ValueName))
                            return false;
                        var sourceVal = Source.GetValue(Key, ValueName);
                        if (!(sourceVal is string))
                            return false;
                        return (Conversions.ToString(sourceVal) ?? "") == (Value.StringValue ?? "");
                    }

                default:
                    {
                        throw new InvalidOperationException("Illegal value type");
                    }
            }
        }
        private static bool ValueListPresent(PolicyRegistrySingleList ValueList, IPolicySource Source, string Key, string ValueName)
        {
            // Determine whether all the values in a value list are in the Registry data
            string sublistKey = string.IsNullOrEmpty(ValueList.DefaultRegistryKey) ? Key : ValueList.DefaultRegistryKey;
            return ValueList.AffectedValues.All(e =>
                {
                    string entryKey = string.IsNullOrEmpty(e.RegistryKey) ? sublistKey : e.RegistryKey;
                    return ValuePresent(e.Value, Source, entryKey, e.RegistryValue);
                });
        }
        public static int DeduplicatePolicies(AdmxBundle Workspace)
        {
            // Merge otherwise-identical pairs of user and computer policies into both-section policies
            int dedupeCount = 0;
            foreach (var cat in Workspace.Policies.GroupBy(c => c.Value.Category))
            {
                foreach (var namegroup in cat.GroupBy(p => p.Value.DisplayName).Select(x => x.ToList()).ToList())
                {
                    if (namegroup.Count != 2)
                        continue;
                    var a = namegroup[0].Value;
                    var b = namegroup[1].Value;
                    if ((int)a.RawPolicy.Section + (int)b.RawPolicy.Section != (int)AdmxPolicySection.Both)
                        continue;
                    if ((a.DisplayExplanation ?? "") != (b.DisplayExplanation ?? ""))
                        continue;
                    if ((a.RawPolicy.RegistryKey ?? "") != (b.RawPolicy.RegistryKey ?? ""))
                        continue;
                    a.Category.Policies.Remove(a);
                    Workspace.Policies.Remove(a.UniqueID);
                    b.RawPolicy.Section = AdmxPolicySection.Both;
                    dedupeCount += 1;
                }
            }
            return dedupeCount;
        }
        public static Dictionary<string, object> GetPolicyOptionStates(IPolicySource PolicySource, PolicyPlusPolicy Policy)
        {
            // Get the element states of an enabled policy
            var state = new Dictionary<string, object>();
            if (Policy.RawPolicy.Elements is null)
                return state;
            foreach (var elem in Policy.RawPolicy.Elements)
            {
                string elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? Policy.RawPolicy.RegistryKey : elem.RegistryKey;
                switch (elem.ElementType ?? "")
                {
                    case "decimal":
                        {
                            state.Add(elem.ID, Conversions.ToUInteger(PolicySource.GetValue(elemKey, elem.RegistryValue)));
                            break;
                        }
                    case "boolean":
                        {
                            BooleanPolicyElement booleanElem = (BooleanPolicyElement)elem;
                            state.Add(elem.ID, GetRegistryListState(PolicySource, booleanElem.AffectedRegistry, elemKey, elem.RegistryValue));
                            break;
                        }
                    case "text":
                        {
                            state.Add(elem.ID, PolicySource.GetValue(elemKey, elem.RegistryValue));
                            break;
                        }
                    case "list":
                        {
                            ListPolicyElement listElem = (ListPolicyElement)elem;
                            if (listElem.UserProvidesNames) // Keys matter, use a dictionary
                            {
                                var entries = new Dictionary<string, string>();
                                foreach (var value in PolicySource.GetValueNames(elemKey))
                                    entries.Add(value, Conversions.ToString(PolicySource.GetValue(elemKey, value)));
                                state.Add(elem.ID, entries);
                            }
                            else // Keys don't matter, use a list
                            {
                                var entries = new List<string>();
                                if (listElem.HasPrefix)
                                {
                                    int n = 1;
                                    while (PolicySource.ContainsValue(elemKey, elem.RegistryValue + n))
                                    {
                                        entries.Add(Conversions.ToString(PolicySource.GetValue(elemKey, elem.RegistryValue + n)));
                                        n += 1;
                                    }
                                }
                                else
                                {
                                    foreach (var value in PolicySource.GetValueNames(elemKey))
                                        entries.Add(value);
                                }
                                state.Add(elem.ID, entries);
                            }

                            break;
                        }
                    case "enum":
                        {
                            // Determine which option has results that match the Registry
                            EnumPolicyElement enumElem = (EnumPolicyElement)elem;
                            int selectedIndex = -1;
                            for (int n = 0, loopTo = enumElem.Items.Count - 1; n <= loopTo; n++)
                            {
                                var enumItem = enumElem.Items[n];
                                if (ValuePresent(enumItem.Value, PolicySource, elemKey, elem.RegistryValue))
                                {
                                    if (enumItem.ValueList is null || ValueListPresent(enumItem.ValueList, PolicySource, elemKey, elem.RegistryValue))
                                    {
                                        selectedIndex = n;
                                        break;
                                    }
                                }
                            }
                            state.Add(elem.ID, selectedIndex);
                            break;
                        }
                    case "multiText":
                        {
                            state.Add(elem.ID, PolicySource.GetValue(elemKey, elem.RegistryValue));
                            break;
                        }
                }
            }
            return state;
        }
        private static bool GetRegistryListState(IPolicySource PolicySource, PolicyRegistryList RegList, string DefaultKey, string DefaultValueName)
        {
            // Whether a list of Registry values is present
            bool isListAllPresent(PolicyRegistrySingleList l) => ValueListPresent(l, PolicySource, DefaultKey, DefaultValueName);
            if (RegList.OnValue is not null)
            {
                if (ValuePresent(RegList.OnValue, PolicySource, DefaultKey, DefaultValueName))
                    return true;
            }
            else if (RegList.OnValueList is not null)
            {
                if (isListAllPresent(RegList.OnValueList))
                    return true;
            }
            else if (Conversions.ToUInteger(PolicySource.GetValue(DefaultKey, DefaultValueName)) == 1U)
                return true;
            if (RegList.OffValue is not null)
            {
                if (ValuePresent(RegList.OffValue, PolicySource, DefaultKey, DefaultValueName))
                    return false;
            }
            else if (RegList.OffValueList is not null)
            {
                if (isListAllPresent(RegList.OffValueList))
                    return false;
            }
            return false;
        }
        public static List<RegistryKeyValuePair> GetReferencedRegistryValues(PolicyPlusPolicy Policy)
        {
            return WalkPolicyRegistry(null, Policy, false);
        }
        public static void ForgetPolicy(IPolicySource PolicySource, PolicyPlusPolicy Policy)
        {
            WalkPolicyRegistry(PolicySource, Policy, true);
        }
        private static List<RegistryKeyValuePair> WalkPolicyRegistry(IPolicySource PolicySource, PolicyPlusPolicy Policy, bool Forget)
        {
            // This function handles both GetReferencedRegistryValues and ForgetPolicy because they require searching through the same things
            var entries = new List<RegistryKeyValuePair>();
            void addReg(string Key, string Value)
            {
                var rkvp = new RegistryKeyValuePair() { Key = Key, Value = Value };
                if (!entries.Contains(rkvp))
                    entries.Add(rkvp);
            };
            // Get all Registry values affected by this policy
            var rawpol = Policy.RawPolicy;
            if (!string.IsNullOrEmpty(rawpol.RegistryValue))
                addReg(rawpol.RegistryKey, rawpol.RegistryValue);
            void addSingleList(PolicyRegistrySingleList SingleList, string OverrideKey)
            {
                if (SingleList is null)
                    return;
                string defaultKey = string.IsNullOrEmpty(OverrideKey) ? rawpol.RegistryKey : OverrideKey;
                string listKey = string.IsNullOrEmpty(SingleList.DefaultRegistryKey) ? defaultKey : SingleList.DefaultRegistryKey;
                foreach (var e in SingleList.AffectedValues)
                {
                    string entryKey = string.IsNullOrEmpty(e.RegistryKey) ? listKey : e.RegistryKey;
                    addReg(entryKey, e.RegistryValue);
                }
            };
            addSingleList(rawpol.AffectedValues.OnValueList, "");
            addSingleList(rawpol.AffectedValues.OffValueList, "");
            if (rawpol.Elements is not null)
            {
                foreach (var elem in rawpol.Elements)
                {
                    string elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? rawpol.RegistryKey : elem.RegistryKey;
                    if (elem.ElementType != "list")
                        addReg(elemKey, elem.RegistryValue);
                    switch (elem.ElementType ?? "")
                    {
                        case "boolean":
                            {
                                BooleanPolicyElement booleanElem = (BooleanPolicyElement)elem;
                                addSingleList(booleanElem.AffectedRegistry.OnValueList, elemKey);
                                addSingleList(booleanElem.AffectedRegistry.OffValueList, elemKey);
                                break;
                            }
                        case "enum":
                            {
                                EnumPolicyElement enumElem = (EnumPolicyElement)elem;
                                foreach (var e in enumElem.Items)
                                    addSingleList(e.ValueList, elemKey);
                                break;
                            }
                        case "list":
                            {
                                if (Forget)
                                {
                                    PolicySource.ClearKey(elemKey); // Delete all the values
                                    PolicySource.ForgetKeyClearance(elemKey);
                                }
                                else
                                {
                                    addReg(elemKey, "");
                                }

                                break;
                            }
                    }
                }
            }
            if (Forget) // Remove them if forgetting
            {
                foreach (var e in entries)
                    PolicySource.ForgetValue(e.Key, e.Value);
            }
            return entries;
        }
        public static void SetPolicyState(IPolicySource PolicySource, PolicyPlusPolicy Policy, PolicyState State, Dictionary<string, object> Options)
        {
            // Write a full policy state to the policy source
            void setValue(string Key, string ValueName, PolicyRegistryValue Value)
            {
                if (Value is null)
                    return;
                switch (Value.RegistryType)
                {
                    case PolicyRegistryValueType.Delete:
                        {
                            PolicySource.DeleteValue(Key, ValueName);
                            break;
                        }
                    case PolicyRegistryValueType.Numeric:
                        {
                            PolicySource.SetValue(Key, ValueName, Value.NumberValue, Microsoft.Win32.RegistryValueKind.DWord);
                            break;
                        }
                    case PolicyRegistryValueType.Text:
                        {
                            PolicySource.SetValue(Key, ValueName, Value.StringValue, Microsoft.Win32.RegistryValueKind.String);
                            break;
                        }
                }
            };
            void setSingleList(PolicyRegistrySingleList SingleList, string DefaultKey)
            {
                if (SingleList is null)
                    return;
                string listKey = string.IsNullOrEmpty(SingleList.DefaultRegistryKey) ? DefaultKey : SingleList.DefaultRegistryKey;
                foreach (var e in SingleList.AffectedValues)
                {
                    string itemKey = string.IsNullOrEmpty(e.RegistryKey) ? listKey : e.RegistryKey;
                    setValue(itemKey, e.RegistryValue, e.Value);
                }
            };
            void setList(PolicyRegistryList List, string DefaultKey, string DefaultValue, bool IsOn)
            {
                if (List is null)
                    return;
                if (IsOn)
                {
                    setValue(DefaultKey, DefaultValue, List.OnValue);
                    setSingleList(List.OnValueList, DefaultKey);
                }
                else
                {
                    setValue(DefaultKey, DefaultValue, List.OffValue);
                    setSingleList(List.OffValueList, DefaultKey);
                }
            };
            var rawpol = Policy.RawPolicy;
            switch (State)
            {
                case PolicyState.Enabled:
                    {
                        if (rawpol.AffectedValues.OnValue is null & !string.IsNullOrEmpty(rawpol.RegistryValue))
                            PolicySource.SetValue(rawpol.RegistryKey, rawpol.RegistryValue, 1U, Microsoft.Win32.RegistryValueKind.DWord);
                        setList(rawpol.AffectedValues, rawpol.RegistryKey, rawpol.RegistryValue, true);
                        if (rawpol.Elements is not null) // Write the elements' states
                        {
                            foreach (var elem in rawpol.Elements)
                            {
                                string elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? rawpol.RegistryKey : elem.RegistryKey;
                                if (!Options.ContainsKey(elem.ID))
                                    continue;
                                var optionData = Options[elem.ID];
                                switch (elem.ElementType ?? "")
                                {
                                    case "decimal":
                                        {
                                            DecimalPolicyElement decimalElem = (DecimalPolicyElement)elem;
                                            if (decimalElem.StoreAsText)
                                            {
                                                PolicySource.SetValue(elemKey, elem.RegistryValue, Conversions.ToString(optionData), Microsoft.Win32.RegistryValueKind.String);
                                            }
                                            else
                                            {
                                                PolicySource.SetValue(elemKey, elem.RegistryValue, Conversions.ToUInteger(optionData), Microsoft.Win32.RegistryValueKind.DWord);
                                            }

                                            break;
                                        }
                                    case "boolean":
                                        {
                                            BooleanPolicyElement booleanElem = (BooleanPolicyElement)elem;
                                            bool checkState = Conversions.ToBoolean(optionData);
                                            if (booleanElem.AffectedRegistry.OnValue is null & checkState)
                                            {
                                                PolicySource.SetValue(elemKey, elem.RegistryValue, 1U, Microsoft.Win32.RegistryValueKind.DWord);
                                            }
                                            if (booleanElem.AffectedRegistry.OffValue is null & !checkState)
                                            {
                                                PolicySource.DeleteValue(elemKey, elem.RegistryValue);
                                            }
                                            setList(booleanElem.AffectedRegistry, elemKey, elem.RegistryValue, checkState);
                                            break;
                                        }
                                    case "text":
                                        {
                                            TextPolicyElement textElem = (TextPolicyElement)elem;
                                            var regType = textElem.RegExpandSz ? Microsoft.Win32.RegistryValueKind.ExpandString : Microsoft.Win32.RegistryValueKind.String;
                                            PolicySource.SetValue(elemKey, elem.RegistryValue, optionData, regType);
                                            break;
                                        }
                                    case "list":
                                        {
                                            ListPolicyElement listElem = (ListPolicyElement)elem;
                                            if (!listElem.NoPurgeOthers)
                                                PolicySource.ClearKey(elemKey);
                                            if (optionData is null)
                                                continue;
                                            var regType = listElem.RegExpandSz ? Microsoft.Win32.RegistryValueKind.ExpandString : Microsoft.Win32.RegistryValueKind.String;
                                            if (listElem.UserProvidesNames)
                                            {
                                                Dictionary<string, string> items = (Dictionary<string, string>)optionData;
                                                foreach (var i in items)
                                                    PolicySource.SetValue(elemKey, i.Key, i.Value, regType);
                                            }
                                            else
                                            {
                                                List<string> items = (List<string>)optionData;
                                                int n = 1;
                                                while (n <= items.Count)
                                                {
                                                    string valueName = listElem.HasPrefix ? listElem.RegistryValue + n : items[n - 1];
                                                    PolicySource.SetValue(elemKey, valueName, items[n - 1], regType);
                                                    n += 1;
                                                }
                                            }

                                            break;
                                        }
                                    case "enum":
                                        {
                                            EnumPolicyElement enumElem = (EnumPolicyElement)elem;
                                            var selItem = enumElem.Items[Conversions.ToInteger(optionData)];
                                            setValue(elemKey, elem.RegistryValue, selItem.Value);
                                            setSingleList(selItem.ValueList, elemKey);
                                            break;
                                        }
                                    case "multiText":
                                        {
                                            PolicySource.SetValue(elemKey, elem.RegistryValue, optionData, Microsoft.Win32.RegistryValueKind.MultiString);
                                            break;
                                        }
                                }
                            }
                        }

                        break;
                    }
                case PolicyState.Disabled:
                    {
                        if (rawpol.AffectedValues.OffValue is null & !string.IsNullOrEmpty(rawpol.RegistryValue))
                            PolicySource.DeleteValue(rawpol.RegistryKey, rawpol.RegistryValue);
                        setList(rawpol.AffectedValues, rawpol.RegistryKey, rawpol.RegistryValue, false);
                        if (rawpol.Elements is not null) // Mark all the elements deleted
                        {
                            foreach (var elem in rawpol.Elements)
                            {
                                string elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? rawpol.RegistryKey : elem.RegistryKey;
                                if (elem.ElementType == "list")
                                {
                                    PolicySource.ClearKey(elemKey);
                                }
                                else if (elem.ElementType == "boolean")
                                {
                                    BooleanPolicyElement booleanElem = (BooleanPolicyElement)elem;
                                    if (booleanElem.AffectedRegistry.OffValue is not null | booleanElem.AffectedRegistry.OffValueList is not null)
                                    {
                                        // Non-implicit checkboxes get their "off" value set when the policy is disabled
                                        setList(booleanElem.AffectedRegistry, elemKey, elem.RegistryValue, false);
                                    }
                                    else
                                    {
                                        PolicySource.DeleteValue(elemKey, elem.RegistryValue);
                                    }
                                }
                                else
                                {
                                    PolicySource.DeleteValue(elemKey, elem.RegistryValue);
                                }
                            }
                        }

                        break;
                    }
            }
        }
        public static bool IsPolicySupported(PolicyPlusPolicy Policy, List<PolicyPlusProduct> Products, bool AlwaysUseAny, bool ApproveLiterals)
        {
            // Whether a policy is supported on a computer with the given products
            if (Policy.SupportedOn is null || Policy.SupportedOn.RawSupport.Logic == AdmxSupportLogicType.Blank)
                return ApproveLiterals;
            bool supEntryMet(PolicyPlusSupportEntry SupportEntry) // Only for products (not support definitions)
            {
                if (SupportEntry.Product is null)
                    return ApproveLiterals;
                if (Products.Contains(SupportEntry.Product) & !SupportEntry.RawSupportEntry.IsRange)
                    return true;
                if (SupportEntry.Product.Children is null || SupportEntry.Product.Children.Count == 0)
                    return false; // Ranges only apply to parent products
                int rangeMin = SupportEntry.RawSupportEntry.MinVersion ?? 0;
                int rangeMax = SupportEntry.RawSupportEntry.MaxVersion ?? SupportEntry.Product.Children.Max(p => p.RawProduct.Version);
                for (int v = rangeMin, loopTo = rangeMax; v <= loopTo; v++)
                {
                    int version = v; // To suppress compiler warnings about iteration variable in lambdas
                    var subproduct = SupportEntry.Product.Children.FirstOrDefault(p => p.RawProduct.Version == version);
                    if (subproduct is null)
                        continue;
                    if (Products.Contains(subproduct))
                        return true;
                    if (subproduct.Children is not null && subproduct.Children.Any(p => Products.Contains(p)))
                        return true;
                }
                return false;
            };
            var entriesSeen = new List<PolicyPlusSupport>();
            Func<PolicyPlusSupport, bool> supDefMet;
            supDefMet = new Func<PolicyPlusSupport, bool>((Support) =>
                {
                    if (entriesSeen.Contains(Support))
                        return false; // Cyclic dependencies
                    entriesSeen.Add(Support);
                    bool requireAll = AlwaysUseAny ? Support.RawSupport.Logic == AdmxSupportLogicType.AllOf : false;
                    // It's much faster to check for plain products, so do that first
                    foreach (var supElem in Support.Elements.Where(e => e.SupportDefinition is null))
                    {
                        bool isMet = supEntryMet(supElem);
                        if (requireAll)
                        {
                            if (!isMet)
                                return false;
                        }
                        else if (isMet)
                            return true;
                    }
                    foreach (var subDef in Support.Elements.Where(e => e.SupportDefinition is not null))
                    {
                        bool isMet = supDefMet(subDef.SupportDefinition);
                        if (requireAll)
                        {
                            if (!isMet)
                                return false;
                        }
                        else if (isMet)
                            return true;
                    }
                    return requireAll; // If all were required and this function hasn't exited yet, all are matched
                });
            return supDefMet(Policy.SupportedOn);
        }
    }

    public enum PolicyState
    {
        NotConfigured = 0,
        Disabled = 1,
        Enabled = 2,
        Unknown = 3
    }
    public class RegistryKeyValuePair : IEquatable<RegistryKeyValuePair>
    {
        public string Key;
        public string Value;
        public bool EqualsRKVP(RegistryKeyValuePair other)
        {
            return other.Key.Equals(Key, StringComparison.InvariantCultureIgnoreCase) & other.Value.Equals(Value, StringComparison.InvariantCultureIgnoreCase);
        }

        bool IEquatable<RegistryKeyValuePair>.Equals(RegistryKeyValuePair other) => EqualsRKVP(other);
        public override bool Equals(object obj)
        {
            if (!(obj is RegistryKeyValuePair))
                return false;
            return EqualsRKVP((RegistryKeyValuePair)obj);
        }
        public override int GetHashCode()
        {
            return Key.ToLowerInvariant().GetHashCode() ^ Value.ToLowerInvariant().GetHashCode();
        }
    }
}