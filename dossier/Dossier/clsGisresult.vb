Public Class clsGisresult
    Property etikett As String
    Property verordnung As String
    Property dateibeschreibung As String
    Property datei As IO.FileInfo
    Property zusatzdateien As New List(Of String)
    Property begleitdateien As New List(Of IO.FileInfo)
End Class
