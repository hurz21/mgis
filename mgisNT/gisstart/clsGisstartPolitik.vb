Class clsGisstartPolitik
    Shared Function getCurrentProcId() As Integer
        Dim currentProcess As Process
        Try
            l("getCurrentProcId---------------------- anfang")
            currentProcess = Process.GetCurrentProcess()
            l("getCurrentProcId---- " & currentProcess.Id)
            Return currentProcess.Id
            l("getCurrentProcId---------------------- ende")
        Catch ex As Exception
            l("Fehler in getCurrentProcId: " & ex.ToString())
            Return 0
        End Try
    End Function
    Shared Sub registerAutostart(appname As String, quellpfad As String)
        Dim quelldatei As String
        Dim zieldatei As String = ""
        Try
            l("registerAutostart---------------------- anfang" & iminternet)
            If iminternet Then Exit Sub
            quelldatei = quellpfad & appname
            zieldatei = Environment.GetFolderPath(Environment.SpecialFolder.Startup) & "\" & appname
            l("quelldatei:" & quelldatei)
            l("zieldatei:" & zieldatei)
            If IO.File.Exists(zieldatei) Then
                l(appname & "Existiert schon in autostart " & Environment.GetFolderPath(Environment.SpecialFolder.Startup))
                My.Computer.FileSystem.DeleteFile(zieldatei)
                My.Computer.FileSystem.CopyFile(quelldatei, zieldatei)
            Else
                My.Computer.FileSystem.CopyFile(quelldatei, zieldatei)
            End If
            l("registerAutostart---------------------- ende")
        Catch ex As Exception
            l("Fehler in registerAutostart: " & ex.ToString())
        End Try

    End Sub
    'Sub gisStarten(prozessname As String)
    '    If gisLaeuftschon(prozessname) Then
    '        If radNachfrage.IsChecked Then
    '            Dim messageboxresult As New MessageBoxResult
    '            messageboxresult = MessageBox.Show("Es läuft bereits eine INSTANZ des GIS. Abschiessen?", "Abschiessen?", MessageBoxButton.YesNo)
    '            If messageboxresult = MessageBoxResult.Yes Then
    '                abschiessen(prozessname)
    '            Else
    '                starten() : Exit Sub
    '            End If
    '        End If
    '        If radImmerNeustart.IsChecked Then
    '            abschiessen(prozessname)
    '            starten() : Exit Sub
    '        End If
    '        If radMultiple.IsChecked Then
    '            starten() : Exit Sub
    '        End If
    '    Else
    '        starten()
    '    End If
    'End Sub

    Shared Function gisLaeuftschon(prozessname As String) As Boolean
        Dim anz = anzahlGisProzesse(prozessname)
        Dim anzahl As Boolean
        '    MsgBox(anz.ToString)
        anzahl = anz > 1
#If DEBUG Then
        anzahl = anz > 0
#End If

        If anzahl Then
            ' Debug.Print("")
            ' MsgBox("löppt")
            Return True
        Else
            '   MsgBox("löppt net")
            Return False
        End If

    End Function

    Private Shared Function anzahlGisProzesse(prozessname As String) As Integer
        Dim myProcesses() As Process
        ' Returns array containing all instances of "Notepad". 
        myProcesses = Process.GetProcessesByName(prozessname)
        Return myProcesses.Count
    End Function

    Shared Sub kill_Click(sender As Object, e As RoutedEventArgs)
        abschiessen("mgis")
        e.Handled = True
    End Sub

    Shared Sub abschiessen(processname As String)
        Dim currentProcess As Process = Process.GetCurrentProcess()
        Dim myProcesses() As Process
        Dim myProcess As Process
        ' Returns array containing all instances of "Notepad". 
        myProcesses = Process.GetProcessesByName(processname)
        For Each myProcess In myProcesses
            If myProcess.Id <> currentProcess.Id Then
                ' MsgBox(myProcess.Id & "akt,current " & currentProcess.Id)
                myProcess.CloseMainWindow()
            End If
        Next
    End Sub

    Friend Shared Function getgisstartOptionen() As String
        '"multiple"  , "neustart", "nachfrage"
        Dim val As String = userIniProfile.WertLesen("gisstart", "mehrfachinstanzen")
        If String.IsNullOrEmpty(val) Then
            userIniProfile.WertSchreiben("gisstart", "mehrfachinstanzen", "multiple")
            Return "multiple"
        Else
            Return val
        End If
    End Function
    Shared Sub gisStartPolitikUmsetzen(prozessname As String)
        Dim gisStartPolitik As String
        If gisLaeuftschon(prozessname) Then
            '   MsgBox("löppt")
            gisStartPolitik = mgis.clsGisstartPolitik.getgisstartOptionen()
            If gisStartPolitik = "nachfrage" Then
                Dim messageboxresult As New MessageBoxResult
                messageboxresult = MessageBox.Show("Es läuft bereits mind. eine INSTANZ des GIS. Abschiessen?", "Abschiessen?", MessageBoxButton.YesNo)
                If messageboxresult = MessageBoxResult.Yes Then
                    abschiessen(prozessname)
                End If
            End If
            If gisStartPolitik = "neustart" Then
                abschiessen(prozessname)
            End If
            If gisStartPolitik = "multiple" Then
            End If
        End If
    End Sub

    Shared Sub setzeKeinHintergrundLayer(layerHgrund As clsLayerPres)
        With layerHgrund
            .ebene = ""
            .titel = "Kein Hintergrund"
            .schema = ""
            .isHgrund = True
            .masstab_imap = ""
            .mit_imap = False
            .mit_legende = False
            .pfad = ""
            .rang = 0
            .sid = 0
            .standardsachgebiet = ""
            .schlagworte = ""
            .mapFile = ""
            .mapFileHeader = ""
            .SortierKriterium = "Kein Hintergrund"
            .tultipp = "Kein Hintergrund"
            .mit_objekten = False
        End With
    End Sub
End Class
