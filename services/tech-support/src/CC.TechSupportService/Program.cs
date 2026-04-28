using CC.TechSupportService.API.Endpoints.Tickets;
using CC.TechSupportService.Infrastructure;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Host.UseWolverine(opts =>
{
    opts.Discovery.CustomizeHandlerDiscovery(t => t.Includes.WithNameSuffix("UseCase"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();
app.MapTicketEndpoints();

app.Run();