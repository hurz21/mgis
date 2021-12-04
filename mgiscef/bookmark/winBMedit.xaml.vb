Public Class winBMedit
    Public Property sofortaktivieren As Boolean = False
    Property _modus As String
    Public Property ladevorgangabgeschlossen As Boolean = False

    Sub New(modus As String)
        InitializeComponent()
        _modus = modus
    End Sub
    Private Sub winBMedit_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If _modus = "neu" Then
            btnBMloeschen.IsEnabled = False
            If STARTUP_mgismodus = "paradigma" Then
                tbBMneu.Text = "Vorgang: " & aktvorgangsid
            End If
            Title = "Lesezeichen neu anlegen"
        End If
        If _modus = "edit" Then
            Title = "Lesezeichen ändern / löschen. ID: " & auswahlBookmark.id
            tbBMneu.Text = auswahlBookmark.titel
            If auswahlBookmark.free4mygruppe Then
                cbFreigabefuerGruppe.IsChecked = True
            Else
                cbFreigabefuerGruppe.IsChecked = False
            End If
            btnBMloeschen.IsEnabled = True
        End If
        ladevorgangabgeschlossen = True
        e.Handled = True
    End Sub
    Private Sub btnBMeditAbbruch_Click(sender As Object, e As RoutedEventArgs)
        Close()
        e.Handled = True
    End Sub

    Private Sub btnBMloeschen_Click(sender As Object, e As RoutedEventArgs)
        Dim mesres As New MessageBoxResult
        mesres = MessageBox.Show("Wirklich löschen ? ", "Löschen ?", MessageBoxButton.YesNo, MessageBoxImage.Question)
        If mesres = MessageBoxResult.Yes Then
            If bmTools.btnBMloeschen(auswahlBookmark) Then


                MsgBox("Löschen erfolgreich")
            Else

                MsgBox("Problem beim löschen")
            End If
        End If
        Close()
        e.Handled = True
    End Sub

    Private Sub btnBookmarkSave_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim newbookmark As New clsBookmark
        Dim newid As Long
        If _modus = "neu" Then
            newbookmark = bmTools.bildebookMarkObj(tbBMneu.Text, CBool(cbFreigabefuerGruppe.IsChecked))
            Dim erfolg As Boolean
            erfolg = bmTools.BMSaveInsert(newbookmark, newid)
            If erfolg Then
                MessageBox.Show("Bookmark >" & newbookmark.titel & "< wurde mit der id: " & newid & " in den Bestand aufgenommen!")
            Else
                MessageBox.Show("Es gab ein Problem, Version " & mgisVersion)
            End If
        End If
        If _modus = "edit" Then
            auswahlBookmark.titel = tbBMneu.Text
            auswahlBookmark.free4mygruppe = CBool(cbFreigabefuerGruppe.IsChecked)
            Dim erfolg As Boolean
            erfolg = bmTools.BMsaveedit(auswahlBookmark) 'bmTools.SaveInsert(auswahlBookmark)
            If erfolg Then
                MessageBox.Show("Bookmark mit id>" & auswahlBookmark.id & "< wurde geändert!")
            Else
                MessageBox.Show("Es gab ein Problem")
            End If
        End If
        Close()
    End Sub

    Private Sub btnBMaktivieren_Click(sender As Object, e As RoutedEventArgs)
        sofortaktivieren = True
        e.Handled = True
        Close()
    End Sub

    Private Sub tbBMneu_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If tbBMneu.Text.Length > 3 Then
            btnBookmarkSave.IsEnabled = True
        End If
        e.Handled = True
    End Sub

    Private Sub cbFreigabefuerGruppe_Click(sender As Object, e As RoutedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnBookmarkSave.IsEnabled = True
        e.Handled = True
    End Sub
End Class
