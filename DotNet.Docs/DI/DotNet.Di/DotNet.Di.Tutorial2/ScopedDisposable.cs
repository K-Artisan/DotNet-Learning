using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Di.Tutorial2
{
    public class ScopedDisposable : IDisposable
    {
        public void Dispose() => Console.WriteLine($"{nameof(ScopedDisposable)}.Dispose()");
    }
}
