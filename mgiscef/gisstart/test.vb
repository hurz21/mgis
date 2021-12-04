Imports System.Net
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
'https://stackoverflow.com/questions/2066489/how-can-you-add-a-certificate-to-webclient-c
'https://stackoverflow.com/questions/9837982/how-to-call-web-service-using-client-certificates-mutual-authentication
'https://dotnetcodr.com/2016/01/18/using-client-certificates-in-net-part-3-installing-the-client-certificate/
Public Class WebClient2
    Inherits System.Net.WebClient

    Private _ClientCertificates As New System.Security.Cryptography.X509Certificates.X509CertificateCollection
    Public ReadOnly Property ClientCertificates() As System.Security.Cryptography.X509Certificates.X509CertificateCollection
        Get
            Return Me._ClientCertificates
        End Get
    End Property
    Protected Overrides Function GetWebRequest(ByVal address As System.Uri) As System.Net.WebRequest
        Dim R = MyBase.GetWebRequest(address)
        If TypeOf R Is HttpWebRequest Then
            Dim WR = DirectCast(R, HttpWebRequest)
            If Me._ClientCertificates IsNot Nothing AndAlso Me._ClientCertificates.Count > 0 Then
                WR.ClientCertificates.AddRange(Me._ClientCertificates)
            End If
        End If
        Return R
    End Function


    'https://dotnetcodr.com/2015/06/11/https-and-x509-certificates-in-net-part-5-validating-certificates-in-code/
    Public Sub SurroundingSub()
        Dim cert As X509Certificate2 = New X509Certificate2("C:\Users\feinen_j\Desktop\cer\buergergis.kreis-offenbach.de.cer")
        Dim expirationDate As String = cert.GetExpirationDateString()
        Dim issuer As String = cert.Issuer()
        Dim effectiveDateString As String = cert.GetEffectiveDateString()
        Dim nameInfo As String = cert.GetNameInfo(X509NameType.SimpleName, True)
        Dim hasPrivateKey As Boolean = cert.HasPrivateKey
        Console.WriteLine(cert.GetName())
        Console.WriteLine(cert.IssuerName())
        Console.WriteLine(cert.GetName())
        Console.WriteLine(cert.GetName())
        Console.WriteLine(expirationDate)
        Console.WriteLine(issuer)
        Console.WriteLine(effectiveDateString)
        Console.WriteLine(nameInfo)
        Console.WriteLine(hasPrivateKey)
        Dim chain As X509Chain = New X509Chain()
        Dim chainPolicy As X509ChainPolicy = New X509ChainPolicy() With {
            .RevocationMode = X509RevocationMode.Online,
            .RevocationFlag = X509RevocationFlag.EntireChain
        }
        chain.ChainPolicy = chainPolicy

        If Not chain.Build(cert) Then

            For Each chainElement As X509ChainElement In chain.ChainElements

                For Each chainStatus As X509ChainStatus In chainElement.ChainElementStatus
                    Console.WriteLine(chainStatus.StatusInformation)
                Next
            Next
        End If
    End Sub
End Class
