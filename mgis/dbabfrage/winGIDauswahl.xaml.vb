Imports System.Data
Public Class winGIDauswahl
    Public Property auswahl As Integer = 0
    Dim ladevorgangabgeschlossen As Boolean = False
    Dim gids As Integer()
    Dim resulttext As String()
    Dim aid As Integer
    Dim schematable, sql, titel As String
    Dim dt As DataTable
    Dim fangRadiusPX As Integer
    Dim fangRadiusM As Double
    Sub New(_aid As Integer, _gids As Integer(), _schematable As String, _titel As String,
                      _fangRadiusPX As Integer, _fangRadiusM As Double)
        InitializeComponent()
        gids = _gids
        aid = _aid
        schematable = _schematable
        titel = _titel
        fangRadiusPX = _fangRadiusPX
        fangRadiusM = _fangRadiusM
    End Sub
    Private Sub btnGoJavascript_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim btn As Button = CType(sender, Button)
        auswahl = CInt(btn.Tag)
        Close()
    End Sub
    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        auswahl = 0
        Close()
    End Sub

    Private Sub winGIDauswahl_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        Me.Top = clsToolsAllg.setPosition("diverse", "dbabfrageformpositiontop", Me.Top)
        Me.Left = clsToolsAllg.setPosition("diverse", "dbabfrageformpositionleft", Me.Left)
        ReDim resulttext(gids.Length)
        CollAuswahltreffer = New List(Of clsauswahlTreffer)
        CollAuswahltreffer.Clear()
        Dim summe As String = ""
        Dim trenn As String = Environment.NewLine
        If fangRadiusM = 1 Then
            tbanzahl.Text = "Fang-Ebene: " & titel & Environment.NewLine & "Sie haben " & gids.Count &
                 " Treffer "
        Else
            tbanzahl.Text = "Fang-Ebene: " & titel & Environment.NewLine & "Sie haben " & gids.Count &
                 " Treffer im Fangradius " & fangRadiusM & " [m] (entspr. " & fangRadiusPX & " Bildschirm-Pixel)."
        End If
        CollAuswahltreffer = bildeCollection(trenn)
        MainListBox.ItemsSource = CollAuswahltreffer
        ladevorgangabgeschlossen = True
    End Sub

    Private Sub cmbFangradiusPX_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim item As New ComboBoxItem
        item = CType(cmbFangradiusPX.SelectedItem, ComboBoxItem)
        setFangradiusPixel(CInt(item.Tag))
        MessageBox.Show("Die Änderung wird bei der nächsten Datenabfrage wirksam. 
                         Die Änderung gilt nur während dieser GIS-Sitzung", "Fangradius wurde geändert")
    End Sub

    Private Shared Sub setFangradiusPixel(item As Integer)
        Try
            l(" setFanradiusPixel ---------------------- anfang")
            myglobalz.fangradius_in_pixel = CInt(item)
            l(" setFanradiusPixel ---------------------- ende")
        Catch ex As Exception
            l("Fehler in setFanradiusPixel: " & ex.ToString())
        End Try
    End Sub

    Private Function bildeCollection(trenn As String) As List(Of clsauswahlTreffer)
        'Dim summe As String = ""
        Dim tempAuswahl As New clsauswahlTreffer
        Dim tempcoll As New List(Of clsauswahlTreffer)
        Try
            l(" bildeCollection ---------------------- anfang")
            For i = 0 To gids.Count - 1
                sql = "select * from " & schematable & " where gid=" & gids(i)
                dt = getDTFromWebgisDB(sql, "postgis20")
                'resulttext(i) = clsDBtools.fieldvalue(dt.Rows(0).Item(2)).Trim & trenn
                'resulttext(i) = resulttext(i) & clsDBtools.fieldvalue(dt.Rows(0).Item(3)).Trim & trenn
                resulttext(i) = buildResultText(trenn)
                If resulttext(i) = String.Empty Then
                    resulttext(i) = "Objekt interne Nummer " & gids(i) & " (Ersatz für " & schematable & ")"
                End If
                tempAuswahl = New clsauswahlTreffer
                tempAuswahl.resulttext = resulttext(i)
                tempAuswahl.gid = gids(i)
                tempcoll.Add(tempAuswahl)
            Next
            l(" bildeCollection ---------------------- ende")
            Return tempcoll
        Catch ex As Exception
            l("Fehler in bildeCollection: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Function buildResultText(trenn As String) As String
        Dim summe As String = ""
        Try
            l(" buildResultText ---------------------- anfang")
            For j = 9 To dt.Columns.Count - 1
                summe = summe & clsString.Capitalize(dt.Columns(j).Caption) & ": " &
                clsDBtools.fieldvalue(dt.Rows(0).Item(j)).Trim & trenn
            Next
            l(" buildResultText ---------------------- ende")
            Return summe
        Catch ex As Exception
            l("Fehler in buildResultText: " & ex.ToString())
            Return "(Objektsuche nicht definiert - bitte beim Admin melden)"
        End Try
    End Function
End Class
