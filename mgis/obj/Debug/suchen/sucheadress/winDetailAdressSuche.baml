   M S B A M L   `   `   `  ����  mgis    mgis.winDetailAdressSuche   5   	   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � Title$  AdressSuche��5      $	��609��6   $��698.205��6&    � WindowStartupLocation$ CenterScreen=�5       � 	Resources 5         XMLSourceComboBoxgemeinden  XMLSourceComboBoxRBfunktion��%:   &	        &	      � 5	   
   6   � 5
   
   6   5      .��5      �� .@�5   
   �� $��Scharf��6    � ToolTip$9 3Basierend auf den amtlichen Daten des Katasteramtes��6"   .��5      ��   flaechenBackground#��[ 6   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��10q�6!   .+�5      �� $ 
Horizontal=�6%   $	��150��6>   .+�5      �� -   5        $��ckFormNichtSchliessen��5   '   $��2q�6l   .��5   "   �� $,��&Formular im Anschluss nicht schliessen��6,   $
��Wrap=�6Z   $��99��6q   5      5   )   �� 5      $ 
Horizontal=�6%   .+�5      �� -   5        $��
tbGemeinde��5   8   $��Gemeinde��6(   $��5,0,0,0q�6J    �� Padding$ 5q�6[   $��90��6g   $
��Left=�6r   6�   �� 5      -   5        $��cmbgemeinde��5   '    �� DisplayMemberPath$ @Name��5   A    �� SelectedValuePath$	 @ID��5   A   $��20��5!   '   $��2��62    � DataContext 5   (   �� 	 �� Source#	 [  
 �� XPath$
 /Variablen/Variable����5   A   ��  �� SelectedValue 5   A   ��  �� Path$ ID�5!   2   5"      �� 5#      $ 
Horizontal=�6%   .+�5$      �� -   5        $��	tbStrasse��5$   7   $��Strasse��6(   $��5,0,0,0q�6I   $ 5q�6[   $��90��6g   $
��Left=�6r   6�   �� 5%      -   5        $��
cmbStrasse��5%   '   $��20��5&   /    �� MaxDropDownHeight$	 823��6<   $ sname��5'   '   $ strcode��6B   $��2��6^   $��1��6q   ��5%   9   �� 5(      5)      �� 5*      $ 
Horizontal=�6%   .+�5+      �� -   5        $��tbHausnr��5+   7   $��Hausnr:��6(   $��5,0,0,0q�6H   $ 5q�6Z   $��90��6f   $
��Left=�6q   6�   �� 5,      -   5        $��	cmbHausnr��5,   '   $��20��5.   /   $	 823��6=   $ hausnrkombi��5/   (   $ hausnrkombi��6I   $��2��50   '   $��1��6:   ��5,   8   �� 51      52      �� 53      -   5        $��spNachnenner��53   >   $ 
Horizontal=�6%   .+�54      �� -	   5        $��
btnDossier��54   %   $��Dossier��67   $��60��55   %     	newButton#��[ 60   $
��Left=�6S   $��5q�6n   �� 56      -
   5        $��
btnabbruch��56   %   $��60��6;    �� IsCancel . $ Schliessen und zur Karte��6V   $��Abbruch��57   6     
lessButton#��[ 6I   58      �� 59      -   5        $��	tbFSTINFO��59   "   $	��200��63    �� TextWrapping$
 Wrap=�6B   $	��130��6V    �� AcceptsReturn . 5:   "   5<      �� 5=      .+�5@      �� -   5        $��gbEigentuemer��5@   7   $��Eigentümer��6#   $��	10,10,0,0q�6L   $	��520��6_   $
��Left=�6k   {�.  .��5A      �� 
�������.+�5B   "   �� $ 
Horizontal=�6-   .+�5D   &   �� $��Aktenzeichen��60   $ 5q�6D   $	��100��6P   (  "Xceed.Wpf.Toolkit.WatermarkTextBox  5G   &   -   5          Name$ tbGrund��5G   R   
�������   	Watermark$  hier Aktenzeichen eingeben��6c   $	��200��6�   $Q KGeben Sie hier eine Begründung für den Zugriff an. Z.B. das Aktenzeichen.��5H   <   �� 5I   &   $ 
Horizontal=�61   $y�Visible=�6J   .+�5J   *   �� $ ��Schnellabfrage in Kurzform��64   $ 10q�6V   �� 5K   *   -   5        $��cbSchnellEigentuemer��5K   3    d� 	IsChecked$
 TrueY�6O   $��10q�6b   5L   '   5N   #   �� 5O   "   -   5        $��btnEigentuemer��5O   )   $��Start��6\   $
��Left=�6l   $��
100,10,0,0q�6�   $	��200��6�   �� 5P   "   $ 
Horizontal=�6-   .+�5Q   &   �� -   5        $��tbWeitergabeVerbot��5Q   =   $��5q�61   $
 Wrap=�6W   $	��300��6k   $
��Left=�6w   $��
100,10,0,0q�6�   �� 5R   &   -   5        $��btnSchnellNachPDF��5R   -   $	��PDF��6f   $y�	Collapsed=�6t   5S   '   5T   #   �� 5U   "   -   5        $��spEigentNotizUebernehmen��5U   /   .+�5V   &   �� $>��8Eigentümer-Notiz in den Paradigma-Vorgang übernehmen: ��60   �� 5W   &   -   5        $!��btnEigentuemerNachParadigma��5W   -   $��starte Übernahme��6z   $
��Left=�6�   $��
100,10,0,0q�6�   $	��200��6�   5X   #   5Y      5Z      �� 5[      -   5        $��gbFSTaradigma��5[   >   $��Paradigma-Vorgang��6#   $��10q�6T   
�������.��5\      �� .+�5_   "   �� -   5        $��btnADRnachParadigma��5_   ,   $@��:Adresse als Raumbezug in den ParadigmaVorgang übernehmen ��6G   $��10,10q�5`   =   �� 5a   "   $ 
Horizontal=�6-   $��5q�6F   .+�5b   &   �� $��	Funktion:��60   $��50��6A   $h bFunktion oder Charakteristik dieser Adresse. In der Karte entspricht dies dem Etikett des Punktes.��6M   �� 5c   &   -   5        $��tbname��5c   .   $	��274��6<   6J   �� 5d   &   -   5         ��Name$ cmbFunktionsvorschlaege��5d   N   $��6��6/   $��2��6=   $	 800��6q   $��25��6�   $ @Name��5g   A   $	 @ID��5h   A    5e      �� #	 [ $
 /Variablen/Variable����5f   A   ��  5i   A   �� $ ID�5j   ?   5l   #   �� 5m   "   $ 
