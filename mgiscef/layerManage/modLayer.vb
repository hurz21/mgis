Imports System.Data
Imports mgis

Module modLayer
    'Public layerSelected As New clsLayerPres
    Public layerActive As New clsLayerPres
    Public tempActivelayer As New clsLayerPres
    Public layerHgrund As New clsLayerPres
    Public layersSelected As New List(Of clsLayerPres)
    Public PDFlayers As New List(Of clsLayerPres)
    Public katlayersList As New List(Of clsLayerPres)
    Public layersSelectedKompakt As New List(Of clsLayerPres)
    Public layersSelectedOld As New List(Of clsLayerPres) ' für die checkbox als tempSpeicher
    Public layersFavorites As New List(Of clsLayerPres)
    Public SuchLayersList As New List(Of clsLayerPres)
    '


    'Public mapfileIMAP As String = ""
    'Public mapfileBILD As String = ""
    'Public mapfileBILDrank0 As String = ""
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

        Dim sb As New Text.StringBuilder
        Try
            sb.AppendLine("MAP")
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

    Function layersselected_copieren(layersSelected As List(Of clsLayerPres)) As List(Of clsLayerPres)
        Dim emp As New List(Of clsLayerPres)
        Try
            l(" MOD layersselected_copieren anfang")
            For Each lay As clsLayerPres In layersSelected
                emp.Add(lay)
            Next
            l(" MOD layersselected_copieren ende")
            Return emp

        Catch ex As Exception
            l("Fehler in layersselected_copieren: ", ex)
            Return emp
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
            l("Fehler in makeMapfileSuchobjekt: ", ex)
            Return ""
        End Try
    End Function

    'Sub StringbuilderAussschreiben(sb As String, datei As String)
    '    Try
    '        l("-StringbuilderAussschreiben--------------------- anfang" & Environment.NewLine &
    '                 "mapfileBILD " & mapfileBILD)
    '        My.Computer.FileSystem.WriteAllText(datei, sb, False, enc)
    '        l("-StringbuilderAussschreiben--------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in StringbuilderAussschreiben: " & datei & Environment.NewLine,ex)
    '    End Try
    'End Sub
    Function layersSelectedNachRangOrdnen(layersSelected As List(Of clsLayerPres)) As List(Of clsLayerPres)
        Dim neu As New List(Of clsLayerPres)
        Try
            l(" MOD layersSelectedNachRangOrdnen anfang")

            For irang = 0 To 100
                For Each lay As clsLayerPres In layersSelected
                    'If lay.rang = irang Then
                    '    neu.Add(lay)
                    'End If
                    If lay.rang <> irang Then
                        Continue For
                    Else
                        neu.Add(lay)
                    End If
                Next
            Next
            Return neu

            l(" MOD layersSelectedNachRangOrdnen ende")

        Catch ex As Exception
            l("Fehler in layersSelectedNachRangOrdnen: ", ex)
            Return neu
        End Try
    End Function

    Friend Function getStandardActiveLayer() As clsLayerPres
        Dim nlayer As New clsLayerPres
        nlayer.aid = 21
        Return nlayer
    End Function

    Friend Sub markWMSlayers(llist As List(Of clsLayer))
        Try
            l(" MOD markWMSlayers anfang")
            For Each lay In llist
                markwmslayerSingle(lay)
            Next
            l(" MOD markWMSlayers ende")
        Catch ex As Exception
            l("Fehler in markWMSlayers: ", ex)
        End Try
    End Sub
    Friend Sub markWMSlayers(llist As List(Of clsLayerPres))
        Try
            l(" MOD markWMSlayers anfang")
            For Each lay In llist
                markwmslayerSingle(lay)
            Next
            l(" MOD markWMSlayers ende")
        Catch ex As Exception
            l("Fehler in markWMSlayers: ", ex)
        End Try
    End Sub

    Function markwmslayerSingle(lay As clsLayer) As Boolean
        Try
            ' l(" MOD markwmslayer anfang")
            For Each wms In wmspropList
                If wms.aid = lay.aid Then
                    lay.iswms = True
                    lay.wmsProps.aid = wms.aid
                    lay.wmsProps.url = wms.url
                    lay.wmsProps.typ = wms.typ
                    lay.wmsProps.format = wms.format
                    lay.wmsProps.stdlayer = wms.stdlayer
                    Return True
                End If
            Next
            '  l(" MOD markwmslayer ende")
            Return False
        Catch ex As Exception
            l("Fehler in markwmslayer: ", ex)
            Return False
        End Try
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

    'Private Function getMapfileFullpath(nlayer As clsLayerPres) As String
    '    Dim datei As String = nlayer.mapFile
    '    datei = datei.Replace("/", "\")
    '    datei = serverUNC & datei.Replace("/", "\")
    '    Return datei
    'End Function

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

    Sub trefferBehandeln(ByRef anzahlSchonGeladeneEbenen As Integer, tlayerlist As List(Of clsLayerPres), ByRef tlayer As clsLayerPres, preslayer As clsLayerPres)
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
    Public Function getPresLayerFromList(aid As Integer, tlist As List(Of clsLayerPres)) As clsLayerPres
        Try
            l(" MOD getPresLayerFromList anfang")
            For Each lay As clsLayerPres In tlist
                If lay.aid = CInt(aid) Then
                    Return lay
                End If
            Next
            l(" MOD getPresLayerFromList ende")
            Return Nothing
        Catch ex As Exception
            l("Fehler in getPresLayerFromList: ", ex)
            Return Nothing
        End Try
    End Function

    Public Sub alteAktiveEbeneDeaktivieren(layerliste As List(Of clsLayerPres))
        For Each lay As clsLayerPres In layerliste
            lay.isactive = False
            lay.RBischecked = False
        Next
    End Sub
    '    Friend Function getHintergrundListe(binImInternet As Boolean) As List(Of clsLayerPres)
    '        Dim tlayerlist As New List(Of clsLayerPres)
    '        Dim tlayer As New clsLayerPres
    '        Dim aid As Integer
    '        If iminternet Or myglobalz.CGIstattDBzugriff Then
    '            Dim result, hinweis, a(), b() As String
    '            Dim spalten, zeilen As Integer
    '            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick & "&modus=gettable&viewname=ref_hintergrundtitel&orderby=titel"
    '            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
    '            If result.IsNothingOrEmpty Then
    '                l("fehler kein kontak zum webserver:  " & aufruf)
    '                End
    '            End If
    '            nachricht(hinweis)
    '            result = result.Trim
    '            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
    '            b = a(0).Split("#"c) : spalten = b.Count
    '            tlayer = New clsLayerPres
    '            For i = 0 To zeilen - 1
    '                tlayer = New clsLayerPres
    '                b = a(i).Split("#"c)
    '                tlayer.aid = CInt(b(1))
    '                aid = tlayer.aid
    '#If DEBUG Then
    '                If tlayer.aid = 1 Then
    '                    Debug.Print(tlayer.thumbnailFullPath)
    '                End If
    '#End If
    '                tlayer = makeHgrundlayer(tlayerlist, aid)
    '            Next
    '        Else
    '            Dim dt As DataTable
    '            dt = getDTFromWebgisDB("select * from ref_hintergrundtitel order by titel", "webgiscontrol")
    '            For i = 0 To dt.Rows.Count - 1
    '                aid = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("aid")))
    '                tlayer = makeHgrundlayer(tlayerlist, aid)
    '            Next
    '        End If
    '        Return tlayerlist
    '    End Function

    Private Function makeHgrundlayer(ByRef tlayerlist As List(Of clsLayerPres), aid As Integer) As clsLayerPres
        Dim tlayer As clsLayerPres = New clsLayerPres
        tlayer.aid = aid
        If tlayer Is Nothing Then                'fehler in db
        Else
            'If istAuchHintergrund(tlayer) Then
            'Else
            tlayer = pgisTools.getStamm4aid(tlayer)
            'tlayer.thumbnailFullPath = myglobalz.serverUNC & "nkat\thumbnails\" & tlayer.aid & ".png"
            tlayer.thumbnailFullPath = myglobalz.serverWeb & "/nkat/thumbnails/" & tlayer.aid & ".png"
            tlayer.dokutext = "(Sachgeb.: " & tlayer.kategorieLangtext & ") " & Environment.NewLine & clsWebgisPGtools.bildeDokuTooltip(tlayer)
