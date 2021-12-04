Class clsControlling
    Private Sub New()

    End Sub
    'Friend Shared Sub controllingprotokollALT(anzahl As Integer, fdkurz As String)
    '    Try
    '        l("controllingprotokoll---------------------- anfang")
    '        'myglobalz.serverWeb & "/cgi-bin/controlling/counterplus.cgi?&anzahl=12&user=hurax
    '        Dim aufruf As String
    '        Dim sb As New Text.StringBuilder
    '        sb.Append(serverWeb)
    '        sb.Append("/cgi-bin/controlling/counterplus.cgi?" &
    '                  "anzahl=" & anzahl &
    '                  "&modus=" & 1 &
    '                  "&user=" & GisUser.nick &
    '                  "&fd=" & fdkurz)
    '        'modus=1 ist alt
    '        'modus=2 speichert nach sqls
    '        aufruf = sb.ToString
    '        l("controllingprotokoll: genaufruf " & aufruf)
    '        Dim hinweis As String = ""
    '        Dim result As String
    '        result = meineHttpNet.meinHttpJob("", aufruf, hinweis, myglobalz.enc, 0)
    '        l("controllingprotokoll---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in controllingprotokoll: " & ex.ToString())
    '    End Try
    'End Sub

    Shared Sub controllingprotokoll(anzahl As Integer)
        strGlobals.controllingCounter += anzahl
    End Sub

    Shared Sub controllingTransfer(anzahl As Integer)
        Dim modul As String = "GIS"
        Dim total As Integer = anzahl
        Dim jahrmonat As Integer = 0
        Dim jahr = Now.ToString("yyyy")
        Dim monat = Now.ToString("MM")
        Dim querie As String = ""
        Dim hinweis As String = ""
        ' Dim ID As Integer
        Dim returnIdentity As Boolean = True
        Try
            jahrmonat = CInt(jahr) * 100 + CInt(monat)
            If iminternet Then
                'controllincrementHTTP(anzahl, jahrmonat, querie, hinweis)
            Else
                controllincrementDB(anzahl, jahrmonat, querie, hinweis)
            End If

            l("controllingTransfer---------------------- ende")
        Catch ex As Exception
            l("Fehler in controllingTransfer: " & ex.ToString())
        End Try
    End Sub
    Private Shared Sub controllincrementDB(anzahl As Integer, jahrmonat As Integer, ByRef querie As String, ByRef hinweis As String)
        l("controllincrementDB---------------------- anfang")
        querie = " update gis.dbo.GISCONTROLLING set total=total+" & anzahl &
                " where jahrmonat=" & jahrmonat
        Dim dt = modgetdt4sql.getDT4Query(querie, pLightMsql, hinweis)
    End Sub

    Friend Shared Sub controllingprotokollJahreHochzaehlen(anzahl As Integer, fdkurz As String)
        Try
            Dim modul As String = "GIS"
            Dim total As Integer = anzahl
            Dim jahrmonat As Integer = 0
            Dim jahr = Now.ToString("yyyy")
            Dim monat = Now.ToString("MM")
            jahrmonat = CInt(jahr) * 100 + CInt(monat)
            l("controllingprotokollJahreHochzaehlen---------------------- anfang")
            Dim querie As String
            Dim returnIdentity As Boolean = True

            jahr = "2019"
            Try
                For ijahr = 2019 To 2029


                    For i = 1 To 12
                        jahrmonat = CInt(ijahr) * 100 + CInt(i)

                        l("controllingprotokollJahreHochzaehlen b---------------------- anfang")
                        clsSqlparam.paramListe.Clear()
                        querie = "INSERT INTO gis.dbo.GISCONTROLLING (JAHRMONAT,MODUL,TOTAL) VALUES (@JAHRMONAT,@MODUL,@TOTAL)"
                        clsSqlparam.paramListe.Add(New clsSqlparam("JAHRMONAT", jahrmonat)) 'MYGLObalz.sitzung.VorgangsID)
                        clsSqlparam.paramListe.Add(New clsSqlparam("MODUL", "GIS"))
                        clsSqlparam.paramListe.Add(New clsSqlparam("TOTAL", 0))
                        Dim ID = pLightMsql.manipquerie(querie, clsSqlparam.paramListe, True, "gisid")

                    Next
                Next

                l("controllingprotokollJahreHochzaehlen c---------------------- ende")
            Catch ex As Exception
                l("Fehler in controllingprotokollJahreHochzaehlen d: " & ex.ToString())

            End Try

            l("controllingprotokollJahreHochzaehlene---------------------- ende")
        Catch ex As Exception
            l("Fehler in controllingprotokollJahreHochzaehlenf: " & ex.ToString())
        End Try
    End Sub
End Class
