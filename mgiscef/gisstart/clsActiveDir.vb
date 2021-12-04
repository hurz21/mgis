Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports System.Security
Imports System.Data

Public Class clsActiveDir

    Shared Property Name As String = ""
    Shared Property Vorname As String = ""
    Shared Property fdkurz As String = ""
    Shared Property userid As String = ""
    Shared Property emailadress As String = ""

    Shared Function getall(userName As String) As Boolean
        If GisUser.nick = "hurz" Then
            Return True
        End If
        'filter = clsString.umlaut2ue(userName)
        'userName = "Masal_s"
        Dim Vorname As String = ""
        Dim lokdt As DataTable = clsActiveDir.sucheperson(userName)
        If lokdt.IsNothingOrEmpty Then
            Name = ""
            Vorname = ""
            fdkurz = ""
            userid = ""
            emailadress = ""
            Return False
        Else
            Name = lokdt.Rows(0).Item("sn").ToString()
            Vorname = lokdt.Rows(0).Item("givenName").ToString()
            fdkurz = lokdt.Rows(0).Item("department").ToString()
            userid = lokdt.Rows(0).Item("userPrincipalName").ToString
            emailadress = lokdt.Rows(0).Item("mail").ToString
            Return True
        End If
    End Function
    Private Shared Function makeListeOfproperties() As List(Of String)
        Dim liste As New List(Of String)
        liste.Add("givenName") 'vorname
        liste.Add("displayName")
        liste.Add("name")
        liste.Add("sn") 'nachname
        liste.Add("cn")
        liste.Add("sAMAccountName")
        liste.Add("userPrincipalName") 'userid
        liste.Add("telephoneNumber")
        liste.Add("physicalDeliveryOfficeName")
        liste.Add("mail")
        liste.Add("company")
        liste.Add("department")
        liste.Add("manager")
        liste.Add("mobile")
        liste.Add("ou")
        liste.Add("streetAddress")
        Return liste
    End Function






    Public Shared Function GetLDAPUserAttributs(ByVal SearchPerson As String,
                                         ByVal LDAPProperties As List(Of String),
                                         ByVal GlobalCatalog As Boolean) As DataTable
        'SearchPerson = "*" & SearchPerson & "*"
        Dim dt As New DataTable()
        Dim dr As DataRow
        Dim Searcher As New DirectorySearcher()
        Dim SearchResults As SearchResultCollection = Nothing
        Try
            l("GetLDAPUserAttributs---------------------- anfang")
            Searcher.Filter = "(&(objectClass=user)(|(displayName=" & SearchPerson & ")(cn=" & SearchPerson & ")(sAMAccountName=" & SearchPerson & ")))"
            Searcher.SearchScope = SearchScope.Subtree
            dt.Columns.Add(New DataColumn("Domain", GetType(String)))
            For Each Name As String In LDAPProperties
                dt.Columns.Add(New DataColumn(Name, GetType(String)))
                Searcher.PropertiesToLoad.Add(Name)
            Next
            If GlobalCatalog Then
                Dim d As Domain = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain()
                Dim gc As GlobalCatalog = d.Forest.FindGlobalCatalog()
                Searcher.SearchRoot = New DirectoryEntry("GC://" + gc.Name)
            Else
                Dim adsiRoot As New System.DirectoryServices.DirectoryEntry("LDAP://RootDSE")
                Searcher.SearchRoot = New DirectoryEntry("LDAP://" + (adsiRoot.Properties("defaultNamingContext")(0)).ToString)
            End If

            SearchResults = getADdaten(Searcher, SearchResults)
            For Each Result As SearchResult In SearchResults
                dr = dt.NewRow()
                Dim Domain As String = Result.Path
                Domain = Domain.Substring(Domain.IndexOf("DC="))
                Domain = Domain.Replace("DC=", "")
                Domain = Domain.Replace(",", ".")
                dr("Domain") = Domain
                For Each Name As String In LDAPProperties
                    If Result.Properties(Name).Count >= 1 Then
                        dr(Name) = Result.Properties(Name)(0)
                    End If
                Next
                dt.Rows.Add(dr)
            Next
            Return dt
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & SearchPerson & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Shared Function getADdaten(Searcher As DirectorySearcher, SearchResults As SearchResultCollection) As SearchResultCollection
        Try
            l("getADdaten---------------------- anfang")
            SearchResults = Searcher.FindAll()
            l("getADdaten---------------------- ende")
        Catch ex As Exception
            l("Fehler im Active Directory - bitte an die IT wenden / Hr. Saager: ", ex)
        End Try
        Return SearchResults
    End Function

    Shared Function sucheperson(ByVal Name$) As DataTable
        Dim liste As List(Of String) = makeListeOfproperties()
        Dim dt As DataTable
        If Name = "El_Achak_H" Then Name = "el achak_h"
        'GetUserMemberOf("kreis-of", "a670024", "snoopy14")
        ' Authenticate("kreis-of", "a670024", "snoopy14")
        ' dt = GetLDAPUserAttributs("a670024", liste, True)
        'dt = GetLDAPUserAttributs("Schöniger", liste, True)
        dt = GetLDAPUserAttributs(Name, liste, True)
        Return dt
    End Function

    'Public Shared Sub item2objAD(ByVal item As DataRowView, ByRef aktperson As clsPerson)
    '    ' glob2.nachricht("  item2objAD:--------------------------------")
    '    Try
    '        aktperson.clear()
    '        ' % = CInt(item(0).ToString())
    '        aktperson.Raumid = item("physicalDeliveryOfficeName").ToString()       '
    '        aktperson.Name = item("sn").ToString()
    '        aktperson.Vorname = item("givenName").ToString()
    '        'aktperson.Namenszusatz = item("Titel").ToString()
    '        aktperson.Kontakt.elektr.Telefon1 = item("telephoneNumber").ToString()
    '        aktperson.Kontakt.elektr.Email = item("mail").ToString()  '
    '        aktperson.FDkurz = item("department").ToString()    '
    '        aktperson.Arbeitgeber = item("company").ToString() '
    '        aktperson.Gebaeude = item("streetAddress").ToString
    '        aktperson.userid = item("userPrincipalName").ToString() ' 
    '    Catch ex As System.Exception
    '        '  glob2.nachricht("Fehler in item2objAD:" ,ex)
    '    End Try
    'End Sub

End Class

