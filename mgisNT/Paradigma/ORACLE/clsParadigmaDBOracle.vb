'Imports System.Data
'Imports Devart.Data.Oracle

'Public Class clsParadigmaDBOracle
'    '  Public Property eigentschnellKonn As New Devart.Common.DbConnectionStringBuilder
'    Public Shared Property connParadigmaDBOracle As OracleConnection = New OracleConnection()
'    Public Property nameundadresse As String
'    Public Property _mycount As Integer
'    Public Property dt As DataTable

'    Public Shared Sub oeffneconnParadigmaDBOracle()
'        Try
'            'Devart.Data.Oracle.OracleException (0x80004005): Server did not respond within the specified timeout interval
'            connParadigmaDBOracle = New OracleConnection("Data Source=(DESCRIPTION=" &
'                                   "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" & "ora-clu-vip-003" &
'                                   ")(PORT=1521)))" &
'                                   "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" & "paradigma.kreis-of.local" &
'                                   ")));" &
'                                   "User Id=" & "paradigma" &
'                                   ";Password=" & "luftikus12" &
'                                   ";direct=yes;")

'            connParadigmaDBOracle.Open()
'        Catch ex As Exception
'            nachricht("Fehler in oeffneConnectionEigentuemer: " & ex.ToString)
'        End Try
'    End Sub
'    Public Shared Sub paradigmaDBschliessen()
'        Try
'            connParadigmaDBOracle.Close()
'        Catch ex As Exception
'            nachricht("fehler in paradigmaDBschliessen:" & ex.ToString)
'        End Try
'    End Sub
'    Shared Function getDt(query As String) As DataTable
'        Dim com As OracleCommand
'        Dim dt As New DataTable
'        Dim _mycount As Integer
'        Try
'            l("in paradigma oracle getDt ")
'            com = New OracleCommand(query, connParadigmaDBOracle)
'            Dim da As New OracleDataAdapter(com)
'            da.MissingSchemaAction = MissingSchemaAction.AddWithKey
'            dt = New DataTable
'            _mycount = da.Fill(dt)
'            If _mycount < 1 Then
'                l("kein treffer")
'                Return dt
'            End If
'            l("in paradigma oracle getDt fertig")
'            Return dt
'        Catch ex As Exception
'            nachricht("fehler in in paradigma oracle getDt:" & ex.ToString)
'            Return Nothing
'        End Try
'    End Function
'    Shared Sub setSQLParamsRBOracle(ByVal com As OracleCommand, ByVal aktrb As iRaumbezug, ByVal rid As Integer)
'        Try
'            l("setSQLParamsRB ---------------------- anfang")
'            com.Parameters.AddWithValue(":TYP", aktrb.typ)
'            com.Parameters.AddWithValue(":SEKID", aktrb.SekID)
'            com.Parameters.AddWithValue(":TITEL", aktrb.name.Trim)
'            com.Parameters.AddWithValue(":ABSTRACT", aktrb.abstract.Trim)
'            com.Parameters.AddWithValue(":RECHTS", CInt(aktrb.punkt.X))
'            com.Parameters.AddWithValue(":HOCH", CInt(aktrb.punkt.Y))
'            com.Parameters.AddWithValue(":XMIN", CInt(aktrb.box.xl))
'            com.Parameters.AddWithValue(":XMAX", CInt(aktrb.box.xh))
'            com.Parameters.AddWithValue(":YMIN", CInt(aktrb.box.yl))
'            com.Parameters.AddWithValue(":YMAX", CInt(aktrb.box.yh))
'            com.Parameters.AddWithValue(":FREITEXT", CStr(aktrb.Freitext).Trim)
'            com.Parameters.AddWithValue(":ISMAPENABLED", Convert.ToInt16(aktrb.isMapEnabled))
'            com.Parameters.AddWithValue(":FLAECHEQM", CInt(aktrb.FLAECHEQM))
'            com.Parameters.AddWithValue(":LAENGEM", CInt(aktrb.LAENGEM))
'            com.Parameters.AddWithValue(":MITETIKETT", CInt(aktrb.MITETIKETT))
'            l("setSQLParamsRB---------------------- ende")
'        Catch ex As Exception
'            l("Fehler in setSQLParamsRB: ", ex)
'        End Try
'    End Sub
'    Shared Function Raumbezug_abspeichern_Neu_alleDBORACLE(aktrb As iRaumbezug) As Integer
'        Dim anzahlTreffer& = 0, hinweis$ = "", newid& = 0
'        Dim com As OracleCommand
'        Try
'            Dim SQLupdate =
'               "INSERT INTO raumbezug (TYP,SEKID,TITEL,ABSTRACT,RECHTS,HOCH," &
'                                      " XMIN,XMAX,YMIN,YMAX,FREITEXT,ISMAPENABLED,FLAECHEQM,LAENGEM,MITETIKETT) " +
'                                      " VALUES (:TYP,:SEKID,:TITEL,:ABSTRACT,:RECHTS,:HOCH," &
'                                      ":XMIN,:XMAX,:YMIN,:YMAX,:FREITEXT,:ISMAPENABLED,:FLAECHEQM,:LAENGEM,:MITETIKETT)"
'            SQLupdate = SQLupdate & " RETURNING RAUMBEZUGSID INTO :R1"

