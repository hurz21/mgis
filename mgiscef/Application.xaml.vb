Class Application

    ' Ereignisse auf Anwendungsebene wie Startup, Exit und DispatcherUnhandledException
    ' können in dieser Datei verarbeitet werden.

    Private Sub Application_DispatcherUnhandledException(ByVal sender As Object, ByVal e As System.Windows.Threading.DispatcherUnhandledExceptionEventArgs) Handles Me.DispatcherUnhandledException
        '  MsgBox(String.Format("Allgemeiner Fehler!  {0}{1}", vbCrLf, e))
        MessageBox.Show(e.Exception.ToString, "Allgemeiner Fehler", MessageBoxButton.OK, MessageBoxImage.Error)
        e.Handled = True
    End Sub
End Class
