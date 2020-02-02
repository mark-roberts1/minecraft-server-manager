using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class Server
    {
        public int ServerId { get; set; }
        public string Name { get; set; }
        public uint Port { get; set; }
        public string Description { get; set; }
        public ServerStatus Status { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }

    public enum ServerStatus
    {
        Stopped,
        Started
    }
}
