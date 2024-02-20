using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// If the app is started from Dapr, use the Dapr sidecar's HTTP port.
var apiHttpPort = Environment.GetEnvironmentVariable("APP_PORT");
if (!string.IsNullOrEmpty(apiHttpPort))
{
    builder.WebHost.UseUrls($"http://localhost:{apiHttpPort.Trim()}");
}

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
app.MapSubscribeHandler();

app.MapPost("/receive-hello", ([FromBody] HelloMessage hello) =>
    {
        app.Logger.LogInformation($"Received Hello message: {hello.Message}");
        return;
    })
    .WithName("receive-hello")
    .WithOpenApi();

app.Run();

public class HelloMessage
{
    public string Message { get; set; }
}