'            nachricht("nach setSQLbody : " & SQLupdate)
'            oeffneconnParadigmaDBOracle()
'            nachricht("nach dboeffnen  ")

'            com = New OracleCommand(SQLupdate$, connParadigmaDBOracle)
'            nachricht("vor setParams  ")
'            setSQLParamsRBOracle(com, aktrb, 0)

'            newid = clsOracleIns.GetNewid(com, SQLupdate)
'            connParadigmaDBOracle.Close()
'            Return clsOracleIns.gebeNeuIDoderFehler(newid, SQLupdate)
'        Catch ex As Exception
'            nachricht("Fehler beim Abspeichern raumbezug: " & ex.ToString)
'            Return -2
'        End Try
'    End Function

'    Public Shared Function Koppelung_Raumbezug_VorgangOracle(ByVal RaumbezugsID As Integer, ByVal vorgangsid As Integer, ByVal status As Integer) As Integer
'        Dim newid& = -1
'        Try
'            If RaumbezugsID > 0 And vorgangsid > 0 Then
'                Dim SQL As String =
'                  "INSERT INTO Raumbezug2Vorgang   " &
'                  " (RAUMBEZUGSID,VORGANGSID,STATUS) VALUES (:RAUMBEZUGSID,:VORGANGSID,:STATUS) " &
'                  " RETURNING ID INTO :R1"
'                Dim com As OracleCommand
'                oeffneconnParadigmaDBOracle()
'                nachricht("nach dboeffnen  ")
'                com = New OracleCommand(SQL, connParadigmaDBOracle)
'                com.Parameters.AddWithValue(":VORGANGSID", vorgangsid)
'                com.Parameters.AddWithValue(":RAUMBEZUGSID", RaumbezugsID)
'                com.Parameters.AddWithValue(":STATUS", status)

'                newid = clsOracleIns.GetNewid(com, SQL)
'                connParadigmaDBOracle.Close()
'                Return clsOracleIns.gebeNeuIDoderFehler(newid, SQL)

'            Else
'                nachricht("Koppelung Koppelung_Vorgang_Raumbezug / person nicht Möglich. wwerte sind 0!!!")
'                Return -3
'            End If
'        Catch ex As Exception
'            nachricht("Koppelung_Vorgang_Raumbezug Problem beim Abspeichern: " &
'                         ex.ToString & vbCrLf)
'            Return -2
'        End Try
'    End Function



