Imports System.ComponentModel
Imports mgis
Imports CefSharp
Imports Newtonsoft.Json
Public Class winWMShtmlDB
    Private url As String = ""
    Private winformbreite, winformhoehe, pointx, pointy, hoehe, breite, fensterzaehler As Integer
    Public titel As String
    Public layer As New clsLayerPres
    Public hinweis, result, bbox, wmslayers, wmsquery_layers, infoformat As String

    Public legdatei, dokdatei, dokHtml As String
    Public Property ladevorgangabgeschlossen As Boolean = False

    Private Sub btnLegende_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Sub New(_winformwidth As Integer, _winformheight As Integer, _layer As clsLayerPres,
            _bbox As String, _hoehe As Integer, _breite As Integer,
            _pointx As Integer, _pointy As Integer,
            _wmslayers As String, _wmsquery_layers As String, _fensterzaehler As Integer)
        InitializeComponent()
        winformbreite = _winformwidth
        winformhoehe = _winformheight
        wmslayers = _wmslayers
        wmsquery_layers = _wmsquery_layers
        layer = _layer
        pointx = _pointx
        pointy = _pointy
        hoehe = _hoehe
        breite = _breite
        bbox = _bbox
        fensterzaehler = _fensterzaehler
    End Sub

    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub winWMShtmlDB_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        Dim ausgabedatei As String = ""
        Width = winformbreite : Height = winformhoehe : Title = "WMS-Datenbankabfrage"
        setInfoFormat()
        url = clsWMStools.calcWMSGetfeatureInfoURL(bbox, layer, hoehe, breite,
                                                    pointx, pointy, infoformat,
                                                    layer.wmsProps.stdlayer, layer.wmsProps.stdlayer)
        handleHTMLinfoformat()
        If handleJson(ausgabedatei) Then
            If Not ausgabedatei.IsNothingOrEmpty Then
                wb1.Load("file:///" & ausgabedatei)
            End If
        Else
            ' Close()
        End If
        dokdatei = nsMakeRTF.rtf.makeDokuHtml(layerActive, dokHtml, layerActive.aid)
        legdatei = nsMakeRTF.rtf.makeftlLegende4Aid(layerActive, "html", dokHtml)

        If legdatei.IsNothingOrEmpty Then
            rtfTextDoku = "Keine Legende vorhanden"
            'tidok.IsSelected = True
        Else
            Dim fi As IO.FileInfo
            fi = New IO.FileInfo(legdatei)
            If fi.Exists Then
                wbleg.Navigate("file:///" & legdatei)
            End If
        End If

        If fensterzaehler > 0 Then
            Me.Top = clsToolsAllg.setPosition("diverse", "wmsabfrageformpositiontop", Me.Top) + fensterzaehler * 25
            Me.Left = clsToolsAllg.setPosition("diverse", "wmsabfrageformpositionleft", Me.Left) + fensterzaehler * 20
        Else
            Me.Top = clsToolsAllg.setPosition("diverse", "wmsabfrageformpositiontop", Me.Top)
            Me.Left = clsToolsAllg.setPosition("diverse", "wmsabfrageformpositionleft", Me.Left)
        End If
        ladevorgangabgeschlossen = True
    End Sub

    Private Function handleJson(ByRef ausgabedatei As String) As Boolean
        Dim test As Boolean
        Try
            l(" MOD handleJson anfang")
            l("handleJson")
            If layer.wmsProps.format <> "application/geojson" Then
                Return True
            End If
            l("application/geojson: " & layer.titel)

            test = clsTrinkwasser.DoTrinkw418(layer, ausgabedatei, bbox, hoehe, breite,
                                                  pointx, pointy, infoformat,
                                                  layer.wmsProps.stdlayer, layer.wmsProps.stdlayer)
            If test Then Return True
            test = clsRegfnp.DoRegFNP(layer, ausgabedatei, bbox, hoehe, breite,
                                                    pointx, pointy, infoformat,
                                                    layer.wmsProps.stdlayer, layer.wmsProps.stdlayer)
            If test Then Return True

            l(" MOD handleJson ende")
            Return False
        Catch ex As Exception
            l("Fehler in handleJson: ", ex)
            Return False
        End Try
    End Function
    Private Function handleHTMLinfoformat() As Boolean
        Try
            l(" MOD handleHTMLinfoformat anfang")
            If layer.wmsProps.format = "h" Then
                url = clsWMStools.calcWMSGetfeatureInfoURL(bbox, layer, hoehe, breite,
                                                pointx, pointy, infoformat,
                                                layerActive.wmsProps.stdlayer, layerActive.wmsProps.stdlayer)
                If url.IsNothingOrEmpty Then
                    l("Fehler in handleHTMLinfoformat: url is leer" & url)
                    Return False
                Else
                    wb1.Load(url)
                    Width = Width * 1.5
                    Return True
                End If
            End If
            l(" MOD handleHTMLinfoformat ende")
            Return True
        Catch ex As Exception
            l("Fehler in handleHTMLinfoformat: " & url & Environment.NewLine &
              layerActive.wmsProps.getstring(";"), ex)
            Return False
        End Try
    End Function
    Private Sub setInfoFormat()
        If layer.wmsProps.format = "h" Then
            infoformat = "text/html"
        End If
        If layer.wmsProps.format = "p" Then
            infoformat = "text/plain"
        End If
        If layer.wmsProps.format = "application/geojson" Then
            infoformat = "application/geojson"
        End If
    End Sub

    Private Sub winWMShtmlDB_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If fensterzaehler < 1 Then savePosition()
    End Sub
    Private Sub savePosition()
        Try
            userIniProfile.WertSchreiben("diverse", "wmsabfrageformpositiontop", CType(Me.Top, String))
            userIniProfile.WertSchreiben("diverse", "wmsabfrageformpositionleft", CType(Me.Left, String))
        Catch ex As Exception
            l("fehler in saveposition  windb", ex)
        End Try
    End Sub
End Class

