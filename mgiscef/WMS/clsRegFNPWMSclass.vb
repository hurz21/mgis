Imports System.Data

Namespace nsRegfnp
    Public Class Properties
        Public Property OBJECTID As String
        Public Property HEKTAR As String
        Public Property RF_AEND_INFODOC As String
        Public Property ALLG_AEND_NUMMER As String
        Public Property ALLG_STADT_GEM As String
        Public Property ALLG_BEZEICHNUNG As String
        Public Property ALLG_NUTZ As String
        Public Property ALLG_VERF_STAND As String
        Public Property GEN_RP_DATS As String
        Public Property GEN_STANZ_DAT As String
        Public Property GEN_URL_PDF As String
        Public Property Shape_Area As String
    End Class

    Public Class Feature
        Public Property type As String
        Public Property geometry As Object
        Public Property properties As Properties
        Public Property layerName As String
    End Class

    Public Class regfnpAenderungs
        Public Property type As String
        Public Property features As Feature()
    End Class

End Namespace
