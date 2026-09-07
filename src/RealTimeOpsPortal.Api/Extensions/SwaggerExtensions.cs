using Microsoft.OpenApi;

namespace RealTimeOpsPortal.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Real-Time Operations Portal API",
                    Version = "v1",
                    Description = "API for the Real-Time Operations Portal."
                });

            options.AddSecurityDefinition(
                "bearer",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

            options.AddSecurityRequirement(document =>
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecuritySchemeReference(
                            "bearer",
                            document)
                    ] = []
                });
        });

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(
        this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                "Real-Time Operations Portal API v1");

            options.RoutePrefix = "swagger";
        });

        return app;
    }
}