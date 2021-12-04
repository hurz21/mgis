Imports System.ComponentModel
Imports mgis
Public Class winDossier
#Region "vardefs"
    Public toptitel As String = ""
    Public ladevorgangAbgeschlossen As Boolean = False
    Public BPLANbeschreibung As String = ""
    Private Property RESULT_dateien_Bplan As New List(Of clsGisresult)
    Private UTMpt As New myPoint
    Private cWidth As Integer
    Private cHeight As Integer
    Private screenpt As New myPoint
#End Region
#Region "newload"
    Sub New(_UTMpt As Point, _width As Integer, _height As Integer, _screenpt As myPoint)
        InitializeComponent()
        UTMpt.X = _UTMpt.X
        UTMpt.Y = _UTMpt.Y
        cWidth = _width
        cHeight = _height
        screenpt.X = _screenpt.X
        screenpt.Y = _screenpt.Y
    End Sub
    Private Sub winDossier_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        svDossier.Height = 600 : spDossier.Height = svDossier.Height
        Me.Top = clsToolsAllg.setPosition("gisanalyse", "dossierformpositiontop", Me.Top)
        Me.Left = clsToolsAllg.setPosition("gisanalyse", "dossierformpositionleft", Me.Left)
        setTitel(UTMpt)
        If Not GisUser.istalbberechtigt Then spEigentuemer.Visibility = Visibility.Collapsed
        ladevorgangAbgeschlossen = True
        initGISanalyse()
        initcontrols()
        gisanalyse()

    End Sub

    'Private Sub setPositionTop()
    '    Try
    '        l(" setPosition ---------------------- anfang")
    '        Dim topf As String = userIniProfile.WertLesen("gisanalyse", "dossierformpositiontop")
    '        If String.IsNullOrEmpty(topf) Then
    '            userIniProfile.WertSchreiben("gisanalyse", "dossierformpositiontop", CType(Me.Top, String))
    '        Else
    '            Top = CDbl(topf)
    '        End If
    '        l(" getIniDossier ---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in setPosition: " & ex.ToString())
    '    End Try
    'End Sub


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
        If Not clsDossier.Eigentuemer.showControl Or Not GisUser.istalbberechtigt Then
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
    End Sub

    Private Sub initGISanalyse()
        Try
            l(" initGISanalyse ---------------------- anfang")
            clsDossier.UEBKROF.showControl = getIniDossier("UEBKROF")
            clsDossier.Ueb.showControl = getIniDossier("UEB")
            clsDossier.Bplan.showControl = getIniDossier("BPLAN")
            clsDossier.Boris.showControl = getIniDossier("boris")
            clsDossier.Baulasten.showControl = getIniDossier("baulasten")
            clsDossier.Altlast.showControl = getIniDossier("altlast")
            clsDossier.Eigentuemer.showControl = getIniDossier("eigentuemer")
            clsDossier.FFH.showControl = getIniDossier("ffh")
            clsDossier.Foerder.showControl = getIniDossier("foerder")
            clsDossier.Illegale.showControl = getIniDossier("illegale")
            clsDossier.IllegaleAlt.showControl = getIniDossier("illegalealt")
            clsDossier.Kehr.showControl = getIniDossier("kehrbezirk")
            clsDossier.LSG.showControl = getIniDossier("lsg")
            clsDossier.NSG.showControl = getIniDossier("nsg")
            clsDossier.WSG.showControl = getIniDossier("wsg")
            clsDossier.Hbiotope.showControl = getIniDossier("hbiotope")
            clsDossier.Hkomplexe.showControl = getIniDossier("hkomplexe")
            clsDossier.Amphibien.showControl = getIniDossier("amph")
            clsDossier.BSE.showControl = getIniDossier("bse")
            clsDossier.OEKOKO.showControl = getIniDossier("oekoko")
            l(" initGISanalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in initGISanalyse: " & ex.ToString())
        End Try
    End Sub

    Private Function getIniDossier(eintrag As String) As Boolean
        Try
            l(" getIniDossier ---------------------- anfang")
            Dim val As String = userIniProfile.WertLesen("gisanalyse", eintrag)
            If String.IsNullOrEmpty(val) Then
                userIniProfile.WertSchreiben("gisanalyse", eintrag, "1")
                Return True
            Else
                Return CBool(val)
            End If
            l(" getIniDossier ---------------------- ende")
        Catch ex As Exception
            l("Fehler in getIniDossier: " & ex.ToString())
            Return True
        End Try
    End Function
