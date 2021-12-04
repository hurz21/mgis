Class clsSachdaten
    Implements IComparable(Of clsSachdaten)
    Implements ICloneable
    Property feldname As String
    Property feldinhalt As String
    Property neuerFeldname As String
    Property nr As Integer ' reihenfolge
    Public Function CompareTo(other As clsSachdaten) As Integer Implements IComparable(Of clsSachdaten).CompareTo
        Return Me.nr.CompareTo(other.nr)
    End Function
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

Class MaskenObjekt
    Property nr As Integer = 0
    Property feldname As String = ""
    Property titel As String = ""
    Property typ As String = ""
    Property cssclass As String = ""
    Property template As String = ""
End Class


Public Class clsTabellenDef
    Property geomtype As String = "Polygon"
    Shared Function copyTabdef(quelle As clsTabellenDef) As clsTabellenDef
        Dim tempdat As New clsTabellenDef
        Try
            tempdat.aid = quelle.aid
            tempdat.datenbank = quelle.datenbank
            tempdat.gid = quelle.gid
            tempdat.id = quelle.id
            tempdat.Schema = quelle.Schema
            tempdat.linkTabs = quelle.linkTabs
            tempdat.tabelle = quelle.tabelle
            tempdat.tabellen_anzeige = quelle.tabellen_anzeige
            tempdat.tab_id = quelle.tab_id
            tempdat.tab_nr = quelle.tab_nr
            tempdat.geomtype = quelle.geomtype
            tempdat.os_tabellen_name = quelle.os_tabellen_name
            Return tempdat
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Property tabelle As String = ""
    Property os_tabellen_name As String = ""
    Property aid As String = ""
    Property Schema As String = ""

    Property tab_nr As String = ""
    Property tab_id As String = ""
    Property tabtitel As String = ""
    Property tabellen_anzeige As String = ""
    Property gid As String = ""
    Public Property datenbank As String = ""
    Public Property id As Integer
    Property linkTabs As String = ""
End Class
