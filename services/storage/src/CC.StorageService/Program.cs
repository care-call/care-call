using System.Net;
using System.Net.Sockets;
using Amazon.S3;
using CC.ServiceDefaults;
using CC.StorageService.Features;
using CC.StorageService.Persistence;
using CC.StorageService.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<Db>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔧 Настройка S3 клиента
var s3Endpoint = builder.Configuration["S3_ENDPOINT"] ?? "http://localhost:3900";
var accessKey = builder.Configuration["S3_ACCESS_KEY"] ?? "dev-key";
var secretKey = builder.Configuration["S3_SECRET_KEY"] ?? "devsecretkey123";
var bucketName = builder.Configuration["S3_BUCKET"] ?? "care-call-storage";

var s3Config = new AmazonS3Config
{
    ServiceURL = s3Endpoint,
    ForcePathStyle = true,
    UseHttp = true,
    AuthenticationRegion = "us-east-1"
};

builder.Services.AddSingleton<IAmazonS3>(sp =>
    new AmazonS3Client(accessKey, secretKey, s3Config));

builder.Services.AddScoped<IStorageService, S3StorageService>();

builder.Services.AddHttpClient();

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
app.UseAuthorization();

UploadFileFeature.MapEndpoint(app);
DownloadFileFeature.MapEndpoint(app);
LinkFilesFeature.MapEndpoint(app);
GetMetadataFeature.MapEndpoint(app);
GetEntityFilesFeature.MapEndpoint(app);
DeleteFileFeature.MapEndpoint(app);

using (var scope = app.Services.CreateScope())
{
    var s3 = scope.ServiceProvider.GetRequiredService<IAmazonS3>();
    if (!await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3, bucketName))
    {
        await s3.PutBucketAsync(bucketName);
    }
    var dbContext = scope.ServiceProvider.GetRequiredService<Db>();
    await dbContext.Database.MigrateAsync();
}



app.Run();