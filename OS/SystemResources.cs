using System;
using System.Diagnostics;
using System.Timers;

namespace Sverto.General.OS
{
    public class SystemResources : IDisposable
    {
        public SystemResources()
        {
            _CpuCounterApp = new PerformanceCounter();
            _MemoryCounterApp = new PerformanceCounter();
            Process currentProcess = Process.GetCurrentProcess();
            // (See Run -> perfmon.exe)
            var _with1 = _CpuCounterApp;
            _with1.CategoryName = "Process";
            _with1.CounterName = "% Processor Time";
            _with1.InstanceName = currentProcess.ProcessName;
            var _with2 = _MemoryCounterApp;
            _with2.CategoryName = "Process";
            _with2.CounterName = "Working Set - Private";
            _with2.InstanceName = currentProcess.ProcessName;
            currentProcess.Dispose();
            // Set cpu timer
            _CpuTimer = new Timer();
            _CpuTimer.Interval = 100;
            _CpuTimer.Start();
            // Set memory timer
            _MemoryTimer = new Timer();
            _MemoryTimer.Interval = 100;
            _MemoryTimer.Start();
        }

        #region "Resources"
        //Private ReadOnly _CurrentProcess As Process

        private Timer withEventsField__CpuTimer;
        private Timer _CpuTimer
        {
            get { return withEventsField__CpuTimer; }
            set
            {
                if (withEventsField__CpuTimer != null)
                {
                    withEventsField__CpuTimer.Elapsed -= cpuTimer_Tick;
                }
                withEventsField__CpuTimer = value;
                if (withEventsField__CpuTimer != null)
                {
                    withEventsField__CpuTimer.Elapsed += cpuTimer_Tick;
                }
            }
        }
        private readonly PerformanceCounter _CpuCounterApp;
        private double _CpuTimeElapsed;
        private float _CpuPrevValue;

        private Timer withEventsField__MemoryTimer;
        private Timer _MemoryTimer
        {
            get { return withEventsField__MemoryTimer; }
            set
            {
                if (withEventsField__MemoryTimer != null)
                {
                    withEventsField__MemoryTimer.Elapsed -= memoryTimer_Tick;
                }
                withEventsField__MemoryTimer = value;
                if (withEventsField__MemoryTimer != null)
                {
                    withEventsField__MemoryTimer.Elapsed += memoryTimer_Tick;
                }
            }
        }
        private readonly PerformanceCounter _MemoryCounterApp;
        private double _MemoryTimeElapsed;
        #endregion
        private int _MemoryPrevValue;

        private void cpuTimer_Tick(object sender, ElapsedEventArgs e)
        {
            _CpuTimeElapsed += _CpuTimer.Interval;
        }

        private void memoryTimer_Tick(object sender, ElapsedEventArgs e)
        {
            _MemoryTimeElapsed += _MemoryTimer.Interval;
        }

        #region "Methods"
        // Cpu
        public float GetAppCpuUsage()
        {
            float appCpuUseage = 0;
            if (_CpuTimeElapsed >= 1000)
            {
                _CpuTimeElapsed = 0;
                try
                {
                    appCpuUseage = _CpuCounterApp.NextValue() / Environment.ProcessorCount;
                    if (appCpuUseage > 100)
                    {
                        appCpuUseage = 100;
                    }
                }
                catch
                {
                    appCpuUseage = -1;
                }
                _CpuPrevValue = appCpuUseage;
            }
            else
            {
                appCpuUseage = _CpuPrevValue;
            }
            return appCpuUseage;
        }

        // Memory
        public int GetAppMemoryUsage()
        {
            // Cpu hog
            if (_MemoryTimeElapsed >= 1000)
            {
                _MemoryTimeElapsed = 0;
                _MemoryPrevValue = Convert.ToInt32(_MemoryCounterApp.NextValue());
            }
            return _MemoryPrevValue;
            //CInt(Process.GetCurrentProcess().PrivateMemorySize64)
        }
        #endregion

        #region "IDisposable Support"
        // To detect redundant calls
        private bool _Disposed;
        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    _CpuTimer.Stop();
                    _MemoryTimer.Stop();
                    _CpuCounterApp.Dispose();
                    _MemoryCounterApp.Dispose();
                    //_CurrentProcess.Dispose()
                }
            }
            _Disposed = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
