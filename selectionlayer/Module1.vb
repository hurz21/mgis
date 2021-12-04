Module Module1
    Property mycgi As clsCGI4VBNET
    Private nick As String
    Private modus As String
    Private outfile, sql, sqlvalue As String
    Private mac As String
    Public Property vergleichsoperator As String = "gleich"
    Public Property vergleichswert As String = "*"
    Private nid As String
    Private aid As String
    Public Property selinfo As String = ""
    Property zeitStart As Date
    Property raumtyp As String = "punkt"
    Property zeitend As Date
    'http://w2gis02.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer.cgi?user=Feinen_J&vid=23608&modus=einzeln
    'http://w2gis02.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=feij&modus=liste&sql=SELECT gid,geom  FROM arten_tiere.arten_tiere_p  where trim(spec)&value=Bombina variegata
    'http://gis.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=feij&modus=liste&sql=SELECT gid,geom  FROM arten_tiere.arten_tiere_p  where trim(spec)&value=Bombina variegata
    'http://gis.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=feij&modus=liste&sql=SELECT gid,geom  FROM arten_tiere.arten_tiere_p  where trim(spec)&sqlvalue=Bombina variegata


    'http://gis.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=feij&modus=einzeln&sql=SELECT%20gid,geom%20%20FROM%20arten_tiere.arten_tiere_p%20%20where%20trim(spec)&sqlvalue=Bombina%20variegata
    'http://gis.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=feinen_j&modus=einzeln&sql=SELECT%20gid,geom%20%20FROM%20arten_tiere.arten_tiere_p%20%20where%20trim(spec)&sqlvalue=Bombina%20variegata&aid=485
    'SELECT gid,geom  FROM arten_tiere.arten_tiere_p  where trim(spec)='Bombina variegata'
    Sub Main()
        Dim DbTyp As String = "sqls"

#If DEBUG Then
        'isdebugmode = True
        DbTyp = "sqls"
#End If
        mycgi = New clsCGI4VBNET("dr.j.feinen@kreis-offenbach.de")
        protokoll()
        getCgiParams()
        mac = ""

        showSteuerParams()
        l("DbTyp:: " & DbTyp)
        modTools.enc = System.Text.Encoding.GetEncoding(("iso-8859-2"))
        'modTools.enc = System.Text.Encoding.UTF8
        If Not eingabeist_ok() Then
            mycgi.SendHeader("Eingaben unvollständig")
            mycgi.Send("Eingaben unvollständig")
            l("Eingaben unvollständig")
            End
        End If
        'modTools.main2()
        Dim returnstring As String = ""
        Dim SELECTSTATEMENT As String
        If vergleichsoperator = "gleich" Then

            SELECTSTATEMENT = sql '& " = '" & sqlvalue.Trim & "'"
        End If
        If vergleichsoperator = "like" Then

            vergleichswert = vergleichswert.Replace("*", "%")
            SELECTSTATEMENT = sql & " like '" & vergleichswert.Trim & "'"
        End If
        l("SELECTSTATEMENT " & SELECTSTATEMENT)
        Dim result As String = modTools.main2(nick, nid, modus, returnstring, DbTyp, mac, SELECTSTATEMENT, sqlvalue, aid, selinfo, raumtyp, vergleichsoperator, vergleichswert)
        mycgi.SendHeaderAJAX()
        mycgi.Send("job ok" & "#" & returnstring)

        l(result)
        l("dauer ms: " & CStr(System.DateTime.Now.Subtract(zeitStart).TotalMilliseconds))
        l("----------------- finito")
    End Sub
    Public Sub protokoll()
        With My.Application.Log.DefaultFileLogWriter
#If DEBUG Then
            .CustomLocation = "c:\" & "protokoll"
#Else
            .CustomLocation = "d:\websys\" & "protokoll"
#End If

            .BaseFileName = "userSelectionLayer_" & mycgi.GetCgiValue("nick") & "_" & mycgi.GetCgiValue("modus") & "_" & mycgi.GetCgiValue("unr")
            .AutoFlush = True
            .Append = False
        End With
        zeitStart = Now
        l("protokoll now: " & zeitStart)
    End Sub
    Public Sub showSteuerParams()
        l("-----------------showCgiParams ---------------------- ")
        l(mycgi.sFormData)
        l("nick: " & nick)
        l("selinfo: " & selinfo)
        l("vergleichsoperator,: " & vergleichsoperator)
        l("vergleichswert: " & vergleichswert)
        l("raumtyp: " & raumtyp)
        l("aid: " & aid)
        l("modus: " & modus)
        l("nid: " & nid)
        'l("gemcode: " & gemcode)
        'l("fs: " & fs)
        l("outfile: " & outfile)
        l("sql: " & sql)
        l("sqlvalue: " & sqlvalue)
        l("mac: " & mac)
        l("---------------- showCgiParams ende ")
    End Sub

    Private Sub getCgiParams()
        l("getCgiParams -------------------------")
        Try
#If DEBUG Then
            nick = "petersdorff_l"
            nick = "feinen_j"
            nid = "36677"
            nid = "41263                                                                                               "
            modus = "einzeln"
            aid = "485"
            selinfo = "bla"
            vergleichsOperator="gleich"
            vergleichsOperator="like"
            vergleichswert="brachs*"
            'modus = "sachgebiet3307"

            'username = "feij"
            'modus = "einzeln"
            'vid = "27715"
            raumtyp="punkt"

            'modus = "liste"
            'outfile = "Feinen_J"
            'username = "feinen_j"
            'mac = (mycgi.GetCgiValue("mac"))
            'rid = "26929"
            'fs = "FS0607280020000100700" 'der dateiname kann nicht über cgi geleitet werden. funzt nicht
            'gemcode = "728"
            '        rbtyp   fst = 2
            '            url = url & "&vergleichsOperator=" & vergleichsOperator.Trim
            'url = url & "&vergleichswert=" & vergleichswert.Trim
#Else
            raumtyp = mycgi.GetCgiValue("raumtyp")
            selinfo = mycgi.GetCgiValue("selinfo")
            aid = mycgi.GetCgiValue("aid")
            nick = mycgi.GetCgiValue("nick")
            nid = (mycgi.GetCgiValue("nid"))
            modus = (mycgi.GetCgiValue("modus"))
            sql = (mycgi.GetCgiValue("sql"))
            sqlvalue = (mycgi.GetCgiValue("sqlvalue"))
            mac = (mycgi.GetCgiValue("mac"))
            vergleichsoperator = (mycgi.GetCgiValue("vergleichsoperator"))
            vergleichswert = (mycgi.GetCgiValue("vergleichswert"))

            If nick = String.Empty Then nick = (mycgi.GetCgiValue("outfile"))
#End If
            'If isdebugmode Then

            'Else

            'End If
        Catch ex As Exception
            l("fehler in getCgiParams: " & ex.ToString)
        End Try

    End Sub

    Public Function eingabeist_ok() As Boolean
        Return True
        l("eingabeist_ok-------------------")
        Try
            If modus = "einzeln" And CInt(nid) < 1 Then
                l("Fehler :vid) < 1  ")
                Return False
            End If
            'If String.IsNullOrEmpty(username) And String.IsNullOrEmpty(outfile) Then
            '    l("Fehler :username " & username)
            '    Return False
            'End If

            Return True
        Catch ex As Exception
            l("Fehler ineingabeist_ok : " & ex.ToString)
            Return False
        End Try
    End Function
End Module
