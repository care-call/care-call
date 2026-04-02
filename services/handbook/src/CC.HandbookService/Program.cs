using CC.Common.Logging;
using CC.HandbookService.Application;
using CC.HandbookService.Infrastructure;
using CC.HandbookService.Infrastructure.Handbook;
using CC.HandbookService.Api.Endpoints;
using CC.HandbookService.Application.Handbook;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();
builder.Logging.ClearProviders();
builder.Services.AddLogger(builder.Configuration);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAntiforgery();

app.MapGet("/api/v1/languages", async (DatabaseContext db) =>
{
    var languages = await db.Languages.ToListAsync();
    return Results.Ok(languages);
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

app.MapHandbookEndpoints();

app.Run();
