Imports System.Data
Imports mgis

Public Class clsToolsAllg
    Shared Sub userlayerNeuErzeugen(username As String, vid As String) 'GisUser.username,aktvorgangsid
        l("userlayerNeuErzeugen--------------------------------")
        Dim rumpf As String = URLlayer2shpfile
        rumpf &= username
        rumpf &= "&vid=" & vid
        rumpf &= "&modus=einzeln"
        nachricht("url: " & rumpf)
        Dim hinweis As String = ""
        l("meinHttpJob  " & meineHttpNet.meinHttpJob("", rumpf, hinweis, myglobalz.enc, 10000))
    End Sub
    Shared Function koordinateKlickBerechnen(ByVal KoordinateKLickpt As Point?) As String
        Dim newpoint2 As New myPoint
        'Dim aktpoint As New myPoint
        newpoint2.X = CDbl(KoordinateKLickpt.Value.X)
        newpoint2.Y = CDbl(KoordinateKLickpt.Value.Y)
        aktGlobPoint = clsAufrufgenerator.WINPOINTVonCanvasNachGKumrechnen(newpoint2,
                                                                           kartengen.aktMap.aktrange,
                                                                           kartengen.aktMap.aktcanvas)
        aktGlobPoint.SetToInteger()
        Return aktGlobPoint.toString
        newpoint2 = Nothing
        aktGlobPoint = Nothing
    End Function
    Shared Function setPosition(kategorie As String, eintrag As String, aktval As Double) As Double
        'Me.Top = clsToolsAllg.setPosition("diverse", "dbabfrageformpositiontop", Me.Top)
        'Me.Left = clsToolsAllg.setPosition("diverse", "dbabfrageformpositionleft", Me.Left)
        Dim retval As Double
        Try
            l(" setPosition ---------------------- anfang")
            Dim topf As String = userIniProfile.WertLesen(kategorie, eintrag)
            If String.IsNullOrEmpty(topf) Then
                userIniProfile.WertSchreiben(kategorie, eintrag, CType(aktval, String))
                retval = aktval
            Else
                retval = CDbl(topf)
            End If
            l(" getIniDossier ---------------------- ende")
            Return retval
        Catch ex As Exception
            l("Fehler in setPosition: " & ex.ToString())
            Return aktval
        End Try
    End Function
    Shared Sub startbplankataster()
        Try
            l(" startbplankataster ---------------------- anfang")
            Dim handle As Process = Process.Start(myglobalz.serverUNC & "apps\bplankat\bplanupdate.bat")
            Threading.Thread.Sleep(500)
            Process.Start("c:\ptest\bplankat\bplanupdate.exe")
            l(" startbplankataster ---------------------- ende")
        Catch ex As Exception
            l("Fehler in startbplankataster: " & ex.ToString())
        End Try
    End Sub
    Shared Function genCSV4DT(trenner As String, datatab As DataTable, startspalte As Integer) As String
        Dim out As String
        Dim sb As New Text.StringBuilder
        Try
            l("genCSV4DT---------------------- anfang")
            For j = startspalte To datatab.Columns.Count - 1
                sb.Append(clsDBtools.fieldvalue(datatab.Columns(j).ColumnName).Trim & trenner)
            Next
            sb.Append(Environment.NewLine)
            For i = 0 To datatab.Rows.Count - 1
                For j = startspalte To datatab.Columns.Count - 1
                    sb.Append(clsDBtools.fieldvalue(datatab.Rows(i).Item(j)).Trim() & trenner)
                Next
                sb.Append(Environment.NewLine)
            Next
            out = sb.ToString
            sb = Nothing
            Return out
            l("genCSV4DT---------------------- ende")
        Catch ex As Exception
            l("Fehler in genCSV4DT: " & ex.ToString())
            Return "fehler bei der CSV-Erzeugung"
        End Try
    End Function

    Friend Shared Function initMgisHistory() As String
        Try
            l(" initMgisHistory ---------------------- anfang")
            Dim localAppDatMGISDir As String = System.Environment.GetEnvironmentVariable("APPDATA") & "\mgis"
            Dim ClientCookieDir = localAppDatMGISDir & "\rangecookies\"
            IO.Directory.CreateDirectory(ClientCookieDir)
            '  collHistory = CLstart.HistoryKookie.VerlaufsCookieLesen.exe(ClientCookieDir & "verlaufscookies")
            l(" initMgisHistory ---------------------- ende")
            Return ClientCookieDir
        Catch ex As Exception
            l("Fehler in initMgisHistory: " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Sub mgisRangeCookieSave(range As clsRange, mgisRangecookieDir As String)
        Dim filename As String = ""
        Try
            l(" mgisRangeCookieSave ---------------------- anfang")
            filename = CInt(range.xl) & "_" &
                        CInt(range.xh) & "_" & CInt(range.yl) & "_" & CInt(range.yh) & "_" &
                        clsString.date2string(Now, 5) & ".rng"
            filename = mgisRangecookieDir & filename
            l("filename" & filename)
            IO.File.Create(filename)
            l(" mgisRangeCookieSave ---------------------- ende")
        Catch ex As Exception
            l("Fehler in mgisRangeCookieSave: " & filename & ex.ToString())
        End Try
    End Sub
End Class
