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
            My.Application.Log.WriteEntry("in TabellenKopfausgeben --------------- nop")
            'For Each column As DataColumn In table.Columns
            '          'Console.WriteLine(column.ColumnName)
            'Next
        End Sub
        Public Shared Function makeDBdatumsString(ByVal datum As Date, ByVal dbtyp As String) As String
            Dim datumstring$ = ""
            If datum.ToString Is Nothing Then
                Return ""
            End If
            If String.IsNullOrEmpty(dbtyp) Then
                Return ""
            End If
            If dbtyp = "mysql" Then
                datumstring = Format(Now, "yyyy-MM-dd HH:mm:ss")
            End If
            If dbtyp = "oracle" Then
                datumstring = " to_date('" & Now & "' ,'DD.MM.YYYY HH24:MI:SS') "
                'to_date('2010-12-25 10:17:49' ,'YYYY-MM-DD HH24:MI:SS')
            End If
            Return datumstring
        End Function
 
    
        Shared Sub nachricht(ByVal text$)
            My.Application.Log.WriteEntry("Fehler in clsdbtools.UNION_SQL_erzeugenInn: " & text)
        End Sub
        ''' <summary>
        ''' ergänzt in der datatalbe ein spalte mit dem angegebenen typ
        ''' </summary>
        ''' <param name="meinelokaleDT"></param>
        ''' <param name="Spaltenname"></param>
        ''' <param name="SpaltenTyp"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SpalteZuDatatableHinzufuegen(ByVal meinelokaleDT As DataTable, ByVal Spaltenname$, ByVal SpaltenTyp$) As Boolean '"System.Int16"
            Try
                For Each col As DataColumn In meinelokaleDT.Columns
                    If col.ColumnName = Spaltenname Then
                        GoTo NICHTNEUANLEGEN
                    End If
                Next
                meinelokaleDT.Columns.Add(Spaltenname, System.Type.GetType(SpaltenTyp))
                Return True
NICHTNEUANLEGEN:
                meinelokaleDT.Columns(Spaltenname).ReadOnly = False
                Return True
            Catch ex As Exception
                nachricht("Fehler in  SpalteHinzufuegen: " & ex.ToString)
                Return False
            End Try
        End Function

        Public Shared Sub SpalteInitialisieren(ByVal lokdt As DataTable, ByVal Spaltenname$, ByVal Wert%)
            For Each row As DataRow In lokdt.Rows
                row.Item(Spaltenname) = Wert
            Next
        End Sub

        Public Shared Function bildINstring(ByVal meineDT As DataTable, ByVal meinindex As Integer) As String
            Dim sb As New Text.StringBuilder
            Dim summe As String
            Dim izaehl As Integer = 0
            Try
                For Each zeile As DataRow In meineDT.Rows
                    sb.Append(zeile.Item(meinindex).ToString & ",")
                    izaehl += 1
                    If izaehl > 998 Then
                        Exit For
                    End If
                Next
                summe = sb.ToString
                summe = summe.Substring(0, summe.Length - 1) '?was soll das?
                sb = Nothing
                Return summe
            Catch ex As Exception
                nachricht("Fehler in bildINstring: " & ex.ToString)
                Return ""
            End Try
        End Function

        Public Shared Function bildINstringSpaltenname(ByVal meineDT As DataTable, ByVal spaltenname As String) As String
            Dim sb As New Text.StringBuilder
            Dim summe$
            Try
                For Each zeile As DataRow In meineDT.Rows
                    sb.Append(zeile.Item(spaltenname).ToString & ",")
                Next
                summe$ = sb.ToString
                summe = summe.Substring(0, summe.Length - 1)
                sb = Nothing
                Return summe
            Catch ex As Exception
                nachricht("Fehler in bildINstring: " & ex.ToString)
                Return ""
            End Try
        End Function
    End Class
