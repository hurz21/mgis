   M S B A M L   `   `   `  ����   mgis    mgis.winDetailAdressSuche   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  AdressSuche��$	��609��$��698.205�� � WindowStartupLocation$ CenterScreen=� � 	Resources    XMLSourceComboBoxgemeinden  XMLSourceComboBoxRBfunktion��%   &	        &	      � � .���� .@��� $��Scharf�� � ToolTip$9 3Basierend auf den amtlichen Daten des Katasteramtes��.����   flaechenBackground#��[ .+���  �� Orientation$ 
Horizontal=�.+��� $��10q�.+��� $ 
Horizontal=�$	��150��.+��� -   $��ckFormNichtSchliessen��$��2q�.���� $,��&Formular im Anschluss nicht schliessen��$
��Wrap=�$��99���� $ 
Horizontal=�.+��� -   $��
tbGemeinde��$��Gemeinde��$��5,0,0,0q� �� Padding$ 5q�$��90��$
��Left=��� -   $��cmbgemeinde�� �� DisplayMemberPath$ @Name�� �� SelectedValuePath$	 @ID��$��20��$��2�� � DataContext �� 	 �� Source#	 [  
 �� XPath$
 /Variablen/Variable������  �� SelectedValue ��  �� Path$ ID��� $ 
Horizontal=�.+��� -   $��	tbStrasse��$��Strasse��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -   $��
cmbStrasse��$��20�� �� MaxDropDownHeight$	 823��$ sname��$ strcode��$��2��$��1������ �� $ 
Horizontal=�.+��� -   $��tbHausnr��$��Hausnr:��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -   $��	cmbHausnr��$��20��$	 823��$ hausnrkombi��$ hausnrkombi��$��2��$��1������ �� -   $��spNachnenner��$ 
Horizontal=�.+��� -	   $��
btnDossier��$��Dossier��$��60��  	newButton#��[ $
��Left=�$��5q��� -
   $��
btnabbruch��$��60�� �� IsCancel . $ Schliessen und zur Karte��$��Abbruch��  
lessButton#��[ �� -   $��	tbFSTINFO��$	��200�� �� TextWrapping$
 Wrap=�$	��130�� �� AcceptsReturn . �� .+��� -   $��gbEigentuemer��$��Eigentümer��$��	10,10,0,0q�$	��520��$
��Left=�{�.  .���� 
�������.+��� $ 
Horizontal=�.+��� $��Aktenzeichen��$ 5q�$	��100��(  "Xceed.Wpf.Toolkit.WatermarkTextBox  -     Name$ tbGrund��
�������   	Watermark$  hier Aktenzeichen eingeben��$	��200��$Q KGeben Sie hier eine Begründung für den Zugriff an. Z.B. das Aktenzeichen.���� $ 
Horizontal=�$y�Visible=�.+��� $ ��Schnellabfrage in Kurzform��$ 10q��� -   $��cbSchnellEigentuemer�� d� 	IsChecked$
 TrueY�$��10q��� -   $��btnEigentuemer��$��Start��$
��Left=�$��
100,10,0,0q�$	��200���� $ 
Horizontal=�.+��� -   $��tbWeitergabeVerbot��$��5q�$
 Wrap=�$	��300��$
��Left=�$��
100,10,0,0q��� -   $��btnSchnellNachPDF��$	��PDF��$y�	Collapsed=��� -   $��spEigentNotizUebernehmen��.+��� $>��8Eigentümer-Notiz in den Paradigma-Vorgang übernehmen: ���� -   $!��btnEigentuemerNachParadigma��$��starte Übernahme��$
��Left=�$��
100,10,0,0q�$	��200���� -   $��gbFSTaradigma��$��Paradigma-Vorgang��$��10q�
�������.���� .+��� -   $��btnADRnachParadigma��$@��:Adresse als Raumbezug in den ParadigmaVorgang übernehmen ��$��10,10q��� $ 
Horizontal=�$��5q�.+��� $��	Funktion:��$��50��$h bFunktion oder Charakteristik dieser Adresse. In der Karte entspricht dies dem Etikett des Punktes.���� -   $��tbname��$	��274���� -    ��Name$ cmbFunktionsvorschlaege��$��6��$��2��$	 800��$��25��$ @Name��$	 @ID�� �� #	 [ $
 /Variablen/Variable������  �� $ ID��� $ 
Horizontal=�$��5q�.+��� $��
Freitext: ��$��50���� -   $��
tbFreitext��$	��300���� -   $��btnAdresseNachParadigma��$	��200��$��starte Übernahme��$��5q��� $��Unscharf��$9 3Basierend auf den amtlichen Daten des Katasteramtes��.���� $��
10,10,0,10q�$	��324��$
��Left=�#��[ .+��� 	Baustelle�� $ 
Horizontal=�$	��150��.+��� -   $��ckFormNichtSchliessen2��$��15q��� $,��&Formular im Anschluss nicht schliessen��$
��Wrap=�$��60��$��99���� $��1q�
���   �$��5q�._��� .+��� $ 
Horizontal=�$
��5,25q�.+��� $ 
Horizontal=�.+��� -   $��tbGemeinde2��$��Gemeinde��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -   $��cmbgemeinde2��$ @Name��$	 @ID��$��20��$��2�� �� #	 [  $
 /Variablen/Variable������  �� $ ID��� -   $��cbOhneGemeinde��$ FalseY�$ ��alle Gemeinden durchsuchen��$��5q��� $ 
Horizontal=�$
��5,25q�.+��� $��Filter:��$ 5q��� -   $��
tbStrasse2��$�� ��$��5,0,0,0q�$��5q�$��90��$
��Left=��� -   $��btnUnscharfSuchen��$��Strasse suchen���� .+��� $��Gefilterte Strassen:��$ 5q��� -    $��cmbStrasse2��$	 823��$ sname��$ strcode��$��10,0,0,0q����� �� $ 
Horizontal=�$
��5,25q�.+��� -!   $��	tbHausnr2��$��Hausnr:��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -"   $��
cmbHausnr2��$��42��$	 823��$ hausnrkombi��$ hausnrkombi��$��2��$��1������ �� $��Auf Google-Basis��.��]� .+��� $ 
Horizontal=�$	��Top=�#��[ .+�  -#   $ tbGoogleEingabe��$ Dreieich, Frankfurter 23��$	��300��$��30��$��50,50,50,50q�$� �Geben Sie hier die Adresse so ein, wie Sie es von GoogleMaps gewohnt sind. Die Koordiaten werden aus der Google Datenbank geholt, aber im GIS dargestellt.���� -$   $��btnGoogleStrassensuche��$��Start��$	��100��$��30��