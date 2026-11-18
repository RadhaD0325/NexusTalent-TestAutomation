
using System;
using NexusTalent.PluginSet.Services;

namespace NexusTalent.PluginSet.Tests.Fakes
{
    public class FakeCandidateRepository
        : ICandidateRepository
    {
        public int InterviewCount { get; set; } = 1;

        public int GetInterviewCount(Guid candidateId)
        {
            return InterviewCount;
        }
    }
}
