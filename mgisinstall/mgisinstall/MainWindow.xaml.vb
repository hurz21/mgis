Class MainWindow
    Public modul As String = "bplankataster" ' bplanupdater
    Public targetroot, sourceroot, filename As String
    Sub New()
        InitializeComponent()
    End Sub
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        targetroot = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) & "\" & modul
        'targetroot = Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles) & "\" & modul
        sourceroot = "https://buergergis.kreis-offenbach.de/fkat/paradigma/mgis/"
        spErfolg.Visibility = Visibility.Collapsed
        spAusfuehren.Visibility = Visibility.Collapsed
        tbVorabinfo.Text = getvorabinfo()
        sc1.Visibility = Visibility.Collapsed
        Height = 450
        clsTools.ParadigmaVersion = My.Resources.BuildDate.Trim.Replace(vbCrLf, "")
        Title = Title & " (" & Environment.UserName & "&" & Environment.UserDomainName & ") Built: " & clsTools.ParadigmaVersion
    End Sub
    Private Sub OnNavigate(sender As Object, e As RequestNavigateEventArgs)
        e.Handled = True
        Process.Start(e.Uri.AbsoluteUri)

    End Sub
    Private Function getvorabinfo() As String
        Dim aas As String
        aas = "Diese Programm vermeidet Probleme der webgestützten Anwendung, wie " & Environment.NewLine &
              "sie z.B. bei der Darstellung von PDF-Dateien im Browser entstehen. " & Environment.NewLine &
              "Beim Einsatz in der Kommune kann der Zugriff durch 'Caching' beschleunigt werden. " & Environment.NewLine &
              "Dann arbeitet das System auch Offline." & Environment.NewLine
        aas = "Dies ist die Windows-Version des 'BürgerGIS Kreis Offenbach' " & Environment.NewLine '&
        '"Es bietet folgende Vorteile: " & Environment.NewLine &
        '" - Einwandfreie Darstellung und Download von PDF-PLänen " & Environment.NewLine &
        '" - Beschleunigeter Zugriff durch 'Caching'  " & Environment.NewLine
        Return aas
    End Function

    Private Sub starteAnwendungsUpdater(targetroot As String, exe As String)
        Try
            clsTools.l(" starteAnwendungsUpdater ---------------------- anfang")
            MsgBox("Die Anwendung wird gestartet:" & Environment.NewLine &
                   targetroot & "\" & exe,, "Install beendet")
            Process.Start(targetroot & "\" & exe)
            clsTools.l(" starteAnwendungsUpdater ---------------------- ende")
        Catch ex As Exception
            clsTools.l("Fehler in starteAnwendungsUpdater: " & ex.ToString())
        End Try

    End Sub

    Private Sub btnInstall_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If InstallUpdater() Then
            Dim text As String
            text = "Das Programm wurde installiert. Sie können es im Verzeichnis " & Environment.NewLine &
                            targetroot & Environment.NewLine & "finden. " & Environment.NewLine &
                            "Es sollte nun eine Verknüpfung auf dem Desktop angelegt werden." & Environment.NewLine &
                            "Sie können dies händisch erledigen, hierzu können sie das Programmverzeichnis öffen" & Environment.NewLine &
                            "und eine Verknüpfung auf das Programm 'bplanupdate.exe' erstellen" & Environment.NewLine &
                            "(rechte Maustaste auf 'bplanupdate.exe' ...). " & Environment.NewLine &
                            "Sie sollten die Verknüpfung benennen als 'Bebauungsplakataster Kreis Offenbach'" & Environment.NewLine & Environment.NewLine &
                            "Alternativ können sie dies auch vom Installationsprogramm erledigen lassen." & Environment.NewLine &
                            "Dies klappt aber - abhängig von Ihrem PC - nicht immer!"
            tbinfo.Text = (text) & Environment.NewLine
            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
            ' MessageBox.Show(text, "Installation erfolgreich")
            tbinfo.Visibility = Visibility.Visible
            spErfolg.Visibility = Visibility.Visible
            btnShortcut.Visibility = Visibility.Collapsed
            sc1.Visibility = Visibility.Visible
            spAusfuehren.Visibility = Visibility.Visible

            btnStarteUpdate.Visibility = Visibility.Visible

            spTop.Visibility = Visibility.Collapsed
            tbVorabinfo.Visibility = Visibility.Collapsed
            'btnInstall.Visibility = Visibility.Collapsed
            'btnabbruch.Visibility = Visibility.Collapsed
        End If
    End Sub
    Sub mylog(text As String)
        clsTools.l(Now.ToString)
        tbinfo.Text &= (text) & Environment.NewLine
        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    End Sub

    Private Function InstallUpdater() As Boolean
        clsTools.setLogfile("Mgisinstall")
        clsTools.l(Now.ToString)
        'down programmdateien
        'shortcut erzeugen 
        filename = "mgisupdate.exe"  '-----------------------------------
        If clsTools.down(sourceroot, filename, targetroot) Then
            mylog(filename & " wurde gedownloaded")
            filename = "pl.ico" '-----------------------------------
            mylog(filename & " wird gedownloaded")
            If clsTools.down(sourceroot, filename, targetroot) Then
                mylog(filename & " wird gedownloaded")
                mylog("Verknüpfung wird angelegt")
                filename = "Interop.IWshRuntimeLibrary.dll" '-----------------------------------
                mylog(filename & " wird gedownloaded")
                Dim downloaddir = AppDomain.CurrentDomain.BaseDirectory
                mylog("downloaddir: " & downloaddir)
                If clsTools.down(sourceroot, filename, targetroot) Then
                    Return True
                Else
                    Return False
                End If
            Else
                mylog(filename & " wurde NICHT gedownloaded")
                Return False
            End If
        Else
            mylog(filename & " wurde NICHT gedownloaded")
            Return False
        End If
    End Function

    Private Function MakeShortcut(ByVal File As String, ByVal ShortcutFolder As String, ByVal Name As String,
                                  ByVal WorkDirectory As String, iconloc As String) As Boolean
        Try
            clsTools.l(" MakeShortcut ---------------------- anfang")
            Dim WshShell As Object = CreateObject("WScript.Shell")
            Dim NewShortcut As Object = WshShell.CreateShortcut(ShortcutFolder & "\" & Name & ".lnk")

            NewShortcut.TargetPath = File
            NewShortcut.WindowStyle = 1
            NewShortcut.IconLocation = iconloc & ",0"
            NewShortcut.WorkingDirectory = WorkDirectory
            NewShortcut.Save()
            clsTools.l(" MakeShortcut ---------------------- ende")
            Return True
        Catch ex As Exception
            clsTools.l("Fehler in MakeShortcut: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Sub btnExplorer_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Process.Start(targetroot)
    End Sub

    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Close()
    End Sub

    Private Sub btnStarteUpdate_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True

        starteAnwendungsUpdater(targetroot, "bplanupdate.exe")

        Close()

    End Sub

    Private Sub btnShortcut_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'doShortcut()

        clsShortcut.MakeShortcut(targetroot & "\" & "mgisupdate.exe",
                                 "Kreis_Offenbach DesktopGIS",
                                 targetroot & "\pl.ico")
    End Sub

    Private Function doShortcut() As Boolean
        Try
            clsTools.l(" doShortcut ---------------------- anfang")
            clsTools.l("vor make")
            Dim desktop As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            If MakeShortcut(targetroot & "\mgisupdate.exe", desktop, "Kreis Offenbach DesktopGIS",
                            targetroot, targetroot & "\pl.ico") Then
                'tbinfo.Text &= ("Verknüpfung wurde angelegt") & Environment.NewLine

                clsTools.l(tbinfo.Text)
                Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents

                MessageBox.Show("Verknüpfung wurde angelegt. Sie finden die Verknüpfung auf dem Desktop unter " &
                                "der Bezeichnung 'Kreis Offenbach - DesktopGIS'! ", "Verknüpfung anlegen")

                clsTools.l(" doShortcut ---------------------- ende")
                Return True
            Else
                MessageBox.Show("Verknüpfung wurde NICHT angelegt. Bitte legen Sie die Verknüpfung selber an! ", "Verknüpfung anlegen")
                Return False
            End If

        Catch ex As Exception
            clsTools.l("Fehler in doShortcut: " & ex.ToString())
            Return False
        End Try
    End Function


End Class

