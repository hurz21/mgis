Public Class clsBaulasten
    Friend Shared Function getInfo4point(winpt As myPoint, ByRef rESULT_text As String,
                                        ByRef kurz As String, ByRef pdf As String, schematabelle As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" getInfo4point ---------------------- anfang")
            Return getBaulastenExtracted(rESULT_text, kurz, pdf, schematabelle, winpt)
            l(" getInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Shared Function getBaulastenExtracted(ByRef rESULT_text As String, ByRef kurz As String,
                                       ByRef pdf As String,
                                       schematabelle As String, winpt As myPoint) As Boolean
        Dim dt As System.Data.DataTable
        l("getBaulastenExtracted ---------------------- ")
        kurz = "" : pdf = ""
        dt = clsDossier.getDtHauptabfrageFlaeche(winpt, schematabelle)
        Try
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein nsg")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                Dim datei As String
                For i = 0 To dt.Rows.Count - 1
                    kurz = kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("jahr_blattnr")).Trim
                    pdf = clsDBtools.fieldvalue(dt.Rows(i).Item("tiff")).Trim
                    'wsgpdf = wsgpdf & "," & datei
                Next
                rESULT_text = bildeINFO(dt)
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
