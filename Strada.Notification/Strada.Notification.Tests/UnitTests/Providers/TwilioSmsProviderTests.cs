using Moq;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Interfaces;
using Strada.Notification.Infrastructure.Providers;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace Strada.Notification.Tests.UnitTests.Providers;

public class TwilioSmsProviderTests
{
    private readonly Mock<IProviderSettingsRepository> _mockSettingsRepository;
    private readonly TwilioSmsProvider _twilioProvider;

    public TwilioSmsProviderTests()
    {
        _mockSettingsRepository = new Mock<IProviderSettingsRepository>();
        _twilioProvider = new TwilioSmsProvider(_mockSettingsRepository.Object);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldReturnSuccess_WhenMessageIsSentSuccessfully()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetByNameAsync("Twilio"))
            .ReturnsAsync(new ProviderSettings("Twilio", true, 1, "valid-auth-token"));

        var mockMessageResource = new Mock<MessageResource>();
        mockMessageResource.Setup(m => m.Status).Returns(MessageResource.StatusEnum.Delivered);

        // Simula o envio bem-sucedido da mensagem
        TwilioClient.Init("valid-auth-token", "valid-auth-token");

        // Act
        var result = await _twilioProvider.SendSmsAsync("+1234567890", "Test SMS");

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldReturnFailure_WhenProviderIsDisabled()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetByNameAsync("Twilio"))
            .ReturnsAsync(new ProviderSettings("Twilio", false, 1, "valid-auth-token"));

        // Act
        var result = await _twilioProvider.SendSmsAsync("+1234567890", "Test SMS");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Twilio provider is not enabled or configured.", result.ErrorMessage);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldReturnFailure_WhenApiKeyIsMissing()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetByNameAsync("Twilio"))
            .ReturnsAsync(new ProviderSettings("Twilio", true, 1, null));

        // Act
        var result = await _twilioProvider.SendSmsAsync("+1234567890", "Test SMS");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Twilio API key is missing.", result.ErrorMessage);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldReturnFailure_WhenTwilioThrowsException()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetByNameAsync("Twilio"))
            .ReturnsAsync(new ProviderSettings("Twilio", true, 1, "valid-auth-token"));

        // Simula uma exceção lançada pela API Twilio
        TwilioClient.Init("valid-auth-token", "valid-auth-token");
        var exception = new ApiException("Twilio API failed.");

        // Act
        var result = await _twilioProvider.SendSmsAsync("+1234567890", "Test SMS");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Twilio exception:", result.ErrorMessage);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldReturnFailure_WhenMessageFailsToDeliver()
    {
        // Arrange
        _mockSettingsRepository.Setup(repo => repo.GetByNameAsync("Twilio"))
            .ReturnsAsync(new ProviderSettings("Twilio", true, 1, "valid-auth-token"));

        var mockMessageResource = new Mock<MessageResource>();
        mockMessageResource.Setup(m => m.Status).Returns(MessageResource.StatusEnum.Failed);
        mockMessageResource.Setup(m => m.ErrorMessage).Returns("Failed to deliver.");

        TwilioClient.Init("valid-auth-token", "valid-auth-token");

        // Act
        var result = await _twilioProvider.SendSmsAsync("+1234567890", "Test SMS");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Failed to deliver.", result.ErrorMessage);
    }
}