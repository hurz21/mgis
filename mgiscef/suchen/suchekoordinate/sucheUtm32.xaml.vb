Public Class sucheUtm32
    Public Property returnCode As Boolean = False

    Private Sub startKoord_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim a(), b() As String
        aktGlobPoint.strX = CType(tbrechts.Text, String).Replace(".", ",")
        aktGlobPoint.strY = CType(tbhoch.Text, String).Replace(".", ",")
        a = aktGlobPoint.strX.Split(","c)
        b = aktGlobPoint.strY.Split(","c)

        kartengen.aktMap.aktrange = calcBbox(a(0), b(0), 100)
        '   starteWebbrowserControl(bbox)
        returnCode = True
        Close()

    End Sub

    Private Sub sucheUtm32_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        tbrechts.Text = CType(CInt(aktGlobPoint.strX), String)
        tbhoch.Text = CType(CInt(aktGlobPoint.strY), String)
    End Sub

    Private Sub btnAbbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Close()
    End Sub

    Private Sub btnKoordUmrechner2_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim umreechnerUrl = "https://www.deine-berge.de/Rechner/Koordinaten"
        Process.Start(umreechnerUrl)

    End Sub
End Class
