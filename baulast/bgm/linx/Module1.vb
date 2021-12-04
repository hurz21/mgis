Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Security.Cryptography


Module meineExtensionsIsnullorempty

    <Runtime.CompilerServices.Extension()>
    Public Function IsNothingOrEmpty(ByRef dt As DataTable) As Boolean
        Dim result As Boolean = (dt Is Nothing)
        If Not result Then result = dt.Rows.Count = 0
        Return result
    End Function
    <Runtime.CompilerServices.Extension()>
    Public Function IsNothingOrEmpty(ByRef text As String) As Boolean
        Return String.IsNullOrEmpty(text)

    End Function
    <Runtime.CompilerServices.Extension()>
    Public Function IsNothingOrEmpty(ByRef icoll As ICollection) As Boolean
        Return icoll Is Nothing Or icoll.Count = 0
    End Function

End Module
