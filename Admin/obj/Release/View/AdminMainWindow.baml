   M S B A M L   `   `   `  ����   SharedClassesD2clr-namespace:SharedClasses;assembly=SharedClassesSharedClasses   AdminApp   AdminApp.AdminMainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     LsharedClasses2clr-namespace:SharedClasses;assembly=SharedClasses     -     � Title$  FancyDiving��$	��600��$
��1020�� � 	Resources    TabItemContent �� DataType �� Property  PurpleBrush �� Value �� Triggers �� Binding �� Mode �� Value  
GreenBrush  GreyBorderBrush  	StyleGrid	 J� SnapsToDevicePixels
 � RowDefinitions � ColumnDefinitions �� ResizeBehavior �� ShowsPreview  OppositeConverter �� 	Converter �� Orientation ��Name-  'System.Windows.Input.KeyboardNavigation   TabIndex �� UpdateSourceTrigger(  "System.Windows.Controls.DatePicker  Name   SelectedDate �� SelectedValuePath �� SelectedValue �� Path 	�Name 	� 	GroupName d� 	IsChecked ��Name �� 
IsReadOnly  ButtonBorderBrush  HideGroupWhenDisabled �� SelectedItem �� View �� Width  �� DisplayMemberBinding! ��Name" �� StringFormat  IndexConverter 	 JudgeListview# �� 
TargetType$ �� BasedOn% �� CellTemplate 
 VisibilityConverter  invertedVisibilityConverter& �� Padding  NegativeToNullConverter' �� ElementName  	BlueBrush( �� AlternationCount) �� RelativeSource* �� AncestorType+ ��Name  HideButtonWhenDisabled��%n  &	        7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7 	 7��7  7 
 7  7  7 	 7��7��7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7��7  7  7  7  �� $ sharedClasses:Competition��.R��� $��0,2,0,0q����� .���  � ��8    ��  �� *	started+$ OneWay=� true.a���  � ��8  ._��� $��1,0,1,1q�8�� ._�� 8�� 	 . 
 �� �� $��43 � �� $	��300 �$	��500��$	��200���� $��10 ��� .+��� $ PreviousAndNext=�$��1��$��Center=�$��Stretch=�
������  .  $��10��|�.  � $��0��$��0��	 . 
 �� $��31��$	��55* ��� $��10 ��� $��21��$	��45* �.+��� $ PreviousAndNext=�$��1��$��Stretch=�$��Center=�
������  .  $��10��|�.  �� $��0��$��0��$��Tävling��$��Stretch=�$��	10,10,0,0q�{��� *	started+8  $ OneWay=�.���� $��Disabled=�$
��Auto=�.��� .+��� $ Vertical=�$��5,0,5,5q�.+��� $��Namn:���� $ tb_competitionName��$��24��$ 0������ *name+$ PropertyChanged=�$ TwoWay=��� $ 
Horizontal=�.+��� $ Vertical=�.+��� $��Datum:��  $ date_competition��$��23��$
��Left=�$ 1�� �� *date+$ TwoWay=��� $ Vertical=�$��20,0,0,0q�{��� *
finished+8  $ OneWay=�.+��� $��Åldersklass���� $	��100��$ Content��$��24��$ 2�� �� $ ageGroup�$ TwoWay=�.@��� .��Internationell�� .��	A-klass�� .��	B-klass�� .��	C-klass�� .��	D-klass�� .��	E-klass�� $ 
Horizontal=�.+��� $ Vertical=�{��� *
finished+8  $ OneWay=�.+��� $��Kön:���� $ 
Horizontal=�.+�	� $ rb_gender_men��$��0,0,10,0q�$
��Män��$ radioGender��$ 3�� �� *gender+$ TwoWay=�	� $ rb_gender_women��$��Kvinnor��$ radioGender��$ 4�� �� *gender+8  $ TwoWay=��� $ Vertical=�$��20,0,0,0q�{��� *
finished+8  $ OneWay=�.+��� $�� ���� $ 	cb_syncro��$��Syncro�� �� *syncro+�� $��Rundor:���� $ 
Horizontal=�{��� *
finished+8 	 $ OneWay=�.+��� $��btn_decreaseRounds��-   $��Center=�$��0q�.��-�� $��	tb_rounds�� . $��30��$��0,1,0,1q�8��
 $��Center=����� *rounds+$ TwoWay=��� $��btn_increaseRounds��-   $��Center=�$��0q�.��+�� 8�� $��0��$��2��$��Domare��$��10,0,0,0q�$��Stretch=�{��� *	started+8  $ OneWay=�.��� .+��� $��c_lv_judges��$��0,0,0,34q� �� *selectedJudge+$ TwoWay=����� *judges+ �� .P��� $
��Namn��$	 165��  �� *name+�� $��Nationalitet��$ 80��  �� *nationality+�� $ 
Horizontal=�$��Right=�$��Bottom=�.+��� $! btn_addJudge��-   $��5,0,0,0q�$��
Lägg till��{��� *
finished+8  $ OneWay=��� $! btn_removeJudge��-   $��5,0,0,0q�$��Ta bort��{��� *
finished+8  $ OneWay=��� 8�� $��2��$��0��$��Nuvarande hopp��$��10,0,0,0q�$��Stretch=�{��� *	started+$ OneWay=�.���� $��Disabled=�$
��Auto=�.��� .+��� $ Vertical=�$��10q�.+��� �� $��Namn:��$��70�� �� ���� $ currentContestant.name�$ OneWay=��� �� $��Land:��$��70�� �� ���� $# currentContestant.nationality�$ OneWay=��� �� $��Hopp:��$��70�� �� ��`� $" 
{0} av {1}��.1��� $ currentRound�8  �� $ rounds��� �� $��Hoppkod:��$��70�� �� ���� $ currentJump.jumpCode�$ OneWay=��� �� $��Höjd:��$��70�� �� ���� $ currentJump.height�$
" {0}m��$ OneWay=��� $��	0,10,0,30q�$��1q�8�� ._�� 
 �� ��  �� �� $��50 �.+��� 8�� $��0��$��0��$��0q�$��0q�
