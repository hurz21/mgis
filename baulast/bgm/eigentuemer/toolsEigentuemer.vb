Imports System.Data
Imports System.Data.SqlClient
Public Class toolsEigentuemer
    Public Shared paradigmaMsql As New clsDBspecMSSQL
    Public Shared paradigmaMsqlmyconn As SqlConnection
    Public Shared mssqlhost As String = "msql01"
    Shared Sub initMssql()
        Try
            l(" MOD initMssql anfang")
            paradigmaMsql.mydb = New clsDatenbankZugriff
            paradigmaMsql.mydb.Host = mssqlhost
            paradigmaMsql.mydb.username = "sgis" : paradigmaMsql.mydb.password = "WinterErschranzt.74"
            paradigmaMsql.mydb.Schema = "Paradigma"
            paradigmaMsql.mydb.Tabelle = "" : paradigmaMsql.mydb.dbtyp = "sqls"
            l(" MOD initMssql ende")
        Catch ex As Exception
            l("Fehler in initMssql: " & ex.ToString())
        End Try
    End Sub
    Friend Shared Function geteigentuemertext(fSTausGISListe As List(Of clsFlurstueck)) As String
        l(" MOD geteigentuemertext anfang")
        Dim strLage, res, schnell As String
        Dim aktfst As New clsFlurstueck
        Try
            initMssql()
            For Each fsd As clsFlurstueck In fSTausGISListe
                strLage = fsd.FS & ": " & getlage(fsd.FS)
                l(strLage)
                schnell = getSchnellbatchEigentuemer(fsd.FS)
                l(schnell)
                res = res & Environment.NewLine & strLage & Environment.NewLine &
                  schnell & strLage & Environment.NewLine
            Next
            l(" MOD geteigentuemertext ende " & res)
            Return res
        Catch ex As Exception
            l("Fehler in geteigentuemertext: " & ex.ToString())
            Return "fehler geteigentuemertext"
        End Try
    End Function

    Shared Function getSchnellbatchEigentuemer(fS As String) As String
        l(" MOD getSchnellbatchEigentuemer anfang")
        Dim dt As DataTable = Nothing
        Dim Eigentuemernameundadresse As String = ""
        Dim eigentumerKurzinfo = "", hinweis As String = ""
        Dim sql As String = "select * from paradigma.dbo.alkis_fs2eigentuemer where fs='" & fS & "'"
        Try
            dt = modgetdt4sql.getDT4Query(sql, toolsEigentuemer.paradigmaMsql, hinweis)
            If dt.Rows.Count > 0 Then
                eigentumerKurzinfo = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("TOOLTIP")))
                Eigentuemernameundadresse = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("NAMENADRESSEN")))
                '  Return eigentumerKurzinfo
                Return Eigentuemernameundadresse
            Else
                Return "Fehler. Kein Flurstück in den GIS-Daten gefunden ? Der Zeitraum zwischen 2002 und 2010 hat keine historischen Kastaster-Daten !"
            End If
            l(" MOD getSchnellbatchEigentuemer ende")
        Catch ex As Exception
            l("Fehler in getSchnellbatchEigentuemer: " & ex.ToString())
        End Try
    End Function
    Friend Shared Function getlage(fs As String) As String
        Dim dt As DataTable
        Dim strlage = ""
        Dim hinweis As String = ""
        Try
            l(" getlage ---------------------- anfang")
            'Dim sql As String
            fstREC.mydb.SQL = "select * from flurkarte.basis_ext_f where fs='" & fs & "'"
            'dt = getDTFromWebgisDB(sql, "postgis20") 
            l(fstREC.mydb.SQL)
            hinweis = fstREC.getDataDT()
            If fstREC.dt.Rows.Count > 0 Then
                strlage = "Lage: " & clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item("name")).Trim
                strlage = strlage & ", " & clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item("lage")).Trim
                If clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item("hausnr")).Trim <> String.Empty Then
                    strlage = strlage & ", Nr: " & clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item("hausnr")).Trim & ". "
                    'strlage = strlage & "Bez: " & clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item("bezeich")).Trim
                Else
                    strlage = strlage & ". "
                End If
            Else
                strlage = ""
            End If
            l(" getlage ---------------------- ende: " & strlage)
            Return strlage
        Catch ex As Exception
            l("Fehler in getlage: " & fs & ", " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
