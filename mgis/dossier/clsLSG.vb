Imports System.Data

Public Class clsLSG
    Friend Shared Function getLSGInfo4point(winpt As myPoint, ByRef rESULT_text_LSG As String,
                                        ByRef LSG As String, ByRef Lsgpdf As String, schematabelle As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" getLSGInfo4point ---------------------- anfang")

            Return getlSGExtracted(rESULT_text_LSG, LSG, schematabelle, Lsgpdf, winpt)
            l(" getLSGInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getLSGInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function getlSGExtracted(ByRef rESULT_text_lSG As String, ByRef lSG As String, schematabelle As String,
                                            ByRef pdf As String, winpt As myPoint) As Boolean
        Dim dt As System.Data.DataTable
        Try
            l(" getlSGExtracted ---------------------- anfang")
            dt = clsDossier.getDtHauptabfrageFlaeche(winpt, schematabelle)
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein nsg")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                pdf = clsDBtools.fieldvalue(dt.Rows(0).Item("verordnung")).Trim
                l("pdf=" & clsDBtools.fieldvalue(dt.Rows(0).Item("verordnung")).ToString)

                lSG = clsDBtools.fieldvalue(dt.Rows(0).Item("name_2")).ToString.Trim
                pdf = "\\w2gis02\gdvell\nkat\aid\342\texte\" & pdf.Trim & ".pdf"
                rESULT_text_lSG = bildeLSGINFO(dt)
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
