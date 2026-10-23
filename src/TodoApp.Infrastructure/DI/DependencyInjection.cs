using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TodoApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => 
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnString"),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorCodesToAdd: null
                        );
                    }
                )
            );

            services.AddEndpointsApiExplorer();

            //services.AddHsts(options =>
            //{
            //    options.MaxAge = TimeSpan.FromDays(365);
            //    options.IncludeSubDomains = true;
            //    options.Preload = true;
            //});

            return services;
        }
    }
}
