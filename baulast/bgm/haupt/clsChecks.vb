Public Class clsChecks
    Friend Shared Sub vollstaendig(pfad As String)
        Dim rootdir As String = pfad
        Dim temp As String = ""
        Dim gemarkung As String = ""
        'update " & tools.srv_schema & "." & tools.srv_tablename & " set tiff2='fkat/baulasten/' || trim(gemarkung) || '/' || trim(jahr_blattnr) || '.tiff'
        Dim sb As New Text.StringBuilder
        getallTiffsinDB(temp, fstREC.mydb, "select * from " & tools.srv_schema & "." & tools.srv_tablename & " order by gemcode")
        sb.AppendLine("Folgende TIFFs fehlen als Dateien")
        Dim blattnr, datei As String
        Dim fi As IO.FileInfo
        For i = 0 To fstREC.dt.Rows.Count - 1
            'tiff = clsDBtools.fieldvalue(fstREC.dt.Rows(i).Item("tiff2"))
            blattnr = clsDBtools.fieldvalue(fstREC.dt.Rows(i).Item("jahr_blattnr"))
            gemarkung = clsDBtools.fieldvalue(fstREC.dt.Rows(i).Item("gemarkung"))
            datei = srv_unc_path & "\fkat\baulasten\" & gemarkung.Trim & "\" & blattnr.Trim & ".tiff"
            fi = New IO.FileInfo(datei)
            If fi.Exists Then
                Debug.Print("")
            Else
                sb.AppendLine(datei)
            End If
        Next
        Dim ousgasbe As String = tools.baulastenoutDir & "\tiffpruefungnormal" & Now.ToString("yyyyMMddhhmm") & ".txt"
        IO.File.WriteAllText(ousgasbe, sb.ToString)
        Process.Start(ousgasbe)
    End Sub
End Class
