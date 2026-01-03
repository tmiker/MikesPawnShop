using MediatR;

namespace Products.Write.Application.CQRS.DevTests
{
    public class AzurePingTestHandler : IRequestHandler<AzurePingTest, AzurePingTestResult>
    {
        public async Task<AzurePingTestResult> Handle(AzurePingTest test, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, test.PingTestURI);
            var result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode) return new AzurePingTestResult(true, null);
            return new AzurePingTestResult(false, "Azure Storage not available. Verify network firewall settings allow current IP address.");
        }
    }
}
