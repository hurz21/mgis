Imports Devart.Data.Oracle
Imports System.Data

Public Class clsOracleIns
    ''' <summary>
    ''' für INSERT unter Oracle
    ''' </summary>
    ''' <param name="com"></param>
    ''' <param name="SqlString">Mit Returning sequenz</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewid(ByRef com As OracleCommand, ByVal SqlString As string) As Long
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
    Shared Sub nachricht(ByVal text$)
        My.Log.WriteEntry("IN clsOracleIns: " & text)
    End Sub
End Class
