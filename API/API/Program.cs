using Amazon.Runtime;
using Amazon.S3;
using API.Configuration;
using API.Data;
using API.Shared.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddFastEndpoints();

var jwtSecret = builder.Configuration["Jwt:SecretKey"]
    ?? throw new InvalidOperationException("Jwt:SecretKey is missing in configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

var frontendOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:3000" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(frontendOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "Suvlet API";
        s.Version = "v1";
    };
});

builder.Services.Configure<CloudflareR2Options>(
    builder.Configuration.GetSection(CloudflareR2Options.SectionName));

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var r2Opts = builder.Configuration.GetSection(CloudflareR2Options.SectionName).Get<CloudflareR2Options>()
        ?? throw new InvalidOperationException("CloudflareR2 configuration is missing.");

    var credentials = new BasicAWSCredentials(r2Opts.AccessKey, r2Opts.SecretKey);
    var config = new AmazonS3Config
    {
        ServiceURL = r2Opts.Endpoint,
        ForcePathStyle = true
    };
    return new AmazonS3Client(credentials, config);
});

builder.Services.AddScoped<R2StorageService>();

var app = builder.Build();

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.UseOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Suvlet API Reference")
           .WithTheme(ScalarTheme.Moon)
           .WithOpenApiRoutePattern("/swagger/v1/swagger.json");
});

app.Run();