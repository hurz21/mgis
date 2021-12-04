Imports System.ComponentModel
Public Class clsFlurstueck
    'Inherits clsGEOPoint
    Implements ICloneable

    Implements INotifyPropertyChanged

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
     Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(ByVal prop As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub

    Public Property zeigtauf As String
    Public Property weistauf As String
    Public Property gebucht As String
    Public Property fsgml As String

    Property gemparms As New clsGemarkungsParams
    Public Shadows Function toString(ByVal delim As String) As String
        Dim a$, wert$
        Dim summe$ = ""
        Try
            For Each pi As System.Reflection.PropertyInfo In Me.GetType().GetProperties()
                a$ = pi.Name
                wert$ = String.Format("={0}", pi.GetValue(Me, Nothing))
                summe &= a$ & wert$ & vbCrLf & delim$
            Next
            Return summe
        Catch ex As Exception
            Return "ERROR"
        End Try
    End Function
    Private _gemarkungLiegtIMKreis As Boolean
    Public Property gemarkungLiegtIMKreis() As Boolean
        Get
            Return _gemarkungLiegtIMKreis
        End Get
        Set(ByVal value As Boolean)
            _gemarkungLiegtIMKreis = value
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
                _gemarkungLiegtIMKreis = False
            Else
                _gemarkungLiegtIMKreis = True
            End If
        End Set
    End Property
    Private _gemeindename$
    Public Property gemeindename() As String
        Get
            Return _gemeindename$
        End Get
        Set(ByVal value As String)
            _gemeindename$ = value
        End Set
    End Property
    Private _grundbuchblattnr$
    Public Property grundbuchblattnr() As String
        Get
            Return _grundbuchblattnr$
        End Get
        Set(ByVal value As String)
            _grundbuchblattnr$ = value
        End Set
    End Property
    Private _gemarkungstext$
    Public Property gemarkungstext() As String
        Get
            Return _gemarkungstext
        End Get
        Set(ByVal value As String)
            _gemarkungstext = value
            OnPropertyChanged("gemarkungstext")
            If _gemarkungstext.Trim.Length > 0 Then

                gemcode% = CInt(gemparms.gemarkungstext2gemcode(_gemarkungstext))
                _gemeindename = gemparms.gemcode2gemeindetext(gemcode%)
                _gemeindeNr = CInt(gemparms.gemcode2gemeindenr(_gemcode))
                If IsNumeric(_gemarkungstext) Then
                    _gemarkungstext = gemparms.gemcode2gemarkungstext(_gemcode)
                End If
                gemarkungstextNORM = clsString.normalizeSuchdbStrasse(_gemarkungstext)
                If _gemeindeNr < 1 Or _gemeindeNr > 13 Then
                    _gemarkungLiegtIMKreis = False
                    RaiseEvent flurstuckunbrauchbar(Me, 2)
                Else
                    _gemarkungLiegtIMKreis = True
                End If
            Else
                'RaiseEvent 
                RaiseEvent flurstuckunbrauchbar(Me, 1)
                'todo keine gemarkung angegeben
            End If
        End Set
    End Property
    Private _gemarkungstextNORM$
    Public Property gemarkungstextNORM() As String
        Get
            Return _gemarkungstextNORM
        End Get
        Set(ByVal value As String)
            _gemarkungstextNORM = value
            'OnPropertyChanged("gemarkungstext")
        End Set
    End Property
    Private _flur%
    Public Property flur() As Integer
        Get
            Return _flur%
        End Get
        Set(ByVal value As Integer)
            _flur% = value
            OnPropertyChanged("flur")
        End Set
    End Property
    Private _FS$
    Public Property FS() As String
        Get
            Return _FS$
        End Get
        Set(ByVal value As String)
            _FS$ = value
            OnPropertyChanged("FS")
        End Set
    End Property
    Private _gemcode%
    Public Property gemcode() As Integer
        Get
            Return _gemcode
        End Get
        Set(ByVal value As Integer)
            _gemcode = value
        End Set
    End Property
    Private _fstueckKombi$
    Public Property fstueckKombi() As String
        Get
            Return _fstueckKombi
        End Get
        Set(ByVal value As String)
            _fstueckKombi = value
            splitFstueckkombi()
        End Set
    End Property

    Private _zaehler%
    Public Property zaehler() As Integer
        Get
            Return _zaehler%
        End Get
        Set(ByVal value As Integer)
            _zaehler% = value
            OnPropertyChanged("zaehler")
        End Set
    End Property
    Private _nenner%
    Public Property nenner() As Integer
        Get
            Return _nenner%
        End Get
        Set(ByVal value As Integer)
            _nenner% = value
            OnPropertyChanged("nenner")
        End Set
    End Property
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MemberwiseClone()
    End Function
    Public Function buildFS() As String
        Dim fs$, fuell$, fs1$, fs2$, fs3$, fs4$
        Try
            If _nenner > 9999 Or _zaehler > 9999 Then Return "-4712"
            fs1$ = "FS060" & _gemcode%.ToString
            fuell = "000"                                                        '_flur = CInt(Val(flur$)).ToString
            fs2$ = fuell.Substring(_flur.ToString.Length) & _flur
            fuell = "00000"
            fs3 = fuell.Substring((_zaehler.ToString).Length) + _zaehler.ToString
            fuell = "000"
            fs4 = fuell.Substring((_nenner.ToString).Length) + (_nenner.ToString) + "00"
            fs = fs1 + fs2 + fs3 + fs4
            Return fs
        Catch ex As Exception
            Return "-4711"
        End Try
    End Function
    Public Function getPROBAUGGemcode(ByVal gemarkung As String) As Integer
        Try
            Select Case CInt(Val(gemarkung))
                Case 2, 35, 60
                    Return 732 'bayerseich,egeksbach,im bruehl
                Case 4
                    Return 730 'Dreieichenhain 
                Case 5
                    Return 756 ' 756	Sprendlingen 
                Case 6
                    Return 752 '	Offenthal
                Case 7
                    Return 734  'Götzenhain
                Case 8
                    Return 726  'Buchschlag
                Case 9
                    Return 736  'Hainstadt 
                Case 10
                    Return 740  'Klein-Krotzenburg
                Case 11
                    Return 753  'Rembrücken 
                Case 12
                    Return 744  'Mainflingen
                Case 13
                    Return 759  'Zellhausen
                Case 14
                    Return 742  'Lämmerspiel      
                Case 15
                    Return 728  'Dietesheim       
                Case 16
                    Return 750  'Obertshausen     
                Case 17
                    Return 737  'Hausen           
                Case 18
                    Return 760  'Zeppelinheim     
                Case 19, 42
                    Return 748  'gravenbruch ,Neu-Isenburg
                Case 20
                    Return 739  'Jügesheim                
                Case 21
                    Return 731  'Dudenhofen               
                Case 22
                    Return 747  'Nieder-Roden             
                Case 23
                    Return 735  'Hainhausen               
                Case 24
                    Return 758  'Weiskirchen              
                Case 25
                    Return 757  'Urberach                 
                Case 26, 25
                    Return 749  'Ober-Roden               
                Case 28
                    Return 745  'Messenhausen             
                Case 29
                    Return 733 '	Froschhausen            
                Case 30
                    Return 741 '	Klein-Welzheim
                Case 32
                    Return 738  'Heusenstamm    
                Case 34
                    Return 755  'Seligenstadt   
                Case 36
                    Return 746 '	Mühlheim      
                Case 41, 23
                    Return 743  'Langen,oberlinden
                Case 40, 33
                    Return 729  'Dietzenbach die 33 ist meine persönl. vermutung
                Case 50
                    Return 751  'Offenbach 
                Case Else
                    'unbekannte_gemarkungen$ &= gemarkung & "; "
                    Return 0
            End Select
        Catch ex As Exception
            'mylog.log("ERROR: getGemcode: " & _
            '          ex.Message + " " + _
            '         ex.Source + " ")
            Return -4711
        End Try
    End Function
    Public Sub New()

    End Sub

    Public Sub New(ByVal neuFS$)
        If neuFS$.Length = 21 Then
            splitOldFS(neuFS$)
        Else
            _zaehler = 0
            _nenner = 0
            _flur = 0
            _gemcode = 0
            _FS = ""
            _fstueckKombi = ""
        End If
        _grundbuchblattnr = ""
    End Sub
    Public Sub SetPartFromOldFS(ByVal lokFS As String)
        _gemcode = CInt(lokFS.Substring(4, 4))
        _flur = CInt(lokFS.Substring(8, 3))
        _zaehler = CInt(lokFS.Substring(11, 5))
        _nenner = CInt(lokFS.Substring(16, 3))
    End Sub
    Public Shared Function NewFS2OldFS(newfs As String) As String
        Try
            If Not newfs.ToLower.StartsWith("fs") Then
                Dim temp As String = newfs.Replace("_", "0")
                temp = temp.Replace(" ", "")
                temp = "FS" & temp
                Dim teil1, teil2 As String
                teil1 = temp.Substring(0, 16)
                teil2 = temp.Substring(17, 5)
                newfs = teil1 & teil2
                Return newfs
            Else
                Return newfs
            End If
        Catch ex As Exception
            nachricht("fehler in NewFS2OldFS: " & ex.ToString)
            Return "error"
        End Try
    End Function
    Shared Sub nachricht(t As String)
        My.Log.WriteEntry(t)
    End Sub
    Public Function splitOldFS(ByVal lokFS As String) As Boolean
        Try
            SetPartFromOldFS(lokFS)
            _fstueckKombi = _zaehler.ToString + "/" + _nenner.ToString

            _gemeindename = gemparms.gemcode2gemarkungstext(_gemcode)
            _gemarkungstext = gemparms.gemcode2gemarkungstext(_gemcode)
            _gemeindeNr = CType(gemparms.gemcode2gemeindenr(_gemcode), Integer)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function splitFstueckkombi() As Boolean
        Try
            Dim results = _fstueckKombi.Split("/"c)
            zaehler = CInt(results(0))
            If results.Length > 0 Then
                nenner = CInt(results(1))
            Else
                nenner = 0I
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function buildFstueckkombi() As String
        Try
            Return zaehler.ToString & "/" & nenner.ToString
        Catch ex As Exception
            Return "-1"
        End Try
    End Function
    Public Event flurstuckunbrauchbar(ByVal obj As Object, ByVal data As Integer)
    Public Property flaecheqm() As Double
    Private _GKrechts As Double
    Public Property GKrechts() As Double
        Get
            Return _GKrechts
        End Get
        Set(ByVal value As Double)
            _GKrechts = value
        End Set
    End Property
    Private _GKhoch As Double
    Public Property GKhoch() As Double
        Get
            Return _GKhoch
        End Get
        Set(ByVal value As Double)
            _GKhoch = value
        End Set
    End Property

    Public Property gid As Integer = 0
    Public Property gueltig As String = ""
    Public Property genese As Integer = 1

    Sub clear()
        gemarkungstext = ""
        gemcode = 0
        flur = 0
        zaehler = 0
        nenner = 0
        fstueckKombi = ""
        FS = ""
        flaecheqm = 0

    End Sub
End Class
