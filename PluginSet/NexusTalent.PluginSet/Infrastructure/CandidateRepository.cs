
using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NexusTalent.PluginSet.Services;

namespace NexusTalent.PluginSet.Infrastructure
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly IOrganizationService _service;

        public CandidateRepository(IOrganizationService service)
        {
            _service = service;
        }

        public int GetInterviewCount(Guid candidateId)
        {
            var query = new QueryExpression("nex_interview")
            {
                ColumnSet = new ColumnSet(false),
                TopCount = 1
            };

            query.Criteria.AddCondition(
                "nex_candidate",
                ConditionOperator.Equal,
                candidateId);

            return _service.RetrieveMultiple(query).Entities.Count;
        }
    }
}
