Imports System.Data
Imports System.IO
Module Module1
    Private sachgebietid As String
    Public dina4InMM, dina3InMM, dina4InPixel, dina3InPixel As New clsCanvas
    Public debug_protokoll As Boolean = True
    Property mycgi As clsCGI4VBNET
    Property enc As Text.Encoding = Text.Encoding.UTF8
    'http://w2gis02.kreis-of.local/cgi-bin/controlling/counterplus.exe?&anzahl=12
    ' http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=weinachtsmann&modus=gettable&viewname=ref_doku
    'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=weinachtsmann&modus=biniminternet
    'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=weinachtsmann&modus=getstamm 
    'https://buergergis.kreis-offenbach.de/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=weinachtsmann&modus=gettable&viewname=hintergrund
    'https://buergergis.kreis-offenbach.de/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?modus=userandinternetinfo&nick=zahnlückenpimpf&pw=kucksdu&userinfo=feinen_j+DC4A3E91649500000000000000E0+KREIS-OF+kucksdu+KROF-000019
    'https://buergergis.kreis-offenbach.de/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getbegleitbplan&pdf=di_41&gemarkung=dietzenbach

    'http://gis.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=prepbaulast&tiff=61364&gemarkung=langen


    'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?nick=weinachtsmann&modus=setFavorit&gruppe=gruppe&titel=titel&vorhanden=vorhanden&gecheckt=gecheckt&hgrund=hgrund&aktiv=aktiv&ts=ts
    Sub Main()
        Dim iminternet As Boolean
        Dim nick, pw, userinfo As String
        Dim ist_Standard As String = ""
        Dim viewname As String = ""
        Dim prefix As String = ""
        Dim aid As String = "0"
        Dim wherestring As String = ""
        Dim modus As String = "get"
        Dim result, tabelle, colname, neuerwert As String

        mycgi = New clsCGI4VBNET("dr.j.feinen@kreis-offenbach.de")
        If debug_protokoll Then protokoll()
        l(CType(Now, String))
        l("start dbgrab:" & Now.ToString)
        l(mycgi.GetCgiValue("user"))
        l(mycgi.GetCgiValue("user"))

        Dim stmp = CType(System.Environment.GetEnvironmentVariable("MG_iminternet"), String)
        Dim hinweis = " MG_iminternet:" & stmp
        l("  sasss MG_iminternet:stmp " & stmp)

        If Not String.IsNullOrEmpty(stmp) Then
            If Val(stmp) < 1 Then
                iminternet = False
            Else
                iminternet = True
            End If
        End If
        l("stmp  " & mycgi.GetCgiValue("stmp"))

        l("iminternet  " & iminternet)

        l(mycgi.GetCgiValue("viewname"))
        l(mycgi.GetCgiValue("prefix"))
        l(mycgi.GetCgiValue("aid"))
        l(mycgi.GetCgiValue("tabelle"))
        l(mycgi.GetCgiValue("colname"))
        l(mycgi.GetCgiValue("neuerwert"))
        l("modus:" & mycgi.GetCgiValue("modus"))
        l(mycgi.GetCgiValue("sachgebietid"))
        l(mycgi.GetCgiValue("orderby"))
        defineDinA4Dina3Formate()

        l("CGI_ServerName:  " & mycgi.CGI_ServerName)
        l("ist_Standard " + mycgi.GetCgiValue("ist_Standard"))

        tabelle = Trim(mycgi.GetCgiValue("tabelle"))
        colname = Trim(mycgi.GetCgiValue("colname"))
        neuerwert = Trim(mycgi.GetCgiValue("neuerwert"))

        prefix = Trim(mycgi.GetCgiValue("prefix"))
        modus = Trim(mycgi.GetCgiValue("modus")) : If modus = String.Empty Then modus = "get"
        modus = modus.ToLower
        aid = Trim(mycgi.GetCgiValue("aid"))
        sachgebietid = Trim(mycgi.GetCgiValue("sachgebietid"))
        ist_Standard = Trim(mycgi.GetCgiValue("ist_Standard"))


        nick = Trim(mycgi.GetCgiValue("nick"))
        Dim gruppe = Trim(mycgi.GetCgiValue("gruppe"))
        Dim titel = Trim(mycgi.GetCgiValue("titel"))
        Dim vorhanden = Trim(mycgi.GetCgiValue("vorhanden"))
        Dim gecheckt = Trim(mycgi.GetCgiValue("gecheckt"))
        Dim hgrund = Trim(mycgi.GetCgiValue("hgrund"))
        Dim aktiv = Trim(mycgi.GetCgiValue("aktiv"))
        Dim ts = Trim(mycgi.GetCgiValue("ts"))
