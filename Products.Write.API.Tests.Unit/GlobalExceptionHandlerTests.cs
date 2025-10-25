using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Products.Write.API.ExceptionHandling.ExceptionHandlers;
using Products.Write.API.ExceptionHandling.Exceptions;

namespace Products.Write.API
{
    public class GlobalExceptionHandlerTests
    {
        private readonly GlobalExceptionHandler _handler;
        private readonly Mock<ILogger<GlobalExceptionHandler>> _loggerMock;
        private readonly Mock<IProblemDetailsService> _problemDetailsServiceMock;
        private readonly DefaultHttpContext _httpContext;
        public GlobalExceptionHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
            _problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            var environment = new Mock<IWebHostEnvironment>();
            environment.Setup(e => e.EnvironmentName).Returns("Development");
            _handler = new GlobalExceptionHandler(
                _loggerMock.Object,
                environment.Object,
                _problemDetailsServiceMock.Object);
            _httpContext = new DefaultHttpContext();
            _httpContext.TraceIdentifier = "test-trace-id";
            _httpContext.Request.Path = "/test";
            _httpContext.Request.Method = "GET";
        }
        [Fact]
        public async Task TryHandleAsync_WithValidationException_ReturnsTrue()
        {
            // Arrange
            var exception = new ValidationException("Test validation error");
            _problemDetailsServiceMock
                .Setup(x => x.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
                .ReturnsAsync(true);
            // Act
            var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);
            // Assert
            Assert.True(result);
            Assert.Equal(400, _httpContext.Response.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test validation error")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }
        [Fact]
        public async Task TryHandleAsync_WithNotFoundException_Returns404()
        {
            // Arrange
            var exception = new NotFoundException("User", "123");
            _problemDetailsServiceMock
                .Setup(x => x.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
                .ReturnsAsync(true);
            // Act
            var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);
            // Assert
            Assert.True(result);
            Assert.Equal(404, _httpContext.Response.StatusCode);
        }
    }
}
