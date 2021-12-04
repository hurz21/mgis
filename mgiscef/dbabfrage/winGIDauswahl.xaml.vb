Imports System.ComponentModel
Imports System.Data
Public Class winGIDauswahl
    Public Property auswahl As Integer = 0
    Public Property gesamtListeAllerObjekte As String = ""
    Dim ladevorgangabgeschlossen As Boolean = False
    Dim gids As New List(Of Integer)
    Dim resulttext As String()
    Dim aid As Integer
    Dim schematable, sql, colsql, colresult, titel As String
    Dim dt As DataTable
    Dim fangRadiusPX As Integer
    Dim fangRadiusM As Double
    Dim fensterZaehler As Integer = 0
    Sub New(_aid As Integer, _gids As List(Of Integer), _schematable As String, _titel As String,
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
        Dim buttonINfostringspecfunc As String = ""
        auswahl = CInt(btn.Tag)
        clsMiniMapTools.handlejavascript(clsMiniMapTools.genjavascriptMimikry(os_tabelledef.aid,
                                 os_tabelledef.tab_nr,
                                 CType(auswahl, String)), os_tabelledef, buttonINfostringspecfunc)
        'If os_tabelledef.gid = "0" Then
        '    os_tabelledef.gid = CType(auswahl, String)
        'End If
        'If buttonINfostringspecfunc Is Nothing Then buttonINfostringspecfunc = ""
        If gesamtSachdatList Is Nothing Then
        Else
            clsMiniMapTools.createRtfAndShowDialog(buttonINfostringspecfunc, layerActive.titel, fensterZaehler, False)
            fensterZaehler += 1 : If fensterZaehler = 5 Then fensterZaehler = 1
        End If
    End Sub
    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        auswahl = 0
        Close()
    End Sub

    Private Sub winGIDauswahl_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        Me.Top = clsToolsAllg.setPosition("diverse", "dbmultipleformpositiontop", Me.Top)
        Me.Left = clsToolsAllg.setPosition("diverse", "dbmultipleformpositionleft", Me.Left)
        ReDim resulttext(gids.Count)
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

        CollAuswahltreffer = bildeCollection(trenn, gesamtListeAllerObjekte)
        MainListBox.ItemsSource = CollAuswahltreffer
        MainListBox.Items.Refresh()
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

    Private Sub btnListe_Click(sender As Object, e As RoutedEventArgs)
        Dim ausgabedatei As String
        Try
            l(" MOD btnListe_Click anfang")
            Dim ausgabeDIR As String = strGlobals.localDocumentCacheRoot  '& "" & aid
            IO.Directory.CreateDirectory(ausgabeDIR)
            'If GisUser.ADgruppenname.ToLower = "umwelt" Then
            '    ausgabedatei = ausgabeDIR & "\" & Format(Now, "yyyy_hhmmss_ddMM") & ".txt"
            'Else
            ausgabedatei = ausgabeDIR & "\" & Format(Now, "yyyy_hhmmss_ddMM") & ".txt"
            'End If

            IO.File.WriteAllText(ausgabedatei, gesamtListeAllerObjekte, Text.UTF8Encoding.UTF8) '  myglobalz.enc)
            'IO.File.WriteAllText(ausgabedatei.Replace(".html", ".docx"), clsString.changeUmlaut2Html(strdok)) 
            l(" btnListe_Click ---------------------- ende")
            'OpenDokument(ausgabedatei)
            Process.Start("WORDPAD", ausgabedatei)
            'If GisUser.ADgruppenname.ToLower = "umwelt" Then
            '    'OpenDokumentWith("C:\kreisoffenbach\txtctrlNEU\ParadigmaTextControl.exe", ausgabedatei)
            'Else
            '    OpenDokument(ausgabedatei)
            'End If
        Catch ex As Exception
            l("Fehler in btnListe_Click: " & ex.ToString())

        End Try
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

    Private Function bildeCollection(trenn As String, ByRef gesamtListeAllerObjekte As String) As List(Of clsauswahlTreffer)
        Dim tempAuswahl As New clsauswahlTreffer
        Dim tempcoll As New List(Of clsauswahlTreffer)
        Dim result As String = "", hinweis As String = "", LastColNames() As String

        Try
            l(" bildeCollection ---------------------- anfang")
            For i = 0 To gids.Count - 1
                sql = "select * from " & schematable & " where gid=" & gids(i)
                If iminternet Or CGIstattDBzugriff Then
                    ModsachdatenTools.getColnames(schematable, LastColNames, hinweis)
                    result = clsToolsAllg.getSQL4Http(sql, "postgis20", hinweis, "getsql") : l(hinweis)
                    result = result.Trim
                    Debug.Print(CType(LastColNames.Count, String))
                    resulttext(i) = clsSachdatentools.buildResultTextArray(trenn, LastColNames, result)
                Else
                    dt = getDTFromWebgisDB(sql, "postgis20")
                    resulttext(i) = clsSachdatentools.buildResultTextDT(trenn, dt)
                End If
                If resulttext(i) = String.Empty Then
                    resulttext(i) = "Objekt interne Nummer " & gids(i) & " (Ersatz für " & schematable & ")"
                End If
                gesamtListeAllerObjekte += resulttext(i) & Environment.NewLine
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


    Private Sub winGIDauswahl_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        savePosition()
    End Sub
    Private Sub savePosition()
        Try
            userIniProfile.WertSchreiben("diverse", "dbmultipleformpositiontop", CType(Me.Top, String))
            userIniProfile.WertSchreiben("diverse", "dbmultipleformpositionleft", CType(Me.Left, String))
        Catch ex As Exception
            l("fehler in saveposition  windb", ex)
        End Try
    End Sub
End Class
