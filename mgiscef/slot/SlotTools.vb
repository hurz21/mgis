Imports mgis

Public Class SlotTools
    Friend Shared Function createAllSlots(layerHgrund As clsLayerPres, layers As List(Of clsLayerPres),
                                            cv0 As Canvas, cv1 As Canvas, cv2 As Canvas, cv3 As Canvas, cv4 As Canvas, cv5 As Canvas,
                                            cv6 As Canvas, cv7 As Canvas, cv8 As Canvas, cv9 As Canvas, cv10 As Canvas,
                                            cv11 As Canvas, cv12 As Canvas, cv13 As Canvas, cv14 As Canvas, cv15 As Canvas,
                                            cv16 As Canvas, cv17 As Canvas, cv18 As Canvas, cv19 As Canvas, cv20 As Canvas,
                                            cv21 As Canvas, cv22 As Canvas, cv23 As Canvas, cv24 As Canvas, cv25 As Canvas,
                                            cv26 As Canvas, cv27 As Canvas, cv28 As Canvas, cv29 As Canvas, cv30 As Canvas,
                                            cv31 As Canvas, cv32 As Canvas, cv33 As Canvas, cv34 As Canvas, cv35 As Canvas,
                                            cv36 As Canvas, cv37 As Canvas, cv38 As Canvas, cv39 As Canvas, cv40 As Canvas,
                                            cv41 As Canvas, cv42 As Canvas, cv43 As Canvas, cv44 As Canvas, cv45 As Canvas,
                                            cv46 As Canvas, cv47 As Canvas, cv48 As Canvas, cv49 As Canvas, cv50 As Canvas,
                                            OSmapCanvas As Canvas) As clsSlot()
        Dim newlist As clsSlot()
        ReDim newlist(50) 'vorher 30
        Try
            'Dim temp As New clsSlot


            For i = 0 To 50
                newlist(i) = New clsSlot
                newlist(i).funktion = "Vordergrundebene"
                newlist(i).mapfile = ""
                newlist(i).layer = New clsLayerPres 'alle
                newlist(i).canvas = New Canvas
                newlist(i).image = New Image
                newlist(i).bitmap = New BitmapImage
                newlist(i).refresh = False
                newlist(i).slotnr = i
                newlist(i).darstellen = False
            Next

            newlist(0).canvas = cv0
            newlist(1).canvas = cv1
            newlist(2).canvas = cv2
            newlist(3).canvas = cv3
            newlist(4).canvas = cv4
            newlist(5).canvas = cv5
            newlist(6).canvas = cv6
            newlist(7).canvas = cv7
            newlist(8).canvas = cv8
            newlist(9).canvas = cv9
            newlist(10).canvas = cv10

            newlist(11).canvas = cv11
            newlist(12).canvas = cv12
            newlist(13).canvas = cv13
            newlist(14).canvas = cv14
            newlist(15).canvas = cv15
            newlist(16).canvas = cv16
            newlist(17).canvas = cv17
            newlist(18).canvas = cv18
            newlist(19).canvas = cv19
            newlist(20).canvas = cv20

            newlist(21).canvas = cv21
            newlist(22).canvas = cv22
            newlist(23).canvas = cv23
            newlist(24).canvas = cv24
            newlist(25).canvas = cv25
            newlist(26).canvas = cv26
            newlist(27).canvas = cv27
            newlist(28).canvas = cv28
            newlist(29).canvas = cv29
            newlist(30).canvas = cv30

            newlist(31).canvas = cv31
            newlist(32).canvas = cv32
            newlist(33).canvas = cv33
            newlist(34).canvas = cv34
            newlist(35).canvas = cv35
            newlist(36).canvas = cv36
            newlist(37).canvas = cv37
            newlist(38).canvas = cv38
            newlist(39).canvas = cv39
            newlist(40).canvas = cv40

            newlist(41).canvas = cv41
            newlist(42).canvas = cv42
            newlist(43).canvas = cv43
            newlist(44).canvas = cv44
            newlist(45).canvas = cv45
            newlist(46).canvas = cv46
            newlist(47).canvas = cv47
            newlist(48).canvas = cv48
            newlist(49).canvas = cv49
            newlist(50).canvas = cv50
            Return newlist

        Catch ex As Exception
            l("fehler in createAllSlots ", ex)
            Return Nothing
        End Try
    End Function



    Friend Shared Sub setAllSlotsEmpty(istart As Integer)
        Try
            l(" setAllSlotsEmpty ---------------------- anfang")
            For i = istart To slots.Length - 1
                slots(i).mapfile = ""
                slots(i).layer = New clsLayerPres 'alle 
                slots(i).image = New Image
                slots(i).bitmap = New BitmapImage
                slots(i).refresh = False
                slots(i).darstellen = False
                slots(i).setEmpty()
            Next
            l(" setAllSlotsEmpty ---------------------- ende")
        Catch ex As Exception
            l("Fehler in setAllSlotsEmpty: " & ex.ToString())
        End Try
    End Sub

    Friend Shared Function layers2Slots(layerHgrund As clsLayerPres, layers As List(Of clsLayerPres),
                                          osmapcanvas As Canvas,
                                          vgrundRefresh As Boolean, hgrundRefresh As Boolean) As Integer
        Try
            slots(0).refresh = hgrundRefresh
            If hgrundRefresh Then
                If layerHgrund.titel.ToLower = "kein hintergrund" Then
                    slots(0).setEmpty()
                    slots(0).darstellen = False
                    slots(0).mapfile = ""
                Else
                    slots(0).setEmpty()
                    slots(0).darstellen = False
                    slots(0).mapfile = layerHgrund.mapFile.Replace("layer.map", "header.map")
                    slots(0).layer = layerHgrund.kopie
                End If
            Else
                'keine refresh - keine aktion
            End If

            'layers ist sortiert nach rank
            Dim aktslot As Integer = 0
            Dim count As Integer = 0
            If vgrundRefresh Then
                For Each ebene As clsLayerPres In layers
                    If ebene.mithaken Then
                        aktslot = getEmptySlot()
                        If aktslot < 0 Then
                            MessageBox.Show("Die Bestandsliste darf beliebig groß werden. " & Environment.NewLine &
                                            "Aber es werden zuviele Ebenen gleichzeitig dargestellt !" & Environment.NewLine &
                                            "Das ist meist nicht gewollt. " & Environment.NewLine &
                                            "Bitte beim Admin melden. " & count & " von 30 ", "Wichtiger Hinweis", MessageBoxButton.OK, MessageBoxImage.Error)
                        Else
                            slots(aktslot).mapfile = ebene.mapFile.Replace("layer.map", "header.map")
                            slots(aktslot).refresh = True
                            slots(aktslot).darstellen = True
                            slots(aktslot).layer = ebene.kopie
                            count += 1
                        End If
                    End If
                Next
            End If


            Return count
        Catch ex As Exception
            l("fehler in createAllSlots ", ex)
            Return 1
        End Try
    End Function

    Shared Function getEmptySlot() As Integer
        Debug.Print(CType(slots.Count, String))
        For i = 1 To slots.Length - 1
            If slots(i).mapfile = "" Then
                Return i
            End If
        Next
        Return -1

    End Function
End Class
