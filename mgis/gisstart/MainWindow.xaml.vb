Imports System.ComponentModel
Imports System.Data
Imports mgis
' modus=paradigma vorgangsid=9609 range=484593,484993,5544035,5544435 beschreibung="Neubau eines Zweifamilienhauses mit Garagen und PKW-Stellplätzen" az="II-67-3311-38579-17-wa-5_1-04769-17"
'
Class MainWindow
    Public Property mainWindow As Window = Me.mainWindow
    Private RubberbandStartpt As Point?
    Private RubberbandEndpt As Point?
    Private aktrangebox As Rectangle
    Public Property ladevorgangAbgeschlossen As Boolean = False
    Private Property curContentMousePoint As Point
    Private origContentMousePoint As Point
    Private Property isDraggingFlag As Boolean
    'Public Property kreisimabstand_Radius_m As Integer
    Public Property KreislinienRadius As Double = 0
    Private myPolyVertexCount As Integer
    Private KoordinateKLickpt As Point?
    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        btnWMSgetfeatureinfo.Visibility = Visibility.Collapsed
        'Public Const DinA4Breite As Double = 29.7
        'Public Const dina4Hoehe As Double = 21.0
        dina4InMM.w = 297 : dina4InMM.h = 210
        dina3InMM.w = 420 : dina3InMM.h = 297

        dina4InPixel.w = 842 : dina4InPixel.h = 595
        dina3InPixel.w = 1191 : dina3InPixel.h = 842
        clsInitStrings.setMainstrings()
        clsStartup.initParadigmaAdmins()

        'If Not nsUmgebung.setzeUmgebung(binZuhause) Then
        '    MsgBox("umgebung nicht definiert. Abbruch")
        '    End
        'End If
        GisUser.username = Environment.UserName
#If DEBUG Then
        '  GisUser.username = "Klingler_B"
        'GisUser.username = "Stich_K"
        'GisUser.username = "becker_a"
        'GisUser.username = "hurz"
        '  GisUser.username = "Jaeger_C"
        '  GisUser.username = "Buchmann_U"
        '  GisUser.username = "Mueller_B"
        '  GisUser.username = "Weicker-Zoeller_C"
        '  GisUser.username = "sindl_p"
        '  GisUser.username = "asasd"
        'GisUser.username = "pilz_j"
        'GisUser.username = "waldschmitt_r"
        'GisUser.username = "ackermann_r"
        'GisUser.username = "schmittner_u"
        'GisUser.username = "nehler_U"
        'GisUser.username = "schoeniger_j"
