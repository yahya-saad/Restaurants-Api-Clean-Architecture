using Restaurants.API.Extensions;
using Restaurants.API.Middlewares;
using Restaurants.Application;
using Restaurants.Infrastructure;
using Restaurants.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<RequestLoggingFliter>();
}).AddNewtonsoftJson();

builder.Services.AddOpenApi();
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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
