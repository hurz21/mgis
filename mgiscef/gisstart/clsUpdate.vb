'Public Class clsUpdate
'    Friend Shared Sub Check4Update()
'        Try
'            l(" MOD Check4Update anfang")
'            If Not iminternet Then Exit Sub
'            Dim quellPfad As String = myglobalz.serverWeb & "/fkat/paradigma/mgis/bin/"
'            Dim quellDatei = "mgis_version.txt"
'            Dim zielPfad As String = IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "mgis")
'            Dim zielDatei = "mgis_version.txt"
'            Dim lokaleVersion As String = ""
'            lokaleVersion = getLokaleversion(quellDatei)

'            meineHttpNet.createDir(zielPfad)


'            Dim r As Boolean = meineHttpNet.down(quellPfad & quellDatei, zielDatei, zielPfad)
'            If r Then
'                Dim QuellVersion = IO.File.ReadAllText(IO.Path.Combine(zielPfad, zielDatei))
'                If CInt(lokaleVersion) > CInt(QuellVersion) Then
'                    Dim sollupdateStarten As Boolean = getSollUpdateStarten(QuellVersion, lokaleVersion)
'                    If sollupdateStarten Then
'                        starteUpdate()
'                    End If
'                End If
'            End If
'            l(" MOD Check4Update ende")
'        Catch ex As Exception
'            l("Fehler in Check4Update: " & ex.ToString())
'        End Try
'    End Sub

'    Private Shared Sub starteUpdate()
'        Try
'            l(" MOD starteUpdate anfang")
'            Dim currentProcess As Process = Process.GetCurrentProcess()
'            MsgBox("mgis currentProcess " & currentProcess.Id)
'            Dim startinfo As New ProcessStartInfo
'            startinfo.FileName = Environment.CurrentDirectory & "\mgisupdate.exe"
'            startinfo.WorkingDirectory = Environment.CurrentDirectory
'            startinfo.Arguments = " /proc=" & currentProcess.Id
'            Process.Start(startinfo)
'            l(" MOD starteUpdate ende")
'        Catch ex As Exception
'            l("Fehler in starteUpdate: " & ex.ToString())
'        End Try
'    End Sub

'    Private Shared Function getSollUpdateStarten(quellVersion As String, lokaleVersion As String) As Boolean
'        Try
'            l(" MOD getSollUpdateStarten anfang")
'            Dim result = InputBox("Es ist eine neuere Version des GIS verfügbar: " & Environment.NewLine &
'                 "Alte Version: " & Environment.NewLine &
'                 "Neue Version: " & Environment.NewLine &
'                 "Möchten sie nun das Update ausführen ? (J/N): " & Environment.NewLine, "Update ausführen ?", "n")
'            Return If(result.Trim.ToLower <> "j", True, False)
'            l(" MOD getSollUpdateStarten ende")
'            Return False
'        Catch ex As Exception
'            l("Fehler in getSollUpdateStarten: " & ex.ToString())
'            Return False
'        End Try
'    End Function

'    Private Shared Function getLokaleversion(dateifullname As String) As String
'        Dim result As String = ""
'        Try
'            l(" MOD getLokaleversion anfang")
'            result = IO.File.ReadAllText(dateifullname)
'            l(" MOD getLokaleversion ende")
'            Return result
'        Catch ex As Exception
'            l("Fehler in getLokaleversion: " & ex.ToString())
'            Return ""
'        End Try
'    End Function
'End Class
