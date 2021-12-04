Imports System.Data
Imports Npgsql
Imports webgiscontrol

Module dbeditTools
    Function setSQLbody() As String
        Return " SET DATEINAMEOHNEEXT=:DATEINAMEOHNEEXT" &
                    ",ART=:ART" &
                    ",DATEITYP=:DATEITYP" &
                    ",ORDNER=:ORDNER" &
                    ",BESCHREIBUNG=:BESCHREIBUNG" &
                    ",QUELLENTYP=:QUELLENTYP" &
                    ",herkunft=:herkunft" &
                    ",WANNVEROEFFENTLICHT=:WANNVEROEFFENTLICHT" &
                    ",SCHLAGWORTE=:SCHLAGWORTE" &
                    ",URL=:URL" &
                    ",QUELLE=:QUELLE" &
                    ",ORIGINALNAME=:ORIGINALNAME" &
                    ",ISTGUELTIG=:ISTGUELTIG"
    End Function
    Friend Function datenUebernehmen(UPDATEnameIDspalte As String, aid As Integer, tabelle As String,
                                     SETspaltenname As String,
                                     neuerwert As String, spaltentyp As String, server As String, datenbank As String) As Boolean
        Dim dt As New DataTable
        Dim ersterteil As String
        Dim Sql As String
        Try
            Dim setStatement As String
            Dim alterSidWert As String = ""
            Dim neuerSidWert As String = ""
            'setStatement = " SET TITEL=:TITEL"
            setStatement = " SET " & SETspaltenname.ToUpper & "=:" & SETspaltenname.ToUpper & ""
            makeConnection(server, datenbank, "postgres", "lkof4", "5432")
            myconn.Open()
            '  Dim Sql As String = "UPDATE  " & tabelle & setStatement & " WHERE AID=:AID " & sidtext
            Sql = "UPDATE  " & tabelle & setStatement & " WHERE " & UPDATEnameIDspalte & "=:" & UPDATEnameIDspalte & " "
            Dim com As New NpgsqlCommand(Sql, myconn)
            Dim da As New NpgsqlDataAdapter(com)
            ersterteil = ":" & SETspaltenname.ToUpper
            com.Parameters.AddWithValue(ersterteil, (neuerwert))
            com.Parameters.AddWithValue(":" & UPDATEnameIDspalte, aid)
            Dim anzahlTreffer = CInt(com.ExecuteNonQuery)
            myconn.Close()
            If anzahlTreffer < 1 Then
                l("Problem beim Abspeichern:" & Sql)
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            l("fehler in datenUebernehmen: " & ex.ToString)
            Return False
        End Try
        Return True
    End Function

    Friend Sub DatenausGruppenAID(aktaid As Integer, aktStamm As clsStamm)
        Dim dtstamm As New System.Data.DataTable
        Try
            dtstamm = getDT("select * from gruppe2aid where aid=" & aktaid, tools.dbServername, "webgiscontrol")
            If dtstamm.Rows.Count < 1 Then
                '    MsgBox("Kein Eintrag in Schlagworte vorhanden")
                dbeditTools.zeileEinfuegen("public.gruppe2aid", "aid", aktaid & " ", " RETURNING id", tools.dbServername, "webgiscontrol")
                '  aktStamm.schlagworte = ""
            Else
                aktStamm.gruppen.internet = CBool(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("internet")))
                aktStamm.gruppen.intranet = CBool(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("intranet")))
                aktStamm.gruppen.umwelt = CBool(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("umwelt")))
                aktStamm.gruppen.sicherheit = CBool(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("sicherheit")))
                aktStamm.gruppen.bauaufsicht = CBool(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("bauaufsicht")))
            End If
        Catch ex As Exception
            l("Fehler in DatenausGruppenAID. evtl. existiert kein Eintrag für AID: " & aktaid & ex.ToString)
        End Try
    End Sub

    Friend Sub DatentabelleAnezigen(aktaid As Integer, aktStamm As clsStamm, tab_nr As Integer)
        Dim aktschema As New clsSchema
        Dim sql As String
        aktschema.schemaname = ""
        Try

            If tab_nr < 1 Then
                    Exit Sub
                End If
                schemaColl.Add(aktschema)
            If aktStamm.tabellenListen(tab_nr - 1).tabelle = String.Empty Then
                l("warnung keine tabelle vorhanden ")
                Exit Sub
            End If
            sql = "select *  from " & aktStamm.tabellenListen(tab_nr - 1).Schema & "." &
            aktStamm.tabellenListen(tab_nr - 1).tabelle & " limit 1000"
            wgisdt = getDT(sql, tools.dbServername, "postgis20")
        Catch ex As Exception
            l("fehler in DatentabelleAnezigen Vermutlich gibt es keine datentabelle" & ex.ToString)
        End Try
    End Sub
    Friend Function sachgebietUpdaten(aid As Integer, tabelle As String,
                                     spaltenname As String,
                                     neuerwert As String, alterSIDwert As String) As Boolean

        'beditTools.sachgebietUpdaten(aktaid, "ebenen_sachgebiete", "sid", CType(neuStamm.sid, String), CType(aktStamm.sid, String))
        Dim dt As New DataTable
        Dim ersterteil As String
        Try
            Dim setStatement As String
            Dim Sql As String
            Dim neuerSidWert As String = ""
            setStatement = " SET TITEL=:TITEL"
            setStatement = " SET " & spaltenname.ToUpper & "=:" & spaltenname.ToUpper & ""

            makeConnection(tools.dbServername, "webgiscontrol", "postgres", "lkof4", "5432")
            myconn.Open()
            '  Dim Sql As String = "UPDATE  " & tabelle & setStatement & " WHERE AID=:AID " & sidtext
            Sql = "UPDATE  " & tabelle & setStatement & " WHERE AID=:AID and SID=" & alterSIDwert
            Dim com As New NpgsqlCommand(Sql, myconn)
            Dim da As New NpgsqlDataAdapter(com)
            ersterteil = ":" & spaltenname.ToUpper
            com.Parameters.AddWithValue(ersterteil, (neuerwert))
            com.Parameters.AddWithValue(":AID", aid)


            Dim anzahlTreffer = CInt(com.ExecuteNonQuery)
            myconn.Close()
            If anzahlTreffer < 1 Then
                l("Problem beim Abspeichern:" & Sql)
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            l("fehler in datenUebernehmen: " & ex.ToString)
            Return False
        End Try
        Return True
    End Function
    Friend Function getIsHgrund(aktaid As Integer, aktStamm As clsStamm) As Boolean
        Try
            wgisdt = getDT("SELECt * FROM  hintergrund  where aid=" & aktaid, tools.dbServername, "webgiscontrol")
            If wgisdt.Rows.Count > 0 Then

                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            l("fehler in getIsHgrund: " & ex.ToString)
            Return False
        End Try
    End Function

    Friend Function zeileEinfuegen(schemaPunktTabelle As String, spaltennamen As String,
                                   Werte As String, returningID As String, hostname As String, schema As String) As Integer
        'dbeditTools.zeileEinfuegen("public.schlagworte", "aid,schlagworte", aktaid & " '' ")
        '
        '       insert into schlagworte 
        '              (aid,schlagworte)
        '               values(10,'')
        'makeConnection(tools.dbServername, "webgiscontrol", "postgres", "lkof4", "5432")
        makeConnection(hostname, schema, "postgres", "lkof4", "5432")
        myconn.Open()
        Werte = Werte.Trim
        If Not IsNumeric(Werte) Then
            If Not Werte.Contains(",") Then
                Werte = "'" & Werte & "'"
            End If
        End If

        Dim Sql As String =
            "insert into " & schemaPunktTabelle & " (" & spaltennamen & ") " &
            " values (" & Werte & ")" & returningID
        Dim com As New NpgsqlCommand(Sql, myconn)
        Try
            Dim serialNumber As Integer
            serialNumber = CInt(com.ExecuteScalar)
            myconn.Close()
            If serialNumber < 1 Then
                l("Problem beim Abspeichern:" & Sql)
                Return serialNumber
            Else
                Return serialNumber
            End If
        Catch ex As Exception
            l("fehler in zeileEinfuegen: " & ex.ToString)
            Return 0
        End Try
    End Function

    Public Sub initCmbRang()
        rangColl.Clear()
        Dim aktSG As New clsSachgebietsCombo
        'aktSG.sid = ""
        'aktSG.sachgebiet = "---alle---"
        'sgColl.Add(aktSG)
        Try
            wgisdt = getDT("SELECt rang, merkmal FROM  rangstufen  order by rang", tools.dbServername, "webgiscontrol")
            If wgisdt.Rows.Count < 1 Then
                l("fehler initCmbRang kleiner 1")
                MsgBox("initCmbRang kleiner 1")
            End If
            For Each item As DataRow In wgisdt.AsEnumerable
                aktSG = New clsSachgebietsCombo
                aktSG.sid = item.Item("rang").ToString
                aktSG.sachgebiet = item.Item("merkmal").ToString
                rangColl.Add(aktSG)
            Next
        Catch ex As Exception
            l("fehler initCmbRang " & ex.ToString)
            '   MsgBox("initCmbRang " & ex.ToString)
        End Try
    End Sub

    Sub initHaupSachgebietAuswahlColl()
        sgHauptColl.Clear()
        Dim aktSG As New clsSachgebietsCombo
        Try
            aktSG.sid = ""
            aktSG.sachgebiet = "---alle---"
            sgHauptColl.Add(aktSG)
            wgisdt = getDT("SELECt * FROM  sachgebiete  where ist_standard=true order by sachgebiet", tools.dbServername, "webgiscontrol")
            For Each item As DataRow In wgisdt.AsEnumerable
                aktSG = New clsSachgebietsCombo
                aktSG.sid = item.Item("sid").ToString
                aktSG.sachgebiet = item.Item("sachgebiet").ToString
                sgHauptColl.Add(aktSG)
            Next
        Catch ex As Exception
            l("fehler in initHaupSachgebietAuswahlColl " & ex.ToString)
            MsgBox("initCmbRang " & ex.ToString)
        End Try
    End Sub

    Friend Sub getAnzahlAttributtabellen(aktaid As Integer, astamm As clsStamm)
        Try
            wgisdt = getDT("select count(*) from attributtabellen where aid=" & aktaid, tools.dbServername, "webgiscontrol")
            astamm.AnzahlAttributtabellenReal = CInt(clsDBtools.fieldvalue(wgisdt.Rows(0).Item(0)).ToString())
        Catch ex As Exception
            l("fehler in initHaupSachgebietAuswahlColl " & ex.ToString)
            MsgBox("initCmbRang " & ex.ToString)
        End Try
    End Sub

    Sub DatenAusAttributtabelle(aktaid As Integer, aktstamm As clsStamm)
        Dim attributtabelleDef As New clsTabellenDef
        Dim dtstamm As New System.Data.DataTable
        Try
            l("DatenAusAttributtabelle-----------------------")
            For i = 1 To aktstamm.AnzahlAttributtabellenReal
                attributtabelleDef = New clsTabellenDef
                dtstamm = getDT("select * from attributtabellen where aid=" & aktaid & " and tab_nr=" & i, tools.dbServername, "webgiscontrol")
                Dim a = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("schema"))
                attributtabelleDef.Schema = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("schema"))
                attributtabelleDef.tabelle = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("tabelle"))
                attributtabelleDef.tabellen_anzeige = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("tabellen_anzeige"))
                attributtabelleDef.tabtitel = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("tab_titel"))
                attributtabelleDef.tab_id = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("tab_id"))
                attributtabelleDef.tab_nr = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("tab_nr"))
                attributtabelleDef.id = CInt(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("id")))
                attributtabelleDef.linkTabs = (clsDBtools.fieldvalue(dtstamm.Rows(0).Item("linkTabs")))
                aktstamm.tabellenListen.Add(attributtabelleDef)
            Next
            '   dtstamm = getDT("select * from attributtabellen where aid=" & aktaid)

            'tbaid.Text = dtstamm.Rows(0).Item("AID").ToString
            'tbmassstab_imap.Text = dtstamm.Rows(0).Item("masstab_imap").ToString
            ''tbmitlegende.Text = dtstamm.Rows(0).Item("mit_legende").ToString

            'tbrang.Text = dtstamm.Rows(0).Item("rang").ToString
            'tbtitel.Text = dtstamm.Rows(0).Item("titel").ToString
        Catch ex As Exception
            l("fehler in DatenAusAttributtabelle " & ex.ToString)
        End Try
    End Sub

    Sub DatenAusStammDarstellen(aktStamm As clsStamm, aktaid As Integer)
        Dim dtstamm As New System.Data.DataTable
        Try
            dtstamm = getDT("select * from " & stamm_tabelle & " where aid=" & aktaid, tools.dbServername, "webgiscontrol")
            'tbaid.Text = dtstamm.Rows(0).Item("AID").ToString
            aktStamm.masstab_imap = CInt(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("masstab_imap")).ToString)
            aktStamm.mit_legende = CBool(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("mit_legende")))
            aktStamm.status = CBool(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("status")))
            aktStamm.mit_objekten = CBool(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("mit_objekten")))
            aktStamm.mit_imap = CBool(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("mit_imap")))



            aktStamm.rang = CInt(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("rang")).ToString)
            aktStamm.titel = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("titel")).ToString
            aktStamm.ebene = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("ebene")).ToString
            If stamm_tabelle <> "stamm" Then

                aktStamm.sachgebiet = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("sachgebiet")).ToString
                aktStamm.sid = CInt(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("sid")))
                aktStamm.pfad = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("pfad")).ToString
            End If

            'aktStamm.anzahl_attributtabellen = CInt(clsDBtools.fieldvalue(dtstamm.Rows(0).Item("anzahl_attributtabellen")).ToString)
        Catch ex As Exception
            l("fehler in DatenAusStammDarstellen " & ex.ToString)
        End Try
    End Sub
    Sub DatenAusSchlagwortDarstellen(aktaid As Integer, aktstamm As clsStamm)
        Dim dtstamm As New System.Data.DataTable
        Try
            dtstamm = getDT("select * from ref_schlagworte where aid=" & aktaid, tools.dbServername, "webgiscontrol")
            If dtstamm.Rows.Count < 1 Then
                '    MsgBox("Kein Eintrag in Schlagworte vorhanden")
                dbeditTools.zeileEinfuegen("public.schlagworte", "aid,schlagworte", aktaid & ",'' ", " RETURNING wid", tools.dbServername, "webgiscontrol")
                aktstamm.schlagworte = ""
            Else
                aktstamm.schlagworte = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("schlagworte")).ToString
            End If

        Catch ex As Exception
            l("Fehler in schlagworteanzeigen. evtl. existiert kein Eintrag für AID: " & aktaid & ex.ToString)
        End Try
    End Sub

    Friend Sub HintergrundAktionAid(aktaid As Integer, modus As String)
        l("HintergrundAktionAid " & modus)
        Dim neueid As Integer
        Try
            If modus = "add" Then
                neueid = dbeditTools.zeileEinfuegen("public.hintergrund", "aid", aktaid & " ", " RETURNING id", tools.dbServername, "webgiscontrol")
                l("neueid " & neueid)
            End If
            If modus = "delete" Then
                neueid = dbeditTools.zeileLoeschen("public.hintergrund", "aid=" & aktaid)

            End If
        Catch ex As Exception
            l("Fehler in HintergrundAktionAid.   " & aktaid & ex.ToString)
        End Try
    End Sub

    Function zeileLoeschen(SchemaPUnktTabelle As String, Kriterium As String) As Integer
        'neueid = dbeditTools.zeileLoeschen("public.hintergrund", "aid=aktaid")
        l("zeileLoeschen")
        makeConnection(tools.dbServername, "webgiscontrol", "postgres", "lkof4", "5432")
        myconn.Open()
        Dim Sql As String =
            "delete from " & SchemaPUnktTabelle & " where " & Kriterium & " "
        Dim com As New NpgsqlCommand(Sql, myconn)
        Dim da As New NpgsqlDataAdapter(com)
        Try
            Dim serialNumber As Integer
            serialNumber = CInt(com.ExecuteScalar)
            myconn.Close()
            If serialNumber = 0 Then
                l("zeileLoeschen keine probleme" & Sql)
                Return serialNumber
            Else
                l("zeileLoeschen Problem beim Abspeichern:" & Sql)
                Return serialNumber
            End If
        Catch ex As Exception
            l("fehler in zeileLoeschen: " & ex.ToString)
            Return 0
        End Try
    End Function

    Sub initSchemaAuswahlColl()
        schemaColl.Clear()
        Dim aktschema As New clsSchema
        aktschema.schemaname = ""
        Try
            schemaColl.Add(aktschema)
            Dim sql = "select nspname  from pg_catalog.pg_namespace " &
                      " where nspname not like 'pg_%' order by nspname"
            wgisdt = getDT(sql, tools.dbServername, "postgis20")
            For Each item As DataRow In wgisdt.AsEnumerable
                aktschema = New clsSchema
                aktschema.schemaname = CType(item.ItemArray(0), String)
                ' aktschema.schemaname = a.schemaname
                schemaColl.Add(aktschema)
            Next
        Catch ex As Exception
            l("fehler in initSchemaAuswahlColl " & ex.ToString)
        End Try
    End Sub

    Friend Function getMaxTabnr(aktaid As Integer) As Integer
        Try
            Dim sql As String = "select max(tab_nr) from attributtabellen where aid=" & aktaid
            wgisdt = getDT(sql, tools.dbServername, "webgiscontrol")
            Dim max As Integer
            Dim test As String
            test = (clsDBtools.fieldvalue(wgisdt.Rows(0).Item(0)).ToString)
            If test = String.Empty Then
                Return 0
            Else
                max = CInt(clsDBtools.fieldvalue(wgisdt.Rows(0).Item(0)).ToString)
            End If

        Catch ex As Exception
            l("fehler in initSchemaAuswahlColl " & ex.ToString)
        End Try
    End Function

    Sub initTabellenAuswahlColl(schema As String)
        schematabellenColl.Clear()
        Dim aktschema As New clsSchemaTabelle
        aktschema.tabellenname = ""
        Try
            schematabellenColl.Add(aktschema)
            Dim sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = '" &
                      schema & "' order by table_name "

            wgisdt = getDT(sql, tools.dbServername, "postgis20")
            For Each item As DataRow In wgisdt.AsEnumerable
                aktschema = New clsSchemaTabelle
                aktschema.schemaname = schema
                aktschema.tabellenname = CType(item.ItemArray(0), String)
                ' aktschema.schemaname = a.schemaname
                schematabellenColl.Add(aktschema)
            Next
        Catch ex As Exception
            l("fehler in initTabellenAuswahlColl " & ex.ToString)
        End Try
    End Sub
    Sub initTABIDAuswahlColl(tabellenname As String, schemaname As String, loktabIDColl As List(Of clsSachgebietsCombo))
        'tabIDColl
        loktabIDColl.Clear()
        Dim akttabid As New clsSachgebietsCombo
        akttabid.sachgebiet = ""
        Try
            loktabIDColl.Add(akttabid)
            Dim sql = " select column_name from information_schema.columns where table_schema='" & schemaname &
                "' and table_name='" & tabellenname & "' "
            wgisdt = getDT(sql, tools.dbServername, "postgis20")
            For Each item As DataRow In wgisdt.AsEnumerable
                akttabid = New clsSachgebietsCombo
                akttabid.sachgebiet = CType(item.ItemArray(0), String)
                ' aktschema.schemaname = a.schemaname
                loktabIDColl.Add(akttabid)
            Next
        Catch ex As Exception
            l("fehler in initSchemaAuswahlColl " & ex.ToString)
        End Try
    End Sub

    Friend Sub DatenAusDokuDarstellen(aktaid As Integer, aktStamm As clsStamm)
        aktStamm = getDoku4Stamm(aktaid, aktStamm)

    End Sub

    Private Function getDoku4Stamm(aktaid As Integer, aktStamm As clsStamm) As clsStamm
        Dim dtstamm As New System.Data.DataTable
        Try
            aktStamm.aktDoku.clear()
            dtstamm = getDT("select * from doku where aid=" & aktaid, tools.dbServername, "webgiscontrol")
            If dtstamm.Rows.Count < 1 Then
                '    MsgBox("Kein Eintrag in Schlagworte vorhanden")
                dbeditTools.zeileEinfuegen("public.doku", "aid,inhalt", aktaid & ",'' ", " RETURNING id", tools.dbServername, "webgiscontrol")
            Else
                aktStamm.aktDoku.aktualitaet = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("aktualitaet")).ToString
                aktStamm.aktDoku.beschraenkungen = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("beschraenkungen")).ToString
                'aktStamm.aktDoku.calcedOwner = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("aktualitaet")).ToString
                aktStamm.aktDoku.datenabgabe = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("datenabgabe")).ToString
                aktStamm.aktDoku.entstehung = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("entstehung")).ToString
                aktStamm.aktDoku.inhalt = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("inhalt")).ToString
                aktStamm.aktDoku.masstab = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("masstab")).ToString
                aktStamm.aktDoku.internes = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("internes")).ToString
            End If
            Return aktStamm
        Catch ex As Exception
            l("Fehler in DatenAusDokuDarstellen. evtl. existiert kein Eintrag für AID: " & aktaid & ex.ToString)
            Return Nothing
        End Try


    End Function
    Friend Sub vorlagenDT(aktaid As Integer, aktStamm As clsStamm, tab_nr As Integer)
        Dim sql As String
        Try
            sql = "select *  from public.tabellenvorlagen where aid=" & aktaid &
                " and tab_nr=" & tab_nr &
                "  order by tab_nr,nr"
            'sql = "select *  from public.tabellenvorlagen where aid=" & aktaid &
            '    " and tab_nr=" & tab_nr &
            '    "  order by tab_nr,nr"

            wgisdt = getDT(sql, tools.dbServername, "webgiscontrol")
        Catch ex As Exception
            l("fehler in vorlagenDT " & ex.ToString)
        End Try
    End Sub
    Friend Function vorlagenDT2list(aktaid As Integer, aktStamm As clsStamm) As List(Of MaskenObjekt)
        Dim nmask As New MaskenObjekt
        Dim maskenListe As New List(Of MaskenObjekt)
        Try
            For i = 0 To wgisdt.Rows.Count - 1
                nmask = New MaskenObjekt
                nmask.id = CInt(clsDBtools.fieldvalue(wgisdt.Rows(i).Item("id")))
                nmask.tab_nr = CInt(clsDBtools.fieldvalue(wgisdt.Rows(i).Item("tab_nr")))
                nmask.nr = CInt(clsDBtools.fieldvalue(wgisdt.Rows(i).Item("nr")))
                nmask.feldname = (clsDBtools.fieldvalue(wgisdt.Rows(i).Item("feldname")))
                nmask.titel = (clsDBtools.fieldvalue(wgisdt.Rows(i).Item("titel")))
                nmask.typ = (clsDBtools.fieldvalue(wgisdt.Rows(i).Item("typ")))
                nmask.cssclass = (clsDBtools.fieldvalue(wgisdt.Rows(i).Item("cssclass")))
                nmask.template = (clsDBtools.fieldvalue(wgisdt.Rows(i).Item("template")))
                maskenListe.Add(nmask)
            Next
            Return maskenListe
        Catch ex As Exception
            l("fehler in vorlagenDT2list " & ex.ToString)
            Return Nothing
        End Try
    End Function

    Friend Sub TabVorlageEinfuegen(aktaid As Integer, tabNr As String)
        Dim anz As Integer
        anz = dbeditTools.zeileEinfuegen("public.tabellenvorlagen", "aid,tab_Nr", aktaid & "," & tabNr & " ", " RETURNING id", tools.dbServername, "webgiscontrol")
    End Sub

    Friend Sub TabVorlageloeschen(aktid As Integer, id As Integer)
        Dim neueid = dbeditTools.zeileLoeschen("public.tabellenvorlagen", "id=" & id)
    End Sub
End Module
