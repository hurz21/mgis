Imports Npgsql

Public Class clsGetSql
    Public Shared Function getschema4aid(aid As String, tabnr As String, dbname As String) As String
        Try
            l(" MOD getgetschema4aid anfang")
            Dim SQL, result As String
            SQL = "select schema,tabelle,tab_id,tab_nr,linktabs,tab_titel,tabellen_anzeige from public.attributtabellen  where aid=" & aid &
                     " and tab_nr  = " & tabnr & ""
            l("sql " & SQL)
            result = dbgrabsimple(SQL, False, dbname)
            l(result)
            Return result
            l(" MOD getgetschema4aid ende")

        Catch ex As Exception
            l("Fehler in getgetschema4aid: " & ex.ToString())
            Return "fehler ingetgetschema4aid"
        End Try
    End Function

    Friend Shared Function getsql(sql As String, dbname As String) As String
        Try
            l(" MOD getsql anfang")
            l("1sql " & sql)
            Dim result As String
            sql = sql.Replace("#", " ")
            l("dbname " & dbname)
            result = dbgrabsimple(sql, False, dbname)
            l("getsql: " & result)
            l(" MOD getsql ende")
            Return result
        Catch ex As Exception
            l("Fehler in getsql: " & ex.ToString())
            Return "Fehler in getsql: " & ex.ToString()
        End Try
    End Function


End Class
