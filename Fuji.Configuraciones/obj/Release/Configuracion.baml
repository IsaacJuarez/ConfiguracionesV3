   M S B A M L   `   `   `  ���� O  KFuji.Configuraciones, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Fuji.ConfiguracionesFuji.Configuraciones  -clr-namespace:MaskedTextBoxMaskedTextBox  (    "Fuji.Configuraciones.Configuracion   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     0l"clr-namespace:Fuji.Configuraciones     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:MaskedTextBox       � Title$  Configuración��$	��400��$	��600��$	��600��$	��500�� � 
ResizeMode$ NoResize=� � Icon$ assets/favicon.ico��.��� $��	0,0,0,-12q�����  �� ImageSource$ assets/WallPaperWhite.jpg�� j� Stretch$ UniformToFill=�.+��� $��	0,0,0,288q�.+�� $��20��$��	286,0,0,0q� � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ txtUserActive��$��Usuario:��$
��Bold"�
��� � �$��20,0,0,0q��� -   $ txtSitioActive��$��1��$��Sitio:��$
��Bold"�
����  �$��20,0,0,0q��� -    ��Name$ 	imgActive��$��2��$��assets/offline.png��$��15��$��15�� � ToolTip$$ Fuera de linea con el servidor��$��	30,0,10,1q��� $��0,40,0,0q�.+��� -   	 ��Name$	 
tabControl��$
��auto��$
��10,0q�$
��auto��.@��� $��Local��$��	0,-2,-2,0q�$��Right=�$��43��.��� ���	#FFE5E5E5.+��� .+�� $��10q� �� $
��Auto ��� $��* �
 � RowDefinitions
 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* �.+��� .��Clave del Sitio:�� -    ��Name$ txtClaveSitio�� �� 	MaxLength$ 4�� �� CharacterCasing$ Upper=�$��1��$��0,0,0,0q�{�.  �� $��1��.��Nombre del Sitio:�� -   $ txtNombreSitio��$ Upper=�$��1��$��1��$��0,0,0,0q��� $��2��.��IP:�� $��2��$��1��.+�� $��5q� �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �
 �� $
��Auto �.+��� -   $ txtIPC1��$�� ��$ 3��$��50���� $
��Bold"�$��.��$��1���� -   $ txtIPC2��$��2��$�� ��$ 3��$��50���� $
��Bold"�$��.��$��3���� -	   $ txtIPC3��$��4��$�� ��$ 3��$��50���� $
��Bold"�$��.��$��5���� -
   $ txtIPC4��$��6��$�� ��$ 3��$��50���� -    ��Name$ 
btnIPLocal��$��7��$��IP Local��$��20��$��Bottom=��� $��3��.��Mascara de Red:�� $��3��$��1��.+�� $��5q� �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �
 �� $
��Auto �.+��� -   $ 	txtMaskC1��$�� ��$ 3��$��50���� $
��Bold"�$��.��$��1���� -   $ 	txtMaskC2��$��2��$�� ��$ 3��$��50���� $
��Bold"�$��.��$��3���� -   $ 	txtMaskC3��$��4��$�� ��$ 3��$��50���� $
��Bold"�$��.��$��5���� -   $ 	txtMaskC4��$��6��$�� ��$ 3��$��50���� $��4��.��	Puerto:�� -   $ txtPuertoCliente��$ 5��$��4��$��1��$��0,0,0,0q��� $��5��$��AETitle:���� -   $ txtAET��$�� ��{�.  $��5��$��1���� $��6��.��
Carpeta:�� -   $ 	txtFolder��$��6��$��1��$�� ��$��0,0,85,0q��� -   $ 	btnFolder��$��Elegir��$��6��$��1��$��	365,0,0,0q��� -   $ btnSaveCliente��$��Guardar��$��453,10,10,10q��� $��Servidor��$��22��$��Bottom=�.��� ���	#FFE5E5E5.+��� .+��  �� $
��Auto � z� SharedSizeGroup$ Label���� $��* �.+��� $��0��$��26��$	��Top=�$��2��$��	0,0,362,0q�$��Right=�.��IP�� $��	206,0,0,0q�$��1��.+�� $��5q� �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �
 �� $
��Auto �.+��� -   $ txtIPS1��$�� ��$ 3��$��50���� $
��Bold"�$��.��$��1���� -   $ txtIPS2��$��2��$�� ��$ 3��$��50���� $
��Bold"�$��.��$��3���� -   $ txtIPS3��$��4��$�� ��$ 3��$��50���� $
��Bold"�$��.��$��5���� -   $ txtIPS4��$��6��$�� ��$ 3��$��50���  �� $
��auto �$ Label���� $��* �.+��� $��0�� J� RenderTransformOrigin$ 4.709,0.641&�$��2��$��	0,0,363,0q�$��Right=�.��Puerto�� -   $ txtPuertoServer��$��1��$�� ��$ 5��$��	210,0,0,0q��  �� $
��auto �$ Label���� $��* �.+��� $��0��$��Right=�$��2��$��	0,0,365,0q�$��84��.��AETitle Server�� -   $ txtAETitleServer��$�� ��$��1��$��	208,0,0,0q��� -   $ btnSaveServer��$��Guardar��$��453,10,10,10q��� -   $ 
btnRunServ��$��Reiniciar Servicio��$��
10,1,477,0q��� -   $ btnSalir��$��Salir��
�������$��487,-16,10,10q