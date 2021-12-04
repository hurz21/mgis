Public Class clsWindrose
    Public Shared Function GetWindrosenHyperlink(ByVal x As Double, ByVal y As Double) As String
        Dim xgk, ygk As Double
        Dim newpunit As New myPoint
        newpunit.X = CDbl(x.ToString.Replace(".", ","))
        newpunit.Y = CDbl(y.ToString.Replace(".", ","))

        Dim punktliste() As myPoint
        ReDim punktliste(0)
        punktliste(0) = newpunit
        Dim quellstring As String = modKoordTrans.bildeQuellKoordinatenString(punktliste)
        Dim aufruf As String = modKoordTrans.bildeaufruf(quellstring, punktliste.Count.ToString, "UTM", "GK")
        Dim hinweis As String = ""
        Dim result As String = meineHttpNet.meinHttpJob(myglobalz.ProxyString, aufruf, hinweis, myglobalz.enc, 5000)

        '  Dim result As String = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
        nachricht(hinweis)

        Dim r As String=""
        Dim h As String = ""

        modKoordTrans.getLongLatFromResultSingle(result, r, h, xyTrenner)
        xgk = CDbl(r)
        ygk = CDbl(h)
        Dim xwert As Integer = glattWertberechnen(xgk)
        Dim ywert As Integer = glattWertberechnen(ygk)
        Dim windrosenHyperlink As String = getWindrodenHyperlinktext(xwert, ywert)
        Return windrosenHyperlink
    End Function

    Public Shared Function getWindrodenHyperlinktext(ByVal xwert As Integer, ByVal ywert As Integer) As String
        Dim windrosenHyperlink As String = "http://windrosen.hessen.de/php/windrose.php?ID=" & xwert & "_" & ywert '3484500_5541500"
        Return windrosenHyperlink
    End Function

    Public Shared Function glattWertberechnen(ByVal wert As Double) As Integer
        Dim xwert As Integer = CInt(wert)
        Dim kopfwert As Integer = CInt(xwert.ToString.Substring(0, 4))
        Dim restwert As Integer = xwert - (kopfwert * 1000)
        Dim schwanz As Integer = 0
        If restwert > 500 Then
            schwanz = 500
        Else
            schwanz = 0
        End If
        xwert = (kopfwert * 1000) + schwanz
        Return xwert
    End Function

 
End Class
