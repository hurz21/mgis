Imports System.Data
'keine verweise auf oracle-dll
Imports mgis

Module modParadigma
    Friend Function getuserlayeraid(userName As String) As Integer
        Try
            l("getuserlayeraid userName; postgis " & userName)
            Dim dt As DataTable
            Dim schema As String = If(iminternet, "externparadigma", "public")
            dt = getDTFromWebgisDB("select userlayeraid  from " & schema & ".nutzer where lower(name)='" & userName.Trim.ToLower & "'", "webgiscontrol")

            l("getuserlayeraid ende " & clsDBtools.fieldvalue(dt.Rows(0).Item(0)).ToString())
            Return CInt(clsDBtools.fieldvalue(dt.Rows(0).Item(0)).ToString())
        Catch ex As Exception
            l("fehler in getuserlayeraid: ", ex)
            Return -1
        End Try
    End Function



    Private Function pointCollNachWkt(rbtyp As RaumbezugsTyp, pc As PointCollection) As String
        '"POLYGON((486005.601 5545785.497,486051.016 5545808.79,486056.814 5545811.749,486042.067 5545850.171,485973.674 5545830.973,486005.601 5545785.497))"
        Dim sb As New Text.StringBuilder
        Dim xc, yc As String
        Try
            If rbtyp = RaumbezugsTyp.Polygon Then
                sb.Append("POLYGON((")
            End If
            If rbtyp = RaumbezugsTyp.Polyline Then
                sb.Append("LINESTRING(")
            End If
            If pc Is Nothing OrElse pc.Count < 1 Then
                l("fehler zuwenig punkte")
                Return ""
            End If
            For i = 0 To pc.Count - 1
                xc = (pc.Item(i).X).ToString.Replace(",", ".")
                yc = (pc.Item(i).Y).ToString.Replace(",", ".")
                If i = 0 Then
                    sb.Append(xc & " " & yc)
                Else
                    sb.Append("," & xc & " " & yc)
                End If
            Next
            If rbtyp = RaumbezugsTyp.Polygon Then
                sb.Append("))")
            End If
            If rbtyp = RaumbezugsTyp.Polyline Then
                sb.Append(")")
            End If

            Return sb.ToString
        Catch ex As Exception
            l("fehler in pointCollNachWkt ", ex)
            Return ""
        End Try
    End Function

    Private Function bildePointColl(
                                    aktPolygon As clsParapolygon) As PointCollection
        Dim apoint As New Point
        Dim lokpc As New PointCollection
        Dim a() As String
        Try
            Select Case aktPolygon.Typ
                Case RaumbezugsTyp.Polygon
                    a = aktPolygon.GKstring.Split(";"c)
                    For i = 0 To a.Count - 1 Step 2
                        If a(i) = String.Empty Then Continue For
                        apoint = New Point
                        apoint.X = CDbl(a(i))
                        apoint.Y = CDbl(a(i + 1))
                        lokpc.Add(apoint)
                    Next
                Case RaumbezugsTyp.Polyline
                    a = aktPolygon.GKstring.Split(";"c)
                    For i = 0 To a.Count - 1 Step 2
                        If a(i) = String.Empty Then Continue For
                        apoint = New Point
                        apoint.X = CDbl(a(i))
                        apoint.Y = CDbl(a(i + 1))
                        lokpc.Add(apoint)
                    Next
            End Select
            Return lokpc
        Catch ex As Exception
            l("fehler in bildePointColl ", ex)
            Return Nothing
        End Try
    End Function
    Private Function darfVorgangBearbeiten(userName As String, aktvorgangsid As String) As Boolean
        l("darfVorgangBearbeiten-------------------------")
        Dim hinweis As String = ""
        Dim darf As Boolean
        Try
            Dim bearbstring As String
            Dim dt As DataTable
            Dim sql As String = "select bearbeiter,weiterebearb from t41 where vorgangsid=" & aktvorgangsid
            'dt = modParadigma.getDTFromParadigmaDB(, paradigmaDBTyp)
            dt = modgetdt4sql.getDT4Query(sql, paradigmaMsql, hinweis)
            bearbstring = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item(0))) & "," & CStr(clsDBtools.fieldvalue(dt.Rows(0).Item(1)))
            bearbstring = bearbstring.ToLower
            dt = Nothing
            darf = modParadigma.darfBearbeiten(userName, aktvorgangsid, bearbstring)
            If darf Then
                l("darfVorgangBearbeiten darf")
            Else
                l("darfVorgangBearbeiten darf nicht")
            End If
            Return darf
        Catch ex As Exception
            l("fehler in darfVorgangBearbeiten ", ex)
            Return False
        End Try
    End Function
    Public Function darfBearbeiten(ByVal user As String,
                                   ByRef vid As String, bearbstring As String) As Boolean

        If bearbstring <> "" Then
            Dim darf As Boolean
            darf = UsernamePasstZuBearbeiterString(user, bearbstring)
            If Not darf Then
                darf = aktUserIstParadigmaAdmin(user)
            End If
            Return darf
        Else
            Return False
        End If
    End Function

    Function UsernamePasstZuBearbeiterString(user As String, bearbstring As String) As Boolean
        Try
            l("in UsernamePasstZuBearbeiterString ")
            Dim dt As DataTable
            Dim sql As String = "", hinweis As String = ""
            sql = "select initial_ from " & strGlobals.tabbearbeiter & " where lower(username)='" & user.ToLower & "'"
            dt = modgetdt4sql.getDT4Query(sql, paradigmaMsql, hinweis)
            ' dt = modParadigma.getDTFromParadigmaDB(, paradigmaDBTyp)
            If dt.Rows.Count < 1 Then
                Return False
            End If
            l("initial: " & CStr(clsDBtools.fieldvalue(dt.Rows(0).Item(0))))
            If bearbstring.Contains(CStr(clsDBtools.fieldvalue(dt.Rows(0).Item(0))).ToLower) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            nachricht("fehler in UsernamePasstZuBearbeiterString:", ex)
            Return False
        End Try
    End Function
    Public Sub generateAndSaveSerialShapeInDb()
        nachricht("generateAndSaveSerialShapeInDb---------------------------------------------")
        Dim rumpf As String = ""
        Dim hinweis As String = ""
        Try
            'http://w2gis02.kreis-of.local/cgi-bin/apps/paradigmaex/serialserver/pg/serialserver.cgi?user=feinen_j&vid=9609&rid=57318&gemcode=729&FS=FS0607290050049000000&postgis=1
            rumpf = URLserialserver
            rumpf &= GisUser.nick
            rumpf &= "&vid=" & aktvorgangsid
            rumpf &= "&rid=" & CInt(aktFST.RaumbezugsID)
            rumpf &= "&gemcode=" & aktFST.normflst.gemcode
            rumpf &= "&FS=" & aktFST.normflst.FS
            rumpf &= "&postgis=1"
            nachricht("url: " & rumpf)
            meineHttpNet.meinHttpJob(ProxyString, rumpf, hinweis, myglobalz.enc, 0)
            l(hinweis)
            '    Dim result As String = meineHttpNet.meinHttpJob(ProxyString, aufruf, hinweis, myglobalz.enc, 5000)
        Catch ex As Exception
            nachricht("fehler in: generateAndSaveSerialShapeInDb: ", ex)
        End Try
    End Sub


    Function aktUserIstParadigmaAdmin(user As String) As Boolean
        Try
            l("aktUserIstParadigmaAdmin ---------------------- anfang")
            For i = 0 To paradigmaAdmins.Count - 1
                If user.ToLower.Trim = paradigmaAdmins(i).ToLower.Trim Then
                    Return True
                End If
            Next
            Return False
            l("aktUserIstParadigmaAdmin---------------------- ende")
        Catch ex As Exception
            l("Fehler in aktUserIstParadigmaAdmin: " & ex.ToString())
            Return False
        End Try
    End Function
    Friend Function GeometrieNachParadigma(aktPolygon As clsParapolygon, aktPolyline As clsParapolyline) As Boolean
        Dim pc As New PointCollection
        Dim koppelid, polygonid As Integer
        Try
            If aktPolygon.ShapeSerial.IsNothingOrEmpty Then
                pc = bildePointColl(aktPolygon)
                aktPolygon.ShapeSerial = pointCollNachWkt(aktPolygon.Typ, pc)
                aktPolygon = setRBtyp(aktPolygon.Typ, aktPolygon)
                aktPolygon.FlaecheQm = CDbl(aktPolygon.Area)
            End If

            aktPolygon.LaengeM = CDbl(aktPolygon.LaengeM)
            ' If modParadigma.darfVorgangBearbeiten(GisUser.nick, aktvorgangsid) Then
            aktPolygon = PolygonObjVorbereiten(aktPolygon)

                aktPolygon.RaumbezugsID = modParadigma.Raumbezug_abspeichern_Neu_alleDB(aktPolygon)
                'aktPolygon.RaumbezugsID = pradigmaDB.Raumbezug_abspeichern_Neu_alleDBORACLE(aktPolygon)
                If aktPolygon.RaumbezugsID > 0 Then
                    'koppelid = pradigmaDB.Koppelung_Raumbezug_VorgangOracle(CInt(aktPolygon.RaumbezugsID), CInt(aktvorgangsid), 0)
                    koppelid = modParadigma.Koppelung_Raumbezug_Vorgang(CInt(aktPolygon.RaumbezugsID), CInt(aktvorgangsid), 0)
                    If koppelid > 0 Then
                        'polygonid = pradigmaDB.RB_FLST_Serial_abspeichern_NeuORACLE(CInt(aktvorgangsid),
                        '                                                CInt(aktPolygon.RaumbezugsID),
                        '                                                aktPolygon.ShapeSerial,
                        '                                                aktPolygon.Typ,
                        '                                                aktPolygon.Area)
                        polygonid = modParadigma.RB_FLST_Serial_abspeichern_Neu(CInt(aktvorgangsid),
                                                                        CInt(aktPolygon.RaumbezugsID),
                                                                        aktPolygon.ShapeSerial,
                                                                        aktPolygon.Typ,
                                                                        aktPolygon.Area)
                        If polygonid > 0 Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        'koppelid hat nicht geklappt
                        Return False
                    End If
                Else
                    'raumbezuganlegen hat nicht geklappt
                    Return False
                End If

                nachricht("PolygonNeuSpeichern: raumbezugsID%: " & aktPolygon.RaumbezugsID)
            'Else
            '    MsgBox("Der Vorgang ist für Sie nicht editierbar!")
            '    Return False
            'End If
        Catch ex As Exception
            l("fehler in GeometrieNachParadigma: ", ex)
            Return False
        End Try
    End Function

    Private Function RB_FLST_Serial_abspeichern_Neu(v1 As Integer, v2 As Integer, shapeSerial As String, typ As RaumbezugsTyp, area As Double) As Integer
        Return modSQLsTools.RB_FLST_Serial_abspeichern_Neusqls(CInt(aktvorgangsid),
                                                                       CInt(aktPolygon.RaumbezugsID),
                                                           aktPolygon.ShapeSerial,
                                                           aktPolygon.Typ,
                                                           aktPolygon.Area)
        'If paradigmaDBTyp = "oracle" Then
        '    Return clsParadigmaDBOracle.RB_FLST_Serial_abspeichern_NeuORACLE(CInt(aktvorgangsid),
        '                                                                CInt(aktPolygon.RaumbezugsID),
        '                                                    aktPolygon.ShapeSerial,
        '                                                    aktPolygon.Typ,
        '                                                    aktPolygon.Area)
        'End If
        'If paradigmaDBTyp = "sqls" Then
        '    Return modSQLsTools.RB_FLST_Serial_abspeichern_Neusqls(CInt(aktvorgangsid),
        '                                                               CInt(aktPolygon.RaumbezugsID),
        '                                                   aktPolygon.ShapeSerial,
        '                                                   aktPolygon.Typ,
        '                                                   aktPolygon.Area)
        'End If
    End Function

    Private Function setRBtyp(geometrietyp As RaumbezugsTyp, aktPolygon As clsParapolygon) As clsParapolygon
        If geometrietyp = RaumbezugsTyp.Polygon Then
            aktPolygon.Typ = RaumbezugsTyp.Polygon
        End If
        If geometrietyp = RaumbezugsTyp.Polyline Then
            aktPolygon.Typ = RaumbezugsTyp.Polyline
        End If
        Return aktPolygon
    End Function

    Private Function PolygonObjVorbereiten(aktPolygon As clsParapolygon) As clsParapolygon
        Dim pc As New PointCollection
        Try
            'standardwert ist polygon
            If aktPolygon.Typ = RaumbezugsTyp.unbekannt Then
                aktPolygon.Typ = RaumbezugsTyp.Polygon
            End If
            If aktPolygon.Typ = RaumbezugsTyp.Polygon Then
                aktPolygon.SekID = 0
                If aktPolygon.WKTstring.IsNothingOrEmpty Then
                    pc = bildePointColl(aktPolygon)
                    aktPolygon.WKTstring = pointCollNachWkt(aktPolygon.Typ, pc)
                    aktPolygon = setRBtyp(aktPolygon.Typ, aktPolygon)
                    aktPolygon.FlaecheQm = CDbl(aktPolygon.Area)
                End If
                ' aktPolygon.ShapeSerial = wktstring
                aktPolygon.box.BBOX = holeKoordinatenFuerUmkreis(aktPolygon.WKTstring, aktPolygon.Typ)
                aktPolygon.box.bbox_split()
                aktPolygon.defineAbstract()
                'aktPolygon.box.CalcCenter()
                aktPolygon.punkt.X = CInt(aktPolygon.box.xcenter)
                aktPolygon.punkt.Y = CInt(aktPolygon.box.ycenter)
                aktPolygon.name = "Polygon"
                aktPolygon.Freitext = "Polygon vom Gis am " & Format(Now, "yyyy-MM-dd mmss")
            End If
            If aktPolygon.Typ = RaumbezugsTyp.Polyline Then
                aktPolygon.SekID = 0
                ' aktPolygon.ShapeSerial = wktstring

                If aktPolygon.WKTstring.IsNothingOrEmpty Then
                    pc = bildePointColl(aktPolygon)
                    aktPolygon.WKTstring = pointCollNachWkt(aktPolygon.Typ, pc)
                    aktPolygon = setRBtyp(aktPolygon.Typ, aktPolygon)
                    aktPolygon.FlaecheQm = CDbl(aktPolygon.Area)
                End If

                aktPolygon.box.BBOX = holeKoordinatenFuerUmkreis(aktPolygon.WKTstring, aktPolygon.Typ)
                aktPolygon.box.bbox_split()
                aktPolygon.defineAbstract()
                'aktPolygon.box.CalcCenter()
                aktPolygon.punkt.X = CInt(aktPolygon.box.xcenter)
                aktPolygon.punkt.Y = CInt(aktPolygon.box.ycenter)
                aktPolygon.name = "Polyline"
                aktPolygon.Freitext = "PolyLine vom Gis am " & Format(Now, "yyyy-MM-dd mmss")
            End If
            Return aktPolygon
        Catch ex As Exception
            nachricht("Fehler in PolygonObjVorbereiten: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Function deleteRaumbezug2all(rid As Integer, vid As Integer, v As String) As Boolean
        Return modSQLsTools.deleteRaumbezug2allOraclesqls(rid, vid, "raumbezug2vorgang")

    End Function

    Friend Function deleteRaumbezug(rid As Integer, vid As Integer) As Boolean
        Return modSQLsTools.deleteRaumbezugSqls(rid, vid)
    End Function

    Friend Function punktNachParadigma(aktPoint As myPoint) As Boolean
        Dim aktPMU As New clsParaUmkreis
        Dim umkreisID, koppelid, raumbezugsID As Integer
        Try
            aktPMU.Status = 0
            aktPMU.Typ = RaumbezugsTyp.Umkreis
            aktPMU.mitEtikett = False
            aktPMU.Freitext = "Punkt vom GIS"
            aktPMU.Name = "vom Gis"
            aktPMU.Radius = 100
            aktPMU.punkt = aktPoint
            umkreisID = modParadigma.RB_Umkreis_abspeichern_Neu(aktPMU)
            If umkreisID < 1 Then Return False
            aktPMU.Typ = RaumbezugsTyp.Umkreis
            aktPMU.SekID = umkreisID
            aktPMU.defineAbstract()
            defineBBOX(aktPMU.Radius, aktPMU)
            raumbezugsID = modParadigma.Raumbezug_abspeichern_Neu_alleDB(aktPMU)
            aktPMU.RaumbezugsID = raumbezugsID
            koppelid = modParadigma.Koppelung_Raumbezug_Vorgang(CInt(aktPMU.RaumbezugsID), CInt(aktvorgangsid), 0)
            If koppelid > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            l("fehler in punktNachParadigma: ", ex)
            Return False
        End Try
    End Function

    Private Function RB_Umkreis_abspeichern_Neu(aktPMU As clsParaUmkreis) As Integer
        Return modSQLsTools.RB_Umkreis_abspeichern_Neusqls(aktPMU)
        'If paradigmaDBTyp = "oracle" Then
        '    Return clsParadigmaDBOracle.RB_Umkreis_abspeichern_NeuOracle(aktPMU)
        'End If
        'If paradigmaDBTyp = "sqls" Then
        '    Return modSQLsTools.RB_Umkreis_abspeichern_Neusqls(aktPMU)
        'End If
    End Function



    Sub defineBBOX(ByVal radius As Double, ByVal rb As iRaumbezug)
        With rb
            If rb.isMapEnabled Then
                .box.xl = .punkt.X - radius
                .box.xh = .punkt.X + radius
                .box.yl = .punkt.Y - radius
                .box.yh = .punkt.Y + radius
            Else
                .box.xl = 0
                .box.xh = 0
                .box.yl = 0
                .box.yh = 0
            End If
        End With
    End Sub







    Public Function getInitial(username As String) As String
        Dim name, vorname, a(), Initiale As String
        Try
            If String.IsNullOrEmpty(username) Then
                name = "???"
                vorname = "harvey"
            End If
            a = username.Split("_"c)
            name = a(0) : vorname = a(1)
            Initiale = (name.Substring(0, 3) & vorname.Substring(0, 1)).ToLower
            Return Initiale
        Catch ex As Exception
            l("fehler in getInitial", ex)
            Return "???"
        End Try
    End Function

    Function GetExtension(ByVal fi As IO.FileInfo) As String
        Dim extension As String
        Try
            extension = fi.Extension.Replace(".", "")
            If extension.IsNothingOrEmpty Then extension = "txt"
            Return extension
        Catch ex As Exception
            Return "err"
        End Try
    End Function

    Friend Function checkIN_FileArchiv(QuelleFullname As String,
                                       ByRef Archivname As String,
                                       ByRef erfolgreich As Boolean,
                                       ArchivSubdir As String,
                                       NEWSAVEMODE As Boolean,
                                       dokid As Integer,
                                       rootdir As String) As Boolean
        l("checkIN_FileArchiv: input  OriginalFullname: " & QuelleFullname)
        l("checkIN_FileArchiv: input  Archivname: " & Archivname)
        l("checkIN_FileArchiv: input  erfolgreich: " & erfolgreich)
        l("checkIN_FileArchiv: input  NEWSAVEMODE: " & NEWSAVEMODE)
        l("checkIN_FileArchiv: input  dokid: " & dokid)
        Dim result As MessageBoxResult
        Dim dokumentpfad As String
        Dim ZielGesamtpfad As String
        Dim ZielDateiFullName As String = ""
        Dim Fquell As IO.FileInfo = Nothing
        Dim Fziel As IO.FileInfo = Nothing
        dokumentpfad = ArchivSubdir
        ZielGesamtpfad = rootdir & dokumentpfad 'myGlobalz.Arc.rootDir
        erfolgreich = False
        erzeugeUnterverzeichnis(ZielGesamtpfad)
        nachricht("in checkIN_FileArchiv")
        Try
            Fquell = New IO.FileInfo(QuelleFullname)
            Dim normname As String = ""
            If NEWSAVEMODE Then
                ZielDateiFullName = ZielGesamtpfad & "\" & dokid
            Else
                normname = clsString.normalize_Filename(Fquell.Name)
                ZielDateiFullName = ZielGesamtpfad & "\" & normname
            End If


            Try
                IO.File.Copy(Fquell.FullName, ZielDateiFullName)
                erfolgreich = True
                nachricht("Kopieren ins Archiv: " & Fquell.FullName)
                Archivname = ZielDateiFullName
            Catch ex As Exception
                nachricht("FEhler 2Kopieren ins Archiv gescheitert!")
                Archivname = ""
            End Try
            'End If
            Fquell = Nothing
            Fziel = Nothing
            nachricht("checkIN_FileArchiv: output  Archivname: " & Archivname)
            nachricht("checkIN_FileArchiv: output  dokumentpfad: " & dokumentpfad)
            Return True 'ist unverändert: warum wird das wieder zurückgeliefert?
        Catch ex As Exception
            Fquell = Nothing
            nachricht("FEhler checkIN_FileArchiv: FEHLER  OriginalFullname: " & QuelleFullname)
            result = MessageBox.Show("Diese Datei existiert schon: " & QuelleFullname & vbCrLf & "Kopie anlegen ?",
             "Einchecken von Dokumenten", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
            If result = MessageBoxResult.Yes Then
                Try
                    ' IO.File.Copy(dateiname , dateiganz, True)
                Catch ex2 As Exception
                    nachricht("FEhler checkIN_FileArchiv: FEHLER2  OriginalFullname" & QuelleFullname)
                    MessageBox.Show("Fehler beim überschreiben. Die Datei wird ggf. von einem anderen Programm benutzt!" & vbCrLf & ex2.ToString)
                    Return False
                End Try
            End If
            Archivname = ""
            Return False
        End Try
    End Function

    Friend Function Koppelung_Raumbezug_Vorgang(rid As Integer, vid As Integer, v3 As Integer) As Integer
        Return modSQLsTools.Koppelung_Raumbezug_VorgangSqls(CInt(rid), CInt(vid), 0)
        'If paradigmaDBTyp = "oracle" Then
        '    Return clsParadigmaDBOracle.Koppelung_Raumbezug_VorgangOracle(CInt(rid), CInt(vid), 0)
        'End If
        'If paradigmaDBTyp = "sqls" Then
        '    Return modSQLsTools.Koppelung_Raumbezug_VorgangSqls(CInt(rid), CInt(vid), 0)
        'End If
    End Function

    Function RB_Adresse_abspeichern_Neu() As Integer
        'If paradigmaDBTyp = "oracle" Then
        '    Return clsParadigmaDBOracle.RB_Adresse_abspeichern_NeuOracle()
        'End If
        'If paradigmaDBTyp = "sqls" Then
        Return modSQLsTools.RB_Adresse_abspeichern_Neusqls()
        'End If
    End Function

    Friend Function RB_FLST_abspeichern_Neu() As Integer
        Return modSQLsTools.RB_FLST_abspeichern_Neusqls()
        'If paradigmaDBTyp = "oracle" Then
        '    Return clsParadigmaDBOracle.RB_FLST_abspeichern_NeuOracle()
        'End If
        'If paradigmaDBTyp = "sqls" Then
        '    Return modSQLsTools.RB_FLST_abspeichern_Neusqls()
        'End If
    End Function

    Function Paradigma_Adresse_Neu(radius As Integer) As Integer
        Dim adresseID As Integer
        Dim raumbezugsID As Integer
        Dim koppelungsID4 As Integer
        Try
            adresseID = modParadigma.RB_Adresse_abspeichern_Neu()

            If adresseID < 1 Then
                nachricht("Problem beim abspeichern (Paradigma_Adresse_Neu,adresseID)")
                Return 0
            End If

            aktadr.SekID = adresseID
            aktadr.defineAbstract()
            defineBBOX(radius, aktadr)
            aktadr.Typ = RaumbezugsTyp.Adresse
            raumbezugsID = modParadigma.Raumbezug_abspeichern_Neu_alleDB(aktadr)

            If raumbezugsID < 1 Then
                nachricht("Problem beim abspeichern (Paradigma_Adresse_Neu, raumbezugsID)")
                Return 0
            End If
            aktadr.RaumbezugsID = raumbezugsID
            'koppelungsID4 = Koppelung_Raumbezug_VorgangOracle(CInt(aktadr.RaumbezugsID), CInt(aktvorgangsid), 0)
            koppelungsID4 = modParadigma.Koppelung_Raumbezug_Vorgang(CInt(aktadr.RaumbezugsID), CInt(aktvorgangsid), 0)
            l(" Koppelung_Raumbezug_Vorgang:" & koppelungsID4% & " ")
            Return koppelungsID4

        Catch ex As Exception
            l("fehler in Paradigma_Adresse_Neu ", ex)
            Return -1
        End Try
    End Function

    Friend Function Raumbezug_abspeichern_Neu_alleDB(aktadr As iRaumbezug) As Integer
        'If paradigmaDBTyp = "oracle" Then
        '    Return clsParadigmaDBOracle.Raumbezug_abspeichern_Neu_alleDBORACLE(aktadr)
        'End If
        'If paradigmaDBTyp = "sqls" Then
        Return modSQLsTools.Raumbezug_abspeichern_Neu_alleDBsqls(aktadr)
        'End If
    End Function

    Function DokNachParadigma(dokFullname As String, vid As String, Beschreibung As String) As Boolean
        Dim neuDOKID As Integer
        Dim OriginalName, OriginalFullname As String
        Dim dateidatum As Date
        Dim oritest As IO.FileInfo
        Dim dt As DataTable
        Try
            l("DokNachParadigma---------------------- anfang")
            Dim sql As String = "", hinweis As String = ""
            sql = "select arcdir from t41 where vorgangsid=" & vid
            'dt = modParadigma.getDTFromParadigmaDB(, paradigmaDBTyp)
            dt = modgetdt4sql.getDT4Query(sql, paradigmaMsql, hinweis)

            PARADIGMA_Vorgangs_ArchivSubdir = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            erzeugeUnterverzeichnis(strGlobals.PARADIGMA_ARCHIV_rootdir & PARADIGMA_Vorgangs_ArchivSubdir)
            oritest = New IO.FileInfo(dokFullname)
            OriginalName = oritest.Name
            oritest = Nothing
            dateidatum = Now
            OriginalFullname = dokFullname
            neuDOKID = modParadigma.checkin_Dokumente(PARADIGMA_Vorgangs_ArchivSubdir, Beschreibung, OriginalFullname, OriginalName,
                                                      dateidatum, vid, 0, True)

            If neuDOKID > 0 Then
                Dim archivDateiFullname As String = ""
                Dim erfolgreich As Boolean = False
                erfolgreich = modParadigma.checkIN_FileArchiv(OriginalFullname, archivDateiFullname, erfolgreich,
                                                          PARADIGMA_Vorgangs_ArchivSubdir, True, neuDOKID,
                                                          strGlobals.PARADIGMA_ARCHIV_rootdir)
                If erfolgreich Then
                    Return True
                Else
                    l("fehler einchecken ins filearchiv fehlgeschlagen:")
                End If
            Else
                l("fehler neudokid wurde nicht erzeugt:")
            End If
            l("DokNachParadigma---------------------- ende")
            Return False
        Catch ex As Exception
            l("Fehler in DokNachParadigma: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Function checkin_Dokumente(pARADIGMA_Vorgangs_ArchivSubdir As String, beschreibung As String, originalFullname As String,
                                       originalName As String, dateidatum As Date, vid As String, v1 As Integer, v2 As Boolean) As Integer
        Dim neudokid As Integer
        'If paradigmaDBTyp = "oracle" Then
        '    neuDOKID = modOracleTools.checkin_DokumenteOracle(pARADIGMA_Vorgangs_ArchivSubdir, beschreibung, originalFullname, originalName,
        '                                        dateidatum, vid, 0, True)
        '    Return neudokid
        'End If
        'If paradigmaDBTyp = "sqls" Then
        '    neudokid = modSQLsTools.checkin_Dokumentesqls(pARADIGMA_Vorgangs_ArchivSubdir, beschreibung, originalFullname, originalName,
        '                                        dateidatum, vid, 0, True)
        '    Return neudokid
        'End If
        neudokid = modSQLsTools.checkin_DokumenteDB(pARADIGMA_Vorgangs_ArchivSubdir, beschreibung, originalFullname, originalName,
                                                dateidatum, vid, 0, True)
        Return neudokid
    End Function

    'Function getDTFromParadigmaDB(query As String, paradigmaDBTyp As String) As DataTable
    '    Dim dt As New DataTable
    '    Dim hinweis As String
    '    'If paradigmaDBTyp = "oracle" Then
    '    '    dt = modOracleTools.getDTFromParadigmaDBOracle(query)
    '    'End If
    '    'If paradigmaDBTyp = "sqls" Then
    '    '    dt = modSQLsTools.getDTFromParadigmaDBsqls(query)
    '    'End If
    '    'dt = modSQLsTools.getDTFromParadigmaDBsqls(query)
    '    dt = modgetdt4sql.getDT4Query(query, gisdokREC, hinweis)
    '    Return dt
    'End Function

    Function getParadigmaAbteilung4FDumwelt(user As String) As String
        l("getAbteilung4FDumwelt-------------------------" & GisUser.nick)
        Dim ParadigmaAbteilung As String = ""
        Dim sql As String = "", hinweis As String = ""

        If GisUser.nick = "hurz" Then
            l("GisUser.nick = hurz")
            ParadigmaAbteilung = "untere naturschutzbehörde"
            Return ParadigmaAbteilung
        End If
        Try
            Dim dt As DataTable
            sql = "select * from " & strGlobals.tabbearbeiter & " where lower(username)= '" & user.ToLower & "'"
            '   dt = modParadigma.getDTFromParadigmaDB(, paradigmaDBTyp)
            l(sql)
            dt = modgetdt4sql.getDT4Query(sql, paradigmaMsql, hinweis)
            If dt.Rows.Count > 0 Then
                ParadigmaAbteilung = CStr(clsDBtools.fieldvalue(dt.Rows(0).Item("abteilung")))
            Else
                ParadigmaAbteilung = ""
            End If
            dt = Nothing
            Return ParadigmaAbteilung
        Catch ex As Exception
            l("fehler in getAbteilung4FDumwelt " & sql, ex)
            Return ""
        End Try
    End Function

    Sub erzeugeUnterverzeichnis(ByVal relativpfad$)
        Try
            IO.Directory.CreateDirectory(relativpfad)
        Catch ex As Exception
            nachricht("Fehler in: erzeugeUnterverzeichnis: ", ex)
        End Try
    End Sub

    Friend Function getDTFromWebgisDB(queryString As String, Db As String) As DataTable
        l("getDTFromWebgisDB-------------------------" & Environment.NewLine &
        " webgisREC.mydb.Schema " & webgisREC.mydb.Schema & Environment.NewLine &
        "queryString : " & queryString)
        Try
#If DEBUG Then
            If iminternet Then
                Debug.Print("")
            End If
#End If
            webgisREC.mydb.Schema = Db
            webgisREC.mydb.SQL = queryString
            l(webgisREC.getDataDT())
            Return webgisREC.dt
        Catch ex As Exception
            l("fehler in getDTFromWebgisDB ", ex)
            Return Nothing
        End Try
    End Function

    Friend Function calcNewMaxRange(aktvorgangsid As String) As clsRange
        Dim newrange As New clsRange
        Dim hinweis As String = ""
        Dim querie As String = ""
        Try
            l("calcNewMaxRange---------------------- anfang" & aktvorgangsid)
            querie = "Select round(min(xmin),0) As xmin,round(max(xmax),0) As xmax,round(min(ymin),0) As ymin,round(max(ymax),0) As ymax  " &
                " from raumbezugundvorg where  xmin>0 And xmax>0 And ymin>0 And ymax>0 And ISMAPENABLED=1 And vorgangsid=" & aktvorgangsid
            Dim dt As DataTable
            '     dt = modParadigma.getDTFromParadigmaDB(querie, paradigmaDBTyp)

            dt = modgetdt4sql.getDT4Query(querie, paradigmaMsql, hinweis)
            l("calcNewMaxRange count: " & dt.Rows.Count)
            If dt.Rows.Count > 0 Then
                newrange.xl = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("xmin")))
                newrange.xh = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("xmax")))
                newrange.yl = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("ymin")))
                newrange.yh = CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item("ymax")))
            Else
                Return Nothing
            End If
            dt = Nothing

            l("calcNewMaxRange---------------------- ende")
            Return newrange
        Catch ex As Exception
            l("Fehler in calcNewMaxRange: " & ex.ToString() & " " & Environment.NewLine & querie)
            Return Nothing
        End Try
    End Function

    Function NeuesFSTspeichern(radius As Integer) As Boolean
        Dim sekid As Integer
        Dim koppelid As Integer
        l("NeuesFSTspeichern--------------")
        Try
            sekid = modParadigma.RB_FLST_abspeichern_Neu()
            If sekid < 1 Then
                l("sekid < 1")
                Return False
            End If
            aktFST.SekID = sekid
            aktFST.defineAbstract()
            If aktFST.punkt.X < 1 Then
                aktFST.punkt.X = aktFST.normflst.GKrechts
                aktFST.punkt.Y = aktFST.normflst.GKhoch
            End If


            defineBBOX(radius, aktFST)
            aktFST.coordsAbstract = aktFST.normflst.GKrechts & "," & aktFST.normflst.GKhoch
            Dim raumbezugsID As Integer = Raumbezug_abspeichern_Neu_alleDB(aktFST)
            '  Dim raumbezugsID% = DBraumbezug_Mysql.Raumbezug_abspeichern_Neu(myGlobalz.sitzung.aktFST)
            If raumbezugsID < 1 Then
                l("raumbezugsID < 1")
                Return False
            End If

            koppelid = Koppelung_Raumbezug_Vorgang(raumbezugsID, CInt(aktvorgangsid), 0)
            If koppelid < 1 Then
                l("koppelid < 1")
                Return False
            End If
            nachricht("kooplungsid: " & koppelid)
            aktFST.RaumbezugsID = raumbezugsID
            generateAndSaveSerialShapeInDb()
            Return True

        Catch ex As Exception
            l("Fehler in NeuesFSTspeichern ", ex)
            Return False
        End Try
    End Function
    Function emailAdresses4VID(vid As Integer) As List(Of String)
        Dim neu As New List(Of String)
        Try
            l("in emailAdresses4VID ")
            Dim dt As DataTable
            Dim sql As String = "", hinweis As String = ""
            sql = "select ffemail from beteiligte where vorgangsid=" & vid
            dt = modgetdt4sql.getDT4Query(sql, paradigmaMsql, hinweis)
            ' dt = modParadigma.getDTFromParadigmaDB(, paradigmaDBTyp)
            If dt.Rows.Count < 1 Then
                Return neu
            End If
            'Dim summe As New Text.StringBuilder
            For Each row As DataRow In dt.Rows
                neu.Add(row.Item(0).ToString)
            Next
            'l("result:" & summe.ToString)
            Return neu
        Catch ex As Exception
            nachricht("fehler in emailAdresses4VID:", ex)
            Return Nothing
        End Try
    End Function
End Module
