Imports System.Data

Public Class clsIllegaleNeu
    Friend Shared Function getIllegaleInfo4point(winpt As myPoint, ByRef rESULT_text As String,
                                        ByRef kurz As String, ByRef pdf As String, schematabelle As String) As Boolean
        Dim resulttext As String = ""
        Try
            l(" getIllegaleInfo4point ---------------------- anfang")
            Return getIllegaleExtracted(rESULT_text, kurz, pdf, schematabelle, winpt)
            l(" getIllegaleInfo4point ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getIllegaleInfo4point: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Shared Function getIllegaleExtracted(ByRef rESULT_text As String, ByRef kurz As String,
                                       ByRef pdf As String,
                                       schematabelle As String, winpt As myPoint) As Boolean
        Dim dt As System.Data.DataTable
        l("getIllegaleExtracted ---------------------- ")
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
                    kurz = kurz & ", " & clsDBtools.fieldvalue(dt.Rows(i).Item("vid")).Trim
                    pdf = clsDBtools.fieldvalue(dt.Rows(i).Item("vid")).Trim
                    'wsgpdf = wsgpdf & "," & datei
                Next
                rESULT_text = bildeIllegaleINFO(dt)
                Return True
            End If
            l(" getIllegaleExtracted ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in getIllegaleExtracted: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function bildeIllegaleINFO(DataRow As DataTable) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = ", " & Environment.NewLine
            If DataRow.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & DataRow.Rows.Count & " Ausweisungen !" & trenn
            End If
            For i = 0 To DataRow.Rows.Count - 1
                'summe = summe & " ----------------------------------- " & trenn
                summe = summe & clsDBtools.fieldvalue(DataRow.Rows(i).Item("rbtitel").ToString.Trim & " ")
                summe = summe & clsDBtools.fieldvalue(DataRow.Rows(i).Item("name").ToString.Trim & " ")

                summe = summe & ", Paradigma-Vorgang: " & clsDBtools.fieldvalue(DataRow.Rows(i).Item("vid").ToString.Trim & trenn)
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeIllegaleINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
