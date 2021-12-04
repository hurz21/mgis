
Module modMapserverlayer

    Private _ebenen1 As String

    Public Property KartenMapfileTemplate As String
    Property userlayeraidNKATDIR As String

    Public Property KartenHTMfileTemplate As String

    Public Property KartenMAPfile As String

    Public Property KartenIMGDir As String

    Public Property KartenVorgangsDir As String

    Public Property tellungShape As String

    Public Property Kartenprojektdir As String

    Public Property KartenJPGDir As String

    Public Property KartenRoot As String
    Public Property kartenebenenName As String
    Public Property ebenen As Integer()




    Function paradigmaXMLeinlesen() As Boolean
        l("in KarteErstellen ---------------------------")
        Dim paradigmaXML As String = "paradigma.xml" 'My.Resources.Resources.ParadigmaKonfigFile
        Try
            Dim testfile As New IO.FileInfo(paradigmaXML)
            If Not testfile.Exists Then
                MsgBox("Die Konfiguration konnte nicht gefunden werden")
                End
            End If
            iniDict = clsINIXML.XMLiniReader(paradigmaXML) '"g:\appsconfig\paradigma.xml")
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    'Sub ini_WebgisREC()
    '    With Webgis_MYDB
    '        .Host = CType(iniDict("WebgisDB.MySQLServer"), String)
    '        .Schema = CType(iniDict("WebgisDB.Schema"), String)
    '        .Tabelle = CType(iniDict("WebgisDB.Tabelle"), String)
    '        .ServiceName = CType(iniDict("WebgisDB.ServiceName"), String)
    '        .username = CType(iniDict("WebgisDB.username"), String)
    '        .password = CType(iniDict("WebgisDB.password"), String)
    '        .dbtyp = CType(iniDict("WebgisDB.dbtyp"), String) 
    '    End With
    'End Sub
    Sub ini_raumbezug()
#If DEBUG Then
        With Paradigma_MYDB
            .Host = CType(myglobalz.iniDict("VorgangDB.MySQLServer"), String)
            .Schema = CType(myglobalz.iniDict("VorgangDB.Schema"), String)
            .Tabelle = CType(myglobalz.iniDict("VorgangDB.Tabelle"), String)
            .ServiceName = CType(myglobalz.iniDict("VorgangDB.ServiceName"), String)
            .username = CType(myglobalz.iniDict("VorgangDB.username"), String)
            .password = CType(myglobalz.iniDict("VorgangDB.password"), String)
            .dbtyp = CType(myglobalz.iniDict("VorgangDB.dbtyp"), String)

            .Host = "msql01"
            .Schema = "Paradigma"
            .Tabelle = "paradigma.kreis-of.local"
            .ServiceName = "vorgang"
            .username = "sgis"
            .password = "WinterErschranzt.74"
            .dbtyp = "sqls"
        End With

        With Paradigma_MYDB
            .Host = CType(myglobalz.iniDict("VorgangDB.MySQLServer"), String)
            .Schema = CType(myglobalz.iniDict("VorgangDB.Schema"), String)
            .Tabelle = CType(myglobalz.iniDict("VorgangDB.Tabelle"), String)
            .ServiceName = CType(myglobalz.iniDict("VorgangDB.ServiceName"), String)
            .username = CType(myglobalz.iniDict("VorgangDB.username"), String)
            .password = CType(myglobalz.iniDict("VorgangDB.password"), String)
            .dbtyp = CType(myglobalz.iniDict("VorgangDB.dbtyp"), String)

            .Host = "msql01"
            .Schema = "Paradigma"
            .Tabelle = "paradigma.kreis-of.local"
            .ServiceName = "vorgang"
            .username = "sgis"
            .password = "WinterErschranzt.74"
            .dbtyp = "sqls"
        End With
        l("  Oracle_MYDB " & Paradigma_MYDB.tostring)
#Else
        l("vor paradigma" )
        With Paradigma_MYDB
            .Host = CType(myglobalz.iniDict("VorgangDB.MySQLServer"), String)
            .Schema = CType(myglobalz.iniDict("VorgangDB.Schema"), String)
            .Tabelle = CType(myglobalz.iniDict("VorgangDB.Tabelle"), String)
            .ServiceName = CType(myglobalz.iniDict("VorgangDB.ServiceName"), String)
            .username = CType(myglobalz.iniDict("VorgangDB.username"), String)
            .password = CType(myglobalz.iniDict("VorgangDB.password"), String)
            .dbtyp = CType(myglobalz.iniDict("VorgangDB.dbtyp"), String)
        End With
        l("  Oracle_MYDB " & Paradigma_MYDB.tostring)
