Imports System.Data

Public Class clsNSG
    Friend Shared Function getNSGInfo4point(winpt As myPoint, item As clsDossierItem, ByRef strError As String) As Boolean
        Dim resulttext As String = ""
        Try

            Return getNSGExtracted(item, winpt, strError)
            l(" getNSGInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getNSGInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function getNSGExtracted(item As clsDossierItem, winpt As myPoint, ByRef strError As String) As Boolean
        Dim dt As System.Data.DataTable
        Try
            l(" getNSGExtracted ---------------------- anfang")
            clsDossier.Question(winpt, dt, item.schematabelle, strError)
            If strError.ToLower.StartsWith("fehler") Then
                l(" nach question ---------------------- ende" & strError)
                Return False
            End If
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein nsg")
                Return False
            Else
                item.kurz = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                item.datei = clsDBtools.fieldvalue(dt.Rows(0).Item("verordnung")).Trim
                l("pdf=" & clsDBtools.fieldvalue(dt.Rows(0).Item("verordnung")).ToString)
                'SQL = "SELECT * FROM schutzgebiete.v_naturschutzglb_f where gid='" & aid & "'" '
                'l("sql: " & SQL)
                'dt = getDTFromWebgisDB(SQL, "postgis20")
                'l("Anzahl=" & dt.Rows.Count)
                item.kurz = clsDBtools.fieldvalue(dt.Rows(0).Item("name_2")).ToString.Trim

                item.datei = m.appServerUnc & "\fkat\natur\natlandgeb\texte\" & item.datei.Trim & ".pdf"


                item.result = bildeNSGINFO(dt)
                Return True
            End If
            l(" getNSGExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getNSGExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildeNSGINFO(dataRow As DataTable) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = ", " & Environment.NewLine
            summe = summe & clsDBtools.fieldvalue(dataRow.Rows(0).Item("art").ToString.Trim & trenn)
            summe = summe & clsDBtools.fieldvalue(dataRow.Rows(0).Item("name_2").ToString.Trim & trenn)
            summe = summe & "ausg.: " & clsDBtools.fieldvalue(dataRow.Rows(0).Item("ausgewiesen").ToString.Trim & trenn)
            summe = summe & "Fläche [ha]: " & clsDBtools.fieldvalue(dataRow.Rows(0).Item("flaeche_ha").ToString.Trim & trenn)
            summe = summe & "veröff.: " & clsDBtools.fieldvalue(dataRow.Rows(0).Item("veroeffentlicht").ToString.Trim & trenn)
            'summe = summe & "url.: " & clsDBtools.fieldvalue(dataRow.Rows(0).Item("url").ToString.Trim & trenn)
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeNSGINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
