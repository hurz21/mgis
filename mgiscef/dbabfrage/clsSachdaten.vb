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
    Public Function toStringa(trenn As String) As String
        Dim a As New Text.StringBuilder
        a.Append(" aid: " & aid.ToString & trenn)
        a.Append(" gid: " & gid.ToString & trenn)
        a.Append(" tab_nr: " & tab_nr.ToString & trenn)
        a.Append(" datenbank: " & datenbank.ToString & trenn)
        a.Append(" Schema: " & Schema.ToString & trenn)
        a.Append(" id: " & id.ToString & trenn)
        a.Append(" linkTabs: " & linkTabs.ToString & trenn)
        a.Append(" tabelle: " & tabelle.ToString & trenn)
        a.Append(" tabellen_anzeige: " & tabellen_anzeige.ToString & trenn)
        a.Append(" geomtype: " & geomtype.ToString & trenn)
        a.Append(" tab_id: " & tab_id.ToString & trenn)
        a.Append(" os_tabellen_name: " & os_tabellen_name.ToString & trenn)
        a.Append(" tabellenvorlage: " & tabellenvorlage.ToString & trenn)
        Return a.ToString
    End Function
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
            tempdat.tabellenvorlage = quelle.tabellenvorlage
            Return tempdat
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Property tabellenvorlage As Integer = 0
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
    Function getOSTabellenName() As String
        If tabelle.IsNothingOrEmpty Then Return ""
        If tabelle.ToLower.StartsWith("os_") Then Return tabelle
        Return "os_" & tabelle
    End Function
End Class
