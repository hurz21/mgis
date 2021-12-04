Imports mgis

Public Class clsWMStools
    Friend Shared Function getdbTabTemplate(aid As Integer) As String
        'Return dbTemplateString
        Dim datei As String = strGlobals.gisWorkingDir & "\templates\" & aid & ".html"
        Try
            l(" MOD getdbTabTemplate anfang " & aid)
            datei = strGlobals.gisWorkingDir & "\templates\" & aid & ".html"
            datei = IO.File.ReadAllText(datei)
            Return datei
            l(" MOD getdbTabTemplate ende")
        Catch ex As Exception
            l("Fehler in getdbTabTemplate: " & ex.ToString())
            Return ""
        End Try
        'Return "<html><head> <meta charset='utf-8'><title>[TITEL]</title>" &
        '        "<style>img {vertical-align: text-top;float: left;        }    </style></head>" &
        '        "<body bgcolor='#cccccc'>&nbsp; <h1 > [TITEL] </h1> &nbsp;" &
        '        " &nbsp; <p> [NUTZUNG]&nbsp;</p> &nbsp; " &
        '        " [AENDERUNGSTITEL]&nbsp; <table style='width:100%, vertical-align: text-top;'>[TABLEBODY]</table>" &
        '        "" &
        '        "</body></html>"
    End Function
    Friend Shared Function SaveTemplate(templatestring As String, aid As Integer) As String
        Dim ausgabedatei As String
        Try
            l(" MOD SaveTemplate anfang")
            Dim ausgabeDIR As String = strGlobals.localDocumentCacheRoot  '& "" & aid
            IO.Directory.CreateDirectory(ausgabeDIR)
            ausgabedatei = ausgabeDIR & "\" & aid & "_" & ".html"
            IO.File.WriteAllText(ausgabedatei, templatestring)
            'IO.File.WriteAllText(ausgabedatei.Replace(".html", ".docx"), clsString.changeUmlaut2Html(strdok)) 
            l(" SaveTemplate ---------------------- ende")
            Return ausgabedatei
        Catch ex As Exception
            l("Fehler in SaveTemplate: " & ex.ToString())
            Return ""
        End Try
    End Function
    Shared Function REGFNPgenTabAenderungsobjekt(obj As nsRegfnp.regfnpAenderungs) As String
        Dim tab As New Text.StringBuilder
        Dim temp As String
        Dim vorschauaenderung, vorschauergaenzung As String
        For i = 0 To obj.features.Count - 1
            vorschauaenderung = "https://mapview.region-frankfurt.de/images/RegFNP_Aend/" &
                obj.features(i).properties.RF_AEND_INFODOC & "/" &
                obj.features(i).properties.RF_AEND_INFODOC &
                "_vorschau_" & "g27" & ".jpg"
            vorschauergaenzung = "https://mapview.region-frankfurt.de/images/RegFNP_Aend/ergaenzung/ergaenzung_" &
                obj.features(i).properties.OBJECTID & "_vorschau.jpg"

            'tab.Append("<tr><td>ObjektID</td><td>" & obj.features(i).properties.OBJECTID & "</td></tr>")
            'tab.Append("<tr><td>Hektar</td><td>" & obj.features(i).properties.HEKTAR & "</td></tr>")
            'tab.Append("<tr><td>RF_AEND_INFODOC</td><td>" & obj.features(i).properties.RF_AEND_INFODOC & "</td></tr>")
            'tab.Append("<tr><td>ALLG_AEND_NUMMER</td><td>" & obj.features(i).properties.ALLG_AEND_NUMMER & "</td></tr>")
            'tab.Append("<tr><td>ALLG_STADT_GEM</td><td>" & obj.features(i).properties.ALLG_STADT_GEM & "</td></tr>")
            'tab.Append("<tr><td>---</td><td>" & "</td></tr>")
            temp = obj.features(i).properties.ALLG_NUTZ
            temp = temp.Replace(vbCrLf, "<br>")
            tab.Append("<tr><td class='norm'>Nutzung:</td><td  class='feldinhalt'>" & temp & "</td></tr>")
            'tab.Append("<tr><td>---</td><td>" & "</td></tr>")
            temp = obj.features(i).properties.ALLG_BEZEICHNUNG
            temp = temp.Replace(vbCrLf, "<br>")
            tab.Append("<tr><td class='norm'>Bezeichnung:</td><td class='feldinhalt'>" & temp & "</td></tr>")
            'tab.Append("<tr><td class='norm'>---</td><td class='feldinhalt'>" & "</td></tr>")
            tab.Append("<tr><td class='norm'>Verf.Stand</td><td class='feldinhalt'>" & obj.features(i).properties.ALLG_VERF_STAND & "</td></tr>")
            'tab.Append("<tr><td>GEN_RP_DATS</td><td>" & obj.features(i).properties.GEN_RP_DATS & "</td></tr>")
            tab.Append("<tr><td class='norm'>Bekanntmachung im StaatsAZ:</td><td class='feldinhalt'>" & obj.features(i).properties.GEN_STANZ_DAT & "</td></tr>")
            If obj.features(i).properties.GEN_URL_PDF.Trim.IsNothingOrEmpty Then
            Else
                tab.Append("<tr><td class='norm'>PDF</td><td class='feldinhalt'><a  target='_blank' href='" & obj.features(i).properties.GEN_URL_PDF & "'>" & obj.features(i).properties.GEN_URL_PDF & "</a></td></tr>")
            End If
            'tab.Append("<tr><td>---</td><td> " & "<img src='" & vorschauaenderung & "' alt='Smiley face'  >" & "</td></tr>")
            If obj.features(i).properties.ALLG_BEZEICHNUNG.Contains("Ergänzung") Then
                ' tab.Append("<tr><td>---</td><td> " & "<img src='" & vorschauergaenzung & "' alt='Vorschau Ergänzung'  >" & "</td></tr>")
            End If
            'tab.Append("<tr><td class='norm'>---</td><td class='feldinhalt'>--------------" & "</td></tr>")
        Next
        Return tab.ToString
    End Function

    Friend Shared Function getWMSinfos(aid As Integer) As clsLayerPres
        Dim lay As New clsLayerPres
        lay.aid = aid
        Try
            ' l(" MOD markwmslayer anfang") 
            For Each wms In wmspropList
                If wms.aid = aid Then
                    lay.iswms = True
                    lay.wmsProps.aid = wms.aid
                    lay.wmsProps.url = wms.url
                    lay.wmsProps.typ = wms.typ
                    lay.wmsProps.format = wms.format
                    lay.wmsProps.stdlayer = wms.stdlayer
                    Return lay
                End If
            Next
            '  l(" MOD markwmslayer ende")
            Return Nothing
        Catch ex As Exception
            l("Fehler in markwmslayer: " & ex.ToString())
            Return Nothing
        End Try
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

    Friend Shared Function calcWMSGetfeatureInfoURL(bbox As String, lay As clsLayerPres, hoehe As Integer, breite As Integer,
                                             pointx As Integer, pointy As Integer, format As String,
                                             wmslayers As String, wmsquery_layers As String) As String
        Try
            l("calcWMSGetfeatureInfoURL---------------------- anfang")
            'formate : "text/html" "text/plain"
            'Dim dt As DataTable
            'dt = getDTFromWebgisDB("SELECt daten FROM wms where aid=" & aid, "webgiscontrol")
            Dim stringg As String
            stringg = lay.wmsProps.url
            'stringg = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
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
