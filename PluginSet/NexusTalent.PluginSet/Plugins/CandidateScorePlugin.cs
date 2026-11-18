
using Microsoft.Xrm.Sdk;
using NexusTalent.PluginSet.Infrastructure;
using NexusTalent.PluginSet.Models;
using NexusTalent.PluginSet.Services;
using System;

namespace NexusTalent.PluginSet.Plugins
{
    public class CandidateScorePlugin : IPlugin
    {
        public void Execute(IServiceProvider sp)
        {
            var context = (IPluginExecutionContext)
                sp.GetService(typeof(IPluginExecutionContext));

            var tracing = (ITracingService)
                sp.GetService(typeof(ITracingService));

            // Guard 1: Check message and stage
            if (context.MessageName != "Update" ||
                context.Stage != 20)
                return;

            // Guard 2: Check Target
            if (!context.InputParameters.Contains("Target") ||
                !(context.InputParameters["Target"] is Entity target))
                return;

            if (target.LogicalName != "nex_candidate")
                return;

            // Get PreImage
            context.PreEntityImages.TryGetValue(
                "PreImage", out Entity pre);

            // Build request
            int oldScore =
                pre?.GetAttributeValue<int>("nex_score") ?? 0;

            int newScore = target.Contains("nex_score")
                ? target.GetAttributeValue<int>("nex_score")
                : oldScore;

            var request = new ScoreRequest
            {
                CandidateId = target.Id,
                OldScore = oldScore,
                NewScore = newScore
            };

            // Call business logic
           // var service = new CandidateScoreService();

            var factory = (IOrganizationServiceFactory)
                sp.GetService(typeof(IOrganizationServiceFactory));

            var organizationService =
                factory.CreateOrganizationService(context.UserId);

            var repository =
                new CandidateRepository(organizationService);

            var logger = new TraceLogger(tracing);

            var clock = new SystemClock();

            var service = new CandidateScoreService(
                repository,
                logger,
                clock);

            var result = service.Evaluate(request);

            target["nex_grade"] = result.Grade;


            //var result = service.Evaluate(request);

            // Apply result
            target["nex_grade"] = result.Grade;

            tracing.Trace(
                "Candidate Grade: {0}, Notify: {1}",
                result.Grade,
                result.Notify);
        }
    }
}
