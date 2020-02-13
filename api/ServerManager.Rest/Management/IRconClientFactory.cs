using Rcon.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public interface IRconClientFactory
    {
        IRconClient GetRconClient(string address, int port);
    }
}
