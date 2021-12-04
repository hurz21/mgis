
Public Class clsCanvas
    Private _titel As String
    Public Property titel() As String
        Get
            Return _titel
        End Get
        Set(ByVal value As String)
            If _titel = value Then
                Return
            End If
            _titel = value
        End Set
    End Property
    Public w As Long
    Public h As Long
    Public Function xcenter() As Long
        Return CLng(w / 2)
    End Function
    Public Function ycenter() As Long
        Return CLng(h / 2)
    End Function
    Public Function portrait() As Boolean
        If h > w Then
            portrait = True
        Else
            portrait = False
        End If
    End Function
    Public Function quotient() As Double
        Dim nenner As Double
        If w = 0 Then
            nenner = 1
        Else
            nenner = w
        End If
        Return h / nenner
    End Function
    Public Overrides Function toString() As String
        Dim d$ = vbCrLf
        Dim Show As New System.Text.StringBuilder
        Show.Append(d$)
        Show.Append(String.Format("quotient: {0}{1}", quotient(), d$))
        Show.Append(String.Format("w: {0}{1}", w, d$))
        Show.Append(String.Format("h: {0}{1}", h, d$))
        Show.Append(String.Format("xcenter: {0}{1}", xcenter(), d$))
        Show.Append(String.Format("ycenter: {0}{1}", ycenter(), d$))
        Show.Append(String.Format("portrait: {0}{1}", portrait(), d$))
        Return Show.ToString
    End Function

    Sub clear()
        w = 0
        h = 0
        titel = ""
    End Sub

End Class
