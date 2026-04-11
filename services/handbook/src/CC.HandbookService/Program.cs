using CC.Common.Logging;
using CC.HandbookService.Application;
using CC.HandbookService.Infrastructure;
using CC.HandbookService.Api.Endpoints;
using CC.HandbookService.Infrastructure.Persistence;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Logging.ClearProviders();
builder.Services.AddLogger(builder.Configuration);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Host.UseWolverine(opts =>
{
    opts.Discovery.CustomizeHandlerDiscovery(t => t.Includes.WithNameSuffix("UseCase"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.Services.ApplyMigrationsAsync();

app.MapHandbookEndpoints();

app.Run();
