Imports System.Data

Public Class clsStrasseUnscharfSuchen
    Friend Shared Function getDatatable(strassenTeil As String, ohnegemeinde As Boolean) As DataTable
        Dim schluesssellike As String
        strassenTeil = strassenTeil.ToLower.Trim
        schluesssellike = "06" & aktadr.Gisadresse.gemeindebigNRstring
        If ohnegemeinde Then
            adrREC.mydb.SQL =
                   "SELECT distinct (gemeindetext || ', ' || trim(sname)) as sname,(strcode || ',' || gemeindenr) as strcode  FROM flurkarte.halofs  " &
                   " where  lower(sname)  like '%" & strassenTeil & "%' " &
                   " order by  (sname)  "
        Else
            adrREC.mydb.SQL =
               "SELECT distinct trim(sname) as sname,strcode  FROM flurkarte.halofs  " &
               " where gemeindeNR  = " & aktadr.Gisadresse.gemeindebigNRstring & "" &
               " and lower(sname)  like '%" & strassenTeil & "%' " &
               " order by  (sname)  "
        End If

        '  myGlobalz.adrREC.mydb.Schema = "halosort"
        Dim hinweis As String = adrREC.getDataDT()
        Return adrREC.dt
    End Function
End Class
