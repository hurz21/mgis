
Public Class clsAdress
    Implements ICloneable
    Implements IDisposable

    Public gemparms As New clsGemarkungsParams
    Private _strassennameNORM$
    Property geom As String
    Sub clear()
        gemeindeName = ""
        gemeindeNr = 0
        strasseName = ""
        HausKombi = ""
        gemeindeNr = 0
        strasseCode = 0
        hausNr = 0
        hausZusatz = ""

    End Sub
    Sub New()
        ' TODO: Complete member initialization 
    End Sub
    Property Quelle As String 'halo oder lage oder fehlt
    Public Property strassennameNORM() As String
        Get
            Return _strassennameNORM$
        End Get
        Set(ByVal value As String)
            _strassennameNORM$ = value
        End Set
    End Property
    Private _raum$
    Public Property raum() As String
        Get
            Return _raum$
        End Get
        Set(ByVal value As String)
            _raum$ = value
        End Set
    End Property

    Public Sub New(ByRef gemeindenameIN$)
        If gemeindenameIN$.Trim.Length > 0 Then
            gemeindeName = gemeindenameIN$
        End If
    End Sub
    Private _erster_buchstabe$
    Public Property erster_buchstabe() As String
        Get
            Return _erster_buchstabe$
        End Get
        Set(ByVal value As String)
            _erster_buchstabe$ = value
        End Set
    End Property

    Private _gemeindeName$
    Public Property gemeindeName() As String
        Get
            Return _gemeindeName$
        End Get
        Set(ByVal value As String)
            _gemeindeName$ = value.Trim.Replace(" ", "")

            If _gemeindeName$.Trim.Length < 1 Then
                RaiseEvent adresseunbrauchbar(Me, 1)
            End If
            If Not gemparms.liegtGemeindeImKreisOffenbach(_gemeindeName) Then
                If IsNumeric(_gemeindeName) Then
                    falls_gemeindenameNumerisch_ist_liefere_korrekten_gemeindename_or_die()
                Else
                    falls_gemeindename_ein_gemarkungsname_ist_liefere_korrekten_gemeindename_or_die()
                End If
            End If

            ErmittleDieGemeindeNrOrDie()

        End Set
    End Property

    Private Sub falls_gemeindenameNumerisch_ist_liefere_korrekten_gemeindename_or_die()
        If gemparms.nummer2gemeindename(_gemeindeName$).StartsWith("ERROR") Then
            'gemeindename unbekannt
            'auf google umschalten
            RaiseEvent adresseunbrauchbar(Me, 2)
        Else
            _gemeindeName = gemparms.nummer2gemeindename(_gemeindeName$)
        End If
    End Sub

    Private Sub ErmittleDieGemeindeNrOrDie()
        'ermittleDieGemeindeNr
        If _gemeindeName <> "ERROR" Then
            If gemparms.gemeindetext2gemeindenr(_gemeindeName.ToString) <> "ERROR" Then
                gemeindeNr = CType(gemparms.gemeindetext2gemeindenr(_gemeindeName.ToString), Integer)
            Else
                'todo
                'If gemeindeNr < 1 Then
                'die bignr ermitteln
            End If

        Else
            gemeindeNr = -1                 'setzt gleichzeitig Liegtimkreis
            RaiseEvent adresseunbrauchbar(Me, 2)
        End If
    End Sub

    Private Sub falls_gemeindename_ein_gemarkungsname_ist_liefere_korrekten_gemeindename_or_die()
        'ist_gemeindename_eingeamrkungsname
        If gemparms.gemarkungstext2gemeindetext(_gemeindeName.ToString) <> "ERROR" Then
            'gemeindename ist in wirklichkeit ein gemarkungsname
            'z.b. rembrücken als gemeindenname rein -
            '  aha gemeindename ist ein gemarkungsname
            'welcher gemeindename ist dem gemarkungsnamen zugeordnet?
            ' heusenstamm als gemeindename raus
            _gemeindeName = gemparms.gemarkungstext2gemeindetext(_gemeindeName.ToString)
        Else
            RaiseEvent adresseunbrauchbar(Me, 2)
        End If
    End Sub
    Private _gemeindeLiegtImKreis As Boolean
    Public Property gemeindeLiegtIMKreis() As Boolean
        Get
            Return _gemeindeLiegtImKreis
        End Get
        Set(ByVal value As Boolean)
            _gemeindeLiegtImKreis = value
            If Not _gemeindeLiegtImKreis Then
                RaiseEvent adresseunbrauchbar(Me, 2)
            End If
        End Set
    End Property
    Private _gemeindeNr%
    Public Property gemeindeNr() As Integer
        Get
            Return _gemeindeNr%
        End Get
        Set(ByVal value As Integer)
            _gemeindeNr% = value
            If _gemeindeNr% < 1 Or _gemeindeNr% > 13 Then
                'liegt ausserhalb vom kreis offenbach
                gemeindeLiegtIMKreis = False
            Else
                gemeindeLiegtIMKreis = True
            End If
        End Set
    End Property
    Private _strasseName$
    Public Property strasseName() As String
        Get
            Return _strasseName$
        End Get
        Set(ByVal value As String)
            _strasseName$ = value
            _strasseName$ = _strasseName$.Replace(Chr(34), "")
            If _strasseName.Length < 1 Then
                RaiseEvent adresseunbrauchbar(Me, 10)
            End If
            If _strasseName.ToLower.StartsWith("postfa") Then
                RaiseEvent adresseunbrauchbar(Me, 11)
            End If
            'If _strasseName.ToLower.StartsWith("forstrettung") Then
            '  RaiseEvent besterForstrettungspunkt(Me, 12)
            'End If
            _strassennameNORM = clsString.normalizeSuchdbStrasse(_strasseName$)
        End Set
    End Property
    Private _strasseCode%
    Public Property strasseCode() As Integer
        Get
            Return _strasseCode%
        End Get
        Set(ByVal value As Integer)
            _strasseCode% = value
        End Set
    End Property
    Private _hausNr%
    Public Property hausNr() As Integer
        Get
            Return _hausNr%
        End Get
        Set(ByVal value As Integer)
            _hausNr% = value
        End Set
    End Property
    Private _hausZusatz$
    Public Property hausZusatz() As String
        Get
            Return _hausZusatz$
        End Get
        Set(ByVal value As String)
            If value.Trim.Length < 0 Then
                If _hausZusatz$.Length < 1 Then
                    _hausZusatz$ = value
                End If
            Else
                _hausZusatz$ = value
            End If
        End Set
    End Property
    Private _HausKombi$
    'ist die nummer vor der auftrennung
    Public Property HausKombi() As String
        Get
            Return _HausKombi$
        End Get
        Set(ByVal value As String)
            _HausKombi$ = value
            'If _HausKombi.Length < 1 Then
            '  'If is_strasse_mit_hausnummer() Then
            '  '  'wurde schon zerlegt
            '  '  '_HausKombi wurde neu erzeugt
            '  'Else
            '  '  'keine hausnummer
            '  'End If
            'End If
            If IsNumeric(HausKombi$) Then
                hausNr = CInt(Val(HausKombi))
                hausZusatz = ""
            Else
                hauskombiZerlegen()
            End If
        End Set
    End Property
    Private _GKrechts%
    Public Property GKrechts() As Integer
        Get
            Return _GKrechts%
        End Get
        Set(ByVal value As Integer)
            _GKrechts% = value
        End Set
    End Property
    Private _GKhoch%
    Public Property GKhoch() As Integer
        Get
            Return _GKhoch%
        End Get
        Set(ByVal value As Integer)
            _GKhoch% = value
        End Set
    End Property

    Public Shadows Function toString(ByVal delim$) As String
        Try
            Dim a$, wert$
            Dim summe$ = ""
            For Each pi As System.Reflection.PropertyInfo In Me.GetType().GetProperties()
                a$ = pi.Name
                wert$ = "=" & pi.GetValue(Me, Nothing).ToString
                summe &= a$ & wert$ & vbCrLf & delim$
            Next
            Return summe
        Catch ex As Exception
            Return "ERROR"
        End Try
    End Function
    Public Property gemeindebigNRstring As String
    Public Function gemeindeNrBig() As String
        Dim tbignr = "4380"
        Dim s$ = gemeindeNr.ToString
        Try
            If s$.StartsWith("438") Then Return s$ 'ist schon big
            If Val(s$) > 9 Then
                tbignr = "4380" & Val(s$).ToString
            Else
                tbignr = "43800" & Val(s$).ToString
            End If
            Return tbignr
        Catch ex As Exception
            Return "ERROR"
        End Try
    End Function

    Public Function makeHAString() As String
        Dim HA$ = "HA"
        Try
            Return HA$
        Catch ex As Exception
            Return "ERROR"
        End Try
    End Function
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function



    Public Event adresseunbrauchbar(ByVal obj As Object, ByVal data As Integer)

    'Public Function is_strasse_mit_hausnummer(ByVal suchstrasse$, _
    '                                      ByVal tempstrasse$, _
    '                                      ByVal temphausnummer%, _
    '                                      ByVal tempzusatz$) As Boolean
    '    Public Function is_strasse_mit_hausnummer() As Boolean
    '        Dim i%, anfang_nr%, ende_nr%
    '        Dim lStrasseName As String = _strasseName.Trim.ToLower
    '        Dim altelenge% = lStrasseName.Length

    '        Try
    '            ' Fehlerbehandlung aktivieren.   
    '            'mylog "strassehausnummer_zerlegen: ############################################## Eingang"
    '            'mylog "suchstrasse$  &  tempstrasse$:" & suchstrasse$ & " temps:" & tempstrasse$  
    '            'tempstrasse$ = ""
    '            'temphausnummer% = -4711
    '            'tempzusatz$ = ""

    '            anfang_nr = -1
    '            For i = 1 To Len(_strasseName)
    '                ''mylog  Mid$(suchstrasse$ &  i &  1)
    '                If IsNumeric(Mid$(_strasseName, i, 1)) Then
    '                    'mylog "DEBUCK strassehausnummer_zerlegen: anfangnr:" & i & Len(suchstrasse)
    '                    If anfang_nr < 1 Then
    '                        anfang_nr = i
    '                        GoTo zum_ende_der_hnr
    '                    End If
    '                End If
    '            Next
    '            'tempstrasse$ = suchstrasse$
    '            'temphausnummer% = -4711
    '            'tempzusatz$ = ""
    '            'mylog "strassehausnummer_zerlegen: ############################################## Ausgang keine zahl enthalten also raus hier"
    '            Return False    'keine zahl enthalten also raus hier

    'zum_ende_der_hnr:
    '            If anfang_nr = Len(_strasseName) Then
    '                'fertig
    '                ende_nr = anfang_nr
    '            End If
    'fertig:

    '            strasseName = Mid$(_strasseName, 1, anfang_nr - 1).Trim.ToLower
    '            _HausKombi = Mid$(lStrasseName, anfang_nr, Len(lStrasseName) - anfang_nr + 1)
    '        Catch ex As Exception

    '        End Try
    '    End Function
    Public Sub hauskombiZerlegen()
        Dim HK$ = _HausKombi.Trim
        Dim a$()
        Try
            If HK.Contains("-") Then
                a = HK.Split("-"c)
                If IsNumeric(a(0)) Then
                    _hausNr = CInt(a(0))
                    _hausZusatz = a(1)
                    Exit Sub
                End If
            End If

            If HK.Contains(" ") Then
                a = HK.Split(" "c)
                If IsNumeric(a(0)) Then
                    _hausNr = CInt(a(0))
                    _hausZusatz = a(1)
                    Exit Sub
                End If
            End If

            For i = 1 To Len(HK)
                If Not IsNumeric(Mid$(HK, i, 1)) Then
                    _hausNr = CInt(Mid$(HK, 1, i - 1))
                    _hausZusatz = Mid$(HK, i, HK.Length - i + 1)
                    Exit Sub
                End If
            Next

        Catch ex As Exception
            Dim FehlerHinweis$ = "Error / Fehler: " & vbCrLf +
             ex.Message + " " & vbCrLf +
             ex.StackTrace + " " & vbCrLf +
             ex.Source + " "
        End Try
    End Sub


    'Public Function gemcode2text2(ByVal auswahlspalte$, ByVal auswahlwert$, ByVal ausgabespalte$) As String

    'End Function

    Private disposedValue As Boolean = False        ' So ermitteln Sie überflüssige Aufrufe

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: Anderen Zustand freigeben (verwaltete Objekte).
            End If

            ' TODO: Eigenen Zustand freigeben (nicht verwaltete Objekte).
            ' TODO: Große Felder auf NULL festlegen.
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(ByVal disposing As Boolean) Bereinigungscode ein.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Shadows Function toString() As String
        Return gemeindeName & " " & strasseName & " " & HausKombi
    End Function

End Class
