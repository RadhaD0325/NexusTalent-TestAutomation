
using System;

namespace NexusTalent.PluginSet.Services
{
    public interface ICandidateRepository
    {
        int GetInterviewCount(Guid candidateId);
    }
}
