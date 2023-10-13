using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PolicyPlus.csharp.Elements;

namespace PolicyPlus.csharp.Models.Sources.Admx
{
    public class AdmxBundle
    {
        private readonly Dictionary<AdmxFile, AdmlFile> _sourceFiles = new();
        private readonly Dictionary<string, AdmxFile> _namespaces = new();

        // Temporary lists from ADMX files that haven't been integrated yet
        private readonly List<AdmxCategory> _rawCategories = new();

        private readonly List<AdmxProduct> _rawProducts = new();
        private readonly List<AdmxPolicy> _rawPolicies = new();
        private readonly List<AdmxSupportDefinition> _rawSupport = new();

        // Lists that include all items, even those that are children of others
        public Dictionary<string, PolicyPlusCategory> FlatCategories { get; } = new();

        public Dictionary<string, PolicyPlusProduct> FlatProducts { get; } = new();

        // Lists of top-level items only
        public Dictionary<string, PolicyPlusCategory> Categories { get; } = new();

        public Dictionary<string, PolicyPlusProduct> Products { get; } = new();
        public Dictionary<string, PolicyPlusPolicy> Policies { get; } = new();
        public Dictionary<string, PolicyPlusSupport> SupportDefinitions { get; } = new();

        public IEnumerable<AdmxLoadFailure> LoadFolder(string path, string languageCode)
        {
            var fails = new List<AdmxLoadFailure>();
            foreach (var file in Directory.EnumerateFiles(path))
            {
                if (!file.EndsWith(".admx", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (AddSingleAdmx(file, languageCode) is { } fail)
                {
                    fails.Add(fail);
                }
            }
            BuildStructures();
            return fails;
        }

        public IEnumerable<AdmxLoadFailure> LoadFile(string path, string languageCode)
        {
            var fail = AddSingleAdmx(path, languageCode);
            BuildStructures();
            return fail is null ? Array.Empty<AdmxLoadFailure>() : (new[] { fail });
        }

        private AdmxLoadFailure? AddSingleAdmx(string admxPath, string languageCode)
        {
            // Load ADMX file
            AdmxFile admx;
            AdmlFile adml;
            try
            {
                admx = AdmxFile.Load(admxPath);
            }
            catch (System.Xml.XmlException ex)
            {
                return new AdmxLoadFailure(AdmxLoadFailType.BadAdmxParse, admxPath, ex.Message);
            }
            catch (Exception ex)
            {
                return new AdmxLoadFailure(AdmxLoadFailType.BadAdmx, admxPath, ex.Message);
            }
            if (_namespaces.ContainsKey(admx.AdmxNamespace))
            {
                return new AdmxLoadFailure(AdmxLoadFailType.DuplicateNamespace, admxPath, admx.AdmxNamespace);
            }
            // Find the ADML file
            var fileTitle = Path.GetFileName(admxPath);
            var admlPath = Path.ChangeExtension(admxPath.Replace(fileTitle, languageCode + @"\" + fileTitle), "adml");
            if (!File.Exists(admlPath))
            {
                var language = languageCode.Split('-')[0];
                foreach (var langSubdir in Directory.EnumerateDirectories(Path.GetDirectoryName(admxPath)!))
                {
                    var langSubdirTitle = Path.GetFileName(langSubdir);
                    if ((langSubdirTitle.Split('-')[0] ?? string.Empty) != (language ?? string.Empty))
                    {
                        continue;
                    }

                    var similarLanguagePath = Path.ChangeExtension(admxPath.Replace(fileTitle, langSubdirTitle + @"\" + fileTitle), "adml");
                    if (!File.Exists(similarLanguagePath))
                    {
                        continue;
                    }

                    admlPath = similarLanguagePath;
                    break;
                }
            }
            if (!File.Exists(admlPath))
            {
                admlPath = Path.ChangeExtension(admxPath.Replace(fileTitle, @"en-US\" + fileTitle), "adml");
            }

            if (!File.Exists(admlPath))
            {
                return new AdmxLoadFailure(AdmxLoadFailType.NoAdml, admxPath);
            }
            // Load the ADML
            try
            {
                adml = AdmlFile.Load(admlPath);
            }
            catch (System.Xml.XmlException ex)
            {
                return new AdmxLoadFailure(AdmxLoadFailType.BadAdmlParse, admxPath, ex.Message);
            }
            catch (Exception ex)
            {
                return new AdmxLoadFailure(AdmxLoadFailType.BadAdml, admxPath, ex.Message);
            }
            // Stage the raw ADMX info for BuildStructures
            _rawCategories.AddRange(admx.Categories);
            _rawProducts.AddRange(admx.Products);
            _rawPolicies.AddRange(admx.Policies);
            _rawSupport.AddRange(admx.SupportedOnDefinitions);
            _sourceFiles.Add(admx, adml);
            _namespaces.Add(admx.AdmxNamespace, admx);
            return null;
        }

        private void BuildStructures()
        {
            var supIds = new Dictionary<string, PolicyPlusSupport>();
            var polIds = new Dictionary<string, PolicyPlusPolicy>();
            // First pass: Build the structures without resolving references
            var catIds = _rawCategories.Select(rawCat => new PolicyPlusCategory { DisplayName = ResolveString(rawCat.DisplayCode, rawCat.DefinedIn), DisplayExplanation = ResolveString(rawCat.ExplainCode, rawCat.DefinedIn), UniqueId = QualifyName(rawCat.Id, rawCat.DefinedIn), RawCategory = rawCat }).ToDictionary(cat => cat.UniqueId);

            var productIds = _rawProducts.Select(rawProduct => new PolicyPlusProduct { DisplayName = ResolveString(rawProduct.DisplayCode, rawProduct.DefinedIn), UniqueId = QualifyName(rawProduct.Id, rawProduct.DefinedIn), RawProduct = rawProduct }).ToDictionary(product => product.UniqueId);
            foreach (var rawSup in _rawSupport)
            {
                var sup = new PolicyPlusSupport
                {
                    DisplayName = ResolveString(rawSup.DisplayCode, rawSup.DefinedIn),
                    UniqueId = QualifyName(rawSup.Id, rawSup.DefinedIn)
                };
                if (rawSup.Entries is not null)
                {
                    sup.Elements.AddRange(rawSup.Entries.Select(rawSupEntry => new PolicyPlusSupportEntry
                    {
                        RawSupportEntry = rawSupEntry
                    }));
                }
                sup.RawSupport = rawSup;
                supIds.Add(sup.UniqueId, sup);
            }
            foreach (var rawPol in _rawPolicies)
            {
                var pol = new PolicyPlusPolicy
                {
                    DisplayExplanation = ResolveString(rawPol.ExplainCode, rawPol.DefinedIn),
                    DisplayName = ResolveString(rawPol.DisplayCode, rawPol.DefinedIn)
                };
                if (!string.IsNullOrEmpty(rawPol.PresentationId))
                {
                    pol.Presentation = ResolvePresentation(rawPol.PresentationId, rawPol.DefinedIn);
                }

                pol.UniqueId = QualifyName(rawPol.Id, rawPol.DefinedIn);
                pol.RawPolicy = rawPol;
                polIds.Add(pol.UniqueId, pol);
            }
            // Second pass: Resolve references and link structures
            foreach (var cat in catIds.Values)
            {
                if (string.IsNullOrEmpty(cat.RawCategory.ParentId))
                {
                    continue;
                }

                var parentCatName = ResolveRef(cat.RawCategory.ParentId, cat.RawCategory.DefinedIn);
                var parentCat = FindCatById(parentCatName, catIds);
                if (parentCat is null)
                {
                    continue; // In case the parent category doesn't exist
                }

                parentCat.Children.Add(cat);
                cat.Parent = parentCat;
            }
            foreach (var product in productIds.Values)
            {
                if (product.RawProduct.Parent is null)
                {
                    continue;
                }

                var parentProductId = QualifyName(product.RawProduct.Parent.Id, product.RawProduct.DefinedIn); // Child products can't be defined in other files
                var parentProduct = FindProductById(parentProductId, productIds);
                parentProduct.Children.Add(product);
                product.Parent = parentProduct;
            }
            foreach (var sup in supIds.Values)
            {
                foreach (var supEntry in sup.Elements)
                {
                    var targetId = ResolveRef(supEntry.RawSupportEntry.ProductId, sup.RawSupport.DefinedIn); // Support or product
                    supEntry.Product = FindProductById(targetId, productIds);
                    if (supEntry.Product is null)
                    {
                        supEntry.SupportDefinition = FindSupById(targetId, supIds);
                    }
                }
            }
            foreach (var pol in polIds.Values)
            {
                var catId = ResolveRef(pol.RawPolicy.CategoryId, pol.RawPolicy.DefinedIn);
                if (FindCatById(catId, catIds) is { } ownerCat)
                {
                    ownerCat.Policies.Add(pol);
                    pol.Category = ownerCat;
                }
                var supportId = ResolveRef(pol.RawPolicy.SupportedCode, pol.RawPolicy.DefinedIn);
                pol.SupportedOn = FindSupById(supportId, supIds);
            }
            // Third pass: Add items to the final lists
            foreach (var cat in catIds)
            {
                FlatCategories.Add(cat.Key, cat.Value);
                if (cat.Value.Parent is null)
                {
                    Categories.Add(cat.Key, cat.Value);
                }
            }
            foreach (var product in productIds)
            {
                FlatProducts.Add(product.Key, product.Value);
                if (product.Value.Parent is null)
                {
                    Products.Add(product.Key, product.Value);
                }
            }
            foreach (var pol in polIds)
            {
                Policies.Add(pol.Key, pol.Value);
            }

            foreach (var sup in supIds)
            {
                SupportDefinitions.Add(sup.Key, sup.Value);
            }
            // Purge the temporary partially-constructed items
            _rawCategories.Clear();
            _rawProducts.Clear();
            _rawSupport.Clear();
            _rawPolicies.Clear();
        }

        private PolicyPlusProduct? FindProductById(string uid, Dictionary<string, PolicyPlusProduct> productIds) => FindInTempOrFlat(uid, productIds, FlatProducts);

        private PolicyPlusSupport? FindSupById(string uid, Dictionary<string, PolicyPlusSupport> supIds) => FindInTempOrFlat(uid, supIds, SupportDefinitions);

        private PolicyPlusCategory? FindCatById(string uid, Dictionary<string, PolicyPlusCategory> catIds) => FindInTempOrFlat(uid, catIds, FlatCategories);

        private static T? FindInTempOrFlat<T>(string uniqueId, IReadOnlyDictionary<string, T> tempDict, Dictionary<string, T> flatDict)
        {
            // Get the best available structure for an ID
            if (tempDict.TryGetValue(uniqueId, out var flat))
            {
                return flat;
            }

            if (flatDict is not null && flatDict.TryGetValue(uniqueId, out var orFlat))
            {
                return orFlat;
            }
            return default;
        }

        public string ResolveString(string displayCode, AdmxFile admx)
        {
            // Find a localized string from a display code
            if (string.IsNullOrEmpty(displayCode))
            {
                return string.Empty;
            }

            if (!displayCode.StartsWith("$(string."))
            {
                return displayCode;
            }

            var stringId = displayCode.Substring(9, displayCode.Length - 10);
            var dict = _sourceFiles[admx].StringTable;
            return dict.TryGetValue(stringId, out var s) ? s : displayCode;
        }

        public Presentation? ResolvePresentation(string displayCode, AdmxFile admx)
        {
            // Find a presentation from a code
            if (!displayCode.StartsWith("$(presentation."))
            {
                return null;
            }

            var presId = displayCode.Substring(15, displayCode.Length - 16);
            var dict = _sourceFiles[admx].PresentationTable;
            return dict.TryGetValue(presId, out var presentation) ? presentation : null;
        }

        private static string QualifyName(string id, AdmxFile admx) => admx.AdmxNamespace + ":" + id;

        private static string ResolveRef(string @ref, AdmxFile admx)
        {
            // Get a fully qualified name from a code and the current scope
            if (!@ref.Contains(':'))
            {
                return QualifyName(@ref, admx);
            }

            var parts = @ref.Split(":", 2);
            if (!admx.Prefixes.ContainsKey(parts[0]))
            {
                return @ref;
            }

            var srcNamespace = admx.Prefixes[parts[0]];
            return srcNamespace + ":" + parts[1];

            // Assume a literal
        }

        public IReadOnlyDictionary<AdmxFile, AdmlFile> Sources => _sourceFiles;
    }
}