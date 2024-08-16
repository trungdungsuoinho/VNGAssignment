using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNGAssignment.Middlewares;

namespace VNGAssignment.Tests
{
    public class XAuthMiddlewareTests
    {
        [Fact]
        public async Task XAuthMiddleware_ReturnUnauthorized()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers["xAuth"] = "";

            var middleware = new XAuthMiddleware(next: (innerHttpContext) =>
            {
                return Task.CompletedTask;
            }, logger: Mock.Of<ILogger<XAuthMiddleware>>());

            await middleware.InvokeAsync(context);

            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        }

        [Fact]
        public async Task XAuthMiddleware_PassAuthorized()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers["xAuth"] = "valid_token";

            var middleware = new XAuthMiddleware(next: (innerHttpContext) =>
            {
                return Task.CompletedTask;
            }, logger: Mock.Of<ILogger<XAuthMiddleware>>());

            await middleware.InvokeAsync(context);

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }

}
