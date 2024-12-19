using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoCR.SharedDomain.Response
{
    public class RpTSE
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public GometaCedula Sociedad { get; set; }
        public DatosPersonaTSE Civil { get; set; }
        public HaciendaContribuyente Dimex { get; set; }
        public List<Result> ListaPersonas { get; set; }
    }
}
