Imports mgis

Public Class clsKehrbezirk
    Friend Shared Function getKehrbezirkInfo4point(winpt As myPoint, item As clsDossierItem,
                                                   ByVal schematabelleAtt As String,
                                                   ByRef strError As String) As Boolean

        Return getKehrbezirkInfo4AdressExtracted(item,
                                                 schematabelleAtt, winpt, strError)
    End Function
    Private Shared Function getKehrbezirkInfo4AdressExtracted(item As clsDossierItem,
                                                              ByVal schematabelleAtt As String,
                                                              winpt As myPoint, ByRef strError As String) As Boolean
        Dim dt As System.Data.DataTable
        Dim sql As String
        Try
            l(" getKehrbezirkInfo4AdressExtracted ---------------------- anfang")
            dt = clsDossier.getDtHauptabfrageFlaeche(winpt, item.schematabelle, strError)
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein kehrbezirk")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                l("pdf=" & clsDBtools.fieldvalue(dt.Rows(0).Item("kehrbezirk")).ToString)
                sql = "SELECT * FROM " & schematabelleAtt & " where gid=" & aid & "" '
                l("sql: " & sql)
                dt = clsPgtools.getDTFromWebgisDB(sql, "postgis20", strError)
                l("Anzahl=" & dt.Rows.Count)
                item.kurz = clsDBtools.fieldvalue(dt.Rows(0).Item("kehrbezirk")).ToString.Trim
                item.result = fegerinfo(dt)
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
            resulttext = resulttext & "Kehrbezirk: " & clsDBtools.fieldvalue(dt.Rows(0).Item("kehrbezirk")).ToString.Trim & trenn
            resulttext = resulttext & " - " & clsDBtools.fieldvalue(dt.Rows(0).Item("Nachname")).ToString.Trim & " " & clsDBtools.fieldvalue(dt.Rows(0).Item("vorname")).ToString.Trim & ", "
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("vorname")).ToString.Trim & trenn
            resulttext = resulttext & " - " & clsDBtools.fieldvalue(dt.Rows(0).Item("strasse")).ToString.Trim & " " &
                clsDBtools.fieldvalue(dt.Rows(0).Item("hnr")).ToString.Trim & " " &
                clsDBtools.fieldvalue(dt.Rows(0).Item("plz")).ToString.Trim & " " &
                clsDBtools.fieldvalue(dt.Rows(0).Item("ort")).ToString.Trim & trenn
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("ort")).ToString.Trim & trenn
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("hnr")).ToString.Trim & trenn
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("plz")).ToString.Trim & " " & clsDBtools.fieldvalue(dt.Rows(0).Item("ort")).ToString.Trim & trenn
            'resulttext = resulttext & clsDBtools.fieldvalue(dt.Rows(0).Item("ort")).ToString.Trim & trenn
            resulttext = resulttext & " - " & clsDBtools.fieldvalue(dt.Rows(0).Item("fax")).ToString.Trim & ", "
            resulttext = resulttext & " - " & clsDBtools.fieldvalue(dt.Rows(0).Item("tel")).ToString.Trim & ", "
            resulttext = resulttext & " - " & clsDBtools.fieldvalue(dt.Rows(0).Item("mobil")).ToString.Trim & ", "
            resulttext = resulttext & " - " & clsDBtools.fieldvalue(dt.Rows(0).Item("email")).ToString.Trim & ", "
            resulttext = resulttext & " - " & clsDBtools.fieldvalue(dt.Rows(0).Item("bemerkung")).ToString.Trim & " "
            Return resulttext
            l(" fegerinfo ---------------------- ende")
        Catch ex As Exception
            l("Fehler in fegerinfo: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
