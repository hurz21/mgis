Imports System.Data
Imports Npgsql
Module modDBgrab
    Property myconn As New Npgsql.NpgsqlConnection
    Private _mycount As Integer
    Friend Function putsql(sql As String, dbname As String) As String
        Try
            l(" MOD putsql anfang")
            l("1sql " & sql)
            Dim result As String
            Dim anzahltreffer As Long
            Dim newid As Integer = 0
            sql = sql.Replace("#", " ")
            l("dbname " & dbname)
            Dim com As NpgsqlCommand
            l("vor conn!")
            getconnection(dbname)
            l("vor open!")
            myconn.Open()

            l("Sql b: " & sql)
            com = New NpgsqlCommand(sql, myconn)
            anzahltreffer = CInt(com.ExecuteNonQuery)
            l("anzahlTreffer: " & anzahltreffer)
            com.Dispose()
            myconn.Close()
            l("fertig")
            Return anzahltreffer.ToString
        Catch pex As NpgsqlException
            l("putsql " & pex.ToString)
            'l("newid:" & newid)
            Return "putsql " & pex.ToString
        Catch ex As Exception
            l("putsql" & ex.ToString)
            Return "putsql " & ex.ToString
        End Try
    End Function



    Public Function dbgrabsimple(sql As String, iminternet As Boolean, dbasename As String) As String
        l("1dbgrabMain " & sql)
        getconnection(dbasename)
        myconn.Open()
        l("db open erfolgreich")

        Dim result As String = ""

        'If iminternet Then
        '    sql = sql & " and  aid in (select aid from gruppe2aid where internet=true)"
        'Else
        '    sql = sql & " and  aid in (select aid from gruppe2aid where intranet=true)"
        'End If
        l("sql:" & sql)
        If grabDataTable(sql) Then
            l("dt.count " & dt.Rows.Count)
            result = concatResultstring("#", "$")
            l("dbgrabsimple " & result)
        End If
        myconn.Close() : myconn.Dispose()
        l("fertig")
        Return result
    End Function
    Public Function dbgrabMain(viewname As String, aidstring As String, iminternet As Boolean) As String
        l("1dbgrabMain " & aidstring)
        getconnection("webgiscontrol")
        myconn.Open()
        l("db open erfolgreich")
        Dim sql As String
        Dim result As String = ""
        If aidstring = String.Empty Then
            'intranet
            sql = "select * from " & viewname & "  where status=true order by titel"
        Else
            'internet
            sql = "select * from " & viewname & aidstring & "  and status=true order by titel"
        End If


        l("sql:" & sql)
        If grabDataTable(sql) Then
            result = concatResultstring("#", "$")
            l(result)
        End If
        myconn.Close() : myconn.Dispose()
        l("fertig")
        Return result
    End Function
    Public Function dbgrabMain2(sql As String) As String
        l("dbgrabMain2 ")
        getconnection("webgiscontrol")
        myconn.Open()
        l("db open erfolgreich")

        Dim result As String = ""



        l("sql:" & sql)
        If grabDataTable(sql) Then
            result = concatResultstring("#", "$")
            l(result)
        End If
        myconn.Close() : myconn.Dispose()
        l("fertig")
        Return result
    End Function
    Public Function dbgrabMain(sql As String) As String
        l("1dbgrabMain " & sql)
        getconnection("webgiscontrol")
        myconn.Open()
        l("db open erfolgreich")

        Dim result As String = ""

        l("sql:" & sql)
        If grabDataTable(sql) Then
            result = concatResultstring("#", "$")
            l(result)
        End If
        myconn.Close() : myconn.Dispose()
        l("fertig")
        Return result
    End Function



    Private dt As DataTable
    Private anzahlTreffer As Integer
    Private newid As Long

    Public Sub getconnection(dbasename As String)
        l("getconnection")
        Dim cbl As New Npgsql.NpgsqlConnectionStringBuilder
#If DEBUG Then
        cbl.Host = "w2gis02"
#Else
        cbl.Host = "localhost"
