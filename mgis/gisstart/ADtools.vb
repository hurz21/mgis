Imports System.Data
Namespace NSfstmysql
    Class ADtools
        Public Shared verbotsString As String = "Der Auszug aus dem Amtlichen Liegenschaftskataster-Informationssystem (ALKIS) darf nur " &
            "intern verwendet werden." & " Eine Weitergabe des Auszugs an Dritte ist unzulässig." &
            " Auskünfte aus dem ALKIS an Dritte erteilt - bei Vorliegen eines berechtigten Interesses - " &
            "das Katasteramt (kundenservice.afb-heppenheim@hvbg.hessen.de). Alle Zugriffe werden protokolliert."
        Public Shared Function istUserAlbBerechtigt(ByVal username As String) As Boolean
            If Environment.UserName = "hurz" Then
                clsActiveDir.fdkurz = "umwelt"
                Return True
            End If
            My.Log.WriteEntry("istUserAlbBerechtigt----------------------------------")
            Dim Filter As String = clsString.umlaut2ue(username)
            ' Dim lokdt As DataTable = clsActiveDir.sucheperson(Filter)
            'Dim Name As String = lokdt.Rows(0).Item("sn").ToString()
            'Dim Vorname As String = lokdt.Rows(0).Item("givenName").ToString()
            ' clsActiveDir.fdkurz = lokdt.Rows(0).Item("department").ToString()
            'Dim userid As String = lokdt.Rows(0).Item("userPrincipalName").ToString
            'userid =   GisUser.username

            If istFachdienstErlaubt(clsActiveDir.fdkurz.ToLower.Replace("fachdienst", "").Trim) Then
                Return True
            Else
                l("ist nicht fd berechtigt")
            End If

            GisUser.username = GisUser.username.ToLower.Replace("@kreis-of.local", "")
            If istUseridErlaubt(GisUser.username.ToLower.Trim) Then
                Return True
            Else

            End If
            l("istUserAlbBerechtigt  -- false ende")
            Return False
        End Function

        Shared Function istFachdienstErlaubt(p1 As String) As Boolean

            Try
                l("istFachdienstErlaubt---------------------- anfang")
                If p1.ToLower.Contains("bauaufsicht") Then
                    My.Log.WriteEntry("bauaufsicht erteilt")
                    Return True
                End If
                If p1.ToLower.Contains("umwelt") Then
                    My.Log.WriteEntry("umwelt erteilt")
                    Return True
                End If
                Return False
                l("istFachdienstErlaubt---------------------- ende")
            Catch ex As Exception
                l("Fehler in istFachdienstErlaubt: " & ex.ToString())
                Return False
            End Try
        End Function

        Private Shared Function istUseridErlaubt(p1 As String) As Boolean
            Try
                l("istUseridErlaubt ---------------------- anfang")

                Dim dt As DataTable
                Dim SQL = "select username from webgiscontrol.public.albrechte where istaktiv=1 and istuserid=1 " &
                " and trim(lower(username))='" & p1.Trim.ToLower & "'"
                dt = getDTFromWebgisDB(SQL, "webgiscontrol")
                l("dt.Rows.Count: " & dt.Rows.Count)
                If dt.Rows.Count > 0 Then
                    l(p1 & " erteilt")
                    Return True
                End If
                l(p1 & " NICHT erteilt")
                Return False
                l("---------------------- ende")
            Catch ex As Exception
                l("Fehler in : " & ex.ToString())
                Return False
            End Try
        End Function

    End Class
End Namespace