������ ���� *judges+ ���� '��       "# ��.���  � y�$ 	Collapsed=����� 8$  "# ��.���  � |� .  ��  � ��
 �   � �� .P��� $��plats��  �� *	judgeSeat+8  �� $
��namn��  �� *name+�� $ 30��% �� .R�� .+��� $��16��$��16��$��../VSO_Link_blue_16x.png��y��� *
occupied+8  �� $��16��$��16��$��../Unlink_16x.png��y��� *
occupied+8  �� $��1��$��0��$& 5,2,5,2q�$��0,1,0,0q�8�� ._��� $��6,0,6,0q�$��Total���� 8�� $��0��$��1��$��0q�$��50��$��0q�
������ ���� *currentJump.scores+ ���� '��       "# ��.���  � y�$ 	Collapsed=��� '��       "# ��8$  .���  � ��
 �   � ��  �� *	ignored+ True.a���  � ��
 �  ������ 8$  "# ��.���  � |� .  ��  � ��$ Stretch=� �� .P��� $ 40��$��plats��  �� *points+8  �� $��Center=�$��1��$��1��$& 5,2,5,2q�$��0,1,0,0q�8�� ._��� $��Center=�$
��Left=�$��6,0,6,0q����� *currentJump.jumpScore+]� $��Stretch=�$��Bottom=�$��10q�.+��� $ cb_nextJumpAuto��-   $ FalseY�$
��Left=�$��Center=�$��Nästa automatiskt���� $! btn_nextJump��-   $��Right=�{��� $' cb_nextJumpAuto��$ 	IsChecked�8  .��]� .+��� $��../Forward.ico���� $��Nästa hopp��� $��0��$��2��	 . 
 �� $��31���� $��10 ��� $��21��.+��� $ PreviousAndNext=�$��1��$��Stretch=�$��Center=�
������  .  $��10��|�.  �� $��0��$��0��$��	0,10,10,0q�{��� *	started+8  $ OneWay=����� "# ��8$  .���  � ��$ 	Deltagare�� ��  �� *
finished+ true.a���  � ��$ Slutresultat����  � ��8   ��  � ��8 ! .��� .+��� $��Stretch=�$��0,0,0,34q� �� *selectedContestant+$ TwoWay=����� *contestants+ �� .P��� $
��Namn��$	 250��  �� *name+�� $
��Kön��$ 50��  �� *genderString+�� $��Klubb��  �� *club+�� $��Nationalitet��$	 100��  �� *nationality+�� $��Totalpoäng��  �� *
totalScore+�� $ 
Horizontal=�$��Right=�$��Bottom=�.+��� $! btn_addContestant��-   $��5,0,0,0q�$��
Lägg till��{��� *
finished+8 " $ OneWay=��� $! btn_removeContestant��-	   $��5,0,0,0q�$��Ta bort��{��� *
finished+8 # $ OneWay=��� 8��$ $��2��$��0��$
��Hopp��$��0,0,10,0q�{��� *	started+8 % $ OneWay=�.��� .+��� -
   $��Stretch=�$��0,0,0,34q� �� *selectedJump+$ TwoWay=����� *selectedContestant.jumps+$ TwoWay=�( �� * selectedContestant.jumps.Count+ �� .P��� $��#��$ 30��  �� *!(ItemsControl.AlternationIndex)+) �� $* ListViewItemL�8 & �� $��Höjd��$	 100��  �� *height+�� $��Hoppkod��$	 100��  �� *
jumpCode+�� $��Svårighetsgrad��$	 100��  �� *
difficulty+�� $��Poäng��$	 100��  �� *	jumpScore+�� $ 
Horizontal=�$��Right=�$��Bottom=�.+��� $! btn_editJump��-   $��Ändra��{��� *
finished+8 ' $ OneWay=��� 8��( $��0��$��0��$��Nuvarande ställning��$��	0,10,10,0q�{��� *	started+$ OneWay=�.��� .+��� $��lv_highscore��$��0q����� *	highscore+ �� .P��� $
��Namn��$	 300��  �� *name+�� $��Totalpoäng��  �� *
totalScore+�� 8��) $��2��$��0��$��Kommande hopp��$��0,0,10,0q�{��� *	started+$ OneWay=�.��� .+��� $+ lv_commingJumps��-   $��0q� �� *currentJump+$ OneWay=����� *jumpsInCurrentCompetition+���� 8$ * "# ��.���  � |� .   �� .P��� $��Höjd��$	 100��  �� *height+�� $��Hoppkod��$	 100��  �� *
jumpCode+�� $��Svårighetsgrad��$	 100��  �� *
difficulty+�� $��Poäng��$ 50��  �� *	jumpScore+�� $��2��$��1��$��10q�{��� *
finished+8 + $ OneWay=�.+��� $! btn_StopCompetition��-   8��, $��Right=�$��Bottom=�$��Center=�{��� *	started+$ OneWay=�.��]� .+��� $��../Stop.ico���� $��Avsluta tävling���� $! btn_StartCompetition��-   8��- $��Right=�$��Bottom=�$��Center=�{��� *	started+8 . $ OneWay=�.��]� .+��� $��
../Run.ico���� $��Starta tävling��.���   BackgroundBrush#��[ .+��� -   , ��Name$, tabControlDynamic��$y�	Collapsed=�$��10q�$��0q�����    	TabHeader  CloseTabStyle��%�   &	       7  �� $ TabItem��.R�]� .+��� $! 	btnDelete��-   8��  $��Right=����� *Name+) �� "* ���� ���� *Header+) �� "* ������ - �� 
TargetType"- ��.R�� }�. .   TabNavigation$. Local=�	 .  �� / �Name$/ ColumnDefinition0��$��* ��� $/ ColumnDefinition2��$��0 �
 �� $/ RowDefinition0��$
��Auto ��� $/ RowDefinition1��$��* �.+�]� $��HeaderPanel��$��0��$��0��$��2,2,2,0q�0 K� ZIndex$0 1��1 K� IsItemsHost1 . $ 1���� $��ContentPanel��$��0��$��1��#��z��#��z��#��z��2   DirectionalNavigation$2 	Contained=�$ 2��$. Local=�._��� $��PART_SelectedContentHost��#��z��3 �� SelectedContent#��z3 $��SelectedContent��"4 �� SelectedContentStringFormat5 �� ContentStringFormat#5 z4 6 �� SelectedContentTemplate#��z6 #	 z	 �� -   $��
tabControl��$y�Visible=�$��10q�$��0q�.@��� -   7 ��Name$7 tab_home��$	��Hem��$
��Left=�.���� $��0,2,0,0q�#��[ ._��� $��1,0,1,1q�#��[ ._�� #��[ .+��� -   8 ��Name$8 lbl_welcome��$��
Välkommen��$��Center=�$��64��$��	0,100,0,0q�$	��Top=�{�.  $��28(�$
��Bold"��� $��
10,0,10,10q�$��Bottom=�.+��� -   $! btn_newCompetition_copy��$��Skapa ny tävling��$��5,0,0,0q�$��Right=��� -   $7 tab_competitions��$��
Tävlingar��$
��Left=�.���� $��0,2,0,0q�#��[ ._��� $��1,0,1,1q�#��[ ._�� #��[ .+��� -   $+ lv_competitions��$��0q�$��10,10,10,43q�����  9 P� Property: P� Value��%�   '��       7��7  �� "# ��8$    ��  �� *
finished+ True.a���  � ��8  P� 9 �@��
IsSelected: . . ���  � ��
 ����� �� .P��� $��Id��$ 50��  �� *id+�� $
��Namn��$	 250��  �� *name+�� $
��Kön��$ 80��  �� *genderString+�� $��Syncro��$ 60��  �� *syncro+�� $��Höjd��$ 50��  �� *height+�� $��Datum��$ 90��  �� *date+$" 
yyyy-MM-dd���� $��Tävlingsklass��$	 120��  �� *
ageGroup+�� $��
10,0,10,10q�$��Bottom=�$��Right=�$ 
Horizontal=�.+��� -   $! btn_deleteCompetition��$��Ta bort tävling��$��5,0,0,0q�$��Right=��� -   $! btn_editCompetition��$��Redigera tävling��$��5,0,0,0q�$��Right=��� -   $! btn_newCompetition��$��Skapa ny tävling��$��5,0,0,0q�$��Right=��� -   $7 tab_contestants��$��	Tävlande��$
��Left=�.���� $��0,2,0,0q�#��[ ._��� $��1,0,1,1q�#��[ ._�� #��[ .+��� -   $+ lv_contestants��$��0q�$��10,10,10,43q�����  �� .P��� $��Id��$ 50��  �� *id+�� $
��Namn��$	 220��  �� *name+�� $
��Kön��$ 50��  �� *genderString+�� $��Klubb��$	 100��  �� *club+�� $��Nationalitet��$	 100��  �� *nationality+�� $��Födelsedatum��$	 100��  �� *	birthdate+$" 
yyyy-MM-dd���� $��
10,0,10,10q�$��Bottom=�$��Right=�$ 
Horizontal=�.+��� -   $! btn_delete_contestant��$��Ta bort tävlande��$��5,0,0,0q�$��Right=��� -   $! btn_edit_contestant��$��Redigera tävlande��$��5,0,0,0q�$��Right=��� -   $! btn_new_contestant��$��Skapa ny tävlande��$��5,0,0,0q�$��Right=��� -   $7 
tab_judges��$��Domare��$
��Left=�.���� $��0,2,0,0q�#��[ ._��� $��1,0,1,1q�#��[ ._�� #��[ .+��� -    $+ 	lv_judges��$��0q�$��10,10,10,43q�����  �� .P��� $��Id��$ 50��  �� *id+�� $
��Namn��$	 220��  �� *name+�� $��Nationalitet��$	 100��  �� *nationality+�� $��
10,0,10,10q�$��Bottom=�$��Right=�$ 
Horizontal=�.+��� -!   $! btn_delete_judge��$��Ta bort domare��$��5,0,0,0q�$��Right=��� -"   $! btn_edit_judge��$��Redigera domare��$��5,0,0,0q�$��Right=��� -#   $! btn_new_judge��$��Skapa ny domare��$��5,0,0,0q�$��Right=