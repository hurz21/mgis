Imports mgis

Module createPDF
    Private PaintPngScale As Double = 2.0
    'serverweb & "/cgi-bin/mapserv70/mapserv.exe?mapsize=1660+1050&mapext=481568+5538735+485958+5541512&map=/inetpub/wwwroot/buergergis/mapfile/drucken_quer_schriftfeld.map&ortsangabe=&bemerkung=&druckmasstab=1:12313&datum=732
    '   Dim aufruf = serverweb & "/cgi-bin/mapserv70/mapserv.exe?mapsize=1660+1050&mapext=481568+5538735+485958+5541512&map=/inetpub/wwwroot/buergergis/mapfile/drucken_quer_schriftfeld.map&ortsangabe=&bemerkung=&druckmasstab=1:12313&datum=732"

    '_breite = CType(842, String)
    '_hoehe = CType(595, String)
    'Dim npdf As New winPDF("842", "595")
    'npdf.Show()
    Public Property _breite As String
    Public Property _hoehe As String
    Public Property aufruf As String
    Public Property PicturesPath As String

    Function dateiNachMeineBilderKopieren(pdfdatei As String) As String
        Dim neuername As String
        Try
            l("dateiNachMeineBilderKopieren---------------------- anfang")

            Dim fi As New IO.FileInfo(pdfdatei)
            neuername = PicturesPath & "\" & fi.Name
            fi.CopyTo(neuername, True)
            l("dateiNachMeineBilderKopieren---------------------- ende")
            Return neuername

        Catch ex As Exception
            l("Fehler in dateiNachMeineBilderKopieren: " & ex.ToString())
            Return pdfdatei
        End Try
    End Function

    'Public Function makeandloadPDF(masstabsModus As String, PDF_PrintRange As clsRange,
    '                          druckmasstab As Double, ausrichtung As String, pdf_bemerkung As String, pdf_ort As String,
    '                          paintPNG As Boolean, hochaufloesend As Boolean, isa4Formatchecked As Boolean,
    '                               schnelldruck As Boolean, ByRef pdfDatei As String) As Boolean
    '    l("in makeandloadPDF--------------------------")
    '    PicturesPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
    '    Dim mapfileStringtext As String
    '    mapfileStringtext = createMapfilePDF(paintPNG, hochaufloesend)
    '    StringbuilderAussschreiben(mapfileStringtext, mapfileBILD)
    '    calcWeightHeight(ausrichtung, hochaufloesend, isa4Formatchecked)

    '    If paintPNG Then scale4Paint()
    '    PDF_PrintRange = bildePDFRange(masstabsModus, PDF_PrintRange)

    '    aufruf = bildeaufrufPDF(_breite, _hoehe, PDF_PrintRange, druckmasstab, pdf_bemerkung, pdf_ort)
    '    l("aufruf: " & aufruf)

    '    Dim hinweis, ergebnis As String, meinHttpTimeout, dauer As Integer
    '    calcWartezeiten(hochaufloesend, meinHttpTimeout, dauer)

    '    ergebnis = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, meinHttpTimeout)
    '    l("hinweis " & hinweis) : l("------------------------aufruf ergebnis'")
    '    If ergebnis.IsNothingOrEmpty Then
    '        MessageBox.Show("FEhler bei erzeugung der PDF-Datei, timeout: " & meinHttpTimeout & hinweis, "Fehler")
    '        Return False
    '    End If
    '    l(ergebnis)
    '    Dim kopie As String
    '    pdfDatei = extractPDFfilename(ergebnis, paintPNG, hochaufloesend)

    '    If pdfdatei.Length < 3 Then
    '        MessageBox.Show("Fehler bei der Erzeugung der PDF-Datei", "Fehler")
    '        Return False
    '    End If
    '    Threading.Thread.Sleep(dauer)
    '    Dim pdfWurdeErzeugt As Boolean
    '    pdfWurdeErzeugt = pruefeExistenzderPdfDatei(paintPNG, dauer, pdfdatei)
    '    If pdfWurdeErzeugt Then
    '        If paintPNG Then
    '            kopie = dateiNachMeineBilderKopieren(pdfdatei)
    '            OpenDokumentWith("mspaint.exe", kopie)
    '            pdfDatei = kopie
    '            Return True
    '        End If
    '        If hochaufloesend Then
    '            Dim outpdf As String
    '            outpdf = calcPDFOUTfilename(pdfdatei)
    '            Threading.Thread.Sleep(1000)
    '            selberPDFerzeugen(pdfdatei, CInt(_breite), CInt(_hoehe), outpdf, isa4Formatchecked)
    '            OpenDokument(outpdf)
    '        Else

    '            If schnelldruck Then
    '                Dim param As String
    '                param = "start /B ""Drucken"" ""C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe"" /t """ & pdfdatei & """"
    '                Dim datei As String
    '                datei = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) & "\print.bat"
    '                My.Computer.FileSystem.WriteAllText(datei, param, False, enc)
    '                Microsoft.VisualBasic.Shell(datei)
    '            Else
    '                OpenDokument(pdfdatei)
    '            End If
    '        End If
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function

    Private Function calcPDFOUTfilename(pdfdatei As String) As String
        Dim outpdf As String
        Dim fi As New IO.FileInfo(pdfdatei)
        outpdf = PicturesPath & "\" & fi.Name
        outpdf = outpdf.Replace(".png", ".pdf").Replace(".PNG", ".pdf")
        fi = Nothing
        Return outpdf
    End Function

    Private Function selberPDFerzeugen(pngdatei As String, w As Integer, h As Integer, outpdf As String, isa4checked As Boolean) As Boolean
        If wrapItextSharp.createImagePdf(pngdatei, outpdf, w, h, isa4checked) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function pruefeExistenzderPdfDatei(paintPNG As Boolean, dauer As Integer, pdfdatei As String) As Boolean
        Dim fi As New IO.FileInfo(pdfdatei)
        If fi.Exists Then
            Return True
        Else
            Threading.Thread.Sleep(dauer * 4)
            Return True
        End If
        Return False
    End Function

    Private Sub calcWartezeiten(schmittner As Boolean, ByRef meinHttpTimeout As Integer, ByRef dauer As Integer)
        If schmittner Then
            dauer = 9000
            meinHttpTimeout = 50000
        Else
            dauer = 1000
            meinHttpTimeout = 25000
        End If
    End Sub

    Private Sub scale4Paint()
        _hoehe = CStr(CDbl(_hoehe) * PaintPngScale)
        _breite = CStr(CDbl(_breite) * PaintPngScale)
    End Sub

    'Private Sub calcWeightHeight(ausrichtung As String, hochaufloesend As Boolean, isA4Formatchecked As Boolean)
    '    If isA4Formatchecked Then
    '        If ausrichtung = "quer" Then
    '            _breite = CType(dina4InPixel.w, String)
    '            _hoehe = CType(dina4InPixel.h, String)
    '            If hochaufloesend Then
    '                _breite = CType(1658, String)
    '                _hoehe = CType(1097, String)
    '                '_breite = CType(CInt(_breite) * 1.23522316, String)
    '                '_hoehe = CType(CInt(_hoehe) * 1.23522316, String)
    '            End If
    '        Else
    '            _hoehe = CType(dina4InPixel.w, String)
    '            _breite = CType(dina4InPixel.h, String)
    '            If hochaufloesend Then
    '                _breite = CType(1097, String)
    '                _hoehe = CType(1658, String)
    '                '_breite = CType(CInt(_breite) * 1.23522316, String)
    '                '_hoehe = CType(CInt(_hoehe) * 1.23522316, String)
    '            End If
    '        End If
    '    Else
    '        'A3
    '        If ausrichtung = "quer" Then
    '            _breite = CType(dina3InPixel.w, String)
    '            _hoehe = CType(dina3InPixel.h, String)
    '            'If hochaufloesend Then
    '            '    _breite = CType(1658 * 1.96912114, String)
    '            '    _hoehe = CType(1097 * 1.843697479, String)
    '            '    '_breite = ctype(cint(_breite) * 1.23522316, string)
    '            '    '_hoehe = ctype(cint(_hoehe) * 1.23522316, string)
    '            'End If
    '        Else
    '            _hoehe = CType(dina3InPixel.w, String)
    '            _breite = CType(dina3InPixel.h, String)
    '            'If hochaufloesend Then
    '            '    _breite = CType(1097 * 1.843697479, String)
    '            '    _hoehe = CType(1658 * 1.96912114, String)
    '            '    '_breite = CType(CInt(_breite) * 1.23522316, String)
    '            '    '_hoehe = CType(CInt(_hoehe) * 1.23522316, String)
    '            'End If
    '        End If
    '    End If
    '    '_breite = CType(1200, String)
    '    '_hoehe = CType(848.4848485, String)
    'End Sub

    'Private Function extractPDFfilename(ergebnis As String, pngStattPdf As Boolean, schmittner As Boolean) As String
    '    Dim pdfdatei, rest, endung As String
    '    Dim a, b As Integer
    '    Try
    '        If pngStattPdf Or schmittner Then
    '            endung = ".png"
    '        Else
    '            endung = ".pdf"
    '        End If
    '        a = ergebnis.IndexOf("/cache/")
    '        If a < 1 Then
    '            Return ""
    '        End If
    '        b = ergebnis.IndexOf(endung)
    '        rest = ergebnis.Substring(a, (b - a))
    '        pdfdatei = serverUNC & rest & endung
    '        pdfdatei = pdfdatei.Replace("\/", "\")
    '        pdfdatei = pdfdatei.Replace("/", "\")
    '        Return pdfdatei
    '    Catch ex As Exception
    '        l("fehler in extractPDFfilename ", ex)
    '        Return ""
    '    End Try
    'End Function

    'Private Function bildeaufrufPDF(_breite As String, _hoehe As String,
    '                                temprange As clsRange, druckmasstab As Double,
    '                                pdf_bemerkung As String, pdf_ort As String) As String
    '    'mapsize=842+595'; // Auflösung für MapServer: 72 dpi
    '    Dim massstabzeile As String
    '    If druckmasstab < 1 Then
    '        massstabzeile = "&druckmasstab="
    '    Else
    '        massstabzeile = "&druckmasstab=1:" & (CInt(druckmasstab).ToString)
    '    End If
    '    aufruf = "/cgi-bin/mapserv70/mapserv.exe?mapsize=" & _breite & "+" & _hoehe &
    '            "&mapext=" &
    '            CInt(temprange.xl) & "+" &
    '            CInt(temprange.yl) & "+" &
    '            CInt(temprange.xh) & "+" &
    '            CInt(temprange.yh) & "" &
    '            "&map=" & mapfileBILD & "" &
    '    "&ortsangabe=" & pdf_ort &
    '    "&bemerkung=" & pdf_bemerkung &
    '    massstabzeile &
    '    "&datum=" & Format(Now, "dd.MM.yyyy")
    '    aufruf = serverWeb & aufruf
    '    aufruf = aufruf.Replace("\", "/")
    '    Return aufruf
    'End Function

    'Private Function bildePDFRange(modus As String, PDF_PrintRange As clsRange) As clsRange
    '    Dim temprange As New clsRange
    '    If modus = "mitmasstab" Then
    '        temprange.xl = PDF_PrintRange.xl
    '        temprange.xh = PDF_PrintRange.xh
    '        temprange.yl = PDF_PrintRange.yl
    '        temprange.yh = PDF_PrintRange.yh
    '    Else
    '        temprange.xl = kartengen.aktMap.aktrange.xl
    '        temprange.xh = kartengen.aktMap.aktrange.xh
    '        temprange.yl = kartengen.aktMap.aktrange.yl
    '        temprange.yh = kartengen.aktMap.aktrange.yh
    '    End If
    '    Return temprange
    'End Function


    '<Obsolete>
    'Friend Function OLDcalcPDFrahmenPositionInPixel(canvaswidth As Double, canvasheight As Double,
    '                                 framewidth As Double, frameheight As Double) As myPoint
    '    Dim temp As New myPoint
    '    Try
    '        temp.X = CInt((canvaswidth / 2) - (framewidth / 2))
    '        temp.Y = CInt((canvasheight / 2) - (frameheight / 2))
    '        Return temp
    '    Catch ex As Exception
    '        l("fehler in calcPDFrahmenPositionInPixel: ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    Friend Function calcPDFrahmenPositionInPixel(myPDFRect As Rectangle, mittelPunkt As myPoint) As myPoint
        Dim temp As New myPoint
        Try
            l("calcPDFrahmenPositionInPixel---------------------- anfang")
            temp.X = mittelPunkt.X - myPDFRect.Width / 2
            temp.Y = mittelPunkt.Y - myPDFRect.Height / 2
            Return temp
            l("calcPDFrahmenPositionInPixel---------------------- ende")
        Catch ex As Exception
            l("Fehler in calcPDFrahmenPositionInPixel: " & ex.ToString())
            Return Nothing
        End Try
    End Function
End Module
