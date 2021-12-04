Public Class clsMapfile

    Friend Shared Function buildmapfilepdf(nick As String, pngorpdf As String, hochaufloesend As String, layers As String,
                                           hoehe As Integer, breite As Integer, isa4Formatchecked As Boolean, mapfileFST As String) As String
        'Dim pdfheader = "'/websys/mapfiles/system/paradigma_d_q_s_PDFheader.map'"
        'Dim pngheader = "'/websys/mapfiles/system/paradigma_d_q_s_PNGheader.map'"
        'Dim PDFdruck_MapFileMinimalQuerHochaufl As String = "'/websys/mapfiles/system/paradigma_minimal_querHochaufl.map'"
        'Dim PDFdruck_MapFileMinimalQuer = "'/websys/mapfiles/system/paradigma_minimal_quer.map'"


        Dim pdfheader = "'/nkat/vorlage/paradigma/system/paradigma_d_q_s_PDFheader.map'"
        Dim pngheader = "'/nkat/vorlage/paradigma/system/paradigma_d_q_s_PNGheader.map'"
        Dim PDFdruck_MapFileMinimalQuerHochaufl As String = "'/nkat/vorlage/paradigma/system/paradigma_minimal_querHochaufl.map'"
        Dim PDFdruck_MapFileMinimalQuer = "'/nkat/vorlage/paradigma/system/paradigma_minimal_quer.map'"

        Dim a(), result, mapfilefullname As String
        Dim mapfileOutCachePathroot = "d:\cache\mapfiles\"
        Dim nkat As String = "/nkat/aid/"
        Try
            l(" MOD buildmapfilepdf anfang")
            l("nick: " & nick & Environment.NewLine &
                "pngorpdf:" & pngorpdf & Environment.NewLine &
                "hochaufloesend:" & hochaufloesend & Environment.NewLine &
                "layers:" & layers & Environment.NewLine &
                "hoehe:" & hoehe & Environment.NewLine &
                "breite:" & breite & Environment.NewLine &
                "isa4Formatchecked:" & isa4Formatchecked & Environment.NewLine
                 )
            mapfilefullname = mapfileOutCachePathroot & nick & "_" & clsString.date2string(Now, 5) & "i.map"
            Dim sb As New Text.StringBuilder
            sb.AppendLine("MAP")
            If pngorpdf = "png" Or hochaufloesend = "1" Then
                l("hochaufloesend header")
                sb.AppendLine("INCLUDE " & pngheader)
            Else
                l("nicht hochaufloesend header")
                sb.AppendLine("INCLUDE " & pdfheader)
            End If
            l("111 " & sb.ToString)
            a = layers.Split(","c)
            l("a.count " & a.Count)
            For i = 0 To a.Count - 1
                If a(i) = "338" Then Continue For

                sb.AppendLine("INCLUDE '" & nkat & a(i) & "/layer.map" & "'")
                sb.AppendLine(" ")
            Next
            l("22 " & sb.ToString)
            If mapfileFST.Trim <> String.Empty Then
                sb.AppendLine("INCLUDE '" & mapfileFST & "'")
                sb.AppendLine(" ")
            End If
            l("22a " & sb.ToString)
            If hochaufloesend = "1" Then
                l("hochaufloesend stempel")
                sb.AppendLine("INCLUDE " & PDFdruck_MapFileMinimalQuerHochaufl)
            Else
                l("nicht hochaufloesend stempel")
                sb.AppendLine("INCLUDE " & PDFdruck_MapFileMinimalQuer)
            End If

            sb.AppendLine("End")
            l("mapfilefullname " & mapfilefullname)
            l("33 " & sb.ToString)
            IO.File.WriteAllText(mapfilefullname, sb.ToString)
            l(" MOD buildmapfilepdf ende")
            Return mapfilefullname
        Catch ex As Exception
            l("Fehler in buildmapfilepdf: " & ex.ToString())
            Return "fehler bei Erstellung des Mapfiles"
        End Try
    End Function

    Shared Function bildeaufrufMapserverPDF(_breite As String, _hoehe As String,
                                       temprange As String, druckmasstab As Double,
                                       pdf_bemerkung As String, pdf_ort As String,
                                       mapfilefullname As String, isa4Formatchecked As Boolean) As String
        'https://buergergis.kreis-offenbach.de/cgi-bin/mapserv70/mapserv.cgi?
        '           mapsize=842+595&mapext=484926+5541956+485580+5542394&map=C:/ptest/mgis/cache/mapfiles/feinen_j_20190111071517384.map
        '           &ortsangabe=UTM32: 485253, 5542175&bemerkung=feinen_j&druckmasstab=&datum=11.01.2019 
        'mapsize=842+595'; // Auflösung für MapServer: 72 dpi
        Dim massstabzeile, aufruf, mapserverExeString, serverWeb As String
        serverWeb = "http://127.0.0.1"
        serverWeb = "http://localhost"
        'mapserverExeString = "d:\inetpub\scripts\mapserv70\mapserv.exe " & Chr(34) & "QUERY_STRING="

        'mapserverExeString = "/cgi-bin/mapserv70/mapserv.cgi"
        mapserverExeString = ""
        If druckmasstab < 1 Then
            massstabzeile = "&druckmasstab="
        Else
            massstabzeile = "&druckmasstab=1:" & (CInt(druckmasstab).ToString)
        End If
        aufruf = mapserverExeString & "mapsize=" & _breite & "+" & _hoehe &
                "&mapext=" & temprange &
                "&map=" & mapfilefullname & "" &
                "&ortsangabe=" & pdf_ort &
                "&hoehe=" & _hoehe &
                "&breite=" & _breite &
                "&isa4Formatchecked=" & isa4Formatchecked &
                "&bemerkung=" & pdf_bemerkung &
                massstabzeile &
                "&datum=" & Format(Now, "dd.MM.yyyy")
        aufruf = aufruf
        aufruf = aufruf.Replace("\", "/")
        'aufruf = aufruf & Chr(34)
        'aufruf = aufruf.Replace("//w2gis02/gdvell", "d:")
        l("bildeaufrufMapserverPDF: " & aufruf)
        Return aufruf
    End Function

    Friend Shared Function bildeMapfileFST(fstname As String, fstabstract As String) As String
        Dim mapfile, tabelle, schema, gid As String
        Try
            l(" MOD bildeMapfileFST anfang")
            l(" fstname " & fstname)
            'Dim fstTabDef As New clsTabellenDef
            Dim paradigma_hervorhebungflaecheFSTmap As String = "/nkat/vorlage/paradigma_hervorhebungflaecheFST.map"
            'flurkarte.basis_f
            If fstname.Contains(".") Then
                Dim a = fstname.Split("."c)
                tabelle = a(1).Trim
                Schema = a(0).Trim
                gid = CType(fstabstract, String)
            End If

            l(" fstTabDef.tabelle " & tabelle)
            l(" fstTabDef.Schema " & schema)
            'If clsFSTtools.extractSchemaTab(fstTabDef, aktFST.name) Then
            '    fstTabDef.gid = CType(aktFST.abstract, String)
            'End If
            'Dim oslayer As New clsLayerPres
            mapfile = makeMapfileSuchobjekt("d:\" & paradigma_hervorhebungflaecheFSTmap.Replace(".map", "PDF.map"),
                                                "d:\" & "cache\gis\" & clsString.date2string(Now, 5) & "PDF.map",
                                                schema & "." & tabelle, gid)
            mapfile = mapfile.Replace("d:\", "d:\").Replace("/", "\").Replace("\\", "\")
            l("mapfile " & mapfile)
            'oslayer.aid = 6000
            'oslayer.rang = 80
            ''oslayer.mapFile = "/cache/gis/test.map" 'strGlobals.paradigma_hervorhebungflaechemap
            'oslayer.mithaken = True
            'oslayer.tabname = os_tabelledef.tabelle
            'oslayer.schema = os_tabelledef.Schema
            'emplayers.Add(oslayer)
            l(" MOD bildeMapfileFST ende")
            Return mapfile
        Catch ex As Exception
            l("Fehler in bildeMapfileFST: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function makeMapfileSuchobjekt(template As String, outfile As String, table As String, gid As String) As String
        Dim strtemplate As String = ""
        Try
            l(" MOD makeMapfileSuchobjekt anfang")
            l("  template " & template)

            template = template.Replace("/", "\")
            outfile = outfile.Replace("/", "\")
            strtemplate = IO.File.ReadAllText(template)

            strtemplate = strtemplate.Replace("%gid%", gid.ToString)
            strtemplate = strtemplate.Replace("%table%", table.ToString)
            IO.File.WriteAllText(outfile, strtemplate)
            l("  outfile " & outfile)
            l("  strtemplate " & strtemplate)
            l(" MOD makeMapfileSuchobjekt ende")
            Return outfile
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
