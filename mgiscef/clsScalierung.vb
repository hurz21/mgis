Public Class clsScalierung
    Private Const inchinCentimeters As Double = 2.5409999999999999
    Public dDPI As Integer = 72
    Public shared nachrichtentext As String=""
 

    Private Shared Function ScaleI(ByVal xdif As Double, ByVal w As Long, ByVal dDPI As Double) As Long
        'calculiert den maßstab
        Dim sd As Double
        Dim dp As Double
        Try
            sd = xdif
            sd = sd * 100
            dp = w / dDPI
            dp = dp * inchinCentimeters
            sd = sd / dp
            Return CLng(sd)
        Catch ex As Exception
            nachricht(String.Format("FEHLER ScaleI {0}", ex.Message))
            Return CLng(-1)
        End Try
    End Function

    Private Shared Sub SetScale(ByVal myScale As Long, ByVal cx As Double, ByVal cy As Double, _
    ByRef aktrange As clsRange, _
    ByVal w As Long, ByVal H As Long, ByVal dDPI As Double)
        'berechnet das neue Koordinaten fenster
        'IN: Maßstab -Scale
        'OUT: Range
        'kuprion
        Dim PicRangeX As Double
        Try

            nachricht("SetScale: " & myScale)

            ' Pixel
            PicRangeX = w
            ' DPI -> Inches
            PicRangeX = PicRangeX / dDPI
            ' cm
            PicRangeX = PicRangeX * inchinCentimeters
            ' mm = UOR !
            PicRangeX = PicRangeX / 100
            ' Scalierung  
            PicRangeX = PicRangeX * myScale
            ' mck 20071024
            'PicRangeX = PicRangeX / 4 * 3
            ' Relativ zum Center
            PicRangeX = PicRangeX / 2
            aktrange.xl = CInt(cx - PicRangeX)
            aktrange.xh = CInt(cx + PicRangeX)
            ' Höhe anhand Seitenverhältnis des Bildes
            Dim dxy As Double
            dxy = H / w
            PicRangeX = PicRangeX * dxy
            aktrange.yl = CInt(cy - PicRangeX)
            aktrange.yh = CInt(cy + PicRangeX)
        Catch ex As Exception
            l(ex.ToString)
        End Try
    End Sub

    Public Shared Sub Skalierung(ByVal dDPI As Double,
                                ByVal post_aktion$,
                                ByVal mapScale As Double,
                                ByVal akt_range As clsRange,
                                ByVal post_w%, ByVal post_h%,
                                ByVal post_scale As Long, ByVal post_range As clsRange, ByVal pixCanvas As clsCanvas)
        nachricht(String.Format("in Skalierung ###################################{0}", post_aktion))
        Try
            '1. Berechnen für Standardfall
            mapScale = ScaleI(akt_range.xdif, post_w, dDPI)
            nachricht(String.Format("mapscale / postscale:{0} / {1}", mapScale, post_scale))
            '2. Falls neue Scalierung vorgegeben ist, dann neue Rahmenkoordinaten berechnen
            If post_aktion = "SC" Then
                'mapScale = post_scale
                nachricht("Fall: SCALE vorgegeben")
                nachricht(String.Format("akt_range: {0}", akt_range))
                SetScale(post_scale, (akt_range.xl / 2) + (akt_range.xh / 2), (akt_range.yl / 2) + _
                 (akt_range.yh / 2), akt_range, post_w, post_h, dDPI)
                mapScale = ScaleI(akt_range.xdif, post_w, dDPI)
                nachricht(String.Format("nachher: {0}", akt_range))
            Else
                nachricht("Fall: ZB vorgegeben")
                nachricht("koordinaten nachberechnen entsprechend dem img-verhältnis  " & akt_range.quotient)

                mapScale = ScaleI(post_range.xdif, post_w, dDPI)

                Dim post_range_xcenter = post_range.xl + ((post_range.xh - post_range.xl) / 2)
                Dim post_range_ycenter = post_range.yl + ((post_range.yh - post_range.yl) / 2)

                nachricht(String.Format("################### post: {0}", post_range))   '

                nachricht("xext: " & post_range.xcenter - post_range.xl)
                nachricht("yext: " & post_range.ycenter - post_range.yl)

                '###############     adjust von markus
                Dim dPxy As Double = pixCanvas.w / pixCanvas.h
                Dim MapW As Double = post_range.xdif
                Dim MapH As Double = post_range.ydif
                Dim dMxy As Double
                If MapH = 0.0# Then Exit Sub
                dMxy = MapW / MapH
                Dim MapCx As Double = post_range.xcenter
                Dim MapCy As Double = post_range.ycenter

                If dPxy < dMxy Then
                    akt_range.yl = MapCy - (MapW / 2 / dPxy)
                    akt_range.yh = MapCy + (MapW / 2 / dPxy)
                Else
                    akt_range.xl = MapCx - (MapH / 2 * dPxy)
                    akt_range.xh = MapCx + (MapH / 2 * dPxy)
                End If
                '##########
            End If
            mapScale = ScaleI(akt_range.xdif, post_w, dDPI)
            nachricht(String.Format("################### nachher: {0}", akt_range))

            nachricht("skalierung normales ende #####################################################")
        Catch ex As Exception
            l(String.Format("FEHLER in Skalierung: {0}", akt_range))
            Dim FehlerHinweis$ = String.Format("mergen, Fehler: {0}{1} {0}{2} {0}{3} ", vbCrLf, ex.Message, ex.StackTrace, ex.Source)
            nachricht(String.Format("fehler in skalieerung {0}{1}", vbCrLf, FehlerHinweis$))
        End Try
    End Sub

Private Shared Sub nachricht(p1 As String)
        l(p1)
        nachrichtentext =p1
 End Sub 
 
End Class
