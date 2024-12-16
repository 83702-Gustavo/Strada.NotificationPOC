using System.Net;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Strada.Notification.Infrastructure.Providers;

namespace Strada.Notification.Tests.UnitTests.Providers;

public class PushProviderTests
{
    [Fact]
    public async Task SendAsync_ShouldSendPushNotificationSuccessfully()
    {
        // Arrange
        var settings = new PushSettings
        {
            ServiceUrl = "https://fcm.googleapis.com/fcm/send",
            ApiKey = "test-api-key"
        };

        var options = Mock.Of<IOptions<PushSettings>>(o => o.Value == settings);

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        var httpClient = new HttpClient(handlerMock.Object);

        var provider = new PushProvider(options, httpClient);

        // Act
        var result = await provider.SendAsync("recipient-token", "Test message");

        // Assert
        Assert.True(result.IsSuccess);
    }
}