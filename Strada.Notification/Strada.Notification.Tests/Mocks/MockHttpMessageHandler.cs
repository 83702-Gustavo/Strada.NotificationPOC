using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _sendFunc;

    public MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> sendFunc)
    {
        _sendFunc = sendFunc ?? throw new ArgumentNullException(nameof(sendFunc));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_sendFunc(request));
    }
}