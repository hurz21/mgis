Imports System.Data

Public Class clsSachdatentools
    Shared Sub dossierOhneImap(KoordinateKLickpt As Point?)
        Dim fangRadiusInMeter As Double
        Dim utmpt As Point = clsMiniMapTools.makeUTM(KoordinateKLickpt)
        clsMiniMapTools.makeTabname()
        fangRadiusInMeter = clsSachdatentools.calcFangradiusM(globCanvasWidth, myglobalz.fangradius_in_pixel,
                              kartengen.aktMap.aktrange.xdif, layerActive.tabname)
        'FS feststellen
        aktFST.clear()
        aktFST.punkt.X = utmpt.X
        aktFST.punkt.Y = utmpt.Y
        aktFST.normflst.FS = pgisTools.getFS4UTM(utmpt)
        aktFST.normflst.splitFS(aktFST.normflst.FS)

        clsFSTtools.holeKoordinaten4Flurstueck(aktFST.normflst.nenner.ToString, WinDetailSucheFST.AktuelleBasisTabelle, aktFST)
        getSerialFromPostgis(aktFST.normflst.FS, False, WinDetailSucheFST.AktuelleBasisTabelle) ' setzt  aktFST.serial 
        'btnSuchobjAusSchalten.Visibility = Visibility.Visible 
        clsFSTtools.dossierPrepMinimum()
    End Sub
    Private Shared Function getGID4FS(fs As String, schema As String,
                                              tabelle As String) As Integer()
        Dim Sql As String
        l("getGID4POINTlayer       ")
        Sql = "SELECT *  from " & schema & "." & tabelle & " as g,  " &
                    "ST_Buffer(ST_CurveToLine( ( select geom from  flurkarte.basis_f   where fs='" & fs & "')),-0.5,2) as b   " &
                    " WHERE  ST_Intersects(g.geom, b) "
        l("sql: " & Sql)
        Try
            l("getGID4layer ")
            Dim dt As DataTable
            Dim gids() As Integer
            dt = getDTFromWebgisDB(Sql, "postgis20")
            ReDim gids(dt.Rows.Count - 1)
            For i = 0 To dt.Rows.Count - 1
                l("getGID4layer " & clsDBtools.fieldvalue(dt.Rows(i).Item(0)))
                gids(i) = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item(0)))
            Next
            Return gids 'CInt(clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
        Catch ex As Exception
            l("Fehler in getGID4POINTlayer: ", ex)
            Return Nothing
        End Try
    End Function
    Shared Function fsMitAktiveEbene(aid As Integer,
                                     fs As String) As Integer()
        Dim gids() As Integer
        Dim summe As String = ""
        Try
            l(" fsMitAktiveEbene ---------------------- anfang")
            l("aid " & aid)
            l("fs " & fs)
            ' schema, tabelle gid zur aid ermitteln 
            os_tabelledef = New clsTabellenDef
            os_tabelledef.aid = CStr(aid)
            os_tabelledef.gid = "0"
            os_tabelledef.datenbank = "postgis20"
            os_tabelledef.tab_nr = CType(1, String)
            sachdatenTools.getSChema(os_tabelledef)
            'schema = "planung" : tabelle = "bebauungsplan_f" 
            gids = (getGID4FS(fs, os_tabelledef.Schema, os_tabelledef.tabelle))
            l("Fehler in fsMitAktiveEbene: gids.Count " & gids.Count & " aid:" & aid)
            Return gids
        Catch ex As Exception
            l("Fehler in fsMitAktiveEbene:   " & " aid:" & aid & ex.ToString())
            Return Nothing
        End Try
        Return Nothing
    End Function
    Private Shared Function dossierAktiveEbene(winpt As Point, aid As Integer,
                                               radiusInMeter As Integer) As Integer()
        Dim summe As String = ""
        Try
            l(" dossierAkviveEbene ---------------------- anfang")
            ' schema, tabelle gid zur aid ermitteln 
            os_tabelledef = New clsTabellenDef
            os_tabelledef.aid = CStr(aid)
            os_tabelledef.gid = "0"
            os_tabelledef.datenbank = "postgis20"
            os_tabelledef.tab_nr = CType(1, String)
            sachdatenTools.getSChema(os_tabelledef)
            'schema = "planung" : tabelle = "bebauungsplan_f"
            Dim gids() As Integer
            gids = (getGID4POINTlayer(winpt, os_tabelledef.Schema, os_tabelledef.tabelle, radiusInMeter))
            Return gids
        Catch ex As Exception
            l("Fehler in dossierAkviveEbene: " & ex.ToString())
            Return Nothing
        End Try
        Return Nothing
    End Function

    Shared Function erzeugeUndOeffneEigentuemerPDF(text As String) As String
        Dim lokalitaet, flaeche As String
        lokalitaet = getlokalitaetstring(aktFST)
        flaeche = clsFSTtools.getFlaecheZuFlurstueck(aktFST)
        lokalitaet = lokalitaet & " " & flaeche
        Dim ausgabedatei As String = tools.calcEigentuemerAusgabeFile

        wrapItextSharp.createSchnellEigentuemer(text, ausgabedatei, albverbotsString, lokalitaet)
        Return ausgabedatei
    End Function
    Friend Shared Function getActiveLayer4point(winpt As Point, aid As Integer,
                                               cwidth As Integer, cheight As Integer,
                                               _screenPT As Point?,
                                                fangRadiusInMeter As Double) As Integer()
        Dim returnvalue As String = ""
        Dim screenpt As New myPoint
        Dim gids As Integer()
        Try
            l(" getActiveLayer4point ---------------------- anfang")
            screenpt.X = _screenPT.Value.X
            screenpt.Y = _screenPT.Value.Y
            gids = dossierAktiveEbene(winpt, aid, CInt(fangRadiusInMeter))
            l(" getActiveLayer4point ---------------------- ende")
            Return gids
        Catch ex As Exception
            l("Fehler in getActiveLayer4point: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Shared Function calcFangradiusM(cwidth As Integer,
                                    fangradius_in_pixel As Integer,
                                    xdifMeter As Double,
                                    tabname As String) As Double
        Dim radiusInMeter As Integer
        Dim MeterProPixel As Double
        Try
            l(" calcFangradiusM ---------------------- anfang")
            If tabname.EndsWith("_f") Then
                radiusInMeter = 1
            Else
                MeterProPixel = xdifMeter / cwidth
                'If MeterProPixel < 1 Then MeterProPixel = 1
                radiusInMeter = CInt((MeterProPixel) * fangradius_in_pixel)
            End If
            l(" calcFangradiusM ---------------------- ende")
            Return radiusInMeter
        Catch ex As Exception
            l("Fehler in calcFangradiusM: " & ex.ToString())
            Return 4
        End Try
    End Function

    Shared Sub StartGisDossierExtern(winpt As Point, aktaid As Integer, cwidth As Integer,
                                          cheight As Integer, screenx As Double, screeny As Double,
                                             radiusInMeter As Integer, username As String,
                                             obergruppe As String, vid As String, fs As String,
                                     geometrietyp As String, unterGruppe As String)
        Dim strKoord As String = " koordinate=" & CStr(winpt.X).Replace(",", ".") & "," & CStr(winpt.Y).Replace(",", ".")
        Dim strIstAlb As String = " istalbberechtigt=0 "
        Dim strAktaid As String = " aktaid=" & aktaid & " "
        Dim strbreite As String = " breite=" & cwidth
        Dim strhoehe As String = " hoehe=" & cheight
        Dim strscreenx As String = " screenx=" & screenx
        Dim strscreeny As String = " screeny=" & screeny
        Dim strradius As String = " radiusinmeter=" & radiusInMeter
        Dim strusername As String = " username=" & username
        Dim strobergruppe As String = " obergruppe=" & obergruppe
        Dim strunterGruppe As String = " untergruppe=" & unterGruppe
        Dim strFS As String = " fs=" & fs
        Dim strVID As String = " vid=" & vid
        Dim strGeometrie As String = " geometrie=" & geometrietyp ' punkt flurstueck

        If GisUser.istalbberechtigt Then
            strIstAlb = " istalbberechtigt=1 "
        Else
            strIstAlb = " istalbberechtigt=0 "
        End If

        Dim params As String = strGeometrie & strVID & strFS & strKoord & " " & strIstAlb & " " & strAktaid &
            strbreite & strhoehe & strscreenx & strscreeny & strradius & strusername & strobergruppe & strunterGruppe
        Try
            l("StartGisDossierExtern ---------------------- anfang")
            l("myglobalz.gisdossierexe   " & myglobalz.gisdossierexe)
            l("params " & params)
            Process.Start(myglobalz.gisdossierexe, params)
            l("StartGisDossierExtern ---------------------- ende")
        Catch ex As Exception
            l("Fehler in StartGisDossierExtern: " & params & " " & ex.ToString())
        End Try
    End Sub

    Private Shared Function getGID4POINTlayer(winpt As Point, schema As String,
                                              tabelle As String,
                                              radiusInMeter As Integer) As Integer()
        Dim innerSQL As String
        l("getGID4POINTlayer       ")
        innerSQL = "  ST_Buffer(SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ")," &
            PostgisDBcoordinatensystem.ToString & ")," & radiusInMeter & ",2)"
        l(innerSQL)
        Dim SQL = "SELECT * " &
                " from " & schema & "." & tabelle & " as g, " & innerSQL & " as b " &
                "   WHERE  ST_Intersects(g.geom, b) order by gid desc"
        l(SQL)
        Try
            l("getGID4layer ")
            Dim dt As DataTable
            Dim gids() As Integer
            dt = getDTFromWebgisDB(SQL, "postgis20")
            ReDim gids(dt.Rows.Count - 1)
            For i = 0 To dt.Rows.Count - 1
                l("getGID4layer " & clsDBtools.fieldvalue(dt.Rows(i).Item(0)))
                gids(i) = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item(0)))
            Next
            Return gids 'CInt(clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
        Catch ex As Exception
            l("Fehler in getGID4POINTlayer: ", ex)
            Return Nothing
        End Try
    End Function

    Friend Shared Sub getdossier(utmpt As Point, aid As Integer, cwidth As Integer, cHeight As Integer,
                                 _screenPT As Point?, inputfs As String, InputGeometrie As String)
        Dim returnvalue As String = ""
        Dim screenpt As New myPoint
        Dim gids As Integer()
        Try
            l(" getdossier ---------------------- anfang")
            screenpt.X = _screenPT.Value.X
            screenpt.Y = _screenPT.Value.Y

            Dim radius_in_pixel As Integer = 7
            Dim MeterProPixel As Double
            Dim radiusInMeter As Integer

            MeterProPixel = kartengen.aktMap.aktrange.xdif / cwidth
            If MeterProPixel < 1 Then MeterProPixel = 1
            radiusInMeter = CInt((MeterProPixel) * radius_in_pixel)
            l("fehler kein fehler dossier aufruf " & Environment.UserName)
            StartGisDossierExtern(utmpt, layerActive.aid, cwidth, cHeight,
                                    screenpt.X, screenpt.Y, radiusInMeter,
                                    GisUser.username, GisUser.ADgruppenname, "0", inputfs,
                                  InputGeometrie, GisUser.favogruppekurz
                                    )
            l(" getdossier ---------------------- ende")
        Catch ex As Exception
            l("Fehler in getdossier: " & ex.ToString())
        End Try
    End Sub
End Class
