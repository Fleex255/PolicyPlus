using System;
using System.Collections.Generic;
using System.Linq;
using PolicyPlus.csharp.UI.Elements;

namespace PolicyPlus.csharp.Models
{
    public static class PolicyProcessing
    {
        public static PolicyState GetPolicyState(IPolicySource policySource, PolicyPlusPolicy policy)
        {
            // Determine the basic state of a policy
            var enabledEvidence = 0m;
            var disabledEvidence = 0m;
            var rawpol = policy.RawPolicy;

            // Check the policy's standard Registry values
            if (!string.IsNullOrEmpty(rawpol.RegistryValue))
            {
                if (rawpol.AffectedValues.OnValue is null)
                {
                    CheckOneVal(new PolicyRegistryValue { NumberValue = 1U, RegistryType = PolicyRegistryValueType.Numeric }, rawpol.RegistryKey, rawpol.RegistryValue, ref enabledEvidence);
                }
                else
                {
                    CheckOneVal(rawpol.AffectedValues.OnValue, rawpol.RegistryKey, rawpol.RegistryValue, ref enabledEvidence);
                }
                if (rawpol.AffectedValues.OffValue is null)
                {
                    CheckOneVal(new PolicyRegistryValue { RegistryType = PolicyRegistryValueType.Delete }, rawpol.RegistryKey, rawpol.RegistryValue, ref disabledEvidence);
                }
                else
                {
                    CheckOneVal(rawpol.AffectedValues.OffValue, rawpol.RegistryKey, rawpol.RegistryValue, ref disabledEvidence);
                }
            }
            CheckValList(rawpol.AffectedValues.OnValueList, rawpol.RegistryKey, ref enabledEvidence);
            CheckValList(rawpol.AffectedValues.OffValueList, rawpol.RegistryKey, ref disabledEvidence);
            // Check the policy's elements
            if (rawpol.Elements is not null)
            {
                var deletedElements = 0m;
                var presentElements = 0m;
                foreach (var elem in rawpol.Elements)
                {
                    var elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? rawpol.RegistryKey : elem.RegistryKey;
                    switch (elem.ElementType)
                    {
                        case "list":
                            {
                                var neededValues = 0;
                                if (policySource.WillDeleteValue(elemKey, ""))
                                {
                                    deletedElements++;
                                    neededValues = 1;
                                }

                                if (policySource.GetValueNames(elemKey).Count == 0)
                                {
                                    continue;
                                }

                                deletedElements -= neededValues;
                                presentElements++;
                                break;
                            }
                        case "boolean":
                            {
                                var booleanElem = (BooleanPolicyElement)elem;
                                if (policySource.WillDeleteValue(elemKey, elem.RegistryValue))
                                {
                                    deletedElements++; // Implicit checkboxes are deleted when the policy is disabled
                                }
                                else
                                {
                                    var checkboxDisabled = 0m;
                                    CheckOneVal(booleanElem.AffectedRegistry.OffValue, elemKey, elem.RegistryValue, ref checkboxDisabled);
                                    CheckValList(booleanElem.AffectedRegistry.OffValueList, elemKey, ref checkboxDisabled);
                                    deletedElements += checkboxDisabled * 0.1m; // Checkboxes in the off state are weak evidence for the policy being disabled
                                    CheckOneVal(booleanElem.AffectedRegistry.OnValue, elemKey, elem.RegistryValue, ref presentElements);
                                    CheckValList(booleanElem.AffectedRegistry.OnValueList, elemKey, ref presentElements);
                                }

                                break;
                            }
                        default:
                            {
                                if (policySource.WillDeleteValue(elemKey, elem.RegistryValue))
                                {
                                    deletedElements++;
                                }
                                else if (policySource.ContainsValue(elemKey, elem.RegistryValue))
                                {
                                    presentElements++;
                                }

                                break;
                            }
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

            if (disabledEvidence > enabledEvidence)
            {
                return PolicyState.Disabled;
            }
            return enabledEvidence == 0m ? // No evidence for either side, or unknown
                PolicyState.NotConfigured : PolicyState.Unknown;

            void CheckOneVal(PolicyRegistryValue value, string key, string valueName, ref decimal evidenceVar)
            {
                if (value is null)
                {
                    return;
                }

                if (ValuePresent(value, policySource, key, valueName))
                {
                    evidenceVar++;
                }
            }

            void CheckValList(PolicyRegistrySingleList valList, string defaultKey, ref decimal evidenceVar)
            {
                if (valList is null)
                {
                    return;
                }

                var listKey = string.IsNullOrEmpty(valList.DefaultRegistryKey) ? defaultKey : valList.DefaultRegistryKey;
                foreach (var regVal in valList.AffectedValues)
                {
                    var entryKey = string.IsNullOrEmpty(regVal.RegistryKey) ? listKey : regVal.RegistryKey;
                    CheckOneVal(regVal.Value, entryKey, regVal.RegistryValue, ref evidenceVar);
                }
            }
        }

        private static bool ValuePresent(PolicyRegistryValue value, IPolicySource source, string key, string valueName)
        {
            // Determine whether the given value is found in the Registry
            switch (value.RegistryType)
            {
                case PolicyRegistryValueType.Delete:
                    {
                        return source.WillDeleteValue(key, valueName);
                    }
                case PolicyRegistryValueType.Numeric:
                    {
                        if (!source.ContainsValue(key, valueName))
                        {
                            return false;
                        }

                        var sourceVal = source.GetValue(key, valueName);
                        if (sourceVal is not uint && sourceVal is not int)
                        {
                            return false;
                        }

                        return (uint)sourceVal == value.NumberValue;
                    }
                case PolicyRegistryValueType.Text:
                    {
                        if (!source.ContainsValue(key, valueName))
                        {
                            return false;
                        }

                        var sourceVal = source.GetValue(key, valueName);
                        if (sourceVal is not string)
                        {
                            return false;
                        }

                        return (sourceVal.ToString() ?? "") == (value.StringValue ?? "");
                    }

                default:
                    {
                        throw new InvalidOperationException("Illegal value type");
                    }
            }
        }

        private static bool ValueListPresent(PolicyRegistrySingleList valueList, IPolicySource source, string key, string valueName)
        {
            // Determine whether all the values in a value list are in the Registry data
            var sublistKey = string.IsNullOrEmpty(valueList.DefaultRegistryKey) ? key : valueList.DefaultRegistryKey;
            return valueList.AffectedValues.All(e =>
                {
                    var entryKey = string.IsNullOrEmpty(e.RegistryKey) ? sublistKey : e.RegistryKey;
                    return ValuePresent(e.Value, source, entryKey, valueName);
                });
        }

        public static int DeduplicatePolicies(AdmxBundle workspace)
        {
            // Merge otherwise-identical pairs of user and computer policies into both-section policies
            var dedupeCount = 0;
            foreach (var cat in workspace.Policies.GroupBy(c => c.Value.Category))
            {
                foreach (var namegroup in cat.GroupBy(p => p.Value.DisplayName).Select(x => x.ToList()).ToList())
                {
                    if (namegroup.Count != 2)
                    {
                        continue;
                    }

                    var a = namegroup[0].Value;
                    var b = namegroup[1].Value;
                    if ((int)a.RawPolicy.Section + (int)b.RawPolicy.Section != (int)AdmxPolicySection.Both)
                    {
                        continue;
                    }

                    if ((a.DisplayExplanation ?? "") != (b.DisplayExplanation ?? ""))
                    {
                        continue;
                    }

                    if ((a.RawPolicy.RegistryKey ?? "") != (b.RawPolicy.RegistryKey ?? ""))
                    {
                        continue;
                    }

                    _ = a.Category.Policies.Remove(a);
                    _ = workspace.Policies.Remove(a.UniqueId);
                    b.RawPolicy.Section = AdmxPolicySection.Both;
                    dedupeCount++;
                }
            }
            return dedupeCount;
        }

        public static Dictionary<string, object> GetPolicyOptionStates(IPolicySource policySource, PolicyPlusPolicy policy)
        {
            // Get the element states of an enabled policy
            var state = new Dictionary<string, object>();
            if (policy.RawPolicy.Elements is null)
            {
                return state;
            }

            foreach (var elem in policy.RawPolicy.Elements)
            {
                var elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? policy.RawPolicy.RegistryKey : elem.RegistryKey;
                switch (elem.ElementType ?? "")
                {
                    case "decimal":
                        {
                            state.Add(elem.Id, Convert.ToUInt32(policySource.GetValue(elemKey, elem.RegistryValue)));
                            break;
                        }
                    case "boolean":
                        {
                            var booleanElem = (BooleanPolicyElement)elem;
                            state.Add(elem.Id, GetRegistryListState(policySource, booleanElem.AffectedRegistry, elemKey, elem.RegistryValue));
                            break;
                        }
                    case "text":
                        {
                            state.Add(elem.Id, policySource.GetValue(elemKey, elem.RegistryValue));
                            break;
                        }
                    case "list":
                        {
                            var listElem = (ListPolicyElement)elem;
                            if (listElem.UserProvidesNames) // Keys matter, use a dictionary
                            {
                                var entries = policySource.GetValueNames(elemKey).ToDictionary(value => value, value => policySource.GetValue(elemKey, value).ToString());

                                state.Add(elem.Id, entries);
                            }
                            else // Keys don't matter, use a list
                            {
                                var entries = new List<string>();
                                if (listElem.HasPrefix)
                                {
                                    for (var n = 1; policySource.ContainsValue(elemKey, elem.RegistryValue + n); n++)
                                    {
                                        var st = policySource.GetValue(elemKey, elem.RegistryValue + n)?.ToString();
                                        if (st is null)
                                        {
                                            continue;
                                        }

                                        entries.Add(st);
                                    }
                                }
                                else
                                {
                                    entries.AddRange(policySource.GetValueNames(elemKey));
                                }
                                state.Add(elem.Id, entries);
                            }

                            break;
                        }
                    case "enum":
                        {
                            // Determine which option has results that match the Registry
                            var enumElem = (EnumPolicyElement)elem;
                            var selectedIndex = -1;
                            for (int n = 0, loopTo = enumElem.Items.Count - 1; n <= loopTo; n++)
                            {
                                var enumItem = enumElem.Items[n];
                                if (!ValuePresent(enumItem.Value, policySource, elemKey, elem.RegistryValue))
                                {
                                    continue;
                                }

                                if (enumItem.ValueList is not null && !ValueListPresent(enumItem.ValueList,
                                        policySource, elemKey, elem.RegistryValue))
                                {
                                    continue;
                                }

                                selectedIndex = n;
                                break;
                            }
                            state.Add(elem.Id, selectedIndex);
                            break;
                        }
                    case "multiText":
                        {
                            state.Add(elem.Id, policySource.GetValue(elemKey, elem.RegistryValue));
                            break;
                        }
                }
            }
            return state;
        }

        private static bool GetRegistryListState(IPolicySource policySource, PolicyRegistryList regList, string defaultKey, string defaultValueName)
        {
            if (regList.OnValue is not null)
            {
                if (ValuePresent(regList.OnValue, policySource, defaultKey, defaultValueName))
                {
                    return true;
                }
            }
            else if (regList.OnValueList is not null)
            {
                if (IsListAllPresent(regList.OnValueList, policySource, defaultKey, defaultValueName))
                {
                    return true;
                }
            }
            else if (Convert.ToUInt32(policySource.GetValue(defaultKey, defaultValueName)) == 1U)
            {
                return true;
            }

            if (regList.OffValue is null)
            {
                if (regList.OffValueList is not null && IsListAllPresent(regList.OffValueList, policySource, defaultKey, defaultValueName))
                {
                    return false;
                }
            }
            else
            {
                if (ValuePresent(regList.OffValue, policySource, defaultKey, defaultValueName))
                {
                    return false;
                }
            }

            return false;
        }

        // Whether a list of Registry values is present
        private static bool IsListAllPresent(PolicyRegistrySingleList l, IPolicySource policySource, string defaultKey, string defaultValueName) => ValueListPresent(l, policySource, defaultKey, defaultValueName);

        public static List<RegistryKeyValuePair> GetReferencedRegistryValues(PolicyPlusPolicy policy) => WalkPolicyRegistry(null, policy, false);

        public static void ForgetPolicy(IPolicySource policySource, PolicyPlusPolicy policy) => WalkPolicyRegistry(policySource, policy, true);

        private static List<RegistryKeyValuePair> WalkPolicyRegistry(IPolicySource? policySource, PolicyPlusPolicy policy, bool forget)
        {
            // This function handles both GetReferencedRegistryValues and ForgetPolicy because they
            // require searching through the same things
            var entries = new List<RegistryKeyValuePair>();

            // Get all Registry values affected by this policy
            var rawpol = policy.RawPolicy;
            if (!string.IsNullOrEmpty(rawpol.RegistryValue))
            {
                AddReg(rawpol.RegistryKey, rawpol.RegistryValue, entries);
            }
            AddSingleList(rawpol.AffectedValues.OnValueList, "", rawpol, entries);
            AddSingleList(rawpol.AffectedValues.OffValueList, "", rawpol, entries);
            if (rawpol.Elements is not null)
            {
                foreach (var elem in rawpol.Elements)
                {
                    var elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? rawpol.RegistryKey : elem.RegistryKey;
                    if (elem.ElementType != "list")
                    {
                        AddReg(elemKey, elem.RegistryValue, entries);
                    }

                    switch (elem.ElementType ?? "")
                    {
                        case "boolean":
                            {
                                var booleanElem = (BooleanPolicyElement)elem;
                                AddSingleList(booleanElem.AffectedRegistry.OnValueList, elemKey, rawpol, entries);
                                AddSingleList(booleanElem.AffectedRegistry.OffValueList, elemKey, rawpol, entries);
                                break;
                            }
                        case "enum":
                            {
                                var enumElem = (EnumPolicyElement)elem;
                                foreach (var e in enumElem.Items)
                                {
                                    AddSingleList(e.ValueList, elemKey, rawpol, entries);
                                }

                                break;
                            }
                        case "list":
                            {
                                if (policySource is null)
                                {
                                    break;
                                }
                                if (forget)
                                {
                                    policySource.ClearKey(elemKey); // Delete all the values
                                    policySource.ForgetKeyClearance(elemKey);
                                }
                                else
                                {
                                    AddReg(elemKey, "", entries);
                                }

                                break;
                            }
                    }
                }
            }

            if (forget && policySource is not null)
            {
                foreach (var e in entries)
                {
                    policySource.ForgetValue(e.Key, e.Value);
                }
            }
            return entries;
        }

        private static void AddSingleList(PolicyRegistrySingleList singleList, string overrideKey, AdmxPolicy rawpol, List<RegistryKeyValuePair> entries)
        {
            if (singleList is null)
            {
                return;
            }

            var defaultKey = string.IsNullOrEmpty(overrideKey) ? rawpol.RegistryKey : overrideKey;
            var listKey = string.IsNullOrEmpty(singleList.DefaultRegistryKey) ? defaultKey : singleList.DefaultRegistryKey;
            foreach (var e in singleList.AffectedValues)
            {
                var entryKey = string.IsNullOrEmpty(e.RegistryKey) ? listKey : e.RegistryKey;
                AddReg(entryKey, e.RegistryValue, entries);
            }
        }

        private static void AddReg(string key, string value, ICollection<RegistryKeyValuePair> entries)
        {
            var rkvp = new RegistryKeyValuePair { Key = key, Value = value };
            if (!entries.Contains(rkvp))
            {
                entries.Add(rkvp);
            }
        }

        public static void SetPolicyState(IPolicySource policySource, PolicyPlusPolicy policy, PolicyState state, Dictionary<string, object>? options)
        {
            var rawpol = policy.RawPolicy;
            switch (state)
            {
                case PolicyState.Enabled:
                    {
                        if (rawpol.AffectedValues.OnValue is null && !string.IsNullOrEmpty(rawpol.RegistryValue))
                        {
                            policySource.SetValue(rawpol.RegistryKey, rawpol.RegistryValue, 1U, Microsoft.Win32.RegistryValueKind.DWord);
                        }

                        SetList(rawpol.AffectedValues, rawpol.RegistryKey, rawpol.RegistryValue, true, policySource);
                        if (rawpol.Elements is not null) // Write the elements' states
                        {
                            foreach (var elem in rawpol.Elements)
                            {
                                var elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? rawpol.RegistryKey : elem.RegistryKey;
                                if (options?.ContainsKey(elem.Id) == false)
                                {
                                    continue;
                                }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                var optionData = options[elem.Id];
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                switch (elem.ElementType ?? "")
                                {
                                    case "decimal":
                                        {
                                            var decimalElem = (DecimalPolicyElement)elem;
                                            if (decimalElem.StoreAsText)
                                            {
                                                policySource.SetValue(elemKey, elem.RegistryValue, optionData.ToString()!, Microsoft.Win32.RegistryValueKind.String);
                                            }
                                            else
                                            {
                                                policySource.SetValue(elemKey, elem.RegistryValue, Convert.ToUInt32(optionData), Microsoft.Win32.RegistryValueKind.DWord);
                                            }

                                            break;
                                        }
                                    case "boolean":
                                        {
                                            var booleanElem = (BooleanPolicyElement)elem;
                                            var checkState = (bool)optionData;
                                            if (booleanElem.AffectedRegistry.OnValue is null && checkState)
                                            {
                                                policySource.SetValue(elemKey, elem.RegistryValue, 1U, Microsoft.Win32.RegistryValueKind.DWord);
                                            }
                                            if (booleanElem.AffectedRegistry.OffValue is null && !checkState)
                                            {
                                                policySource.DeleteValue(elemKey, elem.RegistryValue);
                                            }
                                            SetList(booleanElem.AffectedRegistry, elemKey, elem.RegistryValue, checkState, policySource);
                                            break;
                                        }
                                    case "text":
                                        {
                                            var textElem = (TextPolicyElement)elem;
                                            var regType = textElem.RegExpandSz ? Microsoft.Win32.RegistryValueKind.ExpandString : Microsoft.Win32.RegistryValueKind.String;
                                            policySource.SetValue(elemKey, elem.RegistryValue, optionData, regType);
                                            break;
                                        }
                                    case "list":
                                        {
                                            var listElem = (ListPolicyElement)elem;
                                            if (!listElem.NoPurgeOthers)
                                            {
                                                policySource.ClearKey(elemKey);
                                            }

                                            if (optionData is null)
                                            {
                                                continue;
                                            }

                                            var regType = listElem.RegExpandSz ? Microsoft.Win32.RegistryValueKind.ExpandString : Microsoft.Win32.RegistryValueKind.String;
                                            if (listElem.UserProvidesNames)
                                            {
                                                foreach (var i in (Dictionary<string, string>)optionData)
                                                {
                                                    policySource.SetValue(elemKey, i.Key, i.Value, regType);
                                                }
                                            }
                                            else
                                            {
                                                var items = (List<string>)optionData;
                                                for (var n = 1; n <= items.Count; n++)
                                                {
                                                    var valueName = listElem.HasPrefix ? listElem.RegistryValue + n : items[n - 1];
                                                    policySource.SetValue(elemKey, valueName, items[n - 1], regType);
                                                }
                                            }

                                            break;
                                        }
                                    case "enum":
                                        {
                                            var enumElem = (EnumPolicyElement)elem;
                                            var selItem = enumElem.Items[(int)optionData];
                                            SetValue(elemKey, elem.RegistryValue, selItem.Value, policySource);
                                            SetSingleList(selItem.ValueList, elemKey, policySource);
                                            break;
                                        }
                                    case "multiText":
                                        {
                                            policySource.SetValue(elemKey, elem.RegistryValue, optionData, Microsoft.Win32.RegistryValueKind.MultiString);
                                            break;
                                        }
                                }
                            }
                        }

                        break;
                    }
                case PolicyState.Disabled:
                    {
                        if (rawpol.AffectedValues.OffValue is null && !string.IsNullOrEmpty(rawpol.RegistryValue))
                        {
                            policySource.DeleteValue(rawpol.RegistryKey, rawpol.RegistryValue);
                        }

                        SetList(rawpol.AffectedValues, rawpol.RegistryKey, rawpol.RegistryValue, false, policySource);
                        if (rawpol.Elements is not null) // Mark all the elements deleted
                        {
                            foreach (var elem in rawpol.Elements)
                            {
                                var elemKey = string.IsNullOrEmpty(elem.RegistryKey) ? rawpol.RegistryKey : elem.RegistryKey;
                                if (elem.ElementType == "list")
                                {
                                    policySource.ClearKey(elemKey);
                                }
                                else if (elem.ElementType == "boolean")
                                {
                                    var booleanElem = (BooleanPolicyElement)elem;
                                    if (booleanElem.AffectedRegistry.OffValue is not null || booleanElem.AffectedRegistry.OffValueList is not null)
                                    {
                                        // Non-implicit checkboxes get their "off" value set when
                                        // the policy is disabled
                                        SetList(booleanElem.AffectedRegistry, elemKey, elem.RegistryValue, false, policySource);
                                    }
                                    else
                                    {
                                        policySource.DeleteValue(elemKey, elem.RegistryValue);
                                    }
                                }
                                else
                                {
                                    policySource.DeleteValue(elemKey, elem.RegistryValue);
                                }
                            }
                        }

                        break;
                    }
                case PolicyState.NotConfigured:
                case PolicyState.Unknown:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private static void SetList(PolicyRegistryList list, string defaultKey, string defaultValue, bool isOn, IPolicySource policySource)
        {
            if (list is null)
            {
                return;
            }

            if (isOn)
            {
                SetValue(defaultKey, defaultValue, list.OnValue, policySource);
                SetSingleList(list.OnValueList, defaultKey, policySource);
            }
            else
            {
                SetValue(defaultKey, defaultValue, list.OffValue, policySource);
                SetSingleList(list.OffValueList, defaultKey, policySource);
            }
        }

        private static void SetSingleList(PolicyRegistrySingleList singleList, string defaultKey, IPolicySource policySource)
        {
            if (singleList is null)
            {
                return;
            }

            var listKey = string.IsNullOrEmpty(singleList.DefaultRegistryKey) ? defaultKey : singleList.DefaultRegistryKey;
            foreach (var e in singleList.AffectedValues)
            {
                var itemKey = string.IsNullOrEmpty(e.RegistryKey) ? listKey : e.RegistryKey;
                SetValue(itemKey, e.RegistryValue, e.Value, policySource);
            }
        }

        private static void SetValue(string key, string valueName, PolicyRegistryValue value, IPolicySource policySource)
        {
            // Write a full policy state to the policy source
            if (value is null)
            {
                return;
            }

            switch (value.RegistryType)
            {
                case PolicyRegistryValueType.Delete:
                    {
                        policySource.DeleteValue(key, valueName);
                        break;
                    }
                case PolicyRegistryValueType.Numeric:
                    {
                        policySource.SetValue(key, valueName, value.NumberValue, Microsoft.Win32.RegistryValueKind.DWord);
                        break;
                    }
                case PolicyRegistryValueType.Text:
                    {
                        policySource.SetValue(key, valueName, value.StringValue, Microsoft.Win32.RegistryValueKind.String);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(value.RegistryType.ToString());
            }
        }

        public static bool IsPolicySupported(PolicyPlusPolicy policy, List<PolicyPlusProduct> products, bool alwaysUseAny, bool approveLiterals)
        {
            // Whether a policy is supported on a computer with the given products
            if (policy.SupportedOn is null || policy.SupportedOn.RawSupport.Logic == AdmxSupportLogicType.Blank)
            {
                return approveLiterals;
            }
            var entriesSeen = new List<PolicyPlusSupport>();
            return SupDefMet(policy.SupportedOn, entriesSeen, alwaysUseAny, approveLiterals, products);
        }

        private static bool SupDefMet(PolicyPlusSupport support, ICollection<PolicyPlusSupport> entriesSeen, bool alwaysUseAny, bool approveLiterals, List<PolicyPlusProduct> products)
        {
            if (entriesSeen.Contains(support))
            {
                return false; // Cyclic dependencies
            }

            entriesSeen.Add(support);
            var requireAll = alwaysUseAny && support.RawSupport.Logic == AdmxSupportLogicType.AllOf;
            // It's much faster to check for plain products, so do that first
            foreach (var supElem in support.Elements.Where(e => e.SupportDefinition is null))
            {
                var isMet = SupEntryMet(supElem, approveLiterals, products);
                if (requireAll)
                {
                    if (!isMet)
                    {
                        return false;
                    }
                }
                else if (isMet)
                {
                    return true;
                }
            }
            foreach (var subDef in support.Elements.Where(e => e.SupportDefinition is not null))
            {
                var isMet = SupDefMet(subDef.SupportDefinition, entriesSeen, alwaysUseAny, approveLiterals, products);
                if (requireAll)
                {
                    if (!isMet)
                    {
                        return false;
                    }
                }
                else if (isMet)
                {
                    return true;
                }
            }
            return requireAll; // If all were required and this function hasn't exited yet, all are matched
        }

        private static bool SupEntryMet(PolicyPlusSupportEntry supportEntry, bool approveLiterals, List<PolicyPlusProduct> products)
        {
            if (supportEntry.Product is null)
            {
                return approveLiterals;
            }

            if (products.Contains(supportEntry.Product) && !supportEntry.RawSupportEntry.IsRange)
            {
                return true;
            }

            if (supportEntry.Product.Children is null || supportEntry.Product.Children.Count == 0)
            {
                return false; // Ranges only apply to parent products
            }

            var rangeMin = supportEntry.RawSupportEntry.MinVersion ?? 0;
            var rangeMax = supportEntry.RawSupportEntry.MaxVersion ?? supportEntry.Product.Children.Max(p => p.RawProduct.Version);
            for (var v = rangeMin; v <= rangeMax; v++)
            {
                var version = v; // To suppress compiler warnings about iteration variable in lambdas
                var subproduct = supportEntry.Product.Children.Find(p => p.RawProduct.Version == version);
                if (subproduct is null)
                {
                    continue;
                }

                if (products.Contains(subproduct))
                {
                    return true;
                }

                if (subproduct.Children?.Any(products.Contains) == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}