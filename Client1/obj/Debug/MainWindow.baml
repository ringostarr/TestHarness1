   M S B A M L   `   `   `  ����B  >Client1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:Client1Client1  5   	       Client1.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Client1     5   	   -   5          � Title$  CLient1��5   	   $	��500��6   $	��600��6&   .��5	      � $��0,0,-142,-157q�6   .+�5
   
   �� -   5         ��Name$ button��5
      $��SendRequest��6!   $
��Left=�67   $��122,343,0,0q�6R   $	��Top=�6g   $��75��6   6�   �� 5   
   -   5        $ button1��5      $��UploadFiles��6"   $
��Left=�68   $��416,343,0,0q�6S   $	��Top=�6h   $��75��6�   6�   �� 5   
   -   5         ��Name$ FTLabel��5      $��Label��6!   $
��Left=�61   $��301,340,0,0q�6L   $	��Top=�6a   �� 5   
   -   5         ��Name$ 	xmlviewer��5      $
��Left=�6%   $	��247��6@   $��
339,76,0,0q�6M    �� TextWrapping$
 Wrap=�6a   $��TextBox��6u   $	��Top=�6�   $	��228��6�   �� 5   
   -   5        $ Createxmlbutton��5      $��	CreateXML��6*   $
��Left=�6>   $��
352,40,0,0q�6Y   $	��Top=�6m   $��75��6�   6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGrid��5      $
��Left=�6%   $��	49,89,0,0q�6@   $	��Top=�6S   $��59��6k   $	��148��6w   �� 5   
   -   5        $ gtefiles��5      $��	Get Dll's��6#   $
��Left=�67   $��	49,61,0,0q�6R   $	��Top=�6e   $��75��6}   6�     5   
   -	   5        $ 	dataGrid1��5      $
��Left=�6&   $��
49,184,0,0q�6A   $	��Top=�6U    J� RenderTransformOrigin$ -2.583,-1.385&�6m   $��88��6�   $	��135��6�   �� 5   
   -
   5        $
 LOGS��5      $
��Left=�6    $��97��6;   $��194,174,0,0q�6G   $
 Wrap=�6\   $��TextBox��6p   $	��Top=�6   $	��131��6�   �� 5   
   -   5        $ 
Logresults��5      $��	Logresult��6%   $
��Left=�69   $��213,130,0,0q�6T   $	��Top=�6i   $��75��6�   6�   �� 5   
   -   5        $ Timert��5      $��TIme Taken in ms:��6    $
��Left=�6<   $��250,415,0,0q�6W   $	��Top=�6l   $	��186��6�   5      5      