Public Class clsStamm
    Property gruppen As New clsGruppen2Aid
    Property tabellenListen As New List(Of clsTabellenDef)
    Property vorlagenListe As New List(Of MaskenObjekt)
    Property aktDoku As New clsDoku
    'Property attributtabelleDef As New clsTabellenDef
    Property isHintergrund As Boolean
    Property aid As Integer
    Property status As Boolean
    Property ebene As String
    Property titel As String
    Property rang As Integer
    Property mit_imap As Boolean
    Property masstab_imap As Integer

    Property mit_objekten As Boolean
    Property mit_legende As Boolean
    Property sachgebietid As Integer
    Property sachgebiet As String
    Public Property pfad As String
    Property schlagworte As String
    Public Property sid As Integer
    Public Property anzahl_attributtabellen As Integer

    Public Property AnzahlAttributtabellenReal As Integer
End Class
Public Class clsGruppen2Aid
    Property id As Integer
    Property aid As Integer
    Property internet As Boolean = True
    Property intranet As Boolean = True
    Property umwelt As Boolean = True
    Property sicherheit As Boolean = True
    Property bauaufsicht As Boolean = True
End Class
Public Class MaskenObjekt
    Property nr As Integer = 0
    Property feldname As String = ""
    Property titel As String = ""
    Property typ As String = ""
    Property cssclass As String = ""
    Property template As String = ""
    Property tab_nr As Integer = 1
    Property id As Integer
    Public Property anwendung As Integer = 2

    Sub clear()
        nr = 0
        feldname = ""
        titel = ""
        typ = ""
        cssclass = ""
        template = ""
        tab_nr = 1
        id = 0
        anwendung = 2
    End Sub
End Class
Public Class clsTabellenDef
    Property tabelle As String
    Property aid As String
    Property Schema As String

    Property tab_nr As String
    Property tab_id As String
    Property tabtitel As String
    Property tabellen_anzeige As String
    Property gid As String
    Public Property datenbank As String
    Public Property id As Integer
    Property linkTabs As String
End Class
