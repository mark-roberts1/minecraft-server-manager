using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class UserLogin
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
