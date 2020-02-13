using Rcon.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public class RconClientFactory : IRconClientFactory
    {
        public IRconClient GetRconClient(string address, int port)
        {
            return new RconClient(address, port);
        }
    }
}
