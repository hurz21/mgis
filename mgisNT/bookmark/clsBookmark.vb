Class clsBookmark
    Property id As Integer = 0
    Property titel As String = ""
    Property datum As Date = CDate("1970-01-01")
    Property fav As New clsFavorit
    Property range As New clsRange
    Property user As New clsUser
    Property freigabe_Intranet As Boolean = False
    Property free4mygruppe As Boolean = False
    Sub clear()
        titel = ""
        datum = CDate("1970-01-01")
        fav.clear()
        range.clear()
        id = 0
    End Sub
End Class

