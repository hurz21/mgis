Imports System.Data
Imports mgis

Public Class clsFSTtools
    Shared Function extractSchemaTab(ByRef fstTabDef As clsTabellenDef, afst As ParaFlurstueck) As Boolean
        Dim a() As String
        Try
            l(" extractSchemaTab ---------------------- anfang")
            a = afst.name.Split("."c)
            fstTabDef.tabelle = a(1).Trim
            fstTabDef.Schema = a(0).Trim


            l(" extractSchemaTab ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in extractSchemaTab: " & ex.ToString())
            Return False
        End Try
    End Function
    Shared Function erzeugeundOeffneEigentuemerPDF(tbWeitergabeVerbot As String, aalbverbotsString As String) As String
        Dim lokalitaet, flaeche As String
        'Dim ausgabeDIR As String = My.Computer.FileSystem.SpecialDirectories.Temp '& "" & aid
        'ausgabeDIR = tools.calcEigentuemerAusgabeFile 'My.Computer.FileSystem.SpecialDirectories.MyDocuments
        lokalitaet = getlokalitaetstring(aktFST)
        flaeche = clsFSTtools.getFlaecheZuFlurstueck(aktFST)
        lokalitaet = lokalitaet & " " & flaeche
        Dim ausgabedatei As String = tools.calcEigentuemerAusgabeFile
        'EigentuemerPDF = ausgabeDIR & "\eigentuemer" & Format(Now, "ddMMyyyy_hhmmss") & ".pdf"
        wrapItextSharp.createSchnellEigentuemer(tbWeitergabeVerbot, ausgabedatei, aalbverbotsString, lokalitaet)
        Return ausgabedatei
    End Function
    Shared Function dt2objFst(dtaktuell As DataTable) As List(Of clsFlurauswahl)
        Dim tobj As New clsFlurauswahl

        Dim neu As New List(Of clsFlurauswahl)
        For i = 0 To dtaktuell.Rows.Count - 1
            tobj = New clsFlurauswahl
            tobj.id = CInt(clsDBtools.fieldvalue(dtaktuell.Rows(i).Item(0)))
            tobj.nenner = clsDBtools.fieldvalue(dtaktuell.Rows(i).Item(1))

            If tobj.nenner <> String.Empty Then
                tobj.displayText = tobj.id & "/" & tobj.nenner
            Else
                tobj.displayText = CType(tobj.id, String)
            End If
            neu.Add(tobj)
        Next
        Return neu
    End Function
    Shared Function initGemarkungsListview() As List(Of clsFlurauswahl)

        Dim a(32) As String
        Try
            l(" MOD ---------------------- anfang")
            a(1) = "Buchschlag ;726"
            a(2) = "Dietesheim ;728"
            a(3) = "Dietzenbach ;729"
            a(4) = "Dreieichenhain ;730"
            a(5) = "Dudenhofen ;731"
            a(6) = "Egelsbach ;732"
            a(7) = "Froschhausen ;733"
            a(8) = "Götzenhain ;734"
            a(9) = "Hainhausen ;735"
            a(10) = "Hainstadt ;736"
            a(11) = "Hausen ;737"
            a(12) = "Heusenstamm ;738"
            a(13) = "Jügesheim ;739"
            a(14) = "Klein-Krotzenburg ;740"
            a(15) = "Klein-Welzheim ;741"
            a(16) = "Lämmerspiel ;742"
            a(17) = "Langen ;743"
            a(18) = "Mainflingen ;744"
            a(19) = "Messenhausen ;745"
            a(20) = "Mühlheim ;746"
            a(21) = "Nieder-Roden ;747"
            a(22) = "Neu-Isenburg ;748"
            a(23) = "Ober-Roden ;749"
            a(24) = "Obertshausen ;750"
            a(25) = "Offenthal ;752"
            a(26) = "Rembrücken ;753"
            a(27) = "Seligenstadt ;755"
            a(28) = "Sprendlingen ;756"
            a(29) = "Urberach ;757"
            a(30) = "Weiskirchen ;758"
            a(31) = "Zellhausen ;759"
            a(32) = "Zeppelinheim ;760"
            Dim b() As String
            Dim gemListe As New List(Of clsFlurauswahl)
            Dim temp As New clsFlurauswahl
            For i = 1 To a.Length - 1
                b = a(i).Split(";"c)
                temp = New clsFlurauswahl
                temp.id = CInt(b(1))
                temp.displayText = (b(0))
                gemListe.Add(temp)
            Next
            Return gemListe

            l(" MOD ---------------------- ende")
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Shared Function initFlureListe() As List(Of clsFlurauswahl)
        Dim dt As DataTable = holeFlureDT()
        'cmbFlur.DataContext = dt
        Dim flurnummern As New List(Of clsFlurauswahl)
        Dim aflur As New clsFlurauswahl
        For i = 0 To dt.Rows.Count - 1
            aflur = New clsFlurauswahl
            aflur.id = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item(0)))
            aflur.displayText = clsDBtools.fieldvalue(dt.Rows(i).Item(0))
            flurnummern.Add(aflur)
        Next
        Return flurnummern
    End Function
    Shared Sub dossierPrepMinimum()
        Dim utm As New Point
        utm.X = aktFST.punkt.X
        utm.Y = aktFST.punkt.Y
        Dim KoordinateKLickpt As New Point
        KoordinateKLickpt.X = 1
        KoordinateKLickpt.Y = 1
        'globCanvasWidth = 2
        'globCanvasHeight = 2



        clsSachdatentools.getdossier(utm, layerActive.aid,
                                            CInt(2), CInt(2),
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
