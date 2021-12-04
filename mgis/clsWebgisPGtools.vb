Imports System.Data
Imports mgis

Public Class clsWebgisPGtools
    Private Const SidHintergrund As Integer = 46
    Public Shared Function getAllLayersFromDB(ByRef alle As List(Of clsLayer),
                                         clsActiveDir_fdkurz As String) As List(Of clsLayer)
        l("getAllLayersFromDB                      ")
        l("getAllLayersFromDBclsActiveDir_fdkurz  " & clsActiveDir_fdkurz)

        Dim hinweis As String = ""
        'Dim alle As New List(Of clsLayer)
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
            sql = "select *  from  public.std_stamm where status=true "
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
        Try
            l("saveFavoritDB---------------------- anfang")
            l("  GisUser.username " & GisUser.username)
            l("gruppe " & gruppe)
            If gruppe = "fix" Then
                l("fehler in FavoritDBsave: gruppe = fix")
                Exit Sub
            End If
            '  GisUser.username =   GisUser.username.ToLower.Trim
            uname = uname.ToLower.Trim
            gruppe = gruppe.ToLower.Trim
            Dim istschonvorhanden As Boolean
            Dim altesFavoritObjekt As New clsFavorit
            Dim sql As String = "select * from public.favoriten where username='" & uname.ToLower.Trim & "' and gruppe='" & gruppe.Trim.ToLower & "'"
            Dim dt As DataTable
            dt = getDTFromWebgisDB(sql, "webgiscontrol")
            istschonvorhanden = hatRecords(dt)
            If istschonvorhanden Then
                altesFavoritObjekt = favoritDb2Obj(dt)
                l("istschonvorhanden " & istschonvorhanden)
                If altesFavoritObjekt Is Nothing Then
                    l("altesFavoritObjekt is nothing daher  ende")
                    Exit Sub
                End If
                If altesFavoritObjekt.isSameAs(favoritakt) Then
                    l("saveFavoritDB speichern nicht nötig, da identischer inhalt ende")
                    Exit Sub
                End If
                favoritUpdateDB(gruppe, favoritakt)
            Else
                favoritInsertDB(gruppe, favoritakt)
            End If
            l("saveFavoritDB---------------------- ende")
        Catch ex As Exception
            l("Fehler in saveFavoritDB: " & ex.ToString())
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

    Private Shared Sub favoritUpdateDB(gruppe As String, favoritakt As clsFavorit)
        Try
            l("favoritUpdateDB---------------------- anfang")
            Dim newid As Long
            Dim res As Long
            webgisREC.mydb.SQL = "update  public.favoriten set " &
                                        "  titel ='" & favoritakt.titel & "' " &
                                        ", vorhanden ='" & favoritakt.vorhanden & "'" &
                                        ",  gecheckt ='" & favoritakt.gecheckted & "'" &
                                        ",  hgrund ='" & favoritakt.hgrund & "'" &
                                        ",  aktiv ='" & favoritakt.aktiv & "'" &
                                        ",  ts ='" & DateTime.Now & "'" &
                                        " where lower(username)='" & GisUser.username.ToLower.Trim & "'" &
                                        " and  lower(gruppe)='" & gruppe.ToLower.Trim & "'"
            res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
            l("favoritUpdateDB---------------------- ende")
        Catch ex As Exception
            l("Fehler in favoritUpdateDB: " & ex.ToString())
        End Try
    End Sub

    Private Shared Sub favoritInsertDB(gruppe As String, favoritakt As clsFavorit)
        Try
            l("favoritInsertDB---------------------- anfang")
            Dim newid As Long
            Dim res As Long
            webgisREC.mydb.SQL = "INSERT INTO  public.favoriten " &
                        "(username,gruppe,titel,vorhanden,gecheckt,hgrund,aktiv,ts) " &
                         "VALUES('" &
                           GisUser.username.ToLower.Trim & "','" & gruppe.ToLower.Trim & "','" &
                        favoritakt.titel & "','" & favoritakt.vorhanden & "','" & favoritakt.gecheckted & "','" &
                        favoritakt.hgrund & "','" & favoritakt.aktiv & "','" &
                        DateTime.Now &
            "')  "
            res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
            l(" favoritInsertDB---------------------- ende" & webgisREC.mydb.SQL)
        Catch ex As Exception
            l("Fehler in favoritInsertDB: " & ex.ToString())
        End Try
    End Sub



    Private Shared Function initPRESlayerObjekte(alle As List(Of clsLayer)) As List(Of clsLayerPres)
        Dim pres As New List(Of clsLayerPres)
        pres.Clear()
        Dim presLayer As New clsLayerPres
        Try
            l(" MOD ---------------------- anfang")
            For Each simpleLayer As clsLayer In alle
