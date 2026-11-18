
using System;
using NexusTalent.PluginSet.Models;

namespace NexusTalent.PluginSet.Services
{
    public class CandidateScoreService
    {
        private readonly ICandidateRepository _repository;
        private readonly ILogger _logger;
        private readonly IClock _clock;

        public CandidateScoreService(
            ICandidateRepository repository,
            ILogger logger,
            IClock clock)
        {
            _repository = repository;
            _logger = logger;
            _clock = clock;
        }

        public ScoreResult Evaluate(ScoreRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.NewScore < 0 ||
                request.NewScore > 100)
                throw new ArgumentOutOfRangeException(
                    nameof(request.NewScore));

            int interviewCount =
                _repository.GetInterviewCount(request.CandidateId);

            bool crossedThreshold =
                request.OldScore < 70 &&
                request.NewScore >= 70;

            var result = new ScoreResult
            {
                Grade = request.NewScore >= 70 ? "A" : "B",

                Notify = crossedThreshold && interviewCount > 0
            };

            _logger.Info(
                "Candidate score evaluated at " +
                _clock.UtcNow.ToString("O"));

            return result;
        }
    }
}
