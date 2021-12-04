Imports System.Data
Imports mgis

Public Class clsToolsAllg


    Private Shared Function ajaxSchemaAttribute(result As String) As clsTabellenDef
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        'Dim lok As New List(Of clsFlurauswahl)
        'Dim strasse As New clsFlurauswahl
        Dim fdat As New clsTabellenDef
        Dim oldname As String = ""
        Try
            l(" ajaxSchemaAttribute html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in ajaxSchemaAttribute: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            'strasse = New clsFlurauswahl
            'b = a(i).Split("#"c)
            fdat.Schema = b(0).Trim
            fdat.tabelle = b(1).Trim
            fdat.tab_id = b(2).Trim
            fdat.tab_nr = b(3).Trim
            fdat.linkTabs = b(4).Trim
            fdat.tabtitel = b(5).Trim
            fdat.tabellen_anzeige = b(6).Trim

            Return fdat
            l(" ajaxSchemaAttribute ---------------------- ende")
        Catch ex As Exception
            l("Fehler in ajaxSchemaAttribute: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Shared Sub userlayerNeuErzeugen(username As String, vid As String) 'GisUser.nick,aktvorgangsid
        l("userlayerNeuErzeugen--------------------------------")
        Dim rumpf As String = URLlayer2shpfile
        rumpf &= username
        rumpf &= "&vid=" & vid
        rumpf &= "&modus=einzeln"
        nachricht("url: " & rumpf)
        Dim hinweis As String = ""
        l("meinHttpJob  " & meineHttpNet.meinHttpJob("", rumpf, hinweis, myglobalz.enc, 10000))
    End Sub
    Shared Function koordinateKlickBerechnen(ByVal KoordinateKLickpt As Point?) As String
        Dim newpoint2 As New myPoint
        'Dim aktpoint As New myPoint
        newpoint2.X = CDbl(KoordinateKLickpt.Value.X)
        newpoint2.Y = CDbl(KoordinateKLickpt.Value.Y)
        aktGlobPoint = clsAufrufgenerator.WINPOINTVonCanvasNachGKumrechnen(newpoint2,
                                                                           kartengen.aktMap.aktrange,
                                                                           kartengen.aktMap.aktcanvas)
        aktGlobPoint.SetToInteger()
        Return aktGlobPoint.toString
        newpoint2 = Nothing
        aktGlobPoint = Nothing
    End Function
    Shared Function setPosition(kategorie As String, eintrag As String, aktval As Double) As Double
        'Me.Top = clsToolsAllg.setPosition("diverse", "dbabfrageformpositiontop", Me.Top)
        'Me.Left = clsToolsAllg.setPosition("diverse", "dbabfrageformpositionleft", Me.Left)
        Dim retval As Double
        Try
            l(" setPosition ---------------------- anfang")
            Dim topf As String = userIniProfile.WertLesen(kategorie, eintrag)
            If String.IsNullOrEmpty(topf) Then
                userIniProfile.WertSchreiben(kategorie, eintrag, CType(aktval, String))
                retval = aktval
            Else
                retval = CDbl(topf)
            End If
            l(" getIniDossier ---------------------- ende")
            Return retval
        Catch ex As Exception
            l("Fehler in setPosition: " & ex.ToString())
            Return aktval
        End Try
    End Function
    Shared Sub startbplankataster()
        Dim handle As Process
        Try
            l(" startbplankataster ---------------------- anfang")
            Threading.Thread.Sleep(500)
            If iminternet Then
                Dim exe = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) & "\" & "bplankataster\bplanupdate.exe"
                Dim fi As New IO.FileInfo(exe)
                If fi.Exists Then
                    handle = Process.Start(exe)
                Else
                    MessageBox.Show("Die Software wird nun installiert.")
                    Process.Start("https://buergergis.kreis-offenbach.de/fkat/paradigma/bplan/bplaninstall.exe")
                End If
            Else
                'MsgBox(strGlobals.bplanupdateBat)
                handle = Process.Start(strGlobals.bplanupdateBat)
            End If
            l(" startbplankataster ---------------------- ende")
        Catch ex As Exception
            l("Fehler in startbplankataster: " & ex.ToString())
        End Try
    End Sub
    Shared Function genCSV4DT(trenner As String, datatab As DataTable, startspalte As Integer) As String
        Dim out As String
        Dim sb As New Text.StringBuilder
        Try
            l("genCSV4DT---------------------- anfang")
            For j = startspalte To datatab.Columns.Count - 1
                sb.Append(clsDBtools.fieldvalue(datatab.Columns(j).ColumnName).Trim & trenner)
            Next
            sb.Append(Environment.NewLine)
            For i = 0 To datatab.Rows.Count - 1
                For j = startspalte To datatab.Columns.Count - 1
                    sb.Append(clsDBtools.fieldvalue(datatab.Rows(i).Item(j)).Trim() & trenner)
                Next
                sb.Append(Environment.NewLine)
            Next
            out = sb.ToString
            sb = Nothing
            Return out
            l("genCSV4DT---------------------- ende")
        Catch ex As Exception
            l("Fehler in genCSV4DT: " & ex.ToString())
            Return "fehler bei der CSV-Erzeugung"
        End Try
    End Function

    Friend Shared Function initMgisHistory() As String
        Try
            l(" initMgisHistory ---------------------- anfang")
            Dim localAppDatMGISDir As String = System.Environment.GetEnvironmentVariable("APPDATA") & "\mgis"
            Dim ClientCookieDir = localAppDatMGISDir & "\rangecookies\"
            IO.Directory.CreateDirectory(ClientCookieDir)
            '  collHistory = CLstart.HistoryKookie.VerlaufsCookieLesen.exe(ClientCookieDir & "verlaufscookies")
            l(" initMgisHistory ---------------------- ende")
            Return ClientCookieDir
        Catch ex As Exception
            l("Fehler in initMgisHistory: " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Sub mgisRangeCookieSave(range As clsRange, mgisRangecookieDir As String)
        Dim filename As String = ""
        Try
            l(" mgisRangeCookieSave ---------------------- anfang")
            filename = CInt(range.xl) & "_" &
                        CInt(range.xh) & "_" & CInt(range.yl) & "_" & CInt(range.yh) & "_" &
                        clsString.date2string(Now, 5) & ".rng"
            filename = mgisRangecookieDir & filename
            l("filename" & filename)
            IO.File.Create(filename)
            l(" mgisRangeCookieSave ---------------------- ende")
        Catch ex As Exception
            l("Fehler in mgisRangeCookieSave: " & filename & ex.ToString())
        End Try
    End Sub
    Friend Shared Function getSchemaFromHTTP(Fdaten1 As clsTabellenDef, ByRef hinweis As String) As clsTabellenDef
        'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getschema4aid&aid=173&tabnr=1
        Dim result As String
        Dim fdat As New clsTabellenDef
        Try
            l(" MOD getSchemaFromHTTP---------------------- anfang")
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick &
                "&modus=getschema4aid" &
                "&aid=" & Fdaten1.aid &
                "&tabnr=" & Fdaten1.tab_nr
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            fdat = ajaxSchemaAttribute(result)
            fdat.aid = Fdaten1.aid
            Return fdat
        Catch ex As Exception
            l("Fehler beim getSchemaFromHTTP ", ex)
            Return Nothing
        End Try
    End Function
    Friend Shared Function getSQL4Http(sQL As String, dbname As String, ByRef hinweis As String, modus As String, Optional timeout As Integer = 5000) As String
        'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getschema4aid&aid=173&tabnr=1
        'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getsql&skl=#Select#distinct#okategorie#from##planung.os_bebauungsplan_f#order#by#okategorie&dbname=postgis20
        Dim result As String
        'Dim fdat As New clsTabellenDef
        Try
            l(" MOD getSQL4Http---------------------- anfang")
            sQL = sQL.Trim.Replace(" ", "+")
            'sQL = sQL.Trim.Replace("'", "&27")
            l(sQL)
            l(dbname)
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick &
                "&modus=" & modus &
                "&dbname=" & dbname &
                "&sql=" & sQL
            l(aufruf)
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, timeout)
            nachricht(hinweis)
            'fdat = ajaxSchemaAttribute(result)
            If result.IsNothingOrEmpty Then
                l("Fehler beim getSQL4Http: rsult is notzhing  " & sQL & " " & dbname & " " & modus)
            End If
            Return result
        Catch ex As Exception
            l("Fehler beim getSQL4Http " & " " & dbname & " " & modus & " ", ex)
            Return Nothing
        End Try
    End Function

    Friend Shared Function ajaxMakeOSkat(result As String) As String()
        Dim zeilen As Integer
        Dim a() As String
        Dim lok As New List(Of clsFlurauswahl)
        Dim strasse As New clsFlurauswahl
        Dim oldname As String = ""
        Dim feld() As String
        Try
            l(" ajaxMakeOSkat html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in ajaxMakeOSkat: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            ReDim feld(a.Count - 1)
            For i = 0 To zeilen - 1
                feld(i) = a(i)
            Next
            l(" ajaxMakeOSkat ---------------------- ende")
            Return feld

        Catch ex As Exception
            l("Fehler in ajaxMakeOSkat: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Shared Function bildeOSInt_arrayCollDB(basisrec As clsDBspecPG) As List(Of String())
        Dim oslcoll As New List(Of String())
        Dim linearray() As String
        Try
            l("bildeOS_arrayColl---------------------- anfang")
            For izeile = 0 To basisrec.dt.Rows.Count - 1
                ReDim linearray(basisrec.dt.Columns.Count - 1)
                For jspalte = 0 To basisrec.dt.Columns.Count - 1
                    linearray(0) = clsDBtools.fieldvalue(basisrec.dt.Rows(izeile).Item(0)).Trim
                    linearray(jspalte) = clsDBtools.fieldvalue(basisrec.dt.Rows(izeile).Item(jspalte)).Trim
                Next
                oslcoll.Add(linearray)
            Next
            Return oslcoll
            l("bildeOS_arrayColl---------------------- ende")
        Catch ex As Exception
            l("Fehler in bildeOS_arrayColl2: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Shared Function bildeOSInt_arrayColl_ajax(result As String) As List(Of String())
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim oldname As String = ""
        Dim oslcoll As New List(Of String())
        Dim linearray() As String
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
                ReDim linearray(b.Count - 1)
                For jspalte = 0 To b.Count - 1
                    linearray(0) = clsDBtools.fieldvalue(b(0)).Trim
                    linearray(jspalte) = clsDBtools.fieldvalue(b((jspalte))).Trim
                Next
                oslcoll.Add(linearray)
            Next
            Return oslcoll
            l(" bildeOSInt_arrayColl_ajax ---------------------- ende")
        Catch ex As Exception
            l("Fehler in bildeOSInt_arrayColl_ajax: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Shared Function getUserinfoFromServer(user As clsUser,
                                       ByRef hinweis As String) As String
        'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getschema4aid&aid=173&tabnr=1
        'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getsql&skl=#Select#distinct#okategorie#from##planung.os_bebauungsplan_f#order#by#okategorie&dbname=postgis20
        'https://buergergis.kreis-offenbach.de/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?nick=zahnlückenpimpf&pw=kucksdu&modus=userandinternetinfo&userinfo=feinen_j+DC4A3E91649500000000000000E0+KREIS-OF+kucksdu+KROF-000019        Dim result As String
        Dim result As String = ""
        Try
            l(" MOD getUserinfo---------------------- anfang" & Environment.NewLine &
                        user.nick & Environment.NewLine)
            aufruf =
                myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?pw=" & user.EmailPW &
                "&modus=userandinternetinfo" &
                "&nick=" & user.nick &
                "&machinename=" & user.MachineName &
                "&macadress=" & user.macAdress &
                "&cpuid=" & user.cpuID &
                "&domainname=" & user.domain &
                "&user=" & user.nick &
                "&ts=" & clsString.date2string(Now, 1)

            l(aufruf)
            'If iminternet Then
            '    aufruf = aufruf & "&userinfo=" & userinfo
            'Else

            'End If
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            ' result = "179,18"
            If result.IsNothingOrEmpty Then
                Return ""
            Else
                result = result.Trim
                l("result: " & result)
                result = result.Replace("#", "")
                nachricht(hinweis)
                Return result
            End If

        Catch ex As Exception
            l("Fehler beim getUserinfo " & Environment.NewLine &
              "result:" & result & "<" & Environment.NewLine &
              aufruf & Environment.NewLine &
              ex.ToString)
            Return Nothing
        End Try
    End Function

    Friend Shared Function getKategorienListe(allLayersPres As List(Of clsLayerPres)) As List(Of clsUniversal)
        Dim newlist As New List(Of clsUniversal)
        Dim uni As New clsUniversal
        Try
            l(" MOD getKategorienListe anfang")
            'For Each d As clsLayerPres In allLayersPres
            '    Debug.Print(d.standardsachgebiet)
            'Next
            Dim result = From v In allLayersPres
                         Where v.standardsachgebiet.ToLower <> "unsichtbar" And
                                v.standardsachgebiet.ToLower <> "hintergrund" And
                                v.standardsachgebiet.ToLower <> "luftbild" And
                                v.standardsachgebiet.ToLower <> "stadtplan"
                         Order By v.standardsachgebiet
                         Select v.standardsachgebiet, v.kategorieLangtext, v.kategorieToolTip Distinct
            For Each res In result
                uni = New clsUniversal
                uni.tag = res.standardsachgebiet
                uni.titel = res.kategorieLangtext
                uni.ToolTip = clsLayerHelper.getkatTooltipFromFile(uni.tag, strGlobals.gisWorkingDir & "\kat")
                newlist.Add(uni)
            Next
            'result = result.Where(Function(d) d..Contains("GHI"))
            'newlist = result.ToList  'esult, List(Of String))
            l(" MOD getKategorienListe ende")
            Return newlist
        Catch ex As Exception
            l("Fehler in getKategorienListe: " & ex.ToString())
            Return newlist
        End Try
    End Function


End Class
