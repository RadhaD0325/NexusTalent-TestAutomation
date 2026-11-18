
using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace NexusTalent.PluginSet.Tests.Fakes
{
    public class TestServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services =
            new Dictionary<Type, object>();

        public List<Type> RequestedServices { get; } =
            new List<Type>();

        public TestServiceProvider(
            IPluginExecutionContext context,
            ITracingService tracing)
        {
            _services[typeof(IPluginExecutionContext)] = context;
            _services[typeof(ITracingService)] = tracing;
        }

        public object GetService(Type serviceType)
        {
            RequestedServices.Add(serviceType);

            _services.TryGetValue(serviceType, out var service);

            return service;
        }
    }
}