#If DEBUG Then
        tabelle = "schlagworte"
        aid = CType(151, String)
        colname = "schlagworte"
        neuerwert = "hunger"
        modus = "set"
        '----------- 
        aid = CType(151, String)
        modus = "getstamm"
        iminternet = False
        modus = "biniminternet"
        '---------------
        modus = "getstrassen"
        neuerwert = "438001"
        mycgi.CGI_ServerName = "w2gis02.kreis-of.local"
        '------------------
        modus = "getsql"
        '
        '  modus = "userandinternetinfo"
        nick = "zahnlückenpimpf"
        pw = "kucksdu"
        userinfo = "feinen_j+DC4A3E91649500000000000000E0+KREIS-OF+kucksdu+KROF-000019"

        modus = "getbegleitbplan"
        '  Sql = "SELECT ST_EXTENT(geom) FROM public.schneischen where gid in (400006)"
        'modus = "gettable"
        '#######################################
        'nick=weinachtsmann&modus=setFavorit&gruppe=gruppe&titel=titel&vorhanden=vorhanden&gecheckt=gecheckt&hgrund=hgrund&aktiv=aktiv&ts=ts
        modus = "setfavorit"
        nick = "feinen_j2"

        gruppe = "zuletzt"
        titel = "testtitel"
        vorhanden = ";171;25;26;172;174;173;176;93;64;358;352;"
        gecheckt = ";171;25;26;172;174;173;176;93;64;358;352;"
        hgrund = "253"
        aktiv = "173"
        ts = "22.11.2019+08:54:33"
        '#############################
        modus = "prepbaulast"

        'pdf=di_41&gemarkung=dietzenbach
        '++++++++++++++++++++

