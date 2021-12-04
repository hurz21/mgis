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
    Friend Shared Function Muss3DinternOeffnenOeffnen() As Boolean
        If (myglobalz.userIniProfile.WertLesen("Diverse", "3DinternOeffnen")).IsNothingOrEmpty Then
            Return True
        Else
            If (myglobalz.userIniProfile.WertLesen("Diverse", "3DinternOeffnen")) = "1" Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Friend Shared Function PDFimmerAcrobOeffnenat() As Boolean
        If (myglobalz.userIniProfile.WertLesen("Diverse", "PDFimmerAcrobat")).IsNothingOrEmpty Then
            Return False
        Else
            If (myglobalz.userIniProfile.WertLesen("Diverse", "PDFimmerAcrobat")) = "1" Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Shared Function bildeStatusText() As String
        Dim titelliste As String = ""
        Dim count As Integer
        Dim sb As New Text.StringBuilder
        sb.Append("xl=" & kartengen.aktMap.aktrange.xl & Environment.NewLine)
        sb.Append("xh=" & kartengen.aktMap.aktrange.xh & Environment.NewLine)
        sb.Append("yl=" & CInt(kartengen.aktMap.aktrange.yl) & Environment.NewLine)
        sb.Append("yh=" & CInt(kartengen.aktMap.aktrange.yh) & Environment.NewLine)
        sb.Append(" " & Environment.NewLine)
        sb.Append("modus " & STARTUP_mgismodus & Environment.NewLine)
        sb.Append("user " & GisUser.username & " " & clsActiveDir.fdkurz & Environment.NewLine)
        sb.Append("nick " & GisUser.nick & Environment.NewLine)
        sb.Append("fd " & GisUser.favogruppekurz & Environment.NewLine)
        getVorhandeneEbenen(titelliste)
        sb.Append("vorhandene Ebenen: " & layersSelected.Count & Environment.NewLine &
                      titelliste & Environment.NewLine)
        getGescheckteEbene(titelliste, count)
        sb.Append("gecheckte Ebenen: " & count & Environment.NewLine &
                      titelliste & Environment.NewLine)
        sb.Append("aktiv: " & layerActive.aid & " (" & layerActive.titel & ")" & Environment.NewLine)
        sb.Append("hgrund: " & layerHgrund.aid & " (" & layerHgrund.titel & ")" & Environment.NewLine)
        sb.Append("GroupLayerSqlString: " & GroupLayerSqlString & Environment.NewLine)
        sb.Append("lastGeomAsWKT: " & lastGeomAsWKT & Environment.NewLine)

        Return sb.ToString
    End Function

    Friend Shared Function getminErrorMessagesFromIni() As Boolean
        'l(" MOD getminErrorMessagesFromIni anfang")
        Try
            If iminternet Then
                Return True
            End If
            Dim test = myglobalz.userIniProfile.WertLesen("LOGGING", "minErrorMessages")
            If test.IsNothingOrEmpty Then
                userIniProfile.WertSchreiben("LOGGING", "minErrorMessages", "true")
                Return True
            Else
                If test.ToLower = "true" Then
                    Return True
                Else
                    Return False
                End If
            End If
            l(" MOD getminErrorMessagesFromIni ende")
        Catch ex As Exception
            l("Fehler in getminErrorMessagesFromIni: ", ex)
            Return True
        End Try
    End Function

    Friend Shared Function PDFreaderExistiert(pdfReader As String) As Boolean
        Dim fi As IO.FileInfo
        Try
            l(" MOD PDFreaderExistiert anfang: " & pdfReader)
            fi = New IO.FileInfo(pdfReader)
            If fi.Exists Then
                Return True
            Else
                Return False
            End If
            l(" MOD PDFreaderExistiert ende")
            Return True
        Catch ex As Exception
            l("Fehler in PDFreaderExistiert: ", ex)
            Return False
        End Try
    End Function
End Class
