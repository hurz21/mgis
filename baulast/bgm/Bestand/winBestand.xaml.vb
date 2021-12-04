Public Class winBestand
    Sub New()
        InitializeComponent()
    End Sub

    Private Sub winBestand_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        Dim sql = "select * from " & tools.srv_schema & "." & tools.srv_tablename & "    order by genese desc"
        'Dim sql = "select * from " & tools.srv_schema & "." & tools.srv_tablename & "  where lower(gemeinde)='dietzenbach' order by fs"
        l("bestand geladen")
        initGemarkungsCombo()
        l("bestand geladen " & sql)
        'cmbgemarkung.SelectedIndex = 0
        refreshGISBestand(sql)
        'refreshProbaug()
        l("bestand geladen fwertig")
        Title = "BGM: Bestand; " & Environment.UserName & "; V.: " & bgmVersion
    End Sub

    Private Sub refreshProbaug()
        clsToolWerkzeuge.init()
        dgProbaug.DataContext = rawList
    End Sub

    Private Sub refreshGISBestand(sql As String)
        l("bestand --------")
        dgBestand.DataContext = Nothing
        tools.baulastListe.Clear()
        Dim hinweis As String = ""

        'Dim sql = "select * from " & tools.srv_schema & "." & tools.srv_tablename & " where jahr_blattnr ='" & v & "'"

        hinweis = clsGIStools.getGISrecord2(sql)
        tools.baulastListe = clsGIStools.fstGIS2BLOBJ()
        dgBestand.DataContext = tools.baulastListe
        l("getSerialFromBasis---------------------- ende")
    End Sub

    Sub initGemarkungsCombo()
        l("initGemarkungsCombo " & "C:\kreisoffenbach\bgm\gemarkungen.xml")
        Dim existing As XmlDataProvider = TryCast(Me.Resources("XMLSourceComboBoxgemarkungen"), XmlDataProvider)
        existing.Source = New Uri("C:\kreisoffenbach\bgm\gemarkungen.xml")
        l("initGemarkungsCombo ende")
    End Sub
    Private Sub dgBestand_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If dgBestand.SelectedItem Is Nothing Then Exit Sub
        Dim item As New clsBaulast
        Try
            item = CType(dgBestand.SelectedItem, clsBaulast)
            Dim neu As New winDetail((item.blattnr)) ' 0=modus neu
            neu.ShowDialog()
        Catch ex As Exception
            nachricht(ex.ToString)
            Exit Sub
        End Try
    End Sub

    Private Sub cmbgemarkung_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If cmbgemarkung.SelectedItem Is Nothing Then Exit Sub

        Dim myvali$ = CStr(cmbgemarkung.SelectedValue)
        Dim myvalx = CType(cmbgemarkung.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        dgBestand.DataContext = Nothing
        Dim sql = "select * from " & tools.srv_schema & "." & tools.srv_tablename & "  where lower(gemarkung)='" & myvals.ToLower.Trim & "' order by fs"
        l(sql)
        refreshGISBestand(sql)
        tbTreffer.Text = tools.baulastListe.Count & " Treffer"
    End Sub

    Private Sub dgProbaug_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnPROBAUGinit_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        refreshProbaug()
    End Sub
End Class
