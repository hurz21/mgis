Public Class clsStartup
    Shared Sub createDir(targetroot As String)
        Try
            l(" createDir ---------------------- anfang" & targetroot)
            'MsgBox("Vor targetroot createdir " & targetroot)
            IO.Directory.CreateDirectory(targetroot)
            l(" createDir ---------------------- ende")

        Catch ex As Exception
            l("Fehler in createDir: " & ex.ToString())
            MsgBox(ex.Message & " fehler in createdir  " & targetroot)
        End Try
    End Sub
End Class
