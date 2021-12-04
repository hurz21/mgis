Public Class winSendEmail
    Private ladevorgangabgeschlossen As Boolean = False
    Private dt As System.Data.DataTable
    Private liste As New List(Of String)
    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Close()
    End Sub
    Sub New()
        InitializeComponent()
    End Sub
    Private Sub winSendEmail_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        If iminternet Then spADsuche.Visibility = Visibility.Collapsed
        Dim trenner As String = Environment.NewLine '"<br>"
        tbBetreff.Text = "Wie besprochen die Karte  "
        tbbody.Text = "Sehr geehrte Damen und Herren," & trenner &
                          trenner &
                          trenner &
                          trenner &
                          trenner &
                        "Mit freundlichen Grüßen" & trenner
        tbReceiver.Text = getOldReceiver()
        clsSendmailTools.empfaengerListeZuletzt = clsSendmailTools.getAlteEmpfaengerlisteZuletzt()
        cmbEmailszuletzt.ItemsSource = clsSendmailTools.empfaengerListeZuletzt
        cmbEmailszuletzt.IsDropDownOpen = True
        cmbEmailszuletzt.Visibility = Visibility.Visible
        '
        If iminternet Then
            spParadigma.IsEnabled = False
        Else
            If STARTUP_mgismodus = "paradigma" Then
                clsSendmailTools.empfaengerListeParadigma = clsSendmailTools.GetEmpfaengerListeParadigma(aktvorgang.id)
                cmbEmailsParadigma.ItemsSource = clsSendmailTools.empfaengerListeParadigma
                cmbEmailsParadigma.IsDropDownOpen = True
                cmbEmailsParadigma.Visibility = Visibility.Visible
            Else
                spParadigma.IsEnabled = False
            End If
        End If
        ladevorgangabgeschlossen = True
    End Sub

    Private Function getOldReceiver() As String
        Dim retval As String = ""
        If Not String.IsNullOrEmpty(userIniProfile.WertLesen("email", "lastemailadress")) Then
            retval = userIniProfile.WertLesen("email", "lastemailadress")
            retval = retval.Replace(",", ", ")
            retval = retval.Replace(",  ", ", ")
        End If
        Return retval
    End Function

    Private Sub btnsend_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim empfaenger As String = tbReceiver.Text '"dr.j.feinen@kreis-offenbach.de"
        If empfaenger = String.Empty Then
            MsgBox("keine Empfänger") : Exit Sub
        End If
        'panningAusschalten()
        Dim ausrichtung As String
        ausrichtung = "quer"
        PDF_PrintRange.xl = kartengen.aktMap.aktrange.xl
        PDF_PrintRange.xh = kartengen.aktMap.aktrange.xh
        PDF_PrintRange.yl = kartengen.aktMap.aktrange.yl
        PDF_PrintRange.yh = kartengen.aktMap.aktrange.yh
        Dim hochaufloesend As Boolean = False
        Dim ausgabedatei As String = ""
        Dim localAnhangFile As String = ""
        If rbPNGmail.IsChecked Then
            makeandloadPDF("mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, "", "", True, hochaufloesend,
                True, False, ausgabedatei, True, localAnhangFile, layersSelected)
        Else
            'PDF
            makeandloadPDF("mitmasstab", PDF_PrintRange, PDF_druckMassStab, ausrichtung, "", "", False, hochaufloesend,
              True, False, ausgabedatei, True, localAnhangFile, layersSelected)
        End If

        '   opendirec(localfile)
        'Dim vorlage As String = "mailto:" & System.Web.HttpUtility.UrlEncode(empfaenger) & "?subject=Hello&body=Test&attachment=" & System.Web.HttpUtility.UrlEncode(Chr(34) & Chr(34) & localfile & Chr(34) & Chr(34))
        ''   vorlage = System.Web.HttpUtility.UrlEncode(vorlage)
        'Process.Start(vorlage)
        'versenden

        Dim body = tbbody.Text
        body = body.Replace(vbCrLf, "<br>")
        empfaenger = empfaenger.Replace(";", ",").Replace(vbCrLf, "").Trim
        Dim betreff = tbBetreff.Text
        Dim von = empfaenger
        If GisUser.ichNutzeDenGisserver Then
            betreff = von & " " & betreff
        End If
        Dim test As Boolean = modMail.mailen(iminternet, GisUser, empfaenger, localAnhangFile, body, von, betreff)

        If test Then
            MsgBox("Email wurde erfolgreich versendet!")
        Else
            MsgBox("Email wurde NICHT erfolgreich versendet!")
        End If

        userIniProfile.WertSchreiben("email", "lastemailadress", empfaenger)
        clsSendmailTools.saveEmpfaengerHistory(empfaenger)
        Close()
    End Sub



    Private Sub btnsuchen_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        tbfilter.Text = clsString.umlaut2ue(tbfilter.Text)
        liste.Clear()
        cmbEmails.ItemsSource = Nothing
        dt = clsActiveDir.sucheperson("*" & tbfilter.Text.Trim & "*")
        'dt = clsActiveDir.sucheperson(tbfilter.Text)
        liste.Clear()

        For i = 0 To dt.Rows.Count - 1
            If Not clsDBtools.fieldvalue(dt.Rows(i).Item("mail")).Contains("@") Then Continue For
            If clsDBtools.fieldvalue(dt.Rows(i).Item("mail")) = String.Empty Then Continue For
            If clsDBtools.fieldvalue(dt.Rows(i).Item("mail")).StartsWith("$") Then Continue For
            liste.Add(clsDBtools.fieldvalue(dt.Rows(i).Item("mail")))
            liste.Sort()
        Next
        cmbEmails.ItemsSource = liste
        cmbEmails.IsDropDownOpen = True
    End Sub

    Private Sub cbEmails_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If cmbEmails.SelectedValue Is Nothing Then Exit Sub
        Dim auswahl As String
        auswahl = cmbEmails.SelectedValue.ToString
        If tbReceiver.Text = String.Empty Then
            tbReceiver.Text = auswahl
        Else
            tbReceiver.Text = tbReceiver.Text & "," & Environment.NewLine & auswahl
        End If

    End Sub

    Private Sub tbfilter_TextChanged(sender As Object, e As TextChangedEventArgs)
        e.Handled = True

    End Sub

    Private Sub cmbEmailszuletzt_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If cmbEmailszuletzt.SelectedValue Is Nothing Then Exit Sub
        Dim auswahl As String
        auswahl = cmbEmailszuletzt.SelectedValue.ToString
        If tbReceiver.Text = String.Empty Then
            tbReceiver.Text = auswahl
        Else
            tbReceiver.Text = tbReceiver.Text & "," & Environment.NewLine & auswahl
        End If
    End Sub

    Private Sub btnClearEliste_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbReceiver.Text = ""
    End Sub

    Private Sub tbReceiver_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbReceiver.TextChanged
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        btnsend.IsEnabled = True
    End Sub

    Private Sub cmbEmailsParadigma_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If cmbEmailsParadigma.SelectedValue Is Nothing Then Exit Sub
        Dim auswahl As String
        auswahl = cmbEmailsParadigma.SelectedValue.ToString
        If tbReceiver.Text = String.Empty Then
            tbReceiver.Text = auswahl
        Else
            tbReceiver.Text = tbReceiver.Text & "," & Environment.NewLine & auswahl
        End If
    End Sub
End Class