'    Shared Sub setSQLParamsFLST_serial(ByVal com As OracleCommand, ByVal vid As Integer, ByVal rbid As Integer, ByVal serial As String, ByVal id As Integer, ByVal Typ As Integer, ByVal areaqm As Double)
'        com.Parameters.AddWithValue(":RAUMBEZUGSID", rbid)
'        com.Parameters.AddWithValue(":VORGANGSID", vid)
'        com.Parameters.AddWithValue(":SERIALSHAPE", serial)
'        com.Parameters.AddWithValue(":TYP", Typ)
'        com.Parameters.AddWithValue(":AREAQM", areaqm)
'        '   com.Parameters.AddWithValue(":ID", id)
'    End Sub
'    Public Shared Function RB_FLST_Serial_abspeichern_NeuORACLE(ByVal vid As Integer,
'                                                   ByVal rbid As Integer,
'                                                   ByVal serial As String,
'                                                   ByVal typ As Integer,
'                                                   ByVal area As Double) As Integer
'        Dim hinweis As String = ""
'        Dim newid As Long = 0
'        Dim com As OracleCommand
'        nachricht("RB_FLST_Serial_abspeichern_Neu -------------------------------------")
'        Try
'            Dim SQLupdate As String = "INSERT INTO RAUMBEZUG2GEOPOLYGON (RAUMBEZUGSID,VORGANGSID,TYP,AREAQM,SERIALSHAPE) " +
'                                         " VALUES (:RAUMBEZUGSID,:VORGANGSID,:TYP,:AREAQM,:SERIALSHAPE)"
'            SQLupdate = SQLupdate & " RETURNING ID INTO :R1"
'            nachricht("nach setSQLbody : " & SQLupdate)
'            oeffneconnParadigmaDBOracle()
'            nachricht("nach dboeffnen  ")
'            com = New OracleCommand(SQLupdate, connParadigmaDBOracle)
'            nachricht("vor setParams  ")
'            setSQLParamsFLST_serial(com, vid, rbid, serial, 0, typ, area)
'            newid = clsOracleIns.GetNewid(com, SQLupdate)
'            connParadigmaDBOracle.Close()
'            Return clsOracleIns.gebeNeuIDoderFehler(newid, SQLupdate)
'        Catch mex As OracleException
'            nachricht("Fehler in RB_FLST_Serial_abspeichern_Neu mex: " & vbCrLf & mex.ToString)
'            Return -2
'        Catch ex As Exception
'            nachricht("Fehler in RB_FLST_Serial_abspeichern_Neu: " & vbCrLf & ex.ToString)
'            Return -2
'        End Try
'    End Function



'    Shared Function RB_Umkreis_abspeichern_NeuOracle(aktpmu As clsParaUmkreis) As Integer
'        Dim anzahlTreffer& = 0, hinweis$ = "", newid& = 0
'        Dim com As OracleCommand
'        Try
'            Dim SQLupdate As String = "INSERT INTO Paraumkreis (RADIUSM,BESCHREIBUNG) " +
'                      " VALUES (:RADIUSM,:BESCHREIBUNG )"
'            SQLupdate$ = SQLupdate$ & " RETURNING ID INTO :R1"

'            nachricht("nach setSQLbody : " & SQLupdate)
'            oeffneconnParadigmaDBOracle()
'            nachricht("nach dboeffnen  ")
'            com = New OracleCommand(SQLupdate$, connParadigmaDBOracle)
'            nachricht("vor setParams  ")
'            setSQLparamsUmkreisRB(com, 0, aktpmu)

'            newid = clsOracleIns.GetNewid(com, SQLupdate)
'            connParadigmaDBOracle.Close()
'            Return clsOracleIns.gebeNeuIDoderFehler(newid, SQLupdate)
'        Catch ex As Exception
'            nachricht("Fehler beim Abspeichern: " & ex.ToString)
'            Return -2
'        End Try
'    End Function

'    Shared Function setsqlbodyUmkreisRB() As String
'        Return " set " &
'         " RADIUSM=:RADIUSM" &
'         ",BESCHREIBUNG=:BESCHREIBUNG"
'    End Function

'    Shared Sub setSQLparamsUmkreisRB(ByVal com As OracleCommand, ByVal sekid%, aktpmu As clsParaUmkreis)
'        '	com = New OracleCommand(myGlobalz.sitzung.tempREC.mydb.SQL, myGlobalz.sitzung.tempREC.myconn)
'        com.Parameters.AddWithValue(":RADIUSM", aktpmu.Radius)
'        com.Parameters.AddWithValue(":BESCHREIBUNG", aktpmu.Name)
'        ' com.Parameters.AddWithValue(":ID", sekid)
'    End Sub

