Imports System.ComponentModel
Imports System.Data
Imports System.Threading.Tasks
Imports CefSharp
Imports mgis
'Imports Squirrel
' modus=paradigma vorgangsid=9609 range=484593,484993,5544035,5544435 beschreibung="Neubau eines Zweifamilienhauses mit Garagen und PKW-Stellplätzen" az="II-67-3311-38579-17-wa-5_1-04769-17"
'ZAB3-QBCW-B453-3QAH-BWEK-G92Y-83
'c:\kreisoffenbach\mgis\mgis.exe modus=probaug suchmodus="adresse" gemeinde="[BAUORT]" strasse="[STRASSE]" hausnr="[HNR]" az="[AZ-NUMMER]-[AZ-JAHR]"
'pause

Class MainWindow
    Private wb3D As CefSharp.Wpf.ChromiumWebBrowser
    Private wbvogel As CefSharp.Wpf.ChromiumWebBrowser
    Private wb3Disinit As Boolean = False
    Private wbvogelisinit As Boolean = False
    Private letztekategorieAuswahl As String = ""

    'Public Property mainWindow As Window = Me.mainWindow
    Private RubberbandStartpt As Point?
    Private RubberbandEndpt As Point?
    Private aktrangebox As Rectangle
    Private wmsfensterzaehler As Integer = 0
    Private fensterzaehler As Integer = 0
    Public Shared Property ladevorgangAbgeschlossen As Boolean = False
    Private Property curContentMousePoint As Point
    Private origContentMousePoint As Point
    Private Property isDraggingFlag As Boolean
    Public Property KreislinienRadius As Double = 0
    Public Property pdf_mapdruckComboInitFertig As Boolean
    Private myPolyVertexCount As Integer
    Private KoordinateKLickpt As Point?
    'IMAP------------------ 
    Public imapTemplateString As String = ""

    Public imapBackGroundWroker As New BackgroundWorker : Public IMAPaufruf As String
    Public xkorrektur As Double = 0
    Public ykorrektur As Double = 0
    'IMAP------------------ 
    Sub New()
        InitializeComponent()
    End Sub
    'Private Async Sub squirrel_erneuern()
    '    'The following code is added to cause the application to check for, download
    '    'and install any New releases of App in the background while you use the application. 
    '    '      Squirrel --releasify mgis.1.0.1.nupkg --releaseDir "D:\avs_cube\mgis\mgiscef\releasifyDir"
    '    ' Squirrel --releasify mgis.1.0.5.nupkg --releaseDir "s:\fkat\paradigma\mgis\releases"  --no-msi  
    '    Dim rel As String
    '    rel = "http://w2gis02.kreis-of.local/fkat/paradigma/mgis/releases/"
    '    rel = "https://buergergis.kreis-offenbach.de/fkat/paradigma/mgis/releases/"
    '    'worx:     rel = "\\w2gis02\gdvell\fkat\paradigma\mgis\releases\"
    '    Using mgr As New UpdateManager(rel) '
    '        'If iminternet Then Await mgr.UpdateApp
    '        ' Await mgr.UpdateApp
    '    End Using
    'End Sub
    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        clsStartup.setLogfile() : l("Start " & Now)
        l(" Environment.UserName " & Environment.UserName)

        btnOSdropdown.Visibility = Visibility.Collapsed
        Me.Activate()
        'squirrel_erneuern
        'btnWMSgetfeatureinfo.Visibility = Visibility.Collapsed
        l(Width & ", " & Height) : Debug.Print(Left & ", " & Top)
        'IMAP------------------
        'MsgBox(strGlobals.imaptemplateFile)

        l("ladevorgangAbgeschlossen1 = " & ladevorgangAbgeschlossen)
        WebBrowser1.JavascriptObjectRepository.Register("boundAsync", New BoundObject(), True)
        CefSharpSettings.LegacyJavascriptBindingEnabled = True
        l("ladevorgangAbgeschlossen = " & ladevorgangAbgeschlossen)

        'IMAP------------------ 
        clsStartup.defineDinA4Dina3Formate()
#If DEBUG Then
        'Datenbank_als_HTML      = False
        iminternet = clsStartup.getIminternet
        'iminternet = True
        HauptServerName = "gis" '"w2gis02"
#Else
        HauptServerName = "gis"
        iminternet = clsStartup.getIminternet
#End If
        l("vor definevar")
        Reboot4GISHost()
        l("nach definevar")
        'WindowState = WindowState.Normal
        'Me.Activate()
        'Me.Show()
        'Me.Topmost = True
        ladevorgangAbgeschlossen = True
    End Sub

    Private Sub Reboot4GISHost()
        clsInitStrings.setMainstrings()
        setzeGISWorkingDir()

        clsStartup.initParadigmaAdmins()
        GisUser.username = Environment.UserName
        GisUser.nick = Environment.UserName
        l("Start " & Now) : l("mgisversion:" & mgisVersion)
        l(" GisUser.username " & GisUser.username)
        If iminternet Then
            myglobalz.minErrorMessages = False
        Else
            myglobalz.minErrorMessages = False
        End If
        If iminternet Then
            btnDossier.Visibility = Visibility.Collapsed
        Else
            btnDossier.Visibility = Visibility.Visible
        End If

        If IO.File.Exists(strGlobals.imaptemplateFile) Then
            imapTemplateString = My.Computer.FileSystem.ReadAllText(strGlobals.imaptemplateFile) : l("imapTemplateString geladen")
        End If
        If IO.File.Exists(strGlobals.dbtemplateFile) Then
            dbTemplateString = My.Computer.FileSystem.ReadAllText(strGlobals.dbtemplateFile) : l("dbTemplateString geladen")
        End If

#If DEBUG Then
        'GisUser.username = "hurz"
        'GisUser.nick = "hurz"
#End If
        If iminternet Then
            Dim nick As String = Environment.UserName, pw As String = ""
            nick = clsString.normalize_Filename(clsString.umlaut2ue(GisUser.nick), "_")
            GisUser.nick = clsString.normalize_Filename(clsString.umlaut2ue(GisUser.nick), "_")
#If DEBUG Then
            'GisUser.username = "Wilhelm"
            'GisUser.nick = "Wilhelm"
#End If
            'If clsStartup.getNicknameAndPWFromLokalCookie(nick, pw, IO.Path.Combine(strGlobals.gisWorkingDir, "credo.txt")) Then
            '    If nick.IsNothingOrEmpty Then
            '        'GisUser.nick = nick
            '        GisUser.EmailPW = pw
            '        GisUser.nick = clsString.normalize_Filename(clsString.umlaut2ue(GisUser.nick), "_")
            '    Else
            '        GisUser.nick = nick
            '        GisUser.EmailPW = pw
            '        GisUser.nick = clsString.normalize_Filename(clsString.umlaut2ue(GisUser.nick), "_")
            '    End If
            'Else
            '    '    MsgBox("Sie sollten ihre Zugangsdaten eingeben.")

            'End If
        Else
            GisUser.nick = Environment.UserName
            GisUser.EmailPW = ""
            GisUser.nick = clsString.normalize_Filename(clsString.umlaut2ue(GisUser.nick), "_")
        End If


        GisUser.macAdress = clsString.normalize_Filename(clsString.umlaut2ue(clsGetComputerID.getMacAddress), "_")
        GisUser.cpuID = clsString.normalize_Filename(clsString.umlaut2ue(clsGetComputerID.getCPU_ID), "_")
        GisUser.MachineName = clsString.normalize_Filename(clsString.umlaut2ue(Environment.MachineName), "_")
        GisUser.domain = clsString.normalize_Filename(clsString.umlaut2ue(Environment.UserDomainName), "_")
        GisUser.rites = clsStartup.getUserAndInternetInfoRites(GisUser)

        'l("imapTemplateString " & imapTemplateString)
        'l("dbTemplateString " & dbTemplateString)


        'MsgBox(GisUser.nick)
#If DEBUG Then
        'GisUser.nick = "Fedinen_j"
        'GisUser.nick = "Stich_K"
        'GisUser.nick = "becker_a"
        'GisUser.nick = "hurz"
        '  GisUser.nick = "Jaeger_C"
        '  GisUser.nick = "Buchmann_U"
        '  GisUser.nick = "Mueller_B"
        '  GisUser.nick = "Weicker-Zoeller_C"
        '  GisUser.nick = "sindl_p"
        '  GisUser.nick = "asasd"
        'GisUser.nick = "pilz_j"
        'GisUser.nick = "waldschmitt_r"
        'GisUser.nick = "ackermann_r"
        'GisUser.nick = "schmittner_u"
        'GisUser.nick = "nehler_U"
        'GisUser.nick = "schoeniger_j"
