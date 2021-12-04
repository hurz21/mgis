Imports System.Data

Public Class clsAltlast
    Friend Shared Function getAltlastInfo4point(winpt As myPoint, ByRef rESULT_text_wSG As String,
                                          ByRef wSG As String, ByRef wsgpdf As String, schematabelle As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" getAltlastInfo4point ---------------------- anfang")
            Return getAltlastExtracted(rESULT_text_wSG, wSG, wsgpdf, schematabelle, winpt)
            l(" getAltlastInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getAltlastInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function getAltlastExtracted(ByRef rESULT_text_wSG As String, ByRef wSG As String,
                                           ByRef wsgpdf As String,
                                           schematabelle As String, winpt As myPoint) As Boolean
        Dim dt As System.Data.DataTable
        l("getAltlastExtracted ---------------------- ")
        wSG = "" : wsgpdf = ""
        dt = clsDossier.getDtHauptabfrageFlaeche(winpt, schematabelle)
        Try
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein nsg")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                Dim datei As String
                For i = 0 To dt.Rows.Count - 1
                    wSG = wSG & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("ident")).Trim
                    'datei = "\\w2gis02\gdvell\" & clsDBtools.fieldvalue(dt.Rows(i).Item("link").ToString.Replace("/", "\").Trim)
                    'wsgpdf = wsgpdf & "," & datei
                Next
                'wSG = clsDBtools.fieldvalue(dt.Rows(0).Item("schutzzone")).Trim
                rESULT_text_wSG = bildealtlastINFO(dt)
                Return True
            End If
            l(" getAltlastExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getAltlastExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildealtlastINFO(DataRow As DataTable) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = ", " & Environment.NewLine
            'If DataRow.Rows.Count > 1 Then
            '    summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            'End If
            For i = 0 To DataRow.Rows.Count - 1
                'summe = summe & " ----------------------------------- " & trenn
                summe = summe & clsDBtools.fieldvalue(DataRow.Rows(i).Item("ausweisung").ToString.Trim & " ")
                summe = summe & clsDBtools.fieldvalue(DataRow.Rows(i).Item("name").ToString.Trim & " ")

                summe = summe & clsDBtools.fieldvalue(DataRow.Rows(i).Item("zus_setz").ToString.Trim & trenn)
                summe = summe & "Zeitraum: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("zeit").ToString.Trim & trenn)
                summe = summe & "Stand: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("stand").ToString.Trim & trenn)
                summe = summe & "Stoffe: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("stoffe").ToString.Trim & trenn)

                summe = summe & "Fläche [qm]: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("flaeche_m2").ToString.Trim & trenn)
                summe = summe & "Volumen [m3]: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Vol_m3").ToString.Trim & trenn)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildealtlastINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
