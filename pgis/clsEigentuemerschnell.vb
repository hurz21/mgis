Imports System.Data
Imports Devart.Data.Oracle

Public Class clsEigentuemerschnell
    Public Property eigentschnellKonn As New Devart.Common.DbConnectionStringBuilder
    Public Property EigentuemerSchnellDB As OracleConnection = New OracleConnection()

    Public Sub oeffneConnectionEigentuemer()
        Try
            EigentuemerSchnellDB = New OracleConnection("Data Source=(DESCRIPTION=" &
                                   "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" & "ora-clu-vip-003" &
                                   ")(PORT=1521)))" &
                                   "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" & "gis.kreis-of.local" &
                                   ")));" &
                                   "User Id=" & "gis" &
                                   ";Password=" & "A604l6rrpn" &
                                   ";direct=yes;")

            EigentuemerSchnellDB.Open()
        Catch ex As Exception
            nachricht("Fehler in oeffneConnectionEigentuemer: " & ex.ToString)
        End Try
    End Sub
    'Sub nachricht(ttt As String)

    'End Sub
    Public Function getEigentuemerdata(ByVal fs As String,
                                        ByRef kurzinfo As String,
                                        ByRef nameundadresse As String,
                                        ByRef _mycount As Integer,
                                        ByRef dt As DataTable) As Boolean
        Dim com As OracleCommand
        Try
            l("in getEigentuemerdata ")
            Dim sql = "select * from fs2eigentuemer where fs='" & fs & "'"
            com = New OracleCommand(sql, EigentuemerSchnellDB)
            Dim da As New OracleDataAdapter(com)
            'da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            dt = New DataTable
            _mycount = da.Fill(dt)
            If _mycount < 1 Then
                Return False
            End If
            kurzinfo = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("TOOLTIP")))
            nameundadresse = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("NAMENADRESSEN")))
            Return True
        Catch ex As Exception
            nachricht("fehler in getEigentuemerdata:" & ex.ToString)
            Return False
        End Try
    End Function
    Public Sub eigentuemerdbSchliessen()
        EigentuemerSchnellDB.Close()
    End Sub

    'Function homa(fs As String, bezirk As String) As Boolean
    '    '  clsEigentuemerschnell.oeffneConnectionEigentuemer(EigentuemerSchnellDB)

    'End Function



End Class
