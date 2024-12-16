using Microsoft.Extensions.Options;
using Moq;
using Strada.Notification.Infrastructure.Providers;

namespace Strada.Notification.Tests.UnitTests.Providers;

public class EmailProviderTests
{
    [Fact]
    public async Task SendAsync_ShouldSendEmailSuccessfully()
    {
        // Arrange
        var settings = new EmailSettings
        {
            SmtpHost = "smtp.gmail.com",
            SmtpPort = 587,
            SmtpUsername = "test@gmail.com",
            SmtpPassword = "password"
        };

        var options = Mock.Of<IOptions<EmailSettings>>(o => o.Value == settings);
        var provider = new EmailProvider(options);

        // Act
        var result = await provider.SendAsync("recipient@gmail.com", "Test email message");

        // Assert
        Assert.True(result.IsSuccess);
    }
}