'Imports System.Data
'Imports System.Data.SqlClient
'Public Class clsEigentuemerSQLS
'    'Public Property eigentschnellKonn As New Devart.Common.DbConnectionStringBuilder
'    Public Property EigentuemerSchnellDB As SqlConnection = New SqlConnection

'    Public Sub oeffneConnectionSQLS()
'        Try
'            EigentuemerSchnellDB = New SqlConnection("Server=msql01;Database=Paradigma;User=sgis;Pwd=WinterErschranzt.74;")
'            EigentuemerSchnellDB.Open()
'        Catch ex As Exception
'            nachricht("Fehler in oeffneConnectionEigentuemer: " ,ex)
'        End Try
'    End Sub
'    Function getDt(query As String) As DataTable
'        Dim com As SqlCommand
'        Dim _mycount As Long
'        Try
'            l("in paradigma oracle getDt ")
'            Dim dt As DataTable
'            com = New SqlCommand(query, EigentuemerSchnellDB)
'            Dim da As New SqlDataAdapter(com)
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
'            nachricht("fehler in in paradigma oracle getDt:" ,ex)
'            Return Nothing
'        End Try
'    End Function

'    Public Sub schliesseConnectionSQLS()
'        EigentuemerSchnellDB.Close()
'    End Sub

'End Class


