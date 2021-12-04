Class clsStartup
    Private Sub New()

    End Sub
    'modus=paradigma vorgangsid="9609" range="490248,491254,5548144,5548704" 
    Friend Shared Function getIminternet() As Boolean

        If System.Environment.GetEnvironmentVariable("USERDNSDOMAIN") = "KREIS-OF.LOCAL" And
                System.Environment.GetEnvironmentVariable("USERDOMAIN") = "KREIS-OF" Then
            Return False
        Else
            Return True
        End If

    End Function

    'Private Shared Function internetFromMarkerFile(datei As String) As Boolean
    '    Try
    '        Dim fi As New IO.FileInfo(datei)
    '        If fi.Exists Then
    '            Return False
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Return True
    '    End Try
    'End Function

    Shared Function setPaintsoftware(oldprog As String) As String
        Try
            l(" MOD setPaintsoftware anfang")
            Dim topf As String = myglobalz.userIniProfile.WertLesen("software", "usepaint")
            If topf.ToLower.Trim.IsNothingOrEmpty Then
                userIniProfile.WertSchreiben("software", "usepaint", "true")
                userIniProfile.WertSchreiben("software", "paintexefullpath", oldprog)
                'oldprog bleibt
            Else
                If topf.ToLower.Trim = "true" Then
                    'oldprog bleibt
                Else
                    oldprog = myglobalz.userIniProfile.WertLesen("software", "paintexefullpath")
                End If
            End If
            l(" MOD setPaintsoftware ende")
            Return oldprog
        Catch ex As Exception
            l("Fehler in setPaintsoftware: " & ex.ToString())
            Return oldprog
        End Try
    End Function
    Shared Function setPDFreader(oldreader As String) As String
        'wird nur für schnelldruck gebraucht
        Try
            l(" MOD setPDFreader anfang")
            Dim topf As String = myglobalz.userIniProfile.WertLesen("software", "pdfreaderpfad")
            If topf.ToLower.Trim.IsNothingOrEmpty Then
                userIniProfile.WertSchreiben("software", "pdfreaderpfad", oldreader)
            Else
                oldreader = topf
            End If
            l(" MOD setPDFreader ende")
            Return oldreader
        Catch ex As Exception
            l("Fehler in setPDFreader: " & ex.ToString())
            Return oldreader
        End Try
    End Function


    Friend Shared Function istGISAdmin() As Boolean
        If Environment.UserName.ToLower = "zahnlückenpimpf" Or
            Environment.UserName.ToLower = "zahnlueckenpimpf" Or
            Environment.UserName.ToLower = "hurz" Or
            Environment.UserName.ToLower = "thieme_m" Or
            Environment.UserName.ToLower = "hurz" Or
            Environment.UserName.ToLower = "feinen_j" Then
            Return True
        End If
        Return False
    End Function
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
        'MsgBox(strGlobals.localDocumentCacheRoot)
        With My.Log.DefaultFileLogWriter
#If DEBUG Then
            '.CustomLocation = mgisUserRoot & "logs\"
