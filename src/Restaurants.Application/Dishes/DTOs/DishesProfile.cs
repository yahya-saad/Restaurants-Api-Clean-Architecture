using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.DTOs;
internal class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<Dish, DishDto>();
    }
}
