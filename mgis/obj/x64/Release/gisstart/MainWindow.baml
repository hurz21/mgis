   M S B A M L   `   `   `  ����   mgis    mgis.MainWindow   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Icon$'  !/mgis;component/icons/favicon.ico�� � Title$ Desktop-GIS��$	��950��$
��1895�� � WindowStartupLocation$ Manual=� � 	Resources    dgRowStyleHistory �� 
TargetType)  #System.Windows.Controls.DataGridRow �� Property �� Value �� Triggers P� Property	 P� Value  imgKreisUebersicht
 �� 	UriSource   XMLSourceComboBoxHintergrund  ListItemTemplate  flaechenBackground �� Orientation � ToolTip d� 	IsChecked �� Mode �� UpdateSourceTrigger
 � Tag � LayoutTransform �� ScaleX �� ScaleY 	� 	GroupName ��Name  listenButton ��Name ��Name ^� ShowDuration
 J� Uid _� 	StaysOpen��%C  &	        &	 x     &	 �     &	 �     7  7  7  7  �� "  .���  �@J�SnapsToDevicePixels .  P�  �@J�IsMouseOver	 . . ���  � �� �#CCCC00�� $"
 ../icons/kreisuebersicht.png?�� �� .R��� 8��  .+��� $ 
Horizontal=�8�� $��18��.+��� -   $ Ebene ausschalten��$��1q� �� *
mithaken+$ TwoWay=�$ PropertyChanged=� �� *aid+ �� $	 1.2Q�$	 1.2Q�	� -   $ 
radioLayer��$��1q�$) #Datenbank dieser Ebene aktiv machen�� �� *RBischecked+$ TwoWay=�$ PropertyChanged=� �� *aid+y��� *RBsichtbarkeit+�� $ btnrefreshminimap��-   $��15��8�� $ Ebene entfernen�� �� *aid+.���� $ myimagerefresh��$$��/mgis;component/icons/redx.png���� $ txtitel��-   $ 30000��$��5,0,0,0q�8�� ���� *titel+ �� *aid+ �� *sid+���� *myFontStyle+���� *myFontWeight+ _� $	��400��$	��600�� . .���� $	��400��$	��600��.+��� $
��Wrap=�$	��600������ *
dokutext+�� $	��210��$	��140������ *thumbnailFullPath+.��]� #��[ .+�]� -    ]�Name$ tigis�� � Cursor$ Arrow�� K� ZIndex$ 0��.+��� -    ��Name$ dockMenu��$ 0��$��2��$��0,0,0,12q� �� CanVerticallyScroll . $
��Left=�$	��420��$ Arrow��.+��� -   $ 
stPDFDruck��$y�Visible=�$ 0��$	��420��$	��500��$
��Left=�#��[ $��Center=�$ 
Horizontal=�$��0,0,-0,0q�.+��� $��2q�
���   �._��� .+��� $ 
Horizontal=�.+��� $�� Druckausgabe PDF��$��16(��� -	   $��
schliessen��$��5q�$��12(�$��Right=�
�������
���  ��$��	220,0,0,0q��� $ 
Horizontal=�$��10q�.+��� $��Format��$	��120��  �� Padding$  0q�	� -
   ! 	�Name$! 
rbFormatA4��$ radioFormat��$��DinA4��$
 TrueY�$��0,0,10,0q�	� -   $! 
rbFormatA3��$ radioFormat��$��DinA3��{�. �� $ 
Horizontal=�$��10q�.+��� $��Maßstab verwenden��$	��120��$  0q�	� -   $! rbOhneMasstab��$ 
grpMasstab��$��ohne Maßstab��$
 TrueY�	� -   $! rbMitMasstab��$ 
grpMasstab��$��mit Maßstab��$��	10,0,10,0q��� -   $��spAusrichtung��$ 
Horizontal=�$��10q�.+��� $��Ausrichtung��$	��120��$  0q�	� -   $
! quer��$ Radioausrichtung��$
��Quer��$
 TrueY�$��0,0,10,0q�	� -   $
! hoch��$ Radioausrichtung��$
��Hoch���� -   $��spdruckmasstab��$ 
Horizontal=�$��5q�.+��� $��Druckmaßstab   ��$	��120��$  5q��� $��1q�$��2q�
���   �._��� $ 
Horizontal=�.+��� -   $ tbMasstabDruck��$��
1 : 10.000��$  2q�$ Druckmaßstab���� -   " ��Name$" cmbMasstabDruck��$ Druckmaßstab��# �� MaxDropDownHeight$	# 600��$��20��$��24��$��Bottom=�$ �� DisplayMemberPath$$ 
displayVal��% �� SelectedValuePath$% tagVal������ �� $ 
Horizontal=�$��10q�.+��� $��	Bemerkung��$	��120��$  0q��� -   & ��Name$& tbPDF_Bemerkung��$��bmerkung��$	��250��$��0q��� $ 
Horizontal=�$��10q�.+��� $	��Ort��$	��120��$  0q��� -   $& 	tbPDF_Ort��$	��ort��$	��250��$��0q��� -   ' ��Name$' gbPDFparadigma��$��Paradigma-Vorgang��$��1q�
�������.���� .+��� -   ( ��Name$( cbPDFnachParadigma��$5��/PDF-Datei in den Paradigma-Vorgang übernehmen ��$ FalseY�$��20,10q��� $ 
Horizontal=�.+��� $��Bezeichnung im Vorgang: ��$��20,10q��� -   $& tbPDFnachParadigmaTitel��$	��200��$��25��$��Druckfertige Karte���� $��PDF erzeugen��.���� .+��� -   $��btnPrintPdf��$��Pdf-Datei erstellen��$	��200���� -   $��cbhochaufloesend��$ FalseY�$+��%Klassisch (Luftbilder hochauflösend)��$��5q�$��Center=��� .+��� -   $��tbPdfMasstabserror��$	��150��$	��400��$
��Wrap=�
���  ��$��14(�$
��Bold"��� -   $ stMenu��$ 0��#��[ .+��� -   $��spMenueHead��$	��105��.+��� $ 
Horizontal=�$ Arrow��.+��� -   $ btnAddLayer��$��48��$��40��$" Explorer: Ebenen hinzufügen��#��[ .���� $ 
Horizontal=�.+��� $��+��$��6��$��	-0,-5,0,0q��� $#��/mgis;component/icons/gis.png��$��30���� 
���   �$��48��.���� -   $( cballeeinaus��$
 TrueY�$� �Die Haken werden rausgenommen, die Ebenen werden nicht mehr dargestellt, sind aber noch in der Liste der Vordergrundebenen vorhanden.��$��5,5,5,0q�.���� .+��� $��Ebenen���� $��ein/aus���� -    $ btnRemoveAllLayers��$��24��$��24��$ Alle Ebenen entfernen��.���� $ 
Horizontal=�$��15��$��15��$h bALLE Ebenen werden aus der Liste der Vordergrundebenen entfernt. Die Liste ist anschließend leer.��.+��� $$��/mgis;component/icons/redx.png���� 
���   �$��9(�$��48��.���� $ 
Horizontal=�$��48��.+��� -!   $" cmbfavorite��) �� Text$) favorite��$��28��$��30��$��18(�$@ :Ebenenzusammenstellungen der verschiedenen Ämter aufrufen��.@��� $ nichts��$�� ���� $
 uwbb��$
��UWBB��$+ %Untere Wasser und Bodenschutzbehörde���� $	 unb��$	��UNB��$@ :Untere Naturschutzbehörde, mit Lubi und weisser Flurkarte���� $ unb_2��$��UNB-2��$d ^Untere Naturschutzbehörde, gleiche Themen aber schneller weil ohne weisse Flurkarte und Lubi ���� $
 immi��$��	Immission��$ Immissionsschutz���� $
 ille��$��Illegale Bauten��$  ���� $ feger��$��Feger��$ Schornsteinfegerwesen���� $ ba��$��Bauaufsicht���� $
 ba_2��$��Bauaufsicht-Minimal��$B <gleiche Themen aber nur wenige angeklickt. Übersichtlicher!���� $
 gebw��$��Gebäudewirtschaft���� $
 kats��$��Sicherheit (KatS)��$+ %Katastrophenschutz / Gesundheitswesen���� $ soziales��$��Soziales��$ Soziales���� $ intranet��$��Intranet��$ unspezifisch im Intranet���� -"   $��btnGruppeFavo��$��48��$��30��  
lessButton#��[ $) #Die Ansicht meiner Gruppe aufrufen.��.���� $ 
Horizontal=�.+��� $'��!/mgis;component/icons/gruppe1.png��$��30���� -#   $��btnMeineFavo��$��48��$��30��$' !Die persönliche Ansicht aufrufen��#��[ .���� $ 
Horizontal=�.+��� $,��&/mgis;component/icons/einzelperson.ico��$��22���� -$   $��btnFavoSave��$��48��$��30��$G ADie aktuelle Zusammenstellung als persönliche Ansicht speichern.��#��[ .���� $ 
Horizontal=�.+��� $.��(/mgis;component/icons/meineSpeichern.png��$��25��$��25���� $��	10,15,0,0q�$��40��.+��� -%   $��btnbookmark��$	�� L ��$��48��$��30��#��[ $ 600000�� �� .+��� $
��Wrap=�$	��210��$��40��$��14(�$��10q�Lesezeichen�� $
��Wrap=�$	��210��$	��100��$��12(�$��10q���Mit Lesezeichen können Sie die aktuelle Situation komplett abspeichern. Sie müssen Ihr dabei einen Titel geben, damit Sie Sie leicht wiedererkennen können.�� $
��Wrap=�$	��210��$	��100��$��12(�$��10q���Das ist nützlich, wenn Sie z.B. eine Besprechung vorbereiten wollen und dann lediglich die Lesezeichen nutzen um völlig verschiedene Orte und Ebenenzusammenstellungen darzustellen.�� -&   $��btnEbenenaktualisieren��$�� ��$��30��$ Änderungen übernehmen��$��58��$y�	Collapsed=��� $ 
Horizontal=�.+��� -'   $��btnStartBplan��$p jDas                              lankataster wird gestartet. Dort können sie sich einen B-Plan aussuchen!��#��[ $��5q�$��Bebauungsplankataster��$��10(�$	��120���� -(   $��btnExplorer��$�� . . .��$��60��#��[ $y�	Collapsed=��� -)   $��btnWMSgetfeatureinfo��$	��WMS��$��60��$ WMS-Datenabfrage��$y�Visible=�
����  �$��25��$��5q��� -*   $��
btnDossier��$��Dossier��$
��Bold"�$N HSie können auch einfach mit der RECHTEN Maustaste in die Karte klicken.��$��60��  	newButton#��[ �� $��5,5,0,0q�.+��� -+   $��cbSOeinschalten��$
 TrueY�$��25��$��70��.���� $��10(�$
��Wrap=�Suchobjekt sichtbar�� -,   $��btnMgisHistoryBack��$��<��$��5,0,0,0q�#��[ �� --   $��spInfoLegende��$	��300��$��25��.+��� -.   $ tbInfolegende��$��Info zur Liste unten��$��Center=�$  2q��� -/   $ 
tbFavoname��$y�	Collapsed=��� -0   $��svMainScrollviewer����. #��[ $	��400��.���� -1   * ��Name$* MainListBox��#��[ $	��400��+ J� RenderTransformOrigin$+ 0.5,0.5&�$ Arrow�����	#FFCCCCCC$��1,1,1,1q�$��Arial)�$��12(�$��0q�#��[ $	��200���� -2   $��tbInfopanel��, �� TextWrapping$
, Wrap=�$	��100��]� -3   $ dockTop��$	��Top=�$��50��$	��Top=�$ 0��#��[ .+��� -4   $��spButtonMenu��$ 
Horizontal=�$ Arrow��.+�&   Xceed.Wpf.Toolkit.DropDownButton  -5   $��spVIDParadigma��$��78��$��1q�#��[ $��12(�$��40��.���� $��0q�$��40��.+��� -6   $ stackVorgangsid��$ Vertical=�$��40��.+��� $��VorgangsID:��$  0q�$��10(��� -7   $ tbVorgangsid��$��58��$  0q�$��arial)�$
��bold"�$��16(�990909-   DropDownContent- �� $	��639��$	��849��$��5q�#��[ .+��� $ 
Horizontal=�$��5q�$	��800��.+��� $��Neuen Vorgang anlegen: ��$  3q�$	��250���� -8   $��btnNeuenVorgangAnlegen��$��anlegen��$��5q��� -9   $��btnvorgangabbruch��$��Abbruch��$��Right=��� $ 
Horizontal=�$��5q�.+��� $��Zu Vorgang wechseln: ��$  3q�$	��150���� -:   $ ��tbzuVorgangManuellWechseln��$	��100��
��������� -;   $!��btnzuVorgangManuellWechseln��$��	 wechseln���� 
���   �$��1q�$��5q�._��� $ 
Horizontal=�$��5q�.+�	� -<   $!��rbVorgangInParadigmaOeffnen��$" rbgVorgangInParadigmaOeffnen��$ FalseY�$��in Paradigma öffnen ��	� -=   $��rbVorgangRangeholen��$" rbgVorgangInParadigmaOeffnen��$
 trueY�$&�� Aussschnitt des Vorgangs öffnen��&   System.Windows.Controls.DataGrid  ->   .  Name$. 	dgHistory��$y�Visible=�$��5q�$	��380��/   AutoGenerateColumns/ .    dgHeaderStyle0   ColumnHeaderStyle#0 [ ���� 1   Columns1 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn2   Header$2 Nr��3   Width5  /System.Windows.Controls.DataGridLengthConverter$3 SizeToCells 1  +System.Windows.Controls.DataGridBoundColumn4   Binding4 �� *ID+  $2 Datum��$3 SizeToCells 4 �� *Datum+  $2 Az��5   MaxWidth$	5 350Q�4 �� *AZ+6   ElementStyle6 �� " ��.���  � ��$
 Wrap=�  $2 Titel��$	5 400Q�4 �� *Titel+6 �� " ��.���  � ��$
 Wrap=��� -?   $��btnParadigmaLight��$��	Vorgänge��$y�	Collapsed=��� $��1q�$��2q�
���   �$��40��._��� $ 
Horizontal=�$��40��.+�	� -@   $! rbHgrundAktiveEbene��$��0,15,0,0q�$��40���� -A   $ tbhgrund��$��60��$��40��$��12(�$  3q�$
��Wrap=�7 �� TextAlignment$7 Center=�$ 30000�� _� $	��400��$	��600�� . .���� $	��400��$	��600��.+��� $
��Wrap=�$	��600������ *
dokutext+�� $	��210��$	��140������ *thumbnailFullPath+�� -B   $" 	cmbHgrund��$��25��$��40��$ Hintergrund-Auswahl��$��16(�$	# 823������ ���� .R��� $ 600000������ *titel+ _� $	��400��$	��600�� . .���� $	��400��$	��600��.+��� $
��Wrap=�$	��600������ *
dokutext+�� $	��210��$	��140������ *thumbnailFullPath+�� -C   $��gbNavi��$��0q�$	��197��$��50��.���� $ 
Horizontal=�.+��� -D   $ btnFlstsuchen��$�� Flst �� 	 topmenuButton#��[	  �� .+��� Suchen nach Flurstücken�� auch: Eigentümerauskunft�� -E   $ btnAdressesuchen��$�� Adr ��#��[	  �� .+��� Suchen nach Adresse�� auch: Eigentümerauskunft�� -F   $��btnKoordinatesuchen��$�� Koo ��#��[	 $  Suchen nach UTM-Koordinate��  -G   $��btnOSdropdown��$��48��$��1q�$5 /Suche nach Objekten in den Themen (Objektsuche)��$��16(�$	��Obj��- �� $	��500��#��[ .+��� $��Objektsuche-Vorauswahl���� $7��1Wählen Sie das Thema in dem gesucht werden soll:���� $ 
Horizontal=�.+��� $��Center=�.+��� -H   $��
tbOSfilter��$��natur��$	��100��  -I   $. 	dgOSliste��$	��330��$	��400��8   CanUserAddRows8 .  / .  9   SelectionMode$9 Single=�:   SelectionUnit$: FullRow=�#0 [ $��Normal"����� 1   $2 Klicken zum Sortieren��$	5 250Q�4 �� *titel+�� -J   $ 
btnzoomin2��#��[	 .���� $&�� /mgis;component/icons/zoomin.png��$��32��$��32�� �� .+��� HineinZoomen�� 42Vergrößert den Ausschnitt in der Mitte der Karte�� -K   $ btnzoomout2��#��[	 .���� $'��!/mgis;component/icons/zoomout.png��$��32��$��32�� �� .+��� HerausZoomen�� 31Verkleinert den Ausschnitt in der Mitte der Karted� -L   ; d�Name$; chkBoxAusschnitt��$ FalseY�$��1,1,1,1q�$��48��$��40�� �� .+��� 
Ausschnitt�� 42Vergrößert einen beliebigen Ausschnitt der Karte���� .���  � �� �� < �� 
StartPoint$< 0.5,0&�= �� EndPoint$= 0.5,1&�.��� > � Color$> 	LightGray��? � Offset$? 0Q�� $> black��$	? 0.5Q�� $> black��$	? 0.5Q�� $> 	LightGray��$? 1Q�.���� $,��&/mgis;component/icons/auschnittNeu.png��$
��Fill=�$��38��$��38���� -M   $��brdPan��$��1q�$��2q�
���   �$��40�� �� .+��� Verschieben�� -+Seitlich die Karte verschieben. Rastet ein!._��� -N   $( 	chkBoxPan��$ FalseY�$��1,1,1,1q�$��5(�.���� .+��� $%��/mgis;component/icons/kreuz.png��$��19��$��19��$
��Fill=��� $��Verschieben��$  0,5,0,0q�$��8(��� $��1q�$��2q�
���   �$��40�� �� .+��� Bildschirmmaßstab�� 20Wählen Sie hier einen anderen Bildschirmmasstab�� (Mausrad-sensibel)._��� $ 
Horizontal=�$��40��.+��� -O   $ 	tbMasstab��$��
1 : 10.000��$��0,0,0,0q�$
��Wrap=�$��46��$7 Center=�$��40���� -P   $" 
cmbMasstab��$��20��$��40��$��Bottom=�$$ 
displayVal��$% tagVal������ �� -Q   $ 
btnrefresh��$��29��$ Karte auffrischen��#��[	 .���� $'��!/mgis;component/icons/refresh.png���� -R   $ rbfit2��$��29��$1 +Auschnitt wird an die Raumbezüge angepasst��$��R��#��[	 �� $��1q�$��1q�
���   �$��40�� �� .+��� Messen�� .,Strecken, Flächen, Koordinaten, Kreislinien�� (Mausrad-sensibel)._��� $ 
Horizontal=�$��40��.+��� $��Messen��$  5,8,2,0q�$��40���� -S   $" 	cmbMessen��$��20��$��2,9,0,0q�$) Messen��$��16(�.@��� $ 
Koordinate��$��UTM-Koordinate abfragen���� $ Fläche��$��Fläche messen���� $ Strecke��$��Strecke messen���� $ kreis��$��Kreislinie im Abstand���� $ windrose��$.��(Windrose (vorherrschende Windrichtungen)���� $
 leer��$
��leer��$y�	Collapsed=�  -T   $��	btnMessen��$��1��$��1q�$  ��$��10(�@   IsOpen@ .  $��38��- �� $ 
Horizontal=�$	��350��#��[ $��5q�.+��� .+��� -U   $��tbmessenhinweis��$�� ��$��10(�$��Normal"�$��80��$	��100��$
��Left=�$
��Wrap=��� $ 
Horizontal=�.+��� -V   $��btnNeueMessung��$��Neue Messung��$
��Left=��� -W   $��btnKoordUmrechner��$< 6Übergibt die Koordinaten an einen Externen Umrechner.��$��Koord.Umrechner���� -X   $��btnNachParadigma��$* $Übergibt den Raumbezug an Paradigma��$��nach Paradigma���� $	��250��.+��� -Y   $��btnMessenSchliessen��#��[ $	�� X ��$��30��
�������
���   �$
��Bold"�$��10(�$��Right=��� -Z   $ btnGetFlaecheEnde��$	��200��$ Messergebnis anzeigen��$
��Left=�.���� .+��� $��Messergebnis anzeigen��$��10(��� $ 
Horizontal=�$��5q�$	��490��.+��� .+��� $��Messergebnis:��$
��Bold"��� -[   $& tbzwischenwert��$	��225��$y�Visible=�$��letztes Segment��$��10(��� -\   $& tbMinimapCoordinate2��$	��225��$y�Visible=�$��10(�$)��#Summe bisher (ohne letztes Segment)���� -]   $ btnPdf��#��[	 $y�Visible=�$	��PDF��$&  Ausgabe der Karte ins PDF-Format���� -^   $ btnSchnelldruck��#��[	 $y�Visible=�$. (Ausgabe der Karte sofort auf den Drucker��.���� $,��&/mgis;component/icons/schnelldruck.png��$��28��$��28���� -_   $ 
globalfit2��#��[	 $��48��$ Kreisansicht��.���� $'��!/mgis;component/icons/kreisof.png���� -`   $ btnPNG��$ 30000��#��[	 A ^� ToolTipA �� .+��� 31Hier können sie die Grafik bearbeiten, also z.B.�� $"Texte oder Markierungen einfügen.�� B@Ausserdem wird das Verzeichnis mit der Grafikdatei geöffnet, so�� ,*dass sie die Datei schnell finden können..���� $%��/mgis;component/icons/stift.png��$��28��$��28��  -a   $��btnGmaps��$��70��$��1q�$� �Den  gewählten Ausschnitt in Googles Kartendiensten darstellen: Googles 3D-Ansicht, Vogelperspektive mit 4 Blickrichtungen und normales Luftbild��$��16(�@ .  $��38��- �� $	��320��#��[ $��5q�.+��� $ 
Horizontal=�.+��� $��Googles 3D-Ansicht��$	��250��$��12(�$  5q��� -b   $ btngoogle3d��$y�Visible=�$ 60000��$�� start��$��12(� �� .+��� Echte 3D-Ansicht�� 97Es wird ein Googlefenster ausserhalb des GIS geöffnet.�� caDabei wird der Ausschnitt übergeben. In einigen Fällen kann es zu geringen Abweichungen kommen.�� !Steuern Sie die 3D-Ansicht mit:�� CA- Mausrad drehen: Hineinverkleinern und Herausvergrößern (Zoom)�� _]- Maustaste links gedrückt halten und ziehen: Darstellung beweg sich in Richtung des Ziehens�� vt- Maustaste links gedrückt halten + Strg-Taste gedrückt halten und ziehen: Darstellung lässt sich beliebig kippen�� -c   $��btnGmapsSchliessen��$	�� X ��$��30��
�������
���   �$
��Bold"�$��10(�$��Right=�$��10,0,0,0q��� $ 
Horizontal=�.+��� $��Googles Vogelperspektive��$	��250��$��12(�$  5q��� -d   $ btnVogel��$y�Visible=�$�� start��$��12(��� $ 
Horizontal=�.+��� $0��*Googles normales Luftbild mit Routenplaner��$	��250��$��12(�$  5q��� -e   $ btnGooglemaps��$�� start��$��12(�$y�Visible=�$P JGooglemaps wird für diesen Ausschnitt in einem eigenen Fenster geöffnet.��.���� .+��� $�� g00gle��$
��Bold"�$��11(�$  0q�B �� 
LineHeight$B 17��C �� LineStackingStrategy$C BlockLineHeight=��� $	�� 3D��$
��Bold"�$��11(�$  0q�$B 17��$C BlockLineHeight=��� -f   $��btnHandbuch��#��[	 $ Handbuch��.���� $(��"/mgis;component/icons/handbuch.png��$��28��$��28���� -g   $ btnTools��#��[	 $ 	Werkzeuge��.���� $$��/mgis;component/icons/tool.png��$��32��$��32���� -h   $ btnOptionen��#��[	 $ Optionen / Einstellungen��.���� $(��"/mgis;component/icons/optionen.png��$��28��$��28��  -i   $��btnHGSlider��$��50��$��1q�$� Transparenz von Vordergrund und Hintergrund lassen sich hier getrennt einstellen. Gilt nicht fürs PDF. Windows-Snip empfohlen.��$��16(�@ .  $��38��- �� $	��300��#��[ .+��� $ 
Horizontal=�$��5q�$	��300��.+��� $��Hintergrund Helligkeit���� -j   $��btnSliderSchliessen��#��[ $	�� X ��$��30��
�������
���   �$
��Bold"�$��10(�$��Right=�$��40,0,0,0q��� $ 
Horizontal=�.+��� $��Center=�$	��300��.+��� $��Center=�$��10q�$ 
Horizontal=�.+��� $��0%���� -k   $��sldHgrundOpac��D � Maximum$	D 255Q�E �� TickPlacement$E BottomRight=�F �� TickFrequency$F 10Q�$	��200���� $
��100%���� $ 
Horizontal=�$��5q�$	��300��.+��� $%��Vordergrund Undurchsichtigkeit ���� $ 
Horizontal=�.+��� $��Center=�$	��300��.+��� $��Center=�$��10q�$ 
Horizontal=�.+��� $��0%���� -l   $��sldVGrundOpac��$D 1Q�$E BottomRight=�$F 10Q�G � Value$G 1Q�$	��200���� $
��100%��.���� $	�� T ��$
��Bold"�$��24(�$  0q�� -m   H �Name$H dockMap��$ 0��I � ColumnDefinitionsI �� $��0* ��� .+��� -n   $��	btnMinMax��$
�� <> ��$
��Left=�#��[ $	��Top=�$	 499��$��2��$��29���� -o   J ��Name$	J cv0��$ 0��
�������$��0,0,0,0q�$	��Top=�$
��Left=�$��2���� -p   $J 
HGabdecker��$ 0��
������ $	��Top=�$
��Left=�$��0,0,0,0q�$��2���� -q   $	J cv1��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -r   $	J cv2��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -s   $	J cv3��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -t   $	J cv4��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -u   $	J cv5��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -v   $	J cv6��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -w   $	J cv7��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -x   $	J cv8��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -y   $	J cv9��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -z   $
J cv10��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -{   $
J cv11��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -|   $
J cv12��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -}   $
J cv13��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -~   $
J cv14��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -   $
J cv15��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -�   $
J cv16��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -�   $
J cv17��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -�   $
J cv18��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -�   $
J cv19��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -�   $
J cv20��$ 0��$��0,0,0,0q�$	��Top=�$
��Left=�$��2��$��10��$��11���� -�   $J OSmapCanvas��$ 0��$��0,0,0,0q�$��2��$	��Top=�$
��Left=��� -�   $J imageMapCanvas��$ 0��$��0,0,0,0q�
������ $	��Top=�$
��Left=�$��2��$	��570��.+��� -�   $ imgKarte��$ 0��K �� Left$K 0��$
��1460��$	��980��$y�	Collapsed=��� -�   $J kreisUebersichtCanvas��$ 0��$y�	Collapsed=�$��	839,0,0,0q�$��1��.+��� -�   $ imgkreisuebersicht��$ 0��$K 0��$y�Visible=�#��[ �� -�   $J 
suchCanvas��$ 0��$y�	Collapsed=�$	��Top=�$
��Left=�$��0,0,0,0q�$��2���� -�   $ imgpin��$ 0��$y�	Collapsed=�$#��/mgis;component/icons/pin.png��$��1��$��30��$��30���� -�   $ 	stContext��$y�Visible=�$ 0��$��2��#��[ .+��� -�   $��
stContext1��$ 
Horizontal=�.+��� -�   $ stpKnoeppeVertical��$	��110��.+��� -�   $ btnDoku��$��0,50,0,0q�$	��100��$��Dokumentation���� -�   $ btnObjektsuche��$��0,0,0,0q�$	��100��$��Objektsuche���� -�   $ 
btnLegende��$��0,0,0,0q�$	��100��$��49��.���� .+��� $��Center=��� $�freischwebende���� $��Center=��� $�Legende���� -�   $��
stContext2��$��20q�.+��� -�   $��
stContext3��.+��� -�   $ stpDokuUndLegende��$
��Left=�$	��900��$��Center=�$ 
Horizontal=�#��[ .+��� -�   $ stpDoku��$	��550��$��Center=�$��0q�$	��Top=�.+��� $ 
Horizontal=�.+��� $��Dokumentation��$��Normal$�$  9q�$��0,0,10,0q�$��19(�$
��Left=��� -�   $ btnKopieInClipboard��$)��#Doku in die Zwischenablage kopieren��$��5q�$��0,0,10,0q�$��Right=��� $ 
Horizontal=�.+��� -�   $��zurück zur Karte��$��5q�$��12(�$��Right=�
�������
���  ���� -�   L ��Name$L richTextBoxDoku��M �� AcceptsReturnM . N �� 
AcceptsTabN . #��[ $	��Top=�$	��400��$
��Left=��� -�   $ stpPDFliste��#��[ $
��1200��.+��� -�   $��tbHinweisausPDFDateien��$��18(�$��Black"�
�������$��20q�  -�   $. 
dgPDFliste��$��20q�$	��300��8 .  / .  $9 Single=�$: FullRow=�#0 [ $��Normal"�$
��Left=����� * +1   $2 Klicken zum Sortieren��$	5 450Q�4 �� *titelspalte1+  $2 Klicken zum Sortieren��4 �� *titelspalte2+�� -�   $ stpObjektsuche��#��[ $	��Top=�$
��Left=�$
��1300��.+��� $ 
Horizontal=�$��Right=�.+��� -�   $ 
btncloseOS��$��zurück zur Karte��$��5q�$��12(�$��Right=�
�������
���  ���� -�   $��tbHinweisObjektsuche��$��18(�$��Black"�
�������$��10q�$��Objektsuche��$��25���� $ 
Horizontal=�.+��� -�   $��	btnOS2CSV��$��Tabelle nach Excel��$��5q�$��Normal$�$��12(�
��� � ��� -�   $" cmbOSKat��$��5q�$ Filter nach Kategorie��$	# 600��$	��200��$��24��$��Bottom=����� �� $��Textfilter: ��$��5q�$  5q��� -�   $��tbOSTextfilter��$	��250��$��25��$ bitte nur EIN Wort/Silbe��$��5q�$�� ���� -�   $��btnOSTextfilterStart��$��Suchen��$��25��$��5q��� -�   $��tbOS_Result��$��result��$��10q�$��Normal$�$��12(��� ��. $��Visible=�$
��Auto=�.��  -�   $. dgObjektsuche��$��5q�$	��600��"O   VerticalScrollBarVisibility$
O Auto=�$	��910��$P   HorizontalScrollBarVisibility$
P Auto=�8 .  / .  $
��Left=�$9 Single=�$: FullRow=�#0 [ $��Normal"����� * +1   $	2 gid��$
3 Auto 4 �� *[0]+  4 �� *[1]+  4 �� *[2]+  4 �� *[3]+  4 �� *[4]+  4 �� *[5]+  4 �� *[6]+  4 �� *[7]+  4 �� *[8]+  4 �� *[9]+  4 �� *[10]+  4 �� *[11]+  4 �� *[12]+  4 �� *[13]+  4 �� *[14]+  4 �� *[15]+  4 �� *[16]+  4 �� *[17]+  4 �� *[18]+  4 �� *[19]+  4 �� *[20]+�� -�   $ 
stckBalken��$ 0��$	��200��$��25��$��Right=�$��Bottom=�$��1��.+��� -�   $ 	btnBalken��$	��200��$��25��Q J� Opacity$	Q 0.5Q�$��400 m��
���   �
��� ����� -�   $J cvPDFrechteck��$y�Visible=�$ 0��$��0,0,0,0q�
������ $��1���� -�   $ stwinthemen��$ 0��$��0,0,0,0q�$	��550��$
��Left=�$	��Top=�$ 
Horizontal=�#��[ $��2��$	��600��.+��� $ 0��.+��� -�   $ stSachgebiete��$��0q�$ 0��.+��� $0��*Vordergrundebenen in Kategorien aufgeteilt��$��16(�$��	10,20,0,0q�$
��Bold"�
�������R� -�   $	��tv1��$
��Left=�$	��900��$��10,0,0,0q�$	��Top=�$	��400��$��14(�$��courier)�
������� R �� Color��%2  (��       * SystemColors.HighlightBrushKey+)(��      *$"SystemColors.HighlightTextBrushKey+)(�� (     *1/SystemColors.InactiveSelectionHighlightBrushKey+)(�� ?     *53SystemColors.InactiveSelectionHighlightTextBrushKey+)�� $R Transparent���� $R Black���� $R Transparent���� $R Black������ " Q�.���  �@Q�
IsExpanded . .@�Q� $��	Allgemein��$ 	allgemein��$
��Bold"�Q� $��Boden / Abfall��$ Boden��$
��Bold"�Q� $��Grenzen��$ Grenzen��$
��Bold"�Q� $��Klima Luft Lärm��$ Klima��$
��Bold"�Q� $��Historisches��$ 	Flurkarte��$
��Bold"�.@�Q� $��
Flurkarten��$ 	Flurkarte��$��Normal"�Q� $!��Landschaftsschutzgebiete AD��$ h_landschaftsschutz��$��Normal"�Q� $��
Luftbilder��$ 
h_luftbild��$��Normal"�Q� $��Regionalplan 2002-2011��$ h_regionalplan��$��Normal"�Q� $��Topographische Karten��$ 
h_topkarte��$��Normal"�Q� $��Verschiedenes��$ h_verschiedenes��$��Normal"�Q� $��Liegenschaften��$ kreisoffenbach��$
��Bold"�.@�Q� $��Kreis Offenbach��$ kreisoffenbach��$��Normal"�Q� $��	Gemeinden��$ 	gemeinden��$��Normal"�Q� $��	Kreishaus��$ 	kreishaus��$��Normal"�Q� $��Natur und Landschaft��$
��Bold"�$ arten��.@�Q� $��Arten und Biotope��$ arten��$��Normal"�Q� $��Förderflächen etc.��$ foerder��$��Normal"�Q� $��Natur��$ natur��$��Normal"�Q� $��Schutzgebiete��$ schutzgebiete��$��Normal"�Q� $��	Paradigma��$ 	Paradigma��$��Normal"�Q� $��Landschaftsplanung��$
��Bold"�$ lp_bestandserfassung��.@�Q� $��Bestandserfassung��$ lp_bestandserfassung��$��Normal"�Q� $��	Bewertung��$ lp_bewertung1��$��Normal"�Q� $��Zusammenfassung��$ lp_bewertung2��$��Normal"�Q� $��Leitbild��$ lp_leitbild��$��Normal"�Q� $��Entwicklung��$ lp_entwicklung��$��Normal"�Q� $��Planung und Bauen��$
��Bold"�$ planung��.@�Q� $��Planung��$ planung��$��Normal"�Q� $��Regionaler FNP��$ regfnp��$��Normal"�Q� $��Denkmalschutz��$ denkmalschutz��$��Normal"�Q� $��Verkehr��$
��Bold"�$ verkehr��.@�Q� $��Strasse und Schiene��$ verkehr��$��Normal"�Q� $��
Radverkehr��$ verkehr_rad��$��Normal"�Q� $��Sicherheit und Ordnung��$
��Bold"�$ 
sicherheit��.@�Q� $��Katastrophenschutz��$ 
sicherheit��$��Normal"�Q� $��Hochwasserschutz��$ hochwasserschutz��$��Normal"�Q� $��Waffenwesen��$ waffenwesen��$��Normal"�Q� $��Soziales��$ soziales��$
��Bold"�Q� $��Freizeit und Tourismus��$ 	Tourismus��$
��Bold"�Q� $��Wasser��$ Wasser��$
��Bold"�Q� $��
Wirtschaft��$ 
Wirtschaft��$
��Bold"��� .+��� $��Center=�$y�	Collapsed=�.+��� -�   $ 	nachLinks��$	�� <<��$��5q�$��Center=�$ alles nach links kopieren���� -�   $ 
nachRechts��$	�� >>��$��5q�$��Center=��� $y�Visible=�$��0q�.+��� -�   $ btnclosesuchform��$��	zur Karte��$��5q�$��12(�$��Right=�
�������
���  ���� $��  Volltextsuche��$��16(�$��	20,0,20,0q�$
��Bold"��� $C��=    hier max 4 Stichworte eingeben, durch Leerzeichen trennen��$��Italic$�$��	20,0,20,0q��� $��Normal$��� $ 
Horizontal=�.+��� $  5q�$��14(�$> 8hier max 4 Stichworte eingeben, duch Leerzeichen trennen���� $�Suchen:��(  "Xceed.Wpf.Toolkit.WatermarkTextBox  -�   S  Name$S tbStichwort��T   	Watermark$>T 8hier max 4 Stichworte eingeben, duch Leerzeichen trennen��$	��340���� -�   $ btnStichwort��U �� 	IsDefaultU . $��56��$ 	adelante!��.���� $��start���� -�   $ 	tbtreffer��$  5q�$��-���� -�   $ tbKategorie��$  5q�$��-��$��14(�$
��Bold"�  -�   $. 
dgErgebnis��$
��Left=�$	��550��$��14(�$	��600��$
��1000��8 .  / .  V   GridLinesVisibility$
V None=�$9 Single=� 
 dgRowStyleTrefferW   RowStyle#W [
 $: FullRow=�#0 [ $��Normal"����� * +1   $2 Gefunden��$	3 450 4 �� *titel+6 �� " ��.���  � ��$ NoWrap=���  �@�ToolTip �� *	tultipp+�� -�   $��tbebenenauswahlinfo��$y�Hidden=�$H��BKlicken Sie auf den Namen der Ebene um sie zur Karte hinzuzufügen��$��20q�]� -�   $ tivogel��$y�	Collapsed=�.+��� $ 
Horizontal=�$	��Top=�$��Right=�.+��� -�   $ btnzurueckZumGis��$��zurück zum GIS��$	��100��$��30��$	��Top=�$��Right=�$��20,5,5,5q�(	  "System.Windows.Controls.WebBrowser	  -�   X 	 Name$X webBrowserControlVogel��