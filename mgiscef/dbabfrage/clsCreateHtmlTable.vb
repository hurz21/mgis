Namespace nsMakeHTML
    Public Class clsCreateHtmlTable
        Friend Shared htmlDateiString As String = ""
        Friend Shared Function createTable(sdorig As List(Of clsSachdaten), titel As String, aid As Integer, grossfont As Integer, kleinfont As Integer, mittelfont As Integer) As String
            Dim summe As New Text.StringBuilder
            Dim sdkopie As New List(Of clsSachdaten)
            Dim numbOfRows As Integer = sdorig.Count
            Dim numberOfColumns As Integer = 2
            Dim t As String = Environment.NewLine
            Try
                l(" MOD ----------createTable------------ anfang")
                Debug.Print(sdorig.Count.ToString)
                sdkopie = nsMakeRTF.kopiereListe(sdorig)
                'kopp
                'summe.Append("<h3>" & titel & "</h2>" & t)
                summe.Append("<table  class='schalter' cellspacing='7' cellpadding='5'>    " & t)
                summe.Append("    <colgroup>" & t)
                summe.Append("     <col width='132px'>" & t)
                summe.Append("     <col width='132px'>" & t)
                summe.Append("     <col width='132px'>" & t)
                summe.Append("     </colgroup>" & t)
                summe.Append("       <tr>" & t)
                summe.Append("  <th colspan ='3'>" & titel & "</th>" & t)
                summe.Append(" </tr>" & t)
                summe.Append("  </table>" & t)

                summe.Append("<table id='tabelleneintraege' class='tabelleneintraege' cellspacing='0' cellpadding='5'>" & t)
                summe.Append("    <colgroup>" & t)
                summe.Append("    <col width ='150'>" & t)
                summe.Append("     <col width='241'> " & t)
                summe.Append(" </colgroup>" & t)
                Dim Wert As String = ""
                For i = 0 To sdorig.Count - 1
                    Wert = sdkopie(i).feldinhalt.Trim
                    If Wert.IsNothingOrEmpty Then Continue For
                    If Wert.ToLower.EndsWith(".pdf") Or
                       Wert.ToLower.EndsWith(".application") Or
                       Wert.ToLower.EndsWith(".jpg") Or
                       Wert.ToLower.EndsWith(".tiff") Or
                       Wert.ToLower.EndsWith(".html") Then
                        Continue For
                    End If
                    If Wert.Contains("http") Then
                        If (Not Wert.ToLower.EndsWith("/.pdf")) Then
                            summe.Append(" <tr>")
                            summe.Append("   <td class='norm'>")
                            summe.Append(sdkopie(i).neuerFeldname.Trim)
                            summe.Append("   </td>")
                            summe.Append("   <td class='alink'>")
                            summe.Append("<a target='_blank' href='" & sdkopie(i).feldinhalt.Trim & "'>" & sdkopie(i).feldinhalt.Trim & "</a>")
                            'summe.Append(sdkopie(i).feldinhalt.Trim & t)
                            summe.Append("   </td>")
                            summe.Append(" </tr>" & t)
                            Continue For
                        End If
                    End If
                    If sdkopie(i).neuerFeldname.Trim.ToLower = "neue_tabelle" Then
                        summe.Append(" <tr>")
                        summe.Append("   <td class='norm'>")
                        summe.Append("") 'sdkopie(i).neuerFeldname.Trim)
                        summe.Append("   </td>")
                        summe.Append("   <td class='norm'><b>")
                        summe.Append(sdkopie(i).feldinhalt.Trim & t)
                        summe.Append("  </b> </td>")
                        summe.Append(" </tr>" & t)
                    Else
                        summe.Append(" <tr>")
                        summe.Append("   <td class='norm'>")
                        summe.Append(sdkopie(i).neuerFeldname.Trim)
                        summe.Append("   </td>")
                        summe.Append("   <td class='feldinhalt'>")
                        summe.Append(sdkopie(i).feldinhalt.Trim & t)
                        summe.Append("   </td>")
                        summe.Append(" </tr>" & t)
                    End If

                Next
                summe.Append(" </table>" & t)
                l(" MOD createTable ende")
                Return summe.ToString
            Catch ex As Exception
                l("Fehler in createTable: " & ex.ToString())
                summe.Append(" </table>" & t)
                Return summe.ToString
                'Return ""
            End Try
        End Function

    End Class

End Namespace
