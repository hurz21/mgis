
Public Class MyFormatter
	Implements IValueConverter

	Public Function Convert(ByVal value As Object, _
	 ByVal targetType As System.Type, _
	 ByVal parameter As Object, _
	 ByVal culture As System.Globalization.CultureInfo) As Object _
	 Implements System.Windows.Data.IValueConverter.Convert

		If parameter IsNot Nothing Then
            If TypeOf value Is Date Then
                If CType(value, Date) = #1:01:01 AM# Or
                   CType(value, Date) = #1/1/0001 12:00:00 AM# Then
                    Return ""
                End If
            End If
            If value Is Nothing Then Return ""
            If value.ToString = "" Then
                Return ""
            End If
            Return Format(value, parameter.ToString())
		End If
		Return value
	End Function

	Public Function ConvertBack(ByVal value As Object, _
		ByVal targetType As System.Type, _
		ByVal parameter As Object, _
		ByVal culture As System.Globalization.CultureInfo) As Object _
		Implements System.Windows.Data.IValueConverter.ConvertBack

		If targetType Is GetType(Date) OrElse targetType Is GetType(Nullable(Of Date)) Then
			If IsDate(value) Then
				Return CDate(value)
			ElseIf value.ToString() = "" Then
				Return Nothing
			Else
				Return Now() 'invalid type was entered so just give a default.
			End If
		ElseIf targetType Is GetType(Decimal) Then
			If IsNumeric(value) Then
				Return CDec(value)
			Else
				Return 0
			End If
		End If
		Return value
	End Function
End Class

