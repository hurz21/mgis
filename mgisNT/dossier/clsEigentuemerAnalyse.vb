Imports System.Data
Public Class clsEigentuemerAnalyse
    Shared Function getFS4coordinates(wpt As myPoint, ByRef fs As String, schematabelle As String) As Boolean
        Dim innerSQL As String = " SELECT ST_GeomFromText('POINT(" & wpt.X & " " & wpt.Y & ")'," &
                                               PostgisDBcoordinatensystem.ToString &
                                               ")"
        l(innerSQL)
        Dim SQL = "  SELECT * " &
                "  FROM " & schematabelle & " " &
                "  WHERE ST_contains( " & schematabelle & ".geom,(" & innerSQL & "  )" & "  );"
        l("sql: " & SQL)
        Dim dt As DataTable
        l(" getBplanInfoErmitteln ---------------------- anfang")
        dt = getDTFromWebgisDB(SQL, "postgis20")
        l("Anzahl=" & dt.Rows.Count)
        If dt.Rows.Count < 1 Then
            Return False
        Else
            fs = clsDBtools.fieldvalue(dt.Rows(0).Item("fs")).ToString.Trim
            Return True
        End If
    End Function
End Class
