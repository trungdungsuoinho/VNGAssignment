namespace VNGAssignment.Middlewares
{    public class XAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<XAuthMiddleware> _logger;

        public XAuthMiddleware(RequestDelegate next, ILogger<XAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("xAuth", out var xAuthToken) || string.IsNullOrEmpty(xAuthToken))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
