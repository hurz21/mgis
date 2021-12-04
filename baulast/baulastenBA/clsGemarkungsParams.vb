
Public Class clsGemarkungsParams
    'Dim gemparms As New clsGemarkungsParams
    'gemparms.init() : Dim result$ = "ERROR"
    '	Dim a = From item In gemparms.parms Where item.gemarkungstext.ToLower = "disetesheim" Select item.gemeindetext
    '	If a.ToArray.Length > 0 Then result$ = a.ToList(0).ToString
    Public parms As New List(Of clsGemarkungsParams)
    Property gemarkungstext() As String
    Property gemeindetext() As String 
    Property gemcode() As Integer
    Property gemeindenr() As Integer
    Property gemarkungskuerzel As String
    Property gemarkungsPLZ() As String
    Sub init()
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Buchschlag", .gemeindetext = "Dreieich", .gemcode = 726, .gemeindenr = 2, .gemarkungskuerzel = "BC", .gemarkungsPLZ = "63303"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Dietesheim", .gemeindetext = "Mühlheim", .gemcode = 728, .gemeindenr = 8, .gemarkungskuerzel = "DI", .gemarkungsPLZ = "63165"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Dietzenbach", .gemeindetext = "Dietzenbach", .gemcode = 729, .gemeindenr = 1, .gemarkungskuerzel = "DB", .gemarkungsPLZ = "63128"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Dreieichenhain", .gemeindetext = "Dreieich", .gemcode = 730, .gemeindenr = 2, .gemarkungskuerzel = "DR", .gemarkungsPLZ = "63303"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Dudenhofen", .gemeindetext = "Rodgau", .gemcode = 731, .gemeindenr = 11, .gemarkungskuerzel = "DU", .gemarkungsPLZ = "63110"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Egelsbach", .gemeindetext = "Egelsbach", .gemcode = 732, .gemeindenr = 3, .gemarkungskuerzel = "EG", .gemarkungsPLZ = "63329"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Froschhausen", .gemeindetext = "Seligenstadt", .gemcode = 733, .gemeindenr = 13, .gemarkungskuerzel = "FR", .gemarkungsPLZ = "63500"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Götzenhain", .gemeindetext = "Dreieich", .gemcode = 734, .gemeindenr = 2, .gemarkungskuerzel = "GH", .gemarkungsPLZ = "63303"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Hainhausen", .gemeindetext = "Rodgau", .gemcode = 735, .gemeindenr = 11, .gemarkungskuerzel = "HI", .gemarkungsPLZ = "63110"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Hainstadt", .gemeindetext = "Hainburg", .gemcode = 736, .gemeindenr = 4, .gemarkungskuerzel = "HA", .gemarkungsPLZ = "63512"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Hausen", .gemeindetext = "Obertshausen", .gemcode = 737, .gemeindenr = 10, .gemarkungskuerzel = "HN", .gemarkungsPLZ = "63179"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Heusenstamm", .gemeindetext = "Heusenstamm", .gemcode = 738, .gemeindenr = 5, .gemarkungskuerzel = "HM", .gemarkungsPLZ = "63150"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Jügesheim", .gemeindetext = "Rodgau", .gemcode = 739, .gemeindenr = 11, .gemarkungskuerzel = "JH", .gemarkungsPLZ = "63110"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Klein-Krotzenburg", .gemeindetext = "Hainburg", .gemcode = 740, .gemeindenr = 4, .gemarkungskuerzel = "KR", .gemarkungsPLZ = "63512"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Klein-Welzheim", .gemeindetext = "Seligenstadt", .gemcode = 741, .gemeindenr = 13, .gemarkungskuerzel = "KW", .gemarkungsPLZ = "63500"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Lämmerspiel", .gemeindetext = "Mühlheim", .gemcode = 742, .gemeindenr = 8, .gemarkungskuerzel = "LM", .gemarkungsPLZ = "63165"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Langen", .gemeindetext = "Langen", .gemcode = 743, .gemeindenr = 6, .gemarkungskuerzel = "LA", .gemarkungsPLZ = "63225"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Mainflingen", .gemeindetext = "Mainhausen", .gemcode = 744, .gemeindenr = 7, .gemarkungskuerzel = "MA", .gemarkungsPLZ = "63533"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Messenhausen", .gemeindetext = "Rödermark", .gemcode = 745, .gemeindenr = 12, .gemarkungskuerzel = "ME", .gemarkungsPLZ = "63322"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Mühlheim", .gemeindetext = "Mühlheim", .gemcode = 746, .gemeindenr = 8, .gemarkungskuerzel = "MU", .gemarkungsPLZ = "63165"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Mühlheim am main", .gemeindetext = "Mühlheim am main", .gemcode = 746, .gemeindenr = 8, .gemarkungskuerzel = "MU", .gemarkungsPLZ = "63165"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Nieder-Roden", .gemeindetext = "Rodgau", .gemcode = 747, .gemeindenr = 11, .gemarkungskuerzel = "NR", .gemarkungsPLZ = "63110"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Neu-Isenburg", .gemeindetext = "Neu-Isenburg", .gemcode = 748, .gemeindenr = 9, .gemarkungskuerzel = "NE", .gemarkungsPLZ = "63263"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Ober-Roden", .gemeindetext = "Rödermark", .gemcode = 749, .gemeindenr = 12, .gemarkungskuerzel = "OR", .gemarkungsPLZ = "63322"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Obertshausen", .gemeindetext = "Obertshausen", .gemcode = 750, .gemeindenr = 10, .gemarkungskuerzel = "OB", .gemarkungsPLZ = "63179"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Offenthal", .gemeindetext = "Dreieich", .gemcode = 752, .gemeindenr = 2, .gemarkungskuerzel = "OT", .gemarkungsPLZ = "63303"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Rembrücken", .gemeindetext = "Heusenstamm", .gemcode = 753, .gemeindenr = 5, .gemarkungskuerzel = "RE", .gemarkungsPLZ = "63150"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Seligenstadt", .gemeindetext = "Seligenstadt", .gemcode = 755, .gemeindenr = 13, .gemarkungskuerzel = "SE", .gemarkungsPLZ = "63500"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Sprendlingen", .gemeindetext = "Dreieich", .gemcode = 756, .gemeindenr = 2, .gemarkungskuerzel = "SP", .gemarkungsPLZ = "63303"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Urberach", .gemeindetext = "Rödermark", .gemcode = 757, .gemeindenr = 12, .gemarkungskuerzel = "UR", .gemarkungsPLZ = "63322"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Weiskirchen", .gemeindetext = "Rodgau", .gemcode = 758, .gemeindenr = 11, .gemarkungskuerzel = "WE", .gemarkungsPLZ = "63110"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Zellhausen", .gemeindetext = "Mainhausen", .gemcode = 759, .gemeindenr = 7, .gemarkungskuerzel = "ZE", .gemarkungsPLZ = "63533"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Zeppelinheim", .gemeindetext = "Neu-Isenburg", .gemcode = 760, .gemeindenr = 9, .gemarkungskuerzel = "ZH", .gemarkungsPLZ = "63263"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Forstrettungspunkt", .gemeindetext = "Forstrettungspunkt", .gemcode = 0, .gemeindenr = 23, .gemarkungskuerzel = "FRP", .gemarkungsPLZ = "0"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Groß-Steinheim", .gemeindetext = "Hanau", .gemcode = 999, .gemeindenr = 14, .gemarkungskuerzel = "HGR", .gemarkungsPLZ = "0"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Offenbach", .gemeindetext = "Offenbach", .gemcode = 751, .gemeindenr = 15, .gemarkungskuerzel = "OF", .gemarkungsPLZ = "63065"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Bieber", .gemeindetext = "Offenbach", .gemcode = 725, .gemeindenr = 16, .gemarkungskuerzel = "OFI", .gemarkungsPLZ = "63065"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Bürgel", .gemeindetext = "Offenbach", .gemcode = 727, .gemeindenr = 17, .gemarkungskuerzel = "OFU", .gemarkungsPLZ = "63065"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Rumpenheim", .gemeindetext = "Offenbach", .gemcode = 754, .gemeindenr = 18, .gemarkungskuerzel = "OFR", .gemarkungsPLZ = "63065"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Frankfurt", .gemeindetext = "Frankfurt", .gemcode = 998, .gemeindenr = 19, .gemarkungskuerzel = "FFM", .gemarkungsPLZ = "63000"})
        parms.Add(New clsGemarkungsParams With {.gemarkungstext = "Neu-Isenburg", .gemeindetext = "NeuIsenburg", .gemcode = 748, .gemeindenr = 9, .gemarkungskuerzel = "NE", .gemarkungsPLZ = "63263"})

    End Sub
    Public Function gemarkungstext2gemcode(ByVal gemarkungstext as string) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemarkungstext.ToLower = gemarkungstext$.ToLower Select item.gemcode
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function
    Public Function gemarkungstext2gemeindetext(ByVal gemarkungstext as string) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemarkungstext.ToLower = gemarkungstext$.ToLower Select item.gemeindetext
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function
    Public Function gemarkungstext2gemeindenr(ByVal gemarkungstext as string) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemarkungstext.ToLower = gemarkungstext$.ToLower Select item.gemeindenr
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function

    Public Function gemcode2gemarkungstext(ByVal gemcode as integer) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemcode.ToString.ToLower = gemcode%.ToString.ToLower Select item.gemarkungstext
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function
    Public Function gemcode2gemarkungsplz(ByVal gemcode as integer) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemcode.ToString.ToLower = gemcode%.ToString.ToLower Select item.gemarkungsPLZ
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function
    Public Function gemcode2gemeindetext(ByVal gemcode as integer) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemcode.ToString.ToLower = gemcode%.ToString.ToLower Select item.gemeindetext
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function
    Public Function gemcode2gemeindenr(ByVal gemcode as integer) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemcode.ToString.ToLower = gemcode%.ToString.ToLower Select item.gemeindenr
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function
    Public Function gemeindetext2gemeindenr(ByVal gemeindetext as string) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemeindetext.ToString.ToLower = gemeindetext.ToLower Select item.gemeindenr
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function
    Public Function gemarkungsPLZ2gemeindetext(ByVal gemarkungsPLZ as string) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemarkungsPLZ.ToString.ToLower = gemarkungsPLZ.ToLower Select item.gemeindetext
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function
    Public Function gemeindenr2gemeindetext(ByVal gemeindenr as string) as  String
        Dim result$ : init()
        Dim a = From item In parms Where item.gemeindenr.ToString.ToLower = gemeindenr.ToString.ToLower Select item.gemeindetext
        If a.ToArray.Length > 0 Then
            result$ = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return result
    End Function

    Public Function gemeindetext2PLZ(ByVal gemeindetext as string) as  String ' geändert von integer auf string wg ddr und urbanke
        Dim result As String : init()
        Dim a = From item In parms Where item.gemeindetext.ToString.ToLower = gemeindetext.ToLower Select item.gemarkungsPLZ
        If a.ToArray.Length > 0 Then
            result = a.ToList(0).ToString
        Else
            result = "ERROR"
        End If
        Return (result)
    End Function


    Public Function nummer2gemeindename(ByVal nr as string) as  String
        Dim nri% = CInt(nr$)
        Try
            'ist des die PLZ?
            If nri% > 60000 And nri% < 70000 Then
                'ist wohl eine POSTLEITZAHL
                If gemarkungsPLZ2gemeindetext(nri.ToString) <> "ERROR" Then
                    Return gemarkungsPLZ2gemeindetext(nri.ToString)
                End If
            End If
            'ist es eine Gemarkungsnummer
            If nri > 720 And nri < 1000 Then
                'es ist vermutlich eine gemarkungsnummer
                If gemcode2gemeindetext(nri) <> "ERROR" Then
                    Return gemcode2gemeindetext(nri)
                End If
            End If

            'gemeindenummer nach vermessungswesen
            If nr.StartsWith("438") Then
                If nri% > 438000 Then
                    nri% = nri% - 438000
                    'gemeindename holen
                    If gemeindenr2gemeindetext(nri.ToString) <> "ERROR" Then
                        Return gemeindenr2gemeindetext(nri.ToString)
                    End If
                End If
            End If
            Return "ERROR"
        Catch ex As Exception
            Return "ERROR" & ex.Message
        End Try
    End Function
    Public Shared Function liegtGemeindeImKreisOffenbach(ByVal Gemeindename as string) as  Boolean
        ' in: s$ gemeindename
        ' out: true      wenn gemeindename in der liste der namen vorkommt
        '       false     wenn er nicht drin vorkommt
        'verwendet die datei:
        ' sollte bebehalten werden
        Try
            If String.IsNullOrEmpty(Gemeindename) Then Return False
            Dim a$(13), i%
            'myLog("gibt es die gemeinde_anfang: " & s)
            Gemeindename = Gemeindename.ToLower
            'ausnahmen und korrekturen
            Gemeindename = Gemeindename.Replace(Chr(34), "")
            Gemeindename = Gemeindename.Replace(" ", "")
            If Gemeindename = "mühlheim am main" Then Gemeindename = "mühlheim"
            If Gemeindename = "mühlheimammain" Then Gemeindename = "mühlheim"
            If Gemeindename = "muehlheim" Then Gemeindename = "mühlheim"
            If Gemeindename = "roedermark" Then Gemeindename = "rödermark"
            'If gemname = "neu-isenburg" Then gemname = "neuisenburg"
            If Gemeindename.EndsWith("hlheim") Then Gemeindename = "mühlheim"
            If Gemeindename.EndsWith("dermark") Then Gemeindename = "rödermark"

            'mylogmsg "gibt es die gemeinde: " & s
            a(1) = "dietzenbach"
            a(2) = "langen"
            a(3) = "egelsbach"
            a(4) = "neuisenburg"
            a(5) = "heusenstamm"
            a(6) = "rödermark"
            a(7) = "rodgau"
            a(8) = "seligenstadt"
            a(9) = "hainburg"
            a(10) = "mainhausen"
            a(11) = "mühlheim"
            a(12) = "obertshausen"
            a(13) = "dreieich"
            For i = 1 To 13
                'mylogmsg a(i)
                If clsString.normalizeGemeindenamen(Gemeindename) = a(i) Then
                    Return True
                End If
            Next
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
