Imports Xceed.Wpf.Toolkit

Class clsTooltipps
    Private Sub New()

    End Sub
    Shared Sub setTooltipSuchobjektPolygon(tb12 As Polygon, popuplevel As String)
        'Dim popuplevel As String = "alles" '"ohneBild" , "nichts"
        'https://stackoverflow.com/questions/1167021/how-code-binding-textimage-tooltip-in-wpf-c-sharp
        Dim t1, t2 As TextBlock
        t1 = New TextBlock()
        Try
            l("setTooltipSuchobjektPolygon---------------------- anfang")
            't1.Background = Brushes.AliceBlue
            t1.HorizontalAlignment = HorizontalAlignment.Center
            t1.VerticalAlignment = VerticalAlignment.Center
            t1.FontStyle = FontStyles.Normal
            t1.FontWeight = FontWeights.Bold
            t1.FontSize = 12
            t1.Text =
            "Wie kann man dieses Suchobjekt wieder loswerden?" & Environment.NewLine

            t2 = New TextBlock()
            t2.Background = Brushes.AliceBlue
            t2.HorizontalAlignment = HorizontalAlignment.Center
            t2.VerticalAlignment = VerticalAlignment.Center
            t2.Text = Environment.NewLine &
            " => Einfach das Häkchen bei 'Suchobjekt sichtbar' rausnehmen." & Environment.NewLine

            Dim td As System.Windows.Controls.Image = New System.Windows.Controls.Image
            Dim mi As BitmapImage = New BitmapImage()
            mi.BeginInit()
            mi.UriSource = New Uri(strGlobals.suchobjektPNG)
            mi.EndInit()
            td.Source = mi

            Dim td2 As System.Windows.Controls.Image = New System.Windows.Controls.Image
            Dim mi1 As BitmapImage = New BitmapImage()
            mi1.BeginInit()
            mi1.UriSource = New Uri(strGlobals.suchobjektPNG2)
            mi1.EndInit()
            td2.Source = mi1

            Dim toolTipPanel As StackPanel = New StackPanel()

            If popuplevel = "alles" Then toolTipPanel.Children.Add(t1)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(td)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(t2)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(td2)

            'toolTipPanel.Width = 530
            'toolTipPanel.Height = 392

            toolTipPanel.Width = 330
            toolTipPanel.Height = 390

            ToolTipService.SetToolTip(tb12, toolTipPanel)
            ToolTipService.SetShowDuration(tb12, 60000)
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub
    Shared Sub setTooltipExplorerButton(tb12 As Button, popuplevel As String)
        'Dim popuplevel As String = "alles" '"ohneBild" , "nichts"
        'https://stackoverflow.com/questions/1167021/how-code-binding-textimage-tooltip-in-wpf-c-sharp
        Dim t1 As TextBlock
        t1 = New TextBlock()
        Try
            l("setTooltipExplorerButton---------------------- anfang")
            't1.Background = Brushes.AliceBlue
            t1.HorizontalAlignment = HorizontalAlignment.Center
            t1.VerticalAlignment = VerticalAlignment.Center
            t1.FontStyle = FontStyles.Normal
            t1.FontWeight = FontWeights.Bold
            t1.FontSize = 12
            t1.Text =
            "Hier öffnen Sie den Themen-Explorer !" & Environment.NewLine &
                "  " & Environment.NewLine &
              "Nutzen sie Themenbaum oder Volltextsuche" & Environment.NewLine
            Dim td As System.Windows.Controls.Image = New System.Windows.Controls.Image
            Dim mi As BitmapImage = New BitmapImage()
            mi.BeginInit()
            mi.UriSource = New Uri(strGlobals.explorerPNG)
            mi.EndInit()
            td.Source = mi

            Dim toolTipPanel As StackPanel = New StackPanel()

            If popuplevel = "alles" Then toolTipPanel.Children.Add(t1)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(td)

            toolTipPanel.Width = 260
            toolTipPanel.Height = 200

            ToolTipService.SetToolTip(tb12, toolTipPanel)
            ToolTipService.SetShowDuration(tb12, 60000)
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub

    Friend Shared Sub setTooltipSuchobjImgPin(imgpin As Image, popuplevel As String)
        'Dim popuplevel As String = "alles" '"ohneBild" , "nichts"
        'https://stackoverflow.com/questions/1167021/how-code-binding-textimage-tooltip-in-wpf-c-sharp
        Dim t1, t2 As TextBlock
        t1 = New TextBlock()
        Try
            l("setTooltipSuchobjImgPin---------------------- anfang")
            't1.Background = Brushes.AliceBlue
            t1.HorizontalAlignment = HorizontalAlignment.Center
            t1.VerticalAlignment = VerticalAlignment.Center
            t1.FontStyle = FontStyles.Normal
            t1.FontWeight = FontWeights.Bold
            t1.FontSize = 12
            t1.Text =
            "Wie kann man dieses Suchobjekt wieder loswerden?" & Environment.NewLine

            t2 = New TextBlock()
            t2.Background = Brushes.AliceBlue
            t2.HorizontalAlignment = HorizontalAlignment.Center
            t2.VerticalAlignment = VerticalAlignment.Center
            t2.Text = Environment.NewLine &
            " => Einfach das Häkchen bei 'Suchobjekt sichtbar' rausnehmen." & Environment.NewLine

            Dim td As System.Windows.Controls.Image = New System.Windows.Controls.Image
            Dim mi As BitmapImage = New BitmapImage()
            mi.BeginInit()
            mi.UriSource = New Uri(strGlobals.suchobjektPNG)
            mi.EndInit()
            td.Source = mi

            Dim td2 As System.Windows.Controls.Image = New System.Windows.Controls.Image
            Dim mi1 As BitmapImage = New BitmapImage()
            mi1.BeginInit()
            mi1.UriSource = New Uri(strGlobals.suchobjektPNG2)
            mi1.EndInit()
            td2.Source = mi1

            Dim toolTipPanel As StackPanel = New StackPanel()

            If popuplevel = "alles" Then toolTipPanel.Children.Add(t1)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(td)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(t2)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(td2)

            'toolTipPanel.Width = 530
            'toolTipPanel.Height = 392

            toolTipPanel.Width = 330
            toolTipPanel.Height = 390

            ToolTipService.SetToolTip(imgpin, toolTipPanel)
            ToolTipService.SetShowDuration(imgpin, 60000)
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub

    Shared Sub setTooltipSuchobjLoeschenButton(tb12 As CheckBox, popuplevel As String)
        'Dim popuplevel As String = "alles" '"ohneBild" , "nichts"
        'https://stackoverflow.com/questions/1167021/how-code-binding-textimage-tooltip-in-wpf-c-sharp
        Dim t1 As TextBlock
        t1 = New TextBlock()
        Try
            l("setTooltipSuchobjLoeschenButton---------------------- anfang")
            't1.Background = Brushes.AliceBlue
            t1.HorizontalAlignment = HorizontalAlignment.Center
            t1.VerticalAlignment = VerticalAlignment.Center
            t1.FontStyle = FontStyles.Normal
            t1.FontWeight = FontWeights.Bold
            t1.FontSize = 12
            t1.Text =
            "Hier blenden Sie Suchobjekte aus/ein!" & Environment.NewLine &
                "  " & Environment.NewLine &
              "Es gibt drei verschiedene Sorten:" & Environment.NewLine &
              " - runde für Adressen " & Environment.NewLine &
              " - vieleckige für Flurstücke" & Environment.NewLine &
              " - gelbunterlegte für Objekte" & Environment.NewLine
            Dim td As System.Windows.Controls.Image = New System.Windows.Controls.Image
            Dim mi As BitmapImage = New BitmapImage()
            mi.BeginInit()
            mi.UriSource = New Uri(strGlobals.suchobjekt3sorten)
            mi.EndInit()
            td.Source = mi

            Dim toolTipPanel As StackPanel = New StackPanel()

            If popuplevel = "alles" Then toolTipPanel.Children.Add(t1)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(td)

            toolTipPanel.Width = 230
            toolTipPanel.Height = 300

            ToolTipService.SetToolTip(tb12, toolTipPanel)
            ToolTipService.SetShowDuration(tb12, 60000)
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub
    '
    Shared Sub setTooltipInfoLegende(tb12 As StackPanel, popuplevel As String) 'tbInfolegende
        'Dim popuplevel As String = "alles" '"ohneBild" , "nichts"
        'https://stackoverflow.com/questions/1167021/how-code-binding-textimage-tooltip-in-wpf-c-sharp
        Dim t1 As TextBlock
        t1 = New TextBlock()
        Try
            l("setTooltipInfoLegende---------------------- anfang")
            't1.Background = Brushes.AliceBlue
            t1.HorizontalAlignment = HorizontalAlignment.Center
            t1.VerticalAlignment = VerticalAlignment.Center
            t1.FontStyle = FontStyles.Normal
            t1.FontWeight = FontWeights.Bold
            t1.FontSize = 12
            t1.Text =
            "Dokumentation = Maus über Ebenenbeschreibung halten" & Environment.NewLine &
            "Legende       = Rechte Maustaste auf Ebenenbeschreibung  " & Environment.NewLine &
            "Objektsuche   = Linke Maustaste auf Ebenenbeschreibung" & Environment.NewLine
            Dim td As System.Windows.Controls.Image = New System.Windows.Controls.Image
            Dim mi As BitmapImage = New BitmapImage()
            mi.BeginInit()
            mi.UriSource = New Uri(strGlobals.explorerPNG)
            mi.EndInit()
            td.Source = mi

            Dim toolTipPanel As StackPanel = New StackPanel()
            Dim bildPanel As StackPanel = New StackPanel()
            bildPanel.Width = 200
            bildPanel.Height = 300

            If popuplevel = "alles" Then bildPanel.Children.Add(td)

            If popuplevel = "alles" Then toolTipPanel.Children.Add(t1)
            'If popuplevel = "alles" Then toolTipPanel.Children.Add(bildPanel)

            toolTipPanel.Width = 430
            toolTipPanel.Height = 100

            ToolTipService.SetToolTip(tb12, toolTipPanel)
            ToolTipService.SetShowDuration(tb12, 60000)
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub

    Friend Shared Sub setTooltipVogel(tb12 As Button, popuplevel As String)
        'Dim popuplevel As String = "alles" '"ohneBild" , "nichts"
        'https://stackoverflow.com/questions/1167021/how-code-binding-textimage-tooltip-in-wpf-c-sharp
        Dim t1 As TextBlock
        t1 = New TextBlock()
        Try
            l("setTooltipVogel---------------------- anfang")
            't1.Background = Brushes.AliceBlue
            t1.HorizontalAlignment = HorizontalAlignment.Center
            t1.VerticalAlignment = VerticalAlignment.Center
            t1.FontStyle = FontStyles.Normal
            t1.FontWeight = FontWeights.Bold
            t1.FontSize = 12
            t1.Text =
              "Wechsel zu Vogelperspektive !" & Environment.NewLine &
              "  " & Environment.NewLine &
              "Es handelt sich um Google-Daten aus vier Befliegungen." & Environment.NewLine &
              "Für jede Himmelsrichtung gibt es ein eigenes Luftbild. " & Environment.NewLine &
              "So kann man in Bereiche schauen, die sonst von Gebäuden verdeckt sind " & Environment.NewLine &
              "oder im Schatten liegen. " & Environment.NewLine &
              "Wechseln sie die Himmelsrichtung mit dem Symbol rechts unten:" & Environment.NewLine
            Dim td As System.Windows.Controls.Image = New System.Windows.Controls.Image
            Dim mi As BitmapImage = New BitmapImage()
            mi.BeginInit()
            mi.UriSource = New Uri(strGlobals.vogeldrehenpng)
            mi.EndInit()
            td.Source = mi

            Dim toolTipPanel As StackPanel = New StackPanel()
            Dim bildPanel As StackPanel = New StackPanel()
            bildPanel.Children.Add(td)

            bildPanel.Width = 230
            bildPanel.Height = 200

            If popuplevel = "alles" Then toolTipPanel.Children.Add(t1)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(bildPanel)

            toolTipPanel.Width = 530
            toolTipPanel.Height = 300

            ToolTipService.SetToolTip(tb12, toolTipPanel)
            ToolTipService.SetShowDuration(tb12, 60000)
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub

    Friend Shared Sub setTooltipAktvorgang(spVID As DropDownButton, popuplevel As String)
        Dim t1 As TextBlock
        t1 = New TextBlock()
        Try
            l("setTooltipAktvorgang---------------------- anfang")
            't1.Background = Brushes.AliceBlue
            t1.HorizontalAlignment = HorizontalAlignment.Center
            t1.VerticalAlignment = VerticalAlignment.Center
            t1.FontStyle = FontStyles.Normal
            t1.FontWeight = FontWeights.Bold
            t1.FontSize = 12
            t1.Text =
            "Aktueller Vorgang:" & Environment.NewLine &
            "  Az: " & aktvorgang.az & Environment.NewLine &
            "  Beschreibung: " & aktvorgang.beschreibung & Environment.NewLine &
            "-----------------------------" & Environment.NewLine &
            "Hier können Sie den Vorgang wechseln oder einen Neuen anlegen:" & Environment.NewLine
            Dim td As System.Windows.Controls.Image = New System.Windows.Controls.Image
            Dim mi As BitmapImage = New BitmapImage()
            mi.BeginInit()
            mi.UriSource = New Uri(strGlobals.vorgangsmenuepng)
            mi.EndInit()
            td.Source = mi

            Dim toolTipPanel As StackPanel = New StackPanel()

            If popuplevel = "alles" Then toolTipPanel.Children.Add(t1)
            If popuplevel = "alles" Then toolTipPanel.Children.Add(td)

            toolTipPanel.Width = 560
            toolTipPanel.Height = 400

            ToolTipService.SetToolTip(spVID, toolTipPanel)
            ToolTipService.SetShowDuration(spVID, 60000)
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub
End Class
