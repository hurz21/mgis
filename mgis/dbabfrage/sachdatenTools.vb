Imports System.Data
Imports mgis

Module sachdatenTools
    Sub korrigiereTabellenSchemaFallsEintraegeFalsch(os_tabelledef As clsTabellenDef)
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
            l("Fehler in korrigiereTabellenSchemaFallsEintraegeFalsch: " & ex.ToString)
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
        Try
            Dim dt As DataTable
            dt = getDTFromWebgisDB("select count(*) from public.attributtabellen where aid=" & aktaid, "webgiscontrol")
            Return CInt(clsDBtools.fieldvalue(dt.Rows(0).Item(0)).ToString())
        Catch ex As Exception
            l("fehler in initHaupSachgebietAuswahlColl " & ex.ToString)
            MsgBox("initCmbRang " & ex.ToString)
            Return -1
        End Try
    End Function
    Friend Sub getSChema(Fdaten1 As clsTabellenDef) 'ByRef aktschema As String, ByRef akttabelle As String,
        'ByRef aktidspalte As String,
        'ByRef tabnr As String,
        'aid As Integer,
        'linktab2 As String)
        l("getschema---------------------")
        Dim SQL As String = ""
        Try

            If Fdaten1.aid = "0" Then
                l("keine AID angegeben")
                Exit Sub
            End If
            SQL = "select * from public.attributtabellen   where aid=" & Fdaten1.aid & " and tab_nr=" & Fdaten1.tab_nr
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "webgiscontrol")
            If clsWebgisPGtools.hatRecords(dt) Then
                Fdaten1.Schema = clsDBtools.fieldvalue(dt.Rows(0).Item("schema"))
                Fdaten1.tabelle = clsDBtools.fieldvalue(dt.Rows(0).Item("tabelle"))
                Fdaten1.tab_id = clsDBtools.fieldvalue(dt.Rows(0).Item("tab_id")) 'id_spalte
                Fdaten1.tab_nr = clsDBtools.fieldvalue(dt.Rows(0).Item("tab_nr"))
                Fdaten1.linkTabs = clsDBtools.fieldvalue(dt.Rows(0).Item("linktabs"))
                Fdaten1.tabtitel = clsDBtools.fieldvalue(dt.Rows(0).Item("tab_titel"))
                Fdaten1.tabellen_anzeige = clsDBtools.fieldvalue(dt.Rows(0).Item("tabellen_anzeige"))
            Else
                l("fehler schema leer, sql: " & SQL)
            End If
        Catch ex As Exception
            l("fehler in getSChema: " & SQL & Environment.NewLine & ex.ToString)
        End Try
    End Sub

    Friend Function bplanbegleitInfoCalcDirectory(gemarkung As String, pdf As String, fkat As String) As String
        Try
            l("bplanbegleitInfoCalcDirectory---------------------- anfang")
            Dim root As String
            root = fkat & "bplan" & gemarkung & "\" & pdf & "\"
            Return root
            l("-bplanbegleitInfoCalcDirectory--------------------- ende")
        Catch ex As Exception
            l("Fehler in bplanbegleitInfoCalcDirectory: " & ex.ToString())
            Return ""
        End Try
    End Function
    Friend Function getBegleitplanFileliste(pdf As String, verzeichnis As String) As List(Of IO.FileInfo)
        Dim di As New IO.DirectoryInfo(verzeichnis)
        Dim templiste As IO.FileInfo()
        Dim ausschluss As String
        Dim begleitfilelist = New List(Of IO.FileInfo)
        Try
            l("getBegleitplanFileliste---------------------- anfang")
            templiste = di.GetFiles("*.pdf")
            Dim dra As IO.FileInfo
            ausschluss = pdf & ".pdf"
            'list the names of all files in the specified directory
            For Each dra In templiste
                Debug.Print(dra.ToString)
                If ausschluss <> dra.Name.ToLower Then
                    begleitfilelist.Add(dra)
                End If
            Next
            Return begleitfilelist
            l("getBegleitplanFileliste---------------------- ende")
        Catch ex As Exception
            l("Fehler in getBegleitplanFileliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Function getsachdaten(Fdaten1 As clsTabellenDef, ByRef aktfs As String) As List(Of clsSachdaten) 'aktschema As String, akttabelle As String, aktidspalte As String,
        '                           objektid As String,
        '                         tabnr As String) As List(Of clsSachdaten)
        Dim temp = ""
        Try
            Dim newsd As New clsSachdaten
            Dim wert As String
            Dim sachdatListm As New List(Of clsSachdaten)
            If IsNumeric(Fdaten1.gid) Then
                wert = Fdaten1.gid
            Else
                wert = "'" & Fdaten1.gid & "'"
            End If
            Dim SQL = "select * from " & Fdaten1.Schema & "." & Fdaten1.tabelle &
                          "   where " & Fdaten1.tab_id & "=" & wert
            'Dim dt As DataTable = clsWebgisPGtools.holeDTfromFKAT(SQL)

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
        Catch ex As Exception
            l("fehler in getSChema: " & ex.ToString)
            Return Nothing
        End Try
    End Function

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
        Try
            If Fdaten1.tabtitel.IsNothingOrEmpty Then
                titelvorspann = ""
            Else
                titelvorspann = Fdaten1.tabtitel & "."
            End If
            Dim dt As DataTable
            Dim Sql = "select * from public.tabellenvorlagen where aid=" & Fdaten1.aid &
                   " and tab_nr=" & Fdaten1.tab_nr &
                   " order by  nr"
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
        Catch ex As Exception
            l("Fehler keine tabellenvorlage gefunden, vermutlich stimmt die aid nicht" & ex.ToString)
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
                                        ByRef buttonINfostring As String) As String
        'akttabelle As String, aktschema As String, objektid As String) As String
        l("combineDatenAndDef--------------------")
        Dim kombi As String = ""
        Dim sb As New Text.StringBuilder
        l("combineDatenAndDef--------------------")
        Try
            For Each maske As MaskenObjekt In maskenObjektList
                l("zeile.feldname: " & maske.feldname)
                l("zeile.typ: " & maske.typ)
                Select Case maske.typ
                    Case "n"
                        attachTitel(maske.feldname, sachdatList, maske)
                    Case "l"
                        attachLink(maske.feldname, sachdatList, maske)
                    Case "t"
                        attachTemplate(maske.feldname, sachdatList, maske, buttonINfostring)
                    Case "s"
                        attachSumme(maske.feldname, sachdatList, maske)
                    Case "u"
                        attachSumme(maske.feldname, sachdatList, maske)
                    Case Else
                        attachTitel(maske.feldname, sachdatList, maske)
                End Select
            Next
            l(sb.ToString)
            Return sb.ToString
        Catch ex As Exception
            l("fehler in combineDatenAndDef: " & ex.ToString)
            Return "".ToString
        End Try
    End Function

    Private Sub attachSumme(feldname As String, sachdatList As List(Of clsSachdaten), maske As MaskenObjekt)
        Dim link As String
        Dim icnt As Integer = 0
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
                entferneHTMLausLink(link)

                If Not link.ToLower.Contains("http") Then
                    link = link
                End If
                neusachdat.feldinhalt = link.Trim ' maske.template ' weil typ 'l'
                neusachdat.neuerFeldname = maske.titel
                neusachdat.nr = icnt
                ' sachdatList.Add(neusachdat)
                icnt += 1

            Next

        Catch ex As Exception
            l("fehler in attachSumme: " & ex.ToString)
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
                                ByRef buttonINfostring As String)
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
                buttonINfostring = link

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
            l("fehler in attachTemplate: " & ex.ToString)
        End Try
    End Sub

    Private Sub attachLink(feldname As String, sachdatList As List(Of clsSachdaten), maske As MaskenObjekt)
        Dim neusachdat As New clsSachdaten
        'Dim a() As String
        Dim link As String
        Try
            neusachdat.feldname = maske.feldname
            link = getTemplates(maske.template, sachdatList)
            entferneHTMLausLink(link)
            If link.ToLower.Contains("http") Then
                'bleibt unverändert
            Else

                'If link.Contains("/fkat/bplan")
                If link.ToLower.StartsWith("\\w2gis02") Then
                Else
                    link = (serverUNC & link).Replace("/", "\")
                End If


            End If

            neusachdat.feldinhalt = link.Trim ' maske.template ' weil typ 'l'
            neusachdat.neuerFeldname = maske.titel
            neusachdat.nr = maske.nr


            sachdatList.Add(neusachdat)

            'For Each sachdat As clsSachdaten In sachdatList
            '    If feldname.ToLower.Trim = sachdat.feldname.ToLower.Trim Then
            '        sachdat.neuerFeldname = maske.titel
            '        sachdat.nr = maske.nr
            '        Exit For
            '    End If
            'Next

        Catch ex As Exception
            l("fehler in attachLink: " & ex.ToString)
        End Try
    End Sub

    Private Function getTemplates(template As String, sachdatList As List(Of clsSachdaten)) As String
        l("getTemplates-------------------------------")
        Dim dummy As String
        Dim feldname As String
        Dim feldinhalt As String
        Try
            template = template.ToLower.Trim
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
            l("fehler in getTemplates: " & ex.ToString)
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
            l("fehler in attachTitel: " & ex.ToString)
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
    '        l("fehler in makeResultString: " & ex.ToString)
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
            l("fehler in alleSpaltenOhneNrRausschmeissen: " & ex.ToString)
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
            l("fehler in makeNotMaske: " & ex.ToString)
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
            l("Fehler in getLinkTab2ValueFrom: " & ex.ToString)
            Return ""
        End Try
    End Function

    Friend Sub entferneHTML(sachdatListTabelle1 As List(Of clsSachdaten))
        Try
            'https://wiki.selfhtml.org/wiki/Referenz:HTML/Zeichenreferenz

            For Each sd As clsSachdaten In sachdatListTabelle1
                If Not sd.feldinhalt.IsNothingOrEmpty Then
                    sd.feldinhalt = sd.feldinhalt.Replace("<br>", Environment.NewLine)
                    'If Not (sd.feldinhalt.Contains("\nat")) Then
                    '    sd.feldinhalt = sd.feldinhalt.Replace("\n", Environment.NewLine)
                    'End If

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
            l("Fehler in entferneHTML: " & ex.ToString)
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
            l("bplanbegleitInfoAufloesen---------------------- ende")
        Catch ex As Exception
            l("Fehler in bplanbegleitInfoAufloesen: " & ex.ToString())
            gemarkung = ""
            pdf = ""
        End Try
    End Sub
End Module
