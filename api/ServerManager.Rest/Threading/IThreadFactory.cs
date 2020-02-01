using System;
using System.IO;
using System.Threading;

namespace ServerManager.Rest.Threading
{
    public interface IThreadFactory
    {
        bool IsAlive { get; }
        ThreadCreationResult CreateSingleExecutionThread(Action<CancellationToken> start, ErrorEventHandler errorHandler);
        ThreadCreationResult CreateIntervalThread(Action threadProc, ErrorEventHandler errorHandler, int interval, bool background = true);
        Exception GetLastException();
        void Start();
        void Stop();
    }
}
