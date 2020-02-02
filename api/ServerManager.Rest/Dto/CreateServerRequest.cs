using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class CreateServerRequest
    {
        public string Name { get; set; }
        public int Port { get; set; }

    }
}
