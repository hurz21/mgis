Module myglobalz
        Public enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("iso-8859-1")
    Public iniDict As New Dictionary(Of String, String)

    Public Webgis_MYDB As New LIBDB.clsDatenbankZugriff
    Public Paradigma_MYDB As New LIBDB.clsDatenbankZugriff
    Public Postgis_MYDB As New LIBDB.clsDatenbankZugriff

Property gis_serverD As String 

Property GIS_WebServer As String 
    Property aktbox As  clsrange

End Module
