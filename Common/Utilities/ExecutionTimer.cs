using System;
using System.Diagnostics;

namespace Utilities
{
    public class ExecutionTimer
    {
        Stopwatch _watch;
        string _what;
        object _context;

        public ExecutionTimer(string activity, dynamic context = null)
        {
            _watch = Stopwatch.StartNew();
            _what = activity;
            _context = context;
        }

        public void Dispose()
        {
            _watch.Stop();
            // Do something
        }
    }

    public class ExecutionTimerFactory
    {
        public ExecutionTimer Create(string activity, dynamic context = null)
        {
            return new ExecutionTimer(activity, context);
        }
    }
}
