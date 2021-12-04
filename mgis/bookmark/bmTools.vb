Imports mgis

Module bmTools
    Friend Function bildebookMark(titel As String, cbFreigabefuerGruppe As Boolean) As clsBookmark
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
            l("Fehler in bildebookMark: " & ex.ToString())
            MsgBox("Fehler in bildebookMark: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Function BMSaveInsert(bm As clsBookmark, ByRef newid As Long) As Boolean
        Try
            l("bmInsertDB---------------------- anfang")
            Dim res As Long
            Dim gruppe As String
            gruppe = clsString.normalize(bm.user.ADgruppenname.ToLower.Trim)

            webgisREC.mydb.SQL = "INSERT INTO  public.bookmarks " &
                        "(username,user_gruppe,titel,vorhanden,gecheckt,hgrund,aktiv,ts,xl,xh,yl,yh,free4mygruppe) " &
                         "VALUES('" &
                        bm.user.username.ToLower.Trim & "','" & gruppe & "','" &
                        bm.titel & "','" & bm.fav.vorhanden & "','" & bm.fav.gecheckted & "','" &
                          bm.fav.hgrund & "','" & bm.fav.aktiv & "','" &
                        DateTime.Now & "'," &
                         CInt(bm.range.xl) & "," &
                        CInt(bm.range.xh) & "," & CInt(bm.range.yl) & "," & CInt(bm.range.yh) & "," & CBool(bm.free4mygruppe) &
            ") RETURNING id " '
            res = webgisREC.sqlexecute(newid)
            l(webgisREC.hinweis)
            If newid < 1 Then
                Return False
            Else
                Return True
            End If
            l(" bmInsertDB---------------------- ende" & webgisREC.mydb.SQL)
        Catch ex As Exception
            l("Fehler in bmInsertDB: " & ex.ToString())
            MsgBox("Fehler in bmInsertDB: " & ex.ToString())
            Return False
        End Try
    End Function

    Friend Function btnBMloeschen(bm As clsBookmark) As Boolean
        Try
            Dim newid As Long
            Dim res As Long
            l("btnBMloeschen---------------------- anfang")
            webgisREC.mydb.SQL = "delete from public.bookmarks where id=" & bm.id
            res = webgisREC.sqlexecute(newid)
            l(webgisREC.hinweis)
            If res < 1 Then
                Return False
            Else
                Return True
            End If
            l("-btnBMloeschen--------------------- ende")
        Catch ex As Exception
            l("Fehler in btnBMloeschen : " & ex.ToString())
            Return False
        End Try
    End Function

    Friend Function BMsaveedit(bm As clsBookmark) As Boolean
        Dim newid As Long
        Dim res As Long
        Try
            l("saveedit---------------------- anfang")
            webgisREC.mydb.SQL = "update public.bookmarks set titel='" & bm.titel.Trim & "'," &
                "free4mygruppe=" & CBool(bm.free4mygruppe) &
                " where id=" & bm.id
            res = webgisREC.sqlexecute(newid) : l(webgisREC.hinweis)
            If res < 1 Then
                Return False
            Else
                Return True
            End If
            l(" saveedit---------------------- ende" & webgisREC.mydb.SQL)
            Return True
            l("saveedit---------------------- ende")
        Catch ex As Exception
            l("Fehler in saveedit: " & ex.ToString())
            Return False
        End Try
    End Function
End Module
