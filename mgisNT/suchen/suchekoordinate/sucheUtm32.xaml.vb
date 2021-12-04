Public Class sucheUtm32
    Public Property returnCode As Boolean = False

    Private Sub startKoord_Click(sender As Object, e As RoutedEventArgs)
        aktGlobPoint.strX = CType(tbrechts.Text, String)
        aktGlobPoint.strY = CType(tbhoch.Text, String)
        kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, 100)
        '   starteWebbrowserControl(bbox)
        returnCode = True
        Close()
        e.Handled = True

    End Sub

    Private Sub sucheUtm32_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        tbrechts.Text = CType(CInt(aktGlobPoint.strX), String)
        tbhoch.Text = CType(CInt(aktGlobPoint.strY), String)
    End Sub

    Private Sub btnAbbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Close()
    End Sub
End Class
