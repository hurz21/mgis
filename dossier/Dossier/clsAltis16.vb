Public Class clsAltis16
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
        Try
            item.kurz = "" : item.datei = ""
            clsDossier.Question(winpt, dt, item.schematabelle, strError)
            If strError.ToLower.StartsWith("fehler") Then
                l(" nach question ---------------------- ende" & strError)
                Return False
            End If
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein treffer")
                Return False
            Else
                'Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                Dim datei As String
                For i = 0 To dt.Rows.Count - 1
                    item.kurz = item.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("gid")).Trim
                    'pdf = clsDBtools.fieldvalue(dt.Rows(i).Item("tiff")).Trim
                    'wsgpdf = wsgpdf & "," & datei
                Next
                item.kurz = " vorhanden"
                item.result = clsDossier.bildeAttributTabelle(dt)
                Return True
            End If
            l(" getBaulastenExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getBaulastenExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    'Private Shared Function bildeINFO(dt As System.Data.DataTable) As String
    '    Try
    '        Dim summe As String = ""
    '        Dim trenn As String = " " & Environment.NewLine
    '        If dt.Rows.Count > 1 Then
    '            summe = summe & " Es gibt hier " & dt.Rows.Count & " Ausweisungen !" & trenn
    '        End If
    '        For i = 0 To dt.Rows.Count - 1

    '            summe = summe & "Art: " & clsDBtools.fieldvalue(dt.Rows(i).Item("art_fl").ToString.Trim & trenn)
    '            summe = summe & "Status: " & clsDBtools.fieldvalue(dt.Rows(i).Item("status").ToString.Trim & trenn)
    '            summe = summe & "ID: " & clsDBtools.fieldvalue(dt.Rows(i).Item("altis_id").ToString.Trim & trenn)
    '            summe = summe & "Arbeitsname: " & clsDBtools.fieldvalue(dt.Rows(i).Item("Arbeitsname").ToString.Trim & trenn)
    '            summe = summe & "Zuständig: " & clsDBtools.fieldvalue(dt.Rows(i).Item("zust_behoerde").ToString.Trim & trenn)
    '            summe = summe & "Gemeinde: " & clsDBtools.fieldvalue(dt.Rows(i).Item("Gemeinde").ToString.Trim & trenn)
    '            summe = summe & "Strasse: " & clsDBtools.fieldvalue(dt.Rows(i).Item("Strasse").ToString.Trim & trenn)
    '            summe = summe & "Koordinate: " & clsDBtools.fieldvalue(dt.Rows(i).Item("rechts").ToString.Trim) &
    '                ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("hoch").ToString.Trim & trenn)
    '            summe = summe & " ----------------------------------- " & trenn
    '        Next
    '        Return summe
    '    Catch ex As Exception
    '        nachricht("fehler in bildeBaulastenINFO: " & ex.ToString)
    '        Return "keine info"
    '    End Try
    'End Function
End Class
