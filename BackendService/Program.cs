using BackendService.Data.DataSeed;
using CustomLibrary.Extensions;
using CustomLibrary.Helper;
using CustomLibrary.Middlewares;
using CustomLibrary.Settings;
using ManagementUserService;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Make Folder
var filePath = Guard.Against.NullOrWhiteSpace(builder.Configuration.GetSection(nameof(MediaSetting))["FilePath"], "FilePath");
var rootFilePath = Path.Combine(builder.Environment.ContentRootPath, filePath);
Directory.CreateDirectory(rootFilePath);

builder.Services.Configure<MediaSetting>(builder.Configuration.GetSection(nameof(MediaSetting)));

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services
    .AddCustomDbContext(builder.Configuration)
    .AddApplicationSettings(builder.Configuration)
    .AddRequiredService()
    .AddRepository()
    .AddCustomMvc()
    .AddCustomAuthorization()
    .AddMapster()
    .AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".HEIC"] = "image/heic";
provider.Mappings[".heif"] = "image/heif";
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, filePath)),
    RequestPath = "/Media",
    ContentTypeProvider = provider,
    //OnPrepareResponse = 
});

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true);
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowCredentials();
});

app.UseRouting();

app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

SeedData.Seed(app.Services);

app.Run();
