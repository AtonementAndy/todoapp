using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Jobs;
using TodoApp.Application.Models.Entities;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Infrastructure.DI
{
    public static class DependencyInjection
    {
        // Postgre
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

            services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
            services.AddScoped<ISmsService, ConsoleSmsService>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddIdentityCore<User>(options => 
            {
                // Password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                // User
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();

            // Quartz.NET
            services.AddQuartz(q =>
            {
                q.UsePersistentStore(s =>
                {
                    s.UsePostgres(configuration.GetConnectionString("DefaultConnString")!);
                });

                q.AddJob<TaskReminderJob>(options => options.WithIdentity("TaskReminderJob"));
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            // Something :)
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
