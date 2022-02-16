Imports System.Data

Public Class clsGIStools
    Shared Function fstGIS2BLOBJ() As List(Of clsBaulast)
        'Dim tfst As New clsFlurstueck
        Dim bl As New clsBaulast
        Dim liste As New List(Of clsBaulast)
        Try
            l(" MOD fstGIS2BLOBJ anfang")
            For i = 0 To fstREC.dt.Rows.Count - 1
                'tfst = New clsFlurstueck
                bl = New clsBaulast
                bl.katFST = New clsFlurstueck

                bl.blattnr = CInt(fstREC.dt.Rows(i).Item("jahr_blattnr")).ToString.Trim
                bl.katFST.gid = CInt((fstREC.dt.Rows(i).Item("gid")).ToString.Trim)
                bl.katFST.gemcode = CInt(fstREC.dt.Rows(i).Item("gemcode"))
                bl.katFST.flur = CInt(fstREC.dt.Rows(i).Item("flur"))
                bl.katFST.zaehler = CInt(fstREC.dt.Rows(i).Item("zaehler"))
                bl.katFST.nenner = CInt(fstREC.dt.Rows(i).Item("nenner"))
                bl.katFST.FS = (fstREC.dt.Rows(i).Item("fs")).ToString.Trim
                bl.katFST.gemeindename = (fstREC.dt.Rows(i).Item("gemeinde")).ToString.Trim
                bl.katFST.gemarkungstext = (fstREC.dt.Rows(i).Item("gemarkung")).ToString.Trim
                bl.katFST.zeigtauf = (fstREC.dt.Rows(i).Item("gefundenin")).ToString.Trim
                bl.laufnr = CInt((fstREC.dt.Rows(i).Item("baulastnr")).ToString.Trim)
                bl.status = ((fstREC.dt.Rows(i).Item("kennzeichen1")).ToString.Trim)
                bl.datei = ((fstREC.dt.Rows(i).Item("tiff2")).ToString.Trim)
                bl.gueltig = ((fstREC.dt.Rows(i).Item("gueltig")).ToString.Trim)
                bl.datum1 = ((fstREC.dt.Rows(i).Item("datum")).ToString.Trim)
                bl.genese = CInt((fstREC.dt.Rows(i).Item("genese")))

                bl.katFST.fstueckKombi = bl.katFST.buildFstueckkombi().Trim
                liste.Add(bl)
            Next
            Return liste

            l(" MOD fstGIS2BLOBJ ende")
        Catch ex As Exception
            l("Fehler in fstGIS2BLOBJ: " & ex.ToString())
        End Try
    End Function
    Shared Function fstGIS2OBJ() As List(Of clsFlurstueck)
        Dim tfst As New clsFlurstueck
        Dim liste As New List(Of clsFlurstueck)
        Try
            l(" MOD fstGIS2OBJ anfang")
            For i = 0 To fstREC.dt.Rows.Count - 1
                tfst = New clsFlurstueck
                tfst.gemcode = CInt(fstREC.dt.Rows(i).Item("gemcode"))
                tfst.flur = CInt(fstREC.dt.Rows(i).Item("flur"))
                tfst.zaehler = CInt(fstREC.dt.Rows(i).Item("zaehler"))
                tfst.nenner = CInt(fstREC.dt.Rows(i).Item("nenner"))
                tfst.FS = (fstREC.dt.Rows(i).Item("fs")).ToString.Trim
                tfst.gemeindename = (fstREC.dt.Rows(i).Item("gemeinde")).ToString.Trim
                tfst.gemarkungstext = (fstREC.dt.Rows(i).Item("gemarkung")).ToString.Trim
                tfst.gemeindename = (fstREC.dt.Rows(i).Item("gefundenin")).ToString.Trim
                tfst.gid = CInt((fstREC.dt.Rows(i).Item("gid")).ToString.Trim)
                tfst.gebucht = ((fstREC.dt.Rows(i).Item("baulastnr")).ToString.Trim)
                tfst.genese = CInt((fstREC.dt.Rows(i).Item("genese")).ToString.Trim)
                tfst.fstueckKombi = tfst.buildFstueckkombi().Trim
                liste.Add(tfst)
            Next
            Return liste
            l(" MOD fstGIS2OBJ ende")
        Catch ex As Exception
            l("Fehler in fstGIS2OBJ: " & ex.ToString())
            Return liste
        End Try
    End Function

    Friend Shared Function fromProbauGObjekt(fSTausPROBAUGListe As List(Of clsFlurstueck)) As List(Of clsFlurstueck)
        Dim tfst As New clsFlurstueck
        Dim liste As New List(Of clsFlurstueck)
        Try
            l(" MOD fstGIS2OBJ anfang")
            For i = 0 To fSTausPROBAUGListe.Count - 1
                tfst = New clsFlurstueck
                tfst.gemcode = CInt(fSTausPROBAUGListe.Item(i).gemcode)
                tfst.flur = CInt(fSTausPROBAUGListe.Item(i).flur)
                tfst.zaehler = CInt(fSTausPROBAUGListe.Item(i).zaehler)
                tfst.nenner = CInt(fSTausPROBAUGListe.Item(i).nenner)
                tfst.FS = (fSTausPROBAUGListe.Item(i).FS)
                tfst.gemeindename = (fSTausPROBAUGListe.Item(i).gemeindename)
                tfst.gemarkungstext = (fSTausPROBAUGListe.Item(i).gemarkungstext)
                tfst.gemeindename = ("digitalisiert").ToString.Trim
                tfst.gid = 0
                tfst.gebucht = (fSTausPROBAUGListe.Item(i).gebucht).ToString.Trim
                tfst.fstueckKombi = tfst.buildFstueckkombi().Trim
                liste.Add(tfst)
            Next
            Return liste
            l(" MOD fstGIS2OBJ ende")
        Catch ex As Exception
            l("Fehler in fstGIS2OBJ: " & ex.ToString())
            Return liste
        End Try
    End Function

    Friend Shared Function getGISrecord2(sql As String) As String
        Dim hinweis As String
        Try
            l(" MOD ---------------------- anfang")
            l("getGISrecord2---------------------- anfang")
            fstREC.mydb.SQL = sql
            l(fstREC.mydb.SQL)
            hinweis = fstREC.getDataDT()
            If fstREC.dt.Rows.Count < 1 Then
                'Return ""
            Else
                Debug.Print(clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item(0)))
            End If

            Return hinweis

            l(" MOD getGISrecord2 ende")
        Catch ex As Exception
            l("Fehler in getGISrecord2: " & ex.ToString())
        End Try
    End Function
    Friend Shared Function getGISrecord(v As Integer) As String
        Dim hinweis As String
        Try
            l(" MOD ---------------------- anfang")
            l("getSerialFromBasis---------------------- anfang")
            fstREC.mydb.SQL = "select * from " & tools.srv_schema & "." & tools.srv_tablename & " where jahr_blattnr ='" & v & "' order by gemcode, flur, zaehler, nenner"
            l(fstREC.mydb.SQL)
            hinweis = fstREC.getDataDT()
            If fstREC.dt.Rows.Count < 1 Then
                'Return ""
            Else
                Debug.Print(clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item(0)))
            End If

            Return hinweis

            l(" MOD getGISrecordv ende")
        Catch ex As Exception
            l("Fehler in getGISrecordv: " & ex.ToString())
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
    Friend Shared Function calcNewRange(gidstring As String) As clsRange
        Dim drange As New clsRange
        Dim rangestring, hinweis As String
        Try
            l(" MOD calcNewRange anfang")
            If gidstring.IsNothingOrEmpty Then
                MessageBox.Show("Sie haben noch kein GIS-Flurstück angelegt! Abbruch!")
                Return drange
            End If
            fstREC.mydb.SQL = "SELECT ST_EXTENT(geom) FROM " & tools.srv_schema & "." & tools.srv_tablename & "   where gid in (" & gidstring & ")"

            l(fstREC.mydb.SQL)
            hinweis = fstREC.getDataDT()
            If fstREC.dt.Rows.Count < 1 Then
                MessageBox.Show("Fehler bei der Ermittlung der Lokalität der GIS-Flurstücke!!")
                Return drange
            Else
                rangestring = clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item(0)).Trim
            End If
            drange.BBOX = rangestring
            drange.bbox_split()
            l(" MOD calcNewRange ende")
            Return drange
        Catch ex As Exception
            l("Fehler in MOcalcNewRangeD: " & ex.ToString())
            Return drange
        End Try
    End Function

    Friend Shared Function bildegidstring() As String
        Dim summe As String = ""
        For Each item As clsFlurstueck In tools.FSTausGISListe
            summe = summe & "," & item.gid
        Next
        summe = clsString.removeLeadingChar(summe, ",")
        'summe = summe.Substring(0, summe.Length - 1)
        Return summe
    End Function

    Friend Shared Function loeschenGISDatensatz(text As String) As Integer
        l("getSerialFromBasis---------------------- anfang")
        Dim newid As Long = 0
        Dim hinweis As String = ""
        Try
            l(" MOD ---------------------- anfang")
            fstREC.mydb.SQL = "delete from " & tools.srv_schema & "." & tools.srv_tablename & " where jahr_blattnr ='" & text & "'  "
            l(fstREC.mydb.SQL)
            Dim retcode = fstREC.dboeffnen(hinweis)
            newid = fstREC.sqlexecute(newid)
            retcode = fstREC.dbschliessen(hinweis)
            If fstREC.dt.Rows.Count < 1 Then
                Return 0
            Else
                Debug.Print(clsDBtools.fieldvalue(fstREC.dt.Rows(0).Item(0)))
            End If
            l(" MOD ---------------------- ende")
            Return CInt(newid)
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
            Return 0
        End Try
    End Function

    Friend Shared Function loescheTiffaufGISServer(baulastnr As String, gemarkung As String) As Boolean
        Dim aufruf, hinweis As String
        l(" MOD loescheTiffaufGISServer anfang")
        Try
            aufruf = "http://gis.kreis-of.local/cgi-bin/apps/neugis/dbgrab/dbgrab.cgi?user=feinen_j&modus=prepbaulast&tiff=" & baulastnr.Trim &
                    "&gemarkung=" & gemarkung.Trim.ToLower
            l("droptiff aufruf " & aufruf)
            Dim result = meineHttpNet.meinHttpJob("", aufruf, hinweis, tools.enc, 5000)
            If result.ToLower.Contains("fehler") Then
                Return False
            Else
                Return True
            End If
            l(" MOD loescheTiffaufGISServer ende: " & result)
        Catch ex As Exception
            l("Fehler in loescheTiffaufGISServer: " & ex.ToString())
            Return False
        End Try
    End Function

    Friend Shared Function updateGISDB(baulastblatnr As String, zuielname As String, gemarkung As String, endung As String) As Boolean
        Dim sql As String
        Dim neuerTIFFname As String
        'fkat/baulasten/Sprendlingen/2284.tiff     
        'srv_subdirBaulsten/Klein-Welzheim/13022.tiff            
        'tiff like 'KeineDaten.htm 


        Try
            l(" MOD updateGISDB anfang")
            neuerTIFFname = srv_subdirBaulsten & "/" & gemarkung & "/" & baulastblatnr.Trim & endung
            'update " & tools.srv_schema & "." & tools.srv_tablename & " set tiff2='fkat/baulasten/' || trim(gemarkung) || '/' || trim(jahr_blattnr) || '.tiff'
            sql = "update " & tools.srv_schema & "." & tools.srv_tablename & " Set tiff='" & neuerTIFFname & "' where jahr_blattnr='" & baulastblatnr & "'"
            Dim dtRBplus As New DataTable
            Dim erfolg = sqlausfuehren(sql, fstREC.mydb, dtRBplus)
            l(" MOD updateGISDB ende")
            Return erfolg
        Catch ex As Exception
            l("Fehler in updateGISDB: " & ex.ToString())
            Return False
        End Try
    End Function
End Class
