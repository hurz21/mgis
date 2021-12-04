Imports System.Data
Imports mgis

Public Class clsADRtools
    Friend Shared Function getBox4Adresses(ByRef hinweis As String, ByRef result As String, SQL As String, innersql As String) As String
        l("getBox4Adresses---------------------- anfang")
        Dim box As String
        Try
            If iminternet Or CGIstattDBzugriff Then
                result = clsToolsAllg.getSQL4Http(SQL, "postgis20", hinweis, "getsql") : l(hinweis)
                result = result.Trim
                If result.IsNothingOrEmpty Then
                    box = ""
                End If
                box = result.Replace("$", "").Replace(vbCrLf, "")
                'box = clsADRtools.holeBoxKoordinatenFuerStrasseHTTP(innersql, os_tabelledef.Schema & "." & os_tabelledef.tabelle)
            Else
                box = pgisTools.holeBoxKoordinatenFuerStrasseDB(SQL, os_tabelledef.Schema & "." & os_tabelledef.tabelle)
            End If
            Return box
        Catch ex As Exception
            l("fehler in getBox4Adresses: ", ex)
            Return "-1"
        End Try
    End Function
    Friend Shared Function getAdressTab4GID(gid As Integer) As String
        Dim tab As String = ""
        Try
            If CInt(gid) < 300000 Then
                tab = "flurkarte.halofs"
            End If
            If CInt(gid) > 400000 And CInt(os_tabelledef.gid) < 407000 Then
                tab = "flurkarte.schneischen"
            End If
            If CInt(gid) > 407000 Then
                tab = "flurkarte.strassobj"
            End If
            Return tab
        Catch ex As Exception
            l("fehler in getAdressTab4GID: ", ex)
            Return "-1"
        End Try
    End Function
    Shared Sub inithausnrComboDT()
        adrREC.mydb.SQL =
         "SELECT distinct  hausnrkombi,gml_id,rechts,hoch,  abs(hausnr) FROM flurkarte.halofs " &
         " where gemeindenr = '" & aktadr.Gisadresse.gemeindebigNRstring & "'" &
         " and strcode ='" & aktadr.Gisadresse.strasseCode & "'" &
         " order by  abs(hausnr)"
        Dim hinweis As String = adrREC.getDataDT()
    End Sub
    Shared Function erzeugeUndOeffneEigentuemerPDF(tbWeitergabeVerbot As String, summentex As String) As String
        Dim lokalitaet, flaeche As String

        lokalitaet = summentex
        flaeche = clsFSTtools.getFlaecheZuFlurstueck(aktFST)
        lokalitaet = lokalitaet & " " & flaeche
        'IO.Directory.CreateDirectory(ausgabeDIR)
        'Dim ausgabedatei As String = ausgabeDIR & "\eigentuemer" & Format(Now, "dd.MM.yyyy_hhmmss") & ".pdf"
        Dim ausgabedatei As String = tools.calcEigentuemerAusgabeFile
        wrapItextSharp.createSchnellEigentuemer(tbWeitergabeVerbot, ausgabedatei, albverbotsString, lokalitaet, aktadr.defineAbstract)
        Return ausgabedatei
    End Function
    Shared Function adresseNaCHpARADIGMA(freitext As String, aname As String) As Boolean ' 
        Dim umkreisID As Integer
        Try
            aktadr.setcoordsAbstract()
            aktadr.Freitext = freitext.Trim
            aktadr.Name = aname.Trim
            aktadr.Gisadresse.Quelle = "halo"
            aktadr.Gisadresse.gemeindeName = clsString.Capitalize(aktadr.Gisadresse.gemeindeName)
            aktadr.Typ = RaumbezugsTyp.Adresse
            aktadr.isMapEnabled = True
            aktadr.PLZ = "0"
            aktadr.FS = ""
            aktadr.Postfach = ""
            aktadr.Adresstyp = adressTyp.ungueltig
            Dim radius = 100
            'modEW.Paradigma_Adresse_Neu(radius)
            umkreisID = modParadigma.Paradigma_Adresse_Neu(radius)
            If umkreisID > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            l("fehler in adresseNaCHpARADIGMA ", ex)
            Return False
        End Try

    End Function
    Shared Function initHausnrliste(dt As DataTable) As List(Of clsFlurauswahl)
        Try
            l(" initHausnrliste ---------------------- anfang")
            Dim hausnrListe As New List(Of clsFlurauswahl)
            Dim hausnr As New clsFlurauswahl
            For i = 0 To dt.Rows.Count - 1
                hausnr = New clsFlurauswahl
                hausnr.nenner = (clsDBtools.fieldvalue(dt.Rows(i).Item(1))) & "#" &
                                clsDBtools.fieldvalue(dt.Rows(i).Item(2)) & "#" &
                                clsDBtools.fieldvalue(dt.Rows(i).Item(3))                     'weistauf,rechts hoch
                hausnr.displayText = clsDBtools.fieldvalue(dt.Rows(i).Item(0)) 'hausnrkombi
                hausnrListe.Add(hausnr)
            Next
            Return hausnrListe
            l(" initHausnrliste ---------------------- ende")
        Catch ex As Exception
            l("Fehler in initHausnrliste: ", ex)
            Return Nothing
        End Try

    End Function
    Shared Function ajaxStrassenliste(result As String) As List(Of clsFlurauswahl)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim lok As New List(Of clsFlurauswahl)
        Dim strasse As New clsFlurauswahl
        Dim oldname As String = ""
        Try
            l(" initStrassenliste html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in ajaxStrassenliste a: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            strasse = New clsFlurauswahl
            For i = 0 To zeilen - 1
                strasse = New clsFlurauswahl
                b = a(i).Split("#"c)
                strasse.displayText = b(0).Trim
                strasse.id = CInt(b(1))
                If strasse.displayText <> oldname Then
                    strasse.marker = CInt(b(2))
                    oldname = strasse.displayText
                    lok.Add(strasse)
                End If
            Next
            Return lok
            l(" ajaxStrassenliste ---------------------- ende")
        Catch ex As Exception
            l("Fehler in ajaxStrassenliste b: ", ex)
            Return Nothing
        End Try
    End Function
    Shared Function initStrassenliste(dt As DataTable) As List(Of clsFlurauswahl)
        Try
            l(" initStrassenliste ---------------------- anfang")
            Dim lok As New List(Of clsFlurauswahl)
            Dim strasse As New clsFlurauswahl
            Dim oldname As String = ""
            For i = 0 To dt.Rows.Count - 1
                strasse = New clsFlurauswahl
                strasse.id = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item(1)))
                strasse.displayText = clsDBtools.fieldvalue(dt.Rows(i).Item(0))
                If strasse.displayText <> oldname Then
                    strasse.marker = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item(2)))
                    oldname = strasse.displayText
                    lok.Add(strasse)
                End If
            Next
            Return lok
            l(" initStrassenliste ---------------------- ende")
        Catch ex As Exception
            l("Fehler in initStrassenliste: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Shared Function getStrassenlisteFromHTTP(gemeindebigNRstring As String, ByRef hinweis As String) As List(Of clsFlurauswahl)
        'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getstrassen&w1=438001
        Dim result As String
        Dim strassenl As New List(Of clsFlurauswahl)
        Try
            l(" MOD getStrassenlisteFromHTTP---------------------- anfang")
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick & "&modus=getstrassen&w1=" & gemeindebigNRstring
            l("getstrassen vorher " & Now.ToString)
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            l("getstrassen nachher " & Now.ToString)
            nachricht(hinweis)
            result = result.Trim
            If result.IsNothingOrEmpty Then
                Return Nothing
            End If
            strassenl = ajaxStrassenliste(result)
            Return strassenl
        Catch ex As Exception
            l("Fehler beim getStrassenlisteFromHTTP ", ex)
            Return Nothing
        End Try
    End Function

    Friend Shared Function getHausnrlisteFromHTTP(gemeindebigNRstring As String, strasseCode As Integer, hinweis As String) As List(Of clsFlurauswahl)
        Dim result As String
        Dim hausnrliste As New List(Of clsFlurauswahl)
        Try
            l(" MOD getHausnrlisteFromHTTP---------------------- anfang")
            aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick &
                "&modus=gethausnr" &
                "&gemeinde=" & gemeindebigNRstring &
                "&strcode=" & strasseCode
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            result = result.Trim
            If result.IsNothingOrEmpty Then
                Return Nothing
            End If
            hausnrliste = ajaxHausnrListe(result)
            If hausnrliste Is Nothing Then
                l("fehler in getHausnrlisteFromHTTP: " & gemeindebigNRstring & ", " & strasseCode)
            End If
            Return hausnrliste
        Catch ex As Exception
            l("Fehler beim getHausnrlisteFromHTTP ", ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function ajaxHausnrListe(result As String) As List(Of clsFlurauswahl)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim lok As New List(Of clsFlurauswahl)
        Dim hausnr As New clsFlurauswahl
        Dim oldname As String = ""
        Try
            l(" ajaxHausnrListe html---------------------- anfang")
            If result.IsNothingOrEmpty Then
                l("Fehler in ajaxHausnrListe: " & result)
                Return Nothing
            End If
            result = result.Trim
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            hausnr = New clsFlurauswahl
            For i = 0 To zeilen - 1
                hausnr = New clsFlurauswahl
                b = a(i).Split("#"c)
                hausnr.displayText = b(0).Trim
                hausnr.nenner = (b(1).Trim & "#" &
                                b(2).Trim & "#" &
                                b(3).Trim)
                lok.Add(hausnr)
            Next
            Return lok
            l(" ajaxHausnrListe ---------------------- ende")
        Catch ex As Exception
            l("Fehler in ajaxHausnrListe: " & result & Environment.NewLine, ex)
            Return Nothing
        End Try
    End Function

    'Friend Shared Function holeBoxKoordinatenFuerStrasseHTTP(innersql As String, ByRef hinweis As String) As String
    '    'http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=getstrassen&w1=438001
    '    Dim result As String
    '    Dim strassenl As New List(Of clsFlurauswahl)
    '    Try
    '        l(" MOD getStrassenlisteFromHTTP---------------------- anfang")
    '        aufruf = myglobalz.serverWeb & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=" & GisUser.nick & "&modus=getsql&w1=" & gemeindebigNRstring
    '        result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
    '        nachricht(hinweis)
    '        strassenl = ajaxStrassenliste(result)
    '        Return strassenl
    '    Catch ex As Exception
    '        l("Fehler beim getStrassenlisteFromHTTP " ,ex)
    '        Return Nothing
    '    End Try
    'End Function
End Class
