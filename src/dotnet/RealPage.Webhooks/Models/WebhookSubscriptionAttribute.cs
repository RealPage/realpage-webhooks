using System;

namespace RealPage.Webhooks.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class WebhookSubscriptionAttribute : Attribute
    {
        public WebhookSubscriptionAttribute()
        {
        }
    }
}
