'Imports System.Data
'Imports Devart.Data.Oracle

'Public Class clsEigentuemerschnellOracle
'    Public Property eigentschnellKonn As New Devart.Common.DbConnectionStringBuilder
'    Public Property EigentuemerSchnellDB As OracleConnection = New OracleConnection()

'    Public Sub oeffneConnectionEigentuemer()
'        Try

'            EigentuemerSchnellDB = New OracleConnection("Data Source=(DESCRIPTION=" &
'                                   "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" & "ora-clu-vip-003" &
'                                   ")(PORT=1521)))" &
'                                   "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" & "gis.kreis-of.local" &
'                                   ")));" &
'                                   "User Id=" & "gis" &
'                                   ";Password=" & "A604l6rrpn" &
'                                   ";direct=yes;")

'            EigentuemerSchnellDB.Open()
'        Catch ex As Exception
'            nachricht("Fehler in oeffneConnectionEigentuemer: " & ex.ToString)
'        End Try
'    End Sub


'    Function getDt(query As String) As DataTable
'        Dim com As OracleCommand
'        Dim _mycount As Long
'        Try
'            l("in paradigma oracle getDt ")
'            Dim dt As DataTable

'            com = New OracleCommand(query, EigentuemerSchnellDB)
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

'    Public Sub eigentuemerdbSchliessen()
'        EigentuemerSchnellDB.Close()
'    End Sub

'End Class
