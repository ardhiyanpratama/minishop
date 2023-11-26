using BackendService.Data.DataSeed;
using CustomLibrary.Extensions;
using CustomLibrary.Middlewares;
using ManagementUserService;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddCustomDbContext(builder.Configuration)
    .AddApplicationSettings(builder.Configuration)
    .AddRequiredService()
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
