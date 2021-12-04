Imports System.IO

Public Class winRTF
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub winRTF_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            Dim alles As String
            Using datei As IO.StreamReader = New IO.StreamReader("info.rtf")
                alles = datei.ReadToEnd
            End Using
            Dim documentBytes = Text.Encoding.UTF8.GetBytes(alles)
            Dim reader = New MemoryStream(documentBytes)
            reader.Position = 0
            richTextBox1.SelectAll()
            richTextBox1.Selection.Load(reader, DataFormats.Rtf)
        Catch ex As Exception
            l("fehler in winRTF_Loaded " & ex.ToString)
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
