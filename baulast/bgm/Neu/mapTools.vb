Public Class mapTools


    Friend Shared Function genPreviewURL(aktrange As clsRange, breite As Integer, hoehe As Integer, hgrund As String, puffer As Integer, gid As String) As String
        Try
            l(" genPreview ---------------------- anfang")
            Dim mapsize As String
            Dim Mapfile As String
            'http://gis.kreis-of.local/cgi-bin/mapserv722/mapserv.exe?mode=map&mapsize=400+300&mapext=484999+5542166+485168+5542289&map=/inetpub/wwwroot/apps/paradigma/nt/header_2.map&gid=6
            Dim geodatenroot, url As String
            mapsize = breite & "+" & hoehe
            Mapfile = calcMapfileName(hgrund)
            '#If DEBUG Then
            Mapfile = "/baulastenundflurkarte_test.map"
            '#End If
            Dim xl, xh, yl, yh As Integer

            xl = CInt(aktrange.xl) - puffer
            xh = CInt(aktrange.xh) + puffer
            yl = CInt(aktrange.yl) - puffer
            yh = CInt(aktrange.yh) + puffer

            geodatenroot = "http://gis.kreis-of.local/cgi-bin/mapserv722/mapserv.cgi?mode=map&mapsize=" & mapsize

            url = geodatenroot & "&mapext=" & (xl & "+" & (yl) & "+" & (xh) & "+" & (yh))
            url = url & "&map=/nkat/vorlage/paradigma/baulasten" & Mapfile
            url = url & "&gid=" & gid
            'url = url & "&gid=173"

            url = url & "&ts=" & clsString.getTimestamp
            l(url)
            l(" genPreview ---------------------- ende")
            Return url
            'Else
            '    '
            'End If
        Catch ex As Exception
            l("Fehler in genPreview: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function calcMapfileName(hgrund As String) As String
        Dim mapfile As String

        Dim vgrundstring = "baulasten"

        mapfile = "/" & vgrundstring & "und" & hgrund & ".map"
        Return Mapfile
    End Function


End Class
