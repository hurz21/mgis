Imports System.Data

Imports System.Data.Common
Imports System.Data.SqlClient


Public Class clsDBspecMSSQL
    Implements IDB_grundfunktionen
    Implements ICloneable
    Implements IDisposable
    Private _mydb As New clsDatenbankZugriff
    '	Private mylog As LIBgemeinsames.clsLogging
    Public Property myconn() As SqlConnection
    Public hinweis$ = ""
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
    Protected Overrides Sub Finalize()
        ' Simply call Dispose(False).
        Dispose(False)
    End Sub
    Sub l(t As String)
        nachricht(t)
    End Sub


    Public Function manipquerie(querie As String,
                        slqparamlist As List(Of clsSqlparam),
                        ReturnIdentity As Boolean, returnColumn As String) As Integer Implements IDB_grundfunktionen.manipquerie
        l("Neu_speichern_sqls manip-----------------------------------------------------")
        Dim anzahlTreffer& = 0, newid& = 0
        Dim com As SqlCommand
        'Dim DBDA As SqlDataAdapter
        'Dim DBDT As DataTable
        Try
            querie = querie.ToUpper
            If ReturnIdentity Then
                querie = querie & ";SELECT CAST(scope_identity() AS int);"
            End If
            l("nach setSQLbody : " & querie)
            clsSqlparam.korrigiereParam(mydb.dbtyp, slqparamlist)
            Dim retcode = dboeffnen(hinweis)
            l("nach dboeffnen  ")
            com = New SqlCommand(querie, myconn)
            nachricht("vor setParams  ")
            Dim dval As Object
            Dim strDval As String
            For i = 0 To slqparamlist.Count - 1
                dval = slqparamlist(i).obj
                strDval = dval.ToString
                If slqparamlist(i).obj.ToString.StartsWith("1754-01-01") Or
                    slqparamlist(i).obj.ToString.StartsWith("01.01.1754") Or
                    strDval.StartsWith("01.01.0001") Then
                    dval = DBNull.Value
                End If
                com.Parameters.AddWithValue(slqparamlist(i).name, dval)
            Next
            slqparamlist.Clear()
            If ReturnIdentity Then
                Dim kobjssss = com.ExecuteScalar
                If kobjssss Is Nothing Then
                    newid = 0
                Else
                    newid = CLng(kobjssss.ToString)
                End If
                myconn.Close()
                If newid < 1 Then
                    l("Problem beim Abspeichern : " & querie)
                    Return -1
                Else
                    Return CInt(newid)
                End If
            Else
                anzahlTreffer& = CInt(com.ExecuteNonQuery)'ret number of affe rows
                myconn.Close()
                If anzahlTreffer < 1 Then
                    nachricht("Problem beim Abspeichern:" & querie)
                    Return -1
                Else
                    Return CInt(anzahlTreffer)
                End If
            End If
        Catch ex As Exception
            l("Fehler beim Abspeichern: " & ex.ToString)
            Return -2
        End Try
    End Function
    Public Function sqlexecute(ByRef newID As Long) As Long Implements IDB_grundfunktionen.sqlexecute
        Dim retcode As Integer
        Dim com As New SqlCommand
        Dim anzahlTreffer As Long
        Dim anz As Object
        Try
            If mydb.dbtyp = "PgSql" Then
                retcode = dboeffnen(Hinweis$)
            End If
            com = New SqlCommand
            retcode = 0
            com.Connection = myconn
            com.CommandText = mydb.SQL
            com.CommandType = CommandType.Text
            Dim p_theid As New SqlParameter
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
        Catch myerror As SqlException
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
        My.Log.WriteEntry("IN mssql: " & text)
    End Sub

    Shared Sub nachricht_Mbox(ByVal text$)
        MsgBox(text$)
        My.Log.WriteEntry("IN mssql: " & text)
    End Sub

    Public Function dboeffnen(ByRef resultstring As String) As Integer Implements IDB_grundfunktionen.dboeffnen
        Try
            If doConnection(hinweis$) Then
                '  nachricht(myconn.ConnectionString)
                myconn.Open()
            Else
                hinweis$ = "Fehler bei der Erstellung der connection:" & hinweis & myconn.ConnectionString
            End If

        Catch myerror As SqlException
            hinweis$ &= "nPgSqlException, beim ÖFFNEN UU. ist die DB nicht aktiv. " & vbCrLf & "Fehler beim Öffnen der DB " &
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
            nachricht(mydb.Host & ", Datenbank ist nicht aktiv!" & vbCrLf & e.ToString)
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
        Catch myerror As SqlException
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


    Public Shared Function getConnection(ByVal mydb As clsDatenbankZugriff) As SqlConnection
        Dim myconn As SqlConnection = New SqlConnection(getPostgresconnectionString(mydb))
        'myconn.unicode = True
        Return myconn
    End Function

    Public Function getDataDT() As String Implements IDB_grundfunktionen.getDataDT
        Dim retcode As Integer, hinweis As String = ""
        _mycount = 0
#If DEBUG Then
        'If iminternet Then
        '    Debug.Print("")
        'End If
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
            Dim com As New SqlCommand(mydb.SQL, myconn)
            Dim da As New SqlDataAdapter(com)
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
        Catch myerror As SqlException
            retcode = -1
            hinweis &= "FEHLER, getDataDT Database connection nPgSqlException: " &
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

    Public Property mycount As Long Implements IDB_grundfunktionen.mycount
        Get
            Return _mycount
        End Get
        Set(ByVal value As Long)
            _mycount = value
        End Set
    End Property

    Private _dt As New DataTable
    Property dt() As DataTable Implements IDB_grundfunktionen.dt
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
    Private Shared Function getPostgresconnectionString(mydb As clsDatenbankZugriff) As String
        Dim hinweis As String = ""
        Dim csb As New SqlConnectionStringBuilder
        Dim myconn As New SqlConnection
        csb.DataSource = mydb.Host '@"myServer\InstanceName,1433";
        csb.InitialCatalog = mydb.Schema
        csb.UserID = mydb.username
        csb.Password = mydb.password
        csb.Pooling = False
        csb.MinPoolSize = 1
        csb.MaxPoolSize = 20
        myconn = New SqlConnection(csb.ConnectionString)
        Return csb.ConnectionString
    End Function
    Public Function doConnection(ByRef hinweis As String) As Boolean Implements IDB_grundfunktionen.doConnection
        Try
            Dim csb As New SqlConnectionStringBuilder
            csb.DataSource = mydb.Host '@"myServer\InstanceName,1433";
            csb.InitialCatalog = mydb.Schema
            csb.UserID = mydb.username
            csb.Password = mydb.password
            'csb.Pooling = False
            'csb.MinPoolSize = 1
            'csb.MaxPoolSize = 20
            myconn = New SqlConnection(csb.ConnectionString)
            Return True
        Catch ex As Exception
            nachricht("fehler mssql doConnection " & ex.ToString)
            Return False
        End Try
    End Function

End Class


