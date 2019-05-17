Imports System.IO
Public Class AdmxBundle
    Private SourceFiles As New Dictionary(Of AdmxFile, AdmlFile)
    Private Namespaces As New Dictionary(Of String, AdmxFile)
    ' Temporary lists from ADMX files that haven't been integrated yet
    Private RawCategories As New List(Of AdmxCategory)
    Private RawProducts As New List(Of AdmxProduct)
    Private RawPolicies As New List(Of AdmxPolicy)
    Private RawSupport As New List(Of AdmxSupportDefinition)
    ' Lists that include all items, even those that are children of others
    Public FlatCategories As New Dictionary(Of String, PolicyPlusCategory)
    Public FlatProducts As New Dictionary(Of String, PolicyPlusProduct)
    ' Lists of top-level items only
    Public Categories As New Dictionary(Of String, PolicyPlusCategory)
    Public Products As New Dictionary(Of String, PolicyPlusProduct)
    Public Policies As New Dictionary(Of String, PolicyPlusPolicy)
    Public SupportDefinitions As New Dictionary(Of String, PolicyPlusSupport)
    Public Function LoadFolder(Path As String, LanguageCode As String) As IEnumerable(Of AdmxLoadFailure)
        Dim fails As New List(Of AdmxLoadFailure)
        For Each file In Directory.EnumerateFiles(Path)
            If file.ToLowerInvariant.EndsWith(".admx") Then
                Dim fail = AddSingleAdmx(file, LanguageCode)
                If fail IsNot Nothing Then fails.Add(fail)
            End If
        Next
        BuildStructures()
        Return fails
    End Function
    Public Function LoadFile(Path As String, LanguageCode As String) As IEnumerable(Of AdmxLoadFailure)
        Dim fail = AddSingleAdmx(Path, LanguageCode)
        BuildStructures()
        Return If(fail Is Nothing, {}, {fail})
    End Function
    Private Function AddSingleAdmx(AdmxPath As String, LanguageCode As String) As AdmxLoadFailure
        ' Load ADMX file
        Dim admx As AdmxFile, adml As AdmlFile
        Try
            admx = AdmxFile.Load(AdmxPath)
        Catch ex As Xml.XmlException
            Return New AdmxLoadFailure(AdmxLoadFailType.BadAdmxParse, AdmxPath, ex.Message)
        Catch ex As Exception
            Return New AdmxLoadFailure(AdmxLoadFailType.BadAdmx, AdmxPath, ex.Message)
        End Try
        If Namespaces.ContainsKey(admx.AdmxNamespace) Then Return New AdmxLoadFailure(AdmxLoadFailType.DuplicateNamespace, AdmxPath, admx.AdmxNamespace)
        ' Find the ADML file
        Dim fileTitle = Path.GetFileName(AdmxPath)
        Dim admlPath = Path.ChangeExtension(AdmxPath.Replace(fileTitle, LanguageCode & "\" & fileTitle), "adml")
        If Not File.Exists(admlPath) Then
            Dim language = LanguageCode.Split("-"c)(0)
            For Each langSubdir In Directory.EnumerateDirectories(Path.GetDirectoryName(AdmxPath))
                Dim langSubdirTitle = Path.GetFileName(langSubdir)
                If langSubdirTitle.Split("-"c)(0) = language Then
                    Dim similarLanguagePath = Path.ChangeExtension(AdmxPath.Replace(fileTitle, langSubdirTitle & "\" & fileTitle), "adml")
                    If File.Exists(similarLanguagePath) Then
                        admlPath = similarLanguagePath
                        Exit For
                    End If
                End If
            Next
        End If
        If Not File.Exists(admlPath) Then admlPath = Path.ChangeExtension(AdmxPath.Replace(fileTitle, "en-US\" & fileTitle), "adml")
        If Not File.Exists(admlPath) Then Return New AdmxLoadFailure(AdmxLoadFailType.NoAdml, AdmxPath)
        ' Load the ADML
        Try
            adml = AdmlFile.Load(admlPath)
        Catch ex As Xml.XmlException
            Return New AdmxLoadFailure(AdmxLoadFailType.BadAdmlParse, AdmxPath, ex.Message)
        Catch ex As Exception
            Return New AdmxLoadFailure(AdmxLoadFailType.BadAdml, AdmxPath, ex.Message)
        End Try
        ' Stage the raw ADMX info for BuildStructures
        RawCategories.AddRange(admx.Categories)
        RawProducts.AddRange(admx.Products)
        RawPolicies.AddRange(admx.Policies)
        RawSupport.AddRange(admx.SupportedOnDefinitions)
        SourceFiles.Add(admx, adml)
        Namespaces.Add(admx.AdmxNamespace, admx)
        Return Nothing
    End Function
    Private Sub BuildStructures()
        Dim catIds As New Dictionary(Of String, PolicyPlusCategory)
        Dim productIds As New Dictionary(Of String, PolicyPlusProduct)
        Dim supIds As New Dictionary(Of String, PolicyPlusSupport)
        Dim polIds As New Dictionary(Of String, PolicyPlusPolicy)
        Dim findCatById = Function(UID As String) FindInTempOrFlat(UID, catIds, FlatCategories)
        Dim findSupById = Function(UID As String) FindInTempOrFlat(UID, supIds, SupportDefinitions)
        Dim findProductById = Function(UID As String) FindInTempOrFlat(UID, productIds, FlatProducts)
        ' First pass: Build the structures without resolving references
        For Each rawCat In RawCategories
            Dim cat As New PolicyPlusCategory
            cat.DisplayName = ResolveString(rawCat.DisplayCode, rawCat.DefinedIn)
            cat.DisplayExplanation = ResolveString(rawCat.ExplainCode, rawCat.DefinedIn)
            cat.UniqueID = QualifyName(rawCat.ID, rawCat.DefinedIn)
            cat.RawCategory = rawCat
            catIds.Add(cat.UniqueID, cat)
        Next
        For Each rawProduct In RawProducts
            Dim product As New PolicyPlusProduct
            product.DisplayName = ResolveString(rawProduct.DisplayCode, rawProduct.DefinedIn)
            product.UniqueID = QualifyName(rawProduct.ID, rawProduct.DefinedIn)
            product.RawProduct = rawProduct
            productIds.Add(product.UniqueID, product)
        Next
        For Each rawSup In RawSupport
            Dim sup As New PolicyPlusSupport
            sup.DisplayName = ResolveString(rawSup.DisplayCode, rawSup.DefinedIn)
            sup.UniqueID = QualifyName(rawSup.ID, rawSup.DefinedIn)
            If rawSup.Entries IsNot Nothing Then
                For Each rawSupEntry In rawSup.Entries
                    Dim supEntry As New PolicyPlusSupportEntry
                    supEntry.RawSupportEntry = rawSupEntry
                    sup.Elements.Add(supEntry)
                Next
            End If
            sup.RawSupport = rawSup
            supIds.Add(sup.UniqueID, sup)
        Next
        For Each rawPol In RawPolicies
            Dim pol As New PolicyPlusPolicy
            pol.DisplayExplanation = ResolveString(rawPol.ExplainCode, rawPol.DefinedIn)
            pol.DisplayName = ResolveString(rawPol.DisplayCode, rawPol.DefinedIn)
            If rawPol.PresentationID <> "" Then pol.Presentation = ResolvePresentation(rawPol.PresentationID, rawPol.DefinedIn)
            pol.UniqueID = QualifyName(rawPol.ID, rawPol.DefinedIn)
            pol.RawPolicy = rawPol
            polIds.Add(pol.UniqueID, pol)
        Next
        ' Second pass: Resolve references and link structures
        For Each cat In catIds.Values
            If cat.RawCategory.ParentID <> "" Then
                Dim parentCatName = ResolveRef(cat.RawCategory.ParentID, cat.RawCategory.DefinedIn)
                Dim parentCat = findCatById(parentCatName)
                If parentCat Is Nothing Then Continue For ' In case the parent category doesn't exist
                parentCat.Children.Add(cat)
                cat.Parent = parentCat
            End If
        Next
        For Each product In productIds.Values
            If product.RawProduct.Parent IsNot Nothing Then
                Dim parentProductId = QualifyName(product.RawProduct.Parent.ID, product.RawProduct.DefinedIn) ' Child products can't be defined in other files
                Dim parentProduct = findProductById(parentProductId)
                parentProduct.Children.Add(product)
                product.Parent = parentProduct
            End If
        Next
        For Each sup In supIds.Values
            For Each supEntry In sup.Elements
                Dim targetId = ResolveRef(supEntry.RawSupportEntry.ProductID, sup.RawSupport.DefinedIn) ' Support or product
                supEntry.Product = findProductById(targetId)
                If supEntry.Product Is Nothing Then supEntry.SupportDefinition = findSupById(targetId)
            Next
        Next
        For Each pol In polIds.Values
            Dim catId = ResolveRef(pol.RawPolicy.CategoryID, pol.RawPolicy.DefinedIn)
            Dim ownerCat = findCatById(catId)
            If ownerCat IsNot Nothing Then
                ownerCat.Policies.Add(pol)
                pol.Category = ownerCat
            End If
            Dim supportId = ResolveRef(pol.RawPolicy.SupportedCode, pol.RawPolicy.DefinedIn)
            pol.SupportedOn = findSupById(supportId)
        Next
        ' Third pass: Add items to the final lists
        For Each cat In catIds
            FlatCategories.Add(cat.Key, cat.Value)
            If cat.Value.Parent Is Nothing Then Categories.Add(cat.Key, cat.Value)
        Next
        For Each product In productIds
            FlatProducts.Add(product.Key, product.Value)
            If product.Value.Parent Is Nothing Then Products.Add(product.Key, product.Value)
        Next
        For Each pol In polIds
            Policies.Add(pol.Key, pol.Value)
        Next
        For Each sup In supIds
            SupportDefinitions.Add(sup.Key, sup.Value)
        Next
        ' Purge the temporary partially-constructed items
        RawCategories.Clear()
        RawProducts.Clear()
        RawSupport.Clear()
        RawPolicies.Clear()
    End Sub
    Private Function FindInTempOrFlat(Of T)(UniqueID As String, TempDict As Dictionary(Of String, T), FlatDict As Dictionary(Of String, T)) As T
        ' Get the best available structure for an ID
        If TempDict.ContainsKey(UniqueID) Then
            Return TempDict(UniqueID)
        ElseIf FlatDict IsNot Nothing AndAlso FlatDict.ContainsKey(UniqueID) Then
            Return FlatDict(UniqueID)
        Else
            Return Nothing
        End If
    End Function
    Public Function ResolveString(DisplayCode As String, Admx As AdmxFile) As String
        ' Find a localized string from a display code
        If DisplayCode = "" Then Return ""
        If Not DisplayCode.StartsWith("$(string.") Then Return DisplayCode
        Dim stringId = DisplayCode.Substring(9, DisplayCode.Length - 10)
        Dim dict = SourceFiles(Admx).StringTable
        If dict.ContainsKey(stringId) Then Return dict(stringId) Else Return DisplayCode
    End Function
    Public Function ResolvePresentation(DisplayCode As String, Admx As AdmxFile) As Presentation
        ' Find a presentation from a code
        If Not DisplayCode.StartsWith("$(presentation.") Then Return Nothing
        Dim presId = DisplayCode.Substring(15, DisplayCode.Length - 16)
        Dim dict = SourceFiles(Admx).PresentationTable
        If dict.ContainsKey(presId) Then Return dict(presId) Else Return Nothing
    End Function
    Private Function QualifyName(ID As String, Admx As AdmxFile) As String
        Return Admx.AdmxNamespace & ":" & ID
    End Function
    Private Function ResolveRef(Ref As String, Admx As AdmxFile) As String
        ' Get a fully qualified name from a code and the current scope
        If Ref.Contains(":") Then
            Dim parts = Split(Ref, ":", 2)
            If Admx.Prefixes.ContainsKey(parts(0)) Then
                Dim srcNamespace = Admx.Prefixes(parts(0))
                Return srcNamespace & ":" & parts(1)
            Else
                Return Ref ' Assume a literal
            End If
        Else
            Return QualifyName(Ref, Admx)
        End If
    End Function
    Public ReadOnly Property Sources As IReadOnlyDictionary(Of AdmxFile, AdmlFile)
        Get
            Return SourceFiles
        End Get
    End Property
End Class
Public Enum AdmxLoadFailType
    BadAdmxParse
    BadAdmx
    NoAdml
    BadAdmlParse
    BadAdml
    DuplicateNamespace
End Enum
Public Class AdmxLoadFailure
    Public FailType As AdmxLoadFailType
    Public AdmxPath As String
    Public Info As String
    Public Sub New(FailType As AdmxLoadFailType, AdmxPath As String, Info As String)
        Me.FailType = FailType
        Me.AdmxPath = AdmxPath
        Me.Info = Info
    End Sub
    Public Sub New(FailType As AdmxLoadFailType, AdmxPath As String)
        MyClass.New(FailType, AdmxPath, "")
    End Sub
    Public Overrides Function ToString() As String
        Dim failMsg = "Couldn't load " & AdmxPath & ": " & GetFailMessage(FailType, Info)
        If Not failMsg.EndsWith(".") Then failMsg &= "."
        Return failMsg
    End Function
    Private Shared Function GetFailMessage(FailType As AdmxLoadFailType, Info As String) As String
        Select Case FailType
            Case AdmxLoadFailType.BadAdmxParse
                Return "The ADMX XML couldn't be parsed: " & Info
            Case AdmxLoadFailType.BadAdmx
                Return "The ADMX is invalid: " & Info
            Case AdmxLoadFailType.NoAdml
                Return "The corresponding ADML is missing"
            Case AdmxLoadFailType.BadAdmlParse
                Return "The ADML XML couldn't be parsed: " & Info
            Case AdmxLoadFailType.BadAdml
                Return "The ADML is invalid: " & Info
            Case AdmxLoadFailType.DuplicateNamespace
                Return "The " & Info & " namespace is already owned by a different ADMX file"
        End Select
        Return If(Info = "", "An unknown error occurred", Info)
    End Function
End Class