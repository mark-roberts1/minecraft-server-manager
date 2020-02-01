using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ServerManager.Rest.Threading;

namespace ServerManager.Rest.IO
{
    public class DirectoryListener : IDirectoryListener
    {
        private readonly IDiskOperator _diskOperator;
        private readonly IThreadFactory _threadFactory;
        private readonly List<string> _cache = new List<string>();
        private readonly object _syncLock = new object();
        private bool disposed;
        private string path;

        /// <summary>
        /// The directory to monitor.
        /// </summary>
        public string Path
        {
            // no lock required for reads.
            get => path;
            set
            {
                // clear the cache since we will be raising events about a new directory.
                ClearCache();
                path = string.IsNullOrEmpty(value) ? value : value.EnsureTrailingSlash();
            }
        }

        /// <summary>
        /// If set to <see langword="true"/>, listener will listen for created files recursively. 
        /// If <see langword="false"/>, it will only listen at the top-level.
        /// </summary>
        public bool IncludeSubdirectories { get; set; }

        /// <summary>
        /// Returns <see langword="true"/> if the listener thread is alive, <see langword="false"/> if not.
        /// </summary>
        public bool IsListening => _threadFactory.IsAlive;

        /// <summary>
        /// Constructs an instance of <see cref="DirectoryListener"/>
        /// </summary>
        /// <param name="diskOperator">object used to perform disk operations. Defaults to internal <see cref="DiskOperator"/></param>
        /// <param name="threadFactory">object used to create threads. Defaults to internal <see cref="ThreadFactory"/></param>
        public DirectoryListener(IDiskOperator diskOperator = null, IThreadFactory threadFactory = null)
        {
            _diskOperator = diskOperator ?? new DiskOperator();
            _threadFactory = threadFactory ?? new ThreadFactory();
        }

        /// <summary>
        /// Starts the listener thread.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the Path is null or empty.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when Path is set, but does not exist.</exception>
        /// <exception cref="ThreadStateException">Thrown when Start has been called after <see cref="IDirectoryListener"/> is running.</exception>
        public virtual void Start()
        {
            if (string.IsNullOrEmpty(Path))
            {
                // prevent unknown exceptions down the line.
                throw new ArgumentNullException("Directory cannot be null or empty");
            }

            else if (!_diskOperator.DirectoryExists(Path))
            {
                // prevent unknown exceptions down the line.
                throw new DirectoryNotFoundException($"Directory \"{Path}\" does not exist");
            }

            if (IsListening)
            {
                // This is important to check, because the old thread could still be running. We don't want to orphan the thread with no way to manage it.
                throw new ThreadStateException($"The directory listener for \"{Path}\" is already running.");
            }

            _threadFactory.CreateIntervalThread(Run, Thread_Error, 5000);
            _threadFactory.Start();
        }

        /// <summary>
        /// Stops the listener thread. Does not clear the cache.
        /// </summary>
        public virtual void Stop()
        {
            _threadFactory.Stop();
        }

        /// <summary>
        /// Clears the cached files that we have already handled.
        /// </summary>
        public virtual void ClearCache()
        {
            lock (_syncLock)
            {
                _cache.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                Stop();
                ClearCache();
                createdHandler = null;
                errorHandler = null;
                disposed = true;
            }
        }

        private FileSystemEventHandler createdHandler;

        /// <summary>
        /// Event handler that fires when the listener hears a file that it hasn't heard before.
        /// </summary>
        public event FileSystemEventHandler Created
        {
            add
            {
                if (createdHandler != null)
                {
                    createdHandler = null;
                }

                createdHandler += value;
            }
            remove
            {
                createdHandler -= value;
            }
        }

        private ErrorEventHandler errorHandler;

