Imports System.Xml
Public Class clsINIXML
    Public iniDict As Dictionary(Of String, String)
    Public Shared Function XMLiniReader(ByVal xml_inifile_fullpath$) As Dictionary(Of String, String)
        'beisüpielaufruf
        '1. Dim iniDict As Dictionary(Of String, String) = clsXML_ini.XMLiniReader(meininifile$)
        '2. Dim iminternet As Boolean = CType(iniDict("ServerSpezifisch.iminternet"), Boolean)
        'Dim entry As KeyValuePair(Of String, String)
        'Console.WriteLine("==============================")
        'For Each entry In iniDict
        '  Console.WriteLine(entry.Key & "=" & entry.Value)
        'Next
        glob2.nachricht("1in XmlTextReader------------------------")
        Dim iniDict As New Dictionary(Of String, String)
        '		Dim _xlsminifile$
        Dim gruppe$
        ' Wir benötigen einen XmlReader für das Auslesen der XML-Datei 
        glob2.nachricht("333 Vor XmlTextReader" & xml_inifile_fullpath$)
        Try
            Using XMLReader As XmlReader = New XmlTextReader(xml_inifile_fullpath$)
                glob2.nachricht("333 nach XmlTextReader" & xml_inifile_fullpath$)
                ' Es folgt das Auslesen der XML-Datei 
                With XMLReader
                    Do While .Read ' Es sind noch Daten vorhanden 
                        ' Welche Art von Daten liegt an? 
                        Select Case .NodeType
                            ' Ein Element 
                            Case XmlNodeType.Element
                                gruppe = .Name
                                ' Alle Attribute (Name-Wert-Paare) abarbeiten 
                                If .AttributeCount > 0 Then
                                    ' Es sind noch weitere Attribute vorhanden 
                                    While .MoveToNextAttribute ' nächstes 
                                        iniDict.Add(String.Format("{0}.{1}", gruppe, .Name), .Value)
                                    End While
                                End If
                                ' Ein Text 
                            Case XmlNodeType.Text
                                ' Console.WriteLine("Es folgt ein Text: " & .Value)
                                ' Ein Kommentar 
                            Case XmlNodeType.Comment
                                'Console.WriteLine("Es folgt ein Kommentar: " & .Value)
                        End Select
                    Loop
                    ' Weiter nach Daten schauen 
                    .Close()
                    ' XMLTextReader schließen 
                End With
            End Using
            glob2.nachricht("vor dem return in xmlinireadera")
            Return iniDict
        Catch ex As Exception
          '  glob2.nachricht_und_Mbox("in xmlinireader:  " & ex.ToString)
            Return Nothing
        End Try
    End Function
     
    Public Sub New(ByVal xlsminifile$)
        glob2.nachricht("2 in XmlTextReader-----------" & xlsminifile$)
        iniDict = XMLiniReader(xlsminifile$)
    End Sub
End Class
