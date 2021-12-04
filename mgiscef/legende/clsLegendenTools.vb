Imports mgis

Public Class clsLegendenTools
    Friend Shared Function getLegendeFromHTTP(aid As Integer, hinweis As String) As List(Of clsLegendenItem)
        Dim result As String
        Dim fsts As New List(Of clsLegendenItem)
        Try
            l(" MOD getLegendeFromHTTP---------------------- anfang")
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick &
                    "&modus=getlegende&aid=" & aid
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            result = result.Trim
            If result.IsNothingOrEmpty Then
                Return Nothing
            End If
            nachricht(hinweis)
            fsts = ajaxlegendeliste(result)
            If fsts Is Nothing Then
                l("fehler in getLegendeFromHTTP: aid " & aid)
            End If
            Return fsts
        Catch ex As Exception
            l("Fehler beim getLegendeFromHTTP ", ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function ajaxlegendeliste(result As String) As List(Of clsLegendenItem)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim lok As New List(Of clsLegendenItem)
        Dim fst As New clsLegendenItem
        Dim oldname As String = ""
        Try
            l(" ajaxlegendeliste html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in ajaxlegendeliste: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            fst = New clsLegendenItem
            For i = 0 To zeilen - 1
                fst = New clsLegendenItem
                b = a(i).Split("#"c)
                fst.aid = CInt(b(1).Trim)
                fst.nr = CInt(b(4).Trim)
                fst.titel = b(5).Trim
                If fst.aid = 0 OrElse fst.nr = 0 Then
                    Continue For
                End If
                lok.Add(fst)
            Next
            Return lok
            l(" ajaxlegendeliste ---------------------- ende")
        Catch ex As Exception
            l("Fehler in ajaxlegendeliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function
End Class
