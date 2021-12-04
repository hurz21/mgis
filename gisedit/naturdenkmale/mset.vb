Public Class mset
    Public Shared VGcanvasImage As New Image
    Public Shared zielmapfileUNC As String
    Public Shared zielmapfileURL As String
    Public Shared Property ProxyString As String = ""
    Shared Property enc As Text.Encoding = System.Text.Encoding.GetEncoding("iso-8859-1")
    Public Shared Property serverUNC As String = "\\gis\gdvell"
    Public Shared Property gemeinde_verz As String = serverUNC & "\apps\test\mgis\combos\gemeinden.xml"

    Public Shared queryIndividuenRoot As String = "select * from  schutzgebiete.naturdenkmal_f [WHERESTRING]" ' order by lfd_nr "

    Public Shared queryIndividuenEDITRoot As String = "select * from  paradigma_userdata.ndindividuenedit [WHERESTRING]  "
    Shared Property ndgruppen As New List(Of clsNDgruppe)
    Shared Property ndindividuen As New List(Of clsNDinidividuum)
    Public Shared basisrec As New clsDBspecPG
    Public Shared mitte As New myPoint
    Public Shared aktrange As New clsRange
    Public Shared Property serverWeb As String = "http://gis.kreis-of.local"
    Shared Sub initdb()
        mset.basisrec.mydb = New clsDatenbankZugriff
        mset.basisrec.mydb.Host = "gis"
        mset.basisrec.mydb.username = "postgres" : mset.basisrec.mydb.password = "lkof4"
        mset.basisrec.mydb.Schema = "postgis20"
        mset.basisrec.mydb.Tabelle = "flurkarte.basis_f" : mset.basisrec.mydb.dbtyp = "postgis"
    End Sub
End Class
