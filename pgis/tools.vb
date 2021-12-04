

Imports System.Data

Module tools
    'Public bbox As New clsRange
    Public punktarrayInM() As myPoint
    Public ProxyString As String = ""
    Public adrREC As New clsDBspecPG
    Public fstREC As New clsDBspecPG
    Public webgisREC As New clsDBspecPG
    Public bbox As New clsRange
    Public HausKombi, weistauf, zeigtauf,
        rechts, hoch,
        latitude, longitude, gemeindestring, gemarkungsstring, strcode, sname, gemeindebigNRstring As String
    Public _verz As String = "\\w2gis02\gdvell\apps\eigentuemer\gemarkungen.xml"
    Public gemeinde_verz As String = "\\w2gis02\gdvell\apps\eigentuemer\gemeinden.xml"
    Public _protokoll As String = "\\w2gis02\gdvell\apps\eigentuemer\zugriffNEU.txt"
    Public zugriffsprot As String = "\\w2gis02\gdvell\apps\test\pgis\zugriff3d.txt"
    Public fehlerlog As String = "\\w2gis02\gdvell\apps\pgis\Fehler3dpgis\" & Environment.UserName & ".txt"
    Public aktFST As New clsFlurstueck
    Public fdkurz As String
    Public verbotsString As String = "Der Auszug aus dem Amtlichen Liegenschaftskataster-Informationssystem (ALKIS) darf nur " & "intern verwendet werden." & " Eine Weitergabe des Auszugs an Dritte ist unzulässig." & " Auskünfte aus dem ALKIS an Dritte erteilt - bei Vorliegen eines berechtigten Interesses - " & "das Katasteramt (kundenservice.afb-heppenheim@hvbg.hessen.de). Alle Zugriffe werden protokolliert."


    Sub l(text As String)
        nachricht(text)
    End Sub
    Function getproxystring() As String
        Dim wert$ = "-1"
        Dim a$ = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\" &
                    "Microsoft\Windows\CurrentVersion\Internet Settings",
                    "ProxyServer", wert).ToString
        If a = "-1" Then
            a = ""
        Else
            a = "http://" & a$
        End If
        nachricht("myGlobalz.ProxyString$: " & a)
        Return a
    End Function
    Friend Function getSchnellbatch(fS As String) As String
        Dim eigSDB As New clsEigentuemerschnell
        Dim dt As DataTable = Nothing
        Dim Eigentuemernameundadresse, eigentumerKurzinfo As String
        Dim mycount As Integer
        eigSDB.oeffneConnectionEigentuemer()
        If eigSDB.getEigentuemerdata(fS, eigentumerKurzinfo, Eigentuemernameundadresse, mycount, dt) Then
            Return eigentumerKurzinfo
        Else
            Return "fehler"
        End If
    End Function
    Public Sub Protokollausgabe_aller_Parameter(flurstueck As String, grund As String)
        Try
            Dim sw As New IO.StreamWriter(_protokoll, True)
            sw.WriteLine(Now & "#" & Environment.UserName & "#" & fdkurz & "#" & "DESKTOP" & "#" & grund & "#" & flurstueck & "#" & "#" & "#" & "#" & "#")
            sw.Close()
            sw.Dispose()
        Catch ex As Exception
            'sw.WriteLine("Fehler in kontzrollausgabe:" & ex.ToString)
        End Try
    End Sub

    Public Sub Protokollausgabe_aller_Zugriff(n3d As String)
        Try
            Dim sw As New IO.StreamWriter(zugriffsprot, True)
            sw.WriteLine(Now & "#" & Environment.UserName & "#" & fdkurz & "#" & "pgis" & "#" & n3d & "#" & "" & "#" & "#" & "#" & "#" & "#")
            sw.Close()
            sw.Dispose()
        Catch ex As Exception
            'sw.WriteLine("Fehler in kontzrollausgabe:" & ex.ToString)
        End Try
    End Sub
    Sub nachricht(text As String)
        'Debug.Print(text)
        Try
            Dim sw As New IO.StreamWriter(fehlerlog, True)
            sw.WriteLine(Now & "#" & Environment.UserName & "#" & text)
            sw.Close()
            sw.Dispose()
        Catch ex As Exception
            'sw.WriteLine("Fehler in kontzrollausgabe:" & ex.ToString)
            MsgBox("nachricht " & ex.ToString)
        End Try
    End Sub
End Module
