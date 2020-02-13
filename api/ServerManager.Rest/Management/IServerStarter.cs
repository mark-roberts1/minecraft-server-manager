using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public interface IServerStarter
    {
        bool StartServer(string workingDirectory, uint minMemoryMb, uint maxMemoryMb, OperatingSystem operatingSystem);
    }

    public enum OperatingSystem
    {
        Linux,
        Windows,
        Mac
    }
}
