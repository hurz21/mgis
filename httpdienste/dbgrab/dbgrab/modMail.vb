Module modMail
    Public Function mailrausSMTP(ByVal von As String, _
                                    ByVal an As String, _
                                    ByVal betreff As String, _
                                    ByVal nachricht As String, _
                                    ByVal anHang As String, _
                                    ByVal iminternet As Boolean, _
                                    ByVal mailserverinternet As String, _
                                    ByVal mailserverintranet As String, _
                                    ByRef hinweis As String, _
                                    ByVal inifile As String, _
                                    ByVal CC As String) As Boolean
        Dim smtp As System.Net.Mail.SmtpClient
        Dim msg As System.Net.Mail.MailMessage
        Dim userid As String = "", userpw As String = "", mailserver As String = ""
        Try
            l("modmail---------------")
            l("iminternet: " & iminternet)
            If iminternet Then
                If mailserverinternet = String.Empty Then
                    mailserver = "mail.gmx.net"
                    userid = "j.feinen@gmx.net"
                    userpw = "thuyan19"
                Else
                    mailserver = "mail.gmx.net"
                    userid = "j.feinen@gmx.net"
                    userpw = "thuyan19"
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
            l("mailserver: " & mailserver)
            l("userid: " & userid)
            l("userpw: " & userpw)
            msg = New System.Net.Mail.MailMessage(von, an)
            msg.Subject = betreff
            msg.Body = nachricht
            msg.IsBodyHtml = True
            ' Add a carbon copy recipient.
            If Not String.IsNullOrEmpty(CC) Then
                Dim copie As System.Net.Mail.MailAddress = New System.Net.Mail.MailAddress(CC)
                msg.CC.Add(copie)
            End If
            l("Subject: " & betreff)
            smtp = New System.Net.Mail.SmtpClient(mailserver, 25)
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

            smtp.Credentials = New System.Net.NetworkCredential(von, "snoopy8")
            ' msg.Attachments.Add(New System.Net.Mail.Attachment(anHang)) '
            smtp.Send(msg)
            msg.Dispose()
            hinweis = "ok"
            smtp.Dispose()

        Catch ex As Exception
            ' l("Fehler in mailrausSMTP: " & ex.ToString)
            Return False
        End Try
    End Function

End Module