#If DEBUG Then
            If tlayer.aid = 1 Then
                Debug.Print(tlayer.thumbnailFullPath)
            End If

#End If
            If tlayer.aid > 0 Then tlayerlist.Add(tlayer)
            'End If
        End If
        Return tlayer
    End Function

    Function getLayer4sachgebiet(sg As String) As List(Of clsLayerPres)
        Dim tlayerlist As New List(Of clsLayerPres)
        Dim tlayer As New clsLayerPres
        sg = sg.Trim.ToLower
        Dim anzahlSchonGeladeneEbenen = 0
        For Each preslayer As clsLayerPres In allLayersPres
            If preslayer.isHgrund Then
                Debug.Print("keine hintergründe")
            Else
                If preslayer.standardsachgebiet.ToLower = (sg) Then
                    'If Not warSchonGeladen(preslayer.aid, layersSelected) Then
                    '    If Not warSchonGeladen(preslayer.aid, tlayerlist) Then
                    '        tlayer = CType(preslayer.Clone, clsLayerPres)
                    '        tlayerlist.Add(tlayer)
                    '    End If
                    'End If
                    trefferBehandeln(anzahlSchonGeladeneEbenen, tlayerlist, tlayer, preslayer)
                End If
            End If
        Next
        Return tlayerlist
    End Function

    Function istAuchHintergrund(tlayerAid As Integer) As Boolean
        For Each nlay As clsLayerPres In hgrundLayers
            If nlay.aid = tlayerAid Then
                Return True
            End If
        Next
        Return False
    End Function

    Function istAuchHintergrund(tlayer As clsLayerPres) As Boolean
        For Each nlay As clsLayerPres In hgrundLayers
            If nlay.aid = tlayer.aid Then
                Return True
            End If
        Next
        Return False
    End Function
    Function istAuchHintergrund(tlayer As clsLayer) As Boolean
        For Each nlay As clsLayer In hgrundLayers
            If nlay.aid = tlayer.aid Then
                Return True
            End If
        Next
        Return False
    End Function

    Friend Function getColorBrush4hauptSachgebiet(sid As String) As SolidColorBrush
        Dim Converter = New System.Windows.Media.BrushConverter()
        Dim Brush As SolidColorBrush
        Brush = CType(Converter.ConvertFromString("#FFF6F3E1"), SolidColorBrush)
        Return Brush
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
            l("Fehler in kopiereLayersSeelected: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Function getHintergrundListe(binImInternet As Boolean) As List(Of clsLayerPres)
        Dim tlayerlist As New List(Of clsLayerPres)
        Dim tlayer As New clsLayerPres
        Dim aid As Integer
        If iminternet Or myglobalz.CGIstattDBzugriff Then
            Dim result As String = "", hinweis As String = "", a(), b() As String
            Dim spalten, zeilen As Integer
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick & "&modus=gettable&viewname=ref_hintergrundtitel&orderby=titel"
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            If result.IsNothingOrEmpty Then
                l("fehler kein kontak zum webserver:  " & aufruf)
                End
            End If
            nachricht(hinweis)
            result = result.Trim
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            tlayer = New clsLayerPres
            For i = 0 To zeilen - 1
                tlayer = New clsLayerPres
                b = a(i).Split("#"c)
                tlayer.aid = CInt(b(1))
                aid = tlayer.aid
#If DEBUG Then
                If tlayer.aid = 1 Then
                    Debug.Print(tlayer.thumbnailFullPath)
                End If
#End If
                tlayer = makeHgrundlayer(tlayerlist, aid)
            Next
        Else
            Dim dt As DataTable
            dt = getDTFromWebgisDB("select * from ref_hintergrundtitel order by titel", "webgiscontrol")
            For i = 0 To dt.Rows.Count - 1
                aid = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("aid")))
                tlayer = makeHgrundlayer(tlayerlist, aid)
            Next
        End If
        Return tlayerlist
    End Function
    Friend Function getWMSpropList(allLayersPres As List(Of clsLayerPres), allLayers As List(Of clsLayer)) As List(Of wmsProps)
        Dim wmspropList As New List(Of wmsProps)
        Dim mwsp As New wmsProps
        Dim sql As String = "select * from public.wms"
        Try
            l(" MOD markWMSlayers anfang")
            If iminternet Or CGIstattDBzugriff Or 1 = 1 Then
                Dim result As String = "", hinweis As String = "", a(), b() As String
                Dim spalten, zeilen As Integer
                aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick &
                    "&modus=getsql&viewname=wmslayers" &
                    "&sql=" & sql &
                    "&dbname=webgiscontrol"
                result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
                If result.IsNothingOrEmpty Then
                    l("fehler kein kontak zum webserver:  " & aufruf)
                    End
                End If
                nachricht(hinweis)
                result = result.Trim
                a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
                b = a(0).Split("#"c) : spalten = b.Count
                For i = 0 To zeilen - 1
                    mwsp = New wmsProps
                    b = a(i).Split("#"c)
                    mwsp.aid = CInt(b(1))
                    mwsp.url = CStr(b(2))
                    mwsp.typ = CStr(b(3))
                    mwsp.format = CStr(b(4))
                    mwsp.stdlayer = CStr(b(5))
                    wmspropList.Add(mwsp)
                Next
                l(" MOD markWMSlayers ende")
                Return wmspropList
            Else

            End If

        Catch ex As Exception
            l("Fehler in markWMSlayers: ", ex)
            Return Nothing
        End Try
    End Function

    Private Sub addHgrund2layers(emplayers As List(Of clsLayerPres))
        If layerHgrund.aid > 0 Then
            layerHgrund.mithaken = True
            emplayers.Add(layerHgrund)
        End If
    End Sub

    Friend Function bestandHatWmsLayer(ByRef wmslayerTitelSumme As String) As Boolean
        Dim summe As String = ""
        Dim retcode As Boolean = False
        Try
            l("bestandHatWmsLayer---------------------- anfang")
            For Each lay In layersSelected
                If lay.iswms And lay.mithaken Then
                    summe = summe & " " & lay.titel & Environment.NewLine
                    retcode = True
                End If
            Next
            l("bestandHatWmsLayer---------------------- ende")
            wmslayerTitelSumme = summe
            Return retcode
        Catch ex As Exception
            l("Fehler in bestandHatWmsLayer: ", ex)
            Return False
        End Try
    End Function

    Friend Function kopiereSelectedLayers(layersSelected As List(Of clsLayerPres),
                                          mithaken As Boolean, ohnewms As Boolean) As List(Of clsLayerPres)
        Dim neuliste As New List(Of clsLayerPres)
        Dim neu As New clsLayerPres
        Try
            l(" MOD kopiereSelectedLayers anfang")
            If mithaken Then
                If ohnewms Then
                    For Each lay In layersSelected
                        If (Not lay.iswms) And lay.mithaken Then
                            neu = lay.kopie
                            neuliste.Add(neu)
                        End If
                    Next
                Else
                    For Each lay In layersSelected
                        If lay.mithaken Then
                            neu = lay.kopie
                            neuliste.Add(neu)
                        End If
                    Next
                End If
            Else
                If ohnewms Then
                    For Each lay In layersSelected
                        If (Not lay.iswms) And (Not lay.mithaken) Then
                            neu = lay.kopie
                            neuliste.Add(neu)
                        End If
                    Next
                Else
                    For Each lay In layersSelected
                        If (Not lay.mithaken) Then
                            neu = lay.kopie
                            neuliste.Add(neu)
                        End If
                    Next
                End If
            End If

            l(" MOD kopiereSelectedLayers ende")
            Return neuliste
        Catch ex As Exception
            l("Fehler in kopiereSelectedLayers: ", ex)
            Return neuliste
        End Try
    End Function
End Module
