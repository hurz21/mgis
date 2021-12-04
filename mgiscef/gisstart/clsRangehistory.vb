Public Class clsRangehistory
    Shared Sub rangeHistoryLeeren()
        Dim AlleKookieFiles As IO.FileInfo() = Nothing
        Dim reverseKookieFiles As IO.FileInfo() = Nothing
        Dim count As Integer
        Dim di As New IO.DirectoryInfo(myglobalz.mgisRangecookieDir)
        Try
            l(" rangeHistoryLeeren ---------------------- anfang")
            If di.Exists Then
                AlleKookieFiles = di.GetFiles("*.rng")
                count = AlleKookieFiles.GetUpperBound(0) + 1
                ReDim reverseKookieFiles(AlleKookieFiles.GetUpperBound(0))
                nachricht("Es wurden " & count & " HistoryItems gefunden.")
                nachricht("last" & myglobalz.mgisBackmodusLastCookie)
                clsRangehistory.RNGhistAufraeumen(AlleKookieFiles, reverseKookieFiles)
            End If
            l(" rangeHistoryLeeren ---------------------- ende")
        Catch ex As Exception
            l("Fehler in rangeHistoryLeeren: " & ex.ToString())
        End Try
    End Sub

    Shared Sub RNGhistAufraeumen(AlleKookieFiles() As IO.FileInfo, reverseKookieFiles() As IO.FileInfo)
        Dim j = 0
        Dim maxfiles = 20
        Dim ordFiles = From f In AlleKookieFiles Order By f.CreationTime
        Try
            l(" MOD RNGhistAufraeumen anfang")
            For i = ordFiles.Count - 1 To 0 Step -1
                If j > maxfiles Then
                    Try
                        My.Computer.FileSystem.DeleteFile(ordFiles(i).FullName)
                    Catch ex As Exception

                    End Try
                Else
                    reverseKookieFiles(j) = ordFiles(i)
                    j += 1
                End If
            Next
            l(" MOD RNGhistAufraeumen ende")
        Catch ex As Exception
            l("Fehler in RNGhistAufraeumen: " & ex.ToString())
        End Try
    End Sub
End Class
