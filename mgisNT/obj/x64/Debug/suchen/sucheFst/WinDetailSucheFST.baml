   M S B A M L   `   `   `  ����
  mgisNT    mgis.WinDetailSucheFST   5   	   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � Title$  GIS Flurstückssuche��5      $	��809��6!   $	��907��6.   
 � Top$	 100��6;    � Left$	 500��6E    � WindowStartupLocation$ CenterScreen=�5   
    � 	Resources 5          XMLSourceComboBoxgemarkungen  XMLSourceComboBoxRBfunktion  tbBackground �� 
TargetType �� Property �� Value �� Triggers	 P� Property
 P� Value  lvGemarkungsTemplate ��Name  
lessButton
 � Tag
 J� Uid  lvFlureTemplate  lvFstkombiTemplate��%�  &	        &	      &	 &     &	 �     7  &	 �    7  &	 �    7  � 5   
   6   � 5	   
   6   �� 5
   
   " ��6%   .�5      ��  � ��
 ���� 6,    5      P� 5      	 �@J�IsMouseOver
 . . �5      ��  � ��
 �����63   �� 5       � ��
 �   �63   5      5      �� 5   
   .R�5      �� $ txtitel��6g   -   5        $��0q�5   #   $	��110��62   $��20��6>   $��Center=�5   B   
�������8��  ��5      �� *displayText+ 68   �� *displayText+ 6T   �� *id+5      5      �� 5   
   .R�5      �� $ txtitel��6g   -   5        $��0q�5    "   $��40��63   $��Center=�5"      8��  ��5      �� *displayText+ 68   �� *displayText+ 6T   �� *id+5%      5&      �� 5'   
   .R�5(      �� $ txtitel��6b   -   5        $��0q�5)   #   8��  $��50��6Y   $��Center=�5*   ;   ��5(      �� *displayText+ 68   �� *nenner+ 6O   �� *id+5,      5-      5/      .��50      �� $��0,0,0,0q�6    �� Orientation$ 
Horizontal=�6"     flaechenBackground#��[ 6;   .+�52   
   �� $ 
Horizontal=�6   $��5,5,0,0q�6.   .+�53      �� .+�54      �� $ 
Horizontal=�6   $	��205��66   $
��Left=�6C   $��5,0,0,0q�6^   .+�55      �� -   5        $��ckFormNichtSchliessen��55      $��0q�6d   .��58      �� $,��&Formular im Anschluss nicht schliessen��6$   $
��Wrap=�6R   $	��199��6i   5:      5;      �� 5<      -   5         ��Name$ 	cbFSTHist��5<      $'��!historische Flurstücke verwenden��6.   $��5,0,0,0q�6Y   $
��Left=�6�    � ToolTip$` ZSuche in historischen Flurstücken. Suche dauert mind. 1-2 min. Nur nutzen wenn sinnvoll !��5=      $	��200��5>      �� 5?      $ Vertical=�6   $��5q�64   .+�5@      �� $��Ausgewählt: ��6    �� 5A      -   5        $��tbresult��5A       $��Black"�60   5B      �� 5C      $ Vertical=�6   .+�5D      �� $ 
Horizontal=�6!   $��5q�6:   .+�5E      �� -   5        $��tbGemarkung��5E   6   $��
Gemarkung:��6$   $��5,0,0,0q�6I    �� Padding$ 0q�6Z   $��70��6f   $
��Left=�6q   $��normal"�6�   6�   �� 5F      -   5        $��cmbgemarkung��5F   #    �� MaxDropDownHeight$	 523��67   $��25��6O    �� DisplayMemberPath$ @Name��5I   A    �� SelectedValuePath$	 @ID��5J   A   $��5,0,0,0q�5L   q    � DataContext 5G   (   ��  �� Source# [   �� XPath$ /Variablen/Variable����5H   A   ��  �� SelectedValue 5K   A   ��  �� Path$ ID�5L   q   5N      �� 5O      -	   5         ��Name$ lvGemarkungen��5Q      
�������$	��255��5O   ;    � Cursor$ Arrow��5P      $	��350��6(   ���	#FFCCCCCC$��1,1,1,1q�5S      $��Arial)�5T      $��11(�5U      $��0q�5V      #��[ 5W      ��5Y      �� 5Z      .R�5[   "   � $	��240��6,   5\      5^      5`      �� 5a      $ 
Horizontal=�6   $) #In historischen Flurstücken suchen��66   5h      �� 5i      -
   5        $��tbFlurnrWaelen��5i   A   $ 
