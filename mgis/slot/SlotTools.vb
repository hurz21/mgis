Imports mgis

Public Class SlotTools
    Friend Shared Function createAllSlots(layerHgrund As clsLayerPres, layers As List(Of clsLayerPres),
                                          cv1 As Canvas, hgmapcanvas As Canvas, osmapcanvas As Canvas,
                                          vgrundRefresh As Boolean, hgrundRefresh As Boolean) As List(Of clsSlot)
        Dim newlist As New List(Of clsSlot)
        Try
            Dim temp As New clsSlot
            temp.funktion = "Hintergrundebene"
            temp.mapfile = layerHgrund.mapFile.Replace("layer.map", "header.map")
            temp.layer = layerHgrund
            temp.canvas = hgmapcanvas
            temp.image = New Image
            temp.bitmap = New BitmapImage
            temp.slotnr = 0
            temp.darstellen = True
            temp.refresh = hgrundRefresh
            newlist.Add(temp)

            temp = New clsSlot
            temp.funktion = "Vordergrundebene"
            temp.mapfile = mapfileCachePathroot & GisUser.username & "_" & clsString.date2string(Now, 5) & ".map"
            temp.layer = New clsLayerPres 'alle
            temp.canvas = cv1
            temp.image = New Image
            temp.bitmap = New BitmapImage
            temp.refresh = vgrundRefresh
            temp.slotnr = 1
            temp.darstellen = True
            newlist.Add(temp)

            temp = New clsSlot
            temp.funktion = "OS-Suche"
            temp.mapfile = mapfileCachePathroot & GisUser.username & "_" & clsString.date2string(Now, 5) & ".map"
            temp.layer = Nothing 'alle
            temp.canvas = osmapcanvas
            temp.image = New Image
            temp.bitmap = New BitmapImage
            temp.refresh = True
            temp.slotnr = 2
            temp.darstellen = True
            newlist.Add(temp)

            Return newlist
        Catch ex As Exception
            l("fehler in createAllSlots " & ex.ToString)
            Return Nothing
        End Try
    End Function
End Class
