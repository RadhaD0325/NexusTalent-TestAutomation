
using Moq;
using NexusTalent.PluginSet.Models;
using NexusTalent.PluginSet.Services;
using NexusTalent.PluginSet.Tests.Builders;
using NexusTalent.PluginSet.Tests.Fakes;
using System;
using Xunit;

namespace NexusTalent.PluginSet.Tests.Services
{
    public class CandidateScoreServiceTests
    {
        [Fact]
        public void Evaluate_ReturnsGradeA_WhenScoreIsAbove70()
        {
            // Arrange
            var candidate = new CandidateBuilder()
                .WithScore(80)
                .Build();

            var repository = new FakeCandidateRepository
            {
                InterviewCount = 1
            };

            var logger = new FakeLogger();

            var clock = new FixedClock(
                new DateTime(2026, 11, 10));

            var service = new CandidateScoreService(
                repository, logger, clock);

            var request = new ScoreRequest
            {
                CandidateId = candidate.Id,
                OldScore = 60,
                NewScore = candidate.GetAttributeValue<int>(
                    "nex_score")
            };

            // Act
            var result = service.Evaluate(request);

            // Assert
            Assert.Equal("A", result.Grade);
            Assert.True(result.Notify);
            Assert.Single(logger.Messages);
        }

        [Fact]
        public void Evaluate_ReturnsGradeB_WhenScoreIsBelow70()
        {
            // Arrange
            var repository = new FakeCandidateRepository();
            var logger = new FakeLogger();

            var clock = new FixedClock(
                new DateTime(2026, 11, 10));

            var service = new CandidateScoreService(
                repository, logger, clock);

            var request = new ScoreRequest
            {
                CandidateId = Guid.NewGuid(),
                OldScore = 40,
                NewScore = 50
            };

            // Act
            var result = service.Evaluate(request);

            // Assert
            Assert.Equal("B", result.Grade);
            Assert.False(result.Notify);
        }

        [Fact]
        public void Evaluate_DoesNotNotify_WhenNoInterviewExists()
        {
            // Arrange
            var repository = new FakeCandidateRepository
            {
                InterviewCount = 0
            };

            var logger = new FakeLogger();

            var clock = new FixedClock(
                new DateTime(2026, 11, 10));

            var service = new CandidateScoreService(
                repository, logger, clock);

            var request = new ScoreRequest
            {
                CandidateId = Guid.NewGuid(),
                OldScore = 60,
                NewScore = 80
            };

            // Act
            var result = service.Evaluate(request);

            // Assert
            Assert.Equal("A", result.Grade);
            Assert.False(result.Notify);
        }

        [Fact]
        public void Evaluate_ThrowsException_WhenScoreIsInvalid()
        {
            // Arrange
            var service = new CandidateScoreService(
                new FakeCandidateRepository(),
                new FakeLogger(),
                new FixedClock(DateTime.UtcNow));

            var request = new ScoreRequest
            {
                CandidateId = Guid.NewGuid(),
                OldScore = 50,
                NewScore = 120
            };

            // Act and Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => service.Evaluate(request));
        }

        [Fact]
        public void Evaluate_ReturnsGradeA_WhenScoreIs100()
        {
            var repository = new Mock<ICandidateRepository>();
            repository.Setup(x => x.GetInterviewCount(It.IsAny<Guid>()))
                      .Returns(0);

            var logger = new Mock<ILogger>();
            var clock = new Mock<IClock>();

            var service = new CandidateScoreService(
                repository.Object, logger.Object, clock.Object);

            var result = service.Evaluate(new ScoreRequest
            {
                CandidateId = Guid.NewGuid(),
                OldScore = 80,
                NewScore = 100
            });

            Assert.Equal("A", result.Grade);
        }

        [Fact]
        public void Evaluate_DoesNotNotify_WhenScoreRemainsBelow70()
        {
            var repository = new Mock<ICandidateRepository>();
            repository.Setup(x => x.GetInterviewCount(It.IsAny<Guid>()))
                      .Returns(1);

            var logger = new Mock<ILogger>();
            var clock = new Mock<IClock>();

            var service = new CandidateScoreService(
                repository.Object, logger.Object, clock.Object);

            var result = service.Evaluate(new ScoreRequest
            {
                CandidateId = Guid.NewGuid(),
                OldScore = 40,
                NewScore = 60
            });

            Assert.False(result.Notify);
        }

        [Fact]
        public void Evaluate_PassesCorrectCandidateIdToRepository()
        {
            var repository = new Mock<ICandidateRepository>();
            var candidateId = Guid.NewGuid();

            repository.Setup(x => x.GetInterviewCount(candidateId))
                      .Returns(1);

            var logger = new Mock<ILogger>();
            var clock = new Mock<IClock>();

            var service = new CandidateScoreService(
                repository.Object, logger.Object, clock.Object);

            service.Evaluate(new ScoreRequest
            {
                CandidateId = candidateId,
                OldScore = 50,
                NewScore = 75
            });

            repository.Verify(
                x => x.GetInterviewCount(candidateId),
                Times.Once);
        }

        [Fact]
        public void Evaluate_CallsLogger_WhenEvaluationCompletes()
        {
            var repository = new Mock<ICandidateRepository>();
            repository.Setup(x => x.GetInterviewCount(It.IsAny<Guid>()))
                      .Returns(0);

            var logger = new Mock<ILogger>();
            var clock = new Mock<IClock>();

            var service = new CandidateScoreService(
                repository.Object, logger.Object, clock.Object);

            service.Evaluate(new ScoreRequest
            {
                CandidateId = Guid.NewGuid(),
                OldScore = 20,
                NewScore = 40
            });

            logger.Verify(
                x => x.Info(It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public void Evaluate_DoesNotNotify_WhenOldScoreIsAlreadyAbove70()
        {
            var repository = new Mock<ICandidateRepository>();
            repository.Setup(x => x.GetInterviewCount(It.IsAny<Guid>()))
                      .Returns(1);

            var logger = new Mock<ILogger>();
            var clock = new Mock<IClock>();

            var service = new CandidateScoreService(
                repository.Object, logger.Object, clock.Object);

            var result = service.Evaluate(new ScoreRequest
            {
                CandidateId = Guid.NewGuid(),
                OldScore = 75,
                NewScore = 90
            });

            Assert.Equal("A", result.Grade);
            Assert.False(result.Notify);
        }
    }
}
