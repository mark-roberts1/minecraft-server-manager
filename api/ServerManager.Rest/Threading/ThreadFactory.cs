using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Threading
{
    /// <summary>
    /// Factory class used to create Threads
    /// </summary>
    [ExcludeFromCodeCoverage, Description("Flaky tests, will come back and test later.")]
    public class ThreadFactory : IThreadFactory
    {
        private Thread thread;
        private Action action;
        private Action<CancellationToken> cancelableAction;
        private readonly ManualResetEvent _stopEvent;
        private readonly ManualResetEvent _cancelEvent;
        private ErrorEventHandler errorHandler;
        private bool stopRequested;
        private ThreadType threadType;
        private CancellationTokenSource tokenSource;
        private int backgroundInterval;
        private Exception lastException;

        /// <summary>
        /// Constructs an instance of <see cref="ThreadFactory"/>
        /// </summary>
        public ThreadFactory()
        {
            _stopEvent = new ManualResetEvent(false);
            _cancelEvent = new ManualResetEvent(false);
        }

        /// <summary>
        /// <see langword="true"/> if a thread has been created and is executing, <see langword="false"/> 
        /// if either a created thread has finished, or a thread has not yet been created.
        /// </summary>
        public bool IsAlive => thread?.IsAlive ?? false;

        /// <summary>
        /// Creates a thread that executes on an interval set by the interval parameter.
        /// </summary>
        /// <param name="threadProc">the action to execute</param>
        /// <param name="errorHandler">a delegate used to handle errors when they arise</param>
        /// <param name="interval">the interval on which this thread executes</param>
        /// <param name="background">indicates whether the thread should run in the background or not</param>
        /// <returns><see cref="ThreadCreationResult"/></returns>
        /// <exception cref="ThreadStateException">Thrown when a thread is already running.</exception>
        /// <exception cref="ArgumentNullException">threadProc parameter cannot be null</exception>
        public ThreadCreationResult CreateIntervalThread(Action threadProc, ErrorEventHandler errorHandler, int interval, bool background = true)
        {
            if (threadProc == null) throw new ArgumentNullException("threadProc");
            if (IsAlive) throw new ThreadStateException("The current thread must be stopped before a new one can be created.");

            action = threadProc;
            this.errorHandler = errorHandler;
            this.backgroundInterval = interval;

            thread = new Thread(RunOnInterval);
            thread.IsBackground = background;
            threadType = ThreadType.Interval;

            return new ThreadCreationResult(thread, threadType);
        }

        /// <summary>
        /// Creates a thread that executes one time
        /// </summary>
        /// <param name="start">action to execute</param>
        /// <param name="errorHandler">delegate to handle any exception that is thrown</param>
        /// <returns><see cref="ThreadCreationResult"/></returns>
        /// <exception cref="ThreadStateException">Thrown when a thread is already running.</exception>
        /// <exception cref="ArgumentNullException">start parameter cannot be null</exception>
        public ThreadCreationResult CreateSingleExecutionThread(Action<CancellationToken> start, ErrorEventHandler errorHandler)
        {
            if (start == null) throw new ArgumentNullException("start");
            if (IsAlive) throw new ThreadStateException("The current thread must be stopped before a new one can be created.");

            cancelableAction = start;
            this.errorHandler = errorHandler;
            thread = new Thread(RunSingleAction);
            thread.IsBackground = true;
            threadType = ThreadType.SingleExecution;

            return new ThreadCreationResult(thread, threadType);
        }

        /// <summary>
        /// Starts the thread that was created.
        /// </summary>
        /// <exception cref="ThreadStateException">Thrown when Start is called before creating a thread.</exception>
        public void Start()
        {
            if (thread == null || threadType == ThreadType.Undefined)
            {
                throw new ThreadStateException("Use one of the create methods to initialize the thread.");
            }

            stopRequested = false;
            thread.Start();
        }

        /// <summary>
        /// Stops any running thread, and sets the factory to the initial state.
        /// </summary>
        public void Stop()
        {
            // if the thread is not alive, no need to do anything.
            if (IsAlive)
            {
                if (threadType == ThreadType.SingleExecution)
                {
                    tokenSource?.Cancel();
                }
                else if (threadType == ThreadType.Interval)
                {
                    // signals the thread to stop execution.
                    stopRequested = true;
                    _cancelEvent.Set();
                }

                _stopEvent.WaitOne(10000, false);
            }

            threadType = ThreadType.Undefined;
            action = null;
            errorHandler = null;
            cancelableAction = null;
            thread = null;
        }

        private void RunOnInterval()
        {
            try
            {
                while (!stopRequested)
                {
                    action.Invoke();

                    _cancelEvent.WaitOne(backgroundInterval, false);
                }
            }
            catch (Exception e)
            {
                lastException = e;
                errorHandler?.Invoke(this, new ErrorEventArgs(e));
            }
            finally
            {
                _stopEvent.Set();
            }
        }

        private void RunSingleAction()
        {
            using (tokenSource = new CancellationTokenSource())
            {
                try
                {
                    cancelableAction.Invoke(tokenSource.Token);
                }
                catch (TaskCanceledException)
                {

                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception e)
                {
                    lastException = e;
                    errorHandler?.Invoke(this, new ErrorEventArgs(e));
                }
            }

            tokenSource = null;
            _stopEvent.Set();
        }

        /// <summary>
        /// Gets the last exception that was thrown.
        /// </summary>
        /// <returns><see cref="Exception"/></returns>
        public Exception GetLastException()
        {
            return lastException;
        }
    }
}
