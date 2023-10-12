using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic;

namespace PolicyPlus
{
    public class AdmxBundle
    {
        private Dictionary<AdmxFile, AdmlFile> SourceFiles = new Dictionary<AdmxFile, AdmlFile>();
        private Dictionary<string, AdmxFile> Namespaces = new Dictionary<string, AdmxFile>();
        // Temporary lists from ADMX files that haven't been integrated yet
        private List<AdmxCategory> RawCategories = new List<AdmxCategory>();
        private List<AdmxProduct> RawProducts = new List<AdmxProduct>();
        private List<AdmxPolicy> RawPolicies = new List<AdmxPolicy>();
        private List<AdmxSupportDefinition> RawSupport = new List<AdmxSupportDefinition>();
        // Lists that include all items, even those that are children of others
        public Dictionary<string, PolicyPlusCategory> FlatCategories = new Dictionary<string, PolicyPlusCategory>();
        public Dictionary<string, PolicyPlusProduct> FlatProducts = new Dictionary<string, PolicyPlusProduct>();
        // Lists of top-level items only
        public Dictionary<string, PolicyPlusCategory> Categories = new Dictionary<string, PolicyPlusCategory>();
        public Dictionary<string, PolicyPlusProduct> Products = new Dictionary<string, PolicyPlusProduct>();
        public Dictionary<string, PolicyPlusPolicy> Policies = new Dictionary<string, PolicyPlusPolicy>();
        public Dictionary<string, PolicyPlusSupport> SupportDefinitions = new Dictionary<string, PolicyPlusSupport>();
        public IEnumerable<AdmxLoadFailure> LoadFolder(string Path, string LanguageCode)
        {
            var fails = new List<AdmxLoadFailure>();
            foreach (var file in Directory.EnumerateFiles(Path))
            {
                if (file.ToLowerInvariant().EndsWith(".admx"))
                {
                    var fail = AddSingleAdmx(file, LanguageCode);
                    if (fail is not null)
                        fails.Add(fail);
                }
            }
            BuildStructures();
            return fails;
        }
        public IEnumerable<AdmxLoadFailure> LoadFile(string Path, string LanguageCode)
        {
            var fail = AddSingleAdmx(Path, LanguageCode);
            BuildStructures();
            return fail is null ? Array.Empty<AdmxLoadFailure>() : (new[] { fail });
        }
        private AdmxLoadFailure AddSingleAdmx(string AdmxPath, string LanguageCode)
        {
            // Load ADMX file
            AdmxFile admx;
            AdmlFile adml;
            try
            {
                admx = AdmxFile.Load(AdmxPath);
            }
            catch (System.Xml.XmlException ex)
            {
                return new AdmxLoadFailure(AdmxLoadFailType.BadAdmxParse, AdmxPath, ex.Message);
            }
            catch (Exception ex)
            {
                return new AdmxLoadFailure(AdmxLoadFailType.BadAdmx, AdmxPath, ex.Message);
            }
            if (Namespaces.ContainsKey(admx.AdmxNamespace))
                return new AdmxLoadFailure(AdmxLoadFailType.DuplicateNamespace, AdmxPath, admx.AdmxNamespace);
            // Find the ADML file
            string fileTitle = Path.GetFileName(AdmxPath);
            string admlPath = Path.ChangeExtension(AdmxPath.Replace(fileTitle, LanguageCode + @"\" + fileTitle), "adml");
            if (!File.Exists(admlPath))
            {
                string language = LanguageCode.Split('-')[0];
                foreach (var langSubdir in Directory.EnumerateDirectories(Path.GetDirectoryName(AdmxPath)))
                {
                    string langSubdirTitle = Path.GetFileName(langSubdir);
                    if ((langSubdirTitle.Split('-')[0] ?? "") == (language ?? ""))
                    {
                        string similarLanguagePath = Path.ChangeExtension(AdmxPath.Replace(fileTitle, langSubdirTitle + @"\" + fileTitle), "adml");
                        if (File.Exists(similarLanguagePath))
                        {
                            admlPath = similarLanguagePath;
                            break;
                        }
                    }
                }
            }
            if (!File.Exists(admlPath))
                admlPath = Path.ChangeExtension(AdmxPath.Replace(fileTitle, @"en-US\" + fileTitle), "adml");
            if (!File.Exists(admlPath))
                return new AdmxLoadFailure(AdmxLoadFailType.NoAdml, AdmxPath);
            // Load the ADML
            try
            {
                adml = AdmlFile.Load(admlPath);
            }
            catch (System.Xml.XmlException ex)
            {
                return new AdmxLoadFailure(AdmxLoadFailType.BadAdmlParse, AdmxPath, ex.Message);
            }
            catch (Exception ex)
            {
                return new AdmxLoadFailure(AdmxLoadFailType.BadAdml, AdmxPath, ex.Message);
            }
            // Stage the raw ADMX info for BuildStructures
            RawCategories.AddRange(admx.Categories);
            RawProducts.AddRange(admx.Products);
            RawPolicies.AddRange(admx.Policies);
            RawSupport.AddRange(admx.SupportedOnDefinitions);
            SourceFiles.Add(admx, adml);
            Namespaces.Add(admx.AdmxNamespace, admx);
            return null;
        }
        private void BuildStructures()
        {
            var catIds = new Dictionary<string, PolicyPlusCategory>();
            var productIds = new Dictionary<string, PolicyPlusProduct>();
            var supIds = new Dictionary<string, PolicyPlusSupport>();
            var polIds = new Dictionary<string, PolicyPlusPolicy>();
            PolicyPlusCategory findCatById(string UID) => FindInTempOrFlat(UID, catIds, FlatCategories);
            PolicyPlusSupport findSupById(string UID) => FindInTempOrFlat(UID, supIds, SupportDefinitions);
            PolicyPlusProduct findProductById(string UID) => FindInTempOrFlat(UID, productIds, FlatProducts);
            // First pass: Build the structures without resolving references
            foreach (var rawCat in RawCategories)
            {
                var cat = new PolicyPlusCategory();
                cat.DisplayName = ResolveString(rawCat.DisplayCode, rawCat.DefinedIn);
                cat.DisplayExplanation = ResolveString(rawCat.ExplainCode, rawCat.DefinedIn);
                cat.UniqueID = QualifyName(rawCat.ID, rawCat.DefinedIn);
                cat.RawCategory = rawCat;
                catIds.Add(cat.UniqueID, cat);
            }
            foreach (var rawProduct in RawProducts)
            {
                var product = new PolicyPlusProduct();
                product.DisplayName = ResolveString(rawProduct.DisplayCode, rawProduct.DefinedIn);
                product.UniqueID = QualifyName(rawProduct.ID, rawProduct.DefinedIn);
                product.RawProduct = rawProduct;
                productIds.Add(product.UniqueID, product);
            }
            foreach (var rawSup in RawSupport)
            {
                var sup = new PolicyPlusSupport();
                sup.DisplayName = ResolveString(rawSup.DisplayCode, rawSup.DefinedIn);
                sup.UniqueID = QualifyName(rawSup.ID, rawSup.DefinedIn);
                if (rawSup.Entries is not null)
                {
                    foreach (var rawSupEntry in rawSup.Entries)
                    {
                        var supEntry = new PolicyPlusSupportEntry();
                        supEntry.RawSupportEntry = rawSupEntry;
                        sup.Elements.Add(supEntry);
                    }
                }
                sup.RawSupport = rawSup;
                supIds.Add(sup.UniqueID, sup);
            }
            foreach (var rawPol in RawPolicies)
            {
                var pol = new PolicyPlusPolicy();
                pol.DisplayExplanation = ResolveString(rawPol.ExplainCode, rawPol.DefinedIn);
                pol.DisplayName = ResolveString(rawPol.DisplayCode, rawPol.DefinedIn);
                if (!string.IsNullOrEmpty(rawPol.PresentationID))
                    pol.Presentation = ResolvePresentation(rawPol.PresentationID, rawPol.DefinedIn);
                pol.UniqueID = QualifyName(rawPol.ID, rawPol.DefinedIn);
                pol.RawPolicy = rawPol;
                polIds.Add(pol.UniqueID, pol);
            }
            // Second pass: Resolve references and link structures
            foreach (var cat in catIds.Values)
            {
                if (!string.IsNullOrEmpty(cat.RawCategory.ParentID))
                {
                    string parentCatName = ResolveRef(cat.RawCategory.ParentID, cat.RawCategory.DefinedIn);
                    var parentCat = findCatById(parentCatName);
                    if (parentCat is null)
                        continue; // In case the parent category doesn't exist
                    parentCat.Children.Add(cat);
                    cat.Parent = parentCat;
                }
            }
            foreach (var product in productIds.Values)
            {
                if (product.RawProduct.Parent is not null)
                {
                    string parentProductId = QualifyName(product.RawProduct.Parent.ID, product.RawProduct.DefinedIn); // Child products can't be defined in other files
                    var parentProduct = findProductById(parentProductId);
                    parentProduct.Children.Add(product);
                    product.Parent = parentProduct;
                }
            }
            foreach (var sup in supIds.Values)
            {
                foreach (var supEntry in sup.Elements)
                {
                    string targetId = ResolveRef(supEntry.RawSupportEntry.ProductID, sup.RawSupport.DefinedIn); // Support or product
                    supEntry.Product = findProductById(targetId);
                    if (supEntry.Product is null)
                        supEntry.SupportDefinition = findSupById(targetId);
                }
            }
            foreach (var pol in polIds.Values)
            {
                string catId = ResolveRef(pol.RawPolicy.CategoryID, pol.RawPolicy.DefinedIn);
                var ownerCat = findCatById(catId);
                if (ownerCat is not null)
                {
                    ownerCat.Policies.Add(pol);
                    pol.Category = ownerCat;
                }
                string supportId = ResolveRef(pol.RawPolicy.SupportedCode, pol.RawPolicy.DefinedIn);
                pol.SupportedOn = findSupById(supportId);
            }
            // Third pass: Add items to the final lists
            foreach (var cat in catIds)
            {
                FlatCategories.Add(cat.Key, cat.Value);
                if (cat.Value.Parent is null)
                    Categories.Add(cat.Key, cat.Value);
            }
            foreach (var product in productIds)
            {
                FlatProducts.Add(product.Key, product.Value);
                if (product.Value.Parent is null)
                    Products.Add(product.Key, product.Value);
            }
            foreach (var pol in polIds)
                Policies.Add(pol.Key, pol.Value);
            foreach (var sup in supIds)
                SupportDefinitions.Add(sup.Key, sup.Value);
            // Purge the temporary partially-constructed items
            RawCategories.Clear();
            RawProducts.Clear();
            RawSupport.Clear();
            RawPolicies.Clear();
        }
        private T FindInTempOrFlat<T>(string UniqueID, Dictionary<string, T> TempDict, Dictionary<string, T> FlatDict)
        {
            // Get the best available structure for an ID
            if (TempDict.ContainsKey(UniqueID))
            {
                return TempDict[UniqueID];
            }
            else if (FlatDict is not null && FlatDict.ContainsKey(UniqueID))
            {
                return FlatDict[UniqueID];
            }
            else
            {
                return default;
            }
        }
        public string ResolveString(string DisplayCode, AdmxFile Admx)
        {
            // Find a localized string from a display code
            if (string.IsNullOrEmpty(DisplayCode))
                return "";
            if (!DisplayCode.StartsWith("$(string."))
                return DisplayCode;
            string stringId = DisplayCode.Substring(9, DisplayCode.Length - 10);
            var dict = SourceFiles[Admx].StringTable;
            if (dict.ContainsKey(stringId))
                return dict[stringId];
            else
                return DisplayCode;
        }
        public Presentation ResolvePresentation(string DisplayCode, AdmxFile Admx)
        {
            // Find a presentation from a code
            if (!DisplayCode.StartsWith("$(presentation."))
                return null;
            string presId = DisplayCode.Substring(15, DisplayCode.Length - 16);
            var dict = SourceFiles[Admx].PresentationTable;
            if (dict.ContainsKey(presId))
                return dict[presId];
            else
                return null;
        }
        private string QualifyName(string ID, AdmxFile Admx)
        {
            return Admx.AdmxNamespace + ":" + ID;
        }
        private string ResolveRef(string Ref, AdmxFile Admx)
        {
            // Get a fully qualified name from a code and the current scope
            if (Ref.Contains(":"))
            {
                string[] parts = Strings.Split(Ref, ":", 2);
                if (Admx.Prefixes.ContainsKey(parts[0]))
                {
                    string srcNamespace = Admx.Prefixes[parts[0]];
                    return srcNamespace + ":" + parts[1];
                }
                else
                {
                    return Ref;
                } // Assume a literal
            }
            else
            {
                return QualifyName(Ref, Admx);
            }
        }
        public IReadOnlyDictionary<AdmxFile, AdmlFile> Sources
        {
            get
            {
                return SourceFiles;
            }
        }
    }

    public enum AdmxLoadFailType
    {
        BadAdmxParse,
        BadAdmx,
        NoAdml,
        BadAdmlParse,
        BadAdml,
        DuplicateNamespace
    }

    public class AdmxLoadFailure
    {
        public AdmxLoadFailType FailType;
        public string AdmxPath;
        public string Info;
        public AdmxLoadFailure(AdmxLoadFailType FailType, string AdmxPath, string Info)
        {
            this.FailType = FailType;
            this.AdmxPath = AdmxPath;
            this.Info = Info;
        }
        public AdmxLoadFailure(AdmxLoadFailType FailType, string AdmxPath) : this(FailType, AdmxPath, "")
        {
        }
        public override string ToString()
        {
            string failMsg = "Couldn't load " + AdmxPath + ": " + GetFailMessage(FailType, Info);
            if (!failMsg.EndsWith("."))
                failMsg += ".";
            return failMsg;
        }
        private static string GetFailMessage(AdmxLoadFailType FailType, string Info)
        {
            switch (FailType)
            {
                case AdmxLoadFailType.BadAdmxParse:
                    {
                        return "The ADMX XML couldn't be parsed: " + Info;
                    }
                case AdmxLoadFailType.BadAdmx:
                    {
                        return "The ADMX is invalid: " + Info;
                    }
                case AdmxLoadFailType.NoAdml:
                    {
                        return "The corresponding ADML is missing";
                    }
                case AdmxLoadFailType.BadAdmlParse:
                    {
                        return "The ADML XML couldn't be parsed: " + Info;
                    }
                case AdmxLoadFailType.BadAdml:
                    {
                        return "The ADML is invalid: " + Info;
                    }
                case AdmxLoadFailType.DuplicateNamespace:
                    {
                        return "The " + Info + " namespace is already owned by a different ADMX file";
                    }
            }
            return string.IsNullOrEmpty(Info) ? "An unknown error occurred" : Info;
        }
    }
}