Imports System.ComponentModel
Imports gisDossier
Class MainWindowDossier
    '479424,5543425 wsgHNUGwms im neufestsetzungsverfahren 
    'zwei bplane :   485448,5541926
    'ein bplan, mit begleitdateien
    'ein bplan eine datei: 477371,5542382
    'ein bplan, keine datei: 499000,5543132
    'kein bplan 477382,5543008

    Public protokollGesamt As String = ""
    Public protokollKompakt As String = ""
    Public toptitel As String = ""
    Public ladevorgangAbgeschlossen As Boolean = False
    Public BPLANbeschreibung As String = ""
    Public komplettansicht As Boolean = False
    Private trenn As String = Environment.NewLine
    Private Property RESULT_dateien_Bplan As New List(Of clsGisresult)
#Region "invisible"
    Sub New()
        InitializeComponent()
    End Sub
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        Height = 800

        setDIN()
        m.dossierVersion = My.Resources.BuildDate.Trim.Replace(vbCrLf, "")
        m.GisUser.username = Environment.UserName

        setUNCServer()
        setLogfile() : l("Start " & Now) : l("version:" & m.dossierVersion)
        handleInifile()
        m.mapAllArguments(Environment.GetCommandLineArgs())
#If DEBUG Then
        If Environment.UserName = "hurz" Then
            m.initdb("localhost")
        Else
            m.initdb("w2gis02")
        End If
#Else
        m.initdb("w2gis02")
#End If

        ladevorgangAbgeschlossen = True
        initGISanalyse()
        If m.GisUser.ADgruppenname.ToLower = "bauaufsicht" Then
            btnDossierKonfig.Visibility = Visibility.Collapsed
        End If

        If m.MAPPINGfs.IsNothingOrEmpty Then
            m.flurstuecksModus = clsTools.getIniDossier("flurstuecksmodus")
        Else
            m.flurstuecksModus = True
        End If

        If m.flurstuecksModus Then
            cbFSmodus.IsChecked = True
            Background = Brushes.AliceBlue
        Else
            cbFSmodus.IsChecked = False
            Background = Brushes.Beige
        End If
        If komplettansicht Then
            cbkompaktansicht.IsChecked = False
        Else
            cbkompaktansicht.IsChecked = True
        End If
        initcontrols()
        If m.flurstuecksModus Then
            cbFSmodus.IsChecked = True
            If m.MAPPINGfs.IsNothingOrEmpty Then
                cbFSmodus.IsEnabled = False
            Else
                cbFSmodus.IsEnabled = True
            End If
            m.UTMpt = clsTools.getDoubleFromPointString(m.MAPPINGpunktKoordinatenString)
        End If

        setTitel(m.UTMpt)
        If m.MAPPINGgeometrie = "punkt" Then
            If m.UTMpt.isValid Then
                If cbFSmodus.IsChecked Then
                    m.flurstuecksModus = True
                Else
                    m.flurstuecksModus = False
                End If
            Else
                MsgBox("Punkt ist ungültig, Programmende")
                End
            End If
        End If
        gisanalyse()
        protokollGesamt = tbInfo.Text
        If cbkompaktansicht.IsChecked Then
            tbInfo.Text = protokollKompakt
        End If
        btnprotokollPDF.IsEnabled = True
        kompaktieren()
    End Sub

    Private Sub setUNCServer()
        m.appServerUnc = "\\w2gis02\gdvell"
#If DEBUG Then
        If m.GisUser.username = "hurz" Then
            m.appServerUnc = "d:"
        Else
            m.appServerUnc = "\\w2gis02\gdvell"
        End If
#Else
        m.appServerUnc = m.appServerUnc & ""
