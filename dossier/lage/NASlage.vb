Public Class NASlage
    Public Property gmlid As String = ""
    Public Property GemeindeName As String = ""
    Public Property GemeindeNr As String = ""
    Public Property strassenname As String = ""
    Public Property hausnummer As String = ""
    Public Property lageschluessel As String = ""
    Public Property fs As String = ""
    Public Property weistauf As String = ""
    Public Property zeigtauf As String = ""
    Public Property Lage As String = ""
    Public Property kreis As String = ""
    Public Property regbez As String = ""
    Public Property land As String = ""
    Property strAusgabe As String = ""
    Function calcLageschluessel() As String
        lageschluessel = land.Trim & regbez.Trim & kreis.Trim & GemeindeNr.Trim & Lage.Trim
        Return lageschluessel
    End Function
End Class
