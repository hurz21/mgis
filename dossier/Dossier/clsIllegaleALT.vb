Public Class clsIllegaleALT
    Friend Shared Function getIllegaleALTInfo4point(winpt As myPoint, item As clsDossierItem, ByRef strError As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" getIllegaleALTInfo4point ---------------------- anfang")
            Return getIllegaleALTExtracted(item, winpt, strError)
            l(" getIllegaleALTInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getIllegaleALTInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Shared Function getIllegaleALTExtracted(item As clsDossierItem, winpt As myPoint, ByRef strError As String) As Boolean
        Dim dt As System.Data.DataTable
        l("getIllegaleExtracted ---------------------- ")

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
                    item.kurz = item.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("aktenzeichen")).Trim
                    item.datei = "" 'clsDBtools.fieldvalue(dt.Rows(i).Item("vid")).Trim
                    'wsgpdf = wsgpdf & "," & datei
                Next
                item.result = bildeIllegaleALTINFO(dt)
                Return True
            End If
            l(" getIllegaleExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getIllegaleExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildeIllegaleALTINFO(DataRow As System.Data.DataTable) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = ", " & Environment.NewLine
            If DataRow.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            End If
            For i = 0 To DataRow.Rows.Count - 1
                'summe = summe & " ----------------------------------- " & trenn
                summe = summe & "Aktenzeichen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("aktenzeichen").ToString.Trim & " ") & trenn
                summe = summe & "Az-Bauaufsicht: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("bauaufsicht_aktenzeichen").ToString.Trim & " ") & trenn
                summe = summe & "FNP: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("fnp").ToString.Trim & " ") & trenn
                summe = summe & "Zuletzt bearbeitet am: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("ltzt_bearbtgsschritt_am").ToString.Trim & " ") & trenn
                summe = summe & "abgeschlossen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("abgeschl").ToString.Trim & " ") & trenn
                summe = summe & "Wiedervorlage am:: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("wvl_d").ToString.Trim & " ") & trenn
                summe = summe & "Bisher: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("bemerkungen").ToString.Trim & " ") & trenn
                summe = summe & "Ordner: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("ordner").ToString.Trim & " ") & trenn
                summe = summe & "Stadt/Gemeinde: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("stadt_gemeinde").ToString.Trim & " ") & trenn
                summe = summe & ", Gemarkung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Gemarkung").ToString.Trim & " ")
                summe = summe & ", Flur: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Flur").ToString.Trim & " ")
                summe = summe & ", Flurstück: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Flurstueck").ToString.Trim & " ")
                summe = summe & ", Parzelle: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Parzelle").ToString.Trim & " ") & trenn
                summe = summe & "Art der Nutzung:: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("art_der_nutzung").ToString.Trim & " ") & trenn

                summe = summe & "Art der baul. Anlagen:: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("art_der_baul_anlagen").ToString.Trim & " ") & trenn

                summe = summe & "Fall gehört zum Bereich:: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("bereich").ToString.Trim & " ") & trenn

            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeIllegaleINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
