using System;
using System.ComponentModel;
using System.Diagnostics;

namespace PolicyPlus.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DetailAdmx m_DetailAdmx;

            public DetailAdmx DetailAdmx
            {
                [DebuggerHidden]
                get
                {
                    m_DetailAdmx = Create__Instance__(m_DetailAdmx);
                    return m_DetailAdmx;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_DetailAdmx))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DetailAdmx);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public DetailCategory m_DetailCategory;

            public DetailCategory DetailCategory
            {
                [DebuggerHidden]
                get
                {
                    m_DetailCategory = Create__Instance__(m_DetailCategory);
                    return m_DetailCategory;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_DetailCategory))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DetailCategory);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public DetailPolicy m_DetailPolicy;

            public DetailPolicy DetailPolicy
            {
                [DebuggerHidden]
                get
                {
                    m_DetailPolicy = Create__Instance__(m_DetailPolicy);
                    return m_DetailPolicy;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_DetailPolicy))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DetailPolicy);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public DetailProduct m_DetailProduct;

            public DetailProduct DetailProduct
            {
                [DebuggerHidden]
                get
                {
                    m_DetailProduct = Create__Instance__(m_DetailProduct);
                    return m_DetailProduct;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_DetailProduct))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DetailProduct);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public DetailSupport m_DetailSupport;

            public DetailSupport DetailSupport
            {
                [DebuggerHidden]
                get
                {
                    m_DetailSupport = Create__Instance__(m_DetailSupport);
                    return m_DetailSupport;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_DetailSupport))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DetailSupport);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public DownloadAdmx m_DownloadAdmx;

            public DownloadAdmx DownloadAdmx
            {
                [DebuggerHidden]
                get
                {
                    m_DownloadAdmx = Create__Instance__(m_DownloadAdmx);
                    return m_DownloadAdmx;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_DownloadAdmx))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DownloadAdmx);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public EditPol m_EditPol;

            public EditPol EditPol
            {
                [DebuggerHidden]
                get
                {
                    m_EditPol = Create__Instance__(m_EditPol);
                    return m_EditPol;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_EditPol))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_EditPol);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public EditPolDelete m_EditPolDelete;

            public EditPolDelete EditPolDelete
            {
                [DebuggerHidden]
                get
                {
                    m_EditPolDelete = Create__Instance__(m_EditPolDelete);
                    return m_EditPolDelete;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_EditPolDelete))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_EditPolDelete);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public EditPolKey m_EditPolKey;

            public EditPolKey EditPolKey
            {
                [DebuggerHidden]
                get
                {
                    m_EditPolKey = Create__Instance__(m_EditPolKey);
                    return m_EditPolKey;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_EditPolKey))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_EditPolKey);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public EditPolMultiStringData m_EditPolMultiStringData;

            public EditPolMultiStringData EditPolMultiStringData
            {
                [DebuggerHidden]
                get
                {
                    m_EditPolMultiStringData = Create__Instance__(m_EditPolMultiStringData);
                    return m_EditPolMultiStringData;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_EditPolMultiStringData))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_EditPolMultiStringData);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public EditPolNumericData m_EditPolNumericData;

            public EditPolNumericData EditPolNumericData
            {
                [DebuggerHidden]
                get
                {
                    m_EditPolNumericData = Create__Instance__(m_EditPolNumericData);
                    return m_EditPolNumericData;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_EditPolNumericData))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_EditPolNumericData);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public EditPolStringData m_EditPolStringData;

            public EditPolStringData EditPolStringData
            {
                [DebuggerHidden]
                get
                {
                    m_EditPolStringData = Create__Instance__(m_EditPolStringData);
                    return m_EditPolStringData;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_EditPolStringData))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_EditPolStringData);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public EditPolValue m_EditPolValue;

            public EditPolValue EditPolValue
            {
                [DebuggerHidden]
                get
                {
                    m_EditPolValue = Create__Instance__(m_EditPolValue);
                    return m_EditPolValue;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_EditPolValue))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_EditPolValue);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public EditSetting m_EditSetting;

            public EditSetting EditSetting
            {
                [DebuggerHidden]
                get
                {
                    m_EditSetting = Create__Instance__(m_EditSetting);
                    return m_EditSetting;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_EditSetting))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_EditSetting);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public ExportReg m_ExportReg;

            public ExportReg ExportReg
            {
                [DebuggerHidden]
                get
                {
                    m_ExportReg = Create__Instance__(m_ExportReg);
                    return m_ExportReg;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_ExportReg))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_ExportReg);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public FilterOptions m_FilterOptions;

            public FilterOptions FilterOptions
            {
                [DebuggerHidden]
                get
                {
                    m_FilterOptions = Create__Instance__(m_FilterOptions);
                    return m_FilterOptions;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_FilterOptions))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_FilterOptions);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public FindById m_FindById;

            public FindById FindById
            {
                [DebuggerHidden]
                get
                {
                    m_FindById = Create__Instance__(m_FindById);
                    return m_FindById;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_FindById))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_FindById);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public FindByRegistry m_FindByRegistry;

            public FindByRegistry FindByRegistry
            {
                [DebuggerHidden]
                get
                {
                    m_FindByRegistry = Create__Instance__(m_FindByRegistry);
                    return m_FindByRegistry;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_FindByRegistry))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_FindByRegistry);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public FindByText m_FindByText;

            public FindByText FindByText
            {
                [DebuggerHidden]
                get
                {
                    m_FindByText = Create__Instance__(m_FindByText);
                    return m_FindByText;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_FindByText))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_FindByText);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public FindResults m_FindResults;

            public FindResults FindResults
            {
                [DebuggerHidden]
                get
                {
                    m_FindResults = Create__Instance__(m_FindResults);
                    return m_FindResults;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_FindResults))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_FindResults);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public ImportReg m_ImportReg;

            public ImportReg ImportReg
            {
                [DebuggerHidden]
                get
                {
                    m_ImportReg = Create__Instance__(m_ImportReg);
                    return m_ImportReg;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_ImportReg))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_ImportReg);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public ImportSpol m_ImportSpol;

            public ImportSpol ImportSpol
            {
                [DebuggerHidden]
                get
                {
                    m_ImportSpol = Create__Instance__(m_ImportSpol);
                    return m_ImportSpol;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_ImportSpol))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_ImportSpol);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public InspectPolicyElements m_InspectPolicyElements;

            public InspectPolicyElements InspectPolicyElements
            {
                [DebuggerHidden]
                get
                {
                    m_InspectPolicyElements = Create__Instance__(m_InspectPolicyElements);
                    return m_InspectPolicyElements;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_InspectPolicyElements))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_InspectPolicyElements);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public InspectSpolFragment m_InspectSpolFragment;

            public InspectSpolFragment InspectSpolFragment
            {
                [DebuggerHidden]
                get
                {
                    m_InspectSpolFragment = Create__Instance__(m_InspectSpolFragment);
                    return m_InspectSpolFragment;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_InspectSpolFragment))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_InspectSpolFragment);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public LanguageOptions m_LanguageOptions;

            public LanguageOptions LanguageOptions
            {
                [DebuggerHidden]
                get
                {
                    m_LanguageOptions = Create__Instance__(m_LanguageOptions);
                    return m_LanguageOptions;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_LanguageOptions))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_LanguageOptions);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public ListEditor m_ListEditor;

            public ListEditor ListEditor
            {
                [DebuggerHidden]
                get
                {
                    m_ListEditor = Create__Instance__(m_ListEditor);
                    return m_ListEditor;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_ListEditor))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_ListEditor);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public LoadedAdmx m_LoadedAdmx;

            public LoadedAdmx LoadedAdmx
            {
                [DebuggerHidden]
                get
                {
                    m_LoadedAdmx = Create__Instance__(m_LoadedAdmx);
                    return m_LoadedAdmx;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_LoadedAdmx))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_LoadedAdmx);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public LoadedProducts m_LoadedProducts;

            public LoadedProducts LoadedProducts
            {
                [DebuggerHidden]
                get
                {
                    m_LoadedProducts = Create__Instance__(m_LoadedProducts);
                    return m_LoadedProducts;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_LoadedProducts))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_LoadedProducts);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public LoadedSupportDefinitions m_LoadedSupportDefinitions;

            public LoadedSupportDefinitions LoadedSupportDefinitions
            {
                [DebuggerHidden]
                get
                {
                    m_LoadedSupportDefinitions = Create__Instance__(m_LoadedSupportDefinitions);
                    return m_LoadedSupportDefinitions;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_LoadedSupportDefinitions))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_LoadedSupportDefinitions);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public Main m_Main;

            public Main Main
            {
                [DebuggerHidden]
                get
                {
                    m_Main = Create__Instance__(m_Main);
                    return m_Main;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Main))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Main);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public OpenAdmxFolder m_OpenAdmxFolder;

            public OpenAdmxFolder OpenAdmxFolder
            {
                [DebuggerHidden]
                get
                {
                    m_OpenAdmxFolder = Create__Instance__(m_OpenAdmxFolder);
                    return m_OpenAdmxFolder;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_OpenAdmxFolder))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_OpenAdmxFolder);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public OpenPol m_OpenPol;

            public OpenPol OpenPol
            {
                [DebuggerHidden]
                get
                {
                    m_OpenPol = Create__Instance__(m_OpenPol);
                    return m_OpenPol;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_OpenPol))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_OpenPol);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public OpenSection m_OpenSection;

            public OpenSection OpenSection
            {
                [DebuggerHidden]
                get
                {
                    m_OpenSection = Create__Instance__(m_OpenSection);
                    return m_OpenSection;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_OpenSection))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_OpenSection);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public OpenUserGpo m_OpenUserGpo;

            public OpenUserGpo OpenUserGpo
            {
                [DebuggerHidden]
                get
                {
                    m_OpenUserGpo = Create__Instance__(m_OpenUserGpo);
                    return m_OpenUserGpo;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_OpenUserGpo))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_OpenUserGpo);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public OpenUserRegistry m_OpenUserRegistry;

            public OpenUserRegistry OpenUserRegistry
            {
                [DebuggerHidden]
                get
                {
                    m_OpenUserRegistry = Create__Instance__(m_OpenUserRegistry);
                    return m_OpenUserRegistry;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_OpenUserRegistry))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_OpenUserRegistry);
                }
            }

        }


    }
}