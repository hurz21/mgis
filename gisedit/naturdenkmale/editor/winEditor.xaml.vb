Imports System.ComponentModel

Public Class winEditor
    Property schema As String
    Property gruppenID As String
    Public Property individualID As Integer = 0
    Property tabelle As String
    Property ladevorgangabgeschlossen As Boolean = False
    Public Property aktgid As String
    Property ndindividuenListe As New List(Of clsNDinidividuum)
    Property VGmyBitmapImage As New BitmapImage
    Property hgrund As String = "flurkarte"

    Sub New(_schema As String, _tabelle As String, _gruppenID As String, _individualID As Integer)
        InitializeComponent()
        schema = _schema
        tabelle = _tabelle
        gruppenID = _gruppenID
        individualID = _individualID
    End Sub

    Private Sub winEditor_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        DatePicker1.SelectedDate = Now
        'fuelleEditFelder(editOjektGIDNr)
        fuelleFixeGruppenFelder(gruppenID) 'editOjektGIDNr)
        fuelleFixeIndividuenFelder(individualID)
        setTitel()
        refreshIndividuenDG()

        If ndindividuenListe.Count > 0 Then
            individualID = ndindividuenListe.Item(0).gid
        End If
        mset.mitte = clstools.getcoord(ndindividuenListe)
        mset.aktrange.point2range(mset.mitte, 200)
        If individualID > 0 Then
            aktgid = CType(individualID, String)
            fuelleIndividuum(CType(individualID, String), gruppenID)
            gbindivEditor.Visibility = Visibility.Visible
        End If
        refreshMap()
        ladevorgangabgeschlossen = True
    End Sub

    Private Sub refreshMap()
        Dim url As String = clstools.genPreviewURL(ndindividuenListe.Item(0), mset.aktrange, CInt(VGmapCanvas.Width), CInt(VGmapCanvas.Height), hgrund, False)
        setPreviewImageFromHttpURL(url)
        Canvas.SetTop(VGcanvasImage, 0)
        Canvas.SetLeft(VGcanvasImage, 0)
    End Sub
    Private Sub clearCanvas()
        GC.Collect()
        VGmapCanvas.Children.Clear()
        If VGcanvasImage IsNot Nothing Then
            VGcanvasImage.Source = Nothing
            VGcanvasImage = Nothing
        End If
        VGcanvasImage = New Image
        leeresbild(VGcanvasImage)

    End Sub

    Private Sub leeresbild(canvasImage As Image)
        Dim myBitmapImage As New BitmapImage()
        Dim aufruf As String = mset.serverWeb & "/apps/paradigma/ndman/leer.png" '"P:\a_vs\NEUPara\mgis\leer.png"
        Try
            myBitmapImage.BeginInit()
            myBitmapImage.UriSource = New Uri(aufruf, UriKind.Absolute)
            myBitmapImage.EndInit()
            canvasImage.Source = myBitmapImage
            GC.Collect()
        Catch ex As Exception
            clstools.l("fehler in leeresbild: " & aufruf & " /// " & ex.ToString)
        End Try
    End Sub
    Private Sub setPreviewImageFromHttpURL(url As String)
        'https mach tprobleme
        'Dim VGcanvasImage = New Image
        Try
            clstools.l(" setImageFromHttpURL ---------------------- anfang")
            clearCanvas()
            'Exit Sub
            mset.VGcanvasImage = New Image
            mset.VGcanvasImage.Name = "canvasImage"
            VGmapCanvas.Children.Add(mset.VGcanvasImage)
            VGmapCanvas.SetZIndex(mset.VGcanvasImage, 100)

            VGmyBitmapImage = New BitmapImage
            VGmyBitmapImage.BeginInit()
            VGmyBitmapImage.UriSource = New Uri(url, UriKind.Absolute)
            VGmyBitmapImage.EndInit()
            AddHandler VGmyBitmapImage.DownloadCompleted, AddressOf vgmyBitmapImage_DownloadCompleted
            Threading.Thread.Sleep(900)
            'VGcanvasImage.Source = VGmyBitmapImage
            clstools.l(" setImageFromHttpURL ---------------------- ende")
        Catch ex As Exception
            clstools.l("Fehler in setImageFromHttpURL: " & ex.ToString())
        End Try
    End Sub
    Private Sub vgmyBitmapImage_DownloadCompleted(sender As Object, e As EventArgs)
        mset.VGcanvasImage.Source = VGmyBitmapImage
        'clstools.saveImageasThumbnail2(clstools.auswahlBplan, clstools.BPLcachedir, VGmyBitmapImage)
    End Sub
    Private Sub fuelleFixeIndividuenFelder(gid As Object)
        Try
            clstools.l(" fuelleFixeIndividuenFelder ---------------------- anfang")

            clstools.l(" fuelleFixeIndividuenFelder ---------------------- ende")
        Catch ex As Exception
            clstools.l("Fehler in fuelleFixeIndividuenFelder: " & ex.ToString())
        End Try
    End Sub

    Function hatEditEintrag(aktgid As Integer) As Boolean
        Dim query = mset.queryIndividuenEDITRoot.Replace("[WHERESTRING]", " where gid=" & aktgid & "")
        mset.basisrec.mydb.SQL = query
        clstools.l(mset.basisrec.mydb.SQL)
        Dim hinweis As String
        hinweis = mset.basisrec.getDataDT()
        'ndgruppen = tools.dt2NDgruppen(clsTools.basisrec.dt) 
        If mset.basisrec.dt.Rows.Count < 1 Then
            Return False
        Else
            Return True
        End If
    End Function
    Sub refreshIndividuenDG()
        Try
            clstools.l(" refreshindividuenDG ---------------------- anfang")
            mset.basisrec.mydb.SQL = "SELECT * " &
                            "   FROM schutzgebiete.naturdenkmal_f " &
                            "   left outer join  paradigma_userdata.ndindividuenedit  on " &
                            "   schutzgebiete.naturdenkmal_f .gid = paradigma_userdata.ndindividuenedit.gid " &
                            " where  aid='" & gruppenID & "'"

            clstools.l(mset.basisrec.mydb.SQL)
            Dim hinweis As String
            hinweis = mset.basisrec.getDataDT()
            'ndgruppen = tools.dt2NDgruppen(clsTools.basisrec.dt) 
            'sql.ExecQuery(query, False)
            'If SQL.HasException(True) Then Exit Sub

            ndindividuenListe = clstools.dt2NDindivuduen(mset.basisrec.dt)
            dgNDindividuen.DataContext = ndindividuenListe


            clstools.l(" refreshindividuenDG ---------------------- ende")
        Catch ex As Exception
            clstools.l("Fehler in refreshindividuenDG: " & ex.ToString())
        End Try
    End Sub
    Private Sub DatePickerUntersuchung_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        tbuntersuchung.Text = Format(DatePickerUntersuchung.SelectedDate, "dd.MM.yyy")
        e.Handled = True
    End Sub

    Private Sub tbablaufks_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If tbablaufks.Text <> String.Empty Then btnablaufks.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btablaufks_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " ablaufdatumks ='" & tbablaufks.Text & "' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnablaufks.IsEnabled = False
        End If
        e.Handled = True
    End Sub

    Private Sub DatePickerablaufks_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        tbablaufks.Text = Format(DatePickerablaufks.SelectedDate, "dd.MM.yyy")
        e.Handled = True
    End Sub

    Private Sub tbBemerkung_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnBemerkung.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnBemerkung_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " bemerkung ='" & tbBemerkung.Text & "' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnBemerkung.IsEnabled = False
        End If
        e.Handled = True
    End Sub

    Private Sub cbKronensicherung_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnKronensicherung.IsEnabled = True
        If cbKronensicherung.IsChecked Then
            spKSdatum.IsEnabled = True
        Else
            spKSdatum.IsEnabled = True
        End If
        e.Handled = True
    End Sub

    Private Sub btnKronensicherung_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " kronensicherung ='" & CStr(CBool(cbKronensicherung.IsChecked)) & "' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnKronensicherung.IsEnabled = False
        End If
        e.Handled = True
    End Sub



    Private Sub tbregelkontrol_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub

        If tbregelkontrol.Text <> String.Empty Then btnregelkontrol.IsEnabled = True
        e.Handled = True
    End Sub
    Private Sub btnregelkontrol_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " regelkontrolle ='" & tbregelkontrol.Text & "' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnregelkontrol.IsEnabled = False
        End If
        e.Handled = True
    End Sub
    Private Sub btnVID_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " paradigmavid ='" & tbvid.Text & "' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        btnVID.IsEnabled = False
        e.Handled = True
    End Sub

    Private Sub DatePicker1_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        tbregelkontrol.Text = Format(DatePicker1.SelectedDate, "dd.MM.yyy")
        e.Handled = True
    End Sub
    Private Sub tbuntersuchung_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnuntersuchung.IsEnabled = True
        If tbuntersuchung.Text <> String.Empty Then btnuntersuchung.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnuntersuchung_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " untersuchung ='" & tbuntersuchung.Text & "' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnuntersuchung.IsEnabled = False
        End If
        e.Handled = True
    End Sub
    Private Sub tbvid_TextChanged(sender As Object, e As TextChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnVID.IsEnabled = True
        refreshIndividuenDG()
    End Sub
    Sub fuelleEditFelder(objektid As String)
        Try
            clstools.l("fuelleEditFelder---------------------- anfang")
            mset.basisrec.mydb.SQL = "select * from " & clstools.editSchema & "." & clstools.editTable &
           " where " & clstools.editOjektGIDNSpaltenname & "='" & objektid & "'"
            clstools.l(mset.basisrec.mydb.SQL)
            Dim hinweis As String
            hinweis = mset.basisrec.getDataDT()
            'ndgruppen = tools.dt2NDgruppen(clsTools.basisrec.dt)

            tbvid.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("paradigmavid"))
            tbregelkontrol.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("regelkontrolle"))
            tbuntersuchung.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("untersuchung"))
            Dim test = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("kronensicherung"))

            If CBool(clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("kronensicherung"))) = True Then
                cbKronensicherung.IsChecked = True
            Else
                cbKronensicherung.IsChecked = False
            End If
            tbablaufks.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("ablaufdatumks"))
            tbBemerkung.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("bemerkung")).Trim
            clstools.l("fuelleEditFelder ---------------------- ende")
        Catch ex As Exception
            clstools.l("Fehler in fuelleEditFelder: " & ex.ToString())
        End Try
    End Sub
    Private Sub setTitel()
        Title = "Editor: Naturdenkmal (161), Gruppennummer: " & gruppenID
    End Sub
    Private Sub fuelleFixeGruppenFelder(objektid As String)
        Try
            clstools.l("fuelleFixeFelder---------------------- anfang")
            mset.basisrec.mydb.SQL = "select * from  schutzgebiete.naturdenkmal_a " &
           " where aid" & "='" & objektid & "'"
            clstools.l(mset.basisrec.mydb.SQL)
            Dim hinweis As String
            hinweis = mset.basisrec.getDataDT()
            'ndgruppen = tools.dt2NDgruppen(clsTools.basisrec.dt)

            tbgemeinde.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("gemeinde"))
            tbgemarkung.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("gemarkung"))
            tbstammunfang.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("stammumfang"))
            tbName.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("name")).Trim
            tbBeschreibung.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("beschreibung")).Trim
            tbGruppenid.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("aid")).Trim
            clstools.l("fuelleFixeFelder---------------------- ende")
        Catch ex As Exception
            clstools.l("Fehler in fuelleFixeFelder: " & ex.ToString())

        End Try
    End Sub
    Private Sub btnclose_Click(sender As Object, e As RoutedEventArgs)
        Close()
        e.Handled = True
    End Sub

    Private Sub btnNDindividuen2excel_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub dgNDindividuen_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        ladevorgangabgeschlossen = False
        'If Not ladevorgangabgeschlossen Then Exit Sub
        If dgNDindividuen.SelectedItem Is Nothing Then Exit Sub
        Dim item2 As clsNDinidividuum = CType(dgNDindividuen.SelectedItem, clsNDinidividuum)
        If item2 Is Nothing Then Exit Sub
        gbindivEditor.Visibility = Visibility.Visible
        aktgid = CType(item2.gid, String)
        fuelleIndividuum(aktgid, gruppenID)
        dgNDindividuen.SelectedItem = Nothing
        ladevorgangabgeschlossen = True
    End Sub

    Private Sub fuelleIndividuum(aktgidlok As String, gruppenID As String)
        Try
            clstools.l(" fuelleIndividuum ---------------------- anfang")
            If Not hatEditEintrag(CInt(aktgidlok)) Then
                clstools.ndindivEditorAnlegen(aktgidlok, gruppenID)
            End If
            fuelleEditFelder2(aktgidlok)
            clstools.l(" fuelleIndividuum ---------------------- ende")
        Catch ex As Exception
            clstools.l("Fehler in fuelleIndividuum: " & ex.ToString())
        End Try
    End Sub



    Private Sub mouseOverIndividuen(sender As Object, e As MouseEventArgs)
        e.Handled = True
        dgNDindividuen.Width = 900
        spAktgruppe.Width = 300
    End Sub

    Private Sub mouseOverAktgruppe(sender As Object, e As MouseEventArgs)
        e.Handled = True
        dgNDindividuen.Width = 900
        spAktgruppe.Width = 300
    End Sub

    Private Sub btnGruppeLoeschen_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        clstools.loescheGruppe(gruppenID, ndindividuenListe)
    End Sub

    Sub fuelleEditFelder2(gid As String)
        Try
            clstools.l("fuelleEditFelder---------------------- anfang")
            mset.basisrec.mydb.SQL = "SELECT * " &
                            "   FROM schutzgebiete.naturdenkmal_f " &
                            "   left outer join  paradigma_userdata.ndindividuenedit  on " &
                            "   schutzgebiete.naturdenkmal_f .gid = paradigma_userdata.ndindividuenedit.gid " &
                            " where  ndindividuenedit.gid=" & gid & ""
            clstools.l(mset.basisrec.mydb.SQL)
            Dim hinweis As String
            hinweis = mset.basisrec.getDataDT()
            'ndgruppen = tools.dt2NDgruppen(clsTools.basisrec.dt)

            If mset.basisrec.dt.Rows.Count > 0 Then
                Dim temp As String
                tbBeschreibungINDI.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("name"))
                tbGID.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("gid"))
                tbvid.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("paradigmavid"))
                tbplakette.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("plakette"))


                temp = CStr(clsDBtools.fieldvalueDate(mset.basisrec.dt.Rows(0).Item("regelkontrolle")).ToString("dd.MM.yyyy"))
                If temp = "01.01.0001" Then
                    tbregelkontrol.Text = ""
                Else
                    tbregelkontrol.Text = temp
                End If



                temp = CStr(clsDBtools.fieldvalueDate(mset.basisrec.dt.Rows(0).Item("untersuchung")).ToString("dd.MM.yyyy"))
                If temp = "01.01.0001" Then
                    tbuntersuchung.Text = ""
                Else
                    tbuntersuchung.Text = temp
                End If


                temp = CStr(clsDBtools.fieldvalueDate(mset.basisrec.dt.Rows(0).Item("auge")).ToString("dd.MM.yyyy"))
                If temp = "01.01.0001" Then
                    tbauge.Text = ""
                Else
                    tbauge.Text = temp
                End If


                Dim test = CStr(clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("kronensicherung")))
                If test.IsNothingOrEmpty Then
                    cbKronensicherung.IsChecked = False
                Else
                    If CBool(test) = True Then
                        cbKronensicherung.IsChecked = True
                    Else
                        cbKronensicherung.IsChecked = False
                    End If
                End If

                If cbKronensicherung.IsChecked Then
                    spKSdatum.IsEnabled = True
                Else
                    spKSdatum.IsEnabled = True
                End If

                test = CStr(clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("verkehrssicher")))
                If test.IsNothingOrEmpty Then
                    cbVerkehrssicher.IsChecked = False
                Else
                    If CBool(test) = True Then
                        cbVerkehrssicher.IsChecked = True
                    Else
                        cbVerkehrssicher.IsChecked = False
                    End If
                End If

                temp = CStr(clsDBtools.fieldvalueDate(mset.basisrec.dt.Rows(0).Item("ablaufdatumks")).ToString("dd.MM.yyyy"))
                If temp = "01.01.0001" Then
                    tbablaufks.Text = ""
                Else
                    tbablaufks.Text = temp
                End If


                tbBemerkung.Text = clsDBtools.fieldvalue(mset.basisrec.dt.Rows(0).Item("bemerkung")).Trim
            End If
            clstools.l("fuelleEditFelder ---------------------- ende")
        Catch ex As Exception
            clstools.l("Fehler in fuelleEditFelder: " & ex.ToString())
        End Try
    End Sub

    Private Sub btnPlakette_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        updatendindividueneditPlakette()
        btnPlakette.IsEnabled = False
        e.Handled = True
    End Sub

    Private Sub updatendindividueneditPlakette()
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " plakette ='" & tbplakette.Text & "' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
    End Sub

    Private Sub tbplakette_TextChanged(sender As Object, e As TextChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnPlakette.IsEnabled = True
        refreshIndividuenDG()
    End Sub

    Private Sub btnZuParadigmaVID_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If tbvid.Text.IsNothingOrEmpty Then
            MessageBox.Show("Sie haben noch nichts hier eingegeben!")
        Else
            clstools.paradigmavorgangaufrufen(tbvid.Text)
        End If

    End Sub

    Private Sub btnrefreshListeIndividuen_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        refreshIndividuenDG()
    End Sub

    Private Sub PDF_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim pfad As String '= "\\file-office\Office\UMWELT\B\2-neue Struktur\3 - Naturschutz\30 - Rechtsgrundlagen und Allgemeines\300 - Rechtsgrundlagen, Urteile\3002 - Verordnungen, Erlasse, Richtlinien\Naturdenkmale\manager\"
        pfad = "\\gis\gdvell\nkat\aid\161\manager\"
        Dim datei = gruppenID & ".pdf"
        Process.Start(pfad & datei)
    End Sub

    Private Sub btnzumGis_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim rangestring, param, gisexe As String
        gisexe = "C:\ptest\mgis\mgis.exe"
        Dim lu, ro As New myPoint
        lu.X = mset.aktrange.xl
        lu.Y = mset.aktrange.yl
        ro.X = mset.aktrange.xh
        ro.Y = mset.aktrange.yh
        rangestring = clstools.calcrangestring(lu, ro)
        param = "modus=""bebauungsplankataster""  range=""" & rangestring & ""
        Process.Start(gisexe, param)
    End Sub

    Private Sub btngoolgePlain_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim url As String
        url = clstools.getGoogleMapsString(mset.aktrange, mset.enc)
        'webBrowserControlVogel.Navigate(New Uri(calcURI4vogel))
        Process.Start(url)
    End Sub

    Private Sub btn3d_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        clstools.google3d(mset.aktrange, mset.enc)
    End Sub

    Private Sub btnplus_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim xdifalt As Double = mset.aktrange.xdif / 2
        Dim xdifnew As Double
        mset.aktrange.CalcCenter()
        xdifnew = CInt(xdifalt - (xdifalt / 2))
        mset.aktrange.xl = mset.aktrange.xcenter - xdifnew
        mset.aktrange.xh = mset.aktrange.xcenter + xdifnew
        mset.aktrange.yl = mset.aktrange.ycenter - xdifnew
        mset.aktrange.yh = mset.aktrange.ycenter + xdifnew
        refreshMap()
    End Sub

    Private Sub btnminus_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim xdifalt As Double = mset.aktrange.xdif / 2
        Dim xdifnew As Double
        mset.aktrange.CalcCenter()
        xdifnew = CInt(xdifalt * 1.5)
        mset.aktrange.xl = mset.aktrange.xcenter - xdifnew
        mset.aktrange.xh = mset.aktrange.xcenter + xdifnew
        mset.aktrange.yl = mset.aktrange.ycenter - xdifnew
        mset.aktrange.yh = mset.aktrange.ycenter + xdifnew
        refreshMap()
    End Sub

    Private Sub cmbHintergrund_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If cmbHintergrund.SelectedItem Is Nothing Then Exit Sub
        '  Dim myvali$ = CStr(cmbHintergrund.SelectedValue)    Dim tcmb As New ComboBoxItem
        Dim myvalx = CType(cmbHintergrund.SelectedItem, ComboBoxItem)
        hgrund = myvalx.Tag.ToString.Trim
        refreshMap()
    End Sub

    Private Sub btnDeleteRegelkontrolle_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " regelkontrolle ='#1/1/0001 12:00:00 AM#' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnregelkontrol.IsEnabled = False
            tbregelkontrol.Text = ""
        End If
        e.Handled = True
    End Sub

    Private Sub btndeleteUntersucheung_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " untersuchung ='#1/1/0001 12:00:00 AM#' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnuntersuchung.IsEnabled = False
            tbuntersuchung.Text = ""
        End If
        e.Handled = True
    End Sub

    Private Sub btndeleteAblaufKS_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " ablaufdatumks ='#1/1/0001 12:00:00 AM#' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnablaufks.IsEnabled = False
            tbablaufks.Text = ""
        End If
        e.Handled = True
    End Sub

    Private Sub tbauge_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If tbauge.Text <> String.Empty Then btnauge.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnauge_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " auge ='" & tbauge.Text & "' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnauge.IsEnabled = False
        End If
        e.Handled = True
    End Sub

    Private Sub DatePickerauge_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        tbauge.Text = Format(DatePickerauge.SelectedDate, "dd.MM.yyy")
        e.Handled = True
    End Sub

    Private Sub btnDeleteauge_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " auge ='#1/1/0001 12:00:00 AM#' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnauge.IsEnabled = False
            tbauge.Text = ""
        End If
        e.Handled = True
    End Sub

    Private Sub winEditor_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim mesres As New MessageBoxResult
        If anychange() Then
            mesres = MessageBox.Show("Sie haben nicht alle Änderungen gespeichert. Änderungen verwerfen ?", "Vorsicht", MessageBoxButton.YesNo,
                                   MessageBoxImage.Question, MessageBoxResult.No)
            If mesres = MessageBoxResult.Yes Then
                e.Cancel = False
                ' Exit sub
            Else
                e.Cancel = true
                Exit sub
            End If
        End If
    End Sub

    Private Function anychange() As Boolean
        If btnablaufks.IsEnabled Or btnVID.IsEnabled Or btnPlakette.IsEnabled Or
            btnregelkontrol.IsEnabled Or btnauge.IsEnabled Or
            btnuntersuchung.IsEnabled Or btnKronensicherung.IsEnabled Or
            btnBemerkung.IsEnabled Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub btnVerkehrssicher_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If Not clstools.darfAendern(Environment.UserName) Then
            MsgBox("Keine Autorisierung für " & Environment.UserName)
            Exit Sub
        End If
        Dim newid, res As Long
        mset.basisrec.mydb.SQL = "update  paradigma_userdata.ndindividuenedit set " &
                                        " verkehrssicher ='" & CStr(CBool(cbVerkehrssicher.IsChecked)) & "' " &
                                        " where gid=" & aktgid
        res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
        If res = 1 Then
            btnVerkehrssicher.IsEnabled = False
        End If
        e.Handled = True
    End Sub

    Private Sub cbVerkehrssicher_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnVerkehrssicher.IsEnabled = True
        'If cbVerkehrssicher.IsChecked Then
        '    spKSdatum.IsEnabled = True
        'Else
        '    spKSdatum.IsEnabled = True
        'End If
        e.Handled = True
    End Sub

    Private Sub BtnAllgAusweisung_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim pfad As String '= "\\file-office\Office\UMWELT\B\2-neue Struktur\3 - Naturschutz\30 - Rechtsgrundlagen und Allgemeines\300 - Rechtsgrundlagen, Urteile\3002 - Verordnungen, Erlasse, Richtlinien\Naturdenkmale\manager\"
        pfad = "\\gis\gdvell\nkat\aid\161\texte\naturdenkmal.pdf"
        '      Dim datei = gruppenID & ".pdf"
        Process.Start(pfad)
    End Sub
End Class
