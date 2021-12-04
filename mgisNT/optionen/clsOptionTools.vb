Public Class clsOptionTools
    Shared Sub einlesenParadigmaDominiert(ByRef paradigmaDominiertzuletztFavoriten As Boolean)
        Dim test As String = myglobalz.userIniProfile.WertLesen("gisstart", "paradigmaDominiertFavoriten")
        If STARTUP_mgismodus <> "probaug" Then
            test = "False" ' wird erst mal fest installiert
        End If
        If test.IsNothingOrEmpty Then
            paradigmaDominiertzuletztFavoriten = False
        Else
            paradigmaDominiertzuletztFavoriten = CBool(test)
        End If
    End Sub
    Shared Function aufzweitembildschirmstartengetImmeraufzweiten() As Boolean?
        Return CType(userIniProfile.WertLesen("gisstart", "ImmerAufZweitemScreen"), Boolean?)
    End Function

    Shared Function bildeStatusText() As String
        Dim titelliste As String = ""
        Dim sb As New Text.StringBuilder
        sb.Append("mapfile: " & mapfileBILD & Environment.NewLine)
        sb.Append("xl=" & kartengen.aktMap.aktrange.xl & Environment.NewLine)
        sb.Append("xh=" & kartengen.aktMap.aktrange.xh & Environment.NewLine)
        sb.Append("yl=" & CInt(kartengen.aktMap.aktrange.yl) & Environment.NewLine)
        sb.Append("yh=" & CInt(kartengen.aktMap.aktrange.yh) & Environment.NewLine)
        sb.Append(" " & Environment.NewLine)
        sb.Append("modus " & STARTUP_mgismodus & Environment.NewLine)
        sb.Append("user " & GisUser.username & " " & clsActiveDir.fdkurz & Environment.NewLine)
        sb.Append("fd " & GisUser.favogruppekurz & Environment.NewLine)
        getVorhandeneEbenen(titelliste)
        sb.Append("vorhandene Ebenen: " & Environment.NewLine &
           titelliste & Environment.NewLine)
        getGescheckteEbene(titelliste)
        sb.Append("gecheckte Ebenen: " & Environment.NewLine &
                titelliste & Environment.NewLine)
        sb.Append("aktiv: " & layerActive.aid & " (" & layerActive.titel & ")" & Environment.NewLine)
        sb.Append("hgrund: " & layerHgrund.aid & " (" & layerHgrund.titel & ")" & Environment.NewLine)
        sb.Append("GroupLayerSqlString: " & GroupLayerSqlString & Environment.NewLine)
        sb.Append("lastGeomAsWKT: " & lastGeomAsWKT & Environment.NewLine)

        Return sb.ToString
    End Function
End Class
