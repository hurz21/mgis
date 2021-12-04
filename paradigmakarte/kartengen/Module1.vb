Imports kartengen

Module Module1
    Public MeinNULLDatumAlsDate As Date = CDate("0001-01-01 01:01:01")
    Public SQL As New SQLControlMSSQL
    Public coordinatesystemNumber As String = "25832"
    Sub Main()
        Dim av As New Vorgang
        Dim iok, ifail As Integer
        Dim query, serial As String
        Dim Table = "VS_D"
        Dim rbtyp As Integer
        Dim vs_d As String =
   " (SELECT TOP (100) PERCENT s.ID, s.VORGANGSID, s.BEARBEITER, s.EINGANG, s.BESCHREIBUNG, s.BEMERKUNG, s.ERLEDIGT, s.LETZTEBEARBEITUNG, s.ORTSTERMIN, s.STELLUNGNAHME, " &
   "             s.LASTACTIONHEROE, s.ISTINVALID, s.PROBAUGAZ, s.GEMKRZ, s.AUFNAHME, s.ALTAZ, s.AZ2, s.WEITEREBEARB, s.ARCDIR, s.DARFNICHTVERNICHTETWERDEN, s.STORAUMNR, s.STOTITEL,  " &
   "             s.GUTACHTENMIT, s.GUTACHTENDRIN, s.ABGABEBA, s.PARAGRAF, s.HATRAUMBEZUG, s.INTERNENR, v.VORGANGSID AS mid, v.SACHGEBIETNR, v.VORGANGSNR, v.VORGANGSGEGENSTAND,  " &
   "             v.SACHGEBIETSTEXT, v.ISTUNB, v.AZ, v.TS " &
   " FROM   paradigma.dbo.t41 AS s INNER JOIN " &
   "             paradigma.dbo.t43 AS v ON s.VORGANGSID = v.VORGANGSID " &
   " ORDER BY s.LETZTEBEARBEITUNG DESC) "

        truncateZielTabelle("paradigma_userdata.vorgaenge")
        truncateZielTabelle("paradigma_userdata.vorgaenge_p")
        Table = "RAUMBEZUGUNDVORG"
        'query = "select * from " & Table & " where  LETZTEBEARBEITUNG  > to_date('" & "01.01.2015" & "','DD.MM.YYYY') "
        'query = "select * from " & Table & " order by vorgangsid desc"
        query = " SELECT r.RAUMBEZUGSID,r.TYP,r.TITEL,r.SEKID,r.ABSTRACT,r.RECHTS,r.HOCH,r.XMIN,r.XMAX,r.YMIN,r.YMAX,r.FREITEXT,r.ISMAPENABLED,rv.raumbezugsid as ridRV,rv.vorgangsid,rv.status,r.flaecheqm,r.laengem,r.mitetikett,v.sachgebietnr, " &
                    " v.sachgebietstext ,v.letztebearbeitung " &
                    " FROM paradigma.dbo.raumbezug2vorgang rv,paradigma.dbo.raumbezug r  , " & vs_d & " v " &
                    " where r.raumbezugsid=rv.raumbezugsid  " &
                    " and v.vorgangsid=rv.vorgangsid "
        iok = 0
        ifail = 0
        SQL.ExecQuery(query)
        Dim icnt As Integer = 0
        Dim punkt As New myPoint
        ' ERROR HANDLING
        If SQL.HasException(True) Then Exit Sub
        l("anzahl " & SQL.DBDT.Rows.Count)
        schleife(av, iok, ifail, serial, rbtyp, icnt, punkt)
        l("anz: " & SQL.DBDT.Rows.Count)
        l("icnt: " & icnt)
        l("iOK: " & iok)
        l("faile: " & ifail)
        Process.Start("C:\Users\feinen_j\AppData\Roaming\KREIS OFFENBACH\kartengen\1.0.0.0")
    End Sub

    Private Sub schleife(ByRef av As Vorgang, ByRef iok As Integer, ByRef ifail As Integer, ByRef serial As String, ByRef rbtyp As Integer, ByRef icnt As Integer, ByRef punkt As myPoint)
        Try
            l("schleife---------------------- anfang")
            For i = 0 To SQL.DBDT.Rows.Count - 1
                punkt = New myPoint
                av = New Vorgang
                l(i.ToString)
                'Debug.Print("" & (SQL.DBDT.Rows(i).Item("lastactionheroe")))
                If Not datenausParadigmaHolen(av, rbtyp, punkt, i) Then
                    l("fehler bei einlesen in i, daten konnten nicht geholt werden " & i)
                    ifail += 1
                    Continue For
                End If

                clsString.kuerzeTextauf(av.Stammdaten.Beschreibung, 99)
                clsString.kuerzeTextauf(av.Stammdaten.az.sachgebiet.Header, 99)
                'serial = getGeomFromCoordinates(punkt)
                If punkt.X > 0 And punkt.Y > 0 Then
                    'ausgabe
                    serial = "POINT (" & CInt((punkt.X)) & " " & CInt(punkt.Y) & ")"
                    If datenNachPostGISschreiben("vorgaenge", av, punkt, serial, rbtyp) Then
                        iok += 1
                    Else
                        l("fehler beim schreiben av.Stammdaten.ID, vid " & av.Stammdaten.ID)
                        ifail += 1
                    End If
                End If
                icnt += 1
            Next
            l("schleife ---------------------- ende")
        Catch ex As Exception
            l("Fehler in schleife: " & ex.ToString())
        End Try
    End Sub

    Private Sub truncateZielTabelle(zieltab As String)
        Dim querey = "TRUNCATE " & zieltab & " RESTART IDENTITY;"
        Try
            'l("---------------------- anfang")
            pgTools.sqlausfuehren(querey)
            querey = "TRUNCATE " & zieltab & " RESTART IDENTITY;"
            pgTools.sqlausfuehren(querey)
            'l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
        End Try
    End Sub

    Private Function getGeomFromCoordinates(punkt As myPoint) As String
        Dim sql As String = "select  GeometryFromText ( 'POINT ( " & punkt.X & " " & punkt.Y & " )', " & coordinatesystemNumber & " )"
        Dim dt As New DataTable
        dt = pgTools.sqlausfuehren(sql)
        Return dt.Rows(0).Item(0)
    End Function

    Private Function datenausParadigmaHolen(ByRef av As Vorgang, ByRef rbtyp As Integer, ByRef punkt As myPoint, i As Integer) As Boolean
        Try
            'l("---------------------- anfang")
            av.Stammdaten.ID = clsDBtools.fieldvalue(SQL.DBDT.Rows(i).Item("vorgangsid"))
            av.Stammdaten.Beschreibung = clsDBtools.fieldvalue(SQL.DBDT.Rows(i).Item("titel"))
            av.Stammdaten.LetzteBearbeitung = CDate(clsDBtools.fieldvalue(SQL.DBDT.Rows(i).Item("LetzteBearbeitung")))
            av.Stammdaten.az.sachgebiet.Header = clsDBtools.fieldvalue(SQL.DBDT.Rows(i).Item("sachgebietstext"))
            av.Stammdaten.az.sachgebiet.Zahl = clsDBtools.fieldvalue(SQL.DBDT.Rows(i).Item("sachgebietnr"))
            punkt.X = clsDBtools.fieldvalue(SQL.DBDT.Rows(i).Item("rechts"))
            punkt.Y = clsDBtools.fieldvalue(SQL.DBDT.Rows(i).Item("hoch"))
            rbtyp = clsDBtools.fieldvalue(SQL.DBDT.Rows(i).Item("typ"))
            'l("---------------------- ende")
            Return True
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return False
        End Try
    End Function

    Sub l(v As String)
        Debug.Print(v)
        My.Application.Log.WriteEntry(v)
    End Sub

    Private Function datenNachPostGISschreiben(zieltabelle As String, av As Vorgang, punkt As myPoint, serial As String, rbtyp As Integer) As Boolean
        Dim query As String
        Dim ersterBuchstabe, letztesjahr As String
        Dim dt As DataTable
        Try
            'l("---------------------- anfang")
            ersterBuchstabe = getErsterBuchstabe(av.Stammdaten.az.sachgebiet.Zahl)
            letztesjahr = getletztesjahr(av.Stammdaten.LetzteBearbeitung)
            query = "INSERT INTO paradigma_userdata." & zieltabelle & " " &
                            "(GEOM,SGNR,TITEL,SGTEXT,PARADIGMAVID, LETZTEBEARBEITUNG,LETZTESJAHR,SGNR1) " &
                            "VALUES( ST_Buffer( ST_GeomFromText('" & serial & "'," & coordinatesystemNumber & "),4,2),'" &
                            av.Stammdaten.az.sachgebiet.Zahl & "','" &
                           clsString.kuerzeTextauf(av.Stammdaten.Beschreibung, 99).Replace("'", " ") & "','" &
                            av.Stammdaten.az.sachgebiet.Header.Replace("'", " ") & "'" & "," &
                            av.Stammdaten.ID & ",'" &
                            av.Stammdaten.LetzteBearbeitung & "','" &
                            letztesjahr & "','" &
                            ersterBuchstabe & "')"
            dt = pgTools.sqlausfuehren(query)
            If dt Is Nothing Then
                Debug.Print("")
            Else
                ' Return True
            End If
            'query = "INSERT INTO paradigma_userdata.vorgaenge " &
            '                "(GEOM,SGNR,TITEL,SGTEXT,PARADIGMAVID, LETZTEBEARBEITUNG,LETZTESJAHR,SGNR1) " &
            '                "VALUES( ST_GeomFromText('" & serial & "'," & coordinatesystemNumber & "),'" &


            query = "INSERT INTO paradigma_userdata.vorgaenge_p " &
                            "(GEOM,SGNR,TITEL,SGTEXT,PARADIGMAVID, LETZTEBEARBEITUNG,LETZTESJAHR,SGNR1) " &
                            "VALUES( ST_GeomFromText('" & serial & "'," & coordinatesystemNumber & " ),'" &
                            av.Stammdaten.az.sachgebiet.Zahl & "','" &
                            clsString.kuerzeTextauf(av.Stammdaten.Beschreibung, 99).Replace("'", " ") & "','" &
                            av.Stammdaten.az.sachgebiet.Header.Replace("'", " ") & "'" & "," &
                            av.Stammdaten.ID & ",'" &
                            av.Stammdaten.LetzteBearbeitung & "','" &
                            letztesjahr & "','" &
                            ersterBuchstabe & "')"
            dt = pgTools.sqlausfuehren(query)


            If dt Is Nothing Then
                Return False
            Else
                Return True
            End If
            'l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return False
        End Try
    End Function

    Private Function getletztesjahr(letzteBearbeitung As Date) As String
        Dim a As String
        Try
            'l("---------------------- anfang")
            a = Format(letzteBearbeitung, "yyyy")
            Return a
            'l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return " "
        End Try
    End Function

    Private Function getErsterBuchstabe(zahl As String) As String
        Dim a As String
        Try
            l("---------------------- anfang")
            a = zahl.Substring(0, 1)
            Return a
            l("---------------------- ende")
        Catch ex As Exception
            l("Fehler in : " & ex.ToString())
            Return " "
        End Try
    End Function
End Module
