
using NexusTalent.PluginSet.Services;
using System;
using System.Threading;

namespace NexusTalent.PluginSet.Tests.Fakes
{
    public class FixedClock : IClock
    {
        public DateTime UtcNow { get; }

        public FixedClock(DateTime dateTime)
        {
            UtcNow = dateTime;
        }
    }
}
