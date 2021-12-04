Module modOracle

    'Function getDTOracle(sql As String, ByRef tempDT As DataTable) As Integer
    '    l("in getRaumbezugPlusDT -------------------------------------")
    '    l(sql)
    '    Dim myoracle As Oracle.OracleConnection
    '    ' Dim com As Devart.Data.Oracle.OracleCommand
    '    Dim _mycount As Long
    '    host = "ora-clu-vip-003"
    '    schema = "paradigma"
    '    Dim ServiceName As String = "paradigma.kreis-of.local"
    '    dbuser = "paradigma"
    '    dbpw = "luftikus12"
    '    Try
    '        myoracle = New Devart.Data.Oracle.OracleConnection("Data Source=(DESCRIPTION=" &
    '                            "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" & host & ")(PORT=1521)))" &
    '                             "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=ora-clu-vip-004)(PORT=1521)))" &
    '                            "(LOAD_BALANCE=yes)(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" & ServiceName & ")));" &
    '                            "User Id=" & dbuser & ";Password=" & dbpw & ";")


    '        myoracle.Open()
    '        Dim com As New OracleCommand(sql, myoracle)
    '        Dim da As New OracleDataAdapter(com)
    '        da.MissingSchemaAction = MissingSchemaAction.AddWithKey
    '        tempDT = New DataTable
    '        _mycount = da.Fill(tempDT)
    '        myoracle.Close()
    '        com.Dispose()
    '        da.Dispose()
    '        Return CInt(_mycount)
    '    Catch oex As OracleException
    '        nachricht("Fehler in getRaumbezugPlusDT&:" & oex.ToString & " / " & sql)
    '        Return -1
    '    Catch ex As Exception
    '        nachricht("Fehler in getRaumbezugPlusDT&:" & ex.ToString & " / " & sql)
    '        Return -2
    '    End Try
    '    myoracle.Close()
    'End Function
    Function getserial4RID(rid As String) As String
        Dim serialDT As New DataTable
        Try
            l("getserial4RID -------------------")
            'modOracle.getDTOracle("Select serialshape from raumbezug2geopolygon where raumbezugsid=" & rid, serialDT)
            Dim anz As Integer
            Dim sql = "Select serialshape from raumbezug2geopolygon where raumbezugsid=" & rid
            l("sql: " & sql)
            anz = clsSQLS.getDTSQLS(sql, serialDT)
            l("getserial4RID: " & CStr(serialDT.Rows.Count))
            Return CStr(serialDT.Rows(0).Item("serialshape"))
        Catch ex As Exception
            l("fehler in getserial4RID: " & ex.ToString)
            Return "fehler"
        End Try

    End Function
End Module
