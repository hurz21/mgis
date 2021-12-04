Public Class clsLegende
    Friend Shared Function getLegende(aid As String, dbname As String) As String
        Try
            l(" MOD getLegende anfang")
            Dim SQL, result As String
            SQL = "select *   from  legenden " &
                    " where aid = " & aid &
                    " order by nr "
            l("sql " & SQL)
            result = dbgrabsimple(SQL, False, dbname)
            l(result)

            l(" MOD getLegende ende")
            Return result
        Catch ex As Exception
            l("Fehler in getLegende: " & ex.ToString())
            Return "Fehler in getLegende: " & ex.ToString()
        End Try
    End Function

    Friend Shared Function getgetschema4aid(v1 As String, v2 As String, v3 As String) As String
        Throw New NotImplementedException()
    End Function
End Class
