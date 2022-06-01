Public Module SystemInfo

    Public Function HasGroupPolicyInfrastructure() As Boolean
        Dim windowsEdition As Integer
        PInvoke.GetProductInfo(6, 0, 0, 0, windowsEdition)
        Return {6, &H10, &H12, &H40, &H50, 8, &HC, &H27, &H25, &HA, &HE, &H29, &HF, &H26, &H3C,
            &H3E, &H3B, &H3D, &H2A, &H1E, &H20, &H1F, &H4D, &H4C, &H67, &H32, &H36, &H33,
            &H37, &H18, &H23, &H21, 9, &H19, &H3F, &H38, &H4F, 7, &HD, &H28, &H24, &H34, &H35,
            &H17, &H2E, &H14, &H2B, &H60, &H15, &H2C, &H5F, &H16, &H2D, 1, &H1C, &H11, &H1D,
            &H79, &H7A}.Contains(windowsEdition)
    End Function
End Module
