   M S B A M L   `   `   `  ����   mgis    mgis.WinDetailSucheFST   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  GIS Flurstückssuche��$	��709��$	��907��
 � Top$	 100�� � Left$	 500�� � WindowStartupLocation$ CenterScreen=� � 	Resources     XMLSourceComboBoxgemarkungen  XMLSourceComboBoxRBfunktion��%   &	        &	      � � .���� $��0,0,0,0q� �� Orientation$ 
Horizontal=�  flaechenBackground#��[ .+��� $ 
Horizontal=�$��10,10,10,10q�.+��� .+��� $ 
Horizontal=�$	��150��.+��� -   $��ckFormNichtSchliessen��$��2q�.���� $,��&Formular im Anschluss nicht schliessen��$
��Wrap=�$��60��$��99���� $ 
Horizontal=�.+��� -   $��tbGemarkung��$��	Gemarkung��$��5,0,0,0q� �� Padding$ 5q�$��90��$
��Left=��� -   $��cmbgemarkung�� �� MaxDropDownHeight$	 523��$��25�� �� DisplayMemberPath$ @Name��	 �� SelectedValuePath$		 @ID��$��1,0,9,0q�
 � DataContext
 ��  �� Source# [   �� XPath$ /Variablen/Variable������  �� SelectedValue ��  �� Path$ ID��� $ 
Horizontal=� � ToolTip$) #In historischen Flurstücken suchen��.+��� -   $��tbFlur��$
��Flur��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -   $��cmbFlur��$	 723��$��25��$
 flur��$
	 flur��$��1,0,9,0q����� �� -   $��	cbFSTHist��$��	:Historie��$��5,5,5,5q�$C =Suche nach historischen Flurstücken. Suche dauert ca. 1-2min��$��RightToLeft=�$��64���� $ 
Horizontal=�$) #In historischen Flurstücken suchen��.+��� -   $��	tbZaehler��$��Zähler:��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -   $��
cmbZaehler��$	 723��$��25��$ zaehler��$	 zaehler��$��1,0,9,0q����� �� -	   $��hist1��$��Zähler:��$ 5q�$��50���� -
   $��cmbZaehlerHist��$	 723��$��25��$ zaehler��$	 zaehler��$��1,0,9,0q�$y�Visible=����� �� $ 
Horizontal=�$) #In historischen Flurstücken suchen��.+��� -   $��tbNenner��$��Nenner:��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -   $��	cmbNenner��$ nenner��$	 nenner��$��1,0,9,0q�$��25��$	 723������ �� -   $��hist2��$��Nenner:��$ 5q�$��50���� -   $��cmbNennerHist��$ nenner��$	 nenner��$��1,0,9,0q�$��25��$	 723������ �� -   $��spNachnenner��$ 
Horizontal=�.+��� -   $��
btnDossier��$��Dossier��$��60��  	newButton#��[ $
��Left=�$��5q��� -   $��
btnabbruch��$��60�� �� IsCancel . $ Schliessen und zur Karte��$��Abbruch��  
lessButton#��[ �� $��0,240q�$y�Hidden=�.+��� $/��)Suche nach untergegangenen Flurstücken: ��$
��Wrap=��� -   $��cbFSTalt��$��
aktivieren��$��5q��� -   $��spAuswahlAlt��$y�Hidden=�.+��� $;��5Wählen Sie die Datenbank in der gesucht werden soll:���� -   $��	cmbFSTalt��.@��� 
 � Tag$ aktuell��.��	Aktuell�� $
 2016��.��2016�� $
 2015��.��2015�� $
 2014��.��2014�� $
 2013��.��2013�� $
 2012��.��2012�� $
 2011��.��2011�� $
 2010��.��2010�� $
 2002��.��2002�� $
 2000��.��2000�� $
 1999��.��1999�� $
 1998��.��1998�� -   $��
fstaltinfo��$
��Wrap=��� $��10q��� .+��� -   $��gbEigentuemer��$��Eigentümer��$��	10,10,0,0q�$	��520��$
��Left=�{�.  .���� 
�������.+��� $ 
Horizontal=�.+��� $��Aktenzeichen��$ 5q�$	��100��(  "Xceed.Wpf.Toolkit.WatermarkTextBox  -     Name$ tbGrund��
�������   	Watermark$  hier Aktenzeichen eingeben��$	��200��$Q KGeben Sie hier eine Begründung für den Zugriff an. Z.B. das Aktenzeichen.���� $ 
Horizontal=�$y�Visible=�.+��� $ ��Schnellabfrage in Kurzform��$ 10q��� -   $��cbSchnellEigentuemer�� d� 	IsChecked$
 TrueY�$��10q��� -   $��btnEigentuemer��$��Start��$	��200��$
��Left=�$��100,10,10,10q��� $ 
Horizontal=�.+��� -   $��tbWeitergabeVerbot��$��5q� �� TextWrapping$
 Wrap=�$	��300��$
��Left=�$��100,10,10,10q��� -   $��btnSchnellNachPDF��$	��PDF��$y�	Collapsed=��� -   $��spEigentNotizUebernehmen��{�.  .+��� $>��8Eigentümer-Notiz in den Paradigma-Vorgang übernehmen: ���� -   $!��btnEigentuemerNachParadigma��$��starte Übernahme��$
��Left=�$��
100,10,0,0q�$	��200���� -   $��gbFSTaradigma��$��Paradigma-Vorgang��{�.  $��	10,10,0,0q�$	��520��$
��Left=�.���� .+��� 
�������.+��� -   $��btnADRnachParadigma��$D��>Flurstueck als Raumbezug in den Paradigma-Vorgang übernehmen ��$��10,10q��� $ 
Horizontal=�$��5q�.+��� $��	Funktion:��$��50��$i cFunktion oder Charakteristik dieser Adresse. In der Karte entspreicht dies dem Etikett des Punktes.���� -    $��tbKurz��$	��274���� -!    ��Name$ cmbFunktionsvorschlaege��$	 800��$��25��$ @Name��$		 @ID��
 �� # [ $ /Variablen/Variable������  �� $ ID��� $ 
Horizontal=�$��5q�.+��� $��
Freitext: ��$��50���� -"   $��
tbFreitext��$	��300���� -#   $ ��btnFlurstueckNachParadigma��$��starte Übernahme��$��5q�$	��200��