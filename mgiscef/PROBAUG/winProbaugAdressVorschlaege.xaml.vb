Public Class winProbaugAdressVorschlaege
    Public _adressVorschlage As New List(Of clsAdress)
    Public _gesuchteAdresse As New clsAdress
    Public rechts, hoch As Double
    Public Property ausgewaehlt As Boolean = False

    Sub New(adressVorschlage As List(Of clsAdress), gesuchteAdresse As clsAdress)
        InitializeComponent()
        _adressVorschlage = adressVorschlage
        _gesuchteAdresse = gesuchteAdresse
    End Sub

    Private Sub winProbaugAdressVorschlaege_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        dgadrVorschlaege.DataContext = _adressVorschlage
        Title = "ProbauG-GIS: Keine Hausnummer: " & clsString.Capitalize(_gesuchteAdresse.gemeindeName) & " " & _gesuchteAdresse.strasseName & " " & _gesuchteAdresse.HausKombi
        e.Handled = True
    End Sub

    Private Sub btnAbbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        ausgewaehlt = False
        Close()
    End Sub

    Private Sub dgadrVorschlaege_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If dgadrVorschlaege.SelectedItem Is Nothing Then Exit Sub
        Dim item As clsAdress
        Try
            item = CType(dgadrVorschlaege.SelectedItem, clsAdress)
        Catch ex As Exception
            e.Handled = True
            Exit Sub
        End Try
        item = CType(dgadrVorschlaege.SelectedItem, clsAdress)
        If item Is Nothing Then
            item = CType(dgadrVorschlaege.SelectedItem, clsAdress)
            If item Is Nothing Then Return
        End If
        rechts = item.GKrechts
        hoch = item.GKhoch
        ausgewaehlt = True
        Close()
    End Sub
End Class
