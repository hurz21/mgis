Public Class clsMiniMapTools
    'kurzversion
    Public Shared Function punktvonGKnachCanvasUmrechnen(ByVal aktpointInMeter As myPoint, ByVal birdsrangeInMeter As clsRange,
                                                         ByVal KreiscanvasInPixel As clsCanvas,
                                                         Optional divisor As Integer = 100) As myPoint
        Dim testr As Double, testh As Double
        Dim neupoint As New myPoint
        Try

            nachricht("divisor:" & divisor)
            nachricht("aktpoint.X:" & aktpointInMeter.X)
            nachricht("aktpoint.y:" & aktpointInMeter.Y)
            nachricht("divisor:" & divisor)
            nachricht("divisor:" & divisor)
            nachricht("birdsrangeInMeter.xdif:" & birdsrangeInMeter.xdif)
            nachricht("birdsrangeInMeter.ydif:" & birdsrangeInMeter.ydif)
            testr = ((aktpointInMeter.X / divisor) - birdsrangeInMeter.xl) / birdsrangeInMeter.xdif
            testr = testr * KreiscanvasInPixel.w
            testh = ((aktpointInMeter.Y / divisor) - birdsrangeInMeter.yl) / birdsrangeInMeter.ydif
            testh = KreiscanvasInPixel.h - (testh * KreiscanvasInPixel.h)
            testr = Fix(testr)
            testh = Fix(testh)
            neupoint.X = CInt(testr)
            neupoint.Y = CInt(testh)
            Return neupoint
        Catch ex As Exception
            nachricht("fehler: " & ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Shared Function polygonNachCanvasUmrechnen(punktarrayGK As myPoint(),
                                                       ByVal birdsrange As clsRange,
                                                       ByVal Kreiscanvas As clsCanvas) As myPoint()
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
            nachricht("Fehler in polygonNachCanvasUmrechnen:" & ex.ToString)
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
            nachricht("Fehler in zerlegeInPunkte (" & rid & "):" & " meinpointer: " & meinpointer & ": " & gkstring & ex.ToString)
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
            nachricht("fehler in koords2PointArray: (rid: " & rid & ")" & ex.ToString)
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
            nachricht("Fehler in bildeTeilFlaechenPointer:" & ex.ToString)
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
            nachricht("Fehler in bildeNurKoordinatenArray:" & ex.ToString)
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
            nachricht("Fehler in leereFelderAbschneiden_:" & ex.ToString)
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
            nachricht("Fehler in getKoordinatenstart: " & ex.ToString)
            Return -1
        End Try
    End Function

    Friend Shared Sub Polygon_MouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        Dim aktPolygon As System.Windows.Shapes.Polygon = DirectCast(e.Source, System.Windows.Shapes.Polygon)
        Mouse.Capture(Nothing)
        Dim KoordinateKLickpt As Point?
        KoordinateKLickpt = e.GetPosition(aktPolygon)

        Dim javascriptOriginalstring As String = ""
        Dim fangRadiusInMeter As Double

        javascriptOriginalstring = aktPolygon.Tag.ToString
        l("javascriptOriginalstring " & javascriptOriginalstring)

        Dim utmpt As Point = makeUTM(KoordinateKLickpt)
        makeTabname()
        fangRadiusInMeter = clsSachdatentools.calcFangradiusM(globCanvasWidth, myglobalz.fangradius_in_pixel,
                              kartengen.aktMap.aktrange.xdif, layerActive.tabname)
        'FS feststellen
        aktFST.clear()
        aktFST.punkt.X = utmpt.X
        aktFST.punkt.Y = utmpt.Y
        aktFST.normflst.FS = pgisTools.getFS4UTM(utmpt)
        aktFST.normflst.splitFS(aktFST.normflst.FS)
        aktFST.abstract = aktFST.normflst.gemarkungstext & ", Flur: " & aktFST.normflst.flur & ", Fst: " & aktFST.normflst.fstueckKombi

        'clsFSTtools.holeKoordinaten4Flurstueck(tbNenner.Text)
        'getSerialFromPostgis(aktFST.normflst.FS) ' setzt  aktFST.serial  

        Dim jjj As New winImapMenue(utmpt.X & ", " & utmpt.Y, aktFST.abstract, layerActive.titel)
        jjj.ShowDialog()
        Dim auswahl = jjj.auswahl
        Select Case auswahl
            Case "punkt"
                handleMouseDownImagemap(KoordinateKLickpt, aktPolygon.Tag.ToString)
            Case "fs"
                verschneideFSmitAktiverEbene(aktFST.normflst.FS, layerActive.aid, javascriptOriginalstring, fangRadiusInMeter)
                'setBoundingRefresh(kartengen.aktMap.aktrange)
                'suchObjektModus = "fst"
            Case "dossier"
                clsFSTtools.dossierPrepMinimum()
        End Select
    End Sub

    Private Shared Sub verschneideFSmitAktiverEbene(fS As String,
                                                    aid As Integer,
                                                    javascriptOriginalstring As String,
                                                    fangradiusinmeter As Double)
        Try
            l(" verschneideFSmitAktiverEbene ---------------------- anfang")
            Dim gids() As Integer
            gids = clsSachdatentools.fsMitAktiveEbene(aid, fS)
            handleGIDsAuswahl(javascriptOriginalstring, fangradiusinmeter, gids)
            l(" verschneideFSmitAktiverEbene ---------------------- ende")
        Catch ex As Exception
            l("Fehler in verschneideFSmitAktiverEbene: " & ex.ToString())
        End Try
    End Sub

    Public Shared Sub Polygon_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        e.Handled = True
        Dim aktPolygon As System.Windows.Shapes.Polygon = DirectCast(e.Source, System.Windows.Shapes.Polygon)
        Mouse.Capture(Nothing)
        handleMouseDownImagemap(e.GetPosition(aktPolygon), CType(aktPolygon.Tag, String))
    End Sub

    Shared Sub handleMouseDownImagemap(KoordinateKLickpt As Point?, aktPolygonTag As String)
        Dim javascriptOriginalstring As String = ""
        Dim newtechno As Boolean = True
        Dim fangRadiusInMeter As Double
        Dim gids As Integer()

        javascriptOriginalstring = aktPolygonTag.ToString
        If javascriptOriginalstring.Trim.IsNothingOrEmpty Then
            l("handleMouseDownImagemap kein javascripttag, keine dbabfrage. abbruch")
            Exit Sub
        End If
        l("javascriptOriginalstring " & javascriptOriginalstring)
        'If newtechno Then
        Dim utmpt As Point = makeUTM(KoordinateKLickpt)
        makeTabname()

        fangRadiusInMeter = clsSachdatentools.calcFangradiusM(
                                           globCanvasWidth, myglobalz.fangradius_in_pixel,
                                          kartengen.aktMap.aktrange.xdif, layerActive.tabname)
        gids = clsSachdatentools.getActiveLayer4point(utmpt, layerActive.aid,
                                                        globCanvasWidth, globCanvasHeight,
                                                        KoordinateKLickpt,
                                                        fangRadiusInMeter)
        handleGIDsAuswahl(javascriptOriginalstring, fangRadiusInMeter, gids)
    End Sub

    Private Shared Sub handleGIDsAuswahl(javascriptOriginalstring As String, fangRadiusInMeter As Double, gids() As Integer)
        Dim javascriptMimikry As String
        If gids Is Nothing OrElse gids.Length = 0 Then
            handlejavascript(javascriptOriginalstring)
        End If
        If gids.Length = 1 Then
            os_tabelledef.gid = CType(gids(0), String)
            If CInt(os_tabelledef.gid) > 0 Then
                javascriptMimikry = genjavascriptMimikry(os_tabelledef.aid, os_tabelledef.tab_nr, os_tabelledef.gid)
                clsMiniMapTools.handlejavascript(javascriptMimikry)
                l(" dossierAkviveEbene ---------------------- ende")
            End If
        End If
        If gids.Length > 1 Then
            l(" mehr als eine gid")
            Dim auswahl As New winGIDauswahl(CInt(os_tabelledef.aid), gids,
                                             os_tabelledef.Schema & ".os_" & os_tabelledef.tabelle,
                                             layerActive.titel, fangradius_in_pixel, fangRadiusInMeter)
            auswahl.ShowDialog()
            If auswahl.auswahl > 0 Then
                handlejavascript(genjavascriptMimikry(os_tabelledef.aid,
                                                      os_tabelledef.tab_nr,
                                                      CType(auswahl.auswahl, String)))
            End If
        End If
    End Sub

    Shared Sub makeTabname()
        If layerActive.tabname.IsNothingOrEmpty Then
            os_tabelledef = New clsTabellenDef
            os_tabelledef.aid = CStr(layerActive.aid)
            os_tabelledef.gid = "0"
            os_tabelledef.datenbank = "postgis20"
            os_tabelledef.tab_nr = CType(1, String)
            sachdatenTools.getSChema(os_tabelledef)
            layerActive.tabname = os_tabelledef.tabelle
        End If
    End Sub

    Shared Function makeUTM(KoordinateKLickpt As Point?) As Point
        Dim bbox As String = clsToolsAllg.koordinateKlickBerechnen(KoordinateKLickpt).Replace(" ", "")
        Dim a() As String = bbox.Split(","c)
        Dim utmpt As New Point
        utmpt.X = (CDbl(a(0).Replace(".", ",")))
        utmpt.Y = (CDbl(a(1).Replace(".", ",")))
        Return utmpt
    End Function

    Private Shared Function genjavascriptMimikry(aid As String, tabnr As String, gid As String) As String
        Return "javascript:Datenabfrage(" & aid & ", " &
                                                     tabnr &
                                                    ", " & gid & ")"
    End Function


    Shared Sub handlejavascript(javascriptOriginalstring As String)
        Dim istKlassischeHTMLTemplate As Boolean = False
        Dim buttonINfostring As String = ""
        Dim istklickbar As Boolean
        Dim featureclass As String, klassisch_gid As String
        Dim strAktfs As String = ""
        Dim result As String = ""
        Try
            l(" handlejavascript ---------------------- anfang")
            result = execDB4objabfrage(javascriptOriginalstring, aktObjID, istklickbar, buttonINfostring,
                                                 akttabnr, istKlassischeHTMLTemplate,
                                                 featureclass, klassisch_gid, strAktfs)
            aktFS2aktFST_init(strAktfs)
            If Not istklickbar Then
                'kein dbabfrage
                Exit Sub
            End If
            If istKlassischeHTMLTemplate Then
                ' htmlTemplate = "MSKamphibienarten.htm" ' "MSKamphibien_2.htm"
                Dim aid = aktObjID
                klassischeDBabfrage(aid, featureclass, klassisch_gid)
            Else
                dbAbfrageDiaglogPrep(buttonINfostring, layerActive.titel)
            End If
            l(" handlejavascript ---------------------- ende")
        Catch ex As Exception
            l("Fehler in handlejavascript: " & ex.ToString())
        End Try
    End Sub



    Shared Sub aktFS2aktFST_init(aktfs As String)
        If Not aktfs.IsNothingOrEmpty Then
            aktFST.normflst.FS = aktfs
            aktFST.normflst.splitFS(aktfs)
        End If
    End Sub

    Shared Sub klassischeDBabfrage(aid As Integer, featureclass As String, gid As String)
        Dim Param As String
        Try
            l("---------------------- anfang")
            'Param = "/cgi-bin/apps/gis/getrecord/getrecord4template.cgi"
            'Param = Param & "?lookup=" + "true"
            'Param = Param & "&aktive_ebene=" + featureclass
            'Param = Param & "&object_id=" & aid
            'Param = Param & "&templatefile=" + gid
            'Param = Param & "&activelayer=" + featureclass
            'Param = Param & "&apppfad=/profile/register/"
            Param = "/buergergis/php/vorlage_amphibienkartierung.php?&gid=" & gid &
                    "&querytitel=Amphibien-%20und%20Gew%E4sserkartierung%20(1991)"
            Param = serverWeb & Param
            Process.Start(Param)
            l(" - ---------------------ende")
        Catch ex As Exception
            l("Fehler In : " & ex.ToString())
        End Try
    End Sub

    Shared Sub dbAbfrageDiaglogPrep(buttonINfostring As String, titel As String)
        gesamtSachdatList = sachdatenTools.alleSpaltenOhneNrRausschmeissen(gesamtSachdatList)
        If gesamtSachdatList Is Nothing Then
            l("warnung in dbAbfrageDiaglogPrep:")
            Exit Sub
        End If
        Dim flowdoc As FlowDocument
        Dim isUserLayer As Boolean = False
        Dim freiDB As winDBabfrage
        Try
            flowdoc = nsMakeRTF.rtf.makeRtfDoku(gesamtSachdatList, titel, CType(aktObjID, String))
            l("dbAbfrageDiaglogPrep---------------------- anfang")
            isUserLayer = layerActive.aid = GisUser.userLayerAid
            freiDB = New winDBabfrage("", "text", buttonINfostring, isUserLayer, flowdoc)
            freiDB.Show()
            If freiDB.Soll_refreshmap Then
                MsgBox("Bitte klicken Sie oben auf 'Karte auffrischen'")
            End If
            freiDB = Nothing
            l("dbAbfrageDiaglogPrep---------------------- ende")
        Catch ex As Exception
            l("Fehler in dbAbfrageDiaglogPrep: " & ex.ToString())
        End Try
    End Sub

    Private Shared Function execDB4objabfrage(JSoriginal As String, ByRef aid As Integer, ByRef istklickbar As Boolean,
                                               ByRef buttonINfostring As String, ByRef tabnr As Integer,
                                             ByRef istKlassischeHTMLTemplate As Boolean, ByRef featureclass As String,
                                              ByRef gid As String, ByRef aktfs As String) As String
        If String.IsNullOrEmpty(JSoriginal) Then Return "keine dbinfos"
        Dim params As String()
        Dim objektidValue As Integer = 0
        nachricht("dbabfrage-----------------")
        Try
            istKlassischeHTMLTemplate = getIstKlassische(JSoriginal)
            istklickbar = getIstklickbar(JSoriginal)
            params = clsJavascript.isoliereCGIparameter(JSoriginal)
            If Not istklickbar Then
                Return ""
            End If
            Dim ergebnis As String
            If istKlassischeHTMLTemplate Then
                aid = CInt(params(0))
                featureclass = params(1)
                gid = params(2)
                Return "klassisch"
            Else
                objektidValue = CInt(params(2))
            End If
            aid = CInt(params(2))
            tabnr = getTabNrFromJavascript(params)

            If layerActive.aid < 1 Then
                If layerHgrund.isactive Then
                    layerActive.aid = layerHgrund.aid
                End If
            End If
            ergebnis = getDBrecord(layerActive.aid, objektidValue, buttonINfostring, tabnr, aktfs)

            nachricht("original:" & JSoriginal)
            nachricht("ergebnis:" & ergebnis)
            Return ergebnis
        Catch ex As Exception
            nachricht("fehler in dbabfrage-----------------jsoriginal: " & Environment.NewLine & JSoriginal & Environment.NewLine & ex.ToString)
            Return "fehler in dbabfrage"
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

    Private Shared Function getIstKlassische(jSoriginal As String) As Boolean
        Try
            l("istKlassische---------------------- anfang") 'Datenabfrage(7,'show_mydb_in_window
            If jSoriginal.ToLower.Contains("datenabfrage(7,") Then
                Return True
            Else
                Return False
            End If
            l("istKlassische---------------------- ende")
        Catch ex As Exception
            l("Fehler in istKlassische: " & ex.ToString())
            Return False
        End Try
    End Function

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

    Shared Function getDBrecord(aid As Integer, objektidValue As Integer, ByRef buttonINfostring As String,
                                        tabnr As Integer, ByRef aktfs As String) As String
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
            anzahl_attributtabellen = sachdatenTools.getAnzahlAttributtabellen(aid)
            Fdaten1.aid = CType(aid, String)
            Fdaten1.gid = CType(objektidValue, String)
            'Fdaten1.tab_nr = CType(1, String)
            Fdaten1.tab_nr = CType(tabnr, String)
            sachdatenTools.getSChema(Fdaten1)
            korrigiereTabellenSchemaFallsEintraegeFalsch(Fdaten1)
            Dim linkTabs As String()
            linkTabs = Fdaten1.linkTabs.Split(","c)
            tempDat = clsTabellenDef.copyTabdef(Fdaten1)
            'teilisteerstellen
            sachdatListTabelle1 = sachdatenTools.getsachdaten(tempDat, aktfs)
            If sachdatListTabelle1 Is Nothing Then
                l("fehler Keine Sachdaten gefunden")
                Return ""
            End If


            erzeugeTeilListeSachdaten(tempDat, result, maskenObjektList, sachdatListTabelle1, buttonINfostring)
            '-----------------------
            inGesamtListeKopieren(sachdatListTabelle1)

            For i = 2 To anzahl_attributtabellen
                tempDat = New clsTabellenDef
                tempDat.aid = CType(aid, String)
                tempDat.tab_nr = CType(i, String)
                sachdatenTools.getSChema(tempDat)
                tempDat.tab_id = linkTabs(i - 2) ' die quelle in der 1.tabelle
                tempDat.gid = sachdatenTools.getLinkTab2ValueFrom(sachdatListTabelle1, tempDat.tab_id)

                'teilisteerstellen
                sachdatList = sachdatenTools.getsachdaten(tempDat, aktfs)
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
                inGesamtListeKopieren(sachdatList)
                '-----------------------
            Next
            ' result = sachdatenTools.makeResultString(sachdatList)
            Return result
        Catch ex As Exception
            nachricht("fehler in getDBrecord-----------------original:" & ex.ToString)
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
            anzahl_attributtabellen = sachdatenTools.getAnzahlAttributtabellen(aid)
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
            sachdatListTabelle1 = sachdatenTools.getsachdaten(tempDat, aktfs)
            If sachdatListTabelle1 Is Nothing Then
                l("fehler Keine Sachdaten gefunden")
                Return ""
            End If


            erzeugeTeilListeSachdaten(tempDat, result, maskenObjektList, sachdatListTabelle1, buttonINfostring)
            '-----------------------
            inGesamtListeKopieren(sachdatListTabelle1)

            For i = 2 To anzahl_attributtabellen
                tempDat = New clsTabellenDef
                tempDat.aid = CType(aid, String)
                tempDat.tab_nr = CType(i, String)
                sachdatenTools.getSChema(tempDat)
                tempDat.tab_id = linkTabs(i - 2) ' die quelle in der 1.tabelle
                tempDat.gid = sachdatenTools.getLinkTab2ValueFrom(sachdatListTabelle1, tempDat.tab_id)

                'teilisteerstellen
                sachdatList = sachdatenTools.getsachdaten(tempDat, aktfs)
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
                inGesamtListeKopieren(sachdatList)
                '-----------------------
            Next
            ' result = sachdatenTools.makeResultString(sachdatList)
            Return result
        Catch ex As Exception
            nachricht("fehler in getDBrecord-----------------original:" & ex.ToString)
            Return ""
        End Try
    End Function

    Private Shared Sub inGesamtListeKopieren(sachdatList As List(Of clsSachdaten))
        For i = 0 To sachdatList.Count - 1
#If DEBUG Then
            If i > 29 Then
                Debug.Print("")
            End If
#End If
            gesamtSachdatList.Add(sachdatList(i))
        Next
    End Sub

    Private Shared Sub erzeugeTeilListeSachdaten(tempDat As clsTabellenDef, ByRef result As String, ByRef maskenObjektList As List(Of MaskenObjekt),
                                                 sachdatList As List(Of clsSachdaten),
                                                  ByRef buttonINfostring As String)
        Try
            l("erzeugeTeilListeSachdaten ---------------------- anfang")
            maskenObjektList = sachdatenTools.getmaskenObjektList(tempDat)
            If maskenObjektList Is Nothing OrElse maskenObjektList.Count < 1 Then
                maskenObjektList = sachdatenTools.makeNotMaske(sachdatList)
            End If
            If maskenObjektList IsNot Nothing Then
                result = sachdatenTools.combineDatenAndDef(sachdatList, maskenObjektList, "#", tempDat, buttonINfostring)
                sachdatenTools.entferneHTML(sachdatList)
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
            nachricht("Fehler in calc_area: " & Environment.NewLine & ex.ToString)
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
