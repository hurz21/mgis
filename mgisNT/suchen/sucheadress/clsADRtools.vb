Imports System.Data
Imports mgis

Public Class clsADRtools
    Shared Function erzeugeUndOeffneEigentuemerPDF(tbWeitergabeVerbot As String, summentex As String) As String
        Dim lokalitaet, flaeche As String
        'Dim ausgabeDIR As String = My.Computer.FileSystem.SpecialDirectories.Temp '& "" & aid
        'ausgabeDIR = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        lokalitaet = summentex
        flaeche = clsFSTtools.getFlaecheZuFlurstueck(aktFST)
        lokalitaet = lokalitaet & " " & flaeche
        'IO.Directory.CreateDirectory(ausgabeDIR)
        'Dim ausgabedatei As String = ausgabeDIR & "\eigentuemer" & Format(Now, "dd.MM.yyyy_hhmmss") & ".pdf"
        Dim ausgabedatei As String = tools.calcEigentuemerAusgabeFile
        wrapItextSharp.createSchnellEigentuemer(tbWeitergabeVerbot, ausgabedatei, albverbotsString, lokalitaet)
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
            l("Fehler in initHausnrliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Shared Function initStrassenliste(result As String) As List(Of clsFlurauswahl)
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim lok As New List(Of clsFlurauswahl)
        Dim strasse As New clsFlurauswahl
        Dim oldname As String = ""
        Try
            l(" initStrassenliste html---------------------- anfang")
            result = result.Trim
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
            l(" initStrassenliste ---------------------- ende")
        Catch ex As Exception
            l("Fehler in initStrassenliste: " & ex.ToString())
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
            l("Fehler in initStrassenliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Shared Function getStrassenlisteFromHTTP(gemeindebigNRstring As String, ByRef hinweis As String) As List(Of clsFlurauswahl)
        Dim zeilen, spalten As Integer
        Dim result As String
        Dim strassenl As New List(Of clsFlurauswahl)
        'Dim Pres As New List(Of clsLayerPres)
        Try
            l(" MOD getAllLayersFromHttp---------------------- anfang")
            'aufruf = "http://w2gis02.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=weinachtsmann&modus=getstamm"
            aufruf = strGlobals.buergergisInternetServer & "/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=weinachtsmann&modus=getstrassen&w1=" & gemeindebigNRstring
            result = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
            nachricht(hinweis)
            Return initStrassenliste(result)
        Catch ex As Exception

        End Try
    End Function
End Class
