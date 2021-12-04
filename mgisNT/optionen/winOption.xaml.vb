Imports System.Data
Imports mgis

Public Class winOption
    Dim aufzweitembildschirmstarten, hauptbildschirmStehtLinks As Boolean

    Sub New()
        InitializeComponent()
    End Sub
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
        tbStatus.Text = statustext
        Dim lokExplorerAlphabetisch As Boolean = True
        If exploreralphabetisch Then
            cbExploreralphabetisch.IsChecked = True
        Else
            cbExploreralphabetisch.IsChecked = False
        End If
        clsStartup.einlesenZweiterBildschirm(aufzweitembildschirmstarten, hauptbildschirmStehtLinks)
        cbImmerAufZweitemScreen.IsChecked = aufzweitembildschirmstarten
        cbhauptbildschirmStehtLinks.IsChecked = hauptbildschirmStehtLinks

        clsOptionTools.einlesenParadigmaDominiert(ParadigmaDominiertzuletztFavoriten)
        'ParadigmaDominiertzuletztFavoriten = False
        If ParadigmaDominiertzuletztFavoriten Then
            radParadigmaDominiertzuletztFavoriten.IsChecked = True
        Else
            radParadigmaDominiertzuletztFavoriten.IsChecked = False
        End If
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
            myglobalz.NoImageMap = True
        Else
            myglobalz.NoImageMap = False
        End If
    End Sub

    Private Sub btnLogfileMailen_Click(sender As Object, e As RoutedEventArgs)
        l("fehler btnLogfileMailen_Click")
        e.Handled = True
    End Sub

End Class
