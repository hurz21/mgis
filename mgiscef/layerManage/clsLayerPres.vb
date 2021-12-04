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
    'Public Property LayerSichtbarkeit As Visibility = Visibility.Visible
    Protected Sub OnPropertyChanged(ByVal prop As String)
        anychange = True
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    Property kategorieOBJ As New clsUniversal
    Property justAdded As Boolean = False
    Private _sortierKriterium As String
    Public Property sortierKriterium() As String
        Get
            Return _etikett
        End Get
        Set(ByVal value As String)
            _sortierKriterium = value
        End Set
    End Property
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
    Public Property Etikett() As String
        Get
            Return _etikett
        End Get
        Set(ByVal value As String)
            _etikett = value
            OnPropertyChanged("etikett")
        End Set
    End Property

    Friend Function kopieAttributeNach(ByRef klay As clsLayerPres) As Boolean
        klay.isUserlayer = isUserlayer
        klay.aid = aid
        klay.sid = sid
        klay.ebene = ebene
        klay.titel = titel
        klay.rang = rang
        klay.mit_imap = mit_imap
        klay.masstab_imap = masstab_imap
        klay.mit_legende = mit_legende
        klay.pfad = pfad
        klay.schema = schema
        klay.schlagworte = schlagworte
        klay.standardsachgebiet = standardsachgebiet
        klay.status = status
        klay.RBischecked = isactive
        klay.mit_objekten = mit_objekten
        klay.mapFile = klay.calcMapfileFullname("layer")
        klay.mapFileHeader = klay.calcMapfileFullname("header")
        klay.suchfeld = klay.titel & " " & klay.schlagworte
        klay.schongeladen = schongeladen
        klay.myFontStyle = myFontStyle
        klay.mithaken = mithaken
        klay.kategorieLangtext = kategorieLangtext
        klay.kategorieToolTip = kategorieToolTip
        Return True
    End Function



    Public Property kategorie As String
    Public Property etikettfarbe As SolidColorBrush = Brushes.Black
    Public Property dokutext As String
    Public Property schongeladen As Integer = 0
    Public Property tabname As String = ""
    Public Property isUserlayer As Boolean = False
    'Public Property kastenHoehe As Double = 19

    Public Function convLayer2PresLayer(simpleLayer As clsLayer) As clsLayerPres
        Dim presLayer As New clsLayerPres
        presLayer.aid = simpleLayer.aid
        presLayer.sid = simpleLayer.sid
        presLayer.ebene = simpleLayer.ebene
        presLayer.titel = simpleLayer.titel
        presLayer.isHgrund = simpleLayer.isHgrund
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
        presLayer.iswms = simpleLayer.iswms
        presLayer.kategorieLangtext = simpleLayer.kategorieLangtext
        presLayer.kategorieToolTip = simpleLayer.kategorieToolTip
        Return presLayer
    End Function

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function
    Public Function SortByNameAscending(name1 As String, name2 As String) As Integer
        Return name1.CompareTo(name2)
    End Function
    Public Function CompareTo(other As clsLayerPres) As Integer Implements IComparable(Of clsLayerPres).CompareTo
        Return Me.Etikett.CompareTo(other.sortierKriterium)
    End Function

    Friend Sub clearPres()
        clear() 'vererbt
        Etikett = ""
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
        presLayer.isUserlayer = isUserlayer
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
        presLayer.schongeladen = schongeladen
        presLayer.myFontStyle = myFontStyle
        presLayer.mithaken = mithaken
        presLayer.iswms = iswms
        presLayer.isUserlayer = isUserlayer
        presLayer.isHgrund = isHgrund
        presLayer.isactive = isactive
        Return presLayer
    End Function
End Class
