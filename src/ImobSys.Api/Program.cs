using FluentValidation;
using FluentValidation.AspNetCore;
using ImobSys.Api.Common.Endpoints;
using ImobSys.Api.Infrastructure;
using ImobSys.Api.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services
    .AddEndpoints()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations(dropDatabase: true);
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();
