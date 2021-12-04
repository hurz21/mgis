Imports System.Data
Imports System.Data.Common

'Namespace LIBDB
Public Interface IDB_grundfunktionen
        Function dboeffnen(ByRef resultstring As String) As Integer
        Function dbschliessen(ByRef resultstring As String) As Integer
        Function doConnection(ByRef hinweis As String) As Boolean ' As System.Data.Common.DbConnection
        Function getDataDT() As String
        Function sqlexecute(ByRef newID&) As Long
        Property mycount() As Long
        Property dt() As DataTable
        Property mydb() As clsDatenbankZugriff
        Function manipquerie(query As String, slqparamlist As List(Of clsSqlparam), ReturnIdentity As Boolean,
                             returnColumn As String) As Integer
    End Interface

'End Namespace
