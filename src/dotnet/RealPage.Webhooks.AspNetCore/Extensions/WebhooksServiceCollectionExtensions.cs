using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealPage.Webhooks.Models;
using System;

namespace RealPage.Webhooks.AspNetCore.Extensions
{
    public static class WebhooksServiceCollectionExtensions
    {
        public static IServiceCollection AddWebhooks(this IServiceCollection services, Action<WebhookOptions> configureOptions)
        {
            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions), @"Webhook options are required.");

            services
                .AddOptions<WebhookOptions>()
                .Configure(configureOptions);

            return services;
        }

        public static IServiceCollection AddWebhooks(this IServiceCollection services, IConfiguration configuration)
        {
            services
                 .Configure<WebhookOptions>(configuration.GetSection("Webhooks"));

            return services;
        }
    }
}
