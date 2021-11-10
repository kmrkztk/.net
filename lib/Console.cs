using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class ConsoleEx
    {
        public static string WriteLine(string message)
        {
            Console.WriteLine(message);
            return message;
        }
        public static void Pause()
        {
            Console.WriteLine("please type any key...");
            Console.ReadKey();
        }
        public static void LoggingUnhandledException() => System.Threading.Thread.GetDomain().UnhandledException += (sender, e) => Console.WriteLine(e.ExceptionObject);
        public static void WriteElapse(Action action, string message = null)
        {
            var tick = Environment.TickCount;
            try
            {
                action.Invoke();
            }
            finally
            {
                Console.WriteLine("{0} {1}ms", message, Environment.TickCount - tick);
            }
        }
        public static T WriteElapse<T>(Func<T> action, string message = null)
        {
            var tick = Environment.TickCount;
            try
            {
                return action.Invoke();
            }
            finally
            {
                Console.WriteLine("{0} {1}ms", message, Environment.TickCount - tick);
            }
        }
    }
}
