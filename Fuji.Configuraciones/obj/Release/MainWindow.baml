   M S B A M L   `   `   `  ���� O  KFuji.Configuraciones, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Fuji.ConfiguracionesFuji.Configuraciones  %    Fuji.Configuraciones.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:Fuji.Configuraciones       � Title$  Configuración - Login��$��350.925��$��345.314��$	��400��$	��429�� � 
ResizeMode$ NoResize=� � Icon$ assets/favicon.ico��.��� $��10,10,10,10q�����  �� ImageSource$ assets/WallPaperWhite.jpg�� j� Stretch$ UniformToFill=� � RowDefinitions �� $��1* ��� $��2* �.+��� -    ��Name$ image��$
��Left=�$��90��$	��Top=�$��81,0,0,0q�$	��164��$��assets/FEED2CJAV.png��� $��1��$��	10,0,10,0q� � ColumnDefinitions �� $��1* ��� $��2* � �� $��1* ��� $��1* ��� $��1* ��� $��1* �.+��� -    ��Name$ 
lblUsuario��$��	Usuario *��$��Right=�$��12(�$
��Bold"�$��0,4,10,4q�$��Center=�	 J� RenderTransformOrigin$	 -0.158,0.825&�$��28��$��63���� -   
 ��Name$
 
txtUsuario�� �� CharacterCasing$ Upper=�$��1��$
��Left=�$��23��$��10,9,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��163���� -   $ lblContraseña��$��Contraseña *��$��Right=�$��12(�$
��Bold"�$��0,7,0,1q�$��1��$��Center=�$��28��$��84���� $��1��$
��Left=�$��26��$��	10,10,0,0q�$��1��$	��Top=�$	��163��.+�G� -    G�Name$ txtPassword��$��25�� G� 	MaxLength$ 12���� -   $ lblSitio��$��Clave de Sitio��$��Right=�$��12(�$
��Bold"�$	��0,4q�$��2��$��Center=�$��28��$��86���� -   $
 txtSitio��$ Upper=�$��1��$��2��$
��Left=� �� 	MaxLength$ 5��$��22��$��10,9,0,0q�{�.  $
 Wrap=�$�� ��$	��Top=�$	��163���� -    ��Name$ btnLogin��$��Entrar��$��10(�$��1��$
��Left=�$��
118,10,0,0q�$��3��$	��Top=�$��55��$��17��$	 
0.505,1.12&��� -	   $ btnSol��$��Solicitar Nuevo Sitio��$��10(�$
��Left=�$��0,10,0,0q�$��3��$	��Top=�$	��101��$��17��$��2��