using System;
using System.Diagnostics;

namespace Bion.Core
{
    public class ConsoleWatch : IDisposable
    {
        private Stopwatch _watch;
        private Func<string> _endMessage;

        public ConsoleWatch(string message, Func<string> endMessage = null)
        {
            System.Console.WriteLine(message);
            _watch = Stopwatch.StartNew();
            _endMessage = endMessage ?? (() => "Done");
        }

        public void Dispose()
        {
            System.Console.WriteLine($"{_endMessage()} in {_watch.ElapsedMilliseconds:n0}ms.");
        }
    }
}
