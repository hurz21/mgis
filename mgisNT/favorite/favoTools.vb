Imports System.Data

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
            clsWebgisPGtools.FavoritDBsave(GisUser.username, gruppe, favoritakt)
            l("SaveFavorit---------------------- ende")
        Catch ex As Exception
            l("Fehler in SaveFavorit: " & ex.ToString())
        End Try
    End Sub

    Function makeFavoritObjekt() As clsFavorit
        Dim Lokfavoritakt As New clsFavorit
        Try
            l("makeFavoritObjekt---------------------- anfang")

            Lokfavoritakt.aktiv = getAktivAid()
            Lokfavoritakt.vorhanden = getVorhandeneEbenen("")
            Lokfavoritakt.gecheckted = getGescheckteEbene("")
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
    '            Return mgisRemoteUserRoot & "favoriten\zuletzt\" & gruppe & "_" & GisUser.username & "_Favorit.txt"
    '        Case "meine", "meinespeichern"
    '            Return mgisRemoteUserRoot & "favoriten\pers\" & GisUser.username & "_Favorit.txt"
    '        Case Else
    '            Return mgisRemoteUserRoot & "favoriten\" & gruppe & "_Favorit.txt"
    '    End Select
    'End Function

    Private Function getHgrund() As String
        Return CType(layerHgrund.aid, String)
    End Function

    Function getGescheckteEbene(ByRef titelliste As String) As String
        Dim saidliste As String = ""
        titelliste = ""
        For Each clay As clsLayerPres In layersSelected
            If clay.mithaken Then
                If STARTUP_mgismodus = "paradigma" Then
                    If clay.aid = GisUser.userLayerAid And GisUser.userLayerAid > 0 Then Continue For 'userlayer wird nicht gespeichert
                End If
                saidliste = saidliste & ";" & clay.aid
                titelliste = titelliste & " (" & clay.aid & ")" + clay.titel & Environment.NewLine
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
        Dim favoritaktaltesObjekt As New clsFavorit
        Dim istschonvorhanden As Boolean
        Dim sql As String
        Try
            l("FavoriteLaden---------------------------")
            l("gruppe---------------------------" & gruppe)
            l("name---------------------------" & uname)
            'Dim altesobjekt As New clsFavorit
            Dim tempgruppe As String = gruppe
            If gruppe = "meine" Then tempgruppe = "meinespeichern"
            Dim dt As DataTable
            If iminternet Then
                favoritakt = favoTools.favoritReadUserIni(tempgruppe.ToLower.Trim)
                If favoritakt.vorhanden.IsNothingOrEmpty Then
                    favoritakt = setFavoritMinimum()
                    Return False
                Else
                    Return True
                End If
            Else
                sql = "select * from public.favoriten " &
                                      " where lower(trim(username))='" & uname.ToLower.Trim &
                                      "' and lower(trim(gruppe))='" & tempgruppe.ToLower.Trim & "'"
                l("sql " & sql)
                dt = getDTFromWebgisDB(sql, "webgiscontrol")
                istschonvorhanden = clsWebgisPGtools.hatRecords(dt)
                l("FavoriteLaden istschonvorhanden " & istschonvorhanden)
                If istschonvorhanden Then
                    favoritakt = favoritDb2Obj(dt)
                    l("FavoriteLaden true")
                    Return True
                Else
                    l("FavoriteLaden false")
                    'favofile = calcMeinFavoriteDateiname(gruppe)
                    'Using fr As New IO.StreamReader(favofile)
                    favoritakt = setFavoritMinimum()
                    '    Return True
                    'End Using
                    Return False
                End If
            End If
        Catch ex As Exception
            l("warnung in FavoriteLaden " & "  fehlt.", ex)
            Return False
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

    Function favoritDb2Obj(lokdt As System.Data.DataTable) As clsFavorit
        Dim favo As New clsFavorit
        Try
            l("favoritDb2Obj---------------------- anfang")
            favo.titel = clsDBtools.fieldvalue(lokdt.Rows(0).Item("titel"))
            favo.vorhanden = clsDBtools.fieldvalue(lokdt.Rows(0).Item("vorhanden"))
            favo.gecheckted = clsDBtools.fieldvalue(lokdt.Rows(0).Item("gecheckt"))
            favo.hgrund = clsDBtools.fieldvalue(lokdt.Rows(0).Item("hgrund"))
            favo.aktiv = clsDBtools.fieldvalue(lokdt.Rows(0).Item("aktiv"))
            Return favo
            l("favoritDb2Obj---------------------- ende")
        Catch ex As Exception
            l("Fehler in favoritDb2Obj: " & ex.ToString())
            Return Nothing
        End Try
    End Function
    Sub favoritSaveUserIni(gruppe As String, favoritakt As clsFavorit)
        myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "username", GisUser.username.ToLower.Trim)
        myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "gruppe", gruppe.ToLower.Trim)
        myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "titel", favoritakt.titel)
        myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "vorhanden", favoritakt.vorhanden)
        myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "gecheckt", favoritakt.gecheckted)
        myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "hgrund", favoritakt.hgrund)
        myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "aktiv", favoritakt.aktiv)
        myglobalz.userIniProfile.WertSchreiben("FAVORIT_" & gruppe, "ts", DateTime.Now.ToShortDateString)
    End Sub
    Function favoritReadUserIni(gruppe As String) As clsFavorit
        Dim favo As New clsFavorit
        Dim temp As String = ""
        Try
            temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "titel")
            favo.titel = If(temp.IsNothingOrEmpty, "", temp)

            temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "vorhanden")
            favo.vorhanden = If(temp.IsNothingOrEmpty, "", temp)

            temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "gecheckt")
            favo.gecheckted = If(temp.IsNothingOrEmpty, "", temp)

            temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "hgrund")
            favo.hgrund = If(temp.IsNothingOrEmpty, "", temp)

            temp = myglobalz.userIniProfile.WertLesen("FAVORIT_" & gruppe, "aktiv")
            favo.aktiv = If(temp.IsNothingOrEmpty, "", temp)
            Return favo
        Catch ex As Exception
            l("Fehler in favoritReadUserIni: " & ex.ToString())
            Return favo
        End Try
    End Function
End Module
