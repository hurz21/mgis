Imports System.Data
Imports Npgsql
Module offlinetools
    'Property sgColl As New List(Of sachgebietsCombo)
    Dim myconn As New NpgsqlConnection
    Private dt As DataTable
    Public enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252)

    Friend Sub movealleLegs(legenden As List(Of clsLegendenItem))
        Dim quellRoot, zielroot, qdatei, zdatei, legroot, sachgebiet, ebenenprosa As String
        Dim serror As String = ""
        quellRoot = tools.serverUNC & "\fkat\"
        Dim aid As Integer
        zielroot = tools.serverUNC & "\nkat\aid\"
        Try
            dt = getDT("SELECt * FROM  " & stamm_tabelle, tools.dbServername, "webgiscontrol")
            For i = 0 To dt.Rows.Count - 1
                aid = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("aid")))
                sachgebiet = clsDBtools.fieldvalue(dt.Rows(i).Item("stdsg"))
                ebenenprosa = clsDBtools.fieldvalue(dt.Rows(i).Item("ebene"))

                legroot = quellRoot & sachgebiet & "\" & ebenenprosa & "\images\"
                Dim fd As New IO.DirectoryInfo(legroot)
                If fd.Exists Then
                    Console.WriteLine(dt.Rows(i).Item("aid"))
                    Dim fileEntries As String() = IO.Directory.GetFiles(legroot)
                    For j = 0 To fileEntries.Count - 1
                        Dim fo As New IO.FileInfo(fileEntries(j))
                        zdatei = zielroot & aid & "\legende\"
                        IO.Directory.CreateDirectory(zdatei)
                        zdatei = zdatei & fo.Name
                        If fo.Name.ToLower = "thumbs.db" Then
                            Continue For
                        End If
                        fo.CopyTo(zdatei, True)
                    Next
                Else
                    Continue For
                End If
            Next
        Catch ex As Exception
            l("fehler ind checkAllDoku " & ex.ToString)
        End Try

    End Sub

    Friend Sub copyNatlandPDF()
        Dim erroroprot As String
        Dim quellRoot, zielroot, qdatei, zdatei, legroot, dateispalte, rootdir, titel2, titel1, mf, layerstring As String
        Dim serror As String = ""
        quellRoot = tools.serverUNC & ""
        Dim aid, sid, typ As Integer
        Dim olddatei As String = ""
        zielroot = tools.serverUNC & "\nkat\aid\"
        Try
            dt = getDT("SELECt * FROM  natur.nat_naturlandschaft_imap ", tools.dbServername, "postgis20")
            For i = 0 To dt.Rows.Count - 1

                titel1 = (clsDBtools.fieldvalue(dt.Rows(i).Item("art"))).Trim.Replace("'", "")
                titel2 = (clsDBtools.fieldvalue(dt.Rows(i).Item("name"))).Trim.Replace("'", "") & " (" &
                    (clsDBtools.fieldvalue(dt.Rows(i).Item("ausgewiesen"))).Trim & ")"
                dateispalte = (clsDBtools.fieldvalue(dt.Rows(i).Item("verordnung"))).Trim & ".pdf"
                rootdir = "nkat/aid/160/pdfdateien"
                typ = 2
                aid = 160
                sid = 25
                If olddatei = dateispalte Then Continue For
                aid = dbeditTools.zeileEinfuegen("public.pdfdateien", "titelspalte1,titelspalte2,rootdir,dateispalte,sid,aid,typ",
                                                 "'" & titel1 & "'" & ", " &
                                                 "'" & titel2 & "'" & ", " &
                                                 "'" & rootdir & "'" & ", " &
                                                  "'" & dateispalte & "'" & ", " &
                                                    sid & ", " &
                                                     aid & ", " &
                                                      typ & " ",
                                                 " RETURNING id", tools.dbServername, "webgiscontrol")
                olddatei = dateispalte

            Next
            MsgBox(erroroprot)
        Catch ex As Exception
            l("fehler ind checkAllDoku " & ex.ToString)
        End Try
    End Sub
    Friend Sub movealleMapfiles()
        Dim erroroprot As String
        Dim quellRoot, zielroot, qdatei, zdatei, legroot, sachgebiet, ebenenprosa, pfad, ebene, mf, layerstring As String
        Dim serror As String = ""
        quellRoot = tools.serverUNC & ""
        Dim aid As Integer
        zielroot = tools.serverUNC & "\nkat\aid\"
        Try
            dt = getDT("SELECt * FROM  " & stamm_tabelle, tools.dbServername, "webgiscontrol")
            For i = 0 To dt.Rows.Count - 1

                ebene = (clsDBtools.fieldvalue(dt.Rows(i).Item("ebene")))
                pfad = (clsDBtools.fieldvalue(dt.Rows(i).Item("pfad")))
                aid = CInt(clsDBtools.fieldvalue(dt.Rows(i).Item("aid")))
                sachgebiet = clsDBtools.fieldvalue(dt.Rows(i).Item("stdsg"))
                ebenenprosa = clsDBtools.fieldvalue(dt.Rows(i).Item("ebene"))

                qdatei = (quellRoot & pfad & ebene & "_" & "layer" & ".map").Replace("/", "\")
                zdatei = zielroot & aid
                IO.Directory.CreateDirectory(zdatei)
                layerstring = "/nkat/aid/" & aid & "/layer.map"
                zdatei = zdatei & "\layer.map"
                If aid = 111 Then
                    Debug.Print("")
                End If
                Dim fo As New IO.FileInfo(qdatei)
                If fo.Exists Then
                    fo.CopyTo(zdatei, True)
                Else
                    erroroprot = erroroprot & " Fehler in : " & qdatei & Environment.NewLine
                End If

                'headerfile
                Dim header = zielroot & aid
                header = header & "\header.map"
                createHeaderFile(layerstring, header)

                killFile(qdatei)
                killFile(qdatei.Replace("_layer.map", "_header.map"))

                qdatei = (quellRoot & pfad).Replace("/", "\")
                erroroprot = erroroprot & htmsKopieren(qdatei, zielroot & aid)
            Next
            MsgBox(erroroprot)
        Catch ex As Exception
            l("fehler ind checkAllDoku " & ex.ToString)
        End Try
    End Sub

    Private Sub killFile(zdatei As String)
        Try
            Dim fi As New IO.FileInfo(zdatei)
            fi.Delete()
        Catch ex As Exception
            l("fehler ind killFile " & ex.ToString)
        End Try

    End Sub

    Private Function htmsKopieren(maproot As String, zielroot As String) As String
        Dim zdatei As String
        Dim prot As String
        Dim fd As New IO.DirectoryInfo(maproot)
        If Not fd.Exists Then
            Return "htm -verzeichnis exoistier t nicht: " & maproot & Environment.NewLine
        End If
        Dim fileEntries As String() = IO.Directory.GetFiles(maproot, "*.htm*")
        Try

            For j = 0 To fileEntries.Count - 1
                Dim fo As New IO.FileInfo(fileEntries(j))
                IO.Directory.CreateDirectory(zielroot)
                zdatei = zielroot & "\" & fo.Name
                IO.Directory.CreateDirectory(zielroot)
                ' zdatei = zdatei & fo.Name
                If fo.Name.ToLower = "thumbs.db" Then
                    Continue For
                End If
                If fo.Exists Then
                    fo.CopyTo(zdatei, True)
                Else
                    prot = prot & " htm fehlt: " & fo.FullName & Environment.NewLine
                End If

            Next
            Return prot

        Catch ex As Exception
            l("fehler in createMapfilePDF " & ex.ToString)
        End Try
    End Function

    Sub createHeaderFile(layerfile As String, headerfile As String)
        ' /fkat/flurkarte/flurkarte2016/flurkarte2016_header.map"
        'MAP
        'INCLUDE '/inetpub/wwwroot/buergergis/mapfile/header.map',
        'INCLUDE '/fkat/boden/bodentyp/bodentyp_layer.map',,
        'End
        l("in createMapfilePDF--------------------------")
        Try
            Dim sb As New Text.StringBuilder
            sb.AppendLine("MAP")
            sb.AppendLine("INCLUDE '/inetpub/wwwroot/buergergis/mapfile/header.map'")
            sb.AppendLine("INCLUDE '" & layerfile & "'")
            sb.AppendLine("END")
            My.Computer.FileSystem.WriteAllText(headerfile, sb.ToString, False, enc)
            sb = Nothing
        Catch ex As Exception
            l("fehler in createMapfilePDF " & ex.ToString)
        End Try
    End Sub
End Module
