using ImobSys.Api.Common.Endpoints;
using ImobSys.Api.Infrastructure.Database;
using FluentValidation;
using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Modules.Admin.Features.Properties;

public static class GetPropertyById
{
    public sealed record Response(Guid Id, string Name, string Description, Guid TypeId);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet($"{AdminPaths.Properties}/{{id:guid}}", HandlerAsync)
                .WithName(nameof(GetPropertyById))
                .WithTags(nameof(Property))
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }

        private static async Task<IResult> HandlerAsync(
            [FromRoute] Guid id,
            [FromServices] ImobSysDbContext db,
            CancellationToken cancellationToken = default)
        {
            var entity = await db.Properties
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(entity);
        }
    }
}
