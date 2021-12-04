Imports System.Data
Imports mgis
Public Class clsLastrange
    Friend Shared Sub lastrangeDBsave(uname As String, newRange As clsRange)
        Dim istschonvorhanden As Boolean = False
        Dim altesLastrangetObjekt As New clsRange
        Dim schema, sql As String
        Try
            l("lastrangeDBsave---------------------- anfang")
            l("  GisUser.nick " & GisUser.nick)
            l("uname  " & uname)
            l("range " & newRange.toString)
            Dim oldrange As New clsRange
            If newRange Is Nothing OrElse Not newRange.istBrauchbar Then
                l("warnung in lastrangeDBsave: Not newRange.istBrauchbar")
                Exit Sub
            End If
            '  GisUser.nick =   GisUser.nick.ToLower.Trim
            uname = uname.ToLower.Trim

            schema = If(iminternet, "externparadigma", "public")

            Sql = "select * from " & schema & ".lastrange where trim(lower(username))='" &
                uname.ToLower.Trim & "' "

            oldrange = clsLastrange.lastrangeLadenDB(GisUser.nick)
            If oldrange Is Nothing Then
                l("fehler lastrangeDBsave oldrange is nothing")
                istschonvorhanden = False
                lastrangeInsertDB(newRange)
            Else
                If oldrange.xl > 1000 Then istschonvorhanden = True
                'Dim dt As DataTable
                'dt = getDTFromWebgisDB(sql, "webgiscontrol")
                'istschonvorhanden = clsWebgisPGtools.hatRecords(dt)
                If istschonvorhanden Then
                    'altesLastrangetObjekt = lastrangeDb2Obj(dt)
                    'l("istschonvorhanden " & istschonvorhanden)
                    'If altesLastrangetObjekt Is Nothing Then
                    '    l("altesFavoritObjekt is nothing daher  ende")
                    '    Exit Sub
                    'End If
                    'If altesLastrangetObjekt.isSameAs(daRange) Then
                    '    l("saveFavoritDB speichern nicht nötig, da identischer inhalt ende")
                    '    Exit Sub
                    'End If
                    lastrangeUpdateDB(newRange)
                Else
                    lastrangeInsertDB(newRange)
                End If
            End If

            l("lastrangeDBsave---------------------- ende")
        Catch ex As Exception
            l("Fehler in lastrangeDBsave: " & " uname  " & uname & Environment.NewLine &
                " newRange.toString  " & newRange.toString & Environment.NewLine &
                " istschonvorhanden  " & istschonvorhanden & Environment.NewLine &
                " schema  " & schema & Environment.NewLine &
                " Sql  " & sql & Environment.NewLine &
                " GisUser.nick  " & GisUser.nick & Environment.NewLine &
                " uname  " & uname & Environment.NewLine &
                " uname  " & uname & Environment.NewLine &
              ex.ToString())
        End Try
    End Sub

    Private Shared Function lastrangeDb2Obj(dt As DataTable) As clsRange
        Dim favo As New clsRange
        Try
            l("lastrangeDb2Obj---------------------- anfang")
            favo.xl = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("xl")))
            favo.xh = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("xh")))
            favo.yl = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("yl")))
            favo.yh = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("yh")))
            'favo.aktiv = clsDBtools.fieldvalue(dt.Rows(0).Item("aktiv"))
            Return favo
            l("lastrangeDb2Obj---------------------- ende")
        Catch ex As Exception
            l("Fehler in lastrangeDb2Obj: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Private Shared Sub lastrangeUpdateDB(darange As clsRange)
        Dim result As String = "", hinweis As String = "", imarkt As String = "", schema As String = ""
        Try
            l("lastrangeUpdateDB---------------------- anfang")
            imarkt = "1"
            schema = If(iminternet, "externparadigma", "public")
            If darange Is Nothing Then
                l("fehler in lastrangeUpdateDB a: is nothing")
            End If
            If Not darange.istBrauchbar Then
                l("fehler in lastrangeUpdateDB b:   istBrauchbar false")
            End If
            webgisREC.mydb.SQL = "update " & schema & ".lastrange set" &
                                        " xl=" & CInt(darange.xl) & "" &
                                        ",xh=" & CInt(darange.xh) & "" &
                                        ",yl=" & CInt(darange.yl) & "" &
                                        ",yh=" & CInt(darange.yh) & "" &
                                        ",ts='" & DateTime.Now & "'" &
                                        " where lower(username)='" & GisUser.nick.ToLower.Trim & "'"
            imarkt = "2"
            'If iminternet Or CGIstattDBzugriff Then!
            result = clsToolsAllg.getSQL4Http(webgisREC.mydb.SQL, "webgiscontrol", hinweis, "putsql", 0) : l(hinweis)
            imarkt = "2a"
            Exit Sub '!!!!!!!!!!!!!!!!!!!!!!!!
            If result Is Nothing Then
                Exit Sub
            End If
            If result.IsNothingOrEmpty Then
                'l("fehler in lastrangeUpdateDB b:   istBrauchbar false")
            Else
                result = result.Replace("$", "").Replace(vbCrLf, "")
                imarkt = "2b"
            End If
            'Else
            '    res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
            'End If
            imarkt = "3"
            l("lastrangeUpdateDB---------------------- ende")
        Catch ex As Exception
            l("Fehler in lastrangeUpdateDB c: " & imarkt & Environment.NewLine &
                result & Environment.NewLine &
                   hinweis & Environment.NewLine &
                schema & Environment.NewLine &
                webgisREC.mydb.SQL & Environment.NewLine &
              ex.ToString())
        End Try
    End Sub

    Private Shared Sub lastrangeInsertDB(darange As clsRange)
        Dim result As String = "", hinweis As String = ""
        Try
            l("lastrangeInsertDB---------------------- anfang")
            Dim schema As String = If(iminternet, "externparadigma", "public")
            webgisREC.mydb.SQL = "INSERT INTO " & schema & ".lastrange" &
                        " (username,xl,xh,yl,yh,ts)" &
                         " VALUES('" &
                           GisUser.nick.ToLower.Trim & "'," & CInt(darange.xl) & "," &
                        CInt(darange.xh) & "," & CInt(darange.yl) & "," & CInt(darange.yh) & ",'" &
                        DateTime.Now & "')"
            'If iminternet Or CGIstattDBzugriff Then
            result = clsToolsAllg.getSQL4Http(webgisREC.mydb.SQL, "webgiscontrol", hinweis, "putsql") : l(hinweis)
            result = result.Replace("$", "").Replace(vbCrLf, "")
            If result.IsNothingOrEmpty Then

            Else

            End If
            'Else
            '    res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
            'End If
            l(" lastrangeInsertDB---------------------- ende" & webgisREC.mydb.SQL)
        Catch ex As Exception
            l("Fehler in lastrangeInsertDB: " & ex.ToString())
        End Try
    End Sub

    Friend Shared Function lastrangeLadenDB(uname As String) As clsRange
        Dim lastrangeaktaltesObjekt As New clsRange
        Dim sql As String = "", result As String = "", hinweis As String = ""
        Dim schema As String
        Dim count As String = "0"
        Try
            l("lastrangeLaden---------------------------")
            schema = If(iminternet, "externparadigma", "public")

            sql = "select * from " & schema & ".lastrange where lower(trim(username))='" & uname.ToLower.Trim & "'  "
            If iminternet Or CGIstattDBzugriff Then
                result = clsToolsAllg.getSQL4Http(sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
                count = "1"

                If result.IsNothingOrEmpty Then
                    'lastrangeaktaltesObjekt
                    Return Nothing
                Else
                    result = result.Trim
                    count = "2"
                    result = result.Replace("$", "").Replace(vbCrLf, "")
                    count = "3"
                    lastrangeaktaltesObjekt = dt4ajax(result)
                    count = "4"
                End If
                Return lastrangeaktaltesObjekt
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(sql, "webgiscontrol")
                'Dim dt As System.Data.DataTable = clsWebgisPGtools.holeDTfromWebgisControl(sql)
                Dim istschonvorhanden As Boolean = clsWebgisPGtools.hatRecords(dt)
                l("lastrangeLaden istschonvorhanden " & istschonvorhanden)
                If istschonvorhanden Then
                    lastrangeaktaltesObjekt = lastrangeDb2Obj(dt)
                    l("lastrangeLaden true")
                    Return lastrangeaktaltesObjekt
                Else
                    l("lastrangeLaden false")
                    Return lastrangeaktaltesObjekt
                End If
            End If

            'Else

            'End If
        Catch ex As Exception
            l("fehler in lastrangeLaden   . " & count & "_" & sql & Environment.NewLine &
               "result: " & result & "<" & Environment.NewLine &
               schema & Environment.NewLine, ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function dt4ajax(result As String) As clsRange
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim oldname As String = ""
        Dim favo As New clsRange
        Try
            l(" dt4ajax html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in dt4ajax: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            favo.xl = CDbl(clsDBtools.fieldvalue((b(1))))
            favo.xh = CDbl(clsDBtools.fieldvalue((b(2))))
            favo.yl = CDbl(clsDBtools.fieldvalue((b(3))))
            favo.yh = CDbl(clsDBtools.fieldvalue((b(4))))
            Return favo
            l(" dt4ajax ---------------------- ende")
        Catch ex As Exception
            l("Fehler in dt4ajax: " & ex.ToString())
            Return Nothing
        End Try
    End Function
End Class
