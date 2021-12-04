Public Class strGlobals
    Public Shared SidHintergrund As Integer = 46 ' wird benötigt um zu bestimmen, ob eine aid zum hintergrund gehört
    Public Shared buergergisInternetServer As String = "https://buergergis.kreis-offenbach.de"
    Public Shared nkat As String = "/nkat/aid/"
#Region "exe"
    Public Shared Property mapserverExeString As String = "mapserv70/mapserv.cgi"
    Public Shared Property paintProgramm As String = "mspaint.exe"

    Public Shared Property cPtestMgis As String = "C:\ptest\mgis\"
    Public Shared Property pdfReader As String = "C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe"
    Public Shared gisdossierexe As String = "C:\ptest\gisdossier\gisdossier.exe "
    Public Shared bplanupdateExe As String = "c:\ptest\bplankat\bplanupdate.exe"
    Public Shared bplanupdateBat As String = "apps\bplankat\bplanupdate.bat"
    Public Shared chromeFile As String = "CHROME.EXE" '"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"

    Public Shared paradigmaExe As String = "C:\ptest\main\paradigma.exe"
    Public Shared paradigmadetail As String = "c:\ptest\paradigmadetail\paradigmadetail.exe"

    Public Shared PL_bestandexe As String = "C:\ptest\PL\PL_BESTAND.exe"
    Public Shared ALBnas2TestExe As String = "apps\test\mgis\alkis\albnas2test.exe "
    Public Shared gisEdit As String = "C:\ptest\gisedit\gisedit.exe "

#End Region
#Region "Dokumente bilder"
    Public Shared gisguidedocx As String = "c:\ptest\mgis\gisguide.docx"
    Public Shared suchobjektPNG As String = "c:\ptest\mgis\bilder\suchobjekt.png"
    Public Shared suchobjektPNG2 As String = "c:\ptest\mgis\bilder\suchobj2.png"
    Public Shared explorerPNG As String = "c:\ptest\mgis\bilder\explorer.png"
    Public Shared suchobjekt3sorten As String = "c:\ptest\mgis\bilder\suchobjekte_3sorten.png"
    Public Shared vogeldrehenpng As String = "c:\ptest\mgis\bilder\vogeldrehen.png"
    Public Shared vorgangsmenuepng As String = "c:\ptest\mgis\bilder\vorgangsmenue.png"
    Public Shared hinweis_KeineEbenenrtf As String = "c:\ptest\mgis\hinweis_KeineEbenen.rtf"
    Public Shared PARADIGMA_ARCHIV_rootdir As String = "\\file-paradigma\Paradigma\test\paradigmaArchiv\backup\archiv"
#End Region
#Region "mapfiles"
    Public Shared normMapfileHeader As String = "'/inetpub/wwwroot/buergergis/mapfile/header.map'"
    Public Shared PDFdruck_MapFileMinimalQuer As String = "'/websys/mapfiles/system/paradigma_minimal_quer.map'"
    Public Shared PDFdruck_MapFileMinimalQuerHochaufl As String = "'/websys/mapfiles/system/paradigma_minimal_querHochaufl.map'"
    Public Shared PDFdruck_MapFileHeaderr As String = "'/websys/mapfiles/system/paradigma_d_q_s_PDFheader.map'"
    Public Shared PNGdruck_MapFileHeaderr As String = "'/websys/mapfiles/system/paradigma_d_q_s_PNGheader.map'"
    Public Shared paradigmaTopLayerMap As String = "/nkat/vorlage/paradigmaToplayer.map"
    Public Shared paradigma_hervorhebungflaechemap As String = "/nkat/vorlage/paradigma_hervorhebungflaeche.map"
    Public Shared paradigma_hervorhebungflaecheFSTmap As String = "/nkat/vorlage/paradigma_hervorhebungflaecheFST.map"
    'Public Shared suchobjektPNG2 As String = "c:\ptest\mgis\bilder\suchobj2.png"
    'Public Shared suchobjektPNG2 As String = "c:\ptest\mgis\bilder\suchobj2.png"
#End Region


End Class
