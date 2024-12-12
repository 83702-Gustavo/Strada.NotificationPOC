using Moq;
using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Application.Services;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Tests.UnitTests;

public class NotificationServiceTests
{
    private readonly Mock<INotificationProvider> _mockSmsProvider;
    private readonly Mock<INotificationProvider> _mockEmailProvider;
    private readonly Mock<INotificationRepository> _mockRepository;
    private readonly NotificationService _service;

    public NotificationServiceTests()
    {
        _mockSmsProvider = new Mock<INotificationProvider>();
        _mockEmailProvider = new Mock<INotificationProvider>();
        _mockRepository = new Mock<INotificationRepository>();

        // Setup mock providers
        _mockSmsProvider
            .Setup(p => p.CanHandle(NotificationType.Sms))
            .Returns(true);
        _mockSmsProvider
            .Setup(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success());

        _mockEmailProvider
            .Setup(p => p.CanHandle(NotificationType.Email))
            .Returns(true);
        _mockEmailProvider
            .Setup(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success());

        // Inject mocks into the service
        _service = new NotificationService(
            new[] { _mockSmsProvider.Object, _mockEmailProvider.Object },
            _mockRepository.Object
        );
    }

    [Fact]
    public async Task SendNotificationAsync_ShouldUseSmsProvider_WhenTypeIsSms()
    {
        // Arrange
        var recipient = "+123456789";
        var message = "Test SMS notification";
        var type = NotificationType.Sms;

        // Act
        var result = await _service.SendNotificationAsync(type, recipient, message);

        // Assert
        Assert.True(result.IsSuccess);
        _mockSmsProvider.Verify(p => p.SendAsync(recipient, message), Times.Once);
        _mockEmailProvider.Verify(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SendNotificationAsync_ShouldUseEmailProvider_WhenTypeIsEmail()
    {
        // Arrange
        var recipient = "user@example.com";
        var message = "Test Email notification";
        var type = NotificationType.Email;

        // Act
        var result = await _service.SendNotificationAsync(type, recipient, message);

        // Assert
        Assert.True(result.IsSuccess);
        _mockEmailProvider.Verify(p => p.SendAsync(recipient, message), Times.Once);
        _mockSmsProvider.Verify(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SendNotificationAsync_ShouldReturnFailure_WhenNoProviderCanHandle()
    {
        // Arrange
        var recipient = "+123456789";
        var message = "Test Push notification";
        var type = NotificationType.Push; // No provider configured for Push notifications

        // Act
        var result = await _service.SendNotificationAsync(type, recipient, message);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("No provider found for notification type 'Push'.", result.ErrorMessage);
    }

    [Fact]
    public async Task SendNotificationAsync_ShouldSaveNotification_WhenSuccessful()
    {
        // Arrange
        var recipient = "user@example.com";
        var message = "Test Email notification";
        var type = NotificationType.Email;

        // Act
        await _service.SendNotificationAsync(type, recipient, message);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Notification>()), Times.Once);
    }

    [Fact]
    public async Task SendNotificationAsync_ShouldNotSaveNotification_WhenFailed()
    {
        // Arrange
        var recipient = "+123456789";
        var message = "Test SMS notification";
        var type = NotificationType.Sms;

        _mockSmsProvider
            .Setup(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Failure("Provider error"));

        // Act
        var result = await _service.SendNotificationAsync(type, recipient, message);

        // Assert
        Assert.False(result.IsSuccess);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Notification>()), Times.Never);
    }
}
