using Microsoft.AspNetCore.Builder;
using RealPage.Webhooks.Models;
using System;

namespace RealPage.Webhooks.AspNetCore.Extensions
{
    public static class WebhookSubsciptionEndpointConventionBuilderExtensions
    {
        public static TBuilder RequireWebhooksSubsciption<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Add(endpointBuilder =>
            {
                endpointBuilder.Metadata.Add(new WebhookSubscriptionAttribute());
            });
            return builder;
        }
    }
}
