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
    
    public partial class tbl_CAT_TipoUsuario
    {
        public tbl_CAT_TipoUsuario()
        {
            this.tbl_CAT_Usuarios = new HashSet<tbl_CAT_Usuarios>();
        }
    
        public int intTipoUsuarioID { get; set; }
        public string vchTipoUsuario { get; set; }
        public Nullable<bool> bitEstatus { get; set; }
        public Nullable<System.DateTime> datFecha { get; set; }
        public string vchUserAdmin { get; set; }
    
        public virtual ICollection<tbl_CAT_Usuarios> tbl_CAT_Usuarios { get; set; }
    }
}
