# RealPage.Webhooks.AspNetCore

A set of .NET Standard packages for validating and authorizing webhook events (requests) issued from RealPage.

Please follow the steps

* **Usage in your project**
  * Add reference to the `RealPage.Webhooks.AspNetCore` package

```bash  
dotnet add package RealPage.Webhooks.AspNetCore
```

* Set `Webhooks:SecretKey` in appsettings.json (or `IConfiguration`)

``` json
  "Webhooks": {
    "SecretKey": "YOURSECRETVALUE"
  }
```

* Configure dependency injection

``` c#
using RealPage.Webhooks.AspNetCore.Extensions;

services.AddWebhooks(Configuration);
  // or
  //services
  //    .AddWebhooks(options =>
  //    {
  //        options.SecretKey = Configuration.GetValue<string>("ProvisioningSecret");
  //    });

app.UseWebhooks();
```

## Traditional APIs

Create an API method & decorate with `WebhookSubscription` attribute

```c#
using RealPage.Webhooks.AspNetCore.Filters;
using RealPage.Webhooks.Models;

[HttpPost]
[Route("api/provisioning")]
[AllowAnonymous]
[WebhookSubscription]
public IActionResult Post([FromBody] WebhookEvent<JToken> webhookEvent)
{
    // Process the event data.
    return Ok();
}
```

## Minimal APIs

If you're using .NET 7 or later, there's a really easy way to set up a webhook subscriber!

Simply map an endpoint that will subscribe to a webhook as follows:

```c#
app.MapPost("/v1/prospect-changed", (WebhookEvent<ProspectChange> eventBody) =>
{
    app.Logger.LogInformation("Processing a {topic} event for {prospect-id}", eventBody.Topic, eventBody.Payload.Id);
    // your real logic to handle the event would go here
    return Results.Ok(eventBody.Id);
}).RequireWebhooksSubsciption();
```

As an alternative, you can also apply the `WebhookSubscription` attribute if you
prefer that syntax:

```c#
app.MapPost("/v1/prospect-changed", [WebhookSubscription] (WebhookEvent<ProspectChange> eventBody) =>
{
    app.Logger.LogInformation("Processing a {topic} event for {prospect-id}", eventBody.Topic, eventBody.Payload.Id);
    // your real logic to handle the event would go here
    return Results.Ok(eventBody.Id);
});
```
