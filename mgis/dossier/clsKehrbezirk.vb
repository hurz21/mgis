Imports mgis

Public Class clsKehrbezirk
    Friend Shared Function getKehrbezirkInfo4point(winpt As myPoint, ByRef rESULT_text_kehr As String,
                                                   ByRef kehrbezirk As String,
                                                   ByVal schematabelleGeo As String,
                                                   ByVal schematabelleAtt As String) As Boolean

        Return getKehrbezirkInfo4AdressExtracted(rESULT_text_kehr, kehrbezirk, schematabelleGeo, schematabelleAtt, winpt)
    End Function
    Private Shared Function getKehrbezirkInfo4AdressExtracted(ByRef RESULT_text As String,
                                                              ByRef kehrbezirk As String,
                                                              ByVal schematabelleGeo As String,
                                                              ByVal schematabelleAtt As String,
                                                              winpt As myPoint) As Boolean
        Dim dt As System.Data.DataTable
        Dim sql As String
        Try
            l(" getKehrbezirkInfo4AdressExtracted ---------------------- anfang")
            dt = clsDossier.getDtHauptabfrageFlaeche(winpt, schematabelleGeo)
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein kehrbezirk")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("name")).ToString.Trim
                l("pdf=" & clsDBtools.fieldvalue(dt.Rows(0).Item("name")).ToString)
                SQL = "SELECT * FROM " & schematabelleAtt & " where name='" & aid & "'" '
                l("sql: " & SQL)
                dt = getDTFromWebgisDB(SQL, "postgis20")
                l("Anzahl=" & dt.Rows.Count)
                kehrbezirk = clsDBtools.fieldvalue(dt.Rows(0).Item("AID")).ToString.Trim
                RESULT_text = fegerinfo(dt)
                Return True
            End If
            l(" getKehrbezirkInfo4AdressExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getKehrbezirkInfo4AdressExtracted: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Shared Function fegerinfo(dt As System.Data.DataTable) As String
        Dim trenn As String = " " & Environment.NewLine
        Dim resulttext As String = ""
        Try
            l(" fegerinfo ---------------------- anfang")
            resulttext = resulttext & "Kehrbezirk: " & clsDBtools.fieldvalue(dt.Rows(0).Item("AID")).ToString.Trim & trenn
            resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("Nachname")).ToString.Trim & " " & clsDBtools.fieldvalue(dt.Rows(0).Item("vorname")).ToString.Trim & ", "
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("vorname")).ToString.Trim & trenn
            resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("strasse")).ToString.Trim & " " & clsDBtools.fieldvalue(dt.Rows(0).Item("hnr")).ToString.Trim & " " & clsDBtools.fieldvalue(dt.Rows(0).Item("plz")).ToString.Trim & " " & clsDBtools.fieldvalue(dt.Rows(0).Item("ort")).ToString.Trim & trenn
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("ort")).ToString.Trim & trenn
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("hnr")).ToString.Trim & trenn
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("plz")).ToString.Trim & " " & clsDBtools.fieldvalue(dt.Rows(0).Item("ort")).ToString.Trim & trenn
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("ort")).ToString.Trim & trenn
            resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("tel")).ToString.Trim & ", "
            resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("mobil")).ToString.Trim & ", "
            resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("email")).ToString.Trim & ", "
            resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("bemerkung")).ToString.Trim & " "
            Return resulttext
            l(" fegerinfo ---------------------- ende")
        Catch ex As Exception
            l("Fehler in fegerinfo: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
