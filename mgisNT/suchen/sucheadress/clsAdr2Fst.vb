Imports System.Data

Public Class clsAdr2Fst
    Public Shared Function getflurstueck(aktADR As clsAdress) As DataTable
        'koordinate der adresse aus halo holen
        Dim innerSQL As String
        Try
            l("getflurstueck---------------------- anfang")
            innerSQL = String.Format("  SELECT geom  " &
                                 "  FROM public.halofs where gemeindenr={0} AND strcode={1} AND Hausnr='{2}' and lower(zusatz)='{3}'",
                              aktADR.gemeindeNrBig, aktADR.strasseCode, aktADR.hausNr,
                              aktADR.hausZusatz.ToLower.Trim)
            l(innerSQL)
            Dim sql = "  SELECT * " &
                        "  FROM flurkarte.basis_f " &
                        "  WHERE ST_contains( flurkarte.basis_f.geom,(" & innerSQL & "  )" & "  );"
            'adresskoordinate mit flurstück verschneiden
            Dim dt As DataTable
            dt = getDTFromWebgisDB(sql, "postgis20")
            l("sql: " & sql)
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                Return dt
            Else
                'Dim fs As String = clsDBtools.fieldvalue(dt.Rows(0).Item("fs")).ToString.Trim
                Return dt
            End If
            l("getflurstueck---------------------- ende")
        Catch ex As Exception
            l("Fehler in getflurstueck: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Public Shared Sub mapFlurstueck(ByVal fsDT As DataTable, ByVal clsFlurstueck As clsFlurstueck,
                             ByRef summentext As String)
        Try
            l(" mapFlurstueck ---------------------- anfang")
            If fsDT.Rows.Count < 1 Then
                MsgBox("UUUPS: Es ist hier kein Flurstück zugeordnet. Daher ist keine Eigentümerabfrage möglich. Bitte informieren Sie den Admin! (Tel. 4434). Programm wird beendet!")
                summentext = "UUUPS: Es ist hier kein Flurstück zugeordnet. Daher ist keine Eigentümerabfrage möglich. Bitte informieren Sie den Admin! (Tel. 4434)"
                Exit Sub
            End If
            clsFlurstueck.gemcode = CInt(clsDBtools.fieldvalue(fsDT.Rows(0).Item("gemcode")))
            clsFlurstueck.flur = CInt(clsDBtools.fieldvalue(fsDT.Rows(0).Item("flur")))
            clsFlurstueck.zaehler = CInt(clsDBtools.fieldvalue(fsDT.Rows(0).Item("zaehler")))
            clsFlurstueck.nenner = CInt(clsDBtools.fieldvalue(fsDT.Rows(0).Item("nenner")))
            Dim a As String = clsDBtools.fieldvalue(fsDT.Rows(0).Item("gisarea")).Trim.Replace(".", ",")
            clsFlurstueck.flaecheqm = CInt(a)
            clsFlurstueck.istgebucht = CStr(clsDBtools.fieldvalue(fsDT.Rows(0).Item("istgebucht")))
            clsFlurstueck.FS = CStr(clsDBtools.fieldvalue(fsDT.Rows(0).Item("FS")))
            Dim gemparms As New clsGemarkungsParams
            clsFlurstueck.gemarkungstext = gemparms.gemcode2gemarkungstext(clsFlurstueck.gemcode)
            gemparms = Nothing
            Dim fst As String = ""
            summentext = ""
            For i = 0 To fsDT.Rows.Count - 1
                fst = fst & clsFlurstueck.gemarkungstext & ", "
                fst = fst & CInt(clsDBtools.fieldvalue(fsDT.Rows(i).Item("flur"))) & ", "
                fst = fst & CInt(clsDBtools.fieldvalue(fsDT.Rows(i).Item("zaehler"))) & "/"
                fst = fst & CInt(clsDBtools.fieldvalue(fsDT.Rows(i).Item("nenner"))) & " "
                summentext = summentext & " " & fst & ", "
                fst = ""
            Next
            l(" mapFlurstueck ---------------------- ende")
        Catch ex As Exception
            l("Fehler in mapFlurstueck: " & ex.ToString())
        End Try
    End Sub

End Class
