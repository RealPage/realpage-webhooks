using Microsoft.AspNetCore.Builder;
using RealPage.Webhooks.AspNetCore.Middleware;

namespace RealPage.Webhooks.AspNetCore.Extensions
{
    public static class WebhooksApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWebhooks(this IApplicationBuilder app)
        {
            app.UseMiddleware<WebhookValidationMiddleware>();
            return app;
        }
    }
}
