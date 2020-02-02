using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class ServerCommandRequest
    {
        public string Command { get; set; }
        public CommandStatus Status { get; set; }
    }
    public enum CommandStatus
    {
        Started,
        Finished
    }
}
