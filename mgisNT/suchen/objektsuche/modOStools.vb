Module modOStools
    Friend Sub os_dbanzeigen(paradigmavid As String, ebenentitel As String)
        Try
            l("os_dbanzeigen---------------------- anfang")
            Dim aktfs As String
            aktObjID = CInt(os_tabelledef.gid)
            Dim buttoninfostring As String = ""
            Dim ergebnis = clsMiniMapTools.getDBrecord(CInt(os_tabelledef.aid), CInt(os_tabelledef.gid), buttoninfostring,
                                                       CInt(os_tabelledef.tab_nr), aktfs)


            'Dim ergebnis = clsMiniMapTools.getDBrecord4menu(CInt(os_tabelledef.aid), CInt(os_tabelledef.gid), buttoninfostring,
            '                                           (os_tabelledef.tabelle), aktfs)
            clsMiniMapTools.aktFS2aktFST_init(aktfs)
            l("ergebnis: " & ergebnis)
            clsMiniMapTools.dbAbfrageDiaglogPrep(buttoninfostring, ebenentitel)
            l("os_dbanzeigen---------------------- ende")
        Catch ex As Exception
            l("Fehler in os_dbanzeigen: ", ex)
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

                acanvas.CalcCenter()

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
                suchObjektModus = "puffer"
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
