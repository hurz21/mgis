   M S B A M L   `   `   `  ����  mgis    mgis.WinDetailSucheFST   5   	   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � Title$  GIS Flurstückssuche��5      $	��709��6!   $	��907��6.   
 � Top$	 100��6;    � Left$	 500��6E    � WindowStartupLocation$ CenterScreen=�6Q    � 	Resources 5          XMLSourceComboBoxgemarkungen  XMLSourceComboBoxRBfunktion��%:   &	        &	      � 5   
   6   � 5   
   6   5	      .��5
      �� $��0,0,0,0q�6    �� Orientation$ 
Horizontal=�6"     flaechenBackground#��[ 6;   .+�5   
   �� $ 
Horizontal=�6   $��10,10,10,10q�6.   .+�5      �� .+�5      �� $ 
Horizontal=�6   $	��150��66   .+�5      �� -   5        $��ckFormNichtSchliessen��5      $��2q�6d   .��5      �� $,��&Formular im Anschluss nicht schliessen��6$   $
��Wrap=�6R   $��60��6g   $��99��6s   5      5      �� 5      $ 
Horizontal=�6   .+�5      �� -   5        $��tbGemarkung��5   1   $��	Gemarkung��6    $��5,0,0,0q�6D    �� Padding$ 5q�6U   $��90��6a   $
��Left=�6l   6�   �� 5      -   5        $��cmbgemarkung��5       �� MaxDropDownHeight$	 523��63   $��25��6K    �� DisplayMemberPath$ @Name��5   A   	 �� SelectedValuePath$		 @ID��5   A   $��1,0,9,0q�5    q   
 � DataContext
 5   (   ��  �� Source# [   �� XPath$ /Variablen/Variable����5   A   ��  �� SelectedValue 5   A   ��  �� Path$ ID�5    q   5"      �� 5#      $ 
Horizontal=�6    � ToolTip$) #In historischen Flurstücken suchen��66   .+�5$      �� -   5        $��tbFlur��5$   ,   $
��Flur��6    $��5,0,0,0q�6;   $ 5q�6M   $��90��6Y   $
��Left=�6d   6�   �� 5%      -   5        $��cmbFlur��5%      $	 723��6G   $��25��6_   $
 flur��5&   '   $
	 flur��6A   $��1,0,9,0q�6\   ��5%   .   �� 5'      �� 5(      -   5        $��	cbFSTHist��5(      $��	:Historie��60   $��5,5,5,5q�6D   $C =Suche nach historischen Flurstücken. Suche dauert ca. 1-2min��5)      $��RightToLeft=�6f   $��64��6�   5+      �� 5,      $ 
Horizontal=�6   $) #In historischen Flurstücken suchen��67   .+�5-      �� -   5        $��	tbZaehler��5-   /   $��Zähler:��6    $��5,0,0,0q�6A   $ 5q�6S   $��90��6_   $
��Left=�6j   6�   �� 5.      -   5        $��
cmbZaehler��5.      $	 723��6y   $��25��6�   $ zaehler��5/   '   $	 zaehler��6D   $��1,0,9,0q�6b   ��5.   1   �� 5/   b   �� 50      -	   5        $��hist1��50   ;   $��Zähler:��6    $ 5q�6/   $��50��6H   �� 51      -
   5        $��cmbZaehlerHist��51      $	 723��6�   $��25��6�   $ zaehler��52   '   $	 zaehler��6D   $��1,0,9,0q�6b   $y�Visible=�6t   ��51   5   �� 52   t   54      �� 55      $ 
Horizontal=�6   $) #In historischen Flurstücken suchen��67   .+�56      �� -   5        $��tbNenner��56   /   $��Nenner:��6    $��5,0,0,0q�6@   $ 5q�6R   $��90��6^   $
��Left=�6i   6�   �� 57      -   5        $��	cmbNenner��57      $ nenner��58   '   $	 nenner��6C   $��1,0,9,0q�6a   $��25��6t   $	 723��59   W   ��57   0   �� 59   W   �� 5:      -   5        $��hist2��5:   /   $��Nenner:��6    $ 5q�6<   $��50��6H   �� 5;      -   5        $��cmbNennerHist��5;      $ nenner��5<   '   $	 nenner��6C   $��1,0,9,0q�6a   $��25��6t   $	 723��5=   [   ��5;   4   �� 5=   [   5>      �� 5?      -   5        $��spNachnenner��5?   6   $ 
Horizontal=�6   .+�5@      �� -   5        $��
btnDossier��5@      $��Dossier��6/   $��60��5A   %     	newButton#��[ 60   $
��Left=�6S   $��5q�6n   �� 5B      -   5        $��
btnabbruch��5B      $��60��63    �� IsCancel . $ Schliessen und zur Karte��6N   $��Abbruch��5C   6     
lessButton#��[ 6I   5D      �� 5E      $��0,240q�6   $y�Hidden=�6,   .+�5F      �� $/��)Suche nach untergegangenen Flurstücken: ��6    $
��Wrap=�6P   �� 5G      -   5        $��cbFSTalt��5G      $��
aktivieren��6/   $��5q�6D   6O   �� 5H      -   5        $��spAuswahlAlt��5H   !   $y�Hidden=�65   .+�5I      �� $;��5Wählen Sie die Datenbank in der gesucht werden soll:��6"   �� 5J      -   5        $��	cmbFSTalt��5J   #   .@�5K      �� 
 � Tag$ aktuell��6'   .��65   	Aktuell6>   �� 5L      $
 2016��6'   .��62   201668   �� 5M      $
 2015��6'   .��62   201568   �� 5N      $
 2014��6'   .��62   201468   �� 5O      $
 2013��6'   .��62   201368   �� 5P      $
 2012��6'   .��62   201268   �� 5Q      $
 2011��6'   .��62   201168   �� 5R      $
 2010��6'   .��62   201068   �� 5S      $
 2002��6'   .��62   200268   �� 5T      $
 2000��6'   .��62   200068   �� 5U      $
 1999��6'   .��62   199968   �� 5V      $
 1998��6'   .��62   199868   5W      �� 5X      -   5        $��
fstaltinfo��5X       $
��Wrap=�62   5Z      5[      5\      5]      �� 5^      $��10q�6   5`      5a      �� 5b   
   .+�5c      �� -   5        $��gbEigentuemer��5c   +   $��Eigentümer��6   $��	10,10,0,0q�6@   $	��520��6S   $
