using ImobSys.Api.Common.Endpoints;
using ImobSys.Api.Infrastructure.Database;
using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Modules.Admin.Features.Properties;

public static class GetPropertyBySearchTerm
{
    public sealed record Response(Guid Id, string Name, string Description, string TypeName);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(AdminPaths.Properties, HandlerAsync)
                .WithName(nameof(GetPropertyBySearchTerm))
                .WithTags(nameof(Property))
                .Produces<IEnumerable<Response>>(StatusCodes.Status200OK);
        }

        private static async Task<IResult> HandlerAsync(
            [FromQuery] string? searchTerm,
            [FromServices] ImobSysDbContext db,
            CancellationToken cancellationToken = default)
        {
            var results = await db.Properties
                .AsNoTracking()
                .Where(x => string.IsNullOrEmpty(searchTerm)
                    || EF.Functions.ILike(x.Name, $"%{searchTerm}%")
                    || EF.Functions.ILike(x.Description, $"%{searchTerm}%"))
                .Select(x => new Response(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Type.Name))
                .ToListAsync(cancellationToken);

            return TypedResults.Ok(results);
        }
    }
}
