'Imports System.Drawing
Public Class clsRange
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
  Public xl As Double
  Public xh As Double
  Public yl As Double
  Public yh As Double
  Public xcenter As Double
  Public ycenter As Double
  Public boundboxtext As String
  Public Function xdif() As Double
    Return xh - xl
  End Function
  Public Function ydif() As Double
    Return yh - yl
  End Function
  Public Function bbox_split() As Boolean
    Try
      If BBOX.Length < 1 Then Return False
      Dim a$() = BBOX.Split(","c)
      xl = CDbl(Val(a$(0)))
      xh = CDbl(Val(a$(1)))
      yl = CDbl(Val(a$(2)))
      yh = CDbl(Val(a$(3)))
      Return True
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
  Public Function inside(ByVal mypoint As geoPoint) As Boolean
    'point inside range ?
        If mypoint.X > xl And _
             mypoint.X < xh And _
             mypoint.Y > yl And _
             mypoint.Y < yh Then
            Return True
        Else
            Return False
        End If
  End Function
  Public Overrides Function toString() As String
    Dim d$ = vbCrLf : Dim Show$ = d$
    Show$ &= "-------------------rangeausgabe--------------------------" & d$
    Show$ &= "portrait: " & portrait() & d$
    Show$ &= "quotient: " & quotient() & d$
    Show$ &= "xDif: " & xdif() & d$
    Show$ &= "yDif: " & ydif() & d$
    Show$ &= "xcenter: " & xcenter & d$
    Show$ &= "ycenter: " & ycenter & d$
    Show$ &= "xl: " & xl & d$
    Show$ &= "xh: " & xh & d$
    Show$ &= "yl: " & yl & d$
    Show$ &= "yh: " & yh & d$
    Show$ &= "bounboxtext: " & boundboxtext & d$
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
	Public Sub New()
		xl = 0
		xh = 0
		yl = 0
		yh = 0
	End Sub
End Class
