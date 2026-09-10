using RealTimeOpsPortal.Api.Extensions;
using RealTimeOpsPortal.Api.Hubs;
using RealTimeOpsPortal.Api.Middleware;
using RealTimeOpsPortal.Application;
using RealTimeOpsPortal.Infrastructure;
using RealTimeOpsPortal.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Core services
builder.Services.AddControllers();

// Application layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Authentication / Authorization
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddCurrentUserContext();

builder.Services.AddPortalAuthorization();

builder.Services.AddPortalSignalR();

// Swagger
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();

    await using var scope = app.Services.CreateAsyncScope();

    var seeder =
        scope.ServiceProvider
            .GetRequiredService<DevelopmentDataSeeder>();

    await seeder.SeedAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<CurrentUserMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.MapHub<OperationsHub>("/hubs/operations");

app.Run();