Imports System.Net
Public Class meineHttpNet
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

    Public Shared Function meinHttpJob(proxystring As String, aufrufstring As String, ByRef hinweis As String) As String
        Dim myProxy As WebProxy
        Dim newUri As Uri
        Dim antwort As String = "?"
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
            Dim myReq As HttpWebRequest = DirectCast(WebRequest.Create(aufrufstring),
                                                     HttpWebRequest)
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
            Dim encode As Text.Encoding = System.Text.Encoding.GetEncoding("utf-8")
            If webResponse IsNot Nothing Then
                If webResponse.StatusCode = HttpStatusCode.OK Then
                    Dim ReceiveStream As IO.Stream = webResponse.GetResponseStream()
                    Dim readStream As New IO.StreamReader(ReceiveStream, encode)
                    antwort = readStream.ReadToEnd()
                    readStream.Dispose()
                    ReceiveStream.Dispose()
                    hinweis &= "antwort: " & antwort & Environment.NewLine
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
End Class
