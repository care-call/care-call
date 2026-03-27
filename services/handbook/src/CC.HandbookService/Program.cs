using CC.HandbookService.Application.Import;
using CC.HandbookService.Infastructure;
using CC.HandbookService.Validate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<ValidateCsvFile>();
builder.Services.AddSingleton<HandbookInMemoryStore>();
builder.Services.AddScoped<ICsvImportService, ImportCsvFile>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "Handbook API v1"); });
}

var MapGroup = app.MapGroup("api/import").WithTags("ImportCsvFiles");

MapGroup.MapPost("/{handbook}", async (
    string handbook,
    IFormFile file,
    ICsvImportService csvImportService,
    CancellationToken token) =>
{
    if (file.Length == 0)
    {
        return Results.BadRequest(new { Message = "File is empty" });
    }

    if (!Enum.TryParse<HandbookType>(handbook, true, out var parsedHandbookType))
    {
        return Results.BadRequest(new
        {
            Message = "Unknown handbook type. Allowed values: language, age, problems"
        });
    }

    await using var stream = file.OpenReadStream();
    var result = await csvImportService.ImportAsyncFile(stream, parsedHandbookType, token);
    return Results.Ok(result);
}).DisableAntiforgery();

app.Run();