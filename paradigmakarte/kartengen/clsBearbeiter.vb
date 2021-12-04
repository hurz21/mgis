Public Class clsBearbeiter
    Inherits Person
    Implements ICloneable

    Private _username As String
    Property gruppentext As String = ""
    Property istausgewaehlt As Boolean = false
    Property ImageFilePath As String=""
    Public Property username() As String
        Get
            Return _username
        End Get
        Set(ByVal Value As String)
            _username = Value
            OnPropertyChanged("username")
        End Set
    End Property

    Public Property ID As Integer
    Public Property GISPassword() As String


    Public Property Rang() As String
    ''' <summary>
    ''' legt fest welche gruppen das standardmaessig auf die vorgaenge zugreifen duerfen
    ''' </summary>
    ''' <remarks></remarks>
    Private _STDGRANTS As String = ""
    Public Property STDGRANTS() As String
        Get
            Return _STDGRANTS
        End Get
        Set(ByVal value As String)
            _STDGRANTS = value
            OnPropertyChanged("STDGRANTS")
        End Set
    End Property


    Private _initiale As String = ""
    Public Property Initiale() As String
        Get
            Return _initiale
        End Get
        Set(ByVal Value As String)
            _initiale = Value
            OnPropertyChanged("Initiale")
        End Set
    End Property

    Public Function getInitial() As String
        If String.IsNullOrEmpty(Name) Then
            Name = "???"
        End If
        If String.IsNullOrEmpty(Vorname) Then
            Vorname = "???"
        End If
        Try
            Initiale = (Name.Substring(0, 3) & Vorname.Substring(0, 1)).ToLower
            Return Initiale
        Catch ex As Exception
            Return "???"
        End Try
    End Function
    ''' <summary>
    ''' Rites sind hier die gruppenzugehörigkeiten des bearbeiters
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Rites() As String

    Property Kuerzel2Stellig As String

    Sub bearbeiterclear()
        username = ""
        Initiale = ""
        Rites = ""
        STDGRANTS = ""
        Kuerzel2Stellig=""
        ID = 0
    End Sub

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class
