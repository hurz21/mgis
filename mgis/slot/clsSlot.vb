Public Class clsSlot
    Property mapfile As String = ""
    Property canvas As New Canvas
    Property image As New Image
    Property bitmap As New BitmapImage()
    Property aufruf As String = ""
    Property darstellen As Boolean = False
    Property funktion As String = ""
    Property slotnr As Int16 = 0
    Property layer As New clsLayerPres
    Property refresh As Boolean = False
    Sub setEmpty()
        canvas.Children.Clear()
        image = New Image
        leeresbild(image)
        canvas.Children.Add(image)
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
            l("fehler in leeresbild: " & aufruf & " /// " & ex.ToString)
        End Try
    End Sub
    Public Function BildGenaufrufMAPserver(mapfile As String, domain As String, amap As clsMapSpec) As String
        Try
            Dim sb As New Text.StringBuilder
            sb.Append(domain)
            sb.Append("/cgi-bin/mapserv70/mapserv.exe?mode=map&map=")
            sb.Append(mapfile.Replace("\", "/").Replace("d:", ""))
            sb.Append("&mapsize=" & amap.aktcanvas.w & "+" & amap.aktcanvas.h)
            sb.Append("&ts=" & Format(Now, "yyyyMMddhhmmss"))

            sb.Append("&mapext=" &
                      CInt(amap.aktrange.xl) & "+" &
                      CInt(amap.aktrange.yl) & "+" &
                      CInt(amap.aktrange.xh) & "+" &
                      CInt(amap.aktrange.yh & "+"))
            Dim a$ = sb.ToString
            l("clsaufrufgenerator: genaufruf " & a)
            a = sb.ToString
            l(String.Format("clsaufrufgenerator: genaufrufkomplett{0}{1}", vbCrLf, a))
            sb = Nothing
            aufruf = a
            Return a
        Catch ex As Exception
            nachricht("Fehler genaufruf: " & ex.ToString)
            Return "Fehler"
        End Try
    End Function
End Class
