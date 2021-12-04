Public Class clsParadigmaVorgang
    Shared Function getInfo4point(winpt As myPoint, item As clsDossierItem, ByRef strError As String) As Boolean
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
                    item.kurz = item.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("paradigmavid")).Trim
                    'pdf = clsDBtools.fieldvalue(dt.Rows(i).Item("tiff")).Trim
                    'wsgpdf = wsgpdf & "," & datei
                Next
                item.result = bildeINFO(dt, item)
                Return True
            End If
            l(" getBaulastenExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getBaulastenExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildeINFO(DataRow As System.Data.DataTable, item As clsDossierItem) As String
        Try
            Dim summe As String = ""
            Dim datum As Date
            Dim oldvid As String = ""
            Dim newobj As New clsMyComboboxItem
            Dim trenn As String = " " & Environment.NewLine
            If DataRow.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            End If
            newobj.vid = "" : newobj.titel = "" : item.ParadigmaListe.Add(newobj)
            For i = 0 To DataRow.Rows.Count - 1
                datum = CDate(clsDBtools.fieldvalue(DataRow.Rows(i).Item("LetzteBearbeitung").ToString))
                newobj = New clsMyComboboxItem

                newobj.vid = (clsDBtools.fieldvalue(DataRow.Rows(i).Item("paradigmavid")))

                newobj.titel = newobj.vid & " - " & datum.ToString("yyyy-MM-dd") & " - " &
                               clsDBtools.fieldvalue(DataRow.Rows(i).Item("sgnr").ToString.Trim) & " - " &
                               clsDBtools.fieldvalue(DataRow.Rows(i).Item("titel").ToString.Trim)
                newobj.tipp = clsDBtools.fieldvalue(DataRow.Rows(i).Item("sgtext").ToString.Trim)
                If newobj.vid <> oldvid Then
                    item.ParadigmaListe.Add(newobj)
                    oldvid = newobj.vid
                End If

                summe = summe & "Sachgebiet_Nr: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("sgnr").ToString.Trim & trenn)
                summe = summe & "Sachgebiet: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("sgtext").ToString.Trim & trenn)

                summe = summe & "Titel: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("titel").ToString.Trim & trenn)
                summe = summe & "ParadigmaNr: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("paradigmavid").ToString.Trim & trenn)
                summe = summe & "LetzteBearbeitung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("LetzteBearbeitung").ToString.Trim & trenn)
                summe = summe & "letztes Jahr: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("letztesjahr").ToString.Trim & trenn)
                summe = summe & " ----------------------------------- " & trenn
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeBaulastenINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
