Public Class clsMedia
    'https://buergergis.kreis-offenbach.de/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getbegleitbplan&pdf=di_41&gemarkung=dietzenbach

    Shared Function getbplanbegleit(gemarkung As String, pdf As String) As String
        Dim filter As String = "d:\fkat\"
#If DEBUG Then
        filter = "l:\fkat\"
#End If
        gemarkung = clsString.ue2umlaut(gemarkung)
        Dim verzeichnis As String = filter & "\bplan" & gemarkung & "\" & pdf & "\"
        Dim di As New IO.DirectoryInfo(verzeichnis)
        Dim templiste As IO.FileInfo()
        Dim ausschluss As String
        Dim begleitfilelist = New List(Of IO.FileInfo)
        Try
            l(" MOD getbplanbegleit anfang")
            Try
                l("getBegleitplanFileliste---------------------- anfang")
                templiste = di.GetFiles("*.pdf")
                Dim dra As IO.FileInfo
                ausschluss = pdf & ".pdf"
                'list the names of all files in the specified directory
                For Each dra In templiste
                    Debug.Print(dra.ToString)
                    If ausschluss <> dra.Name.ToLower Then
                        begleitfilelist.Add(dra)
                    End If
                Next
                l("zwischen " & begleitfilelist.Count)
                Dim summe As String = ""
                For Each datei As IO.FileInfo In begleitfilelist
                    summe = summe & datei.Name & "#"
                Next
                l(summe)
                l("getBegleitplanFileliste---------------------- ende")
                Return summe
            Catch ex As Exception
                l("Fehler in getBegleitplanFileliste: " & ex.ToString())
                Return Nothing
            End Try
            l(" MOD getbplanbegleit ende")
            Return ""
        Catch ex As Exception
            l("Fehler in getbplanbegleit: " & ex.ToString())
            Return "Fehler in getbplanbegleit: "
        End Try
    End Function
End Class
