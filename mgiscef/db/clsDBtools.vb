Imports System.Data
Public Class clsDBtools
    'erwartet als Parameter einen String oder eine Nullable-Variable
    'funktioniert wie ToString, liefert aber "<NULL>", wenn der Parameter Nothing enthält
    Public Shared Function ToStringOblank(ByVal s As String) As String
        If s Is Nothing Then
            Return "<NULL>"
        End If
        If s Is "" Then
            Return " "
        End If
        Return s
    End Function
    Public Shared Function ToStringOrNull(ByVal s As String) As String
        If s Is Nothing Then
            Return "<NULL>"
        Else
            Return s
        End If
    End Function
    Public Shared Function ToStringOrNull(Of T As Structure)(ByVal nullvalue As Nullable(Of T)) As String
        If nullvalue.HasValue Then
            Return nullvalue.ToString
        Else
            Return "<NULL>"
        End If
    End Function
    'erwartet als Parameter eine Nullable-Variable
    'liefert den Wert des Parameters oder DBNullValue, wenn der Parameter Nothing enthält
    Public Shared Function ValueOrDBNull(Of t As Structure)(ByVal data As Nullable(Of t)) As Object
        If data.HasValue Then
            Return data.Value
        Else
            Return DBNull.Value
        End If
    End Function
    'erwartet als Parameter ein Feld eines Datensatzes (datarow!spaltenname)
    'liefert den Wert des Parameters oder Nothing, wenn der Parameter DBNull.Value enthält
    Public Shared Function StringOrNothing(ByVal obj As Object) As String
        If obj Is DBNull.Value Then
            Return Nothing
        Else
            Return obj.ToString
        End If
    End Function
    Public Shared Function ValueOrNothing(Of T As Structure)(ByVal obj As Object) As Nullable(Of T)
        If obj Is DBNull.Value Then
            Return Nothing
        Else
            Return CType(obj, T)
        End If
    End Function
    'erwartet als Parameter ein Feld eines Datensatzes (datarow!spaltenname)
    'liefert den Wert des Parameters oder Nothing, wenn der Parameter DBNull.Value enthält
    Public Shared Function fieldvalue(ByVal obj As Object) As String
        If obj Is DBNull.Value Then
            Return ""
        Else
            Return obj.ToString
        End If
    End Function
    ''erwartet als Parameter ein Feld eines Datensatzes (datarow!spaltenname)
    ''liefert den Wert des Parameters oder Nothing, wenn der Parameter DBNull.Value enthält
    Public Shared Function fieldvalueDate(ByVal obj As Object) As Date
        If obj Is DBNull.Value Then

            Return Nothing
        Else
            Return DirectCast(obj, Date)
        End If
    End Function
    'erwartet als Parameter ein Feld eines Datensatzes (datarow!spaltenname)
    'liefert den Wert des Parameters oder Nothing, wenn der Parameter DBNull.Value enthält
    Public Shared Function fieldvalue2(ByVal obj As Object) As String
        If obj Is Nothing Then

            Return ""
        End If
        If obj Is DBNull.Value Then

            Return ""
        Else
            Return obj.ToString
        End If
    End Function
    'erwartet als Parameter ein Feld eines Datensatzes (datarow!spaltenname)
    'liefert den Wert des Parameters oder Nothing, wenn der Parameter DBNull.Value enthÃ¤lt
    Public Shared Function feldWert(ByRef dt As DataTable, ByVal row%, ByVal col%) As Object
        Dim ret As Object
        If dt.Rows(row).Item(col) Is DBNull.Value Then
            If dt.Columns(col).DataType().Name.StartsWith("Int") Then
                ret = 0
                Return ret
            End If
        Else
            ret = dt.Rows(row).Item(col)
            Return ret
        End If
        Return ""
    End Function
    Public Shared Sub TabellenKopfausgeben(ByVal table As DataTable)
        My.Log.WriteEntry("in TabellenKopfausgeben --------------- nop")
        'For Each column As DataColumn In table.Columns
        '          'Console.WriteLine(column.ColumnName)
        'Next
    End Sub
End Class
