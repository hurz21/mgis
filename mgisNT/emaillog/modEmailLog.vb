Module modEmailLog
    Public Sub nachricht(ByVal text As String)
        Dim anhang As String = ""
        Try
            If myglobalz.minErrorMessages Then
                If Not (text.ToLower.StartsWith("fehler") Or text.ToLower.StartsWith("warnung")) Then
                    Exit Sub
                End If
            End If
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
                If text.StartsWith("fehler ACHTUNG! Ebene ist defekt") Then
                    anhang = ""
                Else
                    If Not iminternet Then
                        anhang = genScreenshotAndSaveLocal()
                    End If
                End If

            End If
            Dim emailEmpfaenger As String
            If text.StartsWith("fehler ACHTUNG! Ebene ist defekt") Then
                If iminternet Then
                    emailEmpfaenger = "dr.j.feinen@kreis-offenbach.de"
                Else
                    emailEmpfaenger = "dr.j.feinen@kreis-offenbach.de,m.thieme@kreis-offenbach.de"
                End If
            Else
                emailEmpfaenger = "dr.j.feinen@kreis-offenbach.de"
                My.Log.DefaultFileLogWriter.Flush()
                Dim fi As New IO.FileInfo(My.Log.DefaultFileLogWriter.FullLogFileName)
                Dim logrtxt = IO.Path.Combine(System.IO.Path.GetTempPath, "dump.txt")
                fi.CopyTo(logrtxt, True)
                fi = Nothing
                anhang = anhang & "," & logrtxt
            End If
            Dim test As Boolean = mailrausSMTP(von:="dr.j.feinen@kreis-offenbach.de",
                                                   an:=emailEmpfaenger,
                                                           betreff:=keyword & " in mgis: " & ", user: " & GisUser.username,
                                                            nachricht:=text.Replace(vbCrLf, "<br>"),
                                                            anHang:=anhang,
                                                            iminternet:=iminternet,
                                                            mailserverinternet:="",
                                                            mailserverintranet:="",
                                                            hinweis:="",
                                                            inifile:="",
                                                            CC:="")
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
