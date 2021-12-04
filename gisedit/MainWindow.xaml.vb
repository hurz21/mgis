Class MainWindow
    ' layeraid=161 gid=13 username=feinen_j editid=438015
    Sub New()
        InitializeComponent()
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' layeraid=161 gid=3 username=Feinen_J editid=438002
        Dim arguments As String() = Environment.GetCommandLineArgs()
        '  Dim arguments As String() = {" modus=paradigma", "vorgangsid=9609", "range=490248,491254,5548144,5548704"}

        clsShortcut.MakeShortcut("\\gis\gdvell\apps\test\gisedit\giseditUpdate.bat", "ND-Manager.lnk",
                                 "\\gis\gdvell\apps\gisedit\ge3.ico")
#If DEBUG Then
#End If
        clstools.mapAllArguments(arguments)
        tbinfo.Text = clstools.editSchema & ", " & clstools.editTable & ", layer: " & clstools.editLayerAid & ", " &
            clstools.editUsername & ", editOjektGIDNr: " & clstools.editOjektGIDNr & ", "
        If clstools.editLayerAid = Nothing Then clstools.editLayerAid = "161"
        Select Case clstools.editLayerAid
            Case "161"
                bearbeite161()
        End Select
        e.Handled = True
    End Sub

    Private Sub bearbeite161()
        Dim neuform As New win161
        neuform.ShowDialog()
        End
    End Sub
End Class
