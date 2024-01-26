using Microsoft.Extensions.DependencyInjection;
using TestApp.Broker.Operations;
using TestApp.Domain.Adapters;

namespace TestApp.Broker
{
    public static class BrokerModuleDependency
    {
        public static void AddBrokerModule(this IServiceCollection services)
        {
            services.AddTransient<IPublishAdapter, PublishManager>();
        }
    }
}
