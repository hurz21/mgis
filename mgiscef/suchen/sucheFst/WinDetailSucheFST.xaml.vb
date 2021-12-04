Imports System.ComponentModel
Imports System.Data
Imports mgis
' userIniProfile.WertSchreiben("Minimap", "Ausschnitt_info", "1")
'If String.IsNullOrEmpty( userIniProfile.WertLesen("Outlook", "anzeigen")) Then
' userIniProfile.WertSchreiben("Outlook", "anzeigen", "False")
'Else
'mycSimple.outlookAnzeigen = CBool( userIniProfile.WertLesen("Outlook", "anzeigen"))
'End If
Public Class WinDetailSucheFST
    'Private Property anyChange As Boolean = False
    Property ladevorgangabgeschlossen As Boolean = False
    Private modus$
    Public Shared AktuelleBasisTabelle As String = "flurkarte.basis_f"
    Private EigentuemerPDF As String
    Public Property returnValue As Boolean = False
    Public Property historyLast As Boolean = False
    Public Property fstkombis As New List(Of clsFlurauswahl)




    Sub New(ByVal _modus As String)
        InitializeComponent()
        modus = _modus
    End Sub
    Private Sub Window_Flurstuecksauswahl_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        Try
            l(" MOD ---------------------- anfang")
            initGemarkungsCombo()
            lvGemarkungen.ItemsSource = clsFSTtools.initGemarkungsListview()
            cmbgemarkung.Visibility = Visibility.Collapsed
            gbFSTaradigma.IsEnabled = False
            btnDossier.Visibility = Visibility.Collapsed
            spEigentNotizUebernehmen.Visibility = Visibility.Collapsed
            tbFlurnrWaelen.Visibility = Visibility.Collapsed
            spFST.Visibility = Visibility.Collapsed
            'stckBuchstaben1.Visibility = Visibility.Collapsed

            'histControlls("aus")
            'cbFSTHist.Visibility = Visibility.Collapsed : cbFSTHist.Visibility = Visibility.Hidden
            'If modus = "ort" Then
            '      btnEigentuemerALKIS.Visibility = Windows.Visibility.Hidden
            'End If
            'If modus = "eigentümer" Then
            '    btnEigentuemerALKIS.Visibility = Windows.Visibility.Visible
            'End If
            'anyChange = False
            'Title = StammToolsNs.setWindowTitel.exe(modus, " ")

            tbWeitergabeVerbot.Text = albverbotsString
            spEigentNotizUebernehmen.Visibility = Visibility.Collapsed
            setzeGrundFuerEigentuemerabfrage(tbGrund.Text)
            wennAlbBerechtigtDannGroupboxEinschalten()

            If STARTUP_mgismodus = "paradigma" Then
                gbFSTaradigma.Visibility = Visibility.Visible
                gbFSTaradigma.IsEnabled = True
                spEigentNotizUebernehmen.Visibility = Visibility.Visible
                btnFlurstueckNachParadigma.Visibility = Visibility.Visible
            Else
                spEigentNotizUebernehmen.IsEnabled = False
                gbFSTaradigma.Visibility = Visibility.Collapsed
                spEigentNotizUebernehmen.Visibility = Visibility.Collapsed
            End If
            If Not aktFST.normflst.FS.IsNothingOrEmpty Then
                'tbGemarkung.Text = aktFST.normflst.gemarkungstext
                'tbFlur.Text = CType(aktFST.normflst.flur, String)
                displayresult(aktFST.normflst)
                lvFlure.ItemsSource = clsFSTtools.initFlureListe()
                initFSTliste("")
                lvGemarkungen.Visibility = Visibility.Visible
                lvFlure.Visibility = Visibility.Collapsed
                lvFstkombi.Visibility = Visibility.Collapsed
                If Not iminternet Then btnDossier.Visibility = Visibility.Visible
                cmbgemarkung.Visibility = Visibility.Collapsed
                clsFSTtools.holeKoordinaten4Flurstueck(aktFST.normflst.nenner.ToString, WinDetailSucheFST.AktuelleBasisTabelle, aktFST)
                returnValue = False
            Else
                'cmbgemarkung.IsDropDownOpen = True
            End If
            Top = 50
            ladevorgangabgeschlossen = True
            l(" MOD Window_Flurstuecksauswahl_Loaded ende")
        Catch ex As Exception
            l("Fehler in Window_Flurstuecksauswahl_Loaded: " & ex.ToString())
        End Try
    End Sub



    Private Sub wennAlbBerechtigtDannGroupboxEinschalten()
        'If Not ladevorgangabgeschlossen Then Exit Sub
        If GisUser.istalbberechtigt Then
            gbEigentuemer.Visibility = Visibility.Visible
            gbEigentuemer.IsEnabled = True
        Else
            gbEigentuemer.Visibility = Visibility.Collapsed
            gbEigentuemer.IsEnabled = False
        End If
    End Sub

    Sub initGemarkungsCombo()
        Dim existing As XmlDataProvider = TryCast(Me.Resources("XMLSourceComboBoxgemarkungen"), XmlDataProvider)
        existing.Source = New Uri(Paradigma_GemarkungsXML)
        existing = TryCast(Me.Resources("XMLSourceComboBoxRBfunktion"), XmlDataProvider)
        existing.Source = New Uri(Paradigma_funktionen_verz)
    End Sub


    Private Sub cmbgemarkung_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbgemarkung.SelectionChanged
        e.Handled = True
        If cmbgemarkung.SelectedItem Is Nothing Then Exit Sub
        Dim theID$ = CStr(cmbgemarkung.SelectedValue)
        Dim myvalx = CType(cmbgemarkung.SelectedItem, System.Xml.XmlElement)
        Dim displaytext$ = myvalx.Attributes(1).Value.ToString

        gemarkungGewaehlt(theID, displaytext)
        '  cmbFlur.IsDropDownOpen = True

    End Sub

    Private Sub gemarkungGewaehlt(theID As String, displaytext As String)
        Try
            l(" MOD gemarkungGewaehlt anfang")
            aktFST.normflst.gemcode = CInt(theID)
            aktFST.normflst.gemarkungstext = displaytext.Trim
            displayresult(aktFST.normflst)
            lvFlure.ItemsSource = clsFSTtools.initFlureListe()
            lvGemarkungen.Visibility = Visibility.Collapsed
            cmbgemarkung.Visibility = Visibility.Visible
            l(" MOD gemarkungGewaehlt ende")
        Catch ex As Exception
            l("Fehler in gemarkungGewaehlt: " & ex.ToString())
        End Try
    End Sub

    Private Sub displayresult(normflst As clsFlurstueck)
        tbresult.Text = normflst.gemarkungstext & ", Flur: " & normflst.flur & ", Flurstück:  " & normflst.fstueckKombi
    End Sub



    'Private Sub cmbFlur_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbFlur.SelectionChanged
    '    e.Handled = True
    '    Dim item2 As DataRowView = CType(cmbFlur.SelectedItem, DataRowView)
    '    If item2 Is Nothing Then Exit Sub
    '    Dim flur$ = item2.Row.ItemArray(0).ToString
    '    tbFlur.Text = flur 
    '    flurselectectExtract(flur)
    'End Sub

    Private Sub flurselectectExtract(flur As String)
        'cbFSTHist.Visibility = Visibility.Visible
        'cmbZaehler.IsEnabled = True
        aktFST.normflst.flur = CInt(flur)
        displayresult(aktFST.normflst)
        initFSTliste(flur)
        '    cmbZaehler.IsDropDownOpen = True
    End Sub

    Sub initFSTliste(ziffern As String)
        Dim dtaktuell As DataTable
        Dim tab As String = ""
        If cbFSTHist.IsChecked Then
            tab = myglobalz.histFstView
            'dtaktuell = holeZNDT(myglobalz.histFstView, ziffern)
            aktFST.normflst.istHistorisch = True
            historyLast = True
        Else
            tab = WinDetailSucheFST.AktuelleBasisTabelle
            ziffern = ""
            aktFST.normflst.istHistorisch = False
            historyLast = False
        End If
        fstkombis.Clear()
        Dim hinweis As String = ""
        If iminternet Or CGIstattDBzugriff Then
            fstkombis = clsFSTtools.getFSTlisteFromHTTP(aktFST.normflst.gemcode, aktFST.normflst.flur,
                                                        WinDetailSucheFST.AktuelleBasisTabelle,
                                                        hinweis)
        Else
            dtaktuell = holeZNDT(tab, ziffern)
            fstkombis = clsFSTtools.dt2objFst(dtaktuell)
        End If
        lvFstkombi.ItemsSource = fstkombis
    End Sub

    Private Function getKleineFstListe(fstkombis As List(Of clsFlurauswahl), ziffern As String) As List(Of clsFlurauswahl)
        Dim kleine As New List(Of clsFlurauswahl)
        Try
            l(" getKleineFstListe ---------------------- anfang")
            For Each item As clsFlurauswahl In fstkombis
                If item.displayText.StartsWith(ziffern) Then

                    kleine.Add(item)
                End If
            Next
            l(" getKleineFstListe ---------------------- ende")
            Return kleine
        Catch ex As Exception
            l("Fehler in getKleineFstListe: " & ex.ToString())
            Return Nothing
        End Try
    End Function



    'Private Sub cmbZaehler_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbZaehler.SelectionChanged
    '    e.Handled = True
    '    Dim item2 As DataRowView = CType(cmbZaehler.SelectedItem, DataRowView)
    '    If item2 Is Nothing Then Exit Sub
    '    Dim item3$ = item2.Row.ItemArray(0).ToString
    '    cmbNenner.IsEnabled = True
    '    tbZaehler.Text = item2.Row.ItemArray(0).ToString
    '    aktFST.normflst.zaehler = CInt(item3$)
    '    aktFST.normflst.nenner = Nothing
    '    aktFST.normflst.istHistorisch = False
    '    Dim dt As DataTable = initNennerCombo()
    '    tbWeitergabeVerbot.Text = albverbotsString
    '    If dt.Rows.Count = 1 Then
    '        tbNenner.Text = dt.Rows(0).Item(0).ToString
    '        aktFST.normflst.nenner = CInt(tbNenner.Text)
    '        aktFST.normflst.FS = aktFST.normflst.buildFS
    '        FSGKrechtsGKHochwertHolen(aktFST.normflst, WinDetailSucheFST.AktuelleBasisTabelle)
    '        aktFST.normflst.GKhoch = CInt(aktFST.normflst.GKhoch)
    '        aktFST.normflst.GKrechts = CInt(aktFST.normflst.GKrechts)
    '        aktFST.punkt.X = aktFST.normflst.GKrechts
    '        aktFST.punkt.Y = aktFST.normflst.GKhoch
    '        aktGlobPoint.strX = CType(CInt(aktFST.normflst.GKrechts), String)
    '        aktGlobPoint.strY = CType(CInt(aktFST.normflst.GKhoch), String)
    '        kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, CInt(aktFST.normflst.radius) * 2)
    '        returnValue = True
    '        spNachnenner.Visibility = Visibility.Visible
    '        If Not ckFormNichtSchliessen.IsChecked Then
    '            Close()
    '        End If
    '        wennAlbBerechtigtDannGroupboxEinschalten()
    '    Else
    '        'cmbNenner.IsDropDownOpen = True
    '    End If
    '    e.Handled = True
    'End Sub

    'Function initNennerCombo() As DataTable
    '    Dim dt As DataTable = holeNennerDT(WinDetailSucheFST.AktuelleBasisTabelle)
    '    cmbNenner.DataContext = dt
    '    Dim dthist As DataTable = holeNennerDT(myglobalz.histFstView)
    '    cmbNennerHist.DataContext = dthist
    '    Return dt
    'End Function



    'Private Sub cmbNenner_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbNenner.SelectionChanged
    '    e.Handled = True
    '    Dim item2 As DataRowView = CType(cmbNenner.SelectedItem, DataRowView)
    '    If item2 Is Nothing Then Exit Sub
    '    Try
    '    Catch ex As Exception
    '        Exit Sub
    '    End Try
    '    tbNenner.Text = item2.Row.ItemArray(0).ToString
    '    aktFST.normflst.istHistorisch = False
    '    clsFSTtools.holeKoordinaten4Flurstueck(tbNenner.Text, WinDetailSucheFST.AktuelleBasisTabelle, aktFST)
    '    returnValue = True
    '    If ckFormNichtSchliessen.IsChecked Then
    '        spNachnenner.Visibility = Visibility.Visible
    '        If aktvorgangsid.Trim.Length > 2 Then
    '            tbGrund.Text = aktvorgangsid
    '            If STARTUP_mgismodus.ToLower = "paradigma" Then
    '                gbFSTaradigma.IsEnabled = True
    '            End If
    '            wennAlbBerechtigtDannGroupboxEinschalten()
    '        End If
    '    Else
    '        Close()
    '    End If
    'End Sub




    Private Sub chkInsArchiv_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub chkEreignisMap_Click_1(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub
    Private Sub btnEigentuemer_Click(sender As Object, e As RoutedEventArgs)
        If Not GisUser.istalbberechtigt Then
            MsgBox("Der User: " & GisUser.nick & " ist nicht berechtigt auf die Eigentümerdaten zuzugreifen. ")
            Exit Sub
        End If
        If tbGrund.Text Is Nothing OrElse tbGrund.Text.Trim.Length < 2 Then
            MsgBox("Bitte eine Begründung (z.B. das Aktenzeichen) eingeben!")
            Exit Sub
        End If
        If tbresult.Text.IsNothingOrEmpty Then
            MessageBox.Show("Sie müssen zuerst ein Flurstück auswählen!")
            Exit Sub
        End If
        GrundFuerEigentuemerabfrage = tbGrund.Text

        'If cbSchnellEigentuemer.IsChecked Then
        If aktFST.normflst.istHistorisch Then
            MessageBox.Show("Sie haben ein historisches Flurstück gewählt. " & Environment.NewLine &
                            "Dazu liegen keine Eigentümerdaten vor. " & Environment.NewLine &
                            "Wählen sie ein aktuelles Flurstück für Eigentümerdaten!", "Historisches Flurstück")
        Else
            Dim strLage As String = ""
            strLage = clsSachdatentools.getlage(aktFST.normflst.FS)

            tbWeitergabeVerbot.Text = SchnellausgabeMitProtokoll(tbGrund.Text, aktFST)
            EigentuemerPDF = clsSachdatentools.erzeugeUndOeffneEigentuemerPDF(tbWeitergabeVerbot.Text, strLage)
            OpenDokument(EigentuemerPDF)
        End If
        'Else
        '    If aktFST.normflst.istHistorisch Then
        '        MessageBox.Show("Sie haben ein historisches Flurstück gewählt. " & Environment.NewLine &
        '                        "Dazu liegen keine Eigentümerdaten vor. " & Environment.NewLine &
        '                        "Wählen sie das aktuelle Flurstück für Eigentümerdaten!", "Historisches Flurstück")
        '    Else
        '        Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents
        '        'tbKurz.Text = "Bitte warten  ...."
        '        Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
        '        aktFST.normflst.splitFS(aktFST.normflst.FS)
        '        Dim specparms(8) As String
        '        specparms(3) = aktFST.normflst.FS
        '        specparms(4) = aktFST.normflst.FS
        '        Dim weistauf As String = ""
        '        Dim zeigtauf As String = ""
        '        Dim gebucht As String = ""
        '        Dim areaqm As String = ""
        '        If Module2.holeRestlicheParams4FST(aktFST.normflst.FS, weistauf, zeigtauf, gebucht, areaqm) Then
        '            specparms(5) = weistauf
        '            specparms(6) = zeigtauf
        '            specparms(7) = gebucht
        '            specparms(8) = areaqm
        '            EigentuemerPDF = getEigentuemerDatei(specparms)
        '            tools.openDocument(EigentuemerPDF)
        '        End If
        '    End If 
        'End If
        If STARTUP_mgismodus = "paradigma" Then
            spEigentNotizUebernehmen.Visibility = Visibility.Visible
            spEigentNotizUebernehmen.IsEnabled = True
            btnEigentuemerNachParadigma.IsEnabled = True
        End If
        e.Handled = True
    End Sub
    Function SchnellausgabeMitProtokoll(grund As String, afst As ParaFlurstueck) As String
        'If schonSchnellAusgegeben Then Exit Sub
        If grund Is Nothing OrElse grund.Trim.Length < 2 OrElse grund = "Aktenzeichen" Then
            MsgBox("Bitte eine Begründung (z.B. das Aktenzeichen) eingeben!")
            FocusManager.SetFocusedElement(Me, tbGrund)
            Return ""
        End If
        Dim info As String

        info = "Eigentümer in Kurzform: " & Environment.NewLine &
                                    getSchnellbatchEigentuemer(afst.normflst.FS)

        'schonSchnellAusgegeben = True
        Protokollausgabe_aller_Parameter(afst.normflst.FS, grund)
        Return info
    End Function
    Public Sub Protokollausgabe_aller_Parameter(flurstueck As String, grund As String)
        Try
            Dim sw As New IO.StreamWriter(eigentuemer_protokoll, True)
            sw.WriteLine(Now & "#" & GisUser.nick & "#" & clsActiveDir.fdkurz & "#" & "DESKTOP" & "#" & grund & "#" & flurstueck & "#" & "#" & "#" & "#" & "#")
            sw.Close()
            sw.Dispose()
        Catch ex As Exception
            'sw.WriteLine("Fehler in kontzrollausgabe:" ,ex)
        End Try
    End Sub



    Private Sub ckFormNichtSchliessen_Checked(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnFlurstueckNachParadigma_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        clsFSTtools.fstnachParadigmaSpeichern(tbFreitext.Text.Trim, tbKurz.Text.Trim)
        Close()
    End Sub


    Private Sub cmbFunktionsvorschlaege_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If cmbFunktionsvorschlaege.SelectedItem Is Nothing Then Exit Sub
        Dim myvalx = CType(cmbFunktionsvorschlaege.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        tbKurz.Text = myvals
    End Sub

    'Private Sub cmbFSTalt_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    '    If cmbFSTalt.SelectedItem Is Nothing Then Exit Sub
    '    Dim item As ComboBoxItem
    '    item = CType(cmbFSTalt.SelectedItem, ComboBoxItem)
    '    Select Case item.Tag.ToString.ToLower.Trim
    '        Case "aktuell"
    '            AktuelleBasisTabelle = "flurkarte.basis_f"
    '        Case "1998", "1999", "2000", "2001", "2002"
    '            AktuelleBasisTabelle = "h_flurkarte.j" & item.Tag.ToString.ToLower & "_flurstueck_f"
    '        Case Else
    '            AktuelleBasisTabelle = "h_flurkarte.j" & item.Tag.ToString.ToLower & "_basis_f"
    '    End Select
    '    fstaltinfo.Text = "Gesucht wird in Flurkarte: " & item.Tag.ToString.ToLower.Trim & Environment.NewLine
    '    fstaltinfo.Text = fstaltinfo.Text & Environment.NewLine &
    '        "Falls Sie die historische Flurkarte laden wollen: " & Environment.NewLine &
    '        "      " & Environment.NewLine &
    '        "- Aktivieren Sie den Explorer links oben und  " & Environment.NewLine &
    '        "- wählen Sie dann die historischen Flurkarten: " & Environment.NewLine &
    '        "- klicken Sie auf die gewünschte Flurkarte. " & Environment.NewLine &
    '        "- schliessen Sie den Explorer " & Environment.NewLine


    '    'If item.Tag.ToString.ToLower.Trim = "fstakt" Then
    '    '    AktuelleBasisTabelle = "flurkarte.basis_f"
    '    'End If

    '    'stContext.Visibility = Visibility.Collapsed
    '    'panningAusschalten()
    '    'aktaid = CInt(nck.Tag)
    '    e.Handled = True
    'End Sub

    'Private Sub cbFSTalt_Click(sender As Object, e As RoutedEventArgs)
    '    If cbFSTalt.IsChecked Then
    '        spAuswahlAlt.Visibility = Visibility.Visible
    '    Else
    '        spAuswahlAlt.Visibility = Visibility.Hidden
    '    End If
    '    e.Handled = True
    'End Sub

    Private Sub btnSchnellNachPDF_Click(sender As Object, e As RoutedEventArgs)
        EigentuemerPDF = clsSachdatentools.erzeugeUndOeffneEigentuemerPDF(tbWeitergabeVerbot.Text, "")
        e.Handled = True
    End Sub


    Private Sub btnEigentuemerNachParadigma_Click(sender As Object, e As RoutedEventArgs)
        If modParadigma.DokNachParadigma(EigentuemerPDF, aktvorgangsid, "Eigentümer: ") Then
            MsgBox("Die Übernahme des Dokumentes nach Paradigma war erfolgreich!")
        Else
            MsgBox("Die Übernahme des Dokumentes nach Paradigma war NICHT erfolgreich!")
        End If
        Close()
        e.Handled = True
    End Sub

    Private Sub btnDossier_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        clsFSTtools.dossierPrepMinimum()
    End Sub




    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        returnValue = False
        Close()
    End Sub



    'Private Sub cmbNennerHist_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    '    e.Handled = True
    '    Dim item2 As DataRowView = CType(cmbNennerHist.SelectedItem, DataRowView)
    '    If item2 Is Nothing Then Exit Sub
    '    Try
    '    Catch ex As Exception
    '        Exit Sub
    '    End Try
    '    tbNenner.Text = item2.Row.ItemArray(0).ToString
    '    aktFST.normflst.istHistorisch = True
    '    clsFSTtools.holeKoordinaten4Flurstueck(tbNenner.Text, myglobalz.histFstView, aktFST)
    '    returnValue = True
    '    If ckFormNichtSchliessen.IsChecked Then
    '        spNachnenner.Visibility = Visibility.Visible
    '        If aktvorgangsid.Trim.Length > 2 Then
    '            tbGrund.Text = aktvorgangsid
    '            If STARTUP_mgismodus.ToLower = "paradigma" Then
    '                gbFSTaradigma.IsEnabled = True
    '            End If
    '        End If
    '    Else
    '        historyLast = True
    '        Close()
    '    End If
    '    Close() 'auf jeden fall schliessen damit keine eigentümerabfrage möglich ist
    'End Sub

    'Private Sub cbFSTHist_Click(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True
    '    If cbFSTHist.IsChecked Then
    '        histControlls("ein")
    '    Else
    '        histControlls("aus")
    '    End If
    'End Sub

    'Private Sub histControlls(einaus As String)
    '    If einaus = "ein" Then
    '        hist1.Visibility = Visibility.Visible
    '        hist2.Visibility = Visibility.Visible
    '        cmbZaehlerHist.Visibility = Visibility.Visible
    '        cmbNennerHist.Visibility = Visibility.Visible
    '    Else
    '        hist1.Visibility = Visibility.Collapsed
    '        hist2.Visibility = Visibility.Collapsed
    '        cmbZaehlerHist.Visibility = Visibility.Collapsed
    '        cmbNennerHist.Visibility = Visibility.Collapsed
    '    End If 
    'End Sub 
    Private Sub txFlur_MouseDown(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim nck As Button = CType(sender, Button)
        Dim flur As String
        flur = CType(nck.Tag, String)
        'tbFlur.Text = flur
        lvFstkombi.Visibility = Visibility.Visible
        spFST.Visibility = Visibility.Visible
        'stckBuchstaben1.Visibility = Visibility.Visible
        flurselectectExtract(flur)

        tbfilter.Text = ""
        FocusManager.SetFocusedElement(Me, tbfilter)
    End Sub

    Private Sub lvFstkombi_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub lvFlure_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub txfst_MouseDown(sender As Object, e As RoutedEventArgs)
        Dim nck As Button = CType(sender, Button)
        Dim fstueckkombi As String
        fstueckkombi = CType(nck.Content, String)
        aktFST.normflst.zaehler = CInt(nck.Uid)
        aktFST.normflst.nenner = CInt(nck.Tag)
        'tbFstkombi.Text = fstueckkombi

        displayresult(aktFST.normflst)
        'tbNenner.Text = aktFST.normflst.nenner.ToString
        ' aktFST.normflst.istHistorisch = Falsei
        If cbFSTHist.IsChecked Then

            clsFSTtools.holeKoordinaten4Flurstueck(aktFST.normflst.nenner.ToString, myglobalz.histFstView, aktFST)

        Else
            clsFSTtools.holeKoordinaten4Flurstueck(aktFST.normflst.nenner.ToString, WinDetailSucheFST.AktuelleBasisTabelle, aktFST)
        End If
        returnValue = True
        e.Handled = True
        If ckFormNichtSchliessen.IsChecked Then
            If Not iminternet Then btnDossier.Visibility = Visibility.Visible
            If aktvorgangsid.Trim.Length > 2 Then
                tbGrund.Text = aktvorgangsid
                ' SchnellausgabeMitProtokoll(tbGrund.Text)

                If STARTUP_mgismodus.ToLower = "paradigma" Then
                    gbFSTaradigma.IsEnabled = True
                End If
                wennAlbBerechtigtDannGroupboxEinschalten()
            End If
        Else
            Close()
        End If
    End Sub





    Private Sub cbFSTHist_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        lvFstkombi.ItemsSource = Nothing
    End Sub



    'Private Sub txGemarkungs_MouseDown(sender As Object, e As RoutedEventArgs)

    'End Sub
    Private Sub txGemarkungs_MouseDown(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim nck As Button = CType(sender, Button)
        'Dim fstueckkombi As String
        'fstueckkombi = CType(nck.Text, String)
        'aktFST.normflst.zaehler = CInt(nck.Uid)
        'aktFST.normflst.nenner = CInt(nck.Tag) 
        tbFlurnrWaelen.Visibility = Visibility.Visible
        lvFlure.Visibility = Visibility.Visible
        gemarkungGewaehlt(nck.Uid, CType(nck.Content, String))
    End Sub

    'Private Sub btnstreet(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True
    '    Dim a As String = (sender.ToString)
    '    a = a.Replace("System.Windows.Controls.Button:", "")
    '    a = a.Trim.ToLower
    '    zifferZuFST(a)
    'End Sub

    'Private Sub zifferZuFST(a As String)
    '    'Dim strassenListe As New List(Of clsFlurauswahl)
    '    'lvStrassen.Visibility = Visibility.Visible
    '    'initStrassenCombo(a)
    '    'strassenListe = initStrassenliste(adrREC.dt)
    '    'lvStrassen.ItemsSource = strassenListe

    '    displayresult(aktFST.normflst)
    '    initFSTliste(a)
    'End Sub
    'Private Sub myTestKey(sender As Object, e As KeyEventArgs)
    '    e.Handled = True
    '    'If stckBuchstaben1.Visibility = Visibility.Visible Then
    '    '    'MsgBox(e.Key & " " & Chr(KeyInterop.VirtualKeyFromKey(e.Key)))
    '    '    Dim a As String = Chr(KeyInterop.VirtualKeyFromKey(e.Key))
    '    '    zifferZuFST(a.ToLower)
    '    'End If
    'End Sub

    Private Sub tbfilter_TextChanged(sender As Object, e As TextChangedEventArgs)
        e.Handled = True
        Dim cand As String = tbfilter.Text.ToLower
        Dim fstklein As New List(Of clsFlurauswahl)
        For Each fst As clsFlurauswahl In fstkombis
            If Not fst.displayText.StartsWith(cand) Then Continue For
            If fst.nenner <> String.Empty Then
                fst.displayText = fst.id & "/" & fst.nenner
            Else
                fst.displayText = CType(fst.id, String)
            End If
            fstklein.Add(fst)
        Next
        lvFstkombi.ItemsSource = fstklein
    End Sub


End Class
