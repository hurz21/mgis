Imports System.ComponentModel
Imports System.Data

Public Class ParaAdresse
    Implements iRaumbezug
    Implements ICloneable
    Implements INotifyPropertyChanged
    Implements IDisposable
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

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
     Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(ByVal prop As String)
        anychange = True
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    Public anychange As Boolean

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

    Public Sub KopieVonParaAdresse(ByVal quelle As ParaAdresse) 'myGlobalz.sitzung.aktADR,  Gisadresse.gemeindeName 
        Gisadresse.gemeindeName = quelle.Gisadresse.gemeindeName
        Gisadresse.gemeindeNr = quelle.Gisadresse.gemeindeNr
        Gisadresse.strasseName = quelle.Gisadresse.strasseName
        Gisadresse.HausKombi = quelle.Gisadresse.HausKombi
    End Sub
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
    Private _fS As String
    Public Property FS() As String
        Get
            Return _fS
        End Get
        Set(ByVal Value As String)
            _fS = Value
            OnPropertyChanged("FS")
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
    '<System.Obsolete("Use RaumbezugsID instead."), _
    'EditorBrowsable(EditorBrowsableState.Never)> _
    'Public Property id() As Long
    '  Get
    '    Return RaumbezugsID
    '  End Get
    '  Set(ByVal value As Long)
    '    RaumbezugsID = value
    '  End Set
    'End Property
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
    Public Property Name() As String Implements iRaumbezug.name 'wurde in der datenbank in TITEL umbenannt
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
    Private _gisadresse As New clsAdress("")
    Public Property Gisadresse() As clsAdress
        Get
            Return _gisadresse
        End Get
        Set(ByVal value As clsAdress)
            _gisadresse = value
            OnPropertyChanged("Gisadresse")
        End Set
    End Property
    Private _pLZ As String ' geändert von integer auf string wg. ddr und urbanke
    Public Property PLZ() As String
        Get
            Return _pLZ
        End Get
        Set(ByVal Value As String)
            _pLZ = Value
            OnPropertyChanged("PLZ")
        End Set
    End Property
    Private _postfach As String
    Public Property Postfach() As String
        Get
            Return _postfach
        End Get
        Set(ByVal Value As String)
            _postfach = Value
            OnPropertyChanged("Postfach")
        End Set
    End Property
    Private _adresstyp As adressTyp
    Public Property Adresstyp() As adressTyp
        Get
            Return _adresstyp
        End Get
        Set(ByVal Value As adressTyp)
            _adresstyp = Value
            OnPropertyChanged("Adresstyp")
        End Set
    End Property
    Public Function defineAbstract() As String
        If PLZ Is Nothing Then
            abstract = clsString.Capitalize(Gisadresse.gemeindeName) & ", " &
                Postfach & " " &
                Gisadresse.strasseName & " " &
                Gisadresse.HausKombi & " "
        Else
            abstract = "" & PLZ & " " & clsString.Capitalize(Gisadresse.gemeindeName) & ", " &
            Postfach & " " &
            Gisadresse.strasseName & " " &
            Gisadresse.HausKombi & " "
        End If
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
        Gisadresse.clear()
        PLZ = ""
        Postfach = ""
        FS = ""
        Name = ""
        punkt.X = 0
        punkt.Y = 0
        SekID = 0
        box.xl = 0
        box.xh = 0
        box.yl = 0
        box.yh = 0
        punkt.X = 0
        punkt.Y = 0
        coordsAbstract = ""
        abstract = ""
        Freitext = ""
        Return True
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean ' So ermitteln Sie überflüssige Aufrufe
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                _Gisadresse.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
