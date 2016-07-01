Imports System.IO
Public Class AdmxBundle
    Private RawCategories As New List(Of AdmxCategory)
    Private RawProducts As New List(Of AdmxProduct)
    Private RawPolicies As New List(Of AdmxPolicy)
    Private RawSupport As New List(Of AdmxSupportDefinition)
    Private AdmxFiles As New List(Of AdmxFile)
    Private AdmlFiles As New Dictionary(Of AdmxFile, AdmlFile)
    Public Categories As New List(Of PolicyPlusCategory)
    Public Sub LoadFolder(Path As String, LanguageCode As String)
        For Each file In Directory.EnumerateFiles(Path)
            If file.ToLowerInvariant.EndsWith(".admx") Then AddSingleAdmx(file, LanguageCode)
        Next
        BuildStructures()
    End Sub
    Public Sub LoadFile(Path As String, LanguageCode As String)
        AddSingleAdmx(Path, LanguageCode)
        BuildStructures()
    End Sub
    Private Sub AddSingleAdmx(AdmxPath As String, LanguageCode As String)
        Dim admx = AdmxFile.Load(AdmxPath)
        Dim fileTitle = Path.GetFileName(AdmxPath)
        Dim admlPath = Path.ChangeExtension(AdmxPath.Replace(fileTitle, LanguageCode & "\" & fileTitle), "adml")
        Dim adml = AdmlFile.Load(admlPath)
        RawCategories.AddRange(admx.Categories)
        RawProducts.AddRange(admx.Products)
        RawPolicies.AddRange(admx.Policies)
        RawSupport.AddRange(admx.SupportedOnDefinitions)
        AdmxFiles.Add(admx)
        AdmlFiles.Add(admx, adml)
    End Sub
    Private Sub BuildStructures() ' TODO: the rest of the structures
        Dim catIds As New Dictionary(Of String, PolicyPlusCategory)
        ' First pass: Build the structures without resolving references
        For Each rawCat In RawCategories
            Dim cat As New PolicyPlusCategory
            cat.DefinedIn = rawCat.DefinedIn
            cat.DisplayName = ResolveString(rawCat.DisplayCode, rawCat.DefinedIn)
            cat.UniqueID = QualifyName(rawCat.ID, rawCat.DefinedIn)
            cat.RawCategory = rawCat
            catIds.Add(cat.UniqueID, cat)
        Next
        ' Second pass: Resolve references and link structures
        For Each cat In catIds.Values
            If cat.RawCategory.ParentID <> "" Then
                Dim parentCatName = ResolveRef(cat.RawCategory.ParentID, cat.DefinedIn)
                If Not catIds.ContainsKey(parentCatName) Then Continue For
                Dim parentCat = catIds(parentCatName)
                parentCat.Children.Add(cat)
                cat.Parent = parentCat
            End If
        Next
        ' Third pass: Add top-level items to the final lists
        For Each cat In catIds.Values
            If cat.Parent Is Nothing Then Categories.Add(cat)
        Next
    End Sub
    Private Function ResolveString(DisplayCode As String, Admx As AdmxFile) As String
        If Not DisplayCode.StartsWith("$(string.") Then Return DisplayCode
        Dim stringId = DisplayCode.Substring(9, DisplayCode.Length - 10)
        Dim dict = AdmlFiles(Admx).StringTable
        If dict.ContainsKey(stringId) Then Return dict(stringId) Else Return DisplayCode
    End Function
    Private Function QualifyName(ID As String, Admx As AdmxFile) As String
        Return Admx.AdmxNamespace & ":" & ID
    End Function
    Private Function ResolveRef(Ref As String, Admx As AdmxFile) As String
        If Ref.Contains(":") Then
            Dim parts = Split(Ref, ":", 2)
            Dim srcNamespace = Admx.Prefixes(parts(0))
            Return srcNamespace & ":" & parts(1)
        Else
            Return QualifyName(Ref, Admx)
        End If
    End Function
End Class
