using ServerManager.Rest.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class ServerInfo
    {
        public int ServerId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public ServerStatus Status { get; set; }
        public ServerPropertyList Properties { get; set; }

        internal string GetUniqueServerName()
            => $"{ServerId}_{Name}_{Version}";
    }

    public enum ServerStatus
    {
        Stopped,
        Started
    }
}
