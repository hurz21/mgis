Public Class winBM
    Property aktion As String
    Private ladevorgangabgeschlossen As Boolean = False
    Public Property lesezeicheneRitemousekeypressed As Boolean = False
    Public Property lesezeichene4GRUPPERitemousekeypressed As Boolean = False
    Private Property bmlistprivat As New List(Of clsBookmark)
    Sub New()
        InitializeComponent()
    End Sub
    Private Sub winBM_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        refreshPrivatliste()
        refresh4gruppe()
        refreshVongruppe()
        If iminternet Then
            ti4Gruppe.IsEnabled = False
            tVONgruppe.IsEnabled = False
        End If
        ladevorgangabgeschlossen = True
    End Sub

    Private Sub refreshVongruppe()
        Dim schema As String = If(iminternet, "externparadigma", "public")
        Dim sql = "select * from " & schema & ".bookmarks where " &
            "free4mygruppe=true order by ts desc"
        l(sql)
        'Dim dt As System.Data.DataTable
        'dt = getDTFromWebgisDB(sql, "webgiscontrol")
        'Debug.Print(dt.Rows.Count.ToString)
        Dim neulist As New List(Of clsBookmark)
        l(sql)
        neulist = getBMlisteObj(neulist, sql)
        If neulist Is Nothing Then
            tVONgruppe.Header = "Freigaben von meiner Gruppe "
            dgBMvongruppe.DataContext = Nothing
        Else
            tVONgruppe.Header = "Freigaben von meiner Gruppe " & neulist.Count.ToString
            dgBMvongruppe.DataContext = neulist
        End If
    End Sub

    Private Sub refresh4gruppe()
        Dim schema As String = If(iminternet, "externparadigma", "public")
        Dim sql = "select * from " & schema & ".bookmarks where lower(username)='" & GisUser.nick.ToLower & "' " &
            " and free4mygruppe=true " &
            " order by ts desc"
        l(sql)
        Dim neulist As New List(Of clsBookmark)
        l(sql)
        neulist = getBMlisteObj(neulist, sql)
        If neulist Is Nothing Then
            ti4Gruppe.Header = "Freigaben von mir für meine Gruppe 0"
            dgBM4gruppe.DataContext = Nothing
        Else
            ti4Gruppe.Header = "Freigaben von mir für meine Gruppe " & neulist.Count.ToString
            dgBM4gruppe.DataContext = neulist
        End If
        'ti4Gruppe.Header = "Freigaben von mir für meine Gruppe " & neulist.Count.ToString 
    End Sub
    Private Function refreshPrivatliste() As Boolean
        Dim neulist As New List(Of clsBookmark)
        Dim schema As String = If(iminternet, "externparadigma", "public")
        Dim sql = "select * from " & schema & ".bookmarks where lower(username)='" & GisUser.nick.ToLower & "' " &
            "order by ts desc"
        l(sql)
        neulist = getBMlisteObj(neulist, sql)
        If neulist Is Nothing Then
            tibmprivat.Header = "Meine privaten Lesezeichen 0"
            dgBMliste.DataContext = Nothing
        Else
            tibmprivat.Header = "Meine privaten Lesezeichen " & neulist.Count.ToString
            dgBMliste.DataContext = neulist
        End If
        Return True
    End Function

    Private Function getBMlisteObj(neulist As List(Of clsBookmark), sql As String) As List(Of clsBookmark)
        'If iminternet Or CGIstattDBzugriff Then
        Dim result As String
        Dim hinweis As String = ""
        result = clsToolsAllg.getSQL4Http(sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
        result = result.Trim
        If result.IsNothingOrEmpty Then
            neulist = Nothing
        Else
            neulist = bmTools.getNeulistBM_AJAX(result)
        End If
        'Else
        '    Dim dt As System.Data.DataTable
        '    dt = getDTFromWebgisDB(sql, "webgiscontrol")
        '    Debug.Print(dt.Rows.Count.ToString)

        '    dgBMliste.DataContext = dt
        'End If

        Return neulist
    End Function

    Private Sub dgBMliste_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dgBMliste.SelectedItem Is Nothing Then Exit Sub
        Dim item2 As clsBookmark
        Try
            l("dgBMliste_SelectionChanged---------------------- anfang")
            item2 = CType(dgBMliste.SelectedItem, clsBookmark)
            If item2 Is Nothing Then Exit Sub
            auswahlBookmark = item2.kopiereBookmark()
            If lesezeicheneRitemousekeypressed Then
                lesezeicheneRitemousekeypressed = False
                l("---------------------- ende")
                Dim bmedit As New winBMedit("edit")
                bmedit.ShowDialog()
                If bmedit.sofortaktivieren Then
                    aktion = "bmaktivieren"
                    Close()
                Else
                    refreshPrivatliste()
                    refresh4gruppe()
                End If
            Else
                aktion = "bmaktivieren"
                Close()
            End If
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try

    End Sub





    Private Sub btnBMaktivieren_Click(sender As Object, e As RoutedEventArgs)
        aktion = "bmaktivieren"
        e.Handled = True
        Close()
    End Sub

    Private Sub btnAbbruch_Click(sender As Object, e As RoutedEventArgs)
        aktion = "nichts"
        auswahlBookmark = Nothing
        e.Handled = True
        Close()
    End Sub

    Private Sub dgBM4gruppe_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If dgBM4gruppe.SelectedItem Is Nothing Then Exit Sub
        Try
            e.Handled = True
            l("dgBM4gruppe_SelectionChanged---------------------- anfang")
            Dim item2 As clsBookmark = CType(dgBM4gruppe.SelectedItem, clsBookmark)
            If item2 Is Nothing Then Exit Sub
            auswahlBookmark = item2.kopiereBookmark()
            l("---------------------- ende")

            If lesezeichene4GRUPPERitemousekeypressed Then
                lesezeichene4GRUPPERitemousekeypressed = False
                l("---------------------- ende")
                Dim bmedit As New winBMedit("edit")
                bmedit.ShowDialog()
                If bmedit.sofortaktivieren Then
                    aktion = "bmaktivieren"
                    Close()
                Else
                    refreshPrivatliste()
                    refresh4gruppe()
                End If
            Else
                aktion = "bmaktivieren"
                Close()
            End If

            'Dim bmedit As New winBMedit("edit")
            'bmedit.ShowDialog()
            'If bmedit.sofortaktivieren Then
            '    aktion = "bmaktivieren"
            '    Close()
            'Else
            '    refreshPrivatliste()
            'End If
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub

    Private Sub dgBMvongruppe_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If dgBMvongruppe.SelectedItem Is Nothing Then Exit Sub
        Try

            l("dgBMvongruppe_SelectionChanged---------------------- anfang")
            Dim item2 As clsBookmark = CType(dgBMvongruppe.SelectedItem, clsBookmark)
            If item2 Is Nothing Then Exit Sub
            auswahlBookmark = item2.kopiereBookmark()
            l("dgBMvongruppe_SelectionChanged---------------------- ende")
            'Dim bmedit As New winBMedit("edit")
            'bmedit.ShowDialog()
            'If bmedit.sofortaktivieren Then
            aktion = "bmaktivieren"
            Close()
            'Else
            '    refreshPrivatliste()
            'End If
        Catch ex As Exception
            l("Fehler in dgBMvongruppe_SelectionChanged: " & ex.ToString())
        End Try
    End Sub

    Private Sub dgBMliste_MouseRightButtonUp(sender As Object, e As MouseButtonEventArgs)
        Dim dep As DependencyObject = DirectCast(e.OriginalSource, DependencyObject)
        While (dep IsNot Nothing) AndAlso Not (TypeOf dep Is DataGridCell)
            dep = VisualTreeHelper.GetParent(dep)
        End While
        If dep Is Nothing Then
            Return
        End If
        If TypeOf dep Is DataGridCell Then
            Dim cell As DataGridCell = TryCast(dep, DataGridCell)
            cell.Focus()
            While (dep IsNot Nothing) AndAlso Not (TypeOf dep Is DataGridRow)
                dep = VisualTreeHelper.GetParent(dep)
            End While
            Dim row As DataGridRow = TryCast(dep, DataGridRow)
            lesezeicheneRitemousekeypressed = True
            dgBMliste.SelectedItem = row.DataContext
        End If
        e.Handled = True
    End Sub

    Private Sub dgBM4gruppe_MouseRightButtonUp(sender As Object, e As MouseButtonEventArgs)
        Dim dep As DependencyObject = DirectCast(e.OriginalSource, DependencyObject)
        While (dep IsNot Nothing) AndAlso Not (TypeOf dep Is DataGridCell)
            dep = VisualTreeHelper.GetParent(dep)
        End While
        If dep Is Nothing Then
            Return
        End If
        If TypeOf dep Is DataGridCell Then
            Dim cell As DataGridCell = TryCast(dep, DataGridCell)
            cell.Focus()
            While (dep IsNot Nothing) AndAlso Not (TypeOf dep Is DataGridRow)
                dep = VisualTreeHelper.GetParent(dep)
            End While
            Dim row As DataGridRow = TryCast(dep, DataGridRow)
            lesezeichene4GRUPPERitemousekeypressed = True
            dgBM4gruppe.SelectedItem = row.DataContext
        End If
        e.Handled = True
    End Sub

    Private Sub btnBMneuanlegen_Click(sender As Object, e As RoutedEventArgs)
        Dim bmedit As New winBMedit("neu")
        bmedit.ShowDialog()
        refreshPrivatliste()
        refresh4gruppe()
        refreshVongruppe()
        e.Handled = True
    End Sub
End Class
