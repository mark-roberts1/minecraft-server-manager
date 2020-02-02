using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class DeleteUserResponse
    {
        public int UserId { get; set; }
        public bool UserDeleted { get; set; }
    }
}
