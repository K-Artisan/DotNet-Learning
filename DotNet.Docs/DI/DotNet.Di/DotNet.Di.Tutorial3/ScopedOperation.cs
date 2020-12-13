using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Di.Tutorial3
{
    public class ScopedOperation : IScopedOperation
    {
        public string OperationId { get; } = Guid.NewGuid().ToString()[^4..];

        public void Display()
        {
            Console.WriteLine($"{OperationId}.{nameof(ScopedOperation)}.Display() at { DateTime.UtcNow}");
        }

        public void Dispose()
        {
            Console.WriteLine($"{OperationId}.{nameof(ScopedOperation)}.Dispose() at { DateTime.UtcNow}");
        }
    }
}
