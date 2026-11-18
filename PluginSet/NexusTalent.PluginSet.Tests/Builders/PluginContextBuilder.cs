
using Microsoft.Xrm.Sdk;
using Moq;
using System.Data.Common;

namespace NexusTalent.PluginSet.Tests.Builders
{
    public class PluginContextBuilder
    {
        public static Mock<IPluginExecutionContext> Build(
            string message = "Update",
            int stage = 20,
            Entity target = null)
        {
            var mock = new Mock<IPluginExecutionContext>();

            var inputs = new ParameterCollection();

            if (target != null)
                inputs["Target"] = target;

            mock.SetupGet(x => x.MessageName)
                .Returns(message);

            mock.SetupGet(x => x.Stage)
                .Returns(stage);

            mock.SetupGet(x => x.InputParameters)
                .Returns(inputs);

            mock.SetupGet(x => x.PreEntityImages)
                .Returns(new EntityImageCollection());

            return mock;
        }
    }
}
