Imports System.Data
Imports mgis

Module modEW
    Private STR_Cachealb As String = "/cache/alb/"
    Private INT_Wartezeit As Integer = 40000
    Private ALBserverUNC As String = myglobalz.serverUNC

    'cache = "/cache/alb/"
    'Wartezeit = "8000"
    Function getEigentuemerDatei(params As String()) As String
        Dim datum, namensteil2, exeKutablePfad, Parameter, summa As String
        Dim mywebpdf = String.Format("eigentuemer_alkis_{0}_.rtf", GisUser.nick)
        Dim hinweis As String = ""
        Dim dateisystemPDF As String = GisUser.nick
        'If aktvorgangsid = "" Then
        '    aktvorgangsid = "9609"
        'End If

        datum = Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)

        namensteil2 = datum & "_" & aktvorgangsid.Trim & "_" & params(3).Trim & "_"
        filename_Festlegen(namensteil2, mywebpdf, dateisystemPDF, GisUser.nick)


        nachricht("dateisystemPDF :" & dateisystemPDF)
        If IO.File.Exists(dateisystemPDF) Then IO.File.Delete(dateisystemPDF)

        exeKutablePfad = strGlobals.ALBnas2TestExe

        Parameter = GetParameter_Eigentuemer(mywebpdf, namensteil2, (aktvorgangsid), params)
        summa = exeKutablePfad & Parameter
        nachricht("shill: " & summa)
        'summa = meineHttpNet.meinHttpJob("", summa, hinweis, myglobalz.enc, 5000)
        shellenUndTryen(dateisystemPDF, summa)
        nachricht(hinweis)
        Return dateisystemPDF
    End Function
    Private Function shellenUndTryen(dateisystemPDF As String, summa As String) As Boolean
        Try
            l("shellenUndTryen------------------------")
            l("summa " & summa)
            l("summadateisystemPDF " & dateisystemPDF)
            nachricht("pid:" & Microsoft.VisualBasic.Shell(summa).ToString)
            System.Threading.Thread.Sleep(INT_Wartezeit)
            If IO.File.Exists(dateisystemPDF) Then
                nachricht("1 versuch ")
                Return True
            Else
                System.Threading.Thread.Sleep(INT_Wartezeit)
                If IO.File.Exists(dateisystemPDF) Then
                    nachricht("2 versuch erfolgreich")
                    Return True
                Else
                    System.Threading.Thread.Sleep(INT_Wartezeit)
                    If IO.File.Exists(dateisystemPDF) Then
                        nachricht("2 versuch erfolgreich")
                        Return True
                    Else
                        l("RTF-Datei konnte nicht erzeugt werden 1")
                        MsgBox("RTF-Datei konnte nicht erzeugt werden 1")
                        Return False
                    End If
                    l("RTF-Datei konnte nicht erzeugt werden 2")
                    MsgBox("RTF-Datei konnte nicht erzeugt werden 2")
                    Return False
                End If
            End If

        Catch ex As Exception
            nachricht("Fehler in send_Shellbatch ", ex)
            Return False
        End Try
    End Function
    Private Function GetParameter_Eigentuemer(ByVal mywebpdf$, ByVal namensteil2$, ByVal vorgangid As String, specparms As String()) As String
        Dim Parameter$ = " mastermodus=batch "
        Try
            Parameter &= "username=" & GisUser.nick & " "
            Parameter &= "password=" & "2483e14219cce6fe63d8ac91afc92618" & " "
            '  Parameter &= "password=" & "2" & " "
            Parameter &= "FS=" & specparms(3).Trim & " "
            Parameter &= "FSGML=" & specparms(4).Trim & " "
            Parameter &= "WEISTAUF=" & specparms(5).Trim & " "
            Parameter &= "ZEIGTAUF=" & specparms(6).Trim & " "
            Parameter &= "GEBUCHT=" & specparms(7).Trim & " "
            Parameter &= "AREAQM=" & specparms(8).Trim & " "
            Parameter &= "TEIL2=" & namensteil2$ & " "
            Parameter &= "AZ=" & vorgangid & " "
            Parameter &= "AUSGABEMODUS=" & "rtf" & " "
            Return Parameter
        Catch ex As Exception
            nachricht("Fehler: GetParameter_Eigentuemer: ", ex)
            Return "fehler GetParameter_Eigentuemer"
        End Try
    End Function

    Private Function filename_Festlegen(ByVal namensteil2 As String,
                                    ByRef filenameImWebCache As String,
                                    ByRef filenameImLokalenCache As String,
                                    ByVal username As String) As Boolean
        filenameImLokalenCache = username & namensteil2 & ".rtf"
        filenameImWebCache = STR_Cachealb & filenameImLokalenCache.Replace("\", "/")
        Dim mastermodus$ = "batch"
        If mastermodus.ToLower = "batch" Then
            filenameImLokalenCache = ALBserverUNC & STR_Cachealb & filenameImLokalenCache
        Else
            filenameImLokalenCache = "d:" & STR_Cachealb & filenameImLokalenCache
        End If
        filenameImLokalenCache = filenameImLokalenCache.Replace("/", "\")

        nachricht("filenameImLokalenCache: " & filenameImLokalenCache)
        nachricht("filenameImWebCache: " & filenameImWebCache)
        Return True
    End Function

    Sub Protokollausgabe_aller_Parameter(flurstueck As String, grund As String)
        Try
            Dim sw As New IO.StreamWriter(eigentuemer_protokoll, True)
            sw.WriteLine(Now & "#" & GisUser.nick & "#" & clsActiveDir.fdkurz & "#" & "DESKTOP" & "#" & grund & "#" & flurstueck & "#" & "#" & "#" & "#" & "#")
            sw.Close()
            sw.Dispose()
        Catch ex As Exception
            'sw.WriteLine("Fehler in kontzrollausgabe:" ,ex)
        End Try
    End Sub

    Friend Function getRidVid4ObjId(akthost As String, aktSchema As String, aktTabelle As String, objID As Integer,
                                    ByRef rid As Integer, ByRef vid As Integer) As Boolean
        Dim basisrec As New clsDBspecPG
        Dim hinweis As String = ""
        Try
            l("getRidVid4ObjId-------------------------------                            ")
            'basisrec.mydb = CType(fstREC.mydb.Clone, clsDatenbankZugriff)
            basisrec.mydb.SQL = "SELECT raumbezugsid,vid FROM " & aktSchema & "." & aktTabelle & " where gid=" & objID
            l(basisrec.mydb.SQL)
            hinweis = basisrec.getDataDT()
            If basisrec.dt.Rows.Count < 1 Then
                Return False
            Else
                rid = CInt(clsDBtools.fieldvalue(basisrec.dt.Rows(0).Item(0)))
                vid = CInt(clsDBtools.fieldvalue(basisrec.dt.Rows(0).Item(1)))
                Return True
            End If
        Catch ex As Exception
            l("fehler in getRidVid4ObjId: ", ex)
            Return False
        End Try
    End Function

    Friend Function killRidVidinPG(akthost As String, aktSchema As String, aktTabelle As String, objID As Integer, rid As Integer, vid As Integer) As Boolean
        Dim basisrec As New clsDBspecPG
        Dim hinweis As String = ""
        Try
            l("killRidVidinPG                         ")
            'basisrec.mydb = CType(fstREC.mydb.Clone, clsDatenbankZugriff)
            basisrec.mydb.SQL = "delete FROM " & aktSchema & "." & aktTabelle.ToLower & " where gid=" & objID
            l(basisrec.mydb.SQL)
            hinweis = basisrec.getDataDT()
            If basisrec.dt.Rows.Count < 1 Then
                Return False
            Else
                rid = CInt(clsDBtools.fieldvalue(basisrec.dt.Rows(0).Item(0)))
                vid = CInt(clsDBtools.fieldvalue(basisrec.dt.Rows(0).Item(1)))
                Return True
            End If
        Catch ex As Exception
            l("fehler in killRidVidinPG: ", ex)
            Return False
        End Try
    End Function
    'Friend Function holePUFFERPolygonFuerPolygon(mypoly As String, pufferinMeter As Double) As String
    '    'endcap=flat join=mitre' muss weggelassen werden, sonst ist polygonempty
    '    Dim Sql As String
    '    Try
    '        Sql = "SELECt ST_AsText(SetSRID(ST_Buffer(ST_GeomFromText('" & mypoly & "', " & PostgisDBcoordinatensystem & ") ," &
    '            pufferinMeter.ToString.Replace(",", ".") & ")," & PostgisDBcoordinatensystem & "))"
    '        If iminternet Or CGIstattDBzugriff Then
    '            Dim result As String = "", hinweis As String = ""
    '            result = clsToolsAllg.getSQL4Http(Sql, "postgis20", hinweis, "getsql") : l(hinweis)
    '            result = result.Replace("$", "").Replace(vbCrLf, "")
    '            Return (result.Trim)
    '        Else
    '            Dim dt As DataTable
    '            dt = getDTFromWebgisDB(Sql, "postgis20")
    '            'Dim hinweis As String = fstREC.getDataDT()
    '            Return (clsDBtools.fieldvalue(clsDBtools.fieldvalue(dt.Rows(0).Item(0))))
    '        End If
    '    Catch ex As Exception
    '        nachricht("Fehler in holePUFFERPolygonFuerPoint: " ,ex)
    '        Return ""
    '    End Try
    'End Function

    'Function holeKoordinatenFuerUmkreis(aktPolygon As String) As String
    '    fstREC.mydb.SQL = "SELECT ST_EXTENT(ST_GeomFromText('" & aktPolygon & "', " & PostgisDBcoordinatensystem & ")) "
    '    Try
    '        Dim hinweis As String = fstREC.getDataDT()
    '        Return clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item(0))
    '    Catch ex As Exception
    '        l("Fehler in holeKoordinatenFuerUmkreis: ", ex)
    '        Return ""
    '    End Try
    'End Function

    'Function holeAreaFuerUmkreis(aktPolygon As String) As Double
    '    Dim SQL As String
    '    Try
    '        l("holeAreaFuerUmkreis")
    '        SQL = "SELECT ST_AREA(ST_GeomFromText('" & aktPolygon & "'," & PostgisDBcoordinatensystem & "))"
    '        If iminternet Or CGIstattDBzugriff Then
    '            Dim result, hinweis As String
    '            result = clsToolsAllg.getSQL4Http(SQL, "postgis20", hinweis, "getsql") : l(hinweis)
    '            result = result.Replace("$", "").Replace(vbCrLf, "")
    '            Return CDbl(result.Trim)
    '        Else
    '            Dim dt As DataTable
    '            dt = getDTFromWebgisDB(SQL, "postgis20")
    '            Return CDbl(clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
    '        End If

    '    Catch ex As Exception
    '        l("Fehler in holeAreaFuerUmkreis: ", ex)
    '        Return -1
    '    End Try
    'End Function

    'Public Function holePUFFERPolygonFuerGID(aktGID As Integer, aktTabelle As String, aktSchema As String, pufferinmeter As Double, Optional ByVal fromview As Boolean = True) As String
    '    Try
    '        fstREC.mydb.SQL = "SELECT ST_AsText(SetSRID(ST_Buffer(geom," & pufferinmeter.ToString.Replace(",", ".") &
    '                ",'endcap=flat join=mitre')," & PostgisDBcoordinatensystem & "))  FROM " &
    '                aktSchema & "." & aktTabelle & " where gid=" & aktGID
    '        Dim hinweis As String = fstREC.getDataDT()
    '        Return clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item(0))
    '    Catch ex As Exception
    '        l("Fehler in holePUFFERPolygonFuerGID: ", ex)
    '        Return ""
    '    End Try
    'End Function

    Public Function holePUFFERPolygonFuerLinktab(aktGID As Integer, aktTabelle As String, aktSchema As String, pufferinmeter As Double, linktab As String,
                                                 Optional ByVal fromview As Boolean = True) As String
        Dim SQL As String
        Dim result As String = "", hinweis As String = ""
        Try
            'If linktab.IsNothingOrEmpty Then
            linktab = "gid"
            If aktGID < 1 Then Return ""
            'End If
            SQL = "SELECT ST_AsText(ST_SetSRID(ST_Buffer(geom," & pufferinmeter.ToString.Replace(",", ".") &
                    ",'endcap=flat join=mitre')," & PostgisDBcoordinatensystem & "))  FROM " &
                    aktSchema & "." & aktTabelle & " where " & linktab & "=" & aktGID
            If iminternet Or CGIstattDBzugriff Then
                result = clsToolsAllg.getSQL4Http(SQL, "postgis20", hinweis, "getsql") : l(hinweis)
                result = result.Replace("$", "").Replace(vbCrLf, "")
                Return result.Trim
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(SQL, "postgis20")
                Return clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            End If
        Catch ex As Exception
            l("Fehler in holePUFFERPolygonFuerGID: ", ex)
            Return ""
        End Try
    End Function

    Function holePUFFERPolygonFuerPoint(myPoint As myPoint, pufferinMeter As Double) As String
        'endcap=flat join=mitre' muss weggelassen werden, sonst ist polygonempty
        Dim SQL = "SELECt ST_AsText(ST_SetSRID(ST_Buffer(ST_MakePoint(" & CStr(myPoint.X).Replace(",", ".") &
                "," & CStr(myPoint.Y).Replace(",", ".") &
                ")," & pufferinMeter.ToString.Replace(",", ".") & ")," & PostgisDBcoordinatensystem & "))"
        Try
            If iminternet Or CGIstattDBzugriff Then
                Dim result As String = "", hinweis As String = ""
                result = clsToolsAllg.getSQL4Http(SQL, "postgis20", hinweis, "getsql") : l(hinweis)
                result = result.Replace("$", "").Replace(vbCrLf, "")
                Return (result.Trim)
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(SQL, "postgis20")
                Return (clsDBtools.fieldvalue(clsDBtools.fieldvalue(dt.Rows(0).Item(0))))
            End If
        Catch ex As Exception
            l("Fehler in holePUFFERPolygonFuerPoint: ", ex)
            Return ""
        End Try
    End Function
    Function holeExtentFuerGID(aktGID As Integer, aktTabelle As String, aktSchema As String,
                               Optional ByVal fromview As Boolean = True) As String
        Dim result As String = "", hinweis As String = ""
        Dim SQL = "SELECT ST_EXTENT(geom) FROM " & aktSchema & "." & aktTabelle & " where " & " gid " & "=" & aktGID
        Try
            If iminternet Or CGIstattDBzugriff Then
                result = clsToolsAllg.getSQL4Http(SQL, "postgis20", hinweis, "getsql") : l(hinweis)
                result = result.Replace("$", "").Replace(vbCrLf, "")
                Return result
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(SQL, "postgis20")
                Return clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            End If

        Catch ex As Exception
            l("Fehler in holeKoordinatenFuerGID: ", ex)
            Return ""
        End Try
    End Function
    'Function holeExtentFuerLinktab(aktGID As Integer, aktTabelle As String, aktSchema As String, linktab As String,
    '                           Optional ByVal fromview As Boolean = True) As String
    '    'fstREC.mydb.SQL = "SELECT ST_EXTENT(geom) FROM " & aktSchema & "." & aktTabelle & " where gid=" & aktGID
    '    If linktab.IsNothingOrEmpty Then linktab = "gid"
    '    fstREC.mydb.SQL = "SELECT ST_EXTENT(geom) FROM " & aktSchema & "." & aktTabelle & " where " & linktab.Trim & "=" & aktGID
    '    Try
    '        Dim hinweis As String = fstREC.getDataDT()
    '        Return clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item(0))
    '    Catch ex As Exception
    '        l("Fehler in holeKoordinatenFuerGID: ", ex)
    '        Return ""
    '    End Try
    'End Function

    Public Function holeAreaFuerGID(aktGID As Integer, aktTabelle As String, aktSchema As String, Optional ByVal fromview As Boolean = True) As Double
        Dim SQL = "SELECt st_area(geom)  FROM " & aktSchema & "." & aktTabelle & " where gid=" & aktGID
        Dim result As String = "", hinweis As String = ""
        Try
            If iminternet Or CGIstattDBzugriff Then
                result = clsToolsAllg.getSQL4Http(SQL, "postgis20", hinweis, "getsql") : l(hinweis)
                result = result.Replace("$", "").Replace(vbCrLf, "")
                Return CDbl(result)
            Else
                Dim dt As DataTable
                dt = getDTFromWebgisDB(SQL, "postgis20")
                Return CDbl(clsDBtools.fieldvalue(clsDBtools.fieldvalue(dt.Rows(0).Item(0))))
            End If
        Catch ex As Exception
            l("Fehler in holeAreaFuerGID: ", ex)
            Return -1
        End Try
    End Function
    Friend Function bildePufferFuerPolygon(ByRef clsParapolygon As clsParapolygon, pufferinMeter As Double,
                                           Fdat As clsTabellenDef,
                                           ByRef puffer_area As Double, ByRef acanvas As clsRange,
                                           take_os_view As Boolean) As Boolean
        Dim anychange As Boolean = False
        Dim aktarea As Double
        'aktGID = clsPostgis.holeGID4Fs(akttabelle, aktschema, FS)
        l("Fdat.tabelle, " & Fdat.tabelle)
        l("Fdat.gid, " & Fdat.gid)
        l("Fdat.Schema, " & Fdat.Schema)
        Try
            '    Dim acanvas As New clsRange
            clsParapolygon = New clsParapolygon
            If take_os_view Then
                acanvas.BBOX = holeExtentFuerGID(CInt(Fdat.gid), Fdat.tabelle, Fdat.Schema, False)
                'acanvas.BBOX = holeExtentFuerLinktab(CInt(Fdat.gid), Fdat.os_tabellen_name, Fdat.Schema, Fdat.linkTabs, False)
            Else
                acanvas.BBOX = holeExtentFuerGID(CInt(Fdat.gid), Fdat.tabelle, Fdat.Schema, False)
            End If

            Fdat.geomtype = clsPolygonVerschn.getWktGeomTyp(acanvas.BBOX)

            Select Case Fdat.geomtype
                Case "point"
                    nachricht("USERAKTION: bildePufferFuerPolygon ")
                    acanvas.bbox_split()
                    'acanvas.CalcCenter()
                    Dim npoint As New myPoint
                    npoint.X = acanvas.xcenter
                    npoint.Y = acanvas.ycenter
                    clsParapolygon.ShapeSerial = holePUFFERPolygonFuerPoint(npoint, 30) 'pufferinMeter)
                    clsParapolygon.WKTstring = clsParapolygon.ShapeSerial
                    'clsParapolygon.ShapeSerial = holePUFFERPolygonFuerGID(CInt(Fdat.gid), Fdat.tabelle, Fdat.Schema, pufferinMeter, False)
                Case "polygon"
                    clsParapolygon.ShapeSerial = holePUFFERPolygonFuerLinktab(CInt(Fdat.gid), Fdat.tabelle, Fdat.Schema, pufferinMeter, "gid", False)
                    clsParapolygon.WKTstring = clsParapolygon.ShapeSerial
                    aktarea = holeAreaFuerGID(CInt(Fdat.gid), Fdat.tabelle, Fdat.Schema, False)
                    clsParapolygon.FlaecheQm = aktarea
                    clsParapolygon.Typ = RaumbezugsTyp.Polygon
                Case "polyline"
                    clsParapolygon.ShapeSerial = holePUFFERPolygonFuerLinktab(CInt(Fdat.gid), Fdat.tabelle, Fdat.Schema, pufferinMeter, "gid", False)
                    clsParapolygon.WKTstring = clsParapolygon.ShapeSerial
                    aktarea = 0 'holeAreaFuerGID(CInt(Fdat.gid), Fdat.tabelle, Fdat.Schema, False)
                    clsParapolygon.Typ = RaumbezugsTyp.Polyline
            End Select
            clsParapolygon.originalQuellString = clsParapolygon.ShapeSerial

            If clsParapolygon.ShapeSerial.IsNothingOrEmpty Then
                MsgBox("Puffer für Polygon konnte nicht berechnet werden")
                anychange = False
            Else
                puffer_area = clsPolygonVerschn.holeAreaFuerWKT(clsParapolygon.WKTstring)
                clsParapolygon.myPoly = New Polygon
                clsParapolygon.myPoly.Name = "myPoly"
                '   anychange = NSpostgis.clsPostgis.ObjektAlsRaumbezugspeichern(aktPuffer, aktarea, aktBOX, "Puffer [m]: " & pufferinMeter)
                anychange = True
            End If
            Return anychange
        Catch ex As Exception
            nachricht("fehler in bildePufferFuerPunkt: ", ex)
            Return False
        End Try
    End Function

    'Private Function holeKoordinatenFuerGeom(geom As String, v As Boolean) As String
    '    fstREC.mydb.SQL = "SELECT ST_EXTENT('" & geom & "') FROM foo"
    '    Try
    '        Dim hinweis As String = fstREC.getDataDT()
    '        Return clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item(0))
    '    Catch ex As Exception
    '        l("Fehler in holeKoordinatenFuerGID: ", ex)
    '        Return ""
    '    End Try
    'End Function

    'End Function
End Module