#End If


        'btnSuchobjAusSchalten.Visibility = Visibility.Collapsed
        userIniProfile = New clsINIDatei(mgisRemoteUserRoot & "userinis\" & GisUser.nick & ".ini")

        'myglobalz.minErrorMessages = True
#If DEBUG Then
        myglobalz.minErrorMessages = False
#End If
        clsSendmailTools.getEmailAccountFromIni(GisUser)

        Me.Top = setPosition("diverse", "windetailformpositiontop", Me.Top)
        Me.Left = setPosition("diverse", "windetailformpositionleft", Me.Left)
        Me.Width = setPosition("diverse", "windetailformpositionwidth", Me.Width)
        Me.Height = setPosition("diverse", "windetailformpositionheight", Me.Height)

        exploreralphabetisch = exploreralphabetischFeststellen()

        zweitenBildschirm()

        'If clsStartup.gisMaximiertStarten(True) Then
        '    WindowState = WindowState.Maximized
        'End If
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
        If GisUser.ADgruppenname.ToLower = "umwelt" Then
            btnSelection.Visibility = Visibility.Visible
        Else
            btnSelection.Visibility = Visibility.Collapsed
        End If


        wb3D = New CefSharp.Wpf.ChromiumWebBrowser
        tiGOogle3D.Children.Add(wb3D)
        AddHandler wb3D.IsBrowserInitializedChanged, AddressOf OnIsBrowserInitializedChanged3D

        wbvogel = New CefSharp.Wpf.ChromiumWebBrowser
        dpVogel2.Children.Add(wbvogel)
        AddHandler wbvogel.IsBrowserInitializedChanged, AddressOf OnIsBrowserInitializedChangedVogel

        myglobalz.minErrorMessages = clsOptionTools.getminErrorMessagesFromIni()
        If clsStartup.istGISAdmin() Then
            myglobalz.minErrorMessages = False
        End If
        l(" myglobalz.minErrorMessages " & myglobalz.minErrorMessages)
        l("strGlobals.gisWorkingDir " & strGlobals.gisWorkingDir)
    End Sub

    Private Shared Sub setzeGISWorkingDir()
        l("workingdir alt: " & strGlobals.gisWorkingDir)
        IO.Directory.SetCurrentDirectory(strGlobals.gisWorkingDir)
        l("workingdir neu: " & strGlobals.gisWorkingDir)
    End Sub

    Private Sub OnIsBrowserInitializedChangedVogel(sender As Object, e As DependencyPropertyChangedEventArgs)
        aktGlobPoint.X = kartengen.aktMap.aktrange.xcenter
        aktGlobPoint.Y = kartengen.aktMap.aktrange.ycenter
        'google3dintro()

        Dim uncdatei As String = clsStartup.calcURI4vogel
        Dim url As String = uncdatei.Replace(myglobalz.serverUNC, myglobalz.serverWeb).Replace("\", "/")
        wbvogel.Load((url))
        wbvogelisinit = True
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
            l("Fehler in exploreralphabetischFeststellen: ", ex)
            Return False
        End Try
    End Function

    Private Sub startroutine(explorerAlphabetisch As Boolean)
        currentProcID = clsGisstartPolitik.getCurrentProcId()
        'Dim test As String = Commandl
        Dim arguments As String() = Environment.GetCommandLineArgs()
        l("GetCommandLineArgs: {0}" & String.Join(" ", arguments))
        clsStartup.mapAllArguments(arguments)

        If Not iminternet Then
            Dim stealth = clsStartup.getStealthName4FeinenJ(arguments)
            If Not stealth.IsNothingOrEmpty Then
                GisUser.username = stealth
                GisUser.nick = stealth
                Dim rites As String = ""
                GisUser.rites = clsStartup.getUserAndInternetInfoRites(GisUser)
            End If
        End If
#If DEBUG Then
        'STARTUP_mgismodus = "bebauungsplankataster"
        'STARTUP_mgismodus = "paradigma"
        'STARTUP_mgismodus = "vanilla"
        'GisUser.nick = "Weber_S"
        'GisUser.nick = "Sikora_T"
        'GisUser.nick = "Pieroth_s"
        'GisUser.nick = "Ritter_m"
        'GisUser.nick = "Englert_N"

        'GisUser.nick = "May_y"
        ' GisUser.nick = "koslov_h"
        ' aktvorgangsid weiter unten einstellen
        'GisUser.nick = "El_Achak_H"
#End If
        'clsStartup.LegendenCacheLoeschen
        'If GisUser.nick.ToLower = "stich_k" Or GisUser.nick.ToLower = "feinen_j" Or GisUser.nick.ToLower = "zahnlueckenpimpf" Then
        tiExplorerKategorie.Visibility = Visibility.Collapsed
        tiGesamtgExplorer.Visibility = Visibility.Visible
        'Else
        '    '  tiExplorerKategorie.Visibility = Visibility.Collapsed
        '    'tiGesamtgExplorer.Visibility = Visibility.Collapsed
        '    tiGesamtgExplorer.IsEnabled = True
        '    tiSuche.IsEnabled = False
        '    tiSuche.Visibility = Visibility.Collapsed
        'End If

        If STARTUP_mgismodus = "paradigma" Then
            aktvorgangsid = clsStartup.getStartupArgument(arguments, "vorgangsid=")

#If DEBUG Then
            'aktvorgangsid = "38373"
            ' aktvorgangsid = "9609"
            ''aktvorgangsid = "37036" 
            'STARTUP_mgismodus = "paradigma"
            'aktvorgangsid = "9609" 
#End If
            aktvorgang.id = CInt(aktvorgangsid)
            tbVorgangsid.Text = aktvorgangsid
        Else
            spVIDParadigma.Visibility = Visibility.Collapsed
            stackVorgangsid.IsEnabled = False
        End If

        rbfit2.Visibility = Visibility.Collapsed
        clsGisstartPolitik.InstallUpdateInAutostart()
        Dim STARTUP_rangestring As String = clsStartup.getStartupArgument(arguments, "range=")

        clsGisstartPolitik.gisStartPolitikUmsetzen(prozessname)
        clsINIDatei.UserinifileAnlegen(0, mgisRemoteUserRoot & "userinis\", GisUser.nick)

        If clsStartup.gisMaximiertStarten(True) Then
            WindowState = WindowState.Maximized
        End If
        clsOptionTools.einlesenParadigmaDominiert(ParadigmaDominiertzuletztFavoriten)

        initdb()
        myglobalz.mgisRangecookieDir = clsToolsAllg.initMgisHistory
        clsRangehistory.rangeHistoryLeeren()

        setUserFDkurz(GisUser)
        l("user_fdkurz  1. " & GisUser.ADgruppenname)
        If iminternet Then
            GisUser.favogruppekurz = "iminternet"
        Else
            If GisUser.favogruppekurz = "umwelt" Then
                GisUser.paradigmaAbteilung = modParadigma.getParadigmaAbteilung4FDumwelt(GisUser.nick)

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
        End If

        l("setUserFDkurz clsActiveDir.fdkurz " & clsActiveDir.fdkurz)
        l("globalParadigmaUser  2. " & GisUser.favogruppekurz)
        VorgangsButtonSichtbarMachen()
        If iminternet Then
        Else
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
        End If

        setGruppenFavoritTextButton()


        If iminternet Then
        Else
            If clsActiveDir.fdkurz.Trim.ToLower = "umwelt" Then
                If STARTUP_mgismodus = "paradigma" Then
                    GisUser.userLayerAid = modParadigma.getuserlayeraid(GisUser.nick)
                    l(" GisUser.userLayerAid  " & GisUser.userLayerAid)
                End If
            End If
        End If

        ProxyString = getproxystring()

        allDokus = clsWebgisPGtools.getAllDokus(myglobalz.iminternet) : l("anzahl dokus: " & allDokus.Count)

        allLayersPres = clsWebgisPGtools.getAllLayersPres(myglobalz.iminternet, clsActiveDir.fdkurz, allLayers) : l("anzahl allLayersPres: " & allLayersPres.Count)
        hgrundLayers = modLayer.getHintergrundListe(myglobalz.iminternet) : l("anzahl hgrundLayers: " & hgrundLayers.Count)


        kategorienliste = clsToolsAllg.getKategorienListe(allLayersPres)
        clsLayerHelper.setKatinfo2Layers(allLayersPres)
        wmspropList = modLayer.getWMSpropList(allLayersPres, allLayers) : l("anzahl wmspropList: " & wmspropList.Count)
        modLayer.markWMSlayers(allLayers)

        initHintergrundCMB() 'muss vorne sein


        For Each clay As clsLayerPres In allLayersPres
            'clay.isHgrund = calcIshgrund(clay, allLayersPres)
            clay.isHgrund = modLayer.istAuchHintergrund(clay)
        Next
        clsWebgisPGtools.calcOwners(allDokus)
        clsWebgisPGtools.dombineLayerDoku(allLayersPres, allDokus)
        clsWebgisPGtools.dombineLayerDoku(hgrundLayers, allDokus)
        clsWebgisPGtools.calcEtikett_kategorie_tultipp(allLayersPres, explorerAlphabetisch)
        lvEbenenAlle.Height = 900
        lvEbenenKompakt.Height = 900
        lvEbenenKategorie.Height = 900
        '200er ebenen ausschalten
        stContext.Visibility = Visibility.Collapsed
        stwinthemen.Visibility = Visibility.Collapsed
        stPDFDruck.Visibility = Visibility.Collapsed

        initVGCanvasSize()
        setCanvasSizes(cvtop)

        showAktVorgangsid()
        kartengen.Domainstring = serverWeb
        btnGetFlaecheEnde.Visibility = Visibility.Collapsed
        If STARTUP_mgismodus = "probaug" Then
            kartengen.aktMap.aktrange = handleProbaugModus(STARTUP_rangestring)
        Else
            If ProbaugSuchmodus = "flurstueck" Or ProbaugSuchmodus = "adresse" Then
                kartengen.aktMap.aktrange = handleProbaugModus(STARTUP_rangestring)
                setGlobalz4ProbaugAdresse()
            Else
                kartengen.aktMap.aktrange = clsStartup.setMapFirstRange(STARTUP_rangestring)
            End If
        End If
        WebBrowser1.Visibility = Visibility.Collapsed
        defineGruppenFavoriten()
        ' modLayer.getLayerHgrund()
        ''cmbHgrund.SelectedValue = layerHgrund.aid
        'layersSelected = getStandardlayersAids()
        ' layersSelected = modLayer.getCompleteLayers()
        initMasstabCombo()

        cmbMasstab.ItemsSource = masstaebe
        ' masstaebe.Clear()

        layersSelected.Sort()
        myglobalz.slots = SlotTools.createAllSlots(layerHgrund, layersSelected,
                                                   cv0, cv1, cv2, cv3, cv4, cv5, cv6, cv7, cv8, cv9, cv10,
                                                   cv11, cv12, cv13, cv14, cv15, cv16, cv17, cv18, cv19, cv20,
                                                   cv21, cv22, cv23, cv24, cv25, cv26, cv27, cv28, cv29, cv30,
                                                   cv31, cv32, cv33, cv34, cv35, cv36, cv37, cv38, cv39, cv40,
                                                   cv41, cv42, cv43, cv44, cv45, cv46, cv47, cv48, cv49, cv50,
                                                   OSmapCanvas)
        SlotTools.setAllSlotsEmpty(0)
        If layerActive.iswms Then
            'MessageBox.Show("Es ist eine WMS-Ebene aktiv. Zur Datenabfrage bitte jeweils den blauen Knopf  'WMS' nutzen.", "WMS-Ebene aktiv")
            'btnWMSgetfeatureinfo.Visibility = Visibility.Visible
            panningAusschalten()
            WebBrowser1.Visibility = Visibility.Collapsed
            cvtop.Cursor = Cursors.Hand
            CanvasClickModus = "wmsdatenabfrage"
        End If
        clsLayerHelper.setKatinfo2Layers(layersSelected)
        refreshMap(True, True)

        cvPDFrechteck.Visibility = Visibility.Collapsed
        clsWebgisPGtools.getOSliste(allLayersPres, "")
        dgOSliste.DataContext = allOSLayers
        Title = clsStartup.getWindowTitel(tbVorgangsid.Text, allLayersPres.Count)
        l("layersSelected.Count " & layersSelected.Count)
        hinweisFallsKeineEbenenGeladenSind()
        generateExplorer(kategorienliste)
        generateKategorienCMB(kategorienliste)

    End Sub



    Private Sub generateKategorienCMB(kategorienliste As List(Of clsUniversal))
        For Each item As clsUniversal In kategorienliste
            Dim cb As New ComboBoxItem
            cb.Name = "cmbE_" & item.tag.Trim
            cb.Content = item.titel 'clsString.Capitalize(item.tag.Replace("h_", "Hist. ")) ' kat.ToUpper
            cb.Tag = item.tag.ToLower.Trim
            'Dim tt As New ToolTip
            'Dim Binding As New Binding
            'Binding.Path = New PropertyPath("c:\kreisoffenbach\mgis\kat\allgemein.txt") 'item.ToolTip)
            'tt.SetBinding(ToolTipProperty, Binding)
            'cb.ToolTip = tt
            cb.ToolTip = item.tooltip

            'item.ToolTip = item.ToolTip.Replace("\", "/")

            'Dim Binding As New Binding
            ''= New Binding("Path=" & item.ToolTip)
            ''Binding.Path = item.ToolTip
            ''Binding.Source = New PropertyPath(item.ToolTip)
            'Binding.Path = New PropertyPath(item.ToolTip)

            'cb.SetBinding(ToolTipProperty, Binding)
            '_textBlock.SetBinding(TextBlock.ToolTipProperty, Binding)
            cb.FontWeight = FontWeights.Bold
            cbEbenenKategorien.Items.Add(cb)
        Next
    End Sub
    Private Sub generateExplorer(kategorienliste As List(Of clsUniversal))
        Try
            For Each item As clsUniversal In kategorienliste
                Dim tb As New TextBlock
                tb.Name = "tbE_" & item.tag.Trim
                tb.Text = item.titel 'clsString.Capitalize(kat.Replace("h_", "Hist. ")) 'kat.ToUpper
                tb.Tag = item.tag.ToLower.Trim
                tb.ToolTip = item.tooltip
                tb.FontWeight = FontWeights.Bold
                'tb.MouseRightButtonDown += New MouseButtonEventHandler(cc_CopyToClip)
                AddHandler tb.MouseDown, AddressOf tbE_mousedown
                spExplorerParent.Children.Add(tb)
                'spExplorerParent.RegisterName(tb.Name, tb)
                '----------------------------
                Dim lv As New ListView
                lv.Name = "lvE_" & item.tag.Trim
                '   lv.Background = "{StaticResource flaechenBackground}"
                lv.Background = Brushes.Beige
                Dim pt As New Point
                pt.X = 0.5 : pt.Y = 0.5
                lv.RenderTransformOrigin = pt
                lv.Visibility = Visibility.Collapsed
                'lv.BorderBrush = Brushes.DarkGray 
                'Dim tn As New Thickness(top:=1, left:=1, right:=1, bottom:=1)
                'lv.BorderThickness = tn
                lv.FontSize = 12
                lv.FontFamily = New FontFamily("arial")
                'lv.ScrollViewer.HorizontalScrollBarVisibility = "Disabled"
                AddHandler lv.SelectionChanged, AddressOf lvEbenenAlle_SelectionChanged
                AddHandler lv.PreviewMouseWheel, AddressOf lvEXP_PreviewMouseWheel
                lv.ItemTemplate = CType(Me.FindResource("lvGesamtExplorerTemplate"), DataTemplate)
                spExplorerParent.Children.Add(lv)
                'spExplorerParent.RegisterName(lv.Name, lv)
            Next
        Catch ex As Exception
            l("fehler in generateExplorer: ", ex)
        End Try
    End Sub

    Private Sub tbE_mousedown(sender As Object, e As MouseButtonEventArgs)
        Dim tb As TextBlock = CType(sender, TextBlock)
        letztekategorieAuswahl = tb.Tag.ToString.ToLower.Trim
        Dim expOeffnen As Boolean = False
        setWeightAndMode(tb, expOeffnen)
        Dim aktlistview As ListView = getListview4Name("lvE_" & letztekategorieAuswahl.Trim)

        expKategorieOeffnen(expOeffnen, aktlistview, letztekategorieAuswahl, tb.Tag.ToString)
    End Sub

    Private Function getListview4Name(tag As String) As ListView
        For Each lv As ListView In FindVisualChildren(Of ListView)(Me)
            If lv.Name = tag Then
                Return lv
                Continue For
            End If
        Next
        Return Nothing
    End Function

    Private Function handleProbaugModus(STARTUP_rangestring As String) As clsRange
        If clsProbaug.sindProbaugSuchParamsOK(ProbaugSuchmodus, probaugAdresse, probaugFST) Then
            Dim prorange As New clsRange
            Dim errorhinweis As String = ""
            tbVorgangsid.Text = aktvorgang.az.ToString
            prorange = clsProbaug.getAktrangeFromProbaug(ProbaugSuchmodus, probaugAdresse, probaugFST, errorhinweis)
            l(" prorange nhinweis " & errorhinweis)
            If prorange Is Nothing Then
                Dim mesres As MessageBoxResult
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
                suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen
                imgpin.Visibility = Visibility.Visible
                suchCanvas.Visibility = Visibility.Visible
                '
                setGlobalz4ProbaugAdresse()

                'btnSuchobjAusSchalten.Visibility = Visibility.Visible
            End If
        Else
            MsgBox("Die von Probaug übergebenen Suchparameter sind nicht brauchbar.")
            kartengen.aktMap.aktrange = clsStartup.setMapFirstRange(STARTUP_rangestring)
        End If
        Return kartengen.aktMap.aktrange
    End Function

    Private Sub setGlobalz4ProbaugAdresse()
        If ProbaugSuchmodus = "adresse" Then
            nachricht("USERAKTION: ProbaugSuchmodus = adresse suchen ")
            'kartengen.aktMap.aktrange.CalcCenter()
            aktGlobPoint.X = kartengen.aktMap.aktrange.xcenter
            aktGlobPoint.Y = kartengen.aktMap.aktrange.ycenter
            aktGlobPoint.strX = CType(aktGlobPoint.X, String)
            aktGlobPoint.strY = CType(aktGlobPoint.Y, String)

            aktPolygon.ShapeSerial = holePUFFERPolygonFuerPoint(aktGlobPoint, 30) 'pufferinMeter)
            aktPolygon.originalQuellString = aktPolygon.ShapeSerial
            aktFST.normflst.serials.Clear()
            aktFST.normflst.serials.Add(aktPolygon.ShapeSerial)
            suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen
            setBoundingRefresh(kartengen.aktMap.aktrange)
        End If
    End Sub

    Private Sub VorgangsButtonSichtbarMachen()
        'den vorgangsbuttonSichtbarMachen 
        If iminternet Then
            spVIDParadigma.Visibility = Visibility.Collapsed
            btnParadigmaLight.Visibility = Visibility.Collapsed
            Exit Sub
        End If
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
                    btnParadigmaLight.Visibility = Visibility.Collapsed
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
        If layersSelected.Count < 1 Then
            Dim rtfdatei As String
            'rtfdatei = myglobalz.mgisUserRoot & "hinweis_KeineEbenen.rtf"
            rtfdatei = strGlobals.hinweis_KeineEbenenrtf
            Dim dokdatei = ""
            Dim freileg As New winLeg(rtfdatei, dokdatei, "rtf", 0)
            freileg.Show()
        End If
    End Sub

    Private Function gisStartFavoritenUmsetzen() As Boolean
        Throw New NotImplementedException()
    End Function

    Private Sub setGruppenFavoritTextButton()
        btnGruppeFavo.ToolTip = "Die Ansicht meiner Gruppe aufrufen. Meine Gruppe ist '" & GisUser.favogruppekurz.ToUpper & "' !"
    End Sub

    Private Sub zweitenBildschirm()
        Dim zweiterScreenvorhanden As Boolean = False
        Dim aufzweitembildschirmstarten As Boolean = False
        Dim hauptbildschirmStehtLinks As Boolean = False
        clsStartup.einlesenZweiterBildschirm(aufzweitembildschirmstarten, hauptbildschirmStehtLinks)
    End Sub

    Private Sub defineGruppenFavoriten()
        If STARTUP_mgismodus = "probaug" Or STARTUP_mgismodus = "paradigma" Then
            'meinfavofile = favoTools.calcMeinFavoriteDateiname("nichtauffindbar")
            If ParadigmaDominiertzuletztFavoriten Then
                favoTools.FavoritLaden("fix", GisUser.favogruppekurz)
            Else
                If Not favoTools.FavoritLaden("zuletzt", GisUser.nick) Then
                    favoTools.FavoritLaden("fix", GisUser.favogruppekurz)
                End If
            End If
        Else
            If Not favoTools.FavoritLaden("zuletzt", GisUser.nick) Then
                favoTools.FavoritLaden("fix", GisUser.favogruppekurz)
            End If
        End If
        favoritenUmsetzen()
    End Sub

    Private Sub initHintergrundCMB()

        cmbHgrund.ItemsSource = Nothing
        'hgrundLayers.Sort()
        Dim leer As New clsLayerPres
        leer.titel = "kein Hintergrund"
        leer.aid = 0
        hgrundLayers.Add(leer)

        'leer = New clsLayerPres
        'leer.titel = "Hintergrund: Helligkeit einstellen"
        'leer.aid = -1
        'hgrundLayers.Add(leer)

        'clsWebgisPGtools.dombineLayerDoku(templayers, allDokus)
        'hgrundLayers.Sort()
        cmbHgrund.ItemsSource = hgrundLayers
    End Sub

    Public Sub refreshMap(vgrundRefresh As Boolean, hgrundrefresh As Boolean)
        GC.Collect()
        zwischenbildBitteWarten()
        Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
        If vgrundRefresh And hgrundrefresh Then
            SlotTools.setAllSlotsEmpty(0)
        End If
        If vgrundRefresh And Not hgrundrefresh Then
            SlotTools.setAllSlotsEmpty(1)
        End If
        'svMainScrollviewer.Height = 800
        'lvEbenenAlle.Height = 800
        Dim layersUsed4Controlling As Integer = 0
        'mapfileNamenNeuBerechnen()
        ' initVGCanvasSize()
        setCanvasSizes(cvtop)

        calcBalkenbreite()
        stckBalken.Visibility = Visibility.Visible
        showLayersliste()
        'modLayer.createMapfileHG() entfällt weil header schon existiert

        Dim layersNachRangSortiert As List(Of clsLayerPres) = modLayer.sortiereLayers(layersSelected)

        tbInfolegende.Text = "__Bestandsliste_(" & layersSelected.Count & ")_"

        layersUsed4Controlling = SlotTools.layers2Slots(layerHgrund, layersNachRangSortiert,
                             OSmapCanvas,
                            vgrundRefresh, hgrundrefresh)



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

        initMassstab(cvtop.Width, cvtop.Height, (Width - dockMenu.Width))
        showTBmasstab(aktMasstab)
        Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
        Debug.Print(layerActive.aid.ToString)
        presentMap(vgrundRefresh, hgrundrefresh)
        Debug.Print(Width & "," & Height)

        If Not myglobalz.mgisBackModus Then
            If ladevorgangAbgeschlossen Then
                clsToolsAllg.mgisRangeCookieSave(kartengen.aktMap.aktrange, myglobalz.mgisRangecookieDir)
                myglobalz.mgisBackModus = False
            End If
        End If
        GC.Collect()
    End Sub


    Private Sub calcBalkenbreite()
        '  mapCanvas.Width 
        'balkenbreite ist 200
        Debug.Print(cvtop.Width & ", " & btnBalken.Width & ", " & kartengen.aktMap.aktrange.xdif)
        Dim balkenbreiteInMeter As Double '= (kartengen.aktMap.aktrange.xdif)
        Dim meterProPixel As Double = (kartengen.aktMap.aktrange.xdif / cvtop.Width)
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
        layerHgrund.dokutext = "(Sachgeb.: " & layerHgrund.kategorieLangtext & ") " & Environment.NewLine &
            clsWebgisPGtools.bildeDokuTooltip(layerHgrund)
        tbhgrund.ToolTip = layerHgrund.dokutext

        'lvEbenenAlle.ItemsSource = Nothing
        'lvEbenenAlle.Items.Refresh()
        'lvEbenenAlle.ItemsSource = layersSelected
        'lvEbenenAlle.Items.Refresh()

        'lvEbenenKompakt.ItemsSource = Nothing
        'lvEbenenKompakt.Items.Refresh()
        'lvEbenenKompakt.ItemsSource = clsLayerHelper.getKompaktLayers(layersSelected)
        'lvEbenenKompakt.Items.Refresh()
        refreshExplorerView("")
    End Sub

    Sub presentMap(vgrundRefresh As Boolean, hgrundrefresh As Boolean)
        Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
        Dim countererror As Integer = 0
        Try
            kartengen.aktMap.aktcanvas.w = CLng(cvtop.Width) : kartengen.aktMap.aktcanvas.h = CLng(cvtop.Height)
            skalieren()
            clsStartup.setzeAktKoordinate()
            suchCanvas.Children.Clear()
            clearAllSlots()
            GC.Collect()
            countererror = 0
            Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
            slots(0).aufruf = slots(0).BildGenaufrufMAPserver(slots(0).mapfile, myglobalz.serverWeb, kartengen.aktMap, slots(0).layer.isUserlayer)
            If hgrundrefresh Then 'slots(0).refresh Then
                If slots(0).layer.titel.ToLower = "kein hintergrund" Then
                    slots(0).setEmpty()
                Else
                    If slots(0).mapfile <> "" Then
                        MapModeAbschicken(slots(0)) 'hintergrund
                    End If
                End If
            End If
            countererror = 1
            If vgrundRefresh Then
                For i = 1 To 30
                    If slots(i).refresh Then
                        If slots(i).mapfile <> "" Then
                            slots(i).aufruf = slots(i).BildGenaufrufMAPserver(slots(i).mapfile, myglobalz.serverWeb, kartengen.aktMap, slots(i).layer.isUserlayer)
                            MapModeAbschicken(slots(i)) 'vordergrund
                        End If
                    End If
                Next
            End If

            countererror = 2
            'topLayer80
            Dim topLayer As New clsSlot
            topLayer.canvas = cvtop
            topLayer.funktion = "topLayer"
            topLayer.mapfile = strGlobals.paradigmaTopLayerMap
            topLayer.slotnr = slotnrMeaning.cvtopPanLayer80 '80
            topLayer.refresh = True
            topLayer.darstellen = True
            topLayer.setEmpty()
            topLayer.BildGenaufrufMAPserver(topLayer.mapfile, myglobalz.serverWeb, kartengen.aktMap, topLayer.layer.isUserlayer)
            MapModeAbschicken(topLayer) 'vordergrund
            countererror = 3
            'slots(1).BildGenaufrufMAPserver(slots(1).mapfile, myglobalz.serverWeb, kartengen.aktMap)
            'If slots(1).refresh Then MapModeAbschicken(slots(1)) 'vordergrund

            'os_objekt 81
            If os_tabelledef.tabelle.IsNothingOrEmpty Or os_tabelledef.gid.IsNothingOrEmpty Then
                slots(2).aufruf = "fehler"
            Else
                If OSrefresh Then
                    Dim OSLayer As New clsSlot
                    OSLayer.canvas = cvtop
                    OSLayer.funktion = "ObjektSuche"
                    OSLayer.mapfile = strGlobals.paradigma_hervorhebungflaechemap
                    OSLayer.slotnr = slotnrMeaning.suchobjektOSliste81
                    OSLayer.refresh = True
                    OSLayer.darstellen = True
                    OSLayer.setEmpty()
                    OSLayer.aufruf = clsAufrufgenerator.bildeAufrufEinzelOS(os_tabelledef, OSLayer.mapfile)
                    MapModeAbschicken(OSLayer) 'objektsuche 
                End If
            End If

            countererror = 4
            If suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen Then
                Dim fstTabDef As New clsTabellenDef
                If aktFST.name.IsNothingOrEmpty Then

                Else
                    If aktFST.name.Contains(".") Then
                        If clsFSTtools.extractSchemaTab(fstTabDef, aktFST.name) Then
                            fstTabDef.gid = CType(aktFST.abstract, String)
                            Dim FSTLayer As New clsSlot
                            FSTLayer.canvas = cvtop
                            FSTLayer.funktion = "ObjektSuche Flurstück"
                            FSTLayer.mapfile = strGlobals.paradigma_hervorhebungflaecheFSTmap
                            FSTLayer.slotnr = slotnrMeaning.suchObjektFlurstueck82 '82
                            FSTLayer.refresh = True
                            FSTLayer.darstellen = True
                            FSTLayer.setEmpty()
                            FSTLayer.aufruf = clsAufrufgenerator.bildeAufrufEinzelOS(fstTabDef, FSTLayer.mapfile)
                            MapModeAbschicken(FSTLayer) 'objektsuche 
                        End If

                    End If
                End If

            End If
            countererror = 5
            'imageMapCanvas.Children.Clear()
            If sollImagemapDarstellen() Then
                countererror = 51
                layerActive.masstab_imap = masstabsKorrektur(layerActive.titel, layerActive.masstab_imap)
                xkorrektur = 0.03333 * cvtop.Width
                ykorrektur = 0.04854 * cvtop.Height

                xkorrektur = cvtop.Width
                ykorrektur = cvtop.Height

                kartengen.imageMap = genImageMapTextstring()
                WebBrowser1.Width = cvtop.Width ' + 550
                WebBrowser1.Height = cvtop.Height '+ 550 
                'MsgBox("soll " & kartengen.imageMap)
                'dockMap.SetZIndex(WebBrowser1, 100)

            Else
                countererror = 52
                'MsgBox("layerActive.iswms " & layerActive.iswms)
                If layerActive.iswms Then
                    WebBrowser1.Visibility = Visibility.Collapsed
                Else
                    WebBrowser1.Visibility = Visibility.Visible
                    imagemapSchalten("")
                End If
                l("es soll keine imagemap erzeugt werden")
            End If

            If CanvasClickModus = "pan" Then
                WebBrowser1.Visibility = Visibility.Collapsed
                'Else
                '    If layerActive.iswms Then
                '        WebBrowser1.Visibility = Visibility.Collapsed ' nein stört beim panning
                '    Else
                '        WebBrowser1.Visibility = Visibility.Visible ' nein stört beim panning
                '    End If
            End If

            countererror = 6




            countererror = 7
            l("aktFST.normflst: " & aktFST.normflst.tostring(Environment.NewLine))
            Dim inselnImSuchPolygon As Integer = handleSuchOBJData(suchCanvas, aktFST.normflst.serials)

            countererror = 8
            SuchOBJNachrichtAusgeben(inselnImSuchPolygon, aktPolygon.serials.Count)
        Catch ex As Exception
            If countererror = 51 Then
            Else
                l("fehler in presentMap: " & countererror, ex)
            End If

        End Try
    End Sub

    Private Sub imagemapSchalten(imap As String)
        Dim strtest As String
        Dim cnt As Integer = 0
        Try
            l(" imagemapSchalten ---------------------- anfang")
            If imap.Contains("msQueryByRect():") Or imap.Contains("msReturnPage():") Then imap = ""
            'If imap.Contains("msQueryByRect():") Then imap = ""
            cnt = 1

            strtest = imapTemplateString.Replace("[IMAGEMAPFROMMAPSERVER]", imap)
            cnt = 2
            If imap <> String.Empty Then
                cnt = 3
                strtest = strtest.Replace("[BREITE]", CInt((xkorrektur)).ToString)
                strtest = strtest.Replace("[HOEHE]", CInt(ykorrektur).ToString)
            Else
                cnt = 4
                strtest = strtest.Replace("[BREITE]", "1")
                strtest = strtest.Replace("[HOEHE]", "1")
            End If
            cnt = 5
            strtest = strtest.Replace("[LADEVORGANGABGESCHLOSSEN]", ladevorgangAbgeschlossen.ToString)
            '  l("imagemapSchalten " & strtest)
            WebBrowser1.LoadHtml(strtest, myglobalz.myfakeurl)
            l(" imagemapSchalten ---------------------- ende")
        Catch ex As Exception
            l("vehler in imagemapSchalten: " & cnt & " , ", ex)
        End Try
    End Sub

    Private Shared Function sollImagemapDarstellen() As Boolean
        l("sollImagemapDarstellen------------------------------")
        If strGlobals.NoImageMap Then Return False
        If layerActive.iswms Then Return False
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
    End Function
    Private Function genImageMapTextstring() As String
        Dim imagemap As String = ""
        Dim hinweis As String = ""
        l("genImageMapTextstring")
        If CDbl(layerActive.masstab_imap) > aktMasstab Then
            l("genImageMapTextstring vor aufruf")
            IMAPaufruf = kartengen.ImapGenaufrufMAPserver(layerActive.mapFileHeader)
            l("genImageMapTextstring vor IMAPaufruf " & IMAPaufruf)
            initWorker()
            WebBrowser1.Visibility = Visibility.Visible
            nachricht("genImageMapTextstring b " & hinweis)
        Else
            'keineimap
            imagemap = String.Empty
            nachricht("imagemap = String.Empty b ")
        End If
        Return imagemap
    End Function
    Private Sub initWorker()
        'imapBackGroundWroker.Dispose()
        'GC.Collect()
        'imapBackGroundWroker = New BackgroundWorker
        AddHandler imapBackGroundWroker.DoWork, AddressOf imapBackGroundWroker_DoWork
        imapBackGroundWroker.WorkerSupportsCancellation = True
        imapBackGroundWroker.RunWorkerAsync()
    End Sub
    Private Sub imapBackGroundWroker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
        Dim hinweis As String = ""
        'Dim breite, hoehe As String
        'kartengen.imageMap = meineHttpNet.meinHttpJob("", IMAPaufruf, hinweis, myglobalz.enc, 5000)
        kartengen.imageMap = meineHttpNet.meinHttpJob("", IMAPaufruf, hinweis, Text.Encoding.UTF8, 5000)
        l("imapBackGroundWroker_DoWork")
        If imapBackGroundWroker.CancellationPending Then
            e.Cancel = True
            Exit Sub
        End If
        'breite =
        'hoehe =
        'MsgBox("kartengen.imageMap " & kartengen.imageMap)
        If kartengen.imageMap <> String.Empty Then
            l("imapBackGroundWroker_DoWork kartengen.imageMap <> String.Empty ")
            'l(kartengen.imageMap)
            imagemapSchalten(kartengen.imageMap)
            ' strtest = imapTemplateString.Replace("[IMAGEMAPFROMMAPSERVER]", kartengen.imageMap)
            ' My.Computer.FileSystem.WriteAllText(imapReadyFile, strtest, False)
            'Threading.Thread.Sleep(9000)
            '  WebBrowser1.Load(imapReadyFile)
            'MsgBox(strtest)  
        Else
            l("imapBackGroundWroker_DoWork kartengen.imageMap leer")
            imagemapSchalten("")
            'strtest = imapTemplateString.Replace("[IMAGEMAPFROMMAPSERVER]", "")
        End If
        'strtest = strtest.Replace("[BREITE]", CInt((xkorrektur)).ToString)
        'strtest = strtest.Replace("[HOEHE]", CInt(ykorrektur).ToString)
        'WebBrowser1.LoadHtml(strtest,   myglobalz.myfakeurl )
        e.Result = "huhu"
    End Sub


    Private Shared Function masstabsKorrektur(layertitle As String, layerImapScale As String) As String ' layerActive.titel,layerActive.masstab_imap
        Try
            If layertitle.ToLower.Contains("lurkart") Or
                layertitle.ToLower.Contains("ücksgrenzen") Then ' flurkarte
                'korrektur weil in der db selten ein wert angegeben wurde
                'layerImapScale = CType(2000, String)
            End If
            If CInt(layerImapScale) < 1 Then
                'korrektur weil in der db selten ein wert angegeben wurde
                layerImapScale = CType(10000, String)
            End If
            Return layerImapScale
        Catch ex As Exception
            l("fehler in masstabsKorrektur ", ex)
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
        If suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen Then
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
            l("warnung in handleSuchOBJData: ", ex)
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
            l("fehler in inselnEntfernen: ", ex)
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
            l("fehler in simpleList2PLuslist ", ex)
            Return Nothing
        End Try
    End Function


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
            l("Fehler in calcMiddleFromPointcollection: ", ex)
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
            'nachricht("fehler In addPolygonSchleifeKeypoints: " & Environment.NewLine &   clsParapolygon.GKstring.tostring & Environment.NewLine ,ex)
            l("fehler in addPolygonSchleifeKeypoints: " & Environment.NewLine & Environment.NewLine, ex)
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
            l("fehler in drawPolygon2Canvas", ex)
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
            l("drawPolyline2Canvas", ex)
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
            l("fehler in drawPolygon2Canvas", ex)
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



    Private Sub showAktVorgangsid()
        If aktvorgangsid.Trim.Length > 2 Then
            tbVorgangsid.Text = aktvorgangsid
            'MsgBox(aktvorgangsid)
        End If
    End Sub
    Private Sub skalieren()
        Dim pixcanvas As New clsCanvas
        pixcanvas.w = CLng(cvtop.Width)
        pixcanvas.h = CLng(cvtop.Height)
        Dim handle As New clsScalierung
        nachricht("presentMap: vor skaliereung ")
        clsScalierung.Skalierung(72, "ZB", 1, kartengen.aktMap.aktrange, CInt(pixcanvas.w), CInt(pixcanvas.h), 1,
                                           kartengen.aktMap.aktrange, pixcanvas)
        nachricht("presentMap: nach skaliereung ")
    End Sub

    Sub initdb()
        If iminternet Then Exit Sub
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
        If GisUser.nick = "hurz" Then
            pLightMsql.mydb.username = "serveradmin" : pLightMsql.mydb.password = "lkof4"
            paradigmaMsql.mydb.username = "serveradmin" : paradigmaMsql.mydb.password = "lkof4"
        End If
#End If
    End Sub
    Private Sub initVGCanvasSize()
        'Dim currentScreenIsPrimaryScreen As Boolean = True

        'hier wird auf bildschirmgröße skaliert, nicht auf die fenstergröße
        'dockTop.Height = 100

        'Width = CLng(System.Windows.SystemParameters.PrimaryScreenWidth)
        'Height = CLng(System.Windows.SystemParameters.PrimaryScreenHeight)
        'Width = CLng(System.Windows.SystemParameters.VirtualScreenWidth)
        '    Height = CLng(System.Windows.SystemParameters.VirtualScreenHeight)
        Debug.Print(Left & " " & Top)
        'cvtop.Width = CLng(System.Windows.SystemParameters.PrimaryScreenWidth) - CLng(dockMenu.Width)
        'cvtop.Height = CLng(System.Windows.SystemParameters.PrimaryScreenHeight) - CLng(dockTop.Height)
        If (Width - CLng(dockMenu.Width)) > myglobalz.maxPixelOutputSize Then
            cvtop.Width = myglobalz.maxPixelOutputSize
        Else
            cvtop.Width = (Width - CLng(dockMenu.Width))
        End If

        If (Height - CLng(dockTop.Height)) > myglobalz.maxPixelOutputSize Then
            cvtop.Height = myglobalz.maxPixelOutputSize
        Else
            cvtop.Height = Height - CLng(dockTop.Height)
        End If


        globCanvasWidth = CInt(cvtop.Width)
        globCanvasHeight = CInt(cvtop.Height)
        Debug.Print(Width & "," & Height)
        slotsResize(cvtop.Width, cvtop.Height)
        kartengen.aktMap.aktcanvas.w = CLng(cvtop.Width)
        kartengen.aktMap.aktcanvas.h = CLng(cvtop.Height)
        'cvtop.Width = CLng(Me.Width) - CLng(dockMenu.Width)
        'cvtop.Height = CLng(Me.Height) - CLng(dockTop.Height)
    End Sub

    Private Sub setCanvasSizes(sizecanvas As Canvas)
        Dim faktor = 1

        'cvtop.Width = CLng(Me.Width) - CLng(dockMenu.Width)
        'cvtop.Height = CLng(Me.Height) - CLng(dockTop.Height)

        OSmapCanvas.Width = CLng(sizecanvas.Width * faktor)
        OSmapCanvas.Height = CLng(sizecanvas.Height * faktor)
        WebBrowser1.Width = CLng(sizecanvas.Width * faktor)
        WebBrowser1.Height = CLng(sizecanvas.Height * faktor)

        suchCanvas.Width = CLng(sizecanvas.Width * faktor)
        suchCanvas.Height = CLng(sizecanvas.Height * faktor)
        kreisUebersichtCanvas.Width = CLng(sizecanvas.Width * faktor)
        kreisUebersichtCanvas.Height = CLng(sizecanvas.Height * faktor)

        cvPDFrechteck.Width = CLng(sizecanvas.Width * faktor)
        cvPDFrechteck.Height = CLng(sizecanvas.Height * faktor)

        stContext.Width = sizecanvas.Width '- 100
        stContext.Height = sizecanvas.Height ' - 100

        stContext2.Width = sizecanvas.Width '- 100
        stContext3.Width = sizecanvas.Width '- 100

        stwinthemen.Width = sizecanvas.Width
        stwinthemen.Height = sizecanvas.Height

        stpDokuUndLegende.Width = stContext.Width - stpKnoeppeVertical.Width - 50 '50 = margins
        stpObjektsuche.Width = stContext.Width - stpKnoeppeVertical.Width - 50 '50 = margins

        svMainScrollviewer.Height = sizecanvas.Height - spMenueHead.Height
        svEbenenKompakt.Height = sizecanvas.Height - spMenueHead.Height
        svGesamtExplorer.Height = sizecanvas.Height - spMenueHead.Height
        svEbenenKategorie.Height = sizecanvas.Height - spMenueHead.Height
        svEbenenSuche.Height = sizecanvas.Height - spMenueHead.Height

        lvEbenenAlle.Height = sizecanvas.Height - spMenueHead.Height
        lvEbenenKompakt.Height = sizecanvas.Height - spMenueHead.Height
        lvEbenenKategorie.Height = sizecanvas.Height - spMenueHead.Height
        lvEbenenSuche.Height = sizecanvas.Height - spMenueHead.Height
        'spMenueHead.Height
    End Sub
    Sub setMapImageSize()
        kartengen.aktMap.aktcanvas.w = CLng(cvtop.Width)
        kartengen.aktMap.aktcanvas.h = CLng(cvtop.Height)
    End Sub
    'Sub vgmyBitmapImage_DownloadCompleted(sender As Object, e As RoutedEventArgs)
    '    VGcanvasImage.Source = vgmyBitmapImage
    'End Sub
    Sub MapModeAbschicken(aslot As clsSlot)
        Dim a As String = "0"
        'l("aslot aufruf: " & aslot.aufruf)
        'l("aslot slotnr: " & aslot.slotnr)
        Try
            aslot.bitmap = New BitmapImage
            aslot.bitmap.BeginInit()
            a = "-1-"
            'aslot.bitmap.UriSource = New Uri(aslot.aufruf, UriKind.Absolute)
            Dim nuri As Uri = New Uri(aslot.aufruf, UriKind.Absolute)
            aslot.bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache
            'wg  = New BitmapImage problem absolute InteropServices.COMException (0x80072EE4
            'https://stackoverflow.com/questions/23104672/wpf-exception-when-load-a-image-by-url

            '  aslot. bitmap.CacheOption = BitmapCacheOption.OnLoad ???? nich probiert
            aslot.bitmap.CacheOption = BitmapCacheOption.None 'sollte richtig sein
            a = "-1a-"
            aslot.bitmap.UriSource = nuri
            'aslot.bitmap.UriSource = New Uri(aslot.aufruf, UriKind.Absolute)
            aslot.bitmap.EndInit()
            nuri = Nothing
            a = "2"
            'aslot.image.Source = aslot.bitmap
            If aslot.slotnr = slotnrMeaning.cvtopPanLayer80 Or
                aslot.slotnr = slotnrMeaning.suchobjektOSliste81 Or
                aslot.slotnr = slotnrMeaning.suchObjektFlurstueck82 Then
                a = "3a"
                aslot.image.Source = aslot.bitmap
                'If aslot.slotnr > 80 Then
                '    Debug.Print(CType(aslot.slotnr, String))
                'End If

                'AddHandler aslot.bitmap.DownloadCompleted, Function(sender, e) slotImage_DownloadCompleted80plus(aslot.slotnr)
                'AddHandler aslot.bitmap.DownloadFailed, Function(sender, e) slotImageDownloadFailed80plus(aslot)
            Else
                a = "3b"
                'aslot.image.Source = aslot.bitmap
                'tbinfohgrund.Visibility = Visibility.Collapsed
                AddHandler aslot.bitmap.DownloadCompleted, Function(sender, e) slotImage_DownloadCompleted(aslot)
                AddHandler aslot.bitmap.DownloadFailed, Function(sender, e) slotImageDownloadFailed(aslot)
            End If
            a = "4"
            GC.Collect()
        Catch ex As Exception
            l("fehler in MapModeAbschicken2: " & a & ", " & aslot.aufruf & " /// ", ex)
        End Try
    End Sub



    Private Function slotImageDownloadFailed(aslot As clsSlot) As EventHandler(Of ExceptionEventArgs)
        Dim info As String = "Eine " & aslot.funktion & " '" & aslot.layer.titel & "' konnte nicht erstellt werden (Timeout).  " & Environment.NewLine & Environment.NewLine
        If aslot.layer.titel.ToLower.Contains("wms") Then
            info &= "Hinweis: WMS - Ebenen können von seiten des Anbieters (" & aslot.layer.ldoku.datenabgabe & ") zeitweise deaktiviert werden."
        Else

        End If
        info &= "Tipp: " & Environment.NewLine &
                " Probieren Sie es noch einmal (Auffrischen Taste) oder " & Environment.NewLine & Environment.NewLine &
                "  schalten sie diese Ebene vorübergehend aus !" & Environment.NewLine & Environment.NewLine &
                "  Der Admin wird informiert und kümmert sich um das Problem." & Environment.NewLine &
                "" & Environment.NewLine
        MessageBox.Show(info, "Hoppla")
        Return Nothing
        ' l("fehler ACHTUNG! Ebene ist defekt, bitte korrigieren: " & slots(slotnr).funktion & " " & slots(slotnr).layer.titel & " aid: " & slots(slotnr).layer.aid)
    End Function
    Private Function slotImage_DownloadCompleted(aslot As clsSlot) As EventHandler(Of ExceptionEventArgs)
        aslot.image.Source = aslot.bitmap
        tbinfohgrund.Visibility = Visibility.Collapsed

        Return Nothing
    End Function
    Private Sub zoomin_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'neue range berechnen
        'darstellen
        myglobalz.mgisBackModus = False
        panningAusschalten()
        resizeWindow()
        Dim breite As Double = kartengen.aktMap.aktrange.xdif()
        kartengen.aktMap.aktrange.xl = kartengen.aktMap.aktrange.xl + (breite / 3)
        kartengen.aktMap.aktrange.xh = kartengen.aktMap.aktrange.xh - (breite / 3)
        Dim hohe As Double = kartengen.aktMap.aktrange.ydif()
        kartengen.aktMap.aktrange.yl = kartengen.aktMap.aktrange.yl + (hohe / 3)
        kartengen.aktMap.aktrange.yh = kartengen.aktMap.aktrange.yh - (hohe / 3)
        refreshMap(True, True) ' 
    End Sub

    Private Sub zoomout_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        myglobalz.mgisBackModus = False
        panningAusschalten()
        resizeWindow()
        'eigentuemerfunktionAusschalten()
        Dim breite As Double = kartengen.aktMap.aktrange.xdif()
        kartengen.aktMap.aktrange.xl = kartengen.aktMap.aktrange.xl - (breite / 3)
        kartengen.aktMap.aktrange.xh = kartengen.aktMap.aktrange.xh + (breite / 3)
        Dim hohe As Double = kartengen.aktMap.aktrange.ydif()
        kartengen.aktMap.aktrange.yl = kartengen.aktMap.aktrange.yl - (hohe / 3)
        kartengen.aktMap.aktrange.yh = kartengen.aktMap.aktrange.yh + (hohe / 3)
        refreshMap(True, True)
    End Sub
    Private Sub globalfit_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        myglobalz.mgisBackModus = False
        panningAusschalten()
        resizeWindow()
        kreisUebersichtCanvas.Visibility = Visibility.Visible
#Disable Warning BC42025 ' Access of shared member, constant member, enum member or nested type through an instance
        dockMap.SetZIndex(kreisUebersichtCanvas, 0)
#Enable Warning BC42025 ' Access of shared member, constant member, enum member or nested type through an instance
        imgpin.Visibility = Visibility.Collapsed

        kartengen.aktMap.aktrange = clsStartup.setMapKreisRange()
        refreshMap(True, True)
    End Sub

    Private Sub drawAktRange2Uebersicht(aktrange As clsRange)
        'Dim myBrush As SolidColorBrush
        Dim punkteCanvas, lu, ro, luPix, roPix As New myPoint
        Dim UeKanwas As New clsCanvas
        Dim kreisrange As New clsRange
        Try
            l("drawAktRange2Uebersicht---------------------- anfang")
            'kreisUebersichtCanvas.Children.Clear()
            kreisUebersichtCanvas.Children.Clear()
            'kreisUebersichtCanvas.Children.Remove(aktrangebox)
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
            kreisUebersichtCanvas.Children.Add(imgkreisuebersicht)
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
            l("Fehler in drawAktRange2Uebersicht: ", ex)
        End Try
    End Sub

    Private Sub rbfit_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        panningAusschalten()
        imgpin.Visibility = Visibility.Collapsed
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

            endpt = e.GetPosition(cvtop)


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
        aktrangebox.Width = 0
        aktrangebox.Height = 0
        Mouse.Capture(Nothing)
        RubberbandStartpt = Nothing
        RubberbandEndpt = Nothing
        'Me.Cursor = Nothing
        WebBrowser1.Visibility = Visibility.Visible
        refreshMap(True, True)
        'Me.Cursor = System.Windows.InputCursors.ArrowCD
    End Sub
    Sub setBoundingRefresh(ByVal myrange As clsRange) 'ByVal xl As Double, ByVal xh As Double, ByVal yl As Double, ByVal yh As Double)
        kartengen.aktMap.aktrange.rangekopierenVon(myrange)
        'xdifKorrektur
        If kartengen.aktMap.aktrange.xdif() < 1 Then kartengen.aktMap.aktrange.xh += 1
        If kartengen.aktMap.aktrange.ydif() < 1 Then kartengen.aktMap.aktrange.yh += 1
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
            l("Daneben. Bitte nochmal probieren!")
        End Try
    End Sub

    Private Sub RubberbandStart(ByVal e As System.Windows.Input.MouseButtonEventArgs)
        RubberbandStartpt = e.GetPosition(WebBrowser1)
        'Me.Cursor = System.Windows.Input.Cursors.Cross
    End Sub
    Private Sub chkBoxPan_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim icount As Integer = 0
        Try
            l(" MOD chkBoxPan_Click anfang")

            resizeWindow()
            imgpin.Visibility = Visibility.Collapsed
            myglobalz.mgisBackModus = False
            'eigentuemerfunktionAusschalten()
            If chkBoxPan.IsChecked Then
                zeichneOverlaysGlob = True : zeichneImageMapGlob = False
                CanvasClickModus = "pan"
                WebBrowser1.Visibility = Visibility.Collapsed
                cvtop.Cursor = System.Windows.Input.Cursors.ScrollAll
                Debug.Print(cvtop.Visibility & ", " & cvtop.IsEnabled)
                chkBoxPan.IsEnabled = True
                brdPan.IsEnabled = True
                spButtonMenu.ToolTip = "Bitte zuerst den Verschiebemodus beenden"

            End If
            If Not chkBoxPan.IsChecked Then
                pannauss()
                refreshMap(True, True)
            End If
            l(" MOD chkBoxPan_Click ende")
        Catch ex As Exception
            l("Fehler in chkBoxPan_Click: ", ex)
        End Try
    End Sub

    Private Sub pannauss()
        zeichneOverlaysGlob = True : zeichneImageMapGlob = True
        'refreshMap(True, True)
        If layerActive.iswms Then
            panningAusschalten()
            WebBrowser1.Visibility = Visibility.Collapsed
            cvtop.Cursor = Cursors.Hand
            CanvasClickModus = "wmsdatenabfrage"
        Else
            cvtop.Cursor = Cursors.Arrow
            CanvasClickModus = ""
            WebBrowser1.Visibility = Visibility.Visible
        End If
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
        PDF_postition_desRahmensAbfragen(auswahlRechteck)
        '  PDF_druckMassStab = calcPDFMassstab(PDF_PrintRange, CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked))

        e.Handled = True
    End Sub

    Private Function PDF_postition_desRahmensAbfragen(Rechteck As Rectangle) As Double
        Try
            l("PDF_postition_desRahmensBestimmen---------------------- anfang")
            Dim aleft As Double = Canvas.GetLeft(Rechteck)
            If Double.IsNaN(aleft) Then
                Return 0
            End If

            Dim btop As Double = Canvas.GetTop(Rechteck) '- 21
            '21 zuviel bei toop
            Debug.Print(cvtop.Width & " " & cvtop.Height & ", " & Rechteck.Width & ", " & Rechteck.Height)

            Dim pixRange As New clsRange
            pixRange.xl = aleft
            pixRange.xh = aleft + Rechteck.Width
            pixRange.yl = btop '- Hoehe_desTabcontrols
            pixRange.yh = btop + Rechteck.Height '- Hoehe_desTabcontrols
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

            'Dim mas As Double
            'mas = calcPDFMassstab(PDF_PrintRange)
            'Return mas
            Return 1
            l("PDF_postition_desRahmensBestimmen---------------------- ende")
        Catch ex As Exception
            l("Fehler in PDF_postition_desRahmensBestimmen: ", ex)
            Return 0
        End Try
    End Function

    Function calcPDFMassstab(PDF_PrintRangeTemp As clsRange, a4format As Boolean, querformat As Boolean) As Double
        Dim mas As Double
        Dim aktCV As New clsCanvas
        If PDF_PrintRange.xdif < 1 Then
            Return 0
        End If
        If a4format Then
            aktCV = dina4InMM
        Else
            aktCV = dina3InMM
        End If
        If querformat Then
            mas = PDF_PrintRange.xdif
            mas = mas * 100
            mas = mas / ((aktCV.w) / 10) '28 '28 cm ist die breite des rahmens auf papier
        Else
            mas = Math.Abs(PDF_PrintRange.ydif)
            mas = mas * 100
            mas = mas / ((aktCV.w) / 10) '29.7 '28 '28 cm ist die breite des rahmens auf papier
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
            If (dragOffset.X + (auswahlRechteck.Width)) > cvtop.Width Then
                Canvas.SetLeft(auswahlRechteck, cvtop.Width - auswahlRechteck.Width)
            Else
                '  Canvas.SetLeft(myRect, dragOffset.X)
            End If


            If dragOffset.Y < 0 Then
                Canvas.SetTop(auswahlRechteck, 0)
            Else
                Canvas.SetTop(auswahlRechteck, dragOffset.Y)
            End If
            If (Canvas.GetTop(auswahlRechteck)) + auswahlRechteck.Height > cvtop.Height - dockTop.Height Then
                Canvas.SetTop(auswahlRechteck, cvtop.Height - auswahlRechteck.Height - dockTop.Height)
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
        'tbFavoname.Text = Canvas.GetTop(auswahlRechteck) & " , " & cvtop.Height & ", " & (Canvas.GetTop(auswahlRechteck) + auswahlRechteck.Height)
        e.Handled = True
    End Sub

    Private Sub myCanvas_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) _
        Handles cvtop.MouseLeftButtonDown, cvtop.MouseLeftButtonDown
        Select Case CanvasClickModus.ToLower
            Case "ausschnitt"
                If chkBoxAusschnitt.IsChecked Then RubberbandStart(e)
            Case "pan"
                isDraggingFlag = True
                origContentMousePoint = e.GetPosition(cvtop)
        End Select
        e.Handled = True
    End Sub
    Private Sub canvas1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) _
        Handles cvtop.MouseMove, cvtop.MouseMove
        Select Case CanvasClickModus.ToLower
            Case "ausschnitt"
                If chkBoxAusschnitt.IsChecked Then RubberbandMove(e)
            Case "pan"
                If isDraggingFlag Then
                    Dim dragOffset As Vector
                    curContentMousePoint = e.GetPosition(cvtop)
                    dragOffset = curContentMousePoint - origContentMousePoint
                    For i = 0 To 30
                        If slots(i).refresh Then
                            Canvas.SetTop(slots(i).image, dragOffset.Y)
                            Canvas.SetLeft(slots(i).image, dragOffset.X)
                        End If

                    Next


                    'Canvas.SetTop(slots(1).image, dragOffset.Y)
                    'Canvas.SetLeft(slots(1).image, dragOffset.X)

                    'Canvas.SetTop(slots(2).image, dragOffset.Y)
                    'Canvas.SetLeft(slots(2).image, dragOffset.X)
                    tbMinimapCoordinate2.Text = CType(curContentMousePoint.X, String)
                End If
            Case "strecke"
                Dim tempPT As New Point?
                Dim winpt As New Point
                Dim delim As String = ";"
                tempPT = e.GetPosition(cvtop)
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


    Private Sub canvas1_MouseLeftButtonUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles cvtop.MouseLeftButtonUp, cvtop.MouseLeftButtonUp
        e.Handled = True
        Select Case CanvasClickModus.ToLower
            Case "ausschnitt"
                If chkBoxAusschnitt.IsChecked Then RubberbandFinish()
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                If layerActive.iswms Then
                    panningAusschalten()
                    WebBrowser1.Visibility = Visibility.Collapsed
                    cvtop.Cursor = Cursors.Hand
                    CanvasClickModus = "wmsdatenabfrage"
                Else
                    CanvasClickModus = ""
                    WebBrowser1.Visibility = Visibility.Visible
                End If

            Case "wmsdatenabfrage"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cvtop)
                Dim bbox As String = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt).Replace(" ", "")
                bbox = clsWMStools.calcVollstBbox(bbox)
                If layerActive.wmsProps.stdlayer = "dummy" Then
                    Dim utmpt As Point = clsMiniMapTools.makeUTM(KoordinateKLickpt)
                    os_tabelledef = clsMiniMapTools.makeTabname(layerActive)
                    Dim javascriptMimikry = clsMiniMapTools.genjavascriptMimikry(os_tabelledef.aid, os_tabelledef.tab_nr, os_tabelledef.gid)
                    'FS feststellen
                    aktFST.clear()
                    aktFST.punkt.X = utmpt.X
                    aktFST.punkt.Y = utmpt.Y
                    aktFST.normflst.FS = pgisTools.getFS4UTM(utmpt)
                    aktFST.normflst.splitFS(aktFST.normflst.FS)
                    aktFST.abstract = aktFST.normflst.gemarkungstext & ", Flur: " & aktFST.normflst.flur & ", Fst: " & aktFST.normflst.fstueckKombi
                    fensterzaehler += 1 : If fensterzaehler = 5 Then fensterzaehler = 1
                    clsMiniMapTools.handleMouseDownImagemap(KoordinateKLickpt, javascriptMimikry, fensterzaehler)
                Else
                    Dim uuu As New winWMShtmlDB(800, 990, layerActive, bbox, CInt(cvtop.Height), CInt(cvtop.Width),
                                           CInt(KoordinateKLickpt.Value.X), CInt(KoordinateKLickpt.Value.Y),
                                           layerActive.wmsProps.stdlayer, layerActive.wmsProps.stdlayer, wmsfensterzaehler)
                    uuu.Show()
                    wmsfensterzaehler += 1 : If wmsfensterzaehler = 5 Then wmsfensterzaehler = 1
                End If
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
            Case "pointactivemodus"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cvtop)
                Dim bbox As String = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt).Replace(" ", "")
                Dim a() As String = bbox.Split(","c)
                Dim utmpt As New Point
                utmpt.X = (CDbl(a(0).Replace(".", ",")))
                utmpt.Y = (CDbl(a(1).Replace(".", ",")))
                Debug.Print("" & layerActive.dokutext)
                If layerActive.tabname.IsNothingOrEmpty Then
                    os_tabelledef = New clsTabellenDef
                    os_tabelledef.tab_nr = CType(1, String)
                    os_tabelledef = ModsachdatenTools.getSChemaDB(layerActive.aid, 1)
                    If os_tabelledef Is Nothing Then
                        l("Fehler in pointactivemodus i: " & "," & layerActive.aid)
                    End If
                    os_tabelledef.aid = CStr(layerActive.aid)
                    os_tabelledef.gid = "0"
                    os_tabelledef.datenbank = "postgis20"
                    layerActive.tabname = os_tabelledef.tabelle
                End If

                Dim fangRadiusInMeter = clsSachdatentools.calcFangradiusM(CInt(cvtop.Width),
                                            myglobalz.fangradius_in_pixel,
                                            kartengen.aktMap.aktrange.xdif, "")
                Dim gids As List(Of Integer) = clsSachdatentools.getActiveLayer4point(utmpt, layerActive.aid,
                                                                     CInt(cvtop.Width), CInt(cvtop.Height),
                                                                     KoordinateKLickpt, fangRadiusInMeter, os_tabelledef)
                If gids Is Nothing Or gids.Count < 1 Then
                    l("fehler in    Case 'pointactivemodus': gids is nothing , vermutlich ist layerActive.aid =0 : " & layerActive.aid)
                End If

                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True

                If layerActive.iswms Then
                    panningAusschalten()
                    WebBrowser1.Visibility = Visibility.Collapsed
                    cvtop.Cursor = Cursors.Hand
                    CanvasClickModus = "wmsdatenabfrage"
                Else
                    WebBrowser1.Visibility = Visibility.Visible
                    CanvasClickModus = ""
                End If
            Case "dossiermodus"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cvtop)
                Dim bbox As String = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt).Replace(" ", "")
                Dim a() As String = bbox.Split(","c)
                Dim utmpt As New Point
                utmpt.X = (CDbl(a(0).Replace(".", ",")))
                utmpt.Y = (CDbl(a(1).Replace(".", ",")))




                clsSachdatentools.getdossier(utmpt, layerActive.aid,
                                            CInt(cvtop.Width), CInt(cvtop.Height),
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
                suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                If layerActive.iswms Then
                    panningAusschalten()
                    WebBrowser1.Visibility = Visibility.Collapsed
                    cvtop.Cursor = Cursors.Hand
                    CanvasClickModus = "wmsdatenabfrage"
                Else
                    WebBrowser1.Visibility = Visibility.Visible
                    CanvasClickModus = ""
                End If

                refreshMap(True, True)
            Case "koordinate"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cvtop)
                'CanvasClickModus = ""
                'Dim temp = koordinateKlickBerechnen(KoordinateKLickpt) & " [m]"
                tbzwischenwert.Text = "Ihre UTM-Koordinate:"
                tbMinimapCoordinate2.Text = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint
                btnNeueMessung.IsEnabled = True
                btnMessen.IsOpen = True

                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                punktNachParadigmaUebernehemn()
                btnMessen.IsOpen = True

                If layerActive.iswms Then
                    panningAusschalten()
                    WebBrowser1.Visibility = Visibility.Collapsed
                    cvtop.Cursor = Cursors.Hand
                    CanvasClickModus = "wmsdatenabfrage"
                Else
                    WebBrowser1.Visibility = Visibility.Visible
                    CanvasClickModus = ""
                End If
            Case "windrose"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cvtop)
                CanvasClickModus = ""
                'Dim temp = koordinateKlickBerechnen(KoordinateKLickpt) & " [m]"
                tbMinimapCoordinate2.Text = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint

                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True
                windroseBerechnen(aktGlobPoint)
                If layerActive.iswms Then
                    panningAusschalten()
                    WebBrowser1.Visibility = Visibility.Collapsed
                    cvtop.Cursor = Cursors.Hand
                    CanvasClickModus = "wmsdatenabfrage"
                Else
                    WebBrowser1.Visibility = Visibility.Visible
                    CanvasClickModus = ""
                End If
            Case "kreisimabstand"
                Mouse.Capture(Nothing)
                KoordinateKLickpt = e.GetPosition(cvtop)
                CanvasClickModus = ""
                Dim kreisimabstand_Radius_pixel As Double = Nothing
                kreisimabstand_Radius_pixel = (KreislinienRadius * cvtop.Width) / kartengen.aktMap.aktrange.xdif

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
                cvtop.Children.Add(myEllipse)
                cvtop.SetLeft(myEllipse, KoordinateKLickpt.Value.X - (kreisimabstand_Radius_pixel / 2))
                cvtop.SetTop(myEllipse, KoordinateKLickpt.Value.Y - (kreisimabstand_Radius_pixel / 2))





                'Dim temp = koordinateKlickBerechnen(KoordinateKLickpt) & " [m]"
                'tbMinimapCoordinate2.Text = koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint
                If layerActive.iswms Then
                    panningAusschalten()
                    WebBrowser1.Visibility = Visibility.Collapsed
                    cvtop.Cursor = Cursors.Hand
                    CanvasClickModus = "wmsdatenabfrage"
                Else
                    WebBrowser1.Visibility = Visibility.Visible
                    CanvasClickModus = ""
                End If
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = True


            Case "flaeche"
                Dim tempPT As Point? = Nothing
                Dim winpt As New Point
                tempPT = e.GetPosition(cvtop)
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
                tempPT = e.GetPosition(cvtop)
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
                btnNeueMessung.Visibility = Visibility.Visible
                btnNeueMessung.IsEnabled = True
            Case "pan"
                CanvasClickModus = "pan" ' bleibt!!
                isDraggingFlag = False
                Dim dragOffset As Vector = curContentMousePoint - origContentMousePoint
                Dim neuerBildschirmMittelPunktInPoints As Point
                neuerBildschirmMittelPunktInPoints = New Point() With {.X = ((cvtop.Width) / 2) - dragOffset.X,
                                                                       .Y = (cvtop.Height / 2) - dragOffset.Y}
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
                zeichneOverlaysGlob = True
                zeichneImageMapGlob = False
                neuerMittelPunktInUTM = Nothing
                Debug.Print("wb " & WebBrowser1.Visibility)
        End Select
    End Sub
    Private Sub windroseBerechnen(pt As myPoint)
        If pt.X < 1 Or pt.Y < 1 Then
            MsgBox("Sie haben keine gültige Koordinate." &
                   " Somit ist es nicht möglich eine Windrose zu bekommen!")
            Exit Sub
        Else
            Dim windrosenHyperlink As String = clsWindrose.GetWindrosenHyperlink(pt.X, pt.Y)

            panningAusschalten()
            tiWindrose.Visibility = Visibility.Visible
            dpvogel.Visibility = Visibility.Visible
            tigis.Visibility = Visibility.Collapsed
            tiWindrose.IsSelected = True
            panningAusschalten()
            webBrowserControlWindrose.Navigate(windrosenHyperlink)


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
        WebBrowser1.Visibility = Visibility.Collapsed
        tbzwischenwert.Visibility = Visibility.Collapsed
        'clsMiniMapTools.VisibilityDerKinderschalten(myCanvas2, Windows.Visibility.Collapsed)
        zeichneOverlaysGlob = True : zeichneImageMapGlob = False
        'gisDarstellenAlleEbenen()
        DrawPolylinie(cvtop)
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
        e.Handled = True
        'Exit Sub
        Try
            l("btnGetFlaecheEnde_Click---------------------- anfang")
            panningAusschalten()
            WebBrowser1.Visibility = Visibility.Visible 'löscht auch die Darstellung auf der Karte!"!!!
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
                    myPolyLoeschen("flaeche")
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
            If aktPolygon.GKstring.IsNothingOrEmpty Then
                l("fehler Geometrie nicht verwendbar.")
            Else
                'polyGeometrieNachParadigmaUebernehmen()
                lastGeomAsWKT = serialGKStringnachWKT(aktPolygon.GKstring, CanvasClickModus)
                l("lastGeomAsWKT: " & lastGeomAsWKT)
            End If
            l("CanvasClickModus " & CanvasClickModus)
            'CanvasClickModus = ""
            cmbMessen.SelectedItem = Nothing
            l("btnGetFlaecheEnde_Click---------------------- ende")
        Catch ex As Exception
            l("Fehler in btnGetFlaecheEnde_Click: ", ex)
        End Try

    End Sub
    Sub punktNachParadigmaUebernehemn()
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            If aktvorgangsid <> String.Empty Then
                Dim mesred As MessageBoxResult = userWill("Punkt")
                If mesred = MessageBoxResult.Yes Then
                    If modParadigma.punktNachParadigma(aktGlobPoint) Then
                        clsToolsAllg.userlayerNeuErzeugen(GisUser.nick, myglobalz.aktvorgangsid)
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
                        clsToolsAllg.userlayerNeuErzeugen(GisUser.nick, myglobalz.aktvorgangsid)
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
            myPolyLoeschen("strecke")
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
    Private Sub myPolyLoeschen(typ As String)
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
            l("Fehler inmyPolyFinish : ", ex)
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

        DrawPolygon(cvtop)
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
        WebBrowser1.Visibility = Visibility.Collapsed
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
        WebBrowser1.Visibility = Visibility.Collapsed
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
        WebBrowser1.Visibility = Visibility.Collapsed
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
        WebBrowser1.Visibility = Visibility.Collapsed
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
            suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen
            '-----------------------------------
            'btnSuchobjAusSchalten.Visibility = Visibility.Visible
            setBoundingRefresh(kartengen.aktMap.aktrange)
        End If
        utm = Nothing
        refreshMap(True, True)
    End Sub

    Private Sub adresssuche()
        nachricht("USERAKTION: adr suchen ")
        Dim adrs As New winDetailAdressSuche
        adrs.ShowDialog()
        Dim ergebnis As Boolean = CBool(adrs.retunrvalue)
        l(adrs.strasseOhneHausnr.ToString)
        If ergebnis Then
            If adrs.strasseOhneHausnr Then
                zeigeAdresseOhneHausNr(adrs)
            Else
                zeigeAdresseMitHausNr()
            End If
        Else
            'btnSuchobjAusSchalten.Visibility = Visibility.Collapsed
        End If
        adrs = Nothing
        refreshMap(True, True)
        'cmbSuchen.SelectedIndex = 0
    End Sub

    Private Shared Sub zeigeAdresseOhneHausNr(adrs As winDetailAdressSuche)
        Dim hinweis As String = ""
        Dim result As String
        Dim SQL, innersql As String

        l("zeigeAdresseOhneHausNr: " & adrs.strasseOhneHausnr.ToString & aktadr.ToString)
        OSrefresh = True
        os_tabelledef.Schema = "flurkarte"
        os_tabelledef.tabelle = "haloschneise"
        innersql = "SELECT gid FROM " & os_tabelledef.Schema & "." & os_tabelledef.tabelle &
                 " where strcode=" & aktadr.Gisadresse.strasseCode &
                 " And   gemeindenr='" & aktadr.Gisadresse.gemeindeNrBig() & "'" & " limit 1"

        result = clsToolsAllg.getSQL4Http(innersql, "postgis20", hinweis, "getsql") : l(hinweis)
        result = result.Trim
        '  os_tabelledef.gid = result.Replace("$", "").Replace(vbCrLf, "")
        os_tabelledef.gid = result
        Dim tab As String = ""
        Dim gids() As String
        gids = os_tabelledef.gid.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries)
        If gids.Count > 1 Then
            'mehrfachtreffer
            os_tabelledef.gid = gids(0)
        Else
            'nur ein treffer
            os_tabelledef.gid = gids(0)
        End If

        tab = clsADRtools.getAdressTab4GID(CInt(os_tabelledef.gid))
        'SQL = "SELECT ST_EXTENT(geom) FROM " & os_tabelledef.Schema & "." & os_tabelledef.tabelle & " where gid in (" & innersql & ")"
        SQL = "SELECT ST_EXTENT(geom) FROM " & tab & " where gid in (" & os_tabelledef.gid & ")"
        Dim boxString As String = ""
        Dim xl, xh, yl, yh As Double

        boxString = clsADRtools.getBox4Adresses(hinweis, result, SQL, innersql)

        If boxString.IsNothingOrEmpty Then
            'l("warnung in rechtsHochwertHolen box ist leer " & pFST.FS)
            'pFST.GKrechts = 0
        Else
            Dim puffer = 100
            If postgisBOX2range(boxString, xl, xh, yl, yh) Then
                kartengen.aktMap.aktrange.xl = xl
                kartengen.aktMap.aktrange.xh = xh
                kartengen.aktMap.aktrange.yl = yl
                kartengen.aktMap.aktrange.yh = yh
                kartengen.aktMap.aktrange.addBuffer(puffer)
                aktGlobPoint.X = kartengen.aktMap.aktrange.xcenter
                aktGlobPoint.Y = kartengen.aktMap.aktrange.ycenter
                aktGlobPoint.strX = CType(aktGlobPoint.X, String)
                aktGlobPoint.strY = CType(aktGlobPoint.Y, String)
            Else
                l("Fehler in rechtsHochwertHolen keine box gefunden ")
            End If
        End If
    End Sub

    Private Sub zeigeAdresseMitHausNr()
        'mithausnr
        Debug.Print("")
        aktPolygon.ShapeSerial = holePUFFERPolygonFuerPoint(aktGlobPoint, 30) 'pufferinMeter)
        aktPolygon.originalQuellString = aktPolygon.ShapeSerial
        aktFST.normflst.serials.Clear()
        aktFST.normflst.serials.Add(aktPolygon.ShapeSerial)
        suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen
        setBoundingRefresh(kartengen.aktMap.aktrange)
    End Sub

    Private Function getGID4Street(aktadr As ParaAdresse, schema As String, tabelle As String) As String
        l("getGID4Street")
        Dim hinweis As String = ""
        Dim summe As String = "#"
        Dim Sql As String
        Try
            'Sql = "SELECT gid,ST_EXTENT(geom)  FROM   " & schema & "." & tabelle &
            Sql = "SELECT gid FROM " & schema & ".""" & tabelle & """" &
                         " where strcode=" & aktadr.Gisadresse.strasseCode &
                         " And   gemeindenr='" & aktadr.Gisadresse.gemeindeNrBig() & "'"
            l(Sql)
            Dim dt As DataTable
            dt = getDTFromWebgisDB(Sql, "postgis20")
            If dt.Rows.Count < 1 Then
                Return "0"
            Else
                l("getGID4fs fertig")

                For i = 0 To dt.Rows.Count - 1
                    summe = summe & "," & ((clsDBtools.fieldvalue(dt.Rows(i).Item(0))))
                Next
                summe = summe.Replace("#,", "")
                Return summe 'CInt((clsDBtools.fieldvalue(dt.Rows(0).Item(0))))
            End If
        Catch ex As Exception
            l("fehler in getGID4fs: ", ex)
            Return "-1"
        End Try
    End Function

    Private Sub flurstueckssuche()
        nachricht("USERAKTION: flst suchen ")
        Dim flst As New WinDetailSucheFST("ort")
        flst.ShowDialog()
        If CBool(flst.returnValue) Then
            If flst.historyLast Then
                getSerialFromPostgis(aktFST.normflst.FS, True, myglobalz.histFstView)
                aktFST.abstract = getGID4fs(aktFST.normflst.FS, True, myglobalz.histFstView)
                aktFST.name = myglobalz.histFstView
            Else
                aktFST.abstract = getGID4fs(aktFST.normflst.FS, False, WinDetailSucheFST.AktuelleBasisTabelle)
                aktFST.name = WinDetailSucheFST.AktuelleBasisTabelle
                'getSerialFromPostgis(aktFST.normflst.FS, False, WinDetailSucheFST.AktuelleBasisTabelle) ' setzt  aktFST.serial 
            End If
            Dim anzahlFflurstuecksteile As Integer = 0
            If flst.historyLast Then
                If aktFST.abstract.Contains(",") Then
                    Dim a() As String
                    a = aktFST.abstract.Split(","c)
                    anzahlFflurstuecksteile = a.Count
                Else
                    anzahlFflurstuecksteile = 1
                End If

                If anzahlFflurstuecksteile > 1 Then
                    MessageBox.Show("Dieses Flurstück bestand in den Jahren " & aktFST.abstract & ".  ", "Wichtiger Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            Else
                If aktFST.abstract.Contains(",") Then
                    Dim a() As String
                    a = aktFST.abstract.Split(","c)
                    anzahlFflurstuecksteile = a.Count
                Else
                    anzahlFflurstuecksteile = 1
                End If
                If anzahlFflurstuecksteile > 1 Then
                    MessageBox.Show("Dieses Flurstück besteht aus " & anzahlFflurstuecksteile & " Teilen!!!", "Wichtiger Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            End If
            'btnSuchobjAusSchalten.Visibility = Visibility.Visible
            'kartengen.aktMap.aktrange wurde im formular vorher schon gesetzt
            setBoundingRefresh(kartengen.aktMap.aktrange)
            suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen
            refreshMap(True, True)
        Else
        End If

        'cmbSuchen.SelectedIndex = 0
    End Sub



    Private Sub btngoogle3d_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        aktGlobPoint.X = kartengen.aktMap.aktrange.xcenter
        aktGlobPoint.Y = kartengen.aktMap.aktrange.ycenter
        Dim intern = clsOptionTools.Muss3DinternOeffnenOeffnen()
        intern = False
        If intern Then
            If wb3Disinit Then
                google3dintro()
            End If
            panningAusschalten()
            dpvogel.Visibility = Visibility.Visible
            tigis.Visibility = Visibility.Collapsed
            tiWindrose.Visibility = Visibility.Collapsed

            ti3D.IsSelected = True
            panningAusschalten()
        Else

            google3dintroExtern()
        End If


    End Sub

    Private Sub google3dintro()
        Dim gis As New clsGISfunctions
        Dim result As String = ""
        Dim nbox As New clsRange
        Dim longitude, latitude As String
        panningAusschalten()
        Try
            nachricht("USERAKTION: googlekarte  vgoogle3dintro")
            panningAusschalten()
            Dim radius = 300
            nbox.xl = CInt(aktGlobPoint.X) - radius
            nbox.yl = CInt(aktGlobPoint.Y) - (radius * 2)
            nbox.xh = CInt(aktGlobPoint.X) + radius
            nbox.yh = CInt(aktGlobPoint.Y)
            result = gis.GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(nbox, False, longitude, latitude)
            If result = "fehler" Or result = "" Then
            Else
                '   Process.Start(strGlobals.chromeFile, result)
                dpvogel.Visibility = Visibility.Visible
                tigis.Visibility = Visibility.Collapsed
                panningAusschalten()
                ti3D.IsSelected = True
                wb3D.Load((result))
                wb3Disinit = True
            End If
            gis = Nothing
        Catch ex As Exception
            l("fehler in starteWebbrowserControl: " & result, ex)
        End Try
    End Sub
    Private Sub google3dintroExtern()
        Dim gis As New clsGISfunctions
        Dim result As String = ""
        Dim nbox As New clsRange
        Dim longitude, latitude As String
        'panningAusschalten()
        Try
            nachricht("USERAKTION: googlekarte  vgoogle3dintro")
            'panningAusschalten()
            Dim radius = 300
            nbox.xl = CInt(aktGlobPoint.X) - radius
            nbox.yl = CInt(aktGlobPoint.Y) - (radius * 2)
            nbox.xh = CInt(aktGlobPoint.X) + radius
            nbox.yh = CInt(aktGlobPoint.Y)
            result = gis.GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(nbox, False, longitude, latitude)
            If result = "fehler" Or result = "" Then
            Else
                Process.Start(strGlobals.chromeFile, result)
                'dpvogel.Visibility = Visibility.Visible
                'tigis.Visibility = Visibility.Collapsed
                'panningAusschalten()
                'ti3D.IsSelected = True
                'wb3D.Load((result))
                'wb3Disinit = True
            End If
            gis = Nothing
        Catch ex As Exception
            l("fehler in google3dintroExtern: " & result, ex)
        End Try
    End Sub

    Private Sub btnVogel_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        refreshVogel()
    End Sub

    Private Sub refreshVogel()
        panningAusschalten()
        dpvogel.Visibility = Visibility.Visible
        tigis.Visibility = Visibility.Collapsed
        panningAusschalten()
        If wbvogelisinit Then
            Dim uncdatei As String = clsStartup.calcURI4vogel
            wbvogel.Load(uncdatei)
        End If
        tivogel.IsSelected = True
    End Sub

    Private Sub btnzurueckZumGis_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        dpvogel.Visibility = Visibility.Collapsed
        tigis.Visibility = Visibility.Visible
        GC.Collect()
    End Sub

    Private Sub btnAddLayer_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'holeExplorer()
        MessageBox.Show("Bitte nutzen Sie für die Suche nach Ebenen die Stichwortsuche oder den Explorer-Reiter unten.")
        tiGesamtgExplorer.IsSelected = True
    End Sub

    Public Sub makeThemenVis()
        stContext.Visibility = Visibility.Collapsed
        stwinthemen.Visibility = Visibility.Visible
    End Sub

    Private Sub makeThemenInVis()
        stContext.Visibility = Visibility.Collapsed
        stwinthemen.Visibility = Visibility.Collapsed
    End Sub

    Private Sub lvEbenenAlle_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        Exit Sub
        'If lvEbenenAlle.SelectedItem Is Nothing Then Exit Sub
        'If lvEbenenAlle.SelectedValue Is Nothing Then Exit Sub 
    End Sub
    Private Sub chkauswahlgeaendert(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        panningAusschalten()
        resizeWindow()
        Dim nck As CheckBox = CType(sender, CheckBox)
        Dim action As String = If(nck.IsChecked, "add", "sub")
        Dim pickAid As Integer = CInt(CStr(nck.Tag))

        If action = "sub" Then
            entferneEbeneauslayersSelected(pickAid)
        End If
        ebenenListeAktualisieren()
        Debug.Print("action " & action & " aid: " & CStr(pickAid))
        If action = "sub" Then
            entferneEbeneAusSlots(pickAid)
        End If
        If action = "add" Then
            'layer bilden
            zwischenbildBitteWarten()
            Dim nlay As New clsLayerPres
            nlay.aid = pickAid
            pgisTools.getStamm4aid(nlay)
            Dim aktslot = SlotTools.getEmptySlot()
            slots(aktslot).mapfile = nlay.mapFile.Replace("layer.map", "header.map")
            slots(aktslot).refresh = True
            slots(aktslot).darstellen = True
            slots(aktslot).layer = nlay.kopie
            'job abschicken
            slots(aktslot).BildGenaufrufMAPserver(slots(aktslot).mapfile, myglobalz.serverWeb, kartengen.aktMap, slots(aktslot).layer.isUserlayer)
            MapModeAbschicken(slots(aktslot))
            'nach range neu sortieren
        End If
        refreshExplorerView("")

        refreshKategorienListe()
        e.Handled = True
    End Sub

    Private Shared Sub entferneEbeneAusSlots(pickAid As Integer)
        For i = 0 To slots.Length - 1
            If slots(i).layer.aid = pickAid Then
                slots(i).setEmpty()
                slots(i).layer.clear()
                slots(i).mapfile = ""
                Exit For
            End If
        Next
    End Sub

    Private Sub entferneEbeneauslayersSelected(pickAid As Integer)
        For Each lay As clsLayerPres In layersSelected
            If lay.aid <> pickAid Then
                Continue For
            End If
            lay.isactive = False
            lay.RBischecked = False
            lay.mithaken = False
            If layerActive.aid = lay.aid Then
                layerActive.aid = 0 'schaltet die darstellung des punktes weg
                WebBrowser1.LoadHtml("", myglobalz.myfakeurl) ' soll die alte imagemap löschen, sonst stimmt sie nicht mit layeractive überein 
            End If
        Next
    End Sub

    Private Sub chkAktiveEbenegeaendert(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim nck As RadioButton = CType(sender, RadioButton)
        Dim tag As Integer
        panningAusschalten()
        tag = CInt(nck.Tag)
        'MsgBox("layerActive.iswms tag " & tag)
        'btnWMSgetfeatureinfo.Visibility = Visibility.Collapsed
        modLayer.alteAktiveEbeneDeaktivieren(layersSelected)
        markWMSlayers(layersSelected)
        For Each lay As clsLayerPres In layersSelected
            If lay.aid = CInt(CStr(tag)) Then
                'MsgBox("layerActive.iswms 0 " & layerActive.iswms)
                lay.isactive = True
                lay.mithaken = True
                lay.RBischecked = True
                layerActive = CType(lay.Clone, clsLayerPres)
                layerActive.aid = lay.aid
                layerActive.iswms = lay.iswms
                layerHgrund.isactive = False
                If lay.iswms Then
                    markwmslayerSingle(lay)
                    'MsgBox("layerActive.iswms a " & layerActive.iswms)
                    panningAusschalten()
                    WebBrowser1.Visibility = Visibility.Collapsed
                    cvtop.Cursor = Cursors.Hand
                    CanvasClickModus = "wmsdatenabfrage"
                Else
                    WebBrowser1.LoadHtml("", myglobalz.myfakeurl) ' soll die alte imagemap löschen, sonst stimmt sie nicht mit layeractive überein
                End If
            End If
        Next
        ebenenListeAktualisieren()

        refreshKategorienListe()
        rbHgrundAktiveEbene.IsChecked = False
        refreshMap(True, False)
    End Sub


    Private Sub btnEbenenaktualisieren_Click(sender As Object, e As RoutedEventArgs)
        ebenenListeAktualisieren()
        leereSelectedlayersNachPres(layersSelected)

        refreshExplorerView("allenothing")
        refreshMap(True, True)
        e.Handled = True
    End Sub

    Private Sub ebenenListeAktualisieren()
        'entladen
        Debug.Print(layerActive.aid.ToString)
        For i = 0 To layersSelected.Count - 1
            If layersSelected(i).mithaken Then
                If Not warSchonGeladen(layersSelected(i).aid, layersSelected) Then
                    For Each lay As clsLayer In layersSelected
#If DEBUG Then
                        If layersSelected(i).aid = 186 Then
                            Debug.Print("")
                        End If
#End If
                        If lay.aid = CInt(layersSelected(i).aid) Then
                            layersSelected.Add(layersSelected(i))
                            refreshExplorerView("lvrefresh")
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
                        If layersSelected(j).aid = 186 Then
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




    'Private Sub zeigeLegendeUndDoku(aktaid As Integer, aktsid As Integer, layerHatOS As Boolean)
    '    stContext.Width = cvtop.Width '- 100
    '    stContext.Height = cvtop.Height ' - 100
    '    stpDokuUndLegende.Width = stContext.Width - stpKnoeppeVertical.Width
    '    'stContext.Visibility = Visibility.Collapsed
    '    'stwinthemen.Visibility = Visibility.Visible
    '    'stwinthemen.Width = cvtop.Width - 100
    '    'stwinthemen.Height = cvtop.Height - 100
    '    Debug.Print("")
    '    btnObjektsuche.ToolTip = "Keine Objektsuche für diese Ebene verfügbar"
    '    btnObjektsuche.Visibility = Visibility.Collapsed
    '    stContext.Visibility = Visibility.Visible
    '    stwinthemen.Visibility = Visibility.Collapsed
    '    stpDoku.Visibility = Visibility.Visible
    '    'stpLegende.Visibility = Visibility.Visible
    '    stpObjektsuche.Visibility = Visibility.Collapsed
    '    stpDokuUndLegende.Visibility = Visibility.Visible

    '    ladeRTF(CStr(aktaid), "\rtfdoku\", richTextBoxDoku)
    '    'ladeRTF(CStr(aktaid), "\rtflegend\", richTextBoxLeg)
    '    If ladePDF(aktaid, aktsid) > 0 Then
    '        stpPDFliste.Visibility = Visibility.Visible
    '    Else
    '        stpPDFliste.Visibility = Visibility.Collapsed

    '    End If
    'End Sub

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

    'Private Sub btnDoku_Click(sender As Object, e As RoutedEventArgs)
    '    If Not ladevorgangAbgeschlossen Then Exit Sub
    '    stpDoku.Visibility = Visibility.Visible
    '    'stpLegende.Visibility = Visibility.Visible
    '    stpPDFliste.Visibility = Visibility.Visible
    '    stpObjektsuche.Visibility = Visibility.Collapsed
    '    Dim nck As Button = CType(sender, Button)

    '    'Dim aktsid = CInt(nck.Uid)

    '    'MsgBox(" baustelle dokumentation aid Button : " & CStr(nck.Tag))
    '    'stContext.Visibility = Visibility.Collapsed
    '    '  ladeRTF(CStr(nck.Tag))

    '    ladeRTF(CStr(aktaid), "\rtfdoku\", richTextBoxDoku)
    '    'ladeRTF(CStr(aktaid), "\rtflegend\", richTextBoxLeg)
    '    If ladePDF(aktaid, aktsid) > 0 Then
    '        stpPDFliste.Visibility = Visibility.Visible
    '    Else
    '        stpPDFliste.Visibility = Visibility.Collapsed

    '    End If
    '    e.Handled = True
    'End Sub

    'Private Sub ladeRTF(aid As String, subdir As String, richTextBoxAll As RichTextBox)
    '    Try
    '        Dim ddatei = serverUNC & "nkat\aid\" & aid & subdir & aid & ".rtf"
    '        Dim fi As New IO.FileInfo(ddatei)
    '        richTextBoxAll.Document.Blocks.Clear()
    '        If fi.Exists Then
    '            Using datei As IO.StreamReader = New IO.StreamReader(ddatei)
    '                rtfTextDoku = datei.ReadToEnd
    '            End Using
    '            Dim documentBytes = Text.Encoding.UTF8.GetBytes(rtfTextDoku)
    '            Dim reader = New System.IO.MemoryStream(documentBytes)
    '            reader.Position = 0
    '            richTextBoxAll.SelectAll()
    '            richTextBoxAll.Selection.Load(reader, DataFormats.Rtf)
    '        Else
    '            'keine Datei gefunden
    '        End If
    '    Catch ex As Exception
    '        l("fehler in winRTF_Loaded " ,ex)
    '        MsgBox(ex.ToString)
    '    End Try
    'End Sub
    Private Function bildeSQLString(schema As String, tabelle As String, okat As String, ofeld As String, volltextsucheSql As String) As String
        Try
            Dim sql As String = ""
            l("bildeSQLString---------------------- anfang")
            If okat = String.Empty And ofeld = String.Empty Then
                sql = "Select  * from  " & schema & ".os_" & tabelle
            End If
            If okat = String.Empty And ofeld <> String.Empty Then
                sql = "Select  * from  " & schema & ".os_" & tabelle & " where " &
                                  "(" & volltextsucheSql & ")  "
            End If
            If okat <> String.Empty And ofeld = String.Empty Then
                sql = "Select  * from  " & schema & ".os_" & tabelle & " where lower(okategorie)= '" & okat.ToLower & "'"
            End If
            If okat <> String.Empty And ofeld <> String.Empty Then
                sql = "Select  * from  " & schema & ".os_" & tabelle & " where " &
                " lower(okategorie) = '" & okat.ToLower & "' and (" & volltextsucheSql & ")  "
            End If
            l("sql:" & sql)
            l("bildeSQLString---------------------- ende")
            Return sql
        Catch ex As Exception
            l("Fehler in bildeSQLString: ", ex)
            Return ""
        End Try
    End Function


    Private Sub sichtbarMachenObjektsuche()
        stpDoku.Visibility = Visibility.Collapsed
        stpPDFliste.Visibility = Visibility.Collapsed
        stpObjektsuche.Visibility = Visibility.Visible
    End Sub

    Private Sub btnLegende_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub


        schowLegendeDoku(aktaid)
        'Dim nlay As New clsLayerPres
        'nlay.aid = aktaid 'CInt(nck.Tag) 
        'pgisTools.getStamm4aid(nlay)
        'showFreiLegende4Aid(nlay)

    End Sub

    Private Sub showFreiLegende4Aid(nlay As clsLayerPres)
        Dim legdatei As String = "", dokdatei As String = "", dokHtml As String = ""
        '= serverUNC & "nkat\aid\" & daaid & "\rtflegend\" & daaid & ".rtf"
        dokdatei = nsMakeRTF.rtf.makeDokuHtml(nlay, dokHtml, nlay.aid)
        legdatei = nsMakeRTF.rtf.makeftlLegende4Aid(nlay, "html", dokHtml)

        'If legdatei = "error" Or legdatei = "" Then
        '    'MessageBox.Show("Keine Legende vorhanden!")
        '    Exit Sub
        'End If
        stContext.Visibility = Visibility.Collapsed
        If legdatei IsNot Nothing Then
            Dim freileg As New winLeg(legdatei, dokdatei, "html", nlay.aid)
            freileg.Show()
        End If

    End Sub
    Private Sub clearAllSlots()
        GC.Collect()
        Exit Sub
        For i = 0 To slots.Count - 1
            If slots(i).refresh Then
                slots(i).setEmpty()
            End If
        Next
        GC.Collect()
    End Sub
    'Private Sub clearCanvasALT(vgrundRefresh As Boolean, hgrundrefresh As Boolean, osrefresh As Boolean)
    '    GC.Collect()
    '    If vgrundRefresh Then
    '        cvtop.Children.Clear()
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
            l("fehler in leeresbild: " & aufruf & " /// ", ex)
        End Try
    End Sub

    Private Sub btnrefresh_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'darstellen
        If Not ladevorgangAbgeschlossen Then Exit Sub
        panningAusschalten()
        resizeWindow()
        refreshMap(True, True)
        'btnrefresh.Background = Brushes.Black
    End Sub



    Private Sub btnremovelayerFromList(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        ' nur vordergrund
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim nck As Button = CType(sender, Button)
        Dim tag As Integer
        panningAusschalten()
        tag = CType(nck.Tag, Int16)
        Dim mesres As MessageBoxResult
        mesres = MessageBox.Show("Ebene wirklich löschen?",
                                 "Vertippt? " & aktvorgangsid, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
        Try
            If mesres = MessageBoxResult.Yes Then
                'aus layersselected entferneen
                For Each clay As clsLayerPres In layersSelected
                    If clay.aid = CInt(tag) Then
                        layersSelected.Remove(clay)
                        If clay.aid = layerActive.aid Then
                            layerActive.clearPres()
                        End If
                        refreshExplorerView("")
                        Exit For
                    End If
                Next
                'ebenenListeAktualisieren()
                refreshMap(True, False)
            End If
            refreshExplorerView("")
        Catch ex As Exception
            l("Fehler in btnremovelayerFromList ", ex)
        End Try

    End Sub











    Private Sub btnclosesuchform_Click(sender As Object, e As RoutedEventArgs)
        stwinthemen.Visibility = Visibility.Collapsed
        e.Handled = True
    End Sub
    Private Sub btnStichwort_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        stichwortsucheDurchfuehr()

    End Sub

    Private Sub stichwortsucheDurchfuehr()
        tbStichwort.Text = tbStichwort.Text.ToLower.Trim
        tbStichwort.Text = clsString.normalize_Filename(tbStichwort.Text, " ")
        tbStichwort.Text = tbStichwort.Text.Replace("-", " ").Trim
        stichwortsucheDurchfuehren()
        tbebenenauswahlinfo.Visibility = Visibility.Visible
    End Sub
    <Obsolete>
    Private Sub stichwortsucheDurchfuehrenALT()
        Dim anzahlSchonGeladeneEbenen As Integer = 0
        If tbStichwort.Text.Trim.Count < 3 Then
            MsgBox("Bitte mind. 3 Buchstaben angeben !", MsgBoxStyle.OkOnly, "Hinweis")
            Exit Sub
        End If
        LastThemenSuche = "stichwort"
        SuchLayersList = modLayer.getLayer4stichwort(tbStichwort.Text, anzahlSchonGeladeneEbenen)
        SuchLayersList.Sort()
        Dim warschongeladenString As String = ""
        If anzahlSchonGeladeneEbenen > 0 Then
            warschongeladenString = "  (Es sind bereits " & anzahlSchonGeladeneEbenen &
                " Ebenen davon geladen und werden daher nur in grau angezeigt.)"
        End If
        tbtreffer.Text = "   >>> " & SuchLayersList.Count & " Treffer.  " & warschongeladenString
        dgErgebnis.ItemsSource = SuchLayersList
    End Sub
    Private Sub stichwortsucheDurchfuehren()
        Dim anzahlSchonGeladeneEbenen As Integer = 0
        If tbStichwort.Text.Trim.Count < 3 Then
            MsgBox("Bitte mind. 3 Buchstaben angeben !", MsgBoxStyle.OkOnly, "Hinweis")
            Exit Sub
        End If
        LastThemenSuche = "stichwort"
        SuchLayersList = modLayer.getLayer4stichwort(tbStichwort.Text, anzahlSchonGeladeneEbenen)
        SuchLayersList.Sort()
        Dim warschongeladenString As String = ""
        If anzahlSchonGeladeneEbenen > 0 Then
            warschongeladenString = "  (Es sind bereits " & anzahlSchonGeladeneEbenen &
                " Ebenen davon geladen und werden daher nur in grau angezeigt.)"
        End If
        tbtreffer.Text = "   >>> " & SuchLayersList.Count & " Treffer.  " & warschongeladenString
        tiSuche.IsSelected = True
        tiSuche.Header = "Suche (" & SuchLayersList.Count & " Treffer)"
        lvEbenenSuche.ItemsSource = SuchLayersList
    End Sub

    Private Sub dgErgebnis_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If dgErgebnis.SelectedItem Is Nothing Then Exit Sub
        Dim item As New clsLayerPres
        Try
            item = CType(dgErgebnis.SelectedItem, clsLayerPres)
        Catch ex As Exception
            l(ex.ToString)
            Exit Sub
        End Try
        If item.schongeladen = 1 Then
            MessageBox.Show("Die Ebene ist bereits geladen. Schauen sie bitte links in die Liste.", "Ebene ist schon geladen", MessageBoxButton.OK, MessageBoxImage.Information)
            Exit Sub
        End If
        'neuesLayerHinzufuegen
        item.mithaken = True
        item.RBischecked = False
        item.isactive = False
        item.justAdded = True
        layersSelected.Add(item)
        refreshExplorerView("lvrefresh")
        'ergebenislisteaktualisieren
        layersSelected.Sort()

        If LastThemenSuche = "stichwort" Then
            stichwortsucheDurchfuehren()
        End If
        refreshMap(True, True)
        dgErgebnis.SelectedItem = Nothing
    End Sub


    Private Sub btnPdf_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Debug.Print(OSrefresh.ToString)
        panningAusschalten()
        If dockMenu.Width < 1 Then
            dockMenu.Width = 460
            refreshMap(True, True)
            Exit Sub
        End If
        resizeWindow()
        'mapfileNamenNeuBerechnen()
        If aktFST.name.IsNothingOrEmpty Then
            cbmitsuchobjekt.IsEnabled = False
            cbmitsuchobjekt.IsChecked = False
        Else
            cbmitsuchobjekt.IsEnabled = True
            cbmitsuchobjekt.IsChecked = False
        End If

        If stPDFDruck.Visibility = Visibility.Collapsed Then
            disableMyStackpanel(spButtonMenu, False)
            disableMyStackpanel(spObereButtonMenu, False)
            rbMitMasstab.IsChecked = False

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
            disableMyStackpanel(spObereButtonMenu, True)
        End If
        Dim wmslayerTitelSumme As String
        PDFlayers = modLayer.kopiereSelectedLayers(layersSelected, mithaken:=True, ohnewms:=False)
        If modLayer.bestandHatWmsLayer(wmslayerTitelSumme) Then
            Dim mesres As New MessageBoxResult
            mesres = MessageBox.Show("Sie haben auch WMS-Ebenen geladen. Diese können die Erzeugung der PDF-Datei " & Environment.NewLine &
                            "bremsen und/oder verhindern." & Environment.NewLine &
                            "" & Environment.NewLine &
                            "Möchten Sie die WMS-Ebenen mitdrucken ? (N) " & Environment.NewLine &
                            "" & Environment.NewLine &
                            "J - WMS-Ebenen mit drucken " & Environment.NewLine &
                            "N - Ohne WMS-Ebenen drucken" & Environment.NewLine &
                            "" & Environment.NewLine &
                            wmslayerTitelSumme & Environment.NewLine,
                            "Vorsicht: WMS-Ebenen haben Einschränkungen",
                            MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
            If mesres = MessageBoxResult.Yes Then
            Else
                PDFlayers = modLayer.kopiereSelectedLayers(PDFlayers, mithaken:=True, ohnewms:=True)
            End If
            'MessageBox.Show("Sie haben auch WMS-Layer geladen. Diese können die Erzeugung der PDF-Datei " & Environment.NewLine &
            '                "bremsen und/oder verhindern." & Environment.NewLine &
            '                "" & Environment.NewLine &
            '                "Tipp: Lassen Sie die WMS-Ebenen bei der PDF-Erzeugung weg." & Environment.NewLine &
            '                "" & Environment.NewLine &
            '                wmslayerTitelSumme & Environment.NewLine, "Vorsicht WMS-Dienste haben Einschränkungen"
            '                )
        End If
        If iminternet Then
            ' cbhochaufloesend.Visibility = Visibility.Collapsed
            'cbmitsuchobjekt.Visibility = Visibility.Collapsed
        End If
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
        If auswahlRechteck Is Nothing Then
            auswahlRechteck = New Rectangle()
        End If
        'pdfrahmenNeuPLatzieren("quer")
        'cvPDFrechteck.Visibility = Visibility.Visible
        'PDF_druckMassStab = PDF_postition_desRahmensBestimmen()
        'tbMasstabDruck.Text = CInt(PDF_druckMassStab).ToString


        '  setAuswahlRechteckProps("quer", auswahlRechteck)

        tbPDF_Bemerkung.Text = getPDFBemerkung()
        tbPDF_Ort.Text = getPDFOrt(kartengen.aktMap.aktrange)
        'spdruckmasstab.Visibility = Visibility.Hidden
        If STARTUP_mgismodus = "vanilla" Then
            gbPDFparadigma.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Function getPDFOrt(lokrange As clsRange) As String
        'lokrange.CalcCenter()
        Return "UTM32: " & CInt(lokrange.xcenter) & ", " & CInt(lokrange.ycenter)
    End Function

    Private Function getPDFBemerkung() As String
        If aktvorgangsid.IsNothingOrEmpty Then
            Return GisUser.nick
        Else
            Return GisUser.nick & " (" & aktvorgangsid & ")"
        End If
    End Function



    Private Sub cmbHgrund_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        resizeWindow()
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
            'btnHGSlider.IsOpen = True
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
            refreshExplorerView("lvrefresh")
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
            l("Fehler in isInSelectedLayers: ", ex)
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
        resizeWindow()
        myglobalz.mgisBackModus = False
        imgpin.Visibility = Visibility.Collapsed
        If chkBoxAusschnitt.IsChecked Then
            WebBrowser1.Visibility = Visibility.Collapsed
            zeichneOverlaysGlob = True : zeichneImageMapGlob = False
            'panningAusschalten()
            'refreshMap()
            cvtop.Cursor = Cursors.Cross
            CanvasClickModus = "Ausschnitt"

            DrawRectangle(cvtop)
        Else
            WebBrowser1.Visibility = Visibility.Visible
            zeichneOverlaysGlob = True : zeichneImageMapGlob = True
            refreshMap(True, True)
            CanvasClickModus = ""
            WebBrowser1.Visibility = Visibility.Visible
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
        'tbFavoname.Text = favoritakt.titel
        cballeeinaus.IsChecked = True
        e.Handled = True
    End Sub

    Private Sub handleFavorite(tag As String, istfix As String)
        'favo einlesen
        Dim erfolg As Boolean
        Select Case tag.ToLower
            Case "meine"
                erfolg = favoTools.FavoritLaden("meine", GisUser.nick)
            Case "meinespeichern"
                favoTools.FavoritSave(GisUser.nick)
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
        layerActive.aid = favoTools.getStandardActiveLayer(favoritakt.aktiv, favoritakt.gecheckted) 'getinteger(favoritakt.aktiv)
        layerHgrund.aid = favoTools.getinteger(favoritakt.hgrund) 'CInt(favoritakt.hgrund.Replace(";", ""))
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
            markWMSlayers(layersSelected)
            If layerHgrund.aid = layerActive.aid Then
                'hintergrund ist aktiv
                layerHgrund.isactive = True
                layerHgrund.mithaken = True
                layerHgrund.RBischecked = True
                rbHgrundAktiveEbene.IsChecked = True
                If layerHgrund.iswms Then
                    layerActive.iswms = True
                    markwmslayerSingle(layerHgrund)
                    markwmslayerSingle(layerActive)
                End If
            End If
        End If

        'cmbHgrund.SelectedValue = layerHgrund.aid
        layersSelected.Clear()

        layersSelected.Clear()

        refreshExplorerView("")

        Dim vorhanden() As String : vorhanden = favoritakt.vorhanden.Split(";"c)
        Dim gecheckt() As String : gecheckt = favoritakt.gecheckted.Split(";"c)
        layersSelected = ListeVorhandeneLayersUmsetzenNachPres(vorhanden, gecheckt)
        markWMSlayers(layersSelected)
        layersSelected.Sort()
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            userlayerCorrectDarstellen(CInt(tbVorgangsid.Text))
        End If
        'tbFavoname.Text = favoritakt.titel
    End Sub

    Private Sub refreshExplorerView(befehl As String)
        Try
            If befehl = "lvrefresh" Then
                lvEbenenAlle.Items.Refresh()
                lvEbenenKompakt.Items.Refresh()
            End If
            If befehl = "allenothing" Then
                lvEbenenAlle.ItemsSource = Nothing
                lvEbenenAlle.Items.Refresh()
                lvEbenenKompakt.ItemsSource = Nothing
                lvEbenenKompakt.Items.Refresh()
            End If
            If befehl = "normal" Or
                befehl = "" Then
                lvEbenenAlle.ItemsSource = Nothing
                lvEbenenAlle.Items.Refresh()
                lvEbenenAlle.ItemsSource = layersSelected
                lvEbenenAlle.Items.Refresh()

                lvEbenenKompakt.ItemsSource = Nothing
                lvEbenenKompakt.Items.Refresh()
                layersSelectedKompakt = clsLayerHelper.getKompaktLayers(layersSelected)
                lvEbenenKompakt.ItemsSource = layersSelectedKompakt
                lvEbenenKompakt.Items.Refresh()


            End If

        Catch ex As Exception
            l("fehler in refreshExplorerView ", ex)
        End Try
    End Sub

    Private Function ListeVorhandeneLayersUmsetzenNachPres(vorhanden() As String, gecheckt() As String) As List(Of clsLayerPres)

        Dim newlist As New List(Of clsLayerPres)
        erzeugeLeereselectedLayerliste(vorhanden, newlist, gecheckt)
        leereSelectedlayersNachPres(newlist)
        clsLayerHelper.setKatinfo2Layers(newlist)
        Return newlist
    End Function

    Private Sub leereSelectedlayersNachPres(ByRef layselect As List(Of clsLayerPres))
        For Each nlay As clsLayerPres In layselect
#If DEBUG Then
            If nlay.aid = 1 Then
                Debug.Print("")
            End If
#End If
            'nlay.thumbnailFullPath = myglobalz.serverUNC & "nkat\thumbnails\" & nlay.aid & ".png"
            nlay.thumbnailFullPath = myglobalz.serverWeb & "/nkat/thumbnails/" & nlay.aid & ".png"

            If nlay.justAdded Then
                nlay.farbe = Brushes.IndianRed
            Else
                nlay.farbe = getColorBrush4hauptSachgebiet(nlay.standardsachgebiet)
            End If
            nlay.etikettfarbe = Brushes.LightGray

            nlay = clsWebgisPGtools.setSichtbarkeitRBaktiveEbene(nlay)
            'If nlay.aid = GisUser.userLayerAid And GisUser.userLayerAid > 0 And (Not nlay.isHgrund) Then
            If nlay.aid = GisUser.userLayerAid And GisUser.userLayerAid > 0 Then
                nlay.aid = GisUser.userLayerAid
                nlay.isUserlayer = True
                pgisTools.getStamm4aid(nlay)
                nlay.titel = "Paradigma Ebene: " & GisUser.nick & "(" & tbVorgangsid.Text & ")"
                nlay.thumbnailFullPath = myglobalz.serverUNC & "nkat\thumbnails\userlayer.png"
                nlay.dokutext = "Die Raumbezüge des Paradigmavorgangs werden hier in blau dargestellt."
                nlay.RBischecked = False
                nlay.RBsichtbarkeit = Visibility.Visible
                nlay.isactive = False
                'nlay.farbe = Brushes.LightSalmon

                'nlay.dokutext = clsWebgisPGtools.bildeDokuTooltip(nlay)
            End If
            If layerActive.aid = nlay.aid Then
                layerActive.isactive = True
                nlay.isactive = True
                If nlay.mithaken Then
                    nlay.RBischecked = True
                Else
                    nlay.RBischecked = False
                End If


                pgisTools.getStamm4aid(layerActive)
                nlay.etikettfarbe = Brushes.White
                nlay.dokutext = "(Sachgeb.: " & nlay.kategorieLangtext & ") " & Environment.NewLine &
                    clsWebgisPGtools.bildeDokuTooltip(nlay)
                If modLayer.markwmslayerSingle(nlay) Then
                    layerActive.iswms = nlay.iswms
                    layerActive.wmsProps.url = nlay.wmsProps.url
                    layerActive.wmsProps.format = nlay.wmsProps.format
                    layerActive.wmsProps.typ = nlay.wmsProps.typ
                    layerActive.wmsProps.stdlayer = nlay.wmsProps.stdlayer
                End If

            End If
            If layerHgrund.aid = nlay.aid Then
                nlay.isHgrund = True
                nlay.mithaken = True

                layerHgrund.mithaken = True
                pgisTools.getStamm4aid(layerHgrund)
                nlay.dokutext = "(Sachgeb.: " & nlay.kategorieLangtext & ") " & Environment.NewLine & clsWebgisPGtools.bildeDokuTooltip(nlay)
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
            nlay.dokutext = "(Sachgeb.: " & nlay.kategorieLangtext & ") " & Environment.NewLine & clsWebgisPGtools.bildeDokuTooltip(nlay)
        Next
    End Sub

    Private Sub erzeugeLeereselectedLayerliste(vorhanden() As String, newlist As List(Of clsLayerPres), gecheckt() As String)
        Dim nlay As New clsLayerPres
        For Each vorh As String In vorhanden
            nlay = New clsLayerPres
            If Not vorh.IsNothingOrEmpty Then
                nlay.aid = CInt(vorh)
#If DEBUG Then
                If nlay.aid = 1 Then
                    Debug.Print("")
                End If
#End If
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

    Private Shared Sub userlayerCorrectDarstellen(vid As Integer)
        Dim nlay As New clsLayerPres
        If Not GisUser.ADgruppenname.ToLower = "umwelt" Then
            'If Not GisUser.favogruppekurz = "umwelt" Then

            Exit Sub
        End If
        l("userlayer in liste einbauen")
        'If STARTUP_mgismodus.ToLower <> "paradigma" Then
        '    l("kein paradigmamodus")
        '    'MsgBox("keinpmode")
        '    Exit Sub
        'End If
        If GisUser.userLayerAid > 0 Then
            If clsString.isinarray(favoritakt.vorhanden, CType(GisUser.userLayerAid, String), ";") Then

            Else
                If layerActive.aid = GisUser.userLayerAid And GisUser.userLayerAid > 0 Then
                    nlay.aid = GisUser.userLayerAid
                    pgisTools.getStamm4aid(nlay)
                    nlay.titel = "Paradigma Ebene: " & GisUser.nick & "(" & vid & ")"
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
                    nlay.titel = "Paradigma Ebene: " & GisUser.nick & "(" & vid & ")"
                    nlay.thumbnailFullPath = myglobalz.serverUNC & "nkat\thumbnails\userlayer.png"
                    nlay.dokutext = "Die Raumbezüge des Paradigmavorgangs werden hier in blau dargestellt."
                    nlay.suchfeld = nlay.titel & " " & nlay.schlagworte
                    nlay.mithaken = True
                    nlay.RBischecked = False
                    nlay.RBsichtbarkeit = Visibility.Visible
                    nlay.isactive = False
                    nlay.farbe = Brushes.LightSalmon
                End If
                layersSelected.Add(nlay)

                'refreshExplorerView("lvrefresh")
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
        SuchLayersList = modLayer.getLayer4sachgebiet("grenzen")
        SuchLayersList.Sort()
        clsWebgisPGtools.dombineLayerDoku(SuchLayersList, allDokus)
        dgErgebnis.ItemsSource = SuchLayersList
        e.Handled = True
    End Sub

    Private Sub cballeeinaus_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
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
            l("cballeeinaus_Click---------------------- ende")
        Catch ex As Exception
            l("Fehler in cballeeinaus_Click: ", ex)
        End Try
    End Sub

    Private Sub btnBalken_Click(sender As Object, e As RoutedEventArgs)
        stckBalken.Visibility = Visibility.Collapsed
        e.Handled = True

    End Sub

    Private Sub btnflurlarte_Click(sender As Object, e As RoutedEventArgs)
        LastThemenSuche = "hauptsachgebiet"
        dgErgebnis.ItemsSource = Nothing
        SuchLayersList = modLayer.getLayer4sachgebiet("flurkarte")
        SuchLayersList.Sort()
        clsWebgisPGtools.dombineLayerDoku(SuchLayersList, allDokus)
        dgErgebnis.ItemsSource = SuchLayersList
        e.Handled = True
    End Sub

    Private Sub cmbMasstab_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        resizeWindow()
        'If cmbMasstab.SelectedValue Is Nothing Then
        '    Exit Sub
        'End If
        myglobalz.mgisBackModus = False
        panningAusschalten()
        If cmbMasstab.SelectedItem Is Nothing Then Exit Sub
        Dim item As clsMasstab = CType(cmbMasstab.SelectedItem, clsMasstab)

        'tbMasstab.Text = " 1: " & CType(item.Tag, String)
        'setTBmasstab(CType(item.Tag, Double))  
        setNewMasstab(item.intval, False)
        ' cmbMasstab.SelectedValue = Nothing
        cmbMasstab.SelectedItem = Nothing
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

    Private Sub MouseWheelHandlerTBm(sender As Object, e As MouseWheelEventArgs) Handles tbMasstab.MouseWheel, WebBrowser1.MouseWheel
        ' Moves the TextBox named box when the mouse wheel is rotated.
        ' The TextBox is on a Canvas named MainCanvas.
        panningAusschalten()
        Dim tmasstab As Integer = CInt(scaleScreen.aktMasstab)
        ' If the mouse wheel delta is positive, move the box up.
        If e.Delta > 0 Then

            'reinzoomen

            Mouse.Capture(Nothing)
            KoordinateKLickpt = e.GetPosition(cvtop)
            tbMinimapCoordinate2.Text = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint
            'CanvasClickModus = ""
            'Dim tt = Canvas.GetTop(cvtop)
            'If Canvas.GetTop(cvtop) >= 1 Then
            '    Canvas.SetTop(cvtop, Canvas.GetTop(cvtop) - 1)
            'End If
            ' Debug.Print(CType(aktmasstabTag, String) & scaleScreen.aktMasstab)
            For i = 0 To (masstaebe.Count - 1)
                'Debug.Print(masstaebe(i).ToString)
                If masstaebe(i).intval > tmasstab Then
                    setNewMasstab(masstaebe(i).intval, True)
                    Exit Sub
                End If
            Next


            'den naechsthöheren Maßstab nehmen
        End If

        ' If the mouse wheel delta is negative, move the box down.
        If e.Delta < 0 Then
            'reinzoomen
            Mouse.Capture(Nothing)
            KoordinateKLickpt = e.GetPosition(cvtop)
            tbMinimapCoordinate2.Text = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt) & " [m]" ' === aktpoint

            Debug.Print(CType(aktmasstabTag, String) & scaleScreen.aktMasstab)
            For i = (masstaebe.Count - 1) To 0 Step -1
                Debug.Print(masstaebe(i).ToString)
                If masstaebe(i).intval < tmasstab Then
                    setNewMasstab(masstaebe(i).intval, True)
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
        disableMyStackpanel(spObereButtonMenu, True)
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


    Private Function getAusrichtung(querformat As Boolean) As String
        Dim ausrichtung As String
        If querformat Then
            ausrichtung = "quer"
        Else
            ausrichtung = "hoch"
        End If
        Return ausrichtung
    End Function



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
    Private Sub quer_Checked(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If auswahlRechteck Is Nothing Then auswahlRechteck = New Rectangle



        '  Debug.Print("auswahlRechteck " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)
        pdfrahmenNeuPLatzieren("quer")
        'PDF_druckMassStab = PDF_postition_desRahmensBestimmen(auswahlRechteck)
        Debug.Print("auswahlRechteck " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)
        PDF_postition_desRahmensAbfragen(auswahlRechteck)

        'PDF_druckMassStab = calcPDFMassstab(PDF_PrintRange, CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked))
        'tbMasstabDruck.Text = CInt(PDF_druckMassStab).ToString

        tbPDF_Bemerkung.Text = getPDFBemerkung()
        tbPDF_Ort.Text = getPDFOrt(kartengen.aktMap.aktrange)
        'btnMakePDFohnemass.IsEnabled = True
        e.Handled = True
    End Sub
    Private Sub hoch_Checked(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Debug.Print("auswahlRechteck " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)
        pdfrahmenNeuPLatzieren("hoch")
        'PDF_druckMassStab = PDF_postition_desRahmensBestimmen(auswahlRechteck) 
        PDF_postition_desRahmensAbfragen(auswahlRechteck)
        'PDF_druckMassStab = calcPDFMassstab(PDF_PrintRange, CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked))
        'tbMasstabDruck.Text = CInt(PDF_druckMassStab).ToString 
        tbPDF_Bemerkung.Text = getPDFBemerkung()
        tbPDF_Ort.Text = getPDFOrt(kartengen.aktMap.aktrange)
        'btnMakePDFohnemass.IsEnabled = False
        e.Handled = True
    End Sub

    Private Sub pdfrahmenNeuPLatzieren(hochQuerModus As String)
        Dim newtopLeftPoint, alterMittelPunkt As New myPoint
        Dim count As New Text.StringBuilder
        Try
            count.Append(",auswahlRechtecka " & auswahlRechteck.Width & ", " & auswahlRechteck.Height & Environment.NewLine)
            alterMittelPunkt = getAltermittelpunkt(auswahlRechteck, cvtop) 'muss aufgerufen werden BEVOR die Form geändert wird
            count.Append(",alterMittelPunktb " & alterMittelPunkt.toString & Environment.NewLine)
            cvPDFrechteck.Children.Clear()
            count.Append(",auswahlRechteckc " & auswahlRechteck.Width & ", " & auswahlRechteck.Height & Environment.NewLine)
            setAuswahlRechteckProps(hochQuerModus, auswahlRechteck)
            cvPDFrechteck.Children.Add(auswahlRechteck)

            count.Append(",auswahlRechteckd " & auswahlRechteck.Width & ", " & auswahlRechteck.Height & Environment.NewLine)
            newtopLeftPoint = createPDF.calcPDFrahmenPositionInPixel(auswahlRechteck, alterMittelPunkt)
            count.Append(",newtopLeftPoint" & newtopLeftPoint.toString & Environment.NewLine)
            cvPDFrechteck.SetLeft(auswahlRechteck, CInt(newtopLeftPoint.X))
            cvPDFrechteck.SetTop(auswahlRechteck, CInt(newtopLeftPoint.Y))
        Catch ex As Exception
            l("fehler in pdfrahmenNeuPLatzieren: hochQuerModus: " & hochQuerModus & ", count: " & count.ToString, ex)
        End Try
    End Sub

    Private Sub setAuswahlRechteckProps(hochQuerModus As String, altesRechteck As Rectangle)
        Try
            l(" MOD setAuswahlRechteckProps anfang: " & hochQuerModus)
            If rbFormatA4.IsChecked Then
                setMyPdfRectA4(hochQuerModus, altesRechteck)
            End If
            If Not rbFormatA4.IsChecked Then ' A3
                setMyPdfRectA3(hochQuerModus, auswahlRechteck)
            End If
            l(" MOD setAuswahlRechteckProps ende")
        Catch ex As Exception
            l("Fehler in setAuswahlRechteckProps: hochQuerModus: " & hochQuerModus & " " & altesRechteck.ToString, ex)
        End Try
    End Sub

    Private Shared Sub initPdfauswahlRechteck()
        auswahlRechteck = New Rectangle
        auswahlRechteck.Stroke = Brushes.Black
        auswahlRechteck.StrokeThickness = 2
        auswahlRechteck.Name = "herrmann"
        auswahlRechteck.Fill = Brushes.Transparent
        auswahlRechteck.HorizontalAlignment = HorizontalAlignment.Left
        auswahlRechteck.VerticalAlignment = VerticalAlignment.Center
        auswahlRechteck.Width = 495
        auswahlRechteck.Height = 350
    End Sub

    Private Shared Sub setMyPdfRectA3(hochQuerModus As String, altesRechteck As Rectangle)
        Dim w = altesRechteck.Width : Dim h = altesRechteck.Height

        If hochQuerModus = "quer" Then
            If w > h Then
                'ist schon quer
                auswahlRechteck.Width = w
                auswahlRechteck.Height = h
            Else
                'ist noch nicht quer
                auswahlRechteck.Width = h
                auswahlRechteck.Height = w
            End If

        Else
            If w < h Then
                'ist schon hoch
                auswahlRechteck.Width = w
                auswahlRechteck.Height = h
            Else
                'ist noch nicht hoch
                auswahlRechteck.Width = h
                auswahlRechteck.Height = w
            End If
        End If



        'If hochQuerModus = "quer" Then
        '    auswahlRechteck.Width = 700.5 '350 * 1,414285714 = 495
        '    auswahlRechteck.Height = auswahlRechteck.Width * 0.707070707 'a4
        '    'myRect.Height = myRect.Width * 0.661921708
        '    'myRect.Height = myRect.Width * 0.706650831 'basierend auf 842,595 
        'Else
        '    auswahlRechteck.Width = 495
        '    auswahlRechteck.Height = auswahlRechteck.Width * 1.414285714 'a4
        '    'myRect.Height = myRect.Width * 1.510752688
        '    'myRect.Height = myRect.Width * 1.41512605 'basierend auf 842,595
        'End If
    End Sub

    Private Shared Sub setMyPdfRectA4(modus As String, altesRechteck As Rectangle)
        Dim w, h As Double
        w = altesRechteck.Width
        h = altesRechteck.Height
        If modus = "quer" Then
            If h > w Then
                auswahlRechteck.Width = h
                auswahlRechteck.Height = w
            Else
                auswahlRechteck.Width = w
                auswahlRechteck.Height = h
            End If
        Else 'hochformat
            If h > w Then
                auswahlRechteck.Width = w
                auswahlRechteck.Height = h
            Else
                auswahlRechteck.Width = h
                auswahlRechteck.Height = w
            End If
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
            l("Fehler in getAltermittelpunkt: ", ex)
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
            markWMSlayers(layersSelected)
            layerActive.aid = layerHgrund.aid
            layerHgrund.isactive = True
            pgisTools.getStamm4aid(layerActive)
            layerActive.mithaken = True
            layerActive.isactive = True
            layerActive.iswms = layerHgrund.iswms
            layerActive.RBischecked = True
            If layerActive.iswms Then
                markwmslayerSingle(layerActive)
                markwmslayerSingle(layerHgrund)
                'MsgBox("layerActive.iswms a " & layerActive.iswms)
                panningAusschalten()
                WebBrowser1.Visibility = Visibility.Collapsed
                cvtop.Cursor = Cursors.Hand
                CanvasClickModus = "wmsdatenabfrage"
            End If
            showLayersliste()
        Else
            layerActive.aid = 0
            layerHgrund.isactive = False
            layerHgrund.RBischecked = False
            CanvasClickModus = ""
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

        schowLegendeDoku(aktaid)
    End Sub

    Private Sub cmbMasstabDruck_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If Not pdf_mapdruckComboInitFertig Then Exit Sub
        Dim bal = cmbMasstabDruck.SelectedItem
        If bal Is Nothing Then Exit Sub

        Dim item As clsMasstab = CType(cmbMasstabDruck.SelectedItem, clsMasstab)
        If item Is Nothing Then
            Exit Sub
        End If

        Dim MasstabAusgewaehlt As String
        Dim rectWidthInPixel, rectHoheInPixel As Double
        initDruckMasstabCombo(CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked), cvtop.Width, cvtop.Height)
        cmbMasstabDruck.ItemsSource = druckMasstaebe

        MasstabAusgewaehlt = calcNewScreenScale(item.intval, rectWidthInPixel, rectHoheInPixel,
                                  CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked),
                                  cvtop.Width, cvtop.Height)
        If masstabtools.rectIstZuGross(rectWidthInPixel, rectHoheInPixel, cvtop.Width, cvtop.Height, 25) Then

            tbPdfMasstabserror.Text =
                "WICHTIG: Massstab ist zu klein für diesen Ausschnitt. Bitte: " &
                            " 1. Verlassen Sie die PDF-Druckfunktion" &
                            " 2. vergrößern Sie den Ausschnitt und " &
                            " 3. wiederholen Sie die Auswahl"
        Else
            tbMasstabDruck.Text = MasstabAusgewaehlt
            auswahlRechteck.Width = rectWidthInPixel
            auswahlRechteck.Height = rectHoheInPixel

            Debug.Print("auswahlRechteck " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)
            PDF_postition_desRahmensAbfragen(auswahlRechteck)
            PDF_druckMassStab = calcPDFMassstab(PDF_PrintRange, CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked))
        End If
        cmbMasstabDruck.SelectedItem = Nothing
    End Sub



    Private Sub btnFlstsuchen_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        resizeWindow()
        panningAusschalten()
        flurstueckssuche()
        cbSOeinschalten.IsChecked = True
        myglobalz.mgisBackModus = False
        'imgpin.Visibility = Visibility.Visible
        e.Handled = True
    End Sub

    Private Sub btnAdressesuchen_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        resizeWindow()
        myglobalz.mgisBackModus = False
        panningAusschalten()
        adresssuche()
        cbSOeinschalten.IsChecked = True
        imgpin.Visibility = Visibility.Visible
        e.Handled = True
    End Sub

    Private Sub btnKoordinatesuchen_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        resizeWindow()
        myglobalz.mgisBackModus = False
        utmKoordinate()
        ' imgpin.Visibility = Visibility.Visible
        e.Handled = True
    End Sub


    Private Sub cmbSuchezwei_SelectionChanged_1(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnOptionen_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        Dim optmen As New winOption
        optmen.ShowDialog()
        If optmen.kartenHintergrundGrau Then
            cv0.Background = Brushes.LightGray
        Else
            cv0.Background = Brushes.White
        End If
        If optmen.cbUseCache.IsChecked Then
            strGlobals.UseDownloadCache = True
        Else
            strGlobals.UseDownloadCache = False
        End If
        e.Handled = True
    End Sub

    Private Sub tbVorgangsid_TextChanged(sender As Object, e As TextChangedEventArgs)
        aktvorgangsid = tbVorgangsid.Text
        e.Handled = True
    End Sub

    'Private Sub btnObjektsuche_Click(sender As Object, e As RoutedEventArgs)
    '    If Not ladevorgangAbgeschlossen Then Exit Sub
    '    zeigeObjektsuche("")
    '    e.Handled = True
    'End Sub

    Private Sub zeigeObjektsuche(titel As String)
        'stContext.Width = cvtop.Width '- 100
        'stContext.Height = cvtop.Height '- 100
        stpObjektsuche.Width = stContext.Width - stpKnoeppeVertical.Width - 50 '50=margins
        stContext.Visibility = Visibility.Visible
        sichtbarMachenObjektsuche()
        'schema und Tabelle holen
        'sid und aktaid exitsierren hier bereits
        os_tabelledef = New clsTabellenDef
        Debug.Print("")
        'btnObjektsuche.ToolTip = "Objektsuche verfügbar"
        'btnObjektsuche.Visibility = Visibility.Collapsed

        If iminternet Or CGIstattDBzugriff Then
            Dim hinweis As String = ""
            os_tabelledef.aid = CType(aktaid, String)
            os_tabelledef.tab_nr = CType(1, String)
            os_tabelledef = clsToolsAllg.getSchemaFromHTTP(os_tabelledef, hinweis)
            'korrektur_erstmal_eingespart
        Else
            os_tabelledef = ModsachdatenTools.getSChemaDB(aktaid, 1)
            If os_tabelledef Is Nothing Then

                os_tabelledef.gid = "0"

                os_tabelledef.tab_nr = CType(1, String)
            End If
            os_tabelledef.datenbank = "postgis20"
            os_tabelledef.aid = CStr(aktaid)
            korrigiereTabellenSchemaFallsEintraegeFalschDB(os_tabelledef)
        End If
        tbHinweisObjektsuche.Text = "Objektsuche_" & titel
        refreshOS("", "")
    End Sub

    Private Sub refreshOS(os_kat As String, os_feld As String)
        If os_kat.StartsWith("___") Then os_kat = ""
        Dim hinweis As String = ""
        Dim volltextsucheSQL As String = ""
        l("refreshOS " & os_kat & ", " & os_feld)
        cmbOSKat.DataContext = getOSkategorienliste(hinweis)
        If Not os_feld.Trim.IsNothingOrEmpty Then
            If iminternet Or CGIstattDBzugriff Then
                volltextsucheSQL = " lower(ofeldsuche) like '%" & os_feld.Trim.ToLower & "%'"
            Else
                volltextsucheSQL = bildeOSVolltextsucheDB(os_tabelledef.Schema, os_tabelledef.tabelle, os_feld.Trim)
            End If
            If Not volltextsucheSQL.IsNothingOrEmpty Then
                volltextsucheSQL = "   " & volltextsucheSQL & " "
            End If
        End If

        OSrec.mydb.SQL = bildeSQLString(os_tabelledef.Schema, os_tabelledef.tabelle, os_kat, os_feld, volltextsucheSQL)
        l(OSrec.mydb.SQL)
        Dim oslIntColl As New List(Of String())
        Dim linearray As String()
        ReDim linearray(20)
        Dim dataanz As Integer
        Dim LastColNames() As String
        If iminternet Or CGIstattDBzugriff Then
            Dim result = clsToolsAllg.getSQL4Http(OSrec.mydb.SQL, "postgis20", hinweis, "getsql")
            l(hinweis)
            result = result.Trim
            If result.IsNothingOrEmpty Then
                oslIntColl = Nothing
            End If
            oslIntColl = clsToolsAllg.bildeOSInt_arrayColl_ajax(result)
            setFirstColumnsInvisible(8)
            ModsachdatenTools.getColnames(os_tabelledef.Schema & "." & os_tabelledef.getOSTabellenName, LastColNames, hinweis)

            schreibespaltenkoepfeCOLL(LastColNames, 8)
            dgObjektsuche.ItemsSource = oslIntColl
            dgObjektsuche.Visibility = Visibility.Visible
            btnOS2CSV.Visibility = Visibility.Visible
            tbOS_Result.Text = "Für diese Objektart wurden " & dataanz & " Objekte gefunden! " &
                "Klicken Sie ein Objekt an für weitere Aktionen!" & Environment.NewLine &
                "Zum Sortieren klicken Sie auf die Spaltenköpfe"
        Else
            hinweis = OSrec.getDataDT()
            dataanz = OSrec.dt.Rows.Count
            If OSrec.dt.Rows.Count < 1 Then
                dgObjektsuche.ItemsSource = Nothing
                tbOS_Result.Text = "Für diese Objektart ist keine Objektsuche eingerichtet!"
                dgObjektsuche.Visibility = Visibility.Collapsed
                btnOS2CSV.Visibility = Visibility.Collapsed
            Else
                oslIntColl = clsToolsAllg.bildeOSInt_arrayCollDB(OSrec)
                setFirstColumnsInvisible(8)
                dgObjektsuche.DataContext = OSrec.dt
                basisrec = tools.holeSpaltenKoepfe(basisrec, os_tabelledef.Schema, os_tabelledef.getOSTabellenName)
                schreibeSpaltenkoepfeDT(basisrec)
                dgObjektsuche.ItemsSource = oslIntColl
                dgObjektsuche.Visibility = Visibility.Visible
                btnOS2CSV.Visibility = Visibility.Visible
                tbOS_Result.Text = "Für diese Objektart wurden " & dataanz & " Objekte gefunden! " &
                    "Klicken Sie ein Objekt an für weitere Aktionen!" & Environment.NewLine &
                    "Zum Sortieren klicken Sie auf die Spaltenköpfe"
            End If
        End If
    End Sub

    Private Sub schreibespaltenkoepfeCOLL(lastColNames() As String, anzahl As Integer)
        Try
            l("schreibespaltenkoepfeCOLL---------------------- anfang")
            For i = anzahl To lastColNames.Count - 1
                dgObjektsuche.Columns(i).Header = clsString.Capitalize(lastColNames(i))
            Next
            'For j = 0 To basisrec.dt.Rows.Count - 1
            '    '  If (j) > (basisrec.dt.Rows.Count - 1) Then Exit For
            '    dgObjektsuche.Columns(j).Header = clsString.Capitalize(clsDBtools.fieldvalue(basisrec.dt.Rows(j).Item(0)))
            'Next
            l("schreibespaltenkoepfeCOLL---------------------- ende")
        Catch ex As Exception
            l("Fehler in schreibespaltenkoepfeCOLL: ", ex)
        End Try
    End Sub

    Private Function getOSkategorienliste(hinweis As String) As String()
        Dim katstring(), SQL, result As String
        Try
            l(" MOD getOSkategorienliste anfang")
            If iminternet Or CGIstattDBzugriff Then
                SQL = "Select distinct okategorie from  " & os_tabelledef.Schema & ".os_" & os_tabelledef.tabelle &
                     " order by okategorie"
                result = clsToolsAllg.getSQL4Http(SQL, "postgis20", hinweis, "getsql")
                l(hinweis)
                result = result.Trim
                If result.IsNothingOrEmpty Then
                    Return Nothing
                End If
                katstring = clsToolsAllg.ajaxMakeOSkat(result)
            Else
                katstring = initOSComboBoxArrayDB(os_tabelledef.Schema, os_tabelledef.tabelle)
            End If
            l(" MOD getOSkategorienliste ende")
            Return katstring
        Catch ex As Exception
            l("Fehler in getOSkategorienliste: ", ex)
            Return Nothing
        End Try
    End Function

    Private Function bildeOSVolltextsucheDB(schema As String, tabelle As String, suchstring As String) As String
        Dim hinweis, dtyp As String
        Dim startspalte As Integer = 9
        Dim icount As Integer = startspalte
        Dim sb As New Text.StringBuilder
        Try
            l("bildeOSVolltextsuche---------------------- anfang")
            OSrec.mydb.SQL = "Select * from  " & schema & ".os_" & tabelle
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
            l("Fehler inbildeOSVolltextsuche : ", ex)
            Return ""
        End Try
    End Function
    Private Function initOSComboBoxArrayDB(schema As String, tabelle As String) As String()
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
            l("Fehler in initOSComboBox: ", ex)
            Return Nothing
        End Try
    End Function

    Private Sub setFirstColumnsInvisible(anzahl As Integer)
        For i = 0 To anzahl
            dgObjektsuche.Columns(i).Visibility = Visibility.Collapsed
        Next
    End Sub



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




    Async Sub dgObjektsuche_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
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
        If Not clsSachdatentools.splitDBinfo(fulllink) Then
            MsgBox("fehler im Fulllink: " & fulllink)
        End If

        Dim actionwin As New winboxOS(clsSachdatentools.pdfDateinameISOk(pdfspalte), pdfspalte)
        actionwin.ShowDialog()

        Select Case actionwin.aktion
            Case "pdfdateizumobjektladen"
                If clsSachdatentools.pdfDateinameISOk(pdfspalte) Then
                    Dim _pdfDatei As String '                    'pdfspalte /fkat/bplanDietzenbach/di_4/di_4.pdf 
                    If pdfspalte.ToString.ToLower.Contains("bplan") Then
                        _pdfDatei = clsSachdatentools.makeLokalBplaneDatei(pdfspalte.ToString.ToLower, strGlobals.UseDownloadCache)
                        OpenDokument(_pdfDatei)
                    Else
                        _pdfDatei = clsSachdatentools.makeLokalDatei(pdfspalte.ToString.ToLower, os_tabelledef.tabelle)
                        OpenDokument(_pdfDatei)
                    End If

                End If
            Case "dbabfrage"
                '    'ist ein fulllink für die DB vorhanden? wenn ja verwenden
                If clsSachdatentools.splitDBinfo(fulllink) Then 'planung,bebauungsplan_f,10
                    os_tabelledef.aid = CStr(aktaid)
                    If os_tabelledef.tabelle.IsNothingOrEmpty Then 'das ist neu!!! huier bezieht er sich damit auf fulllink und die darin enthaltene tabelle
                        os_tabelledef.tabelle = clsSachdatentools.getTabname4tabnr(aktaid, "1")
                    End If
                    korrigiereTabellenSchemaFallsEintraegeFalschDB(os_tabelledef)
                    If Not os_tabelledef.tabelle.ToLower.StartsWith("os_") Then
                        os_tabelledef.tab_nr = clsSachdatentools.getTabnr4Tabname(os_tabelledef.Schema, os_tabelledef.tabelle)
                    End If
                    os_tabelledef.os_tabellen_name = os_tabelledef.getOSTabellenName
                    'If os_tabelledef.linkTabs.IsNothingOrEmpty Then
                    'Else
                    '    os_tabelledef.gid = getGID4OS_tabelle(os_tabelledef)
                    'End If
                Else
                    os_tabelledef.tabelle = clsSachdatentools.getTabname4tabnr(aktaid, "1")
                    korrigiereTabellenSchemaFallsEintraegeFalschDB(os_tabelledef)
                    If Not os_tabelledef.tabelle.ToLower.StartsWith("os_") Then
                        os_tabelledef.tab_nr = clsSachdatentools.getTabnr4Tabname(os_tabelledef.Schema, os_tabelledef.tabelle)
                    End If
                    os_tabelledef.os_tabellen_name = os_tabelledef.getOSTabellenName
                    If Not os_tabelledef.linkTabs.IsNothingOrEmpty Then
                        os_tabelledef.gid = clsSachdatentools.getGID4OS_tabelle(os_tabelledef)
                    End If
                    l("fehler in flullinkdb")
                    MsgBox("fehler im Fulllinkdb: " & fulllink)
                End If
                modOStools.os_dbanzeigen(paradigmaVID, ebenentitel)

            Case "zurkarte"
                tabellenErmitteln()
                If os_tabelledef.tabelle.IsNothingOrEmpty Then
                    os_tabelledef.tabelle = clsSachdatentools.getTabname4tabnr(aktaid, "1")
                End If
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

                    Dim tempTabdef As New clsTabellenDef
                    tempTabdef = clsTabellenDef.copyTabdef(os_tabelledef)

                    'puffererzeugt = modEW.bildePufferFuerPolygon(aktPolygon, 0.001, os_tabelledef, puffer_area, acanvas, True)
                    puffererzeugt = modEW.bildePufferFuerPolygon(aktPolygon, 0.001, tempTabdef, puffer_area, acanvas, True)
                    If puffererzeugt Then
                        'tools.geometieNachParadigmaUebernehmen(aktvorgangsid, aktPolygon)
                        If modParadigma.GeometrieNachParadigma(aktPolygon, aktPolyline) Then
                            clsToolsAllg.userlayerNeuErzeugen(GisUser.nick, myglobalz.aktvorgangsid)
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
            os_tabelledef.tab_nr = clsSachdatentools.getTabnr4Tabname(os_tabelledef.Schema, os_tabelledef.tabelle)
        End If
        stContext.Visibility = Visibility.Collapsed
        'If os_tabelledef.os_tabellen_name.IsNothingOrEmpty OrElse (Not os_tabelledef.os_tabellen_name.ToLower.StartsWith("os_")) Then
        os_tabelledef.os_tabellen_name = os_tabelledef.getOSTabellenName '"os_" & os_tabelledef.tabelle.Replace("os_", "")
        'End If
    End Sub

    Private Function getParadigmaVID(v As String) As String
        Try
            l("getParadigmaVID---------------------- anfang")

            Return v
            l("getParadigmaVID---------------------- ende")
        Catch ex As Exception
            l("Fehler in getParadigmaVID : ", ex)
            Return ""
        End Try
    End Function



    Private Sub btncloseOS_Click(sender As Object, e As RoutedEventArgs)
        stContext.Visibility = Visibility.Collapsed
        e.Handled = True
    End Sub

    Private Sub zeigeKreisUebersicht(sender As Object, e As MouseEventArgs)
        e.Handled = True
        Exit Sub
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
        e.Handled = True
        Mouse.Capture(Nothing)
        KoordinateKLickpt = e.GetPosition(imgkreisuebersicht)
        CanvasClickModus = ""
        If tools.liegtImkreisOffenbach(KoordinateKLickpt) Then
            Dim neupunktString As String
            neupunktString = KreisUebersichtkoordinateKlickBerechnen(KoordinateKLickpt)
            splitKoordinatenstring(neupunktString)
            kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, 1500)
            setBoundingRefresh(kartengen.aktMap.aktrange)
        End If

        kreisUebersichtUnsichtbar()


        refreshMap(True, True)

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
        favoTools.FavoritSave(GisUser.nick)
        showInfo("Die Zussammenstellung wurde in 'Meine Favoriten' gespeichert")
        e.Handled = True
    End Sub

    Private Sub showInfo(v As String)
        MsgBox(v)
    End Sub

    Private Sub zwischenbildBitteWarten()

        tbinfohgrund.Visibility = Visibility.Visible
        If slots(0).refresh Then
            If slots(0).layer.titel.ToLower = "kein hintergrund" Then
                'slots(0).setEmpty()
                'myBitmapImage = New BitmapImage()
                'myBitmapImage.BeginInit()
                'myBitmapImage.UriSource = New Uri("/mgis;component/icons/leer.png", UriKind.RelativeOrAbsolute)
                'myBitmapImage.EndInit()
                'slots(0).image.Source = myBitmapImage
                slots(0).setEmpty()
                tbinfohgrund.Visibility = Visibility.Collapsed
            End If
        End If
        Exit Sub
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
                    tbinfohgrund.Visibility = Visibility.Collapsed
                Else
                    myBitmapImage = New BitmapImage()
                    myBitmapImage.BeginInit()
                    myBitmapImage.UriSource = New Uri("/mgis;component/icons/bwv.png", UriKind.RelativeOrAbsolute)
                    myBitmapImage.EndInit()
                    slots(0).image.Source = myBitmapImage
                    'slots(0).image.Width = 600
                    'slots(0).image.Width = 300
                End If

            End If
            Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
            If slots(1).refresh Then
                myBitmapImage = New BitmapImage()
                myBitmapImage.BeginInit()
                myBitmapImage.UriSource = New Uri("/mgis;component/icons/bwh.png", UriKind.RelativeOrAbsolute)
                myBitmapImage.EndInit()
                slots(1).image.Source = myBitmapImage
                'slots(1).image.Width = 600
                'slots(1).image.Width = 300
            End If
            Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
            Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
        Catch ex As Exception
            l("Fehler in : zwischenbild ---------ende-----------------", ex)
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
            l("dgOSliste_SelectionChanged ", ex)
            Exit Sub
        End Try
        e.Handled = True
    End Sub



    Private Sub btnOSdropdown_Click(sender As Object, e As RoutedEventArgs)
        MsgBox("asdsdasd")
        e.Handled = True
    End Sub

    Private Sub tbOSfilter_TextChanged(sender As Object, e As TextChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If tbOSfilter.Text.Length < 2 Then Exit Sub
        dgOSliste.DataContext = Nothing
        clsWebgisPGtools.getOSliste(allLayersPres, tbOSfilter.Text.ToLower)
        dgOSliste.DataContext = allOSLayers

    End Sub
    Private Sub btnPNG_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        panningAusschalten()
        Dim ausrichtung As String
        ausrichtung = "quer"
        PDF_PrintRange.xl = kartengen.aktMap.aktrange.xl
        PDF_PrintRange.xh = kartengen.aktMap.aktrange.xh
        PDF_PrintRange.yl = kartengen.aktMap.aktrange.yl
        PDF_PrintRange.yh = kartengen.aktMap.aktrange.yh
        Dim hochaufloesend As Boolean = False
        Dim ausgabedatei As String = ""
        Dim localfile As String = ""

        'makeandloadPDF("842", "595", "mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, True, hochaufloesend)
        makeandloadPDF("mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, True, hochaufloesend,
                       CBool(rbFormatA4.IsChecked), False, ausgabedatei, CBool(cbmitsuchobjekt.IsChecked), localfile, layersSelected)
        opendirec(localfile)
        strGlobals.paintProgramm = clsStartup.setPaintsoftware(strGlobals.paintProgramm)
        OpenDokumentWith(strGlobals.paintProgramm, localfile)
    End Sub

    Private Shared Sub opendirec(ausgabedatei As String)
        Try
            l("opendirec---------------------- anfang")
            l("ausgabedatei " & ausgabedatei)
            Dim fi As New IO.FileInfo(ausgabedatei)
            Process.Start(fi.DirectoryName)
            fi = Nothing
            l("opendirec---------------------- ende")
        Catch ex As Exception
            l("Fehler in opendirec: " & ausgabedatei & "///", ex)
        End Try
    End Sub

    Private Sub btnNaturSG_Click(sender As Object, e As RoutedEventArgs)
        LastThemenSuche = "hauptsachgebiet"
        dgErgebnis.ItemsSource = Nothing
        SuchLayersList = modLayer.getLayer4sachgebiet("schutzgebiete")
        SuchLayersList.Sort()
        clsWebgisPGtools.dombineLayerDoku(SuchLayersList, allDokus)
        dgErgebnis.ItemsSource = SuchLayersList
        e.Handled = True
    End Sub

    Private Sub btnTools_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        Dim optmen As New winEigentuemer4Polygon
        optmen.Show()
        refreshMap(True, True)
        e.Handled = True
    End Sub
    Private Sub zindexeSetzen()
#Disable Warning BC42025 ' Access of shared member, constant member, enum member or nested type through an instance
        tigis.SetZIndex(dockMap, 200) ' wg panning nur 200

        tigis.SetZIndex(dockMenu, 300)
        tigis.SetZIndex(dockTop, 300)

        dockMap.SetZIndex(cv0, 5)
        'dockMap.SetZIndex(HGabdecker, 6)
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

        dockMap.SetZIndex(cv21, 31)
        dockMap.SetZIndex(cv22, 32)
        dockMap.SetZIndex(cv23, 33)
        dockMap.SetZIndex(cv24, 34)
        dockMap.SetZIndex(cv25, 35)
        dockMap.SetZIndex(cv26, 36)
        dockMap.SetZIndex(cv27, 37)
        dockMap.SetZIndex(cv28, 38)
        dockMap.SetZIndex(cv29, 39)
        dockMap.SetZIndex(cv30, 40)
        dockMap.SetZIndex(cv31, 41)
        dockMap.SetZIndex(cv32, 42)
        dockMap.SetZIndex(cv33, 43)
        dockMap.SetZIndex(cv34, 44)
        dockMap.SetZIndex(cv35, 45)
        dockMap.SetZIndex(cv36, 46)
        dockMap.SetZIndex(cv37, 47)
        dockMap.SetZIndex(cv38, 48)
        dockMap.SetZIndex(cv39, 49)
        dockMap.SetZIndex(cv40, 50)
        dockMap.SetZIndex(cv41, 51)
        dockMap.SetZIndex(cv42, 52)
        dockMap.SetZIndex(cv43, 53)
        dockMap.SetZIndex(cv44, 54)
        dockMap.SetZIndex(cv45, 55)
        dockMap.SetZIndex(cv46, 56)
        dockMap.SetZIndex(cv47, 57)
        dockMap.SetZIndex(cv48, 58)
        dockMap.SetZIndex(cv49, 59)
        dockMap.SetZIndex(cv50, 60)

        dockMap.SetZIndex(cvtop, 80)

        dockMap.SetZIndex(WebBrowser1, 100)
        dockMap.SetZIndex(imgpin, 110)
        dockMap.SetZIndex(stckBalken, 110)
        dockMap.SetZIndex(suchCanvas, 250)
        dockMap.SetZIndex(stwinthemen, 500)
        dockMap.SetZIndex(cvPDFrechteck, 400)
        dockMap.SetZIndex(stContext, 500)

        'imageMapCanvas.SetZIndex(imgKarte, 100) 'macht keinen sinn 

        dockMenu.SetZIndex(stPDFDruck, 300)
        dockMenu.SetZIndex(stMenu, 300)
#Enable Warning BC42025 ' Access of shared member, constant member, enum member or nested type through an instance
    End Sub

    'Private Sub sldHgrundOpac_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
    '    setHGabdecker4SliderValue(CInt(sldHgrundOpac.Value))
    '    'HGcanvasImageRange0.Opacity = CInt(sldHgrundOpac.Value)
    '    'HGcanvasImageRange0. = CInt(sldHgrundOpac.Value)
    '    e.Handled = True
    'End Sub

    'Private Sub setHGabdecker4SliderValue(val As Integer)
    '    Dim abdeckfarbe As New SolidColorBrush
    '    abdeckfarbe = New SolidColorBrush(Color.FromArgb(CByte(val), 255, 255, 255)) ' 
    '    HGabdecker.Background = abdeckfarbe
    '    abdeckfarbe = Nothing
    'End Sub

    'Private Sub cmbHgrund_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
    '    btnHGSlider.IsOpen = True
    '    e.Handled = True
    'End Sub

    'Private Sub btnSliderSchliessen_Click(sender As Object, e As RoutedEventArgs)
    '    schliesseSliderDialog()
    '    e.Handled = True
    'End Sub

    'Private Sub schliesseSliderDialog()
    '    btnHGSlider.IsOpen = False
    'End Sub

    'Private Sub sldVGrundOpac_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
    '    If Not ladevorgangAbgeschlossen Then Exit Sub
    '    '' setHGabdecker4SliderValue(CInt(sldHgrundOpac.Value))
    '    cvtop.Opacity = CDbl(sldVGrundOpac.Value)
    '    slots(1).image.Opacity = CDbl(sldVGrundOpac.Value)
    '    tbInfopanel.Text = CType(CDbl(sldVGrundOpac.Value), String)
    '    'e.Handled = True
    'End Sub

    Private Sub rbchkMitMasstab_Checked(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        initPdfauswahlRechteck()
        pdf_mapdruckComboInitFertig = False
        Debug.Print("auswahlRechteck " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)
        initDruckMasstabCombo(CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked), cvtop.Width, cvtop.Height)
        cmbMasstabDruck.ItemsSource = druckMasstaebe
        pdf_mapdruckComboInitFertig = True
        If rbMitMasstab.IsChecked Then
            spdruckmasstab.Visibility = Visibility.Visible
            spAusrichtung.Visibility = Visibility.Visible

            'auswahlRechteck.Width = 0
            'auswahlRechteck.Height = 0
            If quer.IsChecked Then

                auswahlRechteck.Width = 495
                auswahlRechteck.Height = 350
                pdfrahmenNeuPLatzieren("quer")
            Else
                auswahlRechteck.Width = 350
                auswahlRechteck.Height = 495
                pdfrahmenNeuPLatzieren("hoch")
            End If
            cvPDFrechteck.Visibility = Visibility.Visible
            'PDF_druckMassStab = PDF_postition_desRahmensBestimmen(auswahlRechteck)

            Debug.Print("auswahlRechteck " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)
            PDF_postition_desRahmensAbfragen(auswahlRechteck)
            PDF_druckMassStab = calcPDFMassstab(PDF_PrintRange, CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked))

            tbMasstabDruck.Text = CInt(PDF_druckMassStab).ToString
        Else
            cvPDFrechteck.Visibility = Visibility.Collapsed
            spdruckmasstab.Visibility = Visibility.Hidden
            spAusrichtung.Visibility = Visibility.Hidden
        End If
        Debug.Print("auswahlRechteck " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)
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
        'PDF_druckMassStab = PDF_postition_desRahmensBestimmen(auswahlRechteck)

        PDF_postition_desRahmensAbfragen(auswahlRechteck)
        PDF_druckMassStab = calcPDFMassstab(PDF_PrintRange, CBool(rbFormatA4.IsChecked), CBool(quer.IsChecked))

        ausrichtung = getAusrichtung(CBool(quer.IsChecked))
        If cbhochaufloesend.IsChecked Then
            hochaufloesend = True
        Else
            hochaufloesend = False
        End If
        Dim localfile As String = ""
        'makeandloadPDF("842", "595", "mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, False, hochaufloesend)
        makeandloadPDF("mitmasstab", PDF_PrintRange, CDbl(tbMasstabDruck.Text), ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, False,
                       hochaufloesend, CBool(rbFormatA4.IsChecked), False, ausgabedatei, CBool(cbmitsuchobjekt.IsChecked), localfile, PDFlayers)
    End Sub

    Private Sub btnMakePDFohnemass_Click(sender As Object, e As RoutedEventArgs)
        Dim ausgabedatei As String = ""
        PDFohneMasstab(False, ausgabedatei)
        e.Handled = True
    End Sub

    Private Function PDFohneMasstab(schnelldruck As Boolean, ByRef ausgabedatei As String, Optional ByRef batchfile As String = "") As Boolean
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
        If schnelldruck Then hochaufloesend = False
        ausrichtung = getAusrichtung(CBool(quer.IsChecked))
        PDF_druckMassStab = 0
        'Dim localfile As String = ""
        'makeandloadPDF("842", "595", "mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text, False, hochaufloesend)
        If makeandloadPDF("ohnemasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, tbPDF_Bemerkung.Text, tbPDF_Ort.Text,
                       False, hochaufloesend, CBool(rbFormatA4.IsChecked), schnelldruck, ausgabedatei, CBool(cbmitsuchobjekt.IsChecked), batchfile, PDFlayers) Then
            'schnelldruck ausgabedatei: http://w2gis02.kreis-of.local/cache/MS15502453701420.pdf 
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnPrintPdf_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Debug.Print(OSrefresh.ToString)
        'Debug.Print(mapfileBILD)
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
        If clsOptionTools.PDFimmerAcrobOeffnenat() Then
            Dim si As New ProcessStartInfo
            If GisUser.favogruppekurz = "ba" Then

            Else

            End If
            If clsOptionTools.PDFreaderExistiert(strGlobals.pdfReader) Then
                si.FileName = strGlobals.pdfReader
                si.WorkingDirectory = "c:\kreisoffenbach\mgis"
                si.Arguments = ausgabedatei
                Process.Start(si)
                myglobalz.userIniProfile.WertSchreiben("Diverse", "PDFimmerAcrobat", "1")
            Else
                myglobalz.userIniProfile.WertSchreiben("Diverse", "PDFimmerAcrobat", "0")
                OpenDokument(ausgabedatei)
            End If
        Else
            myglobalz.userIniProfile.WertSchreiben("Diverse", "PDFimmerAcrobat", "0")
            OpenDokument(ausgabedatei)
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
        disableMyStackpanel(spObereButtonMenu, True)
    End Sub

    Private Sub txtitel_MouseDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
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
                    If lay.iswms Then
                        schowLegendeDoku(aktaid)
                        'MessageBox.Show("Bei WMS-Diensten lassen sich die Objekte nicht einzeln suchen." & Environment.NewLine, "WMS-Dienst", MessageBoxButton.OK, MessageBoxImage.Information)
                    Else
                        zeigeObjektsuche(titel)

                    End If
                Else
                    'zeigeLegendeUndDoku(aktaid, aktsid, lay.mit_objekten)
                    schowLegendeDoku(aktaid)
                End If
            End If
        Next

    End Sub

    Private Sub btnHandbuch_Click(sender As Object, e As RoutedEventArgs)
        panningAusschalten()
        'Dim aaa As New winHandbuch obsolet
        'aaa.ShowDialog()
        OpenDokument(strGlobals.gisguidedocx)
        e.Handled = True
    End Sub

    Private Sub btnGruppeFavo_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        panningAusschalten()
        resizeWindow()
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
        If tv1.SelectedItem Is Nothing Then Exit Sub
        Dim tv As New TreeViewItem
        tv = CType(tv1.SelectedItem, TreeViewItem)
        If tv.Tag Is Nothing Then Exit Sub
        tbKategorie.Text = "Kategorie: " & tv.Header.ToString
        treeview2Kat(tv.Tag.ToString.ToLower)
        tbebenenauswahlinfo.Visibility = Visibility.Visible
        e.Handled = True
    End Sub

    Sub treeview2Kat(tag As String)
        LastThemenSuche = "hauptsachgebiet"
        dgErgebnis.ItemsSource = Nothing
        SuchLayersList = modLayer.getLayer4sachgebiet(tag)
        SuchLayersList.Sort()
        clsWebgisPGtools.dombineLayerDoku(SuchLayersList, allDokus)
        dgErgebnis.ItemsSource = SuchLayersList
    End Sub


    Private Sub btnSchnelldruck_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        resizeWindow()
        Dim ausgabedatei As String = "", batchFile = ""
        strGlobals.pdfReader = clsStartup.setPDFreader(strGlobals.pdfReader)
        If PDFohneMasstab(True, ausgabedatei, batchFile) Then
            Microsoft.VisualBasic.Shell(batchFile)
        Else
            'MsgBox("Download gescheidert!")
        End If
    End Sub

    Private Sub btnExplorer_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        holeExplorer()
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
        'Dim nlay As New clsLayerPres
        'nlay.aid = CInt(nck.Tag)
        'pgisTools.getStamm4aid(nlay)
        'showFreiLegende4Aid(nlay)

        schowLegendeDoku(CInt(nck.Tag))
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
        starteParadigma()
    End Sub

    Private Sub starteParadigma()
        'Dim neuervorgangstgring As String
        Try
            l("starteParadigma---------------------- anfang")
            If paradigmaLaeuftschon() Then
                '  initdb den vordergrundholen
            Else
                'neuervorgangstgring = strGlobals.paradigmaExe
                Dim si As New ProcessStartInfo
                si.FileName = strGlobals.paradigmaExe
                si.WorkingDirectory = "c:\kreisoffenbach\main"
                si.Arguments = "modus=neu"
                'Process.Start(neuervorgangstgring, "modus=neu")
                Process.Start(si)
                si = Nothing
            End If

            l("starteParadigma---------------------- ende")
        Catch ex As Exception
            l("Fehler in : ", ex)
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
                If STARTUP_mgismodus = "paradigma" Then
                    clsToolsAllg.userlayerNeuErzeugen(GisUser.nick, myglobalz.aktvorgangsid)
                Else
                    nachricht("fehler userlayer von fremd aufgerufen")
                End If
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
        aktPolygon.clear()
        aktPolyline.clear()
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
        'Dim userid As Integer = getUsersIdFromParadigma(GisUser.nick)
        gisdokstarten(GisUser.nick)
    End Sub

    'Private Shared Function getUsersIdFromParadigma(lusername As String) As Integer
    '    GisUser.PL_UserNr = modPLUser.getUsernr(lusername)
    '    If GisUser.PL_UserNr < 1 Then
    '        GisUser.PL_UserNr = modPLUser.addUser(lusername, GisUser.ADgruppenname)
    '    End If
    'End Function

    Private Function gisdokstarten(lusername As String) As System.Diagnostics.Process
        'Dim datei, param As String
        'datei = strGlobals.PL_bestandexe
        'param = "username=" & GisUser.nick
        'l("gisdokstarten " & param & Environment.NewLine & datei)
        'Dim proc As New Process
        'proc = Process.Start(datei, param) 
        Dim si As New ProcessStartInfo
        si.FileName = strGlobals.PL_bestandexe
        si.WorkingDirectory = "c:\kreisoffenbach\pl"
        si.Arguments = "username=" & GisUser.nick
        'Process.Start(neuervorgangstgring, "modus=neu")
        Dim proc As New Process
        proc = Process.Start(si)
        si = Nothing
        Return proc
    End Function

    Private Sub btnGooglemaps_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'panningAusschalten()
        'tivogel.Visibility = Visibility.Visible
        'tigis.Visibility = Visibility.Collapsed
        'panningAusschalten()
        Dim url, lonlatstring As String
        url = getGoogleMapsString(lonlatstring)
        'webBrowserControlVogel.Navigate(url)
        'Process.Start(url)


        Process.Start("iexplore.exe", url)
        e.Handled = True
    End Sub

    Private Function getGoogleMapsString(ByRef lonlatstring As String) As String
        Try
            nachricht("USERAKTION: googlekarte  vogel")
            Dim gis As New clsGISfunctions
            Dim result As String
            'kartengen.aktMap.aktrange.CalcCenter()
            result = gis.GoogleMapsAufruf_Extern(kartengen.aktMap.aktrange, True, lonlatstring)
            If result = "fehler" Or result = "" Then
                Return ""
            Else
                '  gis.starten(result)
                '  GMtemplates.templateStarten(result)
                Return result
            End If
            gis = Nothing
        Catch ex As Exception
            l("fehler in starteWebbrowserControl2: " & kartengen.aktMap.aktrange.toString(), ex)
            Return ""
        End Try
    End Function

    'Private Sub btnGmapsSchliessen_Click(sender As Object, e As RoutedEventArgs)
    '    btnGmaps.IsOpen = False
    '    e.Handled = True
    'End Sub

    Private Sub btnMessenSchliessen_Click(sender As Object, e As RoutedEventArgs)
        btnMessen.IsOpen = False
        If layerActive.iswms Then
            panningAusschalten()
            WebBrowser1.Visibility = Visibility.Collapsed
            cvtop.Cursor = Cursors.Hand
            CanvasClickModus = "wmsdatenabfrage"
        Else
            WebBrowser1.Visibility = Visibility.Visible
            CanvasClickModus = ""
        End If

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
            cmbMessen.IsDropDownOpen = True
            'messestrecke()
            'btnMessen.IsOpen = True
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
            Dim longitude, latitude As String

            modKoordTrans.getLongLatFromResultSingle(result, longitude, latitude, xyTrenner)
            umreechnerUrl = umreechnerUrl & latitude.Replace(",", ".") & "," & longitude.Replace(",", ".")
        End If
        Process.Start(umreechnerUrl)
    End Sub

    Private Sub btnNachParadigma_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            If aktvorgangsid <> String.Empty Then
                If modParadigma.GeometrieNachParadigma(aktPolygon, aktPolyline) Then
                    clsToolsAllg.userlayerNeuErzeugen(GisUser.nick, myglobalz.aktvorgangsid)
                    MsgBox("Das Objekt wurde in die Paradigma-DB als Raumbezug übernommen. " & Environment.NewLine &
                           "Drücken Sie oben die RefreshTaste um die Änderung anzuzeigen!", MsgBoxStyle.OkOnly, "Datenübernahme OK")
                Else
                    MsgBox("Datenübernahme war nicht erfolgreich. Bitte beim Admin melden!")
                End If
            End If
        End If
    End Sub

    'Private Sub btnWMSgetfeatureinfo_Click(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True
    '    If layerActive.iswms Then
    '        MsgBox("Klicken Sie jetzt einen Punkt in  der Karte an:", , "WMS - Datenabfrage")
    '        panningAusschalten()
    '        WebBrowser1.Visibility = Visibility.Collapsed
    '        cvtop.Cursor = Cursors.Hand
    '        CanvasClickModus = "wmsdatenabfrage"
    '    Else
    '        If clsWMStools.istpointactivemodus(layerActive.aid) Then
    '            MsgBox("Klicken Sie jetzt einen Punkt in  der Karte an:", , "Datenabfrage")
    '            panningAusschalten()
    '            WebBrowser1.Visibility = Visibility.Collapsed
    '            CanvasClickModus = "pointactivemodus"
    '        End If
    '    End If
    '    'btnWMSgetfeatureinfo.Visibility=Visibility.Collapsed
    'End Sub

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
            ausgabeDIR = strGlobals.localDocumentCacheRoot
            l("ausgabeDIR anlegen " & ausgabeDIR)
            IO.Directory.CreateDirectory(ausgabeDIR)
            l("csvAusgabe---------------------- ende")

            outfile = ausgabeDIR & "\liste_" & clsString.date2string(Now, 2) & ".csv"
            l("csvAusgabe " & outfile)
            My.Computer.FileSystem.WriteAllText(outfile, out, False, enc)
            OpenDokument(outfile)
            l("csvAusgabe---------------------- ende")
        Catch ex As Exception
            l("Fehler in csvAusgabe: ", ex)
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
        MsgBox("Klicken Sie jetzt einen Punkt in  der Karte an:", , "Dossierabfrage")
        panningAusschalten()
        WebBrowser1.Visibility = Visibility.Collapsed
        CanvasClickModus = "dossiermodus"
    End Sub

    Private Sub btnMgisHistoryBack_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If mgishistoryBack() Then
            myglobalz.mgisBackModus = True
            refreshMap(True, True)
        Else
            myglobalz.mgisBackModus = False
            'refreshMap(True, True)
        End If
    End Sub

    Private Function mgishistoryBack() As Boolean
        Dim AlleKookieFiles As IO.FileInfo() = Nothing
        Dim reverseKookieFiles As IO.FileInfo() = Nothing
        Dim count As Integer
        Dim idx As Integer = 0
        Try
            l(" mgishistoryBack ---------------------- anfang")
            'dateiliste erstellen
            Dim di As New IO.DirectoryInfo(myglobalz.mgisRangecookieDir)
            'Dim so As New IO.SearchOption
            idx = 1
            If di.Exists Then
                idx = 2
                AlleKookieFiles = di.GetFiles("*.rng")
                idx = 21
                count = AlleKookieFiles.GetUpperBound(0) + 1
                idx = 22
                ReDim reverseKookieFiles(AlleKookieFiles.GetUpperBound(0))
                idx = 23
                nachricht("Es wurden " & count & " HistoryItems gefunden.")
                idx = 24
                nachricht("last" & myglobalz.mgisBackmodusLastCookie)
                idx = 25
                clsRangehistory.RNGhistAufraeumen(AlleKookieFiles, reverseKookieFiles)
                idx = 26
                For i = 0 To reverseKookieFiles.Count - 1
                    l("reverseKookieFiles(i).Name: " & i & "," & reverseKookieFiles(i).Name)
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

                        Return True
                    End If
                Next
                idx = 27
                myglobalz.mgisBackModus = True
                Return False
                'darstellen
            Else
                idx = 3
                Return False
            End If
            l(" mgishistoryBack ---------------------- ende")
        Catch ex As Exception
            l("Fehler in mgishistoryBack: " & idx & ", " & myglobalz.mgisRangecookieDir & ", ", ex)
            Return False
        End Try
    End Function



    Private Sub imageMapCanvas_MouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
        Mouse.Capture(Nothing)
        Dim KoordinateKLickpt As Point?
        KoordinateKLickpt = e.GetPosition(WebBrowser1)
        clsSachdatentools.dossierOhneImap(KoordinateKLickpt)
        setBoundingRefresh(kartengen.aktMap.aktrange)
        suchObjektModus = suchobjektmodusEnum.flurstuecksObjektDarstellen
        refreshMap(True, True)
        e.Handled = True
    End Sub

    Private Sub Window_SizeChanged(sender As Object, e As SizeChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        'btnrefresh.Background = Brushes.Green
        ' resizeWindow()
    End Sub
    Sub resizeWindow()
        Dim maxwidth = 1131
        Try
            l(" MOD resizeWindow anfang")
            'Debug.Print(Left & "," & Top & "," & ActualWidth)
            Width = ActualWidth
            Height = ActualHeight
            If Width < maxwidth Then
                Width = maxwidth
            End If
            If Height < 800 Then
                Height = 800
            End If
            initVGCanvasSize()
            kartengen.aktMap.aktcanvas.w = CLng(cvtop.Width)
            kartengen.aktMap.aktcanvas.h = CLng(cvtop.Height)
            l(" MOD resizeWindow ende")
        Catch ex As Exception
            l("Fehler in resizeWindow: ", ex)
        End Try
    End Sub

    Private Sub slotsResize(width As Double, height As Double)
        Try
            l(" slotsResize ---------------------- anfang")
            '    For i = 0 To 20
            '    slots(i).canvas.Width = width
            '    slots(i).canvas.Height = height
            '    slots(i).image.Width = width

            '    slots(i).image.Height = height
            'Next
            cv0.Width = width : cv0.Height = height
            cv1.Width = width : cv1.Height = height
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
            l(" slotsResize ---------------------- ende")
        Catch ex As Exception
            l("Fehler in slotsResize: ", ex)
        End Try
    End Sub

    Private Sub Window_StateChanged(sender As Object, e As EventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Select Case WindowState
            Case WindowState.Maximized
                If (hatNurEinenBildschirm()) Then
                    Width = System.Windows.SystemParameters.PrimaryScreenWidth
                    Height = System.Windows.SystemParameters.PrimaryScreenHeight
                Else
                    MessageBox.Show("Bitte den Bildschirm auffrischen (Grüner Knopf Oben-Mitte)")
                End If
                initVGCanvasSize()
                'Debug.Print(Width & "," & Height)
                'cvtop.Width = CLng(Width) - CLng(dockMenu.Width)
                'cvtop.Height = CLng(Height) - CLng(dockTop.Height)
                'globCanvasWidth = CInt(cvtop.Width)
                'globCanvasHeight = CInt(cvtop.Height)
                'slotsResize(cvtop.Width, cvtop.Height)
                refreshMap(True, True)
            Case WindowState.Minimized
            Case WindowState.Normal
                Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.8)
                Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.8)
                resizeWindow()
                'btnrefresh.Background = Brushes.Green
                refreshMap(True, True)
        End Select
        resizeWindow()
    End Sub

    Private Function hatNurEinenBildschirm() As Boolean
        Try
            l(" MOD hatNureinenBildschirm anfang")
            If System.Windows.SystemParameters.PrimaryScreenWidth = System.Windows.SystemParameters.VirtualScreenWidth And
               System.Windows.SystemParameters.PrimaryScreenHeight = System.Windows.SystemParameters.VirtualScreenHeight Then
                Return True
            Else
                Return False
            End If
            l(" MOD hatNureinenBildschirm ende")
            Return True
        Catch ex As Exception
            l("Fehler in hatNurEinenBildschirm: ", ex)
            Return False
        End Try
    End Function

    Private Sub rbFormatA3_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        ' Debug.Print("auswahlRechteck " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)
        rbMitMasstab.IsChecked = False : rbOhneMasstab.IsChecked = True : quer.IsChecked = True : hoch.IsChecked = False
        auswahlRechteck = New Rectangle()
        If quer.IsChecked Then
            auswahlRechteck.Width = 700.5
            auswahlRechteck.Height = 495
            pdfrahmenNeuPLatzieren("quer")
        Else
            auswahlRechteck.Width = 495
            auswahlRechteck.Height = 700.5
            pdfrahmenNeuPLatzieren("hoch")
        End If

    End Sub
    Private Sub rbFormatA4_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        rbMitMasstab.IsChecked = False : rbOhneMasstab.IsChecked = True : quer.IsChecked = True : hoch.IsChecked = False

        auswahlRechteck = New Rectangle()
        Debug.Print("auswahlRechteck " & auswahlRechteck.Width & ", " & auswahlRechteck.Height)
        If quer.IsChecked Then
            auswahlRechteck.Width = 495
            auswahlRechteck.Height = 350
            pdfrahmenNeuPLatzieren("quer")
        Else

            auswahlRechteck.Width = 350
            auswahlRechteck.Height = 495
            pdfrahmenNeuPLatzieren("hoch")
        End If
    End Sub
    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        tools.rangeSpeichern(kartengen.aktMap.aktrange)
        favoTools.FavoritSave("zuletzt")
        'tools.dirSpeichern()
        savePosition()
        clsStartup.LegendenCacheLoeschen()
        clsControlling.controllingTransfer(strGlobals.controllingCounter)
        'clsUpdate.Check4Update()
    End Sub
    Private Sub savePosition()
        Try
            l(CType(Me.Top, String))
            l(CType(Me.Left, String))
            l(CType(Me.ActualWidth, String))
            l(CType(Me.ActualHeight, String))
            myglobalz.userIniProfile.WertSchreiben("diverse", "windetailformpositiontop", CType(Me.Top, String))
            myglobalz.userIniProfile.WertSchreiben("diverse", "windetailformpositionleft", CType(Me.Left, String))
            myglobalz.userIniProfile.WertSchreiben("diverse", "windetailformpositionwidth", CType(Me.ActualWidth, String))
            myglobalz.userIniProfile.WertSchreiben("diverse", "windetailformpositionheight", CType(Me.ActualHeight, String))
        Catch ex As Exception
            l("fehler in saveposition  windb", ex)
        End Try
    End Sub
    Shared Function setPosition(kategorie As String, eintrag As String, aktval As Double) As Double
        'Me.Top = clsToolsAllg.setPosition("diverse", "dbabfrageformpositiontop", Me.Top)
        'Me.Left = clsToolsAllg.setPosition("diverse", "dbabfrageformpositionleft", Me.Left)
        Dim retval As Double
        Try
            l(" setPosition ---------------------- anfang")
            Dim topf As String = myglobalz.userIniProfile.WertLesen(kategorie, eintrag)
            If String.IsNullOrEmpty(topf) Then
                myglobalz.userIniProfile.WertSchreiben(kategorie, eintrag, CType(aktval, String))
                retval = aktval
            Else
                retval = CDbl(topf)
            End If
            l(" getIniDossier ---------------------- ende")
            Return retval
        Catch ex As Exception
            l("Fehler in setPosition: ", ex)
            Return aktval
        End Try
    End Function

    Private Sub btnOsZurDokuLegende_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        schowLegendeDoku(aktaid)
    End Sub

    Private Sub schowLegendeDoku(myaid As Integer)
        Dim nlay As New clsLayerPres
        nlay.aid = CInt(myaid)
        pgisTools.getStamm4aid(nlay)
        showFreiLegende4Aid(nlay)
    End Sub

    Private Sub MainWindow_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles Me.SizeChanged
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub

    End Sub

    Private Sub BtnDatenschutz_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        clsStartup.Datenschutz()
    End Sub
    Private Sub schreibeSpaltenkoepfeDT(basisrec As clsDBspecPG)
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

    Private Sub btnEmail_Click(sender As Object, e As RoutedEventArgs)

        'PNG-Datei erstellen
        e.Handled = True
        Dim dia As New winSendEmail()
        dia.Show()
    End Sub

    Private Sub OnIsBrowserInitializedChanged3D(sender As Object, e As DependencyPropertyChangedEventArgs)
        aktGlobPoint.X = kartengen.aktMap.aktrange.xcenter
        aktGlobPoint.Y = kartengen.aktMap.aktrange.ycenter
        google3dintro()
    End Sub

    Private Sub OnNavigate(sender As Object, e As RequestNavigateEventArgs)
        Process.Start(e.Uri.AbsoluteUri)
        e.Handled = True
    End Sub

    Private Sub TbEbenensuche_TextChanged(sender As Object, e As TextChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        tbStichwort.Text = tbEbenensuche.Text
        If tbEbenensuche.Text.EndsWith(vbCrLf) Then
            tbEbenensuche.Text = tbEbenensuche.Text.Replace(vbCrLf, "")
            tbStichwort.Text = tbEbenensuche.Text
            tbEbenensuche2.Text = tbEbenensuche.Text.Trim
            'makeThemenVis()
            'treeview2Kat("Grenzen".ToLower)
            'tbebenenauswahlinfo.Visibility = Visibility.Visible
            'tbKategorie.Text = "Kategorie: Grenzen"

            stichwortsucheDurchfuehr()
            'FocusManager.SetFocusedElement(Me, btnEbenensuche)
        End If
        If tbEbenensuche.Text.Length = 0 Then
            makeThemenInVis()
        End If
        If tbEbenensuche.Text.Length < 1 Then
            btnStichwortsuchebeenden.IsEnabled = False
            btnEbenensuche.IsEnabled = False
        Else
            btnStichwortsuchebeenden.IsEnabled = True
            btnEbenensuche.IsEnabled = True
        End If
    End Sub

    Private Sub BtnEbenensuche_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True

        makeThemenVis()
        treeview2Kat("Grenzen".ToLower)
        tbebenenauswahlinfo.Visibility = Visibility.Visible
        tbKategorie.Text = "Kategorie: Grenzen"

        stichwortsucheDurchfuehr()
        FocusManager.SetFocusedElement(Me, btnEbenensuche)
    End Sub

    Private Sub BtnStichwortsuchebeenden_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbEbenensuche.Text = ""
        tbEbenensuche2.Text = ""
        makeThemenInVis()
    End Sub

    Private Sub Tivogel_MouseDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        wbvogelisinit = False
        refreshVogel()

    End Sub

    Private Sub AddTicket(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        wbvogelisinit = True
        refreshVogel()
    End Sub
    Private Sub BtnKreisuiebersicht_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        ' Exit Sub
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

    'Private Sub btnKompress_Click(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True
    '    removeJustAdded()
    '    Dim komprimieren As Boolean = True
    '    komprimierteLayerMarkieren(komprimieren)
    '    leereSelectedlayersNachPres(layersSelected)
    '    layersSelected.Sort()
    '    showLayersliste()
    'End Sub

    'Private Shared Sub komprimierteLayerMarkieren(komprimieren As Boolean)
    '    For Each nlay As clsLayerPres In layersSelected
    '        If nlay.mithaken Then
    '            nlay.LayerSichtbarkeit = Visibility.Visible
    '            nlay.kastenHoehe = 19
    '        Else
    '            If komprimieren Then
    '                nlay.LayerSichtbarkeit = Visibility.Collapsed
    '                nlay.kastenHoehe = 0.01
    '            Else
    '                nlay.LayerSichtbarkeit = Visibility.Visible
    '                nlay.kastenHoehe = 19
    '            End If
    '        End If
    '    Next
    'End Sub

    Private Shared Sub removeJustAdded()
        For Each nlay As clsLayerPres In layersSelected
            If nlay.justAdded Then nlay.justAdded = False
        Next
    End Sub

    Private Sub btnNichtKompress_Click(sender As Object, e As RoutedEventArgs)
        removeJustAdded()
        Dim komprimieren As Boolean = False
        leereSelectedlayersNachPres(layersSelected)
        layersSelected.Sort()
        showLayersliste()
    End Sub

    Private Sub cbEbenenKategorien_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        GC.Collect()
        Dim item As ComboBoxItem = CType(cbEbenenKategorien.SelectedItem, ComboBoxItem)
        letztekategorieAuswahl = item.Tag.ToString
        If letztekategorieAuswahl.IsNothingOrEmpty Then Exit Sub
        If letztekategorieAuswahl = "_" Then Exit Sub
        refreshKategorienListe()
        GC.Collect()
    End Sub

    Private Sub refreshKategorienListe()
        Try
            katlayersList = clsLayerHelper.layers4Kategorie(letztekategorieAuswahl)
            katlayersList = clsLayerHelper.markiereSchonGeladeneLayerKATEGORIE(katlayersList, layersSelected)
            lvEbenenKategorie.ItemsSource = katlayersList
        Catch ex As Exception
            l("fehler in refreshKategorienListe ", ex)
        End Try
    End Sub

    Private Sub tiExplorerKategorie_MouseDown(sender As Object, e As MouseButtonEventArgs)
        cbEbenenKategorien.IsDropDownOpen = True
        cbEbenenKategorien.MaxDropDownHeight = lvEbenenKategorie.Height - 30
        e.Handled = True
    End Sub

    Private Sub chkauswahlgeaendertKategorie(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        e.Handled = True
        panningAusschalten()
        resizeWindow()
        Dim nck As CheckBox = CType(sender, CheckBox)
        Dim action As String = If(nck.IsChecked, "add", "sub")
        Dim pickAid As Integer = CInt(CStr(nck.Tag))

        addOrSubLayer(action, pickAid)
    End Sub

    Private Sub addOrSubLayer(action As String, pickAid As Integer)
        If action = "sub" Then
            entferneEbeneauslayersSelected(pickAid)
        End If
        ebenenListeAktualisieren()
        Debug.Print("action " & action & " aid: " & CStr(pickAid))
        If action = "sub" Then
            entferneEbeneAusSlots(pickAid)
        End If
        If action = "add" Then
            zwischenbildBitteWarten()
            Dim nlay As New clsLayerPres
            nlay.aid = pickAid
            If warSchonGeladen(nlay.aid, layersSelected) Then
                Debug.Print(layerActive.aid.ToString)
                For i = 0 To layersSelected.Count - 1
                    If nlay.aid = layersSelected(i).aid Then
                        layersSelected(i).mithaken = True
                        nlay = layersSelected(i).kopie
                        Exit For
                    End If
                Next
                nlay.mithaken = True
                nlay.RBischecked = False
                nlay.isactive = False
                nlay = clsWebgisPGtools.setSichtbarkeitRBaktiveEbene(nlay)
                Dim aktslot = SlotTools.getEmptySlot()
                slots(aktslot).mapfile = nlay.mapFile.Replace("layer.map", "header.map")
                slots(aktslot).refresh = True
                slots(aktslot).darstellen = True
                slots(aktslot).layer = nlay.kopie
                'job abschicken
                slots(aktslot).BildGenaufrufMAPserver(slots(aktslot).mapfile, myglobalz.serverWeb, kartengen.aktMap, slots(aktslot).layer.isUserlayer)
                MapModeAbschicken(slots(aktslot))
            Else
                pgisTools.getStamm4aid(nlay)
                nlay.mithaken = True
                nlay.RBischecked = False
                nlay.isactive = False

                nlay = clsWebgisPGtools.setSichtbarkeitRBaktiveEbene(nlay)
                'nlay.justAdded = True
                layersSelected.Add(nlay)
                Dim aktslot = SlotTools.getEmptySlot()

                slots(aktslot).mapfile = nlay.mapFile.Replace("layer.map", "header.map")
                slots(aktslot).refresh = True
                slots(aktslot).darstellen = True
                slots(aktslot).layer = nlay.kopie
                'job abschicken
                slots(aktslot).BildGenaufrufMAPserver(slots(aktslot).mapfile, myglobalz.serverWeb, kartengen.aktMap, slots(aktslot).layer.isUserlayer)
                MapModeAbschicken(slots(aktslot))
                'nach range neu sortieren
            End If
        End If
        refreshExplorerView("normal")
        refreshKategorienListe()
    End Sub

    Private Sub chkauswahlgeaendertExplorer(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        e.Handled = True
        panningAusschalten()
        resizeWindow()
        Dim nck As CheckBox = CType(sender, CheckBox)
        Dim action As String = If(nck.IsChecked, "add", "sub")
        Dim pickAid As Integer = CInt(CStr(nck.Tag))

        If action = "sub" Then
            entferneEbeneauslayersSelected(pickAid)
        End If
        ebenenListeAktualisieren()
        Debug.Print("action " & action & " aid: " & CStr(pickAid))
        If action = "sub" Then
            entferneEbeneAusSlots(pickAid)
        End If
        If action = "add" Then
            zwischenbildBitteWarten()
            Dim nlay As New clsLayerPres
            nlay.aid = pickAid
            If warSchonGeladen(nlay.aid, layersSelected) Then
                Debug.Print(layerActive.aid.ToString)
                For i = 0 To layersSelected.Count - 1
                    If nlay.aid = layersSelected(i).aid Then
                        layersSelected(i).mithaken = True
                        nlay = layersSelected(i).kopie
                        Exit For
                    End If
                Next
                nlay.mithaken = True
                nlay.RBischecked = False
                nlay.isactive = False
                nlay = clsWebgisPGtools.setSichtbarkeitRBaktiveEbene(nlay)
                Dim aktslot = SlotTools.getEmptySlot()
                slots(aktslot).mapfile = nlay.mapFile.Replace("layer.map", "header.map")
                slots(aktslot).refresh = True
                slots(aktslot).darstellen = True
                slots(aktslot).layer = nlay.kopie
                'job abschicken
                slots(aktslot).BildGenaufrufMAPserver(slots(aktslot).mapfile, myglobalz.serverWeb, kartengen.aktMap, slots(aktslot).layer.isUserlayer)
                MapModeAbschicken(slots(aktslot))
            Else
                pgisTools.getStamm4aid(nlay)
                nlay.mithaken = True
                nlay.RBischecked = False
                nlay.isactive = False

                nlay = clsWebgisPGtools.setSichtbarkeitRBaktiveEbene(nlay)
                'nlay.justAdded = True
                layersSelected.Add(nlay)
                Dim aktslot = SlotTools.getEmptySlot()

                slots(aktslot).mapfile = nlay.mapFile.Replace("layer.map", "header.map")
                slots(aktslot).refresh = True
                slots(aktslot).darstellen = True
                slots(aktslot).layer = nlay.kopie
                'job abschicken
                slots(aktslot).BildGenaufrufMAPserver(slots(aktslot).mapfile, myglobalz.serverWeb, kartengen.aktMap, slots(aktslot).layer.isUserlayer)
                MapModeAbschicken(slots(aktslot))
                'nach range neu sortieren
            End If
        End If
        refreshExplorerView("normal")
        refreshKategorienListe()

    End Sub
    Private Sub chkAktiveEbenegeaendertKategorie(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim nck As RadioButton = CType(sender, RadioButton)
        Dim pick As Integer
        Dim schonda As Boolean = False
        panningAusschalten()
        pick = CInt(nck.Tag)
        modLayer.alteAktiveEbeneDeaktivieren(layersSelected)
        markWMSlayers(layersSelected)
        Dim oldLay As clsLayerPres = getPresLayerFromList(pick, layersSelected)
        If oldLay Is Nothing Then
            Debug.Print("")
        Else
            schonda = True
            If oldLay.isactive Then
                oldLay.isactive = False
                oldLay.RBischecked = False
            Else
                oldLay.isactive = True
                oldLay.RBischecked = True
                layerActive = CType(oldLay.Clone, clsLayerPres)
                layerActive.aid = oldLay.aid
                layerActive.iswms = oldLay.iswms
                layerHgrund.isactive = False
            End If
            oldLay.mithaken = True

            If oldLay.iswms Then
                markwmslayerSingle(oldLay)
                panningAusschalten()
                WebBrowser1.Visibility = Visibility.Collapsed
                cvtop.Cursor = Cursors.Hand
                CanvasClickModus = "wmsdatenabfrage"
            Else
                WebBrowser1.LoadHtml("", myglobalz.myfakeurl) ' soll die alte imagemap löschen, sonst stimmt sie nicht mit layeractive überein
            End If
        End If

        Dim katLay As clsLayerPres = getPresLayerFromList(pick, katlayersList)
        If katLay Is Nothing Then
        Else
            katLay.isactive = True
            katLay.mithaken = True
            katLay.RBischecked = True
            layerActive = CType(katLay.Clone, clsLayerPres)
            layerActive.aid = katLay.aid
            layerActive.iswms = katLay.iswms
            layerHgrund.isactive = False
            If katLay.iswms Then
                markwmslayerSingle(katLay)
                panningAusschalten()
                WebBrowser1.Visibility = Visibility.Collapsed
                cvtop.Cursor = Cursors.Hand
                CanvasClickModus = "wmsdatenabfrage"
            Else
                WebBrowser1.LoadHtml("", myglobalz.myfakeurl) ' soll die alte imagemap löschen, sonst stimmt sie nicht mit layeractive überein
            End If
            If oldLay Is Nothing Then layersSelected.Add(katLay)
        End If
        refreshExplorerView("normal")
        refreshKategorienListe()

        refreshExplorerView("normal")
        refreshKategorienListe()
        ebenenListeAktualisieren()
        rbHgrundAktiveEbene.IsChecked = False
        refreshMap(True, False)
    End Sub
    Private Sub chkAktiveEbenegeaendertSuche(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim nck As RadioButton = CType(sender, RadioButton)
        Dim pick As Integer
        Dim schonda As Boolean = False
        panningAusschalten()
        pick = CInt(nck.Tag)
        modLayer.alteAktiveEbeneDeaktivieren(layersSelected)
        markWMSlayers(layersSelected)
        Dim oldLay As clsLayerPres = getPresLayerFromList(pick, layersSelected)
        If oldLay Is Nothing Then
            Debug.Print("")
        Else
            schonda = True
            If oldLay.isactive Then
                oldLay.isactive = False
                oldLay.RBischecked = False
            Else
                oldLay.isactive = True
                oldLay.RBischecked = True
                layerActive = CType(oldLay.Clone, clsLayerPres)
                layerActive.aid = oldLay.aid
                layerActive.iswms = oldLay.iswms
                layerHgrund.isactive = False
            End If
            oldLay.mithaken = True

            If oldLay.iswms Then
                markwmslayerSingle(oldLay)
                panningAusschalten()
                WebBrowser1.Visibility = Visibility.Collapsed
                cvtop.Cursor = Cursors.Hand
                CanvasClickModus = "wmsdatenabfrage"
            Else
                WebBrowser1.LoadHtml("", myglobalz.myfakeurl) ' soll die alte imagemap löschen, sonst stimmt sie nicht mit layeractive überein
            End If
        End If

        Dim katLay As clsLayerPres = getPresLayerFromList(pick, SuchLayersList)
        If katLay Is Nothing Then
        Else
            katLay.isactive = True
            katLay.mithaken = True
            katLay.RBischecked = True
            layerActive = CType(katLay.Clone, clsLayerPres)
            layerActive.aid = katLay.aid
            layerActive.iswms = katLay.iswms
            layerHgrund.isactive = False
            If katLay.iswms Then
                markwmslayerSingle(katLay)
                panningAusschalten()
                WebBrowser1.Visibility = Visibility.Collapsed
                cvtop.Cursor = Cursors.Hand
                CanvasClickModus = "wmsdatenabfrage"
            Else
                WebBrowser1.LoadHtml("", myglobalz.myfakeurl) ' soll die alte imagemap löschen, sonst stimmt sie nicht mit layeractive überein
            End If
            If oldLay Is Nothing Then layersSelected.Add(katLay)
        End If
        refreshExplorerView("normal")
        refreshKategorienListe()

        refreshExplorerView("normal")
        refreshKategorienListe()
        ebenenListeAktualisieren()
        rbHgrundAktiveEbene.IsChecked = False
        refreshMap(True, False)
    End Sub
    Private Sub chkAktiveEbenegeaendertExplorer(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim nck As RadioButton = CType(sender, RadioButton)
        Dim pick As Integer
        Dim schonda As Boolean = False
        panningAusschalten()
        pick = CInt(nck.Tag)
        modLayer.alteAktiveEbeneDeaktivieren(layersSelected)
        markWMSlayers(layersSelected)
        Dim oldLay As clsLayerPres = getPresLayerFromList(pick, layersSelected)
        If oldLay Is Nothing Then
            Debug.Print("")
        Else
            schonda = True
            If oldLay.isactive Then
                oldLay.isactive = False
                oldLay.RBischecked = False
            Else
                oldLay.isactive = True
                oldLay.RBischecked = True
                layerActive = CType(oldLay.Clone, clsLayerPres)
                layerActive.aid = oldLay.aid
                layerActive.iswms = oldLay.iswms
                layerHgrund.isactive = False
            End If
            oldLay.mithaken = True

            If oldLay.iswms Then
                markwmslayerSingle(oldLay)
                panningAusschalten()
                WebBrowser1.Visibility = Visibility.Collapsed
                cvtop.Cursor = Cursors.Hand
                CanvasClickModus = "wmsdatenabfrage"
            Else
                WebBrowser1.LoadHtml("", myglobalz.myfakeurl) ' soll die alte imagemap löschen, sonst stimmt sie nicht mit layeractive überein
            End If
        End If

        Dim katLay As clsLayerPres = getPresLayerFromList(pick, allLayersPres)
        If katLay Is Nothing Then
        Else
            katLay.isactive = True
            katLay.mithaken = True
            katLay.RBischecked = True
            layerActive = CType(katLay.Clone, clsLayerPres)
            layerActive.aid = katLay.aid
            layerActive.iswms = katLay.iswms
            layerHgrund.isactive = False
            If katLay.iswms Then
                markwmslayerSingle(katLay)
                panningAusschalten()
                WebBrowser1.Visibility = Visibility.Collapsed
                cvtop.Cursor = Cursors.Hand
                CanvasClickModus = "wmsdatenabfrage"
            Else
                WebBrowser1.LoadHtml("", myglobalz.myfakeurl) ' soll die alte imagemap löschen, sonst stimmt sie nicht mit layeractive überein
            End If
            If oldLay Is Nothing Then layersSelected.Add(katLay)
        End If
        refreshExplorerView("normal")
        refreshKategorienListe()

        refreshExplorerView("normal")
        refreshKategorienListe()
        ebenenListeAktualisieren()
        rbHgrundAktiveEbene.IsChecked = False
        refreshMap(True, False)
    End Sub
    Private Sub tiExplorerKategorie_MouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        'MsgBox("hurz") 
    End Sub

    Private Sub expKategorieOeffnen(expOeffnen As Boolean, aktListView As ListView, tag As String, titel As String)
        If expOeffnen Then
            aktListView.Visibility = Visibility.Visible
            Dim loklayersList = clsLayerHelper.layers4Kategorie(tag)
            loklayersList = clsLayerHelper.markiereSchonGeladeneLayerEXPLORER(loklayersList, layersSelected)
#If DEBUG Then
            clsLayerHelper.writeKatTooltipToFile(tag, loklayersList, "l:\apps\mgis\kat", titel)
#End If
            aktListView.ItemsSource = loklayersList
            aktListView.Height = (loklayersList.Count * 12 * 2.1) + 10
        Else
            aktListView.Visibility = Visibility.Collapsed
        End If
        GC.Collect()
    End Sub
    'Private Sub tbEXPallgemein_MouseDown(sender As Object, e As MouseButtonEventArgs)
    '    e.Handled = True
    '    Dim tb As TextBlock = CType(sender, TextBlock) : Dim tag As String = tb.Tag.ToString.ToLower.Trim
    '    Dim expOeffnen As Boolean = False
    '    setWeightAndMode(tb, expOeffnen)
    '    expKategorieOeffnen(expOeffnen, lvEXPallgemein, tag)
    'End Sub

    'Private Sub tbEXPgrenzen_MouseDown(sender As Object, e As MouseButtonEventArgs)
    '    e.Handled = True
    '    Dim tb As TextBlock = CType(sender, TextBlock) : Dim tag As String = tb.Tag.ToString.ToLower.Trim
    '    Dim expOeffnen As Boolean = False
    '    setWeightAndMode(tb, expOeffnen)
    '    expKategorieOeffnen(expOeffnen, lvEXPgrenzen, tag)
    'End Sub

    Private Shared Sub setWeightAndMode(ByRef tb As TextBlock, ByRef expOeffnen As Boolean)
        If tb.FontWeight = FontWeights.Bold Then
            tb.FontWeight = FontWeights.Black : expOeffnen = True
        Else
            tb.FontWeight = FontWeights.Bold : expOeffnen = False
        End If
    End Sub

    'Private Sub tbEXPboden_MouseDown(sender As Object, e As MouseButtonEventArgs)
    '    e.Handled = True
    '    Dim tb As TextBlock = CType(sender, TextBlock) : Dim tag As String = tb.Tag.ToString.ToLower.Trim
    '    Dim expOeffnen As Boolean = False
    '    setWeightAndMode(tb, expOeffnen)
    '    expKategorieOeffnen(expOeffnen, lvEXPboden, tag)
    '    GC.Collect()
    'End Sub

    'Private Sub tbEXPh_flurkarten_MouseDown(sender As Object, e As MouseButtonEventArgs)
    '    e.Handled = True
    '    Dim tb As TextBlock = CType(sender, TextBlock) : Dim tag As String = tb.Tag.ToString.ToLower.Trim
    '    Dim expOeffnen As Boolean = False
    '    setWeightAndMode(tb, expOeffnen)
    '    expKategorieOeffnen(expOeffnen, lvEXPflurkarte, tag)
    'End Sub
    'Private Sub tbEXPklima_MouseDown(sender As Object, e As MouseButtonEventArgs)
    '    e.Handled = True
    '    Dim tb As TextBlock = CType(sender, TextBlock) : Dim tag As String = tb.Tag.ToString.ToLower.Trim
    '    Dim expOeffnen As Boolean = False
    '    setWeightAndMode(tb, expOeffnen)
    '    expKategorieOeffnen(expOeffnen, lvEXPklima, tag)
    'End Sub
    'Private Sub tbEXPh_landschaftsschutz_MouseDown(sender As Object, e As MouseButtonEventArgs)
    '    e.Handled = True
    '    Dim tb As TextBlock = CType(sender, TextBlock) : Dim tag As String = tb.Tag.ToString.ToLower.Trim
    '    Dim expOeffnen As Boolean = False
    '    setWeightAndMode(tb, expOeffnen)
    '    expKategorieOeffnen(expOeffnen, lvEXPh_landschaftsschutz, tag)
    'End Sub
    'Private Sub btnExplorerAufklappen_Click(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True
    '    GC.Collect()
    '    openCloseExplorer(True)
    '    GC.Collect()
    'End Sub
    Private Sub openCloseExplorer(expOeffnen As Boolean)
        For Each item As clsUniversal In kategorienliste
            For Each lv As ListView In FindVisualChildren(Of ListView)(Me)
                If lv.Name = "lvE_" & item.tag Then
                    expKategorieOeffnen(expOeffnen, lv, item.tag, item.titel)
                    Continue For
                End If
            Next
        Next
    End Sub
    Public Iterator Function FindVisualChildren(Of T As DependencyObject)(depObj As DependencyObject) As IEnumerable(Of T)
        If depObj IsNot Nothing Then
            For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(depObj) - 1
                Dim child As DependencyObject = VisualTreeHelper.GetChild(depObj, i)
                If child IsNot Nothing AndAlso TypeOf child Is T Then
                    Yield DirectCast(child, T)
                End If
                For Each childOfChild As T In FindVisualChildren(Of T)(child)
                    Yield childOfChild
                Next
            Next
        End If
    End Function
    'Private Sub btnExplorerZuklappen_Click(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True
    '    GC.Collect()
    '    openCloseExplorer(False)
    '    GC.Collect()
    'End Sub

    Private Sub lvEXP_PreviewMouseWheel(sender As Object, e As MouseWheelEventArgs)
        If Not e.Handled Then
            e.Handled = True
            Dim eventArg = New MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            eventArg.RoutedEvent = UIElement.MouseWheelEvent
            eventArg.Source = sender
            Dim parent = TryCast((CType(sender, Control)).Parent, UIElement)
            parent.[RaiseEvent](eventArg)
        End If
    End Sub



    Private Sub btnKategorieEinzelRefresh_Click(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        GC.Collect()
        'Dim item As ComboBoxItem = CType(cbEbenenKategorien.SelectedItem, ComboBoxItem)
        'letztekategorieAuswahl = item.Tag.ToString
        If letztekategorieAuswahl.IsNothingOrEmpty Then Exit Sub
        If letztekategorieAuswahl = "_" Then Exit Sub
        refreshKategorienListe()
        GC.Collect()
    End Sub

    Private Sub btnExplorerKlappen_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        GC.Collect()
        If btnExplorerKlappen.IsChecked Then
            openCloseExplorer(True)
            tbKlappenActionText.Text = "Alles zu"
        Else
            openCloseExplorer(False)
            tbKlappenActionText.Text = "Alles auf"
        End If
        GC.Collect()
    End Sub

    Private Sub TbEbenensuche2_TextChanged(sender As Object, e As TextChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        tbStichwort.Text = tbEbenensuche2.Text
        If tbEbenensuche2.Text.EndsWith(vbCrLf) Then
            tbEbenensuche2.Text = tbEbenensuche2.Text.Replace(vbCrLf, "")
            tbStichwort.Text = tbEbenensuche2.Text
            stichwortsucheDurchfuehr()
            tbEbenensuche.Text = tbEbenensuche2.Text.Trim
        End If
        If tbEbenensuche2.Text.Length = 0 Then
            makeThemenInVis()
        End If
        If tbEbenensuche2.Text.Length < 1 Then
            btnStichwortsuchebeenden.IsEnabled = False
            btnEbenensuche.IsEnabled = False
        Else
            btnStichwortsuchebeenden.IsEnabled = True
            btnEbenensuche.IsEnabled = True
        End If
    End Sub

    Private Sub btnExplorerEinzelRefresh_Click(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        spExplorerParent.Children.Clear()
        refreshExplorerView("")
        'spExplorerParent.Children.Clear()
        generateExplorer(kategorienliste)
    End Sub

    Private Sub btnSelection_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim selaid As Integer = selectionTools.isSelectionLayerLoaded(GisUser.nick, layersSelected)
        If selaid < 1 Then
            selaid = selectionTools.isSelectionLayerErzeugt(GisUser.nick, allLayersPres)
            If selaid < 1 Then
                'ebene ist noch nicht vorhanden
                'jeztt anlegen
                MessageBox.Show("Die persönliche Ebene '" & "Auswahl: " & GisUser.nick.ToLower & "' existiert noch nicht und wird jetzt erzeugt. " & Environment.NewLine &
                                "Bitte das GIS neustarten.", "Auswahl: " & GisUser.nick.ToLower)
                selectionTools.createuserlayer(GisUser.nick)
                Close()
            Else
                'ebene ist vorhanden
                'jeztt adden
                MessageBox.Show("Die persönliche Ebene '" & "Auswahl: " & GisUser.nick.ToLower & "' wird der Bestandsliste hinzugefügt!", "Auswahl: " & GisUser.nick.ToLower)
                'selectionTools.adduserlayer(GisUser.nick, selaid)
                addOrSubLayer("add", selaid)
                Dim sel As New winSelection(0, Me)
                sel.Owner = Me
                sel.Show()
            End If
        Else
            'ebene exist und ist schon in der liste
            Dim sel As New winSelection(0, Me)
            sel.Show()
        End If

    End Sub

    Private Sub btnHostChange_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        iminternet = True
        Reboot4GISHost()
    End Sub
End Class




