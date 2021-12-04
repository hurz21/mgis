Public Class clsCGI4VBNET
	'Public enc As Text.Encoding = Text.Encoding.GetEncoding("8859")
	' environment variables
	'
	Public enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("iso-8859-1")
	Public CGI_Accept As String
	Public CGI_AuthType As String
	Public CGI_ContentLength As String
	Public CGI_ContentType As String
	Public CGI_Cookie As String
	Public CGI_GatewayInterface As String
	Public CGI_PathInfo As String
	Public CGI_PathTranslated As String
	Public CGI_QueryString As String
	Public CGI_Referer As String
	Public CGI_RemoteAddr As String
	Public CGI_RemoteHost As String
	Public CGI_RemoteIdent As String
	Public CGI_RemoteUser As String
	Public CGI_RequestMethod As String
	Public CGI_ScriptName As String
	Public CGI_ServerSoftware As String
	Public CGI_ServerName As String
	Public CGI_ServerPort As String
	Public CGI_ServerProtocol As String
	Public CGI_UserAgent As String
	Public lContentLength As Long		' CGI_ContentLength converted to Long

	Public sErrorDesc As String	' constructed error message
	Public sFormData As String ' url-encoded data sent by the server

	Structure pair
		Dim Name As String
		Dim Value As String
	End Structure
	' array of name=value pairs
	Public tPair() As pair
	' Private sEmailValue As String ' webmaster's/your email address
    Public sEmail As String


    Public Sub New(ByVal sEmailValue$)
        Try

            InitCgi()                    ' Load environment vars and perform other initialization
            GetFormData()            ' Read data sent by the server
            'If Logging Then My.Application.Log.WriteEntry("APP: " + "db" + " Date: " + Now.ToString)
            sEmail = sEmailValue

