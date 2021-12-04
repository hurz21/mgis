Imports System.Data
Imports System.IO
Imports mgis

Public Class clsDossier
    Public Shared Bplan As New clsDossierItem
    Public Shared Eigentuemer As New clsDossierItem
    Public Shared Kehr As New clsDossierItem
    Public Shared NSG As New clsDossierItem
    Public Shared LSG As New clsDossierItem
    Public Shared FFH As New clsDossierItem
    Public Shared WSG As New clsDossierItem
    Public Shared Altlast As New clsDossierItem
    Public Shared Boris As New clsDossierItem
    Public Shared Illegale As New clsDossierItem
    Public Shared IllegaleAlt As New clsDossierItem
    Public Shared Baulasten As New clsDossierItem
    Public Shared Ueb As New clsDossierItem
    Public Shared UEBKROF As New clsDossierItem
    Public Shared Foerder As New clsDossierItem
    Public Shared Hbiotope As New clsDossierItem
    Public Shared Hkomplexe As New clsDossierItem
    Public Shared HNaturdenkmale As New clsDossierItem
    Public Shared Amphibien As New clsDossierItem
    Public Shared BSE As New clsDossierItem
    Public Shared OEKOKO As New clsDossierItem


    Shared Function getDtHauptabfrageFlaeche(winpt As myPoint, SchemaTabelle As String) As DataTable
        'SchemaTabelle = "schutzgebiete.ffhgebiet_f"
        'dt = clsDossier.getDtHauptabfrageFlaeche(winpt, SchemaTabelle) 
        Dim innerSQL, SQl As String
        Dim dt As DataTable
        l(" getDtHauptabfrageFlaeche ---------------------- anfang " & SchemaTabelle)
        Try
            innerSQL = "select SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ")," &
                    PostgisDBcoordinatensystem.ToString & ")"
            l(innerSQL)
            SQl = "SELECT * " &
                "  FROM " & SchemaTabelle &
                "  WHERE ST_contains( " & SchemaTabelle & ".geom,(" & innerSQL & ")" & ");"
            l("sql: " & SQl)
            l(" getDtHauptabfrageFlaeche ---------------------- anfang")
            dt = getDTFromWebgisDB(SQl, "postgis20")
            l(" getDtHauptabfrageFlaeche ---------------------- ende")
            Return dt
        Catch ex As Exception
            l("Fehler in getDtHauptabfrageFlaeche: " & ex.ToString())
            Return Nothing
        End Try
    End Function





    Friend Shared Function makebplanPDFliste(rESULT_dateien_Bplan As List(Of clsGisresult)) As List(Of clsGisresult)
        Dim blist As New List(Of clsGisresult)
        Dim gr As New clsGisresult
        Try
            l(" makebplanPDFliste ---------------------- anfang")
            For i = 0 To rESULT_dateien_Bplan.Count - 1
                For j = 0 To rESULT_dateien_Bplan(i).begleitdateien.Count - 1
                    gr = New clsGisresult
                    gr.etikett = CStr(rESULT_dateien_Bplan(i).begleitdateien(j).Name).Replace(".pdf", "").Replace(".PDF", "")
                    gr.verordnung = CStr(rESULT_dateien_Bplan(i).datei.Name).Replace(".pdf", "").Replace(".PDF", "")
                    gr.datei = rESULT_dateien_Bplan(i).begleitdateien(j)
                    blist.Add(gr)
                Next
            Next
            l(" makebplanPDFliste ---------------------- ende")
            Return blist
        Catch ex As Exception
            l("Fehler in makebplanPDFliste: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Shared Function getGID4_AREAlayer(winpt As Point, schema As String, tabelle As String) As Integer
        Dim innerSQL As String = "select SetSRID(ST_MakePoint(" & winpt.X & "," & winpt.Y & ")," &
            PostgisDBcoordinatensystem.ToString & ")"
        l(innerSQL)
        '  SELECT GEMARKUNG,nr,NR,PDF,titel,RECHTS,HOCH " &
        Dim SQL = "SELECT * " &
                "  FROM " & schema & "." & tabelle & " " &
                "  WHERE ST_contains( " & schema & "." & tabelle & ".geom,(" & innerSQL & "  )" & "  );"
        l(SQL)
        Try
            l("getGID4layer ")
            Dim dt As DataTable
            dt = getDTFromWebgisDB(SQL, "postgis20")
            l("getGID4layer " & clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
            Return CInt(clsDBtools.fieldvalue(dt.Rows(0).Item(0)))
        Catch ex As Exception
            l("Fehler in holeKoordinatenFuerGID: ", ex)
            Return 0
        End Try
    End Function

    Friend Shared Function istDossierModus(aid As Integer) As Boolean
        Return True
    End Function
End Class
