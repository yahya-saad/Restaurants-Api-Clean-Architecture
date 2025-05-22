using AutoMapper;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Tests;
public class RestaurantProfileTests
{
    [Fact]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantsProfile>());
        var mapper = config.CreateMapper();
        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Fast Food",
            HasDelivery = true,
            ContractEmail = null,
            ContractNumber = null,
            Address = new Address
            {
                Street = "123 Test St",
                City = "Test City",
                PostalCode = "12345"
            },
            Dishes = new List<Dish>()
        };

        var restaurantDto = new RestaurantDto
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Fast Food",
            HasDelivery = true,
            Street = "123 Test St",
            City = "Test City",
            PostalCode = "12345"
        };
        // Act
        var result = mapper.Map<RestaurantDto>(restaurant);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(restaurantDto);
    }

    [Fact]
    public void CreateMap_ForCreateReastaurantDtoToRestaurantDto_MapsCorrectly()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantsProfile>());
        var mapper = config.CreateMapper();
        var createRestaurantDto = new CreateRestaurantDto
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Fast Food",
            HasDelivery = true,
            ContractEmail = null,
            ContractNumber = null,
            Address = new Address
            {
                Street = "123 Test St",
                City = "Test City",
                PostalCode = "12345"
            }
        };

        var restaurant = new Restaurant
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Fast Food",
            HasDelivery = true,
            ContractEmail = null,
            ContractNumber = null,
            Address = new Address
            {
                Street = "123 Test St",
                City = "Test City",
                PostalCode = "12345"
            },
            Dishes = new List<Dish>()
        };

        var result = mapper.Map<Restaurant>(createRestaurantDto);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(restaurant);

    }

    [Fact]
    public void CreateMap_ForPatchRestaurantDtoToRestaurant_MapsCorrectly()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantsProfile>());
        var mapper = config.CreateMapper();
        var patchRestaurantDto = new PatchRestaurantDto
        {
            Name = "Updated Restaurant",
            Description = "Updated Description",
            Category = "Updated Category",
            HasDelivery = false,
        };
        var restaurant = new Restaurant
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Fast Food",
            HasDelivery = true,
        };
        var result = mapper.Map<Restaurant>(restaurant);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(restaurant);
    }

    [Fact]
    public void CreateMap_ForUpdateRestaurantDtoToRestaurant_MapsCorrectly()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantsProfile>());
        var mapper = config.CreateMapper();
        var updateRestaurantDto = new UpdateRestaurantDto
        {
            Name = "Updated Restaurant",
            Description = "Updated Description",
            Category = "Updated Category",
            HasDelivery = false,
        };
        var restaurant = new Restaurant
        {
            Name = "Updated Restaurant",
            Description = "Updated Description",
            Category = "Updated Category",
            HasDelivery = false,
        };
        var result = mapper.Map<Restaurant>(updateRestaurantDto);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(restaurant);
    }

}
