using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Strada.Notification.Infrastructure.Persistence;

namespace Strada.Notification.Tests.IntegrationTests.Controllers;

public class NotificationControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public NotificationControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<NotificationDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task SendNotification_ShouldReturnSuccess_WhenRequestIsValid()
    {
        // Arrange
        var requestContent = new StringContent(
            JsonSerializer.Serialize(new
            {
                recipient = "+1234567890",
                message = "Test notification",
                type = "SMS"
            }),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/notifications/send", requestContent);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }

    [Fact]
    public async Task SendNotification_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var requestContent = new StringContent(
            JsonSerializer.Serialize(new
            {
                recipient = "", // Invalid recipient
                message = "Test notification",
                type = "SMS"
            }),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/notifications/send", requestContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetNotifications_ShouldReturnList_WhenDataExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

        context.Notifications.Add(new Domain.Entities.Notification("+1234567890", "Test message", Domain.Enums.NotificationType.Sms));
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/notifications");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("+1234567890", responseBody);
    }
}