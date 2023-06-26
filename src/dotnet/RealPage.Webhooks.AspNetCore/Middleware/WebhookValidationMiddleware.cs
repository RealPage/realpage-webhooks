using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RealPage.Webhooks.Models;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RealPage.Webhooks.Providers;

namespace RealPage.Webhooks.AspNetCore.Middleware
{
    public class WebhookValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebhookValidationMiddleware> _logger;
        private readonly WebhookOptions _webhookOptions;

        public WebhookValidationMiddleware(RequestDelegate next, 
            ILogger<WebhookValidationMiddleware> logger,
            IOptions<WebhookOptions> webhookOptions)
        {
            _next = next;
            _logger = logger;
            _webhookOptions = webhookOptions.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var metadata = endpoint?.Metadata.GetMetadata<WebhookSubscriptionAttribute>();
            if(metadata == null)
            {
                // skiping webhooks validation logic, as endpoint is not enabled for webhook subscription.
                await _next.Invoke(context);
                return;
            }

            var request = context.Request;
            var headers = request.Headers;
            var signature = headers["signature"];
            var clientId = headers["clientid"];

            if (string.IsNullOrWhiteSpace(signature))
            {
                _logger.LogError("Unauthorized access - no signature");
                await ThrowUnauthorized(context.Response, "Unauthorized access");
                return;
            }
            if (string.IsNullOrWhiteSpace(_webhookOptions.SecretKey))
            {
                _logger.LogError("Unauthorized - Missing secret key in configuration.");
                await ThrowUnauthorized(context.Response, "Missing secret key");
                return;
            }

            _logger.LogTrace("Validating signature {clientid}", clientId);
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8,
                   detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var hash = HashProvider.GenerateHash(_webhookOptions.SecretKey, body);
                if (!string.Equals(signature, hash, StringComparison.CurrentCultureIgnoreCase))
                {
                    _logger.LogError("Invalid signature {signature} {hash}", signature, hash);
                    await ThrowUnauthorized(context.Response, "Invalid signature");
                    return;
                }
            }
            _logger.LogDebug("Signature validated {clientid}", clientId);
            await _next.Invoke(context);
        }

        private async Task ThrowUnauthorized(HttpResponse response, string message)
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await response.WriteAsync(message);
        }
    }
}
