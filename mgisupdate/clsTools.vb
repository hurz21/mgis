Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Public Class MyPolicy
    Implements ICertificatePolicy

    Public Function CheckValidationResult(ByVal srvPoint As ServicePoint,
                ByVal cert As X509Certificate, ByVal request As WebRequest,
                ByVal certificateProblem As Integer) _
            As Boolean Implements ICertificatePolicy.CheckValidationResult
        'Return True to force the certificate to be accepted.
        Return True
    End Function
End Class
Public Class clsTools
    Public Shared Property ParadigmaVersion As String = ""
    Public Shared Property serverWeb As String = "http://w2gis02.kreis-of.local"
    Public Shared bplanRootdir As String = serverWeb & "/fkat/bplan" '"http://w2gis02.kreis-of.local/fkat/bplan" '"D:\fkat\bplan" 
    Public Shared Property ProxyString As String = ""
    Public Shared Property ImInternet As Boolean = True
    'Shared Property templist As New List(Of clsBplan)
    Shared Property isWebServerAlive As Boolean = True

    Shared Function down(quellAdresse As String, dateiName As String, zielVerzeichnis As String) As Boolean

        Dim myStringWebResource As String = Nothing
        Dim myWebClient As New WebClient()
        Dim lok As String = ""
        System.Net.ServicePointManager.CertificatePolicy = New MyPolicy()
        Try
            l(" down ---------------------- anfang")
            l("quellAdresse " & quellAdresse)
            l("dateiName " & dateiName)
            l("zielVerzeichnis " & zielVerzeichnis)
            'remoteUri = quelle
            dateiName = dateiName
            myStringWebResource = quellAdresse + dateiName
            createDir(zielVerzeichnis)
            lok = zielVerzeichnis & "\" & dateiName

            myWebClient.DownloadFile(myStringWebResource, lok)
            l(" down ---------------------- ende " & lok)
            Return True
        Catch ex As Exception
            Dim info = "Fehler in down: " &
                ", dest: " & lok &
                ", quellAdresse:" & quellAdresse &
                ", dateiName: " & dateiName &
                ", zielVerzeichnis: " & zielVerzeichnis &
                Environment.NewLine & ex.ToString()
            l(info)
            MsgBox(info)
            Return False
        End Try
        End
    End Function

    Friend Shared Function setIminternetFromLokalFile(dateiImInternet As String) As Boolean
        Dim fi As IO.FileInfo
        Try
            fi = New IO.FileInfo(dateiImInternet)
            If fi.Exists Then
                fi = Nothing
                Return True 'IO.File.ReadAllText(dateiImInternet)
            Else
                Return False
            End If
        Catch ex As Exception
            l("setIminternetFromLokalFile" & ex.ToString)
            Return False
        End Try
    End Function

    Friend Shared Function getProcid(arguments() As String, test As String) As String
        Dim retval As String = ""
        For Each sttelement In arguments
            'MsgBox("sttelement " & sttelement)
            If sttelement.Contains(test) Then
                retval = sttelement.Replace(test, "").Trim.ToLower
                Return retval
            End If
        Next
        Return ""
    End Function

    Shared Sub l(text As String)
        Dim anhang As String = ""
        Try
            My.Log.WriteEntry(text)
            mitFehlerMail(text, anhang)
        Catch ex As Exception

        End Try
    End Sub
    Shared Sub createDir(targetroot As String)
        Try
            l(" createDir ---------------------- anfang" & targetroot)
            'MsgBox("Vor targetroot createdir " & targetroot)
            IO.Directory.CreateDirectory(targetroot)
            l(" createDir ---------------------- ende")
        Catch ex As Exception
            l("Fehler in createDir: " & targetroot) ' & ex.ToString())
            MsgBox("Fehler in createdir  " & targetroot)
        End Try
    End Sub

    Public Shared Sub mitFehlerMail(ByVal text As String, ByVal anhang As String)
        If text.ToLower.StartsWith("fehler") Or text.ToLower.StartsWith("error") Or
            text.ToLower.StartsWith("warnungd") Or text.ToLower.StartsWith("problem") Then
            My.Log.WriteEntry("!!!!  mitFehlerMail: Ein Fehler/Warnung ist aufgetreten !!!! ----------------------------")
            Dim keyword As String
            'MsgBox(text)
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
                                                           betreff:=keyword & " in mgisupdate: " & ", user: " & Environment.UserName,
                                                            nachricht:=text.Replace(vbCrLf, "<br>"),
                                                            anHang:=anhang,
                                                            iminternet:=ImInternet,
                                                            mailserverinternet:="",
                                                            mailserverintranet:="",
                                                            hinweis:="",
                                                            inifile:="",
                                                            CC:="")


        End If
        My.Log.WriteEntry("----------------------------")

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


    Friend Shared Function setIminternet(test As String) As Boolean
        Try
            l(" setIminternet ---------------------- anfang")

            'Dim dsdas = IO.File.ReadAllText(test)
            Dim fi As New IO.FileInfo(test)
            If fi.Exists Then
                fi = Nothing
                Return False
            Else
                fi = Nothing
                Return True
            End If
            l(" setIminternet ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in setIminternet: " & ex.ToString())
            Return True
        End Try
    End Function
    Public Shared Sub setLogfile(modul As String, pfad As String)
        With My.Log.DefaultFileLogWriter
#If DEBUG Then
            '.CustomLocation = mgisUserRoot & "logs\"
#Else
#End If
            '.CustomLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & modul
            '.BaseFileName = modul & "_" & Environment.UserName 
            .CustomLocation = pfad
            .BaseFileName = modul & "_" & Format(Now, "yyyyMMddhhmmss")
            .AutoFlush = True
            .Append = False
        End With
    End Sub
End Class
