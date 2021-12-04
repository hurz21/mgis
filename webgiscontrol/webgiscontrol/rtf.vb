Module rtf
    Public zielroot As String = tools.serverUNC & "\nkat\aid\"
    Sub makertfDoku(aktDoku As clsDoku, titel As String)
        Dim rtb2 As New RichTextBox
        Dim flw2 As New FlowDocument
        Dim paraHeader = New Paragraph()

        'Header			Aktennotiz
        paraHeader = New Paragraph()
        paraHeader.FontFamily = New FontFamily("Arial")
        paraHeader.FontSize = 24
        paraHeader.FontWeight = FontWeights.Bold
        paraHeader.Inlines.Add(New Run("Dokumentation zur GIS-Ebene: "))
        flw2.Blocks.Add(paraHeader)

        'Header			Aktennotiz
        paraHeader = New Paragraph()
        paraHeader.FontFamily = New FontFamily("Arial")
        paraHeader.FontSize = 24
        paraHeader.FontWeight = FontWeights.Bold
        paraHeader.Inlines.Add(New Run(titel$))
        flw2.Blocks.Add(paraHeader)

        'Allg.
        paraHeader = New Paragraph()
        paraHeader.FontFamily = New FontFamily("Arial")
        paraHeader.FontSize = 12
        paraHeader.FontWeight = FontWeights.Normal
        Dim headerDatum As Date = Now
        If headerDatum < CDate("1970-01-01") Then
            headerDatum = Now
        End If
        Dim nr As Run
        '  paraHeader.Inlines.Add(New Run("Datum: " & Format(headerDatum, "dd.MM.yyyy")))
        'paraHeader.Inlines.Add(New Run("Datum: " & Format(now, "dd.MM.yyyy"))) wg birgit klingler zurück zu ereignisdatum
        'Header			Aktennotiz
        'paraHeader = New Paragraph()
        'paraHeader.FontFamily = New FontFamily("Arial")
        'paraHeader.FontSize = 12
        'paraHeader.FontWeight = FontWeights.Bold


        'paraHeader.Inlines.Add(New LineBreak())
        'paraHeader.Inlines.Add(New Run("InterneNr: "))

        'spalte
        nr = New Run("InterneNr: ")
        nr.FontFamily = New FontFamily("Arial")
        nr.FontSize = 12
        nr.FontWeight = FontWeights.Bold
        paraHeader.Inlines.Add(nr)

        'wert
        nr = New Run(CType(aktDoku.aid, String))
        nr.FontFamily = New FontFamily("Arial")
        nr.FontSize = 12
        nr.FontWeight = FontWeights.Normal
        paraHeader.Inlines.Add(nr)
        paraHeader.Inlines.Add(New LineBreak())

        paraHeader.Inlines.Add(New LineBreak())
        paraHeader.Inlines.Add(New LineBreak())
        'spalte
        nr = New Run("Aktualität: ") : nr.FontWeight = FontWeights.Bold : paraHeader.Inlines.Add(nr)

        'wert
        nr = New Run(htm2cr(CType(aktDoku.aktualitaet, String))) : nr.FontWeight = FontWeights.Normal : paraHeader.Inlines.Add(nr)
        paraHeader.Inlines.Add(New LineBreak())
        paraHeader.Inlines.Add(New LineBreak())
        'spalte
        nr = New Run("Beschränkungen: ") : nr.FontWeight = FontWeights.Bold : paraHeader.Inlines.Add(nr)
        'wert
        nr = New Run(htm2cr(CType((aktDoku.beschraenkungen), String))) : nr.FontWeight = FontWeights.Normal : paraHeader.Inlines.Add(nr)
        paraHeader.Inlines.Add(New LineBreak())
        paraHeader.Inlines.Add(New LineBreak())
        'spalte
        nr = New Run("Datenabgabe: ") : nr.FontWeight = FontWeights.Bold : paraHeader.Inlines.Add(nr)
        'wert
        nr = New Run(htm2cr(CType(aktDoku.datenabgabe, String))) : nr.FontWeight = FontWeights.Normal : paraHeader.Inlines.Add(nr)
        paraHeader.Inlines.Add(New LineBreak())
        paraHeader.Inlines.Add(New LineBreak())
        'spalte
        nr = New Run("Entstehung: ") : nr.FontWeight = FontWeights.Bold : paraHeader.Inlines.Add(nr)
        'wert
        nr = New Run(htm2cr(CType(aktDoku.entstehung, String))) : nr.FontWeight = FontWeights.Normal : paraHeader.Inlines.Add(nr)
        paraHeader.Inlines.Add(New LineBreak())
        paraHeader.Inlines.Add(New LineBreak())
        'spalte
        nr = New Run("Inhalt: ") : nr.FontWeight = FontWeights.Bold : paraHeader.Inlines.Add(nr)
        'wert
        nr = New Run(htm2cr(CType(aktDoku.inhalt, String))) : nr.FontWeight = FontWeights.Normal : paraHeader.Inlines.Add(nr)
        paraHeader.Inlines.Add(New LineBreak())
        paraHeader.Inlines.Add(New LineBreak())
        'spalte
        nr = New Run("Maßstab: ") : nr.FontWeight = FontWeights.Bold : paraHeader.Inlines.Add(nr)
        'wert
        nr = New Run(htm2cr(CType(aktDoku.masstab, String))) : nr.FontWeight = FontWeights.Normal : paraHeader.Inlines.Add(nr)
        paraHeader.Inlines.Add(New LineBreak())
        paraHeader.Inlines.Add(New LineBreak())

        paraHeader.Inlines.Add(New LineBreak())
        paraHeader.Inlines.Add(New LineBreak())
        flw2.Blocks.Add(paraHeader)

        rtb2.Document = flw2

        Dim ausgabeDIR As String = zielroot & "\" & aktDoku.aid & "\rtfdoku"   '"L:\rtfs\d\" '& "" & aid
        IO.Directory.CreateDirectory(ausgabeDIR)
        Dim ausgabedatei As String = ausgabeDIR & "\" & aktDoku.aid & ".rtf"
        Dim filename As String = schreibeInRTFDatei(rtb2, ausgabedatei$)
        If Not filename.StartsWith("Fehler") Then
            '  Process.Start(filename)
            'End
        End If
    End Sub

    Function htm2cr(text As String) As String
        Dim neu As String
        Try
            neu = text.Replace("<a target=_blank ", " ")
            neu = neu.Replace("&ndash;", " ")
            neu = neu.Replace("&nbsp;", " ")
            neu = neu.Replace("href=", " ")

            neu = neu.Replace("</a>", " ")
            neu = neu.Replace("<br>", Environment.NewLine)
            neu = neu.Replace("<ul>", Environment.NewLine)
            neu = neu.Replace("</ul>", Environment.NewLine)
            neu = neu.Replace("<li>", Environment.NewLine)
            neu = neu.Replace("</li>", Environment.NewLine)
            neu = neu.Replace("<b>", Environment.NewLine)
            neu = neu.Replace("</b>", Environment.NewLine)
            neu = neu.Replace(">", " ")
            Return neu
        Catch ex As Exception
            l("fehler in htm2cr " & ex.ToString)
        End Try
    End Function
    Public Function schreibeInRTFDatei(ByVal rtb2 As RichTextBox, ByVal dateiname As String) As String
        Try
            Dim fs As IO.FileStream
            fs = New IO.FileStream(dateiname, IO.FileMode.Create)
            Dim tr As New TextRange(rtb2.Document.ContentStart, rtb2.Document.ContentEnd)
            tr.Save(fs, DataFormats.Rtf)
            rtb2.Selection.Save(fs, DataFormats.Rtf)
            fs.Close()
            Return dateiname
        Catch ex As Exception
            Return "Fehler: " & ex.ToString
        End Try
    End Function

    Sub makertflegende(aktleg As List(Of clsLegendenItem), titel As String, sachgebiet As String, ebenenprosa As String, aid As Integer)
        Dim rtb2 As New RichTextBox
        Dim flw2 As New FlowDocument
        Dim paraHeader = New Paragraph()
        Dim datei, root As String

        root = tools.serverUNC & "\nkat\aid\" & aid
        datei = root & "\legende\"

        'Header			Aktennotiz
        'paraHeader = New Paragraph()
        'paraHeader.FontFamily = New FontFamily("Arial")
        'paraHeader.FontSize = 20
        'paraHeader.FontWeight = FontWeights.Bold
        'paraHeader.Inlines.Add(New Run("Legende zu"))


        paraHeader = New Paragraph()
        paraHeader.FontFamily = New FontFamily("Arial")
        paraHeader.FontSize = 22
        paraHeader.FontWeight = FontWeights.Bold
        paraHeader.Inlines.Add(New Run(("Legende zu " & titel$)))
        flw2.Blocks.Add(paraHeader)

        paraHeader.Inlines.Add(New LineBreak())

        ' flw2.Blocks.Add(paraHeader)

        'paraHeader = New Paragraph()

        For Each nleg As clsLegendenItem In aktleg
            Dim ziel = datei & nleg.nr & ".gif"
            diaanlegen(ziel, flw2, paraHeader, nleg.titel)
            aid = nleg.aid
        Next
        flw2.Blocks.Add(paraHeader)

        rtb2.Document = flw2
        Dim ausgabeDIR As String = tools.serverUNC & "\nkat\aid\" & aid & "\rtflegend\" '& "" & aid
        IO.Directory.CreateDirectory(ausgabeDIR)
        Dim ausgabedatei As String = ausgabeDIR & "" & aid & ".rtf"
        Dim filename As String = schreibeInRTFDatei(rtb2, ausgabedatei$)
        If Not filename.StartsWith("Fehler") Then
            '  Process.Start(filename)
            'End
        End If
    End Sub
    Sub diaanlegen(ByVal fotodatei As String,
                 ByVal flw2 As FlowDocument,
                 ByVal paraHeader As System.Windows.Documents.Paragraph,
                 ByVal Titel As String)
        Dim image As New Image
        Dim bimg As BitmapImage = New BitmapImage()
        Dim nr As Run
        Try
            image.Width = 30
            image.Height = 25
            image.Stretch = Stretch.UniformToFill


            bimg.BeginInit()
            bimg.UriSource = New Uri(fotodatei, UriKind.Absolute)
            bimg.DecodePixelWidth = 600

            bimg.EndInit()
            image.Source = bimg
            paraHeader.FontSize = 15

            'spalte
            nr = New Run("  ") : nr.FontWeight = FontWeights.Bold : paraHeader.Inlines.Add((image))

            'wert
            nr = New Run(htm2cr(CType("    " & Titel, String))) : nr.FontWeight = FontWeights.Normal : paraHeader.Inlines.Add(nr)
            paraHeader.Inlines.Add(New LineBreak())





            'flw2.Blocks.Add(New BlockUIContainer(image))

            ' paraHeader = New Paragraph()
            'paraHeader.FontSize = 12
            'paraHeader.FontWeight = FontWeights.Normal
            'paraHeader.Inlines.Add(New Run((Titel)))
            flw2.Blocks.Add(paraHeader)
            image = Nothing
            bimg = Nothing

        Catch ex As Exception
            l("Fehler ind " & ex.ToString)
        End Try
    End Sub
End Module
