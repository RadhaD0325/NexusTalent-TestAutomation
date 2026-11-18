
using System;
using NexusTalent.PluginSet.Services;

namespace NexusTalent.PluginSet.Infrastructure
{
    public class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
