﻿#ExternalChecksum("..\..\..\Bestand\winBestand.xaml","{8829d00f-11b8-4213-878b-770e8597ac16}","7A0F05AC10D4F65F46E0E51A0AC0C76BF462DB8BFC01AD51BBB6C5F80F0D4B92")
'------------------------------------------------------------------------------
' <auto-generated>
'     Dieser Code wurde von einem Tool generiert.
'     Laufzeitversion:4.0.30319.42000
'
'     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
'     der Code erneut generiert wird.
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
'''winBestand
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class winBestand
    Inherits System.Windows.Window
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\..\Bestand\winBestand.xaml",33)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents cmbgemarkung As System.Windows.Controls.ComboBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Bestand\winBestand.xaml",43)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents tbTreffer As System.Windows.Controls.TextBlock
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Bestand\winBestand.xaml",45)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents dgBestand As System.Windows.Controls.DataGrid
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Bestand\winBestand.xaml",76)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnPROBAUGinit As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Bestand\winBestand.xaml",90)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents dgProbaug As System.Windows.Controls.DataGrid
    
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
        Dim resourceLocater As System.Uri = New System.Uri("/bgm;component/bestand/winbestand.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\..\Bestand\winBestand.xaml",1)
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
            Me.cmbgemarkung = CType(target,System.Windows.Controls.ComboBox)
            
            #ExternalSource("..\..\..\Bestand\winBestand.xaml",39)
            AddHandler Me.cmbgemarkung.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me.cmbgemarkung_SelectionChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 2) Then
            Me.tbTreffer = CType(target,System.Windows.Controls.TextBlock)
            Return
        End If
        If (connectionId = 3) Then
            Me.dgBestand = CType(target,System.Windows.Controls.DataGrid)
            
            #ExternalSource("..\..\..\Bestand\winBestand.xaml",48)
            AddHandler Me.dgBestand.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me.dgBestand_SelectionChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 4) Then
            Me.btnPROBAUGinit = CType(target,System.Windows.Controls.Button)
            
            #ExternalSource("..\..\..\Bestand\winBestand.xaml",76)
            AddHandler Me.btnPROBAUGinit.Click, New System.Windows.RoutedEventHandler(AddressOf Me.btnPROBAUGinit_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 5) Then
            Me.dgProbaug = CType(target,System.Windows.Controls.DataGrid)
            
            #ExternalSource("..\..\..\Bestand\winBestand.xaml",93)
            AddHandler Me.dgProbaug.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me.dgProbaug_SelectionChanged)
            
            #End ExternalSource
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class

