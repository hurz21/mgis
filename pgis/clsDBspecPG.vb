Imports System.Data
Imports Npgsql

Public Class clsDBspecPG
    Implements db_grundfunktionen
	Implements ICloneable
	Private _mydb As New clsDatenbankZugriff
	Public myconn As Npgsql.NpgsqlConnection
	Public hinweis$ = ""
	Private _mycount As Long
	Sub nachricht (text$)
        'todo
        My.Log.WriteEntry("in clsDBspecMYSQL: " & text$)
 End Sub
	Public Function sqlexecute(ByRef newID As Long) As Long Implements db_grundfunktionen.sqlexecute

		Dim retcode As Integer, Hinweis$ = ""
		Dim com As New Npgsql.NpgsqlCommand()
		Dim anzahlTreffer&
		Try
			If mydb.dbtyp = "mysql" Then
				retcode = dboeffnen(Hinweis$)
			End If
			retcode = 0
			com.Connection = myconn
			com.CommandText = mydb.SQL
			anzahlTreffer& = CInt(com.ExecuteNonQuery)
			If mydb.SQL.StartsWith("insert".ToLower) Then
				com.CommandText = "Select LAST_INSERT_ID()"
				newID = CLng(com.ExecuteScalar)
			End If
			Return anzahlTreffer&
		Catch myerror As OleDb.OleDbException
			retcode = -1
			Hinweis &= "sqlexecute: Database connection error: " & _
			 myerror.Message & " " & _
			 myerror.Source & " " & _
			 myerror.StackTrace & " " & _
			 mydb.getDBinfo("")
			nachricht(Hinweis)
			Return 0
		Catch e As Exception
			retcode = -2
			Hinweis &= "sqlexecute: Allgemeiner Fehler: " & _
			 e.Message & " " & _
			 e.Source & " " & _
			 mydb.Schema
			nachricht(Hinweis)
			Return 0
		Finally
			com.Dispose()
			dbschliessen(Hinweis)
		End Try
	End Function

	Public Function dboeffnen(ByRef resultstring As String) As Integer Implements db_grundfunktionen.dboeffnen
		Dim retcode%
		Try
			retcode = 0
			If doConnection(hinweis$) Then
				myconn.Open()
			Else
				hinweis$ = "Fehler bei der erstellung der connection:" & hinweis
			End If

		Catch myerror As Npgsql.NpgsqlException
            hinweis$ &= "MySqlException, beim ÖFFNERN UU. ist die DB nicht aktiv. " & vbCrLf & "Fehler beim Öffnen der DB " & _
             "Database connection error: " & _
             myerror.Message & " " & _
             mydb.host & " " & _
             mydb.Schema
            Return -1
        Catch e As Exception
            hinweis$ &= "beim ÖFFNEN Database connection error: " & _
             e.Message & " " & _
             e.Source & " " & _
             mydb.Schema
            Return -2
        End Try
        Return retcode
    End Function

    Public Function dbschliessen(ByRef resultstring As String) As Integer Implements db_grundfunktionen.dbschliessen
        Try
            myconn.Close()
            myconn.Dispose()
            Return 0
        Catch myerror As Npgsql.NpgsqlException
            resultstring$ &= "UU. ist die DB nicht aktiv. " & vbCrLf & "Fehler beim schliessen der DB " & _
             "Database connection error: " & _
             myerror.Message & " " & _
             mydb.host & " " & _
             mydb.Schema
            Return -1
        Catch e As Exception
            resultstring$ &= "Database connection error: schliessen" & _
             e.Message & " " & _
             e.Source & " " & _
             mydb.Schema
            Return -1
        End Try
    End Function

    Public Function doConnection(ByRef hinweis As String) As Boolean Implements db_grundfunktionen.doConnection
        Try
            'myconn = New Npgsql.NpgsqlConnection(String.Format("Data Source={0};Initial Catalog={1};User ID={2};PWD={3}", _
            '                     mydb.host, mydb.Schema, mydb.username, mydb.password))
              Dim csb As New NpgsqlConnectionStringBuilder
             csb.Host = mydb.host
            ' csb. = mydb.Schema
            csb.UserName = mydb.username
            csb.Password = mydb.password
            csb.Database = mydb.Schema
            csb.Port = CInt("5432")
            csb.Pooling = False
            csb.MinPoolSize = 1
            csb.MaxPoolSize = 20
            csb.Timeout = 15
            csb.SslMode = SslMode.Disable
            myconn = New NpgsqlConnection(csb.ConnectionString)
            Return True
        Catch ex As Exception
            hinweis$ &= ex.Message & ex.Source
            Return False
        End Try
    End Function
    '    Private Sub makeConnection(ByVal host As String, datenbank As String, ByVal dbuser As String, ByVal dbpw As String, ByVal dbport As String)
    '    Dim csb As New NpgsqlConnectionStringBuilder
    '    Try
    '       ' l("makeConnection")
    '        'If String.IsNullOrEmpty(mydb.ServiceName) Then
    '        'klassisch
    '        csb.Host = host
    '        ' csb. = mydb.Schema
    '        csb.UserName = dbuser
    '        csb.Password = dbpw
    '        csb.Database = datenbank
    '        csb.Port = CInt(dbport)
    '        csb.Pooling = False
    '        csb.MinPoolSize = 1
    '        csb.MaxPoolSize = 20
    '        csb.Timeout = 15
    '        csb.SslMode = SslMode.Disable
    '        myconn = New NpgsqlConnection(csb.ConnectionString)
    '        l("makeConnection fertig")
    '    Catch ex As Exception
    '        l("fehler in makeConnection" & ex.ToString)
    '    End Try
    'End Sub

    Public Function getDataDT() As String Implements db_grundfunktionen.getDataDT
        Dim retcode As Integer, hinweis As String = ""
        _mycount = 0
        retcode = dboeffnen(hinweis$)

        If retcode < 0 Then
            hinweis$ &= String.Format("FEHLER, Datenbank in getDataDT  konnte nicht geöffnet werden! {0}{1}", vbCrLf, mydb.getDBinfo(""))
            Return hinweis
        End If
        Try
            Dim com As New Npgsql.NpgsqlCommand(mydb.SQL, myconn)
            Dim da As New Npgsql.NpgsqlDataAdapter(com)
            'da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            dt = New DataTable
            _mycount = da.Fill(dt)
            retcode = dbschliessen(hinweis$)
            If retcode < 0 Then
                hinweis$ &= "FEHLER, Datenbank in getDataDT konnte nicht geschlossen werden! " & vbCrLf & mydb.getDBinfo("")
            End If
            com.Dispose()
            da.Dispose()
            Return hinweis
        Catch myerror As Npgsql.NpgsqlException
            retcode = -1
            hinweis &= "FEHLER, getDataDT Database connection error: " & _
             myerror.Message & " " & _
             myerror.Source & " " & _
             myerror.StackTrace & " " & _
             mydb.host & " " & mydb.Schema
            Return hinweis
        Catch e As Exception
            retcode = -2
            hinweis &= "FEHLER, getDataDT Database connection error: " & _
             e.Message & " " & _
             e.Source & " " & _
             mydb.Schema
            Return hinweis
        Finally
            retcode = dbschliessen(hinweis$)
            If retcode < 0 Then
                hinweis$ &= "FEHLER, 2 Datenbank konnte nicht geschlossen werden! " & vbCrLf & mydb.getDBinfo("")
            End If
        End Try
    End Function

	Public Sub New()
		MyClass.New("mysql")
	End Sub
	Public Sub New(ByVal dbtypIn$)
		mydb.dbtyp = dbtypIn$
	End Sub
	Public Function Clone() As Object Implements System.ICloneable.Clone
		Return MemberwiseClone()
	End Function

	Public Property mycount() As Long Implements db_grundfunktionen.mycount
		Get
			Return _mycount
		End Get
		Set(ByVal value As Long)
			_mycount = value
		End Set
	End Property

	Private _dt As New DataTable
	Property dt() As System.Data.DataTable Implements db_grundfunktionen.dt
		Get
			Return _dt
		End Get
		Set(ByVal value As DataTable)
			_dt = value
		End Set
	End Property

	Public Property mydb() As clsDatenbankZugriff Implements db_grundfunktionen.mydb
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
            My.Log.WriteEntry("ADOgetOneString_neu: " & hinweis)
			If mycount > 0 Then
				Return dt.Rows(0).Item(0).ToString
			Else
				Return ""
			End If
		Catch e As Exception
			myMessage = "Error : " & _
			 e.Message & " " & _
			 e.Source & " " & hinweis
			Return myMessage
		End Try
	End Function

End Class