Imports System.Data

Public Class clsLSG
    Friend Shared Function getLSGInfo4point(winpt As myPoint, item As clsDossierItem, ByRef strError As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" getLSGInfo4point ---------------------- anfang")

            Return getlSGExtracted(item, winpt, strError)
            l(" getLSGInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getLSGInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function getlSGExtracted(item As clsDossierItem, winpt As myPoint, ByRef strError As String) As Boolean
        Dim dt As System.Data.DataTable
        Try
            l(" getlSGExtracted ---------------------- anfang")
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

                item.kurz = clsDBtools.fieldvalue(dt.Rows(0).Item("name_2")).ToString.Trim
                item.datei = m.appServerUnc & "\nkat\aid\342\texte\" & item.datei.Trim & ".pdf"
                item.result = bildeLSGINFO(dt)
                Return True
            End If
            l(" getlSGExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getlSGExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildeLSGINFO(dataRow As DataTable) As String
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