#End If
        'If istgueltig(aid) Then
        '    aidstring = " where aid=" & aid & " "
        'Else
        '    aidstring = ""
        'End If
        ' iminternet = sindImmInternet()
        'If mycgi.CGI_ServerName.ToLower = "w2gis02.kreis-of.local" Then
        '    iminternet = False
        'Else
        '    iminternet = True
        'End If
        If modus = "prepbaulast" Then
            l(modus)
            Dim datei = "d:\fkat\baulasten\" & Trim(mycgi.GetCgiValue("gemarkung")) & "\" & Trim(mycgi.GetCgiValue("tiff") & ".tiff")
            l("datei:" & datei)
            'datei = "d:\Paradigmacache\tab462\images\61364.tiff"
            Dim fi As New IO.FileInfo(datei)
            If fi.Exists Then
                l("datei esistiert" & datei)
                Try
                    fi.Delete()
                    l("datei geloescht" & datei)
                    result = "ok"
                Catch ex As Exception
                    l("fehler datei fehler " & ex.ToString)
                    result = "fehler " & ex.ToString
                Finally
                    fi = Nothing
                End Try
            End If
        End If

        If modus = "biniminternet" Then
            l("biniminternet----------------")
            If iminternet Then
                result = "1"
            Else
                result = "0"
            End If
        End If
        l("vor if")
        If modus = "setfavorit" Then

            l("nick " & nick)
            l("gruppe " & gruppe)
            l("titel " & titel)
            l("vorhanden " & vorhanden)
            l("gecheckt  " & gecheckt)
            l("hgrund       " & hgrund)
            l("aktiv   " & aktiv)
            l("ts   " & (mycgi.GetCgiValue("ts")))
            If modDBgrab.favoritExists(nick, gruppe, iminternet) Then
                'update
                modDBgrab.favoritUpdate(nick, gruppe, iminternet, titel, vorhanden, gecheckt, hgrund, aktiv, ts)
            Else
                'insert
                modDBgrab.favoritInsert(nick, gruppe, iminternet, titel, vorhanden, gecheckt, hgrund, aktiv, ts)
            End If
            result = "OK"
        End If
            If modus = "userandinternetinfo" Then
            Dim rites As String = ""
            l("userinfo " & (mycgi.GetCgiValue("userinfo")))
            l("pw " & (mycgi.GetCgiValue("pw")))
            l("nick " & (mycgi.GetCgiValue("nick")))
            l("machinename " & (mycgi.GetCgiValue("machinename")))
            l("domainname  " & (mycgi.GetCgiValue("domainname")))
            l("cpuid       " & (mycgi.GetCgiValue("cpuid")))
            l("macadress   " & (mycgi.GetCgiValue("macadress")))
            If modDBgrab.userexists(mycgi.GetCgiValue("nick"), mycgi.GetCgiValue("pw"), mycgi.GetCgiValue("machinename"),
                                     mycgi.GetCgiValue("domainname"), mycgi.GetCgiValue("cpuid"), mycgi.GetCgiValue("macadress"),
                                    iminternet, rites) Then
                l("user vorhanden")
                result = rites '= baulasten 179...
                l(result)
            Else
                l("user NICHT vorhanden")
                result = "existstiert nicht"
                l(result)
                If modDBgrab.usercreate(mycgi.GetCgiValue("nick"), mycgi.GetCgiValue("pw"), iminternet, mycgi.GetCgiValue("machinename"),
                                         mycgi.GetCgiValue("domainname"), mycgi.GetCgiValue("cpuid"), mycgi.GetCgiValue("macadress")) Then
                    l("user angelegt")
                    result = "0"
                    l(result)
                Else
                    l("user nicht angelegt")
                    result = "-1"
                    l(result)
                End If
            End If
            result = result & "#" '& iminternet
        End If
        If modus = "getstamm" Then
            viewname = "std_stamm"
            l(mycgi.GetCgiValue("aidlist"))

            If iminternet Then
                wherestring = " where  aid in (select aid from gruppe2aid where internet=true)"
            Else
                wherestring = ""
            End If
            result = dbgrabMain(viewname, wherestring, iminternet)
        End If
        If modus = "getstamm2" Then
            l(modus)
            l(mycgi.GetCgiValue("aidlist"))
            viewname = "std_stamm"
            wherestring = "select * from std_stamm where  aid in (" & mycgi.GetCgiValue("aidlist") & ")"
            l("wherestring: " & wherestring)
            result = dbgrabMain2(wherestring)
        End If
        If modus = "getbegleitbplan" Then
            l("MOdus getbegleitbplan")
            Dim pdf = mycgi.GetCgiValue("pdf").Trim.ToLower
            Dim gemarkung = mycgi.GetCgiValue("gemarkung").Trim.ToLower
#If DEBUG Then
            pdf = "di_100" : gemarkung = "dietzenbach"
