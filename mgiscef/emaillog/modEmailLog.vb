Module modEmailLog
    Public Sub nachricht(ByVal text As String, exec As Exception)
        Dim anhang As String = ""
        Try
            text = text.Replace("DefaultSource	Information	0	", "")
            text = text & Environment.NewLine & ToLogString(exec, "")

            'text = text & exec.ToString
            If myglobalz.minErrorMessages Then
                If Not (text.ToLower.StartsWith("fehler") Or text.ToLower.StartsWith("warnung")) Then
                    Exit Sub
                End If
            End If
            My.Log.WriteEntry(text)
#If DEBUG Then
            If iminternet And Environment.UserDomainName.ToLower = "kreis-of" Then Exit Sub
#End If
            If Not iminternet Then
                mitFehlerMail(text, anhang)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub nachricht(ByVal text As String)
        Dim anhang As String = ""
        Try
            If myglobalz.minErrorMessages Then
                If Not (text.ToLower.StartsWith("fehler") Or text.ToLower.StartsWith("warnung")) Then
                    Exit Sub
                End If
            End If
            My.Log.WriteEntry(text)
#If DEBUG Then
            If iminternet And Environment.UserDomainName.ToLower = "kreis-of" Then Exit Sub
#End If
            If Not iminternet Then
                mitFehlerMail(text, anhang)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub mitFehlerMail(ByVal text As String, ByVal anhang As String)
        If Not (istechterFehler(text)) Then
            Exit Sub
        End If
        My.Log.WriteEntry("!!!!  mitFehlerMail: Ein Fehler/Warnung ist aufgetreten !!!! ---------------------------- " & GisUser.macAdress & "/" & GisUser.domain)
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
                    'anhang = "" ' genScreenshotAndSaveLocal()
                    anhang = genScreenshotAndSaveLocal()
                    anhang = ""
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
            Dim summe As String = IO.File.ReadAllText(logrtxt)
            summe = summe.Replace("DefaultSource	Information	0	", "")

            IO.File.WriteAllText(logrtxt, summe)
            anhang = anhang & "," & logrtxt
        End If
        Dim von = "dr.j.feinen@kreis-offenbach.de"
        Dim betreff = keyword & " in mgis: " & ", user: " & GisUser.nick
        Dim body = text.Replace(vbCrLf, "<br>") & Environment.NewLine & GisUser.myString()
        Dim test As Boolean = modMail.mailen(iminternet, GisUser, emailEmpfaenger, anhang, body, von, betreff)

        '    Dim mailservername = "", mailserverKonto = "", mailserverPW = ""
        '    Dim mailport As Integer = 25, mailssl As Boolean = True
        '    modMail.getMailCredentials(iminternet, mailservername, mailserverKonto, mailserverPW, mailport, mailssl, GisUser)
        '    Dim erfolg As Boolean = modMail.mailrausSMTP(von:=von,
        '                                               an:=emailEmpfaenger,
        '                                                   betreff:=betreff,
        '                                                    nachricht:=body,
        '                                                    anHang:=anhang,
        '                                                    mailserverName:=mailservername,
        '                                                    mailserverkonto:=mailserverKonto,
        '                                                    mailserverPw:=mailserverPW,
        '                                                    hinweis:="",
        '                                                    inifile:="",
        '                                                    CC:="",
        '                                                    port:=mailport,
        '                                                    ssl:=mailssl)
        'End If
        My.Log.WriteEntry("----------------------------")
    End Sub

    Private Function istechterFehler(text As String) As Boolean
        Return text.ToLower.StartsWith("fehler") Or text.ToLower.StartsWith("error") Or
                                text.ToLower.Contains("allgemeiner fehler") Or
                                text.ToLower.StartsWith("warnungd") Or text.ToLower.StartsWith("problem")
    End Function

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
        nachricht(v & Environment.NewLine, excep)
    End Sub
End Module
