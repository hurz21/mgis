Imports System.IO

Public Class winAlleLayer
    Private datei As String = "checkallLayers.txt"
    Property mapfileBILD As String
    Sub New(_mapfileBILD As String)
        InitializeComponent()
        mapfileBILD = _mapfileBILD
    End Sub
    Private Sub winAlleLayer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    End Sub

    Private Sub Start_Click(sender As Object, e As RoutedEventArgs)
        Dim rangetext As String
        If rbGross.IsChecked Then rangetext = "gross"
        If rbmittel.IsChecked Then rangetext = "mittel"
        If rbklein.IsChecked Then rangetext = "klein"
        aktrange = tools.getRange(rangetext)

        tberrorcnt.Text = checkAllLayers(aktrange, datei, stamm_tabelle) & " Fehler gefunden"
        e.Handled = True
    End Sub

    Private Function checkAllLayers(aktrange As clsRange, datei As String, tabelle As String) As String
        Dim serror As String = ""
        Dim aktaid As Integer

        Dim test As String
        Dim w As Double = 600
        Dim h As Double = 600
        Dim j As Integer = 0
        Dim errorcnt As Integer = 0
        Dim sw As New IO.StreamWriter(datei)
        Try
            wgisdt = getDT("SELECt * FROM  " & tabelle & " where status=true ", tools.dbServername, "webgiscontrol")
            For i = 0 To wgisdt.Rows.Count - 1
                j += 1
                aktaid = CInt(clsDBtools.fieldvalue(wgisdt.Rows(i).Item("aid")))
                If aktaid = 30 Then
                    Debug.Print(aktaid.ToString)
                End If
                test = mapfileErzeugen(aktaid, mapfileBILD)
                Console.WriteLine(wgisdt.Rows(i).Item("aid"))
                tbinfo.Text = j & "(" & wgisdt.Rows.Count & ")" & "," & clsDBtools.fieldvalue(wgisdt.Rows(i).Item("aid")) & ": " & clsDBtools.fieldvalue(wgisdt.Rows(i).Item("titel"))
                Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
                Dim tbaufruf As String = aufrufbilden(aktrange, mapfileBILD, w, h)
                Dim hinweis, fehler As String
                fehler = meineHttpNet.meinHttpJob("", tbaufruf, hinweis)
                If fehler.Substring(0, 100).ToLower.Contains("png") Then
                    ' sw.WriteLine(aktaid & clsDBtools.fieldvalue(wgisdt.Rows(i).Item("titel")) & ": ok")
                Else
                    sw.WriteLine(aktaid & ": " & clsDBtools.fieldvalue(wgisdt.Rows(i).Item("titel")) & fehler)
                    sw.WriteLine("__________________________________")
                    errorcnt += 1
                End If
                '   sw.WriteLine("__________________________________")
            Next
            sw.Close()
            sw.Dispose()

            opendocument(datei)
            Return CType(errorcnt, String)
        Catch ex As Exception
            l("fehler ind checkAllLegende " & ex.ToString)
            Return "fehler"
        End Try
    End Function

    Private Sub btnDateiaufrufen_Click(sender As Object, e As RoutedEventArgs)
        Try

            opendocument(datei)
        Catch ex As Exception

        End Try
        e.Handled = True
    End Sub

    Private Sub cbmserrorExt_Click(sender As Object, e As RoutedEventArgs)
        Dim datei = tools.serverUNC & "\ms_error.txt"
        Dim readText As String
        Try
            readText = IO.File.ReadAllText(datei)
            opendocument(datei)
        Catch ex As Exception

        End Try


        e.Handled = True
    End Sub
End Class
