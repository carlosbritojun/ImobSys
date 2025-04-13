using FluentValidation;
using ImobSys.Api.Common.Endpoints;
using ImobSys.Api.Infrastructure.Database;
using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ImobSys.Api.Modules.Admin.Features.PropertiesTypes;

public static class CreatePropertyType
{
    public sealed record Request(string Name, string Description);
    public sealed record Response(Guid Id, string Name, string Description);
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
            app.MapPost(AdminPaths.PropertiesTypes, HandlerAsync)
                .WithName(nameof(CreatePropertyType))
                .WithTags(nameof(PropertyType))
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
        }

        private static async Task<IResult> HandlerAsync(
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

            var entity = new PropertyType(
                Guid.NewGuid(),
                request.Name,
                request.Description);

            db.PropertiesTypes.Add(entity);
            await db.SaveChangesAsync(cancellationToken);

            return TypedResults.Created(
                $"{AdminPaths.Properties}/{entity.Id}",
                new Response(Guid.NewGuid(), request.Name, request.Description));
        }
    }
}