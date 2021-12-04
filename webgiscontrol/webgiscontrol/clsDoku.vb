Public Class clsDoku
    Property aid As Integer = 0
    Property inhalt As String = ""
    Property entstehung As String = ""
    Property aktualitaet As String = ""
    Property masstab As String = ""
    Property beschraenkungen As String = ""
    Property datenabgabe As String = ""
    Property calcedOwner As String = ""
    Property internes As String = ""

    Friend Sub clear()
        aid = 0
        inhalt = ""
        entstehung = ""
        aktualitaet = ""
        masstab = ""
        beschraenkungen = ""
        datenabgabe = ""
        calcedOwner = ""
        internes = ""
    End Sub
End Class
Public Class clsLegendenItem
    Property aid As Integer
    Property nr As Integer
    Property titel As String

End Class
