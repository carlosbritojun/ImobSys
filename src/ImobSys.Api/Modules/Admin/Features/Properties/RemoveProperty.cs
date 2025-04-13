using ImobSys.Api.Common.Endpoints;
using ImobSys.Api.Infrastructure.Database;
using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Modules.Admin.Features.Properties;

public static class RemoveProperty
{
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete($"{AdminPaths.Properties}/{{id:guid}}", HandlerAsync)
                .WithName(nameof(RemoveProperty))
                .WithTags(nameof(Property))
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);
        }

        private static async Task<IResult> HandlerAsync(
            [FromRoute] Guid id,
            [FromServices] ImobSysDbContext db,
            CancellationToken cancellationToken = default)
        {
            var entity = await db.Properties.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            db.Properties.Remove(entity);
            await db.SaveChangesAsync(cancellationToken);

            return TypedResults.NoContent();
        }
    }
}