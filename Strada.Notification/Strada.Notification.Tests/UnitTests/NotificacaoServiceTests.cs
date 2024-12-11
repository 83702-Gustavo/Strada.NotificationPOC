using Moq;
using Strada.Notification.Application.Services;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Tests.UnitTests;

public class NotificacaoServiceTests
{
    private readonly Mock<INotificationRepository> _repositoryMock;
    private readonly Mock<INotificationProvider> _smsProviderMock;
    private readonly Mock<INotificationProvider> _emailProviderMock;
    private readonly Mock<INotificationProvider> _whatsappProviderMock;
    private readonly NotificacaoService _notificacaoService;

    public NotificacaoServiceTests()
    {
        _repositoryMock = new Mock<INotificationRepository>();

        _smsProviderMock = new Mock<INotificationProvider>();
        _smsProviderMock.Setup(p => p.CanHandle(TipoNotificacao.Sms)).Returns(true);
        _smsProviderMock.Setup(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.CompletedTask);

        _emailProviderMock = new Mock<INotificationProvider>();
        _emailProviderMock.Setup(p => p.CanHandle(TipoNotificacao.Email)).Returns(true);
        _emailProviderMock.Setup(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(Task.CompletedTask);

        _whatsappProviderMock = new Mock<INotificationProvider>();
        _whatsappProviderMock.Setup(p => p.CanHandle(TipoNotificacao.WhatsApp)).Returns(true);
        _whatsappProviderMock.Setup(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(Task.CompletedTask);

        // Serviço configurado com provedores mockados
        _notificacaoService = new NotificacaoService(
            new List<INotificationProvider>
            {
                _smsProviderMock.Object,
                _emailProviderMock.Object,
                _whatsappProviderMock.Object
            },
            _repositoryMock.Object
        );
    }

    [Fact]
    public async Task Should_Send_Sms_Notification_And_Save_To_Repository()
    {
        // Arrange
        var type = TipoNotificacao.Sms;
        var recipient = "+5511999999999";
        var message = "Hello from SMS!";

        // Act
        var result = await _notificacaoService.EnviaNotificacaoAsync(type, recipient, message);

        // Assert
        Assert.True(result.IsSuccess);
        _smsProviderMock.Verify(p => p.SendAsync(recipient, message), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task Should_Fail_If_No_Provider_Can_Handle_TipoNotificacao()
    {
        // Arrange
        var type = TipoNotificacao.Push; // Nenhum provedor registrado para Push
        var recipient = "+5511999999999";
        var message = "Hello from Push!";

        // Act
        var result = await _notificacaoService.EnviaNotificacaoAsync(type, recipient, message);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("No providers available for notification type 'Push'.", result.ErrorMessage);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Notificacao>()), Times.Never);
    }

    [Theory]
    [InlineData(null, "Hello from SMS!")]  // Destinatário nulo
    [InlineData("", "Hello from SMS!")]    // Destinatário vazio
    [InlineData("+5511999999999", null)]  // Mensagem nula
    [InlineData("+5511999999999", "")]    // Mensagem vazia
    public async Task Should_Fail_If_Input_Is_Invalid(string recipient, string message)
    {
        // Arrange
        var type = TipoNotificacao.Sms;

        // Act
        var result = await _notificacaoService.EnviaNotificacaoAsync(type, recipient, message);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("cannot be null or empty", result.ErrorMessage);
        _smsProviderMock.Verify(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Notificacao>()), Times.Never);
    }

    [Fact]
    public async Task Should_Try_All_Providers_If_First_Fails()
    {
        // Arrange
        var type = TipoNotificacao.WhatsApp;
        var recipient = "+5511999999999";
        var message = "Hello from WhatsApp!";

        // Simula falha no primeiro provedor
        _whatsappProviderMock.SetupSequence(p => p.SendAsync(It.IsAny<string>(), It.IsAny<string>()))
                             .Throws(new Exception("Simulated failure"))
                             .Returns(Task.CompletedTask);

        // Act
        var result = await _notificacaoService.EnviaNotificacaoAsync(type, recipient, message);

        // Assert
        Assert.True(result.IsSuccess);
        _whatsappProviderMock.Verify(p => p.SendAsync(recipient, message), Times.Exactly(2));
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Notificacao>()), Times.Once);
    }
}
