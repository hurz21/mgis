Imports System.Data
Imports mgis

Module modLayer
    'Public layerSelected As New clsLayerPres
    Public layerActive As New clsLayerPres
    Public layerHgrund As New clsLayerPres
    Public layersSelected As New List(Of clsLayerPres)
    Public layersSelectedOld As New List(Of clsLayerPres) ' für die checkbox als tempSpeicher
    Public layersFavorites As New List(Of clsLayerPres)
    Public layersTemp As New List(Of clsLayerPres)
    '
    Public mapfileCachePathroot As String = serverUNC & "cache\mapfiles\"

    Public mapfileIMAP As String = ""
    Public mapfileBILD As String = ""
    Public mapfileBILDrank0 As String = ""
    'Public collLayerPres As New List(Of clsLayerPres)

    Public erzeugeImagemap As Boolean = True

    Friend Function getCompleteLayers() As List(Of clsLayerPres)
        l("fehler : keine Standardebenen vorhanden???")
        If layersSelected.Count < 1 Then
            Return Nothing
        Else
            For Each preslayer As clsLayerPres In layersSelected
                pgisTools.getStamm4aid(preslayer)
                preslayer.mithaken = True
            Next
            Return layersSelected
        End If
    End Function

    Friend Function getStandardlayersAids() As List(Of clsLayerPres)
        Dim tlist As New List(Of clsLayerPres)
        Dim nlayer As New clsLayerPres
        'nlayer.aid = 210
        'tlist.Add(nlayer)
        nlayer = New clsLayerPres
        nlayer.aid = 21
        nlayer.isactive = True
        nlayer.RBischecked = True
        tlist.Add(nlayer)
        Return tlist
    End Function

    'Friend Sub createMapfileVG(ByRef layersUsed4Controlling As Integer,
    '                           layersNachRangSortiert As List(Of clsLayerPres),
    '                           normMapfileHeader As String)

    'End Sub

    Function CreateVGMapfileString(ByRef layersUsed4Controlling As Integer, layersNachRangSortiert As List(Of clsLayerPres),
                                           normMapfileHeader As String) As String
        ' /fkat/flurkarte/flurkarte2016/flurkarte2016_header.map"
        'MAP
        'INCLUDE '/inetpub/wwwroot/buergergis/mapfile/header.map',
        'INCLUDE '/fkat/boden/bodentyp/bodentyp_layer.map',,
        'End
        Dim sb As New Text.StringBuilder
        Try
            sb.AppendLine("MAP")
            'sb.AppendLine("INCLUDE '/inetpub/wwwroot/buergergis/mapfile/header.map'")
            sb.AppendLine("INCLUDE " & normMapfileHeader & "")
            For Each nlayer As clsLayerPres In layersNachRangSortiert
#If DEBUG Then
                If nlayer.aid = 45 Then
                    Debug.Print("")
                End If
