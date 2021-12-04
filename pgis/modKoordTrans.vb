Imports System.Net
Imports System.IO
Imports System.Text

Module modKoordTrans
    Public PaareTrenner As Char = CChar("_")
    Public xyTrenner As Char = CChar(";")



    Function bildeQuellKoordinatenString(punktarrayInM As myPoint()) As String
        Dim QkoordString As String = ""
        Dim aktp = New myPoint()
        Try
            nachricht("bildeQuellKoordinatenString ----------------")
            If punktarrayInM Is Nothing Then Return ""
            For Each punkt In punktarrayInM
                QkoordString = QkoordString & punkt.X.ToString.Replace(",", ".") & xyTrenner & punkt.Y.ToString.Replace(",", ".") & PaareTrenner
            Next
            QkoordString = QkoordString.Substring(0, QkoordString.Length - 1)
            nachricht("bildeQuellKoordinatenString ----------------" & QkoordString)
            Return QkoordString
        Catch ex As Exception
            nachricht("fehler in bildeQuellKoordinatenString: " & ex.ToString)
            Return ""
        End Try
    End Function

    Function bildeaufruf(qkoordstring As String, qkoordcount As String, qkoordSys As String, zkoordSys As String) As String
        Dim a As String
        Try
            nachricht("bildeaufruf: " & qkoordstring)
            a = "http://w2gis02.kreis-of.local/cgi-bin/apps/paradigmaex/KoordinatenServer/KoordinatenServer.cgi?user=" &
                      Environment.UserName &
                      "&Q_CoordString=" & qkoordstring &
                      "&Q_CoordCount=" & qkoordcount &
                      "&Q_Coordsys=" & qkoordSys &
                      "&Z_Coordsys=" & zkoordSys
            nachricht("bildeaufruf: " & a)
            Return a
        Catch ex As Exception
            nachricht("fehler in bildeaufruf:" & ex.ToString)
            Return ""
        End Try
    End Function

    Sub getLongLatFromResultSingle(result As String, ByRef longitude As String, ByRef latitude As String)
        Dim a As String()
        Try
            result = result.Trim
            a = result.Split(xyTrenner)
            longitude = a(0)
            latitude = a(1)
        Catch ex As Exception

        End Try
    End Sub

    Function getLongLatFromResultBulk(result As String) As myPoint()
        Dim paare, a As String()
        Dim punkte() As myPoint
        Try
            l("getLongLatFromResultBulk-----------------------")
            l(PaareTrenner)
            l(result)
            result = result.Trim
            paare = result.Split(PaareTrenner)
            ReDim punkte(paare.Count - 1)
            For i = 0 To paare.GetUpperBound(0)
                a = paare(i).Split(xyTrenner)
                punkte(i) = New myPoint
                punkte(i).X = CDbl(a(0))
                punkte(i).Y = CDbl(a(1))
            Next
            Return punkte
        Catch ex As Exception
            l("fehler in getLongLatFromResultBulk: (" & result & ") " & ex.ToString)
            Return Nothing
        End Try
    End Function
End Module
