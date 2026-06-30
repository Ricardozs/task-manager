using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Middleware;
using TaskManager.Application;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
if (corsOrigins is { Length: > 0 })
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AngularDev", policy =>
        {
            policy.WithOrigins(corsOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (app.Environment.IsDevelopment() &&
        builder.Configuration.GetValue("SeedDemoData", false))
    {
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var seeded = await DemoDataSeeder.SeedAsync(db, passwordHasher);
        if (seeded)
        {
            app.Logger.LogInformation(
                "Demo data seeded: {Email} with {TaskCount} sample tasks.",
                DemoDataSeeder.DemoEmail,
                DemoDataSeeder.DemoTaskCount);
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.yaml", "Task Manager API v1");
        options.RoutePrefix = "swagger";
    });
}

app.MapGet("/openapi/v1.yaml", () =>
{
    var path = ResolveOpenApiPath(app.Environment);
    if (path is null)
    {
        return Results.NotFound();
    }

    return Results.File(path, "application/yaml");
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (corsOrigins is { Length: > 0 })
{
    app.UseCors("AngularDev");
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static string? ResolveOpenApiPath(IWebHostEnvironment environment)
{
    string[] candidates =
    [
        Path.Combine(AppContext.BaseDirectory, "openapi.yaml"),
        Path.Combine(environment.ContentRootPath, "openapi.yaml"),
    ];

    return candidates.FirstOrDefault(File.Exists);
}
