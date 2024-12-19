using System;
using System.Collections.Generic;
using System.Text;
using ToDoCR.SharedDomain.Response;

namespace ToDoCR.SharedDomain.Models
{
    public class MorosidadPatronalModel : BaseResponse
    {
        public string Cedula { get; set; }
        public string EstadoPatrono { get; set; }
        public string LugarPago { get; set; }
        public string MontoAdeudado { get; set; }
        public string Situacion { get; set; }
        public System.DateTime FechaDato { get; set; }
        public string Nombre { get; set; }

        public MorosidadPatronalModel()
        {
            Status = "OK";
        }
    }
}
