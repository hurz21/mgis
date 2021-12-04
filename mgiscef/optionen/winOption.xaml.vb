Imports System.Data
Imports mgis

Public Class winOption
    Dim aufzweitembildschirmstarten, hauptbildschirmStehtLinks As Boolean
    Dim ladevorgangabgeschlossen As Boolean = False
    Sub New()
        InitializeComponent()
    End Sub

    Public Property kartenHintergrundGrau As Boolean = False

    Private Sub winOption_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim gisStartPolitik As String = mgis.clsGisstartPolitik.getgisstartOptionen()

        Select Case gisStartPolitik
            Case "multiple"
                radMultiple.IsChecked = True
            Case "neustart"
                radImmerNeustart.IsChecked = True
            Case "nachfrage"
                radNachfrage.IsChecked = True
            Case Else
                radMultiple.IsChecked = True
        End Select
        Dim statustext As String = clsOptionTools.bildeStatusText()
        If clsStartup.istGISAdmin() Then
            tbStatus.Text = statustext
            tiStatus.Visibility = Visibility.Visible
        Else
            tiStatus.Visibility = Visibility.Collapsed
        End If
        'If clsStartup.istGISAdmin() Then
        '    tiLogfile.Visibility = Visibility.Visible
        'Else
        '    tiLogfile.Visibility = Visibility.Collapsed
        'End If
        Dim lokExplorerAlphabetisch As Boolean = True
        If exploreralphabetisch Then
            cbExploreralphabetisch.IsChecked = True
        Else
            cbExploreralphabetisch.IsChecked = False
        End If
        clsStartup.einlesenZweiterBildschirm(aufzweitembildschirmstarten, hauptbildschirmStehtLinks)
        cbImmerAufZweitemScreen.IsChecked = aufzweitembildschirmstarten
        cbhauptbildschirmStehtLinks.IsChecked = hauptbildschirmStehtLinks

        setcbMaximierstarten()
        clsOptionTools.einlesenParadigmaDominiert(ParadigmaDominiertzuletztFavoriten)
        'ParadigmaDominiertzuletztFavoriten = False
        If ParadigmaDominiertzuletztFavoriten Then
            radParadigmaDominiertzuletztFavoriten.IsChecked = True
        Else
            radParadigmaDominiertzuletztFavoriten.IsChecked = False
        End If

        strGlobals.pdfReader = clsStartup.setPDFreader(strGlobals.pdfReader)
        tbPDFexe.Text = strGlobals.pdfReader


        strGlobals.paintProgramm = clsStartup.setPaintsoftware(strGlobals.paintProgramm)
        tbPaintexe.Text = strGlobals.paintProgramm
        If strGlobals.paintProgramm = "mspaint.exe" Then
            rbPaintexe.IsChecked = True
        Else
            rbPaint2exe.IsChecked = True
            tbPaintexe.IsEnabled = True
        End If


        If iminternet Then
            strGlobals.meinWordProcessor = myglobalz.userIniProfile.WertLesen("software", "wordexefullpath")
            'If strGlobals.meinWordProcessor.IsNothingOrEmpty Then
            'Else
            'End If
        Else
            'strGlobals.meinWordProcessor = "WINWORD.EXE"  ' sowieso
        End If
        tbWordexe.Text = strGlobals.meinWordProcessor


        myglobalz.minErrorMessages = clsOptionTools.getminErrorMessagesFromIni()
        cbLoggingEin.IsChecked = If(myglobalz.minErrorMessages, False, True)
        If iminternet Then cbLoggingEin.IsEnabled = False

        If iminternet Then
            gbEmail.IsEnabled = True
            clsSendmailTools.getEmailAccountFromIni(GisUser)
            tbMailkonto.Text = GisUser.EmailAdress
            tbMailPW.Text = GisUser.EmailPW
            tbMailserver.Text = GisUser.EmailServer
            tbProxy.Text = GisUser.proxy
            cbIChNutze.IsChecked = GisUser.ichNutzeDenGisserver
        Else
            gbEmail.IsEnabled = False
            tbMailkonto.Text = GisUser.EmailAdress
        End If
        If Not iminternet Then
            If clsOptionTools.Muss3DinternOeffnenOeffnen() Then
                cb3DinternOeffnen.IsChecked = True
            Else
                cb3DinternOeffnen.IsChecked = False
            End If
        End If
        If clsOptionTools.PDFimmerAcrobOeffnenat() Then
            cbPDFimmerAcrobat.IsChecked = True
        Else
            cbPDFimmerAcrobat.IsChecked = False

        End If
        ladevorgangabgeschlossen = True
    End Sub
    Private Sub setcbMaximierstarten()
        Try
            l(" MOD setcbMaximierstarten anfang")
            Dim topf As String = myglobalz.userIniProfile.WertLesen("gisstart", "maximiertstarten")
            If topf.ToLower.Trim = "true" Then
                cbMaximiertstarten.IsChecked = True
            Else
                cbMaximiertstarten.IsChecked = False
            End If
            l(" MOD setcbMaximierstarten ende")
        Catch ex As Exception
            l("Fehler in setcbMaximierstarten: " & ex.ToString())
        End Try
    End Sub

    Private Sub radMultiple_Click(sender As Object, e As RoutedEventArgs)
        userIniProfile.WertSchreiben("gisstart", "mehrfachinstanzen", "multiple")
        e.Handled = True
    End Sub
    Private Sub radImmerNeustart_Click(sender As Object, e As RoutedEventArgs)
        userIniProfile.WertSchreiben("gisstart", "mehrfachinstanzen", "neustart")
        e.Handled = True
    End Sub
    Private Sub radNachfrage_Click(sender As Object, e As RoutedEventArgs)
        userIniProfile.WertSchreiben("gisstart", "mehrfachinstanzen", "nachfrage")
        e.Handled = True
    End Sub

    Private Sub cbhauptbildschirmStehtLinks_Click(sender As Object, e As RoutedEventArgs)
        If cbhauptbildschirmStehtLinks.IsChecked Then
            userIniProfile.WertSchreiben("gisstart", "hauptbildschirmStehtLinks", "1")
        Else
            userIniProfile.WertSchreiben("gisstart", "hauptbildschirmStehtLinks", "0")
        End If
        e.Handled = True
    End Sub



    Private Sub cbImmerAufZweitemScreen_Click(sender As Object, e As RoutedEventArgs)
        If cbImmerAufZweitemScreen.IsChecked Then
            userIniProfile.WertSchreiben("gisstart", "ImmerAufZweitemScreen", "1")
        Else
            userIniProfile.WertSchreiben("gisstart", "ImmerAufZweitemScreen", "0")
        End If
        e.Handled = True
    End Sub

    Private Sub radParadigmaDominiertzuletztFavoriten_Click(sender As Object, e As RoutedEventArgs)
        If radParadigmaDominiertzuletztFavoriten.IsChecked Then
            ParadigmaDominiertzuletztFavoriten = True
            userIniProfile.WertSchreiben("gisstart", "paradigmaDominiertFavoriten", "true")
        Else
            ParadigmaDominiertzuletztFavoriten = False
            userIniProfile.WertSchreiben("gisstart", "paradigmaDominiertFavoriten", "false")
        End If
        e.Handled = True
    End Sub

    Private Sub cbExploreralphabetisch_Click(sender As Object, e As RoutedEventArgs)
        If cbExploreralphabetisch.IsChecked Then
            myglobalz.userIniProfile.WertSchreiben("Diverse", "exploreralphabetisch", "1")
            exploreralphabetisch = True
        Else
            myglobalz.userIniProfile.WertSchreiben("Diverse", "exploreralphabetisch", "0")
            exploreralphabetisch = False
        End If
        e.Handled = True
    End Sub

    Private Sub cblayerThumbnailsAnzeigen_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub cbNoImageMap_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbNoImageMap.IsChecked Then
            strGlobals.NoImageMap = True
        Else
            strGlobals.NoImageMap = False
        End If
    End Sub

    Private Sub cbMaximiertstarten_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If cbMaximiertstarten.IsChecked Then
            myglobalz.userIniProfile.WertSchreiben("gisstart", "maximiertstarten", CType("True", String))
        Else
            myglobalz.userIniProfile.WertSchreiben("gisstart", "maximiertstarten", CType("False", String))
        End If
    End Sub

    Private Sub BtnSaveAcrobarexepath_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnSaveAcrobarexepath.IsEnabled = False
        userIniProfile.WertSchreiben("software", "pdfreaderpfad", tbPDFexe.Text)
    End Sub



    Private Sub tbPDFexe_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbPDFexe.TextChanged
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnSaveAcrobarexepath.IsEnabled = True
    End Sub

    Private Sub rbPaintexe_Checked(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If rbPaintexe.IsChecked Then
            strGlobals.paintProgramm = "mspaint.exe"
            tbPaintexe.IsEnabled = False
            btnSavepaintFullpath.IsEnabled = False
            userIniProfile.WertSchreiben("software", "usepaint", "true")
        Else
            tbPaintexe.IsEnabled = True
            userIniProfile.WertSchreiben("software", "usepaint", "false")
            Dim test = myglobalz.userIniProfile.WertLesen("software", "paintexefullpath")
            If test.Trim.Length < 5 Then
            Else
                strGlobals.paintProgramm = test.Trim
            End If
        End If
    End Sub

    Private Sub tbPaintexe_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbPaintexe.TextChanged
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnSavepaintFullpath.IsEnabled = True
    End Sub

    Private Sub btnLogfileMailen_Click(sender As Object, e As RoutedEventArgs)
        l("fehler btnLogfileMailen_Click")
        e.Handled = True
    End Sub

    Private Sub CbProxy_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub CbMailserver_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True

    End Sub

    Private Sub btnMailkonto_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If tbMailkonto.Text.Contains("@") And tbMailkonto.Text.Length > 5 Then
            userIniProfile.WertSchreiben("email", "konto", tbMailkonto.Text.Trim)
            GisUser.EmailAdress = tbMailkonto.Text.Trim
        Else
            MsgBox("Bitte eine gültige Email-Adresse angeben.")
        End If
        btnMailkonto.IsEnabled = False
    End Sub

    Private Sub tbMailkonto_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbMailkonto.TextChanged
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnMailkonto.IsEnabled = True
    End Sub

    Private Sub btnMailPW_Click(sender As Object, e As RoutedEventArgs) Handles btnMailPW.Click
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If tbMailPW.Text.Length > 5 Then
            userIniProfile.WertSchreiben("email", "kontopw", tbMailPW.Text.Trim)
            GisUser.EmailPW = tbMailPW.Text.Trim
        Else
            MsgBox("Bitte eine gültiges PW  angeben.")
        End If
        btnMailPW.IsEnabled = False
    End Sub

    Private Sub tbMailPW_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbMailPW.TextChanged
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnMailPW.IsEnabled = True
    End Sub

    Private Sub btnMailserver_Click(sender As Object, e As RoutedEventArgs) Handles btnMailserver.Click
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If tbMailserver.Text.Length > 5 Then
            userIniProfile.WertSchreiben("email", "mailserver", tbMailserver.Text.Trim)
            GisUser.EmailServer = tbMailserver.Text.Trim
        Else
            MsgBox("Bitte eine gültigen Mailserver  angeben.")
        End If
        btnMailserver.IsEnabled = False
    End Sub

    Private Sub tbMailserver_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbMailserver.TextChanged
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnMailserver.IsEnabled = True
    End Sub

    Private Sub tbProxy_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbProxy.TextChanged
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnproxy.IsEnabled = True
    End Sub

    Private Sub btnproxy_Click(sender As Object, e As RoutedEventArgs) Handles btnproxy.Click
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If tbProxy.Text.Length > 5 Then
            userIniProfile.WertSchreiben("email", "proxy", tbProxy.Text.Trim)
            GisUser.proxy = tbProxy.Text.Trim
        Else
            MsgBox("Bitte eine gültigen Proxy  angeben.")
        End If
        btnproxy.IsEnabled = False
    End Sub

    Private Sub cbIChNutze_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If cbIChNutze.IsChecked Then
            userIniProfile.WertSchreiben("email", "ichNutzeDenGisserver", "true")
            GisUser.ichNutzeDenGisserver = True
        Else
            userIniProfile.WertSchreiben("email", "ichNutzeDenGisserver", "false")
            GisUser.ichNutzeDenGisserver = False
        End If
    End Sub

    Private Sub CbLoggingEin_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbLoggingEin.IsChecked Then
            myglobalz.minErrorMessages = False
            userIniProfile.WertSchreiben("LOGGING", "minErrorMessages", "false")
        Else
            myglobalz.minErrorMessages = True
            userIniProfile.WertSchreiben("LOGGING", "minErrorMessages", "true")
        End If
    End Sub

    Private Sub BtnOpenLogdir_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Process.Start(My.Log.DefaultFileLogWriter.CustomLocation)
    End Sub

    Private Sub btnMailserver_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnMailPW_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnproxy_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub BtnSaveWordExe_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        userIniProfile.WertSchreiben("software", "wordexefullpath", tbWordexe.Text)
        btnSaveWordExe.IsEnabled = False
    End Sub

    Private Sub TbWordexe_TextChanged(sender As Object, e As TextChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnSaveWordExe.IsEnabled = True
    End Sub
    Private Sub Cb3DinternOeffnen_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cb3DinternOeffnen.IsChecked Then
            myglobalz.userIniProfile.WertSchreiben("Diverse", "3DinternOeffnen", "1")
        Else
            myglobalz.userIniProfile.WertSchreiben("Diverse", "3DinternOeffnen", "0")
        End If
    End Sub

    Private Sub cbPDFimmerAcrobat_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbPDFimmerAcrobat.IsChecked Then
            myglobalz.userIniProfile.WertSchreiben("Diverse", "PDFimmerAcrobat", "1")
        Else
            myglobalz.userIniProfile.WertSchreiben("Diverse", "PDFimmerAcrobat", "0")
        End If
    End Sub

    Private Sub btnMapBackgroundGrey_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        kartenHintergrundGrau = True
        Close()
    End Sub

    Private Sub cbUseCache_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnSavepaintFullpath_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        userIniProfile.WertSchreiben("software", "paintexefullpath", tbPaintexe.Text)
        btnSavepaintFullpath.IsEnabled = False
    End Sub
End Class
