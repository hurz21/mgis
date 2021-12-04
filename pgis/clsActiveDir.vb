Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports System.Security
Imports System.Data

Public Class clsActiveDir
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



    'Public Function GetUserMemberOf(ByVal domain As String, ByVal username As String, ByVal password As String, Optional ByRef exeption As Exception = Nothing) As Collections.Generic.List(Of String)
    '    Dim searcher As DirectorySearcher = Nothing
    '    Dim colEntry As New Collections.Generic.List(Of String)
    '    Dim att$ = "telephoneNumber" '"MemberOf"
    '    att = "department"

    '    Try
    '        searcher = New DirectorySearcher(New DirectoryEntry("LDAP://" & domain, username, password))
    '        searcher.Filter = String.Concat("(&(objectClass=User) (sAMAccountName=", username, "))")
    '        searcher.PropertiesToLoad.Add("MemberOf")
    '        searcher.PropertiesToLoad.Add("telephoneNumber")
    '        searcher.PropertiesToLoad.Add("department")
    '        searcher.PropertiesToLoad.Add("streetAddress")
    '        searcher.PropertiesToLoad.Add("mail")

    '        Dim result As SearchResult = searcher.FindOne
    '        For i As Integer = 0 To result.Properties(att$).Count - 1
    '            Dim sProp As String = result.Properties(att$)(i)
    '            'colEntry.Add(sProp.Substring(3, sProp.IndexOf(",") - 3))
    '            colEntry.Add(sProp)
    '        Next

    '    Catch ex As Exception
    '        exeption = ex

    '    Finally
    '        searcher.Dispose()
    '    End Try

    '    Return colEntry

    'End Function
    'Public Function Authenticate(ByVal domain As String, ByVal username As String, ByVal password As String) As Boolean
    '    Dim pwd As New SecureString()
    '    Dim bAuth As Boolean = False
    '    Dim entry As DirectoryEntry = Nothing

    '    'Durchlaufe das Passwort und hänge es dem SecureString an 
    '    For Each c As Char In password
    '        pwd.AppendChar(c)
    '    Next

    '    'Bewirkt, dass das Passwort nicht mehr verändert werden kann 
    '    pwd.MakeReadOnly()

    '    'Passwort wird einem Pointer übergeben, damit dieser später "entschlüsselt" werden kann 
    '    Dim pPwd As IntPtr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(pwd)

    '    Try
    '        entry = New DirectoryEntry(String.Concat("LDAP://", domain), username, System.Runtime.InteropServices.Marshal.PtrToStringBSTR(pPwd))
    '        Dim nativeObject As Object = entry.NativeObject
    '        'For Each entrie In entry
    '        '    Console.WriteLine(entrie.ToString)
    '        'Next
    '        bAuth = True
    '    Catch ex As Exception
    '        bAuth = False
    '    Finally
    '        entry.Close()
    '        entry.Dispose()
    '    End Try
    '    Return bAuth
    'End Function



    Public Shared Function GetLDAPUserAttributs(ByVal SearchPerson As String,
                                         ByVal LDAPProperties As List(Of String),
                                         ByVal GlobalCatalog As Boolean) As DataTable
        SearchPerson = "*" & SearchPerson & "*"
        Dim dt As New DataTable()
        Dim dr As DataRow
        Dim Searcher As New DirectorySearcher()
        Dim SearchResults As SearchResultCollection = Nothing
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
        SearchResults = Searcher.FindAll()
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
    End Function

    Shared Function sucheperson(ByVal Name$) As DataTable
        Dim liste As List(Of String) = makeListeOfproperties()
        Dim dt As DataTable
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
    '        '  glob2.nachricht("Fehler in item2objAD:" & ex.ToString)
    '    End Try
    'End Sub

End Class

