﻿#ExternalChecksum("..\..\..\MainWindow.xaml","{8829d00f-11b8-4213-878b-770e8597ac16}","895C5C14EA623D81A6792E963654DD3EC921440027F5BFA5BD06BFBEE2B5638B")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports baulastenBA
Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Shell


'''<summary>
'''MainWindow
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class MainWindow
    Inherits System.Windows.Window
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",28)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents cbOhneblnr0 As System.Windows.Controls.CheckBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",29)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnINIT As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",30)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnAlleBAs As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",31)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnNurKatOK As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",32)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnDBausschreiben As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",33)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnTIFF As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",34)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnTIFF2 As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",35)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnExplorer As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",40)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents tbinfo As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",43)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents dgMain As System.Windows.Controls.DataGrid
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainWindow.xaml",66)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents auswahlspalte As System.Windows.Controls.DataGridCheckBoxColumn
    
    #End ExternalSource
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")>  _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/baulastenBA;component/mainwindow.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\..\MainWindow.xaml",1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)
        
        #End ExternalSource
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")>  _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me.cbOhneblnr0 = CType(target,System.Windows.Controls.CheckBox)
            
            #ExternalSource("..\..\..\MainWindow.xaml",28)
            AddHandler Me.cbOhneblnr0.Click, New System.Windows.RoutedEventHandler(AddressOf Me.cbOhneblnr0_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 2) Then
            Me.btnINIT = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\MainWindow.xaml",29)
            AddHandler Me.btnINIT.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnINIT_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 3) Then
            Me.btnAlleBAs = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\MainWindow.xaml",30)
            AddHandler Me.btnAlleBAs.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnAlleBAs_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 4) Then
            Me.btnNurKatOK = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\MainWindow.xaml",31)
            AddHandler Me.btnNurKatOK.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnNurKatOK_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 5) Then
            Me.btnDBausschreiben = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\MainWindow.xaml",32)
            AddHandler Me.btnDBausschreiben.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnDBausschreiben_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 6) Then
            Me.btnTIFF = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\MainWindow.xaml",33)
            AddHandler Me.btnTIFF.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnTIFF_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 7) Then
            Me.btnTIFF2 = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\MainWindow.xaml",34)
            AddHandler Me.btnTIFF2.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnTIFFnormal_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 8) Then
            Me.btnExplorer = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\MainWindow.xaml",35)
            AddHandler Me.btnExplorer.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnExplorer_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 9) Then
            Me.tbinfo = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 10) Then
            Me.dgMain = CType(target,System.Windows.Controls.DataGrid)
            
            #ExternalSource("..\..\..\MainWindow.xaml",45)
            AddHandler Me.dgMain.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me.dgOSliste_SelectionChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 11) Then
            Me.auswahlspalte = CType(target,System.Windows.Controls.DataGridCheckBoxColumn)
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class

