Public Class clsInitStrings
    Shared Sub setMainstrings()
#If DEBUG Then

        If iminternet Then
            myglobalz.serverWeb = "https://buergergis.kreis-offenbach.de"
            'myglobalz.postgresHost = "localhost"
            'myglobalz.serverUNC = "d:\"
            'myglobalz.mssqlhost = "localhost\SQLEXPRESS"
            myglobalz.postgresHost = ""
            myglobalz.serverUNC = strGlobals.cPtestMgis
            myglobalz.mssqlhost = ""
            myglobalz.eigentuemer_protokoll = ""
            myglobalz.mgisRemoteUserRoot = strGlobals.cPtestMgis & "test\"
        Else
            myglobalz.postgresHost = myglobalz.HauptServerName
            myglobalz.serverUNC = "\\" & HauptServerName & "\gdvell\"
            myglobalz.serverWeb = "http://" & HauptServerName & ".kreis-of.local"
            myglobalz.mssqlhost = "msql01"
            If myglobalz.getMapsFromInternet Then
                myglobalz.serverWeb = "https://buergergis.kreis-offenbach.de"
                myglobalz.serverWeb = "http://" & HauptServerName & ".kreis-of.local"
            Else
                myglobalz.serverWeb = "http://localhost"
                myglobalz.serverWeb = "http://" & HauptServerName & ".kreis-of.local"
            End If
            myglobalz.eigentuemer_protokoll = myglobalz.serverUNC & "apps\eigentuemer\zugriffNEU.txt"
            myglobalz.mgisRemoteUserRoot = myglobalz.serverUNC & "apps\test\mgis\"
        End If
#Else
        myglobalz.postgresHost = myglobalz.HauptServerName
        myglobalz.serverUNC = "\\" & myglobalz.HauptServerName & "\gdvell\"
        myglobalz.serverWeb = "http://" & myglobalz.HauptServerName & ".kreis-of.local"
        myglobalz.mssqlhost = "msql01"
               myglobalz.eigentuemer_protokoll = myglobalz.serverUNC & "apps\eigentuemer\zugriffNEU.txt"
            myglobalz.mgisRemoteUserRoot = myglobalz.serverUNC & "apps\test\mgis\"
#End If
        myglobalz.URLserialserver = myglobalz.serverWeb & "/cgi-bin/apps/paradigmaex/serialserver/pg/serialserver.cgi?user="
        myglobalz.URLlayer2shpfile = myglobalz.serverWeb & "/cgi-bin/apps/paradigmaex/layer2shpfile/userlayer2postgis/userlayer2postgis.cgi?user="
        Paradigma_GemarkungsXML = strGlobals.cPtestMgis & "combos\gemarkungen.xml"
        Paradigma_funktionen_verz = strGlobals.cPtestMgis & "combos\RBfunktion.xml"
    End Sub

End Class
