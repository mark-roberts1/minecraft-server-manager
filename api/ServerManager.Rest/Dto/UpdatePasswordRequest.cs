using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class UpdatePasswordRequest
    {
        public string OriginalPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
