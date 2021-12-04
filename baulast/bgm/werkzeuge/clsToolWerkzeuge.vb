Imports System.Data

Public Class clsToolWerkzeuge

    'ausgabeKatNichtOk(rawList, "c:\baulastenout\Baulasten_katNichtOK" & Now.ToString("yyyyMMddhhmm") & ".csv")
    Shared Function init() As String
        Dim datei As String = tools.baulastenoutDir & "\Baulasten_katNichtOK" & Now.ToString("yyyyMMddhhmm") & ".csv"
        'Dim sql As String

        'l(" MOD holeProBaugDaten anfang")

        'sql = clsProBGTools.getSQLProbaug(2026)
        'clsProBGTools.initBaulastBlattnr(sql)
        'datei = "c:\baulastenout\Baulasten_katNichtOK" & Now.ToString("yyyyMMddhhmm") & ".csv"
        'clsToolWerkzeuge.ausgabeKatNichtOk(rawList, datei)
        Dim sql, sqlgeschlossen As String
        sql = "select * from gisview2 order by feld9 desc"
        sql = "SELECT OBJ01BL.FELD4, OBJ01BL.FELD5, OBJ01BL.FELD9, " &
            "OBJ03.FELD3, OBJ03.FELD4, OBJ03.FELD5, OBJ03.FELD6, OBJ03.FELD7 ,OBJ01BL.FELD1, OBJVG.FELD1, OBJVG.FELD2, " &
            "OBJVG.FELD3, ALB.FELD5, ALB.FELD4 ,OBJ01BL.FELD3,OBJ01BL.FELD6 as krof2,OBJ01BL.FELD7 as krof2 " &
            "FROM OBJ01BL LEFT OUTER JOIN OBJVG ON OBJVG.FELD4 = OBJ01BL.FELD1,  OBJ03 LEFT OUTER JOIN ALB ON ALB.FELD1 = OBJ03.FELD4 AND ALB.FELD2 = OBJ03.FELD5 AND ALB.FELD3 = OBJ03.FELD6 WHERE OBJ01BL.FELD1 = OBJ03.FELD1"

        sql = "SELECT OBJ01BL.FELD4 as a1, OBJ01BL.FELD5 as a2, OBJ01BL.FELD9 as a3, OBJ03.FELD3 as a4, OBJ03.FELD4 as a5, " &
            "OBJ03.FELD5 as a6, OBJ03.FELD6 as a7, OBJ03.FELD7 as a8,OBJ01BL.FELD1 as a9, OBJVG.FELD1 as a10," &
            "OBJVG.FELD2 as a11, OBJVG.FELD3 as a12, ALB.FELD5 as a13, ALB.FELD4 as a14,OBJ01BL.FELD3 as a15,OBJ01BL.FELD6 as angelegt," &
            "OBJ01BL.FELD7 as loesch,OBJ01BL.FELD8 as beschr  " &
            "FROM OBJ01BL LEFT OUTER JOIN OBJVG ON OBJVG.FELD4 = OBJ01BL.FELD1, " &
            " OBJ03 LEFT OUTER JOIN ALB ON ALB.FELD1 = OBJ03.FELD4 AND ALB.FELD2 = OBJ03.FELD5 AND ALB.FELD3 = OBJ03.FELD6 " &
            "WHERE OBJ01BL.FELD1 = OBJ03.FELD1"

        'OBJ01BL.FELD7  is gelöscht datum
        'OBJ01BL.FELD4 is blnr
        sql = sql & " and    OBJ01BL.FELD7 =' '"
        '----------------------------
        'sql = sql & "   and OBJ01BL.FELD4 ='90764' "
        'sql = sql & "   and OBJ01BL.FELD4 ='90764' "
        'sql = sql & "   and OBJ01BL.FELD4 ='20937' " 'pose mehrfach
        'sql = sql & "   and OBJ01BL.FELD4 ='2026' " 'pose mehrfach
        'sql = sql & "   and OBJ01BL.FELD4 =3103  'and OBJ01BL.FELD7 =' '"
        'sql = "select * from gisview2  where feld10=2017  order by feld9 desc"
        sqlgeschlossen = "SELECT  feld3 from obj01bla "
        'order nach laufnr


        'checkTiffs()
        ___showdispatcher(sql & Environment.NewLine)
        initProbaugNrProbaugGemarkungtext() : initgemeinde()
        initKatasterGemarkungtext()
        katasterGemarkungslist = splitKatasterGemarkung()
        probaugGemarkungsdict = splitgem()
        gemeindedict = splitgemeinde()
        ___showdispatcher("gemeinde verzeichnis erstellt" & Environment.NewLine)

        Dim balistDT1 As System.Data.DataTable
        Dim geschlossenDT As System.Data.DataTable

        ___showdispatcher("baulasten einlesen " & Environment.NewLine)
        'balistDT1 = getbalist2Oracle(sql)
        If clsProBGTools.ProbauGIstOracle Then
            balistDT1 = getbalist2Oracle(sql)
        Else
            balistDT1 = clsProBGTools.getbalist2MSSQL(sql)
        End If


        If clsProBGTools.ProbauGIstOracle Then
            geschlossenDT = getbalist2Oracle(sqlgeschlossen)
        Else
            geschlossenDT = clsProBGTools.getbalist2MSSQL(sqlgeschlossen)
        End If

        'geschlossenDT = getbalist2Oracle(sqlgeschlossen)
        ___showdispatcher("baulasten geschlossenDT: " & geschlossenDT.Rows.Count & Environment.NewLine)


        ___showdispatcher("datentabelle " & balistDT1.Rows.Count & " baulasten eingelesen" & Environment.NewLine)
        ___showdispatcher("baulasten liste erstellen ")

        rawList = dtnachobj(balistDT1, geschlossenDT)

        ___showdispatcher(" - abgeschlossen" & Environment.NewLine)
        ___showdispatcher("baulasten liste jetzt erweitern ... ")
        ___showdispatcher("")
        objErweitern(rawList, anzahltiff, anzahl_dateiexitiert, anzahl_blattNrIst0)
        ___showdispatcher("prüfen ob katasterdaten Ok " & Environment.NewLine)
        istKatasterFormellOK(rawList, anzahlKatasterFormellOK)
        ___showdispatcher("prüfen ob katasterdaten Ok  - abgeschlossen" & Environment.NewLine)
        ___showdispatcher("Liste der als gelöscht markierten Objekte bilden" & Environment.NewLine)

        list4Geloscht = tools.bildeGeloeschteListe(rawList, anzahlGeloschte)

        ___showdispatcher("Liste der als gelöscht markierten Objekte  - abgeschlossen" & Environment.NewLine)
        ___showdispatcher("Alle als gelöscht markierten objekte löschen" & Environment.NewLine)
        viererLoeschen(vierergeloescht)
        ___showdispatcher("Alle als gelöscht markierten  Objekte löschen - abgeschlossen " & Environment.NewLine)
        Dim katnichtOKAberMitTiff_summe As String
        ___showdispatcher("Prüfen ob Baulasten mit Tiff aber ohne Katasterangaben " & Environment.NewLine)

        istKatnichtOKaberTiffVorhanden(rawList, katnichtOKAberMitTiff_summe)

        ___showdispatcher("Prüfen ob Baulasten mit Tiff aber ohne Katasterangaben  - abgeschlossen" & Environment.NewLine)
        ___showdispatcher("baulasten liste jetzt erweitern - abgeschlossen " & Environment.NewLine)
        ' showdispatcher("    mit Tiff-Datei: " & anzahltiff)
        ___showdispatcher("   Tiff-Datei existiert: " & anzahl_dateiexitiert & Environment.NewLine)
        ___showdispatcher("   BlattNr = 0: " & anzahl_blattNrIst0 & Environment.NewLine)
        ___showdispatcher("   KatasterOK: " & anzahlKatasterFormellOK & Environment.NewLine)
        ___showdispatcher("   katnichtOKAberMitTiff_summe: " & Environment.NewLine & katnichtOKAberMitTiff_summe & Environment.NewLine)
        ___showdispatcher("   anzahlGeloschtMarkiert: " & anzahlGeloschte & Environment.NewLine)
        ___showdispatcher("   real gelöscht: " & vierergeloescht & Environment.NewLine)
        ' getAllSerials(anzahl_mitSerial, OUTohneFlurstueck:="c:\baulastenout\ohneFlurstueck.txt")
        clsToolWerkzeuge.ausgabeKatNichtOk(rawList, datei)

        Return datei
    End Function

    Private Shared Sub ___showdispatcher(v As String)

    End Sub

    Shared Sub ausgabeKatNichtOk(rawList As List(Of clsBaulast), v As String)
        Dim summme As New Text.StringBuilder
        Dim trenn As String = ";"
        Try
            ' summme.Append("Baulasten mit defekten Flurstücksansprachen" & Environment.NewLine)

            summme.Append("gemeinde " & trenn &
                        "baulast: " & trenn &
                        "Bauort " & trenn &
                        "blattnr " & trenn &
                        "Kat. gemarkung: " & trenn &
                        "Kat. gemarkungtext: " & trenn &
                        "Kat. flur: " & trenn &
                        "Kat. zaehler: " & trenn &
                        Environment.NewLine)


            l("istKatnichtOKaberTiffVorhanden---------------------- anfang")
            For Each lok As clsBaulast In rawList
                If lok.katasterFormellOK Then

                    Debug.Print("  ok")
                Else

                    'summme.Append("gemeinde " & lok.gemeindeText & ", baulast: " & lok.baulastnr &
                    '              ", Bauort " & lok.bauortNr & ", blattnr " & lok.blattnr &
                    '              ", Kat. gemarkung: " & lok.katFST.gemcode &
                    '                  ", Kat. flur: " & lok.katFST.flur &
                    '                      ", Kat. zaehler: " & lok.katFST.zaehler &
                    '              Environment.NewLine) 
                    'Dim fst As New clsFlurstuecklok.katfst.gem
                    'fst.gemcode = lok.ge

                    If lok.katFST.gemarkungstext = String.Empty Then
                        lok.katFST.gemarkungstext = lok.katFST.gemparms.gemcode2gemarkungstext(lok.katFST.gemcode)
                    End If

                    summme.Append(lok.gemeindeText & trenn &
                                  lok.baulastnr & trenn &
                                  lok.bauortNr & trenn &
                                  lok.blattnr & trenn &
                                  lok.katFST.gemcode & trenn &
                                           lok.katFST.gemarkungstext & trenn &
                                  lok.katFST.flur & trenn &
                                  lok.katFST.zaehler & trenn &
                                Environment.NewLine)
                End If
            Next
            IO.File.WriteAllText(v, summme.ToString)
        Catch ex As Exception
            Debug.Print("fehler")
            l("fehler in ausgabeKatNichtOk: " & ex.ToString)
        End Try
    End Sub



End Class
