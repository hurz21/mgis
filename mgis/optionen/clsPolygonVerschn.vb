Imports System.Data
Public Class clsPolygonVerschn
    Shared Function erzeugeCSVausgabeEigentuemer(collFST As List(Of clsFlurstueck), trenner As String) As String
        Dim gemparms As New clsGemarkungsParams

        Dim sb As New Text.StringBuilder
        Dim i As Integer = 1
        Try
            l("erzeugeCSVausgabeEigentuemer---------------------- anfang")
            sb.Append("lnr" & trenner)
        sb.Append("Gemarkung" & trenner)
        sb.Append("Flur" & trenner)
        sb.Append("Zahler" & trenner)
        sb.Append("Nenner" & trenner)
        sb.Append("NameAdresse" & trenner)
        sb.Append(Environment.NewLine)

        For Each fst As clsFlurstueck In collFST
            sb.Append(i.ToString & trenner)
            sb.Append(gemparms.gemcode2gemarkungstext(fst.gemcode) & trenner)
            sb.Append(fst.flur & trenner)
            sb.Append(fst.zaehler & trenner)
            sb.Append(fst.nenner & trenner)
            sb.Append(fst.schnellNamenUndAdresse.Replace(";", " ").Replace(vbCr, " ").Replace(vbCrLf, " ").Replace(vbLf, " ") & trenner)
            sb.Append(Environment.NewLine)
            'sb.Append(fst.gemcode & trenner)
            'sb.Append(fst.gemcode & trenner)
            i += 1
        Next
        Return sb.ToString

            l("erzeugeCSVausgabeEigentuemer---------------------- ende")
        Catch ex As Exception
            l("Fehler in erzeugeCSVausgabeEigentuemer: " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Function polygonstringLesen(polygonCookieLokal As String) As String
        Try
            l("polygonstringLesen---------------------- anfang")
            Dim FI As New IO.FileInfo(polygonCookieLokal)
            If FI.Exists Then
                Return My.Computer.FileSystem.ReadAllText(polygonCookieLokal)
            End If
            FI = Nothing
            Return ""
            l("polygonstringLesen---------------------- ende")
        Catch ex As Exception
            l("Fehler in polygonstringLesen: " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Sub polygonstringSpeicchern(polygonCookieLokal As String, polygonWKTString As String)
        Try
            l("polygonstringSpeicchern---------------------- anfang")
            My.Computer.FileSystem.WriteAllText(polygonCookieLokal, polygonWKTString, False, enc)
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in polygonstringSpeicchern: " & ex.ToString())
        End Try
    End Sub


    Shared Function getSchnellEigentuemer(collFST As List(Of clsFlurstueck)) As Boolean
        'Dim eigSDB As New clsEigentuemerSQLS
        Dim dt As DataTable = Nothing
        Dim sql As String
        Dim Eigentuemernameundadresse As String = ""
        Dim eigentumerKurzinfo As String = ""
        Dim mycount As Integer
        Dim kommaListe, hinweis As String
        Dim nameundadresse, fs As String
        Dim schrittweite As Integer = 20
        Dim iend As Integer
        l("getSchnellEigentuemer------------------")
        Try
            For j = 0 To collFST.Count - 1 Step schrittweite
                iend = (j + 1) + schrittweite
                If iend > collFST.Count - 1 Then
                    iend = collFST.Count - 1
                End If
                kommaListe = bildeFSlisteAusCollection(collFST, j, iend)
                kommaListe = clsString.removeLastChar(kommaListe)
                sql = "select * from paradigma.dbo.alkis_fs2eigentuemer where fs in (" & kommaListe & ")"
                l("getSchnellEigentuemer---------------------- anfang")

                'dt = modSQLsTools.getDTFromParadigmaDBsqls(sql)

                dt = modgetdt4sql.getDT4Query(sql, paradigmaMsql, hinweis)
                'eigSDB.oeffneConnectionEigentuemer()
                eigentumerKurzinfo = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("TOOLTIP")))
                Eigentuemernameundadresse = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("NAMENADRESSEN")))

                'If eigSDB.getEigentuemerdata(eigentumerKurzinfo, Eigentuemernameundadresse, mycount, dt, sql) Then
                For i = 0 To dt.Rows.Count - 1
                    nameundadresse = CStr(clsDBtools.fieldvalue(dt.Rows(i).Item("NAMENADRESSEN")))
                    fs = CStr(clsDBtools.fieldvalue(dt.Rows(i).Item("fs")))
                    For Each fst As clsFlurstueck In collFST
                        If fs = fst.FS Then
                            fst.schnellNamenUndAdresse = nameundadresse
                            Continue For
                        End If
                    Next
                Next
                'Else
                '    l("getSchnellEigentuemer---------------------- ende keine eigentuemer vorhanden")
                '    Return False
                'End If
            Next

            Return True
            l("getSchnellEigentuemer---------------------- ende")
        Catch ex As Exception
            l("Fehler in getSchnellEigentuemer: " & ex.ToString())
            Return False
        End Try
    End Function


    Shared Function bildeFSlisteAusCollection(collFST As List(Of clsFlurstueck), istart As Integer, iend As Integer) As String
        Dim sb As New Text.StringBuilder()
        Try
            l("bildeFSlisteAusCollection---------------------- anfang")
            'sb.Append(collFST.Count & Environment.NewLine)
            For i = istart To iend
                sb.Append(Chr(39) & collFST(i).FS & Chr(39) & ",") ' Environment.NewLine)
            Next
            Return sb.ToString
            l("bildeFSlisteAusCollection---------------------- ende")
        Catch ex As Exception
            l("Fehler in bildeFSlisteAusCollection: " & ex.ToString())
            Return ""
        End Try
    End Function

    Shared Function bildeFSTCollection(fstAuswahl As DataTable) As List(Of clsFlurstueck)
        Dim aktFS As New clsFlurstueck
        Dim tcoll As New List(Of clsFlurstueck)
        Try
            l("bildeFSTCollection---------------------- anfang")
            For i = 0 To fstAuswahl.Rows.Count - 1
                aktFS = New clsFlurstueck
                aktFS.FS = clsDBtools.fieldvalue(fstAuswahl.Rows(i).Item("fs"))
                aktFS.weistauf = clsDBtools.fieldvalue(fstAuswahl.Rows(i).Item("weistauf"))
                aktFS.zeigtauf = clsDBtools.fieldvalue(fstAuswahl.Rows(i).Item("zeigtauf"))
                aktFS.istgebucht = clsDBtools.fieldvalue(fstAuswahl.Rows(i).Item("istgebucht"))
                aktFS.gemcode = CInt(clsDBtools.fieldvalue(fstAuswahl.Rows(i).Item("gemcode")))
                aktFS.flur = CInt(clsDBtools.fieldvalue(fstAuswahl.Rows(i).Item("flur")))
                aktFS.zaehler = CInt(clsDBtools.fieldvalue(fstAuswahl.Rows(i).Item("zaehler")))
                aktFS.nenner = CInt(clsDBtools.fieldvalue(fstAuswahl.Rows(i).Item("nenner")))
                aktFS.flaecheqm = CDbl(clsDBtools.fieldvalue(fstAuswahl.Rows(i).Item("gisarea")))
                tcoll.Add(aktFS)
            Next
            Return tcoll
            l("bildeFSTCollection---------------------- ende")
        Catch ex As Exception
            l("Fehler in bildeFSTCollection: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Shared Function holeFSTlistFuerPolygon(pgPolygon As String) As DataTable
        Dim SQL = "SELECT * FROM flurkarte.basis_f " &
                              "WHERE ST_Within(flurkarte.basis_f.geom, " &
                              "ST_GeomFromText('" &
                              pgPolygon &
                              "', " & PostgisDBcoordinatensystem & ")) "
        Try
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            Return dt
        Catch ex As Exception
            nachricht("Fehler in holeKoordinatenFuerUmkreis: " & ex.ToString)
            Return Nothing
        End Try
    End Function
    Shared Function holeAreaFuerWKT(shapeSerial As String) As Double
        Dim SQL = "SELECT ST_Area(geom) FROM (SELECT ST_GeomFromText('" &
           shapeSerial &
            "'," & PostgisDBcoordinatensystem & ")) as foo(geom); "
        Try
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            Return CDbl(clsDBtools.fieldvalue(clsDBtools.fieldvalue(dt.Rows(0).Item(0))))
        Catch ex As Exception
            l("Fehler in holeAreaFuerGID: ", ex)
            Return -1
        End Try
    End Function

    Shared Function getWktGeomTyp(aktBOX As String) As String
        'geomTypeBOX(490560 5548579,490560 5548579)
        Dim a() As String
        Try
            aktBOX = aktBOX.Replace("BOX(", "")
            aktBOX = aktBOX.Replace(")", "")
            a = aktBOX.Split(","c)
            If a.Length = 2 Then
                If a(0) = a(1) Then
                    Return "point"
                End If
            End If
            Return "polygon"
        Catch ex As Exception
            nachricht("fehler in bildePufferFuerPunkt: " & ex.ToString)
            Return ""
        End Try


    End Function
End Class
