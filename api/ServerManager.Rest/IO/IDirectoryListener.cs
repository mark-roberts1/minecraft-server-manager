using System;
using System.IO;
using System.Threading;

namespace ServerManager.Rest.IO
{
    public interface IDirectoryListener : IDisposable
    {
        /// <summary>
        /// The directory to monitor.
        /// </summary>
        string Path { get; set; }
        /// <summary>
        /// If set to <see langword="true"/>, listener will listen for created files recursively. 
        /// If <see langword="false"/>, it will only listen at the top-level.
        /// </summary>
        bool IncludeSubdirectories { get; set; }
        /// <summary>
        /// Returns <see langword="true"/> if the listener thread is alive, <see langword="false"/> if not.
        /// </summary>
        bool IsListening { get; }
        /// <summary>
        /// Starts the listener thread.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the Path is null or empty.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when Path is set, but does not exist.</exception>
        /// <exception cref="ThreadStateException">Thrown when Start has been called after <see cref="IDirectoryListener"/> is running.</exception>
        void Start();
        /// <summary>
        /// Stops the listener thread. Does not clear the cache.
        /// </summary>
        void Stop();
        /// <summary>
        /// Clears the cached files that we have already handled.
        /// </summary>
        void ClearCache();
        /// <summary>
        /// Event handler that fires when the listener hears a file that it hasn't heard before.
        /// </summary>
        event FileSystemEventHandler Created;
        /// <summary>
        /// method to handle errors that occur in the listener.
        /// </summary>
        /// <exception cref="DirectoryListenerStoppedException">Thrown when an exception is causing the listener thread to stop executing.</exception>
        /// <exception cref="DirectoryReadException">Thrown when an exception is thrown when checking the path or subdirectory for files/subdirectories.</exception>
        /// <exception cref="EventHandlerException">Thrown when an exception is thrown while executing a delegate method.</exception>
        /// <exception cref="Exception">Thrown when an exception is thrown that is not expected.</exception>
        event ErrorEventHandler Error;
    }
}
