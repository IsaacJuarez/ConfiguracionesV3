   M S B A M L   `   `   `  ����O  KFuji.Configuraciones, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Fuji.ConfiguracionesFuji.Configuraciones  5   	   %    Fuji.Configuraciones.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:Fuji.Configuraciones     5   	     � Title$  Configuración - Login��5   	   $��350.925��6'   $��345.314��68   $	��400��6H   $	��429��6X    � 
ResizeMode$ NoResize=�6g    � Icon$ assets/favicon.ico��5	   	   .��5      � $��10,10,10,10q�6   ��5   
   �� 5       �� ImageSource$ assets/WallPaperWhite.jpg��6    j� Stretch$ UniformToFill=�6A   6[    � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��2* �6   .+�5   
   �� -   5         ��Name$ image��5      $
��Left=�6   $��90��6:   $	��Top=�6F   $��81,0,0,0q�6^   $	��164��6p   $��assets/FEED2CJAV.png��6|   � 5   
   $��1��6   $��	10,0,10,0q�6    � ColumnDefinitions 5      �� 5      $��1* �6#   �� 5      $��2* �6#    5      �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    .+�5      �� -   5         ��Name$ 
lblUsuario��5      $��	Usuario *��6*   $��Right=�6>   $��12(�6Z   $
��Bold"�6h   $��0,4,10,4q�6z   $��Center=�6�   	 J� RenderTransformOrigin$	 -0.158,0.825&�6�   $��28��6�   $��63��6�   �� 5       -   5        
 ��Name$
 
txtUsuario��5        �� CharacterCasing$ Upper=�6*   $��1��6B   $
��Left=�6R   $��23��6m   $��10,9,0,0q�6y    �� TextWrapping$
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��163��6�   �� 5!      -   5        $ lblContraseña��5!      $��Contraseña *��6+   $��Right=�6B   $��12(�6^   $
��Bold"�6l   $��0,7,0,1q�6~   $��1��6�   $��Center=�6�   $��28��6�   $��84��6�   �� 5"      $��1��6   $
��Left=�6)   $��26��6D   $��	10,10,0,0q�6P   $��1��6c   $	��Top=�6p   $	��163��6�   .+�5#      G� -   5         G�Name$ txtPassword��5#      $��25��63    G� 	MaxLength$ 12��6?   5$      �� 5%      -   5        $ lblSitio��5%      $��Clave de Sitio��6&   $��Right=�6?   $��12(�6[   $
��Bold"�6i   $	��0,4q�6{   $��2��6�   $��Center=�6�   $��28��6�   $��86��6�   �� 5&      -   5        $
 txtSitio��5&      $ Upper=�6(   $��1��6@   $��2��6P   $
��Left=�6]    �� 	MaxLength$ 5��6x   $��22��6�   $��10,9,0,0q�6�   {�.  $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��163��6�   �� 5'      -   5         ��Name$ btnLogin��5'      $��Entrar��6'   $��10(�68   $��1��6G   $
��Left=�6W   $��
118,10,0,0q�6r   $��3��6�   $	��Top=�6�   $��55��6�   $��17��6�   $	 
0.505,1.12&�6�   6�   �� 5(      -	   5        $ btnSol��5(      $��Solicitar Nuevo Sitio��6%   $��10(�6E   $
��Left=�6S   $��0,10,0,0q�6n   $��3��6�   $	��Top=�6�   $	��101��6�   $��17��6�   $��2��6�   5)      5*      5+      