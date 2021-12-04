Imports System.ComponentModel

''' <summary>
''' x,y, as double
''' </summary>
''' <remarks></remarks>
Public Class myPoint
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
    Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(ByVal prop As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    Property z As Double = 0
    Private _x As Double
    Public Property X() As Double
        Get
            Return _x
        End Get
        Set(ByVal Value As Double)
            _x = Value

            OnPropertyChanged("X")
        End Set
    End Property
    Private _y As Double
    Public Property Y() As Double
        Get
            Return _y
        End Get
        Set(ByVal Value As Double)
            _y = Value
            'strY = CType(Value, String)
            OnPropertyChanged("Y")
        End Set
    End Property
    Overrides Function toString() As String
        Return String.Format("{0}, {1}", X, Y)
    End Function
    'Property strX As String
    Private _strX As String
    Public Property strX() As String
        Get
            Return _strX
        End Get
        Set(ByVal value As String)
            _strX = value
            X = CDbl(_strX.Replace(",", "."))
        End Set
    End Property
    'Property strY As String
    Private _strY As String
    Public Property strY() As String
        Get
            Return _strY
        End Get
        Set(ByVal value As String)
            _strY = value
            Y = CDbl(_strY.Replace(",", "."))
        End Set
    End Property
    Sub New()
        X = 0
        Y = 0
    End Sub
    Public Sub SetToInteger()
        X = Int(Math.Round(X))
        Y = Int(Math.Round(Y))
    End Sub
    Function isValid() As Boolean
        Try
            If X = Nothing Then Return False
            If Y = Nothing Then Return False
            If X < 10 Then Return False
            If Y < 10 Then Return False
            If CStr(X).StartsWith("32") Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class
