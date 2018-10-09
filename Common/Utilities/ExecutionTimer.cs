using System;
using System.Diagnostics;

namespace Utilities
{
    public class Demo
    {
        public static void TimeTask(string activityName, Action action)
        {
            Console.WriteLine($"========================= BEGIN =========================");
            Console.WriteLine($"------------------ '{activityName}'");
            var sw = Stopwatch.StartNew();

            try
            {
                action();
            }
            finally
            {

                sw.Stop();
                Console.WriteLine($"Ending activity '{activityName}'. Completed in {sw.ElapsedMilliseconds} ms");
                Console.WriteLine($"<------------------------ END -------------------------->");
                Console.WriteLine();
            }
        }

        public static void TimeStep(string step, Action action)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                action();
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"Step '{step}' took {sw.ElapsedMilliseconds} ms");
            }
        }
    }
}
