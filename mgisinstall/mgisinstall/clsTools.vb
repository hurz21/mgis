Imports System.Net
Imports System.Text
Public Class clsTools
    Public Shared Property ParadigmaVersion As String


    Shared Property enc As Text.Encoding = System.Text.Encoding.GetEncoding("iso-8859-1")
    Shared Property domainMapserv As String
    'Public Shared Property serverWeb As String = "https://buergergis.kreis-offenbach.de"
    Public Shared Property serverWeb As String = "http://w2gis02.kreis-of.local"

    Public Shared bplanRootdir As String = serverWeb & "/fkat/bplan" '"http://w2gis02.kreis-of.local/fkat/bplan" '"D:\fkat\bplan" 
    Public Shared Property ProxyString As String = ""
    Public Shared Property ImInternet As Boolean = True

    Shared Sub l(text As String)
        Dim anhang As String = ""
        Try
            My.Log.WriteEntry(text)
            mitFehlerMail(text, anhang)
        Catch ex As Exception

        End Try
    End Sub


    Public Shared Sub mitFehlerMail(ByVal text As String, ByVal anhang As String)
        If text.ToLower.StartsWith("fehler") Or text.ToLower.StartsWith("error") Or
            text.ToLower.StartsWith("warnungd") Or text.ToLower.StartsWith("problem") Then
            My.Log.WriteEntry("!!!!  mitFehlerMail: Ein Fehler/Warnung ist aufgetreten !!!! ----------------------------")
            Dim keyword As String
            MsgBox(text)
            If text.ToLower.StartsWith("warnung") Then
                keyword = "warnung"
                anhang = ""
            Else
                keyword = "Fehler"
                '    anhang = genScreenshotAndSaveLocal()
            End If
            My.Log.DefaultFileLogWriter.Flush()
            Dim fi As New IO.FileInfo(My.Log.DefaultFileLogWriter.FullLogFileName)
            Dim logrtxt = IO.Path.Combine(System.IO.Path.GetTempPath, "dump.txt")
            fi.CopyTo(logrtxt, True)
            fi = Nothing

            anhang = anhang & "," & logrtxt
            Dim test As Boolean = mailrausSMTP(von:="dr.j.feinen@kreis-offenbach.de",
                                                   an:="dr.j.feinen@kreis-offenbach.de",
                                                           betreff:=keyword & " in bplan: " & ", user: " & Environment.UserName,
                                                            nachricht:=text.Replace(vbCrLf, "<br>"),
                                                            anHang:=anhang,
                                                            iminternet:=True,
                                                            mailserverinternet:="",
                                                            mailserverintranet:="",
                                                            hinweis:="",
                                                            inifile:="",
                                                            CC:="")


        End If
        My.Log.WriteEntry("----------------------------")

    End Sub
    Shared Sub createDir(targetroot As String)
        Try
            l(" createDir ---------------------- anfang" & targetroot)
            'MsgBox("Vor targetroot createdir " & targetroot)
            IO.Directory.CreateDirectory(targetroot)
            l(" createDir ---------------------- ende")

        Catch ex As Exception
            l("Fehler in createDir: " & ex.ToString())
            MsgBox(ex.Message & " fehler in createdir  " & targetroot)
        End Try
    End Sub
    Shared Function down(rootdir As String, filename As String, targetroot As String) As Boolean
        Dim remoteUri As String
        Dim myStringWebResource As String = Nothing
        Dim myWebClient As New WebClient()
        Dim lok As String = ""
        Try
            l(" down ---------------------- anfang")
            l("rootdir " & rootdir)
            l("filename " & filename)
            l("targetroot " & targetroot)
            remoteUri = rootdir
            filename = filename
            myStringWebResource = remoteUri + filename
            createDir(targetroot)
            lok = targetroot & "\" & filename
            myWebClient.DownloadFile(myStringWebResource, lok)
            l(" down ---------------------- ende " & lok)
            Return True
        Catch ex As Exception
            l("Fehler in down: " & ex.ToString())
            MsgBox(ex.ToString & Environment.NewLine &
                   lok)
            Return False
        End Try
        End
    End Function
    Public Shared Sub setLogfile(modul As String)
        With My.Log.DefaultFileLogWriter
#If DEBUG Then
            '.CustomLocation = mgisUserRoot & "logs\"