#Else
#End If
            '.CustomLocation = My.Computer.FileSystem.SpecialDirectories.Temp & "\mgis_logs\"
            .CustomLocation = strGlobals.localDocumentCacheRoot & "\logs\"
            '.BaseFileName = GisUser.username & "_" & Format(Now, "yyyyMMddhhmmss")
            .BaseFileName = "mgis_" & Format(Now, "yyyyMMddhhmmss")
            .AutoFlush = False
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
            l("fehler in ", ex)
            Return "fehler"
        End Try
    End Function

    Friend Shared Function getNicknameAndPWFromLokalCookie(ByRef nick As String, ByRef pw As String, cookiefullpath As String) As Boolean
        Dim datei As String

        Dim a As String()
        Dim fi As IO.FileInfo
        Try
            l(" MOD getNicknameAndPW anfang")
            datei = cookiefullpath
            fi = New IO.FileInfo(datei)

            Dim retcode As Boolean
            If fi.Exists Then
                fi = Nothing
                a = IO.File.ReadAllLines(datei)
                If a.Length > 0 Then
                    nick = a(0)
                    pw = a(1)
                    nick = clsString.normalize_Filename(clsString.umlaut2ue(nick), "_")
                    pw = clsString.normalize_Filename(clsString.umlaut2ue(pw), "_")
                    retcode = True
                Else
                    nick = ""
                    pw = ""
                    retcode = False
                End If

            Else
                'l("fehler in getNicknameAndPW file not found datei " & datei)
                fi = Nothing
                nick = clsString.normalize_Filename(clsString.umlaut2ue(nick), "_") ' vorher GisUser.nick
                pw = ""
                retcode = True
            End If
            l(" MOD getNicknameAndPW ende")
            Return retcode
        Catch ex As Exception
            l("Fehler in getNicknameAndPW: " & ex.ToString())
            nick = "unbekannt"
            pw = ""
            Return False
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




    '<Obsolete>
    'Private Shared Function dateieinlesen(datei As String) As String
    '    Try
    '        Dim lestext As String
    '        Dim fi As IO.StreamReader
    '        fi = New IO.StreamReader(datei)
    '        lestext = fi.ReadToEnd
    '        fi.Close()
    '        fi.Dispose()
    '        fi = Nothing
    '        Return lestext
    '    Catch ex As Exception
    '        nachricht("Fehler in dateieinlesen: " ,ex)
    '        Return ""
    '    End Try
    'End Function

    Shared Sub setzeAktKoordinate()
        'kartengen.aktMap.aktrange.CalcCenter()
        aktGlobPoint.strX = CType(kartengen.aktMap.aktrange.xcenter, String)
        aktGlobPoint.strY = CType(kartengen.aktMap.aktrange.ycenter, String)
    End Sub



    Shared Function calcURI4vogel() As String
        Try
            nachricht("USERAKTION: googlekarte  vogel")
            'MsgBox("Google verlangt für die 'Vogelperspektive' jetzt Geld. Das wird vermutlich relativ geringe Kosten verursachen." & Environment.NewLine &
            '       " Wer nicht darauf verzichten will bitte bei mir melden 4434!",, "Google Vogelperspektive jetzt kostenpflichtig")
            Dim gis As New clsGISfunctions
            Dim result As String
            Dim longitude, latitude As String
            result = gis.GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(kartengen.aktMap.aktrange, True, longitude, latitude)
            If result = "fehler" Or result = "" Then
                Return ""
            Else
                '  gis.starten(result)
                '  GMtemplates.templateStarten(result)
                Return result
            End If
            gis = Nothing
        Catch ex As Exception
            nachricht("fehler in starteWebbrowserControl1: ", ex)
            Return ""
        End Try
    End Function



    Shared Function setMapFirstRange(STARTUP_rangestring As String) As clsRange
        'Dim lastrangecookie As String = ""
        Dim newrange As clsRange
        newrange = setMapKreisRange() ' als notnagel vorabladen
        Try
            If STARTUP_rangestring.IsNothingOrEmpty Then
                'lastrangecookie = mgisRemoteUserRoot & "lastrange\" & GisUser.nick & "_lastRange.txt"
                'Dim fi As New IO.FileInfo(lastrangecookie)
                'If fi.Exists Then
                Dim tranfge As New clsRange
                tranfge = tools.rangeLadenLastOne()
                If tranfge Is Nothing Then
                    newrange = setMapKreisRange()
                Else
                    If tranfge.xl > 1000 Then
                        'kartengen.aktMap.aktrange.rangekopierenVon(tranfge)
                        newrange.rangekopierenVon(tranfge)
                    Else
                        newrange = setMapKreisRange()
                    End If
                    'Else
                    '    newrange = setMapKreisRange() ???? warum raus???
                    'End If
                End If
            Else
                l("Startup_rangestringauswerten")
                newrange = clsStartup.Startup_rangestringauswerten(STARTUP_rangestring)
            End If
            Return newrange
        Catch ex As Exception
            l("fehler in setMapFirstRange: STARTUP_rangestring: " & STARTUP_rangestring & Environment.NewLine & " newrange: " & newrange.toString & " /// ", ex)
            Return Nothing
        End Try
    End Function

    Friend Shared Sub LegendenCacheLoeschen()
        Try
            l(" LegendenCacheLoeschen ---------------------- anfang")
            Dim ausgabeDIR As String = strGlobals.localDocumentCacheRoot
            Dim di = IO.Directory.CreateDirectory(ausgabeDIR)
            Dim templiste As IO.FileInfo()
            templiste = di.GetFiles("*.html")
            For Each datei In templiste
                datei.Delete()
            Next
            templiste = di.GetFiles("*.docx")
            For Each datei In templiste
                datei.Delete()
            Next
            l(" LegendenCacheLoeschen ---------------------- ende")
        Catch ex As Exception
            l("Fehler in LegendenCacheLoeschen: " & ex.ToString())
        End Try
    End Sub
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
            '    If   GisUser.nick <> GisUser.nick Then
            '        result = "GIS " & vid
            '    Else
            '        result = "GIS " & vid & "." &   GisUser.nick
            '    End If
            'Else
            '    result = "GIS " & " Modus: " & STARTUP_mgismodus & ", Bearbeiter: " &
            '                    Environment.UserDomainName & "." &
            '                      GisUser.nick
            'End If
            '

            result.Append("Desktop GIS ")
            If STARTUP_mgismodus = "paradigma" Then
                result.Append("; Paradigma Vorgang: " & vid)
            End If
            result.Append("; Bearbeiter: " & Environment.UserDomainName & "." & GisUser.nick)
            result.Append("; (" & GisUser.favogruppekurz & ", " & clsActiveDir.fdkurz & ")")
            result.Append("; Modus: " & STARTUP_mgismodus)
            result.Append(" // " & GisUser.userLayerAid)
            result.Append(" [v." & mgisVersion & "] ")
            'result.Append("; Ebenen: " & anzahlEbenen)
            result.Append("; Win64bit: " & Environment.Is64BitOperatingSystem)
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
                l("arg: " & sttelement)
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
                    GisUser.nick = GisUser.username
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
    Shared Sub defineDinA4Dina3Formate()
        l("defineDinA4Dina3Formate")
        dina4InMM.w = 297 : dina4InMM.h = 210
        dina3InMM.w = 420 : dina3InMM.h = 297

        dina4InPixel.w = 842 : dina4InPixel.h = 595
        dina3InPixel.w = 1191 : dina3InPixel.h = 842
    End Sub

    Friend Shared Function getStealthName4FeinenJ(arguments() As String) As String
        l("in stealth1")
        If GisUser.username.ToLower = "feinen_j".ToLower Then
            Dim stealth As String
            l("vor stealth2")
            stealth = clsStartup.getStartupArgument(arguments, "username=")
            l("  stealth " & stealth)
            If Not stealth.IsNothingOrEmpty Then
                ' GisUser.username = stealth
                ' l("  stealth wurde aktiviert   GisUser.nick; " & GisUser.nick)
                l("  stealth " & stealth)
                Return stealth
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    Friend Shared Function getUserAndInternetInfoRites(user As clsUser) As String
        Dim result As String = "", hinweis As String = "", userinfo As String
        l(" MOD getUserInfo anfang")
        l(" user.nick " & user.nick)
        Try
            'userinfo = makeLokalUserinfo(user)
            result = clsToolsAllg.getUserinfoFromServer(user, hinweis) : l(hinweis)
            result = result.Replace("$", "").Replace(vbCrLf, "")
        l(" MOD getUserInfo ende Rites: " & result & "<")
            If result.IsNothingOrEmpty Then
                Return "0"
            Else
                Return result
            End If
        Catch ex As Exception
            l("Fehler in getUserAndInternetInfoRites: " & ex.ToString())
            Return ""
        End Try
    End Function

    'Shared Function makeLokalUserinfo(user As clsUser) As String
    '    Try
    '        l(" MOD makeUserinfo anfang")
    '        Dim ui As String = clsString.umlaut2ue(user.username & "+" +
    '                                               user.macAdress.Replace(",", "") & "+" &
    '                                               user.domain & "+" &
    '                                               user.EmailPW & "+" &
    '                                               user.MachineName)
    '        l(" MOD makeUserinfo ende")
    '        Return ui
    '    Catch ex As Exception
    '        l("Fehler in makeUserinfo: " & ex.ToString())
    '        Return ""
    '    End Try
    'End Function

    Friend Shared Function gisMaximiertStarten(aktval As Boolean) As Boolean
        Dim retval As Boolean
        Try
            l(" setPosition ---------------------- anfang")
            Dim topf As String = myglobalz.userIniProfile.WertLesen("gisstart", "maximiertstarten")
            If String.IsNullOrEmpty(topf) Then
                myglobalz.userIniProfile.WertSchreiben("gisstart", "maximiertstarten", CType(aktval, String))
                retval = aktval
            Else
                retval = CBool(topf)
            End If

            l(" getIniDossier ---------------------- ende")
            Return retval
        Catch ex As Exception
            l("Fehler in setPosition: " & ex.ToString())
            Return aktval
        End Try
    End Function

    Friend Shared Sub Datenschutz()
        Try
            Process.Start(strGlobals.datenschutzDoc)
        Catch ex As Exception
            l("Fehler in Datenschutz: " & ex.ToString())
        End Try
    End Sub
End Class
