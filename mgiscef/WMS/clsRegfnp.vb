Imports System.ComponentModel
Imports mgis
Imports Newtonsoft.Json
Public Class clsRegfnp
    Shared Function getRegfnpHaupt(url As String) As String
        Dim a() As String
        Dim result As String = "", hinweis As String = ""
        Try
            l(" getRegfnpHaupt ---------------------- anfang")
            result = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, System.Text.Encoding.UTF8, 5000)
            If result Is Nothing Then Return ""

            a = result.Split(";"c)
            l(" getRegfnpHaupt ---------------------- ende")
            Return a(13)
        Catch ex As Exception
            l("Fehler in getRegfnpHaupt: " & url & Environment.NewLine & ex.ToString())
            Return "fehler"
        End Try
    End Function

    Shared Function getRegfnpAenderung(url As String) As String
        Dim result As String = "", hinweis As String = ""
        Try
            l(" MOD getRegfnpAenderung anfang")
            result = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, System.Text.Encoding.UTF8, 5000)
            Dim objRegfnpAenderung As nsRegfnp.regfnpAenderungs
            objRegfnpAenderung = JsonConvert.DeserializeObject(Of nsRegfnp.regfnpAenderungs)(result)
            'http://jsonutils.com/ zum erstellen der struktur
            Dim tab As String = clsWMStools.REGFNPgenTabAenderungsobjekt(objRegfnpAenderung)
            l(" MOD getRegfnpAenderung ende")
            Return tab
        Catch ex As Exception
            l("Fehler in getRegfnpAenderung: " & ex.ToString())
            Return ""
        End Try
    End Function
    Shared Function DoRegFNP(layer As clsLayerPres, ByRef ausgabedatei As String, bbox As String, hoehe As Integer, breite As Integer,
                                             pointx As Integer, pointy As Integer, format As String,
                                             wmslayers As String, wmsquery_layers As String) As Boolean
        Dim templatestring As String
        Dim url = "", titel As String = ""
        Try
            l(" MOD DoRegFNP anfang")
            If layer.aid = 186 Then
                l("regfnp")
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
                                        pointx, pointy, "text/plain",
                                        layer.wmsProps.stdlayer, layer.wmsProps.stdlayer)
                Dim nutzungplan As String = getRegfnpHaupt(url)


                If nutzungplan.IsNothingOrEmpty Then
                    MessageBox.Show("Der Dienst steht zur Zeit nicht zur Verfügung!" & Environment.NewLine &
                                              "Sie sollten die Ebene bis auf weiteres ausschalten: " & Environment.NewLine &
                                              layerActive.titel)
                    Return False
                End If
                If nutzungplan.EndsWith(", B") Then nutzungplan = nutzungplan.Replace(", B", ", Bestand")
                If nutzungplan.EndsWith(", P") Then nutzungplan = nutzungplan.Replace(", P", ", geplant")

                Dim aid As Integer, alterText As String, neuerText As String
                aid = getNrfromText(nutzungplan)
                alterText = nutzungplan.Replace(aid.ToString, "").Trim
                neuerText = getNeuerText(aid)

                nutzungplan = neuerText & " (" & aid & " " & alterText & ") "
                layer = clsWMStools.getWMSinfos(187)
                url = clsWMStools.calcWMSGetfeatureInfoURL(bbox, layer, hoehe, breite,
                                        pointx, pointy, layer.wmsProps.format,
                                        layer.wmsProps.stdlayer, layer.wmsProps.stdlayer)
                l("url " & url)
                Dim aenderungstab As String = getRegfnpAenderung(url)
                If aenderungstab.Trim = String.Empty Then
                    templatestring = templatestring.Replace("[AENDERUNGSTITEL]", "")
                Else
                    templatestring = templatestring.Replace("[AENDERUNGSTITEL]", "Änderungsebene:")
                End If
                templatestring = templatestring.Replace("[NUTZUNG]", nutzungplan)
                templatestring = templatestring.Replace("[TITEL]", titel)
                templatestring = templatestring.Replace("[TABLEBODY]", aenderungstab)
                templatestring = templatestring.Replace("[DBINFO]", aenderungstab)
                ausgabedatei = clsWMStools.SaveTemplate(templatestring, layer.aid)
                Return True
            End If
            l(" MOD DoRegFNP ende")
            Return True
        Catch ex As Exception
            l("Fehler in DoRegFNP: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function getNeuerText(aid As Integer) As String
        Dim res As String = ""
        Dim sql As String = "" : Dim hinweis As String = ""
        Try
            l(" MOD getNeuerText anfang")
            sql = "select art from regfnp.rfp11_f where aid=" & aid & " limit 1"
            sql = "select art from paradigma_userdata.regfnpnutzung where aid=" & aid
            Dim result = clsToolsAllg.getSQL4Http(sql, "postgis20", hinweis, "getsql")
            l(hinweis)

            If result.IsNothingOrEmpty Then
                Return ""
            Else
                result = result.Trim
                res = result.Replace("$", "")
            End If
            l(" MOD getNeuerText ende")
            Return res
        Catch ex As Exception
            l("Fehler in getNeuerText: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function getNrfromText(nutzungplan As String) As Integer
        Dim a() As String
        Dim result As Integer
        Try
            l(" MOD getNrfromText anfang")
            If nutzungplan = String.Empty Then
                Return 0
            End If
            a = nutzungplan.Split(" "c)
            If IsNumeric(a(0)) Then
                result = CInt(a(0))
            Else
                result = 0
            End If
            l(" MOD getNrfromText ende " & result)
            Return result
        Catch ex As Exception
            l("Fehler in getNrfromText: " & ex.ToString())
            Return -1
        End Try
    End Function
End Class
