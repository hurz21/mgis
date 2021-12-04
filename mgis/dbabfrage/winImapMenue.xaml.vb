Public Class winImapMenue
    Private strPunkt As String = ""
    Private strFS As String = ""
    Private titelAID As String = ""
    Public auswahl As String = ""

    Sub New(_strPunkt As String, _strFS As String, _titelAID As String)
        InitializeComponent()
        strPunkt = _strPunkt
        strFS = _strFS
        titelAID = _titelAID
    End Sub
    Private Sub winImapMenue_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        Title = "Datenbankabfrage für Punkt/Flurstück"
        tbUTM.Text = strPunkt
        tbFS.Text = strFS
        gb1.Header = "Aktiv: " & titelAID
    End Sub

    Private Sub btnPunkt_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        auswahl = "punkt"
        Close()
    End Sub

    Private Sub btnFS_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        auswahl = "fs"
        Close()
    End Sub

    Private Sub btnDossier_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        auswahl = "dossier"
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        auswahl = ""
        Close()
    End Sub
End Class
