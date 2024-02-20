var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDaprClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapSubscribeHandler();
app.UseHttpsRedirection();

app.MapGet("/hello-world", () =>
    {
        const string message = "Hello World from Dapr";
        return message;
    })
    .WithName("Hello World")
    .WithOpenApi();

app.Run();