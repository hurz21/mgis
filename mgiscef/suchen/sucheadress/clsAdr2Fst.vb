Imports System.Data
Imports mgis

Public Class clsAdr2Fst
    Shared Function getSQL4FST4Adr() As String
        Dim sql As String
        Dim fs As String
        l("getflurstueck---------------------- anfang")
        'innerSQL = String.Format("SELECT geom  " &
        '                         " FROM flurkarte.halofs where gemeindenr={0} AND strcode={1} AND Hausnr='{2}' and lower(zusatz)='{3}'",
        '                      myglobalz.aktadr.Gisadresse.gemeindeNrBig, myglobalz.aktadr.Gisadresse.strasseCode, myglobalz.aktadr.Gisadresse.hausNr,
        '                      myglobalz.aktadr.Gisadresse.hausZusatz.ToLower.Trim)
        fs = String.Format("SELECT fs  " &
                                 " FROM flurkarte.halofs where gemeindenr={0} AND strcode={1} AND Hausnr='{2}' and lower(zusatz)='{3}'",
                              myglobalz.aktadr.Gisadresse.gemeindeNrBig, myglobalz.aktadr.Gisadresse.strasseCode, myglobalz.aktadr.Gisadresse.hausNr,
                              myglobalz.aktadr.Gisadresse.hausZusatz.ToLower.Trim)
        l(fs)
        'sql = "SELECT * " &
        '            "  FROM flurkarte.basis_f " &
        '            "  WHERE ST_contains(flurkarte.basis_f.geom,(" & innerSQL & ")" & ");"
        sql = "SELECT * " &
                    "  FROM flurkarte.basis_f " &
                    "  WHERE fs in (" & fs & ")" & ";"
        Return sql
    End Function
    Public Shared Function getflurstueckDB(aktADR As clsAdress, sql As String) As DataTable
        'koordinate der adresse aus halo holen
        'Dim innerSQL As String
        Try
            l("getflurstueck---------------------- anfang")
            'innerSQL = String.Format("SELECT geom  " &
            '                     " FROM public.halofs where gemeindenr={0} AND strcode={1} AND Hausnr='{2}' and lower(zusatz)='{3}'",
            '                  aktADR.gemeindeNrBig, aktADR.strasseCode, aktADR.hausNr,
            '                  aktADR.hausZusatz.ToLower.Trim)
            'l(innerSQL)
            'Dim sql = "SELECT * " &
            '            "  FROM flurkarte.basis_f " &
            '            "  WHERE ST_contains(flurkarte.basis_f.geom,(" & innerSQL & ")" & ");"
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

    Public Shared Function mapFlurstueckDB(ByVal fsDT As DataTable, ByVal clsFlurstueck As clsFlurstueck, ByRef anzahl As Integer
                            ) As String
        Dim summentext As String
        Try
            l(" mapFlurstueck ---------------------- anfang")
            If fsDT.Rows.Count < 1 Then
                MsgBox("UUUPS: Es ist hier kein Flurstück zugeordnet. Daher ist keine Eigentümerabfrage möglich. Bitte informieren Sie den Admin! (Tel. 4434). Programm wird beendet!")
                summentext = "UUUPS: Es ist hier kein Flurstück zugeordnet. Daher ist keine Eigentümerabfrage möglich. Bitte informieren Sie den Admin! (Tel. 4434)"
                anzahl = 0
                Return summentext
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
            anzahl = fsDT.Rows.Count
            Return summentext
        Catch ex As Exception
            l("Fehler in mapFlurstueck: " & ex.ToString())
            anzahl = 0
            Return ""
        End Try
    End Function

    Friend Shared Function getflurstueckajax(result As String, clsFlurstueck As clsFlurstueck, ByRef anzahl As Integer) As String
        Dim izeilen, ispalten As Integer
        Dim recs(), cols() As String
        Dim lok As New List(Of clsFlurauswahl)
        Dim strasse As New clsFlurauswahl
        Dim oldname As String = ""
        Dim summentext As New Text.StringBuilder
        Try
            l(" getflurstueckajax html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in getflurstueckajax: " & result)
                Return Nothing
            End If
            recs = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : izeilen = recs.Count
            cols = recs(0).Split("#"c) : ispalten = cols.Count
            clsFlurstueck.gemcode = CInt(cols(4))
            clsFlurstueck.flur = CInt(cols(5))
            clsFlurstueck.zaehler = CInt(cols(6))
            clsFlurstueck.nenner = CInt(cols(7))
            Dim a As String = clsDBtools.fieldvalue(cols(20)).Trim.Replace(".", ",")
            clsFlurstueck.flaecheqm = CInt(a)
            clsFlurstueck.istgebucht = CStr(cols(13))
            clsFlurstueck.FS = CStr(cols(2))
            Dim gemparms As New clsGemarkungsParams
            clsFlurstueck.gemarkungstext = gemparms.gemcode2gemarkungstext(clsFlurstueck.gemcode)
            '   gemparms = Nothing

            For i = 0 To izeilen - 1
                cols = recs(i).Split("#"c)
                Dim gema = (cols(4))
                gema = (gemparms.gemcode2gemarkungstext(CInt(gema)))

                summentext.Append(gema & ", ")
                summentext.Append(cols(5) & ", ")
                summentext.Append(cols(6) & "/")
                summentext.Append(cols(7))
                'summentext.Append(recs(i))

                'fst = fst & clsFlurstueck.gemarkungstext & ", "
                'fst = fst & CInt(clsDBtools.fieldvalue(fsDT.Rows(i).Item("flur"))) & ", "
                'fst = fst & CInt(clsDBtools.fieldvalue(fsDT.Rows(i).Item("zaehler"))) & "/"
                'fst = fst & CInt(clsDBtools.fieldvalue(fsDT.Rows(i).Item("nenner"))) & " "
                'summentext = summentext & " " & fst & ", "
            Next
            l(" getflurstueckajax ---------------------- ende")
            Return summentext.ToString
        Catch ex As Exception
            l("Fehler in getflurstueckajax: " & ex.ToString())
            Return Nothing
        End Try
    End Function
End Class
