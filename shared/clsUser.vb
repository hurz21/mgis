Public Class clsUser
    Property userid As Integer = 0
    Property username As String = ""
    Property ADgruppenname As String = ""
    Property favogruppekurz As String = "intranet"
    Property favogruppeLang As String = "Standarduser"
    Property UmweltUntergruppe As String = ""
    Property istalbberechtigt As Boolean = False
    Property istParadigmaAdmin As Boolean = False
    Property userLayerAid As Integer = 0 ' nur für paradigma
    Property paradigmaAbteilung As String = ""
    Property PL_UserNr As Integer = 0
    Property macAdress As String = ""
    Property cpuID As String = ""
    Property EmailAdress As String = ""
    Property EmailPW As String = ""
    Property EmailServer As String = ""
    Property domain As String = ""

    Public Property MachineName As String = ""
    Public Property nick As String = ""
    Public Property rites As String = "0"
    Public Property ichNutzeDenGisserver As Boolean = False
    Public Property proxy As String = ""

    Sub New()
        favogruppekurz = "intranet"
        favogruppeLang = "Standarduser"
    End Sub
    Function myString() As String
        Dim sb As New Text.StringBuilder
        Try
            l(" MOD mystring anfang")
            sb.Append(" userid " & userid & Environment.NewLine)
            sb.Append(" username " & username & Environment.NewLine)
            sb.Append(" ADgruppenname " & ADgruppenname & Environment.NewLine)
            sb.Append(" favogruppekurz " & favogruppekurz & Environment.NewLine)
            sb.Append(" favogruppeLang " & favogruppeLang & Environment.NewLine)
            sb.Append(" UmweltUntergruppe " & UmweltUntergruppe & Environment.NewLine)
            sb.Append(" istalbberechtigt " & istalbberechtigt & Environment.NewLine)
            sb.Append(" userLayerAid " & userLayerAid & Environment.NewLine)
            sb.Append(" paradigmaAbteilung " & paradigmaAbteilung & Environment.NewLine)
            sb.Append(" PL_UserNr " & PL_UserNr & Environment.NewLine)
            sb.Append(" mac " & macAdress & Environment.NewLine)
            sb.Append(" domain " & domain & Environment.NewLine)
            sb.Append(" EmailAdress " & EmailAdress & Environment.NewLine)
            sb.Append(" EmailPW " & EmailPW & Environment.NewLine)
            sb.Append(" EmailServer " & EmailServer & Environment.NewLine)
            sb.Append(" proxy " & proxy & Environment.NewLine)
            sb.Append(" ichNutzeDenGisserver " & ichNutzeDenGisserver & Environment.NewLine)

            sb.Append(" MachineName " & MachineName & Environment.NewLine)
            sb.Append(" nick " & nick & Environment.NewLine)
            l(" MOD mystring ende")
            Return sb.ToString
        Catch ex As Exception
            l("Fehler in mystring: ", ex)
            Return ""
        End Try
    End Function
End Class