#End If
                If nlayer.mithaken Then
                    sb.AppendLine("INCLUDE '" & nlayer.mapFile & "'")
                    sb.AppendLine(" ")
                    layersUsed4Controlling += 1
                End If
            Next
            sb.AppendLine("End")
            Dim summe As String = sb.ToString
            l("createMapfileVG Vor dem ausschreiben")
            Return summe
        Catch ex As Exception
            l("fehler in createMapfileVG ", ex)
            Return ""
        End Try
    End Function

    Function sortiereLayers(lliste As List(Of clsLayerPres)) As List(Of clsLayerPres)
        Dim layersNachRangSortiert As New List(Of clsLayerPres)
        layersNachRangSortiert = layersselected_copieren(lliste)
        ' If layerHgrund.aid > 0 Then emplayers.Add(layerHgrund)
        layersNachRangSortiert = layersSelectedNachRangOrdnen(layersNachRangSortiert)
        Return layersNachRangSortiert
    End Function

    Private Function layersselected_copieren(layersSelected As List(Of clsLayerPres)) As List(Of clsLayerPres)
        Dim emp As New List(Of clsLayerPres)
        For Each lay As clsLayerPres In layersSelected
            emp.Add(lay)
        Next
        Return emp
    End Function

    Friend Function createMapfilePDF(paintPNG As Boolean, hochaufloesend As Boolean, mitsuchobjekt As Boolean) As String
        ' /fkat/flurkarte/flurkarte2016/flurkarte2016_header.map"
        'MAP
        'INCLUDE '/inetpub/wwwroot/buergergis/mapfile/header.map',
        'INCLUDE '/fkat/boden/bodentyp/bodentyp_layer.map',,
        'End
        l("in createMapfilePDF--------------------------")
        Try
            Dim sb As New Text.StringBuilder
            sb.AppendLine("MAP")
            If paintPNG Or hochaufloesend Then
                sb.AppendLine("INCLUDE " & strGlobals.PNGdruck_MapFileHeaderr)
            Else
                sb.AppendLine("INCLUDE " & strGlobals.PDFdruck_MapFileHeaderr)
            End If
            Dim emplayers As New List(Of clsLayerPres)
            emplayers = layersselected_copieren(layersSelected)
            If layerHgrund.aid > 0 Then
                layerHgrund.mithaken = True
                emplayers.Add(layerHgrund)
            End If
            If mitsuchobjekt Then
                If Not aktFST.name.IsNothingOrEmpty Then
                    Dim fstTabDef As New clsTabellenDef
                    If clsFSTtools.extractSchemaTab(fstTabDef, aktFST) Then
                        fstTabDef.gid = CType(aktFST.abstract, String)
                    End If
                    Dim oslayer As New clsLayerPres
                    oslayer.mapFile = makeMapfileSuchobjekt(myglobalz.serverUNC & strGlobals.paradigma_hervorhebungflaecheFSTmap.Replace(".map", "PDF.map"),
                                                            myglobalz.serverUNC & "/cache/gis/" & clsString.date2string(Now, 5) & "PDF.map",
                           fstTabDef.Schema & "." & fstTabDef.tabelle, fstTabDef.gid)

                    oslayer.aid = 6000
                    oslayer.rang = 80
                    'oslayer.mapFile = "/cache/gis/test.map" 'strGlobals.paradigma_hervorhebungflaechemap
                    oslayer.mithaken = True
                    oslayer.tabname = os_tabelledef.tabelle
                    oslayer.schema = os_tabelledef.Schema
                    emplayers.Add(oslayer)
                End If
            End If
            emplayers = layersSelectedNachRangOrdnen(emplayers)

            'emplayers = layersSelectedNachRangOrdnen(layersSelected)
            For Each nlayer As clsLayerPres In emplayers
                If nlayer.mithaken Then
                    If hochaufloesend Then
                        'If nlayer.aid = 72 Then
                        '    '/nkat/aid/151/layer.map
                        '    'sb.AppendLine("INCLUDE '/nkat/aid/349/layer.map" & "'")
                        '    sb.AppendLine("INCLUDE '" & nlayer.mapFile & "'")
                        '    sb.AppendLine(" ")
                        'Else
                        sb.AppendLine("INCLUDE '" & nlayer.mapFile & "'")
                        sb.AppendLine(" ")
                        'End If
                    Else
                        sb.AppendLine("INCLUDE '" & nlayer.mapFile & "'")
                        sb.AppendLine(" ")
                    End If

                End If
            Next
            If hochaufloesend Then
                sb.AppendLine("INCLUDE " & strGlobals.PDFdruck_MapFileMinimalQuerHochaufl)
            Else
                sb.AppendLine("INCLUDE " & strGlobals.PDFdruck_MapFileMinimalQuer)
            End If

            sb.AppendLine("End")
            Return sb.ToString

        Catch ex As Exception
            l("fehler in createMapfilePDF ", ex)
            Return ""
        End Try
    End Function

    Private Function makeMapfileSuchobjekt(template As String, outfile As String, table As String, gid As String) As String
        Dim strtemplate As String = ""
        Try
            l(" MOD makeMapfileSuchobjekt anfang")
            template = template.Replace("/", "\")
            outfile = outfile.Replace("/", "\")
            strtemplate = IO.File.ReadAllText(template)

            strtemplate = strtemplate.Replace("%gid%", gid.ToString)
            strtemplate = strtemplate.Replace("%table%", table.ToString)
            IO.File.WriteAllText(outfile, strtemplate)
            l(" MOD makeMapfileSuchobjekt ende")

            Return outfile
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
            Return ""
        End Try
    End Function

    Sub StringbuilderAussschreiben(sb As String, datei As String)
        Try
            l("-StringbuilderAussschreiben--------------------- anfang")
            l("mapfileBILD " & mapfileBILD)
            My.Computer.FileSystem.WriteAllText(datei, sb, False, enc)
            l("-StringbuilderAussschreiben--------------------- ende")
        Catch ex As Exception
            l("Fehler in StringbuilderAussschreiben: " & ex.ToString())
        End Try
    End Sub

    'Friend Sub createMapfileHG()
    '    mapfileBILDrank0 = "d:/" + layerHgrund.mapFile.Replace("layer.map", "header.map")
    '    mapfileBILDrank0 = "" + layerHgrund.mapFile.Replace("layer.map", "header.map")
    '    Exit Sub
    '    ' /fkat/flurkarte/flurkarte2016/flurkarte2016_header.map"
    '    'MAP
    '    'INCLUDE '/inetpub/wwwroot/buergergis/mapfile/header.map',
    '    'INCLUDE '/fkat/boden/bodentyp/bodentyp_layer.map',,
    '    'End
    '    Try
    '        Dim sb As New Text.StringBuilder
    '        sb.AppendLine("MAP")
    '        'sb.AppendLine("INCLUDE '/inetpub/wwwroot/buergergis/mapfile/header.map'")
    '        sb.AppendLine("INCLUDE " & normMapfileHeader & "")
    '        'Dim emplayers As New List(Of clsLayerPres)
    '        'emplayers = layersselected_copieren(layersSelected)
    '        '' If layerHgrund.aid > 0 Then emplayers.Add(layerHgrund)
    '        'emplayers = layersSelectedNachRangOrdnen(emplayers)


    '        If layerHgrund.aid > 0 Then
    '            sb.AppendLine("INCLUDE '" & layerHgrund.mapFile & "'")
    '        End If

    '        sb.AppendLine(" ")

    '        sb.AppendLine("End")
    '        l("createMapfileHG Vor dem ausschreiben")
    '        My.Computer.FileSystem.WriteAllText(mapfileBILDrank0, sb.ToString, False, enc)
    '    Catch ex As Exception
    '        l("fehler in createMapfileHG ", ex)
    '    End Try
    'End Sub

    Private Function layersSelectedNachRangOrdnen(layersSelected As List(Of clsLayerPres)) As List(Of clsLayerPres)
        Dim neu As New List(Of clsLayerPres)

        For irang = 0 To 100
            For Each lay As clsLayerPres In layersSelected
                If lay.rang = irang Then
                    neu.Add(lay)
                End If
            Next
        Next
        Return neu
    End Function

    Friend Function getStandardActiveLayer() As clsLayerPres
        Dim nlayer As New clsLayerPres
        nlayer.aid = 21
        Return nlayer
    End Function

    'Friend Sub getLayerHgrund()
    '    layerHgrund.aid = 210
    '    pgisTools.getStamm4aid(layerHgrund)
    '    layerHgrund.isHgrund = True
    '    layerHgrund.mithaken = True
    'End Sub

    Friend Function getLayerActive() As clsLayerPres
        pgisTools.getStamm4aid(layerActive)
        layerActive.mithaken = True
        layerActive.RBischecked = True
        Return layerActive
    End Function

    Private Function getMapfileFullpath(nlayer As clsLayerPres) As String
        Dim datei As String = nlayer.mapFile
        datei = datei.Replace("/", "\")
        datei = serverUNC & datei.Replace("/", "\")
        Return datei
    End Function

    Friend Function getLayer4stichwort(text As String, ByRef anzahlSchonGeladeneEbenen As Integer) As List(Of clsLayerPres)
        Dim tlayerlist As New List(Of clsLayerPres)
        Dim tlayer As New clsLayerPres
        anzahlSchonGeladeneEbenen = 0
        text = text.Replace(",", " ") : text = text.Replace(".", " ") : text = text.Replace(";", " ") : text = text.Replace(":", " ")
        Dim woerter() As String
        woerter = text.Trim.Split(" "c)
        For i = 0 To woerter.Count - 1
            woerter(i) = woerter(i).Trim
        Next
        text = text.Trim.ToLower
        For Each preslayer As clsLayerPres In allLayersPres
            If preslayer.isHgrund Then Continue For
            If woerter.Length = 1 Then
                If preslayer.suchfeld.ToLower.Contains(woerter(0)) Then
                    trefferBehandeln(anzahlSchonGeladeneEbenen, tlayerlist, tlayer, preslayer)
                End If
            End If
            If woerter.Length = 2 Then
                If preslayer.suchfeld.ToLower.Contains(woerter(0)) Or
                   preslayer.suchfeld.ToLower.Contains(woerter(1)) Then
                    trefferBehandeln(anzahlSchonGeladeneEbenen, tlayerlist, tlayer, preslayer)
                End If
            End If
            If woerter.Length = 3 Then
                If preslayer.suchfeld.ToLower.Contains(woerter(0)) Or
                    preslayer.suchfeld.ToLower.Contains(woerter(1)) Or
                    preslayer.suchfeld.ToLower.Contains(woerter(2)) Then
                    trefferBehandeln(anzahlSchonGeladeneEbenen, tlayerlist, tlayer, preslayer)
                End If
            End If
            If woerter.Length > 3 Then
                If preslayer.suchfeld.ToLower.Contains(woerter(0)) Or
                    preslayer.suchfeld.ToLower.Contains(woerter(1)) Or
                    preslayer.suchfeld.ToLower.Contains(woerter(2)) Or
                    preslayer.suchfeld.ToLower.Contains(woerter(3)) Then
                    trefferBehandeln(anzahlSchonGeladeneEbenen, tlayerlist, tlayer, preslayer)
                End If
            End If

        Next
        Return tlayerlist
    End Function

    Private Sub trefferBehandeln(ByRef anzahlSchonGeladeneEbenen As Integer, tlayerlist As List(Of clsLayerPres), ByRef tlayer As clsLayerPres, preslayer As clsLayerPres)
        If warSchonGeladen(preslayer.aid, layersSelected) Then
            anzahlSchonGeladeneEbenen += 1
            tlayer = CType(preslayer.Clone, clsLayerPres)
            tlayer.schongeladen = 1
            tlayerlist.Add(tlayer)
        Else
            If warSchonGeladen(preslayer.aid, tlayerlist) Then
                anzahlSchonGeladeneEbenen += 1
            Else
                tlayer = CType(preslayer.Clone, clsLayerPres)
                tlayerlist.Add(tlayer)
            End If
        End If
    End Sub

    Public Function warSchonGeladen(aid As Integer, tlist As List(Of clsLayerPres)) As Boolean
        For Each lay As clsLayer In tlist
#If DEBUG Then
            If lay.aid = 303 Then
                Debug.Print("")
            End If
#End If
            If lay.aid = CInt(aid) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub alteAktiveEbeneDeaktivieren(layerliste As List(Of clsLayerPres))
        For Each lay As clsLayerPres In layerliste
            lay.isactive = False
            lay.RBischecked = False
        Next
    End Sub
    Friend Function getHintergrund(binImInternet As Boolean) As List(Of clsLayerPres)
        Dim tlayerlist As New List(Of clsLayerPres)
        Dim tlayer As New clsLayerPres
        Dim aid As Integer

        If iminternet Then
            Dim result, hinweis, a(), b() As String
            Dim spalten, zeilen As Integer
            aufruf = strGlobals.buergergisInternetServer & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=weinachtsmann&modus=gettable&viewname=doku4stamm"
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            result = result.Trim
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count

            'allLayers.Clear()
            tlayer = New clsLayerPres
            For i = 0 To zeilen - 1
                tlayer = New clsLayerPres
                'tlayer = initSingleDokuFromArray(tlayer, i, a)
                b = a(i).Split("#"c)
                tlayer.aid = CInt(b(0))
                tlayerlist.Add(tlayer)
            Next

        Else
            Dim dt As DataTable
            dt = getDTFromWebgisDB("select * from hintergrund ", "webgiscontrol")
            For i = 0 To dt.Rows.Count - 1
                aid = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("aid")))
                tlayer = makeHgrundlayer(tlayerlist, aid)
            Next
        End If
        Return tlayerlist
    End Function

    Private Function makeHgrundlayer(tlayerlist As List(Of clsLayerPres), aid As Integer) As clsLayerPres
        Dim tlayer As clsLayerPres = New clsLayerPres
        tlayer.aid = aid
        If tlayer Is Nothing Then                'fehler in db
        Else
            If istAuchHintergrund(tlayer) Then
            Else
                tlayer = pgisTools.getStamm4aid(tlayer)
                tlayer.thumbnailFullPath = myglobalz.serverUNC & "nkat\thumbnails\" & tlayer.aid & ".png"
                tlayer.dokutext = clsWebgisPGtools.bildeDokuTooltip(tlayer)
                tlayerlist.Add(tlayer)
            End If
        End If

        Return tlayer
    End Function

    Function getLayer4sachgebiet(sg As String) As List(Of clsLayerPres)
        Dim tlayerlist As New List(Of clsLayerPres)
        Dim tlayer As New clsLayerPres
        sg = sg.Trim.ToLower
        Dim anzahlSchonGeladeneEbenen = 0
        For Each preslayer As clsLayerPres In allLayersPres
            If Not preslayer.isHgrund Then
                If preslayer.standardsachgebiet.ToLower = (sg) Then
                    'If Not warSchonGeladen(preslayer.aid, layersSelected) Then
                    '    If Not warSchonGeladen(preslayer.aid, tlayerlist) Then
                    '        tlayer = CType(preslayer.Clone, clsLayerPres)
                    '        tlayerlist.Add(tlayer)
                    '    End If
                    'End If
                    trefferBehandeln(anzahlSchonGeladeneEbenen, tlayerlist, tlayer, preslayer)
                End If
            Else
                Debug.Print("keine hintergründe")
            End If
        Next
        Return tlayerlist
    End Function



    Private Function istAuchHintergrund(tlayer As clsLayerPres) As Boolean
        For Each nlay As clsLayerPres In hgrundLayers
            If nlay.aid = tlayer.aid Then
                Return True
            End If
        Next
        Return False
    End Function

    Friend Function getColorBrush4hauptSachgebiet(sid As String) As SolidColorBrush
        Dim Converter = New System.Windows.Media.BrushConverter()
        Dim Brush As SolidColorBrush
        Select Case sid
            Case "allgemein"
                Brush = CType(Converter.ConvertFromString("#D8DADC"), SolidColorBrush)
                Return Brush
            Case "boden"
                Brush = CType(Converter.ConvertFromString("#E0C0A0"), SolidColorBrush)
                Return Brush
            Case "grenzen"
                Brush = CType(Converter.ConvertFromString("#ea7b7b"), SolidColorBrush)
                Return Brush
            Case "klima"
                Brush = CType(Converter.ConvertFromString("#F8F880"), SolidColorBrush)
                Return Brush
            Case "h_landschaftsschutz"
                Brush = CType(Converter.ConvertFromString("#B0B0B0"), SolidColorBrush)
                Return Brush
            Case "h_landschaftsschutz"
                Brush = CType(Converter.ConvertFromString("#B0B0B0"), SolidColorBrush)
                Return Brush
            Case "h_luftbild"
                Brush = CType(Converter.ConvertFromString("#B0B0B0"), SolidColorBrush)
                Return Brush
            Case "h_regionalplan"
                Brush = CType(Converter.ConvertFromString("#B0B0B0"), SolidColorBrush)
                Return Brush
            Case "h_topkarte"
                Brush = CType(Converter.ConvertFromString("#B0B0B0"), SolidColorBrush)
                Return Brush
            Case "h_verschiedenes"
                Brush = CType(Converter.ConvertFromString("#B0B0B0"), SolidColorBrush)
                Return Brush
            Case "kreisoffenbach"
                Brush = CType(Converter.ConvertFromString("#D0B0FF"), SolidColorBrush)
                Return Brush
            Case "kreishaus"
                Brush = CType(Converter.ConvertFromString("#D0B0FF"), SolidColorBrush)
                Return Brush
            Case "gemeinden"
                Brush = CType(Converter.ConvertFromString("#D0B0FF"), SolidColorBrush)
                Return Brush
            Case "kreishaus"
                Brush = CType(Converter.ConvertFromString("#D0B0FF"), SolidColorBrush)
                Return Brush
            Case "arten"
                Brush = CType(Converter.ConvertFromString("#A0E0A0"), SolidColorBrush)
                Return Brush
            Case "foerder"
                Brush = CType(Converter.ConvertFromString("#A0E0A0"), SolidColorBrush)
                Return Brush
            Case "boden"
                Brush = CType(Converter.ConvertFromString("#B0B0B0"), SolidColorBrush)
                Return Brush
            Case "natur"
                Brush = CType(Converter.ConvertFromString("#A0E0A0"), SolidColorBrush)
                Return Brush
            Case "planung"
                Brush = CType(Converter.ConvertFromString("#FFB0B0"), SolidColorBrush)
                Return Brush
            Case "regfnp"
                Brush = CType(Converter.ConvertFromString("#FFB0B0"), SolidColorBrush)
                Return Brush
            Case "denkmal"
                Brush = CType(Converter.ConvertFromString("#FFB0B0"), SolidColorBrush)
                Return Brush
            Case "verkehr_rad"
                Brush = CType(Converter.ConvertFromString("#FFB0B0"), SolidColorBrush)
                Return Brush
            Case "verkehr"
                Brush = CType(Converter.ConvertFromString("#FFB0B0"), SolidColorBrush)
                Return Brush
            Case "sicherheit"
                Brush = CType(Converter.ConvertFromString("#80D0F8"), SolidColorBrush)
                Return Brush
            Case "hochwasserschutz"
                Brush = CType(Converter.ConvertFromString("#80D0F8"), SolidColorBrush)
                Return Brush
            Case "waffenwesen"
                Brush = CType(Converter.ConvertFromString("#80D0F8"), SolidColorBrush)
                Return Brush
            Case "soziales"
                Brush = CType(Converter.ConvertFromString("#FFD080"), SolidColorBrush)
                Return Brush
            Case "tourismus"
                Brush = CType(Converter.ConvertFromString("#D0E000"), SolidColorBrush)
                Return Brush
            Case "wasser"
                Brush = CType(Converter.ConvertFromString("#AAECE0"), SolidColorBrush)
                Return Brush
            Case "wirtschaft"
                Brush = CType(Converter.ConvertFromString("#FFA0FF"), SolidColorBrush)
                Return Brush
            Case Else
                Brush = CType(Converter.ConvertFromString("#D8DADC"), SolidColorBrush)
                Return Brush
        End Select
    End Function

    Friend Function kopiereLayersSeelected(temp As List(Of clsLayerPres)) As List(Of clsLayerPres)
        Dim neulist As New List(Of clsLayerPres)
        Dim neu As New clsLayerPres
        Try
            l("kopiereLayersSeelected---------------------- anfang")
            For Each nlay As clsLayerPres In temp
                neu = New clsLayerPres
                neu = CType(nlay.Clone, clsLayerPres)
                neulist.Add(neu)
            Next
            Return neulist
            l("kopiereLayersSeelected---------------------- ende")
        Catch ex As Exception
            l("Fehler in kopiereLayersSeelected: " & ex.ToString())
            Return Nothing
        End Try
    End Function
End Module
