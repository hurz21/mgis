Public Class clsLayer
    Friend Property kategorieLangtext As String = ""
    ''' <summary>
    ''' for the ACTIVE layer
    ''' </summary>
    ''' <returns></returns>
    ''' prop    
    ''' prop
    ''' 
    Friend Property ldoku As New clsDoku
    Property wmsProps As New wmsProps
    Property iswms As Boolean = False
    Public Property mapFileHeader As String = ""
    Property mapFile As String
    Property standardsachgebiet As String = ""
    Property mit_objekten As Boolean
    Property aid As Integer
    Property titel As String = ""
    Property isHgrund As Boolean = False
    Property isactive As Boolean = False
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
    Public Property kategorieToolTip As String = ""

    Sub clear()
        iswms = False
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
        kategorieLangtext = ""
        kategorieToolTip = ""
    End Sub
    ''' <summary>
    ''' braucht den pfad
    ''' </summary>
    ''' <returns></returns>
    Function calcMapfileFullname(mapfileTyp As String) As String
        Try
            Dim mf As String
            'mf = pfad & ebene & "_" & mapfileTyp & ".map"
            mf = strGlobals.nkat & aid & "/" & mapfileTyp & ".map"
            Return mf
        Catch ex As Exception
            l("fehler in calcMapfileFullname:", ex)
            Return ""
        End Try
    End Function
End Class
Public Class clsLegendenItem
    Property aid As Integer = 0
    Property nr As Integer = 0
    Property titel As String = ""
End Class
Public Class wmsProps
    Property aid As Integer = 0
    Property url As String = ""
    Property typ As String = "full" 'or template
    Property format As String = "h" 'html or t)ext or 
    Public Property stdlayer As String = ""
    Public Function getstring(trenn As String) As String
        Return aid & trenn & url & trenn & typ & trenn & format & trenn & stdlayer
    End Function

End Class
Public Class clsDoku
    Property aid As Integer = 0
    Property inhalt As String = ""
    Property entstehung As String = ""
    Property aktualitaet As String = ""
    Property massstab As String = ""
    Property beschraenkungen As String = ""
    Property datenabgabe As String = ""
    Property calcedOwner As String = ""

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
