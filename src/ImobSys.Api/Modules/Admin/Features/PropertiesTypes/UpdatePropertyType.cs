using FluentValidation;
using ImobSys.Api.Common.Endpoints;
using ImobSys.Api.Infrastructure.Database;
using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Modules.Admin.Features.PropertiesTypes;

public static class UpdatePropertyType
{
    public sealed record Request(string Name, string Description);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        }
    }

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut($"{AdminPaths.PropertiesTypes}/{{id:guid}}", HandlerAsync)
                .WithName(nameof(UpdatePropertyType))
                .WithTags(nameof(PropertyType))
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest);
        }

        private static async Task<IResult> HandlerAsync(
            [FromRoute] Guid id,
            [FromBody] Request request,
            [FromServices] IValidator<Request> validator,
            [FromServices] ImobSysDbContext db,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var entity = await db.PropertiesTypes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            entity.Update(
                request.Name,
                request.Description);

            await db.SaveChangesAsync(cancellationToken);

            return TypedResults.NoContent();
        }
    }
}
