using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class CreateServerRequest
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public ServerPropertyList Properties { get; set; }
    }
}
