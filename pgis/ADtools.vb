Imports System.Data

Namespace NSfstmysql
    Class ADtools
        Public Shared Function istUserAlbBerechtigt(ByVal username As String, ByRef FDkurz As String) As Boolean
            My.Log.WriteEntry("istUserAlbBerechtigt")
            Dim Filter As String = clsString.umlaut2ue(username)
            Dim lokdt As DataTable = clsActiveDir.sucheperson(Filter)
            Dim Name As String = lokdt.Rows(0).Item("sn").ToString()
            Dim Vorname As String = lokdt.Rows(0).Item("givenName").ToString()
            FDkurz = lokdt.Rows(0).Item("department").ToString()
            Dim userid As String = lokdt.Rows(0).Item("userPrincipalName").ToString
            userid = Environment.UserName
            If istFachdienstErlaubt(FDkurz.ToLower.Replace("fachdienst", "").Trim) Then
                Return True
            End If

            userid = userid.ToLower.Replace("@kreis-of.local", "")
            If istUseridErlaubt(userid.ToLower) Then
                Return True
            End If
            My.Log.WriteEntry("istUserAlbBerechtigt  -- false ende")
            Return False
        End Function

        Private Shared Function istFachdienstErlaubt(p1 As String) As Boolean
            '   myGlobalz.baulastREC.mydb.SQL="select username from webgiscontrol.albrechte where istaktiv=1 and istuserid=0 " & 
            '    " and lower(username) like '%" & p1.trim & "%'"
            'myGlobalz.baulastREC.getDataDT()
            'If myGlobalz.baulastREC.dt.Rows.Count>0 Then
            '     My.Log.WriteEntry(p1 & " erteilt")
            '    Return true                
            'End If
            If p1.ToLower.Contains("bauaufsicht") Then
                My.Log.WriteEntry("bauaufsicht erteilt")
                Return True
            End If
            If p1.ToLower.Contains("umwelt") Then
                My.Log.WriteEntry("umwelt erteilt")
                Return True
            End If
            'Return false
        End Function

        Private Shared Function istUseridErlaubt(p1 As String) As Boolean

            webgisREC.mydb.SQL = "select username from public.albrechte where istaktiv=1 and istuserid=1 " &
                " and lower(username)='" & p1.ToLower.Trim & "'"
            webgisREC.getDataDT()
            If webgisREC.dt.Rows.Count > 0 Then
                My.Log.WriteEntry(p1 & " erteilt")
                Return True
            End If
            'If p1.ToLower.Contains("yalcin_e") Then
            '    My.Log.WriteEntry("yalcin_e erteilt")
            '    Return True
            'End If
            'If p1.ToLower.Contains("rickert_m") Then
            '    My.Log.WriteEntry("rickert_m erteilt")
            '    Return True
            'End If
            'If p1.ToLower.Contains("faust_e") Then
            '    My.Log.WriteEntry("faust_e erteilt")
            '    Return True
            'End If
            'If p1.ToLower.Contains("funk_k") Then
            '    My.Log.WriteEntry("funk_k erteilt")
            '    Return True
            'End If
            'If p1.ToLower.Contains("trumpp_e") Then
            '    My.Log.WriteEntry("trumpp_e erteilt")
            '    Return True
            'End If
            'If p1.ToLower.Contains("feinen_j") Then
            '    My.Log.WriteEntry("feinen_j erteilt")
            '    Return True
            'End If

        End Function

    End Class
End Namespace

