Imports CefSharp
Imports mgis
Imports System.ComponentModel
Imports System.Data
Imports System.Threading.Tasks
REM /buergergis/php/query_sachdaten.php?option=mapclick&aid=' + aid + '&tab_nr=' + tab_nr + '&gid=' + gid + '&querytitel=' + queryTitel;
REM http://w2gis02.kreis-of.local/buergergis/php/query_sachdaten.php?option=mapclick&aid=348&tab_nr=1&gid=4&querytitel='bla'
REM https://buergergis.kreis-offenbach.de/buergergis/php/query_sachdaten.php?option=mapclick&aid=348&tab_nr=1&gid=4&querytitel='bla'
Public Class winDBabfrage
    Public legdatei, dokdatei, dokHtml As String
    Property Datenbank_als_HTML As Boolean = True
    Property _rtfdatei As String
    'Property _flow As FlowDocument
    Property _modus As String
    Property _buttonINfostring As String = ""
    Property _isUserLayer As Boolean
    Property secfuncParms As String()
    Property Soll_refreshmap As Boolean = False
    Property paradigmavid As String = ""
    Property editTableID As String = ""
    Public Property EigentuemerPDF As String = ""
    Private ladevorgangAbgeschlossen As Boolean = False
    Dim fensterZaehler As Integer = 0
    Private bufferid As Integer = 0
    Property _isOSsuche As Boolean = False

    'Sub New(rtfdatei As String, modus As String, buttonINfostring As String, isUserLayer As Boolean, _fensterZaehler As Integer, isOSsuche As Boolean,
    '        Optional flow As FlowDocument = Nothing)
    Sub New(rtfdatei As String, modus As String, buttonINfostring As String, isUserLayer As Boolean, _fensterZaehler As Integer, isOSsuche As Boolean)
        ' This call is required by the designer.
        InitializeComponent()
        _rtfdatei = rtfdatei
        _modus = modus 'datei oder text, dabei ist text die DB abfrage
        '_flow = flow
        _buttonINfostring = buttonINfostring
        _isUserLayer = isUserLayer
        fensterZaehler = _fensterZaehler
        _isOSsuche = isOSsuche
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub windbabfrage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            Top = 1
            e.Handled = True
            btnDossier.Visibility = Visibility.Collapsed
            l("windbabfrage_Loaded -------------------")
            l("buttonINfostring " & _buttonINfostring)
            l("fdaten " & os_tabelledef.gid)
            bufferid = CInt(os_tabelledef.gid)

            'If Datenbank_als_HTML Then
            freiLegende.Visibility = Visibility.Collapsed
                WebBrowser1.Visibility = Visibility.Visible
                WebBrowser1.JavascriptObjectRepository.Register("boundAsync", New BoundObject(), True)
                CefSharpSettings.LegacyJavascriptBindingEnabled = True
                WebBrowser1.LoadHtml(_rtfdatei, myglobalz.myfakeurl)
            'Else

            '    Debug.Print(CType(myglobalz.gesamtSachdatList.Count, String))
            '    freiLegende.Visibility = Visibility.Visible
            '    freiLegende.Document = _flow
            '    WebBrowser1.Visibility = Visibility.Collapsed
            'End If
            btnZumParadigmaVorgang.Visibility = Visibility.Collapsed
            tbEigentuemerSchnell.Visibility = Visibility.Collapsed
            spDBObjToolbar.Visibility = Visibility.Collapsed
            gbEigentuemer.Visibility = Visibility.Collapsed
            imgEditDB.Visibility = Visibility.Collapsed
            spDB.Width = 900
            addButtons(gesamtSachdatList)

            Dim rtfTextDoku As String
            If _buttonINfostring.Trim = String.Empty Then
                'If _buttonINfostring.ToLower.Contains("eigentuemer") Then
                'gbEigentuemer.Visibility = Visibility.Collapsed
                'spDB.Width = 900
                'l("eigentuemer nicht erlaubt")
                'End If
                'WebBrowser1.Width = 900
            Else
                If _buttonINfostring.ToLower.Contains("bplanbegleit") Then
                    l("begleitmaterial zu beplan")
                    Dim gemarkung As String = "", pdf As String = "", verzeichnis As String
                    ModsachdatenTools.bplanbegleitInfoAufloesen(_buttonINfostring, gemarkung:=gemarkung, pdf:=pdf)
                    verzeichnis = ModsachdatenTools.bplanbegleitInfoCalcDirectory(gemarkung, pdf,
                                                                               myglobalz.serverUNC & "fkat")
                    'Dim begleitfilelist As New List(Of IO.FileInfo)
                    Dim begleitfileURLs As New List(Of clsFlurauswahl)
                    'If iminternet Or CGIstattDBzugriff Then
                    begleitfileURLs = clsSachdatentools.getBegleitplanFilelisteInternet(pdf, gemarkung, verzeichnis)
                    'Else
                    '    begleitfileURLs = clsSachdatentools.getBegleitplanFilelisteIntranet(pdf, verzeichnis, gemarkung)
                    'End If

                    If begleitfileURLs.Count > 0 Then
                        dgZusatzinfo.Visibility = Visibility.Visible
                        dgZusatzinfo.IsEnabled = True
                    Else
                        dgZusatzinfo.Visibility = Visibility.Collapsed
                        dgZusatzinfo.IsEnabled = False
                    End If
                    Debug.Print("")
                    dgZusatzinfo.DataContext = begleitfileURLs
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
                    If Not iminternet Then btnDossier.Visibility = Visibility.Visible
                    createButtonEigentuemer()
                    spDB.Width = 400
                    'Width = 1200 ' wird unten geregelt
                    WebBrowser1.Height = Height - 200
                    tbWeitergabeVerbot.Text = albverbotsString
                    tbEigentuemerSchnell.Visibility = Visibility.Visible
                End If

                If _buttonINfostring.ToLower.Contains("paradigmavid") Then
                    l("paradigmavid   erlaubt")
                    paradigmavid = getParadigmaVID(_buttonINfostring, 3)
                    If Not paradigmavid.IsNothingOrEmpty Then

                        If paradigmavid = "0" OrElse paradigmavid = "[paradigmavid]" Then
                            btnZumParadigmaVorgang.Visibility = Visibility.Collapsed
                            imgEditDB.Visibility = Visibility.Collapsed
                        Else
                            btnZumParadigmaVorgang.Visibility = Visibility.Visible
                            btnZumParadigmaVorgang.Content = "Zum Vorgang <" & paradigmavid & "> in Paradigma"
                            'createButtonEigentuemer()
                            'tbWeitergabeVerbot.Text = "Hier könn"
                        End If
                    End If
                    If ModsachdatenTools.userIstLayerEditor(GisUser.nick, layerActive.aid) Then
                        editTableID = geteditTableID(layerActive.aid, _buttonINfostring)
                        imgEditDB.Visibility = Visibility.Visible
                    End If
                End If
            End If

            If _modus = "text" Then
                initGemarkungsCombo()
                If layerActive.titel.ToLower.StartsWith("auswahl: ") Then
                    'tempActivelayer = CType(layerActive.Clone, clsLayerPres)
                    tempActivelayer.aid = CInt(os_tabelledef.aid)
                Else
                    tempActivelayer = CType(layerActive.Clone, clsLayerPres)
                End If
                Title = "DB-Anzeige (Aid: " & tempActivelayer.aid & ", " & bufferid & ")"

                'If _flow IsNot Nothing Then
                '    rtfTextDoku = textmodus(_flow)
                'End If
                Width = 950
                Height = 990
                spDBObjToolbar.Visibility = Visibility.Visible
                If _isUserLayer Then
                    btnLoeschen.Visibility = Visibility.Visible
                Else
                    btnLoeschen.Visibility = Visibility.Collapsed
                End If

                dokdatei = nsMakeRTF.rtf.makeDokuHtml(tempActivelayer, dokHtml, tempActivelayer.aid)
                legdatei = nsMakeRTF.rtf.makeftlLegende4Aid(tempActivelayer, "html", dokHtml)

                If legdatei.IsNothingOrEmpty Then
                    rtfTextDoku = "Keine Legende vorhanden"
                    'tidok.IsSelected = True
                Else
                    Dim fi As IO.FileInfo
                    fi = New IO.FileInfo(legdatei)
                    If fi.Exists Then
                        wbleg.Navigate("file:///" & legdatei)
                    End If
                End If
                If _buttonINfostring.ToLower.Contains("eigentümer") Then
                Else
                    Width = 600
                    WebBrowser1.Height = Height - 200
                End If
                'MsgBox(WebBrowser1.Width & "," & WebBrowser1.Height)
                'WebBrowser1.Width = 600

            Else
                tilegende.Visibility = Visibility.Collapsed
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
            If fensterZaehler > 0 Then
                Me.Top = clsToolsAllg.setPosition("diverse", "dbabfrageformpositiontop", Me.Top) + fensterZaehler * 25
                Me.Left = clsToolsAllg.setPosition("diverse", "dbabfrageformpositionleft", Me.Left) + fensterZaehler * 20
            Else
                Me.Top = clsToolsAllg.setPosition("diverse", "dbabfrageformpositiontop", Me.Top)
                Me.Left = clsToolsAllg.setPosition("diverse", "dbabfrageformpositionleft", Me.Left)
            End If

            ladevorgangAbgeschlossen = True
        Catch ex As Exception
            l("fehler in windbabfrage_Loaded " & _buttonINfostring & "_", ex)
        End Try
    End Sub


    Private Sub addButtons(sdObj As List(Of clsSachdaten))
        Dim tempdat As String
        Debug.Print(sdObj.Count.ToString)
        Dim numbOfRows As Integer = sdObj.Count
        For i = 0 To numbOfRows - 1
            tempdat = sdObj(i).feldinhalt
            If tempdat.ToLower.EndsWith("/.pdf") Then
                tempdat = ""
            End If
            If tempdat.ToLower.EndsWith(".pdf") Or
                       tempdat.ToLower.EndsWith(".application") Or
                       tempdat.ToLower.EndsWith(".jpg") Or
                       tempdat.ToLower.EndsWith(".tiff") Or
                       tempdat.ToLower.EndsWith(".html") Then
                createNewButton(sdObj, i)
            End If
            'If tempdat.Contains("http") AndAlso (Not tempdat.ToLower.EndsWith("/.pdf")) Then

            '    'If clsSachdatentools.istBaulastTiff(tempdat) Then
            '    '    hlink.Inlines.Add(" " & Environment.NewLine & "")
            '    'Else
            '    '    'hlink.Inlines.Add("Hyperlink: " & Environment.NewLine & tempdat)
            '    '    hlink.FontSize = 8
            '    'End If
            '    createNewButton(sdObj, tempdat, i)
            '    hlink.ToolTip = tempdat
            '    Try
            '        hlink.NavigateUri = New Uri(tempdat)
            '        AddHandler hlink.Click, AddressOf linkausfuehren
            '        AddHandler hlink.MouseDown, AddressOf linkausfuehren
            '    Catch ex As Exception
            '        l("fehler in createTable link.NavigateUri: url unbrauchbar " & tempdat)
            '    End Try

            '    hlink.Cursor = Cursors.Hand


            '    currentRow.FontSize = grossfont
            '    currentRow.FontStyle = FontStyles.Italic
            '    currentRow.FontWeight = FontWeights.Black
            '    Dim runn As New Run(sdObj(i).neuerFeldname)
            '    runn.FontSize = 12

            '    If hlink.FontSize = 8 Then
            '        currentRow.Cells.Add(New TableCell(New Paragraph(runn)))
            '    Else
            '        currentRow.Cells.Add(New TableCell(New Paragraph(runn)))
            '    End If
            '    currentRow.Cells.Add(New TableCell(New Paragraph((hlink))))
            '    currentRow.Cells.Add(New TableCell(New Paragraph(New Run(" "))))
            '    currentRow.FontStyle = FontStyles.Normal
            '    currentRow.FontWeight = FontWeights.Normal
            'Else
            '    If sdObj(i).neuerFeldname = "neueTabelle" Then
            '        sdObj(i).neuerFeldname = "Zusatztabelle"
            '        currentRow.FontSize = mittelfont
            '        currentRow.FontStyle = FontStyles.Italic
            '        currentRow.FontWeight = FontWeights.Black
            '    Else
            '        currentRow.FontSize = kleinfont
            '        currentRow.FontStyle = FontStyles.Normal
            '        currentRow.FontWeight = FontWeights.Normal
            '    End If
            '    currentRow.Cells.Add(New TableCell(New Paragraph(New Run(sdObj(i).neuerFeldname))))
            '    currentRow.Cells.Add(New TableCell(New Paragraph(New Run(tempdat))))
            '    currentRow.Cells.Add(New TableCell(New Paragraph(New Run(" "))))
            'End If
            If tempdat.IsNothingOrEmpty Then tempdat = " "
        Next

    End Sub

    Private Sub createNewButton(ByRef sdObj As List(Of clsSachdaten), i As Integer)
        Dim mbutton As New Button
        mbutton.IsEnabled = True
        mbutton.Content = sdObj(i).neuerFeldname & " Datei zeigen"
        mbutton.FontSize = 12
        mbutton.ToolTip = sdObj(i).feldinhalt
        mbutton.Height = 30
        mbutton.Background = Brushes.Transparent
        mbutton.Foreground = Brushes.Blue
        mbutton.HorizontalAlignment = HorizontalAlignment.Center
        mbutton.Margin = New Thickness(5)
        mbutton.Padding = New Thickness(5)
        mbutton.Tag = sdObj(i).feldinhalt
        If sdObj(i).feldinhalt.ToLower.EndsWith(".application") Then
            mbutton.Content = sdObj(i).neuerFeldname '& " starten"
            sdObj(i).feldinhalt = ""
            sdObj(i).feldinhalt = ""
        End If
        Try
            'mbutton.NavigateUr = New Uri(tempdat)
            AddHandler mbutton.Click, AddressOf nsMakeRTF.rtf.buttonausfuehrenAsync
            AddHandler mbutton.MouseDown, AddressOf nsMakeRTF.rtf.buttonausfuehrenAsync
        Catch ex As Exception
            l("fehler in createTable link.NavigateUri: url unbrauchbar " & sdObj(i).feldinhalt)
        End Try
        spButtonleiste.Children.Add(mbutton)
    End Sub



    'Private Sub createNewButton(titel As String)
    '    Try
    '        Dim mbutton As New Button
    '        mbutton.Width = 100
    '        mbutton.Height = 40
    '        mbutton.Content = titel
    '        mbutton.ToolTip = titel
    '        AddHandler mbutton.Click, AddressOf buttonausfuehrenAsync
    '        AddHandler mbutton.MouseDown, AddressOf buttonausfuehrenAsync
    '        spButtonleiste.Children.Add(mbutton)
    '    Catch ex As Exception
    '        l("fehler addButtons " & _buttonINfostring & "_" ,ex)
    '    End Try
    'End Sub

    Private Sub savePosition()
        Try
            userIniProfile.WertSchreiben("diverse", "dbabfrageformpositiontop", CType(Me.Top, String))
            userIniProfile.WertSchreiben("diverse", "dbabfrageformpositionleft", CType(Me.Left, String))
        Catch ex As Exception
            l("fehler in saveposition  windb", ex)
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

    Private Function getParadigmaVID(_buttonINfostring As String, index As Integer) As String
        Dim a() As String
        Try
            l("getParadigmaVID---------------------- anfang")
            'specfunc,ParadigmaVID,startParadigma,[paradigmavid] 
            a = _buttonINfostring.Split(","c)
            l(a(index))
            l("getParadigmaVID---------------------- ende")
            Return a(index)
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
            l("fehler in createButton ", ex)
        End Try
    End Sub

    'Private Function textmodus(_text As FlowDocument) As String
    '    Dim neuflow As New FlowDocument
    '    'If myglobalz.Datenbank_als_HTML Then
    '    'Else
    '    neuflow = _flow
    '    freiLegende.Document = neuflow
    '    'End If
    '    Return ""
    'End Function

    Private Function dateimodus() As String
        Dim rtfTextDoku As String
        Using datei As IO.StreamReader = New IO.StreamReader(_rtfdatei)
            rtfTextDoku = datei.ReadToEnd
        End Using
        Dim documentBytes = Text.Encoding.UTF8.GetBytes(rtfTextDoku)
        Dim reader = New System.IO.MemoryStream(documentBytes) With {
            .Position = 0
        }
        freiLegende.SelectAll()
        freiLegende.Selection.Load(reader, DataFormats.Rtf)
        Return rtfTextDoku
    End Function

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Datenbank_als_HTML Then
            Clipboard.Clear()
            'https://blogs.msdn.microsoft.com/jmstall/2007/01/21/copying-html-on-the-clipboard/
            Dim temp = "Version:1.0" & Environment.NewLine &
                "StartHTML:000125" &
                "EndHTML:000260" &
                "StartFragment:000209" &
                "EndFragment:000222" &
                "SourceURL:file:///C:/temp/test.htm" &
                nsMakeHTML.clsCreateHtmlTable.htmlDateiString
            Clipboard.SetText(temp, TextDataFormat.Html)
        Else
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

        End If
        GC.Collect()
        MsgBox("Sie können den Text jetzt mit Strg-v  in ein Word-Dokument einfügen!",, "Zwischenablage")

    End Sub


    Private Sub btnSpecFunc_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Select Case secfuncParms(2).ToLower
            Case "geteigentuemerdatei"
                If tbGrund.Text Is Nothing OrElse tbGrund.Text.Trim.Length < 2 Then
                    setzeGrundFuerEigentuemerabfrage(tbGrund.Text)
                    MsgBox("Bitte eine Begründung (z.B. das Aktenzeichen) eingeben!")
                    Exit Sub
                End If
                Dim strLage As String = ""
                strLage = clsSachdatentools.getlage(secfuncParms(3).Trim)
                tbEigentuemerSchnell.Text = geteigentuemerDatei2(tbGrund.Text, secfuncParms(3).Trim)
                GrundFuerEigentuemerabfrage = tbGrund.Text
                strLage = strLage & Environment.NewLine
                EigentuemerPDF = clsSachdatentools.erzeugeUndOeffneEigentuemerPDF(tbEigentuemerSchnell.Text, strLage)
                OpenDokument(EigentuemerPDF)
                If STARTUP_mgismodus = "paradigma" Then
                    spEigentNotizUebernehmen.Visibility = Visibility.Visible
                    spEigentNotizUebernehmen.IsEnabled = True
                End If

                If cbNichtSchliessen.IsChecked Then
                Else
                    Close()
                End If

        End Select
    End Sub
    Function geteigentuemerDatei2(grund As String, fs As String) As String
        If Not GisUser.istalbberechtigt Then
            MsgBox("Der User: " & GisUser.nick & " ist nicht berechtigt auf die Eigentümerdaten zuzugreifen. Programmende")
            Return ""
        End If
        'If Not NSfstmysql.ADtools.istUserAlbBerechtigt(  GisUser.nick, fdkurz) Then
        '    MsgBox("Der User: " &   GisUser.nick & " ist nicht berechtigt auf die Eigentümerdaten zuzugreifen. Programmende")
        '    Exit Sub
        'End If
        Dim pdfEigentuemerDatei As String
        Dim info As String = ""
        If cbSchnellEigentuemer.IsChecked Then

            info = "Eigentümer in Kurzform: " & Environment.NewLine &
                                        getSchnellbatchEigentuemer(fs)
            tbEigentuemerSchnell.Visibility = Visibility.Visible
            Protokollausgabe_aller_Parameter(secfuncParms(3).Trim, grund)
            Return info
        Else
            'faktisch ausgeschaltet
            Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents
            tbEigentuemerSchnell.Text = "Bitte warten  ...."
            Dispatcher.Invoke(Threading.DispatcherPriority.Background, Function() 0) 'Doevents
            pdfEigentuemerDatei = getEigentuemerDatei(secfuncParms)
            tools.openDocument(pdfEigentuemerDatei)
            Return "PDF wurde erzeugt"
        End If
    End Function

    Private Sub btnpuffern_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim lokaleAid As Integer = 0
        Dim lokalebufferid As Integer = 0
        Dim lokaleAkttabnr As Integer = 0
        Dim pufferstring As String
        pufferstring = getPufferString()
        If pufferstring.IsNothingOrEmpty Then
            MessageBox.Show("Bitte eine Zahl eingeben!")
            Exit Sub
        End If

        '1. in PG  über aktaid und   aktObjID   die geom auslesen
        ' puffern
        ' in suchebene darstellen
        'rbtyp 2 und 3

        If _isOSsuche Then
            lokaleAid = CInt(os_tabelledef.aid)
            lokalebufferid = CInt(os_tabelledef.gid)
            lokaleAkttabnr = CInt(os_tabelledef.tab_nr)
        Else
            lokaleAid = layerActive.aid
            lokalebufferid = bufferid
            lokaleAkttabnr = akttabnr
        End If
        Dim pufferinMeter As Double = CDbl(pufferstring.Replace(".", ","))

        Dim fdaten1 As New clsTabellenDef With {
            .aid = CType(lokaleAid, String),
            .gid = CType(lokalebufferid, String),
            .tab_nr = CType(lokaleAkttabnr, String)
        }
        fdaten1 = ModsachdatenTools.getSChemaDB(lokaleAid, lokaleAkttabnr)

        If fdaten1 Is Nothing Then
            fdaten1.aid = CType(lokaleAid, String)
            fdaten1.gid = CType(lokalebufferid, String)
            fdaten1.tab_nr = CType(lokaleAkttabnr, String)
        End If
        fdaten1.aid = CType(lokaleAid, String)
        fdaten1.gid = CType(lokalebufferid, String)
        l(" fdaten1:  " & fdaten1.toStringa(";"))

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
            suchObjektModus = suchobjektmodusEnum.pufferObjektDarstellen
            MessageBox.Show("Der Puffer von " & pufferinMeter & " m wurde generiert.  
                             Pufferfläche: " & puffer_area & " qm.  
                             Um den Puffer darzustellen drücken Sie bitte den Button 'Auffrischen'",
                            "Puffer fertig", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
        'MsgBox("Baustelle")


    End Sub

    Private Function getPufferString() As String
        Dim result As String = ""
        Try
            l(" MOD getPufferString anfang")
            result = InputBox("Bitte den Abstand in Meter angeben. Dezimaltrennung mit '.'", "Pufferabstand eingeben", "11.5")
            If IsNumeric(result) Then
                Return result
            End If
            l(" MOD getPufferString ende")
            Return ""
        Catch ex As Exception
            l("Fehler in getPufferString: " & ex.ToString())
            Return ""
        End Try
    End Function

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
        If Not modEW.getRidVid4ObjId("postgis20", "paradigma_userdata", GisUser.nick, aktObjID, rid, vid) Then
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
        If modEW.killRidVidinPG("postgis20", "paradigma_userdata", GisUser.nick, aktObjID, rid, vid) Then
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

    'Private Sub btnEditDB_Click(sender As Object, e As RoutedEventArgs)
    '    tools.GISeditoraufrufen(layerActive.aid, GisUser.nick, CType(aktObjID, String), editTableID.ToString)
    '    e.Handled = True
    'End Sub

    Private Sub btnSchnellNachPDF_Click(sender As Object, e As RoutedEventArgs)
        clsSachdatentools.erzeugeUndOeffneEigentuemerPDF(tbEigentuemerSchnell.Text, aktadr.defineAbstract)
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



    Sub dgZusatzinfo_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If dgZusatzinfo.SelectedItem Is Nothing Then Exit Sub
        Dim item As clsFlurauswahl
        Try
            item = CType(dgZusatzinfo.SelectedItem, clsFlurauswahl)
        Catch ex As Exception
            e.Handled = True
            Exit Sub
        End Try
        item = CType(dgZusatzinfo.SelectedItem, clsFlurauswahl)
        If item Is Nothing OrElse item.displayText.IsNothingOrEmpty Then
            Exit Sub
        Else
            'If iminternet Or CGIstattDBzugriff Then
            'Dim targetroot As String = "c:\ptest\bplankat\cache\bplaene\bplan"
            Dim targetroot As String = strGlobals.localDocumentCacheRoot & "\bplankat\cache\bplaene\bplan"
            Dim targetdir As String = targetroot & "" & item.temp & "\" & item.temp2 & "\"
            Dim zieldatei = targetroot & "" & item.temp & "\" & item.temp2 & "\" & item.displayText
            If clsSachdatentools.schonImCache(targetdir, item.displayText, True) Then
                OpenDokument(zieldatei)
            Else
                Dim r As Boolean = meineHttpNet.down(item.nenner, item.displayText, targetroot & "" & item.temp & "\" & item.temp2)
                If r Then
                    'mylog((Str()) & " ok")
                    'Threading.Thread.Sleep(2000)
                    OpenDokument(zieldatei)
                Else
                    'mylog(Str() & " NICHT erhalten")
                    'Return False
                End If
            End If
            'Else
            '    OpenDokument(item.nenner)
            'End If
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
        If fensterZaehler < 1 Then savePosition()
    End Sub

    'Private Sub btnRTFdatei2Word_Click(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True
    '    Dim tdatei As String = ""
    '    tdatei = IO.Path.Combine(strGlobals.localDocumentCacheRoot, "temp")
    '    clsStartup.createDir(tdatei)
    '    IO.File.WriteAllText(tdatei & "\temp.html", nsMakeHTML.clsCreateHtmlTable.htmlDateiString, encoding:=Text.Encoding.UTF8)
    '    If iminternet Then
    '        OpenDokument(tdatei & "\temp.html") 'openoffice
    '    Else
    '        OpenWithArguments("WINWORD.EXE", tdatei & "\temp.html")
    '    End If
    'End Sub

    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        Dim tdatei As String = ""
        tdatei = IO.Path.Combine(strGlobals.localDocumentCacheRoot, "temp")
        clsStartup.createDir(tdatei)
        tdatei = tdatei & "\temp" & clsString.date2string(Now, 1) & ".html"
        IO.File.WriteAllText(tdatei, nsMakeHTML.clsCreateHtmlTable.htmlDateiString, encoding:=Text.Encoding.UTF8)
        If iminternet Then
            strGlobals.meinWordProcessor = myglobalz.userIniProfile.WertLesen("software", "wordexefullpath")
            If strGlobals.meinWordProcessor.IsNothingOrEmpty Then
                OpenDokument(tdatei) 'openoffice 
            Else
                'strGlobals.meinWordProcessor = "C:\Program Files (x86)\Windows NT\Accessories\wordpad.exe"
                OpenWithArguments(strGlobals.meinWordProcessor, tdatei) 'openoffice
            End If
        Else
            OpenWithArguments(strGlobals.meinWordProcessor, tdatei)
        End If
    End Sub

    Private Sub ImageLegende_MouseDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        Dim tdatei As String = dokdatei
        'tdatei = IO.Path.Combine(strGlobals.localDocumentCacheRoot, "temp")
        'clsStartup.createDir(tdatei)
        'tdatei = tdatei & "\temp" & clsString.date2string(Now, 1) & ".html"
        'IO.File.WriteAllText(tdatei, nsMakeHTML.clsCreateHtmlTable.htmlDateiString, encoding:=Text.Encoding.UTF8)
        If iminternet Then
            OpenDokument(tdatei) 'openoffice
        Else
            OpenWithArguments(strGlobals.meinWordProcessor, tdatei)
        End If
    End Sub

    Private Sub Image_MouseDown_1(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        tools.GISeditoraufrufen(layerActive.aid, GisUser.nick, CType(bufferid, String), editTableID.ToString)

    End Sub

    Private Sub cmbSelVal_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnDossier_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        aktFST.punkt = pgisTools.getPunkt4fs("flurkarte.basis_f where fs='" & aktFST.normflst.FS & "'")
        Dim utm As New Point With {
                                    .X = aktFST.punkt.X,
                                    .Y = aktFST.punkt.Y
                                    }
        Dim KoordinateKLickpt As New Point With {
            .X = 1,
            .Y = 1
        }
        globCanvasWidth = 2
        globCanvasHeight = 2
        clsSachdatentools.getdossier(utm, layerActive.aid,
                                            CInt(globCanvasWidth), CInt(globCanvasHeight),
                                            KoordinateKLickpt, aktFST.normflst.FS, "flaeche")
    End Sub
    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub
End Class
