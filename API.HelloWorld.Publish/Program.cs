using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use Cloud Events
app.UseCloudEvents();

app.MapPost("/publish-hello", async (DaprClient daprClient, [FromBody] HelloMessage message) =>
    {
        app.Logger.LogInformation($"Publishing Hello message: {message.Message}");
        await daprClient.PublishEventAsync("rabbitmq", "hellotopic", message);
        app.Logger.LogInformation($"Publish successful");
        return message;
    })
    .WithName("publish-hello")
    .WithOpenApi();

app.Run();

public class HelloMessage
{
    public string Message { get; set; }
}