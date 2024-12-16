using Moq;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Tests.Mocks;

public static class MockNotificationRepository
{
    public static Mock<INotificationRepository> Create()
    {
        var mock = new Mock<INotificationRepository>();
        var fakeNotifications = new List<Domain.Entities.Notification>();

        mock.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Notification>()))
            .Callback((Domain.Entities.Notification notification) => fakeNotifications.Add(notification))
            .Returns(Task.CompletedTask);

        mock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(fakeNotifications);

        mock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => fakeNotifications.FirstOrDefault(n => n.Id == id));

        return mock;
    }
}