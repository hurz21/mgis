Imports System.ComponentModel
Public Class ParaPunkt
    Implements iRaumbezug
    Implements ICloneable
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
     Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(ByVal prop As String)
        anychange = True
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    Public anychange As Boolean
    Public Property mitEtikett As Boolean = False Implements iRaumbezug.MITETIKETT
    Private _FlaecheQm As Double
    Public Property FlaecheQm() As Double Implements iRaumbezug.FLAECHEQM
        Get
            Return _FlaecheQm
        End Get
        Set(ByVal value As Double)
            _FlaecheQm = value
            OnPropertyChanged("FlaecheQm")
        End Set
    End Property

    Private _LaengeM As Double
    Public Property LaengeM() As Double Implements iRaumbezug.LAENGEM
        Get
            Return _LaengeM
        End Get
        Set(ByVal value As Double)
            _LaengeM = value
            OnPropertyChanged("LaengeM")
        End Set
    End Property

    Private _isMapEnabled As Boolean = True
    Public Property isMapEnabled() As Boolean Implements iRaumbezug.isMapEnabled
        Get
            Return _isMapEnabled
        End Get
        Set(ByVal value As Boolean)
            _isMapEnabled = value
            OnPropertyChanged("isMapEnabled")
        End Set
    End Property


    Public Function punktisvalid() As Boolean Implements iRaumbezug.PunktIsValid
        If punkt.X < 10000 Then Return False
        If punkt.Y < 10000 Then Return False
        Return True
    End Function

    Private _Freitext As String
    Public Property Freitext() As String Implements iRaumbezug.Freitext
        Get
            Return _Freitext
        End Get
        Set(ByVal value As String)
            _Freitext = value
            OnPropertyChanged("Freitext")
        End Set
    End Property

    Private _status As Integer
    Public Property Status As Integer Implements iRaumbezug.Status
        Get
            Return _status
        End Get
        Set(ByVal Value As Integer)
            _status = Value
            OnPropertyChanged("Status")
        End Set
    End Property

    Private _sekID As Long
    Public Property SekID() As Long Implements iRaumbezug.SekID
        Get
            Return _sekID
        End Get
        Set(ByVal Value As Long)
            _sekID = Value
            OnPropertyChanged("SekID")
        End Set
    End Property

    Private _raumbezugsid As Long
    Public Property RaumbezugsID() As Long Implements iRaumbezug.id
        Get
            Return _raumbezugsid
        End Get
        Set(ByVal value As Long)
            _raumbezugsid = value
            OnPropertyChanged("RaumbezugsID")
        End Set
    End Property
    Private _box As New clsRange
    Public Property box() As clsRange Implements iRaumbezug.box
        Get
            Return _box
        End Get
        Set(ByVal value As clsRange)
            _box = value
            OnPropertyChanged("box")
        End Set
    End Property
    ''' <summary>
    ''' ergibt sich aus der suchangabe: dreieich, am hasenpfad 1
    ''' </summary>
    ''' <remarks></remarks>
    Private _abstract As String
    Public Property abstract() As String Implements iRaumbezug.abstract
        Get
            Return _abstract
        End Get
        Set(ByVal Value As String)
            _abstract = Value
            OnPropertyChanged("abstract")
        End Set
    End Property
    ''' <summary>
    ''' z.B. hier emissionsquelle
    ''' </summary>
    ''' <remarks></remarks>
    Private _name As String
    Public Property Name() As String Implements iRaumbezug.name
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
            OnPropertyChanged("Name")
        End Set
    End Property
    Private _punkt As New myPoint
    Public Property punkt() As myPoint Implements iRaumbezug.punkt
        Get
            Return _punkt
        End Get
        Set(ByVal value As myPoint)
            _punkt = value
            OnPropertyChanged("punkt")
        End Set
    End Property
    Private _typ As RaumbezugsTyp   'macht hier keinen sinn- besser neue variable eine etage höher
    Public Property Typ() As RaumbezugsTyp Implements iRaumbezug.typ
        Get
            Return _typ
        End Get
        Set(ByVal value As RaumbezugsTyp)
            _typ = value
            OnPropertyChanged("Typ")
        End Set
    End Property
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function
    Public Function defineAbstract() As String
        abstract = Name & " " & setcoordsAbstract()
        Return abstract
    End Function

    Public Function setcoordsAbstract() As String
        coordsAbstract = punkt.X & " , " & punkt.Y
        Return coordsAbstract
    End Function
    Private _coordsAbstract As String
    Public Property coordsAbstract() As String
        Get
            Return _coordsAbstract
        End Get
        Set(ByVal Value As String)
            _coordsAbstract = Value
            OnPropertyChanged("coordsAbstract")
        End Set
    End Property

    Public Function clear() As Boolean
        Name = ""
        Freitext = ""
        punkt.X = 0
        punkt.Y = 0
        SekID = 0
        box.xl = 0
        box.xh = 0
        box.yl = 0
        box.yh = 0
        punkt.X = 0
        punkt.Y = 0
        Return True
    End Function


End Class
Public Class clsParaUmkreis
    Inherits ParaPunkt
    'typ 7
    Sub New()
        Radius = 100
    End Sub

    Private _Radius As Integer
    Public Property Radius() As Integer
        Get
            Return _Radius
        End Get
        Set(ByVal value As Integer)
            _Radius = value
            OnPropertyChanged("Radius")
        End Set
    End Property
    Public Shadows Function clear() As Boolean
        Radius = 0
        MyBase.clear()
        Return True
    End Function

    Public Shadows Function defineAbstract() As String
        abstract = String.Format("{0} , Radius={1}", Name, Radius)
        Return abstract
    End Function
End Class
