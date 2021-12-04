Imports mgis

Public Class SlotTools
    Friend Shared Function createAllSlots(layerHgrund As clsLayerPres, layers As List(Of clsLayerPres),
                                            cv0 As Canvas, cv1 As Canvas, cv2 As Canvas, cv3 As Canvas, cv4 As Canvas, cv5 As Canvas,
                                            cv6 As Canvas, cv7 As Canvas, cv8 As Canvas, cv9 As Canvas, cv10 As Canvas,
                                            cv11 As Canvas, cv12 As Canvas, cv13 As Canvas, cv14 As Canvas, cv15 As Canvas,
                                            cv16 As Canvas, cv17 As Canvas, cv18 As Canvas, cv19 As Canvas, cv20 As Canvas,
                                            cv21 As Canvas, cv22 As Canvas, cv23 As Canvas, cv24 As Canvas, cv25 As Canvas,
                                            cv26 As Canvas, cv27 As Canvas, cv28 As Canvas, cv29 As Canvas, cv30 As Canvas,
                                            OSmapCanvas As Canvas) As clsSlot()
        Dim newlist As clsSlot()
        ReDim newlist(30)
        Try
            'Dim temp As New clsSlot


            For i = 0 To 30
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
            'newlist(30).canvas = OSmapCanvas

            'newlist(30) = New clsSlot
            'newlist(30).funktion = "OS-Suche"
            'newlist(30).mapfile = mapfileCachePathroot & GisUser.username & "_" & clsString.date2string(Now, 5) & ".map"
            'newlist(30).layer = Nothing 'alle
            'newlist(30).canvas = OSmapCanvas
            'newlist(30).image = New Image
            'newlist(30).bitmap = New BitmapImage
            'newlist(30).refresh = True
            'newlist(30).slotnr = 30
            'newlist(30).darstellen = True
            Return newlist

        Catch ex As Exception
            l("fehler in createAllSlots " & ex.ToString)
            Return Nothing
        End Try
    End Function



    Friend Shared Sub setAllSlotsEmpty(istart As Integer)
        Try
            l(" setAllSlotsEmpty ---------------------- anfang")
            'For Each slot As clsSlot In myglobalz.slots
            For i = istart To slots.Length - 1
                'If slots(i).slotnr = 0 Then slot.funktion = "Hintergrundebene"
                'If slot.slotnr = 30 Then slot.funktion = "Objektsuche"
                'If slot.slotnr > 0 And slot.slotnr < 21 Then
                '    slot.funktion = "Vordergrundebene"
                'End If
                slots(i).mapfile = ""
                slots(i).layer = New clsLayerPres 'alle 
                slots(i).image = New Image
                slots(i).bitmap = New BitmapImage
                slots(i).refresh = False
                slots(i).darstellen = False
                slots(i).setEmpty()

                'If slot.slotnr > 10 Then
                '    Debug.Print("")
                'End If
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
            'For i = 1 To 20
            '    slots(i).funktion = "Vordergrundebene"
            '    slots(i).mapfile = layers(i).mapFile.Replace("layer.map", "header.map")
            'Next
            'layers ist sortiert nach rank
            Dim aktslot As Integer = 0
            Dim count As Integer = 0
            If vgrundRefresh Then
                For Each ebene As clsLayerPres In layers
                    If ebene.mithaken Then
                        aktslot = getEmptySlot()
                        If aktslot < 0 Then
                            MessageBox.Show("zuviele ebenen " & count)
                        Else
                            slots(aktslot).mapfile = ebene.mapFile.Replace("layer.map", "header.map")
                            slots(aktslot).refresh = True
                            slots(aktslot).darstellen = True
                            slots(aktslot).layer = ebene.kopie


                            '#If DEBUG Then
                            '                            l("slots(" & aktslot & ")= " & slots(aktslot).layer.titel & ", " & slots(aktslot).layer.rang)
                            '                            If slots(aktslot).layer.aid = 339 Then
                            '                                Debug.Print("")
                            '                                l("slots(" & aktslot & ")= " & slots(aktslot).layer.titel & ", " & slots(aktslot).layer.rang)
                            '                            End If
                            '#End If
                            count += 1
                        End If
                    End If
                Next
            End If
            'temp = New clsSlot
            'temp.funktion = "OS-Suche"
            'temp.mapfile = mapfileCachePathroot & GisUser.username & "_" & clsString.date2string(Now, 5) & ".map"
            'temp.layer = Nothing 'alle
            'temp.canvas = osmapcanvas
            'temp.image = New Image
            'temp.bitmap = New BitmapImage
            'temp.refresh = True
            'temp.slotnr = 30
            'temp.darstellen = True

            Return count
        Catch ex As Exception
            l("fehler in createAllSlots " & ex.ToString)
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
