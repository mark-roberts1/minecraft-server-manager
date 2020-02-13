using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public class MinecraftServerStarter : IServerStarter
    {
        public bool StartServer(string workingDirectory, uint minMemoryMb, uint maxMemoryMb, OperatingSystem operatingSystem)
        {
            switch (operatingSystem)
            {
                case OperatingSystem.Linux:
                    return StartLinux(workingDirectory, minMemoryMb, maxMemoryMb);
                case OperatingSystem.Windows:
                    return StartWindows(workingDirectory, minMemoryMb, maxMemoryMb);
                case OperatingSystem.Mac:
                    return StartMac(workingDirectory, minMemoryMb, maxMemoryMb);
                default:
                    throw new ArgumentException("Idk how to work on this OS, bro");
            }
        }

        private bool StartWindows(string workingDirectory, uint minMemoryMb, uint maxMemoryMb)
        {
            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "java.exe",
                        Arguments = $"-Xms{minMemoryMb}M -Xmx{maxMemoryMb}M -jar server.jar nogui",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = workingDirectory
                    }
                };

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool StartLinux(string workingDirectory, uint minMemoryMb, uint maxMemoryMb)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"java -Xms{minMemoryMb}M -Xmx{maxMemoryMb}M -jar server.jar nogui\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                }
            };

            return process.Start();
        }

        private bool StartMac(string workingDirectory, uint minMemoryMb, uint maxMemoryMb)
        {
            throw new NotImplementedException("Mac is not yet supported");
        }
    }
}
