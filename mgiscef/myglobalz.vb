Imports System.Data.SqlClient
'Imports Devart.Data.Oracle


Module myglobalz
    Public maxPixelOutputSize As Integer = 4048
    'Public Datenbank_als_HTML As Boolean
    Public CGIstattDBzugriff As Boolean
    Public slots() As clsSlot
    Public dbTemplateString As String = ""
#Region "Konstanten"
    '  zuhauseteil
    '     mgisroot = "c:\apps\mgis\"
    '            mgisUserRoot = "c:\apps\test\mgis\"
    '            serverUNC = "c:\"
    '            serverWeb = "http://localhost"
    '            postgresHost = "localhost"
    '###########################################

    Public Const prozessname As String = "MGIS"
    Public Const albverbotsString As String = "Der Auszug aus dem Amtlichen Liegenschaftskataster-Informationssystem (ALKIS) darf nur " &
                                            " intern verwendet werden." &
                                            " Eine Weitergabe des Auszugs an Dritte ist unzulässig." &
                                            " Auskünfte aus dem ALKIS an Dritte erteilt - bei Vorliegen eines berechtigten Interesses - " &
                                            " das Katasteramt (kundenservice.afb-heppenheim@hvbg.hessen.de). Alle Zugriffe werden protokolliert."
    Public eigentuemer_protokoll As String = ""
    Public URLserialserver As String = ""
    Public URLlayer2shpfile As String = ""
    Public Const PostgisDBcoordinatensystem As String = "25832"

    'Public mgisRemoteRoot As String = ""
    Public mgisRemoteUserRoot As String = ""

    Public iminternet As Boolean = False
    Public serverUNC As String = ""
    Public serverWeb As String = ""
    Public HauptServerName As String = ""
    Public postgresHost As String = ""
    Public mssqlhost As String = ""



#End Region
    Public histFstView As String = "h_flurkarte.hflurkarte" ' ist ein materialized view !!!! '"h_flurkarte.synhistfst" '"h_flurkarte.basisab2010"
    Public globCanvasWidth, globCanvasHeight As Integer

    Public mgisVersion As String = My.Resources.BuildDate.Trim.Replace(vbCrLf, "")
    Public paradigmaMsql As New clsDBspecMSSQL
    Public paradigmaMsqlmyconn As SqlConnection

    Public pLightMsql As New clsDBspecMSSQL
    Public pLightMsqlmyconn As SqlConnection
    'Public paradigmadokREC As New clsDBspecOracle
    'Public paradigmaRECmyconn As OracleConnection
    'Public paradigmaDBTyp As String = "oracle"
    Public paradigmaDBTyp As String = "sqls"
    '################################
    Public mgisRangecookieDir As String = "" : Public mgisBackModus As Boolean = False : Public mgisBackmodusLastCookie As Date = Now

    Public PARADIGMA_Vorgangs_ArchivSubdir As String ' wird erst bei der ersten benutzung initialisiert. 
    Public GisUser As New clsUser
    Public GrundFuerEigentuemerabfrage As String = ""
    Public gemeinde_verz As String = ""
    Public Paradigma_GemarkungsXML As String = ""
    Public Paradigma_funktionen_verz As String = ""

    Public dina4InMM, dina3InMM, dina4InPixel, dina3InPixel As New clsCanvas

    Public ParadigmaDominiertzuletztFavoriten As Boolean = True

    Public lastGeomAsWKT As String = ""
    Public collFST As New List(Of clsFlurstueck)
    Public OSrefresh As Boolean = False
    Public currentProcID As Integer = 0
    'Public istAlbBerechtigt As Boolean = False
    Public paradigmaAdmins(2) As String
    Public CollAuswahltreffer As New List(Of clsauswahlTreffer)

    Public os_tabelledef As New clsTabellenDef
    Public aktvorgang As New clsVorgang
    'pfade 
    Public aktObjID As Integer = 0
    Public akttabnr As Integer = 1
    Public suchObjektModus As Integer = 0 'String = "" '"fst" ' oder 'puffer'

    Public gesamtSachdatList As New List(Of clsSachdaten)

    Public userIniProfile As clsINIDatei

    Public aktvorgangsid As String = ""
    Public adrREC As New clsDBspecPG
    'Public fstREC As New clsDBspecPG
    Public webgisREC As New clsDBspecPG
    Public basisrec As New clsDBspecPG
    Public OSrec As New clsDBspecPG
    Public areaqmaktFST As New ParaFlurstueck
    Public oldSuchFlurstueck As New ParaFlurstueck
    Public aktadr As New ParaAdresse
    Public aktFST As New ParaFlurstueck

    Public Property ProbaugSuchmodus As String
    Public probaugAdresse As New ParaAdresse
    Public probaugFST As New ParaFlurstueck
    'Public aktRechts, aktHoch As String


    'Public latitude, longitude As String
    Public punktarrayInM() As myPoint
    Public ProxyString As String = ""
    Public allLayers As New List(Of clsLayer)
    Public allOSLayers As New List(Of clsLayerPres)
    Public wmspropList As New List(Of wmsProps)
    Public allDokus As New List(Of clsDoku)
    Public allLayersPres As New List(Of clsLayerPres)
    Public kategorienliste As New List(Of clsUniversal)
    Public hgrundLayers As New List(Of clsLayerPres)
    'Public favoritBauaufsicht As New clsFavorit
    Public favoritakt As New clsFavorit
    'Public gisuser.favogruppekurz  gisuser.favogruppekurz As String = ""
    Public aktaid, aktsid As Integer
    'Public   GisUser.nick As String
    Public STARTUP_mgismodus As String = "vanilla" 'probaug,paradigma

    Public GroupLayerSqlString As String = ""
    Property auswahlBookmark As New clsBookmark

    '
    Public enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252)
    Public Property aktmasstabTag As Integer
    Public Property masstaebe As New List(Of clsMasstab)
    Public Property druckMasstaebe As New List(Of clsMasstab)
    Public Property fangradius_in_pixel As Integer = 7
    Public Property minErrorMessages As Boolean = False
    Public Property myfakeurl As String = ""
    Public Property ColumnnamesColl As New Dictionary(Of String, String())
    'Public Property getMapsFromInternet As Boolean = False

    Public PDF_PrintRange As New clsRange
    Public PDF_druckMassStab As Double
    Public Hoehe_desTabcontrols As Integer = 0 '9
    Public rtfTextDoku, rtfTextLegende As String

    Public auswahlRechteck As New Rectangle()

    ' Public VGcanvasImage As New Image
    'Public OScanvasImage As New Image
    'Public HGcanvasImageRange0 As New Image

    'Public VGmyBitmapImage As New BitmapImage()
    'Public HGmyBitmapImage As New BitmapImage()
    'Public OSmyBitmapImage As New BitmapImage()


    Public zeichneImageMapGlob As Boolean = True
    Public zeichneOverlaysGlob As Boolean = True

    Public CanvasClickModus As String = ""
    'Public aktPoint As New myPoint

    Public aktGlobPoint As New myPoint
    Public aktPolygon As New clsParapolygon
    Public aktPolyline As New clsParapolyline
    Public LastThemenSuche As String
    Public exploreralphabetisch As Boolean = True
End Module


