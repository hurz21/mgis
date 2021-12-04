Imports System.Data
Public Class clsUebKrof
    Friend Shared Function getInfo4point(winpt As myPoint, dossieritem As clsDossierItem, ByRef strError As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" getUEBKROFInfo4point ---------------------- anfang")
            Return getExtracted(dossieritem, winpt, strError)
            l(" getUEBKROFInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getUEBKROFInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Shared Function getExtracted(dossieritem As clsDossierItem, winpt As myPoint, ByRef strError As String) As Boolean
        Dim dt As System.Data.DataTable
        l("getExtracted ---------------------- ")
        clsDossier.Question(winpt, dt, dossieritem.schematabelle, strError)
        If strError.ToLower.StartsWith("fehler") Then
            l(" nach question ---------------------- ende" & strError)
            Return False
        End If
        Try
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("keine items")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                For i = 0 To dt.Rows.Count - 1
                    dossieritem.kurz = clsDBtools.fieldvalue(dt.Rows(i).Item("name_2")).Trim
                    dossieritem.datei = clsDBtools.fieldvalue(dt.Rows(i).Item("verordnung")).Trim
                Next
                dossieritem.result = bildeINFO(dt)
                Return True
            End If
            l(" getExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildeINFO(DataRow As DataTable) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = ", " & Environment.NewLine
            If DataRow.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            End If
            For i = 0 To DataRow.Rows.Count - 1
                'summe = summe & " ----------------------------------- " & trenn
                summe = summe & Environment.NewLine
                summe = summe & "Bezeichnung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("name").ToString.Trim & trenn)
                summe = summe & "ausgewiesen am: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("ausgewiesen").ToString.Trim & trenn)
                summe = summe & "Veröffentlicht: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("veroeffentlicht").ToString.Trim & trenn)
                summe = summe & "Fläche: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("flaeche_ha").ToString.Trim & trenn)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeIllegaleINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
