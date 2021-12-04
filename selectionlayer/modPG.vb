
Imports Npgsql
Imports Devart.Data.Oracle
Imports Devart.Data
Module modPG

    Public coordinatesystemNumber As String = "25832" '31467"'25832lt mapfile



    Public myconn As NpgsqlConnection

    Public Property dtRBplus As DataTable
    Function sqlausfuehren(sql As String, Postgis_MYDB As LIBDB.clsDatenbankZugriff, tempdt As DataTable) As Boolean
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

    Friend Function userLayerActiveDirErzeugen(tablename As String, aid As Integer) As Integer
        Dim sql As String
        Dim userlayeraid As Integer
        Dim erfolg As Boolean
        Try
            l("userLayerActiveDirErzeugen----------------------")
            dtRBplus = New DataTable
            'sid 47 ist unsichtb ar
            sql = "insert into public.gruppe2aid (aid) " &
            "      values (" & aid & " ) returning id"
            '#########  umstellen des schemas
            Postgis_MYDB.Schema = "webgiscontrol"
            'erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
            l("userLayerActiveDirErzeugensql: " & sql)
            userlayeraid = ZeileEinfuegen(sql, Postgis_MYDB, dtRBplus)
            l("userlayeraid " & userlayeraid)
            Postgis_MYDB.Schema = "postgis20"
            '#########  umstellen des schemas
            l("userLayerActiveDirErzeugen erfolg: " & erfolg)
            Return userlayeraid
        Catch ex As Exception
            l("fehler in userLayerActiveDirErzeugen: " & tablename)
            Return 0
        End Try
    End Function

    Function getval(sql As String, Postgis_MYDB As LIBDB.clsDatenbankZugriff, tempdt As DataTable) As Integer
        '  ini_PGREC(tablename)
        makeConnection(Postgis_MYDB.Host, Postgis_MYDB.Schema, Postgis_MYDB.username, Postgis_MYDB.password, "5432")
        l("in sqlausfuehren")
        l(sql)
        Dim serialNumber As Integer
        Try
            myconn.Open()
            Dim com As New NpgsqlCommand(sql, myconn)
            'Dim da As New NpgsqlDataAdapter(com)
            'da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            ' dtRBplus = New DataTable

            'If ReturnIdentity Then
            '    querie = querie & " returning " & returnColumn & " "
            'End If


            serialNumber = CInt(com.ExecuteScalar)
            myconn.Close()
            myconn.Close()
            myconn.Dispose()
            com.Dispose()
            'da.Dispose()
            l("getval: " & serialNumber)
            Return serialNumber

            l("sqlausfuehren fertig")
        Catch ex As Exception
            l("fehler in sqlausfuehren: " & ex.ToString)
            Return 0
        End Try
    End Function
    Function ZeileEinfuegen(sql As String, Postgis_MYDB As LIBDB.clsDatenbankZugriff, tempdt As DataTable) As Integer
        '  ini_PGREC(tablename)
        makeConnection(Postgis_MYDB.Host, Postgis_MYDB.Schema, Postgis_MYDB.username, Postgis_MYDB.password, "5432")
        l("in sqlausfuehren")
        l(sql)
        Dim serialNumber As Integer
        Try
            myconn.Open()
            Dim com As New NpgsqlCommand(sql, myconn)
            'If ReturnIdentity Then
            '    querie = querie & " returning " & returnColumn & " "
            'End If
            serialNumber = CInt(com.ExecuteScalar)
            myconn.Close()
            myconn.Close()
            myconn.Dispose()
            com.Dispose()
            'da.Dispose()
            If serialNumber < 1 Then
                l("fehler Problem beim Abspeichern:" & sql)
                Return 0
            Else
                Return serialNumber
            End If
            l("sqlausfuehren fertig")
        Catch ex As Exception
            l("fehler in sqlausfuehren: " & ex.ToString)
            Return 0
        End Try
    End Function



    Sub ini_PGREC(tablename As String)
        With Postgis_MYDB
            .Host = CType(iniDict("postgres_MYDB.MySQLServer"), String)
            .Schema = CType(iniDict("postgres_MYDB.Schema"), String)
            .Tabelle = tablename    'CType(iniDict("postgres_MYDB.Tabelle"), String)
            .ServiceName = "paradigma_userdata" '' CType(iniDict("postgres_MYDB.ServiceName"), String)
            .username = CType(iniDict("postgres_MYDB.username"), String)
            .password = CType(iniDict("postgres_MYDB.password"), String)
            .dbtyp = CType(iniDict("postgres_MYDB.dbtyp"), String)
            '  webgisREC = CType(setDbRecTyp(Webgis_MYDB), LIBDB.IDB_grundfunktionen)
            ' webgisREC.mydb = CType(.Clone, LIBDB.clsDatenbankZugriff)
        End With
        'host = "w2gis02" : datenbank = "postgis20" : schema = "flurkarte" : tabelle = "basis_f" : dbuser = "postgres" : dbpw = "lkof4" : dbport = "5432"

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
    Function pgDBtableAnlegen(ByRef summe As String) As Integer
        l(" in pgDBtableAnlegen")
        Dim sql As String
        Dim erfolg As Boolean
        Postgis_MYDB.Tabelle = Postgis_MYDB.Tabelle.ToLower.Trim
        Postgis_MYDB.ServiceName = Postgis_MYDB.ServiceName.ToLower.Trim
        Dim sequencename1 As String = Postgis_MYDB.ServiceName & "." & Postgis_MYDB.Tabelle & "_gid_seq"
        sequencename1 = sequencename1.ToLower
        Dim sequencename2 As String = Postgis_MYDB.ServiceName & "." & Chr(34) & Postgis_MYDB.Tabelle & "_gid_seq" & Chr(34)
        sequencename2 = sequencename2.ToLower
        dtRBplus = New DataTable

        sql = "DROP TABLE if exists  " & Postgis_MYDB.ServiceName & "." & Postgis_MYDB.Tabelle.ToLower & " CASCADE;"
        erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
        sql = "DROP SEQUENCE if exists " & sequencename2 & " CASCADE;"
        erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
        sql = "DROP INDEX if exists " & Chr(34) & Postgis_MYDB.ServiceName & "." & Postgis_MYDB.Tabelle & "_geom_1382367448886" & Chr(34) & " CASCADE;"


        erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
        summe = summe & sql
        If Not erfolg Then Return -1

        sql =
              "CREATE SEQUENCE " & sequencename2 &
                "  INCREMENT 1" &
                "  MINVALUE 1" &
                "  MAXVALUE 9223372036854775807" &
                "  START 1" &
                "  CACHE 1;" &
                "ALTER SEQUENCE " & sequencename2 &
                "  OWNER TO postgres;"

        erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
        summe = summe & sql
        If Not erfolg Then Return -2
        tablename = tablename.ToLower
        sql =
                "CREATE TABLE  " & Postgis_MYDB.ServiceName & "." & Chr(34) & tablename.ToLower & Chr(34) & "" &
                " (" &
                "  gid integer NOT NULL DEFAULT nextval(' " & sequencename2 & "'::regclass)," &
                "  art character(50)," &
                "  name character(100)," &
                "  RBTITEL character(250)," &
                "  RBTYP character(1)," &
                "  FarbeFuell character(100)," &
                "  FarbeGrenz character(100)," &
                "  SYMBOL character(100)," &
                "  RAUMBEZUGSID character(100)," &
                "  VID character(100)," &
                "  illegstatus integer," &
                "  gebiet character(300)," &
                "  geom geometry(Geometry,25832)," &
                "  CONSTRAINT  " & Chr(34) & Postgis_MYDB.Tabelle & "_pkey" & Chr(34) & " PRIMARY KEY (gid)" &
                ") " &
                " WITH (" &
                "  OIDS=FALSE" &
                ");ALTER TABLE " & Postgis_MYDB.ServiceName & "." & Chr(34) & Postgis_MYDB.Tabelle.ToLower & Chr(34) & "  OWNER TO postgres;"

        erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
        summe = summe & sql
        If Not erfolg Then Return -3

        sql = "CREATE INDEX " & Chr(34) & Postgis_MYDB.Tabelle & "_geom_1382367448886" & Chr(34) &
                "  ON " & Postgis_MYDB.ServiceName & "." & Chr(34) & Postgis_MYDB.Tabelle & Chr(34) & "" &
                "  USING gist" &
                "  (geom);"
        erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
        'summe = summe & sql
        If Not erfolg Then Return -4
        Return 1
    End Function

    Private Sub KontrollAusgabeRBplus(ByVal i As Integer)
        Try
            l("KontrollAusgabeRBplus-------------")
            l(CStr(clsDBtools.fieldvalue(dtRBplus.Rows(i).Item("vorgangsid"))))
            l(CStr(clsDBtools.fieldvalue(dtRBplus.Rows(i).Item("TYP"))))
            l(CStr(clsDBtools.fieldvalue(dtRBplus.Rows(i).Item("rechts"))))
            l(CStr(clsDBtools.fieldvalue(dtRBplus.Rows(i).Item("hoch"))))
            l(CStr(clsDBtools.fieldvalue(dtRBplus.Rows(i).Item("freitext"))))
            l(CStr(clsDBtools.fieldvalue(dtRBplus.Rows(i).Item("abstract"))))
            l(CStr(clsDBtools.fieldvalue(dtRBplus.Rows(i).Item("titel"))))
            l(CStr(clsDBtools.fieldvalue(dtRBplus.Rows(i).Item("ismapenabled"))))
            l(CStr(clsDBtools.fieldvalue(dtRBplus.Rows(i).Item("RAUMBEZUGSID"))))
            l("KontrollAusgabeRBplus ende")
        Catch ex As Exception
            l("fehler in KontrollAusgabeRBplus: " & ex.ToString)
        End Try
    End Sub
    Function doRBschleife(dtplus As DataTable, dtPolygon As DataTable, sachgebiet As String, ByRef returnstring As String) As Integer
        l("in doRBschleife")
        l("anzahl objekte: " & dtplus.Rows.Count)
        Dim tempDT As New DataTable
        Dim anzahlErfolgreich As Integer = 0
        Dim vid As String = ""
        Dim oldvid As String = ""
        Dim marker As String = ""
        Dim sw As New Text.StringBuilder
        Dim typ As String
        Postgis_MYDB.Tabelle = Postgis_MYDB.Tabelle.ToLower
        Try
            For i = 0 To dtplus.Rows.Count - 1
                l(i & " von (" & dtplus.Rows.Count & ")--------------------------------- >")
                If CStr(dtplus.Rows(i).Item("ismapenabled")) <> "1" Then Continue For
                KontrollAusgabeRBplus(i)
                vid = CStr(clsDBtools.fieldvalue(dtplus.Rows(i).Item("vorgangsid"))).Trim
                l("vid: " & vid)

                typ = CStr(dtplus.Rows(i).Item("TYP")).Trim
                l("typ: " & typ)
                Select Case typ
                    Case "1", "5", "7" 'punkt und adresse
                        schreibePunktInUserDb(dtplus.Rows(i))
                        anzahlErfolgreich += 1
                    Case "2", "3" 'flurstück und polygon
                        If sachgebiet = "3307" Then
                            If schreibePolygonInUserDbIllegbau(dtplus.Rows(i), dtPolygon) Then
                                anzahlErfolgreich += 1
                                If oldvid = vid Then
                                    marker = " ### "
                                Else
                                    marker = ""
                                End If
                                ' sw.Append(vid & " erfolgreich" & marker & Environment.NewLine)
                                oldvid = vid
                            Else
                                sw.Append(vid & " nicht erfolgreich" & marker & Environment.NewLine)
                                oldvid = vid
                            End If
                        Else
                            'normales sg
                            If schreibePolygonInUserDb(dtplus.Rows(i), dtPolygon) Then
                                anzahlErfolgreich += 1
                            End If

                        End If
                    Case "4"
                        schreibePolyLineInUserDb(dtplus.Rows(i))
                        anzahlErfolgreich += 1
                End Select
            Next
            l("#############################################################################################")
            l("anzahl erfolgreiche umsetzung: " & anzahlErfolgreich & " von : " & dtplus.Rows.Count)
            l("#############################################################################################")
            l("ende doRBschleife")

            returnstring = sw.ToString
            Return anzahlErfolgreich
        Catch ex As Exception
            l("fehler in doRBschleife: " & ex.ToString)
            Return anzahlErfolgreich
        End Try
    End Function

    Private Sub schreibePunktInUserDb(dataRow As DataRow)
        l(" in schreibePunktInUserDb")
        Dim sql As String
        Dim art As String = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("freitext"))), 49).Trim
        Dim name As String = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("abstract"))), 99).Trim
        Dim rbtitel As String = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("titel"))), 249).Trim
        If rbtitel = String.Empty Then
            rbtitel = "-"
        End If
        Dim rbtyp As String = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 1).Trim
        Dim FARBEFUELL As String = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 100).Trim
        Dim FARBEGRENZ As String = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 100).Trim
        Dim SYMBOL As String = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 100).Trim
        l("art " & art)
        sql =
            "INSERT INTO " & Postgis_MYDB.ServiceName & "." & Chr(34) & Postgis_MYDB.Tabelle & Chr(34) &
                        "(GEOM,ART,NAME,RBTITEL,RBTYP,FARBEFUELL,FARBEGRENZ,SYMBOL,RAUMBEZUGSID,VID) " &
                        "VALUES( ST_GeomFromText('POINT(" &
                        CStr(dataRow.Item("rechts")) & " " &
                        CStr(dataRow.Item("hoch")) & ")'," & coordinatesystemNumber &
                        "),'" & art & "','" &
                        name & "','" &
                        rbtitel & "'" & ",'" &
                        rbtyp & "','" &
                        FARBEFUELL & "','" &
                        FARBEGRENZ & "','" &
                        SYMBOL & "','" &
                        CStr(clsDBtools.fieldvalue(dataRow.Item("RAUMBEZUGSID"))) & "','" &
                       modTools.nid & "')"
        Dim erfolg As Boolean
        erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
        ' summe = summe & sql
        l("erfolg: " & erfolg)
    End Sub

    Private Function schreibePolygonInUserDb(dataRow As DataRow, dtPolygon As DataTable) As Boolean
        l(" in schreibePolygonInUserDb")
        Dim serial As String
        Dim FARBEGRENZ, SYMBOL, FARBEFUELL, art, name, rbtitel, rbtyp As String
        Dim erfolg As Boolean
        Try
            l("CStr(dataRow.Item(RAUMBEZUGSID))" & CStr(dataRow.Item("RAUMBEZUGSID")))
            serial = getserial4RID(CStr(dataRow.Item("RAUMBEZUGSID"))).Trim
            l("serial vorher: " & serial)
            serial = nondbtools.serialGKStringnachWKT(serial).Trim
            l("serial nachher: " & serial)
            werteUasZeileHolen(dataRow, FARBEGRENZ, SYMBOL, FARBEFUELL, art, name, rbtitel, rbtyp)
            erfolg = schreibePolygonInUserPG(CInt(dataRow.Item("raumbezugsid")), serial, FARBEGRENZ, SYMBOL, FARBEFUELL, art, name, rbtitel, rbtyp)
            l("erfolg: " & erfolg)

            If dtPolygon IsNot Nothing And dtPolygon.Rows.Count > 1 Then
                For i = 0 To dtPolygon.Rows.Count - 1
                    If CInt(dataRow.Item("raumbezugsid")) = CInt(dtPolygon.Rows(i).Item("raumbezugsid")) Then
                        If serial = CStr(dtPolygon.Rows(i).Item("serialshape")).Trim Then
                            'schon ausgeschrieben
                        Else

                            erfolg = schreibePolygonInUserPG(CInt(dataRow.Item("raumbezugsid")), CType(dtPolygon.Rows(i).Item("serialshape"), String),
                                                             FARBEGRENZ, SYMBOL, FARBEFUELL, art, name, rbtitel, rbtyp)
                            l("erfolg: " & erfolg)
                        End If
                    End If
                Next
            End If
            Return erfolg
        Catch ex As Exception
            l("fehler in schreibePolygonInUserDb" & ex.ToString)
            Return False
        End Try
    End Function
    Public Function ShapeSerialstringIstWKT(ShapeSerial As String) As Boolean
        Try
            If IsNumeric(ShapeSerial.Substring(0, 1)) Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function schreibePolygonInUserPG(rid As Integer, serial As String, FARBEGRENZ As String, SYMBOL As String,
                                             FARBEFUELL As String, art As String, name As String, rbtitel As String, rbtyp As String) As Boolean
        Dim erfolg As Boolean
        Dim sql As String
        Try
            If Not ShapeSerialstringIstWKT(serial) Then Return False
            sql = "INSERT INTO " & Postgis_MYDB.ServiceName & "." & Chr(34) & Postgis_MYDB.Tabelle & Chr(34) &
                         "(GEOM,ART,NAME,RBTITEL,RBTYP,FARBEFUELL,FARBEGRENZ,SYMBOL,RAUMBEZUGSID,VID) " &
                        "VALUES( ST_GeomFromText('" & serial & "'," & coordinatesystemNumber & "),'" &
                           art & "','" &
                        name.Trim & "','" &
                        rbtitel.Trim & "'" & ",'" &
                        rbtyp.Trim & "','" &
                        FARBEFUELL.Trim & "','" &
                        FARBEGRENZ.Trim & "','" &
                        SYMBOL.Trim & "','" &
                        CStr(rid).Trim & "','" &
                        nid.Trim & "')"

            erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
            Return erfolg
        Catch ex As Exception
            l("fehler in schreibePolygonInUserPG" & ex.ToString)
            Return False
        End Try
    End Function

    Private Sub werteUasZeileHolen(dataRow As DataRow, ByRef FARBEGRENZ As String, ByRef SYMBOL As String, ByRef FARBEFUELL As String, ByRef art As String, ByRef name As String, ByRef rbtitel As String, ByRef rbtyp As String)
        Try
            art = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("freitext"))), 49).Trim
            name = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("abstract"))), 99).Trim
            rbtitel = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("titel"))), 249).Trim
            If rbtitel = String.Empty Then
                rbtitel = "-"
            End If
            rbtyp = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 1).Trim
            FARBEFUELL = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 100).Trim
            FARBEGRENZ = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 100).Trim
            SYMBOL = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 100).Trim
            l("art " & art)
        Catch ex As Exception
            l("fehler in werteUasZeileHolen: " & ex.ToString)
        End Try
    End Sub

    Private Sub schreibePolyLineInUserDb(dataRow As DataRow)
        l(" in schreibePolyLineInUserDb")
        Dim serial As String
        serial = getserial4RID(CStr(dataRow.Item("RAUMBEZUGSID")))
        l("serial vorher: " & serial)
        serial = nondbtools.serialGKStringnachWKT(serial)
        l("serial nachher: " & serial)
        Dim sql As String
        Dim FARBEGRENZ, SYMBOL, FARBEFUELL, art, name, rbtitel, rbtyp As String
        werteUasZeileHolen(dataRow, FARBEGRENZ, SYMBOL, FARBEFUELL, art, name, rbtitel, rbtyp)

        sql =
            "INSERT INTO " & Postgis_MYDB.ServiceName & "." & Chr(34) & Postgis_MYDB.Tabelle & Chr(34) &
                            "(GEOM,ART,NAME,RBTITEL,RBTYP,FARBEFUELL,FARBEGRENZ,SYMBOL,RAUMBEZUGSID,VID) " &
                        "VALUES( ST_GeomFromText('" & serial & "'," & coordinatesystemNumber & "),'" &
  art & "','" &
                        name & "','" &
                        rbtitel & "'" & ",'" &
                        rbtyp & "','" &
                        FARBEFUELL & "','" &
                        FARBEGRENZ & "','" &
                        SYMBOL & "'," &
        CStr(clsDBtools.fieldvalue(dataRow.Item("RAUMBEZUGSID"))) & "," &
                        nid & ")"
        Dim erfolg As Boolean
        erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
        ' summe = summe & sql
        l("erfolg: " & erfolg)
    End Sub
    Private Function schreibePolygonInUserDbIllegbau(dataRow As DataRow, dtPolygon As DataTable) As Boolean
        l(" in schreibePolygonInUserDb--------------------------------------")
        Dim name, rbtitel, rbtyp, FARBEFUELL, FARBEGRENZ, SYMBOL, status, gebiet, serial, art, sql As String
        Try
            serial = getserial4RID(CStr(dataRow.Item("RAUMBEZUGSID")))
            l("serial vorher: " & serial)
            serial = nondbtools.serialGKStringnachWKT(serial)
            If serial.Contains("fehler") Then
                l("Fehler bei Geometrie: " & nid)
                Return False
            End If
            l("serial nachher: " & serial)
            art = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("freitext"))), 49)
            name = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("abstract"))), 99)
            rbtitel = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("titel"))), 249)
            rbtyp = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 1)
            FARBEFUELL = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 100)
            FARBEGRENZ = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 100)
            SYMBOL = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("typ"))), 100)
            status = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("statusillegbau"))), 100)
            gebiet = clsString.kuerzeTextauf(CStr(clsDBtools.fieldvalue(dataRow.Item("gebiet"))), 100)
            nid = CStr(clsDBtools.fieldvalue(dataRow.Item("vorgangsid")))
            l("art " & art)
            l("vidI HANH " & nid)
            sql =
            "INSERT INTO " & Postgis_MYDB.ServiceName & "." & Chr(34) & Postgis_MYDB.Tabelle & Chr(34) &
                         "(GEOM,ART,NAME,RBTITEL,RBTYP,FARBEFUELL,FARBEGRENZ,SYMBOL,illegstatus,gebiet,RAUMBEZUGSID,VID) " &
                        "VALUES( ST_GeomFromText('" & serial & "'," & coordinatesystemNumber & "),'" &
                           art & "','" &
                        name & "','" &
                        rbtitel & "'" & ",'" &
                        rbtyp & "','" &
                        FARBEFUELL & "','" &
                        FARBEGRENZ & "','" &
                        SYMBOL & "'," &
                         status & ",'" &
                          gebiet & "','" &
                        CStr(clsDBtools.fieldvalue(dataRow.Item("RAUMBEZUGSID"))) & "','" &
                        nid & "')"
            Dim erfolg As Boolean
            erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
            ' summe = summe & sql
            l("erfolg: " & erfolg)
            Return erfolg
        Catch ex As Exception
            l("fehler in schreibePolygonInUserDbIllegbau: " & ex.ToString)
            Return False
        End Try
    End Function

    Function getUserebeneAidFromNutzerTab(username As String, ByRef useridINtern As Integer, ByRef mac As String) As Integer
        Dim sql As String
        Dim userlayeraid As Integer
        Dim erfolg As Boolean
        l("getUserebeneAidFromNutzerTab")
        Try
            sql = "select * from public.nutzer where lower(name)='" & username.Trim.ToLower & "'"
            Postgis_MYDB.Schema = "webgiscontrol"
            l("sql " & sql)
            dtRBplus = New DataTable
            erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
            l("erfolg " & erfolg)
            Postgis_MYDB.Schema = "postgis20"
            l("dtRBplus.Rows.Count " & dtRBplus.Rows.Count)
            If dtRBplus.Rows.Count > 0 Then
                userlayeraid = CInt(clsDBtools.fieldvalue(dtRBplus.Rows(0).Item("selectionlayeraid")))
                useridINtern = CInt(clsDBtools.fieldvalue(dtRBplus.Rows(0).Item("nid")))
                mac = clsDBtools.fieldvalue(dtRBplus.Rows(0).Item("pruef"))
            Else
                userlayeraid = 0
                useridINtern = 0
                mac = ""
            End If
            l("erfolg: " & erfolg)
            Return userlayeraid
        Catch ex As Exception
            l("fehler in getUserebeneAidFromNutzerTab: " & tablename & ex.ToString)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' liefert die aid zurück
    ''' </summary>
    ''' <param name="tablename"></param>
    ''' <returns></returns>
    Friend Function userLayerInStammErzeugenAid(tablename As String, nick As String) As Integer
        Dim sql As String
        Dim userlayeraid As Integer
        Dim erfolg As Boolean
        Try
            l("userLayerInStammErzeugenAid----------------")
            dtRBplus = New DataTable
            'sid 47 ist unsichtb ar
            sql = "insert into public.stamm (ebene,titel,sid,rang,mit_objekten,mit_legende,masstab_imap) " &
            "      values ('sel_" & nick & "' , 'Auswahl: " & nick & "', 47,41,False,False,1000000) returning aid"
            '#########  umstellen des schemas
            Postgis_MYDB.Schema = "webgiscontrol"
            'erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)

            userlayeraid = ZeileEinfuegen(sql, Postgis_MYDB, dtRBplus)
            Postgis_MYDB.Schema = "postgis20"
            '#########  umstellen des schemas
            l("userLayerInStammErzeugenAid erfolg: " & erfolg)
            Return userlayeraid
        Catch ex As Exception
            l("fehler in userLayerErzeugenAid: " & tablename)
            Return 0
        End Try
    End Function

    Friend Function userLayerAttribErzeugenAid(tablename As String, aid As Integer) As Integer
        Dim sql As String
        Dim userlayeraid As Integer
        Dim erfolg As Boolean
        Try
            l("userLayerAttribErzeugenAid--------------------------")
            dtRBplus = New DataTable
            'sid 47 ist unsichtb ar
            sql = "insert into public.attributtabellen (aid,tab_nr,schema,tabelle,tab_id,ebene) " &
            "      values (" & aid & " , 1,'Paradigma_userdata','" & tablename & "', 'gid','ParadigmaUser') returning id"
            '#########  umstellen des schemas
            Postgis_MYDB.Schema = "webgiscontrol"
            'erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
            l("sql--------------------------" & sql)
            userlayeraid = ZeileEinfuegen(sql, Postgis_MYDB, dtRBplus)
            l("userLayerAttribErzeugenAid erzegut: " & userlayeraid)
            Postgis_MYDB.Schema = "postgis20"
            '#########  umstellen des schemas
            l("erfolg: " & erfolg)
            Return userlayeraid
        Catch ex As Exception
            l("fehler in userLayerErzeugenAid: " & tablename)
            Return 0
        End Try
    End Function


    Friend Function InsertInNutzertabGetNid(username As String, userEbeneAid As Integer, mac As String) As Integer
        Dim sql As String
        Dim userlayeraid As Integer
        Dim erfolg As Boolean
        Try
            l("InsertInNutzertabAid-------------------")
            dtRBplus = New DataTable
            sql = "insert into public.nutzer (name,selectionlayeraid,pruef) " &
            "      values ('" & username & "' ,  " & userEbeneAid & ",'" & mac & "') returning nid"
            '#########  umstellen des schemas
            Postgis_MYDB.Schema = "webgiscontrol"
            l("InsertInNutzertabAid-------------------" & sql)
            userlayeraid = ZeileEinfuegen(sql, Postgis_MYDB, dtRBplus)
            Postgis_MYDB.Schema = "postgis20"
            '#########  umstellen des schemas
            l("erfolg: " & erfolg)
            Return userlayeraid
        Catch ex As Exception
            l("fehler in InsertInNutzertabAid: " & tablename)
            Return 0
        End Try
    End Function

    Friend Function UpdateNutzertabAid(useridINtern As Integer, userEbeneAid As Integer, mac As String) As Boolean
        Dim sql As String
        Dim userlayeraid As Integer
        Dim erfolg As Boolean
        Try
            l("UpdateNutzertabAid------------")
            dtRBplus = New DataTable

            sql = "update public.nutzer set selectionlayeraid= " & userEbeneAid &
                ",pruef='" & mac & "'" &
                " where nid=" & useridINtern
            l("UpdateNutzertabAid------------" & sql)
            '#########  umstellen des schemas
            Postgis_MYDB.Schema = "webgiscontrol"
            erfolg = modPG.sqlausfuehren(sql, Postgis_MYDB, dtRBplus)
            Postgis_MYDB.Schema = "postgis20"
            '#########  umstellen des schemas
            l("erfolg: " & erfolg)
            Return erfolg
        Catch ex As Exception
            l("fehler in UpdateNutzertabAid: " & useridINtern)
            Return False
        End Try
    End Function
End Module
