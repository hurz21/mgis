   M S B A M L   `   `   `  ���� 
  mgisNT    mgis.WinDetailSucheFST   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  GIS Flurstückssuche��$	��809��$	��907��
 � Top$	 100�� � Left$	 500�� � WindowStartupLocation$ CenterScreen=� � 	Resources     XMLSourceComboBoxgemarkungen  XMLSourceComboBoxRBfunktion  tbBackground �� 
TargetType �� Property �� Value �� Triggers	 P� Property
 P� Value  lvGemarkungsTemplate ��Name  
lessButton
 � Tag
 J� Uid  lvFlureTemplate  lvFstkombiTemplate��%m  &	        &	      &	 
     &	 �     7  &	      7  &	 �    7  � � �� " ��.���  � ��
 ����  P� 	 �@J�IsMouseOver
 . . ���  � ��
 �������  � ��
 �   ��� .R��� $ txtitel��-   $��0q�$	��110��$��20��$��Center=�
�������8��  ���� *displayText+ �� *displayText+ �� *id+�� .R��� $ txtitel��-   $��0q�$��40��$��Center=�8��  ���� *displayText+ �� *displayText+ �� *id+�� .R��� $ txtitel��-   $��0q�8��  $��50��$��Center=����� *displayText+ �� *nenner+ �� *id+.���� $��0,0,0,0q� �� Orientation$ 
Horizontal=�  flaechenBackground#��[ .+��� $ 
Horizontal=�$��5,5,0,0q�.+��� .+��� $ 
Horizontal=�$	��205��$
��Left=�$��5,0,0,0q�.+��� -   $��ckFormNichtSchliessen��$��0q�.���� $,��&Formular im Anschluss nicht schliessen��$
��Wrap=�$	��199���� -    ��Name$ 	cbFSTHist��$'��!historische Flurstücke verwenden��$��5,0,0,0q�$
��Left=� � ToolTip$` ZSuche in historischen Flurstücken. Suche dauert mind. 1-2 min. Nur nutzen wenn sinnvoll !��$	��200���� $ Vertical=�$��5q�.+��� $��Ausgewählt: ���� -   $��tbresult��$��Black"��� $ Vertical=�.+��� $ 
Horizontal=�$��5q�.+��� -   $��tbGemarkung��$��
Gemarkung:��$��5,0,0,0q� �� Padding$ 0q�$��70��$
��Left=�$��normal"��� -   $��cmbgemarkung�� �� MaxDropDownHeight$	 523��$��25�� �� DisplayMemberPath$ @Name�� �� SelectedValuePath$	 @ID��$��5,0,0,0q� � DataContext ��  �� Source# [   �� XPath$ /Variablen/Variable������  �� SelectedValue ��  �� Path$ ID��� -	    ��Name$ lvGemarkungen��
�������$	��255�� � Cursor$ Arrow��$	��350�����	#FFCCCCCC$��1,1,1,1q�$��Arial)�$��11(�$��0q�#��[ ���� .R�� $	��240���� $ 
Horizontal=�$) #In historischen Flurstücken suchen���� -
   $��tbFlurnrWaelen��$ 
Horizontal=�$��5q�.+��� $��Flurnummer wählen: ���� -   $ lvFlure��
�������$	��300��$ Arrow�����	#FFCCCCCC$��1,1,1,1q�$��Arial)�$��12(�$��0q�#��[ ���� .R�� $	��240���� -   $��spFST��$ 
Horizontal=�$��5q�.+��� $ Vertical=�$��5q�.+��� $��Flurstück wählen��$��0q��� $ 
Horizontal=�$��15,5,0,0q�.+��� $��Filter:��$��Center=��� -   $��tbfilter��$��39��$�� ��$��15,0,0,0q�
��������� -   $��svMainScrollviewer����. $��Hidden=�$��Hidden=�#��[ $	��300��.���� -   $ 
lvFstkombi��
�������$	��300��$ Arrow�����	#FFCCCCCC$��1,1,1,1q�$��Arial)�$��12(�$��0q�#��[ ���� .R�� $	��240���� $ 
Horizontal=�.+��� -   $��
btnDossier��$��Dossier��$
��Left=�$��60��  	newButton#��[ $��5q��� .+��� -   $��gbEigentuemer��$��Eigentümer��$��5,5,0,0q�$	��520��$
��Left=�{�.  .���� 
�������.+��� $ 
Horizontal=�.+��� $��Aktenzeichen��$ 5q�$	��100��(  "Xceed.Wpf.Toolkit.WatermarkTextBox  -     Name$ tbGrund��
�������   	Watermark$  hier Aktenzeichen eingeben��$	��200��$Q KGeben Sie hier eine Begründung für den Zugriff an. Z.B. das Aktenzeichen.���� $ 
Horizontal=�$y�	Collapsed=�.+��� $ ��Schnellabfrage in Kurzform��$ 10q��� -   $��cbSchnellEigentuemer�� d� 	IsChecked$
 TrueY�$��10q��� -   $��btnEigentuemer��$��Start��$	��200��$
��Left=�$��100,10,10,10q��� $ 
Horizontal=�.+��� -   $��tbWeitergabeVerbot��$��5q� �� TextWrapping$
 Wrap=�$	��300��$
��Left=�$��100,10,10,10q��� -   $��btnSchnellNachPDF��$	��PDF��$y�	Collapsed=��� -   $��spEigentNotizUebernehmen��{�.  .+��� $>��8Eigentümer-Notiz in den Paradigma-Vorgang übernehmen: ���� -   $!��btnEigentuemerNachParadigma��$��starte Übernahme��$
��Left=�$��
100,10,0,0q�$	��200���� -   $��gbFSTaradigma��$��Paradigma-Vorgang��{�.  $��5,5,0,0q�$	��520��$
��Left=�.���� .+��� 
�������.+��� -   $��btnADRnachParadigma��$D��>Flurstueck als Raumbezug in den Paradigma-Vorgang übernehmen ��$��10,10q��� $ 
Horizontal=�$��5q�.+��� $��	Funktion:��$��50��$i cFunktion oder Charakteristik dieser Adresse. In der Karte entspreicht dies dem Etikett des Punktes.���� -   $��tbKurz��$	��274���� -     ��Name$  cmbFunktionsvorschlaege��$	 800��$��25��$ @Name��$	 @ID�� �� # [ $ /Variablen/Variable������  �� $ ID��� $ 
Horizontal=�$��5q�.+��� $��
Freitext: ��$��50���� -   $��
tbFreitext��$	��300���� -   $ ��btnFlurstueckNachParadigma��$��starte Übernahme��$��5q�$	��200���� -   $��
btnabbruch��$��60��! �� IsCancel! . $ Schliessen und zur Karte��$��Right=�$	��Top=�$��Abbruch��#��[ $��15q