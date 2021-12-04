Imports System.Data
Imports System.Threading.Tasks

Namespace nsMakeRTF
    Module rtf

        Public zielroot As String = serverUNC & "nkat\aid\"
        Function makeRtfDoku(sdObj As List(Of clsSachdaten), titel As String, objid As String, aid As Integer) As FlowDocument
            'Dim rtb2 As New RichTextBox
            Dim grossfont As Integer = 18
            Dim kleinfont As Integer = 12
            Dim mittelfont As Integer = 16

            Dim flowDoc As New FlowDocument
            'flowDoc.IsEnabled = True

            Dim paraHeader = New Paragraph()

            'Header			Aktennotiz
            paraHeader = New Paragraph With {
                .FontFamily = New FontFamily("Arial"),
                .FontSize = grossfont,
                .FontWeight = FontWeights.Bold
            }
            'paraHeader.Inlines.Add(New Run("Datenbankinformation zum Objekt: " & objid))
            'flowDoc.Blocks.Add(paraHeader)


            createTable(sdObj, flowDoc, titel, grossfont, kleinfont, mittelfont, 10)

            Return flowDoc
        End Function

        Private Sub createTable(sdorig As List(Of clsSachdaten), flowDoc As FlowDocument,
                                titel As String,
                                grossfont As Integer, kleinfont As Integer,
                                mittelfont As Integer,
                                cellspace As Integer)

            '
            'Create the Table...
            'www.c-sharpcorner.com/Resources/801/adding-a-table-to-a-wpf-document.aspx
            '
            Dim sdkopie As New List(Of clsSachdaten)
            Debug.Print(sdorig.Count.ToString)
            Dim numbOfRows As Integer = sdorig.Count
            Dim numberOfColumns As Integer = 2
            Dim table1 = New Table()
            Dim tempdat As String
            Dim currentRow As TableRow

            Dim para As Paragraph
            Dim hlink As Hyperlink

            Dim mbutton As Button
            Try
                sdkopie = kopiereListe(sdorig)
                l("createTable---------------------- anfang")
                ' ...And add it to the FlowDocument Blocks collection.
                flowDoc.Blocks.Add(table1)
                ' Set some global formatting properties for the table.
                table1.CellSpacing = 0 'cellspace
                table1.Background = Brushes.White
                'table1.BorderThickness = New Thickness(2)
                'table1.BorderBrush = Brushes.Black
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
                        table1.RowGroups(0).Rows(i + 1).Background = Brushes.Beige 'Brushes.LightSteelBlue
                    End If

                    currentRow = table1.RowGroups(0).Rows(i + 1)


                    ' Global formatting for the row.
                    currentRow.FontSize = kleinfont
                    currentRow.FontWeight = FontWeights.Normal

                    tempdat = sdkopie(i).feldinhalt
                    If tempdat.ToLower.EndsWith("/.pdf") Then
                        tempdat = ""
                    End If
                    If tempdat.ToLower.EndsWith(".pdf") Or
                       tempdat.ToLower.EndsWith(".application") Or
                       tempdat.ToLower.EndsWith(".jpg") Or
                       tempdat.ToLower.EndsWith(".tiff") Or
                       tempdat.ToLower.EndsWith(".html") Then
                        'tempdat = "i am button"
                        para = New Paragraph()
                        'para.BorderBrush = Brushes.Black
                        'para.BorderThickness = New Thickness(2)
                        mbutton = New Button()
                        flowDoc.Blocks.Add(para)

                        mbutton.IsEnabled = True
                        mbutton.Content = "datei zeigen"
                        mbutton.FontSize = 10
                        mbutton.ToolTip = tempdat

                        mbutton.Width = 100
                        mbutton.Height = 40

                        mbutton.Tag = tempdat
                        If tempdat.ToLower.EndsWith(".application") Then
                            mbutton.Content = "starten"
                            tempdat = ""
                            sdkopie(i).feldinhalt = ""
                        End If
                        Try
                            'mbutton.navigateur = new uri(tempdat)
                            AddHandler mbutton.Click, AddressOf buttonausfuehrenAsync
                            AddHandler mbutton.MouseDown, AddressOf buttonausfuehrenAsync
                        Catch ex As Exception
                            l("fehler in createtable link.navigateuri: url unbrauchbar " & tempdat)
                        End Try

                        '   hlink.cursor = cursors.hand

                        Dim container As InlineUIContainer = New InlineUIContainer(mbutton)
                        ' rtnotes.caretposition.paragraph.inlines.add(container);
                        currentRow.FontSize = grossfont
                        currentRow.FontStyle = FontStyles.Italic
                        currentRow.FontWeight = FontWeights.Black
                        ' currentrow.cells.add(new tablecell(new paragraph(new run(sdobj(i).neuerfeldname))))
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
                        If clsSachdatentools.istBaulastTiff(tempdat) Then
                            hlink.Inlines.Add(" " & Environment.NewLine & "")
                        Else
                            'hlink.Inlines.Add("Hyperlink: " & Environment.NewLine & tempdat)
                            hlink.FontSize = 8
                        End If

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
                        Dim runn As New Run(sdkopie(i).neuerFeldname)
                        runn.FontSize = 12

                        If hlink.FontSize = 8 Then
                            currentRow.Cells.Add(New TableCell(New Paragraph(runn)))
                        Else
                            currentRow.Cells.Add(New TableCell(New Paragraph(runn)))
                        End If
                        currentRow.Cells.Add(New TableCell(New Paragraph((hlink))))
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(" "))))
                        currentRow.FontStyle = FontStyles.Normal
                        currentRow.FontWeight = FontWeights.Normal
                    Else
                        If sdkopie(i).neuerFeldname = "neueTabelle" Then
                            sdkopie(i).neuerFeldname = "Zusatztabelle"
                            currentRow.FontSize = mittelfont
                            currentRow.FontStyle = FontStyles.Italic
                            currentRow.FontWeight = FontWeights.Black

                        Else
                            currentRow.FontSize = kleinfont
                            currentRow.FontStyle = FontStyles.Normal
                            currentRow.FontWeight = FontWeights.Normal

                        End If
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(sdkopie(i).neuerFeldname))))
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(tempdat))))
                        currentRow.Cells.Add(New TableCell(New Paragraph(New Run(" "))))
                    End If
                    For j = 0 To currentRow.Cells.Count - 1
                        currentRow.Cells(j).BorderThickness = New Thickness(0, 1, 1, 1)
                        currentRow.Cells(j).BorderBrush = Brushes.LightGray
                        currentRow.Cells(j).ColumnSpan = 1

                    Next
                    If tempdat.IsNothingOrEmpty Then tempdat = " "
                Next




                l("createTable---------------------- ende")
            Catch ex As Exception
                l("Fehler in rtf createTable: " & ex.ToString())
            End Try
        End Sub

        Friend Function kopiereListe(sdObj As List(Of clsSachdaten)) As List(Of clsSachdaten)
            Dim neu As New List(Of clsSachdaten)
            Dim item As New clsSachdaten
            Try
                For Each ele As clsSachdaten In sdObj
                    item = CType(ele.Clone, clsSachdaten)
                    neu.Add(item)
                Next
                Return neu
            Catch ex As Exception
                l("Fehler in rtf kopiereListe: " & ex.ToString())
                Return Nothing
            End Try
        End Function

        Public Async Function buttonausfuehrenAsync(sender As Object, e As RoutedEventArgs) As Task
            Dim link As Button = CType(sender, Button)
            Dim lokaleDatei As String = ""
            If link.Tag.ToString = myglobalz.serverUNC & "startbplankataster.application" Or
                link.Tag.ToString.ToLower.EndsWith("startbplankataster.application") Then
                clsToolsAllg.startbplankataster()
                Return
            End If
            Try
                l(" buttonausfuehren ---------------------- anfang")
                If link.Tag.ToString.ToLower.EndsWith(".pdf") Or
                    link.Tag.ToString.ToLower.EndsWith(".tiff") Or
                    link.Tag.ToString.ToLower.EndsWith(".tif") Then
                    'If iminternet Then
                    lokaleDatei = Await handleMediumAsync(link, lokaleDatei, False) 'false wg. lsg verordnung ist neu
                    'Threading.Thread.Sleep(2000)
                    If lokaleDatei.IsNothingOrEmpty Then
                        Process.Start(link.Tag.ToString)
                    Else
                        If lokaleDatei.StartsWith("http") Then
                            Process.Start(lokaleDatei)
                        Else

                            OpenDokument(lokaleDatei)
                        End If

                    End If
                Else
                    Process.Start(link.Tag.ToString)
                End If
                l(" buttonausfuehren ---------------------- ende")
            Catch ex As Exception
                l("Fehler in buttonausfuehren: " & link.Tag.ToString & ",," & ex.ToString())
            End Try

        End Function

        Friend Async Function handleMediumAsync(link As Button, lokaleDatei As String, usedownloadcache As Boolean) As Task(Of String)
            If link.Tag.ToString.ToLower.Contains("bplan") Then
                lokaleDatei = clsSachdatentools.makeLokalBplaneDatei(link.Tag.ToString.ToLower, usedownloadcache)
                'Dim loktask As Task(Of String) = clsSachdatentools.makeLokalBplaneDatei(link.Tag.ToString.ToLower)
                'lokaleDatei = Await loktask
                Return lokaleDatei
            End If
            If link.Tag.ToString.ToLower.Contains("baulasten") Then
                'http://w2gis02.kreis-of.local/fkat/baulasten/Langen/60825.tiff
                lokaleDatei = makeLokalBaulastenDatei(link.Tag.ToString.ToLower)

                Return lokaleDatei
            End If

            If link.Tag.ToString.ToLower.Contains("nkat\aid") Or
                link.Tag.ToString.ToLower.Contains("nkat/aid") Then
                lokaleDatei = makeLokalAIDDatei(link.Tag.ToString.ToLower, usedownloadcache)

                Return lokaleDatei
            End If
            If link.Tag.ToString.ToLower.StartsWith("http") Then
                If link.Tag.ToString.ToLower.Contains("geodaten.kreis-offenbach.de") Then
                    lokaleDatei = makeLokalGEODATENDatei(link.Tag.ToString.ToLower)
                Else

                    lokaleDatei = link.Tag.ToString
                End If
            End If
            Return lokaleDatei
        End Function

        Private Function makeLokalGEODATENDatei(link As String) As String
            'http://geodaten.kreis-offenbach.de/natura2000/allgemeiner_VO_Text/Natura2000-VO-Text_allgemeiner_Teil.pdf
            Dim lokaleDatei As String
            Dim zieldir = "", zieldatei As String = ""
            Dim erfolg As Boolean
            Try
                l(" MOD makeLokalGEODATENDatei anfang")
                erfolg = clsSachdatentools.getDownloadtargetGeodaten(link,
                                                                       strGlobals.localDocumentCacheRoot,
                                                                      zieldir,
                                                                      zieldatei)
                If clsSachdatentools.schonImCache(zieldir, zieldatei, True) Then
                    lokaleDatei = zieldir & "\" & zieldatei
                Else
                    If erfolg Then
                        If meineHttpNet.down(link, zieldatei, zieldir) Then
                            l("makeLokalGEODATENDatei downlaod erfolgreich")
                            lokaleDatei = (zieldir & "\" & zieldatei).Replace("\\", "\")
                        Else
                            l("makeLokalGEODATENDatei downlaod nicht erfolgreich")
                            'Return False
                            lokaleDatei = ""
                        End If
                    Else
                        l("Fehler makeLokalGEODATENDatei konnte nicht berechnet werden. " & link.ToString)
                        lokaleDatei = ""
                    End If
                End If
                Return lokaleDatei
                l(" MOD makeLokalGEODATENDatei ende")
                'Return zieldir & "\" & zieldatei
            Catch ex As Exception
                l("Fehler in makeLokalGEODATENDatei: " & ex.ToString())
                Return ""
            End Try
        End Function

        Private Function makeLokalAIDDatei(link As String, usecache As Boolean) As String
            ' '\\w2gis02\gdvell\\nkat\aid\341\texte\33-1996b.pdf
            Dim lokaleDatei As String
            Dim zieldir = "", zieldatei As String = ""
            Dim erfolg As Boolean
            Try
                l(" MOD makeLokalAIDDatei anfang")
                erfolg = clsSachdatentools.getDownloadtargetAID(link,
                                                                strGlobals.localDocumentCacheRoot,
                                                                zieldir,
                                                                zieldatei)
                If clsSachdatentools.schonImCache(zieldir, zieldatei, usecache) Then
                    lokaleDatei = zieldir & "\" & zieldatei
                Else
                    If erfolg Then
                        If meineHttpNet.down(link, zieldatei, zieldir) Then
                            l("makeLokalBaulastenDatei downlaod erfolgreich")
                            lokaleDatei = (zieldir & "\" & zieldatei).Replace("\\", "\")
                        Else
                            l("makeLokalBaulastenDatei downlaod nicht erfolgreich")
                            'Return False
                            lokaleDatei = ""
                        End If
                    Else
                        l("Fehler makeLokalBaulastenDateizieldatei konnte nicht berechnet werden. " & link.ToString)
                        lokaleDatei = ""
                    End If
                End If
                Return lokaleDatei
                l(" MOD makeLokalAIDDatei ende")
                'Return zieldir & "\" & zieldatei
            Catch ex As Exception
                l("Fehler in makeLokalAIDDatei: " & ex.ToString())
                Return ""
            End Try
        End Function

        Private Function makeLokalBaulastenDatei(link As String) As String
            Dim lokaleDatei As String
            Dim zieldir = "", zieldatei As String = ""
            Dim erfolg As Boolean
            Try
                l(" MOD makeLokalBaulastenDatei anfang")
                link = link.Trim.ToLower.Replace(".tiff", ".pdf")
                link = link.Trim.ToLower.Replace("/paradigmacache", "/fkat")
                erfolg = clsSachdatentools.getDownloadtargetBaulasten(link,
                                                                      IO.Path.Combine(strGlobals.localDocumentCacheRoot, "baulasten"),
                                                                      zieldir,
                                                                      zieldatei)

                ' >> CACHE ist aus, weil ständige aktualisierung
                'If clsSachdatentools.schonImCache(zieldir, zieldatei, True) Then
                '    lokaleDatei = zieldir & "\" & zieldatei
                'Else
                If erfolg Then
                        If meineHttpNet.down(link, zieldatei, zieldir) Then
                            l("makeLokalBaulastenDatei downlaod erfolgreich")
                            lokaleDatei = zieldir & "\" & zieldatei
                        Else
                            l("makeLokalBaulastenDatei downlaod nicht erfolgreich")
                            'Return False
                            lokaleDatei = ""
                        End If
                    Else
                        l("Fehler makeLokalBaulastenDateizieldatei konnte nicht berechnet werden. " & link.ToString)
                        lokaleDatei = ""
                    End If
                'End If
                Return lokaleDatei
                l(" MOD makeLokalBaulastenDatei ende")
            Catch ex As Exception
                l("Fehler in makeLokalBaulastenDatei: " & ex.ToString())
                Return ""
            End Try
        End Function

        Friend Sub linkausfuehren(sender As Object, e As RoutedEventArgs)
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

        Public Function schreibeInRTFDatei(ByVal rtb2 As RichTextBox, ByVal dateiname As String) As Boolean
            Try
                Dim fs As IO.FileStream
                fs = New IO.FileStream(dateiname, IO.FileMode.Create)
                Dim tr As New TextRange(rtb2.Document.ContentStart, rtb2.Document.ContentEnd)
                tr.Save(fs, DataFormats.Rtf)
                rtb2.Selection.Save(fs, DataFormats.Rtf)
                fs.Close()
                'Return dateiname
                Return True
            Catch ex As Exception
                l("Fehler  schreibeInRTFDatei: " & dateiname, ex)
                Return False
            End Try
        End Function

        Friend Function makeftlLegende4Aid(nlay As clsLayerPres, format As String, dokHtml As String) As String
            Dim aktlegende As New List(Of clsLegendenItem)
            Dim datei As String = "", root As String = "", hinweis As String = ""
            Dim ausgabedatei As String = ""
            Try
                l("makeftlLegende4Aid---------------------- anfang")
                If iminternet Or CGIstattDBzugriff Then
                    aktlegende = clsLegendenTools.getLegendeFromHTTP(nlay.aid, hinweis)
                    If aktlegende Is Nothing Then
                        Return ""
                    End If
                    root = myglobalz.serverWeb & "\nkat\aid\" & nlay.aid
                    datei = (root & "\legende\").Replace("\", "/")
                Else
                    aktlegende = bildeLegendCollectionDB(nlay.aid)
                    root = myglobalz.serverUNC & "nkat\aid\" & nlay.aid
                    datei = root & "\legende\"
                End If
                Dim erfolg As Boolean = False
                If format = "rtf" Then
                    Dim rtb2 As New RichTextBox
                    rtb2 = makeLegendeRTF(aktlegende, datei, nlay.titel)
                    If rtb2 IsNot Nothing Then
                        Dim ausgabeDIR As String = My.Computer.FileSystem.SpecialDirectories.Temp '& "" & aid
                        IO.Directory.CreateDirectory(ausgabeDIR)
                        ausgabedatei = ausgabeDIR & "\" & nlay.aid & ".rtf"
                        erfolg = schreibeInRTFDatei(rtb2, ausgabedatei)
                        If Not erfolg Then
                            ausgabedatei = ""
                        End If
                    End If
                End If
                If format = "html" Then
                    Dim strHtml As String = ""
                    If aktlegende.Count = 1 Then
                        If aktlegende(0).titel = "Sammellegende" Then
                            datei = datei & "1.pdf"
                            'If OpenDokument(ziel) Then
                            '    Return Nothing
                            'Else
                            '    Return Nothing
                            'End If

                        End If
                    End If
                    strHtml = makeLegendeHTML(aktlegende, datei, nlay.titel, dokHtml)
                    'Dim ausgabeDIR As String = My.Computer.FileSystem.SpecialDirectories.Temp '& "" & aid
                    Dim ausgabeDIR As String = strGlobals.localDocumentCacheRoot '& "" & aid

                    IO.Directory.CreateDirectory(ausgabeDIR)
                    ausgabedatei = ausgabeDIR & "\leg_" & nlay.aid & ".html"
                    IO.File.WriteAllText(ausgabedatei, strHtml)
                    IO.File.WriteAllText(ausgabedatei.Replace(".html", ".docx"), clsString.changeUmlaut2Html(strHtml))
                End If
                Return ausgabedatei
                l("makeftlLegende4Aid---------------------- ende")
            Catch ex As Exception
                l("Fehler in makeftlLegende4Aid: " & ex.ToString())
                Return ""
            End Try
        End Function

        Private Function makeLegendeHTML(aktlegende As List(Of clsLegendenItem), datei As String, titel As String, dokhtml As String) As String
            Dim template As String = "<!DOCTYPE html><html><head><title>[TITEL]</title></head><body><H1>[TITEL]</H1><table style='width:100%'>[TABLEBODY]</table></body></html>"
            template = "<html><head> <meta charset='utf-8'>" &
                "<title>[TITEL]</title>" &
                "<style> img { vertical-align: text-top; float: left;}    </style>" &
                "</head>" &
                "<body><H1>[TITEL]</H1>" &
                "<table style='width:100%'>[TABLEBODY]</table>" &
                "[DOKU]</body></html>"
            Dim tabBody As New Text.StringBuilder
            Dim ziel As String = ""
            Dim final As String = ""
            Try
                final = template
                If aktlegende Is Nothing OrElse aktlegende.Count < 1 Then
                    tabBody.Append("<tr>")
                    tabBody.Append("<td>")
                    tabBody.Append("<p> Keine Legende vorhanden </p>")
                    'tabBody.Append(nleg.titel)
                    tabBody.Append("</td>")
                    tabBody.Append("</tr>")
                Else
                    If aktlegende.Count = 1 And datei.Trim.ToLower.EndsWith(".pdf") Then

                        tabBody.Append("<tr>")
                            tabBody.Append("<td>")
                        tabBody.Append("<a target=_blank href='" & datei & " '>Sammellegende</a>")

                        tabBody.Append("</td>")
                            tabBody.Append("</tr>")

                    Else
                        For Each nleg As clsLegendenItem In aktlegende
                            ziel = datei & nleg.nr & ".png"
                            tabBody.Append("<tr>")
                            tabBody.Append("<td>")
                            tabBody.Append("<img class='leg_eintrag'  src='" & ziel & "'>")
                            tabBody.Append(nleg.titel)
                            tabBody.Append("</td>")
                            tabBody.Append("</tr>")
                        Next
                    End If

                End If

                final = final.Replace("[TITEL]", titel)
                final = final.Replace("[TABLEBODY]", tabBody.ToString)
                final = final.Replace("[DOKU]", dokhtml)
                Return final
            Catch ex As Exception
                l("Fehler in makeLegendeHTML: " & ex.ToString())
                Return ""
            End Try
        End Function

        Private Function makeLegendeRTF(aktlegende As List(Of clsLegendenItem), datei As String, titel As String) As RichTextBox
            Try
                Dim flw2 As New FlowDocument
                Dim paraHeader = New Paragraph()
                Dim rtb2 As New RichTextBox
                paraHeader = New Paragraph()
                paraHeader.FontFamily = New FontFamily("Arial")
                paraHeader.FontSize = 22
                paraHeader.FontWeight = FontWeights.Bold
                paraHeader.Inlines.Add(New Run(("Legende zu " & titel)))
                flw2.Blocks.Add(paraHeader)

                paraHeader.Inlines.Add(New LineBreak())
                If aktlegende.Count = 1 Then
                    If aktlegende(0).titel = "Sammellegende" Then
                        Dim ziel = datei & "1.pdf"
                        If OpenDokument(ziel) Then
                            Return Nothing
                        Else
                            Return Nothing
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
                Return rtb2
            Catch ex As Exception
                l("fehler in makeLegendeRTF ", ex)
                Return Nothing
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
                nr = New Run(htm2cr(CType("    " & Titel, String)))
                nr.FontWeight = FontWeights.Normal : paraHeader.Inlines.Add(nr)
                paraHeader.Inlines.Add(New LineBreak())
                flw2.Blocks.Add(paraHeader)
                image = Nothing
                bimg = Nothing

            Catch ex As Exception
                l("Fehler ind diaanlegen ", ex)
            End Try
        End Sub
        Private Function bildeLegendCollectionDB(lokaid As Integer) As List(Of clsLegendenItem)
            Dim newleg As New clsLegendenItem
            Dim aktlegende As New List(Of clsLegendenItem)
            Try
                l("bildeLegendCollection---------------------- anfang")
                Dim dt As DataTable
                dt = getDTFromWebgisDB("SELECt * FROM  legenden  where aid=" & lokaid & " order by nr", "webgiscontrol")
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
                    newleg.aid = lokaid
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
                    'l("htm2cr text is nothing")
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
                l("fehler in htm2cr " & CStr(text), ex)
                Return ""
            End Try
        End Function

        Friend Function makeDokuHtml(nlay As clsLayerPres, ByRef tablebody As String, aid As Integer) As String
            Dim ndok As New clsDoku
            Dim strdok As String = ""
            Dim ausgabedatei As String = ""
            strdok = genhtmldokString(nlay, ndok, ausgabedatei, tablebody, aid)


            Dim ausgabeDIR As String = strGlobals.localDocumentCacheRoot   '& "" & aid

            IO.Directory.CreateDirectory(ausgabeDIR)
            ausgabedatei = ausgabeDIR & "\dok_" & nlay.aid & ".html"
            IO.File.WriteAllText(ausgabedatei, strdok)
            IO.File.WriteAllText(ausgabedatei.Replace(".html", ".docx"), clsString.changeUmlaut2Html(strdok))
            Return ausgabedatei
        End Function

        Private Function genhtmldokString(nlay As clsLayerPres, ByRef ndok As clsDoku,
                                          ByRef ausgabedatei As String,
                                          ByRef tablebody As String, aid As Integer) As String

            Dim template As String = "<!DOCTYPE html><html><head><title>[TITEL]</title></head><body><H1>[TITEL]</H1><table style='width:100%'>[TABLEBODY]</table></body></html>"
            template = "<html><head> <meta charset='utf-8'><title>[TITEL]</title>" &
                "<style>img {vertical-align: text-top;float: left;        }    </style></head>" &
                "<body><H1>[TITEL]</H1>" & "<table style='width:100%'>[TABLEBODY]</table>" &
                "[THUMBNAIL]" &
                "</body></html>"
            Dim tabBody As New Text.StringBuilder
            Dim ziel As String = ""
            Dim final As String = ""
            Dim thumbnail As String = ""
            Try
                ndok = clsWebgisPGtools.getDoku4aid(nlay.aid)
                final = template
                If ndok Is Nothing Then
                    tabBody.Append("Keine Dokumentation vorhanden")
                Else

                    tabBody.Append("<tr>")
                    tabBody.Append("<td class='leg_eintrag'    > Inhalt: </td>")
                    tabBody.Append("<td class='leg_eintrag'    > " & ndok.inhalt & "</td>")
                    tabBody.Append("</tr>")

                    tabBody.Append("<tr>")
                    tabBody.Append("<td class='leg_eintrag'    > Entstehung: </td>")
                    tabBody.Append("<td class='leg_eintrag'    > " & ndok.entstehung & "</td>")
                    tabBody.Append("</tr>")

                    tabBody.Append("<tr>")
                    tabBody.Append("<td class='leg_eintrag'    > Aktualitaet: </td>")
                    tabBody.Append("<td class='leg_eintrag'    > " & ndok.aktualitaet & "</td>")
                    tabBody.Append("</tr>")

                    tabBody.Append("<tr>")
                    tabBody.Append("<td class='leg_eintrag'    > Beschraenkungen: </td>")
                    tabBody.Append("<td class='leg_eintrag'    > " & ndok.beschraenkungen & "</td>")
                    tabBody.Append("</tr>")


                    tabBody.Append("<tr>")
                    tabBody.Append("<td class='leg_eintrag'    > Datenabgabe: </td>")
                    tabBody.Append("<td class='leg_eintrag'    > " & ndok.datenabgabe & "</td>")
                    tabBody.Append("</tr>")
                End If
                thumbnail = " <img src='" & myglobalz.serverWeb.Replace("\", "/") & "/nkat/thumbnails/" & nlay.aid & ".png" & "' alt='Beispielausschnitt aus dieser Ebene'  >"
                final = final.Replace("[THUMBNAIL]", thumbnail)
                final = final.Replace("[TITEL]", nlay.titel)
                final = final.Replace("[TABLEBODY]", tabBody.ToString)
                tablebody = "<h2>Dokumentation </h2><table style='width:100%'>" & tabBody.ToString & "</table> <p>" & thumbnail & "</p>"
                Return final
            Catch ex As Exception
                l("fehler in makeDokuHtml: ", ex)
                Return ""
            End Try
        End Function
    End Module
End Namespace