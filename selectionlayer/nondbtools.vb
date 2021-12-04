Public Class nondbtools
    Shared Function isWKT(serial As String) As Boolean
        If serial.StartsWith("POLY") Or serial.StartsWith("MULTI") Or serial.StartsWith("LINE") Or serial.StartsWith("POINT") Then
            Return True
        End If
        Return False
    End Function
    Shared Function serialGKStringnachWKT(serialshape As String) As String
        '5;0;488899,219402985|5551751,148|489972,908955224|5551940,62262687|490036,067164179|5551498,51516418|489025,535820896|5551372,19874627|488899,219402985|5551751,148
        ' oder mit mehr müll am anfang:
        '5;0;14;21;38;473757.386000|5536316.027000|473676.211000|5536562.134000|473643.425000|5536662.774000|473965.600000|5536774.008000|474000.303000|5536662.781000|474009.019000|5536642.889000|474028.241000|5536581.602000|474012.117000|5536576.434000|473977.292000|5536627.285000|473953.651000|5536530.803000|473945.116000|5536486.289000|473952.037000|5536413.997000|473903.341000|5536364.095000|473757.386000|5536316.027000|473823.794000|5536491.299000|473819.895000|5536499.436000|473788.598000|5536509.243000|473782.830000|5536491.979000|473813.898000|5536481.633000|473821.645000|5536485.482000|473823.794000|5536491.299000|473926.183000|5536630.784000|473895.245000|5536634.663000|473893.246000|5536643.070000|473888.198000|5536653.006000|473876.123000|5536655.006000|473865.997000|5536652.897000|473861.888000|5536649.898000|473857.280000|5536642.090000|473857.020000|5536632.584000|473858.430000|5536627.196000|473846.734000|5536639.142000|473834.169000|5536626.187000|473845.994000|5536614.381000|473840.457000|5536608.664000|473853.881000|5536575.687000|473892.286000|5536593.019000|473926.183000|5536630.784000|474015.226000|5536656.473000|474013.047000|5536655.904000|474011.099000|5536656.713000|474004.162000|5536664.290000|473969.848000|5536775.477000|473986.922000|5536781.365000|474015.226000|5536656.473000|
        'serialshape = "5;0;14;21;38;473757.386000|5536316.027000|473676.211000|5536562.134000|473643.425000|5536662.774000|473965.600000|5536774.008000|474000.303000|5536662.781000|474009.019000|5536642.889000|474028.241000|5536581.602000|474012.117000|5536576.434000|473977.292000|5536627.285000|473953.651000|5536530.803000|473945.116000|5536486.289000|473952.037000|5536413.997000|473903.341000|5536364.095000|473757.386000|5536316.027000|473823.794000|5536491.299000|473819.895000|5536499.436000|473788.598000|5536509.243000|473782.830000|5536491.979000|473813.898000|5536481.633000|473821.645000|5536485.482000|473823.794000|5536491.299000|473926.183000|5536630.784000|473895.245000|5536634.663000|473893.246000|5536643.070000|473888.198000|5536653.006000|473876.123000|5536655.006000|473865.997000|5536652.897000|473861.888000|5536649.898000|473857.280000|5536642.090000|473857.020000|5536632.584000|473858.430000|5536627.196000|473846.734000|5536639.142000|473834.169000|5536626.187000|473845.994000|5536614.381000|473840.457000|5536608.664000|473853.881000|5536575.687000|473892.286000|5536593.019000|473926.183000|5536630.784000|474015.226000|5536656.473000|474013.047000|5536655.904000|474011.099000|5536656.713000|474004.162000|5536664.290000|473969.848000|5536775.477000|473986.922000|5536781.365000|474015.226000|5536656.473000|"

        'POLYGON((474899.771 5537760.418,474897.104 5537770.057,47
        Dim header, nurkoordinaten, c() As String
        Dim abspann As String
        Dim sw As New Text.StringBuilder
        Dim a As String = serialshape
        Try
            If isWKT(a) Then
                'a=a.Replace(";", " ")
                Return a
            End If

            Select Case a.Substring(0, 4)
                Case "5;0;"
                    header = "POLYGON(("
                    nurkoordinaten = BildeNurKoordinaten(a)
                    'nurkoordinaten = a.Replace("5;0;", "")
                    'nurkoordinaten = nurkoordinaten.Replace(";", " ")
                    abspann = "))"
                Case "3;0;"
                    header = "LINESTRING("
                    nurkoordinaten = BildeNurKoordinaten(a)
                    'nurkoordinaten = a.Replace("3;0;", "")
                    'nurkoordinaten = nurkoordinaten.Replace(";", " ")
                    abspann = ")"
                Case Else
                    nurkoordinaten = a.Replace(";", " ")
                    header = ""
                    abspann = ""
            End Select

            nurkoordinaten = nurkoordinaten.Replace(",", ".")
            nurkoordinaten = nurkoordinaten.Replace("|", " ")
            nurkoordinaten = nurkoordinaten.Replace(abspann, "")

            c = nurkoordinaten.Split(" "c)

            For i = 0 To c.GetUpperBound(0)
                If i Mod 2 = 0 Then
                    sw.Append(c(i) & " ")
                Else
                    sw.Append(c(i) & ",")
                End If
            Next
            a = header & sw.ToString().Trim & abspann
            'a = a.Replace(";", " ")
            a = a.Replace(",)", ")")
            Return a
        Catch ex As Exception

            Return "fehler in serialGKStringnachWKT" & ex.ToString
        End Try
    End Function

    Private Shared Function BildeNurKoordinaten(gesamtstring As String) As String
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
End Class
