Class clsStartup
    'modus=paradigma vorgangsid="9609" range="490248,491254,5548144,5548704" 
    Shared Sub initParadigmaAdmins()
        ReDim paradigmaAdmins(5)
        paradigmaAdmins(0) = "nehler_u"
        paradigmaAdmins(1) = "weyers_g"
        paradigmaAdmins(2) = "kuhn_p"
        paradigmaAdmins(3) = "feinen_j"
        paradigmaAdmins(4) = "thieme_m"
        paradigmaAdmins(5) = "kroemmelbein_m"
    End Sub
    Shared Sub setLogfile()
        With My.Log.DefaultFileLogWriter
#If DEBUG Then
            '.CustomLocation = mgisUserRoot & "logs\"
#Else
#End If
            .CustomLocation = My.Computer.FileSystem.SpecialDirectories.Temp & "\mgis_logs\"
            '  .CustomLocation = mgisUserRoot & "logs\"
            .BaseFileName = GisUser.username
            .AutoFlush = True
            .Append = False
        End With
    End Sub
    Friend Shared Function setMGISmodus(arguments() As String) As String
        Try
            l("setMGISmodus---------------------- anfang")
            For Each sttelement In arguments
                'MsgBox(sttelement)
                If sttelement.Contains("modus=paradigma") Then
                    l("modus=paradigma also gesetzt")
                    Return "paradigma"
                End If
                If sttelement.Contains("modus=probaug") Then
                    l("modus=probaug also gesetzt")
                    'myglobalz.ProbaugSuchmodus = clsProbaugArgs.getProbaugArguments(arguments)
                    Return "probaug"
                End If
            Next
            Return "vanilla"
            l("setMGISmodus---------------------- ende")
        Catch ex As Exception
            l("Fehler in setMGISmodus: " & ex.ToString())
            Return "vanilla"
        End Try
    End Function

    Friend Shared Function getStartupArgument(arguments() As String, suchstring As String) As String
        Dim a() As String
        Try
            For Each sttelement In arguments
                'MsgBox(sttelement)
                If sttelement.Contains(suchstring) Then
                    a = sttelement.Split("="c)
                    Return a(1)
                End If
            Next
            Return ""
        Catch ex As Exception
            l("fehler in " & ex.ToString)
            Return "fehler"
        End Try
    End Function
    Public Shared Sub einlesenZweiterBildschirm(ByRef aufzweitembildschirmstarten As Boolean, ByRef hauptbildschirmStehtLinks As Boolean)
        Dim test As String = myglobalz.userIniProfile.WertLesen("gisstart", "ImmerAufZweitemScreen")
        If test.IsNothingOrEmpty Then
            aufzweitembildschirmstarten = False
        Else
            aufzweitembildschirmstarten = CBool(test)
        End If

        test = myglobalz.userIniProfile.WertLesen("gisstart", "hauptbildschirmStehtLinks")
        If test.IsNothingOrEmpty Then
            hauptbildschirmStehtLinks = False
        Else
            hauptbildschirmStehtLinks = CBool(test)
        End If
    End Sub
    <Obsolete>
    Friend Shared Sub getgisstartOptionen(ByRef zweiterScreenvorhanden As Boolean,
                                   ByRef aufzweitembildschirmstarten As Boolean,
                                   ByRef hauptbildschirmStehtLinks As Boolean)
        l("getgisstartOptionen")
        Dim datei As String = ""
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            datei = "O:\UMWELT-PARADIGMA\div\user\ini\feij.ini"
        Else
            datei = IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDocuments),
                                 "Paradigma\gisstart.txt")
            l("datei")
            Dim fo As New IO.FileInfo(datei)
            Dim lestext As String

            If fo.Exists Then
                'einlesen auswerten
                lestext = dateieinlesen(datei)
                Dim a() As String
                a = lestext.Split(CType(vbCrLf, Char()))
                If a(0).Trim = "1" Then
                    zweiterScreenvorhanden = True
                Else
                    zweiterScreenvorhanden = False
                End If
                If a(2).Trim = "1" Then
                    aufzweitembildschirmstarten = True
                Else
                    aufzweitembildschirmstarten = False
                End If
                If a(4).Trim = "1" Then
                    hauptbildschirmStehtLinks = True
                Else
                    hauptbildschirmStehtLinks = False
                End If
            Else
                zweiterScreenvorhanden = False
                l("getgisstartOptionen- ende zweiterScreenvorhanden = False")
                Exit Sub
            End If
        End If


        l("getgisstartOptionen- ende")
    End Sub



    <Obsolete>
    Private Shared Function dateieinlesen(datei As String) As String
        Try
            Dim lestext As String
            Dim fi As IO.StreamReader
            fi = New IO.StreamReader(datei)
            lestext = fi.ReadToEnd
            fi.Close()
            fi.Dispose()
            fi = Nothing
            Return lestext
        Catch ex As Exception
            nachricht("Fehler in dateieinlesen: " & ex.ToString)
            Return ""
        End Try
    End Function

    Shared Sub setzeAktKoordinate()
        kartengen.aktMap.aktrange.CalcCenter()
        aktGlobPoint.strX = CType(kartengen.aktMap.aktrange.xcenter, String)
        aktGlobPoint.strY = CType(kartengen.aktMap.aktrange.ycenter, String)
    End Sub

    Shared Function calcURI4vogel() As String
        Try
            nachricht("USERAKTION: googlekarte  vogel")
            Dim gis As New clsGISfunctions
            Dim result As String
            result = gis.GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(kartengen.aktMap.aktrange, True)
            If result = "fehler" Or result = "" Then
                Return ""
            Else
                '  gis.starten(result)
                '  GMtemplates.templateStarten(result)
                Return result
            End If
            gis = Nothing
        Catch ex As Exception
            nachricht("fehler in starteWebbrowserControl1: " & ex.ToString)
            Return ""
        End Try
    End Function


    Shared Function setMapFirstRange(STARTUP_rangestring As String) As clsRange
        Dim lastrangecookie As String = ""
        Dim newrange As New clsRange
        newrange = setMapKreisRange() ' als notnagel vorabladen
        Try
            If STARTUP_rangestring.IsNothingOrEmpty Then
                lastrangecookie = mgisUserRoot & "\lastrange\" & GisUser.username & "_lastRange.txt"
                'Dim fi As New IO.FileInfo(lastrangecookie)
                'If fi.Exists Then
                Dim tranfge As New clsRange
                tranfge = tools.rangeLaden()
                If tranfge.xl > 1000 Then
                    'kartengen.aktMap.aktrange.rangekopierenVon(tranfge)
                    newrange.rangekopierenVon(tranfge)
                End If
                'Else
                '    newrange = setMapKreisRange()
                'End If
            Else
                l("Startup_rangestringauswerten")
                newrange = clsStartup.Startup_rangestringauswerten(STARTUP_rangestring)
            End If
            Return newrange
        Catch ex As Exception
            l("fehler in setMapFirstRange: " & lastrangecookie & " /// " & ex.ToString)
            Return Nothing
        End Try
    End Function

    Friend Shared Function Startup_rangestringauswerten(STARTUP_rangestring As String) As clsRange
        l("Startup_rangestringauswerten")
        Dim a() As String
        Dim newurange As New clsRange
        Try
            a = STARTUP_rangestring.Split(","c)
            newurange.xl = CDbl(a(0))
            newurange.xh = CDbl(a(1))
            newurange.yl = CDbl(a(2))
            newurange.yh = CDbl(a(3))
            Return newurange
        Catch ex As Exception
            l("fehler in Startup_rangestringauswerten", ex)
            Return Nothing
        End Try
    End Function


    Friend Shared Function getWindowTitel(vid As String, anzahlEbenen As Integer) As String
        Dim result As New Text.StringBuilder
        Try
            l("getWindowTitel---------------------- anfang")
            'If STARTUP_mgismodus = "paradigma" Then
            '    If   GisUser.username <> Environment.UserName Then
            '        result = "GIS " & vid
            '    Else
            '        result = "GIS " & vid & "." &   GisUser.username
            '    End If
            'Else
            '    result = "GIS " & " Modus: " & STARTUP_mgismodus & ", Bearbeiter: " &
            '                    Environment.UserDomainName & "." &
            '                      GisUser.username
            'End If
            '

            result.Append("Desktop GIS ")
            If STARTUP_mgismodus = "paradigma" Then
                result.Append("; Paradigma Vorgang: " & vid)
            End If
            result.Append("; Bearbeiter: " & Environment.UserDomainName & "." & GisUser.username)
            result.Append("; (" & GisUser.favogruppekurz & ", " & clsActiveDir.fdkurz & ")")
            result.Append("; Modus: " & STARTUP_mgismodus)
            result.Append(" // " & GisUser.userLayerAid)
            result.Append(" [v." & mgisVersion & "] ")
            result.Append("; Ebenen: " & anzahlEbenen)
            result.Append("; " & paradigmaDBTyp)
            Return result.ToString
            l("getWindowTitel---------------------- ende")
        Catch ex As Exception
            l("Fehler in getWindowTitel: " & ex.ToString())
            Return ""
        End Try
    End Function
    Shared Function setMapKreisRange() As clsRange 'kartengen.aktMap.aktrange
        Dim neurange As New clsRange
        neurange.xl = 470685
        neurange.xh = 503544
        neurange.yl = 5530566
        neurange.yh = 5553593

        neurange.yl = 5530966
        neurange.yh = 5553993

        neurange.xl = 471185
        neurange.xh = 504144
        Return neurange
    End Function
    Friend Shared Sub mapAllArguments(arguments() As String)
        Try
            l("mapAllArguments---------------------- anfang")
            For Each sttelement In arguments
                If sttelement.Contains("modus=bebauungsplankataster") Then
                    l("modus=bebauungsplankataster also gesetzt")
                    STARTUP_mgismodus = "bebauungsplankataster"
                End If
                If sttelement.Contains("modus=paradigma") Then
                    l("modus=paradigma also gesetzt")
                    STARTUP_mgismodus = "paradigma"
                End If
                If sttelement.Contains("username=") Then
                    l("modus=username")
                    GisUser.username = sttelement.Replace("username=", "").Trim.ToLower
                End If
                If sttelement.Contains("modus=probaug") Then
                    STARTUP_mgismodus = "probaug"
                    l("modus=probaug ")
                End If
                If sttelement.Contains("suchmodus=adresse") Then
                    ProbaugSuchmodus = "adresse"
                    l(" suchmodus=adresse")
                End If
                If sttelement.Contains("suchmodus=flurstueck") Then
                    ProbaugSuchmodus = "flurstueck"
                    l("suchmodus=flurstueck ")
                End If
                If sttelement.Contains("gemeinde=") Then
                    probaugAdresse.Gisadresse.gemeindeName = sttelement.Replace("gemeinde=", "").Trim.ToLower
                    l("gemeinde " & probaugAdresse.Gisadresse.gemeindeName)
                End If
                If sttelement.Contains("strasse=") Then
                    probaugAdresse.Gisadresse.strasseName = sttelement.Replace("strasse=", "").Trim.ToLower
                    l("strasse " & probaugAdresse.Gisadresse.strasseName)
                End If
                If sttelement.Contains("hausnr=") Then
                    probaugAdresse.Gisadresse.HausKombi = sttelement.Replace("hausnr=", "").Trim.ToLower
                    l(" probaugAdresse.Gisadresse.HausKombi " & probaugAdresse.Gisadresse.HausKombi)
                End If
                If sttelement.Contains("gemarkung=") Then
                    probaugFST.normflst.gemarkungstext = sttelement.Replace("gemarkung=", "").Trim.ToLower
                    l("   probaugFST.normflst.gemarkungstext  " & probaugFST.normflst.gemarkungstext)
                End If
                If sttelement.Contains("flur=") Then
                    probaugFST.normflst.flur = CInt(sttelement.Replace("flur=", "").Trim.ToLower)
                    l("   probaugFST.normflst.gemarkungstext  " & probaugFST.normflst.gemarkungstext)
                End If
                If sttelement.Contains("fstueck=") Then
                    probaugFST.normflst.fstueckKombi = sttelement.Replace("fstueck=", "").Trim.ToLower
                    l(" probaugFST.normflst.flur  " & probaugFST.normflst.flur)
                End If
                If sttelement.Contains("beschreibung=") Then
                    aktvorgang.beschreibung = sttelement.Replace("beschreibung=", "").Trim
                    l("beschreibung " & aktvorgang.beschreibung)
                End If
                If sttelement.Contains("az=") Then
                    aktvorgang.az = sttelement.Replace("az=", "").Trim
                    l("az " & aktvorgang.az)
                End If
            Next
            l("mapAllArguments---------------------- ende")
        Catch ex As Exception
            l("Fehler in mapAllArguments: " & ex.ToString())
        End Try
        'modus=probaug suchmodus=flurstueck gemarkung=dietzenbach flur=5 fstueck=490/0"
    End Sub
End Class