Horizontal=�6-   $��5q�6F   .+�5n   &   �� $��
Freitext: ��60   $��50��6B   �� 5o   &   -   5        $��
tbFreitext��5o   .   $	��300��6@   6N   5p   #   �� 5q   "   -   5        $��btnAdresseNachParadigma��5q   )   $	��200��6n   $��starte Übernahme��6z   $��5q�6�   5s      5t      5u      5w      5y      5z      �� 5{   
   $��Unscharf��6   $9 3Basierend auf den amtlichen Daten des Katasteramtes��6%   .��5|      �� $��
10,10,0,10q�6   $	��324��6-   $
��Left=�69   #��[ 6U   .+�5}      �� 	Baustelle6(   �� 5~      $ 
Horizontal=�6   $	��150��66   .+�5      �� -   5        $��ckFormNichtSchliessen2��5      $��15q�6f   �� 5�      $,��&Formular im Anschluss nicht schliessen��6    $
��Wrap=�6N   $��60��6c   $��99��6o   5�      �� 5�      $��1q�6   
���   �$��5q�6A   ._�5�      �� .+�5�      �� $ 
Horizontal=�6%   $
��5,25q�6>   .+�5�      �� $ 
Horizontal=�6)   .+�5�   "   �� -   5        $��tbGemeinde2��5�   <   $��Gemeinde��6,   $��5,0,0,0q�6O   $ 5q�6`   $��90��6l   $
��Left=�6w   6�   �� 5�   "   -   5        $��cmbgemeinde2��5�   +   $ @Name��5�   A   $	 @ID��5�   A   $��20��5�   '   $��2��62    5�   (   �� #	 [  $
 /Variablen/Variable����5�   A   ��  5�   A   �� $ ID�5�   2   �� 5�   "   -   5        $��cbOhneGemeinde��5�   -   $ FalseY�6C   $ ��alle Gemeinden durchsuchen��6U   $��5q�6z   5�      5�      �� 5�      $ 
Horizontal=�6%   $
��5,25q�6?   .+�5�      �� $��Filter:��6(   $ 5q�67   �� 5�      -   5        $��
tbStrasse2��5�   .   $�� ��6&   $��5,0,0,0q�6A   $��5q�6S   $��90��6_   $
��Left=�5�      6;   �� 5�      -   5        $��btnUnscharfSuchen��5�   %   $��Strasse suchen��6^   5�      �� 5�      .+�5�      �� $��Gefilterte Strassen:��6(   $ 5q�6D   �� 5�      -    5        $��cmbStrasse2��5�   '   $	 823��6�   $ sname��5�   '   $ strcode��6B   $��10,0,0,0q�6^   ��5�   :   �� 5�   ^   5�      �� 5�      $ 
Horizontal=�6%   $
��5,25q�6>   .+�5�      �� -!   5        $��	tbHausnr2��5�   7   $��Hausnr:��6(   $��5,0,0,0q�6I   $ 5q�6[   $��90��6g   $
��Left=�6r   6�   �� 5�      -"   5        $��
cmbHausnr2��5�   '   $��42��5�   /   $	 823��6=   $ hausnrkombi��5�   (   $ hausnrkombi��6I   $��2��6j   $��1��6}   ��5�   9   �� 5�      5�      5�      5�      5�      5�      �� 5�   
   $��Auf Google-Basis��6   .��5�      ]� .+�5�      �� $ 
Horizontal=�6   $	��Top=�66   #��[ 6K   .+�5�        -#   5        $ tbGoogleEingabe��5�   1   $ Dreieich, Frankfurter 23��6J   $	��300��5�   /   $��30��6;   $��50,50,50,50q�6H   $� �Geben Sie hier die Adresse so ein, wie Sie es von GoogleMaps gewohnt sind. Die Koordiaten werden aus der Google Datenbank geholt, aber im GIS dargestellt.��5�       �� 5�      -$   5        $��btnGoogleStrassensuche��5�      $��Start��6`   $	��100��6p   $��30��6|   5�      5�      5�      5�      5�      