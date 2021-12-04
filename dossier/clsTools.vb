
Public Class clsTools

    Shared Function erzeugeUndOeffneText2Pdf(text As String) As String
        Dim ausgabedatei As String = calcEigentuemerAusgabeFile()
        wrapItextSharp.createText2PDF(text, ausgabedatei)
        Return ausgabedatei
    End Function
    Shared Function erzeugeUndOeffneEigentuemerPDF(text As String, aktfst As ParaFlurstueck,
                                                   ByRef strError As String) As String
        Dim lokalitaet, flaeche As String
        lokalitaet = getlokalitaetstring(aktfst)
        lokalitaet = lokalitaet & m.NASlage.strAusgabe
        'flaeche = clsFSTtools.getFlaecheZuFlurstueck(aktfst, strError, aktfst.normflst.weistauf, aktfst.normflst.zeigtauf)
        flaeche = CType(aktfst.FlaecheQm, String)
        lokalitaet = lokalitaet & " " & flaeche
        Dim ausgabedatei As String = calcEigentuemerAusgabeFile()
        wrapItextSharp.createSchnellEigentuemer(text, ausgabedatei, m.albverbotsString, lokalitaet)
        Return ausgabedatei
    End Function
    Friend Shared Function calcEigentuemerAusgabeFile() As String
        Dim EigentuemerPDF As String
        Dim ausgabeDIR As String ' = My.Computer.FileSystem.SpecialDirectories.Temp '& "" & aid
        Try
            l("calcEigentuemerAusgabeFile---------------------- anfang")
            ausgabeDIR = System.IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Eigentuemer")
            l("ausgabeDIR anlegen " & ausgabeDIR)
            IO.Directory.CreateDirectory(ausgabeDIR)
            l("calcEigentuemerAusgabeFile---------------------- ende")
            EigentuemerPDF = ausgabeDIR & "\eigentuemer" & Format(Now, "ddMMyyyy_hhmmss") & ".pdf"
            l("EigentuemerPDF " & EigentuemerPDF)
            Return EigentuemerPDF
        Catch ex As Exception
            l("Fehler in calcEigentuemerAusgabeFile: " & ex.ToString())
            Return ""
        End Try
    End Function
    Shared Function getlokalitaetstring(aktFST As ParaFlurstueck) As String
        Dim summe As String = ""
        Dim trenner As String = ", "
        aktFST.normflst.fstueckKombi = aktFST.normflst.buildFstueckkombi()
        summe = summe & aktFST.normflst.gemarkungstext & trenner
        summe = summe & "Flur: " & aktFST.normflst.flur & trenner
        summe = summe & "Flurstueck: " & aktFST.normflst.fstueckKombi & trenner
        Return summe
    End Function
    Shared Sub klassischeDBabfrage(aktObjID As Integer, featureclass As String, htmlTemplate As String)
        Dim Param As String
        Try
            l("---------------------- anfang")
            Param = "/cgi-bin/apps/gis/getrecord/getrecord4template.cgi"
            Param = Param & "?lookup=" + "true"
            Param = Param & "&aktive_ebene=" + featureclass
            Param = Param & "&object_id=" & aktObjID
            Param = Param & "&templatefile=" + htmlTemplate
            Param = Param & "&activelayer=" + featureclass
            Param = Param & "&apppfad=/profile/register/"
            Param = m.serverWeb & Param
            Process.Start(Param)
            l(" - ---------------------ende")
        Catch ex As Exception
            l("Fehler In : " & ex.ToString())
        End Try
    End Sub
    Shared Sub paradigmavorgangaufrufen(paradigmaVID As String)
        Dim modul, param As String
        Try
            l("paradigmavorgangaufrufen---------------------- anfang")
            modul = "c:\ptest\paradigmadetail\paradigmadetail.exe"
            param = " /vid=" & paradigmaVID '
            'MsgBox(param)
            Process.Start(modul, param)
            l("paradigmavorgangaufrufen---------------------- ende")
        Catch ex As Exception
            l("Fehler in paradigmavorgangaufrufen: " & ex.ToString())
        End Try
    End Sub
    Shared Function getIniDossier(eintrag As String) As Boolean
        Try
            l(" getIniDossier ---------------------- anfang")
            Dim val As String = m.userIniProfile.WertLesen("gisanalyse", eintrag)
            If String.IsNullOrEmpty(val) Then
                m.userIniProfile.WertSchreiben("gisanalyse", eintrag, "1")
                Return True
            Else
                Return CBool(val)
            End If
            l(" getIniDossier ---------------------- ende")
        Catch ex As Exception
            l("Fehler in getIniDossier: " & ex.ToString())
            Return True
        End Try
    End Function

    Friend Shared Function makeTable4WMS(result As String, icols As Integer, ByRef spaltenwert As String) As String
        Dim items As String()
        Dim newstring As New Text.StringBuilder
        Dim spaltenkoepfe As String()
        ReDim spaltenkoepfe(icols)
        Try
            l(" makeTable4WMS ---------------------- anfang")
            items = result.Split(";"c)
            For i = 0 To icols
                spaltenkoepfe(i) = items(i).ToLower
                spaltenkoepfe(i) = clsString.Capitalize(spaltenkoepfe(i))
            Next
            For j = 0 To icols - 1
                If spaltenkoepfe(j).ToLower = spaltenwert.ToLower Then
                    spaltenwert = items(j + icols)
                End If
                newstring.Append(spaltenkoepfe(j) & ": " & items(j + icols) & Environment.NewLine)
            Next
            l(" makeTable4WMS ---------------------- ende")
            Return newstring.ToString
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
        End Try
    End Function

    Shared Function getDoubleFromPointString(sTARTUP_punktKoordinatenString As String) As myPoint
        Dim a() As String
        Dim newpoint As New myPoint
        Try
            l(" getDoubleFromString ---------------------- anfang")
            a = sTARTUP_punktKoordinatenString.Split(","c)
            newpoint.X = CDbl(a(0).Replace(",", ".").Trim)
            newpoint.Y = CDbl(a(1).Replace(",", ".").Trim)
            l(" getDoubleFromString ---------------------- ende")
            Return newpoint
        Catch ex As Exception
            l("Fehler in getDoubleFromString: >" & sTARTUP_punktKoordinatenString & "< " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Shared Function setPosition(kategorie As String, eintrag As String, aktval As Double) As Double
        'Me.Top = clsToolsAllg.setPosition("diverse", "dbabfrageformpositiontop", Me.Top)
        'Me.Left = clsToolsAllg.setPosition("diverse", "dbabfrageformpositionleft", Me.Left)
        Dim retval As Double
        Try
            l(" setPosition ---------------------- anfang")
            Dim topf As String = m.userIniProfile.WertLesen(kategorie, eintrag)
            If String.IsNullOrEmpty(topf) Then
                m.userIniProfile.WertSchreiben(kategorie, eintrag, CType(aktval, String))
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

    Friend Shared Function getErsterVorgang(trim As String) As String
        Dim a() As String
        Try
            l(" getErsterVorgang ---------------------- anfang")
            a = trim.Split(","c)
            Return a(0)
            l(" getErsterVorgang ---------------------- ende")
        Catch ex As Exception
            l("Fehler in getErsterVorgang: " & ex.ToString())
            Return ""
        End Try
    End Function
End Class
