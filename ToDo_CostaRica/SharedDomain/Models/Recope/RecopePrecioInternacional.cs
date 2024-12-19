namespace ToDoCR.SharedDomain.Models.Recope
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Periodo
    {
        [JsonProperty("desde")]
        public string Desde { get; set; }

        [JsonProperty("hasta")]
        public string Hasta { get; set; }
    }

    public class Materiale
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nomprod")]
        public string Nomprod { get; set; }

        [JsonProperty("precios")]
        public List<double> Precios { get; set; }
    }

    public class RecopePrecioInternacional
    {
        [JsonProperty("periodos")]
        public List<Periodo> Periodos { get; set; }

        [JsonProperty("materiales")]
        public List<Materiale> Materiales { get; set; }
    }

}
