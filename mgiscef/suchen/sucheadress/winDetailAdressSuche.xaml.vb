Imports System.Data
Public Class winDetailAdressSuche
    'Public gemeinde_verz As String = serverUNC & "apps\test\mgis\combos\gemeinden.xml"
    'Public funktionen_verz As String = serverUNC & "apps\test\mgis\combos\RBfunktion.xml"


    Public EigentuemerPDF As String
    Public Property retunrvalue As Boolean = False
    Property summentext As String = "" ' wird hier ignoriert
    Property strasseOhneHausnr As Boolean = False

    Public Property strassenListe As New List(Of clsFlurauswahl)

    Private Sub winDetailAdressSuche_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        initGemeindeCombo()
        lvGemarkungen.ItemsSource = initgemeindeListe()
        'cmbgemeinde.IsDropDownOpen = True
        '  aktadr.clear()
        cmbgemeinde.Visibility = Visibility.Collapsed
        'cmbStrasse.Visibility = Visibility.Collapsed
        'cmbHausnr.Visibility = Visibility.Collapsed

        spNachnenner.Visibility = Visibility.Collapsed
        lvStrassen.Visibility = Visibility.Collapsed

        spAlleBuchstaben.Visibility = Visibility.Collapsed
        lvHausnr.Visibility = Visibility.Collapsed


        tbWeitergabeVerbot.Text = albverbotsString
        spEigentNotizUebernehmen.IsEnabled = False
        If GisUser.istalbberechtigt Then
            gbEigentuemer.Visibility = Visibility.Visible
            gbEigentuemer.IsEnabled = True
        Else
            gbEigentuemer.Visibility = Visibility.Collapsed
            gbEigentuemer.IsEnabled = False
        End If
        tbFSTINFO.Visibility = Visibility.Collapsed
        setzeGrundFuerEigentuemerabfrage(tbGrund.Text)
        Top = 50
        'Left = 500
        If STARTUP_mgismodus = "paradigma" Then
            gbFSTaradigma.Visibility = Visibility.Visible
            spEigentNotizUebernehmen.Visibility = Visibility.Visible
        Else
            spEigentNotizUebernehmen.IsEnabled = False
            gbFSTaradigma.Visibility = Visibility.Collapsed
            spEigentNotizUebernehmen.Visibility = Visibility.Collapsed
        End If

        If Not aktadr.Gisadresse.gemeindebigNRstring.IsNothingOrEmpty Then
            tbGemeinde.Text = clsString.Capitalize(aktadr.Gisadresse.gemeindeName)
            tbStrasse.Text = aktadr.Gisadresse.strasseName
            tbHausnr.Text = aktadr.Gisadresse.HausKombi
            initStrassenCombo()
            aktadr.Gisadresse.strasseName = tbStrasse.Text
            'cmbStrasse.DataContext = adrREC.dt
            clsADRtools.inithausnrComboDT()
            'cmbHausnr.DataContext = adrREC.dt
            aktadr.Gisadresse.HausKombi = tbHausnr.Text
            retunrvalue = False
        Else
            'cmbgemarkung.IsDropDownOpen = True
        End If
        e.Handled = True
    End Sub

    Shared Function initgemeindeListe() As List(Of clsFlurauswahl)
        Dim a(13) As String
        Try
            l(" MOD ---------------------- anfang")
            a(1) = "438001;Dietzenbach"
            a(2) = "438002;Dreieich"
            a(3) = "438003;Egelsbach"
            a(4) = "438004;Hainburg"
            a(5) = "438005;Heusenstamm"
            a(6) = "438006;Langen"
            a(7) = "438007;Mainhausen"
            a(8) = "438008;Mühlheim"
            a(9) = "438009;Neu-Isenburg"
            a(10) = "438010;Obertshausen"
            a(11) = "438011;Rodgau"
            a(12) = "438012;Rödermark"
            a(13) = "438013;Seligenstadt"
            Dim b() As String
            Dim gemListe As New List(Of clsFlurauswahl)
            Dim temp As New clsFlurauswahl
            For i = 1 To a.Length - 1
                b = a(i).Split(";"c)
                temp = New clsFlurauswahl
                temp.id = CInt(b(0))
                temp.displayText = (b(1))
                gemListe.Add(temp)
            Next
            Return gemListe
            l(" MOD ---------------------- ende")
        Catch ex As Exception
            l("Fehler in initgemeindeListe: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Sub btnEigentuemerNachParadigma_Click(sender As Object, e As RoutedEventArgs)
        If modParadigma.DokNachParadigma(EigentuemerPDF, aktvorgangsid, "Eigentümer: ") Then
            MsgBox("Die Übernahme des Dokumentes nach Paradigma war erfolgreich!")
        Else
            MsgBox("Die Übernahme des Dokumentes nach Paradigma war NICHT erfolgreich!")
        End If
        Close()
    End Sub
    Sub initGemeindeCombo()
        Dim existing As XmlDataProvider = TryCast(Me.Resources("XMLSourceComboBoxgemeinden"), XmlDataProvider)
        existing.Source = New Uri(myglobalz.gemeinde_verz)
        existing = TryCast(Me.Resources("XMLSourceComboBoxRBfunktion"), XmlDataProvider)
        existing.Source = New Uri(myglobalz.Paradigma_funktionen_verz)
    End Sub
    Private Sub cmbGemeinde_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        e.Handled = True
        If cmbgemeinde.SelectedItem Is Nothing Then Exit Sub
        Dim myvalx = CType(cmbgemeinde.SelectedItem, System.Xml.XmlElement)
        Dim gemtext As String = myvalx.Attributes(1).Value.ToString
        Dim gemNr As String = CStr(cmbgemeinde.SelectedValue)
        lvHausnr.ItemsSource = Nothing
        showStrassen(gemtext, gemNr)
        tbfilter.Text = ""
        'cmbStrasse.IsDropDownOpen = True
    End Sub

    Private Sub showStrassen(gemtext As String, gemNr As String)
        'Dim strassenListe As New List(Of clsFlurauswahl)
        l("showStrassen")
        aktadr.Gisadresse.gemeindebigNRstring = gemNr
        aktadr.Gisadresse.gemeindeName = gemtext
        tbGemeinde.Text = clsString.Capitalize(aktadr.Gisadresse.gemeindeName)
        strassenListe = Nothing
        If iminternet Or myglobalz.CGIstattDBzugriff Then

            l("showStrassen internet")
            Dim hinweis As String = ""
            strassenListe = clsADRtools.getStrassenlisteFromHTTP(aktadr.Gisadresse.gemeindebigNRstring, hinweis)
        Else
            l("showStrassen intranet")
            initStrassenCombo()
            'cmbStrasse.DataContext = adrREC.dt
            strassenListe = clsADRtools.initStrassenliste(adrREC.dt)
        End If
        lvStrassen.ItemsSource = strassenListe
    End Sub



    Public Sub initStrassenCombo()
        ' Dim a = "SELECT lage,bezeichnung,gml_id FROM halosort.lageschluessel where schluesselgesamt like ""06438001%"" order by bezeichnung"
        'Dim schluesssellike As String
        'schluesssellike = "06" & aktadr.Gisadresse.gemeindebigNRstring
        'adrREC.mydb.SQL =
        ' "SELECT schluesselgesamt,lage,bezeichnung,gml_id FROM public.lageschluessel " &
        ' " where schluesselgesamt like '" & schluesssellike & "%'" &
        ' " order by bezeichnung  "
        'If buchstabe.IsNothingOrEmpty Then
        adrREC.mydb.SQL =
                     "SELECT distinct trim(sname) as sname,strcode,mitadr   FROM flurkarte.haloschneise " &
                     " where gemeindenr  = " & aktadr.Gisadresse.gemeindebigNRstring & "" &
                     " order by  (sname),mitadr  desc"
        'Else
        '    adrREC.mydb.SQL = "SELECT distinct trim(sname) as sname,strcode,mitadr  FROM public.halofsplus " &
        '             " where gemeindenr  = " & aktadr.Gisadresse.gemeindebigNRstring & "" &
        '             " and lower(sname) like '" & buchstabe.ToLower & "%' " &
        '             " order by  (sname) ,mitadr desc "
        'End If
        '  myGlobalz.adrREC.mydb.Schema = "halosort"
        Dim hinweis As String = adrREC.getDataDT()
    End Sub



    Private Function showHausnr(sname As String, scode As Integer) As MessageBoxResult
        Dim mesresulkt As New MessageBoxResult
        mesresulkt = MessageBoxResult.OK
        Dim hinweis As String = ""
        Try
            l(" showHausnr ---------------------- anfang")
            aktadr.Gisadresse.strasseName = sname
            aktadr.Gisadresse.strasseCode = scode
            tbStrasse.Text = aktadr.Gisadresse.strasseName
            Dim hausnrListe As New List(Of clsFlurauswahl)

            If iminternet Or CGIstattDBzugriff Then
                hausnrListe = clsADRtools.getHausnrlisteFromHTTP(aktadr.Gisadresse.gemeindebigNRstring, aktadr.Gisadresse.strasseCode, hinweis)
            Else
                clsADRtools.inithausnrComboDT()
                hausnrListe = clsADRtools.initHausnrliste(adrREC.dt)
            End If
            lvHausnr.ItemsSource = hausnrListe
            If hausnrListe Is Nothing OrElse hausnrListe.Count < 1 Then
                strasseOhneHausnr = True
                Dim infotext As String
                Dim mesresult As New MessageBoxResult
                infotext = "Hinweis: Es konnten keine Hausnummern zu dieser Straße gefunden werden." & Environment.NewLine &
                    "Entweder es handelt sich um ein Gewann / Flurbezeichnung, oder " & Environment.NewLine &
                    "es gibt keine amtl. registrierten Adressen in der Straße." & Environment.NewLine &
                    "" & Environment.NewLine &
                    "" & Environment.NewLine
                mesresult = CType(MessageBox.Show(infotext, "Keine Hausnummern gefunden zu: " & sname), MessageBoxResult)

                If mesresult = MessageBoxResult.Cancel Then
                    Return mesresult
                Else
                    retunrvalue = True
                    Close()
                    tbHausnr.Text = ""
                End If
            Else
                strasseOhneHausnr = False
            End If
            If hausnrListe IsNot Nothing Then
                If hausnrListe.Count = 1 Then
                    Dim hausnrkombi As String = hausnrListe(0).displayText.Trim
                    MessageBox.Show("Es wurde nur eine Hausnummer gefunden!" & Environment.NewLine &
                                "Hausnummer: " & hausnrkombi, "Adresssuche")
                    Dim tobj As String
                    tobj = hausnrListe(0).nenner
                    tobj = tobj.Trim
                    showhausnummerAuswahl(hausnrkombi, tobj)
                    If Not ckFormNichtSchliessen.IsChecked Then
                        DialogResult = False
                        Me.Close()
                    End If
                End If
            End If
            l(" showHausnr ---------------------- ende")
            Return mesresulkt
        Catch ex As Exception
            l("Fehler in showHausnr: " & ex.ToString())
            Return mesresulkt
        End Try
    End Function



    'Private Sub cmbHausnr_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
    '    e.Handled = True
    '    If cmbHausnr.SelectedItem Is Nothing Then Exit Sub
    '    Dim hausnrkombi$ = CStr(cmbHausnr.SelectedValue)
    '    Dim item2 As DataRowView = CType(cmbHausnr.SelectedItem, DataRowView)
    '    If item2 Is Nothing Then Exit Sub

    '    Dim weistauf = item2.Row.ItemArray(1).ToString
    '    Dim rechts = item2.Row.ItemArray(2).ToString
    '    Dim hoch = item2.Row.ItemArray(3).ToString

    '    handelhausnr(hausnrkombi, rechts, hoch, weistauf)
    '    If Not ckFormNichtSchliessen.IsChecked Then
    '        DialogResult = False
    '        Me.Close()
    '    End If
    'End Sub

    Private Sub handelhausnr(hausnrkombi As String, rechts As String, hoch As String, weistauf As String, flurstueckErmitteln As Boolean)
        aktGlobPoint.strX = rechts
        aktGlobPoint.strY = hoch

        aktadr.Gisadresse.HausKombi = hausnrkombi
        myglobalz.aktadr.Gisadresse.hauskombiZerlegen()
        If flurstueckErmitteln Then
            Dim sql As String = "" : Dim hinweis As String = ""
            sql = clsAdr2Fst.getSQL4FST4Adr()
            Dim anzahl As Integer
            If iminternet Or CGIstattDBzugriff Then
                Dim result = clsToolsAllg.getSQL4Http(sql, "postgis20", hinweis, "getsql")
                l(hinweis)
                result = result.Trim
                If result.IsNothingOrEmpty Then
                    summentext = ""
                End If
                summentext = clsAdr2Fst.getflurstueckajax(result, myglobalz.aktFST.normflst, anzahl)
            Else
                Dim fsDT As DataTable = clsAdr2Fst.getflurstueckDB(myglobalz.aktadr.Gisadresse, sql)
                summentext = clsAdr2Fst.mapFlurstueckDB(fsDT, myglobalz.aktFST.normflst, anzahl)
            End If
            displayFurstueck("Folgende(s) " & anzahl & " Flurstück(e) ist dieser Adresse zugeordnet: " & vbCrLf & summentext)
        End If

        aktadr.punkt.X = CDbl(CInt(aktGlobPoint.strX))
        aktadr.punkt.Y = CDbl(CInt(aktGlobPoint.strY))
        tbHausnr.Text = hausnrkombi
        kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, 100)
        spNachnenner.Visibility = Visibility.Visible
        retunrvalue = True
        'starteWebbrowserControl(bbox)
        tbFreitext.Text = "Adresse: " & tbGemeinde.Text & ", " & tbStrasse.Text & ", " & tbHausnr.Text & ", "
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            gbFSTaradigma.Visibility = Visibility.Visible
            spEigentNotizUebernehmen.Visibility = Visibility.Visible
        Else
            gbFSTaradigma.Visibility = Visibility.Collapsed
            spEigentNotizUebernehmen.Visibility = Visibility.Collapsed
        End If
        tbFSTINFO.Visibility = Visibility.Visible
    End Sub



    Private Sub displayFurstueck(summentext As String)
        tbFSTINFO.Text = summentext
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        e.Handled = True
        If Not ckFormNichtSchliessen.IsChecked Then
            DialogResult = False
            Me.Close()
        End If
    End Sub

    Private Sub ckFormNichtSchliessen_Checked(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub btnAdresseNachParadigma_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If myglobalz.aktFST.normflst.FS.IsNothingOrEmpty Then
            MsgBox("Sie müssen erst eine Adresse auswählen!")
            Exit Sub
        End If
        Dim erfolg As Boolean
        erfolg = clsADRtools.adresseNaCHpARADIGMA(tbFreitext.Text.Trim, tbname.Text.Trim)
        If erfolg Then
            MessageBox.Show("Die Adresse wurde erfolgreich nach Paradigma übernommen")
        Else
            MessageBox.Show("Die Adresse wurde NICHT nach Paradigma übernommen")
        End If
        Close()
        e.Handled = True
    End Sub



    Private Sub cmbFunktionsvorschlaege_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If cmbFunktionsvorschlaege.SelectedItem Is Nothing Then Exit Sub
        'Dim myvali$ = CStr(cmbFunktionsvorschlaege.SelectedValue)
        Dim myvalx = CType(cmbFunktionsvorschlaege.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        tbname.Text = myvals
        e.Handled = True
    End Sub

    Private Sub btnUnscharfSuchen_Click(sender As Object, e As RoutedEventArgs)
        If tbStrasse2.Text.Trim.Length < 1 Then
            MsgBox("Sie müssen eine Strasse eingeben, mind. 3 Buchstaben")
            Exit Sub
        End If
        If tbGemeinde2.Text.Trim.Length < 1 Then
            MsgBox("Sie müssen eine Strasse eingeben, mind. 3 Buchstaben")
            Exit Sub
        End If

        adrREC.dt = clsStrasseUnscharfSuchen.getDatatable(tbStrasse2.Text, CBool(cbOhneGemeinde.IsChecked))
        If adrREC.dt IsNot Nothing Then
            cmbStrasse2.DataContext = adrREC.dt
            cmbStrasse2.IsDropDownOpen = True
        End If
        e.Handled = True
    End Sub

    Private Sub cmbgemeinde2_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        aktadr.Gisadresse.gemeindebigNRstring = CStr(cmbgemeinde2.SelectedValue)
        Dim myvalx = CType(cmbgemeinde2.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        tbGemeinde2.Text = myvals
        aktadr.Gisadresse.gemeindeName = myvals
        e.Handled = True
    End Sub

    Private Sub ckFormNichtSchliessen2_Checked(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub cmbStrasse2_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If cmbStrasse2.SelectedItem Is Nothing Then Exit Sub
        Dim myvali$ = CStr(cmbStrasse2.SelectedValue)
        Dim item2 As DataRowView = CType(cmbStrasse2.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        If CBool(cbOhneGemeinde.IsChecked) Then
            aktadr.Gisadresse.strasseName = item2.Row.ItemArray(0).ToString.Trim
            Dim a() As String
            a = aktadr.Gisadresse.strasseName.Split(","c)

            aktadr.Gisadresse.gemeindeName = a(0).Trim
            aktadr.Gisadresse.strasseName = a(1).Trim
            Dim btemp As String = (item2.Row.ItemArray(1).ToString.Trim)
            a = btemp.Split(","c)
            aktadr.Gisadresse.strasseCode = CInt(a(0))
            aktadr.Gisadresse.gemeindebigNRstring = a(1)
        Else
            aktadr.Gisadresse.strasseName = item2.Row.ItemArray(0).ToString.Trim

            aktadr.Gisadresse.strasseCode = CInt(item2.Row.ItemArray(1).ToString.Trim)
        End If


        'sname = item2.Row.ItemArray(2).ToString.Trim


        'Dim zeigtauf, summentext As String
        'Dim gemeindestring As String = item3$.Replace("06438" & , "001")
        '  gemeindestring = item3$.Substring(5, 3).Trim

        'cmbHausnr.IsEnabled = True
        tbStrasse.Text = aktadr.Gisadresse.strasseName ' item2.Row.ItemArray(0).ToString 
        'myGlobalz.aktADR.strasseCode = CInt(item4)
        'myGlobalz.aktADR.strasseName = item5

        'myGlobalz.adrREC.mydb.Host = "kis"
        'myGlobalz.adrREC.mydb.Schema = "albnas"

        clsADRtools.inithausnrComboDT()
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

            cmbHausnr2.DataContext = adrREC.dt
            cmbHausnr2.IsDropDownOpen = True
        End If
        e.Handled = True
    End Sub

    Private Sub cmbHausnr2_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If cmbHausnr2.SelectedItem Is Nothing Then Exit Sub
        Dim hausnrkombi$ = CStr(cmbHausnr2.SelectedValue)
        Dim item2 As DataRowView = CType(cmbHausnr2.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Dim summentext As String = "" ' wird hier ignoriert 
        aktadr.Gisadresse.HausKombi = hausnrkombi
        myglobalz.aktadr.Gisadresse.hauskombiZerlegen()

        If Not ckFormNichtSchliessen.IsChecked Then
            Dim sql As String = ""
            Dim hinweis As String = ""
            sql = clsAdr2Fst.getSQL4FST4Adr()
            Dim anzahl As Integer
            If iminternet Or CGIstattDBzugriff Then
                Dim result = clsToolsAllg.getSQL4Http(sql, "postgis20", hinweis, "getsql")
                l(hinweis)
                result = result.Trim
                If result.IsNothingOrEmpty Then
                    summentext = ""
                End If
                summentext = clsAdr2Fst.getflurstueckajax(result, myglobalz.aktFST.normflst, anzahl)
            Else
                Dim fsdt As DataTable = clsAdr2Fst.getflurstueckDB(myglobalz.aktadr.Gisadresse, sql)
                summentext = clsAdr2Fst.mapFlurstueckDB(fsdt, myglobalz.aktFST.normflst, anzahl)
            End If
            displayFurstueck("Folgende(s) " & anzahl &
                    " Flurstück(e) ist dieser Adresse zugeordnet: " & vbCrLf & summentext)
        End If

        Dim weistauf = item2.Row.ItemArray(1).ToString
        aktGlobPoint.strX = item2.Row.ItemArray(2).ToString
        aktGlobPoint.strY = item2.Row.ItemArray(3).ToString
        aktadr.punkt.X = CDbl(CInt(aktGlobPoint.strX))
        aktadr.punkt.Y = CDbl(CInt(aktGlobPoint.strY))
        tbHausnr.Text = hausnrkombi
        kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, 100)
        retunrvalue = True
        'starteWebbrowserControl(bbox)
        tbFreitext.Text = "Adresse: " & tbGemeinde.Text & ", " & tbStrasse.Text & ", " & tbHausnr.Text & ", "
        If STARTUP_mgismodus.ToLower = "paradigma" Then
            gbFSTaradigma.IsEnabled = True
        End If
        e.Handled = True
        If Not ckFormNichtSchliessen.IsChecked Then
            DialogResult = False
            Me.Close()
        End If
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
        If myglobalz.aktFST.normflst.FS.IsNothingOrEmpty Then
            MsgBox("Sie müssen erst eine Adresse auswählen!")
            Exit Sub
        End If
        GrundFuerEigentuemerabfrage = tbGrund.Text

        'If cbSchnellEigentuemer.IsChecked Then
        SchnellausgabeMitProtokoll(tbGrund.Text)
        EigentuemerPDF = clsADRtools.erzeugeUndOeffneEigentuemerPDF(tbWeitergabeVerbot.Text, summentext)
        OpenDokument(EigentuemerPDF)
        'Else
        '    Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents
        '    'tbKurz.Text = "Bitte warten  ...."
        '    Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
        '    aktFST.normflst.splitFS(aktFST.normflst.FS)
        '    Dim specparms(8) As String
        '    specparms(3) = aktFST.normflst.FS
        '    specparms(4) = aktFST.normflst.FS
        '    Dim weistauf As String = ""
        '    Dim zeigtauf As String = ""
        '    Dim gebucht As String = ""
        '    Dim areaqm As String = ""
        '    If Module2.holeRestlicheParams4FST(aktFST.normflst.FS, weistauf, zeigtauf, gebucht, areaqm) Then
        '        specparms(5) = weistauf
        '        specparms(6) = zeigtauf
        '        specparms(7) = gebucht
        '        specparms(8) = areaqm
        '        EigentuemerPDF = getEigentuemerDatei(specparms)
        '        tools.openDocument(EigentuemerPDF)
        '    End If
        'End If
        If STARTUP_mgismodus = "paradigma" Then
            spEigentNotizUebernehmen.Visibility = Visibility.Visible
            spEigentNotizUebernehmen.IsEnabled = True
            gbFSTaradigma.Visibility = Visibility.Visible
            gbFSTaradigma.IsEnabled = True
        End If
        e.Handled = True
    End Sub
    Sub SchnellausgabeMitProtokoll(grund As String)
        'If schonSchnellAusgegeben Then Exit Sub
        If grund Is Nothing OrElse grund.Trim.Length < 2 OrElse grund = "Aktenzeichen" Then
            MsgBox("Bitte eine Begründung (z.B. das Aktenzeichen) eingeben!")
            FocusManager.SetFocusedElement(Me, tbGrund)
            Exit Sub
        End If
        Dim info As String
        info = "Eigentümer in Kurzform: " & Environment.NewLine &
                                    getSchnellbatchEigentuemer(aktFST.normflst.FS)
        tbWeitergabeVerbot.Text = info
        'schonSchnellAusgegeben = True
        Protokollausgabe_aller_Parameter(aktFST.normflst.FS, grund)
    End Sub

    Private Sub btnGoogleStrassensuche_Click(sender As Object, e As RoutedEventArgs)
        MsgBox("Baustelle")
        e.Handled = True
    End Sub

    Private Sub btnSchnellNachPDF_Click(sender As Object, e As RoutedEventArgs)
        clsADRtools.erzeugeUndOeffneEigentuemerPDF(tbWeitergabeVerbot.Text, summentext)
        e.Handled = True
    End Sub


    Private Sub btnDossier_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim utm As New Point
        utm.X = aktadr.punkt.X
        utm.Y = aktadr.punkt.Y
        Dim KoordinateKLickpt As New Point
        KoordinateKLickpt.X = 1
        KoordinateKLickpt.Y = 1
        globCanvasWidth = 2
        globCanvasHeight = 2
        clsSachdatentools.getdossier(utm, layerActive.aid,
                                            CInt(globCanvasWidth), CInt(globCanvasHeight),
                                            KoordinateKLickpt, "", "punkt")
    End Sub

    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        retunrvalue = False
        Close()
    End Sub

    Private Sub showhausnummerAuswahl(hausnrkombi As String, tobj As String)
        Dim rechts, hoch, weistauf As String
        Dim a() As String
        a = tobj.Split("#"c)
        weistauf = a(0)
        rechts = a(1)
        hoch = a(2)

        handelhausnr(hausnrkombi, rechts, hoch, weistauf, CBool(ckFormNichtSchliessen.IsChecked))
    End Sub

    'Private Sub btnstreet(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True

    '    lvHausnr.ItemsSource = Nothing
    '    Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    '    Dim a As String = (sender.ToString)
    '    a = a.Replace("System.Windows.Controls.Button:", "")
    '    a = a.Trim.ToLower
    '    buchstabeZuStrasse(a)
    'End Sub

    'Private Sub buchstabeZuStrasse(a As String)

    '    lvStrassen.Visibility = Visibility.Visible
    '    initStrassenCombo(a)
    '    strassenListe = initStrassenliste(adrREC.dt)
    '    lvStrassen.ItemsSource = strassenListe
    'End Sub

    Private Sub txGemarkungs_MouseDown(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim nck As Button = CType(sender, Button)
        cmbgemeinde.Visibility = Visibility.Visible
        lvGemarkungen.Visibility = Visibility.Collapsed
        lvStrassen.Visibility = Visibility.Visible
        spAlleBuchstaben.Visibility = Visibility.Visible

        showStrassen(CType(nck.Tag, String), nck.Uid)
        lvHausnr.ItemsSource = Nothing
        FocusManager.SetFocusedElement(Me, tbfilter)
    End Sub

    Private Sub txStrassen_MouseDown(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim nck As Button = CType(sender, Button)
        Dim mesresult As New MessageBoxResult
        lvHausnr.Visibility = Visibility.Visible
        lvStrassen.Visibility = Visibility.Visible
        spAlleBuchstaben.Visibility = Visibility.Visible
        lvHausnr.ItemsSource = Nothing
        'cmbStrasse.Visibility = Visibility.Visible
        mesresult = showHausnr(CType(nck.Tag, String), CInt(nck.Uid))
    End Sub
    Private Sub txhausnr_MouseDown(sender As Object, e As MouseButtonEventArgs)

    End Sub
    Private Sub txhausnr_MouseDown(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim nck As Button = CType(sender, Button)
        Dim tobj As String = nck.Tag.ToString.Trim
        Dim hausnrkombi As String = nck.Content.ToString.Trim
        showhausnummerAuswahl(hausnrkombi, tobj)

        If Not ckFormNichtSchliessen.IsChecked Then
            DialogResult = False
            Me.Close()
        End If
    End Sub


    'Private Sub myTestKey(sender As Object, e As KeyEventArgs)
    '    e.Handled = True
    '    If spAlleBuchstaben.Visibility = Visibility.Visible Then
    '        'MsgBox(e.Key & " " & Chr(KeyInterop.VirtualKeyFromKey(e.Key)))
    '        Dim a As String = Chr(KeyInterop.VirtualKeyFromKey(e.Key))
    '        buchstabeZuStrasse(a.ToLower)
    '    End If
    'End Sub

    Private Sub tbfilter_TextChanged(sender As Object, e As TextChangedEventArgs)
        e.Handled = True
        Dim cand As String = tbfilter.Text.ToLower
        Dim strKlein As New List(Of clsFlurauswahl)
        For Each str As clsFlurauswahl In strassenListe
            If cand.Length > 1 Then
                If Not str.displayText.ToLower.Contains(cand) Then Continue For
            Else
                If Not str.displayText.ToLower.StartsWith(cand) Then Continue For
            End If

            'If str.nenner <> String.Empty Then
            '    str.displayText = str.id & "/" & str.nenner
            'Else
            '    str.displayText = CType(str.id, String)
            'End If
            strKlein.Add(str)
        Next
        lvHausnr.ItemsSource = Nothing
        lvStrassen.ItemsSource = strKlein
    End Sub
End Class
