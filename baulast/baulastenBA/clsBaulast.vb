Public Class clsBaulast
    Property geloescht As Boolean = False
    Property katasterFormellOK As Boolean = False
    Property blattnr As String
    'Property feld2 As String
    Property status As String '1- eintrag, 2 änderung 4 verzicht gelöscht
    Property datei As String

    Property dateiExistiert As Boolean
    Property bauortNr As String
    Property gemeindeText As String
    Property probaugFST As New clsFlurstueck
    Property katFST As New clsFlurstueck
    Property gueltig As String

    Public Property laufnr As Integer
    Public Property ka1 As String
    Public Property ka2 As String
    Public Property hatTiff As Boolean
    Public Property baulastnr As String
    Public Property datum As String
    Public Property katastergemarkungText As String
    Public Property serial As String
    Public Property gefundenIn As String
    Public Property datum1 As String = ""
    Public Property datumgeloescht As String = ""
End Class
Public Class myComboBoxItem
    Property mySttring As String
    Property myindex As String
End Class
