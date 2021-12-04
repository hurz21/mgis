Imports System.Data
Imports mgis

Module favoTools


    Friend Sub FavoritSave(userName As String)
        'Dim favofile As String
        Dim gruppe As String
        Try
            l("SaveFavorit---------------------- anfang")
            If userName = "zuletzt" Then
                'favofile = calcMeinFavoriteDateiname("zuletzt")
                gruppe = "zuletzt"
            Else
                'favofile = calcMeinFavoriteDateiname("meinespeichern")
                gruppe = "meinespeichern"
            End If

            favoritakt = makeFavoritObjekt()
            'clsWebgisPGtools.FavoritDBsaveOLD(GisUser.nick, gruppe, favoritakt)
            clsWebgisPGtools.FavoritDBsave(GisUser.nick, gruppe, favoritakt)
            l("SaveFavorit---------------------- ende")
        Catch ex As Exception
            l("Fehler in SaveFavorit: " & ex.ToString())
        End Try
    End Sub

    Sub favoritInsertHTTP(gruppe As String, favoritakt As clsFavorit)
        l("favoritInsertDB---------------------- anfang")
        Dim schema, SQL, hinweis, result As String
        Try

            hinweis = ""
            result = ""
            schema = If(iminternet, "externparadigma", "public")
            SQL = "INSERT INTO " & schema & ".favoriten " &
                                            "(username,gruppe,titel,vorhanden,gecheckt,hgrund,aktiv,ts) " &
                                            "VALUES('" &
                                            GisUser.nick.ToLower.Trim & "','" & gruppe.ToLower.Trim & "','" &
                                            favoritakt.titel & "','" & favoritakt.vorhanden & "','" & favoritakt.gecheckted & "','" &
                                            favoritakt.hgrund & "','" & favoritakt.aktiv & "','" &
                                            DateTime.Now & "')  "
            result = clsToolsAllg.getSQL4Http(SQL, "webgiscontrol", hinweis, "putsql") : l(hinweis)
            result = result.Replace("$", "").Replace(vbCrLf, "").Trim
        Catch ex As Exception
            l("Fehler in favoritInsertDB: schema " & schema & Environment.NewLine &
            "schema " & schema & Environment.NewLine &
            "gruppe " & gruppe & Environment.NewLine &
            "SQL " & SQL & Environment.NewLine &
            "result " & result & Environment.NewLine &
            "hinweis " & hinweis & Environment.NewLine &
            "favoritakt " & favoritakt.nachstring("#") & Environment.NewLine &
              ex.ToString())
        End Try
    End Sub

    Function makeFavoritObjekt() As clsFavorit
        Dim Lokfavoritakt As New clsFavorit

        Dim count As Integer
        Try
            l("makeFavoritObjekt---------------------- anfang")

            Lokfavoritakt.aktiv = getAktivAid()
            Lokfavoritakt.vorhanden = getVorhandeneEbenen("")
            Lokfavoritakt.gecheckted = getGescheckteEbene("", count)
            'If favoritakt.gecheckted.Trim.Length < 5 Then
            '    l("fehler favoritakt.gecheckted.Trim.Length < 5, nicht gespeichert!!!")
            '    Exit Sub
            'End If
            Lokfavoritakt.hgrund = getHgrund()
            l("makeFavoritObjekt---------------------- ende")
            Return Lokfavoritakt
        Catch ex As Exception
            l("Fehler in makeFavoritObjekt: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    'Public Function calcMeinFavoriteDateiname(gruppe As String) As String
    '    Select Case gruppe
    '        Case "zuletzt"
    '            Return mgisRemoteUserRoot & "favoriten\zuletzt\" & gruppe & "_" & GisUser.nick & "_Favorit.txt"
    '        Case "meine", "meinespeichern"
    '            Return mgisRemoteUserRoot & "favoriten\pers\" & GisUser.nick & "_Favorit.txt"
    '        Case Else
    '            Return mgisRemoteUserRoot & "favoriten\" & gruppe & "_Favorit.txt"
    '    End Select
    'End Function

    Private Function getHgrund() As String
        Return CType(layerHgrund.aid, String)
    End Function

    Function getGescheckteEbene(ByRef titelliste As String, ByRef count As Integer) As String
        Dim saidliste As String = ""
        count = 0
        titelliste = ""

        For Each clay As clsLayerPres In layersSelected
            If clay.mithaken Then
                If STARTUP_mgismodus = "paradigma" Then
                    If clay.aid = GisUser.userLayerAid And GisUser.userLayerAid > 0 Then Continue For 'userlayer wird nicht gespeichert
                End If
                saidliste = saidliste & ";" & clay.aid
                titelliste = titelliste & " (" & clay.aid & ")" + clay.titel & Environment.NewLine
                count += 1
            End If
        Next
        Return saidliste
    End Function

    Function getVorhandeneEbenen(ByRef titelliste As String) As String
        Dim saidliste As String = ""
        titelliste = ""
        For Each clay As clsLayerPres In layersSelected
            If STARTUP_mgismodus = "paradigma" Then
                If clay.aid = GisUser.userLayerAid And GisUser.userLayerAid > 0 Then Continue For
            End If
            saidliste = saidliste & ";" & clay.aid
            titelliste = titelliste & " (" & clay.aid & ")" + clay.titel & Environment.NewLine
        Next
        Return saidliste
    End Function

    Private Function getAktivAid() As String
        Dim ret As String
        If layerActive.aid < 1 Then
            If layerHgrund.isactive Then
                ret = CType(layerHgrund.aid, String)
            Else
                ret = CType(0, String)
            End If
        Else
            ret = CType(layerActive.aid, String)
        End If
        Return ret
    End Function
    Friend Function FavoritLaden(gruppe As String, uname As String) As Boolean
        Try
            l(" MOD FavoritLaden anfang")
            loadFromHTTP(gruppe, uname) 'myglobalz.favoritakt wird geladen
            favoritakt.vorhanden = removeHgrund(favoritakt.vorhanden)
            favoritakt.gecheckted = removeHgrund(favoritakt.gecheckted)

            favoritakt.vorhanden = removeDuplicates(favoritakt.vorhanden)
            favoritakt.gecheckted = removeDuplicates(favoritakt.gecheckted)
            l(" MOD FavoritLaden ende")
            Return True
        Catch ex As Exception
            l("Fehler in FavoritLaden: " & ex.ToString())
            Return False
        End Try
    End Function
    Private Function removeHgrund(kandidat As String) As String
        Dim newstring As String = ""
        Dim a() As String
        Dim sb As New Text.StringBuilder
        Try
            l(" MOD removeHgrund anfang")
            a = kandidat.Split(";"c)
            For i = 0 To a.Count - 1
                If a(i) = String.Empty Then Continue For
                If modLayer.istAuchHintergrund(CInt(a(i))) Then
                Else
                    sb.Append(a(i) & ";")
                End If
            Next
            newstring = clsString.removeLastChar(sb.ToString)
            l(" MOD removeHgrund ende")
            Return newstring
        Catch ex As Exception
            l("Fehler in removeHgrund: " & ex.ToString())
            Return kandidat
        End Try
    End Function

    Private Function removeDuplicates(kandidat As String) As String
        Dim newstring As String = ""
        Dim a(), b() As String
        'Dim sb As New Text.StringBuilder
        Try
            l(" MOD removeHgrund anfang")
            a = kandidat.Split(";"c)
            ReDim b(a.Count - 1)
            For i = 0 To a.Count - 1
                'If a(i) = String.Empty Then Continue For
                If clsString.isinarray(newstring, a(i), ";") Then

                Else
                    newstring = newstring & a(i) & ";"
                End If
            Next
            newstring = clsString.removeLastChar(newstring)
            l(" MOD removeHgrund ende")
            Return newstring
        Catch ex As Exception
            l("Fehler in removeHgrund: " & ex.ToString())
            Return kandidat
        End Try
    End Function

    Private Function loadFromHTTP(gruppe As String, uname As String) As Boolean
        Dim favoritaktaltesObjekt As New clsFavorit
        Dim sql As String = "", result As String = "", hinweis As String = ""
        Dim schema As String = If(iminternet, "externparadigma", "public")
        Try
            l("loadFromHTTP---------------------------")
            l("gruppe---------------------------" & gruppe)
            l("name---------------------------" & uname)
            'Dim altesobjekt As New clsFavorit
            Dim tempgruppe As String = gruppe
            If gruppe = "meine" Then tempgruppe = "meinespeichern"
            sql = "select * from " & schema & ".favoriten" &
                        " where lower(trim(username))='" & uname.ToLower.Trim &
                        "' and lower(trim(gruppe))='" & tempgruppe.ToLower.Trim & "'"
            l("sql " & sql)
            'Dim dt As DataTable
            'If iminternet Or CGIstattDBzugriff Then
            result = clsToolsAllg.getSQL4Http(sql, "webgiscontrol", hinweis, "getsql") : l(hinweis)
            result = result.Replace("$", "").Replace(vbCrLf, "")
            If result.IsNothingOrEmpty Then
                strGlobals.FavoriteneintragSchonvorhanden = False
                favoritakt = setFavoritMinimum()
                'hier speichern?
                Return False
            Else
                favoritakt = favoritFromAjax(result)
                strGlobals.FavoriteneintragSchonvorhanden = False
                Return True
            End If
        Catch ex As Exception
            l("warnung in loadFromHTTP " & "  fehlt.", ex)
            Return False
        End Try
    End Function

    Friend Function getinteger(id As String) As Integer
        If (id.IsNothingOrEmpty) Then
            Return 0
        End If
        Return CInt(id.Replace(";", ""))
    End Function
    Private Function favoritFromAjax(result As String) As clsFavorit
        Dim zeilen, spalten As Integer
        Dim a(), b() As String
        Dim oldname As String = ""
        Dim favo As New clsFavorit
        Try
            l(" favoritFromAjax html---------------------- anfang")
            result = result.Trim
            If result.IsNothingOrEmpty Then
                l("Fehler in favoritFromAjax 2: " & result)
                Return Nothing
            End If
            a = result.Split(New Char() {"$"c}, StringSplitOptions.RemoveEmptyEntries) : zeilen = a.Count
            b = a(0).Split("#"c) : spalten = b.Count
            favo.vorhanden = clsDBtools.fieldvalue(b(4)) '("vorhanden"))
            favo.gecheckted = clsDBtools.fieldvalue(b(5)) '("gecheckt"))
            favo.hgrund = clsDBtools.fieldvalue(b(6)) '("hgrund"))
            favo.aktiv = clsDBtools.fieldvalue(b(7)) '("aktiv")) 
            Return favo
            l(" favoritFromAjax ---------------------- ende")
        Catch ex As Exception
            l("Fehler in favoritFromAjax 1: " & ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Function setFavoritMinimum() As clsFavorit
        Dim fav As New clsFavorit
        fav.titel = ""
        fav.vorhanden = "4"
        fav.gecheckted = "4"
        fav.hgrund = "380"
        fav.aktiv = ""
        Return fav
    End Function

    Friend Function getStandardActiveLayer(favoritakt_aktiv As String, favoritakt_gecheckted As String) As Integer
        'getinteger(favoritakt.aktiv)
        Dim result As Integer = 0
        Try
            l(" MOD getStandardActiveLayer anfang")
            result = getinteger(favoritakt_aktiv)
            If result > 0 Then
                If clsString.isinarray(favoritakt_gecheckted, result.ToString, ";") Then
                    'result ist richtig
                Else
                    result = 0
                End If
            End If
            l(" MOD getStandardActiveLayer ende")
            Return result
        Catch ex As Exception
            l("Fehler in getStandardActiveLayer: " & ex.ToString())
            Return 0
        End Try
    End Function

    'Function favoritDb2Obj(lokdt As System.Data.DataTable) As clsFavorit
    '    Dim favo As New clsFavorit
    '    Try
    '        l("favoritDb2Obj---------------------- anfang")
    '        favo.titel = clsDBtools.fieldvalue(lokdt.Rows(0).Item("titel"))
    '        favo.vorhanden = clsDBtools.fieldvalue(lokdt.Rows(0).Item("vorhanden"))
    '        favo.gecheckted = clsDBtools.fieldvalue(lokdt.Rows(0).Item("gecheckt"))
    '        favo.hgrund = clsDBtools.fieldvalue(lokdt.Rows(0).Item("hgrund"))
    '        favo.aktiv = clsDBtools.fieldvalue(lokdt.Rows(0).Item("aktiv"))
    '        Return favo
    '        l("favoritDb2Obj---------------------- ende")
    '    Catch ex As Exception
    '        l("Fehler in favoritDb2Obj: " & ex.ToString())
    '        Return Nothing
    '    End Try
    'End Function
    'Sub favoritSaveUserIni(gruppe As String, favoritakt As clsFavorit)
    '    myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "username", GisUser.nick.ToLower.Trim)
    '    myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "gruppe", gruppe.ToLower.Trim)
    '    myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "titel", favoritakt.titel)
    '    myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "vorhanden", favoritakt.vorhanden)
    '    myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "gecheckt", favoritakt.gecheckted)
    '    myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "hgrund", favoritakt.hgrund)
    '    myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "aktiv", favoritakt.aktiv)
    '    myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "ts", DateTime.Now.ToShortDateString)
    'End Sub
    'Function favoritReadUserIni(gruppe As String) As clsFavorit
    '    l("favoritReadUserIni" & gruppe)
    '    Dim favo As New clsFavorit
    '    Dim temp As String = ""
    '    Try
    '        temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "titel")
    '        favo.titel = If(temp.IsNothingOrEmpty, "", temp)

    '        temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "vorhanden")
    '        favo.vorhanden = If(temp.IsNothingOrEmpty, "", temp)

    '        temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "gecheckt")
    '        favo.gecheckted = If(temp.IsNothingOrEmpty, "", temp)

    '        temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "hgrund")
    '        favo.hgrund = If(temp.IsNothingOrEmpty, "", temp)

    '        temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "aktiv")
    '        favo.aktiv = If(temp.IsNothingOrEmpty, "", temp)
    '        l("favo laden erfolgreich: " & favo.vorhanden)
    '        Return favo
    '    Catch ex As Exception
    '        l("Fehler in favoritReadUserIni: " & ex.ToString())
    '        Return favo
    '    End Try
    'End Function
End Module
