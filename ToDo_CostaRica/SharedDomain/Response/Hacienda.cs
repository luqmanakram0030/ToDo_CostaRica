using System;
using System.Collections.Generic;
using System.Text;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace ToDoCR.SharedDomain.Response
{
    public partial class HaciendaContribuyente
    {
        [J("nombre")] public string Nombre { get; set; }
        [J("tipoIdentificacion")] public string TipoIdentificacion { get; set; }
        [J("regimen")] public HaciendaRegimen Regimen { get; set; }
        [J("situacion")] public HaciendaSituacion Situacion { get; set; }
        [J("actividades")] public List<HaciendaActividades> Actividades { get; set; }
        [J("code")] public string Code { get; set; }
        [J("status")] public string Status { get; set; }
    }

    public partial class HaciendaActividades
    {
        public HaciendaActividades()
        {

        }
        [J("estado")] public string Estado { get; set; }
        [J("tipo")] public string Tipo { get; set; }
        [J("codigo")] public string Codigo { get; set; }
        [J("descripcion")] public string Descripcion { get; set; }
    }

    public partial class HaciendaRegimen
    {
        public HaciendaRegimen()
        {

        }
        [J("codigo")] public long Codigo { get; set; }
        [J("descripcion")] public string Descripcion { get; set; }
    }

    public partial class HaciendaSituacion
    {
        public HaciendaSituacion()
        {

        }
        [J("moroso")] public string Moroso { get; set; }
        [J("omiso")] public string Omiso { get; set; }
        [J("estado")] public string Estado { get; set; }
        [J("administracionTributaria")] public string AdministracionTributaria { get; set; }
    }
}
