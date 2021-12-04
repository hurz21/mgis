Imports System.Data
Imports mgis

Public Class clsMiniMapTools
    'kurzversion
    Public Shared Function punktvonGKnachCanvasUmrechnen(ByVal aktpointInMeter As myPoint,
                                                         ByVal birdsrangeInMeter As clsRange,
                                                         ByVal KreiscanvasInPixel As clsCanvas,
                                                         Optional divisor As Integer = 100) As myPoint
        nachricht("punktvonGKnachCanvasUmrechnen--------------------")
        Dim testr As Double, testh As Double
        Dim neupoint As New myPoint
        Dim cnt As Integer = 0
        Try

            nachricht("aktpoint.X:" & aktpointInMeter.X)
            nachricht("aktpoint.y:" & aktpointInMeter.Y)
            nachricht("divisor:" & divisor)
            nachricht("divisor:" & divisor)
            nachricht("birdsrangeInMeter.xdif:" & birdsrangeInMeter.xdif)
            nachricht("birdsrangeInMeter.ydif:" & birdsrangeInMeter.ydif)
            nachricht("divisor:" & divisor)
            cnt = 1
            testr = ((aktpointInMeter.X / divisor) - birdsrangeInMeter.xl) / birdsrangeInMeter.xdif : cnt = 3
            testr = testr * KreiscanvasInPixel.w : cnt = 4
            testh = ((aktpointInMeter.Y / divisor) - birdsrangeInMeter.yl) / birdsrangeInMeter.ydif : cnt = 5
            testh = KreiscanvasInPixel.h - (testh * KreiscanvasInPixel.h) : cnt = 6
            testr = Fix(testr) : cnt = 7
            testh = Fix(testh) : cnt = 8
            neupoint.X = CInt(testr) : cnt = 9
            neupoint.Y = CInt(testh) : cnt = 10
            Return neupoint
        Catch ex As Exception
            nachricht("fehler punktvonGKnachCanvasUmrechnen: " & aktpointInMeter.toString & "," & cnt & "," & divisor & ",", ex)
            Return neupoint
        End Try
    End Function

    Public Shared Function polygonNachCanvasUmrechnen(punktarrayGK As myPoint(),
                                                       ByVal birdsrange As clsRange,
                                                       ByVal Kreiscanvas As clsCanvas) As myPoint()

        l("polygonNachCanvasUmrechnen-----------------")
        Try
            Dim punkteCanvas(punktarrayGK.GetUpperBound(0)) As myPoint
            For i = 0 To punktarrayGK.GetUpperBound(0)
                If IsNothing(punktarrayGK(i)) Then
                    Debug.Print("")
                    Continue For
                End If
                punkteCanvas(i) = New myPoint
                punkteCanvas(i) = clsMiniMapTools.punktvonGKnachCanvasUmrechnen(punktarrayGK(i), birdsrange, Kreiscanvas, divisor:=100)
                '   RandKorrektur(Kreiscanvas, punkteCanvas, i)
            Next
            Return punkteCanvas
        Catch ex As Exception
            nachricht("warnung in polygonNachCanvasUmrechnen:", ex)
            Return Nothing
        End Try
    End Function
    Private Shared Sub Polygon_MouseDownFS(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Dim eee As System.Windows.Shapes.Polygon = DirectCast(e.Source, System.Windows.Shapes.Polygon)
        e.Handled = True
    End Sub


    Public Shared Function zerlegeInPunkte(gkstring As String,
                                           dezimalTrenner As Char,
                                           ByRef multipolygonpointer() As Integer,
                                           rid As Integer,
                                           ShapeSerialstringIstPostGis As Boolean) As myPoint()
        'RID dient nur der identifikation im logfile

        Dim istart As Integer = 0

        Dim a(), nurKoordinaten() As String
        ' Dim multipolygonpointer() As Integer
        Dim myp() As myPoint = Nothing

        Dim meinpointer As Integer
        Dim errorout As String = "errorout"
        Try
            If String.IsNullOrEmpty(gkstring) Then
                nachricht("Fehler: gkstring ist leer!!!")
                Return Nothing
            End If
            errorout = errorout & ", gkstring: " & gkstring
            meinpointer = 2
            a = gkstring.Split(";"c)
            If ShapeSerialstringIstPostGis Then
                istart = 0
            Else
                istart = getKoordinatenstart(gkstring, dezimalTrenner)
                If istart < 0 Then
                    Return Nothing
                End If
            End If

            leereFelderAbschneiden(a)
            'If rid= 26159 Then
            '    Debug.Print("")
            'End If
            nurKoordinaten = bildeNurKoordinatenArray(a, istart)
            multipolygonpointer = bildeTeilFlaechenPointer(a, istart)
            myp = koords2PointArray(dezimalTrenner, nurKoordinaten, rid)
            Return myp
        Catch ex As Exception
            nachricht("Fehler in zerlegeInPunkte (" & rid & "):" & " meinpointer: " & meinpointer & ": " & gkstring, ex)
            Return myp
        End Try
    End Function
    Private Shared Function koords2PointArray(ByVal dezimalTrenner As Char,
                                                    ByVal nurKoordinaten As String(),
                                                    rid As Integer
                                                    ) As myPoint()
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim b() As String
        Dim meinpointer As Integer
        Dim myp As myPoint()
        Dim oben As Integer
        Try
            oben = CInt((nurKoordinaten.GetUpperBound(0) + 1))
            ReDim myp(CInt(oben / 2) - 1)
            meinpointer = 6
            For i = 0 To oben Step 2
                If i > nurKoordinaten.GetUpperBound(0) Then
                    'wg vid: 24291
                    Continue For
                End If
                If nurKoordinaten(i).IsNothingOrEmpty Then
                    Continue For
                End If
                If Not nurKoordinaten(i).Contains(dezimalTrenner) Then
                    nurKoordinaten(i) = nurKoordinaten(i) & dezimalTrenner & "0"
                End If
                If nurKoordinaten(i).Contains(dezimalTrenner) Then
                    If j > myp.GetUpperBound(0) Then
                        Continue For
                    End If
                    myp(j) = New myPoint
                    If nurKoordinaten(i).IsNothingOrEmpty Then
                        Continue For
                    End If
                    'integeranteil isolieren. warum nicht cint()? weil dezimalpunkt aknn unterschiedlich sein

                    Dim k = CStr(CDbl(nurKoordinaten(i).Replace(".", ",")) * 100)
                    'Dim k = CStr(CDbl(nurKoordinaten(i).Replace(".", ",")))

                    ' dezimalTrenner=","c
                    b = k.Split(","c)
                    myp(j).X = CDbl(b(0))
                    ' myp(j).X = CDbl(nurKoordinaten(i).Replace(".",","))
                    If i + 1 > nurKoordinaten.Length - 1 Then
                        Debug.Print("")
                        k = CStr(CDbl(nurKoordinaten(1).Replace(".", ",")) * 100)
                        'k = CStr(CDbl(nurKoordinaten(1).Replace(".", ",")))
                    Else
                        k = CStr(CDbl(nurKoordinaten(i + 1).Replace(".", ",")) * 100)
                        'k = CStr(CDbl(nurKoordinaten(i + 1).Replace(".", ",")))
                    End If

                    b = k.Split(","c)
                    myp(j).Y = CDbl(b(0))
                    '  myp(j).Y = CDbl(nurKoordinaten(i).Replace(".",","))

                    meinpointer = 8
                    j = j + 1
                End If
            Next
            meinpointer = 9
            Return myp
        Catch ex As Exception
            nachricht("fehler in koords2PointArray: (rid: " & rid & ")", ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function bildeTeilFlaechenPointer(a As String(), istart As Integer) As Integer()
        Dim neu As Integer()
        Try
            ReDim neu(a.Length - 1)
            ' Array.Copy(a, istart, neu, 0, 2)
            For i = 0 To istart - 1
                neu(i) = CInt(a(i))
            Next
            ReDim Preserve neu(istart - 1)
            ' leereFelderAbschneiden(neu)
            Return neu
        Catch ex As Exception
            nachricht("Fehler in bildeTeilFlaechenPointer:", ex)
            Return Nothing
        End Try
    End Function
    Private Shared Function bildeNurKoordinatenArray(a As String(), istart As Integer) As String()
        Dim neu As String()
        Try
            ReDim neu(a.Length - 1)
            If istart = 0 Then
                Return a
            Else
                Array.Copy(a, istart, neu, 0, (a.GetUpperBound(0) - (istart - 1)))
                leereFelderAbschneiden(neu)
                Return neu
            End If
        Catch ex As Exception
            nachricht("Fehler in bildeNurKoordinatenArray:", ex)
            Return Nothing
        End Try
    End Function
    Private Shared Sub leereFelderAbschneiden(ByRef neu As String())
        Try
            For i = neu.GetUpperBound(0) To 0 Step -1
                If neu(i).IsNothingOrEmpty Then
                    ReDim Preserve neu(i - 1)
                End If
            Next
        Catch ex As Exception
            nachricht("Fehler in leereFelderAbschneiden_:", ex)
        End Try
    End Sub
    Private Shared Function getKoordinatenstart(gkstring As String, dezimalTrenner As Char) As Integer
        Try
            Dim a As String() = gkstring.Split(";"c)
            For i = 0 To a.GetUpperBound(0)
                If a(i).Contains(dezimalTrenner) Then
                    Return i
                End If
            Next
            Return -1
        Catch ex As Exception
            nachricht("Fehler in getKoordinatenstart: ", ex)
            Return -1
        End Try
    End Function

    'Friend Shared Sub Polygon_MouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
    '    e.Handled = True
    '    Dim aktPolygon As System.Windows.Shapes.Polygon = DirectCast(e.Source, System.Windows.Shapes.Polygon)
    '    Mouse.Capture(Nothing)
    '    Dim KoordinateKLickpt As Point?
    '    KoordinateKLickpt = e.GetPosition(aktPolygon)

    '    Dim javascriptOriginalstring As String = ""
    '    Dim fangRadiusInMeter As Double

    '    javascriptOriginalstring = aktPolygon.Tag.ToString
    '    l("javascriptOriginalstring " & javascriptOriginalstring)

    '    Dim utmpt As Point = makeUTM(KoordinateKLickpt)
    '    os_tabelledef = clsMiniMapTools.makeTabname(layerActive)
    '    fangRadiusInMeter = clsSachdatentools.calcFangradiusM(globCanvasWidth, myglobalz.fangradius_in_pixel,
    '                          kartengen.aktMap.aktrange.xdif, layerActive.tabname)
    '    'FS feststellen
    '    aktFST.clear()
    '    aktFST.punkt.X = utmpt.X
    '    aktFST.punkt.Y = utmpt.Y
    '    aktFST.normflst.FS = pgisTools.getFS4UTM(utmpt)
    '    aktFST.normflst.splitFS(aktFST.normflst.FS)
    '    aktFST.abstract = aktFST.normflst.gemarkungstext & ", Flur: " & aktFST.normflst.flur & ", Fst: " & aktFST.normflst.fstueckKombi

    '    'clsFSTtools.holeKoordinaten4Flurstueck(tbNenner.Text)
    '    'getSerialFromPostgis(aktFST.normflst.FS) ' setzt  aktFST.serial  

    '    Dim jjj As New winImapMenue(utmpt.X & ", " & utmpt.Y, aktFST.abstract, layerActive.titel)
    '    jjj.ShowDialog()
    '    Dim auswahl = jjj.auswahl
    '    Select Case auswahl
    '        Case "punkt"
    '            handleMouseDownImagemap(KoordinateKLickpt, aktPolygon.Tag.ToString, 0)
    '        Case "fs"
    '            verschneideFSmitAktiverEbene(aktFST.normflst.FS, layerActive.aid, javascriptOriginalstring, fangRadiusInMeter, 0)
    '            'setBoundingRefresh(kartengen.aktMap.aktrange)
    '            'suchObjektModus = "fst"
    '        Case "dossier"
    '            clsFSTtools.dossierPrepMinimum()
    '    End Select
    'End Sub

    'Private Shared Sub verschneideFSmitAktiverEbene(fS As String,
    '                                                aid As Integer,
    '                                                javascriptOriginalstring As String,
    '                                                fangradiusinmeter As Double, fensterzaehler As Integer)
    '    Try
    '        l(" verschneideFSmitAktiverEbene ---------------------- anfang")
    '        Dim gids As New List(Of Integer)
    '        gids = clsSachdatentools.fsMitAktiveEbene(aid, fS)
    '        handleGIDsAuswahl(javascriptOriginalstring, fangradiusinmeter, gids, fensterzaehler)
    '        l(" verschneideFSmitAktiverEbene ---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in verschneideFSmitAktiverEbene: " & ex.ToString())
    '    End Try
    'End Sub

    Public Shared Sub Polygon_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        e.Handled = True
        Dim aktPolygon As System.Windows.Shapes.Polygon = DirectCast(e.Source, System.Windows.Shapes.Polygon)
        Mouse.Capture(Nothing)
        handleMouseDownImagemap(e.GetPosition(aktPolygon), CType(aktPolygon.Tag, String), 0)
    End Sub

    Shared Sub handleMouseDownImagemap(KoordinateKLickpt As Point?, aktPolygonTag As String, fensterzaehler As Integer)
        Dim javascriptOriginalstring As String = ""
        Dim newtechno As Boolean = True
        Dim fangRadiusInMeter As Double
        Dim gids As New List(Of Integer)
        Dim utmpt As New Point
        Try
            l("handleMouseDownImagemap aktPolygonTag.ToString " & aktPolygonTag.ToString)
            javascriptOriginalstring = aktPolygonTag.ToString
            If javascriptOriginalstring.Trim.IsNothingOrEmpty Then
                l("handleMouseDownImagemap kein javascripttag, keine dbabfrage. abbruch")
                Exit Sub
            End If
            l("javascriptOriginalstring " & javascriptOriginalstring)
            'If newtechno Then
            utmpt = makeUTM(KoordinateKLickpt)


            tempActivelayer = CType(layerActive.Clone, clsLayerPres)
            Dim params() As String = clsJavascript.isoliereCGIparameter(javascriptOriginalstring)
            If layerActive.titel.ToLower.StartsWith("auswahl: ") Then
                tempActivelayer.aid = CInt(params(0))
            Else
            End If
            os_tabelledef = clsMiniMapTools.makeTabname(tempActivelayer)
            fangRadiusInMeter = clsSachdatentools.calcFangradiusM(
                                                    globCanvasWidth, myglobalz.fangradius_in_pixel,
                                                    kartengen.aktMap.aktrange.xdif, tempActivelayer.tabname)
            Select Case os_tabelledef.tabellen_anzeige.ToLower
                Case "homepage"
                    params = clsJavascript.isoliereCGIparameter(javascriptOriginalstring)
                    showKreisOffenbachHomepage(params, os_tabelledef)
                Case "vorlage"
                    params = clsJavascript.isoliereCGIparameter(javascriptOriginalstring)
                    Debug.Print("")
                    handleVorlage(params, os_tabelledef)
                Case "schalter_html"
                    params = clsJavascript.isoliereCGIparameter(javascriptOriginalstring)
                    Debug.Print("")
                    Dim aufruf = "/buergergis/php/query_sachdaten.php?option=mapclick&aid=" & params(0) &
                            "&tab_nr=" & params(1) & "&gid=" + params(2) & "&querytitel=" & tempActivelayer.titel
                    aufruf = serverWeb & aufruf

                    Dim result As String
                    Dim hinweis As String = ""
                    Dim buttonINfostring As String = ""
                    Dim timeout = 5000
                    result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, timeout)
                    '  result = result.Replace("$", "").Replace(vbCrLf, "")
                    Dim rtfdatei As String = dbTemplateString.Replace("[DBINFO]", result)
                    Dim freiDB As winDBabfrage = New winDBabfrage(rtfdatei, "text", buttonINfostring, CBool(tempActivelayer.aid = GisUser.userLayerAid), fensterzaehler, Nothing)
                    freiDB.Show()
                Case "landschaftsplan"
                    params = clsJavascript.isoliereCGIparameter(javascriptOriginalstring)
                    Debug.Print("")
                    handleLandschaftsplan(params, os_tabelledef)
                Case Else 'tooltip,schalter,landschaftsplan,vorlage,normal,attributtabelle,homepage,""
                    Dim ersatzGID As Integer
                    params = clsJavascript.isoliereCGIparameter(javascriptOriginalstring)
                    ersatzGID = getGIDfromParams(params)
                    gids = clsSachdatentools.getActiveLayer4point(utmpt, tempActivelayer.aid,
                                                   globCanvasWidth, globCanvasHeight,
                                                   KoordinateKLickpt,
                                                   fangRadiusInMeter, os_tabelledef)
                    If gids Is Nothing Or gids.Count < 1 Then
                        params = clsJavascript.isoliereCGIparameter(javascriptOriginalstring)
                        If ersatzGID > 0 Then
                            gids.Add(ersatzGID)
                            handleGIDsAuswahl(fangRadiusInMeter, gids, fensterzaehler)
                        Else
                            l("fehler in handleMouseDownImagemap a: gids is nothing , vermutlich ist layerActive.aid =0 oder der fangradius ist zu klein: " & tempActivelayer.aid &
                              ",fangRadiusInMeter:" & fangRadiusInMeter)
                        End If
                    Else
                        handleGIDsAuswahl(fangRadiusInMeter, gids, fensterzaehler)
                    End If
            End Select
        Catch ex As Exception
            l("fehler in handleMouseDownImagemap b ", ex)
        End Try
    End Sub

    'Private Shared Sub handleAttributtabelle(fangRadiusInMeter As Double, gids As List(Of Integer), fensterzaehler As Integer, os_tabelledef As clsTabellenDef)
    '    Dim javascriptMimikry As String = ""
    '    Dim buttonINfostringspecfunc As String = ""
    '    Try
    '        l(" MOD handleAttributtabelle anfang")
    '        If gids.Count = 1 Then
    '            os_tabelledef.gid = CType(gids(0), String)
    '            If CInt(os_tabelledef.gid) > 0 Then



    '                javascriptMimikry = genjavascriptMimikry(os_tabelledef.aid, os_tabelledef.tab_nr, os_tabelledef.gid)
    '                clsMiniMapTools.handlejavascript(javascriptMimikry, os_tabelledef, buttonINfostringspecfunc)
    '                dbAbfrageDiaglogPrep(buttonINfostringspecfunc, layerActive.titel, fensterzaehler)
    '                l(" dossierAkviveEbene ---------------------- ende")

    '            End If
    '        End If
    '        If gids.Count > 1 Then
    '            l(" mehr als eine gid")
    '            Dim auswahl As New winGIDauswahl(CInt(os_tabelledef.aid), gids,
    '                                                 os_tabelledef.Schema & ".os_" & os_tabelledef.tabelle,
    '                                                 layerActive.titel, fangradius_in_pixel, fangRadiusInMeter)
    '            auswahl.ShowDialog()
    '        End If
    '        l(" MOD handleAttributtabelle ende")
    '    Catch ex As Exception
    '        l("Fehler in handleAttributtabelle: " & ex.ToString())
    '    End Try
    'End Sub

    Private Shared Function getGIDfromParams(params() As String) As Integer
        Dim result As Integer = 0
        l(" MOD getGISfromParams anfang")
        Try
            If IsNumeric(params(2).Trim) Then
                Return CInt(params(2))
            Else
                Return 0
            End If
            l(" MOD getGISfromParams ende")
            Return result
        Catch ex As Exception
            l("Fehler in getGISfromParams: " & ex.ToString())
            Return 0
        End Try
    End Function

    Private Shared Sub handleLandschaftsplan(params() As String, os_tabelledef As clsTabellenDef)
        Dim Param, result, hinweis As String
        Try
            l(" MOD handleLandschaftsplan anfang")
            Param = "/buergergis/php/query_sachdaten.php?option=mapclick&aid=" & os_tabelledef.aid &
                  "&tab_nr=" + os_tabelledef.tab_nr + "&gid=" + params(2) + "&querytitel=Landschaftsplan"
            Param = serverWeb & Param
            result = meineHttpNet.meinHttpJob(ProxyString, Param, hinweis, myglobalz.enc, 5000)
            If result.IsNothingOrEmpty Then
            Else
                result = result.Trim
                Dim remotefile = serverWeb & result
                Dim zieldatei, zieldir, a() As String
                '/fkat/lp_bewertung1/l_k11-m25-b13.pdf
                a = result.Split("/"c)
                zieldatei = a(a.Count - 1)
                zieldir = IO.Path.Combine(strGlobals.localDocumentCacheRoot & "\landschaftsplan")
                Dim localfile = zieldir & "\" & zieldatei
                If clsSachdatentools.schonImCache(zieldir, zieldatei, True) Then
                    localfile = zieldir & "\" & zieldatei
                Else
                    If meineHttpNet.down(remotefile, zieldatei, zieldir) Then
                        localfile = zieldir & "\" & zieldatei
                    Else
                        l("Fehler in handleLandschaftsplan: ")
                    End If
                End If
                OpenDokument(zieldir & "\" & zieldatei)
            End If
            l(" MOD handleLandschaftsplan ende")
        Catch ex As Exception
            l("Fehler in handleLandschaftsplan: " & ex.ToString())
        End Try
    End Sub

    Private Shared Sub handleVorlage(params() As String, os_tabelledef As clsTabellenDef)
        Dim Param As String
        Try
            l(" MOD handleVorlage anfang")
            Select Case CInt(os_tabelledef.aid)
                Case 7
                    Param = "/buergergis/php/vorlage_amphibienkartierung.php?a=1&gid=" & params(2) &
                            "&querytitel=Amphibien-%20und%20Gew%E4sserkartierung%20(1991)"
                    Param = serverWeb & Param
                    Process.Start(Param)
                Case 225
                    Param = "/buergergis/php/vorlage_radwegweiser.php?a=1&gid=" & params(2) &
                           "&querytitel=Radwegweiser"
                    Param = serverWeb & Param
                    Process.Start(Param)
                Case 327
                    Param = "/buergergis/php/vorlage_wegweiserfoto_2009.php?a=1&gid=" & params(2) &
                                               "&querytitel=Radwegweiser Foto 2009"
                    Param = serverWeb & Param
                    Process.Start(Param)
                Case 328
                    Param = "/buergergis/php/vorlage_wegweiserfoto_2015.php?a=1&gid=" & params(2) &
                                               "&querytitel=Radwegweiser Foto 2015"
                    Param = serverWeb & Param
                    Process.Start(Param)
            End Select
            l(" MOD handleVorlage ende")
        Catch ex As Exception
            l("Fehler in handleVorlage: " & ex.ToString())
        End Try
    End Sub

    Private Shared Sub showKreisOffenbachHomepage(params() As String, os_tabelledef As clsTabellenDef)
        Dim sql, result, hinweis As String
        Try
            l(" MOD showKreisOffenbachHomepage anfang")

            sql = "select link from " & os_tabelledef.Schema & "." & os_tabelledef.tabelle & " where gid=" & params(2)

            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick &
                "&modus=getsql&sql=" & sql & "&dbname=postgis20"
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            If result.IsNothingOrEmpty Then
            Else
                result = result.Trim
                result = result.Replace("$", "")
                aufruf = "https://www.kreis-offenbach.de/adr.phtml?call=suche&FID=" & result
                Process.Start(aufruf)
            End If

            l(" MOD showKreisOffenbachHomepage ende")
        Catch ex As Exception
            l("Fehler in showKreisOffenbachHomepage: " & ex.ToString())
        End Try
    End Sub

    Private Shared Sub handleGIDsAuswahl(fangRadiusInMeter As Double, gids As List(Of Integer), fensterzaehler As Integer)
        Dim javascriptMimikry As String
        Dim buttonINfostringspecfunc As String = ""
        Try
            'If gids Is Nothing OrElse gids.Count = 0 Then
            '    handlejavascript(javascriptOriginalstring, 0)
            'End If
            'If UR_tabelledef.tabellen_anzeige.ToLower = "attributtabelle" Then
            '    os_tabelledef.tab_nr = "2"
            '    os_tabelledef.tabellen_anzeige = "attributtabelle"
            'End If
            If gids.Count = 1 Then
                os_tabelledef.gid = CType(gids(0), String)
                If CInt(os_tabelledef.gid) > 0 Then
                    javascriptMimikry = genjavascriptMimikry(os_tabelledef.aid, os_tabelledef.tab_nr, os_tabelledef.gid)
                    clsMiniMapTools.handlejavascript(javascriptMimikry, os_tabelledef, buttonINfostringspecfunc)
                    createRtfAndShowDialog(buttonINfostringspecfunc, layerActive.titel, fensterzaehler, isOSsuche:=True)
                    l(" dossierAkviveEbene ---------------------- ende")

                End If
            End If
            If gids.Count > 1 Then
                l(" mehr als eine gid")
                Dim auswahl As New winGIDauswahl(CInt(os_tabelledef.aid), gids,
                                                     os_tabelledef.Schema & ".os_" & os_tabelledef.tabelle,
                                                     layerActive.titel, fangradius_in_pixel, fangRadiusInMeter)
                auswahl.Show()
                'If auswahl.auswahl > 0 Then
                'handlejavascript(genjavascriptMimikry(os_tabelledef.aid,
                '                    os_tabelledef.tab_nr,
                '                    CType(auswahl.auswahl, String)))
                'End If
            End If
        Catch ex As Exception
            l("fehler in handleGIDsAuswahl:", ex)
        End Try
    End Sub

    Shared Function makeTabname(ByRef lokLayAct As clsLayerPres) As clsTabellenDef
        Dim loktabdef As New clsTabellenDef
        Try
            l(" MOD makeTabname anfang")
            'If lokLayAct.tabname.IsNothingOrEmpty Then
            loktabdef = New clsTabellenDef
            loktabdef.aid = CStr(lokLayAct.aid)
            loktabdef.gid = "0"
            loktabdef.datenbank = "postgis20"
            loktabdef.tab_nr = CType(1, String)
            loktabdef = ModsachdatenTools.getSChemaDB(lokLayAct.aid, 1)
            If loktabdef Is Nothing Then
                loktabdef = New clsTabellenDef
                loktabdef.aid = CStr(lokLayAct.aid)
                loktabdef.tab_nr = CType(1, String)
            End If
            loktabdef.datenbank = "postgis20"
            loktabdef.gid = "0"
            loktabdef.aid = CStr(lokLayAct.aid)
            lokLayAct.tabname = loktabdef.tabelle
            Return loktabdef
            'End If
            l(" MOD makeTabname ende")
            Return loktabdef
        Catch ex As Exception
            l("Fehler in makeTabname: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Shared Function makeUTM(KoordinateKLickpt As Point?) As Point
        Dim bbox As String = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt).Replace(" ", "")
        Dim a() As String = bbox.Split(","c)
        Dim utmpt As New Point
        utmpt.X = (CDbl(a(0).Replace(".", ",")))
        utmpt.Y = (CDbl(a(1).Replace(".", ",")))
        Return utmpt
    End Function

    Shared Function genjavascriptMimikry(aid As String, tabnr As String, gid As String) As String
        Return "javascript:Datenabfrage(" & aid & ", " &
                                                     tabnr &
                                                    ", " & gid & ")"
    End Function


    Shared Sub handlejavascript(javascriptOriginalstring As String, UR_tabellendef As clsTabellenDef, ByRef buttonINfostringspecfunc As String)
        'Dim istKlassischeHTMLTemplate As Boolean = False 
        Dim istklickbar As Boolean
        Dim featureclass As String = "", klassisch_gid As String = ""
        Dim strAktfs As String = ""
        Dim ergebnis As String = ""

        Try
            l(" handlejavascript ---------------------- anfang")
            If os_tabelledef.tabellen_anzeige = "attributtabelle" Then
                If os_tabelledef.gid = "0" Then
                    Dim result As Boolean = zerlegeJavascript(javascriptOriginalstring, aktObjID, buttonINfostringspecfunc,
                                                   akttabnr, klassisch_gid, strAktfs)
                    os_tabelledef.gid = aktObjID.ToString
                End If
                dbAttributtabelle(buttonINfostringspecfunc, ergebnis, isOSsuche:=False)
                Exit Sub
            End If
            If os_tabelledef.tabellen_anzeige = "schalter" Then
                Dim result As Boolean = zerlegeJavascript(javascriptOriginalstring, aktObjID, buttonINfostringspecfunc,
                                                   akttabnr, klassisch_gid, strAktfs)
                os_tabelledef.gid = aktObjID.ToString
                dbAttributtabelle(buttonINfostringspecfunc, ergebnis, isOSsuche:=False)
                Exit Sub
            End If
            istklickbar = getIstklickbar(javascriptOriginalstring)
            If istklickbar Then
                Dim result As Boolean = zerlegeJavascript(javascriptOriginalstring, aktObjID, buttonINfostringspecfunc,
                                               akttabnr, klassisch_gid, strAktfs)
                If layerActive.titel.ToLower.StartsWith("auswahl:") Then
                    ergebnis = getDBrecord(tempActivelayer.aid, CType(aktObjID, String), buttonINfostringspecfunc, akttabnr, strAktfs, "")
                Else
                    ergebnis = getDBrecord(layerActive.aid, CType(aktObjID, String), buttonINfostringspecfunc, akttabnr, strAktfs, "")
                End If
                nachricht("ergebnis:" & ergebnis)
                aktFS2aktFST_init(strAktfs)
            Else
                Exit Sub
            End If
            l(" handlejavascript ---------------------- ende")
        Catch ex As Exception
            l("Fehler in handlejavascript: " & ex.ToString())
        End Try
    End Sub

    Shared Sub dbAttributtabelle(ByRef buttonINfostringspecfunc As String, ByRef ergebnis As String, isOSsuche As Boolean)
        Dim linkTabs(), jumpGID, jumptabelle, sql, result As String
        Dim lokAid As Integer = 0
        Try
            l(" MOD dbAttributtabelle anfang")
            linkTabs = os_tabelledef.linkTabs.Split(","c)
            sql = "select " & linkTabs(0) & " from " & os_tabelledef.Schema & "." & os_tabelledef.tabelle &
                             " where gid=" & os_tabelledef.gid
            l("sql " & sql)
            result = clsSachdatentools.getOneValSQL("postgis20", sql) : l("result " & result)
            jumpGID = result.Trim
            jumptabelle = clsSachdatentools.getTabname4tabnr(CInt(os_tabelledef.aid), "2")
            If isOSsuche Then
                lokAid = CInt(os_tabelledef.aid)
            Else
                lokAid = layerActive.aid
            End If
            ergebnis = getDBrecord(lokAid, (jumpGID), buttonINfostringspecfunc, 2, "", os_tabelledef.tabellen_anzeige)
            nachricht("ergebnis:" & ergebnis)
            l(" MOD dbAttributtabelle ende")
        Catch ex As Exception
            l("Fehler in dbAttributtabelle: " & ex.ToString())
        End Try
    End Sub

    Shared Sub aktFS2aktFST_init(aktfs As String)
        If Not aktfs.IsNothingOrEmpty Then
            aktFST.normflst.FS = aktfs
            aktFST.normflst.splitFS(aktfs)
        End If
    End Sub

    'Shared Sub klassischeDBabfrage(aid As Integer, featureclass As String, gid As String)
    '    Dim Param As String
    '    Try
    '        l("---------------------- anfang")
    '        Param = "/buergergis/php/vorlage_amphibienkartierung.php?&gid=" & gid &
    '                "&querytitel=Amphibien-%20und%20Gew%E4sserkartierung%20(1991)"
    '        Param = serverWeb & Param
    '        Process.Start(Param)
    '        l(" - ---------------------ende")
    '    Catch ex As Exception
    '        l("Fehler In : " & ex.ToString())
    '    End Try
    'End Sub

    Shared Sub createRtfAndShowDialog(buttonINfostring As String, titel As String, fensterzaehler As Integer, isOSsuche As Boolean)
        'gesamtSachdatList = ModsachdatenTools.alleSpaltenOhneNrRausschmeissen(gesamtSachdatList => getsachdaten
        'If gesamtSachdatList Is Nothing Then => wird aussen abgefragt
        '    l("warnung in dbAbfrageDiaglogPrep:")
        '    Exit Sub
        'End If
        'Dim flowdoc As FlowDocument

        Dim hinweis As String = ""

        'Dim flowdoc As FlowDocument
        Dim htmltabelle As String = ""
        Try
            l("dbAbfrageDiaglogPrep---------------------- anfang")
            l(" isUserLayer: " & CBool(layerActive.aid = GisUser.userLayerAid))
            'flowdoc = nsMakeRTF.rtf.makeRtfDoku(gesamtSachdatList, titel, CType(aktObjID, String), layerActive.aid)
            htmltabelle = nsMakeHTML.clsCreateHtmlTable.createTable(gesamtSachdatList, titel, layerActive.aid, 10, 10, 10)
            nsMakeHTML.clsCreateHtmlTable.htmlDateiString = dbTemplateString.Replace("[DBINFO]", htmltabelle)
            Dim freiDB As winDBabfrage = New winDBabfrage(nsMakeHTML.clsCreateHtmlTable.htmlDateiString, "text", buttonINfostring, CBool(layerActive.aid = GisUser.userLayerAid),
                                                          fensterzaehler, isOSsuche)
            If buttonINfostring.ToLower.Contains("eigentümer") Then
                freiDB.Show() 'Dialog()
            Else

                freiDB.Show()
            End If
            If freiDB.Soll_refreshmap Then
                MsgBox("Bitte klicken Sie oben auf 'Karte auffrischen'")
            End If
            'freiDB = Nothing
            l("dbAbfrageDiaglogPrep---------------------- ende")
        Catch ex As Exception
            l("Fehler in dbAbfrageDiaglogPrep: " & ex.ToString())
        End Try
    End Sub

    Private Shared Function zerlegeJavascript(JSoriginal As String, ByRef aktobjId As Integer,
                                               ByRef buttonINfostringspecfunc As String, ByRef tabnr As Integer,
                                              ByRef gid As String, ByRef aktfs As String) As Boolean
        If String.IsNullOrEmpty(JSoriginal) Then Return False '"keine dbinfos"
        Dim params(), ergebnis As String
        Dim objektidValue As Integer = 0
        nachricht("dbabfrage-----------------")
        Try
            params = clsJavascript.isoliereCGIparameter(JSoriginal)

            objektidValue = CInt(params(2))
            aktobjId = CInt(params(2)) '?????? CInt(params(0))
            tabnr = getTabNrFromJavascript(params)

            If layerActive.aid < 1 Then
                If layerHgrund.isactive Then
                    layerActive.aid = layerHgrund.aid
                End If
            End If
            nachricht("original:" & JSoriginal)
            Return True
        Catch ex As Exception
            nachricht("fehler in dbabfrage-----------------jsoriginal: " & Environment.NewLine & JSoriginal & Environment.NewLine, ex)
            Return False '"fehler in dbabfrage"
        End Try
    End Function

    Private Shared Function getIstklickbar(jSoriginal As String) As Boolean
        'javascript:DatenabfragePunkt(17, 1, 7252)
        If jSoriginal.ToLower.Contains("javascript:") Then
            Return True
        Else
            Return False
        End If
    End Function

    'Private Shared Function getIstKlassische(jSoriginal As String) As Boolean
    '    Try
    '        l("istKlassische---------------------- anfang") 'Datenabfrage(7,'show_mydb_in_window
    '        If jSoriginal.ToLower.Contains("datenabfrage(7,") Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '        l("istKlassische---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in istKlassische: " & ex.ToString())
    '        Return False
    '    End Try
    'End Function

    Private Shared Function getTabNrFromJavascript(params() As String) As Integer
        Try
            l("getTabNrFromJavascript---------------------- anfang")
            If IsNumeric(params(1)) Then
                Return CInt(params(1))
            Else
                Return CInt(1)
            End If
            l("getTabNrFromJavascript---------------------- ende")
        Catch ex As Exception
            l("Fehler in getTabNrFromJavascript: ", ex)
            Return 1
        End Try


    End Function

    Shared Function getDBrecord(aid As Integer, objektidValue As String, ByRef buttonINfostringspecfunc As String,
                                        tabnr As Integer, ByRef aktfs As String, tabellen_anzeige As String) As String
        l("getDBrecord-------------" & aid & " " & objektidValue)
        Dim Fdaten1 As New clsTabellenDef
        Dim QuellTabelle As New clsTabellenDef
        Dim zusatzDatenListe As New List(Of clsTabellenDef)
        Dim result As String = ""
        Dim maskenObjektList As New List(Of MaskenObjekt)
        Dim sachdatList As New List(Of clsSachdaten)
        Dim sachdatListTabelle1 As New List(Of clsSachdaten)
        Try
            Dim anzahl_attributtabellen As Integer
            gesamtSachdatList.Clear()
            anzahl_attributtabellen = ModsachdatenTools.getAnzahlAttributtabellen(aid)
            'Dim fdatenalt As New clsTabellenDef
            'fdatenalt = fdatenalt.copyTabdef(Fdaten1)
            Fdaten1 = ModsachdatenTools.getSChemaDB(aid, tabnr)
            If Fdaten1 Is Nothing Then
                Fdaten1.aid = CType(aid, String)
                Fdaten1.tab_nr = CType(tabnr, String)
            End If
            Fdaten1.aid = CType(aid, String)
            Fdaten1.gid = CType(objektidValue, String)

            korrigiereTabellenSchemaFallsEintraegeFalschDB(Fdaten1)
            Dim linkTabs As String()
            linkTabs = Fdaten1.linkTabs.Split(","c)
            QuellTabelle = clsTabellenDef.copyTabdef(Fdaten1)
            'If Fdaten1.linkTabs.IsNothingOrEmpty Then
            '    linkTabs = fdatenalt.linkTabs.Split(","c) ' für 'schalter'
            'End If
            'teilisteerstellen
            sachdatListTabelle1 = ModsachdatenTools.getsachdaten(QuellTabelle, aktfs)
            If sachdatListTabelle1 Is Nothing Then
                l("fehler Keine Sachdaten gefunden")
                Return ""
            End If
            erzeugeTeilListeSachdaten(QuellTabelle, result, maskenObjektList, sachdatListTabelle1, buttonINfostringspecfunc)
            '-----------------------
            inGesamtListeKopieren(gesamtSachdatList, sachdatListTabelle1)

            If tabellen_anzeige = "attributtabelle" Then
                anzahl_attributtabellen = anzahl_attributtabellen - 1
            End If
            Dim istart = 2
            If tabellen_anzeige = "schalter" Then
                anzahl_attributtabellen = anzahl_attributtabellen
                istart = 3
            End If

            For i = istart To anzahl_attributtabellen
                QuellTabelle = New clsTabellenDef
                QuellTabelle = ModsachdatenTools.getSChemaDB(aid, i)
                If QuellTabelle Is Nothing Then
                    l("warnung in getDBrecord schleife i: " & i)
                    Continue For
                Else

                End If
                QuellTabelle.aid = CType(aid, String)
                QuellTabelle.tab_nr = CType(i, String)


                QuellTabelle.tab_id = linkTabs(i - 2) ' die quelle in der 1.tabelle
                QuellTabelle.gid = ModsachdatenTools.getLinkTab2ValueFrom(sachdatListTabelle1, QuellTabelle.tab_id)

                'teilisteerstellen
                sachdatList = ModsachdatenTools.getsachdaten(QuellTabelle, aktfs)
                If sachdatList Is Nothing Then
                    l("fehler Keine Sachdaten gefunden")
                    Return ""
                End If
                'tabellenHeader erzeugen
                Dim tabheader As New clsSachdaten
                tabheader.feldinhalt = QuellTabelle.tabtitel
                tabheader.feldname = "neueTabelle"
                tabheader.neuerFeldname = "NEUE_TABELLE"
                tabheader.nr = 1
                gesamtSachdatList.Add(tabheader)

                erzeugeTeilListeSachdaten(QuellTabelle, result, maskenObjektList, sachdatList, buttonINfostringspecfunc)
                inGesamtListeKopieren(gesamtSachdatList, sachdatList)
                If buttonINfostringspecfunc Is Nothing Then
                    buttonINfostringspecfunc = ""
                End If
                '-----------------------
            Next
            ' result = sachdatenTools.makeResultString(sachdatList)
            gesamtSachdatList = ModsachdatenTools.alleSpaltenOhneNrRausschmeissen(gesamtSachdatList)
            Return result
        Catch ex As Exception
            nachricht("fehler in getDBrecord-----------------original:", ex)
            Return ""
        End Try
    End Function

    Shared Function getDBrecord4menu(aid As Integer, objektidValue As Integer, ByRef buttonINfostring As String,
                                        tabelle As String, ByRef aktfs As String) As String
        l("getDBrecord-------------" & aid & " " & objektidValue)
        Dim Fdaten1 As New clsTabellenDef
        Dim tempDat As New clsTabellenDef
        Dim zusatzDatenListe As New List(Of clsTabellenDef)
        Dim result As String = ""
        Dim maskenObjektList As New List(Of MaskenObjekt)
        Dim sachdatList As New List(Of clsSachdaten)
        Dim sachdatListTabelle1 As New List(Of clsSachdaten)
        Try
            Dim anzahl_attributtabellen As Integer
            gesamtSachdatList.Clear()
            anzahl_attributtabellen = ModsachdatenTools.getAnzahlAttributtabellen(aid)
            Fdaten1.aid = CType(aid, String)
            Fdaten1.gid = CType(objektidValue, String)
            Fdaten1.tabelle = os_tabelledef.tabelle
            Fdaten1.tab_id = os_tabelledef.tab_id

            Fdaten1.Schema = os_tabelledef.Schema
            'Fdaten1.tab_nr = CType(1, String)
            'Fdaten1.tab_nr = CType(tabnr, String)
            'sachdatenTools.getSChema(Fdaten1)
            'korrigiereTabellenSchemaFallsEintraegeFalsch(Fdaten1)
            Dim linkTabs As String()
            linkTabs = Fdaten1.linkTabs.Split(","c)
            tempDat = clsTabellenDef.copyTabdef(Fdaten1)
            'teilisteerstellen
            sachdatListTabelle1 = ModsachdatenTools.getsachdaten(tempDat, aktfs)
            If sachdatListTabelle1 Is Nothing Then
                l("fehler Keine Sachdaten gefunden")
                Return ""
            End If


            erzeugeTeilListeSachdaten(tempDat, result, maskenObjektList, sachdatListTabelle1, buttonINfostring)
            '-----------------------
            inGesamtListeKopieren(gesamtSachdatList, sachdatListTabelle1)

            For i = 2 To anzahl_attributtabellen
                tempDat = New clsTabellenDef

                tempDat = ModsachdatenTools.getSChemaDB(aid, i)
                If tempDat Is Nothing Then
                    l("Fehler in getDBrecord i: " & i & "," & aid)
                    Continue For
                End If
                tempDat.aid = CType(aid, String)
                tempDat.tab_nr = CType(i, String)

                tempDat.tab_id = linkTabs(i - 2) ' die quelle in der 1.tabelle
                tempDat.gid = ModsachdatenTools.getLinkTab2ValueFrom(sachdatListTabelle1, tempDat.tab_id)

                'teilisteerstellen
                sachdatList = ModsachdatenTools.getsachdaten(tempDat, aktfs)
                If sachdatList Is Nothing Then
                    l("fehler Keine Sachdaten gefunden")
                    Return ""
                End If
                'tabellenHeader erzeugen
                Dim tabheader As New clsSachdaten
                tabheader.feldinhalt = tempDat.tabtitel
                tabheader.feldname = "neueTabelle"
                tabheader.neuerFeldname = "neueTabelle"
                tabheader.nr = 1
                gesamtSachdatList.Add(tabheader)

                erzeugeTeilListeSachdaten(tempDat, result, maskenObjektList, sachdatList, buttonINfostring)
                inGesamtListeKopieren(gesamtSachdatList, sachdatList)
                '-----------------------
            Next
            ' result = sachdatenTools.makeResultString(sachdatList)
            Return result
        Catch ex As Exception
            nachricht("fehler in getDBrecord-----------------original:", ex)
            Return ""
        End Try
    End Function

    Private Shared Sub inGesamtListeKopieren(Gesamtliste As List(Of clsSachdaten), sachdatList As List(Of clsSachdaten))
        For i = 0 To sachdatList.Count - 1
#If DEBUG Then
            If i > 29 Then
                Debug.Print("")
            End If
#End If
            Gesamtliste.Add(sachdatList(i))
        Next
    End Sub

    Private Shared Sub erzeugeTeilListeSachdaten(tempDat As clsTabellenDef, ByRef result As String, ByRef maskenObjektList As List(Of MaskenObjekt),
                                                 sachdatList As List(Of clsSachdaten),
                                                  ByRef buttonINfostringspecfunc As String)
        Dim ergaenz As New List(Of clsSachdaten)
        Try
            l("erzeugeTeilListeSachdaten ---------------------- anfang")
            maskenObjektList = ModsachdatenTools.getmaskenObjektList(tempDat)
            If maskenObjektList Is Nothing OrElse maskenObjektList.Count < 1 Then
                maskenObjektList = ModsachdatenTools.makeNotMaske(sachdatList)
            End If
            If maskenObjektList IsNot Nothing Then
                result = ModsachdatenTools.combineDatenAndDef(sachdatList, maskenObjektList, "#", tempDat, buttonINfostringspecfunc, ergaenz)
                '  ModsachdatenTools.entferneHTML(sachdatList)
                inGesamtListeKopieren(sachdatList, ergaenz)
                sachdatList.Sort()
            Else
                l("warnung erzeugeTeilListeSachdaten hat keine liste erzeugt")
            End If
            l("erzeugeTeilListeSachdaten---------------------- ende")
        Catch ex As Exception
            l("Fehler in erzeugeTeilListeSachdaten: " & ex.ToString())
        End Try

    End Sub

    ''' <summary>
    ''' punkte nach GK ueberführen
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function PolygonAufbereiten(ByVal polygonchen As clsParapolygon) As Boolean 'myGlobalz.sitzung.aktPolygon
        l("PolygonAufbereiten----------------------")
        Dim delim As String = ";"
        Dim gkstring As New Text.StringBuilder
        Dim dx, dy As Double
        Try
            l("PolygonAufbereiten---------------------- anfang")
            l("polygonchen.myPoly.Points.Count " & polygonchen.myPoly.Points.Count)
            If polygonchen.myPoly.Points.Count < 1 Then
                l("polygonchen.myPoly.Points.Count < 1  abbruch")
                Return False
            End If
            'gkstring generieren
            l("PolygonAufbereiten  vor schleife")
            For Each punkt As Point In polygonchen.myPoly.Points
                dx = CInt((clsMiniMapTools.MyPointVonCanvasNachGKumrechnen(punkt).X) * 100) / 100
                dy = CInt((clsMiniMapTools.MyPointVonCanvasNachGKumrechnen(punkt).Y) * 100) / 100
                l(dx & " " & dy)
                gkstring.Append(CDbl(dx) & delim & CDbl(dy) & delim)
            Next
            'anfangspunk nochmal an Ende wiederholen
            dx = CInt((clsMiniMapTools.MyPointVonCanvasNachGKumrechnen(polygonchen.myPoly.Points(0)).X) * 100) / 100
            dy = CInt((clsMiniMapTools.MyPointVonCanvasNachGKumrechnen(polygonchen.myPoly.Points(0)).Y) * 100) / 100
            gkstring.Append((dx) & delim & (dy) & delim)
            polygonchen.GKstring = gkstring.ToString

            l("polygonchen.GKstring:" & polygonchen.GKstring)


            Return True
            l("PolygonAufbereiten---------------------- ende")
        Catch ex As Exception
            l("Fehler in PolygonAufbereiten: " & ex.ToString())
            Return False
        End Try
    End Function
    Public Shared Sub GK_FlaecheErmitteln(aktPolygon As clsParapolygon)
        Dim newPoints As New PointCollection
        Dim a As String() = aktPolygon.GKstring.Split(";"c)
        For i = 0 To a.GetUpperBound(0) - 2 Step 2
            Dim np As New Point
            np.X = CDbl(a(i))
            np.Y = CDbl(a(i + 1))
            newPoints.Add(np)
        Next
        aktPolygon.Area = clsMiniMapTools.calc_area(newPoints)
        newPoints = Nothing
    End Sub
    Public Shared Function calc_area(ByVal ptColl As PointCollection) As Single
        'fläche berechnen
        Dim produkt As Double, summe As Double, i%
        Dim ysumme As Double, xsumme As Double
        Dim anzahl = ptColl.Count
        Dim ptA() As Point
        If ptColl Is Nothing Then
            nachricht("Fehler ptColl is nothing. Fläche kann nicht berechnet werden!")
            Return 0
        End If
        Try
            ptA = ptColl.ToArray
            ReDim Preserve ptA(ptA.Length)
            ptA(ptA.Length - 1).X = ptA(0).X
            ptA(ptA.Length - 1).Y = ptA(0).Y
            For i = 0 To ptA.GetUpperBound(0)
                MyPointVonCanvasNachGKumrechnen(ptA(i))
            Next
            nachricht("#################### calc_area  eingang")
            calc_area = -1
            summe = 0
            If anzahl > 2 Then
                For i = 0 To ptA.GetUpperBound(0) - 1
                    xsumme = ptA(i).X - ptA(i + 1).X
                    ysumme = ptA(i).Y + ptA(i + 1).Y
                    nachricht("## " & CStr(ptA(i).X & " " & ptA(i).Y))
                    produkt = ysumme * xsumme / 2
                    summe = summe + produkt
                Next i
                Return CSng(Math.Abs(summe))
            End If
            nachricht("#################### calc_area ausgang  " & CStr(calc_area))
            Return -1
        Catch ex As Exception
            nachricht("Fehler in calc_area: " & Environment.NewLine, ex)
            Return -2
        End Try
    End Function

    Public Shared Function MyPointVonCanvasNachGKumrechnen(ByVal ptA As Point) As myPoint
        Dim ptTemp, ptTemp2 As New myPoint
        ptTemp.X = ptA.X
        ptTemp.Y = ptA.Y
        ptTemp2 = clsAufrufgenerator.WINPOINTVonCanvasNachGKumrechnen(ptTemp, kartengen.aktMap.aktrange, kartengen.aktMap.aktcanvas)
        ptA.X = ptTemp2.X
        ptA.Y = ptTemp2.Y
        Return ptTemp2
    End Function
End Class
