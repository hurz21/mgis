Imports System.Data
Imports Npgsql
Imports webgiscontrol

Module modDB
    Property sgColl As New List(Of clsSachgebietsCombo)
    Property sgHauptColl As New List(Of clsSachgebietsCombo)
    Property rangColl As New List(Of clsSachgebietsCombo)
    Property schemaColl As New List(Of clsSchema)
    Property schematabellenColl As New List(Of clsSchemaTabelle)
    Property tabIDColl As New List(Of clsSachgebietsCombo)
    Public myconn As New NpgsqlConnection
    Friend wgisdt As DataTable
    Public dokudt As DataTable



    'Function refreshEbenenListe() As DataTable
    '    Dim dt As New DataTable
    '    makeConnection(tools.dbServername, "webgiscontrol", "postgres", "lkof4", "5432")
    '    myconn.Open()

    '    Dim SQL As String = "SELECt * FROM  ver_stamm"
    '    l(SQL)
    '    Dim com As New NpgsqlCommand(SQL, myconn)
    '    Dim da As New NpgsqlDataAdapter(com)
    '    da.MissingSchemaAction = MissingSchemaAction.AddWithKey
    '    dt = New DataTable
    '    Dim _mycount = da.Fill(dt)
    '    'serial = CStr(dt.Rows(0).Item(0))
    '    myconn.Close()
    '    myconn.Dispose()
    '    com.Dispose()
    '    da.Dispose()
    '    Return dt

    '    l("getSerialFromPostgis fertig")
    'End Function


    Public Sub makeConnection(ByVal host As String, datenbank As String, ByVal dbuser As String, ByVal dbpw As String, ByVal dbport As String)
        Dim csb As New NpgsqlConnectionStringBuilder
        Try
            l("makeConnection")
            csb.Host = host
            csb.UserName = dbuser
            csb.Password = dbpw
            csb.Database = datenbank
            csb.Pooling = False
            csb.MinPoolSize = 1
            csb.MaxPoolSize = 20
            csb.Timeout = 15
            csb.SslMode = SslMode.Disable
            myconn = New NpgsqlConnection(csb.ConnectionString)
            l("makeConnection fertig")
        Catch ex As Exception
            l("fehler in makeConnection" & ex.ToString)
        End Try
    End Sub
    Sub l(text As String)
        Debug.Print(text)
        If text.ToLower.StartsWith("fehler") Then
            MsgBox(text)
        End If

    End Sub
    Function getDT(sql As String, server As String, datenbank As String) As DataTable
        Dim dt As New DataTable
        Try
            l(" MOD getDT anfang")
            makeConnection(server, datenbank, "postgres", "lkof4", "5432")
            myconn.Open()
            ' Dim SQL As String = "SELECt * FROM  ver_stamm"
            l(sql)
            Dim com As New NpgsqlCommand(sql, myconn)
            Dim da As New NpgsqlDataAdapter(com)
            'da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            dt = New DataTable
            Dim _mycount = da.Fill(dt)
            'serial = CStr(dt.Rows(0).Item(0))
            myconn.Close()
            myconn.Dispose()
            com.Dispose()
            da.Dispose()
            Return dt
            l("getDT fertig")

            l(" MOD getDT ende")
        Catch ex As Exception
            l("Fehler in getDT: " & ex.ToString())
        End Try
    End Function



    Sub initSachgebietAuswahlColl()
        sgColl.Clear()
        Dim aktSG As New clsSachgebietsCombo
        aktSG.sid = ""
        aktSG.sachgebiet = "---alle---"
        sgColl.Add(aktSG)
        wgisdt = getDT("SELECt * FROM  sachgebiete where ist_standard=true order by sachgebiet", tools.dbServername, "webgiscontrol")
        For Each item As DataRow In wgisdt.AsEnumerable
            aktSG = New clsSachgebietsCombo
            aktSG.sid = item.Item("sid").ToString
            aktSG.sachgebiet = item.Item("sachgebiet").ToString
            sgColl.Add(aktSG)
        Next
    End Sub



    Friend Function checkAllDoku(dokus As List(Of clsDoku)) As String
        Dim serror As String = ""
        Try
            wgisdt = getDT("SELECt * FROM  " & stamm_tabelle, tools.dbServername, "webgiscontrol")
            For i = 0 To wgisdt.Rows.Count - 1
                Console.WriteLine(wgisdt.Rows(i).Item("aid"))
                If StammAidInDokuvorhanden(CInt(clsDBtools.fieldvalue(wgisdt.Rows(i).Item("aid"))), dokus) Then
                    'ok
                Else
                    serror = serror & Environment.NewLine &
                    "Aid " & CStr(wgisdt.Rows(i).Item("aid")) & " fehlt in der Dokumentation"
                End If
            Next
            Return serror
        Catch ex As Exception
            l("fehler ind checkAllDoku " & ex.ToString)
            Return "fehler"
        End Try
    End Function



    Private Function StammAidInDokuvorhanden(aid As Integer, dokus As List(Of clsDoku)) As Boolean
        Try
            For Each ndok As clsDoku In dokus
                If ndok.aid = aid Then
                    Return True
                End If
            Next
            Return False
        Catch ex As Exception
            l("fehler ind checkAllDoku " & ex.ToString)
            Return False
        End Try
    End Function

    Friend Function makertfDokuLoop(dokus As List(Of clsDoku)) As String
        Try
            Dim titel As String
            Dim newDok As clsDoku
            For Each ndok As clsDoku In dokus
                newDok = New clsDoku
                newDok.aid = ndok.aid
                If ndok.aid = 346 Then
                    Debug.Print("")
                End If
                newDok.aktualitaet = ndok.aktualitaet
                newDok.beschraenkungen = ndok.beschraenkungen
                newDok.datenabgabe = ndok.datenabgabe
                newDok.entstehung = ndok.entstehung
                newDok.inhalt = ndok.inhalt
                newDok.masstab = ndok.masstab
                titel = gettitel(newDok.aid)
                makertfDoku(newDok, titel)
            Next
            Return "alle RTF ok"
        Catch ex As Exception
            l("fehler ind checkAllDoku " & ex.ToString)
            Return "fehler bei rtfs"
        End Try
    End Function

    Private Function gettitel(aid As Integer) As String
        Try


            For i = 0 To wgisdt.Rows.Count - 1
                If aid = CInt(wgisdt.Rows(i).Item("aid")) Then
                    Return CStr(wgisdt.Rows(i).Item("titel"))

                End If
            Next
            Return ""
        Catch ex As Exception
            l("fehler ind gettitel " & ex.ToString)
            Return ""
        End Try
    End Function
    Friend Function checkAllLegende(legenden As List(Of clsLegendenItem)) As String
        Dim serror As String = ""
        Dim icnt As Integer
        Try
            wgisdt = getDT("SELECt * FROM  " & stamm_tabelle, tools.dbServername, "webgiscontrol")
            For i = 0 To wgisdt.Rows.Count - 1
                Console.WriteLine(wgisdt.Rows(i).Item("aid"))
                If StammAidInLegendenvorhanden(CInt(clsDBtools.fieldvalue(wgisdt.Rows(i).Item("aid"))), legenden) Then
                    'ok
                Else
                    icnt += 1
                    serror = serror & Environment.NewLine &
                    "Aid " & CStr(wgisdt.Rows(i).Item("aid")) & " fehlt in der Legende (" & CStr(wgisdt.Rows(i).Item("titel")) & ")"
                End If
            Next
            Return icnt & "fehler insgesamt" & Environment.NewLine &
                serror
        Catch ex As Exception
            l("fehler ind checkAllLegende " & ex.ToString)
            Return "fehler"
        End Try
    End Function




    Friend Function LegendeCollectioneinlesen() As List(Of clsLegendenItem)
        Dim legenden As New List(Of clsLegendenItem)
        Dim newleg As New clsLegendenItem
        Try


            dokudt = getDT("SELECt * FROM  legenden  where aid>0 and nr>0  order by aid,nr", tools.dbServername, "webgiscontrol")
            For i = 0 To dokudt.Rows.Count - 1
                newleg = New clsLegendenItem
                newleg.aid = CInt(CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("aid"))))
                newleg.nr = CInt(CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("nr"))))
                newleg.titel = (CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("titel"))))
                If newleg.aid = 0 OrElse newleg.nr = 0 Then

                Else
                    legenden.Add(newleg)
                End If

                '                titel = gettitel(newleg.aid)
                'makertfDoku(newleg, titel)
            Next
            Return legenden
        Catch ex As Exception
            l("fehler ind checkAllDoku " & ex.ToString)
            Return Nothing
        End Try
    End Function
    Private Function StammAidInLegendenvorhanden(aid As Integer, legenden As List(Of clsLegendenItem)) As Boolean
        Try
            For Each nleg As clsLegendenItem In legenden

                If nleg.aid = aid Then
                    Return True
                End If
            Next
            Return False
        Catch ex As Exception
            l("fehler ind checkAllDoku " & ex.ToString)
            Return False
        End Try
    End Function
    Friend Function DokusColleinlesen() As List(Of clsDoku)
        Dim dokus As New List(Of clsDoku)
        Dim newDok As New clsDoku
        Try
            dokudt = getDT("SELECt * FROM  doku ", tools.dbServername, "webgiscontrol")
            For i = 0 To dokudt.Rows.Count - 1
                newDok = New clsDoku
                newDok.aid = CInt(CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("aid"))))
                newDok.aktualitaet = (CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("aktualitaet"))))
                newDok.beschraenkungen = (CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("beschraenkungen"))))
                newDok.datenabgabe = (CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("datenabgabe"))))
                newDok.entstehung = (CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("entstehung"))))
                newDok.inhalt = (CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("inhalt"))))
                newDok.masstab = (CStr(clsDBtools.fieldvalue(dokudt.Rows(i).Item("masstab"))))
                dokus.Add(newDok)
            Next
            Return dokus
        Catch ex As Exception
            l("fehler ind DokusColleinlesen " & ex.ToString)
            Return Nothing
        End Try
    End Function
    Friend Function makeRTFlegenden(legenden As List(Of clsLegendenItem)) As String
        Try
            Dim titel As String = ""
            Dim sachgebiet, ebenenprosa As String
            Dim newleg As clsLegendenItem
            Dim aktLeg As New List(Of clsLegendenItem)
            Dim oldaid As Integer
            oldaid = legenden(0).aid
            For Each nleg As clsLegendenItem In legenden
                newleg = New clsLegendenItem
                newleg.aid = nleg.aid
                newleg.nr = nleg.nr
                newleg.titel = nleg.titel
                sachgebiet = "boden"
                ebenenprosa = "bodentyp"

                If newleg.aid = oldaid Then
                    aktLeg.Add(newleg)
                Else
                    'ausgeben
                    getStammZuAid(oldaid, sachgebiet, ebenenprosa, titel)
                    titel = htm2cr(titel)
                    rtf.makertflegende(aktLeg, titel, sachgebiet, ebenenprosa, oldaid)
                    oldaid = newleg.aid
                    aktLeg.Clear()
                End If

            Next
            Return "alle RTF ok"
        Catch ex As Exception
            l("fehler ind checkAllDoku " & ex.ToString)
            Return "fehler bei rtfs"
        End Try
    End Function

    Private Sub getStammZuAid(aid As Integer, ByRef sachgebiet As String, ByRef ebenenprosa As String, ByRef titel As String)
        Dim dtstamm As New System.Data.DataTable
        dtstamm = getDT("select * from " & stamm_tabelle & " where aid=" & aid, tools.dbServername, "webgiscontrol")
        sachgebiet = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("stdsg"))
        ebenenprosa = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("ebene"))
        titel = clsDBtools.fieldvalue(dtstamm.Rows(0).Item("titel"))
    End Sub

End Module