#End If


    End Sub
    Sub ini_WebgisREC()
        l("vor ini_WebgisREC ")
        With Webgis_MYDB
            .Host = CType(iniDict("WebgisDB.MySQLServer"), String)
            .Schema = CType(iniDict("WebgisDB.Schema"), String)
            .Tabelle = CType(iniDict("WebgisDB.Tabelle"), String)
            .ServiceName = CType(iniDict("WebgisDB.ServiceName"), String)
            .username = CType(iniDict("WebgisDB.username"), String)
            .password = CType(iniDict("WebgisDB.password"), String)
            .dbtyp = CType(iniDict("WebgisDB.dbtyp"), String)
            '  webgisREC = CType(setDbRecTyp(Webgis_MYDB), LIBDB.IDB_grundfunktionen)
            ' webgisREC.mydb = CType(.Clone, LIBDB.clsDatenbankZugriff)
        End With
        l("Webgis_MYDB " & Webgis_MYDB.tostring)
    End Sub
    Public Sub VerzeichnisseEinrichten(kartenebenenName As String, aid As Integer, raumtyp As String)
        l("VerzeichnisseErreichnen")
        If raumtyp = "punkt" Or raumtyp = "" Then
            KartenMapfileTemplate = myglobalz.gis_serverD & "/paradigmacache/vorlagen/selection.map"
        End If
        If raumtyp = "flaeche" Then
            KartenMapfileTemplate = myglobalz.gis_serverD & "/paradigmacache/vorlagen/selection_flaeche.map"
        End If
        KartenHTMfileTemplate = myglobalz.gis_serverD & "/paradigmacache/vorlagen/raumbezug.htm"
        KartenRoot = myglobalz.gis_serverD & "\paradigmacache"
        KartenJPGDir = myglobalz.gis_serverD & "\paradigmacache\"
        Kartenprojektdir = KartenRoot & "\" & kartenebenenName
        '   tellungShape.kartenDatadir = Kartenprojektdir & "\data"
        KartenVorgangsDir = KartenRoot & "\VORGANG\"
        KartenIMGDir = Kartenprojektdir & "\images"
        KartenMAPfile = Kartenprojektdir & "\raumbezug.map"
        userlayeraidNKATDIR = myglobalz.gis_serverD & "\nkat\aid\" & aid & "\"
        l("VerzeichnisseErreichnen - ende")
    End Sub
    Public Sub Verzeichnisse_ausgeben()
        l("Ausgabe der verzeichnisse:---------------------------")
        l("shapeModul.KartenMapfileTemplate " & KartenMapfileTemplate)
        l(" _shapeModul.KartenRoot " & KartenRoot)
        l("_shapeModul.KartenJPGDir " & KartenJPGDir)
        l(" _shapeModul.Kartenprojektdir " & Kartenprojektdir)
        l(" _shapeModul.KartenVorgangsDir " & KartenVorgangsDir)
        l(" _shapeModul.KartenIMGDir " & KartenIMGDir)
        l("_shapeModul.KartenMAPfile " & KartenMAPfile)
        l("userlayeraidNKATDIR " & userlayeraidNKATDIR)

        l("Ausgabe der verzeichnisse:--------------------------- Ende")
    End Sub
    Public Sub Verzeichnisse_anlegen()
        glob2.nachricht("Verzeichnisse_anlegen------------------------------------")
        Try
            l("Verzeichnisse_anlegen---------------------- anfang")
            l(userlayeraidNKATDIR)
            If Not IO.Directory.Exists(userlayeraidNKATDIR) Then IO.Directory.CreateDirectory(userlayeraidNKATDIR)
            l(KartenRoot)
            If Not IO.Directory.Exists(KartenRoot) Then IO.Directory.CreateDirectory(KartenRoot)
            l(KartenVorgangsDir)
            If Not IO.Directory.Exists(KartenVorgangsDir) Then IO.Directory.CreateDirectory(KartenVorgangsDir)
            'If Not IO.Directory.Exists(kartenDatadir) Then IO.Directory.CreateDirectory(kartenDatadir)
            l(KartenJPGDir)
            If Not IO.Directory.Exists(KartenJPGDir) Then IO.Directory.CreateDirectory(KartenJPGDir)
            l(KartenIMGDir)
            If Not IO.Directory.Exists(KartenIMGDir) Then IO.Directory.CreateDirectory(KartenIMGDir)



            glob2.nachricht("Verzeichnisse_anlegen #################ende ####")

            l("Verzeichnisse_anlegen---------------------- ende")
        Catch ex As Exception
            l("Fehler in Verzeichnisse_anlegen: " & ex.ToString())
        End Try
    End Sub





End Module
