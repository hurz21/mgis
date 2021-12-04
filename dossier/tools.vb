Module tools
    Public aktrange As New clsRange
    Function OpenDokument(full As String) As Boolean
        Dim fi As IO.FileInfo
        Try
            l(" OpenDokument ---------------------- anfang")
            fi = New IO.FileInfo(full.Replace("/", "\"))
            If fi.Exists Then
                Process.Start(full)
                fi = Nothing
                Return True
            Else
                ' MsgBox("Datei konnte nicht geFunden werden!")
                Return False
            End If
            l(" OpenDokument ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in OpenDokument: " & ex.ToString())
            Return False
        End Try
    End Function

    Sub setLogfile()
        With My.Log.DefaultFileLogWriter
#If DEBUG Then
            '.CustomLocation = mgisUserRoot & "logs\"
#Else
#End If
            .CustomLocation = My.Computer.FileSystem.SpecialDirectories.Temp & "\mgisDossier_logs\"
            '  .CustomLocation = mgisUserRoot & "logs\"
            .BaseFileName = m.GisUser.username
            .AutoFlush = True
            .Append = False
        End With
    End Sub


    Public Sub nachricht(ByVal text As String)
        Dim anhang As String = ""
        Try
            My.Log.WriteEntry(text)
            mitFehlerMail(text, anhang)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub mitFehlerMail(ByVal text As String, ByVal anhang As String)
        If text.ToLower.StartsWith("fehler") Or text.ToLower.StartsWith("error") Or
                text.ToLower.Contains("allgemeiner fehler") Or
            text.ToLower.StartsWith("warnungd") Or text.ToLower.StartsWith("problem") Then
            My.Log.WriteEntry("!!!!  mitFehlerMail: Ein Fehler/Warnung ist aufgetreten !!!! ----------------------------")
            Dim keyword As String
            If text.ToLower.StartsWith("warnung") Then
                keyword = "warnung"
                anhang = ""
            Else
                keyword = "Fehler"
                anhang = genScreenshotAndSaveLocal()
            End If
            My.Log.DefaultFileLogWriter.Flush()
            Dim fi As New IO.FileInfo(My.Log.DefaultFileLogWriter.FullLogFileName)
            Dim logrtxt = IO.Path.Combine(System.IO.Path.GetTempPath, "dump.txt")
            fi.CopyTo(logrtxt, True)
            fi = Nothing

            anhang = anhang & "," & logrtxt
            'Dim test As Boolean = modMail.mailrausSMTP(von:="dr.j.feinen@kreis-offenbach.de",
            '                                    an:="dr.j.feinen@kreis-offenbach.de",
            '                                    betreff:=keyword & " in gisdossier: " & ", user: " & m.GisUser.username,
            '                                    nachricht:=text.Replace(vbCrLf, "<br>") & "version:" & m.dossierVersion,
            '                                    anHang:=anhang,
            '                                    iminternet:=False,
            '                                    mailserverinternet:="",
            '                                    mailserverintranet:="",
            '                                    hinweis:="",
            '                                    inifile:="",
            '                                    CC:="")

            Dim mailservername = "", mailserverKonto = "", mailserverPW = "" 'As String
            Dim mailport As Integer = 25, mailssl As Boolean = True

            Dim test As Boolean = modMail.mailrausSMTP(von:="dr.j.feinen@kreis-offenbach.de",
                                                        an:="dr.j.feinen@kreis-offenbach.de",
                                        betreff:=keyword & " in gisdossier: " & ", user: " & m.GisUser.username,
                                        nachricht:=text.Replace(vbCrLf, "<br>") & "version:" & m.dossierVersion,
                                        anHang:=anhang,
                                        mailserverName:="",
                                        mailserverkonto:="",
                                        mailserverPw:="",
                                        hinweis:="",
                                        inifile:="",
                                        CC:="",
                                        port:=mailport,
                                        ssl:=mailssl)
        End If
        My.Log.WriteEntry("----------------------------")

    End Sub
    Private Function genScreenshotAndSaveLocal() As String
        Dim anhang As String
        Dim screenshot As System.Drawing.Bitmap
        Dim graph As System.Drawing.Graphics
        Try
            anhang = IO.Path.Combine(System.IO.Path.GetTempPath, "dump.png")
            screenshot = New System.Drawing.Bitmap(CInt(System.Windows.SystemParameters.PrimaryScreenWidth),
                                                   CInt(System.Windows.SystemParameters.PrimaryScreenHeight),
                                                   System.Drawing.Imaging.PixelFormat.Format32bppPArgb)

            'My.Computer.Screen.WorkingArea.Width,
            '                                       My.Computer.Screen.WorkingArea.Height,
            '                                       System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
            graph = System.Drawing.Graphics.FromImage(screenshot)
            '  graph.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy)
            graph.CopyFromScreen(New System.Drawing.Point(0, 0), New System.Drawing.Point(0, 0),
               New System.Drawing.Size(CInt(System.Windows.SystemParameters.PrimaryScreenWidth),
                                                  CInt(System.Windows.SystemParameters.PrimaryScreenHeight)))
            graph.Save()
            screenshot.Save(anhang, System.Drawing.Imaging.ImageFormat.Png)
            Return anhang
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Sub l(v As String)
        nachricht(v)
    End Sub
    Sub l(v As String, excep As Exception)
        nachricht(v & Environment.NewLine & excep.ToString & Environment.NewLine)
    End Sub
End Module
