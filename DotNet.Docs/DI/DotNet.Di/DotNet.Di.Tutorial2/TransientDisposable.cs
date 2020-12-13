using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Di.Tutorial2
{
    public class TransientDisposable : IDisposable
    {
        public void Dispose() => Console.WriteLine($"{nameof(TransientDisposable)}.Dispose()");
    }
}
