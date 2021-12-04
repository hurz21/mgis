Imports System.Data
Imports mgis

Public Class clsWebgisPGtools
    Public Shared Function getAllLayersFromDB(ByRef alle As List(Of clsLayer),
                                                    clsActiveDir_fdkurz As String) As List(Of clsLayer)
        l("getAllLayersFromDB                      ")
        l("getAllLayersFromDBclsActiveDir_fdkurz  " & clsActiveDir_fdkurz)
        Dim hinweis As String = ""
        GroupLayerSqlString = composeSqlAllLayers(clsActiveDir_fdkurz)
        Dim dt As DataTable
        dt = getDTFromWebgisDB(GroupLayerSqlString, "webgiscontrol")
        nachricht(hinweis)
        alle = genAllLayers(dt)
        Return alle
    End Function

    Private Shared Function composeSqlAllLayers(clsActiveDir_fdkurz As String) As String
        Dim sql As String
        Dim anhang As String
        Try
            sql = "select *  from  public.std_stamm_2 where status=true "
            Select Case clsActiveDir_fdkurz.ToLower.Trim
                Case "umwelt"
                    anhang = " and  aid in (select aid from gruppe2aid where umwelt=true)"
                Case "gefahrenabwehr- und gesundheitszentrum" 'sicherheit"
                    anhang = " and  aid in (select aid from gruppe2aid where sicherheit=true)"
                Case "bauaufsicht - allgemeine bauvorhaben", "bauaufsicht - besondere bauvorhaben"
                    anhang = " and  aid in (select aid from gruppe2aid where bauaufsicht=true)"
                Case Else
                    anhang = " and  aid in (select aid from gruppe2aid where intranet=true)"
            End Select
            anhang = anhang & " and status=true "
            sql = sql & anhang & " order by titel"
            l("sql" & sql)
            Return sql
        Catch ex As Exception
            l("fehler in MakeSqlAllLayers. schwerer fehler/ Fachdienst unbekannt? ", ex)
            Return ""
        End Try
    End Function

    Friend Shared Sub FavoritDBsave(uname As String, gruppe As String, favoritakt As clsFavorit)
        Dim result As String = "", hinweis As String = "", sql As String
        Dim altesFavoritObjekt As New clsFavorit
        Dim icount As Integer = 0
        Try
            l("saveFavoritDB---------------------- anfang")
            l("  GisUser.nick " & GisUser.nick)
            l("gruppe " & gruppe)
            icount = 1
            If gruppe = "fix" Then
                l("fehler in FavoritDBsave: gruppe = fix")
                Exit Sub
            End If
            icount = 2
            '  GisUser.nick =   GisUser.nick.ToLower.Trim
            uname = uname.ToLower.Trim
            gruppe = gruppe.ToLower.Trim
            'Dim istschonvorhanden As Boolean 
            'Dim dt As DataTable
            'Dim schema As String = If(iminternet, "externparadigma", "public")
            'sql = "select * from " & schema & ".favoriten where username='" & uname.ToLower.Trim & "' and gruppe='" & gruppe.Trim.ToLower & "'"
            'icount = 3
            'result = clsToolsAllg.getSQL4Http(sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
            'icount = 4
            'If result.IsNothingOrEmpty Then
            '    result = ""
            'End If
            'result = result.Trim
            'icount = 11
            If result.IsNothingOrEmpty Then
                strGlobals.FavoriteneintragSchonvorhanden = False
            Else
                strGlobals.FavoriteneintragSchonvorhanden = True
            End If
            icount = 5
            ' result = result.Replace("$", "").Replace(vbCrLf, "")
            'If iminternet Or CGIstattDBzugriff Then
            favoritUpdateHTTP(gruppe, favoritakt, GisUser.nick)
            'If strGlobals.FavoriteneintragSchonvorhanden Then
            '    icount = 6
            '    favoritUpdateHTTP(gruppe, favoritakt)
            '    'favoritUpdate(gruppe, favoritakt)
            'Else
            '    icount = 7
            '    favoTools.favoritInsertHTTP(gruppe, favoritakt)
            'End If
            icount = 8
            'Else
            '    dt = getDTFromWebgisDB(sql, "webgiscontrol")
            '    istschonvorhanden = hatRecords(dt)
            'End If
            'If istschonvorhanden Then
            '    If iminternet Then
            '    Else
            '        altesFavoritObjekt = favoritDb2Obj(dt)
            '    End If
            '    l("istschonvorhanden " & istschonvorhanden)
            '    If altesFavoritObjekt Is Nothing Then
            '        l("altesFavoritObjekt is nothing daher  ende")
            '        Exit Sub
            '    End If
            '    If altesFavoritObjekt.isSameAs(favoritakt) Then
            '        l("saveFavoritDB speichern nicht nötig, da identischer inhalt ende")
            '        Exit Sub
            '    End If
            '    favoritUpdate(gruppe, favoritakt)
            'Else
            '    favoritInsertDB(gruppe, favoritakt)
            'End If
            l("saveFavoritDB---------------------- ende")
        Catch ex As Exception
            l("Fehler in saveFavoritDB: " & uname & "/" & Environment.NewLine &
              gruppe & "//" & Environment.NewLine &
              favoritakt.nachstring(Environment.NewLine) &
             "sql:   " & sql & "////" & Environment.NewLine &
             "result:" & result & "////" & Environment.NewLine &
             "icount:" & icount & "////" & Environment.NewLine &
              ex.ToString())
        End Try
    End Sub
    'Friend Shared Sub FavoritDBsaveOLD(uname As String, gruppe As String, favoritakt As clsFavorit)
    '    Dim result As String = "", hinweis As String = "", sql As String
    '    Dim altesFavoritObjekt As New clsFavorit
    '    Dim icount As Integer = 0
    '    Try
    '        l("saveFavoritDB---------------------- anfang")
    '        l("  GisUser.nick " & GisUser.nick)
    '        l("gruppe " & gruppe)
    '        icount = 1
    '        If gruppe = "fix" Then
    '            l("fehler in FavoritDBsave: gruppe = fix")
    '            Exit Sub
    '        End If
    '        icount = 2
    '        '  GisUser.nick =   GisUser.nick.ToLower.Trim
    '        uname = uname.ToLower.Trim
    '        gruppe = gruppe.ToLower.Trim
    '        'Dim istschonvorhanden As Boolean 
    '        'Dim dt As DataTable
    '        Dim schema As String = If(iminternet, "externparadigma", "public")
    '        sql = "select * from " & schema & ".favoriten where username='" & uname.ToLower.Trim & "' and gruppe='" & gruppe.Trim.ToLower & "'"
    '        icount = 3
    '        result = clsToolsAllg.getSQL4Http(sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
    '        icount = 4
    '        If result.IsNothingOrEmpty Then
    '            result = ""
    '        End If
    '        result = result.Trim
    '        icount = 11
    '        If result.IsNothingOrEmpty Then
    '            strGlobals.FavoriteneintragSchonvorhanden = False
    '        Else
    '            strGlobals.FavoriteneintragSchonvorhanden = True
    '        End If
    '        icount = 5
    '        ' result = result.Replace("$", "").Replace(vbCrLf, "")
    '        'If iminternet Or CGIstattDBzugriff Then
    '        If strGlobals.FavoriteneintragSchonvorhanden Then
    '            icount = 6
    '            favoritUpdateHTTP(gruppe, favoritakt)
    '            'favoritUpdate(gruppe, favoritakt)
    '        Else
    '            icount = 7
    '            favoTools.favoritInsertHTTP(gruppe, favoritakt)
    '        End If
    '        icount = 8
    '        'Else
    '        '    dt = getDTFromWebgisDB(sql, "webgiscontrol")
    '        '    istschonvorhanden = hatRecords(dt)
    '        'End If
    '        'If istschonvorhanden Then
    '        '    If iminternet Then
    '        '    Else
    '        '        altesFavoritObjekt = favoritDb2Obj(dt)
    '        '    End If
    '        '    l("istschonvorhanden " & istschonvorhanden)
    '        '    If altesFavoritObjekt Is Nothing Then
    '        '        l("altesFavoritObjekt is nothing daher  ende")
    '        '        Exit Sub
    '        '    End If
    '        '    If altesFavoritObjekt.isSameAs(favoritakt) Then
    '        '        l("saveFavoritDB speichern nicht nötig, da identischer inhalt ende")
    '        '        Exit Sub
    '        '    End If
    '        '    favoritUpdate(gruppe, favoritakt)
    '        'Else
    '        '    favoritInsertDB(gruppe, favoritakt)
    '        'End If
    '        l("saveFavoritDB---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in saveFavoritDB: " & uname & "/" & Environment.NewLine &
    '          gruppe & "//" & Environment.NewLine &
    '          favoritakt.nachstring(Environment.NewLine) &
    '         "sql:   " & sql & "////" & Environment.NewLine &
    '         "result:" & result & "////" & Environment.NewLine &
    '         "icount:" & icount & "////" & Environment.NewLine &
    '          ex.ToString())
    '    End Try
    'End Sub

    Private Shared Sub favoritUpdateHTTP(gruppe As String, favoritakt As clsFavorit, nick As String)
        Dim SQL As String = ""
        Dim hinweis As String = "", result As String
        Try
            l("favoritUpdateHTTP---------------------- anfang")
            '  'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?nick=weinachtsmann&modus=setFavorit&gruppe=gruppe&titel=titel&vorhanden=vorhanden&gecheckt=gecheckt&hgrund=hgrund&aktiv=aktiv&ts=ts
            Try
                l(" MOD getUserinfo---------------------- anfang" & Environment.NewLine &
                            nick & Environment.NewLine)
                aufruf =
                    myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?pw=1" &
                    "&modus=setFavorit" &
                    "&nick=" & nick &
                    "&gruppe=" & gruppe &
                    "&vorhanden=" & favoritakt.vorhanden &
                    "&gecheckt=" & favoritakt.gecheckted &
                    "&hgrund=" & favoritakt.hgrund &
                    "&aktiv=" & favoritakt.aktiv &
                    "&ts=" & clsString.date2string(Now, 1)
                l(aufruf)
                'If iminternet Then
                '    aufruf = aufruf & "&userinfo=" & userinfo
                'Else

                'End If
                result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
                ' result = "179,18"
                If result.IsNothingOrEmpty Then
                Else
                    result = result.Trim
                    l("result: " & result)
                    result = result.Replace("#", "")
                    nachricht(hinweis)
                    result = result.Replace("$", "").Replace(vbCrLf, "")
                End If

            Catch ex As Exception
                l("Fehler beim favoritUpdateHTTP " & Environment.NewLine &
                  "result:" & result & "<" & Environment.NewLine &
                  aufruf & Environment.NewLine &
                  ex.ToString)
            End Try

        Catch ex As Exception
            l("Fehler in favoritUpdateHTTP: " & SQL & Environment.NewLine & ex.ToString())
        End Try
    End Sub

    Shared Function hatRecords(dt As System.Data.DataTable) As Boolean
        Dim istschonvorhanden As Boolean
        Try
            'l("hatRecords---------------------- anfang")
            If dt.Rows.Count > 0 Then
                istschonvorhanden = True
            Else
                istschonvorhanden = False
            End If
            'l("hatRecords---------------------- ende  " & istschonvorhanden)
            Return istschonvorhanden
        Catch ex As Exception
            l("Fehler in hatRecords: " & ex.ToString())
            Return False
        End Try
    End Function

    'Private Shared Sub favoritUpdate(gruppe As String, favoritakt As clsFavorit)
    '    Try
    '        l("favoritUpdateDB---------------------- anfang")
    '        If iminternet Then
    '            favoTools.favoritSaveUserIni(gruppe, favoritakt)
    '        Else
    '            favoTools.favoritUpdateDB(gruppe, favoritakt)
    '        End If
    '    Catch ex As Exception
    '        l("Fehler in favoritUpdateDB: " & ex.ToString())
    '    End Try
    'End Sub

    'Private Shared Sub favoritUpdateDB(gruppe As String, favoritakt As clsFavorit)
    '    Dim newid As Long
    '    Dim res As Long
    '    webgisREC.mydb.SQL = "update  public.favoriten set " &
    '                                "  titel ='" & favoritakt.titel & "' " &
    '                                ", vorhanden ='" & favoritakt.vorhanden & "'" &
    '                                ",  gecheckt ='" & favoritakt.gecheckted & "'" &
    '                                ",  hgrund ='" & favoritakt.hgrund & "'" &
    '                                ",  aktiv ='" & favoritakt.aktiv & "'" &
    '                                ",  ts ='" & DateTime.Now & "'" &
    '                                " where lower(username)='" & GisUser.nick.ToLower.Trim & "'" &
    '                                " and  lower(gruppe)='" & gruppe.ToLower.Trim & "'"
    '    res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
    '    l("favoritUpdateDB---------------------- ende")
    'End Sub

    'Private Shared Sub favoritInsertDB(gruppe As String, favoritakt As clsFavorit)
    '    Try
    '        l("favoritInsertDB---------------------- anfang")
    '        Dim newid As Long
    '        Dim res As Long
    '        If iminternet Then
    '            favoritSaveUserIni(gruppe, favoritakt)
    '        Else
    '            FavoritInsertDB(gruppe, favoritakt, newid, res)
    '            l(" favoritInsertDB---------------------- ende" & webgisREC.mydb.SQL)
    '        End If
    '    Catch ex As Exception
    '        l("Fehler in favoritInsertkDB: " & ex.ToString())
    '    End Try
    'End Sub


    'Private Shared Sub FavoritInsertDB(gruppe As String, favoritakt As clsFavorit, ByRef newid As Long, ByRef res As Long)
    '    webgisREC.mydb.SQL = "INSERT INTO  public.favoriten " &
    '                                        "(username,gruppe,titel,vorhanden,gecheckt,hgrund,aktiv,ts) " &
    '                                        "VALUES('" &
    '                                        GisUser.nick.ToLower.Trim & "','" & gruppe.ToLower.Trim & "','" &
    '                                        favoritakt.titel & "','" & favoritakt.vorhanden & "','" & favoritakt.gecheckted & "','" &
    '                                        favoritakt.hgrund & "','" & favoritakt.aktiv & "','" &
    '                                        DateTime.Now & "')  "
    '    res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
    'End Sub 
    Private Shared Function initPRESlayerObjekte(alle As List(Of clsLayer)) As List(Of clsLayerPres)
        Dim pres As New List(Of clsLayerPres)
        pres.Clear()
        Dim presLayer As New clsLayerPres
        Try
            l(" MOD ---------------------- anfang")
            For Each simpleLayer As clsLayer In alle
