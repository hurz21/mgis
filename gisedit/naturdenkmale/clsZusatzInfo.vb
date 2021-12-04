Imports System.ComponentModel

Public Class clsZusatzInfo
    Implements INotifyPropertyChanged


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
     Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(ByVal prop As String)

        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    'Property paradigmaVID As String = ""

    Private _paradigmaVID As String
    Public Property paradigmaVID() As String
        Get
            Return _paradigmaVID
        End Get
        Set(ByVal value As String)
            _paradigmaVID = value
            OnPropertyChanged("paradigmaVID")
        End Set
    End Property
    Property auge As Date

    Property regelkontrolle As Date
    Property untersuchung As Date
    Property kronensicherung As Boolean = False
    Property ablaufdatumks As Date
    Property bemerkung As String = ""
    Public Property verkehrssicher As Boolean = False
End Class