        /// <summary>
        /// method to handle errors that occur in the listener.
        /// </summary>
        /// <exception cref="DirectoryListenerStoppedException">Thrown when an exception is causing the listener thread to stop executing.</exception>
        /// <exception cref="DirectoryReadException">Thrown when an exception is thrown when checking the path or subdirectory for files/subdirectories.</exception>
        /// <exception cref="EventHandlerException">Thrown when an exception is thrown while executing a delegate method.</exception>
        /// <exception cref="Exception">Thrown when an exception is thrown that is not expected.</exception>
        public event ErrorEventHandler Error
        {
            add
            {
                if (errorHandler != null)
                {
                    errorHandler = null;
                }

                errorHandler += value;
            }
            remove
            {
                errorHandler -= value;
            }
        }

        private void Run()
        {
            var filesFound = CheckForFiles(Path, Path);

            lock (_syncLock)
            {
                // find out what files we have in the cache but did not see this time around.
                var removedFromPath = _cache.Except(filesFound).ToList();

                // remove from the cache so it doesn't grow out of control.
                foreach (var removed in removedFromPath)
                {
                    _cache.Remove(removed);
                }
            }
        }

        private void Thread_Error(object sender, ErrorEventArgs e)
        {
            HandleException(new DirectoryListenerStoppedException("DirectoryListener stopped unexpectedly and needs to be restarted.", e.GetException()));
        }

        private IEnumerable<string> CheckForFiles(string path, string rootPath)
        {
            // keeps track of every unlocked file we see in the current path we are searching. These will be returned.
            var filesFound = new Queue<string>();
            var current = new Queue<string>();

            foreach (var file in GetFiles(path))
            {
                current.Enqueue(file);
            }

            if (IncludeSubdirectories)
            {
                foreach (var directory in GetDirectories(path))
                {
                    current.Enqueue(directory);
                }
            }

            if (current == null || !current.Any()) return filesFound;


            foreach (var node in current)
            {
                var absolutePath = _diskOperator.CombinePaths(path, node);

                try
                {
                    if (_diskOperator.FileExists(absolutePath))
                    {
                        // skip for now, if file is still here we'll check it again 5 seconds later.
                        if (_diskOperator.IsFileLocked(absolutePath)) continue;

                        filesFound.Enqueue(absolutePath);

                        lock (_syncLock)
                        {
                            if (_cache.Contains(absolutePath)) continue;
                        }

                        try
                        {
                            var relativeToRoot = absolutePath.Replace(rootPath, string.Empty);

                            var args = new FileSystemEventArgs(WatcherChangeTypes.Created, rootPath, relativeToRoot);

                            createdHandler?.Invoke(this, args);
                        }
                        catch (Exception e)
                        {
                            HandleException(new EventHandlerException($"An error occurred while invoking Created delegate {createdHandler.Method.Name}.", e));
                        }

                        lock (_syncLock)
                        {
                            _cache.Add(absolutePath);
                        }
                    }
                    else if (_diskOperator.DirectoryExists(absolutePath) && IncludeSubdirectories)
                    {
                        foreach(var innerNode in CheckForFiles(absolutePath, rootPath))
                        {
                            filesFound.Enqueue(innerNode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }

            return filesFound;
        }

        private IEnumerable<string> GetFiles(string path)
        {
            try
            {
                return _diskOperator.GetFiles(path).Select(s => s.Replace(path.EnsureTrailingSlash(), string.Empty));
            }
            catch (Exception e)
            {
                HandleException(new DirectoryReadException($"An error occurred while checking {path} for files.", e));
            }

            return new List<string>();
        }

        private IEnumerable<string> GetDirectories(string path)
        {
            try
            {
                return _diskOperator.GetDirectories(path).Select(s => s.Replace(path.EnsureTrailingSlash(), string.Empty));
            }
            catch (Exception e)
            {
                HandleException(new DirectoryReadException($"An error occurred while checking {path} for subdirectories.", e));
            }

            return new List<string>();
        }

        private void HandleException(Exception exception)
        {
            if (errorHandler == null) throw exception;

            errorHandler.Invoke(this, new ErrorEventArgs(exception));
        }
    }
}
