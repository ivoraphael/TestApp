using Microsoft.Extensions.DependencyInjection;
using TestApp.Data.Repositories;
using TestApp.Domain.Interfaces.Repositories;

namespace TestApp.Data.Data
{
    public static class DataModuleDependency
    {
        public static void AddDataModule(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExternalClientMessageRepository, ExternalClientMessageRepository>();
        }
    }
}
