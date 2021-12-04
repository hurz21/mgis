

Module modListe
    Friend Function exekuteVorgangsListe(outfile As String, aktbox As clsRange) As String
        Dim listenDateiName As String
        Dim recs As String()
        Dim result As Integer
        listenDateiName = "\\w2gis02\gdvell\paradigmacache\" & outfile & "\aktvorgangsliste.txt"
        recs = dateieinlesen(listenDateiName)
        'einmal truncate table  outfile; als sql absetzen
        Dim summe As String = ""

        glob2.nachricht("point_shpfile_erzeugen ============================================================ vor")
        Dim erfolg As Integer = modPG.pgDBtableAnlegen(summe)
        l("erfolg: " & erfolg)
        l("summe: " & summe)


        For i = 10 To recs.Count - 1
            nid = recs(i)
            l("vor exekuteEinzelVorgang: " & nid)
            If Not nid.Trim = String.Empty Then
                result = machmal(CInt(nid))
            End If

            l("nach exekuteEinzelVorgang: " & nid & " /" & result)
        Next
        Return CStr(recs.Count - 10)
    End Function

    Private Function dateieinlesen(listenDateiName As String) As String()
        Dim recs As String()
        Dim inhalt As String
        l("listenDateiName " & listenDateiName)
        Try
            inhalt = IO.File.ReadAllText(listenDateiName)
            recs = inhalt.Split(CType(vbCr, Char()))
            For i = 0 To recs.Count - 1
                recs(i) = recs(i).Replace(vbLf, "")
            Next

            Return recs
        Catch ex As Exception
            l("fehler in dateieinlesen:  " & ex.ToString)
            Return Nothing
        End Try

    End Function

    Function machmal(vid As Integer) As Integer
        ReDim ebenen(0) : ebenen(0) = vid

        'modOracle.getDTOracle("Select * from raumbezugplus where vorgangsid=" & vid, dtRBplus)

        Dim anz As Integer
        anz = clsSQLS.getDTSQLS("Select * from raumbezugplus where vorgangsid=" & vid, dtRBplus)
        Dim returnstring As String = ""
        Dim dtRBpolygon As New DataTable

        modPG.doRBschleife(dtRBplus, dtRBpolygon, "", returnstring)
        l(returnstring)
        glob2.nachricht("point_shpfile_erzeugen ============================================================ ende")
        Return 1
    End Function
End Module
