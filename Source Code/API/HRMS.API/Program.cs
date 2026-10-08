using System.Text;
using FluentValidation;
using HRMS.Application.Validators;
using HRMS.Infrastructure;
using HRMS.Infrastructure.Hubs;
using HRMS.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateTenantRequestValidator>();

// Register Custom Persistence and Infrastructure
builder.Services.AddPersistence();
builder.Services.AddInfrastructure();

// JWT Authentication
var jwtKey = Environment.GetEnvironmentVariable("HRMS_JWT_KEY") 
    ?? builder.Configuration["Jwt:Key"] 
    ?? "HRMSDevSecretKey_MustBe32CharactersLong!!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "HRMS.API";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "HRMS.Client";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    // Allow JWT Token passed via SignalR query string for websockets
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("http://localhost:4200", "https://localhost:4200", "http://127.0.0.1:4200", "https://127.0.0.1:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Global Exception & Security Handling Middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (UnauthorizedAccessException ex)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";
        var res = HRMS.Application.DTOs.ApiResponse<object>.FailureResult(ex.Message);
        await context.Response.WriteAsJsonAsync(res);
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Unhandled exception processing request {Path}", context.Request.Path);
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        var msg = app.Environment.IsDevelopment() ? ex.Message : "An unexpected server error occurred. Please contact system support.";
        var res = HRMS.Application.DTOs.ApiResponse<object>.FailureResult(msg);
        await context.Response.WriteAsJsonAsync(res);
    }
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => Results.Redirect("/scalar/v1"));
app.MapGet("/swagger", () => Results.Redirect("/scalar/v1"));
app.MapGet("/scalar", () => Results.Redirect("/scalar/v1"));

app.UseCors("AllowAngularApp");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();

public partial class Program { }
