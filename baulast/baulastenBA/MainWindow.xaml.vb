Imports System.ComponentModel
Imports System.Data
Imports baulastenBA

Class MainWindow
    Public bgmVersion As String = My.Resources.BuildDate.Trim.Replace(vbCrLf, "")
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        setLogfile(logfile) : l("Start " & Now) : l("bgmVersion:" & bgmVersion)
        Title = "BGM - Baulastenprotokoll; " & "Version: " & bgmVersion & "; User: " & Environment.UserName
        If isautho Then
        Else
            Close()
        End If
        initdb()
    End Sub
    Private Shared Function isAutho() As Boolean
        Return Environment.UserName.ToLower = "storcksdieck_a" Or
                    Environment.UserName.ToLower = "hartmann_s" Or
                    Environment.UserName.ToLower = "feinen_j" Or
                    Environment.UserName.ToLower = "zahnlückenpimpf" Or
                    Environment.UserName.ToLower = "kroemmelbein_m"
    End Function
    Sub setLogfile(logfile As String)
        With My.Log.DefaultFileLogWriter
#If DEBUG Then
            '.CustomLocation = mgisUserRoot & "logs\"
            logfile = "d:\" & "" ' & Environment.UserName & "_"
#Else
#End If
            '.CustomLocation = My.Computer.FileSystem.SpecialDirectories.Temp & "\mgis_logs\"
            .CustomLocation = logfile '
            '.BaseFileName = GisUser.username & "_" & Format(Now, "yyyyMMddhhmmss")
            .BaseFileName = Environment.UserName & "_baBatch_" & Format(Now, "yyyyMMddhhmmss")
            .AutoFlush = True
            .Append = False
        End With
    End Sub

    Private Function getgeschlossen(sql As String) As DataTable
        Dim oOracleConn As OracleConnection

        oOracleConn = New OracleConnection("Data Source=  (DESCRIPTION =  " &
                                        "  (ADDRESS = (PROTOCOL = TCP)(HOST = ora-clu-scan.kreis-of.local)(PORT = 1521))  " &
                                        "  (LOAD_BALANCE = yes)  " &
                                        "  (CONNECT_DATA =    " &
                                        "  (SERVER = DEDICATED)  " &
                                        "    (SERVICE_NAME = bau.kreis-of.local) " &
                                        "   )  );User Id=bauguser;Password=test;")
        oOracleConn.Open()
        nachricht("open")
        Dim dt As System.Data.DataTable

        Dim com As OracleCommand
        Dim _mycount As Long
        com = New OracleCommand(sql, oOracleConn) '"select * from " & tabname$
        Dim da As New OracleDataAdapter(com)
        da.MissingSchemaAction = MissingSchemaAction.AddWithKey
        dt = New DataTable
        nachricht("fill")
        Console.WriteLine("vor fill")
        _mycount = da.Fill(dt)

        nachricht("fillfertig: " & _mycount)
        nachricht("in gisview2" & "  wurden  " & _mycount & " datensätze gefunden.=======================")
        oOracleConn.Close()
        com.Dispose()
        da.Dispose()
        Return dt
    End Function

    Private Sub ausgabeKatNichtOk(rawList As List(Of clsBaulast), v As String)
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
                If Not lok.katasterFormellOK Then

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
        End Try
    End Sub

    Private Sub showAllBas()
        Dim evensQuery As Object = builtLinQ(evensQuery)
        dgMain.DataContext = evensQuery
        '___showdispatcher(" " & evensQuery.Count & " baulasten übrig" & Environment.NewLine)
        ___showdispatcher("fertig" & Environment.NewLine)
    End Sub

    Private Sub ___showdispatcher(text As String)
        'tbinfo.Text &= text & Environment.NewLine
        tbinfo.AppendText(text)
        tbinfo.SelectionStart = tbinfo.Text.Length
        tbinfo.ScrollToEnd()
        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents 
        l(text)
    End Sub
    Private Sub dgOSliste_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If dgMain.SelectedItem Is Nothing Then Exit Sub
        e.Handled = True
        Dim aktBL As New clsBaulast
        Try
            l("dgOSliste_SelectionChanged---------------------- anfang")
            aktBL = CType(dgMain.SelectedItem, clsBaulast)
            If aktBL.dateiExistiert Then
                Process.Start(aktBL.datei)
            End If

            l("dgOSliste_SelectionChanged---------------------- ende")
        Catch ex As Exception
            l("Fehler in dgOSliste_SelectionChanged: " & ex.ToString())
        End Try
        e.Handled = True
    End Sub
    Private Sub cbOhneblnr0_Click(sender As Object, e As RoutedEventArgs)
        dgMain.DataContext = Nothing
        Dim evensQuery As Object
        evensQuery = builtLinQ(evensQuery)
        dgMain.DataContext = evensQuery
        e.Handled = True
    End Sub
    Private Function builtLinQ(evensQuery1 As Object) As Object
        Dim evensQuery As Object
        If cbOhneblnr0.IsChecked Then
            evensQuery = From ba In rawList
                         Order By ba.gemeindeText, ba.probaugFST.gemarkungstext
        Else
            evensQuery = From ba In rawList
                         Where ba.blattnr <> "0" And Not ba.blattnr.IsNothingOrEmpty
                         Order By ba.gemeindeText, ba.probaugFST.gemarkungstext
        End If
        ' dgMain.DataContext = evensQuery
        Return evensQuery
    End Function

    Sub btnNurKatOK_Click(sender As Object, e As RoutedEventArgs)
        Dim evensQuery As Object
        ZeigeNurAuszuschreibende(evensQuery)
        dgMain.DataContext = evensQuery
        e.Handled = True
    End Sub


    Private Sub ZeigeNurAuszuschreibende(evensQuery As Object)
        evensQuery = From ba In rawList
                     Where Not ba.geloescht And ba.katasterFormellOK
                     Order By ba.gemeindeText, ba.probaugFST.gemarkungstext
        GC.Collect()
    End Sub

    Private Sub btnAlleBAs_Click(sender As Object, e As RoutedEventArgs)
        showAllBas()
        e.Handled = True
    End Sub
    Private Sub truncateZielTabelle(zieltab As String)
        Dim querey = "TRUNCATE " & zieltab & " RESTART IDENTITY;"
        Dim erfolg As Boolean
        Try
            'l("---------------------- anfang")
            Dim dtRBplus As New System.Data.DataTable
            erfolg = sqlausfuehren(querey, fstREC.mydb, dtRBplus)

            'pgTools.sqlausfuehren(querey)
            'querey = "TRUNCATE " & zieltab & " RESTART IDENTITY;"
            'pgTools.sqlausfuehren(querey)
            'l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub
    Private Sub btnDBausschreiben_Click(sender As Object, e As RoutedEventArgs)
        ___showdispatcher("Geometrie (serial )  wird geholt ...  bitte warten " & Environment.NewLine)

