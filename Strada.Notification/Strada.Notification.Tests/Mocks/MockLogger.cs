using Microsoft.Extensions.Logging;
using Moq;

public static class MockLogger<T>
{
    public static Mock<ILogger<T>> Create()
    {
        return new Mock<ILogger<T>>();
    }
}