Imports System.Data

Namespace nsMakeRTF
    Module rtf

        Public zielroot As String = serverUNC & "nkat\aid\"
        Function makeRtfDoku(sdObj As List(Of clsSachdaten), titel As String, objid As String) As FlowDocument
            'Dim rtb2 As New RichTextBox
            Dim grossfont As Integer = 20
            Dim kleinfont As Integer = 12
            Dim mittelfont As Integer = 18

            Dim flowDoc As New FlowDocument
            'flowDoc.IsEnabled = True

            Dim paraHeader = New Paragraph()

            'Header			Aktennotiz
            paraHeader = New Paragraph()
            paraHeader.FontFamily = New FontFamily("Arial")
            paraHeader.FontSize = grossfont
            paraHeader.FontWeight = FontWeights.Bold
            paraHeader.Inlines.Add(New Run("Datenbankinformation zum Objekt: " & objid))
            flowDoc.Blocks.Add(paraHeader)



            ''Header			Aktennotiz
            'paraHeader = New Paragraph()
            'paraHeader.FontFamily = New FontFamily("Arial")
            'paraHeader.FontSize = grossfont
            'paraHeader.FontWeight = FontWeights.Bold
            'paraHeader.Inlines.Add(New Run("_____________"))
            'flowDoc.Blocks.Add(paraHeader)


            'paraHeader.Inlines.Add(New LineBreak())
            'flowDoc.Blocks.Add(paraHeader)
            createTable(sdObj, flowDoc, titel, grossfont, kleinfont, mittelfont, 10)
            Dim ausgabeDIR As String = zielroot & "" & "rtftest"   '"L:\rtfs\d\" '& "" & aid

            Return flowDoc
        End Function

        Private Sub createTable(sdObj As List(Of clsSachdaten), flowDoc As FlowDocument,
                                titel As String,
                                grossfont As Integer, kleinfont As Integer,
                                mittelfont As Integer,
                                cellspace As Integer)

            '
            'Create the Table...
            'www.c-sharpcorner.com/Resources/801/adding-a-table-to-a-wpf-document.aspx
            '

            Debug.Print(sdObj.Count.ToString)
            Dim numbOfRows As Integer = sdObj.Count
            Dim numberOfColumns As Integer = 3
            Dim table1 = New Table()
            Dim tempdat As String
            Dim currentRow As TableRow

            Dim para As Paragraph
            Dim hlink As Hyperlink

            Dim mbutton As Button
            Try
                l("createTable---------------------- anfang")
                ' ...And add it to the FlowDocument Blocks collection.
                flowDoc.Blocks.Add(table1)
                ' Set some global formatting properties for the table.
                table1.CellSpacing = cellspace
                table1.Background = Brushes.White

                ' Create And add an empty TableRowGroup to hold the table's Rows.
                table1.RowGroups.Add(New TableRowGroup())

                ' Add the first (title) row.
                table1.RowGroups(0).Rows.Add(New TableRow())

                ' Alias the current working row for easy reference.
                currentRow = table1.RowGroups(0).Rows(0)

                ' Global formatting for the title row.
                currentRow.Background = Brushes.Silver
                currentRow.FontSize = grossfont
                currentRow.FontWeight = System.Windows.FontWeights.Bold

                ' Add the header row with content, 
                currentRow.Cells.Add(New TableCell(New Paragraph(New Run(titel))))
                'And set the row to span all 6 columns.
                currentRow.Cells(0).ColumnSpan = numberOfColumns


                For i = 0 To numbOfRows - 1
                    ' Add the third row.
                    table1.RowGroups(0).Rows.Add(New TableRow())

                    If i Mod 2 = 0 Then
                        table1.RowGroups(0).Rows(i + 1).Background = Brushes.Beige
                    Else
                        table1.RowGroups(0).Rows(i + 1).Background = Brushes.LightSteelBlue
                    End If

                    currentRow = table1.RowGroups(0).Rows(i + 1)

                    ' Global formatting for the row.
                    currentRow.FontSize = kleinfont
                    currentRow.FontWeight = FontWeights.Normal

                    tempdat = sdObj(i).feldinhalt
                    If tempdat.ToLower.EndsWith("/.pdf") Then
                        tempdat = ""
                    End If
                    If tempdat.ToLower.EndsWith(".pdf") Or
                        tempdat.ToLower.EndsWith(".application") Or
                        tempdat.ToLower.EndsWith(".tiff") Then
                        'tempdat = "i am button"
                        para = New Paragraph()
                        mbutton = New Button()
                        flowDoc.Blocks.Add(para)

                        mbutton.IsEnabled = True
                        'mbutton.Inlines.Add("Button: " & Environment.NewLine & tempdat)

                        mbutton.Content = "Datei zeigen"

                        mbutton.ToolTip = tempdat

                        mbutton.Width = 100
                        mbutton.Height = 40

                        mbutton.Tag = tempdat
                        If tempdat.ToLower.EndsWith(".application") Then
                            mbutton.Content = "starten"
                            tempdat = ""
                            sdObj(i).feldinhalt = ""
                        End If
                        Try
                            'mbutton.NavigateUr = New Uri(tempdat)
                            AddHandler mbutton.Click, AddressOf buttonausfuehren
                            AddHandler mbutton.MouseDown, AddressOf buttonausfuehren
                        Catch ex As Exception
                            l("fehler in createTable link.NavigateUri: url unbrauchbar " & tempdat)
                        End Try

                        '   hlink.Cursor = Cursors.Hand

                        Dim container As InlineUIContainer = New InlineUIContainer(mbutton)
                        ' rtNotes.CaretPosition.Paragraph.Inlines.Add(container);
                        currentRow.FontSize = grossfont
                        currentRow.FontStyle = FontStyles.Italic
                        currentRow.FontWeight = FontWeights.Black
                        ' currentRow.Cells.Add(New TableCell(New Paragraph(New Run(sdObj(i).neuerFeldname))))
                        currentRow.Cells.Add(New TableCell(New Paragraph((container))))
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(" "))))
                        currentRow.FontStyle = FontStyles.Normal
                        currentRow.FontWeight = FontWeights.Normal
                    End If
                    If tempdat.Contains("http") AndAlso (Not tempdat.ToLower.EndsWith("/.pdf")) Then
                        para = New Paragraph()
                        hlink = New Hyperlink()
                        flowDoc.Blocks.Add(para)

                        hlink.IsEnabled = True
                        hlink.Inlines.Add("Hyperlink: " & Environment.NewLine & tempdat)
                        hlink.ToolTip = tempdat
                        Try
                            hlink.NavigateUri = New Uri(tempdat)
                            AddHandler hlink.Click, AddressOf linkausfuehren
                            AddHandler hlink.MouseDown, AddressOf linkausfuehren
                        Catch ex As Exception
                            l("fehler in createTable link.NavigateUri: url unbrauchbar " & tempdat)
                        End Try

                        hlink.Cursor = Cursors.Hand


                        currentRow.FontSize = grossfont
                        currentRow.FontStyle = FontStyles.Italic
                        currentRow.FontWeight = FontWeights.Black
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(sdObj(i).neuerFeldname))))
                        currentRow.Cells.Add(New TableCell(New Paragraph((hlink))))
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(" "))))
                        currentRow.FontStyle = FontStyles.Normal
                        currentRow.FontWeight = FontWeights.Normal

                    Else
                        If sdObj(i).neuerFeldname = "neueTabelle" Then
                            sdObj(i).neuerFeldname = "Zusatztabelle"
                            currentRow.FontSize = mittelfont
                            currentRow.FontStyle = FontStyles.Italic
                            currentRow.FontWeight = FontWeights.Black
                        Else
                            currentRow.FontSize = kleinfont
                            currentRow.FontStyle = FontStyles.Normal
                            currentRow.FontWeight = FontWeights.Normal
                        End If
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(sdObj(i).neuerFeldname))))
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(tempdat))))
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(" "))))
                    End If
                    If tempdat.IsNothingOrEmpty Then tempdat = " "
                    ' Add cells with content to the third row.

                Next



                l("createTable---------------------- ende")
            Catch ex As Exception
                l("Fehler in createTable: " & ex.ToString())
            End Try
        End Sub
        Private Sub buttonausfuehren(sender As Object, e As RoutedEventArgs)
            Dim link As Button = CType(sender, Button)
            If link.Tag.ToString = myglobalz.serverUNC & "startbplankataster.application" Then
                clsToolsAllg.startbplankataster()
            Else
                Try
                    l(" buttonausfuehren ---------------------- anfang")
                    If link.Tag.ToString.ToLower.EndsWith(".pdf") Or
                            link.Tag.ToString.ToLower.EndsWith(".tiff") Or
                               link.Tag.ToString.ToLower.EndsWith(".tif") Then
                        OpenDokument(link.Tag.ToString)
                    Else
                        Process.Start(link.Tag.ToString)
                    End If
                    l(" buttonausfuehren ---------------------- ende")
                Catch ex As Exception
                    l("Fehler in buttonausfuehren: " & link.Tag.ToString & ",," & ex.ToString())
                End Try
            End If
        End Sub
        Private Sub linkausfuehren(sender As Object, e As RoutedEventArgs)
            Dim link As Hyperlink = CType(sender, Hyperlink)
            Process.Start(link.NavigateUri.ToString)

        End Sub

        Private Sub createTableBAK(sdObj As List(Of clsSachdaten), flowDoc As FlowDocument)
            '
            'Create the Table...
            'www.c-sharpcorner.com/Resources/801/adding-a-table-to-a-wpf-document.aspx
            '
            Debug.Print(sdObj.Count.ToString)
            Dim table1 = New Table()
            ' ...And add it to the FlowDocument Blocks collection.
            flowDoc.Blocks.Add(table1)
            ' Set some global formatting properties for the table.
            table1.CellSpacing = 100
            table1.Background = Brushes.White

            Dim numberOfColumns As Integer = 6
            For i = 0 To numberOfColumns
                'Dim col As New TableColumn
                'col.
                'table1.Columns.Add(col)
                table1.Columns.Add(New TableColumn())
                ' Set alternating background colors for the middle colums.
                If i Mod 2 = 0 Then
                    table1.Columns(i).Background = Brushes.Beige
                Else
                    table1.Columns(i).Background = Brushes.LightSteelBlue
                End If
            Next
            ' Create And add an empty TableRowGroup to hold the table's Rows.
            table1.RowGroups.Add(New TableRowGroup())

            ' Add the first (title) row.
            table1.RowGroups(0).Rows.Add(New TableRow())

            ' Alias the current working row for easy reference.
            Dim currentRow As TableRow = table1.RowGroups(0).Rows(0)

            ' Global formatting for the title row.
            currentRow.Background = Brushes.Silver
            currentRow.FontSize = 40
            currentRow.FontWeight = System.Windows.FontWeights.Bold

            ' Add the header row with content, 
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("2004 Sales Project"))))
            ' And set the row to span all 6 columns.
            currentRow.Cells(0).ColumnSpan = 6

            ' Add the second (header) row.
            table1.RowGroups(0).Rows.Add(New TableRow())
            currentRow = table1.RowGroups(0).Rows(1)

            ' Global formatting for the header row.
            currentRow.FontSize = 18
            currentRow.FontWeight = FontWeights.Bold

            ' Add cells with content to the second row.
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("Product"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("Quarter 1"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("Quarter 2"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("Quarter 3"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("Quarter 4"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("TOTAL"))))

            ' Add the third row.
            table1.RowGroups(0).Rows.Add(New TableRow())
            currentRow = table1.RowGroups(0).Rows(2)

            ' Global formatting for the row.
            currentRow.FontSize = 12
            currentRow.FontWeight = FontWeights.Normal

            ' Add cells with content to the third row.
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("Widgets"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("$50,000"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("$55,000"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("$60,000"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("$65,000"))))
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("$230,000"))))

            ' Bold the first cell.
            currentRow.Cells(0).FontWeight = FontWeights.Bold

            table1.RowGroups(0).Rows.Add(New TableRow())
            currentRow = table1.RowGroups(0).Rows(3)

            ' Global formatting for the footer row.
            currentRow.Background = Brushes.LightGray
            currentRow.FontSize = 18
            currentRow.FontWeight = System.Windows.FontWeights.Normal

            ' Add the header row with content, 
            currentRow.Cells.Add(New TableCell(New Paragraph(New Run("Projected 2004 Revenue: $810,000"))))
            ' And set the row to span all 6 columns.
            currentRow.Cells(0).ColumnSpan = 6
            '###################
        End Sub

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

        Friend Function makeftlLegende4Aid(nlay As clsLayerPres) As String
            Dim aktlegende As New List(Of clsLegendenItem)
            Dim rtb2 As New RichTextBox
            Dim flw2 As New FlowDocument
            Dim paraHeader = New Paragraph()
            Dim datei, root As String
            root = myglobalz.serverUNC & "nkat\aid\" & nlay.aid
            datei = root & "\legende\"
            Try
                l("makeftlLegende4Aid---------------------- anfang")
                aktlegende = bildeLegendCollection(nlay.aid)
                paraHeader = New Paragraph()
                paraHeader.FontFamily = New FontFamily("Arial")
                paraHeader.FontSize = 22
                paraHeader.FontWeight = FontWeights.Bold
                paraHeader.Inlines.Add(New Run(("Legende zu " & nlay.titel)))
                flw2.Blocks.Add(paraHeader)

                paraHeader.Inlines.Add(New LineBreak())
                If aktlegende.Count = 1 Then
                    If aktlegende(0).titel = "Sammellegende" Then
                        Dim ziel = datei & "1.pdf"
                        If OpenDokument(ziel) Then
                            Return ""
                        Else
                            Return "error"
                        End If

                    End If
                End If
                For Each nleg As clsLegendenItem In aktlegende
                    Dim ziel = datei & nleg.nr & ".png"
                    diaanlegen(ziel, flw2, paraHeader, nleg.titel)
                    ' aid = nleg.aid
                Next
                flw2.Blocks.Add(paraHeader)
                rtb2.Document = flw2
                rtb2.IsDocumentEnabled = True
                Dim ausgabeDIR As String = My.Computer.FileSystem.SpecialDirectories.Temp '& "" & aid
                IO.Directory.CreateDirectory(ausgabeDIR)
                Dim ausgabedatei As String = ausgabeDIR & "\" & nlay.aid & ".rtf"
                Dim filename As String = schreibeInRTFDatei(rtb2, ausgabedatei$)
                If Not filename.StartsWith("Fehler") Then
                    '  Process.Start(filename)
                    'End
                End If
                Return filename
                l("makeftlLegende4Aid---------------------- ende")
            Catch ex As Exception
                l("Fehler in makeftlLegende4Aid: " & ex.ToString())
                Return ""
            End Try
        End Function
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
                l("Fehler ind diaanlegen " & ex.ToString)
            End Try
        End Sub
        Private Function bildeLegendCollection(daaid As Integer) As List(Of clsLegendenItem)
            Dim newleg As New clsLegendenItem
            Dim aktlegende As New List(Of clsLegendenItem)
            Try
                l("bildeLegendCollection---------------------- anfang")
                Dim dt As DataTable
                dt = getDTFromWebgisDB("SELECt * FROM  legenden  where aid=" & daaid & " order by nr", "webgiscontrol")
                For i = 0 To dt.Rows.Count - 1
                    newleg = New clsLegendenItem
                    newleg.aid = CInt(CStr(clsDBtools.fieldvalue(dt.Rows(i).Item("aid"))))
                    newleg.nr = CInt(CStr(clsDBtools.fieldvalue(dt.Rows(i).Item("nr"))))
                    newleg.titel = (CStr(clsDBtools.fieldvalue(dt.Rows(i).Item("titel")))).Trim
                    If newleg.aid = 0 OrElse newleg.nr = 0 Then
                        Continue For
                    End If
                    aktlegende.Add(newleg)
                Next
                If aktlegende.Count < 1 Then
                    'kein eintrag in der legenden Tabelle
                    newleg = New clsLegendenItem
                    newleg.aid = daaid
                    newleg.nr = 1
                    newleg.titel = "Sammellegende"
                    aktlegende.Add(newleg)
                End If
                Return aktlegende
                l("bildeLegendCollection---------------------- ende")
            Catch ex As Exception
                l("Fehler in bildeLegendCollection : " & ex.ToString())
                Return Nothing
            End Try
        End Function

        Function htm2cr(text As String) As String
            Dim neu As String = ""
            Try
                If text.IsNothingOrEmpty Then
                    '  l(text)
                    l("htm2cr text is nothing")
                    Return ""
                End If
                neu = text.Replace("<a target=_blank ", " ")
                neu = neu.Replace("&ndash;", " ")
                neu = neu.Replace("&nbsp;", " ")
                neu = neu.Replace("href=", " ")

                neu = neu.Replace("</a>", " ")
                neu = neu.Replace("<br>", Environment.NewLine)
                neu = neu.Replace("<ul>", " - " & Environment.NewLine)
                neu = neu.Replace("</ul>", Environment.NewLine)
                neu = neu.Replace("<li>", " - " & Environment.NewLine)
                neu = neu.Replace("</li>", Environment.NewLine)
                neu = neu.Replace("<b>", Environment.NewLine)
                neu = neu.Replace("</b>", Environment.NewLine)
                neu = neu.Replace(">", " ")
                ' l("htm2cr-----------ende")
                Return neu
            Catch ex As Exception
                l("fehler in htm2cr >" & CStr(text) & "<" & ex.ToString)
                Return ""
            End Try
        End Function
    End Module
End Namespace