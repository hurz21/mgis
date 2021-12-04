Imports System.ComponentModel
Public Class clsAktenzeichen
    Implements INotifyPropertyChanged
    Implements ICloneable

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
    Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(ByVal prop As String)
        anychange = True
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    Public anychange As Boolean

    Private _sachgebiet As New AktenzeichenSachgebiet
    Public Property sachgebiet() As AktenzeichenSachgebiet
        Get
            Return _sachgebiet
        End Get
        Set(ByVal Value As AktenzeichenSachgebiet)
            _sachgebiet = Value
            OnPropertyChanged("sachgebiet")
        End Set
    End Property

    Private _gesamt As string
    Property gesamt() As string
        Get
            Return _gesamt
        End Get
        Set(ByVal value  As string)
            _gesamt = value
            OnPropertyChanged("gesamt")
        End Set
    End Property

    Private _stamm$ = "II-67"
    Public Property stamm() As String
        Get
            Return _stamm
        End Get
        Set(ByVal value As String)
            _stamm$ = value
            OnPropertyChanged("stamm")
        End Set
    End Property

    Public Shared Function istNeu() As Boolean
        'todo hier db-abfrage durchführen
        Return True
    End Function

    Private _verfasser$
    Public Property verfasser() As String
        Get
            Return _verfasser$
        End Get
        Set(ByVal value As String)
            _verfasser$ = value
            OnPropertyChanged("verfasser")
        End Set
    End Property
    Private _schreiber$
    Public Property schreiber() As String
        Get
            Return _schreiber$
        End Get
        Set(ByVal value As String)
            _schreiber$ = value
            OnPropertyChanged("schreiber")
        End Set
    End Property
    Private _vorgangsbeschreibung As String
    Public Property Prosa() As String
        Get
            Return _vorgangsbeschreibung
        End Get
        Set(ByVal Value As String)
            _vorgangsbeschreibung = Value
            OnPropertyChanged("Prosa")
        End Set
    End Property

    ''' <summary>
    ''' ist nur die laufenden nur in einem sachgebiet
    ''' </summary>
    ''' <remarks></remarks>
    Private _vorgangsnummer As Integer
    Public Property Vorgangsnummer() As Integer
        Get
            Return _vorgangsnummer
        End Get
        Set(ByVal Value As Integer)
            _vorgangsnummer = Value
            OnPropertyChanged("Vorgangsnummer")
        End Set
    End Property


    'Private _laufnr%
    'Public Property laufnr() As Integer
    '    Get
    '        Return _laufnr%
    '    End Get
    '    Set(ByVal value As Integer)
    '        _laufnr% = value
    '        OnPropertyChanged("laufnr")
    '    End Set
    'End Property

    Public Function AZ_concat(ByVal initiale$) As String 'myGlobalz.sitzung.Bearbeiter.Initiale
        Dim d$ = "-"
        '	vorgangsbeschreibung = klammerraus(vorgangsbeschreibung)
        'gesamt$ = stamm & d$ & sachgebiet.Nummer & d$ & Vorgangsnummer & d$ & vorgangsbeschreibung & d$ & verfasser & d$ & schreiber
        gesamt$ = stamm & d$ & sachgebiet.Zahl & d$ & Prosa & d$ & initiale  '& d$ & schreiber
        Return gesamt$
    End Function

    Public Function AZ_concatNEU(ByVal initiale$) As String 'myGlobalz.sitzung.Bearbeiter.Initiale
        Dim d$ = "-"
        '	vorgangsbeschreibung = klammerraus(vorgangsbeschreibung)
        'gesamt$ = stamm & d$ & sachgebiet.Nummer & d$ & Vorgangsnummer & d$ & vorgangsbeschreibung & d$ & verfasser & d$ & schreiber
        gesamt$ = stamm & d$ & "XXXXX" & d$ & sachgebiet.Zahl & d$ & Format(Now, "yyyy") & d$ & initiale & d$ & Prosa '& d$ & schreiber
        Return gesamt$
    End Function


    Public Function AZ_concat_stamm() As String
        Dim d$ = "-"
        Return stamm & d$ & sachgebiet.Zahl & d$ & Prosa     '& d$ & schreiber
    End Function

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function

    Sub clear()
        Vorgangsnummer = 0
        Prosa = ""
        schreiber = ""
        verfasser = ""
        gesamt = ""
        stamm = ""
        sachgebiet.clear()
    End Sub
End Class
