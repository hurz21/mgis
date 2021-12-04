Imports System.Data
Imports gisDossier

Public Class clsNASLageTools
    Friend Shared Function getlage(weistauf As String, zeigtauf As String, ByRef strError As String) As NASlage
        Dim neulage As New NASlage
        Dim dt As DataTable
        Dim SQL As String = ""
        Try
            l(" getlage ---------------------- anfang")
            l(" weistauf " & weistauf)
            l(" zeigtauf " & zeigtauf)
            If Not String.IsNullOrEmpty(weistauf) Then
                SQL = "SELECT * FROM public.alkis_lagemithn where gml_id='" & weistauf & "'"
                l(" getBplanInfoErmitteln ---------------------- anfang")
                l(" SQL " & SQL)
                dt = clsPgtools.getDTFromWebgisDB(SQL, "postgis20", strError)
                nachricht(strError)
                If dt.Rows.Count > 0 Then
                    neulage = spaltenMapping(dt, True)
                    neulage.calcLageschluessel()
                    Return neulage
                Else
                    nachricht("fehler KEINE treffer" & SQL)
                    nachricht("lageGrunddatenHolenErfolgreich false")
                    Return Nothing
                End If
            End If
            If Not String.IsNullOrEmpty(zeigtauf) Then
                SQL = "SELECT * FROM public.alkis_lageohnehn where gml_id='" & zeigtauf & "'"
                l(" SQL " & SQL)
                dt = clsPgtools.getDTFromWebgisDB(SQL, "postgis20", strError)
                nachricht(strError)
                If dt.Rows.Count > 0 Then
                    neulage = spaltenMapping(dt, False)
                    neulage.calcLageschluessel()
                    Return neulage
                Else
                    nachricht("fehler KEINE treffer" & SQL)
                    nachricht("lageGrunddatenHolenErfolgreich false")
                    Return Nothing
                End If
            End If
            l(" getlage ---------------------- ende")
        Catch ex As Exception
            l("Fehler in getlage: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Shared Function spaltenMapping(dt As DataTable, weistauf As Boolean) As NASlage
        Dim neulage As New NASlage
        Try
            l(" spaltenMapping ---------------------- anfang")
            neulage.GemeindeNr = (clsDBtools.fieldvalue(dt.Rows(0).Item("gemeinde")).ToString).Trim
            neulage.Lage = (clsDBtools.fieldvalue(dt.Rows(0).Item("Lage")).ToString).Trim
            neulage.kreis = (clsDBtools.fieldvalue(dt.Rows(0).Item("kreis")).ToString).Trim
            neulage.regbez = (clsDBtools.fieldvalue(dt.Rows(0).Item("regbez")).ToString).Trim
            neulage.land = (clsDBtools.fieldvalue(dt.Rows(0).Item("land")).ToString).Trim
            If weistauf Then
                neulage.hausnummer = (clsDBtools.fieldvalue(dt.Rows(0).Item("hausnummer"))).ToString.Trim
            End If
            Return neulage
            l(" spaltenMapping ---------------------- ende")
        Catch ex As Exception
            l("Fehler in spaltenMapping: " & ex.ToString())
            Return neulage
        End Try
    End Function

    Friend Shared Function getstrassename(lageschluessel As String) As String
        Dim sql, strError As String
        Dim dt As DataTable
        Try
            l(" getstrassename ---------------------- anfang")
            sql = "SELECT * FROM public.lageschluessel where schluesselgesamt='" & lageschluessel & "'"
            dt = clsPgtools.getDTFromWebgisDB(sql, "postgis20", strError)
            nachricht(strError)
            If dt.Rows.Count > 0 Then
                Return (clsDBtools.fieldvalue(dt.Rows(0).Item("bezeichnung")).ToString).Trim
            Else
                Return ""
            End If
            l(" getstrassename ---------------------- ende")
            Return ""
        Catch ex As Exception
            l("Fehler in getstrassename: " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Function getgemeindename(gemeindenr As String) As String
        'den gemeindename ergründen
        Dim gemparms As New clsGemarkungsParams
        gemparms.init() : Dim result$ = "ERROR"
        Dim a = From item In gemparms.parms Where item.gemeindenr = CDbl(CInt(gemeindenr)) Select item.gemeindetext
        If a.ToArray.Length > 0 Then result$ = a.ToList(0).ToString
        Dim gemeindenanme$ = result$
        Return gemeindenanme.Trim
    End Function
End Class
