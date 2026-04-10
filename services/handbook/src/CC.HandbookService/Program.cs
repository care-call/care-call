using CC.Common.Logging;
using CC.HandbookService.Application;
using CC.HandbookService.Infrastructure;
using CC.HandbookService.Api.Endpoints;
using CC.HandbookService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Logging.ClearProviders();
builder.Services.AddLogger(builder.Configuration);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

await app.Services.ApplyMigrationsAsync();

app.MapHandbookEndpoints();

app.Run();
