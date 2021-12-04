Module masstabtools

    Function calcNewScreenScale(massstabszahl As Integer,
                                ByRef rectWidthInPixel As Double,
                                ByRef rectHoheInPixel As Double,
                                rbFormatA4_IsChecked As Boolean,
                                quer_IsChecked As Boolean,
                                cv1_Width As Double,
                                cv1_Height As Double) As String
        'mrect neu zeichnen, entsprechend maßstab
        'xdif bestimmen
        Dim neuerPDFmasstab, altermassstab As Double
        Dim HoeheMeterInNatur, BreiteMeterInNatur As Double
        Dim aktcv As New clsCanvas
        Try
            altermassstab = PDF_druckMassStab
            neuerPDFmasstab = massstabszahl ' item.tagVal 
            If rbFormatA4_IsChecked Then
                aktcv = dina4InMM
            Else
                aktcv = dina3InMM
            End If
            HoeheMeterInNatur = (((aktcv.h) / 10) * neuerPDFmasstab) / 100
            BreiteMeterInNatur = (((aktcv.w) / 10) * neuerPDFmasstab) / 100
            'Dim rectWidthInPixel, rectHoheInPixel As Double
            If quer_IsChecked Then
                rectWidthInPixel = (BreiteMeterInNatur * cv1_Width) / kartengen.aktMap.aktrange.xdif
                rectHoheInPixel = (HoeheMeterInNatur * cv1_Height) / kartengen.aktMap.aktrange.ydif
            Else
                rectHoheInPixel = (BreiteMeterInNatur * cv1_Width) / kartengen.aktMap.aktrange.xdif
                rectWidthInPixel = (HoeheMeterInNatur * cv1_Height) / kartengen.aktMap.aktrange.ydif
            End If

            'tbMasstabDruck.Text = CType(item.tagVal, String)
            Return CType(massstabszahl, String)
        Catch ex As Exception
            l("fehler in calcNewScreenScale", ex)
            Return ""
        End Try
    End Function

    Function getDruckMasstaebe() As String()
        Dim a(27) As String
        Try
            l("getDruckMasstaebe---------------------- anfang")
            a(0) = "1 : 100"
            a(1) = "1 : 200"
            a(2) = "1 : 250"
            a(3) = "1 : 333"
            a(4) = "1 : 500"
            a(5) = "1 : 750"
            a(6) = "1 : 1000"
            a(7) = "1 : 1100"
            a(8) = "1 : 1200"
            a(9) = "1 : 1300"
            a(10) = "1 : 1400"
            a(11) = "1 : 1500"
            a(12) = "1 : 2000"
            a(13) = "1 : 3000"
            a(14) = "1 : 5000"
            a(15) = "1 : 7500"
            a(16) = "1 : 10000"
            a(17) = "1 : 15000"
            a(18) = "1 : 25000"
            a(19) = "1 : 50000"
            a(20) = "1 : 75000"
            a(21) = "1 : 80000"
            a(22) = "1 : 85000"
            a(23) = "1 : 95000"
            a(24) = "1 : 100000"
            a(25) = "1 : 120000"
            a(26) = "1 : 150000"
            a(27) = "1 : 200000"
            l("getDruckMasstaebe---------------------- ende")
            Return a
        Catch ex As Exception
            l("Fehler in getDruckMasstaebe: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Function getBildschirmMasstaebe() As String()
        Dim a(13) As String
        a(0) = "1 :   100"
        a(1) = "1 :   250"
        a(2) = "1 :   500"
        a(3) = "1 :   750"
        a(4) = "1 :   1000"
        a(5) = "1 :   1500"
        a(6) = "1 :   2000"
        a(7) = "1 :   5000"
        a(8) = "1 :   10000"
        a(9) = "1 :   15000"
        a(10) = "1 :   25000"
        a(11) = "1 :   50000"
        a(12) = "1 :   100000"
        a(13) = "1 :   250000"
        Return a
    End Function
    Friend Sub initMasstabCombo()
        Dim a(), b() As String
        Dim nm As New clsMasstab
        a = getBildschirmMasstaebe()
        For i = 0 To a.Count - 1
            b = a(i).Split(":"c)
            nm = New clsMasstab
            nm.intval = CInt(b(1).Trim)
            nm.displayVal = a(i)
            masstaebe.Add(nm)
        Next
    End Sub
    Friend Sub initDruckMasstabCombo(rbFormatA4_IsChecked As Boolean,
                                     quer_IsChecked As Boolean, cv1_Width As Double,
                                     cv1_Height As Double)
        Dim a(), b() As String
        Dim nm As New clsMasstab
        druckMasstaebe.Clear()
        a = getDruckMasstaebe()
        For i = 0 To a.Count - 1
            b = a(i).Split(":"c)
            nm = New clsMasstab
            nm.intval = CInt(b(1).Trim)
            nm.displayVal = a(i)
            'test = calcNewScreenScale((nm.intval), rectWidthInPixel, rectHoheInPixel,
            '                            CBool(rbFormatA4_IsChecked), CBool(quer_IsChecked),
            '                            cv1_Width, cv1_Height)
            'If masstabtools.rectIstZuGross(rectWidthInPixel, rectHoheInPixel, cv1_Width, cv1_Height, 25) Then
            '    druckMasstaebe.Add(nm)
            '    Debug.Print("")
            'Else
            '    druckMasstaebe.Add(nm)
            'End If
            druckMasstaebe.Add(nm)
        Next
    End Sub

    Friend Function rectIstZuGross(rectWidthInPixel As Double, rectHoheInPixel As Double,
                                   mapW As Double, mapH As Double, puffer As Integer) As Boolean
        Try
            l("rectIstZuGross---------------------- anfang")
            If (CInt(rectWidthInPixel) + puffer) > mapW Or
                   (CInt(rectHoheInPixel) + puffer) > mapH - puffer Then
                Return True
            Else
                Return False
            End If
            l("rectIstZuGross---------------------- ende")
        Catch ex As Exception
            l("Fehler in rectIstZuGross: " & ex.ToString())
            Return False
        End Try
    End Function
End Module
