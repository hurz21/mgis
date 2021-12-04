Public Class winLeg
    Property _rtfdatei As String
    Property _flow As FlowDocument
    Property _modus As String
    Property _buttonINfostring As String = ""
    Property _isUserLayer As Boolean
    Property secfuncParms As String()
    Property Soll_refreshmap As Boolean = False
    Sub New(rtfdatei As String, Optional flow As FlowDocument = Nothing)
        ' This call is required by the designer.
        InitializeComponent()
        _rtfdatei = rtfdatei
        '_modus = modus 'datei oder text, dabei ist text die DB abfrage
        '_flow = flow
        '_buttonINfostring = buttonINfostring
        '_isUserLayer = isUserLayer
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub winLeg_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            l("winLeg_Loaded -------------------")
            Dim rtfTextDoku As String
            'If _modus = "datei" Then
            Title = "Legende-Anzeige (Aid: " & aktaid & ")"
            Dim fi As New IO.FileInfo(_rtfdatei)
            If fi.Exists Then
                rtfTextDoku = dateimodus()
            Else
                Close()
                If aktaid > 0 Then


                    MsgBox("Es existiert keine Legende zu diesem Thema!")
                End If
            End If
            e.Handled = True
        Catch ex As Exception
            l("fehler in winRTF_Loaded " & ex.ToString)
        End Try
    End Sub




    Private Function dateimodus() As String
        Dim rtfTextDoku As String
        Using datei As IO.StreamReader = New IO.StreamReader(_rtfdatei)
            rtfTextDoku = datei.ReadToEnd
        End Using
        Dim documentBytes = Text.Encoding.UTF8.GetBytes(rtfTextDoku)
        Dim reader = New System.IO.MemoryStream(documentBytes)
        reader.Position = 0
        freiLegende.SelectAll()
        freiLegende.Selection.Load(reader, DataFormats.Rtf)
        Return rtfTextDoku
    End Function

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Clipboard.Clear()
        Dim szz As String = ""
        Dim content As New TextRange(freiLegende.Document.ContentStart, freiLegende.Document.ContentEnd)
        If content.CanSave(DataFormats.Rtf) Then
            Using stream = New IO.MemoryStream
                content.Save(stream, DataFormats.Rtf, True)
                ' Dim sw As New IO.StreamWriter(tstream)
                szz = System.Text.Encoding.ASCII.GetString(stream.ToArray())
            End Using
        End If
        Clipboard.SetText(szz, TextDataFormat.Rtf)
        GC.Collect()
        MsgBox("Sie können den Text jetzt mit Strg-v  in ein Word-Dokument einfügen!",, "Zwischenablage")
        e.Handled = True
    End Sub

    Private Sub btnRTFdatei2Word_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        OpenDokument(_rtfdatei)
    End Sub

    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Close()
    End Sub
End Class
