Imports System.Data
Imports System.Data.OracleClient
Imports baulastenBA

Imports Npgsql

Module tools
    Public logfile As String = "\\gis\gdvell\apps\test\bgm\" & "logs\" ' & Environment.UserName & "_"
    Public probaugGemarkungsdict As New Dictionary(Of Integer, String)
    Public katasterGemarkungslist As New List(Of myComboBoxItem)
    Public gemeindedict As New Dictionary(Of Integer, String)
    Public gem(37) As String
    Public gemeinde(13) As String
    Public katasterGem(35) As String
    Public rawList As New List(Of clsBaulast)
    Public list4Geloscht As New List(Of clsBaulast)
    Public pfad As String = "\\gis\gdvell\fkat\baulasten\"
    Public Const postgresHost As String = "gis"
    Private Const ConnectionString As String = "Data Source=  (DESCRIPTION =  " &
                                            "  (ADDRESS = (PROTOCOL = TCP)(HOST = ora-clu-scan.kreis-of.local)(PORT = 1521))  " &
                                            "  (LOAD_BALANCE = yes)  " &
                                            "  (CONNECT_DATA =    " &
                                            "  (SERVER = DEDICATED)  " &
                                            "    (SERVICE_NAME = bau.kreis-of.local) " &
                                            "   )  );User Id=bauguser;Password=test;"
    Public fstREC As New clsDBspecPG
    Public anzahltiff, anzahl_dateiexitiert, anzahl_blattNrIst0, anzahlKatasterFormellOK, anzahlGeloschte, vierergeloescht, anzahl_mitSerial As Integer
    Sub l(v As String)
        nachricht(v)
    End Sub
    Sub nachricht(ByVal text$)
        My.Log.WriteEntry(text)
    End Sub
    Sub initdb()
        Dim host As String = postgresHost
        Try
            l(" MOD initdb anfang")
            fstREC.mydb = New clsDatenbankZugriff
            fstREC.mydb.Host = host
            fstREC.mydb.username = "postgres" : fstREC.mydb.password = "lkof4"
            fstREC.mydb.Schema = "postgis20"
            fstREC.mydb.Tabelle = "flurkarte.basis_f" : fstREC.mydb.dbtyp = "postgis"
            l(" MOD initdb ende")
        Catch ex As Exception
            l("Fehler in initdb: " & ex.ToString())
        End Try
    End Sub

    Sub istKatnichtOKaberTiffVorhanden(balist As List(Of clsBaulast), ByRef katnichtOKAberMitTiff_summe As String)
        katnichtOKAberMitTiff_summe = ""
        Dim iz As Integer = 0
        Dim summme As New Text.StringBuilder
        Try
            l("istKatnichtOKaberTiffVorhanden---------------------- anfang")
            For Each lok As clsBaulast In balist
                If Not lok.katasterFormellOK Then
                    If lok.dateiExistiert Then
                        summme.Append(" " & lok.gemeindeText & lok.baulastnr & " " & lok.bauortNr & " " & lok.blattnr & Environment.NewLine)
                    End If
                End If
            Next
            katnichtOKAberMitTiff_summe = summme.ToString
            l("istKatnichtOKaberTiffVorhanden---------------------- ende")
        Catch ex As Exception
            l("Fehler in istKatnichtOKaberTiffVorhanden: " & ex.ToString())
        End Try
    End Sub

    Friend Function bildeGeloeschteListe(rawList As List(Of clsBaulast), ByRef anzahlGeloschte As Integer) As List(Of clsBaulast)

        anzahlGeloschte = 0
        Dim newlist As New List(Of clsBaulast)
        'status
        '1 - eintrag
        '2 - änderung
        '3 - 
        '4 - verz gelöscht
        Try
            l("bildeGeloeschteListe---------------------- anfang")
            For Each lok As clsBaulast In rawList
                If lok.datumgeloescht.Trim <> String.Empty Then
                    lok.geloescht = True
                    newlist.Add(lok)
                    anzahlGeloschte += 1
                End If
            Next
            Return newlist
            l("bildeGeloeschteListe---------------------- ende")
        Catch ex As Exception
            l("Fehler inbildeGeloeschteListe : " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Sub istKatasterFormellOK(balist As List(Of clsBaulast), ByRef anzahlKatasterFormellOK As Integer)
        anzahlKatasterFormellOK = 0
        Dim iz As Integer = 0

        Try
            l("istKatasterFormellOK---------------------- anfang")
            For Each lok As clsBaulast In balist
                If lok.katFST.gemcode < 1 Then
                    lok.katasterFormellOK = False
                    Continue For
                End If
                If lok.katFST.flur < 1 Then
                    lok.katasterFormellOK = False
                    Continue For
                End If
                If lok.katFST.zaehler < 1 Then
                    lok.katasterFormellOK = False
                    Continue For
                End If
                lok.katFST.FS = lok.katFST.buildFS()
#If DEBUG Then
                'If lok.katFST.FS = "FS0607570080000300300" Then
                '    Debug.Print("")
                'End If
                'If lok.kennzeichen1 = "4" Then
                '    Debug.Print("")
                'End If
#End If

                anzahlKatasterFormellOK += 1
                lok.katasterFormellOK = True
            Next
            l("istKatasterFormellOK---------------------- ende")
        Catch ex As Exception
            l("Fehler in istKatasterFormellOK: " & ex.ToString())
        End Try
    End Sub

    Function splitKatasterGemarkung() As List(Of myComboBoxItem)
        Dim dict As New List(Of myComboBoxItem)
        Dim a() As String
        Dim my As New myComboBoxItem
        For i = 0 To katasterGem.Count - 1
            my = New myComboBoxItem
            a = katasterGem(i).Replace(vbTab, " ").Split(";"c)
            my.myindex = a(1).Trim
            my.mySttring = (a(0).Trim)
            dict.Add(my)
        Next
        Return dict
    End Function
    Function splitgemeinde() As Dictionary(Of Integer, String)
        Dim dict As New Dictionary(Of Integer, String)
        Dim a() As String
        For i = 0 To gemeinde.Count - 1
            a = gemeinde(i).Trim.Replace(vbTab, "").Split(";"c)
            dict.Add(CInt(a(0).Trim), a(1).Trim)
        Next
        Return dict
    End Function
    Function getbalist2(sql As String) As DataTable
        l("getbalist2 OracleConnection " & sql)
        l("getbalist2 " & ConnectionString)
        Dim oOracleConn As OracleConnection
        Try


            '"Data Source=  (DESCRIPTION =  " &
            '                                "  (ADDRESS = (PROTOCOL = TCP)(HOST = ora-clu-scan.kreis-of.local)(PORT = 1521))  " &
            '                                "  (LOAD_BALANCE = yes)  " &
            '                                "  (CONNECT_DATA =    " &
            '                                "  (SERVER = DEDICATED)  " &
            '                                "    (SERVICE_NAME = bau.kreis-of.local) " &
            '                                "   )  );User Id=bauguser;Password=test;"

            'Dim str = New OracleConnectionStringBuilder
            'str.Add("Data Source", "ora-clu-scan.kreis-of.local")
            'str.Add("port", "1521")
            'str.Add("User ID", "bauguser")
            'str.Add("Password", "test")
            'str.Add("SERVICE NAME", "bau.kreis-of.local")
            'oOracleConn = New Devart.Data.Oracle.OracleConnection(str.ConnectionString)
            'oOracleConn.Open()


            oOracleConn = New OracleConnection(ConnectionString)
            l("vor open")
            oOracleConn.Open()
            nachricht("nach open")
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
            l(" MOD getbalist2 ende")
            Return dt
        Catch oracleex As OracleException
            l("Fehler in getbalist2 ora: " & oracleex.ToString())
        Catch ex As Exception
            l("Fehler in getbalist2: " & ex.ToString())
        End Try
    End Function



    Function splitgem() As Dictionary(Of Integer, String)
        Dim dict As New Dictionary(Of Integer, String)
        Dim a() As String
        For i = 0 To gem.Count - 1
            a = gem(i).Replace(vbTab, " ").Split(" "c)
            dict.Add(CInt(a(0).Trim), a(1).Trim)
        Next
        Return dict
    End Function

    Function dtnachobj(balistDT As DataTable, geschlossen As DataTable) As List(Of clsBaulast)
        Dim nlist As New List(Of clsBaulast)
        Dim lok As New clsBaulast
        Dim a As String
        Dim b As String
        Dim iz As Integer = 0
        Try
            l("dtnachobj ---------------------- anfang")
#If DEBUG Then
            'For i = 0 To 100
            For i = 0 To balistDT.Rows.Count - 1
#Else
            For i = 0 To balistDT.Rows.Count - 1
#End If
                l("nr: " & i & ", " & clsDBtools.fieldvalue(balistDT.Rows(i).Item("a1")).Trim)
                lok = New clsBaulast
                lok.blattnr = clsDBtools.fieldvalue(balistDT.Rows(i).Item("a1")).Trim
                lok.baulastnr = clsDBtools.fieldvalue(balistDT.Rows(i).Item("a2")).Trim
#If DEBUG Then

                If lok.blattnr = "90764" Then
                Debug.Print("")
                End If
#End If
                lok.bauortNr = clsDBtools.fieldvalue(balistDT.Rows(i).Item("a4")).Trim
                lok.probaugFST.gemcode = CInt(clsDBtools.fieldvalue(balistDT.Rows(i).Item("a5")).Trim)
                a = clsDBtools.fieldvalue(balistDT.Rows(i).Item("a6")).Trim
                Console.WriteLine("iz1 " & iz)

                If a.IsNothingOrEmpty Then
                    lok.probaugFST.flur = 0
                Else
                    If IsNumeric(a) Then
                        lok.probaugFST.flur = CInt(a)
                    Else
                        lok.probaugFST.flur = 0
                    End If

                End If

                lok.probaugFST.fstueckKombi = clsDBtools.fieldvalue(balistDT.Rows(i).Item("a7")).Trim
                lok.gueltig = clsDBtools.fieldvalue(balistDT.Rows(i).Item("a8")).Trim
                lok.datum = (clsDBtools.fieldvalue(balistDT.Rows(i).Item("a10")))
                lok.status = clsDBtools.fieldvalue(balistDT.Rows(i).Item("a3")).Trim
                lok.laufnr = CInt(clsDBtools.fieldvalue(balistDT.Rows(i).Item("a9")))
                lok.datum1 = clsDBtools.fieldvalue(balistDT.Rows(i).Item("angelegt")).Trim
                lok.datumgeloescht = clsDBtools.fieldvalue(balistDT.Rows(i).Item("loesch")).Trim
                'b = clsDBtools.fieldvalue(balistDT.Rows(i).Item("a13")).Trim
                If istgeschlossen(lok.blattnr, geschlossen) Then Continue For
                iz += 1
                nlist.Add(lok)
            Next
            Return nlist
            l("dtnachobj ---------------------- ende")
        Catch ex As Exception
            l("Fehler in dtnachobj: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Function istgeschlossen(blattnr As String, geschlossen As DataTable) As Boolean
        Try
            'l(" MOD istgeschlossen anfang")
            For Each ds As DataRow In geschlossen.AsEnumerable
                If CStr(ds.Item(0)) = blattnr Then
                    Return True
                End If
            Next
            Return False
            'l(" MOD istgeschlossen ende")
            Return True
        Catch ex As Exception
            l("Fehler in istgeschlossen: " & ex.ToString())
            Return False
        End Try
    End Function

    Function calcDateiname(lok As clsBaulast) As String
        Dim datei As String
        datei = pfad & lok.probaugFST.gemarkungstext & "\" & lok.blattnr & ".tiff"
        Return datei
    End Function

    Function getBauort(bauortNr As String) As String
        Dim test As Integer
        test = CInt(bauortNr.Trim)
        Dim retval As String = bauortNr
        Try
            retval = gemeindedict(test)
        Catch ex As Exception
            retval = bauortNr
        End Try
        Return retval
    End Function

    Function getTiff(lok As clsBaulast, pfad As Object) As Boolean
        Return False
    End Function

    Function getProbaugGemarkungsText(probaugGemarkung As Integer) As String
        Dim test As Integer
        Dim retval As String
        Try
            l("---------------------- anfang")
            test = CInt(probaugGemarkung)
            retval = CType(probaugGemarkung, String)
            Try
                retval = probaugGemarkungsdict(test)
            Catch lex As Exception
                retval = CType(probaugGemarkung, String)
            End Try

            Return retval
            l("---------------------- ende")
        Catch zex As Exception
            l("Fehler in : " & zex.ToString())
            Return "unbekannt"
        End Try
    End Function



    Function calcDateiExistiert(lok As clsBaulast) As Boolean
        Dim fi As New IO.FileInfo(lok.datei)
        If fi.Exists Then
            fi = Nothing
            Return True
        Else
            fi = Nothing
            Return False
        End If
    End Function
    Function istSchonVorhanden(fS As String) As Boolean
        Dim hinweis As String = ""
        fstREC.mydb.SQL = "select * from paradigma_userdata.hartmann_f   where fs='" & fS & "'"
        l(fstREC.mydb.SQL)
        hinweis = fstREC.getDataDT()
        If fstREC.dt.Rows.Count < 1 Then

            Return False
        Else
            Return True
        End If
    End Function


    Sub initKatasterGemarkungtext()
        katasterGem(0) = "Bieber                             ;725"
        katasterGem(1) = "Buchschlag                         ;726"
        katasterGem(2) = "Bürgel                             ;727"
        katasterGem(3) = "Dietesheim                         ;728"
        katasterGem(4) = "Dietzenbach                        ;729"
        katasterGem(5) = "Dreieichenhain                     ;730"
        katasterGem(6) = "Dudenhofen                         ;731"
        katasterGem(7) = "Egelsbach                          ;732"
        katasterGem(8) = "Froschhausen                       ;733"
        katasterGem(9) = "Götzenhain                         ;734"
        katasterGem(10) = "Hainhausen                         ;735"
        katasterGem(11) = "Hainstadt                          ;736"
        katasterGem(12) = "Hausen                             ;737"
        katasterGem(13) = "Heusenstamm                        ;738"
        katasterGem(14) = "Jügesheim                          ;739"
        katasterGem(15) = "Klein-Krotzenburg                  ;740"
        katasterGem(16) = "Klein-Welzheim                     ;741"
        katasterGem(17) = "Lämmerspiel                        ;742"
        katasterGem(18) = "Langen                             ;743"
        katasterGem(19) = "Mainflingen                        ;744"
        katasterGem(20) = "Messenhausen                       ;745"
        katasterGem(21) = "Mühlheim                           ;746"
        katasterGem(22) = "Nieder-Roden                       ;747"
        katasterGem(23) = "Neu-Isenburg                       ;748"
        katasterGem(24) = "Ober-Roden                         ;749"
        katasterGem(25) = "Offenbach                          ;751"
        katasterGem(26) = "Offenthal                          ;752"
        katasterGem(27) = "Rembrücken                         ;753"
        katasterGem(28) = "Rumpenheim                         ;754"
        katasterGem(29) = "Seligenstadt                       ;755"
        katasterGem(30) = "Sprendlingen                       ;756"
        katasterGem(31) = "Urberach                           ;757"
        katasterGem(32) = "Weiskirchen                        ;758"
        katasterGem(33) = "Zellhausen                         ;759"
        katasterGem(34) = "Zeppelinheim                       ;760"
        katasterGem(35) = "Obertshausen                       ;750"

    End Sub
    Sub initProbaugNrProbaugGemarkungtext()
        gem(0) = "4	Dreieichenhain"
        gem(1) = "5	Sprendlingen"
        gem(2) = "6	Offenthal"
        gem(3) = "7	Götzenhain"
        gem(4) = "8	Buchschlag"
        gem(5) = "9	Hainstadt"
        gem(6) = "10 Klein-Krotzenburg"
        gem(7) = "11 Rembrücken"
        gem(8) = "12 Mainflingen"
        gem(9) = "13 Zellhausen"
        gem(10) = "14	Lämmerspiel"
        gem(11) = "15	Dietesheim"
        gem(12) = "16	Obertshausen"
        gem(13) = "17	Hausen"
        gem(14) = "18	Zeppelinheim"
        gem(15) = "20	Jügesheim"
        gem(16) = "21	Dudenhofen"
        gem(17) = "22	Nieder-Roden"
        gem(18) = "23	Hainhausen"
        gem(19) = "24	Weiskirchen"
        gem(20) = "25	Urberach"
        gem(21) = "26	Ober-Roden"
        gem(22) = "28	Messenhausen"
        gem(23) = "29	Froschhausen"
        gem(24) = "30	Klein-Welzheim"
        gem(25) = "32	Heusenstamm"
        gem(26) = "34	Seligenstadt"
        gem(27) = "35	Egelsbach"
        gem(28) = "36	Mühlheim"
        gem(29) = "40	Dietzenbach"
        gem(30) = "41	Langen"
        gem(31) = "42	Neu-Isenburg"
        gem(32) = "2	Bayerseich"
        gem(33) = "60	Im-Brühl"
        gem(34) = "27	Unbekannt27"
        gem(35) = "3	Unbekannt3"
        gem(36) = "33	Unbekannt33"
        gem(37) = "0	Unbekannt0"
    End Sub
    Function initgemeinde() As String
        gemeinde(0) = "1 ;Dietzenbach                        "
        gemeinde(1) = "2 ;Dreieich                           "
        gemeinde(2) = "3 ;Egelsbach                          "
        gemeinde(3) = "4 ;Hainburg                           "
        gemeinde(4) = "5 ;Heusenstamm                        "
        gemeinde(5) = "6 ;Langen                             "
        gemeinde(6) = "7 ;Mainhausen                         "
        gemeinde(7) = "8 ;Mühlheim                           "
        gemeinde(8) = "9 ;Neu-Isenburg                       "
        gemeinde(9) = "10;Obertshausen                       "
        gemeinde(10) = "0 ;Offenbach                          "
        gemeinde(11) = "11;Rodgau                             "
        gemeinde(12) = "12;Rödermark                          "
        gemeinde(13) = "13;Seligenstadt                       "
        'gemeinde(14) = "8 ;Muehlheim                          "

    End Function

    Function objErweitern(balist As List(Of clsBaulast), ByRef anzahltiff As Integer,
                              ByRef anzahl_dateiexitiert As Integer,
                              ByRef anzahl_blattnrIst0 As Integer) As Boolean
        anzahltiff = 0
        anzahl_dateiexitiert = 0
        anzahl_blattnrIst0 = 0
        Dim iz As Integer = 0
        Try
            l("objErweitern---------------------- anfang")
            For Each lok As clsBaulast In balist
                Try
                    lok.probaugFST.gemarkungstext = getProbaugGemarkungsText(lok.probaugFST.gemcode)
                Catch lex As Exception
                    ' lok.probaugFST.gemarkungstext = "unbekannt" ' (" & lok.probaugFST.gemcode.ToString & ")"
                End Try

                'setKatasterGemarkung(lok, katasterGemarkungsdict)
                'If iz = 7300 Then
                '    Debug.Print("")
                'End If
                Console.WriteLine(iz.ToString & " von " & balist.Count)
                iz += 1
                getKatasterGemarkung(lok, katasterGemarkungslist)
                lok.gemeindeText = getBauort(lok.bauortNr)
                lok.katFST.flur = getKatFlur(lok)
                lok.katFST.fstueckKombi = lok.katFST.buildFstueckkombi
                lok.katFST.zaehler = getKatzaehler(lok)
                If lok.katFST.zaehler < 1 Then
                    getKatZaehlerUndNenner(lok)
                End If

                lok.hatTiff = getTiff(lok, pfad)
                If lok.hatTiff Then anzahltiff += 1
                lok.datei = calcDateiname(lok)
                lok.dateiExistiert = calcDateiExistiert(lok)
                If lok.dateiExistiert Then anzahl_dateiexitiert += 1
                If lok.blattnr = "0" Or lok.blattnr.IsNothingOrEmpty Then
                    anzahl_blattnrIst0 += 1
                End If
            Next
            Return True
            l("objErweitern---------------------- ende")
        Catch ex As Exception
            l("Fehler in objErweitern: " & ex.ToString())
            Return False
        End Try
    End Function

    Sub getKatZaehlerUndNenner(lok As clsBaulast)
        Dim temp, a(), b() As String
        Try
            l("getKatZaehlerUndNenner---------------------- anfang")
            '1468/3 tlw.
            If lok.probaugFST.fstueckKombi.IsNothingOrEmpty Then
                lok.katFST.zaehler = 0
                lok.katFST.nenner = 0
            End If
            temp = lok.probaugFST.fstueckKombi.Replace("\", "/").ToLower
            temp = temp.Replace("//", "/")
            temp = temp.Replace("(", " ")
            temp = temp.Replace(")", " ")
            temp = temp.Replace("a", " ")
            temp = temp.Replace("b", " ")
            temp = temp.Replace("c", " ")
            temp = temp.Replace("d", " ")
            temp = temp.Replace("e", " ")
            temp = temp.Replace("f", " ")
            temp = temp.Replace("g", " ")
            temp = temp.Trim
            If temp.EndsWith("/") Then
                temp = temp.Replace("/", "")
            End If

            If (temp.Contains("/")) Then

                b = temp.Split("/"c)
                'zaehler
                If IsNumeric(b(0)) Then
                    lok.katFST.zaehler = CInt(b(0))
                Else
                    lok.katFST.zaehler = 0
                End If
                'nenner
                If IsNumeric(b(1)) Then
                    lok.katFST.nenner = CInt(b(1))
                Else
                    b(1) = b(1).Replace("-", " ")
                    b(1) = b(1).Replace(".", " ")
                    a = b(1).Split(" "c)
                    If IsNumeric(a(0)) Then
                        lok.katFST.nenner = CInt(a(0))
                    Else
                        lok.katFST.nenner = 0
                    End If
                End If

            Else
                If IsNumeric(temp) Then
                    lok.katFST.zaehler = CInt(temp)

                    lok.katFST.zaehler = 0
                Else
                    lok.katFST.zaehler = 0
                    lok.katFST.nenner = 0
                End If
            End If
            l("getKatZaehlerUndNenner---------------------- ende")
        Catch ex As Exception
            l("Fehler in getKatZaehlerUndNenner: " & ex.ToString())

        End Try
    End Sub

    Function getKatzaehler(lok As clsBaulast) As Integer
        Try
            l("getKatzaehler---------------------- anfang")
            If lok.probaugFST.fstueckKombi.IsNothingOrEmpty Then
                Return 0
            End If
            If IsNumeric(lok.probaugFST.fstueckKombi) Then
                Return CInt(lok.probaugFST.fstueckKombi)
            End If
            Return 0
            l("getKatzaehler---------------------- ende")
        Catch ex As Exception
            l("Fehler in getKatzaehler: " & ex.ToString())
        End Try

    End Function

    Function getKatFlur(lok As clsBaulast) As Integer
        Try
            l("getKatFlur---------------------- anfang")
            If lok.probaugFST.flur < 1 Then
                Debug.Print("")
                Return 0
            End If
            Return lok.probaugFST.flur
            l("getKatFlur---------------------- ende")
        Catch ex As Exception
            l("Fehler in getKatFlur: " & ex.ToString())
        End Try
    End Function

    Private Sub getKatasterGemarkung(lok As clsBaulast, katasterGemarkungslist As List(Of myComboBoxItem))
        Try
            l("getKatasterGemarkung---------------------- anfang")
            For i = 0 To katasterGemarkungslist.Count - 1
                If lok.probaugFST.gemarkungstext.Trim.ToLower = katasterGemarkungslist(i).mySttring.ToLower Then
                    lok.katFST.gemcode = CInt(katasterGemarkungslist(i).myindex.ToLower)
                    Exit Sub
                End If
            Next
            lok.katFST.gemcode = 0
            nachricht("probaugGemarkugnen ohne Kataster:" & lok.probaugFST.gemarkungstext.Trim.ToLower)
            l("getKatasterGemarkung---------------------- ende")
        Catch ex As Exception
            l("Fehler in getKatasterGemarkung: " & ex.ToString())
        End Try
    End Sub
    Friend Function loescheEintragInRawList(geloescht As clsBaulast) As Boolean
        Dim retval As Boolean = False
        Return True
        Try
            l("loescheEintragInRawList---------------------- anfang")
            For Each lok As clsBaulast In rawList
                If lok.bauortNr = geloescht.bauortNr And
                   lok.blattnr = geloescht.blattnr And
                   lok.geloescht = False Then
                    lok.geloescht = True
                    retval = True
                End If
            Next

            l("loescheEintragInRawList---------------------- ende")
            Return retval
        Catch ex As Exception
            l("Fehler in loescheEintragInRawList: " & ex.ToString())
            Return False
        End Try
    End Function

    Sub viererLoeschen(ByRef viererGeloescht As Integer)
        viererGeloescht = 0
        l("viererLoeschen---------------------- anfang")
        Try
            l("viererLoeschen---------------------- anfang")
            For Each geloescht As clsBaulast In list4Geloscht
                If Not geloescht.katasterFormellOK Then Continue For
                'If istSchonVorhanden(lok.katFST.FS) Then
                'End If
                If tools.loescheEintragInRawList(geloescht) Then
                    viererGeloescht += 1
                End If
            Next
            l("viererLoeschen---------------------- ende")
        Catch ex As Exception
            l("Fehler in viererLoeschen: " & ex.ToString())
        End Try
    End Sub

    Friend Function getSerialFromBasis(lok As clsBaulast, Tabname As String) As String
        Dim hinweis As String = ""
        Try
            l("getSerialFromBasis---------------------- anfang")
            fstREC.mydb.SQL = "select ST_AsText(ST_CurveToLine(geom)) from " & Tabname & "   where fs='" & lok.katFST.FS & "'"
            l(fstREC.mydb.SQL)
            hinweis = fstREC.getDataDT()
            If fstREC.dt.Rows.Count < 1 Then
                Return ""
            Else
                Return clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item(0))
            End If
            l("getSerialFromBasis---------------------- ende")
        Catch ex As Exception
            l("Fehler in getSerialFromBasis: " & ex.ToString())
            Return ""
        End Try
    End Function
    Sub getAllSerials(ByRef anzahl_mitSerial As Integer, OUTohneFlurstueck As String)
        Dim temp, tabname As String
        anzahl_mitSerial = 0
        Dim iz As Integer = 0
        tabname = "flurkarte.basis_f"
        Dim sw As New IO.StreamWriter(OUTohneFlurstueck)
        Dim trenn As String = ";"
        sw.WriteLine("gemeinde " & trenn &
                        "baulast: " & trenn &
                        "Bauort " & trenn &
                        "blattnr " & trenn &
                        "Kat. gemnr: " & trenn &
                        "Kat. gemarkung: " & trenn &
                        "Kat. flur: " & trenn &
                        "Kat. zaehler: " & trenn)
        Try
            l("getAllSerials---------------------- anfang")

            For Each lok As clsBaulast In rawList
                Console.WriteLine("getAllSerials " & iz)
                If lok.blattnr = "90764" Then
                    Debug.Print("")
                End If
                iz += 1
                If Not lok.katasterFormellOK Then Continue For
                If lok.geloescht Then Continue For
                temp = tools.getSerialFromBasis(lok, tabname)
                If temp.IsNothingOrEmpty Then
                    'wurde nicht in akt daten gefunden
                    tools.getSerialFromHistBasis(lok, tabname, anzahl_mitSerial)
                    sw.WriteLine(lok.gemeindeText & trenn &
                                  lok.baulastnr & trenn &
                                  lok.bauortNr & trenn &
                                  lok.blattnr & trenn &
                                  lok.katFST.gemcode & trenn &
                                    lok.katFST.gemarkungstext & trenn &
                                  lok.katFST.flur & trenn &
                                  lok.katFST.zaehler & trenn &
                                    lok.gefundenIn & trenn)
                Else
                    lok.serial = temp
                    lok.gefundenIn = tabname
                    anzahl_mitSerial += 1
                End If
            Next
            l("getAllSerials---------------------- ende")
            sw.Close()
            sw.Dispose()
        Catch ex As Exception
            l("Fehler ingetAllSerials : " & ex.ToString())
            sw.Close()
            sw.Dispose()
        End Try
    End Sub

    Private Function getSerialFromHistBasis(lok As clsBaulast, ByRef gefundenin As String, ByRef anzahl_mitSerial As Integer) As String
        Dim basisarray(), tabname, temp As String
        Try
            l("getSerialFromHistBasis---------------------- anfang")
            basisarray = getBasisArray()
            For i = 0 To basisarray.Count - 1
                tabname = "h_flurkarte." & basisarray(i)
                temp = tools.getSerialFromBasis(lok, tabname)
                If temp.IsNothingOrEmpty Then
                    'lok.serial = ""
                    'lok.gefundenIn = "null"
                Else
                    lok.serial = temp
                    lok.gefundenIn = tabname
                    anzahl_mitSerial += 1
                    Return temp
                End If
            Next
            lok.serial = ""
            lok.gefundenIn = "null"
            Return ""
            l("getSerialFromHistBasis---------------------- ende")
        Catch ex As Exception
            l("Fehler in getSerialFromHistBasis: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Function getBasisArray() As String()
        Dim basis(14) As String
        basis(0) = "j2019_basis_f"
        basis(1) = "j2018_basis_f"
        basis(2) = "j2017_basis_f"
        basis(3) = "j2016_basis_f"
        basis(4) = "j2015_basis_f"
        basis(5) = "j2014_basis_f"
        basis(6) = "j2013_basis_f"
        basis(7) = "j2012_basis_f"
        basis(8) = "j2011_basis_f"
        basis(9) = "j2010_basis_f"

        basis(10) = "j1998_flurstueck_f"
        basis(11) = "j1999_flurstueck_f"
        basis(12) = "j2000_flurstueck_f"
        basis(13) = "j2001_flurstueck_f"
        basis(14) = "j2002_flurstueck_f"

        Return basis
    End Function



    Sub write2postgis(lok As clsBaulast, ByRef erfolg As Boolean, ByRef sql As String, coordinatesystemNumber As String, datei As String, datei2 As String)
        Try
            sql = "INSERT INTO paradigma_userdata.hartmann_f " &
                         "(geom,fs,kennzeichen1,baulastnr,jahr_blattnr,bauort,gueltig," &
                         "datum,flur,flurstueck,zaehler,nenner,gefundenin,tiff,gemeinde,gemarkung,gemcode,tiff2) " &
                        "VALUES( ST_GeomFromText('" & lok.serial & "'," & coordinatesystemNumber & "),'" &
                            lok.katFST.FS & "','" &
                            lok.status.Trim & "','" &
                            lok.baulastnr.Trim & "','" &
                            lok.blattnr.Trim & "','" &
                            lok.bauortNr.Trim & "','" &
                            lok.gueltig.Trim & "','" &
                            lok.datum.Trim & "','" &
                            lok.katFST.flur & "','" &
                            lok.katFST.fstueckKombi.Trim & "','" &
                            lok.katFST.zaehler & "','" &
                            lok.katFST.nenner & "','" &
                            lok.gefundenIn & "','" &
                            datei & "','" &
                            lok.gemeindeText & "','" &
                            lok.probaugFST.gemarkungstext & "','" &
                            lok.katFST.gemcode & "','" &
                            datei2 & "')"
            Dim dtRBplus As New DataTable
            erfolg = sqlausfuehren(sql, fstREC.mydb, dtRBplus)

        Catch ex As Exception
            l("fehler in schreibePolygonInUserPG" & ex.ToString)

        End Try
    End Sub

    Private Sub makeConnection(ByVal host As String, datenbank As String, ByVal dbuser As String, ByVal dbpw As String, ByVal dbport As String)
        Dim csb As New NpgsqlConnectionStringBuilder
        Try
            l("makeConnection")
            'If String.IsNullOrEmpty(mydb.ServiceName) Then
            'klassisch
            csb.Host = host
            ' csb. = mydb.Schema
            csb.UserName = dbuser
            csb.Password = dbpw
            csb.Database = datenbank
            csb.Port = CInt(dbport)
            csb.Pooling = False
            csb.MinPoolSize = 1
            csb.MaxPoolSize = 20
            csb.Timeout = 15
            csb.SslMode = SslMode.Disable
            myconn = New NpgsqlConnection(csb.ConnectionString)
            l("makeConnection fertig " & csb.ConnectionString)
        Catch ex As Exception
            l("fehler in makeConnection" & ex.ToString)
        End Try
    End Sub


    Public myconn As NpgsqlConnection

    Function sqlausfuehren(sql As String, Postgis_MYDB As clsDatenbankZugriff, tempdt As DataTable) As Boolean
        '  ini_PGREC(tablename)
        makeConnection(Postgis_MYDB.Host, Postgis_MYDB.Schema, Postgis_MYDB.username, Postgis_MYDB.password, "5432")
        l("in sqlausfuehren")
        l(sql)
        Try
            myconn.Open()
            Dim com As New NpgsqlCommand(sql, myconn)
            Dim da As New NpgsqlDataAdapter(com)
            'da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            ' dtRBplus = New DataTable
            Dim _mycount = da.Fill(tempdt)
            myconn.Close()
            myconn.Dispose()
            com.Dispose()
            da.Dispose()
            l("sqlausfuehren fertig")
            Return True
        Catch ex As Exception
            l("fehler in sqlausfuehren: " & ex.ToString)
            Return False
        End Try
    End Function


    Function getallTiffsinDB(temp As String, postgis_mydb As clsDatenbankZugriff, sql As String) As Boolean
        Dim hinweis As String = ""
        Try
            l(" MOD istInHartmannDB anfang")
            makeConnection(postgis_mydb.Host, postgis_mydb.Schema, postgis_mydb.username, postgis_mydb.password, "5432")
            fstREC.mydb.SQL = sql  '   where lower(trim(tiff2))='" & temp.Trim.ToLower & "'"
            l(fstREC.mydb.SQL)
            hinweis = fstREC.getDataDT()
            If fstREC.dt.Rows.Count < 1 Then
                Return False
            Else
                Return True
            End If
            l(" MOD istInHartmannDB ende")
            Return True
        Catch ex As Exception
            l("Fehler in istInHartmannDB: " & ex.ToString())
            Return False
        End Try
    End Function
End Module
