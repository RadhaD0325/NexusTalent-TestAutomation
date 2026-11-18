
using System.Collections.Generic;
using NexusTalent.PluginSet.Services;

namespace NexusTalent.PluginSet.Tests.Fakes
{
    public class FakeLogger : ILogger
    {
        public List<string> Messages { get; }
            = new List<string>();

        public void Info(string message)
        {
            Messages.Add(message);
        }
    }
}
