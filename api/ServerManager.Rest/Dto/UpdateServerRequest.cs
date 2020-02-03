using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class UpdateServerRequest
    {
        public string NewName { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public ServerPropertyList NewProperties { get; set; }
    }
}
