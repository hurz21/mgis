Imports System.Data
Imports Npgsql
Class MainWindow
    Dim offlinedt As New DataTable
    Property aktAid As Integer
    Property akttitel As String
    Property aktpfad As String
    Property aktebene As String
    Property legenden As New List(Of clsLegendenItem)
    'Property stamm As New List(Of clsStamm)
    Property dokus As New List(Of clsDoku)
    Property gisRoot As String
    Property updatebat As String
    Property gisexe As String

    Public mapfileCachePathroot As String = serverUNC & "websys\mapfiles\cache\"
    Property mapfileBILD As String = mapfileCachePathroot & Environment.UserName & ".map"
    Public Property ladevorgangAbgeschlossen As Boolean = False

    Sub New()
        InitializeComponent()
    End Sub
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

#If DEBUG Then
        dbServername = "localhost"
        dbServername = "gis"
        serverUNC = "d:\"
        domainstring = "http://127.0.0.1"

        dbServername = "gis"
        serverUNC = "\\gis\gdvell\"
        domainstring = "http://gis.kreis-of.local"
#Else
        dbServername = "gis"
        serverUNC = "\\gis\gdvell\"
        domainstring = "http://gis.kreis-of.local"
#End If



        mapfileCachePathroot = serverUNC & "websys\mapfiles\cache\"


        tbVidParadigma.Text = "36677"
        stamm_tabelle = "std_stamm"
        bildetitel()
        mapfileBILD = mapfileCachePathroot & Environment.UserName & ".map"
        Dim sgauswahl As String = ""
        If Environment.UserName = "feinen_j" Then
            moveleg.Visibility = Visibility.Visible
        Else
            moveleg.Visibility = Visibility.Collapsed
        End If
        legenden = modDB.LegendeCollectioneinlesen()
        dokus = modDB.DokusColleinlesen
        initAuswahlliste(sgauswahl) : dgEbenen.DataContext = wgisdt
        initSachgebietAuswahlColl() : cmbSachgebiet.DataContext = sgColl
        updatebat = tools.serverUNC & "\apps\test\mgis\mgisAktualisieren.bat"
        gisRoot = "C:\kreisoffenbach\mgis\"
        gisexe = gisRoot & "mgis.exe"
        ladevorgangAbgeschlossen = True
    End Sub

    Private Sub bildetitel()
        Title = "GIS Kontroll Studio" & vbTab & "           Stamm: " & stamm_tabelle & ", " & domainstring
    End Sub

    Private Sub dgEbenen_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If dgEbenen.SelectedItem Is Nothing Then Exit Sub
            Dim item As DataRowView = CType(dgEbenen.SelectedItem, DataRowView)
            aktAid = CInt(item("AID").ToString)
            aktpfad = (item("pfad").ToString)
            aktebene = (item("ebene").ToString)
            akttitel = (item("titel").ToString)
            Dim teststatus = (item("status").ToString)
            gbEbenenspec.Header = "Ebenenspezifisch:   " & akttitel & " (" & CType(aktAid, String) & ")"

            ' OpenEbenenHauptForm(aktAid)
        Catch ex As Exception
            l("dgEbenen_SelectionChanged " & ex.ToString)
        End Try
    End Sub

    Private Sub OpenEbenenHauptForm(aktAid As Integer)
        Dim eh As New winEbenenHaupt(aktAid)
        eh.ShowDialog()
    End Sub

    Private Sub cmbSachgebiet_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim sgitem As New clsSachgebietsCombo
        If cmbSachgebiet.SelectedItem Is Nothing Then Exit Sub
        '  If cmbSachgebiet.SelectedIndex = 0 Then Exit Sub

        If stamm_tabelle = "stamm" Then
            MsgBox("Diese Abfrage funzt nur über den View STD_stamm")
            Exit Sub
        End If

        sgitem = CType(cmbSachgebiet.SelectedItem, clsSachgebietsCombo)
        If sgitem.sid = String.Empty Then
            initAuswahlliste("") : dgEbenen.DataContext = wgisdt
        Else
            initAuswahlliste(sgitem.sid) : dgEbenen.DataContext = wgisdt
        End If
        cmbSachgebiet.SelectedItem = Nothing
        e.Handled = True
    End Sub

    Private Sub sucheStarten_Click(sender As Object, e As RoutedEventArgs)
        Dim sql As String
        If IsNumeric(tbSuchfilter.Text.Trim) Then
            sql = "SELECt * FROM  " & stamm_tabelle & " where aid=" & tbSuchfilter.Text.ToLower.Trim &
                " or lower(titel) like '%" & tbSuchfilter.Text.ToLower.Trim & "%'"
        Else
            sql = "SELECt * FROM  " & stamm_tabelle & " where lower(titel) like '%" & tbSuchfilter.Text.ToLower.Trim & "%'"
        End If
        offlinedt = getDT(sql, tools.dbServername, "webgiscontrol")
        dgEbenen.DataContext = offlinedt
    End Sub

    Private Sub chkDoku_Click(sender As Object, e As RoutedEventArgs)
        Dim dokuFehler As String
        dokuFehler = modDB.checkAllDoku(dokus)
        Dim wini As winInfo
        If dokuFehler <> "" Then
            dokuFehler = dokuFehler & Environment.NewLine
            dokuFehler = dokuFehler & modDB.makertfDokuLoop(dokus)
            wini = New winInfo(dokuFehler)
        Else
            wini = New winInfo(dokuFehler)
        End If
        wini.ShowDialog()
        e.Handled = True
    End Sub

    Private Sub chkLeg_Click(sender As Object, e As RoutedEventArgs)
        Dim legFehler As String
        legFehler = modDB.checkAllLegende(legenden)
        legFehler = legFehler & modDB.makeRTFlegenden(legenden)
        If legFehler = "" Then
            legFehler = "legenden OK" & Environment.NewLine
        Else
            Dim wini As New winInfo(legFehler)
            wini.ShowDialog()
        End If

    End Sub

    Private Sub moveleg_Click(sender As Object, e As RoutedEventArgs)
        ' offlinetools.movealleLegs(legenden)
        '  offlinetools.movealleMapfiles()
        offlinetools.copyNatlandPDF()
        e.Handled = True
    End Sub

    Private Sub btnZumVerzeichnis_Click(sender As Object, e As RoutedEventArgs)
        If aktAid = 0 Then
            MsgBox("Zuerst eine Ebene auswählen")
            Exit Sub
        End If
        Dim zielroot = tools.serverUNC & "\nkat\aid\" & aktAid & ""
        openDirectory(zielroot)
        e.Handled = True
    End Sub

    Private Sub btnLayerMapfile_Click(sender As Object, e As RoutedEventArgs)
        If aktAid = 0 Then
            MsgBox("Zuerst eine Ebene auswählen")
            Exit Sub
        End If
        Dim zielroot = tools.serverUNC & "\nkat\aid\" & aktAid & "\layer.map"

        opendocument(zielroot)

        e.Handled = True
    End Sub



    Private Sub btnHeaderMapfile_Click(sender As Object, e As RoutedEventArgs)
        If aktAid = 0 Then
            MsgBox("Zuerst eine Ebene auswählen")
            Exit Sub
        End If
        Dim zielroot = tools.serverUNC & "\nkat\aid\" & aktAid & "\header.map"
        opendocument(zielroot)
        e.Handled = True
    End Sub

    Private Sub btnFkatdir_Click(sender As Object, e As RoutedEventArgs)
        If aktAid = 0 Then
            MsgBox("Zuerst eine Ebene auswählen")
            Exit Sub
        End If
        Dim zielroot = tools.serverUNC & aktpfad & "\" '& aktebene & "\"
        zielroot = zielroot.Replace("/", "\").Replace("\\", "\").Replace("\gis\", "\\gis\")
        Try
            l("btnFkatdir_Click---------------------- anfang")
            opendocument(zielroot)
            l("btnFkatdir_Click---------------------- ende")
        Catch ex As Exception
            l("Fehler in btnFkatdir_Click: " & ex.ToString())
        End Try

        e.Handled = True
    End Sub


    Private Sub btndatabse_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnSachgebiete_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnDoku_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnLegende_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnPDFbeiwerk_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnTiff_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnInternet_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub btnLayerMapfileTesten_Click(sender As Object, e As RoutedEventArgs)
        If aktAid = 0 Then
            MsgBox("Zuerst eine Ebene auswählen")
            Exit Sub
        End If
        Dim test = mapfileErzeugen(aktAid, mapfileBILD)
        Dim mft As New mapfileTest(mapfileBILD, "einzeln", aktAid)
        mft.Show()
        e.Handled = True
    End Sub
    Private Sub btnStartGIS_Click(sender As Object, e As RoutedEventArgs)
        Process.Start(updatebat)
        Dim Parameter As String = buildMgisMOdusString()
        'Process.Start(Parameter)
        Dim startinfo As New ProcessStartInfo
        startinfo.WorkingDirectory = "c:\kreisoffenbach\mgis"
        startinfo.FileName = gisexe
        tbAufruf.Text = Parameter
        startinfo.Arguments = Parameter
        ' Dim params As String = " modus=probaug suchmodus=adresse gemeinde=dietzenbach strasse=am rebstock hausnr=42"
        'btnStartGISProbaug.ToolTip = params
        '   params   = " modus=probaug suchmodus=flurstueck gemarkung=dietzenbach flur=5 fstueck=490/0"
        '  Process.Start(gisexe, Parameter)
        Process.Start(startinfo)
        e.Handled = True
    End Sub

    Private Function buildMgisMOdusString() As String
        Dim modus, summe, username, vid, adresse, fstueck As String
        adresse = ""
        fstueck = ""
        modus = " modus=vanilla "
        If rbvanilla.IsChecked Then
            modus = " modus=vanilla "
        End If
        If tbStealth.Text = "" Then
            username = ""
        Else
            username = " username=" & tbStealth.Text.Trim
        End If
        If rbprobaug.IsChecked Then
            modus = " modus=probaug az=1212-2017" 'suchmodus=adresse gemeinde=dietzenbach strasse=""am rebstock"" hausnr=42"
        End If
        If rbparadigma.IsChecked Then
            modus = " modus=paradigma  vorgangsid=" & tbVidParadigma.Text & " range=490248,491254,5548144,5548704"
        End If
        If rbAdressweise.IsChecked Then
            adresse = " suchmodus=adresse gemeinde=""" & tbGemeinde.Text & """ strasse=""" & tbStrasse.Text & """ hausnr=""" & tbHausnr.Text & """"
        End If
        If rbKatasterweise.IsChecked Then
            fstueck = " suchmodus=flurstueck gemarkung=""" & tbGemarkung.Text.Trim & """ flur=""" & tbFlur.Text & """ fstueck=""" & tbFlurstueck.Text & """"
        End If
        summe = modus & username & fstueck & adresse
        Return summe.Trim
    End Function

    Private Sub btnStartGISProbaug_Click(sender As Object, e As RoutedEventArgs)
        Process.Start(updatebat)
        Dim params As String = " modus=probaug suchmodus=adresse gemeinde=dietzenbach strasse=am rebstock hausnr=42"
        'btnStartGISProbaug.ToolTip = params
        '   params   = " modus=probaug suchmodus=flurstueck gemarkung=dietzenbach flur=5 fstueck=490/0"
        Process.Start(gisexe, params)
        e.Handled = True
    End Sub
    Private Sub btnStartGISParadigma_Click(sender As Object, e As RoutedEventArgs)
        Process.Start(updatebat)
        Dim params As String = " modus=paradigma vorgangsid=" & tbVidParadigma.Text & " range=490248,491254,5548144,5548704"
        Process.Start(gisexe, params)
        e.Handled = True
    End Sub
    Private Sub btnWebgisINtranet_Click(sender As Object, e As RoutedEventArgs)
        Dim datei As String = tools.serverUNC & "/buergergis/index.htm"
        Process.Start("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", datei)
        e.Handled = True
    End Sub
    Private Sub btnalleLayerChecken_Click(sender As Object, e As RoutedEventArgs)
        Dim www As New winAlleLayer(mapfileBILD)
        www.Show()
        e.Handled = True
    End Sub

    Private Sub cmbStammTab_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If cmbStammTab.SelectedItem Is Nothing Then Exit Sub
        Dim item As ComboBoxItem = CType(cmbStammTab.SelectedItem, ComboBoxItem)
        If item.Tag.ToString = "stamm" Then
            stamm_tabelle = "stamm"
        Else
            stamm_tabelle = "std_stamm"
        End If
        bildetitel()
        initAuswahlliste("") : dgEbenen.DataContext = wgisdt
        e.Handled = True
    End Sub

    Private Sub btnstammedit_Click(sender As Object, e As RoutedEventArgs)
        If aktAid = 0 Then
            MsgBox("Zuerst eine Ebene auswählen")
            Exit Sub
        End If
        Dim dbedit As New winEbenenHaupt(aktAid)
        dbedit.Show()
        e.Handled = True
    End Sub

    Private Sub btnNurHintergrund_Click(sender As Object, e As RoutedEventArgs)
        wgisdt = getDT("SELECt * FROM  " & stamm_tabelle & " where aid in (select aid from hintergrund)", tools.dbServername, "webgiscontrol")
        dgEbenen.DataContext = wgisdt
        e.Handled = True
    End Sub

    Private Sub btnAddNewLayer_Click(sender As Object, e As RoutedEventArgs)
        Dim report As String = ""
        Dim newaid As Integer = createNewLayer(report)
        If newaid > 0 Then
            aktAid = newaid
            MsgBox(report, MsgBoxStyle.OkOnly, " Neue Ebene anlegen")
        Else

        End If
        e.Handled = True
    End Sub

    Private Sub btnSG_Click(sender As Object, e As RoutedEventArgs)
        Dim sg As New winSachgeiete
        sg.Show()
        e.Handled = True
    End Sub

    Private Sub btnStatus0_Click(sender As Object, e As RoutedEventArgs)
        wgisdt = getDT("SELECt * FROM  " & stamm_tabelle & " where status=false", tools.dbServername, "webgiscontrol")
        dgEbenen.DataContext = wgisdt
        e.Handled = True
    End Sub

    Private Sub btnStartGISStealth_Click(sender As Object, e As RoutedEventArgs)
        'Process.Start(updatebat)
        'Dim params As String = " modus=paradigma vorgangsid=" & tbVidParadigma.Text & " username=" & tbStealth.Text
        'Process.Start(gisexe, params)
        e.Handled = True
    End Sub

    Private Sub BtnPGADMIN_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim pga As String = "C:\Program Files (x86)\pgAdmin III\1.18\pgadmin3.exe"
        Process.Start(pga, "/s=w2")
    End Sub

    Private Sub BtnPGADMINbg_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim pga As String = "C:\Program Files (x86)\pgAdmin III\1.18\pgadmin3.exe"
        Process.Start(pga, "/s=buergergis")
    End Sub

    Private Sub BtnWMS_Click(sender As Object, e As RoutedEventArgs)
        Dim sql = "SELECT s.titel,     w.id,     w.aid,     w.daten,     w.typ,     w.format,     w.stdlayer   FROM wms w, " &
               " stamm s " &
               " WHERE s.aid = w.aid " &
               " ORDER BY s.titel;"
        wgisdt = getDT(sql, tools.dbServername, "webgiscontrol")
        dgEbenen.DataContext = wgisdt
        e.Handled = True
    End Sub

    Private Sub cmbServer_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If cmbServer.SelectedItem Is Nothing Then Exit Sub
        Dim item As ComboBoxItem = CType(cmbServer.SelectedItem, ComboBoxItem)
        If item.Tag.ToString = "intern" Then
            domainstring = "http://gis.kreis-of.local"
            tools.dbServername = "gis"
        Else
            domainstring = "https://buergergis.kreis-offenbach.de"
            tools.dbServername = "buergergis"
        End If
        Title = Title & "// " & domainstring
        bildetitel()
        initAuswahlliste("") : dgEbenen.DataContext = wgisdt
        e.Handled = True
    End Sub
End Class

