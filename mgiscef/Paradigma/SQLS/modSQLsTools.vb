Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports mgis

Module modSQLsTools
    'Friend Function getDTFromParadigmaDBsqls(queryString As String) As DataTable
    '    l("getDTFromParadigmaDB-------------------------")
    '    'Dim eigSDB As New clsEigentuemerSQLS
    '    Dim dt As DataTable
    '    Dim hinweis As String
    '    Try
    '        dt = modgetdt4sql.getDT4Query(queryString, paradigmadokREC, hinweis)
    '        Return dt
    '    Catch ex As Exception
    '        l("fehler in getDTFromParadigmaDB ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    Friend Function checkin_DokumenteDB(relativpfad As String, beschreibung As String, originalFullname As String,
                                          originalName As String, dateidatum As Date, vid As String, eid As Integer,
                                          newsavemode As Boolean) As Integer
        Dim returnIdentity As Boolean = True
        Dim querie As String
        Try
            l("checkin_DokumenteDB---------------------- anfang")
            clsSqlparam.paramListe.Clear()
            querie = "INSERT INTO  dokumente (RELATIVPFAD,DATEINAMEEXT,TYP,BESCHREIBUNG,CHECKINDATUM,FILEDATUM,EXIFDATUM,EXIFLONG,EXIFLAT,EXIFDIR," +
                            "EXIFHERSTELLER,ORIGINALFULLNAME,INITIAL_,REVISIONSSICHER,ORIGINALNAME,VID,EID,NEWSAVEMODE) " +
                 " VALUES (@RELATIVPFAD,@DATEINAMEEXT,@TYP,@BESCHREIBUNG,@CHECKINDATUM,@FILEDATUM,@EXIFDATUM,@EXIFLONG,@EXIFLAT,@EXIFDIR," +
                           "@EXIFHERSTELLER,@ORIGINALFULLNAME,@INITIAL_,@REVISIONSSICHER,@ORIGINALNAME,@VID,@EID,@NEWSAVEMODE)"
            dateidatum = Now
            Dim fo As New IO.FileInfo(originalFullname)
            'clsSqlparam.paramListe.Add(New clsSqlparam("USERNAME", username.ToLower.Trim)) 'MYGLObalz.sitzung.VorgangsID)
            'clsSqlparam.paramListe.Add(New clsSqlparam("ABTEILUNG", ABTEILUNG.ToLower.Trim))
            Dim revisionssicher As Boolean = False
            Dim extension As String
            extension = GetExtension(fo)
            populateDokumente(relativpfad, beschreibung, originalFullname, originalName, dateidatum, vid, eid, newsavemode, fo, revisionssicher, extension)

            fo = Nothing

            Dim ID = paradigmaMsql.manipquerie(querie, clsSqlparam.paramListe, True, "dokumentid")
            If ID > 0 Then Return ID Else Return 0
            l("checkin_DokumenteDB---------------------- ende")
        Catch ex As Exception
            l("Fehler in checkin_DokumenteDB: " & ex.ToString())
            Return -1
        End Try
    End Function

    Private Sub populateDokumente(relativpfad As String, beschreibung As String, originalFullname As String, originalName As String, dateidatum As Date, vid As String, eid As Integer, newsavemode As Boolean, fo As FileInfo, revisionssicher As Boolean, extension As String)
        clsSqlparam.paramListe.Add(New clsSqlparam("RELATIVPFAD", relativpfad$.Replace("\", "/")))
        clsSqlparam.paramListe.Add(New clsSqlparam("DATEINAMEEXT", fo.Name))
        clsSqlparam.paramListe.Add(New clsSqlparam("TYP", extension))
        clsSqlparam.paramListe.Add(New clsSqlparam("BESCHREIBUNG", beschreibung))
        clsSqlparam.paramListe.Add(New clsSqlparam("CHECKINDATUM", DateTime.Now()))
        clsSqlparam.paramListe.Add(New clsSqlparam("FILEDATUM", dateidatum))
        clsSqlparam.paramListe.Add(New clsSqlparam("EXIFDATUM", Now))
        clsSqlparam.paramListe.Add(New clsSqlparam("EXIFLONG", ""))
        clsSqlparam.paramListe.Add(New clsSqlparam("EXIFLAT", ""))
        clsSqlparam.paramListe.Add(New clsSqlparam("EXIFDIR", ""))
        clsSqlparam.paramListe.Add(New clsSqlparam("EXIFHERSTELLER", ""))
        clsSqlparam.paramListe.Add(New clsSqlparam("ORIGINALFULLNAME", originalFullname))
        clsSqlparam.paramListe.Add(New clsSqlparam("INITIAL_", getInitial(GisUser.nick.ToLower)))
        clsSqlparam.paramListe.Add(New clsSqlparam("REVISIONSSICHER", CInt(revisionssicher)))
        clsSqlparam.paramListe.Add(New clsSqlparam("NEWSAVEMODE", CInt(newsavemode)))
        clsSqlparam.paramListe.Add(New clsSqlparam("ORIGINALNAME", originalName))
        clsSqlparam.paramListe.Add(New clsSqlparam("VID", vid))
        clsSqlparam.paramListe.Add(New clsSqlparam("EID", eid))
    End Sub


    Private Sub setSQLParamsDOKSQLS(com As SqlCommand, relativpfad As String, fo As FileInfo, beschreibung As String,
                                 originalFullname As String, originalName As String, fi As FileInfo, revisionssicher As Boolean,
                                 dateidatum As Date, vid As Integer, eid As Object, nEWSAVEMODE As Object)
        Dim extension As String
        extension = GetExtension(fo)
        com.Parameters.AddWithValue("@RELATIVPFAD", relativpfad$.Replace("\", "/"))
        com.Parameters.AddWithValue("@DATEINAMEEXT", fi.Name)
        com.Parameters.AddWithValue("@TYP", extension)
        com.Parameters.AddWithValue("@BESCHREIBUNG", beschreibung)
        com.Parameters.AddWithValue("@CHECKINDATUM", DateTime.Now())
        com.Parameters.AddWithValue("@FILEDATUM", dateidatum)
        com.Parameters.AddWithValue("@EXIFDATUM", Now)
        com.Parameters.AddWithValue("@EXIFLONG", "")
        com.Parameters.AddWithValue("@EXIFLAT", "")
        com.Parameters.AddWithValue("@EXIFDIR", "")
        com.Parameters.AddWithValue("@EXIFHERSTELLER", "")
        com.Parameters.AddWithValue("@ORIGINALFULLNAME", originalFullname)
        com.Parameters.AddWithValue("@INITIAL_", getInitial(GisUser.nick.ToLower))
        com.Parameters.AddWithValue("@REVISIONSSICHER", CInt(revisionssicher))
        com.Parameters.AddWithValue("@NEWSAVEMODE", CInt(nEWSAVEMODE))
        com.Parameters.AddWithValue("@ORIGINALNAME", originalName)
        com.Parameters.AddWithValue("@VID", vid)
        com.Parameters.AddWithValue("@EID", eid)
    End Sub

    Private Sub setSQLParamsRBSQLS(ByVal com As SqlCommand, ByVal aktrb As iRaumbezug, ByVal rid As Integer)
        Try
            l("setSQLParamsRB ---------------------- anfang")
            com.Parameters.AddWithValue("@TYP", aktrb.typ)
            com.Parameters.AddWithValue("@SEKID", aktrb.SekID)
            com.Parameters.AddWithValue("@TITEL", aktrb.name.Trim)
            com.Parameters.AddWithValue("@ABSTRACT", aktrb.abstract.Trim)
            com.Parameters.AddWithValue("@RECHTS", CInt(aktrb.punkt.X))
            com.Parameters.AddWithValue("@HOCH", CInt(aktrb.punkt.Y))
            com.Parameters.AddWithValue("@XMIN", CInt(aktrb.box.xl))
            com.Parameters.AddWithValue("@XMAX", CInt(aktrb.box.xh))
            com.Parameters.AddWithValue("@YMIN", CInt(aktrb.box.yl))
            com.Parameters.AddWithValue("@YMAX", CInt(aktrb.box.yh))
            com.Parameters.AddWithValue("@FREITEXT", CStr(aktrb.Freitext).Trim)
            com.Parameters.AddWithValue("@ISMAPENABLED", Convert.ToInt16(aktrb.isMapEnabled))
            com.Parameters.AddWithValue("@FLAECHEQM", CInt(aktrb.FLAECHEQM))
            com.Parameters.AddWithValue("@LAENGEM", CInt(aktrb.LAENGEM))
            com.Parameters.AddWithValue("@MITETIKETT", CInt(aktrb.MITETIKETT))
            l("setSQLParamsRB---------------------- ende")
        Catch ex As Exception
            l("Fehler in setSQLParamsRB: ", ex)
        End Try
    End Sub



    Friend Function Raumbezug_abspeichern_Neu_alleDBsqls(aktrb As iRaumbezug) As Integer
        Dim querie As String
        ' Dim ID As Integer
        Dim returnIdentity As Boolean = True
        Try
            l("Raumbezug_abspeichern_Neu_alleDBsqls---------------------- anfang")
            clsSqlparam.paramListe.Clear()
            querie = "INSERT INTO raumbezug (TYP,SEKID,TITEL,ABSTRACT,RECHTS,HOCH," &
                                            " XMIN,XMAX,YMIN,YMAX,FREITEXT,ISMAPENABLED,FLAECHEQM,LAENGEM,MITETIKETT) " +
                                            " VALUES (@TYP,@SEKID,@TITEL,@ABSTRACT,@RECHTS,@HOCH," &
                                            "@XMIN,@XMAX,@YMIN,@YMAX,@FREITEXT,@ISMAPENABLED,@FLAECHEQM,@LAENGEM,@MITETIKETT)"

            polulateRaumbezug(aktrb)
            Dim ID = paradigmaMsql.manipquerie(querie, clsSqlparam.paramListe, True, "raumbezugsid")
            If ID > 0 Then Return ID Else Return 0
            l("Raumbezug_abspeichern_Neu_alleDBsqls---------------------- ende")
        Catch ex As Exception
            l("Fehler in Raumbezug_abspeichern_Neu_alleDBsqls: " & ex.ToString())
            Return -1
        End Try
    End Function

    Private Sub polulateRaumbezug(aktrb As iRaumbezug)
        l("polulateRaumbezug ---------------------- anfang")
        clsSqlparam.paramListe.Add(New clsSqlparam("TYP", aktrb.typ))
        clsSqlparam.paramListe.Add(New clsSqlparam("SEKID", aktrb.SekID))
        clsSqlparam.paramListe.Add(New clsSqlparam("TITEL", aktrb.name.Trim))
        clsSqlparam.paramListe.Add(New clsSqlparam("ABSTRACT", aktrb.abstract.Trim))
        clsSqlparam.paramListe.Add(New clsSqlparam("RECHTS", CInt(aktrb.punkt.X)))
        clsSqlparam.paramListe.Add(New clsSqlparam("HOCH", CInt(aktrb.punkt.Y)))
        clsSqlparam.paramListe.Add(New clsSqlparam("XMIN", CInt(aktrb.box.xl)))
        clsSqlparam.paramListe.Add(New clsSqlparam("XMAX", CInt(aktrb.box.xh)))
        clsSqlparam.paramListe.Add(New clsSqlparam("YMIN", CInt(aktrb.box.yl)))
        clsSqlparam.paramListe.Add(New clsSqlparam("YMAX", CInt(aktrb.box.yh)))
        clsSqlparam.paramListe.Add(New clsSqlparam("FREITEXT", CStr(aktrb.Freitext).Trim))
        clsSqlparam.paramListe.Add(New clsSqlparam("ISMAPENABLED", Convert.ToInt16(aktrb.isMapEnabled)))
        clsSqlparam.paramListe.Add(New clsSqlparam("FLAECHEQM", CInt(aktrb.FLAECHEQM)))
        clsSqlparam.paramListe.Add(New clsSqlparam("LAENGEM", CInt(aktrb.LAENGEM)))
        clsSqlparam.paramListe.Add(New clsSqlparam("MITETIKETT", CInt(aktrb.MITETIKETT)))
        l("polulateRaumbezug---------------------- ende")
    End Sub

    Friend Function RB_Adresse_abspeichern_Neusqls() As Integer
        Dim querie As String
        ' Dim ID As Integer
        Dim returnIdentity As Boolean = True
        Try
            l("RB_Adresse_abspeichern_Neusqls---------------------- anfang")
            clsSqlparam.paramListe.Clear()
            querie = "INSERT INTO ParaAdresse (GEMEINDENR,GEMEINDETEXT,STRASSENNAME,STRCODE,FS,HAUSNRKOMBI,PLZ,POSTFACH,ADRESSTYP) " +
                                  " VALUES (@GEMEINDENR,@GEMEINDETEXT,@STRASSENNAME,@STRCODE,@FS,@HAUSNRKOMBI,@PLZ,@POSTFACH,@ADRESSTYP)"

            'clsSqlparam.paramListe.Add(New clsSqlparam("USERNAME", username.ToLower.Trim)) 'MYGLObalz.sitzung.VorgangsID)


            populateParaadresse()

            Dim ID = paradigmaMsql.manipquerie(querie, clsSqlparam.paramListe, True, "id")
            If ID > 0 Then Return ID Else Return 0
            l("RB_Adresse_abspeichern_Neusqls---------------------- ende")
        Catch ex As Exception
            l("Fehler in RB_Adresse_abspeichern_Neusqls: " & ex.ToString())
            Return -1
        End Try
    End Function

    Private Sub populateParaadresse()
        Try
            clsSqlparam.paramListe.Add(New clsSqlparam("GEMEINDENR", aktadr.Gisadresse.gemeindeNrBig()))
            clsSqlparam.paramListe.Add(New clsSqlparam("GEMEINDETEXT", aktadr.Gisadresse.gemeindeName.Trim))
            clsSqlparam.paramListe.Add(New clsSqlparam("STRASSENNAME", aktadr.Gisadresse.strasseName.Trim))
            clsSqlparam.paramListe.Add(New clsSqlparam("STRCODE", aktadr.Gisadresse.strasseCode))
            clsSqlparam.paramListe.Add(New clsSqlparam("FS", aktadr.FS))
            clsSqlparam.paramListe.Add(New clsSqlparam("HAUSNRKOMBI", aktadr.Gisadresse.HausKombi))
            If aktadr.PLZ.IsNothingOrEmpty Then
                aktadr.PLZ = "0"
            End If
            clsSqlparam.paramListe.Add(New clsSqlparam("PLZ", CInt(aktadr.PLZ)))
            clsSqlparam.paramListe.Add(New clsSqlparam("POSTFACH", aktadr.Postfach))
            clsSqlparam.paramListe.Add(New clsSqlparam("ADRESSTYP", CInt(aktadr.Adresstyp)))
        Catch ex As Exception
            nachricht("Fehler beim populateParaadresse: ", ex)
        End Try
    End Sub

    Private Sub SETSQLPARAMSADRESSERBsqls(com As SqlCommand, v As Integer)
        Try
            com.Parameters.AddWithValue("@GEMEINDENR", aktadr.Gisadresse.gemeindeNrBig())
            com.Parameters.AddWithValue("@GEMEINDETEXT", aktadr.Gisadresse.gemeindeName.Trim)
            com.Parameters.AddWithValue("@STRASSENNAME", aktadr.Gisadresse.strasseName.Trim)
            com.Parameters.AddWithValue("@STRCODE", aktadr.Gisadresse.strasseCode)
            com.Parameters.AddWithValue("@FS", aktadr.FS)
            com.Parameters.AddWithValue("@HAUSNRKOMBI", aktadr.Gisadresse.HausKombi)
            If aktadr.PLZ.IsNothingOrEmpty Then
                aktadr.PLZ = "0"
            End If
            com.Parameters.AddWithValue("@PLZ", CInt(aktadr.PLZ))
            com.Parameters.AddWithValue("@POSTFACH", aktadr.Postfach)
            com.Parameters.AddWithValue("@ADRESSTYP", CInt(aktadr.Adresstyp))
        Catch ex As Exception
            nachricht("Fehler beim SETSQLPARAMSADRESSERB: ", ex)
        End Try
    End Sub
    Friend Function Koppelung_Raumbezug_VorgangSqls(rid As Integer, vid As Integer, status As Integer) As Integer
        Dim querie As String
        ' Dim ID As Integer
        Dim returnIdentity As Boolean = True
        Try
            l("Koppelung_Raumbezug_VorgangSqls---------------------- anfang")
            clsSqlparam.paramListe.Clear()
            querie = "INSERT INTO Raumbezug2Vorgang   " &
                  " (RAUMBEZUGSID,VORGANGSID,STATUS) VALUES (@RAUMBEZUGSID,@VORGANGSID,@STATUS) "
            'clsSqlparam.paramListe.Add(New clsSqlparam("USERNAME", username.ToLower.Trim)) 'MYGLObalz.sitzung.VorgangsID)
            clsSqlparam.paramListe.Add(New clsSqlparam("VORGANGSID", vid))
            clsSqlparam.paramListe.Add(New clsSqlparam("RAUMBEZUGSID", rid))
            clsSqlparam.paramListe.Add(New clsSqlparam("STATUS", status))
            Dim ID = paradigmaMsql.manipquerie(querie, clsSqlparam.paramListe, True, "ID")
            If ID > 0 Then Return ID Else Return 0
            l("Koppelung_Raumbezug_VorgangSqls---------------------- ende")
        Catch ex As Exception
            l("Fehler in Koppelung_Raumbezug_VorgangSqls: " & ex.ToString())
            Return -1
        End Try
    End Function

    Friend Function RB_FLST_Serial_abspeichern_Neusqls(vid As Integer, rid As Integer, serial As String, typ As RaumbezugsTyp,
                                                       area As Double) As Integer
        Dim querie As String
        ' Dim ID As Integer
        Dim returnIdentity As Boolean = True
        Try
            l("RB_FLST_Serial_abspeichern_Neusqls---------------------- anfang")
            clsSqlparam.paramListe.Clear()
            querie = "INSERT INTO RAUMBEZUG2GEOPOLYGON (RAUMBEZUGSID,VORGANGSID,TYP,AREAQM,SERIALSHAPE) " +
                                         " VALUES (@RAUMBEZUGSID,@VORGANGSID,@TYP,@AREAQM,@SERIALSHAPE)"
            'clsSqlparam.paramListe.Add(New clsSqlparam("USERNAME", username.ToLower.Trim)) 'MYGLObalz.sitzung.VorgangsID)
            clsSqlparam.paramListe.Add(New clsSqlparam("RAUMBEZUGSID", rid))
            clsSqlparam.paramListe.Add(New clsSqlparam("VORGANGSID", vid))
            clsSqlparam.paramListe.Add(New clsSqlparam("SERIALSHAPE", serial))
            clsSqlparam.paramListe.Add(New clsSqlparam("TYP", typ))
            clsSqlparam.paramListe.Add(New clsSqlparam("AREAQM", area))
            Dim ID = paradigmaMsql.manipquerie(querie, clsSqlparam.paramListe, True, "ID")
            If ID > 0 Then Return ID Else Return 0
            l("RB_FLST_Serial_abspeichern_Neusqls---------------------- ende")
        Catch ex As Exception
            l("Fehler in RB_FLST_Serial_abspeichern_Neusqls: " & ex.ToString())
            Return -1
        End Try
    End Function

    Private Sub setSQLParamsFLST_serial(com As SqlCommand, vid As Integer, rbid As Object, serial As String,
                                        areaqm As Integer, typ As RaumbezugsTyp, area As Double)
        com.Parameters.AddWithValue("@RAUMBEZUGSID", rbid)
        com.Parameters.AddWithValue("@VORGANGSID", vid)
        com.Parameters.AddWithValue("@SERIALSHAPE", serial)
        com.Parameters.AddWithValue("@TYP", typ)
        com.Parameters.AddWithValue("@AREAQM", areaqm)
    End Sub

    Friend Function deleteRaumbezugSqls(rid As Integer, vid As Integer) As Boolean
        Dim anzahlTreffer& = 0, hinweis$ = "", newid& = 0
        Dim querie As String
        Try
            querie = "delete from raumbezug where RAUMBEZUGSID=" & rid
            paradigmaMsql.dt = getDT4Query(querie, paradigmaMsql, hinweis)
            If paradigmaMsql.dt IsNot Nothing Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            nachricht("Fehler beim Abspeichern: ", ex)
            Return False
        End Try
    End Function



    Friend Function deleteRaumbezug2allOraclesqls(rid As Integer, vid As Integer,
                                             tablename As String) As Boolean
        Dim anzahlTreffer& = 0, hinweis$ = "", newid& = 0
        Dim querie As String
        Try
            'querie = "delete from " & tablename & " where RAUMBEZUGSID=:RAUMBEZUGSID and VORGANGSID=:VORGANGSID"

            querie = "delete from " & tablename & " where RAUMBEZUGSID=" & rid & " and vorgangsid=" & vid
            paradigmaMsql.dt = getDT4Query(querie, paradigmaMsql, hinweis)
            Return True
        Catch ex As Exception
            nachricht("Fehler beim Abspeichern: ", ex)
            Return False
        End Try
    End Function


    Friend Function RB_FLST_abspeichern_Neusqls() As Integer
        Dim querie As String
        ' Dim ID As Integer
        Dim returnIdentity As Boolean = True
        Try
            l("RB_FLST_abspeichern_Neusqls---------------------- anfang")
            clsSqlparam.paramListe.Clear()
            querie = "INSERT INTO ParaFlurstueck (GEMCODE,FLUR,ZAEHLER,NENNER,ZNKOMBI,GEMARKUNGSTEXT,FS,FLAECHEQM) " +
                            " VALUES (@GEMCODE,@FLUR,@ZAEHLER,@NENNER,@ZNKOMBI,@GEMARKUNGSTEXT,@FS,@FLAECHEQM)"
            'clsSqlparam.paramListe.Add(New clsSqlparam("USERNAME", username.ToLower.Trim))
            clsSqlparam.paramListe.Add(New clsSqlparam("GEMCODE", aktFST.normflst.gemcode))
            clsSqlparam.paramListe.Add(New clsSqlparam("FLUR", aktFST.normflst.flur))
            clsSqlparam.paramListe.Add(New clsSqlparam("ZAEHLER", aktFST.normflst.zaehler))
            clsSqlparam.paramListe.Add(New clsSqlparam("NENNER", aktFST.normflst.nenner))
            clsSqlparam.paramListe.Add(New clsSqlparam("ZNKOMBI", aktFST.normflst.fstueckKombi))
            clsSqlparam.paramListe.Add(New clsSqlparam("GEMARKUNGSTEXT", aktFST.normflst.gemarkungstext))
            clsSqlparam.paramListe.Add(New clsSqlparam("FS", aktFST.normflst.FS))
            clsSqlparam.paramListe.Add(New clsSqlparam("FLAECHEQM", aktFST.normflst.flaecheqm))


            Dim ID = paradigmaMsql.manipquerie(querie, clsSqlparam.paramListe, True, "ID")
            If ID > 0 Then Return ID Else Return 0
            l("RB_FLST_abspeichern_Neusqls---------------------- ende")
        Catch ex As Exception
            l("Fehler in RB_FLST_abspeichern_Neusqls: " & ex.ToString())
            Return -1
        End Try
    End Function

    Friend Function RB_Umkreis_abspeichern_Neusqls(aktPMU As clsParaUmkreis) As Integer
        Dim querie As String
        ' Dim ID As Integer
        Dim returnIdentity As Boolean = True
        Try
            l("RB_Umkreis_abspeichern_Neusqls---------------------- anfang")
            clsSqlparam.paramListe.Clear()
            querie = "INSERT INTO Paraumkreis (RADIUSM,BESCHREIBUNG) " +
                      " VALUES (@RADIUSM,@BESCHREIBUNG )"
            'clsSqlparam.paramListe.Add(New clsSqlparam("USERNAME", username.ToLower.Trim))
            clsSqlparam.paramListe.Add(New clsSqlparam("RADIUSM", aktPMU.Radius))
            clsSqlparam.paramListe.Add(New clsSqlparam("BESCHREIBUNG", aktPMU.Name))
            Dim ID = paradigmaMsql.manipquerie(querie, clsSqlparam.paramListe, True, "ID")
            If ID > 0 Then Return ID Else Return 0
            l("RB_Umkreis_abspeichern_Neusqls---------------------- ende")
        Catch ex As Exception
            l("RB_Umkreis_abspeichern_Neusqls in addUser: " & ex.ToString())
            Return -1
        End Try
    End Function
    Private Sub setSQLparamsUmkreisRB(com As SqlCommand, v As Integer, aktPMU As clsParaUmkreis)
        com.Parameters.AddWithValue("@RADIUSM", aktPMU.Radius)
        com.Parameters.AddWithValue("BESCHREIBUNG", aktPMU.Name)
    End Sub

    Private Sub SETSQLPARAMSFLSTsqls(com As SqlCommand)
        com.Parameters.AddWithValue("@GEMCODE", aktFST.normflst.gemcode)
        com.Parameters.AddWithValue("@FLUR", aktFST.normflst.flur)
        com.Parameters.AddWithValue("@ZAEHLER", aktFST.normflst.zaehler)
        com.Parameters.AddWithValue("@NENNER", aktFST.normflst.nenner)
        com.Parameters.AddWithValue("@ZNKOMBI", aktFST.normflst.fstueckKombi)
        com.Parameters.AddWithValue("@GEMARKUNGSTEXT", aktFST.normflst.gemarkungstext)
        com.Parameters.AddWithValue("@FS", aktFST.normflst.FS)
        com.Parameters.AddWithValue("@FLAECHEQM", aktFST.normflst.flaecheqm)
    End Sub
End Module
