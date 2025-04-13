using ImobSys.Api.Common.Endpoints;
using ImobSys.Api.Infrastructure.Database;
using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Modules.Admin.Features.PropertiesTypes;

public static class GetPropertyTypeById
{
    public sealed record Response(Guid Id, string Name, string Description, Guid CustomerId);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet($"{AdminPaths.PropertiesTypes}/{{id:guid}}", HandlerAsync)
                .WithName(nameof(GetPropertyTypeById))
                .WithTags(nameof(PropertyType))
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }

        private static async Task<IResult> HandlerAsync(
            [FromRoute] Guid id,
            [FromServices] ImobSysDbContext db,
            CancellationToken cancellationToken = default)
        {
            var entity = await db.PropertiesTypes
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
