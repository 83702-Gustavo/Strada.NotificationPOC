using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Application.Services;
using Strada.Notification.Domain.Interfaces;
using Strada.Notification.Infrastructure.Persistence;
using Strada.Notification.Infrastructure.Repositories;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly ServiceProvider ServiceProvider;

    public IntegrationTestBase()
    {
        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddDbContext<NotificationDbContext>(options =>
            options.UseInMemoryDatabase("IntegrationTests"));
        
        serviceCollection.AddScoped<INotificationRepository, NotificationRepository>();
        serviceCollection.AddScoped<IProviderSettingsRepository, ProviderSettingsRepository>();
        serviceCollection.AddScoped<INotificationService, NotificationService>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}