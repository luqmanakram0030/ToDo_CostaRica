using System;
using System.Collections.Generic;
using System.Text;

using ToDoCR.SharedDomain.Models.Loteria;

namespace ToDoCR.SharedDomain.Response
{
    public class RpLogin
    {
        public string Status { get; set; }
        public string User { get; set; }
        public string Mensaje { get; set; }
        public RpLoginData Data { get; set; }

        public class RpLoginData
        {
            public List<MiLoteriaGuardada> MiLoteriaGuardada;
        }
    }
}
