   M S B A M L   `   `   `  ����I  EViaRailProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:ViaRailProjectViaRailProject  5   	       ViaRailProject.Customer   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:ViaRailProject     5   	     � Title$  Customer��5   	   $	��450��6   $	��900��6'   .��5	      � .+�5   
   �� -   5        $��Add to Cart��5      $
��Left=�6'   $	��Top=�6B   $	��100��6Z   $��226,250,0,0q�6f   6{   �� 5   
   -   5        $��tripListBox��5      $��Center=�6%   $	��853��6B   $��
0,50,0,200q�6N   �� 5   
   -   5        $��quantityTextBox��5      $
��Left=�6)   $	��Top=�6D   $��50��6\   $��145,252,0,0q�6g   $��0��6|   .��6�   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value
 �����63    �� Triggers 5      P� 5       P� Property �@J�	IsFocused P� Value . . �5      ��  � ��
 �   �6;   �� 5       � ��$  ��65   5      5      .��5      5      �� 5   
   $	��Top=�6   $
��Left=�6,   $��	25,16,0,0q�6G   $	��338��6Z   �� 6g   $�Welcome, Customer��6k   �� 6�   $�, ��6�   �� 6�   $#�please add trips to your cart��6�   �� 6�   $� from the list��6�   6�   �� 5   
   -   5        $��Checkout��5      $	��Top=�6$   $
��Left=�6<   $��
26,277,0,0q�6W   $	��100��6�   �� 5   
   $	��Top=�6   $
��Left=�6,   $��
26,302,0,0q�6G   �� 6\    � Language$ en-ca�6`   $�Tickets Selected:��6q   6�   �� 5   
   -   5        $��totalSalesTextBlock��5      $
��Left=�6/   $	��Top=�6J   $��458,253,0,0q�6b   $	��123��6w   �� 5   
   -   5        $��cartListBox��5      $��Center=�6%   $	��Top=�6B   $	��853��6Z   $��	0,323,0,0q�6f   $��73��6y   5      �� 5    
   $	��Top=�6   $
��Left=�6,   $��
26,252,0,0q�6G   �� 6\   $�Number of Tickets��6`   �� 6{   $ en-ca�6   $�:��6�   6�   �� 5!   
   $	��Top=�6   $
��Left=�6,   $��365,253,0,0q�6G   �� 6]   $ en-ca�6a   $�Total amount:��6r   6�   �� 5"   
   -   5        $��Back to Main��5"      $	��100��6G   $��776,0,0,384q�6S   $
��Left=�6h   5#      5$      