Horizontal=�6   $��5q�66   .+�5m      �� $��Flurnummer wählen: ��6    5o      �� 5t      -   5        $ lvFlure��5v      
�������$	��300��5t   7   $ Arrow��5u      ���	#FFCCCCCC$��1,1,1,1q�5x      $��Arial)�5y      $��12(�5z      $��0q�5{      #��[ 5}      ��5      �� 5�      .R�5�      � $	��240��6(   5�      5�      �� 5�      -   5        $��spFST��5�   B   $ 
Horizontal=�6   $��5q�67   .+�5�      �� $ Vertical=�6!   $��5q�68   .+�5�      �� $��Flurstück wählen��6$   $��0q�6<   5�      �� 5�      $ 
Horizontal=�6!   $��15,5,0,0q�6:   .+�5�      �� $��Filter:��6$   $��Center=�63   �� 5�      -   5        $��tbfilter��5�   "   $��39��62   $�� ��6=   $��15,0,0,0q�6F   
�������6k   5�      5�      �� 5�      -   5        $��svMainScrollviewer��5�      ��. $��Hidden=�6R   $��Hidden=�5�      #��[ 5�   #   $	��300��5�   "   .��5�      �� -   5        $ 
lvFstkombi��5�      
�������$	��300��5�   ;   $ Arrow��5�      ���	#FFCCCCCC$��1,1,1,1q�5�      $��Arial)�5�      $��12(�5�      $��0q�5�      #��[ 5�      ��5�      �� 5�      .R�5�   "   � $	��240��6,   5�      5�      5�      �� 5�      $ 
Horizontal=�6   .+�5�      �� -   5        $��
btnDossier��5�      $��Dossier��6/   $
��Left=�6Z   $��60��5�   %     	newButton#��[ 60   $��5q�6U   5�      5�      5�      �� 5�   
   .+�5�      �� -   5        $��gbEigentuemer��5�   +   $��Eigentümer��6   $��5,5,0,0q�6@   $	��520��6Q   $
��Left=�5�      {�.  .��5�      �� 
�������.+�5�      �� $ 
Horizontal=�6!   .+�5�      �� $��Aktenzeichen��6$   $ 5q�68   $	��100��6D   (  "Xceed.Wpf.Toolkit.WatermarkTextBox  5�      -   5          Name$ tbGrund��5�   F   
�������   	Watermark$  hier Aktenzeichen eingeben��6W   $	��200��6   $Q KGeben Sie hier eine Begründung für den Zugriff an. Z.B. das Aktenzeichen.��5�   <   �� 5�      $ 
Horizontal=�6%   $y�	Collapsed=�6>   .+�5�      �� $ ��Schnellabfrage in Kurzform��6(   $ 10q�6J   �� 5�      -   5        $��cbSchnellEigentuemer��5�   '    d� 	IsChecked$
 TrueY�6C   $��10q�6V   5�      5�      �� 5�      -   5        $��btnEigentuemer��5�      $��Start��6P   $	��200��6b   $
��Left=�6n   $��100,10,10,10q�6�   �� 5�      $ 
Horizontal=�6!   .+�5�      �� -   5        $��tbWeitergabeVerbot��5�   1   $��5q�6%    �� TextWrapping$
 Wrap=�6K   $	��300��6_   $
��Left=�6k   $��100,10,10,10q�6�   �� 5�      -   5        $��btnSchnellNachPDF��5�   !   $	��PDF��6Z   $y�	Collapsed=�6h   5�      5�      �� 5�      -   5        $��spEigentNotizUebernehmen��5�   #   {�.  .+�5�      �� $>��8Eigentümer-Notiz in den Paradigma-Vorgang übernehmen: ��6$   �� 5�      -   5        $!��btnEigentuemerNachParadigma��5�   !   $��starte Übernahme��5�   !   $
��Left=�6<   $��
100,10,0,0q�6W   $	��200��6k   5�      5�      5�      �� 5�      -   5        $��gbFSTaradigma��5�   2   $��Paradigma-Vorgang��6   {�.  $��5,5,0,0q�6Z   $	��520��6k   $
��Left=�6w   .��5�      �� .+�5�      �� 
�������.+�5�      �� -   5        $��btnADRnachParadigma��5�   $   $D��>Flurstueck als Raumbezug in den Paradigma-Vorgang übernehmen ��6?   $��10,10q�5�   =   �� 5�      $ 
Horizontal=�6%   $��5q�6>   .+�5�      �� $��	Funktion:��6(   $��50��69   $i cFunktion oder Charakteristik dieser Adresse. In der Karte entspreicht dies dem Etikett des Punktes.��6E   �� 5�      -   5        $��tbKurz��5�   &   $	��274��64   6B   �� 5�      -   5          ��Name$  cmbFunktionsvorschlaege��5�   +   $	 800��6N   $��25��6f   $ @Name��5  A   $	 @ID��5  A    5�      �� # [ $ /Variablen/Variable����5   A   ��  5  A   �� $ ID�5  ?   5     �� 5     $ 
Horizontal=�6%   $��5q�6>   .+�5     �� $��
Freitext: ��6(   $��50��6:   �� 5	     -   5        $��
tbFreitext��5	  &   $	��300��68   6F   5
     �� 5     -   5        $ ��btnFlurstueckNachParadigma��5  !   $��starte Übernahme��6l   $��5q�6�   $	��200��5  !   5     5     5     5     �� 5  
   -   5        $��
btnabbruch��5     $��60��6'   ! �� IsCancel! . $ Schliessen und zur Karte��6B   $��Right=�5     $	��Top=�6-   $��Abbruch��5  6   #��[ 6I   $��15q�6m   5     5     