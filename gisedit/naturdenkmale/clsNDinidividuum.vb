Imports System.ComponentModel
Public Class clsNDinidividuum
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
     Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(ByVal prop As String)

        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    'Public zusatz As New clsZusatzInfo
    Private _ZusatzInfo As New clsZusatzInfo
    Public Property ZusatzInfo() As clsZusatzInfo
        Get
            Return _ZusatzInfo
        End Get
        Set(ByVal value As clsZusatzInfo)
            _ZusatzInfo = value
            OnPropertyChanged("ZusatzInfo")
        End Set
    End Property


    Property gid As Integer = 0
    Property vid As String = ""
    Property aid As Integer = 0
    Property name As String = ""
    Property beschreibung As String = ""

    Property gemeinde As String = ""
    Property gemarkung As String = ""
    Property radius As Integer = 0
    'Property tk25 As String = ""
    'Property grundeigentum As String = ""
    'Property ordnungswidrig As String = ""
    Property umgebung As String = ""
    Property flaeche_qm As Double

    Property rechts As Double = 0
    Property hoch As Double = 0
    Public Property lfd_nr As Integer
    Property plakette As String = ""
    'Public Property bemerkung As String
End Class

