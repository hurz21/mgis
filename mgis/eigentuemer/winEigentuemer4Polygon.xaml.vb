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
        polygonCookieLokal = IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDocuments),
                               "polygonCookie.txt")

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
        verschneidung1(tbtest.Text)
        e.Handled = True
    End Sub

    Private Sub verschneidung1(PolygonWKTString As String)
        Dim fstAuswahl As DataTable
        Try
            l("verschneidung1---------------------- anfang")

            l("polygonCookieLokal " & polygonCookieLokal)
            '    userIniProfile.WertSchreiben("Diverse", "lastGeomAsWKT", PolygonWKTString)
            clsPolygonVerschn.polygonstringSpeicchern(polygonCookieLokal, PolygonWKTString)
            fstAuswahl = clsPolygonVerschn.holeFSTlistFuerPolygon(PolygonWKTString)
            collFST.Clear()
            collFST = clsPolygonVerschn.bildeFSTCollection(fstAuswahl)

            If GisUser.istalbberechtigt Then
                clsPolygonVerschn.getSchnellEigentuemer(collFST)
                Dim text As String = clsPolygonVerschn.erzeugeCSVausgabeEigentuemer(collFST, ";")
                Dim datei As String = IO.Path.Combine(Environment.GetFolderPath(
                                                      System.Environment.SpecialFolder.CommonDocuments),
                                                      "temp.csv")
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
        suchObjektModus = "fst"
        Close()
        e.Handled = True
    End Sub
End Class
