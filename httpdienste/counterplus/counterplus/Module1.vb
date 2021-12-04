
Module Module1
    Property mycgi As clsCGI4VBNET
    Property enc As Text.Encoding = Text.Encoding.UTF8
    'http://w2gis02.kreis-of.local/cgi-bin/controlling/counterplus.exe?&anzahl=12
    'http://w2gis02.kreis-of.local/cgi-bin/controlling/counterplus.cgi?anzahl=12&modus=1&fd=
    'http://w2gis02.kreis-of.local/cgi-bin/controlling/counterplus.cgi?anzahl=12&modus=1&fd=unb
    Sub Main()
        Dim sindiminternet As Boolean = sindImmInternet()
        Dim jahr, anzahl As String
        mycgi = New clsCGI4VBNET("dr.j.feinen@kreis-offenbach.de")
        protokoll()
        l(CType(Now, String))
        l("start dbgrab:" & Now.ToString)
        l("anzahl " & mycgi.GetCgiValue("anzahl"))
        l("modus " & mycgi.GetCgiValue("modus"))
        l("fd " & mycgi.GetCgiValue("fd"))
        anzahl = mycgi.GetCgiValue("anzahl")
#If DEBUG Then
        anzahl = "12"
#End If


        jahr = Format(Now, "yyyy")
        l("sindiminternet " & sindiminternet)
        Dim sql As String
        sql = composeSQL(sindiminternet, jahr)
        l(sql)
        '   If istZeileVorhanden(sql) Then
        sql = composeSQLUpdate(sindiminternet, jahr, CInt(anzahl))
        l(sql)
        inkrementTotal(sql)
        '  End If

    End Sub

    Private Function istZeileVorhanden(sql As String) As Boolean
        l("istZeileVorhanden---------------------")
        Dim erfolg As Boolean = grabDataTable(sql)

        l("fertig")
        Return erfolg
    End Function

    Private Function composeSQL(sindiminternet As Boolean, jahr As String) As String
        l("composeSQL-------------------------------")
        Dim where As String = " where iminternet=" & sindiminternet &
            " And jahr='" & jahr & "'"
        Return "select * From public.controlling_total  " & where
    End Function

    Private Function composeSQLUpdate(sindiminternet As Boolean, jahr As String, anzahl As Integer) As String
        l("composeSQLUpdate-------------------------------")
        Dim where As String = " where iminternet=" & sindiminternet &
            " and jahr='" & jahr & "'"
        Return "update   public.controlling_total  set total=total + " & anzahl & where
    End Function
    Public Sub l(text As String)
        My.Application.Log.WriteEntry(text)
    End Sub
    Private Sub protokoll()
        With My.Application.Log.DefaultFileLogWriter
#If DEBUG Then
            .CustomLocation = "c:\" & "protokoll"
#Else
            .CustomLocation = "d:\websys\" & "protokoll"
#End If
            .BaseFileName = "counterplus" '& mycgi.GetCgiValue("anzahl") & "_" & mycgi.GetCgiValue("viewname")
            .AutoFlush = True
            .Append = False
        End With
    End Sub
    Private Function sindImmInternet() As Boolean
        Dim stmp = CType(System.Environment.GetEnvironmentVariable("MG_iminternet"), String)
        Dim hinweis = " MG_iminternet:" & stmp
        l("  sasss MG_iminternet:stmp " & stmp)
        l("sind im internet " & stmp)
        If stmp = "0" Then
            Return False
        End If
        If stmp = "1" Then
            Return True
        End If
        Return False
    End Function


End Module
