Public Class clsLayerHelper
    Friend Shared Function getKompaktLayers(layersSelected As List(Of clsLayerPres)) As List(Of clsLayerPres)
        Dim neulist As New List(Of clsLayerPres)
        Try
            For Each nlay As clsLayerPres In layersSelected
                If nlay.mithaken Then
                    neulist.Add(nlay)
                End If
            Next
            Return neulist
        Catch ex As Exception
            l("Fehler in ", ex)
            Return neulist
        End Try
    End Function

    Friend Shared Function layers4Kategorie(sg As String) As List(Of clsLayerPres)
        Dim tlayerlist As New List(Of clsLayerPres)
        Dim tlayer As New clsLayerPres
        Dim anzahlSchonGeladeneEbenen = 0
        Try
            sg = sg.Trim.ToLower
            For Each preslayer As clsLayerPres In allLayersPres
                If preslayer.isHgrund Then
                    Debug.Print("keine hintergründe")
                Else
                    If preslayer.standardsachgebiet.ToLower = (sg) Then
                        'If Not warSchonGeladen(preslayer.aid, layersSelected) Then
                        '    If Not warSchonGeladen(preslayer.aid, tlayerlist) Then
                        '        tlayer = CType(preslayer.Clone, clsLayerPres)
                        '        tlayerlist.Add(tlayer)
                        '    End If
                        'End If
                        '  trefferBehandeln(anzahlSchonGeladeneEbenen, tlayerlist, tlayer, preslayer)
                        preslayer.dokutext = "(Sachgeb.: " & preslayer.kategorieLangtext & ") " & Environment.NewLine &
                            clsWebgisPGtools.bildeDokuTooltip(preslayer)
                        preslayer.thumbnailFullPath = myglobalz.serverWeb & "/nkat/thumbnails/" & preslayer.aid & ".png"
                        tlayerlist.Add(preslayer)
                    End If
                End If
            Next
            Return tlayerlist
        Catch ex As Exception
            l("fehler in ", ex)
            Return tlayerlist
        End Try
    End Function

    Shared Function markiereSchonGeladeneLayerKATEGORIE(katlayersList As List(Of clsLayerPres), layersSelected As List(Of clsLayerPres)) As List(Of clsLayerPres)
        Dim altLayer As New clsLayerPres
        Dim newKatlist As New List(Of clsLayerPres)
        Try
            For Each Klay As clsLayerPres In katlayersList
                altLayer = modLayer.getPresLayerFromList(CInt(Klay.aid), layersSelected)
                If altLayer Is Nothing Then
                    ''  layer kommt in der liste nicht mehr vor (rote löschtaste)
                    ''    katlayersList.Remove(Klay) muesste eigentlich removed werden
                    'Klay.mithaken = False
                    'Klay.myFontStyle = FontStyles.Italic
                    'Klay.schongeladen = 0
                    'Klay.RBischecked = False
                    'newKatlist.Add(Klay)
                    Continue For
                End If
                altLayer.kopieAttributeNach(Klay)
                Klay.etikettfarbe = Brushes.DarkGray
                If Klay.mithaken Then
                    Klay.myFontStyle = FontStyles.Normal
                    Klay.schongeladen = 1
                Else
                    Klay.myFontStyle = FontStyles.Italic
                    Klay.schongeladen = 0
                End If
                newKatlist.Add(Klay)
            Next
            Return newKatlist
        Catch ex As Exception
            l("fehler in ", ex)
            Return newKatlist
        End Try
    End Function
    Shared Function markiereSchonGeladeneLayerEXPLORER(katlayersList As List(Of clsLayerPres), layersSelected As List(Of clsLayerPres)) As List(Of clsLayerPres)
        Dim altLayer As New clsLayerPres
        Dim newKatlist As New List(Of clsLayerPres)
        Try
            For Each Klay As clsLayerPres In katlayersList
                altLayer = modLayer.getPresLayerFromList(CInt(Klay.aid), layersSelected)
                If altLayer Is Nothing Then
                    '  layer kommt in der liste nicht mehr vor (rote löschtaste)
                    '    katlayersList.Remove(Klay) muesste eigentlich removed werden
                    Klay.mithaken = False
                    Klay.myFontStyle = FontStyles.Italic
                    Klay.schongeladen = 0
                    Klay.RBischecked = False
                    newKatlist.Add(Klay)
                    Continue For
                End If
                altLayer.kopieAttributeNach(Klay)
                Klay.etikettfarbe = Brushes.DarkGray
                If Klay.mithaken Then
                    Klay.myFontStyle = FontStyles.Normal
                    Klay.schongeladen = 1
                Else
                    Klay.myFontStyle = FontStyles.Italic
                    Klay.schongeladen = 0
                End If
                newKatlist.Add(Klay)
            Next
            Return newKatlist
        Catch ex As Exception
            l("fehler in ", ex)
            Return newKatlist
        End Try
    End Function
    Shared Function kopiereEigenschaftenFallsSchongeladen(Klay As clsLayerPres, tlist As List(Of clsLayerPres)) As clsLayerPres
        Dim neu As clsLayerPres
        For Each altLayers As clsLayerPres In tlist
            neu = New clsLayerPres
            If altLayers.aid = CInt(Klay.aid) Then
                neu = altLayers.kopie()
                neu.schongeladen = 1
                neu.myFontStyle = FontStyles.Normal
                neu.mithaken = True
                Return neu
            End If
        Next
        Return Klay
    End Function

    Friend Shared Sub writeKatTooltipToFile(tag As String, liste As List(Of clsLayerPres), pfad As String, titel As String)
        Dim summe As String = clsString.Capitalize(titel) & ": " & Environment.NewLine
        Try
            l(" MOD writeKatTooltipToFile anfang")
            For Each item As clsLayerPres In liste
                summe &= "- " & item.Etikett & Environment.NewLine
            Next
            IO.File.WriteAllText(pfad & "\" & tag & ".txt", summe)
            l(" MOD writeKatTooltipToFile ende")
        Catch ex As Exception
            l("Fehler in writeKatTooltipToFile: " & ex.ToString())
        End Try
    End Sub
    Shared Function getkatTooltipFromFile(tag As String, pfad As String) As String
        Dim summe As String = ""
        'Return summe
        Try
            'l(" MOD getkatTooltipFromfile anfang")
            'summe = pfad & "\" & tag & ".txt"
            'Return summe
            If IO.File.Exists(pfad & "\" & tag & ".txt") Then
                summe = IO.File.ReadAllText(pfad & "\" & tag & ".txt")
            Else
                summe = ""
            End If
            'l(" MOD getkatTooltipFromfile ende")
            Return summe
        Catch ex As Exception
            l("Fehler in getkatTooltipFromfile: " & ex.ToString())
            Return summe
        End Try
    End Function

    Friend Shared Sub setKatinfo2Layers(allLayersPres As List(Of clsLayerPres))
        For Each layer As clsLayerPres In allLayersPres
            layer.dokutext = "(Sachgeb.: " & layer.kategorieLangtext & ") " & Environment.NewLine &
                             clsWebgisPGtools.bildeDokuTooltip(layer)
        Next

    End Sub
End Class
