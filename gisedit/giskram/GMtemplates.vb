Module GMtemplates

    Sub nachricht(text As String)
        clsTools.l(text)
    End Sub
    Public Function templateEinlesen(datei As String) As String
        Dim fi As New IO.FileInfo(datei)
        Dim summe As String
        Try
            If Not fi.Exists Then
                Return ""
            End If
            fi = Nothing
            Using fis As New IO.StreamReader(datei)
                summe = fis.ReadToEnd
            End Using
            Return summe.ToString
        Catch ex As Exception
            nachricht("fehler in templateEinlesen: datei" & datei & " --> " & ex.ToString)
            Return ""
        End Try
    End Function

    Private Sub buildStringLoop(ByVal polygon As myPoint(), ByVal polygonstring As Text.StringBuilder)
        Try

            For i = 0 To polygon.Length - 1
                polygonstring.Append(" new google.maps.LatLng(" &
                                     polygon(i).Y.ToString.Replace(",", ".") & ", " &
                                      polygon(i).X.ToString.Replace(",", ".") & ") ," & Environment.NewLine)
            Next
        Catch ex As Exception
            nachricht("fehler in buildStringLoop: datei" & " --> " & ex.ToString)
        End Try

    End Sub
    Public Function templateAnpassen(templ As String, coords As String, title As String,
                                         polygon As myPoint(),
                                         TEXTKOERPER As String) As String
        Try


            templ = templ.Replace("[LATLONG]", coords)
            templ = templ.Replace("[TITLE]", title)
            'templ = templ.Replace("[POLYGON]", polygon)
            templ = templ.Replace("[TEXTKOERPER]", TEXTKOERPER)

            Dim polygonstring As New Text.StringBuilder

            ' new google.maps.LatLng(25.774252, -80.190262), 
            If polygon IsNot Nothing Then
                buildStringLoop(polygon, polygonstring)
                templ = templ.Replace("[POLYGON]", polygonstring.ToString.Substring(0, polygonstring.Length - 3))
            Else
                templ = templ.Replace("[POLYGON]", " ")
            End If
            Return templ
        Catch ex As Exception
            nachricht("fehler in templateAnpassen: datei" & " --> " & ex.ToString)
            Return ""
        End Try
    End Function

    Public Function templateAuschreiben(templ As String, serverunc As String) As String
        Dim outfile As String
        Try
            'outfile = initP.getValue("GisServer.gisCacheDir")
            'outfile = serverunc & "cache\gis\"
            outfile = IO.Path.Combine(serverunc, Environment.UserName & "_" & ".html")
            My.Computer.FileSystem.WriteAllText(outfile, templ, False)
            Return outfile
        Catch ex As Exception
            nachricht("fehler in templateAuschreiben: datei" & " --> " & ex.ToString)
            Return ""
        End Try
    End Function

    Public Sub templateStarten(templ As String)
        Try
            Process.Start(New ProcessStartInfo(templ))
        Catch ex As Exception
            nachricht("fehler in templateStarten: datei" & " --> " & ex.ToString)
        End Try

    End Sub




End Module


