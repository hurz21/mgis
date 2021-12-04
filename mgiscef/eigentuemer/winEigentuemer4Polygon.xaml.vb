Imports System.Data

Public Class winEigentuemer4Polygon
    Dim polygonCookieLokal As String
    Sub New()
        InitializeComponent()
        If Not GisUser.istalbberechtigt Then
            tabEigent.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub winEigentuemer4Polygon_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        polygonCookieLokal = IO.Path.Combine(strGlobals.localDocumentCacheRoot, "polygonCookie.txt")
        l("polygonCookieLokal " & polygonCookieLokal)
        tbtest.Text = lastGeomAsWKT
        If lastGeomAsWKT.IsNothingOrEmpty Then
            lastGeomAsWKT = clsPolygonVerschn.polygonstringLesen(polygonCookieLokal)
            If lastGeomAsWKT.IsNothingOrEmpty Then
            Else
                tbtest.Text = lastGeomAsWKT
            End If
        Else
            tbtest.Text = lastGeomAsWKT
            clsPolygonVerschn.polygonstringSpeicchern(polygonCookieLokal, lastGeomAsWKT)
        End If
    End Sub
    Private Sub btntest_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If tbtest.Text.IsNothingOrEmpty Then
            MessageBox.Show("Feld ist leer. Abbruch!" & Environment.NewLine &
                            " Eine Fläche kann mit der Messfunktion erzeugt werden." & Environment.NewLine &
                            " Die gemessene Fläche wird dann hier abgelegt." & Environment.NewLine,
                            "Keine Eingabe")
        Else
            verschneidung1(tbtest.Text)
        End If

    End Sub

    Private Sub verschneidung1(PolygonWKTString As String)
        Dim sql As String
        Try
            l("verschneidung1---------------------- anfang")
            l("polygonCookieLokal " & polygonCookieLokal)
            If PolygonWKTString.IsNothingOrEmpty Then Exit Sub
            '    userIniProfile.WertSchreiben("Diverse", "lastGeomAsWKT", PolygonWKTString)
            clsPolygonVerschn.polygonstringSpeicchern(polygonCookieLokal, PolygonWKTString)
            sql = "SELECT * FROM flurkarte.basis_f " &
                              "WHERE ST_Within(flurkarte.basis_f.geom, " &
                              "ST_GeomFromText('" &
                              PolygonWKTString &
                              "', " & PostgisDBcoordinatensystem & ")) "
            If iminternet Or CGIstattDBzugriff Then
                Dim result As String = "", hinweis As String = ""

                result = clsToolsAllg.getSQL4Http(sql, "postgis20", hinweis, "getsql") : l(hinweis)
                result = result.Replace(vbCrLf, "").Trim
                collFST.Clear()
                collFST = clsPolygonVerschn.bildeFSTCollectionAJAX(result)
            Else
                Dim fstAuswahl As DataTable
                fstAuswahl = clsPolygonVerschn.holeFSTlistFuerPolygon(PolygonWKTString, sql)
                collFST.Clear()
                collFST = clsPolygonVerschn.bildeFSTCollection(fstAuswahl)
            End If


            If GisUser.istalbberechtigt Then
                clsPolygonVerschn.getSchnellEigentuemer(collFST)
                Dim text As String = clsPolygonVerschn.erzeugeCSVausgabeEigentuemer(collFST, ";")
                Dim datei As String = IO.Path.Combine(strGlobals.localDocumentCacheRoot,
                                                      "verschneidung" & clsString.timestamp & ".csv")
                l("csvausgabe: " & datei)
                My.Computer.FileSystem.WriteAllText(datei, text, False, enc)
                OpenDokument(datei)
            End If
            l("verschneidung1---------------------- ende")
        Catch ex As Exception
            l("Fehler in verschneidung1: " & ex.ToString())
        End Try
    End Sub

    Private Sub btnInKarteanzeigen_Click(sender As Object, e As RoutedEventArgs)
        lastGeomAsWKT = tbtest.Text
        aktPolygon.ShapeSerial = tbtest.Text
        aktFST.normflst.serials.Add(aktPolygon.ShapeSerial)
        suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen
        Close()
        e.Handled = True
    End Sub
End Class
