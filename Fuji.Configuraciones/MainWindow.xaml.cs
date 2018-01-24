using Fuji.Configuraciones.DataAccess;
using Fuji.Configuraciones.Extensions;
using Fuji.Configuraciones.Feed2Service;
using System;
using System.Configuration;
using System.IO;
using System.Windows;

namespace Fuji.Configuraciones
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginDataAccess LoginDA;
        private ConfiguracionDataAccess ConfigDA;
        public static int id_sitio = 0;
        public static Entidades.clsConfiguracion _configura;
        public static string path = ConfigurationManager.AppSettings["ConfigDirectory"] != null ? ConfigurationManager.AppSettings["ConfigDirectory"].ToString() : ""; 
        public static string URL_Feed2 = ConfigurationManager.AppSettings["URL_Feed2"] != null ? ConfigurationManager.AppSettings["URL_Feed2"].ToString() : "";
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                _configura = new Entidades.clsConfiguracion();
                //ConfigDA = new ConfiguracionDataAccess();
                //tbl_ConfigSitio _config = new tbl_ConfigSitio();
                //string mensaje = "";
                if (File.Exists(path + "info.xml"))
                {
                    _configura = XMLConfigurator.getXMLfile();
                    if (_configura != null)
                    {
                        if (_configura.vchClaveSitio != "")
                        {
                            txtSitio.Text = _configura.vchClaveSitio;
                            txtSitio.IsEnabled = false;
                        }
                        else
                        {
                            //    SitioName site = new SitioName();
                            //    site.Show();
                            //    this.Close();
                            txtSitio.IsEnabled = true;
                        }
                    }
                    
                }
                else
                {
                    txtSitio.IsEnabled = true;
                    //    string mensaje = "";
                    //    bool existe = ConfigDA.getVerificaSitio(txtSitio.Text, ref mensaje);
                    //    if (existe)
                    //    {
                    //        tbl_ConfigSitio mdl = new tbl_ConfigSitio();
                    //        ConfigDA.getConfiguracion(txtSitio.Text, ref mensaje);
                    //    }

                }
            }
            catch (Exception eMW)
            {
                MessageBox.Show("Existe un error al iniciar la aplicación: " + eMW.Message);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPassword.Password != "" && txtUsuario.Text != "")
                {
                    //verificar en base de datos.
                    //Agregar un superUsuario
                    LoginDA = new LoginDataAccess();
                    string mensaje = "";
                    clsUsuario mdl = new clsUsuario();
                    LoginResponse response = new LoginResponse();
                    response = LoginDA.Logear(txtUsuario.Text, Security.Encrypt(txtPassword.Password), ref mdl, ref mensaje);
                    bool successPass = false;
                    bool successUser = false;
                    if (mdl != null)
                    {
                        if ((mdl.vchUsuario != "" && mdl.vchPassword != null) && (mdl.vchUsuario != "" && mdl.vchPassword != ""))
                        {
                            string pass = Security.Decrypt(mdl.vchPassword);
                            successUser = txtUsuario.Text.Trim().ToUpper() == mdl.vchUsuario.Trim().ToUpper();
                            successPass = txtPassword.Password == pass;
                        }
                    }
                    //string pass = Security.Decrypt(_configura.vchPassword);
                    //successUser = txtUsuario.Text.Trim().ToUpper() == _configura.vchUsuario.Trim().ToUpper();
                    //successPass = txtPassword.Password == pass;

                    if (successPass && successUser)
                    {
                        Configuracion conf = new Configuracion(txtSitio.Text, txtUsuario.Text, 1,1);
                        conf.Show();
                        this.Close();
                    }
                    else
                    {
                        mensaje = "";
                        if(!successUser && !successPass)
                        {
                            mensaje = "Usuario y contraseña incorrecta.";
                        }
                        else{
                            if (!successPass)
                            {
                                mensaje = "Contraseña incorrecta.";
                            }
                            if (!successUser)
                            {
                                mensaje = "Usuario incorrecto.";
                            }
                        }
                        MessageBox.Show(mensaje,"Error");
                    }
                }
                else
                {
                    MessageBox.Show("Los campos de usuario y contraseña son requeridos");
                }
            }
            catch (Exception eLogin)
            {
                Log.EscribeLog("Error en el Login: " + eLogin.Message);
                MessageBox.Show("Existe un error, favor de verificar: " + eLogin.Message);
            }
        }

        private void btnSol_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(URL_Feed2);
            }
            catch(Exception eBS)
            {
                Log.EscribeLog("Error al abrir el portar para solicitud de Sitio. " + eBS.Message);
            }
        }
    }
}
