Imports Devart.Data.Oracle
Imports System.Data

Public Class clsDBspecOracle
    Implements IDB_grundfunktionen
    Implements ICloneable
    Implements IDisposable
    Private _mydb As New clsDatenbankZugriff
    '	Private mylog As LIBgemeinsames.clsLogging
    Public Property myconn() As OracleConnection
    Public hinweis$ = ""
    Private Params As New OracleParameterCollection
    Private _mycount As Long
    Private disposed As Boolean = False
    'Implement IDisposable.
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(disposing As Boolean)
        If disposed = False Then
            If disposing Then
                ' Free other state (managed objects).
                dt.Dispose()
                _dt.Dispose()
                disposed = True
            End If
            ' Free your own state (unmanaged objects).
            ' Set large fields to null.
        End If
    End Sub
    Sub l(text As String)

    End Sub
    Sub l(text As String, ex As Exception)

    End Sub
    Protected Overrides Sub Finalize()
        ' Simply call Dispose(False).
        Dispose(False)
    End Sub
    Public Sub AddParam(Name As String, Value As Object, dbtyp As String)
        Dim NewParam As OracleParameter
        Try
            l("AddParam---------------------- anfang")
            NewParam = New OracleParameter(Name, Value)
            Params.Add(NewParam)
        Catch ex As Exception
            l(dbtyp & ", Fehler in AddParam: " & ex.ToString())
        End Try
    End Sub
    Public Shared Function GetNewid(ByRef com As OracleCommand, ByVal SqlString As String) As Long
        Dim newid&
        If String.IsNullOrEmpty(SqlString) Then
            nachricht("Fehler in GetNewid&: SQLstring ist leer!!!")
            Return -3
        End If
        Try
            com.CommandText = SqlString
            com.CommandType = CommandType.Text
            Dim p_theid As New OracleParameter
            p_theid.DbType = DbType.Decimal
            p_theid.Direction = ParameterDirection.ReturnValue
            p_theid.ParameterName = ":R1"
            com.Parameters.Add(p_theid)
            Dim rtn = CInt(com.ExecuteNonQuery)
            newid = CLng(p_theid.Value)
            Return newid
        Catch oex As OracleException
            nachricht("Fehler in GetNewid&:" & oex.ToString & " / " & SqlString)
            Return -1
        Catch ex As Exception
            nachricht("Fehler in GetNewid&:" & ex.ToString & " / " & SqlString)
            Return -2
        End Try
    End Function

    Public Shared Function gebeNeuIDoderFehler(ByVal newid As Long, ByVal sqlstring As String) As Integer 'myGlobalz.sitzung.tempREC.mydb.SQL
        If newid < 1 Then
            nachricht("Problem beim Abspeichern:" & sqlstring)
            Return -1
        Else
            Return CInt(newid)
        End If
    End Function
    Public Function manipquerie(querie As String,
                                            slqparamlist As List(Of clsSqlparam),
                                            ReturnIdentity As Boolean, returnColumn As String) As Integer Implements IDB_grundfunktionen.manipquerie
        nachricht("Neu_speichern_Oracle -----------------------------------------------------")
        Dim anzahlTreffer& = 0, hinweis$ = "", newid& = 0
        Dim com As OracleCommand
        Dim dbtyp = "oracle"
        Try
            If ReturnIdentity Then
                querie = querie & " RETURNING " & returnColumn & " INTO :R1"
            End If
            querie = querie.Replace("@", ":")
            querie = querie.ToUpper
            nachricht("nach setSQLbody : " & querie)
            clsSqlparam.korrigiereParam(mydb.dbtyp, slqparamlist)
            Dim retcode = dboeffnen(hinweis)
            nachricht("nach dboeffnen  ")
            com = New OracleCommand(querie, myconn)
            nachricht("vor setParams  ")
            com.Parameters.Clear()
            setComParms(slqparamlist, com)
            If ReturnIdentity Then
                newid = GetNewid(com, querie)
                myconn.Close()
                If newid < 1 Then
                    l("Problem beim Abspeichern : " & querie)
                    Return -1
                Else
                    Return CInt(newid)
                End If
            Else
                anzahlTreffer& = CInt(com.ExecuteNonQuery)
                myconn.Close()
                If anzahlTreffer < 1 Then
                    nachricht("Problem beim Abspeichern:" & querie)
                    Return -1
                Else
                    Return CInt(anzahlTreffer)
                End If
            End If
        Catch ex As Exception
            l("Fehler beim Abspeichern: ", ex)
            Return -2
        End Try
    End Function

    Private Shared Sub setComParms(slqparamlist As List(Of clsSqlparam), com As OracleCommand)
        For i = 0 To slqparamlist.Count - 1
            com.Parameters.AddWithValue(slqparamlist(i).name, slqparamlist(i).obj)
        Next
    End Sub

    Public Function sqlexecute(ByRef newID As Long) As Long Implements IDB_grundfunktionen.sqlexecute
        Dim retcode As Integer, Hinweis$ = ""
        Dim com As New OracleCommand
        Dim anzahlTreffer As Long
        Dim anz As Object
        Try

            com = New OracleCommand
            retcode = 0
            com.Connection = myconn
            com.CommandText = mydb.SQL
            com.CommandType = CommandType.Text
            Dim p_theid As New OracleParameter
            If mydb.SQL.ToLower.StartsWith("insert") Then
                p_theid.DbType = DbType.Decimal
                p_theid.Direction = ParameterDirection.ReturnValue
                p_theid.ParameterName = ":R1"
                com.Parameters.Add(p_theid)
            End If
            anz = com.ExecuteNonQuery
            anzahlTreffer = CLng(anz)
            'wird die anzahl auch bei delete zurückgegeben ???
            If mydb.SQL.ToLower.StartsWith("insert") Then
                'com.CommandText = "Select max(id) from " & mydb.Tabelle
                'newID = CLng(com.ExecuteScalar)             
                Dim rtn = CInt(com.ExecuteNonQuery)
                newID = CLng(p_theid.Value)
            End If
            Return anzahlTreffer
        Catch myerror As OracleException
            retcode = -1
            Hinweis &= "sqlexecute: Database connection error: " &
             myerror.Message & " " &
             myerror.Source & " " &
             myerror.StackTrace & " " &
             mydb.getDBinfo("")
            '	mylog.log(Hinweis)
            Return 0
        Catch e As Exception
            retcode = -2
            Hinweis &= "sqlexecute: Allgemeiner Fehler: " &
             e.Message & " " &
             e.Source & " " &
             mydb.Schema
            'mylog.log(Hinweis)
            Return 0
        Finally
            com.Dispose()
            dbschliessen(Hinweis)
        End Try
    End Function

    Shared Sub nachricht(ByVal text$)
        My.Log.WriteEntry("IN oracle: " & text)
    End Sub

    Shared Sub nachricht_Mbox(ByVal text$)
        MsgBox(text$)
        My.Log.WriteEntry("IN Oracle: " & text)
    End Sub

    Public Function dboeffnen(ByRef resultstring As String) As Integer Implements IDB_grundfunktionen.dboeffnen
        Try
            If doConnection(hinweis$) Then
                '  nachricht(myconn.ConnectionString)
                myconn.Open()
            Else
                hinweis$ = "Fehler bei der Erstellung der connection:" & hinweis & myconn.ConnectionString
            End If
        Catch myerror As OracleException

            hinweis$ &= "OracleException, beim ÖFFNEN UU. ist die DB nicht aktiv. " & vbCrLf & "Fehler beim Öffnen der DB " &
             "Database connection error: " &
             myerror.Message & " " &
             mydb.Host & " " &
             mydb.Schema
            nachricht(String.Format("{0}-Datenbank ist nicht aktiv!{1}{2}", mydb.Host, vbCrLf, myerror))
            'glob2.nachricht("Datenbank ist nicht aktiv!" & vbCrLf & mydb.tostring)
            Return -1
        Catch e As Exception
            hinweis$ &= "beim ÖFFNEN Database connection error: " &
             e.Message & " " &
             e.Source & " " &
             mydb.Schema
            nachricht_Mbox(mydb.Host & ", Datenbank ist nicht aktiv!" & vbCrLf & e.ToString)
            'glob2.nachricht("Datenbank ist nicht aktiv!" & vbCrLf & mydb.tostring)
            Return -2
        End Try
        Return 0
    End Function

    Public Function dbschliessen(ByRef resultstring As String) As Integer Implements IDB_grundfunktionen.dbschliessen
        Try
            myconn.Close()
            myconn.Dispose()
            Return 0
        Catch myerror As OracleException
            resultstring$ &= "UU. ist die DB nicht aktiv. " & vbCrLf & "Fehler beim Schliessen der DB " &
                 "Database connection error: " &
                 myerror.Message & " " &
                 mydb.Host & " " &
                 mydb.Schema
            Return -1
        Catch e As Exception
            resultstring$ &= "Database connection error: schliessen" &
             e.Message & " " &
             e.Source & " " &
             mydb.Schema
            Return -1
        End Try
    End Function

    Public Function doConnection(ByRef hinweis As String) As Boolean Implements IDB_grundfunktionen.doConnection
        Try
            Dim csb As New OracleConnectionStringBuilder

            If String.IsNullOrEmpty(mydb.ServiceName) Then
                'klassisch
                csb.Server = mydb.Host
                ' csb. = mydb.Schema
                csb.UserId = mydb.username
                csb.Password = mydb.password
                csb.Pooling = False
                myconn = New OracleConnection(csb.ConnectionString)
            Else
                'TSN
                'myconn = New OracleConnection(getOracleconnectionString(mydb))
                'myconn.Unicode = True
                myconn = getConnection(mydb)
            End If
            Return True
        Catch ex As Exception
            nachricht(ex.ToString)
            Return False
        End Try
    End Function
    Public Shared Function getConnection(ByVal mydb As clsDatenbankZugriff) As OracleConnection
        Dim myconn As OracleConnection = New OracleConnection(getOracleconnectionString(mydb))
        myconn.Unicode = True
        Return myconn
    End Function

    Public Function getDataDT() As String Implements IDB_grundfunktionen.getDataDT
        Dim retcode As Integer, hinweis As String = ""
        _mycount = 0
