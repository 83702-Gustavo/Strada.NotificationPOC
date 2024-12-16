using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Interfaces;
using Strada.Notification.Infrastructure.Providers;
using Xunit;

public class ZenviaSmsProviderTests
{
    private readonly Mock<IProviderSettingsRepository> _mockSettingsRepository;
    private readonly Mock<ILogger<ZenviaSmsProvider>> _mockLogger;

    public ZenviaSmsProviderTests()
    {
        _mockSettingsRepository = new Mock<IProviderSettingsRepository>();
        _mockLogger = new Mock<ILogger<ZenviaSmsProvider>>();
    }

    [Fact]
    public async Task SendSmsAsync_ShouldReturnSuccess_WhenMessageIsSentSuccessfully()
    {
        // Arrange
        var handler = new MockHttpMessageHandler(request =>
        {
            // Simula uma resposta de sucesso da API Zenvia
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new { message = "SMS sent successfully" }))
            };
        });

        var httpClient = new HttpClient(handler);

        _mockSettingsRepository.Setup(repo => repo.GetByNameAsync("Zenvia"))
            .ReturnsAsync(new ProviderSettings("Zenvia", true, 1, "zenvia-api-key"));

        var zenviaProvider = new ZenviaSmsProvider(_mockSettingsRepository.Object, httpClient, _mockLogger.Object);

        // Act
        var result = await zenviaProvider.SendSmsAsync("+1234567890", "Test SMS");

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldReturnFailure_WhenApiRespondsWithError()
    {
        // Arrange
        var handler = new MockHttpMessageHandler(request =>
        {
            // Simula uma resposta de erro da API Zenvia
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonSerializer.Serialize(new { message = "Invalid recipient" }))
            };
        });

        var httpClient = new HttpClient(handler);

        _mockSettingsRepository.Setup(repo => repo.GetByNameAsync("Zenvia"))
            .ReturnsAsync(new ProviderSettings("Zenvia", true, 1, "zenvia-api-key"));

        var zenviaProvider = new ZenviaSmsProvider(_mockSettingsRepository.Object, httpClient, _mockLogger.Object);

        // Act
        var result = await zenviaProvider.SendSmsAsync("+1234567890", "Test SMS");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Zenvia API error", result.ErrorMessage);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldReturnFailure_WhenProviderIsDisabled()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetByNameAsync("Zenvia"))
            .ReturnsAsync(new ProviderSettings("Zenvia", false, 1, "zenvia-api-key"));

        var httpClient = new HttpClient(new MockHttpMessageHandler(request =>
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }));

        var zenviaProvider = new ZenviaSmsProvider(_mockSettingsRepository.Object, httpClient, _mockLogger.Object);

        // Act
        var result = await zenviaProvider.SendSmsAsync("+1234567890", "Test SMS");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Zenvia provider is not enabled or configured.", result.ErrorMessage);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldReturnFailure_WhenApiKeyIsMissing()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetByNameAsync("Zenvia"))
            .ReturnsAsync(new ProviderSettings("Zenvia", true, 1, null));

        var httpClient = new HttpClient(new MockHttpMessageHandler(request =>
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }));

        var zenviaProvider = new ZenviaSmsProvider(_mockSettingsRepository.Object, httpClient, _mockLogger.Object);

        // Act
        var result = await zenviaProvider.SendSmsAsync("+1234567890", "Test SMS");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Zenvia API key is missing.", result.ErrorMessage);
    }
}
