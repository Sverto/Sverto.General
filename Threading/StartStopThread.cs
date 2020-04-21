using System;
using System.Diagnostics;
using System.Threading;

namespace Sverto.General.Threading
{
    /// <summary>
	/// An on demand start/stop thread with events.
	/// </summary>
	/// <remarks></remarks>
	public class StartStopThread
    {
        /// <summary>
        /// Create a new thread with build in start/stop support
        /// </summary>
        /// <param name="onThreadLoop">Method to run every threadloop</param>
        /// <param name="loopInterval">Wait x ms between loops</param>
        /// <param name="stopTimeout">The time after a stop request at which the thread will be force stopped (0 = endless)</param>
        /// <remarks></remarks>
        public StartStopThread(Action onThreadLoop, int loopInterval = 0, int stopTimeout = 3000)
        {
            // Thread
            _OnThreadLoop = onThreadLoop;
            //_ThreadLoopInterval = loopInterval
            this.LoopInterval = loopInterval;
            _ThreadRun = false;
            _ThreadIsRunning = false;
            _ThreadLock = new object();
            _ThreadStopTimerLock = new object();
            _ThreadSleepTime = 0;
            // Timeout Timer
            _ThreadTimeout = stopTimeout;
        }

        /// <summary>
        /// Create a new thread with build in start/stop support
        /// </summary>
        /// <param name="onThreadLoop">Method to run every threadloop</param>
        /// <param name="onThreadInit">Method to run when thread started</param>
        /// <param name="onThreadStopped">Method to run when thread stopped</param>
        /// <param name="loopInterval">Wait x ms between loops</param>
        /// <param name="stopTimeout">The time after a stop request at which the thread will be force stopped (0 = endless)</param>
        public StartStopThread(Action onThreadLoop, Action onThreadInit, ThreadStopMethodDelegate onThreadStopped, int loopInterval = 0, int stopTimeout = 3000) : this(onThreadLoop, loopInterval, stopTimeout)
        {
            _OnThreadInit = onThreadInit;
            _OnThreadStopped = onThreadStopped;
        }

        #region "Delegate"
        public delegate void ThreadStopMethodDelegate(bool wasForced);
        #endregion

        #region "Fields & Properties"
        private Thread _Thread;
        private Action _OnThreadInit;
        private Action _OnThreadLoop;
        private ThreadStopMethodDelegate _OnThreadStopped;
        private bool _ThreadRun;
        private bool _ThreadIsRunning;
        private object _ThreadLock;
        private int _ThreadTimeout;
        private Thread _ThreadStopTimer;
        private bool _ThreadStopTimerIsRunning;
        private object _ThreadStopTimerLock;
        // ms
        private int _ThreadLoopInterval;
        // ms
        private int _ThreadSleepTime;

        public int? ManagedThreadId
        {
            get { return _Thread?.ManagedThreadId; }
        }

        /// <summary>
        /// Background threads do not prevent a process from terminating.
        /// </summary>
        /// <returns></returns>
        public bool IsBackground { get; set; }
        public string Name { get; set; }
        public int LoopInterval
        {
            get { return _ThreadLoopInterval; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Value cannot be less than 0.");
                _ThreadLoopInterval = value;
            }
        }
        #endregion

        #region "Thread"
        [DebuggerStepThrough]
        private void Thread_Main()
        {
            // Init thread vars
            _ThreadIsRunning = true;
            _OnThreadInit?.Invoke();

            // Main loop
            while (KeepRunning())
            {
                // Sleep requested time
                if (_ThreadSleepTime > 0)
                {
                    _ThreadSleepTime -= 5;
                    Thread.Sleep(5);
                    continue;
                }
                else if (_ThreadLoopInterval > 0)
                {
                    // Set wait time for the next loop
                    AddSleep(_ThreadLoopInterval);
                }
                // Run user's thread method
                _OnThreadLoop();
            }
            _ThreadIsRunning = false;

            _OnThreadStopped?.Invoke(false);
        }

        public bool KeepRunning()
        {
            return _ThreadRun;
        }
        #endregion

        #region "Thread Start/Stop/Wait"
        /// <summary>
        /// Start Thread
        /// </summary>
        /// <param name="waitEndPreviousRun">Wait for the previous run to end</param>
        /// <remarks></remarks>
        public void Start(bool waitEndPreviousRun = true)
        {
            // Check if thread isn't running
            if (_ThreadRun)
                return;
            // Finish of the previous run if demanded
            if (waitEndPreviousRun)
            {
                // Wait for processing before going to the next loop
                lock (_ThreadLock)
                {
                }
                //wait
                while (_ThreadIsRunning)
                {
                }
            }
            else
            {
                AbortThread();
                // Abort thread so a new thread can be started without waiting
            }
            lock (_ThreadLock)
            {
                // Start new thread
                _ThreadRun = true;
                _Thread = new Thread(Thread_Main);
                _Thread.Name = Name;
                _Thread.IsBackground = IsBackground;
                _Thread.Start();
            }
        }

        public void Stop(bool wait = false)
        {
            lock (_ThreadLock)
            {
                if (_ThreadRun)
                {
                    // Stop thread
                    _ThreadRun = false;
                    _ThreadSleepTime = 0;
                    // Reset sleep time
                    // Start timeout timer if enabled
                    if (_ThreadTimeout > 0)
                        StartThreadTimeout();
                    //wait
                    if (wait)
                    {
                        while (_ThreadIsRunning)
                        {
                        }
                    }
                }
            }
        }

        public bool IsRunning()
        {
            return _ThreadIsRunning;
        }

        // Wait x ms before triggering users thread method again
        public void AddSleep(int time)
        {
            if (_ThreadRun && time > 0)
            {
                _ThreadSleepTime += time;
            }
        }
        #endregion

        #region "Stop Thread Timeout Handling"
        private void StartThreadTimeout()
        {
            // Start thread timeout
            lock (_ThreadStopTimerLock)
            {
                // Cancel previous timeout if any
                if (_ThreadStopTimer != null && _ThreadStopTimerIsRunning)
                {
                    _ThreadStopTimer.Abort();
                }

                // Start new timeout thread
                _ThreadStopTimerIsRunning = true;
                _ThreadStopTimer = new Thread(Thread_StopTimeout);
                _ThreadStopTimer.Start();
            }
        }

        private void Thread_StopTimeout()
        {
            // Wait x time before aborting
            int waitTime = _ThreadTimeout;
            const int loopTime = 10;
            // Lock start/stop requests until previous thread is stopped (unless it's forced)
            lock (_ThreadLock)
            {
                while (waitTime > 0 & _ThreadIsRunning)
                {
                    waitTime -= loopTime;
                    Thread.Sleep(loopTime);
                }
                // Abort if needed
                //And Not _ThreadRun Then
                if (_ThreadIsRunning)
                {
                    AbortThread();
                }
            }
            _ThreadStopTimerIsRunning = false;
            // Exit thread
        }

        private void AbortThread()
        {
            bool wasThreadRunning = false;
            //
            lock (_ThreadLock)
            {
                // Force stop of thread if active
                if (_ThreadIsRunning)
                {
                    _ThreadRun = false;
                    _Thread.Abort();
                    //_Thread = Nothing
                    _ThreadIsRunning = false;
                    wasThreadRunning = true;
                }
            }
            // stopped event
            _OnThreadStopped(wasThreadRunning);
        }
        #endregion
    }
}
