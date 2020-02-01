using System;
using System.Collections.Generic;
using System.Text;
using ServerManager.Rest.Threading;

namespace ServerManager.Rest.IO
{
    public class DirectoryListenerFactory : IDirectoryListenerFactory
    {
        public IDirectoryListener BuildListener(string directory, bool includeSubdirectories = false, IDiskOperator diskOperator = null, IThreadFactory threadFactory = null)
        {
            return new DirectoryListener(diskOperator, threadFactory)
            {
                Path = directory,
                IncludeSubdirectories = includeSubdirectories
            };
        }
    }
}
