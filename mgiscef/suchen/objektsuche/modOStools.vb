Module modOStools
    Friend Sub os_dbanzeigen(paradigmavid As String, ebenentitel As String)
        Dim ergebnis As String
        Try
            l("os_dbanzeigen---------------------- anfang")
            Dim aktfs As String = ""
            aktObjID = CInt(os_tabelledef.gid)
            Dim buttonINfostringspecfunc As String = ""
            If CInt(os_tabelledef.aid) > 0 And CInt(os_tabelledef.gid) > 0 Then
                If os_tabelledef.tabellen_anzeige = "attributtabelle" Then
                    'Dim linkTabs(), jumpGID, jumptabelle, sql, result As String
                    'linkTabs = os_tabelledef.linkTabs.Split(","c)
                    'sql = "select " & linkTabs(0) & " from " & os_tabelledef.Schema & "." & os_tabelledef.tabelle &
                    '                 " where gid=" & os_tabelledef.gid
                    'l("sql " & sql)
                    'result = clsSachdatentools.getOneValSQL("postgis20", sql)
                    'l("result " & result)
                    'jumpGID = result.Trim
                    'jumptabelle = clsSachdatentools.getTabname4tabnr(CInt(os_tabelledef.aid), "2")
                    'ergebnis = clsMiniMapTools.getDBrecord(layerActive.aid, CInt(jumpGID), buttonINfostringspecfunc, 2, "", os_tabelledef.tabellen_anzeige)
                    'nachricht("ergebnis:" & ergebnis)
                    clsMiniMapTools.dbAttributtabelle(buttonINfostringspecfunc, ergebnis, isOSsuche:=True)
                Else
                    ergebnis = clsMiniMapTools.getDBrecord(CInt(os_tabelledef.aid), (os_tabelledef.gid), buttonINfostringspecfunc,
                                                                     CInt(os_tabelledef.tab_nr), aktfs, "")
                End If
                clsMiniMapTools.aktFS2aktFST_init(aktfs)
                l("ergebnis: " & ergebnis)
                If gesamtSachdatList Is Nothing Then
                Else
                    clsMiniMapTools.createRtfAndShowDialog(buttonINfostringspecfunc, ebenentitel, 0, isOSsuche:=True)
                End If
            Else
                l("fehler in os_dbanzeigen:  m CInt(os_tabelledef.aid) <1 CInt(os_tabelledef.gid) > 0")
            End If
            l("os_dbanzeigen---------------------- ende")
        Catch ex As Exception
            l("Fehler in os_dbanzeigen m CInt(os_tabelledef.aid) <1: ", ex)
        End Try
    End Sub

    Friend Function os_zurkarte() As Boolean
        Dim puffererzeugt As Boolean
        Dim puffer_area As Double
        Dim pufferinMeter As Double = 0.01
        Dim acanvas As New clsRange
        Try
            l("os_zurkarte---------------------- anfang")
            If Not os_tabelledef.tabelle.ToLower.StartsWith("os_") Then
                os_tabelledef.tabelle = "os_" & os_tabelledef.tabelle
            End If
            'If Not os_tabelledef.os_tabellen_name.StartsWith("os_") Then
            '    os_tabelledef.os_tabellen_name = "os_" & os_tabelledef.tabelle
            'End If
            puffererzeugt = modEW.bildePufferFuerPolygon(aktPolygon, pufferinMeter, os_tabelledef, puffer_area, acanvas, True)

            GC.Collect()
            If puffererzeugt Then
                If os_tabelledef.geomtype = "point" Then
                    pufferinMeter = 500
                    acanvas.xl = acanvas.xl - (pufferinMeter / 2)
                    acanvas.xh = acanvas.xh + (pufferinMeter / 2)
                    acanvas.yl = acanvas.yl - (pufferinMeter / 2)
                    acanvas.yh = acanvas.yh + (pufferinMeter / 2)
                    aktFST.normflst.serials.Add(aktPolygon.ShapeSerial)
                Else
                    acanvas.bbox_split()
                End If

                'acanvas.CalcCenter()

                aktGlobPoint.strX = CType(CInt(acanvas.xcenter), String)
                aktGlobPoint.strY = CType(CInt(acanvas.ycenter), String)
                If os_tabelledef.geomtype = "point" Then
                    kartengen.aktMap.aktrange.xl = acanvas.xl
                    kartengen.aktMap.aktrange.xh = acanvas.xh
                    kartengen.aktMap.aktrange.yl = acanvas.yl
                    kartengen.aktMap.aktrange.yh = acanvas.yh
                Else
                    kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, CInt(acanvas.xdif))
                End If

                '   aktFST.normflst.serials.Add(aktPolygon.ShapeSerial)
                suchObjektModus = suchobjektmodusEnum.pufferObjektDarstellen
                l("os_zurkarte---------------------- ende")
                Return True
            End If
            Return False
            l("os_zurkarte---------------------- ende")
        Catch ex As Exception
            l("Fehler in os_zurkarte: ", ex)
            Return False
        End Try

    End Function
End Module
