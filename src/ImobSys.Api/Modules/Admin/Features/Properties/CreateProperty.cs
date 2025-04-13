using ImobSys.Api.Common.Endpoints;
using ImobSys.Api.Infrastructure.Database;
using FluentValidation;
using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ImobSys.Api.Modules.Admin.Features.Properties;

public static class CreateProperty
{
    public sealed record Request(string Name, string Description, Guid TypeId);
    public sealed record Response(Guid Id, string Name, string Description, Guid TypeId);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.TypeId).NotEmpty();
        }
    }

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(AdminPaths.Properties, HandlerAsync)
                .WithName(nameof(CreateProperty))
                .WithTags(nameof(Property))
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

            var entity = new Property(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.TypeId);

            db.Properties.Add(entity);
            await db.SaveChangesAsync(cancellationToken);

            return TypedResults.Created(
                $"{AdminPaths.Properties}/{entity.Id}",
                new Response(Guid.NewGuid(), request.Name, request.Description, request.TypeId));
        }
    }
}