#End If

        clsStartup.setLogfile() : l("Start " & Now) : l("mgisversion:" & mgisVersion)
        'btnSuchobjAusSchalten.Visibility = Visibility.Collapsed
        userIniProfile = New clsINIDatei(mgisUserRoot & "userinis\" & GisUser.username & ".ini")

        exploreralphabetisch = exploreralphabetischFeststellen()

        zweitenBildschirm()
        WindowState = WindowState.Maximized
        startroutine(exploreralphabetisch)
        ' l("fehler")
        zindexeSetzen()
        clsTooltipps.setTooltipExplorerButton(btnAddLayer, "alles")
        clsTooltipps.setTooltipExplorerButton(btnExplorer, "alles")
        clsTooltipps.setTooltipSuchobjLoeschenButton(cbSOeinschalten, "alles")
        clsTooltipps.setTooltipSuchobjImgPin(imgpin, "alles")
        clsTooltipps.setTooltipInfoLegende(spInfoLegende, "alles")
        clsTooltipps.setTooltipVogel(btnVogel, "alles")
        showaktvorgangParadigma()
        ladevorgangAbgeschlossen = True
    End Sub

    Private Function exploreralphabetischFeststellen() As Boolean
        Dim test As String
        Try
            l("exploreralphabetischFeststellen---------------------- anfang")
            test = myglobalz.userIniProfile.WertLesen("Diverse", "exploreralphabetisch")
            If test.IsNothingOrEmpty Then
                myglobalz.userIniProfile.WertSchreiben("Diverse", "exploreralphabetisch", "1")
                Return True
            Else
                If test = "0" Then Return False
                If test = "1" Then Return True
                Return True
            End If
            l("exploreralphabetischFeststellen---------------------- ende")
        Catch ex As Exception
            l("Fehler in exploreralphabetischFeststellen: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Sub startroutine(explorerAlphabetisch As Boolean)
        currentProcID = clsGisstartPolitik.getCurrentProcId()
        Dim arguments As String() = Environment.GetCommandLineArgs()
        clsStartup.mapAllArguments(arguments)
        l("vor stealth1")
        If GisUser.username.ToLower = "feinen_j".ToLower Or GisUser.username.ToLower = "thieme_m".ToLower Then
            Dim stealth As String
            l("vor stealth2")
            stealth = clsStartup.getStartupArgument(arguments, "username=")
            l("  stealth " & stealth)
            If Not stealth.IsNothingOrEmpty Then
                GisUser.username = stealth
                l("  stealth wurde aktiviert   GisUser.username; " & GisUser.username)
            End If
        End If
#If DEBUG Then
        'STARTUP_mgismodus = "bebauungsplankataster"
        'STARTUP_mgismodus = "paradigma"
        'STARTUP_mgismodus = "vanilla"
        'GisUser.username = "Weber_S"
        'GisUser.username = "Sikora_T"
        'GisUser.username = "Pieroth_s"
        ' GisUser.username = "koslov_h"
        ' aktvorgangsid weiter unten einstellen
#End If


        If STARTUP_mgismodus = "paradigma" Then
            aktvorgangsid = clsStartup.getStartupArgument(arguments, "vorgangsid=")

#If DEBUG Then
            'aktvorgangsid = "38373"
            'aktvorgangsid = "9609"
            ''aktvorgangsid = "37036" 
            'STARTUP_mgismodus = "paradigma"
            'aktvorgangsid = "9609" 
#End If
            aktvorgang.id = CInt(aktvorgangsid)
            tbVorgangsid.Text = aktvorgangsid
        Else
            spVIDParadigma.Visibility = Visibility.Collapsed
            stackVorgangsid.IsEnabled = False
            rbfit2.Visibility = Visibility.Collapsed
        End If


        clsGisstartPolitik.registerAutostart("mgisAktualisieren.bat", myglobalz.mgisUserRoot)
        Dim STARTUP_rangestring As String = clsStartup.getStartupArgument(arguments, "range=")

        clsGisstartPolitik.gisStartPolitikUmsetzen(prozessname)
        clsINIDatei.UserinifileAnlegen(0, mgisUserRoot & "userinis\", GisUser.username)
        clsOptionTools.einlesenParadigmaDominiert(ParadigmaDominiertzuletztFavoriten)

        initdb()
        myglobalz.mgisRangecookieDir = clsToolsAllg.initMgisHistory

        setUserFDkurz(GisUser)
        l("user_fdkurz  1. " & GisUser.ADgruppenname)
        If GisUser.favogruppekurz = "umwelt" Then
            GisUser.paradigmaAbteilung = modParadigma.getParadigmaAbteilung4FDumwelt(GisUser.username)

            GisUser.favogruppekurz = getFsvoKurz4Paradigmaabt(GisUser.paradigmaAbteilung)
            If STARTUP_mgismodus = "paradigma" Then
                Threading.Thread.Sleep(1000)
            End If
        End If
        If GisUser.favogruppekurz = "umwelt" Then
            'dürfte nie eintreten
            If Not aktvorgangsid.IsNothingOrEmpty Then
                If IsNumeric(aktvorgangsid) Then
                    STARTUP_mgismodus = "paradigma"
                    l("STARTUP_mgismodus 2= paradigma wurde nachträglich gesetzt")
                End If
            End If
        End If
        l("setUserFDkurz clsActiveDir.fdkurz " & clsActiveDir.fdkurz)
        l("globalParadigmaUser  2. " & GisUser.favogruppekurz)
        VorgangsButtonSichtbarMachen()

        If GisUser.ADgruppenname.ToLower = "umwelt" Then
            spVIDParadigma.Visibility = Visibility.Visible
            tbVorgangsid.Text = aktvorgangsid
            'vorgangsknop ist im ua immer sichtbar

            Dim collHistory As List(Of CLstart.HistoryKookie.HistoryItem)
            'C:\Users\feinen_j\AppData\Roaming\Paradigma\cookies\
            Dim localAppDataParadigmaDir As String = System.Environment.GetEnvironmentVariable("APPDATA") & "\Paradigma"
            Dim ClientCookieDir = localAppDataParadigmaDir & "\cookies\"

            collHistory = CLstart.HistoryKookie.VerlaufsCookieLesen.exe(ClientCookieDir & "verlaufscookies")
            dgHistory.DataContext = collHistory
        End If
        setGruppenFavoritTextButton()


        mapfileBILD = mapfileCachePathroot & "_" & GisUser.username & currentProcID & ".map"
        'mapfileBILDrank0 = mapfileCachePathroot & "_" &   GisUser.username & currentProcID & "_0.map"
        'initdb()
        'STARTUP_mgismodus = "paradigma"
        ' If STARTUP_mgismodus.ToLower = "paradigma" Then
        If clsActiveDir.fdkurz.Trim.ToLower = "umwelt" Then
            If STARTUP_mgismodus = "paradigma" Then
                GisUser.userLayerAid = modParadigma.getuserlayeraid(GisUser.username)
                l(" GisUser.userLayerAid  " & GisUser.userLayerAid)
            End If
        End If
        '  tools.getUsergroup
        ProxyString = getproxystring()

        allLayersPres = clsWebgisPGtools.getAllLayersPres(myglobalz.iminternet, clsActiveDir.fdkurz, allLayers)
        allDokus = clsWebgisPGtools.getAllDokus(myglobalz.iminternet)


        clsWebgisPGtools.calcOwners(allDokus)
        clsWebgisPGtools.dombineLayerDoku(allLayersPres, allDokus)
        clsWebgisPGtools.calcEtikett_kategorie_tultipp(allLayersPres, explorerAlphabetisch)
        clsWebgisPGtools.calcIsHintergrund(allLayersPres)

        initHintergrundCMB()

        'dgErgebnis.Width = 550
        MainListBox.Height = 900
        '200er ebenen ausschalten
        stContext.Visibility = Visibility.Collapsed
        stwinthemen.Visibility = Visibility.Collapsed
        stPDFDruck.Visibility = Visibility.Collapsed

        initVGCanvasSize()
        setCanvasSizes()

        showAktVorgangsid()
        kartengen.Domainstring = serverWeb
        btnGetFlaecheEnde.Visibility = Visibility.Collapsed
        If STARTUP_mgismodus = "probaug" Then
            handleProbaugModus(STARTUP_rangestring)
        Else
            kartengen.aktMap.aktrange = clsStartup.setMapFirstRange(STARTUP_rangestring)
        End If
        imageMapCanvas.Visibility = Visibility.Collapsed
        defineGruppenFavoriten()
        ' modLayer.getLayerHgrund()
        ''cmbHgrund.SelectedValue = layerHgrund.aid
        'layersSelected = getStandardlayersAids()
        ' layersSelected = modLayer.getCompleteLayers()
        initMasstabCombo()

        cmbMasstab.ItemsSource = masstaebe
        ' masstaebe.Clear()

        layersSelected.Sort()

        refreshMap(True, True)

        cvPDFrechteck.Visibility = Visibility.Collapsed
        clsWebgisPGtools.getOSliste(allLayersPres, "")
        dgOSliste.DataContext = allOSLayers
        Title = clsStartup.getWindowTitel(tbVorgangsid.Text, allLayersPres.Count)
        Debug.Print("" & layersSelected.Count)
        hinweisFallsKeineEbenenGeladenSind()
    End Sub

    Private Sub handleProbaugModus(STARTUP_rangestring As String)
        If clsProbaug.sindProbaugSuchParamsOK(ProbaugSuchmodus, probaugAdresse, probaugFST) Then
            Dim prorange As New clsRange
            Dim errorhinweis As String = ""
            tbVorgangsid.Text = aktvorgang.az.ToString
            prorange = clsProbaug.getAktrangeFromProbaug(ProbaugSuchmodus, probaugAdresse, probaugFST, errorhinweis)
            l(" prorange nhinweis " & errorhinweis)
            If prorange Is Nothing Then
                Dim mesres As New MessageBoxResult
                'Dim ortTemp As String
                'ortTemp = calcOrtTemp()
                l("warnung probaugRange konnte nicht ermittelt werden. Programmende." & probaugAdresse.defineAbstract & " " & probaugFST.defineAbstract)
                mesres = MessageBox.Show(errorhinweis & Environment.NewLine & Environment.NewLine &
                                        "GIS beenden ?" & Environment.NewLine,
                                        "Ort konnte nicht gefunden werden",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question)
                If mesres = MessageBoxResult.Yes Then
                    End
                Else
                    kartengen.aktMap.aktrange = clsStartup.setMapFirstRange(STARTUP_rangestring)
                End If
            Else
                kartengen.aktMap.aktrange = prorange
                suchObjektModus = "fst"
                imgpin.Visibility = Visibility.Visible
                suchCanvas.Visibility = Visibility.Visible
                '
                If ProbaugSuchmodus = "adresse" Then
                    nachricht("USERAKTION: ProbaugSuchmodus = adresse suchen ")
                    kartengen.aktMap.aktrange.CalcCenter()
                    aktGlobPoint.X = kartengen.aktMap.aktrange.xcenter
                    aktGlobPoint.Y = kartengen.aktMap.aktrange.ycenter
                    aktPolygon.ShapeSerial = holePUFFERPolygonFuerPoint(aktGlobPoint, 30) 'pufferinMeter)
                    aktPolygon.originalQuellString = aktPolygon.ShapeSerial
                    aktFST.normflst.serials.Clear()
                    aktFST.normflst.serials.Add(aktPolygon.ShapeSerial)
                    suchObjektModus = "fst"
                    setBoundingRefresh(kartengen.aktMap.aktrange)
                End If

                'btnSuchobjAusSchalten.Visibility = Visibility.Visible
            End If
        Else
            MsgBox("Die von Probaug übergebenen Suchparameter sind nicht brauchbar.")
            kartengen.aktMap.aktrange = clsStartup.setMapFirstRange(STARTUP_rangestring)
        End If
    End Sub

    Private Sub VorgangsButtonSichtbarMachen()
        'den vorgangsbuttonSichtbarMachen
        If STARTUP_mgismodus = "paradigma" Then
            spVIDParadigma.Visibility = Visibility.Visible
            btnParadigmaLight.Visibility = Visibility.Collapsed
        Else
            If GisUser.ADgruppenname.ToLower = "umwelt" Then
                spVIDParadigma.Visibility = Visibility.Visible
                btnParadigmaLight.Visibility = Visibility.Collapsed
            Else
                If STARTUP_mgismodus = "probaug" Then
                    spVIDParadigma.Visibility = Visibility.Collapsed
                    btnParadigmaLight.Visibility = Visibility.Collapsed
                Else
#If DEBUG Then
                    spVIDParadigma.Visibility = Visibility.Collapsed
                    btnParadigmaLight.Visibility = Visibility.Collapsed 'IHAHAHAHA
#Else
                    spVIDParadigma.Visibility = Visibility.Collapsed
                    btnParadigmaLight.Visibility = Visibility.Collapsed
#End If
                End If
            End If
        End If
    End Sub

    Private Shared Function calcOrtTemp() As String
        Dim ortTemp As String

        If ProbaugSuchmodus = "flurstueck" Then
            ortTemp = probaugFST.defineAbstract & Environment.NewLine
        Else
            ortTemp = probaugAdresse.defineAbstract & Environment.NewLine
        End If

        Return ortTemp
    End Function

    Private Sub hinweisFallsKeineEbenenGeladenSind()
        If layersSelected.Count < 2 Then
            Dim rtfdatei As String
            rtfdatei = myglobalz.mgisUserRoot & "hinweis_KeineEbenen.rtf"
            rtfdatei = "c:\ptest\mgis\hinweis_KeineEbenen.rtf"
            Dim freileg As New winLeg(rtfdatei)
            freileg.Show()
        End If
    End Sub

    Private Function gisStartFavoritenUmsetzen() As Boolean
        Throw New NotImplementedException()
    End Function

    Private Sub setGruppenFavoritTextButton()
        btnGruppeFavo.ToolTip = "Die Ansicht meiner Gruppe aufrufen. Meine Gruppe ist '" & GisUser.favogruppekurz.ToUpper & "' !"
    End Sub

    'Private Sub reduziereEtitketten()
    '    Try
    '        l("reduziereEtitketten---------------------- anfang")
    '        For Each nlay As clsLayerPres In layersSelected
    '            nlay.SortierKriterium = tools.reduziereEtikettAufTitel(nlay)
    '        Next
    '        l("reduziereEtitketten---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in reduziereEtitketten: " & ex.ToString())
    '    End Try
    'End Sub



    Private Sub zweitenBildschirm()
        Dim zweiterScreenvorhanden As Boolean = False
        Dim aufzweitembildschirmstarten As Boolean = False
        Dim hauptbildschirmStehtLinks As Boolean = False
        '  startup.getgisstartOptionen(zweiterScreenvorhanden, aufzweitembildschirmstarten, hauptbildschirmStehtLinks)
        clsStartup.einlesenZweiterBildschirm(aufzweitembildschirmstarten, hauptbildschirmStehtLinks)

        If aufzweitembildschirmstarten Then
            If hauptbildschirmStehtLinks Then
                Left = SystemParameters.PrimaryScreenWidth
            Else
                Left = -1 * SystemParameters.PrimaryScreenWidth
            End If
        Else

        End If

    End Sub



    Private Sub defineGruppenFavoriten()
        'favoTools.initFavorite("bauaufsicht")
        'Dim meinfavofile As String
        'STARTUP_mgismodus = "paradigma"
        If STARTUP_mgismodus = "probaug" Or STARTUP_mgismodus = "paradigma" Then
            'meinfavofile = favoTools.calcMeinFavoriteDateiname("nichtauffindbar")
            If ParadigmaDominiertzuletztFavoriten Then
                favoTools.FavoritLaden("fix", GisUser.favogruppekurz)
            Else
                If Not favoTools.FavoritLaden("zuletzt", GisUser.username) Then
                    favoTools.FavoritLaden("fix", GisUser.favogruppekurz)
                End If
            End If
        Else
            If Not favoTools.FavoritLaden("zuletzt", GisUser.username) Then
                favoTools.FavoritLaden("fix", GisUser.favogruppekurz)
            End If
        End If

        'fi = New IO.FileInfo(meinfavofile)
        'If fi.Exists Then

        'Else
        '    meinfavofile = favoTools.calcMeinFavoriteDateiname(  GisUser.username)
        '    fi = New IO.FileInfo(meinfavofile)
        '    If fi.Exists Then
        '        favoTools.FavoriteLaden(  GisUser.username)
        '    Else
        '        favoTools.FavoriteLaden(  gisuser.favogruppekurz)
        '    End If
        'End If

        favoritenUmsetzen()
    End Sub

    Private Sub initHintergrundCMB()

        cmbHgrund.ItemsSource = Nothing
        hgrundLayers = modLayer.getHintergrund(myglobalz.iminternet)
        hgrundLayers.Sort()
        Dim leer As New clsLayerPres
        leer.titel = "kein Hintergrund"
        leer.aid = 0

        hgrundLayers.Add(leer)

        'leer = New clsLayerPres
        'leer.titel = "Hintergrund: Helligkeit einstellen"
        'leer.aid = -1
        'hgrundLayers.Add(leer)

        'clsWebgisPGtools.dombineLayerDoku(templayers, allDokus)
        cmbHgrund.ItemsSource = hgrundLayers

    End Sub

    Private Sub refreshMap(vgrundRefresh As Boolean, hgrundrefresh As Boolean)
        GC.Collect()
        myglobalz.slots = SlotTools.createAllSlots(layerHgrund, layersSelected,
                                                   cv1, cv0, OSmapCanvas,
                                                   vgrundRefresh, hgrundrefresh)
        'svMainScrollviewer.Height = 800
        'MainListBox.Height = 800
        Dim layersUsed4Controlling As Integer = 0
        'mapfileNamenNeuBerechnen()
        ' initVGCanvasSize()
        setCanvasSizes()
        calcBalkenbreite()
        stckBalken.Visibility = Visibility.Visible
        showLayersliste()
        'modLayer.createMapfileHG() entfällt weil header schon existiert

        Dim layersNachRangSortiert As List(Of clsLayerPres) = modLayer.sortiereLayers(layersSelected)

        'modLayer.createMapfileVG(layersUsed4Controlling, layersNachRangSortiert,normMapfileHeader)

        Dim summe = modLayer.CreateVGMapfileString(layersUsed4Controlling, layersNachRangSortiert, normMapfileHeader)
        My.Computer.FileSystem.WriteAllText(slots(1).mapfile, summe, False, enc)

        clsControlling.controllingprotokoll(layersUsed4Controlling + 1)
        layerActive.mapFileHeader = ""
        For Each layer As clsLayerPres In layersSelected
            If layer.isactive Then
                layerActive.mapFileHeader = layer.mapFileHeader
                Exit For
            End If
        Next
        If layerActive.mapFileHeader = String.Empty Then
            If layerHgrund.isactive Then
                layerActive.mapFileHeader = layerHgrund.mapFileHeader
            End If
        End If
        erzeugeImagemap = True
        initMassstab(cv1.Width, cv1.Height, System.Windows.SystemParameters.PrimaryScreenWidth)
        showTBmasstab(aktMasstab)
        '  presentMap(vgrundRefresh, hgrundrefresh)
        presentMap()

        If Not myglobalz.mgisBackModus Then
            If ladevorgangAbgeschlossen Then
                clsToolsAllg.mgisRangeCookieSave(kartengen.aktMap.aktrange, myglobalz.mgisRangecookieDir)
                myglobalz.mgisBackModus = False
            End If

        End If
    End Sub

    Private Sub mapfileNamenNeuBerechnen()
        mapfileBILD = mapfileCachePathroot & GisUser.username & "_" & clsString.date2string(Now, 5) & ".map"
        'mapfileBILDrank0 = mapfileCachePathroot & GisUser.username & "_" & clsString.date2string(Now, 5) & "_0.map"
        mapfileBILDrank0 = "" + layerHgrund.mapFile.Replace("layer.map", "header.map")
    End Sub

    Private Sub calcBalkenbreite()
        '  mapCanvas.Width 
        'balkenbreite ist 200
        Debug.Print(cv1.Width & ", " & btnBalken.Width & ", " & kartengen.aktMap.aktrange.xdif)
        Dim balkenbreiteInMeter As Double '= (kartengen.aktMap.aktrange.xdif)
        Dim meterProPixel As Double = (kartengen.aktMap.aktrange.xdif / cv1.Width)
        balkenbreiteInMeter = meterProPixel * btnBalken.Width
        btnBalken.Content = Format(balkenbreiteInMeter, "######,##") & " m"
        aktMasstab = (balkenbreiteInMeter / 5) * 100
        btnBalken.ToolTip = "click To vanish"

        'aktMasstab = (kartengen.aktMap.aktrange.xdif / mapCanvas.Width) '=meter pro pixel
    End Sub

    Private Sub showTBmasstab(balkenbreiteInMeter As Double)
        'tbMasstab.Text = " 1 : " & balkenbreiteInMeter.ToString("n")
        tbMasstab.Text = " 1 : " & Format(balkenbreiteInMeter, "##,##0#")
    End Sub

    Private Sub showLayersliste()
        leereSelectedlayersNachPres(layersSelected)
        ' reduziereEtitketten()
        tbhgrund.Text = layerHgrund.titel
        tbhgrund.Tag = layerHgrund.aid
        tbhgrund.Uid = CType(layerHgrund.sid, String)
        layerHgrund.dokutext = clsWebgisPGtools.bildeDokuTooltip(layerHgrund)
        tbhgrund.ToolTip = layerHgrund.dokutext

        MainListBox.ItemsSource = Nothing
        MainListBox.ItemsSource = layersSelected
    End Sub

    Sub presentMap()
        Try
            kartengen.aktMap.aktcanvas.w = CLng(cv1.Width) : kartengen.aktMap.aktcanvas.h = CLng(cv1.Height)
            skalieren()
            clsStartup.setzeAktKoordinate()
            suchCanvas.Children.Clear()
            clearAllSlots()
            GC.Collect()

            zwischenbildBitteWarten()
            slots(0).BildGenaufrufMAPserver(slots(0).mapfile, myglobalz.serverWeb, kartengen.aktMap)
            If slots(0).refresh Then
                If slots(0).layer.titel.ToLower = "kein hintergrund" Then
                Else
                    MapModeAbschicken(slots(0)) 'hintergrund
                End If
            End If

            slots(1).BildGenaufrufMAPserver(slots(1).mapfile, myglobalz.serverWeb, kartengen.aktMap)
            If slots(1).refresh Then MapModeAbschicken(slots(1)) 'vordergrund

            'os_objekt
            If os_tabelledef.tabelle.IsNothingOrEmpty Or os_tabelledef.gid.IsNothingOrEmpty Then
                slots(2).aufruf = "fehler"
            Else
                If OSrefresh Then
                    'slots(2).BildGenaufrufMAPserver(slots(2).mapfile, myglobalz.serverWeb, kartengen.aktMap)
                    slots(2).aufruf = clsAufrufgenerator.bildeAufrufEinzelOS(os_tabelledef)
                    If slots(2).aufruf <> "fehler" Then
                        MapModeAbschicken(slots(2)) 'objektsuche
                    End If
                End If
            End If

            imageMapCanvas.Children.Clear()
            If sollImagemapDarstellen() Then
                layerActive.masstab_imap = masstabsKorrektur(layerActive.titel, layerActive.masstab_imap)
                kartengen.imageMap = genImageMapTextstring()
                If kartengen.imageMap <> String.Empty Then
                    kartengen.imageMap = kartengen.imageMap.Replace(Chr(39), Chr(34))
                    maleImageMap()
                Else
                    l("leer")
                End If
            Else
                l("es soll keine imagemap erzeugt werden")
            End If
            If Not CanvasClickModus = "pan" Then
                imageMapCanvas.Visibility = Visibility.Visible ' nein stört beim panning
            End If
            Dim inselnImSuchPolygon As Integer = handleSuchOBJData(suchCanvas, aktFST.normflst.serials)
            SuchOBJNachrichtAusgeben(inselnImSuchPolygon, aktPolygon.serials.Count)
        Catch ex As Exception
            l("fehler in presentMap: " & ex.ToString)
        End Try
    End Sub

    Private Shared Function sollImagemapDarstellen() As Boolean
        l("sollImagemapDarstellen------------------------------")
        If myglobalz.NoImageMap Then Return False
        If layerActive.aid = 0 Or layerActive.mapFileHeader = String.Empty Then
            l("layerActive.aid = 0 Or layerActive.mapFileHeader ungültig  ")
            Return False
        Else
            If CDbl(layerActive.masstab_imap) > aktMasstab Then
                l("es soll eine imap erzeugt werden ")
                Return True
            Else
                l("ausserhalb vom imagemapmasstab  ")
                Return False
            End If

        End If
        ' Return layerActive.mapFileHeader = String.Empty OrElse (Not erzeugeImagemap)
    End Function

    Private Function maleImageMap() As Integer
        Dim inselnInImageMap As Integer
        Dim newPcoll As New List(Of pointCollectionPlus)
        Dim dummy As Integer = 0
        Dim imapPointPLusColl As New List(Of pointCollectionPlus)
        Try
            If istImageMapOK() Then
                ' modImagemapDisp.cleanupImagemap(kartengen.imageMap) 
                imapPointPLusColl = imageMap2POintCollLIstMAPSERVER(kartengen.imageMap, False)
                imapPointPLusColl = prep(imapPointPLusColl, inselnInImageMap)
                If imapPointPLusColl Is Nothing Then
                    l("Warnung imapPointPLusColl is nothing")
                End If
                If inselnInImageMap > 10 Then
                    inselnInImageMap = 10
                End If
                '  MsgBox(inselCountImSuchPolygon.ToString)
                For j = 0 To inselnInImageMap + 1
                    newPcoll = bildeNeueListe(imapPointPLusColl)
                    If newPcoll IsNot Nothing Then
                        imapPointPLusColl = prep(newPcoll, dummy)
                        If imapPointPLusColl Is Nothing Then
                            l("Warnung imapPointPLusColl is nothing")
                        End If
                    End If

                Next
                paintIMapCollection(imapPointPLusColl, imageMapCanvas)
                'allesImWebbrowserdarstellen(BILDaufruf, BILDaufrufRange0)
            End If
            Return inselnInImageMap
        Catch ex As Exception
            l("fehler in maleImageMap: " & ex.ToString)
            Return Nothing
        End Try
    End Function

    Private Function genImageMapTextstring() As String
        Dim imagemap As String
        If CDbl(layerActive.masstab_imap) > aktMasstab Then
            'isok
            imgKarte.Width = cv1.Width
            imgKarte.Height = cv1.Height
            Dim IMAPaufruf As String = kartengen.ImapGenaufrufMAPserver(layerActive.mapFileHeader)
            Dim hinweis As String = ""
            imagemap = meineHttpNet.meinHttpJob("", IMAPaufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
        Else
            'keineimap
            imagemap = String.Empty
        End If
        Return imagemap
    End Function

    Private Shared Function masstabsKorrektur(layertitle As String, layerImapScale As String) As String ' layerActive.titel,layerActive.masstab_imap
        Try
            If layertitle.ToLower.Contains("lurkart") Then ' flurkarte
                'korrektur weil in der db selten ein wert angegeben wurde
                layerImapScale = CType(2000, String)
            End If
            If CInt(layerImapScale) < 1 Then
                'korrektur weil in der db selten ein wert angegeben wurde
                layerImapScale = CType(10000, String)
            End If
            Return layerImapScale
        Catch ex As Exception
            l("fehler in masstabsKorrektur " & ex.ToString)
            Return "5000"
        End Try
    End Function

    Private Sub allesImWebbrowserdarstellen(BILDaufruf As String, BILDaufrufRange0 As String)
        'myWebBrowser.Width = mapCanvas.Width
        'myWebBrowser.Height = mapCanvas.Height
        'Dim head = "<img  src='" & BILDaufrufRange0 &
        '    "'   border='0'  z-index=2 " &
        '    " height='" & "600" & "' width='" & "600" & "'>" &
        '"<img id='m-imagemap' name='m-imagemap 'src='" & BILDaufruf &
        '    "' usemap='#m-imagemap' border='0' z-index=3 align='top'" &
        '    " height='" & myWebBrowser.Height & "' width='" & myWebBrowser.Width & "'>"
        'myWebBrowser.NavigateToString(head & kartengen.imageMap)
    End Sub

    Private Function handleSuchOBJData(sCanvas As Canvas, serials As List(Of String)) As Integer
        Dim inselCount As Integer
        l("handleSuchOBJData-------------------------------")
        If serials Is Nothing OrElse serials.Count < 1 Then
            l("handleSuchOBJData---- aktFST.serials Is Nothing OrElse aktFST.serials.Count < 1")
            Return -1
        End If

        'btnSuchobjAusSchalten.Visibility = Visibility.Visible
        Dim myFillColorBrush, myStrokeColorBrush As New SolidColorBrush
        Dim zindex = 100
        Dim zaehler As Integer = 0
        Dim kreiscanvas As New clsCanvas
        Dim tooltip, tag As String
        aktPolygon.serials = serials
        If suchObjektModus = "fst" Then
            'myStrokeColorBrush = New SolidColorBrush(Color.FromArgb(80, 0, 0, 0)) ' Brushes.DarkBlue
            myStrokeColorBrush = New SolidColorBrush(Colors.OrangeRed) ' Brushes.DarkBlue
            ' myFillColorBrush = New SolidColorBrush(Color.FromArgb(80, 250, 100, 0)) ' Brushes.DarkBlue
            myFillColorBrush = Nothing
        Else
            'puffer
            myStrokeColorBrush = New SolidColorBrush(Colors.Aquamarine) ' Brushes.DarkBlue
            '  myStrokeColorBrush = New SolidColorBrush(Color.FromArgb(80, 0, 0, 0)) ' Brushes.DarkBlue
            '  myFillColorBrush = New SolidColorBrush(Color.FromArgb(80, 10, 250, 250)) ' Brushes.DarkBlue
        End If

        tag = "killme"
        tooltip = "Zum Löschen links auf Knopf 'Suchobjekt unsichtbar' klicken"
        kreiscanvas.w = CLng(sCanvas.Width)
        kreiscanvas.h = CLng(sCanvas.Height)
        suchCanvas.Visibility = Visibility.Visible
        aktPolygon.Typ = RaumbezugsTyp.Polygon 'RaumbezugsTyp.Flurstueck

        'aktPolygon.GKstringList = polygonparser.gkstringsausserial_generieren(aktPolygon.ShapeSerial)
        'GKstringList 483069.128;5539515.615;483073.845;5539526.352;482992.036;5539553.982;482988.477;5539543.156;483069.128;5539515.615
        Dim dummy As Integer = 0

        Dim simplelist As New List(Of PointCollection)
        Dim newPcoll As New List(Of pointCollectionPlus)

        Dim inseln As Integer = bearbeiteSerials(sCanvas, inselCount, myFillColorBrush, myStrokeColorBrush, zindex, zaehler,
                                                 kreiscanvas, tooltip, tag, dummy, simplelist, newPcoll, aktPolygon)
        Return inseln
    End Function

    Private Function bearbeiteSerials(sCanvas As Canvas, ByRef inselCount As Integer, myFillColorBrush As SolidColorBrush,
                                      myStrokeColorBrush As SolidColorBrush, zindex As Integer,
                                      zaehler As Integer, kreiscanvas As clsCanvas,
                                      tooltip As String, tag As String,
                                      ByRef dummy As Integer,
                                      ByRef simplelist As List(Of PointCollection),
                                      ByRef newPcoll As List(Of pointCollectionPlus),
                                      lokPolygon As clsParapolygon) As Integer
        Try
            For i = 0 To lokPolygon.serials.Count - 1
                bereinigt.Clear()
                lokPolygon.ShapeSerial = lokPolygon.serials(i)
                simplelist = New List(Of PointCollection)
                simplelist = modWKT.wkt2PointCollList(lokPolygon.ShapeSerial)
                If simplelist Is Nothing Then
                    Continue For
                End If
                bereinigt = simpleList2PLuslist(simplelist)
                bereinigt = prep(bereinigt, inselCount)
                If bereinigt Is Nothing Then
                    l("Warnung bereinigt is nothing1")
                End If
                newPcoll = New List(Of pointCollectionPlus)
                dummy = 0
                inselnEntfernen(dummy, inselCount, newPcoll)
                lokPolygon.GKstringList = modWKT.gkstringsAusPointColl_generieren(modWKT.bereinigt)
                addPolygonFromDBToCanvas(lokPolygon, tag, kartengen.aktMap.aktrange, sCanvas, kreiscanvas,
                                             myFillColorBrush, myStrokeColorBrush, "",
                                             zindex, zaehler, tooltip)
            Next
            l("handleSuchOBJData------------------------------ ende inseln:-" & inselCount)
            Return inselCount
        Catch ex As Exception
            l("warnung in handleSuchOBJData: " & ex.ToString)
            Return -1
        End Try
    End Function

    Private Sub SuchOBJNachrichtAusgeben(inselCount As Integer, aktPolygon_serials_Count As Integer)
        Exit Sub
        Dim inselNachricht As String = ""
        If inselCount > 0 Then
            inselNachricht = "Das Objekt hat " & inselCount & " Inseln !" & Environment.NewLine
        End If
        If aktPolygon_serials_Count > 1 Then
            inselNachricht = inselNachricht & "Das Objekt besteht aus " & aktPolygon.serials.Count & " Teilflächen ! " & Environment.NewLine
        End If
        If inselNachricht <> String.Empty Then
            If suchCanvas.Visibility = Visibility.Visible Then
                MessageBox.Show(inselNachricht, "Hinweis zu Objekt", MessageBoxButton.OK, MessageBoxImage.Information)
            End If
        End If
    End Sub

    Private Shared Sub inselnEntfernen(ByRef dummy As Integer, ucount As Integer, ByRef newPcoll As List(Of pointCollectionPlus))
        Try
            l(" inselnEntfernen ----------------------------------")
            If bereinigt Is Nothing Then
                l("warnung inselnEntfernen bereinigt   is nothing abbruch")
                Exit Sub
            End If
            For j = 0 To ucount + 1
                newPcoll = bildeNeueListe(bereinigt)
                If newPcoll IsNot Nothing Then
                    bereinigt = prep(newPcoll, dummy)
                    If bereinigt Is Nothing Then
                        l("warnung bereinigt is nothing 2")
                    End If
                End If

            Next
            l(" inselnEntfernen ---------------- ende ------------------")
        Catch ex As Exception
            l("fehler in inselnEntfernen: " & ex.ToString)
        End Try
    End Sub

    Private Function simpleList2PLuslist(simplelist As List(Of PointCollection)) As List(Of pointCollectionPlus)
        Dim newpbird As New pointCollectionPlus
        Try
            l(" directionErgaenzen ----------------------------------")
            If simplelist Is Nothing Then
                l("warnung simpleList2PLuslist simplelist   is nothing abbruch")
                Return Nothing
            End If

            For Each tb As PointCollection In simplelist
                newpbird = New pointCollectionPlus
                newpbird.pcoll = tb
                modWKT.bereinigt.Add(newpbird)
            Next
            l(" directionErgaenzen ---------- ende ------------------------")
            Return modWKT.bereinigt
        Catch ex As Exception
            l("fehler in simpleList2PLuslist " & ex.ToString)
            Return Nothing
        End Try
    End Function

    Private Shared Sub paintIMapCollection(imapPointPLusColl As List(Of pointCollectionPlus), lokcanvas As Canvas)
        Try
            l(" paintIMapCollection ---------------------- anfang")
            If imapPointPLusColl Is Nothing Then Exit Sub
            For Each tp As pointCollectionPlus In imapPointPLusColl
                If Not IsNothing(tp.pcoll) Then
                    polygonmalen(tp.href, tp.title, tp.pcoll, lokcanvas)
                End If
            Next
            l(" paintIMapCollection ---------------------- ende")

        Catch ex As Exception
            l("Fehler in paintIMapCollection: " & ex.ToString())

        End Try
    End Sub
    Private Shared Function calcMiddleFromPointarray(punktarrayInM() As myPoint) As myPoint
        Dim midlle As New myPoint
        Try
            l("calcMiddleFromPointcollection---------------------- anfang")
            Dim xmin, xmax, ymin, ymax As Double
            xmin = 1000000000 : ymin = 1000000000 : xmax = 0 : ymax = 0
            For i = 0 To punktarrayInM.Count - 1
                If punktarrayInM(i).X < xmin Then xmin = punktarrayInM(i).X
                If punktarrayInM(i).Y < ymin Then ymin = punktarrayInM(i).Y
                If punktarrayInM(i).X > xmax Then xmax = punktarrayInM(i).X
                If punktarrayInM(i).Y > ymax Then ymax = punktarrayInM(i).Y
            Next
            midlle.X = (xmax - xmin) / 2 + xmin
            midlle.Y = (ymax - ymin) / 2 + ymin
            l("calcMiddleFromPointcollection---------------------- ende")
            Return midlle
        Catch ex As Exception
            l("Fehler in calcMiddleFromPointcollection: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Private Shared Sub addPolygonFromDBToCanvas(clsParapolygon As clsParapolygon,
                                ByVal tag As String,
                                dierange As clsRange,
                                ByVal myCanvas As Canvas,
                                kreiscanvas As clsCanvas,
                                ByVal myFillColorBrush As SolidColorBrush,
                                ByVal myStrokeColorBrush As SolidColorBrush,
                                ByVal name As String,
                                ByVal zindex As Integer,
                                zaehler As Integer,
                                tooltip As String)
        Dim neupointsCanvas() As myPoint
        Dim dezimalTrenner As Char
        Dim myPointCollection As New PointCollection
        Dim multipolygonpointer() As Integer = Nothing
        Dim i As Integer
        Dim middle As New myPoint
        l("addPolygonFromDBToCanvas-------------------")
        Try
            If clsParapolygon.GKstringList Is Nothing Then
                l("warnung clsParapolygon.GKstringList Is Nothing abbruch")
                Exit Sub
            End If
            If clsParapolygon.GKstringList.Count = 0 Then
                l("warnung clsParapolygon.GKstringList.Count = 0 abbruch")
                Exit Sub
            End If
            dezimalTrenner = bestimmeDezimalTrenner(clsParapolygon.GKstringList)
            For i = 0 To clsParapolygon.GKstringList.Count - 1 'multipolygonschleife
                clsParapolygon.GKstring = clsParapolygon.GKstringList(i)
                punktarrayInM = clsMiniMapTools.zerlegeInPunkte(clsParapolygon.GKstring, dezimalTrenner, multipolygonpointer,
                                                                                  CInt(clsParapolygon.RaumbezugsID), clsParapolygon.ShapeSerialstringIstWKT)

                middle = calcMiddleFromPointarray(punktarrayInM)

                middle = clsMiniMapTools.punktvonGKnachCanvasUmrechnen(middle, dierange, kreiscanvas, divisor:=100)
                If punktarrayInM IsNot Nothing Then
                    neupointsCanvas = clsMiniMapTools.polygonNachCanvasUmrechnen(punktarrayInM, dierange, kreiscanvas)
                    ReDim Preserve multipolygonpointer(multipolygonpointer.GetUpperBound(0) + 1)
                    multipolygonpointer(multipolygonpointer.GetUpperBound(0)) = neupointsCanvas.GetUpperBound(0)
                    Dim koordCursor As Integer = 0
                    Dim anzahlKeyPoints = multipolygonpointer(koordCursor)
                    Dim lokzaehler As Integer = zaehler
                    addPolygonSchleifeKeypoints(clsParapolygon, tag, myCanvas, name, zindex, zaehler, tooltip, neupointsCanvas, myPointCollection,
                                               multipolygonpointer, koordCursor, anzahlKeyPoints, lokzaehler, myFillColorBrush,
                                               myStrokeColorBrush, middle)

                    neupointsCanvas = Nothing
                End If
            Next
        Catch ex As Exception
            l("fehler in addPolygonFromDBToCanvas-------------------", ex)
        End Try
    End Sub
    Private Shared Sub addPolygonSchleifeKeypoints(ByVal clsParapolygon As clsParapolygon,
                                                    ByVal tag As String,
                                                    ByVal myCanvas As Canvas,
                                                    ByVal name As String,
                                                    ByVal zindex As Integer,
                                                    ByVal zaehler As Integer,
                                                    ByVal tooltip As String,
                                                    ByVal neupointsCanvas As myPoint(),
                                                    ByVal myPointCollection As PointCollection,
                                                    ByVal multipolygonpointer As Integer(),
                                                    ByVal koordCursor As Integer,
                                                    ByVal anzahlKeyPoints As Integer,
                                                    ByVal lokzaehler As Integer,
                                                    ByVal myFillColorBrush As SolidColorBrush,
                                                    ByVal myStrokeColorBrush As SolidColorBrush,
                                                     middle As myPoint)
        Dim x As Double
        Dim y As Double
        Try
            Dim atest = neupointsCanvas.GetUpperBound(0)
            For i = 0 To neupointsCanvas.GetUpperBound(0)
                lokzaehler = lokzaehler + i
                If IsNothing(neupointsCanvas(i)) Then
                    Continue For
                End If
                If IsNothing(neupointsCanvas(i).X) Then
                    Continue For
                End If
                x = CInt(neupointsCanvas(i).X)
                y = CInt(neupointsCanvas(i).Y)
                myPointCollection.Add(New Point(x, y))
                'If i = anzahlKeyPoints -1  Then  raus, weil sonst fehlt ein keypoint in linien 26084
                If i = anzahlKeyPoints Then
                    If clsParapolygon.Typ = RaumbezugsTyp.Polygon Or clsParapolygon.Typ = RaumbezugsTyp.Flurstueck Then
                        drawPolygon2Canvas(tag, name, lokzaehler, myPointCollection, myCanvas, zindex, tooltip, myFillColorBrush,
                                                     myStrokeColorBrush, 1)
                        drawPolygon2Canvas(tag, name, lokzaehler, myPointCollection, myCanvas, zindex, tooltip, myFillColorBrush,
                                               myStrokeColorBrush, 2)
                        If clsParapolygon.Typ = RaumbezugsTyp.Polygon Then
                            drawFadenkreuz2Canvas(tag, name, lokzaehler, myPointCollection, myCanvas, zindex, tooltip, myFillColorBrush,
                                    myStrokeColorBrush, 3, middle)
                        End If
                    End If
                    If clsParapolygon.Typ = RaumbezugsTyp.Polyline Then
                        drawPolyline2Canvas(tag, name, zaehler, myPointCollection, myCanvas, 10000, myFillColorBrush,
                                                     myStrokeColorBrush)
                    End If
                    myPointCollection.Clear()
                    koordCursor += 1
                    If koordCursor <= multipolygonpointer.GetUpperBound(0) Then
                        anzahlKeyPoints = multipolygonpointer(koordCursor)
                    End If
                End If
            Next
        Catch ex As Exception
            'nachricht("fehler In addPolygonSchleifeKeypoints: " & Environment.NewLine &   clsParapolygon.GKstring.tostring & Environment.NewLine & ex.ToString)
            nachricht("fehler in addPolygonSchleifeKeypoints: " & Environment.NewLine & Environment.NewLine & ex.ToString)
        End Try
    End Sub

    Private Shared Sub drawFadenkreuz2Canvas(ByVal href As String,
                                    ByVal title As String,
                                    ByVal zaehler As Integer,
                                    ByVal myPointCollection As PointCollection,
                                    ByVal canvas1 As Canvas,
                                    zindex As Integer,
                                    tooltip As String,
                                    ByVal myFillColorBrush As SolidColorBrush,
                                    ByVal myStrokeColorBrush As SolidColorBrush,
                                          nummer As Integer,
                                             middle As myPoint)

        Dim myelli As New Ellipse
        'Dim dashes As DoubleCollection = New DoubleCollection()


        Try

            myelli.Name = "ellipse" & zaehler
            myelli.ToolTip = tooltip
            myelli.Tag = href
            myelli.Stroke = Brushes.DarkBlue
            myelli.StrokeThickness = 1
            '  myelli.Fill = myFillColorBrush 'myBrush
            'neu
            'If nummer = 1 Then
            '    dashes.Add(2)
            '    dashes.Add(1)
            '    dashes.Add(4)
            '    dashes.Add(1)
            '    dashes.Add(2)
            '    dashes.Add(1)
            '    myelli.Stroke = myStrokeColorBrush ' Brushes.DarkBlue
            'Else
            '    dashes.Add(2)
            '    dashes.Add(2)
            '    myelli.Stroke = Brushes.Aquamarine ' Brushes.DarkBlue
            'End If


            'myelli.StrokeDashArray = dashes
            myelli.Opacity = 90
            myelli.SnapsToDevicePixels = True
            'myPolygon.StrokeThickness = 1
            ' myelli.Cursor = Cursors.Hand
            'myelli.Points = myPointCollection.Clone
            myelli.Height = 5 : myelli.Width = 5

            canvas1.Children.Add(myelli)
            Canvas.SetZIndex(myelli, zindex)
            Canvas.SetLeft(myelli, CInt(middle.X) - (myelli.Width / 2))
            Canvas.SetTop(myelli, CInt(middle.Y) - (myelli.Height / 2))
            'Canvas.SetLeft(myelli, CInt(middle.X))
            'Canvas.SetTop(myelli, CInt(middle.Y))
        Catch ex As Exception
            l("fehler in drawPolygon2Canvas" & ex.ToString)
        End Try
    End Sub
    Private Shared Sub drawPolyline2Canvas(ByVal href As String,
                                      ByVal title As String,
                                      ByVal zaehler As Integer,
                                      ByVal myPointCollection As PointCollection,
                                      ByVal canvas1 As Canvas,
                                      zindex As Integer,
                                        ByVal myFillColorBrush As SolidColorBrush,
                                        ByVal myStrokeColorBrush As SolidColorBrush)
        'withevents muss auf klassenebene deklariert sein   Private WithEvents myPolygon As Polygon
        Dim myPolygon As New Polyline
        '  Dim myBrush As SolidColorBrush = New SolidColorBrush(Color.FromArgb(20, 0, 100, 250)) 'transparenz ist der erste wert
        Try
            myPolygon.Name = "poly" & zaehler
            myPolygon.ToolTip = href
            myPolygon.Tag = href
            myPolygon.Stroke = myStrokeColorBrush 'Brushes.DarkBlue
            '  myPolygon.Fill = myBrush
            myPolygon.Opacity = 90
            myPolygon.StrokeThickness = 4
            '    myPolygon.Cursor = Cursors.Hand
            myPolygon.Points = myPointCollection.Clone
            '  AddHandler myPolygon.MouseDown, AddressOf Polygon_MouseDown
            canvas1.Children.Add(myPolygon)
            Canvas.SetZIndex(myPolygon, zindex)
            Canvas.SetLeft(myPolygon, 0)
            Canvas.SetTop(myPolygon, 0)
        Catch ex As Exception
            l("drawPolyline2Canvas" & ex.ToString)
        End Try
    End Sub

    Private Shared Sub drawPolygon2Canvas(ByVal href As String,
                                    ByVal title As String,
                                    ByVal zaehler As Integer,
                                    ByVal myPointCollection As PointCollection,
                                    ByVal canvas1 As Canvas,
                                    zindex As Integer,
                                    tooltip As String,
                                    ByVal myFillColorBrush As SolidColorBrush,
                                    ByVal myStrokeColorBrush As SolidColorBrush,
                                          nummer As Integer)

        Dim myPolygon As New Polygon
        Dim dashes As DoubleCollection = New DoubleCollection()
        Try
            myPolygon.Name = "poly" & zaehler
            myPolygon.ToolTip = tooltip
            myPolygon.Tag = href
            myPolygon.Stroke = myStrokeColorBrush ' Brushes.DarkBlue
            myPolygon.StrokeThickness = 5
            myPolygon.Fill = myFillColorBrush 'myBrush
            'neu
            If nummer = 1 Then
                dashes.Add(2)
                dashes.Add(1)
                dashes.Add(4)
                dashes.Add(1)
                dashes.Add(2)
                dashes.Add(1)
                myPolygon.Stroke = myStrokeColorBrush ' Brushes.DarkBlue
            Else
                dashes.Add(2)
                dashes.Add(2)
                myPolygon.Stroke = Brushes.Aquamarine ' Brushes.DarkBlue
                clsTooltipps.setTooltipSuchobjektPolygon(myPolygon, "alles")
            End If


            myPolygon.StrokeDashArray = dashes
            myPolygon.Opacity = 90
            myPolygon.SnapsToDevicePixels = True

            'AddHandler myPolygon.MouseDown, AddressOf Suchobjekt_Polygon_MouseDown


            'myPolygon.StrokeThickness = 1
            myPolygon.Cursor = Cursors.Hand
            myPolygon.Points = myPointCollection.Clone
            canvas1.Children.Add(myPolygon)
            Canvas.SetZIndex(myPolygon, zindex)
            Canvas.SetLeft(myPolygon, 0)
            Canvas.SetTop(myPolygon, 0)
        Catch ex As Exception
            l("fehler in drawPolygon2Canvas" & ex.ToString)
        End Try
    End Sub
    'Shared Sub Suchobjekt_Polygon_MouseDown(sender As Object, e As MouseButtonEventArgs)
    '    Dim eee As System.Windows.Shapes.Polygon = DirectCast(e.Source, System.Windows.Shapes.Polygon)
    '    Debug.Print(eee.Tag.ToString)


    '    imgpin.Visibility = Visibility.Collapsed
    '    'suchCanvas.Visibility = Visibility.Collapsed


    '    'System.Windows.Application.Current.MainWindow.ima
    '    'imgpin.Visibility = Visibility.Collapsed
    '    'suchCanvas.Visibility = Visibility.Collapsed
    'End Sub

    Private Shared Function istImageMapOK() As Boolean
        Try
            If kartengen.imageMap Is Nothing Then Return False
            If kartengen.imageMap.ToLower.Contains("Search returned no results".ToLower) OrElse
                kartengen.imageMap = String.Empty Then
                l("warnung in istImageMapOK: Search returned no results, es wurde keine imagemap erzeugt: " & kartengen.imageMap & Environment.NewLine &
" layerActive.aid = 0 Or layerActive.mapFileHeader = String.Empty" & layerActive.aid & " " & layerActive.mapFileHeader & Environment.NewLine &
layerActive.titel)
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            l("fehler in istImageMapOK" & ex.ToString)
            Return False
        End Try
    End Function

    Private Sub showAktVorgangsid()
        If aktvorgangsid.Trim.Length > 2 Then
            tbVorgangsid.Text = aktvorgangsid
            'MsgBox(aktvorgangsid)
        End If
    End Sub
    Private Sub skalieren()
        Dim pixcanvas As New clsCanvas
        pixcanvas.w = CLng(cv1.Width)
        pixcanvas.h = CLng(cv1.Height)
        Dim handle As New clsScalierung
        nachricht("presentMap: vor skaliereung ")
        clsScalierung.Skalierung(72, "ZB", 1, kartengen.aktMap.aktrange, CInt(pixcanvas.w), CInt(pixcanvas.h), 1,
                                           kartengen.aktMap.aktrange, pixcanvas)
        nachricht("presentMap: nach skaliereung ")
    End Sub

    Sub initdb()
        adrREC.mydb = New clsDatenbankZugriff
        adrREC.mydb.Host = postgresHost
        adrREC.mydb.username = "postgres" : adrREC.mydb.password = "lkof4"
        adrREC.mydb.Schema = "postgis20"
        adrREC.mydb.Tabelle = "flurkarte.basis_f" : adrREC.mydb.dbtyp = "postgis"

        basisrec.mydb = New clsDatenbankZugriff
        basisrec.mydb.Host = postgresHost
        basisrec.mydb.username = "postgres" : basisrec.mydb.password = "lkof4"
        basisrec.mydb.Schema = "postgis20"
        basisrec.mydb.Tabelle = "flurkarte.basis_f" : basisrec.mydb.dbtyp = "postgis"

        OSrec.mydb = New clsDatenbankZugriff
        OSrec.mydb.Host = postgresHost
        OSrec.mydb.username = "postgres" : OSrec.mydb.password = "lkof4"
        OSrec.mydb.Schema = "postgis20"
        OSrec.mydb.Tabelle = "flurkarte.basis_f" : OSrec.mydb.dbtyp = "postgis"

        webgisREC.mydb = New clsDatenbankZugriff
        webgisREC.mydb.Host = postgresHost
        webgisREC.mydb.username = "postgres" : webgisREC.mydb.password = "lkof4"
        webgisREC.mydb.Schema = "webgiscontrol"
        webgisREC.mydb.Tabelle = "flurkarte.basis_f" : webgisREC.mydb.dbtyp = "postgis"

        paradigmaMsql.mydb = New clsDatenbankZugriff
        paradigmaMsql.mydb.Host = myglobalz.mssqlhost
        paradigmaMsql.mydb.username = "sgis" : paradigmaMsql.mydb.password = "WinterErschranzt.74"
        paradigmaMsql.mydb.Schema = "Paradigma"
        paradigmaMsql.mydb.Tabelle = "" : paradigmaMsql.mydb.dbtyp = "sqls"

        pLightMsql.mydb = New clsDatenbankZugriff
        pLightMsql.mydb.Host = myglobalz.mssqlhost
        pLightMsql.mydb.username = "sgis" : pLightMsql.mydb.password = "WinterErschranzt.74"
        pLightMsql.mydb.Schema = "GIS"
        pLightMsql.mydb.Tabelle = "" : pLightMsql.mydb.dbtyp = "sqls"
#If DEBUG Then
        If Environment.UserName = "hurz" Then
            pLightMsql.mydb.username = "serveradmin" : pLightMsql.mydb.password = "lkof4"
            paradigmaMsql.mydb.username = "serveradmin" : paradigmaMsql.mydb.password = "lkof4"
        End If
#End If
    End Sub
    Private Sub initVGCanvasSize()
        dockTop.Height = 50
        'dockMenu.Width = 0

        cv1.Width = CLng(System.Windows.SystemParameters.PrimaryScreenWidth) - CLng(dockMenu.Width)
        cv1.Height = CLng(System.Windows.SystemParameters.PrimaryScreenHeight) - CLng(dockTop.Height)
        globCanvasWidth = CInt(cv1.Width)
        globCanvasHeight = CInt(cv1.Height)
        slotsResize(cv1.Width, cv1.Height)
        'cv1.Width = CLng(Me.Width) - CLng(dockMenu.Width)
        'cv1.Height = CLng(Me.Height) - CLng(dockTop.Height)
    End Sub

    Private Sub setCanvasSizes()
        Dim faktor = 1

        'cv1.Width = CLng(Me.Width) - CLng(dockMenu.Width)
        'cv1.Height = CLng(Me.Height) - CLng(dockTop.Height)

        OSmapCanvas.Width = CLng(cv1.Width * faktor)
        OSmapCanvas.Height = CLng(cv1.Height * faktor)
        imageMapCanvas.Width = CLng(cv1.Width * faktor)
        imageMapCanvas.Height = CLng(cv1.Height * faktor)

        suchCanvas.Width = CLng(cv1.Width * faktor)
        suchCanvas.Height = CLng(cv1.Height * faktor)
        kreisUebersichtCanvas.Width = CLng(cv1.Width * faktor)
        kreisUebersichtCanvas.Height = CLng(cv1.Height * faktor)

        cvPDFrechteck.Width = CLng(cv1.Width * faktor)
        cvPDFrechteck.Height = CLng(cv1.Height * faktor)

        stContext.Width = cv1.Width '- 100
        stContext.Height = cv1.Height ' - 100

        stContext2.Width = cv1.Width '- 100
        stContext3.Width = cv1.Width '- 100

        stwinthemen.Width = cv1.Width
        stwinthemen.Height = cv1.Height

        stpDokuUndLegende.Width = stContext.Width - stpKnoeppeVertical.Width - 50 '50=margins
        stpObjektsuche.Width = stContext.Width - stpKnoeppeVertical.Width - 50 '50=margins

        svMainScrollviewer.Height = cv1.Height - spMenueHead.Height
        MainListBox.Height = cv1.Height - spMenueHead.Height
        'spMenueHead.Height
    End Sub



    Sub setMapImageSize()
        kartengen.aktMap.aktcanvas.w = CLng(cv1.Width)
        kartengen.aktMap.aktcanvas.h = CLng(cv1.Height)
    End Sub
    'Sub vgmyBitmapImage_DownloadCompleted(sender As Object, e As RoutedEventArgs)
    '    VGcanvasImage.Source = vgmyBitmapImage
    'End Sub
    Sub MapModeAbschicken(aslot As clsSlot)
        Try
            If aslot.aufruf.IsNothingOrEmpty Then
                l("MapModeAbschicken aufrufstring ist leer ")
                Exit Sub
            End If
            aslot.bitmap = New BitmapImage
            aslot.bitmap.BeginInit()
            aslot.bitmap.UriSource = New Uri(aslot.aufruf, UriKind.Absolute)
            aslot.bitmap.EndInit()
            l("aufruf: " & aslot.aufruf)
            AddHandler aslot.bitmap.DownloadCompleted, Function(sender, e) slotImage_DownloadCompleted(aslot.slotnr)
            AddHandler aslot.bitmap.DownloadFailed, Function(sender, e) slotImageDownloadFailed(aslot.slotnr)
            GC.Collect()
        Catch ex As Exception
            l("fehler in MapModeAbschicken2: " & aufruf & " /// " & ex.ToString)
        End Try
    End Sub
    Private Function slotImage_DownloadCompleted(slotnr As Integer) As EventHandler(Of ExceptionEventArgs)
        slots(slotnr).image.Source = slots(slotnr).bitmap
    End Function
    Private Function slotImageDownloadFailed(slotnr As Integer) As EventHandler(Of ExceptionEventArgs)
        Dim info As String = "Eine " & slots(slotnr).funktion & " '" & slots(slotnr).layer.titel & "' ist konnte nicht erstellt werden (Timeout).  " & Environment.NewLine & Environment.NewLine
        If slots(slotnr).layer.titel.ToLower.Contains("wms") Then
            info &= "Hinweis: WMS - Ebenen können von seiten des Anbieters (" & slots(slotnr).layer.ldoku.datenabgabe & ") zeitweise deaktiviert werden."
        Else

        End If
        info &= "Tipp: " & Environment.NewLine &
                " Probieren Sie es noch einmal (Auffrischen Taste) oder " & Environment.NewLine & Environment.NewLine &
                "  schalten sie diese Ebene vorübergehend aus !" & Environment.NewLine & Environment.NewLine &
                "  Der Admin wird informiert und kümmert sich um das Problem." & Environment.NewLine &
                "" & Environment.NewLine
        MessageBox.Show(info,
                        "Hoppla")
        l("fehler slotImageDownloadFailed " & slots(slotnr).funktion & " " & slots(slotnr).layer.titel)
    End Function

    Private Sub zoomin_Click(sender As Object, e As RoutedEventArgs)
        'neue range berechnen
        'darstellen
        myglobalz.mgisBackModus = False
        panningAusschalten()
        Dim breite As Double = kartengen.aktMap.aktrange.xdif()
        kartengen.aktMap.aktrange.xl = kartengen.aktMap.aktrange.xl + (breite / 3)
        kartengen.aktMap.aktrange.xh = kartengen.aktMap.aktrange.xh - (breite / 3)
        Dim hohe As Double = kartengen.aktMap.aktrange.ydif()
        kartengen.aktMap.aktrange.yl = kartengen.aktMap.aktrange.yl + (hohe / 3)
        kartengen.aktMap.aktrange.yh = kartengen.aktMap.aktrange.yh - (hohe / 3)
        refreshMap(True, True) ' 
        e.Handled = True
    End Sub

    Private Sub zoomout_Click(sender As Object, e As RoutedEventArgs)
        myglobalz.mgisBackModus = False
        panningAusschalten()
        'eigentuemerfunktionAusschalten()
        Dim breite As Double = kartengen.aktMap.aktrange.xdif()
        kartengen.aktMap.aktrange.xl = kartengen.aktMap.aktrange.xl - (breite / 3)
        kartengen.aktMap.aktrange.xh = kartengen.aktMap.aktrange.xh + (breite / 3)
        Dim hohe As Double = kartengen.aktMap.aktrange.ydif()
        kartengen.aktMap.aktrange.yl = kartengen.aktMap.aktrange.yl - (hohe / 3)
        kartengen.aktMap.aktrange.yh = kartengen.aktMap.aktrange.yh + (hohe / 3)
        refreshMap(True, True)
        e.Handled = True
    End Sub
    Private Sub globalfit_Click(sender As Object, e As RoutedEventArgs)
        myglobalz.mgisBackModus = False
        panningAusschalten()
        kreisUebersichtCanvas.Visibility = Visibility.Visible
#Disable Warning BC42025 ' Access of shared member, constant member, enum member or nested type through an instance
        dockMap.SetZIndex(kreisUebersichtCanvas, 0)
#Enable Warning BC42025 ' Access of shared member, constant member, enum member or nested type through an instance
        imgpin.Visibility = Visibility.Collapsed

        kartengen.aktMap.aktrange = clsStartup.setMapKreisRange()
        refreshMap(True, True)
        e.Handled = True
    End Sub

    Private Sub drawAktRange2Uebersicht(aktrange As clsRange)
        'Dim myBrush As SolidColorBrush
        Dim punkteCanvas, lu, ro, luPix, roPix As New myPoint
        Dim UeKanwas As New clsCanvas
        Dim kreisrange As New clsRange
        Try
            l("drawAktRange2Uebersicht---------------------- anfang")
            kreisUebersichtCanvas.Children.Remove(aktrangebox)
            aktrangebox = New Rectangle
            aktrangebox.Name = "aktrange"
            'myBrush = New SolidColorBrush(Color.FromArgb(20, 0, 0, 250)) 'transparenz ist der erste wert
            aktrangebox.Stroke = Brushes.Red
            aktrangebox.StrokeThickness = 4
            aktrangebox.Opacity = 90
            ' aktrangebox.Fill = myBrush
            punkteCanvas = New myPoint

            kreisrange = clsStartup.setMapKreisRange()

            UeKanwas.w = CLng(kreisUebersichtCanvas.Width)
            UeKanwas.h = CLng(kreisUebersichtCanvas.Height)
            l("kreisUebersichtCanvas.Width " & kreisUebersichtCanvas.Width)
            l("kreisUebersichtCanvas.Height " & kreisUebersichtCanvas.Height)
            UeKanwas.w = 390
            UeKanwas.h = 279

            lu.X = CDbl(CLng(kartengen.aktMap.aktrange.xl))
            lu.Y = CDbl(CLng(kartengen.aktMap.aktrange.yl))

            luPix = clsMiniMapTools.punktvonGKnachCanvasUmrechnen(lu, kreisrange, UeKanwas, divisor:=1)

            ro.X = CDbl(CLng(kartengen.aktMap.aktrange.xh))
            ro.Y = CDbl(CLng(kartengen.aktMap.aktrange.yh))

            roPix = clsMiniMapTools.punktvonGKnachCanvasUmrechnen(ro, kreisrange, UeKanwas, divisor:=1)
            kreisUebersichtCanvas.Children.Add(aktrangebox)
            kreisUebersichtCanvas.SetLeft(aktrangebox, luPix.X)
            kreisUebersichtCanvas.SetTop(aktrangebox, roPix.Y)
            Dim dx, dy As Double
            dx = Math.Abs(roPix.X - luPix.X)
            dy = Math.Abs(roPix.Y - luPix.Y)
            If dx > 5 Then
                aktrangebox.Width = dx
                aktrangebox.Height = dy
            Else
                aktrangebox.Width = 10
                aktrangebox.Height = 10
            End If
            'aktrangebox.Width = roPix.X - luPix.X
            'aktrangebox.Height = Math.Abs(roPix.Y - luPix.Y)
            l("drawAktRange2Uebersicht---------------------- ende")
        Catch ex As Exception
            l("Fehler in drawAktRange2Uebersicht: " & ex.ToString())
        End Try
    End Sub

    Private Sub rbfit_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        imgpin.Visibility = Visibility.Collapsed
        e.Handled = True
    End Sub
    Sub panningAusschalten()
        chkBoxPan.IsChecked = False
    End Sub
    Private Sub DrawRectangle(ByVal mycanvas As Canvas)
        aktrangebox = New Rectangle
        aktrangebox.Name = "rubberbox"
        Dim myBrush As SolidColorBrush = New SolidColorBrush(Color.FromArgb(20, 0, 0, 250)) 'transparenz ist der erste wert
        aktrangebox.Stroke = Brushes.Black
        aktrangebox.StrokeThickness = 2
        aktrangebox.Opacity = 90
        aktrangebox.Fill = myBrush
        System.Windows.Controls.Panel.SetZIndex(aktrangebox, 110)
        Canvas.SetZIndex(aktrangebox, 110)
        mycanvas.Children.Add(aktrangebox)
        System.Windows.Controls.Panel.SetZIndex(aktrangebox, 110)
        Canvas.SetZIndex(aktrangebox, 110)
    End Sub
    Private Sub RubberbandMove(ByVal e As System.Windows.Input.MouseEventArgs)
        If e.LeftButton = MouseButtonState.Pressed And RubberbandStartpt.HasValue Then
            Dim endpt As System.Windows.Point

            endpt = e.GetPosition(cv1)


            RubberbandEndpt = endpt
            Dim x0, y0, w, h As Double
            x0 = Math.Min(RubberbandStartpt.Value.X, endpt.X)
            y0 = Math.Min(RubberbandStartpt.Value.Y, endpt.Y)
            w = Math.Abs(endpt.X - RubberbandStartpt.Value.X)
            h = Math.Abs(endpt.Y - RubberbandStartpt.Value.Y)
            Canvas.SetLeft(aktrangebox, x0)
            Canvas.SetTop(aktrangebox, y0)
            Canvas.SetZIndex(aktrangebox, 110)
            aktrangebox.Width = w
            aktrangebox.Height = h
        End If
    End Sub
    Private Sub RubberbandFinish()
        chkBoxAusschnitt.IsChecked = False
        ausschnittNeuBerechnen(RubberbandStartpt, RubberbandEndpt)
        setBoundingRefresh(kartengen.aktMap.aktrange)
        refreshMap(True, True)
        aktrangebox.Width = 0
        aktrangebox.Height = 0
        Mouse.Capture(Nothing)
        RubberbandStartpt = Nothing
        RubberbandEndpt = Nothing
        'Me.Cursor = Nothing
        imageMapCanvas.Visibility = Visibility.Visible
        'Me.Cursor = System.Windows.InputCursors.ArrowCD
    End Sub
    Sub setBoundingRefresh(ByVal myrange As clsRange) 'ByVal xl As Double, ByVal xh As Double, ByVal yl As Double, ByVal yh As Double)
        kartengen.aktMap.aktrange.rangekopierenVon(myrange)
        'xdifKorrektur
        If kartengen.aktMap.aktrange.xdif() < 1 Then kartengen.aktMap.aktrange.xh += 1
        If kartengen.aktMap.aktrange.ydif() < 1 Then kartengen.aktMap.aktrange.yh += 1
        '  refreshMap(True, True)
    End Sub
    Shared Sub ausschnittNeuBerechnen(ByVal RubberbandStartpt As Point?, ByVal RubberbandEndpt As Point?)
        Try
            Dim newpoint As New myPoint
            newpoint.X = CDbl(RubberbandStartpt.Value.X)
            newpoint.Y = CDbl(RubberbandStartpt.Value.Y)
            Dim newpoint2 As New myPoint
            newpoint2.X = CDbl(RubberbandEndpt.Value.X)
            newpoint2.Y = CDbl(RubberbandEndpt.Value.Y)

            newpoint = clsAufrufgenerator.WINPOINTVonCanvasNachGKumrechnen(newpoint, kartengen.aktMap.aktrange, kartengen.aktMap.aktcanvas)
            newpoint2 = clsAufrufgenerator.WINPOINTVonCanvasNachGKumrechnen(newpoint2, kartengen.aktMap.aktrange, kartengen.aktMap.aktcanvas)
            If newpoint.X > newpoint2.X Then
                kartengen.aktMap.aktrange.xl = newpoint2.X
                kartengen.aktMap.aktrange.xh = newpoint.X
            Else
                kartengen.aktMap.aktrange.xl = newpoint.X
                kartengen.aktMap.aktrange.xh = newpoint2.X
            End If

            If newpoint.Y > newpoint2.Y Then
                kartengen.aktMap.aktrange.yl = newpoint2.Y
                kartengen.aktMap.aktrange.yh = newpoint.Y
            Else
                kartengen.aktMap.aktrange.yl = newpoint.Y
                kartengen.aktMap.aktrange.yh = newpoint2.Y
            End If
        Catch ex As Exception
            nachricht("Daneben. Bitte nochmal probieren!")
        End Try
    End Sub

    Private Sub RubberbandStart(ByVal e As System.Windows.Input.MouseButtonEventArgs)
        RubberbandStartpt = e.GetPosition(imageMapCanvas)
        'Me.Cursor = System.Windows.Input.Cursors.Cross
    End Sub
    Private Sub chkBoxPan_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        imgpin.Visibility = Visibility.Collapsed
        myglobalz.mgisBackModus = False
        'eigentuemerfunktionAusschalten()
        If chkBoxPan.IsChecked Then
            zeichneOverlaysGlob = True : zeichneImageMapGlob = False
            CanvasClickModus = "pan"
            imageMapCanvas.Visibility = Visibility.Collapsed
            cv1.Cursor = System.Windows.Input.Cursors.ScrollAll

            chkBoxPan.IsEnabled = True
            brdPan.IsEnabled = True
            spButtonMenu.ToolTip = "Bitte zuerst den Verschiebemodus beeenden"
        End If
        If Not chkBoxPan.IsChecked Then
            pannauss()
        End If
        e.Handled = True
    End Sub

    Private Sub pannauss()
        zeichneOverlaysGlob = True : zeichneImageMapGlob = True
        'refreshMap(True, True)
        CanvasClickModus = ""
        imageMapCanvas.Visibility = Visibility.Visible

        spButtonMenu.ToolTip = ""
    End Sub

    Private Sub disableMyStackpanel(mysp As StackPanel, modus As Boolean)
        For Each ele As FrameworkElement In mysp.Children
            ele.IsEnabled = modus
        Next
    End Sub

    Private Sub cvPDFrechteck_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) _
        Handles cvPDFrechteck.MouseLeftButtonDown, cvPDFrechteck.MouseLeftButtonDown
        isDraggingFlag = True
        origContentMousePoint = e.GetPosition(auswahlRechteck)
        e.Handled = True
    End Sub

    Private Sub cvPDFrechteck_MouseLeftButtonUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) _
        Handles cvPDFrechteck.MouseLeftButtonUp, cvPDFrechteck.MouseLeftButtonUp

        isDraggingFlag = False
        PDF_druckMassStab = PDF_postition_desRahmensBestimmen()
        e.Handled = True
    End Sub

    Private Function PDF_postition_desRahmensBestimmen() As Double
        Try
            l("PDF_postition_desRahmensBestimmen---------------------- anfang")
            Dim aleft = Canvas.GetLeft(auswahlRechteck)
            Dim btop = Canvas.GetTop(auswahlRechteck) '- 21
            '21 zuviel bei toop
            Debug.Print(cv1.Width & " " & cv1.Height & ", " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)

            Dim pixRange As New clsRange
            pixRange.xl = aleft
            pixRange.xh = aleft + auswahlRechteck.Width
            pixRange.yl = btop '- Hoehe_desTabcontrols
            pixRange.yh = btop + auswahlRechteck.Height '- Hoehe_desTabcontrols
            Dim a As String()

            Dim pixLinksUnten As New System.Windows.Point
            pixLinksUnten.X = pixRange.xl
            pixLinksUnten.Y = pixRange.yl
            Dim pixRechtsOben As New Point
            pixRechtsOben.X = pixRange.xh
            pixRechtsOben.Y = pixRange.yh

            Dim temp As String
            'Dim utmPrintPDFrange As New clsRange
            Dim utmLinksUnten As New Point
            temp = clsToolsAllg.koordinateKlickBerechnen(pixLinksUnten)
            a = temp.Split(","c)
            utmLinksUnten.X = CDbl((a(0)))
            utmLinksUnten.Y = CDbl((a(1)))

            Dim utmRechtsOben As New Point
            temp = clsToolsAllg.koordinateKlickBerechnen(pixRechtsOben)
            a = temp.Split(","c)
            utmRechtsOben.X = CDbl((a(0)))
            utmRechtsOben.Y = CDbl((a(1)))


            PDF_PrintRange.xl = utmLinksUnten.X
            PDF_PrintRange.xh = utmRechtsOben.X
            PDF_PrintRange.yl = utmLinksUnten.Y '+ 27
            PDF_PrintRange.yh = utmRechtsOben.Y '+ 27

            Dim mas As Double
            mas = calcPDFMassstab()
            Return mas
            l("PDF_postition_desRahmensBestimmen---------------------- ende")
        Catch ex As Exception
            l("Fehler in PDF_postition_desRahmensBestimmen: " & ex.ToString())
            Return 0
        End Try
    End Function

    Function calcPDFMassstab() As Double
        Dim mas As Double
        Dim aktCV As New clsCanvas
        If rbFormatA4.IsChecked Then
            aktCV = dina4InMM
        Else
            aktCV = dina3InMM
        End If
        If quer.IsChecked Then
            mas = PDF_PrintRange.xdif
            mas = mas * 100
            mas = mas / ((aktCV.w) / 10) '28 '28 cm ist die breite des rahmens auf papier
        Else
            mas = PDF_PrintRange.xdif
            mas = mas * 100
            mas = mas / ((aktCV.h) / 10) '29.7 '28 '28 cm ist die breite des rahmens auf papier
        End If

        Return mas
    End Function

    Private Sub cvPDFrechteck_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) _
        Handles cvPDFrechteck.MouseMove, cvPDFrechteck.MouseMove
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim dragOffset As Vector
        If isDraggingFlag Then
            curContentMousePoint = e.GetPosition(cvPDFrechteck)
            dragOffset = curContentMousePoint - origContentMousePoint
            If dragOffset.X < 0 Then
                Canvas.SetLeft(auswahlRechteck, 0)
            Else
                Canvas.SetLeft(auswahlRechteck, dragOffset.X)
            End If
            If (dragOffset.X + (auswahlRechteck.Width)) > cv1.Width Then
                Canvas.SetLeft(auswahlRechteck, cv1.Width - auswahlRechteck.Width)
            Else
                '  Canvas.SetLeft(myRect, dragOffset.X)
            End If


            If dragOffset.Y < 0 Then
                Canvas.SetTop(auswahlRechteck, 0)
            Else
                Canvas.SetTop(auswahlRechteck, dragOffset.Y)
            End If
            If (Canvas.GetTop(auswahlRechteck)) + auswahlRechteck.Height > cv1.Height - dockTop.Height Then
                Canvas.SetTop(auswahlRechteck, cv1.Height - auswahlRechteck.Height - dockTop.Height)
            Else
                '  Canvas.SetTop(myRect, dragOffset.Y)
            End If


            'If (dragOffset.Y + (myRect.Height)) > mapCanvas.Height Then
            '    Canvas.SetTop(myRect, mapCanvas.Height - myRect.Height)
            'Else
            '    '  Canvas.SetTop(myRect, dragOffset.Y)
            'End If
            'Canvas.SetTop(myRect, dragOffset.Y)
            'Canvas.SetLeft(myRect, dragOffset.X)
        End If
        tbFavoname.Text = Canvas.GetTop(auswahlRechteck) & " , " & cv1.Height & ", " & (Canvas.GetTop(auswahlRechteck) + auswahlRechteck.Height)
        e.Handled = True
    End Sub

    Private Sub myCanvas_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) _
        Handles cv1.MouseLeftButtonDown, cv1.MouseLeftButtonDown
        Select Case CanvasClickModus.ToLower
            Case "ausschnitt"
                If chkBoxAusschnitt.IsChecked Then RubberbandStart(e)
            Case "pan"
                isDraggingFlag = True
                origContentMousePoint = e.GetPosition(cv1)
        End Select
        e.Handled = True
    End Sub
    Private Sub canvas1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) _
        Handles cv1.MouseMove, cv1.MouseMove
        Select Case CanvasClickModus.ToLower
            Case "ausschnitt"
                If chkBoxAusschnitt.IsChecked Then RubberbandMove(e)
            Case "pan"
                If isDraggingFlag Then
                    Dim dragOffset As Vector
                    curContentMousePoint = e.GetPosition(cv1)
                    dragOffset = curContentMousePoint - origContentMousePoint
                    Canvas.SetTop(slots(0).image, dragOffset.Y)
                    Canvas.SetLeft(slots(0).image, dragOffset.X)

                    Canvas.SetTop(slots(1).image, dragOffset.Y)
                    Canvas.SetLeft(slots(1).image, dragOffset.X)

                    Canvas.SetTop(slots(2).image, dragOffset.Y)
                    Canvas.SetLeft(slots(2).image, dragOffset.X)
                    tbMinimapCoordinate2.Text = CType(curContentMousePoint.X, String)
                End If
            Case "strecke"
                Dim tempPT As New Point?
                Dim winpt As New Point
                Dim delim As String = ";"
                tempPT = e.GetPosition(cv1)
                btnMessen.IsOpen = True
                winpt.X = tempPT.Value.X
                winpt.Y = tempPT.Value.Y
                'aktPolyline.myLine.Points.Add(winpt) sonst mal er alles voll
                Dim utmpt As New Point
                utmpt.X = CInt((clsMiniMapTools.MyPointVonCanvasNachGKumrechnen(winpt).X) * 100) / 100
                utmpt.Y = CInt((clsMiniMapTools.MyPointVonCanvasNachGKumrechnen(winpt).Y) * 100) / 100
                Dim a As Double = aktPolyline.Distanz + PolygonTools.calcDistanz(utmpt, aktPolyline.alterPunkt)
                tbzwischenwert.Visibility = Visibility.Visible
                tbzwischenwert.Text = (a - aktPolyline.Distanz).ToString
                btnMessen.IsOpen = True
                '  tbMinimapCoordinate2.Text = a.ToString("N2")
                '   tbMinimapCoordinate2.Text = CType(curContentMousePoint.X, String)
        End Select
        e.Handled = True
    End Sub


    Private Sub canvas1_MouseLeftButtonUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles cv1.MouseLeftButtonUp, cv1.MouseLeftButtonUp
        e.Handled = True
        Select Case CanvasClickModus.ToLower
            Case "ausschnitt"
                If chkBoxAusschnitt.IsChecked Then RubberbandFinish()
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                CanvasClickModus = ""
                imageMapCanvas.Visibility = Visibility.Visible
            Case "wmsdatenabfrage"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cv1)
                Dim bbox As String = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt).Replace(" ", "")
                bbox = clsWMS.calcVollstBbox(bbox)
                Dim url As String
                url = clsWMS.calcWMSGetfeatureInfoURL(bbox, layerActive.aid, CInt(cv1.Height), CInt(cv1.Width),
                                                      CInt(KoordinateKLickpt.Value.X), CInt(KoordinateKLickpt.Value.Y), "text/html",
                                                      "", "")
                Process.Start(url)
                imageMapCanvas.Visibility = Visibility.Visible
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                CanvasClickModus = ""
            Case "pointactivemodus"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cv1)
                Dim bbox As String = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt).Replace(" ", "")
                Dim a() As String = bbox.Split(","c)
                Dim utmpt As New Point
                utmpt.X = (CDbl(a(0).Replace(".", ",")))
                utmpt.Y = (CDbl(a(1).Replace(".", ",")))
                Debug.Print("" & layerActive.dokutext)
                If layerActive.tabname.IsNothingOrEmpty Then
                    os_tabelledef = New clsTabellenDef
                    os_tabelledef.aid = CStr(layerActive.aid)
                    os_tabelledef.gid = "0"
                    os_tabelledef.datenbank = "postgis20"
                    os_tabelledef.tab_nr = CType(1, String)
                    sachdatenTools.getSChema(os_tabelledef)
                    layerActive.tabname = os_tabelledef.tabelle
                End If

                Dim fangRadiusInMeter = clsSachdatentools.calcFangradiusM(CInt(cv1.Width),
                                            myglobalz.fangradius_in_pixel,
                                            kartengen.aktMap.aktrange.xdif, "")
                clsSachdatentools.getActiveLayer4point(utmpt, layerActive.aid,
                                                                     CInt(cv1.Width), CInt(cv1.Height),
                                                                     KoordinateKLickpt, fangRadiusInMeter)
                imageMapCanvas.Visibility = Visibility.Visible
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                CanvasClickModus = ""
            Case "dossiermodus"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cv1)
                Dim bbox As String = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt).Replace(" ", "")
                Dim a() As String = bbox.Split(","c)
                Dim utmpt As New Point
                utmpt.X = (CDbl(a(0).Replace(".", ",")))
                utmpt.Y = (CDbl(a(1).Replace(".", ",")))




                clsSachdatentools.getdossier(utmpt, layerActive.aid,
                                            CInt(cv1.Width), CInt(cv1.Height),
                                            KoordinateKLickpt, "", "punkt")

                'FS feststellen
                aktFST.clear()
                aktFST.punkt.X = utmpt.X
                aktFST.punkt.Y = utmpt.Y
                aktFST.normflst.FS = pgisTools.getFS4UTM(utmpt)
                aktFST.normflst.splitFS(aktFST.normflst.FS)

                clsFSTtools.holeKoordinaten4Flurstueck(aktFST.normflst.nenner.ToString, WinDetailSucheFST.AktuelleBasisTabelle, aktFST)
                getSerialFromPostgis(aktFST.normflst.FS, False, WinDetailSucheFST.AktuelleBasisTabelle) ' setzt  aktFST.serial  

                setBoundingRefresh(kartengen.aktMap.aktrange)
                refreshMap(True, True)
                suchObjektModus = "fst"

                imageMapCanvas.Visibility = Visibility.Visible
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                CanvasClickModus = ""
            Case "koordinate"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cv1)
                'CanvasClickModus = ""
                'Dim temp = koordinateKlickBerechnen(KoordinateKLickpt) & " [m]"
                tbzwischenwert.Text = "Ihre UTM-Koordinate:"
                tbMinimapCoordinate2.Text = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint
                btnNeueMessung.IsEnabled = True
                btnMessen.IsOpen = True
                imageMapCanvas.Visibility = Visibility.Visible
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                punktNachParadigmaUebernehemn()
                btnMessen.IsOpen = True
            Case "windrose"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cv1)
                CanvasClickModus = ""
                'Dim temp = koordinateKlickBerechnen(KoordinateKLickpt) & " [m]"
                tbMinimapCoordinate2.Text = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint
                imageMapCanvas.Visibility = Visibility.Visible
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                windroseBerechnen(aktGlobPoint)
            Case "kreisimabstand"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cv1)
                CanvasClickModus = ""
                Dim kreisimabstand_Radius_pixel As Double = Nothing
                kreisimabstand_Radius_pixel = (KreislinienRadius * cv1.Width) / kartengen.aktMap.aktrange.xdif

                '#########################
                Dim myEllipse As New Ellipse
                Dim mySolidColorBrush As New SolidColorBrush()
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0)
                'myEllipse.Fill = mySolidColorBrush
                myEllipse.StrokeThickness = 2
                myEllipse.Stroke = Brushes.Black


                ' Set the width and height of the Ellipse.
                myEllipse.Width = kreisimabstand_Radius_pixel
                myEllipse.Height = kreisimabstand_Radius_pixel

                ' Add the Ellipse to the StackPanel.
                cv1.Children.Add(myEllipse)
                cv1.SetLeft(myEllipse, KoordinateKLickpt.Value.X - (kreisimabstand_Radius_pixel / 2))
                cv1.SetTop(myEllipse, KoordinateKLickpt.Value.Y - (kreisimabstand_Radius_pixel / 2))





                'Dim temp = koordinateKlickBerechnen(KoordinateKLickpt) & " [m]"
                'tbMinimapCoordinate2.Text = koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint
                imageMapCanvas.Visibility = Visibility.Visible
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True


            Case "flaeche"
                Dim tempPT As Point? = Nothing
                Dim winpt As New Point
                tempPT = e.GetPosition(cv1)
                winpt.X = tempPT.Value.X
                winpt.Y = tempPT.Value.Y
                aktPolygon.myPoly.Points.Add(winpt)
                myPolyVertexCount% += 1
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                btnMessen.IsOpen = True

                'cmbMessen.SelectedIndex = 0
            Case "strecke"
                Dim tempPT As Point? = Nothing
                Dim winpt As New Point
                Dim delim As String = ";"
                tempPT = e.GetPosition(cv1)
                winpt.X = tempPT.Value.X
                winpt.Y = tempPT.Value.Y

                aktPolyline.myLine.Points.Add(winpt)
                Dim utmpt As New Point
                utmpt.X = CInt((clsMiniMapTools.MyPointVonCanvasNachGKumrechnen(winpt).X) * 100) / 100
                utmpt.Y = CInt((clsMiniMapTools.MyPointVonCanvasNachGKumrechnen(winpt).Y) * 100) / 100

                aktPolyline.Distanz = aktPolyline.Distanz +
                              PolygonTools.calcDistanz(utmpt, aktPolyline.alterPunkt)
                aktPolyline.GKstring = aktPolyline.GKstring &
                             CDbl(utmpt.X) & delim & CDbl(utmpt.Y) & delim
                aktPolyline.alterPunkt.X = utmpt.X
                aktPolyline.alterPunkt.Y = utmpt.Y

                tbMinimapCoordinate2.Text = aktPolyline.Distanz.ToString("########.##") & " [m]"
                myPolyVertexCount += 1
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True

                btnMessen.IsOpen = True

            Case "pan"
                CanvasClickModus = "pan" ' bleibt!!
                isDraggingFlag = False

                Dim dragOffset As Vector = curContentMousePoint - origContentMousePoint
                Dim neuerBildschirmMittelPunktInPoints As Point

                neuerBildschirmMittelPunktInPoints = New Point() With {.X = ((cv1.Width) / 2) - dragOffset.X,
                                                                       .Y = (cv1.Height / 2) - dragOffset.Y}
                'neuerBildschirmMittelPunktInPoints.X = neuerBildschirmMittelPunktInPoints.X + (2 * dockMenu.Width)

                dragOffset = Nothing
                Dim temp = clsToolsAllg.koordinateKlickBerechnen(neuerBildschirmMittelPunktInPoints)
                'UTMCoordinate.Text = temp
                Dim a As String()
                a = temp.Split(","c)
                Dim neuerMittelPunktInUTM As New myPoint
                neuerMittelPunktInUTM.X = CDbl((a(0)))
                neuerMittelPunktInUTM.Y = CDbl((a(1)))

                Dim breite As Double = kartengen.aktMap.aktrange.xdif()
                Dim hohe As Double = kartengen.aktMap.aktrange.ydif()

                kartengen.aktMap.aktrange.xl = neuerMittelPunktInUTM.X - (breite / 2)
                kartengen.aktMap.aktrange.xh = neuerMittelPunktInUTM.X + (breite / 2)

                kartengen.aktMap.aktrange.yl = neuerMittelPunktInUTM.Y - (hohe / 2)
                kartengen.aktMap.aktrange.yh = neuerMittelPunktInUTM.Y + (hohe / 2)
                refreshMap(True, True)
                'Dim erfolg As Boolean = clsMiniMapTools.setMapCookie(CLstart.myc.kartengen.aktMap, myglobalz.sitzung.aktVorgangsID)
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = False
                neuerMittelPunktInUTM = Nothing
        End Select
    End Sub




    Private Sub windroseBerechnen(pt As myPoint)
        If pt.X < 1 Or pt.Y < 1 Then
            MsgBox("Sie haben keine gültige Koordinate." &
                   " Somit ist es nicht möglich eine Windrose zu bekommen!")
            Exit Sub
        Else
            Dim windrosenHyperlink As String = clsWindrose.GetWindrosenHyperlink(pt.X, pt.Y)
            'Process.Start(windrosenHyperlink$)
            'MessageBox.Show("Windrose", "Empfehlung", MessageBoxButton.OK, MessageBoxImage.Information)
            panningAusschalten()
            tivogel.Visibility = Visibility.Visible
            tigis.Visibility = Visibility.Collapsed
            panningAusschalten()
            webBrowserControlVogel.Navigate(New Uri(windrosenHyperlink))
        End If
    End Sub



    Private Sub btnGetLinie_ClickExtracted()
        nachricht("USERAKTION: strecke messen ")
        panningAusschalten()
        'MsgBox("Wählen sie die Strecke in der Karte indem Sie die Punkte anklicken (Linke Maustaste drücken)",, "Streckenmessung")
        CanvasClickModus = "strecke"
        btnGetFlaecheEnde.IsEnabled = True
        btnGetFlaecheEnde.Visibility = Visibility.Collapsed
        btnNeueMessung.Visibility = Visibility.Collapsed


        btnGetFlaecheEnde.Background = Brushes.Red
        btnGetFlaecheEnde.Content = "Messen Ende"
        aktPolyline.clear()
        tbmessenhinweis.Text = "Wählen sie die Strecke in der Karte indem Sie die Punkte anklicken (Linke Maustaste drücken)"
        tbMinimapCoordinate2.Text = ""
        tbzwischenwert.Text = ""
        btnNeueMessung.IsEnabled = False
        imageMapCanvas.Visibility = Visibility.Collapsed
        tbzwischenwert.Visibility = Visibility.Collapsed
        'clsMiniMapTools.VisibilityDerKinderschalten(myCanvas2, Windows.Visibility.Collapsed)
        zeichneOverlaysGlob = True : zeichneImageMapGlob = False
        'gisDarstellenAlleEbenen()
        DrawPolylinie(cv1)
        btnMessen.IsOpen = True
    End Sub
    Private Sub DrawPolylinie(ByVal mycanvas As Canvas)
        aktPolyline.myLine = New Polyline
        aktPolyline.myLine.Name = "myLine"
        Dim myBrush As SolidColorBrush = New SolidColorBrush(Color.FromArgb(20, 0, 100, 250)) 'transparenz ist der erste wert
        aktPolyline.myLine.Stroke = Brushes.DarkBlue
        aktPolyline.myLine.StrokeThickness = 4
        aktPolyline.myLine.Opacity = 90
        aktPolyline.myLine.Fill = myBrush
        System.Windows.Controls.Panel.SetZIndex(aktPolyline.myLine, 100)
        Canvas.SetZIndex(aktPolyline.myLine, 100)
        mycanvas.Children.Add(aktPolyline.myLine)
        System.Windows.Controls.Panel.SetZIndex(aktPolyline.myLine, 100)
        Canvas.SetZIndex(aktPolyline.myLine, 100)
        myPolyVertexCount = 0
    End Sub


    Private Sub btnGetFlaecheEnde_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            l("btnGetFlaecheEnde_Click---------------------- anfang")
            panningAusschalten()
            imageMapCanvas.Visibility = Visibility.Visible
            CanvasClickModus = CanvasClickModus.ToLower
            btnGetFlaecheEnde.Visibility = Visibility.Collapsed
            If STARTUP_mgismodus.ToLower = "paradigma" Then
                If aktvorgangsid <> String.Empty Then
                    btnNachParadigma.Visibility = Visibility.Visible
                End If
            End If

            tbzwischenwert.Visibility = Visibility.Collapsed
            If CanvasClickModus = "flaeche" Then
                btnGetFlaecheEnde.IsEnabled = False
                btnGetFlaecheEnde.Background = Brushes.Black
                'lastGeomAsWKT = serialGKStringnachWKT(aktPolygon.ShapeSerial)
                If PolygonWurdeErstellt() Then
                    l("PolygonWurdeErstellt")
                Else
                    l("PolygonWurde NICHT Erstellt")
                    e.Handled = True
                    Exit Sub
                End If
                btnMessen.IsOpen = True
                'btnGetFlaecheEnde.Visibility = Visibility.Collapsed
            End If

            If CanvasClickModus = "strecke" Then
                btnGetFlaecheEnde.IsEnabled = False
                'btnGetFlaecheEnde.Visibility = Visibility.Collapsed
                If Not btnGetlinieEnde_ClickExtracted() Then
                    e.Handled = True
                    Exit Sub
                End If
                aktPolygon = convPolygon2Polyline(aktPolyline)
                'tbMinimapCoordinate2.Visibility = Visibility.Collapsed
                btnMessen.IsOpen = True
            End If
            btnNeueMessung.IsEnabled = True
            l("aktPolygon.GKstring " & aktPolygon.GKstring)
            If Not aktPolygon.GKstring.IsNothingOrEmpty Then
                'polyGeometrieNachParadigmaUebernehmen()
                lastGeomAsWKT = serialGKStringnachWKT(aktPolygon.GKstring, CanvasClickModus)
                l("lastGeomAsWKT: " & lastGeomAsWKT)
            Else
                l("fehler Geometrie nicht verwendbar.")
            End If
            l("CanvasClickModus " & CanvasClickModus)
            'CanvasClickModus = ""
            cmbMessen.SelectedItem = Nothing
            l("btnGetFlaecheEnde_Click---------------------- ende")
        Catch ex As Exception
            l("Fehler in btnGetFlaecheEnde_Click: " & ex.ToString())
        End Try
        e.Handled = True
    End Sub
    Sub punktNachParadigmaUebernehemn()
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            If aktvorgangsid <> String.Empty Then
                Dim mesred As MessageBoxResult = userWill("Punkt")
                If mesred = MessageBoxResult.Yes Then
                    If modParadigma.punktNachParadigma(aktGlobPoint) Then
                        clsToolsAllg.userlayerNeuErzeugen(GisUser.username, myglobalz.aktvorgangsid)
                        MsgBox("Das Objekt wurde in die Paradigma-DB als Raumbezug übernommen. 
                               Drücken Sie oben die RefreshTaste um die Änderung anzuzeigen!",
                               MsgBoxStyle.OkOnly, "Datenübernahme OK")
                    Else
                        MsgBox("Datenübernahme war nicht erfolgreich. Bitte beim Admin melden!")
                    End If
                End If
            End If
        End If
    End Sub
    Private Shared Sub polyGeometrieNachParadigmaUebernehmen()
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            If aktvorgangsid <> String.Empty Then
                Dim mesred As MessageBoxResult = userWill(clsString.Capitalize(CanvasClickModus))
                If mesred = MessageBoxResult.Yes Then
                    If modParadigma.GeometrieNachParadigma(aktPolygon, aktPolyline) Then
                        clsToolsAllg.userlayerNeuErzeugen(GisUser.username, myglobalz.aktvorgangsid)
                        MsgBox("Das Objekt wurde in die Paradigma-DB als Raumbezug übernommen.  
                               Drücken Sie oben die RefreshTaste um die Änderung anzuzeigen!",
                               MsgBoxStyle.OkOnly, "Datenübernahme OK")
                    Else
                        MsgBox("Datenübernahme war nicht erfolgreich. Bitte beim Admin melden!")
                    End If
                End If
            End If
        End If
    End Sub

    Private Shared Function userWill(text As String) As MessageBoxResult
        Return MessageBox.Show("Geometrie-Objekt (" & text & ") nach Paradigma-Vorgang " & aktvorgangsid & " übernehmen?",
                                 "Paradigma Datenübernahme nach Vorgang " & aktvorgangsid,
                               MessageBoxButton.YesNo,
                               MessageBoxImage.Question,
                               MessageBoxResult.No)
    End Function

    Private Function convPolygon2Polyline(aktPolyline As clsParapolyline) As clsParapolygon
        Dim aktpo As New clsParapolygon
        Try
            l("convPolygon2Polyline-----------------------")
            aktpo.Area = 0
            aktpo.LaengeM = aktPolyline.Distanz
            aktpo.GKstring = aktPolyline.GKstring
            aktpo.ShapeSerial = aktPolyline.GKstring
            aktpo.Area = CInt(aktPolyline.Distanz)
            aktpo.LaengeM = CInt(aktPolyline.Distanz)
            aktpo.Typ = RaumbezugsTyp.Polyline
            l("convPolygon2Polyline----------------------- ende")
            Return aktpo
        Catch ex As Exception
            l("fehler in convPolygon2Polyline ", ex)
            Return Nothing
        End Try
    End Function


    Private Function PolygonWurdeErstellt() As Boolean
        Try
            l("PolygonWurdeErstellt---------------------- anfang")
            l("PolygonWurdeErstelltmyPolyVertexCount " & myPolyVertexCount%)
            If myPolyVertexCount% > 2 Then
                zeichnereihenfolgeInvertieren()
                If clsMiniMapTools.PolygonAufbereiten(aktPolygon) Then
                    clsMiniMapTools.GK_FlaecheErmitteln(aktPolygon)
                    tbMinimapCoordinate2.Text = CLng(aktPolygon.Area).ToString & " [qm}"
                    aktPolygon.Typ = RaumbezugsTyp.Polygon
                End If
            Else
                MsgBox("Zu wenig Punkte für eine Flächenberechnung. Mind. 3 Punkte sind erforderlich!")
                Return False
            End If
            l(" vor myPolyFinish")
            myPolyFinish("flaeche")
            Return True
            l("PolygonWurdeErstellt---------------------- ende")
        Catch ex As Exception
            l("Fehler in PolygonWurdeErstellt: ", ex)
            Return False
        End Try
    End Function

    Private Shared Sub zeichnereihenfolgeInvertieren()
        Try
            l("zeichnereihenfolgeInvertieren---------------------- anfang")
            Dim parray() As Point = getPointArray(aktPolygon.myPoly.Points)
            l("1aktPolygon.myPoly.Points " & aktPolygon.myPoly.Points.Count)
            If SignedPolygonArea(parray) > 0 Then
                l("2aktPolygon.myPoly.Points " & aktPolygon.myPoly.Points.Count)
                l("gegen uhrzeiger")
                aktPolygon.myPoly.Points = modWKT.zeichenrichtungInvertieren(aktPolygon.myPoly.Points)
                l("4aktPolygon.myPoly.Points " & aktPolygon.myPoly.Points.Count)
            Else
                l("3aktPolygon.myPoly.Points " & aktPolygon.myPoly.Points.Count)
                l("mit uhrzeiger")
            End If
            l("zeichnereihenfolgeInvertieren---------------------- ende")
        Catch ex As Exception
            l("Fehler in zeichnereihenfolgeInvertieren: ", ex)
        End Try
    End Sub

    Private Function btnGetlinieEnde_ClickExtracted() As Boolean
        Try
            l("btnGetlinieEnde_ClickExtracted---------------------- anfang")
            If myPolyVertexCount > 1 Then
                ' tbMinimapCoordinate2.Text = CLng(aktPolyline.Distanz).ToString unnötig 
            Else
                MsgBox("Zu wenig Punkte für eine Flächenberechnung. Mind. 3 Punkte sind erforderlich!")
                Return False
            End If
            myPolyFinish("strecke")
            tbzwischenwert.Visibility = Visibility.Visible
            aktPolyline.Distanz = 0
            btnMessen.IsOpen = True
            Return True
            l("btnGetlinieEnde_ClickExtracted---------------------- ende")
        Catch ex As Exception
            l("Fehler in btnGetlinieEnde_ClickExtracted: ", ex)
            Return False
        End Try
    End Function
    Private Sub myPolyFinish(typ As String)
        Try
            l("myPolyFinish---------------------- anfang")
            chkBoxAusschnitt.IsChecked = False
            If typ = "flaeche" Then
                aktPolygon.myPoly.Width = 0
                aktPolygon.myPoly.Height = 0
            End If
            If typ = "strecke" Then
                aktPolyline.myLine.Width = 0
                aktPolyline.myLine.Height = 0
            End If
            Mouse.Capture(Nothing)
            ' cmbMessen.SelectedValue = ""

            l("myPolyFinish---------------------- ende")
        Catch ex As Exception
            l("Fehler inmyPolyFinish : " & ex.ToString())
        End Try
    End Sub
    Private Sub inputGetFlaeche()
        nachricht("USERAKTION: fläche messen ")
        'MsgBox(glob2.getMsgboxText("polygonErfassen", New List(Of String)(New String() {})))
        'btnGetFlaecheEnde.Background = Brushes.Red
        CanvasClickModus = "flaeche"
        btnGetFlaecheEnde.IsEnabled = True
        btnMessen.IsOpen = True
        tbmessenhinweis.Text = "Klicken sie die Eckpunkte der Fläche ab. "
        tbzwischenwert.Text = ""
        tbMinimapCoordinate2.Text = ""
        'clsMiniMapTools.VisibilityDerKinderschalten(myCanvas, Windows.Visibility.Collapsed)
        zeichneOverlaysGlob = True : zeichneImageMapGlob = False
        'gisDarstellenAlleEbenen()

        DrawPolygon(cv1)
    End Sub
    Private Sub DrawPolygon(ByVal mycanvas As Canvas)
        aktPolygon.myPoly = New Polygon
        aktPolygon.myPoly.Name = "myPoly"
        Dim myBrush As SolidColorBrush = New SolidColorBrush(Color.FromArgb(20, 0, 100, 250)) 'transparenz ist der erste wert
        aktPolygon.myPoly.Stroke = Brushes.DarkBlue
        aktPolygon.myPoly.StrokeThickness = 2
        aktPolygon.myPoly.Opacity = 90
        aktPolygon.myPoly.Fill = myBrush
        System.Windows.Controls.Panel.SetZIndex(aktPolygon.myPoly, 100)
        Canvas.SetZIndex(aktPolygon.myPoly, 100)
        mycanvas.Children.Add(aktPolygon.myPoly)
        System.Windows.Controls.Panel.SetZIndex(aktPolygon.myPoly, 100)
        Canvas.SetZIndex(aktPolygon.myPoly, 100)
        myPolyVertexCount% = 0
    End Sub

    Private Sub cmbMessen_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        panningAusschalten()
        If cmbMessen.SelectedItem Is Nothing Then Exit Sub
        Dim item As ComboBoxItem = CType(cmbMessen.SelectedItem, ComboBoxItem)
        If item.Tag.ToString = "Messen" Then Exit Sub
        e.Handled = True
        tbMinimapCoordinate2.Visibility = Visibility.Visible
        btnNachParadigma.Visibility = Visibility.Collapsed
        messenAuswahl(item.Tag.ToString)
        cmbMessen.SelectedItem = Nothing
        e.Handled = True
    End Sub

    Private Sub messenAuswahl(item As String)
        SuchobjektAusschalten()
        Select Case item
            Case "Koordinate"
                messeKoordinate()
            Case "Fläche"
                messeFlaeche()
            Case "Strecke"
                messestrecke()
            Case "kreis"
                kreislinieimAbstand()
            Case "windrose"
                zeigeWindrose()
        End Select
    End Sub

    Private Sub zeigeWindrose()
        nachricht("USERAKTION: koordinate messen ")
        panningAusschalten()
        btnKoordUmrechner.Visibility = Visibility.Collapsed
        imageMapCanvas.Visibility = Visibility.Collapsed
        MsgBox("Wählen sie den Punkt in der Karte (Linke Maustaste drücken)",, "Punkt wählen")
        CanvasClickModus = "windrose"
        'clsMiniMapTools.VisibilityDerKinderschalten(myCanvas, Windows.Visibility.Collapsed)
        zeichneOverlaysGlob = True : zeichneImageMapGlob = False
        'gisDarstellenAlleEbenen()
        '  cmbMessen.SelectedIndex = Nothing
    End Sub

    Private Sub kreislinieimAbstand()
        nachricht("USERAKTION: kreislinieimAbstand messen ")
        panningAusschalten()
        btnKoordUmrechner.Visibility = Visibility.Collapsed
        Dim message, title As String
        Dim myValue As String
        message = "Eine Zahl in Meter ohne Zusatz wie [m] oder meter"
        title = "Radius eingeben"
        If CDbl(KreislinienRadius) < 1 Then
            KreislinienRadius = kartengen.aktMap.aktrange.xdif / 5   ' Set default value.
        End If
        KreislinienRadius = KreislinienRadius / 2
        myValue = InputBox(message, title, CType(KreislinienRadius, String))
        ' If user has clicked Cancel, set myValue to defaultValue
        If myValue Is "" Then
            ' myValue = KreislinienRadius
            Exit Sub
        End If

        Dim temp As String = myValue.ToString
        temp = temp.Replace(".", ",")
        If IsNumeric(temp) Then
            KreislinienRadius = CInt(temp)
        Else
            MsgBox("Bitte nur die Zahl eingeben!")
            Exit Sub
        End If
        KreislinienRadius = KreislinienRadius * 2
        imageMapCanvas.Visibility = Visibility.Collapsed
        ' MsgBox("Wählen sie den Punkt in der Karte (Linke Maustaste drücken)",, "Punkt wählen")
        CanvasClickModus = "kreisimAbstand"
        zeichneOverlaysGlob = True : zeichneImageMapGlob = False
        'cmbMessen.SelectedIndex = 0
    End Sub

    Private Sub messestrecke()
        btnKoordUmrechner.Visibility = Visibility.Collapsed
        btnGetLinie_ClickExtracted()
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            If aktvorgangsid <> String.Empty Then

                'btnNachParadigma.Visibility = Visibility.Visible
                aktPolygon.Typ = RaumbezugsTyp.Polyline
                btnGetFlaecheEnde.Visibility = Visibility.Visible
            End If
        End If

        btnNeueMessung.IsEnabled = False
    End Sub

    Private Sub messeFlaeche()
        nachricht("USERAKTION: FlächeMessen angeklickt")
        panningAusschalten()
        imageMapCanvas.Visibility = Visibility.Collapsed
        tbzwischenwert.Visibility = Visibility.Collapsed
        btnKoordUmrechner.Visibility = Visibility.Collapsed
        btnNeueMessung.IsEnabled = False
        btnNeueMessung.Visibility = Visibility.Visible
        btnGetFlaecheEnde.Visibility = Visibility.Visible
        inputGetFlaeche()


    End Sub

    Private Sub messeKoordinate()
        nachricht("USERAKTION: koordinate messen ")
        panningAusschalten()
        imageMapCanvas.Visibility = Visibility.Collapsed
        'MsgBox("Wählen sie den Punkt in der Karte (Linke Maustaste drücken)",, "Punkt wählen")
        CanvasClickModus = "koordinate"
        tbmessenhinweis.Text = "Wählen sie den Punkt in der Karte (Linke Maustaste drücken)"
        btnKoordUmrechner.Visibility = Visibility.Visible
        tbMinimapCoordinate2.Text = ""
        tbzwischenwert.Text = ""
        btnNeueMessung.IsEnabled = False
        btnMessen.IsOpen = True
        'clsMiniMapTools.VisibilityDerKinderschalten(myCanvas, Windows.Visibility.Collapsed)
        zeichneOverlaysGlob = True : zeichneImageMapGlob = False
        'gisDarstellenAlleEbenen()
        cmbMessen.SelectedIndex = 0
    End Sub



    Private Sub utmKoordinate()
        nachricht("USERAKTION: sucheUtm32 suchen ")
        Dim utm As New sucheUtm32
        utm.ShowDialog()
        If utm.returnCode Then
            aktPolygon.ShapeSerial = holePUFFERPolygonFuerPoint(aktGlobPoint, 30) 'pufferinMeter)
            aktPolygon.originalQuellString = aktPolygon.ShapeSerial
            aktFST.normflst.serials.Clear()
            aktFST.normflst.serials.Add(aktPolygon.ShapeSerial)
            suchObjektModus = "fst"
            '-----------------------------------
            'btnSuchobjAusSchalten.Visibility = Visibility.Visible
            setBoundingRefresh(kartengen.aktMap.aktrange)
            refreshMap(True, True)
        End If
        utm = Nothing
        'cmbSuchen.SelectedIndex = 0
    End Sub

    Private Sub adresssuche()
        nachricht("USERAKTION: adr suchen ")
        Dim adrs As New winDetailAdressSuche
        adrs.ShowDialog()
        Dim ergebnis As Boolean = CBool(adrs.retunrvalue)
        If ergebnis Then
            aktPolygon.ShapeSerial = holePUFFERPolygonFuerPoint(aktGlobPoint, 30) 'pufferinMeter)
            aktPolygon.originalQuellString = aktPolygon.ShapeSerial
            aktFST.normflst.serials.Clear()
            aktFST.normflst.serials.Add(aktPolygon.ShapeSerial)
            suchObjektModus = "fst"
            setBoundingRefresh(kartengen.aktMap.aktrange)
            refreshMap(True, True)
        Else
            'btnSuchobjAusSchalten.Visibility = Visibility.Collapsed
        End If
        adrs = Nothing
        'cmbSuchen.SelectedIndex = 0
    End Sub
    Private Sub flurstueckssuche()
        nachricht("USERAKTION: flst suchen ")
        Dim flst As New WinDetailSucheFST("ort")
        flst.ShowDialog()
        If CBool(flst.returnValue) Then
            If flst.historyLast Then
                getSerialFromPostgis(aktFST.normflst.FS, True, myglobalz.histFstView) ' setzt  aktFST.serial 
            Else
                getSerialFromPostgis(aktFST.normflst.FS, False, WinDetailSucheFST.AktuelleBasisTabelle) ' setzt  aktFST.serial 
            End If
            If aktFST.normflst.serials.Count > 1 Then
                MessageBox.Show("Dieses Flurstück besteht aus " & aktFST.normflst.serials.Count & " (vermutl. durch einen Weg) getrennten Teilen!!!", "Wichtiger Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning)
            End If
            'btnSuchobjAusSchalten.Visibility = Visibility.Visible
            'kartengen.aktMap.aktrange wurde im formular vorher schon gesetzt
            setBoundingRefresh(kartengen.aktMap.aktrange)
            refreshMap(True, True)
            suchObjektModus = "fst"
        Else
        End If
        'cmbSuchen.SelectedIndex = 0
    End Sub

    Private Sub btngoogle3d_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        google3dintro()
    End Sub

    Private Sub google3dintro()
        Dim gis As New clsGISfunctions
        Dim result As String
        Dim nbox As New clsRange
        panningAusschalten()

        Try
            nachricht("USERAKTION: googlekarte  vgoogle3dintro")

            panningAusschalten()
            ' calcBbox(rechts, hoch, nbox, 900)
            Dim radius = 300
            nbox.xl = CInt(aktGlobPoint.strX) - radius
            nbox.yl = CInt(aktGlobPoint.strY) - (radius * 2)
            nbox.xh = CInt(aktGlobPoint.strX) + radius
            nbox.yh = CInt(aktGlobPoint.strY)
            result = gis.GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(nbox, False, longitude, latitude)
            If result = "fehler" Or result = "" Then
            Else
                Process.Start("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", result)
            End If
            gis = Nothing
            ' Protokollausgabe_aller_Zugriff("ja")

        Catch ex As Exception
            nachricht("fehler in starteWebbrowserControl: " & ex.ToString)
        End Try
    End Sub

    Private Sub btnVogel_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        tivogel.Visibility = Visibility.Visible
        tigis.Visibility = Visibility.Collapsed
        panningAusschalten()
        webBrowserControlVogel.Navigate(New Uri(clsStartup.calcURI4vogel))
        e.Handled = True
    End Sub

    'Private Sub btnaktualisiernvogel_Click(sender As Object, e As RoutedEventArgs)
    '    starteWebbrowserControl()
    '    e.Handled = True
    'End Sub

    Private Sub btnzurueckZumGis_Click(sender As Object, e As RoutedEventArgs)
        tivogel.Visibility = Visibility.Collapsed
        tigis.Visibility = Visibility.Visible
        e.Handled = True
    End Sub

    Private Sub btnAddLayer_Click(sender As Object, e As RoutedEventArgs)
        holeExplorer()
        'panningAusschalten()
        'If GisUser.ADgruppenname.ToLower = "umwelt" Then
        '    'btnKatParadigma.Visibility = Visibility.Visible
        'Else
        '    'btnKatParadigma.Visibility = Visibility.Collapsed
        'End If
        'If stwinthemen.Visibility = Visibility.Visible Then
        '    makeThemenInVis()
        'Else
        '    makeThemenVis()
        'End If
        'FocusManager.SetFocusedElement(Me, tbStichwort)
        e.Handled = True
    End Sub

    Public Sub makeThemenVis()
        stContext.Visibility = Visibility.Collapsed
        stwinthemen.Visibility = Visibility.Visible
    End Sub

    Private Sub makeThemenInVis()
        stContext.Visibility = Visibility.Collapsed
        stwinthemen.Visibility = Visibility.Collapsed
    End Sub

    Private Sub MainListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If MainListBox.SelectedItem Is Nothing Then Exit Sub
        If MainListBox.SelectedValue Is Nothing Then Exit Sub
        'aktfoto = CType(MainListBox.SelectedValue, Dokument)
        'If aktfoto.Initiale = "loeschmich" Then Exit Sub
        'Gesamtcursor = aktfoto.Handlenr
        'zeigeInMain(CLstart.myc.collFotos.Item(Gesamtcursor))
        e.Handled = True
    End Sub
    Private Sub chkauswahlgeaendert(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        e.Handled = True
        panningAusschalten()
        Dim nck As CheckBox = CType(sender, CheckBox)
        'angeklickt = True
        If nck.IsChecked Then
        Else
            For Each lay As clsLayerPres In layersSelected
                If lay.aid <> CInt(CStr(nck.Tag)) Then Continue For
                lay.isactive = False
                lay.RBischecked = False
                lay.mithaken = False
                If layerActive.aid <> lay.aid Then Continue For
                layerActive.aid = 0 'schaltet die darstellung des punktes weg
            Next
        End If
        ebenenListeAktualisieren()
        refreshMap(True, False)
        e.Handled = True
    End Sub
    Private Sub chkAktiveEbenegeaendert(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim nck As RadioButton = CType(sender, RadioButton)
        Dim tag As Integer
        panningAusschalten()
        tag = CInt(nck.Tag)
        modLayer.alteAktiveEbeneDeaktivieren(layersSelected)

        For Each lay As clsLayerPres In layersSelected
            If lay.aid = CInt(CStr(tag)) Then
                lay.isactive = True
                lay.mithaken = True
                lay.RBischecked = True
                layerActive = CType(lay.Clone, clsLayerPres)
                layerActive.aid = lay.aid
                layerHgrund.isactive = False
                If clsWMS.istWMSDBabfrage(lay.aid) Then
                    btnWMSgetfeatureinfo.Visibility = Visibility.Visible
                    MsgBox("Umschaltung in WMS-Datenabfragemodus !" & Environment.NewLine &
                             Environment.NewLine &
                           "Benutzen Sie den blauen Knopf oben  " & Environment.NewLine &
                           "mit der Aufschrift 'WMS'. " & Environment.NewLine &
                           " " & Environment.NewLine &
                           "Drücken Sie diesen Knopf und dann in die Karte, " & Environment.NewLine &
                           " Es erscheint dann die Datenbankinformation zum" & Environment.NewLine &
                           "angeklicketen Objekt!" & Environment.NewLine &
                           " " & Environment.NewLine,, "WMS - Datenabfrage")
                    btnWMSgetfeatureinfo.Visibility = Visibility.Visible
                    panningAusschalten()
                    'imageMapCanvas.Visibility = Visibility.Collapsed
                    'CanvasClickModus = "wmsdatenabfrage"

                End If
            End If
        Next
        ebenenListeAktualisieren()
        ' layerHgrund.isactive=true
        ' angeklickten hgrund ausklicken 
        rbHgrundAktiveEbene.IsChecked = False
        refreshMap(True, False)
        e.Handled = True
    End Sub


    Private Sub btnEbenenaktualisieren_Click(sender As Object, e As RoutedEventArgs)
        ebenenListeAktualisieren()
        leereSelectedlayersNachPres(layersSelected)
        MainListBox.ItemsSource = Nothing
        refreshMap(True, True)
        e.Handled = True
    End Sub

    Private Sub ebenenListeAktualisieren()
        'entladen
        For i = 0 To layersSelected.Count - 1
            If layersSelected(i).mithaken Then
                If Not warSchonGeladen(layersSelected(i).aid, layersSelected) Then
                    For Each lay As clsLayer In layersSelected
#If DEBUG Then
                        If layersSelected(i).aid = 303 Then
                            Debug.Print("")
                        End If
#End If
                        If lay.aid = CInt(layersSelected(i).aid) Then
                            layersSelected.Add(layersSelected(i))
                        End If
                    Next
                Else

                End If
            Else
                'ohne haken
                If warSchonGeladen(layersSelected(i).aid, layersSelected) Then
                    'entladen
                    For j = 0 To layersSelected.Count - 1
#If DEBUG Then
                        If layersSelected(j).aid = 303 Then
                            Debug.Print("")
                        End If
#End If
                        If layersSelected(j).aid = CInt(layersSelected(i).aid) Then
                            'a layersSelected(i).aid = 0
                            layersSelected(i).mithaken = False
                            layersSelected(i).RBischecked = False
                            layersSelected(i).isactive = False
                        End If
                    Next
                Else

                End If
            End If
        Next
        'a  erneuerLayercollection()
    End Sub

    'Private Shared Sub erneuerLayercollection()
    '    Dim tmpColl As New List(Of clsLayerPres)
    '    For Each layer In layersSelected
    '        If layer.aid > 0 Then
    '            tmpColl.Add(layer)
    '        End If
    '    Next
    '    layersSelected.Clear()
    '    For Each layer In tmpColl
    '        If layer.aid > 0 Then
    '            layersSelected.Add(layer)
    '        End If
    '    Next
    'End Sub




    Private Sub zeigeLegendeUndDoku(aktaid As Integer, aktsid As Integer, layerHatOS As Boolean)
        stContext.Width = cv1.Width '- 100
        stContext.Height = cv1.Height ' - 100
        stpDokuUndLegende.Width = stContext.Width - stpKnoeppeVertical.Width
        'stContext.Visibility = Visibility.Collapsed
        'stwinthemen.Visibility = Visibility.Visible
        'stwinthemen.Width = cv1.Width - 100
        'stwinthemen.Height = cv1.Height - 100
        Debug.Print("")
        btnObjektsuche.ToolTip = "Keine Objektsuche für diese Ebene verfügbar"
        btnObjektsuche.Visibility = Visibility.Collapsed
        stContext.Visibility = Visibility.Visible
        stwinthemen.Visibility = Visibility.Collapsed
        stpDoku.Visibility = Visibility.Visible
        'stpLegende.Visibility = Visibility.Visible
        stpObjektsuche.Visibility = Visibility.Collapsed
        stpDokuUndLegende.Visibility = Visibility.Visible

        ladeRTF(CStr(aktaid), "\rtfdoku\", richTextBoxDoku)
        'ladeRTF(CStr(aktaid), "\rtflegend\", richTextBoxLeg)
        If ladePDF(aktaid, aktsid) > 0 Then
            stpPDFliste.Visibility = Visibility.Visible
        Else
            stpPDFliste.Visibility = Visibility.Collapsed

        End If
    End Sub

    Private Function ladePDF(aktaid As Integer, aktsid As Integer) As Integer
        'prüfen ob es einen eintrag in webgiscontrol.pdfdateien gibt
        Dim dt As DataTable = holePdf2SidDT(aktaid, aktsid)
        If dt.Rows.Count < 1 Then
            tbHinweisausPDFDateien.Text = ""
            dgPDFliste.DataContext = Nothing
            Return 0
        End If
        'wenn ja: 
        dgPDFliste.DataContext = dt
        tbHinweisausPDFDateien.Text = "Wichtig!!! Es wurden " & dt.Rows.Count & " PDF-Dateien zum Thema gefunden. "
        Return dt.Rows.Count
        'dt liste erstellen
        'objektliste umwandeln
        'objektliste darstellen
    End Function

    Private Sub btnDoku_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        stpDoku.Visibility = Visibility.Visible
        'stpLegende.Visibility = Visibility.Visible
        stpPDFliste.Visibility = Visibility.Visible
        stpObjektsuche.Visibility = Visibility.Collapsed
        Dim nck As Button = CType(sender, Button)

        'Dim aktsid = CInt(nck.Uid)

        'MsgBox(" baustelle dokumentation aid Button : " & CStr(nck.Tag))
        'stContext.Visibility = Visibility.Collapsed
        '  ladeRTF(CStr(nck.Tag))

        ladeRTF(CStr(aktaid), "\rtfdoku\", richTextBoxDoku)
        'ladeRTF(CStr(aktaid), "\rtflegend\", richTextBoxLeg)
        If ladePDF(aktaid, aktsid) > 0 Then
            stpPDFliste.Visibility = Visibility.Visible
        Else
            stpPDFliste.Visibility = Visibility.Collapsed

        End If
        e.Handled = True
    End Sub

    Private Sub ladeRTF(aid As String, subdir As String, richTextBoxAll As RichTextBox)
        Try
            Dim ddatei = serverUNC & "nkat\aid\" & aid & subdir & aid & ".rtf"
            Dim fi As New IO.FileInfo(ddatei)
            richTextBoxAll.Document.Blocks.Clear()
            If fi.Exists Then
                Using datei As IO.StreamReader = New IO.StreamReader(ddatei)
                    rtfTextDoku = datei.ReadToEnd
                End Using
                Dim documentBytes = Text.Encoding.UTF8.GetBytes(rtfTextDoku)
                Dim reader = New System.IO.MemoryStream(documentBytes)
                reader.Position = 0
                richTextBoxAll.SelectAll()
                richTextBoxAll.Selection.Load(reader, DataFormats.Rtf)
            Else
                'keine Datei gefunden
            End If
        Catch ex As Exception
            l("fehler in winRTF_Loaded " & ex.ToString)
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Function bildeSQLString(schema As String, tabelle As String, okat As String, ofeld As String, volltextsucheSql As String) As String
        Try
            Dim sql As String = ""
            l("bildeSQLString---------------------- anfang")
            If okat = String.Empty And ofeld = String.Empty Then
                sql = " Select  * from  " & schema & ".os_" & tabelle
            End If

            If okat = String.Empty And ofeld <> String.Empty Then
                'sql = " Select  * from  " & schema & ".os_" & tabelle & " where lower(ofeldsuche) like '%" & ofeld.ToLower & "%'"

                sql = " Select  * from  " & schema & ".os_" & tabelle & " where " &
                                  "(" & volltextsucheSql & ")  "
            End If

            If okat <> String.Empty And ofeld = String.Empty Then
                sql = " Select  * from  " & schema & ".os_" & tabelle & " where lower(okategorie)= '" & okat.ToLower & "'"
            End If

            If okat <> String.Empty And ofeld <> String.Empty Then
                sql = " Select  * from  " & schema & ".os_" & tabelle & " where " &
                " lower(okategorie) = '" & okat.ToLower & "' and (" & volltextsucheSql & ")  "
            End If
            l("sql:" & sql)
            l("bildeSQLString---------------------- ende")
            Return sql
        Catch ex As Exception
            l("Fehler in bildeSQLString: " & ex.ToString)
            Return ""
        End Try
    End Function


    Private Sub sichtbarMachenObjektsuche()
        stpDoku.Visibility = Visibility.Collapsed
        stpPDFliste.Visibility = Visibility.Collapsed
        stpObjektsuche.Visibility = Visibility.Visible
    End Sub

    Private Sub btnLegende_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        'Dim nck As Button = CType(sender, Button)
        'MsgBox(" legende aid Button : " & CStr(nck.Tag))
        'stContext.Visibility = Visibility.Collapsed
        Dim nlay As New clsLayerPres
        nlay.aid = aktaid 'CInt(nck.Tag)

        pgisTools.getStamm4aid(nlay)
        showFreiLegende4Aid(nlay)
        e.Handled = True
    End Sub

    Private Sub showFreiLegende4Aid(nlay As clsLayerPres)
        Dim rtfdatei As String
        '= serverUNC & "nkat\aid\" & daaid & "\rtflegend\" & daaid & ".rtf"
        rtfdatei = nsMakeRTF.rtf.makeftlLegende4Aid(nlay)
        If rtfdatei = "error" Or rtfdatei = "" Then
            MessageBox.Show("Keine Legende vorhanden!")
            Exit Sub
        End If
        stContext.Visibility = Visibility.Collapsed
        Dim freileg As New winLeg(rtfdatei)
        freileg.Show()
    End Sub
    Private Sub clearAllSlots()
        GC.Collect()
        For i = 0 To slots.Count - 1
            If slots(i).refresh Then
                'slots(i).canvas.Children.Clear()
                'If slots(i).image IsNot Nothing Then
                '    slots(i).image.Source = Nothing
                '    slots(i).image = Nothing
                'End If
                'slots(i).image = New Image
                'leeresbild(slots(i).image)
                slots(i).setEmpty()
            End If
        Next
        GC.Collect()
    End Sub
    'Private Sub clearCanvasALT(vgrundRefresh As Boolean, hgrundrefresh As Boolean, osrefresh As Boolean)
    '    GC.Collect()
    '    If vgrundRefresh Then
    '        cv1.Children.Clear()
    '        If VGcanvasImage IsNot Nothing Then
    '            VGcanvasImage.Source = Nothing
    '            VGcanvasImage = Nothing
    '        End If
    '        VGcanvasImage = New Image
    '        leeresbild(VGcanvasImage)
    '    End If
    '    'If osrefresh Then
    '    OSmapCanvas.Children.Clear()
    '    If OScanvasImage IsNot Nothing Then
    '        OScanvasImage.Source = Nothing
    '        OScanvasImage = Nothing
    '    End If
    '    OScanvasImage = New Image
    '    leeresbild(OScanvasImage)
    '    'End If
    '    'leeresbild(canvasImage)
    '    'mapCanvas.Children.Add(canvasImage)
    '    GC.Collect()
    '    If hgrundrefresh Then
    '        cv0.Children.Clear()
    '        If HGcanvasImageRange0 IsNot Nothing Then
    '            HGcanvasImageRange0.Source = Nothing
    '            HGcanvasImageRange0 = Nothing
    '        End If
    '        HGcanvasImageRange0 = New Image
    '        leeresbild(HGcanvasImageRange0)
    '    End If
    'End Sub


    Private Sub leeresbild(canvasImage As Image)
        Dim myBitmapImage As New BitmapImage()
        '     Dim aufruf As String = New Uri("/mgis;component/icons/leer.png", UriKind.Absolute) 'serverWeb & "/leer.png" '"P:\a_vs\NEUPara\mgis\leer.png"
        Try
            myBitmapImage.BeginInit()
            myBitmapImage.UriSource = New Uri("/mgis;component/icons/leer.png", UriKind.RelativeOrAbsolute)
            myBitmapImage.EndInit()
            canvasImage.Source = myBitmapImage
            GC.Collect()
        Catch ex As Exception
            l("fehler in leeresbild: " & aufruf & " /// " & ex.ToString)
        End Try
    End Sub

    Private Sub btnrefresh_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'darstellen
        If Not ladevorgangAbgeschlossen Then Exit Sub
        panningAusschalten()
        resizeWindow()
        refreshMap(True, True)
        starteAnimation()

    End Sub

    Private Sub starteAnimation()

    End Sub

    Private Sub btnremovelayerFromList(sender As Object, e As RoutedEventArgs)
        ' nur vordergrund
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim nck As Button = CType(sender, Button)
        Dim tag As Integer
        panningAusschalten()
        tag = CType(nck.Tag, Int16)
        Dim mesres As MessageBoxResult
        mesres = MessageBox.Show("Ebene wirklich löschen?",
                                 "Vertippt? " & aktvorgangsid, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
        If mesres = MessageBoxResult.Yes Then
            'aus layersselected entferneen
            For Each clay As clsLayerPres In layersSelected
                If clay.aid = CInt(tag) Then
                    layersSelected.Remove(clay)
                    If clay.aid = layerActive.aid Then
                        layerActive.clearPres()
                    End If
                    Exit For
                End If
            Next
            'ebenenListeAktualisieren()
            refreshMap(True, False)
        End If
        e.Handled = True
    End Sub











    Private Sub btnclosesuchform_Click(sender As Object, e As RoutedEventArgs)
        stwinthemen.Visibility = Visibility.Collapsed
        e.Handled = True
    End Sub
    Private Sub btnStichwort_Click(sender As Object, e As RoutedEventArgs)

        tbStichwort.Text = tbStichwort.Text.ToLower.Trim
        tbStichwort.Text = clsString.normalize_Filename(tbStichwort.Text, " ")
        tbStichwort.Text = tbStichwort.Text.Replace("-", " ").Trim
        stichwortsucheDurchfuehren()
        tbebenenauswahlinfo.Visibility = Visibility.Visible
        e.Handled = True
    End Sub

    Private Sub stichwortsucheDurchfuehren()
        Dim anzahlSchonGeladeneEbenen As Integer = 0
        If tbStichwort.Text.Trim.Count < 3 Then
            MsgBox("Bitte mind. 3 Buchstaben angeben !", MsgBoxStyle.OkOnly, "Hinweis")
            Exit Sub
        End If
        LastThemenSuche = "stichwort"
        layersTemp = modLayer.getLayer4stichwort(tbStichwort.Text, anzahlSchonGeladeneEbenen)
        layersTemp.Sort()
        Dim warschongeladenString As String = ""
        If anzahlSchonGeladeneEbenen > 0 Then
            warschongeladenString = "  (Es sind bereits " & anzahlSchonGeladeneEbenen &
                " Ebenen davon geladen und werden daher nur in grau angezeigt.)"
        End If
        tbtreffer.Text = "   >>> " & layersTemp.Count & " Treffer.  " & warschongeladenString


        dgErgebnis.ItemsSource = layersTemp
    End Sub

    Private Sub dgErgebnis_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If dgErgebnis.SelectedItem Is Nothing Then Exit Sub
        Dim item As New clsLayerPres
        Try
            item = CType(dgErgebnis.SelectedItem, clsLayerPres)
        Catch ex As Exception
            nachricht(ex.ToString)
            Exit Sub
        End Try
        'neuesLayerHinzufuegen
        item.mithaken = True
        item.RBischecked = False
        item.isactive = False
        layersSelected.Add(item)
        'ergebenislisteaktualisieren
        If LastThemenSuche = "stichwort" Then
            stichwortsucheDurchfuehren()
        End If
        refreshMap(True, True)
        dgErgebnis.SelectedItem = Nothing
        e.Handled = True

    End Sub


    Private Sub btnPdf_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        mapfileNamenNeuBerechnen()
        If stPDFDruck.Visibility = Visibility.Collapsed Then
            disableMyStackpanel(spButtonMenu, False)
            rbMitMasstab.IsChecked = False

            schliesseSliderDialog()
            setHGabdecker4SliderValue(0)
            openPDFdialog()
            If rbMitMasstab.IsChecked Then
                spdruckmasstab.Visibility = Visibility.Visible
                spAusrichtung.Visibility = Visibility.Visible
                cmbMasstabDruck.SelectedValue = Nothing

            Else
                cmbMasstabDruck.SelectedValue = Nothing
                spdruckmasstab.Visibility = Visibility.Hidden
                spAusrichtung.Visibility = Visibility.Hidden
            End If
        Else
            closePDFDialog()
            disableMyStackpanel(spButtonMenu, True)
        End If
        e.Handled = True
    End Sub

    Private Sub openPDFdialog()
        stPDFDruck.Visibility = Visibility.Visible
        stMenu.Visibility = Visibility.Collapsed
        stPDFDruck.Width = dockMenu.Width
        stPDFDruck.Height = dockMenu.Height
        quer.IsChecked = True
        rbFormatA4.IsChecked = True
        rbOhneMasstab.IsChecked = True
        cbhochaufloesend.IsChecked = False
        'pdfrahmenNeuPLatzieren("quer")
        'cvPDFrechteck.Visibility = Visibility.Visible
        'PDF_druckMassStab = PDF_postition_desRahmensBestimmen()
        'tbMasstabDruck.Text = CInt(PDF_druckMassStab).ToString


        setAuswahlRechteckProps("quer")

        tbPDF_Bemerkung.Text = getPDFBemerkung()
        tbPDF_Ort.Text = getPDFOrt(kartengen.aktMap.aktrange)
        'spdruckmasstab.Visibility = Visibility.Hidden
        If STARTUP_mgismodus = "vanilla" Then
            gbPDFparadigma.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Function getPDFOrt(lokrange As clsRange) As String
        lokrange.CalcCenter()
        Return "UTM32: " & CInt(lokrange.xcenter) & ", " & CInt(lokrange.ycenter)
    End Function

    Private Function getPDFBemerkung() As String
        If aktvorgangsid.IsNothingOrEmpty Then
            Return GisUser.username
        Else
            Return GisUser.username & " (" & aktvorgangsid & ")"
        End If
    End Function



    Private Sub cmbHgrund_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        panningAusschalten()
        myglobalz.mgisBackModus = False
        Dim oldHgrund As New clsLayerPres
        oldHgrund.aid = layerHgrund.aid
        oldHgrund.titel = layerHgrund.titel
        Dim temp As clsLayerPres = CType(cmbHgrund.SelectedValue, clsLayerPres)
        'Dim aid As String = CType(cmbHgrund.SelectedValue, String)
        If temp.aid = 1 Then
            Debug.Print("")
        End If
        If temp.aid = -1 Then
            btnHGSlider.IsOpen = True
            cmbHgrund.SelectedValue = "0"
            e.Handled = True
            Exit Sub
        End If
        slots(1).refresh = False

        Dim item As clsLayerPres = CType(cmbHgrund.SelectedItem, clsLayerPres)
        tbhgrund.Text = CType(item.titel, String)

        If temp.aid = 0 Then
            layerHgrund.clearPres()
            layerHgrund.aid = 0
            layerHgrund.titel = "Kein Hintergrund"
            layerHgrund.mithaken = False
            slots(0).layer = layerHgrund.kopie
        End If
        ' zwischenbildBitteWarten() 'False, True)
        If CInt(temp.aid) > 0 Then

            layerHgrund.aid = CInt(temp.aid)
            layerHgrund.mithaken = True
            pgisTools.getStamm4aid(layerHgrund)
            If luftbildErsetztFlurkarte(oldHgrund, layerHgrund) Then
                'oldHgrund.aid
                addWeisseFlurkarte()
                slots(1).refresh = True
            End If
            If layerHgrund.mit_imap Then
                rbHgrundAktiveEbene.Visibility = Visibility.Visible
            Else
                rbHgrundAktiveEbene.Visibility = Visibility.Collapsed
            End If
        End If
        stContext.Visibility = Visibility.Collapsed
        refreshMap(False, True)
        e.Handled = True
    End Sub

    Private Sub addWeisseFlurkarte()
        Dim newlayer As New clsLayerPres
        newlayer.aid = 72 ' weisse flurkarte
        newlayer.aid = 358 'sbo schwarz

        If isInSelectedLayers(newlayer.aid) Then
            For Each lay As clsLayerPres In layersSelected
                If lay.aid = newlayer.aid Then
                    lay.mithaken = True
                End If
            Next
        Else
            pgisTools.getStamm4aid(newlayer)
            newlayer.RBischecked = False
            newlayer.isactive = False
            layersSelected.Add(newlayer)
        End If

    End Sub

    Private Function isInSelectedLayers(aid As Integer) As Boolean
        Try
            l("isInSelectedLayers---------------------- anfang")
            For Each lay In layersSelected
                If lay.aid = aid Then Return True
            Next
            l("isInSelectedLayers---------------------- ende")
            Return False
        Catch ex As Exception
            l("Fehler in isInSelectedLayers: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Function luftbildErsetztFlurkarte(oldHgrund As clsLayerPres, layerHgrund As clsLayerPres) As Boolean
        If oldHgrund.aid = 253 And layerHgrund.titel.ToLower.StartsWith("luftbild") Then
            Return True
        End If
        Return False
    End Function

    Private Sub chkBoxAusschnitt_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        myglobalz.mgisBackModus = False
        imgpin.Visibility = Visibility.Collapsed
        If chkBoxAusschnitt.IsChecked Then
            imageMapCanvas.Visibility = Visibility.Collapsed
            zeichneOverlaysGlob = True : zeichneImageMapGlob = False
            'panningAusschalten()
            'refreshMap()
            cv1.Cursor = Cursors.Cross
            CanvasClickModus = "Ausschnitt"

            DrawRectangle(cv1)
        Else
            imageMapCanvas.Visibility = Visibility.Visible
            zeichneOverlaysGlob = True : zeichneImageMapGlob = True
            refreshMap(True, True)
            CanvasClickModus = ""
            imageMapCanvas.Visibility = Visibility.Visible
        End If

        e.Handled = True
    End Sub

    Private Sub btnRemoveAllLayers_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        panningAusschalten()
        makeThemenInVis()
        Dim mesres As MessageBoxResult
        mesres = MessageBox.Show("ALLE Ebenen wirklich löschen?",
                                 "Vertippt? " & aktvorgangsid, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
        If mesres = MessageBoxResult.Yes Then
            'panningAusschalten()
            cballeeinaus.IsChecked = True
            layersSelected.Clear()
            layerActive.aid = 0
            refreshMap(True, False)
        End If
        e.Handled = True
    End Sub



    Private Sub nachLinks_Click(sender As Object, e As RoutedEventArgs)
        'Dim saidliste = ""
        For Each clay As clsLayerPres In layersTemp
            'saidliste = saidliste & ";" & clay.aid
            clay = clsWebgisPGtools.setPresetationProps(clay)
            clay = clsWebgisPGtools.setSichtbarkeitRBaktiveEbene(clay)
            layersSelected.Add(clay)
        Next
        makeThemenInVis()
        refreshMap(True, False)
        e.Handled = True
    End Sub

    Private Sub nachRechts_Click(sender As Object, e As RoutedEventArgs)
        layersSelected.Clear()
        MainListBox.ItemsSource = Nothing
        e.Handled = True
    End Sub


    Private Sub btnnatur_Click(sender As Object, e As RoutedEventArgs)

    End Sub



    Private Sub cmbfavorite_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        panningAusschalten()
        makeThemenInVis()
        Dim item As ComboBoxItem
        item = CType(cmbfavorite.SelectedItem, ComboBoxItem)
        If item.Tag.ToString = "0" Then Exit Sub
        If item.Tag.ToString = "nichts" Then Exit Sub
        handleFavorite(item.Tag.ToString, "fix")
        cmbfavorite.SelectedIndex = 0
        tbFavoname.Text = favoritakt.titel
        cballeeinaus.IsChecked = True
        e.Handled = True
    End Sub

    Private Sub handleFavorite(tag As String, istfix As String)
        'favo einlesen
        Dim erfolg As Boolean
        Select Case tag.ToLower
            Case "meine"
                erfolg = favoTools.FavoritLaden("meine", GisUser.username)
            Case "meinespeichern"
                favoTools.FavoritSave(GisUser.username)
                Exit Sub
            Case Else
                erfolg = favoTools.FavoritLaden(istfix, tag)
        End Select
        If Not erfolg Then
            MsgBox("Sie haben noch keine Favoriten gespeichert.")
            Exit Sub
        End If
        favoritenUmsetzen()
        'in explorer und karte umsetzen
        refreshMap(True, True)
    End Sub

    Private Sub favoritenUmsetzen()
        'favo auf aid umstellen
        layerActive.aid = getinteger(favoritakt.aktiv)
        layerHgrund.aid = getinteger(favoritakt.hgrund) 'CInt(favoritakt.hgrund.Replace(";", ""))
        If layerActive.aid > 0 Then
            pgisTools.getStamm4aid(layerActive)
            'hintergrund ist aktiv
            layerActive.isactive = True
            layerActive.mithaken = True
            layerActive.RBischecked = True
        End If



        If layerHgrund.aid = 0 Then
            ' hgrund ist KEIN HGRUND
            clsGisstartPolitik.setzeKeinHintergrundLayer(layerHgrund)
            rbHgrundAktiveEbene.Visibility = Visibility.Collapsed
            layerHgrund.mithaken = False
            If layerActive.isHgrund Then
                'leerer hintergrund kann nicht aktiv sein
                layerActive.aid = 0
                layerActive.RBsichtbarkeit = Visibility.Collapsed
                layerActive.isactive = False
                layerActive.mithaken = False
                layerActive.RBischecked = False

            Else

            End If
        Else
            pgisTools.getStamm4aid(layerHgrund)
            If layerHgrund.mit_imap Then
                rbHgrundAktiveEbene.Visibility = Visibility.Visible
            Else
                rbHgrundAktiveEbene.Visibility = Visibility.Collapsed
            End If

            If layerHgrund.aid = layerActive.aid Then
                'hintergrund ist aktiv
                layerHgrund.isactive = True
                layerHgrund.mithaken = True
                layerHgrund.RBischecked = True
                rbHgrundAktiveEbene.IsChecked = True
            End If
        End If

        'cmbHgrund.SelectedValue = layerHgrund.aid
        layersSelected.Clear()

        layersSelected.Clear()

        MainListBox.ItemsSource = Nothing
        MainListBox.ItemsSource = layersSelected

        Dim vorhanden() As String : vorhanden = favoritakt.vorhanden.Split(";"c)
        Dim gecheckt() As String : gecheckt = favoritakt.gecheckted.Split(";"c)
        layersSelected = ListeVorhandeneLayersUmsetzenNachPres(vorhanden, gecheckt)
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            userlayerCorrectDarstellen()
        End If
        tbFavoname.Text = favoritakt.titel
    End Sub

    Private Shared Function getinteger(id As String) As Integer
        If (id.IsNothingOrEmpty) Then
            Return 0
        End If
        Return CInt(id.Replace(";", ""))
    End Function

    Private Function ListeVorhandeneLayersUmsetzenNachPres(vorhanden() As String, gecheckt() As String) As List(Of clsLayerPres)

        Dim newlist As New List(Of clsLayerPres)
        erzeugeLeereselectedLayerliste(vorhanden, newlist, gecheckt)
        leereSelectedlayersNachPres(newlist)
        Return newlist
    End Function

    Private Sub leereSelectedlayersNachPres(ByRef newlist As List(Of clsLayerPres))
        For Each nlay As clsLayerPres In newlist
#If DEBUG Then
            If nlay.aid = 358 Then
                Debug.Print("")
            End If
#End If
            ' nlay.thumbnailFullPath = myglobalz.serverUNC & "\nkat\aid\" & nlay.aid & "\thumbnail\tn.png"
            nlay.thumbnailFullPath = myglobalz.serverUNC & "nkat\thumbnails\" & nlay.aid & ".png"
            nlay.farbe = getColorBrush4hauptSachgebiet(nlay.standardsachgebiet)
            nlay.etikettfarbe = Brushes.LightGray
            'If nlay.mit_objekten Then
            '    nlay.myFontStyle = FontStyles.Italic
            'End If
            nlay = clsWebgisPGtools.setSichtbarkeitRBaktiveEbene(nlay)
            If nlay.aid = GisUser.userLayerAid And GisUser.userLayerAid > 0 Then
                nlay.aid = GisUser.userLayerAid
                pgisTools.getStamm4aid(nlay)
                nlay.titel = "VorgangsRaumbezüge: " & GisUser.username
                nlay.thumbnailFullPath = myglobalz.serverUNC & "nkat\thumbnails\userlayer.png"
                nlay.dokutext = "Die Raumbezüge des Paradigmavorgangs werden hier in blau dargestellt."
                nlay.RBischecked = False
                nlay.RBsichtbarkeit = Visibility.Visible
                nlay.isactive = False
                nlay.farbe = Brushes.LightSalmon
                'nlay.dokutext = clsWebgisPGtools.bildeDokuTooltip(nlay)
            End If
            If layerActive.aid = nlay.aid Then
                layerActive.isactive = True
                nlay.isactive = True
                nlay.RBischecked = True
                pgisTools.getStamm4aid(layerActive)
                nlay.etikettfarbe = Brushes.White
                nlay.dokutext = clsWebgisPGtools.bildeDokuTooltip(nlay)
            End If
            If layerHgrund.aid = nlay.aid Then
                nlay.isHgrund = True
                nlay.mithaken = True

                layerHgrund.mithaken = True
                pgisTools.getStamm4aid(layerHgrund)
                nlay.dokutext = clsWebgisPGtools.bildeDokuTooltip(nlay)
            End If
            'If istLayerGechecktet(nlay.aid, gecheckt) Then
            '    nlay.mithaken = True
            'End If
            nlay.kategorie = clsWebgisPGtools.bildeNiceSachgebiet(nlay).ToLower



            If nlay.mithaken Then
                nlay.myFontWeight = FontWeights.Bold
            Else
                nlay.myFontWeight = FontWeights.Normal
            End If
            nlay.suchfeld = nlay.titel & " " & nlay.schlagworte
            nlay.dokutext = clsWebgisPGtools.bildeDokuTooltip(nlay)
        Next
    End Sub

    Private Sub erzeugeLeereselectedLayerliste(vorhanden() As String, newlist As List(Of clsLayerPres), gecheckt() As String)
        Dim nlay As New clsLayerPres
        For Each vorh As String In vorhanden
            nlay = New clsLayerPres
            If Not vorh.IsNothingOrEmpty Then
                nlay.aid = CInt(vorh)
                If nlay.aid < 1 Then Continue For
                If istLayerGechecktet(nlay.aid, gecheckt) Then
                    nlay.mithaken = True
                End If

                nlay = pgisTools.getStamm4aid(nlay)
                If nlay.aid < 1 Then
                    'Hier werden deaktivierte Layers aussortiert
                    Continue For
                End If
                newlist.Add(nlay)
            End If
        Next
    End Sub

    Private Shared Sub userlayerCorrectDarstellen()
        Dim nlay As New clsLayerPres
        l("userlayer in liste einbauen")
        If STARTUP_mgismodus.ToLower <> "paradigma" Then
            l("kein paradigmamodus")
            MsgBox("keinpmode")
            Exit Sub
        End If
        If GisUser.userLayerAid > 0 Then
            If clsString.isinarray(favoritakt.vorhanden, CType(GisUser.userLayerAid, String), ";") Then

            Else
                If layerActive.aid = GisUser.userLayerAid And GisUser.userLayerAid > 0 Then
                    nlay.aid = GisUser.userLayerAid
                    pgisTools.getStamm4aid(nlay)
                    nlay.titel = "VorgangsRaumbezüge: " & GisUser.username
                    nlay.suchfeld = nlay.titel & " " & nlay.schlagworte
                    nlay.thumbnailFullPath = myglobalz.serverUNC & "nkat\thumbnails\userlayer.png"
                    nlay.dokutext = "Die Raumbezüge des Paradigmavorgangs werden hier in blau dargestellt."
                    nlay.mithaken = True
                    nlay.RBischecked = True
                    nlay.isactive = True
                    nlay.RBsichtbarkeit = Visibility.Visible
                    nlay.farbe = Brushes.LightSalmon
                Else
                    nlay.aid = GisUser.userLayerAid
                    pgisTools.getStamm4aid(nlay)
                    nlay.titel = "VorgangsRaumbezüge: " & GisUser.username
                    nlay.thumbnailFullPath = myglobalz.serverUNC & "nkat\thumbnails\userlayer.png"
                    nlay.dokutext = "Die Raumbezüge des Paradigmavorgangs werden hier in blau dargestellt."
                    nlay.suchfeld = nlay.titel & " " & nlay.schlagworte
                    nlay.mithaken = True
                    nlay.RBischecked = False
                    nlay.RBsichtbarkeit = Visibility.Visible
                    nlay.isactive = False
                    nlay.farbe = Brushes.LightSalmon
                End If
                'nlay.aid = userLayerAid
                'pgisTools.getStamm4aid(nlay)
                'nlay.titel = "Userlayer: " &   GisUser.username
                'nlay.mithaken = True
                'nlay.RBischecked = True
                'nlay.RBsichtbarkeit = Visibility.Visible
                'nlay.farbe = Brushes.LightSalmon
                layersSelected.Add(nlay)
            End If
        End If
    End Sub

    Private Function istLayerGechecktet(aid As Integer, gecheckt() As String) As Boolean
        For Each str As String In gecheckt
            If aid.ToString = str Then
                If aid <> GisUser.userLayerAid Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function



    Private Sub btnGrenzen_Click(sender As Object, e As RoutedEventArgs)
        LastThemenSuche = "hauptsachgebiet"
        dgErgebnis.ItemsSource = Nothing
        layersTemp = modLayer.getLayer4sachgebiet("grenzen")
        layersTemp.Sort()
        clsWebgisPGtools.dombineLayerDoku(layersTemp, allDokus)
        dgErgebnis.ItemsSource = layersTemp
        e.Handled = True
    End Sub

    Private Sub cballeeinaus_Click(sender As Object, e As RoutedEventArgs)
        Try
            l("cballeeinaus_Click---------------------- anfang")
            panningAusschalten()

            If ladevorgangAbgeschlossen Then
                If cballeeinaus.IsChecked Then
                    For i = 0 To layersSelected.Count - 1
                        layersSelected(i).mithaken = layersSelectedOld(i).mithaken
                    Next
                Else
                    layersSelectedOld = modLayer.kopiereLayersSeelected(layersSelected)
                    'alle vordergrundebenen aus
                    For Each nlay As clsLayerPres In layersSelected
                        nlay.mithaken = False
                        nlay.isactive = False
                        nlay.RBischecked = False
                    Next
                End If
            End If
            refreshMap(True, False)
            e.Handled = True
            l("cballeeinaus_Click---------------------- ende")
        Catch ex As Exception
            l("Fehler in cballeeinaus_Click: " & ex.ToString())
        End Try
    End Sub

    Private Sub btnBalken_Click(sender As Object, e As RoutedEventArgs)
        stckBalken.Visibility = Visibility.Collapsed
        e.Handled = True

    End Sub

    Private Sub btnflurlarte_Click(sender As Object, e As RoutedEventArgs)
        LastThemenSuche = "hauptsachgebiet"
        dgErgebnis.ItemsSource = Nothing
        layersTemp = modLayer.getLayer4sachgebiet("flurkarte")
        layersTemp.Sort()
        clsWebgisPGtools.dombineLayerDoku(layersTemp, allDokus)
        dgErgebnis.ItemsSource = layersTemp
        e.Handled = True
    End Sub

    Private Sub cmbMasstab_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub

        myglobalz.mgisBackModus = False
        panningAusschalten()
        If cmbMasstab.SelectedItem Is Nothing Then Exit Sub
        Dim item As clsMasstab = CType(cmbMasstab.SelectedItem, clsMasstab)

        'tbMasstab.Text = " 1: " & CType(item.Tag, String)
        'setTBmasstab(CType(item.Tag, Double))  
        setNewMasstab(item.tagVal, False)
        cmbMasstab.SelectedValue = Nothing
        e.Handled = True
    End Sub

    Private Sub setNewMasstab(item As Integer, useMouseWheelCenter As Boolean)
        myglobalz.aktmasstabTag = CInt(item)
        calcNewRange(CDbl(item), useMouseWheelCenter)
        refreshMap(True, True)
    End Sub

    Private Sub btncloseDoku_Click(sender As Object, e As RoutedEventArgs)
        stContext.Visibility = Visibility.Collapsed
        e.Handled = True
    End Sub

    Private Sub btnKopieInClipboard_Click(sender As Object, e As RoutedEventArgs)
        Clipboard.Clear()
        Clipboard.SetText(rtfTextDoku, TextDataFormat.Rtf)
        MsgBox("Sie können den Text jetzt mit Strg-v  in ein Word-Dokument einfügen!",, "Zwischenablage")
        e.Handled = True
    End Sub

    Private Sub btnKopieInClipboardLegende_Click(sender As Object, e As RoutedEventArgs)

        Try
            l("btnKopieInClipboardLegende_Click---------------------- anfang")
            Clipboard.Clear()
            Clipboard.SetText(rtfTextLegende, TextDataFormat.Rtf)
            MsgBox("Sie können den Text jetzt mit Strg-v  in ein Word-Dokument einfügen!",, "Zwischenablage")
            l("btnKopieInClipboardLegende_Click---------------------- ende")
        Catch ex As Exception
            l("Fehler in btnKopieInClipboardLegende_Click: ", ex)

        End Try


        e.Handled = True
    End Sub

    Private Sub btnSuchobjAusSchalten_Click(sender As Object, e As RoutedEventArgs)
        SuchobjektAusschalten()
        e.Handled = True
    End Sub



    Public Sub Polygon_MouseDownKIllSuchPolygon(sender As Object, e As MouseButtonEventArgs)
        Dim eee As System.Windows.Shapes.Polygon = DirectCast(e.Source, System.Windows.Shapes.Polygon)
        aktFST = New ParaFlurstueck
        imgpin.Visibility = Visibility.Collapsed
        suchCanvas.Visibility = Visibility.Collapsed

        ' refreshMap()
    End Sub

    Private Sub MouseWheelHandlerTBm(sender As Object, e As MouseWheelEventArgs) Handles tbMasstab.MouseWheel, imageMapCanvas.MouseWheel
        ' Moves the TextBox named box when the mouse wheel is rotated.
        ' The TextBox is on a Canvas named MainCanvas.
        panningAusschalten()
        Dim tmasstab As Integer = CInt(scaleScreen.aktMasstab)
        ' If the mouse wheel delta is positive, move the box up.
        If e.Delta > 0 Then

            'reinzoomen

            Mouse.Capture(Nothing)
            KoordinateKLickpt = e.GetPosition(cv1)
            tbMinimapCoordinate2.Text = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint
            'CanvasClickModus = ""
            'Dim tt = Canvas.GetTop(cv1)
            'If Canvas.GetTop(cv1) >= 1 Then
            '    Canvas.SetTop(cv1, Canvas.GetTop(cv1) - 1)
            'End If
            ' Debug.Print(CType(aktmasstabTag, String) & scaleScreen.aktMasstab)
            For i = 0 To (masstaebe.Count - 1)
                'Debug.Print(masstaebe(i).ToString)
                If masstaebe(i).tagVal > tmasstab Then
                    setNewMasstab(masstaebe(i).tagVal, True)
                    Exit Sub
                End If
            Next


            'den naechsthöheren Maßstab nehmen
        End If

        ' If the mouse wheel delta is negative, move the box down.
        If e.Delta < 0 Then
            'reinzoomen
            Mouse.Capture(Nothing)
            KoordinateKLickpt = e.GetPosition(cv1)
            tbMinimapCoordinate2.Text = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint

            Debug.Print(CType(aktmasstabTag, String) & scaleScreen.aktMasstab)
            For i = (masstaebe.Count - 1) To 0 Step -1
                Debug.Print(masstaebe(i).ToString)
                If masstaebe(i).tagVal < tmasstab Then
                    setNewMasstab(masstaebe(i).tagVal, True)
                    Exit Sub
                End If
            Next
            'Dim tt = Canvas.GetTop(mapCanvas)
            'If (Canvas.GetTop(mapCanvas) + mapCanvas.Height) <= mapCanvas.Height Then
            '    Canvas.SetTop(mapCanvas, Canvas.GetTop(mapCanvas) + 1)
            'End If
        End If
        'tbMasstab.Text = (e.Delta).ToString


    End Sub

    Private Sub tbMasstab_MouseDown(sender As Object, e As MouseButtonEventArgs)
        cmbMasstab.IsDropDownOpen = True
        e.Handled = True
    End Sub

    Private Sub btnclosePDF_Click(sender As Object, e As RoutedEventArgs)
        closePDFDialog()
        disableMyStackpanel(spButtonMenu, True)
        e.Handled = True
    End Sub

    Private Sub closePDFDialog()
        tbPdfMasstabserror.Text = ""
        stPDFDruck.Visibility = Visibility.Collapsed
        stMenu.Visibility = Visibility.Visible
        cvPDFrechteck.Visibility = Visibility.Collapsed
        cvPDFrechteck.Children.Clear()
        auswahlRechteck = Nothing
        stMenu.Visibility = Visibility.Visible
        GC.Collect()
    End Sub


    Private Function getAusrichtung() As String
        Dim ausrichtung As String
        If quer.IsChecked Then
            ausrichtung = "quer"
        Else
            ausrichtung = "hoch"
        End If
        Return ausrichtung
    End Function

    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        tools.rangeSpeichern(kartengen.aktMap.aktrange)
        favoTools.FavoritSave("zuletzt")
        'tools.dirSpeichern()
    End Sub


    Private Sub dgPDFliste_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If dgPDFliste.SelectedItem Is Nothing Then Exit Sub
        '   Dim myvali$ = CStr(dgPDFliste.SelectedValue)
        Dim item2 As DataRowView = CType(dgPDFliste.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Dim root, datei, full As String

        root = item2.Row.ItemArray(4).ToString.Trim
        datei = item2.Row.ItemArray(5).ToString.Trim
        full = serverUNC & root & "\" & datei
        full = full.Replace("/", "\")
        OpenDokument(full)
        e.Handled = True
    End Sub




    'Private Sub useMapShare(pDF_PrintRange As clsRange, pDF_druckMassStab As Double, ausrichtung As String, text1 As String, text2 As String)
    '    Dim cgi, params As String
    '    cgi = myglobalz.serverUNC & "inetpub\scripts\shp2img6\shp2img6.exe "
    '    'cgi = cgi & " -m \\w2gis02\gdvell\websys\mapfiles\cache\feinen_j.map "
    '    cgi = cgi & mapfileBILD
    '    cgi = cgi & " -e 480445 5541797 481229 5542316 -s 1658 1097  "
    '    cgi = cgi & " -o \\w2gis02\gdvell\cache\gis\feinen_j_lubtest.png  "
    '    cgi = cgi & " -dpi 72  -i PNG  -opa 0 -trans 0  "
    '    Process.Start(cgi)

    '    'Dim temprange As New clsRange
    '    'temprange = bildePDFRange(modus, pDF_PrintRange, temprange)
    'End Sub

    Private Sub quer_Checked(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        pdfrahmenNeuPLatzieren("quer")
        PDF_druckMassStab = PDF_postition_desRahmensBestimmen()
        tbMasstabDruck.Text = CInt(PDF_druckMassStab).ToString
        tbPDF_Bemerkung.Text = getPDFBemerkung()
        tbPDF_Ort.Text = getPDFOrt(kartengen.aktMap.aktrange)
        'btnMakePDFohnemass.IsEnabled = True
        e.Handled = True
    End Sub
    Private Sub hoch_Checked(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        pdfrahmenNeuPLatzieren("hoch")
        PDF_druckMassStab = PDF_postition_desRahmensBestimmen()
        tbMasstabDruck.Text = CInt(PDF_druckMassStab).ToString
        tbPDF_Bemerkung.Text = getPDFBemerkung()
        tbPDF_Ort.Text = getPDFOrt(kartengen.aktMap.aktrange)
        'btnMakePDFohnemass.IsEnabled = False
        e.Handled = True
    End Sub

    Private Sub pdfrahmenNeuPLatzieren(hochQuerModus As String)
        Dim newtopLeftPoint, alterMittelPunkt As New myPoint
        Try
            alterMittelPunkt = getAltermittelpunkt(auswahlRechteck, cv1) 'muss aufgerufen werden BEVOR die Form geändert wird
            cvPDFrechteck.Children.Clear()
            setAuswahlRechteckProps(hochQuerModus)
            cvPDFrechteck.Children.Add(auswahlRechteck)

            newtopLeftPoint = createPDF.calcPDFrahmenPositionInPixel(auswahlRechteck, alterMittelPunkt)

            cvPDFrechteck.SetLeft(auswahlRechteck, CInt(newtopLeftPoint.X))
            cvPDFrechteck.SetTop(auswahlRechteck, CInt(newtopLeftPoint.Y))
        Catch ex As Exception
            l("fehler in pdfrahmenNeuPLatzieren" & ex.ToString)
        End Try
    End Sub

    Private Sub setAuswahlRechteckProps(hochQuerModus As String)
        auswahlRechteck = New Rectangle
        auswahlRechteck.Stroke = Brushes.Black
        auswahlRechteck.StrokeThickness = 2
        auswahlRechteck.Name = "herrmann"
        auswahlRechteck.Fill = Brushes.Transparent
        auswahlRechteck.HorizontalAlignment = HorizontalAlignment.Left
        auswahlRechteck.VerticalAlignment = VerticalAlignment.Center
        If rbFormatA4.IsChecked Then
            setMyPdfRectA4(hochQuerModus)
        End If
        If Not rbFormatA4.IsChecked Then ' A3
            setMyPdfRectA3(hochQuerModus)
        End If
    End Sub

    Private Shared Sub setMyPdfRectA3(hochQuerModus As String)
        If hochQuerModus = "quer" Then
            auswahlRechteck.Width = 700.5 '350 * 1,414285714 = 495
            auswahlRechteck.Height = auswahlRechteck.Width * 0.707070707 'a4
            'myRect.Height = myRect.Width * 0.661921708
            'myRect.Height = myRect.Width * 0.706650831 'basierend auf 842,595 
        Else
            auswahlRechteck.Width = 495
            auswahlRechteck.Height = auswahlRechteck.Width * 1.414285714 'a4
            'myRect.Height = myRect.Width * 1.510752688
            'myRect.Height = myRect.Width * 1.41512605 'basierend auf 842,595
        End If
    End Sub

    Private Shared Sub setMyPdfRectA4(modus As String)
        If modus = "quer" Then
            auswahlRechteck.Width = 495 '350 * 1,414285714 = 495
            auswahlRechteck.Height = auswahlRechteck.Width * 0.707070707 'a4
            'myRect.Height = myRect.Width * 0.661921708
            'myRect.Height = myRect.Width * 0.706650831 'basierend auf 842,595 
        Else
            auswahlRechteck.Width = 350
            auswahlRechteck.Height = auswahlRechteck.Width * 1.414285714 'a4
            'myRect.Height = myRect.Width * 1.510752688
            'myRect.Height = myRect.Width * 1.41512605 'basierend auf 842,595
        End If
    End Sub

    Private Function getAltermittelpunkt(myPDFRect As Rectangle, derCanvas As Canvas) As myPoint
        Dim aleft, btop As Double
        Dim temp As New myPoint
        Try
            l("getAltermittelpunkt---------------------- anfang")
            If derCanvas Is Nothing Then
                l("fehler derCanvas is nothign " & derCanvas.ToString)
                Return Nothing
            End If

            l("getAltermittelpunkt---------------------- anfang2")
            If myPDFRect IsNot Nothing Then
                l("myPDFRect isnot nothing")
                If Double.IsNaN(Canvas.GetLeft(myPDFRect)) Then
                    l("getAltermittelpunkt 1")
                    temp.X = derCanvas.Width / 2
                    temp.Y = derCanvas.Height / 2
                Else
                    l("getAltermittelpunkt 2")
                    aleft = Canvas.GetLeft(myPDFRect)
                    btop = Canvas.GetTop(myPDFRect) '- 21
                    temp.X = aleft + (myPDFRect.Width / 2)
                    temp.Y = btop + (myPDFRect.Height / 2)
                End If
            Else
                l("warnung myPDFRect is nothing, liegt der rahmen ausserhalb des bildschirms?")
                temp.X = derCanvas.Width / 2
                temp.Y = derCanvas.Height / 2
            End If
            l("getAltermittelpunkt---------------------- ende")
            Return temp
        Catch ex As Exception
            l("Fehler in getAltermittelpunkt: " & ex.ToString())
            temp.X = derCanvas.Width / 2
            temp.Y = derCanvas.Height / 2
            Return temp
        End Try
    End Function

    Private Sub rbAktiveEbene_Checked(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        panningAusschalten()

        If rbHgrundAktiveEbene.IsChecked Then
            'alteaktive ebvene deaktivieren
            modLayer.alteAktiveEbeneDeaktivieren(layersSelected)
            layerActive.aid = layerHgrund.aid
            layerHgrund.isactive = True
            pgisTools.getStamm4aid(layerActive)
            layerActive.mithaken = True
            layerActive.isactive = True
            layerActive.RBischecked = True
            showLayersliste()
        Else
            layerActive.aid = 0
            layerHgrund.isactive = False
            layerHgrund.RBischecked = False
        End If
        refreshMap(False, True)
        e.Handled = True
    End Sub
    Private Sub tbhgrund_MouseDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        panningAusschalten()
        Dim nck As TextBlock = CType(sender, TextBlock)
        'MsgBox("aid text : " & CStr(nck.Tag))
        aktaid = CInt(nck.Tag)
        Dim sid = CInt(nck.Uid)
        zeigeLegendeUndDoku(aktaid, sid, False)
    End Sub

    Private Sub cmbMasstabDruck_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim item As clsMasstab = CType(cmbMasstabDruck.SelectedItem, clsMasstab)

        If item Is Nothing Then
            e.Handled = True
            Exit Sub
        End If
        Dim MasstabAusgewaehlt As String
        Dim rectWidthInPixel, rectHoheInPixel As Double
        initDruckMasstabCombo(CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked), cv1.Width, cv1.Height)
        cmbMasstabDruck.ItemsSource = druckMasstaebe

        MasstabAusgewaehlt = calcNewScreenScale(item.tagVal, rectWidthInPixel, rectHoheInPixel,
                                  CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked),
                                  cv1.Width, cv1.Height)
        If masstabtools.rectIstZuGross(rectWidthInPixel, rectHoheInPixel, cv1.Width, cv1.Height, 25) Then

            tbPdfMasstabserror.Text =
                "WICHTIG: Massstab ist zu klein für diesen Ausschnitt. Bitte: " &
                " 1. Verlassen Sie die PDF-Druckfunktion" &
                " 2. vergrößern Sie den Ausschnitt und " &
                " 3. wiederholen Sie die Auswahl"
        Else
            tbMasstabDruck.Text = MasstabAusgewaehlt
            PDF_druckMassStab = PDF_postition_desRahmensBestimmen()
            auswahlRechteck.Width = rectWidthInPixel
            auswahlRechteck.Height = rectHoheInPixel
        End If

        e.Handled = True
    End Sub



    Private Sub btnFlstsuchen_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        flurstueckssuche()
        cbSOeinschalten.IsChecked = True
        myglobalz.mgisBackModus = False
        'imgpin.Visibility = Visibility.Visible
        e.Handled = True
    End Sub

    Private Sub btnAdressesuchen_Click(sender As Object, e As RoutedEventArgs)

        myglobalz.mgisBackModus = False
        panningAusschalten()
        adresssuche()
        cbSOeinschalten.IsChecked = True
        imgpin.Visibility = Visibility.Visible
        e.Handled = True
    End Sub

    Private Sub btnKoordinatesuchen_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        myglobalz.mgisBackModus = False
        utmKoordinate()
        ' imgpin.Visibility = Visibility.Visible
        e.Handled = True
    End Sub

    'Private Sub cmbSuchezwei_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    '    If Not ladevorgangAbgeschlossen Then Exit Sub
    '    Dim item As ComboBoxItem = CType(cmbSuchezwei.SelectedItem, ComboBoxItem)
    '    If item.Tag.ToString = "s" Then Exit Sub
    '    e.Handled = True
    '    ortesuchen(item.Tag.ToString)
    '    imgpin.Visibility = Visibility.Visible
    '    e.Handled = True
    'End Sub

    'Private Sub ortesuchen(tag As String)
    '    Try
    '        Select Case tag
    '            Case "gemeinden"
    '                MessageBox.Show("Baustelle")
    '            Case "ot"
    '                MessageBox.Show("Baustelle")
    '            Case "hll"
    '                MessageBox.Show("Baustelle")
    '            Case "koord"
    '                utmKoordinate()
    '        End Select
    '    Catch ex As Exception
    '        l("fehler in ortesuchen" & ex.ToString)

    '    End Try
    'End Sub

    Private Sub cmbSuchezwei_SelectionChanged_1(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnOptionen_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        Dim optmen As New winOption
        optmen.ShowDialog()
        e.Handled = True
    End Sub

    Private Sub tbVorgangsid_TextChanged(sender As Object, e As TextChangedEventArgs)
        aktvorgangsid = tbVorgangsid.Text
        e.Handled = True
    End Sub

    Private Sub btnObjektsuche_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        zeigeObjektsuche("")
        e.Handled = True
    End Sub

    Private Sub zeigeObjektsuche(titel As String)
        'stContext.Width = cv1.Width '- 100
        'stContext.Height = cv1.Height '- 100
        stpObjektsuche.Width = stContext.Width - stpKnoeppeVertical.Width - 50 '50=margins
        stContext.Visibility = Visibility.Visible
        sichtbarMachenObjektsuche()
        'schema und Tabelle holen
        'sid und aktaid exitsierren hier bereits
        os_tabelledef = New clsTabellenDef
        Debug.Print("")
        btnObjektsuche.ToolTip = "Objektsuche verfügbar"
        btnObjektsuche.Visibility = Visibility.Collapsed
        os_tabelledef.aid = CStr(aktaid)
        os_tabelledef.gid = "0"
        os_tabelledef.datenbank = "postgis20"
        os_tabelledef.tab_nr = CType(1, String)
        sachdatenTools.getSChema(os_tabelledef)

        korrigiereTabellenSchemaFallsEintraegeFalsch(os_tabelledef)
        tbHinweisObjektsuche.Text = "Objektsuche_" & titel
        refreshOS("", "")
    End Sub

    Private Sub refreshOS(os_kat As String, os_feld As String)
        'Dim os_kat As String = ""
        'Dim os_feld As String = ""
        'os_feld = "1050"
        If os_kat.StartsWith("___") Then os_kat = ""
        Dim hinweis As String = ""
        Dim volltextsucheSQL As String = ""
        'basisrec.mydb = CType(fstREC.mydb.Clone, clsDatenbankZugriff)

        Dim katstring() As String = initOSComboBoxArray(os_tabelledef.Schema, os_tabelledef.tabelle)
        cmbOSKat.DataContext = katstring

        If Not os_feld.Trim.IsNothingOrEmpty Then
            volltextsucheSQL = bildeOSVolltextsuche(os_tabelledef.Schema, os_tabelledef.tabelle, os_feld.Trim)
            If Not volltextsucheSQL.IsNothingOrEmpty Then
                volltextsucheSQL = "   " & volltextsucheSQL & " "
            End If
        End If

        OSrec.mydb.SQL = bildeSQLString(os_tabelledef.Schema, os_tabelledef.tabelle, os_kat, os_feld, volltextsucheSQL)
        l(OSrec.mydb.SQL)
        hinweis = OSrec.getDataDT()
        'Dim oslAttrColl As New List(Of String())
        Dim oslIntColl As New List(Of String())
        Dim linearray As String()
        ReDim linearray(20)
        Dim dataanz As Integer
        dataanz = OSrec.dt.Rows.Count
        If OSrec.dt.Rows.Count < 1 Then
            dgObjektsuche.ItemsSource = Nothing
            tbOS_Result.Text = "Für diese Objektart ist keine Objektsuche eingerichtet!"
            dgObjektsuche.Visibility = Visibility.Collapsed
            btnOS2CSV.Visibility = Visibility.Collapsed
        Else
            ' oslAttrColl = bildeOS_arrayColl(basisrec)
            oslIntColl = bildeOSInt_arrayColl(OSrec)
            setFirstColumnsInvisible(8)
            dgObjektsuche.DataContext = OSrec.dt
            basisrec = tools.holeSpaltenKoepfe(basisrec)
            schreibeSpaltenkoepfe(basisrec)
            dgObjektsuche.ItemsSource = oslIntColl
            dgObjektsuche.Visibility = Visibility.Visible

            btnOS2CSV.Visibility = Visibility.Visible
            tbOS_Result.Text = "Für diese Objektart wurden " & dataanz & " Objekte gefunden! " &
                "Klicken Sie ein Objekt an für weitere Aktionen!" & Environment.NewLine &
                "Zum Sortieren klicken Sie auf die Spaltenköpfe"
        End If
    End Sub

    Private Function bildeOSVolltextsuche(schema As String, tabelle As String, suchstring As String) As String
        Dim hinweis, dtyp As String

        'Dim dtyp As New DataColumn
        Dim startspalte As Integer = 9
        Dim icount As Integer = startspalte
        Dim sb As New Text.StringBuilder
        Try
            l("bildeOSVolltextsuche---------------------- anfang")
            OSrec.mydb.SQL = " Select * from  " & schema & ".os_" & tabelle
            hinweis = OSrec.getDataDT()
            suchstring = suchstring.Trim.ToLower
            l("genCSV4DT---------------------- anfang")
            For j = startspalte To OSrec.dt.Columns.Count - 1
                dtyp = (OSrec.dt.Columns(j).DataType.ToString)
                icount += 1
                If dtyp = "System.String" Then
                    sb.Append(" lower(" & clsDBtools.fieldvalue(OSrec.dt.Columns(j).ColumnName).Trim & ") like '%" & suchstring & "%'  or ")
                End If
            Next
            Dim rest As String = sb.ToString
            rest = clsString.ReplaceLastOccurrence(rest, " or ", " ")
            Return rest
            l("bildeOSVolltextsuche---------------------- ende")
        Catch ex As Exception
            l("Fehler inbildeOSVolltextsuche : " & ex.ToString())
            Return ""
        End Try
    End Function


    Private Function initOSComboBoxArray(schema As String, tabelle As String) As String()
        Dim hinweis As String = ""
        Dim recs(1000) As String
        Try
            l("initOSComboBox---------------------- anfang")
            OSrec.mydb.SQL = " Select distinct okategorie from  " & schema & ".os_" & tabelle & " order by okategorie"
            hinweis = OSrec.getDataDT()
            For i = 0 To OSrec.dt.Rows.Count - 1
                recs(i) = clsDBtools.fieldvalue(OSrec.dt.Rows(i).Item(0)).Trim
            Next
            recs((OSrec.dt.Rows.Count)) = "_________"
            ReDim Preserve recs(OSrec.dt.Rows.Count)
            Return recs
            l("initOSComboBox---------------------- ende")
        Catch ex As Exception
            l("Fehler in initOSComboBox: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Sub setFirstColumnsInvisible(anzahl As Integer)
        For i = 0 To anzahl
            dgObjektsuche.Columns(i).Visibility = Visibility.Collapsed
        Next
    End Sub

    Private Function bildeOSInt_arrayColl(basisrec As clsDBspecPG) As List(Of String())
        Dim oslcoll As New List(Of String())
        Dim linearray() As String
        Try
            l("bildeOS_arrayColl---------------------- anfang")
            For i = 0 To basisrec.dt.Rows.Count - 1
                ReDim linearray(basisrec.dt.Columns.Count - 1)
                For j = 0 To basisrec.dt.Columns.Count - 1
                    linearray(0) = clsDBtools.fieldvalue(basisrec.dt.Rows(i).Item(0)).Trim
                    linearray(j) = clsDBtools.fieldvalue(basisrec.dt.Rows(i).Item(j)).Trim
                Next
                oslcoll.Add(linearray)
            Next
            Return oslcoll
            l("bildeOS_arrayColl---------------------- ende")
        Catch ex As Exception
            l("Fehler in bildeOS_arrayColl2: ", ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function bildeOS_arrayColl(basisrec As clsDBspecPG) As List(Of String())
        Dim oslcoll As New List(Of String())
        Dim linearray() As String
        Try
            l("bildeOS_arrayColl---------------------- anfang")
            For i = 0 To basisrec.dt.Rows.Count - 1
                ReDim linearray(20)
                For j = 9 To basisrec.dt.Columns.Count - 1
                    linearray(0) = clsDBtools.fieldvalue(basisrec.dt.Rows(i).Item(0)).Trim
                    linearray(j - 8) = clsDBtools.fieldvalue(basisrec.dt.Rows(i).Item(j)).Trim
                Next
                oslcoll.Add(linearray)
            Next
            Return oslcoll
            l("bildeOS_arrayColl---------------------- ende")
        Catch ex As Exception
            l("Fehler in bildeOS_arrayColl1: ", ex)
            Return Nothing
        End Try
    End Function

    Private Sub schreibeSpaltenkoepfe(basisrec As clsDBspecPG)
        Try
            l("schreibeSpaltenkoepfe---------------------- anfang")
            For j = 0 To basisrec.dt.Rows.Count - 1
                '  If (j) > (basisrec.dt.Rows.Count - 1) Then Exit For
                dgObjektsuche.Columns(j).Header = clsString.Capitalize(clsDBtools.fieldvalue(basisrec.dt.Rows(j).Item(0)))
            Next
            l("schreibeSpaltenkoepfe---------------------- ende")
        Catch ex As Exception
            l("Fehler in schreibeSpaltenkoepfe: ", ex)
        End Try
    End Sub


    Private Sub dgObjektsuche_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If dgObjektsuche.SelectedItem Is Nothing Then Exit Sub
        Dim item2 As String() = CType(dgObjektsuche.SelectedItem, String())
        If item2 Is Nothing Then Exit Sub
        Dim paradigmaVID, fulllink, pdfspalte, geom As String
        Dim ebenentitel = tbHinweisObjektsuche.Text.Replace("Objektsuche_", "")
        paradigmaVID = getParadigmaVID(item2(6))

        geom = item2(1).ToString.Trim
        fulllink = item2(4).ToString.Trim
        pdfspalte = item2(8)

        l("fulllink " & fulllink)
        If Not splitDBinfo(fulllink) Then
            MsgBox("fehler im Fulllink: " & fulllink)
        End If

        Dim actionwin As New winboxOS(pdfDateiIstVorhanden(pdfspalte), pdfspalte)
        actionwin.ShowDialog()

        Select Case actionwin.aktion
            Case "pdfdateizumobjektladen"
                If pdfDateiIstVorhanden(pdfspalte) Then
                    Dim _pdfDatei = serverUNC & pdfspalte 'pdfspalte muss den fullname enthalten
                    _pdfDatei = _pdfDatei.Replace("/", "\")
                    OpenDokument(_pdfDatei)
                End If

            Case "dbabfrage"
                'If (item2(7).ToString.Trim.IsNothingOrEmpty Or item2(7).ToString.StartsWith("dummy")) Then
                'If (geom.IsNothingOrEmpty) Then
                '    'als standard wird der geometrielink verwendet
                'Else
                '    'ist ein fulllink für die DB vorhanden? wenn ja verwenden
                If splitDBinfo(fulllink) Then
                    os_tabelledef.tabelle = getTabname4tabnr(aktaid, "1")
                    korrigiereTabellenSchemaFallsEintraegeFalsch(os_tabelledef)
                    If Not os_tabelledef.tabelle.ToLower.StartsWith("os_") Then
                        os_tabelledef.tab_nr = getTabnr4Tabname(os_tabelledef.Schema, os_tabelledef.tabelle)
                    End If
                    If os_tabelledef.os_tabellen_name.IsNothingOrEmpty OrElse (Not os_tabelledef.os_tabellen_name.ToLower.StartsWith("os_")) Then
                        os_tabelledef.os_tabellen_name = "os_" & os_tabelledef.tabelle.Replace("os_", "")
                    End If
                    If Not os_tabelledef.linkTabs.IsNothingOrEmpty Then
                        os_tabelledef.gid = getGID4OS_tabelle(os_tabelledef)
                    End If
                Else
                    os_tabelledef.tabelle = getTabname4tabnr(aktaid, "1")
                    korrigiereTabellenSchemaFallsEintraegeFalsch(os_tabelledef)
                    If Not os_tabelledef.tabelle.ToLower.StartsWith("os_") Then
                        os_tabelledef.tab_nr = getTabnr4Tabname(os_tabelledef.Schema, os_tabelledef.tabelle)
                    End If
                    If os_tabelledef.os_tabellen_name.IsNothingOrEmpty OrElse (Not os_tabelledef.os_tabellen_name.ToLower.StartsWith("os_")) Then
                        os_tabelledef.os_tabellen_name = "os_" & os_tabelledef.tabelle.Replace("os_", "")
                    End If
                    If Not os_tabelledef.linkTabs.IsNothingOrEmpty Then
                        os_tabelledef.gid = getGID4OS_tabelle(os_tabelledef)
                    End If
                    l("fehler in flullinkdb")
                    MsgBox("fehler im Fulllinkdb: " & fulllink)
                End If
                modOStools.os_dbanzeigen(paradigmaVID, ebenentitel)
            Case "zurkarte"
                tabellenErmitteln()

                If modOStools.os_zurkarte() Then
                    OSrefresh = True
                    refreshMap(True, True)
                    stContext.Visibility = Visibility.Collapsed
                    cbSOeinschalten.IsChecked = True
                End If
            Case "zuparadigmavorgangsid"
                If IsNumeric(paradigmaVID) AndAlso CInt(paradigmaVID) > 0 Then
                    'MsgBox("Bitte schließen Sie einen evtl. an)
                    tools.paradigmavorgangaufrufen(paradigmaVID)
                Else
                    MsgBox("ungültige Vorgangsid: " & paradigmaVID)
                End If
            Case "zuparadigmahinzufuegen"
                Debug.Print("zuparadigmahinzufuegen" & aktvorgangsid)
                If STARTUP_mgismodus = "paradigma" AndAlso IsNumeric(aktvorgangsid) Then
                    tabellenErmitteln()
                    Dim acanvas As New clsRange
                    Dim puffer_area As Double
                    Dim puffererzeugt As Boolean
                    l("os_zurkarte---------------------- anfang")
                    l("os_tabelledef tabelle" & os_tabelledef.tabelle)
                    l("os_tabelledef aid" & os_tabelledef.aid)
                    l("os_tabelledef gid" & os_tabelledef.gid)
                    l("os_tabelledef Schema" & os_tabelledef.Schema)
                    If Not os_tabelledef.tabelle.ToLower.StartsWith("os_") Then
                        os_tabelledef.tabelle = "os_" & os_tabelledef.tabelle
                    End If
                    puffererzeugt = modEW.bildePufferFuerPolygon(aktPolygon, 0.001, os_tabelledef, puffer_area, acanvas, True)
                    If puffererzeugt Then
                        'tools.geometieNachParadigmaUebernehmen(aktvorgangsid, aktPolygon)
                        If modParadigma.GeometrieNachParadigma(aktPolygon, aktPolyline) Then
                            clsToolsAllg.userlayerNeuErzeugen(GisUser.username, myglobalz.aktvorgangsid)
                            MsgBox("Das Objekt wurde in die Paradigma-DB als Raumbezug übernommen. " & Environment.NewLine &
                                   "Drücken Sie oben die RefreshTaste um die Änderung anzuzeigen!", MsgBoxStyle.OkOnly, "Datenübernahme OK")
                        Else
                            MsgBox("Datenübernahme war nicht erfolgreich. Bitte beim Admin melden!")
                        End If
                    End If
                Else
                    MsgBox("Kein Paradigma-Vorgang ausgewählt. Abbruch.")
                End If
            Case Else
                ' MsgBox("Baustelle")
        End Select
        actionwin = Nothing
        dgObjektsuche.SelectedItem = Nothing
        GC.Collect()
        e.Handled = True
    End Sub

    Private Sub tabellenErmitteln()
        If Not os_tabelledef.tabelle.ToLower.StartsWith("os_") Then
            os_tabelledef.tab_nr = getTabnr4Tabname(os_tabelledef.Schema, os_tabelledef.tabelle)
        End If
        stContext.Visibility = Visibility.Collapsed
        If os_tabelledef.os_tabellen_name.IsNothingOrEmpty OrElse (Not os_tabelledef.os_tabellen_name.ToLower.StartsWith("os_")) Then
            os_tabelledef.os_tabellen_name = "os_" & os_tabelledef.tabelle.Replace("os_", "")
        End If
    End Sub

    Private Function getParadigmaVID(v As String) As String
        Try
            l("getParadigmaVID---------------------- anfang")

            Return v
            l("getParadigmaVID---------------------- ende")
        Catch ex As Exception
            l("Fehler in getParadigmaVID : " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Function pdfDateiIstVorhanden(dateiname As String) As Boolean
        If dateiname.IsNothingOrEmpty OrElse dateiname.ToLower.StartsWith("dummy") Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Function getGID4OS_tabelle(os_tabelledef As clsTabellenDef) As String
        Dim tabelle As String = ""
        Dim sql As String
        Dim dt As DataTable
        Try
            l("getGID4OS_tabelle---------------------- anfang")
            sql = "select gid from " & os_tabelledef.Schema & "." & os_tabelledef.tabelle & " where " & os_tabelledef.linkTabs & "=" & os_tabelledef.gid
            l(sql)
            dt = getDTFromWebgisDB(sql, "postgis20")
            tabelle = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            Return tabelle
            l("getGID4OS_tabelle---------------------- ende")
        Catch ex As Exception
            l("Fehler in getGID4OS_tabelle: " & sql & Environment.NewLine, ex)
            Return "-1"
        End Try
    End Function

    Private Function getTabname4tabnr(aid As Integer, tabnr As String) As String
        'Dim hinweis As String
        Dim tabelle As String
        Try
            l("getTabname4tabnr---------------------- anfang")
            Dim dt As DataTable
            dt = getDTFromWebgisDB("select tabelle from public.attributtabellen where aid=" & aid & " and tab_nr=" & tabnr & "", "webgiscontrol")
            tabelle = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            dt = Nothing
            Return tabelle
            l("getTabname4tabnr---------------------- ende")
        Catch ex As Exception
            l("Fehler in getTabname4tabnr: ", ex)
            Return "-1"
        End Try
    End Function

    Private Function getTabnr4Tabname(schema As String, tabelle As String) As String
        Dim hinweis As String
        Dim tabnr As String
        Try
            l("getTabnr4Tabname---------------------- anfang")
            Dim dt As DataTable
            dt = getDTFromWebgisDB("select tab_nr from public.attributtabellen where schema='" & schema & "' and tabelle='" & tabelle & "'", "webgiscontrol")
            tabnr = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            dt = Nothing
            Return tabnr
            l("getTabnr4Tabname---------------------- ende")
        Catch ex As Exception
            l("warnung in getTabnr4Tabname: ", ex)
            Return "1"
        End Try
    End Function

    Private Shared Function splitDBinfo(fl As String) As Boolean
        Dim fulllink As String
        Dim a() As String
        Try
            l("splitDBinfo---------------------- anfang")
            fulllink = fl
            a = fulllink.Split(","c)
            os_tabelledef.Schema = a(0)
            os_tabelledef.tabelle = a(1)
            os_tabelledef.gid = a(2)

            Return True
            l("splitDBinfo---------------------- ende")
        Catch ex As Exception
            l("Fehler in : splitDBinfo ", ex)
            Return False
        End Try
    End Function

    Private Sub btncloseOS_Click(sender As Object, e As RoutedEventArgs)
        stContext.Visibility = Visibility.Collapsed
        e.Handled = True
    End Sub

    Private Sub zeigeKreisUebersicht(sender As Object, e As MouseEventArgs)
        panningAusschalten()

        If kreisUebersichtCanvas.Visibility = Visibility.Collapsed Then
            kreisUebersichtCanvas.Visibility = Visibility.Visible
            drawAktRange2Uebersicht(kartengen.aktMap.aktrange)
            dockMap.SetZIndex(kreisUebersichtCanvas, 500)
        Else
            kreisUebersichtCanvas.Visibility = Visibility.Collapsed
            e.Handled = True
            Exit Sub
        End If
        nachricht("USERAKTION: kreisuebersicht messen ")
        panningAusschalten()
        'CanvasClickModus = "kreisuebersicht"
        e.Handled = True
    End Sub

    Private Sub kreisuebersichtMousedown(sender As Object, e As MouseButtonEventArgs)
        Mouse.Capture(Nothing)
        KoordinateKLickpt = e.GetPosition(imgkreisuebersicht)
        CanvasClickModus = ""
        If tools.liegtImkreisOffenbach(KoordinateKLickpt) Then
            Dim neupunktString As String
            neupunktString = KreisUebersichtkoordinateKlickBerechnen(KoordinateKLickpt)
            splitKoordinatenstring(neupunktString)
            kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, 1500)
            setBoundingRefresh(kartengen.aktMap.aktrange)
            refreshMap(True, True)
        End If

        kreisUebersichtUnsichtbar()
        e.Handled = True
    End Sub

    Sub kreisUebersichtUnsichtbar()
        kreisUebersichtCanvas.Visibility = Visibility.Collapsed
        dockMap.SetZIndex(kreisUebersichtCanvas, 0)
    End Sub

    Private Sub imageMapCanvas_MouseEnter(sender As Object, e As MouseEventArgs)
        kreisUebersichtUnsichtbar()
        e.Handled = True
    End Sub

    Private Sub btnFavoSave_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        makeThemenInVis()
        favoTools.FavoritSave(GisUser.username)
        showInfo("Die Zussammenstellung wurde in 'Meine Favoriten' gespeichert")
        e.Handled = True
    End Sub

    Private Sub showInfo(v As String)
        MsgBox(v)
    End Sub

    Private Sub zwischenbildBitteWarten()
        Dim myBitmapImage As New BitmapImage()
        Try
            If slots(0).refresh Then
                If slots(0).layer.titel.ToLower = "kein hintergrund" Then
                    'slots(0).setEmpty()
                    'myBitmapImage = New BitmapImage()
                    'myBitmapImage.BeginInit()
                    'myBitmapImage.UriSource = New Uri("/mgis;component/icons/leer.png", UriKind.RelativeOrAbsolute)
                    'myBitmapImage.EndInit()
                    'slots(0).image.Source = myBitmapImage
                    slots(0).setEmpty()
                Else
                    myBitmapImage = New BitmapImage()
                    myBitmapImage.BeginInit()
                    myBitmapImage.UriSource = New Uri("/mgis;component/icons/bwv.png", UriKind.RelativeOrAbsolute)
                    myBitmapImage.EndInit()
                    slots(0).image.Source = myBitmapImage
                End If

            End If
            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
            If slots(1).refresh Then
                myBitmapImage = New BitmapImage()
                myBitmapImage.BeginInit()
                myBitmapImage.UriSource = New Uri("/mgis;component/icons/bwh.png", UriKind.RelativeOrAbsolute)
                myBitmapImage.EndInit()
                slots(1).image.Source = myBitmapImage
            End If
            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
        Catch ex As Exception
            nachricht("Fehler in : zwischenbild ---------ende-----------------" & ex.ToString)
        End Try
    End Sub

    Private Sub btnMinMax_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If dockMenu.Width > 10 Then
            dockMenu.Width = 0
            refreshMap(True, True)
            'btnMinMax.Content = " &gt;&gt; "
            Exit Sub
        End If
        If dockMenu.Width < 1 Then
            dockMenu.Width = 460
            refreshMap(True, True)
            'btnMinMax.Content = " &lt;&lt; "
            Exit Sub
        End If
    End Sub

    Private Sub dgOSliste_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If dgOSliste.SelectedItem Is Nothing Then Exit Sub
        panningAusschalten()
        myglobalz.mgisBackModus = False
        e.Handled = True
        Dim item As New clsLayerPres
        Try
            item = CType(dgOSliste.SelectedItem, clsLayerPres)
            aktaid = item.aid
            'btnOSdropdown.RaiseEvent(New RoutedEventArgs(Button.ClickEvent, btnOSdropdown))
            btnOSdropdown.IsOpen = False
            dgOSliste.SelectedItem = Nothing
            zeigeObjektsuche("")
            cbSOeinschalten.IsChecked = True
        Catch ex As Exception
            nachricht(ex.ToString)
            Exit Sub
        End Try
        e.Handled = True
    End Sub



    Private Sub btnOSdropdown_Click(sender As Object, e As RoutedEventArgs)
        MsgBox("asdsdasd")
        e.Handled = True
    End Sub

    Private Sub tbOSfilter_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If tbOSfilter.Text.Length < 2 Then Exit Sub
        dgOSliste.DataContext = Nothing
        clsWebgisPGtools.getOSliste(allLayersPres, tbOSfilter.Text.ToLower)
        dgOSliste.DataContext = allOSLayers
        e.Handled = True
    End Sub


    Private Sub btnPNG_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        Dim ausrichtung As String
        ausrichtung = "quer"
        PDF_PrintRange.xl = kartengen.aktMap.aktrange.xl
        PDF_PrintRange.xh = kartengen.aktMap.aktrange.xh
        PDF_PrintRange.yl = kartengen.aktMap.aktrange.yl
        PDF_PrintRange.yh = kartengen.aktMap.aktrange.yh
        Dim hochaufloesend As Boolean = False
        Dim ausgabedatei As String = ""
        'makeandloadPDF("842", "595", "mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, True, hochaufloesend)
        makeandloadPDF("mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, True, hochaufloesend,
                       CBool(rbFormatA4.IsChecked), False, ausgabedatei)
        opendirec(ausgabedatei)
        e.Handled = True
    End Sub

    Private Shared Sub opendirec(ausgabedatei As String)
        Dim direc As String
        Try
            l("opendirec---------------------- anfang")
            l("ausgabedatei " & ausgabedatei)
            Dim fi As New IO.FileInfo(ausgabedatei)
            Process.Start(fi.DirectoryName)
            l("opendirec---------------------- ende")
        Catch ex As Exception
            l("Fehler in opendirec: " & ausgabedatei & "///" & ex.ToString())
        End Try
    End Sub

    Private Sub btnNaturSG_Click(sender As Object, e As RoutedEventArgs)
        LastThemenSuche = "hauptsachgebiet"
        dgErgebnis.ItemsSource = Nothing
        layersTemp = modLayer.getLayer4sachgebiet("schutzgebiete")
        layersTemp.Sort()
        clsWebgisPGtools.dombineLayerDoku(layersTemp, allDokus)
        dgErgebnis.ItemsSource = layersTemp
        e.Handled = True
    End Sub

    Private Sub btnTools_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        Dim optmen As New winEigentuemer4Polygon
        optmen.ShowDialog()
        refreshMap(True, True)
        e.Handled = True
    End Sub
    Private Sub zindexeSetzen()
#Disable Warning BC42025 ' Access of shared member, constant member, enum member or nested type through an instance
        tigis.SetZIndex(dockMap, 200) ' wg panning nur 200

        tigis.SetZIndex(dockMenu, 300)
        tigis.SetZIndex(dockTop, 300)

        dockMap.SetZIndex(cv0, 5)
        dockMap.SetZIndex(HGabdecker, 6)
        dockMap.SetZIndex(OSmapCanvas, 8)

        dockMap.SetZIndex(cv1, 11)
        dockMap.SetZIndex(cv2, 12)
        dockMap.SetZIndex(cv3, 13)
        dockMap.SetZIndex(cv4, 14)
        dockMap.SetZIndex(cv5, 15)
        dockMap.SetZIndex(cv6, 16)
        dockMap.SetZIndex(cv7, 17)
        dockMap.SetZIndex(cv8, 18)
        dockMap.SetZIndex(cv9, 19)
        dockMap.SetZIndex(cv10, 20)
        dockMap.SetZIndex(cv11, 21)
        dockMap.SetZIndex(cv12, 22)
        dockMap.SetZIndex(cv13, 23)
        dockMap.SetZIndex(cv14, 24)
        dockMap.SetZIndex(cv15, 25)
        dockMap.SetZIndex(cv16, 26)
        dockMap.SetZIndex(cv17, 27)
        dockMap.SetZIndex(cv18, 28)
        dockMap.SetZIndex(cv19, 29)
        dockMap.SetZIndex(cv20, 30)

        dockMap.SetZIndex(imageMapCanvas, 100)
        dockMap.SetZIndex(imgpin, 110)
        dockMap.SetZIndex(stckBalken, 110)
        dockMap.SetZIndex(suchCanvas, 250)
        dockMap.SetZIndex(stwinthemen, 500)
        dockMap.SetZIndex(cvPDFrechteck, 400)
        dockMap.SetZIndex(stContext, 500)

        imageMapCanvas.SetZIndex(imgKarte, 100) 'macht keinen sinn 

        dockMenu.SetZIndex(stPDFDruck, 300)
        dockMenu.SetZIndex(stMenu, 300)
#Enable Warning BC42025 ' Access of shared member, constant member, enum member or nested type through an instance
    End Sub

    Private Sub sldHgrundOpac_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
        setHGabdecker4SliderValue(CInt(sldHgrundOpac.Value))
        'HGcanvasImageRange0.Opacity = CInt(sldHgrundOpac.Value)
        'HGcanvasImageRange0. = CInt(sldHgrundOpac.Value)
        e.Handled = True
    End Sub

    Private Sub setHGabdecker4SliderValue(val As Integer)
        Dim abdeckfarbe As New SolidColorBrush
        abdeckfarbe = New SolidColorBrush(Color.FromArgb(CByte(val), 255, 255, 255)) ' 
        HGabdecker.Background = abdeckfarbe
        abdeckfarbe = Nothing
    End Sub

    'Private Sub cmbHgrund_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
    '    btnHGSlider.IsOpen = True
    '    e.Handled = True
    'End Sub

    Private Sub btnSliderSchliessen_Click(sender As Object, e As RoutedEventArgs)
        schliesseSliderDialog()
        e.Handled = True
    End Sub

    Private Sub schliesseSliderDialog()
        btnHGSlider.IsOpen = False
    End Sub

    Private Sub sldVGrundOpac_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
        If Not ladevorgangAbgeschlossen Then Exit Sub
        '' setHGabdecker4SliderValue(CInt(sldHgrundOpac.Value))
        cv1.Opacity = CDbl(sldVGrundOpac.Value)
        slots(1).image.Opacity = CDbl(sldVGrundOpac.Value)
        tbInfopanel.Text = CType(CDbl(sldVGrundOpac.Value), String)
        'e.Handled = True
    End Sub

    Private Sub rbchkMitMasstab_Checked(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        initDruckMasstabCombo(CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked), cv1.Width, cv1.Height)
        cmbMasstabDruck.ItemsSource = druckMasstaebe
        If rbMitMasstab.IsChecked Then
            spdruckmasstab.Visibility = Visibility.Visible
            spAusrichtung.Visibility = Visibility.Visible
            If quer.IsChecked Then
                pdfrahmenNeuPLatzieren("quer")
            Else
                pdfrahmenNeuPLatzieren("hoch")
            End If
            cvPDFrechteck.Visibility = Visibility.Visible
            PDF_druckMassStab = PDF_postition_desRahmensBestimmen()
            tbMasstabDruck.Text = CInt(PDF_druckMassStab).ToString
        Else
            cvPDFrechteck.Visibility = Visibility.Collapsed
            spdruckmasstab.Visibility = Visibility.Hidden
            spAusrichtung.Visibility = Visibility.Hidden
        End If
        e.Handled = True
    End Sub


    Private Sub btnMakePdfMitMasstab_Click(sender As Object, e As RoutedEventArgs)
        Dim ausgabedatei As String = ""
        PDFmitmasstab(ausgabedatei)
        e.Handled = True
    End Sub

    Private Sub PDFmitmasstab(ByRef ausgabedatei As String)
        Dim ausrichtung As String
        Dim hochaufloesend As Boolean
        PDF_druckMassStab = PDF_postition_desRahmensBestimmen()
        ausrichtung = getAusrichtung()
        If cbhochaufloesend.IsChecked Then
            hochaufloesend = True
        Else
            hochaufloesend = False
        End If
        'makeandloadPDF("842", "595", "mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, False, hochaufloesend)
        makeandloadPDF("mitmasstab", PDF_PrintRange, CDbl(tbMasstabDruck.Text), ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, False,
                       hochaufloesend, CBool(rbFormatA4.IsChecked), False, ausgabedatei)
    End Sub

    Private Sub btnMakePDFohnemass_Click(sender As Object, e As RoutedEventArgs)
        Dim ausgabedatei As String = ""
        PDFohneMasstab(False, ausgabedatei)
        e.Handled = True
    End Sub

    Private Sub PDFohneMasstab(schnelldruck As Boolean, ByRef ausgabedatei As String)
        Dim ausrichtung As String = "quer"

        PDF_PrintRange.xl = kartengen.aktMap.aktrange.xl
        PDF_PrintRange.xh = kartengen.aktMap.aktrange.xh
        PDF_PrintRange.yl = kartengen.aktMap.aktrange.yl
        PDF_PrintRange.yh = kartengen.aktMap.aktrange.yh
        Dim hochaufloesend As Boolean = False
        If cbhochaufloesend.IsChecked Then
            hochaufloesend = True
        Else
            hochaufloesend = False
        End If
        ausrichtung = getAusrichtung()
        PDF_druckMassStab = 0
        'makeandloadPDF("842", "595", "mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, False, hochaufloesend)
        makeandloadPDF("ohnemasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text,
                       False, hochaufloesend, CBool(rbFormatA4.IsChecked), schnelldruck, ausgabedatei)
    End Sub

    Private Sub btnPrintPdf_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim ausgabedatei As String = ""
        If rbOhneMasstab.IsChecked Then
            spdruckmasstab.Visibility = Visibility.Hidden
            spAusrichtung.Visibility = Visibility.Hidden
            PDFohneMasstab(False, ausgabedatei)
        Else
            spdruckmasstab.Visibility = Visibility.Visible
            spAusrichtung.Visibility = Visibility.Visible
            PDFmitmasstab(ausgabedatei)
        End If
        If cbPDFnachParadigma.IsChecked Then
            If IsNumeric(aktvorgangsid) Then
                If modParadigma.DokNachParadigma(ausgabedatei, aktvorgangsid, tbPDFnachParadigmaTitel.Text) Then
                    MsgBox("Die Übernahme des Dokumentes nach Paradigma war erfolgreich!")
                Else
                    MsgBox("Die Übernahme des Dokumentes nach Paradigma war NICHT erfolgreich!")
                End If
            End If
        End If
        closePDFDialog()
        disableMyStackpanel(spButtonMenu, True)
    End Sub




    Private Sub txtitel_MouseDown(sender As Object, e As MouseButtonEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim nck As TextBlock = CType(sender, TextBlock)
        stContext.Visibility = Visibility.Collapsed
        panningAusschalten()
        'MsgBox("aid text : " & CStr(nck.Tag))
        'Dim myfontstyle As New FontStyle
        'myfontstyle = CType(sender, FontStyle)
        aktaid = CInt(nck.Tag)
        aktsid = CInt(nck.Uid)
        For Each lay As clsLayerPres In layersSelected
            If lay.aid = aktaid Then
                If lay.mit_objekten Then
                    Dim titel = DirectCast(sender, System.Windows.Controls.TextBlock).[Text]
                    zeigeObjektsuche(titel)
                Else
                    zeigeLegendeUndDoku(aktaid, aktsid, lay.mit_objekten)
                End If
            End If
        Next
        e.Handled = True
    End Sub

    Private Sub btnHandbuch_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        'Dim aaa As New winHandbuch obsolet
        'aaa.ShowDialog()
        OpenDokument("c:\ptest\mgis\gisguide.docx")
        e.Handled = True
    End Sub

    Private Sub btnGruppeFavo_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        makeThemenInVis()

        Select Case GisUser.favogruppekurz
            Case "unb", "ba", "immi", "ille", "kats", "soziales", "intranet", "gebw"
                handleFavorite(GisUser.favogruppekurz, "fix")
            Case Else
                handleFavorite("intranet", "fix")
        End Select
        e.Handled = True
    End Sub
    Private Sub btnMeineFavo_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        makeThemenInVis()
        handleFavorite("meine", "fix")
        e.Handled = True
    End Sub

    Private Sub TreeView_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        'Dim tree As TreeView = CType(sender, TreeView)

        e.Handled = True
        If tv1.SelectedItem Is Nothing Then Exit Sub
        Dim tv As New TreeViewItem
        tv = CType(tv1.SelectedItem, TreeViewItem)
        If tv.Tag Is Nothing Then Exit Sub
        tbKategorie.Text = "Kategorie: " & tv.Header.ToString
        treeview2Kat(tv.Tag.ToString.ToLower)
        tbebenenauswahlinfo.Visibility = Visibility.Visible

    End Sub

    Sub treeview2Kat(tag As String)
        LastThemenSuche = "hauptsachgebiet"
        dgErgebnis.ItemsSource = Nothing
        layersTemp = modLayer.getLayer4sachgebiet(tag)
        layersTemp.Sort()
        clsWebgisPGtools.dombineLayerDoku(layersTemp, allDokus)
        dgErgebnis.ItemsSource = layersTemp
    End Sub

    Private Sub rbFormatA3_Click(sender As Object, e As RoutedEventArgs)
        If quer.IsChecked Then
            pdfrahmenNeuPLatzieren("quer")
        Else
            pdfrahmenNeuPLatzieren("hoch")
        End If
        e.Handled = True
    End Sub

    Private Sub btnSchnelldruck_Click(sender As Object, e As RoutedEventArgs)
        Dim ausgabedatei As String = ""
        PDFohneMasstab(True, ausgabedatei)
        e.Handled = True
    End Sub

    Private Sub btnExplorer_Click(sender As Object, e As RoutedEventArgs)
        holeExplorer()
        e.Handled = True
    End Sub

    Private Sub holeExplorer()
        panningAusschalten()
        If stwinthemen.Visibility = Visibility.Visible Then
            makeThemenInVis()
        Else
            makeThemenVis()
            treeview2Kat("Grenzen".ToLower)
            tbebenenauswahlinfo.Visibility = Visibility.Visible
            tbKategorie.Text = "Kategorie: Grenzen"
        End If
        FocusManager.SetFocusedElement(Me, tbStichwort)
    End Sub

    Private Sub txtitel_MouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim nck As TextBlock = CType(sender, TextBlock)
        stContext.Visibility = Visibility.Collapsed
        panningAusschalten()
        'aktaid = CInt(nck.Tag)
        Dim nlay As New clsLayerPres
        nlay.aid = CInt(nck.Tag)
        pgisTools.getStamm4aid(nlay)
        showFreiLegende4Aid(nlay)
        e.Handled = True
    End Sub

    Private Sub btnBMliste_Click(sender As Object, e As RoutedEventArgs)
        Dim bmliste As New winBM
        bmliste.ShowDialog()
        If bmliste.aktion = "nichts" Then
            'nichts
        End If
        If bmliste.aktion = "bmaktivieren" Then
            If auswahlBookmark IsNot Nothing Then
                panningAusschalten()
                makeThemenInVis()
                aktiviereBM(auswahlBookmark)
            End If
        End If
        e.Handled = True
    End Sub

    Private Sub aktiviereBM(auswahlBookmark As clsBookmark)
        'favoriten aktivieren
        favoritakt.aktiv = auswahlBookmark.fav.aktiv
        favoritakt.gecheckted = auswahlBookmark.fav.gecheckted
        favoritakt.hgrund = auswahlBookmark.fav.hgrund
        favoritakt.titel = auswahlBookmark.fav.titel
        favoritakt.vorhanden = auswahlBookmark.fav.vorhanden
        favoritenUmsetzen()
        'range aktivieren
        If auswahlBookmark.range IsNot Nothing Then
            kartengen.aktMap.aktrange = auswahlBookmark.range
        End If
        refreshMap(True, True)
    End Sub

    Private Sub btnNeuenVorgangAnlegen_Click(sender As Object, e As RoutedEventArgs)
        spVIDParadigma.IsOpen = False
        e.Handled = True
        ' start   C:\ptest\main\paradigma.exe
        starteParadigma()
    End Sub

    Private Sub starteParadigma()
        Dim neuervorgangstgring As String

        Try
            l("starteParadigma---------------------- anfang")
            If paradigmaLaeuftschon() Then
                '  initdb den vordergrundholen
            Else
                neuervorgangstgring = "C:\ptest\main\paradigma.exe"
                Process.Start(neuervorgangstgring, "modus=neu")
            End If

            l("starteParadigma---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub

    Private Function paradigmaLaeuftschon() As Boolean
        Return False
    End Function

    Private Sub dgHistory_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If dgHistory.SelectedItem Is Nothing Then Exit Sub
        Dim item As CLstart.HistoryKookie.HistoryItem
        Try
            item = CType(dgHistory.SelectedItem, CLstart.HistoryKookie.HistoryItem)
        Catch ex As Exception
            e.Handled = True
            Exit Sub
        End Try
        item = CType(dgHistory.SelectedItem, CLstart.HistoryKookie.HistoryItem)
        If item Is Nothing Then
            item = CType(dgHistory.SelectedItem, CLstart.HistoryKookie.HistoryItem)
            If item Is Nothing Then Return
        End If
        paradigmaVorgangImGISaktivMachen(item)
        dgHistory.SelectedItem = Nothing
        e.Handled = True
    End Sub

    Private Sub paradigmaVorgangImGISaktivMachen(item As CLstart.HistoryKookie.HistoryItem)

        If rbVorgangRangeholen.IsChecked Then
            aktvorgangsid = CType(item.ID, String)
            aktvorgang.id = CInt(aktvorgangsid)
            aktvorgang.beschreibung = item.Titel
            aktvorgang.az = item.AZ
            If STARTUP_mgismodus = "vanilla" And GisUser.ADgruppenname.ToLower = "umwelt" Then
                STARTUP_mgismodus = "paradigma"
                Title = clsStartup.getWindowTitel(tbVorgangsid.Text, allLayersPres.Count)
            End If
            showaktvorgangParadigma()
            'ausschnittHolen
            Dim newrange As New clsRange
            newrange = modParadigma.calcNewMaxRange(aktvorgangsid)
            If newrange IsNot Nothing Then
                kartengen.aktMap.aktrange = newrange
                clsToolsAllg.userlayerNeuErzeugen(GisUser.username, myglobalz.aktvorgangsid)
                refreshMap(True, True)
            End If
        End If
        If rbVorgangInParadigmaOeffnen.IsChecked Then
            tools.paradigmavorgangaufrufen(CType(aktvorgangsid, String))
        End If
        spVIDParadigma.IsOpen = False
    End Sub

    Private Sub showaktvorgangParadigma()
        If STARTUP_mgismodus = "paradigma" Then
            clsTooltipps.setTooltipAktvorgang(spVIDParadigma, "alles")
            tbVorgangsid.Text = CType(aktvorgangsid, String)
        End If

    End Sub

    Private Sub btnzuVorgangManuellWechseln_Click(sender As Object, e As RoutedEventArgs)
        If IsNumeric(tbzuVorgangManuellWechseln.Text) Then
            aktvorgangsid = CType(CInt(tbzuVorgangManuellWechseln.Text), String)
            tbVorgangsid.Text = aktvorgangsid
            Dim histit As New CLstart.HistoryKookie.HistoryItem("")
            histit.ID = CInt(aktvorgangsid)
            histit.AZ = ""
            histit.Titel = ""
            paradigmaVorgangImGISaktivMachen(histit)
        End If
        spVIDParadigma.IsOpen = False
        e.Handled = True
    End Sub

    Private Sub btnvorgangabbruch_Click(sender As Object, e As RoutedEventArgs)
        spVIDParadigma.IsOpen = False
        e.Handled = True
    End Sub
    Sub SuchobjektAusschalten()
        panningAusschalten()
        ' aktPolygon = New clsParapolygon
        oldSuchFlurstueck = aktFST
        aktFST = New ParaFlurstueck

        'aktPolygon = New clsParapolygon
        OSrefresh = False
        imgpin.Visibility = Visibility.Collapsed
        suchCanvas.Visibility = Visibility.Collapsed
        'btnSuchobjAusSchalten.Visibility = Visibility.Collapsed
        refreshMap(True, False)
    End Sub
    Sub SuchobjektEinschalten()
        panningAusschalten()
        ' aktPolygon = New clsParapolygon
        '   aktFST = New ParaFlurstueck
        aktFST = oldSuchFlurstueck
        'aktPolygon = New clsParapolygon
        OSrefresh = True
        imgpin.Visibility = Visibility.Visible
        suchCanvas.Visibility = Visibility.Visible
        'btnSuchobjAusSchalten.Visibility = Visibility.Collapsed
        refreshMap(True, False)
    End Sub

    Private Sub cbSOeinschalten_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If cbSOeinschalten.IsChecked Then
            SuchobjektEinschalten()
        Else
            SuchobjektAusschalten()
        End If
        e.Handled = True
    End Sub

    Private Sub btnbookmark_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        myglobalz.mgisBackModus = False
        Dim bmliste As New winBM
        bmliste.ShowDialog()
        If bmliste.aktion = "nichts" Then
            'nichts
        End If
        If bmliste.aktion = "bmaktivieren" Then
            If auswahlBookmark IsNot Nothing Then
                panningAusschalten()
                makeThemenInVis()
                aktiviereBM(auswahlBookmark)
            End If
        End If
        e.Handled = True
    End Sub

    Private Sub btnParadigmaLight_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'Dim userid As Integer = getUsersIdFromParadigma(GisUser.username)
        gisdokstarten(GisUser.username)
    End Sub

    'Private Shared Function getUsersIdFromParadigma(lusername As String) As Integer
    '    GisUser.PL_UserNr = modPLUser.getUsernr(lusername)
    '    If GisUser.PL_UserNr < 1 Then
    '        GisUser.PL_UserNr = modPLUser.addUser(lusername, GisUser.ADgruppenname)
    '    End If
    'End Function

    Private Function gisdokstarten(lusername As String) As System.Diagnostics.Process
        Dim datei, param As String
        datei = "C:\ptest\PL\PL_BESTAND.exe"
        param = "username=" & GisUser.username

        l("gisdokstarten " & param & Environment.NewLine & datei)
        Dim proc As New Process
        proc = Process.Start(datei, param)
        Return proc
    End Function

    Private Sub btnGooglemaps_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        'tivogel.Visibility = Visibility.Visible
        'tigis.Visibility = Visibility.Collapsed
        'panningAusschalten()
        Dim url As String
        url = getGoogleMapsString()
        'webBrowserControlVogel.Navigate(New Uri(calcURI4vogel))
        Process.Start(url)
        e.Handled = True
    End Sub

    Private Function getGoogleMapsString() As String
        Try
            nachricht("USERAKTION: googlekarte  vogel")
            Dim gis As New clsGISfunctions
            Dim result As String
            kartengen.aktMap.aktrange.CalcCenter()
            result = gis.GoogleMapsAufruf_Extern(kartengen.aktMap.aktrange, True)
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

    Private Sub btnGmapsSchliessen_Click(sender As Object, e As RoutedEventArgs)
        btnGmaps.IsOpen = False
        e.Handled = True
    End Sub

    Private Sub btnMessenSchliessen_Click(sender As Object, e As RoutedEventArgs)
        btnMessen.IsOpen = False
        CanvasClickModus = ""
        e.Handled = True
    End Sub

    Private Sub btnNeueMessung_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If CanvasClickModus = "koordinate" Then
            messeKoordinate()
        End If
        If CanvasClickModus = "flaeche" Then
            messeFlaeche()
            btnMessen.IsOpen = True
        End If
        If CanvasClickModus = "strecke" Then
            messestrecke()
            btnMessen.IsOpen = True
        End If
    End Sub

    Private Sub btnKoordUmrechner_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim umreechnerUrl As String
        Dim aktp As New myPoint
        umreechnerUrl = "https://www.deine-berge.de/Rechner/Koordinaten/Dezimal/" '50.021827,8.769315"
        If tbMinimapCoordinate2.Text.IsNothingOrEmpty Then
            'es wird keine koordinate übergeben
        Else
            Dim temp As String = tbMinimapCoordinate2.Text.Replace("[m]", "").Trim
            Dim a = temp.Split(","c)
            aktp.X = CDbl(a(0))
            aktp.Y = CDbl(a(1))
            ReDim punktarrayInM(0)
            punktarrayInM(0) = aktp
            Dim quellstring As String = modKoordTrans.bildeQuellKoordinatenString(punktarrayInM)
            Dim aufruf As String = modKoordTrans.bildeaufruf4KoordinatenServer(quellstring, punktarrayInM.Count.ToString, "UTM", "WINKEL_G")
            Dim hinweis As String = ""
            Dim result As String = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            nachricht("result: " & result)
            modKoordTrans.getLongLatFromResultSingle(result, longitude, latitude)
            umreechnerUrl = umreechnerUrl & latitude.Replace(",", ".") & "," & longitude.Replace(",", ".")
        End If
        Process.Start(umreechnerUrl)
    End Sub

    Private Sub btnNachParadigma_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            If aktvorgangsid <> String.Empty Then
                If modParadigma.GeometrieNachParadigma(aktPolygon, aktPolyline) Then
                    clsToolsAllg.userlayerNeuErzeugen(GisUser.username, myglobalz.aktvorgangsid)
                    MsgBox("Das Objekt wurde in die Paradigma-DB als Raumbezug übernommen. " & Environment.NewLine &
                           "Drücken Sie oben die RefreshTaste um die Änderung anzuzeigen!", MsgBoxStyle.OkOnly, "Datenübernahme OK")
                Else
                    MsgBox("Datenübernahme war nicht erfolgreich. Bitte beim Admin melden!")
                End If
            End If
        End If
    End Sub

    Private Sub btnWMSgetfeatureinfo_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If clsWMS.istWMSDBabfrage(layerActive.aid) Then
            MsgBox("Klicken Sie jetzt einen Punkt in  der Karte an:", , "WMS - Datenabfrage")
            panningAusschalten()
            imageMapCanvas.Visibility = Visibility.Collapsed
            CanvasClickModus = "wmsdatenabfrage"
        Else
            If clsWMS.istpointactivemodus(layerActive.aid) Then
                MsgBox("Klicken Sie jetzt einen Punkt in  der Karte an:", , "Datenabfrage")
                panningAusschalten()
                imageMapCanvas.Visibility = Visibility.Collapsed
                CanvasClickModus = "pointactivemodus"
            End If
        End If
        'btnWMSgetfeatureinfo.Visibility=Visibility.Collapsed
    End Sub

    Private Sub btnOS2CSV_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        csvAusgabe()
    End Sub

    Private Shared Sub csvAusgabe()
        Dim trenner As String = ";"
        Dim out As String = ""
        Dim ausgabeDIR As String = ""
        Dim outfile As String
        Try
            l("csvAusgabe---------------------- anfang")
            out = clsToolsAllg.genCSV4DT(trenner, OSrec.dt, 9)
            ausgabeDIR = System.IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Paradigma")
            l("ausgabeDIR anlegen " & ausgabeDIR)
            IO.Directory.CreateDirectory(ausgabeDIR)
            l("csvAusgabe---------------------- ende")

            outfile = ausgabeDIR & "\liste_" & clsString.date2string(Now, 2) & ".csv"
            l("csvAusgabe " & outfile)
            My.Computer.FileSystem.WriteAllText(outfile, out, False, enc)
            OpenDokument(outfile)
            l("csvAusgabe---------------------- ende")
        Catch ex As Exception
            l("Fehler in csvAusgabe: " & ex.ToString())
        End Try
    End Sub

    Private Sub cmbOSKat_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If cmbOSKat.SelectedItem Is Nothing Then Exit Sub
        Dim katos As String = CType(cmbOSKat.SelectedItem, String)
        refreshOS(katos, tbOSTextfilter.Text.Trim)
    End Sub
    Private Sub btnOSTextfilterStart_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim katos As String
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If cmbOSKat.SelectedItem Is Nothing Then
            katos = ""
        Else
            katos = CType(cmbOSKat.SelectedItem, String)
        End If
        refreshOS(katos, tbOSTextfilter.Text.Trim)
    End Sub
    Private Sub btnStartBplan_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        clsToolsAllg.startbplankataster()
    End Sub

    Private Sub btnDossier_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True

        'If clsDossier.istDossierModus(layerActive.aid) Then
        MsgBox("Klicken Sie jetzt einen Punkt in  der Karte an:", , "Dossierabfrage")
        panningAusschalten()
        imageMapCanvas.Visibility = Visibility.Collapsed
        CanvasClickModus = "dossiermodus"
        'End If

    End Sub

    Private Sub btnMgisHistoryBack_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        mgishistoryBack()
    End Sub

    Private Sub mgishistoryBack()
        Dim AlleKookieFiles As IO.FileInfo() = Nothing
        Dim reverseKookieFiles As IO.FileInfo() = Nothing
        Dim count As Integer
        Try
            l(" mgishistoryBack ---------------------- anfang")
            'dateiliste erstellen
            Dim di As New IO.DirectoryInfo(myglobalz.mgisRangecookieDir)
            'Dim so As New IO.SearchOption
            If di.Exists Then
                AlleKookieFiles = di.GetFiles("*.rng")
                count = AlleKookieFiles.GetUpperBound(0) + 1
                ReDim reverseKookieFiles(AlleKookieFiles.GetUpperBound(0))
                nachricht("Es wurden " & count & " HistoryItems gefunden.")
                nachricht("last" & myglobalz.mgisBackmodusLastCookie)
                Dim j = 0
                Dim ordFiles = From f In AlleKookieFiles Order By f.CreationTime
                For i = ordFiles.Count - 1 To 0 Step -1
                    reverseKookieFiles(j) = ordFiles(i)
                    j += 1
                Next
                For i = 0 To reverseKookieFiles.Count - 1
                    If reverseKookieFiles(i).CreationTime >= mgisBackmodusLastCookie Then
                        nachricht("test " & reverseKookieFiles(i).CreationTime)
                        Continue For
                    Else
                        'treffer
                        mgisBackmodusLastCookie = reverseKookieFiles(i).CreationTime
                        Dim a() As String
                        a = reverseKookieFiles(i).Name.Split("_"c)
                        kartengen.aktMap.aktrange.xl = CInt(a(0))
                        kartengen.aktMap.aktrange.xh = CInt(a(1))
                        kartengen.aktMap.aktrange.yl = CInt(a(2))
                        kartengen.aktMap.aktrange.yh = CInt(a(3))
                        myglobalz.mgisBackModus = True
                        refreshMap(True, True)
                        Exit Sub
                    End If
                Next



                myglobalz.mgisBackModus = True
                'darstellen
            End If

            l(" mgishistoryBack ---------------------- ende")
        Catch ex As Exception
            l("Fehler in mgishistoryBack: " & ex.ToString())
        End Try
    End Sub

    Private Sub imageMapCanvas_MouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        Mouse.Capture(Nothing)
        Dim KoordinateKLickpt As Point?
        KoordinateKLickpt = e.GetPosition(imageMapCanvas)
        clsSachdatentools.dossierOhneImap(KoordinateKLickpt)
        setBoundingRefresh(kartengen.aktMap.aktrange)
        refreshMap(True, True)
        suchObjektModus = "fst"
    End Sub

    Private Sub Window_SizeChanged(sender As Object, e As SizeChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        resizeWindow()
    End Sub
    Sub resizeWindow()

        If Width < 1131 Then Width = 1131
        If Height < 800 Then Height = 800
        dockTop.Height = 50
        cv1.Width = CLng(Width) - CLng(dockMenu.Width)
        cv1.Height = CLng(Height) - CLng(dockTop.Height)
        globCanvasWidth = CInt(cv1.Width)
        globCanvasHeight = CInt(cv1.Height)
        kartengen.aktMap.aktcanvas.w = CLng(cv1.Width)
        kartengen.aktMap.aktcanvas.h = CLng(cv1.Height)
        slotsResize(cv1.Width, cv1.Height)
        btnrefresh.Background = Brushes.Green
    End Sub

    Private Sub slotsResize(width As Double, height As Double)
        cv2.Width = width : cv2.Height = height
        cv3.Width = width : cv3.Height = height
        cv4.Width = width : cv4.Height = height
        cv5.Width = width : cv5.Height = height
        cv6.Width = width : cv6.Height = height
        cv7.Width = width : cv7.Height = height
        cv8.Width = width : cv8.Height = height
        cv9.Width = width : cv9.Height = height
        cv10.Width = width : cv10.Height = height
        cv11.Width = width : cv11.Height = height
        cv12.Width = width : cv12.Height = height
        cv13.Width = width : cv13.Height = height
        cv14.Width = width : cv14.Height = height
        cv15.Width = width : cv15.Height = height
        cv16.Width = width : cv16.Height = height
        cv17.Width = width : cv17.Height = height
        cv18.Width = width : cv18.Height = height
        cv19.Width = width : cv19.Height = height
        cv20.Width = width : cv20.Height = height
    End Sub

    Private Sub Window_StateChanged(sender As Object, e As EventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Select Case WindowState
            Case WindowState.Maximized
                'initVGCanvasSize()
                '  resizeWindow() btn.SetValue(Canvas.LeftProperty,50.0);
                'cv1.VerticalAlignment = VerticalAlignment.Top
                cv1.Width = CLng(System.Windows.SystemParameters.PrimaryScreenWidth) - CLng(dockMenu.Width)
                cv1.Height = CLng(System.Windows.SystemParameters.PrimaryScreenHeight) - CLng(dockTop.Height)
                globCanvasWidth = CInt(cv1.Width)
                globCanvasHeight = CInt(cv1.Height)
                slotsResize(cv1.Width, cv1.Height)
                refreshMap(True, True)
            Case WindowState.Minimized
            Case WindowState.Normal
                resizeWindow()
                refreshMap(True, True)
        End Select
        resizeWindow()
    End Sub
End Class



