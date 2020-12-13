using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Di.Sample1
{
    public class MessageWriter: IMessageWriter
    {
        public void Write(string message)
        {
            Console.WriteLine($"MessageWriter.Write(message: \"{message}\")");
        }
    }
}