#End If
            l(pdf & "," & gemarkung)
            result = clsMedia.getbplanbegleit(gemarkung, pdf)
        End If
        If modus = "buildmapfilepdf" Then
            l("MOdus buildmapfilepdf")
            Dim mapfilefullname As String = ""
            Dim pngorpdf = mycgi.GetCgiValue("typ").Trim.ToLower
            Dim hochaufloesend As String = mycgi.GetCgiValue("hires").Trim.ToLower
            Dim layers As String = mycgi.GetCgiValue("layers").Trim.ToLower
            Dim isa4 As Boolean = CBool(mycgi.GetCgiValue("isa4Formatchecked").Trim.ToLower)
            nick = mycgi.GetCgiValue("nick").Trim.ToLower
            l("hoehe" & mycgi.GetCgiValue("hoehe").Trim.ToLower)
            l("breite " & mycgi.GetCgiValue("breite").Trim.ToLower)
            l("isa4Formatchecked " & mycgi.GetCgiValue("isa4Formatchecked").Trim.ToLower)
            l("druckmasstab " & mycgi.GetCgiValue("druckmasstab").Trim.ToLower)
            l("bemerkung " & mycgi.GetCgiValue("bemerkung").Trim.ToLower)
            l("ort " & mycgi.GetCgiValue("ort").Trim.ToLower)
            l("mapext " & mycgi.GetCgiValue("mapext").Trim.ToLower)
            l("layers " & layers)
            l("mitsuchobjekt " & mycgi.GetCgiValue("mitsuchobjekt").Trim.ToLower)
            l("fstname " & mycgi.GetCgiValue("fstname").Trim.ToLower)
            l("fstabstract " & mycgi.GetCgiValue("fstabstract").Trim.ToLower)
            Dim mapfileFST As String = ""
            If mycgi.GetCgiValue("mitsuchobjekt").Trim.ToLower = "1" Then
                l("vor mitsuchobjekt bildeMapfileFST")
                mapfileFST = clsMapfile.bildeMapfileFST(mycgi.GetCgiValue("fstname").Trim.ToLower, mycgi.GetCgiValue("fstabstract").Trim.ToLower)
            End If

            l("vor aufruf")
            mapfilefullname = clsMapfile.buildmapfilepdf(nick, pngorpdf, hochaufloesend, layers, CInt(mycgi.GetCgiValue("hoehe").Trim.ToLower),
                                                CInt(mycgi.GetCgiValue("breite").Trim.ToLower),
                                                isa4, mapfileFST)
            l("mapfilefullname " & mapfilefullname)
            'Threading.Thread.Sleep(400)
            Dim aufrufMapserv = clsMapfile.bildeaufrufMapserverPDF((mycgi.GetCgiValue("breite").Trim.ToLower), (mycgi.GetCgiValue("hoehe").Trim.ToLower),
                                                    mycgi.GetCgiValue("mapext").Trim.ToLower, CDbl(mycgi.GetCgiValue("druckmasstab").Trim.ToLower),
                                                    mycgi.GetCgiValue("bemerkung"),
                                                    mycgi.GetCgiValue("ort").Trim,
                                                    mapfilefullname, isa4)
            aufrufMapserv = Chr(34) & "QUERY_STRING=" & aufrufMapserv & Chr(34)
            Dim mapserverExeString = "d:\inetpub\scripts\mapserv722\mapserv.exe " '& Chr(34) & "QUERY_STRING="
            l("aufrufMapserv " & aufrufMapserv)
            Dim batchfile = "d:\cache\mapfiles\" & nick & clsString.date2string(Now, 5) & "_aufruf.bat"
            Dim outfile = "d:\cache\mapfiles\" & nick & "_out" & clsString.date2string(Now, 5) & ".txt"

            l("batchfile " & batchfile)
            IO.File.WriteAllText(batchfile, mapserverExeString & " " & aufrufMapserv & " > " & outfile & Environment.NewLine)
            l("vor start")
            'Threading.Thread.Sleep(900)
            Dim retval As Integer = 1
            '  retval = Microsoft.VisualBasic.Shell(mapserverExeString & " " & aufrufMapserv & " > d:\cache\mapfiles\out.txt",, True, 9000)
            retval = Microsoft.VisualBasic.Shell(batchfile,, True, 9000)
            Threading.Thread.Sleep(1900)
            l("danach1")
            If retval <> 0 Then
                result = "fehler in cgi dbgrab BUILDMAPFILEPDF"
            Else
                result = IO.File.ReadAllText(outfile)
                dateiLoeschen(outfile)
                dateiLoeschen(batchfile)
            End If
            l("danach")

            ' result = ergebnis.ExitCode.ToString
            l("result " & result)
        End If
        If modus = "getstrassen" Then
            l("MOdus getstrassen")
            l((mycgi.GetCgiValue("w1")))
            result = clsSuche.getstrassen(CStr(mycgi.GetCgiValue("w1")), "postgis20")
        End If
        If modus = "gethausnr" Then
            l("MOdus gethausnr")
            l((mycgi.GetCgiValue("gemeinde")))
            result = clsSuche.gethausnr(CStr(mycgi.GetCgiValue("gemeinde")), CStr(mycgi.GetCgiValue("strcode")), "postgis20")
        End If
        If modus = "getflure" Then
            l("MOdus getflure")
            l((mycgi.GetCgiValue("gemarkung")))
            result = clsSuche.getFlure(CStr(mycgi.GetCgiValue("gemarkung")), CStr(mycgi.GetCgiValue("tabelle")), "postgis20")
        End If
        If modus = "getfst" Then
            l("MOdus getflure")
            l((mycgi.GetCgiValue("gemarkung")))
            result = clsSuche.getFST(CStr(mycgi.GetCgiValue("gemarkung")), CStr(mycgi.GetCgiValue("flur")), CStr(mycgi.GetCgiValue("tabelle")), "postgis20")
        End If
        If modus = "getlegende" Then
            l("MOdus getlegende")
            l((mycgi.GetCgiValue("aid")))
            result = clsLegende.getLegende(CStr(mycgi.GetCgiValue("aid")), "webgiscontrol")
        End If
        If modus = "getschema4aid" Then
            l("MOdus getschema4aid")
            l((mycgi.GetCgiValue("aid")))
            result = clsGetSql.getschema4aid(CStr(mycgi.GetCgiValue("aid")), CStr(mycgi.GetCgiValue("tabnr")), "webgiscontrol")
        End If
        If modus = "getsql" Then
            l("MOdus getsql")
            Dim sql, dbname As String
            'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getsql&sql=Select+distinct+okategorie+from++planung.os_bebauungsplan_f+order+by+okategorie&dbname=postgis20
            l((mycgi.GetCgiValue("sql")))
            l((mycgi.GetCgiValue("dbname")))
            l((mycgi.GetCgiValue("modus")))
            sql = mycgi.GetCgiValue("sql")
            dbname = mycgi.GetCgiValue("dbname")
            'modus = "getsql"
            'Dim Sql = "SELECT ST_EXTENT(geom) FROM public.schneischen where gid in (400006)"
            'result = clsGetSql.getsql(Sql, "postgis20")
