Imports System.Management
Public Class clsGetComputerID
    Public Shared Function getMacAddress() As String
        Dim cpuID As String = String.Empty
        Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        For Each mo As ManagementObject In moc
            If (cpuID = String.Empty And CBool(mo.Properties("IPEnabled").Value) = True) Then
                cpuID = mo.Properties("MacAddress").Value.ToString()
            End If
        Next
        Return cpuID
    End Function

    Public Shared Function getCPU_ID() As String

        Dim cpuID As String = String.Empty
        Dim mc As ManagementClass = New ManagementClass("Win32_Processor")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        For Each mo As ManagementObject In moc
            If (cpuID = String.Empty) Then
                cpuID = mo.Properties("ProcessorId").Value.ToString()
            End If
        Next
        Return cpuID
    End Function
    'Shared Function getMacAddresses() As String
    '    Dim summeString As String : Dim a() As String
    '    Try
    '        summeString = MakeMacSumme()
    '        ReDim a(summeString.Count - 1)

    '        For i = 0 To summeString.Count - 1

    '        Next
    '        a = summeString.Split(""c)
    '        Array.Sort(a)
    '        l(a.ToString)
    '    Catch ex As Exception
    '        l("Fehler in getMacAddresses: " & ex.ToString())
    '    End Try
    'End Function

    'Private Shared Function MakeMacSumme() As String
    '    Dim nics() As System.Net.NetworkInformation.NetworkInterface
    '    Dim summe As String = "" : Dim temp As String
    '    Try
    '        nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
    '        For i = 0 To nics.Count - 1
    '            temp = nics(i).GetPhysicalAddress.ToString
    '            'If nics(i).NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.w Then
    '            If ((nics(i).NetworkInterfaceType.ToString.ToLower.Contains("ethernet") And nics(i).IsReceiveOnly) Or
    '                nics(i).NetworkInterfaceType.ToString.ToLower.Contains("wireless") Then
    '                summe = summe & "" & temp & "_"
    '            End If
    '        Next
    '        l("MakeMacSumme: !" & summe)
    '        Return summe
    '    Catch ex As Exception
    '        l("Fehler in MakeMacSumme: " & ex.ToString())
    '    End Try
    'End Function
End Class
