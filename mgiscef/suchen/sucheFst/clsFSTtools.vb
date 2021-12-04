Imports System.Data
Imports mgis

Public Class clsFSTtools

    Shared Function extractSchemaTab(ByRef fstTabDef As clsTabellenDef, afstName As String) As Boolean
        Dim a() As String
        Try
            l(" extractSchemaTab ---------------------- anfang")
            a = afstName.Split("."c)
            fstTabDef.tabelle = a(1).Trim
            fstTabDef.Schema = a(0).Trim
            l(" extractSchemaTab ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in extractSchemaTab: " & ex.ToString())
            Return False
        End Try
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
            l("Fehler in initGemarkungsListview: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Shared Function initFlureListe() As List(Of clsFlurauswahl)
        Dim flurnummern As New List(Of clsFlurauswahl)
        Dim hinweis As String = ""
        Dim aflur As New clsFlurauswahl
        If iminternet Or CGIstattDBzugriff Then
            flurnummern = clsFSTtools.getflurnummernlisteFromHTTP(aktFST.normflst.gemcode, WinDetailSucheFST.AktuelleBasisTabelle, hinweis)
        Else
            Dim dt As DataTable = holeFlureDT()
            For i = 0 To dt.Rows.Count - 1
                aflur = New clsFlurauswahl
                aflur.id = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item(0)))
                aflur.displayText = clsDBtools.fieldvalue(dt.Rows(i).Item(0))
                flurnummern.Add(aflur)
            Next
        End If
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
        Dim result As String = "", hinweis As String = ""
        Dim sql As String
        Try
            l(" getFlaecheZuFlurstueck ---------------------- anfang")
            sql = "select st_area(geom),flaeche from flurkarte.basis_f where fs='" & aktFST.normflst.FS & "'"
            If iminternet Or CGIstattDBzugriff Then
                result = clsToolsAllg.getSQL4Http(sql, "postgis20", hinweis, "getsql") : l(hinweis)
                result = result.Replace("$", "").Replace(vbCrLf, "")
                Return (result.Trim)
            Else
                Dim dt As System.Data.DataTable
                dt = getDTFromWebgisDB(sql, "postgis20")
                If dt.Rows.Count > 0 Then
                    'Return ToString("0.00"))
                    Return "Fläche: " & calcFlaeche(clsDBtools.fieldvalue(dt.Rows(0).Item(0))) & " qm (GIS-Wert), " &
                                          clsDBtools.fieldvalue(dt.Rows(0).Item(1)) & " qm (Grundbuch-Wert)"
                Else
                    Return ""
                End If
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
    Shared Function getflurnummernlisteFromHTTP(gemcode As Integer, aktuelleBasisTabelle As String, hinweis As String) As List(Of clsFlurauswahl)
        Dim result As String
        Dim flurnummern As New List(Of clsFlurauswahl)
        Try
            l(" MOD getflurnummernlisteFromHTTP---------------------- anfang")
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick &
                    "&modus=getflure&gemarkung=" & gemcode &
                    "&tabelle=" & aktuelleBasisTabelle
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            result = result.Trim
            If result.IsNothingOrEmpty Then
                Return Nothing
            End If
            flurnummern = ajaxflurnummernliste(result)
            If flurnummern Is Nothing Then
                l("fehler in getflurnummernlisteFromHTTP " & gemcode & " , " & aktuelleBasisTabelle)
            End If
            Return flurnummern
        Catch ex As Exception
            l("Fehler beim getflurnummernlisteFromHTTP ", ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function ajaxflurnummernliste(result As String) As List(Of clsFlurauswahl)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim lok As New List(Of clsFlurauswahl)
        Dim flur As New clsFlurauswahl
        Dim oldname As String = ""
        Try
            l(" ajaxflurnummernliste html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in ajaxflurnummernliste: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            flur = New clsFlurauswahl
            For i = 0 To zeilen - 1
                flur = New clsFlurauswahl
                b = a(i).Split("#"c)
                flur.id = CInt(b(0).Trim)
                flur.displayText = b(0).Trim
                lok.Add(flur)
            Next
            Return lok
            l(" ajaxflurnummernliste ---------------------- ende")
        Catch ex As Exception
            l("Fehler in ajaxflurnummernliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Friend Shared Function getFSTlisteFromHTTP(gemcode As Integer, flur As Integer, aktuelleBasisTabelle As String, hinweis As String) As List(Of clsFlurauswahl)
        Dim result As String
        Dim fsts As New List(Of clsFlurauswahl)
        Try
            l(" MOD getFSTlisteFromHTTP---------------------- anfang")
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick &
                    "&modus=getfst&gemarkung=" & gemcode &
                    "&flur=" & flur &
                    "&tabelle=" & aktuelleBasisTabelle
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            result = result.Trim
            If result.IsNothingOrEmpty Then
                Return Nothing
            End If
            fsts = ajaxFSTliste(result)
            Return fsts
        Catch ex As Exception
            l("Fehler beim getFSTlisteFromHTTP ", ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function ajaxFSTliste(result As String) As List(Of clsFlurauswahl)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim lok As New List(Of clsFlurauswahl)
        Dim fst As New clsFlurauswahl
        Dim oldname As String = ""
        Try
            l(" ajaxFSTliste html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in ajaxFSTliste: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            fst = New clsFlurauswahl
            For i = 0 To zeilen - 1
                fst = New clsFlurauswahl
                b = a(i).Split("#"c)
                fst.id = CInt(b(0).Trim)
                fst.nenner = b(1).Trim
                If fst.nenner <> String.Empty Then
                    fst.displayText = fst.id & "/" & fst.nenner
                Else
                    fst.displayText = CType(fst.id, String)
                End If
                lok.Add(fst)
            Next
            Return lok
            l(" ajaxFSTliste ---------------------- ende")
        Catch ex As Exception
            l("Fehler in ajaxFSTliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function

End Class
