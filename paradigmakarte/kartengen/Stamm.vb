Imports System.ComponentModel
Public Class Stamm
    Implements INotifyPropertyChanged
    Implements ICloneable

    Private Property nulldatum As Date

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
     Implements INotifyPropertyChanged.PropertyChanged



    Private _InterneNr As String
    Public Property InterneNr() As String
        Get
            Return _InterneNr
        End Get
        Set(ByVal value As String)
            _InterneNr = value
            OnPropertyChanged("InterneNr")
        End Set
    End Property


    Private _hatraumbezug As Boolean
    Public Property hatraumbezug() As Boolean
        Get
            Return _hatraumbezug
        End Get
        Set(ByVal value As Boolean)
            _hatraumbezug = value
            OnPropertyChanged("hatraumbezug")
        End Set
    End Property

    Protected Sub OnPropertyChanged(ByVal prop As String)
        anychange = True
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub

    Public anychange As Boolean




    Private _Paragraf As String
    Public Property Paragraf() As String
        Get
            Return _Paragraf
        End Get
        Set(ByVal value As String)
            _Paragraf = value
            OnPropertyChanged("Paragraf")
        End Set
    End Property


    ''' <summary>
    ''' Abgabe an die bauaufsicht
    ''' </summary>
    ''' <remarks></remarks>
    Private _AbgabeBA As Boolean
    Public Property AbgabeBA() As Boolean
        Get
            Return _AbgabeBA
        End Get
        Set(ByVal value As Boolean)
            _AbgabeBA = value
            OnPropertyChanged("AbgabeBA")
        End Set
    End Property

    Private _darfNichtVernichtetWerden As Boolean
    Public Property darfNichtVernichtetWerden() As Boolean
        Get
            Return _darfNichtVernichtetWerden
        End Get
        Set(ByVal value As Boolean)
            _darfNichtVernichtetWerden = value
            OnPropertyChanged("darfNichtVernichtetWerden")
        End Set
    End Property


    Sub New(ByVal _nulldateum As Date)
        nulldatum = _nulldateum
        clear()
    End Sub
    Public Property ArchivSubdir() As String

    Function createArchivsubdir(ByVal tarchivrootdir As String, tArchivSubdir As String) As Boolean 'myGlobalz.Arc.rootDir.ToString
        Try

            Dim testdir As New IO.DirectoryInfo(tarchivrootdir & tArchivSubdir)
            If Not testdir.Exists Then testdir.Create()
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private _WeiterBearbeiter As String
    Public Property WeitereBearbeiter() As String
        Get
            Return _WeiterBearbeiter
        End Get
        Set(ByVal value As String)
            _WeiterBearbeiter = value
            OnPropertyChanged("WeitereBearbeiter")
        End Set
    End Property




    ''' <summary>
    ''' vorsicht, entspricht nicht dem paradigma
    ''' </summary>
    ''' <remarks></remarks>
    Private _bearbeiter As New clsBearbeiter
    Public Property hauptBearbeiter() As clsBearbeiter
        Get
            Return _bearbeiter
        End Get
        Set(ByVal Value As clsBearbeiter)
            _bearbeiter = Value
            OnPropertyChanged("Bearbeiter")
        End Set
    End Property

    ''' <summary>
    ''' das alte oder alternative aktenzeichen (z.b. wgs)
    ''' </summary>
    ''' <remarks></remarks>
    Private _AltAz As String
    Public Property AltAz() As String
        Get
            Return _AltAz
        End Get
        Set(ByVal value As String)
            _AltAz = value
            OnPropertyChanged("AltAz")
        End Set
    End Property

    Private _probaugaz As String
    Public Property Probaugaz() As String
        Get
            Return _probaugaz
        End Get
        Set(ByVal Value As String)
            _probaugaz = Value
            OnPropertyChanged("Probaugaz")
        End Set
    End Property


    Private _GemKRZ As String
    Public Property GemKRZ() As String
        Get
            Return _GemKRZ
        End Get
        Set(ByVal value As String)
            _GemKRZ = value
            OnPropertyChanged("GemKRZ")
        End Set
    End Property


    Private _erledigt As New Boolean

    Public Property erledigt() As Boolean
        Get
            Return _erledigt
        End Get
        Set(ByVal Value As Boolean)
            _erledigt = Value
            OnPropertyChanged("erledigt")
        End Set
    End Property
  

    Private _letzteBearbeitung As New Date
    Public Property LetzteBearbeitung() As Date
        Get
            Return _letzteBearbeitung
        End Get
        Set(ByVal Value As Date)
            _letzteBearbeitung = Value
            OnPropertyChanged("LetzteBearbeitung")
        End Set
    End Property

    Private _eingangsdatum As New Date
    Public Property Eingangsdatum() As Date
        Get
            Return _eingangsdatum
        End Get
        Set(ByVal Value As Date)
            _eingangsdatum = Value
            OnPropertyChanged("Eingangsdatum")
        End Set
    End Property

    Private _bemerkung As String
    Public Property Bemerkung() As String
        Get
            Return _bemerkung
        End Get
        Set(ByVal Value As String)
            _bemerkung = Value
            OnPropertyChanged("Bemerkung")
        End Set
    End Property

    Private _beschreibung As String
    Public Property Beschreibung() As String
        Get
            Return _beschreibung
        End Get
        Set(ByVal Value As String)
            _beschreibung = Value
            OnPropertyChanged("Beschreibung")
        End Set
    End Property





    Private _az As New clsAktenzeichen
    Property az() As clsAktenzeichen
        Get
            Return _az
        End Get
        Set(ByVal value As clsAktenzeichen)
            _az = value
        End Set
    End Property
    Public Property ID() As Long
    Private _ortstermin As Boolean
    Public Property Ortstermin() As Boolean
        Get
            Return _ortstermin
        End Get
        Set(ByVal Value As Boolean)
            _ortstermin = Value
            OnPropertyChanged("Ortstermin")
        End Set
    End Property
    Private _stellungnahme As Boolean
    Public Property Stellungnahme() As Boolean
        Get
            Return _stellungnahme
        End Get
        Set(ByVal Value As Boolean)
            _stellungnahme = Value
            OnPropertyChanged("Stellungnahme")
        End Set
    End Property
    Private _lastActionHeroe As String
    Public Property LastActionHeroe() As String
        Get
            Return _lastActionHeroe
        End Get
        Set(ByVal Value As String)
            _lastActionHeroe = Value
            OnPropertyChanged("LastActionHeroe")
        End Set
    End Property

    Private _Aufnahmedatum As New Date
    Public Property Aufnahmedatum() As Date
        Get
            Return _Aufnahmedatum
        End Get
        Set(ByVal value As Date)
            _Aufnahmedatum = value
            OnPropertyChanged("Aufnahmedatum")
        End Set
    End Property


    Sub clear()
        Beschreibung = ""
        Bemerkung = ""
        Stellungnahme = False
        Ortstermin = False
        LastActionHeroe = ""
        AbgabeBA = False
        hatraumbezug = False

        Aufnahmedatum = nulldatum
        Eingangsdatum = nulldatum
        LetzteBearbeitung = nulldatum

        darfNichtVernichtetWerden = False
        Probaugaz = ""
        GemKRZ = ""
        AltAz = ""
        WeitereBearbeiter = ""
        ArchivSubdir = ""
        Paragraf = ""
        InterneNr = ""


        az.clear()
        erledigt = False
    End Sub

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class
