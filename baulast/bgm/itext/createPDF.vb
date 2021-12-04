'Imports System.Text
'Imports mgis

'Module createPDF
'    Private PaintPngScale As Double = 2.0

'    Public Property _breite As String
'    Public Property _hoehe As String
'    Public Property aufruf As String
'    Public Property PicturesPath As String

'    Function dateiNachMeineBilderKopieren(pdfdatei As String) As String
'        Dim neuername As String
'        Try
'            l("dateiNachMeineBilderKopieren---------------------- anfang")

'            Dim fi As New IO.FileInfo(pdfdatei)
'            neuername = PicturesPath & "\" & fi.Name
'            fi.CopyTo(neuername, True)
'            l("dateiNachMeineBilderKopieren---------------------- ende")
'            Return neuername

'        Catch ex As Exception
'            l("Fehler in dateiNachMeineBilderKopieren: " & ex.ToString())
'            Return pdfdatei
'        End Try
'    End Function

'    Public Function makeandloadPDF(masstabsModus As String, PDF_PrintRange As clsRange,
'                              druckmasstab As Double, ausrichtung As String, pdf_bemerkung As String, pdf_ort As String,
'                              paintPNG As Boolean, hochaufloesend As Boolean, isa4Formatchecked As Boolean,
'                                   schnelldruck As Boolean, ByRef remoteFileURL As String, mitsuchobjekt As Boolean,
'                                   ByRef localfileFullname As String) As Boolean
'        'pdfmodus : paintPNG,hochaufloesend,normal,schnelldruck
'        l("in makeandloadPDF--------------------------")
'        PicturesPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
'        Dim result As String
'        Dim hinweis As String = ""

'        Try
'            l(" MOD makeandloadPDF anfang")
'            calcWeightHeight(ausrichtung, hochaufloesend, isa4Formatchecked)
'            If paintPNG Then scale4Paint()
'            PDF_PrintRange = bildePDFRange(masstabsModus, PDF_PrintRange)

'            result = genPDFonServer(paintPNG, hochaufloesend, mitsuchobjekt, hinweis, myglobalz.enc, 5000, result,
'                                     CInt(_breite), CInt(_hoehe), isa4Formatchecked, PDF_PrintRange, pdf_ort, pdf_bemerkung, druckmasstab
'                                     )
'            If result.IsNothingOrEmpty Then
'                l("fehler bei der erzeugung des mapfiles:!")
'                'MessageBox.Show("Mapfile konnte nicht erzeugt werden. Bitte an Admin wenden 4434", "Double Panic error")
'                Return False
'            End If
'            Dim ergebnis As String
'            'If iminternet Or newdeal Then
'            remoteFileURL = extractPDFfileURL(paintPNG, hochaufloesend, result)
'            'Else
'            '    aufrufMapserv = bildeaufrufMapserverPDF(_breite, _hoehe, PDF_PrintRange, druckmasstab, pdf_bemerkung, pdf_ort, mapfileBILD, isa4Formatchecked)
'            '    calcWartezeiten(hochaufloesend, meinHttpTimeout, dauer)
'            '    ergebnis = meineHttpNet.meinHttpJob(ProxyString, aufrufMapserv, hinweis, myglobalz.enc, meinHttpTimeout)
'            '    l("hinweis " & hinweis) : l("------------------------aufruf ergebnis'")
'            '    If ergebnis.IsNothingOrEmpty Then
'            '        l("Fehler bei der Erzeugung der PDF-Dateitimeout ")
'            '        MessageBox.Show("FEhler bei erzeugung der PDF-Datei, timeout: " & meinHttpTimeout & hinweis, "Fehler")
'            '        Return False
'            '    End If
'            '    l(ergebnis)
'            '    remoteFileURL = getPDFdateiName(paintPNG, hochaufloesend, ergebnis)
'            'End If
'            If remoteFileURL.Length < 3 Then
'                l("Fehler bei der Erzeugung der PDF-Datei ergebnis: " & ergebnis)
'                MessageBox.Show("Fehler bei der Erzeugung der PDF-Datei", "Fehler")
'                Return False
'            End If

