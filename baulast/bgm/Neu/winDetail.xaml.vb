Public Class winDetail
    Property VGmyBitmapImage As New BitmapImage
    Dim modus As String = "neu"

    Sub New(gisID As String)
        InitializeComponent()
        If IsNumeric(gisID) AndAlso CInt(gisID) < 1 Then
            modus = "neu"
        Else
            modus = "edit"
            tbBaulastNr.Text = CType(gisID, String)
        End If
    End Sub
    Sub New()
        InitializeComponent()
        modus = "edit"
        tbBaulastNr.Text = ""
    End Sub

    Private Sub winDetail_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        l("windetail loaded anfang")
        btndigit.Visibility = Visibility.Collapsed
#If DEBUG Then
        If tbBaulastNr.Text.IsNothingOrEmpty Then
            tbBaulastNr.Text = "2026"
        End If
#End If
        If IsNumeric(tbBaulastNr.Text) Then
            refreshProbaug(CInt(tbBaulastNr.Text))
            refreshGIS(CInt(tbBaulastNr.Text))
            refreshTIFFbox()
            'hier wird firstrange calculiert
            gidInString = clsGIStools.bildegidstring()
            range = clsGIStools.calcNewRange(gidInString)
            If Not range.istBrauchbar Then
                btndigit.Visibility = Visibility.Visible
                If My.Computer.Clipboard.ContainsText Then
                    tools.wkt = My.Computer.Clipboard.GetText()
                    If tools.wkt.Trim.ToLower.StartsWith("polygon") Then
                        btndigit.ToolTip = "Klick = Übernehmen dieser Geometrie als temporäres Flurstück !" & tools.wkt
                    Else
                        btndigit.ToolTip = "Das ist keine gültige Geometrie: " & tools.wkt
                    End If
                Else
                    MessageBox.Show("Sie können ein Flurstück selber markieren ! Näheres bei Frau Hartmann. ")
                End If
            End If
            refreshMap()
            tbEigentuemer.Text = toolsEigentuemer.geteigentuemertext(tools.FSTausGISListe)
        End If
        Title = "BGM: BaulastenGISManager 0.11. " & Environment.UserName & " V.: " & bgmVersion


        l("windetail loaded ende")
    End Sub

    Private Sub refreshTIFFbox()
        refreshTiffBitmap()
        If rawList.Count > 0 Then

            Dim fi As New IO.FileInfo(rawList(0).datei)
            If fi.Exists Then
                tbFiledate.Text = fi.LastWriteTime.ToShortDateString
            Else
                tbFiledate.Text = "fehlt"
            End If
        Else
            tbFiledate.Text = "keine gisdaten"
        End If
    End Sub

    Private Sub refreshGIS(BaulastBlattNr As Integer)
        dgAusGIS.DataContext = Nothing
        tools.FSTausGISListe.Clear()
        Dim hinweis As String = ""

        hinweis = clsGIStools.getGISrecord(BaulastBlattNr)
        'If tools.FSTausGISListe.Count < 1 Then
        '    tools.FSTausGISListeFehlt = clsGIStools.fromProbauGObjekt(FSTausPROBAUGListe)
        'Else
        tools.FSTausGISListe = clsGIStools.fstGIS2OBJ()
        'End If
        dgAusGIS.DataContext = tools.FSTausGISListe
        l("getSerialFromBasis---------------------- ende")
    End Sub



    Private Sub btnAusProbaug_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        refreshGIS(CInt(tbBaulastNr.Text))
        gidInString = clsGIStools.bildegidstring()
        range = clsGIStools.calcNewRange(gidInString)
        refreshall()
    End Sub

    Private Sub refreshall()
        refreshProbaug(CInt(tbBaulastNr.Text))
        refreshGIS(CInt(tbBaulastNr.Text))
        refreshTIFFbox()
        refreshMap()
    End Sub
    Private Sub clearCanvas()
        GC.Collect()
        VGmapCanvas.Children.Clear()
        If VGcanvasImage IsNot Nothing Then
            VGcanvasImage.Source = Nothing
            VGcanvasImage = Nothing
        End If
        VGcanvasImage = New Image
        leeresbild(VGcanvasImage)

    End Sub
    Private Sub leeresbild(canvasImage As Image)
        Dim myBitmapImage As New BitmapImage()
        Dim aufruf As String = tools.srv_host_web & "/apps/paradigma/ndman/leer.png" '"P:\a_vs\NEUPara\mgis\leer.png"
        Try
            myBitmapImage.BeginInit()
            myBitmapImage.UriSource = New Uri(aufruf, UriKind.Absolute)
            myBitmapImage.EndInit()
            canvasImage.Source = myBitmapImage
            GC.Collect()
        Catch ex As Exception
            l("fehler in leeresbild: " & aufruf & " /// " & ex.ToString)
        End Try
    End Sub
    Private Sub setPreviewImageFromHttpURL(url As String)
        'https mach tprobleme
        'Dim VGcanvasImage = New Image
        Try
            l(" setImageFromHttpURL ---------------------- anfang")
            clearCanvas()
            VGcanvasImage = New Image
            VGcanvasImage.Name = "canvasImage"
            VGmapCanvas.Children.Add(VGcanvasImage)
            VGmapCanvas.SetZIndex(VGcanvasImage, 100)

            VGmyBitmapImage = New BitmapImage
            VGmyBitmapImage.BeginInit()
            VGmyBitmapImage.UriSource = New Uri(url, UriKind.Absolute)
            VGmyBitmapImage.EndInit()
            AddHandler VGmyBitmapImage.DownloadCompleted, AddressOf vgmyBitmapImage_DownloadCompleted
            Threading.Thread.Sleep(900)
            'VGcanvasImage.Source = VGmyBitmapImage
            l(" setImageFromHttpURL ---------------------- ende")
        Catch ex As Exception
            l("Fehler in setImageFromHttpURL: " & ex.ToString())
        End Try
    End Sub
    Private Sub vgmyBitmapImage_DownloadCompleted(sender As Object, e As EventArgs)
        VGcanvasImage.Source = VGmyBitmapImage
        'clstools.saveImageasThumbnail2(clstools.auswahlBplan, clstools.BPLcachedir, VGmyBitmapImage)
    End Sub
    Private Sub refreshMap()


        Dim url As String = mapTools.genPreviewURL(tools.range, CInt(VGmapCanvas.Width), CInt(VGmapCanvas.Height), "flurkarte", 0, tools.gidInString)
        setPreviewImageFromHttpURL(url)
        Canvas.SetTop(VGcanvasImage, 0)
        Canvas.SetLeft(VGcanvasImage, 0)
    End Sub

    Private Sub refreshProbaug(baulastblattnr As Integer)

        Try
            l(" MOD refreshProbaug anfang")
            dgAusProbaug.DataContext = Nothing
            tools.FSTausPROBAUGListe.Clear()
            clsProBGTools.holeProBaugDaten(baulastblattnr)
            dgAusProbaug.DataContext = FSTausPROBAUGListe
            tbBauort.Text = rawList(0).bauortNr
            tbDatum1.Text = rawList(0).datum1
            tbgueltig.Text = rawList(0).gueltig
            tbGemeinde.Text = rawList(0).gemeindeText
            tbBaulastNr2.Text = rawList(0).baulastnr
            tbBlattnr.Text = rawList(0).blattnr
            tblaufNR.Text = CType(rawList(0).laufnr, String)

            l(" MOD refreshProbaug ende")
        Catch ex As Exception
            l("Fehler in refreshProbaug: " & ex.ToString())
        End Try
    End Sub

    Public Function refreshTiffBitmap() As Boolean
        Dim bitmap As BitmapImage = New BitmapImage()

        Try
            l(" MOD refreshTiffBitmap anfang")
            If rawList(0).dateiExistiert Then
                'btnTiffaufrufen.Visibility = Visibility.Visible 
                bitmap.BeginInit()
                bitmap.CacheOption = BitmapCacheOption.OnLoad ' verhindert fehler beim löschen
                bitmap.UriSource = New Uri(rawList(0).datei)
                bitmap.EndInit()
                imgTiff.Source = bitmap
                bitmap = Nothing
                Return True
            Else
                'btnTiffaufrufen.Visibility = Visibility.Collapsed
                Return Nothing
            End If

            l(" MOD refreshTiffBitmap ende")
        Catch ex As Exception
            l("Fehler in refreshTiffBitmap: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Sub btnTiffaufrufen_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim fi As New IO.FileInfo(rawList(0).datei)
        If fi.Exists Then
            Process.Start(rawList(0).datei)
        Else
            MessageBox.Show("Datei fehlt!!")
        End If
    End Sub

    Private Sub btnGISeintraegeLoeschen_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim anzahl As Integer
        anzahl = clsGIStools.loeschenGISDatensatz(tbBaulastNr.Text)
        MessageBox.Show("Es wurden Datensätze in GIS-Tabelle gelöscht: " & anzahl)
        refreshGIS(CInt(tbBaulastNr.Text))
        refreshMap()
    End Sub

    Private Sub btnVonProbaugNachGISkopieren_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        IO.Directory.CreateDirectory(tools.baulastenoutDir)
        getAllSerials(anzahl_mitSerial, tools.baulastenoutDir & "Baulasten_ohneAktFlurstueck" & Now.ToString("yyyyMMddhhmm") & ".csv")
        ___showdispatcher("  BL mit Geometrie: " & anzahl_mitSerial & Environment.NewLine)
        ___showdispatcher("BL werden in die DB geschrieben ...  bitte warten " & Environment.NewLine)
        writeallWithSerials(CBool(cbAuchUnguetige.IsChecked), 1) '1=aus katasterdaten übernommen
        ___showdispatcher("  ausschreiben fertig: " & Environment.NewLine)
        refreshGIS(CInt(tbBaulastNr.Text))
        Dim gidstring As String = clsGIStools.bildegidstring()
        range = clsGIStools.calcNewRange(gidstring)
        refreshMap()
    End Sub
    Sub writeallWithSerials(auchUngueltige As Boolean, genese As Integer)
        Dim iz As Integer = 0
        Dim erfolg As Boolean
        Dim sql As String
        Dim coordinatesystemNumber As String = "25832" '31467"'25832lt mapfile

        Dim datei As String = ""
        Dim datei2 As String = ""
        Try
            l("writeallWithSerials---------------------- anfang")
            For Each lok As clsBaulast In rawList
                Console.WriteLine("getAllSerials " & iz)
                If lok.blattnr = "8001" Then
                    Debug.Print("")
                End If
                If lok.blattnr = "90764" Then
                    Debug.Print("")
                End If
                If Not lok.katasterFormellOK Or lok.geloescht Then Continue For
                If lok.serial.IsNothingOrEmpty Then Continue For
                iz += 1
                datei = lok.datei.Replace(srv_unc_path & "\", "").Replace("\", "/")
                datei = datei.Replace("flurkarte.basis_f", "flurkarte.aktuell")
                datei = datei.Replace("h_flurkarte.j", "hist.Flurkarte.")
                datei = datei.Replace("_flurstueck_f", "")
                datei = datei.Replace("_basis_f", "")
                datei2 = datei
                If lok.dateiExistiert Then
                Else
                    datei = "KeineDaten.htm"
                End If
                ___showdispatcher(" db ausschreiben  " & iz & " (" & anzahl_mitSerial & ")" & Environment.NewLine)
                If lok.geloescht Then Continue For

                If auchUngueltige Then
                    write2postgis(lok, erfolg, sql, coordinatesystemNumber, datei, datei2, genese)
                Else
                    If lok.gueltig.ToLower = "j" Then
                        write2postgis(lok, erfolg, sql, coordinatesystemNumber, datei, datei2, genese)
                    End If
                End If


            Next
            l("writeallWithSerials---------------------- ende")
        Catch ex As Exception
            l("Fehler in writeallWithSerials: " & ex.ToString())
        End Try
    End Sub
    Private Sub ___showdispatcher(v As String)

    End Sub

    Private Sub btnZumGIS_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim gidstring As String = clsGIStools.bildegidstring()
        range = clsGIStools.calcNewRange(gidstring)

        Dim param, rangestring As String

        Dim lu, ro As New myPoint
        lu.X = range.xl
        lu.Y = range.yl
        ro.X = range.xh
        ro.Y = range.yh
        rangestring = clsGIStools.calcrangestring(lu, ro)
        param = "modus=""bebauungsplankataster""  range=""" & rangestring & ""
        Process.Start(tools.gisexe, param)
    End Sub

    Private Sub dropped(sender As Object, e As DragEventArgs)
        e.Handled = True
        'droptiff(e)
        dropPDF(e)
    End Sub

    Private Sub dropPDF(e As DragEventArgs)
        Dim filenames As String()
        Dim zuielname As String = ""
        Dim endung As String = ".pdf"
        Dim listeZippedFiles, listeNOnZipFiles, allFeiles As New List(Of String)
        Dim titelVorschlag As String = ""
        Try
            l(" MOD dropped anfang")
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                filenames = CType(e.Data.GetData(DataFormats.FileDrop), String())
            End If

            If filenames(0).ToLower.EndsWith(".pdf") Then
                endung = ".pdf"
            End If
            l(" MOD dropped 2")
            If filenames(0).ToLower.EndsWith(endung) Then
                l(" MOD dropped 3")
                zuielname = IO.Path.Combine(srv_unc_path & "\fkat\baulasten", tools.FSTausPROBAUGListe(0).gemarkungstext.Trim & "\" & tbBaulastNr.Text.Trim & endung).ToLower.Trim
                l(" MOD dropped 4 " & filenames(0).ToLower & " nach " & zuielname)
                IO.File.Copy(filenames(0).ToLower, zuielname, True)
                rawList(0).dateiExistiert = True
                rawList(0).datei = zuielname
                l(" MOD dropped 5")
                'pdfdatei erzeugen
                clsTIFFtools.zerlegeMultipageTIFF(zuielname, tools.baulastenoutDir)
                refreshTIFFbox()
                Dim erfolg As Boolean = clsGIStools.updateGISDB(tbBaulastNr.Text, zuielname, tools.FSTausPROBAUGListe(0).gemarkungstext.Trim, endung)
                If erfolg Then
                    Dim mesres As MessageBoxResult
                    mesres = MessageBox.Show("Die tiff - Datei wurde erfolgreich ins GIS kopiert!" & Environment.NewLine &
                                    "Ausserdem wurde die PDF-Datei erzeugt/erneuert." & Environment.NewLine &
                                    "" & Environment.NewLine &
                                    "Soll die Quelldatei gelöscht werden ? (J/N)" & Environment.NewLine &
                                    " J - Löschen" & Environment.NewLine &
                                    " N - bewahren " & Environment.NewLine,
                                             "Quelldatei löschen?", MessageBoxButton.YesNo,
                                                MessageBoxImage.Question, MessageBoxResult.Yes
                                    )
                    If mesres = MessageBoxResult.Yes Then
                        If Not dateiLoeschen(filenames) Then
                            MessageBox.Show("Datei liess sich nicht löschen. Haben Sie sie noch im Zugriff ? Abbruch!!")
                        End If
                    Else

                    End If
                Else
                    MessageBox.Show("DB-Eintrag liess sich nicht erneuern. Bitte beim admin melden ? Abbruch!!")
                End If


            End If

            l(" MOD dropped ende")
        Catch ex As Exception
            l("Fehler in dropped: " & zuielname & Environment.NewLine &
              zuielname.Trim.ToLower & "   " & ex.ToString())
            MessageBox.Show("Datei läßt sich nicht löschen. ")
        End Try
    End Sub
    Private Sub droptiff(e As DragEventArgs)
        Dim filenames As String()
        Dim zuielname As String = ""
        Dim endung As String = ".tiff"
        Dim listeZippedFiles, listeNOnZipFiles, allFeiles As New List(Of String)
        Dim titelVorschlag As String = ""
        Try
            l(" MOD dropped anfang")
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                filenames = CType(e.Data.GetData(DataFormats.FileDrop), String())
            End If
            If filenames(0).ToLower.EndsWith(".tiff") Then
                endung = ".tiff"
            End If
            If filenames(0).ToLower.EndsWith(".tif") Then
                endung = ".tif"
            End If
            l(" MOD dropped 2")
            If filenames(0).ToLower.EndsWith(".tiff") Or filenames(0).ToLower.EndsWith(".tif") Then
                l(" MOD dropped 3")
                zuielname = IO.Path.Combine(srv_unc_path & "\fkat\baulasten", tools.FSTausPROBAUGListe(0).gemarkungstext.Trim & "\" & tbBaulastNr.Text.Trim & ".tiff").ToLower.Trim
                l(" MOD dropped 4 " & filenames(0).ToLower & " nach " & zuielname)
                IO.File.Copy(filenames(0).ToLower, zuielname, True)
                rawList(0).dateiExistiert = True
                rawList(0).datei = zuielname
                l(" MOD dropped 5")
                'pdfdatei erzeugen
                clsTIFFtools.zerlegeMultipageTIFF(zuielname, tools.baulastenoutDir)
                refreshTIFFbox()
                Dim erfolg As Boolean = clsGIStools.updateGISDB(tbBaulastNr.Text, zuielname, tools.FSTausPROBAUGListe(0).gemarkungstext.Trim, endung)
                If erfolg Then
                    Dim mesres As MessageBoxResult
                    mesres = MessageBox.Show("Die tiff - Datei wurde erfolgreich ins GIS kopiert!" & Environment.NewLine &
                                    "Ausserdem wurde die PDF-Datei erzeugt/erneuert." & Environment.NewLine &
                                    "" & Environment.NewLine &
                                    "Soll die Quelldatei gelöscht werden ? (J/N)" & Environment.NewLine &
                                    " J - Löschen" & Environment.NewLine &
                                    " N - bewahren " & Environment.NewLine,
                                             "Quelldatei löschen?", MessageBoxButton.YesNo,
                                                MessageBoxImage.Question, MessageBoxResult.Yes
                                    )
                    If mesres = MessageBoxResult.Yes Then
                        If Not dateiLoeschen(filenames) Then
                            MessageBox.Show("Datei liess sich nicht löschen. Haben Sie sie noch im Zugriff ? Abbruch!!")
                        End If
                    Else

                    End If
                Else
                    MessageBox.Show("DB-Eintrag liess sich nicht erneuern. Bitte beim admin melden ? Abbruch!!")
                End If


            End If

            l(" MOD dropped ende")
        Catch ex As Exception
            l("Fehler in dropped: " & zuielname & Environment.NewLine &
              zuielname.Trim.ToLower & "   " & ex.ToString())
            MessageBox.Show("Datei läßt sich nicht löschen. ")
        End Try
    End Sub

    Private Shared Function dateiLoeschen(filenames() As String) As Boolean
        Dim fi As IO.FileInfo
        Try
            l(" MOD dateiLoeschen anfang")
            fi = New IO.FileInfo(filenames(0).ToLower)
            If fi.Exists Then
                fi.Delete()
            Else

            End If
            fi = Nothing

            l(" MOD dateiLoeschen ende")
            Return True
        Catch ex As Exception
            l("Fehler in dateiLoeschen: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Sub dropTheBomb(sender As Object, e As DragEventArgs)
        e.Handled = True
        droptiff(e)
    End Sub

    Private Sub btndeleteTIFF_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim mesres As MessageBoxResult
        mesres = MessageBox.Show("Soll das Objekt wirklich gelöscht werden ? ", "Objekt löschen", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes)
        If mesres = MessageBoxResult.No Then
            Exit Sub
        End If
        If clsGIStools.loescheTiffaufGISServer(tbBaulastNr.Text.Trim, tools.FSTausPROBAUGListe(0).gemarkungstext.Trim) Then
            imgTiff.Source = Nothing
            MessageBox.Show("Gelöscht")
        Else
            MessageBox.Show("Fehler beim Löschen.")

        End If
        refreshTIFFbox()
    End Sub

    Private Sub btnPDFaufrufen_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True

        Dim fi As New IO.FileInfo(rawList(0).datei.ToLower.Trim.Replace(".tiff", ".pdf"))
        If fi.Exists Then
            Process.Start(rawList(0).datei.ToLower.Trim.Replace(".tiff", ".pdf"))
        Else
            MessageBox.Show("Datei fehlt!!")
        End If

    End Sub

    Private Sub StackPanel_Drop(sender As Object, e As DragEventArgs)
        e.Handled = True

        Dim filenames As String()
        Dim zuielname As String = ""
        Dim listeZippedFiles, listeNOnZipFiles, allFeiles As New List(Of String)
        Dim titelVorschlag As String = ""
        Try
            l(" MOD StackPanel_Drop anfang")

            l(" MOD StackPanel_Drop anfang")
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                filenames = CType(e.Data.GetData(DataFormats.FileDrop), String())
            End If
            l(" MOD dropped 2")
            If filenames(0).ToLower.EndsWith(".tiff") Then
                Dim fi As New IO.FileInfo(filenames(0).ToLower.Trim)
                Dim a() As String
                a = fi.Name.Split("."c)
                tbBaulastNr.Text = a(0)

                fi = Nothing
            End If
            refreshall()
            l(" MOD StackPanel_Drop ende")
        Catch ex As Exception
            l("Fehler in StackPanel_Drop: " & ex.ToString())
        End Try
    End Sub

    Private Sub btnplus_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim xdifalt As Double = range.xdif / 2
        Dim xdifnew As Double
        range.CalcCenter()
        xdifnew = CInt(xdifalt - (xdifalt / 4))
        range.xl = range.xcenter - xdifnew
        range.xh = range.xcenter + xdifnew
        range.yl = range.ycenter - xdifnew
        range.yh = range.ycenter + xdifnew
        refreshMap()
    End Sub

    Private Sub btnminus_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim xdifalt As Double = range.xdif / 2
        Dim xdifnew As Double
        range.CalcCenter()
        xdifnew = CInt(xdifalt * 1.5)
        range.xl = range.xcenter - xdifnew
        range.xh = range.xcenter + xdifnew
        range.yl = range.ycenter - xdifnew
        range.yh = range.ycenter + xdifnew
        refreshMap()
    End Sub

    Private Sub btndigit_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'genese = 2 '2-selbst digitalisiert, 1 = aus dem kataster
        For Each item As clsBaulast In rawList
            item.serial = tools.wkt
        Next
        writeallWithSerials(CBool(cbAuchUnguetige.IsChecked), 2) '1=aus katasterdaten übernommen
    End Sub
End Class