#If DEBUG Then
        If iminternet Then
            Debug.Print("")
        End If
#End If
        retcode = dboeffnen(hinweis$)
        nachricht(retcode.ToString)
        If retcode < 0 Then
            hinweis$ &= String.Format("FEHLER, Datenbank in getDataDT  konnte nicht geöffnet werden! {0}{1}", vbCrLf, mydb.getDBinfo(""))
            nachricht(hinweis)
            Return hinweis
        End If
        Try
            nachricht(mydb.SQL)
            Dim com As New OracleCommand(mydb.SQL, myconn)
            Dim da As New OracleDataAdapter(com)
            'da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            dt = New DataTable
            _mycount = da.Fill(dt)
            retcode = dbschliessen(hinweis)
            If retcode < 0 Then
                hinweis$ &= "FEHLER, Datenbank in getDataDT konnte nicht geschlossen werden! " & vbCrLf & mydb.getDBinfo("")
            End If
            com.Dispose()
            da.Dispose()
            retcode = dbschliessen(hinweis)
            Return hinweis
        Catch myerror As OracleException
            retcode = -1
            hinweis &= "FEHLER, getDataDT Database connection OracleException: " &
             myerror.Message & " " &
             myerror.Source & " " &
             myerror.StackTrace & " " &
               mydb.Host & ", schema:" & mydb.Schema & "/" & mydb.SQL
            Return hinweis
        Catch e As Exception
            retcode = -2
            hinweis &= "FEHLER, getDataDT Database connection error: " &
             e.Message & " " &
             e.Source & " " &
             mydb.Host & ", schema:" & mydb.Schema & "/" & mydb.SQL
            Return hinweis
        Finally
            retcode = dbschliessen(hinweis)
            If retcode < 0 Then
                hinweis$ &= "FEHLER, 2 Datenbank konnte nicht geschlossen werden! " & vbCrLf & mydb.getDBinfo("")
            End If
        End Try
    End Function

    Public Sub New()

    End Sub

    Public Sub New(ByVal dbtypIn$)
        mydb.dbtyp = dbtypIn$
    End Sub

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function

    Public Property mycount() As Long Implements IDB_grundfunktionen.mycount
        Get
            Return _mycount
        End Get
        Set(ByVal value As Long)
            _mycount = value
        End Set
    End Property

    Private _dt As New DataTable
    Property dt As DataTable Implements IDB_grundfunktionen.dt
        Get
            Return _dt
        End Get
        Set(ByVal value As DataTable)
            _dt = value
        End Set
    End Property

    Public Property mydb() As clsDatenbankZugriff Implements IDB_grundfunktionen.mydb
        Get
            Return _mydb
        End Get
        Set(ByVal value As clsDatenbankZugriff)
            _mydb = value
        End Set
    End Property

    Public Function ADOgetOneString_neu() As String
        Dim myMessage$ = "", hinweis$ = ""
        Try
            hinweis = getDataDT()
            If mycount > 0 Then
                Return dt.Rows(0).Item(0).ToString
            Else
                Return ""
            End If
        Catch e As Exception
            myMessage = "Error : " &
             e.Message & " " &
             e.Source & " " & hinweis
            Return myMessage
        End Try
    End Function



    Public Shared Function getOracleconnectionString(mydb As clsDatenbankZugriff) As String
        Return "Data Source=(DESCRIPTION=" &
             "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=ora-clu-vip-003)(PORT=1521)))" &
              "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=ora-clu-vip-004)(PORT=1521)))" &
             "(LOAD_BALANCE=yes) (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" & mydb.ServiceName & ")));" &
             "User Id=" & mydb.username & ";Password=" & mydb.password & ";DIRECT=yes;"
        'Return "Data Source=(DESCRIPTION=" & _
        '          "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=ora-up-test.kreis-of.local)(PORT=1521)))" & _ 
        '          "(LOAD_BALANCE=yes) (CONNECT_DATA=(SERVER=DEDICATED)(SID=paradigt"   & ")));" & _
        '          "User Id=" & mydb.username & ";Password=" & mydb.password & ";"
    End Function


End Class
