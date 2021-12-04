Module modImagemapDisp

    'Public Function imageMap2POintCollLIstMAPSERVER(ByVal imageMap As String,
    '                                           eigentuemerFunktionAktiv As Boolean) As List(Of pointCollectionPlus)
    '    ' malt ohne skalierung
    '    Dim line As String
    '    Dim dauer As Integer = 500
    '    Dim coundauer As Integer = 1
    '    Try
    '        Dim tempPluscoll As New pointCollectionPlus
    '        Dim zeile() As String
    '        zeile = imageMap.Split(CType(vbCrLf, Char()))
    '        'bei mapserv70 vbCrLf
    '        'bei mapserv60 vbCr
    '        Dim coords As String
    '        Dim shape As String
    '        Dim href As String
    '        Dim title As String
    '        Dim zaehler As Integer = 0
    '        Dim eigentuemerTitle As String = ""
    '        'Dim myPointCollection As PointCollection
    '        Dim LokimapPointPLusColl As New List(Of pointCollectionPlus)
    '        Dim textMarker As String
    '        If zeile(0).Contains("'") Then textMarker = "'"
    '        If zeile(0).Contains(Chr(34)) Then textMarker = Chr(34)
    '        textMarker = Chr(34)
    '        'Dim eigentuemerFunktionAktiv As Boolean = True
    '        'For i = zeile.GetUpperBound(0) To 0 Step -1
    '        For i = 0 To zeile.GetUpperBound(0)
    '            tempPluscoll = New pointCollectionPlus
    '            zaehler += 1
    '            If zeile(i).ToLower.Contains("</map>") Or zeile(i).ToLower.Contains("<map id=") Then
    '                tempPluscoll.newImagemap = True
    '            Else
    '                tempPluscoll.newImagemap = False
    '            End If
    '            line = removeMapTag(zeile(i))
    '            If line.IsNothingOrEmpty Then Continue For
    '            shape = GetTagValueMS(line, "shape=") : If shape.IsNothingOrEmpty Then Continue For
    '            coords = GetTagValueMS(line, "coords=") : If coords.IsNothingOrEmpty Then Continue For
    '            title = GetTagValueTitle(line)
    '            href = GetTagValueMS(line, "href=")
    '            'If eigentuemerFunktionAktiv Then
    '            '    Dim fs As String
    '            '    fs = getfsfromLine(line)
    '            '    title = getEigentuemerTitle4FS(fs, "select tooltip from fs2eigentuemer where fsgml='")
    '            'End If

    '            If shape = "point" Then
    '                'coords = GetQuadrat(coords)
    '                coords = GetQuadrat4Circle(coords)
    '            End If

    '            If shape = "circle" Then
    '                coords = GetQuadrat4Circle(coords)
    '            End If

    '            tempPluscoll.pcoll = bildeMyPointCollectionMapserver(coords)
    '            tempPluscoll.href = href
    '            tempPluscoll.title = title
    '            tempPluscoll.typ = shape 'line, poly oder circle
    '            LokimapPointPLusColl.Add(tempPluscoll)
    '        Next
    '        Return LokimapPointPLusColl
    '    Catch ex As Exception
    '        nachricht("Fehler in imageMap2PolygonMap: " ,ex)
    '        Return Nothing
    '    End Try
    'End Function

    'Private Function GetQuadrat4Circle(coords As String) As String
    '    Dim p As String(), quadrat As String
    '    Dim xl, xh, yl, yh As String
    '    Dim radius As Double
    '    Try
    '        If coords.IsNothingOrEmpty Then
    '            nachricht("GetQuadrat coords ist leer. exit")
    '            Return ""
    '        End If
    '        p = coords.Split(","c)
    '        If p.Length() = 2 Then
    '            radius = 10
    '        Else
    '            radius = CDbl(p(2))
    '        End If
    '        xl = CStr(CInt(p(0)) - CInt(radius))
    '        xh = CStr(CInt(p(0)) + CInt(radius))
    '        yl = CStr(CInt(p(1)) - CInt(radius))
    '        yh = CStr(CInt(p(1)) + CInt(radius))

    '        'quadrat = p(0) & "," & p(1) & "," & p(0) & "," & p(3) & "," & p(2) & "," & p(3) & "," & p(2) & "," & p(1) & "," & p(0) & "," & p(1)
    '        quadrat = xl & "," & yl &
    '            " " & xl & "," & yh &
    '            " " & xh & "," & yh &
    '            " " & xh & "," & yl &
    '            " " & xl & "," & yl

    '        'quadrat = xl & "," & yl &
    '        '    " " & xh & "," & yl &
    '        '    " " & xh & "," & yh &
    '        '    " " & xl & "," & yh &
    '        '    " " & xl & "," & yl
    '        Return quadrat
    '    Catch ex As Exception
    '        nachricht("fehler in GetQuadrat coords : " ,ex)
    '        Return ""
    '    End Try
    'End Function

    'Function bildeMyPointCollectionMapserver(ByVal coords As String) As PointCollection
    '    Dim punkte() As String
    '    Dim myPointCollection As New PointCollection
    '    Dim x, y As Integer
    '    Dim i As Integer
    '    If coords.IsNothingOrEmpty Then Return Nothing
    '    Dim b() As String
    '    Try
    '        punkte = coords.Split(" "c)
    '        For i = 0 To punkte.GetUpperBound(0) - 1 Step 1
    '            b = punkte(i).Split(","c)
    '            x = CInt(b(0))
    '            y = CInt(b(1))
    '            myPointCollection.Add(New Point(x, y))
    '        Next
    '        Return myPointCollection
    '    Catch ex As Exception
    '        nachricht("fehler in bildeMyPointCollection: " ,ex)
    '        Return Nothing
    '    End Try
    'End Function

    'Function bildeMyPointCollection(ByVal coords As String) As PointCollection
    '    Dim punkte() As String
    '    Dim myPointCollection As New PointCollection
    '    Dim x, y As Integer
    '    If coords.IsNothingOrEmpty Then Return Nothing
    '    Try
    '        punkte = coords.Split(","c)
    '        For i = 0 To punkte.GetUpperBound(0) Step 2
    '            x = CInt(punkte(i))
    '            y = CInt(punkte(i + 1))
    '            myPointCollection.Add(New Point(x, y))
    '        Next
    '        x = CInt(punkte(0))
    '        y = CInt(punkte(0 + 1))
    '        myPointCollection.Add(New System.Windows.Point(x, y))
    '        Return myPointCollection
    '    Catch ex As Exception
    '        nachricht("fehler in bildeMyPointCollection: " ,ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Private Function GetTagValue(ByVal line As String, ByRef tag As String) As String
    '    Dim rest As String, pos As Integer, textMarker As String = "'"
    '    nachricht("GetTagValue --------  ")

    '    Try
    '        If line.IsNothingOrEmpty Then Return ""
    '        If tag.IsNothingOrEmpty Then Return ""

    '        nachricht("GetTagValue -------- eingabe ist gültig")
    '        If Not line.ToLower.Contains(tag.ToLower) Then Return ""

    '        If tag.ToLower = "shape=" Then
    '            If line.ToLower.Contains("shape=point") Then Return "point"
    '            If line.ToLower.Contains("shape=polygon") Then Return "polygon"
    '            If line.ToLower.Contains("shape=annotation") Then Return "annotation"
    '        End If

    '        textMarker = clsString.getTextDelimiter(line)
    '        'typen coord und title und href
    '        nachricht("GetTagValue -------- eingabe ist gültig")
    '        pos = line.IndexOf(tag)
    '        ' blank=
    '        If pos > 0 Then
    '            pos = pos + tag.Length + 1
    '            rest = line.Substring(pos, line.Length - pos)
    '            pos = rest.IndexOf(" ")
    '            If pos < 0 Then
    '                ' weil href am ende liegt gibts kein blank
    '                pos = rest.Length - 2
    '            End If
    '            rest = rest.Substring(0, pos)
    '            rest = rest.Replace(Chr(34), "").Replace("'", "")
    '        Else
    '            Return ""
    '        End If
    '        Return rest
    '        Return ""
    '    Catch ex As Exception
    '        nachricht("fehler in GetTagValue: " ,ex)
    '        Return ""
    '    End Try
    'End Function


    'Private Function GetTagValueMS(ByVal line As String, ByRef tag As String) As String
    '    Dim rest As String, pos As Integer ', textMarker As String = "'"
    '    ' nachricht("GetTagValue --------  ")

    '    Try
    '        If line.IsNothingOrEmpty Then Return ""
    '        If tag.IsNothingOrEmpty Then Return ""

    '        '  nachricht("GetTagValue -------- eingabe ist gültig")
    '        If Not line.ToLower.Contains(tag.ToLower) Then Return ""
    '        'shape="poly"
    '        If tag.ToLower = "shape=" Then
    '            If line.ToLower.Contains("shape=point") Then Return "point"
    '            If line.ToLower.Contains("shape=polygon") Or
    '                line.ToLower.Contains("shape=""poly""") Then Return "polygon"
    '            If line.ToLower.Contains("shape=annotation") Then Return "annotation"
    '        End If

    '        'typen coord und title und href
    '        ' nachricht("GetTagValue -------- eingabe ist gültig")
    '        'coords="0,0 1022,0 1037,16 1121,38 1121,0 1121,624 1121,464 1066,428 904,283 750,157 582,83 396,20 174,3 55,0 0,0"
    '        pos = line.IndexOf(tag)
    '        If pos > 0 Then
    '            pos = pos + tag.Length + 1
    '            rest = line.Substring(pos, line.Length - pos)
    '            pos = rest.IndexOf("""")
    '            If pos < 0 Then
    '                ' weil href am ende liegt gibts kein blank
    '                pos = rest.Length - 2
    '            End If
    '            rest = rest.Substring(0, pos)
    '            rest = rest.Replace(Chr(34), "").Replace("'", "")
    '        Else
    '            Return ""
    '        End If
    '        rest = rest.Replace("href=", "")
    '        Return rest

    '        pos = line.IndexOf(tag)
    '        ' blank=
    '        Return ""
    '    Catch ex As Exception
    '        nachricht("fehler in GetTagValue: " ,ex)
    '        Return ""
    '    End Try
    'End Function

    'Friend Sub cleanupImagemap(ByRef imageMap As String)
    '    Dim temp As String = imageMap
    '    If imageMap.IsNothingOrEmpty Then
    '        imageMap = ""
    '    End If
    '    Dim rx = New System.Text.RegularExpressions.Regex("</map><map id=""m-imagemap"" name=""m-imagemap"">")
    '    Dim a As String() '</map><map id="m-imagemap" name="m-imagemap">
    '    a = rx.Split(imageMap) '"</map><map id=""m-imagemap"" name=""m-imagemap"">")
    '    Dim summe As String = ""
    '    For i = a.Count - 1 To 0 Step -1
    '        temp = removeMapTag(a(i))
    '        'temp = temp.Replace(" < map id=""m-imagemap"" name=""m-imagemap"">", "")
    '        'temp = temp.Replace("<map id=""m-imagemap"" name=""m-imagemap"">", "")
    '        'temp = temp.Replace("</map>", "").Trim & vbCrLf
    '        summe = summe & temp
    '    Next

    '    imageMap = summe
    'End Sub

    'Private Function removeMapTag(temp As String) As String
    '    Try
    '        l("removeMapTag---------------------- anfang")
    '        temp = temp.Replace("</map>", "").Trim
    '        temp = temp.Replace(" < map id=""m-imagemap"" name=""m-imagemap"">", "")
    '        temp = temp.Replace("<map id=""m-imagemap"" name=""m-imagemap"">", "")
    '        Return temp
    '        l("removeMapTag---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in removeMapTag: ", ex)
    '        Return ""
    '    End Try
    'End Function

    'Private Function GetTagValueTitle(line As String) As String
    '    Dim tag = "title="
    '    Dim rest As String, pos As Integer
    '    ' nachricht("GetTagValueTitle --------  ")
    '    Try
    '        If line.IsNothingOrEmpty Then Return ""
    '        '       nachricht("GetTagValueTitle -------- eingabe ist gültig")
    '        If Not line.ToLower.Contains(tag.ToLower) Then Return ""
    '        'typen coord und title und href
    '        ' nachricht("GetTagValueTitle -------- eingabe ist gültig")
    '        pos = line.IndexOf(tag)
    '        Dim titeltrenner As String
    '        If pos > 0 Then
    '            pos = pos + tag.Length + 1
    '            rest = line.Substring(pos, line.Length - pos)
    '            If rest.StartsWith("'") Then
    '                titeltrenner = "'"
    '            Else
    '                titeltrenner = """"
    '            End If
    '            pos = rest.IndexOf(titeltrenner)
    '            If pos < 0 Then
    '                ' weil href am ende liegt gibts kein blank
    '                pos = rest.Length - 2
    '            End If
    '            rest = rest.Substring(0, pos)
    '            rest = rest.Replace(Chr(34), "").Replace("'", "")
    '        Else
    '            Return ""
    '        End If
    '        Return rest
    '    Catch ex As Exception
    '        l("fehler in GetTagValue: " ,ex)
    '        Return ""
    '    End Try
    'End Function
    'Private Function getfsfromLine(line As String) As String
    '    Dim pos As Integer
    '    Dim fs As String
    '    Try
    '        pos = line.IndexOf("show_MYDB(")
    '        pos = pos + 10
    '        fs = line.Substring(pos, 22)
    '        fs = fs.Replace("'", "")
    '        Return fs
    '    Catch ex As Exception
    '        Return "o.A."
    '    End Try

    'End Function

    'Private Function GetQuadrat(ByVal coords As String) As String
    '    Dim p As String(), quadrat As String

    '    Try
    '        If coords.IsNothingOrEmpty Then
    '            nachricht("GetQuadrat coords ist leer. exit")
    '            Return ""
    '        End If
    '        p = coords.Split(","c)
    '        quadrat = p(0) & "," & p(1) & "," & p(0) & "," & p(3) & "," & p(2) & "," & p(3) & "," & p(2) & "," & p(1) & "," & p(0) & "," & p(1)
    '        Return quadrat
    '    Catch ex As Exception
    '        nachricht("fehler in GetQuadrat coords : " ,ex)
    '        Return ""
    '    End Try
    'End Function
    'Sub polygonmalen(ByVal href As String,
    '                        ByVal title As String,
    '                        ByVal myPointCollection As PointCollection,
    '                        ByVal lokcanvas As Canvas)
    '    'withevents muss auf klassenebene deklariert sein   Private WithEvents myPolygon As Polygon
    '    Dim myPolygon As New Polygon
    '    Try
    '        If IsNothing(myPointCollection) Then
    '            nachricht("warnung in polygonmalen: myPointCollection  ist nothing")
    '        End If
    '        '  myPolygon.Name = "poly" & zaehler
    '        myPolygon.ToolTip = title & Environment.NewLine & " Linke MausTaste = DB, Rechte MT = DB-Menü "
    '        myPolygon.Tag = href
    '        myPolygon.Stroke = Brushes.Black
    '        ' myPolygon.StrokeThickness = 2
    '        myPolygon.Fill = Brushes.Transparent
    '        'myPolygon.Fill = Brushes.Black
    '        myPolygon.StrokeThickness = 0
    '        myPolygon.Cursor = System.Windows.Input.Cursors.ArrowCD
    '        '   myPolygon.Cursor = New Cursor(Environment.CurrentDirectory & "\Cursor1.cur")
    '        myPolygon.ForceCursor = True

    '        myPolygon.Points = myPointCollection

    '        AddHandler myPolygon.MouseDown, AddressOf clsMiniMapTools.Polygon_MouseDown
    '        AddHandler myPolygon.MouseRightButtonDown, AddressOf clsMiniMapTools.Polygon_MouseRightButtonDown
    '        lokcanvas.Children.Add(myPolygon)
    '        Canvas.SetZIndex(myPolygon, 100)
    '        Canvas.SetLeft(myPolygon, 0)
    '        Canvas.SetTop(myPolygon, 0)
    '        'myPolygon.Cursor = System.Windows.Input.Cursors.ArrowCD
    '    Catch ex As Exception
    '        nachricht("fehler in polygonmalen: " ,ex)
    '    End Try
    'End Sub
