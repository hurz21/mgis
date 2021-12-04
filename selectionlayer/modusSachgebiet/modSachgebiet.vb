Imports userlayer2Postgis

Module modSachgebiet
    Dim raumbezugillegal As String =
        " (SELECT r.RAUMBEZUGSID, r.TYP, r.TITEL, r.SEKID, r.ABSTRACT, r.RECHTS, r.HOCH, r.XMIN, r.XMAX, r.YMIN, r.YMAX, r.FREITEXT, r.ISMAPENABLED, rv.RAUMBEZUGSID AS ridRV, rv.VORGANGSID, rv.STATUS, " &
"             r.FLAECHEQM, r.LAENGEM, r.MITETIKETT, v.SACHGEBIETNR, v.SACHGEBIETSTEXT, i.STATUS AS statusIllegbau, i.GEBIET" &
" FROM   dbo.RAUMBEZUG2VORGANG AS rv LEFT OUTER JOIN" &
"            dbo.RAUMBEZUG AS r ON rv.RAUMBEZUGSID = r.RAUMBEZUGSID LEFT OUTER JOIN" &
"             dbo.t43 AS v ON rv.VORGANGSID = v.VORGANGSID LEFT OUTER JOIN" &
"             dbo.ILLEGBAU AS i ON v.VORGANGSID = i.VORGANGSID ) "
    Friend Function getsachgebiet(modus As String) As String
        Try
            Return modus.ToLower.Replace("sachgebiet", "")
        Catch ex As Exception
            l("fehler in getsachgebiet: " & ex.ToString)
            Return ""
        End Try
    End Function

    Friend Function exekuteSachgebiet(sachgebiet As String, aktbox As clsRange, ByRef returnstring As String) As Integer
        ReDim ebenen(0) ': ebenen(0) = sachgebiet
        Dim erfolg As Integer : Dim summe As String = ""
        glob2.nachricht("point_shpfile_erzeugen ============================================================ vor")
        erfolg = modPG.pgDBtableAnlegen(summe)
        l("erfolg: " & erfolg)
        l("summe: " & summe)
        If erfolg < 1 Then Return 0
        '  erfolg = pgDBDatenanlegenAnlegen(myglobalz.Oracle_MYDB)
        If erfolg < 1 Then Return 0
        'modOracle.getDTOracle("Select * from raumbezugillegbau where sachgebietnr='" & sachgebiet & "' order by vorgangsid", dtRBplus)

        Dim anz As Integer
        anz = clsSQLS.getDTSQLS("Select * from " & raumbezugillegal & " vi where sachgebietnr='" & sachgebiet & "' order by vorgangsid", dtRBplus)
        Dim dtRBpolygon As New DataTable
        modPG.doRBschleife(dtRBplus, dtRBpolygon, sachgebiet, returnstring)
        l(returnstring)
        glob2.nachricht("point_shpfile_erzeugen ============================================================ ende")
        Return 1
    End Function
End Module
