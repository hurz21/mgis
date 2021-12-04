Imports IWshRuntimeLibrary

Public Class clsShortcut

    Shared Function MakeShortcut(ByVal File As String,
                          ByVal Name As String, iconfile As String) As Boolean

        'clsShortcut.MakeShortcut("c:\ptest\PL\PL_bestand.exe","ParadigmaLight.lnk" )

        Dim shell As New IWshShell_Class
        Dim desktop As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim shortCut As IWshShortcut = DirectCast(shell.CreateShortcut(IO.Path.Combine(desktop, Name)),
            IWshShortcut)
        ' File = "C:\Users\hurz\Desktop\sicherheit.bat"
        shortCut.TargetPath = File
        shortCut.IconLocation = iconfile & ",0"
        Dim iconLoc As String = shortCut.IconLocation '; // <- example: "c:\icon.ico,0"
        'shortCut.WorkingDirectory= 
        shortCut.Save()
        Return True
    End Function

End Class
