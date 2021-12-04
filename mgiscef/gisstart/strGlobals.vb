Public Class strGlobals
    'Public Shared SidHintergrund As Integer = 46 ' wird benötigt um zu bestimmen, ob eine aid zum hintergrund gehört
    'Public Shared buergergisInternetServer As String = "https://buergergis.kreis-offenbach.de"
    Public Shared nkat As String = "/nkat/aid/"
#Region "exe"
    Public Shared Property NoImageMap As Boolean = False
    'Public Shared gisMaximiertStarten As Boolean = True
    Public Shared Property mapserverExeString As String = "mapserv722/mapserv.cgi" '"mapserv70/mapserv.cgi"
    Public Shared Property paintProgramm As String = "mspaint.exe"
    Public Shared Property controllingCounter As Integer = 0
    Public Shared Property FavoriteneintragSchonvorhanden As Boolean = False
    Public Shared Property cPtest_ As String = "C:\kreisoffenbach"
    'Public Shared Property cPtest_ As String = Environment.CurrentDirectory
    Public Shared Property gisWorkingDir As String = Environment.CurrentDirectory 'cPtest_ & "\mgis\"
    Public Shared Property localDocumentCacheRoot As String = IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDocuments),
                                 "Paradigma\cache")
    Public Shared Property meinWordProcessor As String = "WINWORD.EXE"
    Public Shared Property pdfReader As String = "C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe"
    Public Shared Property tabbearbeiter As String = "t05"
    Public Shared Property UseDownloadCache As Boolean = True
    Public Shared gisdossierexe As String = cPtest_ & "\gisdossier\gisdossier.exe "
    'Public Shared bplanupdateExe As String = cPtest_ & "\bplankat\bplanupdate.exe"

    Public Shared chromeFile As String = "CHROME.EXE" '"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"

    Public Shared paradigmaExe As String = cPtest_ & "\main\paradigma.exe"
    Public Shared paradigmadetail As String = cPtest_ & "\paradigmadetail\paradigmadetail.exe"

    Public Shared PL_bestandexe As String = cPtest_ & "\PL\PL_BESTAND.exe"
    Public Shared bplanupdateBat As String = ""
    Public Shared ALBnas2TestExe As String = ""
    Public Shared gisEdit As String = cPtest_ & "\gisedit\gisedit.exe "
    Public Shared imaptemplateFile As String = gisWorkingDir & "\templates\imaptemplate.htm"
    Public Shared dbtemplateFile As String = gisWorkingDir & "\templates\dbtemplate.htm"
    Public Shared google3Dtemplate As String = gisWorkingDir & "\templates\Infowindow.htm"
#End Region
#Region "Dokumente bilder"
    Public Shared gisguidedocx As String = gisWorkingDir & "\guide\gisguide.pdf"
    Public Shared datenschutzDoc As String = gisWorkingDir & "\guide\datenschutz.txt"

    Public Shared suchobjektPNG As String
    Public Shared suchobjektPNG2 As String
    Public Shared explorerPNG As String
    Public Shared suchobjekt3sorten As String
    Public Shared vogeldrehenpng As String
    Public Shared vorgangsmenuepng As String




    Public Shared hinweis_KeineEbenenrtf As String = gisWorkingDir & "\guide\hinweis_KeineEbenen.rtf"

    Public Shared PARADIGMA_ARCHIV_rootdir As String = "\\file-paradigma\Paradigma\test\paradigmaArchiv\backup\archiv"
#End Region
#Region "mapfiles"
    Public Shared normMapfileHeader As String = "'/websys/mapfiles/system/header.map'"
    'Public Shared PDFdruck_MapFileMinimalQuer As String = "'/websys/mapfiles/system/paradigma_minimal_quer.map'"
    'Public Shared PDFdruck_MapFileMinimalQuerHochaufl As String = "'/websys/mapfiles/system/paradigma_minimal_querHochaufl.map'"
    'Public Shared PDFdruck_MapFileHeaderr As String = "'/websys/mapfiles/system/paradigma_d_q_s_PDFheader.map'"
    'Public Shared PNGdruck_MapFileHeaderr As String = "'/websys/mapfiles/system/paradigma_d_q_s_PNGheader.map'"
    Public Shared paradigmaTopLayerMap As String = "/nkat/vorlage/paradigmaToplayer.map"
    Public Shared paradigma_hervorhebungflaechemap As String = "/nkat/vorlage/paradigma_hervorhebungflaeche.map"
    Public Shared paradigma_hervorhebungflaecheFSTmap As String = "/nkat/vorlage/paradigma_hervorhebungflaecheFST.map"

#End Region

    Friend Shared Function attachCredentials2aufruf(aufruf As String) As String
        Try
            l(" MOD attachCredentials2aufruf anfang")
            aufruf = aufruf & "&nick=" & GisUser.nick & "&pw=" & GisUser.EmailPW '& "&userinfo=" '& clsStartup.makeLokalUserinfo(GisUser)
            l(aufruf)
            l(" MOD attachCredentials2aufruf ende")
            Return aufruf
        Catch ex As Exception
            l("Fehler in attachCredentials2aufruf: " & ex.ToString())
            Return aufruf
        End Try
    End Function
End Class