#If DEBUG Then
            sql = "select column_name from information_schema.columns where table_schema='planung' and table_name='bebauungsplan_f'"
            dbname = "postgis20"
#End If
            result = clsGetSql.getsql(sql, CStr(dbname))
        End If
        If modus = "putsql" Then
            l("MOdus putsql")
            'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getsql&sql=Select+distinct+okategorie+from++planung.os_bebauungsplan_f+order+by+okategorie&dbname=postgis20
            l((mycgi.GetCgiValue("sql")))
            l((mycgi.GetCgiValue("dbname")))
            l((mycgi.GetCgiValue("modus")))
            If modus = "putsql" Then
                result = modDBgrab.putsql(CStr(mycgi.GetCgiValue("sql")), CStr(mycgi.GetCgiValue("dbname")))
            End If
        End If
        If modus = "gettable" Then
            viewname = Trim(mycgi.GetCgiValue("viewname"))
            If viewname = String.Empty Then viewname = "std_stamm"
            l("finaler viewname=" & viewname)
            'If Not (sachgebietid = String.Empty) And (aid = String.Empty) Then
            '    If Not sachgebietid = "undefined" Then
            '        aidstring = " where sid=" & sachgebietid
            '    End If
            'End If
            Dim sql As String '= "select * from " & viewname
            If mycgi.GetCgiValue("orderby") = String.Empty Then
                sql = "select * from " & viewname
            Else
                sql = "select * from " & viewname & " order by " & mycgi.GetCgiValue("orderby")
            End If
            result = dbgrabsimple(sql, iminternet, "webgiscontrol")
        End If
        If modus = "get" Then
            'If prefix = String.Empty Then prefix = "s_"
            viewname = Trim(mycgi.GetCgiValue("viewname"))
            If viewname = String.Empty Then viewname = "std_stamm"
            'viewname = prefix & viewname
            l("finaler viewname=" & viewname)
            'If Not (sachgebietid = String.Empty) And (aid = String.Empty) Then
            '    If Not sachgebietid = "undefined" Then
            '        aidstring = " where sid=" & sachgebietid
            '    End If
            'End If
            Dim sql As String
            If mycgi.GetCgiValue("orderby") = String.Empty Then
                sql = "select * from " & viewname
            Else
                sql = "select * from " & viewname & " order by " & mycgi.GetCgiValue("orderby")
            End If
            result = dbgrabMain(viewname, sql, iminternet)
        End If
        'If modus = "set" Then
        '    Dim erfolg As Boolean
        '    erfolg = dbsetvalue(tabelle, colname, aid, neuerwert, aidstring)
        '    If erfolg Then
        '        result = "ok"
        '    Else
        '        result = "mist"
        '    End If
        'End If
        'If modus = "setebenezusachgebiet" Then
        '    Dim erfolg As Boolean
        '    '"ebenezusachgebiete","sachgebietid",sid(neuerwert),aid kommen an
        '    l(" 1  ist_Standard= " & ist_Standard)
        '    erfolg = dbsetEbenenZuSachgebieteValue("ebenezusachgebiete", "sachgebietid", aid, neuerwert, aidstring, ist_Standard)
        '    If erfolg Then
        '        result = "ok"
        '    Else
        '        result = "mist"
        '    End If
        'End If
        'If modus = "killebenezusachgebiet" Then
        '    Dim erfolg As Boolean
        '    '"ebenezusachgebiete","sachgebietid",sid(neuerwert),aid kommen an
        '    l(" killebenezusachgebiet  ist_Standard= " & ist_Standard)
        '    erfolg = dbkillEbenenZuSachgebiet("ebenezusachgebiete", "sachgebietid", aid, neuerwert)
        '    If erfolg Then
        '        result = "ok"
        '    Else
        '        result = "mist"
        '    End If
        'End If
        If modus.ToLower = "textsuche" Then
            Dim sql As String
            sql = "select * from std_stamm" &
                " where aid in (" &
                "select aid from std_stamm where lower(titel) Like '%###%' union " &
                "select aid from schlagworte where lower(schlagworte) like '%###%')"
            sql = sql.Replace("###", neuerwert.ToLower)
            l(sql)
            result = dbgrabMain(sql)
        End If

        mycgi.SendHeaderAJAX()
        l("nach SendHeaderAJAX")
        mycgi.Send(result)
        l("ende")
        l(CType(Now, String))
    End Sub

    Private Sub dateiLoeschen(outfile As String)
        Try
            l(" MOD dateiLoeschen anfang")
            Dim fi As New IO.FileInfo(outfile)
            If fi.Exists Then
                fi.Delete()
                fi = Nothing
            End If
            l(" MOD dateiLoeschen ende")
        Catch ex As Exception
            l("Fehler in dateiLoeschen: " & outfile & ex.ToString())
        End Try
    End Sub

    Sub defineDinA4Dina3Formate()
        dina4InMM.w = 297 : dina4InMM.h = 210
        dina3InMM.w = 420 : dina3InMM.h = 297

        dina4InPixel.w = 842 : dina4InPixel.h = 595
        dina3InPixel.w = 1191 : dina3InPixel.h = 842
    End Sub
    'Private Function sindImInternet() As Boolean
    '    Try
    '        l("sindImInternet---------------------- anfang")
    '        Dim stmp As String = CType(System.Environment.GetEnvironmentVariable("MG_iminternet"), String)
    '        If stmp = "0" Then Return False
    '        Return True
    '        l("getBIOMsindImInternet---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in sindImInternet: " & ex.ToString)
    '        Return True
    '    End Try
    'End Function

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



    Private Function istgueltig(aid As String) As Boolean
        If aid = String.Empty Then Return False
        If IsNumeric(aid) Then Return True
        Return False
    End Function

    Private Sub protokoll()
        With My.Application.Log.DefaultFileLogWriter
#If DEBUG Then
            .CustomLocation = "c:\" & "protokoll"
#Else
            .CustomLocation = "d:\websys\" & "protokoll"
#End If
            '.BaseFileName = "dbgrab_" & mycgi.GetCgiValue("user") & "_" & mycgi.GetCgiValue("viewname") & "_" & mycgi.GetCgiValue("modus") & "_" & Format(Now, "yyyyMMddhhmmss")
            .BaseFileName = "dbgrab_" & mycgi.GetCgiValue("user") & "_" & mycgi.GetCgiValue("nick") & "_" & mycgi.GetCgiValue("viewname") & "_" & mycgi.GetCgiValue("modus")

            .AutoFlush = True
            .Append = False
        End With
    End Sub

    Public Sub l(text As String)
        If debug_protokoll Then
            My.Application.Log.WriteEntry(text)
        End If
    End Sub
End Module
