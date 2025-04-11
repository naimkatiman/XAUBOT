using Microsoft.Extensions.DependencyInjection;
using XaubotClone.Data;
using XaubotClone.Services;

namespace XaubotClone
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDataRepository, MemoryDataRepository>();
            services.AddScoped<IDataService, DataService>();
            
            return services;
        }
    }
}
