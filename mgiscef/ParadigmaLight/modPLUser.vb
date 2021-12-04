Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports mgis
Module modPLUser
    Friend Function getUsernr(username As String) As Integer
        Dim query As String = "", hinweis As String = ""
        Dim dt As New DataTable
        Try
            l("getUsernr---------------------- anfang")
            query = "select * from PL_BEARBEITER where username='" & username & "'"
            'dt = modSQLsTools.getDTFromParadigmaDBsqls(query)
            dt = modgetdt4sql.getDT4Query(query, pLightMsql, hinweis)
            Dim nr As Integer
            If dt.Rows.Count > 0 Then
                nr = CInt(clsDBtools.fieldvalue(dt.Rows(0).Item("pl_bearbeiterID")))
            Else
                nr = 0
            End If
            Return nr
            l("getUsernr---------------------- ende")
        Catch ex As Exception
            l("Fehler in getUsernr: " & ex.ToString())
            Return -1
        End Try
    End Function

    Friend Function addUser(username As String, ABTEILUNG As String) As Integer
        Dim erfolg As Boolean = False
        Dim querie As String
        ' Dim ID As Integer
        Dim returnIdentity As Boolean = True
        Try
            l("addUser---------------------- anfang")
            clsSqlparam.paramListe.Clear()
            querie = "INSERT INTO PL_BEARBEITER (USERNAME,ABTEILUNG) VALUES (@USERNAME,@ABTEILUNG)"
            clsSqlparam.paramListe.Add(New clsSqlparam("USERNAME", username.ToLower.Trim)) 'MYGLObalz.sitzung.VorgangsID)
            clsSqlparam.paramListe.Add(New clsSqlparam("ABTEILUNG", ABTEILUNG.ToLower.Trim))
            Dim ID = pLightMsql.manipquerie(querie, clsSqlparam.paramListe, True, "pl_bearbeiterid")
            If ID > 0 Then Return ID Else Return 0
            l("addUser---------------------- ende")
        Catch ex As Exception
            l("Fehler in addUser: " & ex.ToString())
            Return -1
        End Try
    End Function
End Module
