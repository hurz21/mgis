﻿Imports System.Data

Public Class clsWSG
    Friend Shared Function getWSGInfo4point(winpt As myPoint, ByRef rESULT_text_wSG As String,
                                       ByRef wSG As String, ByRef wsgpdf As String, schematabelle As String) As Boolean
        Dim resulttext As String = ""

        Try
            l(" getwSGInfo4point ---------------------- anfang")

            Return getwSGExtracted(rESULT_text_wSG, wSG, wsgpdf, schematabelle, winpt)
            l(" getwSGInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getwSGInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function getwSGExtracted(ByRef rESULT_text_wSG As String, ByRef wSG As String,
                                            ByRef wsgpdf As String,
                                            schematabelle As String, winpt As myPoint) As Boolean
        Dim dt As System.Data.DataTable
        wSG = "" : wsgpdf = ""
        dt = clsDossier.getDtHauptabfrageFlaeche(winpt, schematabelle)
        Try
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein nsg")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("neu")).ToString.Trim
                Dim datei As String
                For i = 0 To dt.Rows.Count - 1
                    wSG = wSG & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("schutzzone")).Trim
                    datei = "\\w2gis02\gdvell\" & clsDBtools.fieldvalue(dt.Rows(i).Item("link").ToString.Replace("/", "\").Trim)
                    wsgpdf = wsgpdf & "," & datei
                Next
                'wSG = clsDBtools.fieldvalue(dt.Rows(0).Item("schutzzone")).Trim
                rESULT_text_wSG = bildeWSGINFO(dt)
                Return True
            End If
            l(" getwSGExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getwSGExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildeWSGINFO(DataRow As DataTable) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = ", " & Environment.NewLine
            If DataRow.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            End If
            For i = 0 To DataRow.Rows.Count - 1
                summe = summe & " ----------------------------------- " & trenn
                summe = summe & clsDBtools.fieldvalue(DataRow.Rows(i).Item("art").ToString.Trim & " ")
                summe = summe & clsDBtools.fieldvalue(DataRow.Rows(i).Item("gruppe").ToString.Trim & " ")
                summe = summe & clsDBtools.fieldvalue(DataRow.Rows(i).Item("name").ToString.Trim & trenn)
                summe = summe & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Schutzzone").ToString.Trim & trenn)
                summe = summe & "Ausg.: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("ausgewiesen").ToString.Trim & trenn)

                summe = summe & "Veröff.: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("veroeffentlicht").ToString.Trim & trenn)
                summe = summe & "Geändert.: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("geaendert").ToString.Trim & trenn)
                summe = summe & "Änderung.: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("veroeff_geaendert").ToString.Trim & trenn)
                summe = summe & "HLUG-Nr.: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("hlug").ToString.Trim & trenn)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeWSGINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
