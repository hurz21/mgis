Module polygonparser



    Public Function gkstringausserial_generieren(ShapeSerial As String) As String
        'esrishape oder postgis?
        'kann jetzt aber noch die Anzahl der punkte als 3 byte enthalten zb. "5;0;18;48..."
        If ShapeSerial IsNot Nothing Then
            Dim g As String = ShapeSerial
            If IsNumeric(ShapeSerial.Substring(0, 1)) Then
                'aus shapefile
                g = g.Replace("|", ";")
                Return g.Substring(4, g.Length - 4)
            Else
                'aus postgis
                g = g.Replace("MULTIPOLYGON", "")
                g = g.Replace("POLYGON", "")
                g = g.Replace("CURVECOMPOUNDCURVE", "")
                g = g.Replace("COMPOUNDCURVE", "")
                g = g.Replace("CURVE", "")
                
                g = g.Replace("(", "")
                g = g.Replace(")", "")
                g = g.Replace(",", ";")
                g = g.Replace(" ", ";")
                Return g
            End If
        Else
            Return ""
        End If
    End Function

    Public Function gkstringsausserial_generieren(ShapeSerial As String) As List(Of String)
        'esrishape oder postgis?
        'kann jetzt aber noch die Anzahl der punkte als 3 byte enthalten zb. "5;0;18;48..."
        Dim out As New List(Of String)
        Dim gkstring, teile() As String
        Dim ipos As Integer
        Dim teileZaehl As Integer = 0

        Dim polytrenner As String = ")),(("
        If ShapeSerial IsNot Nothing Then
            Dim g As String = ShapeSerial
            If IsNumeric(ShapeSerial.Substring(0, 1)) Then
                'aus shapefile
                g = g.Replace("|", ";")
                gkstring = g.Substring(4, g.Length - 4)
                out.Add(gkstring)
            Else
                'aus postgis
                If g.StartsWith("MULTIPOLYGON") Then
                    g = MULTIPOLGONanfangEntfernen(g)
                    g = MULTIPOLGONendeEntfernen(g)
start:
                    ipos = g.IndexOf(polytrenner)
                    If g.Length > 4 Then
                        If ipos < 1 Then ipos = g.Length
                        teileZaehl += 1
                        ReDim Preserve teile(teileZaehl)
                        teile(teileZaehl) = g.Substring(0, ipos)
                        If (ipos + 3) < g.Length Then
                            g = g.Substring(ipos + 3)
                        Else
                            g = ""
                        End If
                        teile(teileZaehl) = POLYGONklammernWeg(teile(teileZaehl))
                        out.Add(teile(teileZaehl))
                        GoTo start
                    Else
                        Return out
                    End If

                Else
                    g = POLYGONklammernWeg(g)
                    gkstring = g
                    out.Add(g)
                    Return out
                End If

            End If
        End If
        Return out
    End Function

    Private Function MULTIPOLGONanfangEntfernen(g As String) As String
        g = g.Replace("MULTIPOLYGON(", "")
        Return g
    End Function

    Private Function MULTIPOLGONendeEntfernen(g As String) As String
        g = g.Substring(0, Len(g) - 1)
        Return g
    End Function

    Private Function POLYGONklammernWeg(g As String) As String
        g = g.Replace("MULTIPOLYGON", "")
        g = g.Replace("POLYGON", "")
        g = g.Replace("LINESTRING", "")
        g = g.Replace("CURVECOMPOUNDCURVE", "")
        g = g.Replace("CURVECOMPOUNDCURVE", "")
        g = g.Replace("CIRCULARSTRING", "")
        g = g.Replace("COMPOUNDCURVE", "")
        g = g.Replace("CURVE", "")
        g = g.Replace("POINT", "")
        g = g.Replace("(", "")
        g = g.Replace(")", "")
        g = g.Replace(",", ";")
        g = g.Replace(" ", ";")
        Return g
    End Function



End Module
