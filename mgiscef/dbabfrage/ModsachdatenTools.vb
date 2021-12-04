Imports System.Data
Imports mgis

Module ModsachdatenTools

    Sub korrigiereTabellenSchemaFallsEintraegeFalschDB(os_tabelledef As clsTabellenDef)
        Exit Sub
        Try
            l("korrigiereTabellenSchemaFallsEintraegeFalsch---------------------- anfang")
            basisrec.mydb.SQL = "select table_schema from paradigma_userdata.""vtschema"" where table_name='" & os_tabelledef.tabelle & "'"
            basisrec.getDataDT()
            l(basisrec.mydb.SQL)
            If basisrec.dt.Rows.Count > 0 Then
                os_tabelledef.Schema = clsDBtools.fieldvalue(basisrec.dt.Rows(0).Item(0))
            Else
                'keine Korrektur möglich
            End If
            l("korrigiereTabellenSchemaFallsEintraegeFalsch---------------------- ende")
        Catch ex As Exception
            l("Fehler in korrigiereTabellenSchemaFallsEintraegeFalsch: ", ex)
        End Try
    End Sub
    Function getlokalitaetstring(aktFST As ParaFlurstueck) As String
        Dim summe As String = ""
        Dim trenner As String = ", "
        aktFST.normflst.fstueckKombi = aktFST.normflst.buildFstueckkombi()
        summe = summe & aktFST.normflst.gemarkungstext & trenner
        summe = summe & "Flur: " & aktFST.normflst.flur & trenner
        summe = summe & "Flurstueck: " & aktFST.normflst.fstueckKombi & trenner
        Return summe
    End Function
    Friend Function getAnzahlAttributtabellen(aktaid As Integer) As Integer
        Dim sql = "select count(*) from public.attributtabellen where aid=" & aktaid
        Try
            If iminternet Or CGIstattDBzugriff Then
                Dim result As String = "", hinweis As String = ""
                result = clsToolsAllg.getSQL4Http(sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
                result = result.Replace("$", "").Replace(vbCrLf, "")
                If result.IsNothingOrEmpty Then
                    l("fehler schema leer, keine att tabs sql: " & sql)
                    Return 0
                Else
                    Return CInt(result)
                End If
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(sql, "webgiscontrol")
                Return CInt(clsDBtools.fieldvalue(dt.Rows(0).Item(0)).ToString())
            End If
        Catch ex As Exception
            l("fehler in initHaupSachgebietAuswahlColl ", ex)
            MsgBox("initCmbRang " & ex.ToString)
            Return -1
        End Try
    End Function
    Friend Function getSChemaDB(aid As Integer, tab_nr As Integer) As clsTabellenDef
        l("getSChemaDB---------------------")
        Dim schemadef As New clsTabellenDef

        Dim SQL As String = ""
        Try
            If aid < 1 Then
                l("warnung getSChemaDB- keine AID angegeben")
                Return Nothing
            End If
            schemadef.aid = CType(aid, String)
            SQL = "select * from public.attributtabellen where aid=" & aid & " and tab_nr=" & tab_nr
            If iminternet Or CGIstattDBzugriff Then
                Dim result As String = "", hinweis As String = ""
                result = clsToolsAllg.getSQL4Http(SQL, "webgiscontrol", hinweis, "getsql") : l(hinweis)
                result = result.Replace("$", "").Replace(vbCrLf, "")
                If result.IsNothingOrEmpty Then
                    l("fehler schema leer, sql: " & SQL & "," & aid & "," & tab_nr)
                    Return Nothing
                Else
                    Dim a() As String '174#173#bplanrechtsw#1#planung#bebauungsplan_f#gid##normal##1
                    a = result.Split("#"c)
                    schemadef.Schema = clsDBtools.fieldvalue(a(4))
                    schemadef.tabelle = clsDBtools.fieldvalue(a(5))
                    schemadef.tab_id = clsDBtools.fieldvalue(a(6)) 'gid_spalte
                    schemadef.tab_nr = clsDBtools.fieldvalue(a(3))
                    schemadef.linkTabs = clsDBtools.fieldvalue(a(9))
                    schemadef.tabtitel = clsDBtools.fieldvalue(a(7))
                    schemadef.tabellen_anzeige = clsDBtools.fieldvalue(a(8))
                    schemadef.tabellenvorlage = CInt(clsDBtools.fieldvalue(a(11)))
                    Return schemadef
                End If
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(SQL, "webgiscontrol")
                If clsWebgisPGtools.hatRecords(dt) Then
                    schemadef.Schema = clsDBtools.fieldvalue(dt.Rows(0).Item("schema"))
                    schemadef.tabelle = clsDBtools.fieldvalue(dt.Rows(0).Item("tabelle"))
                    schemadef.tab_id = clsDBtools.fieldvalue(dt.Rows(0).Item("tab_id")) 'id_spalte
                    schemadef.tab_nr = clsDBtools.fieldvalue(dt.Rows(0).Item("tab_nr"))
                    schemadef.linkTabs = clsDBtools.fieldvalue(dt.Rows(0).Item("linktabs"))
                    schemadef.tabtitel = clsDBtools.fieldvalue(dt.Rows(0).Item("tab_titel"))
                    schemadef.tabellen_anzeige = clsDBtools.fieldvalue(dt.Rows(0).Item("tabellen_anzeige"))
                    schemadef.tabellenvorlage = CInt(clsDBtools.fieldvalue(dt.Rows(0).Item("tabellenvorlage")))
                    Return schemadef
                Else
                    l("fehler schema leer, sql: " & SQL & "," & aid & "," & tab_nr)
                    Return Nothing
                End If
            End If
        Catch ex As Exception
            l("fehler in getSChemaDB: " & SQL & Environment.NewLine, ex)
            Return Nothing
        End Try
    End Function

    Friend Function bplanbegleitInfoCalcDirectory(gemarkung As String, pdf As String, fkat As String) As String
        Try
            l("bplanbegleitInfoCalcDirectory---------------------- anfang")
            Dim root As String
            'If iminternet Or CGIstattDBzugriff Then
            root = myglobalz.serverWeb & "/fkat" & "/bplan" & gemarkung & "/" & pdf & ""
            'Else
            '    root = fkat & "\bplan" & gemarkung & "\" & pdf & "\"
            'End If

            Return root
            l("-bplanbegleitInfoCalcDirectory--------------------- ende")
        Catch ex As Exception
            l("Fehler in bplanbegleitInfoCalcDirectory: " & ex.ToString())
            Return ""
        End Try
    End Function


    Friend Function getsachdaten(Fdaten1 As clsTabellenDef, ByRef aktfs As String) As List(Of clsSachdaten) 'aktschema As String, akttabelle As String, aktidspalte As String,
        '                           objektid As String,
        '                         tabnr As String) As List(Of clsSachdaten)
        Dim temp = ""
        Dim sachdatListm As New List(Of clsSachdaten)
        Try
            Dim newsd As New clsSachdaten
            Dim wert As String = "", LastColNames() As String
            Dim result As String = ""
            Dim hinweis As String = ""

            If IsNumeric(Fdaten1.gid) Then
                wert = Fdaten1.gid
            Else
                wert = "'" & Fdaten1.gid & "'"
            End If
            If wert.IsNothingOrEmpty Then
                l("fehler getsachdaten wert is nothing ")
                Return sachdatListm
            End If

            Dim SQL = "select * from " & Fdaten1.Schema & "." & Fdaten1.tabelle &
                          " where " & Fdaten1.tab_id & "=" & wert
            l(SQL)

            'If iminternet Or CGIstattDBzugriff Then
            If iminternet Or CGIstattDBzugriff Then
                ModsachdatenTools.getColnames(Fdaten1.Schema & "." & Fdaten1.tabelle, LastColNames:=LastColNames, hinweis:=hinweis)
                result = clsToolsAllg.getSQL4Http(SQL, "postgis20", hinweis, "getsql") : l(hinweis)
                result = result.Trim
                '559#Bebauungsplan#Neu-Isenburg#Neu-Isenburg#042#42#Quartier II#42 - Quartier II###22.03.2001#####0##1#0#ni_42#31104,4176765311#9#0103000020E86400000100000064000000D078E9A6C12D1D4194C2F548CE26554180E926B1C12D1D419A9999F9CD265541E0240681092E1D412EB29DF7CD265541A0999919782E1D4160BA49F4CD2655413080B7C09F2E1D41E00209EECD26554120AE47E19F2E1D414A0C0233CD265541C047E17AA02E1D4180954323CA265541C047E17AA02E1D41F2D24D52C626554100295C8FA02E1D416ABC74CBC4265541500AD7A3A12E1D41B81E851BC426554100FA7EEAA12E1D41C876BE97C2265541408941E0A22E1D4160E5D0B2C026554120DBF9FEA22E1D410E2DB2E5BC265541B0749398A32E1D41AC1C5ACCB926554170BC7413A42E1D41345EBA31B726554110A8C6CBA42E1D410A022B67B3265541C09DEF27A52E1D416866667EB1265541A0416065022F1D4108560E7DB1265541B0C42030172F1D41FC7E6A7CB126554120D9CE77172F1D41AAC64BBFAE26554160BA498C172F1D41FA285CAFAD26554180ED7CBF172F1D41F4FDD4C0AB265541F0CEF7D3172F1D415E8FC2FDAA26554120022B07182F1D41F2FDD4E8A82655419068916D182F1D414E378901A526554120D9CE77182F1D413A8941E0A4265541002B8796752F1D411E5A6413A5265541002B8796752F1D41022B876EA2265541D078E9A6382F1D41E4A59B6CA22655413008AC1C042F1D41C620B06AA2265541007D3F35DA2E1D41B0726869A2265541405EBAC9A72E1D41C876BE67A2265541102B8716762E1D417C3F3566A22655418014AEC7572E1D413EDF4F65A226554100A8C6CB2B2E1D41C4CAA165A2265541107F6ABCFF2D1D41AC1C5A64A2265541D0A145B6D22D1D4156E3A563A226554180E926B1A52D1D4140355E62A2265541205839B4842D1D41B6C8765EA226554160B81E855C2D1D41A41A2F5DA226554160B81E855C2D1D41E6FBA9299F265541F0285C8F5C2D1D415C39B4089C265541A0999919292D1D4114AE47099C2655413085EBD1282D1D4132DD24B69B26554110AAF1D2192D1D4132DD24B69B265541A0438B6C192D1D41803F35E69A265541E0D022DBC22C1D41000000E89A265541B0726811A72C1D41000000E89A26554160643BDF962C1D410AD7A3E89A265541B01C5AE46C2C1D41C84B37E99A265541F0C9C3826D2C1D41E2BE0E149C265541002731886D2C1D417C3F351E9C2655417016D94E422C1D4132DD241E9C265541E04D62901A2C1D418816D91E9C26554180128340FC2B1D418816D91E9C265541D01C5AE4FB2B1D41A21A2F55A2265541B031F294F12B1D419A2EEB54A2265541F0A7C64BF12B1D41C420B0DAA4265541D04B3789F12B1D41F6FDD420A7265541B0999919F12B1D415A643BCFAB265541308716D9162C1D41C4CAA1D5AB2655414004568E302C1D41548D97CEAB265541D076BE9F4F2C1D4148B6F3CDAB26554120DBF97E632C1D4148B6F3CDAB265541601058B98B2C1D418C4160CDAB26554190EB51B8912C1D418C4160CDAB265541E09F1A2FC02C1D41CECCCCCCAB265541402FDDA4EE2C1D41125839CCAB26554160E3A51B1D2D1D41068195CBAB265541600C02AB1C2D1D41580E2DCAAD265541E0CAA1451C2D1D412EDD2496AF265541B0F3FDD41B2D1D41F0A7C67BB12655415053D8B11B2D1D411ECA162FB726554170BC7493E02C1D411AD9CE2FB726554170BC7493E02C1D419CC42038B726554190976E12B32C1D4136B4C836B7265541F07A14AEA42C1D4136B4C836B7265541D078E9A67F2C1D41E27A1436B7265541203108AC4F2C1D41D0CCCC34B7265541500C022B252C1D4174931834B7265541801283C0F02B1D41B247E132B726554180C0CAA1F02B1D414C0C020BBA26554150DF4F8DF02B1D41ACF1D2D5BA26554150DF4F8DF02B1D41180456D6BC2655411054E3A5F02B1D416891EDCCBE2655418091ED7CF02B1D414AE17A9CC2265541B0EFA746F02B1D414E621070C5265541600E2D32F02B1D416CBC744BC8265541202DB21DF02B1D41A0C420E8CA265541202DB21DF02B1D419CC42028CB26554190BC7413F02B1D41DA2406B1CD2655411078FA9CF02B1D4154F710B1CD265541E0A370BD182C1D41105839B4CD265541B01A2FDD772C1D4118AE4709CE265541102B87169C2C1D41EE51B806CE26554170E5D022F42C1D414260E500CE265541603BDF4F272D1D4162E5D012CE265541D0A145365A2D1D41E4A59B24CE26554190C0CAA18B2D1D41AAF1D235CE265541D078E9A6C12D1D4194C2F548CE265541$
                sachdatListm = getSachdatlistFromAjax(LastColNames, result, aktfs)
                Return sachdatListm
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(SQL, "postgis20")
                Dim ergebnis As String = ""
                For i = 0 To dt.Rows.Count - 1
                    For j = 0 To dt.Columns.Count - 1
                        newsd = New clsSachdaten
                        newsd.feldname = clsDBtools.fieldvalue(dt.Columns(j).ColumnName).Trim
                        newsd.feldinhalt = clsDBtools.fieldvalue(dt.Rows(i).Item(j)).Trim()
                        temp = temp & "#" &
                              clsDBtools.fieldvalue(dt.Columns(j).ColumnName).Trim & "," &
                              clsDBtools.fieldvalue(dt.Rows(i).Item(j)).Trim()
                        sachdatListm.Add(newsd)
                        If newsd.feldname = "fs" Then
                            aktfs = newsd.feldinhalt
                        End If
                    Next
                Next
                Return sachdatListm
            End If
        Catch ex As Exception
            l("fehler in getSChema: ", ex)
            Return sachdatListm
        End Try
    End Function

    Sub getColnames(schematab As String, ByRef LastColNames() As String, ByRef hinweis As String)
        Dim colresult As String
        Try
            If myglobalz.ColumnnamesColl.ContainsKey(schematab) Then
                LastColNames = ColumnnamesColl.Item(schematab)
            Else
                Dim b = schematab.Split("."c)
                Dim colSQL As String
                colSQL =
                "select column_name from information_schema.columns " &
                "where table_schema='" & b(0) &
                "' and table_name='" & b(1) & "'"
                colresult = clsToolsAllg.getSQL4Http(colSQL, "postgis20", hinweis, "getsql") : l(hinweis)
                colresult = colresult.Trim
                colresult = clsString.removeLastChar(colresult)
                '   result = result.Replace("$", "").Replace(vbCrLf, "")
                LastColNames = colresult.Split("$"c)
                ColumnnamesColl.Add(schematab, LastColNames)
            End If
        Catch ex As Exception
            l("fehler in getColnames: ", ex)
        End Try
    End Sub

    Friend Function userIstLayerEditor(username As String, aid As Integer) As Boolean
        Try
            l("userIstLayerEditor---------------------- anfang")
            If username.ToLower = "stich_k" And aid = 161 Then Return True
            If username.ToLower = "feinen_j" And aid = 161 Then Return True
            Return False
            l("userIstLayerEditor---------------------- ende")
        Catch ex As Exception
            l("Fehler in userIstLayerEditor: " & ex.ToString())
            Return False
        End Try
    End Function

    Friend Function getmaskenObjektList(Fdaten1 As clsTabellenDef) As List(Of MaskenObjekt) 'aktschema As String, akttabelle As String, aktidspalte As String,
        'objektid As String, tabnr As String, aid As Integer) As List(Of MaskenObjekt)
        l("getMaskenObjekte ------------------------" & Fdaten1.aid)
        Dim mo As New MaskenObjekt
        Dim moList As New List(Of MaskenObjekt)
        Dim titelvorspann As String = ""
        Dim Sql As String = ""
        Try
            If Fdaten1.tabtitel.IsNothingOrEmpty Then
                titelvorspann = ""
            Else
                titelvorspann = "" ' Fdaten1.tabtitel & "."
            End If
            ' " and anwendung>1 " &
            If Fdaten1.tabellenvorlage = 0 Then
                Sql = "select * from public.tabellenvorlagen where aid=" & Fdaten1.aid &
                " and tab_nr=" & Fdaten1.tab_nr & " and anwendung<3 " &
                " order by nr"
            Else
                Sql = "select * from public.tabellenvorlagen where aid=" & Fdaten1.tabellenvorlage &
                " and tab_nr=" & Fdaten1.tab_nr & " and anwendung<3 " &
                " order by nr"
            End If

            If iminternet Or CGIstattDBzugriff Then
                Dim result As String = "", hinweis As String = ""
                result = clsToolsAllg.getSQL4Http(Sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
                ' result = result.Replace("$", "").Replace(vbCrLf, "")
                If result.IsNothingOrEmpty Then
                    l("fehler schema leer, sql: " & Sql)
                    Return Nothing
                Else
                    moList = getTabellenvorlagen2OBJAjax(result, titelvorspann)
                    Return moList
                End If
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(Sql, "webgiscontrol")
                Dim ergebnis As String = ""
                If dt.Rows.Count > 0 Then
                    For i = 0 To dt.Rows.Count - 1
                        l(i.ToString)
                        mo = New MaskenObjekt
                        mo = maskendtnachObj(titelvorspann, dt, i)
                        ' result = schema & " / " & tabelle & " / " & tab_id
                        moList.Add(mo)
                    Next
                Else
                    l("warnung keine public.tabellenvorlage gefunden, vermutlich stimmt die aid nicht")
                End If
                l(moList.Count & " Objekte wurde gefunden")
                l(" ")
                Return moList
            End If
        Catch ex As Exception
            l("Fehler keine tabellenvorlage gefunden, vermutlich stimmt die aid nicht", ex)
            Return Nothing
        End Try
    End Function

    Private Function getTabellenvorlagen2OBJAjax(result As String, titelvorspann As String) As List(Of MaskenObjekt)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim oldname As String = ""
        Dim sdlist As New List(Of MaskenObjekt)
        Dim mo As MaskenObjekt
        Try
            l(" getTabellenvorlagen2OBJ html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in getTabellenvorlagen2OBJ: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            For izeile = 0 To zeilen - 1
                b = a(izeile).Split("#"c)
                'For jspalte = 0 To b.Count - 1
                ' Dim a() As String '174#173#bplanrechtsw#1#planung#bebauungsplan_f#gid##normal##1
                'a = result.Split("#"c)
                mo = New MaskenObjekt
                mo.nr = CInt(clsDBtools.fieldvalue(b(3)))
                mo.feldname = clsDBtools.fieldvalue(b(4)).Trim
                mo.titel = titelvorspann & clsDBtools.fieldvalue(b(5)).Trim
                mo.typ = clsDBtools.fieldvalue(b(6)).Trim
                mo.cssclass = clsDBtools.fieldvalue(b(7)).Trim
                mo.template = clsDBtools.fieldvalue(b(8)).Trim
                Debug.Print("")
                sdlist.Add(mo)
                'Next
            Next
            Return sdlist
            l(" getTabellenvorlagen2OBJ ---------------------- ende")
        Catch ex As Exception
            l("Fehler in getTabellenvorlagen2OBJ: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Function maskendtnachObj(titelvorspann As String, dt As DataTable, i As Integer) As MaskenObjekt
        Dim mo As New MaskenObjekt
        mo.nr = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("nr")))
        mo.feldname = clsDBtools.fieldvalue(dt.Rows(i).Item("feldname")).Trim
        mo.titel = titelvorspann & clsDBtools.fieldvalue(dt.Rows(i).Item("titel")).Trim
        mo.typ = clsDBtools.fieldvalue(dt.Rows(i).Item("typ")).Trim
        mo.cssclass = clsDBtools.fieldvalue(dt.Rows(i).Item("cssclass")).Trim
        mo.template = clsDBtools.fieldvalue(dt.Rows(i).Item("template")).Trim
        Return mo
    End Function

    Friend Function combineDatenAndDef(sachdatList As List(Of clsSachdaten),
                                       maskenObjektList As List(Of MaskenObjekt), trenner As String,
                                       Fdaten1 As clsTabellenDef,
                                        ByRef buttonINfostringspecfunc As String,
                                       ByRef ergaenz As List(Of clsSachdaten)) As String
        'akttabelle As String, aktschema As String, objektid As String) As String
        l("combineDatenAndDef--------------------")
        Dim kombi As String = ""
        'Dim ergaenz As New List(Of clsSachdaten)
        'Dim sb As New Text.StringBuilder
        l("combineDatenAndDef--------------------")
        Try
            For Each maske As MaskenObjekt In maskenObjektList
                l("zeile.feldname: " & maske.feldname)
                l("zeile.typ: " & maske.typ)
                Select Case maske.typ
                    Case "n"
                        attachTitel(maske.feldname, sachdatList, maske)
                    Case "k"
                        attachKonstante(maske.feldname, sachdatList, maske)
                    Case "l"
                        attachLink(maske.feldname, sachdatList, maske)
                    Case "t", "v"
                        attachTemplate(maske.feldname, sachdatList, maske, buttonINfostringspecfunc)
                    Case "s"
                        attachS(maske.feldname, sachdatList, maske, ergaenz)
                       ' attachTitel(maske.feldname, sachdatList, maske)
                    Case "u"
                        attachGattungArten(maske.feldname, sachdatList, maske)
                    Case "g"
                    Case Else
                        attachTitel(maske.feldname, sachdatList, maske)
                End Select
            Next
            'l(sb.ToString)
            Return ""
        Catch ex As Exception
            l("fehler in combineDatenAndDef: ", ex)
            Return "".ToString
        End Try
    End Function

    Private Sub attachKonstante(feldname As String, sachdatList As List(Of clsSachdaten), maske As MaskenObjekt)
        Dim neusachdat As New clsSachdaten
        Dim link As String
        Try
            neusachdat.feldname = maske.feldname
            '    link = getTemplates(maske.template, sachdatList)
            neusachdat.feldinhalt = maske.template ' maske.template ' weil typ 'l'
            neusachdat.neuerFeldname = maske.titel
            neusachdat.nr = maske.nr
            sachdatList.Add(neusachdat)


            'If link.StartsWith("specfunc") Then
            '    'button erzeugen
            '    '1-Titel des Buttons
            '    '2-function to call
            '    '3... -fs,fsgml,weistauf,zeigtauf,istgebucht,gisarea 
            '    'buttonINfostringspecfunc = link

            'Else
            '    entferneHTMLausLink(link)
            '    If link.ToLower.Contains("http") Then
            '        'bleibt unverändert
            '    Else
            '        link = link
            '    End If
            '    neusachdat.feldinhalt = link.Trim ' maske.template ' weil typ 'l'
            '    neusachdat.neuerFeldname = maske.titel
            '    neusachdat.nr = maske.nr
            '    sachdatList.Add(neusachdat)
            'End If

        Catch ex As Exception
            l("fehler in attachTemplate: ", ex)
        End Try
    End Sub

    Private Sub attachS(feldname As String, sachdatList As List(Of clsSachdaten), maske As MaskenObjekt, ergaenz As List(Of clsSachdaten))
        Dim temp As New clsSachdaten
        Dim icnt As Integer = 0
        Try
            For Each sachdat As clsSachdaten In sachdatList
                If sachdat.feldname.ToLower.Trim = "gid" Then
                    icnt += 1
                End If
                If feldname.ToLower.Trim = sachdat.feldname.ToLower.Trim Then
                    temp = New clsSachdaten
                    'sachdat.neuerFeldname = sachdat.neuerFeldname & "#" & maske.titel & "#" & icnt
                    'sachdat.nr = maske.nr

                    temp.feldinhalt = sachdat.feldinhalt.Trim ' maske.template ' weil typ 'l'
                    temp.neuerFeldname = maske.titel & "_" & icnt
                    temp.feldname = maske.feldname
                    temp.nr = maske.nr
                    ergaenz.Add(temp)
                    ' Exit For
                End If
            Next
        Catch ex As Exception
            l("fehler in attachTitel: ", ex)
        End Try
    End Sub

    Private Sub attachSalt(feldname As String, sachdatList As List(Of clsSachdaten), maske As MaskenObjekt, ergaenz As List(Of clsSachdaten))
        Dim link As String
        Dim icnt As Integer = 1
        Dim oldlink As String = ""
        Dim temp As New clsSachdaten
        'die gesamte tabelle wird aufsummiert
        Try
            For Each neusachdat As clsSachdaten In sachdatList
                If neusachdat.feldinhalt = "55" Or
                        neusachdat.feldinhalt = "56" Or
                        neusachdat.feldinhalt = "57" Then
                    Debug.Print("")
                End If
                temp = New clsSachdaten
                neusachdat.neuerFeldname = maske.titel
                neusachdat.nr = maske.nr
                'neusachdat.feldname = maske.feldname
                link = getTemplates(maske.template, sachdatList)
                'If link.IsNothingOrEmpty Then
                '    neusachdat.nr = 0
                '    Continue For
                'End If
                'If link = oldlink Then
                '    neusachdat.nr = 0
                '    Continue For
                'End If
                'oldlink = link
                ''entferneHTMLausLink(link) 
                'If Not link.ToLower.Contains("http") Then
                '    link = link
                'End If
                temp.feldinhalt = link.Trim ' maske.template ' weil typ 'l'
                temp.neuerFeldname = maske.titel
                temp.feldname = maske.feldname
                temp.nr = icnt
                ergaenz.Add(temp)
                icnt += 1
            Next
            icnt -= 1
        Catch ex As Exception
            l("fehler in attachSumme: ", ex)
        End Try
    End Sub

    Private Sub attachGattungArten(feldname As String, sachdatList As List(Of clsSachdaten), maske As MaskenObjekt)
        Dim link As String
        Dim icnt As Integer = 1
        Dim oldlink As String = ""
        'die gesamte tabelle wird aufsummiert
        Try
            For Each neusachdat As clsSachdaten In sachdatList
                neusachdat.neuerFeldname = maske.titel
                neusachdat.nr = maske.nr

                neusachdat.feldname = maske.feldname
                link = getTemplates(maske.template, sachdatList)
                If link.IsNothingOrEmpty Then
                    neusachdat.nr = 0
                    Continue For
                End If
                If link = oldlink Then
                    neusachdat.nr = 0
                    Continue For
                End If
                oldlink = link
                'entferneHTMLausLink(link)

                If Not link.ToLower.Contains("http") Then
                    link = link
                End If
                neusachdat.feldinhalt = link.Trim ' maske.template ' weil typ 'l'
                neusachdat.neuerFeldname = maske.titel
                neusachdat.nr = icnt
                'sachdatList.Add(neusachdat)
                icnt += 1

            Next
            icnt -= 1
        Catch ex As Exception
            l("fehler in attachSumme: ", ex)
        End Try
    End Sub

    Private Sub entferneHTMLausLink(ByRef link As String)
        Dim a() As String
        link = link.Replace("<a href='", " ")
        link = link.Replace("</a>", " ")
        link = link.Replace("target=", " ")
        link = link.Replace("_blank", " ")

        a = link.Split(">"c)
        link = a(0)
        link = link.Replace("'", "")
        link = link.Trim
    End Sub

    Private Sub attachTemplate(feldname As String, sachdatList As List(Of clsSachdaten), maske As MaskenObjekt,
                                ByRef buttonINfostringspecfunc As String)
        Dim neusachdat As New clsSachdaten
        Dim link As String
        Try
            neusachdat.feldname = maske.feldname
            link = getTemplates(maske.template, sachdatList)
            If link.StartsWith("specfunc") Then
                'button erzeugen
                '1-Titel des Buttons
                '2-function to call
                '3... -fs,fsgml,weistauf,zeigtauf,istgebucht,gisarea 
                buttonINfostringspecfunc = link

            Else
                entferneHTMLausLink(link)
                If link.ToLower.Contains("http") Then
                    'bleibt unverändert
                Else
                    link = link
                End If
                neusachdat.feldinhalt = link.Trim ' maske.template ' weil typ 'l'
                neusachdat.neuerFeldname = maske.titel
                neusachdat.nr = maske.nr
                sachdatList.Add(neusachdat)
            End If

        Catch ex As Exception
            l("fehler in attachTemplate: ", ex)
        End Try
    End Sub

    Private Sub attachLink(feldname As String, sachdatList As List(Of clsSachdaten), maske As MaskenObjekt)
        Dim neusachdat As New clsSachdaten
        Dim link As String
        Try
            neusachdat.feldname = maske.feldname
            link = getTemplates(maske.template, sachdatList)
            entferneHTMLausLink(link)
            If link.ToLower.Trim.EndsWith("/.pdf") Then
                'neusachdat.feldinhalt = "kein PDF verfügbar!" ' maske.template ' weil typ 'l'
                neusachdat.feldinhalt = " " ' maske.template ' weil typ 'l'
                neusachdat.neuerFeldname = maske.titel
                neusachdat.nr = maske.nr
                sachdatList.Add(neusachdat)
            Else
                If link.ToLower.Contains("http") Then
                    'bleibt unverändert
                Else
                    If link.ToLower.StartsWith("\\" & myglobalz.HauptServerName) Then
                    Else
                        'If iminternet Then
                        link = (serverWeb & link).Replace("\", "/")
                        'Else
                        '    link = (serverUNC & link).Replace("/", "\")
                        'End If
                    End If
                End If
                neusachdat.feldinhalt = link.Trim ' maske.template ' weil typ 'l'
                neusachdat.neuerFeldname = maske.titel
                neusachdat.nr = maske.nr
                sachdatList.Add(neusachdat)
            End If
        Catch ex As Exception
            l("fehler in attachLink: ", ex)
        End Try
    End Sub

    Private Function getTemplates(template As String, sachdatList As List(Of clsSachdaten)) As String
        l("getTemplates-------------------------------")
        Dim dummy As String
        Dim feldname As String
        Dim feldinhalt As String
        Try
            template = template.Trim
            For Each sachdat As clsSachdaten In sachdatList
                dummy = "[" & sachdat.feldname.ToLower.Trim & "]".ToLower.Trim
                If template.Contains(dummy) Then
                    feldname = sachdat.feldname.ToLower.Trim
                    feldinhalt = sachdat.feldinhalt.Trim
                    template = template.Replace(dummy, feldinhalt)
                End If
            Next
            Return template
        Catch ex As Exception
            l("fehler in getTemplates: ", ex)
            Return ""
        End Try
    End Function

    Private Sub attachTitel(feldname As String, sachdatList As List(Of clsSachdaten), maske As MaskenObjekt)
        Try
            For Each sachdat As clsSachdaten In sachdatList
                If feldname.ToLower.Trim = sachdat.feldname.ToLower.Trim Then
                    sachdat.neuerFeldname = maske.titel
                    sachdat.nr = maske.nr
                    Exit For
                End If
            Next
        Catch ex As Exception
            l("fehler in attachTitel: ", ex)
        End Try
    End Sub

    'Friend Function makeResultString(sachdatList As List(Of clsSachdaten)) As String
    '    Dim summe As String = ""
    '    Try
    '        For Each sach As clsSachdaten In sachdatList
    '            If sach.nr > 0 Then
    '                summe = summe & sach.neuerFeldname & vbTab & ":  " & sach.feldinhalt & Environment.NewLine
    '            End If
    '        Next
    '        Return summe
    '    Catch ex As Exception
    '        l("fehler in makeResultString: " ,ex)
    '        Return ""
    '    End Try
    'End Function

    Friend Function alleSpaltenOhneNrRausschmeissen(ssachdatList As List(Of clsSachdaten)) As List(Of clsSachdaten)
        Dim neuliste As New List(Of clsSachdaten)
        Dim neuo As clsSachdaten
        Try
            For Each alt As clsSachdaten In ssachdatList
                If alt.nr < 1 Then
                    'überspringen
                Else
                    neuo = New clsSachdaten
                    neuo = CType(alt.Clone, clsSachdaten)
                    neuliste.Add(neuo)
                End If
            Next
            Return neuliste
        Catch ex As Exception
            l("fehler in alleSpaltenOhneNrRausschmeissen: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Function makeNotMaske(sachdatList As List(Of clsSachdaten)) As List(Of MaskenObjekt)
        Dim mo As New MaskenObjekt
        Dim moList As New List(Of MaskenObjekt)
        Try
            For i = 0 To sachdatList.Count - 1
                If sachdatList(i).feldname.Trim = "geom" Then Continue For
                If sachdatList(i).feldname.Trim = "gid" Then Continue For
                If sachdatList(i).feldname.Trim = "aid" Then Continue For
                mo = New MaskenObjekt
                mo.cssclass = "norm"
                mo.feldname = clsString.Capitalize(sachdatList(i).feldname.Trim)
                mo.nr = i
                mo.template = ""
                mo.titel = clsString.Capitalize(sachdatList(i).feldname.Trim)
                mo.typ = "n"
                moList.Add(mo)
            Next
            Return moList
        Catch ex As Exception
            l("fehler in makeNotMaske: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Function getLinkTab2ValueFrom(sachdatListTabelle1 As List(Of clsSachdaten), tab_id As String) As String
        Try
            For Each sd As clsSachdaten In sachdatListTabelle1
                If tab_id.ToLower = sd.feldname.ToLower Then
                    Return sd.feldinhalt
                End If
            Next
            Return ""
        Catch ex As Exception
            l("Fehler in getLinkTab2ValueFrom: ", ex)
            Return ""
        End Try
    End Function

    Friend Sub entferneHTML(sachdatListTabelle1 As List(Of clsSachdaten))
        Try
            'https://wiki.selfhtml.org/wiki/Referenz:HTML/Zeichenreferenz 
            For Each sd As clsSachdaten In sachdatListTabelle1
                If Not sd.feldinhalt.IsNothingOrEmpty Then
                    sd.feldinhalt = sd.feldinhalt.Replace("<br>", Environment.NewLine)
                    If sd.feldinhalt.Contains("&#176;") Then
                        Debug.Print("")
                    End If
                    sd.feldinhalt = sd.feldinhalt.Replace("&#176;", "°")
                    sd.feldinhalt = sd.feldinhalt.Replace("<a href='", " ")
                    sd.feldinhalt = sd.feldinhalt.Replace("</a>", " ")
                    ' sd.feldinhalt =  sd.feldinhalt.Replace("' target='", " ")
                    sd.feldinhalt = sd.feldinhalt.Replace("target=", " ")
                    sd.feldinhalt = sd.feldinhalt.Replace("_blank", " ")
                End If
                If Not sd.neuerFeldname.IsNothingOrEmpty Then
                    sd.neuerFeldname = sd.neuerFeldname.Replace("&#176;", "°")
                End If
            Next
        Catch ex As Exception
            l("Fehler in entferneHTML: ", ex)
        End Try
    End Sub

    Friend Sub bplanbegleitInfoAufloesen(buttonINfostring As String, ByRef gemarkung As String, ByRef pdf As String)
        Try
            l("bplanbegleitInfoAufloesen---------------------- anfang")
            Debug.Print(buttonINfostring)
            'specfunc,pdf,bplanbegleit,di_2-1,Dietzenbach
            Dim temp As String
            temp = buttonINfostring.ToLower
            temp = temp.Replace("specfunc,pdf,bplanbegleit,", "")
            Dim a = temp.Split(","c)
            gemarkung = a(1)
            pdf = a(0)
            If pdf.IsNothingOrEmpty Then
                l("vehler in a bplanbegleitInfoAufloesen: " & buttonINfostring)
            End If
            l("bplanbegleitInfoAufloesen---------------------- ende")
        Catch ex As Exception
            l("Fehler in bplanbegleitInfoAufloesen: " & ex.ToString())
            gemarkung = ""
            pdf = ""
        End Try
    End Sub

    Private Function getSachdatlistFromAjax(colnames() As String, result As String, ByRef aktfs As String) As List(Of clsSachdaten)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim oldname As String = ""
        Dim sdlist As New List(Of clsSachdaten)
        Dim newsd As clsSachdaten
        Try
            l(" bildeOSInt_arrayColl_ajax html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in bildeOSInt_arrayColl_ajax: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            For izeile = 0 To zeilen - 1
                b = a(izeile).Split("#"c)
                For jspalte = 0 To b.Count - 1
                    If colnames(jspalte).ToLower = "geom" Then Continue For
                    newsd = New clsSachdaten
                    newsd.feldname = colnames(jspalte).Trim
                    newsd.feldinhalt = b(jspalte).Trim()
                    sdlist.Add(newsd)
                    Debug.Print("")
                Next
            Next
            Return sdlist
            l(" bildeOSInt_arrayColl_ajax ---------------------- ende")
        Catch ex As Exception
            l("Fehler in bildeOSInt_arrayColl_ajax: " & ex.ToString())
            Return Nothing
        End Try
    End Function
End Module
