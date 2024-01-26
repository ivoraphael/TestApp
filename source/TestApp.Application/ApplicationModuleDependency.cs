using Microsoft.Extensions.DependencyInjection;
using TestApp.Application.Anticorruption;
using TestApp.Application.Services;
using TestApp.Domain.Interfaces.Adapters;
using TestApp.Domain.Services;

namespace TestApp.Application
{
    public static class ApplicationModuleDependency
    {
        public static void AddApplicationModule(this IServiceCollection services)
        {
            services.AddTransient<IExternalClientAdapter, ExternalClientAdapter>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IExternalClientMessageService, ExternalClientMessageService>();            
        }
    }
}
