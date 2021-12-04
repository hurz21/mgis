

Module modEinzeln
    Function exekuteEinzelVorgang(vid As Integer, aktbox As clsRange, dbtyp As String) As Integer
        ReDim ebenen(0) : ebenen(0) = vid
        Dim erfolg As Integer : Dim summe As String = ""
        l("exekuteEinzelVorgang--------------------------------------------------")
        glob2.nachricht("point_shpfile_erzeugen ============================================================ vor")
        If tabelle_existiert_schonImSchema() Then
            l("Tabelle existiert schon muss nicht angelegt werden")
            If tabelleTruncaten() Then
                l("tabelleTruncaten erfolgreich")
                erfolg = 1
            Else
                l("tabelleTruncaten nicht erfolgreich. abbruch")
                erfolg = 1
            End If

        Else
            l("Tabelle wird angelegt")
            erfolg = modPG.pgDBtableAnlegen(summe)
        End If

        l("erfolg: " & erfolg)
        l("summe: " & summe)
        If erfolg < 1 Then Return 0
        '  erfolg = pgDBDatenanlegenAnlegen(myglobalz.Oracle_MYDB)
        If erfolg < 1 Then Return 0

        Dim anzPLUS, anzPolygon As Integer
        If dbtyp = "oracle" Then
            'anzPLUS = modOracle.getDTOracle("Select * from raumbezugplus where vorgangsid=" & vid, dtRBplus)

            anzPLUS = clsSQLS.getDTSQLS("Select * from raumbezugplus where vorgangsid=" & vid, dtRBplus)
        End If
        If dbtyp = "sqls" Then anzPLUS = clsSQLS.getDTSQLS("Select * from raumbezugplus where vorgangsid=" & vid, dtRBplus)

        l("anzahl paradigma objekte: " & anzPLUS)
        '
        Dim dtRBpolygon As New DataTable
        'If dbtyp = "oracle" Then
        '    anzPolygon = modOracle.getDTOracle("Select * from raumbezug2geopolygon where vorgangsid=" & vid, dtRBpolygon)

        'End If
        If dbtyp = "sqls" Then anzPolygon = clsSQLS.getDTSQLS("Select * from raumbezug2geopolygon where vorgangsid=" & vid, dtRBpolygon)
        l("anzahl paradigma polygone: " & anzPolygon)
        Dim returnstring As String = ""
        modPG.doRBschleife(dtRBplus, dtRBpolygon, "", returnstring)
        l(returnstring)
        glob2.nachricht("point_shpfile_erzeugen ============================================================ ende")
        Return 1
    End Function

    Private Function tabelleTruncaten() As Boolean
        'Postgis_MYDB.ServiceName & "." & Postgis_MYDB.ServiceName & "_" & Postgis_MYDB.Tabelle
        l("tabelleTruncaten----------------------------------------------!")
        Dim sql As String
        sql = "truncate paradigma_userdata." & Postgis_MYDB.Tabelle.Trim.ToLower & ""
        l(sql)
        Dim erfolg As Boolean = CBool(getval(sql, Postgis_MYDB, dtRBplus)) '-1 = vorhanden 0 nicht vorhanden
        l("tabelleTruncaten " & erfolg)
        Return erfolg
    End Function

    Private Function tabelle_existiert_schonImSchema() As Boolean
        'Postgis_MYDB.ServiceName & "." & Postgis_MYDB.ServiceName & "_" & Postgis_MYDB.Tabelle
        l("tabelle_existiert_schonImSchema----------------------------------------------!")
        Dim sql = "SELECT EXISTS (" &
   " SELECT 1 " &
   " FROM   pg_catalog.pg_class c" &
   " JOIN   pg_catalog.pg_namespace n ON n.oid = c.relnamespace " &
   " WHERE  n.nspname = 'paradigma_userdata' " &
   " AND    c.relname = '" & Postgis_MYDB.Tabelle & "' " &
   " And    c.relkind = 'r'   " &
   ");"
        sql = "SELECT EXISTS(        SELECT *    FROM information_schema.tables   " &
            " WHERE    table_schema = 'paradigma_userdata' AND     " &
            "   table_name = '" & Postgis_MYDB.Tabelle.Trim.ToLower & "')"
        l(sql)
        Dim erfolg As Boolean = CBool(getval(sql, Postgis_MYDB, dtRBplus)) '-1 = vorhanden 0 nicht vorhanden
        Return erfolg
    End Function

    Friend Sub Testeo()
        Dim datei As String
        Try

            'datei = "\\file-paradigma\paradigma\test\kkkk" & Environment.UserName & ".txt"
            datei = "j:\test\kkkk" & Environment.UserName & ".txt"
            Using jjjj As New IO.StreamWriter(datei)
                jjjj.WriteLine("bla")
            End Using
        Catch ex As Exception
            l("Testeo " & ex.ToString)
        End Try
    End Sub


End Module