'            Dim lokalerDateiName, lokalesVerzeichnis As String
'            If paintPNG And Not hochaufloesend Then
'                lokalerDateiName = getNameOfDownloadFile(remoteFileURL)
'                lokalesVerzeichnis = IO.Path.Combine(strGlobals.localDocumentCacheRoot & "\png")
'                localfileFullname = lokalesVerzeichnis & "\" & lokalerDateiName
'                If meineHttpNet.down(remoteFileURL, lokalerDateiName, lokalesVerzeichnis) Then
'                    Return True
'                Else
'                    Return False
'                End If
'            End If
'            If hochaufloesend Then
'                Dim pngdatei As String
'                'https://buergergis.kreis-offenbach.de/cache/MS154952638210828.png
'                pngdatei = remoteFileURL
'                lokalerDateiName = getNameOfDownloadFile(remoteFileURL)
'                lokalesVerzeichnis = IO.Path.Combine(strGlobals.localDocumentCacheRoot & "\png")
'                If meineHttpNet.down(remoteFileURL, lokalerDateiName, lokalesVerzeichnis) Then
'                    Threading.Thread.Sleep(1000)
'                    pngdatei = lokalesVerzeichnis & "\" & lokalerDateiName
'                    'nun PNG in ein PDF einbetten:
'                    lokalerDateiName = lokalerDateiName.Replace(".png", ".pdf")
'                    lokalesVerzeichnis = IO.Path.Combine(strGlobals.localDocumentCacheRoot & "\pdf")

'                    localfileFullname = lokalesVerzeichnis & "\" & lokalerDateiName

'                    selberPDFerzeugen(pngdatei, CInt(_breite), CInt(_hoehe), localfileFullname, isa4Formatchecked)
'                    remoteFileURL = localfileFullname
'                    'OpenDokument(localfileFullname)
'                    Return True
'                Else
'                    Process.Start(remoteFileURL)
'                    Return False
'                End If
'            End If
'            If schnelldruck Then
'                lokalerDateiName = getNameOfDownloadFile(remoteFileURL)
'                lokalesVerzeichnis = IO.Path.Combine(strGlobals.localDocumentCacheRoot & "\pdf")
'                If meineHttpNet.down(remoteFileURL, lokalerDateiName, lokalesVerzeichnis) Then
'                    Dim param As String
'                    param = "start /B ""Drucken"" """ & strGlobals.pdfReader & """ /t """ & lokalesVerzeichnis & "\" & lokalerDateiName & """"
'                    localfileFullname = strGlobals.localDocumentCacheRoot & "\print.bat"
'                    My.Computer.FileSystem.WriteAllText(localfileFullname, param, False, enc)
'                    Return True
'                Else
'                    l("fehler bei schnelldruck: Download gescheidert! ")
'                    Return False
'                End If
'            End If
'            'zuletzt die normale PDFausgabe
'            lokalerDateiName = getNameOfDownloadFile(remoteFileURL)
'            lokalesVerzeichnis = IO.Path.Combine(strGlobals.localDocumentCacheRoot & "\pdf")
'            localfileFullname = lokalesVerzeichnis & "\" & lokalerDateiName

'            If meineHttpNet.down(remoteFileURL, lokalerDateiName, lokalesVerzeichnis) Then
'                remoteFileURL = localfileFullname
'                Return True
'            Else
'                Process.Start(remoteFileURL)
'                Return False
'            End If
'            l(" MOD makeandloadPDF ende")
'            Return True
'        Catch ex As Exception
'            l("Fehler in makeandloadPDF a: " & ex.ToString())
'            Return False
'        End Try
'    End Function

'    Private Function genPDFonServer(paintPNG As Boolean, hochaufloesend As Boolean,
'                                mitsuchobjekt As Boolean, hinweis As String,
'                                enc As Encoding, timeout As Integer, mapfileUR As String, breite As Integer, hoehe As Integer, isa4 As Boolean,
'                                PDF_PrintRange As clsRange, pdf_bemerkung As String, pdf_ort As String, druckmasstab As Double) As String
'        Dim ergebnis As String = ""
'        Try
'            l(" MOD genPDFonServer anfang")
'            l(" paintPNG " & paintPNG)
'            l(" hochaufloesend " & hochaufloesend)
'            l(" mitsuchobjekt " & mitsuchobjekt)
'            l(" aktFST.name " & aktFST.name)
'            l(" breite " & breite)
'            l(" hoehe " & hoehe)
'            l(" isa4 " & isa4)
'            l(" PDF_PrintRange " & PDF_PrintRange.toString)
'            l(" pdf_bemerkung " & pdf_bemerkung)
'            l(" pdf_ort " & pdf_ort)
'            l(" druckmasstab " & druckmasstab)
'            l(" aktFST.abstract " & aktFST.abstract)

