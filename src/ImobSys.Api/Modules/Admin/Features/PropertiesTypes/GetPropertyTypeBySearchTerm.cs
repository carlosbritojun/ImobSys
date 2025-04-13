using ImobSys.Api.Common.Endpoints;
using ImobSys.Api.Infrastructure.Database;
using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Modules.Admin.Features.PropertiesTypes;

public static class GetPropertyTypesBySearchTerm
{
    public sealed record Response(Guid Id, string Name, string Description);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(AdminPaths.PropertiesTypes, HandlerAsync)
                .WithName(nameof(GetPropertyTypesBySearchTerm))
                .WithTags(nameof(PropertyType))
                .Produces<IEnumerable<Response>>(StatusCodes.Status200OK);
        }

        private static async Task<IResult> HandlerAsync(
            [FromQuery] string? searchTerm,
            [FromServices] ImobSysDbContext db,
            CancellationToken cancellationToken = default)
        {
            var results = await db.PropertiesTypes
                .AsNoTracking()
                .Where(x => string.IsNullOrEmpty(searchTerm)
                    || EF.Functions.ILike(x.Name, $"%{searchTerm}%")
                    || EF.Functions.ILike(x.Description, $"%{searchTerm}%"))
                .Select(x => new Response(
                    x.Id,
                    x.Name,
                    x.Description))
                .ToListAsync(cancellationToken);

            return TypedResults.Ok(results);
        }
    }
}
