Public Class winHaupt
    Sub New()
        InitializeComponent()
    End Sub
    Private Sub winHaupt_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        If isAutho() Then
            'its ok
            tbblnr.Text = "21478"
        Else
            MessageBox.Show("Sie haben keine Berechtigung für diese Anwendung. Abbruch!")
            Close()
        End If
        setLogfile(logfile) : l("Start " & Now) : l("mgisversion:" & bgmVersion)
        initdb()
        Title = "BGM " & " V.: " & bgmVersion
    End Sub

    Private Shared Function isAutho() As Boolean
        Return Environment.UserName.ToLower = "storcksdieck_a" Or
                Environment.UserName.ToLower = "hartmann_s" Or
                Environment.UserName.ToLower = "briese_j" Or
                Environment.UserName.ToLower = "feinen_j" Or
                Environment.UserName.ToLower = "zahnlückenpimpf" Or
                Environment.UserName.ToLower = "kroemmelbein_m"
    End Function

    Private Sub btnNeu_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim neu As New winDetail("0") ' 0=modus neu
        neu.ShowDialog()
    End Sub

    Private Sub btnBestand_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim best As New winBestand()
        best.Show()
    End Sub

    Private Sub btnGIS_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim gisexe = "C:\kreisoffenbach\mgis\mgis.exe"
        'Dim lu, ro As New myPoint
        'lu.X = range.xl
        'lu.Y = range.yl
        'ro.X = range.xh
        'ro.Y = range.yh
        'rangestring = calcrangestring(lu, ro)
        'param = "modus=""bebauungsplankataster""  range=""" & rangestring & ""
        Process.Start(gisexe)
    End Sub



    Private Sub btnEdit_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'If tbblnr.Text.IsNothingOrEmpty Then
        '    MsgBox("bitte geben sie eine blnr ein!")
        '    Exit Sub
        'End If
        Dim neu As New winDetail((tbblnr.Text)) ' 0=modus neu
        neu.ShowDialog()
    End Sub

    Private Sub Window_Drop(sender As Object, e As DragEventArgs)
        e.Handled = True

        Dim filenames As String()
        Dim zuielname As String = ""
        Dim listeZippedFiles, listeNOnZipFiles, allFeiles As New List(Of String)
        Dim titelVorschlag As String = ""
        Try
            l(" MOD ---------------------- anfang")

            l(" MOD dropped anfang")
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                filenames = CType(e.Data.GetData(DataFormats.FileDrop), String())
            End If
            l(" MOD dropped 2")
            If filenames(0).ToLower.EndsWith(".tiff") Then
                Dim fi As New IO.FileInfo(filenames(0).ToLower.Trim)
                Dim a() As String
                a = fi.Name.Split("."c)
                tbblnr.Text = a(0)
                fi = Nothing
                'Dim neu As New winDetail((tbblnr.Text)) ' 0=modus neu
                'neu.ShowDialog()
            End If

            l(" MOD ---------------------- ende")
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
        End Try
    End Sub

    Private Sub btnPDFTool_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim ewrk As New winWerkzeuge
        ewrk.ShowDialog

    End Sub


End Class