#If DEBUG Then
                If simpleLayer.aid = 301 Then
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
            l("Fehler in MOD: " & ex.ToString())
            Return Nothing
        End Try
    End Function


    Friend Shared Function setPresetationProps(presLayer As clsLayerPres) As clsLayerPres
        presLayer.isHgrund = False 'ihah
        presLayer.anychange = False
        presLayer.mithaken = False
        presLayer.RBischecked = False
        presLayer = setSichtbarkeitRBaktiveEbene(presLayer)
        presLayer.myFontStyle = FontStyles.Italic
        presLayer.thumbnailFullPath = myglobalz.serverUNC & "nkat\aid\" & presLayer.aid & "\thumbnail\tn.png"
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

    Friend Shared Sub getAllDokusFromDB(allDokus As List(Of clsDoku))
        Dim dt As DataTable
        dt = getDTFromWebgisDB("select *  from  public.doku ", "webgiscontrol")
        allDokus.Clear()
        Dim dok As New clsDoku
        For i = 0 To dt.Rows.Count - 1
            dok = New clsDoku
            dok = initSingleDokuFromDT(dok, i, dt)
            allDokus.Add(dok)
        Next
    End Sub

    Friend Shared Function getAllDokus(iminternet As Boolean) As List(Of clsDoku)
        If iminternet Then
        Else
            clsWebgisPGtools.getAllDokusFromDB(allDokus)
        End If
        Return allDokus
    End Function

    Friend Shared Function getAllLayersPres(iminternet As Boolean,
                                            fdkurz As String,
                                            allLayers As List(Of clsLayer)) As List(Of clsLayerPres)
        Dim pres As New List(Of clsLayerPres)
        If iminternet Then
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
                    layer.SortierKriterium = layer.titel
                Else
                    layer.SortierKriterium = bildeNiceSachgebiet(layer) & "#" & layer.titel
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
        Dim zeilen, spalten As Integer
        Dim result, hinweis As String
        Dim a(), b() As String

        Dim Pres As New List(Of clsLayerPres)
        Try
            l(" MOD getAllLayersFromHttp---------------------- anfang")
            aufruf = "http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=weinachtsmann&modus=getstamm"
            aufruf = myglobalz.buergergisInternetServer & " /cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=weinachtsmann&modus=getstamm"
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            result = result.Trim
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count

            allLayers.Clear()
            Dim layer As New clsLayer
            For i = 0 To zeilen - 1
                layer = New clsLayer
                layer = initSingleLayerFromArray(layer, i, a)
                allLayers.Add(layer)
            Next

            Pres = initPRESlayerObjekte(allLayers)

            Return Pres
            l(" MOD getAllLayersFromHttp---------------------- ende")
        Catch ex As Exception
            l("Fehler in getAllLayersFromHttp: " & ex.ToString())
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
            If layer.aid = 327 Or layer.aid = 326 Or layer.aid = 328 Then
                Debug.Print("")
            End If
