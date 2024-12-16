using Moq;
using Strada.Notification.Application.DTOs;
using Strada.Notification.Application.Services;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Tests.UnitTests.Services;

public class SettingsServiceTests
{
    private readonly Mock<IProviderSettingsRepository> _mockRepository;
    private readonly SettingsService _service;

    public SettingsServiceTests()
    {
        _mockRepository = new Mock<IProviderSettingsRepository>();
        _service = new SettingsService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllSettingsAsync_ShouldReturnAllProviderSettings()
    {
        // Arrange
        var settings = new List<ProviderSettings>
        {
            new ProviderSettings("Twilio", true, 1, "api-key-1"),
            new ProviderSettings("Zenvia", true, 2, "api-key-2")
        };

        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(settings);

        // Act
        var result = await _service.GetAllSettingsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.Name == "Twilio");
        Assert.Contains(result, s => s.Name == "Zenvia");
    }

    [Fact]
    public async Task UpdateProviderSettingsAsync_ShouldUpdateSettingsSuccessfully()
    {
        // Arrange
        var updateDto = new UpdateProviderSettingsDto
        {
            Name = "Twilio",
            Enabled = false,
            Priority = 2,
            ApiKey = "new-api-key"
        };

        _mockRepository.Setup(repo => repo.GetByNameAsync("Twilio"))
            .ReturnsAsync(new ProviderSettings("Twilio", true, 1, "old-api-key"));

        // Act
        var result = await _service.UpdateProviderSettingsAsync(updateDto);

        // Assert
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ProviderSettings>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProviderSettingsAsync_ShouldReturnError_WhenProviderNotFound()
    {
        // Arrange
        var updateDto = new UpdateProviderSettingsDto
        {
            Name = "UnknownProvider",
            Enabled = true,
            Priority = 1,
            ApiKey = "api-key"
        };

        _mockRepository.Setup(repo => repo.GetByNameAsync("UnknownProvider"))
            .ReturnsAsync((ProviderSettings)null);

        // Act
        var result = await _service.UpdateProviderSettingsAsync(updateDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Provider not found.", result.ErrorMessage);
    }
}