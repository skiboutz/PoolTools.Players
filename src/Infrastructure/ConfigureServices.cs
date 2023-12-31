﻿using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Infrastructure.Data;
using PoolTools.Player.Infrastructure.Data.Interceptors;
using PoolTools.Player.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = "";
        var connectionStringKey = configuration["AZURE_SQL_CONNECTION_STRING_KEY"];
        if (!string.IsNullOrWhiteSpace(connectionStringKey))
        {
            connectionString = configuration[connectionStringKey];
        }

        Guard.Against.Null(connectionString, message: "Connection string referenced by key 'AZURE_SQL_CONNECTION_STRING_KEY' not found.");
        
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