#End If
        l(cbl.Host)
        cbl.Database = dbasename '"webgiscontrol"
        l(cbl.Database)
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
        l(cbl.ConnectionString)
        myconn = New NpgsqlConnection(cbl.ConnectionString)

    End Sub

    Friend Function userexists(nick As String, pwmd As String, machinename As String,
                               domainname As String, cpuID As String, macadress As String,
                               iminternet As Boolean, ByRef rites As String) As Boolean
        Dim result As Boolean = False
        Try
            l(" MOD userexists anfang")
            getconnection("webgiscontrol")
            myconn.Open()
            l("db open erfolgreich")
            Dim sql As String
            Dim schema As String = If(iminternet, "externparadigma", "public")
            If iminternet Then
                sql = "select * from " & schema & ".nutzer where lower(nick)='" & nick.ToLower &
                    "' and lower(machinename)='" & machinename.ToLower & "'"
                sql = "select * from " & schema & ".nutzer where lower(nick)='" & nick.ToLower & "'"
            Else
                sql = "select * from " & schema & ".nutzer where lower(nick)='" & nick.ToLower & "'"
            End If
            l("sql:" & sql)
            If grabDataTable(sql) Then
                If dt.Rows.Count > 0 Then
                    rites = clsDBtools.fieldvalue(dt.Rows(0).Item("pruef"))
                    result = True
                Else
                    rites = ""
                    result = False
                End If
            Else
                rites = ""
                result = False
            End If
            l(result.ToString)
            myconn.Close() : myconn.Dispose()
            l("fertig")
            Return result
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
            rites = ""
            Return False
        End Try
    End Function

    Friend Function favoritExists(nick As String, gruppe As String, iminternet As Boolean) As Boolean
        Try
            l(" MOD ---------------------- anfang")
            l(" MOD favoritExists anfang")
            getconnection("webgiscontrol")
            myconn.Open()
            l("db open erfolgreich")
            Dim sql As String
            Dim schema As String = If(iminternet, "externparadigma", "public")
            If iminternet Then
                sql = "select * from " & schema & ".favoriten where username='" &
                        nick.ToLower.Trim & "' and gruppe='" & gruppe.Trim.ToLower & "'"
            Else
                'sql = "select * from " & schema & ".nutzer where lower(nick)='" & nick.ToLower & "'"

                sql = "select * from " & schema & ".favoriten where username='" &
                  nick.ToLower.Trim & "' and gruppe='" & gruppe.Trim.ToLower & "'"
            End If
            l("sql:" & sql)
            If grabDataTable(sql) Then
                l("dt.Rows.Count:" & dt.Rows.Count)
                If dt.Rows.Count > 0 Then
                    myconn.Close() : myconn.Dispose()
                    Return True
                Else
                    myconn.Close() : myconn.Dispose()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            l("Fehler in favoritExists: " & ex.ToString())
            Return False
        End Try
    End Function

    Friend Sub favoritInsert(nick As String, gruppe As String, iminternet As Boolean, titel As String, vorhanden As String, gecheckted As String, hgrund As String, aktiv As String, ts As String)
        Dim sql As String
        Try
            l(" MOD favoritInsert anfang")
            getconnection("webgiscontrol")
            myconn.Open()
            l("db open erfolgreich")

            Dim schema As String = If(iminternet, "externparadigma", "public")
            If iminternet Then
                sql = "INSERT INTO " & schema & ".favoriten " &
                                            "(username,gruppe,titel,vorhanden,gecheckt,hgrund,aktiv,ts) " &
                                            "VALUES('" &
                                            nick.ToLower.Trim & "','" & gruppe.ToLower.Trim & "','" &
                                            titel & "','" & vorhanden & "','" & gecheckted & "','" &
                                            hgrund & "','" & aktiv & "','" &
                                            DateTime.Now & "')  "
            Else
                sql = "INSERT INTO " & schema & ".favoriten " &
                                            "(username,gruppe,titel,vorhanden,gecheckt,hgrund,aktiv,ts) " &
                                            "VALUES('" &
                                            nick.ToLower.Trim & "','" & gruppe.ToLower.Trim & "','" &
                                            titel & "','" & vorhanden & "','" & gecheckted & "','" &
                                            hgrund & "','" & aktiv & "','" &
                                            DateTime.Now & "')  "
            End If
            l("sql:" & sql)

            Dim com As NpgsqlCommand
            getconnection("webgiscontrol")
            myconn.Open()

            'sql = "  (aid," + colname & ",ist_Standard) values (" & CInt(aid) & ", " & hochkomma & neuerwert & hochkomma & "," & ist_Standard & ")"
            'sql = String.Format("insert into {0}{1}", tabelle, sql)
            l("Sql: " & sql)
            com = New NpgsqlCommand(sql, myconn)
            anzahlTreffer& = CInt(com.ExecuteNonQuery)
            l("anzahlTreffer: " & anzahlTreffer)
            com.Dispose()


            myconn.Close() : myconn.Dispose()


        Catch ex As Exception
            l("Fehler in favoritInsert: " & ex.ToString())
        End Try
    End Sub

    Friend Sub favoritUpdate(nick As String, gruppe As String, iminternet As Boolean, titel As String, vorhanden As String, gecheckted As String, hgrund As String, aktiv As String, ts As String)
        Try
            l(" MOD favoritUpdate anfang")
            l(" MOD favoritUpdate anfang")
            getconnection("webgiscontrol")
            myconn.Open()
            l("db open erfolgreich")
            Dim sql As String
            Dim schema As String = If(iminternet, "externparadigma", "public")
            If iminternet Then
                sql = "update " & schema & ".favoriten set " &
                  "titel ='" & titel & "'" &
                  ",vorhanden ='" & vorhanden & "'" &
                  ",gecheckt ='" & gecheckted & "'" &
                  ",hgrund ='" & hgrund & "'" &
                  ",aktiv ='" & aktiv & "'" &
                  ",ts ='" & DateTime.Now & "'" &
                  " where lower(username)='" & nick.ToLower.Trim & "'" &
                  " and lower(gruppe)='" & gruppe.ToLower.Trim & "'"
            Else


                sql = "update " & schema & ".favoriten set " &
                  "titel ='" & titel & "'" &
                  ",vorhanden ='" & vorhanden & "'" &
                  ",gecheckt ='" & gecheckted & "'" &
                  ",hgrund ='" & hgrund & "'" &
                  ",aktiv ='" & aktiv & "'" &
                  ",ts ='" & DateTime.Now & "'" &
                  " where lower(username)='" & nick.ToLower.Trim & "'" &
                  " and lower(gruppe)='" & gruppe.ToLower.Trim & "'"



            End If
            l("sql:" & sql)
            If grabDataTable(sql) Then
                If dt.Rows.Count > 0 Then
                    myconn.Close() : myconn.Dispose()

                Else
                    myconn.Close() : myconn.Dispose()

                End If
            End If

        Catch ex As Exception
            l("Fehler in favoritUpdate: " & ex.ToString())

        End Try
    End Sub

    Friend Function usercreate(nick As String, pw As String, iminternet As Boolean, machinename As String, domain As String,
                               cpuid As String, macadress As String) As Boolean
        Try
            l(" MOD usercreate anfang")
            Dim com As NpgsqlCommand
            getconnection("webgiscontrol")
            myconn.Open()
            Dim Sql As String
            Dim schema As String = If(iminternet, "externparadigma", "public")
            Sql = "  (name,pwmd,nick,machinename,domain,cpuid,macadress) values ('" &
                nick & "','" & pw & "','" & nick & "','" & machinename & "','" & domain & "','" & cpuid & "','" & macadress & "')"
            Sql = String.Format("insert into  " & schema & ".nutzer " & Sql)
            l("Sql: " & Sql)
            com = New NpgsqlCommand(Sql, myconn)
            anzahlTreffer& = CInt(com.ExecuteNonQuery)
            l("anzahlTreffer: " & anzahlTreffer)
            com.Dispose()
            myconn.Close()
            l(" MOD usercreate ende")
            Return True
        Catch ex As Exception
            l("Fehler in usercreate: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Function grabDataTable(sql As String) As Boolean
        Dim com As New NpgsqlCommand(sql, myconn)
        Dim da As New NpgsqlDataAdapter(com)
        Try
            dt = New DataTable
            _mycount = da.Fill(dt)
            Return True
        Catch ex As Exception
            l("fehler in " & ex.ToString)
            Return False
        End Try
    End Function

    Private Function concatResultstring(trennCol As String, trennRow As String) As String
        Dim sb As New Text.StringBuilder()
        l("concatResultstring-------------- " & trennCol & "-")
        l((dt.Rows.Count - 1) & "/" & (dt.Columns.Count - 1))
        Dim t As String = ""
        For i = 0 To dt.Rows.Count - 1
            '  l("zeile" & i)
            For j = 0 To dt.Columns.Count - 1
                t = Trim(CStr(clsDBtools.fieldvalue2(dt.Rows(i).Item(j))))
                'If t = "274" Then
                '    Debug.Print("")
                'End If
                If j = dt.Columns.Count - 1 Then
                    sb.Append(t & trennRow)
                Else
                    sb.Append(t & trennCol)
                End If
            Next
        Next
        l("concatResultstring--------------Ende")
        Return sb.ToString
    End Function
    Function dbsetEbenenZuSachgebieteValue(tabelle As String, colname As String, aid As String, neuerwert As String, aidstring As String,
                                           ist_Standard As String, iminternet As Boolean) As Boolean
        l("dbsetEbenenZuSachgebieteValue-----------------")
        l("tabelle " + tabelle)
        l("colname " + colname)
        l("aid " + aid)
        l("neuerwert " + neuerwert)
        l("aidstring " + aidstring)
        l("ist_Standard= " + ist_Standard)
        Try
            Dim hochkomma As String = "'"
            '    Dim da As New NpgsqlDataAdapter(com)
            If IsNumeric(neuerwert.Trim) Then
                l("--numerisch")
                hochkomma = " "
            Else
                l("--nicht numerisch")
                hochkomma = "'"
            End If
            If ist_Standard.ToLower = "true" Then
                Dim existiert As Boolean
                existiert = recordExists(tabelle, aid, aidstring, iminternet)
                If existiert Then
                    'Update ausführen
                    l("muss alten record löschen")
                    If deleteEbenenZusachgebiete(tabelle, colname, aid, neuerwert, hochkomma, aidstring, ist_Standard) Then
                        If InsertByAidTabInEbeneZuSachgebiete(tabelle, colname, aid, neuerwert, hochkomma, ist_Standard) Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                Else
                    l("muss inserten")
                    'insert record anlegen
                    If InsertByAidTabInEbeneZuSachgebiete(tabelle, colname, aid, neuerwert, hochkomma, ist_Standard) Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End If
            If ist_Standard.ToLower = "false" Then
                'Dim existiert As Boolean
                'existiert = recordExists(tabelle, aid, aidstring)
                'If existiert Then
                '    'Update ausführen
                '    l("muss updaten")
                '    If UpdateEbenenZusachgebiete(tabelle, colname, aid, neuerwert, hochkomma, aidstring, ist_Standard) Then
                '        Return True
                '    Else
                '        Return False
                '    End If
                'Else
                l("muss inserten")
                'insert record anlegen
                If InsertByAidTabInEbeneZuSachgebiete(tabelle, colname, aid, neuerwert, hochkomma, ist_Standard) Then
                    Return True
                Else
                    Return False
                End If
                'End If
            End If

            Return False
        Catch pex As NpgsqlException
            l(pex.ToString)
            l("newid:" & newid)
            Return False
        Catch ex As Exception
            l(ex.ToString)
            Return False
        End Try
    End Function
    Function dbsetvalue(tabelle As String, colname As String, aid As String, neuerwert As String, aidstring As String, iminternet As Boolean) As Boolean
        l("dbsetvalue-----------------")
        l("tabelle " + tabelle)
        l("colname " + colname)
        l("aid " + aid)
        l("neuerwert " + neuerwert)
        l("aidstring " + aidstring)
        Try
            Dim hochkomma As String = "'"
            '    Dim da As New NpgsqlDataAdapter(com)
            If IsNumeric(neuerwert.Trim) Then
                l("--numerisch")
                hochkomma = " "
            Else
                l("--nicht numerisch")
                hochkomma = "'"
            End If
            Dim existiert As Boolean
            existiert = recordExists(tabelle, aid, aidstring, iminternet)
            If existiert Then
                'Update ausführen
                l("muss updaten")
                If UpdateByAidTab(tabelle, colname, aid, neuerwert, hochkomma, aidstring) Then
                    Return True
                Else
                    Return False
                End If
            Else
                l("muss inserten")
                'insert record anlegen
                If InsertByAidTab(tabelle, colname, aid, neuerwert, hochkomma) Then
                    Return True
                Else
                    Return False
                End If
            End If
            Return False
        Catch pex As NpgsqlException
            l(pex.ToString)
            l("newid:" & newid)
            Return False
        Catch ex As Exception
            l(ex.ToString)
            Return False
        End Try
    End Function
    Private Function deleteEbenenZusachgebiete(tabelle As String, colname As String, aid As String, neuerwert As String, hochkomma As String,
                                               aidstring As String, iststandard As String) As Boolean
        Try
            l("deleteEbenenZusachgebiete-----------------------------")
            Dim com As NpgsqlCommand
            getconnection("webgiscontrol")
            myconn.Open()
            Dim Sql As String
            Sql = " delete from " & tabelle & " where aid=" + aid & " and ist_standard=true"
            'Sql = " set " & colname & "=" & hochkomma & neuerwert & hochkomma & ",ist_standard=" & iststandard & aidstring
            'Sql = String.Format("update {0}{1}", tabelle, Sql)
            l("Sql: " & Sql)
            com = New NpgsqlCommand(Sql, myconn)
            anzahlTreffer& = CInt(com.ExecuteNonQuery)
            l("anzahlTreffer: " & anzahlTreffer)
            com.Dispose()
            myconn.Close()
            Return True
        Catch pex As NpgsqlException
            l("deleteEbenenZusachgebiete " & pex.ToString)
            l("newid:" & newid)
            Return False
        Catch ex As Exception
            l("deleteEbenenZusachgebiete" & ex.ToString)
            Return False
        End Try
    End Function
    Private Function UpdateByAidTab(tabelle As String, colname As String, aid As String, neuerwert As String, hochkomma As String, aidstring As String) As Boolean
        Try
            Dim com As NpgsqlCommand
            getconnection("webgiscontrol")
            myconn.Open()
            Dim Sql As String
            Sql = " set aid=" & aid & ", " & colname & "=" & hochkomma & neuerwert & hochkomma & " " & aidstring
            Sql = String.Format("update {0}{1}", tabelle, Sql)
            l("Sql: " & Sql)
            com = New NpgsqlCommand(Sql, myconn)
            anzahlTreffer& = CInt(com.ExecuteNonQuery)
            l("anzahlTreffer: " & anzahlTreffer)
            com.Dispose()
            myconn.Close()
            Return True
        Catch pex As NpgsqlException
            l("UpdateByAidTab " & pex.ToString)
            l("newid:" & newid)
            Return False
        Catch ex As Exception
            l("UpdateByAidTab" & ex.ToString)
            Return False
        End Try
    End Function

    Private Function InsertByAidTab(tabelle As String, colname As String, aid As String, neuerwert As String,
                                    hochkomma As String) As Boolean
        Try
            Dim com As NpgsqlCommand
            getconnection("webgiscontrol")
            myconn.Open()
            Dim Sql As String
            Sql = "  (aid," + colname & ") values (" & CInt(aid) & ", " & hochkomma & neuerwert & hochkomma & ")"
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


    Private Function InsertByAidTabInEbeneZuSachgebiete(tabelle As String, colname As String, aid As String, neuerwert As String,
                                    hochkomma As String, ist_Standard As String) As Boolean
        Try
            Dim com As NpgsqlCommand
            getconnection("webgiscontrol")
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

    Function recordExists(tabelle As String, aid As String, aidstring As String, iminternet As Boolean) As Boolean
        Dim result As String
        result = dbgrabMain(tabelle, aidstring, iminternet)
        If result = String.Empty Then
            Return False
        Else
            Return True
        End If
    End Function
    Function dbkillEbenenZuSachgebiet(tabelle As String, colname As String, aid As String, sid As String) As Boolean
        l("dbkillEbenenZuSachgebiet-----------------")
        l("tabelle " + tabelle)
        l("colname " + colname)
        l("aid " + aid)
        l("neuerwert " + sid)
        'l("aidstring " + aidstring)
        'l("ist_Standard= " + ist_Standard)
        Try
            Dim com As NpgsqlCommand
            getconnection("webgiscontrol")
            myconn.Open()
            Dim Sql As String
            Sql = "delete from " & tabelle & " where aid=" & aid & " and sachgebietid=" & sid
            l("Sql: " & Sql)
            com = New NpgsqlCommand(Sql, myconn)
            anzahlTreffer& = CInt(com.ExecuteNonQuery)
            l("anzahlTreffer: " & anzahlTreffer)
            com.Dispose()
            myconn.Close()
            Return True
        Catch pex As NpgsqlException
            l(pex.ToString)
            l("newid:" & newid)
            Return False
        Catch ex As Exception
            l(ex.ToString)
            Return False
        End Try
    End Function
End Module
