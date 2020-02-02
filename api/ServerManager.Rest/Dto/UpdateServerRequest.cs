using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class UpdateServerRequest
    {
        public string NewName { get; set; }
        public Dictionary<string, string> NewProperties { get; set; }
    }
}
