using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Configuration.Provider1
{
    public class TransientFaultHandlingOptions
    {
        public bool Enabled { get; set; }
        public string AutoRetryDelay { get; set; }

    }
}
