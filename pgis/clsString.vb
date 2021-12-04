Public Class clsString
  Public Shared Function normalize_Filename(ByVal opfer$) As String
    Try
      opfer$.Replace("'", "_")
      opfer$.Replace(" ", "_")
      opfer$.Replace(",", "_")
      opfer$.Replace(";", "_")
      opfer$.Replace("<", "_")
      opfer$.Replace(">", "_")
      opfer$.Replace("=", "_")
      opfer$.Replace("/", "_")
      Return opfer
    Catch ex As Exception
      Return ""
    End Try
  End Function
  Public Shared Function Capitalize(ByVal Alt$) As String
    If Alt Is Nothing OrElse Alt.Length < 1 Then Return ""
    Dim a$ = Alt.Substring(0, 1).ToUpper
    Dim b$ = Alt.Substring(1, Alt.Length - 1)
    Return a & b
  End Function


  Public Shared Function noWhiteSpace(ByVal meintext$) As String
    meintext$ = meintext$.Replace(vbCrLf, "")
    meintext$ = meintext$.Replace(vbTab, "")
    meintext$ = meintext$.Replace(vbCr, "")
    meintext$ = meintext$.Replace(vbLf, "")
    Return meintext$
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
                        '  My.Application.Log.WriteEntry(" isinarray fertig und true: ")
                        ' GoTo ende
                        Return True
                    End If
                Next
            End If
