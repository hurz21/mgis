Imports mgis

Module bmTools
    Friend Function bildebookMarkObj(titel As String, cbFreigabefuerGruppe As Boolean) As clsBookmark
        Dim bm As New clsBookmark
        Try
            l("bildebookMark---------------------- anfang")
            bm.fav = favoTools.makeFavoritObjekt()
            bm.titel = titel
            bm.datum = Now
            Dim erfolg As Boolean = bm.range.rangekopieren(kartengen.aktMap.aktrange, bm.range)
            If Not bm.range.istBrauchbar Then
                MessageBox.Show("Der ausgewählte Bereich ist nicht brauchbar. Abbruch!")
                Return Nothing
            End If
            bm.free4mygruppe = cbFreigabefuerGruppe
            bm.user = GisUser
            bm.freigabe_Intranet = False
            Return bm
            l("bildebookMark---------------------- ende")
        Catch ex As Exception
            l("Fehler in bildebookMark: ", ex)
            MsgBox("Fehler in bildebookMark: " & ex.ToString)
            Return Nothing
        End Try
    End Function

    Friend Function BMSaveInsert(bm As clsBookmark, ByRef newid As Long) As Boolean
        Try
            l("bmInsertDB---------------------- anfang")
            Dim gruppe, schema As String
            gruppe = clsString.normalize(bm.user.ADgruppenname.ToLower.Trim)
            schema = If(iminternet, "externparadigma", "public")
            webgisREC.mydb.SQL = "INSERT INTO " & schema & ".bookmarks " &
                        "(username,user_gruppe,titel,vorhanden,gecheckt,hgrund,aktiv,ts,xl,xh,yl,yh,free4mygruppe) " &
                         "VALUES('" &
                        bm.user.nick.ToLower.Trim & "','" & gruppe & "','" &
                        bm.titel & "','" & bm.fav.vorhanden & "','" & bm.fav.gecheckted & "','" &
                          bm.fav.hgrund & "','" & bm.fav.aktiv & "','" &
                        DateTime.Now & "'," &
                         CInt(bm.range.xl) & "," &
                        CInt(bm.range.xh) & "," & CInt(bm.range.yl) & "," & CInt(bm.range.yh) & "," & CBool(bm.free4mygruppe) &
                       ") RETURNING id"
            'If iminternet Or CGIstattDBzugriff Then
            Dim hinweis As String = "", result As String
            result = clsToolsAllg.getSQL4Http(webgisREC.mydb.SQL, "webgiscontrol", hinweis, "getsql") : l(hinweis)
            result = result.Replace("$", "")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                Return False
            Else
                If IsNumeric(result) Then
                    newid = CInt(result)
                Else
                    newid = 0
                End If
            End If
            'Else
            '    res = webgisREC.sqlexecute(newid)
            '    l(webgisREC.hinweis)
            'End If
            If newid < 1 Then
                Return False
            Else
                Return True
            End If
            l(" bmInsertDB---------------------- ende" & webgisREC.mydb.SQL)
        Catch ex As Exception
            l("Fehler in bmInsertDB: ", ex)
            MsgBox("Fehler in bmInsertDB: " & ex.ToString)
            Return False
        End Try
    End Function

    Friend Function getNeulistBM_AJAX(result As String) As List(Of clsBookmark)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim oldname As String = ""
        Dim oslcoll As New List(Of clsBookmark)
        Dim neu As clsBookmark
        Try
            l(" getNeulistBMJAX html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in bildeOSInt_arrayColl_ajax: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            For izeile = 0 To zeilen - 1
                b = a(izeile).Split("#"c)
                neu = New clsBookmark
                neu.id = CInt(clsDBtools.fieldvalue(b(0)))
                neu.titel = CStr(clsDBtools.fieldvalue(b(2)))
                neu.fav.vorhanden = CStr(clsDBtools.fieldvalue(b(3)))
                neu.fav.gecheckted = CStr(clsDBtools.fieldvalue(b(4)))
                neu.fav.hgrund = CStr(clsDBtools.fieldvalue(b(5)))
                neu.fav.aktiv = CStr(clsDBtools.fieldvalue(b(6)))
                neu.range.xl = CInt(clsDBtools.fieldvalue(b(7)))
                neu.range.xh = CInt(clsDBtools.fieldvalue(b(8)))
                neu.range.yl = CInt(clsDBtools.fieldvalue(b(9)))
                neu.range.yh = CInt(clsDBtools.fieldvalue(b(10)))
                neu.datum = CDate(Convert.ToDateTime(b(11)))
                neu.user.ADgruppenname = CStr(clsDBtools.fieldvalue(b(12)))
                neu.free4mygruppe = CBool(clsDBtools.toBool(b(13)))
                oslcoll.Add(neu)
            Next
            Return oslcoll
            l(" getNeulistBMJAX ---------------------- ende")
        Catch ex As Exception
            l("Fehler in getNeulistBMJAX: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Function btnBMloeschen(bm As clsBookmark) As Boolean
        Try
            'Dim newid As Long
            'Dim res As Long
            l("btnBMloeschen---------------------- anfang")
            Dim schema As String = If(iminternet, "externparadigma", "public")

            webgisREC.mydb.SQL = "delete from " & schema & ".bookmarks where id=" & bm.id
            'If iminternet Or CGIstattDBzugriff Then
            Dim hinweis As String = "", result As String
            result = clsToolsAllg.getSQL4Http(webgisREC.mydb.SQL, "webgiscontrol", hinweis, "putsql") : l(hinweis)
            result = result.Replace("$", "").Replace(vbCrLf, "")
            'Else
            '    res = webgisREC.sqlexecute(newid)
            '    l(webgisREC.hinweis)
            '    If res < 1 Then
            '        Return False
            '    Else
            '        Return True
            '    End If
            'End If
            If result <> "1" Then
                Return False
            Else
                Return True
            End If
            l("-btnBMloeschen--------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in btnBMloeschen : ", ex)
            Return False
        End Try
    End Function

    Friend Function BMsaveedit(bm As clsBookmark) As Boolean
        'Dim newid As Long
        'Dim res As Long
        Try
            l("saveedit---------------------- anfang")
            Dim schema As String = If(iminternet, "externparadigma", "public")

            webgisREC.mydb.SQL = "update " & schema & ".bookmarks set titel='" & bm.titel.Trim & "'," &
                "free4mygruppe=" & CBool(bm.free4mygruppe) &
                " where id=" & bm.id
            Dim hinweis As String = "", result As String
            result = clsToolsAllg.getSQL4Http(webgisREC.mydb.SQL, "webgiscontrol", hinweis, "putsql") : l(hinweis)
            result = result.Replace("$", "").Replace(vbCrLf, "")

            'res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
            If result <> "1" Then
                Return False
            Else
                Return True
            End If
            l(" saveedit---------------------- ende" & webgisREC.mydb.SQL)
            Return True
            l("saveedit---------------------- ende")
        Catch ex As Exception
            l("Fehler in saveedit: ", ex)
            Return False
        End Try
    End Function
End Module
