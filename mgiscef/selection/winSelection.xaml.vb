Imports System.ComponentModel

Public Class winSelection
    Public Property _aid As Integer = 485
    Public Property selectionvaluecgi As String = ""
    Public Property selectiontabelle As String = ""
    Public Property selectioncol As String = ""
    Public Property raumtyp As String = "punkt"
    Public Property selinfo As String = ""
    Public Property selectioncolCGI As String = ""
    Property ladevorgangabgeschlossen As Boolean = False
    Public Property vergleichsOperator As String = "gleich"
    Public Property sellayernumber As Integer = 0
    Public Property selitemsListe As List(Of clsUniversal)
    Public Property newuniveralList As New List(Of clsUniversal)
    Property _paren As MainWindow

    Sub New(aid As Integer, paren As MainWindow)
        InitializeComponent()
        _paren = paren
    End Sub


    Private Sub winSelection_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        splike.Visibility = Visibility.Collapsed
        Dim selaid As Integer = selectionTools.isSelectionLayerLoaded(GisUser.nick, layersSelected)
        If selaid > 0 Then
            tbinfo3.Text = "Die Auswahlebene '" & "Auswahl: " & GisUser.username & "' ist  geladen. "
        Else
            tbinfo3.Text = "Die Auswahlebene '" & "Auswahl: " & GisUser.username & "' ist nicht geladen. "
        End If
        Me.Top = clsToolsAllg.setPosition("diverse", "winselectiontop", Me.Top)
        Me.Left = clsToolsAllg.setPosition("diverse", "winwinselectionleft", Me.Left)
        Me.Width = clsToolsAllg.setPosition("diverse", "winwinselectionwidth", Me.Width)
        Me.Height = clsToolsAllg.setPosition("diverse", "winwinselectionheight", Me.Height)
        ladevorgangabgeschlossen = True
    End Sub
    Private Sub savePosition()
        Try
            myglobalz.userIniProfile.WertSchreiben("diverse", "winselectiontop", CType(Me.Top, String))
            myglobalz.userIniProfile.WertSchreiben("diverse", "winwinselectionleft", CType(Me.Left, String))
            myglobalz.userIniProfile.WertSchreiben("diverse", "winwinselectionwidth", CType(Me.Width, String))
            myglobalz.userIniProfile.WertSchreiben("diverse", "winwinselectionheight", CType(Me.Height, String))
        Catch ex As Exception
            l("fehler in saveposition  windb", ex)
        End Try
    End Sub
    Private Function initSelectionCombo(selectionTabelle As String, selectioncol As String) As Integer
        Dim hinweis As String = ""
        Dim result As String = ""
        Dim rec() As String


        l(" MOD initSelectionCombo anfang")
        Try
            rec = selectionTools.getCombo4Sql(hinweis, result, selectionTabelle, selectioncol)
            'selectionTools.populateCombobox(rec, cmbSelVal)
            newuniveralList = selectionTools.populateListBox(rec, lvSelection)
            lvSelection.ItemsSource = newuniveralList
            l(" MOD initSelectionCombo ende")
            Return rec.Count
        Catch ex As Exception
            l("Fehler in initSelectionCombo: " & ex.ToString())
            Return 0
        End Try
    End Function



    'Private Sub cmbSelVal_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    '    e.Handled = True
    '    If Not ladevorgangabgeschlossen Then Exit Sub
    '    Dim temp As New ComboBoxItem
    '    Try
    '        l(" MOD cmbSelVal_SelectionChanged anfang")
    '        If cmbSelVal.SelectedItem Is Nothing Then Exit Sub
    '        temp = CType(cmbSelVal.SelectedItem, ComboBoxItem)
    '        selectionvaluecgi = temp.Tag.ToString.Trim
    '        If cmbSelVal.Items.Count < 1 Then Exit Sub
    '        Dim selectionvalue = cmbSelVal.SelectedValue.ToString

    '        selectionvalue = selectionvalue.Replace("System.Windows.Controls.ComboBoxItem:", "")
    '        vergleichsOperator = "gleich"
    '        selectionTools.updateLayer(selectionvaluecgi, selectiontabelle, selectioncolCGI, _aid, selinfo, raumtyp, vergleichsOperator, tbLikeValue.Text.Trim)
    '        l(" MOD cmbSelVal_SelectionChanged ende")
    '    Catch ex As Exception
    '        l("Fehler in cmbSelVal_SelectionChanged: " & ex.ToString())
    '    End Try
    'End Sub
    Private Sub cmbSelectionLayers_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        Try
            l(" MOD cmbSelectionLayers_SelectionChanged anfang")
            'cmbSelVal.Items.Clear()
            Dim cbi As New ComboBoxItem
            cbi = CType(cmbSelectionLayers.SelectedItem, ComboBoxItem)
            sellayernumber = CInt(cbi.Tag.ToString) 'cmbSelectionLayers.SelectedValue.ToString 
            getLayerinfoFromCombo(sellayernumber)
            'cmbSelVal.ItemsSource = Nothing
            Dim anzahl = initSelectionCombo(selectiontabelle, selectioncol)
            tbinfo.Text = anzahl & " Treffer"
            'generateExplorer()

            'lvEbenenAlle.ItemsSource = Nothing
            'lvEbenenAlle.Items.Refresh()
            'lvEbenenAlle.ItemsSource = layersSelected
            'lvEbenenAlle.Items.Refresh()
            l(" MOD cmbSelectionLayers_SelectionChanged ende")
        Catch ex As Exception
            l("Fehler in cmbSelectionLayers_SelectionChanged: " & ex.ToString())
            Return
        End Try
    End Sub

    Sub getLayerinfoFromCombo(sellayernumber As Integer)

        If sellayernumber = 1 Then
            _aid = 485
            selectiontabelle = "arten_tiere.test1"
            selectioncol = "spectag,specdisplay"
            selectioncolCGI = "spectag"
            raumtyp = "punkt"
            selinfo = "[selinfo]"
        End If
        If sellayernumber = 20 Then
            _aid = 485
            selectiontabelle = "arten_tiere.test1"
            selectioncol = "a_art_grp,a_art_grp"
            selectioncolCGI = "a_art_grp"
            selinfo = "[selinfo]"
            raumtyp = "punkt"
        End If
        If sellayernumber = 2 Then
            _aid = 483 'vögel neu
            selectiontabelle = "arten_tiere.test2"
            selectioncol = "spectag,specdisplay"
            selectioncolCGI = "spectag"
            selinfo = "[selinfo]"
            raumtyp = "punkt"
        End If
        If sellayernumber = 3 Then
            _aid = 482 'vögel alt
            selectiontabelle = "arten_tiere.test3"
            selectioncol = "spectag,specdisplay"
            selectioncolCGI = "spectag"
            selinfo = "[selinfo]"
            raumtyp = "punkt"
        End If
        If sellayernumber = 4 Then
            _aid = 484 'libellen
            selectiontabelle = "arten_tiere.test4"
            selectioncol = "spectag,specdisplay"
            selectioncolCGI = "spectag"
            selinfo = "[selinfo]"
            raumtyp = "punkt"
        End If
        If sellayernumber = 5 Then
            _aid = 463 'pflanzen
            selectiontabelle = "arten_pflanzen.test1"
            selectioncol = "spectag,specdisplay"
            selectioncolCGI = "spectag"
            selinfo = "[selinfo]"
            raumtyp = "punkt"
        End If
        If sellayernumber = 30 Then
            _aid = 459 'hessbio
            selectiontabelle = "arten.test1" 'hlbk_biotope_f"
            selectioncol = "spectag,specdisplay"
            selectioncolCGI = "spectag"
            selinfo = "[selinfo]"
            raumtyp = "flaeche"
        End If
        If sellayernumber = 40 Then
            _aid = 185 'hauptkarte
            selectiontabelle = "regfnp.flaechen" 'hlbk_biotope_f"
            selectioncol = "spectag,selinfo"
            selectioncolCGI = "spectag"
            selinfo = "[selinfo]"
            raumtyp = "flaeche"
        End If
    End Sub

    Private Sub winSelection_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        savePosition()
    End Sub

    Private Sub cmbVergleich_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim cb As New ComboBoxItem
        cb = CType(cmbVergleich.SelectedItem, ComboBoxItem)
        vergleichsOperator = cb.Tag.ToString
        If vergleichsOperator = "gleich" Then
            spgleich.Visibility = Visibility.Visible
            lvSelection.Visibility = Visibility.Visible
            splike.Visibility = Visibility.Collapsed
            btnGleichStart.Visibility = Visibility.Visible
        End If
        If vergleichsOperator = "like" Then
            splike.Visibility = Visibility.Visible
            spgleich.Visibility = Visibility.Collapsed
            lvSelection.Visibility = Visibility.Collapsed
            btnGleichStart.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub btnLikeStart_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        '  getLayerinfoFromCombo(sellayernumber)
        Dim result = selectionTools.updateLayer(selectionvaluecgi, selectiontabelle, selectioncolCGI, _aid, selinfo, raumtyp, vergleichsOperator, tbLikeValue.Text.Trim, "gid,geom,selinfo")
        l("result " & result)
        _paren.refreshMap(True, True)
    End Sub

    'Private Sub generateExplorer()
    '    Try
    '        For Each item As clsUniversal In kategorienliste
    '            Dim tb As New TextBlock
    '            tb.Name = "tbE_" & item.tag.Trim
    '            tb.Text = item.titel 'clsString.Capitalize(kat.Replace("h_", "Hist. ")) 'kat.ToUpper
    '            tb.Tag = item.tag.ToLower.Trim
    '            tb.ToolTip = item.ToolTip
    '            tb.FontWeight = FontWeights.Bold
    '            'tb.MouseRightButtonDown += New MouseButtonEventHandler(cc_CopyToClip)
    '            AddHandler tb.MouseDown, AddressOf tbE_mousedown
    '            spExplorerParent.Children.Add(tb)
    '            'spExplorerParent.RegisterName(tb.Name, tb)
    '            '----------------------------
    '            Dim lv As New ListView
    '            lv.Name = "lvE_" & item.tag.Trim
    '            '   lv.Background = "{StaticResource flaechenBackground}"
    '            lv.Background = Brushes.Beige
    '            Dim pt As New Point
    '            pt.X = 0.5 : pt.Y = 0.5
    '            lv.RenderTransformOrigin = pt
    '            lv.Visibility = Visibility.Collapsed
    '            'lv.BorderBrush = Brushes.DarkGray 
    '            'Dim tn As New Thickness(top:=1, left:=1, right:=1, bottom:=1)
    '            'lv.BorderThickness = tn
    '            lv.FontSize = 12
    '            lv.FontFamily = New FontFamily("arial")
    '            'lv.ScrollViewer.HorizontalScrollBarVisibility = "Disabled"
    '            AddHandler lv.SelectionChanged, AddressOf lvEbenenAlle_SelectionChanged
    '            AddHandler lv.PreviewMouseWheel, AddressOf lvEXP_PreviewMouseWheel
    '            lv.ItemTemplate = CType(Me.FindResource("lvGesamtExplorerTemplate"), DataTemplate)
    '            spExplorerParent.Children.Add(lv)
    '            'spExplorerParent.RegisterName(lv.Name, lv)
    '        Next
    '    Catch ex As Exception
    '        l("fehler in generateExplorer: " ,ex)
    '    End Try
    'End Sub
    Private Sub lvEbenenAlle_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        Exit Sub
    End Sub
    Private Sub tbE_mousedown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        Dim tb As TextBlock = CType(sender, TextBlock)
        'letztekategorieAuswahl = tb.Tag.ToString.ToLower.Trim
        'Dim expOeffnen As Boolean = False
        'setWeightAndMode(tb, expOeffnen)
        'Dim aktlistview As ListView = getListview4Name("lvE_" & letztekategorieAuswahl.Trim)

        'expKategorieOeffnen(expOeffnen, aktlistview, letztekategorieAuswahl, tb.Tag.ToString)
    End Sub

    Private Sub chkauswahlgeaendertKategorie(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        'panningAusschalten()
        'resizeWindow()
        Dim nck As CheckBox = CType(sender, CheckBox)
        Dim action As String = If(nck.IsChecked, "add", "sub")
        Dim pickAid As Integer = CInt(CStr(nck.Tag))
        'addOrSubLayer(action, pickAid)

    End Sub

    Private Sub txtitel_MouseDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        Dim nck As TextBlock = CType(sender, TextBlock)
        'stContext.Visibility = Visibility.Collapsed
        'panningAusschalten()
        'MsgBox("aid text : " & CStr(nck.Tag))
        'Dim myfontstyle As New FontStyle
        'myfontstyle = CType(sender, FontStyle)
        aktaid = CInt(nck.Tag)
        aktsid = CInt(nck.Uid)
    End Sub

    Private Sub chkauswahlgeaendert(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnGleichStart_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim angehakte As String = ""
        angehakte = getAngehakte()
        Dim result = selectionTools.updateLayer(angehakte, selectiontabelle, selectioncolCGI, _aid, selinfo, raumtyp, vergleichsOperator, tbLikeValue.Text.Trim, "gid,geom,selinfo")
        l("result " & result)
        _paren.refreshMap(True, True)
    End Sub

    Private Function getAngehakte() As String
        Dim auswahl As String = ""

        Try
            l(" MOD getAngehakte anfang")
            For Each clsuniversal As clsUniversal In newuniveralList
                If clsuniversal.mithaken Then
                    auswahl = clsuniversal.tag & ";" & auswahl
                End If
            Next
            l(" MOD getAngehakte ende")
            Return clsString.removeLastChar(auswahl)
        Catch ex As Exception
            l("Fehler in getAngehakte: " & ex.ToString())
            Return auswahl
        End Try
    End Function
End Class
