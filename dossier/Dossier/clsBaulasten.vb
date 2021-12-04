Public Class clsBaulasten
    Friend Shared Function getInfo4point(winpt As myPoint, item As clsDossierItem, ByRef strError As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" getInfo4point ---------------------- anfang")
            Return getBaulastenExtracted(item, winpt, strError)
            l(" getInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Shared Function getBaulastenExtracted(item As clsDossierItem, winpt As myPoint, ByRef strError As String) As Boolean
        Dim dt As System.Data.DataTable
        l("getBaulastenExtracted ---------------------- ")
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
                    item.kurz = item.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("jahr_blattnr")).Trim
                    item.datei = clsDBtools.fieldvalue(dt.Rows(i).Item("tiff")).Trim
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
            Dim trenn As String = ", " & Environment.NewLine
            If DataRow.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            End If
            For i = 0 To DataRow.Rows.Count - 1
                'summe = summe & " ----------------------------------- " & trenn
                summe = summe & ", Gemeinde: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("gemeinde").ToString.Trim)
                summe = summe & ", Gemarkung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("gemarkung").ToString.Trim & trenn)
                'summe = summe & ", Kennzeichen: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("kennzeichen1").ToString.Trim & trenn)

                summe = summe & ", Baulast: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("jahr_blattnr").ToString.Trim & trenn)
                summe = summe & ", Baulastnr: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("baulastnr").ToString.Trim & trenn)
                'summe = summe & ", Bauort: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("bauort").ToString.Trim & trenn)
                summe = summe & ", GSefundenin: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("gefundenin").ToString.Trim & trenn)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeBaulastenINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
