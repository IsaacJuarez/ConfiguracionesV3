//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fuji.Configuraciones.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_CAT_Usuarios
    {
        public int intUsuarioID { get; set; }
        public Nullable<int> intTipoUsuarioID { get; set; }
        public Nullable<int> intProyectoID { get; set; }
        public string vchNombre { get; set; }
        public string vchApellido { get; set; }
        public string vchUsuario { get; set; }
        public string vchPassword { get; set; }
        public Nullable<bool> bitActivo { get; set; }
        public Nullable<System.DateTime> datFecha { get; set; }
        public string vchUserAdmin { get; set; }
        public Nullable<int> id_Sitio { get; set; }
    
        public virtual tbl_CAT_TipoUsuario tbl_CAT_TipoUsuario { get; set; }
        public virtual tbl_ConfigSitio tbl_ConfigSitio { get; set; }
    }
}
