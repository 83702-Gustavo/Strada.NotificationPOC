using Microsoft.Extensions.Logging;
using Moq;
using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;
using Strada.Notification.Infrastructure.Providers;

namespace Strada.Notification.Tests.UnitTests.Providers;

public class SmsProviderTests
{
    private readonly Mock<IProviderSettingsRepository> _mockSettingsRepository;
    private readonly Mock<ILogger<SmsProvider>> _mockLogger;
    private readonly Mock<ISmsProvider> _mockTwilioProvider;
    private readonly Mock<ISmsProvider> _mockZenviaProvider;
    private readonly SmsProvider _smsProvider;

    public SmsProviderTests()
    {
        _mockSettingsRepository = new Mock<IProviderSettingsRepository>();
        _mockLogger = new Mock<ILogger<SmsProvider>>();
        _mockTwilioProvider = new Mock<ISmsProvider>();
        _mockZenviaProvider = new Mock<ISmsProvider>();

        _mockTwilioProvider.Setup(p => p.Name).Returns("Twilio");
        _mockZenviaProvider.Setup(p => p.Name).Returns("Zenvia");

        _smsProvider = new SmsProvider(
            new[] { _mockTwilioProvider.Object, _mockZenviaProvider.Object },
            _mockSettingsRepository.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task SendAsync_ShouldUseHighestPriorityProvider_WhenProvidersAreAvailable()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<ProviderSettings>
        {
            new ProviderSettings("Twilio", true, 1, "twilio-api-key"),
            new ProviderSettings("Zenvia", true, 2, "zenvia-api-key")
        });

        _mockTwilioProvider
            .Setup(p => p.CanHandle(NotificationType.Sms))
            .Returns(true);

        _mockTwilioProvider
            .Setup(p => p.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _smsProvider.SendAsync("+1234567890", "Test SMS");

        // Assert
        Assert.True(result.IsSuccess);
        _mockTwilioProvider.Verify(p => p.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _mockZenviaProvider.Verify(p => p.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SendAsync_ShouldFallbackToNextProvider_WhenHighestPriorityFails()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<ProviderSettings>
        {
            new ProviderSettings("Twilio", true, 1, "twilio-api-key"),
            new ProviderSettings("Zenvia", true, 2, "zenvia-api-key")
        });

        _mockTwilioProvider
            .Setup(p => p.CanHandle(NotificationType.Sms))
            .Returns(true);

        _mockTwilioProvider
            .Setup(p => p.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Failure("Twilio failed."));

        _mockZenviaProvider
            .Setup(p => p.CanHandle(NotificationType.Sms))
            .Returns(true);

        _mockZenviaProvider
            .Setup(p => p.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _smsProvider.SendAsync("+1234567890", "Test SMS");

        // Assert
        Assert.True(result.IsSuccess);
        _mockTwilioProvider.Verify(p => p.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _mockZenviaProvider.Verify(p => p.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SendAsync_ShouldReturnFailure_WhenNoProvidersAreAvailable()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<ProviderSettings>());

        // Act
        var result = await _smsProvider.SendAsync("+1234567890", "Test SMS");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("No SMS providers are available.", result.ErrorMessage);
    }

    [Fact]
    public async Task SendAsync_ShouldReturnFailure_WhenAllProvidersFail()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<ProviderSettings>
        {
            new ProviderSettings("Twilio", true, 1, "twilio-api-key"),
            new ProviderSettings("Zenvia", true, 2, "zenvia-api-key")
        });

        _mockTwilioProvider
            .Setup(p => p.CanHandle(NotificationType.Sms))
            .Returns(true);

        _mockTwilioProvider
            .Setup(p => p.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Failure("Twilio failed."));

        _mockZenviaProvider
            .Setup(p => p.CanHandle(NotificationType.Sms))
            .Returns(true);

        _mockZenviaProvider
            .Setup(p => p.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Failure("Zenvia failed."));

        // Act
        var result = await _smsProvider.SendAsync("+1234567890", "Test SMS");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("All SMS providers failed.", result.ErrorMessage);
    }
}