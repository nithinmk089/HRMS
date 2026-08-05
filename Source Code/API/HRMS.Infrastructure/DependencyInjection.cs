using HRMS.Application.Interfaces.Services;
using HRMS.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHostedService<NotificationQueueProcessor>();
            services.AddScoped<IDataImportService, DataImportService>();
            
            // Register Notification Channel Services
            services.AddScoped<IEmailNotificationService, EmailNotificationService>();
            services.AddScoped<ISignalRNotificationService, SignalRNotificationService>();
            services.AddScoped<ISmsNotificationService, SmsNotificationService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<ITeamsNotificationService, TeamsNotificationService>();

            return services;
        }
    }
}
