Imports System.Data
Imports System.IO

Public Class clsSachdatentools
    Shared Function pdfDateinameISOk(dateiname As String) As Boolean
        If dateiname.IsNothingOrEmpty OrElse dateiname.ToLower.StartsWith("dummy") Then
            Return False
        Else
            Return True
        End If
    End Function

    Shared Function getGID4OS_tabelle(os_tabelledef As clsTabellenDef) As String
        Dim tabelle As String = "", hinweis As String = ""
        Dim sql As String = "select gid from " & os_tabelledef.Schema & "." & os_tabelledef.tabelle & " where " & os_tabelledef.linkTabs & "=" & os_tabelledef.gid
        Dim dt As DataTable
        Try
            l("getGID4OS_tabelle---------------------- anfang")
            l(sql)
            If iminternet Or CGIstattDBzugriff Then
                sql = clsToolsAllg.getSQL4Http(sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
                tabelle = sql.Replace("$", "").Replace(vbCrLf, "").Trim
            Else
                dt = getDTFromWebgisDB(sql, "webgiscontrol")
                tabelle = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            End If
            Return tabelle
            l("getGID4OS_tabelle---------------------- ende")
        Catch ex As Exception
            l("Fehler in getGID4OS_tabelle: " & sql & Environment.NewLine, ex)
            Return "-1"
        End Try
    End Function
    Shared Function getOneValSQL(dbname As String, sql As String) As String
        'Dim hinweis As String
        Dim tabelle As String = "", hinweis As String = ""
        Try
            l("getTabname4tabnr---------------------- anfang")
            If iminternet Or CGIstattDBzugriff Then
                sql = clsToolsAllg.getSQL4Http(sql, dbname, hinweis, "getsql") : l(hinweis)
                tabelle = sql.Replace("$", "").Replace(vbCrLf, "").Trim
                Return tabelle
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(sql, dbname)
                tabelle = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
                dt = Nothing
                Return tabelle
            End If

            l("getTabname4tabnr---------------------- ende")
        Catch ex As Exception
            l("Fehler in getTabname4tabnr: " & hinweis & iminternet & dbname & ", " & sql, ex)
            Return "-1"
        End Try
    End Function
    Shared Function getTabname4tabnr(aid As Integer, tabnr As String) As String
        'Dim hinweis As String
        Dim tabelle As String = "", hinweis As String = ""
        Dim sql = "select tabelle from public.attributtabellen where aid=" & aid & " and tab_nr=" & tabnr
        Try
            l("getTabname4tabnr---------------------- anfang")
            If iminternet Or CGIstattDBzugriff Then
                sql = clsToolsAllg.getSQL4Http(sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
                tabelle = sql.Replace("$", "").Replace(vbCrLf, "").Trim
                Return tabelle
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(sql, "webgiscontrol")
                tabelle = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
                dt = Nothing
                Return tabelle
            End If

            l("getTabname4tabnr---------------------- ende")
        Catch ex As Exception
            l("Fehler in getTabname4tabnr: ", ex)
            Return "-1"
        End Try
    End Function

    Shared Function getTabnr4Tabname(schema As String, tabelle As String) As String
        Dim tabnr As String = "", sql As String = "", hinweis As String = ""
        Try
            l("getTabnr4Tabname---------------------- anfang")
            sql = "select tab_nr from public.attributtabellen where schema='" & schema & "' and tabelle='" & tabelle & "'"
            If iminternet Or CGIstattDBzugriff Then
                sql = clsToolsAllg.getSQL4Http(sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
                tabnr = sql.Replace("$", "").Replace(vbCrLf, "").Trim
                Return tabnr
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(sql, "webgiscontrol")
                tabnr = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
                dt = Nothing
                Return tabnr
            End If
            l("getTabnr4Tabname---------------------- ende")
        Catch ex As Exception
            l("warnung in getTabnr4Tabname: ", ex)
            Return "1"
        End Try
    End Function

    Shared Function splitDBinfo(fl As String) As Boolean
        Dim fulllink As String
        Dim a() As String
        Try
            l("splitDBinfo---------------------- anfang")
            fulllink = fl
            a = fulllink.Split(","c)
            os_tabelledef.Schema = a(0)
            os_tabelledef.tabelle = a(1)
            os_tabelledef.gid = a(2)

            Return True
            l("splitDBinfo---------------------- ende")
        Catch ex As Exception
            l("Fehler in : splitDBinfo ", ex)
            Return False
        End Try
    End Function
    Shared Function getDownloadtargetBaulasten(url As String, zielroot As String, ByRef zieldir As String, ByRef filename As String) As Boolean
        Dim target As String = ""
        Dim a() As String
        Try
            l(" MOD getDownloadtargetBaulasten anfang")
            'If iminternet Then

            If url.ToLower.Contains("fkatbig") Then
                target = url.Replace(myglobalz.serverWeb.ToLower & "/fkatbig/baulasten/", "")

                zieldir = zielroot
                filename = target
            Else
                target = url.Replace(myglobalz.serverWeb.ToLower & "/fkat/baulasten/", "").Replace(myglobalz.serverWeb.ToLower & "/fkat/baulasten/", "")
                a = target.Split("/"c)
                zieldir = zielroot & "\" & a(0) '& "\" & a(1) 
                filename = a(1)
            End If
            '    target = url.Replace(myglobalz.serverUNC.ToLower & "\fkat\baulasten\", "")
            '    a = target.Split("\"c)
            '    zieldir = zielroot & "\" & a(0) '& "\" & a(1)
            '    filename = a(1)
            'End If

            'target = target.Replace("/", "\")
            l(" MOD getDownloadtargetBaulasten ende")
            Return True
        Catch ex As Exception
            l("Fehler in getDownloadtargetBaulasten: ", ex)
            Return False
        End Try
    End Function

    Shared Function makeLokalBplaneDatei(link As String, usecache As Boolean) As String ' System.Threading.Tasks.Task(Of String)
        Dim lokaleDatei As String
        Dim zieldir = "", zieldatei As String = ""
        Dim erfolg As Boolean
        Try
            l(" MOD makeLokaleDatei anfang")
            link = makeHttpUrl(link)

            'erfolg = getDownloadtargetBplan(link, "c:\ptest\bplankat\cache\bplaene\bplan", zieldir, zieldatei)
            erfolg = getDownloadtargetBplan(link, strGlobals.localDocumentCacheRoot & "\bplankat\cache\bplaene\bplan", zieldir, zieldatei)
            If clsSachdatentools.schonImCache(zieldir, zieldatei, usecache) Then
                lokaleDatei = zieldir & "\" & zieldatei
            Else
                If erfolg Then
                    Dim r As Boolean = meineHttpNet.down(link, zieldatei, zieldir)
                    If r Then
                        '  If meineHttpNet.down(link, zieldatei, zieldir) Then
                        l("downlaod erfolgreich")
                        lokaleDatei = zieldir & "\" & zieldatei
                    Else
                        l("downlaod nicht erfolgreich")
                        'Return False
                        lokaleDatei = ""
                    End If
                Else
                    l("Fehler zieldatei konnte nicht berechnet werden. " & link.ToString)
                    lokaleDatei = ""
                End If
            End If
            Return lokaleDatei
            l(" MOD makeLokaleDatei ende")
        Catch ex As Exception
            l("Fehler in makeLokaleDatei: ", ex)
            Return ""
        End Try
    End Function

    Shared Function getDownloadtargetBplan(url As String, localRoot As String, ByRef zieldir As String, ByRef filename As String) As Boolean
        Dim target As String = ""
        Dim a() As String
        Try
            l(" MOD getDownloadtargetBplan anfang")

            'If iminternet Then
            target = url.Replace(myglobalz.serverWeb.ToLower, "")
            target = target.Replace("/fkat/bplan", "")
            a = target.Split("/"c)
            zieldir = localRoot & "" & a(0) & "\" & a(1)
            filename = a(2)
            'Else
            '    target = url.Replace(myglobalz.serverUNC.ToLower, "")
            '    target = target.Replace("/fkat/bplan", "")
            '    target = target.Replace("\fkat\bplan", "")
            '    target = target.Replace("\", "/")
            '    a = target.Split("/"c)
            '    zieldir = localRoot & "" & a(0) & "\" & a(1)
            '    filename = a(2)
            'End If

            'target = target.Replace("/", "\")
            l(" MOD getDownloadtargetBplan ende")
            Return True
        Catch ex As Exception
            l("Fehler in getDownloadtargetBplan: ", ex)
            Return False
        End Try
    End Function

    Shared Function istBaulastTiff(tempdat As String) As Boolean
        Return tempdat.ToLower.Contains(".tiff") And tempdat.ToLower.Contains("baulasten")
    End Function
    Shared Function schonImCache(targetcacheroot As String, filename As String, usecache As Boolean) As Boolean
        Try
            l(" schonImCache ---------------------- anfang")
            l(" usecache ---------------------- anfang" & usecache)
            If Not usecache Then Return False
            Dim fi As New IO.FileInfo(IO.Path.Combine(targetcacheroot, filename)) 'targetcacheroot & "\" & filename)
            If fi.Exists Then
                Return True
            Else
                Return False
            End If
            l(" schonImCache ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in schonImCache: ", ex)
            Return False
        End Try
    End Function
    Shared Function buildResultTextArray(trenn As String, colnames As String(), result As String) As String
        Dim summe As String = ""
        Dim sdat() As String
        Try
            l(" buildResultText ---------------------- anfang")
            sdat = result.Split("#"c)
            For i = 9 To sdat.Count - 1
                summe = summe & clsString.Capitalize(colnames(i)).Trim & ": " &
                        sdat(i).Trim & trenn
            Next
            l(" buildResultText ---------------------- ende")
            Return summe
        Catch ex As Exception
            l("Fehler in buildResultText1: ", ex)
            Return "(Objektsuche nicht definiert - bitte beim Admin melden)"
        End Try
    End Function

    Shared Function buildResultTextDT(trenn As String, dt As DataTable) As String
        Dim summe As String = ""
        Try
            l(" buildResultText ---------------------- anfang")
            For j = 9 To dt.Columns.Count - 1
                summe = summe & clsString.Capitalize(dt.Columns(j).Caption) & ": " &
                clsDBtools.fieldvalue(dt.Rows(0).Item(j)).Trim & trenn
            Next
            l(" buildResultText ---------------------- ende")
            Return summe
        Catch ex As Exception
            l("Fehler in buildResultText2: ", ex)
            Return "(Objektsuche nicht definiert - bitte beim Admin melden)"
        End Try
    End Function

    Friend Shared Function getBegleitplanFilelisteIntranet(pdf As String, verzeichnis As String, gemarkung As String) As List(Of clsFlurauswahl)
        Dim di As New IO.DirectoryInfo(verzeichnis)
        Dim templiste As IO.FileInfo()
        Dim ausschluss As String = ""
        Dim newpdf As clsFlurauswahl
        Dim begleitfilelist = New List(Of clsFlurauswahl)
        Try
            l("getBegleitplanFileliste---------------------- anfang")
            templiste = di.GetFiles("*.pdf")
            Dim dra As IO.FileInfo
            ausschluss = pdf & ".pdf"
            'list the names of all files in the specified directory
            For Each dra In templiste
                newpdf = New clsFlurauswahl
                newpdf.displayText = dra.Name
                newpdf.nenner = dra.FullName
                newpdf.temp = gemarkung
                newpdf.temp2 = pdf
                Debug.Print(dra.ToString)
                If ausschluss <> dra.Name.ToLower Then
                    begleitfilelist.Add(newpdf)
                End If
            Next
            Return begleitfilelist
            l("getBegleitplanFileliste---------------------- ende")
        Catch ex As Exception
            l("Fehler in getBegleitplanFileliste: ", ex)
            Return Nothing
        End Try
    End Function
    Shared Sub dossierOhneImap(KoordinateKLickpt As Point?)
        Dim fangRadiusInMeter As Double
        Dim utmpt As Point = clsMiniMapTools.makeUTM(KoordinateKLickpt)
        os_tabelledef = clsMiniMapTools.makeTabname(layerActive)
        fangRadiusInMeter = clsSachdatentools.calcFangradiusM(globCanvasWidth, myglobalz.fangradius_in_pixel,
                              kartengen.aktMap.aktrange.xdif, layerActive.tabname)
        'FS feststellen
        aktFST.clear()
        aktFST.punkt.X = utmpt.X
        aktFST.punkt.Y = utmpt.Y
        aktFST.normflst.FS = pgisTools.getFS4UTM(utmpt)
        aktFST.normflst.splitFS(aktFST.normflst.FS)

        clsFSTtools.holeKoordinaten4Flurstueck(aktFST.normflst.nenner.ToString, WinDetailSucheFST.AktuelleBasisTabelle, aktFST)
        aktFST.abstract = pgisTools.getGID4fs(aktFST.normflst.FS, False, WinDetailSucheFST.AktuelleBasisTabelle)
        aktFST.name = WinDetailSucheFST.AktuelleBasisTabelle
        ' getSerialFromPostgis(aktFST.normflst.FS, False, WinDetailSucheFST.AktuelleBasisTabelle) ' setzt  aktFST.serial 

        clsFSTtools.dossierPrepMinimum()
    End Sub
    Private Shared Function getGID4FS(fs As String, schema As String,
                                              tabelle As String) As List(Of Integer)
        Dim Sql As String
        l("getGID4POINTlayer       ")
        Sql = "SELECT *  from " & schema & "." & tabelle & " as g,  " &
                    "ST_Buffer(ST_CurveToLine( ( select geom from  flurkarte.basis_f   where fs='" & fs & "')),-0.5,2) as b   " &
                    " WHERE  ST_Intersects(g.geom, b) "
        l("sql: " & Sql)
        Dim gids As New List(Of Integer)
        Try
            l("getGID4layer ")

            Dim dt As DataTable
            dt = getDTFromWebgisDB(Sql, "postgis20")
            For i = 0 To dt.Rows.Count - 1
                l("getGID4layer " & clsDBtools.fieldvalue(dt.Rows(i).Item(0)))
                gids.Add(CInt(clsDBtools.fieldvalue(dt.Rows(i).Item(0))))
            Next
            Return gids 'CInt(clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
        Catch ex As Exception
            l("Fehler in getGID4POINTlayer: ", ex)
            Return Nothing
        End Try
    End Function
    'Shared Function fsMitAktiveEbene(aid As Integer,
    '                                 fs As String) As List(Of Integer)
    '    Dim gids As New List(Of Integer)
    '    Dim summe As String = ""
    '    Try
    '        l(" fsMitAktiveEbene ---------------------- anfang")
    '        l("aid " & aid)
    '        l("fs " & fs)
    '        If aid < 1 Then
    '            l("fehler in fsMitAktiveEbene aid ist kleiner eins " & fs)
    '            Return Nothing
    '        End If
    '        ' schema, tabelle gid zur aid ermitteln 

    '        os_tabelledef = ModsachdatenTools.getSChemaDB(aid, 1)
    '        If os_tabelledef Is Nothing Then
    '            os_tabelledef = New clsTabellenDef With {
    '           .aid = CStr(aid),
    '           .gid = "0",
    '           .datenbank = "postgis20",
    '           .tab_nr = CType(1, String)
    '       }
    '        End If
    '        os_tabelledef.aid = CStr(aid)
    '        os_tabelledef.datenbank = "postgis20"
    '        gids = (getGID4FS(fs, os_tabelledef.Schema, os_tabelledef.tabelle))
    '        l("  in fsMitAktiveEbene: gids.Count " & gids.Count & " aid:" & aid)
    '        Return gids
    '    Catch ex As Exception
    '        l("Fehler in fsMitAktiveEbene:   " & " aid:" & aid,ex)
    '        Return Nothing
    '    End Try
    '    Return Nothing
    'End Function

    Friend Shared Function getBegleitplanFilelisteInternet(pdf As String, gemarkung As String, verzeichnis As String) As List(Of clsFlurauswahl)
        Dim hinweis As String = "", result As String = "", aufruf As String
        Try
            Dim newpdf As clsFlurauswahl
            Dim begleitfilelist = New List(Of clsFlurauswahl)
            ' Return begleitfilelist
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick &
                                            "&modus=getbegleitbplan" &
                                            "&pdf=" & pdf &
                                            "&gemarkung=" & clsString.umlaut2ue(gemarkung)
            'myglobalz.enc
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000) : l(hinweis)
            result = result.Trim : l(result)
            result = clsString.removeLastChar(result)
            result = result.Trim : l(result)
            If result.IsNothingOrEmpty Then
                Return begleitfilelist
            End If
            Dim a() As String
            Dim datei As String = ""
            a = result.Split("#"c)
            For i = 0 To a.Count - 1
                datei = verzeichnis & "/" & a(i).Trim
                newpdf = New clsFlurauswahl
                newpdf.displayText = a(i).Trim
                newpdf.nenner = datei
                newpdf.temp = gemarkung
                newpdf.temp2 = pdf
                begleitfilelist.Add(newpdf)
            Next
            Return begleitfilelist
        Catch ex As Exception
            l("Fehler in getBegleitplanFilelisteInternet:   " & "  :", ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function dossierAktiveEbene(winpt As Point, aid As Integer,
                                               radiusInMeter As Integer, os_tabelledef As clsTabellenDef) As List(Of Integer)
        Dim summe As String = ""
        Try
            l(" dossierAkviveEbene ---------------------- anfang")
            ' schema, tabelle gid zur aid ermitteln 
            If aid < 1 Then
                l("warnung in dossierAktiveEbene aid <1 ")
                Return Nothing
            End If
            os_tabelledef = ModsachdatenTools.getSChemaDB(aid, 1)
            If os_tabelledef Is Nothing Then
                os_tabelledef = New clsTabellenDef With {
               .aid = CStr(aid),
               .gid = "0",
               .datenbank = "postgis20",
               .tab_nr = CType(1, String)
                   }
            End If
            os_tabelledef.aid = CType(aid, String)
            os_tabelledef.datenbank = "postgis20"
            'schema = "planung" : tabelle = "bebauungsplan_f"
            Dim gids As New List(Of Integer)

            Dim innerSQL As String
            l("getGID4POINTlayer       ")
            innerSQL = "ST_Buffer(ST_SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ")," &
            PostgisDBcoordinatensystem.ToString & ")," & radiusInMeter & ",2)"
            l(innerSQL)
            Dim SQL = "SELECT distinct gid " &
                "from " & os_tabelledef.Schema & "." & os_tabelledef.tabelle & " as g, " & innerSQL & " as b " &
                "WHERE ST_Intersects(g.geom, b) order by gid desc"
            l(SQL)
            If iminternet Or CGIstattDBzugriff Then
                'SQL = "select * " &
                '"from public.stamm " ' & os_tabelledef.Schema & "." & os_tabelledef.tabelle
                gids = (getGID4POINTlayerHTTP(SQL))
            Else
                gids = (getGID4POINTlayerDB(SQL))
            End If
            Return gids
        Catch ex As Exception
            l("Fehler in dossierAkviveEbene: ", ex)
            Return Nothing
        End Try
        Return Nothing
    End Function

    Friend Shared Function getDownloadtargetGeodaten(url As String, localDocumentCacheRoot As String, ByRef zieldir As String, ByRef zieldatei As String) As Boolean
        Try
            'http://geodaten.kreis-offenbach.de/natura2000/allgemeiner_VO_Text/Natura2000-VO-Text_allgemeiner_Teil.pdf
            l(" MOD getDownloadtargetGeodaten anfang")
            Dim target As String = ""
            Dim a() As String
            Try
                l(" MOD getDownloadtargetGeodaten anfang")
                target = url.Replace("http://geodaten.kreis-offenbach.de/".ToLower, "")
                a = target.Split("/"c)
                zieldir = localDocumentCacheRoot & "\geodaten\" & a(0) & "\" & a(1) & "\"
                zieldatei = a(2)
                l(" MOD getDownloadtargetGeodaten ende")
                Return True
            Catch ex As Exception
                l("Fehler in getDownloadtargetGeodaten: ", ex)
                Return False
            End Try
            l(" MOD getDownloadtargetGeodaten ende")
            Return True
        Catch ex As Exception
            l("Fehler in getDownloadtargetGeodaten: ", ex)
            Return False
        End Try
    End Function

    Friend Shared Function getDownloadtargetAID(url As String, localDocumentCacheRoot As String, ByRef zieldir As String, ByRef filename As String) As Boolean
        Try
            ' '\\w2gis02\gdvell\\nkat\aid\341\texte\33-1996b.pdf
            l(" MOD getDownloadtargetAID anfang")
            Dim target As String = ""
            Dim a() As String
            Try
                l(" MOD getDownloadtargetBaulasten anfang")
                target = url.Replace(myglobalz.serverWeb.ToLower, "")
                a = target.Split("/"c)
                zieldir = localDocumentCacheRoot & "\nkat\aid\" & a(3) & "\"
                filename = a(5)
                l(" MOD getDownloadtargetBaulasten ende")
                Return True
            Catch ex As Exception
                l("Fehler in getDownloadtargetBaulasten: ", ex)
                Return False
            End Try
            l(" MOD getDownloadtargetAID ende")
            Return True
        Catch ex As Exception
            l("Fehler in getDownloadtargetAID: ", ex)
            Return False
        End Try
    End Function
    Friend Shared Function getDownloadtargetND(url As String, localDocumentCacheRoot As String, ByRef zieldir As String, ByRef filename As String) As Boolean
        Try
            ' '\\w2gis02\gdvell\\nkat\aid\341\texte\33-1996b.pdf
            l(" MOD getDownloadtargetND anfang")
            Dim target As String = ""
            Dim a() As String
            Try
                l(" MOD getDownloadtargetBaulasten anfang")
                target = url.Replace(myglobalz.serverWeb.ToLower, "")
                a = target.Split("/"c)
                zieldir = localDocumentCacheRoot & "\nkat\aid\" & a(3) & "\"
                filename = a(5)
                l(" MOD getDownloadtargetND ende")
                Return True
            Catch ex As Exception
                l("Fehler in getDownloadtagetDownloadtargetNDrgetBaulasten: ", ex)
                Return False
            End Try
            l(" MOD getDownloadtargetND ende")
            Return True
        Catch ex As Exception
            l("Fehler in getDownloadtargetND: ", ex)
            Return False
        End Try
    End Function

    Friend Shared Function getActiveLayer4point(winpt As Point, aid As Integer,
                                               cwidth As Integer, cheight As Integer,
                                               _screenPT As Point?,
                                                fangRadiusInMeter As Double, os_tabelledef As clsTabellenDef) As List(Of Integer)
        Dim returnvalue As String = ""
        Dim screenpt As New myPoint
        Dim gids As New List(Of Integer)
        Try
            l(" getActiveLayer4point ---------------------- anfang")
            screenpt.X = _screenPT.Value.X
            screenpt.Y = _screenPT.Value.Y
            gids = dossierAktiveEbene(winpt, aid, CInt(fangRadiusInMeter), os_tabelledef)
            l(" getActiveLayer4point ---------------------- ende")
            Return gids
        Catch ex As Exception
            l("Fehler in getActiveLayer4point: ", ex)
            Return Nothing
        End Try
    End Function

    Shared Function calcFangradiusM(cwidth As Integer,
                                    fangradius_in_pixel As Integer,
                                    xdifMeter As Double,
                                    tabname As String) As Double
        Dim radiusInMeter As Integer
        Dim MeterProPixel As Double
        Try
            l(" calcFangradiusM ---------------------- anfang")
            MeterProPixel = xdifMeter / cwidth
            'If MeterProPixel < 1 Then MeterProPixel = 1
            radiusInMeter = CInt((MeterProPixel) * fangradius_in_pixel)

            If tabname.EndsWith("_f") Then
                radiusInMeter = 1
            End If
            If tabname.EndsWith("_p") Then
                MeterProPixel = xdifMeter / cwidth
                'If MeterProPixel < 1 Then MeterProPixel = 1
                radiusInMeter = CInt((MeterProPixel) * fangradius_in_pixel)
                radiusInMeter = 5 + (radiusInMeter * 2)
            End If
            l(" calcFangradiusM ---------------------- ende")
            Return radiusInMeter
        Catch ex As Exception
            l("Fehler in calcFangradiusM: ", ex)
            Return 4
        End Try
    End Function

    Shared Function calcGisDossierParams(winpt As Point, aktaid As Integer, cwidth As Integer,
                                          cheight As Integer, screenx As Double, screeny As Double,
                                             radiusInMeter As Integer, username As String,
                                             obergruppe As String, vid As String, fs As String,
                                     geometrietyp As String, unterGruppe As String) As String
        Dim strKoord As String = " koordinate=" & CStr(winpt.X).Replace(",", ".") & "," & CStr(winpt.Y).Replace(",", ".")
        Dim strIstAlb As String = " istalbberechtigt=0 "
        Dim strAktaid As String = " aktaid=" & aktaid & " "
        Dim strbreite As String = " breite=" & cwidth
        Dim strhoehe As String = " hoehe=" & cheight
        Dim strscreenx As String = " screenx=" & screenx
        Dim strscreeny As String = " screeny=" & screeny
        Dim strradius As String = " radiusinmeter=" & radiusInMeter
        Dim strusername As String = " username=" & username
        Dim strobergruppe As String = " obergruppe=" & obergruppe
        Dim strunterGruppe As String = " untergruppe=" & unterGruppe
        Dim strFS As String = " fs=" & fs
        Dim strVID As String = " vid=" & vid
        Dim strGeometrie As String = " geometrie=" & geometrietyp ' punkt flurstueck

        If GisUser.istalbberechtigt Then
            strIstAlb = " istalbberechtigt=1 "
        Else
            strIstAlb = " istalbberechtigt=0 "
        End If

        Dim params As String = strGeometrie & strVID & strFS & strKoord & " " & strIstAlb & " " & strAktaid &
            strbreite & strhoehe & strscreenx & strscreeny & strradius & strusername & strobergruppe & strunterGruppe
        Return params

    End Function

    Private Shared Sub starteGisDossierProcess(params As String)
        Try
            l("StartGisDossierExtern ---------------------- anfang")
            l("myglobalz.gisdossierexe   " & strGlobals.gisdossierexe)
            l("params " & params)

            Dim si As New ProcessStartInfo
            si.FileName = strGlobals.gisdossierexe
            si.WorkingDirectory = "C:\ptest\gisdossier"
            si.Arguments = params
            'Process.Start(neuervorgangstgring, "modus=neu")
            Process.Start(si)
            si = Nothing


            'Process.Start(strGlobals.gisdossierexe, params)
            l("StartGisDossierExtern ---------------------- ende")
        Catch ex As Exception
            l("Fehler in StartGisDossierExtern: " & params & " ", ex)
        End Try
    End Sub

    Private Shared Function getGID4POINTlayerHTTP(sql As String) As List(Of Integer)
        Try
            l("getGID4layer ")
            Dim result As String = "", hinweis As String = ""
            Dim gids As New List(Of Integer)
            result = clsToolsAllg.getSQL4Http(sql, "postgis20", hinweis:="", modus:="getsql") : l(hinweis)
            'result = result.Replace("$", "").Replace(vbCrLf, "")
            result = result.Trim
            result = clsString.removeLastChar(result)
            Dim a() As String
            a = result.Split("$"c)

            For i = 0 To a.Count - 1
                l("getGID4layer " & a(i))
                If Not clsDBtools.fieldvalue(a(i)).Trim.IsNothingOrEmpty Then
                    gids.Add(CInt(clsDBtools.fieldvalue(a(i))))
                End If
            Next
            Return gids
        Catch ex As Exception
            l("Fehler in getGID4POINTlayer: ", ex)
            Return Nothing
        End Try
    End Function
    Private Shared Function getGID4POINTlayerDB(sql As String) As List(Of Integer)
        Try
            l("getGID4layer ")
            Dim dt As DataTable
            Dim gids As New List(Of Integer)
            dt = getDTFromWebgisDB(sql, "postgis20")
            'ReDim gids(dt.Rows.Count - 1)
            For i = 0 To dt.Rows.Count - 1
                l("getGID4layer " & clsDBtools.fieldvalue(dt.Rows(i).Item(0)))
                gids.Add(CInt(clsDBtools.fieldvalue(dt.Rows(i).Item(0))))
            Next
            Return gids 'CInt(clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
        Catch ex As Exception
            l("Fehler in getGID4POINTlayer: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Shared Sub getdossier(utmpt As Point, aid As Integer, cwidth As Integer, cHeight As Integer,
                                 _screenPT As Point?, inputfs As String, InputGeometrie As String)
        Dim returnvalue As String = ""
        Dim screenpt As New myPoint
        'Dim gids As Integer()
        Try
            l(" getdossier ---------------------- anfang")
            screenpt.X = _screenPT.Value.X
            screenpt.Y = _screenPT.Value.Y

            Dim radius_in_pixel As Integer = 7
            Dim MeterProPixel As Double
            Dim radiusInMeter As Integer

            MeterProPixel = kartengen.aktMap.aktrange.xdif / cwidth
            If MeterProPixel < 1 Then MeterProPixel = 1
            radiusInMeter = CInt((MeterProPixel) * radius_in_pixel)
            l(" kein fehler dossier aufruf " & GisUser.nick)
            Dim params = calcGisDossierParams(utmpt, layerActive.aid, cwidth, cHeight,
                                    screenpt.X, screenpt.Y, radiusInMeter,
                                    GisUser.nick, GisUser.ADgruppenname, "0", inputfs,
                                  InputGeometrie, GisUser.favogruppekurz
                                    )
            starteGisDossierProcess(params)
            l(" getdossier ---------------------- ende")
        Catch ex As Exception
            l("Fehler in getdossier: ", ex)
        End Try
    End Sub

    Friend Shared Function getlage(fs As String) As String
        Try
            l(" getlage ---------------------- anfang")
            Dim dt As DataTable
            Dim strlage = ""
            Dim sql As String
            sql = "select * from flurkarte.basis_ext_f where fs='" & fs & "'"
            dt = getDTFromWebgisDB(sql, "postgis20")
            If dt.Rows.Count > 0 Then
                strlage = "Lage: " & clsDBtools.fieldvalue(dt.Rows(0).Item("name")).Trim
                strlage = strlage & ", " & clsDBtools.fieldvalue(dt.Rows(0).Item("lage")).Trim
                If clsDBtools.fieldvalue(dt.Rows(0).Item("hausnr")).Trim <> String.Empty Then
                    strlage = strlage & ", Nr: " & clsDBtools.fieldvalue(dt.Rows(0).Item("hausnr")).Trim & ". "
                    'strlage = strlage & "Bez: " & clsDBtools.fieldvalue(dt.Rows(0).Item("bezeich")).Trim
                Else
                    strlage = strlage & ". "
                End If
            Else
                strlage = ""
            End If
            l(" getlage ---------------------- ende: " & strlage)
            Return strlage
            Return ""
        Catch ex As Exception
            l("Fehler in getlage: " & fs & ", ", ex)
            Return ""
        End Try
    End Function
    Shared Function erzeugeUndOeffneEigentuemerPDF(text As String, lage As String) As String
        Dim lokalitaet, flaeche As String
        lokalitaet = getlokalitaetstring(aktFST)
        flaeche = clsFSTtools.getFlaecheZuFlurstueck(aktFST)
        lokalitaet = lokalitaet & " " & flaeche
        Dim ausgabedatei As String = tools.calcEigentuemerAusgabeFile

        wrapItextSharp.createSchnellEigentuemer(text, ausgabedatei, albverbotsString, lokalitaet, lage)
        Return ausgabedatei
    End Function

    Friend Shared Function makeLokalDatei(link As String, evtlPfad As String) As String
        Dim lokaleDatei As String
        Dim zieldir = "", zieldatei As String = ""
        Dim erfolg As Boolean
        Try
            l(" MOD makeLokaleDatei anfang")
            link = makeHttpUrl(link)
            If evtlPfad.IsNothingOrEmpty Then evtlPfad = "unbekannt"
            'erfolg = getDownloadtargetBplan(link, "c:\ptest\bplankat\cache\bplaene\bplan", zieldir, zieldatei)
            erfolg = getDownloadtargetND(link, strGlobals.localDocumentCacheRoot, zieldir, zieldatei)
            If clsSachdatentools.schonImCache(zieldir, zieldatei, True) Then
                lokaleDatei = IO.Path.Combine(zieldir, zieldatei)
            Else
                If erfolg Then
                    Dim r As Boolean = meineHttpNet.down(link, zieldatei, zieldir)
                    If r Then
                        '  If meineHttpNet.down(link, zieldatei, zieldir) Then
                        l("downlaod erfolgreich")
                        lokaleDatei = zieldir & "\" & zieldatei
                    Else
                        l("downlaod nicht erfolgreich")
                        'Return False
                        lokaleDatei = ""
                    End If
                Else
                    l("Fehler zieldatei konnte nicht berechnet werden. " & link.ToString)
                    lokaleDatei = ""
                End If
            End If
            Return lokaleDatei
            l(" MOD makeLokaleDatei ende")
        Catch ex As Exception
            l("Fehler in makeLokaleDatei: ", ex)
            Return ""
        End Try
    End Function

    Private Shared Function makeHttpUrl(link As String) As String
        Try
            l(" MOD makeHttpCall anfang")
            If Not link.ToLower.StartsWith("http") Then
                If link.ToLower.StartsWith("\\w2gis02\gdvell") Then
                    link.Replace("\\w2gis02\gdvell", myglobalz.serverWeb)
                Else
                    link = myglobalz.serverWeb & link
                End If
            End If
            l(" MOD makeHttpCall ende")
            Return link
        Catch ex As Exception
            l("Fehler in makeHttpCall: ", ex)
            Return ""
        End Try
    End Function
End Class