'    Shared Function deleteRaumbezugOracle(rid As Integer, vid As Integer) As Boolean
'        Dim anzahlTreffer& = 0, hinweis$ = "", newid& = 0
'        Dim com As OracleCommand
'        Dim SQLupdate As String
'        Try
'            SQLupdate = "delete from raumbezug where RAUMBEZUGSID=:RAUMBEZUGSID"
'            nachricht("nach setSQLbody : " & SQLupdate)
'            oeffneconnParadigmaDBOracle()
'            nachricht("nach dboeffnen  ")
'            com = New OracleCommand(SQLupdate$, connParadigmaDBOracle)
'            nachricht("vor setParams  ")
'            com.Parameters.AddWithValue(":RAUMBEZUGSID", rid)
'            anzahlTreffer& = CInt(com.ExecuteNonQuery)
'            connParadigmaDBOracle.Close()
'            l("anzahlTreffer& " & anzahlTreffer&)
'            If anzahlTreffer < 1 Then
'                nachricht("Problem beim löschen:" & SQLupdate)
'                Return False
'            Else
'                Return True
'            End If
'        Catch ex As Exception
'            nachricht("Fehler beim Abspeichern: " & ex.ToString)
'            Return False
'        End Try
'    End Function

'    Shared Function deleteRaumbezug2allOracle(rid As Integer, vid As Integer, tablename As String) As Boolean
'        Dim anzahlTreffer& = 0, hinweis$ = "", newid& = 0
'        Dim com As OracleCommand
'        Dim SQLupdate As String
'        Try
'            SQLupdate = "delete from " & tablename & " where RAUMBEZUGSID=:RAUMBEZUGSID and VORGANGSID=:VORGANGSID"
'            nachricht("nach setSQLbody : " & SQLupdate)
'            oeffneconnParadigmaDBOracle()
'            nachricht("nach dboeffnen  ")
'            com = New OracleCommand(SQLupdate$, connParadigmaDBOracle)
'            nachricht("vor setParams  ")
'            com.Parameters.AddWithValue(":RAUMBEZUGSID", rid)
'            com.Parameters.AddWithValue(":VORGANGSID", vid)
'            anzahlTreffer& = CInt(com.ExecuteNonQuery)
'            connParadigmaDBOracle.Close()
'            l("anzahlTreffer& " & anzahlTreffer&)
'            If anzahlTreffer < 1 Then
'                nachricht("Problem beim löschen:" & SQLupdate)
'                Return False
'            Else
'                Return True
'            End If
'        Catch ex As Exception
'            nachricht("Fehler beim Abspeichern: " & ex.ToString)
'            Return False
'        End Try
'    End Function
'    Shared Sub SETSQLPARAMSADRESSERB(ByVal COM As OracleCommand, ByVal SEKID%)
'        Try
'            COM.Parameters.AddWithValue(":GEMEINDENR", aktadr.Gisadresse.gemeindeNrBig())
'            COM.Parameters.AddWithValue(":GEMEINDETEXT", aktadr.Gisadresse.gemeindeName.Trim)
'            COM.Parameters.AddWithValue(":STRASSENNAME", aktadr.Gisadresse.strasseName.Trim)
'            COM.Parameters.AddWithValue(":STRCODE", aktadr.Gisadresse.strasseCode)
'            COM.Parameters.AddWithValue(":FS", aktadr.FS)
'            COM.Parameters.AddWithValue(":HAUSNRKOMBI", aktadr.Gisadresse.HausKombi)
'            If aktadr.PLZ.IsNothingOrEmpty Then
'                aktadr.PLZ = "0"
'            End If
'            COM.Parameters.AddWithValue(":PLZ", CInt(aktadr.PLZ))
'            COM.Parameters.AddWithValue(":POSTFACH", aktadr.Postfach)
'            COM.Parameters.AddWithValue(":ADRESSTYP", CInt(aktadr.Adresstyp))
'        Catch ex As Exception
'            nachricht("Fehler beim SETSQLPARAMSADRESSERB: " & ex.ToString)
'        End Try
'    End Sub
'    Public Shared Function RB_Adresse_abspeichern_NeuOracle() As Integer
'        Dim anzahlTreffer& = 0, hinweis$ = "", newid& = 0
'        Dim com As OracleCommand
'        Try
'            Dim SQLupdate$ =
'             "INSERT INTO ParaAdresse (GEMEINDENR,GEMEINDETEXT,STRASSENNAME,STRCODE,FS,HAUSNRKOMBI,PLZ,POSTFACH,ADRESSTYP) " +
'                                  " VALUES (:GEMEINDENR,:GEMEINDETEXT,:STRASSENNAME,:STRCODE,:FS,:HAUSNRKOMBI,:PLZ,:POSTFACH,:ADRESSTYP)"
'            SQLupdate$ = SQLupdate$ & " RETURNING ID INTO :R1"