#If DEBUG Then
                If simpleLayer.aid = 1 Then
                    Debug.Print("")
                End If
#End If
                presLayer = New clsLayerPres
                presLayer = presLayer.convLayer2PresLayer(simpleLayer)
                presLayer = setPresetationProps(presLayer)
                presLayer = setSichtbarkeitRBaktiveEbene(presLayer)
#If DEBUG Then
                If presLayer.mit_objekten Then
                    Debug.Print("")
                End If
#End If
                pres.Add(presLayer)
            Next
            l(" MOD ---------------------- ende")
            Return pres
        Catch ex As Exception
            l("Fehler in initPRESlayerObjekte: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Friend Shared Function setPresetationProps(presLayer As clsLayerPres) As clsLayerPres
        presLayer.isHgrund = False
        presLayer.anychange = False
        presLayer.mithaken = False
        presLayer.RBischecked = False
        presLayer = setSichtbarkeitRBaktiveEbene(presLayer)
        presLayer.myFontStyle = FontStyles.Italic
        'presLayer.thumbnailFullPath = myglobalz.serverUNC & "nkat\aid\" & presLayer.aid & "\thumbnail\tn.png"
        Return presLayer
    End Function

    Shared Function setSichtbarkeitRBaktiveEbene(presLayer As clsLayerPres) As clsLayerPres
        If presLayer.mit_imap Then
            presLayer.RBsichtbarkeit = Visibility.Visible
        Else
            presLayer.RBsichtbarkeit = Visibility.Hidden
        End If
        Return presLayer
    End Function

    Friend Shared Function getAllDokusFromDB() As List(Of clsDoku)
        Dim dt As DataTable
        Dim neulist As New List(Of clsDoku)
        dt = getDTFromWebgisDB("select * from public.doku ", "webgiscontrol")

        Dim dok As New clsDoku
        For i = 0 To dt.Rows.Count - 1
            dok = New clsDoku
            dok = initSingleDokuFromDT(dok, i, dt)
            neulist.Add(dok)
        Next
        Return neulist
    End Function

    Friend Shared Function getAllDokus(iminternet As Boolean) As List(Of clsDoku)
        If iminternet Or myglobalz.CGIstattDBzugriff Then
            allDokus = clsWebgisPGtools.getAllDokusFromHTTP()
        Else
            allDokus = clsWebgisPGtools.getAllDokusFromDB()
        End If
        Return allDokus
    End Function

    Private Shared Function getAllDokusFromHTTP() As List(Of clsDoku)
        Dim zeilen, spalten As Integer
        Dim result As String = "", hinweis As String = ""
        Dim a(), b() As String
        Dim tdokuliste As New List(Of clsDoku)
        Try
            l(" MOD getAllDokusFromHTTP---------------------- anfang")
            'aufruf = "http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick & "&modus=gettable&viewname=ref_doku"
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick & "&modus=gettable&viewname=doku4stamm"
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            result = result.Trim
            If result.IsNothingOrEmpty Then
                Return tdokuliste
            Else
                a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
                b = a(0).Split("#"c) : spalten = b.Count
                Dim layer As New clsDoku
                For i = 0 To zeilen - 1
                    layer = New clsDoku
                    layer = initSingleDokuFromArray(layer, i, a)
                    tdokuliste.Add(layer)
                Next
                Return tdokuliste
            End If

            l(" MOD getAllDokusFromHTTP---------------------- ende")
        Catch ex As Exception
            l("Fehler in getAllDokusFromHTTP: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Shared Function getAllLayersPres(iminternet As Boolean,
                                            fdkurz As String,
                                            allLayers As List(Of clsLayer)) As List(Of clsLayerPres)
        Dim pres As New List(Of clsLayerPres)
        If iminternet Or CGIstattDBzugriff Then
            pres = clsWebgisPGtools.getAllLayersFromHttp(allLayers, clsActiveDir.fdkurz)
        Else
            allLayers = clsWebgisPGtools.getAllLayersFromDB(allLayers, clsActiveDir.fdkurz)
            pres = initPRESlayerObjekte(allLayers)
        End If
        Return pres
    End Function

    Friend Shared Sub calcEtikett_kategorie_tultipp(allLayersPres As List(Of clsLayerPres), explorerAlphabetisch As Boolean)
        Try
            For Each layer As clsLayerPres In allLayersPres
#If DEBUG Then
                If layer.aid = 261 Then
                    Debug.Print("")
                End If
#End If
                layer.kategorie = bildeNiceSachgebiet(layer).ToLower
                If explorerAlphabetisch Then
                    layer.Etikett = layer.titel
                Else
                    layer.Etikett = bildeNiceSachgebiet(layer) & "#" & layer.titel
                End If

                If layer.ldoku.calcedOwner.IsNothingOrEmpty Then

                    layer.tultipp = layer.titel & Environment.NewLine &
                                    clsString.Capitalize(layer.standardsachgebiet.Replace("h_", "")) & Environment.NewLine &
                                     layer.ldoku.aktualitaet & Environment.NewLine &
                                    "(Klicken für Legende, Objektsuche und Dokumentation)"
                Else
                    'layer.etikett = bildeNiceSachgebiet(layer) & "#" & layer.titel & " (" & layer.ldoku.calcedOwner & ") "
                    layer.tultipp = layer.titel & " (" & layer.ldoku.calcedOwner & ") " & Environment.NewLine &
                                  clsString.Capitalize(layer.standardsachgebiet.Replace("h_", "")) & Environment.NewLine &
                                     layer.ldoku.aktualitaet & Environment.NewLine &
                                    "(Klicken für Legende, Objektsuche und Dokumentation)"
                End If
            Next
        Catch ex As Exception
            l("fehler in calcEtikett ", ex)
        End Try
    End Sub

    Friend Shared Function getAllLayersFromHttp(allLayers As List(Of clsLayer), fdkurz As String) As List(Of clsLayerPres)

        Dim result As String = "", resultextra As String = "", hinweis As String = ""
        Dim extralayers As New List(Of clsLayer)
        Dim Pres As New List(Of clsLayerPres)
        Try
            l(" MOD getAllLayersFromHttp---------------------- anfang")
            'aufruf = "http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick & "&modus=getstamm"
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick & "&modus=getstamm"
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            result = result.Trim
            allLayers = getalllayersForResult(result)

            If GisUser.rites.Trim.IsNothingOrEmpty Or GisUser.rites.Trim = "0" Then
                l("getAllLayersFromHttpb " & GisUser.rites)
            Else
                aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick & "&modus=getstamm2&aidlist=" & GisUser.rites
                l("aufruf " & aufruf)
                resultextra = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
                l(resultextra)
                nachricht(hinweis)
                resultextra = resultextra.Trim
                extralayers = getalllayersForResult(resultextra)
                For Each lay As clsLayer In extralayers
                    allLayers.Add(lay)
                Next
                extralayers = Nothing
            End If

            Pres = initPRESlayerObjekte(allLayers)
            Return Pres
            l(" MOD getAllLayersFromHttp---------------------- ende")
        Catch ex As Exception
            l("Fehler in getAllLayersFromHttp: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Shared Function getalllayersForResult(result As String) As List(Of clsLayer)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim newlist As New List(Of clsLayer)
        Dim layer As New clsLayer
        Try
            l(" MOD getalllayersForResult anfang")
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            newlist.Clear()
            'If Not GisUser.rites.IsNothingOrEmpty Then
            '    layer = initSingleLayerFromArray(layer, i, a)
            '    allLayers.Add(layer)
            'End If
            For i = 0 To zeilen - 1
                layer = New clsLayer
                layer = initSingleLayerFromArray(layer, i, a)
                newlist.Add(layer)
            Next
            l(" MOD getalllayersForResult ende")
            Return newlist
        Catch ex As Exception
            l("Fehler in getalllayersForResult: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Shared Function initSingleDokuFromArray(tdoku As clsDoku, i As Integer, a() As String) As clsDoku
        Dim b() As String
        Try
            'b = a(i).Split(New Char() {"#"c}, StringSplitOptions.RemoveEmptyEntries) 
            b = a(i).Split("#"c)
            tdoku.aid = CInt(b(1)) 'clsDBtools.fieldvalue(dt.Rows(i).Item("aid")))

            tdoku.inhalt = CStr(b(2)) ' CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("sid")))
            tdoku.entstehung = CStr(b(3)) '(clsDBtools.fieldvalue(dt.Rows(i).Item("ebene")))
            tdoku.aktualitaet = CStr(b(4)) '(clsDBtools.fieldvalue(dt.Rows(i).Item("titel")))
            tdoku.massstab = CStr(b(5)) 'CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("rang")))

            tdoku.beschraenkungen = CStr(b(6)) 'CBool(clsDBtools.toBool(dt.Rows(i).Item("mit_imap")))
            tdoku.datenabgabe = CStr(b(7)) '(clsDBtools.fieldvalue(dt.Rows(i).Item("masstab_imap")))

            '#If DEBUG Then
            '            If tdoku.mit_objekten Then
            '                Debug.Print("")
            '            End If
            '#End If
            '            tdoku.mapFile = tdoku.calcMapfileFullname("layer")
            '            tdoku.mapFileHeader = tdoku.calcMapfileFullname("header")
            '            tdoku.suchfeld = (tdoku.titel.Trim & " " & tdoku.schlagworte.Trim).ToLower
            Return tdoku
        Catch ex As Exception
            l("fehler in initSingleLayerFromDT: ", ex)
            Return Nothing
        End Try
    End Function
    Private Shared Function initSingleLayerFromArray(layer As clsLayer, i As Integer, a() As String) As clsLayer
        Dim b() As String
        Try
#If DEBUG Then
            If i = 253 Then
                Debug.Print("")
            End If
            If layer.aid = 1 Or layer.aid = 1 Or layer.aid = 1 Then
                Debug.Print("")
            End If
#End If
            'b = a(i).Split(New Char() {"#"c}, StringSplitOptions.RemoveEmptyEntries)
            b = a(i).Split("#"c)
            layer.aid = CInt(b(0)) 'clsDBtools.fieldvalue(dt.Rows(i).Item("aid")))
            layer.sid = CInt(b(11)) ' CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("sid")))
            layer.ebene = CStr(b(2)) '(clsDBtools.fieldvalue(dt.Rows(i).Item("ebene")))
            layer.titel = CStr(b(3)) '(clsDBtools.fieldvalue(dt.Rows(i).Item("titel")))
            layer.rang = CInt(b(4)) 'CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("rang")))
            layer.mit_imap = CBool(b(5)) 'CBool(clsDBtools.toBool(dt.Rows(i).Item("mit_imap")))
            layer.masstab_imap = CStr(b(6)) '(clsDBtools.fieldvalue(dt.Rows(i).Item("masstab_imap")))
            layer.mit_legende = CBool(b(8)) ' CBool(clsDBtools.toBool(dt.Rows(i).Item("mit_legende")))
            layer.status = CBool(b(12)) ' CBool(clsDBtools.toBool(dt.Rows(i).Item("status")))
            layer.schlagworte = CStr(b(13)) ' (clsDBtools.fieldvalue(dt.Rows(i).Item("schlagworte")))

            layer.pfad = CStr(b(10)) '(clsDBtools.fieldvalue(dt.Rows(i).Item("pfad")))
            'layer.schema = (clsDBtools.fieldvalue(dt.Rows(i).Item("schema")))
            'layer.schlagworte = (clsDBtools.fieldvalue(dt.Rows(i).Item("schlagworte")))
            layer.standardsachgebiet = CStr(b(1)) ' (clsDBtools.fieldvalue(dt.Rows(i).Item("sachgebiet")))
            layer.mit_objekten = CBool(b(7)) ' CBool(clsDBtools.toBool(dt.Rows(i).Item("mit_objekten")))
            If b(14) = "m" Then
                layer.iswms = True
            End If
#If DEBUG Then
            If layer.mit_objekten Then
                Debug.Print("")
            End If
#End If
            layer.mapFile = layer.calcMapfileFullname("layer")
            layer.mapFileHeader = layer.calcMapfileFullname("header")
            layer.suchfeld = (layer.titel.Trim & " " & layer.schlagworte.Trim).ToLower

            Return layer
        Catch ex As Exception
            l("fehler in initSingleLayerFromDT: ", ex)
            Return Nothing
        End Try
    End Function

    Shared Function bildeNiceSachgebiet(layer As clsLayerPres) As String
        Dim temp As String
        Try
            temp = clsString.Capitalize(clsString.kuerzeTextauf(layer.standardsachgebiet, 10))
            '  temp = "[" & temp & "]".ToUpper
            'If temp.Contains("sichtb") Then
            '    Debug.Print("")
            '    temp = "_PARADIGMA"
            'End If
            'If temp.Contains("ochw") Then
            '    Debug.Print("")
            '    '  temp = "_PARADIGMA"
            'End If
            If temp.ToLower = "unsichtbar" Then temp = "_PARADIGMA"
            If temp.ToLower = "foerder" Then temp = "FÖRDER"
            If temp.ToLower = "denkmalsc" Then temp = "DENKMAL"
            If temp.ToLower = "h_verschi" Then temp = "VERSCH."
            If temp.ToLower = "h_landsch" Then temp = "HIST.LAND"
            If temp.ToLower = "h_luftbild" Then temp = "HIST.LUBI"
            If temp.ToLower = "h_regiona" Then temp = "HIST.REGIO"
            If temp.ToLower = "h_topkarte" Then temp = "HIST.TK"
            If temp.ToLower = "hochwasse" Then temp = "HOCHW."
            temp = temp.ToUpper
            temp = "[" & temp & "]"
            Return temp
        Catch ex As Exception
            l("fehler in bildeNiceSachgebiet: ", ex)
            Return " "
        End Try
    End Function


    Friend Shared Sub dombineLayerDoku(lliste As List(Of clsLayerPres), allDokus As List(Of clsDoku))
        Try
            l("dombineLayerDoku---------------------- anfang")
            For Each layer As clsLayerPres In lliste
                layer.ldoku = getDoku4aid(layer.aid)
            Next
            l("dombineLayerDoku---------------------- ende")
        Catch ex As Exception
            l("Fehler in dombineLayerDoku: " & ex.ToString())
        End Try
    End Sub

    Shared Function getDoku4aid(aid As Integer) As clsDoku
        Try
            'l("getDoku4aid---------------------- anfang")
            If aid = Nothing Then Return Nothing
            If aid < 1 Then Return Nothing
            For Each ldok As clsDoku In allDokus
                If ldok.aid = aid Then
                    Return ldok
                End If
            Next
            Dim dok As New clsDoku
            '    l("getDoku4aid---------------------- ende")
            Return dok
        Catch ex As Exception
            l("Fehler ingetDoku4aid : aid: " & aid & Environment.NewLine & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Shared Sub calcOwners(allDokus As List(Of clsDoku))
        Try
            For Each ldok As clsDoku In allDokus
                If ldok Is Nothing Then
                    Debug.Print("")
                    Continue For
                End If
                If ldok.datenabgabe Is Nothing Then
                    Debug.Print("")
                    Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim = String.Empty Then Continue For
                If ldok.datenabgabe.ToLower.Trim.Contains("hochtaunuskreis") Then
                    ldok.calcedOwner = "Hochtaunuskreis" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("land hessen") Then
                    ldok.calcedOwner = "Hessen" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("hvbg") Then
                    ldok.calcedOwner = "HVBG" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("kreis offenbach") Then
                    ldok.calcedOwner = "Kreis Offenbach" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("www.region-frankfurt.de") Then
                    ldok.calcedOwner = "RegVerbFRM" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("hlug") Then
                    ldok.calcedOwner = "HLUG" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("hessen-forst") Then
                    ldok.calcedOwner = "Hessen-Forst" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("wirtschaft.hessen.det") Then
                    ldok.calcedOwner = "HMWVL" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("rpda.de") Then
                    ldok.calcedOwner = "RegPräs.Darmstadt" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("langen.de") Then
                    ldok.calcedOwner = "Stadt Langen" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("bfn.de") Then
                    ldok.calcedOwner = "BfN" : Continue For
                End If
                If ldok.datenabgabe.ToLower.Trim.Contains("kvg-offenbach.de") Then
                    ldok.calcedOwner = "KVG-Offenbach" : Continue For
                End If
                'If ldok.datenabgabe.ToLower.Trim.Contains("kvg-offenbach.de") Then
                '    ldok.calcedOwner = "kvg-offenbach" : Continue For
                'End If
                'If ldok.datenabgabe.ToLower.Trim.Contains("kvg-offenbach.de") Then
                '    ldok.calcedOwner = "kvg-offenbach" : Continue For
                'End If
                'If ldok.datenabgabe.ToLower.Trim.Contains("kvg-offenbach.de") Then
                '    ldok.calcedOwner = "kvg-offenbach" : Continue For
                ' End If
            Next


        Catch ex As Exception
            l("fehler in calcOwners: ", ex)
        End Try
    End Sub

    Private Shared Function genAllLayers(dt As DataTable) As List(Of clsLayer)
        Dim allLayers As New List(Of clsLayer)
        Dim layer As New clsLayer
        Try
            For i = 0 To dt.Rows.Count - 1
                layer = New clsLayer
                layer = initSingleLayerFromDT(layer, i, dt)
                If layer.aid < 1 Then Continue For
#If DEBUG Then
                If layer.aid = 45 Then
                    Debug.Print("")
                End If
#End If
                allLayers.Add(layer)
            Next
            Return allLayers
        Catch ex As Exception
            l("fehler in genAllLayers: ", ex)
            Return Nothing
        End Try
    End Function


    Private Shared Function initSingleLayerFromDT(layer As clsLayer, i As Integer, dt As DataTable) As clsLayer
        Try
#If DEBUG Then
            If i = 253 Then
                Debug.Print("")
            End If
            If layer.aid = 327 Or layer.aid = 326 Or layer.aid = 328 Then
                Debug.Print("")
            End If
#End If
            layer.aid = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("aid")))
            layer.sid = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("sid")))
            layer.ebene = (clsDBtools.fieldvalue(dt.Rows(i).Item("ebene")))
            layer.titel = (clsDBtools.fieldvalue(dt.Rows(i).Item("titel")))
            layer.rang = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("rang")))
            layer.mit_imap = CBool(clsDBtools.toBool(dt.Rows(i).Item("mit_imap")))
            layer.masstab_imap = (clsDBtools.fieldvalue(dt.Rows(i).Item("masstab_imap")))
            layer.mit_legende = CBool(clsDBtools.toBool(dt.Rows(i).Item("mit_legende")))
            layer.status = CBool(clsDBtools.toBool(dt.Rows(i).Item("status")))
            layer.schlagworte = (clsDBtools.fieldvalue(dt.Rows(i).Item("schlagworte")))

            layer.pfad = (clsDBtools.fieldvalue(dt.Rows(i).Item("pfad")))
            'layer.schema = (clsDBtools.fieldvalue(dt.Rows(i).Item("schema")))
            'layer.schlagworte = (clsDBtools.fieldvalue(dt.Rows(i).Item("schlagworte")))
            layer.standardsachgebiet = (clsDBtools.fieldvalue(dt.Rows(i).Item("sachgebiet")))
            layer.kategorieLangtext = (clsDBtools.fieldvalue(dt.Rows(i).Item("langtext")))
            layer.kategorieToolTip = (clsDBtools.fieldvalue(dt.Rows(i).Item("tooltip")))
            layer.mit_objekten = CBool(clsDBtools.toBool(dt.Rows(i).Item("mit_objekten")))
            If clsDBtools.fieldvalue(dt.Rows(i).Item("service")) = "m" Then
                layer.iswms = True
            End If
