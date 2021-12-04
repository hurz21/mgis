Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Security.Cryptography


Module meineExtensionsIsnullorempty
    <Runtime.CompilerServices.Extension()> _
    Public Function IsNothingOrEmpty(ByRef dt As DataTable) As Boolean
        Dim result As Boolean = (dt Is Nothing)
        If Not result Then result = dt.Rows.Count = 0
        Return result
    End Function
    <Runtime.CompilerServices.Extension()> _
    Public Function IsNothingOrEmpty(ByRef text As String) As Boolean
        Return String.IsNullOrEmpty(text)

    End Function
    <Runtime.CompilerServices.Extension()> _
    Public Function IsNothingOrEmpty(ByRef icoll As ICollection) As Boolean
        Return icoll Is Nothing Or icoll.Count = 0
    End Function

    ''' <summary> 
    ''' <para>Creates a log-string from the Exception.</para>
    ''' <para>The result includes the stacktrace, innerexception et cetera, separated by <seealso cref="Environment.NewLine"/>.</para>
    ''' </summary>
    ''' <param name="ex">The exception to create the string from.</param>
    ''' <param name="additionalMessage">Additional message to place at the top of the string, maybe be empty or null.</param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension()> _
    Public Function ToLogString(ByVal ex As Exception, ByVal additionalMessage As String) As String
        Dim msg As New StringBuilder()

        If Not String.IsNullOrEmpty(additionalMessage) Then
            msg.Append(additionalMessage)
            msg.Append(Environment.NewLine)
        End If

        If ex IsNot Nothing Then
            Try
                Dim orgEx As Exception = ex

                msg.Append("Exception:")
                msg.Append(Environment.NewLine)
                While orgEx IsNot Nothing
                    msg.Append(orgEx.Message)
                    msg.Append(Environment.NewLine)
                    orgEx = orgEx.InnerException
                End While

                If ex.Data IsNot Nothing Then
                    For Each i As Object In ex.Data
                        msg.Append("Data :")
                        msg.Append(i.ToString())
                        msg.Append(Environment.NewLine)
                    Next
                End If

                If ex.StackTrace IsNot Nothing Then
                    msg.Append("StackTrace:")
                    msg.Append(Environment.NewLine)
                    msg.Append(ex.StackTrace.ToString())
                    msg.Append(Environment.NewLine)
                End If

                If ex.Source IsNot Nothing Then
                    msg.Append("Source:")
                    msg.Append(Environment.NewLine)
                    msg.Append(ex.Source)
                    msg.Append(Environment.NewLine)
                End If

                If ex.TargetSite IsNot Nothing Then
                    msg.Append("TargetSite:")
                    msg.Append(Environment.NewLine)
                    msg.Append(ex.TargetSite.ToString())
                    msg.Append(Environment.NewLine)
                End If

                Dim baseException As Exception = ex.GetBaseException()
                If baseException IsNot Nothing Then
                    msg.Append("BaseException:")
                    msg.Append(Environment.NewLine)
                    msg.Append(ex.GetBaseException())
                End If
            Finally
            End Try
        End If
        Return msg.ToString()
    End Function
End Module
