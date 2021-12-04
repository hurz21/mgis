Public Class clsString
    '  Partial Public Class Text
    Public Shared Function Substring(ByVal Text As [String], ByVal Start As Integer, ByVal Len As Integer) As [String]
        Dim str_len As Integer, max_len As Integer
        Dim buffer As [String]

        If Text Is Nothing Then
            Return ""
        End If
        str_len = Text.Length
        If Start >= str_len Then
            Return ""
        End If
        max_len = str_len - Start
        If Len > max_len Then
            Len = max_len
        End If
        buffer = Text.Substring(Start, Len)
        Return buffer
    End Function
    '   End Class
    Public Overloads Shared Function normalize_Filename(ByRef opfer As String, trenner As String) As String
        Try
            '"_"
            opfer = opfer.Replace("'", trenner)
            opfer = opfer.Replace(" ", trenner)
            opfer = opfer.Replace(",", trenner)
            opfer = opfer.Replace(";", trenner)
            opfer = opfer.Replace("<", trenner)
            opfer = opfer.Replace(">", trenner)
            opfer = opfer.Replace("=", trenner)
            opfer = opfer.Replace("/", trenner)
            opfer = opfer.Replace(":", trenner)
            opfer = opfer.Replace(Chr(39), trenner)
            opfer = opfer.Replace(Chr(34), trenner)
            Return opfer.Trim
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Overloads Shared Function normalize_Filename(ByRef opfer As String) As String
        Try
            Return normalize_Filename(opfer, "_")
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Shared Function Capitalize(ByVal Alt As String) As String
        If Alt Is Nothing OrElse Alt.Length < 1 Then Return ""
        Dim a$ = Alt.Substring(0, 1).ToUpper
        Dim b$ = Alt.Substring(1, Alt.Length - 1)
        Return a & b
    End Function

    ''' <summary>
    ''' wird gegen leerstring getauscht
    ''' </summary>
    ''' <param name="meintext"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function noWhiteSpace(ByRef meintext As String) As String
        meintext = meintext.Replace(vbCrLf, "")
        meintext = meintext.Replace(vbTab, "")
        meintext = meintext.Replace(vbCr, "")
        meintext = meintext.Replace(vbLf, "")
        Return meintext
    End Function
    ''' <summary>
    ''' ersatz des whitespce durch neues zeichen , meist leerzeichen
    ''' </summary>
    ''' <param name="meintext"></param>
    ''' <param name="ersatzZeichen"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function noWhiteSpace(ByRef meintext As String, ersatzZeichen As String) As String
        meintext = meintext.Replace(vbCrLf, ersatzZeichen)
        meintext = meintext.Replace(vbTab, ersatzZeichen)
        meintext = meintext.Replace(vbCr, ersatzZeichen)
        meintext = meintext.Replace(vbLf, ersatzZeichen)
        Return meintext
    End Function

    Public Shared Function isinarray(ByVal o$, _
     ByRef t$, _
     ByVal delim$) As Boolean
        'prüft ob t in o als element vorkommt
        Dim i%, s$()
        Try
            isinarray = False
            If Len(o$) > 1 Then
                s = o$.Split(CChar(delim$))
                For i = 0 To CInt(UBound(s$))
                    If t$ = s$(i) Then
                        isinarray = True
                        '  mylog.log(" isinarray fertig und true: ")
                        ' GoTo ende
                        Return True
                    End If
                Next
            End If
ende:
            'mylog.log(" isinarray fertig und false: ")
            Return False
        Catch ex As Exception
            ' mylog.log("FEHLER isinarray: " & "mapserv ")
            Return False
        End Try
    End Function
    '		Public Shared Function normalize_db(ByVal s$) As String
    '		On Error GoTo normalize_db_Err
    '		normalize_db = s$
    '		s$ = Replace$(s$, " ", "_")
    '		s$ = Replace(s$, "ä", "ae")
    '		s$ = Replace(s$, "ü", "ue")
    '		s$ = Replace(s$, "ö", "oe")
    '		s$ = Replace(s$, "Ä", "Ae")
    '		s$ = Replace(s$, "Ü", "Ue")
    '		s$ = Replace(s$, "Ö", "Oe")
    '		s$ = Replace(s$, "ß", "sz")
    '		normalize_db = s$
    '		Exit Function
    'normalize_db_Err:
    '		myLog.log(Err.Description & vbCrLf & _
    '		 "in dbdirekt.dbdirect.normalize_db " & _
    '		 "at line " & Erl() & _
    '		 "Application Error")
    '		Resume Next
    '	End Function
    Public Shared Function umlaut2ue(ByVal s As String) As String ' von byref auf byval umgestellt
        Try
            If String.IsNullOrEmpty(s) Then Return s$
            s$ = s$.Replace("ü", "ue")
            s$ = s$.Replace("ö", "oe")
            s$ = s$.Replace("ä", "ae")
            s$ = s$.Replace("Ä", "Ae")
            s$ = s$.Replace("Ü", "Ue")
            s$ = s$.Replace("Ö", "Oe")
            s$ = s$.Replace("ß", "ss")
            Return s$
        Catch e As Exception
            Return s
        End Try
    End Function
    Public Shared Function explode(ByVal layerstring$, ByVal delim As Char) As String()
        Dim farr$()
        Try
            layerstring$ = layerstring$.Replace(delim & delim, delim)
            layerstring$ = layerstring$.Trim
            If layerstring$.StartsWith(delim) Then
                layerstring$ = layerstring$.Substring(1)
            End If
            If layerstring$.EndsWith(delim) Then
                layerstring$ = layerstring$.Substring(0, layerstring$.Length - 1)
            End If
            farr$ = layerstring$.Split(delim)
            For i = farr.GetUpperBound(0) To 0 Step -1
                If farr$(i) = "" Then
                    ReDim Preserve farr(i - 1)
                End If
            Next
            Return farr$
        Catch ex As Exception
            'mylog.log("Fehler in explode")
            ReDim farr(0)
            Return farr$
        End Try
    End Function
    Public Shared Function gemeinsames_element(ByVal layers$(), ByVal ug$()) As Boolean
        'is true  wenn mind. ein element übereinstimmt
        'isinarray heist es in javascript
        Try
            Dim ilg%, iug%
            gemeinsames_element = False

            For ilg = 0 To UBound(layers)
                For iug = 0 To UBound(ug)
                    If layers(ilg) = ug(iug) Then
                        'treffer zugriff erlaubt
                        Return True
                    End If
                Next
            Next
            Return False
        Catch ex As Exception

            Return False
        End Try
    End Function
    Public Shared Function normalizeSuchdbStrasse(ByRef s1 As String) As String
        Dim s As String
        'alles lowercase
        'alles ohne blank minus punkt
        Try
            s = Trim$(LCase$(s1))
            s = s.Replace("ß", "ss")
            s = s.Replace("str.", "strasse")
            s = s.Replace("str ", "strasse")
            s = s.Replace(" ", "")
            s = s.Replace(".", "")
            s = s.Replace("_", "")
            s = s.Replace("/", "")
            s = s.Replace(":", "")
            s = s.Replace("-", "")
            s = s.Replace("_", "")
            s = s.Replace("adolph", "adolf")
            s = s.Replace("ä", "ae")
            s = s.Replace("ü", "ue")
            s = s.Replace("ö", "oe")
            Return s
        Catch ex As Exception
            'mylog.logException(ex, "normalizeSuchdbStrasse")
            Return s1
        End Try
    End Function
    Public Shared Function normalizeGemeindenamen(ByVal s1 As String) As String
        Dim s As String = (s1.ToLower.Trim)
        s = s.Replace("ß", "ss")
        s = s.Replace("str.", "strasse")
        s = s.Replace("str ", "strasse")
        s = s.Replace(" ", "")
        s = s.Replace(".", "")
        s = s.Replace("_", "")
        s = s.Replace("/", "")
        s = s.Replace("\", "")
        s = s.Replace(":", "")
        s = s.Replace("-", "")
        s = s.Replace("_", "")
        s = s.Replace("adolph", "adolf")
        Return s
    End Function
    Public Shared Function normalize(ByVal s1 As String) As String
        Dim s As String = (s1.ToLower.Trim)
        s = s.Replace("ß", "ss")
        s = s.Replace("str.", "strasse")
        s = s.Replace("str ", "strasse")
        s = s.Replace(" ", "")
        s = s.Replace(".", "")
        s = s.Replace("_", "")
        s = s.Replace("/", "")
        s = s.Replace("\", "")
        s = s.Replace(":", "")
        s = s.Replace("-", "")
        s = s.Replace("_", "")
        s = s.Replace("adolph", "adolf")
        s = s.Replace("ä", "ae")
        s = s.Replace("ü", "ue")
        s = s.Replace("ö", "oe")
        Return s
    End Function
    Public Shared Function normalizeMetadata(ByVal s1 As String) As String
        Dim s As String = s1.ToLower.Trim
        ' s$ = Replace(s$, "ß", "ss")
        ' s$ = Replace(s$, "str.", "strasse")
        ' s$ = Replace(s$, "str ", "strasse")

        s = s.Replace(".", " ")
        s = s.Replace("_", " ")
        s = s.Replace("/", " ")
        s = s.Replace("\", " ")
        s = s.Replace(":", " ")
        s = s.Replace("-", " ")
        s = s.Replace("_", " ")
        s = s.Replace(",", " ")
        s = s.Replace(";", " ")
        s = s.Replace("adolph", "adolf")
        s = s.Replace("<", ">")
        s = s.Replace("href=", " ")
        s = s.Replace("&nbsp;", " ")
        s = s.Replace("   ", " ")
        s = s.Replace("   ", " ")
        s = s.Replace("  ", " ")
        s = addpaddingString(s, " ")
        Return s
    End Function
    Public Shared Function addpaddingString(ByVal opfer As String, ByVal Padding As String) As String

        Return Padding & opfer & Padding
    End Function

    Public Shared Function changeUmlaut2Html(ByRef s As String) As String
        Try
            s = Replace(s, "ü", "&uuml")
            s = Replace(s, "ö", "&ouml")
            s = Replace(s, "ä", "&auml")
            s = Replace(s, "ß", "&szlig;")
            s = Replace(s, "Ä", "&Auml")
            s = Replace(s, "Ü", "&Uuml")
            Return s
        Catch e As Exception
            'Dim messageValue = String.Format("Fehler:(changeUmlaut2Html): {0}<br>{1} {0}<br>{2} {0}<br>{3}<br> ", _
            ' vbCrLf, e.Message, e.StackTrace, e.Source)
            Return ">fehler bei der umlaufkonvertierung<"
        End Try
    End Function
#Region " Layerstring "
    Public Shared Function cleanLayerString(ByRef s As String) As String
        Try
            s = s.Trim
            s = s.Replace(";;", ";")
            s = s.Replace(vbCrLf, "")
            s = s.Replace(vbLf, "")
            s = s.Replace(vbTab, "")
            s = s.Replace(vbCr, "")
            Return s
        Catch e As Exception
            Return ">fehler bei der umlaufkonvertierung<"
        End Try
    End Function

    Public Shared Function nodoubleStrings(ByRef ebenen_rein As String, ByVal delim As Char) As String
        Dim i, j, izaehl As Int16
        'keine_doppelten_layerangabe
        Dim test_string = ""
        Dim einal As String() = ebenen_rein.Split(delim)
        'b1& = UBound(einal)
        For i = 0 To CShort(UBound(einal))
            izaehl = 0
            For j = 0 To CShort(UBound(einal))
                If einal(i) = einal(j) Then
                    izaehl = CShort(izaehl + 1)
                    If izaehl > 1 Then
                        einal(i) = "-4711" 'doppelte auf 4711 setzen
                    End If
                End If
            Next
        Next

        For j = 0 To CShort(UBound(einal))
            If einal(j) <> "-4711" And einal(j) <> "" Then
                test_string = test_string & einal(j) & delim
            End If
        Next
        ebenen_rein = ebenen_rein.Replace(delim & delim, delim)
        Return test_string
    End Function

    Public Shared Function normalize_layerstring(ByVal ebenen_rein$) As String
        Dim Delim As Char
        ebenen_rein.Replace(Chr(39), "")
        ebenen_rein.Replace(Chr(34), "")

        If ebenen_rein.Contains(";") Then Delim = ";"c
        If ebenen_rein.Contains(",") Then Delim = ","c
        If Delim <> ";"c And Delim <> ","c Then
            Return ebenen_rein
        End If


        Return normalize_layerstring(ebenen_rein, Delim)
    End Function
    Public Shared Function normalize_layerstring(ByVal ebenen_edukt As String, ByVal delim As Char) As String
        Dim test_string As String
        'Dim delim As Char = ";"c
        Try
            normalize_layerstring = ebenen_edukt

            '1. am anfagn kein semikolon
            '2. am ende ein semikolon
            '3. keine doppelten layerangaben
            If Len(ebenen_edukt) < 1 Then Return ""

            ebenen_edukt = ebenen_edukt.Replace("undefined", "")
            ebenen_edukt = ebenen_edukt.Replace(delim & delim, delim)

            '1. am anfagn kein semikolon
            If ebenen_edukt.StartsWith(delim) Then
                test_string = Right$(ebenen_edukt, Len(ebenen_edukt) - 1)
            Else
                test_string = ebenen_edukt
            End If

            ebenen_edukt = test_string
            '2. am ende ein semikolon
            If Not ebenen_edukt.EndsWith(delim) Then
                test_string = ebenen_edukt & delim
            End If
            ebenen_edukt = test_string

            test_string = clsString.nodoubleStrings(ebenen_edukt, delim)

            Return (test_string)

        Catch ex As Exception
            'mylog.fehlerReport(ex)
            Return "-1"
        End Try
    End Function
#End Region
    Public Shared Function changeDbaseUmlaute2ASCIIUmlaute(ByVal vorher As String) As String
        Try
            Dim nachher As String = vorher
            nachher = nachher.Replace(Chr(132), Chr(196))
            nachher = nachher.Replace(Chr(205), Chr(214))
            nachher = nachher.Replace(Chr(162), Chr(220))
            nachher = nachher.Replace(Chr(245), Chr(228))
            nachher = nachher.Replace(Chr(247), Chr(246))
            nachher = nachher.Replace(Chr(179), Chr(252))
            nachher = nachher.Replace(Chr(175), Chr(223))
            nachher = nachher.Replace(Chr(132), Chr(196))
            nachher = nachher.Replace(Chr(132), Chr(196))
            nachher = nachher.Replace(Chr(132), Chr(196))
            Return nachher
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Shared Function changeHTML2text(ByRef s As String) As String
        Try
            s = Replace(s, "<br><br><br><br>", vbCrLf)
            s = Replace(s, "<br><br><br>", vbCrLf)
            s = Replace(s, "<br><br>", vbCrLf)
            s = Replace(s, "<br>", vbCrLf)
            'Dim as$ = vbCrLf & vbCrLf
            s = Replace(s, "as$", vbCrLf)
            Return s
        Catch e As Exception
            Dim messageValue = String.Format("Fehler:(changeUmlaut2Html): {0}<br>{1} {0}<br>{2} {0}<br>{3}<br> ", _
             vbCrLf, e.Message, e.StackTrace, e.Source)

            Return ">fehler bei der umlaufkonvertierung<" & messageValue
        End Try
    End Function
    ''' <summary>
    ''' entfernt den buchstabe am anfang eines wortes
    ''' </summary>
    ''' <param name="mater"></param>
    ''' <param name="buchstabe"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function removeLeadingChar(ByVal mater As String, ByVal buchstabe As String) As String
        Try
            If String.IsNullOrEmpty(mater) Then
                Return ""
            End If
            If String.IsNullOrEmpty(buchstabe) Then
                Return mater
            End If
            If mater.StartsWith(buchstabe) Then
                'remove buchstabe
                mater = mater.Substring(1, mater.Length - 1)
            End If
            Return mater
        Catch ex As Exception
            Return "fehler beim berechnen des anhangs"
        End Try
    End Function

    Shared Function enthaeltUnerlaubteZeichen(a As String) As Boolean
        '< > ? " : | \ / *
        If String.IsNullOrEmpty(a) Then
            Return False
        End If
        If a.Contains("<") Or
           a.Contains(">") Or
           a.Contains("?") Or
           a.Contains(":") Or
           a.Contains("|") Or
           a.Contains("\") Or
           a.Contains("/") Or
           a.Contains("*") Or
           a.Contains(Chr(34)) Or
           a.Contains(Chr(39)) Then
            Return True
        End If
        Return False
    End Function

    Shared Function date2string(ddd As Date, modus As Integer) As String
        Try
            Select Case modus
                Case 1
                    Return ddd.ToString("yyyy-MM-dd_HH_mm_ss").Trim
                Case 2
                    Return ddd.ToString("yyyyMMdd_HHmmss").Trim
                Case 3
                    Return ddd.ToString("yyyyMMdd").Trim
                Case 4
                    Return ddd.ToString("dd.MM.yyyy").Trim
            End Select
            Return "???"
        Catch ex As Exception
            Return "???"
        End Try
    End Function

    Public Shared Function kuerzeTextauf(ByVal candidate As String, ByVal maxlen As Int16) As String
        Try
            If candidate Is Nothing Then Return ""
            If candidate.Length > maxlen Then
                candidate = candidate.Substring(0, maxlen - 1)
                Return candidate
            End If
            Return candidate
        Catch ex As Exception
            'glob2.nachricht_und_Mbox(ex.ToString)
            Return "???"
        End Try
    End Function
End Class
