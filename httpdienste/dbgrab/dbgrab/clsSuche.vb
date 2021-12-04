Public Class clsSuche
    Public Shared Function getstrassen(gemeindebig As String, dbname As String) As String
        Try
            l(" MOD getstrassen anfang")
            Dim SQL, result As String
            SQL =
                     "SELECT distinct trim(sname) as sname,strcode,mitadr   FROM public.haloschneise " &
                     " where gemeindenr  = " & gemeindebig & "" &
                     " order by  (sname),mitadr  desc"
            l("sql " & SQL)
            result = dbgrabsimple(SQL, False, dbname)

            l(result)
            Return result
            l(" MOD getstrassen ende")

        Catch ex As Exception
            l("Fehler in getstrassen: " & ex.ToString())
            Return "Fehler in getstrassen: " & ex.ToString()
        End Try
    End Function

    Friend Shared Function gethausnr(gemeindebig As String, strcode As String, dbname As String) As String
        Try
            l(" MOD gethausnr anfang")
            Dim SQL, result As String
            SQL = "SELECT hausnrkombi,gml_id,rechts,hoch FROM flurkarte.halofs " &
                     " where gemeindenr = '" & gemeindebig & "'" &
                     " and strcode ='" & strcode & "'" &
                     " order by  abs(hausnr)"
            l("sql " & SQL)
            result = dbgrabsimple(SQL, False, dbname)
            l(result)
            Return result
            l(" MOD gethausnr ende")
        Catch ex As Exception
            l("Fehler in gethausnr: " & ex.ToString())
            Return "Fehler in gethausnr: " & ex.ToString()
        End Try
    End Function

    Friend Shared Function getFlure(gemarkung As String, tabelle As String, dbname As String) As String
        Try
            l(" MOD getFlure anfang")
            Dim SQL, result As String
            SQL = "select distinct flur  from  " & tabelle &
                    " where gemcode = " & gemarkung &
                    " order by flur "
            l("sql " & SQL)
            result = dbgrabsimple(SQL, False, dbname)
            l(result)

            l(" MOD getFlure ende")
            Return result
        Catch ex As Exception
            l("Fehler in getFlure: " & ex.ToString())
            Return "False"
        End Try
    End Function

    Friend Shared Function getFST(gemcode As String, flur As String, tabelle As String, dbname As String) As String
        'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getfst&gemarkung=731&flur=7&tabelle=flurkarte.basis_f
        Try
            l(" MOD getFlure anfang")
            Dim SQL, result As String
            SQL = "select distinct zaehler,nenner    from  " & tabelle &
                    " where gemcode = " & gemcode &
                     " and flur   = " & flur &
                    " order by zaehler,nenner "
            l("sql " & SQL)
            result = dbgrabsimple(SQL, False, dbname)
            l(result)

            l(" MOD getFlure ende")
            Return result
        Catch ex As Exception
            l("Fehler in getFlure: " & ex.ToString())
            Return "False"
        End Try
    End Function
End Class