#If DEBUG Then
            If layer.mit_objekten Then
                Debug.Print("")
            End If
#End If
            layer.mapFile = layer.calcMapfileFullname("layer")
            layer.mapFileHeader = layer.calcMapfileFullname("header")
            layer.suchfeld = (layer.titel.Trim & " " & layer.schlagworte.Trim).ToLower
            Return layer
        Catch ex As Exception
            l("fehler in initSingleLayerFromDT: ", ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function initSingleDokuFromDT(ldok As clsDoku, i As Integer, dt As DataTable) As clsDoku
        Try
            'l("initSingleDokuFromDT---------------------- anfang")
            ldok.aid = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("aid")))
            ldok.aktualitaet = (clsDBtools.fieldvalue(dt.Rows(i).Item("aktualitaet")))
            ldok.beschraenkungen = (clsDBtools.fieldvalue(dt.Rows(i).Item("beschraenkungen")))
            ldok.datenabgabe = (clsDBtools.fieldvalue(dt.Rows(i).Item("datenabgabe")))
            ldok.entstehung = (clsDBtools.fieldvalue(dt.Rows(i).Item("entstehung")))
            ldok.inhalt = (clsDBtools.fieldvalue(dt.Rows(i).Item("inhalt")))
            ldok.massstab = (clsDBtools.fieldvalue(dt.Rows(i).Item("masstab")))
        Catch ex As Exception
            l("Fehler in initSingleDokuFromDT: " & ex.ToString())
            Return Nothing
        End Try
        Return ldok
    End Function

    Friend Shared Sub getOSliste(allLayerspres As List(Of clsLayerPres), filter As String)
        allOSLayers.Clear()
        filter = filter.ToLower
        Dim presLayer As New clsLayerPres
        Try
            l("getOSliste---------------------- anfang")
            For Each simpleLayer As clsLayer In allLayerspres
