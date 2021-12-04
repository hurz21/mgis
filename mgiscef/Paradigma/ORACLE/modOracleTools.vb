Imports System.Data
Imports Devart.Data.Oracle
Module modOracleTools
    'Friend Function getDTFromParadigmaDBOracle(queryString As String) As DataTable
    '    l("getDTFromParadigmaDB-------------------------")
    '    Dim dt As DataTable
    '    Try
    '        clsParadigmaDBOracle.oeffneconnParadigmaDBOracle()
    '        dt = clsParadigmaDBOracle.getDt(queryString)
    '        clsParadigmaDBOracle.paradigmaDBschliessen()
    '        Return dt
    '    Catch ex As Exception
    '        l("fehler in getDTFromParadigmaDB ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    'Friend Function checkin_DokumenteOracle(relativpfad As String, beschreibung As String, originalFullname As String, originalName As String,
    '                              dateidatum As Date, aktvorgangsid As String, ereignisid As Integer, NEWSAVEMODE As Boolean) As Integer
    '    Dim anzahlTreffer& = 0
    '    Dim com As OracleCommand
    '    Dim SQLupdate$ = ""
    '    Dim hinweis As String = ""

    '    Dim newid& = -1

    '    Dim fo As New IO.FileInfo(originalFullname)
    '    Dim pradigmaDB As New clsParadigmaDBOracle
    '    Dim dt As DataTable
    '    Try
    '        SQLupdate$ = "INSERT INTO  dokumente (RELATIVPFAD,DATEINAMEEXT,TYP,BESCHREIBUNG,CHECKINDATUM,FILEDATUM,EXIFDATUM,EXIFLONG,EXIFLAT,EXIFDIR," +
    '                        "EXIFHERSTELLER,ORIGINALFULLNAME,INITIAL_,REVISIONSSICHER,ORIGINALNAME,VID,EID,NEWSAVEMODE) " +
    '             " VALUES (:RELATIVPFAD,:DATEINAMEEXT,:TYP,:BESCHREIBUNG,:CHECKINDATUM,:FILEDATUM,:EXIFDATUM,:EXIFLONG,:EXIFLAT,:EXIFDIR," +
    '                       ":EXIFHERSTELLER,:ORIGINALFULLNAME,:INITIAL_,:REVISIONSSICHER,:ORIGINALNAME,:VID,:EID,:NEWSAVEMODE)"
    '        SQLupdate$ = SQLupdate$ & " RETURNING DOKUMENTID INTO :R1"

    '        nachricht("nach setSQLbody : " & SQLupdate)
    '        pradigmaDB.oeffneconnParadigmaDBOracle()

    '        com = New OracleCommand(SQLupdate, pradigmaDB.connParadigmaDBOracle)
    '        nachricht("vor setParams  ")
    '        'seteFiledatum(fi, dateidatum)
    '        dateidatum = Now
    '        setSQLParamsDOKORACLE(com, relativpfad, fo, beschreibung, originalFullname, originalName, fo, False, dateidatum,
    '                           CInt(aktvorgangsid), ereignisid, NEWSAVEMODE)
    '        newid = clsOracleIns.GetNewid(com, SQLupdate)

    '        pradigmaDB.paradigmaDBschliessen()
    '        fo = Nothing
    '        Return clsOracleIns.gebeNeuIDoderFehler(newid, SQLupdate)
    '    Catch ex As Exception
    '        nachricht("Fehler Dok1  beim Abspeichern: " & vbCrLf ,ex)
    '        'nachricht("Fehler Dok1  beim Abspeichern: " & ex)
    '        Return -2
    '    End Try
    'End Function
    Sub setSQLParamsDOKORACLE(ByVal com As OracleCommand,
                                ByVal relativpfad As String,
                            ByVal fi As IO.FileInfo,
                            ByVal Beschreibung As String,
                            ByVal OriginalFullname As String,
                            ByVal OriginalName As String,
                            ByVal fo As IO.FileInfo,
                            ByVal revisionssicher As Boolean,
                            ByVal dateidatum As Date,
                                    VID As Integer, EID As Integer,
                                    NEWSAVEMODE As Boolean)
        Dim extension As String
        extension = GetExtension(fo)
        com.Parameters.AddWithValue(":RELATIVPFAD", relativpfad$.Replace("\", "/"))
        com.Parameters.AddWithValue(":DATEINAMEEXT", fi.Name)
        com.Parameters.AddWithValue(":TYP", extension)
        com.Parameters.AddWithValue(":BESCHREIBUNG", Beschreibung)
        com.Parameters.AddWithValue(":CHECKINDATUM", DateTime.Now())
        com.Parameters.AddWithValue(":FILEDATUM", dateidatum)
        com.Parameters.AddWithValue(":EXIFDATUM", Now)
        com.Parameters.AddWithValue(":EXIFLONG", "")
        com.Parameters.AddWithValue(":EXIFLAT", "")
        com.Parameters.AddWithValue(":EXIFDIR", "")
        com.Parameters.AddWithValue(":EXIFHERSTELLER", "")
        com.Parameters.AddWithValue(":ORIGINALFULLNAME", OriginalFullname)
        com.Parameters.AddWithValue(":INITIAL_", getInitial(GisUser.nick.ToLower))
        com.Parameters.AddWithValue(":REVISIONSSICHER", CInt(revisionssicher))
        com.Parameters.AddWithValue(":NEWSAVEMODE", CInt(NEWSAVEMODE))
        com.Parameters.AddWithValue(":ORIGINALNAME", OriginalName)
        com.Parameters.AddWithValue(":VID", VID)
        com.Parameters.AddWithValue(":EID", EID)
    End Sub
End Module
