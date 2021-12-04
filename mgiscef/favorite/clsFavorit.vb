

Public Class clsFavorit

    Property vorhanden As String = ""
    Property gecheckted As String = ""
    Property aktiv As String = ""
    Property hgrund As String = ""
    Property titel As String = ""

    Friend Function isSameAs(favoritakt As clsFavorit) As Boolean
        Try
            l("isSameAs---------------------- anfang")
            If favoritakt.vorhanden.Trim.ToLower <> vorhanden.Trim.ToLower Then Return False
            If favoritakt.gecheckted.Trim.ToLower <> gecheckted.Trim.ToLower Then Return False
            If favoritakt.hgrund.Trim.ToLower <> hgrund.Trim.ToLower Then Return False
            If favoritakt.aktiv.Trim.ToLower <> aktiv.Trim.ToLower Then Return False
            Return True
            l("isSameAs---------------------- ende")
        Catch ex As Exception
            l("Fehler in clsFavorit.isSameAs: " & ex.ToString())
            Return False
        End Try
    End Function

    Friend Sub clear()
        vorhanden = ""
        gecheckted = ""
        aktiv = ""
        hgrund = ""
        titel = ""
    End Sub
    Friend Function nachstring(trenn As String) As String
        Dim ttt As New Text.StringBuilder
        Try
            ttt.Append("vorhanden " & vorhanden & trenn)
            ttt.Append("gecheckted " & gecheckted & trenn)
            ttt.Append("aktiv " & aktiv & trenn)
            ttt.Append("hgrund " & hgrund & trenn)
            ttt.Append("titel " & titel & trenn)
            Return ttt.ToString
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class
