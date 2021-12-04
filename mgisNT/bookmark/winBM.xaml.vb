Imports System.Data
Public Class winBM
    Property aktion As String
    Public Property lesezeicheneRitemousekeypressed As Boolean = False
    Public Property lesezeichene4GRUPPERitemousekeypressed As Boolean = False

    Sub New()
        InitializeComponent()
    End Sub
    Private Sub winBM_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        refreshPrivatliste()
        refresh4gruppe()
        refreshVongruppe()
        e.Handled = True
    End Sub

    Private Sub refreshVongruppe()
        'Dim sql = "select * from public.bookmarks where  lower(username)<>'" & GisUser.username.ToLower & "' " &
        '    " and   free4mygruppe=true " &
        '    " order by ts desc"
        Dim sql = "select * from public.bookmarks where   " &
            "     free4mygruppe=true " &
            " order by ts desc"
        l(sql)
        Dim dt As System.Data.DataTable
        dt = getDTFromWebgisDB(sql, "webgiscontrol")
        Debug.Print(dt.Rows.Count.ToString)
        tVONgruppe.Header = "Freigaben von meiner Gruppe " & dt.Rows.Count.ToString
        dgBMvongruppe.DataContext = dt
    End Sub

    Private Sub refresh4gruppe()
        Dim sql = "select * from public.bookmarks where lower(username)='" & GisUser.username.ToLower & "' " &
            " and free4mygruppe=true " &
            " order by ts desc"
        l(sql)
        Dim dt As System.Data.DataTable
        dt = getDTFromWebgisDB(sql, "webgiscontrol")
        Debug.Print(dt.Rows.Count.ToString)
        ti4Gruppe.Header = "Freigaben von mir für meine Gruppe " & dt.Rows.Count.ToString
        dgBM4gruppe.DataContext = dt
    End Sub
    Private Sub refreshPrivatliste()
        'Dim sql = "select * from public.bookmarks where lower(username)='" & GisUser.username.ToLower & "' " &
        '    " and free4mygruppe=false " &
        '    " order by ts desc"
        Dim sql = "select * from public.bookmarks where lower(username)='" & GisUser.username.ToLower & "' " &
            "   " &
            " order by ts desc"
        l(sql)
        Dim dt As System.Data.DataTable
        dt = getDTFromWebgisDB(sql, "webgiscontrol")
        Debug.Print(dt.Rows.Count.ToString)
        tibmprivat.Header = "Meine privaten Lesezeichen " & dt.Rows.Count.ToString
        dgBMliste.DataContext = dt
    End Sub

    Private Sub dgBMliste_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        'If Not ladevorgangAbgeschlossen Then Exit Sub
        If dgBMliste.SelectedItem Is Nothing Then Exit Sub
        Dim item2 As DataRowView
        Try

            e.Handled = True
            l("---------------------- anfang")
            item2 = CType(dgBMliste.SelectedItem, DataRowView)
            If item2 Is Nothing Then Exit Sub
            BMdatarowview2auswahlBookmark(item2)
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

    Private Shared Sub BMdatarowview2auswahlBookmark(item2 As DataRowView)
        Try
            l("BMdatarowview2auswahlBookmark---------------------- anfang")
            auswahlBookmark.clear()
            auswahlBookmark.id = CInt(clsDBtools.fieldvalue(item2.Row.ItemArray(0)))
            auswahlBookmark.user.username = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(1)))
            auswahlBookmark.titel = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(2)))
            auswahlBookmark.fav.vorhanden = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(3)))
            auswahlBookmark.fav.gecheckted = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(4)))
            auswahlBookmark.fav.hgrund = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(5)))
            auswahlBookmark.fav.aktiv = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(6)))
            auswahlBookmark.range.xl = CDbl(clsDBtools.fieldvalue(item2.Row.ItemArray(7)))
            auswahlBookmark.range.xh = CDbl(clsDBtools.fieldvalue(item2.Row.ItemArray(8)))
            auswahlBookmark.range.yl = CDbl(clsDBtools.fieldvalue(item2.Row.ItemArray(9)))
            auswahlBookmark.range.yh = CDbl(clsDBtools.fieldvalue(item2.Row.ItemArray(10)))
            auswahlBookmark.datum = CDate(clsDBtools.fieldvalue(item2.Row.ItemArray(11)))
            auswahlBookmark.user.ADgruppenname = CStr(clsDBtools.fieldvalue(item2.Row.ItemArray(12)))
            auswahlBookmark.free4mygruppe = CBool(clsDBtools.toBool(item2.Row.ItemArray(13)))

            l("BMdatarowview2auswahlBookmark---------------------- ende")
        Catch ex As Exception
            l("Fehler in BMdatarowview2auswahlBookmark: " & ex.ToString())
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
            l("---------------------- anfang")
            Dim item2 As DataRowView = CType(dgBM4gruppe.SelectedItem, DataRowView)
            If item2 Is Nothing Then Exit Sub
            BMdatarowview2auswahlBookmark(item2)
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
            Dim item2 As DataRowView = CType(dgBMvongruppe.SelectedItem, DataRowView)
            If item2 Is Nothing Then Exit Sub
            BMdatarowview2auswahlBookmark(item2)
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
