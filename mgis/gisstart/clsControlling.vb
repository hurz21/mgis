Class clsControlling
    Friend Shared Sub controllingprotokollALT(anzahl As Integer, fdkurz As String)
        Try
            l("controllingprotokoll---------------------- anfang")
            'myglobalz.serverWeb & "/cgi-bin/controlling/counterplus.cgi?&anzahl=12&user=hurax
            Dim aufruf As String
            Dim sb As New Text.StringBuilder
            sb.Append(serverWeb)
            sb.Append("/cgi-bin/controlling/counterplus.cgi?" &
                      "anzahl=" & anzahl &
                      "&modus=" & 1 &
                      "&user=" & GisUser.username &
                      "&fd=" & fdkurz)
            'modus=1 ist alt
            'modus=2 speichert nach sqls
            aufruf = sb.ToString
            l("controllingprotokoll: genaufruf " & aufruf)
            Dim hinweis As String = ""
            Dim result As String
            result = meineHttpNet.meinHttpJob("", aufruf, hinweis, myglobalz.enc, 0)
            l("controllingprotokoll---------------------- ende")
        Catch ex As Exception
            l("Fehler in controllingprotokoll: " & ex.ToString())
        End Try
    End Sub
    Shared Sub controllingprotokoll(anzahl As Integer)
        Dim modul As String = "GIS"
        Dim total As Integer = anzahl
        Dim jahrmonat As Integer = 0
        Dim jahr = Now.ToString("yyyy")
        Dim monat = Now.ToString("MM")
        Dim erfolg As Boolean
        Dim querie, hinweis As String
        ' Dim ID As Integer
        Dim returnIdentity As Boolean = True
        Try
            jahrmonat = CInt(jahr) * 100 + CInt(monat)
            l("controllingprotokoll---------------------- anfang")
            querie = " update gis.dbo.GISCONTROLLING set total=total+" & anzahl &
                    " where jahrmonat=" & jahrmonat
            Dim dt = modgetdt4sql.getDT4Query(querie, pLightMsql, hinweis)
            l("controllingprotokoll---------------------- ende")
        Catch ex As Exception
            l("Fehler in controllingprotokoll: " & ex.ToString())
        End Try
    End Sub
    Friend Shared Sub controllingprotokollJahreHochzaehlen(anzahl As Integer, fdkurz As String)
        Try
            Dim modul As String = "GIS"
            Dim total As Integer = anzahl
            Dim jahrmonat As Integer = 0
            Dim jahr = Now.ToString("yyyy")
            Dim monat = Now.ToString("MM")
            jahrmonat = CInt(jahr) * 100 + CInt(monat)
            l("controllingprotokoll---------------------- anfang")
            Dim erfolg As Boolean
            Dim querie As String
            ' Dim ID As Integer
            Dim returnIdentity As Boolean = True

            jahr = "2019"
            Try
                For ijahr = 2019 To 2029


                    For i = 1 To 12
                        jahrmonat = CInt(ijahr) * 100 + CInt(i)

                        l("addUser---------------------- anfang")
                        clsSqlparam.paramListe.Clear()
                        querie = "INSERT INTO gis.dbo.GISCONTROLLING (JAHRMONAT,MODUL,TOTAL) VALUES (@JAHRMONAT,@MODUL,@TOTAL)"
                        clsSqlparam.paramListe.Add(New clsSqlparam("JAHRMONAT", jahrmonat)) 'MYGLObalz.sitzung.VorgangsID)
                        clsSqlparam.paramListe.Add(New clsSqlparam("MODUL", "GIS"))
                        clsSqlparam.paramListe.Add(New clsSqlparam("TOTAL", 0))
                        Dim ID = pLightMsql.manipquerie(querie, clsSqlparam.paramListe, True, "gisid")

                    Next
                Next

                l("addUser---------------------- ende")
            Catch ex As Exception
                l("Fehler in addUser: " & ex.ToString())

            End Try

            l("controllingprotokoll---------------------- ende")
        Catch ex As Exception
            l("Fehler in controllingprotokoll: " & ex.ToString())
        End Try
    End Sub
End Class
