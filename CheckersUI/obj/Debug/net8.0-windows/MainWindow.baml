   M S B A M L   `   `   `  ����E  ACheckersUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:CheckersUI
CheckersUI  5   	       CheckersUI.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:CheckersUI     5   	     � Title$
  Game��5   	    � SizeToContent$ Width=�6   $
��1000��6,   ���	#FF32363C � WindowStartupLocation$ CenterScreen=�5	   	   .��5
      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   (� $��0��6   .��5      � ��5      �� 5       �� ImageSource$ Assets/Board.png��6!   .+�5      A� -   5         A�Name$ RedLightGrid��5       A� Rows$ 8��64    A� Columns$ 8��6=   $	��500��5      $	��500��6+   A� 5      -   5        $ 	BoardGrid��5      $ 8��61   $ 8��6:   $	��500��5      $	��500��6+    �� BitmapScalingMode$ HighQuality=�5      A� 5      -   5        $ HighLightGrid��5      $ 8��65   $ 8��6>   $	��500��5      $	��500��6+   5      5      � 5   
   $��1��6   $	��500��6   .+�5       �� -   5        $��
Назад��5       ���	#FF8DAAB3$��Dubai)�6<   $
��Left=�5!      $��	10,20,0,0q�60   $
��auto��5"      $��50��6#   $	��Top=�6.   6F   �� 5$      -   5        	 ��Name$	 WhitePlayerTimerTextBlock��5$      $��00:00��6;   $	��Top=�6H   $��Center=�6`   
�������$��20(�5%   +   $
��Bold"�69   $��0,20,0,0q�6K   �� 5'      -   5        $	 BlackPlayerTimerTextBlock��5'      $��00:00��6;   $��Bottom=�6H   $��Center=�6c   
�������$��20(�5(   +   $
��Bold"�69   $��0,0,0,20q�6K   �� 5*      -   5        $	 CurrentPlayerTextBlock��5*      $��Ходит: ��68   $��Center=�6G   $��Center=�6b   
�������$��20(�5+   +   $
��Bold"�69   $��0q�6K   5,      5-      5.      