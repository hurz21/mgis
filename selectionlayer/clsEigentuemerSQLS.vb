Imports System.Data
Imports System.Data.SqlClient
Public Class clsSQLS
    'Public Property eigentschnellKonn As New Devart.Common.DbConnectionStringBuilder
    Public Property EigentuemerSchnellDB As SqlConnection = New SqlConnection

    Public Sub oeffneConnectionSQLS()
        Try
            l("oeffneConnectionSQLS")
            Dim cstring As String
            cstring = "Server=msql01;Database=Paradigma;User=sgis;Pwd=WinterErschranzt.74;"
            l(cstring)
            EigentuemerSchnellDB = New SqlConnection(cstring)
            EigentuemerSchnellDB.Open()
            l("oeffneConnectionSQLS nach open")
        Catch ex As Exception
            l("Fehler in oeffneConnectionEigentuemer: " & ex.ToString)
        End Try
    End Sub


    Function getDt(query As String) As DataTable
        Dim com As SqlCommand
        Dim _mycount As Long
        Try
            l("in paradigma  getDt ")
            Dim dt As DataTable

            com = New SqlCommand(query, EigentuemerSchnellDB)
            Dim da As New SqlDataAdapter(com)
            'da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            dt = New DataTable
            _mycount = da.Fill(dt)
            If _mycount < 1 Then
                l("kein treffer")
                Return dt
            End If
            l("in paradigma   getDt fertig")
            Return dt
        Catch ex As Exception
            nachricht("fehler in in paradigma   getDt:" & ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Sub schliesseConnectionSQLS()
        EigentuemerSchnellDB.Close()
    End Sub

    Friend Shared Function getDTSQLS(queryString As String, ByRef dtRBplus As DataTable) As Integer
        l("getDTSQLS-------------------------")
        Dim eigSDB As New clsSQLS
        'Dim dt As DataTable
        Try
            eigSDB.oeffneConnectionSQLS()
            dtRBplus = eigSDB.getDt(queryString)
            eigSDB.schliesseConnectionSQLS()
            Return dtRBplus.Rows.Count
        Catch ex As Exception
            l("fehler in getDTSQLS: " & ex.ToString)
            Return Nothing
        End Try
    End Function



End Class


