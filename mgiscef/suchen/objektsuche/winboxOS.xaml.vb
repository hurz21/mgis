Public Class winboxOS
    Public Property aktion As String
    Public Property pufferinm As String
    Property _pdfVorhanden As Boolean
    Property _pdfDatei As String

    Sub New(pdfVorhanden As Boolean, pdfDatei As String)

        ' This call is required by the designer.
        InitializeComponent()
        _pdfVorhanden = pdfVorhanden
        ' Add any initialization after the InitializeComponent() call.
        _pdfDatei = pdfDatei
    End Sub

    Private Sub btnZurKarte(sender As Object, e As RoutedEventArgs)
        aktion = "zurkarte"
        Close()
        e.Handled = True
    End Sub

    Private Sub btndbanzeigen(sender As Object, e As RoutedEventArgs)
        aktion = "dbabfrage"
        Close()
        e.Handled = True
    End Sub

    Private Sub btnzuParadigma(sender As Object, e As RoutedEventArgs)
        aktion = "zuparadigmahinzufuegen"
        Close()
        e.Handled = True
    End Sub

    Private Sub btnzupuffern(sender As Object, e As RoutedEventArgs)
        If tbpufferinm.Text.IsNothingOrEmpty Then
            MsgBox("Sie müssen zuerst den Abstand der Pufferlinie in Meter angeben. ")
            Exit Sub
        End If
        pufferinm = tbpufferinm.Text
        aktion = "puffern"
        Close()
        e.Handled = True
    End Sub

    Private Sub winboxOS_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If _pdfVorhanden Then
            spPDF.IsEnabled = True
            _pdfDatei = serverUNC & _pdfDatei
            _pdfDatei = _pdfDatei.Replace("/", "\")
        Else
            spPDF.IsEnabled = False
        End If
        If iminternet Then
            btnparaueb.IsEnabled = False
            btnParazum.IsEnabled = False
        End If
        e.Handled = True
    End Sub

    Private Sub btnPdF(sender As Object, e As RoutedEventArgs)
        aktion = "pdfdateizumobjektladen"
        Close()
        e.Handled = True
    End Sub

    Private Sub btnzumParadigmavorgang_Click(sender As Object, e As RoutedEventArgs)
        aktion = "zuparadigmavorgangsid"
        Close()
        e.Handled = True
    End Sub
End Class
