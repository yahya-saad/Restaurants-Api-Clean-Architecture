using Restaurants.Infrastructure;
using Restaurants.Infrastructure.Seeders;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

#region SeedData
using var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<AppSeeder>();
await seeder.SeedAsync();
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
