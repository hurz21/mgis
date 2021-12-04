Imports System.ComponentModel
Imports LIBstammdatenCRUD

Public Class AktenzeichenSachgebiet
    Implements INotifyPropertyChanged
    Implements ICloneable
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
     Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(ByVal prop As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub

    Private _Zahl As String
    Public Property Zahl() As String
        Get
            Return _Zahl
        End Get
        Set(ByVal Value As String)
            _Zahl = Value
            OnPropertyChanged("Zahl")
        End Set
    End Property

    Private _header As String

    Public Sub New()

    End Sub

    Public Property Header() As String
        Get
            Return _header
        End Get
        Set(ByVal Value As String)
            _header = Value
            OnPropertyChanged("Header")
        End Set
    End Property
    Public Property isUNB() As Boolean
    Public Property isImmischionschutz() As Boolean

    Public Function checkObNumerischerVorgang() As Boolean
        '	Return False
        If String.IsNullOrEmpty(Zahl) Then
            Return False
        End If
        If Zahl.StartsWith("5") Or Zahl.StartsWith("6") Or Zahl.StartsWith("7") Then
            isUNB = True
            'Return True
        Else
            isUNB = False
            '	Return False
        End If
        If Zahl.Length < 4 Then
            'fdumwelt
            If Zahl.StartsWith("0") Or Zahl.StartsWith("1") Or Zahl.StartsWith("2") Or _
              Zahl.StartsWith("3") Or Zahl.StartsWith("8") Then
                isImmischionschutz = True
                Return True
            Else
                isImmischionschutz = False
                Return False
            End If
            'Else wasserbehörde
        End If

        Return False
    End Function

    Sub clear()
        Zahl = ""
        Header = ""
        isUNB = False
    End Sub


End Class