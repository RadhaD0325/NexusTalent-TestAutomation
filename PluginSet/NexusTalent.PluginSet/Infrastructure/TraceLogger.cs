
using Microsoft.Xrm.Sdk;
using NexusTalent.PluginSet.Services;

namespace NexusTalent.PluginSet.Infrastructure
{
    public class TraceLogger : ILogger
    {
        private readonly ITracingService _tracing;

        public TraceLogger(ITracingService tracing)
        {
            _tracing = tracing;
        }

        public void Info(string message)
        {
            _tracing.Trace(message);
        }
    }
}