ende:
            '  My.Application.Log.WriteEntry(" isinarray fertig und false: ")
            Return False
        Catch ex As Exception
            ' My.Application.Log.WriteEntry("FEHLER isinarray: " & "mapserv ")
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
    '		  My.Application.Log.WriteEntry(Err.Description & vbCrLf & _
    '		 "in dbdirekt.dbdirect.normalize_db " & _
    '		 "at line " & Erl() & _
    '		 "Application Error")
    '		Resume Next
    '	End Function
    Public Shared Function umlaut2ue(ByRef s$) As String
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
            '  My.Application.Log.WriteEntry("Fehler in explode")
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
            Dim FehlerHinweis$ = ", Fehler: " & vbCrLf + _
             ex.Message & " " & vbCrLf + _
             ex.StackTrace & " " & vbCrLf + _
             ex.Source & " "
            '  My.Application.Log.WriteEntry("ERROR /fehler in gemeinsames_element" & FehlerHinweis$)
            '  My.Application.Log.WriteEntry(FehlerHinweis)
            Return False
        End Try
    End Function
  Public Shared Function normalizeSuchdbStrasse(ByVal s1$) As String
    Dim s$
    'alles lowercase
    'alles ohne blank minus punkt
    Try
      s$ = Trim$(LCase$(s1$))
      s$ = s$.Replace("ß", "ss")
      s$ = s$.Replace("str.", "strasse")
      s$ = s$.Replace("str ", "strasse")
      s$ = s$.Replace(" ", "")
      s$ = s$.Replace(".", "")
      s$ = s$.Replace("_", "")
      s$ = s$.Replace("/", "")
      s$ = s$.Replace(":", "")
      s$ = s$.Replace("-", "")
      s$ = s$.Replace("_", "")
      s$ = s$.Replace("adolph", "adolf")
      s$ = s$.Replace("ä", "ae")
      s$ = s$.Replace("ü", "ue")
      s$ = s$.Replace("ö", "oe")
      Return s$
    Catch ex As Exception
            'My.Application.Log.WriteException(ex, "normalizeSuchdbStrasse")
      Return s1
    End Try
  End Function
  Public Shared Function normalize(ByVal s1$) As String
    Dim s$
    s$ = (s1$.ToLower.Trim)
    s$ = s$.Replace("ß", "ss")
    s$ = s$.Replace("str.", "strasse")
    s$ = s$.Replace("str ", "strasse")
    s$ = s$.Replace(" ", "")
    s$ = s$.Replace(".", "")
    s$ = s$.Replace("_", "")
    s$ = s$.Replace("/", "")
    s$ = s$.Replace("\", "")
    s$ = s$.Replace(":", "")
    s$ = s$.Replace("-", "")
    s$ = s$.Replace("_", "")
    s$ = s$.Replace("adolph", "adolf")
    s$ = s$.Replace("ä", "ae")
    s$ = s$.Replace("ü", "ue")
    s$ = s$.Replace("ö", "oe")
    Return s$
  End Function
  Public Shared Function normalizeMetadata(ByVal s1$) As String
    Dim s$
    s$ = s1$.ToLower.Trim
    ' s$ = Replace(s$, "ß", "ss")
    ' s$ = Replace(s$, "str.", "strasse")
    ' s$ = Replace(s$, "str ", "strasse")

    s$ = s$.Replace(".", " ")
    s$ = s$.Replace("_", " ")
    s$ = s$.Replace("/", " ")
    s$ = s$.Replace("\", " ")
    s$ = s$.Replace(":", " ")
    s$ = s$.Replace("-", " ")
    s$ = s$.Replace("_", " ")
    s$ = s$.Replace(",", " ")
    s$ = s$.Replace(";", " ")
    s$ = s$.Replace("adolph", "adolf")
    s$ = s$.Replace("<", ">")
    s$ = s$.Replace("href=", " ")
    s$ = s$.Replace("&nbsp;", " ")
    s$ = s$.Replace("   ", " ")
    s$ = s$.Replace("   ", " ")
    s$ = s$.Replace("  ", " ")
    s$ = addpaddingString(s$, " ")
    Return s$
  End Function
  Public Shared Function addpaddingString(ByVal opfer$, ByVal Padding$) As String

    Return Padding$ & opfer$ & Padding$
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
      Dim messageValue = String.Format("Fehler:(changeUmlaut2Html): {0}<br>{1} {0}<br>{2} {0}<br>{3}<br> ", _
       vbCrLf, e.Message, e.StackTrace, e.Source)

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
      Dim messageValue = String.Format("Fehler:(cleanLayerString): {0}<br>{1} {0}<br>{2} {0}<br>{3}<br> ", _
       vbCrLf, e.Message, e.StackTrace, e.Source)

      Return ">fehler bei der umlaufkonvertierung<"
    End Try
  End Function
  Public Shared Function nodoubleStrings(ByRef ebenen_rein$, ByVal delim As Char) As String
    Dim einal$(), i%, j%, izaehl%
    'keine_doppelten_layerangaben
    Dim test_string = ""
    einal = ebenen_rein.Split(delim)
    'b1& = UBound(einal)
    For i = 0 To UBound(einal)
      izaehl = 0
      For j = 0 To UBound(einal)
        If einal(i) = einal(j) Then
          izaehl = izaehl + 1
          If izaehl > 1 Then
            einal(i) = "-4711" 'doppelte auf 4711 setzen
          End If
        End If
      Next
    Next

    For j = 0 To UBound(einal)
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
  Public Shared Function normalize_layerstring(ByVal ebenen_rein$, ByVal delim As Char) As String
    Dim test_string$
    'Dim delim As Char = ";"c
    Try
      normalize_layerstring = ebenen_rein$

      '1. am anfagn kein semikolon
      '2. am ende ein semikolon
      '3. keine doppelten layerangaben
      If Len(ebenen_rein) < 1 Then Return ""

      ebenen_rein$ = ebenen_rein$.Replace("undefined", "")
      ebenen_rein$ = ebenen_rein$.Replace(delim & delim, delim)

      '1. am anfagn kein semikolon
      If ebenen_rein$.StartsWith(delim) Then
        test_string$ = Right$(ebenen_rein$, Len(ebenen_rein$) - 1)
      Else
        test_string$ = ebenen_rein$
      End If

      ebenen_rein$ = test_string$
      '2. am ende ein semikolon
      If Not ebenen_rein$.EndsWith(delim) Then
        test_string$ = ebenen_rein$ & delim
      End If
      ebenen_rein$ = test_string$

      test_string$ = clsString.nodoubleStrings(ebenen_rein$, delim)

      Return (test_string)

    Catch ex As Exception
            'My.Application.Log.WriteException(ex)
      Return "-1"
    End Try
  End Function
#End Region
  Public Shared Function changeDbaseUmlaute2ASCIIUmlaute(ByVal vorher$) As String
    Try
      Dim nachher$ = vorher
      nachher$ = nachher$.Replace(Chr(132), Chr(196))
      nachher$ = nachher$.Replace(Chr(205), Chr(214))
      nachher$ = nachher$.Replace(Chr(162), Chr(220))
      nachher$ = nachher$.Replace(Chr(245), Chr(228))
      nachher$ = nachher$.Replace(Chr(247), Chr(246))
      nachher$ = nachher$.Replace(Chr(179), Chr(252))
      nachher$ = nachher$.Replace(Chr(175), Chr(223))
      nachher$ = nachher$.Replace(Chr(132), Chr(196))
      nachher$ = nachher$.Replace(Chr(132), Chr(196))
      nachher$ = nachher$.Replace(Chr(132), Chr(196))
      Return nachher$
    Catch ex As Exception
      Return ""
    End Try

  End Function


End Class
