Public Class clsGISfunctions

    Dim latitude As String
    Dim longitude As String
    Sub nachricht(text As String)
        clsTools.l(text)
    End Sub
    Sub l(text As String)
        clsTools.l(text)
    End Sub
    'Public Function GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(ByVal bbox As clsRange, htmlformat As Boolean,
    '                                                               serverunc As String, templateroot As String,
    '                                                               aktvorgangsid As String, enc As Text.Encoding) As String
    '    nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe -----------------------------------")
    '    l("aktrange=" & bbox.toString)
    '    Dim aktp As New myPoint
    '    Dim abstand As Double
    '    Dim templateFile As String
    '    aktp.X = bbox.xl + bbox.xdif() / 2
    '    aktp.Y = bbox.yl + bbox.ydif() / 2
    '    abstand = bbox.xdif

    '    If Not coordIstOk(aktp) Then
    '        MessageBox.Show("Leider ist die Koordinatenangabe unbrauchbar! " & vbCrLf & aktp.toString())
    '        nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe fehler" & aktp.toString())
    '        Return "fehler"
    '    End If
    '    'Dim punktliste() As myPoint

    '    ReDim punktarrayInM(0)
    '    punktarrayInM(0) = aktp
    '    nachricht("1")
    '    nachricht("aktp: " & aktp.toString)
    '    Dim quellstring As String = modKoordTrans.bildeQuellKoordinatenString(punktarrayInM)
    '    Dim aufruf As String = modKoordTrans.bildeaufruf4KoordinatenServer(quellstring, punktarrayInM.Count.ToString, "UTM", "WINKEL_G")
    '    Dim hinweis As String = ""
    '    Dim result As String = meineHttpNet.meinHttpJob(clsTools.ProxyString, aufruf, hinweis, enc, 5000)
    '    nachricht(hinweis)
    '    nachricht("result: " & result)
    '    modKoordTrans.getLongLatFromResultSingle(result, longitude, latitude)
    '    'GMtemplates.SetLatitude(aktp, longitude, latitude)
    '    Dim TEXTKOERPER As String = "Bitte benutzen Sie das <b>Snipping Tool</b> um das Luftbild zu kopieren. " &
    '        "Sie können es dann über den Knopf <b>Zwischenablage</b> in Paradigma einfügen!"
    '    templateFile = templateroot & "\Infowindow.htm"
    '    nachricht("2")

    '    quellstring = modKoordTrans.bildeQuellKoordinatenString(punktarrayInM)
    '    l("quellstring " & quellstring)
    '    aufruf = modKoordTrans.bildeaufruf4KoordinatenServer(quellstring, punktarrayInM.Count.ToString, "UTM", "WINKEL_G")
    '    nachricht("result: " & result)
    '    nachricht("aufruf: " & aufruf)
    '    Dim punkteInWinkelkoordinaten() As myPoint
    '    result = meineHttpNet.meinHttpJob(clsTools.ProxyString, aufruf, hinweis, enc, 5000)
    '    nachricht(hinweis)
    '    '  modKoordTrans.getLongLatFromResultBulk(result)
    '    nachricht("3")
    '    nachricht("result: " & result)

    '    punkteInWinkelkoordinaten = modKoordTrans.getLongLatFromResultBulk(result)
    '    '  punkteInWinkelkoordinaten = GMtemplates.konvertierePunkteArrayVonUTMnachWinkel(myGlobalz.punktarrayInM)

    '    Dim templ As String = ""
    '    templ = GMtemplates.templateEinlesen(templateFile)

    '    If templ.IsNothingOrEmpty Or Not htmlformat Then
    '        'templatedatei fehlt
    '        ''https://maps.google.com/maps?ll=50.0030653020894,8.76937026434553&t=h
    '        'Dim http As String = "https://maps.google.com/maps?ll=" & latitude.Replace(",", ".") & "," & longitude.Replace(",", ".") & "&t=h"
    '        Dim http As String = "https://www.google.com/maps/@" &
    '            latitude.Replace(",", ".") & "," & longitude.Replace(",", ".") & ",355a,20y,41.6t/data=!3m1!1e3"
    '        'starten(http)
    '        nachricht("http: " & http)
    '        Return http
    '    Else
    '        templ = GMtemplates.templateAnpassen(templ,
    '                                      coords:=latitude.Replace(",", ".") & "," & longitude.Replace(",", "."),
    '                                      title:="Paradigmavorgang Nr: " & aktvorgangsid.ToString,
    '                                      polygon:=punkteInWinkelkoordinaten,
    '                                      TEXTKOERPER:=TEXTKOERPER)
    '        templ = templateAuschreiben(templ, serverunc)
    '        'templateStarten(templ)
    '        nachricht("templ: " & templ)
    '        Return templ
    '    End If
    'End Function


    'Public Sub GISAufruf_Mittelpunkt(ByVal pt As myPoint)
    '    If pt.X < 10000 Then
    '        MessageBox.Show("Es konnten keine brauchbaren Koordinaten gefunden werden!", "GIS", MessageBoxButton.OK, MessageBoxImage.Error)
    '        Exit Sub
    '    End If
    '    Dim radius As String = "200"
    '    If myGlobalz.sitzung.raumbezugsmodus = "adresse" Then
    '        radius = "200"
    '    End If
    '    Dim http As String = CLstart.mycsimple.getServerHTTPdomainIntranet() & "/cgi-bin/suchdb.cgi?modus=42" & _
    '            "&rechts=" & CInt(pt.X) & _
    '            "&hoch=" & CInt(pt.Y) & _
    '            "&abstand=" & radius & _
    '            "&username=" & myGlobalz.sitzung.aktBearbeiter.username & _
    '            "&thema=" & setDefaultThemen() '& _
    '    '"&format=fix800x600"
    '    starten(http)
    'End Sub


    'Shared Function adresseIstOK(ByVal adr As ParaAdresse) As Boolean
    '    If String.IsNullOrEmpty(adr.Gisadresse.gemeindeName) Then
    '        Return False
    '    End If
    '    Return True
    'End Function

    Private Function coordIstOk(ByVal aktp As myPoint) As Boolean
        If String.IsNullOrEmpty(aktp.X.ToString) Then
            Return False
        End If
        If aktp.X < 340000 Then
            Return False
        End If
        Return True
    End Function


    Public Function GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(ByVal bbox As clsRange, htmlformat As Boolean,
                                                                   ByRef laenge As String, ByRef breite As String,
                                                                   serverunc As String, enc As Text.Encoding,
                                                                    punktarrayInM() As myPoint) As String
        nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe -----------------------------------")
        Dim aktp As New myPoint
        Dim abstand As Double
        Dim templateFile As String
        aktp.X = CInt(bbox.xl + bbox.xdif() / 2)
        aktp.Y = CInt(bbox.yl + bbox.ydif() / 2)
        abstand = bbox.xdif

        If Not coordIstOk(aktp) Then
            MessageBox.Show("Leider ist die Koordinatenangabe unbrauchbar! " & vbCrLf & aktp.toString())
            nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe fehler" & aktp.toString())
            Return "fehler"
        End If
        'Dim punktliste() As myPoint

        ReDim punktarrayInM(0)
        punktarrayInM(0) = aktp
        nachricht("1")
        nachricht("aktp: " & aktp.toString)
        Dim quellstring As String = modKoordTrans.bildeQuellKoordinatenString(punktarrayInM)
        Dim aufruf As String = modKoordTrans.bildeaufruf4KoordinatenServer(quellstring, punktarrayInM.Count.ToString, "UTM", "WINKEL_G")
        Dim hinweis As String = ""
        Dim result As String = meineHttpNet.meinHttpJob(mset.ProxyString, aufruf, hinweis, enc, 5000)
        nachricht(hinweis)
        nachricht("result: " & result)
        modKoordTrans.getLongLatFromResultSingle(result, longitude, latitude)
        laenge = longitude : breite = latitude
        'GMtemplates.SetLatitude(aktp, longitude, latitude)
        Dim TEXTKOERPER As String = "Bitte benutzen Sie das <b>Snipping Tool</b> um das Luftbild zu kopieren. " &
            "Sie können es dann über den Knopf <b>Zwischenablage</b> in Paradigma einfügen!"
        'templateFile = initP.getValue("Beide.GoogleMapsTemplateDir") & "Infowindow.htm"
        ' "\\file-paradigma\paradigma\test\paradigmaArchiv\div\GMapTemplates\Infowindow.htm" 
        templateFile = Environment.CurrentDirectory & "\bplaninternet\Infowindow.htm" ' liegt nun im executebaledir

        l("templateFile " & templateFile)

        Dim punkteInWinkelkoordinaten() As myPoint

        '  punkteInWinkelkoordinaten = GMtemplates.konvertierePunkteArrayVonUTMnachWinkel(myGlobalz.punktarrayInM)
        'MsgBox(templateFile)
        Dim templ As String = GMtemplates.templateEinlesen(templateFile)

        If templ.IsNothingOrEmpty Or Not htmlformat Then
            'templatedatei fehlt
            ''https://maps.google.com/maps?ll=50.0030653020894,8.76937026434553&t=h
            'Dim http As String = "https://maps.google.com/maps?ll=" & latitude.Replace(",", ".") & "," & longitude.Replace(",", ".") & "&t=h"
            Dim http As String = "https://www.google.com/maps/@" &
                latitude.Replace(",", ".") & "," & longitude.Replace(",", ".") & ",355a,20y,41.6t/data=!3m1!1e3"
            'starten(http)
            nachricht("http: " & http)
            Return http
        Else
            templ = GMtemplates.templateAnpassen(templ,
                                          coords:=latitude.Replace(",", ".") & "," & longitude.Replace(",", "."),
                                          title:="P-Gis: GoogleMaps ",
                                          polygon:=punkteInWinkelkoordinaten,
                                          TEXTKOERPER:=TEXTKOERPER)
            templ = templateAuschreiben(templ, Environment.CurrentDirectory & "\")
            'templateStarten(templ)
            nachricht("templ: " & templ)
            Return templ
        End If
    End Function
    Public Function GoogleMapsAufruf_Extern(ByVal bbox As clsRange, htmlformat As Boolean, enc As Text.Encoding,
                                             punktarrayInM() As myPoint) As String
        nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe -----------------------------------")
        Dim aktp As New myPoint
        Dim abstand As Double
        Dim templateFile As String = ""
        aktp.X = CInt(bbox.xl + bbox.xdif() / 2)
        aktp.Y = CInt(bbox.yl + bbox.ydif() / 2)
        abstand = bbox.xdif

        If Not coordIstOk(aktp) Then
            MessageBox.Show("Leider ist die Koordinatenangabe unbrauchbar! " & vbCrLf & aktp.toString())
            nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe fehler" & aktp.toString())
            Return "fehler"
        End If
        'Dim punktliste() As myPoint

        ReDim punktarrayInM(0)
        punktarrayInM(0) = aktp
        nachricht("1")
        nachricht("aktp: " & aktp.toString)
        Dim quellstring As String = modKoordTrans.bildeQuellKoordinatenString(punktarrayInM)
        Dim aufruf As String = modKoordTrans.bildeaufruf4KoordinatenServer(quellstring, punktarrayInM.Count.ToString, "UTM", "WINKEL_G")
        Dim hinweis As String = ""
        Dim result As String = meineHttpNet.meinHttpJob(mset.ProxyString, aufruf, hinweis, enc, 5000)
        nachricht(hinweis)
        nachricht("result: " & result)
        modKoordTrans.getLongLatFromResultSingle(result, longitude, latitude)
        nachricht(hinweis)
        result = "https://www.google.com/maps/@" & latitude.Replace(",", ".") & "," & longitude.Replace(",", ".") & "," & CInt(bbox.xdif() / 2) & "m/data=!3m1!1e3"
        Return result
        'laenge = longitude : breite = latitude
        ''GMtemplates.SetLatitude(aktp, longitude, latitude)
        'Dim TEXTKOERPER As String = "Bitte benutzen Sie das <b>Snipping Tool</b> um das Luftbild zu kopieren. " &
        '    "Sie können es dann über den Knopf <b>Zwischenablage</b> in Paradigma einfügen!"
        ''templateFile = initP.getValue("Beide.GoogleMapsTemplateDir") & "Infowindow.htm"
        '' "\\file-paradigma\paradigma\test\paradigmaArchiv\div\GMapTemplates\Infowindow.htm" 
        'templateFile = "Infowindow.htm" ' liegt nun im executebaledir



        'Dim punkteInWinkelkoordinaten() As myPoint

        ''  punkteInWinkelkoordinaten = GMtemplates.konvertierePunkteArrayVonUTMnachWinkel(myGlobalz.punktarrayInM)

        'Dim templ As String = GMtemplates.templateEinlesen(templateFile)

        'If templ.IsNothingOrEmpty Or Not htmlformat Then
        '    'templatedatei fehlt
        '    ''https://maps.google.com/maps?ll=50.0030653020894,8.76937026434553&t=h
        '    'Dim http As String = "https://maps.google.com/maps?ll=" & latitude.Replace(",", ".") & "," & longitude.Replace(",", ".") & "&t=h"
        '    Dim http As String = "https://www.google.com/maps/@" &
        '        latitude.Replace(",", ".") & "," & longitude.Replace(",", ".") & ",355a,20y,41.6t/data=!3m1!1e3"
        '    'starten(http)
        '    nachricht("http: " & http)
        '    Return http
        'Else
        '    templ = GMtemplates.templateAnpassen(templ,
        '                                  coords:=latitude.Replace(",", ".") & "," & longitude.Replace(",", "."),
        '                                  title:="P-Gis: GoogleMaps ",
        '                                  polygon:=punkteInWinkelkoordinaten,
        '                                  TEXTKOERPER:=TEXTKOERPER)
        '    templ = templateAuschreiben(templ)
        '    'templateStarten(templ)
        '    nachricht("templ: " & templ)
        '    Return templ
        'End If
    End Function

End Class
