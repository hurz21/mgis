Module modUserLayer
    Friend Function getUserSelectionEbeneAid(username As String, ByRef useridINtern As Integer, ByRef mac As String) As Integer

        Return modPG.getUserebeneAidFromNutzerTab(username, useridINtern, mac)
    End Function

    Friend Function userLayerErzeugen(ByRef tablename As String, vid As String, _modus As String, nick As String) As Integer
        l("userLayerErzeugen")
        Dim aid = modPG.userLayerInStammErzeugenAid(tablename, nick)
        If aid = 0 Then
            Return 0
        End If
        l("aid wurde erzeugt: " & aid)
        tablename = getTablename(_modus, aid) : l("tablename: " & tablename)
        modPG.userLayerAttribErzeugenAid(tablename, aid)
        modPG.userLayerActiveDirErzeugen(tablename, aid)
        Return aid
    End Function

    Friend Function InsertInNutzertab(username As String, userEbeneAid As Integer, mac As String) As Integer
        Return modPG.InsertInNutzertabGetNid(username, userEbeneAid, mac)
    End Function

    Friend Function updateNutzerTab(useridINtern As Integer, userEbeneAid As Integer, mac As String) As Boolean
        Return modPG.UpdateNutzertabAid(useridINtern, userEbeneAid, mac)
    End Function
End Module
