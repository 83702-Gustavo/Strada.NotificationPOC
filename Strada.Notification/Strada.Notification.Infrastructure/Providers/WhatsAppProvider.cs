using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Strada.Notification.Infrastructure.Providers;

public class WhatsAppProvider : INotificationProvider
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromPhoneNumber;

    public WhatsAppProvider(string accountSid, string authToken, string fromPhoneNumber)
    {
        _accountSid = accountSid ?? throw new ArgumentNullException(nameof(accountSid));
        _authToken = authToken ?? throw new ArgumentNullException(nameof(authToken));
        _fromPhoneNumber = $"whatsapp:{fromPhoneNumber}" ?? throw new ArgumentNullException(nameof(fromPhoneNumber));
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.WhatsApp;
    }

    public async Task<Result> SendAsync(string recipient, string message)
    {
        try
        {
            TwilioClient.Init(_accountSid, _authToken);

            var messageResource = await MessageResource.CreateAsync(
                body: message,
                from: new Twilio.Types.PhoneNumber(_fromPhoneNumber),
                to: new Twilio.Types.PhoneNumber($"whatsapp:{recipient}")
            );

            if (messageResource.Status == MessageResource.StatusEnum.Failed ||
                messageResource.Status == MessageResource.StatusEnum.Undelivered)
            {
                return Result.Failure($"Failed to send WhatsApp message: {messageResource.ErrorMessage}");
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"An unexpected error occurred while sending WhatsApp message: {ex.Message}");
        }
    }
}