Imports IWshRuntimeLibrary
Public Class clsShortcut
    Shared Function MakeShortcut(ByVal File As String,
                          ByVal Name As String, iconlocation As String) As Boolean

        'clsShortcut.MakeShortcut("c:\ptest\PL\PL_bestand.exe","ParadigmaLight.lnk" )
        'clsShortcut.MakeShortcut("\\w2gis02\gdvell\apps\pl\plstart.bat", "ParadigmaLight.lnk","\\w2gis02\gdvell\apps\PL\pl.ico")
        'MsgBox(" MakeShortcut ---------------------- anfang")
        clsTools.l("MakeShortcut.............")
        Dim shell As New IWshRuntimeLibrary.IWshShell_Class
        Try

            clsTools.l("File " & File)
            clsTools.l("Name " & Name)
            clsTools.l("iconlocation " & iconlocation)
            Dim WshShell As WshShellClass = New WshShellClass
            Dim desktop As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            'Dim shortCut As IWshRuntimeLibrary.IWshShortcut = DirectCast(shell.CreateShortcut(IO.Path.Combine(desktop, Name)),
            '    IWshRuntimeLibrary.IWshShortcut)

            Dim shortCut As IWshRuntimeLibrary.IWshShortcut = CType(WshShell.CreateShortcut(desktop & "\" & Name & ".lnk"), IWshRuntimeLibrary.IWshShortcut)

            ' File = "C:\Users\hurz\Desktop\sicherheit.bat"
            shortCut.TargetPath = File
            shortCut.IconLocation = iconlocation & ",0"
            Dim iconLoc As String = shortCut.IconLocation '; // <- example: "c:\icon.ico,0"
            clsTools.l("iconLoc " & iconLoc)
            'shortCut.WorkingDirectory= 
            shortCut.Save()
            Return True
            MsgBox(" MakeShortcut ---------------------- ende")
            Return True
        Catch ex As Exception
            MsgBox("Fehler in MakeShortcut: " & ex.ToString())
            clsTools.l("Fehler in MakeShortcut: " & ex.ToString())
            Return False
        End Try
    End Function

End Class
