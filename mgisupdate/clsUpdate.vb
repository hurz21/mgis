Public Class clsUpdate
    Friend Shared Function getServerVersion(serverVersion As String) As String
        Try
            clsTools.l(" MOD Check4Update anfang")
            If Not clsTools.ImInternet Then Return ""
            Dim quellPfad As String = clsTools.serverWeb & "/fkat/paradigma/mgis/bin/"
            clsTools.createDir(serverVersion)
            Dim r As Boolean = clsTools.down(quellPfad, "mgis_version.txt", serverVersion)
            If r Then
                Dim QuellVersion = IO.File.ReadAllText(IO.Path.Combine(serverVersion, "mgis_version.txt"))

                Return QuellVersion.trim
            End If
            clsTools.l(" MOD Check4Update ende")
            Return ""
        Catch ex As Exception
            clsTools.l("Fehler in Check4Update: " & ex.ToString())
            Return ""
        End Try
    End Function


    Private Shared Function getSollUpdateStarten(quellVersion As String, lokaleVersion As String) As Boolean
        Try
            clsTools.l(" MOD getSollUpdateStarten anfang")
            Dim result = InputBox("Es ist eine neuere Version des GIS verfügbar: " & Environment.NewLine &
                 "Alte Version: " & Environment.NewLine &
                 "Neue Version: " & Environment.NewLine &
                 "Möchten sie nun das Update ausführen ? (J/N): " & Environment.NewLine, "Update ausführen ?", "n")
            Return If(result.Trim.ToLower <> "j", True, False)
            clsTools.l(" MOD getSollUpdateStarten ende")
            Return False
        Catch ex As Exception
            clsTools.l("Fehler in getSollUpdateStarten: " & ex.ToString())
            Return False
        End Try
    End Function

    Friend Shared Function getLokaleversion(dateifullname As String) As String
        Dim result As String = ""
        Try
            clsTools.l(" MOD getLokaleversion anfang")
            result = IO.File.ReadAllText(dateifullname)
            clsTools.l(" MOD getLokaleversion ende")
            Return result
        Catch ex As Exception
            clsTools.l("Fehler in getLokaleversion: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