'            Dim aufrufMapfileBuilder, result As String
'            'If iminternet Or newdeal Then
'            aufrufMapfileBuilder = genaufrufBuildMapfile(paintPNG, hochaufloesend, mitsuchobjekt,
'                                                             CBool(aktFST.name.IsNothingOrEmpty), breite, hoehe, isa4,
'                                                             PDF_PrintRange, pdf_bemerkung, pdf_ort, druckmasstab, aktFST.name, aktFST.abstract)
'            l("aufrufMapfileBuilder " & aufrufMapfileBuilder)
'            aufrufMapfileBuilder = strGlobals.attachCredentials2aufruf(aufrufMapfileBuilder) : l("aufrufMapfileBuilder 2" & aufrufMapfileBuilder)
'            aufrufMapfileBuilder = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?modus=buildmapfilepdf" & aufrufMapfileBuilder
'            l("aufrufMapfileBuilder3 " & aufrufMapfileBuilder)
'            result = meineHttpNet.meinHttpJob(ProxyString, aufrufMapfileBuilder, hinweis, enc, 129000)
'            If result Is Nothing Then

'                l("Fehler  in genPDFonServer result is nothing ")
'                ergebnis = "" '"Fehler in genPDFonServer result is nothing"
'            End If
'            result = result.Trim
'            If result.IsNothingOrEmpty Then
'                '   l("Fehler in genPDFonServer bF:result.IsNothingOrEmpty ")
'                ergebnis = ""
'            Else
'                ergebnis = result
'            End If
'            'Else
'            '    mapfileStringtext = createMapfileStringPDF_DB(paintPNG, hochaufloesend, mitsuchobjekt)
'            '    StringbuilderAussschreiben(mapfileStringtext, mapfileUR)
'            '    mapfileBILD = mapfileUR
'            'End If
'            l(" MOD genPDFonServer ende ergebnis:" & ergebnis)
'            Return ergebnis
'        Catch ex As Exception
'            l("Fehler in genPDFonServer: " & ex.ToString())
'            ergebnis = ""
'            Return ""
'        End Try
'    End Function

'    'Private Function setPDFwurdeErzeugt(paintPNG As Boolean, pdfDatei As String, dauer As Integer) As Boolean
'    '    Dim pdfWurdeErzeugt As Boolean

'    '    'If iminternet Then
'    '    pdfWurdeErzeugt = True
'    '    'Else
'    '    '    pdfWurdeErzeugt = pruefeExistenzderPdfDatei(paintPNG, dauer, pdfDatei)
'    '    'End If

'    '    Return pdfWurdeErzeugt
'    'End Function

'    Private Function extractPDFfileURL(paintPNG As Boolean, hochaufloesend As Boolean, ergebnis As String) As String
'        Dim pdfDatei As String
'        'If iminternet Then
'        pdfDatei = extractPDFfilenameInter(ergebnis, paintPNG, hochaufloesend)
'        'Else
'        '    pdfDatei = extractPDFfilenameIntra(ergebnis, paintPNG, hochaufloesend)
'        'End If
'        Return pdfDatei
'    End Function

'    Function getNameOfDownloadFile(pdfDatei As String) As String
'        Dim zieldatei As String

