Imports mgis

Module modWKT
    Public aktColl As New List(Of pointCollectionPlus)
    Public bereinigt As New List(Of pointCollectionPlus)


    Function wkt2PointCollList(wkt As String) As List(Of PointCollection)
        Try
            If wkt.IsNothingOrEmpty Then
                Return Nothing
            End If
            wkt = wkt.Replace("N(", "N (")
            Dim wktType = wkt.Substring(0, wkt.IndexOf(" ("))
            Dim PointCollectionList As New List(Of PointCollection)
            Dim bereinigt As New List(Of pointCollectionPlus)
            Dim istShapetyp = True
            If istShapetyp Then
                wkt = wkt.Replace(",", "#")
                wkt = wkt.Replace(".", ",")
                If wkt.Contains(";") Then
                    Debug.Print("")
                End If
                'wkt = wkt.Replace("#", "")
                'wkt = wkt.Replace("", "")
            End If
            Select Case wktType.ToUpper.Trim
                Case "POLYGON"
                    PointCollectionList = makePolygon(wkt, 1)
                Case "MULTIPOLYGON"
                    PointCollectionList = makeMultiPolygon(wkt, 1)
                Case Else
                    l("fehler in wkt2PointCollList: wktType wurde nicht erkannt: " & wktType.ToUpper.Trim)
            End Select
            Return PointCollectionList
        Catch ex As Exception
            l("fehler In wkt2wpf ", ex)
            Return Nothing
        End Try
    End Function

    Function bildeNeueListe(bereinigt As List(Of pointCollectionPlus)) As List(Of pointCollectionPlus)
        Dim newP As New List(Of pointCollectionPlus)
        Try
            l("bildeNeueListe---------------------- anfang")
            If bereinigt Is Nothing Then
                Return Nothing
            End If
            For i = 0 To bereinigt.Count - 1
                newP.Add(bereinigt.Item(i))
            Next
            Return newP
            l("bildeNeueListe---------------------- ende")
        Catch ex As Exception
            l("Fehler in bildeNeueListea: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    'Private Sub bildeNeueListe(bereinigt As List(Of polygonPlus))  As List(Of PointCollection)
    '    For i = 0 To bereinigt.Count - 1
    '        newPcoll.Add(bereinigt.Item(i).pcoll)
    '    Next
    'End Sub

    Function makePolygon(wkt As String, faktor As Double) As List(Of PointCollection)
        Dim PolygonList As New List(Of PointCollection)
        Dim points As PointCollection
        Dim PointArray() As String
        Dim Coords() As String
        Try
            wkt = wkt.ToUpper.Trim
            wkt = wkt.Replace(")), ((", ")):((") : wkt = wkt.Replace(")),((", ")):((")
            wkt = wkt.Replace("), (", ");(") : wkt = wkt.Replace("),(", ");(")
            wkt = Replace(wkt, "POLYGON ((", "((")
            Debug.Print("sasd")
            'If wkt.Contains(");(") = True Then
            If wkt.Contains(")#(") = True Then
                'polygon hat innere ringe
                Dim rings = Split(wkt, ")#(")
                For Each ring In rings
                    Dim r = ring
                    r = r.Replace("(", "")
                    r = r.Replace(")", "")
                    'PointArray = Split(r, ",")
                    PointArray = Split(r, "#")
                    points = New PointCollection
                    For Each pnt In PointArray
                        Coords = Split(Trim(pnt), " ")
                        Dim x = CType(Coords(0), Double) * faktor
                        Dim y = CType(Coords(1), Double) * faktor
                        points.Add(New Point(x, y))
                    Next
                    PolygonList.Add(points)
                Next
            Else 'If Polygon does not have interior rings
                Dim r = wkt
                r = r.Replace("(", "")
                r = r.Replace(")", "")
                PointArray = Split(r, ",")
                PointArray = Split(r, "#")
                Dim xyTrenner As String = getXYTrenner(PointArray)
                points = New PointCollection
                For Each pnt In PointArray
                    Coords = Split(Trim(pnt), xyTrenner)
                    Dim x = CType(Coords(0), Double)
                    Dim y = CType(Coords(1), Double)
                    points.Add(New Point(x, y))
                Next
                PolygonList.Add(points)
            End If
            Return PolygonList
        Catch ex As Exception
            '  l("fehler in makePolygon " ,ex)
            Return Nothing
        End Try
    End Function

    Private Function getXYTrenner(PointArray() As String) As String
        Dim xyTrenner As String

        If PointArray(0).Contains("#") Then
            xyTrenner = "#"
        Else
            xyTrenner = " "
        End If

        Return xyTrenner
    End Function

    Function makeMultiPolygon(wkt As String, faktor As Double) As List(Of PointCollection)
        Try


            'Manipulate String to make it easier to split apart and indentify
            'Dim PolygonList As New List(Of PointCollection)
            Dim PointCollectionList As New List(Of PointCollection)
            Dim PolyArray() As String
            Dim i As Integer = 0
            'Dim points As List(Of Point)
            'Dim PointArray() As String
            wkt = wkt.Replace(")), ((", ")):((") : wkt = wkt.Replace(")),((", ")):((")
            wkt = wkt.Replace("), (", ");(") : wkt = wkt.Replace("),(", ");(")
            wkt = Replace(wkt, "MULTIPOLYGON (((", "((")
            wkt = Replace(wkt, ")))", "))")
            'Create an array of each polygon
            PolyArray = Split(wkt, ":")
            For Each ply In PolyArray
                'Dim figure As New MapPathFigure
                'Dim geometry As New MapPathGeometry
                'Dim path As New MapPath
                i = 0
                If ply.Contains(");(") = True Then 'If polygon has interior rings
                    Dim rings = Split(ply, ";")
                    For Each ring In rings
                        Dim r = ring
                        r = r.Replace("(", "")
                        r = r.Replace(")", "")
                        Dim PointArray() As String = Split(r, ",")
                        Dim points As New PointCollection
                        Dim xyTrenner As String = getXYTrenner(PointArray)
                        For Each pnt In PointArray
                            Dim Coords() = Split(Trim(pnt), xyTrenner)
                            Dim x = CType(Coords(0), Double) * faktor
                            Dim y = CType(Coords(1), Double) * faktor
                            points.Add(New Point(x, y))
                        Next
                        PointCollectionList.Add(points)
                        'Dim polyline As New MapPolyLineSegment
                        'polyline.Points = points
                        'If i = 0 Then
                        ' figure.StartPoint = points(0)
                        ' i = i + 1
                        'End If
                        'figure.Segments.Add(polyline)
                    Next
                    'geometry = New MapPathGeometry
                    'geometry.FillRule = FillRule.Nonzero
                    'geometry.Figures.Add(figure)
                    'path = New MapPath With {.Fill = Color, .Stroke = border, .StrokeThickness= 2, .Data = geometry}
                    'path = New MapPath With {.Fill = color, .Data = geometry}
                    'PolygonList.Add(points)
                Else 'If Polygon does not have interior rings
                    Dim r = ply
                    r = r.Replace("(", "")
                    r = r.Replace(")", "")
                    Dim PointArray() As String = Split(r, ",")
                    Dim points As New PointCollection
                    Dim xyTrenner As String = getXYTrenner(PointArray)
                    For Each pnt In PointArray
                        Dim Coords() = Split(Trim(pnt), xyTrenner)
                        Dim x = CType(Coords(0), Double) * faktor

                        Dim y = CType(Coords(1), Double) * faktor
                        points.Add(New Point(x, y))
                    Next
                    'Dim polyline As New MapPolyLineSegment
                    'polyline.Points = points
                    'figure.StartPoint = points(0)
                    'figure.Segments.Add(polyline)
                    'geometry = New MapPathGeometry
                    'geometry.FillRule = FillRule.EvenOdd
                    'geometry.Figures.Add(figure)
                    'path = New MapPath With {.Fill = Color, .Stroke = border, .StrokeThickness= 1, .Data = geometry}
                    PointCollectionList.Add(points)
                End If
            Next
            Return PointCollectionList
        Catch ex As Exception
            l("fehler in makeMultiPolygon ", ex)
            Return Nothing
        End Try
    End Function
    Function PolygonArea(ByVal points() As Point) As Single
        ' Return the absolute value of the signed area.
        ' The signed area is negative if the polyogn is
        ' oriented clockwise.
        Return CSng(Math.Abs(SignedPolygonArea(points)))
    End Function

    Function SignedPolygonArea(ByVal points() As Point) As Double
        Try
            If points Is Nothing Then
                l("warnung SignedPolygonArea keine punkte enthalten ")
                Return 0
            End If
            ' Add the first point to the end.
            ReDim Preserve points(points.Length)
            points(points.Length - 1) = points(0)
            ' Get the areas.
            Dim area As Double = 0
            For i As Integer = 0 To points.Length - 2
                area +=
                    (points(i + 1).X - points(i).X) *
                    (points(i + 1).Y + points(i).Y) / 2
            Next i
            ' Return the result
            Return area
        Catch ex As Exception
            l("fehler in SignedPolygonArea ", ex)
            Return 0
        End Try
    End Function
    Function getPointArray(mPolygon As PointCollection) As Point()
        Dim a As String = mPolygon.ToString
        Dim b() As String
        Dim c() As String
        Dim pts As Point()
        Try
            If a.IsNothingOrEmpty Then
                l("warnung ein polygon wurde nicht dargestellt")
                Return Nothing
            End If
            b = a.Split(" "c)

            ReDim pts(b.Length - 1)
            For i = 0 To b.Count - 1
                c = b(i).Split(";"c)
                pts(i).X = CDbl(c(0))
                pts(i).Y = CDbl(c(1))
            Next
            Return pts
        Catch ex As Exception
            l("fehler in getPointArray ", ex)
            Return Nothing
        End Try
    End Function

    Function polygoneMergen(pcoll1 As PointCollection, pcoll2 As PointCollection) As PointCollection
        Dim points = New PointCollection
        Dim lastpoint As New Point
        Dim parray1() As Point
        Dim lastindex As Integer
        Try
            l("polygoneMergen--------------------------")
            parray1 = getPointArray(pcoll1)
            If parray1 Is Nothing Then
                Return Nothing
            End If
            lastindex = parray1.Length - 1
            lastpoint.X = pcoll1(lastindex).X
            lastpoint.Y = pcoll1(lastindex).Y
            'parray2 = getPointArray(pcoll2)
            '   ReDim Preserve parray1(parray1.Length + parray2.Length)
            For Each pnt As Point In pcoll2
                pcoll1.Add(pnt)
            Next
            pcoll1.Add(lastpoint)
            l("polygoneMergen-------------------------- ende")
            Return pcoll1
        Catch ex As Exception
            l("fehler in polygoneMergen ", ex)
            Return Nothing
        End Try
    End Function
    Function prep(PointCollectionList As List(Of pointCollectionPlus), ByRef ucount As Integer) As List(Of pointCollectionPlus)
        Dim bereinigt As New List(Of pointCollectionPlus)
        Try
            bereinigt = directionErgaenzen(PointCollectionList, ucount)
            If bereinigt Is Nothing Then
                Return Nothing
            End If
            bereinigt = Mergen(bereinigt)
            bereinigt = killPolygon(bereinigt)
            Return bereinigt
        Catch ex As Exception
            l("fehler in prep ", ex)
            Return Nothing
        End Try
    End Function

    Function killPolygon(bereinigt As List(Of pointCollectionPlus)) As List(Of pointCollectionPlus)
        Dim neu As New List(Of pointCollectionPlus)
        Try
            l("killPolygon")
            If bereinigt Is Nothing Then
                l("warnung killPolygon Bereinigt is nothing abbruch")
                Return Nothing
            End If
            For Each mPolygon In bereinigt
                If mPolygon.direction <> "k" Then
                    neu.Add(mPolygon)
                End If
            Next
            Return neu
        Catch ex As Exception
            l("fehler in killPolygon ", ex)
            Return Nothing
        End Try
    End Function

    Function Mergen(aliste As List(Of pointCollectionPlus)) As List(Of pointCollectionPlus)
        Try
            l("Mergen----------------------------")
            If aliste Is Nothing Then
                l("warnung Mergen aliste   is nothing abbruch")
                Return Nothing
            End If
            If aliste.Count < 1 Then
                l("warnung Mergen aliste   aliste.Count < 1 abbruch")
                Return Nothing
            End If
            For i = 0 To aliste.Count - 1
                ' wenn p1 =g und p2=u dann mergen 
                If aliste.Item(i).direction = "g" Then
                    If i + 1 > aliste.Count - 1 Then Continue For
                    If aliste.Item(i + 1).direction = "u" Then
                        If aliste.Item(i).newImagemap Then
                        Else
                            aliste.Item(i).pcoll = polygoneMergen(aliste.Item(i).pcoll, aliste.Item(i + 1).pcoll)
                            'polygon i+1 löschen oder markieren"
                            aliste.Item(i + 1).direction = "k" 'kill
                        End If

                    Else
                        'keine aktion
                    End If
                Else
                    'keine aktion
                End If
            Next
            l("  Mergen ende------------------")
            Return aliste
        Catch ex As Exception
            l("fehler in Mergen ", ex)
            Return Nothing
        End Try
    End Function

    Function directionErgaenzen(PointCollectionList As List(Of pointCollectionPlus), ByRef ucount As Integer) As List(Of pointCollectionPlus)
        Dim tpol As New pointCollectionPlus
        Dim neu As New List(Of pointCollectionPlus)
        Try
            l(" directionErgaenzen ----------------------------------")
            If PointCollectionList Is Nothing Then
                '  l("warnung directionErgaenzen PointCollectionList   is nothing abbruch")
                Return Nothing
            End If

            For Each mPolygon In PointCollectionList
                tpol = New pointCollectionPlus
                Dim parray() As Point = getPointArray(mPolygon.pcoll)
                If parray Is Nothing Then
                    Return Nothing
                End If
                tpol.pcoll = mPolygon.pcoll
                If mPolygon.typ = "polygon" Or mPolygon.typ = "circle" Then
                    If SignedPolygonArea(parray) > 0 Then
                        tpol.direction = "u"
                        tpol.href = mPolygon.href
                        tpol.title = mPolygon.title
                        ucount += 1
                    Else
                        tpol.direction = "g"
                        tpol.href = mPolygon.href
                        tpol.title = mPolygon.title
                    End If
                    neu.Add(tpol)
                End If
                If mPolygon.typ = "line" Then
                    tpol.direction = "g"
                    tpol.href = mPolygon.href
                    tpol.title = mPolygon.title
                    neu.Add(tpol)
                End If
            Next
            l(" directionErgaenzen ------ ende ----------------------------")
            Return neu
        Catch ex As Exception
            l("fehler in directionErgaenzen ", ex)
            Return Nothing
        End Try
    End Function

    Friend Function gkstringsAusPointColl_generieren(bereinigt As List(Of pointCollectionPlus)) As List(Of String)
        Dim tgks As String = ""
        Dim lgks As New List(Of String)
        Try
            l(" gkstringsAusPointColl_generieren ----------------------------------")
            If bereinigt Is Nothing Then
                l("warnung gkstringsAusPointColl_generieren bereinigt   is nothing abbruch")
                Return Nothing
            End If


            For Each mPolygon In bereinigt
                tgks = pointColl2String(mPolygon.pcoll, ";")
                lgks.Add(tgks)
            Next
            l(" gkstringsAusPointColl_generieren ----- ende -----------------------------")
            Return lgks
        Catch ex As Exception
            l("fehler in gkstringsAusPointColl_generieren ", ex)
            Return Nothing
        End Try
    End Function

    Private Function pointColl2String(pcoll As PointCollection, trenner As String) As String
        'GKstringList 483069.128;5539515.615;483073.845;5539526.352;482992.036;5539553.982;482988.477;5539543.156;483069.128;5539515.615
        'GKstringList 486005,601;5545785,497;486051,016;5545808,79;486056,814;5545811,749;486042,067;5545850,171;485973,674;5545830,973;486005,601;5545785,497;
        Dim a() As Point
        Dim tgks As New Text.StringBuilder
        Dim ngks As String
        Try
            a = getPointArray(pcoll)
            If a Is Nothing Then
                Return ""
            End If
            For i = 0 To a.Count - 1
                tgks.Append(a(i).X & trenner & a(i).Y & trenner)
            Next
            ngks = tgks.ToString
            ngks = ngks.Substring(0, tgks.Length - 1)
            Return ngks
        Catch ex As Exception
            l("fehler in pointColl2String ", ex)
            Return Nothing
        End Try
    End Function

    Function isWKT(serial As String) As Boolean
        Try
            l("isWKT---------------------- anfang")
            l("serial " & serial)
            If serial.IsNothingOrEmpty Then
                l("isWKT serial ist leer")
                Return False
            End If
            serial = serial.ToUpper.Trim
            If serial.StartsWith("POLY") Or serial.StartsWith("MULTI") Or serial.StartsWith("LINE") Or serial.StartsWith("POINT") Then
                Return True
            End If
            Return False
            l("isWKT---------------------- ende")
        Catch ex As Exception
            l("Fehler in isWKT: " & ex.ToString())
            Return False
        End Try
    End Function
    Function serialGKStringnachWKT(serialshape As String, geomType As String) As String
        '5;0;488899,219402985|5551751,148|489972,908955224|5551940,62262687|490036,067164179|5551498,51516418|489025,535820896|5551372,19874627|488899,219402985|5551751,148
        ' oder mit mehr müll am anfang:
        '5;0;14;21;38;473757.386000|5536316.027000|473676.211000|5536562.134000|473643.425000|5536662.774000|473965.600000|5536774.008000|474000.303000|5536662.781000|474009.019000|5536642.889000|474028.241000|5536581.602000|474012.117000|5536576.434000|473977.292000|5536627.285000|473953.651000|5536530.803000|473945.116000|5536486.289000|473952.037000|5536413.997000|473903.341000|5536364.095000|473757.386000|5536316.027000|473823.794000|5536491.299000|473819.895000|5536499.436000|473788.598000|5536509.243000|473782.830000|5536491.979000|473813.898000|5536481.633000|473821.645000|5536485.482000|473823.794000|5536491.299000|473926.183000|5536630.784000|473895.245000|5536634.663000|473893.246000|5536643.070000|473888.198000|5536653.006000|473876.123000|5536655.006000|473865.997000|5536652.897000|473861.888000|5536649.898000|473857.280000|5536642.090000|473857.020000|5536632.584000|473858.430000|5536627.196000|473846.734000|5536639.142000|473834.169000|5536626.187000|473845.994000|5536614.381000|473840.457000|5536608.664000|473853.881000|5536575.687000|473892.286000|5536593.019000|473926.183000|5536630.784000|474015.226000|5536656.473000|474013.047000|5536655.904000|474011.099000|5536656.713000|474004.162000|5536664.290000|473969.848000|5536775.477000|473986.922000|5536781.365000|474015.226000|5536656.473000|
        'serialshape = "5;0;14;21;38;473757.386000|5536316.027000|473676.211000|5536562.134000|473643.425000|5536662.774000|473965.600000|5536774.008000|474000.303000|5536662.781000|474009.019000|5536642.889000|474028.241000|5536581.602000|474012.117000|5536576.434000|473977.292000|5536627.285000|473953.651000|5536530.803000|473945.116000|5536486.289000|473952.037000|5536413.997000|473903.341000|5536364.095000|473757.386000|5536316.027000|473823.794000|5536491.299000|473819.895000|5536499.436000|473788.598000|5536509.243000|473782.830000|5536491.979000|473813.898000|5536481.633000|473821.645000|5536485.482000|473823.794000|5536491.299000|473926.183000|5536630.784000|473895.245000|5536634.663000|473893.246000|5536643.070000|473888.198000|5536653.006000|473876.123000|5536655.006000|473865.997000|5536652.897000|473861.888000|5536649.898000|473857.280000|5536642.090000|473857.020000|5536632.584000|473858.430000|5536627.196000|473846.734000|5536639.142000|473834.169000|5536626.187000|473845.994000|5536614.381000|473840.457000|5536608.664000|473853.881000|5536575.687000|473892.286000|5536593.019000|473926.183000|5536630.784000|474015.226000|5536656.473000|474013.047000|5536655.904000|474011.099000|5536656.713000|474004.162000|5536664.290000|473969.848000|5536775.477000|473986.922000|5536781.365000|474015.226000|5536656.473000|"

        'POLYGON((474899.771 5537760.418,474897.104 5537770.057,47
        Dim header As String = "", nurkoordinaten As String = "", c() As String
        Dim abspann As String = ""
        Dim sw As New Text.StringBuilder
        Dim a As String

        Try
            l("serialGKStringnachWKT--------------------------------")
            l("geomType " & geomType)
            l("serialshape " & serialshape)
            l(serialshape)
            If serialshape.IsNothingOrEmpty Then
                Return "fehler in serialGKStringnachWKT  serialshape.IsNothingOrEmpty "
            End If
            a = serialshape
            If isWKT(a) Then
                l("isWKT ")
                Return a
            End If
            If geomType.IsNothingOrEmpty Then
                l("isWKTIsNothingOrEmpty ")
                shapeserial2wkt(header, nurkoordinaten, abspann, a)
            Else
                l("not isWKTIsNothingOrEmpty ")
                simpleserial2wkt(header, nurkoordinaten, abspann, geomType, serialshape)
            End If
            l(nurkoordinaten)
            nurkoordinaten = nurkoordinaten.Replace(",", ".")
            nurkoordinaten = nurkoordinaten.Replace("|", " ")
            If abspann <> "" Then
                nurkoordinaten = nurkoordinaten.Replace(abspann, "")
            End If

            c = nurkoordinaten.Split(" "c)
            l("vor schleife")
            For i = 0 To c.GetUpperBound(0)
                If i Mod 2 = 0 Then
                    sw.Append(c(i) & " ")
                Else
                    sw.Append(c(i) & ",")
                End If
            Next
            l("nach schleife")
            a = header & sw.ToString().Trim & abspann
            'a = a.Replace(";", " ")
            a = a.Replace(",)", ")")
            Return a
        Catch ex As Exception
            Return "fehler in serialGKStringnachWKT" & ex.ToString
        End Try
    End Function

    Private Sub simpleserial2wkt(ByRef header As String, ByRef nurkoordinaten As String,
                                 ByRef abspann As String, geomType As String, serialshape As String)
        Select Case geomType.ToLower
            Case "polygon", "flaeche"
                header = "POLYGON (("
                nurkoordinaten = BildeNurKoordinaten(serialshape)
                abspann = "))"
            Case "polyline", "line", "linestring", "strecke"
                header = "LINESTRING ("
                nurkoordinaten = BildeNurKoordinaten(serialshape)
                abspann = ")"
            Case Else
                nurkoordinaten = geomType.Replace(";", " ")
                header = ""
                abspann = ""
        End Select
    End Sub

    Private Sub shapeserial2wkt(ByRef header As String, ByRef nurkoordinaten As String, ByRef abspann As String, a As String)
        Select Case a.Substring(0, 4)
            Case "5;0;"
                header = "POLYGON (("
                nurkoordinaten = BildeNurKoordinaten(a)
                abspann = "))"
            Case "3;0;"
                header = "LINESTRING ("
                nurkoordinaten = BildeNurKoordinaten(a)
                abspann = ")"
            Case Else
                nurkoordinaten = a.Replace(";", " ")
                header = ""
                abspann = ""
        End Select
    End Sub

    Private Function BildeNurKoordinaten(gesamtstring As String) As String
        Dim b As String()
        Dim summe As New Text.StringBuilder
        b = gesamtstring.Split(";"c)
        For i = 0 To b.GetUpperBound(0)
            If Val(b(i)) < 1000 Then
                Continue For
            Else
                summe.Append(b(i) & " ")
            End If

        Next
        Return summe.ToString.Trim
        'nurkoordinaten = a.Replace("5;0;", "")
        'nurkoordinaten = nurkoordinaten.Replace(";", " ")
    End Function

    Friend Function zeichenrichtungInvertieren(points As PointCollection) As PointCollection
        Dim newPcoll As New PointCollection
        Try
            l("zeichenrichtungInvertieren---------------------- anfang")
            l("points.Count " & points.Count)
            For i = points.Count - 1 To 0 Step -1
                newPcoll.Add(points(i))
            Next
            l("zeichenrichtungInvertieren---------------------- ende")
            Return newPcoll

        Catch ex As Exception
            l("Fehler in zeichenrichtungInvertieren: ", ex)
            Return Nothing
        End Try
    End Function
End Module
Public Class pointCollectionPlus
    Property pcoll As New PointCollection
    Property direction As String = "u" ' oder "g" = gegen den uhrzeiger
    Property href As String = ""
    Property title As String = ""
    Property typ As String = "polygon" ''line, polygon oder circle
    Property newImagemap As Boolean = False
End Class