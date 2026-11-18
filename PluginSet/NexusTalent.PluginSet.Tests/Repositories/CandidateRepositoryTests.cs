
using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NexusTalent.PluginSet.Infrastructure;

namespace NexusTalent.PluginSet.Tests.Repositories
{
    public class CandidateRepositoryTests
    {
        [Fact]
        public void GetInterviewCount_ReturnsZero_WhenNoInterviewExists()
        {
            var service = new Mock<IOrganizationService>();

            service.Setup(x => x.RetrieveMultiple(
                    It.IsAny<QueryBase>()))
                .Returns(new EntityCollection());

            var repository = new CandidateRepository(service.Object);

            int result = repository.GetInterviewCount(Guid.NewGuid());

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetInterviewCount_ReturnsOne_WhenInterviewExists()
        {
            var service = new Mock<IOrganizationService>();

            var interviews = new EntityCollection(
                new List<Entity>
                {
                    new Entity("nex_interview")
                });

            service.Setup(x => x.RetrieveMultiple(
                    It.IsAny<QueryBase>()))
                .Returns(interviews);

            var repository = new CandidateRepository(service.Object);

            int result = repository.GetInterviewCount(Guid.NewGuid());

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetInterviewCount_UsesCorrectCandidateId()
        {
            var service = new Mock<IOrganizationService>();
            var candidateId = Guid.NewGuid();

            QueryExpression? capturedQuery = null;

            service.Setup(x => x.RetrieveMultiple(
                    It.IsAny<QueryBase>()))
                .Callback<QueryBase>(query =>
                    capturedQuery = query as QueryExpression)
                .Returns(new EntityCollection());

            var repository = new CandidateRepository(service.Object);

            repository.GetInterviewCount(candidateId);

            Assert.NotNull(capturedQuery);
            Assert.Equal("nex_interview", capturedQuery.EntityName);
            Assert.Single(capturedQuery.Criteria.Conditions);

            Assert.Equal(
                "nex_candidate",
                capturedQuery.Criteria.Conditions[0].AttributeName);

            Assert.Equal(
                candidateId,
                capturedQuery.Criteria.Conditions[0].Values[0]);
        }

        [Fact]
        public void GetInterviewCount_UsesTopCountOne()
        {
            var service = new Mock<IOrganizationService>();
            QueryExpression? capturedQuery = null;

            service.Setup(x => x.RetrieveMultiple(
                    It.IsAny<QueryBase>()))
                .Callback<QueryBase>(query =>
                    capturedQuery = query as QueryExpression)
                .Returns(new EntityCollection());

            var repository = new CandidateRepository(service.Object);

            repository.GetInterviewCount(Guid.NewGuid());

            Assert.NotNull(capturedQuery);
            Assert.Equal(1, capturedQuery.TopCount);
        }

        [Fact]
        public void GetInterviewCount_ThrowsException_WhenDataverseFails()
        {
            var service = new Mock<IOrganizationService>();

            service.Setup(x => x.RetrieveMultiple(
                    It.IsAny<QueryBase>()))
                .Throws(new InvalidOperationException(
                    "Dataverse query failed"));

            var repository = new CandidateRepository(service.Object);

            Assert.Throws<InvalidOperationException>(
                () => repository.GetInterviewCount(Guid.NewGuid()));
        }
    }
}
