Public Class winLeg
    Property _legdatei As String
    Property _dokdatei As String
    Property _flow As FlowDocument
    Property _modus As String
    Property _buttonINfostring As String = ""
    Property _isUserLayer As Boolean
    Property secfuncParms As String()
    Property _format As String = "rtf"
    Public ReadOnly Property _aid As Integer = 0
    Property Soll_refreshmap As Boolean = False
    Property selectiontabelle As String = ""
    'Property selectionTabelle As String = ""
    Sub New(legdatei As String, dokdatei As String, format As String, aid As Integer, Optional flow As FlowDocument = Nothing)
        ' This call is required by the designer.
        InitializeComponent()
        _legdatei = legdatei
        _dokdatei = dokdatei
        _format = format
        _aid = aid
        '_modus = modus 'datei oder text, dabei ist text die DB abfrage
        '_flow = flow
        '_buttonINfostring = buttonINfostring
        '_isUserLayer = isUserLayer
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub winLeg_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            e.Handled = True
            l("winLeg_Loaded -------------------")
            Title = "Legende-Anzeige (Aid: " & aktaid & ")"
            Dim rtfTextDoku As String
            Dim fi As IO.FileInfo
            If _dokdatei.IsNothingOrEmpty Then tidok.Visibility = Visibility.Collapsed

            If _format = "rtf" Then
                wbleg.Visibility = Visibility.Collapsed
                freiLegende.Visibility = Visibility.Visible
                If _legdatei.IsNothingOrEmpty Then
                    rtfTextDoku = "Keine Legende vorhanden"
                Else
                    fi = New IO.FileInfo(_legdatei)
                    If fi.Exists Then
                        rtfTextDoku = dateimodus()
                        'Else
                        '    Close()
                        '    If aktaid > 0 Then
                        '        MsgBox("Es existiert keine Legende zu diesem Thema!")
                        '    End If
                    Else
                        rtfTextDoku = "Keine Legende vorhanden"
                    End If
                End If

            End If
            If _format = "html" Then
                btnClipboard.Visibility = Visibility.Collapsed
                freiLegende.Visibility = Visibility.Collapsed
                wbleg.Visibility = Visibility.Visible
                wbdok.Visibility = Visibility.Visible
                If _legdatei.IsNothingOrEmpty Then
                    rtfTextDoku = "Keine Legende vorhanden"
                    tidok.IsSelected = True
                Else
                    fi = New IO.FileInfo(_legdatei)
                    If fi.Exists Then
                        wbleg.Navigate("file:///" & _legdatei)
                    End If
                End If

                fi = New IO.FileInfo(_dokdatei)
                If fi.Exists Then
                    wbdok.Navigate("file:///" & _dokdatei)
                End If
            End If

        Catch ex As Exception
            l("fehler in winRTF_Loaded ", ex)
        End Try
    End Sub






    Private Function dateimodus() As String
        Dim rtfTextDoku As String
        Using datei As IO.StreamReader = New IO.StreamReader(_legdatei)
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
        If _format = "rtf" Then
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
        End If

        MsgBox("Sie können den Text jetzt mit Strg-v  in ein Word-Dokument einfügen!",, "Zwischenablage")
        e.Handled = True
    End Sub

    Private Sub btnRTFdatei2Word_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim tdatei As String = ""
        'datei = _legdatei.Replace(".html", ".docx")
        tdatei = _legdatei

        If iminternet Then
            OpenDokument(tdatei)
        Else
            OpenWithArguments("WINWORD.EXE", tdatei)
        End If

    End Sub

    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Close()
    End Sub

    Private Sub btnDOKdatei2Word_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim tdatei As String = ""
        'datei = _legdatei.Replace(".html", ".docx")
        tdatei = _dokdatei
        If iminternet Then
            OpenDokument(tdatei)
        Else
            OpenWithArguments(strGlobals.meinWordProcessor, tdatei)
        End If
    End Sub


End Class
