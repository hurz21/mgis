Public Class winProbaugFSTVorschlaege
    Public _fstVorschlage As New List(Of clsFlurstueck)
    Public _gesuchtesFST As New clsFlurstueck
    Public _gefundenesFST As New clsFlurstueck
    Public rechts, hoch As Double
    Public Property ausgewaehlt As Boolean = False


    Sub New(fstVorschlage As List(Of clsFlurstueck), gesuchtesFST As clsFlurstueck)
        InitializeComponent()
        _fstVorschlage = fstVorschlage
        _gesuchtesFST = gesuchtesFST
    End Sub

    Private Sub winFSTvorschlaege_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        dgfstVorschlaege.DataContext = _fstVorschlage
        Title = "ProbauG-GIS: Flurstück nicht gefunden! " & _gesuchtesFST.gemeindename & "-" & _gesuchtesFST.gemarkungstext & "," & " Flur: " & (_gesuchtesFST.flur) & " " &
            _gesuchtesFST.zaehler & "/" & _gesuchtesFST.nenner
        tbfst.Text = Title
        e.Handled = True
    End Sub

    Private Sub dgfstVorschlaege_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If dgfstVorschlaege.SelectedItem Is Nothing Then Exit Sub
        Dim item As clsFlurstueck
        Try
            item = CType(dgfstVorschlaege.SelectedItem, clsFlurstueck)
        Catch ex As Exception
            e.Handled = True
            Exit Sub
        End Try
        item = CType(dgfstVorschlaege.SelectedItem, clsFlurstueck)
        If item Is Nothing Then
            item = CType(dgfstVorschlaege.SelectedItem, clsFlurstueck)
            If item Is Nothing Then Return
        End If
        _gefundenesFST = item
        _gefundenesFST.gemeindename = _gesuchtesFST.gemeindename
        _gefundenesFST.gemarkungstext = _gesuchtesFST.gemarkungstext
        'rechts = item.GKrechts
        'hoch = item.GKhoch
        ausgewaehlt = True
        Close()
    End Sub

    Private Sub btnAbbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        ausgewaehlt = False
        Close()
    End Sub

End Class
