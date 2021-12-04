Imports System.Data
Imports System.Windows.Forms
Imports pgis

Partial Class MainWindow

    Sub New()
        InitializeComponent()
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        initdb()
        setDefKoord(bbox)
        calcBbox(rechts, hoch, bbox, 100)
        ProxyString = getproxystring()
        starteWebbrowserControl(bbox)
        initGemeindeCombo()
        initGemarkungsCombo()
        l("starte")
        tbrechts.Text = rechts
        tbhoch.Text = hoch
        Protokollausgabe_aller_Zugriff("nein")
        If NSfstmysql.ADtools.istUserAlbBerechtigt(Environment.UserName, fdkurz) Then
            gbEigentuemer.Visibility = Visibility.Visible
        Else
            gbEigentuemer.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Shared Sub setDefKoord(bbox As clsRange)
        'kreishaus
        rechts = "484629"
        hoch = "5540607"
    End Sub


    Private Sub starteWebbrowserControl(zbox As clsRange)
        Try
            nachricht("USERAKTION: googlekarte  vogel")
            Dim gis As New clsGISfunctions
            Dim result As String
            result = gis.GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(zbox, True, longitude, latitude)
            If result = "fehler" Or result = "" Then
            Else
                '  gis.starten(result)
                '  GMtemplates.templateStarten(result)
                wbSample.Navigate(New Uri(result))
            End If
            gis = Nothing
        Catch ex As Exception
            nachricht("fehler in starteWebbrowserControl1: " & ex.ToString)
        End Try
    End Sub

    Private Sub btnaktualisiernvogel_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub btngoogle3d_Click()
        Try
            nachricht("USERAKTION: googlekarte  vogel btn click")
            Dim gis As New clsGISfunctions
            Dim result As String
            Dim nbox As New clsRange
            ' calcBbox(rechts, hoch, nbox, 900)
            Dim radius = 300
            nbox.xl = CInt(rechts) - radius
            nbox.yl = CInt(hoch) - (radius * 2)
            nbox.xh = CInt(rechts) + radius
            nbox.yh = CInt(hoch)
            result = gis.GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(nbox, False, longitude, latitude)
            If result = "fehler" Or result = "" Then
            Else
                Process.Start("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", result)
            End If
            gis = Nothing
            Protokollausgabe_aller_Zugriff("ja")
        Catch ex As Exception
            nachricht("fehler in starteWebbrowserControl: " & ex.ToString)
        End Try
    End Sub
    Private Sub cmbgemeinde_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If cmbgemeinde.SelectedItem Is Nothing Then Exit Sub
        gemeindebigNRstring = CStr(cmbgemeinde.SelectedValue)
        Dim myvalx = CType(cmbgemeinde.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        tbGemeinde.Text = myvals
        gemeindestring = myvals
        'aktADR.gemeindeNr = CInt(CStr(myvali))
        'aktADR.gemeindeName = tbGemeinde.Text
        adrREC.mydb.Host = "gis"
        initStrassenCombo()
        cmbStrasse.DataContext = adrREC.dt
        cmbStrasse.IsDropDownOpen = True
        e.Handled = True
    End Sub
    Sub initGemeindeCombo()
        Dim testfi As New IO.FileInfo(gemeinde_verz)
        If Not testfi.Exists Then
            MessageBox.Show("Die Gemarkungsliste konnte nicht gefunden werden! " & Environment.NewLine &
                            gemeinde_verz)
        End If
        Dim existing As XmlDataProvider = TryCast(Me.Resources("XMLSourceComboBoxgemeinden"), XmlDataProvider)
        Try
            existing.Source = New Uri(gemeinde_verz) 'erz'".\daten\gemarkungen.xml")
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub
    Private Sub cmbStrasse_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbStrasse.SelectionChanged
        If cmbStrasse.SelectedItem Is Nothing Then Exit Sub
        Dim myvali$ = CStr(cmbStrasse.SelectedValue)
        Dim item2 As DataRowView = CType(cmbStrasse.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        sname = item2.Row.ItemArray(0).ToString.Trim
        strcode = item2.Row.ItemArray(1).ToString.Trim
        'sname = item2.Row.ItemArray(2).ToString.Trim


        Dim zeigtauf, summentext As String
        'Dim gemeindestring As String = item3$.Replace("06438" & , "001")
        '  gemeindestring = item3$.Substring(5, 3).Trim

        cmbHausnr.IsEnabled = True
        tbStrasse.Text = sname ' item2.Row.ItemArray(0).ToString 
        'myGlobalz.aktADR.strasseCode = CInt(item4)
        'myGlobalz.aktADR.strasseName = item5

        'myGlobalz.adrREC.mydb.Host = "kis"
        'myGlobalz.adrREC.mydb.Schema = "albnas"

        inithausnrCombo()
        If adrREC.dt.Rows.Count < 1 Then
            Dim infotext As String
            Dim mesresult As New MessageBoxResult

            infotext = "Hinweis: Es konnten keine Hausnummern zu dieser Straße gefunden werden." & Environment.NewLine &
                "Entweder es handelt sich um ein Gewann / Flurbezeichnung, oder " & Environment.NewLine &
                " es gibt keine bewohnten Adressen in der Straße." & Environment.NewLine &
                "" & Environment.NewLine &
                "" & Environment.NewLine &
                " Sie können hier abbrechen      (Abbruch)" & Environment.NewLine &
                "oder sich die zu dieser Gewann gehörigen Flurstücke auflisten lassen (OK)"
            mesresult = CType(MessageBox.Show(infotext, "Keine Hausnummern gefunden"), MessageBoxResult)

            If mesresult = MessageBoxResult.Cancel Then
                Exit Sub
                e.Handled = True
            Else
                tbHausnr.Text = ""
                'buttonenDISablen()

                ' inithausnrCombo2(gemeindestring)
                'zeigtauf = myGlobalz.adrREC.dt.Rows(0).Item("gml_id").ToString
                'getflurstueckWeistauf(zeigtauf, "zeigtauf")
                'mapFlurstueck(myGlobalz.fstREC, myGlobalz.aktFST, summentext)
                'MessageBox.Show(summentext)
                'NennerVerarbeiten(myGlobalz.aktFST.nenner.ToString)
                'buttonEnablen()
            End If
        Else

            cmbHausnr.DataContext = adrREC.dt
            cmbHausnr.IsDropDownOpen = True
        End If
        e.Handled = True
    End Sub
    Private Sub cmbHausnr_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbHausnr.SelectionChanged
        If cmbHausnr.SelectedItem Is Nothing Then Exit Sub
        Dim hausnrkombi$ = CStr(cmbHausnr.SelectedValue)
        Dim item2 As DataRowView = CType(cmbHausnr.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Dim summentext As String = "" ' wird hier ignoriert
        Dim HausKombi = hausnrkombi
        Dim weistauf = item2.Row.ItemArray(1).ToString
        rechts = item2.Row.ItemArray(2).ToString
        hoch = item2.Row.ItemArray(3).ToString
        tbHausnr.Text = hausnrkombi
        calcBbox(rechts, hoch, bbox, 100)
        starteWebbrowserControl(bbox)
        e.Handled = True
    End Sub

    Private Sub calcBbox(rechts As String, hoch As String, bbox As clsRange, radius As Integer)
        bbox.xl = CInt(rechts) - radius
        bbox.yl = CInt(hoch) - radius
        bbox.xh = CInt(rechts) + radius
        bbox.yh = CInt(hoch) + radius
    End Sub

    Private Sub btnInfo_Click(sender As Object, e As RoutedEventArgs)
        Dim aaa As New winRTF
        aaa.ShowDialog()
        e.Handled = True
    End Sub
    Sub initGemarkungsCombo()
        Dim testfi As New IO.FileInfo(_verz)
        If Not testfi.Exists Then
            MessageBox.Show("Die Gemarkungsliste konnte nicht gefunden werden! " & Environment.NewLine &
                            _verz)
        End If
        Dim existing As XmlDataProvider = TryCast(Me.Resources("XMLSourceComboBoxgemarkungen"), XmlDataProvider)
        Try
            existing.Source = New Uri(_verz) 'erz'".\daten\gemarkungen.xml")
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub


    Private Sub cmbNenner_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbNenner.SelectionChanged
        Dim item2 As DataRowView = CType(cmbNenner.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Try
        Catch ex As Exception
            Exit Sub
        End Try
        tbNenner.Text = item2.Row.ItemArray(0).ToString
        NennerVerarbeiten(tbNenner.Text)
        aktFST.FS = aktFST.buildFS
        rechtsHochwertHolen(aktFST)
        rechts = CType(CInt(aktFST.GKrechts), String)
        hoch = CType(CInt(aktFST.GKhoch), String)
        calcBbox(rechts, hoch, bbox, 100)
        starteWebbrowserControl(bbox)
        e.Handled = True
    End Sub
    Private Sub cmbFlur_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbFlur.SelectionChanged
        Dim item2 As DataRowView = CType(cmbFlur.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub

        cmbZaehler.IsEnabled = True
        Dim item3$ = item2.Row.ItemArray(0).ToString
        tbFlur.Text = item2.Row.ItemArray(0).ToString
        aktFST.flur = CInt(item3$)
        initZaehlerCombo()
        cmbZaehler.IsDropDownOpen = True
    End Sub
    Sub initZaehlerCombo()
        holeZaehlerDT()
        cmbZaehler.DataContext = fstREC.dt
    End Sub
    Private Sub cmbgemarkung_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If cmbgemarkung.SelectedItem Is Nothing Then Exit Sub
        Dim myvali$ = CStr(cmbgemarkung.SelectedValue)
        Dim myvalx = CType(cmbgemarkung.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        tbGemarkung.Text = myvals
        aktFST.gemcode = CInt(myvali)
        aktFST.gemarkungstext = tbGemarkung.Text
        initFlureCombo()
        cmbFlur.IsDropDownOpen = True
        e.Handled = True
    End Sub
    Sub initFlureCombo()
        holeFlureDT()
        cmbFlur.DataContext = fstREC.dt
    End Sub
    Private Sub cmbZaehler_SelectionChanged(ByVal sender As System.Object,
                                           ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbZaehler.SelectionChanged
        Dim item2 As DataRowView = CType(cmbZaehler.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Dim item3$ = item2.Row.ItemArray(0).ToString
        cmbNenner.IsEnabled = True
        tbZaehler.Text = item2.Row.ItemArray(0).ToString
        aktFST.zaehler = CInt(item3$)
        aktFST.nenner = Nothing
        initNennerCombo()
        tbWeitergabeVerbot.Text = verbotsString
        If fstREC.dt.Rows.Count = 1 Then
            tbNenner.Text = fstREC.dt.Rows(0).Item(0).ToString
            aktFST.nenner = CInt(tbNenner.Text)
            aktFST.FS = aktFST.buildFS
            rechtsHochwertHolen(aktFST)
            rechts = CType(CInt(aktFST.GKrechts), String)
            hoch = CType(CInt(aktFST.GKhoch), String)
            calcBbox(rechts, hoch, bbox, 100)
            starteWebbrowserControl(bbox)
        Else
            cmbNenner.IsDropDownOpen = True
        End If
        e.Handled = True
    End Sub
    Sub initNennerCombo()
        holeNennerDT()
        cmbNenner.DataContext = fstREC.dt
    End Sub
    Public Shared Sub nennerUndFSPruefen()
        aktFST.FS = aktFST.buildFS()
        aktFST.fstueckKombi = aktFST.buildFstueckkombi
    End Sub
    Private Sub NennerVerarbeiten(ByVal nennertext As String)
        aktFST.nenner = CInt(nennertext)
        nennerUndFSPruefen()
    End Sub

    Private Sub startKoord_Click(sender As Object, e As RoutedEventArgs)
        rechts = CType(tbrechts.Text, String)
        hoch = CType(tbhoch.Text, String)
        calcBbox(rechts, hoch, bbox, 100)
        starteWebbrowserControl(bbox)
        e.Handled = True
    End Sub

    Private Sub btnEigentuemer_Click(sender As Object, e As RoutedEventArgs)
        'MsgBox("Coming soon")
        SchnellausgabeMitProtokoll()
        e.Handled = True
    End Sub
    Private Sub SchnellausgabeMitProtokoll()
        'If schonSchnellAusgegeben Then Exit Sub
        Dim grund As String = tbGrund.Text

        If grund Is Nothing OrElse grund.Trim.Length < 2 OrElse grund = "Aktenzeichen" Then
            MsgBox("Bitte eine Begründung (z.B. das Aktenzeichen) eingeben!")
            FocusManager.SetFocusedElement(Me, tbGrund)
            Exit Sub
        End If
        Dim info As String

        info = "Eigentümer in Kurzform: " & Environment.NewLine &
                                    getSchnellbatch(aktFST.FS)
        tbWeitergabeVerbot.Text = info
        'schonSchnellAusgegeben = True
        Protokollausgabe_aller_Parameter(aktFST.FS, grund)
    End Sub

    Private Sub tbGrund_SelectionChanged(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub
End Class
