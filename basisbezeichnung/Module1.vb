Module Module1
    Public conn As NpgsqlConnection
    Sub Main()
        Dim sql, errortext As String
        Dim dt As DataTable
        '  weistauf = "DEHE06120000QBuu                                           "
        ' sql = "SELECT * FROM public.alkis_lagemithn where  binary gml_id='" & weistauf & "'"
        conn = getConnectString()

        conn.Open()
        sql = "select * from alkis.basis_f"
        dt = getdt(sql, errortext)
        Dim gid As Integer

        Dim zaehler, nenner, kombi, lage, weistauf, zeigtauf, hausnr As String
        For i = 0 To dt.Rows.Count - 1
            gid = CInt(dt.Rows(i).Item("gid"))
            zaehler = CStr(dt.Rows(i).Item("zaehler")).Trim
            nenner = CStr(dt.Rows(i).Item("nenner")).Trim
            weistauf = CStr(dt.Rows(i).Item("weistauf")).Trim
            zeigtauf = CStr(dt.Rows(i).Item("zeigtauf")).Trim
            If nenner = "0" Then
                kombi = zaehler
            Else
                kombi = zaehler & "/" & nenner
            End If
            lage = getlage(weistauf, zeigtauf, hausnr)
            If lage Is Nothing Then
            Else
                updateBasisf(gid, kombi, lage, hausnr)
            End If
            hausnr = ""
        Next

    End Sub
    Sub l(text As String)
        Debug.Print("fehler " & text)
    End Sub
    Private Sub updateBasisf(gid As Integer, kombi As String, lage As String, hausnr As String)
        Try
            l(" MOD ---------------------- anfang")
            Dim Sql = "update alkis.basis_f   set  lage='" & lage.Trim & "'" &
                                                  ", hausnr='" & hausnr.Trim & "' " &
                                                  ", bezeich='" & kombi & "' " &
                                                  " where gid=" & gid
            sqlexecute(Sql)
            l(" MOD ---------------------- ende")
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
        End Try
    End Sub

    Private Sub sqlexecute(sql As String)
        Try
            l(" MOD ---------------------- anfang")
            Dim Res As Object

            Dim anzahlTreffer As Long = 0
            'Dim conn As NpgsqlConnection = New NpgsqlConnection()
            Dim com As Npgsql.NpgsqlCommand = New Npgsql.NpgsqlCommand()
            'conn = getConnectString()
            'conn.Open()
            com.Connection = conn
            com.CommandText = sql

            'com = New NpgsqlCommand(sql, conn)
            Dim da As New NpgsqlDataAdapter(com)

            anzahlTreffer = System.Convert.ToInt64(com.ExecuteNonQuery().ToString())
            l(" MOD ---------------------- ende")
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
        End Try
    End Sub

    Private Function getlage(weistauf As String, zeigtauf As String, ByRef hausnr As String) As String
        Dim sql, errortext, lagebez As String
        Dim lagenr, gemeinde, lageohnenr As Integer
        Try
            Dim dt As DataTable
            If weistauf Is Nothing Or weistauf.Trim = String.Empty Then
                sql = "SELECT * FROM alkis.lagemithn where trim(gml_id)='" & zeigtauf.Trim & "'   "
                dt = getdt(sql, errortext)
                If dt.Rows.Count = 0 Then
                    sql = "SELECT * FROM alkis.lageohnehn where trim(gml_id)='" & zeigtauf.Trim & "'   "
                    dt = getdt(sql, errortext)

                    If dt.Rows.Count = 0 Then Return Nothing
                    If dt.Rows.Count = 1 Then
                        lagenr = CInt(dt.Rows(0).Item("lage"))
                        lageohnenr = CInt(dt.Rows(0).Item("lageohnenr"))
                        gemeinde = CInt(dt.Rows(0).Item("gemeinde"))
                        'hausnr = CStr(dt.Rows(0).Item("hausnummer")).Trim
                    End If
                    sql = "SELECT * FROM alkis.lageschluessel where gemeinde=" & gemeinde &
                            " and lage=" & lagenr
                    dt = getdt(sql, errortext)
                    If dt.Rows.Count = 0 Then Return ""
                    lagebez = CStr(dt.Rows(0).Item("bezeichnung"))
                    Return lagebez.Trim

                End If
            Else
                sql = "SELECT * FROM alkis.lageohnehn where trim(gml_id)='" & weistauf.Trim & "'   "
                dt = getdt(sql, errortext)
                If dt.Rows.Count = 0 Then
                    sql = "SELECT * FROM alkis.lagemithn where trim(gml_id)='" & weistauf.Trim & "'   "
                    dt = getdt(sql, errortext)
                End If
                If dt.Rows.Count < 1 Then Return Nothing
                If dt.Rows.Count = 1 Then
                    lagenr = CInt(dt.Rows(0).Item("lage"))
                    gemeinde = CInt(dt.Rows(0).Item("gemeinde"))
                    hausnr = CStr(dt.Rows(0).Item("hausnummer")).Trim
                End If
                sql = "SELECT * FROM alkis.lageschluessel where gemeinde=" & gemeinde &
                        " and lage=" & lagenr
                dt = getdt(sql, errortext)
                If dt.Rows.Count = 0 Then Return ""
                lagebez = CStr(dt.Rows(0).Item("bezeichnung"))
                Return lagebez.Trim
            End If
        Catch ex As Exception
            Debug.Print("")
        End Try
    End Function

    Function getdt(sql As String, ByRef errortext As String) As DataTable
        Dim dt As DataTable
        Dim mycount As Integer
        Try
            Dim com As NpgsqlCommand
            com = New NpgsqlCommand(sql, conn)
            Dim da As New NpgsqlDataAdapter(com)
            dt = New DataTable
            mycount = da.Fill(dt)
            Return dt
        Catch ex As Exception
            errortext = "fehler in   getDt open:" & ex.ToString()
            Return Nothing
        End Try
    End Function

    Private Function getConnectString() As NpgsqlConnection
#If DEBUG Then
        Return New NpgsqlConnection("Server=" + "gis02" + ";User Id=postgres;" +
                                         "Password=lkof4;Database=" + "postgis20" + ";")
#Else

        Return New NpgsqlConnection("Server=" + "gis02" + ";User Id=postgres;" +
                                    "Password=lkof4;Database=" + "postgis20" + ";")
#End If
    End Function
End Module
