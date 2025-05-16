namespace Restaurants.Application.Restaurants.DTOs;
public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City == null ? null : src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street == null ? null : src.Address.Street))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode == null ? null : src.Address.PostalCode))
            .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.Dishes));

        CreateMap<CreateRestaurantDto, Restaurant>();

        CreateMap<PatchRestaurantDto, Restaurant>().ReverseMap();
        CreateMap<UpdateRestaurantDto, Restaurant>().ReverseMap();

    }
}
