Imports System.Reflection
Imports System
<Obfuscation(Feature:="renaming", Exclude:=True)>
Class BoundObject
    Public Sub showMessage(aidstring As String, coordstring As String)
        Static fensterzaehler As Integer
        'MsgBox("1 " & aidstring & "," & coordstring)
        fensterzaehler += 1 : If fensterzaehler = 5 Then fensterzaehler = 1
        'BoundObject._fensterzaehler = fensterzaehler
        'Debug.Print("ladevorgangAbgeschlossen = " & ladevorgangAbgeschlossen)
        Debug.Print("aidstring = " & aidstring)
        Debug.Print("coordstring = " & coordstring)
        Dim KoordinateKLickpt As Point
        Dim KoordinateKLickpt2 As New Point?
        Dim a() As String = coordstring.Split(","c)
        Dim javascript As String
        Debug.Print("coordstring2 = " + coordstring)
        Application.Current.Dispatcher.BeginInvoke(
                Sub()
                    Try
                        Debug.Print("coordstring3 = " + coordstring)
                        KoordinateKLickpt.X = CDbl(a(0))
                        KoordinateKLickpt.Y = CDbl(a(1))
                        KoordinateKLickpt2 = KoordinateKLickpt ' javascript:Datenabfrage(173, 1, 163)
                        javascript = " javascript:Datenabfrage(" & aidstring & ")"
                        Debug.Print("coordstring4 = " + coordstring)
                        'MsgBox("2 " & aidstring & "," & coordstring)
                        clsMiniMapTools.handleMouseDownImagemap(KoordinateKLickpt2, javascript, fensterzaehler)
                    Catch ex As Exception
                        Debug.Print("coordstring5 = " + coordstring)
                        Debug.WriteLine(ex.ToString())
                    End Try
                End Sub)
    End Sub
    'Public Sub ritebutton(sender As String)
    '    MessageBox.Show(sender)
    '    Dim aidstring As String = TryCast(sender, String)
    '    Dim KoordinateKLickpt As Point
    '    '     MessageBox.Show("Browser Engine: " & browserEngine & ", Url:" & url)
    '    If aidstring.Length < 10 Then
    '    Else
    '        Dim KoordinateKLickpt2 As New Point?
    '        KoordinateKLickpt2 = KoordinateKLickpt ' javascript:Datenabfrage(173, 1, 163)
    '        Dim javascript As String = " javascript:Datenabfrage(" & aidstring & ")"
    '        'clsMiniMapTools.handleMouseDownImagemap(KoordinateKLickpt2, javascript)
    '    End If
    'End Sub

End Class




