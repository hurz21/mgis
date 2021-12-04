Namespace CLstart
    Namespace HistoryKookie
        Public Class HistoryItem
            Public Shared Property verlaufsCookieDir As String ' = _cookiedir & "verlaufscookies"
            Public Property ID As Integer
            Public Property AZ As String
            Public Property Titel As String
            Public Property Datum As Date
            Public Property Dateiname As String
            Property _cookiedir As String
            Public Sub New(cookiedir As String)
                _cookiedir = cookiedir
                If _cookiedir.Contains("verlaufscookies") Then
                    _cookiedir = cookiedir
                Else
                    verlaufsCookieDir = _cookiedir & "verlaufscookies"
                End If
            End Sub
        End Class

        Public Class schreibeVerlaufsCookie

            Property _cookiedir As String

            Public Shared Sub nachricht(text As String)
                My.Log.WriteEntry(text)
            End Sub
            Public Shared Sub nachricht(text As String, ex As Exception)
                My.Log.WriteEntry(text & Environment.NewLine & ex.ToString)
            End Sub

            Public Shared Sub exe(ByVal vorgangsid As String,
                              ByVal beschreibung As String,
                              ByVal az2 As String,
                              Probaugaz As String,
                              gemkrz As String)
                nachricht("schreibeVerlaufsCookie -------------------------------")
                If vorgangsid Is Nothing OrElse vorgangsid.Trim = String.Empty Then
                    nachricht("Fehler in schreibeVerlaufsCookie emdea: vorgangsid is null ")
                    Exit Sub
                End If
                Try

                    If Not istVorgangsidOK(vorgangsid) Then Exit Sub
                    nachricht("vorgangsid: " & vorgangsid)
                    nachricht("HistoryItem.verlaufsCookieDir: " & HistoryItem.verlaufsCookieDir)
                    Dim datei As String = HistoryItem.verlaufsCookieDir & "\" & vorgangsid & ".txt"
                    If Not IO.Directory.Exists(HistoryItem.verlaufsCookieDir) Then IO.Directory.CreateDirectory(HistoryItem.verlaufsCookieDir)
                    nachricht("datei: " & datei)
                    nachricht("beschreibung: " & beschreibung)
                    nachricht("az2: " & az2)
                    nachricht("Probaugaz: " & Probaugaz)
                    nachricht("gemkrz: " & gemkrz)

                    Dim Kookie As New IO.FileInfo(datei)
                    If Kookie.Exists Then
                        Kookie.Delete()
                    End If
                    schreibeVerlaufsCookieExtracted(beschreibung,
                                             datei,
                                             az2,
                                             Probaugaz,
                                             gemkrz)
                    Kookie = Nothing
                    nachricht("schreibeVerlaufsCookie emde: ")
                Catch ex As Exception
                    nachricht("Fehler in schreibeVerlaufsCookie emde: ", ex)
                End Try
            End Sub

            Private Shared Function istVorgangsidOK(ByVal vorgangsid As String) As Boolean
                Try
                    If vorgangsid Is Nothing OrElse vorgangsid = String.Empty Then Return False
                    ' If vorgangsid.IsNothingOrEmpty Then Return False
                    If Not IsNumeric(vorgangsid) Then Return False
                    Return True
                Catch ex As Exception
                    nachricht("Fehler in istVorgangsidOK emde: ", ex)
                    Return False
                End Try
            End Function

            Private Shared Sub schreibeVerlaufsCookieExtracted(ByVal beschreibung As String,
                                                           ByVal datei As String,
                                                           ByVal az2 As String,
                                                           externesAZ As String,
                                                           gemKRZ As String
                                                           )
                Try
                    Using sr As New IO.StreamWriter(datei)
                        sr.WriteLine(beschreibung)
                        sr.WriteLine(Now.ToString)
                        sr.WriteLine(az2.ToString)
                        sr.WriteLine("0") 'ereledigt
                        sr.WriteLine("0") 'WVfällig
                        sr.WriteLine(externesAZ) '
                        sr.WriteLine(gemKRZ)
                        sr.WriteLine("0")
                        sr.WriteLine("0")
                        sr.WriteLine("0")
                        sr.WriteLine("0")
                    End Using
                Catch ex As Exception
                    nachricht("Fehler in schreibeVerlaufsCookieEx emde: ", ex)
                End Try
            End Sub

            Public Sub New(cookiedir As String)
                _cookiedir = cookiedir
            End Sub
        End Class

        Public Class VerlaufsCookieLesen
            Shared Function exe(kookiedir As String) As List(Of HistoryItem)

                Dim AlleKookieFiles As IO.FileInfo() = Nothing
                '  Dim guteKookieFiles As IO.FileInfo() = Nothing
                Dim collHistory As New List(Of HistoryItem)
                Dim newlist As New List(Of HistoryItem)
                Dim count% = 0
                If HistoryItem.verlaufsCookieDir Is Nothing Then
                    HistoryItem.verlaufsCookieDir = kookiedir & "verlaufscookies"
                End If
                If HistoryItem.verlaufsCookieDir = String.Empty Then
                    HistoryItem.verlaufsCookieDir = kookiedir & "verlaufscookies"

                Else
                    If HistoryItem.verlaufsCookieDir.Contains("verlaufscookies") Then
                        HistoryItem.verlaufsCookieDir = kookiedir
                    Else
                        HistoryItem.verlaufsCookieDir = kookiedir & "verlaufscookies"
                    End If
                End If
                nachricht("VerlaufsCookieLesen")
                Try
                    Dim di As New IO.DirectoryInfo(HistoryItem.verlaufsCookieDir)
                    If di.Exists Then
                        AlleKookieFiles = di.GetFiles("*.txt")
                        count = AlleKookieFiles.GetUpperBound(0) + 1
                        nachricht("Es wurden " & count & " HistoryItems gefunden.")
                        nachricht("VerlaufsCookieLesen2")
                        If AlleKookieFiles.Count > 0 Then
                            SammelnDerObjekte(AlleKookieFiles, collHistory, HistoryItem.verlaufsCookieDir)
                        End If
                        Dim anewlist = From iiii In collHistory
                                       Order By iiii.Datum Descending
                                       Take 20
                        newlist = anewlist.ToList
                        ueberZaehligeLoeschen(AlleKookieFiles, newlist)
                        Return newlist
                    Else
                        nachricht("Verzeichnis fehlt: " & HistoryItem.verlaufsCookieDir)
                        Return Nothing
                    End If

                Catch ex As Exception
                    nachricht("fehler in VerlaufsCookieLesen ", ex)
                    Return Nothing
                End Try
            End Function

            Private Shared Sub SammelnDerObjekte(ByVal alleKookieFiles As IO.FileInfo(),
                                             ByVal collHistory As List(Of HistoryItem),
                                             verlaufscookiedir As String
                                         ) 'myGlobalz.ClientCookieDir & "verlaufscookies" 
                Try
                    For Each datei As IO.FileInfo In alleKookieFiles
                        Dim neu As HistoryItem
                        Using sr As New IO.StreamReader(datei.FullName)
                            neu = New HistoryItem(verlaufscookiedir)
                            Dim lDateiNameReplace As String = datei.Name.Replace(".txt", "")
                            If IsNumeric(lDateiNameReplace) Then
                                neu.ID = CInt(lDateiNameReplace)
                                neu.Titel = sr.ReadLine()
                                neu.Datum = CDate(sr.ReadLine())
                                neu.AZ = sr.ReadLine()
                                neu.Dateiname = datei.Name
                            Else
                                ' nachricht("Fehler in SammelnDerObjekte: dateiname ließ sich nicht in Integer wandeln: " & lDateiNameReplace)
                                Continue For
                            End If
                        End Using
                        collHistory.Add(neu)
                    Next
                Catch ex As Exception
                    nachricht("Fehler in SammelnDerObjekte: ", ex)
                End Try
            End Sub

            Private Shared Sub aufArrayLesen(ByRef aryFi As IO.FileInfo(), ByRef count%, ByVal di As IO.DirectoryInfo)
                aryFi = di.GetFiles("*.txt")
                count = aryFi.GetUpperBound(0) + 1
                nachricht("Es wurden " & count & " Vorlagen gefunden.")
            End Sub

            Private Shared Sub ueberZaehligeLoeschen(AlleKookieFiles As IO.FileInfo(), newlist As List(Of HistoryItem))
                Try
                    For Each datei In AlleKookieFiles
                        If istGuterKookie(datei, newlist) Then
                            Continue For
                        Else
                            'löschen
                            Debug.Print("lösche: " & datei.ToString)
                            loescheDatei(datei)
                        End If
                    Next
                Catch ex As Exception
                    nachricht("Fehler in ueberZaehligeLoeschen: ", ex)
                End Try
            End Sub

            Private Shared Function istGuterKookie(datei As IO.FileInfo, newlist As List(Of HistoryItem)) As Boolean
                Try
                    For Each guter As HistoryItem In newlist
                        If guter.Dateiname.ToLower.Trim = datei.Name.ToLower.Trim Then Return True
                    Next
                    Return False
                Catch ex As Exception
                    nachricht("Fehler in istGuterKookie: ", ex)
                    Return False
                End Try
            End Function

            Private Shared Sub loescheDatei(datei As IO.FileInfo)
                Try
                    datei.Delete()
                Catch ex As Exception
                    nachricht("Fehler in loescheDatei: ", ex)
                End Try
            End Sub
            Public Shared Sub nachricht(text As String)
                My.Log.WriteEntry(text)
            End Sub
            Public Shared Sub nachricht(text As String, ex As Exception)
                My.Log.WriteEntry(text & Environment.NewLine & ex.ToString)
            End Sub
        End Class

    End Namespace

End Namespace