Imports System.ComponentModel
Imports System.Data

Public Class win161

    Property ladevorgangabgeschlossen As Boolean = False
    Sub New()
        InitializeComponent()
    End Sub

    Private Sub win161_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        clstools.editTable = "nd_paradigma"
        clstools.editSchema = "paradigma_userdata"
        clstools.editOjektGIDNSpaltenname = "gruppenid"
        clstools.l("editOjektGIDNr : " & clstools.editOjektGIDNr)
        'refreshEditor()
        mset.initdb()
        refresh()
        'tbAnzahl.Text = ndgruppen.Count.ToString
        Dim ddd = mset.gemeinde_verz
        initGemeindeCombo()
        'FocusManager.SetFocusedElement(tbTextfilter, tbTextfilter)
        Keyboard.Focus(tbTextfilter)
        clstools.ParadigmaVersion = My.Resources.BuildDate.Trim.Replace(vbCrLf, "")
        Title = Title & " (Built: " & clstools.ParadigmaVersion & ")"
        ladevorgangabgeschlossen = True

        If clstools.isRemoteCall Then
            If Not CInt(clstools.editOjektGIDNr) < 1 Then
                Dim gruppenEdit As New winEditor(clstools.editSchema, clstools.editTable, CType(clstools.editOjektGIDNr, String), CInt(clstools.editgid))
                gruppenEdit.ShowDialog()
            End If

        End If
        refresh()
        e.Handled = True
    End Sub

    Private Sub refresh()
        fuelleTabNDGruppen(tbGemeindefilter.Text, tbTextfilter.Text)
        fuelleTabNDindividuen(tbGemeindefilter.Text, tbTextfilter.Text)
        fuelleTabErloschen()
    End Sub

    Private Sub fuelleTabErloschen()
        Dim hinweis As String
        mset.basisrec.mydb.SQL = "select * from schutzgebiete.naturdenkmalerloschen_f"
        clstools.l(mset.basisrec.mydb.SQL)
        hinweis = mset.basisrec.getDataDT()
        'mset.ndgruppen = clstools.dt2NDgruppen(mset.basisrec.dt)
        dgNDerloschen.DataContext = mset.basisrec.dt
        tbAnzahlerloschen.Text = mset.basisrec.dt.Rows.Count.ToString
    End Sub

    Sub initGemeindeCombo()
        Dim existing As XmlDataProvider = TryCast(Me.Resources("XMLSourceComboBoxgemeinden"), XmlDataProvider)
        existing.Source = New Uri(mset.gemeinde_verz)
    End Sub
    Private Sub fuelleTabNDindividuen(gemeinde As String, textfilter As String)

        Dim hinweis As String
        mset.basisrec.mydb.SQL = clstools.genSQLNDindividuen(gemeinde, textfilter)

        clstools.l(mset.basisrec.mydb.SQL)
        hinweis = mset.basisrec.getDataDT()
        mset.ndindividuen = clstools.dt2NDindivuduen(mset.basisrec.dt)
        dgNDindividuen.DataContext = mset.ndindividuen
        tbAnzahli.Text = mset.ndindividuen.Count.ToString
    End Sub
    Private Sub fuelleTabNDGruppen(gemeinde As String, textfilter As String)
        Dim hinweis As String
        mset.basisrec.mydb.SQL = clstools.genSQLNDgruppen(gemeinde, textfilter)
        clstools.l(mset.basisrec.mydb.SQL)
        hinweis = mset.basisrec.getDataDT()
        mset.ndgruppen = clstools.dt2NDgruppen(mset.basisrec.dt)
        dgNDgruppen.DataContext = mset.ndgruppen
        tbAnzahl.Text = mset.ndgruppen.Count.ToString
    End Sub
    'Private Sub fuelleTabNDIndividuen(gemeinde As String, textfilter As String)
    '    Dim hinweis As String
    '    mset.basisrec.mydb.SQL = clstools.genSQLNDgruppen(gemeinde, textfilter)
    '    clstools.l(mset.basisrec.mydb.SQL)
    '    hinweis = mset.basisrec.getDataDT()
    '    mset.ndgruppen = clstools.dt2NDgruppen(mset.basisrec.dt)
    '    dgNDgruppen.DataContext = mset.ndgruppen
    '    tbAnzahl.Text = mset.ndgruppen.Count.ToString
    'End Sub


















    Private Sub win161_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        End
    End Sub







    Private Sub dgNDgruppen_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dgNDgruppen.SelectedItem Is Nothing Then Exit Sub

        e.Handled = True
        Dim aktgruppe As clsNDgruppe = CType(dgNDgruppen.SelectedItem, clsNDgruppe)
        If aktgruppe Is Nothing Then Exit Sub

        Dim a = aktgruppe.beschreibung
        clstools.editOjektGIDNr = CType(aktgruppe.aid, String)
        ladevorgangabgeschlossen = False
        If Not clstools.editOjektGIDNr Is Nothing Then
            Dim gruppenEdit As New winEditor(clstools.editSchema, clstools.editTable, clstools.editOjektGIDNr, 0)
            gruppenEdit.ShowDialog()
            refresh()
        End If

        'refreshEditor()
        ladevorgangabgeschlossen = True
        tc1.SelectedIndex = 0
        e.Handled = True
    End Sub

    Private Sub dgNDindividuen_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangabgeschlossen Then Exit Sub
        If dgNDindividuen.SelectedItem Is Nothing Then Exit Sub

        e.Handled = True
        Dim item2 As clsNDinidividuum = CType(dgNDindividuen.SelectedItem, clsNDinidividuum)
        If item2 Is Nothing Then Exit Sub

        Dim gid = item2.gid
        Dim gruppenid = item2.aid

        ladevorgangabgeschlossen = False
        'Dim a = aktgruppe.beschreibung
        'editOjektGIDNr = CType(aktgruppe.aid, String)
        ladevorgangabgeschlossen = False
        If Not gruppenid < 1 Then
            Dim gruppenEdit As New winEditor(clstools.editSchema, clstools.editTable, CType(gruppenid, String), gid)
            gruppenEdit.ShowDialog()
        End If

        ladevorgangabgeschlossen = True
        'tc1.SelectedItem = Nothing
        refresh()
    End Sub

    Private Sub btnNDgruppen2excel_Click(sender As Object, e As RoutedEventArgs)
        Dim excelfile As String
        'excelfile = System.Environment.GetEnvironmentVariable("temp") & "\Paradigma"
        excelfile = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) & "\ausgabe.csv"
        'excelfile = System.IO.Path.GetTempPath() & "ausgabe.csv"
        Dim query As String = clstools.genSQLNDgruppen(tbGemeindefilter.Text, tbTextfilter.Text)
        clstools.createexcelfile(excelfile, mset.ndgruppen)
        clstools.OpenDocument(excelfile)
        e.Handled = True
    End Sub

    Private Sub btnNDindividuen2excel_Click(sender As Object, e As RoutedEventArgs)
        Dim excelfile As String
        'excelfile = System.Environment.GetEnvironmentVariable("temp") & "\Paradigma"
        excelfile = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) & "\ausgabe.csv"
        'excelfile = System.IO.Path.GetTempPath() & "ausgabe.csv"
        clstools.createexcelfile(excelfile, mset.ndindividuen)
        clstools.OpenDocument(excelfile)
        e.Handled = True
    End Sub

    Private Sub cmbgemeinde_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If cmbgemeinde.SelectedItem Is Nothing Then Exit Sub
        Dim gemeindebigNRstring = CStr(cmbgemeinde.SelectedValue)
        Dim myvalx = CType(cmbgemeinde.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        Dim myvali = myvalx.Attributes(0).Value.ToString
        tbGemeindefilter.Text = myvals
        fuelleTabNDGruppen(tbGemeindefilter.Text, tbTextfilter.Text)
    End Sub

    Private Sub btnTextfilter_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True

        Dim hinweis As String
        mset.basisrec.mydb.SQL = clstools.genSQLNDgruppen(tbGemeindefilter.Text, tbTextfilter.Text)
        clstools.l(mset.basisrec.mydb.SQL)
        hinweis = mset.basisrec.getDataDT()
        mset.ndgruppen = clstools.dt2NDgruppen(mset.basisrec.dt)
        dgNDgruppen.DataContext = mset.ndgruppen
        tbAnzahl.Text = mset.ndgruppen.Count.ToString
    End Sub

    Private Sub cmbgemeindeI_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If cmbgemeindeI.SelectedItem Is Nothing Then Exit Sub
        Dim gemeindebigNRstring = CStr(cmbgemeindeI.SelectedValue)
        Dim myvalx = CType(cmbgemeindeI.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString
        Dim myvali = myvalx.Attributes(0).Value.ToString
        tbGemeindefilterI.Text = myvals
        fuelleTabNDindividuen(tbGemeindefilterI.Text, tbTextfilteri.Text)
    End Sub
    Private Sub btnTextfilteri_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim hinweis As String
        mset.basisrec.mydb.SQL = clstools.genSQLNDindividuen(tbGemeindefilterI.Text, tbTextfilteri.Text)
        clstools.l(mset.basisrec.mydb.SQL)
        hinweis = mset.basisrec.getDataDT()
        mset.ndindividuen = clstools.dt2NDindivuduen(mset.basisrec.dt)
        dgNDindividuen.DataContext = mset.ndindividuen
        tbAnzahli.Text = mset.ndindividuen.Count.ToString
    End Sub

    Private Sub dgNDerloschen_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub
End Class
