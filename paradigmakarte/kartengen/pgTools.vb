Module pgTools
    Function sqlausfuehren(sql As String) As DataTable
        '  ini_PGREC(tablename)
        'host = "w2gis02" : datenbank = "postgis20" : schema = "flurkarte" : tabelle = "basis_f" : dbuser = "postgres" : dbpw = "lkof4" : dbport = "5432"
        Dim myconn As NpgsqlConnection
        myconn = makeConnection("w2gis02", "postgis20", "postgres", "lkof4", "5432")
        'l("in sqlausfuehren")
        'l(sql)
        Dim tempdt As New DataTable
        Try
            myconn.Open()
            Dim com As New NpgsqlCommand(sql, myconn)
            Dim da As New NpgsqlDataAdapter(com)
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            ' dtRBplus = New DataTable
            Dim _mycount = da.Fill(tempdt)
            myconn.Close()
            myconn.Dispose()
            com.Dispose()
            da.Dispose()
            'l("sqlausfuehren fertig")
            Return tempdt
        Catch ex As Exception
            l("fehler in sqlausfuehren: " & ex.ToString)
            Return Nothing
        End Try
    End Function
    Function makeConnection(ByVal host As String, datenbank As String, ByVal dbuser As String, ByVal dbpw As String, ByVal dbport As String) As NpgsqlConnection
        Dim csb As New NpgsqlConnectionStringBuilder
        Dim myconn As New NpgsqlConnection
        Try
            '  l("makeConnection")
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
            'l("makeConnection fertig " & csb.ConnectionString)
            Return myconn
        Catch ex As Exception
            l("fehler in makeConnection" & ex.ToString)
            Return Nothing
        End Try
    End Function
End Module
