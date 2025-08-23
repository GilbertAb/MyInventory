using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace External.MyInventoryApi.CrossCutting.Middleware
{
    public class HeaderValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HeaderValidationMiddleware> _logger;

        private const string CorrelationHeader = "x-CorrelationId";

        public HeaderValidationMiddleware(RequestDelegate next, ILogger<HeaderValidationMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Get header if exists
                string? correlationId = context.Request.Headers[CorrelationHeader];

                // Generate if doesn't exist
                if (string.IsNullOrWhiteSpace(correlationId))
                {
                    correlationId = Guid.NewGuid().ToString();
                    
                    _logger.LogInformation("CorrelationId not provided, generated one: {CorrelationId}", correlationId);
                }

                // Add to client response
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(CorrelationHeader))
                    {
                        context.Response.Headers.Add(CorrelationHeader, correlationId);
                    }
                    return Task.CompletedTask;
                });

                _logger.LogInformation("Processing request with CorrelationId: {CorrelationId}", correlationId);

                await _next(context);
            } catch ( Exception ex)
            {
                _logger.LogError(ex, "Error validating request header");
            }
            
        }
    }
}
