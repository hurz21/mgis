Imports System.ComponentModel

Public Class winDBabfrage
    Property _rtfdatei As String
    Property _flow As FlowDocument
    Property _modus As String
    Property _buttonINfostring As String = ""
    Property _isUserLayer As Boolean
    Property secfuncParms As String()
    Property Soll_refreshmap As Boolean = False
    Property paradigmavid As String = ""
    Property editTableID As String = ""
    Public Property EigentuemerPDF As String = ""
    Public ladevorgangAbgeschlossen As Boolean = False

    Sub New(rtfdatei As String, modus As String, buttonINfostring As String, isUserLayer As Boolean, Optional flow As FlowDocument = Nothing)
        ' This call is required by the designer.
        InitializeComponent()
        _rtfdatei = rtfdatei
        _modus = modus 'datei oder text, dabei ist text die DB abfrage
        _flow = flow
        _buttonINfostring = buttonINfostring
        _isUserLayer = isUserLayer
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub windbabfrage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            Top = 1
            e.Handled = True
            btnDossier.Visibility = Visibility.Collapsed
            l("windbabfrage_Loaded -------------------")
            l("buttonINfostring " & _buttonINfostring)
            btnZumParadigmaVorgang.Visibility = Visibility.Collapsed
            tbEigentuemerSchnell.Visibility = Visibility.Collapsed
            spDBObjToolbar.Visibility = Visibility.Collapsed
            gbEigentuemer.Visibility = Visibility.Collapsed
            btnEditDB.Visibility = Visibility.Collapsed
            spDB.Width = 900
            Dim rtfTextDoku As String
            If _buttonINfostring.Trim = String.Empty Then
                'If _buttonINfostring.ToLower.Contains("eigentuemer") Then
                'gbEigentuemer.Visibility = Visibility.Collapsed
                'spDB.Width = 900
                'l("eigentuemer nicht erlaubt")
                'End If
            Else
                If _buttonINfostring.ToLower.Contains("bplanbegleit") Then
                    l("begleitmaterial zu beplan")
                    Dim gemarkung, pdf, verzeichnis As String
                    sachdatenTools.bplanbegleitInfoAufloesen(_buttonINfostring, gemarkung, pdf)
                    verzeichnis = sachdatenTools.bplanbegleitInfoCalcDirectory(gemarkung, pdf,
                                                                               myglobalz.serverUNC & "fkat\")
                    Dim begleitfilelist As New List(Of IO.FileInfo)

                    begleitfilelist = getBegleitplanFileliste(pdf, verzeichnis)
                    If begleitfilelist.Count > 0 Then
                        dgZusatzinfo.Visibility = Visibility.Visible
                        dgZusatzinfo.IsEnabled = True
                    Else
                        dgZusatzinfo.Visibility = Visibility.Visible
                        dgZusatzinfo.IsEnabled = False
                    End If
                    Debug.Print("")
                    dgZusatzinfo.DataContext = begleitfilelist
                    'If begleitfilelist.Count > 0 Then
                    '    tiBegleit.Header = "Zusatzinformationen " & begleitfilelist.Count
                    'Else
                    '    tiBegleit.Header = "Zusatzinformationen - keine "
                    'End If

                    'gbEigentuemer.Visibility = Visibility.Visible
                    'createButtonEigentuemer()
                    'spDB.Width = 400
                    'tbWeitergabeVerbot.Text = verbotsString
                    'tbEigentuemerSchnell.Visibility = Visibility.Visible
                End If
                If _buttonINfostring.ToLower.Contains("eigentümer") Then
                    l("eigentuemer   erlaubt")
                    Debug.Print(aktFST.normflst.FS)
                    gbEigentuemer.Visibility = Visibility.Visible
                    btnDossier.Visibility = Visibility.Visible
                    createButtonEigentuemer()
                    spDB.Width = 400
                    tbWeitergabeVerbot.Text = albverbotsString
                    tbEigentuemerSchnell.Visibility = Visibility.Visible
                End If

                If _buttonINfostring.ToLower.Contains("paradigmavid") Then
                    l("paradigmavid   erlaubt")
                    paradigmavid = getParadigmaVID(_buttonINfostring)
                    If Not paradigmavid.IsNothingOrEmpty Then
                        If Not paradigmavid = "0" Then
                            btnZumParadigmaVorgang.Visibility = Visibility.Visible
                            btnZumParadigmaVorgang.Content = "Zum Vorgang <" & paradigmavid & "> in Paradigma"
                            'createButtonEigentuemer()
                            'tbWeitergabeVerbot.Text = "Hier könn"
                        End If
                    End If
                    If sachdatenTools.userIstLayerEditor(GisUser.username, layerActive.aid) Then
                        editTableID = geteditTableID(layerActive.aid, _buttonINfostring)
                        btnEditDB.Visibility = Visibility.Visible
                    End If
                End If
            End If
            If _modus = "text" Then
                initGemarkungsCombo()
                Title = "DB-Anzeige (Aid: " & layerActive.aid & ", " & aktObjID & ")"
                rtfTextDoku = textmodus(_flow)
                Width = 950
                Height = 990
                spDBObjToolbar.Visibility = Visibility.Visible
                If _isUserLayer Then
                    btnLoeschen.IsEnabled = True
                Else
                    btnLoeschen.IsEnabled = False
                End If
            End If

            If GisUser.istalbberechtigt Then
                gbEigentuemer.IsEnabled = True
                setzeGrundFuerEigentuemerabfrage(tbGrund.Text)
            Else
                gbEigentuemer.IsEnabled = False
            End If
            If STARTUP_mgismodus = "paradigma" Then
                gbFSTaradigma.Visibility = Visibility.Visible
            Else
                gbFSTaradigma.Visibility = Visibility.Collapsed
            End If
            Me.Top = clsToolsAllg.setPosition("diverse", "dbabfrageformpositiontop", Me.Top)
            Me.Left = clsToolsAllg.setPosition("diverse", "dbabfrageformpositionleft", Me.Left)
            ladevorgangAbgeschlossen = True
        Catch ex As Exception
            l("fehler in windbabfrage_Loaded " & ex.ToString)
        End Try
    End Sub
    Private Sub savePosition()
        Try
            userIniProfile.WertSchreiben("diverse", "dbabfrageformpositiontop", CType(Me.Top, String))
            userIniProfile.WertSchreiben("diverse", "dbabfrageformpositionleft", CType(Me.Left, String))
        Catch ex As Exception
            l("fehler in saveposition  windb" & ex.ToString)
        End Try
    End Sub
    Sub initGemarkungsCombo()
        Dim existing As XmlDataProvider = TryCast(Me.Resources("XMLSourceComboBoxgemarkungen"), XmlDataProvider)
        existing.Source = New Uri(Paradigma_GemarkungsXML)
        existing = TryCast(Me.Resources("XMLSourceComboBoxRBfunktion"), XmlDataProvider)
        existing.Source = New Uri(Paradigma_funktionen_verz)
    End Sub


    Private Function geteditTableID(aid As Integer, _buttonINfostring As String) As String
        Dim a() As String
        Try
            l("geteditTableID---------------------- anfang")
            'specfunc,ParadigmaVID,startParadigma,[paradigmavid] 
            a = _buttonINfostring.Split(","c)
            l(a(4))
            l("geteditTableID---------------------- ende")
            Return a(4)
        Catch ex As Exception
            l("Fehler in geteditTableID: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Function getParadigmaVID(_buttonINfostring As String) As String
        Dim a() As String
        Try
            l("getParadigmaVID---------------------- anfang")
            'specfunc,ParadigmaVID,startParadigma,[paradigmavid] 
            a = _buttonINfostring.Split(","c)
            l(a(3))
            l("getParadigmaVID---------------------- ende")
            Return a(3)
        Catch ex As Exception
            l("Fehler in getParadigmaVID: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Sub createButtonEigentuemer()
        Try
            secfuncParms = _buttonINfostring.Split(","c)
            btnSpecFunc.Content = clsString.Capitalize(secfuncParms(1))
        Catch ex As Exception
            l("fehler in createButton " & ex.ToString)
        End Try
    End Sub

    Private Function textmodus(_text As FlowDocument) As String
        Dim neuflow As New FlowDocument
        neuflow = _flow
        freiLegende.Document = neuflow
        Return ""
    End Function

    Private Function dateimodus() As String
        Dim rtfTextDoku As String
        Using datei As IO.StreamReader = New IO.StreamReader(_rtfdatei)
            rtfTextDoku = datei.ReadToEnd
        End Using
        Dim documentBytes = Text.Encoding.UTF8.GetBytes(rtfTextDoku)
        Dim reader = New System.IO.MemoryStream(documentBytes)
        reader.Position = 0
        freiLegende.SelectAll()
        freiLegende.Selection.Load(reader, DataFormats.Rtf)
        Return rtfTextDoku
    End Function

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Clipboard.Clear()
        Dim szz As String = ""
        Dim content As New TextRange(freiLegende.Document.ContentStart, freiLegende.Document.ContentEnd)
        If content.CanSave(DataFormats.Rtf) Then
            Using stream = New IO.MemoryStream
                content.Save(stream, DataFormats.Rtf, True)
                ' Dim sw As New IO.StreamWriter(tstream)
                szz = System.Text.Encoding.ASCII.GetString(stream.ToArray())
            End Using
        End If
        Clipboard.SetText(szz, TextDataFormat.Rtf)
        GC.Collect()
        MsgBox("Sie können den Text jetzt mit Strg-v  in ein Word-Dokument einfügen!",, "Zwischenablage")
        e.Handled = True
    End Sub


    Private Sub btnSpecFunc_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Select Case secfuncParms(2).ToLower
            Case "geteigentuemerdatei"
                If tbGrund.Text Is Nothing OrElse tbGrund.Text.Trim.Length < 2 Then
                    setzeGrundFuerEigentuemerabfrage(tbGrund.Text)
                    MsgBox("Bitte eine Begründung (z.B. das Aktenzeichen) eingeben!")
                    Exit Sub
                End If
                geteigentuemerDatei2(tbGrund.Text)
                GrundFuerEigentuemerabfrage = tbGrund.Text
                EigentuemerPDF = clsSachdatentools.erzeugeUndOeffneEigentuemerPDF(tbEigentuemerSchnell.Text)
                OpenDokument(EigentuemerPDF)
                If STARTUP_mgismodus = "paradigma" Then
                    spEigentNotizUebernehmen.Visibility = Visibility.Visible
                    spEigentNotizUebernehmen.IsEnabled = True
                End If
        End Select
        e.Handled = True
    End Sub
    Sub geteigentuemerDatei2(grund As String)
        If Not GisUser.istalbberechtigt Then
            MsgBox("Der User: " & GisUser.username & " ist nicht berechtigt auf die Eigentümerdaten zuzugreifen. Programmende")
            Exit Sub
        End If
        'If Not NSfstmysql.ADtools.istUserAlbBerechtigt(  GisUser.username, fdkurz) Then
        '    MsgBox("Der User: " &   GisUser.username & " ist nicht berechtigt auf die Eigentümerdaten zuzugreifen. Programmende")
        '    Exit Sub
        'End If
        Dim pdfEigentuemerDatei As String
        If cbSchnellEigentuemer.IsChecked Then
            Dim info As String
            info = "Eigentümer in Kurzform: " & Environment.NewLine &
                                        getSchnellbatchEigentuemer(secfuncParms(3).Trim)
            tbEigentuemerSchnell.Text = info
            tbEigentuemerSchnell.Visibility = Visibility.Visible
            Protokollausgabe_aller_Parameter(secfuncParms(3).Trim, grund)
        Else
            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
            tbEigentuemerSchnell.Text = "Bitte warten  ...."
            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
            pdfEigentuemerDatei = getEigentuemerDatei(secfuncParms)
            tools.openDocument(pdfEigentuemerDatei)
        End If
    End Sub

    Private Sub btnpuffern_Click(sender As Object, e As RoutedEventArgs)
        If tbpufferinmeter.Text.IsNothingOrEmpty OrElse
            Not IsNumeric(tbpufferinmeter.Text) Then
            MessageBox.Show("Bitte eine Zahl eingeben!")
            Exit Sub
        End If
        '1. in PG  über aktaid und   aktObjID   die geom auslesen
        ' puffern
        ' in suchebene darstellen
        'rbtyp 2 und 3
        Dim pufferinMeter As Double = CDbl(tbpufferinmeter.Text.Replace(".", ","))

        Dim fdaten1 As New clsTabellenDef
        fdaten1.aid = CType(layerActive.aid, String)
        fdaten1.gid = CType(aktObjID, String)
        fdaten1.tab_nr = CType(akttabnr, String)
        sachdatenTools.getSChema(fdaten1)
        l(" fdaten1.aid  " & fdaten1.aid)
        l(" fdaten1.gid  " & fdaten1.gid)
        l(" fdaten1.tab_nr  " & fdaten1.tab_nr)

        Dim puffer_area As Double
        Dim puffererzeugt As Boolean
        Dim acanvas As New clsRange
        Dim geomtype As String = "polygon"
        puffererzeugt = modEW.bildePufferFuerPolygon(aktPolygon, pufferinMeter, fdaten1, puffer_area, acanvas, False)
        Close()
        GC.Collect()
        If puffererzeugt Then
            'MsgBox("Das Puffer-Objekt wurde erzeugt und unter 'Raumbezüge' abgelegt.")
            'aktPolygon.name = "puffer"
            aktFST.normflst.serials.Add(aktPolygon.ShapeSerial)
            Soll_refreshmap = True
            suchObjektModus = "puffer"
            MessageBox.Show("Der Puffer von " & pufferinMeter & " m wurde generiert.  
                             Pufferfläche: " & puffer_area & " qm.  
                             Um den Puffer darzustellen drücken Sie bitte den Button 'Auffrischen'",
                            "Puffer fertig", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
        'MsgBox("Baustelle")

        e.Handled = True
    End Sub

    Private Sub btnLoeschen_Click(sender As Object, e As RoutedEventArgs)
        Dim mesred As MessageBoxResult
        mesred = MessageBox.Show("Möchten Sie das Objekt hier und in der ParadigmaDB löschen?", "Userobjekt löschen", MessageBoxButton.YesNo, MessageBoxImage.Question)
        If mesred = MessageBoxResult.Yes Then
            objektLoeschen()

        End If
        Close()
        e.Handled = True
    End Sub

    Private Shared Sub objektLoeschen()
        'erfolgreich
        l("UserObjekt löschen")
        '1. in PG  über aktaid und   aktObjID   die raumbezugsid und vid auslesen
        Dim rid, vid As Integer
        If Not modEW.getRidVid4ObjId("postgis20", "paradigma_userdata", GisUser.username, aktObjID, rid, vid) Then
            l("fehler in objektLoeschen1 ")
            Exit Sub
        End If
        If vid <> CInt(aktvorgangsid) Then
            MsgBox("Fehler in objektLoeschen. vorgangsid stimmt nicht: " & vid & "/" & aktvorgangsid)
            Exit Sub
        End If
        'erfolgreich
        l("rid, vid des zu löschenden objekts; " & rid & " " & vid)
        '2. in PG löschen 
        If modEW.killRidVidinPG("postgis20", "paradigma_userdata", GisUser.username, aktObjID, rid, vid) Then
            l("rid, vid gelöscht " & rid & " " & vid)
        Else
            l("rid, vid nicht gelöscht " & rid & " " & vid)
        End If
        '3. in Paradigma löschen via raumbezugsid und vorgangsid
        Dim erfolg As Boolean

        If modParadigma.deleteRaumbezug(rid, vid) Then
            erfolg = modParadigma.deleteRaumbezug2all(rid, vid, "raumbezug2vorgang")
            l("raumbezug2vorgang  löschen " & erfolg)
            erfolg = modParadigma.deleteRaumbezug2all(rid, vid, "raumbezug2geopolygon")
            l("raumbezug2geopolygon  löschen " & erfolg)
        Else
            l("löschen erfolglos")
        End If
    End Sub

    Private Sub btnZumParadigmaVorgang_Click(sender As Object, e As RoutedEventArgs)
        tools.paradigmavorgangaufrufen(paradigmavid)
        e.Handled = True
    End Sub

    Private Sub btnEditDB_Click(sender As Object, e As RoutedEventArgs)
        tools.GISeditoraufrufen(layerActive.aid, GisUser.username, CType(aktObjID, String), editTableID.ToString)
        e.Handled = True
    End Sub

    Private Sub btnSchnellNachPDF_Click(sender As Object, e As RoutedEventArgs)
        clsSachdatentools.erzeugeUndOeffneEigentuemerPDF(tbEigentuemerSchnell.Text)
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

    'Private Sub btnbplanBegleitListe_Click(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True
    '    tiBegleit.IsSelected = True

    'End Sub



    Private Sub dgZusatzinfo_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If dgZusatzinfo.SelectedItem Is Nothing Then Exit Sub
        Dim item As IO.FileInfo
        Try
            item = CType(dgZusatzinfo.SelectedItem, IO.FileInfo)
        Catch ex As Exception
            e.Handled = True
            Exit Sub
        End Try
        item = CType(dgZusatzinfo.SelectedItem, IO.FileInfo)
        If item Is Nothing Then
        Else
            OpenDokument(item.FullName)
        End If
    End Sub

    Private Sub cmbFunktionsvorschlaege_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If cmbFunktionsvorschlaege.SelectedItem Is Nothing Then Exit Sub
        Dim myvalx = CType(cmbFunktionsvorschlaege.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        tbKurz.Text = myvals
    End Sub
    Private Sub btnFlurstueckNachParadigma_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Debug.Print(secfuncParms(3).Trim)
        aktFST.normflst.FS = secfuncParms(3).Trim
        aktFST.normflst.splitFS(aktFST.normflst.FS)
        aktFST.normflst.buildFstueckkombi()
        clsFSTtools.holeKoordinaten4Flurstueck(aktFST.normflst.nenner.ToString, WinDetailSucheFST.AktuelleBasisTabelle, aktFST)
        aktFST.box.rangekopierenVon(kartengen.aktMap.aktrange)
        '   MsgBox("kopiert kartengen.aktMap.aktrange  " & kartengen.aktMap.aktrange.toString)
        '-------------
        'wo ist die range?
        clsFSTtools.fstnachParadigmaSpeichern(tbFreitext.Text.Trim, tbKurz.Text.Trim)
        Close()
    End Sub
    Private Sub freiLegende_MouseDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
    End Sub

    Private Sub ScrollViewer_MouseDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Close()
    End Sub

    Private Sub winDBabfrage_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        savePosition()
    End Sub
    Private Sub btnDossier_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        aktFST.punkt = pgisTools.getPunkt4fs(aktFST.normflst.FS)
        Dim utm As New Point
        utm.X = aktFST.punkt.X
        utm.Y = aktFST.punkt.Y
        Dim KoordinateKLickpt As New Point
        KoordinateKLickpt.X = 1
        KoordinateKLickpt.Y = 1
        globCanvasWidth = 2
        globCanvasHeight = 2
        clsSachdatentools.getdossier(utm, layerActive.aid,
                                            CInt(globCanvasWidth), CInt(globCanvasHeight),
                                            KoordinateKLickpt, aktFST.normflst.FS, "flaeche")
    End Sub
End Class
