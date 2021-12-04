Imports mgis

Public Class clsGISfunctions

    'Dim latitude As String
    'Dim longitude As String
    'Public Function GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(ByVal bbox As clsRange, htmlformat As Boolean, ByRef longitude As String, ByRef latitude As String) As String
    '    nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe -----------------------------------")
    '    l("aktrange=" & bbox.toString)
    '    Dim lonlatstring, templateFile, quellstring, hinweis As String
    '    lonlatstring = getLonLatFrombbox(bbox, htmlformat, xyTrenner, longitude, latitude)
    '    'GMtemplates.SetLatitude(aktp, longitude, latitude)
    '    Dim TEXTKOERPER As String = "Bitte benutzen Sie das <b>Snipping Tool</b> um das Luftbild zu kopieren. " &
    '        "Sie können es dann über den Knopf <b>Zwischenablage</b> in Paradigma einfügen!"
    '    templateFile = strGlobals.google3Dtemplate
    '    nachricht("2")

    '    'quellstring = modKoordTrans.bildeQuellKoordinatenString(punktarrayInM)
    '    'l("quellstring " & quellstring)
    '    'aufruf = modKoordTrans.bildeaufruf4KoordinatenServer(quellstring, punktarrayInM.Count.ToString, "UTM", "WINKEL_G")
    '    'nachricht("result: " & lonlatstring)
    '    'nachricht("aufruf: " & aufruf)
    '    'Dim punkteInWinkelkoordinaten() As myPoint
    '    nachricht(hinweis)
    '    nachricht("3")
    '    nachricht("result: " & lonlatstring)
    '    'punkteInWinkelkoordinaten = modKoordTrans.getLongLatFromResultBulk(lonlatstring)
    '    Dim longitude, latitude As String
    '    modKoordTrans.getLongLatFromResultSingle(lonlatstring, longitude, latitude, xyTrenner)
    '    '  punkteInWinkelkoordinaten = GMtemplates.konvertierePunkteArrayVonUTMnachWinkel(myGlobalz.punktarrayInM)


    '    Dim templ As String = ""
    '    templ = GMtemplates.templateEinlesen(templateFile)
    '    'MsgBox("templ " & templ & Environment.NewLine & htmlformat)
    '    'MsgBox("htmlformat " & htmlformat)
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
    '                                      polygon:=Nothing,
    '                                      TEXTKOERPER:=TEXTKOERPER)
    '        templ = templateAuschreiben(templ)
    '        'templateStarten(templ)

    '        nachricht("templ: " & templ)
    '        Return templ
    '    End If
    'End Function

    Private Function getLonLatFrombbox(bbox As clsRange, htmlformat As Boolean, xyTrenner As Char, ByRef longitude As String, ByRef latitude As String) As String
        Dim aktp As New myPoint
        Dim abstand As Double
        Dim lonlatstring, quellstring, aufruf, hinweis As String
        Dim cnt = "1"
        l(" MOD getLonLatFrombbox anfang")
        l(" bbox " & bbox.toString)
        l(" htmlformat " & htmlformat.ToString)
        l(" xyTrenner " & xyTrenner.ToString)
        Try
            If Not bbox.istBrauchbar Then
                l("getLonLatFrombbox Not bbox.istBrauchbar")
                Return "fehler"
            End If
            cnt = "2"
            aktp.X = bbox.xl + bbox.xdif() / 2
            aktp.Y = bbox.yl + bbox.ydif() / 2
            abstand = bbox.xdif
            cnt = "3"
            If Not coordIstOk(aktp) Then
                MessageBox.Show("Leider ist die Koordinatenangabe unbrauchbar! c" & vbCrLf & aktp.toString())
                nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe fehler" & aktp.toString())
                Return "fehler"
            End If
            cnt = "4"
            ReDim punktarrayInM(0)
            punktarrayInM(0) = aktp
            nachricht("1")
            nachricht("aktp: " & aktp.toString)
            cnt = "5"
            quellstring = modKoordTrans.bildeQuellKoordinatenString(punktarrayInM)
            cnt = "6"
            aufruf = modKoordTrans.bildeaufruf4KoordinatenServer(quellstring, punktarrayInM.Count.ToString, "UTM", "WINKEL_G")
            cnt = "7"
            hinweis = ""
            l("aufruf " & aufruf)
            lonlatstring = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            cnt = "8"
            If lonlatstring.IsNothingOrEmpty Then
                l("lonlatstring ist leerstring !!!")
                Return ""
            Else
                lonlatstring = lonlatstring.Trim
                nachricht(hinweis)
                nachricht("result: " & lonlatstring)
                'Dim longitude, latitude As String
                cnt = "9"
                modKoordTrans.getLongLatFromResultSingle(lonlatstring, longitude, latitude, xyTrenner)
                cnt = "10"
                Return lonlatstring.Trim
            End If
            l(" MOD getLonLatFrombbox ende")
        Catch ex As Exception
            l("Fehler in getLonLatFrombbox: " & cnt & "   " & aufruf & " // " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Function coordIstOk(ByVal aktp As myPoint) As Boolean
        Try
            l(" MOD coordIstOk anfang")
            If String.IsNullOrEmpty(aktp.X.ToString) Then
                Return False
            End If
            If aktp.X < 340000 Then
                Return False
            End If
            l(" MOD coordIstOk ende")
            Return True

            Return True
        Catch ex As Exception
            l("Fehler in coordIstOk: " & ex.ToString())
            Return False
        End Try
    End Function


    Public Function GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(ByVal bbox As clsRange, htmlformat As Boolean,
                                                                   ByRef longitude As String, ByRef latitude As String) As String
        nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe -----------------------------------")
        Dim aktp As New myPoint
        Dim abstand As Double
        Dim templateFile As String
        aktp.X = CInt(bbox.xl + bbox.xdif() / 2)
        aktp.Y = CInt(bbox.yl + bbox.ydif() / 2)
        abstand = bbox.xdif
        If Not coordIstOk(aktp) Then
            MessageBox.Show("Leider ist die Koordinatenangabe unbrauchbar a! " & vbCrLf & aktp.toString())
            nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe fehler" & aktp.toString())
            Return "fehler"
        End If
        Dim lonlatstring As String
        lonlatstring = getLonLatFrombbox(bbox, htmlformat, xyTrenner, longitude, latitude)
        l("longitude " & longitude)
        l("latitude " & latitude)
        l("lonlatstring " & lonlatstring)
        If longitude.IsNothingOrEmpty OrElse latitude.IsNothingOrEmpty Then
            Return ""
        Else
            Dim punkteInWinkelkoordinaten() As myPoint
            nachricht("3")
            nachricht("result: '" & lonlatstring & "'")

            Dim TEXTKOERPER As String = "Bitte benutzen Sie das <b>Snipping Tool</b> um das Luftbild zu kopieren. " &
                "Sie können es dann über den Knopf <b>Zwischenablage</b> in Paradigma einfügen!"

            templateFile = strGlobals.google3Dtemplate

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
                templ = templateAuschreiben(templ)
                'templateStarten(templ)
                nachricht("templ: " & templ)
                Return templ
            End If
        End If



    End Function
    Public Function GoogleMapsAufruf_Extern(ByVal bbox As clsRange, htmlformat As Boolean, ByRef lonlatString As String) As String
        nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe -----------------------------------")
        Dim result As String = ""
        Dim longitude, latitude As String
        lonlatString = getLongitudelatitude(bbox, htmlformat, longitude, latitude)

        result = "https://www.google.com/maps/@" & latitude.Replace(",", ".") & "," & longitude.Replace(",", ".") & "," & CInt(bbox.xdif() / 2) & "m/data=!3m1!1e3"
        Return result

    End Function

    Private Function getLongitudelatitude(bbox As clsRange, htmlformat As Boolean, ByRef longitude As String, ByRef latitude As String) As String
        Dim aktp As New myPoint
        Dim abstand As Double
        Dim lonlatString As String = ""
        Try
            l(" MOD getLongitudelatitude anfang")

            aktp.X = CInt(bbox.xl + bbox.xdif() / 2)
            aktp.Y = CInt(bbox.yl + bbox.ydif() / 2)
            abstand = bbox.xdif

            If Not coordIstOk(aktp) Then
                MessageBox.Show("Leider ist die Koordinatenangabe unbrauchbar! b" & vbCrLf & aktp.toString())
                nachricht("GoogleMapsAufruf_MittelpunktMItPunkteUebergabe fehler" & aktp.toString())
                Return "fehler"
            End If
            ReDim punktarrayInM(0)
            punktarrayInM(0) = aktp
            nachricht("1")
            nachricht("aktp: " & aktp.toString)
            Dim quellstring As String = modKoordTrans.bildeQuellKoordinatenString(punktarrayInM)
            Dim aufruf As String = modKoordTrans.bildeaufruf4KoordinatenServer(quellstring, punktarrayInM.Count.ToString, "UTM", "WINKEL_G")
            Dim hinweis As String = ""
            Dim result As String = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            nachricht("result: " & result)

            modKoordTrans.getLongLatFromResultSingle(result, longitude, latitude, xyTrenner)
            nachricht(hinweis)
            lonlatString = latitude.Replace(",", ".") & "#" & longitude.Replace(",", ".")
            l(" MOD getLongitudelatitude ende")
            Return lonlatString
        Catch ex As Exception
            l("Fehler in getLongitudelatitude: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
