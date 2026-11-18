
using System;

namespace NexusTalent.PluginSet.Services
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
