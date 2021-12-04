Imports webgiscontrol
Imports System.Data

Module tools
    Public canvasImage As Image
    Public domainstring As String = "http://gis.kreis-of.local"
    Public aktrange As New clsRange
    Public mapfilebild As String
    Public nkat As String = "/nkat/aid/"
    Public stamm_tabelle As String
    Public mgisUserRoot As String = tools.serverUNC & "\apps\test\mgis\"

    Property dbServername As String = "gis"
    Property serverUNC As String = "\\gis\gdvell\"



    Sub openDirectory(zielroot As String)
        Dim fi As New IO.DirectoryInfo(zielroot)
        If fi.Exists Then
            Process.Start(zielroot)
        Else
            MsgBox("Die Datei " & zielroot & " fehlt!")
        End If
    End Sub
    Sub opendocument(zielroot As String)
        'MsgBox(zielroot)
        Dim fi As New IO.FileInfo(zielroot)
        If fi.Exists Then
            Process.Start(zielroot)
        Else
            MsgBox("Die Datei " & zielroot & " fehlt!")
        End If
    End Sub

    Public Sub errorModeAbschicken(aufruf As String, ByRef fehler As String)
        Dim hinweis As String
        Try
            fehler = meineHttpNet.meinHttpJob("", aufruf, hinweis)
        Catch ex As Exception
            l("fehler in MapModeAbschicken: " & aufruf & " /// " & ex.ToString)
        End Try
    End Sub

    Friend Function getRange(name As String) As clsRange
        aktrange.xl = 470685
        aktrange.xh = 503544
        aktrange.yl = 5530566
        aktrange.yh = 5553593
        If name = "klein" Then
            aktrange.xl = 470685
            aktrange.xh = 503544
            aktrange.yl = 5530566
            aktrange.yh = 5553593
        End If
        If name = "gross" Then
            aktrange.xl = 483225
            aktrange.xh = 483562
            aktrange.yl = 5539023
            aktrange.yh = 5539255
        End If
        If name = "mittel" Then
            aktrange.xl = 481568
            aktrange.xh = 485958
            aktrange.yl = 5538793
            aktrange.yh = 5541454
        End If
        Return aktrange
    End Function

    Public Function mapfileErzeugen(aktAid As Integer, mapfileBILD As String) As String
        ' /fkat/flurkarte/flurkarte2016/flurkarte2016_header.map"
        'MAP
        'INCLUDE '/inetpub/wwwroot/buergergis/mapfile/header.map',
        'INCLUDE '/fkat/boden/bodentyp/bodentyp_layer.map',,
        'End
        Dim mt As String = calcMapfileFullname("layer", aktAid)
        Try
            Dim sb As New Text.StringBuilder
            sb.AppendLine("MAP")
            sb.AppendLine("INCLUDE '/inetpub/wwwroot/buergergis/mapfile/header.map'")
            sb.AppendLine("INCLUDE '" & mt & "'")
            sb.AppendLine("End")
            My.Computer.FileSystem.WriteAllText(mapfileBILD, sb.ToString, False, enc)
            Return mt
        Catch ex As Exception
            l("fehler in createMapfileBild " & ex.ToString)
        End Try
    End Function
    Public Function initAuswahlliste(sgauswahl As String) As datatable
        If sgauswahl = String.Empty Then

            If stamm_tabelle = "stamm" Then
                wgisdt = getDT("SELECt * FROM  " & stamm_tabelle & " order by titel", tools.dbServername, "webgiscontrol")
            Else
                wgisdt = getDT("SELECt * FROM  " & stamm_tabelle & " order by aid desc", tools.dbServername, "webgiscontrol")
            End If
        Else
            wgisdt = getDT("SELECt * FROM  " & stamm_tabelle & " where sid=" & sgauswahl, tools.dbServername, "webgiscontrol")
        End If
        Return wgisdt
    End Function
    Function calcMapfileFullname(mapfileTyp As String, aid As Integer) As String
        Try
            Dim mf As String
            'mf = pfad & ebene & "_" & mapfileTyp & ".map"
            mf = nkat & aid & "/" & mapfileTyp & ".map"
            mf = mf.Replace("\\gis\gdvell\", "d:/")
            Return mf
        Catch ex As Exception
            l("fehler in calcMapfileFullname:" & ex.ToString)
            Return ""
        End Try
    End Function
    Public Function aufrufbilden(aktrange As clsRange, mapfile As String, w As Double, h As Double) As String
        Dim sb As New Text.StringBuilder
        sb.Append(domainstring)
        sb.Append("/cgi-bin/mapserv742/mapserv.exe?mode=map&map=")
        sb.Append(mapfile.Replace("\", "/"))
        sb.Append("&mapsize=" & CInt(w) & "+" & CInt(h))
        sb.Append("&ts=" & Format(Now, "yyyyMMddhhmmss"))

        sb.Append("&mapext=" &
                      CInt(aktrange.xl) & "+" &
                      CInt(aktrange.yl) & "+" &
                      CInt(aktrange.xh) & "+" &
                      CInt(aktrange.yh & "+"))
        Dim a$ = sb.ToString
        Return a
    End Function
    Public Function fehlerModusExe(aufruf As String, modus As String) As String
        Dim fehler As String = ""
        Try


            errorModeAbschicken(aufruf, fehler)
            If fehler.Substring(0, 10).ToLower.Contains("png") Then
                Return "PNG OK"
            Else
                If modus = "batch" Then
                    Return fehler
                Else
                    Return "fehler"
                End If

            End If
        Catch ex As Exception
            Return "fehlerModusExe fehler"
        End Try
    End Function


End Module
