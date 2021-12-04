
Imports System.Data
Public Class clsFoerder
    Friend Shared Function getInfo4point(winpt As myPoint, dossieritem As clsDossierItem, ByRef strError As String) As Boolean
        Dim resulttext As String = ""
        Dim dt As DataTable
        Try
            l(" getUEBKROFInfo4point ---------------------- anfang" & dossieritem.schematabelle)
            dt = getExtracted(dossieritem, winpt, strError)
            If dt.IsNothingOrEmpty Then

                Return False

            End If
            Dim aid As String = clsDBtools.fieldvalue(dt.Rows(0).Item("gid")).ToString.Trim

            For i = 0 To dt.Rows.Count - 1
                dossieritem.kurz = dossieritem.kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("fo_id")).Trim
                dossieritem.datei = clsDBtools.fieldvalue(dt.Rows(i).Item("projekt_ID")).Trim
                dossieritem.link = clsDBtools.fieldvalue(dt.Rows(i).Item("aid")).Trim
            Next
            dossieritem.result = bildeINFO(dt, strError)

            l(" getUEBKROFInfo4point ---------------------- ende" & dossieritem.schematabelle)
            Return True
        Catch ex As Exception
            l("Fehler in getUEBKROFInfo4point: " & dossieritem.schematabelle & ex.ToString())
            Return False
        End Try
    End Function
    Private Shared Function getExtracted(item As clsDossierItem, winpt As myPoint, ByRef strError As String) As DataTable
        Dim dt As System.Data.DataTable
        l("getExtracted ---------------------- " & item.schematabelle)
        item.kurz = "" : item.datei = ""
        clsDossier.Question(winpt, dt, item.schematabelle, strError)
        If strError.ToLower.StartsWith("fehler") Then
            l(" nach question ---------------------- ende" & strError)
            Return Nothing
        End If
        Try
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                l("keine items")
                Return Nothing
            Else
                Return dt
            End If
            l(" getExtracted ---------------------- ende" & item.schematabelle)
            Return dt
        Catch ex As Exception
            l("Fehler in getExtracted: " & item.schematabelle & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Shared Function bildeINFO(DataRow As DataTable, ByRef strError As String) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = ", " & Environment.NewLine
            If DataRow.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            End If
            For i = 0 To DataRow.Rows.Count - 1
                'summe = summe & " ----------------------------------- " & trenn
                summe = summe & Environment.NewLine
                summe = summe & "Maßnahme: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("fo_id").ToString.Trim & trenn)
                summe = summe & "ProjektID: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("projekt_id").ToString.Trim & trenn)
                summe = summe & "Fläche: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("aid").ToString.Trim & trenn)

                Dim Sql = "SELECT *  FROM public.dossier_foe_projekt_a  WHERE  projekt_id=" &
                    clsDBtools.fieldvalue(DataRow.Rows(i).Item("projekt_id").ToString.Trim) & ";"
                summe = summe & getPProjektInfo(Sql, strError)

                Sql = "SELECT *  FROM  public.dossier_foe_massnahme_a  WHERE  aid=" &
                    clsDBtools.fieldvalue(DataRow.Rows(i).Item("aid").ToString.Trim) & ";"

                summe = summe & getMassnahme(Sql, strError)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeIllegaleINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function

    Private Shared Function getMassnahme(sql As String, ByRef strError As String) As String
        Dim result As String = ""
        Dim trenn As String = Environment.NewLine
        Dim dt As DataTable
        l(" getMassnahmeInfo ---------------------- anfang " & sql)
        Try
            l("sql: " & sql)
            l(" getDtHauptabfrageFlaeche ---------------------- anfang")
            dt = clsPgtools.getDTFromWebgisDB(sql, "postgis20", strError)
            If dt.Rows.Count < 1 Then Return ""
            l(" getMassnahmeInfo ---------------------- ende")
            result = result & "Maßnahme-------------" & trenn
            result = result & "- projektnummer: " & clsDBtools.fieldvalue(dt.Rows(0).Item("projektnummer").ToString.Trim & trenn)
            result = result & "- massnahme: " & clsDBtools.fieldvalue(dt.Rows(0).Item("massnahme").ToString.Trim & trenn)
            result = result & "- massnahme_beschr: " & clsDBtools.fieldvalue(dt.Rows(0).Item("massnahme_beschr").ToString.Trim & trenn)
            result = result & "- datum_aenderung: " & clsDBtools.fieldvalue(dt.Rows(0).Item("datum_aenderung").ToString.Trim & trenn)
            result = result & "- last_user: " & clsDBtools.fieldvalue(dt.Rows(0).Item("last_user").ToString.Trim & trenn)
            Return result
        Catch ex As Exception
            l("Fehler in getDtHauptabfrageFlaeche: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function getPProjektInfo(sql As String, ByRef strError As String) As String
        Dim result As String = ""
        Dim trenn As String = Environment.NewLine
        Dim dt As DataTable
        l(" getMassnahmeInfo ---------------------- anfang " & sql)
        Try
            l("sql: " & sql)
            l(" getDtHauptabfrageFlaeche ---------------------- anfang")
            dt = clsPgtools.getDTFromWebgisDB(sql, "postgis20", strError)
            If dt.Rows.Count < 1 Then Return ""
            l(" getMassnahmeInfo ---------------------- ende")
            result = result & "Projekt-------------" & trenn
            result = result & "- foerderungsart: " & clsDBtools.fieldvalue(dt.Rows(0).Item("foerderungsart").ToString.Trim & trenn)
            result = result & "- antragsteller: " & clsDBtools.fieldvalue(dt.Rows(0).Item("antragsteller").ToString.Trim & trenn)
            result = result & "- foerderprojekt: " & clsDBtools.fieldvalue(dt.Rows(0).Item("foerderprojekt").ToString.Trim & trenn)
            result = result & "- sachstand: " & clsDBtools.fieldvalue(dt.Rows(0).Item("sachstand").ToString.Trim & trenn)
            result = result & "- bemerkungen: " & clsDBtools.fieldvalue(dt.Rows(0).Item("bemerkungen").ToString.Trim & trenn)
            Return result
        Catch ex As Exception
            l("Fehler in getDtHauptabfrageFlaeche: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class


