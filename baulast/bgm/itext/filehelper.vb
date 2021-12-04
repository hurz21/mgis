Imports System.IO
Public Class filehelper
    '' <summary>
    ''' This class contains directory helper method(s).
    ''' </summary>


    ''' <summary>
    ''' This method starts at the specified directory.
    ''' It traverses all subdirectories.
    ''' It returns a List of those directories.
    ''' </summary>
    Public Shared Function GetFilesRecursive(ByVal initial As String, filter As String) As List(Of String)
        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(initial)

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string
            Dim dir As String = stack.Pop
            Try
                ' Add all immediate file paths
                result.AddRange(Directory.GetFiles(dir, filter))

                ' Loop through all subdirectories and add them to the stack.
                Dim directoryName As String
                For Each directoryName In Directory.GetDirectories(dir)
                    stack.Push(directoryName)
                Next

            Catch ex As Exception
            End Try
        Loop

        ' Return the list
        Return result
    End Function


End Class
