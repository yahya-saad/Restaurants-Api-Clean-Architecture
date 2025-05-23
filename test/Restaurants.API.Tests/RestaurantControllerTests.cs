using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace Restaurants.API.Tests;

public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
    private readonly Mock<ISeeder> seederMock = new();
    public RestaurantControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(cfg =>
        {
            cfg.ConfigureTestServices(services =>
            {
                // Remove all existing registrations for IRestaurantsRepository to avoid accidental fallback to the real implementation
                services.RemoveAll(typeof(IRestaurantsRepository));
                services.AddScoped<IRestaurantsRepository>(_ => _restaurantsRepositoryMock.Object);
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                services.RemoveAll(typeof(ISeeder));
                services.AddScoped<ISeeder>(_ => seederMock.Object);
            });
        });
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetRestaurants_ForValidRequest_Returns200Ok()
    {

        var response = await _client.GetAsync("/api/Restaurants?PageNumber=1&PageSize=5");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetRestaurants_ForInvalidRequest_Returns400BadRequest()
    {
        var response = await _client.GetAsync("/api/Restaurants");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetRestaurantById_ForExistingId_Returns200Ok()
    {
        var id = 1;
        var restaurant = new Restaurant { Id = id, Name = "Test", Description = "Test", Category = "Test" };
        _restaurantsRepositoryMock
            .Setup(repo => repo.GetByIdAsync(id, It.IsAny<CancellationToken>(), It.IsAny<string?>()))
            .ReturnsAsync(restaurant);

        var response = await _client.GetAsync($"/api/Restaurants/{id}");

        var content = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNull();
        content!.Id.Should().Be(id);
        content.Name.Should().Be(restaurant.Name);
    }

    [Fact]
    public async Task GetRestaurantById_ForNonExistingId_Returns400NotFound()
    {
        var id = 1;
        _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(id, It.IsAny<CancellationToken>(), null))
    .ReturnsAsync((Restaurant?)null);
        var response = await _client.GetAsync($"/api/Restaurants/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateRestaurant_ForValidRequest_Returns201Created()
    {
        var createDto = new CreateRestaurantDto
        {
            Name = "New Restaurant",
            Description = "A new place",
            Category = "Italian",
            HasDelivery = true,
            Address = new Address
            {
                Street = "123 Main St",
                City = "Anytown",
                PostalCode = "12345"
            }
        };

        _restaurantsRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Restaurant>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<int>());

        var response = await _client.PostAsJsonAsync("/api/Restaurants", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
