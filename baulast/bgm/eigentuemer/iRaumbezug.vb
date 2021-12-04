Public Interface iRaumbezug
    Property id() As Long
    Property typ() As RaumbezugsTyp
    Property name() As String
    Property box() As clsRange
    Property punkt() As myPoint
    Property abstract() As String
    Property SekID() As Long
    Property Status() As Integer
    Property Freitext As String
    Property isMapEnabled As Boolean
    Property FLAECHEQM As double
    Property LAENGEM As Double
    Function PunktIsValid() As Boolean
    Property MITETIKETT() As Boolean
End Interface


