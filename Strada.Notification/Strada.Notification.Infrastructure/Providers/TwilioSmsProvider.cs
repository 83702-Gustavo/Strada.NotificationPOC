using Microsoft.Extensions.Configuration;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Strada.Notification.Infrastructure.Providers;

public class TwilioSmsProvider : INotificationProvider
{
    private readonly IConfiguration _configuration;

    public TwilioSmsProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool CanHandle(TipoNotificacao type) => type == TipoNotificacao.Sms;

    public async Task SendAsync(string recipient, string message)
    {
        TwilioClient.Init(_configuration["Twilio:AccountSID"], _configuration["Twilio:AuthToken"]);

        await MessageResource.CreateAsync(
            body: message,
            from: new Twilio.Types.PhoneNumber(_configuration["Twilio:FromNumber"]),
            to: new Twilio.Types.PhoneNumber(recipient)
        );
    }
}