Imports System.Data
Public Class clsEigentuemerAnalyse
    Shared Function getFS4coordinates(wpt As myPoint, ByRef fs As String,
                                      schematabelle As String, ByRef strError As String,
                                   ByRef weistauf As String, ByRef zeigtauf As String,
                                      ByRef albflaeche As String) As Boolean
        Dim innerSQL As String = " SELECT ST_GeomFromText('POINT(" & wpt.X & " " & wpt.Y & ")'," &
                                               m.PostgisDBcoordinatensystem.ToString &
                                               ")"
        l(innerSQL)
        Dim SQL = "  SELECT * " &
                "  FROM " & schematabelle & " " &
                "  WHERE ST_contains( ST_CurveToLine(" & schematabelle & ".geom),(" & innerSQL & "  )" & "  );"
        l("sql: " & SQL)
        Dim dt As DataTable
        l(" getBplanInfoErmitteln ---------------------- anfang")
        dt = clsPgtools.getDTFromWebgisDB(SQL, "postgis20", strError)
        l("Anzahl=" & dt.Rows.Count)
        If dt.Rows.Count < 1 Then
            Return False
        Else
            fs = clsDBtools.fieldvalue(dt.Rows(0).Item("fs")).ToString.Trim
            zeigtauf = clsDBtools.fieldvalue(dt.Rows(0).Item("zeigtauf")).ToString.Trim
            weistauf = clsDBtools.fieldvalue(dt.Rows(0).Item("weistauf")).ToString.Trim
            albflaeche = clsDBtools.fieldvalue(dt.Rows(0).Item("flaeche")).ToString.Trim
            Return True
        End If
    End Function
End Class
