﻿#ExternalChecksum("..\..\..\werkzeuge\winWerkzeuge.xaml","{8829d00f-11b8-4213-878b-770e8597ac16}","9C400F2B89B23D9A41324F47C5FAB9E5F29AF88168F97958A8BDB0E3EE0F685A")
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

Imports bgm
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
'''winWerkzeuge
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class winWerkzeuge
    Inherits System.Windows.Window
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",19)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents tbPDFPfad As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",22)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnPDFtool As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",30)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnPruefung1 As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",38)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnPruefung2 As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",46)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnPruefung3 As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",59)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btndoppeltimgis As System.Windows.Controls.Button
    
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
        Dim resourceLocater As System.Uri = New System.Uri("/bgm;component/werkzeuge/winwerkzeuge.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",1)
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
            Me.tbPDFPfad = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 2) Then
            Me.btnPDFtool = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",22)
            AddHandler Me.btnPDFtool.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnPDFtool_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 3) Then
            Me.btnPruefung1 = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",30)
            AddHandler Me.btnPruefung1.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnPruefung1_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 4) Then
            Me.btnPruefung2 = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",38)
            AddHandler Me.btnPruefung2.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnPruefung2_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 5) Then
            Me.btnPruefung3 = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",46)
            AddHandler Me.btnPruefung3.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnPruefung3_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 6) Then
            Me.btndoppeltimgis = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\werkzeuge\winWerkzeuge.xaml",60)
            AddHandler Me.btndoppeltimgis.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btndoppeltimgis_Click)
            
            #End ExternalSource
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class

