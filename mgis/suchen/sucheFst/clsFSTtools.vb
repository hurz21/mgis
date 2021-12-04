Imports mgis

Public Class clsFSTtools
    Shared Sub dossierPrepMinimum()
        Dim utm As New Point
        utm.X = aktFST.punkt.X
        utm.Y = aktFST.punkt.Y
        Dim KoordinateKLickpt As New Point
        KoordinateKLickpt.X = 1
        KoordinateKLickpt.Y = 1
        globCanvasWidth = 2
        globCanvasHeight = 2



        clsSachdatentools.getdossier(utm, layerActive.aid,
                                            CInt(globCanvasWidth), CInt(globCanvasHeight),
                                            KoordinateKLickpt, aktFST.normflst.FS, "flaeche")
    End Sub
    Shared Sub NennerVerarbeiten(ByVal nennertext As String, ByRef pFST As ParaFlurstueck)
        pFST.normflst.nenner = CInt(nennertext)
        nennerUndFSPruefen(pFST)
    End Sub
    Public Shared Sub nennerUndFSPruefen(ByRef pfst As ParaFlurstueck)
        pfst.normflst.FS = pfst.normflst.buildFS()
        pfst.normflst.fstueckKombi = pfst.normflst.buildFstueckkombi
    End Sub
    ''' <summary>
    ''' holt sich die koordinate des extends des flurstücks
    ''' berechnet daraus letztlich den kartengen.aktMap.aktrange
    ''' der sollte auch beim speichern des flurstücks verwendet werden
    ''' setzt: aktFST.normflst.GKhoch,aktFST.punkt.X,aktGlobPoint.strX
    ''' </summary>
    Shared Sub holeKoordinaten4Flurstueck(nennertext As String, schematabelle As String, ByRef pFSt As ParaFlurstueck) 'aktFST
        NennerVerarbeiten(nennertext, aktFST)
        pFSt.normflst.FS = pFSt.normflst.buildFS
        FSGKrechtsGKHochwertHolen(pFSt.normflst, schematabelle)
        pFSt.normflst.GKhoch = CInt(pFSt.normflst.GKhoch)
        pFSt.normflst.GKrechts = CInt(pFSt.normflst.GKrechts)
        pFSt.punkt.X = pFSt.normflst.GKrechts
        pFSt.punkt.Y = pFSt.normflst.GKhoch
        aktGlobPoint.strX = CType(CInt(pFSt.normflst.GKrechts), String)
        aktGlobPoint.strY = CType(CInt(pFSt.normflst.GKhoch), String)
        Dim minradius As Integer
        minradius = calcMinradius(pFSt.normflst.radius)
        kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, minradius)
        'MsgBox("kartengen.aktMap.aktrange  " & kartengen.aktMap.aktrange.toString)
    End Sub
    Shared Sub fstnachParadigmaSpeichern(freitext As String, kurz As String)
        Dim erfolg As Boolean
        Dim radius As Integer
        Try
            aktFST.abstract = aktFST.setcoordsAbstract()
            aktFST.Freitext = freitext
            aktFST.name = kurz
            aktFST.normflst.gemeindename = clsString.Capitalize(aktFST.normflst.gemeindename)
            aktFST.typ = RaumbezugsTyp.Flurstueck
            aktFST.isMapEnabled = True
            aktFST.normflst.fstueckKombi = aktFST.normflst.buildFstueckkombi()
            'aktFST.normflst.FS = ""
            radius = 100
            aktFST.box.rangekopierenVon(kartengen.aktMap.aktrange)
            'modEW.Paradigma_Adresse_Neu(radius)
            erfolg = modParadigma.NeuesFSTspeichern(radius)
            If erfolg Then
                MessageBox.Show("Flurstück wurde erfolgreich in Paradigma gespeichert.")
            Else
                MessageBox.Show("Flurstück wurde NICHT erfolgreich in Paradigma gespeichert.")
            End If
        Catch ex As Exception
            l("fehler in adresseNaCHpARADIGMA ", ex)
        End Try
    End Sub

    Friend Shared Function getFlaecheZuFlurstueck(aktFST As ParaFlurstueck) As String
        Try
            l(" getFlaecheZuFlurstueck ---------------------- anfang")
            Dim sql As String = "select st_area(geom),flaeche from flurkarte.basis_f where fs='" & aktFST.normflst.FS & "'"
            Dim dt As System.Data.DataTable
            dt = getDTFromWebgisDB(sql, "postgis20")
            If dt.Rows.Count > 0 Then
                'Return ToString("0.00"))
                Return "Fläche: " & calcFlaeche(clsDBtools.fieldvalue(dt.Rows(0).Item(0))) & " qm (GIS-Wert), " &
                clsDBtools.fieldvalue(dt.Rows(0).Item(1)) & " qm (Grundbuch-Wert)"
            Else
                Return ""
            End If
            l(" getFlaecheZuFlurstueck ---------------------- ende")
            Return ""
        Catch ex As Exception
            l("Fehler in getFlaecheZuFlurstueck: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function calcFlaeche(val As String) As String
        Dim flaeche As String
        Dim fl As Integer
        Dim fld As Double
        Try
            l(" calcFlaeche ---------------------- anfang")
            flaeche = (val).Replace(".", ".")
            fld = CDbl(flaeche) '* 100
            flaeche = fld.ToString("0.00")
            Return flaeche
            l(" calcFlaeche ---------------------- ende")
        Catch ex As Exception
            l("Fehler in calcFlaeche: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
