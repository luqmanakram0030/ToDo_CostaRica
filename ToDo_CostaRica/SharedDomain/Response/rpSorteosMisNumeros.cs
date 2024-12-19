using System;
using System.Collections.Generic;
using System.Text;
using ToDoCR.SharedDomain.Models.Loteria;

namespace ToDoCR.SharedDomain.Response
{
    public class RpSorteosMisNumeros : BaseResponse
    {
        public List<MiLoteriaGuardada> Lista { get; set; }
    }
}
