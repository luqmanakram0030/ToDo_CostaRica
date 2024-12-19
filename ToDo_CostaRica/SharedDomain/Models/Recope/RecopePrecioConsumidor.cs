using Newtonsoft.Json;

namespace ToDoCR.SharedDomain.Models.Recope
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class RecopePrecioConsumidor
    {
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        public string Impuesto { get; set; }
        public string Precsinimp { get; set; }
        public string Fechaupd { get; set; }
        public string Id { get; set; }
        public string Preciototal { get; set; }
        public string Nomprod { get; set; }
        public string Margenpromedio { get; set; }
    }
}
