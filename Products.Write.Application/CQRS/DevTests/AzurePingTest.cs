using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.CQRS.DevTests
{
    public class AzurePingTest : IRequest<AzurePingTestResult>
    {
        [Required]
        public string PingTestURI { get; init; } 

        public AzurePingTest(string pingTestURI)
        {
            PingTestURI = pingTestURI;
        }
    }
}
