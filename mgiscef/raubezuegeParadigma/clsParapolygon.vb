Imports System.ComponentModel
Public Class clsParapolygon
    Implements iRaumbezug
    Implements ICloneable
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
     Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(ByVal prop As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    Public Function ShapeSerialstringIstWKT() As Boolean
        Try
            If IsNumeric(ShapeSerial.Substring(0, 1)) Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Property WKTstring As String = ""
    Public Property mitEtikett As Boolean = False Implements iRaumbezug.MITETIKETT
    Public Function IstWKT(kandidat As String) As Boolean?
        Try
            If IsNumeric(kandidat.Substring(0, 1)) Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Property originalQuellString As String = ""

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

    Private _area As Double
    Public Property Area() As Double
        Get
            Return _area
        End Get
        Set(ByVal value As Double)
            _area = value
            OnPropertyChanged("Area")
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

    Private _name As String
    Public Property name() As String Implements iRaumbezug.name
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
            OnPropertyChanged("name")
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

    'Private _shapefile As String
    'Public Property shapefile() As String
    '    Get
    '        Return _shapefile
    '    End Get
    '    Set(ByVal value As String)
    '        _shapefile = value
    '        OnPropertyChanged("shapefile")
    '    End Set
    'End Property

    Private _objectidspalte As String
    Public Property objectidspalte() As String
        Get
            Return _objectidspalte
        End Get
        Set(ByVal Value As String)
            _objectidspalte = Value
        End Set
    End Property

    Private _objectid As Long
    Public Property objectid() As Long
        Get
            Return _objectid
        End Get
        Set(ByVal value As Long)
            _objectid = value
        End Set
    End Property
    Public Property GKstringList As New List(Of String)
    ''' <summary>
    ''' die gauss-krüger als delimited string
    ''' </summary>
    ''' <remarks></remarks>
    Private _GKstring As String
    Public Property GKstring() As String
        Get
            Return _GKstring
        End Get
        Set(ByVal value As String)
            _GKstring = value
            OnPropertyChanged("GKstring")
        End Set
    End Property
    Property serials As New List(Of String)

    ''' <summary>
    ''' der serialisierte aktuelle shape (wie mapwingis)
    ''' </summary>
    ''' <remarks></remarks>
    Private _ShapeSerial As String
    Public Property ShapeSerial() As String
        Get
            Return _ShapeSerial
        End Get
        Set(ByVal value As String)
            _ShapeSerial = value
            OnPropertyChanged("ShapeSerial")
        End Set
    End Property


    Private _myPoly As New Polygon
    Public Property myPoly() As Polygon
        Get
            Return _myPoly
        End Get
        Set(ByVal value As Polygon)
            _myPoly = value
            OnPropertyChanged("myPoly")
        End Set
    End Property

    Public Sub serialAusGkstring_generieren(typ As RaumbezugsTyp)
        If GKstring IsNot Nothing Then
            Dim s As String
            If typ = RaumbezugsTyp.Polygon Then
                s = GKstring.Replace(";", "|")
                ShapeSerial = "5;0;" & s
            End If
            If typ = RaumbezugsTyp.Polyline Then
                s = GKstring.Replace(";", "|")
                ShapeSerial = "3;0;" & s
            End If
        End If
    End Sub
    Sub clear()
        Freitext = ""
        Typ = RaumbezugsTyp.Polygon
        abstract = ""
        name = ""
        punkt.X = 0
        punkt.Y = 0
        SekID = 0
        box.xl = 0
        box.xh = 0
        box.yl = 0
        box.yh = 0
        punkt.X = 0
        punkt.Y = 0
        FlaecheQm = 0
        LaengeM = 0
        ShapeSerial = ""
        WKTstring = ""
        GKstring = ""
    End Sub
    Public Function defineAbstract() As String
        abstract = clsString.Capitalize("Polygon ")
        If Typ = RaumbezugsTyp.Polyline Then
            abstract = clsString.Capitalize("Polyline ")
        End If
        Return abstract
    End Function
    Public Function defineBboxfromGKstring(ByRef xmin As Double, ByRef xmax As Double, ByRef ymin As Double, ByRef ymax As Double) As Boolean
        Dim coods$()
        If String.IsNullOrEmpty(GKstring) Then
            nachricht("defineBboxfromGKstring: GKstring ist nothing: ")
            Return False
        End If
        Try
            coods = GKstring.Split(";"c)
            If GKstring Is Nothing Then Return False
            If String.IsNullOrEmpty(GKstring) Then Return False
            xmin = 10000000
            ymin = 1000000000
            xmax = -10000000000
            ymax = -1000000000
            For i = 0 To coods.GetUpperBound(0) - 1 Step 2
                If CDbl(coods(i)) < xmin Then xmin = CDbl(coods(i))
                If CDbl(coods(i + 1)) < ymin Then ymin = CDbl(coods(i + 1))
                If CDbl(coods(i)) > xmax Then xmax = CDbl(coods(i))
                If CDbl(coods(i + 1)) > ymax Then ymax = CDbl(coods(i + 1))
            Next
            Return True
        Catch ex As Exception
            nachricht("fehler in defineBboxfromGKstring: ", ex)
            Return False
        End Try
    End Function
End Class
