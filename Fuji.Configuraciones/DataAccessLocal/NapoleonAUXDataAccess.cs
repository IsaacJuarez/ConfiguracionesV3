using Fuji.Configuraciones.Extensions;
using System;
using System.Linq;

namespace Fuji.Configuraciones.DataAccessLocal
{
    public class NapoleonAUXDataAccess
    {
        public static NAPOLEONAUXEntities NapAuxDA;
        public static bool setConfiguracion(tbl_ConfigSitioAUX mdlConfig, ref string mensaje)
        {
            bool valido = false;
            try
            {
                NapAuxDA = new NAPOLEONAUXEntities();
                var validacion = (from item in NapAuxDA.tbl_ConfigSitioAUX
                                  where item.vchClaveSitio.Trim().ToUpper() == mdlConfig.vchClaveSitio.ToUpper().Trim()
                                  select item).ToList();
                if (validacion != null)
                {
                    if (validacion.Count() > 0)
                    {
                        tbl_ConfigSitioAUX mdl = (from item in NapAuxDA.tbl_ConfigSitioAUX
                                               where item.vchClaveSitio.Trim().ToUpper() == mdlConfig.vchClaveSitio.ToUpper().Trim()
                                               select item).First();
                        mdl = mdlConfig;
                        NapAuxDA.SaveChanges();
                    }
                    else
                    {
                        NapAuxDA.tbl_ConfigSitioAUX.Add(mdlConfig);
                        NapAuxDA.SaveChanges();
                        //id_Sitio = mdlConfig.id_Sitio;
                        valido = true;
                        mensaje = "Cambios correctos.";
                    }
                }
                else
                {
                    NapAuxDA.tbl_ConfigSitioAUX.Add(mdlConfig);
                    NapAuxDA.SaveChanges();
                    //id_Sitio = mdlConfig.id_Sitio;
                    valido = true;
                    mensaje = "Cambios correctos.";
                }
            }
            catch (Exception esC)
            {
                Log.EscribeLog("Existe un error al guardar la información" + esC.Message);
                valido = false;
                mensaje = esC.Message;
            }
            return valido;
        }
    }
}




