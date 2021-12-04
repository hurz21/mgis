Module modCreateNewLayer
    Friend Function createNewLayer(ByRef report As String) As Integer
        Dim layertitle As String
        Dim newAid As Integer
        Try
            layertitle = getNewLayerTitle().Trim
            If layertitle.Trim.Length < 2 Then
                Return 0
            End If

            newAid = dbeditTools.zeileEinfuegen("public.stamm", "titel", layertitle & " ", " RETURNING aid", tools.dbServername, "webgiscontrol")

            ' newAid = 253
            report &= "Neue Ebene:" & Environment.NewLine
            report &= "mit aid " & newAid & "in  stamm angelegt" & Environment.NewLine
            report &= "mit titel " & layertitle & "in  stamm angelegt" & Environment.NewLine

            dbeditTools.zeileEinfuegen("public.schlagworte", "aid,schlagworte", newAid & ",'' ", " RETURNING wid", tools.dbServername, "webgiscontrol")
            report &= "in  schlagworte angelegt" & Environment.NewLine

            dbeditTools.zeileEinfuegen("public.doku", "aid,inhalt", newAid & ",'" & layertitle.Trim & "' ", " RETURNING id", tools.dbServername, "webgiscontrol")
            report &= "in  Doku angelegt" & Environment.NewLine


            dbeditTools.zeileEinfuegen("public.gruppe2aid", "aid", newAid & "" & " ", " RETURNING id", tools.dbServername, "webgiscontrol")
            report &= "in  gruppe2aid (activeDir) angelegt" & Environment.NewLine

            'verzeichnisIn NKAT\AID anlegen
            Dim zielroot As String = createVerzeichnisINNkat(newAid)
            report &= "neues verzeichnis unter nkat angelegt: " & zielroot & Environment.NewLine

            Dim layerstring = "/nkat/aid/" & newAid & "/layer.map"
            Dim header = zielroot '& newAid
            header = header & "header.map"
            createHeaderFile(layerstring, header)
            report &= "header mapfile angelegt" & Environment.NewLine
            header = zielroot & "layer.map"
            createEmptyLayerMapfile(header)
            report &= "leeres Layer-mapfile angelegt" & Environment.NewLine
            'layer map anlegen
            Return newAid
        Catch ex As Exception
            l("fehler in createNewLayer: " & ex.ToString)
            Return 0
        End Try

    End Function

    Private Function createVerzeichnisINNkat(newAid As Integer) As String
        Dim zielroot As String
        Try
            zielroot = tools.serverUNC & "\nkat\aid\" & newAid & "\"
            IO.Directory.CreateDirectory(zielroot)
            Return zielroot
        Catch ex As Exception
            l("fehler in createVerzeichnisINNkat: " & ex.ToString)
            Return "fehler"
        End Try
    End Function

    Private Sub createEmptyLayerMapfile(header As String)
        l("in createEmptyLayerMapfile--------------------------")
        Try
            Dim sb As New Text.StringBuilder
            sb.AppendLine(" ")
            sb.AppendLine(" ")
            sb.AppendLine(" ")
            sb.AppendLine(" ")
            My.Computer.FileSystem.WriteAllText(header, sb.ToString, False, enc)
            sb = Nothing
        Catch ex As Exception
            l("fehler in createMapfilePDF " & ex.ToString)
        End Try
    End Sub

    Function getNewLayerTitle() As String
        Dim layerTitle As String = InputBox("Bitte einen vorläufigen Titel eingeben", "Titel der neuen Ebene", "Flurkarte aktuell")
        If layerTitle.Length < 3 Then
            MsgBox("zu kurz")
            Return ""
        End If
        Return layerTitle.Replace(",", ";") 'wg, numerisch  erkennung
    End Function

    Friend Function createNewAttributeTable(aktaid As Integer, NeueTabNr As Integer) As Integer
        Dim interneID As Integer = dbeditTools.zeileEinfuegen("public.attributtabellen", "aid,tab_nr", aktaid & ", " & NeueTabNr, " RETURNING id", tools.dbServername, "webgiscontrol")
        Return interneID
    End Function
End Module
