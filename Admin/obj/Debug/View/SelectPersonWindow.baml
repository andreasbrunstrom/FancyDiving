   M S B A M L   `   `   `  ����  AdminApp&     AdminApp.View.SelectPersonWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  SelectPersonWindow��5   	   $	��300��6$   $	��300��61    � 
ResizeMode$ NoResize=�6=   .��5      � .+�5	   
   �� -   5         ��Name$ textBox��5	      $��23��6#   $��
10,10,10,0q�6/    �� TextWrapping$
 Wrap=�6C   $�� ��6W   $	��Top=�6_   �� 5
   
   -   5         ��Name$ listBox��5
      $��10,38,10,43q�6$    � 	Resources 5       �� 
TargetType �� Property �� Value��%Y   '��       �� 5      " ��6   .�5      ��  � y�$ 	Collapsed=�63   5      5      	 �� View	 5      �� 5      .P�5      �� 
 �� DisplayMemberBinding
 6%   �� *name+6M   �� 5      
 6%   �� *	birthdate+ �� StringFormat$
 yyyy��6g   �� 5      
 6%   �� *nationality+6T   5      5      �� 5   
   $��10q�6    �� Orientation$ 
Horizontal=�6!   $��Right=�6:   $��Bottom=�6V   .+�5      �� -   5         ��Name$ 
btn_cancel��5   #   $��60��6   $��Avbryt��67   $��23��6H   $��0q�6T   6_   �� 5      -   5        $ btn_addSelectedJudge��5   #   $��60��6   $��23��6A   $��
Lägg till��6M   $��10,0,0,0q�6a   6s   5      5      5      