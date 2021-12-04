Namespace nsTrinkwasserClass
    Public Class Properties
        Public Property OBJECTID As String
        Public Property SHAPE As String
        Public Property WSG_ID As String
        Public Property ZONE As String
        Public Property WSG_KURZNAME As String
        Public Property WSG_ART As String
        Public Property STATUS_RPU As String
        Public Property KREIS_MASSGEBLICH_NAME As String
        Public Property KREIS_MASSGEBLICH_NR As String
        Public Property KREISE As String
        Public Property TK25_BEZEICHNUNGEN As String
        Public Property ARCHIV_HLNUG As String
        Public Property RPU As String
        Public Property STAATSANZEIGER As String
        Public Property STAATSANZEIGER_AENDER As String
        Public Property VERORDNUNGDATUM As String
        Public Property WSG_KEY As String
        Public Property ZONE_KEY As String
        Public Property SHAPE_Length As String
        Public Property SHAPE_Area As String
    End Class

    Public Class Feature
        Public Property type As String
        Public Property geometry As Object
        Public Property properties As Properties
        Public Property layerName As String
    End Class

    Public Class instanz
        Public Property type As String
        Public Property features As IList(Of Feature)
    End Class

End Namespace
