Public Class winSachgeiete
    Private Sub btnRefresh_Click(sender As Object, e As RoutedEventArgs)
        refresh()
        e.Handled = True
    End Sub

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub winSachgeiete_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        refresh
    End Sub

    Private Sub dgSG_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Sub refresh()
        Dim aktschema As New clsSchema
        Dim sql As String
        aktschema.schemaname = ""
        Try
            schemaColl.Add(aktschema)

            sql = "select sachgebiet  from public.sachgebiete order by sachgebiet"
            wgisdt = getDT(sql, tools.dbServername, "webgiscontrol")
            dgDatentabelle.DataContext = wgisdt
        Catch ex As Exception
            l("fehler in DatentabelleAnezigen " & ex.ToString)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub btnremove_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub btnNeuerTitel_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub
End Class