#Else
#End If

            'Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            '.CustomLocation = My.Computer.FileSystem.SpecialDirectories.Temp & "\pllogs\"
            .CustomLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & modul
            '.CustomLocation = ".\"
            '  .CustomLocation = mgisUserRoot & "logs\"
            .BaseFileName = modul & "_" & Environment.UserName '& Now.ToLongTimeString
            'MsgBox(.CustomLocation & "-" & .BaseFileName)
            .AutoFlush = True
            .Append = False
        End With
    End Sub
    Public Shared Function mailrausSMTP(ByVal von As String,
                                ByVal an As String,
                                ByVal betreff As String,
                                ByVal nachricht As String,
                                ByVal anHang As String,
                                ByVal iminternet As Boolean,
                                ByVal mailserverinternet As String,
                                ByVal mailserverintranet As String,
                                ByRef hinweis As String,
                                ByVal inifile As String,
                                ByVal CC As String) As Boolean
        Dim smtp As System.Net.Mail.SmtpClient
        Dim msg As System.Net.Mail.MailMessage
        Dim port As Integer = 25
        Dim userid As String = "", userpw As String = "", mailserver As String = ""
        Try
            clsTools.l("modmail---------------")
            clsTools.l("iminternet: " & iminternet)
            If iminternet Then
                von = "j.feinen@gmx.net"
                If mailserverinternet = String.Empty Then
                    mailserver = "mail.gmx.net"
                    userid = "j.feinen@gmx.net"
                    userpw = "thuyan19"
                    port = 587
                Else
                    mailserver = "mail.gmx.net"
                    userid = "j.feinen@gmx.net"
                    userpw = "thuyan19"
                    port = 587
                End If
            Else
                If mailserverintranet = String.Empty Then
                    mailserver = "10.34.102.254" 'mailserver_intranet
                    userid = "dr.j.feinen@kreis-offenbach.de"
                    userpw = "vinsan21"
                Else
                    mailserver = "mail.gmx.net"
                    userid = "dr.j.feinen@kreis-offenbach.de"
                    userpw = "vinsan21"
                End If
            End If
            clsTools.l("mailserver: " & mailserver)
            clsTools.l("userid: " & userid)
            clsTools.l("userpw: " & userpw)
            msg = New System.Net.Mail.MailMessage(von, an)
            msg.Subject = betreff
            msg.Body = nachricht
            msg.IsBodyHtml = True
            ' Add a carbon copy recipient.
            If Not String.IsNullOrEmpty(CC) Then
                Dim copie As System.Net.Mail.MailAddress = New System.Net.Mail.MailAddress(CC)
                msg.CC.Add(copie)
            End If
            clsTools.l("Subject: " & betreff)
            smtp = New System.Net.Mail.SmtpClient(mailserver, port)
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
            smtp.EnableSsl = True
            smtp.Credentials = New System.Net.NetworkCredential(von, userpw)
            'msg.Attachments.Add(New System.Net.Mail.Attachment(anHang)) '
            anhang_handhaben(anHang, msg)
            smtp.Send(msg)
            msg.Dispose()
            hinweis = "ok"
            '  smtp.Dispose()
            Return True
        Catch ex As Exception
            ' clstools.l("Fehler in mailrausSMTP: " & ex.ToString)
            Return False
        End Try
    End Function
    Public Shared Sub anhang_handhaben(ByRef anHang As String, ByVal msg As System.Net.Mail.MailMessage)
        Try
            If anHang Is Nothing OrElse anHang = "" Then
                Exit Sub
            End If
            If anHang.EndsWith(",") Then anHang = anHang.Substring(0, anHang.Length - 1)
            If anHang.StartsWith(",") Then anHang = anHang.Substring(1, anHang.Length - 1)
            ' clstools.nachricht("vor anhang")
            If anHang.Length > 1 Then
                If anHang.Contains(",") Then
                    'mehrfachanhang
                    Dim filelist$() = anHang.Split(","c)
                    For Each datei As String In filelist
                        If Not String.IsNullOrEmpty(datei) Then msg.Attachments.Add(New System.Net.Mail.Attachment(datei)) '
                    Next
                Else
                    If Not String.IsNullOrEmpty(anHang) Then msg.Attachments.Add(New System.Net.Mail.Attachment(anHang)) '
                End If
            End If
        Catch ex As Exception
            clsTools.l("Fehler bei anhang_handhaben: " & vbCrLf & anHang & vbCrLf & ex.ToString)
        End Try
    End Sub

End Class
