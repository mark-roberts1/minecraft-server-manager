using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace ServerManager.Rest.Threading
{
    [ExcludeFromCodeCoverage, Description("Flaky tests have been ignored for now. Will come back later.")]
    public struct ThreadCreationResult
    {
        private readonly Thread _thread;
        public ThreadCreationResult(Thread thread, ThreadType threadType)
        {
            _thread = thread;
            ThreadType = threadType;
        }

        public bool IsAlive => _thread?.IsAlive ?? false;
        public bool IsBackground => _thread?.IsBackground ?? false;
        public ThreadType ThreadType { get; }
    }
}
