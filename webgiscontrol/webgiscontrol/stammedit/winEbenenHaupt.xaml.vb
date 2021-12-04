Imports MicrosoftXceed.Wpf.Toolkit
Imports webgiscontrol

Public Class winEbenenHaupt

    '    update schlagworte
    '    Set aid=doku.aid 
    'from  doku 
    'where  doku.ebene=  schlagworte.featureclass_obs
    Property aktaid As Integer
    Property aktStamm As New clsStamm
    Property neuStamm As New clsStamm
    Property aktMaske As New MaskenObjekt
    Property ladevorgangabgeschlossen As Boolean = False
    Sub New(aid As Integer)
        InitializeComponent()
        aktaid = aid
    End Sub
    Private Sub winEbenenHaupt_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        'stammdatenHolen 
        Try
            dbeditTools.DatenAusStammDarstellen(aktStamm, aktaid)
            dbeditTools.DatenAusSchlagwortDarstellen(aktaid, aktStamm)

            dbeditTools.DatenAusDokuDarstellen(aktaid, aktStamm)
            dbeditTools.DatenausGruppenAID(aktaid, aktStamm)
            dbeditTools.getAnzahlAttributtabellen(aktaid, aktStamm)
            dbeditTools.DatenAusAttributtabelle(aktaid, aktStamm)
            If aktStamm.status = False Then
                Background = Brushes.LightPink
                cbIstaktiv.Background = Brushes.LightPink
                spIstaktiv.Background = Brushes.LightPink
            End If
            refreshdatenTabelle(1)

            refreshVorlagen(1)

            aktStamm.isHintergrund = dbeditTools.getIsHgrund(aktaid, aktStamm)

            initHaupSachgebietAuswahlColl() : cmbSachgebiet.DataContext = sgHauptColl
            initSchemaAuswahlColl() : cmbSchemas0.DataContext = schemaColl

            initVorlagenspaltenanmen(1)

            initCmbRang() : cmbRang.DataContext = rangColl
            If rangColl.Count < 1 Then
                MsgBox("rangColl.Count < 1")
            End If
            Title = "Ebene " & aktaid & " " & aktStamm.titel & "  " & rangColl.Count
            dbeditTools.getAnzahlAttributtabellen(aktaid, aktStamm)
            showTexte()
            showAttributtabellen(aktStamm)
            calcPfad()
            '  tiTabellenDef.IsSelected = True

            initTabellenAuswahlColl(tbSchema0.Text) : cmbtabelle0.DataContext = schematabellenColl
            initTABIDAuswahlColl(tbtabelle0.Text, tbSchema0.Text, tabIDColl) : cmbtabid0.DataContext = tabIDColl
            cmbLINKTabs0.DataContext = tabIDColl
            ladevorgangabgeschlossen = True
        Catch ex As Exception
            l("fehler in winEbenenHaupt_Loaded " & ex.ToString)
        End Try
    End Sub

    Private Sub initVorlagenspaltenanmen(tabnr As Integer)
        Dim a As String
        Dim vortab As New List(Of clsSachgebietsCombo)
        Try
            If tabnr < 1 Then
                Exit Sub
            End If
            'tabellenenamen festellen
            a = aktStamm.tabellenListen(tabnr - 1).tabelle
            ' dann initTABIDAuswahlColl
            vortab = New List(Of clsSachgebietsCombo)
            initTABIDAuswahlColl(aktStamm.tabellenListen(tabnr - 1).tabelle, aktStamm.tabellenListen(tabnr - 1).Schema, vortab)
            cmbVorlagenFeldname.DataContext = vortab
            cmbVorlagenTemplate.DataContext = vortab
        Catch ex As Exception
            l("fehler ininitVorlagenspaltenanmen " & ex.ToString)
        End Try
    End Sub

    Private Sub refreshdatenTabelle(tab_nr As Integer)
        'tab_nr = tab_nr - 1
        dbeditTools.DatentabelleAnezigen(aktaid, aktStamm, tab_nr)
        dgDatentabelle.DataContext = wgisdt
    End Sub

    Private Sub refreshVorlagen(tab_nr As Integer)
        '  tab_nr -= 1
        dbeditTools.vorlagenDT(aktaid, aktStamm, tab_nr)
        aktStamm.vorlagenListe = dbeditTools.vorlagenDT2list(aktaid, aktStamm)
        dgVorlagen.DataContext = aktStamm.vorlagenListe
    End Sub

    Private Sub showAttributtabellen(aktStamm As clsStamm)
        Try
            Dim I As Integer = 0
            If aktStamm.tabellenListen.Count > 0 Then
                tbDBtab_nr0.Text = aktStamm.tabellenListen(I).tab_nr
                tbSchema0.Text = aktStamm.tabellenListen(I).Schema
                tbtabelle0.Text = aktStamm.tabellenListen(I).tabelle
                tbtab_ID0.Text = aktStamm.tabellenListen(I).tab_id
                tbtabtitel0.Text = aktStamm.tabellenListen(I).tabtitel
                tbTabellenAnzeige0.Text = aktStamm.tabellenListen(I).tabellen_anzeige
                tbtabinterneID.Text = aktStamm.tabellenListen(I).id.ToString
                tbLinkspalten0.Text = aktStamm.tabellenListen(I).linkTabs
                gb1.Visibility = Visibility.Visible
            Else
                ' tbtabinterneID.Text = "1"
                gb1.Visibility = Visibility.Collapsed
                gb2.Visibility = Visibility.Collapsed
                gb3.Visibility = Visibility.Collapsed

            End If
            If aktStamm.tabellenListen.Count > 1 Then
                I = 1
                tbDBtab_nr1.Text = aktStamm.tabellenListen(I).tab_nr
                tbSchema1.Text = aktStamm.tabellenListen(I).Schema
                tbtabelle1.Text = aktStamm.tabellenListen(I).tabelle
                tbtabID1.Text = aktStamm.tabellenListen(I).tab_id
                tbtabtitel1.Text = aktStamm.tabellenListen(I).tabtitel
                tbTabellenAnzeige1.Text = aktStamm.tabellenListen(I).tabellen_anzeige
                gb1.Visibility = Visibility.Visible
                gb2.Visibility = Visibility.Visible
            Else
                gb2.Visibility = Visibility.Collapsed
                gb3.Visibility = Visibility.Collapsed
            End If
            If aktStamm.tabellenListen.Count > 2 Then
                I = 2
                tbDBtab_nr2.Text = aktStamm.tabellenListen(I).tab_nr
                tbSchema2.Text = aktStamm.tabellenListen(I).Schema
                tbtabelle2.Text = aktStamm.tabellenListen(I).tabelle
                tbtabID2.Text = aktStamm.tabellenListen(I).tab_id
                tbtabtitel2.Text = aktStamm.tabellenListen(I).tabtitel
                tbTabellenAnzeige2.Text = aktStamm.tabellenListen(I).tabellen_anzeige
                gb1.Visibility = Visibility.Visible
                gb2.Visibility = Visibility.Visible
                gb3.Visibility = Visibility.Visible
            Else
                gb3.Visibility = Visibility.Collapsed
            End If


        Catch ex As Exception
            l("fehler in showAttributtabellen " & ex.ToString)
        End Try
    End Sub

    Private Sub showTexte()
        tbmassstab_imap.Text = CType(aktStamm.masstab_imap, String)
        cbMitLegende.IsChecked = aktStamm.mit_legende
        cbIstaktiv.IsChecked = aktStamm.status
        cbMitObjekten.IsChecked = aktStamm.mit_objekten

        cbMitImap.IsChecked = aktStamm.mit_imap
        tbHauptSachgebiet.Text = aktStamm.sachgebiet
        tbPfadView.Text = aktStamm.pfad
        tbrang.Text = CType(aktStamm.rang, String)
        tbtitel.Text = aktStamm.titel
        tbEbene.Text = aktStamm.ebene
        tbSchlag.Text = aktStamm.schlagworte
        tbaid.Text = "Aid: " & aktaid & " " & tbtitel.Text '& ", " & dtstamm.Rows(0).Item("titel").ToString
        tAnzahlAttributtabellen.Text = CType(aktStamm.anzahl_attributtabellen, String)
        tAnzahlAttributtabellenReal.Text = CType(aktStamm.AnzahlAttributtabellenReal, String)
        cbIsthgrund.IsChecked = aktStamm.isHintergrund

        'doku
        tbInternes.Text = aktStamm.aktDoku.internes
        tbINhalt.Text = aktStamm.aktDoku.inhalt
        tbEntstehung.Text = aktStamm.aktDoku.entstehung
        tbBeschraenk.Text = aktStamm.aktDoku.beschraenkungen
        tbMassstabDoku.Text = aktStamm.aktDoku.masstab
        tbAktualisiert.Text = aktStamm.aktDoku.aktualitaet
        tbDatenabgabe.Text = aktStamm.aktDoku.datenabgabe

        'gruppe2aid
        cbGruppeBauaufsichtt.IsChecked = aktStamm.gruppen.bauaufsicht
        cbGruppeInternet.IsChecked = aktStamm.gruppen.internet
        cbGruppeIntranet.IsChecked = aktStamm.gruppen.intranet
        cbGruppeSicherheit.IsChecked = aktStamm.gruppen.sicherheit
        cbGruppeUmwelt.IsChecked = aktStamm.gruppen.umwelt
    End Sub



    Private Sub cmbRang_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)


        Dim sgitem As New clsSachgebietsCombo
        If cmbRang.SelectedItem Is Nothing Then Exit Sub
        If Not ladevorgangabgeschlossen Then Exit Sub
        sgitem = CType(cmbRang.SelectedItem, clsSachgebietsCombo)
        tbrang.Text = sgitem.sid
        btnRangschreiben.IsEnabled = True
        'If sgitem.sid = String.Empty Then
        '    initAuswahlliste("") : dgEbenen.DataContext = wgisdt
        'Else
        '    initAuswahlliste(sgitem.sid) : dgEbenen.DataContext = wgisdt
        'End If
        e.Handled = True
    End Sub



    Private Sub cmbSachgebiet_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim sgitem As New clsSachgebietsCombo
        If cmbSachgebiet.SelectedItem Is Nothing Then Exit Sub
        sgitem = CType(cmbSachgebiet.SelectedItem, clsSachgebietsCombo)
        'If sgitem.sid = String.Empty Then
        '    initAuswahlliste("")
        'Else
        '    initAuswahlliste(sgitem.sid)
        'End If
        calcPfad()
        neuStamm.aid = aktStamm.aid
        neuStamm.titel = aktStamm.titel
        neuStamm.sachgebiet = sgitem.sachgebiet
        neuStamm.sid = CInt(sgitem.sid)
        calcPfad()
        tbHauptSachgebiet.Text = sgitem.sachgebiet
        btnHauptSachgeietschreiben.IsEnabled = True
        e.Handled = True
    End Sub



    Private Sub tbtitel_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbtitel.TextChanged
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnTitelschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnSchlagschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "schlagworte", "schlagworte", tbSchlag.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnSchlagschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub tbSchlag_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbSchlag.TextChanged
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnSchlagschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnEbeneschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "stamm", "ebene", tbEbene.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnEbeneschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If

        e.Handled = True
    End Sub

    Private Sub btnRangschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "stamm", "rang", tbrang.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnRangschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub tbSchlag_TextChanged_1(sender As Object, e As TextChangedEventArgs)

    End Sub

    Private Sub tbEbene_TextChanged(sender As Object, e As TextChangedEventArgs)
        calcPfad()
        If Not ladevorgangabgeschlossen Then Exit Sub

        btnEbeneschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub calcPfad()
        tbPfad.Text = "/fkat/" & aktStamm.sachgebiet & "/" & aktStamm.ebene & "/"
    End Sub

    Private Sub cbMitImap_Checked(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnMitImapschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnMitImapschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim wert As Boolean
        wert = CBool(cbMitImap.IsChecked)
        If dbeditTools.datenUebernehmen("AID", aktaid, "stamm", "mit_imap", wert.ToString, "boolean", tools.dbServername, "webgiscontrol") Then
            btnMitImapschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub



    Private Sub tbmassstab_imap_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnmassstab_imapeschreiben.IsEnabled = True
        e.Handled = True
    End Sub
    Private Sub btnmassstab_imapeschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "stamm", "masstab_imap", tbmassstab_imap.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnmassstab_imapeschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cbMitLegende_Checked(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnMitLegendeschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnMitLegendeschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim wert As Boolean
        wert = CBool(cbMitLegende.IsChecked)
        If dbeditTools.datenUebernehmen("AID", aktaid, "stamm", "mit_legende", wert.ToString, "boolean", tools.dbServername, "webgiscontrol") Then
            btnMitLegendeschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cbIstaktiv_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnIstaktivschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnIstaktivschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim wert As Boolean
        wert = CBool(cbIstaktiv.IsChecked)
        If dbeditTools.datenUebernehmen("AID", aktaid, "stamm", "status", wert.ToString, "boolean", tools.dbServername, "webgiscontrol") Then
            btnIstaktivschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cbIsthgrund_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnIsthgrundchreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnIsthgrundchreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        'MsgBox("baustelle")
        'die aid in der hgrundtab einfügen
        Dim ishintergrundNeu As Boolean
        Dim ishintergrundalt As Boolean
        ishintergrundalt = dbeditTools.getIsHgrund(aktaid, aktStamm)
        ishintergrundNeu = CBool(cbIsthgrund.IsChecked)
        If ishintergrundalt = ishintergrundNeu Then
            'keine änderung nötig
            btnIsthgrundchreiben.IsEnabled = False
            Exit Sub
        End If
        If ishintergrundNeu = True Then
            If ishintergrundalt = False Then
                'aid in hrungtab ergänzen
                dbeditTools.HintergrundAktionAid(aktaid, "add")
            End If
        Else
            ishintergrundNeu = False
            If ishintergrundalt = True Then
                'aid in hrungtab entfernen
                dbeditTools.HintergrundAktionAid(aktaid, "delete")
            End If
        End If
        btnIsthgrundchreiben.IsEnabled = False
        'If dbeditTools.datenUebernehmen(aktaid, "stamm", "mit_imap", wert.ToString, "boolean") Then
        '    btnIsthgrundchreiben.IsEnabled = False
        'Else
        '    MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        'End If
        e.Handled = True
    End Sub

    Private Sub btnHauptSachgeietschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        'alteneintrag löschen
        ' nein, update reicht
        'Dim neueid As Integer
        'neueid = dbeditTools.zeileLoeschen("public.ebenen_sachgebiete", "aid=" & aktaid & " and sid=" & aktStamm.sid)

        '    If dbeditTools.sachgebietUpdaten(aktaid, "ebenen_sachgebiete", "sid", CType(neuStamm.sid, String), CType(aktStamm.sid, String)) Then
        If dbeditTools.datenUebernehmen("AID", aktaid, "stamm", "sid", neuStamm.sid.ToString, "integer", tools.dbServername, "webgiscontrol") Then
            btnTitelschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If

        btnHauptSachgeietschreiben.IsEnabled = False
        e.Handled = True
    End Sub
    Private Sub btnTabelleAdd_Click(sender As Object, e As RoutedEventArgs)
        Dim NeueTabNr As Integer
        NeueTabNr = dbeditTools.getMaxTabnr(aktaid)
        NeueTabNr += 1
        Dim interne_id As Integer = createNewAttributeTable(aktaid, NeueTabNr)
        e.Handled = True
    End Sub
    Private Sub btnTab0deleten_Click(sender As Object, e As RoutedEventArgs)
        MsgBox("baustelle")
        e.Handled = True
    End Sub

    Private Sub cmbSchemas0_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim schemaitem As New clsSchema
        If cmbSchemas0.SelectedItem Is Nothing Then Exit Sub
        schemaitem = CType(cmbSchemas0.SelectedItem, clsSchema)
        tbSchema0.Text = schemaitem.schemaname
        btnSchemas0schreiben.IsEnabled = True
        initTabellenAuswahlColl(schemaitem.schemaname) : cmbtabelle0.DataContext = schematabellenColl
        cmbtabelle0.IsDropDownOpen = True
        e.Handled = True
    End Sub

    Private Sub btnSchemas0schreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Try


            If dbeditTools.datenUebernehmen("ID", CInt(tbtabinterneID.Text), "attributtabellen", "schema", tbSchema0.Text, "string", tools.dbServername, "webgiscontrol") Then
                btnSchemas0schreiben.IsEnabled = False
            Else
                MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
            End If
            e.Handled = True
        Catch ex As Exception
            l("fehler in btnSchemas0schreiben_Click: " & ex.ToString)
        End Try
    End Sub

    Private Sub cmbtabelle0_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim tabitem As New clsSchemaTabelle
        If cmbtabelle0.SelectedItem Is Nothing Then Exit Sub
        tabitem = CType(cmbtabelle0.SelectedItem, clsSchemaTabelle)
        tbtabelle0.Text = tabitem.tabellenname
        btntabelle0schreiben.IsEnabled = True

        initTABIDAuswahlColl(tabitem.tabellenname, tbSchema0.Text, tabIDColl)
        cmbtabid0.DataContext = tabIDColl : cmbLINKTabs0.DataContext = tabIDColl
        cmbtabelle0.IsDropDownOpen = True
        e.Handled = True
    End Sub



    Private Sub btntabelle0schreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", CInt(tbtabinterneID.Text), "attributtabellen", "tabelle", tbtabelle0.Text, "string",
                                        tools.dbServername, "webgiscontrol") Then
            btntabelle0schreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cmbtabid0_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim tabitem As New clsSachgebietsCombo
        If cmbtabid0.SelectedItem Is Nothing Then Exit Sub
        tabitem = CType(cmbtabid0.SelectedItem, clsSachgebietsCombo)
        tbtab_ID0.Text = tabitem.sachgebiet
        btntabid0schreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btntabid0schreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", CInt(tbtabinterneID.Text), "attributtabellen", "tab_id", tbtab_ID0.Text, "string",
                                        tools.dbServername, "webgiscontrol") Then
            btntabid0schreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cmbLINKTabs0_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim tabitem As New clsSachgebietsCombo
        If cmbLINKTabs0.SelectedItem Is Nothing Then Exit Sub

        tabitem = CType(cmbLINKTabs0.SelectedItem, clsSachgebietsCombo)
        If tabitem.sachgebiet = String.Empty Then Exit Sub
        tbLinkspalten0.Text = tbLinkspalten0.Text & "," & tabitem.sachgebiet
        ' btntabid0schreiben.IsEnabled = True

        'initTABIDAuswahlColl(tabitem.sachgebiet, tbSchema0.Text)
        'cmbtabid0.DataContext = tabIDColl
        'cmbtabelle0.IsDropDownOpen = True
        cmbLINKTabs0.SelectedIndex = 0
        btnLINKTabs0schreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub dgEbenen_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnInhaltschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "doku", "inhalt", tbINhalt.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnInhaltschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub tbINhalt_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnInhaltschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub tbEntstehung_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnEntstehungschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub tbAktualisiert_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnAktualisiertschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub tbMassstabDoku_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnMassstabschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub tbBeschraenk_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnBeschraenkschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnEntstehungschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "doku", "entstehung", tbEntstehung.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnEntstehungschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnAktualisiertschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "doku", "aktualitaet", tbAktualisiert.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnAktualisiertschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnMassstabschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "doku", "masstab", tbMassstabDoku.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnMassstabschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnBeschraenkschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "doku", "beschraenkungen", tbBeschraenk.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnBeschraenkschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnDatenabgabeschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "doku", "datenabgabe", tbDatenabgabe.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnDatenabgabeschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub tbDatenabgabe_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnDatenabgabeschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub dgSG_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If dgVorlagen.SelectedItem Is Nothing Then Exit Sub
            aktMaske = CType(dgVorlagen.SelectedItem, MaskenObjekt)
            tbvorlagenID.Text = (aktMaske.id.ToString)
            tbvorlagenNr.Text = (aktMaske.nr.ToString)
            tbvorlagenTabNr.Text = (aktMaske.tab_nr.ToString)
            tbvorlagenTemplate.Text = (aktMaske.template.ToString)
            tbvorlagenTitel.Text = (aktMaske.titel.ToString)
            tbvorlagenTyp.Text = (aktMaske.typ.ToString)
            tbvorlagenFeldname.Text = (aktMaske.feldname.ToString)
            tbvorlagenCssClass.Text = (aktMaske.cssclass.ToString)
            tbvorlagenAnwendung.Text = (aktMaske.anwendung.ToString)


            'dbeditTools.getFeldnamen(aktaid, aktStamm)
            'Dim sql = " select column_name from information_schema.columns where table_schema='" & schemaname &
            '    "' and table_name='" & tabellenname & "' "
            btnvorlagenTemplateschreiben.IsEnabled = False
            btnvorlagenNrschreiben.IsEnabled = False
            btnvorlagenTitelschreiben.IsEnabled = False
            btnvorlagenTypschreiben.IsEnabled = False
            btnvorlagenFeldnameschreiben.IsEnabled = False
            btnvorlagenCssClassschreiben.IsEnabled = False

            e.Handled = True
        Catch ex As Exception
            l("dgEbenen_SelectionChanged " & ex.ToString)
        End Try
    End Sub



    Private Sub btnremove_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub btnNeuerTitel_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub btnRefreshVorlagen_Click(sender As Object, e As RoutedEventArgs)
        refreshVorlagen(CInt(tbvorlagenTabnrEingabe.Text))
        initVorlagenspaltenanmen(CInt(tbvorlagenTabnrEingabe.Text))
        e.Handled = True
    End Sub

    Private Sub btnvorlagenIDschreiben_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnvorlagenNrschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", aktMaske.id, "tabellenvorlagen", "nr", tbvorlagenNr.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnvorlagenNrschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnvorlagenFeldnameschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", aktMaske.id, "tabellenvorlagen", "feldname", tbvorlagenFeldname.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnvorlagenFeldnameschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnvorlagenTitelschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", aktMaske.id, "tabellenvorlagen", "titel", tbvorlagenTitel.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnvorlagenTitelschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnvorlagenTypschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", aktMaske.id, "tabellenvorlagen", "typ", tbvorlagenTyp.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnvorlagenTypschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cmbVorlagenFeldname_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim tabitem As New clsSachgebietsCombo
        If cmbVorlagenFeldname.SelectedItem Is Nothing Then Exit Sub
        tabitem = CType(cmbVorlagenFeldname.SelectedItem, clsSachgebietsCombo)
        tbvorlagenFeldname.Text = tabitem.sachgebiet
        'btntabid0schreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub cmbVorlagenTyp_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If cmbVorlagenTyp.SelectedItem Is Nothing Then Exit Sub
        Dim item As ComboBoxItem = CType(cmbVorlagenTyp.SelectedItem, ComboBoxItem)
        tbvorlagenTyp.Text = CType(item.Tag, String)
        btnvorlagenTypschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub cmbVorlagenCssClass_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If cmbVorlagenCssClass.SelectedItem Is Nothing Then Exit Sub
        Dim item As ComboBoxItem = CType(cmbVorlagenCssClass.SelectedItem, ComboBoxItem)
        tbvorlagenCssClass.Text = CType(item.Tag, String)
        btnvorlagenCssClassschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnvorlagenCssClassschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", aktMaske.id, "tabellenvorlagen", "cssclass", tbvorlagenCssClass.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnvorlagenCssClassschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cmbVorlagenTemplate_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim tabitem As New clsSachgebietsCombo
        If cmbVorlagenTemplate.SelectedItem Is Nothing Then Exit Sub
        tabitem = CType(cmbVorlagenTemplate.SelectedItem, clsSachgebietsCombo)
        tbvorlagenTemplate.Text = tbvorlagenTemplate.Text & " [" & tabitem.sachgebiet & "] "
        'btntabid0schreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnvorlagenTemplateschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", aktMaske.id, "tabellenvorlagen", "template", tbvorlagenTemplate.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnvorlagenTemplateschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnDatenTabelleRefresh_Click(sender As Object, e As RoutedEventArgs)
        refreshdatenTabelle(CInt(tbDatentabelleTabnrEingabe.Text))
        e.Handled = True
    End Sub

    Private Sub tbvorlagenNr_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnvorlagenNrschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub tbvorlagenFeldname_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnvorlagenFeldnameschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub tbvorlagenTitel_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnvorlagenTitelschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub tbvorlagenTyp_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnvorlagenTypschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub tbvorlagenCssClass_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnvorlagenCssClassschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub tbvorlagenTemplate_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnvorlagenTemplateschreiben.IsEnabled = True
        e.Handled = True
    End Sub


    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs)
        dbeditTools.TabVorlageEinfuegen(aktaid, tbvorlagenTabnrEingabe.Text)
        refreshVorlagen(CInt(tbvorlagenTabnrEingabe.Text))
        initVorlagenspaltenanmen(CInt(tbvorlagenTabnrEingabe.Text))
        e.Handled = True
    End Sub

    Private Sub btnVorlageremove_Click(sender As Object, e As RoutedEventArgs)
        dbeditTools.TabVorlageloeschen(aktaid, aktMaske.id)
        refreshVorlagen(CInt(tbvorlagenTabnrEingabe.Text))
        initVorlagenspaltenanmen(CInt(tbvorlagenTabnrEingabe.Text))
        e.Handled = True
    End Sub

    Private Sub btnGruppeUmweltSchreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub

        e.Handled = True
    End Sub

    Private Sub btnGruppeInternetSchreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim wert As Boolean
        wert = CBool(cbGruppeInternet.IsChecked)
        If dbeditTools.datenUebernehmen("AID", aktaid, "gruppe2aid", "internet", wert.ToString, "boolean", tools.dbServername, "webgiscontrol") Then
            btnGruppeInternetSchreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnGruppeIntranetSchreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim wert As Boolean
        wert = CBool(cbGruppeIntranet.IsChecked)
        If dbeditTools.datenUebernehmen("AID", aktaid, "gruppe2aid", "intranet", wert.ToString, "boolean", tools.dbServername, "webgiscontrol") Then
            btnGruppeIntranetSchreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnGruppeSicherheitSchreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim wert As Boolean
        wert = CBool(cbGruppeSicherheit.IsChecked)
        If dbeditTools.datenUebernehmen("AID", aktaid, "gruppe2aid", "sicherheit", wert.ToString, "boolean", tools.dbServername, "webgiscontrol") Then
            btnGruppeSicherheitSchreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnGruppeBauaufsichtSchreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim wert As Boolean
        wert = CBool(cbGruppeBauaufsichtt.IsChecked)
        If dbeditTools.datenUebernehmen("AID", aktaid, "gruppe2aid", "bauaufsicht", wert.ToString, "boolean", tools.dbServername, "webgiscontrol") Then
            btnGruppeBauaufsichtSchreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnGruppeUmweltSchreiben_Click_1(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim wert As Boolean
        wert = CBool(cbGruppeUmwelt.IsChecked)
        If dbeditTools.datenUebernehmen("AID", aktaid, "gruppe2aid", "umwelt", wert.ToString, "boolean", tools.dbServername, "webgiscontrol") Then
            btnGruppeUmweltSchreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cbGruppeInternet_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnGruppeInternetSchreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub cbGruppeIntranet_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnGruppeIntranetSchreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub cbGruppeUmwelt_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnGruppeUmweltSchreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub cbGruppeSicherheit_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnGruppeSicherheitSchreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub cbGruppeBauaufsichtt_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnGruppeBauaufsichtSchreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnIstmitObjekten_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim wert As Boolean
        wert = CBool(cbMitObjekten.IsChecked)
        If dbeditTools.datenUebernehmen("AID", aktaid, "stamm", "mit_objekten", wert.ToString, "boolean", tools.dbServername, "webgiscontrol") Then
            btnIstmitObjekten.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cbMitObjekten_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnIstmitObjekten.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btntabtitel0schreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        'tbtabID0
        If dbeditTools.datenUebernehmen("ID", CInt(tbtabinterneID.Text), "attributtabellen", "tab_titel", tbtabtitel0.Text, "string",
                                        tools.dbServername, "webgiscontrol") Then
            btntabtitel0schreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub tbtabtitel0_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btntabtitel0schreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnTitelschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("AID", aktaid, "stamm", "titel", tbtitel.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnTitelschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub cmbTabellen_Anzeige0_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim item As ComboBoxItem = CType(cmbTabellen_Anzeige0.SelectedItem, ComboBoxItem)

        'Dim tabitem As New clsSachgebietsCombo
        'If cmbTabellen_Anzeige0.SelectedItem Is Nothing Then Exit Sub

        tbTabellenAnzeige0.Text = CType(item.Tag, String)
        btnTabellenAnzeige0schreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub btnTabellenAnzeige0schreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", CInt(tbtabinterneID.Text), "attributtabellen", "tabellen_anzeige", tbTabellenAnzeige0.Text, "string",
                                        tools.dbServername, "webgiscontrol") Then
            btnTabellenAnzeige0schreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub btnLINKTabs0schreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", CInt(tbtabinterneID.Text), "attributtabellen", "linktabs", tbLinkspalten0.Text, "string",
                                        tools.dbServername, "webgiscontrol") Then
            btnLINKTabs0schreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub BtnInternesschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        '    If dbeditTools.datenUebernehmen("AID", aktaid, "doku", "inhalt", tbINhalt.Text, "string", tools.dbServername, "webgiscontrol") Then
        If dbeditTools.datenUebernehmen("AID", aktaid, "doku", "internes", tbInternes.Text, "string", tools.dbServername, "webgiscontrol") Then
            'If dbeditTools.datenUebernehmen("internes", CInt(tbInternes.Text), "doku", "internes", tbInternes.Text, "string",
            '                            tools.dbServername, "webgiscontrol") Then
            btnInternesschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub

    Private Sub tbInternes_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnInternesschreiben.IsEnabled = True
        e.Handled = True
    End Sub

    Private Sub TbvorlagenAnwendung_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnvorlagenAnwendungschreiben.IsEnabled = True
        e.Handled = True
    End Sub



    Private Sub CmbVorlagenAnwendung_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If cmbVorlagenAnwendung.SelectedItem Is Nothing Then Exit Sub
        Dim item As ComboBoxItem = CType(cmbVorlagenAnwendung.SelectedItem, ComboBoxItem)
        tbvorlagenAnwendung.Text = CType(item.Tag, String)
        btnvorlagenAnwendungschreiben.IsEnabled = True
        e.Handled = True
    End Sub
    Private Sub BtnvorlagenAnwendungschreiben_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dbeditTools.datenUebernehmen("ID", aktMaske.id, "tabellenvorlagen", "anwendung", tbvorlagenAnwendung.Text, "string", tools.dbServername, "webgiscontrol") Then
            btnvorlagenAnwendungschreiben.IsEnabled = False
        Else
            MsgBox("o oooooooooooooooooo Daten konnten nicht geschrieben werden !")
        End If
        e.Handled = True
    End Sub
End Class

