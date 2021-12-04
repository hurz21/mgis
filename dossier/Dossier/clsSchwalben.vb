Public Class clsUniversell
    Friend Shared Function getInfo4point(winpt As myPoint, item As clsDossierItem,
                                         ByRef strError As String, kurzSpaltenName As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" clsUniversellgetInfo4point ---------------------- anfang: " & item.schematabelle)
            Return getExtracted(item, winpt, strError, kurzSpaltenName)
            l(" clsUniversellgetInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in clsUniversell getInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Shared Function getExtracted(item As clsDossierItem, winpt As myPoint,
                                         ByRef strError As String, kurzSpaltenName As String) As Boolean
        Dim dt As System.Data.DataTable
        l("clsUniversell ---------------------- " & item.schematabelle)
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
                For i = 0 To dt.Rows.Count - 1
                    item.datei = item.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item(kurzSpaltenName)).Trim
                    'pdf = clsDBtools.fieldvalue(dt.Rows(i).Item("tiff")).Trim
                    'wsgpdf = wsgpdf & "," & datei
                Next
                item.kurz = " vorhanden"
                item.result = clsDossier.bildeAttributTabelle(dt)
                Return True
            End If
            l(" clsUniversell ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in clsUniversell: " & ex.ToString())
            Return False
        End Try
    End Function
End Class