#End If
            b = a(i).Split(New Char() {"#"c}, StringSplitOptions.RemoveEmptyEntries)
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

    Shared Sub calcIsHintergrund(allLayersPres As List(Of clsLayerPres))
        For Each clay As clsLayerPres In allLayersPres
            clay.isHgrund = calcIshgrund(clay, allLayersPres)
        Next
    End Sub

    Shared Function calcIshgrund(clay As clsLayerPres, allLayersPres As List(Of clsLayerPres)) As Boolean
        For Each nlay As clsLayerPres In allLayersPres
            If nlay.aid = clay.aid Then
                If nlay.sid = SidHintergrund Then
                    Return True
                End If
            End If
        Next
        Return False
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

    Private Shared Function getDoku4aid(aid As Integer) As clsDoku
        Try
            'l("getDoku4aid---------------------- anfang")
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
            l("Fehler ingetDoku4aid : " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Shared Sub calcOwners(allDokus As List(Of clsDoku))
        For Each ldok As clsDoku In allDokus
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
    End Sub

    Private Shared Function genAllLayers(dt As DataTable) As List(Of clsLayer)
        Dim allLayers As New List(Of clsLayer)
        Dim layer As New clsLayer
        Try
            For i = 0 To dt.Rows.Count - 1
                layer = New clsLayer
                layer = initSingleLayerFromDT(layer, i, dt)
#If DEBUG Then
                If layer.aid = 45 Then
                    Debug.Print("")
                End If
#End If
                allLayers.Add(layer)
            Next
            Return allLayers
        Catch ex As Exception
            l("fehler in genAllLayers: " & ex.ToString)
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
            layer.mit_objekten = CBool(clsDBtools.toBool(dt.Rows(i).Item("mit_objekten")))
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


            'l("initSingleDokuFromDT---------------------- ende")
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
            l("bildeDokuTooltip---------------------- anfang")
            'nlay.dokuText
            Dim ndok As New clsDoku
            ndok = clsWebgisPGtools.getDoku4aid(nlay.aid)
            Dim nurString As String

            nurString = makeTooltipString(ndok)
            nurString = nurString & Environment.NewLine & nlay.schlagworte
            Return nurString
            l("bildeDokuTooltip---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function makeTooltipString(ndok As clsDoku) As String

        Try
            l("makeTooltipString---------------------- anfang")
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

    Friend Shared Sub lastrangeDBsave(uname As String, daRange As clsRange)
        Try
            l("lastrangeDBsave---------------------- anfang")
            l("  GisUser.username " & GisUser.username)
            l("uname  " & uname)
            l("range " & daRange.toString)
            If Not daRange.istBrauchbar Then
                l("fehler in lastrangeDBsave: Not daRange.istBrauchbar")
                Exit Sub
            End If
            '  GisUser.username =   GisUser.username.ToLower.Trim
            uname = uname.ToLower.Trim
            Dim istschonvorhanden As Boolean
            Dim altesLastrangetObjekt As New clsRange
            Dim sql As String = "select * from public.lastrange where trim(lower(username))='" & uname.ToLower.Trim & "' "
            Dim dt As DataTable
            dt = getDTFromWebgisDB(sql, "webgiscontrol")
            'Dim dt As System.Data.DataTable = holeDTfromWebgisControl(sql)
            istschonvorhanden = hatRecords(dt)
            If istschonvorhanden Then
                altesLastrangetObjekt = lastrangeDb2Obj(dt)
                l("istschonvorhanden " & istschonvorhanden)
                If altesLastrangetObjekt Is Nothing Then
                    l("altesFavoritObjekt is nothing daher  ende")
                    Exit Sub
                End If
                If altesLastrangetObjekt.isSameAs(daRange) Then
                    l("saveFavoritDB speichern nicht nötig, da identischer inhalt ende")
                    Exit Sub
                End If
                lastrangeUpdateDB(daRange)
            Else
                lastrangeInsertDB(daRange)
            End If
            l("lastrangeDBsave---------------------- ende")
        Catch ex As Exception
            l("Fehler in lastrangeDBsave: " & ex.ToString())
        End Try
    End Sub

    Private Shared Function lastrangeDb2Obj(dt As DataTable) As clsRange
        Dim favo As New clsRange
        Try
            l("lastrangeDb2Obj---------------------- anfang")
            favo.xl = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("xl")))
            favo.xh = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("xh")))
            favo.yl = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("yl")))
            favo.yh = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("yh")))
            'favo.aktiv = clsDBtools.fieldvalue(dt.Rows(0).Item("aktiv"))
            Return favo
            l("lastrangeDb2Obj---------------------- ende")
        Catch ex As Exception
            l("Fehler in lastrangeDb2Obj: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Private Shared Sub lastrangeUpdateDB(darange As clsRange)
        Try
            l("lastrangeUpdateDB---------------------- anfang")
            Dim newid As Long
            Dim res As Long
            webgisREC.mydb.SQL = "update  public.lastrange set " &
                                        "  xl =" & CInt(darange.xl) & " " &
                                        ", xh =" & CInt(darange.xh) & "" &
                                        ",  yl =" & CInt(darange.yl) & "" &
                                        ",  yh =" & CInt(darange.yh) & "" &
                                        ",  ts ='" & DateTime.Now & "'" &
                                        " where lower(username)='" & GisUser.username.ToLower.Trim & "'"
            res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
            l("lastrangeUpdateDB---------------------- ende")
        Catch ex As Exception
            l("Fehler in lastrangeUpdateDB: " & ex.ToString())
        End Try
    End Sub

    Private Shared Sub lastrangeInsertDB(darange As clsRange)
        Try
            l("lastrangeInsertDB---------------------- anfang")
            Dim newid As Long
            Dim res As Long
            webgisREC.mydb.SQL = "INSERT INTO  public.lastrange " &
                        "(username,xl,xh,yl,yh,ts) " &
                         "VALUES('" &
                           GisUser.username.ToLower.Trim & "'," & CInt(darange.xl) & "," &
                        CInt(darange.xh) & "," & CInt(darange.yl) & "," & CInt(darange.yh) & ",'" &
                        DateTime.Now & "')"
            res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
            l(" lastrangeInsertDB---------------------- ende" & webgisREC.mydb.SQL)
        Catch ex As Exception
            l("Fehler in lastrangeInsertDB: " & ex.ToString())
        End Try
    End Sub

    Friend Shared Function lastrangeLaden(uname As String) As clsRange
        Dim lastrangeaktaltesObjekt As New clsRange
        Dim istschonvorhanden As Boolean
        Dim sql As String
        Try
            l("lastrangeLaden---------------------------")
            sql = "select * from public.lastrange where lower(trim(username))='" & uname.ToLower.Trim & "'  "
            Dim dt As DataTable
            dt = getDTFromWebgisDB(sql, "webgiscontrol")
            'Dim dt As System.Data.DataTable = clsWebgisPGtools.holeDTfromWebgisControl(sql)
            istschonvorhanden = clsWebgisPGtools.hatRecords(dt)

            l("lastrangeLaden istschonvorhanden " & istschonvorhanden)
            If istschonvorhanden Then
                lastrangeaktaltesObjekt = lastrangeDb2Obj(dt)
                l("lastrangeLaden true")
                Return lastrangeaktaltesObjekt
            Else
                l("lastrangeLaden false")
                Return lastrangeaktaltesObjekt
            End If
        Catch ex As Exception
            l("warnung in lastrangeLaden " & "  fehlt.", ex)
            Return Nothing
        End Try
    End Function

End Class
