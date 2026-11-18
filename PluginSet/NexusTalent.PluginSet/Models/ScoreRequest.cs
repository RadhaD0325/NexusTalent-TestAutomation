
using System;

namespace NexusTalent.PluginSet.Models
{
    public sealed class ScoreRequest
    {
        public Guid CandidateId { get; set; }

        public int OldScore { get; set; }

        public int NewScore { get; set; }
    }
}
