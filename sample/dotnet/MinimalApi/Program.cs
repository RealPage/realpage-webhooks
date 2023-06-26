using RealPage.Webhooks.AspNetCore.Extensions;
using RealPage.Webhooks.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddWebhooks(builder.Configuration);

var app = builder.Build();

app.UseHealthChecks("/health");
app.UseWebhooks();

app
    .MapPost("/v1/prospect-changed", (WebhookEvent<ProspectChange> eventBody) =>
    {
        app.Logger.LogInformation("Processing a {topic} event for {prospect-id}", eventBody.Topic, eventBody.Payload.Id);
        // your real logic to handle the event would go here
        return Results.Ok(eventBody.Id);
    })
    .RequireWebhooksSubsciption();

app.Run();

public record ProspectChange
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
