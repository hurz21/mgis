Module scaleScreen
    Public aktMasstab As Double
    Public Scale_bildschirmBreiteInMeter As Double = 0.509 '=1920pixel
    Public mapCanvasBreiteinMeter, mapCanvasHoeheinMeter As Double
    Friend Sub initMassstab(mapCanvas_Width As Double, mapCanvas_Height As Double, fensterbreiteInPixel As Double)        '  
        Dim ScreenMeterProPixel As Double

        ScreenMeterProPixel = Scale_bildschirmBreiteInMeter / CLng(fensterbreiteInPixel) ' 1920
        mapCanvasBreiteinMeter = ScreenMeterProPixel * mapCanvas_Width
        mapCanvasHoeheinMeter = ScreenMeterProPixel * mapCanvas_Height
        aktMasstab = (1 / mapCanvasBreiteinMeter) * kartengen.aktMap.aktrange.xdif
    End Sub

    Friend Sub calcNewRange(neuerMassstab As Double, useMouseWheelCenter As Boolean)
        Dim newRange As New clsRange
        Dim centerX, centerY As Double
        If useMouseWheelCenter Then
            centerX = aktGlobPoint.X
            centerY = aktGlobPoint.Y
        Else
            'kartengen.aktMap.aktrange.CalcCenter()
            centerX = kartengen.aktMap.aktrange.xcenter
            centerY = kartengen.aktMap.aktrange.ycenter
        End If

        Dim neuXdif, neuYdif As Double
        'Dim altXdif As Double
        'altXdif = aktrange.xdif
        neuXdif = neuerMassstab * mapCanvasBreiteinMeter
        neuYdif = neuerMassstab * mapCanvasHoeheinMeter
        'Debug.Print(CType(neuXdif, String) & " " & altXdif)
        kartengen.aktMap.aktrange.xl = centerX - (neuXdif / 2)
        kartengen.aktMap.aktrange.xh = centerX + (neuXdif / 2)
        kartengen.aktMap.aktrange.yl = centerY - (neuYdif / 2)
        kartengen.aktMap.aktrange.yh = centerY + (neuYdif / 2)
        'kartengen.aktMap.aktrange.CalcCenter()
    End Sub
End Module