#Region "aus dem mainwindow.xaml.vb"
    '    Private Shared Sub paintIMapCollection(imapPointPLusColl As List(Of pointCollectionPlus), lokcanvas As Canvas)
    '        Try
    '            l(" paintIMapCollection ---------------------- anfang")
    '            If imapPointPLusColl Is Nothing Then Exit Sub
    '            For Each tp As pointCollectionPlus In imapPointPLusColl
    '                If Not IsNothing(tp.pcoll) Then
    '                    polygonmalen(tp.href, tp.title, tp.pcoll, lokcanvas)
    '                End If
    '            Next
    '            l(" paintIMapCollection ---------------------- ende")

    '        Catch ex As Exception
    '            l("Fehler in paintIMapCollection: " & ex.ToString())

    '        End Try
    '    End Sub




    '    Private Shared Function istImageMapOK() As Boolean
    '        Try
    '            If kartengen.imageMap Is Nothing Then Return False
    '            If kartengen.imageMap.ToLower.Contains("Search returned no results".ToLower) OrElse
    '                kartengen.imageMap = String.Empty Then
    '                l("warnung in istImageMapOK: Search returned no results, es wurde keine imagemap erzeugt: " & kartengen.imageMap & Environment.NewLine &
    '" layerActive.aid = 0 Or layerActive.mapFileHeader = String.Empty" & layerActive.aid & " " & layerActive.mapFileHeader & Environment.NewLine &
    'layerActive.titel)
    '                Return False
    '            Else
    '                Return True
    '            End If
    '        Catch ex As Exception
    '            l("fehler in istImageMapOK" ,ex)
    '            Return False
    '        End Try
    '    End Function

    '    'Private Function maleImageMap() As Integer
    '    Dim inselnInImageMap As Integer
    '    Dim newPcoll As New List(Of pointCollectionPlus)
    '    Dim dummy As Integer = 0
    '    Dim imapPointPLusColl As New List(Of pointCollectionPlus)
    '    Try
    '    If istImageMapOK() Then
    '                ' modImagemapDisp.cleanupImagemap(kartengen.imageMap) 
    '                imapPointPLusColl = imageMap2POintCollLIstMAPSERVER(kartengen.imageMap, False)
    '                imapPointPLusColl = prep(imapPointPLusColl, inselnInImageMap)
    '                If imapPointPLusColl Is Nothing Then
    '                    l("Warnung imapPointPLusColl is nothing")
    '                End If
    '    If inselnInImageMap > 10 Then
    '                    inselnInImageMap = 10
    '                End If
    '    '  MsgBox(inselCountImSuchPolygon.ToString)
    '    For j = 0 To inselnInImageMap + 1
    '                    newPcoll = bildeNeueListe(imapPointPLusColl)
    '                    If newPcoll IsNot Nothing Then
    '                        imapPointPLusColl = prep(newPcoll, dummy)
    '                        If imapPointPLusColl Is Nothing Then
    '                            l("Warnung imapPointPLusColl is nothing")
    '                        End If
    '    End If

    '    Next
    '                paintIMapCollection(imapPointPLusColl, WebBrowser1)
    '                'allesImWebbrowserdarstellen(BILDaufruf, BILDaufrufRange0)
    '            End If
    '    Return inselnInImageMap
    '    Catch ex As Exception
    '            l("fehler in maleImageMap: " ,ex)
    '            Return Nothing
    '    End Try
    '    End Function
#End Region
End Module
