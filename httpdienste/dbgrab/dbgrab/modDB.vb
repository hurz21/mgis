Imports System.Data
Imports Npgsql
Module modDB
    Property myconn As New Npgsql.NpgsqlConnection
    Private _mycount As Integer
    Private dt As DataTable
    Property allegruppen As String()
    Property gruppendict As New Dictionary(Of String, Int16)

    Sub dbMain()
        getconnection()
        myconn.Open()
        'allegruppen = gruppentabEinlesen()
        'rechteZerlegen()
        'myconn.Close()
        'myconn.Open()
        'alleGruppenErfassen() 'obs diente nur der erstellung der tabelle in db
        'alleGruppenausschreiben("n:\gruppen.csv")
        myconn.Close() : myconn.Dispose()
    End Sub
    Private Sub getconnection()
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

End Module
