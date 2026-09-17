using API.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "Suvlet API";
        s.Version = "v1";
    };
});

var app = builder.Build();

app.UseFastEndpoints();
app.UseOpenApi();

app.MapScalarApiReference(options =>
{
    options.WithTitle("Suvlet API Reference")
           .WithTheme(ScalarTheme.Moon)
           .WithOpenApiRoutePattern("/swagger/v1/swagger.json");
});
app.Run();