   M S B A M L   `   `   `  ����  BoggleClient    BoggleClient.SummaryPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  SummaryPage��5      .��5
      � 
�������$��-212,-23,-464,-54q�6    � RowDefinitions 5   
   �� 5      $	��89* �6   �� 5      $	��29* �6   .+�5   
   �� -   5         ��Name$ SummaryLabel��5      $��Game Summary��6&   $
��Left=�6=   $��
125,17,0,0q�6X   $	��Top=�6l   �� 5   
   -   5         ��Name$ AgainButton��5      $��
Play Again��6&   $
��Left=�6;   $��
451,30,0,0q�6V   $	��Top=�6j   $��75��6�   $��1��6�   �� 5   
   $
��Left=�6   $��
120,79,0,0q�6/   $
��Wrap=�6C   $	��Top=�6W   �� 5   
   $
��Left=�6   $��
157,63,0,0q�6/   $
��Wrap=�6C   $	��Top=�6W   �� 5   
   -   5         ��Name$ YourLegalCountTextBlock��5      $
��Left=�65   $��157,250,0,0q�6P   $
��Wrap=�6e   $��0��6y   $	��Top=�6�   �� 5   
   -   5        $ TheirLegalCountTextBlock��5      $
��Left=�66   $��291,250,0,0q�6Q   $
��Wrap=�6f   $��0��6z   $	��Top=�6�   �� 5   
   -   5        $ CommonCountTextBlock��5      $
��Left=�62   $��432,250,0,0q�6M   $
��Wrap=�6b   $��0��6v   $	��Top=�6   �� 5   
   -   5        $ YourIllegalCountTextBlock��5      $
��Left=�67   $��582,250,0,0q�6R   $
��Wrap=�6g   $��0��6{   $	��Top=�6�   �� 5   
   -   5        $  TheirIllegalCountTextBlock��5      $
��Left=�68   $��721,250,0,0q�6S   $
��Wrap=�6h   $��0��6|   $	��Top=�6�   �� 5   
   -   5        $ YourLegalWordsLabel��5      $��Your legal words:��6-   $
��Left=�6I   $��
125,71,0,0q�6d   $	��Top=�6x   �� 5   
   -	   5        $ TheirLegalWordsTextBlock1��5      $��Opponent's legal words:��63   $
��Left=�6U   $��
242,71,0,0q�6p   $	��Top=�6�   �� 5   
   -
   5        $ CommonWordsLabel��5      $��Shared words:��6*   $
��Left=�6B   $��
402,71,0,0q�6]   $	��Top=�6q   �� 5   
   -   5        $ YourIllegalWordsLabel��5      $��Your illegal words:��6/   $
��Left=�6M   $��
545,71,0,0q�6h   $	��Top=�6|   �� 5   
   -   5        $ TheirIllegalWordsLabel��5      $��Their illegal words:��60   $
��Left=�6O   $��
677,71,0,0q�6j   $	��Top=�6~   �� 5   
   -   5        $ YourScoreLabel��5      $��Your score:��6(   $
��Left=�6>   $��
457,25,0,0q�6Y   $	��Top=�6m   �� 5   
   -   5        $ YourScoreTextBlock��5      $
��Left=�60   $��
545,30,0,0q�6K   $
��Wrap=�6_   $��0��6s   $	��Top=�6|   �� 5   
   -   5        $ TheirScoreTextBlock��5      $
��Left=�61   $��
766,27,0,0q�6L   $
��Wrap=�6`   $��0��6t   $	��Top=�6}   �� 5    
   -   5        $ TheirScoreLabel��5       $��Opponent's score:��6)   $
��Left=�6E   $��
637,22,0,0q�6`   $	��Top=�6t    J� RenderTransformOrigin$ -0.142,0.577&�6�   �� 5!   
   $
��Left=�6   $	��130��62   $��552,102,0,0q�6?   $	��Top=�6T   $	��100��6l   .��5"      �� -   5        $ YourIllegalWordsTextBlock��5"      $
��Left=�6;   $
��Wrap=�6V   $	��Top=�6j   $��80��6�   �� 6�   $�Your illega��6�   �� 6�   $�l word��6�   �� 6�   $�s��6�   6�   5#      �� 5$   
   $
��Left=�6   $	��127��62   $��407,105,0,0q�6?   $	��Top=�6T   $	��100��6l   .��5%      �� -   5        $ CommonWordsTextBlock��5%      $
��Left=�66   $
��Wrap=�6Q   $��Common words��6e   $	��Top=�6y   $��80��6�   5&      �� 5'   
   $
��Left=�6   $	��130��62   $��263,102,0,0q�6?   $	��Top=�6T   $	��100��6l   .��5(      �� -   5        $ TheirLegalWordsTextBlock��5(      $
��Left=�6:   $
��Wrap=�6U   $��Their legal words��6i   $	��Top=�6�   $��81��6�   5)      �� 5*   
   $
��Left=�6   $	��141��62   $��140,104,0,0q�6?   $	��Top=�6T   $	��100��6l   .��5+      �� -   5        $ YourLegalWordsTextBlock��5+      $
��Left=�69   $
��Wrap=�6T   $	��Top=�6h   $��81��6�   
��������� 6�   $�Your legal words��6�   6�   5,      �� 5-   
   $
��Left=�6   $	��130��62   $��686,102,0,0q�6?   $	��Top=�6T   $	��100��6l   .��5.      �� -   5        $  TheirIllegalWordsTextBlock��5.      $
��Left=�6<   $
��Wrap=�6W   $	��Top=�6k   $��81��6�   �� 6�   $�Their illeg��6�   �� 6�   $�al words��6�   6�   5/      50      51      