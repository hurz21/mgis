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

    Sub New(ByVal _modus As String)
        InitializeComponent()
        modus = _modus
    End Sub
    Private Sub Window_Flurstuecksauswahl_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        initGemarkungsCombo()
        gbFSTaradigma.IsEnabled = False
        spNachnenner.Visibility = Visibility.Collapsed
        spEigentNotizUebernehmen.Visibility = Visibility.Collapsed

        histControlls("aus") : cbFSTHist.Visibility = Visibility.Collapsed : cbFSTHist.Visibility = Visibility.Hidden
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
            tbGemarkung.Text = aktFST.normflst.gemarkungstext
            tbFlur.Text = CType(aktFST.normflst.flur, String)
            tbZaehler.Text = CType(aktFST.normflst.zaehler, String)
            tbNenner.Text = CType(aktFST.normflst.nenner, String)
            initFlureCombo()
            initZaehlerCombo()
            clsFSTtools.holeKoordinaten4Flurstueck(tbNenner.Text, WinDetailSucheFST.AktuelleBasisTabelle, aktFST)
            returnValue = True
        Else
            'cmbgemarkung.IsDropDownOpen = True
        End If
        Top = 50
        ladevorgangabgeschlossen = True
        e.Handled = True
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
        Dim myvali$ = CStr(cmbgemarkung.SelectedValue)
        Dim myvalx = CType(cmbgemarkung.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        tbGemarkung.Text = myvals
        aktFST.normflst.istHistorisch = False
        aktFST.normflst.gemcode = CInt(myvali)
        aktFST.normflst.gemarkungstext = tbGemarkung.Text
        initFlureCombo()
        cmbFlur.IsDropDownOpen = True

    End Sub
    Sub initFlureCombo()
        Dim dt As DataTable = holeFlureDT()
        cmbFlur.DataContext = dt
    End Sub

    Private Sub cmbFlur_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbFlur.SelectionChanged
        Dim item2 As DataRowView = CType(cmbFlur.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub

        cbFSTHist.Visibility = Visibility.Visible
        cmbZaehler.IsEnabled = True
        Dim item3$ = item2.Row.ItemArray(0).ToString
        tbFlur.Text = item2.Row.ItemArray(0).ToString
        aktFST.normflst.flur = CInt(item3$)
        aktFST.normflst.istHistorisch = False
        initZaehlerCombo()
        cmbZaehler.IsDropDownOpen = True
        e.Handled = True
    End Sub
    Sub initZaehlerCombo()
        Dim dtaktuell As DataTable = holeZaehlerDT(WinDetailSucheFST.AktuelleBasisTabelle)
        cmbZaehler.DataContext = dtaktuell
        Dim dthist As DataTable = holeZaehlerDT(myglobalz.histFstView)
        cmbZaehlerHist.DataContext = dthist
    End Sub

    Private Sub cmbZaehler_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbZaehler.SelectionChanged
        e.Handled = True
        Dim item2 As DataRowView = CType(cmbZaehler.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Dim item3$ = item2.Row.ItemArray(0).ToString
        cmbNenner.IsEnabled = True
        tbZaehler.Text = item2.Row.ItemArray(0).ToString
        aktFST.normflst.zaehler = CInt(item3$)
        aktFST.normflst.nenner = Nothing
        aktFST.normflst.istHistorisch = False
        Dim dt As DataTable = initNennerCombo()
        tbWeitergabeVerbot.Text = albverbotsString
        If dt.Rows.Count = 1 Then
            tbNenner.Text = dt.Rows(0).Item(0).ToString
            aktFST.normflst.nenner = CInt(tbNenner.Text)
            aktFST.normflst.FS = aktFST.normflst.buildFS
            FSGKrechtsGKHochwertHolen(aktFST.normflst, WinDetailSucheFST.AktuelleBasisTabelle)
            aktFST.normflst.GKhoch = CInt(aktFST.normflst.GKhoch)
            aktFST.normflst.GKrechts = CInt(aktFST.normflst.GKrechts)
            aktFST.punkt.X = aktFST.normflst.GKrechts
            aktFST.punkt.Y = aktFST.normflst.GKhoch
            aktGlobPoint.strX = CType(CInt(aktFST.normflst.GKrechts), String)
            aktGlobPoint.strY = CType(CInt(aktFST.normflst.GKhoch), String)
            kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, CInt(aktFST.normflst.radius) * 2)
            returnValue = True
            spNachnenner.Visibility = Visibility.Visible
            If Not ckFormNichtSchliessen.IsChecked Then
                Close()
            End If
            wennAlbBerechtigtDannGroupboxEinschalten()
        Else
            cmbNenner.IsDropDownOpen = True
        End If
        e.Handled = True
    End Sub

    Function initNennerCombo() As DataTable
        Dim dt As DataTable = holeNennerDT(WinDetailSucheFST.AktuelleBasisTabelle)
        cmbNenner.DataContext = dt
        Dim dthist As DataTable = holeNennerDT(myglobalz.histFstView)
        cmbNennerHist.DataContext = dthist
        Return dt
    End Function



    Private Sub cmbNenner_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbNenner.SelectionChanged
        e.Handled = True
        Dim item2 As DataRowView = CType(cmbNenner.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Try
        Catch ex As Exception
            Exit Sub
        End Try
        tbNenner.Text = item2.Row.ItemArray(0).ToString
        aktFST.normflst.istHistorisch = False
        clsFSTtools.holeKoordinaten4Flurstueck(tbNenner.Text, WinDetailSucheFST.AktuelleBasisTabelle, aktFST)
        returnValue = True
        If ckFormNichtSchliessen.IsChecked Then
            spNachnenner.Visibility = Visibility.Visible
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




    Private Sub chkInsArchiv_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub chkEreignisMap_Click_1(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub
    Private Sub btnEigentuemer_Click(sender As Object, e As RoutedEventArgs)
        If Not GisUser.istalbberechtigt Then
            MsgBox("Der User: " & GisUser.username & " ist nicht berechtigt auf die Eigentümerdaten zuzugreifen. ")
            Exit Sub
        End If
        If tbGrund.Text Is Nothing OrElse tbGrund.Text.Trim.Length < 2 Then
            MsgBox("Bitte eine Begründung (z.B. das Aktenzeichen) eingeben!")
            Exit Sub
        End If
        GrundFuerEigentuemerabfrage = tbGrund.Text

        If cbSchnellEigentuemer.IsChecked Then
            If aktFST.normflst.istHistorisch Then
                MessageBox.Show("Sie haben ein historisches Flurstück gewählt. " & Environment.NewLine &
                                "Dazu liegen keine Eigentümerdaten vor. " & Environment.NewLine &
                                "Wählen sie ein aktuelles Flurstück für Eigentümerdaten!", "Historisches Flurstück")
            Else
                tbWeitergabeVerbot.Text = SchnellausgabeMitProtokoll(tbGrund.Text, aktFST)
                EigentuemerPDF = erzeugeundOeffneEigentuemerPDF()
                OpenDokument(EigentuemerPDF)
            End If
        Else
            If aktFST.normflst.istHistorisch Then
                MessageBox.Show("Sie haben ein historisches Flurstück gewählt. " & Environment.NewLine &
                                "Dazu liegen keine Eigentümerdaten vor. " & Environment.NewLine &
                                "Wählen sie das aktuelle Flurstück für Eigentümerdaten!", "Historisches Flurstück")
            Else
                Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
                'tbKurz.Text = "Bitte warten  ...."
                Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
                aktFST.normflst.splitFS(aktFST.normflst.FS)
                Dim specparms(8) As String
                specparms(3) = aktFST.normflst.FS
                specparms(4) = aktFST.normflst.FS
                Dim weistauf, zeigtauf, gebucht, areaqm As String
                If Module2.holeRestlicheParams4FST(aktFST.normflst.FS, weistauf, zeigtauf, gebucht, areaqm) Then
                    specparms(5) = weistauf
                    specparms(6) = zeigtauf
                    specparms(7) = gebucht
                    specparms(8) = areaqm
                    EigentuemerPDF = getEigentuemerDatei(specparms)
                    tools.openDocument(EigentuemerPDF)
                End If
            End If

        End If
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
            Dim sw As New IO.StreamWriter(_protokoll, True)
            sw.WriteLine(Now & "#" & GisUser.username & "#" & clsActiveDir.fdkurz & "#" & "DESKTOP" & "#" & grund & "#" & flurstueck & "#" & "#" & "#" & "#" & "#")
            sw.Close()
            sw.Dispose()
        Catch ex As Exception
            'sw.WriteLine("Fehler in kontzrollausgabe:" & ex.ToString)
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

    Private Sub cmbFSTalt_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If cmbFSTalt.SelectedItem Is Nothing Then Exit Sub
        Dim item As ComboBoxItem
        item = CType(cmbFSTalt.SelectedItem, ComboBoxItem)
        Select Case item.Tag.ToString.ToLower.Trim
            Case "aktuell"
                AktuelleBasisTabelle = "flurkarte.basis_f"
            Case "1998", "1999", "2000", "2001", "2002"
                AktuelleBasisTabelle = "h_flurkarte.j" & item.Tag.ToString.ToLower & "_flurstueck_f"
            Case Else
                AktuelleBasisTabelle = "h_flurkarte.j" & item.Tag.ToString.ToLower & "_basis_f"
        End Select
        fstaltinfo.Text = "Gesucht wird in Flurkarte: " & item.Tag.ToString.ToLower.Trim & Environment.NewLine
        fstaltinfo.Text = fstaltinfo.Text & Environment.NewLine &
            "Falls Sie die historische Flurkarte laden wollen: " & Environment.NewLine &
            "      " & Environment.NewLine &
            "- Aktivieren Sie den Explorer links oben und  " & Environment.NewLine &
            "- wählen Sie dann die historischen Flurkarten: " & Environment.NewLine &
            "- klicken Sie auf die gewünschte Flurkarte. " & Environment.NewLine &
            "- schliessen Sie den Explorer " & Environment.NewLine


        'If item.Tag.ToString.ToLower.Trim = "fstakt" Then
        '    AktuelleBasisTabelle = "flurkarte.basis_f"
        'End If

        'stContext.Visibility = Visibility.Collapsed
        'panningAusschalten()
        'aktaid = CInt(nck.Tag)
        e.Handled = True
    End Sub

    Private Sub cbFSTalt_Click(sender As Object, e As RoutedEventArgs)
        If cbFSTalt.IsChecked Then
            spAuswahlAlt.Visibility = Visibility.Visible
        Else
            spAuswahlAlt.Visibility = Visibility.Hidden
        End If
        e.Handled = True
    End Sub

    Private Sub btnSchnellNachPDF_Click(sender As Object, e As RoutedEventArgs)
        erzeugeundOeffneEigentuemerPDF()
        e.Handled = True
    End Sub

    Private Function erzeugeundOeffneEigentuemerPDF() As String
        Dim lokalitaet, flaeche As String
        'Dim ausgabeDIR As String = My.Computer.FileSystem.SpecialDirectories.Temp '& "" & aid
        'ausgabeDIR = tools.calcEigentuemerAusgabeFile 'My.Computer.FileSystem.SpecialDirectories.MyDocuments
        lokalitaet = getlokalitaetstring(aktFST)
        flaeche = clsFSTtools.getFlaecheZuFlurstueck(aktFST)
        lokalitaet = lokalitaet & " " & flaeche
        Dim ausgabedatei As String = tools.calcEigentuemerAusgabeFile
        'EigentuemerPDF = ausgabeDIR & "\eigentuemer" & Format(Now, "ddMMyyyy_hhmmss") & ".pdf"
        wrapItextSharp.createSchnellEigentuemer(tbWeitergabeVerbot.Text, ausgabedatei, albverbotsString, lokalitaet)
        EigentuemerPDF = ausgabedatei
        Return EigentuemerPDF
    End Function
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
        Close()
    End Sub

    Private Sub cmbZaehlerHist_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        Dim item2 As DataRowView = CType(cmbZaehlerHist.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Dim item3$ = item2.Row.ItemArray(0).ToString
        cmbNenner.IsEnabled = True
        tbZaehler.Text = item2.Row.ItemArray(0).ToString
        aktFST.normflst.zaehler = CInt(item3$)
        aktFST.normflst.nenner = Nothing
        aktFST.normflst.istHistorisch = True
        Dim dt As DataTable = initNennerCombo()
        tbWeitergabeVerbot.Text = albverbotsString
        If dt.Rows.Count = 1 Then
            tbNenner.Text = dt.Rows(0).Item(0).ToString
            aktFST.normflst.nenner = CInt(tbNenner.Text)
            aktFST.normflst.FS = aktFST.normflst.buildFS
            FSGKrechtsGKHochwertHolen(aktFST.normflst, myglobalz.histFstView)
            aktFST.normflst.GKhoch = CInt(aktFST.normflst.GKhoch)
            aktFST.normflst.GKrechts = CInt(aktFST.normflst.GKrechts)
            aktFST.punkt.X = aktFST.normflst.GKrechts
            aktFST.punkt.Y = aktFST.normflst.GKhoch
            aktGlobPoint.strX = CType(CInt(aktFST.normflst.GKrechts), String)
            aktGlobPoint.strY = CType(CInt(aktFST.normflst.GKhoch), String)
            kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, CInt(aktFST.normflst.radius) * 2)
            returnValue = True
            spNachnenner.Visibility = Visibility.Visible
            If Not ckFormNichtSchliessen.IsChecked Then
                Close()
            End If
            '  wennAlbBerechtigtDannGroupboxEinschalten()

            historyLast = True
        Else
            cmbNennerHist.IsDropDownOpen = True
        End If
        e.Handled = True
    End Sub

    Private Sub cmbNennerHist_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        Dim item2 As DataRowView = CType(cmbNennerHist.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Try
        Catch ex As Exception
            Exit Sub
        End Try
        tbNenner.Text = item2.Row.ItemArray(0).ToString

        aktFST.normflst.istHistorisch = True
        clsFSTtools.holeKoordinaten4Flurstueck(tbNenner.Text, myglobalz.histFstView, aktFST)
        returnValue = True
        If ckFormNichtSchliessen.IsChecked Then
            spNachnenner.Visibility = Visibility.Visible
            If aktvorgangsid.Trim.Length > 2 Then
                tbGrund.Text = aktvorgangsid
                ' SchnellausgabeMitProtokoll(tbGrund.Text)

                If STARTUP_mgismodus.ToLower = "paradigma" Then
                    gbFSTaradigma.IsEnabled = True
                End If
                '    wennAlbBerechtigtDannGroupboxEinschalten() 
            End If
        Else
            historyLast = True
            Close()
        End If
        Close() 'auf jeden fall schliessen damit keine eigentümerabfrage möglich ist
    End Sub

    Private Sub cbFSTHist_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbFSTHist.IsChecked Then
            histControlls("ein")
        Else
            histControlls("aus")
        End If
    End Sub

    Private Sub histControlls(einaus As String)
        If einaus = "ein" Then
            hist1.Visibility = Visibility.Visible
            hist2.Visibility = Visibility.Visible
            cmbZaehlerHist.Visibility = Visibility.Visible
            cmbNennerHist.Visibility = Visibility.Visible
        Else
            hist1.Visibility = Visibility.Collapsed
            hist2.Visibility = Visibility.Collapsed
            cmbZaehlerHist.Visibility = Visibility.Collapsed
            cmbNennerHist.Visibility = Visibility.Collapsed
        End If

    End Sub
End Class
