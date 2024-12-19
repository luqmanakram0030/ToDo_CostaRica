using System.Collections.Generic;
using ToDoCR.SharedDomain.Models;

namespace ToDoCR.SharedDomain.Response
{
    public class RpOpinion : BaseResponse
    {
        public List<Opinion> Opinions { get; set; }
    }
}
