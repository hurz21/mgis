Imports System.Data
Imports Npgsql
Module moddb


    Property myconn As New Npgsql.NpgsqlConnection
    Private _mycount As Integer
    Private dt As DataTable

    Sub getconnection()
        Dim cbl As New Npgsql.NpgsqlConnectionStringBuilder
        cbl.Host = "w2gis02"
        cbl.Database = "webgiscontrol"
        ' cbl.t = "flurkarte.basis_f"
        cbl.UserName = "postgres"
        cbl.Password = "lkof4"
        cbl.Port = 5432
        cbl.Pooling = False
        'csb.Protocol = 3'ProtocolVersion.Version3
        cbl.MinPoolSize = 1
        cbl.MaxPoolSize = 20
        'csb.Encoding = 
        cbl.Timeout = 15
        cbl.SslMode = SslMode.Disable
        myconn = New NpgsqlConnection(cbl.ConnectionString)
    End Sub
    Function grabDataTable(sql As String) As Boolean
        l("grabDataTable-----------------------")
        l("sql" & sql)

        l("db open erfolgreich")
        Dim com As New NpgsqlCommand(sql, myconn)
        Dim da As New NpgsqlDataAdapter(com)
        Try
            getconnection()
            myconn.Open()
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            dt = New DataTable
            _mycount = da.Fill(dt)
            myconn.Close() : myconn.Dispose()
            l("grabDataTable erfolgreich")
            Return True
        Catch ex As Exception
            l("fehler in " & ex.ToString)
            Return False
        End Try
    End Function

    Friend Function inkrementTotal(sql As String) As Boolean
        Dim com As NpgsqlCommand
        Dim anzahlTreffer&, newid&
        Try
            l("inkrementTotal-------------------------")
            getconnection()
            myconn.Open()

            l("Sql: " & sql)
            com = New NpgsqlCommand(sql, myconn)
            anzahlTreffer& = CInt(com.ExecuteNonQuery)
            l("anzahlTreffer: " & anzahlTreffer)
            com.Dispose()
            myconn.Close()
            Return True
        Catch pex As NpgsqlException
            l("inkrementTotal " & pex.ToString)
            l("newid:" & newid)
            Return False
        Catch ex As Exception
            l("inkrementTotal" & ex.ToString)
            Return False
        End Try
    End Function

    Public Function dbgrabMain(viewname As String, aidstring As String, iminternet As Boolean) As String
        l("1dbgrabMain " & aidstring)
        getconnection()
        myconn.Open()
        l("db open erfolgreich")
        Dim sql As String
        Dim result As String = ""
        sql = "select * from " & viewname & aidstring & "  and status=true "
        'If iminternet Then
        '    sql = sql & " and  aid in (select aid from gruppe2aid where internet=true)"
        'Else
        '    sql = sql & " and  aid in (select aid from gruppe2aid where intranet=true)"
        'End If
        l("sql:" & sql)
        If grabDataTable(sql) Then
            'result = concatResultstring("#", "$")
            'l(result)
        End If
        myconn.Close() : myconn.Dispose()
        l("fertig")
        Return result
    End Function
    Private Function InsertByAidTabInEbeneZuSachgebiete(tabelle As String, colname As String, aid As String, neuerwert As String,
                                hochkomma As String, ist_Standard As String) As Boolean
        Dim com As NpgsqlCommand
        Dim anzahlTreffer&, newid&
        Try

            getconnection()
            myconn.Open()
            Dim Sql As String
            Sql = "  (aid," + colname & ",ist_Standard) values (" & CInt(aid) & ", " & hochkomma & neuerwert & hochkomma & "," & ist_Standard & ")"
            Sql = String.Format("insert into {0}{1}", tabelle, Sql)
            l("Sql: " & Sql)
            com = New NpgsqlCommand(Sql, myconn)
            anzahlTreffer& = CInt(com.ExecuteNonQuery)
            l("anzahlTreffer: " & anzahlTreffer)
            com.Dispose()
            myconn.Close()
            Return True
        Catch pex As NpgsqlException
            l("InsertByAidTab " & pex.ToString)
            l("newid:" & newid)
            Return False
        Catch ex As Exception
            l("InsertByAidTab" & ex.ToString)
            Return False
        End Try
    End Function
End Module


