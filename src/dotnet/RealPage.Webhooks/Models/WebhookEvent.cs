using System;

namespace RealPage.Webhooks.Models
{
    public class WebhookEvent<T>
    {
        public string Id { get; set; }

        public string Topic { get; set; }

        public DateTime CreatedAt { get; set; }

        public T Payload { get; set; }
    }
}
