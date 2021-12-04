Imports System.Net
Public Class meineHttpNet
    Shared Function down(fullURI As String, filename As String, targetroot As String) As Boolean
        'https://stackoverflow.com/questions/38140909/webclient-downloadfile-with-clientcertificate

        'Dim myStringWebResource As String = Nothing
        Dim myWebClient As New WebClient()
        Dim lok As String = ""
        Try
            l(" down ---------------------- anfang" & Environment.NewLine &
                   "rootdir " & fullURI & Environment.NewLine &
                   "filename " & filename & Environment.NewLine &
                   "targetroot " & targetroot)

            filename = filename
            'myStringWebResource = fullURI '+ filename 
            createDir(targetroot)
            lok = (targetroot & "\" & filename).Replace("\\", "\")
            myWebClient.DownloadFile(fullURI, lok)
            l(" down ---------------------- ende " & lok)
            Return True
        Catch ex As Exception
            l("Fehler in meineHttpNet down2: filename:" & filename & " targetroot:" & targetroot & " fullURI:" & fullURI &
                 Environment.NewLine & ex.ToString())
            Return False
        End Try
    End Function
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
    Public Shared Function sendjobExtracted(url As String, enc As System.Text.Encoding, zeitInMS As Integer) As String
        Try
            'l("sendjobExtracted -----------------------")
            'l("url-: " & url)
            Dim myWebRequest As System.Net.HttpWebRequest = DirectCast(System.Net.HttpWebRequest.Create(url), System.Net.HttpWebRequest)
            myWebRequest.Method = "GET"
            myWebRequest.Timeout = zeitInMS
            Dim myWebResponse As System.Net.HttpWebResponse = DirectCast(myWebRequest.GetResponse(), System.Net.HttpWebResponse)
            Dim myWebSource As New System.IO.StreamReader(myWebResponse.GetResponseStream(), enc)
            Dim myPageSource As String = String.Empty
            myPageSource = myWebSource.ReadToEnd()
            myWebResponse.Close()
            Return myPageSource
        Catch ex As Exception
            Return "Fehler - server busy?"
        End Try
    End Function
    ''' <summary>
    '''  timeout ist kleiner 1 also wird nicht auf die antwort gewartet
    ''' </summary>
    ''' <param name="proxystring"></param>
    ''' <param name="aufrufstring"></param>
    ''' <param name="hinweis"></param>
    ''' <param name="encode"></param>
    ''' <param name="timeoutinMillisec"></param>
    ''' <returns></returns>
    '''   
    Public Shared Function meinHttpJob(proxystring As String, aufrufstring As String, ByRef hinweis As String,
                                       encode As Text.Encoding,
                                       timeoutinMillisec As Integer) As String
        '     Dim encode As Text.Encoding = System.Text.Encoding.GetEncoding("utf-8") << das war ursprünglich
        'encode = myglobalz.enc jetzt so
        Dim myProxy As WebProxy
        Dim newUri As Uri
        Dim antwort As String = "?"
        Dim wait4job As Boolean = True
        proxystring = proxystring.Trim
        aufrufstring = aufrufstring.Trim
        hinweis = "in httpJob-----------------" & Environment.NewLine
        hinweis &= "in httpJob aufrufstring  " & aufrufstring & Environment.NewLine
        hinweis &= "in httpJob proxystring: " & proxystring & Environment.NewLine
        Try
            myProxy = New WebProxy()
            If String.IsNullOrEmpty(proxystring.Trim) Then
                hinweis &= "proxystring is null" & Environment.NewLine
            Else
                If proxystring.Length < 10 Then
                    hinweis &= "proxystring is zu kurz" & Environment.NewLine
                Else
                    hinweis &= "proxystring is not null-" & Environment.NewLine
                    newUri = New Uri(proxystring)
                    myProxy.Address = newUri
                    myProxy.Credentials = CredentialCache.DefaultCredentials
                    hinweis &= "newUri-" & newUri.ToString & Environment.NewLine
                End If
            End If
            If timeoutinMillisec < 1 Then
                hinweis &= " timeout ist <1 also wird nicht auf die antwort gewartet"
                timeoutinMillisec = 40000
                wait4job = False
                'Return "ok"
            End If
            hinweis &= "vor myreq-" & "" & Environment.NewLine
            Dim myReq As HttpWebRequest = DirectCast(WebRequest.Create(aufrufstring),
                                                     HttpWebRequest)
            myReq.Timeout = timeoutinMillisec
            myReq.Proxy = myProxy
            Dim webResponse As HttpWebResponse
            Try
                hinweis &= "vor webResponse-" & "" & Environment.NewLine
                webResponse = TryCast(myReq.GetResponse(), HttpWebResponse)
            Catch ex As Exception
                hinweis &= " catch  webResponse is fehler" & "" & Environment.NewLine
                hinweis &= ex.ToString & Environment.NewLine
                Return Nothing
            End Try
            hinweis &= "webResponse-" & webResponse.ToString & Environment.NewLine
            If Not wait4job Then Return "ok"
            If webResponse IsNot Nothing Then
                If webResponse.StatusCode = HttpStatusCode.OK Then
                    Dim ReceiveStream As IO.Stream
                    ReceiveStream = webResponse.GetResponseStream()
                    Dim readStream As New IO.StreamReader(ReceiveStream, encode)
                    antwort = readStream.ReadToEnd()
                    readStream.Dispose()
                    ReceiveStream.Dispose()
                    '  hinweis &= "antwort: " & antwort & Environment.NewLine
                Else
                    hinweis &= "webResponse.StatusCode is not ok-" & "" & Environment.NewLine
                End If
            End If
            hinweis &= " vor return antwort: " & Environment.NewLine
            Return antwort
        Catch ex As Exception
            hinweis &= "fehler in meinHttpJob: " & Environment.NewLine
            hinweis &= ex.ToString & Environment.NewLine
            'nachricht("fehler in Mykoordtransform: ", ex)
            Return ""
        End Try
    End Function
    Public Shared Function meinHttpJobdefakt(proxystring As String, aufrufstring As String, ByRef hinweis As String,
                                       encode As Text.Encoding,
                                       timeoutinMillisec As Integer, user As String) As String
        '     Dim encode As Text.Encoding = System.Text.Encoding.GetEncoding("utf-8") << das war ursprünglich
        'encode = myglobalz.enc jetzt so
        Dim myProxy As WebProxy
        Dim myReq As HttpWebRequest
        Dim newUri As Uri
        Dim antwort As String = "?"
        Dim webResponse As HttpWebResponse
        proxystring = proxystring.Trim
        aufrufstring = aufrufstring.Trim
        hinweis = "in httpJob-----------------" & Environment.NewLine
        hinweis &= "in httpJob aufrufstring  " & aufrufstring & Environment.NewLine
        hinweis &= "in httpJob proxystring: " & proxystring & Environment.NewLine
        Try
            myProxy = New WebProxy()
            If String.IsNullOrEmpty(proxystring.Trim) Then
                hinweis &= "proxystring is null" & Environment.NewLine
            Else
                If proxystring.Length < 10 Then
                    hinweis &= "proxystring is zu kurz" & Environment.NewLine
                Else
                    hinweis &= "proxystring is not null-" & Environment.NewLine
                    newUri = New Uri(proxystring)
                    myProxy.Address = newUri
                    myProxy.Credentials = CredentialCache.DefaultCredentials
                    hinweis &= "newUri-" & newUri.ToString & Environment.NewLine
                End If
            End If
            hinweis &= "vor myreq-" & "" & Environment.NewLine
            myReq = DirectCast(WebRequest.Create(aufrufstring), HttpWebRequest)
            If timeoutinMillisec < 1 Then
                hinweis &= " timeout ist <1 also wird nicht auf die antwort gewartet"
                'Return "ok"
            End If

            myReq.Timeout = timeoutinMillisec
            myReq.Proxy = myProxy

            Try
                hinweis &= "vor webResponse-" & "" & Environment.NewLine
                webResponse = TryCast(myReq.GetResponse(), HttpWebResponse)

            Catch ex As Exception
                hinweis &= " catch  webResponse is fehler" & "" & Environment.NewLine
                hinweis &= ex.ToString & Environment.NewLine
                l("fehler1 in mein http" & ex.ToString & aufrufstring & " " & user)
                Return Nothing
            End Try
            hinweis &= "webResponse-" & webResponse.ToString & Environment.NewLine

            If webResponse IsNot Nothing Then
                If webResponse.StatusCode = HttpStatusCode.OK Then
                    Dim ReceiveStream As IO.Stream
                    ReceiveStream = webResponse.GetResponseStream()
                    Dim readStream As New IO.StreamReader(ReceiveStream, encode)
                    antwort = readStream.ReadToEnd()
                    readStream.Dispose()
                    ReceiveStream.Dispose()
                    '  hinweis &= "antwort: " & antwort & Environment.NewLine
                Else
                    hinweis &= "webResponse.StatusCode is not ok-" & "" & Environment.NewLine
                End If
            End If
            hinweis &= " vor return antwort: " & Environment.NewLine
            Return antwort
        Catch ex As Exception
            hinweis &= "fehler in meinHttpJob: " & Environment.NewLine
            hinweis &= ex.ToString & Environment.NewLine
            l("fehler2 in mein http" & ex.ToString & aufrufstring & " " & user)
            'nachricht("fehler in Mykoordtransform: ", ex)
            Return ""
        End Try
    End Function
End Class
