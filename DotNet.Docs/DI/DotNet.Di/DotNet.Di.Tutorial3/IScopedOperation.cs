using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Di.Tutorial3
{
    public interface IScopedOperation : IDisposable, IOperation
    {
        void Display();
    }
}
