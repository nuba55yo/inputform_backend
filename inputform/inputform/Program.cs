using System;
using System.Reflection;
using inputform.Persistence;
using inputform.Service.Inputform;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

static Assembly[] GetAppAssemblies()
{
    var prefixes = new[] { "inputform" };
    return AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => !a.IsDynamic)
        .Where(a =>
        {
            var n = a.GetName().Name ?? "";
            return prefixes.Any(p => n.StartsWith(p, StringComparison.OrdinalIgnoreCase));
        })
        .Distinct()
        .ToArray();
}

var assemblies = GetAppAssemblies();

builder.Services.Scan(scan => scan
    .FromAssemblies(assemblies)
    .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository", StringComparison.Ordinal)))
    .AsSelf()
    .WithScopedLifetime());

builder.Services.AddMediatR(assemblies);

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.MapGet("/api/inputform/occupations", async (IMediator mediator, CancellationToken ct) =>
{
    var res = await mediator.Send(new GetOccupationsQuery(), ct);
    return Results.Ok(res);
});

app.MapPost("/api/inputform", async ([FromForm] InputformRequest request, IMediator mediator, CancellationToken ct) =>
{
    var result = await mediator.Send(new CreateInputformCommand(request), ct);

    return result.succeeded
        ? Results.Ok(result.data)
        : Results.BadRequest(new { message = result.error });
}).DisableAntiforgery();

app.Run();
