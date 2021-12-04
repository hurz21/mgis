Public Class clsSlot
    Property mapfile As String = ""
    Property canvas As New Canvas
    Property image As New Image
    Property bitmap As New BitmapImage()
    Property aufruf As String = ""
    Property darstellen As Boolean = False
    Property funktion As String = ""
    Property slotnr As Integer = 0
    Property layer As New clsLayerPres
    Property refresh As Boolean = False
    Sub setEmpty()
        Try
            'l(" setEmpty ---------------------- anfang")
            canvas.Children.Clear()
            image = New Image
            leeresbild(image)
            image.Width = globCanvasWidth
            image.Height = globCanvasHeight

            canvas.Children.Add(image)
            'l(" setEmpty ---------------------- ende")
        Catch ex As Exception
            l("Fehler in setEmpty: " & ex.ToString())
        End Try
    End Sub
    Private Sub leeresbild(canvasImage As Image)
        Dim myBitmapImage As New BitmapImage()
        '     Dim aufruf As String = New Uri("/mgis;component/icons/leer.png", UriKind.Absolute) 'serverWeb & "/leer.png" '"P:\a_vs\NEUPara\mgis\leer.png"
        Try
            myBitmapImage.BeginInit()
            myBitmapImage.UriSource = New Uri("/mgis;component/icons/leer.png", UriKind.RelativeOrAbsolute)
            myBitmapImage.EndInit()
            canvasImage.Source = myBitmapImage
            GC.Collect()
        Catch ex As Exception
            l("fehler in leeresbild: " & aufruf & " /// ", ex)
        End Try
    End Sub
    Public Function BildGenaufrufMAPserver(mapfile As String, domain As String, amap As clsMapSpec, isuserlayer As Boolean) As String
        Try
            Dim sb As New Text.StringBuilder
            'If myglobalz.getMapsFromInternet Then
            '    If isuserlayer Then
            '        sb.Append(domain)
            '    Else
            '        sb.Append(strGlobals.buergergisInternetServer)
            '    End If
            'Else
            sb.Append(domain)
            'End If
            sb.Append("/cgi-bin/" & strGlobals.mapserverExeString & "?mode=map&map=")
            sb.Append(mapfile.Replace("\", "/").Replace("d:", ""))
            sb.Append("&mapsize=" & amap.aktcanvas.w & "+" & amap.aktcanvas.h)
            sb.Append("&ts=" & Format(Now, "yyyyMMddhhmmss"))

            sb.Append("&mapext=" &
                      CInt(amap.aktrange.xl) & "+" &
                      CInt(amap.aktrange.yl) & "+" &
                      CInt(amap.aktrange.xh) & "+" &
                      CInt(amap.aktrange.yh & "+"))
            Dim a$ = sb.ToString
            'l("clsaufrufgenerator: genaufruf " & a)
            a = sb.ToString
            'l("clsaufrufgenerator: genaufrufkomplett " & a)
            sb = Nothing
            aufruf = a
            Return a
        Catch ex As Exception
            nachricht("Fehler genaufruf: ", ex)
            Return "Fehler"
        End Try
    End Function
End Class

Enum slotnrMeaning
    ' slots 0 to 30 sind einfache maps
    cvtopPanLayer80 = 80
    suchobjektOSliste81 = 81
    suchObjektFlurstueck82 = 82
End Enum
Enum suchobjektmodusEnum
    flurstuecksObjektDarstellen = 1 'fst
    pufferObjektDarstellen = 2 'puffer
End Enum
