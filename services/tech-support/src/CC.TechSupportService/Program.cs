using CC.TechSupportService.Infrastructure;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Http;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddWolverineHttp();

builder.Host.UseWolverine(opts =>
{
    opts.PersistMessagesWithPostgresql(builder.Configuration.GetConnectionString("DefaultConnection")!);
    opts.UseEntityFrameworkCoreTransactions();
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

app.MapWolverineEndpoints();
app.Run();