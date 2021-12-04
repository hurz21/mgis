Imports System.Data

Module Module2
    Function getSchnellbatchEigentuemer(fS As String) As String
        'Dim eigSDB As New clsEigentuemerSQLS
        Dim dt As DataTable = Nothing
        Dim Eigentuemernameundadresse As String = ""
        Dim eigentumerKurzinfo = "", hinweis As String = ""
        'Dim mycount As Integer
        Dim sql As String = "select * from paradigma.dbo.alkis_fs2eigentuemer where fs='" & fS & "'"

        'dt = modSQLsTools.getDTFromParadigmaDBsqls(sql)
        dt = modgetdt4sql.getDT4Query(sql, paradigmaMsql, hinweis)
        If dt.Rows.Count > 0 Then
            eigentumerKurzinfo = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("TOOLTIP")))
            Eigentuemernameundadresse = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("NAMENADRESSEN")))
            '  Return eigentumerKurzinfo
            Return Eigentuemernameundadresse
        Else
            Return "Fehler. Zu viele Eigentuemer ? Bitte die langsame Abfrage via Word benutzen !"
        End If
    End Function

    Friend Function holeRestlicheParams4FST(fS As String, ByRef weistauf As String, ByRef zeigtauf As String, ByRef gebucht As String, ByRef areaqm As String) As Boolean
        Try
            l("holeRestlicheParams4FST---------------------- anfang")
            Dim SQL = "select weistauf,zeigtauf,istgebucht,gisarea  from  flurkarte.basis_f " &
                                 " where gemcode = " & aktFST.normflst.gemcode &
                                 " and flur = " & aktFST.normflst.flur &
                                 " and zaehler = " & aktFST.normflst.zaehler &
                                 " and nenner = " & aktFST.normflst.nenner &
                                 " order by nenner  "
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            If dt.Rows.Count < 1 Then
                Return False
            Else
                weistauf = clsDBtools.fieldvalue(dt.Rows(0).Item("weistauf"))
                zeigtauf = clsDBtools.fieldvalue(dt.Rows(0).Item("zeigtauf"))
                gebucht = clsDBtools.fieldvalue(dt.Rows(0).Item("istgebucht"))
                areaqm = clsDBtools.fieldvalue(dt.Rows(0).Item("gisarea"))
                Return True
            End If

            l("holeRestlicheParams4FST---------------------- ende")
        Catch ex As Exception
            l("Fehler in holeRestlicheParams4FST: " & ex.ToString())
            Return False
        End Try
    End Function
End Module
