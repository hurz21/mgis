Public Class selectionTools
    Shared Function getCombo4Sql(ByRef hinweis As String, ByRef result As String, tabelle As String, selectioncol As String) As String()
        l("getCombo4Sql: ")
        Dim innersql As String = ""
        Dim rec() As String
        Try
            l(" MOD getCombo4Sql anfang")
            innersql = "SELECT distinct " & selectioncol & " FROM " & tabelle & " order by  " & selectioncol & ""
            result = clsToolsAllg.getSQL4Http(innersql, "postgis20", hinweis, "getsql") : l(hinweis)
            result = result.Trim
            rec = result.Split("$"c)
            Return rec
            l(" MOD getCombo4Sql ende")
        Catch ex As Exception
            l("Fehler in getCombo4Sql: " & ex.ToString())
            Return rec
        End Try
    End Function
    Shared Sub populateCombobox(rec() As String, combo As ComboBox)
        Dim a() As String
        Try
            l(" MOD ---------------------- anfang")
            For i = 0 To rec.Count - 1
                If rec(i).IsNothingOrEmpty Then Continue For
                a = rec(i).Split("#"c)
                Dim cb As New ComboBoxItem
                cb.Name = "cmbE_" & (i)
                cb.Content = a(1).Trim 'clsString.Capitalize(item.tag.Replace("h_", "Hist. ")) ' kat.ToUpper
                cb.Tag = a(0).Trim
                cb.ToolTip = a(1).Trim
                cb.FontWeight = FontWeights.Normal

                cb.FontFamily = New FontFamily("Arial")

                combo.Items.Add(cb)
            Next
            l(" MOD ---------------------- ende")
        Catch ex As Exception
            l("Fehler in MOD: " & ex.ToString())
        End Try
    End Sub

    Friend Shared Function isSelectionLayerLoaded(nick As String, layersSelected As List(Of clsLayerPres)) As Integer
        Try
            l(" MOD isSelectionLayerLoaded anfang")
            For Each layer As clsLayerPres In layersSelected
                If layer.titel.ToLower = "auswahl: " & nick.ToLower Then
                    Return layer.aid
                End If
            Next
            l(" MOD isSelectionLayerLoaded ende")
            Return 0
        Catch ex As Exception
            l("Fehler in isSelectionLayerLoaded: " & ex.ToString())
            Return 0
        End Try
    End Function

    Shared Function updateLayer(selectionvalue As String, selectiontabelle As String, selectioncol As String, _aid As Integer, selinfo As String,
                                raumtyp As String, vergleichsOperator As String, vergleichswert As String, displaycols As String) As String 'trim(spec)
        Dim url As String
        Dim result, hinweis As String

        Try
            l(" MOD updateLayer anfang")
            url = "http://gis.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=" & GisUser.nick.Trim
            url = url & "&modus=einzeln"
            url = url & "&raumtyp=" & raumtyp
            url = url & "&aid=" & _aid
            url = url & "&vergleichsOperator=" & vergleichsOperator.Trim
            url = url & "&vergleichswert=" & vergleichswert.Trim
            url = url & "&selinfo=" & selinfo.Trim
            url = url & "&sqlvalue=" & selectionvalue.Trim
            url = url & "&table=" & selectiontabelle.Trim
            url = url & "&selectcol=trim(" & selectioncol.Trim & ")"
            url = url & "&displaycol=" & displaycols
            Dim sql As String = makeSqlStatement(displaycols, selectionvalue, selectiontabelle, selectioncol, vergleichsOperator)
            url = url & "&sql=" & sql.Trim

            'http://gis.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=feinen_j&modus=einzeln&sql=SELECT%20gid,geom%20%20FROM%20arten_tiere.arten_tiere_p%20%20where%20trim(spec)&sqlvalue=Bombina%20variegata
            'http://gis.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=Feinen_J&modus=einzeln&sql=select gid,geom from arten_tiere.arten_tiere_p where trim(spec)&sqlvalue= Aeshna isosceles
            'Process.Start(url)
            result = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, System.Text.Encoding.UTF8, 0)
            Return result

            l(" MOD updateLayer ende")
        Catch ex As Exception
            l("Fehler in updateLayer: " & ex.ToString())
            Return ""
        End Try
    End Function

    Private Shared Function makeSqlStatement(displaycols As String, selectionvalue As String, selectiontabelle As String, selectioncol As String, vergleichsOperator As String) As String
        Dim a() As String
        Dim sql As String = ""
        Try
            l(" MOD makeSqlStatement anfang")
            If vergleichsOperator = "gleich" Then
                'url = url & "&sql=select gid,geom,selinfo from " & selectiontabelle.Trim
                'url = url & "      where trim(" & selectioncol.Trim & ")"
                sql = sql & "select " & displaycols & " from " & selectiontabelle.Trim & " where "
                a = selectionvalue.Split(";"c)
                For i = 0 To a.Count - 1
                    sql = sql & " trim(" & selectioncol.Trim & ") = '" & a(i).Trim & "' "
                    If a.Count - 1 > i Then
                        sql = sql & " or "
                    End If
                Next
            End If
            If vergleichsOperator = "like" Then
                sql = sql & "select gid,geom,selinfo from " & selectiontabelle.Trim
                sql = sql & "      where trim(" & selectioncol.Trim & ")"
            End If
            l(" MOD makeSqlStatement ende: " & sql)
            Return sql
        Catch ex As Exception
            l("Fehler in makeSqlStatement: " & ex.ToString())
            Return sql
        End Try
    End Function

    Friend Shared Function populateListBox(rec() As String, lvEbenenAlle As ListView) As List(Of clsUniversal)
        Dim a() As String
        Dim newuniveralList As New List(Of clsUniversal)
        Dim newitem As New clsUniversal
        Try
            l(" MOD populateListBox anfang")
            For i = 0 To rec.Count - 1
                If rec(i).IsNothingOrEmpty Then Continue For
                a = rec(i).Split("#"c)
                newitem = New clsUniversal
                newitem.mithaken = False
                newitem.tag = a(0)
                newitem.titel = a(1)
                newuniveralList.Add(newitem)

            Next i
            l(" MOD populateListBox ende")
            Return newuniveralList
        Catch ex As Exception
            l("Fehler in populateListBox: " & ex.ToString())
            Return newuniveralList
        End Try
    End Function



    Friend Shared Function isSelectionLayerErzeugt(nick As String, allLayersPres As List(Of clsLayerPres)) As Integer
        Try
            l(" MOD isSelectionLayerLoaded anfang")
            For Each layer As clsLayerPres In allLayersPres
                If layer.titel.ToLower = "auswahl: " & nick.ToLower Then
                    Return layer.aid
                End If
            Next
            l(" MOD isSelectionLayerLoaded ende")
            Return 0
        Catch ex As Exception
            l("Fehler in isSelectionLayerLoaded: " & ex.ToString())
            Return -1
        End Try
    End Function

    Friend Shared Sub createuserlayer(nick As String)
        Dim url As String
        Dim result, hinweis As String
        Try
            l(" MOD createuserlayer anfang")
            url = "http://gis.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=" & GisUser.nick.Trim
            url = url & "&modus=einzeln"
            url = url & "&aid=485"
            url = url & "selinfo=[selinfo]"
            url = url & "sqlvalue=Abramis brama (FISH)" '& selectionvalue.Trim
            url = url & "&table=arten_tiere.test1"
            url = url & "&selectcol=trim(spectag)"
            url = url & "&displaycol=gid,geom,selinfo"
            url = url & "&sql=select gid,geom,selinfo from arten_tiere.test1      where trim(spectag)"
            'http://gis.kreis-of.local/cgi-bin/apps/paradigmaex/layer2shpfile/userSelectionLayer/userSelectionLayer.cgi?nick=Feinen_J&modus=einzeln&aid=485
            '&selinfo=[selinfo]
            '&sqlvalue=Abramis brama (FISH)
            '&table=arten_tiere.test1
            '&selectcol=trim(spectag)
            '&displaycol=gid,geom,selinfo
            '&sql=select gid,geom,selinfo from arten_tiere.test1      where trim(spectag)

            result = meineHttpNet.meinHttpJob(ProxyString, url, hinweis, System.Text.Encoding.UTF8, 0)
            'Return result

            l(" MOD createuserlayer ende")
        Catch ex As Exception
            l("Fehler in createuserlayer: " & ex.ToString())
            'Return ""
        End Try
    End Sub

    Friend Shared Sub adduserlayer(nick As String, selaid As Integer)

    End Sub
End Class
