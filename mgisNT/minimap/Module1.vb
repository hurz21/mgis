Module Module1
    Public Function bestimmeDezimalTrenner(ByVal gkstring As List(Of String)) As Char
        Try
            For Each sssstring As String In gkstring
                If sssstring.Contains(".") Then
                    Return "."c
                End If
                If sssstring.Contains(",") Then
                    Return ","c
                End If
            Next
            Return CChar(",") '???
        Catch ex As Exception
            l("fehler inbestimmeDezimalTrenner:", ex)
            Return CChar(",")
        End Try
    End Function
End Module
