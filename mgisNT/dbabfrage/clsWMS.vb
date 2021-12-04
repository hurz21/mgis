Imports System.Data

Class clsWMS

    Friend Shared Function istWMSDBabfrage(aid As Integer) As Boolean
        If aid = 379 Or aid = 186 Or aid = 390 Or aid = 389 Then
            Return True
        End If
        Return False
    End Function
    Shared Function calcVollstBbox(bbox As String) As String
        Dim a() As String
        Dim b As Integer
        Try
            l("calcVollstBbox---------------------- anfang")
            a = bbox.Split(","c)
            b = CInt(a(0)) + 2
            bbox = bbox & "," & b.ToString & "," & (CInt(a(1)) + 2).ToString
            Return bbox
            l("calcVollstBbox---------------------- ende")
        Catch ex As Exception
            l("Fehler in calcVollstBbox: " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Function calcWMSGetfeatureInfoURL(bbox As String, aid As Integer, hoehe As Integer, breite As Integer,
                                             pointx As Integer, pointy As Integer, format As String,
                                             wmslayers As String, wmsquery_layers As String) As String
        Try
            l("calcWMSGetfeatureInfoURL---------------------- anfang")
            'formate : "text/html" "text/plain"
            Dim dt As DataTable
            dt = getDTFromWebgisDB("SELECt daten FROM wms where aid=" & aid, "webgiscontrol")
            Dim stringg As String
            stringg = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            stringg = stringg.Replace("[BBOX]", bbox)
            stringg = stringg.Replace("[INFOFORMAT]", format)
            stringg = stringg.Replace("[HOEHE]", CStr(hoehe))
            stringg = stringg.Replace("[BREITE]", CStr(breite))
            stringg = stringg.Replace("[POINTX]", CStr(pointx))
            stringg = stringg.Replace("[POINTY]", CStr(pointy))
            stringg = stringg.Replace("[LAYERS]", CStr(wmslayers))
            stringg = stringg.Replace("[QUERY_LAYERS]", CStr(wmsquery_layers))
            Return stringg
            l("calcWMSGetfeatureInfoURL---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Function istpointactivemodus(aid As Integer) As Boolean
        Return True
    End Function
End Class
