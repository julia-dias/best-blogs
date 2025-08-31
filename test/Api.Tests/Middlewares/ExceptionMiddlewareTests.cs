using Api.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Service.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests.Middlewares
{
    public class ExceptionMiddlewareTests
    {
        public static TheoryData<Exception, int, string> ExceptionTestData =>
            new()
            {
                { new EntityNotFoundException("Entity not found"), 404, "Entity not found" },
                { new InvalidOperationException("Invalid operation"), 500, null },
                { new Exception("Unexpected error"), 500, null }
            };

        [Theory]
        [MemberData(nameof(ExceptionTestData))]
        public async Task EntityNotFoundException_Returns404(
            Exception exceptionToThrow,
            int expectedStatusCode,
            string expectedMessage)
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                next: (innerHttpContext) => throw exceptionToThrow,
                logger: NullLogger<ExceptionMiddleware>.Instance
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

            // Assert
            Assert.Equal(expectedStatusCode, context.Response.StatusCode);
            if (expectedStatusCode == 500)
            {
                Assert.True(string.IsNullOrWhiteSpace(responseBody));
            }
            else
            {
                var responseJson = JsonSerializer.Deserialize<JsonElement>(responseBody);
                Assert.Equal(expectedMessage, responseJson.GetProperty("Message").GetString());
            }
        }
    }
}
