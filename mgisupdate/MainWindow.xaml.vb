Imports System.Windows
Class MainWindow

    Public modul As String = "bplankataster" ' bplanupdater
    Dim downloadlist As String = "downloadlist.txt"
    Public ZielVerzeichnis, quelleAdresse, dateiname As String
    Private sollGisStarten As String = ""
    Property updateDateiliste As String()
    Sub New()
        InitializeComponent()
    End Sub

    Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        clsTools.setLogfile("mgisupdate", IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDocuments),
                                 "Paradigma\cache") & "\logs\")
        'Kopiert alle exe-files in das unterverzeichnis \bplaninternet 
        'createDir(targetroot)

        Dim arguments As String() = Environment.GetCommandLineArgs()
        sollGisStarten = clsTools.getProcid(arguments, "/gisstarten=")
#If DEBUG Then
#Else
#End If
        'clsShortcut.MakeShortcut(ZielVerzeichnis & "\" & "mgisnt.exe",
        '                         "Kreis_Offenbach - DesktopGIS",
        '                         ZielVerzeichnis & "\giskarten.ico")

        Dim dateiImInternet As String = "c:\KreisOffenbach\iminternet.txt"
        Dim existss As Boolean = clsTools.setIminternetFromLokalFile(dateiImInternet)
        If existss Then
            clsTools.ImInternet = True
        Else
            clsTools.ImInternet = clsTools.setIminternet("\\w2gis02\gdvell\apps\bplankat\npgsql.dll") : clsTools.l("Iminternet " & clsTools.ImInternet)
            clsTools.createDir("c:\KreisOffenbach")
            'clsTools.createDir(ZielVerzeichnis)
            IO.File.WriteAllText(dateiImInternet, "hurz")
        End If
        clsTools.serverWeb = webquelleSetzen(clsTools.ImInternet)




        ZielVerzeichnis = setWorkingDir() : clsTools.l("targetroot " & clsTools.ImInternet)
        clsTools.createDir(ZielVerzeichnis)
        clsTools.createDir(ZielVerzeichnis & "\serverVersion")

        clsShortcut.MakeShortcut(ZielVerzeichnis & "\" & "mgisnt.exe",
                                 "Kreis_Offenbach - DesktopGIS",
                                 ZielVerzeichnis & "\giskarten.ico")

        clsTools.l("targetroot " & ZielVerzeichnis)
        IO.Directory.SetCurrentDirectory(ZielVerzeichnis)

        'clsShortcut.MakeShortcut(ZielVerzeichnis & "\" & "mgisupdate.exe",
        '                         "Kreis_Offenbach - DesktopGIS",
        '                         ZielVerzeichnis & "\giskarten.ico")




        '   rest()
        'MsgBox("Downloads abgeschlossen")
        'End
    End Sub

    Private Sub rest()
        Dim lokaleVersion As String = clsUpdate.getLokaleversion("mgis_version.txt").Trim
        If lokaleVersion = "" Then
            gisstarten(sollGisStarten)
            End
        End If
        clsTools.l(" lokaleVersion: " & lokaleVersion)
        Dim serverVersion As String = clsUpdate.getServerVersion(ZielVerzeichnis & "\serverVersion")
        If serverVersion = String.Empty Then
            gisstarten(sollGisStarten)
            End
        End If
        clsTools.l(" serverVersion: " & serverVersion)
        If CInt(lokaleVersion) >= CInt(serverVersion) Then
            'Dim sollupdateStarten As Boolean = getSollUpdateStarten(QuellVersion, lokaleVersion) 
            clsTools.l("kein update")
            gisstarten(sollGisStarten)
            End
        Else
            clsTools.l("  update wird durchgeführt")
            'clsUpdate.starteUpdate()
            'gisstarten()
            'End
        End If
        quelleAdresse = clsTools.serverWeb & "/fkat/paradigma/mgis/releases/"
        clsTools.l("quelleAdresse" & quelleAdresse)
        updateDateiliste = getdateiliste(quelleAdresse, "updateDateiliste.txt", ZielVerzeichnis)
        If updateDateiliste Is Nothing Then
            clsTools.l("kein update")
            gisstarten(sollGisStarten)
            End
        End If
        clsTools.l("  objekte in updateDateiliste.txt: " & updateDateiliste.Count)
        If startenneu(updateDateiliste) Then
            clsTools.l("-----------------")
            clsTools.l("Anwendung wird gestartet")
            tbinfo.Text &= "-----------------" & Environment.NewLine
            tbinfo.Text &= "Anwendung wird gestartet" & Environment.NewLine
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
        Else
            clsTools.l("-----------------")
            clsTools.l("Bitte beim Admin melden: 06074 8180 4434 / dr.j.feinen@kreis-offenbach.de")
            tbinfo.Text &= "-----------------" & Environment.NewLine
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
            tbinfo.Text &= "Bitte beim Admin melden: 06074 8180 4434 / dr.j.feinen@kreis-offenbach.de" & Environment.NewLine
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
        End If
        Dim icondir, updatedir As String
        icondir = ZielVerzeichnis.Replace("\bplaninternet\", "")
        updatedir = ZielVerzeichnis.Replace("\bplaninternet\", "")
        clsShortcut.MakeShortcut(ZielVerzeichnis & "\" & "mgisupdate.exe",
                                 "Kreis_Offenbach - DesktopGIS",
                                 ZielVerzeichnis & "\giskarten.ico")
        gisstarten(sollGisStarten)
    End Sub

    'Private Sub gisBeenden()
    '    'MsgBox("gisBeenden")
    '    Try
    '        Dim proc As Process = getGISProzess()
    '        If proc Is Nothing Then
    '            MsgBox("GIS wurde schon beendet")
    '            Exit Sub
    '        End If
    '        'MsgBox("vor CloseMainWindow")
    '        proc.CloseMainWindow()
    '        'MsgBox("vor kill")
    '        proc.Kill()
    '        'MsgBox("nach kill")
    '    Catch ex As Exception
    '        MsgBox("fehler " & ex.ToString)
    '    End Try
    'End Sub

    'Private Function getGISProzess() As Process
    '    Try
    '        Return Process.GetProcessById(CInt(sollGisStarten))
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try

    'End Function

    Private Sub gisstarten(sollgisstarten As String)
        Try
            If sollgisstarten.ToLower.Trim <> "false" Then
            Else
                Exit Sub
            End If
            Dim startinfo As New ProcessStartInfo
            startinfo.FileName = "mgisnt.exe"
            startinfo.WorkingDirectory = ZielVerzeichnis
            If clsTools.ImInternet Then
                startinfo.WorkingDirectory = ZielVerzeichnis
            Else
                startinfo.WorkingDirectory = "c:\kreisoffenbach\mgis"
                startinfo.FileName = "mgis.exe"
            End If
#If DEBUG Then
            'startinfo.WorkingDirectory = "c:\KreisOffenbach\mgis"
#End If
            startinfo.UseShellExecute = False
            startinfo.Arguments = ""
            ' Process.Start(IO.Path.Combine(Environment.CurrentDirectory, "mgisnt.exe"))
            Process.Start(startinfo)
        Catch ex As Exception
            clsTools.l(ex.ToString)
        Finally
            Title = "Desktop-GIS Update, Built: " & clsTools.ParadigmaVersion
            Close()
        End Try
    End Sub

    Private Function webquelleSetzen(ImInternet As Boolean) As String
        If ImInternet Then
            clsTools.serverWeb = "https://buergergis.kreis-offenbach.de"
        Else
            clsTools.serverWeb = "http://w2gis02.kreis-of.local"
        End If
        clsTools.l("serverWeb " & clsTools.serverWeb)
        Return clsTools.serverWeb
    End Function
    Sub mylog(text As String)
        clsTools.l(Now.ToString)
        tbinfo.Text &= (text) & Environment.NewLine
        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    End Sub
    Private Function startenneu(updateDateiliste() As String) As Boolean
        Try
            mylog(" startenneu ---------------------- anfang")

            quelleAdresse = clsTools.serverWeb & "/fkat/paradigma/mgis/bin/"
            For Each dateiname As String In updateDateiliste
                clsTools.l("datei: " & dateiname)
                If dateiname = String.Empty Then Continue For
                dateiname = dateiname
                mylog(dateiname)
                If clsTools.down(quelleAdresse, dateiname, ZielVerzeichnis) Then
                    mylog((dateiname) & " ok")
                Else
                    mylog(dateiname & " NICHT erhalten")
                    Return False
                End If
            Next
            mylog(" startenneu ---------------------- ende")
            Return True
        Catch ex As Exception
            mylog("Fehler in startenneu: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Function getdateiliste(quelleadresse As String,
                                   namedateiliste As String,
                                   zielverzeichnis As String) As String()
        Try
            clsTools.l(" getdateiliste ---------------------- anfang")
            dateiname = namedateiliste
            mylog(namedateiliste)
            'Dim tZielVerzeichnis = "c:\KreisOffenbach\mgis\" & dateiname
            If clsTools.down(quelleadresse, dateiname, zielverzeichnis) Then
                mylog("Dateiliste erhalten")
                Dim readText As String = IO.File.ReadAllText(zielverzeichnis & "\" & namedateiliste)
                mylog(readText)
                Dim dliste() As String = readText.Split(CType("#", Char()))
                Return dliste
            Else
                mylog("Dateiliste NICHT erhalten")
                Return Nothing
            End If
            mylog(" getdateiliste ---------------------- ende")
            Return Nothing
        Catch ex As Exception
            mylog("Fehler in getdateiliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Sub btnCALLURL_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Try
            Dim url = "https://buergergis.kreis-offenbach.de/fkat/paradigma/mgis/releases/gisinstaller.exe"
            Process.Start(url)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnEnde_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub btnstart_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True

    End Sub

    'Private Sub starten()
    '    sourceroot = "https://buergergis.kreis-offenbach.de/fkat/paradigma/bplan/"
    '    dateiname = "bplaninternet.exe"

    '    Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    '    If clsTools.down(sourceroot, dateiname, targetroot) Then
    '        tbinfo.Text &= ("Update durchgeführt") & Environment.NewLine
    '        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    '        tbinfo.Text &= ("Anwendung wird gestartet!") & Environment.NewLine
    '        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    '        dateiname = "gemeinden.xml"

    '        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    '        If clsTools.down(sourceroot, dateiname, targetroot) Then
    '            tbinfo.Text &= ("Gemeindeliste aktualisiert") & Environment.NewLine
    '            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    '        End If
    '        Process.Start(targetroot & "\" & "bplaninternet.exe")
    '    Else
    '        tbinfo.Text &= ("Update NICHT durchgeführt. Alte Version wird gestartet.") & Environment.NewLine
    '        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    '        Process.Start(targetroot & "\" & dateiname)
    '    End If
    'End Sub

    Private Function setWorkingDir() As String
        Dim targetroot As String
        If clsTools.ImInternet Then
            'targetroot = IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles), "mgis")
            targetroot = "C:\KreisOffenbach\mgis"
        Else
            targetroot = "c:\KreisOffenbach\mgis"
        End If
        Return targetroot
    End Function
End Class
