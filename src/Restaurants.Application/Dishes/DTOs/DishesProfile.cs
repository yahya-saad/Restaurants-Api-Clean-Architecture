namespace Restaurants.Application.Dishes.DTOs;
internal class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<CreateDishDto, Dish>()
            .ForMember(dest => dest.RestaurantId, opt => opt.Ignore());

        CreateMap<UpdateDishDto, Dish>();
    }
}
