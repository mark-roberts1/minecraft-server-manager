using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string MinecraftUsername { get; set; }
        public string Email { get; set; }
        public UserRole UserRole { get; set; }
        public bool IsLocked { get; set; }
    }
}
