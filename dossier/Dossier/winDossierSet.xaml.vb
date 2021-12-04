
Public Class winDossierSet
    Dim ladevorgangabgeschlossen As Boolean = False
    Sub New()
        InitializeComponent()
    End Sub

    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Close()
    End Sub
    Private Sub cmbProfile_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        MsgBox("Baustelle")
    End Sub
    Private Sub winDossierSet_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        InitControls()
        ladevorgangabgeschlossen = True
    End Sub

    Private Sub InitControls()
        If clsDossier.Altstadtsatzung.showControl Then
            cbAltstadtsatzung.IsChecked = True
        Else
            cbAltstadtsatzung.IsChecked = False
        End If
        If clsDossier.Schwalben.showControl Then
            cbschwalben.IsChecked = True
        Else
            cbschwalben.IsChecked = False
        End If
        If clsDossier.UEBKROF.showControl Then
            cbUebKROFANA.IsChecked = True
        Else
            cbUebKROFANA.IsChecked = False
        End If
        If clsDossier.Altlast.showControl Then
            cbaltlastANA.IsChecked = True
        Else
            cbaltlastANA.IsChecked = False
        End If
        If clsDossier.Baulasten.showControl Then
            cbBaulastANA.IsChecked = True
        Else
            cbBaulastANA.IsChecked = False
        End If
        If clsDossier.Baulasten.showControl Then
            cbBaulastANA.IsChecked = True
        Else
            cbBaulastANA.IsChecked = False
        End If
        If clsDossier.Boris.showControl Then
            cbBorisANA.IsChecked = True
        Else
            cbBorisANA.IsChecked = False
        End If
        If clsDossier.Bplan.showControl Then
            cbbplanANA.IsChecked = True
        Else
            cbbplanANA.IsChecked = False
        End If
        If clsDossier.Eigentuemer.showControl Then
            cbEigentuemerANA.IsChecked = True
        Else
            cbEigentuemerANA.IsChecked = False
        End If
        If clsDossier.FFH.showControl Then
            cbFFHANA.IsChecked = True
        Else
            cbFFHANA.IsChecked = False
        End If
        If clsDossier.Foerder.showControl Then
            cbFoerderANA.IsChecked = True
        Else
            cbFoerderANA.IsChecked = False
        End If
        If clsDossier.Illegale.showControl Then
            cbIlleNeuANA.IsChecked = True
        Else
            cbIlleNeuANA.IsChecked = False
        End If
        If clsDossier.IllegaleAlt.showControl Then
            cbIlleAltNA.IsChecked = True
        Else
            cbIlleAltNA.IsChecked = False
        End If
        If clsDossier.Kehr.showControl Then
            cbKehrbezAna.IsChecked = True
        Else
            cbKehrbezAna.IsChecked = False
        End If

        If clsDossier.LSG.showControl Then
            cbbLSGANA.IsChecked = True
        Else
            cbbLSGANA.IsChecked = False
        End If
        If clsDossier.NSG.showControl Then
            cbNSGANA.IsChecked = True
        Else
            cbNSGANA.IsChecked = False
        End If
        If clsDossier.Ueb.showControl Then
            cbUebANA.IsChecked = True
        Else
            cbUebANA.IsChecked = False
        End If

        If clsDossier.WSG.showControl Then
            cbWSGANA.IsChecked = True
        Else
            cbWSGANA.IsChecked = False
        End If

        If clsDossier.WSG.showControl Then
            cbWSGANA.IsChecked = True
        Else
            cbWSGANA.IsChecked = False
        End If
        If clsDossier.WSG.showControl Then
            cbWSGANA.IsChecked = True
        Else
            cbWSGANA.IsChecked = False
        End If
        If clsDossier.Hbiotope.showControl Then
            cbHbiotop.IsChecked = True
        Else
            cbHbiotop.IsChecked = False
        End If


        If clsDossier.Hkomplexe.showControl Then
            cbHkomplexe.IsChecked = True
        Else
            cbHkomplexe.IsChecked = False
        End If
        If clsDossier.Amphibien.showControl Then
            cbAmphibien.IsChecked = True
        Else
            cbAmphibien.IsChecked = False
        End If
        If clsDossier.BSE.showControl Then
            cbBSE.IsChecked = True
        Else
            cbBSE.IsChecked = False
        End If
        If clsDossier.OEKOKO.showControl Then
            cbOEKOKO.IsChecked = True
        Else
            cbOEKOKO.IsChecked = False
        End If
        If clsDossier.kompensation.showControl Then
            cbKompensation.IsChecked = True
        Else
            cbKompensation.IsChecked = False
        End If
    End Sub

    Private Sub cbUebKROFANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbUebKROFANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "UEBKROF", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "UEBKROF", "0")
        End If
    End Sub

    Private Sub cbbplanANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbbplanANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "BPLAN", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "BPLAN", "0")
        End If
    End Sub

    Private Sub cbNSGANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbNSGANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "nsg", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "nsg", "0")
        End If
    End Sub

    Private Sub cbbLSGANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbbLSGANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "lsg", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "lsg", "0")
        End If
    End Sub

    Private Sub cbNDANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbNDANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "naturdenkmal", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "naturdenkmal", "0")
        End If
    End Sub

    Private Sub cbFoerderANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbFoerderANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "foerder", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "foerder", "0")
        End If
    End Sub

    Private Sub cbEigentuemerANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbEigentuemerANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "eigentuemer", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "eigentuemer", "0")
        End If
    End Sub

    Private Sub cbFFHANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbFFHANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "ffh", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "ffh", "0")
        End If
    End Sub

    Private Sub cbIlleNeuANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbIlleNeuANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "illegale", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "illegale", "0")
        End If
    End Sub

    Private Sub cbIlleAltNA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbIlleAltNA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "illegalealt", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "illegalealt", "0")
        End If
    End Sub

    Private Sub cbWSGANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbWSGANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "wsg", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "wsg", "0")
        End If
    End Sub

    Private Sub cbKehrbezAna_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbKehrbezAna.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "kehrbezirk", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "kehrbezirk", "0")
        End If
    End Sub

    Private Sub cbUebANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbUebANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "UEB", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "UEB", "0")
        End If
    End Sub

    Private Sub cbaltlastANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbaltlastANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "altlast", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "altlast", "0")
        End If
    End Sub

    Private Sub cbBaulastANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbBaulastANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "baulasten", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "baulasten", "0")
        End If
    End Sub

    Private Sub cbBorisANA_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbBorisANA.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "boris", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "boris", "0")
        End If
    End Sub

    Private Sub cbHbiotop_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbHbiotop.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "hbiotope", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "hbiotope", "0")
        End If
    End Sub

    Private Sub cbHkomplexe_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbHkomplexe.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "hkomplexe", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "hkomplexe", "0")
        End If
    End Sub

    Private Sub cbAmphibien_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbAmphibien.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "amph", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "amph", "0")
        End If
    End Sub

    Private Sub cbBSE_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbBSE.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "bse", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "bse", "0")
        End If
    End Sub

    Private Sub cbOEKOKO_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbOEKOKO.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "oekoko", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "oekoko", "0")
        End If
    End Sub

    'Private Sub cbND_Click(sender As Object, e As RoutedEventArgs)
    '    e.Handled = True
    '    If cbND.IsChecked = True Then
    '        clsStartup.userIniProfile.WertSchreiben("gisanalyse", "nd", "1")
    '    Else
    '        clsStartup.userIniProfile.WertSchreiben("gisanalyse", "nd", "0")
    '    End If
    'End Sub

    Private Sub cbaltis16_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbaltis16.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "altis16", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "altis16", "0")
        End If
    End Sub

    Private Sub cbKompensation_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbKompensation.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "kompensation", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "kompensation", "0")
        End If
    End Sub

    Private Sub cbschwalben_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbschwalben.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "schwalben", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "schwalben", "0")
        End If
    End Sub

    Private Sub cbAltstadtsatzung_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbAltstadtsatzung.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "altstadtsatzung", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "altstadtsatzung", "0")
        End If
    End Sub

    Private Sub cbstandorttypisierung_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbstandorttypisierung.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "standorttypisierung", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "standorttypisierung", "0")
        End If
    End Sub

    Private Sub cbwsgHNUGwms_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If cbwsgHNUGwms.IsChecked = True Then
            m.userIniProfile.WertSchreiben("gisanalyse", "wsgHNUGwms", "1")
        Else
            m.userIniProfile.WertSchreiben("gisanalyse", "wsgHNUGwms", "0")
        End If
    End Sub
End Class
