Module pgTools
    Public Sub initStrassenCombo()
        ' Dim a = "SELECT lage,bezeichnung,gml_id FROM halosort.lageschluessel where schluesselgesamt like ""06438001%"" order by bezeichnung"
        Dim schluesssellike As String
        schluesssellike = "06" & gemeindebigNRstring
        'adrREC.mydb.SQL =
        ' "SELECT schluesselgesamt,lage,bezeichnung,gml_id FROM public.lageschluessel " &
        ' " where schluesselgesamt like '" & schluesssellike & "%'" &
        ' " order by bezeichnung  "
        adrREC.mydb.SQL =
         "SELECT distinct trim(sname) as sname,strcode  FROM flurkarte.halofs " &
         " where gemeindeNR  = " & gemeindebigNRstring & "" &
         " order by  (sname)  "
        '  myGlobalz.adrREC.mydb.Schema = "halosort"
        Dim hinweis As String = adrREC.getDataDT()
    End Sub

    Sub initdb()
        adrREC.mydb = New clsDatenbankZugriff
        adrREC.mydb.Host = "gis"
        adrREC.mydb.username = "postgres" : adrREC.mydb.password = "lkof4"
        adrREC.mydb.Schema = "postgis20"
        adrREC.mydb.Tabelle = "flurkarte.basis_f" : adrREC.mydb.dbtyp = "postgis"

        fstREC.mydb = New clsDatenbankZugriff
        fstREC.mydb.Host = "gis"
        fstREC.mydb.username = "postgres" : fstREC.mydb.password = "lkof4"
        fstREC.mydb.Schema = "postgis20"
        fstREC.mydb.Tabelle = "flurkarte.basis_f" : fstREC.mydb.dbtyp = "postgis"

        webgisREC.mydb = New clsDatenbankZugriff
        webgisREC.mydb.Host = "gis"
        webgisREC.mydb.username = "postgres" : webgisREC.mydb.password = "lkof4"
        webgisREC.mydb.Schema = "webgiscontrol"
        webgisREC.mydb.Tabelle = "flurkarte.basis_f" : webgisREC.mydb.dbtyp = "postgis"


    End Sub


    Sub inithausnrCombo()
        'adrREC.mydb.SQL =
        ' "SELECT hausnummer,gml_id FROM albnas.ax_lagebezeichnungmithausnummer " &
        ' " where gemeinde = '" & gemeindestring & "'" &
        ' " and kreis = '38'" &
        ' " and lage ='" & strasseCode & "'" &
        ' " order by  abs(hausnummer)"

        adrREC.mydb.SQL =
         "SELECT hausnr,gml_id,rechts,hoch FROM flurkarte.halofs " &
         " where gemeindenr = '" & gemeindebigNRstring & "'" &
         " and strcode ='" & strcode & "'" &
         " order by  abs(hausnr)"

        Dim hinweis As String = adrREC.getDataDT()
    End Sub
    Private Sub inithausnrCombo2(ByVal gemeindestring As String, strasseCode As String)
        adrREC.mydb.SQL =
         "SELECT lageohnenr,gml_id FROM albnas.ax_lagebezeichnungohnehausnummer " &
         " where gemeinde = '" & gemeindestring & "'" &
         " and kreis = '38'" &
         " and lage ='" & strasseCode & "'" &
         " order by  abs(lageohnenr)"

        adrREC.mydb.Host = "kis"
        adrREC.mydb.Schema = "albnas"
        Dim hinweis As String = adrREC.getDataDT()
    End Sub
    Public Sub holeFlureDT()
        fstREC.mydb.SQL = "select distinct flur  from  flurkarte.basis_f " &
         " where gemcode = " & aktFST.gemcode &
         " order by flur "
        Dim hinweis As String = (fstREC.getDataDT())
        nachricht(hinweis)
    End Sub
    Public Sub holeNennerDT()
        fstREC.mydb.SQL = "select distinct nenner  from  flurkarte.basis_f " &
         " where gemcode = " & aktFST.gemcode &
         " and flur = " & aktFST.flur &
         " and zaehler = " & aktFST.zaehler &
         " order by nenner  "
        nachricht(fstREC.getDataDT())
    End Sub

    Public Sub holeZaehlerDT()
        fstREC.mydb.SQL = "select distinct zaehler  from flurkarte.basis_f " &
         " where gemcode = " & aktFST.gemcode &
         " and flur = " & aktFST.flur &
         " order by zaehler"
        nachricht(fstREC.getDataDT())
    End Sub
    Friend Sub rechtsHochwertHolen(aktFST As clsFlurstueck)
        Dim box As String = holeBoxKoordinatenFuerFS(aktFST.FS, "basis_f", "flurkarte")
        Dim xl, xh, yl, yh As Double
        If postgisBOX2range(box, xl, xh, yl, yh) Then
            aktFST.GKrechts = xl + ((xh - xl) / 2)
            aktFST.GKhoch = yl + ((yh - yl) / 2)
        Else
        End If
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
    Public Function holeBoxKoordinatenFuerFS(fs As String,
                                           aktTabelle As String,
                                           aktSchema As String,
                                           Optional ByVal fromview As Boolean = True) As String
        Dim prefix As String = ".v_" : If Not fromview Then prefix = "."
        prefix = "."
        Dim basisrec As New clsDBspecPG
        Dim hinweis As String = ""
        Try
            basisrec.mydb = CType(fstREC.mydb.Clone, clsDatenbankZugriff)
            basisrec.mydb.SQL = "SELECT ST_EXTENT(geom) FROM " & aktSchema & prefix & aktTabelle & " where fs='" & fs & "'"
            hinweis = basisrec.getDataDT()
            If basisrec.dt.Rows.Count < 1 Then
                Return ""
            Else
                Dim koords As String = clsDBtools.fieldvalue(basisrec.dt.Rows(0).Item(0))
                'gebucht = basisrec.dt.Rows(0).Item("istgebucht").ToString.Trim
                'zeigtauf = basisrec.dt.Rows(0).Item("zeigtauf").ToString.Trim
                'weistauf = basisrec.dt.Rows(0).Item("weistauf").ToString.Trim
                'areaqm = basisrec.dt.Rows(0).Item("gisarea").ToString.Trim
                Return koords
            End If
        Catch ex As Exception
            l("fehler in GetZusatzInfosAusBasis: " & ex.ToString)
        End Try
    End Function
End Module
