using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Di.Sample1
{
    public class LoggingMessageWriter : IMessageWriter
    {
        private readonly ILogger<LoggingMessageWriter> _logger;

        public LoggingMessageWriter(ILogger<LoggingMessageWriter> logger)
        {
            _logger = logger;
        }

        public void Write(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
