using System;
using System.Diagnostics;

namespace Utilities
{
    public class ExecutionTimer : IDisposable
    {
        Stopwatch _watch;
        string _what;
        object _context;

        public ExecutionTimer(string activity, dynamic context = null)
        {
            Console.WriteLine($"Starting activity '{activity}'");
            _watch = Stopwatch.StartNew();
            _what = activity;
            _context = context;
        }

        public void Dispose()
        {
            _watch.Stop();
            Console.WriteLine($"Ending activity '{_what}'. Completed in {_watch.ElapsedMilliseconds} ms");
        }

        public static ExecutionTimer New(string activity, dynamic context = null)
        {
            return new ExecutionTimer(activity, context);
        }
    }
}
