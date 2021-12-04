Imports System.Data

Public Class clsFFH
    Friend Shared Function getFFHInfo4point(winpt As myPoint, ByRef rESULT_text_FFH As String,
                                            ByRef FFH As String, ByRef FFHpdf As String,
                                            ByRef ffhLink As String) As Boolean
        Dim resulttext As String = ""

        Try
            l(" getNSGInfo4point ---------------------- anfang")

            Return getFFHExtracted(rESULT_text_FFH, FFH, FFHpdf, ffhLink, winpt)
            l(" getNSGInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getNSGInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function getFFHExtracted(ByRef rESULT_text_FFH As String, ByRef fFH As String,
                                            ByRef fFHpdf As String,
                                            ByRef ffhLink As String, winpt As myPoint) As Boolean
        Dim dt As System.Data.DataTable
        Dim SchemaTabelle As String
        SchemaTabelle = "public.dossier_ffhgebiet"
        dt = clsDossier.getDtHauptabfrageFlaeche(winpt, SchemaTabelle)
        Try
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein nsg")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                fFHpdf = clsDBtools.fieldvalue(dt.Rows(0).Item("nummer")).Trim
                l("pdf=" & clsDBtools.fieldvalue(dt.Rows(0).Item("nummer")).ToString)

                fFH = clsDBtools.fieldvalue(dt.Rows(0).Item("name")).ToString.Trim

                fFHpdf = "\\w2gis02\gdvell\inetpub\wwwroot\natura2000\allgemeiner_VO_Text\Natura2000-VO-Text_allgemeiner_Teil.pdf"
                ffhLink = "http://geodaten.kreis-offenbach.de/natura2000/Anlagen1-3-4/FFH/" &
                              clsDBtools.fieldvalue(dt.Rows(0).Item("nummer")).ToString.Trim & ".html"

                ffhLink = "http://www.rpda.de/01%20Natura%202000-Verordnung/Natura2000-VO-RPDA/Anlagen1-3-4/FFH/" &
                              clsDBtools.fieldvalue(dt.Rows(0).Item("nummer")).ToString.Trim &
                              ".html"
                rESULT_text_FFH = bildefFHINFO(dt)
                Return True
            End If
            l(" getFFHExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getFFHExtracted: " & ex.ToString())
            Return False
        End Try
    End Function



    Private Shared Function bildefFHINFO(DataRow As DataTable) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = ", " & Environment.NewLine
            summe = summe & clsDBtools.fieldvalue(DataRow.Rows(0).Item("art").ToString.Trim & trenn)
            summe = summe & clsDBtools.fieldvalue(DataRow.Rows(0).Item("name").ToString.Trim & trenn)
            summe = summe & "Fläche [ha]: " & clsDBtools.fieldvalue(DataRow.Rows(0).Item("flaeche_ha").ToString.Trim & trenn)
            summe = summe & "Nummer: " & clsDBtools.fieldvalue(DataRow.Rows(0).Item("nummer").ToString.Trim & trenn)
            'summe = summe & "url.: http://geodaten.kreis-offenbach.de/natura2000/Anlagen1-3-4/FFH/" & clsDBtools.fieldvalue(DataRow.Rows(0).Item("nummer").ToString.Trim & ".html" & trenn)
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeNSGINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function

End Class
