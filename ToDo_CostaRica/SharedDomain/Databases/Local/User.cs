using SQLite;
using System;

namespace ToDoCR.SharedDomain.Databases.Local
{
    public class User
    {
        public int? Id { get; set; }
        public bool LoggedIn { get; set; }
        public bool PushNotifications { get; set; }
        public string Email { get; set; }
        public string GoogleId { get; set; }
        public string FacebookId { get; set; }

        public string PlayerId { get; set; }
        public string PushToken { get; set; }

        public Guid Token { get; set; }
        public Configuracion Config { get; set; }

        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Identificacion { get; set; }
        public string Cel { get; set; }
        public string Pin { get; set; }

        public string Password { get; set; }

        public User()
        {
            Config = new Configuracion();
            Token = Guid.NewGuid();
            Email = "Anonimo";
            PushNotifications = true;
        }

        public class Configuracion
        {
            public bool PushLoteriaNacional { get; set; }
            public bool PushLotto { get; set; }
            public bool PushChances { get; set; }
            public bool PushTresMonazos { get; set; }
            public bool PushNuevosTiempos { get; set; }
        }
    }
}
