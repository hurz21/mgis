Imports mgis
Imports mgis.nsTrinkwasserClass
Imports Newtonsoft.Json

Public Class clsTrinkwasser
    Friend Shared Function DoTrinkw418(layer As clsLayerPres, ByRef ausgabedatei As String, bbox As String, hoehe As Integer,
                                       breite As Integer,
                                             pointx As Integer, pointy As Integer, format As String,
                                             wmslayers As String, wmsquery_layers As String) As Boolean
        Dim templatestring As String
        Dim url As String = "", result As String = "", hinweis As String = ""
        Try
            l(" MOD DoTrinkw418 anfang")
            If layer.aid = 418 Then
                templatestring = clsWMStools.getdbTabTemplate(layer.aid)
                If templatestring.IsNothingOrEmpty Then
                    l("fehler templatedatei fehlt  " & layer.aid)
                    Return False
                End If
                layer = clsWMStools.getWMSinfos(layer.aid)
                If layer Is Nothing Then
                    l("fehler Layer exisiterit nicht in wmsliste")
                    Return False
                End If
                url = clsWMStools.calcWMSGetfeatureInfoURL(bbox, layer, hoehe, breite,
                                        pointx, pointy, layer.wmsProps.format,
                                        layer.wmsProps.stdlayer, layer.wmsProps.stdlayer)
                result = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, System.Text.Encoding.UTF8, 5000)
                '    If result Is Nothing Then Return True
                Dim instanz As nsTrinkwasserClass.instanz
                instanz = JsonConvert.DeserializeObject(Of nsTrinkwasserClass.instanz)(result)
                'http://jsonutils.com/ zum erstellen der struktur

                Dim aenderungstab As String
                aenderungstab = getHtmlTable(instanz)

                If aenderungstab.Trim = String.Empty Then
                    templatestring = templatestring.Replace("[AENDERUNGSTITEL]", "Keine Objekte an dieser Stelle vorhanden!")
                Else
                    templatestring = templatestring.Replace("[AENDERUNGSTITEL]", "")
                End If
                If instanz.features.Count > 1 Then
                    templatestring = templatestring.Replace("[NUTZUNG]", "<b>Es überlagern sich " & instanz.features.Count & " Schutzgebiete!</b>")
                Else
                    templatestring = templatestring.Replace("[NUTZUNG]", "")
                End If
                'templatestring = templatestring.Replace("[TITEL]", titel)
                templatestring = templatestring.Replace("[TABLEBODY]", aenderungstab)
                templatestring = templatestring.Replace("[DBINFO]", aenderungstab)
                ausgabedatei = clsWMStools.SaveTemplate(templatestring, layer.aid)
                l(" MOD DoTrinkw418 ende")
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            l("Fehler in DoTrinkw418: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function getHtmlTable(obj As instanz) As String
        Dim tab As New Text.StringBuilder
        Try
            l(" MOD getinsertText anfang")
            Dim temp As String
            For i = 0 To obj.features.Count - 1
                l(" MOD getinsertText ende")
                temp = obj.features(i).properties.WSG_ID : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>WSG-ID:</td><td  class='feldinhalt'>" & temp & "</td></tr>" & Environment.NewLine)

                If iminternet Then
                    temp = myglobalz.serverWeb & "/nkat/aid/245/texte/wsgid/" & temp & ".pdf"
                    tab.Append("<tr><td class='norm'>PDF</td><td class='feldinhalt'><input type='button'  value='Klick mich' onclick=""openMedium('" & temp & "')""> </input></td></tr>")
                Else
                    temp = myglobalz.serverWeb & "/nkat/aid/245/texte/wsgid/" & temp & ".pdf"
                    tab.Append("<tr><td class='norm'>PDF</td><td class='feldinhalt'><input type='button' value='Klick mich'  onclick=""openMedium('" & temp & "')""> </input></td></tr>")
                End If

                'temp = myglobalz.serverWeb & "/nkat/aid/245/texte/wsgid/" & temp & ".pdf"
                'tab.Append("<tr><td class='norm'>PDF</td><td class='feldinhalt'><a target='_blank' href='" & temp & "'>Klick </input></td></tr>") 
                'tab.Append("<tr><td class='norm'>PDF</td><td class='feldinhalt'><a  target='_blank' href='" & obj.features(i).properties.GEN_URL_PDF & "'>" & obj.features(i).properties.GEN_URL_PDF & "</a></td></tr>")


                temp = obj.features(i).properties.ZONE : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>WSG-Zone:</td><td  class='feldinhalt'>" & temp & "</td></tr>" & Environment.NewLine)

                temp = obj.features(i).properties.WSG_KURZNAME : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>WSG_KURZNAME:</td><td  class='feldinhalt'>" & temp & "</td></tr>")

                temp = obj.features(i).properties.WSG_ART : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>WSG_ART:</td><td  class='feldinhalt'>" & temp & "</td></tr>")

                temp = obj.features(i).properties.STATUS_RPU : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>STATUS_RPU:</td><td  class='feldinhalt'>" & temp & "</td></tr>")

                temp = obj.features(i).properties.KREIS_MASSGEBLICH_NAME : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>KREIS_MASSGEBLICH_NAME:</td><td  class='feldinhalt'>" & temp & "</td></tr>")

                temp = obj.features(i).properties.KREISE : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>KREISE betroffen:</td><td  class='feldinhalt'>" & temp & "</td></tr>")

                temp = obj.features(i).properties.ARCHIV_HLNUG : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>ARCHIV_HLNUG:</td><td  class='feldinhalt'>" & temp & "</td></tr>")

                temp = obj.features(i).properties.STAATSANZEIGER : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>STAATSANZEIGER:</td><td  class='feldinhalt'>" & temp & "</td></tr>")

                If Not obj.features(i).properties.STAATSANZEIGER_AENDER = "Null" Then

                    temp = obj.features(i).properties.STAATSANZEIGER_AENDER : temp = temp.Replace(vbCrLf, "<br>") : temp = temp.Replace("Null", " ")
                    tab.Append("<tr><td class='norm'>STAATSANZEIGER_AENDER:</td><td  class='feldinhalt'>" & temp & "</td></tr>")
                End If

                temp = obj.features(i).properties.VERORDNUNGDATUM : temp = temp.Replace(vbCrLf, "<br>")
                tab.Append("<tr><td class='norm'>VERORDNUNGDATUM:</td><td  class='feldinhalt'>" & temp & "</td></tr>")

                temp = "############################################################################"
                tab.Append("<tr><td class='norm'>#############################</td><td  class='feldinhalt'>" & temp & "</td></tr>")
            Next
            'MsgBox(tab.ToString)
            Return tab.ToString
        Catch ex As Exception
            l("Fehler in getinsertText: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
