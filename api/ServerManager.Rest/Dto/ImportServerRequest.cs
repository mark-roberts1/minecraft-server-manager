using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class ImportServerRequest
    {
        public Dictionary<string,string> OriginProperties{ get; set; }
        public Server Resultant { get; set; }
        public string Name { get; set; }
        public int port { get; set; }
    }
}
