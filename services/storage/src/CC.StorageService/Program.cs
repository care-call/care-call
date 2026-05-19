using Amazon.S3;
using CC.ServiceDefaults;
using CC.StorageService.Features;
using CC.StorageService.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<Db>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var s3Settings = builder.Configuration.GetSection("S3");
var s3Endpoint = s3Settings["Endpoint"] ?? "http://localhost:3900";
var accessKey = s3Settings["AccessKey"] ?? "minioadmin";
var secretKey = s3Settings["SecretKey"] ?? "minioadmin";
var bucketName = s3Settings["Bucket"] ?? "care-call-storage";

var s3Config = new AmazonS3Config
{
    ServiceURL = s3Endpoint,
    ForcePathStyle = true,
    UseHttp = true,
    AuthenticationRegion = "us-east-1"
};

builder.Services.AddSingleton<IAmazonS3>(sp =>
    new AmazonS3Client(accessKey, secretKey, s3Config));

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