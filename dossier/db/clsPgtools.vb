Imports System.Data

Public Class clsPgtools
    Friend Shared Function getDTFromWebgisDB(queryString As String, Db As String, ByRef strError As String) As DataTable
        l("getDTFromWebgisDB-------------------------")
        l(" webgisREC.mydb.Schema " & m.webgisREC.mydb.Schema)
        l("queryString : " & queryString)
        Try
            m.webgisREC.mydb.Schema = Db
            m.webgisREC.mydb.SQL = queryString
            strError = m.webgisREC.getDataDT()
            Return m.webgisREC.dt
        Catch ex As Exception
            l("fehler in getDTFromWebgisDB ", ex)
            Return Nothing
        End Try
    End Function

    Shared Function getDT4Query(sql As String, ByVal myneREC As IDB_grundfunktionen, ByRef hinweis As String) As DataTable
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
    Shared Function getSchnellbatchEigentuemer(fS As String) As String
        'Dim eigSDB As New clsEigentuemerSQLS
        Dim dt As DataTable = Nothing
        Dim Eigentuemernameundadresse As String = ""
        Dim eigentumerKurzinfo, hinweis As String
        'Dim mycount As Integer
        Dim sql As String = "select * from paradigma.dbo.alkis_fs2eigentuemer where fs='" & fS & "'"

        'dt = modSQLsTools.getDTFromParadigmaDBsqls(sql)
        dt = clsPgtools.getDT4Query(sql, m.paradigmaMsql, hinweis)
        If dt.Rows.Count > 0 Then
            eigentumerKurzinfo = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("TOOLTIP")))
            Eigentuemernameundadresse = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("NAMENADRESSEN")))
            '  Return eigentumerKurzinfo
            Return Eigentuemernameundadresse
        Else
            Return "Fehler. Zu viele Eigentuemer ? Bitte die langsame Abfrage via Word benutzen !"
        End If
    End Function

    Friend Function holeRestlicheParams4FST(fS As String, ByRef weistauf As String, ByRef zeigtauf As String,
                                            ByRef gebucht As String, ByRef areaqm As String,
                                            flst As ParaFlurstueck, ByRef strError As String) As Boolean

        'aktFST.normflst
        Try
            l("holeRestlicheParams4FST---------------------- anfang")
            Dim SQL = "select weistauf,zeigtauf,istgebucht,gisarea  from  flurkarte.basis_f " &
                                 " where gemcode = " & flst.normflst.gemcode &
                                 " and flur = " & flst.normflst.flur &
                                 " and zaehler = " & flst.normflst.zaehler &
                                 " and nenner = " & flst.normflst.nenner &
                                 " order by nenner  "
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20", strError)
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
End Class
