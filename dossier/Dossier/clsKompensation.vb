Imports System.Data

Public Class clsKompensation
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
    Private Shared Function getExtracted(item As clsDossierItem, winpt As myPoint,
                                         ByRef strError As String) As Boolean
        Dim dt As System.Data.DataTable
        l("getBaulastenExtracted ---------------------- " & item.schematabelle)
        item.kurz = "" : item.datei = ""
        clsDossier.Question(winpt, dt, item.schematabelle, strError)
        If strError.ToLower.StartsWith("fehler") Then
            l(" nach question ---------------------- ende" & strError)
            Return False
        End If
        Dim summe As String = ""
        Try
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("kein nsg")
                Return False
            Else
                Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim
                For i = 0 To dt.Rows.Count - 1
                    item.kurz = item.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("fo_id")).Trim
                    item.link = item.link & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("fo_komp_id")).Trim
                    item.datei = item.datei & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("fo_kmas_id")).Trim

                    summe &= "Beantragt von: " & clsDBtools.fieldvalue(dt.Rows(i).Item("beantragt_von")).Trim & Environment.NewLine
                    If clsDBtools.fieldvalue(dt.Rows(i).Item("kommunal")) = "1" Then
                        summe &= "Ist KOMMUNAL !" & Environment.NewLine
                        item.kurz = item.kurz & ". Ist KOMMUNAL !"
                    End If
                    'pdf = clsDBtools.fieldvalue(dt.Rows(i).Item("tiff")).Trim
                    'wsgpdf = wsgpdf & "," & datei
                Next

            item.result = summe & bildeINFO(dt, strError)
            Return True
            End If
            l(" getBaulastenExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getBaulastenExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildeINFO(DataRow As System.Data.DataTable, ByRef strError As String) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = ", " & Environment.NewLine
            If DataRow.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            End If
            For i = 0 To DataRow.Rows.Count - 1
                'summe = summe & " ----------------------------------- " & trenn
                summe = summe & Environment.NewLine
                'summe = summe & "id: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("fo_id").ToString.Trim & trenn)
                'summe = summe & "komp: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("fo_komp_id").ToString.Trim & trenn)
                'summe = summe & "massnahme: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("fo_kmas_id").ToString.Trim & trenn)

                Dim Sql = "SELECT *  FROM foerder.kom_verfahren_a  WHERE  fo_komp_id=" &
                    clsDBtools.fieldvalue(DataRow.Rows(i).Item("fo_komp_id").ToString.Trim) & ";"
                summe = summe & getVerfahrennfo(Sql, strError)

                Sql = "SELECT *  FROM  foerder.kom_massnahme_a  WHERE  fo_kmas_id=" &
                    clsDBtools.fieldvalue(DataRow.Rows(i).Item("fo_kmas_id").ToString.Trim) & ";"

                summe = summe & getMassnahme(Sql, strError)
                Sql = "SELECT *  FROM  foerder.kom_flaechen_a  WHERE  fo_kmas_id=" &
          clsDBtools.fieldvalue(DataRow.Rows(i).Item("fo_kmas_id").ToString.Trim) & ";"

                summe = summe & getflaechen(Sql, strError)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeBaulastenINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
    Private Shared Function getflaechen(sql As String, ByRef strError As String) As String
        Dim result As String = ""
        Dim trenn As String = Environment.NewLine
        Dim dt As DataTable
        l(" getverfahrenInfo ---------------------- anfang " & sql)
        Try
            l("sql: " & sql)
            l(" getDtHauptabfrageFlaeche ---------------------- anfang")
            dt = clsPgtools.getDTFromWebgisDB(sql, "postgis20", strError)

            If dt.Rows.Count < 1 Then Return ""
            l(" getMassnahmeInfo ---------------------- ende")
            result = result & "Flächen (Flurstücke)-------------" & trenn
            result = result & "- Kurzform: " & clsDBtools.fieldvalue(dt.Rows(0).Item("fs").ToString.Trim & trenn)
            result = result & "- teilweise: " & clsDBtools.fieldvalue(dt.Rows(0).Item("teilweise").ToString.Trim & trenn)
            result = result & "- dingliche_sicherung: " & clsDBtools.fieldvalue(dt.Rows(0).Item("dingliche_sicherung").ToString.Trim & trenn)
            result = result & "- eigentuemer: " & clsDBtools.fieldvalue(dt.Rows(0).Item("eigentuemer").ToString.Trim & trenn)
            result = result & "- datum: " & clsDBtools.fieldvalue(dt.Rows(0).Item("datum").ToString.Trim & trenn)
            result = result & "- zu_gunsten: " & clsDBtools.fieldvalue(dt.Rows(0).Item("zu_gunsten").ToString.Trim & trenn)
            result = result & "- last_edit: " & clsDBtools.fieldvalue(dt.Rows(0).Item("last_edit").ToString.Trim & trenn)
            result = result & "- last_user: " & clsDBtools.fieldvalue(dt.Rows(0).Item("last_user").ToString.Trim & trenn)
            Return result
        Catch ex As Exception
            l("Fehler in getDtHauptabfrageFlaeche: " & ex.ToString())
            Return ""
        End Try
    End Function
    Private Shared Function getMassnahme(sql As String, ByRef strError As String) As String
        Dim result As String = ""
        Dim trenn As String = Environment.NewLine
        Dim dt As DataTable
        l(" getverfahrenInfo ---------------------- anfang " & sql)
        Try
            l("sql: " & sql)
            l(" getDtHauptabfrageFlaeche ---------------------- anfang")
            dt = clsPgtools.getDTFromWebgisDB(sql, "postgis20", strError)

            If dt.Rows.Count < 1 Then Return ""
            l(" getMassnahmeInfo ---------------------- ende")
            result = result & "Maßnahme-------------" & trenn
            result = result & "- bezeichnung: " & clsDBtools.fieldvalue(dt.Rows(0).Item("bezeichnung").ToString.Trim & trenn)
            result = result & "- massnahmenart: " & clsDBtools.fieldvalue(dt.Rows(0).Item("massnahmenart").ToString.Trim & trenn)
            result = result & "- kompensationsart: " & clsDBtools.fieldvalue(dt.Rows(0).Item("kompensationsart").ToString.Trim & trenn)
            result = result & "- stadt_gemeinde: " & clsDBtools.fieldvalue(dt.Rows(0).Item("stadt_gemeinde").ToString.Trim & trenn)
            result = result & "- status_rb: " & clsDBtools.fieldvalue(dt.Rows(0).Item("status_rb").ToString.Trim & trenn)
            result = result & "- datum: " & clsDBtools.fieldvalue(dt.Rows(0).Item("datum").ToString.Trim & trenn)
            result = result & "- sachstand: " & clsDBtools.fieldvalue(dt.Rows(0).Item("sachstand").ToString.Trim & trenn)
            result = result & "- anlage_jahr: " & clsDBtools.fieldvalue(dt.Rows(0).Item("anlage_jahr").ToString.Trim & trenn)
            result = result & "- flaeche_mass: " & clsDBtools.fieldvalue(dt.Rows(0).Item("flaeche_mass").ToString.Trim & trenn)
            result = result & "- genauigkeit: " & clsDBtools.fieldvalue(dt.Rows(0).Item("genauigkeit").ToString.Trim & trenn)
            result = result & "- bestandskontrolle: " & clsDBtools.fieldvalue(dt.Rows(0).Item("bestandskontrolle").ToString.Trim & trenn)
            result = result & "- notiz_bk: " & clsDBtools.fieldvalue(dt.Rows(0).Item("notiz_bk").ToString.Trim & trenn)
            result = result & "- datum_bk: " & clsDBtools.fieldvalue(dt.Rows(0).Item("datum_bk").ToString.Trim & trenn)
            result = result & "- ausfuehrungskontrolle: " & clsDBtools.fieldvalue(dt.Rows(0).Item("ausfuehrungskontrolle").ToString.Trim & trenn)
            result = result & "- notiz_ak: " & clsDBtools.fieldvalue(dt.Rows(0).Item("notiz_ak").ToString.Trim & trenn)
            result = result & "- datum_ak: " & clsDBtools.fieldvalue(dt.Rows(0).Item("datum_ak").ToString.Trim & trenn)
            result = result & "- nachkontrolle: " & clsDBtools.fieldvalue(dt.Rows(0).Item("nachkontrolle").ToString.Trim & trenn)
            result = result & "- notiz_nk: " & clsDBtools.fieldvalue(dt.Rows(0).Item("notiz_nk").ToString.Trim & trenn)
            result = result & "- datum_nk: " & clsDBtools.fieldvalue(dt.Rows(0).Item("datum_nk").ToString.Trim & trenn)
            result = result & "- erfolgskontrolle: " & clsDBtools.fieldvalue(dt.Rows(0).Item("erfolgskontrolle").ToString.Trim & trenn)
            result = result & "- notiz_ek: " & clsDBtools.fieldvalue(dt.Rows(0).Item("notiz_ek").ToString.Trim & trenn)
            result = result & "- datum_ek: " & clsDBtools.fieldvalue(dt.Rows(0).Item("datum_ek").ToString.Trim & trenn)

            result = result & "- naturraum: " & clsDBtools.fieldvalue(dt.Rows(0).Item("naturraum").ToString.Trim & trenn)
            result = result & "- massnahmentraeger: " & clsDBtools.fieldvalue(dt.Rows(0).Item("massnahmentraeger").ToString.Trim & trenn)
            result = result & "- bemerkungen_anlage: " & clsDBtools.fieldvalue(dt.Rows(0).Item("bemerkungen_anlage").ToString.Trim & trenn)
            result = result & "- bemerkungen_pflege: " & clsDBtools.fieldvalue(dt.Rows(0).Item("bemerkungen_pflege").ToString.Trim & trenn)
            result = result & "- datum_aenderung: " & clsDBtools.fieldvalue(dt.Rows(0).Item("datum_aenderung").ToString.Trim & trenn)


            Return result
        Catch ex As Exception
            l("Fehler in getDtHauptabfrageFlaeche: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function getVerfahrennfo(sql As String, ByRef strError As String) As String
        Dim result As String = ""
        Dim trenn As String = Environment.NewLine
        Dim dt As DataTable
        l(" getverfahrenInfo ---------------------- anfang " & sql)
        Try
            l("sql: " & sql)
            l(" getDtHauptabfrageFlaeche ---------------------- anfang")
            dt = clsPgtools.getDTFromWebgisDB(sql, "postgis20", strError)

            If dt.Rows.Count < 1 Then Return ""
            l(" getMassnahmeInfo ---------------------- ende")
            result = result & "Verfahren-------------" & trenn
            result = result & "- Verfahren: " & clsDBtools.fieldvalue(dt.Rows(0).Item("verfahren").ToString.Trim & trenn)
            result = result & "- Hier: " & clsDBtools.fieldvalue(dt.Rows(0).Item("hier").ToString.Trim & trenn)
            result = result & "- beantragt_von: " & clsDBtools.fieldvalue(dt.Rows(0).Item("beantragt_von").ToString.Trim & trenn)
            result = result & "- Eingriffstyp: " & clsDBtools.fieldvalue(dt.Rows(0).Item("eingriffstyp").ToString.Trim & trenn)
            result = result & "- Gesamtfläche: " & clsDBtools.fieldvalue(dt.Rows(0).Item("gesamtflaeche").ToString.Trim & trenn)
            result = result & "- Zust_Behörde: " & clsDBtools.fieldvalue(dt.Rows(0).Item("zust_behoerde").ToString.Trim & trenn)
            result = result & "- Bescheid_Behörde: " & clsDBtools.fieldvalue(dt.Rows(0).Item("bescheid_behoerde").ToString.Trim & trenn)
            result = result & "- Datum_Bescheid: " & clsDBtools.fieldvalue(dt.Rows(0).Item("Datum_Bescheid").ToString.Trim & trenn)
            result = result & "- Notiz: " & clsDBtools.fieldvalue(dt.Rows(0).Item("notiz").ToString.Trim & trenn)
            result = result & "- Az.: " & clsDBtools.fieldvalue(dt.Rows(0).Item("az").ToString.Trim & trenn)



            Return result
        Catch ex As Exception
            l("Fehler in getDtHauptabfrageFlaeche: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
