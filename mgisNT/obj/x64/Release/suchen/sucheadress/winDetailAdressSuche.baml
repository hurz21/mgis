   M S B A M L   `   `   `  ���� 
  mgisNT    mgis.winDetailAdressSuche   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  AdressSuche��$	��819��$��798.205�� � WindowStartupLocation$ CenterScreen=� � 	Resources    XMLSourceComboBoxgemeinden  XMLSourceComboBoxRBfunktion  tbBackground �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value	 �� Binding
 �� Value  lvGemarkungsTemplate ��Name  
lessButton
 � Tag
 J� Uid  lvStrassenTemplate  lvhausnrnTemplate��%)  &	        &	      &	 
     &	     7  &	 �    7  &	 P    7  � � �� " ��.���  � ��
 ���� ��  � ��$
 Left=� P�  �@J�IsMouseOver . . ���  � ��$
 Bold"���  � ��
 �   ��� 	 �� *marker+
 0.a���  � ��
 ��  ��� 	 �� *marker+
 3.a���  � ��
 � � ��� .R��� $ txtitel��-   $��10,0,0,0q�$	��200��$
��Left=�
�������8��  ���� *displayText+ �� *displayText+ �� *id+�� .R��� $ txtitel��-   $��10,0,0,0q�$	��200��$��11(�$��20��$
��Left=�
�������8��  ���� *displayText+ �� *displayText+ �� *id+�� .R��� $ txtitel��-   $��0,0,0,0q�$��30��$��Center=�
�������8��  ���� *displayText+ �� *nenner+ �� *id+.���� .@��� $��Scharf��.����   flaechenBackground#��[ .+���  �� Orientation$ 
Horizontal=�.+��� $��5q�.+��� $ 
Horizontal=�$	��205��$
��Left=�.+��� -   $��ckFormNichtSchliessen��$��2q�.���� $,��&Formular im Anschluss nicht schliessen��$
��Wrap=�$	��199���� $ 
Horizontal=�.+��� -   $��
tbGemeinde��$��Gemeinde��$��5,0,0,0q� �� Padding$ 5q�$��90��$
��Left=��� -   $��cmbgemeinde�� �� DisplayMemberPath$ @Name�� �� SelectedValuePath$	 @ID��$��20��$��2�� � DataContext ��  �� Source# [   �� XPath$ /Variablen/Variable������  �� SelectedValue ��  �� Path$ ID��� -    ��Name$ lvGemarkungen��
�������$	��360��$	��245�� � Cursor$ Arrow��$	��350�����	#FFCCCCCC$��1,1,1,1q�$��Arial)�$��12(�$
��Left=�$��0q�#��[ �� -   $��spAlleBuchstaben��.+��� -	    ��Name$ stckBuchstaben1��$ 
Horizontal=� � ToolTip$, & Auf der Tastatur die Taste drücken !��{�. .+��� -
   $��	tbStrasse��$��Strasse:��$��5,0,0,0q�$ 5q�$	��157��$
��Left=��� $��Filter:��$��Center=�$ 5q��� -   $��tbfilter��$��39��$�� ��$��5,0,0,0q�
��������� -   $ 
lvStrassen��
�������$	��300��$	��245��$ Arrow��$
��Left=����	#FFCCCCCC$��1,1,1,1q�$��Arial)�$��12(�$��0q�#��[ ���� .R��� $	��230��$
��Left=��� $ 
Horizontal=�.+��� -   $��tbHausnr��$��Hausnr:��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -   $ lvHausnr��
�������$	��350��$	��245��$ Arrow�����	#FFCCCCCC$��1,1,1,1q�$��Arial)�$��12(�$��0q�#��[ ���� .R�� $	��230���� -   $��spNachnenner��$ 
Horizontal=�.+��� -   $��
btnDossier��$��Dossier��$��60��  	newButton#��[ $
��Left=�$��5q��� -   $��	tbFSTINFO��$	��200�� �� TextWrapping$
 Wrap=�$	��130�� �� AcceptsReturn . �� .+��� -   $��gbEigentuemer��$��Eigentümer��$��5,5,0,0q�$	��420��$
��Left=�{�.  .���� 
�������.+��� $ 
Horizontal=�.+��� $��Aktenzeichen��$ 5q�$	��100��(  "Xceed.Wpf.Toolkit.WatermarkTextBox  -     Name$ tbGrund��
�������   	Watermark$  hier Aktenzeichen eingeben��$	��200��$Q KGeben Sie hier eine Begründung für den Zugriff an. Z.B. das Aktenzeichen.���� $ 
Horizontal=�$y�	Collapsed=�.+��� $ ��Schnellabfrage in Kurzform��$ 10q��� -   $��cbSchnellEigentuemer�� d� 	IsChecked$
 TrueY�$��10q��� -   $��btnEigentuemer��$��Start��$
��Left=�$��
100,10,0,0q�$	��200���� $ 
Horizontal=�.+��� -   $��tbWeitergabeVerbot��$��5q�$
 Wrap=�$	��300��$
��Left=�$��
100,10,0,0q��� -   $��btnSchnellNachPDF��$	��PDF��$y�	Collapsed=��� -   $��spEigentNotizUebernehmen��.+��� $>��8Eigentümer-Notiz in den Paradigma-Vorgang übernehmen: ���� -   $!��btnEigentuemerNachParadigma��$��starte Übernahme��$
��Left=�$��
100,10,0,0q�$	��200���� -   $��gbFSTaradigma��$��Paradigma-Vorgang��$��10q�
�������.���� .+��� -   $��btnADRnachParadigma��$@��:Adresse als Raumbezug in den ParadigmaVorgang übernehmen ��$��10,10q��� $ 
Horizontal=�$��5q�.+��� $��	Funktion:��$��50��$h bFunktion oder Charakteristik dieser Adresse. In der Karte entspricht dies dem Etikett des Punktes.���� -   $��tbname��$	��274���� -     ��Name$  cmbFunktionsvorschlaege��$��6��$��2��! �� MaxDropDownHeight$	! 800��$��25��$ @Name��$	 @ID�� �� # [ $ /Variablen/Variable������  �� $ ID��� $ 
Horizontal=�$��5q�.+��� $��
Freitext: ��$��50���� -   $��
tbFreitext��$	��300���� -   $��btnAdresseNachParadigma��$	��200��$��starte Übernahme��$��5q��� -    $��
btnabbruch��$��60��" �� IsCancel" . $ Schliessen und zur Karte��$	��Top=�$��Right=�$��15q�$��Abbruch��#��[ �� $��Unscharf��$9 3Basierend auf den amtlichen Daten des Katasteramtes��.���� $��
10,10,0,10q�$	��324��$
��Left=�#��[ .+��� 	Baustelle�� $ 
Horizontal=�$	��150��.+��� -!   $��ckFormNichtSchliessen2��$��15q��� $,��&Formular im Anschluss nicht schliessen��$
��Wrap=�$��60��$��99���� $��1q�
���   �$��5q�._��� .+��� $ 
Horizontal=�$
��5,25q�.+��� $ 
Horizontal=�.+��� -"   $��tbGemeinde2��$��Gemeinde��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -#   $��cmbgemeinde2��$ @Name��$	 @ID��$��20��$��2�� �� # [  $ /Variablen/Variable������  �� $ ID��� -$   $��cbOhneGemeinde��$ FalseY�$ ��alle Gemeinden durchsuchen��$��5q��� $ 
Horizontal=�$
��5,25q�.+��� $��Filter:��$ 5q��� -%   $��
tbStrasse2��$�� ��$��5,0,0,0q�$��5q�$��90��$
��Left=��� -&   $��btnUnscharfSuchen��$��Strasse suchen���� .+��� $��Gefilterte Strassen:��$ 5q��� -'   $��cmbStrasse2��$	! 823��$ sname��$ strcode��$��10,0,0,0q����� �� $ 
Horizontal=�$
��5,25q�.+��� -(   $��	tbHausnr2��$��Hausnr:��$��5,0,0,0q�$ 5q�$��90��$
��Left=��� -)   $��
cmbHausnr2��$��42��$	! 823��$ hausnrkombi��$ hausnrkombi��$��2��$��1������ �� $��Auf Google-Basis��.��]� .+��� $ 
Horizontal=�$	��Top=�#��[ .+�  -*   $ tbGoogleEingabe��$ Dreieich, Frankfurter 23��$	��300��$��30��$��50,50,50,50q�$� �Geben Sie hier die Adresse so ein, wie Sie es von GoogleMaps gewohnt sind. Die Koordiaten werden aus der Google Datenbank geholt, aber im GIS dargestellt.���� -+   $��btnGoogleStrassensuche��$��Start��$	��100��$��30��