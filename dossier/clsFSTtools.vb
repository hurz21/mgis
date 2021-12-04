Imports mgis

Public Class clsFSTtools
    Shared Sub NennerVerarbeiten(ByVal nennertext As String)
        m.aktFST.normflst.nenner = CInt(nennertext)
        nennerUndFSPruefen()
    End Sub
    Public Shared Sub nennerUndFSPruefen()
        m.aktFST.normflst.FS = m.aktFST.normflst.buildFS()
        m.aktFST.normflst.fstueckKombi = m.aktFST.normflst.buildFstueckkombi
    End Sub
    ''' <summary>
    ''' holt sich die koordinate des extends des flurstücks
    ''' berechnet daraus letztlich den kartengen.aktMap.aktrange
    ''' der sollte auch beim speichern des flurstücks verwendet werden
    ''' setzt:  clsStartup.aktfst.normflst.GKhoch,aktFST.punkt.X,aktGlobPoint.strX
    ''' </summary>
    'Shared Sub holeKoordinaten4Flurstueck(nennertext As String)
    '    NennerVerarbeiten(nennertext)
    '    clsStartup.aktFST.normflst.FS = clsStartup.aktFST.normflst.buildFS
    '    FSGKrechtsGKHochwertHolen(clsStartup.aktFST.normflst)
    '    clsStartup.aktFST.normflst.GKhoch = CInt(clsStartup.aktFST.normflst.GKhoch)
    '    clsStartup.aktFST.normflst.GKrechts = CInt(clsStartup.aktFST.normflst.GKrechts)
    '    clsStartup.aktFST.punkt.X = clsStartup.aktFST.normflst.GKrechts
    '    clsStartup.aktFST.punkt.Y = clsStartup.aktFST.normflst.GKhoch
    '    aktGlobPoint.strX = CType(CInt(clsStartup.aktFST.normflst.GKrechts), String)
    '    aktGlobPoint.strY = CType(CInt(clsStartup.aktFST.normflst.GKhoch), String)
    '    Dim minradius As Integer
    '    minradius = calcMinradius(clsStartup.aktFST.normflst.radius)
    '    kartengen.aktMap.aktrange = calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, minradius)
    '    'MsgBox("kartengen.aktMap.aktrange  " & kartengen.aktMap.aktrange.toString)
    'End Sub
    'Shared Sub fstnachParadigmaSpeichern(freitext As String, kurz As String)
    '    Dim erfolg As Boolean
    '    Dim radius As Integer
    '    Try
    '        clsStartup.aktFST.abstract = clsStartup.aktFST.setcoordsAbstract()
    '        clsStartup.aktFST.Freitext = freitext
    '        clsStartup.aktFST.name = kurz
    '        clsStartup.aktFST.normflst.gemeindename = clsString.Capitalize(clsStartup.aktFST.normflst.gemeindename)
    '        clsStartup.aktFST.typ = RaumbezugsTyp.Flurstueck
    '        clsStartup.aktFST.isMapEnabled = True
    '        clsStartup.aktFST.normflst.fstueckKombi = clsStartup.aktFST.normflst.buildFstueckkombi()
    '        'aktFST.normflst.FS = ""
    '        radius = 100
    '        clsStartup.aktFST.box.rangekopierenVon(aktrange)
    '        'modEW.Paradigma_Adresse_Neu(radius)
    '        erfolg = modParadigma.NeuesFSTspeichern(radius)
    '        If erfolg Then
    '            MessageBox.Show("Flurstück wurde erfolgreich in Paradigma gespeichert.")
    '        Else
    '            MessageBox.Show("Flurstück wurde NICHT erfolgreich in Paradigma gespeichert.")
    '        End If
    '    Catch ex As Exception
    '        l("fehler in adresseNaCHpARADIGMA ", ex)
    '    End Try
    'End Sub

    Friend Shared Function getFlaecheZuFlurstueck(aktFST As ParaFlurstueck,
                                                  ByRef strError As String) As String
        Try
            l(" getFlaecheZuFlurstueck ---------------------- anfang")
            Dim sql As String = "select st_area(geom),flaeche,weistauf,zeigtauf from flurkarte.basis_f where fs='" & m.aktFST.normflst.FS & "'"
            Dim dt As System.Data.DataTable
            dt = clsPgtools.getDTFromWebgisDB(sql, "postgis20", strError)
            If dt.Rows.Count > 0 Then
                'Return ToString("0.00"))
                aktFST.normflst.weistauf = clsDBtools.fieldvalue(dt.Rows(0).Item("weistauf")).Trim
                aktFST.normflst.zeigtauf = clsDBtools.fieldvalue(dt.Rows(0).Item("zeigtauf")).Trim
                aktFST.normflst.flaecheGrundbuch = clsDBtools.fieldvalue(dt.Rows(0).Item("flaeche"))

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
