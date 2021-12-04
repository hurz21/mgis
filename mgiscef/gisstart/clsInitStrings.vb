Public Class clsInitStrings
    Shared Sub setMainstrings()
        l("in setMainstrings")
#If DEBUG Then
        If iminternet Then
            CGIstattDBzugriff = False
            myglobalz.serverWeb = "https://buergergis.kreis-offenbach.de"
            'myglobalz.postgresHost = "localhost"
            'myglobalz.serverUNC = "d:\"
            'myglobalz.mssqlhost = "localhost\SQLEXPRESS"
            myglobalz.postgresHost = ""

            myglobalz.mssqlhost = ""
            myglobalz.eigentuemer_protokoll = ""
            myglobalz.mgisRemoteUserRoot = strGlobals.localDocumentCacheRoot & "\inis\"
            myglobalz.myfakeurl = "https://buergergis.kreis-offenbach.de"
            strGlobals.gisWorkingDir = IO.Path.Combine("c:\kreisoffenbach\", "mgis")
            myglobalz.serverUNC = strGlobals.gisWorkingDir
            strGlobals.localDocumentCacheRoot = IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                                 "Paradigma\mgis\cache")
        Else
            strGlobals.gisWorkingDir = "c:\kreisoffenbach\mgis"
            CGIstattDBzugriff = False
            myglobalz.postgresHost = myglobalz.HauptServerName
            myglobalz.serverUNC = "\\" & HauptServerName & "\gdvell\"
            myglobalz.serverWeb = "http://" & HauptServerName & ".kreis-of.local"
            myglobalz.mssqlhost = "msql01"
            'If myglobalz.getMapsFromInternet Then
            '    myglobalz.serverWeb = "https://buergergis.kreis-offenbach.de"
            '    myglobalz.serverWeb = "http://" & HauptServerName & ".kreis-of.local"
            'Else
            myglobalz.serverWeb = "http://localhost"
            myglobalz.serverWeb = "http://" & HauptServerName & ".kreis-of.local"
            'End If
            myglobalz.eigentuemer_protokoll = myglobalz.serverUNC & "apps\eigentuemer\zugriffNEU.txt"
            myglobalz.mgisRemoteUserRoot = myglobalz.serverUNC & "apps\test\mgis\"
            myglobalz.myfakeurl = "http://w2gis02.kreis-of.local"
        End If
#Else
        'RELEASE-MOde
               l("in realeasemode")
        'MsgBox(myglobalz.myfakeurl)
        If iminternet Then
            myglobalz.HauptServerName = "buergergis"
            CGIstattDBzugriff = True
            myglobalz.postgresHost = myglobalz.HauptServerName
            myglobalz.serverUNC = "\\" & myglobalz.HauptServerName & "\gdvell\"
            myglobalz.serverWeb = "https://buergergis.kreis-offenbach.de"
            myglobalz.mssqlhost = "msql01"
            myglobalz.eigentuemer_protokoll = myglobalz.serverUNC & "apps\eigentuemer\zugriffNEU.txt"
            'myglobalz.mgisRemoteUserRoot = myglobalz.serverUNC & "apps\test\mgis\"
            myglobalz.mgisRemoteUserRoot = strGlobals.localDocumentCacheRoot & "\inis\"
            myglobalz.myfakeurl = "https://buergergis.kreis-offenbach.de"
            strGlobals.gisWorkingDir = IO.Path.Combine("c:\kreisoffenbach\", "mgis")
            myglobalz.serverUNC = strGlobals.gisWorkingDir
            strGlobals.localDocumentCacheRoot = IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                              "Paradigma\mgis\cache")
        Else
            strGlobals.gisWorkingDir = "c:\kreisoffenbach\mgis"
            CGIstattDBzugriff = False
            myglobalz.postgresHost = myglobalz.HauptServerName
            myglobalz.serverUNC = "\\" & myglobalz.HauptServerName & "\gdvell\"
            myglobalz.serverWeb = "http://" & myglobalz.HauptServerName & ".kreis-of.local"
            myglobalz.mssqlhost = "msql01"
            myglobalz.eigentuemer_protokoll = myglobalz.serverUNC & "apps\eigentuemer\zugriffNEU.txt"
            myglobalz.mgisRemoteUserRoot = myglobalz.serverUNC & "apps\test\mgis\"
            myglobalz.myfakeurl = "http://w2gis02.kreis-of.local"
        End If
               l("in setMainstrings 2")
#End If
        'MsgBox(Environment.CurrentDirectory)

        myglobalz.URLserialserver = myglobalz.serverWeb & "/cgi-bin/apps/paradigmaex/serialserver/pg/serialserver.cgi?user="
        myglobalz.URLlayer2shpfile = myglobalz.serverWeb & "/cgi-bin/apps/paradigmaex/layer2shpfile/userlayer2postgis/userlayer2postgis.cgi?user="

        'strGlobals.cPtestMgis:
        strGlobals.imaptemplateFile = strGlobals.gisWorkingDir & "\templates\imaptemplate.htm"
        strGlobals.dbtemplateFile = strGlobals.gisWorkingDir & "\templates\dbtemplate.htm"
        strGlobals.google3Dtemplate = strGlobals.gisWorkingDir & "\templates\Infowindow.htm"
        myglobalz.Paradigma_GemarkungsXML = strGlobals.gisWorkingDir & "\combos\gemarkungen.xml"
        myglobalz.Paradigma_funktionen_verz = strGlobals.gisWorkingDir & "\combos\RBfunktion.xml"
        myglobalz.gemeinde_verz = strGlobals.gisWorkingDir & "\combos\gemeinden.xml"

        strGlobals.gisguidedocx = strGlobals.gisWorkingDir & "\guide\gisguide.pdf"
        strGlobals.datenschutzDoc = strGlobals.gisWorkingDir & "\guide\datenschutz.txt"
        strGlobals.hinweis_KeineEbenenrtf = strGlobals.gisWorkingDir & "\guide\hinweis_KeineEbenen.rtf"


        strGlobals.bplanupdateBat = myglobalz.serverUNC & "apps\bplankat\bplanupdate.bat"
        strGlobals.ALBnas2TestExe = myglobalz.serverUNC & "apps\test\mgis\alkis\albnas2test.exe "

        strGlobals.suchobjektPNG = myglobalz.serverWeb & "/fkat/paradigma/mgis/bilder/suchobjekt.png"
        strGlobals.suchobjektPNG2 = myglobalz.serverWeb & "/fkat/paradigma/mgis/bilder/suchobj2.png"
        strGlobals.explorerPNG = myglobalz.serverWeb & "/fkat/paradigma/mgis/bilder/explorer.png"
        strGlobals.suchobjekt3sorten = myglobalz.serverWeb & "/fkat/paradigma/mgis/bilder/suchobjekte_3sorten.png"
        strGlobals.vorgangsmenuepng = myglobalz.serverWeb & "/fkat/paradigma/mgis/bilder/vorgangsmenue.png"
        strGlobals.vogeldrehenpng = myglobalz.serverWeb & "/fkat/paradigma/mgis/bilder/vogeldrehen.png"


        l("in setMainstrings ende")

    End Sub

End Class
