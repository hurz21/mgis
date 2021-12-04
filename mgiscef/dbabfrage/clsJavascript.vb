Class clsJavascript
    Private Sub New()

    End Sub
    Public Shared Function isoliereCGIparameter(ByRef original As String) As String()
        Dim params As String()

        original = original.Replace("javascript:", "")
        original = original.Replace("DatenabfragePunkt(", "")
        original = original.Replace("Datenabfrage(", "")
        original = original.Replace("DatenabfrageFlaeche(", "")
        original = original.Replace("DatenabfrageFlaecheAttributtabelle(", "")
        original = original.Replace(")", "")
        '  If original.StartsWith("show_db") Then
        original = original.Replace("top.show_MYDB_in_Window", "")
        original = original.Replace("show_MYDB_in_Window", "")
        original = original.Replace("top.show_MYDB", "").Replace("top.show_db", "").Replace("show_MYDB", "").Replace("show_db", "")

        original = original.Replace("'", "").Replace("(", "").Replace(")", "")
        params = original.Split(","c)
        Return params
    End Function
End Class
