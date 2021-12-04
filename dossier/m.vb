Imports System.Data.SqlClient
Public Class m
    Public Shared NASlage As New NASlage
    Public Shared dina4InMM, dina3InMM, dina4InPixel, dina3InPixel As New clsCanvas
    Public Const albverbotsString As String = "Der Auszug aus dem Amtlichen Liegenschaftskataster-Informationssystem (ALKIS) darf nur " &
                                            " intern verwendet werden." &
                                            " Eine Weitergabe des Auszugs an Dritte ist unzulässig." &
                                            " Auskünfte aus dem ALKIS an Dritte erteilt - bei Vorliegen eines berechtigten Interesses - " &
                                            " das Katasteramt (kundenservice.afb-heppenheim@hvbg.hessen.de). Alle Zugriffe werden protokolliert."

    Public Shared dossierVersion As String = My.Resources.BuildDate.Trim.Replace(vbCrLf, "")
    Public Const serverWeb As String = "http://w2gis02.kreis-of.local"
    Public Shared aktFST As New ParaFlurstueck

    Public Shared ProxyString As String = ""
    Public Shared paradigmaMsql As New clsDBspecMSSQL
    Public Shared paradigmaMsqlmyconn As SqlConnection
    Public Shared webgisREC As New clsDBspecPG
    Public Shared basisrec As New clsDBspecPG
    Public Const PostgisDBcoordinatensystem As String = "25832"
    Public Shared appServerUnc As String = ""
    Public Shared mgisUserRoot As String = "" 'm.appServerUnc & "\apps\test\mgis\"
    Public Shared Property userIniProfile As clsINIDatei
    Public Shared Property UTMpt As New myPoint
    Public Shared Property MAPPINGgeometrie As String = ""
    Public Shared Property MAPPINGaktiveaid As Integer = 0
    Public Shared Property MAPPINGpunktKoordinatenString As String = ""
    Public Shared Property MAPPINGbreite As String = ""
    Public Shared Property MAPPINGhoehe As String = ""
    Public Shared Property MAPPINGscreenX As String = ""
    Public Shared Property MAPPINGscreenY As String = ""
    Public Shared Property MAPPINGradiusinmeter As String = "3"
    Public Shared Property flurstuecksModus As Boolean = False
    Public Shared Property MAPPINGvid As String = ""
    Public Shared Property MAPPINGfs As String = ""
    Public Shared GisUser As New clsUser
    Friend Shared Sub mapAllArguments(arguments() As String)
        'geometrie=punkt username=feinen_j koordinate=477710,5544860  istalbberechtigt=1   aktaid=0  breite=1500 hoehe=1030 screenx=694 screeny=165
        'geometrie=punkt username=feinen_j koordinate=491062,5547736  istalbberechtigt=1   aktaid=358  breite=1500 hoehe=1030 screenx=556 screeny=269
        'geometrie=flaeche username=feinen_j   istalbberechtigt=1 aktaid=358 breite=1500 hoehe=1030 screenx=733 screeny=479 radiusinmeter=100 gruppe=umwelt fs=FS0607470010000300400
        Try
            l("mapAllArguments---------------------- anfang")
            For Each sttelement In arguments
                If sttelement.Contains("geometrie=punkt") Then
                    l("geometrie=punkt also gesetzt")
                    MAPPINGgeometrie = "punkt"
                End If

                If sttelement.Contains("geometrie=flurstueck") Then
                    l("geometrie=flurstueck also gesetzt")
                    MAPPINGgeometrie = "flurstueck"
                End If
                If sttelement.Contains("fs=") Then
                    l("fs=")
                    MAPPINGfs = sttelement.Replace("fs=", "").Trim
                End If
                If sttelement.Contains("obergruppe=") Then
                    l("obergruppe=umwelt")
                    GisUser.ADgruppenname = sttelement.Replace("obergruppe=", "").Trim.ToLower
                End If
                If sttelement.Contains("untergruppe=") Then
                    l("untergruppe=gis")
                    GisUser.favogruppekurz = sttelement.Replace("untergruppe=", "").Trim.ToLower
                End If
                If sttelement.Contains("vid=") Then
                    l("vid=")
                    MAPPINGvid = sttelement.Replace("vid=", "").Trim.ToLower
                End If
                If sttelement.Contains("username=") Then
                    l("modus=username")
                    GisUser.username = sttelement.Replace("username=", "").Trim.ToLower
                End If
                If sttelement.Contains("aktiveaid=") Then
                    l("aktiveaid=")
                    MAPPINGaktiveaid = CInt(sttelement.Replace("aktiveaid=", "").Trim)
                End If

                If sttelement.Contains("koordinate=") Then
                    MAPPINGpunktKoordinatenString = sttelement.Replace("koordinate=", "").Trim.ToLower
                    l("STARTUP_punktKoordinaten " & MAPPINGpunktKoordinatenString)
                End If

                If sttelement.Contains("istalbberechtigt=1") Then
                    l("istalbberechtigt=" & sttelement)
                    m.GisUser.istalbberechtigt = True
                End If

                If sttelement.Contains("breite=") Then
                    MAPPINGbreite = sttelement.Replace("breite=", "").Trim.ToLower
                    l("MAPPINGbreite " & MAPPINGbreite)
                End If

                If sttelement.Contains("hoehe=") Then
                    MAPPINGhoehe = sttelement.Replace("hoehe=", "").Trim.ToLower
                    l("MAPPINGhoehe " & MAPPINGhoehe)
                End If

                If sttelement.Contains("screenx=") Then
                    MAPPINGscreenX = sttelement.Replace("screenx=", "").Trim.ToLower
                    l("MAPPINGscreenX " & MAPPINGscreenX)
                End If

                If sttelement.Contains("radiusinmeter=") Then
                    MAPPINGradiusinmeter = sttelement.Replace("radiusinmeter=", "").Trim.ToLower
                    l("MAPPINGradiusinmeter " & MAPPINGradiusinmeter)
                End If

                If sttelement.Contains("screeny=") Then
                    MAPPINGscreenY = sttelement.Replace("screeny=", "").Trim.ToLower
                    l("MAPPINGscreenY " & MAPPINGscreenY)
                End If
            Next
            l("mapAllArguments---------------------- ende")
        Catch ex As Exception
            l("Fehler in mapAllArguments: " & ex.ToString())
        End Try

    End Sub

    Shared Sub initdb(host As String)

        m.webgisREC.mydb = New clsDatenbankZugriff
        m.webgisREC.mydb.Host = host
        m.webgisREC.mydb.username = "postgres" : m.webgisREC.mydb.password = "lkof4"
        m.webgisREC.mydb.Schema = "webgiscontrol"
        m.webgisREC.mydb.Tabelle = "flurkarte.basis_f" : m.webgisREC.mydb.dbtyp = "postgis"

        m.basisrec.mydb = New clsDatenbankZugriff
        m.basisrec.mydb.Host = host
        m.basisrec.mydb.username = "postgres" : m.basisrec.mydb.password = "lkof4"
        m.basisrec.mydb.Schema = "postgis20"
        m.basisrec.mydb.Tabelle = "flurkarte.basis_f" : m.basisrec.mydb.dbtyp = "postgis"

        m.paradigmaMsql.mydb = New clsDatenbankZugriff
        m.paradigmaMsql.mydb.Host = "msql01"
        m.paradigmaMsql.mydb.username = "sgis" : m.paradigmaMsql.mydb.password = "WinterErschranzt.74"
        m.paradigmaMsql.mydb.Schema = "Paradigma"
        m.paradigmaMsql.mydb.Tabelle = "" : m.paradigmaMsql.mydb.dbtyp = "sqls"
    End Sub
End Class
