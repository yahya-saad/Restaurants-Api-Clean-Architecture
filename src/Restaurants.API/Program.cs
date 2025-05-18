using Restaurants.API.Extensions;
using Restaurants.Application;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure;
using Restaurants.Infrastructure.Persistence;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapScalarApiReference(options =>
    {
        options.Title = "Restaurants API";
        options.OpenApiRoutePattern = "/swagger/v1/swagger.json";
    });
}

if (builder.Configuration.GetValue<bool>("RunMigrations"))
{
    await app.ApplyMigrationsAsync<RestaurantDbContext>();
    await app.SeedDataAsync();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapGroup("/api/Identity").MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();
