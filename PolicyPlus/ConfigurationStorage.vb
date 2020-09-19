Imports Microsoft.Win32
Public Class ConfigurationStorage
    Dim ConfigKey As RegistryKey
    Public Sub New(RegistryBase As RegistryHive, Subkey As String)
        Try
            ConfigKey = RegistryKey.OpenBaseKey(RegistryBase, RegistryView.Default).CreateSubKey(Subkey)
        Catch ex As Exception
            ' The key couldn't be created
        End Try
    End Sub
    Public Function GetValue(ValueName As String, DefaultValue As Object) As Object
        If ConfigKey IsNot Nothing Then Return ConfigKey.GetValue(ValueName, DefaultValue) Else Return DefaultValue
    End Function
    Public Sub SetValue(ValueName As String, Data As Object)
        If ConfigKey IsNot Nothing Then ConfigKey.SetValue(ValueName, Data)
    End Sub
    Public Function HasValue(ValueName As String) As Boolean
        Return ConfigKey IsNot Nothing AndAlso ConfigKey.GetValue(ValueName) IsNot Nothing
    End Function
End Class
