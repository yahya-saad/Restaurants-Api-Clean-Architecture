using Restaurants.API.Extensions;
using Restaurants.API.Middlewares;
using Restaurants.Application;
using Restaurants.Infrastructure;
using Restaurants.Infrastructure.Persistence;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Restaurants API V1");
    });
    app.MapScalarApiReference();
}

if (builder.Configuration.GetValue<bool>("RunMigrations"))
{
    await app.ApplyMigrationsAsync<RestaurantDbContext>();
    await app.SeedDataAsync();
}

app.UseExceptionHandler();


app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
