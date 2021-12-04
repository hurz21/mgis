Public Class clsSendmailTools
    Private Shared sendmailroot As String = strGlobals.localDocumentCacheRoot & "\sendmail\"
    Private Shared datei As String = sendmailroot & "\" & GisUser.nick & "_history.txt"
    Public Shared empfaengerListeZuletzt As New List(Of String)
    Public Shared empfaengerListeParadigma As New List(Of String)
    Shared Sub getEmailAccountFromIni(ByRef gisuser As clsUser)
        gisuser.EmailAdress = myglobalz.userIniProfile.WertLesen("email", "konto")
        gisuser.EmailPW = myglobalz.userIniProfile.WertLesen("email", "kontopw")
        gisuser.EmailServer = myglobalz.userIniProfile.WertLesen("email", "mailserver")
        gisuser.proxy = myglobalz.userIniProfile.WertLesen("email", "proxy")
        Dim test As String = myglobalz.userIniProfile.WertLesen("email", "ichNutzeDenGisserver")
        If test = "true" Then
            gisuser.ichNutzeDenGisserver = True
        Else
            gisuser.ichNutzeDenGisserver = False
        End If
    End Sub
    Shared Sub saveEmpfaengerHistory(empfaenger As String)
        Dim alles As String = ""
        Dim fi As IO.FileInfo
        Try
            IO.Directory.CreateDirectory(sendmailroot)
            fi = New IO.FileInfo(datei)
            If Not fi.Exists Then
                fi = Nothing
            Else
                fi = Nothing
                alles = IO.File.ReadAllText(datei)
            End If
            alles = empfaenger & Environment.NewLine & alles
            IO.File.WriteAllText(datei, alles)
        Catch ex As Exception
            l("Fehler in saveEmpfaengerHistory: ", ex)
        End Try
    End Sub

    Friend Shared Function GetEmpfaengerListeParadigma(vid As Integer) As List(Of String)
        Dim neu As New List(Of String)
        Try
            l(" MOD GetEmpfaengerListeParadigma anfang")
            neu = modParadigma.emailAdresses4VID(vid)
            l(" MOD GetEmpfaengerListeParadigma ende")
            Return neu
        Catch ex As Exception
            l("Fehler in GetEmpfaengerListeParadigma: " & ex.ToString())
        End Try
    End Function

    Friend Shared Function getAlteEmpfaengerlisteZuletzt() As List(Of String)
        Dim nl As New List(Of String)
        Dim alles As String = ""
        Dim fi As IO.FileInfo
        Try
            IO.Directory.CreateDirectory(sendmailroot)
            fi = New IO.FileInfo(datei)
            If Not fi.Exists Then
                Return Nothing
            Else
                fi = Nothing
                alles = IO.File.ReadAllText(datei)
                Dim a() As String = alles.Split(CType(vbCrLf, Char()))
                For i = 0 To a.Length - 1
                    If a(i) <> String.Empty Then nl.Add(a(i))
                Next
                Return nl
            End If
        Catch ex As Exception
            l("Fehler in getAlteEmpfaengerliste: ", ex)
            Return Nothing
        End Try
    End Function
End Class
