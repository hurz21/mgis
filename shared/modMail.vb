Class modMail
    Private Sub New()

    End Sub
    Public Shared Function mailen(iminternet As Boolean, gisuser As clsUser, empfaenger As String, localAnhangFile As String, body As String,
                                  von As String, betreff As String) As Boolean
        Dim mailservername = "", mailserverKonto = "", mailserverPW = "" 'As String
        Dim mailport As Integer = 25, mailssl As Boolean = True
        modMail.getMailCredentials(iminternet, mailservername, mailserverKonto, mailserverPW, mailport, mailssl, gisuser)
        If empfaenger = String.Empty Then
            MessageBox.Show("Sie haben keine Emailadresse angegeben. Sie können dies unter OPTIONEN erledigen.",
                            "Abbruch", MessageBoxButton.OK)
            Return False
        End If
        Dim test As Boolean = modMail.mailrausSMTP(von,
                                                an:=empfaenger,
                                                betreff:=betreff,
                                                nachricht:=body,
                                                anHang:=localAnhangFile,
                                                mailserverName:=mailservername,
                                                mailserverkonto:=mailserverKonto,
                                                mailserverPw:=mailserverPW,
                                                hinweis:="",
                                                inifile:="",
                                                CC:="",
                                                port:=mailport,
                                                ssl:=mailssl)
        Return test
    End Function
    Shared Sub getMailCredentials(Iminternet As Boolean,
                               ByRef mailserverName As String,
                               ByRef mailserverKonto As String,
                               ByRef mailserverPw As String,
                               ByRef port As Integer,
                               ByRef SSL As Boolean,
                                  GisUser As clsUser)
        'nach nutzung prüfen ob noch extra-werte hinterlegt sind (inifile?)
        l("getMailCredentials " & Iminternet)
        Try
            If Iminternet Then
                If GisUser.ichNutzeDenGisserver Then
                    'mailserverName = "mail.gmx.net"
                    ''mailserverKonto = "j.feinen@gmx.net"
                    'mailserverKonto = "giskrof@gmx.net"
                    'mailserverPw = "thuyan19"
                    'GisUser.EmailAdress = mailserverKonto

                    mailserverName = "smtp.gmail.com"
                    'mailserverKonto = "j.feinen@gmx.net"
                    'mailserverKonto = "dr.j.feinen@gmail.com"
                    'mailserverPw = "thuyan19"
                    mailserverKonto = "giskreisoffenbach@gmail.com"
                    mailserverPw = "GKOoekl4rz"
                    GisUser.EmailAdress = mailserverKonto
                    port = 587 '465
                    SSL = True
                Else
                    mailserverName = GisUser.EmailServer
                    mailserverKonto = GisUser.EmailAdress
                    mailserverPw = GisUser.EmailPW
                    port = 465
                    SSL = True
                End If
            Else
                mailserverName = "10.34.102.254"
                mailserverKonto = GisUser.EmailAdress '"dr.j.feinen@kreis-offenbach.de"
                mailserverPw = "vinsan21"
                port = 25
                SSL = False
            End If
        Catch ex As Exception
            l("fehler in getMailCredentials ", ex)
        End Try
    End Sub
    Public Shared Function mailrausSMTP(ByVal von As String,
                                    ByVal an As String,
                                    ByVal betreff As String,
                                    ByVal nachricht As String,
                                    ByVal anHang As String,
                                    ByVal mailserverName As String,
                                    ByVal mailserverkonto As String,
                                    ByVal mailserverPw As String,
                                    ByRef hinweis As String,
                                    ByVal inifile As String,
                                    ByVal CC As String,
                                    ByVal port As Integer,
                                    ByVal ssl As Boolean) As Boolean
        Dim smtp As System.Net.Mail.SmtpClient
        Dim msg As System.Net.Mail.MailMessage
        'Dim userid As String = "", userpw As String = "", mailserver As String = ""
        Try
            l("modmail---------------")
            l("mailserver: " & mailserverName & " userid: " & mailserverkonto & " userpw: " & mailserverPw)
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
            'If mailserverName.ToLower.Contains("gmail") Then
            '    port = 465 '465 ' 587 
            'Else
            '    port = 25
            'End If
            smtp = New System.Net.Mail.SmtpClient(mailserverName, port)
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
            smtp.Port = port
            smtp.Host = mailserverName
            'If Not mailserverkonto.ToLower.Contains("@kreis-offenbach") Then
            smtp.EnableSsl = ssl
            'smtp.ConnectTyp = SmtpConnectType.ConnectSTARTTLS
            'End If

            smtp.Credentials = New System.Net.NetworkCredential(mailserverkonto, mailserverPw)
            'msg.Attachments.Add(New System.Net.Mail.Attachment(anHang)) '
            anhang_handhaben(anHang, msg)
            smtp.Send(msg)
            msg.Dispose()
            hinweis = "ok"
            smtp.Dispose()
            Return True
        Catch ex As Exception
            ' l("Fehler in mailrausSMTP: " ,ex)
            Return False
        End Try
    End Function


    'Public Function mailrausSMTP(ByVal von As String, _
    '                                ByVal an As String, _
    '                                ByVal betreff As String, _
    '                                ByVal nachricht As String, _
    '                                ByVal anHang As String, _
    '                                ByVal iminternet As Boolean, _
    '                                ByVal mailserverinternet As String, _
    '                                ByVal mailserverintranet As String, _
    '                                ByRef hinweis As String, _
    '                                ByVal inifile As String, _
    '                                ByVal CC As String) As Boolean
    '    Dim smtp As System.Net.Mail.SmtpClient
    '    Dim msg As System.Net.Mail.MailMessage
    '    Dim userid As String = "", userpw As String = "", mailserver As String = ""
    '    Try
    '        l("modmail---------------")
    '        l("iminternet: " & iminternet)
    '        If iminternet Then
    '            If mailserverinternet = String.Empty Then
    '                mailserver = "mail.gmx.net"
    '                userid = "j.feinen@gmx.net"
    '                userpw = "thuyan19"
    '            Else
    '                mailserver = "mail.gmx.net"
    '                userid = "j.feinen@gmx.net"
    '                userpw = "thuyan19"
    '            End If
    '        Else
    '            If mailserverintranet = String.Empty Then
    '                mailserver = "10.34.102.254" 'mailserver_intranet
    '                userid = "dr.j.feinen@kreis-offenbach.de"
    '                userpw = "vinsan21"
    '            Else
    '                mailserver = "mail.gmx.net"
    '                userid = "dr.j.feinen@kreis-offenbach.de"
    '                userpw = "vinsan21"
    '            End If
    '        End If
    '        l("mailserver: " & mailserver)
    '        l("userid: " & userid)
    '        l("userpw: " & userpw)
    '        msg = New System.Net.Mail.MailMessage(userid, an)
    '        msg.Subject = betreff
    '        msg.Body = nachricht
    '        msg.IsBodyHtml = True
    '        ' Add a carbon copy recipient.
    '        If Not String.IsNullOrEmpty(CC) Then
    '            Dim copie As System.Net.Mail.MailAddress = New System.Net.Mail.MailAddress(CC)
    '            msg.CC.Add(copie)
    '        End If
    '        l("Subject: " & betreff)
    '        smtp = New System.Net.Mail.SmtpClient(mailserver, 25)
    '        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

    '        If userid = "j.feinen@gmx.net" Then
    '            smtp.EnableSsl = True
    '        End If

    '        smtp.Credentials = New System.Net.NetworkCredential(userid, userpw)
    '        'msg.Attachments.Add(New System.Net.Mail.Attachment(anHang)) '
    '        anhang_handhaben(anHang, msg)
    '        smtp.Send(msg)
    '        msg.Dispose()
    '        hinweis = "ok"
    '        smtp.Dispose()
    '        Return True
    '    Catch ex As Exception
    '        ' l("Fehler in mailrausSMTP: " ,ex)
    '        Return False
    '    End Try
    'End Function
    Private Shared Sub anhang_handhaben(ByRef anHang As String, ByVal msg As System.Net.Mail.MailMessage)
        Try
            If anHang.IsNothingOrEmpty() Then
                Exit Sub
            End If
            If anHang.EndsWith(",") Then anHang = anHang.Substring(0, anHang.Length - 1)
            If anHang.StartsWith(",") Then anHang = anHang.Substring(1, anHang.Length - 1)
            '  nachricht("vor anhang")
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
            nachricht("Fehler bei anhang_handhaben: " & vbCrLf & anHang & vbCrLf, ex)
        End Try
    End Sub
End Class
