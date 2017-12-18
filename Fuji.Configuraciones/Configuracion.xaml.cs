using Fuji.Configuraciones.DataAccess;
using Fuji.Configuraciones.DataAccessLocal;
using Fuji.Configuraciones.Entidades;
using Fuji.Configuraciones.Extensions;
using Fuji.Configuraciones.Feed2Service;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Fuji.Configuraciones
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Configuracion : Window
    {
        public static bool cambiosServer = false;
        public static bool cambiosCliente = false;
        public static bool inicio = true;
        //public tbl_ConfigSitio mdlSitio = new tbl_ConfigSitio();
        //public static List<tbl_ConfigSitio> lstSitios = new List<tbl_ConfigSitio>();
        private ConfiguracionDataAccess ConfigDA;
        private LoginDataAccess LoginDA;
        public static string vchUsuarioMaster = "";
        public static string userID = "";
        public static string vchuserID = "";
        public static int intTipoUser = 0;
        public static int id_Sitio_Global = 0;
        public static bool bitEdicion = false;
        public static string ip_local_global = "";
        public static string ip_Server_global = "";
        public static string Mask_local_global = "";
        public static int intUsuarioID = 0;
        public static string vchClaveSitioGlobal = "";
        public static string path = ConfigurationManager.AppSettings["ConfigDirectory"] != null ? ConfigurationManager.AppSettings["ConfigDirectory"].ToString() : "";
        public static string IPServer = ConfigurationManager.AppSettings["IPServer"] != null ? ConfigurationManager.AppSettings["IPServer"].ToString() : "";
        public static string PuertoServer = ConfigurationManager.AppSettings["PuertoServer"] != null ? ConfigurationManager.AppSettings["PuertoServer"].ToString() : "";
        public static string ConfigRepository = ConfigurationManager.AppSettings["ConfigRepository"] != null ? ConfigurationManager.AppSettings["ConfigRepository"].ToString() : "";
        public static string PuertoCliente = ConfigurationManager.AppSettings["PuertoCliente"] != null ? ConfigurationManager.AppSettings["PuertoCliente"].ToString() : "";
        public static string AETitleServer = ConfigurationManager.AppSettings["AETitleServer"] != null ? ConfigurationManager.AppSettings["AETitleServer"].ToString() : "";
        public static bool mensajesInicial = false;



        public static Entidades.clsConfiguracion _conf;

        public static string nickname;
        public static string[] ipAddresses;
        public static string[] subnets;
        public static string[] gateways;
        public static string[] dnses;

        public Configuracion(string vchClaveSitio, string vchUsuarioLog, int userid, int _tipeUser)
        {
            try
            {
                using (new WaitCursor())
                {
                    InitializeComponent();
                    vchClaveSitioGlobal = vchClaveSitio;
                    vchuserID = vchUsuarioLog;
                    txtUserActive.Text = "Usuario: " + vchuserID;
                    txtSitioActive.Text = "Sitio: " + vchClaveSitio;
                    //txtIPS1.Text = "201";
                    //txtIPS2.Text = "149";
                    //txtIPS3.Text = "27";
                    //txtIPS4.Text = "38";
                    //txtPuertoServer.Text = "104";
                    intTipoUser = _tipeUser;
                    userID = vchuserID;
                    ArrayList nicNames = WMIHelper.GetNICNames();
                    if (nicNames.Count > 0)
                    {
                        nickname = nicNames[0].ToString();
                    }
                    loadCurrentSetting(nickname);
                    string ip_local = "";
                    ip_local = Log.GetLocalIPAddress();
                    if (ip_local != "")
                    {
                        ip_local_global = ip_local;
                    }
                    String maskg = "";
                    maskg = Log.getSubnetAddres();
                    if (maskg != "")
                    {
                        Mask_local_global = maskg;
                    }
                    _conf = new Entidades.clsConfiguracion();
                    if (File.Exists(path + "info.xml"))
                    {
                        mensajesInicial = false;
                        _conf = XMLConfigurator.getXMLfile();
                        if (_conf != null)
                        {
                            if (_conf.intTipoUsuario > 0)
                            {
                                if (_conf.intTipoUsuario == 1)
                                {
                                    foreach (TabItem item in tabControl.Items)
                                    {
                                        if ((item as TabItem).Header.ToString() == "Usuarios")
                                        {
                                            (item as TabItem).IsEnabled = true;
                                        }
                                    }
                                    //cargarUsuarios();
                                }
                                else
                                {
                                    foreach (TabItem item in tabControl.Items)
                                    {
                                        if ((item as TabItem).Header.ToString() == "Usuarios")
                                        {
                                            (item as TabItem).IsEnabled = false;
                                        }
                                    }
                                }
                            }
                        }
                        //ConfigDA = new ConfiguracionDataAccess();
                        string mensaje = "";
                        if (intTipoUser == 1)
                        {
                            EnableControls(true);
                            if (vchClaveSitio != "")
                            {
                                txtClaveSitio.IsEnabled = false;
                                txtAET.Text = "AE" + vchClaveSitio;
                                getIP();

                                //mdlSitio = ConfigDA.getConfiguracion(vchClaveSitio, ref mensaje);
                                if (_conf != null && _conf.vchClaveSitio != "")
                                {
                                    fillSitio(_conf);
                                }
                                else
                                {
                                    MessageBox.Show("Existe un error al consultar el sitio. " + mensaje);
                                }
                            }
                        }
                        else
                        {
                            if (vchClaveSitio != "")
                            {
                                //mdlSitio = ConfigDA.getConfiguracion(vchClaveSitio, ref mensaje);
                                if (_conf != null && _conf.vchClaveSitio != "")
                                {
                                    getIP();
                                    fillSitio(_conf);
                                }
                                else
                                {
                                    MessageBox.Show("Existe un error al consultar el sitio. " + mensaje);
                                }
                            }
                            else
                            {
                                EnableControls(false);
                            }
                        }
                    }
                    else
                    {
                        mensajesInicial = true;
                        string mensaje = "";
                        ConfigDA = new ConfiguracionDataAccess();
                        ClienteF2CResponse response = new ClienteF2CResponse();
                        response = ConfigDA.getVerificaSitio(vchClaveSitio, id_Sitio_Global, ref mensaje);
                        if (response.valido)
                        {
                            Feed2Service.tbl_ConfigSitio mdl = new Feed2Service.tbl_ConfigSitio();
                            ConfigDA = new ConfiguracionDataAccess();
                            ClienteF2CResponse response2 = new ClienteF2CResponse();
                            response2 = ConfigDA.getConfiguracion(vchClaveSitio, id_Sitio_Global, ref mensaje);
                            if (response2 != null)
                            {
                                if (response2.ConfigSitio != null)
                                {
                                    fillSitiodb(response2.ConfigSitio);
                                    createFileXML(id_Sitio_Global, vchClaveSitio);
                                    setCliente();
                                    //setServer();
                                    string mesagge = "";
                                    DataAccessLocal.tbl_ConfigSitioAUX mdlComplete = new DataAccessLocal.tbl_ConfigSitioAUX();
                                    mdlComplete = obtenerSitioComplete(response2.ConfigSitio);
                                    if (mdlComplete != null)
                                    {
                                        NapoleonAUXDataAccess.setConfiguracion(mdlComplete, ref mesagge);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Log.EscribeLog("No existe configuración para este sitio " + vchClaveSitio);
                            MessageBox.Show("No existe configuración para este sitio.", "Error");
                        }
                    }
                }
                //enableServer();
                makeping();
            }
            catch (Exception ePL)
            {
                Log.EscribeLog("Error al iniciar la configuración: " + ePL.Message);
                MessageBox.Show("Existe un error al consultar el sitio. " + ePL.Message, "Error");
            }
        }

        private DataAccessLocal.tbl_ConfigSitioAUX obtenerSitioComplete(Feed2Service.clsConfiguracion mdlC)
        {
            DataAccessLocal.tbl_ConfigSitioAUX mdl = new DataAccessLocal.tbl_ConfigSitioAUX();
            try
            {
                mdl.id_Sitio = mdlC.id_Sitio> 0 ? mdlC.id_Sitio : id_Sitio_Global;
                mdl.bitActivo = true;
                mdl.datFechaSistema = DateTime.Now;
                mdl.intPuertoCliente = mdlC.intPuertoCliente > 0 ? mdlC.intPuertoCliente : Convert.ToInt32(txtPuertoCliente.ToString());
                //mdl.in_tPuertoServer = mdlC.intPuertoServer > 0 ? mdlC.intPuertoServer : Convert.ToInt32(txtPuertoServer.ToString());
                mdl.vchAETitle = mdlC.vchAETitle != "" ? mdlC.vchAETitle : txtAET.Text.ToString();
                //mdl.vchAETitleServer = mdlC.vchAETitleServer != "" ? mdlC.vchAETitleServer : txtAETitleServer.Text.ToString();
                mdl.vchClaveSitio = mdlC.vchClaveSitio != "" ? mdlC.vchClaveSitio : txtClaveSitio.Text.ToString();
                mdl.vchIPCliente = mdlC.vchIPCliente != "" ? mdlC.vchIPCliente : (txtIPC1.Text.ToString() + "." + txtIPC2.Text.ToString() + "." + txtIPC3.Text.ToString() + "." + txtIPC4.Text.ToString());
                //mdl.vchIPServidor = mdlC.vchIPServidor != "" ? mdlC.vchIPServidor : (txtIPS1.Text.ToString() + "." + txtIPS2.Text.ToString() + "." + txtIPS3.Text.ToString() + "." + txtIPS4.Text.ToString());
                mdl.vchMaskCliente = mdlC.vchMaskCliente != "" ? mdlC.vchMaskCliente : (txtMaskC1.Text.ToString() + "." + txtMaskC2.Text.ToString() + "." + txtMaskC3.Text.ToString() + "." + txtMaskC4.Text.ToString());
                mdl.vchnombreSitio = mdlC.vchNombreSitio != "" ? mdlC.vchNombreSitio : txtNombreSitio.Text.ToString();
                mdl.vchPathLocal = mdlC.vchPathLocal != "" ? mdlC.vchPathLocal : txtFolder.Text.ToString();
                mdl.vchUserAdmin = mdl.vchUserAdmin != "" ? mdlC.vchUserChanges : txtUserActive.Text.ToString();
            }
            catch(Exception eGO)
            {
                Log.EscribeLog("Existe un error en obtenerSitioComplete: " + eGO.Message);
            }
            return mdl;
        }

        private void createFileXML(int id_Sitio, string vchClaveSitio)
        {
            try
            {
                ClienteF2CResponse response = new ClienteF2CResponse();
                response = ConfigDA.getXMLFileConfig("XMLCONFIG", id_Sitio, vchClaveSitio);
                if (response != null)
                {
                    if (response.vchFormato != "")
                    {
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        System.IO.StreamWriter file = new System.IO.StreamWriter(path + "info.xml");
                        file.WriteLine(response.vchFormato);
                        file.Close();
                    }
                }
            }
            catch(Exception ecFXML)
            {
                Log.EscribeLog("Existe un error en CreateFileXML: " + ecFXML.Message);
            }
        }

        //private void enableServer()
        //{
        //    try
        //    {
        //        if (id_Sitio_Global > 0)
        //        {
        //            txtIPS1.IsEnabled = true;
        //            txtIPS2.IsEnabled = true;
        //            txtIPS3.IsEnabled = true;
        //            txtIPS4.IsEnabled = true;
        //            txtPuertoServer.IsEnabled = true;
        //        }
        //        else
        //        {
        //            txtIPS1.IsEnabled = false;
        //            txtIPS2.IsEnabled = false;
        //            txtIPS3.IsEnabled = false;
        //            txtIPS4.IsEnabled = false;
        //            txtPuertoServer.IsEnabled = false;
        //        }
        //    }
        //    catch (Exception eeS)
        //    {
        //        MessageBox.Show("Error al verificar el sitio: " + eeS.Message, "Error");
        //    }
        //}

        private void fillSitio(Entidades.clsConfiguracion mdlSitio)
        {
            try
            {
                if (mdlSitio.id_Sitio > 0)
                {
                    id_Sitio_Global = mdlSitio.id_Sitio;
                    bitEdicion = true;
                }
                else
                {
                    bitEdicion = false;
                }
                txtClaveSitio.Text = mdlSitio.vchClaveSitio;
                if (mdlSitio.vchIPCliente != null && mdlSitio.vchIPCliente != "")
                {
                    string[] ipformato = mdlSitio.vchIPCliente.Split('.');
                    if (ipformato.Count() > 0)
                    {
                        txtIPC1.Text = ipformato[0];
                        txtIPC2.Text = ipformato[1];
                        txtIPC3.Text = ipformato[2];
                        txtIPC4.Text = ipformato[3];
                    }
                }

                //if (mdlSitio.vchIPServidor != null && mdlSitio.vchIPServidor != "")
                //{
                //    string[] ipformatoS = mdlSitio.vchIPServidor.Split('.');
                //    if (ipformatoS.Count() > 0)
                //    {
                //        txtIPS1.Text = ipformatoS[0];
                //        txtIPS2.Text = ipformatoS[1];
                //        txtIPS3.Text = ipformatoS[2];
                //        txtIPS4.Text = ipformatoS[3];
                //    }
                //    makeping(mdlSitio.vchIPServidor);
                //}
                if (mdlSitio.vchMaskCliente != null && mdlSitio.vchMaskCliente != "")
                {
                    string[] maskformato = mdlSitio.vchMaskCliente.Split('.');
                    if (maskformato.Count() > 0)
                    {
                        txtMaskC1.Text = maskformato[0];
                        txtMaskC2.Text = maskformato[1];
                        txtMaskC3.Text = maskformato[2];
                        txtMaskC4.Text = maskformato[3];
                    }
                }

                txtNombreSitio.Text = mdlSitio.vchNombreSitio;
                txtPuertoCliente.Text = mdlSitio.intPuertoCliente.ToString();
                //txtPuertoServer.Text = mdlSitio.intPuertoServer > 0 ? mdlSitio.intPuertoServer.ToString() : "";
                //txtAETitleServer.Text = mdlSitio.vchAETitleServer.ToString();
                txtFolder.Text = mdlSitio.vchPathLocal;
            }
            catch (Exception efS)
            {
                throw efS;
            }
        }

        private void fillSitiodb(Feed2Service.clsConfiguracion mdlSitio)
        {
            try
            {
                if (mdlSitio.id_Sitio > 0)
                {
                    id_Sitio_Global = mdlSitio.id_Sitio;
                    bitEdicion = true;
                }
                else
                {
                    bitEdicion = false;
                }
                txtClaveSitio.Text = mdlSitio.vchClaveSitio;
                if (mdlSitio.vchIPCliente != null && mdlSitio.vchIPCliente != "")
                {
                    string[] ipformato = mdlSitio.vchIPCliente.Split('.');
                    if (ipformato.Count() > 0)
                    {
                        txtIPC1.Text = ipformato[0];
                        txtIPC2.Text = ipformato[1];
                        txtIPC3.Text = ipformato[2];
                        txtIPC4.Text = ipformato[3];
                    }
                }
                else
                {
                    if (ip_local_global != "")
                    {
                        string[] ipformato = ip_local_global.Split('.');
                        if (ipformato.Count() > 0)
                        {
                            txtIPC1.Text = ipformato[0];
                            txtIPC2.Text = ipformato[1];
                            txtIPC3.Text = ipformato[2];
                            txtIPC4.Text = ipformato[3];
                        }
                    }
                }

                //if (mdlSitio.vchIPServidor != null && mdlSitio.vchIPServidor != "")
                //{
                //    string[] ipformatoS = mdlSitio.vchIPServidor.Split('.');
                //    if (ipformatoS.Count() > 0)
                //    {
                //        txtIPS1.Text = ipformatoS[0];
                //        txtIPS2.Text = ipformatoS[1];
                //        txtIPS3.Text = ipformatoS[2];
                //        txtIPS4.Text = ipformatoS[3];
                //    }
                //    makeping(mdlSitio.vchIPServidor);
                //}
                //else
                //{
                //    if (IPServer != "")
                //    {
                //        string[] ipformatoS = IPServer.Split('.');
                //        if (ipformatoS.Count() > 0)
                //        {
                //            txtIPS1.Text = ipformatoS[0];
                //            txtIPS2.Text = ipformatoS[1];
                //            txtIPS3.Text = ipformatoS[2];
                //            txtIPS4.Text = ipformatoS[3];
                //        }
                //        makeping(IPServer);
                //    }
                //}
                if (mdlSitio.vchMaskCliente != null && mdlSitio.vchMaskCliente != "")
                {
                    string[] maskformato = mdlSitio.vchMaskCliente.Split('.');
                    if (maskformato.Count() > 0)
                    {
                        txtMaskC1.Text = maskformato[0];
                        txtMaskC2.Text = maskformato[1];
                        txtMaskC3.Text = maskformato[2];
                        txtMaskC4.Text = maskformato[3];
                    }
                }
                else
                {
                    if (Mask_local_global != "")
                    {
                        string[] maskformato = Mask_local_global.Split('.');
                        if (maskformato.Count() > 0)
                        {
                            txtMaskC1.Text = maskformato[0];
                            txtMaskC2.Text = maskformato[1];
                            txtMaskC3.Text = maskformato[2];
                            txtMaskC4.Text = maskformato[3];
                        }
                    }
                }

                txtNombreSitio.Text = mdlSitio.vchNombreSitio;
                txtPuertoCliente.Text = mdlSitio.intPuertoCliente > 0 ? mdlSitio.intPuertoCliente.ToString() : (PuertoCliente != "" ? PuertoCliente : "");
                //txtPuertoServer.Text = mdlSitio.intPuertoServer > 0 ? mdlSitio.intPuertoServer.ToString() : (PuertoServer != "" ? PuertoServer : "");
                //txtAETitleServer.Text = AETitleServer;
                txtAET.Text = mdlSitio.vchAETitle;
                txtFolder.Text = ConfigRepository;
            }
            catch (Exception efS)
            {
                Log.EscribeLog("Existe un error en fillSitiodb: " + efS.Message);
            }
        }

        private void makeping()
        {
            try
            {
                LoginDA = new LoginDataAccess();
                bool success = LoginDA.getValidacionServer();
                if (success)
                {
                    imgActive.Source = new BitmapImage(new Uri(@"assets/online.png", UriKind.RelativeOrAbsolute));
                    imgActive.ToolTip = "En linea con el servidor";
                }
                else
                {
                    imgActive.Source = new BitmapImage(new Uri(@"assets/offline.png", UriKind.RelativeOrAbsolute));
                    imgActive.ToolTip = "Fuera de linea con el servidor";
                }
            }
            catch (Exception eMP)
            {
                Log.EscribeLog("Existe un error al hacer ping: " + eMP.Message);
            }
        }

        private void EnableControls(bool _activar)
        {
            try
            {
                txtClaveSitio.IsEnabled = false;
                txtIPC1.IsEnabled = _activar;
                txtIPC2.IsEnabled = _activar;
                txtIPC3.IsEnabled = _activar;
                txtIPC4.IsEnabled = _activar;

                //txtIPS1.IsEnabled = _activar;
                //txtIPS2.IsEnabled = _activar;
                //txtIPS3.IsEnabled = _activar;
                //txtIPS4.IsEnabled = _activar;

                txtMaskC1.IsEnabled = _activar;
                txtMaskC2.IsEnabled = _activar;
                txtMaskC3.IsEnabled = _activar;
                txtMaskC4.IsEnabled = _activar;

                txtNombreSitio.IsEnabled = _activar;
                txtPuertoCliente.IsEnabled = _activar;
                //txtPuertoServer.IsEnabled = _activar;
                btnSaveCliente.IsEnabled = _activar;
                //btnSaveServer.IsEnabled = _activar;
            }
            catch (Exception eEC)
            {
                throw eEC;
            }
        }

        private void btnSaveCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (new WaitCursor())
                {
                    makeping();
                    setCliente();
                }
            }
            catch (Exception eSC)
            {
                MessageBox.Show("Existe un error, favor de verificar: " + eSC.Message);
            }
        }

        private void setCliente()
        {
            try
            {
                if (validarCliente())
                {
                    ConfigDA = new ConfiguracionDataAccess();
                    Feed2Service.clsConfiguracion mdl = new Feed2Service.clsConfiguracion();
                    string mensaje = "";
                    mdl = obtenerCliente();
                    if (mdl != null)
                    {
                        bool success = false;
                        bool successDBServer = false;
                        //if (mdl.id_Sitio > 0)
                        //{
                        ConfigDA = new ConfiguracionDataAccess();
                        ClienteF2CResponse response = new ClienteF2CResponse();
                        response = ConfigDA.updateConfiguracion(mdl, ref mensaje);
                        if(!response.valido)
                        {
                            Log.EscribeLog("No  se pudo almacenar la configuración para el cliente del sitio: " + mdl.vchClaveSitio + " , error:" + mensaje);
                        }
                        mensaje = "";
                        success = XMLConfigurator.setConfiguracionClienteXML(mdl, ref mensaje);
                        if (success)
                        {
                            cambiosCliente = false;
                            //enableServer();
                            string ipcliente = txtIPC1.Text + "." + txtIPC2.Text + "." + txtIPC3.Text + "." + txtIPC4.Text;
                            string maskCliente = txtMaskC1.Text + "." + txtMaskC2.Text + "." + txtMaskC3.Text + "." + txtMaskC4.Text;
                            if (maskCliente != Mask_local_global || ip_local_global != ipcliente)
                            {
                                String DN = string.Join(",", dnses);
                                WMIHelper.SetIP(nickname, ipcliente, maskCliente, gateways[0], DN);
                                //changeIP(ipcliente, maskCliente);
                            }
                            if(!mensajesInicial)
                            {
                                MessageBox.Show("Cambios correctos.", "Feed2Cloud");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Existe un error al actuzalizar los datos: " + mensaje, "Error:");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Favor de verificar la información.", "Advertencia:");
                    }
                }
            }
            catch(Exception eSC)
            {
                Log.EscribeLog("Error en setCliente:" + eSC.Message);
                MessageBox.Show("Error en setCliente:" + eSC.Message, "Error:");
            }
        }

        private Feed2Service.clsConfiguracion obtenerCliente()
        {
            Feed2Service.clsConfiguracion mdl = new Feed2Service.clsConfiguracion();
            try
            {
                mdl.vchClaveSitio = txtClaveSitio.Text;
                mdl.vchNombreSitio = txtNombreSitio.Text;
                string ipCliente = "";
                ipCliente = txtIPC1.Text == "" ? "" : txtIPC1.Text + ".";
                ipCliente = ipCliente + (txtIPC2.Text == "" ? "" : txtIPC2.Text + ".");
                ipCliente = ipCliente + (txtIPC3.Text == "" ? "" : txtIPC3.Text + ".");
                ipCliente = ipCliente + (txtIPC4.Text == "" ? "" : txtIPC4.Text);
                mdl.vchIPCliente = ipCliente;

                string MaskCliente = "";
                MaskCliente = txtMaskC1.Text == "" ? "" : txtMaskC1.Text + ".";
                MaskCliente = MaskCliente + (txtMaskC2.Text == "" ? "" : txtMaskC2.Text + ".");
                MaskCliente = MaskCliente + (txtMaskC3.Text == "" ? "" : txtMaskC3.Text + ".");
                MaskCliente = MaskCliente + (txtMaskC4.Text == "" ? "" : txtMaskC4.Text);
                mdl.vchMaskCliente = MaskCliente;

                mdl.vchAETitle = txtAET.Text;
                
                mdl.intPuertoCliente = Convert.ToInt32(txtPuertoCliente.Text);
                mdl.vchUserChanges = userID;
                mdl.datFechaSistema = DateTime.Now;

                if (id_Sitio_Global > 0)
                {
                    mdl.id_Sitio = id_Sitio_Global;
                }
                mdl.vchPathLocal = txtFolder.Text;
            }
            catch (Exception eOC)
            {
                throw eOC;
            }
            return mdl;
        }

        private void changeIP(string ip_address, string subnet_mask)
        {
            try
            {
                ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection objMOC = objMC.GetInstances();

                foreach (ManagementObject objMO in objMOC)
                {
                    if ((bool)objMO["IPEnabled"])
                    {
                        try
                        {
                            ManagementBaseObject setIP;
                            ManagementBaseObject newIP =
                                objMO.GetMethodParameters("EnableStatic");

                            newIP["IPAddress"] = new string[] { ip_address };
                            newIP["SubnetMask"] = new string[] { subnet_mask };

                            setIP = objMO.InvokeMethod("EnableStatic", newIP, null);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception ecIP)
            {
                MessageBox.Show("Exite un error al actualizar la IP del Sitio. Favor de verificar: " + ecIP.Message, "Error:");
            }
        }

        private bool validarCliente()
        {
            bool completo = true;
            try
            {
                if (txtClaveSitio.Text.Trim() == "")
                {
                    MessageBox.Show("Capturar una clave para el sitio. Solo 4 caracteres.", "Advertencia:");
                    return false;
                }
                string ipCliente = "";
                ipCliente = txtIPC1.Text == "" ? "" : txtIPC1.Text + ".";
                if (ipCliente != "")
                {
                    ipCliente = ipCliente + (txtIPC2.Text == "" ? "" : txtIPC2.Text + ".");
                    if (ipCliente != "")
                    {
                        ipCliente = ipCliente + (txtIPC3.Text == "" ? "" : txtIPC3.Text + ".");
                        if (ipCliente != "")
                        {
                            ipCliente = ipCliente + (txtIPC4.Text == "" ? "" : txtIPC4.Text);
                        }
                    }
                }
                if (ipCliente.Trim() == "" || !validaFormatoIP(ipCliente.Trim()))
                {
                    MessageBox.Show("Capturar una dirección IP para el sitio. Revisar formato de red.", "Advertencia:");
                    return false;
                }
                if (txtNombreSitio.Text.Trim() == "")
                {
                    MessageBox.Show("Capturar un nombre para el sitio.", "Advertencia:");
                    return false;
                }
                if (txtPuertoCliente.Text.Trim() == "" || Convert.ToInt32(txtPuertoCliente.Text) > 99999)
                {
                    MessageBox.Show("Capturar un puerto para el sitio. Menor de 99999.", "Advertencia:");
                    return false;
                }

                string MaskCliente = "";
                MaskCliente = txtMaskC1.Text == "" ? "" : txtMaskC1.Text + ".";
                if (MaskCliente != "")
                {
                    MaskCliente = MaskCliente + (txtMaskC2.Text == "" ? "" : txtMaskC2.Text + ".");
                    if (MaskCliente != "")
                    {
                        MaskCliente = MaskCliente + (txtMaskC3.Text == "" ? "" : txtMaskC3.Text + ".");
                        if (MaskCliente != "")
                        {
                            MaskCliente = MaskCliente + (txtMaskC4.Text == "" ? "" : txtMaskC4.Text);
                        }
                    }
                }
                if (MaskCliente.Trim() == "" || !validaFormatoIP(MaskCliente.Trim()))
                {
                    MessageBox.Show("Capturar una mascara de red para el sitio. Revisar formato de red.", "Advertencia:");
                    return false;
                }
                if (txtFolder.Text == "")
                {
                    MessageBox.Show("Seleccionar un folder para el repositorio SCP", "Advertencia:");
                    return false;
                }
                else
                {
                    if(!Directory.Exists(txtFolder.Text))
                    {
                        MessageBox.Show("El directorio seleccionado para el path no existe.", "Advertencia:");
                        return false;
                    }
                }
            }
            catch (Exception eSC)
            {
                MessageBox.Show("Existe un error, favor de verificar: " + eSC.Message);
                completo = false;
            }
            return completo;
        }

        private bool validaFormatoIP(string str_IP)
        {
            bool valido = false;
            try
            {
                string[] IP_parametros = str_IP.Split('.');
                if (IP_parametros.Length == 4)
                {
                    foreach (string segmento in IP_parametros)
                    {
                        if (Convert.ToInt32(segmento) <= 255 && Convert.ToInt32(segmento) >= 0)
                        {
                            valido = true;
                        }
                    }
                }
            }
            catch (Exception eVIP)
            {
                MessageBox.Show("Existe un error al validar el formato de la IP: " + eVIP.Message, "Error:");
            }
            return valido;
        }

        //private void btnSaveServer_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        using (new WaitCursor())
        //        {
        //            setServer();
        //        }
        //    }
        //    catch (Exception eSC)
        //    {
        //        MessageBox.Show("Existe un error, favor de verificar: " + eSC.Message);
        //    }
        //}

        //private void setServer()
        //{
        //    try
        //    {
        //        if (validaServer())
        //        {
        //            Feed2Service.clsConfiguracion mdlServer = new Feed2Service.clsConfiguracion();
        //            if (vchClaveSitioGlobal != "")
        //            {
        //                mdlServer = obtenerServer();
        //                bool success = false;
        //                string mensaje = "";
        //                ConfigDA = new ConfiguracionDataAccess();
        //                ClienteF2CResponse response = new ClienteF2CResponse();
        //                response = ConfigDA.updateConfiguracionServer(mdlServer,ref mensaje);
        //                if(!response.valido)
        //                {
        //                    Log.EscribeLog("No  se pudo almacenar la configuración para el servidor del sitio: " + mdlServer.vchClaveSitio + " , error:" + mensaje);
        //                }
        //                success = XMLConfigurator.setConfiguracionServerXML(mdlServer, ref mensaje);
        //                if (success)
        //                {
        //                    //enableServer();
        //                    makeping(mdlServer.vchIPServidor);
        //                    cambiosServer = false;
        //                    if(!mensajesInicial)
        //                    {
        //                        MessageBox.Show("Cambios correctos.", "Feed2Cloud");
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Se cargó correctamente la configuración inicial.", "Feed2Cloud");
        //                        mensajesInicial = false;
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show(mensaje, "Error");
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("Se requiere capturar los datos del sitio.", "Advertencia");
        //            }
        //        }
        //    }
        //    catch(Exception ess)
        //    {
        //        MessageBox.Show("Existe un error en setServer: " + ess.Message);
        //    }
        //}

        //private Feed2Service.clsConfiguracion obtenerServer()
        //{
        //    Feed2Service.clsConfiguracion mdl = new Feed2Service.clsConfiguracion();
        //    try
        //    {
        //        mdl.id_Sitio = id_Sitio_Global;
        //        string ipServer = "";
        //        ipServer = txtIPS1.Text == "" ? "" : txtIPS1.Text + ".";
        //        ipServer = ipServer + (txtIPS2.Text == "" ? "" : txtIPS2.Text + ".");
        //        ipServer = ipServer + (txtIPS3.Text == "" ? "" : txtIPS3.Text + ".");
        //        ipServer = ipServer + (txtIPS4.Text == "" ? "" : txtIPS4.Text);
        //        mdl.vchIPServidor = ipServer;
        //        mdl.intPuertoServer = Convert.ToInt32(txtPuertoServer.Text);
        //        mdl.vchAETitleServer = txtAETitleServer.Text;
        //        mdl.vchClaveSitio = vchClaveSitioGlobal;
        //    }
        //    catch (Exception eoS)
        //    {
        //        Log.EscribeLog("Error al obtener datos de Servidor: " + eoS.Message);
        //    }
        //    return mdl;
        //}

        //private bool validaServer()
        //{
        //    bool validaServer = true;
        //    try
        //    {
        //        string ipServidor = "";
        //        ipServidor = txtIPS1.Text == "" ? "" : txtIPS1.Text + ".";
        //        if (ipServidor != "")
        //        {
        //            ipServidor = ipServidor + (txtIPS2.Text == "" ? "" : txtIPS2.Text + ".");
        //            if (ipServidor != "")
        //            {
        //                ipServidor = ipServidor + (txtIPS3.Text == "" ? "" : txtIPS3.Text + ".");
        //                if (ipServidor != "")
        //                {
        //                    ipServidor = ipServidor + (txtIPS4.Text == "" ? "" : txtIPS4.Text);
        //                }
        //            }
        //        }
        //        if (ipServidor.Trim() == "" || !validaFormatoIP(ipServidor.Trim()))
        //        {
        //            MessageBox.Show("Capturar una IP para el Server. Revisar el formato de red.", "Advertencia");
        //            return false;
        //        }
        //        if (txtPuertoServer.Text.Trim() == "" || Convert.ToInt32(txtPuertoServer.Text) > 99999)
        //        {
        //            MessageBox.Show("Capturar un puerto para el Server. Menor de 99999.", "Advertencia:");
        //            return false;
        //        }
        //    }
        //    catch (Exception eVS)
        //    {
        //        MessageBox.Show("Existe un error al validar la configuración del server" + eVS.Message, "Error.");
        //        validaServer = false;
        //    }
        //    return validaServer;
        //}

        private void txtPuertoCliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void txtPuertoServer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TabItem tbActive = tabControl.SelectedItem as TabItem;
                if (tbActive.Header.ToString() != "Usuarios")
                {
                    if (!inicio)
                    {
                        if (cambiosCliente || cambiosServer)
                        {
                            var result = MessageBox.Show("Desea guardar antes los cambios.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.No)
                            {
                                cambiosCliente = false;
                                cambiosServer = false;
                            }
                            else
                            {
                                tabControl.SelectedItem = tbActive;
                            }
                        }
                    }
                }
            }
            catch (Exception eTC)
            {
                MessageBox.Show("Existe un error al cambiar de control: " + eTC.Message, "Error:");
            }
        }

        private void txtClaveSitio_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!inicio)
                    solicitarGuardarCliente();
            }
            catch (Exception eSolGuar)
            {
                MessageBox.Show("Error al solicitar guardar. " + eSolGuar.Message, "Error");
            }
        }

        private void solicitarGuardarCliente()
        {
            try
            {
                cambiosCliente = true;
            }
            catch (Exception eSG)
            {
                throw eSG;
            }
        }
        private void solicitarGuardarServidor()
        {
            try
            {
                cambiosServer = true;
            }
            catch (Exception eSG)
            {
                throw eSG;
            }
        }

        private void txtNombreSitio_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!inicio)
                    solicitarGuardarCliente();
            }
            catch (Exception eSolGuar)
            {
                MessageBox.Show("Error al solicitar guardar. " + eSolGuar.Message, "Error");
            }
        }

        private void txtIPCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!inicio)
                    solicitarGuardarCliente();
            }
            catch (Exception eSolGuar)
            {
                MessageBox.Show("Error al solicitar guardar. " + eSolGuar.Message, "Error");
            }
        }

        private void txtMaskCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                solicitarGuardarCliente();
            }
            catch (Exception eSolGuar)
            {
                MessageBox.Show("Error al solicitar guardar." + eSolGuar.Message, "Error");
            }
        }

        private void txtPuertoCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!inicio)
                    solicitarGuardarCliente();
            }
            catch (Exception eSolGuar)
            {
                MessageBox.Show("Error al solicitar guardar. " + eSolGuar.Message, "Error");
            }
        }

        private void txtIPServer_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                solicitarGuardarServidor();
            }
            catch (Exception eSolGuar)
            {
                MessageBox.Show("Error al solicitar guardar. " + eSolGuar.Message, "Error");
            }
        }

        private void txtPuertoServer_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                solicitarGuardarServidor();
            }
            catch (Exception eSolGuar)
            {
                MessageBox.Show("Error al solicitar guardar. " + eSolGuar.Message, "Error");
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cambiosServer || cambiosCliente)
                {
                    var request = MessageBox.Show("¿Desea salir?, los datos no guardados se perderán.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (request == MessageBoxResult.Yes)
                    {
                        this.Close();
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    this.Close();
                    Application.Current.Shutdown();
                }
            }
            catch (Exception eBS)
            {
                MessageBox.Show("Existe un error al salir. " + eBS.Message, "Error");
            }
        }

        //private void cargarUsuarios()
        //{
        //    try
        //    {
        //        List<clsConfiguracion> _lstUsers = new List<clsConfiguracion>();
        //        _lstUsers.Add(_conf);
        //        //LoginDA = new LoginDataAccess();
        //        //_lstUsers = LoginDA.getUsuarios();
        //        if (_lstUsers != null)
        //        {
        //            if (_lstUsers.Count > 0)
        //            {
        //                dgUsuarios.ItemsSource = _lstUsers;
        //                dgUsuarios.AutoGenerateColumns = false;
        //            }
        //        }
        //    }
        //    catch (Exception eCU)
        //    {
        //        throw eCU;
        //    }
        //}

        //private void btnSaveUser_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (validaUsuarios())
        //        {
        //            clsConfiguracion mdlReq = new clsConfiguracion();
        //            string mensaje = "";
        //            mdlReq = obtenerUsuario();
        //            LoginDA = new LoginDataAccess();
        //            bool success = false;
        //            //if (intUsuarioID > 0)
        //            //{

        //            //success = LoginDA.ActualizarUsuario(mdlReq, ref mensaje);
        //            success = XMLConfigurator.setConfiguracionUsuarioXML(mdlReq, ref mensaje);
        //            if (success)
        //            {
        //                MessageBox.Show(mensaje, "Mensaje:");
        //                limpiarControlUser();
        //                cargarUsuarios();
        //            }
        //            else
        //            {
        //                MessageBox.Show(mensaje, "Error:");
        //            }
        //            //}
        //            //else
        //            //{
        //            //    //success = LoginDA.AgregarUsuario(mdlReq, ref mensaje);
        //            //    success = XMLConfigurator.setConfiguracionUsuarioXML()
        //            //    if (success)
        //            //    {
        //            //        MessageBox.Show(mensaje, "Mensaje:");
        //            //        limpiarControlUser();
        //            //        cargarUsuarios();
        //            //    }
        //            //    else
        //            //    {
        //            //        MessageBox.Show(mensaje, "Error:");
        //            //    }
        //            //}
        //        }
        //    }
        //    catch (Exception eBSU)
        //    {
        //        MessageBox.Show("Existe un erro al guardar los cambios: " + eBSU.Message);
        //    }
        //}

        //private void limpiarControlUser()
        //{
        //    try
        //    {
        //        txtUser.Text = "";
        //        txtNombre.Text = "";
        //        txtPass.Password = "";
        //        cmbTipoUser.SelectedIndex = -1;
        //        intUsuarioID = 0;
        //    }
        //    catch (Exception eLCU)
        //    {
        //        throw eLCU;
        //    }
        //}

        //private clsConfiguracion obtenerUsuario()
        //{
        //    clsConfiguracion mdl = new clsConfiguracion();
        //    try
        //    {
        //        mdl.vchUsuario = txtUser.Text;
        //        mdl.vchNombreUsuario = txtNombre.Text;
        //        mdl.vchPassword = Extensions.Security.Encrypt(txtPass.Password);
        //        mdl.intTipoUsuario = cmbTipoUser.Text.ToString().ToUpper() == "ADMINISTRADOR" ? 1 : 2;
        //        mdl.id_Sitio = lstSitios.First(x => x.vchClaveSitio == cmbSitio.Text.ToString()).id_Sitio;
        //        mdl.bitActivo = true;
        //        //mdl.datFecha = DateTime.Now;
        //        //mdl.vchUserAdmin = vchUsuarioMaster;
        //        //if(intUsuarioID > 0)
        //        //{
        //        //    mdl.intUsuarioID = intUsuarioID;
        //        //}
        //    }
        //    catch (Exception eoU)
        //    {
        //        mdl = null;
        //        throw eoU;
        //    }
        //    return mdl;
        //}

        //private bool validaUsuarios()
        //{
        //    bool valido = true;
        //    try
        //    {
        //        if (txtUser.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Capturar un usuario.", "Advertencia:");
        //            return false;
        //        }
        //        if (txtNombre.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Capturar un Nombre.", "Advertencia:");
        //            return false;
        //        }
        //        if (txtPass.Password.Trim() == "")
        //        {
        //            MessageBox.Show("Capturar una contraseña.", "Advertencia:");
        //            return false;
        //        }
        //        if (cmbTipoUser.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Capturar un tipo de usuario.", "Advertencia:");
        //            return false;
        //        }
        //    }
        //    catch (Exception eVU)
        //    {
        //        throw eVU;
        //    }
        //    return valido;
        //}

        private void txtIPC1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void txtIPC2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void txtIPC3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void txtIPC4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void txtIPC1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextboxTextCheck(sender);
            }
            catch (Exception eTBC)
            {
                MessageBox.Show("Existe un error al evaluar la ip" + eTBC.Message, "Error");
            }
        }

        private static void TextboxTextCheck(object sender)
        {
            TextBox txtbox = (TextBox)sender;
            txtbox.Text = GetNumberFromString(txtbox.Text);
            if (!string.IsNullOrWhiteSpace(txtbox.Text))
            {
                if (Convert.ToInt32(txtbox.Text) > 255)
                {
                    txtbox.Text = "255";
                }
                else if (Convert.ToInt32(txtbox.Text) < 0)
                {
                    txtbox.Text = "0";
                }
            }

            txtbox.CaretIndex = txtbox.Text.Length;
        }

        private static string GetNumberFromString(string str)
        {
            StringBuilder numberBuilder = new StringBuilder();
            foreach (char c in str)
            {
                if (char.IsNumber(c))
                {
                    numberBuilder.Append(c);
                }
            }
            return numberBuilder.ToString();
        }

        private bool focusMoved = false;
        private bool focusMovedM = false;
        private bool focusMovedS = false;

        private void txtIPC1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.OemPeriod)
                {
                    txtIPC2.Focus();
                    focusMoved = true;
                }
                else
                {
                    focusMoved = false;
                }
            }
            catch (Exception epc1)
            {
                MessageBox.Show("Existe un error al validad la primer parte de la ip: " + epc1.Message, "Error");
            }
        }

        private void txtIPC2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.OemPeriod && !focusMoved)
                {
                    txtIPC3.Focus();
                    focusMoved = true;
                }
                else
                {
                    focusMoved = false;
                }
            }
            catch (Exception epc)
            {
                MessageBox.Show("Existe un error al validad la primer parte de la ip: " + epc.Message, "Error");
            }
        }

        private void txtIPC3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.OemPeriod)
                {
                    txtIPC4.Focus();
                }
            }
            catch (Exception epc)
            {
                MessageBox.Show("Existe un error al validad la primer parte de la ip: " + epc.Message, "Error");
            }
        }

        private void txtIPC1_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox txtbx = (TextBox)sender;
                if (txtbx.Text.Length == 3)
                {
                    txtIPC2.Focus();
                }
            }
            catch (Exception epku1)
            {
                MessageBox.Show("Existe un error: " + epku1.Message, "Error:");
            }
        }

        private void txtIPC3_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox txtbx = (TextBox)sender;
                if (txtbx.Text.Length == 3)
                {
                    txtIPC4.Focus();
                }
            }
            catch (Exception epku3)
            {
                MessageBox.Show("Existe un error: " + epku3.Message, "Error:");
            }
        }

        private void txtIPC2_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox txtbx = (TextBox)sender;
                if (txtbx.Text.Length == 3)
                {
                    txtIPC3.Focus();
                }
            }
            catch (Exception epku2)
            {
                MessageBox.Show("Existe un error: " + epku2.Message, "Error:");
            }
        }

        private void txtMaskC1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.OemPeriod)
                {
                    txtMaskC2.Focus();
                    focusMovedM = true;
                }
                else
                {
                    focusMovedM = false;
                }
            }
            catch (Exception epMc1)
            {
                MessageBox.Show("Existe un error al validad la primer parte de la mascara: " + epMc1.Message, "Error");
            }
        }

        private void txtMaskC2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.OemPeriod && !focusMovedM)
                {
                    txtMaskC3.Focus();
                    focusMovedM = true;
                }
                else
                {
                    focusMovedM = false;
                }
            }
            catch (Exception epc)
            {
                MessageBox.Show("Existe un error al validad la primer parte de la ip: " + epc.Message, "Error");
            }
        }

        private void txtMaskC3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.OemPeriod)
                {
                    txtMaskC4.Focus();
                }
            }
            catch (Exception epc)
            {
                MessageBox.Show("Existe un error al validad la primer parte de la ip: " + epc.Message, "Error");
            }
        }

        private void txtMaskC4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void txtMaskC1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void txtMaskC2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void txtMaskC3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9+]");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception eTVP)
            {
                MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
            }
        }

        private void txtMaskC1_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox txtbx = (TextBox)sender;
                if (txtbx.Text.Length == 3)
                {
                    txtMaskC2.Focus();
                }
            }
            catch (Exception epku1)
            {
                MessageBox.Show("Existe un error: " + epku1.Message, "Error:");
            }
        }

        private void txtMaskC2_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox txtbx = (TextBox)sender;
                if (txtbx.Text.Length == 3)
                {
                    txtMaskC3.Focus();
                }
            }
            catch (Exception epku2)
            {
                MessageBox.Show("Existe un error: " + epku2.Message, "Error:");
            }
        }

        private void txtMaskC3_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox txtbx = (TextBox)sender;
                if (txtbx.Text.Length == 3)
                {
                    txtMaskC4.Focus();
                }
            }
            catch (Exception epku3)
            {
                MessageBox.Show("Existe un error: " + epku3.Message, "Error:");
            }
        }

        //private void txtIPS1_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Key == Key.OemPeriod)
        //        {
        //            txtIPS2.Focus();
        //            focusMovedS = true;
        //        }
        //        else
        //        {
        //            focusMovedS = true;
        //        }
        //    }
        //    catch (Exception epc1)
        //    {
        //        MessageBox.Show("Existe un error al validad la primer parte de la ip: " + epc1.Message, "Error");
        //    }
        //}

        //private void txtIPS2_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Key == Key.OemPeriod && !focusMovedS)
        //        {
        //            txtIPS3.Focus();
        //            focusMovedS = true;
        //        }
        //        else
        //        {
        //            focusMovedS = false;
        //        }
        //    }
        //    catch (Exception epc)
        //    {
        //        MessageBox.Show("Existe un error al validad la primer parte de la ip: " + epc.Message, "Error");
        //    }
        //}

        //private void txtIPS3_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Key == Key.OemPeriod)
        //        {
        //            txtIPS4.Focus();
        //        }
        //    }
        //    catch (Exception epc)
        //    {
        //        MessageBox.Show("Existe un error al validad la primer parte de la ip: " + epc.Message, "Error");
        //    }
        //}

        //private void txtIPS4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    try
        //    {
        //        Regex regex = new Regex("[^0-9+]");
        //        e.Handled = regex.IsMatch(e.Text);
        //    }
        //    catch (Exception eTVP)
        //    {
        //        MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
        //    }
        //}

        //private void txtIPS3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    try
        //    {
        //        Regex regex = new Regex("[^0-9+]");
        //        e.Handled = regex.IsMatch(e.Text);
        //    }
        //    catch (Exception eTVP)
        //    {
        //        MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
        //    }
        //}

        //private void txtIPS2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    try
        //    {
        //        Regex regex = new Regex("[^0-9+]");
        //        e.Handled = regex.IsMatch(e.Text);
        //    }
        //    catch (Exception eTVP)
        //    {
        //        MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
        //    }
        //}

        //private void txtIPS1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    try
        //    {
        //        Regex regex = new Regex("[^0-9+]");
        //        e.Handled = regex.IsMatch(e.Text);
        //    }
        //    catch (Exception eTVP)
        //    {
        //        MessageBox.Show("Existe un error al validar los datos de entrada del puerto: " + eTVP.Message, "Error:");
        //    }
        //}

        //private void txtIPS1_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        TextBox txtbx = (TextBox)sender;
        //        if (txtbx.Text.Length == 3)
        //        {
        //            txtIPS2.Focus();
        //        }
        //    }
        //    catch (Exception epku1)
        //    {
        //        MessageBox.Show("Existe un error: " + epku1.Message, "Error:");
        //    }
        //}

        //private void txtIPS2_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        TextBox txtbx = (TextBox)sender;
        //        if (txtbx.Text.Length == 3)
        //        {
        //            txtIPC3.Focus();
        //        }
        //    }
        //    catch (Exception epku2)
        //    {
        //        MessageBox.Show("Existe un error: " + epku2.Message, "Error:");
        //    }
        //}

        //private void txtIPS3_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        TextBox txtbx = (TextBox)sender;
        //        if (txtbx.Text.Length == 3)
        //        {
        //            txtIPC4.Focus();
        //        }
        //    }
        //    catch (Exception epku3)
        //    {
        //        MessageBox.Show("Existe un error: " + epku3.Message, "Error:");
        //    }
        //}

        //private void cmbSitio_Loaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        List<tbl_ConfigSitio> _lst = new List<tbl_ConfigSitio>();
        //        List<string> data = new List<string>();
        //        LoginDA = new LoginDataAccess();
        //        _lst = LoginDA.getSitios();
        //        lstSitios = _lst;
        //        if (_lst != null)
        //        {
        //            if (_lst.Count > 0)
        //            {
        //                foreach (tbl_ConfigSitio item in _lst)
        //                {
        //                    data.Add(item.vchClaveSitio);
        //                }
        //            }
        //        }
        //        // ... Obtiene la referencia del combo.
        //        var comboBox = sender as ComboBox;

        //        // ... Se asigna el itemsource.
        //        comboBox.ItemsSource = data;

        //        // ... Se selecciona el primer elemento.
        //        comboBox.SelectedIndex = 0;
        //    }
        //    catch (Exception eCS)
        //    {
        //        MessageBox.Show("Existe un error al cargar el combo: " + eCS.Message, "Error");
        //    }
        //}

        /// <summary>
		/// Loads current network configuration for the specified NIC and show in 
		/// the current configuration block
		/// </summary>
		/// <param name="nicName"></param>
		private void loadCurrentSetting(string nicName)
        {



            // Load current IP configuration for the selected NIC
            WMIHelper.GetIP(nicName, out ipAddresses, out subnets, out gateways, out dnses);

            // if network connection is disabled, no information will be available
            if (null == ipAddresses || null == subnets || null == gateways || null == dnses)
                return;

            // Show the setting
            //lblCurrentIP.Text = string.Join(",", ipAddresses);
            //lblCurrentSubnet.Text = string.Join(",", subnets);
            //lblCurrentGateway.Text = string.Join(",", gateways);
            //lblCurrentDNS.Text = string.Join(",", dnses);
        }

        private void getIP()
        {
            try
            {
                ArrayList nicNames = WMIHelper.GetNICNames();
                if (nicNames.Count > 0)
                {
                    nickname = nicNames[0].ToString();
                }
                loadCurrentSetting(nickname);


                string ip_local = "";
                ip_local = Log.GetLocalIPAddress();
                if (ip_local != "")
                {
                    ip_local_global = ip_local;
                    string[] ip_parts = ip_local.Split('.');
                    if (ip_parts.Count() == 4)
                    {
                        txtIPC1.Text = ip_parts[0].ToString();
                        txtIPC2.Text = ip_parts[1].ToString();
                        txtIPC3.Text = ip_parts[2].ToString();
                        txtIPC4.Text = ip_parts[3].ToString();
                    }
                }
                string mask_local = "";
                string mask_local1 = "";
                mask_local1 = Log.getSubnetAddres();
                string datos = mask_local1;
                mask_local = datos;
                if (mask_local != "")
                {
                    Mask_local_global = mask_local;
                    string[] mask_parts = mask_local.Split('.');
                    if (mask_parts.Count() == 4)
                    {
                        txtMaskC1.Text = mask_parts[0].ToString();
                        txtMaskC2.Text = mask_parts[1].ToString();
                        txtMaskC3.Text = mask_parts[2].ToString();
                        txtMaskC4.Text = mask_parts[3].ToString();
                    }
                }
            }
            catch (Exception egiP)
            {
                Log.EscribeLog("Existe un erorr al obtener la ip local: " + egiP.Message);
                throw egiP;
            }
        }
        private void btnIPLocal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                getIP();
            }
            catch (Exception ebIPL)
            {
                MessageBox.Show("Existe un error al obtener la IP Local: " + ebIPL.Message, "Error");
                Log.EscribeLog("Error al obtener la IP del sitio: " + ebIPL.Message);
            }
        }

        //private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        tbl_CAT_Usuarios usuario = (tbl_CAT_Usuarios)dgUsuarios.SelectedItem;
        //        if (usuario != null)
        //        {
        //            intUsuarioID = usuario.intUsuarioID;
        //            txtNombre.Text = usuario.vchNombre;
        //            txtUser.Text = usuario.vchUsuario;
        //            txtPass.Password = Security.Decrypt(usuario.vchPassword);
        //            cmbTipoUser.SelectedIndex = usuario.intTipoUsuarioID == 1 ? 0 : 1;
        //            string vchSitio = lstSitios.First(x => x.id_Sitio == usuario.id_Sitio).vchClaveSitio;
        //            cmbSitio.SelectedValue = vchSitio;
        //        }
        //    }
        //    catch (Exception eSC)
        //    {
        //        Log.EscribeLog("Existe un error al obtener la información del Usuario seleccionado: " + eSC.Message);
        //    }
        //}

        private void btnRunServ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (new WaitCursor())
                {
                    ListenerService();
                    SyncService();
                    MoveServive();
                    //SenderService();
                    MessageBox.Show("Servicios Reiniciados.");
                }
            }
            catch (Exception ebR)
            {
                Log.EscribeLog("Existe un error en btnRunServ_Click: " + ebR.Message);
                MessageBox.Show("Existe un error al reiniciar el servicio SCP: " + ebR.Message);
            }
        }

        private void MoveServive()
        {
            string nombreServicio = "MoveFilesService";
            ServiceController c = new ServiceController(nombreServicio);
            int timeoutMilisegundos = 5000;
            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilisegundos);
            try
            {
                if (c != null && c.Status == ServiceControllerStatus.Running)
                {
                    c.Stop();
                    c.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    Log.EscribeLog("MoveFilesService Detenido.");
                }
                c.Refresh();
                if (c != null && c.Status == ServiceControllerStatus.Paused)
                {
                    c.Stop();
                    c.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    Log.EscribeLog("MoveFilesService Detenido.");
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("MoveFilesService Iniciado.");
                }
                c.Refresh();
                if (c != null && c.Status == ServiceControllerStatus.Stopped)
                {
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("MoveFilesService Iniciado.");
                }
            }
            catch (Exception ex)
            {
                Log.EscribeLog("Existe un error al iniciar servicio MoveFilesService: " + ex.Message);
            }
        }

        private void SenderService()
        {
            string nombreServicio = "SenderFeed2SCUService";
            ServiceController c = new ServiceController(nombreServicio);
            int timeoutMilisegundos = 5000;
            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilisegundos);
            try
            {
                if (c != null && c.Status == ServiceControllerStatus.Running)
                {
                    c.Stop();
                    c.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    Log.EscribeLog("SenderFeed2SCUService Detenido.");
                }
                c.Refresh();
                if (c != null && c.Status == ServiceControllerStatus.Paused)
                {
                    c.Stop();
                    c.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    Log.EscribeLog("SenderFeed2SCUService Detenido.");
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("SenderFeed2SCUService Iniciado.");
                }
                c.Refresh();
                if (c != null && c.Status == ServiceControllerStatus.Stopped)
                {
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("SenderFeed2SCUService Iniciado.");
                }
            }
            catch (Exception ex)
            {
                Log.EscribeLog("Existe un error al iniciar servicio: " + ex.Message);
            }
        }

        private void SyncService()
        {
            string nombreServicio = "SyncFeed2Service";
            ServiceController c = new ServiceController(nombreServicio);
            int timeoutMilisegundos = 5000;
            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilisegundos);
            try
            {
                if (c != null && c.Status == ServiceControllerStatus.Running)
                {
                    c.Stop();
                    c.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    Log.EscribeLog("SyncFeed2Service Detenido.");
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("SyncFeed2Service Iniciado.");
                }
                c.Refresh();
                if (c != null && c.Status == ServiceControllerStatus.Paused)
                {
                    c.Stop();
                    c.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    Log.EscribeLog("SyncFeed2Service Detenido.");
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("SyncFeed2Service Iniciado.");
                }
                c.Refresh();
                if (c != null && c.Status == ServiceControllerStatus.Stopped)
                {
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("SyncFeed2Service Iniciado.");
                }
            }
            catch (Exception ex)
            {
                Log.EscribeLog("Existe un error al iniciar servicio: " + ex.Message);
            }
        }

        private void ListenerService()
        {
            string nombreServicio = "ListenerSCPService";
            ServiceController c = new ServiceController(nombreServicio);
            int timeoutMilisegundos = 5000;
            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilisegundos);
            try
            {
                if (c != null && c.Status == ServiceControllerStatus.Running)
                {
                    c.Stop();
                    c.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    Log.EscribeLog("ListenerSCPService Detenido.");
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("ListenerSCPService Iniciado.");
                }
                c.Refresh();
                if (c != null && c.Status == ServiceControllerStatus.Paused)
                {
                    c.Stop();
                    c.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    Log.EscribeLog("ListenerSCPService Detenido.");
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("ListenerSCPService Iniciado.");
                }
                c.Refresh();
                if (c != null && c.Status == ServiceControllerStatus.Stopped)
                {
                    c.Start();
                    c.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Log.EscribeLog("ListenerSCPService Iniciado.");
                }
            }
            catch (Exception ex)
            {
                Log.EscribeLog("Existe un error al iniciar servicio: " + ex.Message);
            }
        }

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        txtFolder.Text = fbd.SelectedPath;
                    }
                }
            }
            catch(Exception eFolder)
            {
                Log.EscribeLog("Existe un error al obtener la carpeta de escritura: " + eFolder.Message);
            }
        }
    }
}
