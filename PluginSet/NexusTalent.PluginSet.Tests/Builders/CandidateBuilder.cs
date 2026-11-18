
using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace NexusTalent.PluginSet.Tests.Builders
{
    public class CandidateBuilder
    {
        private Guid _id =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        private string _name = "Test Candidate";

        private string _email = "test@example.com";

        private int _status = 871850000;

        private int? _score = null;

        public CandidateBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public CandidateBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CandidateBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public CandidateBuilder WithStatus(int status)
        {
            _status = status;
            return this;
        }

        public CandidateBuilder WithScore(int score)
        {
            _score = score;
            return this;
        }

        public CandidateBuilder WithoutScore()
        {
            _score = null;
            return this;
        }

        public Entity Build()
        {
            var candidate = new Entity("nex_candidate", _id)
            {
                ["nex_candidatename"] = _name,
                ["nex_email"] = _email,
                ["nex_candidatestatus"] =
                    new OptionSetValue(_status)
            };

            if (_score.HasValue)
            {
                // Test-only placeholder. Replace with your
                // actual score column if one exists.
                candidate["nex_score"] = _score.Value;
            }

            return candidate;
        }
    }
}
