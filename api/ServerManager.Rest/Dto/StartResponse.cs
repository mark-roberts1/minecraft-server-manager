using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class StartResponse
    {
        public bool DidStart { get; set; }
        public string Log { get; set; }
    }
}
