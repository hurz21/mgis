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
        Dim radiusM = 2
        Dim xl, xh, yl, yh As Integer
        Try
            l("calcVollstBbox---------------------- anfang")
            a = bbox.Split(","c)
            xl = CInt(a(0)) - radiusM
            xh = CInt(a(0)) + radiusM
            yl = CInt(a(1)) - radiusM
            yh = CInt(a(1)) + radiusM
            ' bbox = bbox & "," & b.ToString & "," & (CInt(a(1)) + 2).ToString
            bbox = xl & "," & yl & "," & xh & "," & yh
            Return bbox
            l("calcVollstBbox---------------------- ende")
        Catch ex As Exception
            l("Fehler in calcVollstBbox: " & ex.ToString())
            Return ""
        End Try
    End Function
    Shared Function calcVollstBbox(bbox As String, radiusM As Integer) As String
        'radius 1050m und 558px
        Dim a() As String
        'Dim radiusM = 1050
        Dim xl, xh, yl, yh As Integer
        Try
            l("calcVollstBbox---------------------- anfang")
            a = bbox.Split(","c)
            xl = CInt(a(0)) - radiusM
            xh = CInt(a(0)) + radiusM
            yl = CInt(a(1)) - radiusM
            yh = CInt(a(1)) + radiusM
            ' bbox = bbox & "," & b.ToString & "," & (CInt(a(1)) + 2).ToString
            bbox = xl & "," & yl & "," & xh & "," & yh
            l("box: " & bbox)
            Return bbox
            l("calcVollstBbox---------------------- ende")
        Catch ex As Exception
            l("Fehler in calcVollstBbox: " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Function calcWMSGetfeatureInfoURL(bbox As String, aid As Integer, hoehe As Integer, breite As Integer,
                                             pointx As Integer, pointy As Integer, format As String,
                                             wmslayers As String, wmsquery_layers As String, ByRef strError As String) As String
        Try
            l("calcWMSGetfeatureInfoURL---------------------- anfang")
            Dim stringg As String
            'formate : "text/html" "text/plain"
            If aid > 10000 Then '
                If aid = 10001 Then
                    '10001 = Standorttypisierung für die Biotopentwicklung 1:50.000
                    stringg = "http://geodienste-umwelt.hessen.de/arcgis/services/inspire/boden/MapServer/WmsServer?VERSION=1.3.0&SERVICE=WMS&request=GetFeatureInfo&query_layers=[QUERY_LAYERS]&layers=[LAYERS]&crs=EPSG:25832srs=EPSG:25832&bbox=[BBOX]&height=[HOEHE]&width=[BREITE]&info_format=[INFOFORMAT]&x=[POINTX]&y=[POINTY]"
                End If
                If aid = 10002 Then
                    '10001 = Standorttypisierung für die Biotopentwicklung 1:50.000
                    stringg = "http://geodienste-umwelt.hessen.de/arcgis/services/inspire/boden/MapServer/WmsServer?VERSION=1.3.0&SERVICE=WMS&request=GetFeatureInfo&query_layers=[QUERY_LAYERS]&layers=[LAYERS]&crs=EPSG:25832srs=EPSG:25832&bbox=[BBOX]&height=[HOEHE]&width=[BREITE]&info_format=[INFOFORMAT]&x=[POINTX]&y=[POINTY]"
                    stringg = "http://geodienste-umwelt.hessen.de/arcgis/services/inspire/bewirtschaftungsgebiete/MapServer/WmsServer?language=ger&service=WMS&version=1.1.0&request=GetFeatureInfo&query_layers=[QUERY_LAYERS]&layers=[LAYERS]&srs=EPSG:25832&bbox=[BBOX]&height=[HOEHE]&width=[BREITE]&info_format=[INFOFORMAT]&x=[POINTX]&y=[POINTY]
"
                End If
            Else
                Dim dt As DataTable
                dt = clsPgtools.getDTFromWebgisDB("SELECt daten FROM wms where aid=" & aid, "webgiscontrol", strError)
                stringg = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            End If


            l("bbox" & bbox)
            l("pointx pointy" & pointx & "," & pointy)
            stringg = WMStemplateReplace(bbox, hoehe, breite, pointx, pointy, format, wmslayers, wmsquery_layers, stringg)
            Return stringg
            l("calcWMSGetfeatureInfoURL---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function WMStemplateReplace(bbox As String, hoehe As Integer, breite As Integer, pointx As Integer, pointy As Integer, format As String, wmslayers As String, wmsquery_layers As String, stringg As String) As String
        stringg = stringg.Replace("[BBOX]", bbox)
        stringg = stringg.Replace("[INFOFORMAT]", format)
        stringg = stringg.Replace("[HOEHE]", CStr(hoehe))
        stringg = stringg.Replace("[BREITE]", CStr(breite))
        stringg = stringg.Replace("[POINTX]", CStr(pointx))
        stringg = stringg.Replace("[POINTY]", CStr(pointy))
        stringg = stringg.Replace("[LAYERS]", CStr(wmslayers))
        stringg = stringg.Replace("[QUERY_LAYERS]", CStr(wmsquery_layers))
        Return stringg
    End Function

    Friend Shared Function istpointactivemodus(aid As Integer) As Boolean
        Return True
    End Function
End Class