'        'If iminternet Then
'        zieldatei = pdfDatei.Replace(serverWeb & "/cache/", "")
'        'Else
'        '    zieldatei = pdfDatei.Replace("/", "\")
'        '    zieldatei = zieldatei.Replace("\\gis\gdvell\cache\", "")
'        '    zieldatei = zieldatei.Replace(serverUNC, "")
'        '    zieldatei = zieldatei.Replace(serverUNC & "cache\", "")
'        '    zieldatei = zieldatei.Replace("cache\", "")
'        '    zieldatei = zieldatei.Replace("/", "\")
'        'End If

'        Return zieldatei
'    End Function

'    Private Function extractPDFfilenameInter(ergebnis As String, pngStattPdf As Boolean, hochaufloesend As Boolean) As String
'        Dim pdfdatei, rest, endung As String
'        Dim a, b As Integer
'        l("ergebnis " & ergebnis)
'        l("pngStattPdf " & pngStattPdf)
'        l("hochaufloesend " & hochaufloesend)
'        Try
'            If pngStattPdf Or hochaufloesend Then
'                endung = ".png"
'            Else
'                endung = ".pdf"
'            End If
'            l("endung " & endung)
'            a = ergebnis.IndexOf("/cache/")
'            If a < 1 Then
'                l("fehler in extractPDFfilename a" & ergebnis)
'                Return ""
'            End If
'            b = ergebnis.IndexOf(endung)
'            rest = ergebnis.Substring(a, (b - a))
'            l("rest " & rest)
'            pdfdatei = serverWeb & rest & endung
'            l("pdfdatei " & pdfdatei)
'            pdfdatei = pdfdatei.Replace("\", "/")
'            l("pdfdatei " & pdfdatei)
'            Return pdfdatei
'        Catch ex As Exception
'            l("fehler in extractPDFfilename b: " & ergebnis, ex)
'            Return ""
'        End Try
'    End Function

'    Private Function calcPDFOUTfilename(pdfdatei As String) As String
'        Dim outpdf As String
'        Dim fi As New IO.FileInfo(pdfdatei)
'        outpdf = PicturesPath & "\" & fi.Name
'        outpdf = outpdf.Replace(".png", ".pdf").Replace(".PNG", ".pdf")
'        fi = Nothing
'        Return outpdf
'    End Function

'    Private Function selberPDFerzeugen(pngdatei As String, w As Integer, h As Integer, outpdf As String, isa4checked As Boolean) As Boolean
'        If wrapItextSharp.createImagePdf(pngdatei, outpdf, w, h, isa4checked) Then
'            Return True
'        Else
'            Return False
'        End If
'    End Function

'    Private Function pruefeExistenzderPdfDatei(paintPNG As Boolean, dauer As Integer, pdfdatei As String) As Boolean
'        Dim fi As New IO.FileInfo(pdfdatei)
'        If fi.Exists Then
'            Return True
'        Else
'            Threading.Thread.Sleep(dauer * 4)
'            Return True
'        End If
'        Return False
'    End Function

'    Private Sub calcWartezeiten(schmittner As Boolean, ByRef meinHttpTimeout As Integer, ByRef dauer As Integer)
'        If schmittner Then
'            dauer = 9000
'            meinHttpTimeout = 50000
'        Else
'            dauer = 1000
'            meinHttpTimeout = 25000
'        End If
'    End Sub

'    Private Sub scale4Paint()
'        _hoehe = CStr(CDbl(_hoehe) * PaintPngScale)
'        _breite = CStr(CDbl(_breite) * PaintPngScale)
'    End Sub

'    Private Sub calcWeightHeight(ausrichtung As String, hochaufloesend As Boolean, isA4Formatchecked As Boolean)
'        If isA4Formatchecked Then
'            If ausrichtung = "quer" Then
'                _breite = CType(dina4InPixel.w, String)
'                _hoehe = CType(dina4InPixel.h, String)
'                If hochaufloesend Then
'                    _breite = CType(1658, String)
'                    _hoehe = CType(1097, String)
'                    '_breite = CType(CInt(_breite) * 1.23522316, String)
'                    '_hoehe = CType(CInt(_hoehe) * 1.23522316, String)
'                End If
'            Else
'                _hoehe = CType(dina4InPixel.w, String)
'                _breite = CType(dina4InPixel.h, String)
'                If hochaufloesend Then
'                    _breite = CType(1097, String)
'                    _hoehe = CType(1658, String)
'                    '_breite = CType(CInt(_breite) * 1.23522316, String)
'                    '_hoehe = CType(CInt(_hoehe) * 1.23522316, String)
'                End If
'            End If
'        Else
'            'A3
'            If ausrichtung = "quer" Then
'                _breite = CType(dina3InPixel.w, String)
'                _hoehe = CType(dina3InPixel.h, String)
'                'If hochaufloesend Then
'                '    _breite = CType(1658 * 1.96912114, String)
'                '    _hoehe = CType(1097 * 1.843697479, String)
'                '    '_breite = ctype(cint(_breite) * 1.23522316, string)
'                '    '_hoehe = ctype(cint(_hoehe) * 1.23522316, string)
'                'End If
'            Else
'                _hoehe = CType(dina3InPixel.w, String)
'                _breite = CType(dina3InPixel.h, String)
'                'If hochaufloesend Then
'                '    _breite = CType(1097 * 1.843697479, String)
'                '    _hoehe = CType(1658 * 1.96912114, String)
'                '    '_breite = CType(CInt(_breite) * 1.23522316, String)
'                '    '_hoehe = CType(CInt(_hoehe) * 1.23522316, String)
'                'End If
'            End If
'        End If
'        '_breite = CType(1200, String)
'        '_hoehe = CType(848.4848485, String)
'    End Sub

'    'Private Function extractPDFfilenameIntra(ergebnis As String, pngStattPdf As Boolean, hochaufloesend As Boolean) As String
'    '    Dim pdfdatei, rest, endung As String
'    '    Dim a, b As Integer
'    '    Try
'    '        If pngStattPdf Or hochaufloesend Then
'    '            endung = ".png"
'    '        Else
'    '            endung = ".pdf"
'    '        End If
'    '        a = ergebnis.IndexOf("/cache/")
'    '        If a < 1 Then
'    '            Return ""
'    '        End If
'    '        b = ergebnis.IndexOf(endung)
'    '        rest = ergebnis.Substring(a, (b - a))
'    '        pdfdatei = serverUNC & rest & endung
'    '        pdfdatei = pdfdatei.Replace("\/", "\")
'    '        pdfdatei = pdfdatei.Replace("/", "\")
'    '        Return pdfdatei
'    '    Catch ex As Exception
'    '        l("fehler in extractPDFfilename ", ex)
'    '        Return ""
'    '    End Try
'    'End Function

'    Private Function bildeaufrufMapserverPDF(_breite As String, _hoehe As String,
'                                    temprange As clsRange, druckmasstab As Double,
'                                    pdf_bemerkung As String, pdf_ort As String,
'                                    tempMapfile As String, isa4Formatchecked As Boolean) As String
'        'https://buergergis.kreis-offenbach.de/cgi-bin/mapserv70/mapserv.cgi?
'        '           mapsize=842+595&mapext=484926+5541956+485580+5542394&map=C:/ptest/mgis/cache/mapfiles/feinen_j_20190111071517384.map
'        '           &ortsangabe=UTM32: 485253, 5542175&bemerkung=feinen_j&druckmasstab=&datum=11.01.2019 
'        'mapsize=842+595'; // Auflösung für MapServer: 72 dpi
'        Dim massstabzeile As String
'        If druckmasstab < 1 Then
'            massstabzeile = "&druckmasstab="
'        Else
'            massstabzeile = "&druckmasstab=1:" & (CInt(druckmasstab).ToString)
'        End If
'        aufruf = "/cgi-bin/" & strGlobals.mapserverExeString & "?mapsize=" & _breite & "+" & _hoehe &
'                "&mapext=" &
'                CInt(temprange.xl) & "+" &
'                CInt(temprange.yl) & "+" &
'                CInt(temprange.xh) & "+" &
'                CInt(temprange.yh) & "" &
'                "&map=" & tempMapfile & "" &
'                "&ortsangabe=" & pdf_ort &
'                "&hoehe=" & _hoehe &
'                "&breite=" & _breite &
'                "&isa4Formatchecked=" & isa4Formatchecked &
'                "&bemerkung=" & pdf_bemerkung &
'                massstabzeile &
'                "&datum=" & Format(Now, "dd.MM.yyyy")
'        aufruf = serverWeb & aufruf
'        aufruf = aufruf.Replace("\", "/")
'        aufruf = aufruf.Replace("//gis/gdvell", "d:")
'        l("bildeaufrufMapserverPDF: " & aufruf)
'        Return aufruf
'    End Function

'    Private Function bildePDFRange(modus As String, PDF_PrintRange As clsRange) As clsRange
'        Dim temprange As New clsRange
'        If modus = "mitmasstab" Then
'            temprange.xl = PDF_PrintRange.xl
'            temprange.xh = PDF_PrintRange.xh
'            temprange.yl = PDF_PrintRange.yl
'            temprange.yh = PDF_PrintRange.yh
'        Else
'            temprange.xl = kartengen.aktMap.aktrange.xl
'            temprange.xh = kartengen.aktMap.aktrange.xh
'            temprange.yl = kartengen.aktMap.aktrange.yl
'            temprange.yh = kartengen.aktMap.aktrange.yh
'        End If
'        Return temprange
'    End Function


'    '<Obsolete>
'    'Friend Function OLDcalcPDFrahmenPositionInPixel(canvaswidth As Double, canvasheight As Double,
'    '                                 framewidth As Double, frameheight As Double) As myPoint
'    '    Dim temp As New myPoint
'    '    Try
'    '        temp.X = CInt((canvaswidth / 2) - (framewidth / 2))
'    '        temp.Y = CInt((canvasheight / 2) - (frameheight / 2))
'    '        Return temp
'    '    Catch ex As Exception
'    '        l("fehler in calcPDFrahmenPositionInPixel: ", ex)
'    '        Return Nothing
'    '    End Try
'    'End Function

'    Friend Function calcPDFrahmenPositionInPixel(myPDFRect As Rectangle, mittelPunkt As myPoint) As myPoint
'        Dim temp As New myPoint
'        Try
'            l("calcPDFrahmenPositionInPixel---------------------- anfang")
'            temp.X = mittelPunkt.X - myPDFRect.Width / 2
'            temp.Y = mittelPunkt.Y - myPDFRect.Height / 2
'            Return temp
'            l("calcPDFrahmenPositionInPixel---------------------- ende")
'        Catch ex As Exception
'            l("Fehler in calcPDFrahmenPositionInPixel: " & ex.ToString())
'            Return Nothing
'        End Try
'    End Function
'    Private Function genaufrufBuildMapfile(paintPNG As Boolean, hochaufloesend As Boolean,
'                                           mitsuchobjekt As Boolean, mitFSTName As Boolean, breite As Integer, hoehe As Integer, isa4 As Boolean,
'                                           PDF_PrintRange As clsRange,
'                                           pdf_bemerkung As String, pdf_ort As String,
'                                           druckmasstab As Double,
'                                           FSTname As String, FSTabstract As String) As String
'        Dim aufruf As String = ""
'        Try
'            l(" MOD genaufrufBuildMapfile anfang")
'            If paintPNG Then
'                aufruf = aufruf & "&typ=PNG"
'            Else
'                aufruf = aufruf & "&typ=PDF"
'            End If
'            If hochaufloesend Then
'                aufruf = aufruf & "&hires=1"
'            Else
'                aufruf = aufruf & "&hires=0"
'            End If
'            If mitsuchobjekt Then
'                aufruf = aufruf & "&mitsuchobjekt=1"
'            Else
'                aufruf = aufruf & "&mitsuchobjekt=0"
'            End If
'            If mitFSTName Then
'                aufruf = aufruf & "&mitFSTName=1"
'            Else
'                aufruf = aufruf & "&mitFSTName=0"
'            End If
'            aufruf = aufruf & "&hoehe=" & CInt(hoehe)
'            aufruf = aufruf & "&breite=" & CInt(breite)
'            If isa4 Then
'                aufruf = aufruf & "&isa4Formatchecked=true"
'            Else
'                aufruf = aufruf & "&isa4Formatchecked=false"
'            End If
'            aufruf = aufruf & "&mapext=" & CInt(PDF_PrintRange.xl) & "+" & CInt(PDF_PrintRange.yl) & "+" & CInt(PDF_PrintRange.xh) & "+" & CInt(PDF_PrintRange.yh)
'            aufruf = aufruf & "&bemerkung=" & (pdf_bemerkung)
'            aufruf = aufruf & "&ort=" & (pdf_ort)
'            aufruf = aufruf & "&druckmasstab=" & CInt(druckmasstab)

'            Dim emplayers As New List(Of clsLayerPres)
'            emplayers = layersselected_copieren(layersSelected)
'            If layerHgrund.aid > 0 Then
'                layerHgrund.mithaken = True
'                emplayers.Add(layerHgrund)
'            End If
'            If mitsuchobjekt Then
'                If Not aktFST.name.IsNothingOrEmpty Then
'                    aufruf = aufruf & "&fstname=" & (FSTname.Trim)
'                    aufruf = aufruf & "&fstabstract=" & (FSTabstract.Trim)
'                End If
'            End If


'            Dim layerstring As New Text.StringBuilder
'            emplayers = layersSelectedNachRangOrdnen(emplayers)
'            For Each nlayer As clsLayerPres In emplayers
'                If nlayer.mithaken Then
'                    If hochaufloesend Then
'                        layerstring.Append(nlayer.aid & ",")
'                    Else
'                        layerstring.Append(nlayer.aid & ",")
'                    End If
'                End If
'            Next
'            aufruf = aufruf & "&layers=" & clsString.removeLastChar(layerstring.ToString)
'            l(" aufruf" & aufruf)
'            l(" MOD genaufrufBuildMapfile ende")
'            Return aufruf
'        Catch ex As Exception
'            l("Fehler in genaufrufBuildMapfile: " & ex.ToString())
'            Return ""
'        End Try
'    End Function




'End Module
