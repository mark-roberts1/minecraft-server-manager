using ServerManager.Rest.Threading;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest.IO
{
    public interface IDirectoryListenerFactory
    {
        IDirectoryListener BuildListener(string directory, bool includeSubdirectories = false, IDiskOperator diskOperator = null, IThreadFactory threadFactory = null);
    }
}
