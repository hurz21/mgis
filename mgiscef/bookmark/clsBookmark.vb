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
    Sub New()
        fav = New clsFavorit
        range = New clsRange
        user = New clsUser
    End Sub

    Function kopiereBookmark() As clsBookmark
        Dim newbook As New clsBookmark
        Try
            l("kopiereBookmark---------------------- anfang")
            newbook.clear()
            newbook.id = id
            newbook.user.username = user.username
            newbook.titel = titel
            newbook.fav.vorhanden = fav.vorhanden
            newbook.fav.gecheckted = fav.gecheckted
            newbook.fav.hgrund = fav.hgrund
            newbook.fav.aktiv = fav.aktiv
            newbook.range.xl = range.xl
            newbook.range.xh = range.xh
            newbook.range.yl = range.yl
            newbook.range.yh = range.yh
            newbook.datum = datum
            newbook.user.ADgruppenname = user.ADgruppenname
            newbook.free4mygruppe = free4mygruppe
            Return newbook
            'auswahlBookmark.id = CInt(clsDBtools.fieldvalue(item2.Row.ItemArray(0)))
            'auswahlBookmark.user.username = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(1)))
            'auswahlBookmark.titel = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(2)))
            'auswahlBookmark.fav.vorhanden = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(3)))
            'auswahlBookmark.fav.gecheckted = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(4)))
            'auswahlBookmark.fav.hgrund = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(5)))
            'auswahlBookmark.fav.aktiv = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(6)))
            'auswahlBookmark.range.xl = CDbl(clsDBtools.fieldvalue(item2.Row.ItemArray(7)))
            'auswahlBookmark.range.xh = CDbl(clsDBtools.fieldvalue(item2.Row.ItemArray(8)))
            'auswahlBookmark.range.yl = CDbl(clsDBtools.fieldvalue(item2.Row.ItemArray(9)))
            'auswahlBookmark.range.yh = CDbl(clsDBtools.fieldvalue(item2.Row.ItemArray(10)))
            'auswahlBookmark.datum = CDate(clsDBtools.fieldvalue(item2.Row.ItemArray(11)))
            'auswahlBookmark.user.ADgruppenname = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(12)))
            'auswahlBookmark.free4mygruppe = CBool(clsDBtools.toBool(item2.Row.ItemArray(13)))

            l("kopiereBookmark---------------------- ende")
        Catch ex As Exception
            l("Fehler in kopiereBookmark: ", ex)
            Return newbook
        End Try
    End Function
End Class

