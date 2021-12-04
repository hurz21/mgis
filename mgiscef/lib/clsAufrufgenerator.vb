
Public Class clsAufrufgenerator
    Shared Sub l(texyt As String)
        My.Log.WriteEntry(texyt)
    End Sub
    Shared Sub l(texyt As String, ex As Exception)
        My.Log.WriteEntry(texyt & Environment.NewLine & ex.ToString)
    End Sub
    Sub New(ByVal domainIN As String)
        Domainstring = domainIN
    End Sub

    Sub New()
        ' TODO: Complete member initialization 
    End Sub

    Public Property imagemapDateifullname() As String
    Public Property imageMap() As String
    Public Property gifKartenDateiFullName() As String

    'Private _aktrange As New clsRange
    'Public Property aktrange As clsRange
    '    Get
    '        Return _aktrange
    '    End Get
    '    Set(ByVal Value As clsRange)
    '        _aktrange = Value
    '    End Set
    'End Property
    Private _fitRaumbezuege As New clsRange
    Public Property FitRaumbezuege As clsRange
        Get
            Return _fitRaumbezuege
        End Get
        Set(ByVal Value As clsRange)
            _fitRaumbezuege = Value
        End Set
    End Property
    Private _fitGlobal As New clsRange
    Public Property FitGlobal As clsRange
        Get
            Return _fitGlobal
        End Get
        Set(ByVal Value As clsRange)
            _fitGlobal = Value
        End Set
    End Property
    Private _aktMap As New clsMapSpec
    Public Property aktMap As clsMapSpec
        Get
            Return _aktMap
        End Get
        Set(ByVal Value As clsMapSpec)
            _aktMap = Value
        End Set
    End Property
    Private _mapcred As New clsMapCredentials
    Public Property mapcred As clsMapCredentials
        Get
            Return _mapcred
        End Get
        Set(ByVal Value As clsMapCredentials)
            _mapcred = Value
        End Set
    End Property
    Public Property Domainstring As String
    Public Property gis_serverD() As String

    Shared Function bildeAufrufEinzelOS(os_tabelledef As clsTabellenDef, mapfile As String) As String
        Try
            If os_tabelledef.tabelle.IsNothingOrEmpty Or os_tabelledef.gid.IsNothingOrEmpty Then
                Return "fehler"
            End If
            Dim sb As New Text.StringBuilder
            'sb.Append(kartengen.Domainstring)

            'If myglobalz.getMapsFromInternet Then
            '    sb.Append(strGlobals.buergergisInternetServer)
            'Else
            sb.Append(kartengen.Domainstring)
            'If
            sb.Append("/cgi-bin/" & strGlobals.mapserverExeString & "?mode=map&map=")
            sb.Append(mapfile)
            sb.Append("&mapsize=" & kartengen.aktMap.aktcanvas.w & "+" & kartengen.aktMap.aktcanvas.h)
            sb.Append("&ts=" & Format(Now, "yyyyMMddhhmmssFFF"))
            sb.Append("&modes=" & "OS")

            sb.Append("&mapext=" &
                      CInt(kartengen.aktMap.aktrange.xl) & "+" &
                      CInt(kartengen.aktMap.aktrange.yl) & "+" &
                      CInt(kartengen.aktMap.aktrange.xh) & "+" &
                      CInt(kartengen.aktMap.aktrange.yh & "+"))
            sb.Append("&table=" + os_tabelledef.Schema + "." + os_tabelledef.tabelle + "&gid=" + os_tabelledef.gid)
            Dim a$ = sb.ToString
            ' l("clsaufrufgenerator: genaufruf " & a)
            a = sb.ToString
            ' l(String.Format("clsaufrufgenerator: genaufrufkomplett{0}{1}", vbCrLf, a))
            sb = Nothing
            Return a
        Catch ex As Exception
            nachricht("Fehler genaufruf: ", ex)
            Return "fehler"
        End Try
    End Function
    Public Property MapEngineAufruf As String = ""
    'Function genaufruf() As String
    '    Try

    '        If keineEbenenGewaehlt(aktMap.Hgrund, aktMap.Vgrund) Then
    '            MsgBox("Es wurden keine Ebenen zur Darstellung gewählt. Daher wird die Kreisgrenze aktiviert.", MsgBoxStyle.Information, "Paradigma-GIS Schnittstelle")
    '            aktMap.Vgrund = "kreis;"
    '        End If

    '        'aktMap.layer = combineHgrundVordergrund()
    '        Dim sb As New Text.StringBuilder
    '        sb.Append(Domainstring)
    '        '  sb.Append("/cgi-bin/apps/gis/mapgenparadigma/mapgen.cgi")
    '        sb.Append("/cgi-bin/apps/gis/mapgen/mapgen.cgi")
    '        sb.Append("?UserID=" & mapcred.username)
    '        sb.Append("&passwort=" & mapcred.pw)
    '        sb.Append("&schwanz=" & mapcred.DateinamensSchwanz)
    '        sb.Append("&W=" & aktMap.aktcanvas.w)
    '        sb.Append("&H=" & aktMap.aktcanvas.h)
    '        sb.Append("&XL=" & CInt(aktMap.aktrange.xl))
    '        sb.Append("&XH=" & CInt(aktMap.aktrange.xh))
    '        sb.Append("&YL=" & aktMap.aktrange.yl)
    '        sb.Append("&YH=" & aktMap.aktrange.yh)
    '        sb.Append("&layer=" & aktMap.layer)
    '        sb.Append("&activelayer=" & aktMap.ActiveLayer)
    '        sb.Append("&HGRUND=" & aktMap.Hgrund)




    '        Dim a$ = sb.ToString
    '        'l("clsaufrufgenerator: genaufruf " & a)

    '        Dim rest$ = "&Scale=1&Nr=0&INI=rheinmain" &
    '        "&TGT=MAP&TEMPL=&SLOTT=0&NOPIC=0&ANDERE=&SEL_FILTER=BSEL_FILTERD&SEL_ANDOR=BSEL_ANDORD" &
    '        "&SEL_FELD=SEL_FELD&AKTION=ZB&PDF=0&PDF_SCALE=0&PDF_ARROW=0&PDF_ICONLAYER=0&PDF_LEGEND=&PDF_DOKU=" &
    '        "&PDF_QUELLENNACHWEIS=&PDF_Bearbeiter=&PDF_Ortsteil=&PDF_Bemerkung=&PDF_FORMAT=&VHMODUS=1"

    '        sb.Append(rest)
    '        a = sb.ToString

    '        ' My.Log.WriteEntry(String.Format("clsaufrufgenerator: genaufrufkomplett{0}{1}", vbCrLf, a))
    '        'Dim a$ = serverweb & "/cgi-bin/apps/gis/mapgenparadigma/mapgen.cgi?passwort=60d5a617024915e22902dc99c7fc446b &
    '        '"&schwanz=testtt&user=USER&UserID=a670024" &
    '        '"&W=300&H=200&XL=3487623&YL=5552914&XH=3489433&YH=5554430&Scale=1&Nr=0&INI=rheinmain&activelayer=tk5&layer=realshapeopak;" &
    '        '"&HGRUND=tk5&TGT=MAP&TEMPL=register.htm&SLOTT=0&NOPIC=0&ANDERE=&SEL_FILTER=BSEL_FILTERD&SEL_ANDOR=BSEL_ANDORD" & 
    '        '"&SEL_FELD=SEL_FELD&AKTION=ZB&PDF=0&PDF_SCALE=0&PDF_ARROW=0&PDF_ICONLAYER=0&PDF_LEGEND=&PDF_DOKU=" &
    '        '"&PDF_QUELLENNACHWEIS=&PDF_Bearbeiter=&PDF_Ortsteil=&PDF_Bemerkung=&PDF_FORMAT=&VHMODUS=1"
    '        sb = Nothing
    '        Return a
    '    Catch ex As Exception
    '        nachricht("Fehler genaufruf: " ,ex)
    '        Return "Fehler"
    '    End Try
    'End Function
    'Function BildGenaufruf() As String 'BildGenaufrufMAPserver
    '    Try
    '        If keineEbenenGewaehlt(aktMap.Hgrund, aktMap.Vgrund) Then
    '            MsgBox("Es wurden keine Ebenen zur Darstellung gewählt. Daher wird die Kreisgrenze aktiviert.", MsgBoxStyle.Information, "Paradigma-GIS Schnittstelle")
    '            aktMap.Vgrund = "kreis;"
    '        End If
    '        Dim sb As New Text.StringBuilder
    '        sb.Append(Domainstring)
    '        sb.Append("/cgi-bin/apps/gis/mapgenMAP/mapgen.cgi")
    '        sb.Append("?UserID=" & mapcred.username)
    '        sb.Append("&passwort=" & mapcred.pw)
    '        sb.Append("&schwanz=" & mapcred.DateinamensSchwanz)
    '        sb.Append("&W=" & aktMap.aktcanvas.w)
    '        sb.Append("&H=" & aktMap.aktcanvas.h)
    '        sb.Append("&XL=" & CInt(aktMap.aktrange.xl))
    '        sb.Append("&XH=" & CInt(aktMap.aktrange.xh))
    '        sb.Append("&YL=" & aktMap.aktrange.yl)
    '        sb.Append("&YH=" & aktMap.aktrange.yh)
    '        sb.Append("&layer=" & aktMap.layer)
    '        sb.Append("&activelayer=" & aktMap.ActiveLayer)
    '        sb.Append("&HGRUND=" & aktMap.Hgrund)

    '        Dim a$ = sb.ToString
    '        'l("clsaufrufgenerator: genaufruf " & a)

    '        Dim rest$ = "&Scale=1&Nr=0&INI=rheinmain" &
    '        "&TGT=MAP&TEMPL=&SLOTT=0&NOPIC=0&ANDERE=&SEL_FILTER=BSEL_FILTERD&SEL_ANDOR=BSEL_ANDORD" &
    '        "&SEL_FELD=SEL_FELD&AKTION=ZB&PDF=0&PDF_SCALE=0&PDF_ARROW=0&PDF_ICONLAYER=0&PDF_LEGEND=&PDF_DOKU=" &
    '        "&PDF_QUELLENNACHWEIS=&PDF_Bearbeiter=&PDF_Ortsteil=&PDF_Bemerkung=&PDF_FORMAT=&VHMODUS=1"

    '        sb.Append(rest)
    '        a = sb.ToString

    '        'l(String.Format("clsaufrufgenerator: genaufrufkomplett{0}{1}", vbCrLf, a))
    '        ''Dim a$ = serverweb & "/cgi-bin/apps/gis/mapgenparadigma/mapgen.cgi?passwort=60d5a617024915e22902dc99c7fc446b &
    '        '"&schwanz=testtt&user=USER&UserID=a670024" &
    '        '"&W=300&H=200&XL=3487623&YL=5552914&XH=3489433&YH=5554430&Scale=1&Nr=0&INI=rheinmain&activelayer=tk5&layer=realshapeopak;" &
    '        '"&HGRUND=tk5&TGT=MAP&TEMPL=register.htm&SLOTT=0&NOPIC=0&ANDERE=&SEL_FILTER=BSEL_FILTERD&SEL_ANDOR=BSEL_ANDORD" & 
    '        '"&SEL_FELD=SEL_FELD&AKTION=ZB&PDF=0&PDF_SCALE=0&PDF_ARROW=0&PDF_ICONLAYER=0&PDF_LEGEND=&PDF_DOKU=" &
    '        '"&PDF_QUELLENNACHWEIS=&PDF_Bearbeiter=&PDF_Ortsteil=&PDF_Bemerkung=&PDF_FORMAT=&VHMODUS=1"
    '        sb = Nothing
    '        MapEngineAufruf = a
    '        Return a
    '    Catch ex As Exception
    '        nachricht("Fehler genaufruf: " ,ex)
    '        Return "Fehler"
    '    End Try
    'End Function
    Function ImapGenaufrufMAPserver(mapfile As String) As String
        Try
            'If keineEbenenGewaehlt(aktMap.Hgrund, aktMap.Vgrund) Then
            '    MsgBox("Es wurden keine Ebenen zur Darstellung gewählt. Daher wird die Kreisgrenze aktiviert.",
            '           MsgBoxStyle.Information,
            '           "Paradigma-GIS Schnittstelle")
            '    aktMap.Vgrund = "kreis;"
            'End If
            Dim sb As New Text.StringBuilder
            'If myglobalz.getMapsFromInternet Then
            '    sb.Append(strGlobals.buergergisInternetServer)
            'Else
            sb.Append(kartengen.Domainstring)
            'End If

            sb.Append("/cgi-bin/" & strGlobals.mapserverExeString & "?" &
                      "&mode=nquery&searchmap=true&map=")
            sb.Append(mapfile)
            sb.Append("&mapsize=" & aktMap.aktcanvas.w & "+" & aktMap.aktcanvas.h)

            sb.Append("&mapext=" &
                      CInt(aktMap.aktrange.xl) & "+" &
                      CInt(aktMap.aktrange.yl) & "+" &
                      CInt(aktMap.aktrange.xh) & "+" &
                      CInt(aktMap.aktrange.yh & "+"))
            Dim a$ = sb.ToString
            'l("clsaufrufgenerator: genaufruf " & a)
            a = sb.ToString

            'l(String.Format("clsaufrufgenerator: genaufrufkomplett{0}{1}", vbCrLf, a))
            'Dim a$ = serverweb & "/cgi-bin/apps/gis/mapgenparadigma/mapgen.cgi?passwort=60d5a617024915e22902dc99c7fc446b  
            sb = Nothing
            MapEngineAufruf = a
            Return a
        Catch ex As Exception
            nachricht("Fehler genaufruf: ", ex)
            Return "Fehler"
        End Try
    End Function

    Private Function genLayerstring() As String
        Dim lasy As String
        lasy = aktMap.Vgrund & ";" & aktMap.Hgrund & ";" & mapcred.username & ";"
        lasy = lasy.Replace(";;", ";")
        Return lasy
    End Function

    Function genaufruf4PDF(ortsteil As String,
                           bemerkung As String,
                           POST_PDFFROMSHP2IMG As String,
                           mitlegende As String,
                           mitdoku As String) As String
        Try
            Dim sb, pdfPart As New Text.StringBuilder
            sb.Append(Domainstring)
            sb.Append("/cgi-bin/apps/gis/mapgen/mapgen.cgi")
            sb.Append("?UserID=" & mapcred.username)
            sb.Append("&passwort=" & mapcred.pw)
            sb.Append("&schwanz=" & mapcred.DateinamensSchwanz)

            sb.Append("&XL=" & CInt(aktMap.aktrange.xl))
            sb.Append("&XH=" & CInt(aktMap.aktrange.xh))
            sb.Append("&YL=" & CInt(aktMap.aktrange.yl))
            sb.Append("&YH=" & CInt(aktMap.aktrange.yh))

            sb.Append("&layer=" & genLayerstring())
            sb.Append("&activelayer=")
            sb.Append("&HGRUND=" & aktMap.Hgrund)
            sb.Append("&Scale=1")
            sb.Append("&Nr=0")
            sb.Append("&INI=rheinmain")
            sb.Append("&TGT=MAP")
            sb.Append("&SLOTT=0")
            sb.Append("&AKTION=ZB")
            sb.Append("&VHMODUS=1")

            sb.Append("&TEMPL=schnellpdfParadigma.htm")


            If POST_PDFFROMSHP2IMG = "0" Then
                'klassische Methode
                pdfPart.Append("&PDF=1")
                pdfPart.Append("&PDF_SCALE=0")
                pdfPart.Append("&post_PDF_MERGE=0")
                pdfPart.Append("&PDF_Bearbeiter=" & mapcred.username)
                pdfPart.Append("&PDF_Ortsteil=" & ortsteil)
                pdfPart.Append("&PDF_Bemerkung=" & bemerkung)
                pdfPart.Append("&PDF_FORMAT=a4")
                pdfPart.Append("&POST_PDFQUERFORMAT=1")
                pdfPart.Append("&POST_PDFFROMSHP2IMG=" & POST_PDFFROMSHP2IMG)
                sb.Append("&W=" & CInt(((29.7 / 2.541) * 72) + 60))
                sb.Append("&H=" & CInt((21.0 / 2.541) * 72))

                pdfPart.Append("&PDF_ARROW=0")
                pdfPart.Append("&PDF_ICONLAYER=")
                pdfPart.Append("&PDF_LEGEND=" & mitlegende)
                pdfPart.Append("&PDF_DOKU=" & mitdoku)
                pdfPart.Append("&PDF_QUELLENNACHWEIS=")
            End If

            If POST_PDFFROMSHP2IMG = "1" Then
                pdfPart.Append("&PDF=1")
                pdfPart.Append("&PDF_SCALE=0")
                pdfPart.Append("&post_PDF_MERGE=0")
                pdfPart.Append("&PDF_Bearbeiter=" & mapcred.username)
                pdfPart.Append("&PDF_Ortsteil=" & ortsteil)
                pdfPart.Append("&PDF_Bemerkung=" & bemerkung)
                pdfPart.Append("&PDF_FORMAT=a4")
                pdfPart.Append("&POST_PDFQUERFORMAT=1")
                pdfPart.Append("&POST_PDFFROMSHP2IMG=" & POST_PDFFROMSHP2IMG)
                sb.Append("&W=" & CInt((29.7 / 2.541) * 72))
                sb.Append("&H=" & CInt((21.0 / 2.541) * 72))
            End If




            sb.Append(pdfPart.ToString)
            Dim a = sb.ToString

            'l(String.Format("clsaufrufgenerator: genaufrufkomplett{0}{1}", vbCrLf, a))
            'Dim a$ = serverweb & "/cgi-bin/apps/gis/mapgenparadigma/mapgen.cgi?passwort=60d5a617024915e22902dc99c7fc446b &
            '"&schwanz=testtt&user=USER&UserID=a670024" &
            '"&W=300&H=200&XL=3487623&YL=5552914&XH=3489433&YH=5554430&Scale=1&Nr=0&INI=rheinmain&activelayer=tk5&layer=realshapeopak;" &
            '"&HGRUND=tk5&TGT=MAP&TEMPL=register.htm&SLOTT=0&NOPIC=0&ANDERE=&SEL_FILTER=BSEL_FILTERD&SEL_ANDOR=BSEL_ANDORD" & 
            '"&SEL_FELD=SEL_FELD&AKTION=ZB&PDF=0&PDF_SCALE=0&PDF_ARROW=0&PDF_ICONLAYER=0&PDF_LEGEND=&PDF_DOKU=" &
            '"&PDF_QUELLENNACHWEIS=&PDF_Bearbeiter=&PDF_Ortsteil=&PDF_Bemerkung=&PDF_FORMAT=&VHMODUS=1"
            sb = Nothing
            Return a
        Catch ex As Exception
            nachricht("Fehler genaufruf: ", ex)
            Return "Fehler"
        End Try
    End Function
    Shared Sub nachricht(ByVal text$, ex As Exception)
        'MsgBox(text$)
        l(text & Environment.NewLine, ex)
    End Sub
    Shared Sub nachricht(ByVal text$)
        'MsgBox(text$)
        l(text)
    End Sub

    Sub nachricht_mbox(ByVal text$)
        MsgBox(text$)
        l(text)
    End Sub

    Function genOutfileFullName(ByVal cachedir As String, endung As String) As Boolean
        nachricht("genOutfileFullName ---------------------------" & cachedir.ToString)
        Try
            Dim layertext$ = "" ', endung = ".png"
            'If String.IsNullOrEmpty(aktMap.Vgrund) Then
            '    layertext$ = aktMap.Hgrund
            '    endung$ = ".png"
            'Else
            layertext$ = "merge" 'aktMap.Vordergrundebenen & ";" & aktMap.hgrund
            endung$ = "_.png"
            'End If
            Dim sb As New Text.StringBuilder
            sb.Append(cachedir)
            sb.Append(mapcred.username & "_")
            sb.Append(layertext$.Replace(";", "") & "_")
            sb.Append(mapcred.DateinamensSchwanz.Replace(";", ""))
            sb.Append(endung)
            '	Dim a = sb.ToString
            gifKartenDateiFullName = sb.ToString

            'sb = New Text.StringBuilder
            'sb.Append(cachedir$)
            'sb.Append(mapcred.username & "_")
            'sb.Append(layertext$.Replace(";", "") & "_")
            'sb.Append("0_")
            'sb.Append(mapcred.DateinamensSchwanz.Replace(";", ""))
            'sb.Append(".txt")
            'If Not String.IsNullOrEmpty(aktMap.ActiveLayer.Trim) AndAlso aktMap.ActiveLayer.Trim = layertext$.Replace(";", "") Then
            '    Debug.Print("un1")
            '    imagemapDateifullname = sb.ToString
            'Else
            '    Debug.Print("un2")
            '    imagemapDateifullname = ""
            'End If

            If Not String.IsNullOrEmpty(aktMap.ActiveLayer.Trim) Then
                sb = New Text.StringBuilder
                sb.Append(cachedir$)
                sb.Append(mapcred.username)
                ' sb.Append(aktMap.ActiveLayer.Trim.Replace(";", "") & "_")
                sb.Append("_merge_")
                sb.Append(mapcred.DateinamensSchwanz.Replace(";", ""))
                sb.Append(".txt")

                Debug.Print("un1")
                imagemapDateifullname = sb.ToString
            Else
                Debug.Print("un2")
                imagemapDateifullname = ""
            End If
            '  imagemapDateifullname = sb.ToString
            nachricht("genOutfileFullName ------------- ende --------------" & imagemapDateifullname)
            Return True
        Catch ex As Exception
            nachricht("Fehler genoutfilename: ", ex)
            Return False
        End Try
    End Function

    Public Shared Function istCacheOK(ByRef cachedir$) As Boolean
        l("istCacheOK: ---------------------- ")
        Try
            Dim cachedirtest As New IO.DirectoryInfo(cachedir)
            If Not cachedirtest.Exists Then
                MsgBox("Das Ausgabeverzeichnis ist nicht vorhanden. Die Minimap kann nicht dargestellt werden. Abbruch.")
                Return False
            End If
            Return True
        Catch ex As Exception
            nachricht("Fehler istcacheok: ", ex)
            Return False
        End Try
    End Function

    Public Shared Function WINPOINTVonCanvasNachGKumrechnen(ByVal aktpoint As myPoint, ByVal birdsrange As clsRange, ByVal Kreiscanvas As clsCanvas) As myPoint
        Try
            Dim testr As Double, testh As Double
            testr = ((aktpoint.X * (birdsrange.xdif)) / Kreiscanvas.w) + birdsrange.xl
            testh = ((((Kreiscanvas.h - aktpoint.Y) * (birdsrange.ydif)) / Kreiscanvas.h)) + birdsrange.yl
            Dim neupoint As New myPoint() With {.X = testr, .Y = testh, .strX = CInt(testr).ToString, .strY = CInt(testh).ToString}
            Return neupoint
        Catch ex As Exception
            nachricht("Fehler WINPOINTVonCanvasNachGKumrechnen: ", ex)
            Return Nothing
        End Try
    End Function

    Private Function keineEbenenGewaehlt(hgrund As String, vgrund As String) As Boolean
        hgrund = hgrund.Trim.Replace(";", "").Trim
        vgrund = vgrund.Trim.Replace(";", "").Trim

        If String.IsNullOrEmpty((hgrund & vgrund).Trim) Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
