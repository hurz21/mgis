Public Class clsKomplexe
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
                item.kurz = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
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
                summe = summe & "Titel: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("komplexname").ToString.Trim & trenn)
                summe = summe & "lauf: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("komplexNummer").ToString.Trim & trenn)
                summe = summe & "Naturraum: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Naturraum").ToString.Trim & trenn)
                summe = summe & "Erfasst: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("erfassungsdatum").ToString.Trim & trenn)
                summe = summe & "Fläche: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("komplexFlaeche").ToString.Trim & trenn)
                summe = summe & "Schutzkategorie: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Schutzkategorie").ToString.Trim & trenn)

                summe = summe & "Parag. 20: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("schutz20c").ToString.Trim & trenn)
                summe = summe & "Parag. 20 anteil: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("schutz20c_anteil").ToString.Trim & trenn)

                summe = summe & "Vegetationseinheit: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Vegetationseinheiten").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)


                summe = summe & "Umgebung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Umgebung").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Habitate/Strukturen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("habitate_strukturen").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Höhe-min: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("hoehe_min").ToString.Trim & trenn)
                summe = summe & "Höhe_max: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("hoehe_max").ToString.Trim & trenn)
                summe = summe & "Neigung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("neigung").ToString.Trim & trenn)
                summe = summe & "Exposition: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("exposition").ToString.Trim & trenn)
                summe = summe & "Wasserhaushalt: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Wasserhaushalt").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Untergrund: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("untergrund").ToString.Trim & trenn)
                summe = summe & "WertKriterien: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("WertKriterien").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Bewertung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Bewertung").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Gefährdung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("gefaehrdung").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Quellen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Quellen").ToString.Trim & trenn)
                summe = summe & "Sicherungs-Massnahme: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("SicherungsMassnahme").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
                summe = summe & "Bemerkungen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Bemerkungen").ToString.Trim.Replace("<br>", Environment.NewLine & " - ") & trenn)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeBaulastenINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