#End Region
    Private Sub gisanalyse()
        showTitleUndFlurstueck("flurkarte.basis_f")
        If GisUser.istalbberechtigt Then
            If clsDossier.Eigentuemer.showControl Then eigentuemerAnalyse("flurkarte.basis_f")
        End If
        If clsDossier.Bplan.showControl Then bplananalyse("public.dossier_bplan") '"planung.bebauungsplan_f") 
        If clsDossier.Kehr.showControl Then kehrbezirksAnalyse()
        If clsDossier.NSG.showControl Then nsgAnalyse()
        If clsDossier.LSG.showControl Then lsgAnalyse()
        If clsDossier.FFH.showControl Then ffhAnalyse()
        If clsDossier.Foerder.showControl Then FoerderFlaechenAnalyse()
        If clsDossier.WSG.showControl Then wsgAnalyse()
        If clsDossier.Altlast.showControl Then altlastAnalyse()
        If clsDossier.Illegale.showControl Then IllegaleAnalyse()
        If clsDossier.IllegaleAlt.showControl Then IllegaleALTAnalyse()
        If clsDossier.Boris.showControl Then borisAnalyse()
        If clsDossier.Baulasten.showControl Then baulastenAnalyse()
        If clsDossier.Ueb.showControl Then UeberschemmungsAnalyseHLFU()
        If clsDossier.UEBKROF.showControl Then UeberschemmungsKROFAnalyse()
        If clsDossier.Hbiotope.showControl Then HbiotopeAnalyse()
        If clsDossier.Hkomplexe.showControl Then HKomplexeAnalyse()
        If clsDossier.Amphibien.showControl Then AmphibienAnalyse()
        If clsDossier.BSE.showControl Then bseAnalyse()
        If clsDossier.OEKOKO.showControl Then OEKOKOAnalyse()
    End Sub

    Private Sub OEKOKOAnalyse()
        Try
            l("OEKOKO ---------------------- anfang")
            clsDossier.OEKOKO.schematabelle = "public.dossier_oekoko"
            If clsOekoko.getInfo4point(UTMpt, clsDossier.OEKOKO) Then
                clsDossier.OEKOKO.result = "Ökokonto ---------------------------------" & Environment.NewLine &
                                 clsDossier.OEKOKO.result
                clsDossier.OEKOKO.kurz = clsString.removeLeadingChar(clsDossier.OEKOKO.kurz, ",")
                tbOEKOKO.Text = tbOEKOKO.Text & ": " & clsDossier.OEKOKO.kurz
                tbOEKOKO.ToolTip = tbOEKOKO.Text
                tbOEKOKO.Background = Brushes.LightGreen
                'btnBSE.Visibility = Visibility.Visible
                'btnOEKOKO.IsEnabled = True
                btnOEKOKOtext.IsEnabled = True
            Else
                btnOEKOKOtext.Visibility = Visibility.Collapsed
                tbOEKOKO.FontSize = 10
                tbOEKOKO.Text = "Keine Objekte für Ökokonto gefunden"
                clsDossier.OEKOKO.result = "Ökokonto - Feststellung ---------------------------------" & Environment.NewLine &
                    "Keine Objekte für Ökokonto gefunden"
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
            If clsBSE.getInfo4point(UTMpt, clsDossier.BSE) Then
                clsDossier.BSE.result = "BannSchutzErholungswald ---------------------------------" & Environment.NewLine &
                                 clsDossier.BSE.result
                clsDossier.BSE.kurz = clsString.removeLeadingChar(clsDossier.BSE.kurz, ",")
                tbBSE.Text = tbBSE.Text & ": " & clsDossier.BSE.kurz
                tbBSE.ToolTip = tbBSE.Text
                tbBSE.Background = Brushes.LightGreen
                'btnBSE.Visibility = Visibility.Visible
                'btnBSE.IsEnabled = True
                btnBSEtext.IsEnabled = True
            Else
                btnBSEtext.Visibility = Visibility.Collapsed
                tbBSE.FontSize = 10
                tbBSE.Text = "Keine Objekte für BannSchutzErholungswald gefunden"
                clsDossier.BSE.result = "BannSchutzErholungswald - Feststellung ---------------------------------" & Environment.NewLine &
                    "Keine Objekte für BannSchutzErholungswald gefunden"
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
            If clsAmphibien.getInfo4point(UTMpt, clsDossier.Amphibien) Then
                clsDossier.Amphibien.result = "Amphibienkartierung ---------------------------------" & Environment.NewLine &
                                 clsDossier.Amphibien.result
                clsDossier.Amphibien.kurz = clsString.removeLeadingChar(clsDossier.Amphibien.kurz, ",")
                tbAmph.Text = tbAmph.Text & ": " & clsDossier.Amphibien.kurz
                tbAmph.ToolTip = tbAmph.Text
                tbAmph.Background = Brushes.LightGreen
                btnAmph.Visibility = Visibility.Visible
                btnAmph.IsEnabled = True
                btnAmphtext.IsEnabled = True
            Else
                btnAmphtext.Visibility = Visibility.Collapsed
                btnAmph.Visibility = Visibility.Hidden
                tbAmph.FontSize = 10
                tbAmph.Text = "Keine Objekte der Amphibienkartierung gefunden"
                clsDossier.Amphibien.result = "Amphibienkartierung - Feststellung ---------------------------------" & Environment.NewLine &
                    "Keine Amphibienkartierung gefunden"
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
            If clsKomplexe.getInfo4point(UTMpt, clsDossier.Hkomplexe) Then
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
            Else
                tbHkomplexe.Text = "Keine Objekte der Hess. Biotopkartierung - Komplexe gefunden"
                tbHkomplexe.FontSize = 10
                btnHkomplexetext.Visibility = Visibility.Collapsed
                clsDossier.Hkomplexe.result = "Hess- Biotopkartierung Komplexe - Feststellung ---------------------------------" & Environment.NewLine &
                    "Keine Hess. Komplexe gefunden"
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
            If clsHbiotope.getInfo4point(UTMpt, clsDossier.Hbiotope) Then
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
            Else
                tbHbiotope.Text = "Keine Objekte der Hess. Biotopkartierung (Biotope) gefunden"
                tbHbiotope.FontSize = 10
                'btnHbiotope.IsEnabled = False
                btnHbiotopetext.Visibility = Visibility.Collapsed
                clsDossier.Hbiotope.result = "Hbiotope - Feststellung ---------------------------------" & Environment.NewLine &
                    "Keine Hess. Biotope gefunden"
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
            If clsFoerder.getInfo4point(UTMpt, clsDossier.Foerder) Then
                clsDossier.Foerder.result = "Förderflächen Krof ---------------------------------" & Environment.NewLine &
                                 clsDossier.Foerder.result
                tbFoerder.Text = "Förderfläche: " & clsDossier.Foerder.kurz
                tbFoerder.ToolTip = clsDossier.Foerder.result
                tbFoerder.Background = Brushes.LightGreen
                'btnFoerder.Visibility = Visibility.Visible
                btnFoerdertext.IsEnabled = True
                'btnFoerder.IsEnabled = True
            Else
                tbFoerder.Text = "Kein Förderflächenobjekt"
                tbFoerder.FontSize = 10
                btnFoerdertext.Visibility = Visibility.Collapsed
                clsDossier.Foerder.result = "Förderflächen  - Feststellung ---------------------------------" & Environment.NewLine &
                    "Kein Förderflächen-Objekt"
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
            If clsUebKrof.getInfo4point(UTMpt, clsDossier.UEBKROF) Then
                clsDossier.UEBKROF.result = "Überschwemmungsgebiet Krof ---------------------------------" & Environment.NewLine &
                                clsDossier.UEBKROF.result
                tbUEBKROF.Text = clsDossier.UEBKROF.kurz & ": " & clsDossier.UEBKROF.kurz
                tbUEBKROF.ToolTip = clsDossier.UEBKROF.result
                tbUEBKROF.Background = Brushes.LightGreen
                btnUEBKROF.Visibility = Visibility.Visible
                btnUEBKROFtext.IsEnabled = True
                btnUEBKROF.IsEnabled = True
            Else
                tbUEBKROF.Text = "Kein Überschwemmungsgebiet Krof"
                tbUEBKROF.FontSize = 10
                btnUEBKROF.Visibility = Visibility.Collapsed
                btnUEBKROFtext.Visibility = Visibility.Collapsed
                clsDossier.UEBKROF.result = "Überschwemmungsgebiet Krof - Feststellung ---------------------------------" & Environment.NewLine &
                    "Kein Überschwemmungsgebiet Krof"
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
            bbox = clsWMS.calcVollstBbox(CInt(UTMpt.X) & "," & CInt(UTMpt.Y))
            Dim url, wmslayers, wmsquery_layers As String
            wmslayers = "Ueberschwemmungsgebiete_HQ100_nach_HWG"
            wmsquery_layers = "Ueberschwemmungsgebiete_HQ100_nach_HWG"
            Dim hinweis, resultHQ100, resultHQ200 As String
            url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 248, CInt(cHeight), CInt(cWidth),
                                                  CInt(screenpt.X), CInt(screenpt.Y), "text/plain",
                                                  wmslayers, wmsquery_layers)
            resultHQ100 = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, Text.Encoding.UTF8, 5000)
            '========================================
            wmslayers = "Hochwasser_mit_niedriger_Wahrscheinlichkeit"
            wmsquery_layers = "Hochwasser_mit_niedriger_Wahrscheinlichkeit"
            url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 389, CInt(cHeight), CInt(cWidth),
                                                      CInt(screenpt.X), CInt(screenpt.Y), "text/plain",
                                                  wmslayers, wmsquery_layers)
            resultHQ200 = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, Text.Encoding.UTF8, 5000)


            If resultHQ100.IsNothingOrEmpty Then
                If resultHQ200.IsNothingOrEmpty Then
                    clsDossier.Ueb.result = "Kein Überschwemmungsgebiet HQ100 HLFU"
                    btnUEBttext.Visibility = Visibility.Collapsed
                    tbUEB.Text = clsDossier.Ueb.result
                    tbUEB.FontSize = 10
                    btnUEB.Visibility = Visibility.Visible
                    btnUEBttext.Visibility = Visibility.Collapsed
                Else
                    clsDossier.Ueb.result = "Ist Überschwemmungsgebiet HQ>200 HLFU (HQ Extrem Überflutungsfläche (niedrige Wahrscheinlichkeit nach § 74 WHG)"
                    btnUEBttext.Visibility = Visibility.Visible
                    btnUEBttext.IsEnabled = True
                    tbUEB.Text = "Ist HQ>200 !!!"
                    tbUEB.Background = Brushes.LightGreen
                    'tbUEB.FontSize = 10
                    btnUEB.Visibility = Visibility.Visible
                    btnUEBttext.Visibility = Visibility.Visible
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
            If clsBaulasten.getInfo4point(UTMpt, clsDossier.Ueb.result, clsDossier.Ueb.kurz, clsDossier.Ueb.datei,
                                          "dossier_baulasten") Then
                clsDossier.Ueb.result = "Baulasten  ---------------------------------" & Environment.NewLine &
                                 clsDossier.Ueb.result
                clsDossier.Ueb.kurz = clsString.removeLeadingChar(clsDossier.Ueb.kurz, ",")
                tbBaulasten.Text = tbBaulasten.Text & ": " & clsDossier.Ueb.kurz
                tbBaulasten.ToolTip = tbBaulasten.Text
                tbBaulasten.Background = Brushes.LightGreen
                btnBaulasten.Visibility = Visibility.Visible
                btnBaulasten.IsEnabled = True
                btnBaulastentext.IsEnabled = True
                btnBaulasten.IsEnabled = True
            Else
                tbBaulasten.Text = "Keine Baulasten gefunden"
                tbBaulasten.FontSize = 10
                btnBaulasten.Visibility = Visibility.Collapsed
                btnBaulastentext.Visibility = Visibility.Collapsed
                clsDossier.Baulasten.result = "Baulasten - Feststellung ---------------------------------" & Environment.NewLine &
                    "Keine Baulasten gefunden"
            End If
            zwischenInfo(Environment.NewLine & clsDossier.Ueb.result & Environment.NewLine)
            l("BaulastenAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in BaulastenAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub IllegaleALTAnalyse()
        Try
            l(" IllegaleALTAnalyse ---------------------- anfang")
            If clsIllegaleALT.getIllegaleALTInfo4point(UTMpt, clsDossier.IllegaleAlt.result, clsDossier.IllegaleAlt.kurz, clsDossier.IllegaleAlt.datei, "dossier_illegalealt") Then
                clsDossier.IllegaleAlt.result = "Illegale Bauten ALT  ---------------------------------" & Environment.NewLine &
                                 clsDossier.IllegaleAlt.result
                clsDossier.IllegaleAlt.kurz = clsString.removeLeadingChar(clsDossier.IllegaleAlt.kurz, ",")
                tbIllegaleALT.Text = tbIllegaleALT.Text & ": " & clsDossier.IllegaleAlt.kurz
                tbIllegaleALT.ToolTip = tbIllegaleALT.Text
                tbIllegaleALT.Background = Brushes.LightGreen
                'btnIllegaleALT.Visibility = Visibility.h
                btnIllegaleALTtext.IsEnabled = True
                btnIllegaleALT.IsEnabled = True
            Else
                tbIllegaleALT.Text = "Keine Objekte aus Illegale bauten bis 2004"
                tbIllegaleALT.FontSize = 10
                btnIllegaleALT.Visibility = Visibility.Collapsed
                btnIllegaleALTtext.Visibility = Visibility.Collapsed
                clsDossier.IllegaleAlt.result = "IllegaleALT - Feststellung ---------------------------------" & Environment.NewLine &
                    "Kein IllegaleALT"
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
            If clsIllegaleNeu.getIllegaleInfo4point(UTMpt, clsDossier.Illegale.result, clsDossier.Illegale.kurz,
                                                    clsDossier.Illegale.datei, "public.dossier_illegaleneu") Then
                clsDossier.Illegale.result = "Illegale Bauten neu  ---------------------------------" & Environment.NewLine &
                                 clsDossier.Illegale.result
                clsDossier.Illegale.kurz = clsString.removeLeadingChar(clsDossier.Illegale.kurz, ",")
                tbIllegale.Text = tbIllegale.Text & ": " & clsDossier.Illegale.kurz
                tbIllegale.ToolTip = tbIllegale.Text
                tbIllegale.Background = Brushes.LightGreen
                btnIllegale.Visibility = Visibility.Visible
                btnIllegaletext.IsEnabled = True
                btnIllegale.IsEnabled = True
            Else
                tbIllegale.Text = "Keine Objekte in akt. IllegaleBauten gef."
                tbIllegale.FontSize = 10
                btnIllegale.Visibility = Visibility.Collapsed
                btnIllegaletext.Visibility = Visibility.Collapsed
                clsDossier.Illegale.result = "Illegale - Feststellung ---------------------------------" & Environment.NewLine & "Kein Illegale"
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
            bbox = clsWMS.calcVollstBbox(CInt(UTMpt.X) & "," & CInt(UTMpt.Y))
            Dim url As String
            url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 379, CInt(cHeight), CInt(cWidth),
                                                      CInt(screenpt.X), CInt(screenpt.Y), "text/plain", "", "")
            Dim hinweis As String = ""
            'Dim result As String = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, myglobalz.enc, 5000)
            Dim result As String = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, Text.Encoding.UTF8, 5000)
            Dim a() As String
            clsDossier.Boris.result = ""
            a = result.Split(CType(vbCrLf, Char()))
            For Each text As String In a
                If text.Contains("BRW = '") Then
                    clsDossier.Boris.result = text.Replace("BRW = '", "").Trim
                    'RESULT_text_Boris = RESULT_text_Boris.Replace("Â", "")
                    'RESULT_text_Boris = RESULT_text_Boris.Replace("Ã¤", "ä")
                    clsDossier.Boris.result = clsDossier.Boris.result.Replace("'", "")
                    tbboris.Background = Brushes.LightGreen
                    btnboristtext.IsEnabled = True
                    Exit For
                End If
            Next
            clsDossier.Boris.result = "Bodenrichtwert ?  ---------------------------------" & Environment.NewLine &
                                  clsDossier.Boris.result
            zwischenInfo(Environment.NewLine & clsDossier.Boris.result)
            l(" borisAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in borisAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub altlastAnalyse()
        Try
            l(" altlastAnalyse ---------------------- anfang")
            If clsAltlast.getAltlastInfo4point(UTMpt, clsDossier.Altlast.result, clsDossier.Altlast.kurz, clsDossier.Altlast.datei, "dossier_altflaeche") Then
                clsDossier.Altlast.result = "Altlast-Hinweisfläche ?  ---------------------------------" & Environment.NewLine &
                                 clsDossier.Altlast.result
                clsDossier.Altlast.kurz = clsString.removeLeadingChar(clsDossier.Altlast.kurz, ",")
                tbaltlast.Text = tbaltlast.Text & ": " & clsDossier.Altlast.kurz
                tbaltlast.ToolTip = tbaltlast.Text
                tbaltlast.Background = Brushes.LightGreen
                btnaltlast.Visibility = Visibility.Visible
                btnaltlasttext.IsEnabled = True
                btnaltlast.IsEnabled = True
            Else
                tbaltlast.Text = "Kein Altlast"
                tbaltlast.FontSize = 10
                btnaltlast.Visibility = Visibility.Collapsed
                btnaltlasttext.Visibility = Visibility.Collapsed
                clsDossier.Altlast.result = "Altlast - Feststellung ---------------------------------" & Environment.NewLine & "Kein Altlast"
            End If
            zwischenInfo(Environment.NewLine & clsDossier.Altlast.result & Environment.NewLine)
            l(" altlastAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in altlastAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub showTitleUndFlurstueck(schematabelle As String)
        Dim fs As String
        Try
            l(" showTitleUndFlurstueck ---------------------- anfang")
            toptitel = toptitel & "Dossier für UTM Koordinate: " & CInt(UTMpt.X) & ", " & CInt(UTMpt.Y) & ". Liegt auf Flurstück: "
            If clsEigentuemerAnalyse.getFS4coordinates(UTMpt, fs, schematabelle) Then
                aktFST.clear()
                aktFST.normflst.FS = fs
                aktFST.normflst.splitFS(fs)
                toptitel = toptitel & aktFST.normflst.toShortstring(",")
            End If
            zwischenInfo(toptitel)
            l(" showTitleUndFlurstueck ---------------------- ende")
        Catch ex As Exception
            l("Fehler in showTitleUndFlurstueck: " & ex.ToString())
        End Try
    End Sub

    Private Sub wsgAnalyse()
        Try
            l(" wsgAnalyse ---------------------- anfang")
            If clsWSG.getWSGInfo4point(UTMpt, clsDossier.WSG.result, clsDossier.WSG.kurz, clsDossier.WSG.datei, "public.dossier_wsg") Then
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
            Else
                tbWSG.Text = "Kein WSG"
                tbWSG.FontSize = 10
                btnWSG.Visibility = Visibility.Collapsed
                btnWSGtext.Visibility = Visibility.Collapsed
                clsDossier.WSG.result = "WSG - Feststellung ---------------------------------" & Environment.NewLine &
                    "Kein WSG"
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
            If clsFFH.getFFHInfo4point(UTMpt, clsDossier.FFH.result, clsDossier.FFH.kurz, clsDossier.FFH.datei, clsDossier.FFH.link) Then
                clsDossier.FFH.result = "FFH - Feststellung ---------------------------------" & Environment.NewLine &
                                 clsDossier.FFH.result
                tbFFH.Text = tbFFH.Text & ": " & clsDossier.FFH.kurz
                tbFFH.ToolTip = tbFFH.Text
                tbFFH.Background = Brushes.LightGreen
                btnFFH.Visibility = Visibility.Visible
                btnFFHtext.IsEnabled = True
                btnFFHAnlage.IsEnabled = True
                btnFFH.IsEnabled = True
            Else
                tbFFH.Text = "Kein FFH"
                tbFFH.FontSize = 10
                btnFFH.Visibility = Visibility.Collapsed
                btnFFHtext.Visibility = Visibility.Collapsed
                btnFFHAnlage.Visibility = Visibility.Collapsed
                clsDossier.FFH.result = "FFH - Feststellung ---------------------------------" & Environment.NewLine & "Kein FFH"
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
            If clsLSG.getLSGInfo4point(UTMpt, clsDossier.LSG.result, clsDossier.LSG.kurz, clsDossier.LSG.datei, "public.dossier_lsg") Then
                clsDossier.LSG.result = "LSG - Feststellung ---------------------------------" & Environment.NewLine &
                                   clsDossier.LSG.result
                tbLSG.Text = tbLSG.Text & ": " & clsDossier.LSG.kurz
                tbLSG.ToolTip = tbLSG.Text
                tbLSG.Background = Brushes.LightGreen
                btnLSG.Visibility = Visibility.Visible
                btnLSGtext.IsEnabled = True
                btnLSG.IsEnabled = True
            Else
                tbLSG.Text = "Kein LSG"
                tbLSG.FontSize = 10
                btnLSG.Visibility = Visibility.Collapsed
                btnLSGtext.Visibility = Visibility.Collapsed
                clsDossier.LSG.result = "LSG - Feststellung ---------------------------------" & Environment.NewLine & "Kein LSG"
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
            If clsNSG.getNSGInfo4point(UTMpt, clsDossier.NSG.result, clsDossier.NSG.kurz, clsDossier.NSG.datei, "public.dossier_nsgglb") Then
                clsDossier.NSG.result = "NSG - Feststellung ---------------------------------" & Environment.NewLine &
                                   clsDossier.NSG.result
                tbNSG.Text = tbNSG.Text & ": " & clsDossier.NSG.kurz
                tbNSG.ToolTip = tbNSG.Text
                tbNSG.Background = Brushes.LightGreen
                btnNSG.Visibility = Visibility.Visible
                btnNSGtext.IsEnabled = True
                btnNSG.IsEnabled = True
            Else
                tbNSG.Text = "Kein NSG"
                tbNSG.FontSize = 10
                btnNSG.Visibility = Visibility.Collapsed
                btnNSGtext.Visibility = Visibility.Collapsed
                clsDossier.NSG.result = "NSG - Feststellung ---------------------------------" & Environment.NewLine & "Kein NSG"
            End If
            zwischenInfo(Environment.NewLine & clsDossier.NSG.result & Environment.NewLine)
            l(" nsgAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in nsgAnalyse: " & ex.ToString())
        End Try
    End Sub
    Private Sub setTitel(winpt As myPoint)
        Title = Title & " " & CInt(winpt.X) & ", " & CInt(winpt.Y)
    End Sub

    Private Sub btnbplanaufruf2_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If clsDossier.Bplan.link.IsNothingOrEmpty Then
            MsgBox("Kein Bplan zur Adresse gefunden.")
        Else
            Process.Start(clsDossier.Bplan.link)
        End If
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

    Private Sub btnabbruch_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub btnbplanaufruf_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        If clsDossier.Bplan.datei.IsNothingOrEmpty Then
            MsgBox("Kein Bplan zur Adresse gefunden.")
        Else
            Process.Start(clsDossier.Bplan.datei)
        End If
    End Sub
    Private Sub zwischenInfo(text As String)
        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
        tbInfo.Text = tbInfo.Text & text & Environment.NewLine
        Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Background, Function() 0) 'Doevents
    End Sub



    Private Sub kehrbezirksAnalyse()
        Try
            l(" kehrbezirksAnalyse ---------------------- anfang")
            If clsKehrbezirk.getKehrbezirkInfo4point(UTMpt, clsDossier.Kehr.result, clsDossier.Kehr.kurz, "public.dossier_kehrbezirk", "public.dossier_kehrbezirk_a") Then
                clsDossier.Kehr.result = "Kehrbezirksfeststellung ---------------------------------" & Environment.NewLine &
                                  clsDossier.Kehr.result
                tbKehrbezirk.Background = Brushes.LightGreen
                btnKehrbezirktext.IsEnabled = True
                tbKehrbezirk.Text = tbKehrbezirk.Text & " " & clsDossier.Kehr.kurz
            Else
                tbKehrbezirk.Text = "Kein Kehrbezirk"
                tbKehrbezirk.FontSize = 10
                btnKehrbezirktext.Visibility = Visibility.Collapsed
                clsDossier.Kehr.result = "Kehrbezirksfeststellung ---------------------------------" & Environment.NewLine & "Kein Kehrbezirk festgestellt!"
            End If
            zwischenInfo(Environment.NewLine & clsDossier.Kehr.result & Environment.NewLine)
            zwischenInfo("Kehrbezirksfeststellung Ende-------------------" & Environment.NewLine)
            l(" kehrbezirksAnalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in kehrbezirksAnalyse: " & ex.ToString())
        End Try
    End Sub

    Private Sub eigentuemerAnalyse(schematabelle As String)
        Dim fs As String = ""
        zwischenInfo("Eigentümer festellen ---------------------")
        If clsEigentuemerAnalyse.getFS4coordinates(UTMpt, fs, schematabelle) Then
            aktFST = New ParaFlurstueck
            aktFST.normflst.FS = fs
            aktFST.normflst.splitFS(fs)
            clsDossier.Eigentuemer.kurz = getSchnellbatchEigentuemer(fs.Trim)
            Dim flaeche As String = clsFSTtools.getFlaecheZuFlurstueck(aktFST)

            clsDossier.Eigentuemer.result = "Eigentümerfeststellung für Flurstück " &
                                        aktFST.normflst.toShortstring(",") & Environment.NewLine &
                                        flaeche & Environment.NewLine &
                                        "Eigentümer in Kurzform: " & Environment.NewLine &
                                       clsDossier.Eigentuemer.kurz & " ---------------------------"
            btnEigentuemerPDF.IsEnabled = True
            btnEigentuemertext.IsEnabled = True
            tbEigentuemer.Background = Brushes.LightGreen
        Else
            clsDossier.Eigentuemer.result = "Keine EigentümerInformation gefunden ---------------------------"
            btnEigentuemerPDF.IsEnabled = False
            btnEigentuemertext.IsEnabled = False
        End If
        zwischenInfo(Environment.NewLine & clsDossier.Eigentuemer.result)
    End Sub

    Private Sub bplananalyse(schematabelle As String)
        Dim Plannr As String()
        Dim bplanPDFliste As New List(Of clsGisresult)
        ReDim Plannr(20)
        Dim dt As System.Data.DataTable
        Try
            l(" bplananalyse ---------------------- anfang")
            'zwischenInfo("Bebauungspläne werden für den Punkt gesucht!")
            If clsBplan.getBplanInfo4point(UTMpt, dt, schematabelle) Then
                clsBplan.hurz(dt, clsDossier.Bplan.result, Plannr, RESULT_dateien_Bplan)

                btnbplanaufruf.IsEnabled = True
                btnbplanaufruf.ToolTip = "Taste drücken zum anzeigen"
                tbbplangueltig.Text = "B-Plan vorhanden: " & Plannr(0)
                tbbplangueltig.Background = Brushes.LightGreen
                bplanPDFliste = clsDossier.makebplanPDFliste(RESULT_dateien_Bplan)
                dgZusatzinfo.DataContext = bplanPDFliste
                If bplanPDFliste Is Nothing OrElse bplanPDFliste.Count < 1 Then
                    dgZusatzinfo.Visibility = Visibility.Collapsed
                Else
                    dgZusatzinfo.Visibility = Visibility.Visible
                End If
                btnbplan1text.IsEnabled = True
                clsDossier.Bplan.datei = RESULT_dateien_Bplan.Item(0).datei.FullName.Trim
                If Not hatZweitenBplan(RESULT_dateien_Bplan) Then
                    sp2bplan.Visibility = Visibility.Collapsed
                    btnbplan2text.IsEnabled = False
                Else
                    btnbplan2text.IsEnabled = True
                    sp2bplan.Visibility = Visibility.Visible
                    btnbplanaufruf2.IsEnabled = True
                    clsDossier.Bplan.link = RESULT_dateien_Bplan.Item(1).datei.FullName.Trim
                    tbbplangueltig2.Text = "B-Plan vorhanden: " & Plannr(1)
                End If
                zwischenInfo(clsDossier.Bplan.result)
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
            zwischenInfo("B-Plan Analyse fertig---------------------" & Environment.NewLine)
            l(" bplananalyse ---------------------- ende")
        Catch ex As Exception
            l("Fehler in bplananalyse: " & ex.ToString())
        End Try
    End Sub

    Private Function hatZweitenBplan(rESULT_dateien_Bplan As List(Of clsGisresult)) As Boolean
        If rESULT_dateien_Bplan.Count > 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnUEBKROF_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim datei As String
        datei = clsDossier.UEBKROF.datei
        datei = "\\w2gis02\gdvell\\fkat\wasser\ueberschw\texte\" & datei & ".pdf"
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
        bbox = clsWMS.calcVollstBbox(CInt(UTMpt.X) & "," & CInt(UTMpt.Y))

        url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 248, CInt(cHeight), CInt(cWidth),
                                                  CInt(screenpt.X), CInt(screenpt.Y), "text/html",
                                              wmslayers, wmsquery_layers)
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

    Private Sub btnEigentuemerPDF_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim EigentuemerPDF As String = clsSachdatentools.erzeugeUndOeffneEigentuemerPDF(clsDossier.Eigentuemer.kurz)
        OpenDokument(EigentuemerPDF)
    End Sub

    Private Sub btnBaulastentext_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnBaulasten_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim baulast As String
        If clsDossier.Ueb.datei.ToLower.StartsWith("keine") Then
            MessageBox.Show("Keine Baulast vorhanden")
        Else
            baulast = "\\w2gis02\gdvell\" & clsDossier.Ueb.datei.Replace("/", "\")
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
        tools.paradigmavorgangaufrufen(clsDossier.Illegale.datei)
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

    Private Sub btnDossierKonfig_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim konfig As New winDossierSet
        konfig.Show()
        l("fehler kein fehler dossier2 aufruf " & Environment.UserName)
        Close()
    End Sub

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

    Private Sub btnEigentuemertext_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        tbInfo.Text = ""
        zwischenInfo(clsDossier.Eigentuemer.result)
    End Sub
    Private Sub btnboris_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        Dim bbox As String
        bbox = clsWMS.calcVollstBbox(CInt(UTMpt.X) & "," & CInt(UTMpt.Y))
        Dim url As String
        url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 379, CInt(cHeight), CInt(cWidth),
                                                  CInt(screenpt.X), CInt(screenpt.Y), "text/html", "", "")
        Process.Start(url)
    End Sub
    Private Sub btnborisERleuterung_Click(sender As Object, e As RoutedEventArgs)
        e.Handled = True
        clsDossier.Boris.link = "https://hvbg.hessen.de/sites/hvbg.hessen.de/files/0606006-DARMSTADT-2018.pdf"

        Process.Start(clsDossier.Boris.link)
    End Sub

    Private Sub winDossier_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        savePosition()
    End Sub

    Private Sub savePosition()
        Try
            userIniProfile.WertSchreiben("gisanalyse", "dossierformpositiontop", CType(Me.Top, String))
            userIniProfile.WertSchreiben("gisanalyse", "dossierformpositionleft", CType(Me.Left, String))
        Catch ex As Exception

        End Try
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
        clsMiniMapTools.klassischeDBabfrage(CInt(clsDossier.Amphibien.link), "amphibien", "MSKamphibien_2.htm")
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
End Class