#If DEBUG Then
#Else
        'hartmann_modus  truncateZielTabelle("paradigma_userdata.hartmann_f")

#End If

        getAllSerials(anzahl_mitSerial, "c:\baulastenout\Baulasten_ohneAktFlurstueck" & Now.ToString("yyyyMMddhhmm") & ".csv")
        ___showdispatcher("  BL mit Geometrie: " & anzahl_mitSerial & Environment.NewLine)
        ___showdispatcher("BL werden in die DB geschrieben ...  bitte warten " & Environment.NewLine)
        writeallWithSerials()
        ___showdispatcher("  ausschreiben fertig: " & Environment.NewLine)
        e.Handled = True
    End Sub

    Sub writeallWithSerials()
        Dim iz As Integer = 0
        Dim erfolg As Boolean
        Dim sql As String
        Dim coordinatesystemNumber As String = "25832" '31467"'25832lt mapfile

        Dim datei As String = ""
        Dim datei2 As String = ""
        Try
            l("writeallWithSerials---------------------- anfang")
            For Each lok As clsBaulast In rawList
                Console.WriteLine("getAllSerials " & iz)
                If lok.blattnr = "8001" Then
                    Debug.Print("")
                End If
                If lok.blattnr = "90764" Then
                    Debug.Print("")
                End If
                If Not lok.katasterFormellOK Or lok.geloescht Then Continue For
                If lok.serial.IsNothingOrEmpty Then Continue For
                iz += 1
                datei = lok.datei.Replace("\\gis\gdvell\", "").Replace("\", "/")
                datei = datei.Replace("flurkarte.basis_f", "flurkarte.aktuell")
                datei = datei.Replace("h_flurkarte.j", "hist.Flurkarte.")
                datei = datei.Replace("_flurstueck_f", "")
                datei = datei.Replace("_basis_f", "")
                datei2 = datei
                If lok.dateiExistiert Then
                Else
                    datei = "KeineDaten.htm"
                End If
                ___showdispatcher(" db ausschreiben  " & iz & " (" & anzahl_mitSerial & ")" & Environment.NewLine)
                If lok.geloescht Then Continue For
#If DEBUG Then
                'write2postgis(lok, erfolg, sql, coordinatesystemNumber, datei, datei2)
#Else
                'hartmann_modus    write2postgis(lok, erfolg, sql, coordinatesystemNumber, datei, datei2)
#End If

            Next
            l("writeallWithSerials---------------------- ende")
        Catch ex As Exception
            l("Fehler in writeallWithSerials: " & ex.ToString())
        End Try
    End Sub

    Private Sub btnTIFF_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        checkTiffs()

    End Sub

    Private Shared Sub checkTiffs()
        Dim rootdir As String = "l:\fkat\baulasten"
        Dim temp As String = ""
        'update paradigma_userdata.hartmann_f set tiff2='fkat/baulasten/' || trim(gemarkung) || '/' || trim(jahr_blattnr) || '.tiff'
        Dim sb As New Text.StringBuilder
        getallTiffsinDB(temp, fstREC.mydb, "select lower(tiff2) from paradigma_userdata.hartmann_f")

        Dim tiffFilesArray As String() = IO.Directory.GetFiles(rootdir, "*.tif*", IO.SearchOption.AllDirectories)
        sb.Append("Anzahl Objekt in der GIS-DB: " & fstREC.dt.Rows.Count & Environment.NewLine)
        sb.Append("Anzahl Tiffs unter \fkat\bl: " & tiffFilesArray.Count & Environment.NewLine)
        sb.Append("Folgende tiff-dateien fehlen in der GISdatenbank: " & Environment.NewLine)

        'l:\fkat\baulasten\buchschlag\21181.tiff
        For Each datei In tiffFilesArray
            temp = datei.Replace("l:\", "")
            temp = temp.Replace("\", "/")
            If temp.EndsWith("lnk") Then Continue For
            If isInDB(temp, fstREC.dt) Then
            Else
                sb.Append(temp & Environment.NewLine)
            End If
        Next
        Dim ousgasbe As String = "c:\baulastenout\tiffpruefungInvers.txt"
        IO.File.WriteAllText(ousgasbe, sb.ToString)
        Process.Start(ousgasbe)
    End Sub

    Private Shared Function isInDB(temp As String, dt As DataTable) As Boolean
        For i = 0 To dt.Rows.Count - 1
            If clsDBtools.fieldvalue(dt.Rows(i).Item(0)) = temp.ToLower.Trim Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub btnTIFFnormal_Click(sender As Object, e As RoutedEventArgs)
        Dim rootdir As String = "l:\fkat\baulasten"
        Dim temp As String = ""
        'update paradigma_userdata.hartmann_f set tiff2='fkat/baulasten/' || trim(gemarkung) || '/' || trim(jahr_blattnr) || '.tiff'
        Dim sb As New Text.StringBuilder
        getallTiffsinDB(temp, fstREC.mydb, "select * from paradigma_userdata.hartmann_f order by gemcode")
        sb.AppendLine("Folgende TIFFs fehlen als Dateien")
        Dim tiff, datei As String
        Dim fi As IO.FileInfo
        For i = 0 To fstREC.dt.Rows.Count - 1
            tiff = clsDBtools.fieldvalue(fstREC.dt.Rows(i).Item("tiff2"))
            datei = "l:\" & tiff.Replace("/", "\")
            fi = New IO.FileInfo(datei)
            If fi.Exists Then
                Debug.Print("")
            Else
                sb.AppendLine(datei)
            End If
        Next
        Dim ousgasbe As String = "c:\baulastenout\tiffpruefungnormal" & Now.ToString("yyyyMMddhhmm") & ".txt"
        IO.File.WriteAllText(ousgasbe, sb.ToString)
        Process.Start(ousgasbe)
    End Sub

    Private Sub btnINIT_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim datei = "c:\baulastenout\Baulasten_ohneAktFlurstueck" & Now.ToString("yyyyMMddhhmm") & ".csv"
        ___showdispatcher(" ausgabe nach:  " & datei & Environment.NewLine)
        initprobaugRawlist()
        ___showdispatcher("  " & Environment.NewLine)
        ___showdispatcher(" Geduld, Vernunft und Zeit macht möglich die Unmöglichkeit.  " & Environment.NewLine)
        ___showdispatcher(" " & Environment.NewLine)
        ___showdispatcher("  Protokollerzeugung gestartet: " & rawList.Count & " Objekte werden geprüft !!!" & Environment.NewLine)
        getAllSerials(anzahl_mitSerial, datei)
        ___showdispatcher("  BL mit Geometrie: " & anzahl_mitSerial & Environment.NewLine)
        ___showdispatcher("  BL mit Geometrie: " & anzahl_mitSerial & Environment.NewLine)
        ___showdispatcher("BL werden in die DB geschrieben ...  bitte warten " & Environment.NewLine)
        'writeallWithSerials()
        ___showdispatcher(" protokollausgabe nach:  " & datei & Environment.NewLine)
        ___showdispatcher("  protokoll fertig: " & Environment.NewLine)
        ___showdispatcher("  protokoll wird geöffnet: " & Environment.NewLine)
        Process.Start(datei)
        e.Handled = True
    End Sub

    Private Sub initprobaugRawlist()
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
        balistDT1 = getbalist2(sql)
        geschlossenDT = getbalist2(sqlgeschlossen)
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

        ausgabeKatNichtOk(rawList, "c:\baulastenout\Baulasten_katNichtOK" & Now.ToString("yyyyMMddhhmm") & ".csv")
        Dim purchCount = (From ba In rawList
                          Where Not ba.geloescht And ba.katasterFormellOK
                          Order By ba.gemeindeText, ba.probaugFST.gemarkungstext).Count
        ___showdispatcher("  auszuschreibende Objekte: " & purchCount & Environment.NewLine)
        showAllBas()
    End Sub

    Private Sub btnExplorer_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim a = "c:\baulastenout"
        Process.Start(a)
    End Sub
End Class