#If DEBUG Then
                If simpleLayer.aid = 327 Then
                    Debug.Print("")
                End If
#End If

                presLayer = New clsLayerPres
                If simpleLayer.mit_objekten Then
                    If filter.IsNothingOrEmpty Then
                        presLayer.titel = simpleLayer.titel
                        presLayer.aid = simpleLayer.aid
                        allOSLayers.Add(presLayer)
                    Else
                        If simpleLayer.titel.ToLower.Contains(filter) Then
                            presLayer.titel = simpleLayer.titel
                            presLayer.aid = simpleLayer.aid
                            allOSLayers.Add(presLayer)
                        End If
                    End If
                End If
            Next
            l("getOSliste---------------------- ende")
        Catch ex As Exception
            l("Fehler in getOSliste: " & ex.ToString())

        End Try
    End Sub

    Friend Shared Function bildeDokuTooltip(nlay As clsLayerPres) As String
        Try
            'l("bildeDokuTooltip---------------------- anfang")
            'nlay.dokuText
            Dim ndok As New clsDoku
            ndok = clsWebgisPGtools.getDoku4aid(nlay.aid)
            Dim nurString As String

            nurString = makeTooltipString(ndok)
            nurString = nurString & Environment.NewLine & nlay.schlagworte
            Return nurString
            'l("bildeDokuTooltip---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function makeTooltipString(ndok As clsDoku) As String

        Try
            'l("makeTooltipString---------------------- anfang")
            If ndok Is Nothing Then
                l("makeTooltipString- ndok Is Nothing return leer")
                Return ""
            End If
            Dim sb As New Text.StringBuilder
            sb.Append(nsMakeRTF.rtf.htm2cr(ndok.inhalt) & Environment.NewLine)
            sb.Append(nsMakeRTF.rtf.htm2cr(ndok.entstehung) & Environment.NewLine)
            sb.Append("Aktuell: " & nsMakeRTF.rtf.htm2cr(ndok.aktualitaet) & Environment.NewLine)
            sb.Append(nsMakeRTF.rtf.htm2cr(ndok.beschraenkungen) & Environment.NewLine)
            sb.Append(nsMakeRTF.rtf.htm2cr(ndok.datenabgabe) & Environment.NewLine)
            Return sb.ToString
            l("makeTooltipString---------------------- ende")
        Catch ex As Exception
            l("Fehler in makeTooltipString: " & ex.ToString())
            Return ""
        End Try
    End Function


End Class