#End If
        m.mgisUserRoot = m.appServerUnc & "\apps\test\mgis\"

    End Sub

    Private Sub handleInifile()
        m.userIniProfile = New clsINIDatei(IO.Path.Combine(m.mgisUserRoot, m.GisUser.username & ".ini"))
        m.userIniProfile.WertSchreiben("test", "bla", "jawoll")
        Me.Top = clsTools.setPosition("diverse", "dossierformpositiontop", Me.Top)
        Me.Left = clsTools.setPosition("diverse", "dossierformpositionleft", Me.Left)
    End Sub

    Private Shared Sub setDIN()
        m.dina4InMM.w = 297 : m.dina4InMM.h = 210
        m.dina3InMM.w = 420 : m.dina3InMM.h = 297
    End Sub





    ''' <summary>
    ''' von .showcontrol zu visible/collapsed
    ''' </summary>
    Private Sub initcontrols()
        If Not clsDossier.Ueb.showControl Then spUEB.Visibility = Visibility.Collapsed
        If Not clsDossier.Altlast.showControl Then spaltlast.Visibility = Visibility.Collapsed
        If Not clsDossier.Baulasten.showControl Then spBaulasten.Visibility = Visibility.Collapsed
        If Not clsDossier.Boris.showControl Then spboris.Visibility = Visibility.Collapsed
        If Not clsDossier.Bplan.showControl Then
            spbplan.Visibility = Visibility.Collapsed
            sp2bplan.Visibility = Visibility.Collapsed
            dgZusatzinfo.Visibility = Visibility.Collapsed
        End If
        If Not clsDossier.Eigentuemer.showControl Or Not m.GisUser.istalbberechtigt Then
            spEigentuemer.Visibility = Visibility.Collapsed
        End If
        If Not clsDossier.FFH.showControl Then spFFH.Visibility = Visibility.Collapsed
        If Not clsDossier.Foerder.showControl Then spFoerder.Visibility = Visibility.Collapsed
        If Not clsDossier.Illegale.showControl Then spIllegale.Visibility = Visibility.Collapsed
        If Not clsDossier.IllegaleAlt.showControl Then spIllegaleALT.Visibility = Visibility.Collapsed
        If Not clsDossier.Kehr.showControl Then spKehrbezirk.Visibility = Visibility.Collapsed
        If Not clsDossier.LSG.showControl Then spLSG.Visibility = Visibility.Collapsed
        If Not clsDossier.NSG.showControl Then spNSG.Visibility = Visibility.Collapsed
        If Not clsDossier.WSG.showControl Then spWSG.Visibility = Visibility.Collapsed

        If Not clsDossier.UEBKROF.showControl Then spUEBKROF.Visibility = Visibility.Collapsed
        If Not clsDossier.Hbiotope.showControl Then spHbiotope.Visibility = Visibility.Collapsed
        If Not clsDossier.Hkomplexe.showControl Then spHkomplexe.Visibility = Visibility.Collapsed
        If Not clsDossier.Amphibien.showControl Then spAmph.Visibility = Visibility.Collapsed
        If Not clsDossier.BSE.showControl Then spBSE.Visibility = Visibility.Collapsed
        If Not clsDossier.OEKOKO.showControl Then spOEKOKO.Visibility = Visibility.Collapsed
        If Not clsDossier.ND.showControl Then spND.Visibility = Visibility.Collapsed
        If Not clsDossier.altis16.showControl Then spaltis16.Visibility = Visibility.Collapsed
        If Not clsDossier.paradigmavorgang.showControl Then spparadigma.Visibility = Visibility.Collapsed
        If Not clsDossier.kompensation.showControl Then spKompensation.Visibility = Visibility.Collapsed
        If Not clsDossier.Altstadtsatzung.showControl Then spAltstadtsatzung.Visibility = Visibility.Collapsed
        If Not clsDossier.Schwalben.showControl Then spSchwalben.Visibility = Visibility.Collapsed
    End Sub
    Private Sub initGISanalyse()
        Try
            l(" initGISanalyse ---------------------- anfang")
            komplettansicht = CBool(clsTools.getIniDossier("komplettansicht"))
            clsDossier.UEBKROF.showControl = clsTools.getIniDossier("UEBKROF")
            clsDossier.Ueb.showControl = clsTools.getIniDossier("UEB")
            clsDossier.Bplan.showControl = clsTools.getIniDossier("BPLAN")
            clsDossier.Boris.showControl = clsTools.getIniDossier("boris")
            clsDossier.Baulasten.showControl = clsTools.getIniDossier("baulasten")
            clsDossier.Altlast.showControl = clsTools.getIniDossier("altlast")
            clsDossier.Eigentuemer.showControl = clsTools.getIniDossier("eigentuemer")
            clsDossier.FFH.showControl = clsTools.getIniDossier("ffh")
            clsDossier.Foerder.showControl = clsTools.getIniDossier("foerder")
            clsDossier.Illegale.showControl = clsTools.getIniDossier("illegale")
            clsDossier.IllegaleAlt.showControl = clsTools.getIniDossier("illegalealt")
            clsDossier.Kehr.showControl = clsTools.getIniDossier("kehrbezirk")
            clsDossier.LSG.showControl = clsTools.getIniDossier("lsg")
            clsDossier.NSG.showControl = clsTools.getIniDossier("nsg")
            clsDossier.WSG.showControl = clsTools.getIniDossier("wsg")
            clsDossier.Hbiotope.showControl = clsTools.getIniDossier("hbiotope")
            clsDossier.Hkomplexe.showControl = clsTools.getIniDossier("hkomplexe")
            clsDossier.Amphibien.showControl = clsTools.getIniDossier("amph")
            clsDossier.BSE.showControl = clsTools.getIniDossier("bse")
            clsDossier.OEKOKO.showControl = clsTools.getIniDossier("oekoko")
            clsDossier.ND.showControl = clsTools.getIniDossier("nd")
            clsDossier.altis16.showControl = clsTools.getIniDossier("altis16")
            clsDossier.paradigmavorgang.showControl = clsTools.getIniDossier("paradigmavorgang")
            clsDossier.kompensation.showControl = clsTools.getIniDossier("kompensation")
            clsDossier.Schwalben.showControl = clsTools.getIniDossier("schwalben")
            clsDossier.Altstadtsatzung.showControl = clsTools.getIniDossier("altstadtsatzung")
            l(" initGISanalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in initGISanalyse: " & ex.ToString())
        End Try
    End Sub

#End Region


    Private Sub schwalbenAnalyse()
        Try
            l("schwalben ---------------------- anfang")
            clsDossier.Schwalben.schematabelle = "public.dossier_schwalben"
            If clsUniversell.getInfo4point(m.UTMpt, clsDossier.Schwalben, clsDossier.Schwalben.strerror, "gid") Then
                clsDossier.Schwalben.result = "Schwalben ---------------------------------" & Environment.NewLine &
                                 clsDossier.Schwalben.result
                clsDossier.Schwalben.kurz = clsString.removeLeadingChar(clsDossier.Schwalben.kurz, ",")
                tbSchwalben.Text = tbSchwalben.Text & ": " & clsDossier.Schwalben.kurz
                tbSchwalben.ToolTip = tbSchwalben.Text
                tbSchwalben.Background = Brushes.LightGreen
                btnSchwalben.Visibility = Visibility.Hidden
                btnSchwalben.IsEnabled = True
                btnSchwalbentext.IsEnabled = True
                protokollKompakt = protokollKompakt & "Schwalben: " & clsDossier.Schwalben.kurz & trenn
            Else
                If clsDossier.Schwalben.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Schwalben: FEHLER bei Analyse von Tab.: " & clsDossier.Schwalben.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Schwalben: FEHLER bei analyse von Tab. " & clsDossier.Schwalben.schematabelle)
                    btnSchwalbentext.Visibility = Visibility.Collapsed
                    tbSchwalben.FontSize = 10
                Else
                    btnSchwalbentext.Visibility = Visibility.Collapsed
                    tbSchwalben.FontSize = 10
                    tbSchwalben.Text = "Keine Objekte für Schwalben gefunden"
                    clsDossier.Schwalben.result = "Schwalben - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Objekte für Schwalben gefunden"
                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.Schwalben.result & Environment.NewLine)
            l("Schwalben ---------------------- ende")
        Catch ex As Exception
            l("Fehler in Schwalben: " & ex.ToString())
        End Try
    End Sub

    Private Sub kompensationAnalyse()
        Try
            l("kompensation ---------------------- anfang")
            clsDossier.kompensation.schematabelle = "public.dossier_kompensation"
            If clsKompensation.getInfo4point(m.UTMpt, clsDossier.kompensation, clsDossier.kompensation.strerror) Then
                clsDossier.kompensation.result = "kompensationsflächen ---------------------------------" & Environment.NewLine &
                                 clsDossier.kompensation.result
                clsDossier.kompensation.kurz = clsString.removeLeadingChar(clsDossier.kompensation.kurz, ",")
                tbKompensation.Text = tbKompensation.Text & ": " & clsDossier.kompensation.kurz
                tbKompensation.ToolTip = tbKompensation.Text
                tbKompensation.Background = Brushes.LightGreen
                btnKompensationtext.IsEnabled = True
                protokollKompakt = protokollKompakt & "Komp.Flächen: " & clsDossier.kompensation.kurz & trenn
            Else
                If clsDossier.kompensation.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Kompensationsflächen: FEHLER bei Analyse von Tab.: " & clsDossier.kompensation.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Kompensationsflächen: FEHLER bei analyse von Tab. " & clsDossier.kompensation.schematabelle)
                    tbKompensation.FontSize = 10
                Else

                    tbKompensation.FontSize = 10
                    tbKompensation.Text = "Keine Objekte für Kompensationsflächen gefunden"
                    clsDossier.kompensation.result = "Kompensationsflächen - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Objekte für Kompensationsflächen gefunden"
                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.kompensation.result & Environment.NewLine)
            l("kompensation ---------------------- ende")
        Catch ex As Exception
            l("Fehler in kompensation: " & ex.ToString())
        End Try
    End Sub
    Private Sub paradigmavorgangAnalyse()
        Try
            l("paradigmavorgang ---------------------- anfang")
            clsDossier.paradigmavorgang.schematabelle = "public.dossier_paradigmavorgang"


            If clsParadigmaVorgang.getInfo4point(m.UTMpt, clsDossier.paradigmavorgang, clsDossier.paradigmavorgang.strerror) Then
                clsDossier.paradigmavorgang.result = "paradigmavorgang ---------------------------------" & Environment.NewLine &
                                 clsDossier.paradigmavorgang.result
                clsDossier.paradigmavorgang.kurz = clsString.removeLeadingChar(clsDossier.paradigmavorgang.kurz, ",")
                tbparadigma.Text = tbparadigma.Text & ": " & clsDossier.paradigmavorgang.kurz
                tbparadigma.ToolTip = tbparadigma.Text
                tbparadigma.Background = Brushes.LightGreen
                'btnparadigma.Visibility = Visibility.Visible
                'btnparadigma.IsEnabled = True
                cmbParadigma.Visibility = Visibility.Visible
                cmbParadigma.DataContext = clsDossier.paradigmavorgang.ParadigmaListe
                'btnparadigmavorgangeditor.IsEnabled = True
                'btnparadigmavorgangeditor.Visibility = Visibility.Visible
                btnparadigmatext.IsEnabled = True
                protokollKompakt = protokollKompakt & "Paradigma: " & clsDossier.paradigmavorgang.kurz & trenn
            Else
                If clsDossier.paradigmavorgang.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "paradigmavorgang: FEHLER bei Analyse von Tab.: " & clsDossier.paradigmavorgang.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "paradigmavorgang: FEHLER bei analyse von Tab. " & clsDossier.paradigmavorgang.schematabelle)
                    btnparadigmatext.Visibility = Visibility.Collapsed
                    'btnparadigma.Visibility = Visibility.Hidden
                    cmbParadigma.Visibility = Visibility.Hidden
                    tbparadigma.FontSize = 10
                Else
                    btnparadigmatext.Visibility = Visibility.Collapsed
                    'btnparadigma.Visibility = Visibility.Hidden
                    cmbParadigma.Visibility = Visibility.Hidden
                    tbparadigma.FontSize = 10
                    tbparadigma.Text = "Keine Objekte für paradigmavorgang gefunden"
                    clsDossier.paradigmavorgang.result = "paradigmavorgang - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Objekte für paradigmavorgang gefunden"
                End If

            End If
            zwischenInfo(Environment.NewLine & clsDossier.paradigmavorgang.result & Environment.NewLine)
            l("paradigmavorgang ---------------------- ende")
        Catch ex As Exception
            l("Fehler in paradigmavorgang: " & ex.ToString())
        End Try
    End Sub

    Private Sub altis16Analyse()
        Try
            l("altis16 ---------------------- anfang")
            clsDossier.altis16.schematabelle = "public.dossier_altis16"
            If clsAltis16.getInfo4point(m.UTMpt, clsDossier.altis16, clsDossier.altis16.strerror) Then
                clsDossier.altis16.result = "altis16 ---------------------------------" & Environment.NewLine &
                                 clsDossier.altis16.result
                clsDossier.altis16.kurz = clsString.removeLeadingChar(clsDossier.altis16.kurz, ",")
                tbaltis16.Text = tbaltis16.Text & ": " & clsDossier.altis16.kurz
                tbaltis16.ToolTip = tbaltis16.Text
                tbaltis16.Background = Brushes.LightGreen
                btnaltis16.Visibility = Visibility.Hidden
                btnaltis16.IsEnabled = True
                btnaltis16text.IsEnabled = True
                protokollKompakt = protokollKompakt & "ALTIS: " & clsDossier.altis16.kurz & trenn
            Else
                If clsDossier.altis16.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "altis16: FEHLER bei Analyse von Tab.: " & clsDossier.altis16.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "altis16: FEHLER bei analyse von Tab. " & clsDossier.altis16.schematabelle)
                    btnaltis16text.Visibility = Visibility.Collapsed
                    tbaltis16.FontSize = 10
                Else
                    btnaltis16text.Visibility = Visibility.Collapsed
                    tbaltis16.FontSize = 10
                    tbaltis16.Text = "Keine Objekte für altis16 gefunden"
                    clsDossier.altis16.result = "altis16 - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Objekte für altis16 gefunden"
                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.altis16.result & Environment.NewLine)
            l("altis16 ---------------------- ende")
        Catch ex As Exception
            l("Fehler in altis16: " & ex.ToString())
        End Try
    End Sub

    Private Sub ndAnalyse()
        Try
            l("Naturdenkmale ---------------------- anfang")
            clsDossier.ND.schematabelle = "public.dossier_nd"
            If clsND.getInfo4point(m.UTMpt, clsDossier.ND, clsDossier.ND.strerror) Then
                clsDossier.ND.result = "Naturdenkmale ---------------------------------" & Environment.NewLine &
                                 clsDossier.ND.result
                clsDossier.ND.kurz = clsString.removeLeadingChar(clsDossier.ND.kurz, ",")
                tbND.Text = tbND.Text & ": " & clsDossier.ND.kurz
                tbND.ToolTip = tbND.Text
                tbND.Background = Brushes.LightGreen
                btnND.Visibility = Visibility.Visible
                btnND.IsEnabled = True
                btnNDeditor.IsEnabled = True
                btnNDeditor.Visibility = Visibility.Visible
                btnNDtext.IsEnabled = True
                protokollKompakt = protokollKompakt & "ND: " & clsDossier.ND.kurz & trenn
            Else
                If clsDossier.ND.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "ND: FEHLER bei Analyse von Tab.: " & clsDossier.ND.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "ND: FEHLER bei analyse von Tab. " & clsDossier.ND.schematabelle)
                    btnNDtext.Visibility = Visibility.Collapsed
                    btnNDeditor.Visibility = Visibility.Collapsed
                    tbND.FontSize = 10
                Else
                    btnNDtext.Visibility = Visibility.Collapsed
                    btnNDeditor.Visibility = Visibility.Collapsed
                    tbND.FontSize = 10
                    tbND.Text = "Keine Objekte für Naturdenkmale gefunden"
                    clsDossier.ND.result = "Naturdenkmale - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Objekte für Naturdenkmale gefunden"
                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.ND.result & Environment.NewLine)
            l("Naturdenkmale ---------------------- ende")
        Catch ex As Exception
            l("Fehler in Naturdenkmale: " & ex.ToString())
        End Try
    End Sub

    Private Sub bplananalyse(schematabelle As String)
        Dim Plannr As String()
        Dim bplanPDFBegeleitDateiListe As New List(Of clsGisresult)
        ReDim Plannr(20)
        Dim dt As System.Data.DataTable
        Dim bplankurzliste As String = ""
        Try
            l(" bplananalyse ---------------------- anfang")
            schematabelle = "dossier_bplan"
            clsDossier.Bplan.schematabelle = schematabelle
            'zwischenInfo("Bebauungspläne werden für den Punkt gesucht!")
            Dim hatbplan As Boolean = clsBplan.getBplanInfo4point(m.UTMpt, dt, schematabelle, clsDossier.Bplan.strerror)
            If hatbplan Then
                clsBplan.hurz(dt, clsDossier.Bplan.result, Plannr, RESULT_dateien_Bplan, bplankurzliste)
                Debug.Print("cl" & clsDossier.Bplan.datei)
                btnbplanaufruf.IsEnabled = True
                btnbplanaufruf.ToolTip = "Taste drücken zum anzeigen"
                tbbplangueltig.Text = "B-Plan vorhanden: " & Plannr(0)
                protokollKompakt = protokollKompakt & "B-Plan: " & Plannr(0) & trenn
                tbbplangueltig.Background = Brushes.LightGreen
                bplanPDFBegeleitDateiListe = clsDossier.makebplanPDFliste(RESULT_dateien_Bplan)
                dgZusatzinfo.DataContext = bplanPDFBegeleitDateiListe
                btnbplan1text.IsEnabled = True
                '
                'bplan1-pdf anbieten:
                '
                If RESULT_dateien_Bplan.Count < 1 Then
                    clsDossier.Bplan.datei = ""
                Else
                    clsDossier.Bplan.datei = RESULT_dateien_Bplan.Item(0).datei.FullName.Trim
                End If
                If BplanHatPdfDatei(clsDossier.Bplan.datei) Then
                    'ein bplan-pdf ist vorhanden 
                    btnbplanaufruf.IsEnabled = True
                    tbbplangueltig.Text = " B-Plan-PDF vorhanden: " & bplankurzliste
                Else

                    btnbplanaufruf.IsEnabled = False
                    tbbplangueltig.Text = " B-Plan-PDF fehlt: " & bplankurzliste
                End If
                '
                If bplanHatBegleitDokumente(bplanPDFBegeleitDateiListe) Then
                    dgZusatzinfo.Visibility = Visibility.Visible
                    btnbplan1text.IsEnabled = True
                Else
                    dgZusatzinfo.Visibility = Visibility.Collapsed

                End If
                'If bplanPDFBegeleitDateiListe Is Nothing OrElse bplanPDFBegeleitDateiListe.Count < 1 Then
                '    'dgZusatzinfo.Visibility = Visibility.Collapsed
                '    'tbbplangueltig.Text = " B-Plan vorhanden: " & bplankurzliste & " Keine Scans vorhanden"
                '    'btnbplan1text.IsEnabled = True
                '    'btnbplanaufruf.IsEnabled = False
                '    sp2bplan.Visibility = Visibility.Collapsed
                'Else
                '    'dgZusatzinfo.Visibility = Visibility.Visible
                '    'btnbplan1text.IsEnabled = True

                If hatZweitenBplan(RESULT_dateien_Bplan) Then
                    btnbplan2text.IsEnabled = True
                    sp2bplan.Visibility = Visibility.Visible
                    btnbplanaufruf2.IsEnabled = True
                    clsDossier.Bplan.link = RESULT_dateien_Bplan.Item(1).datei.FullName.Trim
                    tbbplangueltig2.Text = "B-Plan vorhanden: " & Plannr(1)
                    protokollKompakt = protokollKompakt & "B-Plan: " & Plannr(1) & trenn
                Else
                    sp2bplan.Visibility = Visibility.Collapsed
                    btnbplan2text.IsEnabled = False
                End If
                zwischenInfo(clsDossier.Bplan.result)
            Else
                'hat KEINEN BPLAN = aussenbereich
                '
                If clsDossier.Bplan.strerror.ToLower.StartsWith("fehler") Then
                    'fehler bei verschneidung
                    dgZusatzinfo.Visibility = Visibility.Collapsed
                    btnbplanaufruf.Visibility = Visibility.Collapsed
                    btnbplan2text.Visibility = Visibility.Collapsed
                    btnbplan1text.Visibility = Visibility.Collapsed
                    'tbbplangueltig.Text = "" 'Plannr
                    tbbplangueltig.Text = Environment.NewLine & "FEHLER bei Analyse von Tab.: " & clsDossier.Bplan.schematabelle
                    protokollKompakt = protokollKompakt & "B-PLAN: FEHLER bei Analyse von Tab.: " & clsDossier.Bplan.schematabelle & trenn
                    tbbplangueltig.FontSize = 10
                    zwischenInfo(Environment.NewLine & "B-Plan: FEHLER bei analyse von Tab. " & clsDossier.Bplan.schematabelle)
                    sp2bplan.Visibility = Visibility.Collapsed
                Else
                    dgZusatzinfo.Visibility = Visibility.Collapsed
                    btnbplanaufruf.Visibility = Visibility.Collapsed
                    btnbplan2text.Visibility = Visibility.Collapsed
                    btnbplan1text.Visibility = Visibility.Collapsed
                    'tbbplangueltig.Text = "" 'Plannr
                    tbbplangueltig.Text = Environment.NewLine & "Kein B-Plan"
                    tbbplangueltig.FontSize = 10
                    zwischenInfo("Kein B-Plan für diesen Punkt")
                    sp2bplan.Visibility = Visibility.Collapsed
                End If
            End If


            zwischenInfo("B-Plan Analyse fertig---------------------" & Environment.NewLine)
            l(" bplananalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in bplananalyse: " & ex.ToString())
        End Try
    End Sub

    Private Function BplanHatPdfDatei(datei As String) As Boolean
        If datei.Length > 10 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function bplanHatBegleitDokumente(bplanPDFBegeleitDateiListe As List(Of clsGisresult)) As Boolean
        If bplanPDFBegeleitDateiListe Is Nothing OrElse bplanPDFBegeleitDateiListe.Count < 1 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Function hatZweitenBplan(rESULT_dateien_Bplan As List(Of clsGisresult)) As Boolean
        If rESULT_dateien_Bplan.Count > 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub kehrbezirksAnalyse()
        Try
            l(" kehrbezirksAnalyse ---------------------- anfang")
            clsDossier.Kehr.schematabelle = "public.dossier_kehrbezirk"
            If clsKehrbezirk.getKehrbezirkInfo4point(m.UTMpt, clsDossier.Kehr, "public.dossier_kehrbezirk",
                                                     clsDossier.Kehr.strerror) Then
                'clsDossier.Kehr.result = "Kehrbezirksfeststellung ---------------------------------" & Environment.NewLine &
                '                  clsDossier.Kehr.result
                tbKehrbezirk.Background = Brushes.LightGreen
                btnKehrbezirktext.IsEnabled = True
                tbKehrbezirk.Text = tbKehrbezirk.Text & " " & clsDossier.Kehr.kurz
                protokollKompakt = protokollKompakt & clsDossier.Kehr.result & trenn
            Else
                If clsDossier.Kehr.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Kehr: FEHLER bei Analyse von Tab.: " & clsDossier.Kehr.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Kehr: FEHLER bei analyse von Tab. " & clsDossier.Kehr.schematabelle)
                    tbKehrbezirk.FontSize = 10
                    btnKehrbezirktext.Visibility = Visibility.Collapsed
                Else
                    tbKehrbezirk.Text = "Kein Kehrbezirk"
                    tbKehrbezirk.FontSize = 10
                    btnKehrbezirktext.Visibility = Visibility.Collapsed
                    clsDossier.Kehr.result = "Kehrbezirksfeststellung ---------------------------------" & Environment.NewLine & "Kein Kehrbezirk festgestellt!"

                End If
            End If
            zwischenInfo("Kehrbezirksfeststellung ---------------------------------" & Environment.NewLine & clsDossier.Kehr.result & Environment.NewLine)
            zwischenInfo("Kehrbezirksfeststellung Ende-------------------" & Environment.NewLine)
            l(" kehrbezirksAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in kehrbezirksAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub eigentuemerAnalyse(schematabelle As String, ByRef strError As String)
        Dim fs As String = "" : Dim albflaeche As String
        zwischenInfo("Eigentümer festellen ---------------------")
        clsDossier.Eigentuemer.schematabelle = schematabelle
        If clsEigentuemerAnalyse.getFS4coordinates(m.UTMpt, fs, schematabelle, strError, m.aktFST.normflst.weistauf, m.aktFST.normflst.zeigtauf,
                                                   albflaeche) Then
            m.aktFST = New ParaFlurstueck
            m.aktFST.normflst.FS = fs
            m.aktFST.normflst.splitFS(fs)
            clsDossier.Eigentuemer.kurz = clsPgtools.getSchnellbatchEigentuemer(fs.Trim)
            Dim flaechentext As String = clsFSTtools.getFlaecheZuFlurstueck(m.aktFST, strError)

            clsDossier.Eigentuemer.result = "Eigentümerfeststellung für Flurstück " &
                                      m.aktFST.normflst.toShortstring(",") & Environment.NewLine &
                                        flaechentext & m.NASlage.strAusgabe & " " & Environment.NewLine &
                                        "Eigentümer in Kurzform: " & Environment.NewLine &
                                       clsDossier.Eigentuemer.kurz & " ---------------------------"
            btnEigentuemerPDF.IsEnabled = True
            btnEigentuemertext.IsEnabled = True
            tbEigentuemer.Background = Brushes.LightGreen
            protokollKompakt = protokollKompakt & "EigentümerIn: " & clsDossier.Eigentuemer.kurz & trenn
        Else

            If clsDossier.Eigentuemer.strerror.ToLower.StartsWith("fehler") Then
                protokollKompakt = protokollKompakt & "Eigentuemer: FEHLER bei Analyse von Tab.: " & clsDossier.Eigentuemer.schematabelle & trenn
                zwischenInfo(Environment.NewLine & "Eigentuemer: FEHLER bei analyse von Tab. " & clsDossier.Eigentuemer.schematabelle)
            Else
                clsDossier.Eigentuemer.result = "Keine EigentümerInformation gefunden ---------------------------"
            End If
            btnEigentuemerPDF.IsEnabled = False
            btnEigentuemertext.IsEnabled = False
        End If
        zwischenInfo(Environment.NewLine & clsDossier.Eigentuemer.result)
    End Sub

    Private Shared Sub calcNASLageString(weistauf As String, zeigtauf As String, ByRef fehler As String, ByRef lage As NASlage)
        lage = clsNASLageTools.getlage(weistauf, zeigtauf, fehler)
        If lage Is Nothing Then
            lage = New NASlage
            lage.strAusgabe = "Keine Lage ! (Aussenbereich?)"
        Else
            lage.strassenname = clsNASLageTools.getstrassename(lage.lageschluessel)
            lage.GemeindeName = clsNASLageTools.getgemeindename(lage.GemeindeNr)
            lage.strAusgabe = "Lage (Grundbuch): " & lage.GemeindeName & ", " & lage.strassenname & " " & lage.hausnummer.Trim

        End If
    End Sub

    Private Sub btnbplanaufruf_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If clsDossier.Bplan.datei.IsNothingOrEmpty Then
            MsgBox("Kein Bplan zur Adresse gefunden.")
        Else
            Process.Start(clsDossier.Bplan.datei)
        End If
    End Sub

    Private Sub btnUEBKROF_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim datei As String
        datei = clsDossier.UEBKROF.datei
        datei = m.appServerUnc & "\fkat\wasser\ueberschw\texte\" & datei & ".pdf"
        OpenDokument(datei)
    End Sub

    Private Sub btnUEBKROFtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.UEBKROF.result)
    End Sub

    Private Sub btnbplan1text_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Bplan.result)
    End Sub

    Private Sub btnUEB_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim bbox As String
        Dim url, wmslayers, wmsquery_layers As String
        wmslayers = "Ueberschwemmungsgebiete_HQ100_nach_HWG"
        wmslayers = wmslayers & ",Abflussgebiete_HQ100_nach_HWG"
        wmslayers = wmslayers & ",Hochwasser_mit_niedriger_Wahrscheinlichkeit"
        wmslayers = wmslayers & ",Hochwasser_mit_hoher_Wahrscheinlichkeit"
        wmslayers = wmslayers & ",Hochwasser_mit_mittlerer_Wahrscheinlichkeit"

        wmsquery_layers = wmslayers
        bbox = clsWMS.calcVollstBbox(CInt(m.UTMpt.X) & "," & CInt(m.UTMpt.Y))

        url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 248, CInt(m.MAPPINGhoehe), CInt(m.MAPPINGbreite),
                                                  CInt(m.MAPPINGscreenX),
                                              CInt(m.MAPPINGscreenY), "text/html",
                                              wmslayers, wmsquery_layers, clsDossier.Ueb.strerror)
        Process.Start(url)
    End Sub

    Private Sub btnUEBtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Ueb.result)
    End Sub

    Private Sub btnFoerder_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnFoerdertext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Foerder.result)
    End Sub


    Private Sub btnBaulastentext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Baulasten.result)
    End Sub

    Private Sub btnBaulasten_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim baulast As String
        If clsDossier.Baulasten.datei.ToLower.StartsWith("keine") Then
            MessageBox.Show("Kein Baulast-Scan vorhanden")
        Else
            baulast = m.appServerUnc & "\" & clsDossier.Baulasten.datei.Replace("/", "\")
            OpenDokument(baulast)
        End If
    End Sub

    Private Sub btnWSG_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        oeffneMehrerePDFs(clsDossier.WSG.datei)
    End Sub

    Private Sub btnIllegaleALT_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnIllegaleALTtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.IllegaleAlt.result)
    End Sub

    Private Sub oeffneMehrerePDFs(pdfstringKomma As String)
        Dim a() As String
        Try
            l(" oeffneMehrerePDFs ---------------------- anfang")
            a = pdfstringKomma.Split(","c)
            For i = 0 To a.Count - 1
                If Not a(i).IsNothingOrEmpty Then OpenDokument(a(i))
            Next
            l(" oeffneMehrerePDFs ---------------------- ende")
        Catch ex As Exception
            l("Fehler in oeffneMehrerePDFs: " & ex.ToString())
        End Try
    End Sub

    Private Sub btnAlleTexte_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        Dim gesamt As String = getGesamtText()
        zwischenInfo(gesamt)
    End Sub



    Private Sub btnaltlasttext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Altlast.result)
    End Sub

    Private Sub btnboristtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Boris.result)
    End Sub

    Private Sub btnIllegale_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        clsTools.paradigmavorgangaufrufen(clsDossier.Illegale.datei)
    End Sub

    Private Sub btnIllegaletext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Illegale.result)
    End Sub

    Private Sub btnaltlast_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Function getGesamtText() As String
        Dim trenn As String = Environment.NewLine
        Return toptitel & trenn & clsDossier.Boris.result & trenn & clsDossier.Eigentuemer.result &
            clsDossier.Bplan.result & trenn & trenn &
            clsDossier.FFH.result & trenn &
            clsDossier.LSG.result &
           trenn & clsDossier.NSG.result & trenn &
           clsDossier.WSG.result &
           clsDossier.Kehr.result & trenn &
           clsDossier.Altlast.result & trenn
    End Function



    Private Sub btnFFHAnlage_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Try
            Process.Start(clsDossier.FFH.link)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnWSGtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.WSG.result)
    End Sub

    Private Sub btnKehrbezirktext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Kehr.result)
    End Sub
    Private Sub btnFFHtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.FFH.result)
    End Sub
    Private Sub btnFFH_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        OpenDokument(clsDossier.FFH.datei)
    End Sub
    Private Sub btnNSG_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        OpenDokument(clsDossier.NSG.datei)
    End Sub
    Private Sub btnLSGtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.LSG.result)
    End Sub
    Private Sub btnNSGtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.NSG.result)
    End Sub

    Private Sub btnLSG_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        'Verordnungstext:	\\w2gis02\gdvell\\nkat\aid\342\texte\14-2000.pdf	
        OpenDokument(clsDossier.LSG.datei)
    End Sub


    Private Sub btnboris_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim bbox As String
        bbox = clsWMS.calcVollstBbox(CInt(m.UTMpt.X) & "," & CInt(m.UTMpt.Y))
        Dim url As String
        url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 379, CInt(m.MAPPINGhoehe), CInt(m.MAPPINGbreite),
                                                  CInt(m.MAPPINGscreenX), CInt(m.MAPPINGscreenY),
                                              "text/html", "", "", clsDossier.Boris.strerror)
        Process.Start(url)
    End Sub
    Private Sub btnborisERleuterung_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        clsDossier.Boris.link = "https://hvbg.hessen.de/sites/hvbg.hessen.de/files/0606006-DARMSTADT-2018.pdf"

        Process.Start(clsDossier.Boris.link)
    End Sub




    Private Sub btnHbiotope_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnHbiotopeAnlage_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnHbiotopetext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Hbiotope.result)
    End Sub

    Private Sub btnHkomplexetext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Hkomplexe.result)
    End Sub

    Private Sub btnHkomplexeAnlage_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnHkomplexe_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnAmphtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Amphibien.result)
    End Sub

    Private Sub btnAmph_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        clsTools.klassischeDBabfrage(CInt(clsDossier.Amphibien.link), "amphibien", "MSKamphibien_2.htm")
    End Sub

    Private Sub btnBSEtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.BSE.result)
    End Sub

    Private Sub btnOEKOKOtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.OEKOKO.result)
    End Sub

    Private Sub dgZusatzinfo_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If dgZusatzinfo.SelectedItem Is Nothing Then Exit Sub
        Dim item As clsGisresult
        Try
            item = CType(dgZusatzinfo.SelectedItem, clsGisresult)
        Catch ex As Exception
            e.Handled = True
            Exit Sub
        End Try
        item = CType(dgZusatzinfo.SelectedItem, clsGisresult)
        If item Is Nothing Then
        Else
            OpenDokument(CStr(item.datei.FullName))
        End If
        dgZusatzinfo.SelectedItem = Nothing
    End Sub

    Private Sub OEKOKOAnalyse()
        Try
            l("OEKOKO ---------------------- anfang")
            clsDossier.OEKOKO.schematabelle = "public.dossier_oekoko"
            If clsOekoko.getInfo4point(m.UTMpt, clsDossier.OEKOKO, clsDossier.OEKOKO.strerror) Then
                clsDossier.OEKOKO.result = "Ökokonto ---------------------------------" & Environment.NewLine &
                                 clsDossier.OEKOKO.result
                clsDossier.OEKOKO.kurz = clsString.removeLeadingChar(clsDossier.OEKOKO.kurz, ",")
                tbOEKOKO.Text = tbOEKOKO.Text & ": " & clsDossier.OEKOKO.kurz
                tbOEKOKO.ToolTip = tbOEKOKO.Text
                tbOEKOKO.Background = Brushes.LightGreen
                'btnBSE.Visibility = Visibility.Visible
                'btnOEKOKO.IsEnabled = True
                btnOEKOKOtext.IsEnabled = True
                protokollKompakt = protokollKompakt & "Ökokonto: " & clsDossier.OEKOKO.kurz & trenn
            Else

                If clsDossier.OEKOKO.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "XXX: FEHLER bei Analyse von Tab.: " & clsDossier.OEKOKO.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "XXX: FEHLER bei analyse von Tab. " & clsDossier.OEKOKO.schematabelle)
                    btnOEKOKOtext.Visibility = Visibility.Collapsed
                    tbOEKOKO.FontSize = 10
                Else
                    btnOEKOKOtext.Visibility = Visibility.Collapsed
                    tbOEKOKO.FontSize = 10
                    tbOEKOKO.Text = "Keine Objekte für Ökokonto gefunden"
                    clsDossier.OEKOKO.result = "Ökokonto - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Objekte für Ökokonto gefunden"
                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.OEKOKO.result & Environment.NewLine)
            l("OEKOKO ---------------------- ende")
        Catch ex As Exception
            l("Fehler in OEKOKO: " & ex.ToString())
        End Try
    End Sub

    Private Sub bseAnalyse()
        Try
            l("bse ---------------------- anfang")
            clsDossier.BSE.schematabelle = "public.dossier_bse"
            If clsBSE.getInfo4point(m.UTMpt, clsDossier.BSE, clsDossier.BSE.strerror) Then
                clsDossier.BSE.result = "BannSchutzErholungswald ---------------------------------" & Environment.NewLine &
                                 clsDossier.BSE.result
                clsDossier.BSE.kurz = clsString.removeLeadingChar(clsDossier.BSE.kurz, ",")
                tbBSE.Text = tbBSE.Text & ": " & clsDossier.BSE.kurz
                tbBSE.ToolTip = tbBSE.Text
                tbBSE.Background = Brushes.LightGreen
                'btnBSE.Visibility = Visibility.Visible
                'btnBSE.IsEnabled = True
                btnBSEtext.IsEnabled = True
                protokollKompakt = protokollKompakt & "BannSchutzErholungswald: " & clsDossier.BSE.kurz & trenn
            Else
                If clsDossier.BSE.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "BSE: FEHLER bei Analyse von Tab.: " & clsDossier.BSE.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "BSE: FEHLER bei analyse von Tab. " & clsDossier.BSE.schematabelle)
                    spBSE.Visibility = Visibility.Collapsed
                Else
                    btnBSEtext.Visibility = Visibility.Collapsed
                    tbBSE.FontSize = 10
                    tbBSE.Text = "Keine Objekte für BannSchutzErholungswald gefunden"
                    clsDossier.BSE.result = "BannSchutzErholungswald - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Objekte für BannSchutzErholungswald gefunden"
                End If

            End If
            zwischenInfo(Environment.NewLine & clsDossier.BSE.result & Environment.NewLine)
            l("bse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in bse: " & ex.ToString())
        End Try
    End Sub

    Private Sub AmphibienAnalyse()
        Try
            l("Amphibienkartierung ---------------------- anfang")
            clsDossier.Amphibien.schematabelle = "public.dossier_amphibien"
            If clsAmphibien.getInfo4point(m.UTMpt, clsDossier.Amphibien, clsDossier.Amphibien.strerror) Then
                clsDossier.Amphibien.result = "Amphibienkartierung ---------------------------------" & Environment.NewLine &
                                 clsDossier.Amphibien.result
                clsDossier.Amphibien.kurz = clsString.removeLeadingChar(clsDossier.Amphibien.kurz, ",")
                tbAmph.Text = tbAmph.Text & ": " & clsDossier.Amphibien.kurz
                tbAmph.ToolTip = tbAmph.Text
                tbAmph.Background = Brushes.LightGreen
                btnAmph.Visibility = Visibility.Visible
                btnAmph.IsEnabled = True
                btnAmphtext.IsEnabled = True
                protokollKompakt = protokollKompakt & "Amphibien: " & clsDossier.Amphibien.kurz & trenn
            Else

                If clsDossier.Amphibien.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "XXX: FEHLER bei Analyse von Tab.: " & clsDossier.Amphibien.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "XXX: FEHLER bei analyse von Tab. " & clsDossier.Amphibien.schematabelle)
                    btnAmphtext.Visibility = Visibility.Collapsed
                    btnAmph.Visibility = Visibility.Hidden
                    tbAmph.FontSize = 10
                Else
                    btnAmphtext.Visibility = Visibility.Collapsed
                    btnAmph.Visibility = Visibility.Hidden
                    tbAmph.FontSize = 10
                    tbAmph.Text = "Keine Objekte der Amphibienkartierung gefunden"
                    clsDossier.Amphibien.result = "Amphibienkartierung - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Amphibienkartierung gefunden"
                End If

            End If
            zwischenInfo(Environment.NewLine & clsDossier.Amphibien.result & Environment.NewLine)
            l("Amphibienkartierung ---------------------- ende")
        Catch ex As Exception
            l("Fehler in Amphibienkartierung: " & ex.ToString())
        End Try
    End Sub

    Private Sub HKomplexeAnalyse()
        Try
            l("HKomplexe ---------------------- anfang")
            clsDossier.Hkomplexe.schematabelle = "public.dossier_hkomplexe"
            If clsKomplexe.getInfo4point(m.UTMpt, clsDossier.Hkomplexe, clsDossier.Hkomplexe.strerror) Then
                clsDossier.Hkomplexe.result = "Hess. Biotopkartierung  Komplexe ---------------------------------" & Environment.NewLine &
                                 clsDossier.Hkomplexe.result
                clsDossier.Hkomplexe.kurz = clsString.removeLeadingChar(clsDossier.Hkomplexe.kurz, ",")
                tbHkomplexe.Text = tbHkomplexe.Text & ": " & clsDossier.Hkomplexe.kurz
                tbHkomplexe.ToolTip = tbHkomplexe.Text
                tbHkomplexe.Background = Brushes.LightGreen
                'btnHbiotope.Visibility = Visibility.Visible
                'btnHbiotope.IsEnabled = True
                btnHkomplexetext.IsEnabled = True
                'btnHbiotope.IsEnabled = True
                protokollKompakt = protokollKompakt & "Hess.BiotopKomplexe: " & clsDossier.Hkomplexe.kurz & trenn
            Else

                If clsDossier.Hkomplexe.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Hkomplexe: FEHLER bei Analyse von Tab.: " & clsDossier.Hkomplexe.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Hkomplexe: FEHLER bei Analyse von Tab. " & clsDossier.Hkomplexe.schematabelle)
                    tbHkomplexe.FontSize = 10
                    spHkomplexe.Visibility = Visibility.Collapsed
                    btnHkomplexetext.Visibility = Visibility.Collapsed
                Else
                    tbHkomplexe.Text = "Keine Objekte der Hess. Biotopkartierung - Komplexe gefunden"
                    tbHkomplexe.FontSize = 10
                    btnHkomplexetext.Visibility = Visibility.Collapsed
                    clsDossier.Hkomplexe.result = "Hess- Biotopkartierung Komplexe - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Hess. Komplexe gefunden"
                End If

            End If
            zwischenInfo(Environment.NewLine & clsDossier.Hkomplexe.result & Environment.NewLine)
            l("HKomplexe ---------------------- ende")
        Catch ex As Exception
            l("Fehler in HKomplexe: " & ex.ToString())
        End Try
    End Sub
    Private Sub HbiotopeAnalyse()
        Try
            l("Hbiotope ---------------------- anfang")
            clsDossier.Hbiotope.schematabelle = "public.dossier_hbiotope"
            If clsHbiotope.getInfo4point(m.UTMpt, clsDossier.Hbiotope, clsDossier.Hbiotope.strerror) Then
                clsDossier.Hbiotope.result = "Hess. Biotopkartierung  Biotope ---------------------------------" & Environment.NewLine &
                                 clsDossier.Hbiotope.result
                clsDossier.Hbiotope.kurz = clsString.removeLeadingChar(clsDossier.Hbiotope.kurz, ",")
                tbHbiotope.Text = tbHbiotope.Text & ": " & clsDossier.Hbiotope.kurz
                tbHbiotope.ToolTip = tbHbiotope.Text
                tbHbiotope.Background = Brushes.LightGreen
                'btnHbiotope.Visibility = Visibility.Visible
                'btnHbiotope.IsEnabled = True
                btnHbiotopetext.IsEnabled = True
                'btnHbiotope.IsEnabled = True
                protokollKompakt = protokollKompakt & "Hess. Biotope: " & clsDossier.Hbiotope.kurz & trenn
            Else
                If clsDossier.Hbiotope.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Hbiotope: FEHLER bei Analyse von Tab.: " & clsDossier.Hbiotope.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Hbiotope: FEHLER bei Analyse von Tab. " & clsDossier.Hbiotope.schematabelle)
                    tbHbiotope.FontSize = 10
                    btnHbiotopetext.Visibility = Visibility.Collapsed
                Else
                    tbHbiotope.Text = "Keine Objekte der Hess. Biotopkartierung (Biotope) gefunden"
                    tbHbiotope.FontSize = 10
                    btnHbiotopetext.Visibility = Visibility.Collapsed
                    clsDossier.Hbiotope.result = "Hbiotope - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Hess. Biotope gefunden"
                End If

            End If
            zwischenInfo(Environment.NewLine & clsDossier.Hbiotope.result & Environment.NewLine)
            l("Hbiotope ---------------------- ende")
        Catch ex As Exception
            l("Fehler in Hbiotope: " & ex.ToString())
        End Try
    End Sub

    Private Sub FoerderFlaechenAnalyse()
        Try
            l(" Förderflächen ---------------------- anfang")
            clsDossier.Foerder.schematabelle = "public.dossier_foerder"
            If clsFoerder.getInfo4point(m.UTMpt, clsDossier.Foerder, clsDossier.Foerder.strerror) Then
                clsDossier.Foerder.result = "Förderflächen Krof ---------------------------------" & Environment.NewLine &
                                 clsDossier.Foerder.result
                tbFoerder.Text = "Förderfläche: " & clsDossier.Foerder.kurz
                tbFoerder.ToolTip = clsDossier.Foerder.result
                tbFoerder.Background = Brushes.LightGreen
                'btnFoerder.Visibility = Visibility.Visible
                btnFoerdertext.IsEnabled = True
                'btnFoerder.IsEnabled = True
                protokollKompakt = protokollKompakt & "Förderflächen: " & clsDossier.Foerder.kurz & trenn
            Else
                If clsDossier.Foerder.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Foerder: FEHLER bei Analyse von Tab.: " & clsDossier.Foerder.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Foerder: FEHLER bei Analyse von Tab. " & clsDossier.Foerder.schematabelle)
                    tbFoerder.FontSize = 10
                    btnFoerdertext.Visibility = Visibility.Collapsed
                Else
                    tbFoerder.Text = "Kein Förderflächenobjekt"
                    tbFoerder.FontSize = 10
                    btnFoerdertext.Visibility = Visibility.Collapsed
                    clsDossier.Foerder.result = "Förderflächen  - Feststellung ---------------------------------" & Environment.NewLine &
                        "Kein Förderflächen-Objekt"
                End If

            End If
            zwischenInfo(Environment.NewLine & clsDossier.Foerder.result & Environment.NewLine)
            l(" Förderflächen Krof ---------------------- ende")
        Catch ex As Exception
            l("Fehler in Förderflächen Krof: " & ex.ToString())
        End Try
    End Sub

    Private Sub UeberschemmungsKROFAnalyse()
        Try
            l(" Überschwemmungsgebiet Krof ---------------------- anfang")
            clsDossier.UEBKROF.schematabelle = "public.dossier_uebkrof"
            If clsUebKrof.getInfo4point(m.UTMpt, clsDossier.UEBKROF, clsDossier.UEBKROF.strerror) Then
                clsDossier.UEBKROF.result = "Überschwemmungsgebiet Krof ---------------------------------" & Environment.NewLine &
                                clsDossier.UEBKROF.result
                tbUEBKROF.Text = clsDossier.UEBKROF.kurz & ": " & clsDossier.UEBKROF.kurz
                tbUEBKROF.ToolTip = clsDossier.UEBKROF.result
                tbUEBKROF.Background = Brushes.LightGreen
                btnUEBKROF.Visibility = Visibility.Visible
                btnUEBKROFtext.IsEnabled = True
                btnUEBKROF.IsEnabled = True
                protokollKompakt = protokollKompakt & "Überschwemmungsfl. KrOF: " & clsDossier.UEBKROF.kurz & trenn
            Else
                If clsDossier.UEBKROF.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "UEBKROF: FEHLER bei Analyse von Tab.: " & clsDossier.UEBKROF.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "UEBKROF: FEHLER bei Analyse von Tab. " & clsDossier.UEBKROF.schematabelle)
                    tbUEBKROF.FontSize = 10
                    btnUEBKROF.Visibility = Visibility.Collapsed
                    btnUEBKROFtext.Visibility = Visibility.Collapsed
                Else
                    tbUEBKROF.Text = "Kein Überschwemmungsgebiet Krof"
                    tbUEBKROF.FontSize = 10
                    btnUEBKROF.Visibility = Visibility.Collapsed
                    btnUEBKROFtext.Visibility = Visibility.Collapsed
                    clsDossier.UEBKROF.result = "Überschwemmungsgebiet Krof - Feststellung ---------------------------------" & Environment.NewLine &
                        "Kein Überschwemmungsgebiet Krof"
                End If

            End If
            zwischenInfo(Environment.NewLine & clsDossier.UEBKROF.result & Environment.NewLine)
            l(" Überschwemmungsgebiet Krof ---------------------- ende")
        Catch ex As Exception
            l("Fehler in Überschwemmungsgebiet Krof: " & ex.ToString())
        End Try
    End Sub

    Private Sub UeberschemmungsAnalyseHLFU()
        Try
            l(" UEBAnalyse ---------------------- anfang")
            Dim bbox As String
            bbox = clsWMS.calcVollstBbox(CInt(m.UTMpt.X) & "," & CInt(m.UTMpt.Y))
            Dim url, wmslayers, wmsquery_layers As String
            wmslayers = "Ueberschwemmungsgebiete_HQ100_nach_HWG"
            wmsquery_layers = "Ueberschwemmungsgebiete_HQ100_nach_HWG"
            Dim hinweis, resultHQ100, resultHQ200 As String
            url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 248, CInt(m.MAPPINGhoehe), CInt(m.MAPPINGbreite),
                                                  CInt(m.MAPPINGscreenX), CInt(m.MAPPINGscreenY),
                                                  "text/plain",
                                                  wmslayers, wmsquery_layers, clsDossier.Ueb.strerror)
            resultHQ100 = meineHttpNet.meinHttpJob(m.ProxyString, url, hinweis, Text.Encoding.UTF8, 5000)
            '========================================
            wmslayers = "Hochwasser_mit_niedriger_Wahrscheinlichkeit"
            wmsquery_layers = "Hochwasser_mit_niedriger_Wahrscheinlichkeit"
            url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 5389, CInt(m.MAPPINGhoehe), CInt(m.MAPPINGbreite),
                                                      CInt(m.MAPPINGscreenX), CInt(m.MAPPINGscreenY), "text/plain",
                                                  wmslayers, wmsquery_layers, clsDossier.Ueb.strerror)
            resultHQ200 = meineHttpNet.meinHttpJob(m.ProxyString, url, hinweis, Text.Encoding.UTF8, 5000)

            If resultHQ100.IsNothingOrEmpty Then
                If resultHQ200.IsNothingOrEmpty Then
                    If clsDossier.Ueb.strerror.ToLower.Contains("fehler") Then
                        protokollKompakt = protokollKompakt & "Ueb: FEHLER bei Analyse von Tab.: " & clsDossier.Ueb.schematabelle & trenn
                        zwischenInfo(Environment.NewLine & "Ueb: FEHLER bei Analyse von Tab. " & clsDossier.Ueb.schematabelle)
                    Else
                        clsDossier.Ueb.result = "Kein Überschwemmungsgebiet HQ100 HLFU"
                        btnUEBttext.Visibility = Visibility.Collapsed
                        btnUEB.Visibility = Visibility.Hidden
                        tbUEB.Text = clsDossier.Ueb.result
                        tbUEB.FontSize = 10
                        btnUEB.Visibility = Visibility.Hidden
                        btnUEBttext.Visibility = Visibility.Collapsed
                    End If
                Else
                    clsDossier.Ueb.result = "Ist Überschwemmungsgebiet HQ>200 HLFU (HQ Extrem Überflutungsfläche (niedrige Wahrscheinlichkeit nach § 74 WHG)"
                    btnUEBttext.Visibility = Visibility.Visible
                    btnUEBttext.IsEnabled = True
                    tbUEB.Text = "Ist HQ>200 !!!"
                    tbUEB.Background = Brushes.LightGreen
                    'tbUEB.FontSize = 10
                    btnUEB.Visibility = Visibility.Visible
                    btnUEBttext.Visibility = Visibility.Visible
                    protokollKompakt = protokollKompakt & "Überschwemmungsfl. 200HQ: " & clsDossier.Ueb.kurz & trenn
                End If

            Else
                clsDossier.Ueb.result = "ist als Überschwemmungsgebiet_HQ100_nach_HWG festgesetzt."
                If resultHQ100.Contains("festgesetzt;") Then
                    clsDossier.Ueb.result = "ist als Überschwemmungsgebiet_HQ100_nach_HWG festgesetzt."
                    tbUEB.Background = Brushes.LightGreen
                    btnUEBttext.IsEnabled = True
                Else
                    clsDossier.Ueb.result = "ist als Überschwemmungsgebiete_HQ100_nach_HWG festgesetzt."
                    tbUEB.Background = Brushes.LightGreen
                    btnUEBttext.IsEnabled = True
                End If
                protokollKompakt = protokollKompakt & "Überschwemmungsfl. 100HQ: " & clsDossier.Ueb.kurz & trenn
            End If
            clsDossier.Ueb.result = "Überschwemmungsgebiete ?  ---------------------------------" & Environment.NewLine &
                                 clsDossier.Ueb.result
            zwischenInfo(Environment.NewLine & clsDossier.Ueb.result)
            l(" UEB ---------------------- ende")
        Catch ex As Exception
            l("Fehler in UEBAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub baulastenAnalyse()
        Try
            l("baulastenAnalyse ---------------------- anfang")
            clsDossier.Baulasten.schematabelle = "dossier_baulasten"
            If clsBaulasten.getInfo4point(m.UTMpt, clsDossier.Baulasten, clsDossier.Baulasten.strerror) Then
                clsDossier.Baulasten.result = "Baulasten  ---------------------------------" & Environment.NewLine &
                                 clsDossier.Baulasten.result
                clsDossier.Baulasten.kurz = clsString.removeLeadingChar(clsDossier.Baulasten.kurz, ",")
                tbBaulasten.Text = tbBaulasten.Text & ": " & clsDossier.Baulasten.kurz
                tbBaulasten.ToolTip = tbBaulasten.Text
                tbBaulasten.Background = Brushes.LightGreen
                btnBaulasten.Visibility = Visibility.Visible
                btnBaulasten.IsEnabled = True
                btnBaulastentext.IsEnabled = True
                btnBaulasten.IsEnabled = True
                protokollKompakt = protokollKompakt & "Baulasten: " & clsDossier.Baulasten.kurz & trenn
            Else
                If clsDossier.Baulasten.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Baulasten: FEHLER bei Analyse von Tab.: " & clsDossier.Baulasten.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Baulasten: FEHLER bei Analyse von Tab. " & clsDossier.Baulasten.schematabelle)
                    tbBaulasten.FontSize = 10
                    btnBaulasten.Visibility = Visibility.Collapsed
                    btnBaulastentext.Visibility = Visibility.Collapsed
                Else
                    tbBaulasten.Text = "Keine Baulasten gefunden"
                    tbBaulasten.FontSize = 10
                    btnBaulasten.Visibility = Visibility.Collapsed
                    btnBaulastentext.Visibility = Visibility.Collapsed
                    clsDossier.Baulasten.result = "Baulasten - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Baulasten gefunden"
                End If

            End If
            zwischenInfo(Environment.NewLine & clsDossier.Baulasten.result & Environment.NewLine)
            l("BaulastenAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in BaulastenAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub IllegaleALTAnalyse()
        Try
            l(" IllegaleALTAnalyse ---------------------- anfang")
            clsDossier.IllegaleAlt.schematabelle = "dossier_illegalealt"
            If clsIllegaleALT.getIllegaleALTInfo4point(m.UTMpt, clsDossier.IllegaleAlt, clsDossier.IllegaleAlt.strerror) Then
                clsDossier.IllegaleAlt.result = "Illegale Bauten ALT  ---------------------------------" & Environment.NewLine &
                                 clsDossier.IllegaleAlt.result
                clsDossier.IllegaleAlt.kurz = clsString.removeLeadingChar(clsDossier.IllegaleAlt.kurz, ",")
                tbIllegaleALT.Text = tbIllegaleALT.Text & ": " & clsDossier.IllegaleAlt.kurz
                tbIllegaleALT.ToolTip = tbIllegaleALT.Text
                tbIllegaleALT.Background = Brushes.LightGreen
                'btnIllegaleALT.Visibility = Visibility.h
                btnIllegaleALTtext.IsEnabled = True
                btnIllegaleALT.IsEnabled = True
                protokollKompakt = protokollKompakt & "Illegale Bauten (alt): " & clsDossier.IllegaleAlt.kurz & trenn
            Else
                If clsDossier.IllegaleAlt.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "IllegaleAlt: FEHLER bei Analyse von Tab.: " & clsDossier.IllegaleAlt.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "IllegaleAlt: FEHLER bei Analyse von Tab. " & clsDossier.IllegaleAlt.schematabelle)
                    spIllegaleALT.Visibility = Visibility.Collapsed
                Else
                    tbIllegaleALT.Text = "Keine Objekte aus Illegale bauten bis 2004"
                    tbIllegaleALT.FontSize = 10
                    btnIllegaleALT.Visibility = Visibility.Collapsed
                    btnIllegaleALTtext.Visibility = Visibility.Collapsed
                    clsDossier.IllegaleAlt.result = "IllegaleALT - Feststellung ---------------------------------" & Environment.NewLine &
                        "Kein IllegaleALT"
                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.IllegaleAlt.result & Environment.NewLine)
            l(" IllegaleALTAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in IllegaleALTAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub IllegaleAnalyse()
        Try
            l(" IllegaleAnalyse ---------------------- anfang")
            clsDossier.Illegale.schematabelle = "paradigma_userdata.sg_3307"
            If clsIllegaleNeu.getIllegaleInfo4point(m.UTMpt, clsDossier.Illegale, clsDossier.Illegale.strerror) Then
                clsDossier.Illegale.result = "Illegale Bauten neu  ---------------------------------" & Environment.NewLine &
                                 clsDossier.Illegale.result
                clsDossier.Illegale.kurz = clsString.removeLeadingChar(clsDossier.Illegale.kurz, ",")
                tbIllegale.Text = tbIllegale.Text & ": " & clsDossier.Illegale.kurz
                tbIllegale.ToolTip = tbIllegale.Text
                tbIllegale.Background = Brushes.LightGreen
                btnIllegale.Visibility = Visibility.Visible
                btnIllegaletext.IsEnabled = True
                btnIllegale.IsEnabled = True
                protokollKompakt = protokollKompakt & "IllegaleBauten (neu): " & clsDossier.Illegale.kurz & trenn
            Else
                If clsDossier.Illegale.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Illegale: FEHLER bei Analyse von Tab.: " & clsDossier.Illegale.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Illegale: FEHLER bei Analyse von Tab. " & clsDossier.Illegale.schematabelle)
                    spIllegale.Visibility = Visibility.Collapsed
                Else
                    tbIllegale.Text = "Keine Objekte in akt. IllegaleBauten gef."
                    tbIllegale.FontSize = 10
                    btnIllegale.Visibility = Visibility.Collapsed
                    btnIllegaletext.Visibility = Visibility.Collapsed
                    clsDossier.Illegale.result = "Illegale - Feststellung ---------------------------------" & Environment.NewLine & "Kein Illegale"

                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.Illegale.result & Environment.NewLine)
            l(" IllegaleAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in IllegaleAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub borisAnalyse()
        Try
            l(" borisAnalyse ---------------------- anfang")
            Dim bbox As String
            bbox = clsWMS.calcVollstBbox(CInt(m.UTMpt.X) & "," & CInt(m.UTMpt.Y))
            Dim url As String
            'boris 379
            'test 1000
            url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 379, CInt(m.MAPPINGhoehe), CInt(m.MAPPINGbreite),
                                                      CInt(m.MAPPINGscreenX),
                                                  CInt(m.MAPPINGscreenY), "text/plain", "",
                                                  "",
                                                  clsDossier.Boris.strerror)
            Dim hinweis As String = ""
            'Dim result As String = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, myglobalz.enc, 5000)
            Dim result As String = meineHttpNet.meinHttpJob(m.ProxyString, url, hinweis, Text.Encoding.UTF8, 5000)
            Dim a() As String
            clsDossier.Boris.result = ""
            If result.IsNothingOrEmpty Then
                clsDossier.Boris.result = "Fehler: WMS-Service 'Boris' des Landes Hessen ist nicht verfügbar!"
                protokollKompakt = protokollKompakt & "Boris: " & clsDossier.Boris.result & trenn
            Else
                a = result.Split(CType(vbCrLf, Char()))
                For Each text As String In a
                    If text.Contains("BRW = '") Then
                        clsDossier.Boris.result = text.Replace("BRW = '", "").Trim
                        clsDossier.Boris.result = clsDossier.Boris.result.Replace("'", "")
                        tbboris.Background = Brushes.LightGreen
                        btnboristtext.IsEnabled = True
                        protokollKompakt = protokollKompakt & "Boris: " & clsDossier.Boris.result & trenn
                        Exit For
                    End If
                Next
                clsDossier.Boris.result = "Bodenrichtwert ?  ---------------------------------" & Environment.NewLine &
                                      clsDossier.Boris.result
            End If

            zwischenInfo(Environment.NewLine & clsDossier.Boris.result)
            l(" borisAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in borisAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub altlastAnalyse()
        l(" altlastAnalyse ---------------------- anfang")
        Try
            clsDossier.Altlast.schematabelle = "dossier_altflaeche"
            If clsAltlast.getAltlastInfo4point(m.UTMpt, clsDossier.Altlast, clsDossier.Altlast.strerror) Then
                clsDossier.Altlast.result = "Altlast-Hinweisfläche ?  ---------------------------------" & Environment.NewLine &
                                 clsDossier.Altlast.result
                clsDossier.Altlast.kurz = clsString.removeLeadingChar(clsDossier.Altlast.kurz, ",")
                tbaltlast.Text = tbaltlast.Text & ": " & clsDossier.Altlast.kurz
                tbaltlast.ToolTip = tbaltlast.Text
                tbaltlast.Background = Brushes.LightGreen
                btnaltlast.Visibility = Visibility.Hidden
                btnaltlasttext.IsEnabled = True
                btnaltlast.IsEnabled = False
                protokollKompakt = protokollKompakt & "Altlast: " & clsDossier.Altlast.kurz & trenn
            Else
                If clsDossier.Altlast.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Altlast: FEHLER bei Analyse von Tab.: " & clsDossier.Altlast.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Altlast: FEHLER bei Analyse von Tab. " & clsDossier.Altlast.schematabelle)
                    tbaltlast.FontSize = 10
                    btnaltlast.Visibility = Visibility.Collapsed
                    btnaltlasttext.Visibility = Visibility.Collapsed
                Else
                    tbaltlast.Text = "Kein Altlast"
                    tbaltlast.FontSize = 10
                    btnaltlast.Visibility = Visibility.Collapsed
                    btnaltlasttext.Visibility = Visibility.Collapsed
                    clsDossier.Altlast.result = "Altlast - Feststellung ---------------------------------" & Environment.NewLine & "Kein Altlast"

                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.Altlast.result & Environment.NewLine)
            l(" altlastAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in altlastAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub showTitleUndFlurstueck(schematabelle As String, ByRef strError As String)
        Dim fs, albflaeche, fehler As String
        Dim flaechenText As String
        Try
            l(" showTitleUndFlurstueck ---------------------- anfang")
            If m.MAPPINGfs.IsNothingOrEmpty Then
                If clsEigentuemerAnalyse.getFS4coordinates(m.UTMpt, fs, schematabelle, strError,
                                                           m.aktFST.normflst.weistauf,
                                                           m.aktFST.normflst.zeigtauf,
                                                           albflaeche) Then
                    m.aktFST.clear()
                    m.aktFST.normflst.FS = fs
                    m.aktFST.normflst.splitFS(fs)
                End If
            Else
                m.aktFST.clear()
                m.aktFST.normflst.FS = m.MAPPINGfs
                m.aktFST.normflst.splitFS(m.aktFST.normflst.FS)
                flaechenText = clsFSTtools.getFlaecheZuFlurstueck(m.aktFST, strError)
            End If
            l(" m.aktFST.normflst.FS: " & m.aktFST.normflst.FS)
            calcNASLageString(m.aktFST.normflst.weistauf, m.aktFST.normflst.zeigtauf, fehler, m.NASlage)
            If m.flurstuecksModus Then
                toptitel = toptitel & "Dossier für Flurstück: " & m.aktFST.normflst.toShortstring(",") &
                    flaechenText &
                    ", " & m.NASlage.strAusgabe & ", (" & CInt(m.UTMpt.X) & ", " & CInt(m.UTMpt.Y) & ") "
                tbProtokolltitel.Text = "Protokoll für Flurstück: " & m.aktFST.normflst.toShortstring(",")
            Else
                toptitel = toptitel & "Dossier für UTM Koordinate: " & CInt(m.UTMpt.X) & ", " & CInt(m.UTMpt.Y) & "." & Environment.NewLine
                toptitel = toptitel & "Punkt liegt auf Flurstück: " & m.aktFST.normflst.toShortstring(",") &
                     flaechenText &
                   ", " & m.NASlage.strAusgabe
                tbProtokolltitel.Text = "Protokoll für UTM-Koord.: " & CInt(m.UTMpt.X) & ", " & CInt(m.UTMpt.Y)
            End If
            protokollKompakt = protokollKompakt & toptitel & trenn
            zwischenInfo(toptitel)
            l(" showTitleUndFlurstueck ---------------------- ende")
        Catch ex As Exception
            l("Fehler in showTitleUndFlurstueck: " & ex.ToString())
        End Try
    End Sub

    Private Sub wsgAnalyse()
        Try
            l(" wsgAnalyse ---------------------- anfang")
            clsDossier.WSG.schematabelle = "public.dossier_wsg"
            If clsWSG.getWSGInfo4point(m.UTMpt, clsDossier.WSG, clsDossier.WSG.strerror) Then
                clsDossier.WSG.result = "WSG - Feststellung ---------------------------------" & Environment.NewLine &
                                 clsDossier.WSG.result
                clsDossier.WSG.kurz = clsString.removeLeadingChar(clsDossier.WSG.kurz, ",")
                tbWSG.Text = tbWSG.Text & ": " & clsDossier.WSG.kurz
                tbWSG.ToolTip = tbWSG.Text
                tbWSG.Background = Brushes.LightGreen
                btnWSG.Visibility = Visibility.Visible
                btnWSGtext.IsEnabled = True
                'btnWSGAnlage.IsEnabled = True
                btnWSG.IsEnabled = True
                protokollKompakt = protokollKompakt & "WSG: " & clsDossier.WSG.kurz & trenn
            Else
                If clsDossier.WSG.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "WSG: FEHLER bei Analyse von Tab.: " & clsDossier.WSG.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "WSG: FEHLER bei Analyse von Tab. " & clsDossier.WSG.schematabelle)
                    tbWSG.FontSize = 10
                    btnWSG.Visibility = Visibility.Collapsed
                    btnWSGtext.Visibility = Visibility.Collapsed
                Else
                    tbWSG.Text = "Kein WSG"
                    tbWSG.FontSize = 10
                    btnWSG.Visibility = Visibility.Collapsed
                    btnWSGtext.Visibility = Visibility.Collapsed
                    clsDossier.WSG.result = "WSG - Feststellung ---------------------------------" & Environment.NewLine &
                        "Kein WSG"
                End If

            End If
            zwischenInfo(Environment.NewLine & clsDossier.WSG.result & Environment.NewLine)
            l(" wsgAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in wsgAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub ffhAnalyse()
        Try
            l(" ffhAnalyse ---------------------- anfang")
            clsDossier.FFH.schematabelle = "public.dossier_ffhgebiet"
            If clsFFH.getFFHInfo4point(m.UTMpt, clsDossier.FFH, clsDossier.FFH.strerror) Then
                clsDossier.FFH.result = "FFH - Feststellung ---------------------------------" & Environment.NewLine &
                                 clsDossier.FFH.result
                tbFFH.Text = tbFFH.Text & ": " & clsDossier.FFH.kurz
                tbFFH.ToolTip = tbFFH.Text
                tbFFH.Background = Brushes.LightGreen
                btnFFH.Visibility = Visibility.Visible
                btnFFHtext.IsEnabled = True
                btnFFHAnlage.IsEnabled = True
                btnFFH.IsEnabled = True
                protokollKompakt = protokollKompakt & "FFH: " & clsDossier.FFH.kurz & trenn
            Else
                If clsDossier.FFH.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "XXX: FEHLER bei Analyse von Tab.: " & clsDossier.FFH.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "XXX: FEHLER bei Analyse von Tab. " & clsDossier.FFH.schematabelle)
                    tbFFH.FontSize = 10
                    btnFFH.Visibility = Visibility.Collapsed
                    btnFFHtext.Visibility = Visibility.Collapsed
                    btnFFHAnlage.Visibility = Visibility.Collapsed
                Else
                    tbFFH.Text = "Kein FFH"
                    tbFFH.FontSize = 10
                    btnFFH.Visibility = Visibility.Collapsed
                    btnFFHtext.Visibility = Visibility.Collapsed
                    btnFFHAnlage.Visibility = Visibility.Collapsed
                    clsDossier.FFH.result = "FFH - Feststellung ---------------------------------" & Environment.NewLine & "Kein FFH"

                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.FFH.result & Environment.NewLine)
            l(" ffhAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in ffhAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub lsgAnalyse()
        Try
            l(" lsgAnalyse ---------------------- anfang")
            clsDossier.LSG.schematabelle = "public.dossier_lsg"
            If clsLSG.getLSGInfo4point(m.UTMpt, clsDossier.LSG, clsDossier.LSG.strerror) Then
                clsDossier.LSG.result = "LSG - Feststellung ---------------------------------" & Environment.NewLine &
                                   clsDossier.LSG.result
                tbLSG.Text = tbLSG.Text & ": " & clsDossier.LSG.kurz
                tbLSG.ToolTip = tbLSG.Text
                tbLSG.Background = Brushes.LightGreen
                btnLSG.Visibility = Visibility.Visible
                btnLSGtext.IsEnabled = True
                btnLSG.IsEnabled = True
                protokollKompakt = protokollKompakt & "LSG: " & clsDossier.LSG.kurz & trenn
            Else
                If clsDossier.LSG.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "LSG: FEHLER bei Analyse von Tab.: " & clsDossier.LSG.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "LSG: FEHLER bei Analyse von Tab. " & clsDossier.LSG.schematabelle)

                    tbLSG.FontSize = 10
                    btnLSG.Visibility = Visibility.Collapsed
                    btnLSGtext.Visibility = Visibility.Collapsed
                Else
                    tbLSG.Text = "Kein LSG"
                    tbLSG.FontSize = 10
                    btnLSG.Visibility = Visibility.Collapsed
                    btnLSGtext.Visibility = Visibility.Collapsed
                    clsDossier.LSG.result = "LSG - Feststellung ---------------------------------" & Environment.NewLine & "Kein LSG"

                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.LSG.result & Environment.NewLine)
            l(" LsgAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in LsgAnalyse: " & ex.ToString())
        End Try
    End Sub
    Private Sub nsgAnalyse()
        Try
            l(" nsgAnalyse ---------------------- anfang")
            clsDossier.NSG.schematabelle = "public.dossier_nsgglb"
            If clsNSG.getNSGInfo4point(m.UTMpt, clsDossier.NSG, clsDossier.NSG.strerror) Then
                clsDossier.NSG.result = "NSG - Feststellung ---------------------------------" & Environment.NewLine &
                                   clsDossier.NSG.result
                tbNSG.Text = tbNSG.Text & ": " & clsDossier.NSG.kurz
                tbNSG.ToolTip = tbNSG.Text
                tbNSG.Background = Brushes.LightGreen
                btnNSG.Visibility = Visibility.Visible
                btnNSGtext.IsEnabled = True
                btnNSG.IsEnabled = True
                protokollKompakt = protokollKompakt & "NSG: " & clsDossier.NSG.kurz & trenn
            Else
                If clsDossier.NSG.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "NSG: FEHLER bei Analyse von Tab.: " & clsDossier.NSG.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "NSG: FEHLER bei Analyse von Tab. " & clsDossier.NSG.schematabelle)
                    tbNSG.FontSize = 10
                    btnNSG.Visibility = Visibility.Collapsed
                    btnNSGtext.Visibility = Visibility.Collapsed
                Else
                    tbNSG.Text = "Kein NSG"
                    tbNSG.FontSize = 10
                    btnNSG.Visibility = Visibility.Collapsed
                    btnNSGtext.Visibility = Visibility.Collapsed
                    clsDossier.NSG.result = "NSG - Feststellung ---------------------------------" & Environment.NewLine & "Kein NSG"

                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.NSG.result & Environment.NewLine)
            l(" nsgAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in nsgAnalyse: " & ex.ToString())
        End Try
    End Sub
    Private Sub setTitel(winpt As myPoint)
        'If clsStartup.flurstuecksModus Then

        'Else
        '    = "Protokoll für Flurstück: " & clsStartup.aktFST.normflst.toShortstring(",")
        'End If
        Title = Title & " " & CInt(winpt.X) & ", " & CInt(winpt.Y) & " " & " [v." & m.dossierVersion & "] " &
            " Radius: " & m.MAPPINGradiusinmeter & " [m], " &
        m.GisUser.username & " (" & m.GisUser.ADgruppenname & ", " & m.GisUser.favogruppekurz & ") "
    End Sub

    Private Sub btnbplanaufruf2_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If clsDossier.Bplan.link.IsNothingOrEmpty Then
            MsgBox("Kein Bplan zur Adresse gefunden.")
        Else
            Process.Start(clsDossier.Bplan.link)
        End If
    End Sub

    Sub l(v As String)
        nachricht(v)
    End Sub
    Sub l(v As String, excep As Exception)
        nachricht(v & Environment.NewLine & excep.ToString & Environment.NewLine)
    End Sub

    Private Sub MainWindowDossier_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        savePosition()
    End Sub
    Private Sub savePosition()
        Try
            m.userIniProfile.WertSchreiben("diverse", "dossierformpositiontop", CType(Me.Top, String))
            m.userIniProfile.WertSchreiben("diverse", "dossierformpositionleft", CType(Me.Left, String))
        Catch ex As Exception
            l("fehler in saveposition  windb" & ex.ToString)
        End Try
    End Sub
    Private Sub btnDossierKonfig_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim konfig As New winDossierSet
        konfig.Show()
        l("fehler kein fehler dossier2 aufruf " & Environment.UserName)
        Close()
    End Sub
    Private Sub btnEigentuemerPDF_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True

        Dim EigentuemerPDF As String = clsTools.erzeugeUndOeffneEigentuemerPDF(clsDossier.Eigentuemer.kurz, m.aktFST, clsDossier.Eigentuemer.strerror)
        OpenDokument(EigentuemerPDF)
    End Sub
    Private Sub btnEigentuemertext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Eigentuemer.result)
    End Sub
    Private Sub zwischenInfo(text As String)
        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
        tbInfo.Text = tbInfo.Text & text & Environment.NewLine
        tbInfo.Focus()
        tbInfo.CaretIndex = tbInfo.Text.Length
        'tbInfo.ScrollToEnd()
        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    End Sub

    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Close()
    End Sub

    Private Sub cbkompaktansicht_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If cbkompaktansicht.IsChecked Then
            komplettansicht = False
            kompaktieren()
            m.userIniProfile.WertSchreiben("gisanalyse", "komplettansicht", "0")
            tbInfo.Text = protokollKompakt
        Else
            komplettansicht = True
            kompaktieren()
            m.userIniProfile.WertSchreiben("gisanalyse", "komplettansicht", "1")
            tbInfo.Text = protokollGesamt
        End If
    End Sub

    Private Sub kompaktieren()
        Try
            l(" kompaktieren ---------------------- anfang")
            If Not komplettansicht And tbAltstadtsatzung.FontSize = 10 Then
                spAltstadtsatzung.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Altstadtsatzung.showControl Then
                    spAltstadtsatzung.Visibility = Visibility.Visible
                End If
            End If
            If Not komplettansicht And tbStandorttypisierung.FontSize = 10 Then
                spstandorttypisierung.Visibility = Visibility.Collapsed
            Else
                If clsDossier.standorttypisierung.showControl Then
                    spstandorttypisierung.Visibility = Visibility.Visible
                End If
            End If
            If Not komplettansicht And tbwsgHNUGwms.FontSize = 10 Then
                spwsgHNUGwms.Visibility = Visibility.Collapsed
            Else
                If clsDossier.wsgHNUGwms.showControl Then
                    spwsgHNUGwms.Visibility = Visibility.Visible
                End If
            End If
            If Not komplettansicht And tbSchwalben.FontSize = 10 Then
                spSchwalben.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Schwalben.showControl Then
                    spSchwalben.Visibility = Visibility.Visible
                End If
            End If
            If Not komplettansicht And tbKompensation.FontSize = 10 Then
                spKompensation.Visibility = Visibility.Collapsed
            Else
                If clsDossier.kompensation.showControl Then
                    spKompensation.Visibility = Visibility.Visible
                End If
            End If
            If Not komplettansicht And tbparadigma.FontSize = 10 Then
                spparadigma.Visibility = Visibility.Collapsed
            Else
                If clsDossier.paradigmavorgang.showControl Then
                    spparadigma.Visibility = Visibility.Visible
                End If
            End If
            If Not komplettansicht And tbaltis16.FontSize = 10 Then
                spaltis16.Visibility = Visibility.Collapsed
            Else
                If clsDossier.altis16.showControl Then
                    spaltis16.Visibility = Visibility.Visible
                End If
            End If
            If Not komplettansicht And tbND.FontSize = 10 Then
                spND.Visibility = Visibility.Collapsed
            Else
                If clsDossier.ND.showControl Then
                    spND.Visibility = Visibility.Visible
                End If
            End If


            If Not komplettansicht And tbWSG.FontSize = 10 Then
                spWSG.Visibility = Visibility.Collapsed
            Else
                If clsDossier.WSG.showControl Then
                    spWSG.Visibility = Visibility.Visible
                End If
            End If


            If Not komplettansicht And tbEigentuemer.FontSize = 10 Then
                spEigentuemer.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Eigentuemer.showControl Then
                    spEigentuemer.Visibility = Visibility.Visible
                End If
            End If
            If Not komplettansicht And tbbplangueltig.FontSize = 10 Then
                spbplan.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Bplan.showControl Then
                    spbplan.Visibility = Visibility.Visible
                End If
            End If

            If Not komplettansicht And tbaltlast.FontSize = 10 Then
                spaltlast.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Altlast.showControl Then
                    spaltlast.Visibility = Visibility.Visible
                End If
            End If

            If Not komplettansicht And tbAmph.FontSize = 10 Then
                spAmph.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Amphibien.showControl Then spAmph.Visibility = Visibility.Visible
            End If
            If Not komplettansicht And tbBaulasten.FontSize = 10 Then
                spBaulasten.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Baulasten.showControl Then spBaulasten.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbboris.FontSize = 10 Then
                spboris.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Boris.showControl Then spboris.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbBSE.FontSize = 10 Then
                spBSE.Visibility = Visibility.Collapsed
            Else
                If clsDossier.BSE.showControl Then spBSE.Visibility = Visibility.Visible
            End If
            If Not komplettansicht And tbFFH.FontSize = 10 Then
                spFFH.Visibility = Visibility.Collapsed
            Else
                If clsDossier.FFH.showControl Then spFFH.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbFoerder.FontSize = 10 Then
                spFoerder.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Foerder.showControl Then spFoerder.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbHbiotope.FontSize = 10 Then
                spHbiotope.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Hbiotope.showControl Then spHbiotope.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbHkomplexe.FontSize = 10 Then
                spHkomplexe.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Hkomplexe.showControl Then spHkomplexe.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbIllegale.FontSize = 10 Then
                spIllegale.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Illegale.showControl Then spIllegale.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbIllegaleALT.FontSize = 10 Then
                spIllegaleALT.Visibility = Visibility.Collapsed
            Else
                If clsDossier.IllegaleAlt.showControl Then spIllegaleALT.Visibility = Visibility.Visible
            End If
            If Not komplettansicht And tbKehrbezirk.FontSize = 10 Then
                spKehrbezirk.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Kehr.showControl Then spKehrbezirk.Visibility = Visibility.Visible
            End If





            If Not komplettansicht And tbLSG.FontSize = 10 Then
                spLSG.Visibility = Visibility.Collapsed
            Else
                If clsDossier.LSG.showControl Then spLSG.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbNSG.FontSize = 10 Then
                spNSG.Visibility = Visibility.Collapsed
            Else
                If clsDossier.NSG.showControl Then spNSG.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbOEKOKO.FontSize = 10 Then
                spOEKOKO.Visibility = Visibility.Collapsed
            Else
                If clsDossier.OEKOKO.showControl Then spOEKOKO.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbUEB.FontSize = 10 Then
                spUEB.Visibility = Visibility.Collapsed
            Else
                If clsDossier.Ueb.showControl Then spUEB.Visibility = Visibility.Visible
            End If

            If Not komplettansicht And tbUEBKROF.FontSize = 10 Then
                spUEBKROF.Visibility = Visibility.Collapsed
            Else
                If clsDossier.UEBKROF.showControl Then spUEBKROF.Visibility = Visibility.Visible
            End If


            l(" kompaktieren ---------------------- ende")
        Catch ex As Exception
            l("Fehler in kompaktieren: " & ex.ToString())
        End Try
    End Sub

    Private Sub btnprotokollPDF_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim ausgabedatei, text As String
        text = protokollGesamt
        If cbkompaktansicht.IsChecked Then text = protokollKompakt
        ausgabedatei = clsTools.erzeugeUndOeffneText2Pdf(text)
        OpenDokument(ausgabedatei)
    End Sub

    Private Sub btnNDtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.ND.result)
    End Sub

    Private Sub btnND_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim pfad = "\\file-office\Office\UMWELT\B\2-neue Struktur\3 - Naturschutz\30 - Rechtsgrundlagen und Allgemeines\300 - Rechtsgrundlagen, Urteile\3002 - Verordnungen, Erlasse, Richtlinien\Naturdenkmale\manager\"
        Dim datei = clsDossier.ND.kurz.Trim & ".pdf"
        pfad = m.appServerUnc & "\nkat\aid\161\manager\"
        OpenDokument(pfad & datei.Trim)
    End Sub

    Friend Sub GISeditoraufrufen(layeraid As Integer, username As String, gid As String, editid As String)
        l("GISeditoraufrufen---------------------- anfang")
        Dim modul, param, strgid As String
        If gid.Contains(",") Then
            gid = "1"
            editid = "1"
        End If
        Try
            modul = m.appServerUnc & "\apps\gisedit\gisedit.exe "
            modul = "C:\ptest\gisedit\gisedit.exe "
            param = " layeraid=" & layeraid '
            param = param & " gid=" & gid ' 
            param = param & " username=" & username ' 
            param = param & " editid=" & editid ' 
            Process.Start(modul, param)
            l("GISeditoraufrufen---------------------- ende")
        Catch ex As Exception
            l("Fehler in GISeditoraufrufen: " & ex.ToString())
        End Try
    End Sub

    Private Sub btnNDeditor_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        GISeditoraufrufen(161, m.GisUser.username, clsDossier.ND.link, clsDossier.ND.link)
    End Sub



    Private Sub btnaltis16text_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.altis16.result)
    End Sub

    Private Sub btnparadigmatext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.paradigmavorgang.result)
    End Sub

    Private Sub btnparadigma_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Debug.Print(clsDossier.paradigmavorgang.kurz)

        Dim ersterVorgang As String

        ersterVorgang = clsTools.getErsterVorgang(clsDossier.paradigmavorgang.kurz.Trim)

        clsTools.paradigmavorgangaufrufen(ersterVorgang.Trim)
    End Sub

    Private Sub btnKompensationtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.kompensation.result)
    End Sub

    Private Sub cbFSmodus_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        If cbFSmodus.IsChecked Then
            m.flurstuecksModus = True
            Background = Brushes.AliceBlue
            m.userIniProfile.WertSchreiben("gisanalyse", "flurstuecksmodus", "1")
        Else
            m.flurstuecksModus = False
            Background = Brushes.Beige
            m.userIniProfile.WertSchreiben("gisanalyse", "flurstuecksmodus", "0")
        End If
        Close()
        'btnrefresh.IsEnabled = True
    End Sub

    Private Sub btnrefresh_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        gisanalyse()
    End Sub

    Private Sub cbKompaktProtokoll_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
    End Sub

    Private Sub cmbParadigma_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
        If Not ladevorgangAbgeschlossen Then Exit Sub
        Dim auswahl As clsMyComboboxItem = CType(cmbParadigma.SelectedItem, clsMyComboboxItem)

        If auswahl.vid = "" Then Exit Sub
        clsTools.paradigmavorgangaufrufen(auswahl.vid.Trim)
    End Sub

    Private Sub btnSchwalbentext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Schwalben.result)
    End Sub

    Private Sub btnAltstadtsatzungtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Altstadtsatzung.result)
    End Sub

    Private Sub btnAltstadtsatzung_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim verzeichnis = m.appServerUnc & "\nkat\aid\171\satzungen"
        Process.Start(verzeichnis.Trim)
    End Sub

    Private Sub btnStandorttypisierungtext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.standorttypisierung.result)
    End Sub

    Private Sub btnwsgHNUGwmstext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.wsgHNUGwms.result)
    End Sub
End Class
