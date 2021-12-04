Imports System.Data

Public Class clsND
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
                    'item.datei = item.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("aid")).Trim
                    item.link = clsDBtools.fieldvalue(dt.Rows(i).Item("gid")).Trim
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
                summe = summe & "Gemeinde: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Gemeinde").ToString.Trim & trenn)
                summe = summe & "Gemarkung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("Gemarkung").ToString.Trim & trenn)
                summe = summe & "Nr: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("aid").ToString.Trim & trenn)
                summe = summe & "laufnr: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("lfd_nr").ToString.Trim & trenn)

                summe = summe & "Name: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("name").ToString.Trim & trenn)
                summe = summe & "Umgebung: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("umgebung").ToString.Trim & trenn)
                summe = summe & "Radius: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("radius").ToString.Trim & trenn)
                summe = summe & "Fläche[qm]: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("flaeche_qm").ToString.Trim & trenn)
                If DataRow.Rows.Count > 1 Then
                    summe = summe & " ----------------------------------- " & trenn
                End If
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeBaulastenINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
