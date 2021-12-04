'Namespace LIBDB
Public Class clsSqlparam
        Public Shared paramListe As New List(Of clsSqlparam)
        Property name As String
        Property obj As Object
        Sub New(_name As String, _obj As Object)
            name = _name
            obj = _obj
        End Sub
        Shared Sub korrigiereParam(dbtyp As String, paramliste As List(Of clsSqlparam))
            For i = 0 To paramliste.Count - 1
                If dbtyp = "oracle" Then paramliste(i).name = ":" & paramliste(i).name.ToUpper
                If dbtyp = "sqls" Then paramliste(i).name = "@" & paramliste(i).name.ToUpper
                If dbtyp = "mysql" Then paramliste(i).name = "@" & paramliste(i).name
                If dbtyp = "postgres" Then paramliste(i).name = ":" & paramliste(i).name
            Next
        End Sub
    End Class
    Public Class clsDatenbankZugriff
        Implements ICloneable
        Public Function setDBTYP() As Boolean
            dbtyp = getDBTYP()
            Return True
        End Function

        Public Function getDBTYP() As String
            Dim dbtyptest$ = "mysql"
            If Tabelle.ToLower.EndsWith(".dbf") Then dbtyptest = "dbf"
            If Schema.ToLower.EndsWith(".mdb") Then dbtyptest = "mdb"
            Return dbtyptest
        End Function
        Public Overrides Function tostring() As String
            Return getDBinfo("")
        End Function
        Private Sub getDBinfoString(ByRef trenn As String, ByRef info As System.Text.StringBuilder)
            info.Append("clsDatenbankZugriff ++++++++++++ Objektbeginn" & trenn)
            info.Append(String.Format("   dbtyp: {0}{1}", dbtyp, trenn))
            info.Append(String.Format("   Server: {0}{1}", Host, trenn))
            info.Append(String.Format("   Schema: {0}{1}", Schema, trenn))
            info.Append(String.Format("   tabelle: {0}{1}", Tabelle, trenn))
            info.Append(String.Format("   SQL: {0}{1}", SQL, trenn))
            info.Append(String.Format("   SQLWhereValue: {0}{1}", SQLWhere, trenn))
            info.Append(String.Format("   username: {0}{1}", username, trenn))
            info.Append(String.Format("   password: {0}{1}", password, trenn))
            info.Append("clsDatenbankZugriff ++++++++++++ Objektende")
        End Sub
        Public Function getDBinfo(ByVal trenn As String) As String
            Dim info As New System.Text.StringBuilder
            If String.IsNullOrEmpty(trenn) Then
                trenn$ = vbCrLf
            Else
            trenn$ = trenn
        End If
            getDBinfoString(trenn, info)
            Return info.ToString
        End Function
        Public Property SQLWhere() As String

    Public Function cleanSQL() As String
        'Beseitigt reste aus access-abfragen
        If SQL Is Nothing Then
            Return Nothing
        End If
        SQL = SQL.Replace("[", "`")
        SQL = SQL.Replace("]", "`")
        If SQL.ToLower.Contains("like") Then
            If SQL.Contains("*'") Then
                SQL = SQL.Replace("*'", "%'")
            End If
            If SQL.Contains("'*") Then
                SQL = SQL.Replace("'*", "'%")
            End If
        End If
        Return SQL
    End Function
    ''' <summary>
    ''' ServiceName für OracleDB
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ServiceName As String
        Public Property defaultSelectColumnString As String = ""
        Public Property password() As String
        Public Property username() As String
        Public Property Tabelle() As String
        Public Property Host() As String
        Public Property SQL() As String
        ''' <summary>
        ''' "mdb" oder "dbf" oder "mysql"  "postgres"
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public Property dbtyp() As String
        Public Property Schema() As String
        Public Property order() As String
        Public Property link_spalte_name() As String
        Public Function Clone() As Object Implements System.ICloneable.Clone
            Return MemberwiseClone()
        End Function
    End Class
'End Namespace
