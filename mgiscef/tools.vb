Imports System.Data

Module mtools

    Sub setzeGrundFuerEigentuemerabfrage(ByRef text As String)
        If GrundFuerEigentuemerabfrage.IsNothingOrEmpty Then
            If STARTUP_mgismodus = "paradigma" Then
                GrundFuerEigentuemerabfrage = "FD 67, " & GisUser.nick & ", " & aktvorgangsid
            Else
                If STARTUP_mgismodus = "probaug" Then
                    GrundFuerEigentuemerabfrage = "FD 63, " & GisUser.nick & ", " & aktvorgang.az
                Else
                    GrundFuerEigentuemerabfrage = GisUser.nick & ", " & aktvorgangsid & ", " & GisUser.ADgruppenname
                End If
            End If
        End If
        text = GrundFuerEigentuemerabfrage
    End Sub
    Sub OpenDokumentWith(myappname As String, full As String)
        Dim fi As IO.FileInfo
        Try
            l(" OpenDokumentWith ---------------------- anfang " & full)
            fi = New IO.FileInfo(full)
            full = full.Replace("/", "\")
            If fi.Exists Then
                Microsoft.VisualBasic.Shell(myappname & " " & full, AppWinStyle.NormalFocus)
            Else
                MsgBox("Datei konnte nicht gefunden werden!")
            End If
            fi = Nothing
            l(" OpenDokumentWith ---------------------- ende")
            'Return True
        Catch ex As Exception
            l("Fehler in OpenDokumentWith: " & full & ex.ToString())
            'Return False
        End Try
    End Sub
    Function OpenWithArguments(appname As String, full As String) As Boolean
        Dim fi As IO.FileInfo
        Try
            l(" OpenWithArguments ---------------------- anfang")
            fi = New IO.FileInfo(full.Replace("/", "\"))
            If fi.Exists Then
                Process.Start(appname, fi.FullName)
                fi = Nothing
                Return True
            Else
                MessageBox.Show("Datei konnte nicht gefunden werden!", fi.FullName)
                Return False
            End If
            l(" OpenWithArguments ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in OpenWithArguments: " & full & "<" & ex.ToString())
            Return False
        End Try
    End Function
    Function OpenDokument(full As String) As Boolean
        Dim fi As IO.FileInfo
        Try
            l(" OpenDokument ---------------------- anfang")
            If full.IsNothingOrEmpty Then Return False
            fi = New IO.FileInfo(full.Replace("/", "\"))
            If fi.Exists Then
                Process.Start(fi.FullName)
                fi = Nothing
                Return True
            Else
                MessageBox.Show("Datei konnte nicht gefunden werden!", fi.FullName)
                Return False
            End If
            l(" OpenDokument ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in OpenDokument: " & full & "<" & ex.ToString())
            Return False
        End Try
    End Function
    Function getproxystring() As String
        Dim wert$ = "-1"
        Try
            l(" getproxystring ---------------------- anfang")
            Dim a$ = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\" &
                    "Microsoft\Windows\CurrentVersion\Internet Settings",
                    "ProxyServer", wert).ToString
            If a = "-1" Then
                a = ""
            Else
                a = "http://" & a$
            End If
            nachricht("myGlobalz.ProxyString$: " & a)
            l(" getproxystring ---------------------- ende")
            Return a
        Catch ex As Exception
            l("Fehler in getproxystring: " & ex.ToString())
            Return ""
        End Try
    End Function
    'Friend Function zugrifferlaubt(userName As String) As Boolean
    '    'gebäudewirtschaft
    '    'umwelt
    '    'bauaufsicht
    '    'Gefahrenabwehr- und Gesundheitszentrum
    '    '
    '    clsActiveDir.getall("mueller_b") '  GisUser.nick)
    '    If clsActiveDir.fdkurz.Trim.ToLower = "umwelt" Or
    '            clsActiveDir.fdkurz.Trim.ToLower = "bauaufsicht" Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
    Friend Sub setUserFDkurz(gisuser As clsUser)
        l("setUserFDkurz userName " & gisuser.nick)
        gisuser.nick = gisuser.nick.Trim
        If iminternet Then
            gisuser.favogruppekurz = "umwelt"
            clsActiveDir.fdkurz = "umwelt"
            gisuser.ADgruppenname = "internet"
            Exit Sub
        End If
        If clsActiveDir.getall(gisuser.nick) Then
            If NSfstmysql.ADtools.istUserAlbBerechtigt(gisuser.nick) Then
                gisuser.istalbberechtigt = True
                gisuser.EmailAdress = clsActiveDir.emailadress
            End If
            gisuser.ADgruppenname = clsActiveDir.fdkurz
            l("setUserFDkurz clsActiveDir.fdkurz " & clsActiveDir.fdkurz)
            Select Case clsActiveDir.fdkurz.Trim.ToLower
                Case "gebäudewirtschaft"
                    gisuser.favogruppekurz = "gebw"
                Case "umwelt", "feger", "uwbb", "immi", "unb"
                    gisuser.favogruppekurz = "umwelt"

                Case "bauaufsicht", "bauaufsicht - allgemeine bauvorhaben", "bauaufsicht - besondere bauvorhaben"
                    gisuser.favogruppekurz = "ba"
                Case "gefahrenabwehr- und gesundheitszentrum"
                    gisuser.favogruppekurz = "kats"
                Case "jugend und familie"
                    gisuser.favogruppekurz = "soziales"
                Case Else
                    gisuser.favogruppekurz = "intranet"
            End Select
        Else
            MessageBox.Show("Der User >" & gisuser.nick & "< kommt im Active Directory  nicht vor!!!")
            l("fehler in setUserFDkurz Der User kommt im Active Directory nicht vor!!! " & gisuser.nick)
            gisuser.favogruppekurz = "intranet"
        End If

        l("user_fdkurz " & gisuser.favogruppekurz)
        l("setUserFDkurz  ende")
    End Sub

    Friend Function getFsvoKurz4Paradigmaabt(user_fdkurz As String) As String
        Dim retval As String = ""
        'Schornsteinfegerwesen
        'Untere Wasserbehörde
        'Immissionsschutz
        'Untere Naturschutzbehörde
        Select Case user_fdkurz.Trim.ToLower
            Case "schornsteinfegerwesen"
                retval = "feger"
            Case "untere wasserbehörde", "Untere Wasser- und Bodenschutzbehörde"
                retval = "uwbb"
            Case "immissionsschutz"
                retval = "immi"
            Case "untere naturschutzbehörde"
                retval = "unb"
            Case "graphische datenverarbeitung"
                retval = "gis"
            Case Else
                retval = "unb"
        End Select
        Return retval
    End Function
    Sub splitKoordinatenstring(neupunktString As String)
        Dim a() As String
        Try
            l("splitKoordinatenstring---------------------- anfang")
            a = neupunktString.Split(","c)
            aktGlobPoint.strX = a(0).Trim
            aktGlobPoint.strY = a(1).Trim
            l("splitKoordinatenstring---------------------- ende")
        Catch ex As Exception
            l("Fehler in splitKoordinatenstring: ", ex)
            aktGlobPoint.strX = "0"
            aktGlobPoint.strY = "0"
        End Try
    End Sub

    Function KreisUebersichtkoordinateKlickBerechnen(ByVal KoordinateKLickpt As Point?) As String
        Dim newpoint2 As New myPoint
        Dim kueRange As New clsRange
        Dim kuecanvas As New clsCanvas
        Try
            l("KreisUebersichtkoordinateKlickBerechnen---------------------- anfang")
            newpoint2.X = CDbl(KoordinateKLickpt.Value.X)
            newpoint2.Y = CDbl(KoordinateKLickpt.Value.Y)

            kueRange.xl = 470300
            kueRange.xh = 504250
            kueRange.yl = 5531100
            kueRange.yh = 5555200

            kuecanvas.w = 391
            kuecanvas.h = 280
            '470300, 5531100);
            'var referenzMaximum = New Punkt(504250, 5555200);

            aktGlobPoint = clsAufrufgenerator.WINPOINTVonCanvasNachGKumrechnen(newpoint2, kueRange, kuecanvas)
            aktGlobPoint.SetToInteger()
            newpoint2 = Nothing
            Return aktGlobPoint.toString
            l("KreisUebersichtkoordinateKlickBerechnen---------------------- ende")
        Catch ex As Exception
            l("Fehler in KreisUebersichtkoordinateKlickBerechnen: ", ex)
            Return Nothing
        End Try
    End Function
End Module
