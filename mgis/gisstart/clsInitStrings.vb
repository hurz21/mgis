Public Class clsInitStrings
    Shared Sub setMainstrings()
#If DEBUG Then
        If Environment.UserName = "hurz" Or Environment.UserName = "zahnlückenpimpf" Then
            myglobalz.postgresHost = "localhost"
            myglobalz.serverUNC = "d:\"
            myglobalz.mssqlhost = "localhost\SQLEXPRESS"

            If iminternet Then
                myglobalz.serverWeb = "https://buergergis.kreis-offenbach.de"
            Else
                myglobalz.serverWeb = "http://localhost"
                'myglobalz.serverWeb = "https://buergergis.kreis-offenbach.de"
            End If

        Else
            myglobalz.postgresHost = "w2gis02"
            myglobalz.serverUNC = "\\w2gis02\gdvell\"
            myglobalz.serverWeb = "http://w2gis02.kreis-of.local"
            myglobalz.mssqlhost = "msql01"
        End If
#Else
        myglobalz.postgresHost = "w2gis02"
        myglobalz.serverUNC = "\\w2gis02\gdvell\"
        myglobalz.serverWeb = "http://w2gis02.kreis-of.local"
        myglobalz.mssqlhost = "msql01"
#End If


        myglobalz._protokoll = myglobalz.serverUNC & "apps\eigentuemer\zugriffNEU.txt"

        myglobalz.mgisroot = myglobalz.serverUNC & "apps\mgis\"
        myglobalz.mgisUserRoot = myglobalz.serverUNC & "apps\test\mgis\"
        myglobalz.mgisroot = myglobalz.serverUNC & "apps\mgis\"
        myglobalz.mgisUserRoot = myglobalz.serverUNC & "apps\test\mgis\"
        myglobalz.URLserialserver = myglobalz.serverWeb & "/cgi-bin/apps/paradigmaex/serialserver/pg/serialserver.cgi?user="
        myglobalz.URLlayer2shpfile = myglobalz.serverWeb & "/cgi-bin/apps/paradigmaex/layer2shpfile/userlayer2postgis/userlayer2postgis.cgi?user="
        Paradigma_GemarkungsXML = serverUNC & "apps\test\mgis\combos\gemarkungen.xml"
        Paradigma_funktionen_verz = serverUNC & "apps\test\mgis\combos\RBfunktion.xml"
    End Sub

End Class