EndPgm:
        Catch e As Exception
            sErrorDesc = ", Fehler: " & vbCrLf + _
             e.Message + " " & vbCrLf + _
             e.StackTrace + " " & vbCrLf + _
             e.Source + " "
            '  If Logging Then My.Application.Log.WriteEntry(sErrorDesc)
        End Try
    End Sub
    Public Alive As Boolean
    Private Sub InitCgi()
        Try '==============================
            ' Get the environment variables
            '==============================
            '
            ' Environment variables will vary depending on the server.
            ' Replace any variables below with the ones used by your server.
            '
            CGI_Accept = System.Environment.GetEnvironmentVariable("HTTP_ACCEPT")
            CGI_AuthType = System.Environment.GetEnvironmentVariable("AUTH_TYPE")
            CGI_ContentLength = System.Environment.GetEnvironmentVariable("CONTENT_LENGTH")
            CGI_ContentType = System.Environment.GetEnvironmentVariable("CONTENT_TYPE")
            CGI_Cookie = System.Environment.GetEnvironmentVariable("HTTP_COOKIE")
            CGI_GatewayInterface = System.Environment.GetEnvironmentVariable("GATEWAY_INTERFACE")
            CGI_PathInfo = System.Environment.GetEnvironmentVariable("PATH_INFO")
            CGI_PathTranslated = System.Environment.GetEnvironmentVariable("PATH_TRANSLATED")
            CGI_QueryString = System.Environment.GetEnvironmentVariable("QUERY_STRING")
            CGI_Referer = System.Environment.GetEnvironmentVariable("HTTP_REFERER")
            CGI_RemoteAddr = System.Environment.GetEnvironmentVariable("REMOTE_ADDR")
            CGI_RemoteHost = System.Environment.GetEnvironmentVariable("REMOTE_HOST")
            CGI_RemoteIdent = System.Environment.GetEnvironmentVariable("REMOTE_IDENT")
            CGI_RemoteUser = System.Environment.GetEnvironmentVariable("REMOTE_USER")
            CGI_RequestMethod = System.Environment.GetEnvironmentVariable("REQUEST_METHOD")
            CGI_ScriptName = System.Environment.GetEnvironmentVariable("SCRIPT_NAME")
            CGI_ServerSoftware = System.Environment.GetEnvironmentVariable("SERVER_SOFTWARE")
            CGI_ServerName = System.Environment.GetEnvironmentVariable("SERVER_NAME")
            CGI_ServerPort = System.Environment.GetEnvironmentVariable("SERVER_PORT")
            CGI_ServerProtocol = System.Environment.GetEnvironmentVariable("SERVER_PROTOCOL")
            CGI_UserAgent = System.Environment.GetEnvironmentVariable("HTTP_USER_AGENT")
            lContentLength = CLng(CGI_ContentLength)     'convert to long
            My.Application.Log.WriteEntry("CGI_RequestMethod CGI_RequestMethod " & CGI_RequestMethod)
        Catch e As Exception
            sErrorDesc = ", Fehler: " & vbCrLf + _
             e.Message + " " & vbCrLf + _
             e.StackTrace + " " & vbCrLf + _
             e.Source + " "
            ' If Logging Then My.Application.Log.WriteEntry(sErrorDesc)
        End Try
    End Sub

    Private Sub GetFormData()
        Dim sBuff As String = ""         ' buffer to receive POST method data
        Try
            My.Application.Log.WriteEntry("CGI_RequestMethod: " & CGI_RequestMethod)
            My.Application.Log.WriteEntry("sFormData: " & sFormData)
            My.Application.Log.WriteEntry("CGI_QueryString: " & CGI_QueryString)

            If CGI_RequestMethod = "POST" Then

                sBuff = CStr(lContentLength) + Chr(0)
                Do While Len(sFormData) < lContentLength
                    sBuff$ = Console.ReadLine
                    sFormData = sFormData & (sBuff)
                Loop

                If String.IsNullOrEmpty(sFormData) Then
                    StorePairs(CGI_QueryString)
                    Exit Sub
                End If
                ' Make sure posted data is url-encoded
                ' Multipart content types, for example, are not necessarily encoded.
                '
                If CBool(InStr(1, CGI_ContentType, "www-form-urlencoded", CompareMethod.Text)) Then
                    StorePairs(sFormData)
                End If
            Else
                StorePairs(CGI_QueryString)
            End If

        Catch e As Exception
            sErrorDesc = ", Fehler: " & vbCrLf + _
             e.Message + " " & vbCrLf + _
             e.StackTrace + " " & vbCrLf + _
             e.Source + " "
            ' If Me.Logging Then My.Application.Log.WriteEntry(sErrorDesc)
        End Try
    End Sub
	Private Sub StorePairs(ByRef sData As String)
		'=====================================================================
		' Parse and decode form data and/or query string
		' Data is received from server as "name=value&name=value&...name=value"
		' Names and values are URL-encoded
		'
		' Store name/value pairs in array tPair(), and decode them
		'
		' Note: if an element in the query string does not contain an "=",
		'       then it will not be stored.
		'
		' /cgi-bin/pgm.exe?parm=1   "1" gets stored and can be
		'                               retrieved with getCgiValue("parm")
		' /cgi-bin/pgm.exe?1        "1" does not get stored, but can be
		'                               retrieved with urlDecode(CGI_QueryString)
		'
		'======================================================================
		Dim Pointer As Integer ' sData position pointer
		Dim n As Integer ' name/value pair counter
		Dim delim1 As Integer	' position of "="
		Dim delim2 As Integer	' position of "&"
		Dim lastPair As Integer	' size of tPair() array
		Dim lPairs As Integer	' number of name=value pairs in sData
		Try
			lastPair = 0 'UBound(tPair) ' current size of tPair()            
			delim1 = 0
			Do
				delim1 = InStr(delim1 + 1, sData, "=")
				If delim1 = 0 Then Exit Do
				lPairs = lPairs + 1
			Loop

			If lPairs = 0 Then Exit Sub 'nothing to add

			' redim tPair() based on the number of pairs found in sData
			ReDim Preserve tPair(lastPair + lPairs)

			' assign values to tPair().name and tPair().value
			Pointer = 1
			For n = (lastPair + 1) To UBound(tPair)
				delim1 = InStr(Pointer, sData, "=")	' find next equal sign
				If delim1 = 0 Then Exit For ' parse complete

				tPair(n).Name = UrlDecode(Mid(sData, Pointer, delim1 - Pointer))

				delim2 = InStr(delim1, sData, "&")

				' if no trailing ampersand, we are at the end of data
				If delim2 = 0 Then delim2 = Len(sData) + 1

				' value is between the "=" and the "&"
				tPair(n).Value = UrlDecode(Mid(sData, delim1 + 1, delim2 - delim1 - 1))
				Pointer = delim2 + 1
			Next n
		Catch e As Exception
			sErrorDesc = ", Fehler: " & vbCrLf + _
			 e.Message + " " & vbCrLf + _
			 e.StackTrace + " " & vbCrLf + _
			 e.Source + " "
		End Try
	End Sub
	Private Function UrlDecode(ByVal sEncoded As String) As String
		UrlDecode = sEncoded
		'========================================================
		' Accept url-encoded string
		' Return decoded string
		'========================================================

		Dim pos As Integer			' position of InStr target

		If sEncoded = "" Then Return ""

		' convert "+" to space
		'pos = 0
		'Do
		'	pos = InStr(pos + 1,  sEncoded,   "+")
		'	If pos = 0 Then Exit Do
		'	Mid$(sEncoded, pos, 1) = " "
		'Loop
		sEncoded = sEncoded.Replace("+", " ")
		' convert "%xx" to character
		pos = 0
		Try
			Do
				pos = InStr(pos + 1, sEncoded, "%")
				If pos = 0 Then Exit Do

				Mid$(sEncoded, pos, 1) = Chr(CInt("&H" & (Mid(sEncoded, pos + 1, 2))))
				sEncoded = Left$(sEncoded, pos) _
				 & Mid$(sEncoded, pos + 3)
			Loop

			Return sEncoded

		Catch e As Exception
			sErrorDesc = ", Fehler: " & vbCrLf + _
			 e.Message + " " & vbCrLf + _
			 e.StackTrace + " " & vbCrLf + _
			 e.Source + " "
		End Try
	End Function

	Public Function GetCgiValue(ByVal cgiName As String) As String
		'====================================================================
		' Accept the name of a pair
		' Return the value matching the name
		'
		' tPair(0) is always empty.
		' An empty string will be returned
		'    if cgiName is not defined in the form (programmer error)
		'    or, a select type form item was used, but no item was selected.
		'
		' Multiple values, separated by a semi-colon, will be returned
		'     if the form item uses the "multiple" option
		'     and, more than one selection was chosen.
		'     The calling procedure must parse this string as needed.
		'====================================================================
		Dim n As Integer
		Try
			'GetCgiValue = cgiName
			GetCgiValue = ""
			If tPair Is Nothing Then Return ""
			For n = 1 To UBound(tPair)
				If UCase$(cgiName) = UCase$(tPair(n).Name) Then
					If GetCgiValue = "" Then
						GetCgiValue = tPair(n).Value
					Else						 ' allow for multiple selections
						GetCgiValue = GetCgiValue & ";" & tPair(n).Value
					End If
				End If
			Next
			' GetCgiValue = cgiName
		Catch e As Exception
			sErrorDesc = ", Fehler: " & vbCrLf + _
			 e.Message + " " & vbCrLf + _
			 e.StackTrace + " " & vbCrLf + _
			 e.Source + " "
			GetCgiValue = ""
		End Try
	End Function
	Public Function Send(ByVal s As String) As Boolean
		'======================
		'myCGI.Send output to STDOUT
		'======================
		s = s & vbCrLf
		Console.WriteLine(s$)
		Return True
	End Function
	Public Function Send(ByVal s As Double) As Boolean
		'======================
		'myCGI.Send output to STDOUT
		'======================
		Dim a$ = s.ToString & vbCrLf
		Console.WriteLine(a$)
		Return True
	End Function

	Public Function SendHeaderAJAX() As Boolean
		'Console.WriteLine("Status: 200 OK" & vbCrLf)
		Console.WriteLine("Content-type: text/html; charset=ISO-8859-1" & vbCrLf)
		Return True
	End Function
	Public Function SendHeader(ByVal sTitle As String) As Boolean
		Console.WriteLine("Status: 200 OK")
		Console.WriteLine("Content-type: text/html" & vbCrLf)
		Console.WriteLine("<HTML><HEAD><TITLE>" & sTitle & "</TITLE></HEAD>")
		Console.WriteLine("<BODY>")
		Return True
	End Function

	Public Function SendFooter() As Boolean
		'==================================
		' standardized footers can be added
		'==================================
		Console.WriteLine("</BODY></HTML>")
		Return True
	End Function

	Public Function SendB(ByVal s As String) As Boolean
		'============================================
		'Send output to STDOUT without vbCrLf.
		' Use whenmyCGI.Sending binary data. For example,
		' images sent with "Content-type image/jpeg".
		'============================================
        Console.Write(s$)
        Return True
	End Function

End Class
