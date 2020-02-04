using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class CreateServerResponse
    {
        public int ServerId { get; set; }
        public bool Created { get; set; }
    }
}
