Partial Public Class MainWindowDossier
    Private Sub gisanalyse()
        showTitleUndFlurstueck("flurkarte.basis_f", clsDossier.Eigentuemer.strerror)

        If m.GisUser.istalbberechtigt Then
            If clsDossier.Eigentuemer.showControl Then
                eigentuemerAnalyse("flurkarte.basis_f", clsDossier.Eigentuemer.strerror)
            End If
        End If

        If clsDossier.Altstadtsatzung.showControl Then AltstadtsatzungAnalyse()
        If clsDossier.Bplan.showControl Then bplananalyse("public.dossier_bplan") '"planung.bebauungsplan_f") 
        If clsDossier.Kehr.showControl Then kehrbezirksAnalyse()
        If clsDossier.NSG.showControl Then nsgAnalyse()
        If clsDossier.LSG.showControl Then lsgAnalyse()
        If clsDossier.FFH.showControl Then ffhAnalyse()
        If clsDossier.Foerder.showControl Then FoerderFlaechenAnalyse()
        If clsDossier.WSG.showControl Then wsgAnalyse()
        If clsDossier.wsgHNUGwms.showControl And
            istZielgruppe("umwelt,umwelt", m.GisUser.ADgruppenname) Then
            wsgHNUGwmsAna()
        End If
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
        If clsDossier.ND.showControl Then ndAnalyse()
        If clsDossier.altis16.showControl Then altis16Analyse()
        If clsDossier.paradigmavorgang.showControl Then paradigmavorgangAnalyse()
        If clsDossier.kompensation.showControl And
            istZielgruppe("umwelt,bauaufsicht,gebaeudewirtschaft,gebäudewirtschaft", m.GisUser.ADgruppenname) Then
            kompensationAnalyse()
        End If
        If clsDossier.Schwalben.showControl And
            istZielgruppe("umwelt,bauaufsicht,gebaeudewirtschaft,gebäudewirtschaft", m.GisUser.ADgruppenname) Then
            schwalbenAnalyse()
        End If
        If clsDossier.standorttypisierung.showControl And
            istZielgruppe("umwelt,umwelt", m.GisUser.ADgruppenname) Then
            standorttypisierungAna()
        End If

    End Sub

    Private Sub wsgHNUGwmsAna()
        Try
            l(" wsgHNUGwms ---------------------- anfang")
            Dim bbox As String
            ' bbox = clsWMS.calcVollstBbox(CInt(m.UTMpt.X) & "," & CInt(m.UTMpt.Y))
            bbox = clsWMS.calcVollstBbox(CInt(m.UTMpt.X) & "," & CInt(m.UTMpt.Y), 2)
            l("box1 : " & bbox)
            Dim url As String
            'boris 379
            'test 1000
            url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 10002, CInt(m.MAPPINGhoehe), CInt(m.MAPPINGbreite),
                                                      CInt(m.MAPPINGscreenX),
                                                  CInt(m.MAPPINGscreenY), "text/plain", "TWS_HQS_ALK",
                                                  "TWS_HQS_ALK",
                                                  clsDossier.wsgHNUGwms.strerror)
            l(url)
            Dim hinweis As String = ""
            'Dim result As String = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, myglobalz.enc, 5000)
            clsDossier.wsgHNUGwms.result = meineHttpNet.meinHttpJob(m.ProxyString, url, hinweis, Text.Encoding.UTF8, 5000)
            l("  clsDossier.wsgHNUGwms.result " & clsDossier.wsgHNUGwms.result)

            If clsDossier.wsgHNUGwms.result.IsNothingOrEmpty Then
                clsDossier.wsgHNUGwms.result = "Fehler: WMS-Service 'wsgHNUGwms' des Landes Hessen ist nicht verfügbar!"
                protokollKompakt = protokollKompakt & "wsgHNUGwms: " & clsDossier.wsgHNUGwms.result & trenn
            Else
                If clsDossier.wsgHNUGwms.result.StartsWith("@TWS") Then
                    Dim rpu_status As String = "STATUS_RPU"
                    Dim niceString As String = clsTools.makeTable4WMS(clsDossier.wsgHNUGwms.result, 20, rpu_status)
                    clsDossier.wsgHNUGwms.result = niceString
                    'clsDossier.wsgHNUGwms.result = clsDossier.wsgHNUGwms.result.Replace("@TWS_HQS_ALK OBJECTID;SHAPE;WSG_ID;ZONE;WSG_KURZNAME;WSG_ART;STATUS_RPU;KREIS_MASSGEBLICH_NAME;KREIS_MASSGEBLICH_NR;KREISE;TK25_BEZEICHNUNGEN;ARCHIV_HLNUG;RPU;STAATSANZEIGER;STAATSANZEIGER_AENDER;VERORDNUNGDATUM;WSG_KEY;ZONE_KEY;SHAPE_Length;SHAPE_Area;", "")
                    'clsDossier.wsgHNUGwms.result = clsDossier.wsgHNUGwms.result.Replace("Polygon;", "")
                    'a = clsDossier.wsgHNUGwms.result.Split(";"c)
                    'clsDossier.wsgHNUGwms.result = a(1)
                    clsDossier.wsgHNUGwms.kurz = rpu_status

                    btnwsgHNUGwmstext.IsEnabled = True
                    If rpu_status.ToLower.Contains("verfahren") Then
                        tbwsgHNUGwms.Background = Brushes.LightPink
                        tbwsgHNUGwms.Text = tbwsgHNUGwms.Text & ": Verfahren läuft !!!"
                        tbwsgHNUGwms.ToolTip = tbwsgHNUGwms.Text
                    Else
                        tbwsgHNUGwms.Background = Brushes.LightGreen
                        tbwsgHNUGwms.Text = tbwsgHNUGwms.Text & ": " & clsDossier.wsgHNUGwms.kurz
                        tbwsgHNUGwms.ToolTip = tbwsgHNUGwms.Text
                    End If

                    protokollKompakt = protokollKompakt & "WSG HLUG: " & clsDossier.wsgHNUGwms.kurz & trenn
                Else
                    clsDossier.wsgHNUGwms.result = "Keine Daten zu diesem Punkt"

                End If
                'a = result.Split(CType(vbCrLf, Char()))
                'For Each text As String In a
                '    If text.Contains("BRW = '") Then
                '        clsDossier.standorttypisierung.result = text.Replace("BRW = '", "").Trim
                '        clsDossier.standorttypisierung.result = clsDossier.standorttypisierung.result.Replace("'", "")
                '        tbstandorttypisierung.Background = Brushes.LightGreen
                '        btnStandorttypisierungtext.IsEnabled = True
                '        protokollKompakt = protokollKompakt & "standorttypisierung: " & clsDossier.standorttypisierung.result & trenn
                '        Exit For
                '    End If
                'Next
                'clsDossier.wsgHNUGwms.result = "wsgHNUGwms ?  ---------------------------------" & Environment.NewLine &
                '                      clsDossier.wsgHNUGwms.result
            End If

            zwischenInfo(Environment.NewLine & clsDossier.wsgHNUGwms.result)
            l(" wsgHNUGwms ---------------------- ende")
        Catch ex As Exception
            l("Fehler in wsgHNUGwms: " & ex.ToString())
        End Try
    End Sub

    Private Function istZielgruppe(zielgruppen As String, aDgruppenname As String) As Boolean
        Try
            l(" istZielgruppe ---------------------- anfang")
            If clsString.isinarray(zielgruppen, aDgruppenname, ",") Then
                Return True
            Else
                Return False
            End If
            l(" istZielgruppe ---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in istZielgruppe: " & ex.ToString())
            Return False
        End Try
    End Function

    Private Sub standorttypisierungAna()
        Try
            l(" standorttypisierung ---------------------- anfang")
            Dim bbox As String
            ' bbox = clsWMS.calcVollstBbox(CInt(m.UTMpt.X) & "," & CInt(m.UTMpt.Y))
            bbox = clsWMS.calcVollstBbox(CInt(m.UTMpt.X) & "," & CInt(m.UTMpt.Y), 2)

            Dim url As String
            'boris 379
            'test 1000
            url = clsWMS.calcWMSGetfeatureInfoURL(bbox, 10001, CInt(m.MAPPINGhoehe), CInt(m.MAPPINGbreite),
                                                      CInt(m.MAPPINGscreenX),
                                                  CInt(m.MAPPINGscreenY), "text/plain", "Standorttypisierung_Biotopentwicklung_50000",
                                                  "Standorttypisierung_Biotopentwicklung_50000",
                                                  clsDossier.standorttypisierung.strerror)
            Dim hinweis As String = ""
            'Dim result As String = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, myglobalz.enc, 5000)
            clsDossier.standorttypisierung.result = meineHttpNet.meinHttpJob(m.ProxyString, url, hinweis, Text.Encoding.UTF8, 5000)
            Dim a() As String

            If clsDossier.standorttypisierung.result.IsNothingOrEmpty Then
                clsDossier.standorttypisierung.result = "Fehler: WMS-Service 'standorttypisierung' des Landes Hessen ist nicht verfügbar!"
                protokollKompakt = protokollKompakt & "standorttypisierung: " & clsDossier.standorttypisierung.result & trenn
            Else
                If clsDossier.standorttypisierung.result.StartsWith("@Standorttypisierung_Biotopentwicklung_50000") Then
                    clsDossier.standorttypisierung.result = clsDossier.standorttypisierung.result.Replace("@Standorttypisierung_Biotopentwicklung_50000 OBJECTID;STUFE;BEZEICHNER;SHAPE;SHAPE.AREA;SHAPE.LEN;", "")
                    a = clsDossier.standorttypisierung.result.Split(";"c)
                    clsDossier.standorttypisierung.result = a(2)
                    clsDossier.standorttypisierung.kurz = clsDossier.standorttypisierung.result
                    tbStandorttypisierung.Text = tbStandorttypisierung.Text & ": " & clsDossier.standorttypisierung.kurz
                    tbStandorttypisierung.ToolTip = tbStandorttypisierung.Text
                    btnStandorttypisierungtext.IsEnabled = True
                    tbStandorttypisierung.Background = Brushes.LightGreen
                    protokollKompakt = protokollKompakt & "standorttypisierung: " & clsDossier.standorttypisierung.result & trenn
                Else
                    clsDossier.standorttypisierung.result = "Keine Daten zu diesem Punkt"

                End If
                'a = result.Split(CType(vbCrLf, Char()))
                'For Each text As String In a
                '    If text.Contains("BRW = '") Then
                '        clsDossier.standorttypisierung.result = text.Replace("BRW = '", "").Trim
                '        clsDossier.standorttypisierung.result = clsDossier.standorttypisierung.result.Replace("'", "")
                '        tbstandorttypisierung.Background = Brushes.LightGreen
                '        btnStandorttypisierungtext.IsEnabled = True
                '        protokollKompakt = protokollKompakt & "standorttypisierung: " & clsDossier.standorttypisierung.result & trenn
                '        Exit For
                '    End If
                'Next
                clsDossier.standorttypisierung.result = "standorttypisierung ?  ---------------------------------" & Environment.NewLine &
                                      clsDossier.standorttypisierung.result
            End If

            zwischenInfo(Environment.NewLine & clsDossier.standorttypisierung.result)
            l(" standorttypisierung ---------------------- ende")
        Catch ex As Exception
            l("Fehler in standorttypisierung: " & ex.ToString())
        End Try
    End Sub

    Private Sub AltstadtsatzungAnalyse()
        Try
            l("Altstadtsatzung ---------------------- anfang")
            clsDossier.Altstadtsatzung.schematabelle = "public.dossier_Altstadtsatzung_f"
            If clsUniversell.getInfo4point(m.UTMpt, clsDossier.Altstadtsatzung, clsDossier.Altstadtsatzung.strerror, "homepage") Then
                clsDossier.Altstadtsatzung.result = "Altstadtsatzung ---------------------------------" & Environment.NewLine &
                                 clsDossier.Altstadtsatzung.result
                clsDossier.Altstadtsatzung.kurz = clsString.removeLeadingChar(clsDossier.Altstadtsatzung.kurz, ",")
                clsDossier.Altstadtsatzung.datei = clsString.removeLeadingChar(clsDossier.Altstadtsatzung.datei, ",")
                tbAltstadtsatzung.Text = tbAltstadtsatzung.Text & ": " & clsDossier.Altstadtsatzung.kurz
                tbAltstadtsatzung.ToolTip = tbAltstadtsatzung.Text
                tbAltstadtsatzung.Background = Brushes.LightGreen
                btnAltstadtsatzung.IsEnabled = True
                btnAltstadtsatzungtext.IsEnabled = True
                protokollKompakt = protokollKompakt & "Altstadtsatzung: " & clsDossier.Altstadtsatzung.kurz & trenn
            Else
                If clsDossier.Altstadtsatzung.strerror.ToLower.StartsWith("fehler") Then
                    protokollKompakt = protokollKompakt & "Altstadtsatzung: FEHLER bei Analyse von Tab.: " & clsDossier.Altstadtsatzung.schematabelle & trenn
                    zwischenInfo(Environment.NewLine & "Altstadtsatzung: FEHLER bei analyse von Tab. " & clsDossier.Altstadtsatzung.schematabelle)
                    btnAltstadtsatzungtext.Visibility = Visibility.Collapsed
                    tbAltstadtsatzung.FontSize = 10
                Else
                    btnAltstadtsatzungtext.Visibility = Visibility.Collapsed
                    tbAltstadtsatzung.FontSize = 10
                    tbAltstadtsatzung.Text = "Keine Objekte für Altstadtsatzung gefunden"
                    clsDossier.Altstadtsatzung.result = "Altstadtsatzung - Feststellung ---------------------------------" & Environment.NewLine &
                        "Keine Objekte für Altstadtsatzung gefunden"
                End If
            End If
            zwischenInfo(Environment.NewLine & clsDossier.Altstadtsatzung.result & Environment.NewLine)
            l("Altstadtsatzung ---------------------- ende")
        Catch ex As Exception
            l("Fehler in Altstadtsatzung: " & ex.ToString())
        End Try
    End Sub
End Class
