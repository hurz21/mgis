Public Class mapfileTest
    Dim _mapfilebild As String
    Property _modus As String
    Property aktaid As Integer
    Sub New(mapfilebild As String, modus As String, _aktaid As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        _mapfilebild = mapfilebild
        ' Add any initialization after the InitializeComponent() call.
        _modus = modus
        aktaid = _aktaid
    End Sub

    Private Sub mapfileTest_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        tbError.Visibility = Visibility.Collapsed
        myimage.Visibility = Visibility.Visible
        If _modus = "einzeln" Then
            aktrange.xl = 470685
            aktrange.xh = 503544
            aktrange.yl = 5530566
            aktrange.yh = 5553593
            _mapfilebild = _mapfilebild.Replace("\\gis\gdvell\", "d:\")
            bild_erzeugenMapMode(aktrange, _mapfilebild, myimage.Width, myimage.Height)
        End If
    End Sub

    Private Sub btnBilderzeugen_Click(sender As Object, e As RoutedEventArgs)
        aktrange.xl = 470685
        aktrange.xh = 503544
        aktrange.yl = 5530566
        aktrange.yh = 5553593
        bild_erzeugenMapMode(aktrange, _mapfilebild, myimage.Width, myimage.Height)
        'bild_erzeugenBrowseMode(aktrange, _mapfilebild)
        e.Handled = True
    End Sub

    Private Sub bild_erzeugenMapMode(aktrange As clsRange, mapfile As String, bildbreite As Double, bildhoehe As Double)
        Dim w, h As Double
        w = bildbreite : h = bildhoehe ' : myimage.Width : h = myimage.Height
        canvasImage = New Image
        canvasImage.Name = "canvasImage"
        'mapCanvas.Children.Add(canvasImage)
        'mapCanvas.SetZIndex(canvasImage, 100)
        Dim a As String = aufrufbilden(aktrange, mapfile, w, h)
        tbaufruf.Text = a$
        tbError.Text = ""
        MapModeAbschicken(tbaufruf.Text, myimage)
        Dim hinweis, fehler As String
        fehler = meineHttpNet.meinHttpJob("", tbaufruf.Text, hinweis)
        If fehler.Substring(0, 10).ToLower.Contains("png") Then
            tbError.Visibility = Visibility.Collapsed
            myimage.Visibility = Visibility.Visible
        Else
            '   tbError.Text = fehlerModusExe(fehler, "einzeln")
            tbError.Text = fehler
            tbError.Visibility = Visibility.Visible
            myimage.Visibility = Visibility.Collapsed
        End If


    End Sub





    Private Sub btnImagemap_Click(sender As Object, e As RoutedEventArgs)
        '        <HTML>
        '<HEAD><TITLE>MapServer Message</TITLE></HEAD>
        '<!-- MapServer version 6.4.1 OUTPUT=GIF OUTPUT=PNG OUTPUT=JPEG SUPPORTS=PROJ SUPPORTS=GD SUPPORTS=AGG SUPPORTS=FREETYPE SUPPORTS=CAIRO SUPPORTS=OPENGL SUPPORTS=ICONV SUPPORTS=WMS_SERVER SUPPORTS=WMS_CLIENT SUPPORTS=WFS_SERVER SUPPORTS=WFS_CLIENT SUPPORTS=WCS_SERVER SUPPORTS=SOS_SERVER SUPPORTS=THREADS INPUT=JPEG INPUT=POSTGIS INPUT=OGR INPUT=GDAL INPUT=SHAPEFILE -->
        '<BODY BGCOLOR = "#FFFFFF" >
        'loadLayer() : Unknown identifier.Parsing error near (aisdjwaid):(line 2)
        '</BODY></HTML>
    End Sub

    Private Sub MapModeAbschicken(aufruf As String, canvasImage As Image)
        Dim myBitmapImage As New BitmapImage()
        Try
            myBitmapImage.BeginInit()
            myBitmapImage.UriSource = New Uri(aufruf, UriKind.Absolute)
            ' result = meineHttpNet.meinHttpJob("", aufruf, hinweis)
            myBitmapImage.EndInit()
            canvasImage.Source = myBitmapImage
        Catch ex As Exception
            l("fehler in MapModeAbschicken: " & aufruf & " /// " & ex.ToString)
        End Try
    End Sub

    Private Sub btnBilderzeugen3_Click(sender As Object, e As RoutedEventArgs)
        'grosser m
        getRange("gross")
        '483225, 5539023 [m] 483562, 5539255
        bild_erzeugenMapMode(aktrange, _mapfilebild, myimage.Width, myimage.Height)
        e.Handled = True
    End Sub

    Private Sub btnBilderzeugen2_Click(sender As Object, e As RoutedEventArgs)
        getRange("mittel")
        bild_erzeugenMapMode(aktrange, _mapfilebild, myimage.Width, myimage.Height)
        e.Handled = True
    End Sub

    Private Sub cbmserror_Click(sender As Object, e As RoutedEventArgs)
        Dim datei = tools.serverUNC & "\ms_error.txt"
        Dim readText As String = IO.File.ReadAllText(datei)
        opendocument(datei)
        e.Handled = True
        'Dim readText As String = IO.File.ReadAllText(datei)
        ''  Process.Start(datei)
        'myimage.Visibility = Visibility.Collapsed
        'tbError.Visibility = Visibility.Visible
        'tbError.Text = readText
        e.Handled = True
    End Sub

    Private Sub cbmserrorExt_Click(sender As Object, e As RoutedEventArgs)
        Dim datei = tools.serverUNC & "\websys\ms_error.txt"
        Dim readText As String = IO.File.ReadAllText(datei)
        opendocument(datei)
        e.Handled = True
    End Sub

    Private Sub btnLayerMapfile_Click(sender As Object, e As RoutedEventArgs)
        If aktAid = 0 Then
            MsgBox("Zuerst eine Ebene auswählen")
            Exit Sub
        End If
        Dim zielroot = tools.serverUNC & "\nkat\aid\" & aktaid & "\layer.map"
        opendocument(zielroot)
        e.Handled = True
    End Sub
    Friend Shared Function rangeLaden() As clsRange
        Dim datei As String
        Dim aktrange As New clsRange
        Try
            datei = mgisUserRoot & "\lastrange\" & Environment.UserName & "_lastRange.txt"
            Using ddatei As New IO.StreamReader(datei)
                aktrange.xl = CDbl(ddatei.ReadLine)
                aktrange.xh = CDbl(ddatei.ReadLine)
                aktrange.yl = CDbl(ddatei.ReadLine)
                aktrange.yh = CDbl(ddatei.ReadLine)
            End Using
            Return aktrange
        Catch ex As Exception
            l("fehler in rangeLaden " & ex.ToString)
            Return Nothing
        End Try
    End Function
    Private Sub btnBilderzeugenLastRange_Click(sender As Object, e As RoutedEventArgs)
        'grosser m
        'getRange("gross")
        '483225, 5539023 [m] 483562, 5539255
        Dim lastrangecookie = mgisUserRoot & "\lastrange\" & Environment.UserName & "_lastRange.txt"
        Dim fi As New IO.FileInfo(lastrangecookie)
        If fi.Exists Then
            aktrange = rangeLaden()
        Else
            aktrange.xl = 481568
            aktrange.xh = 485958
            aktrange.yl = 5538793
            aktrange.yh = 5541454
        End If
        bild_erzeugenMapMode(aktrange, _mapfilebild, myimage.Width, myimage.Height)
        e.Handled = True
    End Sub
End Class