'            nachricht("nach setSQLbody : " & SQLupdate)
'            oeffneconnParadigmaDBOracle()
'            nachricht("nach dboeffnen  ")
'            com = New OracleCommand(SQLupdate$, connParadigmaDBOracle)
'            nachricht("vor setParams  ")
'            SETSQLPARAMSADRESSERB(com, 0)

'            newid = clsOracleIns.GetNewid(com, SQLupdate)
'            connParadigmaDBOracle.Close()
'            Return clsOracleIns.gebeNeuIDoderFehler(newid, SQLupdate)
'        Catch ex As Exception
'            l("Fehler beim Abspeichern: " & ex.ToString)
'            Return -2
'        End Try
'    End Function



'    Shared Sub SETSQLPARAMSFLSTOracle(ByVal COM As OracleCommand)
'        COM.Parameters.AddWithValue(":GEMCODE", aktFST.normflst.gemcode)
'        COM.Parameters.AddWithValue(":FLUR", aktFST.normflst.flur)
'        COM.Parameters.AddWithValue(":ZAEHLER", aktFST.normflst.zaehler)
'        COM.Parameters.AddWithValue(":NENNER", aktFST.normflst.nenner)
'        COM.Parameters.AddWithValue(":ZNKOMBI", aktFST.normflst.fstueckKombi)
'        COM.Parameters.AddWithValue(":GEMARKUNGSTEXT", aktFST.normflst.gemarkungstext)
'        COM.Parameters.AddWithValue(":FS", aktFST.normflst.FS)
'        COM.Parameters.AddWithValue(":FLAECHEQM", aktFST.normflst.flaecheqm)
'        ' com.Parameters.AddWithValue(":ID", sekid)
'    End Sub
'    Public Shared Function RB_FLST_abspeichern_NeuOracle() As Integer
'        Dim anzahlTreffer& = 0, hinweis$ = "", newid& = 0
'        Dim com As OracleCommand
'        Try
'            Dim SQLUPDATE As String = "INSERT INTO ParaFlurstueck (GEMCODE,FLUR,ZAEHLER,NENNER,ZNKOMBI,GEMARKUNGSTEXT,FS,FLAECHEQM) " +
'                            " VALUES (:GEMCODE,:FLUR,:ZAEHLER,:NENNER,:ZNKOMBI,:GEMARKUNGSTEXT,:FS,:FLAECHEQM)"
'            SQLUPDATE$ = SQLUPDATE$ & " RETURNING ID INTO :R1"
'            oeffneconnParadigmaDBOracle()
'            com = New OracleCommand(SQLUPDATE, connParadigmaDBOracle)
'            SETSQLPARAMSFLSTOracle(com)
'            newid = clsOracleIns.GetNewid(com, SQLUPDATE)
'            paradigmaDBschliessen()
'            Return clsOracleIns.gebeNeuIDoderFehler(newid, SQLUPDATE)
'        Catch ex As Exception
'            l("Fehler beim Abspeichern: " & ex.ToString)
'            Return -2
'        End Try
'    End Function



'End Class
