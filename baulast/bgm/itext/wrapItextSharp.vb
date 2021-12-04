Imports System.Text.RegularExpressions
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Module wrapItextSharp
    Private PDFoffsetleft As Integer = 0
    Private PDFoffset As Integer = 0

    Private post_PDF_SCALE As String = ""
    Public post_PDF_ARROW As Boolean = True
    Public post_PDF_LEGEND As Boolean
    Public post_PDF_DOKU As Boolean
    Public post_PDF_QUELLENNACHWEIS As Boolean
    Public post_PDF_ICONLAYER As String = ""
    Public post_PDF_Bearbeiter As String = ""
    Private post_PDF_Ortsteil As String = ""
    Private post_PDF_Bemerkung As String = ""
    Private post_PDF_FORMAT As String = "A4"
    Private Writer As PdfWriter = Nothing
    Private oDoc As iTextSharp.text.Document
    Private portrait As Boolean = True

    Private cb As PdfContentByte
    Private bf As BaseFont '.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
    Private LayerPIC As iTextSharp.text.Image
    Private _aFormat As New clsCanvas
    Public dina4InMM, dina3InMM, dina4InPixel, dina3InPixel As New clsCanvas
    Sub defineDinA4Dina3Formate()
        dina4InMM.w = 297 : dina4InMM.h = 210
        dina3InMM.w = 420 : dina3InMM.h = 297

        dina4InPixel.w = 842 : dina4InPixel.h = 595
        dina3InPixel.w = 1191 : dina3InPixel.h = 842
    End Sub
    Friend Function createImagePdf(pngdatei As String(), outPDF As String, w As Integer, h As Integer, isa4checked As Boolean, outdir As String) As Boolean
        Try
            l("createPdf---------------------- anfang")
            If isa4checked Then
                _aFormat = dina4InMM
                post_PDF_FORMAT = "A4"
            Else
                _aFormat = dina3InMM
                post_PDF_FORMAT = "A3"
            End If
            PDF_format_festlegen(_aFormat, post_PDF_FORMAT)

            reduceOffsets()
            portrait = get_DOC_and_Orientation(w, h)

            'C:\Users\Public\Documents\Paradigma\cache\pdf\MS15494608499188.pdf
            Dim fi As New IO.FileInfo(outPDF)
            IO.Directory.CreateDirectory(fi.Directory.FullName)
            getWriterOpenAddMeta(outPDF)
            cb = Writer.DirectContent
            For i = 0 To pngdatei.Count - 1
                cb.AddImage(defineMergedPic(portrait, pngdatei(i)))
                addChunk2Dok(oDoc, " " & " ", bf, 14, 12)





                oDoc.NewPage()
            Next
            oDoc.Close()
            l("create createPdf Ende")
            Return True
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in createPdf: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Sub reduceOffsets()
        _aFormat.w = _aFormat.w - PDFoffsetleft - PDFoffset
        _aFormat.h = _aFormat.h - PDFoffset - PDFoffset
    End Sub

    Private Sub PDF_format_festlegen(ByRef A4dim As clsCanvas, kuerzel As String)
        l(" MOD PDF_format_festlegen anfang")
        Try
            l("kuerzel " & kuerzel)
            Select Case kuerzel
                Case "A3"
                    A4dim.w = dina3InPixel.h
                    A4dim.h = dina3InPixel.w
                Case "A4"
                    A4dim.w = dina4InPixel.h
                    A4dim.h = dina4InPixel.w
                Case Else
                    A4dim.w = dina4InPixel.h
                    A4dim.h = dina4InPixel.w
            End Select
            'A4 Breite =595 points
            'A4 Höhe= 842 points
            '  a4    8.2677 * 72 = 595 points
            '29.7 cm / 2.54 = 11.6929 inch
            '11.6929 * 72 = 842 points 
            l(" MOD PDF_format_festlegen ende")
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
        End Try
    End Sub

    Private Function defineMergedPic(ByVal portrait As Boolean,
                                      ByVal Mergefile As String) As iTextSharp.text.Image
        LayerPIC = iTextSharp.text.Image.GetInstance(Mergefile)
        If portrait Then
            LayerPIC.ScaleToFit(_aFormat.w, _aFormat.h)
            l("SCALE:" & _aFormat.h & " " & _aFormat.w & "" & Mergefile)
            LayerPIC.SetAbsolutePosition(PDFoffsetleft, PDFoffset)
        Else
            LayerPIC.ScaleToFit(_aFormat.h, _aFormat.w)
            l("SCALE:" & _aFormat.h & " " & _aFormat.w & " " & Mergefile)
            LayerPIC.SetAbsolutePosition(PDFoffset, PDFoffset)
        End If
        Return LayerPIC
    End Function
    Function get_DOC_and_Orientation(w As Integer, h As Integer) As Boolean
        If h < w Then
            If post_PDF_FORMAT = "A3" Then
                oDoc = New Document(PageSize.A3.Rotate, 0, 0, 0, 0)
            Else
                oDoc = New Document(PageSize.A4.Rotate, 0, 0, 0, 0)
            End If
            Return False
        Else
            If post_PDF_FORMAT = "A3" Then
                oDoc = New Document(PageSize.A3, 0, 0, 0, 0)
            Else
                oDoc = New Document(PageSize.A4, 0, 0, 0, 0)
            End If
            Return True
        End If
    End Function
    Private Sub getWriterOpenAddMeta(outPDF As String)
        Try
            Writer = PdfWriter.GetInstance(oDoc, New System.IO.FileStream(outPDF, System.IO.FileMode.Create))
            Writer.ViewerPreferences = PdfWriter.PageModeUseOC
            Writer.Open()
            oDoc.Open() ' nach oben verschoben
            oDoc.AddTitle("Kreis Offenbach - BürgerGIS")
            oDoc.AddSubject("Jobnummer: ")
            oDoc.AddKeywords("BürgerGIS , Karten, Maps,Dreieich,Egelsbach,Dietzenbach,Rodgau,Mühlheim,Rödermark")
            oDoc.AddCreator("BürgerGIS")
            oDoc.AddAuthor("keinautor" & post_PDF_Bearbeiter)
            oDoc.AddHeader("Expires", "0")

        Catch ex As Exception
            Dim hinweis$ = "FEHLER in getWriter :" & ex.Message
            l(hinweis$)
        End Try
    End Sub

    Friend Sub createSchnellEigentuemer(text As String, ByRef ausgabedatei As String, verbotsString As String, lokalitaet As String, lage As String, gisuser_nick As String)
        l("createSchnellEigentuemer---------------------- anfang")
        Dim records() As String
        Dim bf As BaseFont
        Dim textfont As BaseFont
        Try
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            textfont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            records = zerlegeEigentuemerText(text, "BSt.Nr.:")
            PDF_format_festlegen(_aFormat, "A4")
            'reduceOffsets()
            portrait = True
            oDoc = New Document(PageSize.A4)
            oDoc.SetMargins(50, 50, 50, 50)
            getWriterOpenAddMeta(ausgabedatei)
            'cb = Writer.DirectContent
            'Dim kreislogoFont As iTextSharp.text.pdf.BaseFont 
            'cb.BeginText()
            'cb.SetFontAndSize(bf, 8)
            'cb.SetRGBColorFill(0, 0, 0)
            'Dim left_margin = 50
            'Dim top_margin = 50
            Dim zeilenabstand_gross As Integer = 14
            Dim zeilenabstand_klein As Integer = 10
            addChunk2Dok(oDoc, "Kreis Offenbach   Postfach 1265  63112 Dietzenbach", bf, 8, zeilenabstand_gross)
            addChunk2Dok(oDoc, "Datum: " & Format(Now, "dd.MM.yyyy") & ", " & gisuser_nick, bf, 8, zeilenabstand_gross + 4)
            addChunk2Dok(oDoc, "Auskunft aus dem Liegenschaftsbuch (ALKIS) " & " ", bf, 14, zeilenabstand_gross)
            addChunk2Dok(oDoc, " ---------- Schnellauskunft bis 4500 Zeichen ---------- " & " ", bf, 12, zeilenabstand_gross)
            addChunk2Dok(oDoc, " " & " ", bf, 14, zeilenabstand_gross)
            addChunk2Dok(oDoc, lokalitaet & " ", bf, 12, zeilenabstand_gross)
            addChunk2Dok(oDoc, "" & " ", bf, 12, zeilenabstand_gross)
            addChunk2Dok(oDoc, lage & " ", bf, 12, zeilenabstand_gross)
            addChunk2Dok(oDoc, " " & " ", bf, 14, zeilenabstand_gross)
            Dim temp As String = ""
            'die eigentuemer:
            For i = 0 To records.Count - 1
                temp = records(i).Replace(vbLf, "").Replace(vbTab, "").Replace(vbCrLf, "").Replace(vbCr, "").Trim
                addChunk2Dok(oDoc, temp & " ", bf, 8, zeilenabstand_klein)
            Next
            'addChunk2Dok(oDoc, text & " ", bf, 8, zeilenabstand_gross)


            addChunk2Dok(oDoc, " " & " ", bf, 14, zeilenabstand_klein)
            addChunk2Dok(oDoc, " " & " ", bf, 14, zeilenabstand_klein)
            addChunk2Dok(oDoc, "Wichtiger Hinweis:" & " ", bf, 10, zeilenabstand_klein)
            addChunk2Dok(oDoc, " " & " ", bf, 14, zeilenabstand_klein)
            addChunk2Dok(oDoc, verbotsString & " ", bf, 8, zeilenabstand_klein)
            'cb.EndText()
        Catch ex As Exception
            l("FEhler in createSchnellEigentuemer: " & ex.ToString)
        Finally
            oDoc.Close()
            Writer.Close()
            l("createSchnellEigentuemer Ende")
        End Try
    End Sub

    Private Function zerlegeEigentuemerText(text As String, nach As String) As String()
        Dim recs(10000) As String

        Dim chunk As String
        Dim s As String = text
        Dim i, iold, ianz, ilenge, teillenge As Integer
        Try
            l("zerlegeEigentuemerText---------------------- anfang")
            s = s.Replace(vbCrLf, "")
            ianz = 0
            i = s.IndexOf(nach)
            ilenge = text.Length
            iold = 0
            ' Loop over the found indexes.
            Do While (i <> -1)
                ' Write the substring.
                teillenge = (i) - iold
                chunk = s.Substring(iold, teillenge)
                'Console.WriteLine(chunk)
                iold = i
                recs(ianz) = chunk
                ianz += 1
