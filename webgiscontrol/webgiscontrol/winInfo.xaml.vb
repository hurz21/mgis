Public Class winInfo
    Public _text As String
    Sub New(text As String)

        ' This call is required by the designer.
        InitializeComponent()
        _text = text
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub winInfo_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        tbinfo.Text = _text
    End Sub
End Class
