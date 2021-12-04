Imports System.Data
Imports System.IO
Public Class clsDossier
    Public Shared Altstadtsatzung As New clsDossierItem
    Public Shared Schwalben As New clsDossierItem
    Public Shared Bplan As New clsDossierItem
    Public Shared Eigentuemer As New clsDossierItem
    Public Shared Kehr As New clsDossierItem
    Public Shared NSG As New clsDossierItem
    Public Shared LSG As New clsDossierItem
    Public Shared FFH As New clsDossierItem
    Public Shared WSG As New clsDossierItem
    Public Shared Altlast As New clsDossierItem
    Public Shared Boris As New clsDossierItem
    Public Shared standorttypisierung As New clsDossierItem
    Public Shared wsgHNUGwms As New clsDossierItem
    Public Shared Illegale As New clsDossierItem
    Public Shared IllegaleAlt As New clsDossierItem
    Public Shared Baulasten As New clsDossierItem
    Public Shared Ueb As New clsDossierItem
    Public Shared UEBKROF As New clsDossierItem
    Public Shared Foerder As New clsDossierItem
    Public Shared Hbiotope As New clsDossierItem
    Public Shared Hkomplexe As New clsDossierItem
    Public Shared HNaturdenkmale As New clsDossierItem
    Public Shared Amphibien As New clsDossierItem
    Public Shared BSE As New clsDossierItem
    Public Shared OEKOKO As New clsDossierItem
    Public Shared ND As New clsDossierItem
    Public Shared altis16 As New clsDossierItem
    Public Shared paradigmavorgang As New clsDossierItem
    Public Shared kompensation As New clsDossierItem
    Shared Function getdtFSmitEbene(fs As String, schematabelle As String, ByRef strError As String) As DataTable
        Dim innerSQL, SQl As String
        Dim dt As DataTable
        Try
            l(" getdtFSmitEbene ---------------------- anfang")
            SQl = "SELECT *  from " & schematabelle & " as g,  " &
                    "ST_Buffer(ST_CurveToLine( ( select geom from  flurkarte.basis_f   where fs='" & fs & "')),-0.5,2) as b   " &
                    " WHERE  ST_Intersects(g.geom, b) "
            l("sql: " & SQl)
            l(" getDtHauptabfrageFlaeche ---------------------- anfang")
            dt = clsPgtools.getDTFromWebgisDB(SQl, "postgis20", strError)
            l(" getDtHauptabfragePunktebene ---------------------- ende")
            Return dt
        Catch ex As Exception
            l("Fehler in getdtFSmitEbene: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Shared Function getDtPunktVerschneidung(winpt As myPoint,
                                            SchemaTabelle As String,
                                            radiusInMeter As Integer, ByRef strError As String) As DataTable
        'SchemaTabelle = "schutzgebiete.ffhgebiet_f"
        'dt = clsDossier.getDtHauptabfrageFlaeche(winpt, SchemaTabelle) 
        Dim innerSQL, SQl As String
        Dim dt As DataTable
        l(" getDtHauptabfragePunktebene ---------------------- anfang " & SchemaTabelle)
        Try
            'innerSQL = "select SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ")," &
            '        clsStartup.PostgisDBcoordinatensystem.ToString & ")"
            'innerSQL = "ST_Buffer(SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ",25832)," & radiusInMeter & ",2)"
            l(innerSQL)
            SQl = "SELECT * " &
                "  FROM " & SchemaTabelle & " as g, " &
                "  ST_Buffer(SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & "),25832)," & radiusInMeter & ",2) as b" &
                "  WHERE ST_Intersects(g.geom, b);"
            l("sql: " & SQl)
            l(" getDtHauptabfrageFlaeche ---------------------- anfang")
            dt = clsPgtools.getDTFromWebgisDB(SQl, "postgis20", strError)
            l(" getDtHauptabfragePunktebene ---------------------- ende")
            Return dt
        Catch ex As Exception
            l("Fehler in getDtHauptabfragePunktebene: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Shared Sub Question(winpt As myPoint, ByRef dt As DataTable, schematabelle As String, ByRef strError As String)
        If m.flurstuecksModus Then
            dt = clsDossier.getdtFSmitEbene(m.aktFST.normflst.FS, schematabelle, strError)
        Else
            dt = clsDossier.getDtHauptabfrageFlaeche(winpt, schematabelle, strError)
        End If
    End Sub
    Shared Function getDtHauptabfrageFlaeche(winpt As myPoint, SchemaTabelle As String, ByRef strError As String) As DataTable
        Dim innerSQL, SQl As String
        Dim dt As DataTable
        l(" getDtHauptabfrageFlaeche ---------------------- anfang " & SchemaTabelle)
        Try
            'SchemaTabelle = "muell"
            innerSQL = "select SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ")," &
                    m.PostgisDBcoordinatensystem.ToString & ")"
            l(innerSQL)
            SQl = "SELECT * " &
                "  FROM " & SchemaTabelle &
                "  WHERE ST_contains(ST_CurveToLine( " & SchemaTabelle & ".geom),(" & innerSQL & ")" & ");"
            l("sql: " & SQl)

            l(" getDtHauptabfrageFlaeche ---------------------- anfang")
            dt = clsPgtools.getDTFromWebgisDB(SQl, "postgis20", strError)
            l(" getDtHauptabfrageFlaeche ---------------------- ende")
            Return dt
        Catch ex As Exception
            l("Fehler in getDtHauptabfrageFlaeche: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    'Friend Shared Function getActiveLayer4point(winpt As Point, aid As Integer, dossiertyp As String,
    '                                            cwidth As Integer, cheight As Integer,
    '                                            _screenPT As Point?) As String
    '    Dim returnvalue As String = ""
    '    Dim screenpt As New myPoint
    '    Try
    '        l(" getActiveLayer4point ---------------------- anfang")

    '        screenpt.X = _screenPT.Value.X
    '        screenpt.Y = _screenPT.Value.Y

    '        If dossiertyp = "aktiveebene" Then
    '            Dim radius_in_pixel As Integer = 5
    '            Dim radiusInMeter As Integer = CInt(kartengen.aktMap.aktrange.xdif / cwidth) * radius_in_pixel
    '            returnvalue = dossierAktiveEbene(winpt, aid, dossiertyp, returnvalue, radiusInMeter)
    '        End If
    '        If dossiertyp = "dossier" Then
    '            Dim doss As New winDossier(winpt, cwidth, cheight, screenpt)
    '            doss.Show()
    '            l("fehler kein fehler dossier aufruf " & Environment.UserName)
    '            '  returnvalue = dossierAktiveEbene(winpt, aid, dossiertyp, returnvalue, 3)
    '        End If
    '        l(" getActiveLayer4point ---------------------- ende")
    '        Return returnvalue
    '    Catch ex As Exception
    '        l("Fehler in getActiveLayer4point: " & ex.ToString())
    '        Return ""
    '    End Try
    'End Function
    'Private Shared Function dossierAktiveEbene(winpt As Point, aid As Integer,
    '                                           dossiertyp As String,
    '                                           returnvalue As String,
    '                                           radiusInMeter As Integer) As String
    '    Dim summe As String = ""
    '    Dim javascriptMimikry As String
    '    Try
    '        l(" dossierAkviveEbene ---------------------- anfang")
    '        ' schema, tabelle gid zur aid ermitteln 
    '        os_tabelledef = New clsTabellenDef
    '        os_tabelledef.aid = CStr(aid)
    '        os_tabelledef.gid = "0"
    '        os_tabelledef.datenbank = "postgis20"
    '        os_tabelledef.tab_nr = CType(1, String)
    '        sachdatenTools.getSChema(os_tabelledef)
    '        'schema = "planung" : tabelle = "bebauungsplan_f"
    '        Dim gids() As Integer
    '        gids = (getGID4POINTlayer(winpt, os_tabelledef.Schema, os_tabelledef.tabelle, radiusInMeter))
    '        If gids.Length > 0 Then
    '            os_tabelledef.gid = CType(gids(0), String)
    '            If CInt(os_tabelledef.gid) > 0 Then
    '                javascriptMimikry = "javascript:Datenabfrage(" & os_tabelledef.aid & ", " &
    '                                        os_tabelledef.tab_nr &
    '                                        ", " & os_tabelledef.gid & ")"
    '                clsMiniMapTools.handlejavascript(javascriptMimikry)
    '                l(" dossierAkviveEbene ---------------------- ende")
    '                returnvalue = summe
    '            Else
    '                'ausserhalb
    '                MessageBox.Show("Klicken Sie bitte innerhalb einer markierten Fläche.",
    '                                "Daneben ", MessageBoxButton.OK, MessageBoxImage.Exclamation)
    '                returnvalue = ""
    '            End If
    '        Else
    '            'ausserhalb
    '            MessageBox.Show("Klicken Sie bitte innerhalb einer markierten Fläche.",
    '                            "Daneben ", MessageBoxButton.OK, MessageBoxImage.Exclamation)
    '            returnvalue = ""
    '        End If
    '    Catch ex As Exception
    '        l("Fehler in dossierAkviveEbene: " & ex.ToString())
    '        returnvalue = ""
    '    End Try
    '    Return returnvalue
    'End Function

    Friend Shared Function makebplanPDFliste(rESULT_dateien_Bplan As List(Of clsGisresult)) As List(Of clsGisresult)
        Dim blist As New List(Of clsGisresult)
        Dim gr As New clsGisresult
        Try
            l(" makebplanPDFliste ---------------------- anfang")
            For i = 0 To rESULT_dateien_Bplan.Count - 1
                For j = 0 To rESULT_dateien_Bplan(i).begleitdateien.Count - 1
                    gr = New clsGisresult
                    gr.etikett = CStr(rESULT_dateien_Bplan(i).begleitdateien(j).Name).Replace(".pdf", "").Replace(".PDF", "")
                    gr.verordnung = CStr(rESULT_dateien_Bplan(i).datei.Name).Replace(".pdf", "").Replace(".PDF", "")
                    gr.datei = rESULT_dateien_Bplan(i).begleitdateien(j)
                    blist.Add(gr)
                Next
            Next
            l(" makebplanPDFliste ---------------------- ende")
            Return blist
        Catch ex As Exception
            l("Fehler in makebplanPDFliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Shared Function getGID4_AREAlayer(winpt As Point, schema As String,
                                              tabelle As String,
                                              ByRef strError As String) As Integer
        Dim innerSQL As String = "select SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ")," &
            m.PostgisDBcoordinatensystem.ToString & ")"
        l(innerSQL)
        '  SELECT GEMARKUNG,nr,NR,PDF,titel,RECHTS,HOCH " &
        Dim SQL = "SELECT * " &
                "  FROM " & schema & "." & tabelle & " " &
                "  WHERE ST_contains( ST_CurveToLine(" & schema & "." & tabelle & ".geom),(" & innerSQL & "  )" & "  );"
        l(SQL)
        Try
            l("getGID4layer ")
            Dim dt As DataTable
            dt = clsPgtools.getDTFromWebgisDB(SQL, "postgis20", strError)
            l("getGID4layer " & clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
            Return CInt(clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
        Catch ex As Exception
            l("Fehler in holeKoordinatenFuerGID: ", ex)
            Return 0
        End Try
    End Function
    Private Shared Function getGID4POINTlayer(winpt As Point, schema As String,
                                              tabelle As String,
                                              radiusInMeter As Integer,
                                              ByRef strError As String) As Integer()
        Dim innerSQL As String
        l("getGID4POINTlayer       ")
        innerSQL = "  ST_Buffer(SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ")," &
            m.PostgisDBcoordinatensystem.ToString & ")," & radiusInMeter & ",2)"
        l(innerSQL)
        '  SELECT GEMARKUNG,nr,NR,PDF,titel,RECHTS,HOCH " &
        Dim SQL = "SELECT * " &
                " from " & schema & "." & tabelle & " as g, " & innerSQL & " as b " &
                "   WHERE  ST_Intersects(g.geom, b) order by gid desc"
        l(SQL)
        Try
            l("getGID4layer ")
            Dim dt As DataTable
            Dim gids() As Integer
            dt = clsPgtools.getDTFromWebgisDB(SQL, "postgis20", strError)
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

    Friend Shared Function istDossierModus(aid As Integer) As Boolean
        Return True
    End Function
    Shared Function bildeAttributTabelle(dt As System.Data.DataTable) As String
        Try
            Dim summe As String = ""
            Dim trenn As String = " " & Environment.NewLine
            If dt.Rows.Count > 1 Then
                summe = summe & " Es gibt hier " & dt.Rows.Count & " Ausweisungen !" & trenn
            End If
            For i = 0 To dt.Rows.Count - 1
                For col = 0 To dt.Columns.Count - 1
                    If dt.Columns(col).ColumnName.Trim <> "geom" And
                        dt.Columns(col).ColumnName.Trim <> "b" Then
                        summe = summe & clsString.Capitalize(dt.Columns(col).ColumnName).Trim & ": " &
                                        clsDBtools.fieldvalue(dt.Rows(i).Item(col).ToString.Trim) & trenn
                    End If

                Next
                summe = summe & " ----------------------------------- " & trenn
            Next
            Return summe
        Catch ex As Exception
            nachricht("fehler in bildeBaulastenINFO: " & ex.ToString)
            Return "keine info"
        End Try
    End Function
End Class
