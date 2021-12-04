Imports System.Data

Public Class clsBplan
    Public Shared bplanroot As String = m.appServerUnc & "\fkat\bplan"
    Shared Function getBplanInfo4point(winpt As myPoint, ByRef dt As DataTable, schematabelle As String, ByRef strError As String) As Boolean
        Try
            l(" getBplanInfo4point ---------------------- anfang")
            'schematabelle = "muell"
            clsDossier.Question(winpt, dt, schematabelle, strError)
            If strError.ToLower.StartsWith("fehler") Then
                l(" getBplanInfo4point ---------------------- ende" & strError)
                Return False
            Else
                If dt.Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If

            End If
            'Return getBplanInfoErmitteln(winpt, dt, schematabelle)

            l(" getBplanInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getBplanInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function


    'Private Shared Function getBplanInfoErmitteln(winpt As myPoint, ByRef dt As DataTable, schematabelle As String) As Boolean
    '    Dim innerSQL As String
    '    innerSQL = "select SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ")," &
    '           clsStartup.PostgisDBcoordinatensystem.ToString & ")"
    '    l(innerSQL)
    '    'Dim dt As DataTable
    '    Dim SQL = "SELECT *  " &
    '                "  FROM " & schematabelle & " " &
    '                "  WHERE ST_contains(ST_CurveToLine( " & schematabelle & ".geom),(" & innerSQL & "  )" & "  );"
    '    l("sql: " & SQL)
    '    Try
    '        l(" getBplanInfoErmitteln ---------------------- anfang")
    '        dt = clsPgtools.getDTFromWebgisDB(SQL, "postgis20")
    '        If dt.Rows.Count < 1 Then
    '            l("kein bplan")
    '            Return False
    '        Else
    '            l(" getBplanInfoErmitteln ---------------------- ende")
    '            Return True
    '        End If
    '        l(" getBplanInfoErmitteln fehler ende")
    '        Return True
    '    Catch ex As Exception
    '        l("Fehler in getBplanInfoErmitteln: " & ex.ToString())
    '        Return False
    '    End Try
    'End Function

    Shared Function BplanPDFanbieten(dt As DataTable, index As Integer, ByRef RESULT_dateien As List(Of clsGisresult), ByRef plannr As String) As Boolean
        Try
            l(" BplanPDFanbieten ---------------------- anfang")
            If Not String.IsNullOrEmpty(clsDBtools.fieldvalue(dt.Rows(index).Item("pdf")).ToString) Then
                l("PDF ist vorhanden")
                'prüfen ob zusatzDokus vorhanden sind
                Dim winpfad_start As String = ""
                Dim winpfad As String = ""
                Dim relativpfad As String = ""

                winpfad_start = GetWinpfad_start(bplanroot, dt, index, winpfad, relativpfad)
                Dim newgis As New clsGisresult
                Dim fi As New IO.FileInfo(winpfad_start)
                newgis.datei = fi
                newgis.dateibeschreibung = "Bplan: " & clsDBtools.fieldvalue(dt.Rows(index).Item("pdf")).ToString.Trim
                newgis.begleitdateien = getBegleitplanFileliste(clsDBtools.fieldvalue(dt.Rows(index).Item("pdf")).ToString.Trim,
                                                                bplanroot & relativpfad)
                RESULT_dateien.Add(newgis)
                plannr = clsDBtools.fieldvalue(dt.Rows(index).Item("pdf")).ToString.Trim
                Return True
            End If
            l(" BplanPDFanbieten ---------------------- ende")
            Return False
        Catch ex As Exception
            l("Fehler in BplanPDFanbieten: " & ex.ToString())
            Return False
        End Try
    End Function

    Shared Function getBegleitplanFileliste(pdf As String, verzeichnis As String) As List(Of IO.FileInfo)
        Dim di As New IO.DirectoryInfo(verzeichnis)
        Dim templiste As IO.FileInfo()
        Dim ausschluss As String
        Dim begleitfilelist = New List(Of IO.FileInfo)
        Try
            l("getBegleitplanFileliste---------------------- anfang")
            templiste = di.GetFiles("*.pdf")
            Dim dra As IO.FileInfo
            ausschluss = pdf & ".pdf"
            'list the names of all files in the specified directory
            For Each dra In templiste
                Debug.Print(dra.ToString)
                If ausschluss <> dra.Name.ToLower Then
                    begleitfilelist.Add(dra)
                End If
            Next
            Return begleitfilelist
            l("getBegleitplanFileliste---------------------- ende")
        Catch ex As Exception
            l("Fehler in getBegleitplanFileliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Shared Function GetWinpfad_start(bplanroot As String, dt As DataTable, ByVal Index As Integer,
                                     ByRef winpfad As String,
                                     ByRef relativpfad As String) As String
        Try
            l(" GetWinpfad_start ---------------------- anfang")
            relativpfad = CStr(dt.Rows(Index).Item("gemarkung")).Trim & "\" &
                            CStr(dt.Rows(Index).Item("pdf")).Trim & "\" '&
            'CStr(dt.Rows(i).Item("pdf")).Trim
            l("fkatroot: " & bplanroot)

            winpfad = (bplanroot & relativpfad).ToLower & CStr(dt.Rows(Index).Item("pdf")).Trim & ".pdf"
            l("relativpfad$: " & relativpfad)

            'winpfad = (fkatroot & relativpfad).ToLower & ".pdf"
            l("winpfad: " & winpfad)
            l(" GetWinpfad_start ---------------------- ende")
            Return winpfad
        Catch ex As Exception
            l("Fehler in GetWinpfad_start: " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Sub hurz(dt As DataTable,
                           ByRef rESULT_text_Bplan As String,
                           ByRef summeBplanr() As String,
                           ByRef RESULT_dateien As List(Of clsGisresult),
                           ByRef bplankurzliste As String)
        Dim lResult As Boolean = False
        Dim tempBplanr As String = ""
        Dim anzahlplaene As String = ""
        Try
            l(" hurz ---------------------- anfang")
            If dt.Rows.Count > 1 Then
                anzahlplaene = "An diesem Punkt gelten " & dt.Rows.Count & " Bebauungspläne ! " & Environment.NewLine
                ReDim summeBplanr(dt.Rows.Count - 1)
                rESULT_text_Bplan = rESULT_text_Bplan.Trim & anzahlplaene & Environment.NewLine
            End If
            For i = 0 To dt.Rows.Count - 1
                l("pdf=" & clsDBtools.fieldvalue(dt.Rows(i).Item("pdf")).ToString)
                Debug.Print(" " & clsDBtools.fieldvalue(dt.Rows(i).Item("pdf")).ToString)
                lResult = BplanPDFanbieten(dt, i, RESULT_dateien, tempBplanr)
                summeBplanr(i) = tempBplanr
                rESULT_text_Bplan = rESULT_text_Bplan.Trim & Environment.NewLine &
                                  buildBplanresulttext(dt.Rows, i) & Environment.NewLine
                bplankurzliste = bplankurzliste & " " & clsDBtools.fieldvalue(dt.Rows(i).Item("nr")).Trim
            Next
            l(" hurz ---------------------- ende")
        Catch ex As Exception
            l("Fehler in hurz: " & ex.ToString())
        End Try
    End Sub

    Private Shared Function buildBplanresulttext(drc As DataRowCollection, index As Integer) As String
        Dim sb As New Text.StringBuilder
        Dim trenn As String = " " & Environment.NewLine
        Try
            l(" buildBplanresulttext ---------------------- anfang")
            sb.Append("B-Pläne -----------------------------------------------" & Environment.NewLine)
            sb.Append("Bebauungsplan Nr: " & clsDBtools.fieldvalue(drc.Item(index).Item("nr")).Trim & trenn)
            sb.Append("-------------------" & trenn)
            sb.Append(clsDBtools.fieldvalue(drc.Item(0).Item("titel")).Trim & trenn)
            sb.Append(" Gemeinde: " & clsDBtools.fieldvalue(drc.Item(index).Item("gemeinde")).Trim & trenn)
            sb.Append(" Gemarkung: " & clsDBtools.fieldvalue(drc.Item(index).Item("gemarkung")).Trim & trenn)
            sb.Append(" Baul.Nutz.: " & clsDBtools.fieldvalue(drc.Item(index).Item("baulnutz")).Trim & trenn)
            sb.Append(" Aufstellung: " & clsDBtools.fieldvalue(drc.Item(index).Item("aufstellun")).Trim & trenn)
            sb.Append(" Rechtswirksam: " & clsDBtools.fieldvalue(drc.Item(index).Item("rechtswirk")).Trim & trenn)
            sb.Append(clsDBtools.fieldvalue(drc.Item(index).Item("bemerkung")).Trim & trenn)
            If clsDBtools.fieldvalue(drc.Item(index).Item("wird_ueber")).ToString.Trim.Length > 0 Then
                sb.Append(" Achtung B-Plan wird überlagert von B-Plan" & clsDBtools.fieldvalue(drc.Item(index).Item("wird_ueber")).Trim & trenn)
            End If
            sb.Append(" Fläche[qm]: " & clsDBtools.fieldvalue(drc.Item(index).Item("flaeche_qm")).Trim & trenn)
            Return sb.ToString.Trim
            l(" buildBplanresulttext ---------------------- ende")
        Catch ex As Exception
            l("Fehler in buildBplanresulttext: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
