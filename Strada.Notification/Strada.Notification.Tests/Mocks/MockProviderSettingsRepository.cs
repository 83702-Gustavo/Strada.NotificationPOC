using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Interfaces;

public static class MockProviderSettingsRepository
{
    public static Mock<IProviderSettingsRepository> Create()
    {
        var mock = new Mock<IProviderSettingsRepository>();

        var fakeSettings = new List<ProviderSettings>
        {
            new ProviderSettings("Twilio", true, 1, "twilio-api-key"),
            new ProviderSettings("Zenvia", true, 2, "zenvia-api-key")
        };

        mock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(fakeSettings);

        mock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) => fakeSettings.FirstOrDefault(s => s.Name == name));

        return mock;
    }
}