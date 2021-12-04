Imports System.Data
Public Class winDetailAdressSuche
    Public gemeinde_verz As String = serverUNC & "apps\test\mgis\combos\gemeinden.xml"
    Public funktionen_verz As String = serverUNC & "apps\test\mgis\combos\RBfunktion.xml"
    Public EigentuemerPDF As String
    Public Property retunrvalue As Boolean = False
    Property summentext As String = "" ' wird hier ignoriert

    Private Sub winDetailAdressSuche_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        initGemeindeCombo()
        'cmbgemeinde.IsDropDownOpen = True
        '  aktadr.clear()
        spNachnenner.Visibility = Visibility.Collapsed
        tbWeitergabeVerbot.Text = albverbotsString
        spEigentNotizUebernehmen.IsEnabled = False
        If GisUser.istalbberechtigt Then
            gbEigentuemer.Visibility = Visibility.Visible
            gbEigentuemer.IsEnabled = True
        Else
            gbEigentuemer.Visibility = Visibility.Collapsed
            gbEigentuemer.IsEnabled = False
        End If

        'If istAlbBerechtigt() Then
        '    gbEigentuemer.IsEnabled = True
        'Else
        '    gbEigentuemer.IsEnabled = False
        'End If
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
            tbGemeinde.Text = aktadr.Gisadresse.gemeindeName
            tbStrasse.Text = aktadr.Gisadresse.strasseName
            tbHausnr.Text = aktadr.Gisadresse.HausKombi
            initStrassenCombo()
            aktadr.Gisadresse.strasseName = tbStrasse.Text
            cmbStrasse.DataContext = adrREC.dt
            inithausnrCombo()
            cmbHausnr.DataContext = adrREC.dt
            aktadr.Gisadresse.HausKombi = tbHausnr.Text
        Else
            'cmbgemarkung.IsDropDownOpen = True
        End If
        e.Handled = True
    End Sub
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
        existing.Source = New Uri(gemeinde_verz)
        existing = TryCast(Me.Resources("XMLSourceComboBoxRBfunktion"), XmlDataProvider)
        existing.Source = New Uri(funktionen_verz)
    End Sub
    Private Sub cmbGemeinde_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If cmbgemeinde.SelectedItem Is Nothing Then Exit Sub
        aktadr.Gisadresse.gemeindebigNRstring = CStr(cmbgemeinde.SelectedValue)
        Dim myvalx = CType(cmbgemeinde.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        tbGemeinde.Text = myvals
        aktadr.Gisadresse.gemeindeName = myvals
        'aktADR.gemeindeNr = CInt(CStr(myvali))
        'aktADR.gemeindeName = tbGemeinde.Text

        initStrassenCombo()
        cmbStrasse.DataContext = adrREC.dt
        cmbStrasse.IsDropDownOpen = True
        e.Handled = True
    End Sub

    Public Sub initStrassenCombo()
        ' Dim a = "SELECT lage,bezeichnung,gml_id FROM halosort.lageschluessel where schluesselgesamt like ""06438001%"" order by bezeichnung"
        Dim schluesssellike As String
        schluesssellike = "06" & aktadr.Gisadresse.gemeindebigNRstring
        'adrREC.mydb.SQL =
        ' "SELECT schluesselgesamt,lage,bezeichnung,gml_id FROM public.lageschluessel " &
        ' " where schluesselgesamt like '" & schluesssellike & "%'" &
        ' " order by bezeichnung  "
        adrREC.mydb.SQL =
         "SELECT distinct trim(sname) as sname,strcode  FROM public.halofs " &
         " where gemeindeNR  = " & aktadr.Gisadresse.gemeindebigNRstring & "" &
         " order by  (sname)  "
        '  myGlobalz.adrREC.mydb.Schema = "halosort"
        Dim hinweis As String = adrREC.getDataDT()
    End Sub

    Private Sub cmbStrasse_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        e.Handled = True
        If cmbStrasse.SelectedItem Is Nothing Then Exit Sub
        Dim myvali$ = CStr(cmbStrasse.SelectedValue)
        Dim item2 As DataRowView = CType(cmbStrasse.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        aktadr.Gisadresse.strasseName = item2.Row.ItemArray(0).ToString.Trim
        aktadr.Gisadresse.strasseCode = CInt(item2.Row.ItemArray(1).ToString.Trim)
        cmbHausnr.IsEnabled = True
        tbStrasse.Text = aktadr.Gisadresse.strasseName

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
            End If
        Else
            cmbHausnr.DataContext = adrREC.dt
            cmbHausnr.IsDropDownOpen = True
        End If

    End Sub

    Sub inithausnrCombo()
        'adrREC.mydb.SQL =
        ' "SELECT hausnummer,gml_id FROM albnas.ax_lagebezeichnungmithausnummer " &
        ' " where gemeinde = '" & gemeindestring & "'" &
        ' " and kreis = '38'" &
        ' " and lage ='" & strasseCode & "'" &
        ' " order by  abs(hausnummer)"

        adrREC.mydb.SQL =
         "SELECT hausnrkombi,gml_id,rechts,hoch FROM public.halofs " &
         " where gemeindenr = '" & aktadr.Gisadresse.gemeindebigNRstring & "'" &
         " and strcode ='" & aktadr.Gisadresse.strasseCode & "'" &
         " order by  abs(hausnr)"

        Dim hinweis As String = adrREC.getDataDT()
    End Sub

    Private Sub cmbHausnr_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        e.Handled = True
        If cmbHausnr.SelectedItem Is Nothing Then Exit Sub
        Dim hausnrkombi$ = CStr(cmbHausnr.SelectedValue)
        Dim item2 As DataRowView = CType(cmbHausnr.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        aktadr.Gisadresse.HausKombi = hausnrkombi
        myglobalz.aktadr.Gisadresse.hauskombiZerlegen()
        Dim fsDT As DataTable = clsAdr2Fst.getflurstueck(myglobalz.aktadr.Gisadresse)

        clsAdr2Fst.mapFlurstueck(fsDT, myglobalz.aktFST.normflst, summentext)

        displayFurstueck("Folgende(s) " & fsDT.Rows.Count &
            " Flurstück(e) ist dieser Adresse zugeordnet: " & vbCrLf & summentext)

        Dim weistauf = item2.Row.ItemArray(1).ToString
        aktGlobPoint.strX = item2.Row.ItemArray(2).ToString
        aktGlobPoint.strY = item2.Row.ItemArray(3).ToString
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

        If Not ckFormNichtSchliessen.IsChecked Then
            DialogResult = False
            Me.Close()
        End If
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
        Dim erfolg As Boolean
        erfolg = adresseNaCHpARADIGMA()
        If erfolg Then
            MessageBox.Show("Die Adresse wurde erfolgreich nach Paradigma übernommen")
        Else
            MessageBox.Show("Die Adresse wurde NICHT nach Paradigma übernommen")
        End If
        Close()
        e.Handled = True
    End Sub

    Private Function adresseNaCHpARADIGMA() As Boolean
        Dim umkreisID As Integer
        Try
            aktadr.setcoordsAbstract()
            aktadr.Freitext = tbFreitext.Text.Trim
            aktadr.Name = tbname.Text.Trim
            aktadr.Gisadresse.Quelle = "halo"
            aktadr.Gisadresse.gemeindeName = clsString.Capitalize(aktadr.Gisadresse.gemeindeName)
            aktadr.Typ = RaumbezugsTyp.Adresse
            aktadr.isMapEnabled = True
            aktadr.PLZ = "0"
            aktadr.FS = ""
            aktadr.Postfach = ""
            aktadr.Adresstyp = adressTyp.ungueltig
            Dim radius = 100
            'modEW.Paradigma_Adresse_Neu(radius)
            umkreisID = modParadigma.Paradigma_Adresse_Neu(radius)
            If umkreisID > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            l("fehler in adresseNaCHpARADIGMA ", ex)
            Return False
        End Try
        Close()
    End Function

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

        cmbHausnr.IsEnabled = True
        tbStrasse.Text = aktadr.Gisadresse.strasseName ' item2.Row.ItemArray(0).ToString 
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
        Dim fsdt As DataTable = clsAdr2Fst.getflurstueck(myglobalz.aktadr.Gisadresse)

        clsAdr2Fst.mapFlurstueck(fsdt, myglobalz.aktFST.normflst, summentext)

        displayFurstueck("Folgende(s) " & fsdt.Rows.Count &
            " Flurstück(e) ist dieser Adresse zugeordnet: " & vbCrLf & summentext)

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
            MsgBox("Der User: " & GisUser.username & " ist nicht berechtigt auf die Eigentümerdaten zuzugreifen. ")
            Exit Sub
        End If
        If tbGrund.Text Is Nothing OrElse tbGrund.Text.Trim.Length < 2 Then
            MsgBox("Bitte eine Begründung (z.B. das Aktenzeichen) eingeben!")
            Exit Sub
        End If
        GrundFuerEigentuemerabfrage = tbGrund.Text

        If cbSchnellEigentuemer.IsChecked Then
            SchnellausgabeMitProtokoll(tbGrund.Text)
            EigentuemerPDF = erzeugeUndOeffneEigentuemerPDF()
            OpenDokument(EigentuemerPDF)
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
        erzeugeUndOeffneEigentuemerPDF()
        e.Handled = True
    End Sub

    Private Function erzeugeUndOeffneEigentuemerPDF() As String
        Dim lokalitaet, flaeche As String
        'Dim ausgabeDIR As String = My.Computer.FileSystem.SpecialDirectories.Temp '& "" & aid
        'ausgabeDIR = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        lokalitaet = summentext
        flaeche = clsFSTtools.getFlaecheZuFlurstueck(aktFST)
        lokalitaet = lokalitaet & " " & flaeche
        'IO.Directory.CreateDirectory(ausgabeDIR)
        'Dim ausgabedatei As String = ausgabeDIR & "\eigentuemer" & Format(Now, "dd.MM.yyyy_hhmmss") & ".pdf"
        Dim ausgabedatei As String = tools.calcEigentuemerAusgabeFile
        wrapItextSharp.createSchnellEigentuemer(tbWeitergabeVerbot.Text, ausgabedatei, albverbotsString, lokalitaet)
        Return ausgabedatei
    End Function
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
        Close()
    End Sub
End Class
