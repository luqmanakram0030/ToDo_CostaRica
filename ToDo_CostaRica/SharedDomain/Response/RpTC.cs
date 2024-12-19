using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoCR.SharedDomain.Response
{
    public class RpObtenerTC : BaseResponse
    {
        public List<RpTC> RpTC { get; set; }
    }

    public class RpTC
    {
        public string Entidad { get; set; }
        public decimal Venta { get; set; }
        public decimal Compra { get; set; }
        public decimal Diferencial { get; set; }
        public DateTime Fecha { get; set; }
    }
}
