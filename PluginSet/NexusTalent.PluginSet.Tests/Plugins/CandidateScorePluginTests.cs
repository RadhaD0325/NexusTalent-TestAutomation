
using System;
using Xunit;
using Moq;

using Microsoft.Xrm.Sdk;
using NexusTalent.PluginSet.Plugins;
using NexusTalent.PluginSet.Tests.Builders;
using NexusTalent.PluginSet.Tests.Fakes;

namespace NexusTalent.PluginSet.Tests.Plugins
{
    public class CandidateScorePluginTests
    {
        [Fact]
        public void Execute_Returns_WhenMessageIsNotUpdate()
        {
            // Arrange
            var context = PluginContextBuilder.Build(
                message: "Create");

            var tracing = new Mock<ITracingService>();

            var provider = new TestServiceProvider(
                context.Object, tracing.Object);

            var plugin = new CandidateScorePlugin();

            // Act
            plugin.Execute(provider);

            // Assert
            Assert.DoesNotContain(
                typeof(IOrganizationServiceFactory),
                provider.RequestedServices);
        }

        [Fact]
        public void Execute_Returns_WhenStageIsNotPreOperation()
        {
            // Arrange
            var context = PluginContextBuilder.Build(
                message: "Update",
                stage: 40);

            var tracing = new Mock<ITracingService>();

            var provider = new TestServiceProvider(
                context.Object, tracing.Object);

            var plugin = new CandidateScorePlugin();

            // Act
            plugin.Execute(provider);

            // Assert
            Assert.DoesNotContain(
                typeof(IOrganizationServiceFactory),
                provider.RequestedServices);
        }

        [Fact]
        public void Execute_Returns_WhenTargetIsMissing()
        {
            // Arrange
            var context = PluginContextBuilder.Build(
                message: "Update",
                stage: 20);

            var tracing = new Mock<ITracingService>();

            var provider = new TestServiceProvider(
                context.Object, tracing.Object);

            var plugin = new CandidateScorePlugin();

            // Act
            plugin.Execute(provider);

            // Assert
            Assert.DoesNotContain(
                typeof(IOrganizationServiceFactory),
                provider.RequestedServices);
        }

        [Fact]
        public void Execute_Returns_WhenTargetEntityIsIncorrect()
        {
            // Arrange
            var target = new Entity("nex_interview")
            {
                Id = Guid.NewGuid()
            };

            var context = PluginContextBuilder.Build(
                "Update", 20, target);

            var tracing = new Mock<ITracingService>();

            var provider = new TestServiceProvider(
                context.Object, tracing.Object);

            var plugin = new CandidateScorePlugin();

            // Act
            plugin.Execute(provider);

            // Assert
            Assert.DoesNotContain(
                typeof(IOrganizationServiceFactory),
                provider.RequestedServices);
        }
    }
}
