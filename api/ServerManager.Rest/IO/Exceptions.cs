using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest.IO
{
    public class DirectoryListenerStoppedException : DirectoryListenerException
    {
        public DirectoryListenerStoppedException(string message, Exception inner) : base(message, inner) { }
    }

    public class DirectoryReadException : DirectoryListenerException
    {
        public DirectoryReadException(string message, Exception inner) : base(message, inner) { }
    }

    public class EventHandlerException : DirectoryListenerException
    {
        public EventHandlerException(string message, Exception inner) : base(message, inner) { }
    }

    public abstract class DirectoryListenerException : Exception
    {
        public DirectoryListenerException(string message, Exception inner) : base(message, inner) { }
    }
}
