'195.78.244.4

'85.25.145.22:44400
'85.25.149.184:27961
'212.6.108.249:27972
'212.65.13.87:27960




Public Class clsRange
    Sub l(text As String)
        l(text)
    End Sub
    Public Sub addBuffer(pufferinmeter As Double)
        Try
            xl = xl - pufferinmeter
            xh = xh + pufferinmeter
            yl = yl - pufferinmeter
            yh = yh + pufferinmeter
        Catch ex As Exception
        End Try
    End Sub
    Public Function addBufferToNewRange(pufferinmeter As Double) As clsRange
        Dim newRange As New clsRange
        Try
            newRange.xl = xl - pufferinmeter
            newRange.xh = xh + pufferinmeter
            newRange.yl = yl - pufferinmeter
            newRange.yh = yh + pufferinmeter
            Return newRange
        Catch ex As Exception
            Return newRange
        End Try
    End Function

    Private _BBOXtitel As String
    Public Property Titel() As String
        Get
            Return _BBOXtitel
        End Get
        Set(ByVal value As String)
            If _BBOXtitel = value Then
                Return
            End If
            _BBOXtitel = value
        End Set
    End Property
    Public BBOX As String
    Public Property yh() As Double
    Public Property yl() As Double
    Public Property xh() As Double
    Public Property xl() As Double
    Public xcenter As Double
    Public ycenter As Double

    Public Sub CalcCenter()
        xcenter = xl + (xdif() / 2)
        ycenter = yl + (ydif() / 2)
    End Sub
    Public boundboxtext As String
    Public Function xdif() As Double
        Return xh - xl
    End Function
    Public Function ydif() As Double
        Return yh - yl
    End Function
    Private Sub normalerBoxsplit()
        Dim a$()
        a = BBOX.Split(","c)
        xl = CDbl(Val(a(0)))
        xh = CDbl(Val(a(1)))
        yl = CDbl(Val(a(2)))
        yh = CDbl(Val(a(3)))
    End Sub


    Public Function bbox_split() As Boolean
        Try
            If BBOX.Length < 1 Then Return False
            If BBOX.ToLower.StartsWith("box") Then
                postgisBOX2range(BBOX)
                Return True
            Else
                normalerBoxsplit()
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function quotient() As Double
        Dim nenner As Double
        If xdif() = 0 Then
            nenner = 1
        Else
            nenner = xdif()
        End If
        Return ydif() / nenner
    End Function
    Public Function portrait() As Boolean
        If ydif() > xdif() Then
            portrait = True
        Else
            portrait = False
        End If
    End Function
    Public Function inside(ByVal mypoint As System.Windows.Point) As Boolean
        'point inside range ?
        If mypoint.X > xl And
         mypoint.X < xh And
         mypoint.Y > yl And
         mypoint.Y < yh Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Overrides Function toString() As String
        Dim d$ = vbCrLf : Dim Show$ = d$
        Show$ &= "-------------------rangeausgabe--------------------------" & d$
        Show$ &= String.Format("portrait: {0}{1}", portrait(), d$)
        Show$ &= String.Format("quotient: {0}{1}", quotient(), d$)
        Show$ &= String.Format("xDif: {0}{1}", xdif(), d$)
        Show$ &= String.Format("yDif: {0}{1}", ydif(), d$)
        Show$ &= String.Format("xcenter: {0}{1}", xcenter, d$)
        Show$ &= String.Format("ycenter: {0}{1}", ycenter, d$)
        Show$ &= String.Format("xl: {0}{1}", xl, d$)
        Show$ &= String.Format("xh: {0}{1}", xh, d$)
        Show$ &= String.Format("yl: {0}{1}", yl, d$)
        Show$ &= String.Format("yh: {0}{1}", yh, d$)
        Show$ &= String.Format("bounboxtext: {0}{1}", boundboxtext, d$)
        Show$ &= "---------------------------------------------" & d$
        Return Show$
    End Function

    Public Function istBrauchbar() As Boolean
        If xl < 10 Then Return False
        If yl < 10 Then Return False
        If xh < 10 Then Return False
        If yh < 10 Then Return False
        Return True
    End Function

    Public Function expand(ByVal newBox As clsRange) As Boolean
        Dim summe As Double = xl + xh + yl + yh
        Try
            If Not newBox Is Nothing Then
                If summe < 1 Then
                    xl = newBox.xl
                    xh = newBox.xh
                    yl = newBox.yl
                    yh = newBox.yh
                Else
                    If newBox.xl < xl Then xl = newBox.xl
                    If newBox.xh > xh Then xh = newBox.xh
                    If newBox.yl < yl Then yl = newBox.yl
                    If newBox.yh > yh Then yh = newBox.yh
                End If

            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function rangekopieren(ByVal quelle As clsRange, ByRef ziel As clsRange) As Boolean
        Try
            If Not quelle Is Nothing Then
                ziel.xl = quelle.xl
                ziel.xh = quelle.xh
                ziel.yl = quelle.yl
                ziel.yh = quelle.yh
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function rangekopierenVon(ByVal quellenRange As clsRange) As Boolean
        Try
            If Not quellenRange Is Nothing Then
                xl = quellenRange.xl
                xh = quellenRange.xh
                yl = quellenRange.yl
                yh = quellenRange.yh
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function postgisBOX2range(ByVal box As String) As Boolean
        Try
            If box Is Nothing Then Return False
            If box = String.Empty Then Return False
            'vorsicht bei punkten - die min und max sind gleich
            Dim a(), lu, ro As String
            Dim neubox As String = box          'BOX(483463.4446 5538926.784,483844.154 5539296.5635)
            neubox = neubox.Replace("BOX(", "") '483463.4446 5538926.784,483844.154 5539296.5635)
            neubox = neubox.Replace(")", "")    '483463.4446 5538926.784,483844.154 5539296.5635                                              
            a = neubox.Split(","c)              '483463.4446 5538926.784
            lu = a(0) : ro = a(1)
            a = lu.Split(" "c)
            xl = CDbl(a(0).Replace(".", ","))
            yl = CDbl(a(1).Replace(".", ","))
            a = ro.Split(" "c)
            xh = CDbl(a(0).Replace(".", ","))
            yh = CDbl(a(1).Replace(".", ","))
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function point2range(ByVal oldpoint As myPoint, ByVal radius As Double) As Boolean
        If oldpoint.X < 1 Then Return Nothing
        Try
            xl = oldpoint.X - radius
            xh = oldpoint.X + radius
            yl = oldpoint.Y - radius
            yh = oldpoint.Y + radius
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Sub New()
        xl = 0
        xh = 0
        yl = 0
        yh = 0
    End Sub

    Sub clear()
        xl = 0
        xh = 0
        yl = 0
        yh = 0
        Titel = ""
    End Sub

    Friend Function isSameAs(aktrange As clsRange) As Boolean
        Try
            l("clsRange isSameAs---------------------- anfang")
            If aktrange.xl <> xl Then Return False
            If aktrange.xh <> xh Then Return False
            If aktrange.yl <> yl Then Return False
            If aktrange.yl <> yl Then Return False
            Return True
            l("isSameAs---------------------- ende")
        Catch ex As Exception
            l("Fehler in clsRange.isSameAs: " & ex.ToString())
            Return False
        End Try
    End Function
End Class
