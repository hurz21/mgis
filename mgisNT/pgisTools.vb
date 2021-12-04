Imports System.Data
Imports mgis
Module pgisTools
    ' Public domainstring As String = serverweb" 'ServerHTTPdomainIntranet
    Public kartengen As New clsAufrufgenerator

    Public Function holePdf2SidDT(aid As Integer, sid As Integer) As DataTable
        Try
            l("---------------------- anfang")
            Dim SQL = "select * from  public.pdfdateien " &
            " where sid=" & sid & " or aid=" & aid &
                " order by titelspalte1 "

            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "webgiscontrol")

            Return dt
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return Nothing
        End Try

    End Function
    Public Function holeFlureDT() As DataTable
        Try
            l("holeFlureDT---------------------- anfang")
            Dim SQL = "select distinct flur  from  " & WinDetailSucheFST.AktuelleBasisTabelle & " where gemcode = " & aktFST.normflst.gemcode &
         " order by flur "
            'Dim dt As DataTable = clsWebgisPGtools.holeDTfromFKAT(SQL)
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            Return dt
            l("holeFlureDT---------------------- ende")
        Catch ex As Exception
            l("Fehler in holeFlureDT : " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Public Function holeZNDT(schematabelle As String, ziffer As String) As DataTable
        Dim sql As String
        If ziffer = String.Empty Then
            sql = "select distinct zaehler,nenner   from " & schematabelle & " " &
                   " where gemcode = " & aktFST.normflst.gemcode &
                   " and flur = " & aktFST.normflst.flur &
                   " order by zaehler,nenner"
        Else
            sql = "select distinct zaehler,nenner   from " & schematabelle & " " &
                   " where gemcode = " & aktFST.normflst.gemcode &
                   " and flur = " & aktFST.normflst.flur &
                   " order by zaehler,nenner"
        End If

        'Dim dt As DataTable = clsWebgisPGtools.holeDTfromFKAT(SQL)
        Dim dt As DataTable
        dt = getDTFromWebgisDB(sql, "postgis20")
        Return dt
    End Function


    ''' <summary>
    ''' die bbox wird zurückgegeben
    ''' </summary>
    ''' <param name="rechts"></param>
    ''' <param name="hoch"></param>
    ''' <param name="radius"></param> 
    Public Function calcBbox(rechts As String, hoch As String, radius As Integer) As clsRange
        Dim bbox As New clsRange
        Try
            l("calcBbox---------------------- anfang")
            bbox.xl = CInt(rechts) - radius
            bbox.yl = CInt(hoch) - radius
            bbox.xh = CInt(rechts) + radius
            bbox.yh = CInt(hoch) + radius
            Return bbox
            l("calcBbox---------------------- ende")
        Catch ex As Exception
            l("Fehler in calcBbox: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Function calcMinradius(radius As Double) As Integer
        Dim minradius As Integer
        Try
            l("---------------------- anfang")
            minradius = CInt(radius) * 2
            If minradius < 50 Then
                minradius = 50
            End If
            Return minradius
            l("calcMinradius---------------------- ende")
        Catch ex As Exception
            l("Fehler in calcMinradius: " & ex.ToString())
            Return 111
        End Try
    End Function
    Friend Sub FSGKrechtsGKHochwertHolen(ByRef pFST As clsFlurstueck, tabellenname As String)
        Dim box As String = ""
        Dim xl, xh, yl, yh As Double
        Try
            l("rechtsHochwertHolen---------------------- anfang")
            box = holeBoxKoordinatenFuerFS(pFST.FS, tabellenname)
            'MsgBox("box " & box)
            If box.IsNothingOrEmpty Then
                l("warnung in rechtsHochwertHolen box ist leer " & pFST.FS)
                pFST.GKrechts = 0
            Else
                If postgisBOX2range(box, xl, xh, yl, yh) Then
                    pFST.GKrechts = xl + ((xh - xl) / 2)
                    pFST.GKhoch = yl + ((yh - yl) / 2)
                    If xh - xl > yh - yl Then
                        pFST.radius = (xh - xl) / 2
                    Else
                        pFST.radius = (yh - yl) / 2
                    End If
                Else
                    l("Fehler in rechtsHochwertHolen keine box gefunden " & pFST.FS)
                End If
            End If
            l("rechtsHochwertHolen---------------------- ende")
        Catch ex As Exception
            l("Fehler in rechtsHochwertHolen: " & ex.ToString())
        End Try
    End Sub
    Public Function postgisBOX2range(ByVal box As String,
                                     ByRef xl As Double,
                                     ByRef xh As Double,
                                     ByRef yl As Double,
                                     ByRef yh As Double) As Boolean
        Try
            If box Is Nothing Then Return False
            If box = String.Empty Then Return False
            'vorsicht bei punkten - die min und max sind gleich
            Dim a(), lu, ro As String
            Dim neubox As String = box          'BOX(483463.4446 5538926.784,483844.154 5539296.5635)
            neubox = neubox.Replace("BOX(", "") '483463.4446 5538926.784,483844.154 5539296.5635)
            neubox = neubox.Replace(")", "")    '483463.4446 5538926.784,483844.154 5539296.5635                                              
            a = neubox.Split(","c)              '483463.4446 5538926.784
            lu = a(0) : ro = a(1)
            a = lu.Split(" "c)
            xl = CDbl(a(0).Replace(".", ","))
            yl = CDbl(a(1).Replace(".", ","))
            a = ro.Split(" "c)
            xh = CDbl(a(0).Replace(".", ","))
            yh = CDbl(a(1).Replace(".", ","))
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function holeBoxKoordinatenFuerFS(fs As String, tabellenname As String,
                                        Optional ByVal fromview As Boolean = True) As String
        Dim prefix As String = ".v_" : If Not fromview Then prefix = "."
        prefix = "."
        ' Dim basisrec As New clsDBspecPG
        Dim hinweis As String = ""
        Try
            'basisrec.mydb = CType(fstREC.mydb.Clone, clsDatenbankZugriff)
            Dim SQL = "SELECT ST_EXTENT(geom) FROM " & tabellenname & " where fs='" & fs & "' limit 1"
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            If dt.Rows.Count < 1 Then
                Return ""
            Else
                Dim koords As String = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
                Return koords
            End If
        Catch ex As Exception
            l("fehler in holeBoxKoordinatenFuerFS: " & ex.ToString)
            Return ""
        End Try
    End Function

    Friend Function getFS4UTM(utmpt As Point) As String
        Try
            l(" getFS4UTM ---------------------- anfang")
            Dim innerSQL As String = " SELECT ST_GeomFromText('POINT(" & utmpt.X & " " & utmpt.Y & ")'," &
                                               myglobalz.PostgisDBcoordinatensystem.ToString &
                                               ")"
            l(innerSQL)
            Dim SQL = "  SELECT * " &
                "  FROM flurkarte.basis_f " &
                "  WHERE ST_contains( ST_CurveToLine(flurkarte.basis_f.geom),(" & innerSQL & "  )" & "  );"
            l("sql: " & SQL)
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            l("Anzahl=" & dt.Rows.Count)
            If dt.Rows.Count < 1 Then
                Return ""
            Else
                Return clsDBtools.fieldvalue(dt.Rows(0).Item("fs")).ToString.Trim
                'zeigtauf = clsDBtools.fieldvalue(dt.Rows(0).Item("zeigtauf")).ToString.Trim
                'weistauf = clsDBtools.fieldvalue(dt.Rows(0).Item("weistauf")).ToString.Trim
                'albflaeche = clsDBtools.fieldvalue(dt.Rows(0).Item("flaeche")).ToString.Trim
                'Return True
            End If
            l(" getFS4UTM ---------------------- ende")
            Return ""
        Catch ex As Exception
            l("Fehler in getFS4UTM: " & ex.ToString())
            Return ""
        End Try
    End Function

    Public Function holeNennerDT(schematabelle As String) As DataTable
        Try
            l("---------------------- anfang")
            Dim SQL = "select distinct nenner  from  " & schematabelle & " " &
         " where gemcode = " & aktFST.normflst.gemcode &
         " and flur = " & aktFST.normflst.flur &
         " and zaehler = " & aktFST.normflst.zaehler &
         " order by nenner  "
            'Dim dt As DataTable = clsWebgisPGtools.holeDTfromFKAT(SQL)
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            Return dt
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Friend Function getStamm4aid(layer As clsLayerPres) As clsLayerPres
        Try
            If layer.aid < 1 Then
                Return layer
            End If
            For Each lay As clsLayerPres In allLayersPres
                If lay.aid = layer.aid Then
                    layer.ebene = lay.ebene
                    layer.titel = lay.titel
                    layer.schema = lay.schema
                    layer.isHgrund = lay.isHgrund
                    layer.masstab_imap = lay.masstab_imap
                    layer.mit_imap = lay.mit_imap
                    layer.mit_legende = lay.mit_legende
                    layer.pfad = lay.pfad
                    layer.rang = lay.rang
                    layer.sid = lay.sid
                    layer.standardsachgebiet = lay.standardsachgebiet
                    layer.schlagworte = lay.schlagworte
                    layer.mapFile = lay.calcMapfileFullname("layer")
                    layer.mapFileHeader = lay.calcMapfileFullname("header")
                    layer.SortierKriterium = lay.SortierKriterium
                    layer.isactive = lay.isactive
                    layer.tultipp = lay.tultipp
                    layer.mit_objekten = lay.mit_objekten
                    layer.suchfeld = layer.titel & " " & layer.schlagworte
                    Return layer
                End If
            Next
            l("warnung in getStamm4aid b:  es konnte kein layer gefunden werden. aid = " & layer.aid)
            layer.aid = 0
            Return layer
            '  Return Nothing
        Catch ex As Exception
            l("fehler in getStamm4aid: ", ex)
            Return Nothing
        End Try
    End Function
    Function getSerialFromPostgis(fs As String, isthistorisch As Boolean, tabellenname As String) As Boolean '
        l("getSerialFromPostgis1")
        Dim basisrec As New clsDBspecPG
        Dim hinweis As String = ""
        Try
            Dim Sql As String
            If isthistorisch Then
                Sql = "SELECt ST_AsText(geom)  FROM  " & tabellenname & " " &
                " where fs in(" &
                              "SELECT fs FROM   " & tabellenname & " " &
                              " where fs='" & fs & "' order by gisarea desc ) limit 1"
            Else
                Sql = "SELECt ST_AsText(geom)  FROM  " & tabellenname & " " &
                                " where gid in(" &
                                              "SELECT gid FROM   " & tabellenname & " " &
                                              " where fs='" & fs & "' order by gisarea desc )"
            End If

            l(basisrec.mydb.SQL)
            'Dim dt As DataTable = clsWebgisPGtools.holeDTfromFKAT(Sql)
            Dim dt As DataTable
            dt = getDTFromWebgisDB(Sql, "postgis20")
            If dt.Rows.Count < 1 Then
            Else
                aktFST.normflst.serials.Clear()
                For i = 0 To dt.Rows.Count - 1
                    aktFST.normflst.serials.Add(CStr(dt.Rows(i).Item(0)))
                Next
            End If
            l("getSerialFromPostgis fertig")
            Return True
        Catch ex As Exception
            l("fehler in getSerialFromPostgis: " & ex.ToString)
            Return False
        End Try
    End Function


    Function getGID4fs(fS As String, isthistorisch As Boolean, tabellenname As String) As String
        l("getGID4fs")
        Dim hinweis As String = ""
        Try
            Dim Sql As String
            If isthistorisch Then
                'Sql = "SELECt ST_AsText(geom)  FROM  " & tabellenname & " " &
                '" where fs in(" &
                '              "SELECT fs FROM   " & tabellenname & " " &
                '              " where fs='" & fS & "' order by gisarea desc ) limit 1"
                Sql = "SELECT gid,jahr FROM   " & tabellenname & " " &
                                             " where fs='" & fS & "'   order by jahr desc  "
            Else
                Sql = "SELECT gid FROM   " & tabellenname & " " &
                                              " where fs='" & fS & "'   "
            End If
            l(Sql)
            'Dim dt As DataTable = clsWebgisPGtools.holeDTfromFKAT(Sql)
            Dim dt As DataTable
            dt = getDTFromWebgisDB(Sql, "postgis20")
            If dt.Rows.Count < 1 Then
                Return "0"
            Else
                l("getGID4fs fertig")
                Dim summe As String = "#"
                If isthistorisch Then
                    For i = 0 To dt.Rows.Count - 1
                        summe = summe & "," & (clsDBtools.fieldvalue(dt.Rows(i).Item(1)))
                    Next
                    'summe = (clsDBtools.fieldvalue(dt.Rows(0).Item(1)))
                Else
                    For i = 0 To dt.Rows.Count - 1
                        summe = summe & "," & (clsDBtools.fieldvalue(dt.Rows(i).Item(0)))
                    Next
                    summe = summe.Replace("#,", "")
                End If
                Return summe
            End If
        Catch ex As Exception
            l("fehler in getGID4fs: " & ex.ToString)
            Return "-1"
        End Try
    End Function
    Function holeKoordinatenFuerUmkreis(aktPolygon As String) As String
        Try
            Dim dt As DataTable
            Dim SQL = "SELECT ST_EXTENT(ST_GeomFromText('" & aktPolygon & "', " & PostgisDBcoordinatensystem & ")) "
            dt = getDTFromWebgisDB(SQL, "postgis20")
            Return clsDBtools.fieldvalue(dt.Rows(0).Item(0))
        Catch ex As Exception
            nachricht("Fehler in holeKoordinatenFuerUmkreis: " & ex.ToString)
            Return ""
        End Try
    End Function

    Friend Function getPunkt4fs(strfromWhere As String) As myPoint
        Dim newpoint As New myPoint
        'SELECT ST_AsText(ST_PointOnSurface('POINT(0 5)'::geometry));
        Try
            Dim dt As DataTable
            'Dim SQL = "SELECT ST_EXTENT(ST_GeomFromText('" & aktPolygon & "', " & PostgisDBcoordinatensystem & ")) "
            'dt = getDTFromWebgisDB(SQL, "postgis20")

            Dim SQL = "SELECT ST_AsText(ST_PointOnSurface(geom)) FROM " & strfromWhere
            dt = getDTFromWebgisDB(SQL, "postgis20")
            Dim result As String = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
            nachricht(result)
            '"POINT(480330.353353078 5538953.97)"
            result = result.Replace("POINT(", "").Replace(")", "")
            Dim a() As String
            a = result.Split(" "c)
            newpoint.X = CInt(a(0).Replace(".", ","))
            newpoint.Y = CInt(a(1).Replace(".", ","))
            Return newpoint
        Catch ex As Exception
            nachricht("Fehler in getPunkt4fs: " & ex.ToString)
            Return Nothing
        End Try
    End Function

    Friend Function holeBoxKoordinatenFuerStrasse(gid As String, schematab As String) As String
        Dim hinweis As String = ""
        Try
            'basisrec.mydb = CType(fstREC.mydb.Clone, clsDatenbankZugriff)
            Dim SQL = "SELECT ST_EXTENT(geom) FROM " & schematab & " where gid in (" & gid & ")"
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            If dt.Rows.Count < 1 Then
                Return ""
            Else
                Dim koords As String = clsDBtools.fieldvalue(dt.Rows(0).Item(0))
                Return koords
            End If
        Catch ex As Exception
            l("fehler in holeBoxKoordinatenFuerFS: " & ex.ToString)
            Return ""
        End Try
    End Function
End Module

