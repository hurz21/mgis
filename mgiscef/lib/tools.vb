Imports mgis

Public Class tools
    Shared Sub l(text As String)
        My.Log.WriteEntry(text)
    End Sub
    Shared Sub l(text As String, ex As Exception)
        My.Log.WriteEntry(text & Environment.NewLine & ex.ToString)
    End Sub

    Shared Sub paradigmavorgangaufrufen(paradigmaVID As String)
        'Dim modul, param As String
        Try
            l("paradigmavorgangaufrufen---------------------- anfang")
            'modul = strGlobals.paradigmadetail
            ''param = " /vid=" & paradigmaVID
            'Process.Start(modul, param)



            Dim si As New ProcessStartInfo
            si.FileName = strGlobals.paradigmadetail
            si.WorkingDirectory = "c:\kreisoffenbach\paradigmadetail"
            si.Arguments = " /vid=" & paradigmaVID
            'Process.Start(neuervorgangstgring, "modus=neu")
            Process.Start(si)
            si = Nothing


            l("paradigmavorgangaufrufen---------------------- ende")
        Catch ex As Exception
            l("Fehler in paradigmavorgangaufrufen: " & ex.ToString())
        End Try
    End Sub
    Shared Sub makeMapFile(ByVal inTemplateMapfile As String,
                        ByVal outKartenMAPfile As String,
                        ByVal KartenEbenenName As String,
                        ByVal mitetikett As Boolean,
                        enc As Text.Encoding,
                        GISusername As String)
        l("makeMapFile -----------------------------------------------")
        l(" templateMapfile$: " & inTemplateMapfile)
        l(" KartenMAPfile$$: " & outKartenMAPfile)
        Dim tempsafe As String
        If IO.File.Exists(inTemplateMapfile) Then
            l("Vorlage existiert")
            Using selVorlage As New IO.StreamReader(inTemplateMapfile, enc)
                tempsafe = selVorlage.ReadToEnd
                tempsafe = tempsafe.Replace("[FEATURECLASS]", KartenEbenenName)
                tempsafe = tempsafe.Replace("[SHAPEFILELOCATIONDIR]", "/paradigmacache/" & GISusername)
                If Not mitetikett Then
                    tempsafe = tempsafe.Replace("Labelitem 'RBTITEL'#beipoint", "Labelitem 'RBTYP'")
                End If
            End Using
            My.Computer.FileSystem.WriteAllText(outKartenMAPfile, tempsafe, False, enc)
            l("Mapfile$ wurde erzeugt: " & KartenEbenenName)
        Else
            l("FEHLER: Vorlage exitiert nicht")
        End If
    End Sub



    Shared Sub makeMapFilePostgis(ByVal inTemplateMapfile As String,
                    ByVal outKartenMAPfile As String,
                    ByVal KartenEbenenName As String,
                    ByVal mitetikett As Boolean,
                    enc As Text.Encoding,
                    tableName As String)
        l("makeMapFile -----------------------------------------------")
        l(" templateMapfile$: " & inTemplateMapfile)
        l(" KartenMAPfile$$: " & outKartenMAPfile)
        Dim tempsafe As String
        If IO.File.Exists(inTemplateMapfile) Then
            l("Vorlage existiert")
            Using selVorlage As New IO.StreamReader(inTemplateMapfile, enc)
                tempsafe = selVorlage.ReadToEnd
                tempsafe = tempsafe.Replace("[FEATURECLASS]", KartenEbenenName)
                tempsafe = tempsafe.Replace("[PG_SCHEMA.TABELLE]", "paradigma_userdata." & tableName)
                If Not mitetikett Then
                    tempsafe = tempsafe.Replace("Labelitem 'RBTITEL'#beipoint", "Labelitem 'RBTYP'")
                End If
            End Using
            My.Computer.FileSystem.WriteAllText(outKartenMAPfile, tempsafe, False, enc)
            l("Mapfile$ wurde erzeugt: " & KartenEbenenName)
        Else
            l("FEHLER: Vorlage exitiert nicht")
        End If
    End Sub

    'Friend Shared Sub getUsergroup()
    '    clsActiveDir.getall(GisUser.nick)
    '    If clsActiveDir.fdkurz.Trim.ToLower = "umwelt" Or
    '            clsActiveDir.fdkurz.Trim.ToLower = "bauaufsicht" Then
    '        'Return True
    '    Else
    '        'Return False
    '    End If
    'End Sub



    Public Shared Sub openDocument(pdfEigentuemerDatei As String)
        Dim f1 As New IO.FileInfo(pdfEigentuemerDatei)
        Try
            If f1.Exists Then
                Process.Start(pdfEigentuemerDatei)
            End If
        Catch ex As Exception
            l("fehler in openDocument " & pdfEigentuemerDatei, ex)
        End Try
    End Sub

    Friend Shared Function liegtImkreisOffenbach(koordinateKLickpt As Point?) As Boolean
        Try
            l("liegtImkreisOffenbach---------------------- anfang")
            If koordinateKLickpt.HasValue Then
                If koordinateKLickpt.Value.X > 0 And koordinateKLickpt.Value.X < 169 And
                        koordinateKLickpt.Value.Y < 97 Then
                    'linksoben
                    Return False
                End If
                If koordinateKLickpt.Value.X > 266 And koordinateKLickpt.Value.Y < 66 Then
                    'rechtsoben
                    Return False
                End If

                If koordinateKLickpt.Value.X > 285 And koordinateKLickpt.Value.Y > 218 Then
                    'rechtsunten
                    Return False
                End If
            Else
                Return False
            End If
            Return True
            l("liegtImkreisOffenbach---------------------- ende")
        Catch ex As Exception
            l("Fehler in liegtImkreisOffenbach: ", ex)
            Return False
        End Try
    End Function

    Shared Function holeSpaltenKoepfe(basisrec As clsDBspecPG, schema As String, tabelle As String) As clsDBspecPG
        Try
            l("holeSpaltenKoepfe---------------------- anfang")
            basisrec.mydb.SQL = "Select  column_name From information_schema.columns Where table_schema = '" & schema & "'" &
                                " And table_name = '" & tabelle & "'"
            basisrec.getDataDT()
            l(basisrec.mydb.SQL)
            Return basisrec
            l("holeSpaltenKoepfe---------------------- ende")
        Catch ex As Exception
            l("Fehler in holeSpaltenKoepfe: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Shared Function reduziereEtikettAufTitel(nlay As clsLayerPres) As String
        l("reduziereTitel---------------------- anfang")
        Dim a() As String
        Try
            If nlay.Etikett.Contains("#") Then
                a = nlay.Etikett.Split("#"c)
                ' nlay.titel = a(0)
                l("-reduziereTitel--------------------- ende")
                Return a(1)
            Else
                Return nlay.Etikett
            End If
        Catch ex As Exception
            l("Fehler in reduziereTitel: " & ex.ToString())
            Return nlay.Etikett
        End Try
    End Function

    Friend Shared Sub rangeSpeichern(aktrange As clsRange)
        Try
            If Not (STARTUP_mgismodus = "paradigma") Then
                If iminternet Or CGIstattDBzugriff Then
                    clsLastrange.lastrangeDBsave(GisUser.nick, aktrange)
                    'userIniProfile.WertSchreiben("MAPSETTING", "lastrange",
                    '                             CInt(aktrange.xl) & "," & CInt(aktrange.xh) & "," &
                    '                               CInt(aktrange.yl) & "," & CInt(aktrange.yh)
                    ')
                Else
                    clsLastrange.lastrangeDBsave(GisUser.nick, aktrange)
                End If
            End If
        Catch ex As Exception
            l("fehler in rangeSpeichern ", ex)
        End Try
    End Sub
    Friend Shared Function rangeLadenLastOne() As clsRange
        Dim aktrange As New clsRange
        Try
            'If iminternet Then
            '    'aktrange = clsWebgisPGtools.lastrangeLadenHTTP(GisUser.nick)
            '    If String.IsNullOrEmpty(userIniProfile.WertLesen("MAPSETTING", "lastrange")) Then
            '        '  userIniProfile.WertSchreiben("MAPSETTING", "lastrange", "0")

            '    Else
            '        Dim temp = userIniProfile.WertLesen("MAPSETTING", "lastrange")
            '        Dim a() As String
            '        a = temp.Split(","c)
            '        aktrange.xl = CDbl(a(0))
            '        aktrange.xh = CDbl(a(1))
            '        aktrange.yl = CDbl(a(2))
            '        aktrange.yh = CDbl(a(3))
            '    End If
            'Else
            aktrange = clsLastrange.lastrangeLadenDB(GisUser.nick)
            'End If

            Return aktrange
        Catch ex As Exception
            l("fehler in rangeLaden ", ex)
            Return Nothing
        End Try
    End Function

    Friend Shared Sub GISeditoraufrufen(layeraid As Integer, username As String, gid As String, editid As String)
        l("GISeditoraufrufen---------------------- anfang")
        Dim param As String
        Try
            'modul = strGlobals.gisEdit
            param = " layeraid=" & layeraid '
            param = param & " gid=" & gid ' 
            param = param & " username=" & username ' 
            param = param & " editid=" & editid ' 
            'Process.Start(modul, param)

            Dim si As New ProcessStartInfo
            si.FileName = strGlobals.gisEdit
            si.WorkingDirectory = "c:\ptest\gisedit"
            si.Arguments = param
            'Process.Start(neuervorgangstgring, "modus=neu")
            Process.Start(si)
            si = Nothing

            l("GISeditoraufrufen---------------------- ende")
        Catch ex As Exception
            l("Fehler in GISeditoraufrufen: " & ex.ToString())
        End Try
    End Sub

    Friend Shared Function calcEigentuemerAusgabeFile() As String
        Dim EigentuemerPDF As String
        Dim ausgabeDIR As String ' = My.Computer.FileSystem.SpecialDirectories.Temp '& "" & aid
        Try
            l("calcEigentuemerAusgabeFile---------------------- anfang")
            ausgabeDIR = System.IO.Path.Combine(strGlobals.localDocumentCacheRoot, "Eigentuemer")
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

    'Friend Shared Sub dirSpeichern()
    '    Dim dateiname As String
    '    '  Exit Sub
    '    Try
    '        l("dirSpeichern---------------------- anfang")
    '        dateiname = clsString.normalize(GisUser.ADgruppenname) & "_" & GisUser.nick & ".log"
    '        dateiname = clsString.normalize(GisUser.ADgruppenname) & ".log"
    '        dateiname = myglobalz.serverUNC & "apps\test\mgis\lastrange\" & dateiname
    '        Dim fi As New IO.FileInfo(dateiname)
    '        If fi.Exists Then Exit Sub
    '        fi = Nothing
    '        Dim sw As New IO.StreamWriter(dateiname)
    '        Dim oSubDir As IO.DirectoryInfo
    '        Dim odir = New IO.DirectoryInfo("o:")
    '        l("---------------------- anfang")
    '        alleLaufwerke(sw)
    '        oSubDir = allesUnterO(sw, odir)
    '        sw.Close()
    '        sw.Dispose()
    '        l("dirSpeichern---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in dirSpeichern: " & ex.ToString())
    '    End Try
    'End Sub

    'Private Shared Sub alleLaufwerke(sw As IO.StreamWriter)
    '    For Each Drive As IO.DriveInfo In IO.DriveInfo.GetDrives
    '        If Drive.DriveType = IO.DriveType.CDRom Then
    '            If Drive.IsReady Then
    '                ' wenn Gerät bereit, Laufwekrsbuchstabe und VolumeLabel anzeigen
    '                sw.WriteLine(Drive.Name & " [" & Drive.VolumeLabel & "]")
    '                'ListBox1.Items.Add(Drive.Name & " [" & Drive.VolumeLabel & "]")
    '            Else
    '                ' andernfalls nur Laufwerksbuchstabe anzeigen
    '                sw.WriteLine(Drive.Name & " [nicht bereit]")
    '            End If
    '        End If
    '    Next
    'End Sub

    'Private Shared Function allesUnterO(sw As IO.StreamWriter, odir As IO.DirectoryInfo) As IO.DirectoryInfo
    '    Dim oSubDir As IO.DirectoryInfo

    '    For Each oSubDir In odir.GetDirectories()
    '        sw.WriteLine(oSubDir.FullName)
    '        For Each osubdir2 In oSubDir.GetDirectories
    '            sw.WriteLine(oSubDir.FullName)
    '            For Each osubdir3 In osubdir2.GetDirectories
    '                sw.WriteLine(osubdir3.FullName)
    '                For Each osubdir4 In osubdir3.GetDirectories
    '                    sw.WriteLine(osubdir4.FullName)
    '                Next
    '            Next
    '        Next
    '    Next

    '    Return oSubDir
    'End Function

    Friend Shared Sub geometieNachParadigmaUebernehmen(aktvorgangsid As String, aktPolygon As clsParapolygon)
        Throw New NotImplementedException()

    End Sub
End Class
