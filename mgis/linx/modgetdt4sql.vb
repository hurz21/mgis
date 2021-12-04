Imports System.Data

Module modgetdt4sql
    Public Function getDT4Query(sql As String, ByVal myneREC As IDB_grundfunktionen, ByRef hinweis As String) As DataTable
        'Dim dt As DataTable 
        Try
            l("getDT4Query---------------------- anfang")
            myneREC.mydb.SQL = sql
            nachricht("getDT4Query: " & vbCrLf & myneREC.mydb.SQL)
            hinweis = myneREC.getDataDT()
            nachricht("  hinweis: " & hinweis)
            If myneREC.dt.IsNothingOrEmpty Then
                If myneREC.mydb.SQL.ToLower.Trim.StartsWith("delete") Or
                    myneREC.mydb.SQL.ToLower.Trim.StartsWith("update") Then
                    'dt darf leer sein
                    If hinweis.ToLower.Contains("fehler") Then
                        ' Return -1
                    End If
                Else
                    l("Fatal Error ID " & "konnte nicht gefunden werden!" & myneREC.mydb.getDBinfo(""))
                End If
                Return myneREC.dt
            End If
            ' dt = myneREC.dt
            Return myneREC.dt
            l("getDT4Query---------------------- ende")
        Catch ex As Exception
            l("Fehler in getDT4Query: " & ex.ToString())
            Return Nothing
        End Try
    End Function

End Module
