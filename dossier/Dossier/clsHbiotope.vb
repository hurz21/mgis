Public Class clsHbiotope
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
                    item.kurz = item.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("aid")).Trim
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
                summe = summe & "Nr: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("aid").ToString.Trim & trenn)
                summe = summe & "Titel: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Name").ToString.Trim & trenn)
                summe = summe & "Erfasst: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("erfassungsdatum").ToString.Trim & trenn)
                summe = summe & "Parag. 20: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("schutz20c").ToString.Trim & trenn)
                summe = summe & "Parag. 20 anteil: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("schutz20c_anteil").ToString.Trim & trenn)
                summe = summe & "Biotoptyp: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("biotoptyp").ToString.Trim & trenn)
                summe = summe & "Biotoptypnr: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("biotoptypnummer").ToString.Trim & trenn)
                summe = summe & "Biotoptyp Anteil: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Biotoptypanteil").ToString.Trim & trenn)
                summe = summe & "Nebenbiotoptyp: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("nebenbiotoptyp").ToString.Trim & trenn)
                summe = summe & "Vegetationseinheiten: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Vegetationseinheiten").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Umgebung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Umgebung").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Habitate/Strukturen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("habitate_strukturen").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Höhe-min: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("hoehe_min").ToString.Trim & trenn)
                summe = summe & "Höhe_max: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("hoehe_max").ToString.Trim & trenn)
                summe = summe & "Neigung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("neigung").ToString.Trim & trenn)
                summe = summe & "Exposition: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("exposition").ToString.Trim & trenn)
                summe = summe & "Wasserhaushalt: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Wasserhaushalt").ToString.Trim & trenn)
                summe = summe & "Untergrund: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("untergrund").ToString.Trim & trenn)
                summe = summe & "WertKriterien: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("WertKriterien").ToString.Trim & trenn)
                summe = summe & "Bewertung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Bewertung").ToString.Trim & trenn)
                summe = summe & "Quellen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Quellen").ToString.Trim & trenn)
                summe = summe & "SicherungsMassnahme: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("SicherungsMassnahme").ToString.Trim & trenn)
                summe = summe & "Bemerkungen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Bemerkungen").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeBaulastenINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
