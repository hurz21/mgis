Public Class clsMapSpec
    Public Property layer() As String

    Private _aktrange As New clsRange
    Public Property aktrange As clsRange
        Get
            Return _aktrange
        End Get
        Set(ByVal Value As clsRange)
            _aktrange = Value
        End Set
    End Property
    Private _aktcanvas As New clsCanvas
    Public Property aktcanvas As clsCanvas
        Get
            Return _aktcanvas
        End Get
        Set(ByVal Value As clsCanvas)
            _aktcanvas = Value
        End Set
    End Property
    Public Property ActiveLayer() As String
    Public Property ActiveLayerTitel() As String
    Public Property HgrundTitel() As String
    Private _hgrund As String
    Public Property Hgrund As String
        Get
            Return _hgrund
        End Get
        Set(ByVal Value As String)
            _hgrund = Value
            setLayers()
        End Set
    End Property

    Private _Vgrund As String
    Public Property Vgrund As String
        Get
            Return _Vgrund
        End Get
        Set(ByVal Value As String)
            _Vgrund = Value
            setLayers()
        End Set
    End Property
    Sub setLayers()
        layer = String.Format("{0};{1};", Vgrund, Hgrund)
        layer = layer.Replace(";;", ";")
    End Sub
    sub clear()
        Vgrund=""
        Hgrund=""
        ActiveLayer=""
        ActiveLayerTitel=""
        HgrundTitel=""
        aktcanvas.clear
        aktrange.clear
    End Sub
End Class
