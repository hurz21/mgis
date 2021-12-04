Public Class clsLayer
    ''' <summary>
    ''' for the ACTIVE layer
    ''' </summary>
    ''' <returns></returns>
    ''' prop    
    Friend Property ldoku As New clsDoku
    Public Property mapFileHeader As String = ""
    Property mapFile As String
    Property standardsachgebiet As String = ""
    Property mit_objekten As Boolean
    Property aid As Integer
    Property titel As String = ""
    Property isHgrund As Boolean
    Property isactive As Boolean
    Public Property sid As Integer
    Public Property rang As Integer
    Public Property mit_imap As Boolean
    Public Property masstab_imap As String = ""
    Public Property mit_legende As Boolean
    Public Property pfad As String = ""
    Public Property schema As String = ""
    Public Property schlagworte As String = ""
    Public Property ebene As String = ""
    Public Property status As Boolean = True
    Public Property suchfeld As String = ""

    Sub clear()
        aid = 0
        schema = ""
        ebene = ""
        schlagworte = ""
        pfad = ""
        mit_legende = False
        ldoku.clear()
        mapFileHeader = ""
        masstab_imap = "0"
        rang = 0
        sid = 0
        isactive = False
        isHgrund = False
        titel = ""
        mapFile = ""
        standardsachgebiet = "1"
    End Sub
    ''' <summary>
    ''' braucht den pfad
    ''' </summary>
    ''' <returns></returns>
    Function calcMapfileFullname(mapfileTyp As String) As String
        Try
            Dim mf As String
            'mf = pfad & ebene & "_" & mapfileTyp & ".map"
            mf = nkat & aid & "/" & mapfileTyp & ".map"

            Return mf
        Catch ex As Exception
            l("fehler in calcMapfileFullname:" & ex.ToString)
            Return ""
        End Try
    End Function

End Class
Public Class clsLegendenItem
    Property aid As Integer
    Property nr As Integer
    Property titel As String
End Class
Class clsDoku
    Property aid As Integer
    Property inhalt As String
    Property entstehung As String
    Property aktualitaet As String
    Property massstab As String
    Property beschraenkungen As String
    Property datenabgabe As String
    Property calcedOwner As String

    Friend Sub clear()
        aid = 0
        inhalt = ""
        entstehung = ""
        aktualitaet = ""
        massstab = ""
        beschraenkungen = ""
        datenabgabe = ""
        calcedOwner = ""
    End Sub
End Class
