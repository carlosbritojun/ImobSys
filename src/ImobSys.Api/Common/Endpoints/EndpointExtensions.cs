﻿using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ImobSys.Api.Common.Endpoints;

public static class EndpointExtensions
{
    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(x => x.CustomSchemaIds(type => type.FullName!
            .Replace("ImobSys.Api.Modules.", "")
            .Replace("+", ".")
            .Replace("Features.", "")));

        return services;
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        ServiceDescriptor[] serviceDescriptors = typeof(EndpointExtensions).Assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}
