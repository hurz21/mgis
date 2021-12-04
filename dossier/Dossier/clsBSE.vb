Public Class clsBSE
    Friend Shared Function getInfo4point(winpt As myPoint, item As clsDossierItem, ByRef strError As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" getInfo4point ---------------------- anfang: " & item.schematabelle)
            Return getExtracted(item, winpt, strError)
            l(" getInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Shared Function getExtracted(item As clsDossierItem, winpt As myPoint, ByRef strError As String) As Boolean
        Dim dt As System.Data.DataTable
        l("getBaulastenExtracted ---------------------- " & item.schematabelle)
        item.kurz = "" : item.datei = ""
        clsDossier.Question(winpt, dt, item.schematabelle, strError)
        If strError.ToLower.StartsWith("fehler") Then
            l(" nach question ---------------------- ende" & strError)
            Return False
        End If
        Try
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein nsg")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                Dim datei As String
                For i = 0 To dt.Rows.Count - 1
                    item.kurz = item.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("art")).Trim
                    'pdf = clsDBtools.fieldvalue(dt.Rows(i).Item("tiff")).Trim
                    'wsgpdf = wsgpdf & "," & datei
                Next
                item.result = bildeINFO(dt)
                Return True
            End If
            l(" getBaulastenExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getBaulastenExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildeINFO(DataRow As System.Data.DataTable) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = " " & Environment.NewLine
            If DataRow.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            End If
            For i = 0 To DataRow.Rows.Count - 1
                'summe = summe & " ----------------------------------- " & trenn
                summe = summe & "Typ: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("art").ToString.Trim & trenn)
                summe = summe & "Titel: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("name").ToString.Trim & trenn)
                summe = summe & "Ausgewiesen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Ausgewiesen").ToString.Trim & trenn)
                summe = summe & "Veröffentlicht: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("verordnung").ToString.Trim & trenn)
                summe = summe & "Geändert: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("aenderung_1").ToString.Trim & trenn)
                summe = summe & "Geändert: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("aenderung_2").ToString.Trim & trenn)
                summe = summe & "Geändert: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("aenderung_3").ToString.Trim & trenn)
                summe = summe & "Bemerkung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Bemerkung").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Fläche [ha]: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("flaeche_ha").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeBaulastenINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
