Imports System.ComponentModel
Imports mgis

Public Class clsLayerPres
    Inherits clsLayer
    Implements INotifyPropertyChanged, ICloneable
    Implements IComparable(Of clsLayerPres)

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
     Implements INotifyPropertyChanged.PropertyChanged
    ' Default comparer for Part.
    Public Property RBsichtbarkeit As Visibility = Visibility.Visible
    Protected Sub OnPropertyChanged(ByVal prop As String)
        anychange = True
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    Public anychange As Boolean
    '   Property thumbnailFullPath As String

    Private _thumbnailFullPath As String
    Public Property thumbnailFullPath() As String
        Get
            Return _thumbnailFullPath
        End Get
        Set(ByVal value As String)
            _thumbnailFullPath = value
            OnPropertyChanged("thumbnailFullPath")
        End Set
    End Property

    Property myFontStyle As FontStyle = FontStyles.Normal
    Property myFontWeight As FontWeight = FontWeights.Normal
    Public Property farbe As New SolidColorBrush
    Property tultipp As String = ""
    Private _mithaken As Boolean = False
    Public Property mithaken() As Boolean
        Get
            Return _mithaken
        End Get
        Set(ByVal value As Boolean)
            _mithaken = value
            OnPropertyChanged("mithaken")
        End Set
    End Property

    ''' <summary>
    ''' wird durch 'RBsichtbarkeit' sichtbar
    ''' </summary>
    Private _RBischecked As Boolean = False
    Public Property RBischecked() As Boolean
        Get
            Return _RBischecked
        End Get
        Set(ByVal value As Boolean)
            _RBischecked = value
            OnPropertyChanged("RBischecked")
        End Set
    End Property

    Private _etikett As String = ""
    Public Property SortierKriterium() As String
        Get
            Return _etikett
        End Get
        Set(ByVal value As String)
            _etikett = value
            OnPropertyChanged("etikett")
        End Set
    End Property

    Public Property kategorie As String
    Public Property etikettfarbe As SolidColorBrush
    Public Property dokutext As String
    Public Property schongeladen As Integer = 0
    Public Property tabname As String = ""

    Public Function convLayer2PresLayer(simpleLayer As clsLayer) As clsLayerPres
        Dim presLayer As New clsLayerPres
        presLayer.aid = simpleLayer.aid
        presLayer.sid = simpleLayer.sid
        presLayer.ebene = simpleLayer.ebene
        presLayer.titel = simpleLayer.titel
        presLayer.rang = simpleLayer.rang
        presLayer.mit_imap = simpleLayer.mit_imap
        presLayer.masstab_imap = simpleLayer.masstab_imap
        presLayer.mit_legende = simpleLayer.mit_legende
        presLayer.pfad = simpleLayer.pfad
        presLayer.schema = simpleLayer.schema
        presLayer.schlagworte = simpleLayer.schlagworte
        presLayer.standardsachgebiet = simpleLayer.standardsachgebiet
        presLayer.status = simpleLayer.status
        presLayer.RBischecked = simpleLayer.isactive
        presLayer.mit_objekten = simpleLayer.mit_objekten
        presLayer.mapFile = presLayer.calcMapfileFullname("layer")
        presLayer.mapFileHeader = presLayer.calcMapfileFullname("header")
        presLayer.suchfeld = presLayer.titel & " " & presLayer.schlagworte
        Return presLayer
    End Function

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function
    Public Function SortByNameAscending(name1 As String, name2 As String) As Integer
        Return name1.CompareTo(name2)
    End Function
    Public Function CompareTo(other As clsLayerPres) As Integer Implements IComparable(Of clsLayerPres).CompareTo
        Return Me.SortierKriterium.CompareTo(other.SortierKriterium)
    End Function

    Friend Sub clearPres()
        clear() 'vererbt
        SortierKriterium = ""
        RBsichtbarkeit = Visibility.Collapsed
        mithaken = False
        RBischecked = False
        mit_imap = False
        farbe = Brushes.White
        tultipp = ""
        suchfeld = ""
        dokutext = ""
    End Sub

    Friend Function kopie() As clsLayerPres
        Dim presLayer As New clsLayerPres
        presLayer.aid = aid
        presLayer.sid = sid
        presLayer.ebene = ebene
        presLayer.titel = titel
        presLayer.rang = rang
        presLayer.mit_imap = mit_imap
        presLayer.masstab_imap = masstab_imap
        presLayer.mit_legende = mit_legende
        presLayer.pfad = pfad
        presLayer.schema = schema
        presLayer.schlagworte = schlagworte
        presLayer.standardsachgebiet = standardsachgebiet
        presLayer.status = status
        presLayer.RBischecked = isactive
        presLayer.mit_objekten = mit_objekten
        presLayer.mapFile = presLayer.calcMapfileFullname("layer")
        presLayer.mapFileHeader = presLayer.calcMapfileFullname("header")
        presLayer.suchfeld = presLayer.titel & " " & presLayer.schlagworte
        Return presLayer
    End Function
End Class
