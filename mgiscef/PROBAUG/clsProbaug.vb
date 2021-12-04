Imports System.Data
Imports mgis

Public Class clsProbaug
    'modus=probaug suchmodus="adresse" gemeinde="dietzenbach" strasse="ahornweg" hausnr="22a"
    'modus=probaug suchmodus="adresse"  gemeinde="dietzenbach" strasse="ahornweg" hausnr="20" 
    'modus=probaug suchmodus="adresse"  gemeinde="dietzenbach" strasse="am rebstock" hausnr="42" 
    'modus=probaug suchmodus="adresse"  gemeinde="neu-isenburg" strasse="alexander-von-humboldt-strasse" hausnr="5" 
    'modus=probaug suchmodus="flurstueck" gemarkung="dietzenbach" flur="5" fstueck="490/0"
    'modus="probaug"  suchmodus="flurstueck" gemarkung="mühlheim" flur="11" fstueck="136/1"
    'modus="probaug"  suchmodus="flurstueck" gemarkung="ober-roden" flur="19" fstueck="149/0"
    Friend Shared Function sindProbaugSuchParamsOK(probaugSuchmodus As String, probaugAdresse As ParaAdresse, probaugFST As ParaFlurstueck) As Boolean
        Return True
    End Function

    Shared Function getAktrangeFromProbaug(probaugSuchmodus As String,
                                            probaugAdresse As ParaAdresse,
                                            probaugFST As ParaFlurstueck,
                                            ByRef errorHinweis As String) As clsRange
        Dim newrange As New clsRange
        Dim minradius As Integer
        Try
            l("getAktrangeFromProbaug---------------------- anfang")
            If probaugSuchmodus = "flurstueck" Then
                '               modus = probaug  suchmodus=flurstueck gemarkung="Hainhausen" flur="4" fstueck="387/1"
                ' modus = probaug  suchmodus=flurstueck gemarkung="mühlheim" flur="11" fstueck="136/1"
                'modus = probaug  suchmodus=adresse gemeinde="langen" strasse="im birkenwäldchen" hausnr="10"
                aktFST.clear()
                aktFST = CType(probaugFST.Clone, ParaFlurstueck)
                aktFST.normflst.FS = aktFST.normflst.buildFS
                FSGKrechtsGKHochwertHolen(aktFST.normflst, WinDetailSucheFST.AktuelleBasisTabelle)
                aktGlobPoint.strX = CType(CInt(aktFST.normflst.GKrechts), String)
                aktGlobPoint.strY = CType(CInt(aktFST.normflst.GKhoch), String)

                minradius = calcMinradius(aktFST.normflst.radius)
                'MsgBox(aktFST.normflst.GKrechts.ToString)
                If aktFST.normflst.GKrechts < 10 Then
                    'MsgBox("Flurstück konnte im GIS/ALKIS nicht gefunden werden: " & aktFST.defineAbstract)
                    errorHinweis = "Flurstück konnte im GIS/ALKIS nicht gefunden werden: " & aktFST.defineAbstract
                    Dim FSTvorschlaegeLike As New List(Of clsFlurstueck)
                    FSTvorschlaegeLike = getFSTVorschlaegeLike(aktFST)

                    Dim FSTvorschlaegeHist As New List(Of clsFlurstueck)
                    Dim count = setAktFst2HIST(aktFST)
                    If count > 0 Then

                        MessageBox.Show(" Gesucht: " &
                                clsString.Capitalize(aktFST.normflst.toShortstring(",")) & Environment.NewLine & Environment.NewLine &
                                        " Flurstück ist nicht mehr vorhanden." & Environment.NewLine &
                                        " Es konnte ein historisches Flurstück aus dem Jahre: " & aktFST.normflst.lastyear &
                                        " gefunden werden." & Environment.NewLine &
                                        " Es wird die Position des historischen Flurstücks angezeigt!.", "Historisches Flurstück gefunden aus: " & aktFST.normflst.lastyear)
                        FSGKrechtsGKHochwertHolen(aktFST.normflst, myglobalz.histFstView)
                        aktGlobPoint.strX = CType(CInt(aktFST.normflst.GKrechts), String)
                        aktGlobPoint.strY = CType(CInt(aktFST.normflst.GKhoch), String)
                        l("  historisches Flurstück wurde verwendet " & aktFST.normflst.toShortstring(",") & GisUser.nick & ", ")
                        minradius = calcMinradius(aktFST.normflst.radius)
                        aktFST.normflst.istHistorisch = True
                        getSerialFromPostgis(aktFST.normflst.FS, False, myglobalz.histFstView) ' setzt  aktFST.serial 

                    Else
                        Dim fstvor As New winProbaugFSTVorschlaege(FSTvorschlaegeLike, aktFST.normflst)
                        fstvor.ShowDialog()
                        '##########################
                        If fstvor.ausgewaehlt Then
                            aktFST.normflst = fstvor._gefundenesFST
                            FSGKrechtsGKHochwertHolen(aktFST.normflst, WinDetailSucheFST.AktuelleBasisTabelle)
                            aktGlobPoint.strX = CType(CInt(aktFST.normflst.GKrechts), String)
                            aktGlobPoint.strY = CType(CInt(aktFST.normflst.GKhoch), String)
                            minradius = calcMinradius(aktFST.normflst.radius)
                            getSerialFromPostgis(aktFST.normflst.FS, False, WinDetailSucheFST.AktuelleBasisTabelle) ' setzt  aktFST.serial 
                        Else
                            Return Nothing
                        End If
                    End If
                End If
                Return calcBbox(aktGlobPoint.strX, aktGlobPoint.strY, minradius) 'kartengen.aktMap.aktrange
            End If
            If probaugSuchmodus = "adresse" Then
                Debug.Print(probaugAdresse.Gisadresse.gemeindeName)
                Debug.Print(probaugAdresse.Gisadresse.strasseName)
                Debug.Print(probaugAdresse.Gisadresse.HausKombi)
                Dim radius As Double = 200

                If Not istGueltigeGemeindeImKrOF(probaugAdresse.Gisadresse.gemeindeName) Then
                    errorHinweis = probaugAdresse.Gisadresse.gemeindeName &
                       " ist keine gültige Gemeinde im Kreis Offenbach. " & Environment.NewLine &
                       "Bitte korrigieren sie den Eintrag im Probaug,  " & Environment.NewLine &
                       " und /  oder nutzen sie den Zugang über das Flurstück."
                    'MessageBox.Show(,
                    '                "Falsche Adressangabe", MessageBoxButton.OK, MessageBoxImage.Error)
                    Return clsStartup.setMapKreisRange()
                End If

                If adressSucheVolltreffer(probaugAdresse, radius, newrange) Then
                    'MessageBox.Show("Die Adresse konnte gefunden werden." & Environment.NewLine & Environment.NewLine &
                    'probaugAdresse.Gisadresse.toString & Environment.NewLine & Environment.NewLine &
                    '   " " & Environment.NewLine &
                    '   " ", "Adresssuche - Volltreffer", MessageBoxButton.OK, MessageBoxImage.Information)
                    errorHinweis = "Volltreffer"
                    Return newrange
                End If
                If adressSucheOhneHausnrzusatz(probaugAdresse, radius, newrange) Then
                    errorHinweis = "Der Zusatz zur Hausnummer konnte nicht gefunden werden." & Environment.NewLine &
                    probaugAdresse.Gisadresse.toString & Environment.NewLine & Environment.NewLine &
                       " Ersatzweise wurde die Hausnummer allein verwendet. . " & Environment.NewLine &
                       "Bitte korrigieren sie ggf. den Eintrag im Probaug,  " & Environment.NewLine &
                       " und /  oder nutzen sie den Zugang über das Flurstück."
                    ',
                    '                "Falsche Adressangabe", MessageBoxButton.OK, MessageBoxImage.Error)
                    Return newrange
                Else
                    Debug.Print("hausnr konnte nicht gefunden werden")
                    Dim adressVorschlage As New List(Of clsAdress)
                    If adressSucheOhneHausnr(probaugAdresse, radius, newrange, adressVorschlage) Then
                        Debug.Print("")
                        Dim adrev As New winProbaugAdressVorschlaege(adressVorschlage, probaugAdresse.Gisadresse)
                        adrev.ShowDialog()
                        If adrev.ausgewaehlt Then
                            Dim rechts = adrev.rechts
                            Dim hoch = adrev.hoch
                            '##################
                            newrange = calcBbox(CInt(rechts).ToString, CInt(hoch).ToString, CInt(radius))
                            aktGlobPoint.strX = CInt(rechts).ToString
                            aktGlobPoint.strY = CInt(hoch).ToString
                            aktadr.punkt.X = CDbl(CInt(aktGlobPoint.strX))
                            aktadr.punkt.Y = CDbl(CInt(aktGlobPoint.strY))
                            '####################
                            Return newrange
                        Else
                            Return Nothing
                        End If

                    End If
                End If
                errorHinweis = "Die Adresse konnte nicht gefunden werden:" & Environment.NewLine & Environment.NewLine &
                    probaugAdresse.Gisadresse.toString & Environment.NewLine & Environment.NewLine &
                     "Bitte korrigieren sie ggf. den Eintrag im Probaug,  " & Environment.NewLine &
                       " und /  oder nutzen sie den Zugang über das Flurstück."
                ',
                '                "Falsche Adressangabe", MessageBoxButton.OK, MessageBoxImage.Error)
                Return Nothing
            End If
            l("getAktrangeFromProbaug---------------------- ende fehlerhaft")
            Return Nothing
        Catch ex As Exception
            l("Fehler in getAktrangeFromProbaug: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Private Shared Function setAktFst2HIST(ByRef aktFST As ParaFlurstueck) As Integer
        'FS0607350040038700100 
        Dim at As New clsFlurstueck
        Try
            l("getFSTVorschlaege---------------------- anfang")

            Dim SQL = "SELECT * FROM " & myglobalz.histFstView & "  where fs = '" & aktFST.normflst.FS & "' order by jahr desc limit 1"
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            If dt.Rows.Count < 1 Then
                'keine ähnlichen Flurstücke vorhanden
                Return 0
            Else
                For i = 0 To dt.Rows.Count - 1
                    aktFST.clear()
                    aktFST.normflst.FS = (clsDBtools.fieldvalue(dt.Rows(i).Item("fs")))
                    aktFST.normflst.flur = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("flur")))
                    aktFST.normflst.zaehler = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("zaehler")))
                    aktFST.normflst.nenner = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("nenner")))
                    aktFST.normflst.geom = (clsDBtools.fieldvalue(dt.Rows(i).Item("geom")))
                    aktFST.normflst.lastyear = (clsDBtools.fieldvalue(dt.Rows(i).Item("jahr")))

                Next
                Return dt.Rows.Count
            End If
            l("getFSTVorschlaege---------------------- ende")
        Catch ex As Exception
            l("Fehler in getFSTVorschlaege: " & ex.ToString())
            Return 0
        End Try
    End Function

    Private Shared Function getFSTVorschlaegeLike(aktFST As ParaFlurstueck) As List(Of clsFlurstueck)
        'FS0607350040038700100
        Dim ffskurz As String
        Dim temp As New List(Of clsFlurstueck)
        Dim at As New clsFlurstueck
        Try
            l("getFSTVorschlaege---------------------- anfang")
            ffskurz = aktFST.normflst.FS.Substring(0, 15)
            Dim SQL = "SELECT * FROM " & WinDetailSucheFST.AktuelleBasisTabelle & " where fs like '" & ffskurz & "%' order by fs"
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            If dt.Rows.Count < 1 Then
                'keine ähnlichen Flurstücke vorhanden
                Return Nothing
            Else
                For i = 0 To dt.Rows.Count - 1
                    at = New clsFlurstueck
                    at.FS = (clsDBtools.fieldvalue(dt.Rows(i).Item("fs")))
                    at.flur = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("flur")))
                    at.zaehler = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("zaehler")))
                    at.nenner = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("nenner")))
                    at.geom = (clsDBtools.fieldvalue(dt.Rows(i).Item("geom")))
                    temp.Add(at)
                Next

            End If
            Return temp
            l("getFSTVorschlaege---------------------- ende")
        Catch ex As Exception
            l("Fehler in getFSTVorschlaege: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Shared Function adressSucheOhneHausnr(probaugAdresse As ParaAdresse, radius As Double,
                                                  newrange As clsRange,
                                                      ByRef adressVorschlage As List(Of clsAdress)) As Boolean
        Dim sql, test As String
        Dim temp As String = ""
        Dim adressVorschlaegeDT As DataTable

        Dim aktadr As New clsAdress
        Try
            probaugAdresse.Gisadresse.hauskombiZerlegen()
            l("adressSucheOhneHausnr---------------------- anfang")
            sql = "select * from flurkarte.haloschneise " &
                " where gemeindenr=" & probaugAdresse.Gisadresse.gemeindeNrBig() &
                " and normname='" & probaugAdresse.Gisadresse.strassennameNORM & "'"
            l("adressSucheOhneHausnr---------------------- ende")
            '-------------------------------------
            adressVorschlaegeDT = getDTFromWebgisDB(sql, "postgis20")
            If adressVorschlaegeDT.IsNothingOrEmpty Then
                Return False
            Else
                For i = 0 To adressVorschlaegeDT.Rows.Count - 1
                    test = clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("hausnr"))
                    aktadr = New clsAdress
                    temp = clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("strcode")) & "," &
                           clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("hausnr")) & "," &
                           clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("rechts"))

                    aktadr.gemeindebigNRstring = clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("gemeindenr"))
                    aktadr.strasseCode = CInt(clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("strcode")))
                    If test.IsNothingOrEmpty Then
                        aktadr.hausNr = 0
                    Else
                        aktadr.hausNr = CInt(clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("hausnr")))
                    End If
                    aktadr.hausZusatz = clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("zusatz"))
                    aktadr.GKrechts = CInt(clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("rechts")))
                    aktadr.GKhoch = CInt(clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("hoch")))
                    aktadr.strasseName = clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("sname")).Trim
                    aktadr.HausKombi = clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("hausnrkombi")).Trim
                    aktadr.geom = clsDBtools.fieldvalue(adressVorschlaegeDT.Rows(i).Item("geom"))
                    adressVorschlage.Add(aktadr)
                Next
            End If
            Return True
        Catch ex As Exception
            l("Fehler in adressSucheOhneHausnr: " & temp & Environment.NewLine &
                                                                          ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function adressSucheOhneHausnrzusatz(probaugAdresse As ParaAdresse, radius As Double, ByRef newrange As clsRange) As Boolean
        Dim sql As String

        Try
            probaugAdresse.Gisadresse.hauskombiZerlegen()
            l("adressSucheVolltreffer---------------------- anfang")
            sql = "select ST_EXTENT(geom) from flurkarte.halofs " &
                "where gemeindenr=" & probaugAdresse.Gisadresse.gemeindeNrBig() &
                "  and normname='" & probaugAdresse.Gisadresse.strassennameNORM & "'" &
                "  and hausnr=" & probaugAdresse.Gisadresse.hausNr
            Return dbAdrSuche(radius, newrange, sql)
            l("adressSucheVolltreffer---------------------- ende")
        Catch ex As Exception
            l("Fehler in adressSucheVolltreffer: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function istGueltigeGemeindeImKrOF(gemeindeName As String) As Boolean
        Try
            l("istGueltigeGemeindeImKrOF---------------------- anfang")
            Dim gemparms As New clsGemarkungsParams
            gemparms.init() : Dim result$ = "ERROR"
            Dim a = From item In gemparms.parms Where item.gemeindetext.ToLower = gemeindeName.Trim.ToLower Select item.gemeindenr
            If a.ToArray.Length > 0 Then result$ = a.ToList(0).ToString
            If result = "ERROR" Then
                Return False
            Else
                Return True
            End If
            l("istGueltigeGemeindeImKrOF---------------------- ende")
        Catch ex As Exception
            l("Fehler in istGueltigeGemeindeImKrOF: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function istGemarkung(gemeindeName As String) As Boolean
        Dim gemparms As New clsGemarkungsParams
        gemparms.init() : Dim result$ = "ERROR"
        Dim a = From item In gemparms.parms Where item.gemarkungstext.ToLower = gemeindeName.Trim.ToLower Select item.gemeindetext
        If a.ToArray.Length > 0 Then result$ = a.ToList(0).ToString
        Return False
    End Function

    Private Shared Function getGemeindetext4Gemarkungstext(gemeindeName As String) As String
        'Dim gemparms As New clsGemarkungsParams
        'gemparms.init() : Dim result$ = "ERROR"
        '	Dim a = From item In gemparms.parms Where item.gemarkungstext.ToLower = "disetesheim" Select item.gemeindetext
        '	If a.ToArray.Length > 0 Then result$ = a.ToList(0).ToString
        Return gemeindeName
    End Function
    Private Shared Function adressSucheVolltreffer(probaugAdresse As ParaAdresse, radius As Double,
                                                  ByRef newrange As clsRange) As Boolean
        Dim sql As String
        Try
            l("adressSucheVolltreffer---------------------- anfang")
            sql = "select ST_EXTENT(geom) from flurkarte.halofs " &
                "where gemeindenr=" & probaugAdresse.Gisadresse.gemeindeNrBig() &
                "  and normname='" & probaugAdresse.Gisadresse.strassennameNORM & "'" &
                "  and lower(hausnrkombi)='" & probaugAdresse.Gisadresse.HausKombi & "'"
            Return dbAdrSuche(radius, newrange, sql)
            l("adressSucheVolltreffer---------------------- ende")
        Catch ex As Exception
            l("Fehler in adressSucheVolltreffer: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Shared Function dbAdrSuche(radius As Double, ByRef newrange As clsRange, sql As String) As Boolean
        Dim result As String = "", hinweis As String = ""
        Dim boxstring As String
        Dim xl, xh, yl, yh As Double
        Try
            l("dbAdrSuche---------------------- anfang")
            If iminternet Or CGIstattDBzugriff Then
                result = clsToolsAllg.getSQL4Http(sql, "postgis20", hinweis, "getsql") : l(hinweis)
                result = result.Replace("$", "").Replace(vbCrLf, "").Trim
                If result.IsNothingOrEmpty Then
                    Return False
                Else
                    boxstring = result.Trim
                    If postgisBOX2range(boxstring, xl, xh, yl, yh) Then
                        newrange = calcBbox(CInt(xl).ToString, CInt(yl).ToString, CInt(radius))
                        aktGlobPoint.strX = CInt(xl).ToString
                        aktGlobPoint.strY = CInt(yl).ToString
                        aktadr.punkt.X = CDbl(CInt(aktGlobPoint.strX))
                        aktadr.punkt.Y = CDbl(CInt(aktGlobPoint.strY))
                        Return True
                    Else
                        Return False
                    End If
                End If
            Else
                Dim dt As New DataTable
                dt = getDTFromWebgisDB(sql, "postgis20")
                If dt.IsNothingOrEmpty Then
                    Return False
                Else
                    boxstring = clsDBtools.fieldvalue(dt.Rows(0).Item(0)).ToString
                    If postgisBOX2range(boxstring, xl, xh, yl, yh) Then
                        newrange = calcBbox(CInt(xl).ToString, CInt(yl).ToString, CInt(radius))
                        aktGlobPoint.strX = CInt(xl).ToString
                        aktGlobPoint.strY = CInt(yl).ToString
                        aktadr.punkt.X = CDbl(CInt(aktGlobPoint.strX))
                        aktadr.punkt.Y = CDbl(CInt(aktGlobPoint.strY))
                        Return True
                    Else
                        Return False
                    End If
                End If
            End If
            Return True
            l("dbAdrSuche---------------------- ende")
        Catch ex As Exception
            l("Fehler in dbAdrSuche: " & ex.ToString())
            Return False
        End Try
    End Function

    'Function makeWKT4Geom() As String
    '    Dim basisrec As New clsDBspecPG
    '    Dim hinweis As String = ""
    '    Try
    '        basisrec.mydb = CType(fstREC.mydb.Clone, clsDatenbankZugriff)
    '        basisrec.mydb.SQL = "SELECT ST_EXTENT(geom) FROM " & aktSchema & prefix & aktTabelle & " where fs='" & fs & "'"
    '        hinweis = basisrec.getDataDT()
    '        If basisrec.dt.Rows.Count < 1 Then
    '            Return ""
    '        Else
    '            Dim koords As String = clsDBtools.fieldvalue(basisrec.dt.Rows(0).Item(0))
    '            Return koords
    '        End If
    '    Catch ex As Exception
    '        l("fehler in holeBoxKoordinatenFuerFS: " ,ex)
    '        Return ""
    '    End Try
    'End Function
End Class
