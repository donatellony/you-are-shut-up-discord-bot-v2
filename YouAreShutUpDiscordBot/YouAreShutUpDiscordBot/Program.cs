var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configurationTestValue = builder.Configuration["TEST_VALUE"];
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/testConfiguration", (ILogger<Program> logger) =>
    {
        logger.LogInformation(configurationTestValue);
        return configurationTestValue;
    })
    .WithName("TestConfiguration")
    .WithOpenApi();

app.Run();