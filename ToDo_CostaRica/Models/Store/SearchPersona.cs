

namespace ToDo_CostaRica.Models.Store
{
    public class SearchPersona<T>
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public long FechaTicks { get; set; }
        public string Fecha { get; set; }
        public T Response { get; set; }
    }
}
