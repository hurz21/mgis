Imports System.ComponentModel
Public Class clsNDgruppe
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
    Property aid As Integer = 0
    Property name As String = ""
    Property beschreibung As String = ""
    Property kreis As String = ""
    Property gemeinde As String = ""
    Property gemarkung As String = ""
    Property schutzgrund As String = ""
    Property tk25 As String = ""
    Property grundeigentum As String = ""
    Property ordnungswidrig As String = ""
    Property umgebung As String = ""
    Property veroeffentlicht As String = ""
    Property erloschen As String = ""
    Property veroeff_geloescht As String = ""
    Property flur As String = ""
    Property flurstueck As String = ""
    Property rechtswert As Double = 0
    Property hochwert As Double = 0
    Property stammumfang As String
    Property hoehe As String
    Property kronenbreite As String

End Class

