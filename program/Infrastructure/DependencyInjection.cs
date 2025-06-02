using GosuslugiWinForms.Controllers;
using GosuslugiWinForms.Data;
using GosuslugiWinForms.Repositories;
using GosuslugiWinForms.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GosuslugiWinForms.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<GosuslugiContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=gosuslugi;Username=postgres;Password=P@ssw0rd")
                       .EnableSensitiveDataLogging()
                       .EnableDetailedErrors());
            services.AddScoped<UserRepository>();
            services.AddScoped<ApplicationRepository>();
            services.AddScoped<ServiceRepository>();
            services.AddScoped<RuleRepository>();
            services.AddScoped<ParameterTypeRepository>();
            services.AddScoped<ParameterRepository>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<UserService>();
            services.AddScoped<ApplicationService>();
            services.AddScoped<CivilServantService>();
            services.AddScoped<AdminService>();
            services.AddScoped<AuthenticationController>();
            services.AddScoped<UserController>();
            services.AddScoped<CivilServantController>();
            services.AddScoped<AdminController>();
            return services.BuildServiceProvider();
        }
    }
}