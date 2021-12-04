Imports System.Data
Imports gisEdit
Class clstools
    Public Shared ParadigmaVersion As String
    Public Shared editTable As String
    Public Shared editSchema As String
    Public Shared editUsername As String
    Public Shared editLayerAid As String
    Public Shared editOjektGIDNr As String
    Public Shared editgid As String
    Public Shared isRemoteCall As Boolean = False
    Public Shared editOjektGIDNSpaltenname As String
    Public Shared lu, ro As Point
    Shared Property _CLstart_mycSimple_enc As System.Text.Encoding
    Public query As String
    Shared Sub l(v As String)
        My.Log.WriteEntry(v)
    End Sub
    Friend Shared Sub google3d(aktrange As clsRange, enc As Text.Encoding)

        Dim mm As New myPoint
        aktrange.CalcCenter()
        mm.X = aktrange.xcenter
        mm.Y = aktrange.ycenter
        clstools.google3dintro(mm, "", enc)
    End Sub
    Shared Sub google3dintro(mitte As myPoint, serverunc As String, enc As Text.Encoding)
        Dim gis As New clsGISfunctions
        Dim result As String
        Dim nbox As New clsRange
        Dim longitude, latitude As String
        Dim punktarrayInM() As myPoint

        Try
            nachricht("USERAKTION: google3dintro  vogel btn click")
            ' calcBbox(rechts, hoch, nbox, 900)
            Dim radius = 300
            nbox.xl = CInt(mitte.X) - radius
            nbox.yl = CInt(mitte.Y) - (radius * 2)
            nbox.xh = CInt(mitte.X) + radius
            nbox.yh = CInt(mitte.Y)

            result = gis.GoogleMapsAufruf_MittelpunktMItPunkteUebergabe(nbox, False, longitude, latitude, serverunc, enc,
                                                                        punktarrayInM)
            l("result: " & result)
            If result = "fehler" Or result = "" Then
            Else
                Process.Start("chrome.exe", result)
            End If
            gis = Nothing
            ' Protokollausgabe_aller_Zugriff("ja")

        Catch ex As Exception
            nachricht("fehler in starteWebbrowserControl: " & ex.ToString)
        End Try
    End Sub

    Shared Sub ndindivEditorAnlegen(aktgidlok As String, gruppenID As String)
        mset.basisrec.mydb.SQL = "insert into paradigma_userdata.ndindividuenedit " &
                "  (gid,gruppenid) values (" & aktgidlok & "," & gruppenID & ")"
        Dim newid As Long
        Dim res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
    End Sub
    Shared Function OpenDocument(ByVal DocumentFile As String) As Boolean
        Try
            l("OpenDocument DocumentFile:" & vbCrLf & DocumentFile)
            If DocumentFile Is Nothing OrElse DocumentFile = String.Empty Then
                MsgBox("Der Dateiname ist leer ")
                Return False
            End If
            Dim pInfo As New Diagnostics.ProcessStartInfo
            Dim test As New IO.FileInfo(DocumentFile)
            If Not test.Exists Then
                MessageBox.Show("Die Datei existiert nicht. " & test.Name)
                l("FEHLER:	 Die Datei existiert nicht: " & test.FullName)
                test = Nothing
                Return False
            End If
            With pInfo
                ' Dokument	
                .FileName = DocumentFile
                ' verknüpfte Anwendung starten
                .Verb = "open"
            End With
            test = Nothing
            Process.Start(pInfo)
            l("OpenDocument erfolgreich: ")
            Return True
        Catch ex As Exception
            l("OpenDocument FEHLER: " & vbCrLf & ex.ToString)
            Return False
        End Try
    End Function

    Friend Shared Function getcoord(ndindividuenListe As List(Of clsNDinidividuum)) As myPoint
        Dim np As New myPoint
        Try
            l(" getcoord ---------------------- anfang")
            For Each ele As clsNDinidividuum In ndindividuenListe
                np.X = ele.rechts
                np.Y = ele.hoch
            Next

            l(" getcoord ---------------------- ende")
            Return np
        Catch ex As Exception
            l("Fehler in getcoord: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Shared Function dt2NDgruppen(dBDT As DataTable) As List(Of clsNDgruppe)
        Dim loklist As New List(Of clsNDgruppe)
        Dim ndg As New clsNDgruppe
        Try
            clstools.l(" dt2NDgruppen ---------------------- anfang")
            For i = 0 To dBDT.Rows.Count - 1
                ndg = clstools.rec2NDgruppenOBJ(dBDT.Rows(i))
                loklist.Add(ndg)
            Next
            clstools.l(" dt2NDgruppen ---------------------- ende")
            Return loklist
        Catch ex As Exception
            clstools.l("Fehler in dt2NDgruppen: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Shared Function rec2NDgruppenOBJ(dataRow As DataRow) As clsNDgruppe
        Dim ndg As New clsNDgruppe
        Try
            clstools.l(" rec2NDgruppenOBJ ---------------------- anfang")
            ndg.aid = CInt(clsDBtools.fieldvalue(dataRow.Item("aid")))
            'ndg.gid = CInt(clsDBtools.fieldvalue(dataRow.Item("gid")))
            'ndg.beschreibung = (clsDBtools.fieldvalue(dataRow.Item("beschreibung")))
            'ndg.erloschen = (clsDBtools.fieldvalue(dataRow.Item("erloschen")))
            'ndg.flur = (clsDBtools.fieldvalue(dataRow.Item("flur")))
            ndg.beschreibung = (clsDBtools.fieldvalue(dataRow.Item("name_2")))
            ndg.gemarkung = (clsDBtools.fieldvalue(dataRow.Item("gemarkung")))
            ndg.gemeinde = (clsDBtools.fieldvalue(dataRow.Item("gemeinde")))
            'ndg.grundeigentum = (clsDBtools.fieldvalue(dataRow.Item("grundeigentum")))
            'ndg.hochwert = CDbl(clsDBtools.fieldvalue(dataRow.Item("hochwert")))
            'ndg.hoehe = (clsDBtools.fieldvalue(dataRow.Item("hoehe")))
            'ndg.kreis = (clsDBtools.fieldvalue(dataRow.Item("kreis")))
            'ndg.kronenbreite = (clsDBtools.fieldvalue(dataRow.Item("kronenbreite")))
            'ndg.name = (clsDBtools.fieldvalue(dataRow.Item("name")))

            'ndg.ordnungswidrig = (clsDBtools.fieldvalue(dataRow.Item("ordnungswidrig")))
            'ndg.rechtswert = CDbl(clsDBtools.fieldvalue(dataRow.Item("rechtswert")))
            'ndg.schutzgrund = (clsDBtools.fieldvalue(dataRow.Item("schutzgrund")))
            'ndg.stammumfang = (clsDBtools.fieldvalue(dataRow.Item("stammumfang")))
            'ndg.tk25 = (clsDBtools.fieldvalue(dataRow.Item("tk25")))
            'ndg.umgebung = (clsDBtools.fieldvalue(dataRow.Item("umgebung")))

            'ndg.veroeffentlicht = (clsDBtools.fieldvalue(dataRow.Item("veroeffentlicht")))
            'ndg.veroeff_geloescht = (clsDBtools.fieldvalue(dataRow.Item("veroeff_geloescht")))

            'ndg.ZusatzInfo.ablaufdatumks = (clsDBtools.fieldvalueDate(dataRow.Item("ablaufdatumks")))
            'ndg.ZusatzInfo.bemerkung = (clsDBtools.fieldvalue(dataRow.Item("bemerkung")))
            'ndg.ZusatzInfo.kronensicherung = CBool((clsDBtools.fieldvalue(dataRow.Item("kronensicherung"))))
            'ndg.ZusatzInfo.paradigmaVID = (clsDBtools.fieldvalue(dataRow.Item("paradigmaVID")))
            'ndg.ZusatzInfo.regelkontrolle = (clsDBtools.fieldvalueDate(dataRow.Item("regelkontrolle")))
            'ndg.ZusatzInfo.auge = (clsDBtools.fieldvalueDate(dataRow.Item("auge")))
            'ndg.ZusatzInfo.untersuchung = (clsDBtools.fieldvalueDate(dataRow.Item("untersuchung")))
            clstools.l(" rec2NDgruppenOBJ ---------------------- ende")
            Return ndg
        Catch ex As Exception
            clstools.l("Fehler in rec2NDgruppenOBJ: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Shared Function genSQLNDindividuen(gemeindename As String, textfilter As String) As String
        Dim SQLindivi = "SELECT * " &
                            "   FROM schutzgebiete.naturdenkmal_f " &
                            "   left outer join  paradigma_userdata.ndindividuenedit  on " &
                            "   schutzgebiete.naturdenkmal_f.gid = paradigma_userdata.ndindividuenedit.gid " &
                            " where schutzgebiete.naturdenkmal_f.gid>0 "

        Dim result, Gemeindestring, textfilterstring As String
        Dim orderstring As String ' = " order by gemeinde,gemarkung"
        orderstring = " order by regelkontrolle,auge,untersuchung,ablaufdatumks"
        Try
            clstools.l(" genSQLNDgruppen ---------------------- anfang")

            textfilterstring = genTextfilterstring(textfilter)
            Gemeindestring = genGemeindeString(gemeindename)
            result = SQLindivi & Gemeindestring & textfilterstring & orderstring
            clstools.l(" genSQLNDgruppen ---------------------- ende")
            Return result
        Catch ex As Exception
            clstools.l("Fehler in genSQLNDgruppen: " & ex.ToString())
            Return ""
        End Try
    End Function

    Friend Shared Function genSQLNDgruppen(gemeindename As String, textfilter As String) As String
        Dim queryNDgruppen As String = "select * from  schutzgebiete.naturdenkmal_a  a,paradigma_userdata.nd_paradigma p " &
                                      " where p.gruppenid= a.aid "

        queryNDgruppen = " select   aid,gemeinde,gemarkung,name_2 from  schutzgebiete.naturdenkmal_f  a," &
                        " paradigma_userdata.nd_paradigma p  where p.gruppenid= a.aid   group by aid  ,gemeinde,gemarkung,name_2  "
        Dim orderstring As String = " order by gemeinde,gemarkung"
        Dim result, Gemeindestring, textfilterstring As String
        Try
            clstools.l(" genSQLNDgruppen ---------------------- anfang")
            textfilterstring = genTextfilterstring(textfilter)
            Gemeindestring = genGemeindeString(gemeindename)
            result = queryNDgruppen & Gemeindestring & textfilterstring & orderstring
            clstools.l(" genSQLNDgruppen ---------------------- ende")
            Return result
        Catch ex As Exception
            clstools.l("Fehler in genSQLNDgruppen: " & ex.ToString())
            Return ""
        End Try
    End Function

    Shared Function genTextfilterstring(textfilter As String) As String
        Dim result As String = ""
        If textfilter.IsNothingOrEmpty Then
            result = ""
        Else
            result = " and ((trim(lower(name || gruppenid)) like '%" & textfilter.ToLower.Trim & "%') " &
                " or (trim(lower(bemerkung)) like '%" & textfilter.ToLower.Trim & "%'))"
        End If
        Return result
    End Function

    Shared Function genGemeindeString(ByRef gemeindename As String) As String
        Dim result As String = ""
        If gemeindename.IsNothingOrEmpty Or gemeindename = "Gemeinde" Then
            result = ""
        Else
            result = " and trim(lower(gemeinde))='" & gemeindename.ToLower.Trim & "' "
        End If
        Return result
    End Function

    Friend Shared Sub mapAllArguments(arguments() As String)
        ' layeraid=161 gid=3 username=Feinen_J editid=438002
        Try
            l("mapAllArguments---------------------- anfang")
            For Each sttelement In arguments
                If sttelement.Contains("tabelle=") Then
                    l("tabelle ")
                    editTable = sttelement.Replace("tabelle=", "").Trim.ToLower
                End If
                If sttelement.Contains("layeraid=") Then
                    l("layeraid ")
                    editLayerAid = sttelement.Replace("layeraid=", "").Trim.ToLower
                End If
                If sttelement.Contains("username=") Then
                    l("username ")
                    editUsername = sttelement.Replace("username=", "").Trim.ToLower
                End If
                If sttelement.Contains("editid=") Then
                    l("editOjektGIDNr ")
                    editOjektGIDNr = sttelement.Replace("editid=", "").Trim.ToLower
                    isRemoteCall = True
                End If
                If sttelement.Contains("gid=") Then
                    l("gid ")
                    editgid = sttelement.Replace("gid=", "").Trim.ToLower
                    isRemoteCall = True
                End If
            Next
            l("mapAllArguments---------------------- ende")
        Catch ex As Exception
            l("Fehler in mapAllArguments: " & ex.ToString())
        End Try
    End Sub
    Public Shared Function ohneSemikolon(ByRef p1 As String) As String
        Try
            If String.IsNullOrEmpty(p1) Then
                Return ""
            End If
            Dim temp$ = p1
            temp = temp.Trim
            temp = temp.Replace(";", "_")
            temp = temp.Replace(vbCrLf, "")
            Return temp
        Catch ex As Exception
            l("Fehler in ohneSemikolon: " & ex.ToString)
            Return ""
        End Try
    End Function
    Shared Sub beliebig(tab As DataTable, delim As String, sw As IO.StreamWriter)
        Try
            For i = 0 To tab.Columns.Count - 1
                sw.Write(ohneSemikolon(tab.Columns(i).ColumnName.ToString) & delim)
            Next
            sw.WriteLine(delim)
            For Each p As DataRow In tab.AsEnumerable   'myGlobalz.sitzung.EreignisseRec.dt.AsEnumerable
                For i = 0 To tab.Columns.Count - 1
                    sw.Write(ohneSemikolon(p.Item(i).ToString) & delim)
                Next
                sw.WriteLine(delim)
            Next
        Catch ex As Exception
            l("Fehler bei der Excelausgabeg" & vbCrLf & ex.ToString)
        End Try
    End Sub
    Shared Sub createexcelfile(excelfile As String, liste As List(Of clsNDinidividuum))
        Dim sw As IO.StreamWriter
        Dim delim As String = ";"
        Try
            sw = New IO.StreamWriter(excelfile, False, System.Text.Encoding.GetEncoding("iso-8859-1"))
            'beliebig(mset.basisrec.dt, delim, sw)
            For i = 0 To liste.Count - 1
                sw.Write(ohneSemikolon(liste.Item(i).aid.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).gemeinde.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).gemarkung.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).beschreibung.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).flaeche_qm.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).gid.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).lfd_nr.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).name.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).plakette.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).radius.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).rechts.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).hoch.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).umgebung.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).vid.ToString) & delim)

                sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.ablaufdatumks.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.auge.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.bemerkung.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.kronensicherung.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.verkehrssicher.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.paradigmaVID.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.regelkontrolle.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.untersuchung.ToString) & delim)
                sw.WriteLine(delim)
            Next
            '  sw.WriteLine(vbCrLf)
            'For Each p As DataRow In TAB.AsEnumerable   'myGlobalz.sitzung.EreignisseRec.dt.AsEnumerable
            '    For i = 0 To TAB.Columns.Count - 1
            '        sw.Write(ohneSemikolon(p.Item(i).ToString) & delim)
            '    Next
            '    sw.WriteLine(delim)
            'Next


        Catch ex As Exception
            l("Fehler bei der Excelausgabea" & vbCrLf & ex.ToString)
        Finally
            sw.Close()
            sw.Dispose()
        End Try
    End Sub
    Shared Sub createexcelfile(excelfile As String, liste As List(Of clsNDgruppe))
        Dim sw As IO.StreamWriter
        Dim delim As String = ";"
        Try
            sw = New IO.StreamWriter(excelfile, False, System.Text.Encoding.GetEncoding("iso-8859-1"))
            'beliebig(mset.basisrec.dt, delim, sw)
            For i = 0 To liste.Count - 1
                sw.Write(ohneSemikolon(liste.Item(i).aid.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).gemeinde.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).gemarkung.ToString) & delim)
                sw.Write(ohneSemikolon(liste.Item(i).beschreibung.ToString) & delim)
                'sw.Write(ohneSemikolon(liste.Item(i).gid.ToString) & delim)
                'sw.Write(ohneSemikolon(liste.Item(i).name.ToString) & delim)
                'sw.Write(ohneSemikolon(liste.Item(i).umgebung.ToString) & delim)

                'sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.ablaufdatumks.ToString) & delim)
                'sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.auge.ToString) & delim)
                'sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.bemerkung.ToString) & delim)
                'sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.kronensicherung.ToString) & delim)
                'sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.paradigmaVID.ToString) & delim)
                'sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.regelkontrolle.ToString) & delim)
                'sw.Write(ohneSemikolon(liste.Item(i).ZusatzInfo.untersuchung.ToString) & delim)
                sw.WriteLine(delim)
            Next
        Catch ex As Exception
            l("Fehler bei der Excelausgabeb" & vbCrLf & ex.ToString)
        Finally
            sw.Close()
            sw.Dispose()
        End Try
    End Sub

    Friend Shared Function darfAendern(userName As String) As Boolean
        If userName.ToLower = "feinen_j" Then Return True
        If userName.ToLower = "stich_k" Then Return True
        If userName.ToLower = "waldschmitt_r" Then Return True
        Return False
    End Function

    Friend Shared Function dt2NDindivuduen(dBDT As DataTable) As List(Of clsNDinidividuum)
        Dim loklist As New List(Of clsNDinidividuum)
        Dim ndg As New clsNDinidividuum
        Try
            l(" dt2NDgruppen ---------------------- anfang")
            For i = 0 To dBDT.Rows.Count - 1
                ndg = rec2NDindividuenOBJ(dBDT.Rows(i))
                loklist.Add(ndg)
            Next
            l(" dt2NDgruppen ---------------------- ende")
            Return loklist
        Catch ex As Exception
            l("Fehler in dt2NDgruppen: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Shared Function rec2NDindividuenOBJ(dataRow As DataRow) As clsNDinidividuum
        Dim ndg As New clsNDinidividuum
        Try
            l(" rec2NDindividuenOBJ ---------------------- anfang")
            ndg.aid = CInt(clsDBtools.fieldvalue(dataRow.Item("aid")))
            ndg.gid = CInt(clsDBtools.fieldvalue(dataRow.Item("gid")))
            ndg.gemarkung = (clsDBtools.fieldvalue(dataRow.Item("gemarkung")))
            ndg.lfd_nr = CInt(clsDBtools.fieldvalue(dataRow.Item("lfd_nr")))
            ndg.gemeinde = (clsDBtools.fieldvalue(dataRow.Item("gemeinde")))
            ndg.rechts = CDbl(clsDBtools.fieldvalue(dataRow.Item("rechts")))
            ndg.hoch = CDbl(clsDBtools.fieldvalue(dataRow.Item("hoch")))
            ndg.name = (clsDBtools.fieldvalue(dataRow.Item("name")))
            ndg.beschreibung = (clsDBtools.fieldvalue(dataRow.Item("name_2")))
            ndg.umgebung = (clsDBtools.fieldvalue(dataRow.Item("umgebung")))
            ndg.radius = CInt(clsDBtools.fieldvalue(dataRow.Item("radius")))
            ndg.plakette = (clsDBtools.fieldvalue(dataRow.Item("plakette")))
            ndg.flaeche_qm = CInt(clsDBtools.fieldvalue(dataRow.Item("flaeche_qm")))
            ndg.vid = (clsDBtools.fieldvalue(dataRow.Item("paradigmavid")))
            'ndg.bemerkung = (clsDBtools.fieldvalue(dataRow.Item("bemerkung"))).Trim
            ndg.ZusatzInfo.ablaufdatumks = (clsDBtools.fieldvalueDate(dataRow.Item("ablaufdatumks")))
            ndg.ZusatzInfo.bemerkung = (clsDBtools.fieldvalue(dataRow.Item("bemerkung")))
            ndg.ZusatzInfo.kronensicherung = CBool((clsDBtools.toBool(dataRow.Item("kronensicherung"))))
            ndg.ZusatzInfo.verkehrssicher = CBool((clsDBtools.toBool(dataRow.Item("verkehrssicher"))))
            ndg.ZusatzInfo.paradigmaVID = (clsDBtools.fieldvalue(dataRow.Item("paradigmaVID")))
            ndg.ZusatzInfo.regelkontrolle = (clsDBtools.fieldvalueDate(dataRow.Item("regelkontrolle")))
            ndg.ZusatzInfo.auge = (clsDBtools.fieldvalueDate(dataRow.Item("auge")))
            ndg.ZusatzInfo.untersuchung = (clsDBtools.fieldvalueDate(dataRow.Item("untersuchung")))
            l(" rec2NDindividuenOBJ ---------------------- ende")
            Return ndg
        Catch ex As Exception
            l("Fehler in rec2NDindividuenOBJ: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Shared Sub loescheGruppe(gruppenID As String, ndindividuenListe As List(Of clsNDinidividuum))
        Try
            l(" loescheGruppe ---------------------- anfang")
            If clstools.sindNochIndividuenVorhanden(gruppenID, ndindividuenListe) Then
                MessageBox.Show("Es nind noch Individuen in der Gruppe vorhanden! Löschung der Gruppe nicht möglich, Abbruch!",
                                "Keine Löschung möglich", MessageBoxButton.OK, MessageBoxImage.Exclamation)
                Exit Sub
            Else
                Dim erfolg As Boolean = clstools.LoescheGruppeInDB(gruppenID)
                If erfolg Then
                    MessageBox.Show("Gruppe wurde gelöscht. Bitte Editor schließen!")
                Else
                    MessageBox.Show("Gruppe konnte nicht gelöscht werden!")
                End If
            End If
            l(" loescheGruppe ---------------------- ende")
        Catch ex As Exception
            l("Fehler in loescheGruppe: " & ex.ToString())
        End Try
    End Sub

    Shared Function LoescheGruppeInDB(gruppenID As String) As Boolean
        Dim newid, res As Long
        Try
            l(" LoescheGruppeInDB ---------------------- anfang")
            mset.basisrec.mydb.SQL = "delete from schutzgebiete.naturdenkmal_a where  " &
                                        "  aid='" & gruppenID & "'"
            l(mset.basisrec.mydb.SQL)
            res = mset.basisrec.sqlexecute(newid) : clstools.l(mset.basisrec.hinweis)
            l(" LoescheGruppeInDB ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in LoescheGruppeInDB: " & ex.ToString())
            Return False
        End Try
    End Function

    Shared Function sindNochIndividuenVorhanden(gruppenID As String, ndindividuenListe As List(Of clsNDinidividuum)) As Boolean
        Try
            l(" sindNochIndividuenVorhanden ---------------------- anfang")
            If ndindividuenListe.Count > 0 Then

                Return True
            Else
                Return False
            End If
            l(" sindNochIndividuenVorhanden ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in sindNochIndividuenVorhanden: " & ex.ToString())
            Return False
        End Try
    End Function
    Shared Sub paradigmavorgangaufrufen(paradigmaVID As String)
        Dim modul, param As String
        Try
            l("paradigmavorgangaufrufen---------------------- anfang")
            modul = "c:\ptest\paradigmadetail\paradigmadetail.exe"
            param = " /vid=" & paradigmaVID '
            Process.Start(modul, param)
            l("paradigmavorgangaufrufen---------------------- ende")
        Catch ex As Exception
            l("Fehler in paradigmavorgangaufrufen: " & ex.ToString())
        End Try
    End Sub
    Shared Function getGoogleMapsString(drange As clsRange, enc As Text.Encoding) As String ' kartengen.aktMap.aktrange
        Try
            l("USERAKTION: getGoogleMapsString   ")
            Dim gis As New clsGISfunctions
            Dim result As String
            Dim punktarrayInM() As myPoint
            drange.CalcCenter()
            result = gis.GoogleMapsAufruf_Extern(drange, True, enc, punktarrayInM)
            l("result: " & result)
            If result = "fehler" Or result = "" Then
                Return ""
            Else
                '  gis.starten(result)
                '  GMtemplates.templateStarten(result)
                Return result
            End If
            gis = Nothing
        Catch ex As Exception
            l("fehler in getGoogleMapsString: " & ex.ToString)
            Return ""
        End Try
    End Function
    Friend Shared Function calcrangestring(lu As myPoint, ro As myPoint) As String
        Dim puffer As Double
        Dim res As String = ""
        Try
            l(" calcrangestring ---------------------- anfang")
            puffer = Math.Abs(lu.X - ro.X)
            puffer = puffer / 2

            res = res & CInt((lu.X - puffer)).ToString & ","
            res = res & CInt((ro.X + puffer)).ToString & ","
            res = res & CInt((lu.Y - puffer)).ToString & ","
            res = res & CInt((ro.Y + puffer)).ToString

            l(" calcrangestring ---------------------- ende")
            Return res
        Catch ex As Exception
            l("Fehler in calcrangestring: " & ex.ToString())
            Return ""
        End Try
    End Function
    Friend Shared Function genPreviewURL(nd As clsNDinidividuum, aktrange As clsRange, breite As Integer, hoehe As Integer, hgrund As String, ndeinzeln As Boolean) As String
        Try
            l(" genPreview ---------------------- anfang")

            'l("klein: " & klein)
            Dim mapsize, gid As String
            Dim Mapfile As String
            'http://gis.kreis-of.local/cgi-bin/mapserv722/mapserv.exe?mode=map&mapsize=400+300&mapext=484999+5542166+485168+5542289&map=/inetpub/wwwroot/apps/paradigma/nt/header_2.map&gid=6
            Dim geodatenroot, url As String

            mapsize = breite & "+" & hoehe

            Mapfile = calcMapfileName(hgrund, ndeinzeln)

            Dim radius As Integer
            'If mset.aktrange.xdif < 1 Then
            Dim pp As New myPoint
            pp.X = nd.rechts
            pp.Y = nd.hoch
            radius = 200

            Dim xl, xh, yl, yh, puffer As Integer
            puffer = 0
            xl = CInt(aktrange.xl) - puffer
            xh = CInt(aktrange.xh) + puffer
            yl = CInt(aktrange.yl) - puffer
            yh = CInt(aktrange.yh) + puffer

            geodatenroot = mset.serverWeb & "/cgi-bin/mapserv722/mapserv.cgi?mode=map&mapsize=" & mapsize

            url = geodatenroot & "&mapext=" & (xl & "+" & (yl) & "+" & (xh) & "+" & (yh))
            'url = url & "&map=/inetpub/wwwroot/apps/paradigma/ndman" & Mapfile
            url = url & "&map=/nkat/vorlage/paradigma/ndman" & Mapfile
            url = url & "" & gid
            url = url & "&ts=" & clsString.getTimestamp
            l(url)
            l(" genPreview ---------------------- ende")
            Return url
            'Else
            '    '
            'End If
        Catch ex As Exception
            l("Fehler in genPreview: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function calcMapfileName(hgrund As String, ndeinzeln As Boolean) As String
        Dim Mapfile As String
        Dim hgrundstring As String = ""
        Dim vgrundstring As String = ""
        If hgrund = "flurkarte" Then
            hgrundstring = "flurkarte"
        End If
        If hgrund = "stadtplan" Then
            hgrundstring = "stadtplan"
        End If
        If hgrund = "luftbild" Then
            hgrundstring = "luftbild"
        End If
        If ndeinzeln Then
            vgrundstring = "ndeinzel"
        Else
            vgrundstring = "ndalle"
        End If
        Mapfile = "/" & vgrundstring & "und" & hgrundstring & ".map"
        Return Mapfile
    End Function
End Class