#If DEBUG Then
                If ianz = 19 Then
                    Debug.Print("")
                End If
#End If
                ' Get next index.
                i = s.IndexOf(nach, i + 1)
            Loop
            teillenge = s.Length - iold
            chunk = s.Substring(iold, teillenge)
            recs(ianz) = chunk
            ReDim Preserve recs(ianz)
            Return recs
            l("zerlegeEigentuemerText---------------------- ende")
        Catch ex As Exception
            l("Fehler in zerlegeEigentuemerText: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    'Public Sub write(ByVal cb As PdfContentByte, ByVal Text As String, ByVal X As Integer, ByVal Y As Integer, ByVal font As BaseFont, ByVal Size As Integer)
    '    cb.SetFontAndSize(font, Size)
    '    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Text, X, Y, 0)
    'End Sub

    'New Paragraph("This is Paragraph 1"));
    '                doc.Add(New Paragraph(
    'Public Sub write(oDoc As iTextSharp.text.Document, ByVal cb As PdfContentByte, ByVal Text As String, ByVal font As BaseFont, ByVal Size As Integer)
    '    cb.SetFontAndSize(font, Size)
    '    'cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Text, 0, 0, 0)
    '    'cb.ShowText(Text)
    '    oDoc.Add(New Paragraph(Text))
    'End Sub

    Private Function addChunk2Dok(oDoc As iTextSharp.text.Document,
                                  ByVal titel As String, df As BaseFont, size As Integer, zeilenabstand As Integer) As Paragraph
        Dim chunk As New Chunk
        Dim myparaGraf As Paragraph
        Try
            l("addChunk2Dok---------------------- anfang")
            l(titel)
            If titel.IsNothingOrEmpty Then
                titel = " "
                l("addChunk2Dok titel auf blank gestellt")
            End If
            'Dim a = iTextSharp.text.Font.NORMAL
            myparaGraf = New Paragraph("")
            myparaGraf.Leading = zeilenabstand
            myparaGraf.Clear()

            chunk = New Chunk(titel, FontFactory.GetFont(FontFactory.COURIER, size, iTextSharp.text.Font.NORMAL, New iTextSharp.text.Color(0, 0, 0)))
            myparaGraf.Add(chunk)
            oDoc.Add(myparaGraf)
            Return myparaGraf
            l("addChunk2Dok---------------------- ende")
        Catch ex As Exception
            l("Fehler in addChunk2Dok: " & ex.ToString())
            Return Nothing
        End Try
    End Function
End Module