��Left=�5d      {�.  .��5e      �� 
�������.+�5f      �� $ 
Horizontal=�6!   .+�5h      �� $��Aktenzeichen��6$   $ 5q�68   $	��100��6D   (  "Xceed.Wpf.Toolkit.WatermarkTextBox  5k      -   5          Name$ tbGrund��5k   F   
�������   	Watermark$  hier Aktenzeichen eingeben��6W   $	��200��6   $Q KGeben Sie hier eine Begründung für den Zugriff an. Z.B. das Aktenzeichen.��5l   <   �� 5m      $ 
Horizontal=�6%   $y�Visible=�6>   .+�5n      �� $ ��Schnellabfrage in Kurzform��6(   $ 10q�6J   �� 5o      -   5        $��cbSchnellEigentuemer��5o   '    d� 	IsChecked$
 TrueY�6C   $��10q�6V   5p      5r      �� 5s      -   5        $��btnEigentuemer��5s      $��Start��6P   $	��200��6b   $
��Left=�6n   $��100,10,10,10q�6�   �� 5t      $ 
Horizontal=�6!   .+�5u      �� -   5        $��tbWeitergabeVerbot��5u   1   $��5q�6%    �� TextWrapping$
 Wrap=�6K   $	��300��6_   $
��Left=�6k   $��100,10,10,10q�6�   �� 5v      -   5        $��btnSchnellNachPDF��5v   !   $	��PDF��6Z   $y�	Collapsed=�6h   5x      5y      �� 5z      -   5        $��spEigentNotizUebernehmen��5z   #   {�.  .+�5{      �� $>��8Eigentümer-Notiz in den Paradigma-Vorgang übernehmen: ��6$   �� 5}      -   5        $!��btnEigentuemerNachParadigma��5}   !   $��starte Übernahme��5~   !   $
��Left=�6<   $��
100,10,0,0q�6W   $	��200��6k   5�      5�      5�      �� 5�      -   5        $��gbFSTaradigma��5�   2   $��Paradigma-Vorgang��6   {�.  $��	10,10,0,0q�6Z   $	��520��6m   $
��Left=�6y   .��5�      �� .+�5�      �� 
�������.+�5�      �� -   5        $��btnADRnachParadigma��5�   $   $D��>Flurstueck als Raumbezug in den Paradigma-Vorgang übernehmen ��6?   $��10,10q�5�   =   �� 5�      $ 
Horizontal=�6%   $��5q�6>   .+�5�      �� $��	Funktion:��6(   $��50��69   $i cFunktion oder Charakteristik dieser Adresse. In der Karte entspreicht dies dem Etikett des Punktes.��6E   �� 5�      -    5        $��tbKurz��5�   &   $	��274��64   6B   �� 5�      -!   5         ��Name$ cmbFunktionsvorschlaege��5�   +   $	 800��6N   $��25��6f   $ @Name��5�   A   $		 @ID��5�   A   
 5�      �� # [ $ /Variablen/Variable����5�   A   ��  5�   A   �� $ ID�5�   ?   5�      �� 5�      $ 
Horizontal=�6%   $��5q�6>   .+�5�      �� $��
Freitext: ��6(   $��50��6:   �� 5�      -"   5        $��
tbFreitext��5�   &   $	��300��68   6F   5�      �� 5�      -#   5        $ ��btnFlurstueckNachParadigma��5�   !   $��starte Übernahme��6l   $��5q�6�   $	��200��5�   !   5�      5�      5�      5�      5�      5�      